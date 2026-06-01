using PUPAcadPortal.Data;
using PUPAcadPortal.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Instructor.LMS.Calendar
{
    public partial class CalendarContentInst : UserControl
    {
        // ── Shared state (kept for backward compat with UrDay) ────────────────
        public static int _year, _month;
        public static Dictionary<DateTime, string> notesDict = new();

        // ── View state ────────────────────────────────────────────────────────
        private enum CalendarView { Monthly, Weekly, Daily }
        private CalendarView _currentView = CalendarView.Monthly;
        private DateTime _selectedDate = DateTime.Now.Date;
        private DateTime _navDate = DateTime.Now.Date;
        private EventType? _activeFilter = null;

        // ── Layout panels ─────────────────────────────────────────────────────
        private Panel _pnlTopBar;
        private Panel _pnlViewArea;
        private Panel _pnlSidebar;
        private Panel _pnlBottomDetail;
        private FlowLayoutPanel _pnlDayHeaders;
        private FacultyWeekView _weekView = null!;
        private FacultyDayView _dayView = null!;
        private FacultyNotificationsPanel _notifPanel = null!;
        private FacultySearchPanel _searchPanel = null!;

        // Monthly view
        private FlowLayoutPanel _flpMonth = null!;

        // Toolbar controls
        private Label _lblMonthYear;
        private Button _btnPrev, _btnNext, _btnToday;
        private Button _btnMonthly, _btnWeekly, _btnDaily;
        private Button _btnNotif;
        private Button _btnSearch;
        private Button _btnAddEvent;
        private Button _btnSync;
        private Label _lblNotifBadge;

        // Bottom detail – left column (day events) + right column (upcoming)
        private Label _lblSelDate;
        private FlowLayoutPanel _flpDayEvents;
        private FlowLayoutPanel _flpUpcoming;
        private Label _lblNoEvents;
        private Label _lblNoUpcoming;

        // Active filter buttons
        private readonly List<Button> _filterBtns = new();

        // Wheel filter – kept so we can toggle IsEnabled
        private CalendarWheelFilter _wheelFilter = null!;

        // ── Constants ─────────────────────────────────────────────────────────
        private static readonly Color Maroon = Color.FromArgb(136, 14, 79);
        private static readonly Color MaroonLight = Color.FromArgb(252, 240, 248);
        private static readonly Color MaroonDark = Color.FromArgb(100, 8, 55);
        private static readonly Color GridLine = Color.FromArgb(225, 225, 225);
        private static readonly Font UIFont = new Font("Segoe UI", 9f);
        private static readonly Font BoldFont = new Font("Segoe UI", 9f, FontStyle.Bold);
        private static readonly Font HeaderFont = new Font("Maiandra GD", 22f, FontStyle.Bold);
        private static readonly Font SmallFont = new Font("Segoe UI", 7.5f);

        // ── Constructor ───────────────────────────────────────────────────────
        public CalendarContentInst()
        {
            InitializeComponent();
        }

        // ── Load ─────────────────────────────────────────────────────────────
        private void CalendarContentInst_Load(object sender, EventArgs e)
        {
            FacultyCalendarData.LoadData();

            BuildTopBar();
            BuildViewArea();
            BuildSidebar();
            BuildBottomDetail();
            BuildNotifPanel();
            BuildSearchPanel();

            FacultyCalendarDragDropBridge.Attach(() =>
            {
                RefreshMonthCells();
                RefreshCurrentView();
                RefreshDayDetail(_selectedDate, null);
                RefreshUpcoming();
                RefreshNotifBadge();
            });

            this.Resize += (s, ev) => LayoutAll();

            // ── Scroll wheel → navigate months/weeks/days ────────────────────
            // Disabled automatically when search or notification panel is open.
            _wheelFilter = new CalendarWheelFilter(pnlCalendar, delta =>
            {
                if (delta > 0) NavigatePrev();
                else NavigateNext();
            });
            Application.AddMessageFilter(_wheelFilter);

            UrDay.DaySelected += OnDaySelected;

            this.BeginInvoke((Action)(() =>
            {
                LayoutAll();
                ShowMonthlyView(_navDate);
                OnDaySelected(_selectedDate);
                RefreshNotifBadge();
            }));
        }

        // ══════════════════════════════════════════════════════════════════════
        //  TOP BAR
        // ══════════════════════════════════════════════════════════════════════
        private void BuildTopBar()
        {
            _pnlTopBar = new Panel
            {
                Height = 52,
                Dock = DockStyle.Top,
                BackColor = Color.White,
            };
            _pnlTopBar.Paint += (s, e) =>
            {
                using var p = new Pen(Color.FromArgb(220, 220, 220));
                e.Graphics.DrawLine(p, 0, _pnlTopBar.Height - 1, _pnlTopBar.Width, _pnlTopBar.Height - 1);
            };
            pnlCalendar.Controls.Add(_pnlTopBar);
            _pnlTopBar.BringToFront();

            int x = 10;

            _btnPrev = MakeToolBtn("‹", x, 11, 32, 30); _btnPrev.Font = new Font("Segoe UI", 13f); x += 36;
            _btnToday = MakeToolBtn("Today", x, 11, 58, 30); x += 62;
            _btnNext = MakeToolBtn("›", x, 11, 32, 30); _btnNext.Font = new Font("Segoe UI", 13f); x += 44;

            _btnPrev.Click += (s, e) => NavigatePrev();
            _btnNext.Click += (s, e) => NavigateNext();
            _btnToday.Click += (s, e) =>
            {
                _navDate = DateTime.Now.Date;
                _selectedDate = _navDate;
                RefreshCurrentView();
                OnDaySelected(_selectedDate);
            };

            _lblMonthYear = new Label
            {
                Left = x,
                Top = 8,
                Width = 240,
                Height = 36,
                Font = HeaderFont,
                ForeColor = Maroon,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false,
            };
            _pnlTopBar.Controls.Add(_lblMonthYear);
            x += 248;

            _btnMonthly = MakeViewToggle("Monthly", x, 14, 72); x += 75;
            _btnWeekly = MakeViewToggle("Weekly", x, 14, 64); x += 67;
            _btnDaily = MakeViewToggle("Daily", x, 14, 56); x += 62;

            _btnMonthly.Click += (s, e) => SwitchView(CalendarView.Monthly);
            _btnWeekly.Click += (s, e) => SwitchView(CalendarView.Weekly);
            _btnDaily.Click += (s, e) => SwitchView(CalendarView.Daily);
            UpdateViewToggleVisual();

            _btnAddEvent = new Button
            {
                Text = "+ Add Event",
                Width = 100,
                Height = 30,
                BackColor = Maroon,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = UIFont,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
            };
            _btnAddEvent.FlatAppearance.BorderSize = 0;
            _btnAddEvent.Click += (s, e) => QuickAddEvent(_selectedDate);
            _pnlTopBar.Controls.Add(_btnAddEvent);

            _btnSearch = new Button
            {
                Text = "🔍",
                Width = 32,
                Height = 30,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11f),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(80, 80, 80),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
            };
            _btnSearch.Click += (s, e) => ToggleSearch();
            _pnlTopBar.Controls.Add(_btnSearch);

            _btnSync = new Button
            {
                Text = "⟳ Sync",
                Width = 68,
                Height = 30,
                FlatStyle = FlatStyle.Flat,
                Font = UIFont,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(21, 101, 192),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
            };
            _btnSync.FlatAppearance.BorderColor = Color.FromArgb(21, 101, 192);
            _btnSync.Click += (s, e) => SyncLMS();
            _pnlTopBar.Controls.Add(_btnSync);

            var pnlNotifWrap = new Panel
            {
                Width = 36,
                Height = 30,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.Transparent,
            };
            _btnNotif = new Button
            {
                Text = "🔔",
                Width = 32,
                Height = 30,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11f),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(80, 80, 80),
                Cursor = Cursors.Hand,
                Left = 0,
                Top = 0,
            };
            _btnNotif.FlatAppearance.BorderSize = 0;
            _lblNotifBadge = new Label
            {
                Width = 16,
                Height = 16,
                Left = 18,
                Top = 0,
                BackColor = Color.Red,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 6.5f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false,
            };
            pnlNotifWrap.Controls.Add(_btnNotif);
            pnlNotifWrap.Controls.Add(_lblNotifBadge);
            _btnNotif.Click += (s, e) => ToggleNotifications();
            _pnlTopBar.Controls.Add(pnlNotifWrap);

            _pnlTopBar.Resize += (s, e) => PositionRightButtons();
            PositionRightButtons();

            foreach (Control c in new Control[]
                { _btnPrev, _btnToday, _btnNext, _btnMonthly, _btnWeekly, _btnDaily })
                _pnlTopBar.Controls.Add(c);
        }

        private void PositionRightButtons()
        {
            int right = _pnlTopBar.ClientSize.Width - 8;
            _btnAddEvent.Left = right - _btnAddEvent.Width; _btnAddEvent.Top = 11; right -= _btnAddEvent.Width + 6;
            _btnSearch.Left = right - _btnSearch.Width; _btnSearch.Top = 11; right -= _btnSearch.Width + 4;
            _btnSync.Left = right - _btnSync.Width; _btnSync.Top = 11; right -= _btnSync.Width + 6;

            foreach (Control c in _pnlTopBar.Controls)
            {
                if (c is Panel pnl && pnl.Controls.Contains(_btnNotif))
                {
                    pnl.Left = right - pnl.Width;
                    pnl.Top = 11;
                    break;
                }
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  VIEW AREA
        // ══════════════════════════════════════════════════════════════════════
        private void BuildViewArea()
        {
            _pnlViewArea = new Panel { BackColor = Color.White, Parent = pnlCalendar };

            _pnlDayHeaders = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Height = 28,
                BackColor = Color.FromArgb(245, 245, 245),
                Parent = _pnlViewArea,
            };
            string[] days = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            foreach (var d in days)
                _pnlDayHeaders.Controls.Add(new Label
                {
                    Text = d,
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = false,
                    Font = SmallFont,
                    ForeColor = d == "Sun" ? Color.Crimson : Color.FromArgb(80, 80, 80),
                    BackColor = Color.Transparent,
                    Margin = new Padding(1, 0, 1, 0),
                    Height = 28,
                });

            _flpMonth = new FlowLayoutPanel { BackColor = Color.White, Parent = _pnlViewArea };
            FPLmonth = _flpMonth;

            _weekView = new FacultyWeekView { Parent = _pnlViewArea, Visible = false };
            _weekView.EventClicked += ShowEventDetail;
            _weekView.DayHeaderClicked += d => { _selectedDate = d; OnDaySelected(d); };
            _weekView.SlotDoubleClicked += d => QuickAddEventAtTime(d);

            _dayView = new FacultyDayView { Parent = _pnlViewArea, Visible = false };
            _dayView.EventClicked += ShowEventDetail;
            _dayView.SlotDoubleClicked += d => QuickAddEventAtTime(d);
        }

        // ══════════════════════════════════════════════════════════════════════
        //  SIDEBAR  (empty – legend moved to bottom strip)
        // ══════════════════════════════════════════════════════════════════════
        private void BuildSidebar()
        {
            _pnlSidebar = new Panel
            {
                Width = 0,   // sidebar no longer needed; collapse it
                BackColor = Color.White,
                Parent = pnlCalendar,
                Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Visible = false,
            };
        }

        // ══════════════════════════════════════════════════════════════════════
        //  BOTTOM DETAIL STRIP
        //  Left half  → selected-day events (+ compact legend row)
        //  Right half → upcoming events
        // ══════════════════════════════════════════════════════════════════════
        private void BuildBottomDetail()
        {
            const int H = 220;   // extra 10 px for the legend chip row

            _pnlBottomDetail = new Panel
            {
                Height = H,
                BackColor = Color.White,
                Parent = pnlCalendar,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
            };
            _pnlBottomDetail.Paint += (s, e) =>
            {
                using var p = new Pen(Color.FromArgb(220, 220, 220));
                e.Graphics.DrawLine(p, 0, 0, _pnlBottomDetail.Width, 0);
            };

            // ── LEFT: selected-day events ─────────────────────────────────────

            _lblSelDate = new Label
            {
                Text = "Select a day to manage events",
                Left = 12,
                Top = 6,
                AutoSize = true,
                Font = BoldFont,
                ForeColor = Maroon,
            };
            _pnlBottomDetail.Controls.Add(_lblSelDate);

            var btnAdd = new Button
            {
                Text = "+ Add Event",
                Left = 12,
                Top = 26,
                Width = 100,
                Height = 24,
                BackColor = Maroon,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = UIFont,
                Cursor = Cursors.Hand,
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += (s, e) => QuickAddEvent(_selectedDate);
            _pnlBottomDetail.Controls.Add(btnAdd);

            // Filter buttons
            var filters = new[]
            {
                ("All",       (FacultyEventType?)null),
                ("Class",     (FacultyEventType?)FacultyEventType.Class),
                ("Activity",  (FacultyEventType?)FacultyEventType.Activity),
                ("Quiz",      (FacultyEventType?)FacultyEventType.Quiz),
                ("Deadline",  (FacultyEventType?)FacultyEventType.Deadline),
                ("Exam",      (FacultyEventType?)FacultyEventType.Exam),
            };
            int fx = 120;
            foreach (var (label, ft) in filters)
            {
                var cap = ft;
                Color acc = ft == null
                    ? Color.FromArgb(90, 90, 90)
                    : new FacultyCalendarEvent { Type = ft.Value }.GetColor();
                var btn = MakeFilterButton(label, acc);
                btn.Left = fx; btn.Top = 28;
                btn.Click += (s, e) =>
                {
                    _activeFilter = null;
                    RefreshDayDetail(_selectedDate, cap);
                    HighlightFilterBtn(btn);
                };
                _filterBtns.Add(btn);
                _pnlBottomDetail.Controls.Add(btn);
                fx += btn.Width + 5;
            }

            // ── Compact legend row (bottom of the left column) ────────────────
            BuildBottomLegend();

            _flpDayEvents = new FlowLayoutPanel
            {
                Left = 0,
                Top = 56,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom,
            };
            _flpDayEvents.HorizontalScroll.Enabled = false;
            _flpDayEvents.HorizontalScroll.Visible = false;
            _lblNoEvents = new Label { Text = "No events for this day.", ForeColor = Color.Gray, AutoSize = true, Padding = new Padding(8) };
            _flpDayEvents.Controls.Add(_lblNoEvents);
            _pnlBottomDetail.Controls.Add(_flpDayEvents);

            // ── Divider between left and right halves ─────────────────────────
            var divider = new Panel
            {
                Width = 1,
                BackColor = Color.FromArgb(220, 220, 220),
                Top = 0,
                Height = H,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom,
                Tag = "upcoming_divider",
            };
            _pnlBottomDetail.Controls.Add(divider);

            // ── RIGHT: upcoming events ────────────────────────────────────────
            var lblUp = new Label
            {
                Text = "Upcoming",
                Top = 6,
                AutoSize = true,
                Font = BoldFont,
                ForeColor = Maroon,
                Tag = "upcoming_header",
            };
            _pnlBottomDetail.Controls.Add(lblUp);

            _flpUpcoming = new FlowLayoutPanel
            {
                Top = 28,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
            };
            _flpUpcoming.HorizontalScroll.Enabled = false;
            _flpUpcoming.HorizontalScroll.Visible = false;
            _lblNoUpcoming = new Label { Text = "No upcoming events.", ForeColor = Color.Gray, AutoSize = true, Padding = new Padding(8) };
            _flpUpcoming.Controls.Add(_lblNoUpcoming);
            _pnlBottomDetail.Controls.Add(_flpUpcoming);
        }

        /// <summary>
        /// Builds a compact two-row legend chip strip inside the bottom detail panel,
        /// pinned to the bottom-left corner. Tagged "bottom_legend" for LayoutAll.
        /// </summary>
        private void BuildBottomLegend()
        {
            var types = new[]
            {
                (FacultyEventType.Class,        "Class"),
                (FacultyEventType.Activity,     "Activity"),
                (FacultyEventType.Quiz,         "Quiz"),
                (FacultyEventType.LongQuiz,     "Long Quiz"),
                (FacultyEventType.Deadline,     "Deadline"),
                (FacultyEventType.Exam,         "Exam"),
                (FacultyEventType.Consultation, "Consult"),
                (FacultyEventType.PersonalNote, "Note"),
            };

            var flp = new FlowLayoutPanel
            {
                Left = 4,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Height = 44,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Tag = "bottom_legend",
            };

            foreach (var (t, name) in types)
            {
                var ev = new FacultyCalendarEvent { Type = t };
                var chip = new Panel { Height = 18, BackColor = Color.Transparent, Margin = new Padding(0, 1, 8, 1) };
                var dot = new Panel { Width = 9, Height = 9, Left = 0, Top = 4, BackColor = ev.GetColor() };
                var lbl = new Label { Text = name, Left = 13, Top = 0, AutoSize = true, Font = SmallFont, ForeColor = Color.FromArgb(50, 50, 50), Height = 18 };
                chip.Width = 13 + TextRenderer.MeasureText(name, SmallFont).Width + 6;
                chip.Controls.Add(dot);
                chip.Controls.Add(lbl);
                flp.Controls.Add(chip);
            }
            _pnlBottomDetail.Controls.Add(flp);
        }

        // ══════════════════════════════════════════════════════════════════════
        //  NOTIFICATIONS PANEL
        // ══════════════════════════════════════════════════════════════════════
        private void BuildNotifPanel()
        {
            _notifPanel = new FacultyNotificationsPanel
            {
                Parent = pnlCalendar,
                Visible = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
            };
            // Re-enable wheel scroll when the panel is closed via its ✕ button,
            // but only when in Monthly view (Weekly/Daily need scroll for the timeline).
            _notifPanel.CloseRequested += (s, e) =>
            {
                if (_wheelFilter != null && _currentView == CalendarView.Monthly)
                    _wheelFilter.IsEnabled = true;
            };
        }

        // ══════════════════════════════════════════════════════════════════════
        //  SEARCH PANEL
        // ══════════════════════════════════════════════════════════════════════
        private void BuildSearchPanel()
        {
            _searchPanel = new FacultySearchPanel
            {
                Parent = pnlCalendar,
                Visible = false,
                Height = 290,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            };
            _searchPanel.EventSelected += ev =>
            {
                _navDate = ev.Date;
                _selectedDate = ev.Date;
                SwitchView(CalendarView.Monthly);   // SwitchView sets IsEnabled correctly
                OnDaySelected(ev.Date);
                _searchPanel.Visible = false;
                LayoutAll();
            };
        }

        // ══════════════════════════════════════════════════════════════════════
        //  LAYOUT
        // ══════════════════════════════════════════════════════════════════════
        private void LayoutAll()
        {
            if (pnlCalendar == null) return;
            const int SIDEBAR_W = 0;    // sidebar removed – legend is in the bottom strip
            const int BOTTOM_H = 220;
            const int TOPBAR_H = 52;
            const int SEARCH_H = 290;
            const int LEGEND_H = 44;

            int cw = pnlCalendar.ClientSize.Width;
            int ch = pnlCalendar.ClientSize.Height;

            // Search panel (below topbar, full width)
            if (_searchPanel != null)
            {
                _searchPanel.Top = TOPBAR_H;
                _searchPanel.Left = 0;
                _searchPanel.Width = cw;
                _searchPanel.Height = SEARCH_H;
            }

            // View area
            int viewTop = TOPBAR_H + (_searchPanel?.Visible == true ? SEARCH_H : 0);
            int viewW = cw - SIDEBAR_W - 1;
            int viewH = ch - viewTop - BOTTOM_H;

            if (_pnlViewArea != null)
            {
                _pnlViewArea.Left = 0;
                _pnlViewArea.Top = viewTop;
                _pnlViewArea.Width = viewW;
                _pnlViewArea.Height = viewH;
                LayoutViewContents(viewW, viewH);
            }

            // Sidebar hidden – no sizing needed

            // Bottom detail: left half = day events, right half = upcoming
            if (_pnlBottomDetail != null)
            {
                _pnlBottomDetail.Left = 0;
                _pnlBottomDetail.Top = ch - BOTTOM_H;
                _pnlBottomDetail.Width = cw - SIDEBAR_W - 1;
                _pnlBottomDetail.Height = BOTTOM_H;

                int totalW = _pnlBottomDetail.Width;
                int halfW = totalW / 2;
                int eventsH = BOTTOM_H - 56 - LEGEND_H;  // leaves room for legend at bottom
                int upcomingH = BOTTOM_H - 32;

                // Left: day-events FLP (sits between header row and bottom legend)
                if (_flpDayEvents != null)
                {
                    _flpDayEvents.Width = halfW - 8;
                    _flpDayEvents.Height = eventsH;
                    _flpDayEvents.Top = 56;
                }

                // Bottom legend – pinned to the bottom of the left column
                foreach (Control c in _pnlBottomDetail.Controls)
                {
                    if (c is FlowLayoutPanel leg && leg.Tag?.ToString() == "bottom_legend")
                    {
                        leg.Width = halfW - 8;
                        leg.Top = BOTTOM_H - LEGEND_H;
                        leg.Left = 4;
                        break;
                    }
                }

                // Divider
                foreach (Control c in _pnlBottomDetail.Controls)
                {
                    if (c is Panel div && div.Tag?.ToString() == "upcoming_divider")
                    {
                        div.Left = halfW;
                        div.Height = BOTTOM_H;
                        break;
                    }
                }

                // Right: "Upcoming" label
                foreach (Control c in _pnlBottomDetail.Controls)
                {
                    if (c is Label lbl && lbl.Tag?.ToString() == "upcoming_header")
                    {
                        lbl.Left = halfW + 12;
                        lbl.Top = 6;
                        break;
                    }
                }

                // Right: upcoming FLP
                if (_flpUpcoming != null)
                {
                    _flpUpcoming.Left = halfW + 2;
                    _flpUpcoming.Top = 28;
                    _flpUpcoming.Width = totalW - halfW - 4;
                    _flpUpcoming.Height = upcomingH;
                }
            }

            // Notifications popup (top-right overlay)
            if (_notifPanel != null)
            {
                _notifPanel.Left = cw - _notifPanel.Width - 8;
                _notifPanel.Top = TOPBAR_H + 4;
                _notifPanel.Height = Math.Min(400, ch - TOPBAR_H - 16);
            }
        }

        private void LayoutViewContents(int w, int h)
        {
            const int DAY_HDR_H = 28;

            if (_pnlDayHeaders != null)
            {
                _pnlDayHeaders.Left = 0;
                _pnlDayHeaders.Top = 0;
                _pnlDayHeaders.Width = w;
                int cellW = Math.Max(1, (w - SystemInformation.VerticalScrollBarWidth - 2) / 7);
                foreach (Control c in _pnlDayHeaders.Controls) c.Width = cellW;
            }

            if (_flpMonth != null)
            {
                _flpMonth.Left = 0;
                _flpMonth.Top = DAY_HDR_H;
                _flpMonth.Width = w;
                _flpMonth.Height = h - DAY_HDR_H;
                ResizeCalendarCells();
            }

            if (_weekView != null) { _weekView.Left = 0; _weekView.Top = 0; _weekView.Width = w; _weekView.Height = h; }
            if (_dayView != null) { _dayView.Left = 0; _dayView.Top = 0; _dayView.Width = w; _dayView.Height = h; }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  NAVIGATION
        // ══════════════════════════════════════════════════════════════════════
        private void NavigatePrev()
        {
            _navDate = _currentView switch
            {
                CalendarView.Monthly => _navDate.AddMonths(-1),
                CalendarView.Weekly => _navDate.AddDays(-7),
                CalendarView.Daily => _navDate.AddDays(-1),
                _ => _navDate,
            };
            RefreshCurrentView();
        }

        private void NavigateNext()
        {
            _navDate = _currentView switch
            {
                CalendarView.Monthly => _navDate.AddMonths(1),
                CalendarView.Weekly => _navDate.AddDays(7),
                CalendarView.Daily => _navDate.AddDays(1),
                _ => _navDate,
            };
            RefreshCurrentView();
        }

        private void SwitchView(CalendarView view)
        {
            _currentView = view;
            UpdateViewToggleVisual();

            _pnlDayHeaders.Visible = view == CalendarView.Monthly;
            _flpMonth.Visible = view == CalendarView.Monthly;
            _weekView.Visible = view == CalendarView.Weekly;
            _dayView.Visible = view == CalendarView.Daily;

            // Weekly and Daily views have their own internal scrollable timeline.
            // Disable the month-navigation wheel filter so the user can scroll
            // the hour grid without accidentally jumping weeks/days.
            // Monthly view restores normal wheel-navigation behaviour,
            // UNLESS the search or notification panel is currently open.
            if (_wheelFilter != null)
            {
                bool overlayOpen = (_searchPanel?.Visible == true) ||
                                   (_notifPanel?.Visible == true);
                _wheelFilter.IsEnabled = (view == CalendarView.Monthly) && !overlayOpen;
            }

            RefreshCurrentView();
        }

        private void RefreshCurrentView()
        {
            switch (_currentView)
            {
                case CalendarView.Monthly: ShowMonthlyView(_navDate); break;
                case CalendarView.Weekly: ShowWeeklyView(_navDate); break;
                case CalendarView.Daily: ShowDailyView(_navDate); break;
            }
        }

        private void UpdateNavLabel()
        {
            _lblMonthYear.Text = _currentView switch
            {
                CalendarView.Monthly => new DateTimeFormatInfo().GetMonthName(_navDate.Month).ToUpper() + "  " + _navDate.Year,
                CalendarView.Weekly => $"Week of {GetWeekStart(_navDate):MMM dd}",
                CalendarView.Daily => _navDate.ToString("dddd, MMMM dd, yyyy"),
                _ => "",
            };
        }

        // ══════════════════════════════════════════════════════════════════════
        //  MONTHLY VIEW
        // ══════════════════════════════════════════════════════════════════════
        private void ShowMonthlyView(DateTime dt)
        {
            _year = dt.Year; _month = dt.Month;
            FacultyCalendarData.CurrentYear = dt.Year;
            FacultyCalendarData.CurrentMonth = dt.Month;
            SharedCalendarData.CurrentYear = dt.Year;
            SharedCalendarData.CurrentMonth = dt.Month;

            UpdateNavLabel();
            _flpMonth.Controls.Clear();

            DateTime first = new DateTime(dt.Year, dt.Month, 1);
            int startDOW = (int)first.DayOfWeek;
            int daysInMonth = DateTime.DaysInMonth(dt.Year, dt.Month);

            if (startDOW > 0)
            {
                DateTime prev = first.AddMonths(-1);
                int prevDays = DateTime.DaysInMonth(prev.Year, prev.Month);
                for (int i = startDOW - 1; i >= 0; i--)
                {
                    int d = prevDays - i;
                    _flpMonth.Controls.Add(new UrDay(d.ToString(), prev.Year, prev.Month, false,
                        GetHoliday(prev.Year, prev.Month, d), isStudent: false));
                }
            }

            for (int i = 1; i <= daysInMonth; i++)
                _flpMonth.Controls.Add(new UrDay(i.ToString(), dt.Year, dt.Month, true,
                    GetHoliday(dt.Year, dt.Month, i), isStudent: false));

            int total = _flpMonth.Controls.Count;
            int rem = total % 7;
            if (rem > 0)
            {
                DateTime next = first.AddMonths(1);
                for (int i = 1; i <= 7 - rem; i++)
                    _flpMonth.Controls.Add(new UrDay(i.ToString(), next.Year, next.Month, false,
                        GetHoliday(next.Year, next.Month, i), isStudent: false));
            }

            ResizeCalendarCells();

            foreach (Control c in _flpMonth.Controls)
                if (c is UrDay ud) ud.IsSelected = ud.CellDate == _selectedDate;
        }

        private void ResizeCalendarCells()
        {
            if (_flpMonth == null || _flpMonth.Controls.Count == 0) return;
            int avail = _flpMonth.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 2;
            int cellW = Math.Max(60, avail / 7);
            foreach (Control c in _flpMonth.Controls)
            {
                c.Width = cellW;
                c.Height = 110;
                c.Margin = new Padding(1);
            }
            if (_pnlDayHeaders != null)
                foreach (Control c in _pnlDayHeaders.Controls)
                    c.Width = cellW;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  WEEKLY / DAILY VIEW DELEGATES
        // ══════════════════════════════════════════════════════════════════════
        private void ShowWeeklyView(DateTime dt)
        {
            UpdateNavLabel();
            _weekView.LoadWeek(dt);
        }

        private void ShowDailyView(DateTime dt)
        {
            _navDate = dt;
            UpdateNavLabel();
            _dayView.LoadDay(dt);
        }

        // ══════════════════════════════════════════════════════════════════════
        //  DAY DETAIL (bottom strip)
        // ══════════════════════════════════════════════════════════════════════
        private void OnDaySelected(DateTime date)
        {
            _selectedDate = date;

            foreach (Control c in _flpMonth.Controls)
                if (c is UrDay ud) ud.IsSelected = ud.CellDate == date;

            RefreshDayDetail(date, null);
            RefreshUpcoming();
        }

        private void RefreshDayDetail(DateTime date, FacultyEventType? typeFilter)
        {
            _lblSelDate.Text = date.ToString("dddd, MMMM dd, yyyy");
            _flpDayEvents.Controls.Clear();

            var events = FacultyCalendarData.GetEventsForDate(date)
                .Where(ev => typeFilter == null || ev.Type == typeFilter)
                .ToList();

            if (notesDict.ContainsKey(date.Date) && !string.IsNullOrWhiteSpace(notesDict[date.Date]))
                _flpDayEvents.Controls.Add(MakeEventCard(
                    "🗒 Note", notesDict[date.Date],
                    Color.FromArgb(100, 100, 100), date, null));

            if (events.Count == 0 && _flpDayEvents.Controls.Count == 0)
            {
                _flpDayEvents.Controls.Add(_lblNoEvents);
                return;
            }

            foreach (var ev in events)
            {
                string body = "";
                if (!string.IsNullOrEmpty(ev.StartTime))
                    body += ev.StartTime + (string.IsNullOrEmpty(ev.EndTime) ? "" : " – " + ev.EndTime) + "\n";
                if (!string.IsNullOrEmpty(ev.Room)) body += "Room: " + ev.Room + "\n";
                if (!string.IsNullOrEmpty(ev.Course)) body += ev.Course + "\n";
                body += ev.Description;

                string title = $"{ev.GetTypeIcon()}  [{ev.GetTypeLabel()}]  {ev.Title}";
                if (ev.IsAutoSynced) title += "  🔄";
                _flpDayEvents.Controls.Add(MakeEventCard(title, body.Trim(), ev.GetColor(), date, ev));
            }

            _flpDayEvents.HorizontalScroll.Enabled = false;
            _flpDayEvents.HorizontalScroll.Visible = false;
        }

        private void RefreshUpcoming()
        {
            if (_flpUpcoming == null) return;
            _flpUpcoming.Controls.Clear();
            var upcoming = FacultyCalendarData.GetUpcoming(8);

            if (upcoming.Count == 0)
            {
                _flpUpcoming.Controls.Add(_lblNoUpcoming);
            }
            else
            {
                foreach (var item in upcoming)
                {
                    int daysLeft = (item.Date.Date - DateTime.Now.Date).Days;
                    string when = daysLeft == 0 ? "Today" : daysLeft == 1 ? "Tomorrow" : $"In {daysLeft} days";
                    _flpUpcoming.Controls.Add(MakeUpcomingStrip(item.Ev, item.Date, when));
                }
            }
            _flpUpcoming.HorizontalScroll.Enabled = false;
            _flpUpcoming.HorizontalScroll.Visible = false;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  EVENT CARDS
        // ══════════════════════════════════════════════════════════════════════
        private Panel MakeEventCard(string title, string body, Color accent,
                                    DateTime date, FacultyCalendarEvent? ev)
        {
            int flpW = _flpDayEvents.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;
            int cardW = Math.Max(80, flpW > 20 ? flpW : 420);
            const int HDR_H = 32;
            const int BDY_H = 44;

            var card = new Panel
            {
                Width = cardW,
                Height = HDR_H,
                BackColor = Color.FromArgb(250, 250, 255),
                Margin = new Padding(0, 2, 0, 0),
                Tag = false,
            };

            var bar = new Panel { Width = 5, Height = HDR_H + BDY_H, Left = 0, Top = 0, BackColor = accent };
            card.Controls.Add(bar);

            var lblT = new Label
            {
                Text = title,
                Left = 10,
                Top = 7,
                Width = cardW - 60,
                Font = BoldFont,
                ForeColor = Color.FromArgb(40, 40, 40),
                AutoSize = false,
                Height = 18,
                AutoEllipsis = true,
                Cursor = Cursors.Hand,
            };
            card.Controls.Add(lblT);

            var btnToggle = new Button
            {
                Text = "▾",
                Left = cardW - 48,
                Top = 6,
                Width = 22,
                Height = 20,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.Gray,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
            };
            btnToggle.FlatAppearance.BorderSize = 0;
            card.Controls.Add(btnToggle);

            var lblB = new Label
            {
                Text = string.IsNullOrWhiteSpace(body) ? "(no details)" : body,
                Left = 10,
                Top = HDR_H + 4,
                Width = cardW - (ev != null ? 68 : 24),
                Font = UIFont,
                ForeColor = Color.FromArgb(90, 90, 90),
                AutoSize = false,
                Height = BDY_H - 8,
                AutoEllipsis = true,
                Visible = false,
            };
            card.Controls.Add(lblB);

            if (ev != null)
            {
                var btnEdit = new Button
                {
                    Text = "✏",
                    Left = cardW - 26,
                    Top = 6,
                    Width = 20,
                    Height = 20,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.FromArgb(21, 101, 192),
                    BackColor = Color.Transparent,
                    Font = UIFont,
                    Cursor = Cursors.Hand,
                };
                btnEdit.FlatAppearance.BorderSize = 0;
                var capturedEv = ev;
                btnEdit.Click += (s, e) => EditEvent(capturedEv, date);
                card.Controls.Add(btnEdit);

                var btnDel = new Button
                {
                    Text = "✕",
                    Left = cardW - 48,
                    Top = 6,
                    Width = 20,
                    Height = 20,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.Gray,
                    BackColor = Color.Transparent,
                    Font = UIFont,
                    Cursor = Cursors.Hand,
                };
                btnDel.FlatAppearance.BorderSize = 0;
                btnToggle.Left = cardW - 72;
                btnDel.Click += (s, e) =>
                {
                    if (MessageBox.Show($"Remove '{capturedEv.Title}'?", "Confirm",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        FacultyCalendarData.RemoveEvent(capturedEv.Id);
                        RefreshMonthCells();
                        RefreshDayDetail(date, null);
                        RefreshUpcoming();
                        RefreshNotifBadge();
                    }
                };
                card.Controls.Add(btnDel);
            }

            void Toggle()
            {
                bool exp = (bool)card.Tag;
                exp = !exp;
                card.Tag = exp;
                card.Height = exp ? HDR_H + BDY_H : HDR_H;
                bar.Height = card.Height;
                lblB.Visible = exp;
                btnToggle.Text = exp ? "▴" : "▾";
            }
            btnToggle.Click += (s, e) => Toggle();
            lblT.Click += (s, e) => Toggle();

            return card;
        }

        private Panel MakeUpcomingStrip(FacultyCalendarEvent ev, DateTime date, string when)
        {
            int w = Math.Max(80, _flpUpcoming.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 2);
            var strip = new Panel { Width = w, Height = 38, BackColor = Color.White, Margin = new Padding(0, 2, 0, 0) };
            var dot = new Panel { Width = 8, Height = 8, Top = 15, Left = 8, BackColor = ev.GetColor() };
            var lT = new Label { Text = ev.Title, Left = 22, Top = 3, Width = w - 80, Font = BoldFont, ForeColor = Color.FromArgb(40, 40, 40), AutoSize = false, Height = 18, AutoEllipsis = true };
            var lW = new Label { Text = when, Left = 22, Top = 22, Width = w - 80, Font = SmallFont, ForeColor = Color.Gray, AutoSize = false, Height = 14 };
            var lD = new Label { Text = date.ToString("MMM dd"), Left = w - 56, Top = 10, Width = 52, Font = SmallFont, ForeColor = Color.FromArgb(90, 90, 90), AutoSize = false, TextAlign = ContentAlignment.MiddleRight };
            strip.Controls.AddRange(new Control[] { dot, lT, lW, lD });
            strip.Paint += (s, pe) =>
            {
                using var p = new Pen(Color.FromArgb(235, 235, 235));
                pe.Graphics.DrawLine(p, 0, strip.Height - 1, strip.Width, strip.Height - 1);
            };
            strip.Click += (s, e) => { _selectedDate = date; _navDate = date; SwitchView(CalendarView.Monthly); OnDaySelected(date); };
            return strip;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  EVENT MANAGEMENT
        // ══════════════════════════════════════════════════════════════════════
        private void QuickAddEvent(DateTime date)
        {
            using var dlg = new AddEditFacultyEventForm(date);
            if (dlg.ShowDialog() == DialogResult.OK && dlg.ResultEvent != null)
            {
                FacultyCalendarData.AddEvent(date, dlg.ResultEvent);
                RefreshMonthCells();
                RefreshDayDetail(date, null);
                RefreshUpcoming();
                RefreshNotifBadge();
                _searchPanel.RefreshCourses();
            }
        }

        /// <summary>
        /// Called from week/day view slot double-clicks.
        /// <paramref name="dateTime"/> carries both the date AND the clicked hour,
        /// so the form opens with that slot pre-filled as start time.
        /// </summary>
        private void QuickAddEventAtTime(DateTime dateTime)
        {
            using var dlg = new AddEditFacultyEventForm(dateTime);
            if (dlg.ShowDialog() == DialogResult.OK && dlg.ResultEvent != null)
            {
                FacultyCalendarData.AddEvent(dateTime.Date, dlg.ResultEvent);
                RefreshCurrentView();
                RefreshDayDetail(dateTime.Date, null);
                RefreshUpcoming();
                RefreshNotifBadge();
            }
        }

        private void EditEvent(FacultyCalendarEvent ev, DateTime date)
        {
            using var dlg = new AddEditFacultyEventForm(date, ev);
            if (dlg.ShowDialog() == DialogResult.OK && dlg.ResultEvent != null)
            {
                FacultyCalendarData.UpdateEvent(dlg.ResultEvent);
                RefreshMonthCells();
                RefreshCurrentView();
                RefreshDayDetail(date, null);
                RefreshUpcoming();
                RefreshNotifBadge();
            }
        }

        private void ShowEventDetail(FacultyCalendarEvent ev)
        {
            using var popup = new EventDetailPopup(ev);
            popup.ShowDialog();

            switch (popup.Action)
            {
                case EventDetailPopup.ResultAction.Edit:
                    EditEvent(ev, ev.Date);
                    break;

                case EventDetailPopup.ResultAction.Delete:
                    FacultyCalendarData.RemoveEvent(ev.Id);
                    RefreshMonthCells();
                    RefreshCurrentView();
                    RefreshDayDetail(ev.Date, null);
                    RefreshUpcoming();
                    RefreshNotifBadge();
                    break;
            }
        }

        private void RefreshMonthCells()
        {
            foreach (Control c in _flpMonth.Controls)
                if (c is UrDay ud) ud.RefreshEventPills();
        }

        // ══════════════════════════════════════════════════════════════════════
        //  NOTIFICATIONS
        // ══════════════════════════════════════════════════════════════════════
        private void ToggleNotifications()
        {
            bool willShow = !_notifPanel.Visible;
            _notifPanel.Visible = willShow;

            // Wheel navigation is only active in Monthly view AND when no overlay is open.
            if (_wheelFilter != null)
                _wheelFilter.IsEnabled = !willShow && (_currentView == CalendarView.Monthly);

            if (willShow)
            {
                _notifPanel.Refresh();
                _notifPanel.BringToFront();
            }
        }

        private void RefreshNotifBadge()
        {
            int count = FacultyCalendarData.Notifications.Count;
            _lblNotifBadge.Visible = count > 0;
            _lblNotifBadge.Text = count > 9 ? "9+" : count.ToString();
        }

        // ══════════════════════════════════════════════════════════════════════
        //  SEARCH
        // ══════════════════════════════════════════════════════════════════════
        private void ToggleSearch()
        {
            bool willShow = !_searchPanel.Visible;
            _searchPanel.Visible = willShow;

            // Wheel navigation is only active in Monthly view AND when no overlay is open.
            if (_wheelFilter != null)
                _wheelFilter.IsEnabled = !willShow && (_currentView == CalendarView.Monthly);

            LayoutAll();
        }

        // ══════════════════════════════════════════════════════════════════════
        //  LMS SYNC
        // ══════════════════════════════════════════════════════════════════════
        private void SyncLMS()
        {
            _btnSync.Text = "Syncing…";
            _btnSync.Enabled = false;

            var timer = new System.Windows.Forms.Timer { Interval = 800 };
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                FacultyCalendarData.SyncFromLMS();
                RefreshMonthCells();
                RefreshCurrentView();
                RefreshDayDetail(_selectedDate, null);
                RefreshUpcoming();
                RefreshNotifBadge();
                _btnSync.Text = "⟳ Sync";
                _btnSync.Enabled = true;
                MessageBox.Show("LMS activities and deadlines synced successfully.",
                    "Sync Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            timer.Start();
        }

        // ══════════════════════════════════════════════════════════════════════
        //  HELPERS
        // ══════════════════════════════════════════════════════════════════════
        private string GetHoliday(int year, int month, int day)
        {
            var d = new DateTime(year, month, day);
            return FacultyCalendarData.Holidays.ContainsKey(d) ? FacultyCalendarData.Holidays[d] : "";
        }

        private static DateTime GetWeekStart(DateTime dt) =>
            dt.Date.AddDays(-(int)dt.DayOfWeek);

        private Button MakeToolBtn(string text, int x, int y, int w, int h) => new Button
        {
            Text = text,
            Left = x,
            Top = y,
            Width = w,
            Height = h,
            FlatStyle = FlatStyle.Flat,
            Font = UIFont,
            BackColor = Color.White,
            ForeColor = Color.FromArgb(50, 50, 50),
            Cursor = Cursors.Hand,
        };

        private Button MakeViewToggle(string text, int x, int y, int w) => new Button
        {
            Text = text,
            Left = x,
            Top = y,
            Width = w,
            Height = 24,
            FlatStyle = FlatStyle.Flat,
            Font = SmallFont,
            BackColor = Color.White,
            ForeColor = Color.FromArgb(50, 50, 50),
            Cursor = Cursors.Hand,
        };

        private void UpdateViewToggleVisual()
        {
            if (_btnMonthly == null) return;
            foreach (var btn in new[] { _btnMonthly, _btnWeekly, _btnDaily })
            {
                btn.BackColor = Color.White;
                btn.ForeColor = Color.FromArgb(50, 50, 50);
            }
            var active = _currentView switch
            {
                CalendarView.Monthly => _btnMonthly,
                CalendarView.Weekly => _btnWeekly,
                _ => _btnDaily,
            };
            active.BackColor = Maroon;
            active.ForeColor = Color.White;
        }

        private Button MakeFilterButton(string label, Color accent) => new Button
        {
            Text = label,
            Width = label.Length > 6 ? 78 : 56,
            Height = 22,
            FlatStyle = FlatStyle.Flat,
            Font = SmallFont,
            BackColor = Color.White,
            ForeColor = accent,
            FlatAppearance = { BorderColor = accent },
            Cursor = Cursors.Hand,
        };

        private void HighlightFilterBtn(Button active)
        {
            foreach (var b in _filterBtns)
            {
                b.BackColor = Color.White;
                b.ForeColor = (Color)b.Tag == Color.Empty
                    ? Color.FromArgb(90, 90, 90)
                    : b.FlatAppearance.BorderColor;
            }
            active.BackColor = active.FlatAppearance.BorderColor;
            active.ForeColor = Color.White;
        }
    }
}