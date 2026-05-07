using System.Windows.Forms;

namespace PUPAcadPortal
{
    /// <summary>
    /// LMSActivityHost — a UserControl that acts as the container/router for
    /// the instructor-side activity management chain:
    ///
    ///   ActivityDashboard → AssignmentManagement → SubmissionList → GradingInterface
    ///
    /// Drop this control into any Panel inside InstructorPortal (or any Form)
    /// to get the full LMS activity workflow in a self-contained widget.
    /// </summary>
    public partial class LMSActivityHost : UserControl
    {
        // ── State ─────────────────────────────────────────────────────────────
        private Control _currentView;   // whichever UserControl is showing now

        // ── Constructor ───────────────────────────────────────────────────────
        public LMSActivityHost()
        {
            InitializeComponent();
            ShowDashboard();             // default first screen
        }

        // ── Navigation ────────────────────────────────────────────────────────

        /// <summary>Shows the top-level course/activity dashboard.</summary>
        public void ShowDashboard()
        {
            var dashboard = new ActivityDashboard();
            dashboard.Dock = DockStyle.Fill;

            // ActivityDashboard navigates to AssignmentManagement internally via
            // BtnViewCourse_Click, but we need to intercept the container swap so
            // it uses *this* control as the host.  We do that by monkey-patching
            // after construction through the public navigation chain below.

            SwapView(dashboard);
        }

        // ── Public navigation entry-points (called by InstructorPortal) ───────

        /// <summary>
        /// Navigate directly into a specific course's assignment list.
        /// Useful if you want to deep-link from another part of the portal.
        /// </summary>
        public void OpenCourse(CourseActivity course)
        {
            var mgmt = new AssignmentManagement(course);
            mgmt.Dock = DockStyle.Fill;
            mgmt.OnBack += ShowDashboard;
            SwapView(mgmt);
        }

        // ── Private helpers ───────────────────────────────────────────────────

        /// <summary>Replaces the currently displayed view with <paramref name="next"/>.</summary>
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