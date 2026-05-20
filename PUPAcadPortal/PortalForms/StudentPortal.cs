using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
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
        private const int SidePadding = 16;
        private const int CardGap = 10;
        private List<string[]> enrollmentData = new List<string[]>();
        private Form _parentForm;

        private EventType? _activeFilter = null; 
        private SizeF _designSize;
        private readonly Dictionary<Control, RectangleF> _origBounds = new();
        private readonly Dictionary<Control, float> _origFontSz = new();

        private class StudentAnnouncement
        {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Category { get; set; } = "General";
            public string OfficeName { get; set; } = "Admin Office";
            public string InstructorName { get; set; } = string.Empty;
            public DateTime Date { get; set; } = DateTime.Now;
            public bool IsUrgent { get; set; }
            public bool IsPinned { get; set; }
            public bool IsRead { get; set; }
            public string Status { get; set; } = "active";
        }

        private List<StudentAnnouncement> _announcements = new();
        private string _annCategoryFilter = "All Categories";
        private string _annSortOrder = "Latest First";
        private string _annSearchText = "";

        private ViewAnnouncementStudent _viewStudentUC;

        private static readonly Dictionary<string, Color> CatIconColor = new()
        {
            ["General"] = Color.FromArgb(55, 138, 221),
            ["Academic"] = Color.FromArgb(99, 153, 34),
            ["Schedule"] = Color.FromArgb(186, 117, 23),
            ["Events"] = Color.FromArgb(212, 83, 126),
            ["Examinations"] = Color.FromArgb(127, 119, 221),
            ["Administrative"] = Color.FromArgb(90, 90, 200),
            ["Urgent"] = Color.FromArgb(220, 50, 50),
        };
        private static readonly Dictionary<string, Color> CatBgColor = new()
        {
            ["General"] = Color.FromArgb(230, 241, 251),
            ["Academic"] = Color.FromArgb(234, 243, 222),
            ["Schedule"] = Color.FromArgb(250, 238, 218),
            ["Events"] = Color.FromArgb(251, 234, 240),
            ["Examinations"] = Color.FromArgb(238, 237, 254),
            ["Administrative"] = Color.FromArgb(230, 230, 245),
            ["Urgent"] = Color.FromArgb(255, 235, 235),
        };

        private UrDay _selectedCell;

        public StudentPortal(Form parent)
        {
            InitializeComponent();
            SharedCalendarData.LoadData();
            BuildDayHeaders();
            BuildBottomPanel();
            txtEnrollSearch.KeyDown += txtEnrollSearch_KeyDown;
            //this.Resize += StudentPortal_EnrollmentResize; Breaks enrollment bottom cards, instead of being in the middle: Brylle
            this.Resize += StudentPortal_AccountsResize;
            pnlContainerStudentPortal.Dock = DockStyle.Fill;
            pnlContainerStudentPortal.AutoScroll = true;
            dropSubjectToolStripMenuItem.Click += dropSubjectToolStripMenuItem_Click;
            btnSaveAndAssess.Click += btnSaveAndAssess_Click;
            Form _parentForm = parent;

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

            this.Load += StudentPortal_Load;

            string[] mrow1 = { "Integrated Programming and Technologies 1", "1.00", "P" };
            string[] mrow2 = { "Principles of Accounting", "1.00", "P" };

            dgvMidtermGradeStudent.Rows.Add(mrow1);
            dgvMidtermGradeStudent.Rows.Add(mrow2);

            string[] frow1 = { "Objected Oriented Programming", "1.00", "P" };
            string[] frow2 = { "PATHFIT 4", "1.00", "P" };

            dgvFinalGradeStudent.Rows.Add(frow1);
            dgvFinalGradeStudent.Rows.Add(frow2);
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
            pnlYellow.Parent = clickedButton;
            pnlYellow.BringToFront();
            clickedButton.BackColor = selectedColor;
        }

        //Method na nagpapakita ng content ng bawat button, wala akong maisip na iba kaya eto
        private void showContent(Button button)
        {
            Dictionary<Button, Panel> contents = new Dictionary<Button, Panel>
            {
                { btnDashboard, pnlDashboardContent },
                { btnEnrollment, pnlEnrollContent },
                { btnAccounts, pnlAccountsContentHolder  },
                { btnAnnounce, pnlAnnounce  },
                { btnCalendar, pnlCalendar  },
                { btnSubject, pnlSubject  },
                { btnActivities, pnlActivities  },
                { btnAttendance, pnlAttendance  },
                { btnGrade, pnlGrades  },
            };
            //Kada button na aadd, maglagay ng panel sa form at lagay dito
            foreach (KeyValuePair<Button, Panel> content in contents)
            {
                if (content.Key == clickedButton)
                {
                    //Automatic positioning, wag pakialaman maliban nalang kung binago ang position ng sidebar
                    content.Value.Parent = pnlContainerStudentPortal;
                    content.Value.Dock = DockStyle.Fill;
                    content.Value.Visible = true;
                    content.Value.BringToFront();
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
            //else
            //{
            //    if (MessageBox.Show("Are you sure you want to Logout", "Log Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //    {
            //        e.Cancel = true;
            //    }
            //    else
            //        _parentForm.Show();
            //}
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
            pnllmsSubmenu.Visible = !pnllmsSubmenu.Visible;
            if (pnllmsSubmenu.Visible)
                btnLMS.Text = " LMS                                       ⌄";
            else
                btnLMS.Text = " LMS                                        ›";
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnAnnounce_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
            OnFormResized(this, EventArgs.Empty);
        }

        private void btnSubject_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnActivities_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnGrade_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
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
            rpnlGradeBreakdown.Visible = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            rpnlGradeBreakdown.Visible = true;
            rpnlGradeBreakdown.BringToFront();
        }

        private void btnViewGrades_Click(object sender, EventArgs e)
        {

        }


        private void btnThisWeek_Click(object sender, EventArgs e)
        {
            pnlTW.BringToFront();
            pnlTW.Visible = true;
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            pnlToday.BringToFront();
            pnlToday.Visible = true;
        }

        private void btnNextWeek_Click(object sender, EventArgs e)
        {
            pnlNW.BringToFront();
            pnlNW.Visible = true;
        }

        private void buttonRounded2_Click(object sender, EventArgs e)
        {
            pnlRA.BringToFront();
            pnlRA.Visible = true;
        }

        private void buttonRounded6_Click(object sender, EventArgs e)
        {
            pnlAll.BringToFront();
            pnlAll.Visible = true;
            pnlVA.Visible = false;
        }

        private void buttonRounded7_Click(object sender, EventArgs e)
        {
            pnlAll.BringToFront();
            pnlAll.Visible = true;
            pnlVA.Visible = false;
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            pnlAll.BringToFront();
            pnlAll.Visible = true;
        }

        private void buttonRounded1_Click(object sender, EventArgs e)
        {
            pnlVA.BringToFront();
            pnlVA.Visible = true;
        }

        private void buttonRounded9_Click(object sender, EventArgs e)
        {
            pnlVA.BringToFront();
            pnlVA.Visible = true;
        }

        private void buttonRounded10_Click(object sender, EventArgs e)
        {
            pnlRA.BringToFront();
            pnlRA.Visible = true;
        }

        private void buttonRounded15_Click(object sender, EventArgs e)
        {
            pnlAll.BringToFront();
            pnlAll.Visible = true;
            pnlRA.Visible = false;
        }

        private void StudentPortal_Load(object sender, EventArgs e)
        {
            Enrollment_Initialize();
            Accounts_Initialize();
            btnDashboard.PerformClick();
            SetupMaroonBorders();

            flpAnnouncements.AutoScroll = false;
            flpAnnouncements.FlowDirection = FlowDirection.TopDown;
            flpAnnouncements.WrapContents = false;
            flpAnnouncements.HorizontalScroll.Enabled = false;
            flpAnnouncements.HorizontalScroll.Visible = false;
            flpAnnouncements.AutoScroll = true;

            _viewStudentUC = new ViewAnnouncementStudent
            {
                Visible = false,
                Anchor = AnchorStyles.None,
            };
            _viewStudentUC.CloseRequested += (s, ev) =>
            {
                _viewStudentUC.Visible = false;
                RenderAnnouncements();
                BuildInsightsPanel();
            };
            pnlAnnounce.Controls.Add(_viewStudentUC);

            SeedAnnouncements();
            WireAnnouncementControls();
            RenderAnnouncements();
            BuildCategoryPanel();
            BuildInsightsPanel();

            _designSize = new SizeF(this.ClientSize.Width, this.ClientSize.Height);
            SnapshotControls(this.Controls);
            this.Resize += StudentPortal_ResizeHandler;
        }

        private void SnapshotControls(Control.ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl.Tag is string t && t.Contains("noScale")) continue;
                _origBounds[ctrl] = new RectangleF(ctrl.Left, ctrl.Top, ctrl.Width, ctrl.Height);
                _origFontSz[ctrl] = ctrl.Font.Size;
                if (ctrl.HasChildren) SnapshotControls(ctrl.Controls);
            }
        }
        private void StudentPortal_ResizeHandler(object sender, EventArgs e)
        {
            if (_designSize.Width == 0 || _designSize.Height == 0) return;

            float rx = Math.Max(this.ClientSize.Width, 1024) / _designSize.Width;
            float ry = Math.Max(this.ClientSize.Height, 700) / _designSize.Height;

            this.SuspendLayout();
            ScaleControls(this.Controls, rx, ry);
            this.ResumeLayout(true);

            if (_viewStudentUC != null && _viewStudentUC.Visible)
                CentreInPanel(_viewStudentUC, pnlAnnounce);

            RenderAnnouncements();
            BuildCategoryPanel();
            BuildPinnedPanel();
            BuildInsightsPanel();
        }

        private void ScaleControls(Control.ControlCollection controls, float rx, float ry)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl.Tag is string t && t.Contains("noScale")) continue;
                if (!_origBounds.TryGetValue(ctrl, out RectangleF ob)) continue;

                int newX = R(ob.X * rx);
                int newY = R(ob.Y * ry);
                int newW = Math.Max(1, R(ob.Width * rx));
                int newH = Math.Max(1, R(ob.Height * ry));

                switch (ctrl.Dock)
                {
                    case DockStyle.Fill: break;
                    case DockStyle.Top: ctrl.Height = newH; break;
                    case DockStyle.Bottom: ctrl.Height = newH; break;
                    case DockStyle.Left: ctrl.Width = newW; break;
                    case DockStyle.Right: ctrl.Width = newW; break;
                    default: ctrl.SetBounds(newX, newY, newW, newH); break;
                }

                if (_origFontSz.TryGetValue(ctrl, out float origSz))
                {
                    float newSz = Math.Max(6f, origSz * Math.Min(rx, ry));
                    if (Math.Abs(ctrl.Font.Size - newSz) > 0.15f)
                    {
                        try { ctrl.Font = new Font(ctrl.Font.FontFamily, newSz, ctrl.Font.Style, GraphicsUnit.Point); }
                        catch { }
                    }
                }

                if (ctrl.HasChildren) ScaleControls(ctrl.Controls, rx, ry);
            }
        }

        private static int R(float v) => (int)Math.Round(v);

        private static void CentreInPanel(Control child, Control parent)
        {
            int maxW = Math.Max(200, parent.ClientSize.Width - 40);
            int maxH = Math.Max(100, parent.ClientSize.Height - 40);
            if (child.Width > maxW || child.Height > maxH)
            {
                float s = Math.Min((float)maxW / child.Width, (float)maxH / child.Height);
                child.Width = (int)(child.Width * s);
                child.Height = (int)(child.Height * s);
            }
            int x = (parent.ClientSize.Width - child.Width) / 2;
            int y = (parent.ClientSize.Height - child.Height) / 4;
            child.Location = new Point(Math.Max(0, x), Math.Max(0, y));
        }

        private void SeedAnnouncements()
        {
            _announcements = new List<StudentAnnouncement>
            {
                new() {
                    Id = 1, Title = "Updated Travel Reimbursement Policy",
                    Description   = "Please note that the mileage reimbursement rate for university-related travel has been adjusted. All reimbursement claims submitted after May 1, 2026 must use the new rate of ₱12.00 per km.",
                    Category = "Administrative", OfficeName = "Admin Office", InstructorName = "Dr. Reyes",
                    Date = new DateTime(2026, 4, 15, 10, 30, 0),
                    IsUrgent = true, IsPinned = true, IsRead = false,
                },
                new() {
                    Id = 2, Title = "Midterm Examination Schedule Released",
                    Description   = "The official midterm examination schedule for all BSIT 2nd year subjects is now available. Please check the LMS for your room assignments and bring your student ID on exam day.",
                    Category = "Examinations", OfficeName = "Registrar's Office", InstructorName = "Prof. Santos",
                    Date = new DateTime(2026, 4, 20, 8, 0, 0),
                    IsUrgent = true, IsPinned = true, IsRead = false,
                },
                new() {
                    Id = 3, Title = "Programming 1 – Lab Activity This Friday",
                    Description   = "Bring your laptops for the graded lab activity covering Modules 4 and 5. The activity will be conducted using Visual Studio 2022. No borrowing of equipment will be allowed.",
                    Category = "Academic", OfficeName = "CCIS Department", InstructorName = "Prof. Santos",
                    Date = new DateTime(2026, 4, 18, 9, 0, 0),
                    IsUrgent = false, IsPinned = false, IsRead = true,
                },
                new() {
                    Id = 4, Title = "PUP Foundation Day Celebration – May 17",
                    Description   = "Join us for the PUP Foundation Day celebration. Activities include a student showcase, cultural performances, and a technology exhibit. Attendance is encouraged for all students.",
                    Category = "Events", OfficeName = "Student Affairs Office", InstructorName = "Dr. Cruz",
                    Date = new DateTime(2026, 4, 10, 14, 0, 0),
                    IsUrgent = false, IsPinned = false, IsRead = false,
                },
                new() {
                    Id = 5, Title = "Reminder: Submit Assignment Outputs",
                    Description   = "All pending assignment outputs for Information Management must be submitted via the LMS portal before May 15 at 11:59 PM. Late submissions will not be accepted under any circumstance.",
                    Category = "Academic", OfficeName = "CCIS Department", InstructorName = "Prof. Santos",
                    Date = new DateTime(2026, 4, 8, 11, 0, 0),
                    IsUrgent = false, IsPinned = false, IsRead = true,
                },
                new() {
                    Id = 6, Title = "Library Hours Extended Until June",
                    Description   = "The university library will extend its operating hours to 8:00 AM – 9:00 PM on weekdays starting May 1 through June 30, 2026 to support students during the examination period.",
                    Category = "General", OfficeName = "Library Services", InstructorName = "Librarian Gomez",
                    Date = new DateTime(2026, 4, 5, 7, 30, 0),
                    IsUrgent = false, IsPinned = false, IsRead = false,
                },
                new() {
                    Id = 7, Title = "Enrollment for 2nd Semester Now Open",
                    Description   = "Online enrollment for the 2nd semester of Academic Year 2026–2027 is now open. Students must settle all outstanding balances and secure their assessment forms before enrolling. Visit the Registrar's portal for the step-by-step guide.",
                    Category = "General", OfficeName = "Registrar's Office", InstructorName = "Registrar Dela Torre",
                    Date = new DateTime(2026, 5, 2, 8, 0, 0),
                    IsUrgent = false, IsPinned = false, IsRead = false,
                },
                new() {
                    Id = 8, Title = "Scholarship Application Deadline – May 20",
                    Description   = "All students applying for the CHED Scholarship and PUP Internal Scholarship must submit their complete documentary requirements to the Scholarship Office no later than May 20, 2026 at 5:00 PM. No extensions will be granted.",
                    Category = "Administrative", OfficeName = "Scholarship Office", InstructorName = "Dr. Valdez",
                    Date = new DateTime(2026, 5, 5, 9, 0, 0),
                    IsUrgent = true, IsPinned = false, IsRead = false,
                },
                new() {
                    Id = 9, Title = "Campus-Wide Maintenance: May 14 (No Classes)",
                    Description   = "There will be no classes on May 14, 2026 due to scheduled campus-wide electrical maintenance. All LMS deadlines falling on this date are automatically extended by 24 hours. Students are advised to plan accordingly.",
                    Category = "Schedule", OfficeName = "Facilities Management Office", InstructorName = "Engr. Bautista",
                    Date = new DateTime(2026, 5, 8, 7, 0, 0),
                    IsUrgent = true, IsPinned = true, IsRead = false,
                },
                new() {
                    Id = 10, Title = "Intramural Sports Registration Open",
                    Description   = "Registration for the Annual PUP Intramural Sports Festival is now open. Students interested in joining basketball, volleyball, badminton, or swimming events must register through their respective department coordinators before May 22, 2026.",
                    Category = "Events", OfficeName = "Student Affairs Office", InstructorName = "Coach Mendoza",
                    Date = new DateTime(2026, 5, 6, 10, 0, 0),
                    IsUrgent = false, IsPinned = false, IsRead = true,
                },
                new() {
                    Id = 11, Title = "Final Examination Coverage Posted",
                    Description   = "The official final examination coverage for all BSIT subjects has been posted on the LMS. Students are advised to review the coverage carefully and contact their respective professors for any clarifications. Good luck on your finals!",
                    Category = "Examinations", OfficeName = "CCIS Department", InstructorName = "Prof. Santos",
                    Date = new DateTime(2026, 5, 9, 8, 30, 0),
                    IsUrgent = false, IsPinned = false, IsRead = false,
                },
                new() {
                    Id = 12, Title = "Academic Integrity Seminar – May 16",
                    Description   = "All students are required to attend the Academic Integrity and Anti-Plagiarism Seminar on May 16, 2026 at 1:00 PM via Zoom. Attendance will be recorded and counted toward your class standing. The meeting link will be sent through your official university email.",
                    Category = "Academic", OfficeName = "CCIS Department", InstructorName = "Dr. Reyes",
                    Date = new DateTime(2026, 5, 10, 9, 0, 0),
                    IsUrgent = true, IsPinned = false, IsRead = false,
                },
                new() {
                    Id = 13, Title = "Lost & Found: Items at Security Office",
                    Description   = "Several items including IDs, umbrellas, and a laptop bag have been turned over to the Security Office near Gate 1. Owners may claim their belongings by presenting valid identification. Unclaimed items will be turned over to the Student Affairs Office after May 25, 2026.",
                    Category = "General", OfficeName = "Security Office", InstructorName = "Guard Navarro",
                    Date = new DateTime(2026, 5, 7, 14, 0, 0),
                    IsUrgent = false, IsPinned = false, IsRead = true,
                },
                new() {
                    Id = 14, Title = "IT Career Fair – May 22 at PUP Gymnasium",
                    Description   = "The PUP Career Services Office invites all BSIT students to the annual IT Career Fair on May 22, 2026 from 9:00 AM to 4:00 PM at the university gymnasium. Over 30 technology companies will be present. Bring printed resumes and dress business casual.",
                    Category = "Events", OfficeName = "Career Services Office", InstructorName = "Dr. Cruz",
                    Date = new DateTime(2026, 5, 8, 10, 0, 0),
                    IsUrgent = false, IsPinned = true, IsRead = false,
                },
                new() {
                    Id = 15, Title = "Class Schedule Adjustment – May 13",
                    Description   = "Due to the university-wide convocation, all morning classes on May 13, 2026 are moved to the afternoon session. Students with afternoon conflicts are advised to coordinate with their respective professors. The updated schedule is posted on the Registrar's portal.",
                    Category = "Schedule", OfficeName = "Registrar's Office", InstructorName = "Registrar Dela Torre",
                    Date = new DateTime(2026, 5, 10, 7, 30, 0),
                    IsUrgent = true, IsPinned = false, IsRead = false,
                },
                new() {
                    Id = 16, Title = "Capstone Project Defense Schedule Released",
                    Description   = "The official schedule for 4th-year BSIT Capstone Project defenses has been published on the departmental bulletin board and LMS. All groups are required to submit their final manuscripts and slide decks at least three days before their assigned defense date. Coordinate with your adviser immediately.",
                    Category = "Academic", OfficeName = "CCIS Department", InstructorName = "Prof. Santos",
                    Date = new DateTime(2026, 5, 11, 8, 0, 0),
                    IsUrgent = false, IsPinned = true, IsRead = false,
                },
            };
        }
        private void WireAnnouncementControls()
        {
            txtSearch.TextChanged += (s, e) =>
            {
                _annSearchText = txtSearch.Text.Trim();
                RenderAnnouncements();
            };

            cmbCategory.SelectedIndexChanged += (s, e) =>
            {
                _annCategoryFilter = cmbCategory.Text;
                RenderAnnouncements();
                BuildCategoryPanel();
            };

            cmbSort.SelectedIndexChanged += (s, e) =>
            {
                _annSortOrder = cmbSort.Text;
                RenderAnnouncements();
            };

            btnMarkAllRead.Click += (s, e) =>
            {
                foreach (var a in _announcements) a.IsRead = true;
                RenderAnnouncements();
                BuildInsightsPanel();
            };
        }

        private void RenderAnnouncements()
        {
            flpAnnouncements.SuspendLayout();
            flpAnnouncements.Controls.Clear();
            flpAnnouncements.FlowDirection = FlowDirection.TopDown;
            flpAnnouncements.WrapContents = false;
            flpAnnouncements.AutoScroll = false;
            flpAnnouncements.HorizontalScroll.Enabled = false;
            flpAnnouncements.HorizontalScroll.Visible = false;
            flpAnnouncements.VerticalScroll.Enabled = true;
            flpAnnouncements.AutoScroll = true;

            IEnumerable<StudentAnnouncement> filtered = _announcements;

            if (_annCategoryFilter != "All Categories" && !string.IsNullOrEmpty(_annCategoryFilter))
                filtered = filtered.Where(a => a.Category == _annCategoryFilter);

            if (!string.IsNullOrWhiteSpace(_annSearchText))
            {
                string q = _annSearchText.ToLower();
                filtered = filtered.Where(a =>
                    a.Title.ToLower().Contains(q) ||
                    a.Description.ToLower().Contains(q) ||
                    a.OfficeName.ToLower().Contains(q));
            }

            List<StudentAnnouncement> sorted;

            if (_annSortOrder == "Oldest First")
            {
                sorted = filtered
                    .OrderBy(a => a.IsPinned ? 0 : 1)
                    .ThenBy(a => a.IsRead ? 1 : 0)
                    .ThenBy(a => a.Date)
                    .ToList();
            }
            else
            {
                sorted = filtered
                    .OrderBy(a => a.IsPinned ? 0 : 1)
                    .ThenBy(a => a.IsRead ? 1 : 0)
                    .ThenByDescending(a => a.Date)
                    .ToList();
            }

            int cardWidth = Math.Max(400, flpAnnouncements.ClientSize.Width - 22);

            foreach (var ann in sorted)
                flpAnnouncements.Controls.Add(BuildCard(ann, cardWidth));

            BuildPinnedPanel();
            flpAnnouncements.ResumeLayout();
        }

        private AnnouncementLayout BuildCard(StudentAnnouncement a, int cardWidth)
        {
            var card = new AnnouncementLayout();

            card.LoadStudent(
                id: a.Id,
                title: a.Title,
                description: a.Description,
                category: a.Category,
                officeName: a.OfficeName,
                date: a.Date,
                isUrgent: a.IsUrgent,
                isPinned: a.IsPinned,
                isRead: a.IsRead,
                cardWidth: cardWidth,
                instructorName: a.InstructorName);

            card.CardClicked += (s, id) =>
            {
                var ann = _announcements.Find(x => x.Id == id);
                if (ann != null) ShowStudentAnnouncementDetail(ann);
            };
            card.PinToggled += (s, id) =>
            {
                var ann = _announcements.Find(x => x.Id == id);
                if (ann != null)
                {
                    ann.IsPinned = !ann.IsPinned;
                    RenderAnnouncements();
                    BuildPinnedPanel();
                }
            };

            return card;
        }

        private void ShowStudentAnnouncementDetail(StudentAnnouncement a)
        {
            a.IsRead = true;

            _viewStudentUC.LoadAnnouncement(
                a.Title,
                a.Description,
                a.Category,
                a.OfficeName,
                a.Date,
                a.IsUrgent,
                a.IsPinned,
                a.InstructorName);

            CentreInPanel(_viewStudentUC, pnlAnnounce);

            _viewStudentUC.BringToFront();
            _viewStudentUC.Visible = true;

            RenderAnnouncements();
            BuildInsightsPanel();
        }


        private void BuildPinnedPanel()
        {
            flpPinned.Controls.Clear();
            flpPinned.FlowDirection = FlowDirection.TopDown;
            flpPinned.WrapContents = false;
            flpPinned.AutoScroll = false;
            flpPinned.HorizontalScroll.Enabled = false;
            flpPinned.HorizontalScroll.Visible = false;
            flpPinned.AutoScroll = true;

            var pinned = _announcements.Where(a => a.IsPinned).ToList();
            if (pinned.Count == 0)
            {
                flpPinned.Controls.Add(new Label
                {
                    Text = "No pinned announcements.",
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Padding = new Padding(4),
                });
                return;
            }

            foreach (var a in pinned)
            {
                Color iconCol = CatIconColor.GetValueOrDefault(a.Category, Color.Gray);

                int rowW = Math.Max(100, flpPinned.ClientSize.Width - 4);
                var row = new Panel
                {
                    Width = rowW,
                    Height = 52,
                    BackColor = Color.White,
                    Margin = new Padding(0, 2, 0, 2),
                    Cursor = Cursors.Hand,
                    Tag = a,
                };
                row.Paint += (s, pe) =>
                {
                    using var pen = new Pen(Color.FromArgb(230, 230, 230), 1f);
                    pe.Graphics.DrawRectangle(pen, 0, 0, row.Width - 1, row.Height - 1);
                };

                var dot = new Panel { Size = new Size(8, 8), Location = new Point(8, 22), BackColor = iconCol };
                MakeCircle(dot);
                row.Controls.Add(dot);

                var rowTitle = new Label
                {
                    AutoSize = false,
                    Size = new Size(rowW - 28, 20),
                    Location = new Point(22, 8),
                    Text = a.Title,
                    Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(25, 25, 25),
                    AutoEllipsis = true,
                };
                row.Controls.Add(rowTitle);
                row.Controls.Add(new Label
                {
                    AutoSize = true,
                    Location = new Point(22, 28),
                    Text = a.Date.ToString("MMM d, yyyy"),
                    Font = new Font("Segoe UI", 7.5f),
                    ForeColor = Color.Gray,
                });

                row.Click += (s, ev) => ShowStudentAnnouncementDetail(a);
                rowTitle.Click += (s, ev) => ShowStudentAnnouncementDetail(a);
                flpPinned.Controls.Add(row);
            }
        }

        private void BuildCategoryPanel()
        {
            flpCategories.Controls.Clear();
            flpCategories.FlowDirection = FlowDirection.TopDown;
            flpCategories.WrapContents = false;
            flpCategories.BackColor = Color.White;
            flpCategories.AutoScroll = false;
            flpCategories.HorizontalScroll.Enabled = false;
            flpCategories.HorizontalScroll.Visible = false;
            flpCategories.AutoScroll = true;

            var cats = new[]
            {
                "All Categories", "General", "Academic", "Administrative",
                "Events", "Examinations", "Schedule", "Urgent"
            };

            foreach (var cat in cats)
            {
                int count = cat == "All Categories"
                    ? _announcements.Count
                    : _announcements.Count(a => a.Category == cat);

                bool isActive = _annCategoryFilter == cat;
                Color dotCol = CatIconColor.GetValueOrDefault(cat, Color.FromArgb(139, 0, 0));

                int rowW = Math.Max(100, flpCategories.ClientSize.Width - 4);
                var row = new Panel
                {
                    Width = rowW,
                    Height = 32,
                    BackColor = isActive ? Color.FromArgb(245, 238, 238) : Color.Transparent,
                    Cursor = Cursors.Hand,
                    Margin = new Padding(0, 1, 0, 1),
                };

                var dot = new Panel { Size = new Size(9, 9), Location = new Point(8, 12), BackColor = dotCol };
                MakeCircle(dot);
                row.Controls.Add(dot);

                var lbl = new Label
                {
                    AutoSize = false,
                    Size = new Size(rowW - 52, 32),
                    Location = new Point(24, 0),
                    Text = cat == "All Categories" ? "All" : cat,
                    Font = new Font("Segoe UI", 9f, isActive ? FontStyle.Bold : FontStyle.Regular),
                    ForeColor = Color.FromArgb(40, 40, 40),
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.Transparent,
                };
                row.Controls.Add(lbl);

                var badge = new Label
                {
                    AutoSize = false,
                    Size = new Size(28, 18),
                    Location = new Point(rowW - 34, 7),
                    Text = count.ToString(),
                    Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                    ForeColor = isActive ? Color.White : Color.FromArgb(80, 80, 80),
                    BackColor = isActive ? Color.FromArgb(139, 0, 0) : Color.FromArgb(225, 225, 225),
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                MakeRoundedRegion(badge, 9);
                row.Controls.Add(badge);

                EventHandler handler = (s, ev) =>
                {
                    _annCategoryFilter = cat;
                    cmbCategory.Text = cat;
                    BuildCategoryPanel();
                    RenderAnnouncements();
                };
                row.Click += handler;
                dot.Click += handler;
                lbl.Click += handler;
                badge.Click += handler;

                flpCategories.Controls.Add(row);
            }
        }

        private void BuildInsightsPanel()
        {
            var toRemove = pnlInsights.Controls
                .OfType<Control>()
                .Where(c => c.Name.StartsWith("ins_"))
                .ToList();
            foreach (var c in toRemove) pnlInsights.Controls.Remove(c);

            int total = _announcements.Count;
            int unread = _announcements.Count(a => !a.IsRead);
            int pinned = _announcements.Count(a => a.IsPinned);

            void AddRow(string label, string value, int y, Color col)
            {
                pnlInsights.Controls.Add(new Label
                {
                    Name = "ins_lbl_" + y,
                    AutoSize = true,
                    Text = label,
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = Color.FromArgb(80, 80, 80),
                    Location = new Point(8, y),
                });
                pnlInsights.Controls.Add(new Label
                {
                    Name = "ins_val_" + y,
                    AutoSize = true,
                    Text = value,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    ForeColor = col,
                    Location = new Point(pnlInsights.Width - 50, y - 2),
                });
            }

            AddRow("Total Announcements", total.ToString(), 30, Color.FromArgb(50, 50, 50));
            AddRow("Unread", unread.ToString(), 58, unread > 0 ? Color.FromArgb(200, 0, 0) : Color.Green);
            AddRow("Pinned", pinned.ToString(), 86, Color.FromArgb(150, 100, 0));
            AddRow("Read", (total - unread).ToString(), 114, Color.FromArgb(22, 163, 74));
        }

        // ════════════════════════════════════════════════════════════════════
        //  DRAWING HELPERS
        // ════════════════════════════════════════════════════════════════════
        private static void MakeRoundedRegion(Control c, int radius)
        {
            var path = new GraphicsPath();
            var r = new Rectangle(0, 0, c.Width, c.Height);
            path.AddArc(r.X, r.Y, radius, radius, 180, 90);
            path.AddArc(r.Right - radius, r.Y, radius, radius, 270, 90);
            path.AddArc(r.Right - radius, r.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(r.X, r.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            c.Region = new Region(path);
        }

        private static void MakeCircle(Control c)
        {
            var path = new GraphicsPath();
            path.AddEllipse(0, 0, c.Width, c.Height);
            c.Region = new Region(path);
        }

        private void FitContentPanel(Panel panel)
        {
            panel.Width = ClientSize.Width - pnlSidebar.Width;
            panel.Height = ClientSize.Height - pnlHeader.Height;
            panel.Location = new Point(pnlSidebar.Width, pnlHeader.Height);
        }

        // ─────────────────────────────────────────────────────────────────
        // ENROLLMENT
        // ─────────────────────────────────────────────────────────────────
        private void Enrollment_Initialize()
        {
            btnSaveAndAssess.Text = "Save and Assess";   // change button text
            dgvEnrollment.Visible = true;                // ensure original grid is visible
            dgvEnrollmentConfirmed.Visible = false;      // hide confirmed grid initially
            pnlEnrollmentConfirmedDGV.Visible = false;
            pnlContainerEnrollmentDGV.Visible = true;
            dgvEnrollment.Visible = true;
            Enrollment_LoadData();
            Enrollment_UpdateTotalUnits();
            ApplyStatusStyles();
        }

        private void Enrollment_LoadData()
        {
            enrollmentData.Clear();
            dgvEnrollment.Rows.Clear();

            // Define raw schedule entries (Day, StartTimes, EndTimes, CourseCode, CourseName)
            // Each entry represents one meeting time for a course on a specific day.
            var entries = new List<(string Day, string[] Starts, string[] Ends, string Code, string Name)>
    {
        // Monday
        ("Monday", new[] { "10:30 AM" }, new[] { "1:30 PM" }, "ELEC IT-FE2", "BSIT Free Elective 2"),
        ("Monday", new[] { "2:30 PM" }, new[] { "5:30 PM" }, "COMP 014", "Quantitative Methods with Modeling and Simulation"),

        // Wednesday
        ("Wednesday", new[] { "8:00 AM", "10:30 AM" }, new[] { "10:00 AM", "1:30 PM" }, "COMP 012", "Network Administration"),
        ("Wednesday", new[] { "5:30 PM" }, new[] { "7:30 PM" }, "COMP 009", "Object Oriented Programming"),

        // Thursday
        ("Thursday", new[] { "10:30 AM" }, new[] { "1:30 PM" }, "COMP 009", "Object Oriented Programming"),
        ("Thursday", new[] { "2:30 PM", "5:00 PM" }, new[] { "4:30 PM", "8:00 PM" }, "INTE 202", "Interactive Programming and Technologies 1"),

        // Friday
        ("Friday", new[] { "10:00 AM" }, new[] { "12:00 PM" }, "PATHFIT 4", "Physical Activity Towards Health and Fitness 4"),

        // Saturday
        ("Saturday", new[] { "7:30 AM" }, new[] { "10:30 AM" }, "COMP 013", "Human Computer Interaction"),
        ("Saturday", new[] { "2:30 PM", "5:00 PM" }, new[] { "4:30 PM", "8:00 PM" }, "COMP 010", "Information Management")
    };

            // Group by course code (and name) to combine multiple days
            var grouped = entries.GroupBy(e => new { e.Code, e.Name })
                                 .Select(g => new
                                 {
                                     Code = g.Key.Code,
                                     Name = g.Key.Name,
                                     // Collect schedule lines for each day
                                     ScheduleLines = g.Select(e =>
                                     {
                                         // Build time slots for that day
                                         string times = string.Join(", ", e.Starts.Select((s, i) => $"{s} - {e.Ends[i]}"));
                                         return $"{e.Day} {times}";
                                     }).ToList()
                                 });

            int GetUnits(string courseCode) => courseCode == "PATHFIT 4" ? 2 : 3;

            foreach (var course in grouped)
            {
                // Combine all schedule lines with newline characters
                string scheduleStr = string.Join(Environment.NewLine, course.ScheduleLines);
                int units = GetUnits(course.Code);
                string status = "Pending";

                enrollmentData.Add(new string[] { course.Code, course.Name, units.ToString(), scheduleStr, status });
            }

            // Enable text wrapping in the schedule column to show multiple lines
            dgvEnrollment.Columns["colSchedule"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvEnrollment.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Populate the DataGridView
            foreach (var row in enrollmentData)
            {
                dgvEnrollment.Rows.Add(false, row[0], row[1], row[2], row[3], row[4], "More");
            }

            Enrollment_UpdateTotalUnits();
        }

        private void ApplyStatusStyles()
        {
            foreach (DataGridViewRow row in dgvEnrollment.Rows)
            {
                if (row.IsNewRow) continue;
                string status = row.Cells["colStatus"].Value?.ToString();
                if (status == "Enrolled")
                {
                    row.Cells["colStatus"].Style.BackColor = Color.FromArgb(240, 240, 240);
                    row.Cells["colStatus"].Style.ForeColor = Color.Black;
                }
                else if (status == "Pending")
                {
                    row.Cells["colStatus"].Style.BackColor = Color.Gold;
                    row.Cells["colStatus"].Style.ForeColor = Color.Black;
                }
                else // Dropped or others
                {
                    row.Cells["colStatus"].Style.BackColor = Color.LightGray;
                    row.Cells["colStatus"].Style.ForeColor = Color.Black;
                }
            }
        }

        private void Enrollment_UpdateTotalUnits()
        {
            int total = 0;
            foreach (DataGridViewRow row in dgvEnrollment.Rows)
            {
                if (!row.IsNewRow && int.TryParse(row.Cells["colUnits"].Value?.ToString(), out int u)) total += u;
            }
            lblEnrollTotalUnitsValue.Text = total.ToString();
        }

        private void StudentPortal_EnrollmentResize(object sender, EventArgs e)
        {
            if (!IsHandleCreated) return;
            int contentWidth = pnlEnrollContent.Width;
            int cardWidth = (contentWidth - (SidePadding * 2) - (CardGap * 2)) / 3;
            foreach (var card in new[] { pnlEnrollLeftCard, pnlEnrollMiddleCard, pnlEnrollRightCard })
                card.Width = cardWidth;
            pnlEnrollLeftCard.Left = SidePadding;
            pnlEnrollMiddleCard.Left = SidePadding + cardWidth + CardGap;
            pnlEnrollRightCard.Left = SidePadding + (cardWidth * 2) + (CardGap * 2);
        }

        private void dgvEnrollment_SelectionChanged(object sender, EventArgs e) => dgvEnrollment.ClearSelection();

        private bool allSelected = false;
        private void btnEnrollSelectAll_Click(object sender, EventArgs e)
        {
            allSelected = !allSelected;
            foreach (DataGridViewRow row in dgvEnrollment.Rows)
                row.Cells["colSelect"].Value = allSelected;
            btnEnrollSelectAll.Text = allSelected ? "Deselect All" : "Select All";
        }

        private void txtEnrollSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { btnEnrollSearch.PerformClick(); e.SuppressKeyPress = true; }
        }

        private void btnEnrollSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtEnrollSearch.Text.Trim().ToLower();
            string filterBy = cmbEnrollFilter.SelectedItem?.ToString() ?? "All";
            dgvEnrollment.Rows.Clear();

            if (string.IsNullOrEmpty(searchTerm))
            {
                foreach (var row in enrollmentData) dgvEnrollment.Rows.Add(false, row[0], row[1], row[2], row[3], row[4], "More");
                Enrollment_UpdateTotalUnits();
                ApplyStatusStyles();
                return;
            }

            var filtered = enrollmentData.Where(row =>
                (filterBy == "Course Code" && row[0].ToLower().Contains(searchTerm)) ||
                (filterBy == "Course Title" && row[1].ToLower().Contains(searchTerm)) ||
                (filterBy == "All" && (row[0].ToLower().Contains(searchTerm) || row[1].ToLower().Contains(searchTerm)))
            ).ToList();

            foreach (var row in filtered) dgvEnrollment.Rows.Add(false, row[0], row[1], row[2], row[3], row[4], "More");
            Enrollment_UpdateTotalUnits();
            ApplyStatusStyles();
        }

        private void dgvEnrollment_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Only paint custom header cells
            if (e.RowIndex == -1)
            {
                // Get column indices safely
                int colUnitsIndex = (dgvEnrollment.Columns["colUnits"]?.Index) ?? -1;
                int colStatusIndex = (dgvEnrollment.Columns["colStatus"]?.Index) ?? -1;
                int colActionIndex = (dgvEnrollment.Columns["colAction"]?.Index) ?? -1;

                // Check if current column is one of the ones we want to customise
                if (e.ColumnIndex == colUnitsIndex || e.ColumnIndex == colStatusIndex || e.ColumnIndex == colActionIndex)
                {
                    // Paint background and border only
                    e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

                    // Get header text safely
                    string headerText = dgvEnrollment.Columns[e.ColumnIndex]?.HeaderText ?? "";

                    // Use a safe font and brush
                    using (SolidBrush brush = new SolidBrush(Color.White))
                    using (StringFormat sf = new StringFormat())
                    {
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Center;

                        // Fallback font if e.CellStyle.Font is null
                        Font font = e.CellStyle?.Font ?? new Font("Segoe UI", 9F, FontStyle.Bold);

                        e.Graphics.DrawString(headerText, font, brush, e.CellBounds, sf);
                    }

                    e.Handled = true;
                }
            }
        }

        private void dgvEnrollment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvEnrollment.Columns["colSelect"].Index)
            {
                bool current = Convert.ToBoolean(dgvEnrollment.Rows[e.RowIndex].Cells["colSelect"].Value);
                dgvEnrollment.Rows[e.RowIndex].Cells["colSelect"].Value = !current;
                dgvEnrollment.ClearSelection();
                dgvEnrollment.CurrentCell = null;
            }
            if (e.ColumnIndex == dgvEnrollment.Columns["colAction"].Index && e.RowIndex >= 0)
            {
                Rectangle cellRect = dgvEnrollment.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                Point menuLocation = dgvEnrollment.PointToScreen(new Point(cellRect.Left, cellRect.Bottom));
                cmsEnrollAction.Show(menuLocation);
            }
        }

        private void Enrollment_ShowOverlay()
        {
            pnlViewDetails.Parent = pnlEnrollContent;
            pnlViewDetails.BringToFront();
            pnlViewDetails.Visible = true;
            pnlViewDetails.Location = new Point((pnlViewDetails.Parent.Width - pnlViewDetails.Width) / 2, (pnlViewDetails.Parent.Height - pnlViewDetails.Height) / 2);
        }
        private void Enrollment_HideOverlay() => pnlViewDetails.Visible = false;
        private void Enrollment_viewDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvEnrollment.CurrentRow != null)
            {
                // Retrieve values from the current row
                string code = dgvEnrollment.CurrentRow.Cells["colCode"].Value?.ToString() ?? "";
                string title = dgvEnrollment.CurrentRow.Cells["colTitle"].Value?.ToString() ?? "";
                string units = dgvEnrollment.CurrentRow.Cells["colUnits"].Value?.ToString() ?? "";
                string schedule = dgvEnrollment.CurrentRow.Cells["colSchedule"].Value?.ToString() ?? "";
                string status = dgvEnrollment.CurrentRow.Cells["colStatus"].Value?.ToString() ?? "";

                // Assign to labels (make sure these label names exist in pnlViewDetails)
                lblDetailCode.Text = $"Code: {code}";
                lblDetailTitle.Text = $"Title: {title}";
                lblDetailUnits.Text = $"Units: {units}";
                txtDetailSchedule.Text = $"Schedule: {schedule}";
                lblDetailStatus.Text = $"Status: {status}";
            }
            Enrollment_ShowOverlay();
        }
        private void btnEnrollCloseDetails_Click(object sender, EventArgs e) => Enrollment_HideOverlay();
        private void dropSubjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvEnrollment.CurrentRow == null) return;

            // Confirm drop
            DialogResult result = MessageBox.Show("Are you sure you want to drop this subject?", "Confirm Drop",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            // Get course code of the row to drop
            string courseCode = dgvEnrollment.CurrentRow.Cells["colCode"].Value.ToString();

            // Remove from enrollmentData list
            var toRemove = enrollmentData.FirstOrDefault(r => r[0] == courseCode);
            if (toRemove != null) enrollmentData.Remove(toRemove);

            // Remove from DataGridView
            dgvEnrollment.Rows.RemoveAt(dgvEnrollment.CurrentRow.Index);

            // Update total units
            Enrollment_UpdateTotalUnits();
            ApplyStatusStyles();

            MessageBox.Show("Subject dropped successfully.", "Dropped", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnSaveAndAssess_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Guard: ensure dgvEnrollment exists and has rows
                if (dgvEnrollment == null || dgvEnrollment.IsDisposed)
                {
                    MessageBox.Show("Enrollment grid is not available. Please refresh the page.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (dgvEnrollment.Rows.Count == 0)
                {
                    MessageBox.Show("No courses to enroll.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 2. Confirm save
                DialogResult confirm = MessageBox.Show("Are you sure you want to save and assess your enrollment?\nThis action cannot be undone.",
                    "Confirm Enrollment", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                // 3. Collect selected rows
                List<DataGridViewRow> selectedRows = new List<DataGridViewRow>();
                foreach (DataGridViewRow row in dgvEnrollment.Rows)
                {
                    if (row.IsNewRow) continue;
                    object cellValue = row.Cells["colSelect"]?.Value;
                    if (cellValue != null && Convert.ToBoolean(cellValue))
                        selectedRows.Add(row);
                }

                if (selectedRows.Count == 0)
                {
                    MessageBox.Show("Please select at least one subject to enroll.", "No Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 4. Build DataTable for confirmed enrollment
                DataTable confirmedTable = new DataTable();
                confirmedTable.Columns.Add("Code", typeof(string));
                confirmedTable.Columns.Add("Title", typeof(string));
                confirmedTable.Columns.Add("Units", typeof(string));
                confirmedTable.Columns.Add("Schedule", typeof(string));
                confirmedTable.Columns.Add("Status", typeof(string));

                int totalUnits = 0;
                foreach (DataGridViewRow row in selectedRows)
                {
                    string unitsStr = row.Cells["colUnits"]?.Value?.ToString() ?? "0";
                    confirmedTable.Rows.Add(
                        row.Cells["colCode"]?.Value ?? "",
                        row.Cells["colTitle"]?.Value ?? "",
                        unitsStr,
                        row.Cells["colSchedule"]?.Value ?? "",
                        "Enrolled"
                    );
                    if (int.TryParse(unitsStr, out int u))
                        totalUnits += u;
                }

                // 5. Update total units label
                lblEnrollTotalUnitsValue.Text = totalUnits.ToString();

                // 6. Reposition the confirmed grid panel
                pnlEnrollmentConfirmedDGV.Location = pnlContainerEnrollmentDGV.Location;
                pnlEnrollmentConfirmedDGV.Size = pnlContainerEnrollmentDGV.Size;

                // 7. Configure confirmed grid columns using actual designer column names
                dgvEnrollmentConfirmed.AutoGenerateColumns = false;

                if (dgvEnrollmentConfirmed.Columns.Contains("colCode2"))
                    dgvEnrollmentConfirmed.Columns["colCode2"].DataPropertyName = "Code";
                if (dgvEnrollmentConfirmed.Columns.Contains("colourseTitle2"))
                    dgvEnrollmentConfirmed.Columns["colourseTitle2"].DataPropertyName = "Title";
                if (dgvEnrollmentConfirmed.Columns.Contains("colUnits2"))
                    dgvEnrollmentConfirmed.Columns["colUnits2"].DataPropertyName = "Units";
                if (dgvEnrollmentConfirmed.Columns.Contains("colSchedule2"))
                    dgvEnrollmentConfirmed.Columns["colSchedule2"].DataPropertyName = "Schedule";
                if (dgvEnrollmentConfirmed.Columns.Contains("colStatus2"))
                    dgvEnrollmentConfirmed.Columns["colStatus2"].DataPropertyName = "Status";
                if (dgvEnrollmentConfirmed.Columns.Contains("colAction2"))
                    dgvEnrollmentConfirmed.Columns["colAction2"].Visible = false;

                // 8. Bind data
                dgvEnrollmentConfirmed.DataSource = confirmedTable;
                dgvEnrollmentConfirmed.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                dgvEnrollmentConfirmed.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                if (dgvEnrollmentConfirmed.Columns.Contains("colSchedule2"))
                    dgvEnrollmentConfirmed.Columns["colSchedule2"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                // 9. Remove selection highlight (no blue background on rows or headers)
                dgvEnrollmentConfirmed.EnableHeadersVisualStyles = false;
                dgvEnrollmentConfirmed.DefaultCellStyle.SelectionBackColor = dgvEnrollmentConfirmed.DefaultCellStyle.BackColor;
                dgvEnrollmentConfirmed.DefaultCellStyle.SelectionForeColor = dgvEnrollmentConfirmed.DefaultCellStyle.ForeColor;
                dgvEnrollmentConfirmed.ColumnHeadersDefaultCellStyle.SelectionBackColor = dgvEnrollmentConfirmed.ColumnHeadersDefaultCellStyle.BackColor;
                dgvEnrollmentConfirmed.ColumnHeadersDefaultCellStyle.SelectionForeColor = dgvEnrollmentConfirmed.ColumnHeadersDefaultCellStyle.ForeColor;
                dgvEnrollmentConfirmed.RowHeadersVisible = false;

                // 10. Center align Units and Status columns (both cells and headers)
                if (dgvEnrollmentConfirmed.Columns.Contains("colUnits2"))
                {
                    dgvEnrollmentConfirmed.Columns["colUnits2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvEnrollmentConfirmed.Columns["colUnits2"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dgvEnrollmentConfirmed.Columns.Contains("colStatus2"))
                {
                    dgvEnrollmentConfirmed.Columns["colStatus2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvEnrollmentConfirmed.Columns["colStatus2"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                // 11. Hide original UI, show confirmed grid
                dgvEnrollment.Visible = false;
                pnlContainerEnrollmentDGV.Visible = false;
                btnEnrollSelectAll.Visible = false;
                btnSaveAndAssess.Visible = false;
                pnlEnrollSearchbar.Visible = false;
                pnlEnrollmentConfirmedDGV.Visible = true;
                dgvEnrollmentConfirmed.Visible = true;

                // 12. Update enrollment status card
                lblEnrollStatusTitle.Text = "Officially Enrolled";
                lblEnrollStatusDesc.Text = "You are now officially enrolled. Your subjects have been confirmed.";
                pnlEnrollStatusCard.BackColor = Color.FromArgb(220, 255, 220);
                pictureBox5.BackColor = Color.Green;

                MessageBox.Show("You are officially enrolled!", "Enrollment Successful",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred:\n{ex.Message}\n\nPlease contact support.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // ACCOUNTS – Using DataTable with existing columns
        // ─────────────────────────────────────────────────────────────────

        private DataTable accountsTable;

        private void Accounts_Initialize()
        {
            pnlAccountsContent.Visible = true;
            CreateAccountsTable();
            LoadAccountsData();
            SetupAccountsGridStyle();
            UpdateAccountsSummary();
            PopulateSemesterFilter();
            cmbSelectSem.SelectedIndexChanged += cmbSelectSem_SelectedIndexChanged;
        }

        private void CreateAccountsTable()
        {
            accountsTable = new DataTable();
            // Column names must match the existing DataGridView column Name properties
            accountsTable.Columns.Add("colAccountsRefID", typeof(string));
            accountsTable.Columns.Add("colAccountsDescription", typeof(string));
            accountsTable.Columns.Add("colAccountsAmount", typeof(string));
            accountsTable.Columns.Add("colAccountsDueDate", typeof(string));
            accountsTable.Columns.Add("colAccountsStatus", typeof(string));
            accountsTable.Columns.Add("colAccountsPaidDate", typeof(string));

            // Prevent auto-generation of new columns
            dgvAccounts.AutoGenerateColumns = false;
            dgvAccounts.DataSource = accountsTable;

            // Manually bind each existing column to the corresponding DataTable column
            dgvAccounts.Columns["colAccountsRefID"].DataPropertyName = "colAccountsRefID";
            dgvAccounts.Columns["colAccountsDescription"].DataPropertyName = "colAccountsDescription";
            dgvAccounts.Columns["colAccountsAmount"].DataPropertyName = "colAccountsAmount";
            dgvAccounts.Columns["colAccountsDueDate"].DataPropertyName = "colAccountsDueDate";
            dgvAccounts.Columns["colAccountsStatus"].DataPropertyName = "colAccountsStatus";
            dgvAccounts.Columns["colAccountsPaidDate"].DataPropertyName = "colAccountsPaidDate";
        }

        private void LoadAccountsData()
        {
            accountsTable.Rows.Clear();

            var data = new List<(string year, string sem, string orDate, string orNo, string assessment)>
    {
        ("2425", "First Semester", "09/03/2024", "CASH - Free Education (20240903-000132)", "7,294.00"),
        ("2425", "Second Semester", "02/18/2025", "CASH - Free Education (20250218-000283)", "6,255.00"),
        ("2526", "First Semester", "08/22/2025", "CASH - Free Education (20250822-001891)", "8,616.00"),
        ("2526", "Second Semester", "02/11/2026", "CASH - Free Education (20260211-006026)", "16,566.00")
    };

            foreach (var item in data)
            {
                string refId = $"{item.year}-{item.sem.Replace(" ", "")}";
                string description = $"Tuition & Fees - {item.sem} AY {item.year}\n({item.orNo})";
                string amount = $"₱{item.assessment}";
                string dueDate = "";
                string status = "Paid";
                string paidDate = item.orDate;

                accountsTable.Rows.Add(refId, description, amount, dueDate, status, paidDate);
            }

            dgvAccounts.ClearSelection();
            dgvAccounts.Refresh();
        }

        private void SetupAccountsGridStyle()
        {
            dgvAccounts.EnableHeadersVisualStyles = false;
            dgvAccounts.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvAccounts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvAccounts.GridColor = Color.FromArgb(220, 220, 220);
            dgvAccounts.BackgroundColor = Color.White;
            dgvAccounts.BorderStyle = BorderStyle.None;
            dgvAccounts.RowHeadersVisible = false;
            dgvAccounts.AllowUserToAddRows = false;
            dgvAccounts.AllowUserToResizeRows = false;
            dgvAccounts.AllowUserToResizeColumns = false;
            dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Header styling – light gray, no blue
            dgvAccounts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);
            dgvAccounts.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(60, 60, 60);
            dgvAccounts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvAccounts.ColumnHeadersHeight = 45;
            dgvAccounts.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 247, 250);
            dgvAccounts.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(60, 60, 60);

            // Row styling
            dgvAccounts.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgvAccounts.DefaultCellStyle.ForeColor = Color.FromArgb(60, 60, 60);
            dgvAccounts.DefaultCellStyle.Padding = new Padding(5, 10, 5, 10);
            dgvAccounts.RowTemplate.Height = 50;
            dgvAccounts.DefaultCellStyle.SelectionBackColor = Color.White;
            dgvAccounts.DefaultCellStyle.SelectionForeColor = Color.FromArgb(60, 60, 60);

            // Wrap description column text and auto-size rows
            dgvAccounts.Columns["colAccountsDescription"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvAccounts.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Force Reference ID column header and cell colors
            dgvAccounts.Columns["colAccountsRefID"].HeaderCell.Style.BackColor = Color.FromArgb(245, 247, 250);
            dgvAccounts.Columns["colAccountsRefID"].DefaultCellStyle.ForeColor = Color.Gray;

            dgvAccounts.CellFormatting += dgvAccounts_CellFormatting;
        }

        private void dgvAccounts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Amount column – bold
            if (e.ColumnIndex == dgvAccounts.Columns["colAccountsAmount"].Index && e.Value != null)
            {
                e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                e.CellStyle.ForeColor = Color.FromArgb(30, 30, 30);
            }

            // Status column – color coding
            if (e.ColumnIndex == dgvAccounts.Columns["colAccountsStatus"].Index && e.Value != null)
            {
                string status = e.Value.ToString();
                if (status == "Paid")
                {
                    e.CellStyle.ForeColor = Color.Green;
                    e.CellStyle.BackColor = Color.FromArgb(220, 255, 220);
                }
                else if (status == "Pending")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(180, 120, 0);
                    e.CellStyle.BackColor = Color.FromArgb(255, 243, 200);
                }
            }

            // Reference ID – gray
            if (e.ColumnIndex == dgvAccounts.Columns["colAccountsRefID"].Index && e.Value != null)
            {
                e.CellStyle.ForeColor = Color.Gray;
            }
        }

        private void UpdateAccountsSummary()
        {
            decimal totalAssessment = 0;
            decimal totalPaid = 0;

            // Use the DataTable's default view (respects filtering)
            DataTable dt = (DataTable)dgvAccounts.DataSource;
            if (dt == null) return;

            foreach (DataRow row in dt.Rows)
            {
                string amountStr = row["colAccountsAmount"].ToString();
                string status = row["colAccountsStatus"].ToString();

                amountStr = amountStr.Replace("₱", "").Replace(",", "");
                if (decimal.TryParse(amountStr, out decimal amount))
                {
                    totalAssessment += amount;
                    if (status.Equals("Paid", StringComparison.OrdinalIgnoreCase))
                        totalPaid += amount;
                }
            }

            decimal balance = totalAssessment - totalPaid;

            lblTAPeso.Text = $"₱{totalAssessment:N2}";
            lblTPPeso.Text = $"₱{totalPaid:N2}";
            lblBalancePeso.Text = $"₱{balance:N2}";
        }

        private void PopulateSemesterFilter()
        {
            cmbSelectSem.Items.Clear();
            cmbSelectSem.Items.Add("All");

            foreach (DataRow row in accountsTable.Rows)
            {
                string description = row["colAccountsDescription"].ToString();
                if (description.Contains("First Semester") && !cmbSelectSem.Items.Contains("First Semester"))
                    cmbSelectSem.Items.Add("First Semester");
                else if (description.Contains("Second Semester") && !cmbSelectSem.Items.Contains("Second Semester"))
                    cmbSelectSem.Items.Add("Second Semester");
            }
            cmbSelectSem.SelectedIndex = 0;
        }

        private void cmbSelectSem_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cmbSelectSem.SelectedItem?.ToString();
            if (selected == "All")
            {
                accountsTable.DefaultView.RowFilter = "";
            }
            else
            {
                accountsTable.DefaultView.RowFilter = $"colAccountsDescription LIKE '%{selected}%'";
            }
            UpdateAccountsSummary();
        }

        private void btnAccountsDownloadStatement_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Statement of Account would be generated here (demo).", "Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void StudentPortal_AccountsResize(object sender, EventArgs e)
        {
            if (!IsHandleCreated) return;

            FitContentPanel(pnlAccountsContentHolder);

            int contentWidth = pnlAccountsContent.Width - (SidePadding * 2);
            int cardWidth = (contentWidth - (CardGap * 2)) / 3;

            // --- Top summary cards ---
            pnlTotalAssessment.Width = cardWidth;
            pnlTotalPaid.Width = cardWidth;
            pnlBalance.Width = cardWidth;
            pnlTotalAssessment.Left = SidePadding;
            pnlTotalPaid.Left = SidePadding + cardWidth + CardGap;
            pnlBalance.Left = SidePadding + (cardWidth * 2) + (CardGap * 2);
            pbPaid.Left = (pnlTotalPaid.Width - 95) - 30;
            pbTotalAssessment.Left = (pnlTotalPaid.Width - 95) - 30;
            pbBalance.Left = (pnlTotalPaid.Width - 95) - 30;

            // --- Free Education panel (full width) ---
            pnlAccountsFreeEd.Width = contentWidth;
            // Adjust the description label inside it to wrap and fill the available width
            // The label is inside pnlAccountsFreeEd, positioned to the right of the picture box
            // Picture box is at Left=12, Width=78, so label starts at 96 and should fill the rest minus a margin
            int descLabelWidth = pnlAccountsFreeEd.Width - 96 - 20; // 20px right margin
            if (descLabelWidth > 0)
            {
                lblDescriptionFreeEducProg.Width = descLabelWidth;
                // Also adjust the note label if needed
                lblNoteFreeEducProg.Width = descLabelWidth;
            }

            // --- Semester selection panel ---
            pnlAccountsSelectSem.Width = contentWidth;
            pnlAccountsSelectSem.Top = pnlAccountsFreeEd.Bottom + 20;

            // --- Payment History label and Download button ---
            lblPaymentHistory.Top = pnlAccountsSelectSem.Bottom + 20;
            lblPaymentHistory.Left = SidePadding;

            btnAccountsDownloadStatement.Top = pnlAccountsSelectSem.Bottom + 15; // align with label
            btnAccountsDownloadStatement.Left = contentWidth - btnAccountsDownloadStatement.Width + SidePadding;

            // --- DataGridView ---
            dgvAccounts.Top = lblPaymentHistory.Bottom + 10;
            dgvAccounts.Width = contentWidth;
            dgvAccounts.Height = 300; // fixed height, you can adjust or make dynamic

            // --- Payment Methods section ---
            int paymentCardWidth = (contentWidth - CardGap) / 2;

            lblPaymentMethods.Top = dgvAccounts.Bottom + 20;
            lblPaymentMethods.Left = SidePadding;

            pnlOnlinePayment.Width = paymentCardWidth;
            pnlCashier.Width = paymentCardWidth;
            pnlOnlinePayment.Left = SidePadding;
            pnlCashier.Left = SidePadding + paymentCardWidth + CardGap;
            pnlOnlinePayment.Top = lblPaymentMethods.Bottom + 10;
            pnlCashier.Top = lblPaymentMethods.Bottom + 10;
            btnPaymentSlip.Width = pnlOnlinePayment.Width - 25;
            btnPayOnline.Width = pnlOnlinePayment.Width - 25;

            // --- Enrollment Status section ---
            lblEnrollStatus.Top = pnlOnlinePayment.Bottom + 20;
            lblEnrollStatus.Left = SidePadding;

            pnlEnrollStatusCard.Width = contentWidth;
            pnlEnrollStatusCard.Top = lblEnrollStatus.Bottom + 10;

            // --- Spacer panel (ensures scrolling works) ---
            pnlSpaceProviderAccounts.Top = pnlEnrollStatusCard.Bottom + 20;
            pnlSpaceProviderAccounts.Height = 50;
        }

        // ─────────────────────────────────────────────────────────────────
        // Dashboard quick actions
        // ─────────────────────────────────────────────────────────────────
        private void btnDashboardViewEnrollment_Click(object sender, EventArgs e)
        {
            btnEnrollment.PerformClick();
        }
        private void btnDashboardCourses_Click(object sender, EventArgs e)
        {
            if (pnllmsSubmenu.Visible == false)
                btnLMS.PerformClick();
            btnSubject.PerformClick();
        }
        private void btnDashboardPaymentStatus_Click(object sender, EventArgs e)
        {
            btnAccounts.PerformClick();
        }

        private void btnDownloadCOR_Click(object sender, EventArgs e)
        {
            try
            {
                var img = Properties.Resources.CertificateOfRegistration;
                if (img == null) throw new Exception("Certificate image not found.");
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "PNG Image|*.png";
                    sfd.FileName = "Certificate_of_Registration.png";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        MessageBox.Show("File saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // ─────────────────────────────────────────────────────────────────
        // Maroon border for info cards (draws 1px Maroon border)
        // ─────────────────────────────────────────────────────────────────
        private void SetupMaroonBorders()
        {
            foreach (Panel p in new[] { pnlEnrollLeftCard, pnlEnrollMiddleCard, pnlEnrollRightCard })
            {
                p.BackColor = Color.White;
                p.BorderStyle = BorderStyle.None;
                p.Paint += (s, e) =>
                {
                    ControlPaint.DrawBorder(e.Graphics, p.ClientRectangle,
                        Color.Maroon, 1, ButtonBorderStyle.Solid,
                        Color.Maroon, 1, ButtonBorderStyle.Solid,
                        Color.Maroon, 1, ButtonBorderStyle.Solid,
                        Color.Maroon, 1, ButtonBorderStyle.Solid);
                };
            }
        }

        private void btnPayOnline_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You will be presented options on what online payment medium you'd prefer.", "Pay Online", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnPaymentSlip_Click(object sender, EventArgs e)
        {
            MessageBox.Show("A payment slip would be generated here (demo).", "Pay at the Cashier", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cmbbxCourseSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the currently selected text
            string selectedCourse = cmbbxCourseSelection.SelectedItem.ToString();

            // Reset visibility for all panels first (clean slate)
            pnlAttIntro.Visible = false;
            pnlAttAcc.Visible = false;

            if (selectedCourse == "Introduction to Programming")
            {
                pnlAttIntro.Visible = true;

            }
            else if (selectedCourse == "Principles of Accounting")
            {
                pnlAttAcc.Visible = true;

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvMidtermGradeStudent.Columns[e.ColumnIndex].Name == "GradeBreakdown")
            {
                rpnlGradeBreakdown.Visible = true;
                rpnlGradeBreakdown.BringToFront();
            }
        }

        private void cmbGradingPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlMidtermGradeStudent.Visible = false;
            pnlFinalGradeStudent.Visible = false;

            switch (cmbGradingPeriod.SelectedIndex)
            {
                case 0:
                    pnlMidtermGradeStudent.Visible = true;
                    label9.Visible = true;
                    break;
                case 1:
                    pnlFinalGradeStudent.Visible = true;
                    label9.Visible = true;
                    break;
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void dgvFinalGradeStudent_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvFinalGradeStudent.Columns[e.ColumnIndex].Name == "FGradeBreakdown")
            {
                rpnlGradeBreakdown.Visible = true;
                rpnlGradeBreakdown.BringToFront();
            }
        }
    }
}
