using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class AdminPortal : Form
    {

        private Button clickedButton;
        private Color defaultColor = Color.Maroon;
        private Color selectedColor = Color.FromArgb(109, 0, 0);
        private List<string[]> _savedSchedule = new List<string[]>();

        private void ShowPanel(Panel panelToShow)
        {
            pnlSubOfferingContent.Visible = false;
            pnlCurrentSemester.Visible = false;
            pnlEditSchedule.Visible = false;

            panelToShow.Visible = true;
            panelToShow.BringToFront();
        }
        public AdminPortal()
        {
            InitializeComponent();

            pnlEditSchedule.Visible = false;
            pnlCurrentSemester.Visible = false;
            pnlSubOfferingContent.Visible = false;

            this.WindowState = FormWindowState.Maximized;

            pnlSubOfferingContent.AutoScroll = true;
            btnSO_EditSchedule.Click += btnSO_EditSchedule_Click;
            btnSO_Schedule.Click += btnSO_Schedule_Click;
            btnSO_CurriculumArchive.Click += btnSO_CurriculumArchive_Click;


            AddDuplicateButtonColumn();

            // Add 30 default empty rows to the edit schedule grid and Current Semester grid
            try { dgvSchedule.Rows.Add(30); } catch { }
            try { dgvEditSchedule.Rows.Add(30); } catch { }

            //// Fill first 3 rows with default data

            //// Row 0
            //dgvEditSchedule.Rows[0].Cells[0].Value = "COMP 009";
            //dgvEditSchedule.Rows[0].Cells[1].Value = "Object Oriented Programming";
            //dgvEditSchedule.Rows[0].Cells[2].Value = "3.0";
            //dgvEditSchedule.Rows[0].Cells[3].Value = "2.0";
            //dgvEditSchedule.Rows[0].Cells[4].Value = "5.0";
            //// Row 1
            //dgvEditSchedule.Rows[1].Cells[0].Value = "COMP 010";
            //dgvEditSchedule.Rows[1].Cells[1].Value = "Information Management";
            //dgvEditSchedule.Rows[1].Cells[2].Value = "3.0";
            //dgvEditSchedule.Rows[1].Cells[3].Value = "2.0";
            //dgvEditSchedule.Rows[1].Cells[4].Value = "5.0";
            //// Row 2
            //dgvEditSchedule.Rows[2].Cells[0].Value = "COMP 012";
            //dgvEditSchedule.Rows[2].Cells[1].Value = "Network Administration";
            //dgvEditSchedule.Rows[2].Cells[2].Value = "3.0";
            //dgvEditSchedule.Rows[2].Cells[3].Value = "2.0";
            //dgvEditSchedule.Rows[2].Cells[4].Value = "5.0";

            // Fill first 3 rows with default data for BOTH grids
            string[,] dummyData =
            {
                { "COMP 009", "Object Oriented Programming", "3.0", "2.0", "5.0" },
                { "COMP 010", "Information Management", "3.0", "2.0", "5.0" },
                { "COMP 012", "Network Administration", "3.0", "2.0", "5.0" }
            };

               for (int row = 0; row < 3; row++)
                    {
                        for (int col = 0; col < 5; col++)
                            {
                                dgvEditSchedule.Rows[row].Cells[col].Value = dummyData[row, col];
                                dgvSchedule.Rows[row].Cells[col].Value = dummyData[row, col];
                            }
                    }

        }

        private void AddDuplicateButtonColumn()
        {
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            btn.Name = "colDuplicate";
            btn.HeaderText = "";
            btn.Text = "Duplicate";
            btn.UseColumnTextForButtonValue = true;

            dgvEditSchedule.Columns.Insert(10, btn); // 11th column
        }

        private void dgvEditSchedule_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvEditSchedule.Columns[e.ColumnIndex].Name == "colDuplicate")
            {
                DuplicateRow(e.RowIndex);
            }
        }

        private void DuplicateRow(int rowIndex)
        {
            DataGridViewRow original = dgvEditSchedule.Rows[rowIndex];

            // Insert a new row right below the row duplicate is clicked
            dgvEditSchedule.Rows.Insert(rowIndex + 1, 1);
            int newRowIndex = rowIndex + 1; // The newly inserted row

            // Copy columns 0–4 (course info)
            for (int i = 0; i <= 4; i++)
            {
                dgvEditSchedule.Rows[newRowIndex].Cells[i].Value = original.Cells[i].Value;
            }

            // Leave other columns blank let admin edit them as needed
        }
        //--------------------------
        private void btnSO_CurriculumArchive_Click(object sender, EventArgs e)
        {
            changeButtonColor(btnSubjectOffering);
            clickedButton = btnSubjectOffering;
            showContent(clickedButton);

            pnlSubOfferingContent.Controls.Clear();
            pnlSubOfferingContent.AutoScroll = false;
            pnlSubOfferingContent.Visible = true;
            pnlSubOfferingContent.Padding = new Padding(0);

            var header = new Panel();
            header.Dock = DockStyle.Top;
            header.Height = 48;
            header.BackColor = Color.White;

            var btnCurriculum = new Button();
            btnCurriculum.Text = "Curriculum";
            btnCurriculum.FlatStyle = FlatStyle.Flat;
            btnCurriculum.FlatAppearance.BorderSize = 0;
            btnCurriculum.Size = new Size(120, 40);
            btnCurriculum.Location = new Point(10, 4);
            btnCurriculum.BackColor = Color.FromArgb(230, 230, 230);

            var btnArchive = new Button();
            btnArchive.Text = "Archive";
            btnArchive.FlatStyle = FlatStyle.Flat;
            btnArchive.FlatAppearance.BorderSize = 0;
            btnArchive.Size = new Size(120, 40);
            btnArchive.Location = new Point(136, 4);
            btnArchive.BackColor = Color.Transparent;

            var btnUpdateCurriculum = new Button();
            btnUpdateCurriculum.Text = "Update Curriculum";
            btnUpdateCurriculum.Size = new Size(160, 36);
            btnUpdateCurriculum.BackColor = Color.Maroon;
            btnUpdateCurriculum.ForeColor = Color.White;
            btnUpdateCurriculum.FlatStyle = FlatStyle.Flat;
            btnUpdateCurriculum.FlatAppearance.BorderSize = 0;
            btnUpdateCurriculum.Cursor = Cursors.Hand;
            btnUpdateCurriculum.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnUpdateCurriculum.Location = new Point(pnlSubOfferingContent.ClientSize.Width - 170, 6);
            btnUpdateCurriculum.Click += (s, ev) =>
            {
                MessageBox.Show("Update Curriculum clicked.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            header.Controls.Add(btnCurriculum);
            header.Controls.Add(btnArchive);
            header.Controls.Add(btnUpdateCurriculum);
            header.SizeChanged += (s, ev) =>
            {
                btnUpdateCurriculum.Left = header.ClientSize.Width - btnUpdateCurriculum.Width - 10;
            };

            var pnlCurriculum = new Panel();
            pnlCurriculum.Dock = DockStyle.Fill;
            pnlCurriculum.BackColor = Color.White;
            pnlCurriculum.AutoScroll = true;

            var dgvCurriculum = new DataGridView();
            dgvCurriculum.Location = new Point(0, 0);
            dgvCurriculum.Dock = DockStyle.Top;
            dgvCurriculum.Height = 2000;
            dgvCurriculum.AutoGenerateColumns = false;
            dgvCurriculum.AllowUserToAddRows = false;
            dgvCurriculum.RowHeadersVisible = false;
            dgvCurriculum.ScrollBars = ScrollBars.None;
            dgvCurriculum.BackgroundColor = Color.White;
            dgvCurriculum.BorderStyle = BorderStyle.None;
            dgvCurriculum.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCurriculum.ReadOnly = true;
            dgvCurriculum.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCurriculum.AllowUserToDeleteRows = false;
            dgvCurriculum.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCurriculum.AllowUserToResizeRows = false;
            dgvCurriculum.AllowUserToResizeColumns = false;


            dgvCurriculum.DefaultCellStyle.SelectionBackColor = Color.White;
            dgvCurriculum.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgvCurriculum.Columns.Add(new DataGridViewTextBoxColumn() { Name = "CourseCode", HeaderText = "Course Code", FillWeight = 1f });
            dgvCurriculum.Columns.Add(new DataGridViewTextBoxColumn() { Name = "CourseTitle", HeaderText = "Course Title", FillWeight = 1f });
            dgvCurriculum.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Lec", HeaderText = "Lec", FillWeight = 1f });
            dgvCurriculum.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Lab", HeaderText = "Lab", FillWeight = 1f });
            dgvCurriculum.Columns.Add(new DataGridViewTextBoxColumn() { Name = "TotalUnits", HeaderText = "Total Units", FillWeight = 1f });
            dgvCurriculum.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Year", HeaderText = "Year", FillWeight = 1f });
            dgvCurriculum.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Prerequisite", HeaderText = "Prerequisite", FillWeight = 1f });

            dgvCurriculum.RowCount = 40;

            dgvCurriculum.ClearSelection();
            dgvCurriculum.SelectionChanged += (s, ev) => { };

            pnlCurriculum.Controls.Add(dgvCurriculum);

            var pnlArchive = new Panel();
            pnlArchive.Dock = DockStyle.Fill;
            pnlArchive.BackColor = Color.White;
            pnlArchive.AutoScroll = true;
            pnlArchive.Visible = false;

            var dgvArchive = new DataGridView();
            dgvArchive.Dock = DockStyle.Top;
            dgvArchive.Height = 20 * 25;
            dgvArchive.AutoGenerateColumns = false;
            dgvArchive.AllowUserToAddRows = false;
            dgvArchive.RowHeadersVisible = false;
            dgvArchive.ScrollBars = ScrollBars.None;
            dgvArchive.BackgroundColor = Color.White;
            dgvArchive.BorderStyle = BorderStyle.None;
            dgvArchive.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvArchive.ReadOnly = true;
            dgvArchive.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvArchive.AllowUserToDeleteRows = false;
            dgvArchive.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvArchive.DefaultCellStyle.SelectionBackColor = Color.White;
            dgvArchive.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgvArchive.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Semester", HeaderText = "Semester", FillWeight = 1f });
            dgvArchive.Columns.Add(new DataGridViewTextBoxColumn() { Name = "SchoolYear", HeaderText = "School Year", FillWeight = 1f });
            dgvArchive.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Action", HeaderText = "", FillWeight = 1f });

            dgvArchive.RowCount = 20;
            dgvArchive.ClearSelection();
            pnlArchive.Controls.Add(dgvArchive);

            btnCurriculum.Click += (s, ev) =>
            {
                pnlCurriculum.Visible = true;
                pnlArchive.Visible = false;
                btnUpdateCurriculum.Visible = true;
                btnCurriculum.BackColor = Color.FromArgb(230, 230, 230);
                btnArchive.BackColor = Color.Transparent;
            };
            btnArchive.Click += (s, ev) =>
            {
                pnlArchive.Visible = true;
                pnlCurriculum.Visible = false;
                btnUpdateCurriculum.Visible = false;
                btnArchive.BackColor = Color.FromArgb(230, 230, 230);
                btnCurriculum.BackColor = Color.Transparent;
            };

            pnlSubOfferingContent.Controls.Add(pnlCurriculum);
            pnlSubOfferingContent.Controls.Add(pnlArchive);
            pnlSubOfferingContent.Controls.Add(header);

            pnlCurriculum.Visible = true;
            pnlArchive.Visible = false;

        }

        private void changeButtonColor(Button button)
        {
            if (clickedButton != null)
            {
                clickedButton.BackColor = defaultColor;
            }

            clickedButton = button;
            pnlYellow.Visible = true;
            pnlYellow.Parent = clickedButton.Parent;
            pnlYellow.BringToFront();
            clickedButton.BackColor = selectedColor;
        }

        // Schedule panel
        private void btnSO_Schedule_Click(object sender, EventArgs e)
        {
            changeButtonColor(btnSubjectOffering);
            clickedButton = btnSubjectOffering;
            showContent(clickedButton);

            pnlSubOfferingContent.Controls.Clear();
            pnlSubOfferingContent.AutoScroll = false;
            pnlSubOfferingContent.Visible = true;

            var panelTop = new Panel();
            panelTop.Location = new Point(0, 0);
            panelTop.Width = pnlSubOfferingContent.ClientSize.Width;
            panelTop.Height = 80;
            panelTop.BackColor = Color.White;

            var lblCurrent = new Label();
            lblCurrent.Text = "Current Semester:";
            lblCurrent.Font = new Font("Arial", 18, FontStyle.Bold);
            lblCurrent.ForeColor = Color.Black;
            lblCurrent.Location = new Point(10, 10);
            lblCurrent.AutoSize = true;
            panelTop.Controls.Add(lblCurrent);

            var lblYear = new Label();
            lblYear.Text = "Year Level:";
            lblYear.Font = new Font("Arial", 12, FontStyle.Regular);
            lblYear.ForeColor = Color.Black;
            lblYear.Location = new Point(10, 50);
            lblYear.AutoSize = true;
            panelTop.Controls.Add(lblYear);

            var cmbYearSchedule = new ComboBox();
            cmbYearSchedule.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbYearSchedule.Items.AddRange(new object[] { "All", "1", "2", "3", "4" });
            cmbYearSchedule.SelectedIndex = 0;
            cmbYearSchedule.Location = new Point(120, 46);
            cmbYearSchedule.Size = new Size(80, 24);
            panelTop.Controls.Add(cmbYearSchedule);

            Button btnPrint = new Button();
            btnPrint.Text = "Print";
            btnPrint.Size = new Size(100, 36);
            btnPrint.BackColor = Color.FromArgb(109, 0, 0);
            btnPrint.ForeColor = Color.White;
            btnPrint.FlatStyle = FlatStyle.Flat;
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.Cursor = Cursors.Hand;
            btnPrint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPrint.Location = new Point(panelTop.Width - 110, 22);
            panelTop.Controls.Add(btnPrint);

            Button btnExportPDF = new Button();
            btnExportPDF.Text = "Export to PDF";
            btnExportPDF.Size = new Size(120, 36);
            btnExportPDF.BackColor = Color.FromArgb(109, 0, 0);
            btnExportPDF.ForeColor = Color.White;
            btnExportPDF.FlatStyle = FlatStyle.Flat;
            btnExportPDF.FlatAppearance.BorderSize = 0;
            btnExportPDF.Cursor = Cursors.Hand;
            btnExportPDF.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExportPDF.Location = new Point(panelTop.Width - 240, 22);
            panelTop.Controls.Add(btnExportPDF);

            Button btnExportExcel = new Button();
            btnExportExcel.Text = "Export to Excel";
            btnExportExcel.Size = new Size(130, 36);
            btnExportExcel.BackColor = Color.FromArgb(109, 0, 0);
            btnExportExcel.ForeColor = Color.White;
            btnExportExcel.FlatStyle = FlatStyle.Flat;
            btnExportExcel.FlatAppearance.BorderSize = 0;
            btnExportExcel.Cursor = Cursors.Hand;
            btnExportExcel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExportExcel.Location = new Point(panelTop.Width - 380, 22);
            panelTop.Controls.Add(btnExportExcel);

            panelTop.SizeChanged += (s, ev) =>
            {
                btnPrint.Left = panelTop.Width - 110;
                btnExportPDF.Left = panelTop.Width - 240;
                btnExportExcel.Left = panelTop.Width - 380;
            };

            btnPrint.Click += (s, ev) =>
            {
                MessageBox.Show("Print clicked.", "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnExportPDF.Click += (s, ev) =>
            {
                MessageBox.Show("Export to PDF clicked.", "Export PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnExportExcel.Click += (s, ev) =>
            {
                MessageBox.Show("Export to Excel clicked.", "Export Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            var scrollPanel = new Panel();
            scrollPanel.AutoScroll = true;
            scrollPanel.BackColor = Color.White;

            var dgvSched = new DataGridView();
            dgvSched.Location = new Point(10, 0);
            dgvSched.AutoGenerateColumns = false;
            dgvSched.AllowUserToAddRows = false;
            dgvSched.RowHeadersVisible = false;
            dgvSched.ScrollBars = ScrollBars.None;
            dgvSched.BackgroundColor = Color.White;
            dgvSched.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSched.AllowUserToResizeRows = false;
            dgvSched.AllowUserToResizeColumns = false;
            dgvSched.ReadOnly = true;
            dgvSched.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSched.DefaultCellStyle.SelectionBackColor = Color.White;
            dgvSched.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgvSched.Columns.Add(new DataGridViewTextBoxColumn() { Name = "CourseCode", HeaderText = "Course Code", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvSched.Columns.Add(new DataGridViewTextBoxColumn() { Name = "CourseTitle", HeaderText = "Course Title", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvSched.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Lec", HeaderText = "Lec", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvSched.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Lab", HeaderText = "Lab", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvSched.Columns.Add(new DataGridViewTextBoxColumn() { Name = "TotalUnits", HeaderText = "Total Units", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvSched.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Section", HeaderText = "Section", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvSched.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Day", HeaderText = "Day", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvSched.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Start", HeaderText = "Start", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvSched.Columns.Add(new DataGridViewTextBoxColumn() { Name = "End", HeaderText = "End", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvSched.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Room", HeaderText = "Room", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvSched.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Instructor", HeaderText = "Instructor", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });

            scrollPanel.Controls.Add(dgvSched);

            void PopulateScheduleGrid(string yearFilter)
            {
                dgvSched.Rows.Clear();

                var sectionOrder = new List<string>
                {
                    "BSIT 1-1", "BSIT 1-2",
                    "BSIT 2-1", "BSIT 2-2",
                    "BSIT 3-1", "BSIT 3-2",
                    "BSIT 4-1", "BSIT 4-2"
                };

                var filtered = new List<string[]>(_savedSchedule);
                if (yearFilter != "All")
                {
                    filtered = filtered.FindAll(r =>
                    {
                        string sec = r.Length > 5 ? r[5] : "";
                        return sec.Length >= 7 && sec.Substring(5, 1) == yearFilter;
                    });
                }

                filtered.Sort((a, b) =>
                {
                    int ia = sectionOrder.IndexOf(a[5]); if (ia < 0) ia = 999;
                    int ib = sectionOrder.IndexOf(b[5]); if (ib < 0) ib = 999;
                    return ia.CompareTo(ib);
                });

                foreach (var row in filtered)
                    dgvSched.Rows.Add(row);

                dgvSched.ClearSelection();

                int rowHeight = dgvSched.RowTemplate.Height;
                int headerHeight = dgvSched.ColumnHeadersHeight;
                dgvSched.Height = 700;
            }

            PopulateScheduleGrid("All");

            cmbYearSchedule.SelectedIndexChanged += (s, ev) =>
            {
                string yr = cmbYearSchedule.SelectedItem?.ToString() ?? "All";
                PopulateScheduleGrid(yr);
            };

            void RepositionSchedule()
            {
                int w = pnlSubOfferingContent.ClientSize.Width;
                int h = pnlSubOfferingContent.ClientSize.Height;

                panelTop.SetBounds(0, 0, w, 80);
                scrollPanel.SetBounds(0, 80, w, h - 80);
                dgvSched.Width = w - 20;
            }
            // ---------------------------------------------------------------- end of schedule panel controls

            pnlSubOfferingContent.Controls.Add(scrollPanel);
            pnlSubOfferingContent.Controls.Add(panelTop);
            panelTop.BringToFront();

            pnlSubOfferingContent.Resize += (s, ev) => RepositionSchedule();
            RepositionSchedule();

        }

        //-----------------------------------------------------(Edit Schedule (allen)
        private void btnSO_EditSchedule_Click(object sender, EventArgs e)
        {
            ShowPanel(pnlEditSchedule);
            MessageBox.Show("Edit Schedule clicked.");
        }


        //-------------------------------------


        //Method na nagpapakita ng content ng bawat button, wala akong maisip na iba kaya eto
        private void showContent(Button button)
        {
            Dictionary<Button, Panel> contents = new Dictionary<Button, Panel> { };
            contents.Add(btnDashboard, pnlDashboardContent);
            contents.Add(btnEnrollments, pnlEnrollContent);
            contents.Add(btnSubjectOffering, pnlSubOfferingContent);
            contents.Add(btnSO_CurrentSemester, pnlCurrentSemester);
            contents.Add(btnSO_EditSchedule, pnlEditSchedule);
            //Kada button na aadd, maglagay ng panel sa form at lagay dito
            foreach (KeyValuePair<Button, Panel> content in contents)
            {
                if (content.Key == button)
                {
                    //Automatic positioning, wag pakialaman maliban nalang kung binago ang position ng sidebar
                    content.Value.Location = new Point(pnlSidebar.Size.Width, pnlHeader.Size.Height);
                    content.Value.Visible = true;
                    content.Value.BringToFront();
                }
                else
                {
                    content.Value.Visible = false;
                }
            }
        }

        //Method para pag pinindot yung X sa taas o mag alt-F4, icclose lahat ng forms para di magerror pag ni run uli
        //Lagay to sa bawat form na iaadd, Step 1: Hanapin sa properties ng form yung event na FormClosing, Step 2: Double click para gumawa ng method, Step 3: Copy paste code na nasa loob nito
        private void StudentPortal_Closing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Are you sure you want to exit?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                    Application.Exit();
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnEnrollments_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnSubjectOffering_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
            pnlsubofferingSubmenu.Visible = !pnlsubofferingSubmenu.Visible;
            if (pnlsubofferingSubmenu.Visible)
                btnSubjectOffering.Text = " Subject Offering                    ⌄";
            else
                btnSubjectOffering.Text = " Subject Offering                     ›";
        }

        private void btnLMS_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
            pnllmsSubmenu.Visible = !pnllmsSubmenu.Visible;
            if (pnllmsSubmenu.Visible)
                btnLMS.Text = " LMS                                       ⌄";
            else
                btnLMS.Text = " LMS                                        ›";
        }

        //-------------------------------------------------------------- (1 current sem page)
        private void btnSO_CurrentSemester_Click(object sender, EventArgs e)
        {
            changeButtonColor(btnSubjectOffering);
            clickedButton = btnSubjectOffering;
            showContent(clickedButton);

            // Show Current Semester panel
            pnlSubOfferingContent.Visible = true;
            pnlCurrentSemester.Visible = true;

            // Hide other content panels
            //pnlEditSchedule.Visible = false;
            //pnlOtherPanel.Visible = false;

        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //-------------------------------------------------------------- (events current sem page)
        private void pnlCoursesContent_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSetCurrent_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Set {cmbSY.SelectedItem} semester {cmbSem.SelectedItem} as current.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //-------------------------------------------------------------- (events Edit schedule page)
        private void btnAddSection_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnSO_CurriculumArchive_Click_1(object sender, EventArgs e)
        {

        }

        private void btnSaveSchedule_Click(object sender, EventArgs e)
        {

        }

        private void lblESCurrentSem_Click(object sender, EventArgs e)
        {

        }

        private void lblESYearLevel_Click(object sender, EventArgs e)
        {

        }

        private void btnClearSchedule_Click(object sender, EventArgs e)
        {

        }

        private void lblCourseList_Click(object sender, EventArgs e)
        {

        }

        private void pnlEditSchedule_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblSem_Click(object sender, EventArgs e)
        {

        }

        private void lblSemesterSetup_Click(object sender, EventArgs e)
        {

        }

       

    }
}
