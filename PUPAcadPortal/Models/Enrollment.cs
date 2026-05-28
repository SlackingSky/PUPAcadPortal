using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class Enrollment
{
    public string EnrollmentId { get; set; } = null!;

    public int StudentId { get; set; }

    public string AcademicPeriodId { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime EnrollmentDate { get; set; }

    public virtual AcademicPeriod AcademicPeriod { get; set; } = null!;

    public virtual ICollection<EnrollmentSubject> EnrollmentSubjects { get; set; } = new List<EnrollmentSubject>();

    public virtual Student Student { get; set; } = null!;
}
