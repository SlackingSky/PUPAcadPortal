using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Models;
using PUPAcadPortal.PortalContents.Instructor.LMS.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace PUPAcadPortal.Services
{
    public class ActivityDbService : IActivityDbService
    {
        private readonly Func<AppDbContext> _ctxFactory;

        public ActivityDbService(Func<AppDbContext> ctxFactory)
        {
            _ctxFactory = ctxFactory ?? throw new ArgumentNullException(nameof(ctxFactory));
        }

        //  Dashboard

        public List<CourseActivity> GetCourseActivitiesForProfessor(int professorId)
        {
            using var ctx = _ctxFactory();

            var offerings = ctx.SubjectOfferings
                .Include(o => o.Subject)
                .Include(o => o.Activities)
                    .ThenInclude(a => a.Submissions)
                .Where(o => o.ProfessorId == professorId)
                .AsNoTracking()
                .ToList();

            var result = new List<CourseActivity>();
            int courseCounter = 1;

            foreach (var offering in offerings)
            {
                var activities = offering.Activities?.ToList() ?? new List<Activity>();

                int totalAssignments = activities.Count(a =>
                    string.Equals(a.ActivityType, "Assignment", StringComparison.OrdinalIgnoreCase));
                int totalQuizzes = activities.Count(a =>
                    string.Equals(a.ActivityType, "Quiz", StringComparison.OrdinalIgnoreCase));

                int pending = activities
                    .SelectMany(a => a.Submissions ?? new List<Submission>())
                    .Count(s => s.Grade == null);
                int checkedCount = activities
                    .SelectMany(a => a.Submissions ?? new List<Submission>())
                    .Count(s => s.Grade != null);

                DateTime nearest = activities.Any(a => a.Deadline >= DateTime.Now)
                    ? activities.Where(a => a.Deadline >= DateTime.Now)
                                .OrderBy(a => a.Deadline)
                                .Select(a => a.Deadline)
                                .First()
                    : DateTime.Now.AddDays(30);

                result.Add(new CourseActivity
                {
                    CourseId = courseCounter++,
                    SubjectOfferingId = offering.SubjectOfferingId,
                    CourseName = offering.Subject?.SubjectName ?? offering.SubjectId,
                    CourseCode = offering.Subject?.SubjectCode ?? offering.SubjectId,
                    Section = offering.Section,
                    TotalAssignments = totalAssignments,
                    TotalQuizzes = totalQuizzes,
                    PendingSubmissions = pending,
                    CheckedSubmissions = checkedCount,
                    NearestDeadline = nearest,
                    Status = offering.Status,
                    ActivityCount = activities.Count,
                    Activities = MapActivities(activities)
                });
            }

            return result;
        }

        //  Activity list

        public List<ActivityItem> GetActivitiesForOffering(string subjectOfferingId)
        {
            using var ctx = _ctxFactory();

            var activities = ctx.Activities
                .Include(a => a.Category)
                .Include(a => a.Module)
                .Include(a => a.Submissions)
                .Where(a => a.SubjectOfferingId == subjectOfferingId)
                .AsNoTracking()
                .ToList();

            int enrolled = ctx.EnrollmentSubjects
                .Count(es => es.SubjectOfferingId == subjectOfferingId
                          && es.SubjectStatus == "Officially Enrolled");

            return MapActivities(activities, enrolled);
        }

        //  CRUD

        public ActivityItem CreateActivity(string subjectOfferingId, ActivityItem item)
        {
            using var ctx = _ctxFactory();

            string newId = GenerateActivityId();

            var entity = new Activity
            {
                ActivityId = newId,
                SubjectOfferingId = subjectOfferingId,
                Title = item.Title,
                Description = string.IsNullOrWhiteSpace(item.Description) ? null : item.Description,
                MaxPoints = item.Points,
                Deadline = item.Deadline,
                ActivityType = item.TypeString,
                CategoryId = item.LinkedCategoryId,
                ModuleId = string.IsNullOrEmpty(item.LinkedModuleId) ? null : item.LinkedModuleId,
                IsPublished = false,
                QuizContent = SerializeQuestions(item.Questions),
                RubricContent = SerializeRubric(item.RubricItems),
            };

            ctx.Activities.Add(entity);
            ctx.SaveChanges();

            item.ActivityId = newId;
            item.SubjectOfferingId = subjectOfferingId;
            item.IsPublished = false;
            return item;
        }

        public void UpdateActivity(ActivityItem item)
        {
            using var ctx = _ctxFactory();

            var entity = ctx.Activities.Find(item.ActivityId)
                ?? throw new InvalidOperationException(
                    $"Activity '{item.ActivityId}' not found.");

            entity.Title = item.Title;
            entity.Description = string.IsNullOrWhiteSpace(item.Description) ? null : item.Description;
            entity.MaxPoints = item.Points;
            entity.Deadline = item.Deadline;
            entity.ActivityType = item.TypeString;
            entity.CategoryId = item.LinkedCategoryId;
            entity.ModuleId = string.IsNullOrEmpty(item.LinkedModuleId) ? null : item.LinkedModuleId;
            entity.QuizContent = SerializeQuestions(item.Questions);
            entity.RubricContent = SerializeRubric(item.RubricItems);   // ← FIX 1

            ctx.SaveChanges();
        }

        public void DeleteActivity(string activityId)
        {
            using var ctx = _ctxFactory();

            var entity = ctx.Activities.Find(activityId);
            if (entity == null) return;

            ctx.Activities.Remove(entity);
            ctx.SaveChanges();
        }

        public void TogglePublish(string activityId, bool publish)
        {
            using var ctx = _ctxFactory();

            var entity = ctx.Activities.Find(activityId)
                ?? throw new InvalidOperationException(
                    $"Activity '{activityId}' not found.");

            entity.IsPublished = publish;
            ctx.SaveChanges();
        }

        //  Submissions

        public List<StudentSubmission> GetSubmissionsForActivity(string activityId)
        {
            using var ctx = _ctxFactory();

            var activity = ctx.Activities
                .AsNoTracking()
                .FirstOrDefault(a => a.ActivityId == activityId)
                ?? throw new InvalidOperationException(
                    $"Activity '{activityId}' not found.");

            // Normalise type once for use in the loop below
            string actType = activity.ActivityType?.ToLowerInvariant() ?? "assignment";
            bool isQuiz = actType == "quiz" || actType == "longquiz";
            bool isEssay = actType == "essay";
            bool isFileUpload = actType is "fileupload" or "file upload";

            var enrolledStudents = ctx.EnrollmentSubjects
                .Include(es => es.Enrollment)
                    .ThenInclude(e => e.Student)
                        .ThenInclude(s => s.User)
                .Where(es => es.SubjectOfferingId == activity.SubjectOfferingId
                          && es.SubjectStatus == "Officially Enrolled")
                .AsNoTracking()
                .ToList();

            var submissionMap = ctx.Submissions
                .Where(s => s.ActivityId == activityId)
                .AsNoTracking()
                .ToDictionary(s => s.StudentId);

            var result = new List<StudentSubmission>();

            foreach (var es in enrolledStudents)
            {
                var student = es.Enrollment?.Student;
                if (student == null) continue;

                var user = student.User;
                string fullName = user != null
                    ? $"{user.LastName.ToUpper()}, {user.FirstName} {user.MiddleName}".Trim()
                    : $"Student #{student.StudentId}";

                bool hasSub = submissionMap.TryGetValue(student.StudentId, out var sub);

                string status = !hasSub ? "Missing" : sub!.Status ?? "Submitted";
                bool isChecked = hasSub && sub!.Grade != null;

                // SubmittedFile stores different things depending on activity type:
                //   Quiz        → JSON-serialised answers dictionary
                //   Essay       → plain essay text
                //   FileUpload  → Cloudinary URL
                //   Assignment  → Cloudinary URL or plain text
                string submittedFile = hasSub ? (sub!.SubmittedFile ?? "") : "";

                // EssayContent is what GradingInterface reads for quiz answers AND essay text
                string essayContent = submittedFile;

                // HasFile is true only when a real file URL was stored (FileUpload / Assignment)
                bool hasFile = hasSub
                    && !string.IsNullOrEmpty(submittedFile)
                    && (isFileUpload || (!isQuiz && !isEssay))
                    && (submittedFile.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                        || submittedFile.StartsWith("https://", StringComparison.OrdinalIgnoreCase));

                result.Add(new StudentSubmission
                {
                    StudentId = student.StudentNumber ?? student.StudentId.ToString(),
                    StudentName = fullName,
                    Section = es.Section ?? "",
                    SubmissionTime = hasSub ? sub!.SubmissionDate : DateTime.MinValue,
                    Status = status,
                    Score = hasSub && sub!.Grade != null
                                        ? (int)Math.Round((double)sub!.Grade)
                                        : -1,
                    IsChecked = isChecked,
                    Remarks = hasSub ? (sub!.Remarks ?? "") : "",
                    // EssayContent carries quiz-answer JSON, essay text, or file URL
                    EssayContent = essayContent,
                    // HasFile signals the Download button in the grading UI
                    HasFile = hasFile,
                    SubmissionDbId = hasSub ? sub!.SubmissionId : string.Empty
                });
            }

            return result;
        }

        public void SaveGrade(string submissionId, int score, string remarks)
        {
            using var ctx = _ctxFactory();

            var sub = ctx.Submissions.Find(submissionId)
                ?? throw new InvalidOperationException(
                    $"Submission '{submissionId}' not found.");

            sub.Grade = score;
            sub.Status = "Graded";
            sub.Remarks = remarks?.Length > 500 ? remarks[..500] : remarks;

            ctx.SaveChanges();
        }

        public void ReturnSubmission(string submissionId)
        {
            using var ctx = _ctxFactory();

            var sub = ctx.Submissions.Find(submissionId)
                ?? throw new InvalidOperationException(
                    $"Submission '{submissionId}' not found.");

            sub.Status = "Returned";
            ctx.SaveChanges();
        }

        //  Dropdowns

        public List<GradingCategory> GetCategoriesForOffering(string subjectOfferingId)
        {
            using var ctx = _ctxFactory();

            return ctx.GradingCategories
                .Where(c => c.SubjectOfferingId == subjectOfferingId)
                .AsNoTracking()
                .OrderBy(c => c.CategoryName)
                .ToList();
        }

        public List<Module> GetModulesForOffering(string subjectOfferingId)
        {
            using var ctx = _ctxFactory();

            return ctx.Modules
                .Where(m => m.SubjectOfferingId == subjectOfferingId)
                .AsNoTracking()
                .OrderBy(m => m.UploadDate)
                .ToList();
        }

        // ── Mapping ──────────────────────────────────────────────────────────

        private static List<ActivityItem> MapActivities(
            IEnumerable<Activity> activities,
            int enrolledCount = 0)
        {
            var result = new List<ActivityItem>();
            int idx = 1;

            foreach (var a in activities)
            {
                int submitted = a.Submissions?.Count(s => s.Status != "Missing") ?? 0;
                int late = a.Submissions?.Count(s => s.Status == "Late") ?? 0;
                int graded = a.Submissions?.Count(s => s.Grade != null) ?? 0;
                int total = enrolledCount > 0
                    ? enrolledCount
                    : (a.Submissions?.Count ?? 0);

                ActivityType typeEnum = a.ActivityType?.ToLowerInvariant() switch
                {
                    "quiz" => ActivityType.Quiz,
                    "essay" => ActivityType.Essay,
                    "fileupload" or "file upload" => ActivityType.FileUpload,
                    _ => ActivityType.Assignment
                };

                var rubric = DeserializeRubric(a.RubricContent);   // ← FIX 1
                bool hasRubric = rubric.Count > 0;

                result.Add(new ActivityItem
                {
                    Id = idx++,
                    ActivityId = a.ActivityId,
                    SubjectOfferingId = a.SubjectOfferingId,
                    Title = a.Title,
                    Description = a.Description ?? "",
                    Type = typeEnum,
                    Deadline = a.Deadline,
                    Points = a.MaxPoints,
                    TotalStudents = total,
                    SubmittedCount = submitted,
                    LateCount = late,
                    CheckedCount = graded,
                    MissingCount = Math.Max(0, total - submitted),
                    IsPublished = a.IsPublished,
                    LinkedCategoryId = a.CategoryId,
                    LinkedCategoryName = a.Category?.CategoryName ?? "",
                    LinkedModuleId = a.ModuleId,
                    LinkedModuleTitle = a.Module?.Title ?? "",
                    Questions = DeserializeQuestions(a.QuizContent),
                    RubricItems = rubric,          // ← FIX 1
                    HasRubric = hasRubric,       // ← FIX 1
                });
            }

            return result;
        }

        private static string GenerateActivityId()
            => $"ACT-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";

        // ── Quiz question JSON helpers ────────────────────────────────────────

        private static string? SerializeQuestions(List<QuizQuestion>? questions)
        {
            if (questions == null || questions.Count == 0) return null;

            var dto = questions.Select((q, i) => new
            {
                number = i + 1,
                type = q.QuestionType,
                text = q.QuestionText,
                points = q.Points,
                choices = q.Choices,
                answer = q.CorrectAnswer
            });

            return JsonSerializer.Serialize(dto);
        }

        private static List<QuizQuestion> DeserializeQuestions(string? json)
        {
            if (string.IsNullOrWhiteSpace(json)) return new List<QuizQuestion>();

            try
            {
                using var doc = JsonDocument.Parse(json);
                var list = new List<QuizQuestion>();
                int idx = 1;
                foreach (var el in doc.RootElement.EnumerateArray())
                {
                    var choices = new List<string>();
                    if (el.TryGetProperty("choices", out var choicesEl))
                        foreach (var c in choicesEl.EnumerateArray())
                            choices.Add(c.GetString() ?? "");

                    list.Add(new QuizQuestion
                    {
                        QuestionId = idx,
                        QuestionText = el.TryGetProperty("text", out var t) ? t.GetString() ?? "" : "",
                        QuestionType = el.TryGetProperty("type", out var ty) ? ty.GetString() ?? "MultipleChoice" : "MultipleChoice",
                        Points = el.TryGetProperty("points", out var p) ? p.GetInt32() : 1,
                        Choices = choices,
                        CorrectAnswer = el.TryGetProperty("answer", out var a) ? a.GetString() ?? "" : "",
                    });
                    idx++;
                }
                return list;
            }
            catch
            {
                return new List<QuizQuestion>();
            }
        }

        // ── Rubric JSON helpers ───────────────────────────────────────────────

        /// <summary>Serializes rubric criteria list to JSON for DB storage.</summary>
        private static string? SerializeRubric(List<RubricCriteria>? rubric)
        {
            if (rubric == null || rubric.Count == 0) return null;

            var dto = rubric.Select(r => new
            {
                name = r.Name,
                description = r.Description,
                maxPoints = r.MaxPoints,
            });

            return JsonSerializer.Serialize(dto);
        }

        /// <summary>Deserializes rubric JSON from DB back into instructor-side RubricCriteria list.</summary>
        private static List<RubricCriteria> DeserializeRubric(string? json)
        {
            if (string.IsNullOrWhiteSpace(json)) return new List<RubricCriteria>();

            try
            {
                using var doc = JsonDocument.Parse(json);
                var list = new List<RubricCriteria>();
                int idx = 1;
                foreach (var el in doc.RootElement.EnumerateArray())
                {
                    list.Add(new RubricCriteria
                    {
                        CriteriaId = idx++,
                        Name = el.TryGetProperty("name", out var n) ? n.GetString() ?? "" : "",
                        Description = el.TryGetProperty("description", out var d) ? d.GetString() ?? "" : "",
                        MaxPoints = el.TryGetProperty("maxPoints", out var p) ? p.GetInt32() : 0,
                    });
                }
                return list;
            }
            catch
            {
                return new List<RubricCriteria>();
            }
        }
    }
}