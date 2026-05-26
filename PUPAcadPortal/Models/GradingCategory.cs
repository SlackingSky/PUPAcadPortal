using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class GradingCategory
{
    public int CategoryId { get; set; }

    public string SubjectOfferingId { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public decimal WeightPercentage { get; set; }

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

    public virtual SubjectOffering SubjectOffering { get; set; } = null!;
}
