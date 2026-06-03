using PUPAcadPortal.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;

namespace PUPAcadPortal
{
    /// <summary>
    /// A single calendar day cell used in both the Instructor and Student
    /// monthly calendar views.
    /// </summary>
    public partial class UrDay : UserControl
    {
        // ── Static event raised when the user clicks a day cell ───────────────
        public static event Action<DateTime>? DaySelected;

        // ── Static drag-drop event (faculty calendar only) ────────────────────
        public static event Action<Guid, DateTime>? EventDropped;

        // ── Public state ──────────────────────────────────────────────────────
        public DateTime CellDate { get; private set; }

        public bool IsCurrentMonth { get; private set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                Invalidate();
                panel1?.Invalidate();
            }
        }

        // ── Private fields ────────────────────────────────────────────────────
        private bool _isSelected;
        private readonly bool _isStudent;
        private readonly string _holiday;

        private static readonly Color Maroon = Color.FromArgb(136, 14, 79);
        private static readonly Color TodayBg = Color.FromArgb(255, 243, 205);
        private static readonly Color OtherMonth = Color.FromArgb(245, 245, 245);
        private static readonly Color GridLine = Color.FromArgb(220, 220, 220);
        private static readonly Font DayFont = new Font("Segoe UI", 9f);
        private static readonly Font TodayFont = new Font("Segoe UI", 9f, FontStyle.Bold);
        private static readonly Font PillFont = new Font("Segoe UI", 6.8f);

        // Pill panels (event indicators) stored so we can refresh them
        private readonly List<Panel> _pills = new();

        // ── Constructor ───────────────────────────────────────────────────────
        /// <param name="day">Day number as a string (e.g. "1", "15").</param>
        /// <param name="year">Full year.</param>
        /// <param name="month">Month number 1–12.</param>
        /// <param name="isCurrentMonth">True when the day belongs to the displayed month.</param>
        /// <param name="holiday">Optional Philippine holiday name for this date.</param>
        /// <param name="isStudent">
        ///   True when rendered inside the Student calendar (uses SharedCalendarData);
        ///   false for the Faculty calendar (uses FacultyCalendarData).
        /// </param>
        public UrDay(string day, int year, int month, bool isCurrentMonth,
                     string holiday = "", bool isStudent = false)
        {
            InitializeComponent();

            _isStudent = isStudent;
            _holiday = holiday ?? "";
            IsCurrentMonth = isCurrentMonth;

            if (int.TryParse(day, out int d))
                CellDate = new DateTime(year, month, d);

            // Day number label
            lblDay.Text = day;
            lblDay.ForeColor = IsCurrentMonth
                ? (CellDate.DayOfWeek == DayOfWeek.Sunday ? Color.Crimson : Color.FromArgb(40, 40, 40))
                : Color.FromArgb(190, 190, 190);

            // Today highlight – circle drawn in Paint; set font bold
            if (CellDate == DateTime.Now.Date)
            {
                lblDay.Font = TodayFont;
                lblDay.ForeColor = Color.White;
            }

            // Holiday label reuses lblAnnouncement
            if (!string.IsNullOrEmpty(_holiday))
            {
                lblAnnouncement.Text = _holiday;
                lblAnnouncement.Visible = true;
            }

            // Allow dropping Faculty events onto this cell
            AllowDrop = true;
            DragEnter += UrDay_DragEnter;
            DragDrop += UrDay_DragDrop;

            // Ensure panel1 is on top so clicks are captured
            panel1.BringToFront();
        }

        // ── Load ──────────────────────────────────────────────────────────────
        private void UrDay_Load(object sender, EventArgs e)
        {
            RefreshNoteStrip();
            RefreshEventPills();
        }

        // ── Click ─────────────────────────────────────────────────────────────
        private void panel1_Click(object sender, EventArgs e)
        {
            DaySelected?.Invoke(CellDate);
        }

        // ── Paint ─────────────────────────────────────────────────────────────
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var rc = new Rectangle(0, 0, Width - 1, Height - 1);
            bool isToday = CellDate == DateTime.Now.Date;

            // Background
            Color bg = !IsCurrentMonth ? OtherMonth
                     : isToday ? TodayBg
                                      : Color.White;
            g.Clear(bg);

            // Maroon circle behind the day number when today
            if (isToday)
            {
                int cx = lblDay.Left + lblDay.Width / 2;
                int cy = lblDay.Top + lblDay.Height / 2;
                using var br = new SolidBrush(Maroon);
                g.FillEllipse(br, cx - 13, cy - 13, 26, 26);
            }

            // Selection overlay
            if (_isSelected && IsCurrentMonth)
            {
                using var selBr = new SolidBrush(Color.FromArgb(30, Maroon));
                g.FillRectangle(selBr, rc);
                using var selPen = new Pen(Maroon, 1.5f);
                g.DrawRectangle(selPen, rc);
            }

