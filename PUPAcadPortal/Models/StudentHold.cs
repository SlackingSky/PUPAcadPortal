using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class StudentHold
{
    public int HoldId { get; set; }

    public int StudentId { get; set; }

    public string HoldType { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int PlacedByUserId { get; set; }

    public bool IsResolved { get; set; }

    public DateTime? ResolvedDate { get; set; }

    public virtual User PlacedByUser { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
