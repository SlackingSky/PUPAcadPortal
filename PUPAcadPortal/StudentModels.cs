using System;

namespace PUPAcadPortal
{
    /// <summary>
    /// Represents a course as seen by a student on their activity dashboard.
    /// </summary>
    public class StudentCourse
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public string Instructor { get; set; } = "";
        public int ActivityCount { get; set; }
        public int PendingCount { get; set; }
        public int SubmittedCount { get; set; }
        public int OverdueCount { get; set; }
    }

    /// <summary>
    /// Represents a single activity item as seen by a student.
    /// </summary>
    public class StudentActivityItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Type { get; set; } = "Assignment";
        public DateTime Deadline { get; set; }
        public int Points { get; set; }
        public string SubmissionStatus { get; set; } = "Not Started";
        public string Instructions { get; set; } = "";
        public string EssayDraft { get; set; } = "";
    }
}
