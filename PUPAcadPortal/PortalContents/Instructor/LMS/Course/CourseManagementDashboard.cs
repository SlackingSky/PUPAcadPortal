using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using PUPAcadPortal.PortalContents.Instructor.LMS.Course;
using PUPAcadPortal.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class CourseManagementDashboard : UserControl
    {
        //  Events 
        /// <summary>Raised when the user clicks "Open Course" to navigate into a course.</summary>
        public event Action<CourseDto>? OnOpenCourse;

        //  Services / state 
        private readonly ICourseDbService _courseSvc;
        private readonly IActivityDbService _activitySvc;
        private readonly IModuleDbService _moduleSvc;
        private readonly int _professorId;

        private List<CourseDto> _courses = new();
        private string _searchTerm = string.Empty;
        private System.Windows.Forms.Timer _searchTimer = null!;
        private NullCourseDbService nullCourseDbService;
        private NullActivityDbService nullActivityDbService;
        private NullModuleDbService nullModuleDbService;

        // ── DbContext factory (mirrors ActivityDashboard pattern) ─────────────
        private static AppDbContext CreateContext() => new AppDbContext();

        //  Full DB-backed constructor 
        public CourseManagementDashboard(
            int professorId,
            ICourseDbService courseSvc,
            IActivityDbService activitySvc,
            IModuleDbService moduleSvc)
        {
            _professorId = professorId;
            _courseSvc = courseSvc ?? new NullCourseDbService();
            _activitySvc = activitySvc;
            _moduleSvc = moduleSvc;

            InitializeComponent();
            SetupSearchDebounce();

            flpCourseCards.SizeChanged += (s, e) => ResizeCards();
            this.Load += (s, e) => { LoadFromDb(); ResizeCards(); };
        }

        /// <summary>Convenience constructor — creates all real services automatically.</summary>
        public CourseManagementDashboard(int professorId)
            : this(UserSession.ProfessorID ?? 0,
                   new CourseDbService(CreateContext),
                   new ActivityDbService(CreateContext),
                   new ModuleDbService(CreateContext))
        { }

        /// <summary>WinForms designer-only no-arg constructor (never shows real data).</summary>
        public CourseManagementDashboard()
            : this(0,
                   new NullCourseDbService(),
                   new NullActivityDbService(),
                   new NullModuleDbService())
        { }

        public CourseManagementDashboard(int professorId, NullCourseDbService nullCourseDbService, NullActivityDbService nullActivityDbService, NullModuleDbService nullModuleDbService) : this(professorId)
        {
            this.nullCourseDbService = nullCourseDbService;
            this.nullActivityDbService = nullActivityDbService;
            this.nullModuleDbService = nullModuleDbService;
        }

        /// <summary>
        /// 3-argument constructor: professor ID + course service + activity service.
        /// Module service defaults to a real <see cref="ModuleDbService"/> instance.
        /// Resolves CS1729 when callers pass only three arguments.
        /// </summary>
        public CourseManagementDashboard(
            int professorId,
            ICourseDbService courseSvc,
            IActivityDbService activitySvc)
            : this(professorId,
                   courseSvc,
                   activitySvc,
                   new ModuleDbService(CreateContext))
        { }

        //  Data loading 
        private void LoadFromDb()
        {
            try
            {
                _courses = _courseSvc.GetCoursesForProfessor(_professorId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to load courses:\n{ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _courses = new List<CourseDto>();
            }

            RefreshDashboard();
        }

        //  Search debounce 
        private void SetupSearchDebounce()
        {
            _searchTimer = new System.Windows.Forms.Timer { Interval = 220 };
            _searchTimer.Tick += (s, e) => { _searchTimer.Stop(); RefreshDashboard(); };
        }

        private void TxtSearchCourse_TextChanged(object? sender, EventArgs e)
        {
            _searchTerm = txtSearchCourse.Text;
            _searchTimer.Stop();
            _searchTimer.Start();
        }

        //  Dashboard refresh 
        private void RefreshDashboard()
        {
            flpCourseCards.SuspendLayout();
            flpCourseCards.Controls.Clear();

            var filtered = _courses.FindAll(c =>
                string.IsNullOrEmpty(_searchTerm)
                || c.SubjectName.IndexOf(_searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                || c.SubjectCode.IndexOf(_searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                || c.Section.IndexOf(_searchTerm, StringComparison.OrdinalIgnoreCase) >= 0);

            if (filtered.Count == 0)
                flpCourseCards.Controls.Add(BuildEmptyState());
            else
                foreach (var course in filtered)
                    flpCourseCards.Controls.Add(BuildCourseCard(course));

            UpdateStats();
            flpCourseCards.ResumeLayout();
            ResizeCards();
        }

        // ── Stats 
        private void UpdateStats()
        {
            lblTotalCourses.Text = _courses.Count.ToString();
            lblTotalActivities.Text = _courses.Sum(c => c.ActivityCount).ToString();
            lblTotalStudents.Text = _courses.Sum(c => c.EnrolledCount).ToString();
            lblTotalPending.Text = _courses.Sum(c => c.PendingSubmissions).ToString();
        }

        // ── Card sizing 
        private void ResizeCards()
        {
            if (flpCourseCards.Controls.Count == 0) return;
            const int columns = 3, margin = 10;
            int available = flpCourseCards.ClientSize.Width
                          - flpCourseCards.Padding.Horizontal
                          - (margin * 2 * columns);
            int cardW = Math.Max(320, available / columns);

            foreach (Control ctrl in flpCourseCards.Controls)
            {
                if (ctrl is not Panel card) continue;
                card.Width = cardW;

                foreach (Control c in card.Controls)
                {
                    if (c.Tag?.ToString() == "HEADER")
                    {
                        c.Width = cardW;
                        foreach (Control h in c.Controls)
                            if (h.Tag?.ToString() == "COURSE_NAME") h.Width = cardW - 24;
                    }
                    else if (c.Tag?.ToString() == "STATS") c.Width = cardW - 24;
                    else if (c is buttonRounded btn && btn.Tag?.ToString() == "OPEN_BTN")
                        btn.Location = new Point(cardW - btn.Width - 12, btn.Location.Y);
                }
                card.Invalidate();
            }
        }

        // ── Card builder 
        // ── Card builder 
        private Panel BuildCourseCard(CourseDto course)
        {
            const int cardH = 220, statsY = 92, bottomY = 160;
            int cardW = 430; // resized later

            var card = new Panel
            {
                Width = cardW,
                Height = cardH,
                BackColor = Color.White,
                Margin = new Padding(10),
                Tag = course.SubjectOfferingId
            };
            card.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(218, 218, 225));
                e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            // Header strip (Height is 64)
            var hdr = new Panel
            {
                Width = cardW,
                Height = 64,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(128, 0, 0),
                Tag = "HEADER"
            };
            hdr.Controls.Add(new Label
            {
                Text = $"{course.SubjectCode} — {course.SubjectName}",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                Location = new Point(12, 8),
                Width = cardW - 24,
                Height = 26,
                AutoEllipsis = true,
                Tag = "COURSE_NAME"
            });
            hdr.Controls.Add(new Label
            {
                Text = $"{course.Section}  ·  {course.AcademicPeriod}",
                ForeColor = Color.FromArgb(230, 185, 185),
                Font = new Font("Segoe UI", 8.5F),
                Location = new Point(12, 36),
                Width = cardW - 24,
                Height = 18,
                AutoEllipsis = true
            });
            card.Controls.Add(hdr);

            // Status badge
            Color statusColor = course.Status switch
            {
                "Active" => Color.FromArgb(26, 130, 60),
                "Ongoing" => Color.FromArgb(63, 81, 181),
                "Completed" => Color.FromArgb(100, 100, 110),
                "Archived" => Color.FromArgb(150, 80, 0),
                _ => Color.Gray
            };
            card.Controls.Add(new Label
            {
                Text = course.Status,
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                BackColor = statusColor,
                ForeColor = Color.White,
                Location = new Point(12, 70),
                Size = new Size(72, 16),
                TextAlign = ContentAlignment.MiddleCenter
            });

            // Stats strip
            var pnlStats = new Panel
            {
                Location = new Point(12, statsY),
                Size = new Size(cardW - 24, 54),
                BackColor = Color.FromArgb(248, 248, 251),
                Tag = "STATS"
            };
            pnlStats.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(235, 235, 240));
                e.Graphics.DrawRectangle(pen, 0, 0, pnlStats.Width - 1, pnlStats.Height - 1);
            };

            string[] vals = { course.ActivityCount.ToString(), course.EnrolledCount.ToString(), course.PendingSubmissions.ToString(), course.CheckedSubmissions.ToString() };
            string[] labels = { "Activities", "Students", "Pending", "Checked" };
            Color[] colors = { Color.FromArgb(63, 81, 181), Color.FromArgb(0, 150, 136), Color.FromArgb(211, 84, 0), Color.FromArgb(46, 160, 67) };

            for (int i = 0; i < 4; i++)
            {
                int cw = (cardW - 24) / 4, x = i * cw;
                pnlStats.Controls.Add(new Label { Text = vals[i], Font = new Font("Segoe UI", 15F, FontStyle.Bold), ForeColor = colors[i], Location = new Point(x, 2), Width = cw, Height = 28, TextAlign = ContentAlignment.MiddleCenter });
                pnlStats.Controls.Add(new Label { Text = labels[i], Font = new Font("Segoe UI", 7F), ForeColor = Color.Gray, Location = new Point(x, 32), Width = cw, Height = 15, TextAlign = ContentAlignment.MiddleCenter });
            }
            card.Controls.Add(pnlStats);

            // Instructor label
            card.Controls.Add(new Label
            {
                Text = $"👤 {course.InstructorName}",
                Font = new Font("Segoe UI", 7.5F),
                ForeColor = Color.FromArgb(100, 100, 110),
                Location = new Point(14, bottomY + 4),
                AutoSize = true
            });

            // Schedule label
            if (!string.IsNullOrWhiteSpace(course.Schedule))
                card.Controls.Add(new Label
                {
                    Text = $"🕐 {course.Schedule}",
                    Font = new Font("Segoe UI", 7.5F),
                    ForeColor = Color.FromArgb(100, 100, 110),
                    Location = new Point(14, bottomY + 20),
                    Width = cardW - 160,
                    AutoEllipsis = true
                });

            // Open Course button only (Edit/Delete removed per design update)
            var btnOpen = new buttonRounded
            {
                Text = "Open Course",
                Size = new Size(130, 32),
                Location = new Point(cardW - 142, bottomY + 2),
                BackColor = Color.FromArgb(128, 0, 0),
                ForeColor = Color.White,
                BorderRadius = 14,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Tag = "OPEN_BTN"
            };
            btnOpen.Click += (s, e) => OpenCourse(course);
            card.Controls.Add(btnOpen);

            return card;
        }

        //  Empty state 
        private Panel BuildEmptyState()
        {
            int w = Math.Max(700, flpCourseCards.ClientSize.Width - 40);
            var pnl = new Panel { Width = w, Height = 200, BackColor = Color.FromArgb(252, 252, 255) };
            pnl.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(218, 218, 228), 1.5f);
                e.Graphics.DrawRectangle(pen, 1, 1, pnl.Width - 3, pnl.Height - 3);
            };
            pnl.Controls.Add(new Label
            {
                Text = string.IsNullOrEmpty(_searchTerm)
                                ? "📚  No courses found"
                                : $"📚  No courses matching \"{_searchTerm}\"",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(160, 160, 170),
                AutoSize = false,
                Width = w,
                Height = 60,
                TextAlign = ContentAlignment.BottomCenter,
                Location = new Point(0, 50)
            });
            pnl.Controls.Add(new Label
            {
                Text = string.IsNullOrEmpty(_searchTerm)
                                ? "Use the \"+ Create Course\" button above to add your first course."
                                : "Try a different search term.",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(180, 180, 190),
                AutoSize = false,
                Width = w,
                Height = 24,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 112)
            });
            return pnl;
        }

        private void EditCourse(CourseDto course)
        {
            using var dlg = new CourseFormDialog(_professorId, course, _courseSvc);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            // Replace in-memory entry with refreshed data from DB
            try
            {
                var fresh = _courseSvc.GetCourseById(dlg.Result.SubjectOfferingId);
                int idx = _courses.FindIndex(c => c.SubjectOfferingId == course.SubjectOfferingId);
                if (idx >= 0) _courses[idx] = fresh ?? dlg.Result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Refresh after edit failed:\n{ex.Message}",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                int idx = _courses.FindIndex(c => c.SubjectOfferingId == course.SubjectOfferingId);
                if (idx >= 0) _courses[idx] = dlg.Result;
            }

            RefreshDashboard();
        }

        private void DeleteCourse(CourseDto course)
        {
            var confirm = MessageBox.Show(
                $"Delete course \"{course.SubjectCode} — {course.SubjectName}\" ({course.Section})?\n\n" +
                "This will also remove all associated activities, modules, and grading categories.\n" +
                "This action CANNOT be undone.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                _courseSvc.DeleteCourse(course.SubjectOfferingId);
                _courses.RemoveAll(c => c.SubjectOfferingId == course.SubjectOfferingId);
                RefreshDashboard();
                MessageBox.Show(
                    $"Course \"{course.SubjectCode}\" deleted successfully.",
                    "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Delete failed:\n{ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //  Navigation 
        private void OpenCourse(CourseDto courseDto)
        {
            // Raise event if a parent has subscribed (host-level navigation)
            if (OnOpenCourse != null) { OnOpenCourse.Invoke(courseDto); return; }

            // Self-contained navigation: bridge DTO → CourseActivity then open ClassFilesPage
            Control container = this.Parent;
            if (container == null) return;

            var courseActivity = DtoToCourseActivity(courseDto);

            var filesPage = new ClassFilesPage(courseActivity, _moduleSvc);
            filesPage.Dock = DockStyle.Fill;

            filesPage.OnBack += () =>
            {
                Control c = filesPage.Parent ?? container;
                c.Controls.Remove(filesPage);
                filesPage.Dispose();
                c.Controls.Add(this);
                this.BringToFront();
                // Refresh stats when returning from course
                LoadFromDb();
            };

            filesPage.OnOpenActivities += ca =>
            {
                Control fc = filesPage.Parent ?? container;
                var mgmt = new AssignmentManagement(ca, _activitySvc);
                mgmt.Dock = DockStyle.Fill;
                mgmt.OnBack += () =>
                {
                    Control mc = mgmt.Parent ?? fc;
                    mc.Controls.Remove(mgmt);
                    mgmt.Dispose();
                    mc.Controls.Add(filesPage);
                    filesPage.BringToFront();
                };
                fc.Controls.Remove(filesPage);
                fc.Controls.Add(mgmt);
                mgmt.BringToFront();
            };

            container.Controls.Remove(this);
            container.Controls.Add(filesPage);
            filesPage.BringToFront();
        }

        /// <summary>
        /// Converts a <see cref="CourseDto"/> into the <see cref="CourseActivity"/> shape that
        /// ClassFilesPage / AssignmentManagement expect.
        /// </summary>
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
                Activities = new List<ActivityItem>(),
            };
    }
}