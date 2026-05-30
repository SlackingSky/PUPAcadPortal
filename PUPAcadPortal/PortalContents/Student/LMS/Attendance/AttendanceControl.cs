using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class AttendanceControl : UserControl
    {
        //  Runtime state 
        private DataTable _subjectsDT;
        private DataTable _logsDT;
        private List<SubjectMeta> _subjects = new();
        private Dictionary<string, List<AttRecord>> _records = new();
        private string _selectedCode = null;
        private int _total, _present, _absent, _late, _excused;
        private double _pct;
        private const double REQUIRED_PCT = 80.0;
        private const int LATE_PER_ABS = 3;

        // Constructor
        public AttendanceControl()
        {
            InitializeComponent();
        }

        // BUILD UI
        private void BuildUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = SystemColors.Control;

            var wrapper = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = SystemColors.Control
            };
            this.Controls.Add(wrapper);

            //  Header bar 
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = Color.White,
                Padding = new Padding(12, 6, 12, 0)
            };
            pnlHeader.Paint += (s, e) =>
                e.Graphics.DrawLine(new Pen(Color.FromArgb(220, 220, 220), 1),
                    0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);

            lblPageTitle = MakeLabel("My Attendance", 18, FontStyle.Bold, Color.Maroon);
            lblPageTitle.AutoSize = true;
            lblPageTitle.Location = new Point(12, 4);

            lblSemLbl = MakeLabel("Semester:", 9, FontStyle.Bold);
            lblSemLbl.AutoSize = true;
            lblSemLbl.Location = new Point(12, 46);

            cmbSemester = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(88, 43),
                Width = 185,
                Font = new Font("Segoe UI", 9f)
            };
            cmbSemester.Items.AddRange(new object[]
            {
                "All",
                "1st Semester",
                "2nd Semester"
            });
            cmbSemester.SelectedIndex = 0;
            cmbSemester.SelectedIndexChanged += (s, e) => RefreshAll();

            //  Month | Year | Refresh 
            lblMonthLbl = MakeLabel("Month:", 9, FontStyle.Bold);
            lblMonthLbl.AutoSize = true;
            lblMonthLbl.Location = new Point(12, 74);

            cmbMonth = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(68, 71),
                Width = 130,
                Font = new Font("Segoe UI", 9f)
            };
            cmbMonth.Items.Add("All");
            foreach (var mn in new[] { "January","February","March","April","May","June",
                                       "July","August","September","October","November","December" })
                cmbMonth.Items.Add(mn);
            cmbMonth.SelectedIndex = 0;
            cmbMonth.SelectedIndexChanged += (s, e) => RefreshAll();

            var lblYearLbl = MakeLabel("Year:", 9, FontStyle.Bold);
            lblYearLbl.AutoSize = true;
            lblYearLbl.Location = new Point(210, 74);

            cmbYear = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(255, 71),
                Width = 90,
                Font = new Font("Segoe UI", 9f)
            };
            for (int y = 2025; y <= DateTime.Today.Year + 1; y++)
                cmbYear.Items.Add(y.ToString());
            cmbYear.SelectedItem = DateTime.Today.Year.ToString();
            if (cmbYear.SelectedIndex < 0) cmbYear.SelectedIndex = 0;
            cmbYear.SelectedIndexChanged += (s, e) => RefreshAll();

            btnRefresh = MakeButton("↺  Refresh", Color.Maroon);
            btnRefresh.Location = new Point(360, 68);
            btnRefresh.Size = new Size(108, 27);
            btnRefresh.Click += (s, e) => RefreshAll();

            pnlHeader.Controls.AddRange(new Control[]
                { lblPageTitle, lblSemLbl, cmbSemester,
                  lblMonthLbl, cmbMonth, lblYearLbl, cmbYear, btnRefresh });

            //  Summary cards 
            tlpCards = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 145,
                ColumnCount = 5,
                RowCount = 1,
                BackColor = Color.WhiteSmoke
            };
            for (int i = 0; i < 5; i++)
                tlpCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpCards.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // Overall
            pnlCardOverall = MakeCard();
            lblOverallTitle = MakeCentredLabel("Overall Attendance", 9f, FontStyle.Bold);
            lblOverallTitle.Dock = DockStyle.Top; lblOverallTitle.Height = 32;
            lblOverallTitle.ForeColor = Color.FromArgb(80, 80, 80);
            lblOverallPct = MakeCentredLabel("–", 26, FontStyle.Bold);
            lblOverallPct.ForeColor = Color.ForestGreen;
            lblOverallPct.Dock = DockStyle.Fill;
            pnlCardOverall.Controls.Add(lblOverallPct);
            pnlCardOverall.Controls.Add(lblOverallTitle);

            // Total
            pnlCardTotal = MakeCard();
            lblTotalTitle = MakeCentredLabel("Total Sessions", 9f, FontStyle.Bold);
            lblTotalTitle.Dock = DockStyle.Top; lblTotalTitle.Height = 32;
            lblTotalTitle.ForeColor = Color.FromArgb(80, 80, 80);
            lblTotalValue = MakeCentredLabel("–", 26, FontStyle.Bold);
            lblTotalValue.Dock = DockStyle.Fill;
            pnlCardTotal.Controls.Add(lblTotalValue);
            pnlCardTotal.Controls.Add(lblTotalTitle);

            // Status
            pnlCardStatus = MakeCard();
            lblStatusTitle = MakeCentredLabel("Attendance Status", 9f, FontStyle.Bold);
            lblStatusTitle.Dock = DockStyle.Top; lblStatusTitle.Height = 32;
            lblStatusTitle.ForeColor = Color.FromArgb(80, 80, 80);
            lblStatusText = MakeCentredLabel("–", 15f, FontStyle.Bold);
            lblStatusText.ForeColor = Color.ForestGreen;
            lblStatusText.Dock = DockStyle.Fill;
            pnlCardStatus.Controls.Add(lblStatusText);
            pnlCardStatus.Controls.Add(lblStatusTitle);

            // Required
            pnlCardRequired = MakeCard();
            lblRequiredTitle = MakeCentredLabel("Required Attendance", 9f, FontStyle.Bold);
            lblRequiredTitle.Dock = DockStyle.Top; lblRequiredTitle.Height = 32;
            lblRequiredTitle.ForeColor = Color.FromArgb(80, 80, 80);
            lblRequiredValue = MakeCentredLabel("80%", 26, FontStyle.Bold);
            lblRequiredValue.ForeColor = Color.FromArgb(128, 0, 0);
            lblRequiredValue.Dock = DockStyle.Fill;
            pnlCardRequired.Controls.Add(lblRequiredValue);
            pnlCardRequired.Controls.Add(lblRequiredTitle);

            // Alerts card
            pnlCardAlerts = new Panel
            {
                BackColor = Color.FromArgb(255, 240, 240),
                Dock = DockStyle.Fill,
                Margin = new Padding(4),
                Padding = new Padding(10)
            };
            var alertInner = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                BackColor = Color.Transparent
            };
            alertInner.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            alertInner.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));

            lblAlertText = new Label
            {
                Text = "Loading…",
                ForeColor = Color.Crimson,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9.5f),
                Dock = DockStyle.Fill,
                AutoSize = false
            };
            btnViewDetails = MakeButton("View Details", Color.Maroon);
            btnViewDetails.Dock = DockStyle.Fill;
            btnViewDetails.Margin = new Padding(20, 4, 20, 4);
            btnViewDetails.Click += OnViewDetailsClick;

            alertInner.Controls.Add(lblAlertText, 0, 0);
            alertInner.Controls.Add(btnViewDetails, 0, 1);
            pnlCardAlerts.Controls.Add(alertInner);

            tlpCards.Controls.Add(pnlCardOverall, 0, 0);
            tlpCards.Controls.Add(pnlCardTotal, 1, 0);
            tlpCards.Controls.Add(pnlCardStatus, 2, 0);
            tlpCards.Controls.Add(pnlCardRequired, 3, 0);
            tlpCards.Controls.Add(pnlCardAlerts, 4, 0);

            //  Mini-stats strip 
            pnlMiniStats = new Panel
            {
                Dock = DockStyle.Top,
                Height = 46,
                BackColor = Color.White,
                Padding = new Padding(12, 0, 12, 0)
            };
            pnlMiniStats.Paint += (s, e) =>
                e.Graphics.DrawLine(new Pen(Color.FromArgb(230, 230, 230), 1),
                    0, pnlMiniStats.Height - 1, pnlMiniStats.Width, pnlMiniStats.Height - 1);

            int mx = 10;
            lblMiniPresent = MakeMiniStat("● Present: –", Color.FromArgb(0, 130, 50), ref mx);
            lblMiniLate = MakeMiniStat("● Late: –", Color.DarkOrange, ref mx);
            lblMiniAbsent = MakeMiniStat("● Absent: –", Color.Crimson, ref mx);
            lblMiniExcused = MakeMiniStat("● Excused: –", Color.RoyalBlue, ref mx);

            pbAttendance = new ProgressBar
            {
                Location = new Point(mx + 16, 13),
                Size = new Size(220, 18),
                Minimum = 0,
                Maximum = 100,
                Style = ProgressBarStyle.Continuous
            };
            lblProgressPct = new Label
            {
                AutoSize = true,
                Location = new Point(mx + 246, 13),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                Text = "0%"
            };
            pnlMiniStats.Controls.AddRange(new Control[]
                { lblMiniPresent, lblMiniLate, lblMiniAbsent, lblMiniExcused,
                  pbAttendance, lblProgressPct });

            //  Subjects section title 
            pnlSubjTitle = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(12, 0, 0, 0)
            };
            pnlSubjTitle.Paint += (s, e) =>
                e.Graphics.DrawLine(new Pen(Color.FromArgb(210, 210, 210), 1),
                    0, pnlSubjTitle.Height - 1, pnlSubjTitle.Width, pnlSubjTitle.Height - 1);
            lblSubjectsTitle = MakeLabel(
                "Attendance per Subject  (click a row to see its log)",
                10f, FontStyle.Bold, Color.FromArgb(50, 50, 50));
            lblSubjectsTitle.AutoSize = true;
            lblSubjectsTitle.Location = new Point(12, 10);
            pnlSubjTitle.Controls.Add(lblSubjectsTitle);

            //  Subjects DataGridView 
            dgvSubjects = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 10,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(230, 230, 230),
                ColumnHeadersHeight = 44,
                RowTemplate = { Height = 42 },
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                Cursor = Cursors.Hand,
                ScrollBars = ScrollBars.None
            };
            StyleGridHeader(dgvSubjects);
            AddSubjectColumns();
            dgvSubjects.CellFormatting += DgvSubjects_CellFormatting;
            dgvSubjects.CellClick += DgvSubjects_CellClick;
            dgvSubjects.SelectionChanged += (s, e) => dgvSubjects.ClearSelection();
            dgvSubjects.DataBindingComplete += (s, e) => AutoSizeSubjectsGrid();

            // Log section title 
            pnlLogTitle = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(12, 0, 0, 0)
            };
            pnlLogTitle.Paint += (s, e) =>
                e.Graphics.DrawLine(new Pen(Color.FromArgb(210, 210, 210), 1),
                    0, pnlLogTitle.Height - 1, pnlLogTitle.Width, pnlLogTitle.Height - 1);
            lblAttendanceLogTitle = MakeLabel("Attendance Log", 10f, FontStyle.Bold, Color.FromArgb(50, 50, 50));
            lblAttendanceLogTitle.AutoSize = true;
            lblAttendanceLogTitle.Location = new Point(12, 10);
            pnlLogTitle.Controls.Add(lblAttendanceLogTitle);

            // Logs DataGridView 
            dgvLogs = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 10,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(230, 230, 230),
                ColumnHeadersHeight = 44,
                RowTemplate = { Height = 40 },
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                ScrollBars = ScrollBars.None
            };
            StyleGridHeader(dgvLogs);
            AddLogColumns();
            dgvLogs.CellFormatting += DgvLogs_CellFormatting;
            dgvLogs.SelectionChanged += (s, e) => dgvLogs.ClearSelection();
            dgvLogs.DataBindingComplete += (s, e) => AutoSizeLogsGrid();

            // Bottom spacer 
            var spacer = new Panel { Dock = DockStyle.Top, Height = 32, BackColor = SystemColors.Control };

            // Reverse order
            wrapper.Controls.Add(spacer);
            wrapper.Controls.Add(dgvLogs);
            wrapper.Controls.Add(pnlLogTitle);
            wrapper.Controls.Add(dgvSubjects);
            wrapper.Controls.Add(pnlSubjTitle);
            wrapper.Controls.Add(pnlMiniStats);
            wrapper.Controls.Add(tlpCards);
            wrapper.Controls.Add(pnlHeader);
        }

        //  Grid auto-sizers 
        private void AutoSizeSubjectsGrid()
        {
            if (dgvSubjects == null) return;
            int rowH = dgvSubjects.Rows.GetRowsHeight(DataGridViewElementStates.Visible);
            dgvSubjects.Height = dgvSubjects.ColumnHeadersHeight + rowH + 2;
        }

        private void AutoSizeLogsGrid()
        {
            if (dgvLogs == null) return;
            int rowH = dgvLogs.Rows.GetRowsHeight(DataGridViewElementStates.Visible);
            int desired = dgvLogs.ColumnHeadersHeight + rowH + 2;
            dgvLogs.Height = Math.Min(desired, 620);
            dgvLogs.ScrollBars = desired > 620 ? ScrollBars.Vertical : ScrollBars.None;
        }

        // SEED DATA
        private void SeedData()
        {
            var defs = new[]
            {
                (Code:"ELEC IT-FE2", Name:"BSIT Free Elective 2",                        Units:3, Sched:"Mon 10:30 AM–1:30 PM",       Days:new[]{DayOfWeek.Monday}),
                (Code:"COMP 014",    Name:"Quantitative Methods with Modeling",            Units:3, Sched:"Mon 2:30 PM–5:30 PM",        Days:new[]{DayOfWeek.Monday}),
                (Code:"COMP 012",    Name:"Network Administration",                        Units:3, Sched:"Wed 8:00 AM–1:30 PM",        Days:new[]{DayOfWeek.Wednesday}),
                (Code:"COMP 009",    Name:"Object Oriented Programming",                   Units:3, Sched:"Wed 5:30 PM / Thu 10:30 AM", Days:new[]{DayOfWeek.Wednesday,DayOfWeek.Thursday}),
                (Code:"INTE 202",    Name:"Interactive Programming & Technologies 1",      Units:3, Sched:"Thu 2:30 PM–8:00 PM",        Days:new[]{DayOfWeek.Thursday}),
                (Code:"PATHFIT 4",   Name:"Physical Activity Towards Health & Fitness 4",  Units:2, Sched:"Fri 10:00 AM–12:00 PM",      Days:new[]{DayOfWeek.Friday}),
                (Code:"COMP 013",    Name:"Human Computer Interaction",                    Units:3, Sched:"Sat 7:30 AM–10:30 AM",       Days:new[]{DayOfWeek.Saturday}),
                (Code:"COMP 010",    Name:"Information Management",                        Units:3, Sched:"Sat 2:30 PM–8:00 PM",        Days:new[]{DayOfWeek.Saturday}),
            };

            var pool = new[] { "Present", "Present", "Present", "Present", "Late", "Absent", "Excused" };
            var rng = new Random(42);
            var sem1Start = new DateTime(2025, 8, 1);
            var sem1End = new DateTime(2025, 12, 31);
            var sem2Start = new DateTime(2026, 2, 1);
            var sem2End = new DateTime(2026, 5, 31);

            foreach (var d in defs)
            {
                _subjects.Add(new SubjectMeta { Code = d.Code, Name = d.Name, Units = d.Units, Schedule = d.Sched });
                var recs = new List<AttRecord>();

                // Seed 1st semester records (Aug–Dec 2025)
                for (var dt = sem1Start; dt <= sem1End && dt.Date <= DateTime.Today; dt = dt.AddDays(1))
                {
                    if (!d.Days.Contains(dt.DayOfWeek)) continue;
                    string st = pool[rng.Next(pool.Length)];
                    recs.Add(new AttRecord
                    {
                        Date = dt.Date,
                        Semester = "1st Semester 2026",
                        Status = st,
                        Remarks = BuildRemark(st, rng)
                    });
                }
                // Seed 2nd semester records (Feb–May 2026)
                for (var dt = sem2Start; dt <= sem2End && dt.Date <= DateTime.Today; dt = dt.AddDays(1))
                {
                    if (!d.Days.Contains(dt.DayOfWeek)) continue;
                    string st = pool[rng.Next(pool.Length)];
                    recs.Add(new AttRecord
                    {
                        Date = dt.Date,
                        Semester = "2nd Semester 2026",
                        Status = st,
                        Remarks = BuildRemark(st, rng)
                    });
                }
                _records[d.Code] = recs;
            }
        }

        private static string BuildRemark(string status, Random rng) => status switch
        {
            "Present" => "On Time",
            "Late" => $"Late by {rng.Next(5, 30)} min",
            "Excused" => "Medical certificate",
            _ => "No excuse notice"
        };

        // REFRESH ALL
        private void RefreshAll()
        {
            _selectedCode = null;
            lblAttendanceLogTitle.Text = "Attendance Log";
            ComputeTotals();
            RefreshCards();
            RefreshMiniStats();
            RefreshSubjectsGrid();
            RefreshLogsGrid();
        }

        // FILTERING
        private List<AttRecord> FilteredRecords(string code)
        {
            if (!_records.TryGetValue(code, out var all)) return new();
            var q = all.AsEnumerable();

            string sem = cmbSemester.SelectedItem?.ToString() ?? "All";
            if (sem == "1st Semester")
                q = q.Where(r => r.Semester.Contains("1st Semester"));
            else if (sem == "2nd Semester")
                q = q.Where(r => r.Semester.Contains("2nd Semester"));

            string mon = cmbMonth.SelectedItem?.ToString() ?? "All";
            if (mon != "All")
            {
                int monthNum = DateTime.ParseExact(mon, "MMMM", null).Month;
                q = q.Where(r => r.Date.Month == monthNum);
            }

            string yr = cmbYear.SelectedItem?.ToString() ?? "All";
            if (int.TryParse(yr, out int yearNum))
                q = q.Where(r => r.Date.Year == yearNum);

            return q.ToList();
        }

        // COMPUTE TOTALS
        private void ComputeTotals()
        {
            _total = _present = _absent = _late = _excused = 0;
            foreach (var meta in _subjects)
            {
                var recs = FilteredRecords(meta.Code);
                _total += recs.Count;
                _present += recs.Count(r => r.Status == "Present");
                _absent += recs.Count(r => r.Status == "Absent");
                _late += recs.Count(r => r.Status == "Late");
                _excused += recs.Count(r => r.Status == "Excused");
            }
            int lateAbsences = _late / LATE_PER_ABS;
            int effectivePresent = _present + (_late - lateAbsences * LATE_PER_ABS);
            int effectiveAbsent = _absent + (_late / LATE_PER_ABS);
            _pct = _total > 0
                ? Math.Round(Math.Max(0, (_total - effectiveAbsent) * 100.0 / _total), 1)
                : 0;
        }

        // CARDS
        private void RefreshCards()
        {
            lblOverallPct.Text = $"{_pct}%";
            lblTotalValue.Text = _total.ToString();

            string status = _pct >= 90 ? "Excellent" : _pct >= 80 ? "Good" : _pct >= 65 ? "At Risk" : "Poor";
            Color sc = _pct >= 80 ? Color.ForestGreen : _pct >= 65 ? Color.DarkOrange : Color.Crimson;
            lblStatusText.Text = status;
            lblStatusText.ForeColor = sc;
            lblOverallPct.ForeColor = sc;

            int atRisk = _subjects.Count(meta =>
            {
                var r = FilteredRecords(meta.Code);
                if (r.Count == 0) return false;
                int late = r.Count(x => x.Status == "Late");
                int absent = r.Count(x => x.Status == "Absent");
                int effAbs = absent + (late / LATE_PER_ABS);
                double pct = Math.Max(0, (r.Count - effAbs) * 100.0 / r.Count);
                return pct < REQUIRED_PCT;
            });

            lblAlertText.Text = atRisk == 0
                ? "All subjects above 80%. Keep it up!"
                : $"You have {atRisk} subject{(atRisk > 1 ? "s" : "")} below the 80% threshold.";
            lblAlertText.ForeColor = atRisk == 0 ? Color.FromArgb(0, 120, 60) : Color.Crimson;
            pnlCardAlerts.BackColor = atRisk == 0 ? Color.FromArgb(240, 255, 245) : Color.FromArgb(255, 240, 240);
        }

        // MINI STATS
        private void RefreshMiniStats()
        {
            lblMiniPresent.Text = $"● Present: {_present}";
            lblMiniLate.Text = $"● Late: {_late}";
            lblMiniAbsent.Text = $"● Absent: {_absent}";
            lblMiniExcused.Text = $"● Excused: {_excused}";
            int p = (int)Math.Round(_pct);
            pbAttendance.Value = Math.Clamp(p, 0, 100);
            lblProgressPct.Text = $"{p}% overall";
        }

        // SUBJECTS GRID
        private void RefreshSubjectsGrid()
        {
            if (_subjectsDT == null)
            {
                _subjectsDT = new DataTable();
                _subjectsDT.Columns.Add("Code");
                _subjectsDT.Columns.Add("Subject");
                _subjectsDT.Columns.Add("Schedule");
                _subjectsDT.Columns.Add("Sessions", typeof(int));
                _subjectsDT.Columns.Add("Present", typeof(int));
                _subjectsDT.Columns.Add("Absent", typeof(int));
                _subjectsDT.Columns.Add("Late", typeof(int));
                _subjectsDT.Columns.Add("Excused", typeof(int));
                _subjectsDT.Columns.Add("Attendance%");
                _subjectsDT.Columns.Add("Status");
            }
            _subjectsDT.Rows.Clear();

            foreach (var meta in _subjects)
            {
                var r = FilteredRecords(meta.Code);
                int n = r.Count;
                int pr = r.Count(x => x.Status == "Present");
                int ab = r.Count(x => x.Status == "Absent");
                int lt = r.Count(x => x.Status == "Late");
                int ex = r.Count(x => x.Status == "Excused");

                int effAbs = ab + (lt / LATE_PER_ABS);
                double p = n > 0 ? Math.Round(Math.Max(0, (n - effAbs) * 100.0 / n), 1) : 0;

                string st = p >= 90 ? "Excellent" : p >= 80 ? "Good" : p >= 65 ? "At Risk" : "Dropped";
                _subjectsDT.Rows.Add(meta.Code, meta.Name, meta.Schedule, n, pr, ab, lt, ex, $"{p}%", st);
            }
            dgvSubjects.DataSource = _subjectsDT;
        }

        private void AddSubjectColumns()
        {
            dgvSubjects.Columns.Add(MakeFixedCol("Code", "Code", 90, false));
            dgvSubjects.Columns.Add(MakeFillCol("Subject", "Subject", false));
            dgvSubjects.Columns.Add(MakeFillCol("Schedule", "Schedule", false));
            dgvSubjects.Columns.Add(MakeFixedCol("Sessions", "Sessions", 75, true));
            dgvSubjects.Columns.Add(MakeFixedCol("Present", "Present", 70, true));
            dgvSubjects.Columns.Add(MakeFixedCol("Absent", "Absent", 70, true));
            dgvSubjects.Columns.Add(MakeFixedCol("Late", "Late", 65, true));
            dgvSubjects.Columns.Add(MakeFixedCol("Excused", "Excused", 72, true));
            dgvSubjects.Columns.Add(MakeFixedCol("Attendance%", "Attendance%", 100, true));
            dgvSubjects.Columns.Add(MakeFixedCol("Status", "Status", 95, true));
        }

        private void DgvSubjects_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;
            string col = dgvSubjects.Columns[e.ColumnIndex].Name;

            if (col == "Status")
            {
                string s = e.Value.ToString();
                e.CellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                (e.CellStyle.ForeColor, e.CellStyle.BackColor) = s switch
                {
                    "Excellent" => (Color.FromArgb(0, 120, 60), Color.FromArgb(225, 255, 235)),
                    "Good" => (Color.ForestGreen, Color.FromArgb(235, 255, 240)),
                    "At Risk" => (Color.DarkOrange, Color.FromArgb(255, 243, 205)),
                    _ => (Color.Crimson, Color.FromArgb(255, 235, 235))
                };
            }
            else if (col == "Attendance%")
            {
                if (double.TryParse(e.Value.ToString().Replace("%", ""), out double p))
                {
                    e.CellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    e.CellStyle.ForeColor = p >= REQUIRED_PCT ? Color.ForestGreen : Color.Crimson;
                }
            }
            else if (col == "Absent" && int.TryParse(e.Value.ToString(), out int ab) && ab > 0)
                e.CellStyle.ForeColor = Color.Crimson;
        }

        private void DgvSubjects_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            _selectedCode = dgvSubjects.Rows[e.RowIndex].Cells["Code"]?.Value?.ToString();
            if (_selectedCode == null) return;
            lblAttendanceLogTitle.Text = $"Attendance Log – {_selectedCode}";
            RefreshLogsGrid();
        }

        // LOGS GRID
        private void RefreshLogsGrid()
        {
            if (_logsDT == null)
            {
                _logsDT = new DataTable();
                _logsDT.Columns.Add("Date");
                _logsDT.Columns.Add("Code");
                _logsDT.Columns.Add("Subject");
                _logsDT.Columns.Add("Status");
                _logsDT.Columns.Add("Remarks");
            }
            _logsDT.Rows.Clear();

            var toShow = _selectedCode != null
                ? _subjects.Where(s => s.Code == _selectedCode)
                : _subjects;

            foreach (var meta in toShow)
                foreach (var rec in FilteredRecords(meta.Code).OrderByDescending(r => r.Date))
                    _logsDT.Rows.Add(
                        rec.Date.ToString("MMMM d, yyyy (ddd)"),
                        meta.Code, meta.Name, rec.Status, rec.Remarks);

            dgvLogs.DataSource = _logsDT;
        }

        private void AddLogColumns()
        {
            dgvLogs.Columns.Add(MakeFixedCol("Date", "Date", 175, false));
            dgvLogs.Columns.Add(MakeFixedCol("Code", "Code", 110, false));
            dgvLogs.Columns.Add(MakeFillCol("Subject", "Subject", false));
            dgvLogs.Columns.Add(MakeFixedCol("Status", "Status", 95, true));
            dgvLogs.Columns.Add(MakeFillCol("Remarks", "Remarks", false));
        }

        private void DgvLogs_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;
            if (dgvLogs.Columns[e.ColumnIndex].Name != "Status") return;

            string s = e.Value.ToString();
            e.CellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            (e.CellStyle.ForeColor, e.CellStyle.BackColor) = s switch
            {
                "Present" => (Color.FromArgb(0, 130, 50), Color.FromArgb(225, 255, 235)),
                "Late" => (Color.DarkOrange, Color.FromArgb(255, 243, 205)),
                "Excused" => (Color.RoyalBlue, Color.FromArgb(220, 235, 255)),
                _ => (Color.Crimson, Color.FromArgb(255, 235, 235))
            };
        }

        // VIEW DETAILS 
        private void OnViewDetailsClick(object sender, EventArgs e)
        {
            // Requires AtRiskPopup.cs and AtRiskSubjectInfo in your project.
            var atRiskList = new List<AtRiskSubjectInfo>();

            foreach (var meta in _subjects)
            {
                var r = FilteredRecords(meta.Code);
                if (r.Count == 0) continue;

                int late = r.Count(x => x.Status == "Late");
                int absent = r.Count(x => x.Status == "Absent");
                int effAbs = absent + (late / LATE_PER_ABS);
                double p = Math.Max(0, (r.Count - effAbs) * 100.0 / r.Count);

                if (p < REQUIRED_PCT)
                {
                    atRiskList.Add(new AtRiskSubjectInfo
                    {
                        Code = meta.Code,
                        Name = meta.Name,
                        Attendance = Math.Round(p, 1),
                        Absent = absent
                    });
                }
            }

            Form owner = this.FindForm();
            AtRiskPopup.Show(owner, atRiskList);
        }

        // HELPERS
        private static Panel MakeCard() =>
            new Panel { BackColor = Color.White, Dock = DockStyle.Fill, Margin = new Padding(4) };

        private static Label MakeLabel(string text, float size, FontStyle style, Color? fore = null) =>
            new Label
            {
                Text = text,
                Font = new Font("Segoe UI", size, style),
                ForeColor = fore ?? Color.FromArgb(40, 40, 40),
                AutoSize = true
            };

        private static Label MakeCentredLabel(string text, float size, FontStyle style) =>
            new Label
            {
                Text = text,
                Font = new Font("Segoe UI", size, style),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false
            };

        private static Button MakeButton(string text, Color back)
        {
            var b = new Button
            {
                Text = text,
                BackColor = back,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        private static Label MakeMiniStat(string text, Color color, ref int x)
        {
            var lbl = new Label
            {
                Text = text,
                ForeColor = color,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(x, 13)
            };
            x += 128;
            return lbl;
        }

        private static DataGridViewTextBoxColumn MakeFixedCol(string name, string header, int width, bool centre) =>
            new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                DataPropertyName = name,
                Width = width,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                ReadOnly = true,
                DefaultCellStyle =
                {
                    Alignment = centre
                        ? DataGridViewContentAlignment.MiddleCenter
                        : DataGridViewContentAlignment.MiddleLeft,
                    WrapMode = DataGridViewTriState.True
                }
            };

        private static DataGridViewTextBoxColumn MakeFillCol(string name, string header, bool centre) =>
            new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                DataPropertyName = name,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true,
                DefaultCellStyle =
                {
                    Alignment = centre
                        ? DataGridViewContentAlignment.MiddleCenter
                        : DataGridViewContentAlignment.MiddleLeft,
                    WrapMode = DataGridViewTriState.True
                }
            };

        private static void StyleGridHeader(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(128, 0, 0);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(128, 0, 0);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10f);
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(40, 40, 40);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 220, 220);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(40, 40, 40);
        }

        // INNER MODELS
        private class AttRecord
        {
            public DateTime Date { get; set; }
            public string Semester { get; set; }
            public string Status { get; set; }
            public string Remarks { get; set; }
        }

        private class SubjectMeta
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public int Units { get; set; }
            public string Schedule { get; set; }
        }

        // Now correctly triggered by the designer
        private void AttendanceControl_Load(object sender, EventArgs e)
        {
            BuildUI();
            SeedData();
            RefreshAll();
        }
    }
}