namespace PUPAcadPortal.Services
{

    public sealed class QrScanOutcome
    {
        /// <summary>True when attendance was successfully persisted.</summary>
        public bool Success { get; init; }

        /// <summary>
        /// Human-readable result message.
        /// On success: "Attendance Successfully Recorded".
        /// On failure: the friendly error from QrAttendanceService.
        /// </summary>
        public string Message { get; init; } = string.Empty;

        // ── Fields populated on success only ─────────────────────────────────────

        /// <summary>Full name of the student whose attendance was recorded.</summary>
        public string StudentName { get; init; } = string.Empty;

        /// <summary>SubjectCode of the session's offering (e.g. "COMP 012").</summary>
        public string CourseCode { get; init; } = string.Empty;

        /// <summary>Full subject name (e.g. "Network Administration").</summary>
        public string SubjectName { get; init; } = string.Empty;

        /// <summary>Section identifier (e.g. "BSIT 2-1").</summary>
        public string Section { get; init; } = string.Empty;

        /// <summary>Formatted session date (e.g. "June 06, 2026 (Saturday)").</summary>
        public string SessionDate { get; init; } = string.Empty;

        /// <summary>Local time of the scan formatted as "hh:mm tt".</summary>
        public string TimeIn { get; init; } = string.Empty;

        /// <summary>AttendanceRecord.AttendanceId of the newly created row.</summary>
        public int AttendanceId { get; init; }

        /// <summary>ClassSession.SessionId that was matched.</summary>
        public int SessionId { get; init; }

        // ── Factory helpers ───────────────────────────────────────────────────────

        public static QrScanOutcome Fail(string msg)
            => new() { Success = false, Message = msg };
    }
}