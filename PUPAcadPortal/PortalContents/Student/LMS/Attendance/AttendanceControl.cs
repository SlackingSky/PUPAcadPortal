using PUPAcadPortal.PortalContents.Student.LMS.Attendance;
using PUPAcadPortal.Models;
using PUPAcadPortal.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

namespace PUPAcadPortal.PortalContents.Student.LMS.Attendance
{

    public partial class AttendanceControl : UserControl
    {
        // ── Identity (set before the control is shown) ────────────────────────────
        // e.g. attendanceCtl.CurrentStudentId = Session.CurrentUser.StudentId;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentStudentId { get; set; } = 0;

        // ── Runtime state ─────────────────────────────────────────────────────────
        private List<SubjectMeta> _subjects = new();
        private Dictionary<string, List<AttRecord>> _records = new();

        private string? _selectedCode;
        private int _total, _present, _absent, _late, _excused;
        private double _pct;

        private const double REQUIRED_PCT = 80.0;
        private const int LATE_PER_ABS = 3;

        // ── DB factory ────────────────────────────────────────────────────────────
        private static AppDbContext CreateContext() => new AppDbContext();

        // ── Constructor ───────────────────────────────────────────────────────────
        public AttendanceControl()
        {
            InitializeComponent();
        }

        // ── Load ─────────────────────────────────────────────────────────────────
        private void AttendanceControl_Load(object sender, EventArgs e)
        {
            LoadFromDatabase();
            RefreshAll();
        }

