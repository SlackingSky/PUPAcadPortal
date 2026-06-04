using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class Subject
{
    public string SubjectId { get; set; } = null!;

    public string SubjectCode { get; set; } = null!;

    public string SubjectName { get; set; } = null!;

    public int? DepartmentId { get; set; }

    public string? Description { get; set; }

    public int LecUnits { get; set; }

    public int LabUnits { get; set; }

    public int Units { get; set; }

    public virtual ICollection<Curriculum> Curricula { get; set; } = new List<Curriculum>();

    public virtual Department? Department { get; set; }

    public virtual ICollection<SubjectOffering> SubjectOfferings { get; set; } = new List<SubjectOffering>();

    public virtual ICollection<SubjectPrerequisite> SubjectPrerequisiteRequiredSubjects { get; set; } = new List<SubjectPrerequisite>();

    public virtual ICollection<SubjectPrerequisite> SubjectPrerequisiteSubjects { get; set; } = new List<SubjectPrerequisite>();
}
