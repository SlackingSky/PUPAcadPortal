using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class StudentActivityDashboard : UserControl
    {
        public event Action<StudentCourse> OnOpenCourse;

        private List<StudentCourse> _courses;
        private System.Windows.Forms.Timer _searchTimer;

        public StudentActivityDashboard()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            // Debounced search to prevent lag
            _searchTimer = new System.Windows.Forms.Timer { Interval = 180 };
            _searchTimer.Tick += (s, e) => { _searchTimer.Stop(); RenderCards(); };

            LoadSampleData();
            UpdateStats();

            this.Load += (s, e) => RenderCards();
            flpCards.SizeChanged += (s, e) => RenderCards();
        }

        

        //  Sample data 
        private void LoadSampleData()
        {
            _courses = new List<StudentCourse>
            {
                new StudentCourse { Id=1, Name="Introduction to Programming 1", Code="ITP 101",
                    Instructor="Prof. Juan dela Cruz",
                    ActivityCount=8, PendingCount=2, SubmittedCount=5, OverdueCount=1 },
                new StudentCourse { Id=2, Name="Principles of Accounting", Code="ACC 201",
                    Instructor="Prof. Maria Santos",
                    ActivityCount=5, PendingCount=1, SubmittedCount=4, OverdueCount=0 },
                new StudentCourse { Id=3, Name="Human Computer Interactions", Code="HCI 301",
                    Instructor="Prof. Ana Reyes",
                    ActivityCount=5, PendingCount=1, SubmittedCount=4, OverdueCount=0 },
                new StudentCourse { Id=4, Name="Programming and Technologies 1", Code="PT 101",
                    Instructor="Prof. Carlos Bautista",
                    ActivityCount=10, PendingCount=1, SubmittedCount=8, OverdueCount=1 },
            };
        }

        //  Stats bar 
        private void UpdateStats()
        {
            int tot = 0, pend = 0, sub = 0, over = 0;
            foreach (var c in _courses)
            {
                tot += c.ActivityCount;
                pend += c.PendingCount;
                sub += c.SubmittedCount;
                over += c.OverdueCount;
            }
            lblTotalValue.Text = tot.ToString();
            lblPendingValue.Text = pend.ToString();
            lblSubmittedValue.Text = sub.ToString();
            lblOverdueValue.Text = over.ToString();
        }

        //  Card rendering 
        private void RenderCards()
        {
            if (flpCards.ClientSize.Width < 100) return;

            flpCards.SuspendLayout();
            flpCards.Controls.Clear();

            string search = txtSearch?.Text?.Trim().ToLower() ?? "";
            int padding = flpCards.Padding.Left + flpCards.Padding.Right;
            int cols = flpCards.ClientSize.Width >= 1200 ? 3 :
                             flpCards.ClientSize.Width >= 700 ? 2 : 1;
            int gap = 12;
            int avail = flpCards.ClientSize.Width - padding - (gap * 2 * cols) - 4;
            int cardW = Math.Max(280, avail / cols);
            int cardH = 228;

            foreach (var c in _courses)
            {
                if (!string.IsNullOrEmpty(search) &&
                    !c.Name.ToLower().Contains(search) &&
                    !c.Code.ToLower().Contains(search) &&
                    !c.Instructor.ToLower().Contains(search))
                    continue;

                flpCards.Controls.Add(BuildCourseCard(c, cardW, cardH));
            }

            flpCards.ResumeLayout(true);
        }

        private Panel BuildCourseCard(StudentCourse course, int cardW, int cardH)
        {
            //  Outer card shell 
            var card = new Panel
            {
                Width = cardW,
                Height = cardH,
                BackColor = Color.White,
                Margin = new Padding(8),
                Cursor = Cursors.Hand
            };

            // Subtle border via Paint
            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen = new Pen(Color.FromArgb(220, 220, 220));
                g.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            //  Maroon header strip 
            var pnlTop = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(cardW, 70),
                BackColor = Color.Maroon
            };

            // Overdue badge (top-right)
            if (course.OverdueCount > 0)
            {
                pnlTop.Controls.Add(new Label
                {
                    Text = $"⚠ {course.OverdueCount} Overdue",
                    BackColor = Color.FromArgb(200, 30, 30),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                    Location = new Point(cardW - 88, 9),
                    Size = new Size(82, 18),
                    TextAlign = ContentAlignment.MiddleCenter
                });
            }

            pnlTop.Controls.Add(new Label
            {
                Text = course.Name,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(12, 8),
                Size = new Size(course.OverdueCount > 0 ? cardW - 106 : cardW - 24, 22),
                AutoEllipsis = true
            });

            pnlTop.Controls.Add(new Label
            {
                Text = course.Code,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(220, 180, 180),
                Location = new Point(12, 38),
                AutoSize = true
            });

            //  Instructor line 
            var lblInstr = new Label
            {
                Text = "👤 " + course.Instructor,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(85, 85, 85),
                Location = new Point(12, 78),
                Size = new Size(cardW - 24, 16)
            };

            //  Stats row 
            var statsData = new (string val, string label, Color clr)[]
            {
                (course.ActivityCount.ToString(),  "Activities", Color.FromArgb(128, 0, 0)),
                (course.PendingCount.ToString(),   "Pending",    Color.OrangeRed),
                (course.SubmittedCount.ToString(), "Submitted",  Color.ForestGreen),
            };

            int colW = (cardW - 24) / 3;
            for (int i = 0; i < statsData.Length; i++)
            {
                int x = 12 + i * colW;
                card.Controls.Add(new Label
                {
                    Text = statsData[i].val,
                    Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                    ForeColor = statsData[i].clr,
                    Location = new Point(x, 100),
                    Size = new Size(colW, 30),
                    TextAlign = ContentAlignment.MiddleCenter
                });
                card.Controls.Add(new Label
                {
                    Text = statsData[i].label,
                    Font = new Font("Segoe UI", 7.5F),
                    ForeColor = Color.Gray,
                    Location = new Point(x, 130),
                    Size = new Size(colW, 14),
                    TextAlign = ContentAlignment.MiddleCenter
                });
            }

            //  Divider 
            var divider = new Panel
            {
                Location = new Point(12, 150),
                Size = new Size(cardW - 24, 1),
                BackColor = Color.FromArgb(230, 230, 230)
            };

            //  View Activities button 
            int btnW = Math.Min(160, cardW - 24);
            var btnView = new buttonRounded
            {
                Text = "View Activities",
                Location = new Point(cardW - btnW - 12, 160),
                Size = new Size(btnW, 34),
                BackColor = Color.Maroon,
                ForeColor = Color.White,
                BorderRadius = 17,
                Tag = course,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnView.FlatAppearance.BorderSize = 0;
            btnView.Click += (s, e) => OnOpenCourse?.Invoke(course);

            card.Controls.AddRange(new Control[] { pnlTop, lblInstr, divider, btnView });
            return card;
        }

        //  Search (debounced) 
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            _searchTimer.Start();
        }
    }
}