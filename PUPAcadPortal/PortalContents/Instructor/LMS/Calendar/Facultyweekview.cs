using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using PUPAcadPortal.Data;

namespace PUPAcadPortal
{
    /// <summary>
    /// Full 7-column weekly view with hour rows.
    /// Supports drag-and-drop event rescheduling.
    /// Right-click on any time slot shows a context menu to Add Event or Add Note.
    /// </summary>
    public partial class FacultyWeekView : UserControl
    {
        // ── Events ────────────────────────────────────────────────────────────
        public event Action<FacultyCalendarEvent>? EventClicked;
        public event Action<DateTime>? DayHeaderClicked;
        public event Action<DateTime>? SlotDoubleClicked;

        /// <summary>Fired when the user picks "Add Event" from the right-click menu.</summary>
        public event Action<DateTime>? SlotAddEventRequested;

        /// <summary>Fired when the user picks "Add Note" from the right-click menu.</summary>
        public event Action<DateTime>? SlotAddNoteRequested;

        // ── Config ────────────────────────────────────────────────────────────
        private const int HOUR_W = 52;
        private const int HOUR_H = 52;
        private const int HDR_H = 36;
        private const int HOURS = 24;

        // ── State ─────────────────────────────────────────────────────────────
        private DateTime _weekStart;

        // Drag
        private FacultyCalendarEvent? _dragEvent;
        private Point _dragStartPt;
        private Panel? _dragGhost;
        private bool _dragging;

        // Right-click state
        private DateTime _rightClickDateTime;

        private static readonly Color Maroon = Color.FromArgb(136, 14, 79);
        private static readonly Color GridLine = Color.FromArgb(230, 230, 230);
        private static readonly Color TodayBg = Color.FromArgb(255, 245, 248);
        private static readonly Font UIFont = new Font("Segoe UI", 8f);
        private static readonly Font BoldFont = new Font("Segoe UI", 8f, FontStyle.Bold);
        private static readonly Font HdrFont = new Font("Segoe UI", 9f, FontStyle.Bold);
        private static readonly Font HourFont = new Font("Segoe UI", 7.5f);

        public FacultyWeekView()
        {
            AutoScroll = true;
            BackColor = Color.White;
            BuildSkeleton();
        }

        // ── Public API ────────────────────────────────────────────────────────
        public void LoadWeek(DateTime anyDayInWeek)
        {
            _weekStart = anyDayInWeek.Date.AddDays(-(int)anyDayInWeek.DayOfWeek);
            Rebuild();
        }

        // ── Build skeleton ────────────────────────────────────────────────────
        private void BuildSkeleton()
        {
            _headerRow = new Panel
            {
                Height = HDR_H,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(250, 250, 250),
            };
            _headerRow.Paint += HeaderRow_Paint;
            Controls.Add(_headerRow);

            _gridPanel = new Panel
            {
                Top = HDR_H,
                Left = 0,
                Width = ClientSize.Width,
                Height = HOUR_H * HOURS,
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            };
            _gridPanel.Paint += GridPanel_Paint;
            _gridPanel.MouseDoubleClick += GridPanel_DoubleClick;
            _gridPanel.MouseClick += GridPanel_MouseClick;   // right-click handler

            Controls.Add(_gridPanel);
            Resize += FacultyWeekView_Resize;
        }

        private void FacultyWeekView_Resize(object? sender, EventArgs e)
        {
            _headerRow.Width = ClientSize.Width;
            _gridPanel.Width = ClientSize.Width;
            Rebuild();
        }

        private void Rebuild()
        {
            if (_weekStart == default) return;

            var toRemove = _gridPanel.Controls.OfType<Panel>().ToList();
            foreach (var p in toRemove) _gridPanel.Controls.Remove(p);

            _headerRow.Invalidate();
            _gridPanel.Invalidate();

            int colW = Math.Max(80, (_gridPanel.Width - HOUR_W) / 7);
            var events = FacultyCalendarData.GetEventsForWeek(_weekStart);

            foreach (var ev in events)
            {
                if (ev.IsAllDay) continue;
                if (!TimeSpan.TryParse(ev.StartTime, out var st)) continue;

                int col = (int)ev.Date.DayOfWeek;
                int x = HOUR_W + col * colW + 2;
                int y = (int)(st.TotalMinutes / 60.0 * HOUR_H) + 2;

                int durationMin = 60;
                if (TimeSpan.TryParse(ev.EndTime, out var et))
                    durationMin = Math.Max(15, (int)(et - st).TotalMinutes);
                int h = (int)(durationMin / 60.0 * HOUR_H) - 4;

                var card = MakeEventCard(ev, x, y, colW - 4, h);
                _gridPanel.Controls.Add(card);
            }

            AutoScrollPosition = new Point(0, (int)(7 * HOUR_H));
        }

