using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Models;
using PUPAcadPortal.PortalContents.Student.LMS.Course;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using CourseActivityAttachment = PUPAcadPortal.PortalContents.Student.LMS.Course.ActivityAttachment;
using CourseStudentActivityItem = PUPAcadPortal.PortalContents.Student.LMS.Course.StudentActivityItem;
using CourseStudentCourse = PUPAcadPortal.PortalContents.Student.LMS.Course.StudentCourse;

namespace PUPAcadPortal.Services
{
    public class StudentCourseDbService : IStudentCourseDbService
    {
        private readonly Func<AppDbContext> _ctxFactory;

        public StudentCourseDbService(Func<AppDbContext> ctxFactory)
        {
            _ctxFactory = ctxFactory ?? throw new ArgumentNullException(nameof(ctxFactory));
        }

        private static List<ActivityRubricItem> DeserializeStudentRubric(string? json)
        {
            var result = new List<ActivityRubricItem>();
            if (string.IsNullOrWhiteSpace(json)) return result;
            try
            {
                using var doc = JsonDocument.Parse(json);
                foreach (var el in doc.RootElement.EnumerateArray())
                {
                    result.Add(new ActivityRubricItem
                    {
                        Name = el.TryGetProperty("name", out var n) ? n.GetString() ?? "" : "",
                        Description = el.TryGetProperty("description", out var d) ? d.GetString() ?? "" : "",
                        MaxPoints = el.TryGetProperty("maxPoints", out var p) ? p.GetInt32() : 0,
                    });
                }
            }
            catch { }
            return result;
        }

        public List<CourseStudentCourse> GetCoursesForStudent(int studentId)
        {
            using var ctx = _ctxFactory();

            var enrollments = ctx.EnrollmentSubjects
                .Include(es => es.Enrollment)
                .Include(es => es.SubjectOffering)
                    .ThenInclude(o => o.Subject)
                .Include(es => es.SubjectOffering)
                    .ThenInclude(o => o.Professor)
                        .ThenInclude(p => p.User)
                .Include(es => es.SubjectOffering)
                    .ThenInclude(o => o.RoomSchedules)
                        .ThenInclude(rs => rs.Room)
                .Include(es => es.SubjectOffering)
                    .ThenInclude(o => o.Activities)
                        .ThenInclude(a => a.Submissions)
                .Where(es => es.Enrollment.StudentId == studentId
                          && es.SubjectStatus == "Officially Enrolled")
                .AsNoTracking()
                .ToList();

            var result = new List<CourseStudentCourse>();
            int row = 1;

            foreach (var enrollment in enrollments)
            {
                var offering = enrollment.SubjectOffering;
                var activities = offering.Activities
                    .Where(a => a.IsPublished)
                    .ToList();

                int submitted = 0, pending = 0, overdue = 0;

                foreach (var activity in activities)
                {
                    var sub = activity.Submissions
                        .Where(s => s.StudentId == studentId)
                        .OrderByDescending(s => s.SubmissionDate)
                        .FirstOrDefault();

                    if (sub == null)
                    {
                        if (activity.Deadline < DateTime.Now) overdue++;
                        else pending++;
                    }
                    else if (!string.Equals(sub.Status, "Missing", StringComparison.OrdinalIgnoreCase))
                    {
                        submitted++;
                    }
                }

                result.Add(new CourseStudentCourse
                {
                    Id = row++,
                    SubjectOfferingId = offering.SubjectOfferingId,
                    Name = offering.Subject?.SubjectName ?? offering.SubjectId,
                    Code = offering.Subject?.SubjectCode ?? offering.SubjectId,
                    Instructor = FormatProfessorName(offering.Professor),
                    Schedule = FormatSchedule(offering.RoomSchedules),
                    Room = FormatRoom(offering.RoomSchedules),
                    ActivityCount = activities.Count,
                    PendingCount = pending,
                    SubmittedCount = submitted,
                    OverdueCount = overdue,
                });
            }

            return result
                .OrderBy(c => c.Code)
                .ThenBy(c => c.Name)
                .ToList();
        }

        private static readonly string[] _activeEnrollmentStatuses = {
            "Officially Enrolled",
            "Pending Payment",
            "Enrolled",
            "Enrolled - For Assessment",
        };

