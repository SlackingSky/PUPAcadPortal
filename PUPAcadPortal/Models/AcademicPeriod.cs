using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class AcademicPeriod
{
    public string AcademicPeriodId { get; set; } = null!;

    public string SchoolYear { get; set; } = null!;

    public string Semester { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual ICollection<StudentAccount> StudentAccounts { get; set; } = new List<StudentAccount>();

    public virtual ICollection<SubjectOffering> SubjectOfferings { get; set; } = new List<SubjectOffering>();
}
