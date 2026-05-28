using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class Module
{
    public string ModuleId { get; set; } = null!;

    public string SubjectOfferingId { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? ModuleDescription { get; set; }

    public string? FileUrl { get; set; }

    public DateTime UploadDate { get; set; }

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

    public virtual SubjectOffering SubjectOffering { get; set; } = null!;
}
