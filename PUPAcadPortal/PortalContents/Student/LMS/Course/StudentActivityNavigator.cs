using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using PUPAcadPortal.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Student.LMS.Course
{
    public sealed partial class StudentActivityNavigator : UserControl
    {
        // --- Fields ---
        private readonly int _studentId;
        private readonly string _studentName;
        private readonly IStudentCourseDbService _courseSvc;
        private readonly IModuleDbService _moduleSvc;

        private Control _current;

        // --- Constructors ---

        // Parameterless constructor for WinForms Designer and default Session init
        public StudentActivityNavigator()
            : this(
                UserSession.StudentID ?? 0,
                new StudentCourseDbService(CreateContext),
                UserSession.FullName)
        {
        }

        public StudentActivityNavigator(
            int studentId,
            IStudentCourseDbService courseSvc,
            string studentName)
        {
            _studentId = studentId;
            _studentName = studentName;
            _courseSvc = courseSvc ?? new NullStudentCourseDbService();
            _moduleSvc = studentId > 0
                ? new ModuleDbService(CreateContext)
                : new NullModuleDbService();

            InitializeComponent();
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(245, 245, 245);

            ShowDashboard();
        }

        private static AppDbContext CreateContext() => new AppDbContext();

        // --- Navigation Flow ---

        private void ShowDashboard()
        {
            var dashboard = new StudentActivityDashboard(
                _studentId,
                _courseSvc,
                _studentName)
            { Dock = DockStyle.Fill };

            dashboard.OnOpenCourse += ShowClassFiles;

            SwapView(dashboard);
        }

        private void ShowClassFiles(StudentCourse course)
        {
            var classFiles = new StudentClassFilesPage(
                course,
                _moduleSvc,
                _studentId,
                _courseSvc)
            { Dock = DockStyle.Fill };

            classFiles.OnBack += ShowDashboard;
            classFiles.OnOpenActivities += ShowActivityList;

            SwapView(classFiles);
        }

        private void ShowActivityList(StudentCourse course)
        {
            var activityList = new StudentActivityList(
                course,
                _studentId,
                _courseSvc)
            { Dock = DockStyle.Fill };

            activityList.OnBack += () => ShowClassFiles(course);
            activityList.OnOpenActivity += (activityItem) => ShowActivityAssessment(course, activityItem);

            SwapView(activityList);
        }

        private void ShowActivityAssessment(StudentCourse course, StudentActivityItem activity)
        {
            var submitPage = new StudentActivitySubmit(
                activity,
                course,
                _studentId,
                _courseSvc)
            { Dock = DockStyle.Fill };

            submitPage.OnBack += () => ShowActivityList(course);

            SwapView(submitPage);
        }

        // --- View Management ---

        private void SwapView(Control next)
        {
            SuspendLayout();

            if (_current != null)
            {
                Controls.Remove(_current);
                _current.Dispose();
            }

            _current = next;
            Controls.Add(next);
            next.BringToFront();

            ResumeLayout(true);
        }
    }
}