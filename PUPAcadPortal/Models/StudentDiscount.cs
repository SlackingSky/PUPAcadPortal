using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class StudentDiscount
{
    public int DiscountId { get; set; }

    public int StudentId { get; set; }

    public string DiscountName { get; set; } = null!;

    public decimal Percentage { get; set; }

    public bool? IsActive { get; set; }

    public virtual Student Student { get; set; } = null!;
}
