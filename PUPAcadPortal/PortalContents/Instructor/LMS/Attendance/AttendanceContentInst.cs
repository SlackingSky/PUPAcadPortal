using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;

namespace PUPAcadPortal.PortalContents.Instructor.LMS
{
    /// <summary>
    /// Main attendance module for instructor view.
    /// Wires the Designer-generated UI to live data, grids, QR code, and summary cards.
    /// </summary>
    public partial class AttendanceContentInst : UserControl
    {
        // ── State ─────────────────────────────────────────────────────────────
        private List<StudentAttendanceRecord> _allStudents = new();
        private List<CourseSection> _courseCatalogue = new();
        private List<SessionSlot> _sessionSlots = new();
        private Dictionary<SessionKey, List<StudentAttendanceRecord>> _snapshots = new();

        // ── Injected controls ─────────────────────────────────────────────────
        private SessionAttendanceControl _sessionCard = null!;
        private AttendanceGridControl _grid = null!;

        // ── QR overlay ────────────────────────────────────────────────────────
        private Panel? _qrOverlay = null;
        private QrCodeAttendanceControl? _qrCard = null;

        // ── Debounce timer for search ─────────────────────────────────────────
        private System.Windows.Forms.Timer _searchTimer = null!;
        private string _pendingSearch = "";

        // ─────────────────────────────────────────────────────────────────────
        //  ENTRY POINT
        // ─────────────────────────────────────────────────────────────────────

        private void AttendanceContentInst_Load(object sender, EventArgs e)
        {
            // Position the cards correctly upon control load
            LayoutSummaryCards();
            InitAttendance();
        }


        private void PnlSummaryRow_SizeChanged(object sender, EventArgs e)
        {
            LayoutSummaryCards();
        }

        // Custom styling logic extracted from your internal function
        private void Card_Paint(object sender, PaintEventArgs e)
        {
            if (sender is Panel p && p.Tag is Color accentColor)
            {
                using (var b = new SolidBrush(accentColor))
                {
                    e.Graphics.FillRectangle(b, 0, 0, 4, p.Height);
                }
                using (var bp = new Pen(Color.FromArgb(230, 230, 230)))
                {
                    e.Graphics.DrawRectangle(bp, 0, 0, p.Width - 1, p.Height - 1);
                }
            }
        }

