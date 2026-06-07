using PUPAcadPortal.PortalContents.Student.LMS.Course;
using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using PUPAcadPortal.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Student.LMS.Course
{
    public partial class StudentActivityNavigator : UserControl
    {
        // Keep references so we can pass state back on nav
        private StudentActivityDashboard _dashboard;
        private StudentActivityList _list;
        private StudentActivitySubmit _submit;
        private StudentCourse _currentCourse;
        private readonly int _studentId;
        private readonly IStudentCourseDbService _courseSvc;
        private readonly string _studentName;

        private Control _current;

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
            _courseSvc = courseSvc ?? new NullStudentCourseDbService();
            _studentName = studentName;

            InitializeComponent();
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(245, 245, 245);
            ShowDashboard();
        }

        private static AppDbContext CreateContext() => new AppDbContext();

        //  Navigation 

        private void ShowDashboard()
        {
            _dashboard = new StudentActivityDashboard(
                _studentId,
                _courseSvc,
                _studentName)
            { Dock = DockStyle.Fill };
            _dashboard.OnOpenCourse += ShowList;
            SwapView(_dashboard);
        }

        private void ShowList(StudentCourse course)
        {
            _currentCourse = course;

            _list = new StudentActivityList(course, _studentId, _courseSvc)
            { Dock = DockStyle.Fill };
            _list.OnBack += ShowDashboard;
            _list.OnOpenActivity += ShowSubmit;
            SwapView(_list);
        }

        private void ShowSubmit(StudentActivityItem activity)
        {
            _submit = new StudentActivitySubmit(
                activity,
                _currentCourse,
                _studentId,
                _courseSvc)
            { Dock = DockStyle.Fill };
            _submit.OnBack += () => ShowList(_currentCourse);
            SwapView(_submit);
        }

        //  View swap 

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
