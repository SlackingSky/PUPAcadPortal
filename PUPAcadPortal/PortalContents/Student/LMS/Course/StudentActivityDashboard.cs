using PUPAcadPortal.PortalContents.Student.LMS.Course;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Student.LMS.Course
{
    public partial class StudentActivityDashboard : UserControl
    {
        public event Action<StudentCourse> OnOpenCourse;

        private List<StudentCourse> _courses = new();
        private List<StudentNotification> _notifications = new();
        private System.Windows.Forms.Timer _searchTimer;

        public StudentActivityDashboard()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            _searchTimer = new System.Windows.Forms.Timer { Interval = 180 };
            _searchTimer.Tick += (s, e) => { _searchTimer.Stop(); RenderCards(); };

            LoadSampleData();
            LoadSampleNotifications();
            UpdateStats();

            lblStudentName.Text = "Juan Dela Cruz";

            this.Load += (s, e) => RenderCards();
            flpCards.SizeChanged += flpCards_SizeChanged;
        }

        // ── Sample data ──────────────────────────────────────────────────────

        private void LoadSampleData()
        {
            _courses = new List<StudentCourse>
            {
                new StudentCourse { Id=1, Name="Introduction to Programming 1", Code="ITP 101",
                    Instructor="Prof. Juan dela Cruz", Schedule="MWF 7:30\u20139:00 AM",
                    ActivityCount=8, PendingCount=2, SubmittedCount=5, OverdueCount=1 },
                new StudentCourse { Id=2, Name="Principles of Accounting", Code="ACC 201",
                    Instructor="Prof. Maria Santos", Schedule="TTH 10:00\u201311:30 AM",
                    ActivityCount=5, PendingCount=1, SubmittedCount=4, OverdueCount=0 },
                new StudentCourse { Id=3, Name="Human Computer Interactions", Code="HCI 301",
                    Instructor="Prof. Ana Reyes", Schedule="MWF 1:00\u20132:30 PM",
                    ActivityCount=5, PendingCount=1, SubmittedCount=4, OverdueCount=0 },
                new StudentCourse { Id=4, Name="Programming and Technologies 1", Code="PT 101",
                    Instructor="Prof. Carlos Bautista", Schedule="TTH 1:00\u20132:30 PM",
                    ActivityCount=10, PendingCount=1, SubmittedCount=8, OverdueCount=1 },
                new StudentCourse { Id=5, Name="Data Structures and Algorithms", Code="DSA 201",
                    Instructor="Prof. Leo Pascual", Schedule="MWF 3:00\u20134:30 PM",
                    ActivityCount=6, PendingCount=3, SubmittedCount=3, OverdueCount=0 },
            };
        }

        private void LoadSampleNotifications()
        {
            _notifications = new List<StudentNotification>
            {
                new StudentNotification { Kind="returned",     Title="Activity Returned",
                    Body="Prof. Santos returned 'Integrating Methodologies' with remarks.",
                    Time=DateTime.Now.AddMinutes(-12) },
                new StudentNotification { Kind="deadline",     Title="Deadline Reminder",
                    Body="'Programming Lab Activity 1' is due tomorrow.", Time=DateTime.Now.AddHours(-2) },
                new StudentNotification { Kind="new_activity", Title="New Activity Posted",
                    Body="Prof. Pascual posted 'Sorting Algorithms Quiz'.", Time=DateTime.Now.AddHours(-5) },
            };
            lblNotifCount.Text = _notifications.Count.ToString();
            lblNotifCount.Visible = _notifications.Count > 0;
        }

        // ── Stats ─────────────────────────────────────────────────────────────

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

        // ── Card rendering ────────────────────────────────────────────────────

        private void RenderCards()
        {
            if (flpCards.ClientSize.Width < 100) return;

            flpCards.SuspendLayout();
            flpCards.Controls.Clear();

            string search = txtSearch?.Text?.Trim().ToLower() ?? "";

            int pad = flpCards.Padding.Left + flpCards.Padding.Right;
            int cols = flpCards.ClientSize.Width >= 1300 ? 3
                       : flpCards.ClientSize.Width >= 800 ? 2 : 1;
            int gap = 14;
            int avail = flpCards.ClientSize.Width - pad - (gap * 2 * cols) - 4;
            int cardW = Math.Max(300, avail / cols);
            int cardH = 240;

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
            var card = new Panel
            {
                Width = cardW,
                Height = cardH,
                BackColor = Color.White,
                Margin = new Padding(10),
                Cursor = Cursors.Hand
            };

            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen = new Pen(Color.FromArgb(215, 215, 215));
                g.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            // ── Maroon header strip ──────────────────────────────────────────
            var pnlTop = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(cardW, 76),
                BackColor = Color.Maroon
            };

            pnlTop.Paint += (s, e) =>
            {
                using var brush = new LinearGradientBrush(
                    new Rectangle(0, 0, pnlTop.Width, pnlTop.Height),
                    Color.Maroon, Color.FromArgb(100, 0, 0), 135F);
                e.Graphics.FillRectangle(brush, 0, 0, pnlTop.Width, pnlTop.Height);
            };

            if (course.OverdueCount > 0)
            {
                pnlTop.Controls.Add(new Label
                {
                    Text = $"\u26A0  {course.OverdueCount} Overdue",
                    BackColor = Color.FromArgb(200, 30, 30),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                    Location = new Point(cardW - 94, 8),
                    Size = new Size(88, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                });
            }

            pnlTop.Controls.Add(new Label
            {
                Text = course.Name,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(12, 8),
                Size = new Size(course.OverdueCount > 0 ? cardW - 114 : cardW - 24, 24),
                AutoEllipsis = true
            });
            pnlTop.Controls.Add(new Label
            {
                Text = course.Code,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(215, 175, 175),
                Location = new Point(12, 36),
                AutoSize = true
            });
            pnlTop.Controls.Add(new Label
            {
                Text = "\uD83D\uDD50  " + course.Schedule,
                Font = new Font("Segoe UI", 7.5F),
                ForeColor = Color.FromArgb(200, 160, 160),
                Location = new Point(12, 54),
                AutoSize = true
            });

            var lblInstr = new Label
            {
                Text = "\uD83D\uDC64  " + course.Instructor,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(75, 75, 75),
                Location = new Point(12, 84),
                Size = new Size(cardW - 24, 18)
            };

            var statsData = new (string val, string label, Color clr)[]
            {
                (course.ActivityCount.ToString(),  "Activities", Color.FromArgb(128, 0,   0)),
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
                    Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                    ForeColor = statsData[i].clr,
                    Location = new Point(x, 108),
                    Size = new Size(colW, 30),
                    TextAlign = ContentAlignment.MiddleCenter
                });
                card.Controls.Add(new Label
                {
                    Text = statsData[i].label,
                    Font = new Font("Segoe UI", 7.5F),
                    ForeColor = Color.Gray,
                    Location = new Point(x, 138),
                    Size = new Size(colW, 14),
                    TextAlign = ContentAlignment.MiddleCenter
                });
            }

            var divider = new Panel
            {
                Location = new Point(12, 160),
                Size = new Size(cardW - 24, 1),
                BackColor = Color.FromArgb(232, 232, 232)
            };

            int btnW = Math.Min(164, cardW - 24);
            var btnView = new buttonRounded
            {
                Text = "View Activities  \u2192",
                Location = new Point(cardW - btnW - 12, 168),
                Size = new Size(btnW, 36),
                BackColor = Color.Maroon,
                ForeColor = Color.White,
                BorderRadius = 18,
                Tag = course,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnView.FlatAppearance.BorderSize = 0;
            btnView.Click += (s, e) => OnOpenCourse?.Invoke(course);

            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(252, 248, 248);
            card.MouseLeave += (s, e) => card.BackColor = Color.White;

            card.Controls.AddRange(new Control[] { pnlTop, lblInstr, divider, btnView });
            return card;
        }

        // ── Notification flyout ───────────────────────────────────────────────
        // FIX (Image 6): Position the flyout relative to the Form's screen
        //   coordinates so it appears correctly below the bell, anchored to
        //   the right edge.  The flyout now also has a maroon header with the
        //   bell icon and a proper scrollable list area.

        private void pnlNotifBadge_Click(object sender, EventArgs e)
        {
            ShowNotificationFlyout();
        }

        private void ShowNotificationFlyout()
        {
            // ── Build flyout ──────────────────────────────────────────────────
            var flyout = new Form
            {
                Text = "",
                Size = new Size(420, Math.Min(400, 60 + _notifications.Count * 80)),
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual,
                BackColor = Color.White,
                ShowInTaskbar = false,
                TopMost = true
            };

            // Thin border
            flyout.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(200, 200, 200));
                e.Graphics.DrawRectangle(pen, 0, 0, flyout.Width - 1, flyout.Height - 1);
            };

            // ── Position: bottom-left corner of the bell icon ─────────────────
            // Convert the badge panel's screen position so the flyout appears
            // directly below the bell regardless of where the window is.
            var bellScreen = pnlNotifBadge.PointToScreen(
                new Point(pnlNotifBadge.Width - flyout.Width, pnlNotifBadge.Height + 4));
            flyout.Location = bellScreen;

            // ── Header ───────────────────────────────────────────────────────
            var pnlH = new Panel { Dock = DockStyle.Top, Height = 44, BackColor = Color.Maroon };
            pnlH.Controls.Add(new Label
            {
                Text = "\uD83D\uDD14  Notifications",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(14, 0, 0, 0)
            });
            flyout.Controls.Add(pnlH);

            // ── Notification list ─────────────────────────────────────────────
            var flp = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(8)
            };

            int rowW = 388; // flyout is 420 wide; minus scroll bar padding

            foreach (var n in _notifications)
            {
                var row = new Panel
                {
                    Width = rowW,
                    Height = 72,
                    BackColor = n.IsRead ? Color.White : Color.FromArgb(254, 248, 248),
                    Margin = new Padding(0, 0, 0, 4),
                    Padding = new Padding(10, 8, 10, 8)
                };
                row.Paint += (s, e) =>
                {
                    using var pen = new Pen(Color.FromArgb(230, 230, 230));
                    e.Graphics.DrawRectangle(pen, 0, 0, row.Width - 1, row.Height - 1);
                };

                string icon = n.Kind switch
                {
                    "returned" => "\u21A9",
                    "deadline" => "\u23F0",
                    "feedback" => "\uD83D\uDCDD",
                    _ => "\uD83D\uDCCB"
                };
                Color iconColor = n.Kind switch
                {
                    "returned" => Color.FromArgb(30, 60, 180),
                    "deadline" => Color.OrangeRed,
                    "feedback" => Color.Purple,
                    _ => Color.Maroon
                };

                row.Controls.Add(new Label
                {
                    Text = icon,
                    Font = new Font("Segoe UI", 14F),
                    ForeColor = iconColor,
                    Location = new Point(8, 14),
                    Size = new Size(28, 28),
                    TextAlign = ContentAlignment.MiddleCenter
                });
                row.Controls.Add(new Label
                {
                    Text = n.Title,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(30, 30, 30),
                    Location = new Point(44, 8),
                    Size = new Size(rowW - 54, 18)
                });
                row.Controls.Add(new Label
                {
                    Text = n.Body,
                    Font = new Font("Segoe UI", 8F),
                    ForeColor = Color.FromArgb(80, 80, 80),
                    Location = new Point(44, 27),
                    Size = new Size(rowW - 54, 28),
                    AutoEllipsis = true
                });
                row.Controls.Add(new Label
                {
                    Text = FormatTimeAgo(n.Time),
                    Font = new Font("Segoe UI", 7.5F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    Location = new Point(44, 54),
                    AutoSize = true
                });

                flp.Controls.Add(row);
                n.IsRead = true;
            }

            if (_notifications.Count == 0)
            {
                flp.Controls.Add(new Label
                {
                    Text = "No new notifications.",
                    Font = new Font("Segoe UI", 9.5F),
                    ForeColor = Color.Gray,
                    Padding = new Padding(12),
                    AutoSize = true
                });
            }

            lblNotifCount.Visible = false;
            flyout.Controls.Add(flp);

            // Close on click-away
            flyout.Deactivate += (s, e) => flyout.Close();
            flyout.Show(this);
        }

        private static string FormatTimeAgo(DateTime t)
        {
            var span = DateTime.Now - t;
            if (span.TotalMinutes < 1) return "Just now";
            if (span.TotalMinutes < 60) return $"{(int)span.TotalMinutes}m ago";
            if (span.TotalHours < 24) return $"{(int)span.TotalHours}h ago";
            return t.ToString("MMM dd");
        }

        // ── Events ────────────────────────────────────────────────────────────

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            _searchTimer.Start();
        }

        private void flpCards_SizeChanged(object sender, EventArgs e) => RenderCards();
    }
}