using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class QrScanLog
{
    public int LogId { get; set; }

    public int SessionId { get; set; }

    public int StudentId { get; set; }

    public string QrNonce { get; set; } = null!;

    public DateTime AttemptedAt { get; set; }

    public string ValidationResult { get; set; } = null!;

    public int? AttendanceId { get; set; }

    public string? Notes { get; set; }

    public virtual AttendanceRecord? Attendance { get; set; }

    public virtual ClassSession Session { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
