using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class AttendanceRecord
{
    public int AttendanceId { get; set; }

    public int SessionId { get; set; }

    public int StudentId { get; set; }

    public string Status { get; set; } = null!;

    public string? Remarks { get; set; }

    public virtual ClassSession Session { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
