using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using PUPAcadPortal.Data;

namespace PUPAcadPortal.PortalContents.Instructor.LMS.Calendar
{
    /// <summary>
    /// Single-day hourly timeline.
    /// Drag cards vertically to reschedule within the day.
    /// </summary>
    public partial class FacultyDayView : UserControl
    {
        public event Action<FacultyCalendarEvent>? EventClicked;
        public event Action<DateTime>? SlotDoubleClicked;

        private const int HOUR_W = 60;
        private const int HOUR_H = 60;
        private const int HOURS = 24;
        private const int HDR_H = 50;

        private DateTime _date;
        private Panel _gridPanel = null!;
        private Panel _headerPanel = null!;

        private FacultyCalendarEvent? _dragEvent;
        private bool _dragging;
        private Panel? _dragGhost;

        private static readonly Color Maroon = Color.FromArgb(136, 14, 79);
        private static readonly Color GridLine = Color.FromArgb(230, 230, 230);
        private static readonly Font UIFont = new Font("Segoe UI", 8.5f);
        private static readonly Font BoldFont = new Font("Segoe UI", 8.5f, FontStyle.Bold);
        private static readonly Font HdrFont = new Font("Segoe UI", 13f, FontStyle.Bold);
        private static readonly Font HourFont = new Font("Segoe UI", 8f);

        public FacultyDayView()
        {
            InitializeComponent();
            AutoScroll = true;
            BackColor = Color.White;
            BuildSkeleton();
        }

        public void LoadDay(DateTime date)
        {
            _date = date.Date;
            Rebuild();
        }

        private void BuildSkeleton()
        {
            _headerPanel = new Panel { Dock = DockStyle.Top, Height = HDR_H, BackColor = Color.White };
            _headerPanel.Paint += HeaderPanel_Paint;
            Controls.Add(_headerPanel);

            _gridPanel = new Panel
            {
                Top = HDR_H,
                Left = 0,
                Width = ClientSize.Width,
                Height = HOUR_H * HOURS,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            };
            _gridPanel.Paint += GridPanel_Paint;
            _gridPanel.MouseDoubleClick += GridPanel_MouseDoubleClick;
            Controls.Add(_gridPanel);

            Resize += FacultyDayView_Resize;
        }

        private void GridPanel_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            int hour = Math.Max(0, Math.Min(23, e.Y / HOUR_H));
            SlotDoubleClicked?.Invoke(_date.AddHours(hour));
        }

        private void FacultyDayView_Resize(object? sender, EventArgs e)
        {
            _headerPanel.Width = ClientSize.Width;
            _gridPanel.Width = ClientSize.Width;
            Rebuild();
        }

        private void Rebuild()
        {
            if (_date == default) return;

            var old = _gridPanel.Controls.OfType<Panel>().ToList();
            foreach (var p in old) _gridPanel.Controls.Remove(p);

            _headerPanel.Invalidate();
            _gridPanel.Invalidate();

            int colX = HOUR_W + 4;
            int colW = _gridPanel.Width - HOUR_W - 12;
            var events = FacultyCalendarData.GetEventsForDate(_date).Where(e => !e.IsAllDay).ToList();

            // Detect overlaps for column splitting
            var placed = new List<(FacultyCalendarEvent ev, int colIdx, int totalCols)>();
            foreach (var ev in events)
            {
                if (!TimeSpan.TryParse(ev.StartTime, out var st)) continue;
                int startMin = (int)st.TotalMinutes;
                int endMin = startMin + 60;
                if (TimeSpan.TryParse(ev.EndTime, out var et)) endMin = (int)et.TotalMinutes;

                int ci = 0;
                while (placed.Any(p =>
                {
                    if (!TimeSpan.TryParse(p.ev.StartTime, out var ps)) return false;
                    int psm = (int)ps.TotalMinutes;
                    int pem = psm + 60;
                    if (TimeSpan.TryParse(p.ev.EndTime, out var pe2)) pem = (int)pe2.TotalMinutes;
                    return p.colIdx == ci && startMin < pem && endMin > psm;
                })) ci++;

                placed.Add((ev, ci, 0));
            }

            // Assign totalCols
            for (int i = 0; i < placed.Count; i++)
            {
                var (ev, ci, _) = placed[i];
                if (!TimeSpan.TryParse(ev.StartTime, out var st)) continue;
                int startMin = (int)st.TotalMinutes;
                int endMin = startMin + 60;
                if (TimeSpan.TryParse(ev.EndTime, out var et)) endMin = (int)et.TotalMinutes;

                int maxCol = placed
                    .Where(p =>
                    {
                        if (!TimeSpan.TryParse(p.ev.StartTime, out var ps)) return false;
                        int psm = (int)ps.TotalMinutes;
                        int pem = psm + 60;
                        if (TimeSpan.TryParse(p.ev.EndTime, out var pe2)) pem = (int)pe2.TotalMinutes;
                        return startMin < pem && endMin > psm;
                    })
                    .Max(p => p.colIdx) + 1;

                placed[i] = (ev, ci, maxCol);
            }

            foreach (var (ev, ci, totalCols) in placed)
            {
                if (!TimeSpan.TryParse(ev.StartTime, out var st)) continue;
                int startMin = (int)st.TotalMinutes;
                int endMin = startMin + 60;
                if (TimeSpan.TryParse(ev.EndTime, out var et)) endMin = (int)et.TotalMinutes;

                int total = Math.Max(1, totalCols);
                int subW = colW / total - 2;
                int subX = colX + ci * (subW + 2);
                int y = (int)(startMin / 60.0 * HOUR_H);
                int h = Math.Max(18, (int)((endMin - startMin) / 60.0 * HOUR_H) - 4);

                var card = MakeCard(ev, subX, y + 2, subW, h);
                _gridPanel.Controls.Add(card);
            }

            AutoScrollPosition = new Point(0, 7 * HOUR_H);
        }