        // ── Right-click handler ───────────────────────────────────────────────
        private void GridPanel_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            int colW = Math.Max(80, (_gridPanel.Width - HOUR_W) / 7);
            int col = Math.Max(0, Math.Min(6, (e.X - HOUR_W) / colW));
            int hour = Math.Max(0, Math.Min(23, e.Y / HOUR_H));
            int minute = ((e.Y % HOUR_H) >= HOUR_H / 2) ? 30 : 0;

            _rightClickDateTime = _weekStart.AddDays(col).AddHours(hour).AddMinutes(minute);

            ShowSlotContextMenu(_gridPanel, e.Location);
        }

        // ── Context menu ──────────────────────────────────────────────────────
        private void ShowSlotContextMenu(Control parent, Point location)
        {
            var cms = new ContextMenuStrip();
            cms.Font = new Font("Segoe UI", 9f);
            cms.BackColor = Color.White;

            // ── Header label (non-clickable) ──────────────────────────────────
            var lblItem = new ToolStripLabel
            {
                Text = _rightClickDateTime.ToString("ddd, MMM dd  •  h:mm tt"),
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                ForeColor = Color.FromArgb(136, 14, 79),
                Enabled = false,
            };
            cms.Items.Add(lblItem);
            cms.Items.Add(new ToolStripSeparator());

            // ── Add Event ─────────────────────────────────────────────────────
            var addEvent = new ToolStripMenuItem
            {
                Text = "📅  Add Event Here",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(21, 101, 192),
            };
            addEvent.Click += (s, e) =>
            {
                // Fire the delegate so CalendarContentInst can open the dialog
                if (SlotAddEventRequested != null)
                    SlotAddEventRequested(_rightClickDateTime);
                else
                    SlotDoubleClicked?.Invoke(_rightClickDateTime); // fallback
            };
            cms.Items.Add(addEvent);

            // ── Add Note ──────────────────────────────────────────────────────
            var addNote = new ToolStripMenuItem
            {
                Text = "🗒  Add Note Here",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(80, 80, 80),
            };
            addNote.Click += (s, e) =>
            {
                SlotAddNoteRequested?.Invoke(_rightClickDateTime);
            };
            cms.Items.Add(addNote);

            cms.Items.Add(new ToolStripSeparator());

            // ── View day ──────────────────────────────────────────────────────
            var viewDay = new ToolStripMenuItem
            {
                Text = "📆  View Full Day",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(80, 80, 80),
            };
            viewDay.Click += (s, e) =>
            {
                DayHeaderClicked?.Invoke(_rightClickDateTime.Date);
            };
            cms.Items.Add(viewDay);

            cms.Show(parent.PointToScreen(location));
        }

