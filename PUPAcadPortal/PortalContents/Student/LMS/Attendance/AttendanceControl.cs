using PUPAcadPortal.PortalContents.Student.LMS.Attendance;
using PUPAcadPortal.Models;
using PUPAcadPortal.Services;
using PUPAcadPortal.Data;
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
        //  Identity (set from parent before control is shown)
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentStudentId
        {
            get => _currentStudentId > 0 ? _currentStudentId
                   : (UserSession.StudentID ?? 0);
            set => _currentStudentId = value;
        }
        private int _currentStudentId = 0;
        private List<SubjectMeta> _subjects = new();
        private Dictionary<string, List<AttRecord>> _records = new();

        private string? _selectedCode;
        private int _total, _present, _absent, _late, _excused;
        private double _pct;

        private const double REQUIRED_PCT = 80.0;
        private const int LATE_PER_ABS = 3;

        //  DB factory
        private static AppDbContext CreateContext() => new AppDbContext();
        public AttendanceControl() => InitializeComponent();

        //  Load
        private void AttendanceControl_Load(object sender, EventArgs e)
        {
            LoadFromDatabase();
            RefreshAll();
        }

        // ── Load all attendance data from DB for this student ──────────────────────
        /// <summary>
        /// Reloads subjects and attendance records from scratch.
        /// IMPORTANT: clears both _subjects, _records, AND cmbCourse.Items before
        /// rebuilding so that repeated calls (e.g. triggered by QR scan success)
        /// never produce duplicate entries in the dropdown.
        /// </summary>
        private void LoadFromDatabase()
        {
            _subjects.Clear();
            _records.Clear();

            // Always clear the combo before rebuilding to prevent duplicates
            // when this method is called more than once (e.g. after a QR scan).
            cmbCourse.Items.Clear();
            cmbCourse.Items.Add("All Courses");

            if (CurrentStudentId <= 0) return;

            try
            {
                using var ctx = CreateContext();

                // All SubjectOfferings this student is enrolled in
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
                        Schedule = string.Empty,
                    };
                    _subjects.Add(meta);
                    cmbCourse.Items.Add($"{meta.SubjectCode} – {meta.Name}");

                    // Load all AttendanceRecords for this student in this offering.
                    // FIX: The JOIN goes through ClassSession → SubjectOffering so
                    // QR-scanned records (which are inserted with SessionId only) are
                    // picked up correctly — we no longer rely on a direct OfferingId
                    // field on AttendanceRecord.
                    var rawRecords = ctx.AttendanceRecords
                        .Include(ar => ar.Session)
                            .ThenInclude(cs => cs.SubjectOffering)
                        .Where(ar =>
                            ar.StudentId == CurrentStudentId &&
                            ar.Session.SubjectOfferingId == offering.SubjectOfferingId)
                        .OrderByDescending(ar => ar.Session.SessionDate)
                        .ToList();

                    var list = rawRecords.Select(ar => new AttRecord
                    {
                        Date = ar.Session.SessionDate,
                        AcadYear = offering.AcademicPeriod?.SchoolYear ?? "—",
                        Period = DerivePeriod(ar.Session.SessionDate, offering.AcademicPeriod),
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

            // Reset to "All Courses" after every reload
            if (cmbCourse.Items.Count > 0) cmbCourse.SelectedIndex = 0;
        }

        //  Derive Prelim / Midterm / Final Term from date + academic period
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
                var meta = _subjects.FirstOrDefault(s => s.Code == code);
                if (filtCode != code && filtCode != meta?.SubjectCode)
                    return new();
            }

            q = q.Where(r => r.Date >= dtpFrom.Value.Date && r.Date <= dtpTo.Value.Date);
            return q.ToList();
        }

        //  Totals
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

        //  Summary Cards
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

        //  Mini Stats
        private void RefreshMiniStats()
        {
            lblMiniPresent.Text = $"● Present: {_present}";
            lblMiniLate.Text = $"● Late: {_late}";
            lblMiniAbsent.Text = $"● Absent: {_absent}";
            lblMiniExcused.Text = $"● Excused: {_excused}";
            lblProgressPct.Text = $"{(int)Math.Round(_pct)}% overall";
            pnlProgress?.Invalidate();
        }

        //  Segmented progress bar
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

        //  Subjects grid
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
                string st = p >= 90 ? "Excellent"
                           : p >= 80 ? "Good"
                           : p >= 65 ? "At Risk"
                           : "Dropped";

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
            else if (col == "Absent" && int.TryParse(e.Value.ToString(), out int ab2) && ab2 > 0)
                e.CellStyle.ForeColor = Color.FromArgb(200, 40, 40);
            else if (col == "Late" && int.TryParse(e.Value.ToString(), out int lt2) && lt2 > 0)
                e.CellStyle.ForeColor = Color.FromArgb(180, 110, 0);
        }

        private void DgvSubjects_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string? code = dgvSubjects.Rows[e.RowIndex].Cells["Code"]?.Value?.ToString();
            var match = _subjects.FirstOrDefault(s =>
                s.SubjectCode == code || s.Code == code);
            _selectedCode = match?.Code;
            if (_selectedCode == null) return;
            lblAttendanceLogTitle.Text = $"Attendance Log  –  {code}";
            RefreshLogsGrid();
            RefreshQRGrid();
        }

        //  Attendance log grid
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
                _logsDT.Columns.Add("Verified", typeof(bool));
            }
            _logsDT.Rows.Clear();

            var toShow = _selectedCode != null
                ? _subjects.Where(s => s.Code == _selectedCode)
                : _subjects;

            foreach (var meta in toShow)
                foreach (var rec in FilteredRecords(meta.Code).OrderByDescending(r => r.Date))
                {
                    string statusDisplay = rec.IsQrVerified
                        ? "Present (QR Verified)"
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

            dgvLogs.DataSource = _logsDT;
            if (dgvLogs.Columns.Contains("Verified"))
                dgvLogs.Columns["Verified"].Visible = false;
        }

        private void DgvLogs_CellFormatting(object sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

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

        //  QR scan history grid
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

        //  At-Risk popup
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

        //  Scan QR Code button
        private void BtnScanQR_Click(object sender, EventArgs e)
        {
            if (CurrentStudentId <= 0)
            {
                MessageBox.Show(
                    "Student identity is not set. Please log out and log in again.",
                    "Identity Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
                CurrentStudentId = CurrentStudentId,
            };
            frm.Controls.Add(scanner);

            // FIX: on successful scan reload all data from DB and refresh every
            // grid/card so the new attendance log entry appears immediately.
            scanner.QRCodeScanned += (s2, result) =>
            {
                // Always close the scanner window first
                frm.DialogResult = DialogResult.OK;
                frm.Close();

                // Full reload from DB so the new AttendanceRecord created by
                // QrAttendanceService is reflected in all views.
                LoadFromDatabase();
                RefreshAll();
            };

            frm.ShowDialog(this.FindForm());

            // Safety net: reload even if the scanner was closed without firing the
            // event (e.g. user closed the window after a successful scan).
            LoadFromDatabase();
            RefreshAll();
        }

        //  Filter/combo change handlers ────────────────────────────────────────────
        // These are wired in the designer; add them here if not already present
        // so filter changes immediately refresh all views without a DB reload.
        private void CmbCourse_SelectedIndexChanged(object sender, EventArgs e)
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

        private void CmbPeriod_SelectedIndexChanged(object sender, EventArgs e)
            => RefreshAll();

        private void DtpFrom_ValueChanged(object sender, EventArgs e)
            => RefreshAll();

        private void DtpTo_ValueChanged(object sender, EventArgs e)
            => RefreshAll();

        //  Grid styling helpers
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

        internal static void AutoSizeGrid(DataGridView dgv, int maxHeight)
        {
            if (dgv == null) return;
            int rowsH = dgv.Rows.GetRowsHeight(DataGridViewElementStates.Visible);
            int desired = dgv.ColumnHeadersHeight + rowsH + 2;
            dgv.Height = Math.Min(desired, maxHeight);
            dgv.ScrollBars = desired > maxHeight ? ScrollBars.Vertical : ScrollBars.None;
        }

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

        //  Inner models
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