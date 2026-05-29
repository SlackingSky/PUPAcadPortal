using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;

namespace PUPAcadPortal.PortalContents.Instructor.LMS
{
    //  Data model 
    public enum AttendanceStatus { Present, Absent, Excused }

    public class StudentAttendanceRecord
    {
        public string LastName { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string MiddleInitial { get; set; } = "";
        public string FullName =>
            string.IsNullOrWhiteSpace(MiddleInitial)
                ? $"{FirstName} {LastName}"
                : $"{FirstName} {MiddleInitial}. {LastName}";

        // Keep the old Name property so existing callers compile without edits
        public string Name
        {
            get => FullName;
            set
            {
                var parts = (value ?? "").Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) return;
                LastName = parts[^1];
                FirstName = parts[0];
                if (parts.Length >= 3)
                {
                    // Middle part: strip trailing dot if present
                    MiddleInitial = parts[1].TrimEnd('.');
                }
            }
        }

        public int RowNumber { get; set; }
        public string IdNumber { get; set; } = "";
        public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;
        public string Remarks { get; set; } = "";
    }

    //  Course / Session catalogue 

    public class CourseSection
    {
        public string Code { get; set; } = "";  // e.g. "IT 101"
        public string Title { get; set; } = "";  // e.g. "Introduction to Computing"
        public string Section { get; set; } = "";  // e.g. "BSIT 1-1"
        public string DisplayName => $"{Code} - {Title} ({Section})";
    }

    public class SessionSlot
    {
        public string Label { get; set; } = "";  
    }

    //  Attendance data per session 

    public class SessionKey
    {
        public string CourseDisplay { get; set; } = "";
        public string SessionLabel { get; set; } = "";
        public DateTime Date { get; set; }
        public override bool Equals(object? obj) =>
            obj is SessionKey k &&
            k.CourseDisplay == CourseDisplay &&
            k.SessionLabel == SessionLabel &&
            k.Date.Date == Date.Date;
        public override int GetHashCode() =>
            HashCode.Combine(CourseDisplay, SessionLabel, Date.Date);
    }

    //  Partial InstructorPortal 

    public partial class AttendanceContentInst : UserControl
    {
        //  State 
        private List<StudentAttendanceRecord> _allStudents = new();
        private List<StudentAttendanceRecord> _filteredStudents = new();
        private const int PageSize = 6;
        private int _currentPage = 1;
        private int _totalPages = 1;
        private DonutPanel? _donutPanel;
        private SessionAttendanceControl _sessionCard;
        private AttendanceGridControl _attendanceGrid;
        //  QR overlay 
        private QrCodeAttendanceControl? _qrCard;
        private Panel? _qrOverlay;
        private List<CourseSection> _courseCatalogue = new();
        private List<SessionSlot> _sessionSlots = new();
        private Dictionary<SessionKey, List<StudentAttendanceRecord>> _sessionSnapshots = new();

        //  ENTRY POINT 

        private void InitAttendance()
        {
            BuildCourseCatalogue();
            BuildSessionSlots();
            PopulateDropdowns();       
            SeedAllSessionSnapshots(); 
            LoadCurrentSession();
            _sessionCard = new SessionAttendanceControl { Dock = DockStyle.Fill };
            panel21.Controls.Clear();
            panel21.Controls.Add(_sessionCard);
            _attendanceGrid = new AttendanceGridControl { Dock = DockStyle.Fill };
            pnlGrid.Controls.Clear();
            pnlGrid.Controls.Add(_attendanceGrid);
            _attendanceGrid.AttendanceChanged += (s, e) =>
            {
                UpdateSummaryCards();
                UpdateLastUpdated();
            };

            _attendanceGrid.LoadStudents(_allStudents);

            btnQRCode.Click -= BtnGenerateQRCode_Click;
            btnQRCode.Click += BtnGenerateQRCode_Click;

            SetupAttendanceFilterBar_New();
            UpdateSummaryCards();
            UpdateLastUpdated();
        }

