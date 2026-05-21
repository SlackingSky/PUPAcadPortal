namespace PUPAcadPortal.PortalContents.Admin.SubOffering
{
    partial class EditScheduleContentAdmin
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
            pnlEditSchedule = new Panel();
            panel1 = new Panel();
            btnClearSchedule = new Button();
            btnSaveSchedule = new Button();
            lblESYearLevel = new Label();
            lblESCurrentSem = new Label();
            cmbESYearLevel = new ComboBox();
            dgvEditSchedule = new DataGridView();
            ESCourseCode = new DataGridViewTextBoxColumn();
            ESCourseTitle = new DataGridViewTextBoxColumn();
            ESLab = new DataGridViewTextBoxColumn();
            ESLec = new DataGridViewTextBoxColumn();
            ESTotalUnits = new DataGridViewTextBoxColumn();
            ESSection = new DataGridViewTextBoxColumn();
            ESDay = new DataGridViewComboBoxColumn();
            ESStartTime = new DataGridViewTextBoxColumn();
            EsEndTime = new DataGridViewTextBoxColumn();
            ESRoom = new DataGridViewComboBoxColumn();
            ESInstructor = new DataGridViewTextBoxColumn();
            pnlEditSchedule.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEditSchedule).BeginInit();
            SuspendLayout();
            // 
            // pnlEditSchedule
            // 
            pnlEditSchedule.AutoScroll = true;
            pnlEditSchedule.AutoSize = true;
            pnlEditSchedule.BackColor = Color.White;
            pnlEditSchedule.Controls.Add(panel1);
            pnlEditSchedule.Controls.Add(dgvEditSchedule);
            pnlEditSchedule.Cursor = Cursors.Hand;
            pnlEditSchedule.Dock = DockStyle.Fill;
            pnlEditSchedule.Location = new Point(0, 0);
            pnlEditSchedule.Margin = new Padding(3, 2, 3, 2);
            pnlEditSchedule.Name = "pnlEditSchedule";
            pnlEditSchedule.Size = new Size(1488, 993);
            pnlEditSchedule.TabIndex = 9;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = Color.White;
            panel1.Controls.Add(btnClearSchedule);
            panel1.Controls.Add(btnSaveSchedule);
            panel1.Controls.Add(lblESYearLevel);
            panel1.Controls.Add(lblESCurrentSem);
            panel1.Controls.Add(cmbESYearLevel);
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(1471, 72);
            panel1.TabIndex = 7;
            // 
            // btnClearSchedule
            // 
            btnClearSchedule.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClearSchedule.BackColor = SystemColors.GrayText;
            btnClearSchedule.Cursor = Cursors.Hand;
            btnClearSchedule.FlatAppearance.BorderSize = 0;
            btnClearSchedule.FlatStyle = FlatStyle.Flat;
            btnClearSchedule.ForeColor = Color.White;
            btnClearSchedule.Location = new Point(1203, 36);
            btnClearSchedule.Margin = new Padding(3, 2, 3, 2);
            btnClearSchedule.Name = "btnClearSchedule";
            btnClearSchedule.Size = new Size(122, 27);
            btnClearSchedule.TabIndex = 4;
            btnClearSchedule.Text = "Clear Schedule";
            btnClearSchedule.UseVisualStyleBackColor = false;
            // 
            // btnSaveSchedule
            // 
            btnSaveSchedule.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveSchedule.BackColor = Color.ForestGreen;
            btnSaveSchedule.Cursor = Cursors.Hand;
            btnSaveSchedule.FlatAppearance.BorderSize = 0;
            btnSaveSchedule.FlatStyle = FlatStyle.Flat;
            btnSaveSchedule.ForeColor = Color.White;
            btnSaveSchedule.Location = new Point(1349, 36);
            btnSaveSchedule.Margin = new Padding(3, 2, 3, 2);
            btnSaveSchedule.Name = "btnSaveSchedule";
            btnSaveSchedule.Size = new Size(118, 27);
            btnSaveSchedule.TabIndex = 3;
            btnSaveSchedule.Text = "Save Schedule";
            btnSaveSchedule.UseVisualStyleBackColor = false;
            // 
            // lblESYearLevel
            // 
            lblESYearLevel.AutoSize = true;
            lblESYearLevel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblESYearLevel.Location = new Point(16, 41);
            lblESYearLevel.Name = "lblESYearLevel";
            lblESYearLevel.Size = new Size(88, 18);
            lblESYearLevel.TabIndex = 1;
            lblESYearLevel.Text = "Year Level: ";
            // 
            // lblESCurrentSem
            // 
            lblESCurrentSem.AutoSize = true;
            lblESCurrentSem.Font = new Font("Arial", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblESCurrentSem.Location = new Point(16, 9);
            lblESCurrentSem.Name = "lblESCurrentSem";
            lblESCurrentSem.Size = new Size(219, 29);
            lblESCurrentSem.TabIndex = 0;
            lblESCurrentSem.Text = "Current Semester:";
            // 
            // cmbESYearLevel
            // 
            cmbESYearLevel.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbESYearLevel.FormattingEnabled = true;
            cmbESYearLevel.Items.AddRange(new object[] { "All", "1", "2", "3", "4" });
            cmbESYearLevel.Location = new Point(123, 41);
            cmbESYearLevel.Margin = new Padding(3, 2, 3, 2);
            cmbESYearLevel.Name = "cmbESYearLevel";
            cmbESYearLevel.Size = new Size(70, 23);
            cmbESYearLevel.TabIndex = 2;
            // 
            // dgvEditSchedule
            // 
            dgvEditSchedule.AllowUserToAddRows = false;
            dgvEditSchedule.AllowUserToDeleteRows = false;
            dgvEditSchedule.AllowUserToResizeColumns = false;
            dgvEditSchedule.AllowUserToResizeRows = false;
            dgvEditSchedule.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvEditSchedule.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEditSchedule.BackgroundColor = Color.White;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = Color.White;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvEditSchedule.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvEditSchedule.ColumnHeadersHeight = 29;
            dgvEditSchedule.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvEditSchedule.Columns.AddRange(new DataGridViewColumn[] { ESCourseCode, ESCourseTitle, ESLab, ESLec, ESTotalUnits, ESSection, ESDay, ESStartTime, EsEndTime, ESRoom, ESInstructor });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(255, 255, 128);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvEditSchedule.DefaultCellStyle = dataGridViewCellStyle2;
            dgvEditSchedule.EnableHeadersVisualStyles = false;
            dgvEditSchedule.Location = new Point(26, 89);
            dgvEditSchedule.Margin = new Padding(3, 2, 3, 2);
            dgvEditSchedule.Name = "dgvEditSchedule";
            dgvEditSchedule.RowHeadersVisible = false;
            dgvEditSchedule.RowHeadersWidth = 51;
            dgvEditSchedule.ScrollBars = ScrollBars.Vertical;
            dgvEditSchedule.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEditSchedule.Size = new Size(1446, 495);
            dgvEditSchedule.TabIndex = 6;
            // 
            // ESCourseCode
            // 
            ESCourseCode.FillWeight = 97.12644F;
            ESCourseCode.HeaderText = "Subject Code";
            ESCourseCode.MinimumWidth = 60;
            ESCourseCode.Name = "ESCourseCode";
            ESCourseCode.ReadOnly = true;
            // 
            // ESCourseTitle
            // 
            ESCourseTitle.FillWeight = 148.68898F;
            ESCourseTitle.HeaderText = "Subject Title";
            ESCourseTitle.MinimumWidth = 60;
            ESCourseTitle.Name = "ESCourseTitle";
            ESCourseTitle.ReadOnly = true;
            // 
            // ESLab
            // 
            ESLab.FillWeight = 96.61446F;
            ESLab.HeaderText = "Lab";
            ESLab.MinimumWidth = 6;
            ESLab.Name = "ESLab";
            ESLab.ReadOnly = true;
            // 
            // ESLec
            // 
            ESLec.FillWeight = 89.54315F;
            ESLec.HeaderText = "Lec";
            ESLec.MinimumWidth = 6;
            ESLec.Name = "ESLec";
            ESLec.ReadOnly = true;
            // 
            // ESTotalUnits
            // 
            ESTotalUnits.FillWeight = 159.86705F;
            ESTotalUnits.HeaderText = "Total Units";
            ESTotalUnits.MinimumWidth = 6;
            ESTotalUnits.Name = "ESTotalUnits";
            ESTotalUnits.ReadOnly = true;
            // 
            // ESSection
            // 
            ESSection.FillWeight = 118.475517F;
            ESSection.HeaderText = "Section";
            ESSection.MinimumWidth = 6;
            ESSection.Name = "ESSection";
            ESSection.ReadOnly = true;
            // 
            // ESDay
            // 
            ESDay.FillWeight = 60.0069923F;
            ESDay.HeaderText = "Day";
            ESDay.Items.AddRange(new object[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" });
            ESDay.MinimumWidth = 6;
            ESDay.Name = "ESDay";
            // 
            // ESStartTime
            // 
            ESStartTime.FillWeight = 94.2693253F;
            ESStartTime.HeaderText = "Start";
            ESStartTime.MinimumWidth = 6;
            ESStartTime.Name = "ESStartTime";
            // 
            // EsEndTime
            // 
            EsEndTime.FillWeight = 79.74083F;
            EsEndTime.HeaderText = "End";
            EsEndTime.MinimumWidth = 6;
            EsEndTime.Name = "EsEndTime";
            // 
            // ESRoom
            // 
            ESRoom.FillWeight = 74.04477F;
            ESRoom.HeaderText = "Room";
            ESRoom.Items.AddRange(new object[] { "101", "102", "ComLab 1", "ComLab 2" });
            ESRoom.MinimumWidth = 6;
            ESRoom.Name = "ESRoom";
            // 
            // ESInstructor
            // 
            ESInstructor.FillWeight = 130.421341F;
            ESInstructor.HeaderText = "Instructor";
            ESInstructor.MinimumWidth = 6;
            ESInstructor.Name = "ESInstructor";
            ESInstructor.ReadOnly = true;
            // 
            // EditScheduleContentAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlEditSchedule);
            Name = "EditScheduleContentAdmin";
            Size = new Size(1488, 993);
            pnlEditSchedule.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEditSchedule).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlEditSchedule;
        private Panel panel1;
        private Button btnClearSchedule;
        private Button btnSaveSchedule;
        private Label lblESYearLevel;
        private Label lblESCurrentSem;
        private ComboBox cmbESYearLevel;
        private DataGridView dgvEditSchedule;
        private DataGridViewTextBoxColumn ESCourseCode;
        private DataGridViewTextBoxColumn ESCourseTitle;
        private DataGridViewTextBoxColumn ESLab;
        private DataGridViewTextBoxColumn ESLec;
        private DataGridViewTextBoxColumn ESTotalUnits;
        private DataGridViewTextBoxColumn ESSection;
        private DataGridViewComboBoxColumn ESDay;
        private DataGridViewTextBoxColumn ESStartTime;
        private DataGridViewTextBoxColumn EsEndTime;
        private DataGridViewComboBoxColumn ESRoom;
        private DataGridViewTextBoxColumn ESInstructor;
    }
}
