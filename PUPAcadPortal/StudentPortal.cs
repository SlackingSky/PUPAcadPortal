using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
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

            int bx = 12;
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
                if (c is Button b)
                {
                    bool active = (_activeFilter == null && b.Tag?.ToString() == "All")
                               || (_activeFilter != null && b.Tag?.ToString() == _activeFilter.ToString());

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

            var events = SharedCalendarData.GetEventsForDate(date)
                .Where(ev => _activeFilter == null || ev.Type == _activeFilter)
                .ToList();

            var noteDict = SharedCalendarData.StudentNotes;
            if (noteDict.ContainsKey(date.Date) && !string.IsNullOrWhiteSpace(noteDict[date.Date]))
            {
                var noteCard = MakeEventCard("🗒 Note", noteDict[date.Date], Color.FromArgb(100, 100, 100), date);
                flpDayEvents.Controls.Add(noteCard);
            }

            if (events.Count == 0 && flpDayEvents.Controls.Count == 0)
            {
                flpDayEvents.Controls.Add(lblNoEvents);
                return;
            }

            foreach (var ev in events)
            {
                var card = MakeEventCard(
                    $"[{ev.GetTypeLabel()}]  {ev.Title}",
                    (string.IsNullOrEmpty(ev.StartTime) ? "" : ev.StartTime + (string.IsNullOrEmpty(ev.EndTime) ? "" : " – " + ev.EndTime) + "\n")
                    + (string.IsNullOrEmpty(ev.Room) ? "" : "Room: " + ev.Room + "\n")
                    + ev.Description,
                    ev.GetColor(), date, ev);
                flpDayEvents.Controls.Add(card);
            }
        }

        private void RefreshUpcoming()
        {
            flpUpcoming.Controls.Clear();
            var upcoming = SharedCalendarData.GetUpcoming(6);

            if (upcoming.Count == 0)
            {
                flpUpcoming.Controls.Add(lblNoUpcoming);
                return;
            }

            foreach (var (d, ev) in upcoming)
            {
                int daysLeft = (d.Date - DateTime.Now.Date).Days;
                string when = daysLeft == 0 ? "Today" : daysLeft == 1 ? "Tomorrow" : $"In {daysLeft} days";
                flpUpcoming.Controls.Add(MakeUpcomingStrip(ev, d, when));
            }
        }

        private Panel MakeEventCard(string title, string body, Color accent, DateTime date, CalendarEvent ev = null)
        {
            var card = new Panel
            {
                Width = flpDayEvents.Width - 16,
                Height = 48,
                BackColor = Color.FromArgb(245, 248, 255),
                Margin = new Padding(4, 3, 4, 0),
                Cursor = Cursors.Default,
            };

            var bar = new Panel { Width = 5, Height = card.Height, Left = 0, Top = 0, BackColor = accent };
            var lblTitle = new Label
            {
                Text = title,
                Left = 12,
                Top = 3,
                Width = card.Width - 20,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 40, 40),
                AutoSize = false,
                Height = 18,
            };
            var lblBody = new Label
            {
                Text = body.Trim(),
                Left = 12,
                Top = 22,
                Width = card.Width - 20,
                Font = new Font("Segoe UI", 8f),
                ForeColor = Color.FromArgb(90, 90, 90),
                AutoSize = false,
                Height = 22,
                AutoEllipsis = true,
            };

            card.Controls.AddRange(new Control[] { bar, lblTitle, lblBody });

            if (ev == null)
            {
                card.Cursor = Cursors.Hand;
                card.Click += (s, e) => OnDaySelected(date);
            }

            return card;
        }

        private Panel MakeUpcomingStrip(CalendarEvent ev, DateTime date, string when)
        {
            var strip = new Panel
            {
                Width = flpUpcoming.Width - 8,
                Height = 36,
                BackColor = Color.White,
                Margin = new Padding(4, 2, 4, 0),
            };
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
                if (c is Panel rp && rp.Left > 490)
                    rp.Width = pnlBottom.Width - rp.Left - 8;
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
                { btnDashboard,  pnlDashboardContent },
                { btnEnrollment, pnlEnrollContent    },
                { btnCourses,    pnlCoursesContent   },
                { btnAccounts,   pnlAccountsContent  },
            };
            foreach (var kv in contents)
            {
                bool show = kv.Key == clickedButton;
                kv.Value.Visible = show;
                if (show) kv.Value.Location = new System.Drawing.Point(pnlSidebar.Width, pnlHeader.Height);
            }
        }

        private void StudentPortal_Closing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Are you sure you want to exit?", "Confirm Exit",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    e.Cancel = true;
                else
                    Application.Exit();
            }
        }

        private void StudentPortal_Load(object sender, EventArgs e) { }
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

        private void btnDashboard_Click(object sender, EventArgs e) { changeButtonColor(sender as Button); showContent(clickedButton); }
        private void btnEnrollment_Click(object sender, EventArgs e) { changeButtonColor(sender as Button); showContent(clickedButton); }
        private void btnCourses_Click(object sender, EventArgs e) { changeButtonColor(sender as Button); showContent(clickedButton); }
        private void btnAccounts_Click(object sender, EventArgs e) { changeButtonColor(sender as Button); showContent(clickedButton); }

        private void btnLMS_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
            pnllmsSubmenu.Visible = !pnllmsSubmenu.Visible;
            btnLMS.Text = pnllmsSubmenu.Visible
                ? " LMS                                       ⌄"
                : " LMS                                        ›";
        }

        private void btnLogout_Click(object sender, EventArgs e) => this.Close();

        private void btnAnnounce_Click(object sender, EventArgs e) { pnlAnnounce.BringToFront(); pnlAnnounce.Visible = true; }
        private void btnCalendar_Click(object sender, EventArgs e) { pnlSubject.Visible = false; pnlCalendar.BringToFront(); pnlCalendar.Visible = true; }
        private void btnSubject_Click(object sender, EventArgs e) { pnlSubject.BringToFront(); pnlSubject.Visible = true; }
        private void btnActivities_Click(object sender, EventArgs e) { pnlActivities.BringToFront(); pnlActivities.Visible = true; }
        private void btnAttendance_Click(object sender, EventArgs e) { pnlAttendance.BringToFront(); pnlAttendance.Visible = true; }
        private void btnGrade_Click(object sender, EventArgs e) { pnlGrades.BringToFront(); pnlGrades.Visible = true; }

        private void btnGo1_Click(object sender, EventArgs e)
        {
            pnlLMSFiles.Visible = false;
            pnlSubMenu.BringToFront(); pnlSubMenu.Visible = true;
            pnlLMSActivities.BringToFront(); pnlLMSActivities.Visible = true;
        }

        private void btnBack_Click(object sender, EventArgs e) { pnlSubject.BringToFront(); pnlSubject.Visible = true; }
        private void btnStudFiles_Click(object sender, EventArgs e) { pnlLMSFiles.BringToFront(); pnlLMSFiles.Visible = true; }
        private void btnStudAct_Click(object sender, EventArgs e)
        {
            pnlLMSFiles.Visible = false;
            pnlLMSActivities.BringToFront(); pnlLMSActivities.Visible = true;
        }

        private void pnlAct1_MouseEnter(object sender, EventArgs e) { pnlAct1.BackColor = Color.FromArgb(128, 0, 0); pnlAct1.Cursor = Cursors.Hand; }
        private void pnlAct1_MouseLeave(object sender, EventArgs e) { pnlAct1.BackColor = Color.White; }
        private void pnlAct1_Click(object sender, EventArgs e) { pnlAnsAct1.Visible = true; pnlAnsAct1.BringToFront(); pnlAnsAct1.Dock = DockStyle.Fill; }

        private void btnCancelAct_Click(object sender, EventArgs e) { pnlLMSActivities.BringToFront(); pnlLMSActivities.Visible = true; }
        private void btnCancelAct_Click_1(object sender, EventArgs e) { pnlLMSActivities.BringToFront(); pnlLMSActivities.Visible = true; }
        private void btnSubmit_Click(object sender, EventArgs e) { pnlLMSActivities.BringToFront(); pnlLMSActivities.Visible = true; }

        private void btnAssignAttach_Click(object sender, EventArgs e) { pnlAttachAss.BringToFront(); pnlAttachAss.Visible = true; }
        private void btnCancelAssign_Click(object sender, EventArgs e) { pnlLMSActivities.BringToFront(); pnlLMSActivities.Visible = true; }
        private void btnSaveAss_Click(object sender, EventArgs e) { pnlLMSActivities.BringToFront(); pnlLMSActivities.Visible = true; }

        private void btnAttachCancel_Click(object sender, EventArgs e) => pnlAttachAss.Hide();
        private void btnDoneAttach_Click(object sender, EventArgs e) => pnlAttachAss.Hide();

        private void pnlAss1_MouseEnter(object sender, EventArgs e) { pnlAss1.BackColor = Color.Maroon; pnlAss1.Cursor = Cursors.Hand; }
        private void pnlAss1_MouseLeave(object sender, EventArgs e) => pnlAss1.BackColor = Color.White;
        private void pnlAss1_Click(object sender, EventArgs e) { pnlAss1.BackColor = Color.White; pnlAnsAss.Visible = true; pnlAnsAss.BringToFront(); }

        private void roundedPanel14_MouseLeave(object sender, EventArgs e) => roundedPanel14.BackColor = Color.White;
        private void roundedPanel14_MouseEnter(object sender, EventArgs e) { roundedPanel14.BackColor = Color.Maroon; roundedPanel14.Cursor = Cursors.Hand; }
        private void roundedPanel16_MouseEnter(object sender, EventArgs e) { roundedPanel16.BackColor = Color.Maroon; roundedPanel16.Cursor = Cursors.Hand; }
        private void roundedPanel16_MouseLeave(object sender, EventArgs e) => roundedPanel16.BackColor = Color.White;
    }
}