namespace PUPAcadPortal.PortalContents.Admin.SubOffering
{
    partial class CurrentSemesterContentAdmin
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            pnlCurrentSemester = new Panel();
            dgvCurrentSemester = new DataGridView();
            SubjectCode = new DataGridViewTextBoxColumn();
            SubjectTitle = new DataGridViewTextBoxColumn();
            Lab = new DataGridViewTextBoxColumn();
            Lec = new DataGridViewTextBoxColumn();
            TotalUnits = new DataGridViewTextBoxColumn();
            Year = new DataGridViewTextBoxColumn();
            panel2 = new Panel();
            panel1 = new Panel();
            btnInitialize = new Button();
            btnSetCurrent = new Button();
            lblSemesterSetup = new Label();
            cmbSY = new ComboBox();
            lblCourseList = new Label();
            lblSY = new Label();
            lblSem = new Label();
            cmbSem = new ComboBox();
            pnlCurrentSemester.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCurrentSemester).BeginInit();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // pnlCurrentSemester
            // 
            pnlCurrentSemester.BackColor = Color.White;
            pnlCurrentSemester.Controls.Add(dgvCurrentSemester);
            pnlCurrentSemester.Controls.Add(panel2);
            pnlCurrentSemester.Dock = DockStyle.Fill;
            pnlCurrentSemester.Location = new Point(0, 0);
            pnlCurrentSemester.Margin = new Padding(3, 2, 3, 2);
            pnlCurrentSemester.Name = "pnlCurrentSemester";
            pnlCurrentSemester.Size = new Size(1258, 704);
            pnlCurrentSemester.TabIndex = 1;
            // 
            // dgvCurrentSemester
            // 
            dgvCurrentSemester.AllowUserToAddRows = false;
            dgvCurrentSemester.AllowUserToDeleteRows = false;
            dgvCurrentSemester.AllowUserToResizeColumns = false;
            dgvCurrentSemester.AllowUserToResizeRows = false;
            dgvCurrentSemester.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvCurrentSemester.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCurrentSemester.BackgroundColor = Color.White;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = Color.White;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvCurrentSemester.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvCurrentSemester.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCurrentSemester.Columns.AddRange(new DataGridViewColumn[] { SubjectCode, SubjectTitle, Lab, Lec, TotalUnits, Year });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(255, 255, 128);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvCurrentSemester.DefaultCellStyle = dataGridViewCellStyle2;
            dgvCurrentSemester.EnableHeadersVisualStyles = false;
            dgvCurrentSemester.Location = new Point(26, 137);
            dgvCurrentSemester.Margin = new Padding(3, 2, 3, 2);
            dgvCurrentSemester.MultiSelect = false;
            dgvCurrentSemester.Name = "dgvCurrentSemester";
            dgvCurrentSemester.ReadOnly = true;
            dgvCurrentSemester.RowHeadersVisible = false;
            dgvCurrentSemester.RowHeadersWidth = 51;
            dgvCurrentSemester.ScrollBars = ScrollBars.Vertical;
            dgvCurrentSemester.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCurrentSemester.Size = new Size(1216, 495);
            dgvCurrentSemester.TabIndex = 7;
            // 
            // SubjectCode
            // 
            SubjectCode.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            SubjectCode.HeaderText = "Subject Code";
            SubjectCode.MinimumWidth = 6;
            SubjectCode.Name = "SubjectCode";
            SubjectCode.ReadOnly = true;
            SubjectCode.Width = 122;
            // 
            // SubjectTitle
            // 
            SubjectTitle.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            SubjectTitle.HeaderText = "Subject Title";
            SubjectTitle.MinimumWidth = 6;
            SubjectTitle.Name = "SubjectTitle";
            SubjectTitle.ReadOnly = true;
            SubjectTitle.Resizable = DataGridViewTriState.False;
            // 
            // Lab
            // 
            Lab.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Lab.HeaderText = "Lab";
            Lab.MinimumWidth = 6;
            Lab.Name = "Lab";
            Lab.ReadOnly = true;
            Lab.Width = 58;
            // 
            // Lec
            // 
            Lec.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Lec.HeaderText = "Lec";
            Lec.MinimumWidth = 6;
            Lec.Name = "Lec";
            Lec.ReadOnly = true;
            Lec.Width = 56;
            // 
            // TotalUnits
            // 
            TotalUnits.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            TotalUnits.HeaderText = "Total Units";
            TotalUnits.MinimumWidth = 6;
            TotalUnits.Name = "TotalUnits";
            TotalUnits.ReadOnly = true;
            TotalUnits.Width = 96;
            // 
            // Year
            // 
            Year.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Year.HeaderText = "Year";
            Year.MinimumWidth = 6;
            Year.Name = "Year";
            Year.ReadOnly = true;
            Year.Width = 64;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel2.BackColor = Color.White;
            panel2.Controls.Add(panel1);
            panel2.Controls.Add(lblSemesterSetup);
            panel2.Controls.Add(cmbSY);
            panel2.Controls.Add(lblCourseList);
            panel2.Controls.Add(lblSY);
            panel2.Controls.Add(lblSem);
            panel2.Controls.Add(cmbSem);
            panel2.Location = new Point(0, 0);
            panel2.Margin = new Padding(3, 2, 3, 2);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(18, 0, 18, 0);
            panel2.Size = new Size(1258, 132);
            panel2.TabIndex = 8;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Right;
            panel1.Controls.Add(btnInitialize);
            panel1.Controls.Add(btnSetCurrent);
            panel1.Location = new Point(842, 36);
            panel1.Name = "panel1";
            panel1.Size = new Size(410, 68);
            panel1.TabIndex = 8;
            // 
            // btnInitialize
            // 
            btnInitialize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnInitialize.BackColor = Color.ForestGreen;
            btnInitialize.Enabled = false;
            btnInitialize.FlatAppearance.BorderSize = 0;
            btnInitialize.FlatStyle = FlatStyle.Flat;
            btnInitialize.ForeColor = Color.White;
            btnInitialize.Location = new Point(18, 22);
            btnInitialize.Name = "btnInitialize";
            btnInitialize.Size = new Size(188, 27);
            btnInitialize.TabIndex = 7;
            btnInitialize.Text = "1. Generate Classes (Draft)";
            btnInitialize.UseVisualStyleBackColor = false;
            // 
            // btnSetCurrent
            // 
            btnSetCurrent.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSetCurrent.BackColor = Color.FromArgb(109, 0, 0);
            btnSetCurrent.Enabled = false;
            btnSetCurrent.FlatAppearance.BorderSize = 0;
            btnSetCurrent.FlatStyle = FlatStyle.Flat;
            btnSetCurrent.ForeColor = Color.White;
            btnSetCurrent.Location = new Point(220, 22);
            btnSetCurrent.Margin = new Padding(3, 2, 3, 2);
            btnSetCurrent.Name = "btnSetCurrent";
            btnSetCurrent.Size = new Size(177, 27);
            btnSetCurrent.TabIndex = 6;
            btnSetCurrent.Text = "2. Activate Semester (Lock)";
            btnSetCurrent.UseVisualStyleBackColor = false;
            // 
            // lblSemesterSetup
            // 
            lblSemesterSetup.AutoSize = true;
            lblSemesterSetup.BackColor = Color.White;
            lblSemesterSetup.Font = new Font("Arial", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSemesterSetup.Location = new Point(36, 8);
            lblSemesterSetup.Name = "lblSemesterSetup";
            lblSemesterSetup.Size = new Size(193, 29);
            lblSemesterSetup.TabIndex = 0;
            lblSemesterSetup.Text = "Semester Setup";
            // 
            // cmbSY
            // 
            cmbSY.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSY.FormattingEnabled = true;
            cmbSY.Items.AddRange(new object[] { "2025-2026", "2026-2027", "2027-2028" });
            cmbSY.Location = new Point(138, 40);
            cmbSY.Margin = new Padding(3, 2, 3, 2);
            cmbSY.Name = "cmbSY";
            cmbSY.Size = new Size(158, 23);
            cmbSY.TabIndex = 4;
            // 
            // lblCourseList
            // 
            lblCourseList.AutoSize = true;
            lblCourseList.Font = new Font("Arial", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCourseList.Location = new Point(36, 98);
            lblCourseList.Name = "lblCourseList";
            lblCourseList.Size = new Size(315, 26);
            lblCourseList.TabIndex = 3;
            lblCourseList.Text = "Current Semester Course List";
            // 
            // lblSY
            // 
            lblSY.AutoSize = true;
            lblSY.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSY.Location = new Point(36, 42);
            lblSY.Name = "lblSY";
            lblSY.Size = new Size(99, 18);
            lblSY.TabIndex = 1;
            lblSY.Text = "School Year: ";
            // 
            // lblSem
            // 
            lblSem.AutoSize = true;
            lblSem.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSem.Location = new Point(36, 70);
            lblSem.Name = "lblSem";
            lblSem.Size = new Size(84, 18);
            lblSem.TabIndex = 2;
            lblSem.Text = "Semester: ";
            // 
            // cmbSem
            // 
            cmbSem.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSem.FormattingEnabled = true;
            cmbSem.Items.AddRange(new object[] { "1", "2", "Summer" });
            cmbSem.Location = new Point(122, 68);
            cmbSem.Margin = new Padding(3, 2, 3, 2);
            cmbSem.Name = "cmbSem";
            cmbSem.Size = new Size(77, 23);
            cmbSem.TabIndex = 5;
            // 
            // CurrentSemesterContentAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlCurrentSemester);
            Name = "CurrentSemesterContentAdmin";
            Size = new Size(1258, 704);
            pnlCurrentSemester.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCurrentSemester).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlCurrentSemester;
        private DataGridView dgvCurrentSemester;
        private DataGridViewTextBoxColumn SubjectCode;
        private DataGridViewTextBoxColumn SubjectTitle;
        private DataGridViewTextBoxColumn Lab;
        private DataGridViewTextBoxColumn Lec;
        private DataGridViewTextBoxColumn TotalUnits;
        private DataGridViewTextBoxColumn Year;
        private Panel panel2;
        private Button btnSetCurrent;
        private Button btnInitialize;
        private Label lblSemesterSetup;
        private ComboBox cmbSY;
        private Label lblCourseList;
        private Label lblSY;
        private Label lblSem;
        private ComboBox cmbSem;
        private Panel panel1;
    }
}