        private Panel MakeCard(FacultyCalendarEvent ev, int x, int y, int w, int h)
        {
            var card = new Panel
            {
                Left = x,
                Top = y,
                Width = Math.Max(20, w),
                Height = Math.Max(18, h),
                BackColor = BlendWithWhite(ev.GetColor(), 0.15f),
                Cursor = Cursors.SizeAll,
                Tag = ev,
            };

            card.Paint += (s, pe) =>
            {
                var g = pe.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var rr = RoundedRect(card.ClientRectangle, 4);
                g.FillPath(new SolidBrush(card.BackColor), rr);
                g.FillRectangle(new SolidBrush(ev.GetColor()), 0, 0, 5, card.Height);
                g.DrawPath(new Pen(Color.FromArgb(80, ev.GetColor()), 0.5f), rr);
            };

            string timeStr = string.IsNullOrEmpty(ev.StartTime) ? "" : ev.StartTime;
            if (!string.IsNullOrEmpty(ev.EndTime)) timeStr += " – " + ev.EndTime;

            var lblTitle = new Label
            {
                Text = ev.Title,
                Left = 8,
                Top = 2,
                Width = card.Width - 10,
                Height = h > 36 ? 18 : h - 4,
                Font = BoldFont,
                ForeColor = DarkenColor(ev.GetColor(), 0.4f),
                AutoSize = false,
                AutoEllipsis = true,
            };
            card.Controls.Add(lblTitle);

            if (h > 38 && !string.IsNullOrEmpty(timeStr))
            {
                var lblTime = new Label
                {
                    Text = timeStr,
                    Left = 8,
                    Top = 20,
                    Width = card.Width - 10,
                    Height = 16,
                    Font = UIFont,
                    ForeColor = Color.FromArgb(90, 90, 90),
                    AutoSize = false,
                    AutoEllipsis = true,
                };
                card.Controls.Add(lblTime);
            }

            if (h > 56 && !string.IsNullOrEmpty(ev.Room))
            {
                var lblRoom = new Label
                {
                    Text = "📍 " + ev.Room,
                    Left = 8,
                    Top = 38,
                    Width = card.Width - 10,
                    Height = 16,
                    Font = UIFont,
                    ForeColor = Color.Gray,
                    AutoSize = false,
                    AutoEllipsis = true,
                };
                card.Controls.Add(lblRoom);
            }

            // Click
            Action<object?, EventArgs> clicked = (s, e) => EventClicked?.Invoke(ev);
            card.Click += clicked.Invoke;
            foreach (Control c in card.Controls) c.Click += clicked.Invoke;

            // Drag
            card.MouseDown += (s, me) =>
            {
                if (me.Button != MouseButtons.Left) return;
                _dragEvent = ev;
                _dragging = false;
                card.Capture = true;
            };
            card.MouseMove += (s, me) =>
            {
                if (_dragEvent == null || me.Button != MouseButtons.Left) return;
                if (!_dragging)
                {
                    _dragging = true;
                    _dragGhost = new Panel { Width = card.Width, Height = card.Height, BackColor = Color.FromArgb(160, ev.GetColor()) };
                    _gridPanel.Controls.Add(_dragGhost);
                    _dragGhost.BringToFront();
                }
                if (_dragGhost != null)
                {
                    var pt = _gridPanel.PointToClient(Cursor.Position);
                    _dragGhost.Left = card.Left;
                    _dragGhost.Top = Math.Max(0, pt.Y - _dragGhost.Height / 2);
                }
            };
            card.MouseUp += (s, me) =>
            {
                if (!_dragging) { _dragEvent = null; return; }
                _dragging = false;
                if (_dragGhost != null) { _gridPanel.Controls.Remove(_dragGhost); _dragGhost = null; }
                if (_dragEvent == null) return;

                var pt = _gridPanel.PointToClient(Cursor.Position);
                int newHour = Math.Max(0, Math.Min(23, pt.Y / HOUR_H));
                var captured = _dragEvent;
                _dragEvent = null;

                if (MessageBox.Show(
                        $"Move \"{captured.Title}\" to {_date:MMM dd} at {newHour:D2}:00?",
                        "Move Event", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    captured.StartTime = $"{newHour:D2}:00";
                    if (TimeSpan.TryParse(captured.EndTime, out var oldEnd) &&
                        TimeSpan.TryParse(captured.StartTime, out var newStart))
                    {
                        // Preserve duration
                    }
                    FacultyCalendarData.UpdateEvent(captured);
                    Rebuild();
                }
            };

            return card;
        }

        private void HeaderPanel_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            bool today = _date == DateTime.Now.Date;
            g.Clear(Color.White);

            string dayName = _date.ToString("dddd");
            string dateStr = _date.ToString("MMMM dd, yyyy");
            g.DrawString(dayName, HdrFont, today ? new SolidBrush(Maroon) : Brushes.Black,
                new PointF(HOUR_W + 8, 6));
            g.DrawString(dateStr, new Font("Segoe UI", 9f), Brushes.Gray,
                new PointF(HOUR_W + 8, 28));

            if (today)
                g.FillRectangle(new SolidBrush(Maroon), HOUR_W + 4, HDR_H - 3, 40, 3);

            g.DrawLine(new Pen(Color.FromArgb(200, 200, 200)), 0, HDR_H - 1, Width, HDR_H - 1);
        }

