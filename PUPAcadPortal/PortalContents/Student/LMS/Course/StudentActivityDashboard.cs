using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class StudentActivityDashboard : UserControl
    {
        public event Action<StudentCourse> OnOpenCourse;

        private List<StudentCourse> courses;

        public StudentActivityDashboard()
        {
            InitializeComponent();
            LoadSampleData();
            UpdateStats();

            this.Load += (s, e) => RenderCards();
            flpCards.SizeChanged += (s, e) => RenderCards();
        }


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
            if (flpCards.ClientSize.Width < 100) return;

            flpCards.Controls.Clear();
            string search = txtSearch?.Text?.ToLower() ?? "";
            int padding = flpCards.Padding.Left + flpCards.Padding.Right; 
            int cardMargin = 10;  
            int cols = 3;
            int availWidth = flpCards.ClientSize.Width - padding - (cardMargin * 2 * cols) - 2;
            int cardWidth = Math.Max(280, availWidth / cols);
            int cardHeight = 220;

            foreach (var course in courses)
            {
                if (!string.IsNullOrEmpty(search) &&
                    !course.Name.ToLower().Contains(search) &&
                    !course.Code.ToLower().Contains(search))
                    continue;

                flpCards.Controls.Add(BuildCourseCard(course, cardWidth, cardHeight));
            }
        }

        private Panel BuildCourseCard(StudentCourse course, int cardWidth, int cardHeight)
        {
            var card = new Panel
            {
                Width = cardWidth,
                Height = cardHeight,
                BackColor = Color.White,
                Margin = new Padding(10),
                Cursor = Cursors.Hand
            };

            card.Paint += (s, e) =>
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(220, 220, 220)), 0, 0, card.Width - 1, card.Height - 1);

            var pnlTop = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(cardWidth, 62),
                BackColor = Color.Maroon
            };

            pnlTop.Controls.Add(new Label
            {
                Text = course.Name,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 8),
                Size = new Size(cardWidth - 20, 22),
                AutoEllipsis = true
            });

            pnlTop.Controls.Add(new Label
            {
                Text = course.Code,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(220, 180, 180),
                Location = new Point(10, 35),
                AutoSize = true
            });

            var lblInstr = new Label
            {
                Text = "Instructor: " + course.Instructor,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(80, 80, 80),
                Location = new Point(10, 70),
                Size = new Size(cardWidth - 20, 16)
            };

            var stats = new (string val, string name, Color color)[]
            {
                (course.ActivityCount.ToString(),  "Activities", Color.Maroon),
                (course.PendingCount.ToString(),   "Pending",    Color.OrangeRed),
                (course.SubmittedCount.ToString(), "Submitted",  Color.ForestGreen),
            };

            int colW = (cardWidth - 20) / 3;
            for (int i = 0; i < stats.Length; i++)
            {
                int x = 10 + i * colW;
                card.Controls.Add(new Label
                {
                    Text = stats[i].val,
                    Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                    ForeColor = stats[i].color,
                    Location = new Point(x, 95),
                    Size = new Size(colW, 28),
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                });
                card.Controls.Add(new Label
                {
                    Text = stats[i].name,
                    Font = new Font("Segoe UI", 7F),
                    ForeColor = Color.Gray,
                    Location = new Point(x, 123),
                    Size = new Size(colW, 14),
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                });
            }

            if (course.OverdueCount > 0)
            {
                pnlTop.Controls.Add(new Label
                {
                    Text = $"{course.OverdueCount} Overdue",
                    BackColor = Color.FromArgb(200, 30, 30),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 7F, FontStyle.Bold),
                    Location = new Point(cardWidth - 80, 8),
                    Size = new Size(70, 16),
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                });
            }

            int btnW = Math.Min(160, cardWidth - 20);
            var btnView = new buttonRounded
            {
                Text = "View Activities",
                Location = new Point(cardWidth - btnW - 10, 152),
                Size = new Size(btnW, 32),
                BackColor = Color.Maroon,
                ForeColor = Color.White,
                BorderRadius = 16,
                Tag = course
            };
            btnView.Click += (s, e) => OnOpenCourse?.Invoke(course);

            card.Controls.AddRange(new Control[] { pnlTop, lblInstr, btnView });
            return card;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) => RenderCards();
    }
}