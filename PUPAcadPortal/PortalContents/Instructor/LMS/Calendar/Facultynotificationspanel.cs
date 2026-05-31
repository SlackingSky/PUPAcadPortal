using System;
using System.Drawing;
using System.Windows.Forms;
using PUPAcadPortal.Data;

namespace PUPAcadPortal.PortalContents.Instructor.LMS.Calendar
{
    /// <summary>
    /// A popup panel that lists upcoming reminders and overdue alerts.
    /// Attach it to the parent form and call <see cref="Refresh"/> to update.
    /// </summary>
    public partial class FacultyNotificationsPanel : Panel
    {
        private static readonly Color Maroon = Color.FromArgb(136, 14, 79);
        private static readonly Color Overdue = Color.FromArgb(183, 28, 28);
        private static readonly Color Warning = Color.FromArgb(230, 81, 0);
        private static readonly Color Info = Color.FromArgb(21, 101, 192);
        private static readonly Font UIFont = new Font("Segoe UI", 8.5f);
        private static readonly Font BoldFont = new Font("Segoe UI", 8.5f, FontStyle.Bold);

        public FacultyNotificationsPanel()
        {
            InitializeComponent();

            this.BorderStyle = BorderStyle.None;

            // Shadow simulation via outer border
            this.Paint += (s, e) =>
            {
                using var p = new Pen(Color.FromArgb(200, 200, 200), 1);
                e.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
            };

            // Hide horizontal scrollbar 
            _flp.HorizontalScroll.Enabled = false;
            _flp.HorizontalScroll.Visible = false;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        public new void Refresh()
        {
            _flp.Controls.Clear();
            var notes = FacultyCalendarData.Notifications;

            if (notes.Count == 0)
            {
                _flp.Controls.Add(new Label
                {
                    Text = "No upcoming reminders.",
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Padding = new Padding(4, 8, 0, 0),
                    Font = UIFont,
                });
                return;
            }

            foreach (var n in notes)
            {
                _flp.Controls.Add(MakeRow(n));
            }

            // Enforce no horizontal scroll after dynamic additions
            _flp.HorizontalScroll.Enabled = false;
            _flp.HorizontalScroll.Visible = false;
        }

        private Panel MakeRow(FacultyNotification n)
        {
            Color accent = n.IsOverdue ? Overdue
                         : n.IsToday ? Warning
                         : n.DaysLeft == 1 ? Color.FromArgb(200, 130, 0)
                                           : Info;

            var row = new Panel
            {
                Width = _flp.ClientSize.Width - 16,
                Height = 60,
                BackColor = Color.FromArgb(250, 250, 250),
                Margin = new Padding(0, 0, 0, 4),
            };

            // Left accent bar
            var bar = new Panel { Width = 4, Height = 60, Left = 0, Top = 0, BackColor = accent };
            row.Controls.Add(bar);

            // Badge
            var badge = new Label
            {
                Text = n.GetLabel(),
                Left = 10,
                Top = 6,
                AutoSize = false,
                Width = 70,
                Height = 18,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 7f, FontStyle.Bold),
                BackColor = accent,
                ForeColor = Color.White,
            };
            row.Controls.Add(badge);

            // Title
            var lTitle = new Label
            {
                Text = $"{n.Event.GetTypeIcon()} {n.Event.Title}",
                Left = 10,
                Top = 27,
                Width = row.Width - 20,
                Font = BoldFont,
                ForeColor = Color.FromArgb(40, 40, 40),
                AutoSize = false,
                Height = 18,
                AutoEllipsis = true,
            };
            row.Controls.Add(lTitle);

            // Sub-info
            string sub = n.Date.ToString("MMM dd");
            if (!string.IsNullOrEmpty(n.Event.StartTime)) sub += $"  •  {n.Event.StartTime}";
            if (!string.IsNullOrEmpty(n.Event.Course)) sub += $"  •  {n.Event.Course}";

            var lSub = new Label
            {
                Text = sub,
                Left = 10,
                Top = 44,
                Width = row.Width - 20,
                Font = new Font("Segoe UI", 7.5f),
                ForeColor = Color.Gray,
                AutoSize = false,
                Height = 14,
                AutoEllipsis = true,
            };
            row.Controls.Add(lSub);

            // Border
            row.Paint += (s, e) =>
            {
                using var p = new Pen(Color.FromArgb(230, 230, 230), 1);
                e.Graphics.DrawRectangle(p, 0, 0, row.Width - 1, row.Height - 1);
            };

            return row;
        }
    }
}