using System;
using System.Collections.Generic;
using CourseStudentActivityItem = PUPAcadPortal.PortalContents.Student.LMS.Course.StudentActivityItem;
using CourseStudentCourse = PUPAcadPortal.PortalContents.Student.LMS.Course.StudentCourse;

namespace PUPAcadPortal.Services
{
    public class NullStudentCourseDbService : IStudentCourseDbService
    {
        public List<CourseStudentCourse> GetCoursesForStudent(int studentId)
            => new List<CourseStudentCourse>();

        public List<CourseStudentActivityItem> GetActivitiesForStudentOffering(
            string subjectOfferingId,
            int studentId)
            => new List<CourseStudentActivityItem>();

        public CourseStudentActivityItem SubmitActivity(
            int studentId,
            CourseStudentActivityItem item)
        {
            // Null stub: set plausible status without touching DB
            item.SubmissionStatus = item.Deadline < DateTime.Now ? "Late" : "Submitted";
            item.SubmittedAt = DateTime.Now;
            return item;
        }
    }
}