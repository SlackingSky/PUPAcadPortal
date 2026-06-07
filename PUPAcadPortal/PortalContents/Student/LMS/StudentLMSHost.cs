using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using PUPAcadPortal.PortalContents.Student.LMS.Course;
using PUPAcadPortal.Services;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Student.LMS
{
    /// <summary>
    /// Wrapper shown under the student's "Courses" sidebar button.
    /// Delegates all navigation to <see cref="StudentActivityNavigator"/>.
    /// </summary>
    public sealed class StudentLMSHost : UserControl
    {
        private StudentActivityNavigator? _navigator;

        // ── DB context factory ────────────────────────────────────────────────
        private static AppDbContext CreateContext() => new AppDbContext();

        public StudentLMSHost()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(245, 245, 245);
        }

        /// <summary>
        /// Resets the content to the course-dashboard view.
        /// Called by <c>StudentPortal.btnCourses_Click</c> each time
        /// the student presses the sidebar button.
        /// </summary>
        public void ShowCourseDashboard()
        {
            // Dispose the old navigator so it releases its DB context
            if (_navigator != null)
            {
                Controls.Remove(_navigator);
                _navigator.Dispose();
                _navigator = null;
            }

            int studentId = UserSession.StudentID ?? 0;
            string fullName = UserSession.FullName ?? "Student";

            IStudentCourseDbService svc = studentId > 0
                ? new StudentCourseDbService(CreateContext)
                : (IStudentCourseDbService)new NullStudentCourseDbService();

            _navigator = new StudentActivityNavigator(studentId, svc, fullName)
            {
                Dock = DockStyle.Fill
            };

            Controls.Add(_navigator);
            _navigator.BringToFront();
        }
    }
}