using System;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    /// <summary>
    /// Top-level host that owns the navigation stack for the entire LMS module.
    /// All view swaps happen here; child controls never reference each other directly.
    /// </summary>
    public partial class LMSActivityHost : UserControl
    {
        private Control _currentView;

        public LMSActivityHost()
        {
            InitializeComponent();
            ShowDashboard();
        }

        // ── Public entry-points ───────────────────────────────────────────────

        public void ShowDashboard()
        {
            var dashboard = new ActivityDashboard();
            dashboard.Dock = DockStyle.Fill;
            // Dashboard tells host to open a course
            dashboard.OnOpenCourse += course => OpenCourse(course);
            SwapView(dashboard);
        }

        public void OpenCourse(CourseActivity course)
        {
            var mgmt = new AssignmentManagement(course);
            mgmt.Dock = DockStyle.Fill;
            mgmt.OnBack += ShowDashboard;
            SwapView(mgmt);
        }

        // ── Internal swap ────────────────────────────────────────────────────

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