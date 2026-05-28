using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class RoomSchedule
{
    public int ScheduleId { get; set; }

    public string SubjectOfferingId { get; set; } = null!;

    public string DayOfWeek { get; set; } = null!;

    public string RoomName { get; set; } = null!;

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public virtual SubjectOffering SubjectOffering { get; set; } = null!;
}
