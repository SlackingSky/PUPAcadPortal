using PUPAcadPortal.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using PUPAcadPortal.Utils;

namespace PUPAcadPortal.PortalContents.Instructor.LMS
{
    public partial class CalendarContentInst : UserControl
    {
        public static int _year, _month;
        public static Dictionary<DateTime, string> notesDict = new Dictionary<DateTime, string>();
        private FlowLayoutPanel pnlDayHeaders;
        private Panel pnlBottom;
        private Label lblSelectedDate;
        private FlowLayoutPanel flpDayEvents;
        private FlowLayoutPanel flpUpcoming;
        private Label lblNoEvents;
        private Label lblNoUpcoming;
        private DateTime _lastSelectedDate = DateTime.Now.Date;
        private EventType? _activeFilter = null;

        public CalendarContentInst()
        {
            InitializeComponent();

        }

        private void CalendarContentInst_Load(object sender, EventArgs e)
        {
            BuildDayHeaders();
            BuildBottomPanel();
            FPLmonth.Resize += (s, ev) => { ResizeCalendarCells(); AlignDayHeaders(); };
            SharedCalendarData.LoadData();
            this.Resize += OnFormResized;

            showDays(DateTime.Now.Month, DateTime.Now.Year);

            UrDay.DaySelected += OnDaySelected;
            OnDaySelected(DateTime.Now.Date);

            var wheelFilter = new CalendarWheelFilter(FPLmonth, delta =>
            {
                if (delta > 0) picPrev_Click(this, EventArgs.Empty);
                else picNext_Click(this, EventArgs.Empty);
            });
            System.Windows.Forms.Application.AddMessageFilter(wheelFilter);


            this.BeginInvoke((Action)(() =>
            {
                FitCalendarPanel();
                ResizeCalendarCells();
                CenterMonthLabel();
                PositionBottomPanel();
            }));
        }

        private void BuildBottomPanel()
        {
            const int BOTTOM_H = 220;

            pnlBottom = new Panel
            {
                BackColor = Color.White,
                Height = BOTTOM_H,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
            };
            pnlBottom.Parent = pnlCalendar;
            pnlBottom.BringToFront();

            var sep = new Panel { Height = 1, Dock = DockStyle.Top, BackColor = Color.FromArgb(220, 220, 220) };
            pnlBottom.Controls.Add(sep);

            var pnlLeft = new Panel { Left = 0, Top = 4, Width = 520, Height = BOTTOM_H - 8, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom };
            pnlBottom.Controls.Add(pnlLeft);

            lblSelectedDate = new Label
            {
                Text = "Select a day to manage events",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.Maroon,
                Left = 12,
                Top = 6,
                AutoSize = true,
            };
            pnlLeft.Controls.Add(lblSelectedDate);

            var btnAddEvent = new Button
            {
                Text = "+ Add Event",
                Left = 12,
                Top = 28,
                Width = 95,
                Height = 26,
                BackColor = Color.Maroon,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5f),
                Cursor = Cursors.Hand,
            };
            btnAddEvent.FlatAppearance.BorderSize = 0;
            btnAddEvent.Click += (s, e) => QuickAddEventForSelected();
            pnlLeft.Controls.Add(btnAddEvent);

            int bx = 118;
            var filters = new[] {
                ("All",      (EventType?)null),
                ("Class",    (EventType?)EventType.Class),
                ("Exam",     (EventType?)EventType.Exam),
                ("Deadline", (EventType?)EventType.Deadline),
                ("Consult",  (EventType?)EventType.Consultation),
            };
            foreach (var (label, ft) in filters)
            {
                var captured = ft;
                Color accent = ft == null ? Color.FromArgb(90, 90, 90) : new CalendarEvent { Type = ft.Value }.GetColor();
                var btn = MakeFilterButton(label, accent);
                btn.Left = bx; btn.Top = 30;
                btn.Click += (s, e) =>
                {
                    _activeFilter = captured;
                    RefreshDayDetail(_lastSelectedDate);
                };
                pnlLeft.Controls.Add(btn);
                bx += btn.Width + 5;
            }

