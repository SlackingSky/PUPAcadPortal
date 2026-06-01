using PUPAcadPortal.PortalContents.Instructor.LMS.Course;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class ActivityDashboard : UserControl
    {
        public event Action<CourseActivity> OnOpenCourse;
        private List<CourseActivity> _courses = new List<CourseActivity>();
        private string _filterStatus = "All";
        private string _searchTerm = "";

        public ActivityDashboard()
        {
            InitializeComponent();
            LoadSampleData();
            SetupSearchDebounce();
            RefreshDashboard();

            flpCourseCards.SizeChanged += (s, e) => ResizeCards();
            this.Load += (s, e) => ResizeCards();
        }

        private void LoadSampleData()
        {
            _courses = new List<CourseActivity>
            {
                new CourseActivity
                {
                    CourseId = 1,
                    CourseName = "Introduction to Programming 1",
                    CourseCode = "ITP 101",
                    InstructorName = "Prof. Juan dela Cruz",
                    TotalAssignments = 5, TotalQuizzes = 3,
                    PendingSubmissions = 2, CheckedSubmissions = 6,
                    NearestDeadline = DateTime.Now.AddDays(2),
                    Status = "Active", ActivityCount = 8,
                    Activities = SampleActivities(1)
                },
                new CourseActivity
                {
                    CourseId = 2,
                    CourseName = "Principles of Accounting",
                    CourseCode = "ACC 201",
                    InstructorName = "Prof. Maria Santos",
                    TotalAssignments = 3, TotalQuizzes = 2,
                    PendingSubmissions = 1, CheckedSubmissions = 4,
                    NearestDeadline = DateTime.Now.AddDays(5),
                    Status = "Active", ActivityCount = 5,
                    Activities = SampleActivities(2)
                },
                new CourseActivity
                {
                    CourseId = 3,
                    CourseName = "Human Computer Interactions",
                    CourseCode = "HCI 301",
                    InstructorName = "Prof. Ana Reyes",
                    TotalAssignments = 4, TotalQuizzes = 1,
                    PendingSubmissions = 0, CheckedSubmissions = 5,
                    NearestDeadline = DateTime.Now.AddDays(10),
                    Status = "Ongoing", ActivityCount = 5,
                    Activities = SampleActivities(3)
                },
                new CourseActivity
                {
                    CourseId = 4,
                    CourseName = "Programming and Technologies 1",
                    CourseCode = "PT 101",
                    InstructorName = "Prof. Carlos Bautista",
                    TotalAssignments = 6, TotalQuizzes = 4,
                    PendingSubmissions = 3, CheckedSubmissions = 7,
                    NearestDeadline = DateTime.Now.AddDays(1),
                    Status = "Active", ActivityCount = 10,
                    Activities = SampleActivities(4)
                }
            };
        }

        private static List<ActivityItem> SampleActivities(int courseId)
        {
            var rng = new Random(courseId * 7);
            var types = new[] { ActivityType.Assignment, ActivityType.Quiz, ActivityType.Essay, ActivityType.FileUpload };
            var list = new List<ActivityItem>();
            string[] titles = {
                "Lab Exercise 1", "Midterm Quiz", "Written Report", "Final Project", "Weekly Assignment"
            };
            for (int i = 0; i < titles.Length; i++)
            {
                list.Add(new ActivityItem
                {
                    Id = courseId * 100 + i + 1,
                    CourseId = courseId,
                    Title = titles[i],
                    Type = types[i % types.Length],
                    Deadline = DateTime.Now.AddDays(rng.Next(-2, 14)),
                    Points = (rng.Next(3) + 1) * 25,
                    TotalStudents = 35,
                    SubmittedCount = rng.Next(15, 35),
                    LateCount = rng.Next(0, 5),
                    CheckedCount = rng.Next(5, 15),
                    Description = "Sample activity description for demonstration."
                });
            }
            return list;
        }

        private System.Windows.Forms.Timer _searchTimer;
        private void SetupSearchDebounce()
        {
            _searchTimer = new System.Windows.Forms.Timer { Interval = 200 };
            _searchTimer.Tick += (s, e) =>
            {
                _searchTimer.Stop();
                RefreshDashboard();
            };
        }

        private void RefreshDashboard()
        {
            flpCourseCards.SuspendLayout();
            flpCourseCards.Controls.Clear();

            var filtered = _courses.FindAll(c =>
            {
                bool matchFilter = _filterStatus == "All" || c.Status == _filterStatus;
                bool matchSearch = string.IsNullOrEmpty(_searchTerm)
                    || c.CourseName.IndexOf(_searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                    || c.CourseCode.IndexOf(_searchTerm, StringComparison.OrdinalIgnoreCase) >= 0;
                return matchFilter && matchSearch;
            });

            foreach (var course in filtered)
                flpCourseCards.Controls.Add(CreateCourseCard(course));

            UpdateSummaryStats();
            flpCourseCards.ResumeLayout();
            ResizeCards();
        }

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
                if (!(ctrl is Panel card)) continue;
                card.Width = cardW;
                foreach (Control c in card.Controls)
                {
                    if (c.Tag?.ToString() == "HEADER") { c.Width = cardW; UpdateHeaderChildren(c, cardW); }
                    else if (c.Tag?.ToString() == "STATS") c.Width = cardW - 24;
                    else if (c is buttonRounded b && b.Tag?.ToString() == "OPEN_BTN")
                        b.Location = new Point(cardW - 140, b.Location.Y);
                }
                card.Invalidate();
            }
        }

        private static void UpdateHeaderChildren(Control header, int cardW)
        {
            foreach (Control h in header.Controls)
            {
                if (h.Tag?.ToString() == "COURSE") h.Width = cardW - 24;
            }
        }

        private Panel CreateCourseCard(CourseActivity course)
        {
            const int cardW = 430;
            const int cardH = 175;
            const int statsY = 74;
            const int bottomY = 138;

            var card = new Panel
            {
                Width = cardW,
                Height = cardH,
                BackColor = Color.White,
                Margin = new Padding(10)
            };
            card.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(218, 218, 225));
                e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            var hdr = new Panel
            {
                Width = cardW,
                Height = 64,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(128, 0, 0),
                Tag = "HEADER"
            };

            var lblName = new Label
            {
                Text = course.CourseName,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                Location = new Point(12, 8),
                Width = cardW - 24,
                Height = 26,
                AutoEllipsis = true,
                Tag = "COURSE"
            };
            var lblCode = new Label
            {
                Text = course.CourseCode,
                ForeColor = Color.FromArgb(230, 185, 185),
                Font = new Font("Segoe UI", 8.5F),
                Location = new Point(12, 36),
                Width = 260,
                Height = 18
            };

            hdr.Controls.AddRange(new Control[] { lblName, lblCode });
            card.Controls.Add(hdr);

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

            string[] statVals = { course.TotalAssignments.ToString(), course.TotalQuizzes.ToString(),
                                    course.PendingSubmissions.ToString(), course.CheckedSubmissions.ToString() };
            string[] statLbls = { "Assign.", "Quizzes", "Pending", "Checked" };
            Color[] statColors = { Color.FromArgb(63, 81, 181), Color.FromArgb(0, 150, 136),
                                    Color.FromArgb(211, 84, 0),  Color.FromArgb(46, 160, 67) };
            for (int i = 0; i < 4; i++)
            {
                int colW = (cardW - 24) / 4;
                int x = i * colW;
                pnlStats.Controls.Add(new Label
                {
                    Text = statVals[i],
                    Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                    ForeColor = statColors[i],
                    Location = new Point(x, 2),
                    Width = colW,
                    Height = 28,
                    TextAlign = ContentAlignment.MiddleCenter
                });
                pnlStats.Controls.Add(new Label
                {
                    Text = statLbls[i],
                    Font = new Font("Segoe UI", 7F),
                    ForeColor = Color.Gray,
                    Location = new Point(x, 32),
                    Width = colW,
                    Height = 15,
                    TextAlign = ContentAlignment.MiddleCenter
                });
            }
            card.Controls.Add(pnlStats);

            var lblCount = new Label
            {
                Text = $"{course.ActivityCount} Activities",
                Font = new Font("Segoe UI", 7.5F),
                ForeColor = Color.FromArgb(120, 120, 130),
                Location = new Point(14, bottomY + 6),
                AutoSize = true
            };
            card.Controls.Add(lblCount);

            var btnOpen = new buttonRounded
            {
                Text = "Open Course",
                Size = new Size(126, 30),
                Location = new Point(cardW - 140, bottomY),
                BackColor = Color.FromArgb(128, 0, 0),
                ForeColor = Color.White,
                BorderRadius = 14,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Tag = "OPEN_BTN"
            };
            btnOpen.Click += (s, e) => OpenCourseView(course);
            card.Controls.Add(btnOpen);

            return card;
        }

        private void OpenCourseView(CourseActivity course)
        {
            if (OnOpenCourse != null)
            {
                OnOpenCourse.Invoke(course);
                return;
            }

            Control container = this.Parent;
            if (container == null) return;

            // ── Step 1: open ClassFilesPage ──────────────────────────────────
            var filesPage = new ClassFilesPage(course);
            filesPage.Dock = DockStyle.Fill;

            // Back from ClassFilesPage → return to ActivityDashboard
            filesPage.OnBack += () =>
            {
                Control c = filesPage.Parent ?? container;
                c.Controls.Remove(filesPage);
                filesPage.Dispose();
                c.Controls.Add(this);
                this.BringToFront();
            };

            // Activities button on ClassFilesPage → open AssignmentManagement
            filesPage.OnOpenActivities += (courseActivity) =>
            {
                Control filesContainer = filesPage.Parent ?? container;

                var mgmt = new AssignmentManagement(courseActivity);
                mgmt.Dock = DockStyle.Fill;

                // Back from AssignmentManagement → return to ClassFilesPage
                mgmt.OnBack += () =>
                {
                    Control mc = mgmt.Parent ?? filesContainer;
                    mc.Controls.Remove(mgmt);
                    mgmt.Dispose();
                    mc.Controls.Add(filesPage);
                    filesPage.BringToFront();
                };

                filesContainer.Controls.Remove(filesPage);
                filesContainer.Controls.Add(mgmt);
                mgmt.BringToFront();
            };

            container.Controls.Remove(this);
            container.Controls.Add(filesPage);
            filesPage.BringToFront();
        }

        private void UpdateSummaryStats()
        {
            int acts = 0, pend = 0, chk = 0;
            foreach (var c in _courses)
            {
                acts += c.ActivityCount;
                pend += c.PendingSubmissions;
                chk += c.CheckedSubmissions;
            }
            lblTotalCourses.Text = _courses.Count.ToString();
            lblTotalActivities.Text = acts.ToString();
            lblTotalPending.Text = pend.ToString();
            lblTotalChecked.Text = chk.ToString();
        }

        private void txtSearchCourse_TextChanged(object sender, EventArgs e)
        {
            _searchTerm = txtSearchCourse.Text;
            _searchTimer.Stop();
            _searchTimer.Start();
        }
    }
}