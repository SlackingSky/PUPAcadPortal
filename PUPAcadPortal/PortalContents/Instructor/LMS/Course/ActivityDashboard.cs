using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class ActivityDashboard : UserControl
    {
        private List<CourseActivity> courses = new List<CourseActivity>();
        private string currentFilter = "All";
        private string searchTerm = "";

        public ActivityDashboard()
        {
            InitializeComponent();

            LoadSampleData();

            flpCourseCards.WrapContents = true;
            flpCourseCards.AutoScroll = true;
            flpCourseCards.FlowDirection = FlowDirection.LeftToRight;

            RefreshDashboard();

            flpCourseCards.SizeChanged += (s, e) => ResizeCards();
            this.Load += (s, e) => ResizeCards();
        }

        private void LoadSampleData()
        {
            courses = new List<CourseActivity>
            {
                new CourseActivity
                {
                    CourseId = 1,
                    CourseName = "Introduction to Programming 1",
                    CourseCode = "ITP 101",
                    InstructorName = "Prof. Juan dela Cruz",
                    TotalAssignments = 5,
                    TotalQuizzes = 3,
                    PendingSubmissions = 2,
                    CheckedSubmissions = 6,
                    NearestDeadline = DateTime.Now.AddDays(2),
                    Status = "Active",
                    ActivityCount = 8
                },

                new CourseActivity
                {
                    CourseId = 2,
                    CourseName = "Principles of Accounting",
                    CourseCode = "ACC 201",
                    InstructorName = "Prof. Maria Santos",
                    TotalAssignments = 3,
                    TotalQuizzes = 2,
                    PendingSubmissions = 1,
                    CheckedSubmissions = 4,
                    NearestDeadline = DateTime.Now.AddDays(5),
                    Status = "Active",
                    ActivityCount = 5
                },

                new CourseActivity
                {
                    CourseId = 3,
                    CourseName = "Human Computer Interactions",
                    CourseCode = "HCI 301",
                    InstructorName = "Prof. Ana Reyes",
                    TotalAssignments = 4,
                    TotalQuizzes = 1,
                    PendingSubmissions = 0,
                    CheckedSubmissions = 5,
                    NearestDeadline = DateTime.Now.AddDays(10),
                    Status = "Ongoing",
                    ActivityCount = 5
                },

                new CourseActivity
                {
                    CourseId = 4,
                    CourseName = "Programming and Technologies 1",
                    CourseCode = "PT 101",
                    InstructorName = "Prof. Carlos Bautista",
                    TotalAssignments = 6,
                    TotalQuizzes = 4,
                    PendingSubmissions = 3,
                    CheckedSubmissions = 7,
                    NearestDeadline = DateTime.Now.AddDays(1),
                    Status = "Active",
                    ActivityCount = 10
                }
            };
        }

        private void RefreshDashboard()
        {
            flpCourseCards.SuspendLayout();

            flpCourseCards.Controls.Clear();

            var filtered = courses.FindAll(c =>
            {
                bool matchFilter =
                    currentFilter == "All" ||
                    c.Status == currentFilter;

                bool matchSearch =
                    string.IsNullOrEmpty(searchTerm) ||
                    c.CourseName.ToLower().Contains(searchTerm.ToLower()) ||
                    c.CourseCode.ToLower().Contains(searchTerm.ToLower());

                return matchFilter && matchSearch;
            });

            foreach (var course in filtered)
            {
                Panel card = CreateCourseCard(course);
                flpCourseCards.Controls.Add(card);
            }

            UpdateSummaryStats();

            flpCourseCards.ResumeLayout();

            ResizeCards();
        }

        private void ResizeCards()
        {
            if (flpCourseCards.Controls.Count == 0)
                return;

            const int columns = 3;
            const int margin = 10;

            int availableWidth =
                flpCourseCards.ClientSize.Width -
                flpCourseCards.Padding.Left -
                flpCourseCards.Padding.Right -
                ((margin * 2) * columns);

            int cardWidth = Math.Max(300, availableWidth / columns);

            foreach (Control ctrl in flpCourseCards.Controls)
            {
                if (ctrl is Panel card)
                {
                    card.Width = cardWidth;

                    foreach (Control control in card.Controls)
                    {
                        if (control.Tag?.ToString() == "HEADER")
                        {
                            control.Width = cardWidth;

                            foreach (Control headerCtrl in control.Controls)
                            {
                                if (headerCtrl.Tag?.ToString() == "STATUS")
                                {
                                    headerCtrl.Location = new Point(
                                        cardWidth - headerCtrl.Width - 15,
                                        headerCtrl.Location.Y
                                    );
                                }

                                if (headerCtrl.Tag?.ToString() == "COURSE")
                                {
                                    headerCtrl.Width = cardWidth - 100;
                                }
                            }
                        }

                        if (control.Tag?.ToString() == "INSTRUCTOR")
                        {
                            control.Width = cardWidth - 20;
                        }

                        if (control.Tag?.ToString() == "STATS")
                        {
                            control.Width = cardWidth - 20;
                        }

                        if (control is buttonRounded btn)
                        {
                            btn.Location = new Point(
                                cardWidth - btn.Width - 15,
                                btn.Location.Y
                            );

                            btn.Anchor = AnchorStyles.Top | AnchorStyles.Right;

                            btn.Invalidate();
                        }
                    }

                    card.Invalidate();
                    card.Refresh();
                }
            }
        }

        private Panel CreateCourseCard(CourseActivity course)
        {
            int cardWidth = 430;

            Panel card = new Panel
            {
                Width = cardWidth,
                Height = 220,
                BackColor = Color.White,
                Margin = new Padding(10)
            };

            card.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(220, 220, 220)))
                {
                    e.Graphics.DrawRectangle(
                        pen,
                        0,
                        0,
                        card.Width - 1,
                        card.Height - 1
                    );
                }
            };

            Panel headerPanel = new Panel
            {
                Width = cardWidth,
                Height = 62,
                Dock = DockStyle.Top,
                BackColor = Color.Maroon,
                Tag = "HEADER"
            };

            Label lblCourse = new Label
            {
                Text = course.CourseName,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Location = new Point(10, 8),
                Width = cardWidth - 100,
                Height = 25,
                AutoEllipsis = true,
                Tag = "COURSE"
            };

            Label lblCode = new Label
            {
                Text = course.CourseCode,
                ForeColor = Color.FromArgb(230, 180, 180),
                Font = new Font("Segoe UI", 9F),
                Location = new Point(10, 35),
                Width = 200,
                Height = 18
            };

            Color statusColor =
                course.Status == "Active"
                ? Color.LightGreen
                : course.Status == "Completed"
                    ? Color.LightBlue
                    : Color.Yellow;

            Label lblStatus = new Label
            {
                Text = course.Status,
                BackColor = statusColor,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Size = new Size(65, 22),
                Location = new Point(cardWidth - 80, 18),
                TextAlign = ContentAlignment.MiddleCenter,
                Tag = "STATUS"
            };

            headerPanel.Controls.Add(lblCourse);
            headerPanel.Controls.Add(lblCode);
            headerPanel.Controls.Add(lblStatus);

            Label lblInstructor = new Label
            {
                Text = "Instructor: " + course.InstructorName,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(80, 80, 80),
                Location = new Point(10, 72),
                Width = cardWidth - 20,
                Height = 18,
                Tag = "INSTRUCTOR"
            };

            Panel pnlStats = new Panel
            {
                Location = new Point(10, 95),
                Size = new Size(cardWidth - 20, 52),
                BackColor = Color.FromArgb(248, 248, 248),
                Tag = "STATS"
            };

            string[] titles =
            {
                "Assignments",
                "Quizzes",
                "Pending",
                "Checked"
            };

            string[] values =
            {
                course.TotalAssignments.ToString(),
                course.TotalQuizzes.ToString(),
                course.PendingSubmissions.ToString(),
                course.CheckedSubmissions.ToString()
            };

            for (int i = 0; i < titles.Length; i++)
            {
                int columnWidth = (cardWidth - 20) / 4;
                int x = i * columnWidth;

                Label lblValue = new Label
                {
                    Text = values[i],
                    Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                    ForeColor = Color.Maroon,
                    Location = new Point(x, 2),
                    Width = columnWidth,
                    Height = 26,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                Label lblTitle = new Label
                {
                    Text = titles[i],
                    Font = new Font("Segoe UI", 7F),
                    ForeColor = Color.Gray,
                    Location = new Point(x, 30),
                    Width = columnWidth,
                    Height = 15,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                pnlStats.Controls.Add(lblValue);
                pnlStats.Controls.Add(lblTitle);
            }

            TimeSpan daysLeft = course.NearestDeadline - DateTime.Now;

            string deadlineText;

            if (daysLeft.TotalDays <= 0)
                deadlineText = "Due Today!";
            else if (daysLeft.Days == 1)
                deadlineText = "Due Tomorrow";
            else
                deadlineText = "Due in " + daysLeft.Days + " days";

            Color deadlineColor;

            if (daysLeft.TotalDays <= 0)
                deadlineColor = Color.Red;
            else if (daysLeft.Days <= 1)
                deadlineColor = Color.Red;
            else if (daysLeft.Days <= 3)
                deadlineColor = Color.Orange;
            else
                deadlineColor = Color.ForestGreen;

            Label lblDeadline = new Label
            {
                Text = "Nearest: " + deadlineText,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = deadlineColor,
                Location = new Point(10, 158),
                Width = 220,
                Height = 20
            };

            buttonRounded btnView = new buttonRounded
            {
                Text = "Open Course",
                Size = new Size(110, 32),
                Location = new Point(cardWidth - 125, 152),
                BackColor = Color.Maroon,
                ForeColor = Color.White,
                BorderRadius = 15,
                Cursor = Cursors.Hand,
                Tag = course
            };

            btnView.Click += BtnViewCourse_Click;

            card.Controls.Add(headerPanel);
            card.Controls.Add(lblInstructor);
            card.Controls.Add(pnlStats);
            card.Controls.Add(lblDeadline);
            card.Controls.Add(btnView);

            return card;
        }

        private void BtnViewCourse_Click(object sender, EventArgs e)
        {
            if (sender is buttonRounded btn &&
                btn.Tag is CourseActivity course)
            {
                AssignmentManagement assignmentMgmt =
                    new AssignmentManagement(course);

                assignmentMgmt.Dock = DockStyle.Fill;

                Control container = this.Parent;

                if (container != null)
                {
                    container.Controls.Remove(this);

                    assignmentMgmt.OnBack += () =>
                    {
                        container.Controls.Remove(assignmentMgmt);
                        container.Controls.Add(this);
                        this.BringToFront();
                    };

                    container.Controls.Add(assignmentMgmt);
                    assignmentMgmt.BringToFront();
                }
            }
        }

        private void UpdateSummaryStats()
        {
            int totalActivities = 0;
            int totalPending = 0;
            int totalChecked = 0;

            foreach (var c in courses)
            {
                totalActivities += c.ActivityCount;
                totalPending += c.PendingSubmissions;
                totalChecked += c.CheckedSubmissions;
            }

            lblTotalActivities.Text = totalActivities.ToString();
            lblTotalPending.Text = totalPending.ToString();
            lblTotalChecked.Text = totalChecked.ToString();
            lblTotalCourses.Text = courses.Count.ToString();
        }

        private void txtSearchCourse_TextChanged(object sender, EventArgs e)
        {
            searchTerm = txtSearchCourse.Text;
            RefreshDashboard();
        }

        private void cmbFilterCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentFilter =
                cmbFilterCourse.SelectedItem?.ToString() ?? "All";

            RefreshDashboard();
        }
    }

    public class CourseActivity
    {
        public int CourseId { get; set; }

        public string CourseName { get; set; } = "";

        public string CourseCode { get; set; } = "";

        public string InstructorName { get; set; } = "";

        public int TotalAssignments { get; set; }

        public int TotalQuizzes { get; set; }

        public int PendingSubmissions { get; set; }

        public int CheckedSubmissions { get; set; }

        public DateTime NearestDeadline { get; set; }

        public string Status { get; set; } = "Active";

        public int ActivityCount { get; set; }
    }
}