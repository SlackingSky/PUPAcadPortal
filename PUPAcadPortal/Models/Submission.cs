// ============================================================
//  Submission.cs  — UPDATED
//  Added Status (varchar 20) and Remarks (varchar 20)
//  to match the Submissions table definition exactly.
// ============================================================

using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class Submission
{
    public string SubmissionId { get; set; } = null!;

    public string ActivityId { get; set; } = null!;

    public int StudentId { get; set; }

    public string? SubmittedFile { get; set; }

    public DateTime SubmissionDate { get; set; }

    public decimal? Grade { get; set; }

    public string Status { get; set; } = "Submitted";

    public string? Remarks { get; set; }

    public virtual Activity Activity { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}