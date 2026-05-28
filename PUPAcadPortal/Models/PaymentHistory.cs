using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class PaymentHistory
{
    public int TransactionId { get; set; }

    public int AccountId { get; set; }

    public string? ReferenceId { get; set; }

    public string Description { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? PaidDate { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual StudentAccount Account { get; set; } = null!;
}
