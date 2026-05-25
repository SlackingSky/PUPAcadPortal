using PUPAcadPortal.Dialogs;
using PUPAcadPortal.Utils;
using static PUPAcadPortal.PortalContents.Instructor.LMS.GradesContentInst;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Instructor.LMS
{
    public partial class GradesContentInst : UserControl
    {
        private bool _gradesLoaded = false;
        private List<StudentGradeRecord> _gradeRecords = new List<StudentGradeRecord>();
        private List<StudentGradeRecord> _filteredRecords = new List<StudentGradeRecord>();

        public GradesContentInst()
        {
            InitializeComponent();

            WireGradeManagementButtons();
        }
        //  GRADE WEIGHTS  (PUP Grading System)
        public static GradeWeights CurrentWeights = new GradeWeights();

        public class GradeWeights
        {
            public double AttendancePct { get; set; } = 10;
            public double RecitationPct { get; set; } = 10;
            public double SeatworkPct { get; set; } = 15;
            public double AssignmentPct { get; set; } = 10;
            public double LongTestsPct { get; set; } = 25;
            public double ClassStandingPct => AttendancePct + RecitationPct + SeatworkPct
                                           + AssignmentPct + LongTestsPct; // 70 %
            public double MajorExamsPct { get; set; } = 30;
        }

        //  STUDENT GRADE RECORD
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
                    if (!MT_Attendance.HasValue || !MT_Recitation.HasValue || !MT_Seatwork.HasValue
                     || !MT_Assignment.HasValue || !MT_LongTests.HasValue || !MT_MajorExam.HasValue)
                        return null;

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
                    if (!FT_Attendance.HasValue || !FT_Recitation.HasValue || !FT_Seatwork.HasValue
                     || !FT_Assignment.HasValue || !FT_LongTests.HasValue || !FT_MajorExam.HasValue)
                        return null;

                    double cs = FT_Attendance.Value * (w.AttendancePct / 100.0)
                              + FT_Recitation.Value * (w.RecitationPct / 100.0)
                              + FT_Seatwork.Value * (w.SeatworkPct / 100.0)
                              + FT_Assignment.Value * (w.AssignmentPct / 100.0)
                              + FT_LongTests.Value * (w.LongTestsPct / 100.0);
                    return Math.Round(cs + FT_MajorExam.Value * (w.MajorExamsPct / 100.0), 2);
                }
            }

            // Final Grade = (MTG + FTG) / 2
            public double? FinalGrade =>
                (MidtermGrade.HasValue && FinalTermGrade.HasValue)
                    ? Math.Round((MidtermGrade.Value + FinalTermGrade.Value) / 2.0, 2)
                    : null;

            public double? Quiz => MT_LongTests;
            public double? MidtermG => MT_MajorExam;
            public double? FinalExam => FT_MajorExam;
            public double? Assignments => MT_Assignment;

            public string Remarks
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

            public string RemarksDescription
            {
                get
                {
                    if (!FinalGrade.HasValue) return "Incomplete";
                    double s = FinalGrade.Value;
                    if (s >= 97) return "Excellent";
                    if (s >= 94) return "Excellent";
                    if (s >= 91) return "Very Good";
                    if (s >= 88) return "Very Good";
                    if (s >= 85) return "Good";
                    if (s >= 82) return "Good";
                    if (s >= 79) return "Satisfactory";
                    if (s >= 76) return "Satisfactory";
                    if (s >= 75) return "Passing";
                    return "Failed";
                }
            }
        }
        //  PUP GRADE HELPERS
        private static (string Grade, string Description) GetPupGrade(double? score)
        {
            if (!score.HasValue) return ("INC", "Incomplete");
            double s = score.Value;
            if (s >= 97) return ("1.00", "Excellent");
            if (s >= 94) return ("1.25", "Excellent");
            if (s >= 91) return ("1.50", "Very Good");
            if (s >= 88) return ("1.75", "Very Good");
            if (s >= 85) return ("2.00", "Good");
            if (s >= 82) return ("2.25", "Good");
            if (s >= 79) return ("2.50", "Satisfactory");
            if (s >= 76) return ("2.75", "Satisfactory");
            if (s >= 75) return ("3.00", "Passing");
            return ("5.00", "Failed");
        }

        private static Color GetPupGradeColor(string grade) => grade switch
        {
            "1.00" or "1.25" or "1.50" => Color.FromArgb(16, 124, 65),
            "1.75" or "2.00" or "2.25" => Color.FromArgb(0, 112, 192),
            "2.50" or "2.75" or "3.00" => Color.FromArgb(180, 120, 0),
            "5.00" => Color.Firebrick,
            _ => Color.Gray,
        };

        //  WIRE GRADE MANAGEMENT BUTTONS
        private void WireGradeManagementButtons()
        {
            if (cmbCourseSection != null)
            {
                cmbCourseSection.Items.Clear();
                cmbCourseSection.Items.Add("All Courses");
                cmbCourseSection.Items.Add("IT 101 - Introduction to Computing | BSIT 2-1");
                cmbCourseSection.Items.Add("CS 102 - Data Structures | BSCS 2-2");
                cmbCourseSection.Items.Add("IS 103 - Database Management | BSIS 3-1");
                cmbCourseSection.SelectedIndex = 0;
                cmbCourseSection.SelectedIndexChanged += CmbCourseSection_SelectedIndexChanged;
            }

            if (txtSearch != null) txtSearch.TextChanged += TxtGradeSearch_TextChanged;
            if (btnClearFilters != null) btnClearFilters.Click += BtnClearGradeFilters_Click;
            if (btnReleaseGrades != null) btnReleaseGrades.Click += BtnReleaseGrades_Click;
            if (btnColumnOptions != null) btnColumnOptions.Click += BtnColumnOptions_Click;
            if (btnSaveChanges != null) btnSaveChanges.Click += BtnSaveChanges_Click;
            if (btnExportExcel != null) btnExportExcel.Click += BtnExportExcel_Click;
            if (btnPrintGrades != null) btnPrintGrades.Click += BtnPrintGrades_Click;

            if (btnImportExcel1 != null) btnImportExcel1.Click += BtnImportExcel_Click;

            if (btnEditGradePercentage != null)
                btnEditGradePercentage.Click += (s, e) => ShowEditGradePercentageDialog();
            if (tabControl1 != null)
            {
                tabControl1.SelectedIndexChanged += (s, e) =>
                {
                    _showingMidterm = (tabControl1.SelectedIndex == 0);
                    if (_showingMidterm)
                        RefreshGrid();
                    else
                        RefreshFinalTermGrid();
                };
            }

            if (gridStudents != null)
            {
                gridStudents.CellValueChanged += GridStudents_CellValueChanged;
                gridStudents.EditingControlShowing += GridStudents_EditingControlShowing;
                gridStudents.DefaultValuesNeeded += GridStudents_DefaultValuesNeeded;
                gridStudents.DataError += (s, e) => e.Cancel = true;
                gridStudents.CellEnter += GridStudents_CellEnter;
            }

            if (dataGridView1 != null)
            {
                dataGridView1.CellValueChanged += FinalTermGrid_CellValueChanged;
                dataGridView1.EditingControlShowing += FinalTermGrid_EditingControlShowing;
                dataGridView1.DataError += (s, e) => e.Cancel = true;
                dataGridView1.CellEnter += FinalTermGrid_CellEnter;
            }
        }

        private static readonly string[] _mtEditableCols = { "colAttendance", "colRecitation", "colSeatwork", "colAssignment", "colLongTests", "colMajorExam" };
        private static readonly string[] _ftEditableCols = { "ftColAttendance", "ftColRecitation", "ftColSeatwork", "ftColAssignment", "ftColLongTests", "ftColMajorExam" };

        private void GridStudents_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (gridStudents == null || e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string colName = gridStudents.Columns[e.ColumnIndex].Name;
            bool isEditable = _mtEditableCols.Contains(colName);

            var cell = gridStudents.Rows[e.RowIndex].Cells[e.ColumnIndex];
            cell.Style.SelectionBackColor = isEditable
                ? Color.FromArgb(200, 220, 255)  // blue = "you can type here"
                : Color.FromArgb(255, 220, 50); // gold = read-only
            cell.Style.SelectionForeColor = Color.Black;
            gridStudents.Cursor = isEditable ? Cursors.IBeam : Cursors.Default;
            gridStudents.FirstDisplayedScrollingColumnIndex = Math.Max(0,
                e.ColumnIndex - 1);
        }

        private void FinalTermGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1 == null || e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            bool isEditable = _ftEditableCols.Contains(colName);

            var cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            cell.Style.SelectionBackColor = isEditable
                ? Color.FromArgb(200, 220, 255)   // blue = editable
                : Color.FromArgb(255, 220, 50); // gold = read-only
            cell.Style.SelectionForeColor = Color.Black;

            dataGridView1.Cursor = isEditable ? Cursors.IBeam : Cursors.Default;

            dataGridView1.FirstDisplayedScrollingColumnIndex = Math.Max(0, e.ColumnIndex - 1);
        }

        private void FinalTermGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox tb)
            {
                tb.KeyPress -= GradeCell_KeyPress;
                tb.TextChanged -= GradeCell_TextChanged;
                tb.KeyPress += GradeCell_KeyPress;
                tb.TextChanged += GradeCell_TextChanged;
            }
        }

        private void FinalTermGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridView1 == null) return;

            string[] editableCols = {
        "ftColAttendance", "ftColRecitation", "ftColSeatwork",
        "ftColAssignment", "ftColLongTests",  "ftColMajorExam"
    };
            string changedCol = dataGridView1.Columns[e.ColumnIndex].Name;
            if (!editableCols.Contains(changedCol)) return;

            var row = dataGridView1.Rows[e.RowIndex];
            string id = row.Cells["ftColStudentID"].Value?.ToString() ?? "";
            var record = _gradeRecords.FirstOrDefault(r => r.StudentID == id);
            if (record == null) return;

            double? att = ParseGradeCell(row.Cells["ftColAttendance"].Value);
            double? rec = ParseGradeCell(row.Cells["ftColRecitation"].Value);
            double? seat = ParseGradeCell(row.Cells["ftColSeatwork"].Value);
            double? asgn = ParseGradeCell(row.Cells["ftColAssignment"].Value);
            double? lt = ParseGradeCell(row.Cells["ftColLongTests"].Value);
            double? mex = ParseGradeCell(row.Cells["ftColMajorExam"].Value);

            void CorrectCell(string colName, double? parsed, object raw)
            {
                if (parsed.HasValue && raw != null &&
                    double.TryParse(raw.ToString(), out double rv) && rv != parsed.Value)
                {
                    dataGridView1.CellValueChanged -= FinalTermGrid_CellValueChanged;
                    row.Cells[colName].Value = parsed.Value.ToString("F0");
                    dataGridView1.CellValueChanged += FinalTermGrid_CellValueChanged;
                }
            }
            CorrectCell("ftColAttendance", att, row.Cells["ftColAttendance"].Value);
            CorrectCell("ftColRecitation", rec, row.Cells["ftColRecitation"].Value);
            CorrectCell("ftColSeatwork", seat, row.Cells["ftColSeatwork"].Value);
            CorrectCell("ftColAssignment", asgn, row.Cells["ftColAssignment"].Value);
            CorrectCell("ftColLongTests", lt, row.Cells["ftColLongTests"].Value);
            CorrectCell("ftColMajorExam", mex, row.Cells["ftColMajorExam"].Value);

            record.FT_Attendance = att; record.FT_Recitation = rec;
            record.FT_Seatwork = seat; record.FT_Assignment = asgn;
            record.FT_LongTests = lt; record.FT_MajorExam = mex;

            void ColorCell(string colName, double? v)
            {
                if (!v.HasValue) return;
                row.Cells[colName].Style.ForeColor =
                    v.Value < 75 ? Color.Firebrick :
                    v.Value >= 90 ? Color.FromArgb(16, 124, 65) :
                                    Color.Black;
            }
            ColorCell("ftColAttendance", att);
            ColorCell("ftColRecitation", rec);
            ColorCell("ftColSeatwork", seat);
            ColorCell("ftColAssignment", asgn);
            ColorCell("ftColLongTests", lt);
            ColorCell("ftColMajorExam", mex);

            double? termGrade = record.FinalTermGrade;

            if (termGrade.HasValue && dataGridView1.Columns.Contains("ftColTermGrade"))
            {
                bool pass = termGrade.Value >= 75;
                row.Cells["ftColTermGrade"].Value = termGrade.Value.ToString("F2");
                row.Cells["ftColTermGrade"].Style.ForeColor = pass ? Color.FromArgb(16, 124, 65) : Color.Firebrick;
                row.Cells["ftColTermGrade"].Style.BackColor = pass ? Color.FromArgb(230, 255, 230) : Color.FromArgb(255, 230, 230);
                row.Cells["ftColTermGrade"].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                row.Cells["ftColTermGrade"].Style.SelectionForeColor = row.Cells["ftColTermGrade"].Style.ForeColor;
                row.Cells["ftColTermGrade"].Style.SelectionBackColor = row.Cells["ftColTermGrade"].Style.BackColor;
            }
            else if (dataGridView1.Columns.Contains("ftColTermGrade"))
            {
                row.Cells["ftColTermGrade"].Value = "";
                row.Cells["ftColTermGrade"].Style.BackColor = Color.Empty;
                row.Cells["ftColTermGrade"].Style.ForeColor = Color.Black;
            }

            // ── Recalculate Final Grade (Midterm + FinalTerm) / 2
            if (record.FinalGrade.HasValue)
            {
                if (dataGridView1.Columns.Contains("ftColFinalGrade"))
                {
                    row.Cells["ftColFinalGrade"].Value = record.FinalGrade.Value.ToString("F2");
                    row.Cells["ftColFinalGrade"].Style.ForeColor = record.FinalGrade.Value >= 75
                        ? Color.FromArgb(16, 124, 65) : Color.Firebrick;
                    row.Cells["ftColFinalGrade"].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                }

                var (grade, desc) = GetPupGrade(record.FinalGrade);
                if (dataGridView1.Columns.Contains("ftColRemarks"))
                {
                    row.Cells["ftColRemarks"].Value = grade;
                    row.Cells["ftColRemarks"].ToolTipText = desc;
                    row.Cells["ftColRemarks"].Style.ForeColor = GetPupGradeColor(grade);
                    row.Cells["ftColRemarks"].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                }


                bool allMT = record.MT_Attendance.HasValue && record.MT_Recitation.HasValue &&
                             record.MT_Seatwork.HasValue && record.MT_Assignment.HasValue &&
                             record.MT_LongTests.HasValue && record.MT_MajorExam.HasValue;
                bool allFT = record.FT_Attendance.HasValue && record.FT_Recitation.HasValue &&
                             record.FT_Seatwork.HasValue && record.FT_Assignment.HasValue &&
                             record.FT_LongTests.HasValue && record.FT_MajorExam.HasValue;

                if (allMT && allFT && dataGridView1.Columns.Contains("ftColStatus"))
                {
                    record.Status = "Submitted";
                    row.Cells["ftColStatus"].Value = "Submitted";
                    row.Cells["ftColStatus"].Style.ForeColor = Color.FromArgb(16, 124, 65);
                    row.Cells["ftColStatus"].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                }
            }
            else
            {
                // Not all FT scores filled yet — show INC / Pending
                if (dataGridView1.Columns.Contains("ftColFinalGrade"))
                    row.Cells["ftColFinalGrade"].Value = "";

                if (dataGridView1.Columns.Contains("ftColRemarks"))
                {
                    row.Cells["ftColRemarks"].Value = "INC";
                    row.Cells["ftColRemarks"].ToolTipText = "Incomplete";
                    row.Cells["ftColRemarks"].Style.ForeColor = Color.Gray;
                }

                if (dataGridView1.Columns.Contains("ftColStatus"))
                {
                    row.Cells["ftColStatus"].Value = "Pending";
                    row.Cells["ftColStatus"].Style.ForeColor = Color.FromArgb(200, 100, 0);
                    row.Cells["ftColStatus"].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                }
            }
        }

        private void PopulateGradeManagement()
        {
            if (pnlGrades != null) pnlGrades.Dock = DockStyle.Fill;
            if (panel61 != null) panel61.Dock = DockStyle.Fill;

            if (panel168 != null)
                panel168.Dock = DockStyle.Top;

            if (panelFilters != null)
                panelFilters.Dock = DockStyle.Top;

            if (panelCardsContainer != null)
                panelCardsContainer.Dock = DockStyle.Top;

            if (txtSearch != null)
                txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            if (btnEditGradePercentage != null)
                btnEditGradePercentage.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            if (btnColumnOptions != null)
                btnColumnOptions.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            if (pnlBottomControls != null)
            {
                pnlBottomControls.Dock = DockStyle.Bottom;
                pnlBottomControls.Height = 200;
                pnlBottomControls.Padding = new Padding(4);
                pnlBottomControls.BackColor = Color.White;
            }

            if (panelBottomRow != null)
                panelBottomRow.Dock = DockStyle.Fill;
            if (pnlGradeRecords != null)
                pnlGradeRecords.Dock = DockStyle.Fill;

            if (tabControl1 != null)
                tabControl1.Dock = DockStyle.Fill;

            SetupGridStudentsColumns();

            if (gridStudents != null)
            {
                gridStudents.Dock = DockStyle.Fill;
                gridStudents.BackgroundColor = Color.White;
                gridStudents.Visible = true;
            }

            SetupFinalTermGrid();

            if (dataGridView1 != null)
            {
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.BackgroundColor = Color.White;
                dataGridView1.Visible = true;
            }

            SetupGradingScale();
            SetupLegend();
            UpdateStatCards(new List<StudentGradeRecord>());
            SetupColumnOptionsMenu();

            LoadGradeData();
        }

        private void SetupFinalTermGrid()
        {
            if (dataGridView1 == null) return;
            dataGridView1.Columns.Clear();
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(106, 0, 0);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersHeight = 46;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 220, 255);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;

            void AddCol(string name, string header, int fw, bool editable,
                        DataGridViewContentAlignment align = DataGridViewContentAlignment.MiddleCenter)
            {
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = name,
                    HeaderText = header,
                    ReadOnly = !editable,
                    MinimumWidth = 55,
                    FillWeight = fw,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    DefaultCellStyle = { Alignment = align },
                    HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                });
            }
            AddCol("ftColStudentID", "Student ID", 95, false, DataGridViewContentAlignment.MiddleLeft);
            AddCol("ftColStudentName", "Student Name", 160, false, DataGridViewContentAlignment.MiddleLeft);
            AddCol("ftColCourse", "Course", 60, false);
            AddCol("ftColAttendance", $"Attendance\n({CurrentWeights.AttendancePct}%)", 55, true);
            AddCol("ftColRecitation", $"Recitation\n({CurrentWeights.RecitationPct}%)", 55, true);
            AddCol("ftColSeatwork", $"Seatwork\n({CurrentWeights.SeatworkPct}%)", 55, true);
            AddCol("ftColAssignment", $"Assignment\n({CurrentWeights.AssignmentPct}%)", 60, true);
            AddCol("ftColLongTests", $"Long Tests\n({CurrentWeights.LongTestsPct}%)", 60, true);
            AddCol("ftColMajorExam", $"Major Exam\n({CurrentWeights.MajorExamsPct}%)", 65, true);
            AddCol("ftColTermGrade", "Term Grade", 65, false);
            AddCol("ftColFinalGrade", "Final Grade", 65, false);
            AddCol("ftColStatus", "Status", 60, false);
            AddCol("ftColRemarks", "Remarks", 60, false);


            dataGridView1.CellContentClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                if (dataGridView1.Columns[e.ColumnIndex].Name != "ftColActions") return;
                string id = dataGridView1.Rows[e.RowIndex].Cells["ftColStudentID"].Value?.ToString() ?? "";
                var record = _gradeRecords.FirstOrDefault(r => r.StudentID == id);
                if (record != null)
                {
                    bool prev = _showingMidterm;
                    _showingMidterm = false;
                    ShowEditGradeDialog(record);
                    _showingMidterm = prev;
                    RefreshFinalTermGrid();
                }
            };
        }

        private readonly List<CustomGradeColumn> _customMTColumns = new();
        private readonly List<CustomGradeColumn> _customFTColumns = new();

        public class CustomGradeColumn
        {
            public string ColName { get; set; } = "";
            public string Label { get; set; } = "";
            public string Category { get; set; } = "";
            public bool IsMidterm { get; set; } = true;
            public Dictionary<string, double?> Scores { get; } = new();
        }

        private void SetupColumnOptionsMenu()
        {
            if (btnColumnOptions == null) return;

            var ctx = new ContextMenuStrip();

            var miAdd = new ToolStripMenuItem("＋  Add Custom Grade Column…");
            miAdd.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            miAdd.ForeColor = Color.FromArgb(16, 124, 65);
            miAdd.Click += (s, e) => ShowAddCustomColumnDialog();
            ctx.Items.Add(miAdd);

            var miRemove = new ToolStripMenuItem("－  Remove Custom Column…");
            miRemove.ForeColor = Color.Firebrick;
            miRemove.Click += (s, e) => ShowRemoveCustomColumnDialog();
            ctx.Items.Add(miRemove);

            ctx.Items.Add(new ToolStripSeparator());

            var miVis = new ToolStripMenuItem("👁  Show / Hide Columns");
            miVis.Click += (s, e) => ShowHideColumnsDialog();
            ctx.Items.Add(miVis);

            btnColumnOptions.Click -= BtnColumnOptions_Click;
            btnColumnOptions.Click += (s, e) =>
                ctx.Show(btnColumnOptions, 0, btnColumnOptions.Height);
        }
        private void ShowHideColumnsDialog()
        {
            if (gridStudents == null) return;
            using var frm = new Form
            {
                Text = "Show / Hide Columns",
                Size = new Size(320, 400),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
            };
            var clb = new CheckedListBox { Dock = DockStyle.Fill, CheckOnClick = true };
            foreach (DataGridViewColumn col in gridStudents.Columns)
            {
                int i = clb.Items.Add(col.HeaderText.Replace("\n", " "));
                clb.SetItemChecked(i, col.Visible);
            }
            var btnOk = new Button { Text = "Apply", Dock = DockStyle.Bottom, Height = 32 };
            btnOk.Click += (s, _) =>
            {
                int ci = 0;
                foreach (DataGridViewColumn col in gridStudents.Columns)
                    col.Visible = clb.GetItemChecked(ci++);
                frm.Close();
            };
            frm.Controls.Add(clb);
            frm.Controls.Add(btnOk);
            frm.ShowDialog(this);
        }

        private void ShowAddCustomColumnDialog()
        {
            using var dlg = new Form
            {
                Text = "Add Custom Grade Column",
                Size = new Size(440, 310),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White,
            };

            var titleBar = new Panel { Dock = DockStyle.Top, Height = 42, BackColor = Color.FromArgb(106, 0, 0) };
            var titleLabel = new Label
            {
                Text = "Add Custom Grade Column",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0)
            };
            titleBar.Controls.Add(titleLabel);
            dlg.Controls.Add(titleBar);

            int y = 58;
            void Row(string lbl, Control ctrl)
            {
                dlg.Controls.Add(new Label
                {
                    Text = lbl,
                    Left = 20,
                    Top = y + 3,
                    Width = 130,
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold)
                });
                ctrl.Left = 160; ctrl.Top = y; ctrl.Width = 240;
                dlg.Controls.Add(ctrl);
                y += 38;
            }

            var tbLabel = new TextBox { PlaceholderText = "e.g. Quiz 1, Quiz 2, Project 1" };
            Row("Column Label:", tbLabel);

            var cmbTerm = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTerm.Items.AddRange(new object[] { "Midterm", "Final Term" });
            cmbTerm.SelectedIndex = _showingMidterm ? 0 : 1;
            Row("Term:", cmbTerm);

            var cmbCat = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCat.Items.AddRange(new object[]
            {
                "Seatwork / Short Quiz",
                "Long Test",
                "Assignment / Project",
                "Recitation",
                "Other"
            });
            cmbCat.SelectedIndex = 0;
            Row("Category:", cmbCat);

            var lblInfo = new Label
            {
                Left = 20,
                Top = y,
                Width = 380,
                Height = 32,
                AutoSize = false,
                Font = new Font("Segoe UI", 8f, FontStyle.Italic),
                ForeColor = Color.FromArgb(80, 80, 80),
                Text = "Tip: You can add multiple columns (Quiz 1, Quiz 2, etc.) and\nenter individual scores for each student."
            };
            dlg.Controls.Add(lblInfo);
            y += 36;

            var lblErr = new Label
            {
                Left = 20,
                Top = y,
                Width = 380,
                AutoSize = true,
                ForeColor = Color.Firebrick,
                Font = new Font("Segoe UI", 8.5f)
            };
            dlg.Controls.Add(lblErr);
            y += 22;

            var btnOk = new Button
            {
                Text = "Add Column",
                Left = 160,
                Top = y,
                Width = 120,
                Height = 30,
                BackColor = Color.FromArgb(106, 0, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnOk.FlatAppearance.BorderSize = 0;
            var btnCx = new Button
            {
                Text = "Cancel",
                Left = 288,
                Top = y,
                Width = 80,
                Height = 30,
                FlatStyle = FlatStyle.Flat
            };

            btnOk.Click += (s, _) =>
            {
                string lbl = tbLabel.Text.Trim();
                if (string.IsNullOrEmpty(lbl)) { lblErr.Text = "Please enter a column label."; return; }

                bool mt = cmbTerm.SelectedIndex == 0;
                var list = mt ? _customMTColumns : _customFTColumns;

                string key = (mt ? "mt_" : "ft_") +
                    System.Text.RegularExpressions.Regex.Replace(lbl.ToLower(), "[^a-z0-9]", "_");

                if (list.Any(c => c.ColName == key))
                { lblErr.Text = $"A column named \"{lbl}\" already exists for this term."; return; }

                list.Add(new CustomGradeColumn
                {
                    ColName = key,
                    Label = lbl,
                    Category = cmbCat.Text,
                    IsMidterm = mt
                });

                RebuildGridsWithCustomCols();
                dlg.DialogResult = DialogResult.OK;
            };
            btnCx.Click += (s, _) => dlg.Close();
            dlg.Controls.Add(btnOk);
            dlg.Controls.Add(btnCx);
            dlg.ShowDialog(this);
        }

        private void ShowRemoveCustomColumnDialog()
        {
            var all = _customMTColumns.Concat(_customFTColumns).ToList();
            if (all.Count == 0)
            {
                MessageBox.Show("No custom columns to remove.", "Remove Column",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using var dlg = new Form
            {
                Text = "Remove Custom Column",
                Size = new Size(360, 200),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White,
            };
            dlg.Controls.Add(new Label
            {
                Text = "Select column to remove:",
                Left = 16,
                Top = 16,
                AutoSize = true,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            });
            var cmb = new ComboBox { Left = 16, Top = 40, Width = 310, DropDownStyle = ComboBoxStyle.DropDownList };
            foreach (var c in all)
                cmb.Items.Add($"{c.Label}  [{(c.IsMidterm ? "Midterm" : "Final Term")}]");
            if (cmb.Items.Count > 0) cmb.SelectedIndex = 0;
            dlg.Controls.Add(cmb);

            var btnOk = new Button
            {
                Text = "Remove",
                Left = 160,
                Top = 120,
                Width = 80,
                Height = 30,
                BackColor = Color.Firebrick,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnOk.FlatAppearance.BorderSize = 0;
            var btnCx = new Button { Text = "Cancel", Left = 250, Top = 120, Width = 80, Height = 30, FlatStyle = FlatStyle.Flat };

            btnOk.Click += (s, _) =>
            {
                if (cmb.SelectedIndex < 0) return;
                var col = all[cmb.SelectedIndex];
                _customMTColumns.Remove(col);
                _customFTColumns.Remove(col);
                RebuildGridsWithCustomCols();
                dlg.DialogResult = DialogResult.OK;
            };
            btnCx.Click += (s, _) => dlg.Close();
            dlg.Controls.Add(btnOk);
            dlg.Controls.Add(btnCx);
            dlg.ShowDialog(this);
        }

        private void RebuildGridsWithCustomCols()
        {
            SetupGridStudentsColumns();
            AddCustomColumnsToGrid(gridStudents,
                _showingMidterm ? _customMTColumns : _customFTColumns);
            SetupFinalTermGrid();
            AddCustomColumnsToGrid(dataGridView1, _customFTColumns);
            RefreshGrid();
            RefreshFinalTermGrid();
        }

        private static void AddCustomColumnsToGrid(DataGridView dgv, List<CustomGradeColumn> cols)
        {
            if (dgv == null || cols == null) return;

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
                    HeaderText = cc.Label + "\n(" + cc.Category + ")",
                    ReadOnly = false,
                    MinimumWidth = 65,
                    FillWeight = 65,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter },
                    HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                    Tag = cc,
                };
                if (insertIdx < dgv.Columns.Count)
                    dgv.Columns.Insert(insertIdx++, col);
                else
                    dgv.Columns.Add(col);
            }
        }

        private bool _showingMidterm = true;

        //  SETUP GRID STUDENTS COLUMNS
        private void SetupGridStudentsColumns()
        {
            if (gridStudents == null) return;

            gridStudents.Columns.Clear();
            gridStudents.Dock = DockStyle.Fill;
            gridStudents.AutoGenerateColumns = false;
            gridStudents.AllowUserToAddRows = false;
            gridStudents.AllowUserToDeleteRows = false;
            gridStudents.RowHeadersVisible = false;
            gridStudents.SelectionMode = DataGridViewSelectionMode.CellSelect;
            gridStudents.MultiSelect = false;
            gridStudents.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            gridStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            gridStudents.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(106, 0, 0);
            gridStudents.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            gridStudents.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            gridStudents.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridStudents.EnableHeadersVisualStyles = false;
            gridStudents.ColumnHeadersHeight = 46;
            gridStudents.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 220, 255);
            gridStudents.DefaultCellStyle.SelectionForeColor = Color.Black;

            void AddCol(string name, string header, int fw, bool editable,
                        DataGridViewContentAlignment align = DataGridViewContentAlignment.MiddleCenter)
            {
                gridStudents.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = name,
                    HeaderText = header,
                    ReadOnly = !editable,
                    MinimumWidth = 55,
                    FillWeight = fw,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    DefaultCellStyle = { Alignment = align },
                    HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                });
            }

            AddCol("colStudentID", "Student ID", 95, false, DataGridViewContentAlignment.MiddleLeft);
            AddCol("colStudentName", "Student Name", 160, false, DataGridViewContentAlignment.MiddleLeft);
            AddCol("colCourse", "Course", 60, false);

            AddCol("colAttendance", $"Attendance\n({CurrentWeights.AttendancePct}%)", 55, true);
            AddCol("colRecitation", $"Recitation\n({CurrentWeights.RecitationPct}%)", 55, true);
            AddCol("colSeatwork", $"Seatwork\n({CurrentWeights.SeatworkPct}%)", 55, true);
            AddCol("colAssignment", $"Assignment\n({CurrentWeights.AssignmentPct}%)", 60, true);
            AddCol("colLongTests", $"Long Tests\n({CurrentWeights.LongTestsPct}%)", 60, true);

            AddCol("colMajorExam", $"Major Exam\n({CurrentWeights.MajorExamsPct}%)", 65, true);

            AddCol("colTermGrade", "Term Grade", 65, false);
            AddCol("colFinalGrade", "Final Grade", 65, false);
            AddCol("colStatus", "Status", 60, false);
            AddCol("colRemarks", "Remarks", 60, false);

        }

        private void ShowEditGradePercentageDialog()
        {
            using var dlg = new EditGradePercentageControl(CurrentWeights);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                CurrentWeights = dlg.UpdatedWeights;

                void UpdHeader(string name, string header)
                {
                    if (gridStudents?.Columns[name] != null)
                        gridStudents.Columns[name].HeaderText = header;
                }
                UpdHeader("colAttendance", $"Attendance\n({CurrentWeights.AttendancePct}%)");
                UpdHeader("colRecitation", $"Recitation\n({CurrentWeights.RecitationPct}%)");
                UpdHeader("colSeatwork", $"Seatwork\n({CurrentWeights.SeatworkPct}%)");
                UpdHeader("colAssignment", $"Assignment\n({CurrentWeights.AssignmentPct}%)");
                UpdHeader("colLongTests", $"Long Tests\n({CurrentWeights.LongTestsPct}%)");
                UpdHeader("colMajorExam", $"Major Exam\n({CurrentWeights.MajorExamsPct}%)");

                CommitGridEdits();
                RefreshGrid();
                UpdateStatCards(_filteredRecords);
            }
        }

        //  GRADING SCALE + LEGEND
        private static void AddGridCol(DataGridView dgv, string name, string header, int width, bool editable)
        {
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                Width = width,
                ReadOnly = !editable,
            });
        }

        private void SetupGradingScale()
        {
            if (gridGradingScale == null) return;

            gridGradingScale.Rows.Clear();
            gridGradingScale.AllowUserToAddRows = false;
            gridGradingScale.RowHeadersVisible = false;
            gridGradingScale.BackgroundColor = Color.White;
            gridGradingScale.BorderStyle = BorderStyle.None;
            gridGradingScale.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridGradingScale.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 246, 248);
            gridGradingScale.DefaultCellStyle.SelectionForeColor = Color.Black;

            // (range text, description text, row fore-color, row back-color)
            var rows = new[]
            {
        ("97 – 100",  "1.00 – Excellent",     Color.FromArgb(16, 124, 65),  Color.FromArgb(230, 255, 230)),
        ("94 – 96",   "1.25 – Excellent",     Color.FromArgb(16, 124, 65),  Color.FromArgb(230, 255, 230)),
        ("91 – 93",   "1.50 – Very Good",     Color.FromArgb(16, 124, 65),  Color.FromArgb(230, 255, 230)),
        ("88 – 90",   "1.75 – Very Good",     Color.FromArgb(0,  112, 192), Color.FromArgb(225, 240, 255)),
        ("85 – 87",   "2.00 – Good",          Color.FromArgb(0,  112, 192), Color.FromArgb(225, 240, 255)),
        ("82 – 84",   "2.25 – Good",          Color.FromArgb(0,  112, 192), Color.FromArgb(225, 240, 255)),
        ("79 – 81",   "2.50 – Satisfactory",  Color.FromArgb(180, 120, 0),  Color.FromArgb(255, 245, 220)),
        ("76 – 78",   "2.75 – Satisfactory",  Color.FromArgb(180, 120, 0),  Color.FromArgb(255, 245, 220)),
        ("75",        "3.00 – Passing",        Color.FromArgb(180, 120, 0),  Color.FromArgb(255, 245, 220)),
        ("Below 75",  "5.00 – Failed",         Color.Firebrick,               Color.FromArgb(255, 230, 230)),
    };

            foreach (var (range, desc, fore, back) in rows)
            {
                int idx = gridGradingScale.Rows.Add(range, desc);
                var row = gridGradingScale.Rows[idx];
                row.DefaultCellStyle.ForeColor = fore;
                row.DefaultCellStyle.BackColor = back;
                row.DefaultCellStyle.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                row.DefaultCellStyle.SelectionForeColor = fore;
                row.DefaultCellStyle.SelectionBackColor = back;
            }
        }

        private void SetupLegend()
        {
            if (panelLegend == null) return;
            panelLegend.Controls.Clear();

            //  Title 
            panelLegend.Controls.Add(new Label
            {
                Text = "Legend",
                Font = new Font("Segoe UI", 9.75f, FontStyle.Bold),
                Location = new Point(14, 4),
                AutoSize = true,
                ForeColor = Color.Black,
            });

            //  Items: (dot color, label text) 
            var items = new (Color color, string text)[]
            {
        (Color.FromArgb(16,  124, 65),  "1.00 – 1.50  Excellent / Very Good"),
        (Color.FromArgb(0,   112, 192), "1.75 – 2.25  Good"),
        (Color.FromArgb(180, 120, 0),   "2.50 – 3.00  Satisfactory / Passing"),
        (Color.Firebrick,               "5.00  Failed"),
        (Color.FromArgb(128, 128, 128), "INC   Incomplete"),
        (Color.FromArgb(16,  124, 65),  "Submitted"),
        (Color.FromArgb(200, 100, 0),   "Pending"),
            };

            int dotSize = 13;
            int y = 26;
            int spacing = 21;

            foreach (var (color, text) in items)
            {
                // Colored square dot
                panelLegend.Controls.Add(new Panel
                {
                    Width = dotSize,
                    Height = dotSize,
                    BackColor = color,
                    Location = new Point(18, y + 1),
                });

                // Label text next to dot
                panelLegend.Controls.Add(new Label
                {
                    Text = text,
                    Location = new Point(38, y),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = Color.FromArgb(40, 40, 40),
                });

                y += spacing;
            }
        }

        //  STAT CARDS
        private void UpdateStatCards(List<StudentGradeRecord> records)
        {
            int total = records.Count;
            int submitted = records.Count(r => r.Status == "Submitted");
            int pending = records.Count(r => r.Status == "Pending");
            double avg = records.Where(r => r.FinalGrade.HasValue).Select(r => r.FinalGrade!.Value).DefaultIfEmpty(0).Average();
            double highest = records.Where(r => r.FinalGrade.HasValue).Select(r => r.FinalGrade!.Value).DefaultIfEmpty(0).Max();

            SafeSetLabel(lblTotalVal, total.ToString());
            SafeSetLabel(lblSubmittedVal, $"{submitted} ({(total > 0 ? submitted * 100.0 / total : 0):F2}%)");
            SafeSetLabel(lblPendingVal, $"{pending} ({(total > 0 ? pending * 100.0 / total : 0):F2}%)");
            SafeSetLabel(lblAvgVal, total > 0 ? avg.ToString("F2") : "—");
            SafeSetLabel(lblHighestVal, total > 0 ? highest.ToString("F2") : "—");
        }

        private static void SafeSetLabel(Label lbl, string text) { if (lbl != null) lbl.Text = text; }

        //  SEED DATA
        private List<StudentGradeRecord> BuildSeedData() => new List<StudentGradeRecord>
        {
            new() { StudentID="2024-00001-SM-0", Name="Ablong, Adrian P.",          Course="IT 101",
                MT_Attendance=90, MT_Recitation=85, MT_Seatwork=88, MT_Assignment=87, MT_LongTests=84, MT_MajorExam=86,
                FT_Attendance=88, FT_Recitation=90, FT_Seatwork=85, FT_Assignment=89, FT_LongTests=88, FT_MajorExam=90, Status="Submitted" },
            new() { StudentID="2024-00002-SM-0", Name="Alcaiz, Jared B.",           Course="IT 101",
                MT_Attendance=78, MT_Recitation=72, MT_Seatwork=74, MT_Assignment=70, MT_LongTests=73, MT_MajorExam=75,
                FT_Attendance=80, FT_Recitation=75, FT_Seatwork=76, FT_Assignment=72, FT_LongTests=78, FT_MajorExam=74, Status="Submitted" },
            new() { StudentID="2024-00003-SM-0", Name="Amar, Charls Manuel C.",     Course="IT 101",
                MT_Attendance=95, MT_Recitation=93, MT_Seatwork=94, MT_Assignment=92, MT_LongTests=95, MT_MajorExam=93,
                FT_Attendance=96, FT_Recitation=95, FT_Seatwork=93, FT_Assignment=94, FT_LongTests=96, FT_MajorExam=94, Status="Submitted" },
            new() { StudentID="2024-00004-SM-0", Name="Amen, Jessie C.",            Course="IT 101",
                MT_Attendance=60, MT_Recitation=58, MT_Seatwork=62, MT_Assignment=55, MT_LongTests=60, MT_MajorExam=57,
                FT_Attendance=62, FT_Recitation=60, FT_Seatwork=63, FT_Assignment=58, FT_LongTests=61, FT_MajorExam=59, Status="Submitted" },
            new() { StudentID="2024-00005-SM-0", Name="Amolata, Jhayphee V.",       Course="IT 101",
                MT_Attendance=82, MT_Recitation=80, MT_Seatwork=83, MT_Assignment=79, MT_LongTests=81, MT_MajorExam=82,
                FT_Attendance=84, FT_Recitation=82, FT_Seatwork=80, FT_Assignment=83, FT_LongTests=83, FT_MajorExam=81, Status="Submitted" },
            new() { StudentID="2024-00006-SM-0", Name="Antillon, Reijn Cyrille A.", Course="IT 101",
                MT_Attendance=70, MT_Recitation=68, MT_Seatwork=71, MT_Assignment=67, MT_LongTests=69, MT_MajorExam=70,
                FT_Attendance=72, FT_Recitation=70, FT_Seatwork=69, FT_Assignment=71, FT_LongTests=70, FT_MajorExam=68, Status="Submitted" },
            new() { StudentID="2024-00007-SM-0", Name="Armada, Trisha Mariel D.",   Course="IT 101",
                MT_Attendance=98, MT_Recitation=96, MT_Seatwork=97, MT_Assignment=95, MT_LongTests=97, MT_MajorExam=96,
                FT_Attendance=99, FT_Recitation=97, FT_Seatwork=98, FT_Assignment=96, FT_LongTests=98, FT_MajorExam=97, Status="Submitted" },
            new() { StudentID="2024-00008-SM-0", Name="Asay, Claire Jade A.",       Course="IT 101",
                MT_Attendance=87, MT_Recitation=85, MT_Seatwork=86, MT_Assignment=84, MT_LongTests=86, MT_MajorExam=85,
                FT_Attendance=88, FT_Recitation=87, FT_Seatwork=85, FT_Assignment=86, FT_LongTests=87, FT_MajorExam=86, Status="Submitted" },
            new() { StudentID="2024-00009-SM-0",  Name="Banting, Andrei Justine L.", Course="IT 101", Status="Pending" },
            new() { StudentID="2024-00010-SM-0",  Name="Basilan, Hans Louie L.",     Course="IT 101", Status="Pending" },
            new() { StudentID="2024-00011-SM-0",  Name="Bauit, Clerkjustine N.",     Course="IT 101", Status="Pending" },
            new() { StudentID="2024-00012-SM-0",  Name="Celestino, Randel E.",       Course="IT 101", Status="Pending" },
            new() { StudentID="2024-00013-SM-0",  Name="Cruz, Alexander L.",         Course="IT 101", Status="Pending" },
            new() { StudentID="2024-00014-SM-0",  Name="Dela Cruz, Karl Angelo R.",  Course="IT 101", Status="Pending" },
            new() { StudentID="2024-00015-SM-0",  Name="De Guzman, Lyzlie Anne G.",  Course="IT 101", Status="Pending" },
            new() { StudentID="2024-00016-SM-0", Name="Dizon, Patricia Mae S.",     Course="CS 102",
                MT_Attendance=92, MT_Recitation=90, MT_Seatwork=91, MT_Assignment=89, MT_LongTests=91, MT_MajorExam=90,
                FT_Attendance=93, FT_Recitation=91, FT_Seatwork=92, FT_Assignment=90, FT_LongTests=92, FT_MajorExam=91, Status="Submitted" },
            new() { StudentID="2024-00017-SM-0", Name="Espinosa, Marco Luis T.",    Course="CS 102",
                MT_Attendance=77, MT_Recitation=75, MT_Seatwork=78, MT_Assignment=74, MT_LongTests=76, MT_MajorExam=77,
                FT_Attendance=79, FT_Recitation=77, FT_Seatwork=76, FT_Assignment=75, FT_LongTests=78, FT_MajorExam=76, Status="Submitted" },
            new() { StudentID="2024-00018-SM-0", Name="Flores, Donna Christine R.", Course="CS 102",
                MT_Attendance=84, MT_Recitation=83, MT_Seatwork=85, MT_Assignment=82, MT_LongTests=84, MT_MajorExam=84,
                FT_Attendance=86, FT_Recitation=84, FT_Seatwork=83, FT_Assignment=85, FT_LongTests=85, FT_MajorExam=85, Status="Submitted" },
            new() { StudentID="2024-00019-SM-0", Name="Garcia, Jose Miguel A.",     Course="CS 102",
                MT_Attendance=63, MT_Recitation=61, MT_Seatwork=64, MT_Assignment=60, MT_LongTests=62, MT_MajorExam=61,
                FT_Attendance=65, FT_Recitation=63, FT_Seatwork=62, FT_Assignment=64, FT_LongTests=63, FT_MajorExam=62, Status="Submitted" },
            new() { StudentID="2024-00020-SM-0", Name="Hernandez, Carina B.",       Course="CS 102",
                MT_Attendance=98, MT_Recitation=97, MT_Seatwork=99, MT_Assignment=96, MT_LongTests=98, MT_MajorExam=97,
                FT_Attendance=99, FT_Recitation=98, FT_Seatwork=98, FT_Assignment=97, FT_LongTests=99, FT_MajorExam=98, Status="Submitted" },
            new() { StudentID="2024-00021-SM-0", Name="Ilagan, Francis John M.",    Course="CS 102",
                MT_Attendance=79, MT_Recitation=77, MT_Seatwork=80, MT_Assignment=76, MT_LongTests=78, MT_MajorExam=79,
                FT_Attendance=81, FT_Recitation=79, FT_Seatwork=78, FT_Assignment=80, FT_LongTests=80, FT_MajorExam=77, Status="Submitted" },
            new() { StudentID="2024-00022-SM-0", Name="Javier, Lorraine C.",        Course="CS 102",
                MT_Attendance=56, MT_Recitation=54, MT_Seatwork=57, MT_Assignment=53, MT_LongTests=55, MT_MajorExam=54,
                FT_Attendance=58, FT_Recitation=56, FT_Seatwork=55, FT_Assignment=57, FT_LongTests=57, FT_MajorExam=55, Status="Submitted" },
            new() { StudentID="2024-00023-SM-0", Name="Lim, Kevin Paul D.",         Course="CS 102", Status="Pending" },
            new() { StudentID="2024-00024-SM-0", Name="Lopez, Angela Rose V.",      Course="CS 102", Status="Pending" },
            new() { StudentID="2024-00025-SM-0", Name="Manalo, Renz Gabriel P.",    Course="CS 102", Status="Pending" },
            new() { StudentID="2024-00026-SM-0", Name="Mendoza, Sheila Marie F.",   Course="CS 102", Status="Pending" },
            new() { StudentID="2024-00027-SM-0", Name="Navarro, Danilo Jr. C.",     Course="CS 102", Status="Pending" },
            new() { StudentID="2024-00028-SM-0", Name="Ocampo, Kristine Joy A.",    Course="IS 103",
                MT_Attendance=90, MT_Recitation=88, MT_Seatwork=91, MT_Assignment=87, MT_LongTests=90, MT_MajorExam=89,
                FT_Attendance=92, FT_Recitation=90, FT_Seatwork=89, FT_Assignment=91, FT_LongTests=91, FT_MajorExam=90, Status="Submitted" },
            new() { StudentID="2024-00029-SM-0", Name="Panganiban, Jose Luis R.",   Course="IS 103",
                MT_Attendance=73, MT_Recitation=71, MT_Seatwork=74, MT_Assignment=70, MT_LongTests=72, MT_MajorExam=72,
                FT_Attendance=75, FT_Recitation=73, FT_Seatwork=72, FT_Assignment=74, FT_LongTests=74, FT_MajorExam=73, Status="Submitted" },
            new() { StudentID="2024-00030-SM-0", Name="Quiambao, Rachel Ann T.",    Course="IS 103",
                MT_Attendance=95, MT_Recitation=94, MT_Seatwork=96, MT_Assignment=93, MT_LongTests=95, MT_MajorExam=95,
                FT_Attendance=97, FT_Recitation=95, FT_Seatwork=94, FT_Assignment=96, FT_LongTests=96, FT_MajorExam=96, Status="Submitted" },
            new() { StudentID="2024-00031-SM-0", Name="Ramos, Enrico Santos B.",    Course="IS 103",
                MT_Attendance=62, MT_Recitation=60, MT_Seatwork=63, MT_Assignment=59, MT_LongTests=61, MT_MajorExam=60,
                FT_Attendance=64, FT_Recitation=62, FT_Seatwork=61, FT_Assignment=63, FT_LongTests=63, FT_MajorExam=61, Status="Submitted" },
            new() { StudentID="2024-00032-SM-0", Name="Reyes, Maria Clara O.",      Course="IS 103",
                MT_Attendance=88, MT_Recitation=86, MT_Seatwork=89, MT_Assignment=85, MT_LongTests=87, MT_MajorExam=87,
                FT_Attendance=90, FT_Recitation=88, FT_Seatwork=87, FT_Assignment=89, FT_LongTests=89, FT_MajorExam=88, Status="Submitted" },
            new() { StudentID="2024-00033-SM-0", Name="Santos, Bianca Nicole P.",   Course="IS 103",
                MT_Attendance=80, MT_Recitation=78, MT_Seatwork=81, MT_Assignment=77, MT_LongTests=79, MT_MajorExam=79,
                FT_Attendance=82, FT_Recitation=80, FT_Seatwork=79, FT_Assignment=81, FT_LongTests=81, FT_MajorExam=80, Status="Submitted" },
            new() { StudentID="2024-00034-SM-0", Name="Soriano, Aaron James V.",    Course="IS 103", Status="Pending" },
            new() { StudentID="2024-00035-SM-0", Name="Tan, Melissa Grace C.",      Course="IS 103", Status="Pending" },
            new() { StudentID="2024-00036-SM-0", Name="Torres, Bryan Kyle M.",      Course="IS 103", Status="Pending" },
            new() { StudentID="2024-00037-SM-0", Name="Valdez, Camille Ann S.",     Course="IS 103", Status="Pending" },
            new() { StudentID="2024-00038-SM-0", Name="Villanueva, Rico Dante B.",  Course="IS 103", Status="Pending" },
        };

        //  LOAD / FILTER / REFRESH
        private void LoadGradeData()
        {
            _gradeRecords = BuildSeedData();
            _filteredRecords = new List<StudentGradeRecord>(_gradeRecords);
            _gradesLoaded = true;
            RefreshGrid();
            UpdateStatCards(_filteredRecords);

        }

        private void FilterGradeData(string query)
        {
            query = query?.Trim().ToLower() ?? "";
            string courseFilter = "";
            if (cmbCourseSection != null && cmbCourseSection.SelectedIndex > 0)
            {
                string sel = cmbCourseSection.SelectedItem?.ToString() ?? "";
                int pipeIdx = sel.IndexOf('|');
                string coursePart = pipeIdx > 0 ? sel.Substring(0, pipeIdx).Trim() : sel;
                int dashIdx = coursePart.IndexOf('-');
                courseFilter = dashIdx > 0
                    ? coursePart.Substring(0, dashIdx).Trim().ToLower()
                    : coursePart.ToLower();
            }

            _filteredRecords = _gradeRecords.Where(r =>
            {
                bool matchesCourse = string.IsNullOrEmpty(courseFilter)
                    || r.Course.ToLower().StartsWith(courseFilter);
                bool matchesSearch = string.IsNullOrEmpty(query)
                    || r.StudentID.ToLower().Contains(query)
                    || r.Name.ToLower().Contains(query)
                    || r.Course.ToLower().Contains(query);
                return matchesCourse && matchesSearch;
            }).ToList();

            RefreshGrid();
            UpdateStatCards(_filteredRecords);
        }

        private void RefreshGrid()
        {
            if (gridStudents == null) return;
            gridStudents.Rows.Clear();
            gridStudents.SuspendLayout();
            bool mt = _showingMidterm;

            foreach (var r in _filteredRecords)
            {
                double? att = mt ? r.MT_Attendance : r.FT_Attendance;
                double? rec = mt ? r.MT_Recitation : r.FT_Recitation;
                double? seat = mt ? r.MT_Seatwork : r.FT_Seatwork;
                double? asgn = mt ? r.MT_Assignment : r.FT_Assignment;
                double? lt = mt ? r.MT_LongTests : r.FT_LongTests;
                double? mex = mt ? r.MT_MajorExam : r.FT_MajorExam;
                double? tg = mt ? r.MidtermGrade : r.FinalTermGrade;

                string F(double? v) => v.HasValue ? v.Value.ToString("F0") : "";
                string FG(double? v) => v.HasValue ? v.Value.ToString("F2") : "";

                int idx = gridStudents.Rows.Add(
                     r.StudentID, r.Name, r.Course,
                     F(att), F(rec), F(seat), F(asgn), F(lt), F(mex),
                     FG(tg), FG(r.FinalGrade), r.Status, r.Remarks);

                var row = gridStudents.Rows[idx];

                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.ForeColor = Color.Black;
                    cell.Style.BackColor = Color.Empty;
                    cell.Style.Font = null;
                }

                foreach (string colName in new[] { "colAttendance","colRecitation","colSeatwork",
                                    "colAssignment","colLongTests","colMajorExam" })
                {
                    var cell = row.Cells[colName];
                    if (double.TryParse(cell.Value?.ToString(), out double cv))
                    {
                        if (cv < 75) cell.Style.ForeColor = Color.Firebrick;
                        else if (cv >= 90) cell.Style.ForeColor = Color.FromArgb(16, 124, 65);
                        else cell.Style.ForeColor = Color.Black;
                    }
                    else
                    {
                        cell.Style.ForeColor = Color.Black;
                    }
                }

                if (tg.HasValue)
                {
                    bool pass = tg.Value >= 75;
                    row.Cells["colTermGrade"].Style.BackColor = pass ? Color.FromArgb(230, 255, 230) : Color.FromArgb(255, 230, 230);
                    row.Cells["colTermGrade"].Style.ForeColor = pass ? Color.FromArgb(16, 124, 65) : Color.Firebrick;
                    row.Cells["colTermGrade"].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                }
                if (r.FinalGrade.HasValue)
                {
                    row.Cells["colFinalGrade"].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                    row.Cells["colFinalGrade"].Style.ForeColor = r.FinalGrade.Value >= 75
                        ? Color.FromArgb(16, 124, 65) : Color.Firebrick;
                }
                row.Cells["colStatus"].Style.ForeColor = r.Status == "Submitted"
                    ? Color.FromArgb(16, 124, 65) : Color.FromArgb(200, 100, 0);
                row.Cells["colStatus"].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                row.Cells["colRemarks"].Style.ForeColor = GetPupGradeColor(r.Remarks);
                row.Cells["colRemarks"].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                row.Cells["colRemarks"].ToolTipText = r.RemarksDescription;
            }
            gridStudents.ResumeLayout();
        }

        private void RefreshFinalTermGrid()
        {
            if (dataGridView1 == null) return;
            dataGridView1.Rows.Clear();
            dataGridView1.SuspendLayout();

            foreach (var r in _filteredRecords)
            {
                string F(double? v) => v.HasValue ? v.Value.ToString("F0") : "";
                string FG(double? v) => v.HasValue ? v.Value.ToString("F2") : "";

                int idx = dataGridView1.Rows.Add(
                    r.StudentID, r.Name, r.Course,
                    F(r.FT_Attendance), F(r.FT_Recitation), F(r.FT_Seatwork),
                    F(r.FT_Assignment), F(r.FT_LongTests), F(r.FT_MajorExam),
                    FG(r.FinalTermGrade), FG(r.FinalGrade), r.Status, r.Remarks);

                var row = dataGridView1.Rows[idx];

                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.ForeColor = Color.Black;
                    cell.Style.BackColor = Color.Empty;
                    cell.Style.Font = null;
                }

                foreach (string colName in new[] { "ftColAttendance","ftColRecitation","ftColSeatwork",
                                    "ftColAssignment","ftColLongTests","ftColMajorExam" })
                {
                    if (!dataGridView1.Columns.Contains(colName)) continue;
                    var cell = row.Cells[colName];
                    if (double.TryParse(cell.Value?.ToString(), out double cv))
                    {
                        if (cv < 75) cell.Style.ForeColor = Color.Firebrick;
                        else if (cv >= 90) cell.Style.ForeColor = Color.FromArgb(16, 124, 65);
                        else cell.Style.ForeColor = Color.Black;
                    }
                    else { cell.Style.ForeColor = Color.Black; }
                }

                if (r.FinalTermGrade.HasValue && dataGridView1.Columns.Contains("ftColTermGrade"))
                {
                    bool pass = r.FinalTermGrade.Value >= 75;
                    row.Cells["ftColTermGrade"].Style.BackColor = pass ? Color.FromArgb(230, 255, 230) : Color.FromArgb(255, 230, 230);
                    row.Cells["ftColTermGrade"].Style.ForeColor = pass ? Color.FromArgb(16, 124, 65) : Color.Firebrick;
                    row.Cells["ftColTermGrade"].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                }
                if (r.FinalGrade.HasValue && dataGridView1.Columns.Contains("ftColFinalGrade"))
                {
                    row.Cells["ftColFinalGrade"].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                    row.Cells["ftColFinalGrade"].Style.ForeColor = r.FinalGrade.Value >= 75
                        ? Color.FromArgb(16, 124, 65) : Color.Firebrick;
                }
                if (dataGridView1.Columns.Contains("ftColStatus"))
                {
                    row.Cells["ftColStatus"].Style.ForeColor = r.Status == "Submitted"
                        ? Color.FromArgb(16, 124, 65) : Color.FromArgb(200, 100, 0);
                    row.Cells["ftColStatus"].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                }
                if (dataGridView1.Columns.Contains("ftColRemarks"))
                {
                    row.Cells["ftColRemarks"].Style.ForeColor = GetPupGradeColor(r.Remarks);
                    row.Cells["ftColRemarks"].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                    row.Cells["ftColRemarks"].ToolTipText = r.RemarksDescription;
                }
            }
            dataGridView1.ResumeLayout();
        }

        //  GRID EVENT HANDLERS
        private void CmbCourseSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_gradesLoaded) FilterGradeData(txtSearch?.Text ?? "");
        }

        private void TxtGradeSearch_TextChanged(object sender, EventArgs e)
        {
            FilterGradeData(txtSearch?.Text ?? "");
        }

        private void BtnLoadStudents_Click(object sender, EventArgs e)
        {
            if (cmbCourseSection == null || cmbCourseSection.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a Course & Section first.", "Load Students",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            LoadGradeData();
            MessageBox.Show($"{_gradeRecords.Count} students loaded successfully.",
                "Load Students", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnInputGrades_Click(object sender, EventArgs e)
        {
            if (!_gradesLoaded)
            {
                MessageBox.Show("Please load students first.", "Input Grades",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (gridStudents != null)
                gridStudents.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;

            MessageBox.Show(
                "Grade input mode enabled.\n\n" +
                "• Double-click or press F2 on a grade cell to edit.\n" +
                "• Editable columns: Attendance, Recitation, Seatwork,\n" +
                "  Assignment, Long Tests, Major Exam.\n" +
                "• Custom columns (Quiz 1, Quiz 2, etc.) are also editable.\n" +
                "• Term Grade and Final Grade are calculated automatically.\n\n" +
                "Formula:  FINAL GRADE = (Midterm Grade + Final Term Grade) / 2\n" +
                "Term Grade = Class Standing (70%) + Major Exam (30%)\n\n" +
                "Click 'Save Changes' when done.",
                "Input Grades", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnClearGradeFilters_Click(object sender, EventArgs e)
        {
            if (cmbCourseSection != null) cmbCourseSection.SelectedIndex = -1;
            if (txtSearch != null) txtSearch.Clear();
            if (_gradesLoaded)
            {
                _filteredRecords = new List<StudentGradeRecord>(_gradeRecords);
                RefreshGrid();
                UpdateStatCards(_filteredRecords);
            }
        }

        private void BtnReleaseGrades_Click(object sender, EventArgs e)
        {
            if (!_gradesLoaded)
            {
                MessageBox.Show("No grades to release. Load students first.", "Release Grades",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int unreleased = _gradeRecords.Count(r => r.Status == "Submitted" && !r.Released);
            if (unreleased == 0)
            {
                MessageBox.Show("All submitted grades have already been released.", "Release Grades",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var res = MessageBox.Show(
                $"You are about to release grades for {unreleased} student(s).\n\nStudents will be notified. This action cannot be undone.\n\nProceed?",
                "Release Grades", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                foreach (var r in _gradeRecords.Where(r => r.Status == "Submitted"))
                    r.Released = true;
                RefreshGrid();
                MessageBox.Show("Grades released successfully!", "Release Grades",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnSaveRecord_Click(object sender, EventArgs e) => SaveGradeChanges();
        private void BtnSaveChanges_Click(object sender, EventArgs e) => SaveGradeChanges();

        private void SaveGradeChanges()
        {
            if (!_gradesLoaded)
            {
                MessageBox.Show("Nothing to save yet. Load students first.", "Save",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            CommitGridEdits();
            RefreshGrid();
            RefreshFinalTermGrid();
            UpdateStatCards(_filteredRecords);
            MessageBox.Show("Changes saved successfully!", "Save",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CommitGridEdits()
        {
            if (gridStudents == null) return;
            gridStudents.CommitEdit(DataGridViewDataErrorContexts.Commit);
            gridStudents.EndEdit();
            bool mt = _showingMidterm;

            foreach (DataGridViewRow row in gridStudents.Rows)
            {
                if (row.IsNewRow) continue;
                string id = row.Cells["colStudentID"].Value?.ToString() ?? "";
                var record = _gradeRecords.FirstOrDefault(r => r.StudentID == id);
                if (record == null) continue;

                double? att = ParseGradeCell(row.Cells["colAttendance"].Value);
                double? rec = ParseGradeCell(row.Cells["colRecitation"].Value);
                double? seat = ParseGradeCell(row.Cells["colSeatwork"].Value);
                double? asgn = ParseGradeCell(row.Cells["colAssignment"].Value);
                double? lt = ParseGradeCell(row.Cells["colLongTests"].Value);
                double? mex = ParseGradeCell(row.Cells["colMajorExam"].Value);

                if (mt)
                {
                    record.MT_Attendance = att; record.MT_Recitation = rec;
                    record.MT_Seatwork = seat; record.MT_Assignment = asgn;
                    record.MT_LongTests = lt; record.MT_MajorExam = mex;

                    foreach (var cc in _customMTColumns)
                        if (gridStudents.Columns.Contains(cc.ColName))
                            cc.Scores[id] = ParseGradeCell(row.Cells[cc.ColName].Value);
                }
                else
                {
                    record.FT_Attendance = att; record.FT_Recitation = rec;
                    record.FT_Seatwork = seat; record.FT_Assignment = asgn;
                    record.FT_LongTests = lt; record.FT_MajorExam = mex;

                    foreach (var cc in _customFTColumns)
                        if (gridStudents.Columns.Contains(cc.ColName))
                            cc.Scores[id] = ParseGradeCell(row.Cells[cc.ColName].Value);
                }

                bool allMT = record.MT_Attendance.HasValue && record.MT_Recitation.HasValue &&
                             record.MT_Seatwork.HasValue && record.MT_Assignment.HasValue &&
                             record.MT_LongTests.HasValue && record.MT_MajorExam.HasValue;
                bool allFT = record.FT_Attendance.HasValue && record.FT_Recitation.HasValue &&
                             record.FT_Seatwork.HasValue && record.FT_Assignment.HasValue &&
                             record.FT_LongTests.HasValue && record.FT_MajorExam.HasValue;
                if (allMT && allFT) record.Status = "Submitted";
            }
        }

        private static double? ParseGradeCell(object val)
        {
            if (val == null || string.IsNullOrWhiteSpace(val.ToString())) return null;
            if (!double.TryParse(val.ToString(), out double d)) return null;
            if (d < 0) d = 0;
            if (d > 100) d = 100;
            return d;
        }

        //  COLUMN OPTIONS (legacy fallback handler)
        private void BtnColumnOptions_Click(object sender, EventArgs e)
        {

        }

        //  EXPORT TO CSV
        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            if (!_gradesLoaded || _filteredRecords.Count == 0)
            {
                MessageBox.Show("No data to export. Load students first.", "Export to Excel",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            CommitGridEdits();

            using var sfd = new SaveFileDialog
            {
                Filter = "CSV File (*.csv)|*.csv|All Files (*.*)|*.*",
                FileName = $"Grades_{DateTime.Now:yyyyMMdd_HHmm}.csv",
                Title = "Export Grades",
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                var sb = new StringBuilder();
                sb.AppendLine("StudentID,Name,Course," +
                    "MT_Attendance,MT_Recitation,MT_Seatwork,MT_Assignment,MT_LongTests,MT_MajorExam," +
                    "FT_Attendance,FT_Recitation,FT_Seatwork,FT_Assignment,FT_LongTests,FT_MajorExam," +
                    "MidtermGrade,FinalTermGrade,FinalGrade,Status,Remarks,Description");

                foreach (var r in _filteredRecords)
                {
                    sb.AppendLine(string.Join(",",
                        CsvEsc(r.StudentID), CsvEsc(r.Name), CsvEsc(r.Course),
                        Fmt(r.MT_Attendance), Fmt(r.MT_Recitation), Fmt(r.MT_Seatwork),
                        Fmt(r.MT_Assignment), Fmt(r.MT_LongTests), Fmt(r.MT_MajorExam),
                        Fmt(r.FT_Attendance), Fmt(r.FT_Recitation), Fmt(r.FT_Seatwork),
                        Fmt(r.FT_Assignment), Fmt(r.FT_LongTests), Fmt(r.FT_MajorExam),
                        r.MidtermGrade.HasValue ? r.MidtermGrade.Value.ToString("F2") : "",
                        r.FinalTermGrade.HasValue ? r.FinalTermGrade.Value.ToString("F2") : "",
                        r.FinalGrade.HasValue ? r.FinalGrade.Value.ToString("F2") : "",
                        r.Status, r.Remarks, r.RemarksDescription));
                }

                File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                MessageBox.Show($"Grades exported successfully to:\n{sfd.FileName}",
                    "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export failed: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string Fmt(double? v) => v.HasValue ? v.Value.ToString("F0") : "";

        //  IMPORT FROM CSV
        private void BtnImportExcel_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                Title = "Import Grades from CSV",
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            try
            {
                var lines = File.ReadAllLines(ofd.FileName, Encoding.UTF8);
                if (lines.Length < 2)
                {
                    MessageBox.Show("The file appears to be empty or has no data rows.",
                        "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string[] headers = CsvSplitLine(lines[0]);
                int ColIdx(string name) =>
                    Array.FindIndex(headers, h => h.Trim().Equals(name, StringComparison.OrdinalIgnoreCase));

                int iID = ColIdx("StudentID"); int iName = ColIdx("Name"); int iCourse = ColIdx("Course");
                int iMAtt = ColIdx("MT_Attendance"); int iMRec = ColIdx("MT_Recitation");
                int iMSeat = ColIdx("MT_Seatwork"); int iMAsgn = ColIdx("MT_Assignment");
                int iMLT = ColIdx("MT_LongTests"); int iMMex = ColIdx("MT_MajorExam");
                int iFAtt = ColIdx("FT_Attendance"); int iFRec = ColIdx("FT_Recitation");
                int iFSeat = ColIdx("FT_Seatwork"); int iFAsgn = ColIdx("FT_Assignment");
                int iFLT = ColIdx("FT_LongTests"); int iFMex = ColIdx("FT_MajorExam");

                if (iID < 0)
                {
                    MessageBox.Show("Could not find a 'StudentID' column in the CSV.\nPlease use the exported file as a template.",
                        "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int imported = 0, updated = 0, skipped = 0;

                for (int li = 1; li < lines.Length; li++)
                {
                    if (string.IsNullOrWhiteSpace(lines[li])) continue;
                    string[] cols = CsvSplitLine(lines[li]);
                    if (cols.Length <= iID) { skipped++; continue; }

                    string sid = cols[iID].Trim();
                    if (string.IsNullOrEmpty(sid)) { skipped++; continue; }

                    double? Safe(int idx)
                    {
                        if (idx < 0 || idx >= cols.Length) return null;
                        string raw = cols[idx].Trim();
                        if (string.IsNullOrEmpty(raw)) return null;
                        if (double.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out double v)
                            && v >= 0 && v <= 100) return v;
                        return null;
                    }

                    string safeName = (iName >= 0 && iName < cols.Length) ? cols[iName].Trim() : sid;
                    string safeCourse = (iCourse >= 0 && iCourse < cols.Length) ? cols[iCourse].Trim() : "";

                    var record = _gradeRecords.FirstOrDefault(r => r.StudentID == sid);
                    bool isNew = record == null;

                    if (isNew)
                    {
                        record = new StudentGradeRecord { StudentID = sid, Name = safeName, Course = safeCourse, Status = "Pending" };
                        _gradeRecords.Add(record);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(safeName)) record.Name = safeName;
                        if (!string.IsNullOrEmpty(safeCourse)) record.Course = safeCourse;
                    }

                    if (Safe(iMAtt).HasValue) record.MT_Attendance = Safe(iMAtt);
                    if (Safe(iMRec).HasValue) record.MT_Recitation = Safe(iMRec);
                    if (Safe(iMSeat).HasValue) record.MT_Seatwork = Safe(iMSeat);
                    if (Safe(iMAsgn).HasValue) record.MT_Assignment = Safe(iMAsgn);
                    if (Safe(iMLT).HasValue) record.MT_LongTests = Safe(iMLT);
                    if (Safe(iMMex).HasValue) record.MT_MajorExam = Safe(iMMex);
                    if (Safe(iFAtt).HasValue) record.FT_Attendance = Safe(iFAtt);
                    if (Safe(iFRec).HasValue) record.FT_Recitation = Safe(iFRec);
                    if (Safe(iFSeat).HasValue) record.FT_Seatwork = Safe(iFSeat);
                    if (Safe(iFAsgn).HasValue) record.FT_Assignment = Safe(iFAsgn);
                    if (Safe(iFLT).HasValue) record.FT_LongTests = Safe(iFLT);
                    if (Safe(iFMex).HasValue) record.FT_MajorExam = Safe(iFMex);

                    bool allMT = record.MT_Attendance.HasValue && record.MT_Recitation.HasValue &&
                                 record.MT_Seatwork.HasValue && record.MT_Assignment.HasValue &&
                                 record.MT_LongTests.HasValue && record.MT_MajorExam.HasValue;
                    bool allFT = record.FT_Attendance.HasValue && record.FT_Recitation.HasValue &&
                                 record.FT_Seatwork.HasValue && record.FT_Assignment.HasValue &&
                                 record.FT_LongTests.HasValue && record.FT_MajorExam.HasValue;
                    if (allMT && allFT) record.Status = "Submitted";

                    if (isNew) imported++; else updated++;
                }

                _filteredRecords = new List<StudentGradeRecord>(_gradeRecords);
                _gradesLoaded = true;

                RefreshGrid();
                RefreshFinalTermGrid();
                UpdateStatCards(_filteredRecords);

                MessageBox.Show(
                    $"Import complete!\n\n✔ {imported} new record(s) added.\n✔ {updated} existing record(s) updated.\n" +
                    (skipped > 0 ? $"⚠ {skipped} row(s) skipped." : ""),
                    "Import Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Import failed: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string[] CsvSplitLine(string line)
        {
            var fields = new List<string>();
            bool inQuote = false;
            var cur = new StringBuilder();
            foreach (char ch in line)
            {
                if (ch == '"') { inQuote = !inQuote; }
                else if (ch == ',' && !inQuote) { fields.Add(cur.ToString()); cur.Clear(); }
                else { cur.Append(ch); }
            }
            fields.Add(cur.ToString());
            return fields.ToArray();
        }

        private static string CsvEsc(string s) =>
            s.Contains(',') || s.Contains('"') ? $"\"{s.Replace("\"", "\"\"")}\"" : s;

        //  PRINT GRADES
        private void BtnPrintGrades_Click(object sender, EventArgs e)
        {
            if (!_gradesLoaded || _filteredRecords.Count == 0)
            {
                MessageBox.Show("No data to print. Load students first.", "Print Grades",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            CommitGridEdits();

            using var pd = new PrintDocument { DocumentName = "Grade Report" };
            pd.DefaultPageSettings.Landscape = true;
            pd.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(40, 40, 40, 40);
            int currentRow = 0;
            var printData = _filteredRecords.ToList();

            pd.PrintPage += (s, pe) =>
            {
                var g = pe.Graphics;
                var font = new Font("Segoe UI", 9f);
                var fontBold = new Font("Segoe UI", 9f, FontStyle.Bold);
                var fontTitle = new Font("Segoe UI", 16f, FontStyle.Bold);
                var fontSub = new Font("Segoe UI", 10f, FontStyle.Bold);
                var fontSmall = new Font("Segoe UI", 8.5f);
                var maroon = new SolidBrush(Color.FromArgb(106, 0, 0));
                var white = Brushes.White;
                var black = Brushes.Black;
                var altRow = new SolidBrush(Color.FromArgb(245, 245, 245));

                float x = pe.MarginBounds.Left;
                float y = pe.MarginBounds.Top;
                float pgW = pe.MarginBounds.Width;

                //  Title 
                g.DrawString("PUP – Grade Report", fontTitle, maroon, x, y);
                y += fontTitle.GetHeight(g) + 6;

                //  Course / Semester sub-header 
                string courseLabel = "All Courses";
                string semesterLabel = "1st Semester";

                if (cmbCourseSection != null && cmbCourseSection.SelectedIndex > 0)
                    courseLabel = cmbCourseSection.SelectedItem?.ToString() ?? courseLabel;

                // Left: Course | Right: Semester
                g.DrawString($"Course / Section:  {courseLabel}", fontSub, maroon, x, y);
                string semText = $"Semester:  {semesterLabel}";
                SizeF semSize = g.MeasureString(semText, fontSub);
                g.DrawString(semText, fontSub, maroon, x + pgW - semSize.Width, y);
                y += fontSub.GetHeight(g) + 10;

                // Separator line
                g.DrawLine(new Pen(Color.FromArgb(106, 0, 0), 1.5f), x, y, x + pgW, y);
                y += 8;

                // Column setup 
                // 9 columns total — distribute remaining width to Name
                float c0 = 130;   // Student ID
                float c2 = 65;    // Course
                float c3 = 72;    // MT Grade
                float c4 = 72;    // FT Grade
                float c5 = 75;    // Final Grade
                float c6 = 70;    // Status
                float c7 = 62;    // Remarks
                float c8 = 80;    // Description
                float c1 = pgW - c0 - c2 - c3 - c4 - c5 - c6 - c7 - c8; // Name fills rest

                float[] cw = { c0, c1, c2, c3, c4, c5, c6, c7, c8 };
                string[] hdrTxt = { "Student ID", "Name", "Course",
                         "MT Grade", "FT Grade", "Final Grade",
                         "Status", "Remarks", "Description" };

                float lineH = font.GetHeight(g) + 5;

                //  Header row 
                float cx = x;
                for (int i = 0; i < hdrTxt.Length; i++)
                {
                    g.FillRectangle(maroon, cx, y, cw[i], lineH + 4);
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center,
                        Trimming = StringTrimming.EllipsisCharacter
                    };
                    g.DrawString(hdrTxt[i], fontBold, white,
                        new RectangleF(cx + 2, y, cw[i] - 4, lineH + 4), sf);
                    cx += cw[i];
                }
                y += lineH + 4;

                //  Data rows 
                bool alt = false;
                while (currentRow < printData.Count)
                {
                    if (y + lineH > pe.MarginBounds.Bottom) { pe.HasMorePages = true; return; }

                    var r = printData[currentRow++];
                    cx = x;

                    string[] vals =
                    {
            r.StudentID,
            r.Name,
            r.Course,
            r.MidtermGrade.HasValue   ? r.MidtermGrade.Value.ToString("F2")   : "—",
            r.FinalTermGrade.HasValue ? r.FinalTermGrade.Value.ToString("F2") : "—",
            r.FinalGrade.HasValue     ? r.FinalGrade.Value.ToString("F2")     : "—",
            r.Status,
            r.Remarks,
            r.RemarksDescription,
        };

                    // Alternate row background
                    if (alt)
                        g.FillRectangle(altRow, x, y, pgW, lineH);

                    for (int i = 0; i < vals.Length; i++)
                    {
                        // Color-code specific columns
                        Brush fg = black;
                        if (i == 7) // Remarks
                        {
                            fg = vals[i] switch
                            {
                                "1.00" or "1.25" or "1.50" => new SolidBrush(Color.FromArgb(16, 124, 65)),
                                "1.75" or "2.00" => new SolidBrush(Color.FromArgb(0, 112, 192)),
                                "2.25" or "2.50" => new SolidBrush(Color.FromArgb(180, 120, 0)),
                                "5.00" => new SolidBrush(Color.Firebrick),
                                _ => new SolidBrush(Color.Gray),
                            };
                        }
                        if (i == 6) // Status
                        {
                            fg = vals[i] == "Submitted"
                                ? new SolidBrush(Color.FromArgb(16, 124, 65))
                                : new SolidBrush(Color.FromArgb(200, 100, 0));
                        }

                        // Left-align ID & Name, center rest
                        var sf = new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            Trimming = StringTrimming.EllipsisCharacter,
                            Alignment = (i <= 1) ? StringAlignment.Near : StringAlignment.Center,
                        };
                        g.DrawString(vals[i], font, fg,
                            new RectangleF(cx + 3, y, cw[i] - 6, lineH), sf);
                        cx += cw[i];
                    }

                    // Row bottom border
                    g.DrawLine(Pens.LightGray, x, y + lineH, x + pgW, y + lineH);
                    y += lineH;
                    alt = !alt;
                }

                //  Footer 
                y += 6;
                g.DrawLine(new Pen(Color.FromArgb(106, 0, 0), 1f), x, y, x + pgW, y);
                y += 4;
                int total = printData.Count;
                int submitted = printData.Count(r => r.Status == "Submitted");
                int pending = total - submitted;
                g.DrawString(
                    $"Total Students: {total}     Submitted: {submitted}     Pending: {pending}",
                    fontSmall, maroon, x, y);

                // Page number — bottom right
                string pageInfo = $"Page {currentRow / printData.Count + 1}";
                SizeF piSize = g.MeasureString(pageInfo, fontSmall);
                g.DrawString(pageInfo, fontSmall, maroon, x + pgW - piSize.Width, y);

                pe.HasMorePages = false;
            };

            using var ppd = new PrintPreviewDialog { Document = pd, WindowState = FormWindowState.Maximized };
            ppd.ShowDialog(this);
        }

        //  GRID CELL EDIT EVENTS
        private void GridStudents_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || gridStudents == null) return;

            string[] editableCols = { "colAttendance", "colRecitation", "colSeatwork",
                               "colAssignment", "colLongTests", "colMajorExam" };
            string changedColName = gridStudents.Columns[e.ColumnIndex].Name;
            if (!editableCols.Contains(changedColName)) return;

            var row = gridStudents.Rows[e.RowIndex];
            string id = row.Cells["colStudentID"].Value?.ToString() ?? "";
            var record = _gradeRecords.FirstOrDefault(r => r.StudentID == id);
            if (record == null) return;

            //  Parse each grade cell (values > 100 are clamped to 100 by ParseGradeCell) 
            double? att = ParseGradeCell(row.Cells["colAttendance"].Value);
            double? rec = ParseGradeCell(row.Cells["colRecitation"].Value);
            double? seat = ParseGradeCell(row.Cells["colSeatwork"].Value);
            double? asgn = ParseGradeCell(row.Cells["colAssignment"].Value);
            double? lt = ParseGradeCell(row.Cells["colLongTests"].Value);
            double? mex = ParseGradeCell(row.Cells["colMajorExam"].Value);

            //  Correct displayed value if it was clamped 
            void CorrectCell(string colName, double? parsed, object raw)
            {
                if (parsed.HasValue && raw != null)
                {
                    string rawStr = raw.ToString() ?? "";
                    if (double.TryParse(rawStr, out double rv) && rv != parsed.Value)
                    {
                        // Suppress re-entry of CellValueChanged while we fix the value
                        gridStudents.CellValueChanged -= GridStudents_CellValueChanged;
                        row.Cells[colName].Value = parsed.Value.ToString("F0");
                        row.Cells[colName].Value = parsed.Value.ToString("F0");
                        gridStudents.CellValueChanged += GridStudents_CellValueChanged;
                    }
                }
            }
            CorrectCell("colAttendance", att, row.Cells["colAttendance"].Value);
            CorrectCell("colRecitation", rec, row.Cells["colRecitation"].Value);
            CorrectCell("colSeatwork", seat, row.Cells["colSeatwork"].Value);
            CorrectCell("colAssignment", asgn, row.Cells["colAssignment"].Value);
            CorrectCell("colLongTests", lt, row.Cells["colLongTests"].Value);
            CorrectCell("colMajorExam", mex, row.Cells["colMajorExam"].Value);

            //  Write back to record 
            bool mt = _showingMidterm;
            if (mt)
            {
                record.MT_Attendance = att; record.MT_Recitation = rec;
                record.MT_Seatwork = seat; record.MT_Assignment = asgn;
                record.MT_LongTests = lt; record.MT_MajorExam = mex;
            }
            else
            {
                record.FT_Attendance = att; record.FT_Recitation = rec;
                record.FT_Seatwork = seat; record.FT_Assignment = asgn;
                record.FT_LongTests = lt; record.FT_MajorExam = mex;
            }

            //  Color helper for individual score cells 
            void ColorScoreCell(string colName, double? v)
            {
                if (!v.HasValue) return;
                row.Cells[colName].Style.ForeColor =
                    v.Value < 75 ? Color.Firebrick :
                    v.Value >= 90 ? Color.FromArgb(16, 124, 65) :
                                    Color.Black;
            }
            ColorScoreCell("colAttendance", att);
            ColorScoreCell("colRecitation", rec);
            ColorScoreCell("colSeatwork", seat);
            ColorScoreCell("colAssignment", asgn);
            ColorScoreCell("colLongTests", lt);
            ColorScoreCell("colMajorExam", mex);

            //  Recalculate Term Grade & Final Grade 
            double? termGrade = mt ? record.MidtermGrade : record.FinalTermGrade;

            if (termGrade.HasValue)
            {
                bool pass = termGrade.Value >= 75;

                row.Cells["colTermGrade"].Value = termGrade.Value.ToString("F2");
                row.Cells["colTermGrade"].Style.ForeColor = pass ? Color.FromArgb(16, 124, 65) : Color.Firebrick;
                row.Cells["colTermGrade"].Style.BackColor = pass ? Color.FromArgb(230, 255, 230) : Color.FromArgb(255, 230, 230);
                row.Cells["colTermGrade"].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                row.Cells["colTermGrade"].Style.SelectionForeColor = row.Cells["colTermGrade"].Style.ForeColor;
                row.Cells["colTermGrade"].Style.SelectionBackColor = row.Cells["colTermGrade"].Style.BackColor;
            }
            else
            {
                // Not all 6 scores filled in yet — clear Term Grade
                row.Cells["colTermGrade"].Value = "";
                row.Cells["colTermGrade"].Style.BackColor = Color.Empty;
                row.Cells["colTermGrade"].Style.ForeColor = Color.Black;
            }

            if (record.FinalGrade.HasValue)
            {
                row.Cells["colFinalGrade"].Value = record.FinalGrade.Value.ToString("F2");
                row.Cells["colFinalGrade"].Style.ForeColor = record.FinalGrade.Value >= 75
                    ? Color.FromArgb(16, 124, 65) : Color.Firebrick;
                row.Cells["colFinalGrade"].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);

                var (grade, desc) = GetPupGrade(record.FinalGrade);
                row.Cells["colRemarks"].Value = grade;
                row.Cells["colRemarks"].ToolTipText = desc;
                row.Cells["colRemarks"].Style.ForeColor = GetPupGradeColor(grade);
                row.Cells["colRemarks"].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);

                bool allMT = record.MT_Attendance.HasValue && record.MT_Recitation.HasValue &&
                             record.MT_Seatwork.HasValue && record.MT_Assignment.HasValue &&
                             record.MT_LongTests.HasValue && record.MT_MajorExam.HasValue;
                bool allFT = record.FT_Attendance.HasValue && record.FT_Recitation.HasValue &&
                             record.FT_Seatwork.HasValue && record.FT_Assignment.HasValue &&
                             record.FT_LongTests.HasValue && record.FT_MajorExam.HasValue;

                if (allMT && allFT)
                {
                    record.Status = "Submitted";
                    row.Cells["colStatus"].Value = "Submitted";
                    row.Cells["colStatus"].Style.ForeColor = Color.FromArgb(16, 124, 65);
                    row.Cells["colStatus"].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                }
            }
            else
            {
                // Final Grade incomplete — clear grades, mark pending
                row.Cells["colFinalGrade"].Value = "";
                row.Cells["colRemarks"].Value = "INC";
                row.Cells["colRemarks"].ToolTipText = "Incomplete";
                row.Cells["colRemarks"].Style.ForeColor = Color.Gray;
                row.Cells["colStatus"].Value = "Pending";
                row.Cells["colStatus"].Style.ForeColor = Color.FromArgb(200, 100, 0);
                row.Cells["colStatus"].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            }
        }

        private void GridStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || gridStudents == null) return;
            if (gridStudents.Columns[e.ColumnIndex].Name != "colActions") return;
            string id = gridStudents.Rows[e.RowIndex].Cells["colStudentID"].Value?.ToString() ?? "";
            var record = _gradeRecords.FirstOrDefault(r => r.StudentID == id);
            if (record != null) ShowEditGradeDialog(record);
        }

        private void GridStudents_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox tb)
            {
                // Remove old handlers first to avoid stacking
                tb.KeyPress -= GradeCell_KeyPress;
                tb.TextChanged -= GradeCell_TextChanged;

                tb.KeyPress += GradeCell_KeyPress;
                tb.TextChanged += GradeCell_TextChanged;
            }
        }

        private void GradeCell_TextChanged(object sender, EventArgs e)
        {
            if (sender is not TextBox tb) return;
            string text = tb.Text;
            if (string.IsNullOrWhiteSpace(text)) return;

            if (double.TryParse(text, out double val) && val > 100)
            {
                tb.Text = "100";
                tb.SelectionStart = tb.Text.Length;
            }
        }

        private void GradeCell_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '\b')
                e.Handled = true;
        }

        private void GridStudents_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e) { }

        //  EDIT GRADE DIALOG 
        private void ShowEditGradeDialog(StudentGradeRecord record)
        {
            string termLabel = _showingMidterm ? "Midterm" : "Final Term";
            using var dlg = new Form
            {
                Text = $"Edit Grades – {record.Name}  [{termLabel}]",
                Size = new Size(430, 560),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White,
            };

            // Header bar
            var hdr = new Panel { Dock = DockStyle.Top, Height = 44, BackColor = Color.FromArgb(106, 0, 0) };
            hdr.Controls.Add(new Label
            {
                Text = $"{record.Name}  •  {termLabel}",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0)
            });
            dlg.Controls.Add(hdr);

            int y = 54;
            bool mt = _showingMidterm;

            TextBox AddField(string lbl, double? val)
            {
                dlg.Controls.Add(new Label
                {
                    Text = lbl,
                    Left = 20,
                    Top = y,
                    Width = 210,
                    Height = 22,
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
                });
                var tb = new TextBox
                {
                    Text = val.HasValue ? val.Value.ToString("F0") : "",
                    Left = 240,
                    Top = y,
                    Width = 150,
                    Height = 22
                };
                dlg.Controls.Add(tb);
                y += 34;
                return tb;
            }

            var tbAtt = AddField($"Attendance ({CurrentWeights.AttendancePct}%):", mt ? record.MT_Attendance : record.FT_Attendance);
            var tbRec = AddField($"Recitation ({CurrentWeights.RecitationPct}%):", mt ? record.MT_Recitation : record.FT_Recitation);
            var tbSeat = AddField($"Seatwork ({CurrentWeights.SeatworkPct}%):", mt ? record.MT_Seatwork : record.FT_Seatwork);
            var tbAsgn = AddField($"Assignment ({CurrentWeights.AssignmentPct}%):", mt ? record.MT_Assignment : record.FT_Assignment);
            var tbLT = AddField($"Long Tests ({CurrentWeights.LongTestsPct}%):", mt ? record.MT_LongTests : record.FT_LongTests);
            var tbMex = AddField($"Major Exam ({CurrentWeights.MajorExamsPct}%):", mt ? record.MT_MajorExam : record.FT_MajorExam);

            // Custom columns for this term
            var customFields = new Dictionary<CustomGradeColumn, TextBox>();
            var customList = mt ? _customMTColumns : _customFTColumns;
            if (customList.Count > 0)
            {
                dlg.Controls.Add(new Label
                {
                    Text = "── Custom Columns ──",
                    Left = 20,
                    Top = y,
                    AutoSize = true,
                    Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                    ForeColor = Color.FromArgb(80, 80, 80)
                });
                y += 22;
                foreach (var cc in customList)
                {
                    cc.Scores.TryGetValue(record.StudentID, out double? existing);
                    var tb = AddField($"{cc.Label} ({cc.Category}):", existing);
                    customFields[cc] = tb;
                }
                dlg.Height = Math.Min(700, y + 120);
            }

            var lblPreview = new Label
            {
                Left = 20,
                Top = y + 4,
                Width = 380,
                Height = 22,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.Gray,
                Text = "Term Grade: —",
            };
            dlg.Controls.Add(lblPreview);

            void UpdatePreview()
            {
                double? a = ParseGradeCell(tbAtt.Text), r2 = ParseGradeCell(tbRec.Text),
                        s = ParseGradeCell(tbSeat.Text), ag = ParseGradeCell(tbAsgn.Text),
                        l = ParseGradeCell(tbLT.Text), m = ParseGradeCell(tbMex.Text);
                if (a.HasValue && r2.HasValue && s.HasValue && ag.HasValue && l.HasValue && m.HasValue)
                {
                    var w = CurrentWeights;
                    double cs = a.Value * (w.AttendancePct / 100.0) + r2.Value * (w.RecitationPct / 100.0)
                              + s.Value * (w.SeatworkPct / 100.0) + ag.Value * (w.AssignmentPct / 100.0)
                              + l.Value * (w.LongTestsPct / 100.0);
                    double tg = Math.Round(cs + m.Value * (w.MajorExamsPct / 100.0), 2);
                    lblPreview.Text = $"Term Grade: {tg:F2}  ({(tg >= 75 ? "Passing ✓" : "Below Passing ✗")})";
                    lblPreview.ForeColor = tg >= 75 ? Color.FromArgb(16, 124, 65) : Color.Firebrick;
                }
                else
                {
                    lblPreview.Text = "Term Grade: —  (Incomplete)";
                    lblPreview.ForeColor = Color.Gray;
                }
            }

            foreach (var tb2 in new[] { tbAtt, tbRec, tbSeat, tbAsgn, tbLT, tbMex })
                tb2.TextChanged += (s2, _) => UpdatePreview();
            UpdatePreview();

            y += 36;
            var btnSave = new Button
            {
                Text = "Save",
                Left = 230,
                Top = y,
                Width = 80,
                Height = 30,
                BackColor = Color.FromArgb(106, 0, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
            };
            btnSave.FlatAppearance.BorderSize = 0;
            var btnCancel = new Button { Text = "Cancel", Left = 320, Top = y, Width = 80, Height = 30, FlatStyle = FlatStyle.Flat };

            btnSave.Click += (s2, _) =>
            {
                double? a = ParseGradeCell(tbAtt.Text), re = ParseGradeCell(tbRec.Text),
                        s = ParseGradeCell(tbSeat.Text), ag = ParseGradeCell(tbAsgn.Text),
                        l = ParseGradeCell(tbLT.Text), m = ParseGradeCell(tbMex.Text);
                if (mt)
                {
                    record.MT_Attendance = a; record.MT_Recitation = re;
                    record.MT_Seatwork = s; record.MT_Assignment = ag;
                    record.MT_LongTests = l; record.MT_MajorExam = m;
                }
                else
                {
                    record.FT_Attendance = a; record.FT_Recitation = re;
                    record.FT_Seatwork = s; record.FT_Assignment = ag;
                    record.FT_LongTests = l; record.FT_MajorExam = m;
                }

                // Save custom column scores from dialog
                foreach (var (cc, tb) in customFields)
                    cc.Scores[record.StudentID] = ParseGradeCell(tb.Text);

                bool allMT = record.MT_Attendance.HasValue && record.MT_Recitation.HasValue &&
                             record.MT_Seatwork.HasValue && record.MT_Assignment.HasValue &&
                             record.MT_LongTests.HasValue && record.MT_MajorExam.HasValue;
                bool allFT = record.FT_Attendance.HasValue && record.FT_Recitation.HasValue &&
                             record.FT_Seatwork.HasValue && record.FT_Assignment.HasValue &&
                             record.FT_LongTests.HasValue && record.FT_MajorExam.HasValue;
                if (allMT && allFT) record.Status = "Submitted";

                RefreshGrid();
                RefreshFinalTermGrid();
                UpdateStatCards(_filteredRecords);
                dlg.DialogResult = DialogResult.OK;
            };
            btnCancel.Click += (s2, _) => dlg.Close();

            dlg.Controls.Add(btnSave);
            dlg.Controls.Add(btnCancel);
            dlg.ShowDialog(this);
        }

        private void GradesContentInst_Load(object sender, EventArgs e)
        {
            PopulateGradeManagement();
            FixPanelRendering();
            if (tabControl1 != null)
                tabControl1.Dock = DockStyle.Fill;

            if (gridStudents != null)
            {
                gridStudents.Dock = DockStyle.Fill;
                gridStudents.Visible = true;
            }

            if (!_gradesLoaded)
                LoadGradeData();
            else
            {
                RefreshGrid();
                RefreshFinalTermGrid();
            }
        }

        private void FixPanelRendering()
        {
            // Recursively enable double-buffering (prevents GDI overflow/rainbow
            // artifact on TableLayoutPanel at different DPI settings)
            void SetDoubleBuffered(Control c)
            {
                if (c == null) return;
                try
                {
                    var prop = typeof(Control).GetProperty("DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);
                    prop?.SetValue(c, true, null);
                }
                catch { }
                foreach (Control child in c.Controls)
                    SetDoubleBuffered(child);
            }

            if (panelCardsContainer != null)
            {
                panelCardsContainer.AutoSize = false;
                panelCardsContainer.AutoScroll = false;
                SetDoubleBuffered(panelCardsContainer);
            }
        }
    }
}