            // Cell border
            using var pen = new Pen(GridLine);
            g.DrawRectangle(pen, rc);
        }

        // ── Event pills ───────────────────────────────────────────────────────
        /// <summary>Re-reads event data and rebuilds the coloured pill indicators.</summary>
        public void RefreshEventPills()
        {
            // Remove existing pills
            foreach (var p in _pills) Controls.Remove(p);
            _pills.Clear();

            const int PILL_H = 14;
            const int PILL_GAP = 2;
            const int MAX_PILLS = 3;

            // Determine Y start: below note / holiday strips
            int y = 34;
            if (lblNote.Visible) y = lblNote.Bottom + PILL_GAP;
            if (lblAnnouncement.Visible) y = Math.Max(y, lblAnnouncement.Bottom + PILL_GAP);

            var pills = BuildPillData();
            int total = pills.Count;

            for (int i = 0; i < total; i++)
            {
                if (i >= MAX_PILLS)
                {
                    // "+N more" overflow pill
                    int remaining = total - i;
                    var more = MakePill($"+{remaining} more", Color.FromArgb(130, 130, 130), y, null);
                    Controls.Add(more);
                    _pills.Add(more);
                    more.BringToFront();
                    break;
                }

                var (text, color, ev) = pills[i];
                var pill = MakePill(text, color, y, ev);
                Controls.Add(pill);
                _pills.Add(pill);
                pill.BringToFront();
                y += PILL_H + PILL_GAP;
            }

            // Bring day number on top; then keep panel1 on top for click hit-testing
            lblDay.BringToFront();
            panel1.BringToFront();
        }

        // Returns (displayText, dotColor, optionalFacultyEvent) for each pill.
        private List<(string Text, Color Color, FacultyCalendarEvent? Ev)> BuildPillData()
        {
            var result = new List<(string, Color, FacultyCalendarEvent?)>();

            if (_isStudent)
            {
                // Shared (instructor-posted) events
                foreach (var ev in SharedCalendarData.GetEventsForDate(CellDate))
                    result.Add(($"{ev.GetTypeLabel()} {ev.Title}", ev.GetColor(), null));

                // Student personal events
                foreach (var ev in SharedCalendarData.GetStudentEventsForDate(CellDate))
                    result.Add(($"🔒 {ev.Title}",
                        Color.FromArgb(140, ev.GetColor().R, ev.GetColor().G, ev.GetColor().B), null));
            }
            else
            {
                // Faculty events (drag-and-drop enabled)
                foreach (var ev in FacultyCalendarData.GetEventsForDate(CellDate))
                    result.Add(($"{ev.GetTypeIcon()} {ev.Title}", ev.GetColor(), ev));
            }

            return result;
        }

        private Panel MakePill(string text, Color color, int top, FacultyCalendarEvent? dragSource)
        {
            const int PILL_H = 14;
            var pill = new Panel
            {
                Left = 2,
                Top = top,
                Width = Width - 6,
                Height = PILL_H,
                BackColor = color,
                Cursor = Cursors.Hand,
            };

            var lbl = new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                Font = PillFont,
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(2, 0, 0, 0),
                AutoEllipsis = true,
                BackColor = Color.Transparent,
            };
            pill.Controls.Add(lbl);

            // Clicking a pill still selects the day
            pill.Click += (s, e) => DaySelected?.Invoke(CellDate);
            lbl.Click += (s, e) => DaySelected?.Invoke(CellDate);

            // Drag-and-drop (faculty only)
            if (dragSource != null)
            {
                var captured = dragSource;
                pill.MouseDown += (s, e) =>
                {
                    if (e.Button == MouseButtons.Left)
                        pill.DoDragDrop(captured.Id, DragDropEffects.Move);
                };
                lbl.MouseDown += (s, e) =>
                {
                    if (e.Button == MouseButtons.Left)
                        pill.DoDragDrop(captured.Id, DragDropEffects.Move);
                };
            }

            return pill;
        }

        // ── Note strip ────────────────────────────────────────────────────────
        private void RefreshNoteStrip()
        {
            string note = "";

            if (!_isStudent)
            {
                // Instructor notes (static dict on CalendarContentInst)
                if (CalendarContentInst.notesDict.TryGetValue(CellDate.Date, out string? n) &&
                    !string.IsNullOrWhiteSpace(n))
                    note = n;
            }
            else
            {
                // Student notes (SharedCalendarData)
                if (SharedCalendarData.StudentNotes.TryGetValue(CellDate.Date, out string? n) &&
                    !string.IsNullOrWhiteSpace(n))
                    note = n;
            }

            if (!string.IsNullOrEmpty(note))
            {
                lblNote.Text = "🗒 " + note;
                lblNote.Visible = true;
            }
            else
            {
                lblNote.Text = "";
                lblNote.Visible = false;
            }
        }

        // ── Drag-drop (Faculty calendar rescheduling) ─────────────────────────
        private void UrDay_DragEnter(object? sender, DragEventArgs e)
        {
            e.Effect = (e.Data?.GetDataPresent(typeof(Guid)) == true && IsCurrentMonth)
                ? DragDropEffects.Move
                : DragDropEffects.None;
        }

        private void UrDay_DragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data?.GetData(typeof(Guid)) is Guid id && IsCurrentMonth)
                EventDropped?.Invoke(id, CellDate);
        }
    }
}