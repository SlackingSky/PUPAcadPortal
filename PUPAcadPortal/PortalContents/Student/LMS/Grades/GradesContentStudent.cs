using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Student.LMS
{
    public partial class GradesContentStudent : UserControl
    {
        // GRADES DATA
        private DataTable midtermGradesTable;
        private DataTable finalGradesTable;

        private struct GradeEntry
        {
            public int No;
            public string SubjectCode;
            public string SubjectTitle;
            public int Units;
            public double Grade;
            public double Equivalent;
            public string Remarks;
        }

        private readonly List<GradeEntry> _midtermGrades = new List<GradeEntry>
        {
            new GradeEntry { No=1, SubjectCode="COMP 009",    SubjectTitle="Object Oriented Programming",                          Units=3, Grade=92, Equivalent=1.25, Remarks="PASSED" },
            new GradeEntry { No=2, SubjectCode="COMP 010",    SubjectTitle="Information Management",                               Units=3, Grade=88, Equivalent=1.50, Remarks="PASSED" },
            new GradeEntry { No=3, SubjectCode="COMP 012",    SubjectTitle="Network Administration",                               Units=3, Grade=85, Equivalent=1.75, Remarks="PASSED" },
            new GradeEntry { No=4, SubjectCode="COMP 013",    SubjectTitle="Human Computer Interaction",                          Units=3, Grade=90, Equivalent=1.25, Remarks="PASSED" },
            new GradeEntry { No=5, SubjectCode="COMP 014",    SubjectTitle="Quantitative Methods with Modeling and Simulation",    Units=3, Grade=78, Equivalent=2.25, Remarks="PASSED" },
            new GradeEntry { No=6, SubjectCode="ELEC IT-FE2", SubjectTitle="BSIT Free Elective 2",                                 Units=3, Grade=83, Equivalent=1.75, Remarks="PASSED" },
            new GradeEntry { No=7, SubjectCode="INTE 202",    SubjectTitle="Interactive Programming and Technologies 1",          Units=3, Grade=95, Equivalent=1.00, Remarks="PASSED" },
            new GradeEntry { No=8, SubjectCode="PATHFIT 4",   SubjectTitle="Physical Activity Towards Health and Fitness 4",      Units=2, Grade=88, Equivalent=1.50, Remarks="PASSED" },
        };

        private readonly List<GradeEntry> _finalGrades = new List<GradeEntry>
        {
            new GradeEntry { No=1, SubjectCode="COMP 009",    SubjectTitle="Object Oriented Programming",                          Units=3, Grade=91, Equivalent=1.25, Remarks="PASSED" },
            new GradeEntry { No=2, SubjectCode="COMP 010",    SubjectTitle="Information Management",                               Units=3, Grade=86, Equivalent=1.75, Remarks="PASSED" },
            new GradeEntry { No=3, SubjectCode="COMP 012",    SubjectTitle="Network Administration",                               Units=3, Grade=84, Equivalent=1.75, Remarks="PASSED" },
            new GradeEntry { No=4, SubjectCode="COMP 013",    SubjectTitle="Human Computer Interaction",                          Units=3, Grade=89, Equivalent=1.50, Remarks="PASSED" },
            new GradeEntry { No=5, SubjectCode="COMP 014",    SubjectTitle="Quantitative Methods with Modeling and Simulation",    Units=3, Grade=76, Equivalent=2.25, Remarks="PASSED" },
            new GradeEntry { No=6, SubjectCode="ELEC IT-FE2", SubjectTitle="BSIT Free Elective 2",                                 Units=3, Grade=81, Equivalent=2.00, Remarks="PASSED" },
            new GradeEntry { No=7, SubjectCode="INTE 202",    SubjectTitle="Interactive Programming and Technologies 1",          Units=3, Grade=93, Equivalent=1.00, Remarks="PASSED" },
            new GradeEntry { No=8, SubjectCode="PATHFIT 4",   SubjectTitle="Physical Activity Towards Health and Fitness 4",      Units=2, Grade=85, Equivalent=1.75, Remarks="PASSED" },
        };

        private readonly string[,] _gradeScale = new string[,]
        {
            {"1.00","97-100","Excellent"},
            {"1.25","94-96", "Excellent"},
            {"1.50","91-93", "Very Good"},
            {"1.75","88-90", "Very Good"},
            {"2.00","85-87", "Good"},
            {"2.25","82-84", "Good"},
            {"2.50","79-81", "Satisfactory"},
            {"2.75","76-78", "Satisfactory"},
            {"3.00","75",    "Passing"},
            {"4.00","68-74", "Conditional"},
            {"5.00","Below 68","Failed"},
            {"Inc.", "–",    "Incomplete"},
            {"W",   "–",     "Withdrawal"},
            {"P",   "–",     "Passed (Non-credit)"},
        };
        public GradesContentStudent()
        {
            InitializeComponent();
        }

        // GRADES – Initialize
        private void Grades_Initialize()
        {
            var gradesPanel = new GradesPanel();
            gradesPanel.Dock = DockStyle.Fill;
            pnlGrades.Controls.Clear();
            pnlGrades.Controls.Add(gradesPanel);
        }

        private DataTable BuildGradeTable(List<GradeEntry> entries)
        {
            var dt = new DataTable();
            dt.Columns.Add("#", typeof(int));
            dt.Columns.Add("Subject Code", typeof(string));
            dt.Columns.Add("Subject Title", typeof(string));
            dt.Columns.Add("Units", typeof(int));
            dt.Columns.Add("Grade", typeof(string));
            dt.Columns.Add("Equivalent", typeof(string));
            dt.Columns.Add("Remarks", typeof(string));

            foreach (var e in entries)
                dt.Rows.Add(e.No, e.SubjectCode, e.SubjectTitle, e.Units,
                            e.Grade.ToString("F0"),
                            e.Equivalent.ToString("F2"),
                            e.Remarks);
            return dt;
        }

        private void SetupGradeGrid(DataGridView dgv, DataTable dt)
        {
            dgv.AutoGenerateColumns = false;
            dgv.DataSource = null;

            // Map columns by name
            string[] colNames = { "#", "Subject Code", "Subject Title", "Units", "Grade", "Equivalent", "Remarks" };
            DataGridViewColumn[] dgvCols = null;

            if (dgv == gridStudents)
                dgvCols = new DataGridViewColumn[] { colID, Column2, dataGridViewTextBoxColumn2, colCourse, colQuiz, colMidterm, colFinals };
            else
                dgvCols = new DataGridViewColumn[] { Column1, dataGridViewTextBoxColumn6, dataGridViewTextBoxColumn7, dataGridViewTextBoxColumn8, dataGridViewTextBoxColumn9, dataGridViewTextBoxColumn10, dataGridViewTextBoxColumn11 };

            for (int i = 0; i < dgvCols.Length && i < colNames.Length; i++)
                dgvCols[i].DataPropertyName = colNames[i];

            dgv.DataSource = dt;
            dgv.ClearSelection();

            // Color-code Remarks column
            dgv.CellFormatting += (sender, e) =>
            {
                if (e.RowIndex < 0) return;
                var grid = (DataGridView)sender;
                // Find Remarks column index
                int remIdx = -1;
                for (int c = 0; c < grid.Columns.Count; c++)
                    if (grid.Columns[c].DataPropertyName == "Remarks") { remIdx = c; break; }
                if (e.ColumnIndex == remIdx && e.Value != null)
                {
                    string v = e.Value.ToString();
                    if (v == "PASSED") { e.CellStyle.ForeColor = Color.Green; e.CellStyle.Font = new Font("Segoe UI Black", 9f, FontStyle.Bold); }
                    else if (v == "FAILED") { e.CellStyle.ForeColor = Color.Red; e.CellStyle.Font = new Font("Segoe UI Black", 9f, FontStyle.Bold); }
                    else if (v == "INC") { e.CellStyle.ForeColor = Color.Orange; }
                }
            };
        }

        private void Grades_PopulateGradeScale()
        {
            gridGradeScale.Rows.Clear();

            // Fill 3 columns of grade scale (3 entries per row)
            int rowsNeeded = (int)Math.Ceiling(_gradeScale.GetLength(0) / 3.0);
            for (int r = 0; r < rowsNeeded; r++)
            {
                var row = gridGradeScale.Rows[gridGradeScale.Rows.Add()];
                for (int col = 0; col < 3; col++)
                {
                    int idx = r * 3 + col;
                    if (idx < _gradeScale.GetLength(0))
                    {
                        row.Cells[col * 3 + 0].Value = _gradeScale[idx, 0];
                        row.Cells[col * 3 + 1].Value = _gradeScale[idx, 1];
                        row.Cells[col * 3 + 2].Value = _gradeScale[idx, 2];
                    }
                }
            }
        }

        private void Grades_UpdateSummaryCards(List<GradeEntry> entries)
        {
            if (entries == null || entries.Count == 0) return;

            int totalUnits = entries.Sum(e => e.Units);
            int unitsEarned = entries.Where(e => e.Remarks == "PASSED").Sum(e => e.Units);
            int passed = entries.Count(e => e.Remarks == "PASSED");
            int failed = entries.Count(e => e.Remarks == "FAILED");
            int inProgress = entries.Count(e => e.Remarks == "INC" || e.Remarks == "IN PROGRESS");

            // GWA calculation (weighted average of equivalents for passed subjects)
            double totalWeighted = 0; int totalUnitsPassed = 0;
            foreach (var e in entries.Where(x => x.Remarks == "PASSED"))
            {
                totalWeighted += e.Equivalent * e.Units;
                totalUnitsPassed += e.Units;
            }
            double gwa = totalUnitsPassed > 0 ? totalWeighted / totalUnitsPassed : 0;

            // Update labels
            lblGWAVal.Text = gwa > 0 ? gwa.ToString("F2") : "—";
            lblTotalUnitsVal.Text = totalUnits.ToString();
            lblUnitsEarnedVal.Text = unitsEarned.ToString();
            lblPassedVal.Text = passed.ToString();
            lblFailedVal.Text = failed.ToString();
            lblInProgressVal.Text = inProgress.ToString();
            lblPaginationInfo.Text = $"Showing 1 to {entries.Count} of {entries.Count} subjects";

            // Color GWA based on value
            if (gwa > 0 && gwa <= 1.75) lblGWAVal.ForeColor = Color.FromArgb(16, 124, 65);
            else if (gwa <= 2.50) lblGWAVal.ForeColor = Color.DarkOrange;
            else if (gwa > 2.50) lblGWAVal.ForeColor = Color.DarkRed;
        }

        private void Grades_OnTabChanged()
        {
            if (tabControl1.SelectedIndex == 0)
                Grades_UpdateSummaryCards(_midtermGrades);
            else
                Grades_UpdateSummaryCards(_finalGrades);

            // Clear search when switching tabs
            if (txtSearch.Text != "Search subject code or name...")
                txtSearch.Text = "";
        }

        private void Grades_ApplySearch()
        {
            string term = txtSearch.Text.Trim().ToLower();
            if (term == "search subject code or name..." || string.IsNullOrWhiteSpace(term))
            {
                if (tabControl1.SelectedIndex == 0) gridStudents.DataSource = midtermGradesTable;
                else dataGridView1.DataSource = finalGradesTable;
                return;
            }

            var source = tabControl1.SelectedIndex == 0 ? _midtermGrades : _finalGrades;
            var filtered = source.Where(e =>
                e.SubjectCode.ToLower().Contains(term) ||
                e.SubjectTitle.ToLower().Contains(term)).ToList();

            var filteredTable = BuildGradeTable(filtered);
            if (tabControl1.SelectedIndex == 0)
            {
                gridStudents.DataSource = filteredTable;
                gridStudents.ClearSelection();
            }
            else
            {
                dataGridView1.DataSource = filteredTable;
                dataGridView1.ClearSelection();
            }
            lblPaginationInfo.Text = $"Showing 1 to {filtered.Count} of {filtered.Count} subjects";
        }

        private void Grades_RefreshView()
        {
            // Re-bind the data (could be extended to actually filter by semester/AY)
            if (tabControl1.SelectedIndex == 0)
            {
                gridStudents.DataSource = midtermGradesTable;
                Grades_UpdateSummaryCards(_midtermGrades);
            }
            else
            {
                dataGridView1.DataSource = finalGradesTable;
                Grades_UpdateSummaryCards(_finalGrades);
            }
            gridStudents.ClearSelection();
            dataGridView1.ClearSelection();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string sourceFile = Path.Combine(Application.StartupPath, "Resources", "COG-MTECH.pdf");

            if (!File.Exists(sourceFile))
            {
                MessageBox.Show("Error: COG-MTECH.pdf was not found in the Resources folder.",
                                "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PDF Documents (*.pdf)|*.pdf";
                sfd.FileName = "COG-MTECH.pdf";
                sfd.Title = "Save COG-MTECH Report";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.Copy(sourceFile, sfd.FileName, true);
                        MessageBox.Show("COG-MTECH.pdf has been saved successfully!",
                                        "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message,
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void GradesContentStudent_Load(object sender, EventArgs e)
        {
            Grades_Initialize();
            Grades_RefreshView();
        }
    }
}
