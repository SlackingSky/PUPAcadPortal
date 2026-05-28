using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class StudentAccount
{
    public int AccountId { get; set; }

    public int StudentId { get; set; }

    public string AcademicPeriodId { get; set; } = null!;

    public decimal TotalAssessment { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual AcademicPeriod AcademicPeriod { get; set; } = null!;

    public virtual ICollection<PaymentHistory> PaymentHistories { get; set; } = new List<PaymentHistory>();

    public virtual Student Student { get; set; } = null!;
}
