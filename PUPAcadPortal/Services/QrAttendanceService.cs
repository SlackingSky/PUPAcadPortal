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

        // ── Main entry point ──────────────────────────────────────────────────────
        /// <summary>
        /// Processes a raw QR scan for the given student.
        /// Always writes a QrScanLog row (audit trail).
        /// Returns a QrScanOutcome with Success = true on happy path.
        /// </summary>
        public QrScanOutcome ProcessScan(string rawQrText, int studentId)
        {
            var scanTime = DateTime.UtcNow;

            // ── Step 1: Token validation (pure in-memory, no DB) ─────────────────
            var validation = QrTokenService.Validate(rawQrText);

            if (!validation.IsValid)
            {
                WriteLog(null, studentId, ExtractNonce(rawQrText),
                    scanTime, validation.Result.ToString(), null,
                    $"Token validation failed: {validation.Message}");

                return QrScanOutcome.Fail(FriendlyError(validation.Result));
            }

            var payload = validation.Payload!;

            // ── Step 2: Verify ClassSession exists ────────────────────────────────
            var session = _ctx.ClassSessions
                .Include(cs => cs.SubjectOffering)
                    .ThenInclude(so => so.Subject)
                .FirstOrDefault(cs => cs.SessionId == payload.Sid);

            if (session == null)
            {
                WriteLog(payload.Sid, studentId, payload.Nonce,
                    scanTime, QrValidationResult.SessionNotFound.ToString(), null,
                    "ClassSession row not found.");

                return QrScanOutcome.Fail("Session not found. The QR code may belong to a different system.");
            }

            // Confirm offering ID matches (extra tamper check)
            if (!string.Equals(session.SubjectOfferingId, payload.Oid,
                    StringComparison.OrdinalIgnoreCase))
            {
                WriteLog(payload.Sid, studentId, payload.Nonce,
                    scanTime, QrValidationResult.InvalidSignature.ToString(), null,
                    "SubjectOfferingId mismatch.");

                return QrScanOutcome.Fail("QR code does not match this session.");
            }

            // ── Step 3: Student lookup ────────────────────────────────────────────
            var student = _ctx.Students
                .Include(s => s.User)
                .FirstOrDefault(s => s.StudentId == studentId);

            if (student == null)
            {
                WriteLog(payload.Sid, studentId, payload.Nonce,
                    scanTime, "StudentNotFound", null, "Student row not found.");

                return QrScanOutcome.Fail("Student record not found. Please contact your registrar.");
            }

            // ── Step 4: Duplicate nonce check (same token scanned twice) ──────────
            bool nonceExists = _ctx.AttendanceRecords
                .Any(ar => ar.QrNonce == payload.Nonce);

            if (nonceExists)
            {
                WriteLog(payload.Sid, studentId, payload.Nonce,
                    scanTime, "DuplicateNonce", null,
                    "Nonce already consumed — replay attempt.");

                return QrScanOutcome.Fail("This QR code has already been used.");
            }

            // ── Step 5: Duplicate session check (student already recorded) ────────
            bool sessionRecordExists = _ctx.AttendanceRecords
                .Any(ar => ar.SessionId == payload.Sid && ar.StudentId == studentId);

            if (sessionRecordExists)
            {
                WriteLog(payload.Sid, studentId, payload.Nonce,
                    scanTime, QrValidationResult.AlreadyRecorded.ToString(), null,
                    "Attendance already recorded for this session.");

                return QrScanOutcome.Fail("Your attendance for this session has already been recorded.");
            }

            // ── Step 6: Insert AttendanceRecord ───────────────────────────────────
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
                // Could be a UNIQUE constraint violation from a concurrent scan —
                // treat as duplicate.
                WriteLog(payload.Sid, studentId, payload.Nonce,
                    scanTime, "DbInsertFailed", null,
                    $"SaveChanges failed: {ex.Message}");

                return QrScanOutcome.Fail(
                    "Attendance could not be saved. It may have already been recorded. " +
                    "Please check your attendance history.");
            }

            // ── Step 7: Audit log (success) ───────────────────────────────────────
            WriteLog(payload.Sid, studentId, payload.Nonce,
                scanTime, QrValidationResult.Valid.ToString(), record.AttendanceId,
                "Attendance recorded successfully.");

            // ── Step 8: Build rich outcome for UI display ─────────────────────────
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

        // ── Audit log helper ──────────────────────────────────────────────────────
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

        // ── Extract nonce from raw text even if token is invalid ──────────────────
        private static string ExtractNonce(string raw)
        {
            try
            {
                var parts = raw?.Split('.');
                if (parts?.Length == 2)
                {
                    byte[] json = System.Convert.FromBase64String(
                        parts[0].Replace('-', '+').Replace('_', '/').PadRight(
                            parts[0].Length + (4 - parts[0].Length % 4) % 4, '='));
                    var p = System.Text.Json.JsonSerializer
                                .Deserialize<QrTokenPayload>(json);
                    return p?.Nonce ?? "(none)";
                }
            }
            catch { /* ignore */ }
            return "(none)";
        }

        // ── User-friendly error messages ──────────────────────────────────────────
        private static string FriendlyError(QrValidationResult r) => r switch
        {
            QrValidationResult.Expired
                => "This QR code has expired. Ask your instructor to generate a new one.",
            QrValidationResult.OutsideAttendanceWindow
                => "Attendance scanning is not open yet. Please wait for your class to start.",
            QrValidationResult.SessionDateMismatch
                => "This QR code is not for today's session.",
            QrValidationResult.InvalidSignature
                => "Invalid QR code. Please make sure you are scanning the official attendance code.",
            QrValidationResult.AlreadyRecorded
                => "Your attendance for this session has already been recorded.",
            QrValidationResult.MalformedToken
                => "This QR code does not appear to be an attendance code.",
            _ => "QR code could not be validated. Please try again.",
        };
    }
}