        //  CHANGE 3 – catalogue & seeding 
        private void BuildCourseCatalogue()
        {
            _courseCatalogue = new List<CourseSection>
            {
                new() { Code="IT 101",  Title="Introduction to Programming 1", Section="BSIT 1-1" },
                new() { Code="IT 101",  Title="Introduction to Programming 1", Section="BSIT 1-2" },
                new() { Code="IT 102",  Title="Introduction to Programming 2", Section="BSIT 1-1" },
                new() { Code="CS 201",  Title="Data Structures & Algorithms",  Section="BSCS 2-1" },
                new() { Code="CS 202",  Title="Object-Oriented Programming",   Section="BSCS 2-2" },
                new() { Code="IS 301",  Title="Database Management Systems",   Section="BSIS 3-1" },
                new() { Code="IS 302",  Title="Systems Analysis & Design",     Section="BSIS 3-2" },
                new() { Code="IT 401",  Title="Capstone Project 1",            Section="BSIT 4-1" },
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
            comboBox1.Items.Clear();
            foreach (var c in _courseCatalogue)
                comboBox1.Items.Add(c.DisplayName);
            if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;

            cmbSession.Items.Clear();
            foreach (var s in _sessionSlots)
                cmbSession.Items.Add(s.Label);
            if (cmbSession.Items.Count > 1) cmbSession.SelectedIndex = 1; 
        }

        private void SeedAllSessionSnapshots()
        {
            var today = DateTime.Today;
            var courses = _courseCatalogue.Take(4).ToList();
            var slots = _sessionSlots.ToList();

            var rng = new Random(42); 

            foreach (var course in courses)
            {
                foreach (var slot in slots)
                {
                    for (int dayOffset = -3; dayOffset <= 3; dayOffset++)
                    {
                        var date = today.AddDays(dayOffset);
                        var key = new SessionKey
                        {
                            CourseDisplay = course.DisplayName,
                            SessionLabel = slot.Label,
                            Date = date,
                        };
                        _sessionSnapshots[key] = BuildRosterForSession(course, rng);
                    }
                }
            }

            // Seed the default view (first course, second slot, today)
            LoadCurrentSession();
        }

        private static List<StudentAttendanceRecord> BuildRosterForSession(
            CourseSection course, Random rng)
        {
            // student names (last, first, MI)
            var namePool = new (string Last, string First, string MI)[]
            {
                ("Abad",       "Leonora",     "Y"), ("Aguilar",    "Dominador",   "S"),
                ("Aquino",     "Carlo",       "B"), ("Bautista",   "Fernando",    "K"),
                ("Basilan",    "Hans Louie",  "L"), ("Castillo",   "Ronnie",      "G"),
                ("Concepcion", "Salvador",    "F"), ("Cruz",       "Kevin James", "P"),
                ("Cruz",       "Nestor",      "O"), ("Dela Cruz",  "Alice",       "M"),
                ("Dela Torre", "Aurelio",     "Q"), ("Dizon",      "Orlando",     "B"),
                ("Domingo",    "Rosario",     "E"), ("Espinosa",   "Paz",         "C"),
                ("Fernandez",  "Hermogenes",  "U"), ("Flores",     "Joy",         "E"),
                ("Garcia",     "Ramon",       "F"), ("Gutierrez",  "Jacinta",     "W"),
                ("Hernandez",  "Felicitas",   "T"), ("Lopez",      "Ana",         "C"),
                ("Manalo",     "Kapitan",     "X"), ("Mendoza",    "Gloria",      "J"),
                ("Mercado",    "Quezon",      "D"), ("Navarro",    "Erlinda",     "P"),
                ("Ocampo",     "Mateo",       "Z"), ("Pascual",    "Natividad",   "A"),
                ("Perez",      "Consolacion", "R"), ("Peralta",    "Trinidad",    "G"),
                ("Ramos",      "Andres",      "M"), ("Reyes",      "Maria Angela","R"),
                ("Reyes",      "Ernesto",     "I"), ("Rivera",     "Mark",        "D"),
                ("Santiago",   "Consuelo",    "L"), ("Santos",     "John Doe",    "S"),
                ("Santos",     "Trisha",      "M"), ("Soberano",   "Liza",        "M"),
                ("Soriano",    "Isagani",     "V"), ("Torres",     "Patricia",    "H"),
                ("Villanueva", "Ericka Mae",  "T"), ("Villanueva", "Teresita",    "N"),
                ("Abella",     "Rosemarie",   "D"), ("Alcantara",  "Rogelio",     "F"),
                ("Antiporda",  "Lorena",      "B"), ("Bacungan",   "Manuel",      "A"),
                ("Baluyot",    "Cecilia",     "G"), ("Buenaventura","Ernesto",    "H"),
                ("Cabacungan", "Maricel",     "I"), ("Cabrera",    "Roberto",     "J"),
                ("Camacho",    "Violeta",     "K"), ("Castañeda",  "Alfredo",     "L"),
                ("Dayrit",     "Teodora",     "N"), ("Dela Peña",  "Carlos",      "O"),
                ("Enriquez",   "Cynthia",     "P"), ("Evangelista","Francisco",   "Q"),
                ("Ferrer",     "Lolita",      "R"), ("Galvez",     "Renato",      "S"),
                ("Gonzales",   "Imelda",      "T"), ("Ilagan",     "Bonifacio",   "U"),
                ("Lacson",     "Milagros",    "V"), ("Lagrimas",   "Arsenio",     "W"),
            };

            var picked = namePool.OrderBy(_ => rng.Next()).Take(45).ToList();

            var roster = new List<StudentAttendanceRecord>();
            int idBase = rng.Next(1000, 9999);

            for (int i = 0; i < picked.Count; i++)
            {
                var (last, first, mi) = picked[i];

                // Weighted status: ~78 % Present, ~12 % Absent, ~10 % Excused
                int roll = rng.Next(100);
                AttendanceStatus status;
                string remarks = "";
                if (roll < 78) { status = AttendanceStatus.Present; }
                else if (roll < 90) { status = AttendanceStatus.Absent; }
                else
                {
                    status = AttendanceStatus.Excused;
                    remarks = _excusedRemarks[rng.Next(_excusedRemarks.Length)];
                }

                roster.Add(new StudentAttendanceRecord
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

            return roster;
        }

        private static readonly string[] _excusedRemarks =
        {
            "Medical Certificate",
            "Family Emergency",
            "Hospital Admission",
            "Approved Leave of Absence",
            "OJT Duty",
            "University Event",
            "Athletic Competition",
            "Approved Field Trip",
        };

        private void LoadCurrentSession()
        {
            var key = CurrentSessionKey();
            if (_sessionSnapshots.TryGetValue(key, out var snapshot))
            {
                _allStudents = snapshot;
            }
            else
            {
                var rng = new Random(key.GetHashCode());
                var dummy = _courseCatalogue.FirstOrDefault() ?? new CourseSection();
                _allStudents = BuildRosterForSession(dummy, rng);
                _sessionSnapshots[key] = _allStudents;
            }
            _filteredStudents = new List<StudentAttendanceRecord>(_allStudents);
        }

        private SessionKey CurrentSessionKey()
        {
            string course = comboBox1.Text;
            string session = cmbSession.Text;
            DateTime date = dtpDate.Value.Date;
            return new SessionKey
            {
                CourseDisplay = course,
                SessionLabel = session,
                Date = date,
            };
        }

        //  QR Code overlay

        private void BtnGenerateQRCode_Click(object? sender, EventArgs e)
        {
            if (_qrOverlay != null && !_qrOverlay.IsDisposed && _qrOverlay.Visible)
            {
                _qrCard?.GenerateNew();
                _qrOverlay.BringToFront();
                return;
            }

            const int OVERLAY_W = 340;
            const int HEADER_H = 36;
            const int UC_H = 560;
            const int OVERLAY_H = HEADER_H + UC_H;

            _qrOverlay = new Panel
            {
                Size = new Size(OVERLAY_W, OVERLAY_H),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Padding = Padding.Empty,
            };

            var pnlHeader = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(OVERLAY_W, HEADER_H),
                BackColor = Color.FromArgb(106, 0, 0),
                Padding = Padding.Empty,
            };

            var lblHeader = new Label
            {
                Text = "QR Code Attendance",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Location = new Point(10, 0),
                Size = new Size(OVERLAY_W - 46, HEADER_H),
                TextAlign = ContentAlignment.MiddleLeft,
            };

            var btnClose = new Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(36, HEADER_H),
                Location = new Point(OVERLAY_W - 36, 0),
                Cursor = Cursors.Hand,
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(160, 20, 20);
            btnClose.Click += (s2, e2) => _qrOverlay.Visible = false;

            pnlHeader.Controls.Add(lblHeader);
            pnlHeader.Controls.Add(btnClose);

            _qrCard = new QrCodeAttendanceControl
            {
                Location = new Point(0, HEADER_H),
                Size = new Size(OVERLAY_W, UC_H),
                Anchor = AnchorStyles.Top | AnchorStyles.Left |
                                  AnchorStyles.Right | AnchorStyles.Bottom,
                Session = cmbSession.Text,
                AttendanceDate = dtpDate.Value,
            };

            cmbSession.SelectedIndexChanged += (s2, e2) =>
            { if (_qrCard != null) _qrCard.Session = cmbSession.Text; };
            dtpDate.ValueChanged += (s2, e2) =>
            { if (_qrCard != null) _qrCard.AttendanceDate = dtpDate.Value; };

            _qrCard.QrExpired += (s2, e2) =>
            {
                lblHeader.Text = "QR Code — EXPIRED";
                lblHeader.ForeColor = Color.FromArgb(255, 200, 200);
            };

            _qrCard.GenerateNew();
            lblHeader.Text = "QR Code Attendance";
            lblHeader.ForeColor = Color.White;

            _qrOverlay.Controls.Add(_qrCard);
            _qrOverlay.Controls.Add(pnlHeader);

            Control overlayParent = pnlAttendance;
            _qrOverlay.Location = new Point(
                (overlayParent.Width - OVERLAY_W) / 2,
                (overlayParent.Height - OVERLAY_H) / 2);
            overlayParent.Controls.Add(_qrOverlay);
            _qrOverlay.BringToFront();

            _qrCard.GenerateNew();
        }

        //  Legacy seed (kept so the file compiles if SeedStudentData is called elsewhere) 

       

        //  Filter bar, search, save, export, import 

        private void SetupAttendanceFilterBar_New()
        {
            comboBox1.SelectedIndexChanged += (s, e) =>
            {
                LoadCurrentSession();
                _attendanceGrid.LoadStudents(_allStudents);
                UpdateSummaryCards();
            };

            cmbSession.SelectedIndexChanged += (s, e) =>
            {
                LoadCurrentSession();
                _attendanceGrid.LoadStudents(_allStudents);
                UpdateSummaryCards();
            };

            dtpDate.ValueChanged += (s, e) =>
            {
                LoadCurrentSession();
                _attendanceGrid.LoadStudents(_allStudents);
                UpdateLastUpdated();
                UpdateSummaryCards();
            };

            // Search box
            textBox2.ForeColor = Color.Gray;
            textBox2.GotFocus += (s, e) =>
            {
                if (textBox2.Text == "Search student...")
                { textBox2.Text = ""; textBox2.ForeColor = Color.Black; }
            };
            textBox2.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox2.Text))
                { textBox2.Text = "Search student..."; textBox2.ForeColor = Color.Gray; }
            };
            textBox2.TextChanged += (s, e) =>
            {
                string q = textBox2.Text.Trim();
                if (q == "Search student...") q = "";
                _attendanceGrid.ApplyFilter(q);
            };

