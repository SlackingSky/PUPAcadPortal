using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class RoomSchedule
{
    public int ScheduleId { get; set; }

    public string SubjectOfferingId { get; set; } = null!;

    public string SessionType { get; set; } = null!;

    public string DayOfWeek { get; set; } = null!;

    public int? RoomId { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public virtual Room? Room { get; set; }

    public virtual SubjectOffering SubjectOffering { get; set; } = null!;
}
