using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class ClassSession
{
    public int SessionId { get; set; }

    public string SubjectOfferingId { get; set; } = null!;

    public DateTime SessionDate { get; set; }

    public TimeSpan? StartTime { get; set; }

    public TimeSpan? EndTime { get; set; }

    public string? Topic { get; set; }

    public virtual ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();

    public virtual ICollection<QrSession> QrSessions { get; set; } = new List<QrSession>();

    public virtual SubjectOffering SubjectOffering { get; set; } = null!;
}