        // ── Load all attendance data from the DB for this student ─────────────────
        private void LoadFromDatabase()
        {
            _subjects.Clear();
            _records.Clear();
            cmbCourse.Items.Clear();
            cmbCourse.Items.Add("All Courses");

            if (CurrentStudentId <= 0) return;

            try
            {
                using var ctx = CreateContext();

                // 1. All EnrollmentSubjects for this student → unique SubjectOfferings
                var enrolledOfferings = ctx.EnrollmentSubjects
                    .Include(es => es.Enrollment)
                    .Include(es => es.SubjectOffering)
                        .ThenInclude(so => so.Subject)
                    .Include(es => es.SubjectOffering)
                        .ThenInclude(so => so.AcademicPeriod)
                    .Where(es => es.Enrollment.StudentId == CurrentStudentId)
                    .Select(es => es.SubjectOffering)
                    .Distinct()
                    .ToList();

                foreach (var offering in enrolledOfferings)
                {
                    var meta = new SubjectMeta
                    {
                        Code = offering.SubjectOfferingId,
                        Name = offering.Subject?.SubjectName ?? offering.SubjectOfferingId,
                        SubjectCode = offering.Subject?.SubjectCode ?? offering.SubjectOfferingId,
                        Units = offering.Subject?.LecUnits ?? 0,
                        Schedule = string.Empty,  // populated below
                    };
                    _subjects.Add(meta);
                    cmbCourse.Items.Add($"{meta.SubjectCode} – {meta.Name}");

                    // 2. Load all AttendanceRecords for this student in this offering
                    var rawRecords = ctx.AttendanceRecords
                        .Include(ar => ar.Session)
                        .Where(ar =>
                            ar.StudentId == CurrentStudentId &&
                            ar.Session.SubjectOfferingId == offering.SubjectOfferingId)
                        .OrderByDescending(ar => ar.Session.SessionDate)
                        .ToList();

                    var list = rawRecords.Select(ar => new AttRecord
                    {
                        Date = ar.Session.SessionDate,
                        AcadYear = offering.AcademicPeriod?.SchoolYear ?? "—",
                        Period = DerivePeriod(ar.Session.SessionDate,
                                            offering.AcademicPeriod),
                        Session = ar.Session.Topic ?? "Session",
                        Status = ar.Status,
                        Remarks = ar.Remarks ?? string.Empty,
                        IsQR = ar.IsQrVerified,
                        IsQrVerified = ar.IsQrVerified,
                        QrScannedAt = ar.QrScannedAt,
                        AttendanceId = ar.AttendanceId,
                    }).ToList();

                    _records[meta.Code] = list;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Could not load attendance from the database.\n\n{ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (cmbCourse.Items.Count > 0) cmbCourse.SelectedIndex = 0;
        }

        // ── Derive Prelim / Midterm / Final Term from session date + period ────────
        private static string DerivePeriod(DateTime date, AcademicPeriod? period)
        {
            if (period == null) return "—";
            double total = (period.EndDate - period.StartDate).TotalDays;
            if (total <= 0) return "—";
            double elapsed = (date - period.StartDate).TotalDays;
            double frac = elapsed / total;
            return frac < 0.35 ? "Prelim"
                 : frac < 0.70 ? "Midterm"
                 : "Final Term";
        }

        // ── Filter helper (unchanged logic) ──────────────────────────────────────
        private List<AttRecord> FilteredRecords(string code)
        {
            if (!_records.TryGetValue(code, out var all)) return new();
            var q = all.AsEnumerable();

            string period = cmbPeriod.SelectedItem?.ToString() ?? "All Periods";
            if (period != "All Periods") q = q.Where(r => r.Period == period);

            string course = cmbCourse.SelectedItem?.ToString() ?? "All Courses";
            if (course != "All Courses")
            {
                string filtCode = course.Split('–')[0].Trim();
                if (filtCode != code && filtCode != _subjects.FirstOrDefault(s => s.Code == code)?.SubjectCode)
                    return new();
            }

            q = q.Where(r => r.Date >= dtpFrom.Value.Date && r.Date <= dtpTo.Value.Date);
            return q.ToList();
        }

        // ── Totals ────────────────────────────────────────────────────────────────
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

        // ── Summary Cards ─────────────────────────────────────────────────────────
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

        private int CountAtRiskSubjects() =>
            _subjects.Count(meta =>
            {
                var r = FilteredRecords(meta.Code);
                if (r.Count == 0) return false;
                int lt = r.Count(x => x.Status == "Late");
                int ab = r.Count(x => x.Status == "Absent");
                double p = Math.Max(0, (r.Count - ab - lt / LATE_PER_ABS) * 100.0 / r.Count);
                return p < REQUIRED_PCT;
            });

        // ── Mini Stats ────────────────────────────────────────────────────────────
        private void RefreshMiniStats()
        {
            lblMiniPresent.Text = $"● Present: {_present}";
            lblMiniLate.Text = $"● Late: {_late}";
            lblMiniAbsent.Text = $"● Absent: {_absent}";
            lblMiniExcused.Text = $"● Excused: {_excused}";
            lblProgressPct.Text = $"{(int)Math.Round(_pct)}% overall";
            pnlProgress?.Invalidate();
        }

        // ── Segmented progress bar (unchanged paint) ──────────────────────────────
        private void DrawSegmentedProgress(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var pan = (Panel)sender;
            int w = pan.Width;
            int h = pan.Height;

            using var bg = new SolidBrush(Color.FromArgb(230, 230, 238));
            g.FillRectangle(bg, 0, 0, w, h);
            if (_total <= 0) return;

            int xP = 0;
            int wP = (int)(w * (_present / (double)_total));
            int wL = (int)(w * (_late / (double)_total));
            int wA = (int)(w * (_absent / (double)_total));
            int wE = (int)(w * (_excused / (double)_total));

            using var bP = new SolidBrush(Color.FromArgb(0, 160, 75));
            using var bL = new SolidBrush(Color.FromArgb(220, 150, 0));
            using var bA = new SolidBrush(Color.FromArgb(210, 45, 45));
            using var bE = new SolidBrush(Color.FromArgb(60, 110, 210));

            g.FillRectangle(bP, xP, 0, wP, h); xP += wP;
            g.FillRectangle(bL, xP, 0, wL, h); xP += wL;
            g.FillRectangle(bA, xP, 0, wA, h); xP += wA;
            g.FillRectangle(bE, xP, 0, wE, h);
        }

        // ── Subjects Grid ─────────────────────────────────────────────────────────
        private DataTable? _subjectsDT;

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

                _subjectsDT.Rows.Add(
                    meta.SubjectCode, meta.Name, meta.Schedule,
                    n, pr, lt, ab, ex, $"{p}%", st);
            }
            dgvSubjects.DataSource = _subjectsDT;
        }

        private void DgvSubjects_CellFormatting(object sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;
            string col = dgvSubjects.Columns[e.ColumnIndex].Name;

            if (col == "Status")
            {
                string s = e.Value.ToString()!;
                e.CellStyle.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                (e.CellStyle.ForeColor, e.CellStyle.BackColor) = s switch
                {
                    "Excellent" => (Color.FromArgb(0, 120, 60), Color.FromArgb(220, 255, 235)),
                    "Good" => (Color.FromArgb(0, 130, 70), Color.FromArgb(230, 255, 240)),
                    "At Risk" => (Color.FromArgb(180, 100, 0), Color.FromArgb(255, 243, 210)),
                    _ => (Color.FromArgb(180, 30, 30), Color.FromArgb(255, 232, 232)),
                };
            }
            else if (col == "Att%")
            {
                if (double.TryParse(e.Value.ToString()!.Replace("%", ""), out double p))
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
            string? code = dgvSubjects.Rows[e.RowIndex].Cells["Code"]?.Value?.ToString();
            // Resolve display code back to offering id
            var match = _subjects.FirstOrDefault(s =>
                s.SubjectCode == code || s.Code == code);
            _selectedCode = match?.Code;
            if (_selectedCode == null) return;
            lblAttendanceLogTitle.Text = $"Attendance Log  –  {code}";
            RefreshLogsGrid();
            RefreshQRGrid();
        }

        // ── Logs Grid (read-only — QR-verified rows marked) ───────────────────────
        private DataTable? _logsDT;

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
                _logsDT.Columns.Add("Verified", typeof(bool));  // hidden column
            }
            _logsDT.Rows.Clear();

            var toShow = _selectedCode != null
                ? _subjects.Where(s => s.Code == _selectedCode)
                : _subjects;

            foreach (var meta in toShow)
                foreach (var rec in FilteredRecords(meta.Code).OrderByDescending(r => r.Date))
                {
                    string statusDisplay = rec.IsQrVerified
                        ? "🔒 Present (QR Verified)"
                        : rec.Status;
                    _logsDT.Rows.Add(
                        rec.Date.ToString("MMM d, yyyy (ddd)"),
                        meta.SubjectCode,
                        rec.Session,
                        rec.Period,
                        statusDisplay,
                        rec.Remarks,
                        rec.IsQrVerified);
                }

            // Hide the "Verified" boolean column — it's just for row colouring
            dgvLogs.DataSource = _logsDT;
            if (dgvLogs.Columns.Contains("Verified"))
                dgvLogs.Columns["Verified"].Visible = false;
        }

