using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class EnrollmentSubject
{
    public string EnrollmentSubjId { get; set; } = null!;

    public string EnrollmentId { get; set; } = null!;

    public string SubjectOfferingId { get; set; } = null!;

    public string? Section { get; set; }

    public virtual Enrollment Enrollment { get; set; } = null!;

    public virtual ICollection<FinalCourseGrade> FinalCourseGrades { get; set; } = new List<FinalCourseGrade>();

    public virtual SubjectOffering SubjectOffering { get; set; } = null!;
}
