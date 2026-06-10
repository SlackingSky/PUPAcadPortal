using PUPAcadPortal.Models;
using PUPAcadPortal.PortalContents.Instructor.LMS.Course;
using PUPAcadPortal.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Instructor.LMS
{
    public sealed class ActivityDashboard : UserControl
    {
        private readonly int _professorId;
        private readonly ICourseDbService _courseSvc;
        private readonly IActivityDbService _activitySvc;
        private readonly IModuleDbService _moduleSvc;
        private Control? _current;

        //  DB context factory (mirrors CourseManagementDashboard pattern) 
        private static AppDbContext CreateContext() => new AppDbContext();

        // ── DB-backed constructor 
        public ActivityDashboard(int professorId)
            : this(professorId,
                   new CourseDbService(CreateContext),
                   new ActivityDbService(CreateContext),
                   new ModuleDbService(CreateContext))
        { }

        /// <summary>WinForms designer / no-session fallback.</summary>
        public ActivityDashboard()
            : this(0,
                   new NullCourseDbService(),
                   new NullActivityDbService(),
                   new NullModuleDbService())
        { }

        //  Private shared constructor 
        public ActivityDashboard(
            int professorId,
            ICourseDbService courseSvc,
            IActivityDbService activitySvc,
            IModuleDbService moduleSvc)
        {
            _professorId = professorId;
            _courseSvc = courseSvc;
            _activitySvc = activitySvc;
            _moduleSvc = moduleSvc;

            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(245, 245, 248);

            ShowDashboard();
        }
        private void ShowDashboard()
        {
            var dashboard = new CourseManagementDashboard(
                _professorId,
                _courseSvc,
                _activitySvc,
                _moduleSvc)
            {
                Dock = DockStyle.Fill
            };

            // When the professor clicks "Open Course" on a card, navigate into it
            dashboard.OnOpenCourse += courseDto => NavigateIntoCourse(courseDto);

            SwapView(dashboard);
        }

        private void NavigateIntoCourse(CourseDto courseDto)
        {
            var courseActivity = DtoToCourseActivity(courseDto);

            var filesPage = new ClassFilesPage(courseActivity, _moduleSvc)
            {
                Dock = DockStyle.Fill
            };

            filesPage.OnBack += ShowDashboard;

            filesPage.OnOpenActivities += ca =>
            {
                var mgmt = new AssignmentManagement(ca, _activitySvc)
                {
                    Dock = DockStyle.Fill
                };

                mgmt.OnBack += () => NavigateIntoCourse(courseDto);   // back to course files

                SwapView(mgmt);
            };

            SwapView(filesPage);
        }

        // ── View swapper 

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
        private static CourseActivity DtoToCourseActivity(CourseDto dto) =>
            new CourseActivity
            {
                SubjectOfferingId = dto.SubjectOfferingId,
                CourseName = dto.SubjectName,
                CourseCode = dto.SubjectCode,
                Section = dto.Section,
                InstructorName = dto.InstructorName,
                Schedule = dto.Schedule,
                ActivityCount = dto.ActivityCount,
                TotalAssignments = dto.TotalAssignments,
                TotalQuizzes = dto.TotalQuizzes,
                PendingSubmissions = dto.PendingSubmissions,
                CheckedSubmissions = dto.CheckedSubmissions,
                Status = dto.Status,
                Activities = new System.Collections.Generic.List<ActivityItem>()
            };
    }
}