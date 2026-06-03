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
        private FlowLayoutPanel _flp;
        private Label _lblTitle;
        private Button _btnClose;

        private static readonly Color Maroon = Color.FromArgb(136, 14, 79);
        private static readonly Color Overdue = Color.FromArgb(183, 28, 28);
        private static readonly Color Warning = Color.FromArgb(230, 81, 0);
        private static readonly Color Info = Color.FromArgb(21, 101, 192);
        private static readonly Font UIFont = new Font("Segoe UI", 8.5f);
        private static readonly Font BoldFont = new Font("Segoe UI", 8.5f, FontStyle.Bold);

        // Fired when the ✕ button is clicked so CalendarContentInst can
        // re-enable the wheel filter.
        public event EventHandler? CloseRequested;

        public FacultyNotificationsPanel()
        {
            Width = 320;
            BackColor = Color.White;
            BorderStyle = BorderStyle.None;

            Paint += FacultyNotificationsPanel_Paint;

            // ── Header ────────────────────────────────────────────────────────
            var hdr = new Panel { Dock = DockStyle.Top, Height = 44, BackColor = Maroon };

            _lblTitle = new Label
            {
                Text = "🔔  Notifications",
                Dock = DockStyle.Fill,
                Font = BoldFont,
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0),
            };

            _btnClose = new Button
            {
                Text = "✕",
                Width = 30,
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = UIFont,
                Cursor = Cursors.Hand,
            };
            _btnClose.FlatAppearance.BorderSize = 0;
            _btnClose.Click += BtnClose_Click;

            hdr.Controls.Add(_lblTitle);
            hdr.Controls.Add(_btnClose);
            Controls.Add(hdr);

            // ── Scrollable list ───────────────────────────────────────────────
            _flp = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(8),
            };
            _flp.HorizontalScroll.Enabled = false;
            _flp.HorizontalScroll.Visible = false;
            Controls.Add(_flp);
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
                _flp.Controls.Add(MakeRow(n));

            _flp.HorizontalScroll.Enabled = false;
            _flp.HorizontalScroll.Visible = false;

            // Re-measure rows once the FLP has been laid out
            _flp.SizeChanged -= FlpSizeChanged;
            _flp.SizeChanged += FlpSizeChanged;
        }

        private void FlpSizeChanged(object? sender, EventArgs e)
        {
            // Recalculate every row width when the FLP is resized (e.g. first paint)
            int rowW = EffectiveRowWidth();
            foreach (Control c in _flp.Controls)
            {
                if (c is Panel row && row.Tag is FacultyNotification)
                {
                    row.Width = rowW;
                    // Update child label widths too
                    foreach (Control child in row.Controls)
                        if (child is Label lbl && lbl.Tag?.ToString() == "title_label")
                            lbl.Width = rowW - 16;
                        else if (child is Label lbl2 && lbl2.Tag?.ToString() == "sub_label")
                            lbl2.Width = rowW - 16;
                }
            }
            _flp.HorizontalScroll.Enabled = false;
            _flp.HorizontalScroll.Visible = false;
        }

        /// <summary>Row width = FLP client width (already accounts for padding &amp; scrollbar).</summary>
        private int EffectiveRowWidth()
        {
            // FLP padding is 8 each side; subtract 4 more for the margin gap
            int w = _flp.ClientSize.Width - 4;
            return Math.Max(100, w);
        }

        private Panel MakeRow(FacultyNotification n)
        {
            Color accent = n.IsOverdue ? Overdue
                         : n.IsToday ? Warning
                         : n.DaysLeft == 1 ? Color.FromArgb(200, 130, 0)
                                           : Info;

            // Use FLP client width; falls back to panel width on first render
            int rowW = _flp.ClientSize.Width > 20
                ? EffectiveRowWidth()
                : Math.Max(100, Width - 16 - SystemInformation.VerticalScrollBarWidth);

            var row = new Panel
            {
                Width = rowW,
                Height = 68,
                BackColor = Color.FromArgb(250, 250, 250),
                Margin = new Padding(0, 0, 0, 4),
                Tag = n,   // store notification so FlpSizeChanged can identify rows
            };

            // Left accent bar
            var bar = new Panel
            {
                Width = 4,
                Height = row.Height,
                Left = 0,
                Top = 0,
                BackColor = accent,
            };
            row.Controls.Add(bar);

            // Badge
            var badge = new Label
            {
                Text = n.GetLabel(),
                Left = 10,
                Top = 6,
                AutoSize = false,
                Width = 74,
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
                Top = 28,
                Width = rowW - 16,
                Font = BoldFont,
                ForeColor = Color.FromArgb(40, 40, 40),
                AutoSize = false,
                Height = 18,
                AutoEllipsis = true,
                Tag = "title_label",
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
                Top = 50,
                Width = rowW - 16,
                Font = new Font("Segoe UI", 7.5f),
                ForeColor = Color.Gray,
                AutoSize = false,
                Height = 14,
                AutoEllipsis = true,
                Tag = "sub_label",
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

        private void BtnClose_Click(object? sender, EventArgs e)
        {
            Visible = false;
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void FacultyNotificationsPanel_Paint(object? sender, PaintEventArgs e)
        {
            using var p = new Pen(Color.FromArgb(200, 200, 200), 1);
            e.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
        }
    }
}