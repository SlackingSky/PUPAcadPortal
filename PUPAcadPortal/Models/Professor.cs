using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class Professor
{
    public int ProfessorId { get; set; }

    public int UserId { get; set; }

    public string EmployeeId { get; set; } = null!;

    public string Department { get; set; } = null!;

    public string EmploymentType { get; set; } = null!;

    public int MaxLoad { get; set; }

    public string HighestDegree { get; set; } = null!;

    public int YearsOfExperience { get; set; }

    public string EmploymentStatus { get; set; } = null!;

    public string Rank { get; set; } = null!;

    public virtual ICollection<FinalCourseGrade> FinalCourseGrades { get; set; } = new List<FinalCourseGrade>();

    public virtual ICollection<ProfessorAvailability> ProfessorAvailabilities { get; set; } = new List<ProfessorAvailability>();

    public virtual ICollection<SubjectOffering> SubjectOfferings { get; set; } = new List<SubjectOffering>();

    public virtual User User { get; set; } = null!;
}
