using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class ProfessorAvailability
{
    public int AvailabilityId { get; set; }

    public int ProfessorId { get; set; }

    public string DayOfWeek { get; set; } = null!;

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public virtual Professor Professor { get; set; } = null!;
}
