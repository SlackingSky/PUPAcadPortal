using System.Collections.Generic;
using CourseStudentActivityItem = PUPAcadPortal.PortalContents.Student.LMS.Course.StudentActivityItem;
using CourseStudentCourse = PUPAcadPortal.PortalContents.Student.LMS.Course.StudentCourse;

namespace PUPAcadPortal.Services
{
    public interface IStudentCourseDbService
    {
        List<CourseStudentCourse> GetCoursesForStudent(int studentId);

        List<CourseStudentActivityItem> GetActivitiesForStudentOffering(
            string subjectOfferingId,
            int studentId);

        CourseStudentActivityItem SubmitActivity(
            int studentId,
            CourseStudentActivityItem item);
    }
}
