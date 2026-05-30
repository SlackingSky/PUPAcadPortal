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
        // Runtime state 
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

        private void AttendanceControl_Load(object sender, EventArgs e)
        {
            // Populate dynamic dropdowns
            cmbSemester.SelectedIndex = 0;
            cmbMonth.SelectedIndex = 0;

            cmbYear.Items.Clear();
            for (int y = 2025; y <= DateTime.Today.Year + 1; y++)
                cmbYear.Items.Add(y.ToString());

            cmbYear.SelectedItem = DateTime.Today.Year.ToString();
            if (cmbYear.SelectedIndex < 0) cmbYear.SelectedIndex = 0;

            SeedData();
            RefreshAll();
        }

        // Custom Paints mapped from the Designer
        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.FromArgb(220, 220, 220), 1),
                0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
        }

        private void pnlMiniStats_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.FromArgb(230, 230, 230), 1),
                0, pnlMiniStats.Height - 1, pnlMiniStats.Width, pnlMiniStats.Height - 1);
        }

        private void pnlSubjTitle_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.FromArgb(210, 210, 210), 1),
                0, pnlSubjTitle.Height - 1, pnlSubjTitle.Width, pnlSubjTitle.Height - 1);
        }

        private void pnlLogTitle_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.FromArgb(210, 210, 210), 1),
                0, pnlLogTitle.Height - 1, pnlLogTitle.Width, pnlLogTitle.Height - 1);
        }

        // Grid auto-sizers 
        private void DgvSubjects_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgvSubjects == null) return;
            int rowH = dgvSubjects.Rows.GetRowsHeight(DataGridViewElementStates.Visible);
            dgvSubjects.Height = dgvSubjects.ColumnHeadersHeight + rowH + 2;
        }

        private void DgvLogs_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgvLogs == null) return;
            int rowH = dgvLogs.Rows.GetRowsHeight(DataGridViewElementStates.Visible);
            int desired = dgvLogs.ColumnHeadersHeight + rowH + 2;
            dgvLogs.Height = Math.Min(desired, 620);
            dgvLogs.ScrollBars = desired > 620 ? ScrollBars.Vertical : ScrollBars.None;
        }

        private void Dgv_SelectionChanged(object sender, EventArgs e)
        {
            ((DataGridView)sender).ClearSelection();
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

        private void Filter_Changed(object sender, EventArgs e)
        {
            RefreshAll();
        }

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

            // every 3 lates = 1 effective absence
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

            // Status thresholds aligned to 80 % rule
            string status = _pct >= 90 ? "Excellent" : _pct >= 80 ? "Good" : _pct >= 65 ? "At Risk" : "Poor";
            Color sc = _pct >= 80 ? Color.ForestGreen : _pct >= 65 ? Color.DarkOrange : Color.Crimson;
            lblStatusText.Text = status;
            lblStatusText.ForeColor = sc;
            lblOverallPct.ForeColor = sc;

            // at-risk = below 80 %
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

        private void DgvSubjects_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;
            string col = dgvSubjects.Columns[e.ColumnIndex].Name;

            if (col == "colStatus")
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
            else if (col == "colAttendancePct")
            {
                if (double.TryParse(e.Value.ToString().Replace("%", ""), out double p))
                {
                    e.CellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    e.CellStyle.ForeColor = p >= REQUIRED_PCT ? Color.ForestGreen : Color.Crimson;
                }
            }
            else if (col == "colAbsent" && int.TryParse(e.Value.ToString(), out int ab) && ab > 0)
            {
                e.CellStyle.ForeColor = Color.Crimson;
            }
        }

        private void DgvSubjects_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            _selectedCode = dgvSubjects.Rows[e.RowIndex].Cells["colCode"]?.Value?.ToString();
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

        private void DgvLogs_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;
            if (dgvLogs.Columns[e.ColumnIndex].Name != "colLogStatus") return;

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
            /* NOTE: Ensure AtRiskPopup and AtRiskSubjectInfo are defined somewhere else 
               in your project, as they were referenced in the provided code! */

            /*
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
            */
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
    }
}