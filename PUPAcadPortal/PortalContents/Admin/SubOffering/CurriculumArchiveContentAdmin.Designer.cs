namespace PUPAcadPortal.PortalContents.Admin.SubOffering
{
    partial class CurriculumArchiveContentAdmin
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            pnlCurriculumArchive = new Panel();
            pnlCurriculum = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            label1 = new Label();
            dtpRevisionYear = new DateTimePicker();
            lblCurriculumList = new Label();
            btnUpdateCurriculum = new Button();
            dgvCurriculum = new DataGridView();
            pnlArchive = new Panel();
            dgvArchive = new DataGridView();
            Semester = new DataGridViewTextBoxColumn();
            SchoolYear = new DataGridViewTextBoxColumn();
            blank = new DataGridViewTextBoxColumn();
            panel14 = new Panel();
            btnArchive = new Button();
            btnCurriculum = new Button();
            CourseCode2 = new DataGridViewComboBoxColumn();
            CourseTitle2 = new DataGridViewTextBoxColumn();
            Lab2 = new DataGridViewTextBoxColumn();
            Lec2 = new DataGridViewTextBoxColumn();
            TotalUnits2 = new DataGridViewTextBoxColumn();
            colProgram = new DataGridViewComboBoxColumn();
            Year2 = new DataGridViewComboBoxColumn();
            Semester1 = new DataGridViewComboBoxColumn();
            colRevisionYear = new DataGridViewTextBoxColumn();
            pnlCurriculumArchive.SuspendLayout();
            pnlCurriculum.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCurriculum).BeginInit();
            pnlArchive.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvArchive).BeginInit();
            panel14.SuspendLayout();
            SuspendLayout();
            // 
            // pnlCurriculumArchive
            // 
            pnlCurriculumArchive.BackColor = Color.White;
            pnlCurriculumArchive.Controls.Add(pnlCurriculum);
            pnlCurriculumArchive.Controls.Add(pnlArchive);
            pnlCurriculumArchive.Controls.Add(panel14);
            pnlCurriculumArchive.Dock = DockStyle.Fill;
            pnlCurriculumArchive.Location = new Point(0, 0);
            pnlCurriculumArchive.Margin = new Padding(3, 2, 3, 2);
            pnlCurriculumArchive.Name = "pnlCurriculumArchive";
            pnlCurriculumArchive.Size = new Size(1488, 993);
            pnlCurriculumArchive.TabIndex = 8;
            // 
            // pnlCurriculum
            // 
            pnlCurriculum.BackColor = Color.White;
            pnlCurriculum.Controls.Add(tableLayoutPanel1);
            pnlCurriculum.Controls.Add(lblCurriculumList);
            pnlCurriculum.Controls.Add(btnUpdateCurriculum);
            pnlCurriculum.Controls.Add(dgvCurriculum);
            pnlCurriculum.Dock = DockStyle.Fill;
            pnlCurriculum.Location = new Point(0, 36);
            pnlCurriculum.Margin = new Padding(3, 2, 3, 2);
            pnlCurriculum.Name = "pnlCurriculum";
            pnlCurriculum.Size = new Size(1488, 957);
            pnlCurriculum.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(dtpRevisionYear, 1, 0);
            tableLayoutPanel1.Location = new Point(1224, 20);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(220, 42);
            tableLayoutPanel1.TabIndex = 14;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Right;
            label1.AutoSize = true;
            label1.BackColor = Color.White;
            label1.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ActiveCaptionText;
            label1.Location = new Point(0, 11);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(119, 19);
            label1.TabIndex = 12;
            label1.Text = "Revision Year:";
            // 
            // dtpRevisionYear
            // 
            dtpRevisionYear.Anchor = AnchorStyles.Left;
            dtpRevisionYear.CalendarFont = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpRevisionYear.CustomFormat = "yyyy";
            dtpRevisionYear.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpRevisionYear.Format = DateTimePickerFormat.Custom;
            dtpRevisionYear.Location = new Point(122, 6);
            dtpRevisionYear.MinDate = new DateTime(2005, 1, 1, 0, 0, 0, 0);
            dtpRevisionYear.Name = "dtpRevisionYear";
            dtpRevisionYear.ShowUpDown = true;
            dtpRevisionYear.Size = new Size(95, 29);
            dtpRevisionYear.TabIndex = 13;
            // 
            // lblCurriculumList
            // 
            lblCurriculumList.AutoSize = true;
            lblCurriculumList.BackColor = Color.White;
            lblCurriculumList.Font = new Font("Arial", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCurriculumList.ForeColor = SystemColors.ActiveCaptionText;
            lblCurriculumList.Location = new Point(32, 24);
            lblCurriculumList.Name = "lblCurriculumList";
            lblCurriculumList.Size = new Size(198, 29);
            lblCurriculumList.TabIndex = 11;
            lblCurriculumList.Text = "Curriculum List:";
            // 
            // btnUpdateCurriculum
            // 
            btnUpdateCurriculum.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnUpdateCurriculum.BackColor = Color.FromArgb(109, 0, 0);
            btnUpdateCurriculum.FlatStyle = FlatStyle.Flat;
            btnUpdateCurriculum.ForeColor = Color.White;
            btnUpdateCurriculum.Location = new Point(1307, 584);
            btnUpdateCurriculum.Margin = new Padding(3, 2, 3, 2);
            btnUpdateCurriculum.Name = "btnUpdateCurriculum";
            btnUpdateCurriculum.Size = new Size(140, 27);
            btnUpdateCurriculum.TabIndex = 11;
            btnUpdateCurriculum.Text = "Update Curriculum";
            btnUpdateCurriculum.UseVisualStyleBackColor = false;
            btnUpdateCurriculum.Click += btnUpdateCurriculum_Click;
            // 
            // dgvCurriculum
            // 
            dgvCurriculum.AllowUserToAddRows = false;
            dgvCurriculum.AllowUserToDeleteRows = false;
            dgvCurriculum.AllowUserToResizeColumns = false;
            dgvCurriculum.AllowUserToResizeRows = false;
            dgvCurriculum.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvCurriculum.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCurriculum.BackgroundColor = Color.White;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = Color.White;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvCurriculum.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvCurriculum.ColumnHeadersHeight = 29;
            dgvCurriculum.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCurriculum.Columns.AddRange(new DataGridViewColumn[] { CourseCode2, CourseTitle2, Lab2, Lec2, TotalUnits2, colProgram, Year2, Semester1, colRevisionYear });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(255, 255, 128);
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvCurriculum.DefaultCellStyle = dataGridViewCellStyle3;
            dgvCurriculum.EnableHeadersVisualStyles = false;
            dgvCurriculum.Location = new Point(44, 68);
            dgvCurriculum.Margin = new Padding(3, 2, 3, 2);
            dgvCurriculum.Name = "dgvCurriculum";
            dgvCurriculum.RowHeadersVisible = false;
            dgvCurriculum.RowHeadersWidth = 51;
            dgvCurriculum.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvCurriculum.ScrollBars = ScrollBars.Vertical;
            dgvCurriculum.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCurriculum.Size = new Size(1400, 495);
            dgvCurriculum.TabIndex = 0;
            // 
            // pnlArchive
            // 
            pnlArchive.BackColor = Color.White;
            pnlArchive.Controls.Add(dgvArchive);
            pnlArchive.Dock = DockStyle.Fill;
            pnlArchive.Location = new Point(0, 36);
            pnlArchive.Margin = new Padding(3, 2, 3, 2);
            pnlArchive.Name = "pnlArchive";
            pnlArchive.Size = new Size(1488, 957);
            pnlArchive.TabIndex = 12;
            // 
            // dgvArchive
            // 
            dgvArchive.AllowUserToAddRows = false;
            dgvArchive.AllowUserToDeleteRows = false;
            dgvArchive.AllowUserToOrderColumns = true;
            dgvArchive.AllowUserToResizeColumns = false;
            dgvArchive.AllowUserToResizeRows = false;
            dgvArchive.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvArchive.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvArchive.BackgroundColor = Color.White;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = Color.White;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvArchive.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvArchive.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvArchive.Columns.AddRange(new DataGridViewColumn[] { Semester, SchoolYear, blank });
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.Window;
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle5.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(255, 255, 128);
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dgvArchive.DefaultCellStyle = dataGridViewCellStyle5;
            dgvArchive.EnableHeadersVisualStyles = false;
            dgvArchive.Location = new Point(36, 68);
            dgvArchive.Margin = new Padding(3, 2, 3, 2);
            dgvArchive.Name = "dgvArchive";
            dgvArchive.ReadOnly = true;
            dgvArchive.RowHeadersVisible = false;
            dgvArchive.RowHeadersWidth = 51;
            dgvArchive.ScrollBars = ScrollBars.Vertical;
            dgvArchive.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvArchive.Size = new Size(1424, 495);
            dgvArchive.TabIndex = 0;
            // 
            // Semester
            // 
            Semester.HeaderText = "Semester";
            Semester.MinimumWidth = 6;
            Semester.Name = "Semester";
            Semester.ReadOnly = true;
            // 
            // SchoolYear
            // 
            SchoolYear.HeaderText = "School Year";
            SchoolYear.MinimumWidth = 6;
            SchoolYear.Name = "SchoolYear";
            SchoolYear.ReadOnly = true;
            // 
            // blank
            // 
            blank.HeaderText = "";
            blank.MinimumWidth = 6;
            blank.Name = "blank";
            blank.ReadOnly = true;
            // 
            // panel14
            // 
            panel14.BackColor = Color.White;
            panel14.Controls.Add(btnArchive);
            panel14.Controls.Add(btnCurriculum);
            panel14.Dock = DockStyle.Top;
            panel14.Location = new Point(0, 0);
            panel14.Margin = new Padding(3, 2, 3, 2);
            panel14.Name = "panel14";
            panel14.Size = new Size(1488, 36);
            panel14.TabIndex = 0;
            // 
            // btnArchive
            // 
            btnArchive.BackColor = Color.Transparent;
            btnArchive.FlatAppearance.BorderSize = 0;
            btnArchive.FlatStyle = FlatStyle.Flat;
            btnArchive.Location = new Point(119, 3);
            btnArchive.Margin = new Padding(3, 2, 3, 2);
            btnArchive.Name = "btnArchive";
            btnArchive.Size = new Size(105, 30);
            btnArchive.TabIndex = 1;
            btnArchive.TabStop = false;
            btnArchive.Text = "Archive";
            btnArchive.UseVisualStyleBackColor = false;
            btnArchive.Click += btnArchive_Click;
            // 
            // btnCurriculum
            // 
            btnCurriculum.BackColor = Color.Transparent;
            btnCurriculum.FlatAppearance.BorderSize = 0;
            btnCurriculum.FlatStyle = FlatStyle.Flat;
            btnCurriculum.Location = new Point(9, 3);
            btnCurriculum.Margin = new Padding(3, 2, 3, 2);
            btnCurriculum.Name = "btnCurriculum";
            btnCurriculum.Size = new Size(105, 30);
            btnCurriculum.TabIndex = 0;
            btnCurriculum.TabStop = false;
            btnCurriculum.Text = "Curriculum";
            btnCurriculum.UseVisualStyleBackColor = false;
            btnCurriculum.Click += btnCurriculum_Click;
            // 
            // CourseCode2
            // 
            CourseCode2.DataPropertyName = "SubjectCode";
            CourseCode2.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            CourseCode2.FillWeight = 66.64282F;
            CourseCode2.HeaderText = "Subject Code";
            CourseCode2.MinimumWidth = 6;
            CourseCode2.Name = "CourseCode2";
            CourseCode2.Resizable = DataGridViewTriState.True;
            CourseCode2.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // CourseTitle2
            // 
            CourseTitle2.DataPropertyName = "SubjectName";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            CourseTitle2.DefaultCellStyle = dataGridViewCellStyle2;
            CourseTitle2.FillWeight = 421.4435F;
            CourseTitle2.HeaderText = "Subject Title";
            CourseTitle2.MinimumWidth = 6;
            CourseTitle2.Name = "CourseTitle2";
            CourseTitle2.ReadOnly = true;
            // 
            // Lab2
            // 
            Lab2.DataPropertyName = "LabUnits";
            Lab2.FillWeight = 21.4395962F;
            Lab2.HeaderText = "Lab";
            Lab2.MinimumWidth = 6;
            Lab2.Name = "Lab2";
            Lab2.ReadOnly = true;
            // 
            // Lec2
            // 
            Lec2.DataPropertyName = "LecUnits";
            Lec2.FillWeight = 23.9587688F;
            Lec2.HeaderText = "Lec";
            Lec2.MinimumWidth = 6;
            Lec2.Name = "Lec2";
            Lec2.ReadOnly = true;
            // 
            // TotalUnits2
            // 
            TotalUnits2.DataPropertyName = "Units";
            TotalUnits2.FillWeight = 33.1515274F;
            TotalUnits2.HeaderText = "Units";
            TotalUnits2.MinimumWidth = 6;
            TotalUnits2.Name = "TotalUnits2";
            TotalUnits2.ReadOnly = true;
            // 
            // colProgram
            // 
            colProgram.DataPropertyName = "Program";
            colProgram.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            colProgram.FillWeight = 50F;
            colProgram.HeaderText = "Program";
            colProgram.MinimumWidth = 6;
            colProgram.Name = "colProgram";
            colProgram.Resizable = DataGridViewTriState.True;
            colProgram.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // Year2
            // 
            Year2.DataPropertyName = "YearLevel";
            Year2.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            Year2.FillWeight = 49.2699051F;
            Year2.HeaderText = "Year";
            Year2.MinimumWidth = 6;
            Year2.Name = "Year2";
            Year2.Resizable = DataGridViewTriState.True;
            Year2.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // Semester1
            // 
            Semester1.DataPropertyName = "Semester";
            Semester1.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            Semester1.FillWeight = 84.09402F;
            Semester1.HeaderText = "Semester";
            Semester1.MinimumWidth = 6;
            Semester1.Name = "Semester1";
            Semester1.Resizable = DataGridViewTriState.True;
            Semester1.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // colRevisionYear
            // 
            colRevisionYear.DataPropertyName = "RevisionYear";
            colRevisionYear.FillWeight = 70F;
            colRevisionYear.HeaderText = "Revision Year";
            colRevisionYear.MinimumWidth = 6;
            colRevisionYear.Name = "colRevisionYear";
            colRevisionYear.ReadOnly = true;
            // 
            // CurriculumArchiveContentAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlCurriculumArchive);
            Name = "CurriculumArchiveContentAdmin";
            Size = new Size(1488, 993);
            pnlCurriculumArchive.ResumeLayout(false);
            pnlCurriculum.ResumeLayout(false);
            pnlCurriculum.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCurriculum).EndInit();
            pnlArchive.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvArchive).EndInit();
            panel14.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlCurriculumArchive;
        private Panel pnlCurriculum;
        private Label lblCurriculumList;
        private Button btnUpdateCurriculum;
        private DataGridView dgvCurriculum;
        private Panel panel14;
        private Button btnArchive;
        private Button btnCurriculum;
        private Panel pnlArchive;
        private DataGridView dgvArchive;
        private DataGridViewTextBoxColumn Semester;
        private DataGridViewTextBoxColumn SchoolYear;
        private DataGridViewTextBoxColumn blank;
        private Label label1;
        private DateTimePicker dtpRevisionYear;
        private TableLayoutPanel tableLayoutPanel1;
        private DataGridViewComboBoxColumn CourseCode2;
        private DataGridViewTextBoxColumn CourseTitle2;
        private DataGridViewTextBoxColumn Lab2;
        private DataGridViewTextBoxColumn Lec2;
        private DataGridViewTextBoxColumn TotalUnits2;
        private DataGridViewComboBoxColumn colProgram;
        private DataGridViewComboBoxColumn Year2;
        private DataGridViewComboBoxColumn Semester1;
        private DataGridViewTextBoxColumn colRevisionYear;
    }
}
