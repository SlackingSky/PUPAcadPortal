using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class Activity
{
    public string ActivityId { get; set; } = null!;

    public string SubjectOfferingId { get; set; } = null!;

    public string? ModuleId { get; set; }

    public int? CategoryId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int MaxPoints { get; set; }

    public DateTime Deadline { get; set; }

    public string ActivityType { get; set; } = null!;

    public bool IsPublished { get; set; }

    public string? QuizContent { get; set; }

    public virtual GradingCategory? Category { get; set; }

    public virtual Module? Module { get; set; }

    public virtual SubjectOffering SubjectOffering { get; set; } = null!;

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
