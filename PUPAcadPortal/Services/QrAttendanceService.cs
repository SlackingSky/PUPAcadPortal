using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Models;

namespace PUPAcadPortal.Services
{
    public sealed class QrAttendanceService
    {
        private readonly AppDbContext _ctx;

        public QrAttendanceService(AppDbContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }


        public QrScanOutcome ProcessScan(string rawQrText, int studentId)
        {
            var scanTime = DateTime.UtcNow;

            //  Reject anything that isn't a valid attendance QR format 
            // Try to decode the base64 header portion of the token. If it fails to
            // parse as a QrTokenPayload (missing Sid / Oid fields), it is not an
            // attendance QR code at all — reject immediately with a friendly message.
            // NOTE: signature verification and expiry/window checks are still
            //       commented out below for testing purposes.
            QrTokenPayload? payload;
            try
            {
                var parts = rawQrText?.Split('.');
                if (parts == null || parts.Length < 1)
                    throw new FormatException("No token segments found.");

                string b64 = parts[0].Replace('-', '+').Replace('_', '/');
                int pad = (4 - b64.Length % 4) % 4;
                b64 += new string('=', pad);
                byte[] json = Convert.FromBase64String(b64);
                payload = System.Text.Json.JsonSerializer.Deserialize<QrTokenPayload>(json);
            }
            catch
            {
                // Could not decode — definitely not one of our QR codes.
                WriteLog(null, studentId, ExtractNonce(rawQrText),
                    scanTime, QrValidationResult.MalformedToken.ToString(), null,
                    "Token could not be base64-decoded or deserialized.");

                return QrScanOutcome.Fail(
                    "This does not appear to be a valid attendance QR code.");
            }

            // Payload decoded but missing required fields → not our QR code.
            if (payload == null || payload.Sid <= 0 || string.IsNullOrWhiteSpace(payload.Oid))
            {
                WriteLog(null, studentId, ExtractNonce(rawQrText),
                    scanTime, QrValidationResult.MalformedToken.ToString(), null,
                    "Payload missing required Sid or Oid fields.");

                return QrScanOutcome.Fail(
                    "This does not appear to be a valid attendance QR code.");
            }
            //  End invalid-QR gate 

            //  Signature / expiry / time-window checks — still bypassed for testing
            // var validation = QrTokenService.Validate(rawQrText);
            //
            // bool isExpiredOrWindowFailure =
            //     validation.Result == QrValidationResult.Expired ||
            //     validation.Result == QrValidationResult.OutsideAttendanceWindow ||
            //     validation.Result == QrValidationResult.SessionDateMismatch;
            //
            // if (!validation.IsValid && !isExpiredOrWindowFailure)
            // {
            //     WriteLog(null, studentId, ExtractNonce(rawQrText),
            //         scanTime, validation.Result.ToString(), null,
            //         $"Token validation failed: {validation.Message}");
            //
            //     return QrScanOutcome.Fail(FriendlyError(validation.Result));
            // }
            var session = _ctx.ClassSessions
                .Include(cs => cs.SubjectOffering)
                    .ThenInclude(so => so.Subject)
                .FirstOrDefault(cs => cs.SessionId == payload.Sid);

            if (session == null)
            {
                WriteLog(payload.Sid, studentId, payload.Nonce,
                    scanTime, QrValidationResult.SessionNotFound.ToString(), null,
                    "ClassSession row not found.");

                return QrScanOutcome.Fail(
                    "Session not found. This QR code may belong to a different system.");
            }

            if (!string.Equals(session.SubjectOfferingId, payload.Oid,
                    StringComparison.OrdinalIgnoreCase))
            {
                WriteLog(payload.Sid, studentId, payload.Nonce,
                    scanTime, QrValidationResult.InvalidSignature.ToString(), null,
                    $"OfferingId mismatch: token={payload.Oid} session={session.SubjectOfferingId}");

                return QrScanOutcome.Fail(
                    "QR code does not match this session. Please scan the correct code.");
            }

            var now = DateTime.UtcNow;

            //  DEBUG: expiry gate DISABLED — bypass active/expired QrSession check 
            // var activeQrSession = _ctx.QrSessions
            //     .Where(q => q.SessionId == payload.Sid
            //              && q.IsActive == true
            //              && q.ExpiresAt > now)
            //     .OrderByDescending(q => q.GeneratedAt)
            //     .FirstOrDefault();

            // if (activeQrSession == null)
            // {
            //     var stale = _ctx.QrSessions
            //         .Where(q => q.SessionId == payload.Sid
            //                  && q.IsActive == true
            //                  && q.ExpiresAt <= now)
            //         .ToList();
            //     foreach (var s in stale) s.IsActive = false;
            //     if (stale.Count > 0)
            //     {
            //         try { _ctx.SaveChanges(); } catch { }
            //     }

            //     WriteLog(payload.Sid, studentId, payload.Nonce,
            //         scanTime, "NoActiveQrSession", null,
            //         "No active QrSession found for this ClassSession — may have been deactivated.");

            //     return QrScanOutcome.Fail(
            //         "This QR code is no longer active. Ask your instructor to generate a new one.");
            // }
            //  END DEBUG 

            var student = _ctx.Students
                .Include(s => s.User)
                .FirstOrDefault(s => s.StudentId == studentId);

            if (student == null)
            {
                WriteLog(payload.Sid, studentId, payload.Nonce,
                    scanTime, "StudentNotFound", null,
                    $"StudentId={studentId} not found in Students table.");

                return QrScanOutcome.Fail(
                    "Your student record was not found. Please contact the registrar.");
            }

            // Do NOT use .Include() inside .Any() — EF Core cannot translate
            // navigation property access inside a predicate when Include is present.
            // Use a plain join via a subquery instead.
            bool isEnrolled = _ctx.EnrollmentSubjects
                .Any(es => es.SubjectOfferingId == payload.Oid
                        && _ctx.Enrollments.Any(en =>
                               en.EnrollmentId == es.EnrollmentId
                            && en.StudentId == studentId));

            if (!isEnrolled)
            {
                WriteLog(payload.Sid, studentId, payload.Nonce,
                    scanTime, "NotEnrolled", null,
                    $"Student {studentId} is not enrolled in offering {payload.Oid}.");

                return QrScanOutcome.Fail(
                    "You are not enrolled in this subject. "
                    + "Please verify your enrollment or contact the registrar.");
            }

            // Guard: if nonce is null/empty (edge case from bypassed validation),
            // skip the nonce-uniqueness check rather than sending null to the DB.
            bool nonceUsed = !string.IsNullOrEmpty(payload.Nonce)
                && _ctx.AttendanceRecords.Any(ar => ar.QrNonce == payload.Nonce);

            if (nonceUsed)
            {
                WriteLog(payload.Sid, studentId, payload.Nonce,
                    scanTime, "DuplicateNonce", null,
                    "Nonce already consumed — replay attempt.");

                return QrScanOutcome.Fail("This QR code has already been used.");
            }

            bool alreadyRecorded = _ctx.AttendanceRecords
                .Any(ar => ar.SessionId == payload.Sid && ar.StudentId == studentId);

            if (alreadyRecorded)
            {
                WriteLog(payload.Sid, studentId, payload.Nonce,
                    scanTime, QrValidationResult.AlreadyRecorded.ToString(), null,
                    "Attendance already recorded for this session.");

                return QrScanOutcome.Fail(
                    "Your attendance for this session has already been recorded.");
            }

            //Insert AttendanceRecord 
            var record = new AttendanceRecord
            {
                SessionId = payload.Sid,
                StudentId = studentId,
                Status = "Present",
                Remarks = "Recorded via QR Code",
                IsQrVerified = true,
                QrScannedAt = scanTime,
                QrNonce = payload.Nonce,
            };

            _ctx.AttendanceRecords.Add(record);

            try
            {
                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                // UNIQUE constraint violation from a concurrent scan — treat as duplicate
                WriteLog(payload.Sid, studentId, payload.Nonce,
                    scanTime, "DbInsertFailed", null,
                    $"SaveChanges failed: {ex.Message}");

                return QrScanOutcome.Fail(
                    "Attendance could not be saved — it may have already been recorded. "
                    + "Please check your attendance history.");
            }

            //Audit log (success) 
            WriteLog(payload.Sid, studentId, payload.Nonce,
                scanTime, QrValidationResult.Valid.ToString(), record.AttendanceId,
                "Attendance recorded successfully.");

            // Build rich outcome 
            string fullName = student.User != null
                ? $"{student.User.FirstName} {student.User.LastName}"
                : $"Student #{studentId}";

            string subjectName = session.SubjectOffering?.Subject?.SubjectName
                                  ?? session.SubjectOfferingId;
            string subjectCode = session.SubjectOffering?.Subject?.SubjectCode
                                  ?? string.Empty;
            string section = session.SubjectOffering?.Section ?? string.Empty;
            string sessionDate = session.SessionDate.ToString("MMMM dd, yyyy (dddd)");
            string timeIn = DateTime.Now.ToString("hh:mm tt");

            return new QrScanOutcome
            {
                Success = true,
                Message = "Attendance Successfully Recorded",
                StudentName = fullName,
                CourseCode = subjectCode,
                SubjectName = subjectName,
                Section = section,
                SessionDate = sessionDate,
                TimeIn = timeIn,
                AttendanceId = record.AttendanceId,
                SessionId = payload.Sid,
            };
        }
        private void WriteLog(
            int? sessionId,
            int studentId,
            string nonce,
            DateTime attemptedAt,
            string result,
            int? attendanceId,
            string? notes)
        {
            try
            {
                _ctx.QrScanLogs.Add(new QrScanLog
                {
                    SessionId = sessionId ?? 0,
                    StudentId = studentId,
                    QrNonce = nonce ?? "(none)",
                    AttemptedAt = attemptedAt,
                    ValidationResult = result,
                    AttendanceId = attendanceId,
                    Notes = notes,
                });
                _ctx.SaveChanges();
            }
            catch
            {
                // Never let audit logging crash the UI.
            }
        }

