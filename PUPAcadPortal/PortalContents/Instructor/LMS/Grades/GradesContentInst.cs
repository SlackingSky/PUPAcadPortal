using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PUPAcadPortal.Dialogs;

namespace PUPAcadPortal.PortalContents.Instructor.LMS
{
    public partial class GradesContentInst : UserControl
    {
        private bool _gradesLoaded = false;
        private List<StudentGradeRecord> _gradeRecords = new();
        private List<StudentGradeRecord> _filteredRecords = new();
        private bool _showingMidterm = true;
        private readonly List<CustomGradeColumn> _customMTColumns = new();
        private readonly List<CustomGradeColumn> _customFTColumns = new();
        private static readonly Color Maroon = Color.FromArgb(106, 0, 0);
        private static readonly Color MaroonDark = Color.FromArgb(80, 0, 0);
        private static readonly Color MaroonLight = Color.FromArgb(240, 220, 220);
        private static readonly Color GreenPass = Color.FromArgb(16, 124, 65);
        private static readonly Color RedFail = Color.Firebrick;
        private static readonly Color BlueGood = Color.FromArgb(0, 112, 192);
        private static readonly Color AmberWarn = Color.FromArgb(180, 120, 0);
        private static readonly Color Surface = Color.White;
        private static readonly Color Background = Color.FromArgb(248, 246, 246);
        private static readonly Color BorderLight = Color.FromArgb(220, 210, 210);
        private static readonly Color TextPrimary = Color.FromArgb(30, 30, 30);
        private static readonly Color TextSecondary = Color.FromArgb(100, 100, 100);
        private static readonly Color PendingOrange = Color.FromArgb(200, 100, 0);
        public static GradeWeights CurrentWeights = new();

        public class GradeWeights
        {
            public double AttendancePct { get; set; } = 10;
            public double RecitationPct { get; set; } = 10;
            public double SeatworkPct { get; set; } = 15;
            public double AssignmentPct { get; set; } = 10;
            public double LongTestsPct { get; set; } = 25;
            public double ClassStandingPct =>
                AttendancePct + RecitationPct + SeatworkPct + AssignmentPct + LongTestsPct;
            public double MajorExamsPct { get; set; } = 30;
        }

        public class StudentGradeRecord
        {
            public string StudentID { get; set; } = "";
            public string Name { get; set; } = "";
            public string Course { get; set; } = "";
            public string Status { get; set; } = "Pending";
            public bool Released { get; set; } = false;
            public double? MT_Attendance { get; set; }
            public double? MT_Recitation { get; set; }
            public double? MT_Seatwork { get; set; }
            public double? MT_Assignment { get; set; }
            public double? MT_LongTests { get; set; }
            public double? MT_MajorExam { get; set; }

            public double? FT_Attendance { get; set; }
            public double? FT_Recitation { get; set; }
            public double? FT_Seatwork { get; set; }
            public double? FT_Assignment { get; set; }
            public double? FT_LongTests { get; set; }
            public double? FT_MajorExam { get; set; }

            public double? MidtermGrade
            {
                get
                {
                    var w = CurrentWeights;
                    if (!MT_Attendance.HasValue || !MT_Recitation.HasValue ||
                        !MT_Seatwork.HasValue || !MT_Assignment.HasValue ||
                        !MT_LongTests.HasValue || !MT_MajorExam.HasValue) return null;

                    double cs = MT_Attendance.Value * (w.AttendancePct / 100.0)
                              + MT_Recitation.Value * (w.RecitationPct / 100.0)
                              + MT_Seatwork.Value * (w.SeatworkPct / 100.0)
                              + MT_Assignment.Value * (w.AssignmentPct / 100.0)
                              + MT_LongTests.Value * (w.LongTestsPct / 100.0);
                    return Math.Round(cs + MT_MajorExam.Value * (w.MajorExamsPct / 100.0), 2);
                }
            }

            public double? FinalTermGrade
            {
                get
                {
                    var w = CurrentWeights;
                    if (!FT_Attendance.HasValue || !FT_Recitation.HasValue ||
                        !FT_Seatwork.HasValue || !FT_Assignment.HasValue ||
                        !FT_LongTests.HasValue || !FT_MajorExam.HasValue) return null;

                    double cs = FT_Attendance.Value * (w.AttendancePct / 100.0)
                              + FT_Recitation.Value * (w.RecitationPct / 100.0)
                              + FT_Seatwork.Value * (w.SeatworkPct / 100.0)
                              + FT_Assignment.Value * (w.AssignmentPct / 100.0)
                              + FT_LongTests.Value * (w.LongTestsPct / 100.0);
                    return Math.Round(cs + FT_MajorExam.Value * (w.MajorExamsPct / 100.0), 2);
                }
            }

            public double? FinalGrade =>
                (MidtermGrade.HasValue && FinalTermGrade.HasValue)
                    ? Math.Round((MidtermGrade.Value + FinalTermGrade.Value) / 2.0, 2)
                    : null;

            public string GradeEquivalent
            {
                get
                {
                    if (!FinalGrade.HasValue) return "INC";
                    double s = FinalGrade.Value;
                    if (s >= 97) return "1.00";
                    if (s >= 94) return "1.25";
                    if (s >= 91) return "1.50";
                    if (s >= 88) return "1.75";
                    if (s >= 85) return "2.00";
                    if (s >= 82) return "2.25";
                    if (s >= 79) return "2.50";
                    if (s >= 76) return "2.75";
                    if (s >= 75) return "3.00";
                    return "5.00";
                }
            }

            public string GradeDescription
            {
                get
                {
                    if (!FinalGrade.HasValue) return "Incomplete";
                    double s = FinalGrade.Value;
                    if (s >= 91) return "Excellent / Very Good";
                    if (s >= 85) return "Good";
                    if (s >= 82) return "Good";
                    if (s >= 79) return "Satisfactory";
                    if (s >= 75) return "Passing";
                    return "Failed";
                }
            }

            public string TermStatusLabel(bool midterm)
            {
                double? tg = midterm ? MidtermGrade : FinalTermGrade;
                if (!tg.HasValue) return "INC";
                return tg.Value >= 75 ? "Passed" : "Failed";
            }
        }

        public class CustomGradeColumn
        {
            public string ColName { get; set; } = "";
            public string Label { get; set; } = "";
            public string Category { get; set; } = "";
            public bool IsMidterm { get; set; } = true;
            public Dictionary<string, double?> Scores { get; } = new();
        }

        public GradesContentInst()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
        }

        private void GradesContentInst_Load(object sender, EventArgs e)
        {
            ApplyDoubleBuffer(this);
            SetupComboBoxes();
            SetupGrid();
            SetupFinalTermGrid();
            SetupGradingScaleGrid();
            SetupLegend();
            WireEvents();

            BeginInvoke(new Action(LoadGradeData));
        }

        private static void ApplyDoubleBuffer(Control root)
        {
            foreach (Control c in root.Controls)
            {
                try
                {
                    typeof(Control)
                        .GetProperty("DoubleBuffered",
                            System.Reflection.BindingFlags.NonPublic |
                            System.Reflection.BindingFlags.Instance)
                        ?.SetValue(c, true, null);
                }
                catch { }
                ApplyDoubleBuffer(c);
            }
        }

        private void SetupComboBoxes()
        {
            cmbCourseSection.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCourseSection.Items.Clear();
            cmbCourseSection.Items.Add("All Sections");
            cmbCourseSection.Items.Add("IT 101 – Intro to Computing  |  BSIT 2-1");
            cmbCourseSection.Items.Add("CS 102 – Data Structures  |  BSCS 2-2");
            cmbCourseSection.Items.Add("IS 103 – Database Mgmt  |  BSIS 3-1");
            cmbCourseSection.SelectedIndex = 0;
            cmbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatusFilter.Items.Clear();
            cmbStatusFilter.Items.Add("All Status");
            cmbStatusFilter.Items.Add("Submitted");
            cmbStatusFilter.Items.Add("Pending");
            cmbStatusFilter.SelectedIndex = 0;
        }

