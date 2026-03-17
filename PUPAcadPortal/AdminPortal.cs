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

        public AdminPortal()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
          
            pnlSubOfferingContent.AutoScroll = true;
            btnSO_EditSchedule.Click += btnSO_EditSchedule_Click;
            btnSO_Schedule.Click += btnSO_Schedule_Click;
            btnSO_CurriculumArchive.Click += btnSO_CurriculumArchive_Click;
        }

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

            pnlSubOfferingContent.Controls.Add(scrollPanel);
            pnlSubOfferingContent.Controls.Add(panelTop);
            panelTop.BringToFront();

            pnlSubOfferingContent.Resize += (s, ev) => RepositionSchedule();
            RepositionSchedule();

        }

        private void btnSO_EditSchedule_Click(object sender, EventArgs e)
        {
            changeButtonColor(btnSubjectOffering);
            clickedButton = btnSubjectOffering;
            showContent(clickedButton);

            pnlSubOfferingContent.Controls.Clear();
            pnlSubOfferingContent.Visible = true;
            pnlSubOfferingContent.AutoScroll = true;
            pnlSubOfferingContent.Padding = new Padding(0);

            bool _hasUnsavedChanges = false;
            string _currentYearFilter = "All";

            var inner = new Panel();
            inner.Width = pnlSubOfferingContent.ClientSize.Width;
            inner.Height = 2500;
            inner.Location = new Point(0, 0);

            var lblTitle = new Label();
            lblTitle.Text = "Current Semester";
            lblTitle.Font = new Font("Arial", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.Black;
            lblTitle.Location = new Point(10, 10);
            lblTitle.AutoSize = true;
            inner.Controls.Add(lblTitle);

            var lblYearLevel = new Label();
            lblYearLevel.Text = "Year Level:";
            lblYearLevel.Font = new Font("Arial", 12, FontStyle.Regular);
            lblYearLevel.ForeColor = Color.Black;
            lblYearLevel.Location = new Point(10, 50);
            lblYearLevel.AutoSize = true;
            inner.Controls.Add(lblYearLevel);

            var cmbYearLevel = new ComboBox();
            cmbYearLevel.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbYearLevel.Items.AddRange(new object[] { "All", "1", "2", "3", "4" });
            cmbYearLevel.SelectedIndex = 0;
            cmbYearLevel.Location = new Point(120, 46);
            cmbYearLevel.Size = new Size(80, 24);
            inner.Controls.Add(cmbYearLevel);

            Button btnSaveSchedule = new Button();
            btnSaveSchedule.Text = "Save Schedule";
            btnSaveSchedule.Size = new Size(140, 36);
            btnSaveSchedule.BackColor = Color.FromArgb(0, 130, 60);
            btnSaveSchedule.ForeColor = Color.White;
            btnSaveSchedule.FlatStyle = FlatStyle.Flat;
            btnSaveSchedule.FlatAppearance.BorderSize = 0;
            btnSaveSchedule.Cursor = Cursors.Hand;

            Button btnClearSchedule = new Button();
            btnClearSchedule.Text = "Clear Schedule";
            btnClearSchedule.Size = new Size(140, 36);
            btnClearSchedule.BackColor = Color.FromArgb(160, 160, 160);
            btnClearSchedule.ForeColor = Color.White;
            btnClearSchedule.FlatStyle = FlatStyle.Flat;
            btnClearSchedule.FlatAppearance.BorderSize = 0;
            btnClearSchedule.Cursor = Cursors.Hand;

            Button btnAddSection = new Button();
            btnAddSection.Text = "Add Section";
            btnAddSection.Size = new Size(120, 36);
            btnAddSection.BackColor = Color.FromArgb(109, 0, 0);
            btnAddSection.ForeColor = Color.White;
            btnAddSection.FlatStyle = FlatStyle.Flat;
            btnAddSection.FlatAppearance.BorderSize = 0;
            btnAddSection.Cursor = Cursors.Hand;

            void PositionButtons()
            {
                int w = inner.Width;
                btnSaveSchedule.Location = new Point(w - 150, 8);
                btnClearSchedule.Location = new Point(w - 300, 8);
                btnAddSection.Location = new Point(w - 430, 8);
            }
            PositionButtons();

            inner.Controls.Add(btnSaveSchedule);
            inner.Controls.Add(btnClearSchedule);
            inner.Controls.Add(btnAddSection);

            var dgvEdit = new DataGridView();
            dgvEdit.Location = new Point(10, 85);
            dgvEdit.Width = inner.Width - 20;
            dgvEdit.Height = 1400;
            dgvEdit.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvEdit.AutoGenerateColumns = false;
            dgvEdit.AllowUserToAddRows = false;
            dgvEdit.AllowUserToResizeRows = false;
            dgvEdit.RowHeadersVisible = false;
            dgvEdit.BackgroundColor = Color.White;
            dgvEdit.BorderStyle = BorderStyle.None;
            dgvEdit.ScrollBars = ScrollBars.None;
            dgvEdit.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEdit.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 220, 255);
            dgvEdit.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvEdit.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEdit.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEdit.MultiSelect = false;
            dgvEdit.StandardTab = true;

            dgvEdit.Columns.Add(new DataGridViewTextBoxColumn() { Name = "CourseCode", HeaderText = "Course Code", FillWeight = 80f });
            dgvEdit.Columns.Add(new DataGridViewTextBoxColumn() { Name = "CourseTitle", HeaderText = "Course Title", FillWeight = 150f });
            dgvEdit.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Lec", HeaderText = "Lec", FillWeight = 40f });
            dgvEdit.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Lab", HeaderText = "Lab", FillWeight = 40f });
            dgvEdit.Columns.Add(new DataGridViewTextBoxColumn() { Name = "TotalUnits", HeaderText = "Total Units", FillWeight = 60f });

            var colSection = new DataGridViewComboBoxColumn();
            colSection.Name = "Section"; colSection.HeaderText = "Section"; colSection.FillWeight = 70f;
            colSection.Items.AddRange("BSIT 1-1", "BSIT 1-2", "BSIT 2-1", "BSIT 2-2", "BSIT 3-1", "BSIT 3-2", "BSIT 4-1", "BSIT 4-2");

            cmbYearLevel.SelectedIndexChanged += (s, ev) =>
            {
                string yr = cmbYearLevel.SelectedItem?.ToString() ?? "All";
                colSection.Items.Clear();
                if (yr == "All")
                    colSection.Items.AddRange("BSIT 1-1", "BSIT 1-2", "BSIT 2-1", "BSIT 2-2", "BSIT 3-1", "BSIT 3-2", "BSIT 4-1", "BSIT 4-2");
                else
                    colSection.Items.AddRange($"BSIT {yr}-1", $"BSIT {yr}-2");
            };

            dgvEdit.Columns.Add(colSection);

            var colDay = new DataGridViewComboBoxColumn();
            colDay.Name = "Day"; colDay.HeaderText = "Day"; colDay.FillWeight = 60f;
            colDay.Items.AddRange("Mon", "Tue", "Wed", "Thu", "Fri", "Sat");
            dgvEdit.Columns.Add(colDay);

            dgvEdit.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Start", HeaderText = "Start Time", FillWeight = 70f });
            dgvEdit.Columns.Add(new DataGridViewTextBoxColumn() { Name = "End", HeaderText = "End Time", FillWeight = 70f });

            var colRoom = new DataGridViewComboBoxColumn();
            colRoom.Name = "Room"; colRoom.HeaderText = "Room"; colRoom.FillWeight = 60f;
            colRoom.Items.AddRange("101", "102", "103", "CL1", "CL2", "CL3");
            dgvEdit.Columns.Add(colRoom);

            dgvEdit.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Instructor", HeaderText = "Instructor", FillWeight = 80f });

            var colRemove = new DataGridViewButtonColumn();
            colRemove.Name = "Remove"; colRemove.HeaderText = ""; colRemove.Text = "Remove";
            colRemove.UseColumnTextForButtonValue = true; colRemove.FillWeight = 55f;
            colRemove.DefaultCellStyle.BackColor = Color.FromArgb(180, 0, 0);
            colRemove.DefaultCellStyle.ForeColor = Color.White;
            colRemove.DefaultCellStyle.SelectionBackColor = Color.FromArgb(180, 0, 0);
            colRemove.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvEdit.Columns.Add(colRemove);

            dgvEdit.Rows.Add(35);
            dgvEdit.ClearSelection();
            dgvEdit.DataError += (s, ev) => ev.ThrowException = false;

            inner.Controls.Add(dgvEdit);

            void CheckConflicts()
            {
                foreach (DataGridViewRow r in dgvEdit.Rows)
                {
                    r.DefaultCellStyle.BackColor = Color.White;
                    if (r.Cells["Start"] != null) r.Cells["Start"].ToolTipText = "";
                    if (r.Cells["End"] != null) r.Cells["End"].ToolTipText = "";
                }

                var filledRows = new List<(int RowIndex, string Section, string Day, DateTime Start, DateTime End)>();
                foreach (DataGridViewRow r in dgvEdit.Rows)
                {
                    if (r.Cells["Section"].Value == null || r.Cells["Day"].Value == null ||
                        r.Cells["Start"].Value == null || r.Cells["End"].Value == null) continue;
                    string sec = r.Cells["Section"].Value.ToString();
                    string day = r.Cells["Day"].Value.ToString();
                    DateTime st, en;
                    if (!DateTime.TryParse(r.Cells["Start"].Value.ToString(), out st)) continue;
                    if (!DateTime.TryParse(r.Cells["End"].Value.ToString(), out en)) continue;
                    filledRows.Add((r.Index, sec, day, st, en));
                }

                var conflictRows = new HashSet<int>();
                for (int i = 0; i < filledRows.Count; i++)
                    for (int j = i + 1; j < filledRows.Count; j++)
                    {
                        var a = filledRows[i]; var b = filledRows[j];
                        if (a.Section == b.Section && a.Day == b.Day && a.Start < b.End && b.Start < a.End)
                        {
                            conflictRows.Add(a.RowIndex);
                            conflictRows.Add(b.RowIndex);
                        }
                    }

                foreach (int ri in conflictRows)
                {
                    dgvEdit.Rows[ri].DefaultCellStyle.BackColor = Color.FromArgb(255, 220, 220);
                    dgvEdit.Rows[ri].Cells["Start"].ToolTipText = "⚠ Time conflict detected";
                    dgvEdit.Rows[ri].Cells["End"].ToolTipText = "⚠ Time conflict detected";
                }
            }

            dgvEdit.CellClick += (s, ev) =>
            {
                if (ev.RowIndex < 0 || ev.ColumnIndex < 0) return;
                string colName = dgvEdit.Columns[ev.ColumnIndex].Name;

                if (colName == "Remove")
                {
                    DataGridViewRow row = dgvEdit.Rows[ev.RowIndex];
                    bool hasData = false;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.OwningColumn.Name == "Remove") continue;
                        if (cell.Value != null && !string.IsNullOrWhiteSpace(cell.Value.ToString()))
                        { hasData = true; break; }
                    }
                    if (!hasData) return;
                    var confirm = MessageBox.Show("Are you sure you want to remove this section?",
                        "Confirm Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirm == DialogResult.Yes)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                            if (cell.OwningColumn.Name != "Remove") cell.Value = null;
                        row.DefaultCellStyle.BackColor = Color.White;
                        dgvEdit.ClearSelection();
                        _hasUnsavedChanges = true;
                        CheckConflicts();
                    }
                    return;
                }

                if (dgvEdit.Columns[ev.ColumnIndex] is DataGridViewComboBoxColumn)
                {
                    dgvEdit.BeginEdit(true);
                    var combo = dgvEdit.EditingControl as ComboBox;
                    if (combo != null) combo.DroppedDown = true;
                }
            };

            dgvEdit.CellValueChanged += (s, ev) => { _hasUnsavedChanges = true; CheckConflicts(); };

            dgvEdit.KeyDown += (s, ev) =>
            {
                if (ev.KeyCode == Keys.Enter)
                {
                    ev.Handled = true;
                    int nextRow = (dgvEdit.CurrentCell?.RowIndex ?? 0) + 1;
                    int col = dgvEdit.CurrentCell?.ColumnIndex ?? 0;
                    if (nextRow < dgvEdit.Rows.Count)
                        dgvEdit.CurrentCell = dgvEdit.Rows[nextRow].Cells[col];
                }
            };

            void PopulateEditGrid(string yearFilter)
            {
                foreach (DataGridViewRow row in dgvEdit.Rows)
                    foreach (DataGridViewCell cell in row.Cells)
                        if (cell.OwningColumn.Name != "Remove") cell.Value = null;
                foreach (DataGridViewRow row in dgvEdit.Rows)
                    row.DefaultCellStyle.BackColor = Color.White;

                var filtered = new List<string[]>(_savedSchedule);
                if (yearFilter != "All")
                    filtered = filtered.FindAll(r =>
                    {
                        string sec = r.Length > 5 ? r[5] : "";
                        return sec.Length >= 7 && sec.Substring(5, 1) == yearFilter;
                    });

                int rowIdx = 0;
                foreach (var savedRow in filtered)
                {
                    if (rowIdx >= dgvEdit.Rows.Count) break;
                    for (int ci = 0; ci < savedRow.Length && ci < dgvEdit.Columns.Count - 1; ci++)
                        try { dgvEdit.Rows[rowIdx].Cells[ci].Value = savedRow[ci]; } catch { }
                    rowIdx++;
                }
                rowIdx = 0;
                foreach (var savedRow in filtered)
                {
                    if (rowIdx >= dgvEdit.Rows.Count) break;
                    for (int ci = 0; ci < savedRow.Length && ci < dgvEdit.Columns.Count - 1; ci++)
                        try { dgvEdit.Rows[rowIdx].Cells[ci].Value = savedRow[ci]; } catch { }
                    rowIdx++;
                }
                dgvEdit.ClearSelection();
                CheckConflicts();
                _hasUnsavedChanges = false;
            }

            cmbYearLevel.SelectedIndexChanged += (s, ev) =>
            {
                string newFilter = cmbYearLevel.SelectedItem?.ToString() ?? "All";
                if (newFilter == _currentYearFilter) return;
                if (_hasUnsavedChanges)
                {
                    var result = MessageBox.Show(
                        "You have unsaved changes. Switch year level anyway and discard changes?",
                        "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.No)
                    {
                        int prev = cmbYearLevel.Items.IndexOf(_currentYearFilter);
                        if (prev >= 0) cmbYearLevel.SelectedIndex = prev;
                        return;
                    }
                }

                colSection.Items.Clear();
                if (newFilter == "All")
                    colSection.Items.AddRange("BSIT 1-1", "BSIT 1-2", "BSIT 2-1", "BSIT 2-2", "BSIT 3-1", "BSIT 3-2", "BSIT 4-1", "BSIT 4-2");
                else
                    colSection.Items.AddRange($"BSIT {newFilter}-1", $"BSIT {newFilter}-2");

                _currentYearFilter = newFilter;
                PopulateEditGrid(_currentYearFilter);
            };

            pnlSubOfferingContent.Resize += (s, ev) =>
            {
                inner.Width = pnlSubOfferingContent.ClientSize.Width;
                dgvEdit.Width = inner.Width - 20;
                PositionButtons();
            };

            pnlSubOfferingContent.Controls.Add(inner);
            PopulateEditGrid("All");

            btnAddSection.Click += (s, ev) =>
            {
                dgvEdit.EndEdit();
                DataGridViewRow targetRow = null;
                foreach (DataGridViewRow row in dgvEdit.Rows)
                {
                    bool anyFilled = false;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.OwningColumn.Name == "Remove") continue;
                        if (cell.Value != null && !string.IsNullOrWhiteSpace(cell.Value.ToString()))
                        { anyFilled = true; break; }
                    }
                    if (anyFilled) { targetRow = row; break; }
                }

                if (targetRow == null)
                {
                    MessageBox.Show("Please fill in the section fields before adding.", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                foreach (DataGridViewCell cell in targetRow.Cells)
                {
                    if (cell.OwningColumn.Name == "Remove") continue;
                    if (cell.Value == null || string.IsNullOrWhiteSpace(cell.Value.ToString()))
                    {
                        MessageBox.Show("Please complete all fields before adding a section.", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dgvEdit.CurrentCell = cell;
                        return;
                    }
                }

                CheckConflicts();
                bool hasConflict = false;
                foreach (DataGridViewRow r in dgvEdit.Rows)
                    if (r.DefaultCellStyle.BackColor == Color.FromArgb(255, 220, 220)) { hasConflict = true; break; }
                if (hasConflict)
                {
                    var cont = MessageBox.Show("A time conflict was detected. Do you still want to add this section?",
                        "Conflict Detected", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (cont == DialogResult.No) return;
                }

                string sectionName = targetRow.Cells["Section"].Value?.ToString() ?? "";
                string courseCode = targetRow.Cells["CourseCode"].Value?.ToString() ?? "";
                string courseTitle = targetRow.Cells["CourseTitle"].Value?.ToString() ?? "";
                string lec = targetRow.Cells["Lec"].Value?.ToString() ?? "";
                string lab = targetRow.Cells["Lab"].Value?.ToString() ?? "";
                string instructor = targetRow.Cells["Instructor"].Value?.ToString() ?? "";
                string sectionYear = sectionName.Length >= 7 ? sectionName.Substring(5, 1) : "";

                var allSections = new List<string>
                {
                    "BSIT 1-1", "BSIT 1-2", "BSIT 2-1", "BSIT 2-2",
                    "BSIT 3-1", "BSIT 3-2", "BSIT 4-1", "BSIT 4-2"
                };
                var otherSections = allSections.FindAll(sec =>
                {
                    string yr = sec.Length >= 7 ? sec.Substring(5, 1) : "";
                    return yr == sectionYear && sec != sectionName;
                });

                if (otherSections.Count > 0)
                {
                    string otherList = string.Join(", ", otherSections);
                    var applyToOthers = MessageBox.Show(
                        $"Do you want to apply the same subject info ({courseCode} - {courseTitle}, " + $"Lec: {lec}, Lab: {lab}, Instructor: {instructor}) to the other sections " + $"of the same year level ({otherList})?\n\n" +
                        $"Note: You will still need to set the schedule (Day, Start Time, End Time) for each section separately.",
                        "Apply to Other Sections?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (applyToOthers == DialogResult.Yes)
                    {
                        foreach (string sec in otherSections)
                        {
                            foreach (DataGridViewRow emptyRow in dgvEdit.Rows)
                            {
                                bool isEmpty = true;
                                foreach (DataGridViewCell c in emptyRow.Cells)
                                {
                                    if (c.OwningColumn.Name == "Remove") continue;
                                    if (c.Value != null && !string.IsNullOrWhiteSpace(c.Value.ToString()))
                                    { isEmpty = false; break; }
                                }
                                if (isEmpty)
                                {
                                    try { emptyRow.Cells["CourseCode"].Value = courseCode; } catch { }
                                    try { emptyRow.Cells["CourseTitle"].Value = courseTitle; } catch { }
                                    try { emptyRow.Cells["Lec"].Value = lec; } catch { }
                                    try { emptyRow.Cells["Lab"].Value = lab; } catch { }
                                    try
                                    {
                                        int.TryParse(lec, out int l); int.TryParse(lab, out int lb);
                                        emptyRow.Cells["TotalUnits"].Value = (l + lb).ToString();
                                    }
                                    catch { }
                                    try { emptyRow.Cells["Section"].Value = sec; } catch { }
                                    try { emptyRow.Cells["Instructor"].Value = instructor; } catch { }
                                    break;
                                }
                            }
                        }
                        MessageBox.Show($"{sectionName} has been added, and subject info has been copied to {otherList}." +  "Please set the schedule (Day, Start Time, End Time) for each.",
                            "Section Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"{sectionName} has been added to the table successfully!",
                            "Section Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show($"{sectionName} has been added to the table successfully!",
                        "Section Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                foreach (DataGridViewRow row in dgvEdit.Rows)
                {
                    bool isEmpty = true;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.OwningColumn.Name == "Remove") continue;
                        if (cell.Value != null && !string.IsNullOrWhiteSpace(cell.Value.ToString()))
                        { isEmpty = false; break; }
                    }
                    if (isEmpty) { dgvEdit.CurrentCell = row.Cells[0]; break; }
                }
                _hasUnsavedChanges = true;
            };

            btnClearSchedule.Click += (s, ev) =>
            {
                var confirm = MessageBox.Show("Are you sure you want to clear all schedule data?",
                    "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in dgvEdit.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                            if (cell.OwningColumn.Name != "Remove") cell.Value = null;
                        row.DefaultCellStyle.BackColor = Color.White;
                    }
                    _savedSchedule.Clear();
                    dgvEdit.ClearSelection();
                    _hasUnsavedChanges = false;
                    MessageBox.Show("Schedule cleared.", "Cleared", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            btnSaveSchedule.Click += (s, ev) =>
            {
                dgvEdit.EndEdit();
                CheckConflicts();
                bool hasConflict = false;
                foreach (DataGridViewRow r in dgvEdit.Rows)
                    if (r.DefaultCellStyle.BackColor == Color.FromArgb(255, 220, 220)) { hasConflict = true; break; }
                if (hasConflict)
                {
                    var cont = MessageBox.Show("There are time conflicts in the schedule. Save anyway?",
                        "Conflict Detected", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (cont == DialogResult.No) return;
                }

                foreach (DataGridViewRow row in dgvEdit.Rows)
                {
                    bool anyFilled = false;
                    foreach (DataGridViewCell c in row.Cells)
                    {
                        if (c.OwningColumn.Name == "Remove") continue;
                        if (c.Value != null && !string.IsNullOrWhiteSpace(c.Value.ToString()))
                        { anyFilled = true; break; }
                    }
                    if (!anyFilled) continue;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.OwningColumn.Name == "Remove") continue;
                        if (cell.Value == null || string.IsNullOrWhiteSpace(cell.Value.ToString()))
                        {
                            MessageBox.Show("Please complete all fields before saving.", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            dgvEdit.CurrentCell = cell;
                            return;
                        }
                    }
                }

                _savedSchedule.Clear();
                var sectionOrder = new List<string>
                {
                    "BSIT 1-1", "BSIT 1-2", "BSIT 2-1", "BSIT 2-2",
                    "BSIT 3-1", "BSIT 3-2", "BSIT 4-1", "BSIT 4-2"
                };
                var rows = new List<string[]>();

                foreach (DataGridViewRow row in dgvEdit.Rows)
                {
                    bool anyFilled = false;
                    foreach (DataGridViewCell c in row.Cells)
                    {
                        if (c.OwningColumn.Name == "Remove") continue;
                        if (c.Value != null && !string.IsNullOrWhiteSpace(c.Value.ToString()))
                        { anyFilled = true; break; }
                    }
                    if (!anyFilled) continue;

                    var cols = new string[dgvEdit.Columns.Count - 1];
                    int idx = 0;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.OwningColumn.Name == "Remove") continue;
                        cols[idx++] = cell.Value?.ToString() ?? "";
                    }
                    rows.Add(cols);
                }

                int sectionColIdx = 5;
                rows.Sort((a, b) =>
                {
                    int idxA = sectionOrder.IndexOf(a[sectionColIdx]); if (idxA < 0) idxA = 999;
                    int idxB = sectionOrder.IndexOf(b[sectionColIdx]); if (idxB < 0) idxB = 999;
                    return idxA.CompareTo(idxB);
                });

                _savedSchedule.AddRange(rows);
                _hasUnsavedChanges = false;
                MessageBox.Show("Schedule saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
        }

        //Method na nagpapakita ng content ng bawat button, wala akong maisip na iba kaya eto
        private void showContent(Button button)
        {
            Dictionary<Button, Panel> contents = new Dictionary<Button, Panel> { };
            contents.Add(btnDashboard, pnlDashboardContent);
            contents.Add(btnEnrollments, pnlEnrollContent);
            contents.Add(btnSubjectOffering, pnlSubOfferingContent);
            //Kada button na aadd, maglagay ng panel sa form at lagay dito
            foreach (KeyValuePair<Button, Panel> content in contents) 
            {
                if (content.Key == clickedButton)
                {
                    //Automatic positioning, wag pakialaman maliban nalang kung binago ang position ng sidebar
                    content.Value.Location = new Point(pnlSidebar.Size.Width, pnlHeader.Size.Height);
                    content.Value.Visible = true;
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
                btnSubjectOffering.Text = " Subject Offering                                                                     ⌄";
            else
                btnSubjectOffering.Text = " Subject Offering                                                                      ›";
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

        private void btnSO_CurrentSemester_Click(object sender, EventArgs e)
        {
            changeButtonColor(btnSubjectOffering);
            clickedButton = btnSubjectOffering;
            showContent(clickedButton);

            pnlSubOfferingContent.Controls.Clear();
            pnlSubOfferingContent.AutoScroll = true;    
            pnlSubOfferingContent.Visible = true;

            var inner = new Panel();
            inner.Width = pnlSubOfferingContent.ClientSize.Width;
            inner.Height = 2500;
            inner.Location = new Point(0, 0);

            var lblTitle = new Label();
            lblTitle.Text = "Semester Setup";
            lblTitle.Font = new Font("Arial", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.Black;
            lblTitle.Location = new Point(10, 10);
            lblTitle.AutoSize = true;
            inner.Controls.Add(lblTitle);

            var lblSY = new Label();
            lblSY.Text = "School Year:";
            lblSY.Font = new Font("Arial", 12, FontStyle.Regular);
            lblSY.ForeColor = Color.Black;
            lblSY.Location = new Point(10, 50);
            lblSY.AutoSize = true;
            inner.Controls.Add(lblSY);

            var cmbSY = new ComboBox();
            cmbSY.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSY.Items.AddRange(new object[] { "2025-2026", "2026-2027" });
            cmbSY.SelectedIndex = 0;
            cmbSY.Location = new Point(140, 46);
            cmbSY.Size = new Size(180, 24);
            inner.Controls.Add(cmbSY);

            var lblSem = new Label();
            lblSem.Text = "Semester:";
            lblSem.Font = new Font("Arial", 12, FontStyle.Regular);
            lblSem.ForeColor = Color.Black;
            lblSem.Location = new Point(10, 86);
            lblSem.AutoSize = true;
            inner.Controls.Add(lblSem);

            var cmbSem = new ComboBox();
            cmbSem.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSem.Items.AddRange(new object[] { "1", "2", "3" });
            cmbSem.SelectedIndex = 0;
            cmbSem.Location = new Point(120, 82);
            cmbSem.Size = new Size(80, 24);
            inner.Controls.Add(cmbSem);

            var btnSetCurrent = new Button();
            btnSetCurrent.Text = "Set as Current Semester";
            btnSetCurrent.Size = new Size(180, 36);
            btnSetCurrent.Location = new Point(pnlSubOfferingContent.Width - btnSetCurrent.Width - 30, 60);
            btnSetCurrent.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSetCurrent.BackColor = Color.FromArgb(109, 0, 0);
            btnSetCurrent.ForeColor = Color.White;
            btnSetCurrent.FlatStyle = FlatStyle.Flat;
            btnSetCurrent.FlatAppearance.BorderSize = 0;
            btnSetCurrent.Click += (s, ev) =>
            {
                MessageBox.Show($"Set {cmbSY.SelectedItem} semester {cmbSem.SelectedItem} as current.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            pnlSubOfferingContent.Controls.Add(btnSetCurrent);

            var lblCourseList = new Label();
            lblCourseList.Text = "Current Semester Course List";
            lblCourseList.Font = new Font("Arial", 16, FontStyle.Bold);
            lblCourseList.ForeColor = Color.Black;
            lblCourseList.Location = new Point(10, 140);
            lblCourseList.AutoSize = true;
            pnlSubOfferingContent.Controls.Add(lblCourseList);

            var dgvSched = new DataGridView();
            dgvSched.Location = new Point(10, 85);
            dgvSched.Width = inner.Width - 20;
            dgvSched.Height = 1260;
            dgvSched.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvSched.AutoGenerateColumns = false;
            dgvSched.AllowUserToAddRows = false;
            dgvSched.RowHeadersVisible = false;
            dgvSched.ScrollBars = ScrollBars.None;
            dgvSched.BackgroundColor = Color.White;
            dgvSched.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSched.ReadOnly = true;
            dgvSched.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSched.AllowUserToDeleteRows = false;
            dgvSched.ClearSelection();
            dgvSched.DefaultCellStyle.SelectionBackColor = Color.White;
            dgvSched.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvSched.AllowUserToResizeRows = false;
            dgvSched.AllowUserToResizeColumns = false;

            dgvSched.Columns.Add(new DataGridViewTextBoxColumn() { Name = "CourseCode", HeaderText = "Course Code", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvSched.Columns.Add(new DataGridViewTextBoxColumn() { Name = "CourseTitle", HeaderText = "Course Title", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvSched.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Lec", HeaderText = "Lec", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvSched.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Lab", HeaderText = "Lab", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvSched.Columns.Add(new DataGridViewTextBoxColumn() { Name = "TotalUnits", HeaderText = "Total Units", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvSched.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Year", HeaderText = "Year", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });

            dgvSched.RowCount = 40;
            inner.Controls.Add(dgvSched);

            pnlSubOfferingContent.Resize += (s, ev) =>
            {
                inner.Width = pnlSubOfferingContent.ClientSize.Width;
                dgvSched.Width = inner.Width - 20;
            };

            pnlSubOfferingContent.Controls.Add(inner);
            int csLeft = 10, csRight = 10, csTop = 180, csMinHeight = 240;
            void RepositionCurrentSemesterContainer()
            {
                try
                {
                    int width = Math.Max(600, pnlSubOfferingContent.ClientSize.Width - csLeft - csRight);
                    int bottomPadding = pnlSubOfferingContent.Padding.Bottom;
                    int height = Math.Max(csMinHeight, pnlSubOfferingContent.ClientSize.Height - csTop - bottomPadding - 8);
                    dgvSched.SetBounds(csLeft, csTop, width, height);
                }
                catch { }
            }

            pnlSubOfferingContent.SizeChanged += (s, ev) => RepositionCurrentSemesterContainer();
            this.Resize += (s, ev) => RepositionCurrentSemesterContainer();
            RepositionCurrentSemesterContainer();
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pnlCoursesContent_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
