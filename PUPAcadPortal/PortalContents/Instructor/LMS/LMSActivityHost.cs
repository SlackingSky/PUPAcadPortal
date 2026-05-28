using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class LMSActivityHost : UserControl
    {
        private Control _currentView;   

        public LMSActivityHost()
        {
            InitializeComponent();// default first screen
        }

        public void ShowDashboard()
        {
            var dashboard = new ActivityDashboard();
            dashboard.Dock = DockStyle.Fill;

            SwapView(dashboard);
        }

        public void OpenCourse(CourseActivity course)
        {
            var mgmt = new AssignmentManagement(course);
            mgmt.Dock = DockStyle.Fill;
            mgmt.OnBack += ShowDashboard;
            SwapView(mgmt);
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