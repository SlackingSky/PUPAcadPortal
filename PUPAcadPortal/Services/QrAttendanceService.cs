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

        // Public entry point

        public QrScanOutcome ProcessScan(string rawQrText, int studentId)
        {
            var scanTime = DateTime.UtcNow;

            //Cryptographic token validation (pure in-memory) 
            var validation = QrTokenService.Validate(rawQrText);

            if (!validation.IsValid)
            {
                WriteLog(null, studentId, ExtractNonce(rawQrText),
                    scanTime, validation.Result.ToString(), null,
                    $"Token validation failed: {validation.Message}");

                return QrScanOutcome.Fail(FriendlyError(validation.Result));
            }

            var payload = validation.Payload!;

            // ClassSession existence check 
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

            //  SubjectOfferingId tamper check 
            if (!string.Equals(session.SubjectOfferingId, payload.Oid,
                    StringComparison.OrdinalIgnoreCase))
            {
                WriteLog(payload.Sid, studentId, payload.Nonce,
                    scanTime, QrValidationResult.InvalidSignature.ToString(), null,
                    $"OfferingId mismatch: token={payload.Oid} session={session.SubjectOfferingId}");

                return QrScanOutcome.Fail(
                    "QR code does not match this session. Please scan the correct code.");
            }

            // Active QrSession existence + expiry check 
            var now = DateTime.UtcNow;
            var activeQrSession = _ctx.QrSessions
                .Where(q => q.SessionId == payload.Sid
                         && q.IsActive == true
                         && q.ExpiresAt > now)
                .OrderByDescending(q => q.GeneratedAt)
                .FirstOrDefault();

            if (activeQrSession == null)
            {
                // Auto-expire any stale rows while we are here
                var stale = _ctx.QrSessions
                    .Where(q => q.SessionId == payload.Sid
                             && q.IsActive == true
                             && q.ExpiresAt <= now)
                    .ToList();
                foreach (var s in stale) s.IsActive = false;
                if (stale.Count > 0)
                {
                    try { _ctx.SaveChanges(); } catch { /* best-effort */ }
                }

                WriteLog(payload.Sid, studentId, payload.Nonce,
                    scanTime, "NoActiveQrSession", null,
                    "No active QrSession found for this ClassSession — may have been deactivated.");

                return QrScanOutcome.Fail(
                    "This QR code is no longer active. Ask your instructor to generate a new one.");
            }

            // Registered-student check 
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

            // Enrollment check 
            bool isEnrolled = _ctx.EnrollmentSubjects
                .Include(es => es.Enrollment)
                .Any(es => es.SubjectOfferingId == payload.Oid
                        && es.Enrollment.StudentId == studentId);

            if (!isEnrolled)
            {
                WriteLog(payload.Sid, studentId, payload.Nonce,
                    scanTime, "NotEnrolled", null,
                    $"Student {studentId} is not enrolled in offering {payload.Oid}.");

                return QrScanOutcome.Fail(
                    "You are not enrolled in this subject. "
                    + "Please verify your enrollment or contact the registrar.");
            }

            // Duplicate nonce check (replay prevention) 
            bool nonceUsed = _ctx.AttendanceRecords
                .Any(ar => ar.QrNonce == payload.Nonce);

            if (nonceUsed)
            {
                WriteLog(payload.Sid, studentId, payload.Nonce,
                    scanTime, "DuplicateNonce", null,
                    "Nonce already consumed — replay attempt.");

                return QrScanOutcome.Fail("This QR code has already been used.");
            }

            // Duplicate session check (one record per student per session)
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