        private void SetupGrid()
        {
            gridStudents.Columns.Clear();
            StyleGrid(gridStudents);

            void Col(string name, string header, int fw, bool editable,
                DataGridViewContentAlignment align = DataGridViewContentAlignment.MiddleCenter)
            {
                gridStudents.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = name,
                    HeaderText = header,
                    ReadOnly = !editable,
                    MinimumWidth = 50,
                    FillWeight = fw,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    DefaultCellStyle = { Alignment = align },
                    HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } }
                });
            }

            Col("colStudentID", "Student ID", 95, false, DataGridViewContentAlignment.MiddleLeft);
            Col("colStudentName", "Name", 180, false, DataGridViewContentAlignment.MiddleLeft);
            Col("colAttendance", $"Attendance\n{CurrentWeights.AttendancePct}%", 55, true);
            Col("colRecitation", $"Recitation\n{CurrentWeights.RecitationPct}%", 55, true);
            Col("colSeatwork", $"Seatwork\n{CurrentWeights.SeatworkPct}%", 55, true);
            Col("colAssignment", $"Assignment\n{CurrentWeights.AssignmentPct}%", 60, true);
            Col("colLongTests", $"Long Tests\n{CurrentWeights.LongTestsPct}%", 60, true);
            Col("colMajorExam", $"Major Exam\n{CurrentWeights.MajorExamsPct}%", 65, true);
            Col("colTermGrade", "Term Grade", 70, false);
            Col("colFinalGrade", "Final Grade", 70, false);
            Col("colStatus", "Status", 75, false);
            Col("colRemarks", "Equiv.", 58, false);
        }

        private void SetupFinalTermGrid()
        {
            dataGridView1.Columns.Clear();
            StyleGrid(dataGridView1);

            void Col(string name, string header, int fw, bool editable,
                DataGridViewContentAlignment align = DataGridViewContentAlignment.MiddleCenter)
            {
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = name,
                    HeaderText = header,
                    ReadOnly = !editable,
                    MinimumWidth = 50,
                    FillWeight = fw,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    DefaultCellStyle = { Alignment = align },
                    HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } }
                });
            }

            Col("ftColStudentID", "Student ID", 95, false, DataGridViewContentAlignment.MiddleLeft);
            Col("ftColStudentName", "Name", 180, false, DataGridViewContentAlignment.MiddleLeft);
            Col("ftColAttendance", $"Attendance\n{CurrentWeights.AttendancePct}%", 55, true);
            Col("ftColRecitation", $"Recitation\n{CurrentWeights.RecitationPct}%", 55, true);
            Col("ftColSeatwork", $"Seatwork\n{CurrentWeights.SeatworkPct}%", 55, true);
            Col("ftColAssignment", $"Assignment\n{CurrentWeights.AssignmentPct}%", 60, true);
            Col("ftColLongTests", $"Long Tests\n{CurrentWeights.LongTestsPct}%", 60, true);
            Col("ftColMajorExam", $"Major Exam\n{CurrentWeights.MajorExamsPct}%", 65, true);
            Col("ftColTermGrade", "Term Grade", 70, false);
            Col("ftColFinalGrade", "Final Grade", 70, false);
            Col("ftColStatus", "Status", 75, false);
            Col("ftColRemarks", "Equiv.", 58, false);
        }

        private static void StyleGrid(DataGridView dgv)
        {
            dgv.AutoGenerateColumns = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgv.MultiSelect = false;
            dgv.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersHeight = 48;
            dgv.BackgroundColor = Surface;
            dgv.BorderStyle = BorderStyle.None;
            dgv.GridColor = Color.FromArgb(235, 228, 228);
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.Dock = DockStyle.Fill;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Maroon;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Maroon;
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgv.DefaultCellStyle.ForeColor = TextPrimary;
            dgv.DefaultCellStyle.BackColor = Surface;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 235, 235);
            dgv.DefaultCellStyle.SelectionForeColor = TextPrimary;
            dgv.DefaultCellStyle.Padding = new Padding(4, 0, 4, 0);

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 248, 248);
            dgv.RowTemplate.Height = 32;
        }

        private void SetupGradingScaleGrid()
        {
            if (gridGradingScale == null) return;

            gridGradingScale.Rows.Clear();
            gridGradingScale.AllowUserToAddRows = false;
            gridGradingScale.RowHeadersVisible = false;
            gridGradingScale.BackgroundColor = Surface;
            gridGradingScale.BorderStyle = BorderStyle.None;
            gridGradingScale.CellBorderStyle = DataGridViewCellBorderStyle.None;
            gridGradingScale.GridColor = BorderLight;
            gridGradingScale.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridGradingScale.RowTemplate.Height = 24;
            gridGradingScale.ColumnHeadersHeight = 28;
            gridGradingScale.EnableHeadersVisualStyles = false;
            gridGradingScale.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(230, 215, 215);
            gridGradingScale.ColumnHeadersDefaultCellStyle.ForeColor = Maroon;
            gridGradingScale.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            gridGradingScale.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 235, 235);
            gridGradingScale.DefaultCellStyle.SelectionForeColor = TextPrimary;

            var rows = new[]
            {
                ("97–100","1.00","Excellent",      GreenPass, Color.White),
                ("94–96", "1.25","Excellent",      GreenPass, Color.White),
                ("91–93", "1.50","Very Good",      GreenPass, Color.White),
                ("88–90", "1.75","Very Good",      BlueGood,  Color.White),
                ("85–87", "2.00","Good",           BlueGood,  Color.White),
                ("82–84", "2.25","Good",           BlueGood,  Color.White),
                ("79–81", "2.50","Satisfactory",   AmberWarn, Color.White),
                ("76–78", "2.75","Satisfactory",   AmberWarn, Color.White),
                ("75",    "3.00","Passing",        AmberWarn, Color.White),
                ("<75",   "5.00","Failed",         RedFail,   Color.White),
            };

            foreach (var (range, equiv, desc, fore, back) in rows)
            {
                int i = gridGradingScale.Rows.Add(range, equiv, desc);
                var row = gridGradingScale.Rows[i];
                row.DefaultCellStyle.ForeColor = fore;
                row.DefaultCellStyle.BackColor = back;
                row.DefaultCellStyle.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                row.DefaultCellStyle.SelectionForeColor = fore;
                row.DefaultCellStyle.SelectionBackColor = back;
            }
        }

        private void SetupLegend()
        {
            if (pnlLegendItems == null) return;
            pnlLegendItems.Controls.Clear();

            var entries = new (Color dot, string text)[]
            {
                (GreenPass,  "1.00 – 1.50  Excellent / Very Good"),
                (BlueGood,   "1.75 – 2.25  Good"),
                (AmberWarn,  "2.50 – 3.00  Satisfactory / Passing"),
                (RedFail,    "5.00  Failed"),
                (Color.Gray, "INC   Incomplete"),
                (GreenPass,  "Passed"),
                (PendingOrange, "Pending"),
            };

            int y = 2;
            foreach (var (dot, text) in entries)
            {
                pnlLegendItems.Controls.Add(new Panel
                {
                    Width = 12,
                    Height = 12,
                    BackColor = dot,
                    Location = new Point(4, y + 3),
                });
                pnlLegendItems.Controls.Add(new Label
                {
                    Text = text,
                    Location = new Point(22, y),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = Color.FromArgb(45, 45, 45),
                });
                y += 22;
            }
        }

        private void WireEvents()
        {
            cmbCourseSection.SelectedIndexChanged += (s, e) => ApplyFilters();
            cmbStatusFilter.SelectedIndexChanged += (s, e) => ApplyFilters();
            txtSearch.TextChanged += (s, e) => ApplyFilters();
            btnReleaseGrades.Click += BtnReleaseGrades_Click;
            btnImportCSV.Click += BtnImportCSV_Click;
            btnEditWeights.Click += (s, e) => ShowEditGradePercentageDialog();
            tabTerms.SelectedIndexChanged += (s, e) =>
            {
                _showingMidterm = (tabTerms.SelectedIndex == 0);
                if (_showingMidterm) RefreshMidtermGrid();
                else RefreshFinalTermGrid();
            };

            gridStudents.CellValueChanged += GridStudents_CellValueChanged;
            gridStudents.EditingControlShowing += Grid_EditingControlShowing;
            gridStudents.DataError += (s, e) => e.Cancel = true;
            gridStudents.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                var id = gridStudents.Rows[e.RowIndex].Cells["colStudentID"].Value?.ToString();
                var rec = _gradeRecords.FirstOrDefault(r => r.StudentID == id);
                if (rec != null) ShowStudentDetailPanel(rec);
            };
            gridStudents.CellEnter += (s, e) => HighlightEditableCell(gridStudents, e,
                new[] { "colAttendance", "colRecitation", "colSeatwork", "colAssignment", "colLongTests", "colMajorExam" });
            dataGridView1.CellValueChanged += FinalTermGrid_CellValueChanged;
            dataGridView1.EditingControlShowing += Grid_EditingControlShowing;
            dataGridView1.DataError += (s, e) => e.Cancel = true;
            dataGridView1.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                var id = dataGridView1.Rows[e.RowIndex].Cells["ftColStudentID"].Value?.ToString();
                var rec = _gradeRecords.FirstOrDefault(r => r.StudentID == id);
                if (rec != null) ShowStudentDetailPanel(rec);
            };
            dataGridView1.CellEnter += (s, e) => HighlightEditableCell(dataGridView1, e,
                new[] { "ftColAttendance", "ftColRecitation", "ftColSeatwork", "ftColAssignment", "ftColLongTests", "ftColMajorExam" });
        }

        private static void HighlightEditableCell(DataGridView dgv, DataGridViewCellEventArgs e, string[] editableCols)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            string colName = dgv.Columns[e.ColumnIndex].Name;
            bool isEditable = editableCols.Contains(colName);
            var cell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
            cell.Style.SelectionBackColor = isEditable
                ? Color.FromArgb(200, 220, 255)
                : Color.FromArgb(245, 235, 235);
            cell.Style.SelectionForeColor = TextPrimary;
        }

        private void Grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox tb)
            {
                tb.KeyPress -= GradeCell_KeyPress;
                tb.TextChanged -= GradeCell_TextChanged;
                tb.KeyPress += GradeCell_KeyPress;
                tb.TextChanged += GradeCell_TextChanged;
            }
        }

        private static void GradeCell_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '\b')
                e.Handled = true;
        }

        private static void GradeCell_TextChanged(object sender, EventArgs e)
        {
            if (sender is not TextBox tb) return;
            if (double.TryParse(tb.Text, out double v) && v > 100)
            {
                tb.Text = "100";
                tb.SelectionStart = tb.Text.Length;
            }
        }
        private static List<StudentGradeRecord> BuildSeedData() => new()
        {
            new() { StudentID="2024-00001-SM-0", Name="Ablong, Adrian P.",       Course="IT 101",
                MT_Attendance=90,MT_Recitation=85,MT_Seatwork=88,MT_Assignment=87,MT_LongTests=84,MT_MajorExam=86,
                FT_Attendance=88,FT_Recitation=90,FT_Seatwork=85,FT_Assignment=89,FT_LongTests=88,FT_MajorExam=90, Status="Submitted" },
            new() { StudentID="2024-00002-SM-0", Name="Alcaiz, Jared B.",        Course="IT 101",
                MT_Attendance=78,MT_Recitation=72,MT_Seatwork=74,MT_Assignment=70,MT_LongTests=73,MT_MajorExam=75,
                FT_Attendance=80,FT_Recitation=75,FT_Seatwork=76,FT_Assignment=72,FT_LongTests=78,FT_MajorExam=74, Status="Submitted" },
            new() { StudentID="2024-00003-SM-0", Name="Amar, Charls Manuel C.",  Course="IT 101",
                MT_Attendance=95,MT_Recitation=93,MT_Seatwork=94,MT_Assignment=92,MT_LongTests=95,MT_MajorExam=93,
                FT_Attendance=96,FT_Recitation=95,FT_Seatwork=93,FT_Assignment=94,FT_LongTests=96,FT_MajorExam=94, Status="Submitted" },
            new() { StudentID="2024-00004-SM-0", Name="Amen, Jessie C.",         Course="IT 101",
                MT_Attendance=60,MT_Recitation=58,MT_Seatwork=62,MT_Assignment=55,MT_LongTests=60,MT_MajorExam=57,
                FT_Attendance=62,FT_Recitation=60,FT_Seatwork=63,FT_Assignment=58,FT_LongTests=61,FT_MajorExam=59, Status="Submitted" },
            new() { StudentID="2024-00005-SM-0", Name="Amolata, Jhayphee V.",    Course="IT 101",
                MT_Attendance=82,MT_Recitation=80,MT_Seatwork=83,MT_Assignment=79,MT_LongTests=81,MT_MajorExam=82,
                FT_Attendance=84,FT_Recitation=82,FT_Seatwork=80,FT_Assignment=83,FT_LongTests=83,FT_MajorExam=81, Status="Submitted" },
            new() { StudentID="2024-00006-SM-0", Name="Antillon, Reijn C.",      Course="IT 101",
                MT_Attendance=70,MT_Recitation=68,MT_Seatwork=71,MT_Assignment=67,MT_LongTests=69,MT_MajorExam=70,
                FT_Attendance=72,FT_Recitation=70,FT_Seatwork=69,FT_Assignment=71,FT_LongTests=70,FT_MajorExam=68, Status="Submitted" },
            new() { StudentID="2024-00007-SM-0", Name="Armada, Trisha Mariel D.",Course="IT 101",
                MT_Attendance=98,MT_Recitation=96,MT_Seatwork=97,MT_Assignment=95,MT_LongTests=97,MT_MajorExam=96,
                FT_Attendance=99,FT_Recitation=97,FT_Seatwork=98,FT_Assignment=96,FT_LongTests=98,FT_MajorExam=97, Status="Submitted" },
            new() { StudentID="2024-00008-SM-0", Name="Asay, Claire Jade A.",    Course="IT 101",
                MT_Attendance=87,MT_Recitation=85,MT_Seatwork=86,MT_Assignment=84,MT_LongTests=86,MT_MajorExam=85,
                FT_Attendance=88,FT_Recitation=87,FT_Seatwork=85,FT_Assignment=86,FT_LongTests=87,FT_MajorExam=86, Status="Submitted" },
            new() { StudentID="2024-00009-SM-0",  Name="Banting, Andrei J.",  Course="IT 101", Status="Pending" },
            new() { StudentID="2024-00010-SM-0",  Name="Basilan, Hans L.",    Course="IT 101", Status="Pending" },
            new() { StudentID="2024-00011-SM-0",  Name="Bauit, Clerkjustine N.",Course="IT 101", Status="Pending" },
            new() { StudentID="2024-00012-SM-0",  Name="Celestino, Randel E.", Course="IT 101", Status="Pending" },
            new() { StudentID="2024-00013-SM-0",  Name="Cruz, Alexander L.",   Course="IT 101", Status="Pending" },
            new() { StudentID="2024-00016-SM-0", Name="Dizon, Patricia Mae S.",Course="CS 102",
                MT_Attendance=92,MT_Recitation=90,MT_Seatwork=91,MT_Assignment=89,MT_LongTests=91,MT_MajorExam=90,
                FT_Attendance=93,FT_Recitation=91,FT_Seatwork=92,FT_Assignment=90,FT_LongTests=92,FT_MajorExam=91, Status="Submitted" },
            new() { StudentID="2024-00017-SM-0", Name="Espinosa, Marco Luis T.", Course="CS 102",
                MT_Attendance=77,MT_Recitation=75,MT_Seatwork=78,MT_Assignment=74,MT_LongTests=76,MT_MajorExam=77,
                FT_Attendance=79,FT_Recitation=77,FT_Seatwork=76,FT_Assignment=75,FT_LongTests=78,FT_MajorExam=76, Status="Submitted" },
            new() { StudentID="2024-00018-SM-0", Name="Flores, Donna C.",     Course="CS 102",
                MT_Attendance=84,MT_Recitation=83,MT_Seatwork=85,MT_Assignment=82,MT_LongTests=84,MT_MajorExam=84,
                FT_Attendance=86,FT_Recitation=84,FT_Seatwork=83,FT_Assignment=85,FT_LongTests=85,FT_MajorExam=85, Status="Submitted" },
            new() { StudentID="2024-00019-SM-0", Name="Garcia, Jose Miguel A.", Course="CS 102",
                MT_Attendance=63,MT_Recitation=61,MT_Seatwork=64,MT_Assignment=60,MT_LongTests=62,MT_MajorExam=61,
                FT_Attendance=65,FT_Recitation=63,FT_Seatwork=62,FT_Assignment=64,FT_LongTests=63,FT_MajorExam=62, Status="Submitted" },
            new() { StudentID="2024-00020-SM-0", Name="Hernandez, Carina B.",  Course="CS 102",
                MT_Attendance=98,MT_Recitation=97,MT_Seatwork=99,MT_Assignment=96,MT_LongTests=98,MT_MajorExam=97,
                FT_Attendance=99,FT_Recitation=98,FT_Seatwork=98,FT_Assignment=97,FT_LongTests=99,FT_MajorExam=98, Status="Submitted" },
            new() { StudentID="2024-00021-SM-0", Name="Ilagan, Francis John M.", Course="CS 102",
                MT_Attendance=79,MT_Recitation=77,MT_Seatwork=80,MT_Assignment=76,MT_LongTests=78,MT_MajorExam=79,
                FT_Attendance=81,FT_Recitation=79,FT_Seatwork=78,FT_Assignment=80,FT_LongTests=80,FT_MajorExam=77, Status="Submitted" },
            new() { StudentID="2024-00022-SM-0", Name="Javier, Lorraine C.",   Course="CS 102",
                MT_Attendance=56,MT_Recitation=54,MT_Seatwork=57,MT_Assignment=53,MT_LongTests=55,MT_MajorExam=54,
                FT_Attendance=58,FT_Recitation=56,FT_Seatwork=55,FT_Assignment=57,FT_LongTests=57,FT_MajorExam=55, Status="Submitted" },
            new() { StudentID="2024-00023-SM-0", Name="Lim, Kevin Paul D.",    Course="CS 102", Status="Pending" },
            new() { StudentID="2024-00024-SM-0", Name="Lopez, Angela Rose V.", Course="CS 102", Status="Pending" },
            new() { StudentID="2024-00028-SM-0", Name="Ocampo, Kristine Joy A.", Course="IS 103",
                MT_Attendance=90,MT_Recitation=88,MT_Seatwork=91,MT_Assignment=87,MT_LongTests=90,MT_MajorExam=89,
                FT_Attendance=92,FT_Recitation=90,FT_Seatwork=89,FT_Assignment=91,FT_LongTests=91,FT_MajorExam=90, Status="Submitted" },
            new() { StudentID="2024-00029-SM-0", Name="Panganiban, Jose Luis R.", Course="IS 103",
                MT_Attendance=73,MT_Recitation=71,MT_Seatwork=74,MT_Assignment=70,MT_LongTests=72,MT_MajorExam=72,
                FT_Attendance=75,FT_Recitation=73,FT_Seatwork=72,FT_Assignment=74,FT_LongTests=74,FT_MajorExam=73, Status="Submitted" },
            new() { StudentID="2024-00030-SM-0", Name="Quiambao, Rachel Ann T.", Course="IS 103",
                MT_Attendance=95,MT_Recitation=94,MT_Seatwork=96,MT_Assignment=93,MT_LongTests=95,MT_MajorExam=95,
                FT_Attendance=97,FT_Recitation=95,FT_Seatwork=94,FT_Assignment=96,FT_LongTests=96,FT_MajorExam=96, Status="Submitted" },
            new() { StudentID="2024-00031-SM-0", Name="Ramos, Enrico Santos B.", Course="IS 103",
                MT_Attendance=62,MT_Recitation=60,MT_Seatwork=63,MT_Assignment=59,MT_LongTests=61,MT_MajorExam=60,
                FT_Attendance=64,FT_Recitation=62,FT_Seatwork=61,FT_Assignment=63,FT_LongTests=63,FT_MajorExam=61, Status="Submitted" },
            new() { StudentID="2024-00032-SM-0", Name="Reyes, Maria Clara O.",  Course="IS 103",
                MT_Attendance=88,MT_Recitation=86,MT_Seatwork=89,MT_Assignment=85,MT_LongTests=87,MT_MajorExam=87,
                FT_Attendance=90,FT_Recitation=88,FT_Seatwork=87,FT_Assignment=89,FT_LongTests=89,FT_MajorExam=88, Status="Submitted" },
            new() { StudentID="2024-00033-SM-0", Name="Santos, Bianca Nicole P.", Course="IS 103",
                MT_Attendance=80,MT_Recitation=78,MT_Seatwork=81,MT_Assignment=77,MT_LongTests=79,MT_MajorExam=79,
                FT_Attendance=82,FT_Recitation=80,FT_Seatwork=79,FT_Assignment=81,FT_LongTests=81,FT_MajorExam=80, Status="Submitted" },
            new() { StudentID="2024-00034-SM-0", Name="Soriano, Aaron James V.", Course="IS 103", Status="Pending" },
            new() { StudentID="2024-00035-SM-0", Name="Tan, Melissa Grace C.",  Course="IS 103", Status="Pending" },
        };

        private void LoadGradeData()
        {
            _gradeRecords = BuildSeedData();
            _filteredRecords = new List<StudentGradeRecord>(_gradeRecords);
            _gradesLoaded = true;
            RefreshMidtermGrid();
            RefreshFinalTermGrid();
            UpdateStatCards(_filteredRecords);
        }

        private void ApplyFilters()
        {
            if (!_gradesLoaded) return;

            string query = txtSearch.Text.Trim().ToLowerInvariant();
            string statusFilter = cmbStatusFilter.SelectedIndex > 0
                ? cmbStatusFilter.SelectedItem?.ToString() ?? "" : "";
            string courseFilter = "";

            if (cmbCourseSection.SelectedIndex > 0)
            {
                string sel = cmbCourseSection.SelectedItem?.ToString() ?? "";
                int pipeIdx = sel.IndexOf('–');
                string coursePart = pipeIdx > 0 ? sel.Substring(0, pipeIdx).Trim() : sel;
                int spaceIdx = coursePart.IndexOf(' ');
                courseFilter = spaceIdx > 0
                    ? coursePart.Substring(0, spaceIdx).Trim().ToLowerInvariant()
                    : coursePart.ToLowerInvariant();
            }

            _filteredRecords = _gradeRecords.Where(r =>
            {
                bool mc = string.IsNullOrEmpty(courseFilter) ||
                          r.Course.ToLowerInvariant().StartsWith(courseFilter);
                bool ms = string.IsNullOrEmpty(query) ||
                          r.StudentID.ToLowerInvariant().Contains(query) ||
                          r.Name.ToLowerInvariant().Contains(query);
                bool mst = string.IsNullOrEmpty(statusFilter) ||
                           r.Status == statusFilter;
                return mc && ms && mst;
            }).ToList();

            RefreshMidtermGrid();
            RefreshFinalTermGrid();
            UpdateStatCards(_filteredRecords);
        }

        private void RefreshMidtermGrid()
        {
            if (gridStudents == null) return;
            gridStudents.SuspendLayout();
            gridStudents.Rows.Clear();

            foreach (var r in _filteredRecords)
            {
                string F(double? v) => v.HasValue ? v.Value.ToString("F0") : "";
                string FG(double? v) => v.HasValue ? v.Value.ToString("F2") : "";

                int idx = gridStudents.Rows.Add(
                    r.StudentID, r.Name,
                    F(r.MT_Attendance), F(r.MT_Recitation), F(r.MT_Seatwork),
                    F(r.MT_Assignment), F(r.MT_LongTests), F(r.MT_MajorExam),
                    FG(r.MidtermGrade), FG(r.FinalGrade),
                    r.TermStatusLabel(true), r.GradeEquivalent);

                ApplyRowStyles(gridStudents, gridStudents.Rows[idx], r, true);
            }

            gridStudents.ResumeLayout();
            lblRecordCount.Text = $"{_filteredRecords.Count} students";
        }

        private void RefreshFinalTermGrid()
        {
            if (dataGridView1 == null) return;
            dataGridView1.SuspendLayout();
            dataGridView1.Rows.Clear();

            foreach (var r in _filteredRecords)
            {
                string F(double? v) => v.HasValue ? v.Value.ToString("F0") : "";
                string FG(double? v) => v.HasValue ? v.Value.ToString("F2") : "";

                int idx = dataGridView1.Rows.Add(
                    r.StudentID, r.Name,
                    F(r.FT_Attendance), F(r.FT_Recitation), F(r.FT_Seatwork),
                    F(r.FT_Assignment), F(r.FT_LongTests), F(r.FT_MajorExam),
                    FG(r.FinalTermGrade), FG(r.FinalGrade),
                    r.TermStatusLabel(false), r.GradeEquivalent);

                ApplyRowStyles(dataGridView1, dataGridView1.Rows[idx], r, false);
            }

            dataGridView1.ResumeLayout();
        }

        private static void ApplyRowStyles(DataGridView dgv, DataGridViewRow row,
            StudentGradeRecord r, bool isMidterm)
        {
            foreach (DataGridViewCell cell in row.Cells)
            {
                cell.Style.ForeColor = TextPrimary;
                cell.Style.BackColor = Color.Empty;
                cell.Style.Font = null;
            }

            string[] scoreCols = isMidterm
                ? new[] { "colAttendance", "colRecitation", "colSeatwork", "colAssignment", "colLongTests", "colMajorExam" }
                : new[] { "ftColAttendance", "ftColRecitation", "ftColSeatwork", "ftColAssignment", "ftColLongTests", "ftColMajorExam" };

            foreach (string cn in scoreCols)
            {
                if (!dgv.Columns.Contains(cn)) continue;
                var cell = row.Cells[cn];
                if (double.TryParse(cell.Value?.ToString(), out double cv))
                    cell.Style.ForeColor = cv < 75 ? RedFail : TextPrimary;
            }
            string termCol = isMidterm ? "colStatus" : "ftColStatus";
            double? tg = isMidterm ? r.MidtermGrade : r.FinalTermGrade;
            if (dgv.Columns.Contains(termCol))
            {
                var sc = row.Cells[termCol];
                sc.Style.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                if (tg == null)
                {
                    sc.Style.ForeColor = Color.Gray;
                    sc.Style.BackColor = Color.FromArgb(245, 245, 245);
                }
                else if (tg.Value >= 75)
                {
                    sc.Style.ForeColor = GreenPass;
                    sc.Style.BackColor = Color.FromArgb(228, 248, 236);
                }
                else
                {
                    sc.Style.ForeColor = RedFail;
                    sc.Style.BackColor = Color.FromArgb(255, 232, 232);
                }
            }

            string tgCol = isMidterm ? "colTermGrade" : "ftColTermGrade";
            if (dgv.Columns.Contains(tgCol) && tg.HasValue)
            {
                row.Cells[tgCol].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                row.Cells[tgCol].Style.ForeColor = TextPrimary;
            }

            string fgCol = isMidterm ? "colFinalGrade" : "ftColFinalGrade";
            if (dgv.Columns.Contains(fgCol) && r.FinalGrade.HasValue)
                row.Cells[fgCol].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);

            string remCol = isMidterm ? "colRemarks" : "ftColRemarks";
            if (dgv.Columns.Contains(remCol))
            {
                row.Cells[remCol].Style.ForeColor = GetEquivColor(r.GradeEquivalent);
                row.Cells[remCol].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                row.Cells[remCol].ToolTipText = r.GradeDescription;
            }
        }

        private static Color GetEquivColor(string equiv) => equiv switch
        {
            "1.00" or "1.25" or "1.50" => GreenPass,
            "1.75" or "2.00" or "2.25" => BlueGood,
            "2.50" or "2.75" or "3.00" => AmberWarn,
            "5.00" => RedFail,
            _ => Color.Gray
        };

        private void UpdateStatCards(List<StudentGradeRecord> records)
        {
            int total = records.Count;
            int submitted = records.Count(r => r.Status == "Submitted");
            int pending = records.Count(r => r.Status == "Pending");
            double avg = records.Where(r => r.FinalGrade.HasValue)
                                .Select(r => r.FinalGrade!.Value).DefaultIfEmpty(0).Average();
            double highest = records.Where(r => r.FinalGrade.HasValue)
                                    .Select(r => r.FinalGrade!.Value).DefaultIfEmpty(0).Max();

            lblTotalVal.Text = total.ToString();
            lblSubmittedVal.Text = $"{submitted}";
            lblPendingVal.Text = $"{pending}";
            lblAvgVal.Text = total > 0 ? avg.ToString("F2") : "—";
            lblHighestVal.Text = total > 0 ? highest.ToString("F2") : "—";

            double subPct = total > 0 ? submitted * 100.0 / total : 0;
            double penPct = total > 0 ? pending * 100.0 / total : 0;
            lblSubmittedSub.Text = $"{subPct:F0}% of class";
            lblPendingSub.Text = $"{penPct:F0}% of class";
            lblAvgSub.Text = records.Any(r => r.FinalGrade.HasValue) ? "current average" : "no data yet";
            lblHighestSub.Text = records.Any(r => r.FinalGrade.HasValue) ? "top score" : "no data yet";
            lblTotalSub.Text = $"{(cmbCourseSection.SelectedIndex == 0 ? "all sections" : "this section")}";
        }
        private static readonly string[] _mtEditable =
            { "colAttendance","colRecitation","colSeatwork","colAssignment","colLongTests","colMajorExam" };
        private static readonly string[] _ftEditable =
            { "ftColAttendance","ftColRecitation","ftColSeatwork","ftColAssignment","ftColLongTests","ftColMajorExam" };

        private void GridStudents_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || gridStudents == null) return;
            string cn = gridStudents.Columns[e.ColumnIndex].Name;
            if (!_mtEditable.Contains(cn)) return;

            var row = gridStudents.Rows[e.RowIndex];
            string id = row.Cells["colStudentID"].Value?.ToString() ?? "";
            var rec = _gradeRecords.FirstOrDefault(r => r.StudentID == id);
            if (rec == null) return;

            double? att = ParseCell(row.Cells["colAttendance"].Value);
            double? rcit = ParseCell(row.Cells["colRecitation"].Value);
            double? seat = ParseCell(row.Cells["colSeatwork"].Value);
            double? asgn = ParseCell(row.Cells["colAssignment"].Value);
            double? lt = ParseCell(row.Cells["colLongTests"].Value);
            double? mex = ParseCell(row.Cells["colMajorExam"].Value);

            if (_showingMidterm)
            {
                rec.MT_Attendance = att; rec.MT_Recitation = rcit;
                rec.MT_Seatwork = seat; rec.MT_Assignment = asgn;
                rec.MT_LongTests = lt; rec.MT_MajorExam = mex;
            }

            RecalcRow(gridStudents, row, rec, _showingMidterm);
            UpdateStatus(rec);
        }

        private void FinalTermGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridView1 == null) return;
            string cn = dataGridView1.Columns[e.ColumnIndex].Name;
            if (!_ftEditable.Contains(cn)) return;

            var row = dataGridView1.Rows[e.RowIndex];
            string id = row.Cells["ftColStudentID"].Value?.ToString() ?? "";
            var rec = _gradeRecords.FirstOrDefault(r => r.StudentID == id);
            if (rec == null) return;

            double? att = ParseCell(row.Cells["ftColAttendance"].Value);
            double? rcit = ParseCell(row.Cells["ftColRecitation"].Value);
            double? seat = ParseCell(row.Cells["ftColSeatwork"].Value);
            double? asgn = ParseCell(row.Cells["ftColAssignment"].Value);
            double? lt = ParseCell(row.Cells["ftColLongTests"].Value);
            double? mex = ParseCell(row.Cells["ftColMajorExam"].Value);

            rec.FT_Attendance = att; rec.FT_Recitation = rcit;
            rec.FT_Seatwork = seat; rec.FT_Assignment = asgn;
            rec.FT_LongTests = lt; rec.FT_MajorExam = mex;

            RecalcRow(dataGridView1, row, rec, false);
            UpdateStatus(rec);
        }

        private static void RecalcRow(DataGridView dgv, DataGridViewRow row,
            StudentGradeRecord rec, bool isMidterm)
        {
            ApplyRowStyles(dgv, row, rec, isMidterm);

            string tgCol = isMidterm ? "colTermGrade" : "ftColTermGrade";
            string fgCol = isMidterm ? "colFinalGrade" : "ftColFinalGrade";
            string stCol = isMidterm ? "colStatus" : "ftColStatus";
            string rmCol = isMidterm ? "colRemarks" : "ftColRemarks";

            double? tg = isMidterm ? rec.MidtermGrade : rec.FinalTermGrade;

            if (dgv.Columns.Contains(tgCol))
                row.Cells[tgCol].Value = tg.HasValue ? tg.Value.ToString("F2") : "";
            if (dgv.Columns.Contains(fgCol))
                row.Cells[fgCol].Value = rec.FinalGrade.HasValue ? rec.FinalGrade.Value.ToString("F2") : "";
            if (dgv.Columns.Contains(stCol))
                row.Cells[stCol].Value = rec.TermStatusLabel(isMidterm);
            if (dgv.Columns.Contains(rmCol))
                row.Cells[rmCol].Value = rec.GradeEquivalent;
        }

        private static void UpdateStatus(StudentGradeRecord rec)
        {
            bool allMT = rec.MT_Attendance.HasValue && rec.MT_Recitation.HasValue &&
                         rec.MT_Seatwork.HasValue && rec.MT_Assignment.HasValue &&
                         rec.MT_LongTests.HasValue && rec.MT_MajorExam.HasValue;
            bool allFT = rec.FT_Attendance.HasValue && rec.FT_Recitation.HasValue &&
                         rec.FT_Seatwork.HasValue && rec.FT_Assignment.HasValue &&
                         rec.FT_LongTests.HasValue && rec.FT_MajorExam.HasValue;
            if (allMT && allFT) rec.Status = "Submitted";
        }

        private static double? ParseCell(object val)
        {
            if (val == null || string.IsNullOrWhiteSpace(val.ToString())) return null;
            if (!double.TryParse(val.ToString(), out double d)) return null;
            return Math.Max(0, Math.Min(100, d));
        }

        private void SaveGradeChanges()
        {
            if (!_gradesLoaded)
            {
                ShowInfo("Nothing to save. Load students first.", "Save");
                return;
            }
            CommitGridEdits();
            RefreshMidtermGrid();
            RefreshFinalTermGrid();
            UpdateStatCards(_filteredRecords);
            ShowInfo("Changes saved successfully!", "Saved");
        }

        private void CommitGridEdits()
        {
            gridStudents?.CommitEdit(DataGridViewDataErrorContexts.Commit);
            gridStudents?.EndEdit();
            dataGridView1?.CommitEdit(DataGridViewDataErrorContexts.Commit);
            dataGridView1?.EndEdit();
        }

        private void BtnReleaseGrades_Click(object sender, EventArgs e)
        {
            if (!_gradesLoaded) { ShowWarning("Load students first.", "Release Grades"); return; }
            int unreleased = _gradeRecords.Count(r => r.Status == "Submitted" && !r.Released);
            if (unreleased == 0) { ShowInfo("All submitted grades have already been released.", "Release Grades"); return; }

            if (MessageBox.Show(
                $"Release grades for {unreleased} student(s)?\nThis action cannot be undone.",
                "Release Grades", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                foreach (var r in _gradeRecords.Where(r => r.Status == "Submitted")) r.Released = true;
                RefreshMidtermGrid();
                ShowInfo("Grades released successfully.", "Released");
            }
        }

        private void ShowEditGradePercentageDialog()
        {
            using var dlg = new EditGradePercentageControl(CurrentWeights);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                CurrentWeights = dlg.UpdatedWeights;
                SetupGrid();
                SetupFinalTermGrid();
                CommitGridEdits();
                RefreshMidtermGrid();
                RefreshFinalTermGrid();
                UpdateStatCards(_filteredRecords);
            }
        }

        private void ShowAddCustomColumnDialog()
        {
            using var dlg = new Form
            {
                Text = "Add Custom Column",
                Size = new Size(420, 280),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Surface,
            };

            var hdr = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = Maroon
            };
            hdr.Controls.Add(new Label
            {
                Text = "Add Custom Grade Column",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(14, 0, 0, 0)
            });
            dlg.Controls.Add(hdr);

            int y = 58;
            void Row(string lbl, Control ctrl)
            {
                dlg.Controls.Add(new Label
                {
                    Text = lbl,
                    Left = 18,
                    Top = y + 4,
                    Width = 120,
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold)
                });
                ctrl.Left = 148; ctrl.Top = y; ctrl.Width = 240;
                dlg.Controls.Add(ctrl);
                y += 38;
            }

            var tbLabel = new TextBox { PlaceholderText = "e.g. Quiz 1, Project A" };
            Row("Column Label:", tbLabel);

            var cmbTerm = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTerm.Items.AddRange(new object[] { "Midterm", "Final Term" });
            cmbTerm.SelectedIndex = _showingMidterm ? 0 : 1;
            Row("Term:", cmbTerm);

            var cmbCat = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCat.Items.AddRange(new object[] { "Seatwork", "Long Test", "Assignment", "Recitation", "Other" });
            cmbCat.SelectedIndex = 0;
            Row("Category:", cmbCat);

            var lblErr = new Label
            {
                Left = 18,
                Top = y,
                Width = 370,
                AutoSize = true,
                ForeColor = RedFail,
                Font = new Font("Segoe UI", 8.5f)
            };
            dlg.Controls.Add(lblErr);

            y += 24;
            var btnOk = new Button
            {
                Text = "Add Column",
                Left = 148,
                Top = y,
                Width = 120,
                Height = 30,
                BackColor = Maroon,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnOk.FlatAppearance.BorderSize = 0;
            var btnCx = new Button
            {
                Text = "Cancel",
                Left = 278,
                Top = y,
                Width = 80,
                Height = 30,
                FlatStyle = FlatStyle.Flat
            };

            btnOk.Click += (s, _) =>
            {
                string lbl = tbLabel.Text.Trim();
                if (string.IsNullOrEmpty(lbl)) { lblErr.Text = "Please enter a label."; return; }
                bool mt = cmbTerm.SelectedIndex == 0;
                var list = mt ? _customMTColumns : _customFTColumns;
                string key = (mt ? "mt_" : "ft_") +
                    System.Text.RegularExpressions.Regex.Replace(lbl.ToLower(), "[^a-z0-9]", "_");
                if (list.Any(c => c.ColName == key)) { lblErr.Text = "Column already exists."; return; }

                list.Add(new CustomGradeColumn
                {
                    ColName = key,
                    Label = lbl,
                    Category = cmbCat.Text,
                    IsMidterm = mt
                });

                SetupGrid(); SetupFinalTermGrid();
                AddCustomCols(gridStudents, _customMTColumns);
                AddCustomCols(dataGridView1, _customFTColumns);
                RefreshMidtermGrid(); RefreshFinalTermGrid();
                dlg.DialogResult = DialogResult.OK;
            };
            btnCx.Click += (s, _) => dlg.Close();
            dlg.Controls.Add(btnOk);
            dlg.Controls.Add(btnCx);
            dlg.ShowDialog(this);
        }

        private static void AddCustomCols(DataGridView dgv, List<CustomGradeColumn> cols)
        {
            if (dgv == null) return;
            int insertIdx = -1;
            foreach (DataGridViewColumn c in dgv.Columns)
                if (c.Name.EndsWith("TermGrade", StringComparison.OrdinalIgnoreCase))
                { insertIdx = c.Index; break; }
            if (insertIdx < 0) insertIdx = Math.Max(0, dgv.Columns.Count - 3);

            foreach (var cc in cols)
            {
                if (dgv.Columns.Contains(cc.ColName)) continue;
                var col = new DataGridViewTextBoxColumn
                {
                    Name = cc.ColName,
                    HeaderText = $"{cc.Label}\n({cc.Category})",
                    ReadOnly = false,
                    MinimumWidth = 65,
                    FillWeight = 65,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter },
                    HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                    Tag = cc,
                };
                if (insertIdx < dgv.Columns.Count) dgv.Columns.Insert(insertIdx++, col);
                else dgv.Columns.Add(col);
            }
        }

        private void ShowStudentDetailPanel(StudentGradeRecord rec)
        {
            using var dlg = new Form
            {
                Text = $"Grade Detail — {rec.Name}",
                Size = new Size(560, 600),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Background,
            };

            var hdr = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Maroon };
            hdr.Paint += (s, pe) =>
            {
                var g = pe.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var br = new SolidBrush(Color.FromArgb(40, 255, 255, 255));
                g.FillEllipse(br, -30, -30, 100, 100);
            };
            hdr.Controls.Add(new Label
            {
                Text = rec.Name,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(16, 8)
            });
            hdr.Controls.Add(new Label
            {
                Text = $"ID: {rec.StudentID}  •  Course: {rec.Course}  •  Status: {rec.Status}",
                ForeColor = Color.FromArgb(220, 220, 220),
                Font = new Font("Segoe UI", 8.5f),
                AutoSize = true,
                Location = new Point(17, 34)
            });
            dlg.Controls.Add(hdr);

            var scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(16) };
            dlg.Controls.Add(scroll);

            int y = 8;

            void SectionTitle(string title)
            {
                var lbl = new Label
                {
                    Text = title,
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                    ForeColor = Maroon,
                    Left = 0,
                    Top = y,
                    Width = 500,
                    Height = 20
                };
                scroll.Controls.Add(lbl);
                y += 22;
            }

            void ScoreRow(string label, double? mt, double? ft)
            {
                var pnl = new Panel
                {
                    Left = 0,
                    Top = y,
                    Width = 500,
                    Height = 28,
                    BackColor = Surface
                };
                pnl.Controls.Add(new Label
                {
                    Text = label,
                    Left = 8,
                    Top = 5,
                    Width = 180,
                    Font = new Font("Segoe UI", 9f)
                });
                pnl.Controls.Add(new Label
                {
                    Text = mt.HasValue ? mt.Value.ToString("F0") : "—",
                    Left = 200,
                    Top = 5,
                    Width = 60,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                    ForeColor = mt.HasValue && mt.Value < 75 ? RedFail : TextPrimary
                });
                pnl.Controls.Add(new Label
                {
                    Text = ft.HasValue ? ft.Value.ToString("F0") : "—",
                    Left = 280,
                    Top = 5,
                    Width = 60,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                    ForeColor = ft.HasValue && ft.Value < 75 ? RedFail : TextPrimary
                });
                scroll.Controls.Add(pnl);
                y += 30;
            }

            void GradeRow(string label, double? val, bool useBadge = false)
            {
                var pnl = new Panel
                {
                    Left = 0,
                    Top = y,
                    Width = 500,
                    Height = 34,
                    BackColor = Surface
                };
                pnl.Controls.Add(new Label
                {
                    Text = label,
                    Left = 8,
                    Top = 8,
                    Width = 200,
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold)
                });
                if (val.HasValue)
                {
                    bool pass = val.Value >= 75;
                    var vLbl = new Label
                    {
                        Text = val.Value.ToString("F2"),
                        Left = 210,
                        Top = 6,
                        Width = 90,
                        Height = 22,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                        ForeColor = pass ? GreenPass : RedFail,
                        BackColor = useBadge ? (pass ? Color.FromArgb(228, 248, 236) : Color.FromArgb(255, 232, 232)) : Color.Transparent
                    };
                    pnl.Controls.Add(vLbl);
                }
                else
                {
                    pnl.Controls.Add(new Label
                    {
                        Text = "INC",
                        Left = 210,
                        Top = 8,
                        AutoSize = true,
                        Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                        ForeColor = Color.Gray
                    });
                }
                scroll.Controls.Add(pnl);
                y += 36;
            }

            var hdrRow = new Panel { Left = 0, Top = y, Width = 500, Height = 22, BackColor = Color.FromArgb(230, 215, 215) };
            hdrRow.Controls.Add(new Label
            {
                Text = "Component",
                Left = 8,
                Top = 2,
                Width = 180,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Maroon
            });
            hdrRow.Controls.Add(new Label
            {
                Text = "Midterm",
                Left = 200,
                Top = 2,
                Width = 60,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Maroon,
                TextAlign = ContentAlignment.MiddleCenter
            });
            hdrRow.Controls.Add(new Label
            {
                Text = "Final Term",
                Left = 280,
                Top = 2,
                Width = 80,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Maroon,
                TextAlign = ContentAlignment.MiddleCenter
            });
            scroll.Controls.Add(hdrRow);
            y += 24;

            SectionTitle("Class Standing Components");
            ScoreRow($"Attendance ({CurrentWeights.AttendancePct}%)", rec.MT_Attendance, rec.FT_Attendance);
            ScoreRow($"Recitation ({CurrentWeights.RecitationPct}%)", rec.MT_Recitation, rec.FT_Recitation);
            ScoreRow($"Seatwork ({CurrentWeights.SeatworkPct}%)", rec.MT_Seatwork, rec.FT_Seatwork);
            ScoreRow($"Assignment ({CurrentWeights.AssignmentPct}%)", rec.MT_Assignment, rec.FT_Assignment);
            ScoreRow($"Long Tests ({CurrentWeights.LongTestsPct}%)", rec.MT_LongTests, rec.FT_LongTests);
            y += 4;
            SectionTitle("Major Exam Component");
            ScoreRow($"Major Exam ({CurrentWeights.MajorExamsPct}%)", rec.MT_MajorExam, rec.FT_MajorExam);
            y += 8;
            SectionTitle("Computed Grades");
            GradeRow("Midterm Grade", rec.MidtermGrade, true);
            GradeRow("Final Term Grade", rec.FinalTermGrade, true);
            GradeRow("Final Grade  (Average)", rec.FinalGrade, true);

            if (rec.FinalGrade.HasValue)
            {
                var eqPnl = new Panel { Left = 0, Top = y, Width = 500, Height = 40, BackColor = Surface };
                eqPnl.Controls.Add(new Label
                {
                    Text = "Grade Equivalent:",
                    Left = 8,
                    Top = 10,
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold)
                });
                eqPnl.Controls.Add(new Label
                {
                    Text = $"  {rec.GradeEquivalent}  —  {rec.GradeDescription}  ",
                    Left = 170,
                    Top = 6,
                    Height = 28,
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    ForeColor = GetEquivColor(rec.GradeEquivalent),
                    BackColor = Color.FromArgb(245, 235, 235),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BorderStyle = BorderStyle.FixedSingle,
                });
                scroll.Controls.Add(eqPnl);
                y += 44;
            }

            dlg.ShowDialog(this);
        }

        private void BtnExportCSV_Click(object sender, EventArgs e)
        {
            if (!_gradesLoaded || _filteredRecords.Count == 0)
            { ShowWarning("No data to export.", "Export"); return; }

            CommitGridEdits();
            using var sfd = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                FileName = $"Grades_{DateTime.Now:yyyyMMdd_HHmm}.csv",
                Title = "Export Grades"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                var sb = new StringBuilder();
                sb.AppendLine("StudentID,Name,Course," +
                    "MT_Att,MT_Rec,MT_Seat,MT_Asgn,MT_LT,MT_Exam," +
                    "FT_Att,FT_Rec,FT_Seat,FT_Asgn,FT_LT,FT_Exam," +
                    "MidtermGrade,FinalTermGrade,FinalGrade,Status,Equiv,Description");

                foreach (var r in _filteredRecords)
                    sb.AppendLine(string.Join(",",
                        CE(r.StudentID), CE(r.Name), CE(r.Course),
                        Fmt(r.MT_Attendance), Fmt(r.MT_Recitation), Fmt(r.MT_Seatwork),
                        Fmt(r.MT_Assignment), Fmt(r.MT_LongTests), Fmt(r.MT_MajorExam),
                        Fmt(r.FT_Attendance), Fmt(r.FT_Recitation), Fmt(r.FT_Seatwork),
                        Fmt(r.FT_Assignment), Fmt(r.FT_LongTests), Fmt(r.FT_MajorExam),
                        FG(r.MidtermGrade), FG(r.FinalTermGrade), FG(r.FinalGrade),
                        r.Status, r.GradeEquivalent, r.GradeDescription));

                File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                ShowInfo($"Exported to:\n{sfd.FileName}", "Export Successful");
            }
            catch (Exception ex) { ShowError("Export failed: " + ex.Message, "Error"); }
        }

        private static string Fmt(double? v) => v.HasValue ? v.Value.ToString("F0") : "";
        private static string FG(double? v) => v.HasValue ? v.Value.ToString("F2") : "";
        private static string CE(string s) => s.Contains(',') || s.Contains('"')
            ? $"\"{s.Replace("\"", "\"\"")}\"" : s;

        private void BtnImportCSV_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog { Filter = "CSV (*.csv)|*.csv", Title = "Import Grades" };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            try
            {
                var lines = File.ReadAllLines(ofd.FileName, Encoding.UTF8);
                if (lines.Length < 2) { ShowWarning("File is empty.", "Import"); return; }

                string[] headers = CsvSplit(lines[0]);
                int CI(string n) => Array.FindIndex(headers,
                    h => h.Trim().Equals(n, StringComparison.OrdinalIgnoreCase));

                int iID = CI("StudentID"), iName = CI("Name"), iCourse = CI("Course");
                int iMAtt = CI("MT_Att"), iMRec = CI("MT_Rec"), iMSeat = CI("MT_Seat"),
                    iMAsgn = CI("MT_Asgn"), iMLT = CI("MT_LT"), iMMex = CI("MT_Exam");
                int iFAtt = CI("FT_Att"), iFRec = CI("FT_Rec"), iFSeat = CI("FT_Seat"),
                    iFAsgn = CI("FT_Asgn"), iFLT = CI("FT_LT"), iFMex = CI("FT_Exam");

                if (iID < 0) { ShowError("'StudentID' column not found. Use exported file as template.", "Import Error"); return; }

                int added = 0, updated = 0, skipped = 0;
                for (int li = 1; li < lines.Length; li++)
                {
                    if (string.IsNullOrWhiteSpace(lines[li])) continue;
                    var cols = CsvSplit(lines[li]);
                    if (cols.Length <= iID) { skipped++; continue; }

                    string sid = cols[iID].Trim();
                    if (string.IsNullOrEmpty(sid)) { skipped++; continue; }

                    double? Safe(int idx)
                    {
                        if (idx < 0 || idx >= cols.Length) return null;
                        string raw = cols[idx].Trim();
                        if (string.IsNullOrEmpty(raw)) return null;
                        if (double.TryParse(raw, NumberStyles.Any,
                            CultureInfo.InvariantCulture, out double v) && v >= 0 && v <= 100) return v;
                        return null;
                    }

                    var rec = _gradeRecords.FirstOrDefault(r => r.StudentID == sid);
                    bool isNew = rec == null;
                    if (isNew) { rec = new StudentGradeRecord { StudentID = sid, Status = "Pending" }; _gradeRecords.Add(rec); }

                    if (iName >= 0 && iName < cols.Length && !string.IsNullOrEmpty(cols[iName].Trim()))
                        rec!.Name = cols[iName].Trim();
                    if (iCourse >= 0 && iCourse < cols.Length && !string.IsNullOrEmpty(cols[iCourse].Trim()))
                        rec!.Course = cols[iCourse].Trim();

                    if (Safe(iMAtt).HasValue) rec!.MT_Attendance = Safe(iMAtt);
                    if (Safe(iMRec).HasValue) rec!.MT_Recitation = Safe(iMRec);
                    if (Safe(iMSeat).HasValue) rec!.MT_Seatwork = Safe(iMSeat);
                    if (Safe(iMAsgn).HasValue) rec!.MT_Assignment = Safe(iMAsgn);
                    if (Safe(iMLT).HasValue) rec!.MT_LongTests = Safe(iMLT);
                    if (Safe(iMMex).HasValue) rec!.MT_MajorExam = Safe(iMMex);
                    if (Safe(iFAtt).HasValue) rec!.FT_Attendance = Safe(iFAtt);
                    if (Safe(iFRec).HasValue) rec!.FT_Recitation = Safe(iFRec);
                    if (Safe(iFSeat).HasValue) rec!.FT_Seatwork = Safe(iFSeat);
                    if (Safe(iFAsgn).HasValue) rec!.FT_Assignment = Safe(iFAsgn);
                    if (Safe(iFLT).HasValue) rec!.FT_LongTests = Safe(iFLT);
                    if (Safe(iFMex).HasValue) rec!.FT_MajorExam = Safe(iFMex);

                    UpdateStatus(rec!);
                    if (isNew) added++; else updated++;
                }

                _filteredRecords = new List<StudentGradeRecord>(_gradeRecords);
                _gradesLoaded = true;
                RefreshMidtermGrid(); RefreshFinalTermGrid();
                UpdateStatCards(_filteredRecords);
                ShowInfo($"Import complete:\n✔ {added} added   ✔ {updated} updated" +
                    (skipped > 0 ? $"   ⚠ {skipped} skipped" : ""), "Import Successful");
            }
            catch (Exception ex) { ShowError("Import failed: " + ex.Message, "Error"); }
        }

        private static string[] CsvSplit(string line)
        {
            var fields = new List<string>();
            bool inQ = false;
            var cur = new StringBuilder();
            foreach (char ch in line)
            {
                if (ch == '"') inQ = !inQ;
                else if (ch == ',' && !inQ) { fields.Add(cur.ToString()); cur.Clear(); }
                else cur.Append(ch);
            }
            fields.Add(cur.ToString());
            return fields.ToArray();
        }

        private void BtnPrintGrades_Click(object sender, EventArgs e)
        {
            if (!_gradesLoaded || _filteredRecords.Count == 0)
            { ShowWarning("No data to print.", "Print"); return; }
            CommitGridEdits();

            using var pd = new PrintDocument { DocumentName = "Grade Report" };
            pd.DefaultPageSettings.Landscape = true;
            pd.DefaultPageSettings.Margins = new Margins(36, 36, 36, 36);
            int printRow = 0;
            var data = _filteredRecords.ToList();

            pd.PrintPage += (s, pe) =>
            {
                var g = pe.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                float x = pe.MarginBounds.Left, y2 = pe.MarginBounds.Top;
                float pgW = pe.MarginBounds.Width;
                var mBrush = new SolidBrush(Maroon);
                var fTitle = new Font("Segoe UI", 16f, FontStyle.Bold);
                var fBold = new Font("Segoe UI", 9f, FontStyle.Bold);
                var fReg = new Font("Segoe UI", 9f);
                var fSm = new Font("Segoe UI", 8f);
                float lh = fReg.GetHeight(g) + 6;

                g.DrawString("PUP – Grade Report", fTitle, mBrush, x, y2);
                y2 += fTitle.GetHeight(g) + 4;
                string sec = cmbCourseSection.SelectedIndex > 0
                    ? cmbCourseSection.SelectedItem?.ToString() ?? "All Sections" : "All Sections";
                g.DrawString(sec, fBold, mBrush, x, y2);
                y2 += fBold.GetHeight(g) + 6;
                g.DrawLine(new Pen(Maroon, 1.5f), x, y2, x + pgW, y2);
                y2 += 8;

                float[] cw = { 120, pgW - 120 - 70 - 70 - 70 - 70 - 70, 70, 70, 70, 70, 60 };
                string[] hdrs = { "Student ID", "Name", "MT Grade", "FT Grade", "Final", "Equiv.", "Status" };
                float cx2 = x;
                for (int i = 0; i < hdrs.Length; i++)
                {
                    g.FillRectangle(mBrush, cx2, y2, cw[i], lh + 4);
                    var sf = new StringFormat
                    {
                        Alignment = i <= 1 ? StringAlignment.Near : StringAlignment.Center,
                        LineAlignment = StringAlignment.Center,
                        Trimming = StringTrimming.EllipsisCharacter
                    };
                    g.DrawString(hdrs[i], fBold, Brushes.White, new RectangleF(cx2 + 3, y2, cw[i] - 6, lh + 4), sf);
                    cx2 += cw[i];
                }
                y2 += lh + 4;
                bool alt2 = false;

                while (printRow < data.Count)
                {
                    if (y2 + lh > pe.MarginBounds.Bottom) { pe.HasMorePages = true; return; }
                    var r = data[printRow++];
                    if (alt2) g.FillRectangle(new SolidBrush(Color.FromArgb(248, 244, 244)), x, y2, pgW, lh);
                    string[] vals = { r.StudentID, r.Name,
                        r.MidtermGrade.HasValue ? r.MidtermGrade.Value.ToString("F2") : "—",
                        r.FinalTermGrade.HasValue ? r.FinalTermGrade.Value.ToString("F2") : "—",
                        r.FinalGrade.HasValue ? r.FinalGrade.Value.ToString("F2") : "—",
                        r.GradeEquivalent, r.TermStatusLabel(true) };
                    cx2 = x;
                    for (int i = 0; i < vals.Length; i++)
                    {
                        Brush fg = Brushes.Black;
                        if (i == 5) fg = new SolidBrush(GetEquivColor(vals[i]));
                        if (i == 6) fg = vals[i] == "Passed"
                            ? new SolidBrush(GreenPass) : new SolidBrush(Color.Gray);
                        var sf = new StringFormat
                        {
                            Alignment = i <= 1 ? StringAlignment.Near : StringAlignment.Center,
                            LineAlignment = StringAlignment.Center,
                            Trimming = StringTrimming.EllipsisCharacter
                        };
                        g.DrawString(vals[i], fReg, fg, new RectangleF(cx2 + 3, y2, cw[i] - 6, lh), sf);
                        cx2 += cw[i];
                    }
                    g.DrawLine(Pens.LightGray, x, y2 + lh, x + pgW, y2 + lh);
                    y2 += lh; alt2 = !alt2;
                }
                y2 += 8;
                g.DrawLine(new Pen(Maroon, 1f), x, y2, x + pgW, y2);
                y2 += 4;
                g.DrawString($"Total: {data.Count}   Submitted: {data.Count(r => r.Status == "Submitted")}   Pending: {data.Count(r => r.Status == "Pending")}",
                    fSm, mBrush, x, y2);
                pe.HasMorePages = false;
            };

            using var ppd = new PrintPreviewDialog { Document = pd, WindowState = FormWindowState.Maximized };
            ppd.ShowDialog(this);
        }

        private static void ShowInfo(string msg, string title) =>
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        private static void ShowWarning(string msg, string title) =>
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        private static void ShowError(string msg, string title) =>
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}