using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class FeeBreakdown
{
    public int FeeId { get; set; }

    public int AccountId { get; set; }

    public string FeeName { get; set; } = null!;

    public decimal Amount { get; set; }

    public virtual StudentAccount Account { get; set; } = null!;
}
