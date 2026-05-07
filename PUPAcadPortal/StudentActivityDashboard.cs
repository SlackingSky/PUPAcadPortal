using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    /// <summary>Student-facing activity dashboard showing all courses and summary stats.</summary>
    public partial class StudentActivityDashboard : UserControl
    {
        public event Action<StudentCourse> OnOpenCourse;

        private List<StudentCourse> courses;

        public StudentActivityDashboard()
        {
            InitializeComponent();
            LoadSampleData();
            UpdateStats();
            RenderCards();
        }

        // ── Data ──────────────────────────────────────────────────────────────

        private void LoadSampleData()
        {
            courses = new List<StudentCourse>
            {
                new StudentCourse { Id=1, Name="Introduction to Programming 1", Code="ITP 101",
                    Instructor="Prof. Juan dela Cruz", ActivityCount=8, PendingCount=2, SubmittedCount=5, OverdueCount=1 },
                new StudentCourse { Id=2, Name="Principles of Accounting", Code="ACC 201",
                    Instructor="Prof. Maria Santos", ActivityCount=5, PendingCount=1, SubmittedCount=4, OverdueCount=0 },
                new StudentCourse { Id=3, Name="Human Computer Interactions", Code="HCI 301",
                    Instructor="Prof. Ana Reyes", ActivityCount=5, PendingCount=1, SubmittedCount=4, OverdueCount=0 },
                new StudentCourse { Id=4, Name="Programming and Technologies 1", Code="PT 101",
                    Instructor="Prof. Carlos Bautista", ActivityCount=10, PendingCount=1, SubmittedCount=8, OverdueCount=1 },
            };
        }

        // ── UI update helpers ─────────────────────────────────────────────────

        private void UpdateStats()
        {
            int totalAct = 0, totalPend = 0, totalSub = 0, totalOver = 0;
            foreach (var c in courses)
            {
                totalAct += c.ActivityCount;
                totalPend += c.PendingCount;
                totalSub += c.SubmittedCount;
                totalOver += c.OverdueCount;
            }

            lblTotalValue.Text = totalAct.ToString();
            lblPendingValue.Text = totalPend.ToString();
            lblSubmittedValue.Text = totalSub.ToString();
            lblOverdueValue.Text = totalOver.ToString();
        }

        private void RenderCards()
        {
            flpCards.Controls.Clear();
            string search = txtSearch?.Text?.ToLower() ?? "";

            foreach (var course in courses)
            {
                if (!string.IsNullOrEmpty(search) &&
                    !course.Name.ToLower().Contains(search) &&
                    !course.Code.ToLower().Contains(search))
                    continue;

                flpCards.Controls.Add(BuildCourseCard(course));
            }
        }

        private Panel BuildCourseCard(StudentCourse course)
        {
            var card = new Panel
            {
                Width = 340,
                Height = 210,
                BackColor = Color.White,
                Margin = new Padding(8),
                Cursor = Cursors.Hand
            };
            card.Paint += (s, e) =>
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(220, 220, 220)), 0, 0, card.Width - 1, card.Height - 1);

            // Maroon header (fixed height, not docked so body controls aren't pushed)
            var pnlTop = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(340, 58),
                BackColor = Color.Maroon
            };
            pnlTop.Controls.Add(new Label
            {
                Text = course.Name,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 7),
                Size = new Size(320, 22),
                AutoEllipsis = true
            });
            pnlTop.Controls.Add(new Label
            {
                Text = course.Code,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(220, 180, 180),
                Location = new Point(10, 33),
                AutoSize = true
            });

            // Instructor label
            var lblInstr = new Label
            {
                Text = "Instructor: " + course.Instructor,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(80, 80, 80),
                Location = new Point(10, 66),
                Size = new Size(320, 16)
            };

            // Mini stat numbers
            var stats = new (string val, string name, Color color)[]
            {
                (course.ActivityCount.ToString(),  "Activities", Color.Maroon),
                (course.PendingCount.ToString(),   "Pending",    Color.OrangeRed),
                (course.SubmittedCount.ToString(), "Submitted",  Color.ForestGreen),
            };

            for (int i = 0; i < stats.Length; i++)
            {
                int x = 10 + i * 108;
                card.Controls.Add(new Label
                {
                    Text = stats[i].val,
                    Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                    ForeColor = stats[i].color,
                    Location = new Point(x, 90),
                    Size = new Size(95, 28),
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                });
                card.Controls.Add(new Label
                {
                    Text = stats[i].name,
                    Font = new Font("Segoe UI", 7F),
                    ForeColor = Color.Gray,
                    Location = new Point(x, 118),
                    Size = new Size(95, 14),
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                });
            }

            // View button
            var btnView = new buttonRounded
            {
                Text = "View Activities",
                Location = new Point(175, 148),
                Size = new Size(140, 30),
                BackColor = Color.Maroon,
                ForeColor = Color.White,
                BorderRadius = 15,
                Tag = course
            };
            btnView.Click += (s, e) => OnOpenCourse?.Invoke(course);

            card.Controls.AddRange(new Control[] { pnlTop, lblInstr, btnView });
            return card;
        }

        // ── Event handlers ────────────────────────────────────────────────────

        private void txtSearch_TextChanged(object sender, EventArgs e) => RenderCards();
    }
}