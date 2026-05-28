using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class SubjectPrerequisite
{
    public int PrerequisiteId { get; set; }

    public string SubjectId { get; set; } = null!;

    public string RequiredSubjectId { get; set; } = null!;

    public virtual Subject RequiredSubject { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}