        private void DgvLogs_CellFormatting(object sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            // Tint QR-verified rows
            var row = dgvLogs.Rows[e.RowIndex];
            bool isQr = false;
            if (dgvLogs.Columns.Contains("Verified"))
            {
                var v = row.Cells["Verified"].Value;
                isQr = v is bool b && b;
            }
            if (isQr)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(232, 248, 232);
                row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(210, 240, 210);
            }

            string col = dgvLogs.Columns[e.ColumnIndex].Name;
            if (col == "Status")
            {
                string s = e.Value.ToString()!;
                e.CellStyle.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                if (s.Contains("QR Verified"))
                {
                    e.CellStyle.ForeColor = Color.FromArgb(0, 120, 60);
                    e.CellStyle.BackColor = Color.FromArgb(220, 255, 235);
                }
                else
                {
                    (e.CellStyle.ForeColor, e.CellStyle.BackColor) = s switch
                    {
                        "Present" => (Color.FromArgb(0, 130, 60), Color.FromArgb(220, 255, 235)),
                        "Late" => (Color.FromArgb(180, 110, 0), Color.FromArgb(255, 243, 210)),
                        "Excused" => (Color.FromArgb(50, 100, 200), Color.FromArgb(220, 235, 255)),
                        _ => (Color.FromArgb(180, 30, 30), Color.FromArgb(255, 232, 232)),
                    };
                }
            }
            else if (col == "Period")
            {
                string p = e.Value.ToString()!;
                e.CellStyle.Font = new Font("Segoe UI", 8f, FontStyle.Regular);
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                (e.CellStyle.ForeColor, e.CellStyle.BackColor) = p switch
                {
                    "Prelim" => (Color.FromArgb(70, 70, 180), Color.FromArgb(230, 230, 255)),
                    "Midterm" => (Color.FromArgb(150, 80, 0), Color.FromArgb(255, 245, 220)),
                    "Final Term" => (Color.FromArgb(128, 0, 0), Color.FromArgb(255, 235, 235)),
                    _ => (Color.FromArgb(80, 80, 80), Color.White),
                };
            }
        }

        // ── QR Scan History Grid ──────────────────────────────────────────────────
        private DataTable? _qrDT;

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

            var toShow = _selectedCode != null
                ? _subjects.Where(s => s.Code == _selectedCode)
                : _subjects;

