using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class FinalCourseGrade
{
    public int GradeId { get; set; }

    public string EnrollmentSubjId { get; set; } = null!;

    public int EncodedByProfId { get; set; }

    public decimal FinalCalculatedGrade { get; set; }

    public decimal FinalRating { get; set; }

    public string Remarks { get; set; } = null!;

    public virtual Professor EncodedByProf { get; set; } = null!;

    public virtual EnrollmentSubject EnrollmentSubj { get; set; } = null!;
}
