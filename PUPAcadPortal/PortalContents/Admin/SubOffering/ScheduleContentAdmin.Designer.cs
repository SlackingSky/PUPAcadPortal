namespace PUPAcadPortal.PortalContents.Admin.SubOffering
{
    partial class ScheduleContentAdmin
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
            pnlSchedule = new Panel();
            dgvScheduleView = new DataGridView();
            CourseCode1 = new DataGridViewTextBoxColumn();
            CourseTitle1 = new DataGridViewTextBoxColumn();
            Lec1 = new DataGridViewTextBoxColumn();
            Lab1 = new DataGridViewTextBoxColumn();
            TotalUnits1 = new DataGridViewTextBoxColumn();
            Section1 = new DataGridViewTextBoxColumn();
            Day1 = new DataGridViewTextBoxColumn();
            Start1 = new DataGridViewTextBoxColumn();
            End1 = new DataGridViewTextBoxColumn();
            Room1 = new DataGridViewTextBoxColumn();
            Instructor1 = new DataGridViewTextBoxColumn();
            panel10 = new Panel();
            btnExportExcel = new Button();
            btnExportPDF = new Button();
            cmbYearLevel = new ComboBox();
            label6 = new Label();
            label5 = new Label();
            pnlSchedule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvScheduleView).BeginInit();
            panel10.SuspendLayout();
            SuspendLayout();
            // 
            // pnlSchedule
            // 
            pnlSchedule.AutoScroll = true;
            pnlSchedule.BackColor = Color.White;
            pnlSchedule.Controls.Add(dgvScheduleView);
            pnlSchedule.Controls.Add(panel10);
            pnlSchedule.Dock = DockStyle.Fill;
            pnlSchedule.ForeColor = SystemColors.ControlText;
            pnlSchedule.Location = new Point(0, 0);
            pnlSchedule.Margin = new Padding(3, 2, 3, 2);
            pnlSchedule.Name = "pnlSchedule";
            pnlSchedule.Padding = new Padding(9, 0, 9, 0);
            pnlSchedule.Size = new Size(1488, 993);
            pnlSchedule.TabIndex = 8;
            // 
            // dgvScheduleView
            // 
            dgvScheduleView.AllowUserToDeleteRows = false;
            dgvScheduleView.AllowUserToResizeColumns = false;
            dgvScheduleView.AllowUserToResizeRows = false;
            dgvScheduleView.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvScheduleView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = Color.White;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvScheduleView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvScheduleView.ColumnHeadersHeight = 29;
            dgvScheduleView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvScheduleView.Columns.AddRange(new DataGridViewColumn[] { CourseCode1, CourseTitle1, Lec1, Lab1, TotalUnits1, Section1, Day1, Start1, End1, Room1, Instructor1 });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(255, 255, 128);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvScheduleView.DefaultCellStyle = dataGridViewCellStyle2;
            dgvScheduleView.EnableHeadersVisualStyles = false;
            dgvScheduleView.Location = new Point(35, 89);
            dgvScheduleView.Margin = new Padding(3, 2, 3, 2);
            dgvScheduleView.Name = "dgvScheduleView";
            dgvScheduleView.ReadOnly = true;
            dgvScheduleView.RowHeadersVisible = false;
            dgvScheduleView.RowHeadersWidth = 51;
            dgvScheduleView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvScheduleView.ScrollBars = ScrollBars.Vertical;
            dgvScheduleView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvScheduleView.Size = new Size(1427, 495);
            dgvScheduleView.TabIndex = 1;
            // 
            // CourseCode1
            // 
            CourseCode1.FillWeight = 97.57451F;
            CourseCode1.HeaderText = "Course Code";
            CourseCode1.MinimumWidth = 6;
            CourseCode1.Name = "CourseCode1";
            CourseCode1.ReadOnly = true;
            // 
            // CourseTitle1
            // 
            CourseTitle1.FillWeight = 456.61557F;
            CourseTitle1.HeaderText = "Course Title";
            CourseTitle1.MinimumWidth = 6;
            CourseTitle1.Name = "CourseTitle1";
            CourseTitle1.ReadOnly = true;
            // 
            // Lec1
            // 
            Lec1.FillWeight = 30.4825726F;
            Lec1.HeaderText = "Lec";
            Lec1.MinimumWidth = 6;
            Lec1.Name = "Lec1";
            Lec1.ReadOnly = true;
            // 
            // Lab1
            // 
            Lab1.FillWeight = 32.77244F;
            Lab1.HeaderText = "Lab";
            Lab1.MinimumWidth = 6;
            Lab1.Name = "Lab1";
            Lab1.ReadOnly = true;
            // 
            // TotalUnits1
            // 
            TotalUnits1.FillWeight = 64.0503159F;
            TotalUnits1.HeaderText = "Total Units";
            TotalUnits1.MinimumWidth = 6;
            TotalUnits1.Name = "TotalUnits1";
            TotalUnits1.ReadOnly = true;
            // 
            // Section1
            // 
            Section1.FillWeight = 49.5304565F;
            Section1.HeaderText = "Section";
            Section1.MinimumWidth = 6;
            Section1.Name = "Section1";
            Section1.ReadOnly = true;
            // 
            // Day1
            // 
            Day1.FillWeight = 80.77448F;
            Day1.HeaderText = "Day";
            Day1.MinimumWidth = 6;
            Day1.Name = "Day1";
            Day1.ReadOnly = true;
            // 
            // Start1
            // 
            Start1.FillWeight = 59.4697456F;
            Start1.HeaderText = "Start";
            Start1.MinimumWidth = 6;
            Start1.Name = "Start1";
            Start1.ReadOnly = true;
            // 
            // End1
            // 
            End1.FillWeight = 61.75498F;
            End1.HeaderText = "End";
            End1.MinimumWidth = 6;
            End1.Name = "End1";
            End1.ReadOnly = true;
            // 
            // Room1
            // 
            Room1.FillWeight = 56.41279F;
            Room1.HeaderText = "Room";
            Room1.MinimumWidth = 6;
            Room1.Name = "Room1";
            Room1.ReadOnly = true;
            // 
            // Instructor1
            // 
            Instructor1.FillWeight = 110.56234F;
            Instructor1.HeaderText = "Instructor";
            Instructor1.MinimumWidth = 6;
            Instructor1.Name = "Instructor1";
            Instructor1.ReadOnly = true;
            // 
            // panel10
            // 
            panel10.BackColor = Color.White;
            panel10.Controls.Add(btnExportExcel);
            panel10.Controls.Add(btnExportPDF);
            panel10.Controls.Add(cmbYearLevel);
            panel10.Controls.Add(label6);
            panel10.Controls.Add(label5);
            panel10.Dock = DockStyle.Top;
            panel10.Location = new Point(9, 0);
            panel10.Margin = new Padding(3, 2, 3, 2);
            panel10.Name = "panel10";
            panel10.Size = new Size(1470, 72);
            panel10.TabIndex = 0;
            // 
            // btnExportExcel
            // 
            btnExportExcel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExportExcel.BackColor = Color.FromArgb(109, 0, 0);
            btnExportExcel.FlatStyle = FlatStyle.Flat;
            btnExportExcel.ForeColor = Color.White;
            btnExportExcel.Location = new Point(1194, 34);
            btnExportExcel.Margin = new Padding(3, 2, 3, 2);
            btnExportExcel.Name = "btnExportExcel";
            btnExportExcel.Size = new Size(119, 28);
            btnExportExcel.TabIndex = 9;
            btnExportExcel.Text = "Export to Excel";
            btnExportExcel.UseVisualStyleBackColor = false;
            // 
            // btnExportPDF
            // 
            btnExportPDF.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExportPDF.BackColor = Color.FromArgb(109, 0, 0);
            btnExportPDF.FlatStyle = FlatStyle.Flat;
            btnExportPDF.ForeColor = Color.White;
            btnExportPDF.Location = new Point(1334, 34);
            btnExportPDF.Margin = new Padding(3, 2, 3, 2);
            btnExportPDF.Name = "btnExportPDF";
            btnExportPDF.Size = new Size(119, 28);
            btnExportPDF.TabIndex = 8;
            btnExportPDF.Text = "Export to PDF";
            btnExportPDF.UseVisualStyleBackColor = false;
            // 
            // cmbYearLevel
            // 
            cmbYearLevel.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbYearLevel.FormattingEnabled = true;
            cmbYearLevel.Items.AddRange(new object[] { "1", "2", "3", "4" });
            cmbYearLevel.Location = new Point(122, 44);
            cmbYearLevel.Margin = new Padding(3, 2, 3, 2);
            cmbYearLevel.Name = "cmbYearLevel";
            cmbYearLevel.Size = new Size(70, 23);
            cmbYearLevel.TabIndex = 7;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.ForeColor = SystemColors.ActiveCaptionText;
            label6.Location = new Point(16, 48);
            label6.Name = "label6";
            label6.Size = new Size(84, 18);
            label6.TabIndex = 7;
            label6.Text = "Year Level:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.White;
            label5.Font = new Font("Arial", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = SystemColors.ActiveCaptionText;
            label5.Location = new Point(16, 10);
            label5.Name = "label5";
            label5.Size = new Size(219, 29);
            label5.TabIndex = 7;
            label5.Text = "Current Semester:";
            // 
            // ScheduleContentAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlSchedule);
            Name = "ScheduleContentAdmin";
            Size = new Size(1488, 993);
            pnlSchedule.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvScheduleView).EndInit();
            panel10.ResumeLayout(false);
            panel10.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlSchedule;
        private DataGridView dgvScheduleView;
        private DataGridViewTextBoxColumn CourseCode1;
        private DataGridViewTextBoxColumn CourseTitle1;
        private DataGridViewTextBoxColumn Lec1;
        private DataGridViewTextBoxColumn Lab1;
        private DataGridViewTextBoxColumn TotalUnits1;
        private DataGridViewTextBoxColumn Section1;
        private DataGridViewTextBoxColumn Day1;
        private DataGridViewTextBoxColumn Start1;
        private DataGridViewTextBoxColumn End1;
        private DataGridViewTextBoxColumn Room1;
        private DataGridViewTextBoxColumn Instructor1;
        private Panel panel10;
        private Button btnExportExcel;
        private Button btnExportPDF;
        private ComboBox cmbYearLevel;
        private Label label6;
        private Label label5;
    }
}