        public List<CourseStudentActivityItem> GetActivitiesForStudentOffering(
            string subjectOfferingId,
            int studentId)
        {
            if (string.IsNullOrWhiteSpace(subjectOfferingId))
                return new List<CourseStudentActivityItem>();

            using var ctx = _ctxFactory();

            bool enrolled = ctx.EnrollmentSubjects
                .Any(es => es.SubjectOfferingId == subjectOfferingId
                        && es.Enrollment.StudentId == studentId);

            if (!enrolled)
                return new List<CourseStudentActivityItem>();

            var activities = ctx.Activities
                .Include(a => a.Module)
                .Where(a => a.SubjectOfferingId == subjectOfferingId
                         && a.IsPublished)
                .AsNoTracking()
                .OrderBy(a => a.Deadline)
                .ToList();

            var submissions = ctx.Submissions
                .Where(s => s.StudentId == studentId
                         && ctx.Activities.Any(a => a.ActivityId == s.ActivityId
                                                 && a.SubjectOfferingId == subjectOfferingId
                                                 && a.IsPublished))
                .AsNoTracking()
                .ToList()
                .GroupBy(s => s.ActivityId)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderByDescending(s => s.SubmissionDate).First());

            var result = new List<CourseStudentActivityItem>();
            int row = 1;

            foreach (var activity in activities)
            {
                submissions.TryGetValue(activity.ActivityId, out var submission);

                result.Add(new CourseStudentActivityItem
                {
                    Id = row++,
                    ActivityId = activity.ActivityId,
                    Title = activity.Title,
                    Type = NormalizeActivityType(activity.ActivityType),
                    Instructions = activity.Description ?? "No instructions were provided.",
                    Deadline = activity.Deadline,
                    Points = activity.MaxPoints,
                    SubmissionDbId = submission?.SubmissionId ?? "",
                    SubmissionStatus = NormalizeSubmissionStatus(submission),
                    SubmittedAt = submission?.SubmissionDate,
                    ReturnedAt = string.Equals(
                                           submission?.Status, "Returned",
                                           StringComparison.OrdinalIgnoreCase)
                                       ? submission?.SubmissionDate
                                       : null,
                    Score = submission?.Grade.HasValue == true
                                       ? (int)Math.Round(submission.Grade.Value)
                                       : null,
                    Remarks = submission?.Remarks ?? "",
                    UploadedFilePath = submission?.SubmittedFile ?? "",
                    UploadedFileName = GetFileName(submission?.SubmittedFile),
                    Attachments = BuildAttachments(activity),
                    LockAfterDeadline = false,
                    Questions = DeserializeStudentQuestions(activity.QuizContent),
                    RubricItems = DeserializeStudentRubric(activity.RubricContent),
                });
            }

            return result;
        }

