using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentCode { get; set; } = null!;

    public string DepartmentName { get; set; } = null!;

    public int? DeanProfessorId { get; set; }

    public bool? IsActive { get; set; }

    public virtual Professor? DeanProfessor { get; set; }

    public virtual ICollection<DepartmentPrefix> DepartmentPrefixes { get; set; } = new List<DepartmentPrefix>();

    public virtual ICollection<Professor> Professors { get; set; } = new List<Professor>();

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
