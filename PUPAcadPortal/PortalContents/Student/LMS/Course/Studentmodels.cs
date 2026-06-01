using System;
using System.Collections.Generic;

namespace PUPAcadPortal.PortalContents.Student.LMS.Course
{
    //  Course 
    public class StudentCourse
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public string Instructor { get; set; } = "";
        public string Schedule { get; set; } = "";
        public string Room { get; set; } = "";
        public int ActivityCount { get; set; }
        public int PendingCount { get; set; }
        public int SubmittedCount { get; set; }
        public int OverdueCount { get; set; }
    }

    //  Activity 
    public class StudentActivityItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        /// <summary>Quiz | LongQuiz | Essay | FileUpload | Recitation</summary>
        public string Type { get; set; } = "";
        public string Instructions { get; set; } = "";
        public DateTime Deadline { get; set; }
        public int Points { get; set; }

        /// <summary>Pending | Submitted | Returned | Overdue | Late</summary>
        public string SubmissionStatus { get; set; } = "Pending";
        public DateTime? SubmittedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }

        public int? Score { get; set; }
        public string Remarks { get; set; } = "";
        public string ScoreBreakdown { get; set; } = "";
        public string Suggestions { get; set; } = "";

        // Essay
        public string EssayDraft { get; set; } = "";

        // Quiz
        public List<ActivityQuestion> Questions { get; set; } = new();
        public Dictionary<int, string> Answers { get; set; } = new();

        // File Upload
        public string UploadedFileName { get; set; } = "";
        public string UploadedFilePath { get; set; } = "";
        public long UploadedFileSize { get; set; }
        public string SubmissionNote { get; set; } = "";

        // Attachments (instructor-provided)
        public List<ActivityAttachment> Attachments { get; set; } = new();

        public bool LockAfterDeadline { get; set; } = false;

        // Computed helpers
        public bool IsOverdue => Deadline < DateTime.Now && SubmissionStatus == "Pending";
        public string EffectiveStatus => IsOverdue ? "Overdue" : SubmissionStatus;
    }

    //  Quiz Question 
    public class ActivityQuestion
    {
        public int Number { get; set; }
        /// <summary>MultipleChoice | TrueFalse | Identification | Essay</summary>
        public string QuestionType { get; set; } = "";
        public string Text { get; set; } = "";
        public int Points { get; set; }
        public List<string> Choices { get; set; } = new();
        public string CorrectAnswer { get; set; } = "";
    }

    //  Attachment 
    public class ActivityAttachment
    {
        public string FileName { get; set; } = "";
        public string FilePath { get; set; } = "";
        public long FileSize { get; set; }
        /// <summary>pdf | docx | pptx | image | other</summary>
        public string FileType { get; set; } = "other";

        public string FormattedSize =>
            FileSize >= 1_048_576 ? $"{FileSize / 1_048_576.0:F1} MB" :
            FileSize >= 1_024 ? $"{FileSize / 1_024.0:F0} KB" :
                                    $"{FileSize} B";
    }

    //  Notification 
    public class StudentNotification
    {
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
        public DateTime Time { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
        /// <summary>new_activity | deadline | returned | feedback</summary>
        public string Kind { get; set; } = "new_activity";
    }
}