            flpDayEvents = new FlowLayoutPanel
            {
                Left = 0,
                Top = 62,
                Width = 510,
                Height = BOTTOM_H - 70,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom,
            };
            flpDayEvents.HorizontalScroll.Maximum = 0;
            flpDayEvents.HorizontalScroll.Enabled = false;
            flpDayEvents.HorizontalScroll.Visible = false;
            flpDayEvents.AutoScroll = true;

            lblNoEvents = new Label { Text = "No events for this day. Use '+ Add Event' to create one.", ForeColor = Color.Gray, AutoSize = true, Padding = new Padding(8) };
            flpDayEvents.Controls.Add(lblNoEvents);
            pnlLeft.Controls.Add(flpDayEvents);

            var div = new Panel { Left = 530, Top = 8, Width = 1, Height = BOTTOM_H - 16, BackColor = Color.FromArgb(220, 220, 220), Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left };
            pnlBottom.Controls.Add(div);
            var pnlRight = new Panel
            {
                Name = "pnlRight",
                Left = 538,
                Top = 4,
                Width = 400,
                Height = BOTTOM_H - 8,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                AutoScroll = false,
            }; 
            pnlBottom.Controls.Add(pnlRight);

            var lblUpTitle = new Label { Text = "Upcoming", Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = Color.FromArgb(60, 60, 60), Left = 8, Top = 6, AutoSize = true };
            pnlRight.Controls.Add(lblUpTitle);

            BuildLegend(pnlRight, 8, 28);

            flpUpcoming = new FlowLayoutPanel
            {
                Left = 0,
                Top = 80,
                Width = 400,
                Height = BOTTOM_H - 64,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom,
            };
            flpUpcoming.HorizontalScroll.Maximum = 0;
            flpUpcoming.HorizontalScroll.Enabled = false;
            flpUpcoming.HorizontalScroll.Visible = false;
            flpUpcoming.AutoScroll = true;

            lblNoUpcoming = new Label { Text = "No upcoming events.", ForeColor = Color.Gray, AutoSize = true, Padding = new Padding(8) };
            flpUpcoming.Controls.Add(lblNoUpcoming);
            pnlRight.Controls.Add(flpUpcoming);

            RefreshUpcoming();
            PositionBottomPanel();
        }

