using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class Curriculum
{
    public int CurriculumId { get; set; }

    public string Program { get; set; } = null!;

    public int RevisionYear { get; set; }

    public int YearLevel { get; set; }

    public int SemesterIndex { get; set; }

    public string SubjectId { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Subject Subject { get; set; } = null!;
}
