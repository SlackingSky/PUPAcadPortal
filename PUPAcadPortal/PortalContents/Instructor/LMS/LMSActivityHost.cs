using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using PUPAcadPortal.PortalContents.Instructor.LMS;
using PUPAcadPortal.Services;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    /// <summary>
    /// Wrapper UserControl hosted inside ProfessorPortal's main content panel.
    /// Exposes <see cref="ShowDashboard"/> so the portal can (re)load the
    /// Activity Dashboard at any time.
    /// </summary>
    public partial class LMSActivityHost : UserControl
    {
        private readonly int _professorId;
        private readonly ICourseDbService _courseSvc;
        private readonly IActivityDbService _activitySvc;
        private readonly IModuleDbService _moduleSvc;
        private UserControl? _currentView;

        // ── DB context factory ────────────────────────────────────────────────
        private static AppDbContext CreateContext() => new AppDbContext();

        // ── Default constructor — reads professor ID from the live session ────
        public LMSActivityHost()
            : this(
                UserSession.ProfessorID ?? 0,
                new CourseDbService(CreateContext),
                new ActivityDbService(CreateContext),
                new ModuleDbService(CreateContext))
        {
        }

        // ── DI constructor (4-arg, full services) ─────────────────────────────
        public LMSActivityHost(
            int professorId,
            ICourseDbService courseSvc,
            IActivityDbService activitySvc,
            IModuleDbService moduleSvc)
        {
            _professorId = professorId;
            _courseSvc = courseSvc ?? new NullCourseDbService();
            _activitySvc = activitySvc ?? new NullActivityDbService();
            _moduleSvc = moduleSvc ?? new NullModuleDbService();

            InitializeComponent();

            // Show the activity dashboard immediately on first load
            ShowDashboard();
        }

        // ── Public navigation entry-point ─────────────────────────────────────

        /// <summary>
        /// (Re)loads the Activity Dashboard, discarding any previously shown view.
        /// </summary>
        public void ShowDashboard()
        {
            var dashboard = new ActivityDashboard(
                _professorId,
                _courseSvc,
                _activitySvc,
                _moduleSvc);
            dashboard.Dock = DockStyle.Fill;
            SwapView(dashboard);
        }

        // ── Internal view-swap helper ─────────────────────────────────────────
        private void SwapView(UserControl next)
        {
            SuspendLayout();

            if (_currentView != null)
            {
                Controls.Remove(_currentView);
                _currentView.Dispose();
                _currentView = null;
            }

            _currentView = next;
            Controls.Add(next);
            next.BringToFront();

            ResumeLayout(true);
        }
    }
}