            foreach (var meta in toShow)
                foreach (var rec in FilteredRecords(meta.Code)
                    .Where(r => r.IsQR)
                    .OrderByDescending(r => r.Date))
                {
                    string scanTime = rec.QrScannedAt.HasValue
                        ? rec.QrScannedAt.Value.ToLocalTime().ToString("HH:mm")
                        : "—";
                    _qrDT.Rows.Add(
                        rec.Date.ToString("MMM d, yyyy (ddd)"),
                        meta.SubjectCode,
                        rec.Session,
                        scanTime,
                        rec.Status);
                }

            dgvQR.DataSource = _qrDT;
        }

        // ── At-Risk Popup ─────────────────────────────────────────────────────────
        private void OnViewDetailsClick(object sender, EventArgs e)
        {
            var atRiskList = new List<AtRiskSubjectInfo>();
            foreach (var meta in _subjects)
            {
                var r = FilteredRecords(meta.Code);
                if (r.Count == 0) continue;
                int lt = r.Count(x => x.Status == "Late");
                int ab = r.Count(x => x.Status == "Absent");
                double p = Math.Max(0, (r.Count - ab - lt / LATE_PER_ABS) * 100.0 / r.Count);
                if (p < REQUIRED_PCT)
                    atRiskList.Add(new AtRiskSubjectInfo
                    {
                        Code = meta.SubjectCode,
                        Name = meta.Name,
                        Attendance = Math.Round(p, 1),
                        Absent = ab,
                    });
            }
            AtRiskPopup.Show(this.FindForm(), atRiskList);
        }

        // ── Scan QR Code button ───────────────────────────────────────────────────
        private void BtnScanQR_Click(object sender, EventArgs e)
        {
            using var frm = new Form
            {
                Text = "Scan QR Attendance Code",
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

            var scanner = new QRScanControl
            {
                Dock = DockStyle.Fill,
                CurrentStudentId = CurrentStudentId,   // ← bind student identity
            };
            frm.Controls.Add(scanner);

            // On successful scan: reload attendance data from DB and close
            scanner.QRCodeScanned += (s2, result) =>
            {
                LoadFromDatabase();
                RefreshAll();
                frm.DialogResult = DialogResult.OK;
                frm.Close();
            };

            frm.ShowDialog(this.FindForm());
        }

        // ── Grid styling helpers ──────────────────────────────────────────────────
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

        /// <summary>
        /// Sizes a DataGridView to fit its content rows, capped at
        /// <paramref name="maxHeight"/> pixels.  Called from DataBindingComplete.
        /// </summary>
        internal static void AutoSizeGrid(DataGridView dgv, int maxHeight)
        {
            if (dgv == null) return;
            int rowsH = dgv.Rows.GetRowsHeight(DataGridViewElementStates.Visible);
            int desired = dgv.ColumnHeadersHeight + rowsH + 2;
            dgv.Height = Math.Min(desired, maxHeight);
            dgv.ScrollBars = desired > maxHeight ? ScrollBars.Vertical : ScrollBars.None;
        }

        /// <summary>
        /// Draws a single-pixel border around a card panel with a 4-pixel
        /// accent bar on the left edge in <paramref name="accentColor"/>.
        /// Called from Paint event handlers wired in the designer.
        /// </summary>
        internal static void DrawCardBorder(
            System.Drawing.Graphics g,
            Panel card,
            Color accentColor)
        {
            using (var accentBrush = new SolidBrush(accentColor))
                g.FillRectangle(accentBrush, 0, 0, 4, card.Height);

            using (var borderPen = new Pen(Color.FromArgb(225, 225, 235), 1f))
                g.DrawRectangle(borderPen, 0, 0, card.Width - 1, card.Height - 1);
        }

        // ── Inner models ──────────────────────────────────────────────────────────
        private class AttRecord
        {
            public DateTime Date { get; set; }
            public string AcadYear { get; set; } = string.Empty;
            public string Period { get; set; } = string.Empty;
            public string Session { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
            public string Remarks { get; set; } = string.Empty;
            public bool IsQR { get; set; }
            public bool IsQrVerified { get; set; }
            public DateTime? QrScannedAt { get; set; }
            public int AttendanceId { get; set; }
        }

        private class SubjectMeta
        {
            public string Code { get; set; } = string.Empty;
            public string SubjectCode { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public int Units { get; set; }
            public string Schedule { get; set; } = string.Empty;
        }
    }
}