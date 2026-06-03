using PUPAcadPortal.PortalContents.Student.LMS.Attendance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Student.LMS.Attendance
{
    public partial class AttendanceControl : UserControl
    {
        //  Runtime State 
        private List<SubjectMeta> _subjects = new();
        private Dictionary<string, List<AttRecord>> _records = new();
        private string _selectedCode = null;
        private int _total, _present, _absent, _late, _excused;
        private double _pct;

        private const double REQUIRED_PCT = 80.0;
        private const int LATE_PER_ABS = 3;

        //  Constructor 
        public AttendanceControl()
        {
            InitializeComponent();
        }





        private static void DrawCardBorder(Graphics g, Panel card, Color accent)
        {
            using var pen = new Pen(Color.FromArgb(235, 235, 240), 1);
            g.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
        }



        private void DrawSegmentedProgress(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var pan = (Panel)sender;
            int w = pan.Width;
            int h = pan.Height;

            // Background track
            using var bg = new SolidBrush(Color.FromArgb(230, 230, 238));
            g.FillRectangle(bg, 0, 0, w, h);

            if (_total <= 0) return;

            double pF = (double)_present / _total;
            double lF = (double)_late / _total;
            double aF = (double)_absent / _total;
            double eF = (double)_excused / _total;

            int xP = 0;
            int wP = (int)(w * pF);
            int wL = (int)(w * lF);
            int wA = (int)(w * aF);
            int wE = (int)(w * eF);

            using var bP = new SolidBrush(Color.FromArgb(0, 160, 75));
            using var bL = new SolidBrush(Color.FromArgb(220, 150, 0));
            using var bA = new SolidBrush(Color.FromArgb(210, 45, 45));
            using var bE = new SolidBrush(Color.FromArgb(60, 110, 210));

            g.FillRectangle(bP, xP, 0, wP, h); xP += wP;
            g.FillRectangle(bL, xP, 0, wL, h); xP += wL;
            g.FillRectangle(bA, xP, 0, wA, h); xP += wA;
            g.FillRectangle(bE, xP, 0, wE, h);
        }





        private static void AutoSizeGrid(DataGridView dgv, int maxH)
        {
            if (dgv == null) return;
            int rowH = dgv.Rows.GetRowsHeight(DataGridViewElementStates.Visible);
            int desired = dgv.ColumnHeadersHeight + rowH + 2;
            dgv.Height = Math.Min(desired, maxH);
            dgv.ScrollBars = desired > maxH ? ScrollBars.Vertical : ScrollBars.None;
        }

        private void SeedData()
        {
            var defs = new[]
            {
                (Code:"ELEC IT-FE2", Name:"BSIT Free Elective 2",                        Units:3, Sched:"Mon 10:30–1:30 PM",   Days:new[]{DayOfWeek.Monday}),
                (Code:"COMP 014",    Name:"Quantitative Methods with Modeling",           Units:3, Sched:"Mon 2:30–5:30 PM",    Days:new[]{DayOfWeek.Monday}),
                (Code:"COMP 012",    Name:"Network Administration",                       Units:3, Sched:"Wed 8:00–1:30 PM",    Days:new[]{DayOfWeek.Wednesday}),
                (Code:"COMP 009",    Name:"Object Oriented Programming",                  Units:3, Sched:"Wed/Thu varied",      Days:new[]{DayOfWeek.Wednesday,DayOfWeek.Thursday}),
                (Code:"INTE 202",    Name:"Interactive Programming & Technologies 1",     Units:3, Sched:"Thu 2:30–8:00 PM",    Days:new[]{DayOfWeek.Thursday}),
                (Code:"PATHFIT 4",   Name:"Physical Activity Towards Health & Fitness 4", Units:2, Sched:"Fri 10:00–12:00 PM",  Days:new[]{DayOfWeek.Friday}),
                (Code:"COMP 013",    Name:"Human Computer Interaction",                   Units:3, Sched:"Sat 7:30–10:30 AM",   Days:new[]{DayOfWeek.Saturday}),
                (Code:"COMP 010",    Name:"Information Management",                       Units:3, Sched:"Sat 2:30–8:00 PM",    Days:new[]{DayOfWeek.Saturday}),
            };

            var statusPool = new[] { "Present","Present","Present","Present","Present",
                                     "Late","Absent","Excused" };
            var sessionPool = new[]
            {
                "Lecture 1","Lecture 2","Lab Session","Quiz Session",
                "Recitation","Midterm Exam","Finals Review","Group Activity"
            };
            var rng = new Random(42);
            var sem1Strt = new DateTime(2025, 8, 1);
            var sem1End = new DateTime(2025, 12, 31);
            var sem2Strt = new DateTime(2026, 2, 1);
            var sem2End = new DateTime(2026, 5, 31);

            cmbCourse.Items.Clear();
            cmbCourse.Items.Add("All Courses");
            foreach (var d in defs)
            {
                _subjects.Add(new SubjectMeta
                {
                    Code = d.Code,
                    Name = d.Name,
                    Units = d.Units,
                    Schedule = d.Sched
                });
                cmbCourse.Items.Add($"{d.Code} – {d.Name}");

                var recs = new List<AttRecord>();
                int sessionCounter = 1;

                void AddPeriodRecords(DateTime start, DateTime end, string acadYear, bool isQR)
                {
                    for (var dt = start; dt <= end && dt.Date <= DateTime.Today; dt = dt.AddDays(1))
                    {
                        if (!d.Days.Contains(dt.DayOfWeek)) continue;
                        string st = statusPool[rng.Next(statusPool.Length)];
                        string period = dt.Month <= 9 ? "Prelim"
                                      : dt.Month <= 11 ? "Midterm"
                                      : "Final Term";
                        recs.Add(new AttRecord
                        {
                            Date = dt.Date,
                            AcadYear = acadYear,
                            Period = period,
                            Session = sessionPool[(sessionCounter++ - 1) % sessionPool.Length],
                            Status = st,
                            Remarks = BuildRemark(st, rng),
                            IsQR = isQR || rng.Next(3) == 0
                        });
                    }
                }

                AddPeriodRecords(sem1Strt, sem1End, "2025–2026", false);
                AddPeriodRecords(sem2Strt, sem2End, "2025–2026", true);

                _records[d.Code] = recs;
            }
            cmbCourse.SelectedIndex = 0;
        }

        private static string BuildRemark(string status, Random rng) => status switch
        {
            "Present" => "On Time",
            "Late" => $"Late by {rng.Next(5, 30)} min",
            "Excused" => "Medical certificate",
            _ => "No excuse submitted"
        };

        private List<AttRecord> FilteredRecords(string code)
        {
            if (!_records.TryGetValue(code, out var all)) return new();
            var q = all.AsEnumerable();

            // Period filter
            string period = cmbPeriod.SelectedItem?.ToString() ?? "All Periods";
            if (period != "All Periods")
                q = q.Where(r => r.Period == period);

            // Course filter (skip when showing per-course subjects grid)
            string course = cmbCourse.SelectedItem?.ToString() ?? "All Courses";
            if (course != "All Courses")
            {
                string filtCode = course.Split('–')[0].Trim();
                if (filtCode != code) return new();
            }

            // Date range filter
            q = q.Where(r => r.Date >= dtpFrom.Value.Date && r.Date <= dtpTo.Value.Date);

            return q.ToList();
        }


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
            int effAbs = _absent + (_late / LATE_PER_ABS);
            _pct = _total > 0
                ? Math.Round(Math.Max(0, (_total - effAbs) * 100.0 / _total), 1)
                : 0;
        }

        private void RefreshAll()
        {
            _selectedCode = null;
            if (lblAttendanceLogTitle != null)
                lblAttendanceLogTitle.Text = "Attendance Log";
            ComputeTotals();
            RefreshCards();
            RefreshMiniStats();
            RefreshSubjectsGrid();
            RefreshLogsGrid();
            RefreshQRGrid();
        }

        //  REFRESH CARDS
        private void RefreshCards()
        {
            lblOverallPct.Text = $"{_pct}%";
            lblPresentValue.Text = _present.ToString();
            lblLateValue.Text = _late.ToString();
            lblAbsentValue.Text = _absent.ToString();

            Color attColor = _pct >= 90 ? Color.FromArgb(0, 150, 70)
                           : _pct >= 80 ? Color.FromArgb(0, 130, 60)
                           : _pct >= 65 ? Color.FromArgb(200, 120, 0)
                           : Color.FromArgb(200, 40, 40);
            lblOverallPct.ForeColor = attColor;

            int atRisk = CountAtRiskSubjects();
            if (atRisk == 0)
            {
                lblAlertText.Text = "✓  All subjects meet the 80% requirement. Keep it up!";
                lblAlertText.ForeColor = Color.FromArgb(0, 120, 60);
                pnlCardAlerts.BackColor = Color.FromArgb(240, 255, 248);
            }
            else
            {
                lblAlertText.Text = $"⚠  {atRisk} subject{(atRisk > 1 ? "s" : "")} below the 80% threshold.";
                lblAlertText.ForeColor = Color.FromArgb(180, 40, 40);
                pnlCardAlerts.BackColor = Color.FromArgb(255, 243, 243);
            }
        }

        private int CountAtRiskSubjects()
        {
            return _subjects.Count(meta =>
            {
                var r = FilteredRecords(meta.Code);
                if (r.Count == 0) return false;
                int lt = r.Count(x => x.Status == "Late");
                int ab = r.Count(x => x.Status == "Absent");
                int ea = ab + (lt / LATE_PER_ABS);
                double p = Math.Max(0, (r.Count - ea) * 100.0 / r.Count);
                return p < REQUIRED_PCT;
            });
        }

        //  REFRESH MINI STATS
        private void RefreshMiniStats()
        {
            lblMiniPresent.Text = $"● Present: {_present}";
            lblMiniLate.Text = $"● Late: {_late}";
            lblMiniAbsent.Text = $"● Absent: {_absent}";
            lblMiniExcused.Text = $"● Excused: {_excused}";
            lblProgressPct.Text = $"{(int)Math.Round(_pct)}% overall";
            pnlProgress?.Invalidate();
        }

        //  SUBJECTS GRID
        private DataTable _subjectsDT;
        private void RefreshSubjectsGrid()
        {
            if (_subjectsDT == null)
            {
                _subjectsDT = new DataTable();
                _subjectsDT.Columns.Add("Code");
                _subjectsDT.Columns.Add("Course Name");
                _subjectsDT.Columns.Add("Schedule");
                _subjectsDT.Columns.Add("Sessions", typeof(int));
                _subjectsDT.Columns.Add("Present", typeof(int));
                _subjectsDT.Columns.Add("Late", typeof(int));
                _subjectsDT.Columns.Add("Absent", typeof(int));
                _subjectsDT.Columns.Add("Excused", typeof(int));
                _subjectsDT.Columns.Add("Att%");
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

                _subjectsDT.Rows.Add(meta.Code, meta.Name, meta.Schedule, n, pr, lt, ab, ex, $"{p}%", st);
            }
            dgvSubjects.DataSource = _subjectsDT;
        }



        private void DgvSubjects_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;
            string col = dgvSubjects.Columns[e.ColumnIndex].Name;

            if (col == "Status")
            {
                string s = e.Value.ToString();
                e.CellStyle.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                (e.CellStyle.ForeColor, e.CellStyle.BackColor) = s switch
                {
                    "Excellent" => (Color.FromArgb(0, 120, 60), Color.FromArgb(220, 255, 235)),
                    "Good" => (Color.FromArgb(0, 130, 70), Color.FromArgb(230, 255, 240)),
                    "At Risk" => (Color.FromArgb(180, 100, 0), Color.FromArgb(255, 243, 210)),
                    _ => (Color.FromArgb(180, 30, 30), Color.FromArgb(255, 232, 232))
                };
            }
            else if (col == "Att%")
            {
                if (double.TryParse(e.Value.ToString().Replace("%", ""), out double p))
                {
                    e.CellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    e.CellStyle.ForeColor = p >= REQUIRED_PCT
                        ? Color.FromArgb(0, 130, 60)
                        : Color.FromArgb(200, 40, 40);
                }
            }
            else if (col == "Absent" &&
                     int.TryParse(e.Value.ToString(), out int ab) && ab > 0)
                e.CellStyle.ForeColor = Color.FromArgb(200, 40, 40);
            else if (col == "Late" &&
                     int.TryParse(e.Value.ToString(), out int lt) && lt > 0)
                e.CellStyle.ForeColor = Color.FromArgb(180, 110, 0);
        }

        private void DgvSubjects_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            _selectedCode = dgvSubjects.Rows[e.RowIndex].Cells["Code"]?.Value?.ToString();
            if (_selectedCode == null) return;
            lblAttendanceLogTitle.Text = $"Attendance Log  –  {_selectedCode}";
            RefreshLogsGrid();
            RefreshQRGrid();
        }

        //  LOGS GRID
        private DataTable _logsDT;
        private void RefreshLogsGrid()
        {
            if (_logsDT == null)
            {
                _logsDT = new DataTable();
                _logsDT.Columns.Add("Date");
                _logsDT.Columns.Add("Code");
                _logsDT.Columns.Add("Session");
                _logsDT.Columns.Add("Period");
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
                        rec.Date.ToString("MMM d, yyyy (ddd)"),
                        meta.Code,
                        rec.Session,
                        rec.Period,
                        rec.Status,
                        rec.Remarks);

            dgvLogs.DataSource = _logsDT;
        }



        private void DgvLogs_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;
            string col = dgvLogs.Columns[e.ColumnIndex].Name;

            if (col == "Status")
            {
                string s = e.Value.ToString();
                e.CellStyle.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                (e.CellStyle.ForeColor, e.CellStyle.BackColor) = s switch
                {
                    "Present" => (Color.FromArgb(0, 130, 60), Color.FromArgb(220, 255, 235)),
                    "Late" => (Color.FromArgb(180, 110, 0), Color.FromArgb(255, 243, 210)),
                    "Excused" => (Color.FromArgb(50, 100, 200), Color.FromArgb(220, 235, 255)),
                    _ => (Color.FromArgb(180, 30, 30), Color.FromArgb(255, 232, 232))
                };
            }
            else if (col == "Period")
            {
                string p = e.Value.ToString();
                e.CellStyle.Font = new Font("Segoe UI", 8f, FontStyle.Regular);
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                (e.CellStyle.ForeColor, e.CellStyle.BackColor) = p switch
                {
                    "Prelim" => (Color.FromArgb(70, 70, 180), Color.FromArgb(230, 230, 255)),
                    "Midterm" => (Color.FromArgb(150, 80, 0), Color.FromArgb(255, 245, 220)),
                    "Final Term" => (Color.FromArgb(128, 0, 0), Color.FromArgb(255, 235, 235)),
                    _ => (Color.FromArgb(80, 80, 80), Color.White)
                };
            }
        }

        //  QR GRID
        private DataTable _qrDT;
        private void RefreshQRGrid()
        {
            if (_qrDT == null)
            {
                _qrDT = new DataTable();
                _qrDT.Columns.Add("Date");
                _qrDT.Columns.Add("Course");
                _qrDT.Columns.Add("Session");
                _qrDT.Columns.Add("Scan Time");
                _qrDT.Columns.Add("Status");
            }
            _qrDT.Rows.Clear();

            var rng = new Random(99);
            var toShow = _selectedCode != null
                ? _subjects.Where(s => s.Code == _selectedCode)
                : _subjects;

            foreach (var meta in toShow)
                foreach (var rec in FilteredRecords(meta.Code)
                    .Where(r => r.IsQR)
                    .OrderByDescending(r => r.Date)
                    .Take(40))
                {
                    int h = rng.Next(7, 18);
                    int m = rng.Next(0, 60);
                    _qrDT.Rows.Add(
                        rec.Date.ToString("MMM d, yyyy (ddd)"),
                        meta.Code,
                        rec.Session,
                        $"{h:D2}:{m:D2}",
                        rec.Status);
                }

            dgvQR.DataSource = _qrDT;
        }



        //  VIEW DETAILS POPUP
        private void OnViewDetailsClick(object sender, EventArgs e)
        {
            var atRiskList = new List<AtRiskSubjectInfo>();
            foreach (var meta in _subjects)
            {
                var r = FilteredRecords(meta.Code);
                if (r.Count == 0) continue;
                int lt = r.Count(x => x.Status == "Late");
                int ab = r.Count(x => x.Status == "Absent");
                int ea = ab + (lt / LATE_PER_ABS);
                double p = Math.Max(0, (r.Count - ea) * 100.0 / r.Count);
                if (p < REQUIRED_PCT)
                    atRiskList.Add(new AtRiskSubjectInfo
                    {
                        Code = meta.Code,
                        Name = meta.Name,
                        Attendance = Math.Round(p, 1),
                        Absent = ab
                    });
            }
            AtRiskPopup.Show(this.FindForm(), atRiskList);
        }
        private static void ApplyGridHeaderStyle(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(128, 0, 0);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(128, 0, 0);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(40, 40, 60);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 220, 220);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(40, 40, 60);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(251, 251, 254);
        }

        //  INNER MODELS
        private class AttRecord
        {
            public DateTime Date { get; set; }
            public string AcadYear { get; set; }
            public string Period { get; set; }   // Prelim / Midterm / Final Term
            public string Session { get; set; }
            public string Status { get; set; }
            public string Remarks { get; set; }
            public bool IsQR { get; set; }
        }

        private class SubjectMeta
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public int Units { get; set; }
            public string Schedule { get; set; }
        }

        public void RecordQRAttendance(QRScanResult result)
        {
            if (result == null || string.IsNullOrWhiteSpace(result.CourseCode)) return;

            // Find a matching subject (case-insensitive prefix match)
            SubjectMeta match = _subjects.FirstOrDefault(s =>
                s.Code.Equals(result.CourseCode, StringComparison.OrdinalIgnoreCase))
                ?? _subjects.FirstOrDefault(s =>
                    s.Code.Replace(" ", "").Equals(
                        result.CourseCode.Replace(" ", ""),
                        StringComparison.OrdinalIgnoreCase));

            if (match == null) return;   // unknown course – silently ignore

            if (!_records.TryGetValue(match.Code, out var list))
            {
                list = new List<AttRecord>();
                _records[match.Code] = list;
            }

            list.Add(new AttRecord
            {
                Date = result.ScanTime.Date,
                AcadYear = "2025–2026",
                Period = string.IsNullOrWhiteSpace(result.Period) ? "Unknown" : result.Period,
                Session = string.IsNullOrWhiteSpace(result.Session) ? "QR Scan" : result.Session,
                Status = "Present",
                Remarks = "Recorded via QR scan",
                IsQR = true
            });

            // Live-refresh everything
            RefreshAll();
        }

        //  SCAN / UPLOAD QR BUTTON
        private void BtnScanQR_Click(object sender, EventArgs e)
        {
            using var frm = new Form
            {
                Text = "Scan / Upload QR Attendance",
                Width = 960,
                Height = 680,
                MinimumSize = new Size(860, 580),
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(245, 246, 250),
                Font = new Font("Segoe UI", 9f),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
            };

            var scanner = new QRScanControl { Dock = DockStyle.Fill };
            frm.Controls.Add(scanner);

            // Wire the confirmed scan → record attendance and close
            scanner.QRCodeScanned += (s2, result) =>
            {
                RecordQRAttendance(result);
                frm.DialogResult = DialogResult.OK;
                frm.Close();
            };

            frm.ShowDialog(this.FindForm());
        }

        //  LOAD
        private void AttendanceControl_Load(object sender, EventArgs e)
        {
            SeedData();
            RefreshAll();
        }
    }
}