using PUPAcadPortal.Models;
using PUPAcadPortal.PortalContents.Instructor.LMS.Course;
using System.Collections.Generic;

namespace PUPAcadPortal.Services
{
    public interface IActivityDbService
    {
        //  Dashboard 
        List<CourseActivity> GetCourseActivitiesForProfessor(int professorId);

        //  Activity list 
        List<ActivityItem> GetActivitiesForOffering(string subjectOfferingId);

        //  CRUD 
        ActivityItem CreateActivity(string subjectOfferingId, ActivityItem item);

        void UpdateActivity(ActivityItem item);

        void DeleteActivity(string activityId);

        void TogglePublish(string activityId, bool publish);

        //  Submissions 
        List<StudentSubmission> GetSubmissionsForActivity(string activityId);

        void SaveGrade(string submissionId, int score, string remarks);

        void ReturnSubmission(string submissionId);

        //  Dropdowns 
        List<GradingCategory> GetCategoriesForOffering(string subjectOfferingId);
        List<Module> GetModulesForOffering(string subjectOfferingId);
    }
}