        private void GridPanel_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);

            for (int h = 0; h < HOURS; h++)
            {
                int y = h * HOUR_H;
                string label = h == 0 ? "12 AM" : h < 12 ? $"{h} AM" : h == 12 ? "12 PM" : $"{h - 12} PM";
                g.DrawString(label, HourFont, Brushes.Gray, new PointF(4, y + 3));
                g.DrawLine(new Pen(GridLine), HOUR_W, y, _gridPanel.Width, y);
                g.DrawLine(new Pen(Color.FromArgb(245, 245, 245)),
                    HOUR_W, y + HOUR_H / 2, _gridPanel.Width, y + HOUR_H / 2);
            }

            // Current time
            if (_date == DateTime.Now.Date)
            {
                int ty = (int)(DateTime.Now.TimeOfDay.TotalMinutes / 60.0 * HOUR_H);
                g.DrawLine(new Pen(Maroon, 2), HOUR_W, ty, _gridPanel.Width, ty);
                g.FillEllipse(new SolidBrush(Maroon), HOUR_W - 5, ty - 5, 10, 10);
            }
        }

        private static GraphicsPath RoundedRect(Rectangle r, int rad)
        {
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, rad * 2, rad * 2, 180, 90);
            p.AddArc(r.Right - rad * 2, r.Y, rad * 2, rad * 2, 270, 90);
            p.AddArc(r.Right - rad * 2, r.Bottom - rad * 2, rad * 2, rad * 2, 0, 90);
            p.AddArc(r.X, r.Bottom - rad * 2, rad * 2, rad * 2, 90, 90);
            p.CloseFigure();
            return p;
        }

        private static Color BlendWithWhite(Color c, float r)
        {
            r = Math.Clamp(r, 0f, 1f);
            return Color.FromArgb(
                (int)(c.R + (255 - c.R) * (1f - r)),
                (int)(c.G + (255 - c.G) * (1f - r)),
                (int)(c.B + (255 - c.B) * (1f - r)));
        }

        private static Color DarkenColor(Color c, float a)
        {
            a = Math.Clamp(a, 0f, 1f);
            return Color.FromArgb((int)(c.R * (1 - a)), (int)(c.G * (1 - a)), (int)(c.B * (1 - a)));
        }
    }
}