using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Models;
using PUPAcadPortal.Services;

namespace PUPAcadPortal.PortalContents.Instructor.LMS
{
    public partial class AttendanceContentInst : UserControl
    {
        // ── Runtime state ─────────────────────────────────────────────────────────
        private List<StudentAttendanceRecord> _allStudents = new();
        private List<CourseSection> _courseCatalogue = new();
        private List<SessionSlot> _sessionSlots = new();

        private int? _currentSessionId = null;

        // RoomSchedule times for the selected course (used to build signed token)
        private TimeSpan? _currentStartTime = null;
        private TimeSpan? _currentEndTime = null;

        private SessionAttendanceControl _sessionCard = null!;
        private AttendanceGridControl _grid = null!;
        private System.Windows.Forms.Timer _searchTimer = null!;
        private string _pendingSearch = "";

        // ── DB factory ────────────────────────────────────────────────────────────
        private static AppDbContext CreateContext() => new AppDbContext();

        public AttendanceContentInst() => InitializeComponent();

        // ── Load ─────────────────────────────────────────────────────────────────
        private void AttendanceContentInst_Load(object sender, EventArgs e)
        {
            try
            {
                LayoutSummaryCards();
                InitAttendance();

                // Auto-expire stale QrSession rows on portal load
                try { QrSessionService.ExpireAllStale(); } catch { /* best-effort */ }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Attendance failed to load:\n\n{ex.Message}\n\n{ex.StackTrace}",
                    "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Layout ────────────────────────────────────────────────────────────────
        private void PnlSummaryRow_SizeChanged(object sender, EventArgs e) => LayoutSummaryCards();

        private void Card_Paint(object sender, PaintEventArgs e)
        {
            if (sender is Panel p && p.Tag is Color accentColor)
            {
                using (var b = new SolidBrush(accentColor))
                    e.Graphics.FillRectangle(b, 0, 0, 4, p.Height);
                using (var bp = new Pen(Color.FromArgb(230, 230, 230)))
                    e.Graphics.DrawRectangle(bp, 0, 0, p.Width - 1, p.Height - 1);
            }
        }

        private void LayoutSummaryCards()
        {
            const int PAD = 6;
            const int H = 104;
            int totalW = pnlSummaryRow.ClientSize.Width - PAD * 2;

            int sessionW = (int)(totalW * 0.28);
            int remaining = totalW - sessionW - PAD * 5;
            int cardW = remaining / 5;

            int x = PAD;
            int y = (pnlSummaryRow.ClientSize.Height - H) / 2;
            if (y < 0) y = 0;

            void Place(Panel p, int w)
            {
                p.Location = new Point(x, y);
                p.Size = new Size(w, H);
                x += w + PAD;
                if (p == pnlCardSession)
                {
                    panel21.Location = new Point(4, 22);
                    panel21.Size = new Size(w - 8, H - 26);
                }
            }

            Place(pnlCardSession, sessionW);
            Place(pnlCardPresent, cardW);
            Place(pnlCardLate, cardW);
            Place(pnlCardAbsent, cardW);
            Place(pnlCardExcused, cardW);
            Place(pnlCardLastUpdate, cardW + (totalW - sessionW - cardW * 5 - PAD * 5));
        }

        // ── Init ─────────────────────────────────────────────────────────────────
        private void InitAttendance()
        {
            LoadCoursesFromDb();
            PopulateDropdowns();

            _sessionCard = new SessionAttendanceControl { Dock = DockStyle.Fill };
            panel21.Controls.Clear();
            panel21.Controls.Add(_sessionCard);

            _grid = new AttendanceGridControl { Dock = DockStyle.Fill };
            pnlGrid.Controls.Clear();
            pnlGrid.Controls.Add(_grid);
            _grid.AttendanceChanged += (s, e) =>
            {
                UpdateSummaryCards();
                UpdateLastUpdated();
            };

            _searchTimer = new System.Windows.Forms.Timer { Interval = 160 };
            _searchTimer.Tick += (s, e) =>
            {
                _searchTimer.Stop();
                _grid.ApplyFilter(_pendingSearch);
            };

            LoadCurrentSession();
            _grid.LoadStudents(_allStudents);
            WireFilterBar();
            WireButtons();
            UpdateSummaryCards();
            UpdateLastUpdated();
        }

        // ── Load courses from DB ──────────────────────────────────────────────────
        private void LoadCoursesFromDb()
        {
            try
            {
                using var ctx = CreateContext();
                var offerings = ctx.SubjectOfferings
                    .Include(so => so.Subject)
                    .Include(so => so.RoomSchedules)
                    .OrderBy(so => so.Subject.SubjectCode)
                    .ThenBy(so => so.Section)
                    .ToList();

                _courseCatalogue = offerings.Select(so => new CourseSection
                {
                    Code = so.SubjectOfferingId,
                    Title = so.Subject?.SubjectName ?? so.SubjectOfferingId,
                    Section = so.Section ?? string.Empty,
                    // Resolve the first RoomSchedule time so the QR token window is real
                    StartTime = so.RoomSchedules.FirstOrDefault()?.StartTime,
                    EndTime = so.RoomSchedules.FirstOrDefault()?.EndTime,
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Could not load courses from the database.\n\n{ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _courseCatalogue = new List<CourseSection>();
            }
        }

        private void PopulateDropdowns()
        {
            cmbCourse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCourse.Items.Clear();
            foreach (var c in _courseCatalogue)
                cmbCourse.Items.Add(c.DisplayName);
            if (cmbCourse.Items.Count > 0) cmbCourse.SelectedIndex = 0;

            // Session slot dropdown is kept as a label-only selector;
            // actual attendance window comes from RoomSchedule times.
            cmbSession.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSession.Items.Clear();
            var slots = new[]
            {
                "Morning (7:30 AM – 9:00 AM)",
                "Morning (8:00 AM – 10:00 AM)",
                "Morning (9:00 AM – 10:30 AM)",
                "Midday (10:30 AM – 12:00 PM)",
                "Midday (11:00 AM – 12:30 PM)",
                "Afternoon (1:00 PM – 2:30 PM)",
                "Afternoon (2:30 PM – 4:00 PM)",
                "Evening (5:00 PM – 7:00 PM)",
                "Saturday (8:00 AM – 12:00 PM)",
            };
            foreach (var s in slots) cmbSession.Items.Add(s);
            if (cmbSession.Items.Count > 1) cmbSession.SelectedIndex = 1;
        }

        // ── Load / refresh current session roster from DB ─────────────────────────
        /// <summary>
        /// Finds (or creates) the ClassSession row, loads all enrolled students,
        /// and maps their AttendanceRecords — including QR-verified lock state.
        /// Also resolves the RoomSchedule start/end times for this offering so the
        /// QR token embeds the correct attendance window.
        /// </summary>
        private void LoadCurrentSession()
        {
            if (_courseCatalogue.Count == 0) return;

            int idx = cmbCourse.SelectedIndex;
            if (idx < 0 || idx >= _courseCatalogue.Count) return;

            var course = _courseCatalogue[idx];
            string offeringId = course.Code;
            DateTime date = dtpDate.Value.Date;

            // Cache schedule times for the QR popup
            _currentStartTime = course.StartTime;
            _currentEndTime = course.EndTime;

            try
            {
                using var ctx = CreateContext();

                // 1. Find or create the ClassSession
                var session = ctx.ClassSessions
                    .FirstOrDefault(cs =>
                        cs.SubjectOfferingId == offeringId &&
                        cs.SessionDate.Date == date);

                if (session == null)
                {
                    session = new PUPAcadPortal.Models.ClassSession
                    {
                        SubjectOfferingId = offeringId,
                        SessionDate = date,
                        StartTime = _currentStartTime,
                        EndTime = _currentEndTime,
                        Topic = "—",
                    };
                    ctx.ClassSessions.Add(session);
                    ctx.SaveChanges();
                }

                _currentSessionId = session.SessionId;

                // 2. Load existing AttendanceRecords for this session
                var existingRecords = ctx.AttendanceRecords
                    .Include(ar => ar.Student).ThenInclude(st => st.User)
                    .Where(ar => ar.SessionId == session.SessionId)
                    .ToList();

                // 3. Load enrolled students via EnrollmentSubject
                var enrolledStudents = ctx.EnrollmentSubjects
                    .Include(es => es.Enrollment)
                        .ThenInclude(en => en.Student)
                            .ThenInclude(st => st.User)
                    .Where(es => es.SubjectOfferingId == offeringId)
                    .Select(es => es.Enrollment.Student)
                    .Distinct()
                    .OrderBy(st => st.User.LastName)
                    .ThenBy(st => st.User.FirstName)
                    .ToList();

                // 4. Map to UI model
                var list = new List<StudentAttendanceRecord>();
                int row = 1;

                foreach (var student in enrolledStudents)
                {
                    var existing = existingRecords
                        .FirstOrDefault(ar => ar.StudentId == student.StudentId);

                    AttendanceStatus status = existing == null
                        ? AttendanceStatus.Present
                        : ParseStatus(existing.Status);

                    list.Add(new StudentAttendanceRecord
                    {
                        AttendanceId = existing?.AttendanceId ?? 0,
                        StudentId = student.StudentId,
                        IsQrVerified = existing?.IsQrVerified ?? false,
                        QrScannedAt = existing?.QrScannedAt,
                        RowNumber = row++,
                        LastName = student.User?.LastName ?? string.Empty,
                        FirstName = student.User?.FirstName ?? string.Empty,
                        MiddleInitial = student.User?.MiddleName?.Length > 0
                                            ? student.User.MiddleName.Substring(0, 1)
                                            : string.Empty,
                        IdNumber = student.StudentNumber,
                        Status = status,
                        Remarks = existing?.Remarks ?? string.Empty,
                    });
                }

                _allStudents = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Could not load session roster from database.\n\n{ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ── Filter bar ────────────────────────────────────────────────────────────
        private void WireFilterBar()
        {
            cmbCourse.SelectedIndexChanged += (s, e) => ReloadAndRefresh();
            cmbSession.SelectedIndexChanged += (s, e) => ReloadAndRefresh();
            dtpDate.ValueChanged += (s, e) => ReloadAndRefresh();

            txtSearch.ForeColor = Color.Gray;
            txtSearch.GotFocus += (s, e) =>
            {
                if (txtSearch.Text == "Search student…")
                { txtSearch.Text = ""; txtSearch.ForeColor = Color.Black; }
            };
            txtSearch.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                { txtSearch.Text = "Search student…"; txtSearch.ForeColor = Color.Gray; }
            };
            txtSearch.TextChanged += (s, e) =>
            {
                string q = txtSearch.Text;
                if (q == "Search student…") q = "";
                _pendingSearch = q;
                _searchTimer.Stop();
                _searchTimer.Start();
            };
        }

        private void ReloadAndRefresh()
        {
            LoadCurrentSession();
            _grid.LoadStudents(_allStudents);
            UpdateSummaryCards();
            UpdateLastUpdated();
        }

        // ── Button wiring ─────────────────────────────────────────────────────────
        private void WireButtons()
        {
            btnSaveAttendance.Click -= BtnSave_Click;
            btnSaveAttendance.Click += BtnSave_Click;

            btnRefresh.Click += (s, e) =>
            {
                ReloadAndRefresh();
                MessageBox.Show("Attendance refreshed from database.", "Refresh",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnQRCode.Click -= BtnQrCode_Click;
            btnQRCode.Click += BtnQrCode_Click;

            btnExport.Click += (s, e) => ExportCsv();
            btnImportCSV.Click += (s, e) => ImportCsv();
        }

        // ── QR popup ─────────────────────────────────────────────────────────────
        private void BtnQrCode_Click(object? sender, EventArgs e)
        {
            if (_currentSessionId == null || _currentSessionId <= 0)
            {
                MessageBox.Show(
                    "Please select a valid course and date before generating a QR code.",
                    "No Session", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var course = _courseCatalogue.ElementAtOrDefault(cmbCourse.SelectedIndex);
            if (course == null) return;

            // Open the popup — QrCodePopupForm → QrCodeAttendanceControl →
            // QrSessionService.CreateOrGetActive() handles duplicate prevention.
            using var dlg = new QrCodePopupForm(
                course: cmbCourse.Text,
                session: cmbSession.Text,
                date: dtpDate.Value,
                sessionId: _currentSessionId.Value,
                offeringId: course.Code,
                startTime: _currentStartTime,
                endTime: _currentEndTime);

            dlg.ShowDialog(this);

            // Refresh the grid in case students scanned while the popup was open
            ReloadAndRefresh();
        }

        // ── Save attendance (manual, skips QR-verified rows) ─────────────────────
        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (_currentSessionId == null)
            {
                MessageBox.Show(
                    "No active session to save. Please select a valid course and date.",
                    "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var ctx = CreateContext();

                var existingInDb = ctx.AttendanceRecords
                    .Where(ar => ar.SessionId == _currentSessionId.Value)
                    .ToDictionary(ar => ar.StudentId);

                int savedCount = 0;
                int skippedQr = 0;

                foreach (var ui in _allStudents)
                {
                    // Never overwrite a QR-verified record
                    if (ui.IsQrVerified) { skippedQr++; continue; }

                    string statusStr = StatusString(ui.Status);
                    string remarks = ui.Remarks ?? string.Empty;

                    if (existingInDb.TryGetValue(ui.StudentId, out var dbRec))
                    {
                        if (!dbRec.IsQrVerified)
                        {
                            dbRec.Status = statusStr;
                            dbRec.Remarks = remarks;
                            savedCount++;
                        }
                        else skippedQr++;
                    }
                    else
                    {
                        ctx.AttendanceRecords.Add(new PUPAcadPortal.Models.AttendanceRecord
                        {
                            SessionId = _currentSessionId.Value,
                            StudentId = ui.StudentId,
                            Status = statusStr,
                            Remarks = remarks,
                            IsQrVerified = false,
                        });
                        savedCount++;
                    }
                }

                ctx.SaveChanges();

                // Refresh AttendanceIds from DB
                var saved = ctx.AttendanceRecords
                    .Where(ar => ar.SessionId == _currentSessionId.Value)
                    .ToDictionary(ar => ar.StudentId);
                foreach (var ui in _allStudents)
                    if (saved.TryGetValue(ui.StudentId, out var r))
                        ui.AttendanceId = r.AttendanceId;

                UpdateLastUpdated();
                UpdateSummaryCards();

                int present = _allStudents.Count(x => x.Status == AttendanceStatus.Present);
                int late = _allStudents.Count(x => x.Status == AttendanceStatus.Late);
                int absent = _allStudents.Count(x => x.Status == AttendanceStatus.Absent);
                int excused = _allStudents.Count(x => x.Status == AttendanceStatus.Excused);

                string qrNote = skippedQr > 0
                    ? $"\n\n🔒 {skippedQr} QR-verified record{(skippedQr > 1 ? "s" : "")} were not modified."
                    : string.Empty;

                MessageBox.Show(
                    $"Attendance saved!\n\n" +
                    $"Course   : {cmbCourse.Text}\n" +
                    $"Date     : {dtpDate.Value:MMMM dd, yyyy}\n" +
                    $"Session  : {cmbSession.Text}\n\n" +
                    $"Present  : {present}\n" +
                    $"Late     : {late}\n" +
                    $"Absent   : {absent}\n" +
                    $"Excused  : {excused}\n" +
                    $"Total    : {_allStudents.Count}" +
                    qrNote,
                    "Attendance Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to save attendance to the database.\n\n{ex.Message}",
                    "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Summary cards ─────────────────────────────────────────────────────────
        private void UpdateSummaryCards()
        {
            int total = _allStudents.Count;
            int present = _allStudents.Count(x => x.Status == AttendanceStatus.Present);
            int late = _allStudents.Count(x => x.Status == AttendanceStatus.Late);
            int absent = _allStudents.Count(x => x.Status == AttendanceStatus.Absent);
            int excused = _allStudents.Count(x => x.Status == AttendanceStatus.Excused);

            lblPresentNum.Text = present.ToString();
            lblLateNum.Text = late.ToString();
            lblAbsentNum.Text = absent.ToString();
            lblExcusedNum.Text = excused.ToString();

            lblPresentPct.Text = total > 0 ? $"{present * 100.0 / total:F1}%" : "–";
            lblLatePct.Text = total > 0 ? $"{late * 100.0 / total:F1}%" : "–";
            lblAbsentPct.Text = total > 0 ? $"{absent * 100.0 / total:F1}%" : "–";
            lblExcusedPct.Text = total > 0 ? $"{excused * 100.0 / total:F1}%" : "–";

            _sessionCard?.SetData(present, late, absent, excused);
        }

        private void UpdateLastUpdated()
        {
            lblDateTime.Text = DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt");
            lblByInstructor.Text = "by Instructor";
        }

        // ── Export / Import CSV ───────────────────────────────────────────────────
        private void ExportCsv()
        {
            using var sfd = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = $"Attendance_{dtpDate.Value:yyyyMMdd}.csv",
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;
            try
            {
                var lines = new List<string>
                    { "Row,Last Name,First Name,MI,ID Number,Status,Remarks,QR Verified" };
                foreach (var s in _allStudents)
                    lines.Add(
                        $"{s.RowNumber},{s.LastName},{s.FirstName},{s.MiddleInitial}," +
                        $"{s.IdNumber},{s.Status},{s.Remarks},{s.IsQrVerified}");
                File.WriteAllLines(sfd.FileName, lines);
                MessageBox.Show("Exported successfully!", "Export",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Export failed: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImportCsv()
        {
            using var ofd = new OpenFileDialog { Filter = "CSV files (*.csv)|*.csv" };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            try
            {
                var lines = File.ReadAllLines(ofd.FileName);
                int imported = 0;
                foreach (var line in lines.Skip(1))
                {
                    var parts = line.Split(',');
                    if (parts.Length < 6) continue;
                    if (!int.TryParse(parts[0], out int rowNum)) continue;
                    var rec = _allStudents.FirstOrDefault(s => s.RowNumber == rowNum);
                    if (rec == null || rec.IsQrVerified) continue;  // skip QR-locked rows

                    rec.Status = parts[5].Trim() switch
                    {
                        "Absent" => AttendanceStatus.Absent,
                        "Late" => AttendanceStatus.Late,
                        "Excused" => AttendanceStatus.Excused,
                        _ => AttendanceStatus.Present,
                    };
                    if (parts.Length > 6) rec.Remarks = parts[6].Trim();
                    imported++;
                }
                _grid.LoadStudents(_allStudents);
                UpdateSummaryCards();
                MessageBox.Show($"Imported {imported} records.", "Import",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Import failed: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Helpers ───────────────────────────────────────────────────────────────
        private static AttendanceStatus ParseStatus(string? s) => s switch
        {
            "Absent" => AttendanceStatus.Absent,
            "Late" => AttendanceStatus.Late,
            "Excused" => AttendanceStatus.Excused,
            _ => AttendanceStatus.Present,
        };

        private static string StatusString(AttendanceStatus s) => s switch
        {
            AttendanceStatus.Absent => "Absent",
            AttendanceStatus.Late => "Late",
            AttendanceStatus.Excused => "Excused",
            _ => "Present",
        };
    }
}