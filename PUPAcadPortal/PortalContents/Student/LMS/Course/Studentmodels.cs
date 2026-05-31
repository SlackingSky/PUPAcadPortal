using System;
using System.Collections.Generic;

namespace PUPAcadPortal.PortalContents.Student.LMS.Course
{
    // ─── Course ───────────────────────────────────────────────────────────────
    // Extends the original StudentCourse with optional new fields.
    // The original already has: Id, Name, Code, Instructor,
    //   ActivityCount, PendingCount, SubmittedCount, OverdueCount
    public class StudentCourse
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public string Instructor { get; set; } = "";
        public string Schedule { get; set; } = "";   // new
        public string Room { get; set; } = "";   // new
        public int ActivityCount { get; set; }
        public int PendingCount { get; set; }
        public int SubmittedCount { get; set; }
        public int OverdueCount { get; set; }
    }

    // ─── Activity ──────────────────────────────────────────────────────────────
    // Extends the original StudentActivityItem.
    // Original already has: Id, Title, Type, Instructions, Deadline, Points,
    //   SubmissionStatus, SubmittedAt, ReturnedAt, Score, Remarks,
    //   EssayDraft, Questions, Answers, UploadedFileName, SubmissionNote,
    //   Attachments, LockAfterDeadline
    public class StudentActivityItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Type { get; set; } = "";
        public string Instructions { get; set; } = "";
        public DateTime Deadline { get; set; }
        public int Points { get; set; }

        public string SubmissionStatus { get; set; } = "Pending";
        public DateTime? SubmittedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }

        public int? Score { get; set; }
        public string Remarks { get; set; } = "";

        // new — feedback details
        public string ScoreBreakdown { get; set; } = "";
        public string Suggestions { get; set; } = "";

        // Essay
        public string EssayDraft { get; set; } = "";

        // Quiz
        public List<ActivityQuestion> Questions { get; set; } = new();
        public Dictionary<int, string> Answers { get; set; } = new();

        // File upload
        public string UploadedFileName { get; set; } = "";
        public string UploadedFilePath { get; set; } = "";
        public long UploadedFileSize { get; set; }
        public string SubmissionNote { get; set; } = "";

        // Instructor-provided reference files
        public List<ActivityAttachment> Attachments { get; set; } = new();

        public bool LockAfterDeadline { get; set; } = false;

        // Computed helpers (safe to use in code-behind)
        public bool IsOverdue => Deadline < DateTime.Now && SubmissionStatus == "Pending";
        public string EffectiveStatus => IsOverdue ? "Overdue" : SubmissionStatus;
    }

    // ─── Quiz Question ─────────────────────────────────────────────────────────
    // Same as original ActivityQuestion
    public class ActivityQuestion
    {
        public int Number { get; set; }
        public string QuestionType { get; set; } = "";
        public string Text { get; set; } = "";
        public int Points { get; set; }
        public List<string> Choices { get; set; } = new();
        public string CorrectAnswer { get; set; } = "";
    }

    // ─── Attachment ────────────────────────────────────────────────────────────
    // Extends original ActivityAttachment (original only had FileName, FilePath).
    public class ActivityAttachment
    {
        public string FileName { get; set; } = "";
        public string FilePath { get; set; } = "";
        public long FileSize { get; set; }       // new
        public string FileType { get; set; } = "other"; // new: pdf|docx|pptx|image|other

        // Computed helper — avoids referencing it in Designer
        public string FormattedSize =>
            FileSize >= 1_048_576 ? $"{FileSize / 1_048_576.0:F1} MB" :
            FileSize >= 1_024 ? $"{FileSize / 1_024.0:F0} KB" :
            FileSize > 0 ? $"{FileSize} B" : "";
    }

    // ─── Notification ──────────────────────────────────────────────────────────
    public class StudentNotification
    {
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
        public DateTime Time { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
        public string Kind { get; set; } = "new_activity";
    }
}