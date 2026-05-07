using System.Windows.Forms;

namespace PUPAcadPortal
{
    /// <summary>
    /// StudentLMSHost — self-contained router for the student LMS activity flow:
    ///   StudentActivityDashboard → StudentActivityList → StudentActivitySubmit
    ///
    /// Drop this into any Panel inside StudentPortal to get the full student
    /// activity workflow in one widget.
    /// </summary>
    public partial class StudentLMSHost : UserControl
    {
        // ── State ─────────────────────────────────────────────────────────────
        private Control _currentView;

        // ── Constructor ───────────────────────────────────────────────────────
        public StudentLMSHost()
        {
            InitializeComponent();
            ShowDashboard();
        }

        // ── Navigation ────────────────────────────────────────────────────────

        /// <summary>Shows the student course/activity dashboard.</summary>
        public void ShowDashboard()
        {
            var dash = new StudentActivityDashboard();
            dash.Dock = DockStyle.Fill;

            dash.OnOpenCourse += course =>
            {
                var list = new StudentActivityList(course);
                list.Dock        = DockStyle.Fill;
                list.OnBack      += ShowDashboard;
                list.OnOpenActivity += activity => ShowSubmit(activity, course, list);
                SwapView(list);
            };

            SwapView(dash);
        }

        /// <summary>
        /// Alias for ShowDashboard() — called from StudentPortal when the
        /// Activities sidebar button is clicked so the student always lands
        /// back on the course list.
        /// </summary>
        public void ShowCourseDashboard() => ShowDashboard();

        // ── Private helpers ───────────────────────────────────────────────────

        private void ShowSubmit(StudentActivityItem activity, StudentCourse course, StudentActivityList list)
        {
            var submit = new StudentActivitySubmit(activity, course);
            submit.Dock   = DockStyle.Fill;
            submit.OnBack += () =>
            {
                var updatedList = new StudentActivityList(course);
                updatedList.Dock        = DockStyle.Fill;
                updatedList.OnBack      += ShowDashboard;
                updatedList.OnOpenActivity += act => ShowSubmit(act, course, updatedList);
                SwapView(updatedList);
            };
            SwapView(submit);
        }

        private void SwapView(Control next)
        {
            SuspendLayout();

            if (_currentView != null)
            {
                Controls.Remove(_currentView);
                _currentView.Dispose();
            }

            _currentView = next;
            Controls.Add(next);
            next.BringToFront();

            ResumeLayout(true);
        }
    }
}