        private static string ExtractNonce(string raw)
        {
            try
            {
                var parts = raw?.Split('.');
                if (parts?.Length == 2)
                {
                    string b64 = parts[0]
                        .Replace('-', '+')
                        .Replace('_', '/');
                    int pad = (4 - b64.Length % 4) % 4;
                    b64 += new string('=', pad);

                    byte[] json = Convert.FromBase64String(b64);
                    var p = System.Text.Json.JsonSerializer
                        .Deserialize<QrTokenPayload>(json);
                    return p?.Nonce ?? "(none)";
                }
            }
            catch { /* ignore */ }
            return "(none)";
        }

        private static string FriendlyError(QrValidationResult r) => r switch
        {
            QrValidationResult.Expired
                => "This QR code has expired. Ask your instructor to generate a new one.",
            QrValidationResult.OutsideAttendanceWindow
                => "Attendance scanning is not open yet. Please wait for your class to start.",
            QrValidationResult.SessionDateMismatch
                => "This QR code is not for today's session.",
            QrValidationResult.InvalidSignature
                => "Invalid QR code. Ensure you are scanning the official attendance code.",
            QrValidationResult.AlreadyRecorded
                => "Your attendance for this session has already been recorded.",
            QrValidationResult.MalformedToken
                => "This does not appear to be a valid attendance QR code.",
            QrValidationResult.SessionNotFound
                => "Session not found. The QR code may belong to a different system.",
            _ => "QR code could not be validated. Please try again.",
        };
    }
}