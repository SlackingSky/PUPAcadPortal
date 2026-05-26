using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class ActivityLog
{
    public int LogId { get; set; }

    public int UserId { get; set; }

    public string Action { get; set; } = null!;

    public string Module { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public virtual User User { get; set; } = null!;
}