        // ── Event card ────────────────────────────────────────────────────────
        private Panel MakeEventCard(FacultyCalendarEvent ev, int x, int y, int w, int h)
        {
            var card = new Panel
            {
                Left = x,
                Top = y,
                Width = Math.Max(20, w),
                Height = Math.Max(18, h),
                BackColor = BlendWithWhite(ev.GetColor(), 0.18f),
                Cursor = Cursors.SizeAll,
                Tag = ev,
            };

            card.Paint += (s, pe) =>
            {
                var g = pe.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var rr = RoundedRect(card.ClientRectangle, 4);
                g.FillPath(new SolidBrush(card.BackColor), rr);
                g.FillRectangle(new SolidBrush(ev.GetColor()), new Rectangle(0, 0, 4, card.Height));
                g.DrawPath(new Pen(ev.GetColor(), 0.5f), rr);
            };

            var lbl = new Label
            {
                Text = $"{ev.StartTime}  {ev.Title}",
                Left = 7,
                Top = 2,
                Width = card.Width - 10,
                Height = card.Height - 4,
                Font = h > 30 ? BoldFont : UIFont,
                ForeColor = DarkenColor(ev.GetColor(), 0.35f),
                AutoSize = false,
                AutoEllipsis = true,
            };
            card.Controls.Add(lbl);

            // Click to open detail
            void Clicked(object? s, EventArgs e) => EventClicked?.Invoke(ev);
            card.Click += Clicked;
            lbl.Click += Clicked;

            // Right-click on card: same slot context menu
            card.MouseClick += (s, me) =>
            {
                if (me.Button != MouseButtons.Right) return;
                _rightClickDateTime = ev.Date;
                if (TimeSpan.TryParse(ev.StartTime, out var st))
                    _rightClickDateTime = ev.Date.Add(st);
                ShowSlotContextMenu(card, me.Location);
            };

            // ── Drag ─────────────────────────────────────────────────────────
            card.MouseDown += (s, me) =>
            {
                if (me.Button != MouseButtons.Left) return;
                _dragEvent = ev;
                _dragStartPt = card.PointToScreen(me.Location);
                _dragging = false;
                card.Capture = true;
            };

            card.MouseMove += (s, me) =>
            {
                if (_dragEvent == null || me.Button != MouseButtons.Left) return;
                var cur = card.PointToScreen(me.Location);
                if (!_dragging && Math.Abs(cur.X - _dragStartPt.X) + Math.Abs(cur.Y - _dragStartPt.Y) > 6)
                {
                    _dragging = true;
                    StartDragGhost(card, ev);
                }
                if (_dragging && _dragGhost != null)
                {
                    var pt = _gridPanel.PointToClient(cur);
                    _dragGhost.Left = pt.X - _dragGhost.Width / 2;
                    _dragGhost.Top = pt.Y - _dragGhost.Height / 2;
                }
            };

            card.MouseUp += (s, me) =>
            {
                if (!_dragging) { _dragEvent = null; return; }
                _dragging = false;
                if (_dragGhost != null) { _gridPanel.Controls.Remove(_dragGhost); _dragGhost = null; }
                if (_dragEvent == null) return;

                var dropPt = _gridPanel.PointToClient(Cursor.Position);
                int colW2 = Math.Max(80, (_gridPanel.Width - HOUR_W) / 7);
                int newCol = Math.Max(0, Math.Min(6, (dropPt.X - HOUR_W) / colW2));
                int newHour = Math.Max(0, Math.Min(23, dropPt.Y / HOUR_H));
                var newDate = _weekStart.AddDays(newCol);
                var newTime = TimeSpan.FromHours(newHour);
                var captured = _dragEvent;
                _dragEvent = null;

                if (MessageBox.Show(
                        $"Move \"{captured.Title}\" to {newDate:dddd, MMM dd} at {newTime:hh\\:mm}?",
                        "Move Event", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    captured.Date = newDate;
                    captured.StartTime = newTime.ToString(@"hh\:mm");
                    FacultyCalendarData.UpdateEvent(captured);
                    Rebuild();
                }
            };

            return card;
        }

        private void StartDragGhost(Panel source, FacultyCalendarEvent ev)
        {
            _dragGhost = new Panel
            {
                Width = source.Width,
                Height = source.Height,
                BackColor = Color.FromArgb(160, ev.GetColor()),
                Left = source.Left,
                Top = source.Top,
            };
            _dragGhost.Paint += (s, pe) =>
            {
                using var rr = RoundedRect(_dragGhost!.ClientRectangle, 4);
                pe.Graphics.FillPath(new SolidBrush(_dragGhost.BackColor), rr);
            };
            _gridPanel.Controls.Add(_dragGhost);
            _dragGhost.BringToFront();
        }

        // ── Painting ──────────────────────────────────────────────────────────
        private void HeaderRow_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.FromArgb(250, 250, 250));
            g.FillRectangle(new SolidBrush(Color.FromArgb(245, 245, 245)),
                new Rectangle(0, 0, HOUR_W, HDR_H));

            int colW = Math.Max(80, (_headerRow.Width - HOUR_W) / 7);

