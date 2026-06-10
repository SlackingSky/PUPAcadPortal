using System;
using System.Windows.Forms;
using PUPAcadPortal.Models;
using PUPAcadPortal.Services;
using PUPAcadPortal.Data;

namespace PUPAcadPortal.PortalContents.Student.LMS.Course
{
    public sealed partial class StudentActivityNavigator : UserControl
    {
        private readonly int _studentId;
        private readonly string _studentName;
        private readonly IStudentCourseDbService _courseSvc;
        private readonly IModuleDbService _moduleSvc;

        public StudentActivityNavigator(int studentId, IStudentCourseDbService courseSvc, string studentName)
        {
            _studentId = studentId;
            _courseSvc = courseSvc ?? new NullStudentCourseDbService();
            _studentName = studentName;
            _moduleSvc = studentId > 0
                ? new ModuleDbService(() => new AppDbContext())
                : new NullModuleDbService();
            this.Dock = DockStyle.Fill;

            ShowDashboard();
        }

        private void LoadView(UserControl page)
        {
            for (int i = this.Controls.Count - 1; i >= 0; i--)
            {
                Control ctrl = this.Controls[i];
                this.Controls.RemoveAt(i);
                ctrl.Dispose();
            }

            page.Dock = DockStyle.Fill;
            this.Controls.Add(page);
        }

        private void ShowDashboard()
        {
            var dashboard = new StudentActivityDashboard(_studentId, _courseSvc, _studentName);

            dashboard.OnOpenCourse += (course) => ShowClassFiles(course);

            LoadView(dashboard);
        }

        private void ShowClassFiles(StudentCourse course)
        {
            var classFiles = new StudentClassFilesPage(course, _moduleSvc, _studentId, _courseSvc);

            classFiles.OnBack += () => ShowDashboard();
            classFiles.OnOpenActivities += (c) => ShowActivityList(c);

            LoadView(classFiles);
        }

        private void ShowActivityList(StudentCourse course)
        {
            var activityList = new StudentActivityList(course, _studentId, _courseSvc);

            activityList.OnBack += () => ShowClassFiles(course);

            activityList.OnOpenActivity += (activityItem) => ShowActivityAssessment(course, activityItem);

            LoadView(activityList);
        }

        private void ShowActivityAssessment(StudentCourse course, StudentActivityItem activity)
        {
            var submitPage = new StudentActivitySubmit(activity, course, _studentId, _courseSvc);

            submitPage.OnBack += () => ShowActivityList(course);

            LoadView(submitPage);
        }
    }
}