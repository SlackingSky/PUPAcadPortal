using PUPAcadPortal.Models;
using PUPAcadPortal.PortalContents.Instructor.LMS.Course;
using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Services
{
    /// <summary>
    /// Design-time / unit-test stub for IModuleDbService.
    /// Returns empty collections and performs no database access.
    /// </summary>
    public sealed class NullModuleDbService : IModuleDbService
    {
        public List<Module> GetModulesForOffering(string subjectOfferingId)
            => new List<Module>();

        public Module CreateModule(
            string subjectOfferingId,
            string title,
            string description,
            string? fileUrl = null)
        {
            return new Module
            {
                ModuleId = $"NULL-MOD-{Guid.NewGuid():N}",
                SubjectOfferingId = subjectOfferingId,
                Title = title,
                ModuleDescription = description,
                FileUrl = fileUrl,
                UploadDate = DateTime.Now
            };
        }

        public void UpdateModule(
            string moduleId,
            string title,
            string description,
            string? fileUrl)
        { /* no-op */ }

        public void DeleteModule(string moduleId)
        { /* no-op */ }
    }

    /// <summary>
    /// Design-time / unit-test stub for IActivityDbService.
    /// Returns empty collections and performs no database access.
    /// </summary>
    public sealed class NullActivityDbService : IActivityDbService
    {
        // Dashboard
        public List<CourseActivity> GetCourseActivitiesForProfessor(int professorId)
            => new List<CourseActivity>();

        // Activity list
        public List<ActivityItem> GetActivitiesForOffering(string subjectOfferingId)
            => new List<ActivityItem>();

        // CRUD
        public ActivityItem CreateActivity(string subjectOfferingId, ActivityItem item)
        {
            item.ActivityId = $"NULL-ACT-{Guid.NewGuid():N}";
            item.SubjectOfferingId = subjectOfferingId;
            item.IsPublished = false;
            return item;
        }

        public void UpdateActivity(ActivityItem item)
        { /* no-op */ }

        public void DeleteActivity(string activityId)
        { /* no-op */ }

        public void TogglePublish(string activityId, bool publish)
        { /* no-op */ }

        // Submissions
        public List<StudentSubmission> GetSubmissionsForActivity(string activityId)
            => new List<StudentSubmission>();

        public void SaveGrade(string submissionId, int score, string remarks)
        { /* no-op */ }

        public void ReturnSubmission(string submissionId)
        { /* no-op */ }

        // Dropdowns
        public List<GradingCategory> GetCategoriesForOffering(string subjectOfferingId)
            => new List<GradingCategory>();

        public List<Module> GetModulesForOffering(string subjectOfferingId)
            => new List<Module>();
    }
}