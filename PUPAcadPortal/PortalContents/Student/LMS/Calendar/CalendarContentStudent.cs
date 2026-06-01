using PUPAcadPortal.Data;
using PUPAcadPortal.PortalForms;
using PUPAcadPortal.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Student.LMS
{
    public enum CalendarView { Monthly, Weekly, Daily }

    public enum StudentEventCategory { Class, ActivityDeadline, Quiz, Exam, SchoolEvent, Personal, Consultation }

    public partial class CalendarContentStudent : UserControl
    {
        public static int _year = DateTime.Now.Year;
        public static int _month = DateTime.Now.Month;
        private DateTime _currentWeekStart;
        private DateTime _currentDayDate;
        private CalendarView _view = CalendarView.Monthly;
        private EventType? _activeFilter = null;
        private UrDay _selectedCell;
        private DateTime _lastSelectedDate = DateTime.Now.Date;

        private FlowLayoutPanel pnlDayHeaders;

        private Panel pnlBottom;
        private Label lblSelectedDate;
        private FlowLayoutPanel flpDayEvents;
        private FlowLayoutPanel flpUpcoming;
        private Label lblNoEvents;
        private Label lblNoUpcoming;

        private Panel pnlToolbar;
        private Button btnMonthly, btnWeekly, btnDaily;
        private Label lblViewTitle;
        private Panel pnlViewBody;     
        private Panel pnlReminderBanner;
        private Label lblReminderText;
        private System.Windows.Forms.Timer _reminderTimer;

        private static readonly Color C_Primary = Color.FromArgb(128, 0, 0);   // Maroon
        private static readonly Color C_PrimaryLt = Color.FromArgb(160, 32, 32);
        private static readonly Color C_Accent = Color.FromArgb(66, 133, 244);   // Blue
        private static readonly Color C_Surface = Color.FromArgb(250, 250, 252);
        private static readonly Color C_Border = Color.FromArgb(220, 220, 225);
        private static readonly Color C_TextDark = Color.FromArgb(28, 28, 36);
        private static readonly Color C_TextMid = Color.FromArgb(80, 80, 92);
        private static readonly Color C_TextLight = Color.FromArgb(150, 150, 160);

        private static Color GetCategoryColor(StudentEventCategory cat)
        {
            return cat switch
            {
                StudentEventCategory.Class => Color.FromArgb(66, 133, 244),
                StudentEventCategory.Exam => Color.FromArgb(220, 53, 69),
                StudentEventCategory.ActivityDeadline => Color.FromArgb(255, 140, 0),
                StudentEventCategory.Quiz => Color.FromArgb(138, 43, 226),
                StudentEventCategory.SchoolEvent => Color.FromArgb(32, 178, 170),
                StudentEventCategory.Personal => Color.FromArgb(0, 153, 102),
                StudentEventCategory.Consultation => Color.FromArgb(255, 140, 0),
                _ => Color.DimGray
            };
        }

        private static StudentEventCategory MapEventTypeToCategory(EventType t)
        {
            return t switch
            {
                EventType.Class => StudentEventCategory.Class,
                EventType.Exam => StudentEventCategory.Exam,
                EventType.Deadline => StudentEventCategory.ActivityDeadline,
                EventType.Consultation => StudentEventCategory.Consultation,
                _ => StudentEventCategory.SchoolEvent
            };
        }

        public CalendarContentStudent()
        {
            InitializeComponent();

            SharedCalendarData.LoadData();
            _year = DateTime.Now.Year;
            _month = DateTime.Now.Month;
            _currentWeekStart = StartOfWeek(DateTime.Now.Date);
            _currentDayDate = DateTime.Now.Date;

            BuildToolbar();
            BuildViewBody();
            BuildBottomPanel();
            BuildReminderBanner();

            this.Resize += OnFormResized;

            SwitchView(CalendarView.Monthly);

            UrDay.DaySelected += OnDaySelected;
            OnDaySelected(DateTime.Now.Date);

            var wheelFilter = new CalendarWheelFilter(pnlViewBody, delta =>
            {
                if (_view == CalendarView.Monthly)
                {
                    if (delta > 0) NavigatePrev(); else NavigateNext();
                }
            });
            Application.AddMessageFilter(wheelFilter);

            _reminderTimer = new System.Windows.Forms.Timer { Interval = 60_000 };
            _reminderTimer.Tick += (s, e) => ShowReminders();
            _reminderTimer.Start();
            ShowReminders();   // show on load

            this.Disposed += (s, ev) =>
            {
                UrDay.DaySelected -= OnDaySelected;
                Application.RemoveMessageFilter(wheelFilter);
                this.Resize -= OnFormResized;
                _reminderTimer.Stop();
                _reminderTimer.Dispose();
            };
        }
        private void BuildToolbar()
        {
            const int H = 52;

            pnlToolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = H,
                BackColor = C_Primary,
                Padding = new Padding(0),
            };
            pnlCalendar.Controls.Add(pnlToolbar);
            pnlToolbar.BringToFront();

            var btnPrev = MakeNavButton("◀");
            btnPrev.Left = 12; btnPrev.Top = (H - 30) / 2;
            btnPrev.Click += (s, e) => NavigatePrev();
            pnlToolbar.Controls.Add(btnPrev);

            var btnToday = MakeNavButton("Today");
            btnToday.Width = 64;
            btnToday.Left = btnPrev.Right + 4; btnToday.Top = (H - 30) / 2;
            btnToday.Click += (s, e) => NavigateToday();
            pnlToolbar.Controls.Add(btnToday);

            var btnNext = MakeNavButton("▶");
            btnNext.Left = btnToday.Right + 4; btnNext.Top = (H - 30) / 2;
            btnNext.Click += (s, e) => NavigateNext();
            pnlToolbar.Controls.Add(btnNext);

            lblViewTitle = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                Top = 0,
                Height = H,
            };
            pnlToolbar.Controls.Add(lblViewTitle);

            btnMonthly = MakeViewToggle("Monthly");
            btnWeekly = MakeViewToggle("Weekly");
            btnDaily = MakeViewToggle("Daily");

            btnMonthly.Click += (s, e) => SwitchView(CalendarView.Monthly);
            btnWeekly.Click += (s, e) => SwitchView(CalendarView.Weekly);
            btnDaily.Click += (s, e) => SwitchView(CalendarView.Daily);

            pnlToolbar.Controls.Add(btnMonthly);
            pnlToolbar.Controls.Add(btnWeekly);
            pnlToolbar.Controls.Add(btnDaily);

            pnlToolbar.Resize += (s, e) => RepositionToolbarButtons();
            RepositionToolbarButtons();
        }

        private void RepositionToolbarButtons()
        {
            if (btnMonthly == null) return;
            int right = pnlToolbar.Width - 12;
            btnDaily.Left = right - btnDaily.Width;
            btnWeekly.Left = btnDaily.Left - btnWeekly.Width - 6;
            btnMonthly.Left = btnWeekly.Left - btnMonthly.Width - 6;

            int cx = 12 + 32 + 4 + 64 + 4 + 32 + 12; 
            lblViewTitle.Left = cx;
            lblViewTitle.Width = btnMonthly.Left - cx - 8;
        }

        private Button MakeNavButton(string text)
        {
            var b = new Button
            {
                Text = text,
                Width = 32,
                Height = 30,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(160, 255, 255, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand,
            };
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(210, 255, 255, 255);
            return b;
        }

        private Button MakeViewToggle(string text)
        {
            var b = new Button
            {
                Text = text,
                Width = 72,
                Height = 30,
                Top = 11,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8.5f),
                Cursor = Cursors.Hand,
            };
            b.FlatAppearance.BorderColor = Color.FromArgb(180, 255, 255, 255);
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 255, 255, 255);
            return b;
        }

        private void SetActiveViewButton(CalendarView v)
        {
            foreach (var (btn, cv) in new[] {
                (btnMonthly, CalendarView.Monthly),
                (btnWeekly,  CalendarView.Weekly),
                (btnDaily,   CalendarView.Daily) })
            {
                bool active = cv == v;
                btn.BackColor = active
                    ? Color.FromArgb(255, 255, 255)
                    : Color.Transparent;
                btn.ForeColor = active ? C_Primary : Color.White;
                btn.Font = new Font("Segoe UI", 8.5f, active ? FontStyle.Bold : FontStyle.Regular);
            }
        }

        private void BuildViewBody()
        {
            pnlViewBody = new Panel
            {
                BackColor = C_Surface,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
            };
            pnlCalendar.Controls.Add(pnlViewBody);
        }

        private void PositionViewBody()
        {
            const int BOTTOM_H = 240;
            const int TOOLBAR_H = 52;
            const int BANNER_H = 34;
            int bannerOffset = (pnlReminderBanner != null && pnlReminderBanner.Visible) ? BANNER_H : 0;

            pnlViewBody.Left = 0;
            pnlViewBody.Top = TOOLBAR_H + bannerOffset;
            pnlViewBody.Width = pnlCalendar.ClientSize.Width;
            pnlViewBody.Height = pnlCalendar.ClientSize.Height - TOOLBAR_H - BOTTOM_H - bannerOffset;
        }

        private void BuildReminderBanner()
        {
            pnlReminderBanner = new Panel
            {
                Dock = DockStyle.None,
                Height = 34,
                BackColor = Color.FromArgb(255, 243, 205),
                Visible = false,
            };

            var iconLbl = new Label
            {
                Text = "🔔",
                Left = 8,
                Top = 7,
                AutoSize = true,
                Font = new Font("Segoe UI", 10f),
            };

            lblReminderText = new Label
            {
                Left = 30,
                Top = 9,
                AutoSize = false,
                Height = 18,
                ForeColor = Color.FromArgb(90, 60, 0),
                Font = new Font("Segoe UI", 8.5f),
                AutoEllipsis = true,
            };

            var btnDismiss = new Button
            {
                Text = "✕",
                Width = 22,
                Height = 22,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(120, 80, 0),
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 8f),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
            };
            btnDismiss.FlatAppearance.BorderSize = 0;
            btnDismiss.Click += (s, e) => { pnlReminderBanner.Visible = false; PositionViewBody(); PositionBottomPanel(); };

            pnlReminderBanner.Controls.AddRange(new Control[] { iconLbl, lblReminderText, btnDismiss });

            pnlReminderBanner.Resize += (s, e) =>
            {
                lblReminderText.Width = pnlReminderBanner.Width - 80;
                btnDismiss.Left = pnlReminderBanner.Width - 28;
                btnDismiss.Top = 6;
            };

            pnlCalendar.Controls.Add(pnlReminderBanner);
            pnlReminderBanner.BringToFront();
        }

        private void PositionReminderBanner()
        {
            const int TOOLBAR_H = 52;
            pnlReminderBanner.Left = 0;
            pnlReminderBanner.Top = TOOLBAR_H;
            pnlReminderBanner.Width = pnlCalendar.ClientSize.Width;
            lblReminderText.Width = pnlReminderBanner.Width - 80;

            foreach (Control c in pnlReminderBanner.Controls)
                if (c is Button b && b.Text == "✕")
                    b.Left = pnlReminderBanner.Width - 28;
        }

        private void ShowReminders()
        {
            var upcoming = SharedCalendarData.GetUpcoming(10, includeStudentEvents: true);
            var reminders = new List<string>();

            foreach (var (d, ev) in upcoming)
            {
                int days = (d.Date - DateTime.Now.Date).Days;
                if (days < 0) continue;

                bool highlight = false;
                if (ev.Type == EventType.Exam && days <= 3) highlight = true;
                if (ev.Type == EventType.Deadline && days <= 3) highlight = true;
                if (ev.Type == EventType.Class && days == 0) highlight = true;

                if (highlight)
                {
                    string when = days == 0 ? "TODAY" : days == 1 ? "tomorrow" : $"in {days} days";
                    reminders.Add($"[{ev.GetTypeLabel()}] {ev.Title} — {when}");
                }
            }

            if (reminders.Count == 0)
            {
                pnlReminderBanner.Visible = false;
            }
            else
            {
                lblReminderText.Text = string.Join("   •   ", reminders);
                pnlReminderBanner.Visible = true;
                PositionReminderBanner();
            }

            PositionViewBody();
            PositionBottomPanel();
        }

        private void SwitchView(CalendarView v)
        {
            _view = v;
            SetActiveViewButton(v);
            pnlViewBody.Controls.Clear();

            switch (v)
            {
                case CalendarView.Monthly: BuildMonthlyView(); break;
                case CalendarView.Weekly: BuildWeeklyView(); break;
                case CalendarView.Daily: BuildDailyView(); break;
            }

            UpdateViewTitle();
            PositionViewBody();
            PositionBottomPanel();
        }

        private void UpdateViewTitle()
        {
            switch (_view)
            {
                case CalendarView.Monthly:
                    lblViewTitle.Text = new DateTimeFormatInfo().GetMonthName(_month).ToUpper() + "  " + _year;
                    break;
                case CalendarView.Weekly:
                    var we = _currentWeekStart.AddDays(6);
                    lblViewTitle.Text = _currentWeekStart.ToString("MMM d") + " – " + we.ToString("MMM d, yyyy");
                    break;
                case CalendarView.Daily:
                    lblViewTitle.Text = _currentDayDate.ToString("dddd, MMMM dd, yyyy").ToUpper();
                    break;
            }
        }

        private void NavigatePrev()
        {
            switch (_view)
            {
                case CalendarView.Monthly:
                    _month--; if (_month < 1) { _month = 12; _year--; }
                    BuildMonthlyView(); break;
                case CalendarView.Weekly:
                    _currentWeekStart = _currentWeekStart.AddDays(-7);
                    BuildWeeklyView(); break;
                case CalendarView.Daily:
                    _currentDayDate = _currentDayDate.AddDays(-1);
                    BuildDailyView(); break;
            }
            UpdateViewTitle();
        }

        private void NavigateNext()
        {
            switch (_view)
            {
                case CalendarView.Monthly:
                    _month++; if (_month > 12) { _month = 1; _year++; }
                    BuildMonthlyView(); break;
                case CalendarView.Weekly:
                    _currentWeekStart = _currentWeekStart.AddDays(7);
                    BuildWeeklyView(); break;
                case CalendarView.Daily:
                    _currentDayDate = _currentDayDate.AddDays(1);
                    BuildDailyView(); break;
            }
            UpdateViewTitle();
        }

        private void NavigateToday()
        {
            _month = DateTime.Now.Month;
            _year = DateTime.Now.Year;
            _currentWeekStart = StartOfWeek(DateTime.Now.Date);
            _currentDayDate = DateTime.Now.Date;

            SwitchView(_view);
            OnDaySelected(DateTime.Now.Date);
        }

        private FlowLayoutPanel _fpMonth;
        private FlowLayoutPanel _fpDayHeaders;

        private void BuildMonthlyView()
        {
            pnlViewBody.Controls.Clear();

            _fpDayHeaders = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Height = 30,
                Left = 0,
                Top = 0,
                BackColor = Color.FromArgb(245, 245, 248),
                Padding = new Padding(0),
                Margin = new Padding(0),
            };
            string[] dow = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            foreach (string d in dow)
                _fpDayHeaders.Controls.Add(new Label
                {
                    Text = d,
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = false,
                    Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                    ForeColor = d == "Sunday" ? Color.Crimson : C_TextMid,
                    BackColor = Color.Transparent,
                    Margin = new Padding(0),
                    Height = 30,
                });

            _fpMonth = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Left = 0,
                Top = 30,
                AutoScroll = true,
                BackColor = C_Surface,
                Padding = new Padding(0),
                Margin = new Padding(0),
            };

            pnlViewBody.Controls.Add(_fpDayHeaders);
            pnlViewBody.Controls.Add(_fpMonth);

            pnlViewBody.Resize += MonthlyView_Resize;
            _fpMonth.Resize += (s, e) => ResizeMonthCells();

            PopulateMonthGrid(_month, _year);
            ResizeMonthlyBody();
        }

        private void MonthlyView_Resize(object s, EventArgs e) => ResizeMonthlyBody();

        private void ResizeMonthlyBody()
        {
            if (_fpDayHeaders == null || _fpMonth == null) return;
            _fpDayHeaders.Width = pnlViewBody.ClientSize.Width;
            _fpDayHeaders.Left = 0;

            _fpMonth.Left = 0;
            _fpMonth.Top = 30;
            _fpMonth.Width = pnlViewBody.ClientSize.Width;
            _fpMonth.Height = pnlViewBody.ClientSize.Height - 30;

            ResizeMonthCells();
            AlignMonthHeaders();
        }

        private void ResizeMonthCells()
        {
            if (_fpMonth == null || _fpMonth.Controls.Count == 0) return;
            int available = _fpMonth.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 2;
            int cellW = available / 7;
            _fpMonth.SuspendLayout();
            foreach (Control c in _fpMonth.Controls)
            {
                c.Width = cellW;
                c.Height = 110;
                c.Margin = new Padding(1);
            }
            _fpMonth.ResumeLayout();
            AlignMonthHeaders();
        }

        private void AlignMonthHeaders()
        {
            if (_fpDayHeaders == null || _fpMonth == null) return;
            int available = _fpMonth.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 2;
            int cellW = available / 7;
            foreach (Control c in _fpDayHeaders.Controls) c.Width = cellW;
        }

        private void PopulateMonthGrid(int month, int year)
        {
            if (_fpMonth == null) return;

            if (year < 1) year = DateTime.Now.Year;
            if (year > 9999) year = DateTime.Now.Year;
            if (month < 1) month = 1;
            if (month > 12) month = 12;

            _fpMonth.Controls.Clear();
            _year = year;
            _month = month;

            SharedCalendarData.CurrentYear = year;
            SharedCalendarData.CurrentMonth = month;

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
                    _fpMonth.Controls.Add(new UrDay(d.ToString(), prev.Year, prev.Month, false,
                        GetHoliday(prev.Year, prev.Month, d), isStudent: true));
                }
            }

            for (int i = 1; i <= daysInMonth; i++)
                _fpMonth.Controls.Add(new UrDay(i.ToString(), year, month, true,
                    GetHoliday(year, month, i), isStudent: true));
            int total = _fpMonth.Controls.Count;
            int remainder = total % 7;
            if (remainder > 0)
            {
                DateTime next = firstDay.AddMonths(1);
                for (int i = 1; i <= 7 - remainder; i++)
                    _fpMonth.Controls.Add(new UrDay(i.ToString(), next.Year, next.Month, false,
                        GetHoliday(next.Year, next.Month, i), isStudent: true));
            }

            ResizeMonthCells();
        }

        private void BuildWeeklyView()
        {
            pnlViewBody.Controls.Clear();
            pnlViewBody.Resize -= WeeklyView_Resize;

            var outer = new Panel { Dock = DockStyle.Fill, BackColor = C_Surface };
            pnlViewBody.Controls.Add(outer);
            pnlViewBody.Resize += WeeklyView_Resize;

            RenderWeekGrid(outer);
        }

        private Panel _weekOuter;
        private void WeeklyView_Resize(object s, EventArgs e)
        {
            if (pnlViewBody.Controls.Count > 0 && pnlViewBody.Controls[0] is Panel p)
                p.Size = pnlViewBody.ClientSize;
        }

        private void RenderWeekGrid(Panel outer)
        {
            outer.Controls.Clear();

            int colCount = 7;
            int headerH = 44;
            int timeColW = 54;
            int scrollPad = SystemInformation.VerticalScrollBarWidth + 2;

            var header = new Panel { BackColor = Color.FromArgb(240, 240, 245), Height = headerH, Left = 0, Top = 0, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            outer.Controls.Add(header);

            var scroll = new Panel { Left = 0, Top = headerH, AutoScroll = true, BackColor = Color.White, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom };
            outer.Controls.Add(scroll);

            outer.Resize += (s, e) =>
            {
                header.Width = outer.ClientSize.Width;
                scroll.Width = outer.ClientSize.Width;
                scroll.Height = outer.ClientSize.Height - headerH;
                int colW = (header.Width - timeColW - scrollPad) / 7;
                RenderWeekColumns(header, scroll, colW, timeColW, scrollPad);
            };

            int initColW = (outer.Width - timeColW - scrollPad) / 7;
            scroll.Height = outer.ClientSize.Height - headerH;
            RenderWeekColumns(header, scroll, initColW, timeColW, scrollPad);
        }

        private void RenderWeekColumns(Panel header, Panel scroll, int colW, int timeColW, int scrollPad)
        {
            header.Controls.Clear();
            scroll.Controls.Clear();

            var timeGutter = new Label
            {
                Left = 0,
                Top = 0,
                Width = timeColW,
                Height = header.Height,
                Text = "TIME",
                Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                ForeColor = C_TextLight,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent,
            };
            header.Controls.Add(timeGutter);

            int hourH = 48;   
            int totalH = hourH * 24;

            var timeCol = new Panel { Left = 0, Top = 0, Width = timeColW, Height = totalH, BackColor = Color.FromArgb(248, 248, 252) };
            for (int h = 0; h < 24; h++)
            {
                var lbl = new Label
                {
                    Text = h == 0 ? "" : $"{h:00}:00",
                    Left = 0,
                    Top = h * hourH - 8,
                    Width = timeColW,
                    Height = 16,
                    TextAlign = ContentAlignment.MiddleRight,
                    Font = new Font("Segoe UI", 7f),
                    ForeColor = C_TextLight,
                    Padding = new Padding(0, 0, 4, 0),
                };
                timeCol.Controls.Add(lbl);

                var sep = new Panel { Left = 0, Top = h * hourH, Width = timeColW, Height = 1, BackColor = C_Border };
                timeCol.Controls.Add(sep);
            }
            scroll.Controls.Add(timeCol);

            for (int d = 0; d < 7; d++)
            {
                DateTime day = _currentWeekStart.AddDays(d);
                bool isToday = day.Date == DateTime.Now.Date;
                bool isSunday = day.DayOfWeek == DayOfWeek.Sunday;

                var hcell = new Panel
                {
                    Left = timeColW + d * colW,
                    Top = 0,
                    Width = colW,
                    Height = header.Height,
                    BackColor = isToday ? C_Primary : Color.Transparent,
                };
                var hLbl = new Label
                {
                    Text = day.ToString("ddd").ToUpper() + "\n" + day.Day,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 8.5f, isToday ? FontStyle.Bold : FontStyle.Regular),
                    ForeColor = isToday ? Color.White : (isSunday ? Color.Crimson : C_TextDark),
                    BackColor = Color.Transparent,
                };
                hcell.Controls.Add(hLbl);

                DateTime captured = day;
                hcell.Cursor = Cursors.Hand;
                hcell.Click += (s, e) => {
                    _currentDayDate = captured;
                    SwitchView(CalendarView.Daily);
                };
                header.Controls.Add(hcell);

                var dcol = new Panel
                {
                    Left = timeColW + d * colW,
                    Top = 0,
                    Width = colW,
                    Height = totalH,
                    BackColor = isToday ? Color.FromArgb(254, 248, 248) : Color.White,
                };

                for (int h = 0; h < 24; h++)
                {
                    var line = new Panel { Left = 0, Top = h * hourH, Width = colW, Height = 1, BackColor = C_Border };
                    dcol.Controls.Add(line);
                }

                var rborder = new Panel { Left = colW - 1, Top = 0, Width = 1, Height = totalH, BackColor = C_Border };
                dcol.Controls.Add(rborder);

                var allEvs = SharedCalendarData.GetEventsForDate(day)
                    .Concat(SharedCalendarData.GetStudentEventsForDate(day))
                    .ToList();

                foreach (var ev in allEvs)
                    PlaceWeekEventBlock(dcol, ev, hourH, colW);

                scroll.Controls.Add(dcol);
                dcol.Click += (s, e) => { _currentDayDate = captured; SwitchView(CalendarView.Daily); };
            }

            scroll.AutoScrollPosition = new Point(0, 7 * hourH);
        }

        private void PlaceWeekEventBlock(Panel col, CalendarEvent ev, int hourH, int colW)
        {
            int topY = 8 * hourH;  
            int blockH = hourH - 4;

            if (TimeSpan.TryParse(ev.StartTime, out TimeSpan ts))
                topY = (int)(ts.TotalMinutes / 60.0 * hourH);

            if (!string.IsNullOrEmpty(ev.StartTime) && !string.IsNullOrEmpty(ev.EndTime) &&
                TimeSpan.TryParse(ev.StartTime, out TimeSpan t1) &&
                TimeSpan.TryParse(ev.EndTime, out TimeSpan t2) &&
                t2 > t1)
                blockH = (int)((t2 - t1).TotalMinutes / 60.0 * hourH) - 2;

            blockH = Math.Max(blockH, 18);

            Color c = ev.GetColor();
            var block = new Panel
            {
                Left = 2,
                Top = topY + 1,
                Width = colW - 6,
                Height = blockH,
                BackColor = Color.FromArgb(230, c.R, c.G, c.B),
                Cursor = Cursors.Hand,
            };
            var lbl = new Label
            {
                Text = ev.Title,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 7f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Padding = new Padding(3, 2, 2, 0),
                AutoEllipsis = true,
            };
            block.Controls.Add(lbl);

            var bar = new Panel { Left = 0, Top = 0, Width = 3, Height = blockH, BackColor = c };
            block.Controls.Add(bar);
            bar.BringToFront();

            var tip = new ToolTip();
            string tipText = $"[{ev.GetTypeLabel()}] {ev.Title}";
            if (!string.IsNullOrEmpty(ev.StartTime)) tipText += $"\n{ev.StartTime}" + (string.IsNullOrEmpty(ev.EndTime) ? "" : $" – {ev.EndTime}");
            if (!string.IsNullOrEmpty(ev.Room)) tipText += $"\nRoom: {ev.Room}";
            tip.SetToolTip(block, tipText);
            tip.SetToolTip(lbl, tipText);

            block.Click += (s, e) => ShowEventDetailDialog(ev, DateTime.MinValue);
            lbl.Click += (s, e) => ShowEventDetailDialog(ev, DateTime.MinValue);

            col.Controls.Add(block);
            block.BringToFront();
        }

        private void BuildDailyView()
        {
            pnlViewBody.Controls.Clear();
            pnlViewBody.Resize -= DailyView_Resize;

            var outer = new Panel { Dock = DockStyle.Fill, BackColor = C_Surface };
            pnlViewBody.Controls.Add(outer);
            pnlViewBody.Resize += DailyView_Resize;

            RenderDayTimeline(outer);
        }

        private void DailyView_Resize(object s, EventArgs e)
        {
            if (pnlViewBody.Controls.Count > 0 && pnlViewBody.Controls[0] is Panel p)
                p.Size = pnlViewBody.ClientSize;
        }

        private void RenderDayTimeline(Panel outer)
        {
            outer.Controls.Clear();

            DateTime day = _currentDayDate;
            bool isToday = day.Date == DateTime.Now.Date;
            int timeColW = 60;
            int hourH = 56;
            int totalH = hourH * 24;

            var dayStrip = new Panel
            {
                Left = 0,
                Top = 0,
                Width = outer.Width,
                Height = 40,
                BackColor = isToday ? C_Primary : Color.FromArgb(245, 245, 250),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            };

            var holiday = GetHoliday(day.Year, day.Month, day.Day);
            string headerText = day.ToString("dddd, MMMM dd, yyyy");
            if (!string.IsNullOrEmpty(holiday)) headerText += $"  🌿 {holiday}";

            dayStrip.Controls.Add(new Label
            {
                Text = headerText,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = isToday ? Color.White : C_TextDark,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0),
                BackColor = Color.Transparent,
            });

            var navStrip = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Dock = DockStyle.None,
                AutoSize = true,
                BackColor = Color.Transparent,
            };
            foreach (var (txt, offset) in new[] { ("◀", -1), ("Today", 0), ("▶", 1) })
            {
                int o = offset;
                var b = new Button
                {
                    Text = txt,
                    Width = txt == "Today" ? 56 : 28,
                    Height = 24,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(50, 255, 255, 255),
                    ForeColor = isToday ? Color.White : C_TextDark,
                    Font = new Font("Segoe UI", 8f),
                    Cursor = Cursors.Hand,
                    Margin = new Padding(2),
                };
                b.FlatAppearance.BorderSize = 0;
                b.Click += (s, e) =>
                {
                    _currentDayDate = o == 0 ? DateTime.Now.Date : _currentDayDate.AddDays(o);
                    BuildDailyView();
                    UpdateViewTitle();
                };
                navStrip.Controls.Add(b);
            }
            navStrip.Left = outer.Width - navStrip.PreferredSize.Width - 12;
            navStrip.Top = (40 - 28) / 2;
            dayStrip.Controls.Add(navStrip);
            navStrip.BringToFront();

            dayStrip.Resize += (s, e) =>
            {
                navStrip.Left = dayStrip.Width - navStrip.PreferredSize.Width - 12;
                dayStrip.Width = outer.ClientSize.Width;
            };
            outer.Controls.Add(dayStrip);

            var scroll = new Panel
            {
                Left = 0,
                Top = 40,
                Width = outer.Width,
                Height = outer.Height - 40,
                AutoScroll = true,
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
            };
            outer.Controls.Add(scroll);

            outer.Resize += (s, e) =>
            {
                dayStrip.Width = outer.ClientSize.Width;
                scroll.Width = outer.ClientSize.Width;
                scroll.Height = outer.ClientSize.Height - 40;
            };

            var timelinePanel = new Panel
            {
                Left = 0,
                Top = 0,
                Width = Math.Max(scroll.ClientSize.Width, 400),
                Height = totalH + 20,
                BackColor = Color.White,
            };
            scroll.Controls.Add(timelinePanel);

            for (int h = 0; h < 24; h++)
            {
                var hlbl = new Label
                {
                    Text = $"{h:00}:00",
                    Left = 0,
                    Top = h * hourH,
                    Width = timeColW,
                    Height = hourH,
                    TextAlign = ContentAlignment.TopRight,
                    Font = new Font("Segoe UI", 7.5f),
                    ForeColor = C_TextLight,
                    Padding = new Padding(0, 4, 6, 0),
                    BackColor = Color.Transparent,
                };
                timelinePanel.Controls.Add(hlbl);

                var hline = new Panel
                {
                    Left = timeColW,
                    Top = h * hourH,
                    Width = timelinePanel.Width - timeColW,
                    Height = 1,
                    BackColor = C_Border,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                };
                timelinePanel.Controls.Add(hline);

                var halfline = new Panel
                {
                    Left = timeColW,
                    Top = h * hourH + hourH / 2,
                    Width = timelinePanel.Width - timeColW,
                    Height = 1,
                    BackColor = Color.FromArgb(235, 235, 240),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                };
                timelinePanel.Controls.Add(halfline);
            }

            if (isToday)
            {
                double nowFrac = DateTime.Now.TimeOfDay.TotalMinutes / (24 * 60);
                int nowY = (int)(nowFrac * totalH);

                var nowLine = new Panel
                {
                    Left = timeColW - 6,
                    Top = nowY,
                    Width = timelinePanel.Width - timeColW + 6,
                    Height = 2,
                    BackColor = C_Primary,
                    Anchor = AnchorStyles.Left | AnchorStyles.Right,
                };
                var nowDot = new Panel
                {
                    Left = timeColW - 8,
                    Top = nowY - 4,
                    Width = 10,
                    Height = 10,
                    BackColor = C_Primary,
                };
                MakeCircular(nowDot);
                timelinePanel.Controls.Add(nowLine);
                timelinePanel.Controls.Add(nowDot);
                nowLine.BringToFront();
                nowDot.BringToFront();

                scroll.AutoScrollPosition = new Point(0, Math.Max(0, nowY - scroll.Height / 2));
            }
            else
            {
                scroll.AutoScrollPosition = new Point(0, 7 * hourH);
            }

            var allEvs = SharedCalendarData.GetEventsForDate(day)
                .Select(e => (e, false))
                .Concat(SharedCalendarData.GetStudentEventsForDate(day).Select(e => (e, true)))
                .ToList();

            var allDay = allEvs.Where(x => string.IsNullOrEmpty(x.Item1.StartTime)).ToList();
            var timed = allEvs.Where(x => !string.IsNullOrEmpty(x.Item1.StartTime)).ToList();

            if (allDay.Count > 0)
            {
                var adPanel = new Panel
                {
                    Left = timeColW,
                    Top = 4,
                    Width = timelinePanel.Width - timeColW - 8,
                    Height = allDay.Count * 22 + 4,
                    BackColor = Color.FromArgb(248, 248, 255),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                };
                int ay = 2;
                foreach (var (ev, isP) in allDay)
                {
                    Color ec = ev.GetColor();
                    var eb = new Panel
                    {
                        Left = 0,
                        Top = ay,
                        Width = adPanel.Width - 2,
                        Height = 20,
                        BackColor = Color.FromArgb(200, ec.R, ec.G, ec.B),
                        Cursor = Cursors.Hand,
                        Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                    };
                    string prefix = isP ? "🔒 " : "";
                    eb.Controls.Add(new Label
                    {
                        Text = prefix + $"[{ev.GetTypeLabel()}] {ev.Title}",
                        Dock = DockStyle.Fill,
                        Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                        ForeColor = Color.White,
                        BackColor = Color.Transparent,
                        Padding = new Padding(4, 0, 0, 0),
                        AutoEllipsis = true,
                    });
                    var capturedEv = ev;
                    eb.Click += (s, e) => ShowEventDetailDialog(capturedEv, day);
                    eb.Controls[0].Click += (s, e) => ShowEventDetailDialog(capturedEv, day);
                    adPanel.Controls.Add(eb);
                    ay += 22;
                }
                timelinePanel.Controls.Add(adPanel);
                var adLabel = new Label
                {
                    Text = "ALL DAY",
                    Left = 0,
                    Top = 8,
                    Width = timeColW,
                    Height = allDay.Count * 22,
                    TextAlign = ContentAlignment.TopRight,
                    Font = new Font("Segoe UI", 6.5f, FontStyle.Bold),
                    ForeColor = C_TextLight,
                    Padding = new Padding(0, 4, 6, 0),
                };
                timelinePanel.Controls.Add(adLabel);
                adPanel.BringToFront();
                adLabel.BringToFront();
            }

            foreach (var (ev, isP) in timed)
                PlaceDayEventBlock(timelinePanel, ev, isP, day, hourH, timeColW, timelinePanel.Width);

            timelinePanel.Resize += (s, e) =>
            {
                int tw = timelinePanel.Width;
                foreach (Control c in timelinePanel.Controls)
                    if (c.Anchor.HasFlag(AnchorStyles.Right))
                        c.Width = tw - c.Left - 8;
            };

            if (allEvs.Count == 0)
            {
                timelinePanel.Controls.Add(new Label
                {
                    Text = "No events scheduled for this day.",
                    Left = timeColW + 16,
                    Top = 8 * hourH + 8,
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9f, FontStyle.Italic),
                    ForeColor = C_TextLight,
                });
            }
        }

        private void PlaceDayEventBlock(Panel container, CalendarEvent ev, bool isPersonal, DateTime day, int hourH, int timeColW, int panelWidth)
        {
            int topY = 8 * hourH;
            int blockH = hourH - 6;

            if (TimeSpan.TryParse(ev.StartTime, out TimeSpan t1))
                topY = (int)(t1.TotalMinutes / 60.0 * hourH);

            if (!string.IsNullOrEmpty(ev.EndTime) &&
                TimeSpan.TryParse(ev.EndTime, out TimeSpan t2) && t2 > t1)
                blockH = Math.Max(22, (int)((t2 - t1).TotalMinutes / 60.0 * hourH) - 4);

            Color c = ev.GetColor();
            Color bg = isPersonal ? Color.FromArgb(220, c.R, c.G, c.B) : c;

            var block = new Panel
            {
                Left = timeColW + 4,
                Top = topY + 1,
                Width = panelWidth - timeColW - 16,
                Height = blockH,
                BackColor = bg,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            };

            var bar = new Panel { Left = 0, Top = 0, Width = 4, Height = blockH, BackColor = ControlPaint.Dark(c, 0.2f) };
            block.Controls.Add(bar);

            string prefix = isPersonal ? "🔒 " : "";
            string timeText = ev.StartTime + (string.IsNullOrEmpty(ev.EndTime) ? "" : $" – {ev.EndTime}");

            var titleLbl = new Label
            {
                Text = prefix + $"[{ev.GetTypeLabel()}] {ev.Title}",
                Left = 8,
                Top = 2,
                Width = block.Width - 12,
                Height = 18,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoEllipsis = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            };

            var timeLbl = new Label
            {
                Text = timeText,
                Left = 8,
                Top = 20,
                Width = block.Width - 12,
                Height = 14,
                Font = new Font("Segoe UI", 7f),
                ForeColor = Color.FromArgb(220, 255, 255, 255),
                BackColor = Color.Transparent,
                AutoEllipsis = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Visible = blockH >= 36,
            };

            if (!string.IsNullOrEmpty(ev.Room) && blockH >= 52)
            {
                var roomLbl = new Label
                {
                    Text = "Rm " + ev.Room,
                    Left = 8,
                    Top = 34,
                    Width = block.Width - 12,
                    Height = 14,
                    Font = new Font("Segoe UI", 7f),
                    ForeColor = Color.FromArgb(210, 255, 255, 255),
                    BackColor = Color.Transparent,
                    AutoEllipsis = true,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                };
                block.Controls.Add(roomLbl);
            }

            block.Controls.Add(titleLbl);
            block.Controls.Add(timeLbl);

            var capturedEv = ev;
            var capturedDay = day;
            block.Click += (s, e) => ShowEventDetailDialog(capturedEv, capturedDay);
            titleLbl.Click += (s, e) => ShowEventDetailDialog(capturedEv, capturedDay);
            timeLbl.Click += (s, e) => ShowEventDetailDialog(capturedEv, capturedDay);

            container.Controls.Add(block);
            block.BringToFront();
        }
        private void ShowEventDetailDialog(CalendarEvent ev, DateTime date)
        {
            using var frm = new Form
            {
                Text = "Event Details",
                Size = new Size(420, 340),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White,
            };

            Color hc = ev.GetColor();
            var hdr = new Panel { Dock = DockStyle.Top, Height = 52, BackColor = hc };
            var hTitle = new Label
            {
                Text = $"[{ev.GetTypeLabel()}]  {ev.Title}",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(14, 0, 0, 0),
                BackColor = Color.Transparent,
            };
            hdr.Controls.Add(hTitle);
            frm.Controls.Add(hdr);

            var body = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16, 12, 16, 12) };
            int y = 12;

            void AddRow(string icon, string label, string value)
            {
                if (string.IsNullOrWhiteSpace(value)) return;
                var row = new Panel { Left = 0, Top = y, Width = frm.ClientSize.Width - 32, Height = 26, BackColor = Color.Transparent };
                row.Controls.Add(new Label { Text = icon + "  " + label + ":", Left = 0, Top = 4, Width = 90, AutoSize = false, Height = 18, Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = C_TextMid });
                row.Controls.Add(new Label { Text = value, Left = 96, Top = 4, Width = row.Width - 100, AutoSize = false, Height = 18, Font = new Font("Segoe UI", 8.5f), ForeColor = C_TextDark, AutoEllipsis = true });
                body.Controls.Add(row);
                y += 28;
            }

            if (date != DateTime.MinValue)
                AddRow("📅", "Date", date.ToString("dddd, MMMM dd, yyyy"));

            string timeStr = "";
            if (!string.IsNullOrEmpty(ev.StartTime))
                timeStr = ev.StartTime + (string.IsNullOrEmpty(ev.EndTime) ? "" : $" – {ev.EndTime}");
            AddRow("⏰", "Time", timeStr);
            AddRow("🏫", "Room", ev.Room);

            if (!string.IsNullOrEmpty(ev.Description))
            {
                body.Controls.Add(new Label
                {
                    Text = "📝  Description:",
                    Left = 0,
                    Top = y + 2,
                    AutoSize = true,
                    Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                    ForeColor = C_TextMid,
                });
                y += 22;

                var txtDesc = new RichTextBox
                {
                    Left = 0,
                    Top = y,
                    Width = frm.ClientSize.Width - 32,
                    Height = 80,
                    Text = ev.Description,
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = C_TextDark,
                    BackColor = Color.FromArgb(248, 248, 252),
                    ReadOnly = true,
                    BorderStyle = BorderStyle.None,
                    ScrollBars = RichTextBoxScrollBars.Vertical,
                };
                body.Controls.Add(txtDesc);
            }

            frm.Controls.Add(body);

            var btnClose = new Button
            {
                Text = "Close",
                Width = 90,
                Height = 30,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                FlatStyle = FlatStyle.Flat,
                BackColor = hc,
                ForeColor = Color.White,
                DialogResult = DialogResult.OK,
                Font = new Font("Segoe UI", 9f),
            };
            btnClose.FlatAppearance.BorderSize = 0;
            frm.Controls.Add(btnClose);
            frm.AcceptButton = btnClose;

            frm.Shown += (s, e) =>
            {
                btnClose.Left = frm.ClientSize.Width - btnClose.Width - 16;
                btnClose.Top = frm.ClientSize.Height - btnClose.Height - 10;
            };

            frm.ShowDialog();
        }

        private void BuildBottomPanel()
        {
            const int BOTTOM_H = 240;

            pnlBottom = new Panel
            {
                BackColor = Color.White,
                Height = BOTTOM_H,
                Left = 0,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
            };
            pnlBottom.Parent = pnlCalendar;
            pnlBottom.BringToFront();

            pnlBottom.Controls.Add(new Panel { Height = 1, Dock = DockStyle.Top, BackColor = C_Border });

            var pnlLeft = new Panel
            {
                Left = 0,
                Top = 4,
                Width = 500,
                Height = BOTTOM_H - 8,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom,
                Name = "pnlLeft",
            };
            pnlBottom.Controls.Add(pnlLeft);

            lblSelectedDate = new Label
            {
                Text = "Select a day to see details",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = C_Primary,
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
                Width = 108,
                Height = 26,
                BackColor = C_Accent,
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
                    RefreshDayDetail(_lastSelectedDate);
                    HighlightActiveFilter();
                };
                pnlLeft.Controls.Add(btn);
                bx += btn.Width + 6;
            }

            flpDayEvents = new FlowLayoutPanel
            {
                Left = 0,
                Top = 62,
                Width = 486,
                Height = BOTTOM_H - 70,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
            };
            lblNoEvents = new Label { Text = "No events for this day.", ForeColor = Color.Gray, AutoSize = true, Padding = new Padding(8) };
            flpDayEvents.Controls.Add(lblNoEvents);
            pnlLeft.Controls.Add(flpDayEvents);

            pnlBottom.Controls.Add(new Panel
            {
                Left = 508,
                Top = 8,
                Width = 1,
                Height = BOTTOM_H - 16,
                BackColor = C_Border,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left,
            });

            var pnlRight = new Panel
            {
                Left = 516,
                Top = 4,
                Height = BOTTOM_H - 8,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                Name = "pnlRight",
            };
            pnlBottom.Controls.Add(pnlRight);

            pnlRight.Controls.Add(new Label
            {
                Text = "Upcoming",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = C_TextDark,
                Left = 8,
                Top = 6,
                AutoSize = true,
            });

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

        private void PositionBottomPanel()
        {
            if (pnlBottom == null || pnlCalendar == null) return;
            pnlBottom.Width = pnlCalendar.ClientSize.Width;
            pnlBottom.Top = pnlCalendar.ClientSize.Height - pnlBottom.Height;
            pnlBottom.Left = 0;

            foreach (Control c in pnlBottom.Controls)
                if (c is Panel rp && rp.Name == "pnlRight")
                    rp.Width = pnlBottom.Width - rp.Left - 8;
        }

        private void QuickAddStudentEvent()
        {
            using var dlg = new AddEventForm(_lastSelectedDate);
            if (dlg.ShowDialog() == DialogResult.OK && dlg.CreatedEvent != null)
            {
                SharedCalendarData.AddStudentEvent(_lastSelectedDate, dlg.CreatedEvent);
                RefreshMonthPills();
                RefreshDayDetail(_lastSelectedDate);
                RefreshUpcoming();
                ShowReminders();
            }
        }

        private void RefreshMonthPills()
        {
            if (_fpMonth == null) return;
            foreach (Control ctrl in _fpMonth.Controls)
                if (ctrl is UrDay ud) ud.RefreshEventPills();
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
            foreach (Control c in found[0].Controls)
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
                parent.Controls.Add(new Panel { Width = 10, Height = 10, BackColor = c, Left = cx, Top = y + 2 });
                parent.Controls.Add(new Label { Text = name, Left = cx + 13, Top = y, AutoSize = true, Font = new Font("Segoe UI", 8f), ForeColor = C_TextMid });
                cx += 72;
            }
        }

        private void OnDaySelected(DateTime date)
        {
            if (_fpMonth != null)
                foreach (Control ctrl in _fpMonth.Controls)
                    if (ctrl is UrDay ud) ud.IsSelected = false;

            RefreshDayDetail(date);
            RefreshUpcoming();
        }

        private DateTime GetSelectedDate() => _lastSelectedDate;

        private void RefreshDayDetail(DateTime date)
        {
            _lastSelectedDate = date;
            lblSelectedDate.Text = date.ToString("dddd, MMMM dd, yyyy");

            flpDayEvents.Controls.Clear();

            var sharedEvs = SharedCalendarData.GetEventsForDate(date)
                .Where(ev => _activeFilter == null || ev.Type == _activeFilter).ToList();
            var personalEvs = SharedCalendarData.GetStudentEventsForDate(date)
                .Where(ev => _activeFilter == null || ev.Type == _activeFilter).ToList();

            var noteDict = SharedCalendarData.StudentNotes;
            if (noteDict.ContainsKey(date.Date) && !string.IsNullOrWhiteSpace(noteDict[date.Date]))
                flpDayEvents.Controls.Add(
                    MakeEventCard("🗒 Note", noteDict[date.Date], Color.FromArgb(100, 100, 100), date, isPersonal: false));

            if (sharedEvs.Count == 0 && personalEvs.Count == 0 && flpDayEvents.Controls.Count == 0)
            {
                flpDayEvents.Controls.Add(lblNoEvents);
                return;
            }

            foreach (var ev in sharedEvs)
            {
                string body = (string.IsNullOrEmpty(ev.StartTime) ? "" : ev.StartTime + (string.IsNullOrEmpty(ev.EndTime) ? "" : " – " + ev.EndTime) + "\n")
                            + (string.IsNullOrEmpty(ev.Room) ? "" : "Room: " + ev.Room + "\n")
                            + ev.Description;
                var card = MakeEventCard($"[{ev.GetTypeLabel()}]  {ev.Title}", body, ev.GetColor(), date, ev, isPersonal: false);
                flpDayEvents.Controls.Add(card);
            }

            if (personalEvs.Count > 0 && sharedEvs.Count > 0)
                flpDayEvents.Controls.Add(new Label
                {
                    Text = "── My Personal Events ──",
                    Font = new Font("Segoe UI", 7.5f, FontStyle.Italic),
                    ForeColor = C_TextLight,
                    AutoSize = true,
                    Padding = new Padding(6, 4, 0, 0),
                });

            foreach (var ev in personalEvs)
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

            if (upcoming.Count == 0) { flpUpcoming.Controls.Add(lblNoUpcoming); return; }

            foreach (var (d, ev) in upcoming)
            {
                int daysLeft = (d.Date - DateTime.Now.Date).Days;
                string when = daysLeft == 0 ? "Today" : daysLeft == 1 ? "Tomorrow" : $"In {daysLeft} days";
                bool isP = SharedCalendarData.GetStudentEventsForDate(d).Contains(ev);
                flpUpcoming.Controls.Add(MakeUpcomingStrip(ev, d, when, isP));
            }
        }

        private Panel MakeEventCard(string title, string body, Color accent, DateTime date,
            CalendarEvent ev = null, bool isPersonal = false)
        {
            Color bg = isPersonal ? Color.FromArgb(240, 248, 255) : Color.FromArgb(245, 248, 255);
            var card = new Panel
            {
                Width = flpDayEvents.Width - 16,
                Height = 50,
                BackColor = bg,
                Margin = new Padding(4, 3, 4, 0),
                Cursor = Cursors.Default,
            };

            Color barColor = isPersonal ? Color.FromArgb(160, accent.R, accent.G, accent.B) : accent;
            card.Controls.Add(new Panel { Width = 5, Height = card.Height, Left = 0, Top = 0, BackColor = barColor });

            card.Controls.Add(new Label
            {
                Text = title,
                Left = 12,
                Top = 3,
                Width = card.Width - (isPersonal ? 60 : 20),
                Font = new Font("Segoe UI", 8.5f, isPersonal ? FontStyle.Italic : FontStyle.Bold),
                ForeColor = C_TextDark,
                AutoSize = false,
                Height = 18,
                AutoEllipsis = true,
            });
            card.Controls.Add(new Label
            {
                Text = body.Trim(),
                Left = 12,
                Top = 22,
                Width = card.Width - (isPersonal ? 60 : 20),
                Font = new Font("Segoe UI", 8f),
                ForeColor = C_TextMid,
                AutoSize = false,
                Height = 22,
                AutoEllipsis = true,
            });

            if (ev != null)
            {
                var capturedEv = ev;
                var capturedDay = date;
                card.Cursor = Cursors.Hand;
                card.Click += (s, e) => ShowEventDetailDialog(capturedEv, capturedDay);
                foreach (Control c in card.Controls) c.Click += (s, e) => ShowEventDetailDialog(capturedEv, capturedDay);
            }

            if (isPersonal && ev != null)
            {
                var btnDel = new Button
                {
                    Text = "✕",
                    Left = card.Width - 30,
                    Top = 14,
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
                        RefreshMonthPills();
                        RefreshDayDetail(date);
                        RefreshUpcoming();
                    }
                };
                card.Controls.Add(btnDel);
            }

            return card;
        }

        private Panel MakeUpcomingStrip(CalendarEvent ev, DateTime date, string when, bool isPersonal = false)
        {
            var strip = new Panel
            {
                Width = flpUpcoming.Width - 8,
                Height = 38,
                BackColor = isPersonal ? Color.FromArgb(245, 250, 255) : Color.White,
                Margin = new Padding(4, 2, 4, 0),
                Cursor = Cursors.Hand,
            };

            Color dotColor = isPersonal ? Color.FromArgb(150, ev.GetColor().R, ev.GetColor().G, ev.GetColor().B) : ev.GetColor();
            strip.Controls.Add(new Panel { Width = 8, Height = 8, Top = 15, Left = 4, BackColor = dotColor });

            string displayTitle = isPersonal ? "🔒 " + ev.Title : ev.Title;
            strip.Controls.Add(new Label { Text = displayTitle, Left = 18, Top = 3, Width = strip.Width - 80, Font = new Font("Segoe UI", 8.5f, isPersonal ? FontStyle.Italic : FontStyle.Bold), ForeColor = C_TextDark, AutoSize = false, Height = 18, AutoEllipsis = true });
            strip.Controls.Add(new Label { Text = when, Left = 18, Top = 21, Width = strip.Width - 80, Font = new Font("Segoe UI", 7.5f), ForeColor = Color.Gray, AutoSize = false, Height = 14 });
            strip.Controls.Add(new Label { Text = date.ToString("MMM dd"), Left = strip.Width - 58, Top = 12, Width = 54, Font = new Font("Segoe UI", 8f), ForeColor = C_TextMid, AutoSize = false, TextAlign = ContentAlignment.MiddleRight });

            var capturedEv = ev;
            var capturedDay = date;
            strip.Click += (s, e) => ShowEventDetailDialog(capturedEv, capturedDay);
            foreach (Control c in strip.Controls) c.Click += (s, e) => ShowEventDetailDialog(capturedEv, capturedDay);

            return strip;
        }

        private void OnFormResized(object sender, EventArgs e)
        {
            if (!this.IsHandleCreated || this.IsDisposed) return;
            this.BeginInvoke((Action)(() =>
            {
                try
                {
                    PositionReminderBanner();
                    PositionViewBody();
                    PositionBottomPanel();
                    if (_view == CalendarView.Monthly) ResizeMonthlyBody();
                }
                catch { }
            }));
        }

        private static DateTime StartOfWeek(DateTime dt)
        {
            int diff = (7 + (int)dt.DayOfWeek) % 7;
            return dt.AddDays(-diff).Date;
        }

        private static void MakeCircular(Panel p)
        {
            var gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, p.Width, p.Height);
            p.Region = new Region(gp);
        }

        private string GetHoliday(int year, int month, int day)
        {
            var date = new DateTime(year, month, day);
            return SharedCalendarData.Holidays.ContainsKey(date) ? SharedCalendarData.Holidays[date] : "";
        }

        private void picNext_Click(object sender, EventArgs e) => NavigateNext();
        private void picPrev_Click(object sender, EventArgs e) => NavigatePrev();
    }
}