using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Student.LMS.Grades
{
    public class GradesPanel : UserControl
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
            // Row 0
            {"1.00","97-100","Excellent"},  {"1.25","94-96","Excellent"},    {"1.50","91-93","Very Good"},
            // Row 1
            {"1.75","88-90","Very Good"},   {"2.00","85-87","Good"},          {"2.25","82-84","Good"},
            // Row 2
            {"2.50","79-81","Satisfactory"},{"2.75","76-78","Satisfactory"},  {"3.00","75","Passing"},
            // Row 3
            {"4.00","68-74","Conditional"}, {"5.00","Below 68","Failed"},     {"Inc.","–","Incomplete"},
            // Row 4
            {"W","–","Withdrawal"},         {"P","–","Passed (Non-credit)"},  {"","",""},
        };

        private readonly List<string> _notes = new List<string>();
        private bool _isMidterm = true;

        private ComboBox cmbSemester, cmbAcYear, cmbGradingPeriod, cmbTrend;
        private Button btnGenerateCOG, btnAddNote;
        private Label lblGWA, lblTotalUnits, lblUnitsEarned, lblPassed, lblFailed, lblInProgress;
        private TabControl tabGrades;
        private DataGridView dgvMid, dgvFinal;
        private TextBox txtSearch;
        private Label lblPageInfo;
        private Panel pnlTrendChart, pnlPieChart;
        private DataGridView dgvScale;
        private FlowLayoutPanel flpNotes;
        private Label lblNoNotes;

        private DataTable _dtMid, _dtFinal, _dtMid2, _dtFinal2;
        private DataTable _dt2425s1Mid, _dt2425s1Final, _dt2425s2Mid, _dt2425s2Final;

        public GradesPanel()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.Dock = DockStyle.Fill;
            this.AutoScroll = true;
            BuildUI();
            BuildDataTables();
            BindGrids();
            // Wire semester & academic year switchers after everything is built
            cmbSemester.SelectedIndexChanged += CmbFilterChanged;
            cmbAcYear.SelectedIndexChanged += CmbFilterChanged;
            RefreshAll();
        }

        //  UI CONSTRUCTION  –  fully responsive via TableLayoutPanel
        private void BuildUI()
        {
            //  outer scroll container 
            var scroll = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(245, 245, 245),
                Padding = new Padding(14, 10, 14, 20)
            };
            this.Controls.Add(scroll);

            //  vertical stack (TableLayoutPanel keeps everything responsive) 
            var stack = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 1,
                RowCount = 6,
                BackColor = Color.Transparent,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            stack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            for (int i = 0; i < 6; i++)
                stack.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            scroll.Controls.Add(stack);
            stack.BringToFront();

            //  0. Title 
            var lblTitle = MakeLabel("Grades", 18, FontStyle.Bold, Color.FromArgb(33, 33, 33));
            lblTitle.AutoSize = true;
            lblTitle.Margin = new Padding(0, 0, 0, 8);
            stack.Controls.Add(lblTitle, 0, 0);

            //  1. Filter bar 
            stack.Controls.Add(BuildFilterBar(), 0, 1);

            //  2. Summary cards 
            stack.Controls.Add(BuildSummaryCards(), 0, 2);

            //  3. Main area (grid + charts side by side) 
            stack.Controls.Add(BuildMainArea(), 0, 3);

            //  4. Grade scale 
            stack.Controls.Add(BuildScaleSection(), 0, 4);
        }

        //  Filter bar 
        private Panel BuildFilterBar()
        {
            var bar = new Panel
            {
                BackColor = Color.White,
                Height = 62,
                Dock = DockStyle.Top,
                Padding = new Padding(10, 0, 10, 0),
                Margin = new Padding(0, 0, 0, 8)
            };

            // Left controls
            int fx = 10;
            bar.Controls.Add(MakeLabel("Semester", 10, FontStyle.Regular, Color.FromArgb(100, 100, 100), new Point(fx, 6)));
            cmbSemester = MakeCombo(new[] { "1st Semester", "2nd Semester" }, new Point(fx, 24), 130);
            bar.Controls.Add(cmbSemester);
            fx += 140;

            bar.Controls.Add(MakeLabel("Academic Year", 10, FontStyle.Regular, Color.FromArgb(100, 100, 100), new Point(fx, 6)));
            cmbAcYear = MakeCombo(new[] { "2024 - 2025", "2025 - 2026" }, new Point(fx, 24), 130);
            cmbAcYear.SelectedIndex = 1;
            bar.Controls.Add(cmbAcYear);
            fx += 140;

            bar.Controls.Add(MakeLabel("Grading Period", 10, FontStyle.Regular, Color.FromArgb(100, 100, 100), new Point(fx, 6)));
            cmbGradingPeriod = MakeCombo(new[] { "Regular", "Irregular" }, new Point(fx, 24), 130);
            cmbGradingPeriod.DropDownStyle = ComboBoxStyle.DropDownList;
            // Auto-detect: Regular = 8 subjects, all unique codes, standard units
            cmbGradingPeriod.SelectedIndex = DetectGradingPeriod(_midterm) ? 0 : 1;
            cmbGradingPeriod.Enabled = false; // read-only, auto-detected
            bar.Controls.Add(cmbGradingPeriod);

            // Right-anchored buttons
            btnGenerateCOG = MakeMaroonButton("Generate COG", new Point(0, 24), 120, 28);
            btnGenerateCOG.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGenerateCOG.Click += BtnGenerateCOG_Click;
            bar.Controls.Add(btnGenerateCOG);

            // Position right-anchored button once bar knows its width
            bar.SizeChanged += (s, e) =>
            {
                btnGenerateCOG.Left = bar.Width - 10 - btnGenerateCOG.Width;
            };

            return bar;
        }

        //  Summary cards 
        private Panel BuildSummaryCards()
        {
            var host = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 80,
                ColumnCount = 6,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 8),
                Padding = new Padding(0)
            };
            for (int i = 0; i < 6; i++)
                host.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 6f));
            host.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            var titles = new[]
            {
                "General Weighted Average","Total Units","Units Earned",
                "Passed","Failed","In Progress"
            };
            var vals = new Label[6];

            for (int i = 0; i < 6; i++)
            {
                var card = new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.White,
                    Margin = new Padding(i == 0 ? 0 : 4, 0, i == 5 ? 0 : 0, 0)
                };
                var lTitle = MakeLabel(titles[i], 9, FontStyle.Regular, Color.FromArgb(110, 110, 110));
                lTitle.Location = new Point(10, 8); lTitle.AutoSize = true;
                card.Controls.Add(lTitle);

                vals[i] = MakeLabel("—", 18, FontStyle.Bold, Color.FromArgb(33, 33, 33));
                vals[i].Location = new Point(10, 28); vals[i].AutoSize = true;
                card.Controls.Add(vals[i]);

                host.Controls.Add(card, i, 0);
            }

            lblGWA = vals[0];
            lblTotalUnits = vals[1];
            lblUnitsEarned = vals[2];
            lblPassed = vals[3];
            lblFailed = vals[4];
            lblInProgress = vals[5];

            return host;
        }

        //  Main area: left grid + right panel 
        private Control BuildMainArea()
        {
            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 540,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 8),
                Padding = new Padding(0)
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 71f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 29f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            // Left: subject grades card
            var left = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(0, 0, 8, 0) };

            var lbTitle = MakeLabel("Subject Grades", 12, FontStyle.Regular, Color.FromArgb(128, 0, 0));
            lbTitle.Location = new Point(10, 8); lbTitle.AutoSize = true;
            left.Controls.Add(lbTitle);

            txtSearch = new TextBox
            {
                PlaceholderText = "Search subject code or name...",
                Location = new Point(10, 30),
                Width = 290,
                Height = 26,
                Font = new Font("Segoe UI", 9f),
                BorderStyle = BorderStyle.FixedSingle
            };
            txtSearch.TextChanged += (s, e) => FilterTable();
            left.Controls.Add(txtSearch);

            tabGrades = new TabControl
            {
                Location = new Point(0, 64),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                Font = new Font("Segoe UI", 9f)
            };
            left.SizeChanged += (s, e) =>
            {
                tabGrades.Size = new Size(left.Width, left.Height - 64 - 22);
            };
            tabGrades.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabGrades.ItemSize = new Size(100, 26);
            tabGrades.DrawItem += TabGrades_DrawItem;
            tabGrades.SelectedIndexChanged += (s, e) =>
            {
                _isMidterm = tabGrades.SelectedIndex == 0;
                FilterTable();
                UpdateSummaryCards();
                pnlPieChart?.Invalidate();
                pnlTrendChart?.Invalidate();
            };
            left.Controls.Add(tabGrades);

            var tpMid = new TabPage("Mid Term");
            var tpFinal = new TabPage("Final Term");
            tabGrades.TabPages.Add(tpMid);
            tabGrades.TabPages.Add(tpFinal);

            dgvMid = MakeGradeGrid(tpMid.ClientSize);
            dgvFinal = MakeGradeGrid(tpFinal.ClientSize);
            tpMid.Controls.Add(dgvMid);
            tpFinal.Controls.Add(dgvFinal);
            dgvMid.Dock = dgvFinal.Dock = DockStyle.Fill;

            lblPageInfo = MakeLabel("Showing 1 to 8 of 8 subjects", 9, FontStyle.Regular, Color.FromArgb(130, 130, 130));
            lblPageInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblPageInfo.AutoSize = true;
            left.SizeChanged += (s, e) => lblPageInfo.Location = new Point(10, left.Height - 20);
            left.Controls.Add(lblPageInfo);

            tbl.Controls.Add(left, 0, 0);

            // Right panel
            tbl.Controls.Add(BuildRightPanel(), 1, 0);

            return tbl;
        }

        //  Right panel: trend + pie + notes 
        private Control BuildRightPanel()
        {
            var right = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            right.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            right.RowStyles.Add(new RowStyle(SizeType.Absolute, 170f));
            right.RowStyles.Add(new RowStyle(SizeType.Absolute, 160f));
            right.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            //  GWA Trend card 
            var trendCard = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(0, 0, 0, 8) };

            var trendTitle = MakeLabel("GWA Trend", 12, FontStyle.Regular, Color.FromArgb(33, 33, 33));
            trendTitle.Location = new Point(10, 8); trendTitle.AutoSize = true;
            trendCard.Controls.Add(trendTitle);

            cmbTrend = MakeCombo(new[] { "Per Semester", "Per Year" }, new Point(0, 6), 110);
            cmbTrend.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbTrend.SelectedIndexChanged += (s, e) => pnlTrendChart?.Invalidate();
            trendCard.Controls.Add(cmbTrend);
            trendCard.SizeChanged += (s, e) => cmbTrend.Left = trendCard.Width - cmbTrend.Width - 8;

            pnlTrendChart = new Panel { BackColor = Color.White };
            pnlTrendChart.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            trendCard.SizeChanged += (s, e) =>
            {
                pnlTrendChart.Location = new Point(8, 34);
                pnlTrendChart.Size = new Size(trendCard.Width - 16, trendCard.Height - 42);
            };
            pnlTrendChart.Paint += PnlTrendChart_Paint;
            trendCard.Controls.Add(pnlTrendChart);
            right.Controls.Add(trendCard, 0, 0);

            //  Grade Distribution card 
            var pieCard = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(0, 0, 0, 8) };

            var pieTitle = MakeLabel("Grade Distribution", 12, FontStyle.Regular, Color.FromArgb(33, 33, 33));
            pieTitle.Location = new Point(10, 8); pieTitle.AutoSize = true;
            pieCard.Controls.Add(pieTitle);

            pnlPieChart = new Panel { BackColor = Color.White };
            pnlPieChart.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            pieCard.SizeChanged += (s, e) =>
            {
                pnlPieChart.Location = new Point(8, 30);
                pnlPieChart.Size = new Size(pieCard.Width - 16, pieCard.Height - 38);
            };
            pnlPieChart.Paint += PnlPieChart_Paint;
            pieCard.Controls.Add(pnlPieChart);
            right.Controls.Add(pieCard, 0, 1);

            //  Quick Notes card 
            var notesCard = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            var notesTitle = MakeLabel("Quick Notes", 12, FontStyle.Regular, Color.FromArgb(33, 33, 33));
            notesTitle.Location = new Point(10, 8); notesTitle.AutoSize = true;
            notesCard.Controls.Add(notesTitle);

            flpNotes = new FlowLayoutPanel
            {
                BackColor = Color.White,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            notesCard.SizeChanged += (s, e) =>
            {
                flpNotes.Location = new Point(8, 34);
                flpNotes.Size = new Size(notesCard.Width - 16, notesCard.Height - 76);
            };

            lblNoNotes = MakeLabel("No notes for this grading period.", 10, FontStyle.Regular, Color.FromArgb(160, 160, 160));
            lblNoNotes.Location = new Point(4, 4); lblNoNotes.AutoSize = true;
            flpNotes.Controls.Add(lblNoNotes);
            notesCard.Controls.Add(flpNotes);

            btnAddNote = MakeOutlineButton("+ Add Note", new Point(8, 0), 0, 28);
            btnAddNote.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            notesCard.SizeChanged += (s, e) =>
            {
                btnAddNote.Width = notesCard.Width - 16;
                btnAddNote.Top = notesCard.Height - 36;
            };
            btnAddNote.Click += BtnAddNote_Click;
            notesCard.Controls.Add(btnAddNote);

            right.Controls.Add(notesCard, 0, 2);

            return right;
        }

        //  Grade Scale section 
        private Control BuildScaleSection()
        {
            var card = new Panel
            {
                Dock = DockStyle.Top,
                Height = 218,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 10),
                Padding = new Padding(8)
            };

            var title = MakeLabel("Grade Scale Reference", 12, FontStyle.Regular, Color.FromArgb(33, 33, 33));
            title.Location = new Point(10, 8); title.AutoSize = true;
            card.Controls.Add(title);

            dgvScale = new DataGridView
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(8, 32),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(220, 220, 220),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 9f)
            };
            dgvScale.EnableHeadersVisualStyles = false;
            dgvScale.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);
            dgvScale.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(60, 60, 60);
            dgvScale.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgvScale.ColumnHeadersHeight = 28;
            dgvScale.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 246, 248);
            dgvScale.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 33, 33);
            dgvScale.RowTemplate.Height = 22;

            card.SizeChanged += (s, e) =>
                dgvScale.Size = new Size(card.Width - 16, card.Height - 40);

            string[] colNames = { "Grade", "Percentage", "Description", "Grade", "Percentage", "Description", "Grade", "Percentage", "Description" };
            foreach (var n in colNames)
                dgvScale.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = n });

            PopulateGradeScale();
            card.Controls.Add(dgvScale);
            return card;
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
            foreach (var dg in new[] { dgvMid, dgvFinal })
            {
                dg.CellFormatting -= DgGrades_CellFormatting;
                dg.CellFormatting += DgGrades_CellFormatting;
            }
        }

        private void PopulateGradeScale()
        {
            dgvScale.Rows.Clear();
            int totalEntries = _scale.GetLength(0); // each entry = 1 cell-group of 3 values
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
            var dv = _isMidterm ? dgvMid?.DataSource as DataView
                                 : dgvFinal?.DataSource as DataView;
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

            double tw = 0; int tp = 0;
            foreach (var e in data.Where(e => e.Remarks == "PASSED"))
            { tw += e.Equivalent * e.Units; tp += e.Units; }
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
            bool is2425 = cmbAcYear?.SelectedIndex == 0; // "2024 - 2025" is index 0

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
            ApplyGradeGridColumnSizing(dgvMid);
            ApplyGradeGridColumnSizing(dgvFinal);
            // Re-detect grading period
            var (activeMid, _) = GetActiveData();
            cmbGradingPeriod.SelectedIndex = DetectGradingPeriod(activeMid) ? 0 : 1;
            if (txtSearch != null) txtSearch.Text = "";
            RefreshAll();
        }

        //  Auto-detect Regular / Irregular 
        // A student is considered "Regular" when:
        //   • All subject codes are unique (no repeated code across entries)
        //   • Total units are in the expected regular range (15–25 units)
        //   • No failed or incomplete grades (no "FAILED" / "INC")
        // If any condition fails, the student is flagged as "Irregular".
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

        private void BtnGenerateCOG_Click(object sender, EventArgs e)
        {
            bool is2nd = cmbSemester?.SelectedIndex == 1;
            bool is2425 = cmbAcYear?.SelectedIndex == 0;

            string ayLabel = is2425 ? "2024 - 2025" : "2025 - 2026";

            // Collect both semesters for the selected AY
            List<GradeEntry> sem1Raw, sem2Raw;
            if (is2425)
            {
                sem1Raw = _ay2425_sem1_final.Count > 0 ? _ay2425_sem1_final : _ay2425_sem1_mid;
                sem2Raw = _ay2425_sem2_final.Count > 0 ? _ay2425_sem2_final : _ay2425_sem2_mid;
            }
            else
            {
                sem1Raw = _final.Count > 0 ? _final : _midterm;
                sem2Raw = _final2.Count > 0 ? _final2 : _midterm2;
            }

            static List<CogGenerator.GradeEntry> Convert(List<GradeEntry> src) =>
                src.Select(e => new CogGenerator.GradeEntry
                {
                    SubjectCode = e.SubjectCode,
                    SubjectTitle = e.SubjectTitle,
                    Units = e.Units,
                    Equivalent = e.Equivalent,
                    Remarks = e.Remarks,
                }).ToList();

            var sem1 = Convert(sem1Raw);
            var sem2 = Convert(sem2Raw);

            using (var sfd = new SaveFileDialog
            {
                Filter = "PDF Documents (*.pdf)|*.pdf",
                FileName = $"COG_{ayLabel.Replace(" ", "").Replace("-", "")}.pdf",
                Title = "Save Certificate of Grades"
            })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    string logoPath = System.IO.Path.Combine(
    Application.StartupPath, "Resources", "pup48x48.png");

                    CogGenerator.Generate(sfd.FileName, logoPath, ayLabel, sem1, sem2);

                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = sfd.FileName,
                        UseShellExecute = true
                    });

                    ShowToast("COG generated successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred:\n" + ex.Message,
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private static string HtmlEncode(string s) =>
            System.Net.WebUtility.HtmlEncode(s ?? "");

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
                var lbl = MakeLabel(_notes[i], 10, FontStyle.Regular, Color.FromArgb(60, 60, 60));
                lbl.Location = new Point(4, 6); lbl.Width = row.Width - 28; lbl.AutoEllipsis = true;
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

        //  TOAST
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

        //  FACTORY HELPERS
        private static Label MakeLabel(string text, float sz, FontStyle fs, Color fore, Point? loc = null)
        {
            var l = new Label { Text = text, Font = new Font("Segoe UI", sz, fs), ForeColor = fore, AutoSize = true };
            if (loc.HasValue) l.Location = loc.Value;
            return l;
        }

        private static ComboBox MakeCombo(string[] items, Point loc, int w)
        {
            var c = new ComboBox
            {
                Location = loc,
                Width = w,
                Height = 26,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9f)
            };
            c.Items.AddRange(items);
            c.SelectedIndex = 0;
            return c;
        }

        private static Button MakeMaroonButton(string text, Point loc, int w, int h)
        {
            var b = new Button
            {
                Text = text,
                Location = loc,
                Size = new Size(w, h),
                BackColor = Color.FromArgb(128, 0, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f),
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        private static Button MakeOutlineButton(string text, Point loc, int w, int h)
        {
            var b = new Button
            {
                Text = text,
                Location = loc,
                Size = new Size(w, h),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(60, 60, 60),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f),
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderColor = Color.FromArgb(180, 180, 180);
            return b;
        }

        private static DataGridView MakeGradeGrid(Size sz)
        {
            var dg = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(220, 220, 220),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None, 
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 9f),
                EnableHeadersVisualStyles = false,
                AutoGenerateColumns = true
            };
            dg.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(106, 0, 0);
            dg.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dg.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dg.ColumnHeadersHeight = 30;
            dg.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 246, 248);
            dg.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 33, 33);
            dg.RowTemplate.Height = 26;
            dg.AllowUserToResizeColumns = false;
            dg.AllowUserToResizeRows = false;

            dg.DataBindingComplete += (s, e) => ApplyGradeGridColumnSizing((DataGridView)s);
            return dg;
        }

        private static void ApplyGradeGridColumnSizing(DataGridView dg)
        {
            if (dg.Columns.Count == 0) return;
            // Auto-size specific columns to content
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