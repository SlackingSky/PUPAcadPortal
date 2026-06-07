using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Models;
using PUPAcadPortal.PortalContents.Instructor.LMS.Course;
using System;
using System.Collections.Generic;
using System.Linq;

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
                var activities = offering.Activities.ToList();

                int totalAssignments = activities.Count(a =>
                    string.Equals(a.ActivityType, "Assignment", StringComparison.OrdinalIgnoreCase));
                int totalQuizzes = activities.Count(a =>
                    string.Equals(a.ActivityType, "Quiz", StringComparison.OrdinalIgnoreCase));

                int pending = activities.SelectMany(a => a.Submissions)
                                         .Count(s => s.Grade == null);
                int checked_ = activities.SelectMany(a => a.Submissions)
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
                    CheckedSubmissions = checked_,
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
                          && es.SubjectStatus == "Enrolled");

            return MapActivities(activities, enrolled);
        }


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
                IsPublished = false
            };

            ctx.Activities.Add(entity);
            ctx.SaveChanges();

            // Return the item with DB-generated values filled in
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

            // All enrolled students for this offering
            var enrolledStudents = ctx.EnrollmentSubjects
                .Include(es => es.Enrollment)
                    .ThenInclude(e => e.Student)
                        .ThenInclude(s => s.User)
                .Where(es => es.SubjectOfferingId == activity.SubjectOfferingId
                          && es.SubjectStatus == "Enrolled")
                .AsNoTracking()
                .ToList();

            // Actual submissions keyed by StudentId for O(1) lookup
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
                    HasFile = hasSub && !string.IsNullOrEmpty(sub!.SubmittedFile),
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
            // Truncate to 500 chars to match DB column length
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
                    LinkedModuleTitle = a.Module?.Title ?? ""
                });
            }

            return result;
        }

        private static string GenerateActivityId()
            => $"ACT-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";
    }
}