using PUPAcadPortal.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    /// <summary>
    /// Student-facing course dashboard.
    /// Shows only courses in which the student is enrolled, loaded exclusively from the database.
    /// Students have read-only access: no create, edit, or delete operations are available.
    /// No hardcoded, sample, or mock data exists in this control.
    /// </summary>
    public partial class StudentCourseDashboard : UserControl
    {
        // ── Events ────────────────────────────────────────────────────────────
        /// <summary>Raised when the student clicks "View Activities" on a course card.</summary>
        public event Action<CourseDto>? OnOpenCourse;

        // ── Services / state ──────────────────────────────────────────────────
        private readonly ICourseDbService _courseSvc;
        private readonly IStudentCourseDbService _activitySvc;
        private readonly int _studentId;

        private List<CourseDto> _courses = new();
        private string _searchTerm = string.Empty;
        private System.Windows.Forms.Timer _searchTimer = null!;

        // ── DbContext factory ─────────────────────────────────────────────────
        private static Models.AppDbContext CreateContext() => new Models.AppDbContext();

        // ── Full DB-backed constructor ─────────────────────────────────────────
        public StudentCourseDashboard(
            int studentId,
            ICourseDbService courseSvc,
            IStudentCourseDbService activitySvc)
        {
            _studentId = studentId;
            _courseSvc = courseSvc ?? new NullCourseDbService();
            _activitySvc = activitySvc ?? new NullStudentCourseDbService();

            InitializeComponent();
            SetupSearchDebounce();

            flpCourseCards.SizeChanged += (s, e) => ResizeCards();
            this.Load += (s, e) => { LoadFromDb(); ResizeCards(); };
        }

        /// <summary>Convenience constructor — creates real services automatically.</summary>
        public StudentCourseDashboard(int studentId)
            : this(studentId,
                   new CourseDbService(CreateContext),
                   new StudentCourseDbService(CreateContext))
        { }

        /// <summary>WinForms designer-only no-arg constructor.</summary>
        public StudentCourseDashboard()
            : this(0,
                   new NullCourseDbService(),
                   new NullStudentCourseDbService())
        { }

        // ── Data loading ──────────────────────────────────────────────────────
        private void LoadFromDb()
        {
            try
            {
                _courses = _courseSvc.GetCoursesForStudent(_studentId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to load your courses:\n{ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _courses = new List<CourseDto>();
            }

            RefreshDashboard();
        }

        // ── Search debounce ───────────────────────────────────────────────────
        private void SetupSearchDebounce()
        {
            _searchTimer = new System.Windows.Forms.Timer { Interval = 220 };
            _searchTimer.Tick += (s, e) => { _searchTimer.Stop(); RefreshDashboard(); };
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            _searchTerm = txtSearch.Text;
            _searchTimer.Stop();
            _searchTimer.Start();
        }

        // ── Dashboard refresh ─────────────────────────────────────────────────
        private void RefreshDashboard()
        {
            flpCourseCards.SuspendLayout();
            flpCourseCards.Controls.Clear();

            var filtered = _courses.FindAll(c =>
                string.IsNullOrEmpty(_searchTerm)
                || c.SubjectName.IndexOf(_searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                || c.SubjectCode.IndexOf(_searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                || c.Section.IndexOf(_searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                || c.InstructorName.IndexOf(_searchTerm, StringComparison.OrdinalIgnoreCase) >= 0);

            if (filtered.Count == 0)
                flpCourseCards.Controls.Add(BuildEmptyState());
            else
                foreach (var course in filtered)
                    flpCourseCards.Controls.Add(BuildCourseCard(course));

            UpdateStats();
            flpCourseCards.ResumeLayout();
            ResizeCards();
        }

        // ── Stats bar ─────────────────────────────────────────────────────────
        private void UpdateStats()
        {
            lblStatCoursesVal.Text = _courses.Count.ToString();
            lblStatPendingVal.Text = _courses.Sum(c => c.PendingCount).ToString();
            lblStatSubmittedVal.Text = _courses.Sum(c => c.SubmittedCount).ToString();
            lblStatOverdueVal.Text = _courses.Sum(c => c.OverdueCount).ToString();
        }

        // ── Card sizing ───────────────────────────────────────────────────────
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
                    else if (c is buttonRounded btn && btn.Tag?.ToString() == "VIEW_BTN")
                        btn.Location = new Point(cardW - 148, btn.Location.Y);
                }
                card.Invalidate();
            }
        }

        // ── Card builder ──────────────────────────────────────────────────────
        private Panel BuildCourseCard(CourseDto course)
        {
            const int cardH = 188, statsY = 72, bottomY = 148;
            int cardW = 430; // resized later

            var card = new Panel
            {
                Width = cardW,
                Height = cardH,
                BackColor = Color.White,
                Margin = new Padding(10),
            };
            card.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(218, 218, 225));
                e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            // ── Header strip ───────────────────────────────────────────────
            var hdr = new Panel
            {
                Width = cardW,
                Height = 62,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(128, 0, 0),
                Tag = "HEADER"
            };
            hdr.Controls.Add(new Label
            {
                Text = $"{course.SubjectCode} — {course.SubjectName}",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                Location = new Point(12, 7),
                Width = cardW - 24,
                Height = 26,
                AutoEllipsis = true,
                Tag = "COURSE_NAME"
            });
            hdr.Controls.Add(new Label
            {
                Text = $"Section {course.Section}  ·  {course.AcademicPeriod}",
                ForeColor = Color.FromArgb(230, 185, 185),
                Font = new Font("Segoe UI", 8.5F),
                Location = new Point(12, 36),
                Width = cardW - 24,
                Height = 18,
                AutoEllipsis = true
            });
            card.Controls.Add(hdr);

            // ── Stats strip ────────────────────────────────────────────────
            var pnlStats = new Panel
            {
                Location = new Point(12, statsY),
                Size = new Size(cardW - 24, 52),
                BackColor = Color.FromArgb(248, 248, 251),
                Tag = "STATS"
            };
            pnlStats.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(235, 235, 240));
                e.Graphics.DrawRectangle(pen, 0, 0, pnlStats.Width - 1, pnlStats.Height - 1);
            };

            string[] vals = { course.ActivityCount.ToString(), course.PendingCount.ToString(), course.SubmittedCount.ToString(), course.OverdueCount.ToString() };
            string[] labels = { "Activities", "Pending", "Submitted", "Overdue" };
            Color[] colors =
            {
                Color.FromArgb(63, 81, 181),
                Color.FromArgb(211, 84, 0),
                Color.FromArgb(46, 160, 67),
                Color.FromArgb(185, 50, 50)
            };

            for (int i = 0; i < 4; i++)
            {
                int cw = (cardW - 24) / 4, x = i * cw;
                pnlStats.Controls.Add(new Label { Text = vals[i], Font = new Font("Segoe UI", 15F, FontStyle.Bold), ForeColor = colors[i], Location = new Point(x, 2), Width = cw, Height = 28, TextAlign = ContentAlignment.MiddleCenter });
                pnlStats.Controls.Add(new Label { Text = labels[i], Font = new Font("Segoe UI", 7F), ForeColor = Color.Gray, Location = new Point(x, 32), Width = cw, Height = 15, TextAlign = ContentAlignment.MiddleCenter });
            }
            card.Controls.Add(pnlStats);

            // ── Footer row ─────────────────────────────────────────────────
            // Instructor
            card.Controls.Add(new Label
            {
                Text = $"👤 {course.InstructorName}",
                Font = new Font("Segoe UI", 7.5F),
                ForeColor = Color.FromArgb(100, 100, 110),
                Location = new Point(14, bottomY + 4),
                AutoSize = true
            });

            // Schedule / Room
            if (!string.IsNullOrWhiteSpace(course.Schedule))
                card.Controls.Add(new Label
                {
                    Text = $"🕐 {course.Schedule}{(string.IsNullOrWhiteSpace(course.Room) ? "" : "  |  " + course.Room)}",
                    Font = new Font("Segoe UI", 7.5F),
                    ForeColor = Color.FromArgb(100, 100, 110),
                    Location = new Point(14, bottomY + 20),
                    AutoSize = true,
                    MaximumSize = new Size(cardW - 160, 0),
                    AutoEllipsis = true
                });

            // Overdue badge
            if (course.OverdueCount > 0)
                card.Controls.Add(new Label
                {
                    Text = $"⚠ {course.OverdueCount} overdue",
                    Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                    BackColor = Color.FromArgb(255, 235, 235),
                    ForeColor = Color.FromArgb(185, 50, 50),
                    Location = new Point(14, bottomY - 18),
                    AutoSize = true,
                    Padding = new Padding(4, 2, 4, 2)
                });

            // "View Activities" button
            var btnView = new buttonRounded
            {
                Text = "View Activities",
                Size = new Size(132, 30),
                Location = new Point(cardW - 148, bottomY),
                BackColor = Color.FromArgb(128, 0, 0),
                ForeColor = Color.White,
                BorderRadius = 14,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Tag = "VIEW_BTN"
            };
            btnView.Click += (s, e) => OnOpenCourse?.Invoke(course);
            card.Controls.Add(btnView);

            return card;
        }

        // ── Empty state ───────────────────────────────────────────────────────
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
                                ? "📚  You are not enrolled in any courses"
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
                                ? "Contact your department or registrar to enroll in courses."
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
    }
}