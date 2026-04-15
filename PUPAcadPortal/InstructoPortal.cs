using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class InstructorPortal : Form
    {
        public static int _year, _month;
        public static Dictionary<DateTime, string> notesDict = new Dictionary<DateTime, string>();
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
        private DateTime _lastSelectedDate = DateTime.Now.Date;
        private EventType? _activeFilter = null;

        public InstructorPortal()
        {
            InitializeComponent();
            SharedCalendarData.LoadData();

            pnlAttendance.AutoScrollMinSize = new Size(0, 1000);
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/dd/yyyy hh:mm tt";
        }

        private void changeButtonColor(Button button)
        {
            if (clickedButton != null)
                clickedButton.BackColor = defaultColor;
            clickedButton = button;
            pnlYellow.Visible = true;
            pnlYellow.Parent = clickedButton.Parent;
            pnlYellow.BringToFront();
            clickedButton.BackColor = selectedColor;
        }

        private void showContent(Button button)
        {
            var contents = new Dictionary<Button, Panel>
            {
                { btnDashboard, pnlDashboardContent },
                { btnGrades,    pnlGradesContent    },
                { btnCourses,   pnlCoursesContent   },
            };
            foreach (var kv in contents)
            {
                bool show = kv.Key == clickedButton;
                kv.Value.Visible = show;
                if (show)
                    kv.Value.Location = new Point(pnlSidebar.Width, pnlHeader.Height);
            }
        }

        private void InstructorPortal_Load(object sender, EventArgs e)
        {
            listView_file.Font = new Font("Segoe UI", 11.5f);
            listView_file.View = View.Details;
            listView_file.FullRowSelect = true;
            listView_file.GridLines = false;

            imageList1.ImageSize = new Size(32, 32);
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            listView_file.SmallImageList = imageList1;

            listView_file.Columns.Clear();
            listView_file.Columns.Add("File Name", 250);
            listView_file.Columns.Add("Size", 100);
            listView_file.Columns.Add("Date Uploaded", 180);

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
            System.Windows.Forms.Application.AddMessageFilter(wheelFilter);

            this.FormClosed += (s, ev) =>
            {
                UrDay.DaySelected -= OnDaySelected;
                System.Windows.Forms.Application.RemoveMessageFilter(wheelFilter);
                this.Resize -= OnFormResized;
                SharedCalendarData.SaveData();
            };

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
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
            };
            lblNoEvents = new Label { Text = "No events for this day. Use '+ Add Event' to create one.", ForeColor = Color.Gray, AutoSize = true, Padding = new Padding(8) };
            flpDayEvents.Controls.Add(lblNoEvents);
            pnlLeft.Controls.Add(flpDayEvents);

            var div = new Panel { Left = 530, Top = 8, Width = 1, Height = BOTTOM_H - 16, BackColor = Color.FromArgb(220, 220, 220), Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left };
            pnlBottom.Controls.Add(div);
            var pnlRight = new Panel { Left = 538, Top = 4, Height = BOTTOM_H - 8, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom };
            pnlBottom.Controls.Add(pnlRight);

            var lblUpTitle = new Label { Text = "Upcoming", Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = Color.FromArgb(60, 60, 60), Left = 8, Top = 6, AutoSize = true };
            pnlRight.Controls.Add(lblUpTitle);

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
            int cx = x;
            foreach (var (t, name) in types)
            {
                Color c = new CalendarEvent { Type = t }.GetColor();
                var dot = new Panel { Width = 10, Height = 10, BackColor = c, Left = cx, Top = y + 2 };
                var lbl = new Label { Text = name, Left = cx + 13, Top = y, AutoSize = true, Font = new Font("Segoe UI", 8f), ForeColor = Color.FromArgb(60, 60, 60) };
                parent.Controls.Add(dot);
                parent.Controls.Add(lbl);
                cx += 75;
            }
        }

        private void QuickAddEventForSelected()
        {
            using var dlg = new AddEventForm(_lastSelectedDate);
            if (dlg.ShowDialog() == DialogResult.OK && dlg.CreatedEvent != null)
            {
                SharedCalendarData.AddEvent(_lastSelectedDate, dlg.CreatedEvent);
                // Refresh the UrDay cell pill
                foreach (Control ctrl in FPLmonth.Controls)
                    if (ctrl is UrDay ud) ud.RefreshEventPills();

                RefreshDayDetail(_lastSelectedDate);
                RefreshUpcoming();
            }
        }

        private void OnDaySelected(DateTime date)
        {
            _lastSelectedDate = date;
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

            foreach (var ev in events)
            {
                string body = "";
                if (!string.IsNullOrEmpty(ev.StartTime)) body += ev.StartTime + (string.IsNullOrEmpty(ev.EndTime) ? "" : " – " + ev.EndTime) + "\n";
                if (!string.IsNullOrEmpty(ev.Room)) body += "Room: " + ev.Room + "\n";
                body += ev.Description;
                flpDayEvents.Controls.Add(MakeEventCard($"[{ev.GetTypeLabel()}]  {ev.Title}", body.Trim(), ev.GetColor(), date, ev));
            }
        }

        private void RefreshUpcoming()
        {
            if (flpUpcoming == null) return;
            flpUpcoming.Controls.Clear();
            var upcoming = SharedCalendarData.GetUpcoming(6);
            if (upcoming.Count == 0) { flpUpcoming.Controls.Add(lblNoUpcoming); return; }
            foreach (var (d, ev) in upcoming)
            {
                int daysLeft = (d.Date - DateTime.Now.Date).Days;
                string when = daysLeft == 0 ? "Today" : daysLeft == 1 ? "Tomorrow" : $"In {daysLeft} days";
                flpUpcoming.Controls.Add(MakeUpcomingStrip(ev, d, when));
            }
        }

        private Panel MakeEventCard(string title, string body, Color accent, DateTime date, CalendarEvent ev)
        {
            var card = new Panel
            {
                Width = (flpDayEvents.Width > 20 ? flpDayEvents.Width : 460) - 16,
                Height = 48,
                BackColor = Color.FromArgb(245, 248, 255),
                Margin = new Padding(4, 3, 4, 0),
            };
            var bar = new Panel { Width = 5, Height = card.Height, Left = 0, Top = 0, BackColor = accent };
            var lblT = new Label { Text = title, Left = 12, Top = 3, Width = card.Width - 80, Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = Color.FromArgb(40, 40, 40), AutoSize = false, Height = 18 };
            var lblB = new Label { Text = body, Left = 12, Top = 22, Width = card.Width - 80, Font = new Font("Segoe UI", 8f), ForeColor = Color.FromArgb(90, 90, 90), AutoSize = false, Height = 22, AutoEllipsis = true };

            card.Controls.AddRange(new Control[] { bar, lblT, lblB });

            if (ev != null)
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
                    if (MessageBox.Show($"Remove '{capturedEv.Title}'?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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

            return card;
        }

        private Panel MakeUpcomingStrip(CalendarEvent ev, DateTime date, string when)
        {
            var strip = new Panel { Width = (flpUpcoming.Width > 10 ? flpUpcoming.Width : 380) - 8, Height = 36, BackColor = Color.White, Margin = new Padding(4, 2, 4, 0) };
            var dot = new Panel { Width = 8, Height = 8, Top = 14, Left = 4, BackColor = ev.GetColor() };
            var lblT = new Label { Text = ev.Title, Left = 18, Top = 2, Width = strip.Width - 80, Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = Color.FromArgb(40, 40, 40), AutoSize = false, Height = 18, AutoEllipsis = true };
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
                if (c is Panel rp && rp.Left > 530)
                    rp.Width = pnlBottom.Width - rp.Left - 8;
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

        private void StudentPortal_Closing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Are you sure you want to exit?", "Confirm Exit",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    e.Cancel = true;
                else
                    System.Windows.Forms.Application.Exit();
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e) { changeButtonColor(sender as Button); showContent(clickedButton); }
        private void btnGrades_Click(object sender, EventArgs e) { changeButtonColor(sender as Button); showContent(clickedButton); }
        private void btnCourses_Click(object sender, EventArgs e) { changeButtonColor(sender as Button); showContent(clickedButton); }

        private void btnLMS_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button); showContent(clickedButton);
            pnllmsSubmenu.Visible = !pnllmsSubmenu.Visible;
            btnLMS.Text = pnllmsSubmenu.Visible
                ? " LMS                                       ⌄"
                : " LMS                                        ›";
        }

        private void btnLogout_Click(object sender, EventArgs e) => this.Close();
        private void pnlCoursesContent_Paint(object sender, PaintEventArgs e) { }
        private void btnAnnounceIns_Click(object sender, EventArgs e) { pnlAnnounce.BringToFront(); pnlAnnounce.Visible = true; }
        private void btnCalendarIns_Click(object sender, EventArgs e) { pnlCalendar.BringToFront(); pnlCalendar.Visible = true; }
        private void btnSubjectIns_Click(object sender, EventArgs e) { pnlSubject.BringToFront(); pnlSubject.Visible = true; }
        private void btnActivitiesIns_Click(object sender, EventArgs e) { pnlLMSAct.BringToFront(); pnlLMSAct.Visible = true; }
        private void btnAttendanceIns_Click(object sender, EventArgs e) { pnlAttendance.BringToFront(); pnlAttendance.Visible = true; }
        private void btnGradeIns_Click(object sender, EventArgs e) { pnlGrades.BringToFront(); pnlGrades.Visible = true; }

        private void label24_Click(object sender, EventArgs e) { }
        bool expand = false;
        private void timer1_Tick(object sender, EventArgs e) { }
        private void StatusBtn_Click(object sender, EventArgs e) { timer1.Start(); }
        private void CreateAnnounce_Click(object sender, EventArgs e)
        {
            pnlCreateAnnounce.Visible = !pnlCreateAnnounce.Visible;
            if (pnlCreateAnnounce.Visible)
            {
                pnlCreateAnnounce.BringToFront();
                pnlCreateAnnounce.Location = new Point((this.Width - pnlCreateAnnounce.Width) / 4, (this.Height - pnlCreateAnnounce.Height) / 4);
            }
        }
        private void StatusBtn2_Click(object sender, EventArgs e) { timer1.Start(); }
        private void Sub1_Paint(object sender, PaintEventArgs e) { }
        bool sidebarExpand;
        private void MenuButton_Click(object sender, EventArgs e) { sideBarTimer.Start(); }
        private void btnAssign_Click(object sender, EventArgs e) { pnlLMSAct.Visible = true; pnlLMSAct.BringToFront(); }
        private void btnClassFiles_Click(object sender, EventArgs e) { pnlClassFiles.Visible = true; pnlClassFiles.BringToFront(); }
        private void pnlAttendance_Paint(object sender, PaintEventArgs e) { }
        private void CreateAnnounce_Click_1(object sender, EventArgs e) { pnlCreateAnnounce.Visible = true; pnlCreateAnnounce.BringToFront(); }
        private void btnCancelPost_Click(object sender, EventArgs e) { pnlCreateAnnounce.Visible = false; pnlAnnounce.BringToFront(); }

        private void btnGo1_Click(object sender, EventArgs e)
        {
            pnlSubMenu.Visible = true; pnlSubMenu.BringToFront();
            pnlLMSActivities.Visible = true; pnlLMSActivities.BringToFront();
        }
        private void btnBack_Click(object sender, EventArgs e) { pnlSubject.Visible = true; pnlSubject.BringToFront(); }
        private void btnGeneralAnnounce_Click(object sender, EventArgs e) { pnlGenChats.Visible = true; pnlGenChats.BringToFront(); }
        private void btnLMSActSub_Click(object sender, EventArgs e) { pnlLMSFiles.Visible = false; pnlLMSActivities.Visible = true; pnlLMSActivities.BringToFront(); }
        private void btnLMSFiles_Click(object sender, EventArgs e) { pnlLMSFiles.Visible = true; pnlLMSFiles.BringToFront(); }
        private void btnCreateAct_Click(object sender, EventArgs e) { pnlCreateAct.Visible = true; pnlCreateAct.BringToFront(); }

        private void cmbBXActType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sel = cmbBXActType.SelectedItem?.ToString();
            pnlQuiz1.Visible = false; pnlAssign.Visible = false;
            if (sel == "Quiz") { pnlQuiz1.Visible = true; pnlQuiz1.BringToFront(); }
            else if (sel == "Assignment") { pnlAssign.Visible = true; pnlAssign.BringToFront(); }
        }
        private void btnAssignAttach_Click(object sender, EventArgs e) { pnlAttachAss.Visible = true; pnlAttachAss.BringToFront(); }
        private void btnAttachCancel_Click(object sender, EventArgs e) { pnlAttachAss.Visible = false; }
        private void btnAttachDone_Click(object sender, EventArgs e) { pnlAttachAss.Visible = false; }
        private void btnAttachCancel_Click_1(object sender, EventArgs e) { pnlAttachAss.Visible = false; }
        private void btnDoneAttach_Click(object sender, EventArgs e) { pnlAttachAss.Visible = false; }
        private void btnAddPanel_Click(object sender, EventArgs e)
        {
            ucQuestionCard newCard = new ucQuestionCard();
            newCard.Width = 1250; newCard.Height = 423;
            flowLayoutPanel3.Controls.Add(newCard);
            int centeredMargin = (flowLayoutPanel3.Width - 1250 - 25) / 2;
            if (centeredMargin < 0) centeredMargin = 57;
            foreach (Control ctrl in flowLayoutPanel3.Controls)
            {
                ctrl.Width = 1250;
                ctrl.Margin = new Padding(centeredMargin, 10, 10, 10);
                ctrl.Left = 0;
            }
            RenumberQuestions();
            if (flowLayoutPanel3.Controls.Contains(pnlControlBar))
                flowLayoutPanel3.Controls.SetChildIndex(pnlControlBar, -1);
            flowLayoutPanel3.ScrollControlIntoView(pnlControlBar);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var last = flowLayoutPanel3.Controls.OfType<ucQuestionCard>().LastOrDefault();
            if (last != null) { flowLayoutPanel3.Controls.Remove(last); last.Dispose(); RenumberQuestions(); }
        }

        private void btnSaveQuiz_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Do you want to save this quiz before exiting?", "Save Quiz", MessageBoxButtons.YesNoCancel);
            if (r == DialogResult.Yes || r == DialogResult.No) this.Close();
        }

        private void flowLayoutPanel3_Resize(object sender, EventArgs e)
        {
            int newMargin = (flowLayoutPanel3.Width - 1250 - 25) / 2;
            if (newMargin < 0) newMargin = 10;
            foreach (Control ctrl in flowLayoutPanel3.Controls)
                ctrl.Margin = new Padding(newMargin, 10, 10, 10);
        }

        private void RenumberQuestions()
        {
            int count = 1;
            foreach (Control ctrl in flowLayoutPanel3.Controls)
                if (ctrl is ucQuestionCard card) { card.lblQuestionNumber.Text = "Question " + count; count++; }
        }

        private void pnlLMSFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string path in paths)
            {
                if (Directory.Exists(path)) AddFolderToListView(path);
                else AddFileToListView(path);
            }
        }

        private void AddFolderToListView(string folderPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(folderPath);
            if (!imageList1.Images.ContainsKey("FolderIcon"))
                imageList1.Images.Add("FolderIcon", Properties.Resources.folder_icon);
            int iconIndex = imageList1.Images.IndexOfKey("FolderIcon");
            ListViewItem item = new ListViewItem(dirInfo.Name, iconIndex);
            item.Tag = folderPath;
            item.SubItems.Add("File Folder");
            item.SubItems.Add(DateTime.Now.ToString("g"));
            listView_file.Items.Add(item);
        }

        private void AddFileToListView(string filepath)
        {
            FileInfo fileInfo = new FileInfo(filepath);
            string ext = fileInfo.Extension.ToLower();
            long maxFileSize = 20 * 1024 * 1024;

            if (fileInfo.Length <= maxFileSize)
            {
                if (!imageList1.Images.ContainsKey(ext))
                {
                    if (ext == ".pdf") imageList1.Images.Add(ext, Properties.Resources.pdf_icon);
                    else if (ext == ".png" || ext == ".jpg" || ext == ".jpeg") imageList1.Images.Add(ext, Properties.Resources.png_icon);
                    else if (ext == ".ppt" || ext == ".pptx") imageList1.Images.Add(ext, Properties.Resources.ppt_icon);
                    else imageList1.Images.Add(ext, Icon.ExtractAssociatedIcon(filepath));
                }
                int iconIndex = imageList1.Images.IndexOfKey(ext);
                var item = new ListViewItem(fileInfo.Name, iconIndex);
                item.Tag = filepath;
                double sizeKB = fileInfo.Length / 1024.0;
                item.SubItems.Add(sizeKB >= 1024 ? $"{sizeKB / 1024:F2} MB" : $"{sizeKB:F2} KB");
                item.SubItems.Add(DateTime.Now.ToString("g"));
                listView_file.Items.Add(item);
            }
            else
            {
                MessageBox.Show($"{fileInfo.Name} is over the 20MB limit.", "PUP Acad Portal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void pnlLMSFiles_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void listView_file_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var item = listView_file.GetItemAt(e.X, e.Y);
                if (item != null) { item.Selected = true; contextMenuStrip1.Show(listView_file, e.Location); }
            }
        }

        private void listView_file_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView_file.SelectedItems.Count > 0)
            {
                string fullPath = listView_file.SelectedItems[0].Tag.ToString();
                try { Process.Start(new ProcessStartInfo { FileName = fullPath, UseShellExecute = true }); }
                catch (Exception ex) { MessageBox.Show("Could not open file: " + ex.Message); }
            }
        }

        private void removeFromTheListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView_file.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Remove this file from the list?", "Confirm Remove",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    foreach (ListViewItem item in listView_file.SelectedItems)
                        listView_file.Items.Remove(item);
            }
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView_file.SelectedItems.Count > 0)
            {
                string sourcePath = listView_file.SelectedItems[0].Tag.ToString();
                string fileName = listView_file.SelectedItems[0].Text;
                var sfd = new SaveFileDialog { FileName = fileName, Filter = "All files (*.*)|*.*" };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try { File.Copy(sourcePath, sfd.FileName, true); MessageBox.Show("File saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    catch (Exception ex) { MessageBox.Show("Error saving file: " + ex.Message); }
                }
            }
        }
    }
}