        // Handles the calculation and positioning of the internal cards
        private void LayoutSummaryCards()
        {
            const int PAD = 6;
            const int H = 104;
            int totalW = pnlSummaryRow.ClientSize.Width - PAD * 2;

            // Session card gets ~28%, rest split equally among 5
            int sessionW = (int)(totalW * 0.28);
            int remaining = totalW - sessionW - PAD * 5;
            int cardW = remaining / 5;

            int x = PAD;
            int y = (pnlSummaryRow.ClientSize.Height - H) / 2;

            void Place(Panel p, int w)
            {
                p.Location = new Point(x, y);
                p.Size = new Size(w, H);
                x += w + PAD;

                // Keep panel21 (SessionAttendanceControl host) filling the card
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

        private void InitAttendance()
        {
            BuildCourseCatalogue();
            BuildSessionSlots();
            PopulateDropdowns();
            SeedAllSnapshots();

            // ── Inject SessionAttendanceControl into panel21 ──────────────────
            _sessionCard = new SessionAttendanceControl { Dock = DockStyle.Fill };
            panel21.Controls.Clear();
            panel21.Controls.Add(_sessionCard);

            // ── Inject AttendanceGridControl into pnlGrid ─────────────────────
            _grid = new AttendanceGridControl { Dock = DockStyle.Fill };
            pnlGrid.Controls.Clear();
            pnlGrid.Controls.Add(_grid);
            _grid.AttendanceChanged += (s, e) =>
            {
                UpdateSummaryCards();
                UpdateLastUpdated();
            };

            // ── Debounce search timer ─────────────────────────────────────────
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

        // ─────────────────────────────────────────────────────────────────────
        //  DATA SEEDING
        // ─────────────────────────────────────────────────────────────────────
        private void BuildCourseCatalogue()
        {
            _courseCatalogue = new List<CourseSection>
            {
                new() { Code="IT 101", Title="Introduction to Programming 1", Section="BSIT 1-1" },
                new() { Code="IT 101", Title="Introduction to Programming 1", Section="BSIT 1-2" },
                new() { Code="IT 102", Title="Introduction to Programming 2", Section="BSIT 1-1" },
                new() { Code="CS 201", Title="Data Structures & Algorithms",  Section="BSCS 2-1" },
                new() { Code="CS 202", Title="Object-Oriented Programming",   Section="BSCS 2-2" },
                new() { Code="IS 301", Title="Database Management Systems",   Section="BSIS 3-1" },
                new() { Code="IS 302", Title="Systems Analysis & Design",     Section="BSIS 3-2" },
                new() { Code="IT 401", Title="Capstone Project 1",            Section="BSIT 4-1" },
            };
        }

        private void BuildSessionSlots()
        {
            _sessionSlots = new List<SessionSlot>
            {
                new() { Label = "Morning (7:30 AM - 9:00 AM)"   },
                new() { Label = "Morning (8:00 AM - 10:00 AM)"  },
                new() { Label = "Morning (9:00 AM - 10:30 AM)"  },
                new() { Label = "Midday  (10:30 AM - 12:00 PM)" },
                new() { Label = "Midday  (11:00 AM - 12:30 PM)" },
                new() { Label = "Afternoon (1:00 PM - 2:30 PM)" },
                new() { Label = "Afternoon (2:30 PM - 4:00 PM)" },
                new() { Label = "Evening  (5:00 PM - 7:00 PM)"  },
                new() { Label = "Saturday (8:00 AM - 12:00 PM)" },
            };
        }

        private void PopulateDropdowns()
        {
            // Course — DropDownList only
            cmbCourse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCourse.Items.Clear();
            foreach (var c in _courseCatalogue)
                cmbCourse.Items.Add(c.DisplayName);
            if (cmbCourse.Items.Count > 0) cmbCourse.SelectedIndex = 0;

            // Session — DropDownList only
            cmbSession.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSession.Items.Clear();
            foreach (var s in _sessionSlots)
                cmbSession.Items.Add(s.Label);
            if (cmbSession.Items.Count > 1) cmbSession.SelectedIndex = 1;
        }

        private void SeedAllSnapshots()
        {
            var rng = new Random(42);
            foreach (var course in _courseCatalogue.Take(4))
                foreach (var slot in _sessionSlots)
                    for (int d = -3; d <= 3; d++)
                    {
                        var key = new SessionKey
                        {
                            CourseDisplay = course.DisplayName,
                            SessionLabel = slot.Label,
                            Date = DateTime.Today.AddDays(d),
                        };
                        _snapshots[key] = BuildRoster(course, rng);
                    }
        }

        private static List<StudentAttendanceRecord> BuildRoster(CourseSection course, Random rng)
        {
            var pool = new (string Last, string First, string MI)[]
            {
                ("Abad","Leonora","Y"),("Aguilar","Dominador","S"),("Aquino","Carlo","B"),
                ("Bautista","Fernando","K"),("Basilan","Hans Louie","L"),("Castillo","Ronnie","G"),
                ("Concepcion","Salvador","F"),("Cruz","Kevin James","P"),("Cruz","Nestor","O"),
                ("Dela Cruz","Alice","M"),("Dela Torre","Aurelio","Q"),("Dizon","Orlando","B"),
                ("Domingo","Rosario","E"),("Espinosa","Paz","C"),("Fernandez","Hermogenes","U"),
                ("Flores","Joy","E"),("Garcia","Ramon","F"),("Gutierrez","Jacinta","W"),
                ("Hernandez","Felicitas","T"),("Lopez","Ana","C"),("Manalo","Kapitan","X"),
                ("Mendoza","Gloria","J"),("Mercado","Quezon","D"),("Navarro","Erlinda","P"),
                ("Ocampo","Mateo","Z"),("Pascual","Natividad","A"),("Perez","Consolacion","R"),
                ("Peralta","Trinidad","G"),("Ramos","Andres","M"),("Reyes","Maria Angela","R"),
                ("Reyes","Ernesto","I"),("Rivera","Mark","D"),("Santiago","Consuelo","L"),
                ("Santos","John Doe","S"),("Santos","Trisha","M"),("Soberano","Liza","M"),
                ("Soriano","Isagani","V"),("Torres","Patricia","H"),("Villanueva","Ericka Mae","T"),
                ("Villanueva","Teresita","N"),("Abella","Rosemarie","D"),("Alcantara","Rogelio","F"),
                ("Antiporda","Lorena","B"),("Bacungan","Manuel","A"),("Baluyot","Cecilia","G"),
            };

            var picked = pool.OrderBy(_ => rng.Next()).Take(40).ToList();
            int idBase = rng.Next(1000, 9999);
            var list = new List<StudentAttendanceRecord>();

            for (int i = 0; i < picked.Count; i++)
            {
                var (last, first, mi) = picked[i];
                int roll = rng.Next(100);
                AttendanceStatus status;
                string remarks = "";

                if (roll < 72) status = AttendanceStatus.Present;
                else if (roll < 82) status = AttendanceStatus.Late;
                else if (roll < 92) status = AttendanceStatus.Absent;
                else
                {
                    status = AttendanceStatus.Excused;
                    remarks = _excuseRemarks[rng.Next(_excuseRemarks.Length)];
                }

                list.Add(new StudentAttendanceRecord
                {
                    RowNumber = i + 1,
                    LastName = last,
                    FirstName = first,
                    MiddleInitial = mi,
                    IdNumber = $"2024-{(idBase + i):D5}-SM-0",
                    Status = status,
                    Remarks = remarks,
                });
            }
            return list;
        }

        private static readonly string[] _excuseRemarks =
        {
            "Medical Certificate","Family Emergency","Hospital Admission",
            "Approved Leave of Absence","OJT Duty","University Event",
            "Athletic Competition","Approved Field Trip",
        };

        private void LoadCurrentSession()
        {
            var key = CurrentKey();
            if (!_snapshots.TryGetValue(key, out var snap))
            {
                snap = BuildRoster(_courseCatalogue.First(), new Random(key.GetHashCode()));
                _snapshots[key] = snap;
            }
            _allStudents = snap;
        }

        private SessionKey CurrentKey() => new SessionKey
        {
            CourseDisplay = cmbCourse.Text,
            SessionLabel = cmbSession.Text,
            Date = dtpDate.Value.Date,
        };

        // ─────────────────────────────────────────────────────────────────────
        //  FILTER BAR WIRING
        // ─────────────────────────────────────────────────────────────────────
        private void WireFilterBar()
        {
            cmbCourse.SelectedIndexChanged += (s, e) => ReloadAndRefresh();
            cmbSession.SelectedIndexChanged += (s, e) => ReloadAndRefresh();
            dtpDate.ValueChanged += (s, e) => ReloadAndRefresh();

            // Search box — debounced
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

        // ─────────────────────────────────────────────────────────────────────
        //  BUTTON WIRING
        // ─────────────────────────────────────────────────────────────────────
        private void WireButtons()
        {
            btnSaveAttendance.Click -= BtnSave_Click;
            btnSaveAttendance.Click += BtnSave_Click;

            btnRefresh.Click += (s, e) =>
            {
                ReloadAndRefresh();
                MessageBox.Show("Attendance refreshed.", "Refresh",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnQRCode.Click -= BtnQrCode_Click;
            btnQRCode.Click += BtnQrCode_Click;

            btnExport.Click += (s, e) => ExportCsv();
            btnImportCSV.Click += (s, e) => ImportCsv();
        }

        // ─────────────────────────────────────────────────────────────────────
        //  QR OVERLAY
        // ─────────────────────────────────────────────────────────────────────
        private void BtnQrCode_Click(object? sender, EventArgs e)
        {
            // Re-use existing overlay if already visible
            if (_qrOverlay != null && !_qrOverlay.IsDisposed && _qrOverlay.Visible)
            {
                _qrCard?.GenerateNew();
                _qrOverlay.BringToFront();
                return;
            }

            // Open as a proper modal dialog instead of overlay for reliability
            using var dlg = new QrCodePopupForm(cmbCourse.Text, cmbSession.Text, dtpDate.Value);
            dlg.ShowDialog(this);
        }

        // ─────────────────────────────────────────────────────────────────────
        //  SUMMARY CARDS
        // ─────────────────────────────────────────────────────────────────────
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

        // ─────────────────────────────────────────────────────────────────────
        //  SAVE
        // ─────────────────────────────────────────────────────────────────────
        private void BtnSave_Click(object? sender, EventArgs e)
        {
            int present = _allStudents.Count(x => x.Status == AttendanceStatus.Present);
            int late = _allStudents.Count(x => x.Status == AttendanceStatus.Late);
            int absent = _allStudents.Count(x => x.Status == AttendanceStatus.Absent);
            int excused = _allStudents.Count(x => x.Status == AttendanceStatus.Excused);

            UpdateLastUpdated();
            UpdateSummaryCards();

            MessageBox.Show(
                $"Attendance saved!\n\n" +
                $"Course  : {cmbCourse.Text}\n" +
                $"Date    : {dtpDate.Value:MMMM dd, yyyy}\n" +
                $"Session : {cmbSession.Text}\n\n" +
                $"Present : {present}\n" +
                $"Late    : {late}\n" +
                $"Absent  : {absent}\n" +
                $"Excused : {excused}\n" +
                $"Total   : {_allStudents.Count}",
                "Attendance Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ─────────────────────────────────────────────────────────────────────
        //  EXPORT / IMPORT
        // ─────────────────────────────────────────────────────────────────────
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
                    { "Row,Last Name,First Name,MI,ID Number,Status,Remarks" };
                foreach (var s in _allStudents)
                    lines.Add($"{s.RowNumber},{s.LastName},{s.FirstName},{s.MiddleInitial}," +
                              $"{s.IdNumber},{s.Status},{s.Remarks}");
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
                    if (!int.TryParse(parts[0], out int row)) continue;
                    var rec = _allStudents.FirstOrDefault(s => s.RowNumber == row);
                    if (rec == null) continue;
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
    }
}