        private Button MakeFilterButton(string label, Color accent)
        {
            var btn = new Button
            {
                Text = label,
                Width = 70,
                Height = 24,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8f),
                BackColor = Color.White,
                ForeColor = accent,
            };
            btn.FlatAppearance.BorderColor = accent;
            return btn;
        }

        private void BuildLegend(Panel parent, int x, int y)
        {
            var types = new[] {
                (EventType.Class,        "Class"),
                (EventType.Exam,         "Exam"),
                (EventType.Deadline,     "Deadline"),
                (EventType.Consultation, "Consult"),
                (EventType.Cancelled,    "Cancelled"),
            };

            var flp = new FlowLayoutPanel
            {
                Left = x,
                Top = y,
                Height = 48,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoSize = false,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            };
            flp.Width = Math.Max(10, parent.Width - x - 4);
            parent.Resize += (s, e) => flp.Width = Math.Max(10, parent.Width - x - 4);

            int cx = x;
            foreach (var (t, name) in types)
            {
                Color c = new CalendarEvent { Type = t }.GetColor();

                var chip = new Panel
                {
                    Height = 20,
                    BackColor = Color.Transparent,
                    Margin = new Padding(0, 2, 12, 2),
                };

                var dot = new Panel { Width = 10, Height = 10, Left = 0, Top = 5, BackColor = c };
                var lbl = new Label
                {
                    Text = name,
                    Left = 14,
                    Top = 0,
                    AutoSize = true,
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = Color.FromArgb(50, 50, 50),
                    Height = 20,
                };
                chip.Width = 14 + TextRenderer.MeasureText(name, lbl.Font).Width + 6;
                chip.Controls.Add(dot);
                chip.Controls.Add(lbl);
                flp.Controls.Add(chip);
            }

            parent.Controls.Add(flp);
        }

        private void QuickAddEventForSelected()
        {
            using var dlg = new AddEventForm(_lastSelectedDate);
            if (dlg.ShowDialog() == DialogResult.OK && dlg.CreatedEvent != null)
            {
                SharedCalendarData.AddEvent(_lastSelectedDate, dlg.CreatedEvent);
                foreach (Control ctrl in FPLmonth.Controls)
                    if (ctrl is UrDay ud) ud.RefreshEventPills();

                RefreshDayDetail(_lastSelectedDate);
                RefreshUpcoming();
            }
        }

        private void OnDaySelected(DateTime date)
        {
            _lastSelectedDate = date;
            foreach (Control ctrl in FPLmonth.Controls)
                if (ctrl is UrDay ud) ud.IsSelected = (ud.CellDate == date);
            RefreshDayDetail(date);
            RefreshUpcoming();
        }

        private void RefreshDayDetail(DateTime date)
        {
            _lastSelectedDate = date;
            if (lblSelectedDate != null)
                lblSelectedDate.Text = date.ToString("dddd, MMMM dd, yyyy");

            if (flpDayEvents == null) return;
            flpDayEvents.Controls.Clear();

            var events = SharedCalendarData.GetEventsForDate(date)
                .Where(ev => _activeFilter == null || ev.Type == _activeFilter)
                .ToList();

            if (notesDict.ContainsKey(date.Date) && !string.IsNullOrWhiteSpace(notesDict[date.Date]))
            {
                flpDayEvents.Controls.Add(MakeEventCard(
                    "🗒 Note", notesDict[date.Date],
                    Color.FromArgb(100, 100, 100), date, null));
            }

            if (events.Count == 0 && flpDayEvents.Controls.Count == 0)
            {
                flpDayEvents.Controls.Add(lblNoEvents);
                return;
            }
            else
            {
                foreach (var ev in events)
                {
                    string body = "";
                    if (!string.IsNullOrEmpty(ev.StartTime)) body += ev.StartTime + (string.IsNullOrEmpty(ev.EndTime) ? "" : " – " + ev.EndTime) + "\n";
                    if (!string.IsNullOrEmpty(ev.Room)) body += "Room: " + ev.Room + "\n";
                    body += ev.Description;
                    flpDayEvents.Controls.Add(MakeEventCard($"[{ev.GetTypeLabel()}]  {ev.Title}", body.Trim(), ev.GetColor(), date, ev));
                }
            }

            flpDayEvents.HorizontalScroll.Maximum = 0;
            flpDayEvents.HorizontalScroll.Enabled = false;
            flpDayEvents.HorizontalScroll.Visible = false;
        }

        private void RefreshUpcoming()
        {
            if (flpUpcoming == null) return;
            flpUpcoming.Controls.Clear();
            var upcoming = SharedCalendarData.GetUpcoming(6);
            if (upcoming.Count == 0) { flpUpcoming.Controls.Add(lblNoUpcoming); }
            else
            {
                foreach (var (d, ev) in upcoming)
                {
                    int daysLeft = (d.Date - DateTime.Now.Date).Days;
                    string when = daysLeft == 0 ? "Today" : daysLeft == 1 ? "Tomorrow" : $"In {daysLeft} days";
                    flpUpcoming.Controls.Add(MakeUpcomingStrip(ev, d, when));
                }
            }
            flpUpcoming.HorizontalScroll.Maximum = 0;
            flpUpcoming.HorizontalScroll.Enabled = false;
            flpUpcoming.HorizontalScroll.Visible = false;
        }

        private Panel MakeEventCard(string title, string body, Color accent, DateTime date, CalendarEvent ev)
        {
            int flpW = flpDayEvents.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;
            int cardW = Math.Max(80, flpW > 20 ? flpW : 440);
            const int HDR_H = 32;
            const int BDY_H = 44;

            var card = new Panel
            {
                Width = cardW,
                Height = HDR_H,
                BackColor = Color.FromArgb(245, 248, 255),
                Margin = new Padding(0, 2, 0, 0),
                Tag = false,
            };

            var bar = new Panel { Width = 5, Height = HDR_H + BDY_H, Left = 0, Top = 0, BackColor = accent };
            card.Controls.Add(bar);

            var lblT = new Label
            {
                Text = title,
                Left = 12,
                Top = 7,
                Width = cardW - 58,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 40, 40),
                AutoSize = false,
                Height = 18,
                Cursor = Cursors.Hand,
            };
            card.Controls.Add(lblT);

            var btnToggle = new Button
            {
                Text = "▾",
                Left = cardW - 46,
                Top = 6,
                Width = 22,
                Height = 20,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(100, 100, 100),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
            };
            btnToggle.FlatAppearance.BorderSize = 0;
            card.Controls.Add(btnToggle);

            var lblB = new Label
            {
                Text = string.IsNullOrWhiteSpace(body) ? "(no details)" : body,
                Left = 12,
                Top = HDR_H + 4,
                Width = cardW - (ev != null ? 64 : 24),
                Font = new Font("Segoe UI", 8f),
                ForeColor = Color.FromArgb(90, 90, 90),
                AutoSize = false,
                Height = BDY_H - 8,
                AutoEllipsis = true,
                Visible = false,
            };
            card.Controls.Add(lblB);

            if (ev != null)
            {
                var btnDel = new Button
                {
                    Text = "✕",
                    Left = cardW - 24,
                    Top = 6,
                    Width = 20,
                    Height = 20,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.Gray,
                    BackColor = Color.Transparent,
                    Font = new Font("Segoe UI", 8f),
                    Cursor = Cursors.Hand,
                };
                btnDel.FlatAppearance.BorderSize = 0;
                var capturedEv = ev;
                btnDel.Click += (s, e) =>
                {
                    if (MessageBox.Show($"Remove '{capturedEv.Title}'?", "Confirm",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SharedCalendarData.RemoveEvent(date, capturedEv);
                        foreach (Control ctrl in FPLmonth.Controls)
                            if (ctrl is UrDay ud) ud.RefreshEventPills();
                        RefreshDayDetail(date);
                        RefreshUpcoming();
                    }
                };
                card.Controls.Add(btnDel);
            }

            void Toggle()
            {
                bool expanded = (bool)card.Tag;
                expanded = !expanded;
                card.Tag = expanded;
                card.Height = expanded ? HDR_H + BDY_H : HDR_H;
                bar.Height = card.Height;
                lblB.Visible = expanded;
                btnToggle.Text = expanded ? "▴" : "▾";
                flpDayEvents.HorizontalScroll.Maximum = 0;
                flpDayEvents.HorizontalScroll.Enabled = false;
                flpDayEvents.HorizontalScroll.Visible = false;
            }

            btnToggle.Click += (s, e) => Toggle();
            lblT.Click += (s, e) => Toggle();

            return card;
        }

        private Panel MakeUpcomingStrip(CalendarEvent ev, DateTime date, string when)
        {
            int w = Math.Max(80, flpUpcoming.ClientSize.Width - SystemInformation.VerticalScrollBarWidth);
            var strip = new Panel
            {
                Width = w,
                Height = 36,
                BackColor = Color.White,
                Margin = new Padding(0, 2, 0, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            };
            var dot = new Panel { Width = 8, Height = 8, Top = 14, Left = 4, BackColor = ev.GetColor() };
            var lblT = new Label { Text = ev.Title, Left = 18, Top = 2, Width = w - 76, Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = Color.FromArgb(40, 40, 40), AutoSize = false, Height = 18, AutoEllipsis = true };
            var lblW = new Label { Text = when, Left = 18, Top = 20, Width = w - 76, Font = new Font("Segoe UI", 7.5f), ForeColor = Color.Gray, AutoSize = false, Height = 14 };
            var lblD = new Label { Text = date.ToString("MMM dd"), Left = w - 56, Top = 10, Width = 52, Font = new Font("Segoe UI", 8f), ForeColor = Color.FromArgb(90, 90, 90), AutoSize = false, TextAlign = ContentAlignment.MiddleRight };
            strip.Controls.AddRange(new Control[] { dot, lblT, lblW, lblD });
            return strip;
        }

        private void PositionBottomPanel()
        {
            if (pnlBottom == null || pnlCalendar == null) return;
            pnlBottom.Width = pnlCalendar.ClientSize.Width;
            pnlBottom.Top = pnlCalendar.ClientSize.Height - pnlBottom.Height;
            pnlBottom.Left = 0;

            foreach (Control c in pnlBottom.Controls)
                if (c is Panel rp && rp.Left > 530)
                    rp.Width = pnlBottom.Width - rp.Left - 8;

            if (flpDayEvents != null)
            {
                var parent = flpDayEvents.Parent;
                if (parent != null)
                    flpDayEvents.Width = parent.ClientSize.Width;
                flpDayEvents.HorizontalScroll.Maximum = 0;
                flpDayEvents.HorizontalScroll.Enabled = false;
                flpDayEvents.HorizontalScroll.Visible = false;
            }

            if (flpUpcoming != null)
            {
                var parent = flpUpcoming.Parent;
                if (parent != null)
                    flpUpcoming.Width = parent.ClientSize.Width;

                int stripW = Math.Max(80, flpUpcoming.ClientSize.Width - SystemInformation.VerticalScrollBarWidth);
                foreach (Control c in flpUpcoming.Controls)
                {
                    c.Width = stripW;

                    if (c is Panel strip)
                    {
                        foreach (Control child in strip.Controls)
                        {
                            if (child is Label lbl && lbl.TextAlign == ContentAlignment.MiddleRight)
                            {
                                lbl.Left = stripW - 56;
                                lbl.Width = 52;
                            }
                            else if (child is Label lblBody && lblBody.Left == 18)
                            {
                                child.Width = stripW - 76;
                            }
                        }
                    }
                }

                flpUpcoming.HorizontalScroll.Maximum = 0;
                flpUpcoming.HorizontalScroll.Enabled = false;
                flpUpcoming.HorizontalScroll.Visible = false;
            }
        }

        private void BuildDayHeaders()
        {
            string[] days = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            pnlDayHeaders = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Height = 32,
                Padding = new Padding(0),
                Margin = new Padding(0),
                BackColor = Color.FromArgb(245, 245, 245),
            };
            pnlDayHeaders.Parent = FPLmonth.Parent;
            pnlDayHeaders.Left = FPLmonth.Left;
            pnlDayHeaders.Width = FPLmonth.Width;
            pnlDayHeaders.Top = FPLmonth.Top - 32;
            pnlDayHeaders.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            foreach (string d in days)
                pnlDayHeaders.Controls.Add(new Label
                {
                    Text = d,
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = false,
                    Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                    ForeColor = d == "Sunday" ? Color.Crimson : Color.FromArgb(80, 80, 80),
                    BackColor = Color.Transparent,
                    Margin = new Padding(1, 0, 1, 0),
                    Height = 32,
                });
            pnlDayHeaders.BringToFront();
        }

        private void AlignDayHeaders()
        {
            if (pnlDayHeaders == null) return;
            pnlDayHeaders.Left = FPLmonth.Left;
            pnlDayHeaders.Width = FPLmonth.Width;
            pnlDayHeaders.Top = FPLmonth.Top - pnlDayHeaders.Height;
            int available = FPLmonth.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 2;
            int cellWidth = available / 7;
            foreach (Control ctrl in pnlDayHeaders.Controls)
            {
                ctrl.Width = cellWidth;
            }
        }

        private void showDays(int month, int year)
        {
            FPLmonth.Controls.Clear();
            _year = year; _month = month;
            SharedCalendarData.CurrentYear = year;
            SharedCalendarData.CurrentMonth = month;

            string monthName = new DateTimeFormatInfo().GetMonthName(month);
            lblMonthYear.Text = monthName.ToUpper() + " " + year;
            CenterMonthLabel();

            DateTime firstDay = new DateTime(year, month, 1);
            int startDOW = (int)firstDay.DayOfWeek;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            if (startDOW > 0)
            {
                DateTime prev = firstDay.AddMonths(-1);
                int prevDays = DateTime.DaysInMonth(prev.Year, prev.Month);
                for (int i = startDOW - 1; i >= 0; i--)
                {
                    int d = prevDays - i;
                    FPLmonth.Controls.Add(new UrDay(d.ToString(), prev.Year, prev.Month, false, GetHoliday(prev.Year, prev.Month, d), isStudent: false));
                }
            }

            for (int i = 1; i <= daysInMonth; i++)
                FPLmonth.Controls.Add(new UrDay(i.ToString(), year, month, true, GetHoliday(year, month, i), isStudent: false));

            int total = FPLmonth.Controls.Count;
            int remainder = total % 7;
            if (remainder > 0)
            {
                DateTime next = firstDay.AddMonths(1);
                for (int i = 1; i <= 7 - remainder; i++)
                    FPLmonth.Controls.Add(new UrDay(i.ToString(), next.Year, next.Month, false, GetHoliday(next.Year, next.Month, i), isStudent: false));
            }

            ResizeCalendarCells();
        }

        private string GetHoliday(int year, int month, int day)
        {
            var date = new DateTime(year, month, day);
            return SharedCalendarData.Holidays.ContainsKey(date) ? SharedCalendarData.Holidays[date] : "";
        }

        private void ResizeCalendarCells()
        {
            if (FPLmonth.Controls.Count == 0) return;
            int available = FPLmonth.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 2;
            int cellWidth = available / 7;
            foreach (Control ctrl in FPLmonth.Controls)
            {
                ctrl.Width = cellWidth;
                ctrl.Height = 110;
                ctrl.Margin = new Padding(1);
            }
            AlignDayHeaders();
        }

        private void OnFormResized(object sender, EventArgs e)
        {
            if (!this.IsHandleCreated || this.IsDisposed) return;
            this.BeginInvoke((Action)(() =>
            {
                try { FitCalendarPanel(); ResizeCalendarCells(); CenterMonthLabel(); PositionBottomPanel(); }
                catch { }
            }));
        }

        private void FitCalendarPanel()
        {
            const int BOTTOM_H = 220;
            if (FPLmonth != null)
            {
                int headerBottom = pnlDayHeaders != null
                    ? pnlDayHeaders.Top + pnlDayHeaders.Height : FPLmonth.Top;
                FPLmonth.Width = pnlCalendar.ClientSize.Width - FPLmonth.Left - 4;
                FPLmonth.Top = headerBottom;
                FPLmonth.Height = pnlCalendar.ClientSize.Height - FPLmonth.Top - BOTTOM_H - 4;
            }
        }

        private void CenterMonthLabel()
        {
            if (lblMonthYear == null || pnlCalendar == null) return;
            lblMonthYear.AutoSize = false;
            lblMonthYear.TextAlign = ContentAlignment.MiddleCenter;
            lblMonthYear.Width = pnlCalendar.ClientSize.Width;
            lblMonthYear.Left = 0;
        }

        private void picNext_Click(object sender, EventArgs e) { _month++; if (_month > 12) { _month = 1; _year++; } showDays(_month, _year); }
        private void picPrev_Click(object sender, EventArgs e) { _month--; if (_month < 1) { _month = 12; _year--; } showDays(_month, _year); }

    }
}
