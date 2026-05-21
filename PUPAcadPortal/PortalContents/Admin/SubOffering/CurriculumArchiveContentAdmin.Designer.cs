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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            pnlCurriculumArchive = new Panel();
            pnlCurriculum = new Panel();
            lblCurriculumList = new Label();
            btnUpdateCurriculum = new Button();
            dgvCurriculum = new DataGridView();
            CourseCode2 = new DataGridViewTextBoxColumn();
            CourseTitle2 = new DataGridViewTextBoxColumn();
            Lab2 = new DataGridViewTextBoxColumn();
            Lec2 = new DataGridViewTextBoxColumn();
            TotalUnits2 = new DataGridViewTextBoxColumn();
            Year2 = new DataGridViewTextBoxColumn();
            Semester1 = new DataGridViewTextBoxColumn();
            panel14 = new Panel();
            btnArchive = new Button();
            btnCurriculum = new Button();
            pnlArchive = new Panel();
            dgvArchive = new DataGridView();
            Semester = new DataGridViewTextBoxColumn();
            SchoolYear = new DataGridViewTextBoxColumn();
            blank = new DataGridViewTextBoxColumn();
            pnlCurriculumArchive.SuspendLayout();
            pnlCurriculum.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCurriculum).BeginInit();
            panel14.SuspendLayout();
            pnlArchive.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvArchive).BeginInit();
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
            // 
            // dgvCurriculum
            // 
            dgvCurriculum.AllowUserToAddRows = false;
            dgvCurriculum.AllowUserToDeleteRows = false;
            dgvCurriculum.AllowUserToOrderColumns = true;
            dgvCurriculum.AllowUserToResizeColumns = false;
            dgvCurriculum.AllowUserToResizeRows = false;
            dgvCurriculum.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvCurriculum.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCurriculum.BackgroundColor = Color.White;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = Color.White;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvCurriculum.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvCurriculum.ColumnHeadersHeight = 29;
            dgvCurriculum.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCurriculum.Columns.AddRange(new DataGridViewColumn[] { CourseCode2, CourseTitle2, Lab2, Lec2, TotalUnits2, Year2, Semester1 });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(255, 255, 128);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvCurriculum.DefaultCellStyle = dataGridViewCellStyle2;
            dgvCurriculum.EnableHeadersVisualStyles = false;
            dgvCurriculum.Location = new Point(44, 68);
            dgvCurriculum.Margin = new Padding(3, 2, 3, 2);
            dgvCurriculum.Name = "dgvCurriculum";
            dgvCurriculum.ReadOnly = true;
            dgvCurriculum.RowHeadersVisible = false;
            dgvCurriculum.RowHeadersWidth = 51;
            dgvCurriculum.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvCurriculum.ScrollBars = ScrollBars.Vertical;
            dgvCurriculum.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCurriculum.Size = new Size(1400, 495);
            dgvCurriculum.TabIndex = 0;
            // 
            // CourseCode2
            // 
            CourseCode2.FillWeight = 66.64282F;
            CourseCode2.HeaderText = "Subject Code";
            CourseCode2.MinimumWidth = 6;
            CourseCode2.Name = "CourseCode2";
            CourseCode2.ReadOnly = true;
            // 
            // CourseTitle2
            // 
            CourseTitle2.FillWeight = 421.4435F;
            CourseTitle2.HeaderText = "Subject Title";
            CourseTitle2.MinimumWidth = 6;
            CourseTitle2.Name = "CourseTitle2";
            CourseTitle2.ReadOnly = true;
            // 
            // Lab2
            // 
            Lab2.FillWeight = 21.4395962F;
            Lab2.HeaderText = "Lab";
            Lab2.MinimumWidth = 6;
            Lab2.Name = "Lab2";
            Lab2.ReadOnly = true;
            // 
            // Lec2
            // 
            Lec2.FillWeight = 23.9587688F;
            Lec2.HeaderText = "Lec";
            Lec2.MinimumWidth = 6;
            Lec2.Name = "Lec2";
            Lec2.ReadOnly = true;
            // 
            // TotalUnits2
            // 
            TotalUnits2.FillWeight = 33.1515274F;
            TotalUnits2.HeaderText = "Total Units";
            TotalUnits2.MinimumWidth = 6;
            TotalUnits2.Name = "TotalUnits2";
            TotalUnits2.ReadOnly = true;
            // 
            // Year2
            // 
            Year2.FillWeight = 49.2699051F;
            Year2.HeaderText = "Year";
            Year2.MinimumWidth = 6;
            Year2.Name = "Year2";
            Year2.ReadOnly = true;
            // 
            // Semester1
            // 
            Semester1.FillWeight = 84.09402F;
            Semester1.HeaderText = "Semester";
            Semester1.MinimumWidth = 6;
            Semester1.Name = "Semester1";
            Semester1.ReadOnly = true;
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
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = Color.White;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvArchive.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvArchive.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvArchive.Columns.AddRange(new DataGridViewColumn[] { Semester, SchoolYear, blank });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Window;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(255, 255, 128);
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            dgvArchive.DefaultCellStyle = dataGridViewCellStyle4;
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
            ((System.ComponentModel.ISupportInitialize)dgvCurriculum).EndInit();
            panel14.ResumeLayout(false);
            pnlArchive.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvArchive).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlCurriculumArchive;
        private Panel pnlCurriculum;
        private Label lblCurriculumList;
        private Button btnUpdateCurriculum;
        private DataGridView dgvCurriculum;
        private DataGridViewTextBoxColumn CourseCode2;
        private DataGridViewTextBoxColumn CourseTitle2;
        private DataGridViewTextBoxColumn Lab2;
        private DataGridViewTextBoxColumn Lec2;
        private DataGridViewTextBoxColumn TotalUnits2;
        private DataGridViewTextBoxColumn Year2;
        private DataGridViewTextBoxColumn Semester1;
        private Panel panel14;
        private Button btnArchive;
        private Button btnCurriculum;
        private Panel pnlArchive;
        private DataGridView dgvArchive;
        private DataGridViewTextBoxColumn Semester;
        private DataGridViewTextBoxColumn SchoolYear;
        private DataGridViewTextBoxColumn blank;
    }
}
