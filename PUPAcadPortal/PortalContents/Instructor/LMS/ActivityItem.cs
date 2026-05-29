using System;
using System.Collections.Generic;

namespace PUPAcadPortal
{
    // ── Activity types ────────────────────────────────────────────────────────
    public enum ActivityType { Assignment, Quiz, Essay, FileUpload }

    // ── Main activity model ───────────────────────────────────────────────────
    public class ActivityItem
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public ActivityType Type { get; set; } = ActivityType.Assignment;
        public string TypeString => Type.ToString();
        public DateTime Deadline { get; set; } = DateTime.Now.AddDays(7);
        public int Points { get; set; } = 100;
        public bool HasRubric { get; set; } = false;

        // Submission counters (in-memory)
        public int TotalStudents { get; set; } = 35;
        public int SubmittedCount { get; set; }
        public int LateCount { get; set; }
        public int MissingCount { get; set; }
        public int CheckedCount { get; set; }

        // Sub-collections
        public List<QuizQuestion> Questions { get; set; } = new();
        public List<RubricCriteria> RubricItems { get; set; } = new();
        public List<CourseFileItem> AttachedFiles { get; set; } = new();

        // Derived helpers
        public bool IsOverdue => Deadline < DateTime.Now;
        public int PendingCount => SubmittedCount - CheckedCount;
    }

    // ── Quiz ─────────────────────────────────────────────────────────────────
    public class QuizQuestion
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = "";
        public string QuestionType { get; set; } = "MultipleChoice"; // MultipleChoice | Identification | TrueFalse | Essay
        public List<string> Choices { get; set; } = new();
        public string CorrectAnswer { get; set; } = "";
        public int Points { get; set; } = 1;
    }

    // ── Rubric ───────────────────────────────────────────────────────────────
    public class RubricCriteria
    {
        public int CriteriaId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int MaxPoints { get; set; } = 25;
    }

    // ── Course files ─────────────────────────────────────────────────────────
    public class CourseFileItem
    {
        public int FileId { get; set; }
        public string FileName { get; set; } = "";
        public string FilePath { get; set; } = "";
        public string FileType { get; set; } = "";
        public long FileSizeBytes { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.Now;
        public int CourseId { get; set; }
    }

    // ── Student submission ───────────────────────────────────────────────────
    public class StudentSubmission
    {
        public string StudentId { get; set; } = "";
        public string StudentName { get; set; } = "";
        public string Section { get; set; } = "";
        public string GradeLevel { get; set; } = "";
        public DateTime SubmissionTime { get; set; }
        public string Status { get; set; } = "Missing"; // Submitted | Late | Missing | Returned
        public int Score { get; set; } = -1;
        public bool IsChecked { get; set; } = false;
        public string Remarks { get; set; } = "";
        public bool HasFile { get; set; } = false;
        public string EssayContent { get; set; } = "";
        // Rubric per-criterion scores (keyed by CriteriaId)
        public Dictionary<int, int> RubricScores { get; set; } = new();
    }

    // ── Course activity (course card model) ──────────────────────────────────
    public class CourseActivity
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = "";
        public string CourseCode { get; set; } = "";
        public string InstructorName { get; set; } = "";
        public int TotalAssignments { get; set; }
        public int TotalQuizzes { get; set; }
        public int PendingSubmissions { get; set; }
        public int CheckedSubmissions { get; set; }
        public DateTime NearestDeadline { get; set; }
        public string Status { get; set; } = "Active";
        public int ActivityCount { get; set; }

        // Activities owned by this course
        public List<ActivityItem> Activities { get; set; } = new();
        // Course-level files
        public List<CourseFileItem> CourseFiles { get; set; } = new();
    }
}