using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class StudentPortal : Form
    {
        public static int _year, _month;
        private FlowLayoutPanel pnlDayHeaders;

        private Button clickedButton;
        private Color defaultColor = Color.Maroon;
        private Color selectedColor = Color.FromArgb(109, 0, 0);

        private Panel pnlBottom;
        private Label lblSelectedDate;
        private FlowLayoutPanel flpDayEvents;
        private FlowLayoutPanel flpUpcoming;
        private Label lblNoEvents;
        private Label lblNoUpcoming;

        private EventType? _activeFilter = null;

        private UrDay _selectedCell;

        public StudentPortal()
        {
            InitializeComponent();
            SharedCalendarData.LoadData();
            BuildDayHeaders();
            BuildBottomPanel();

            FPLmonth.Resize += (s, ev) => { ResizeCalendarCells(); AlignDayHeaders(); };
            this.Resize += OnFormResized;

            showDays(DateTime.Now.Month, DateTime.Now.Year);

            UrDay.DaySelected += OnDaySelected;

            OnDaySelected(DateTime.Now.Date);

            var wheelFilter = new CalendarWheelFilter(FPLmonth, delta =>
            {
                if (delta > 0) picPrev_Click(this, EventArgs.Empty);
                else picNext_Click(this, EventArgs.Empty);
            });
            Application.AddMessageFilter(wheelFilter);

            this.FormClosed += (s, ev) =>
            {
                UrDay.DaySelected -= OnDaySelected;
                Application.RemoveMessageFilter(wheelFilter);
                this.Resize -= OnFormResized;
            };
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

            var pnlLeft = new Panel
            {
                Left = 0,
                Top = 4,
                Width = 480,
                Height = BOTTOM_H - 8,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom,
                Name = "pnlLeft",
            };
            pnlBottom.Controls.Add(pnlLeft);

            lblSelectedDate = new Label
            {
                Text = "Select a day to see details",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.Maroon,
                Left = 12,
                Top = 6,
                AutoSize = true,
            };
            pnlLeft.Controls.Add(lblSelectedDate);

            var btnAddMyEvent = new Button
            {
                Text = "+ Add My Event",
                Left = 12,
                Top = 28,
                Width = 105,
                Height = 26,
                BackColor = Color.FromArgb(66, 133, 244),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5f),
                Cursor = Cursors.Hand,
            };
            btnAddMyEvent.FlatAppearance.BorderSize = 0;
            btnAddMyEvent.Click += (s, e) => QuickAddStudentEvent();
            pnlLeft.Controls.Add(btnAddMyEvent);

            int bx = btnAddMyEvent.Right + 8;
            var filters = new[] {
                ("All",      (EventType?)null),
                ("Class",    (EventType?)EventType.Class),
                ("Exam",     (EventType?)EventType.Exam),
                ("Deadline", (EventType?)EventType.Deadline),
            };
            foreach (var (label, ft) in filters)
            {
                var captured = ft;
                var btn = MakeFilterButton(label,
                    ft == null ? Color.FromArgb(90, 90, 90) : new CalendarEvent { Type = ft.Value }.GetColor());
                btn.Left = bx; btn.Top = 30;
                btn.Click += (s, e) =>
                {
                    _activeFilter = captured;
                    RefreshDayDetail(_selectedCell != null ? GetSelectedDate() : DateTime.Now.Date);
                    HighlightActiveFilter();
                };
                pnlLeft.Controls.Add(btn);
                bx += btn.Width + 6;
            }

            flpDayEvents = new FlowLayoutPanel
            {
                Left = 0,
                Top = 62,
                Width = 468,
                Height = BOTTOM_H - 70,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
            };
            lblNoEvents = new Label { Text = "No events for this day.", ForeColor = Color.Gray, AutoSize = true, Padding = new Padding(8) };
            flpDayEvents.Controls.Add(lblNoEvents);
            pnlLeft.Controls.Add(flpDayEvents);

            var div = new Panel
            {
                Left = 490,
                Top = 8,
                Width = 1,
                Height = BOTTOM_H - 16,
                BackColor = Color.FromArgb(220, 220, 220),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left,
            };
            pnlBottom.Controls.Add(div);

            var pnlRight = new Panel
            {
                Left = 498,
                Top = 4,
                Height = BOTTOM_H - 8,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
            };
            pnlBottom.Controls.Add(pnlRight);

            var lblUpcomingTitle = new Label
            {
                Text = "Upcoming",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                Left = 8,
                Top = 6,
                AutoSize = true,
            };
            pnlRight.Controls.Add(lblUpcomingTitle);

            BuildLegend(pnlRight, 8, 30);

            flpUpcoming = new FlowLayoutPanel
            {
                Left = 0,
                Top = 80,
                Width = 400,
                Height = BOTTOM_H - 90,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
            };
            lblNoUpcoming = new Label { Text = "No upcoming events.", ForeColor = Color.Gray, AutoSize = true, Padding = new Padding(8) };
            flpUpcoming.Controls.Add(lblNoUpcoming);
            pnlRight.Controls.Add(flpUpcoming);

            RefreshUpcoming();
            PositionBottomPanel();
        }
        private void QuickAddStudentEvent()
        {
            using var dlg = new AddEventForm(_lastSelectedDate);
            if (dlg.ShowDialog() == DialogResult.OK && dlg.CreatedEvent != null)
            {
                SharedCalendarData.AddStudentEvent(_lastSelectedDate, dlg.CreatedEvent);

                foreach (Control ctrl in FPLmonth.Controls)
                    if (ctrl is UrDay ud) ud.RefreshEventPills();

                RefreshDayDetail(_lastSelectedDate);
                RefreshUpcoming();
            }
        }

        private Button MakeFilterButton(string label, Color accent)
        {
            var btn = new Button
            {
                Text = label,
                Width = 72,
                Height = 24,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8f),
                BackColor = Color.White,
                ForeColor = accent,
                Tag = label,
            };
            btn.FlatAppearance.BorderColor = accent;
            return btn;
        }

        private void HighlightActiveFilter()
        {
            var found = pnlBottom.Controls.Find("pnlLeft", true);
            if (found == null || found.Length == 0) return;

            var pnlLeft = found[0];
            foreach (Control c in pnlLeft.Controls)
            {
                if (c is Button b && b.Tag != null)
                {
                    bool active = (_activeFilter == null && b.Tag.ToString() == "All")
                               || (_activeFilter != null && b.Tag.ToString() == _activeFilter.ToString());

                    b.BackColor = active ? b.ForeColor : Color.White;
                    b.ForeColor = active ? Color.White : b.FlatAppearance.BorderColor;
                }
            }
        }

        private void BuildLegend(Panel parent, int x, int y)
        {
            var types = new[] {
                (EventType.Class,        "Class"),
                (EventType.Exam,         "Exam"),
                (EventType.Deadline,     "Deadline"),
                (EventType.Consultation, "Consult"),
            };

            int cx = x;
            foreach (var (t, name) in types)
            {
                Color c = new CalendarEvent { Type = t }.GetColor();
                var dot = new Panel { Width = 10, Height = 10, BackColor = c, Left = cx, Top = y + 2 };
                var lbl = new Label { Text = name, Left = cx + 13, Top = y, AutoSize = true, Font = new Font("Segoe UI", 8f), ForeColor = Color.FromArgb(60, 60, 60) };
                parent.Controls.Add(dot);
                parent.Controls.Add(lbl);
                cx += 70;
            }
        }

        private void OnDaySelected(DateTime date)
        {
            foreach (Control ctrl in FPLmonth.Controls)
                if (ctrl is UrDay ud) ud.IsSelected = false;

            RefreshDayDetail(date);
            RefreshUpcoming();
        }

        private DateTime GetSelectedDate() => _lastSelectedDate;

        private DateTime _lastSelectedDate = DateTime.Now.Date;

        private void RefreshDayDetail(DateTime date)
        {
            _lastSelectedDate = date;
            lblSelectedDate.Text = date.ToString("dddd, MMMM dd, yyyy");

            flpDayEvents.Controls.Clear();

            var sharedEvents = SharedCalendarData.GetEventsForDate(date)
                .Where(ev => _activeFilter == null || ev.Type == _activeFilter)
                .ToList();

            var personalEvents = SharedCalendarData.GetStudentEventsForDate(date)
                .Where(ev => _activeFilter == null || ev.Type == _activeFilter)
                .ToList();

            var noteDict = SharedCalendarData.StudentNotes;
            if (noteDict.ContainsKey(date.Date) && !string.IsNullOrWhiteSpace(noteDict[date.Date]))
                flpDayEvents.Controls.Add(
                    MakeEventCard("🗒 Note", noteDict[date.Date], Color.FromArgb(100, 100, 100), date, isPersonal: false));

            if (sharedEvents.Count == 0 && personalEvents.Count == 0 && flpDayEvents.Controls.Count == 0)
            {
                flpDayEvents.Controls.Add(lblNoEvents);
                return;
            }

            foreach (var ev in sharedEvents)
            {
                string body = (string.IsNullOrEmpty(ev.StartTime) ? "" : ev.StartTime + (string.IsNullOrEmpty(ev.EndTime) ? "" : " – " + ev.EndTime) + "\n")
                            + (string.IsNullOrEmpty(ev.Room) ? "" : "Room: " + ev.Room + "\n")
                            + ev.Description;
                flpDayEvents.Controls.Add(
                    MakeEventCard($"[{ev.GetTypeLabel()}]  {ev.Title}", body, ev.GetColor(), date, ev, isPersonal: false));
            }

            if (personalEvents.Count > 0 && sharedEvents.Count > 0)
            {
                flpDayEvents.Controls.Add(new Label
                {
                    Text = "── My Personal Events ──",
                    Font = new Font("Segoe UI", 7.5f, FontStyle.Italic),
                    ForeColor = Color.FromArgb(120, 120, 120),
                    AutoSize = true,
                    Padding = new Padding(6, 4, 0, 0),
                });
            }

            foreach (var ev in personalEvents)
            {
                string body = (string.IsNullOrEmpty(ev.StartTime) ? "" : ev.StartTime + (string.IsNullOrEmpty(ev.EndTime) ? "" : " – " + ev.EndTime) + "\n")
                            + (string.IsNullOrEmpty(ev.Room) ? "" : "Room: " + ev.Room + "\n")
                            + ev.Description;
                flpDayEvents.Controls.Add(
                    MakeEventCard($"🔒 [MY {ev.GetTypeLabel()}]  {ev.Title}", body, ev.GetColor(), date, ev, isPersonal: true));
            }
        }

        private void RefreshUpcoming()
        {
            flpUpcoming.Controls.Clear();

            var upcoming = SharedCalendarData.GetUpcoming(6, includeStudentEvents: true);

            if (upcoming.Count == 0)
            {
                flpUpcoming.Controls.Add(lblNoUpcoming);
                return;
            }

            foreach (var (d, ev) in upcoming)
            {
                int daysLeft = (d.Date - DateTime.Now.Date).Days;
                string when = daysLeft == 0 ? "Today" : daysLeft == 1 ? "Tomorrow" : $"In {daysLeft} days";

                bool isPersonal = SharedCalendarData.GetStudentEventsForDate(d).Contains(ev);
                flpUpcoming.Controls.Add(MakeUpcomingStrip(ev, d, when, isPersonal));
            }
        }

        private Panel MakeEventCard(
            string title,
            string body,
            Color accent,
            DateTime date,
            CalendarEvent ev = null,
            bool isPersonal = false)
        {
            Color bg = isPersonal ? Color.FromArgb(240, 248, 255) : Color.FromArgb(245, 248, 255);

            var card = new Panel
            {
                Width = flpDayEvents.Width - 16,
                Height = 48,
                BackColor = bg,
                Margin = new Padding(4, 3, 4, 0),
                Cursor = Cursors.Default,
            };

            Color barColor = isPersonal ? Color.FromArgb(160, accent.R, accent.G, accent.B) : accent;
            var bar = new Panel { Width = 5, Height = card.Height, Left = 0, Top = 0, BackColor = barColor };

            var lblTitle = new Label
            {
                Text = title,
                Left = 12,
                Top = 3,
                Width = card.Width - (isPersonal ? 60 : 20),
                Font = new Font("Segoe UI", 8.5f, isPersonal ? FontStyle.Italic : FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 40, 40),
                AutoSize = false,
                Height = 18,
            };
            var lblBody = new Label
            {
                Text = body.Trim(),
                Left = 12,
                Top = 22,
                Width = card.Width - (isPersonal ? 60 : 20),
                Font = new Font("Segoe UI", 8f),
                ForeColor = Color.FromArgb(90, 90, 90),
                AutoSize = false,
                Height = 22,
                AutoEllipsis = true,
            };

            card.Controls.AddRange(new Control[] { bar, lblTitle, lblBody });

            if (isPersonal && ev != null)
            {
                var btnDel = new Button
                {
                    Text = "✕",
                    Left = card.Width - 30,
                    Top = 12,
                    Width = 22,
                    Height = 22,
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
                        SharedCalendarData.RemoveStudentEvent(date, capturedEv);
                        foreach (Control ctrl in FPLmonth.Controls)
                            if (ctrl is UrDay ud) ud.RefreshEventPills();
                        RefreshDayDetail(date);
                        RefreshUpcoming();
                    }
                };
                card.Controls.Add(btnDel);
            }

            if (ev == null && !isPersonal)
            {
                card.Cursor = Cursors.Hand;
                card.Click += (s, e) => OnDaySelected(date);
            }

            return card;
        }

        private Panel MakeUpcomingStrip(CalendarEvent ev, DateTime date, string when, bool isPersonal = false)
        {
            var strip = new Panel
            {
                Width = flpUpcoming.Width - 8,
                Height = 36,
                BackColor = isPersonal ? Color.FromArgb(245, 250, 255) : Color.White,
                Margin = new Padding(4, 2, 4, 0),
            };

            Color dotColor = isPersonal ? Color.FromArgb(150, ev.GetColor().R, ev.GetColor().G, ev.GetColor().B) : ev.GetColor();
            var dot = new Panel { Width = 8, Height = 8, Top = 14, Left = 4, BackColor = dotColor };

            string displayTitle = isPersonal ? "🔒 " + ev.Title : ev.Title;
            var lblT = new Label { Text = displayTitle, Left = 18, Top = 2, Width = strip.Width - 80, Font = new Font("Segoe UI", 8.5f, isPersonal ? FontStyle.Italic : FontStyle.Bold), ForeColor = Color.FromArgb(40, 40, 40), AutoSize = false, Height = 18, AutoEllipsis = true };
            var lblW = new Label { Text = when, Left = 18, Top = 20, Width = strip.Width - 80, Font = new Font("Segoe UI", 7.5f), ForeColor = Color.Gray, AutoSize = false, Height = 14 };
            var lblD = new Label { Text = date.ToString("MMM dd"), Left = strip.Width - 58, Top = 10, Width = 54, Font = new Font("Segoe UI", 8f), ForeColor = Color.FromArgb(90, 90, 90), AutoSize = false, TextAlign = ContentAlignment.MiddleRight };
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
                if (c is Panel rp && rp.Left > 490)
                    rp.Width = pnlBottom.Width - rp.Left - 8;
        }

        private void changeButtonColor(Button button)
        {
            if (clickedButton != null)
            {
                clickedButton.BackColor = defaultColor;
            }
            clickedButton = button;
            pnlYellow.Visible = true;
            pnlYellow.Parent = clickedButton.Parent;
            pnlYellow.BringToFront();
            clickedButton.BackColor = selectedColor;
        }

        //Method na nagpapakita ng content ng bawat button, wala akong maisip na iba kaya eto
        private void showContent(Button button)
        {
            Dictionary<Button, Panel> contents = new Dictionary<Button, Panel> { };
            contents.Add(btnDashboard, pnlDashboardContent);
            contents.Add(btnEnrollment, pnlEnrollContent);
            contents.Add(btnCourses, pnlCoursesContent);
            contents.Add(btnAccounts, pnlAccountsContent);
            //Kada button na aadd, maglagay ng panel sa form at lagay dito
            foreach (KeyValuePair<Button, Panel> content in contents)
            {
                if (content.Key == clickedButton)
                {
                    //Automatic positioning, wag pakialaman maliban nalang kung binago ang position ng sidebar
                    content.Value.Location = new Point(pnlSidebar.Size.Width, pnlHeader.Size.Height);
                    content.Value.Visible = true;
                }
                else
                {
                    content.Value.Visible = false;
                }
            }
        }

        //Method para pag pinindot yung X sa taas o mag alt-F4, icclose lahat ng forms para di magerror pag ni run uli
        //Lagay to sa bawat form na iaadd, Step 1: Hanapin sa properties ng form yung event na FormClosing, Step 2: Double click para gumawa ng method, Step 3: Copy paste code na nasa loob nito
        private void StudentPortal_Closing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Are you sure you want to exit?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                    Application.Exit();
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
                    Font = new Font("Segoe UI", 9f),
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
            foreach (Control ctrl in pnlDayHeaders.Controls) ctrl.Width = cellWidth;
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
                    FPLmonth.Controls.Add(new UrDay(d.ToString(), prev.Year, prev.Month, false, GetHoliday(prev.Year, prev.Month, d), isStudent: true));
                }
            }

            for (int i = 1; i <= daysInMonth; i++)
                FPLmonth.Controls.Add(new UrDay(i.ToString(), year, month, true, GetHoliday(year, month, i), isStudent: true));

            int total = FPLmonth.Controls.Count;
            int remainder = total % 7;
            if (remainder > 0)
            {
                DateTime next = firstDay.AddMonths(1);
                for (int i = 1; i <= 7 - remainder; i++)
                    FPLmonth.Controls.Add(new UrDay(i.ToString(), next.Year, next.Month, false, GetHoliday(next.Year, next.Month, i), isStudent: true));
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
            FPLmonth.SuspendLayout();
            foreach (Control ctrl in FPLmonth.Controls)
            {
                ctrl.Width = cellWidth;
                ctrl.Height = 110;
                ctrl.Margin = new Padding(1);
            }
            FPLmonth.ResumeLayout();
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
            if (pnlCalendar == null || !pnlCalendar.Visible) return;
            if (pnlSidebar == null || pnlHeader == null) return;

            pnlCalendar.Left = pnlSidebar.Width;
            pnlCalendar.Top = pnlHeader.Height;
            pnlCalendar.Width = this.ClientSize.Width - pnlSidebar.Width;
            pnlCalendar.Height = this.ClientSize.Height - pnlHeader.Height;

            const int BOTTOM_H = 220;
            if (FPLmonth != null)
            {
                int headerBottom = pnlDayHeaders != null
                    ? pnlDayHeaders.Top + pnlDayHeaders.Height
                    : FPLmonth.Top;

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

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnEnrollment_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnCourses_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnAccounts_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }
        private void btnLMS_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
            pnllmsSubmenu.Visible = !pnllmsSubmenu.Visible;
            if (pnllmsSubmenu.Visible)
                btnLMS.Text = " LMS                                       ⌄";
            else
                btnLMS.Text = " LMS                                        ›";
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAnnounce_Click(object sender, EventArgs e)
        {
            pnlAnnounce.BringToFront();
            pnlAnnounce.Visible = true;
        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            pnlSubject.Visible = false;
            pnlCalendar.BringToFront();
            pnlCalendar.Visible = true;
        }

        private void btnSubject_Click(object sender, EventArgs e)
        {
            pnlSubject.BringToFront();
            pnlSubject.Visible = true;
        }

        private void btnActivities_Click(object sender, EventArgs e)
        {
            pnlActivities.BringToFront();
            pnlActivities.Visible = true;
        }

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            pnlAttendance.BringToFront();
            pnlAttendance.Visible = true;
        }

        private void btnGrade_Click(object sender, EventArgs e)
        {
            pnlGrades.BringToFront();
            pnlGrades.Visible = true;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            pnlSubject.BringToFront();
            pnlSubject.Visible = true;
        }

        private void btnStudFiles_Click(object sender, EventArgs e)
        {

            pnlLMSFiles.BringToFront();
            pnlLMSFiles.Visible = true;
        }

        private void btnStudAct_Click(object sender, EventArgs e)
        {
            pnlLMSFiles.Visible = false;
            pnlLMSActivities.BringToFront();
            pnlLMSActivities.Visible = true;
        }

        private void pnlAct1_MouseEnter(object sender, EventArgs e)
        {
            // Change to a light highlight color (e.g., Light Gray or Light Maroon)
            pnlAct1.BackColor = Color.FromArgb(128, 0, 0);
            pnlAct1.Cursor = Cursors.Hand; // Shows the clicking hand icon
        }

        private void pnlAct1_MouseLeave(object sender, EventArgs e)
        {
            // Change back to the original background color (usually White)
            pnlAct1.BackColor = Color.White;
        }

        private void pnlAct1_Click(object sender, EventArgs e)
        {
            // 1. Hide the panel that contains the question/choices
            // Replace 'pnlQuestionArea' with the actual parent container name
            pnlAnsAct1.Visible = false;

            // 2. Show the Answer Action panel
            pnlAnsAct1.Visible = true;
            pnlAnsAct1.BringToFront();
            pnlAnsAct1.Dock = DockStyle.Fill; // Optional: ensures it fills the space
        }

        private void btnCancelAct_Click(object sender, EventArgs e)
        {
            pnlLMSActivities.BringToFront();
            pnlLMSActivities.Visible = true;
        }

        private void btnCancelAct_Click_1(object sender, EventArgs e)
        {
            pnlLMSActivities.BringToFront();
            pnlLMSActivities.Visible = true;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            pnlLMSActivities.BringToFront();
            pnlLMSActivities.Visible = true;
        }

        private void btnAssignAttach_Click(object sender, EventArgs e)
        {
            pnlAttachAss.BringToFront();
            pnlAttachAss.Visible = true;
        }

        private void btnCancelAssign_Click(object sender, EventArgs e)
        {
            pnlLMSActivities.BringToFront(); pnlLMSActivities.Visible = true;
        }

        private void btnSaveAss_Click(object sender, EventArgs e)
        {
            pnlLMSActivities.BringToFront(); pnlLMSActivities.Visible = true;
        }

        private void btnAttachCancel_Click(object sender, EventArgs e)
        {
            pnlAttachAss.Hide();
        }

        private void btnDoneAttach_Click(object sender, EventArgs e)
        {
            pnlAttachAss.Hide();
        }

        private void pnlAss1_MouseEnter(object sender, EventArgs e)
        {
            // Highlights to Maroon on hover
            pnlAss1.BackColor = Color.Maroon;
            pnlAss1.Cursor = Cursors.Hand;
        }

        private void pnlAss1_MouseLeave(object sender, EventArgs e)
        {
            // Changes back to White when the mouse leaves
            pnlAss1.BackColor = Color.White;
        }

        private void pnlAss1_Click(object sender, EventArgs e)
        {
            // 1. Immediately reset the color to White
            pnlAss1.BackColor = Color.White;

            // 2. Switch to the Answer panel
            pnlAnsAss.Visible = true;
            pnlAnsAss.BringToFront();

            // Optional: If pnlAss1 is inside a container that you hide:
            // pnlMainContainer.Visible = false;
        }

        private void roundedPanel14_MouseLeave(object sender, EventArgs e)// ung pangatlong panel sa activities ng courses
        {
            roundedPanel14.BackColor = Color.White;
        }

        private void roundedPanel14_MouseEnter(object sender, EventArgs e)// ung pangatlong panel sa activities ng courses
        {
            roundedPanel14.BackColor = Color.Maroon;
            roundedPanel14.Cursor = Cursors.Hand;
        }

        private void roundedPanel16_MouseEnter(object sender, EventArgs e)
        {
            roundedPanel16.BackColor = Color.Maroon;
            roundedPanel16.Cursor = Cursors.Hand;
        }

        private void roundedPanel16_MouseLeave(object sender, EventArgs e)
        {
            roundedPanel16.BackColor = Color.White;
        }

        private void btnGo1_Click_1(object sender, EventArgs e)
        {
            pnlLMSFiles.Visible = false;
            pnlSubMenu.BringToFront();
            pnlSubMenu.Visible = true;
            pnlLMSActivities.BringToFront();
            pnlLMSActivities.Visible = true;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            //Define the path to the source file in the application's local "Resources" folder
            string sourceFile = Path.Combine(Application.StartupPath, "Resources", "COG-MTECH.pdf");

            //Check if the source file exists
            if (!File.Exists(sourceFile))
            {
                MessageBox.Show("Error: COG-MTECH.pdf was not found in the Resources folder.",
                                "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //Initialize the SaveFileDialog to allow user to select the save location and filename
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                //Set file type filters, default filename and dialog title
                sfd.Filter = "PDF Documents (.pdf)|.pdf";
                sfd.FileName = "COG-MTECH.pdf";
                sfd.Title = "Save COG-MTECH Report";

                //Opens dialog and check if user clicked "Save"
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        //Perform the file copy operation to the user's specified location
                        //The parameter "true" allows overwriting existing files
                        File.Copy(sourceFile, sfd.FileName, true);

                        MessageBox.Show("COG-MTECH.pdf has been saved successfully!",
                                        "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            roundedPanel36.Visible = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            roundedPanel36.Visible = true;
            roundedPanel36.BringToFront();
        }

        private void roundedPanel36_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnViewGrades_Click(object sender, EventArgs e)
        {
            fpnlGradesList.Visible = !fpnlGradesList.Visible;
        }

        private void pnlGrades_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
