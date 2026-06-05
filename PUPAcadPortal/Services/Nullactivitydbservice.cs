using PUPAcadPortal.Models;
using PUPAcadPortal.PortalContents.Instructor.LMS.Course;
using System.Collections.Generic;

namespace PUPAcadPortal.Services
{
    public class NullModuleDbService : IModuleDbService
    {
        public List<Module> GetModulesForOffering(string subjectOfferingId)
            => new();

        public Module CreateModule(
            string subjectOfferingId,
            string title,
            string description,
            string? fileUrl = null)
            => new Module
            {
                ModuleId = $"NULL-{System.Guid.NewGuid():N}",
                SubjectOfferingId = subjectOfferingId,
                Title = title,
                ModuleDescription = description,
                FileUrl = fileUrl,
                UploadDate = System.DateTime.Now,
            };

        public void UpdateModule(string moduleId, string title, string description, string? fileUrl) { }

        public void DeleteModule(string moduleId) { }
    }

    public class NullActivityDbService : IActivityDbService
    {
        public List<CourseActivity> GetCourseActivitiesForProfessor(int professorId)
            => new();

        public List<ActivityItem> GetActivitiesForOffering(string subjectOfferingId)
            => new();

        public ActivityItem CreateActivity(string subjectOfferingId, ActivityItem item)
        {
            item.ActivityId = $"NULL-{System.Guid.NewGuid():N}";
            return item;
        }

        public void UpdateActivity(ActivityItem item) { }

        public void DeleteActivity(string activityId) { }

        public void TogglePublish(string activityId, bool publish) { }

        public List<StudentSubmission> GetSubmissionsForActivity(string activityId)
            => new();

        public void SaveGrade(string submissionId, int score, string remarks) { }

        public void ReturnSubmission(string submissionId) { }

        public List<GradingCategory> GetCategoriesForOffering(string subjectOfferingId)
            => new();

        public List<Module> GetModulesForOffering(string subjectOfferingId)
            => new();
    }
}