            for (int i = 0; i < 7; i++)
            {
                var day = _weekStart.AddDays(i);
                bool today = day.Date == DateTime.Now.Date;
                int x = HOUR_W + i * colW;
                var rect = new Rectangle(x, 0, colW, HDR_H);

                if (today)
                    g.FillRectangle(new SolidBrush(Color.FromArgb(250, 230, 238)), rect);

                g.DrawString(day.ToString("ddd"),
                    UIFont, today ? new SolidBrush(Maroon) : Brushes.Gray,
                    new RectangleF(x + 2, 2, colW - 4, 14),
                    new StringFormat { Alignment = StringAlignment.Center });

                if (today)
                {
                    var cRect = new Rectangle(x + colW / 2 - 12, 14, 24, 18);
                    g.FillEllipse(new SolidBrush(Maroon), cRect);
                    g.DrawString(day.Day.ToString(), BoldFont, Brushes.White,
                        new RectangleF(cRect.X, cRect.Y + 1, cRect.Width, cRect.Height),
                        new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }
                else
                {
                    g.DrawString(day.Day.ToString(), HdrFont, Brushes.Black,
                        new RectangleF(x + 2, 14, colW - 4, 20),
                        new StringFormat { Alignment = StringAlignment.Center });
                }

                g.DrawLine(new Pen(GridLine), x, 0, x, HDR_H);
            }

            g.DrawLine(new Pen(Color.FromArgb(200, 200, 200)), 0, HDR_H - 1, _headerRow.Width, HDR_H - 1);
        }

        private void GridPanel_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);
            int colW = Math.Max(80, (_gridPanel.Width - HOUR_W) / 7);

            for (int h = 0; h < HOURS; h++)
            {
                int y = h * HOUR_H;
                g.DrawString(h == 0 ? "12 AM" : h < 12 ? $"{h} AM" : h == 12 ? "12 PM" : $"{h - 12} PM",
                    HourFont, Brushes.Gray,
                    new RectangleF(2, y + 2, HOUR_W - 4, 16));
                g.DrawLine(new Pen(GridLine), HOUR_W, y, _gridPanel.Width, y);
                g.DrawLine(new Pen(Color.FromArgb(242, 242, 242)),
                    HOUR_W, y + HOUR_H / 2, _gridPanel.Width, y + HOUR_H / 2);
            }

            for (int i = 0; i < 7; i++)
            {
                var day = _weekStart.AddDays(i);
                int x = HOUR_W + i * colW;
                if (day.Date == DateTime.Now.Date)
                    g.FillRectangle(new SolidBrush(TodayBg), new Rectangle(x, 0, colW, _gridPanel.Height));
                g.DrawLine(new Pen(GridLine), x, 0, x, _gridPanel.Height);
            }

            // Current time marker
            if (_weekStart <= DateTime.Now.Date && DateTime.Now.Date < _weekStart.AddDays(7))
            {
                int todayCol = (int)DateTime.Now.DayOfWeek;
                int tx = HOUR_W + todayCol * colW;
                int ty = (int)(DateTime.Now.TimeOfDay.TotalMinutes / 60.0 * HOUR_H);
                g.DrawLine(new Pen(Maroon, 2), tx, ty, tx + colW, ty);
                g.FillEllipse(new SolidBrush(Maroon), tx - 4, ty - 4, 8, 8);
            }
        }

        private void GridPanel_DoubleClick(object? sender, MouseEventArgs e)
        {
            int colW = Math.Max(80, (_gridPanel.Width - HOUR_W) / 7);
            int col = Math.Max(0, Math.Min(6, (e.X - HOUR_W) / colW));
            int hour = Math.Max(0, Math.Min(23, e.Y / HOUR_H));
            SlotDoubleClicked?.Invoke(_weekStart.AddDays(col).AddHours(hour));
        }

        // ── Utility ───────────────────────────────────────────────────────────
        private static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(r.Right - radius * 2, r.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(r.Right - radius * 2, r.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(r.X, r.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static Color BlendWithWhite(Color c, float ratio)
        {
            ratio = Math.Clamp(ratio, 0f, 1f);
            return Color.FromArgb(
                (int)(c.R + (255 - c.R) * (1f - ratio)),
                (int)(c.G + (255 - c.G) * (1f - ratio)),
                (int)(c.B + (255 - c.B) * (1f - ratio)));
        }

        private static Color DarkenColor(Color c, float amount)
        {
            amount = Math.Clamp(amount, 0f, 1f);
            return Color.FromArgb(
                (int)(c.R * (1f - amount)),
                (int)(c.G * (1f - amount)),
                (int)(c.B * (1f - amount)));
        }
    }
}