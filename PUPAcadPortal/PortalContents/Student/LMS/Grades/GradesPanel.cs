using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Student.LMS.Grades
{
    public partial class GradesPanel : UserControl
    {
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

        private readonly List<GradeEntry> _midterm = new List<GradeEntry>
        {
            new GradeEntry{No=1,SubjectCode="COMP 009",   SubjectTitle="Object Oriented Programming",                        Units=3,Grade=92,Equivalent=1.25,Remarks="PASSED"},
            new GradeEntry{No=2,SubjectCode="COMP 010",   SubjectTitle="Information Management",                             Units=3,Grade=88,Equivalent=1.50,Remarks="PASSED"},
            new GradeEntry{No=3,SubjectCode="COMP 012",   SubjectTitle="Network Administration",                             Units=3,Grade=85,Equivalent=1.75,Remarks="PASSED"},
            new GradeEntry{No=4,SubjectCode="COMP 013",   SubjectTitle="Human Computer Interaction",                        Units=3,Grade=90,Equivalent=1.25,Remarks="PASSED"},
            new GradeEntry{No=5,SubjectCode="COMP 014",   SubjectTitle="Quantitative Methods with Modeling and Simulation",  Units=3,Grade=78,Equivalent=2.25,Remarks="PASSED"},
            new GradeEntry{No=6,SubjectCode="ELEC IT-FE2",SubjectTitle="BSIT Free Elective 2",                               Units=3,Grade=83,Equivalent=1.75,Remarks="PASSED"},
            new GradeEntry{No=7,SubjectCode="INTE 202",   SubjectTitle="Interactive Programming and Technologies 1",        Units=3,Grade=95,Equivalent=1.00,Remarks="PASSED"},
            new GradeEntry{No=8,SubjectCode="PATHFIT 4",  SubjectTitle="Physical Activity Towards Health and Fitness 4",    Units=2,Grade=88,Equivalent=1.50,Remarks="PASSED"},
        };

        private readonly List<GradeEntry> _final = new List<GradeEntry>
        {
            new GradeEntry{No=1,SubjectCode="COMP 009",   SubjectTitle="Object Oriented Programming",                        Units=3,Grade=91,Equivalent=1.25,Remarks="PASSED"},
            new GradeEntry{No=2,SubjectCode="COMP 010",   SubjectTitle="Information Management",                             Units=3,Grade=86,Equivalent=1.75,Remarks="PASSED"},
            new GradeEntry{No=3,SubjectCode="COMP 012",   SubjectTitle="Network Administration",                             Units=3,Grade=84,Equivalent=1.75,Remarks="PASSED"},
            new GradeEntry{No=4,SubjectCode="COMP 013",   SubjectTitle="Human Computer Interaction",                        Units=3,Grade=89,Equivalent=1.50,Remarks="PASSED"},
            new GradeEntry{No=5,SubjectCode="COMP 014",   SubjectTitle="Quantitative Methods with Modeling and Simulation",  Units=3,Grade=76,Equivalent=2.25,Remarks="PASSED"},
            new GradeEntry{No=6,SubjectCode="ELEC IT-FE2",SubjectTitle="BSIT Free Elective 2",                               Units=3,Grade=81,Equivalent=2.00,Remarks="PASSED"},
            new GradeEntry{No=7,SubjectCode="INTE 202",   SubjectTitle="Interactive Programming and Technologies 1",        Units=3,Grade=93,Equivalent=1.00,Remarks="PASSED"},
            new GradeEntry{No=8,SubjectCode="PATHFIT 4",  SubjectTitle="Physical Activity Towards Health and Fitness 4",    Units=2,Grade=85,Equivalent=1.75,Remarks="PASSED"},
        };

        //  2nd Semester data 
        private readonly List<GradeEntry> _midterm2 = new List<GradeEntry>
        {
            new GradeEntry{No=1,SubjectCode="COMP 015",   SubjectTitle="Software Engineering",                                  Units=3,Grade=90,Equivalent=1.25,Remarks="PASSED"},
            new GradeEntry{No=2,SubjectCode="COMP 016",   SubjectTitle="Systems Analysis and Design",                          Units=3,Grade=87,Equivalent=1.50,Remarks="PASSED"},
            new GradeEntry{No=3,SubjectCode="COMP 017",   SubjectTitle="Web Systems and Technologies",                         Units=3,Grade=84,Equivalent=1.75,Remarks="PASSED"},
            new GradeEntry{No=4,SubjectCode="COMP 018",   SubjectTitle="Technopreneurship",                                    Units=3,Grade=91,Equivalent=1.25,Remarks="PASSED"},
            new GradeEntry{No=5,SubjectCode="COMP 019",   SubjectTitle="Mobile Application Development",                       Units=3,Grade=79,Equivalent=2.25,Remarks="PASSED"},
            new GradeEntry{No=6,SubjectCode="ELEC IT-FE3",SubjectTitle="BSIT Free Elective 3",                                 Units=3,Grade=82,Equivalent=2.00,Remarks="PASSED"},
            new GradeEntry{No=7,SubjectCode="INTE 203",   SubjectTitle="Interactive Programming and Technologies 2",           Units=3,Grade=94,Equivalent=1.00,Remarks="PASSED"},
            new GradeEntry{No=8,SubjectCode="PATHFIT 5",  SubjectTitle="Physical Activity Towards Health and Fitness 5",       Units=2,Grade=86,Equivalent=1.75,Remarks="PASSED"},
        };

        private readonly List<GradeEntry> _final2 = new List<GradeEntry>
        {
            new GradeEntry{No=1,SubjectCode="COMP 015",   SubjectTitle="Software Engineering",                                  Units=3,Grade=88,Equivalent=1.50,Remarks="PASSED"},
            new GradeEntry{No=2,SubjectCode="COMP 016",   SubjectTitle="Systems Analysis and Design",                          Units=3,Grade=85,Equivalent=1.75,Remarks="PASSED"},
            new GradeEntry{No=3,SubjectCode="COMP 017",   SubjectTitle="Web Systems and Technologies",                         Units=3,Grade=83,Equivalent=1.75,Remarks="PASSED"},
            new GradeEntry{No=4,SubjectCode="COMP 018",   SubjectTitle="Technopreneurship",                                    Units=3,Grade=90,Equivalent=1.25,Remarks="PASSED"},
            new GradeEntry{No=5,SubjectCode="COMP 019",   SubjectTitle="Mobile Application Development",                       Units=3,Grade=77,Equivalent=2.25,Remarks="PASSED"},
            new GradeEntry{No=6,SubjectCode="ELEC IT-FE3",SubjectTitle="BSIT Free Elective 3",                                 Units=3,Grade=80,Equivalent=2.00,Remarks="PASSED"},
            new GradeEntry{No=7,SubjectCode="INTE 203",   SubjectTitle="Interactive Programming and Technologies 2",           Units=3,Grade=92,Equivalent=1.00,Remarks="PASSED"},
            new GradeEntry{No=8,SubjectCode="PATHFIT 5",  SubjectTitle="Physical Activity Towards Health and Fitness 5",       Units=2,Grade=84,Equivalent=1.75,Remarks="PASSED"},
        };

        //   AY 2024-2025  1st Semester 
        private readonly List<GradeEntry> _ay2425_sem1_mid = new List<GradeEntry>
        {
            new GradeEntry{No=1,SubjectCode="COMP 005",   SubjectTitle="Data Structures and Algorithms",                       Units=3,Grade=89,Equivalent=1.50,Remarks="PASSED"},
            new GradeEntry{No=2,SubjectCode="COMP 006",   SubjectTitle="Operating Systems",                                    Units=3,Grade=85,Equivalent=1.75,Remarks="PASSED"},
            new GradeEntry{No=3,SubjectCode="COMP 007",   SubjectTitle="Computer Organization and Architecture",               Units=3,Grade=82,Equivalent=2.00,Remarks="PASSED"},
            new GradeEntry{No=4,SubjectCode="COMP 008",   SubjectTitle="Discrete Mathematics",                                 Units=3,Grade=91,Equivalent=1.25,Remarks="PASSED"},
            new GradeEntry{No=5,SubjectCode="GEED 006",   SubjectTitle="Ethics",                                               Units=3,Grade=87,Equivalent=1.50,Remarks="PASSED"},
            new GradeEntry{No=6,SubjectCode="GEED 007",   SubjectTitle="Science, Technology and Society",                      Units=3,Grade=84,Equivalent=1.75,Remarks="PASSED"},
            new GradeEntry{No=7,SubjectCode="ELEC IT-FE1",SubjectTitle="BSIT Free Elective 1",                                 Units=3,Grade=90,Equivalent=1.25,Remarks="PASSED"},
            new GradeEntry{No=8,SubjectCode="PATHFIT 3",  SubjectTitle="Physical Activity Towards Health and Fitness 3",       Units=2,Grade=86,Equivalent=1.75,Remarks="PASSED"},
        };

        private readonly List<GradeEntry> _ay2425_sem1_final = new List<GradeEntry>
        {
            new GradeEntry{No=1,SubjectCode="COMP 005",   SubjectTitle="Data Structures and Algorithms",                       Units=3,Grade=87,Equivalent=1.50,Remarks="PASSED"},
            new GradeEntry{No=2,SubjectCode="COMP 006",   SubjectTitle="Operating Systems",                                    Units=3,Grade=83,Equivalent=1.75,Remarks="PASSED"},
            new GradeEntry{No=3,SubjectCode="COMP 007",   SubjectTitle="Computer Organization and Architecture",               Units=3,Grade=80,Equivalent=2.00,Remarks="PASSED"},
            new GradeEntry{No=4,SubjectCode="COMP 008",   SubjectTitle="Discrete Mathematics",                                 Units=3,Grade=90,Equivalent=1.25,Remarks="PASSED"},
            new GradeEntry{No=5,SubjectCode="GEED 006",   SubjectTitle="Ethics",                                               Units=3,Grade=85,Equivalent=1.75,Remarks="PASSED"},
            new GradeEntry{No=6,SubjectCode="GEED 007",   SubjectTitle="Science, Technology and Society",                      Units=3,Grade=82,Equivalent=2.00,Remarks="PASSED"},
            new GradeEntry{No=7,SubjectCode="ELEC IT-FE1",SubjectTitle="BSIT Free Elective 1",                                 Units=3,Grade=88,Equivalent=1.50,Remarks="PASSED"},
            new GradeEntry{No=8,SubjectCode="PATHFIT 3",  SubjectTitle="Physical Activity Towards Health and Fitness 3",       Units=2,Grade=84,Equivalent=1.75,Remarks="PASSED"},
        };

        //  AY 2024-2025  2nd Semester 
        private readonly List<GradeEntry> _ay2425_sem2_mid = new List<GradeEntry>
        {
            new GradeEntry{No=1,SubjectCode="COMP 009",   SubjectTitle="Object Oriented Programming",                          Units=3,Grade=88,Equivalent=1.50,Remarks="PASSED"},
            new GradeEntry{No=2,SubjectCode="COMP 010",   SubjectTitle="Information Management",                               Units=3,Grade=84,Equivalent=1.75,Remarks="PASSED"},
            new GradeEntry{No=3,SubjectCode="COMP 011",   SubjectTitle="Social Issues and Professional Practice",              Units=3,Grade=91,Equivalent=1.25,Remarks="PASSED"},
            new GradeEntry{No=4,SubjectCode="GEED 008",   SubjectTitle="Art Appreciation",                                     Units=3,Grade=93,Equivalent=1.00,Remarks="PASSED"},
            new GradeEntry{No=5,SubjectCode="GEED 009",   SubjectTitle="The Contemporary World",                               Units=3,Grade=86,Equivalent=1.75,Remarks="PASSED"},
            new GradeEntry{No=6,SubjectCode="MATH 003",   SubjectTitle="Numerical Methods",                                    Units=3,Grade=79,Equivalent=2.25,Remarks="PASSED"},
            new GradeEntry{No=7,SubjectCode="INTE 201",   SubjectTitle="Integrative Programming and Technologies",             Units=3,Grade=92,Equivalent=1.00,Remarks="PASSED"},
            new GradeEntry{No=8,SubjectCode="PATHFIT 4",  SubjectTitle="Physical Activity Towards Health and Fitness 4",       Units=2,Grade=87,Equivalent=1.50,Remarks="PASSED"},
        };

        private readonly List<GradeEntry> _ay2425_sem2_final = new List<GradeEntry>
        {
            new GradeEntry{No=1,SubjectCode="COMP 009",   SubjectTitle="Object Oriented Programming",                          Units=3,Grade=86,Equivalent=1.75,Remarks="PASSED"},
            new GradeEntry{No=2,SubjectCode="COMP 010",   SubjectTitle="Information Management",                               Units=3,Grade=82,Equivalent=2.00,Remarks="PASSED"},
            new GradeEntry{No=3,SubjectCode="COMP 011",   SubjectTitle="Social Issues and Professional Practice",              Units=3,Grade=89,Equivalent=1.50,Remarks="PASSED"},
            new GradeEntry{No=4,SubjectCode="GEED 008",   SubjectTitle="Art Appreciation",                                     Units=3,Grade=91,Equivalent=1.25,Remarks="PASSED"},
            new GradeEntry{No=5,SubjectCode="GEED 009",   SubjectTitle="The Contemporary World",                               Units=3,Grade=84,Equivalent=1.75,Remarks="PASSED"},
            new GradeEntry{No=6,SubjectCode="MATH 003",   SubjectTitle="Numerical Methods",                                    Units=3,Grade=76,Equivalent=2.25,Remarks="PASSED"},
            new GradeEntry{No=7,SubjectCode="INTE 201",   SubjectTitle="Integrative Programming and Technologies",             Units=3,Grade=90,Equivalent=1.25,Remarks="PASSED"},
            new GradeEntry{No=8,SubjectCode="PATHFIT 4",  SubjectTitle="Physical Activity Towards Health and Fitness 4",       Units=2,Grade=85,Equivalent=1.75,Remarks="PASSED"},
        };

        private readonly string[,] _scale =
        {
            {"1.00","97-100","Excellent"},  {"1.25","94-96","Excellent"},    {"1.50","91-93","Very Good"},
            {"1.75","88-90","Very Good"},   {"2.00","85-87","Good"},         {"2.25","82-84","Good"},
            {"2.50","79-81","Satisfactory"},{"2.75","76-78","Satisfactory"}, {"3.00","75","Passing"},
            {"4.00","68-74","Conditional"}, {"5.00","Below 68","Failed"},    {"Inc.","–","Incomplete"},
            {"W","–","Withdrawal"},         {"P","–","Passed (Non-credit)"}, {"","",""}
        };

        private readonly List<string> _notes = new List<string>();
        private bool _isMidterm = true;

        private DataTable _dtMid, _dtFinal, _dtMid2, _dtFinal2;
        private DataTable _dt2425s1Mid, _dt2425s1Final, _dt2425s2Mid, _dt2425s2Final;

        public GradesPanel()
        {
            InitializeComponent();

            // Initialization setup
            cmbSemester.SelectedIndex = 0;
            cmbAcYear.SelectedIndex = 1;

            BuildDataTables();
            BindGrids();

            // Build fixed Scale setup
            string[] colNames = { "Grade", "Percentage", "Description", "Grade", "Percentage", "Description", "Grade", "Percentage", "Description" };
            foreach (var n in colNames)
                dgvScale.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = n });
            PopulateGradeScale();

            // Wire semester & academic year switchers after everything is built
            cmbSemester.SelectedIndexChanged += CmbFilterChanged;
            cmbAcYear.SelectedIndexChanged += CmbFilterChanged;

            // Apply formatting globally
            dgvMid.CellFormatting += DgGrades_CellFormatting;
            dgvFinal.CellFormatting += DgGrades_CellFormatting;

            RefreshAll();
        }

        //  DATA / BINDING
        private void BuildDataTables()
        {
            // AY 2025-2026
            _dtMid = CreateDT(_midterm);
            _dtFinal = CreateDT(_final);
            _dtMid2 = CreateDT(_midterm2);
            _dtFinal2 = CreateDT(_final2);

            // AY 2024-2025
            _dt2425s1Mid = CreateDT(_ay2425_sem1_mid);
            _dt2425s1Final = CreateDT(_ay2425_sem1_final);
            _dt2425s2Mid = CreateDT(_ay2425_sem2_mid);
            _dt2425s2Final = CreateDT(_ay2425_sem2_final);
        }

        private static DataTable CreateDT(List<GradeEntry> src)
        {
            var dt = new DataTable();
            dt.Columns.Add("#", typeof(int));
            dt.Columns.Add("Subject Code", typeof(string));
            dt.Columns.Add("Subject Title", typeof(string));
            dt.Columns.Add("Units", typeof(int));
            dt.Columns.Add("Grade", typeof(string));
            dt.Columns.Add("Equivalent", typeof(string));
            dt.Columns.Add("Remarks", typeof(string));

            foreach (var e in src)
                dt.Rows.Add(e.No, e.SubjectCode, e.SubjectTitle, e.Units,
                            e.Grade.ToString("F0"), e.Equivalent.ToString("F2"), e.Remarks);

            return dt;
        }

        private void BindGrids()
        {
            dgvMid.DataSource = _dtMid.DefaultView;
            dgvFinal.DataSource = _dtFinal.DefaultView;
        }

        private void PopulateGradeScale()
        {
            dgvScale.Rows.Clear();
            int totalEntries = _scale.GetLength(0);
            int rows = (int)Math.Ceiling(totalEntries / 3.0);

            for (int r = 0; r < rows; r++)
            {
                var vals = new object[9];
                for (int col = 0; col < 3; col++)
                {
                    int idx = r * 3 + col;
                    if (idx < totalEntries)
                    {
                        vals[col * 3 + 0] = _scale[idx, 0];
                        vals[col * 3 + 1] = _scale[idx, 1];
                        vals[col * 3 + 2] = _scale[idx, 2];
                    }
                }
                dgvScale.Rows.Add(vals);
            }
        }

        //  REFRESH / FILTER
        private void RefreshAll()
        {
            FilterTable();
            UpdateSummaryCards();
            pnlTrendChart?.Invalidate();
            pnlPieChart?.Invalidate();
        }

        private void FilterTable()
        {
            string term = txtSearch?.Text.Trim().ToLower() ?? "";
            var (tMid, tFinal) = GetActiveTables();
            var src = _isMidterm ? tMid : tFinal;

            var dv = _isMidterm ? dgvMid?.DataSource as DataView : dgvFinal?.DataSource as DataView;
            if (dv == null) return;

            dv.RowFilter = string.IsNullOrEmpty(term)
                ? ""
                : $"([Subject Code] LIKE '%{term}%' OR [Subject Title] LIKE '%{term}%')";

            int vis = dv.Count;
            int total = src.Rows.Count;
            if (lblPageInfo != null)
                lblPageInfo.Text = $"Showing 1 to {vis} of {total} subjects";
        }

        private void UpdateSummaryCards()
        {
            var (activeMid, activeFinal) = GetActiveData();
            var data = _isMidterm ? activeMid : activeFinal;

            if (data == null || data.Count == 0) return;

            int totalU = data.Sum(e => e.Units);
            int earned = data.Where(e => e.Remarks == "PASSED").Sum(e => e.Units);
            int passed = data.Count(e => e.Remarks == "PASSED");
            int failed = data.Count(e => e.Remarks == "FAILED");
            int ip = data.Count(e => e.Remarks == "INC" || e.Remarks == "IN PROGRESS");

            double tw = 0;
            int tp = 0;
            foreach (var e in data.Where(e => e.Remarks == "PASSED"))
            {
                tw += e.Equivalent * e.Units;
                tp += e.Units;
            }
            double gwa = tp > 0 ? tw / tp : 0;

            lblGWA.Text = gwa > 0 ? gwa.ToString("F2") : "—";
            lblTotalUnits.Text = totalU.ToString();
            lblUnitsEarned.Text = earned.ToString();
            lblPassed.Text = passed.ToString();
            lblFailed.Text = failed.ToString();
            lblInProgress.Text = ip.ToString();

            if (gwa > 0 && gwa <= 1.75) lblGWA.ForeColor = Color.FromArgb(22, 163, 74);
            else if (gwa > 0 && gwa <= 2.50) lblGWA.ForeColor = Color.FromArgb(217, 119, 6);
            else if (gwa > 0) lblGWA.ForeColor = Color.FromArgb(220, 38, 38);
        }

        //  Resolve active dataset from both dropdowns 
        private (List<GradeEntry> mid, List<GradeEntry> final) GetActiveData()
        {
            bool is2nd = cmbSemester?.SelectedIndex == 1;
            bool is2425 = cmbAcYear?.SelectedIndex == 0;

            if (is2425)
                return is2nd
                    ? (_ay2425_sem2_mid, _ay2425_sem2_final)
                    : (_ay2425_sem1_mid, _ay2425_sem1_final);
            else
                return is2nd
                    ? (_midterm2, _final2)
                    : (_midterm, _final);
        }

        private (DataTable mid, DataTable final) GetActiveTables()
        {
            bool is2nd = cmbSemester?.SelectedIndex == 1;
            bool is2425 = cmbAcYear?.SelectedIndex == 0;

            if (is2425)
                return is2nd
                    ? (_dt2425s2Mid, _dt2425s2Final)
                    : (_dt2425s1Mid, _dt2425s1Final);
            else
                return is2nd
                    ? (_dtMid2, _dtFinal2)
                    : (_dtMid, _dtFinal);
        }

        private void CmbFilterChanged(object sender, EventArgs e)
        {
            var (tMid, tFinal) = GetActiveTables();
            dgvMid.DataSource = tMid.DefaultView;
            dgvFinal.DataSource = tFinal.DefaultView;

            // Re-detect grading period
            var (activeMid, _) = GetActiveData();
            cmbGradingPeriod.SelectedIndex = DetectGradingPeriod(activeMid) ? 0 : 1;

            if (txtSearch != null) txtSearch.Text = "";
            RefreshAll();
        }

        private static bool DetectGradingPeriod(List<GradeEntry> entries)
        {
            if (entries == null || entries.Count == 0) return true;
            int totalUnits = entries.Sum(e => e.Units);
            bool uniqueCodes = entries.Select(e => e.SubjectCode).Distinct().Count() == entries.Count;
            bool noFailed = !entries.Any(e => e.Remarks == "FAILED" || e.Remarks == "INC");
            bool normalLoad = totalUnits >= 15 && totalUnits <= 25;
            return uniqueCodes && noFailed && normalLoad;
        }

        //  CHART PAINTING
        private void PnlTrendChart_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var pnl = (Panel)sender;
            int pw = pnl.Width, ph = pnl.Height;
            if (pw < 10 || ph < 10) return;

            bool perYear = cmbTrend?.SelectedIndex == 1;
            double[] midVals, finalVals;
            string[] labels;

            if (!perYear)
            {
                midVals = new[] { 1.53, 1.75, 1.88, 2.00 };
                finalVals = new[] { 1.60, 1.70, 1.80, 1.95 };
                labels = new[] { "2nd'23", "1st'24", "2nd'24", "1st'25" };
            }
            else
            {
                midVals = new[] { 1.64, 1.72, 1.81 };
                finalVals = new[] { 1.68, 1.75, 1.85 };
                labels = new[] { "2023", "2024", "2025" };
            }

            double[] vals = _isMidterm ? midVals : finalVals;
            int count = vals.Length;
            double minV = vals.Min(), maxV = vals.Max();
            double range = maxV - minV < 0.01 ? 0.5 : maxV - minV;
            int padL = 10, padR = 10, padT = 14, padB = 26;
            int chartW = pw - padL - padR;
            int chartH = ph - padT - padB;
            int barW = Math.Max(4, chartW / count - 6);

            using (var br = new SolidBrush(Color.FromArgb(128, 0, 0)))
            using (var textBr = new SolidBrush(Color.FromArgb(100, 100, 100)))
            using (var valBr = new SolidBrush(Color.FromArgb(60, 60, 60)))
            using (var fSmall = new Font("Segoe UI", 8f))
            {
                for (int i = 0; i < count; i++)
                {
                    double norm = (vals[i] - minV) / range;
                    int bH = (int)(norm * chartH * 0.7 + chartH * 0.2);
                    int bx = padL + i * (chartW / count) + 3;
                    int by = padT + chartH - bH;
                    g.FillRectangle(br, new Rectangle(bx, by, barW, bH));

                    string vStr = vals[i].ToString("F2");
                    var vSz = g.MeasureString(vStr, fSmall);
                    g.DrawString(vStr, fSmall, valBr, bx + (barW - vSz.Width) / 2f, by - vSz.Height - 1);

                    var lSz = g.MeasureString(labels[i], fSmall);
                    g.DrawString(labels[i], fSmall, textBr, bx + (barW - lSz.Width) / 2f, ph - padB + 4);
                }
            }
        }

        private void PnlPieChart_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var pnl = (Panel)sender;
            if (pnl.Width < 20 || pnl.Height < 20) return;

            var data = _isMidterm ? _midterm : _final;
            var buckets = new Dictionary<string, int>
            {
                {"1.00-1.25", data.Count(x => x.Equivalent <= 1.25)},
                {"1.50-1.75", data.Count(x => x.Equivalent > 1.25 && x.Equivalent <= 1.75)},
                {"2.00-2.25", data.Count(x => x.Equivalent > 1.75 && x.Equivalent <= 2.25)},
                {"2.50-3.00", data.Count(x => x.Equivalent > 2.25)},
            };

            Color[] colors = {Color.FromArgb(128,0,0), Color.FromArgb(180,83,9),
                               Color.FromArgb(21,128,61), Color.FromArgb(29,78,216)};
            int total = data.Count;
            if (total == 0) return;

            int pieSize = Math.Min(80, pnl.Height - 10);
            int pieX = 8, pieY = (pnl.Height - pieSize) / 2;
            float startAngle = -90f;
            int ci = 0;

            using (var fSmall = new Font("Segoe UI", 8f))
            using (var textBr = new SolidBrush(Color.FromArgb(80, 80, 80)))
            {
                foreach (var kv in buckets)
                {
                    if (kv.Value == 0) { ci++; continue; }
                    float sweep = (kv.Value / (float)total) * 360f;
                    using (var br = new SolidBrush(colors[ci]))
                        g.FillPie(br, pieX, pieY, pieSize, pieSize, startAngle, sweep);
                    startAngle += sweep;

                    int ly = pieY + ci * 20;
                    g.FillRectangle(new SolidBrush(colors[ci]), pieX + pieSize + 12, ly + 4, 10, 10);
                    g.DrawString($"{kv.Key}: {kv.Value}", fSmall, textBr, pieX + pieSize + 26, ly);
                    ci++;
                }
            }
        }

        //  EVENT HANDLERS
        private void DgGrades_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var grid = (DataGridView)sender;
            int remIdx = grid.Columns["Remarks"]?.Index ?? -1;
            if (e.ColumnIndex != remIdx || e.Value == null) return;
            switch (e.Value.ToString())
            {
                case "PASSED":
                    e.CellStyle.ForeColor = Color.FromArgb(22, 163, 74);
                    e.CellStyle.Font = new Font("Segoe UI", 9f);
                    break;
                case "FAILED":
                    e.CellStyle.ForeColor = Color.FromArgb(220, 38, 38);
                    e.CellStyle.Font = new Font("Segoe UI", 9f);
                    break;
                case "INC":
                    e.CellStyle.ForeColor = Color.FromArgb(217, 119, 6);
                    break;
            }
        }

        private void DgGrades_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            var dg = (DataGridView)sender;
            if (dg.Columns.Count == 0) return;
            string[] autoSizeCols = { "#", "Subject Code", "Subject Title" };
            foreach (DataGridViewColumn col in dg.Columns)
            {
                if (Array.IndexOf(autoSizeCols, col.HeaderText) >= 0)
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                else
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }

        private void TabGrades_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tc = (TabControl)sender;
            var tab = tc.TabPages[e.Index];
            bool sel = e.Index == tc.SelectedIndex;
            e.Graphics.FillRectangle(
                sel ? new SolidBrush(Color.White) : new SolidBrush(Color.FromArgb(245, 245, 245)),
                e.Bounds);
            if (sel)
            {
                using (var thick = new Pen(Color.Maroon, 2))
                    e.Graphics.DrawLine(thick, e.Bounds.Left, e.Bounds.Top + 1, e.Bounds.Right - 1, e.Bounds.Top + 1);
            }
            using (var br = new SolidBrush(sel ? Color.FromArgb(128, 0, 0) : Color.FromArgb(100, 100, 100)))
            using (var f = new Font("Segoe UI", 9f))
            {
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString(tab.Text, f, br, e.Bounds, sf);
            }
        }

        private void TabGrades_SelectedIndexChanged(object sender, EventArgs e)
        {
            _isMidterm = tabGrades.SelectedIndex == 0;
            FilterTable();
            UpdateSummaryCards();
            pnlPieChart?.Invalidate();
            pnlTrendChart?.Invalidate();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e) => FilterTable();

        private void CmbTrend_SelectedIndexChanged(object sender, EventArgs e) => pnlTrendChart?.Invalidate();

        private void BtnGenerateCOG_Click(object sender, EventArgs e)
        {
            bool is2nd = cmbSemester?.SelectedIndex == 1;
            bool is2425 = cmbAcYear?.SelectedIndex == 0;
            string ayLabel = is2425 ? "2024 - 2025" : "2025 - 2026";

            // NOTE: Ensure your original `CogGenerator` class remains in your project to handle generation.
            /* List<GradeEntry> sem1Raw, sem2Raw;
            if (is2425) { ... }
            
            using (var sfd = new SaveFileDialog { ... })
            {
                ...
                CogGenerator.Generate(sfd.FileName, logoPath, ayLabel, sem1, sem2);
                ShowToast("COG generated successfully!");
            }
            */
            ShowToast("Function disabled in designer demo.");
        }

        private void BtnAddNote_Click(object sender, EventArgs e)
        {
            using (var dlg = new NoteDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dlg.NoteText))
                {
                    _notes.Add(dlg.NoteText.Trim());
                    RefreshNotes();
                }
            }
        }

        private void RefreshNotes()
        {
            flpNotes.Controls.Clear();
            if (_notes.Count == 0) { flpNotes.Controls.Add(lblNoNotes); return; }

            for (int i = 0; i < _notes.Count; i++)
            {
                int captured = i;
                var row = new Panel { Width = flpNotes.Width - 4, Height = 28, BackColor = Color.White };

                var lbl = new Label { Text = _notes[i], Font = new Font("Segoe UI", 10F, FontStyle.Regular), ForeColor = Color.FromArgb(60, 60, 60), AutoSize = true, Location = new Point(4, 6), Width = row.Width - 28, AutoEllipsis = true };
                row.Controls.Add(lbl);

                var btn = new Button
                {
                    Text = "×",
                    Location = new Point(row.Width - 24, 4),
                    Size = new Size(20, 20),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9f),
                    ForeColor = Color.Gray,
                    BackColor = Color.Transparent,
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += (s2, e2) => { _notes.RemoveAt(captured); RefreshNotes(); };
                row.Controls.Add(btn);
                flpNotes.Controls.Add(row);

                flpNotes.Controls.Add(new Panel { Width = flpNotes.Width - 4, Height = 1, BackColor = Color.FromArgb(230, 230, 230) });
            }
        }

        private void ShowToast(string msg)
        {
            var toast = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual,
                BackColor = Color.FromArgb(22, 163, 74),
                Size = new Size(240, 36),
                ShowInTaskbar = false,
                TopMost = true,
                Opacity = 0
            };
            toast.Controls.Add(new Label
            {
                Text = msg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            });
            var screen = Screen.PrimaryScreen.WorkingArea;
            toast.Location = new Point(screen.Right - 260, screen.Bottom - 50);
            toast.Show();

            var fade = new System.Windows.Forms.Timer { Interval = 30 };
            int step = 0;
            fade.Tick += (s, e) =>
            {
                step++;
                if (step < 10) toast.Opacity = step * 0.1;
                else if (step < 40) toast.Opacity = 1;
                else if (step < 50) toast.Opacity = 1 - (step - 40) * 0.1;
                else { fade.Stop(); toast.Close(); }
            };
            fade.Start();
        }
    }

    //  HELPER DIALOG  –  Add Note
    internal class NoteDialog : Form
    {
        public string NoteText { get; private set; } = "";

        public NoteDialog()
        {
            Text = "Add Note";
            Size = new Size(360, 150);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false; MinimizeBox = false;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 9f);

            var lbl = new Label { Text = "Note:", Location = new Point(12, 16), AutoSize = true };
            var txt = new TextBox { Location = new Point(12, 36), Width = 318, Height = 26, BorderStyle = BorderStyle.FixedSingle };
            var btnOk = new Button
            {
                Text = "Save",
                DialogResult = DialogResult.OK,
                Location = new Point(172, 74),
                Size = new Size(80, 28),
                BackColor = Color.FromArgb(128, 0, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnOk.FlatAppearance.BorderSize = 0;
            var btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new Point(260, 74),
                Size = new Size(70, 28),
                FlatStyle = FlatStyle.Flat
            };
            btnOk.Click += (s, e) => NoteText = txt.Text;
            Controls.AddRange(new Control[] { lbl, txt, btnOk, btnCancel });
            AcceptButton = btnOk; CancelButton = btnCancel;
        }
    }
}