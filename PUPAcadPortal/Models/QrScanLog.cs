using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

/// <summary>
/// Audit trail for every QR scan attempt (success and failure).
/// </summary>
public partial class QrScanLog
{
    public int LogId { get; set; }

    /// <summary>
    /// FK → ClassSession.SessionID; NULL for pre-session failures
    /// </summary>
    public int? SessionId { get; set; }

    /// <summary>
    /// FK → Student.StudentID
    /// </summary>
    public int StudentId { get; set; }

    public string QrNonce { get; set; } = null!;

    public DateTime AttemptedAt { get; set; }

    /// <summary>
    /// e.g. Valid, Expired, AlreadyRecorded, NotEnrolled …
    /// </summary>
    public string ValidationResult { get; set; } = null!;

    /// <summary>
    /// FK → AttendanceRecord.AttendanceID; NULL on failure
    /// </summary>
    public int? AttendanceId { get; set; }

    public string? Notes { get; set; }

    public virtual AttendanceRecord? Attendance { get; set; }

    public virtual ClassSession? Session { get; set; }

    public virtual Student Student { get; set; } = null!;
}
