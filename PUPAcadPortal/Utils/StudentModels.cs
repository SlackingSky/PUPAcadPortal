using System;
using System.Collections.Generic;

namespace PUPAcadPortal
{
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

    public class ActivityQuestion
    {
        public int Number { get; set; }
        public string QuestionType { get; set; } = "MultipleChoice"; // MultipleChoice | TrueFalse | Identification | Essay
        public string Text { get; set; } = "";
        public List<string> Choices { get; set; } = new();   // filled for MC
        public string CorrectAnswer { get; set; } = "";      // for auto-grading (not shown to student)
        public int Points { get; set; } = 1;
    }

    public class ActivityAttachment
    {
        public string FileName { get; set; } = "";
        public string FilePath { get; set; } = "";   // or URL
        public long FileSizeBytes { get; set; }
    }

    public class StudentActivityItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Type { get; set; } = "Quiz";   // Quiz | LongQuiz | FileUpload | Recitation | Essay
        public DateTime Deadline { get; set; }
        public int Points { get; set; }
        public bool LockAfterDeadline { get; set; } = true;

        //  Submission state 
        public string SubmissionStatus { get; set; } = "Pending";
        // Statuses: Pending | Submitted | Late | Returned | Overdue

        public DateTime? SubmittedAt { get; set; }

        //  Grading / Remarks (filled when Returned) 
        public int? Score { get; set; }        // null until graded
        public string Remarks { get; set; } = "";  // instructor feedback
        public DateTime? ReturnedAt { get; set; }

        //  Content 
        public string Instructions { get; set; } = "";
        public List<ActivityAttachment> Attachments { get; set; } = new();
        public List<ActivityQuestion> Questions { get; set; } = new();

        //  Student answers 
        public string EssayDraft { get; set; } = "";
        public Dictionary<int, string> Answers { get; set; } = new(); // questionNumber → answer
        public string UploadedFileName { get; set; } = "";
        public string SubmissionNote { get; set; } = "";
    }
}