        public CourseStudentActivityItem SubmitActivity(
            int studentId,
            CourseStudentActivityItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (string.IsNullOrWhiteSpace(item.ActivityId))
                throw new ArgumentException("ActivityId is required.", nameof(item));

            using var ctx = _ctxFactory();

            var activity = ctx.Activities
                .AsNoTracking()
                .FirstOrDefault(a => a.ActivityId == item.ActivityId)
                ?? throw new InvalidOperationException(
                    $"Activity '{item.ActivityId}' not found.");

            bool enrolled = ctx.EnrollmentSubjects
                .Any(es => es.SubjectOfferingId == activity.SubjectOfferingId
                        && es.Enrollment.StudentId == studentId
                        && es.SubjectStatus == "Officially Enrolled");

            if (!enrolled)
                throw new InvalidOperationException(
                    "The current student is not enrolled in this course.");

            var submission = ctx.Submissions
                .FirstOrDefault(s => s.ActivityId == item.ActivityId
                                  && s.StudentId == studentId);


            var now = DateTime.Now;
            string status = now > activity.Deadline ? "Late" : "Submitted";

            // Determine type before building submittedFile
            bool isQuiz = string.Equals(activity.ActivityType, "Quiz",
                StringComparison.OrdinalIgnoreCase);

            // For quizzes, serialize the answers as JSON; otherwise use the uploaded file path
            string? submittedFile;
            if (isQuiz && item.Answers != null && item.Answers.Count > 0)
                submittedFile = System.Text.Json.JsonSerializer.Serialize(item.Answers);
            else
                submittedFile = BuildSubmittedFileValue(item);

            if (submission == null)
            {
                submission = new Submission
                {
                    SubmissionId = GenerateSubmissionId(),
                    ActivityId = item.ActivityId,
                    StudentId = studentId,
                    SubmissionDate = now,
                    Status = status,
                    SubmittedFile = submittedFile,
                };
                ctx.Submissions.Add(submission);
            }
            else
            {
                if (submission.Grade != null
                    || string.Equals(submission.Status, "Returned", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(submission.Status, "Graded", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(
                        "This activity has already been checked by the instructor.");
                }

                submission.SubmissionDate = now;
                submission.Status = status;
                submission.SubmittedFile = submittedFile;
            }

            ctx.SaveChanges();

            item.SubmissionDbId = submission.SubmissionId;
            item.SubmissionStatus = status;
            item.SubmittedAt = now;

            if (!string.IsNullOrWhiteSpace(submittedFile))
            {
                item.UploadedFilePath = submittedFile;
                item.UploadedFileName = GetFileName(submittedFile);
            }

            // Auto-score quiz submissions
            if (isQuiz && item.Answers != null && item.Answers.Count > 0
                && !string.IsNullOrWhiteSpace(activity.QuizContent))
            {
                var questions = DeserializeQuizForScoring(activity.QuizContent);
                int totalPoints = questions.Sum(q => q.Points);
                int earned = 0;

                foreach (var q in questions)
                {
                    if (item.Answers.TryGetValue(q.Number, out string? studentAnswer)
                        && !string.IsNullOrWhiteSpace(studentAnswer)
                        && string.Equals(studentAnswer.Trim(), q.CorrectAnswer.Trim(),
                           StringComparison.OrdinalIgnoreCase))
                    {
                        earned += q.Points;
                    }
                }

                // Scale earned points to activity's MaxPoints
                int scaledScore = totalPoints > 0
                    ? (int)Math.Round((double)earned / totalPoints * activity.MaxPoints)
                    : 0;

                submission.Grade = scaledScore;
                submission.Status = "Graded";
                submission.SubmittedFile = submittedFile; // already serialized answers JSON above
                ctx.SaveChanges();

                item.Score = scaledScore;
            }

            return item;
        }
        private static List<(int Number, string CorrectAnswer, int Points)> DeserializeQuizForScoring(string json)
        {
            var result = new List<(int, string, int)>();
            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(json);
                foreach (var el in doc.RootElement.EnumerateArray())
                {
                    int num = el.TryGetProperty("number", out var n) ? n.GetInt32() : 0;
                    string ans = el.TryGetProperty("answer", out var a) ? a.GetString() ?? "" : "";
                    int pts = el.TryGetProperty("points", out var p) ? p.GetInt32() : 1;
                    result.Add((num, ans, pts));
                }
            }
            catch { }
            return result;
        }



        private static List<CourseActivityAttachment> BuildAttachments(Activity activity)
        {
            if (activity.Module == null || string.IsNullOrWhiteSpace(activity.Module.FileUrl))
                return new List<CourseActivityAttachment>();

            return new List<CourseActivityAttachment>
            {
                new CourseActivityAttachment
                {
                    FileName = GetFileName(activity.Module.FileUrl, activity.Module.Title),
                    FilePath = activity.Module.FileUrl,
                    FileType = GetExtension(activity.Module.FileUrl),
                }
            };
        }

        private static string? BuildSubmittedFileValue(CourseStudentActivityItem item)
        {
            string? value = null;

            // Priority 1 — uploaded file URL (FileUpload / Assignment with file)
            if (!string.IsNullOrWhiteSpace(item.UploadedFilePath))
                value = item.UploadedFilePath.Trim();
            else if (!string.IsNullOrWhiteSpace(item.UploadedFileName))
                value = item.UploadedFileName.Trim();

            // Priority 2 — essay text (Essay type stores draft text, not a file)
            if (string.IsNullOrWhiteSpace(value) && !string.IsNullOrWhiteSpace(item.EssayDraft))
                value = item.EssayDraft.Trim();

            if (string.IsNullOrWhiteSpace(value))
                return null;

            // SubmittedFile is now TEXT — no hard truncation needed.
            // Apply a generous safety cap of 65535 chars (MySQL TEXT max).
            return value.Length <= 65535 ? value : value[..65535];
        }

        private static string FormatProfessorName(Professor? professor)
        {
            if (professor?.User == null)
                return professor == null ? "TBA" : $"Professor #{professor.ProfessorId}";

            var user = professor.User;
            string fullName = $"{user.FirstName} {user.LastName}".Trim();
            return string.IsNullOrWhiteSpace(fullName) ? "TBA" : $"Prof. {fullName}";
        }

        private static string FormatSchedule(IEnumerable<RoomSchedule> schedules)
        {
            var parts = schedules
                .OrderBy(s => s.DayOfWeek)
                .ThenBy(s => s.StartTime)
                .Select(s => $"{s.DayOfWeek} {FormatTime(s.StartTime)}-{FormatTime(s.EndTime)}")
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct()
                .Take(3)
                .ToList();

            return parts.Count == 0 ? "TBA" : string.Join("; ", parts);
        }

        private static string FormatRoom(IEnumerable<RoomSchedule> schedules)
        {
            var rooms = schedules
                .Select(s => s.Room?.RoomName)
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .Distinct()
                .ToList();

            return rooms.Count == 0 ? "" : string.Join(", ", rooms);
        }

        private static string FormatTime(TimeSpan value)
            => DateTime.Today.Add(value).ToString("h:mm tt");

        private static string NormalizeActivityType(string? activityType)
        {
            return activityType?.Trim().ToLowerInvariant() switch
            {
                "quiz" => "Quiz",
                "longquiz" or "long quiz" => "LongQuiz",
                "essay" => "Essay",
                "fileupload" or "file upload" => "FileUpload",
                "recitation" => "Recitation",
                "lab" => "Assignment",
                _ => "Assignment",
            };
        }

        private static string NormalizeSubmissionStatus(Submission? submission)
        {
            if (submission == null)
                return "Pending";

            return string.IsNullOrWhiteSpace(submission.Status)
                ? "Submitted"
                : submission.Status;
        }

        private static string GetFileName(string? path, string fallback = "")
        {
            if (string.IsNullOrWhiteSpace(path))
                return fallback;

            try
            {
                string name = Path.GetFileName(path);
                return string.IsNullOrWhiteSpace(name) ? fallback : name;
            }
            catch
            {
                return fallback;
            }
        }

        private static string GetExtension(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return "other";

            try
            {
                string ext = Path.GetExtension(path).TrimStart('.').ToLowerInvariant();
                return string.IsNullOrWhiteSpace(ext) ? "other" : ext;
            }
            catch
            {
                return "other";
            }
        }

        private static string GenerateSubmissionId()
            => $"SUB-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";

        // ── Quiz question JSON deserializer (student-facing model) ────────────

        private static List<PUPAcadPortal.PortalContents.Student.LMS.Course.ActivityQuestion>
            DeserializeStudentQuestions(string? json)
        {
            var result = new List<PUPAcadPortal.PortalContents.Student.LMS.Course.ActivityQuestion>();
            if (string.IsNullOrWhiteSpace(json)) return result;

            try
            {
                using var doc = JsonDocument.Parse(json);
                int idx = 1;
                foreach (var el in doc.RootElement.EnumerateArray())
                {
                    var choices = new List<string>();
                    if (el.TryGetProperty("choices", out var choicesEl))
                        foreach (var c in choicesEl.EnumerateArray())
                            choices.Add(c.GetString() ?? "");

                    result.Add(new PUPAcadPortal.PortalContents.Student.LMS.Course.ActivityQuestion
                    {
                        Number = el.TryGetProperty("number", out var n) ? n.GetInt32() : idx,
                        QuestionType = el.TryGetProperty("type", out var ty) ? ty.GetString() ?? "MultipleChoice" : "MultipleChoice",
                        Text = el.TryGetProperty("text", out var t) ? t.GetString() ?? "" : "",
                        Points = el.TryGetProperty("points", out var p) ? p.GetInt32() : 1,
                        Choices = choices,
                        CorrectAnswer = el.TryGetProperty("answer", out var a) ? a.GetString() ?? "" : "",
                    });
                    idx++;
                }
            }
            catch
            {
                // Return whatever was parsed so far; corrupt JSON = no questions.
            }

            return result;
        }
    }
}