namespace PUPAcadPortal.PortalContents.Student.LMS
{
    partial class AttendanceContentStudent
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
            pnlAttendance = new Panel();
            dgvLogs = new DataGridView();
            panelLogTitle = new Panel();
            lblAttendanceLogTitle = new Label();
            dgvSubjects = new DataGridView();
            panelSubjTitle = new Panel();
            lblSubjectsTitle = new Label();
            tableLayoutPanelCards = new TableLayoutPanel();
            cardAlerts = new Panel();
            btnViewDetails = new Button();
            lblAlertText = new Label();
            cardRequired = new Panel();
            lblRequiredValue = new Label();
            lblRequiredTitle = new Label();
            cardStatus = new Panel();
            lblStatusText = new Label();
            lblStatusTitle = new Label();
            cardTotal = new Panel();
            lblTotalValue = new Label();
            lblTotalTitle = new Label();
            cardOverall = new Panel();
            lblOverallPct = new Label();
            lblOverallTitle = new Label();
            panelHeaderControls = new Panel();
            cbmYear = new ComboBox();
            lblYear = new Label();
            button14 = new Button();
            btnRefresh = new Button();
            cmbMonth = new ComboBox();
            lblMonth = new Label();
            cmbSemester = new ComboBox();
            lblSemester = new Label();
            lblPageTitle = new Label();
            pnlAttendance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLogs).BeginInit();
            panelLogTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSubjects).BeginInit();
            panelSubjTitle.SuspendLayout();
            tableLayoutPanelCards.SuspendLayout();
            cardAlerts.SuspendLayout();
            cardRequired.SuspendLayout();
            cardStatus.SuspendLayout();
            cardTotal.SuspendLayout();
            cardOverall.SuspendLayout();
            panelHeaderControls.SuspendLayout();
            SuspendLayout();
            // 
            // pnlAttendance
            // 
            pnlAttendance.BackColor = SystemColors.Control;
            pnlAttendance.CausesValidation = false;
            pnlAttendance.Controls.Add(dgvLogs);
            pnlAttendance.Controls.Add(panelLogTitle);
            pnlAttendance.Controls.Add(dgvSubjects);
            pnlAttendance.Controls.Add(panelSubjTitle);
            pnlAttendance.Controls.Add(tableLayoutPanelCards);
            pnlAttendance.Controls.Add(panelHeaderControls);
            pnlAttendance.Dock = DockStyle.Fill;
            pnlAttendance.Location = new Point(0, 0);
            pnlAttendance.Margin = new Padding(0);
            pnlAttendance.Name = "pnlAttendance";
            pnlAttendance.Size = new Size(1648, 969);
            pnlAttendance.TabIndex = 40;
            // 
            // dgvLogs
            // 
            dgvLogs.BackgroundColor = Color.White;
            dgvLogs.BorderStyle = BorderStyle.None;
            dgvLogs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLogs.Dock = DockStyle.Fill;
            dgvLogs.Location = new Point(0, 616);
            dgvLogs.Name = "dgvLogs";
            dgvLogs.Size = new Size(1648, 353);
            dgvLogs.TabIndex = 6;
            // 
            // panelLogTitle
            // 
            panelLogTitle.Controls.Add(lblAttendanceLogTitle);
            panelLogTitle.Dock = DockStyle.Top;
            panelLogTitle.Location = new Point(0, 576);
            panelLogTitle.Name = "panelLogTitle";
            panelLogTitle.Size = new Size(1648, 40);
            panelLogTitle.TabIndex = 5;
            // 
            // lblAttendanceLogTitle
            // 
            lblAttendanceLogTitle.AutoSize = true;
            lblAttendanceLogTitle.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAttendanceLogTitle.Location = new Point(0, 10);
            lblAttendanceLogTitle.Name = "lblAttendanceLogTitle";
            lblAttendanceLogTitle.Size = new Size(120, 20);
            lblAttendanceLogTitle.TabIndex = 0;
            lblAttendanceLogTitle.Text = "Attendance Log";
            // 
            // dgvSubjects
            // 
            dgvSubjects.BackgroundColor = Color.White;
            dgvSubjects.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(240, 240, 240);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvSubjects.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvSubjects.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSubjects.Dock = DockStyle.Top;
            dgvSubjects.EnableHeadersVisualStyles = false;
            dgvSubjects.Location = new Point(0, 260);
            dgvSubjects.Name = "dgvSubjects";
            dgvSubjects.Size = new Size(1648, 316);
            dgvSubjects.TabIndex = 4;
            // 
            // panelSubjTitle
            // 
            panelSubjTitle.Controls.Add(lblSubjectsTitle);
            panelSubjTitle.Dock = DockStyle.Top;
            panelSubjTitle.Location = new Point(0, 220);
            panelSubjTitle.Name = "panelSubjTitle";
            panelSubjTitle.Size = new Size(1648, 40);
            panelSubjTitle.TabIndex = 3;
            // 
            // lblSubjectsTitle
            // 
            lblSubjectsTitle.AutoSize = true;
            lblSubjectsTitle.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSubjectsTitle.Location = new Point(0, 10);
            lblSubjectsTitle.Name = "lblSubjectsTitle";
            lblSubjectsTitle.Size = new Size(172, 20);
            lblSubjectsTitle.TabIndex = 0;
            lblSubjectsTitle.Text = "Attendance per Subject";
            // 
            // tableLayoutPanelCards
            // 
            tableLayoutPanelCards.ColumnCount = 5;
            tableLayoutPanelCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanelCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanelCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanelCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanelCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanelCards.Controls.Add(cardAlerts, 4, 0);
            tableLayoutPanelCards.Controls.Add(cardRequired, 3, 0);
            tableLayoutPanelCards.Controls.Add(cardStatus, 2, 0);
            tableLayoutPanelCards.Controls.Add(cardTotal, 1, 0);
            tableLayoutPanelCards.Controls.Add(cardOverall, 0, 0);
            tableLayoutPanelCards.Dock = DockStyle.Top;
            tableLayoutPanelCards.Location = new Point(0, 70);
            tableLayoutPanelCards.Name = "tableLayoutPanelCards";
            tableLayoutPanelCards.RowCount = 1;
            tableLayoutPanelCards.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanelCards.Size = new Size(1648, 150);
            tableLayoutPanelCards.TabIndex = 2;
            // 
            // cardAlerts
            // 
            cardAlerts.BackColor = Color.FromArgb(255, 240, 240);
            cardAlerts.Controls.Add(btnViewDetails);
            cardAlerts.Controls.Add(lblAlertText);
            cardAlerts.Dock = DockStyle.Fill;
            cardAlerts.Location = new Point(1321, 5);
            cardAlerts.Margin = new Padding(5);
            cardAlerts.Name = "cardAlerts";
            cardAlerts.Size = new Size(322, 140);
            cardAlerts.TabIndex = 4;
            // 
            // btnViewDetails
            // 
            btnViewDetails.BackColor = Color.Maroon;
            btnViewDetails.FlatStyle = FlatStyle.Flat;
            btnViewDetails.ForeColor = Color.White;
            btnViewDetails.Location = new Point(139, 90);
            btnViewDetails.Name = "btnViewDetails";
            btnViewDetails.Size = new Size(120, 30);
            btnViewDetails.TabIndex = 1;
            btnViewDetails.Text = "View Details";
            btnViewDetails.UseVisualStyleBackColor = false;
            // 
            // lblAlertText
            // 
            lblAlertText.ForeColor = Color.Maroon;
            lblAlertText.Location = new Point(94, 36);
            lblAlertText.Name = "lblAlertText";
            lblAlertText.Size = new Size(180, 40);
            lblAlertText.TabIndex = 0;
            lblAlertText.Text = "You have 1 subject with attendance below 75%.";
            lblAlertText.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cardRequired
            // 
            cardRequired.BackColor = Color.White;
            cardRequired.Controls.Add(lblRequiredValue);
            cardRequired.Controls.Add(lblRequiredTitle);
            cardRequired.Dock = DockStyle.Fill;
            cardRequired.Location = new Point(992, 5);
            cardRequired.Margin = new Padding(5);
            cardRequired.Name = "cardRequired";
            cardRequired.Size = new Size(319, 140);
            cardRequired.TabIndex = 3;
            // 
            // lblRequiredValue
            // 
            lblRequiredValue.Dock = DockStyle.Fill;
            lblRequiredValue.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRequiredValue.Location = new Point(0, 30);
            lblRequiredValue.Name = "lblRequiredValue";
            lblRequiredValue.Size = new Size(319, 110);
            lblRequiredValue.TabIndex = 1;
            lblRequiredValue.Text = "75%";
            lblRequiredValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblRequiredTitle
            // 
            lblRequiredTitle.Dock = DockStyle.Top;
            lblRequiredTitle.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRequiredTitle.Location = new Point(0, 0);
            lblRequiredTitle.Name = "lblRequiredTitle";
            lblRequiredTitle.Size = new Size(319, 30);
            lblRequiredTitle.TabIndex = 0;
            lblRequiredTitle.Text = "Required Attendance";
            lblRequiredTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cardStatus
            // 
            cardStatus.BackColor = Color.White;
            cardStatus.Controls.Add(lblStatusText);
            cardStatus.Controls.Add(lblStatusTitle);
            cardStatus.Dock = DockStyle.Fill;
            cardStatus.Location = new Point(663, 5);
            cardStatus.Margin = new Padding(5);
            cardStatus.Name = "cardStatus";
            cardStatus.Size = new Size(319, 140);
            cardStatus.TabIndex = 2;
            // 
            // lblStatusText
            // 
            lblStatusText.Dock = DockStyle.Fill;
            lblStatusText.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStatusText.ForeColor = Color.ForestGreen;
            lblStatusText.Location = new Point(0, 30);
            lblStatusText.Name = "lblStatusText";
            lblStatusText.Size = new Size(319, 110);
            lblStatusText.TabIndex = 2;
            lblStatusText.Text = "Good";
            lblStatusText.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblStatusTitle
            // 
            lblStatusTitle.Dock = DockStyle.Top;
            lblStatusTitle.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStatusTitle.Location = new Point(0, 0);
            lblStatusTitle.Name = "lblStatusTitle";
            lblStatusTitle.Size = new Size(319, 30);
            lblStatusTitle.TabIndex = 1;
            lblStatusTitle.Text = "Attendance Status";
            lblStatusTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cardTotal
            // 
            cardTotal.BackColor = Color.White;
            cardTotal.Controls.Add(lblTotalValue);
            cardTotal.Controls.Add(lblTotalTitle);
            cardTotal.Dock = DockStyle.Fill;
            cardTotal.Location = new Point(334, 5);
            cardTotal.Margin = new Padding(5);
            cardTotal.Name = "cardTotal";
            cardTotal.Size = new Size(319, 140);
            cardTotal.TabIndex = 1;
            // 
            // lblTotalValue
            // 
            lblTotalValue.BackColor = Color.Transparent;
            lblTotalValue.Dock = DockStyle.Top;
            lblTotalValue.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalValue.Location = new Point(0, 30);
            lblTotalValue.Name = "lblTotalValue";
            lblTotalValue.Size = new Size(319, 50);
            lblTotalValue.TabIndex = 2;
            lblTotalValue.Text = "135";
            lblTotalValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTotalTitle
            // 
            lblTotalTitle.Dock = DockStyle.Top;
            lblTotalTitle.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalTitle.Location = new Point(0, 0);
            lblTotalTitle.Name = "lblTotalTitle";
            lblTotalTitle.Size = new Size(319, 30);
            lblTotalTitle.TabIndex = 1;
            lblTotalTitle.Text = "Total Sessions";
            lblTotalTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cardOverall
            // 
            cardOverall.BackColor = Color.White;
            cardOverall.Controls.Add(lblOverallPct);
            cardOverall.Controls.Add(lblOverallTitle);
            cardOverall.Dock = DockStyle.Fill;
            cardOverall.Location = new Point(5, 5);
            cardOverall.Margin = new Padding(5);
            cardOverall.Name = "cardOverall";
            cardOverall.Size = new Size(319, 140);
            cardOverall.TabIndex = 0;
            // 
            // lblOverallPct
            // 
            lblOverallPct.Dock = DockStyle.Fill;
            lblOverallPct.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblOverallPct.ForeColor = Color.ForestGreen;
            lblOverallPct.Location = new Point(0, 30);
            lblOverallPct.Name = "lblOverallPct";
            lblOverallPct.Size = new Size(319, 110);
            lblOverallPct.TabIndex = 2;
            lblOverallPct.Text = "78%";
            lblOverallPct.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblOverallTitle
            // 
            lblOverallTitle.Dock = DockStyle.Top;
            lblOverallTitle.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblOverallTitle.Location = new Point(0, 0);
            lblOverallTitle.Name = "lblOverallTitle";
            lblOverallTitle.Size = new Size(319, 30);
            lblOverallTitle.TabIndex = 1;
            lblOverallTitle.Text = "Overall Attendance";
            lblOverallTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelHeaderControls
            // 
            panelHeaderControls.Controls.Add(cbmYear);
            panelHeaderControls.Controls.Add(lblYear);
            panelHeaderControls.Controls.Add(button14);
            panelHeaderControls.Controls.Add(btnRefresh);
            panelHeaderControls.Controls.Add(cmbMonth);
            panelHeaderControls.Controls.Add(lblMonth);
            panelHeaderControls.Controls.Add(cmbSemester);
            panelHeaderControls.Controls.Add(lblSemester);
            panelHeaderControls.Controls.Add(lblPageTitle);
            panelHeaderControls.Dock = DockStyle.Top;
            panelHeaderControls.Location = new Point(0, 0);
            panelHeaderControls.Name = "panelHeaderControls";
            panelHeaderControls.Size = new Size(1648, 70);
            panelHeaderControls.TabIndex = 1;
            // 
            // cbmYear
            // 
            cbmYear.FormattingEnabled = true;
            cbmYear.Location = new Point(483, 34);
            cbmYear.Name = "cbmYear";
            cbmYear.Size = new Size(121, 23);
            cbmYear.TabIndex = 8;
            cbmYear.Text = "2026";
            // 
            // lblYear
            // 
            lblYear.AutoSize = true;
            lblYear.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblYear.Location = new Point(447, 38);
            lblYear.Name = "lblYear";
            lblYear.Size = new Size(34, 15);
            lblYear.TabIndex = 7;
            lblYear.Text = "Year:";
            // 
            // button14
            // 
            button14.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button14.FlatStyle = FlatStyle.Flat;
            button14.Location = new Point(2986, 36);
            button14.Name = "button14";
            button14.Size = new Size(100, 25);
            button14.TabIndex = 6;
            button14.Text = "Refresh";
            button14.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Location = new Point(1538, 36);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 25);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            // 
            // cmbMonth
            // 
            cmbMonth.FormattingEnabled = true;
            cmbMonth.Location = new Point(299, 34);
            cmbMonth.Name = "cmbMonth";
            cmbMonth.Size = new Size(121, 23);
            cmbMonth.TabIndex = 4;
            cmbMonth.Text = "month";
            // 
            // lblMonth
            // 
            lblMonth.AutoSize = true;
            lblMonth.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMonth.Location = new Point(247, 38);
            lblMonth.Name = "lblMonth";
            lblMonth.Size = new Size(47, 15);
            lblMonth.TabIndex = 3;
            lblMonth.Text = "Month:";
            // 
            // cmbSemester
            // 
            cmbSemester.FormattingEnabled = true;
            cmbSemester.Location = new Point(64, 35);
            cmbSemester.Name = "cmbSemester";
            cmbSemester.Size = new Size(150, 23);
            cmbSemester.TabIndex = 2;
            cmbSemester.Text = "2nd Semester 2026";
            // 
            // lblSemester
            // 
            lblSemester.AutoSize = true;
            lblSemester.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSemester.Location = new Point(0, 40);
            lblSemester.Name = "lblSemester";
            lblSemester.Size = new Size(64, 15);
            lblSemester.TabIndex = 1;
            lblSemester.Text = "Semester:";
            // 
            // lblPageTitle
            // 
            lblPageTitle.AutoSize = true;
            lblPageTitle.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPageTitle.Location = new Point(3, 1);
            lblPageTitle.Name = "lblPageTitle";
            lblPageTitle.Size = new Size(163, 30);
            lblPageTitle.TabIndex = 0;
            lblPageTitle.Text = "My Attendance";
            // 
            // AttendanceContentStudent
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlAttendance);
            Name = "AttendanceContentStudent";
            Size = new Size(1648, 969);
            Load += AttendanceContentStudent_Load;
            pnlAttendance.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvLogs).EndInit();
            panelLogTitle.ResumeLayout(false);
            panelLogTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSubjects).EndInit();
            panelSubjTitle.ResumeLayout(false);
            panelSubjTitle.PerformLayout();
            tableLayoutPanelCards.ResumeLayout(false);
            cardAlerts.ResumeLayout(false);
            cardRequired.ResumeLayout(false);
            cardStatus.ResumeLayout(false);
            cardTotal.ResumeLayout(false);
            cardOverall.ResumeLayout(false);
            panelHeaderControls.ResumeLayout(false);
            panelHeaderControls.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlAttendance;
        private DataGridView dgvLogs;
        private Panel panelLogTitle;
        private Label lblAttendanceLogTitle;
        private DataGridView dgvSubjects;
        private Panel panelSubjTitle;
        private Label lblSubjectsTitle;
        private TableLayoutPanel tableLayoutPanelCards;
        private Panel cardAlerts;
        private Button btnViewDetails;
        private Label lblAlertText;
        private Panel cardRequired;
        private Label lblRequiredValue;
        private Label lblRequiredTitle;
        private Panel cardStatus;
        private Label lblStatusText;
        private Label lblStatusTitle;
        private Panel cardTotal;
        private Label lblTotalValue;
        private Label lblTotalTitle;
        private Panel cardOverall;
        private Label lblOverallPct;
        private Label lblOverallTitle;
        private Panel panelHeaderControls;
        private ComboBox cbmYear;
        private Label lblYear;
        private Button button14;
        private Button btnRefresh;
        private ComboBox cmbMonth;
        private Label lblMonth;
        private ComboBox cmbSemester;
        private Label lblSemester;
        private Label lblPageTitle;
    }
}