            btnSaveAttendance.Click -= BtnSaveAttendance_Click;
            btnSaveAttendance.Click += BtnSaveAttendance_Click;

            btnRefresh.Click += (s, e) =>
            {
                LoadCurrentSession();
                _attendanceGrid.LoadStudents(_allStudents);
                UpdateSummaryCards();
                UpdateLastUpdated();
                MessageBox.Show("Attendance list refreshed.", "Refresh",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnExport.Click += (s, e) => ExportAttendanceToCSV();
            btnImporttoCSV.Click += (s, e) => ImportAttendanceFromCSV();
        }

        private void RefreshAttendanceView()
        {
            _totalPages = Math.Max(1, (int)Math.Ceiling(_filteredStudents.Count / (double)PageSize));
            if (_currentPage < 1) _currentPage = 1;
            if (_currentPage > _totalPages) _currentPage = _totalPages;

            var page = _filteredStudents
                .Skip((_currentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            dgvAttendance.SuspendLayout();
            dgvAttendance.Rows.Clear();

            foreach (var s in page)
            {
                int idx = dgvAttendance.Rows.Add(
                    s.LastName, s.FirstName, s.MiddleInitial,
                    s.IdNumber, s.Status.ToString(),
                    string.IsNullOrWhiteSpace(s.Remarks) ? "Optional remarks" : s.Remarks);
                dgvAttendance.Rows[idx].Tag = s;
            }

            dgvAttendance.ResumeLayout();
            UpdatePaginationUI();
            UpdateShowingLabel();
            UpdateSummaryCards();
        }

        //  Summary cards 

        private void UpdateSummaryCards()
        {
            int total = _allStudents.Count;
            int present = _allStudents.Count(x => x.Status == AttendanceStatus.Present);
            int absent = _allStudents.Count(x => x.Status == AttendanceStatus.Absent);
            int excused = _allStudents.Count(x => x.Status == AttendanceStatus.Excused);

            lblPresentNum.Text = present.ToString();
            lblAbsentNum.Text = absent.ToString();
            lblExcusedNum.Text = excused.ToString();
            lblPesentPercent.Text = total > 0 ? $"{present * 100.0 / total:F2}%" : "0%";
            lblAbsentPercent.Text = total > 0 ? $"{absent * 100.0 / total:F2}%" : "0%";
            lblExcusedPrecent.Text = total > 0 ? $"{excused * 100.0 / total:F2}%" : "0%";

            _sessionCard?.SetData(present, absent, excused);
        }

        //  Pagination UI (legacy dgvAttendance) 

        private void UpdatePaginationUI()
        {
            int[] visible = GetVisiblePages();
            var pageButtons = new[] { btnPage1, btnPage2, btnPage3 };

            for (int i = 0; i < pageButtons.Length; i++)
            {
                if (i < visible.Length)
                {
                    pageButtons[i].Text = visible[i].ToString();
                    pageButtons[i].Visible = true;
                    bool isActive = visible[i] == _currentPage;
                    pageButtons[i].BackColor = isActive ? Color.FromArgb(128, 0, 0) : Color.White;
                    pageButtons[i].ForeColor = isActive ? Color.White : Color.Black;
                    pageButtons[i].Font = new Font("Segoe UI", 9f,
                        isActive ? FontStyle.Bold : FontStyle.Regular);
                }
                else pageButtons[i].Visible = false;
            }

            btnPagePrev.Enabled = _currentPage > 1;
            btnPagePrevDbl.Enabled = _currentPage > 1;
            btnPageNext.Enabled = _currentPage < _totalPages;
            btnPageNextDbl.Enabled = _currentPage < _totalPages;
        }

        private int[] GetVisiblePages()
        {
            if (_totalPages <= 3) return Enumerable.Range(1, _totalPages).ToArray();
            if (_currentPage == 1) return new[] { 1, 2, 3 };
            if (_currentPage == _totalPages) return new[] { _totalPages - 2, _totalPages - 1, _totalPages };
            return new[] { _currentPage - 1, _currentPage, _currentPage + 1 };
        }

        private void UpdateShowingLabel()
        {
            int start = (_currentPage - 1) * PageSize + 1;
            int end = Math.Min(_currentPage * PageSize, _filteredStudents.Count);
            int tot = _filteredStudents.Count;
            lblShowing.Text = tot == 0
                ? "No students found"
                : $"Showing {start} to {end} of {tot} students";
        }


        private void UpdateLastUpdated()
        {
            lblDateTime.Text = DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt");
        }

        //  Legacy DataGridView events 

        private void DgvAttendance_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
        {
            if (dgvAttendance.IsCurrentCellDirty &&
                dgvAttendance.CurrentCell is DataGridViewComboBoxCell)
                dgvAttendance.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DgvAttendance_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvAttendance.Rows[e.RowIndex];
            if (row.Tag is not StudentAttendanceRecord rec) return;

            if (e.ColumnIndex == dgvAttendance.Columns["colStatus"]?.Index)
            {
                string val = row.Cells["colStatus"].Value?.ToString() ?? "Present";
                rec.Status = val switch
                {
                    "Absent" => AttendanceStatus.Absent,
                    "Excused" => AttendanceStatus.Excused,
                    _ => AttendanceStatus.Present,
                };
                dgvAttendance.InvalidateRow(e.RowIndex);
                UpdateSummaryCards();
                UpdateLastUpdated();
            }

            if (e.ColumnIndex == dgvAttendance.Columns["colRemarks"]?.Index)
            {
                string txt = row.Cells["colRemarks"].Value?.ToString() ?? "";
                if (txt != "Optional remarks") rec.Remarks = txt;
            }
        }

        private void DgvAttendance_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvAttendance.Rows[e.RowIndex];
            if (row.Tag is not StudentAttendanceRecord rec) return;

            Color bg = rec.Status switch
            {
                AttendanceStatus.Absent => Color.FromArgb(255, 245, 245),
                AttendanceStatus.Excused => Color.FromArgb(255, 253, 235),
                _ => Color.White,
            };
            if (!row.Selected)
            {
                row.DefaultCellStyle.BackColor = bg;
                row.DefaultCellStyle.SelectionBackColor = bg;
            }

            int statusIdx = dgvAttendance.Columns["colStatus"]?.Index ?? -1;
            if (e.ColumnIndex == statusIdx && e.Value != null)
            {
                e.CellStyle.ForeColor = e.Value.ToString() switch
                {
                    "Absent" => Color.Firebrick,
                    "Excused" => Color.DarkGoldenrod,
                    _ => Color.FromArgb(0, 140, 0),
                };
                e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            }

            int remarksIdx = dgvAttendance.Columns["colRemarks"]?.Index ?? -1;
            if (e.ColumnIndex == remarksIdx && e.Value?.ToString() == "Optional remarks")
                e.CellStyle.ForeColor = Color.LightGray;
        }

        private void DgvAttendance_EditingControlShowing(object? sender,
            DataGridViewEditingControlShowingEventArgs e)
        {
            int remarksIdx = dgvAttendance.Columns["colRemarks"]?.Index ?? -1;
            if (dgvAttendance.CurrentCell?.ColumnIndex == remarksIdx && e.Control is TextBox tb)
                if (tb.Text == "Optional remarks") { tb.Text = ""; tb.ForeColor = Color.Black; }
        }

        //  Save 

        private void BtnSaveAttendance_Click(object? sender, EventArgs e)
        {
            int present = _allStudents.Count(x => x.Status == AttendanceStatus.Present);
            int absent = _allStudents.Count(x => x.Status == AttendanceStatus.Absent);
            int excused = _allStudents.Count(x => x.Status == AttendanceStatus.Excused);

            UpdateLastUpdated();
            UpdateSummaryCards();

            MessageBox.Show(
                $"Attendance saved successfully!\n\n" +
                $"Course  : {comboBox1.Text}\n" +
                $"Date    : {dtpDate.Value:MMMM dd, yyyy}\n" +
                $"Session : {cmbSession.Text}\n\n" +
                $"Present : {present}\n" +
                $"Absent  : {absent}\n" +
                $"Excused : {excused}\n" +
                $"Total   : {_allStudents.Count}",
                "Attendance Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        //  Export 

        private void ExportAttendanceToCSV()
        {
            using var sfd = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = $"Attendance_{dtpDate.Value:yyyyMMdd}.csv"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                var lines = new List<string> { "Row,Last Name,First Name,MI,ID Number,Status,Remarks" };
                foreach (var s in _allStudents)
                    lines.Add($"{s.RowNumber},{s.LastName},{s.FirstName},{s.MiddleInitial}," +
                              $"{s.IdNumber},{s.Status},{s.Remarks}");

                System.IO.File.WriteAllLines(sfd.FileName, lines);
                MessageBox.Show("Exported successfully!", "Export",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Export failed: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //  Import 
        private void ImportAttendanceFromCSV()
        {
            using var ofd = new OpenFileDialog { Filter = "CSV files (*.csv)|*.csv" };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            try
            {
                var lines = System.IO.File.ReadAllLines(ofd.FileName);
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
                        "Excused" => AttendanceStatus.Excused,
                        _ => AttendanceStatus.Present,
                    };
                    if (parts.Length > 6) rec.Remarks = parts[6].Trim();
                    imported++;
                }

                _attendanceGrid.LoadStudents(_allStudents);
                _currentPage = 1;
                RefreshAttendanceView();
                UpdateLastUpdated();
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

    //  DonutPanel 

    public class DonutPanel : Panel
    {
        private float _presentPct = 0.93f;
        private float _absentPct = 0.05f;
        private float _excusedPct = 0.02f;
        private string _centerText = "93%";
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float PresentPct { get => _presentPct; set { _presentPct = value; Invalidate(); } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float AbsentPct { get => _absentPct; set { _absentPct = value; Invalidate(); } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float ExcusedPct { get => _excusedPct; set { _excusedPct = value; Invalidate(); } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CenterText { get => _centerText; set { _centerText = value; Invalidate(); } }
        private static readonly Color PresentColor = Color.FromArgb(52, 168, 83);
        private static readonly Color AbsentColor = Color.FromArgb(220, 53, 69);
        private static readonly Color ExcusedColor = Color.FromArgb(255, 193, 7);
        public DonutPanel() { DoubleBuffered = true; ResizeRedraw = true; }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int margin = 4;
            var rect = new Rectangle(margin, margin, Width - margin * 2 - 1, Height - margin * 2 - 1);
            int thick = (int)(rect.Width * 0.22f);

            using var bgPen = new Pen(Color.FromArgb(230, 230, 230), thick);
            g.DrawEllipse(bgPen, rect);

            DrawArc(g, rect, thick, -90f, PresentPct * 360f, PresentColor);
            DrawArc(g, rect, thick, -90f + PresentPct * 360f, AbsentPct * 360f, AbsentColor);
            DrawArc(g, rect, thick, -90f + (PresentPct + AbsentPct) * 360f, ExcusedPct * 360f, ExcusedColor);

            var font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            var sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
            };
            g.DrawString(CenterText, font, Brushes.Black,
                new RectangleF(0, 0, Width, Height), sf);
        }

        private static void DrawArc(Graphics g, Rectangle rect, int thickness,
            float startAngle, float sweepAngle, Color color)
        {
            if (sweepAngle <= 0) return;
            using var pen = new Pen(color, thickness)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round,
            };
            g.DrawArc(pen, rect, startAngle, sweepAngle);
        }
    }

    //  ManualEntryDialog 

    public class ManualEntryDialog : Form
    {
        private string _selectedStatus = "Present";
        public string SelectedStatus { get => _selectedStatus; private set => _selectedStatus = value; }
        private ComboBox _combo;

        public ManualEntryDialog(string studentName, string currentStatus)
        {
            Text = "Manual Entry";
            Size = new Size(320, 160);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            var lbl = new Label
            {
                Text = $"Student: {studentName}",
                Location = new Point(16, 16),
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
            };

            _combo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(16, 44),
                Width = 270,
                Font = new Font("Segoe UI", 10f),
            };
            _combo.Items.AddRange(new object[] { "Present", "Absent", "Excused" });
            _combo.Text = currentStatus;

            var btnOk = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new Point(130, 82),
                Size = new Size(75, 28),
                BackColor = Color.FromArgb(128, 0, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
            };
            btnOk.FlatAppearance.BorderSize = 0;

            var btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new Point(214, 82),
                Size = new Size(75, 28),
            };

            btnOk.Click += (s, e) => SelectedStatus = _combo.Text;
            Controls.AddRange(new Control[] { lbl, _combo, btnOk, btnCancel });
            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }
    }
}