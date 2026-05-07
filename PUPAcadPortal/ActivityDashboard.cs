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
            RefreshDashboard();
        }

        private void LoadSampleData()
        {
            courses = new List<CourseActivity>
            {
                new CourseActivity
                {
                    CourseId = 1, CourseName = "Introduction to Programming 1",
                    CourseCode = "ITP 101", InstructorName = "Prof. Juan dela Cruz",
                    TotalAssignments = 5, TotalQuizzes = 3, PendingSubmissions = 2,
                    CheckedSubmissions = 6, NearestDeadline = DateTime.Now.AddDays(2),
                    Status = "Active", ActivityCount = 8
                },
                new CourseActivity
                {
                    CourseId = 2, CourseName = "Principles of Accounting",
                    CourseCode = "ACC 201", InstructorName = "Prof. Maria Santos",
                    TotalAssignments = 3, TotalQuizzes = 2, PendingSubmissions = 1,
                    CheckedSubmissions = 4, NearestDeadline = DateTime.Now.AddDays(5),
                    Status = "Active", ActivityCount = 5
                },
                new CourseActivity
                {
                    CourseId = 3, CourseName = "Human Computer Interactions",
                    CourseCode = "HCI 301", InstructorName = "Prof. Ana Reyes",
                    TotalAssignments = 4, TotalQuizzes = 1, PendingSubmissions = 0,
                    CheckedSubmissions = 5, NearestDeadline = DateTime.Now.AddDays(10),
                    Status = "Ongoing", ActivityCount = 5
                },
                new CourseActivity
                {
                    CourseId = 4, CourseName = "Programming and Technologies 1",
                    CourseCode = "PT 101", InstructorName = "Prof. Carlos Bautista",
                    TotalAssignments = 6, TotalQuizzes = 4, PendingSubmissions = 3,
                    CheckedSubmissions = 7, NearestDeadline = DateTime.Now.AddDays(1),
                    Status = "Active", ActivityCount = 10
                }
            };
        }

        private void RefreshDashboard()
        {
            flpCourseCards.Controls.Clear();

            var filtered = courses.FindAll(c =>
            {
                bool matchFilter = currentFilter == "All" || c.Status == currentFilter;
                bool matchSearch = string.IsNullOrEmpty(searchTerm) ||
                    c.CourseName.ToLower().Contains(searchTerm.ToLower()) ||
                    c.CourseCode.ToLower().Contains(searchTerm.ToLower());
                return matchFilter && matchSearch;
            });

            foreach (var course in filtered)
            {
                var card = CreateCourseCard(course);
                flpCourseCards.Controls.Add(card);
            }

            UpdateSummaryStats();
        }

        private Panel CreateCourseCard(CourseActivity course)
        {
            var card = new Panel
            {
                Width = 370,
                Height = 200,
                BackColor = Color.White,
                Margin = new Padding(10),
                Cursor = Cursors.Hand
            };

            card.Paint += (s, e) =>
            {
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(220, 220, 220)), 0, 0, card.Width - 1, card.Height - 1);
            };

            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.Maroon
            };

            var lblCourse = new Label
            {
                Text = course.CourseName,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Location = new Point(10, 8),
                Width = 280,
                Height = 25
            };

            var lblCode = new Label
            {
                Text = course.CourseCode,
                ForeColor = Color.FromArgb(230, 180, 180),
                Font = new Font("Segoe UI", 9F),
                Location = new Point(10, 35),
                Width = 200,
                Height = 20
            };

            Color statusColor = course.Status == "Active" ? Color.LightGreen :
                               course.Status == "Completed" ? Color.LightBlue : Color.Yellow;
            var lblStatus = new Label
            {
                Text = course.Status,
                BackColor = statusColor,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Location = new Point(300, 20),
                Size = new Size(60, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            headerPanel.Controls.AddRange(new Control[] { lblCourse, lblCode, lblStatus });

            var lblInstructor = new Label
            {
                Text = "Instructor: " + course.InstructorName,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(80, 80, 80),
                Location = new Point(10, 70),
                Width = 350,
                Height = 18
            };

            var pnlStats = new Panel
            {
                Location = new Point(10, 92),
                Size = new Size(350, 50),
                BackColor = Color.FromArgb(248, 248, 248)
            };

            var statLabels = new[]
            {
                ("Assignments", course.TotalAssignments.ToString()),
                ("Quizzes", course.TotalQuizzes.ToString()),
                ("Pending", course.PendingSubmissions.ToString()),
                ("Checked", course.CheckedSubmissions.ToString())
            };

            for (int i = 0; i < statLabels.Length; i++)
            {
                int x = i * 88;
                var lbl = new Label
                {
                    Text = statLabels[i].Item2,
                    Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                    ForeColor = Color.Maroon,
                    Location = new Point(x + 5, 2),
                    Width = 80,
                    Height = 28,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                var lblName = new Label
                {
                    Text = statLabels[i].Item1,
                    Font = new Font("Segoe UI", 7F),
                    ForeColor = Color.Gray,
                    Location = new Point(x + 5, 30),
                    Width = 80,
                    Height = 16,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                pnlStats.Controls.Add(lbl);
                pnlStats.Controls.Add(lblName);
            }

            TimeSpan daysLeft = course.NearestDeadline - DateTime.Now;
            string deadlineText = daysLeft.Days <= 0 ? "Due Today!" :
                                  daysLeft.Days == 1 ? "Due Tomorrow" :
                                  $"Due in {daysLeft.Days} days";
            Color deadlineColor = daysLeft.Days <= 1 ? Color.Red :
                                  daysLeft.Days <= 3 ? Color.Orange : Color.ForestGreen;

            var lblDeadline = new Label
            {
                Text = "Nearest: " + deadlineText,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = deadlineColor,
                Location = new Point(10, 148),
                Width = 200,
                Height = 18
            };

            var btnView = new buttonRounded
            {
                Text = "Open Course",
                Location = new Point(270, 143),
                Size = new Size(90, 30),
                BackColor = Color.Maroon,
                ForeColor = Color.White,
                BorderRadius = 15,
                Cursor = Cursors.Hand,
                Tag = course
            };
            btnView.Click += BtnViewCourse_Click;

            card.Controls.AddRange(new Control[] { headerPanel, lblInstructor, pnlStats, lblDeadline, btnView });

            return card;
        }

        private void BtnViewCourse_Click(object sender, EventArgs e)
        {
            if (sender is buttonRounded btn && btn.Tag is CourseActivity course)
            {
                var assignmentMgmt = new AssignmentManagement(course);
                assignmentMgmt.Dock = DockStyle.Fill;

                var parentForm = this.FindForm();
                if (parentForm != null)
                {
                    var container = this.Parent;
                    if (container != null)
                    {
                        container.Controls.Remove(this);
                        assignmentMgmt.OnBack += () =>
                        {
                            container.Controls.Remove(assignmentMgmt);
                            container.Controls.Add(this);
                        };
                        container.Controls.Add(assignmentMgmt);
                        assignmentMgmt.BringToFront();
                    }
                }
            }
        }

        private void UpdateSummaryStats()
        {
            int totalActivities = 0, totalPending = 0, totalChecked = 0;
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
            currentFilter = cmbFilterCourse.SelectedItem?.ToString() ?? "All";
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