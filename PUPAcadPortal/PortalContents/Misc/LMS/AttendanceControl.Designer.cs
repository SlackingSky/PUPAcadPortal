namespace PUPAcadPortal
{
    partial class AttendanceControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dgvHeaderStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvDefaultStyle = new System.Windows.Forms.DataGridViewCellStyle();

            this.wrapper = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblPageTitle = new System.Windows.Forms.Label();
            this.lblSemLbl = new System.Windows.Forms.Label();
            this.cmbSemester = new System.Windows.Forms.ComboBox();
            this.lblMonthLbl = new System.Windows.Forms.Label();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.lblYearLbl = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tlpCards = new System.Windows.Forms.TableLayoutPanel();
            this.pnlCardOverall = new System.Windows.Forms.Panel();
            this.lblOverallPct = new System.Windows.Forms.Label();
            this.lblOverallTitle = new System.Windows.Forms.Label();
            this.pnlCardTotal = new System.Windows.Forms.Panel();
            this.lblTotalValue = new System.Windows.Forms.Label();
            this.lblTotalTitle = new System.Windows.Forms.Label();
            this.pnlCardStatus = new System.Windows.Forms.Panel();
            this.lblStatusText = new System.Windows.Forms.Label();
            this.lblStatusTitle = new System.Windows.Forms.Label();
            this.pnlCardRequired = new System.Windows.Forms.Panel();
            this.lblRequiredValue = new System.Windows.Forms.Label();
            this.lblRequiredTitle = new System.Windows.Forms.Label();
            this.pnlCardAlerts = new System.Windows.Forms.Panel();
            this.alertInner = new System.Windows.Forms.TableLayoutPanel();
            this.lblAlertText = new System.Windows.Forms.Label();
            this.btnViewDetails = new System.Windows.Forms.Button();
            this.pnlMiniStats = new System.Windows.Forms.Panel();
            this.lblMiniPresent = new System.Windows.Forms.Label();
            this.lblMiniLate = new System.Windows.Forms.Label();
            this.lblMiniAbsent = new System.Windows.Forms.Label();
            this.lblMiniExcused = new System.Windows.Forms.Label();
            this.pbAttendance = new System.Windows.Forms.ProgressBar();
            this.lblProgressPct = new System.Windows.Forms.Label();
            this.pnlSubjTitle = new System.Windows.Forms.Panel();
            this.lblSubjectsTitle = new System.Windows.Forms.Label();
            this.dgvSubjects = new System.Windows.Forms.DataGridView();
            this.pnlLogTitle = new System.Windows.Forms.Panel();
            this.lblAttendanceLogTitle = new System.Windows.Forms.Label();
            this.dgvLogs = new System.Windows.Forms.DataGridView();
            this.spacer = new System.Windows.Forms.Panel();

            // Grid Columns for Subjects
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSchedule = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSessions = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPresent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAbsent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExcused = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttendancePct = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();

            // Grid Columns for Logs
            this.colLogDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLogCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLogSubject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLogStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLogRemarks = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.wrapper.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.tlpCards.SuspendLayout();
            this.pnlCardOverall.SuspendLayout();
            this.pnlCardTotal.SuspendLayout();
            this.pnlCardStatus.SuspendLayout();
            this.pnlCardRequired.SuspendLayout();
            this.pnlCardAlerts.SuspendLayout();
            this.alertInner.SuspendLayout();
            this.pnlMiniStats.SuspendLayout();
            this.pnlSubjTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubjects)).BeginInit();
            this.pnlLogTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).BeginInit();
            this.SuspendLayout();

            // Shared Grid Styles
            dgvHeaderStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dgvHeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dgvHeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dgvHeaderStyle.ForeColor = System.Drawing.Color.White;
            dgvHeaderStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));

            dgvDefaultStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvDefaultStyle.BackColor = System.Drawing.SystemColors.Window;
            dgvDefaultStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            dgvDefaultStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            dgvDefaultStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            dgvDefaultStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));

            // 
            // wrapper
            // 
            this.wrapper.AutoScroll = true;
            this.wrapper.BackColor = System.Drawing.SystemColors.Control;
            this.wrapper.Controls.Add(this.spacer);
            this.wrapper.Controls.Add(this.dgvLogs);
            this.wrapper.Controls.Add(this.pnlLogTitle);
            this.wrapper.Controls.Add(this.dgvSubjects);
            this.wrapper.Controls.Add(this.pnlSubjTitle);
            this.wrapper.Controls.Add(this.pnlMiniStats);
            this.wrapper.Controls.Add(this.tlpCards);
            this.wrapper.Controls.Add(this.pnlHeader);
            this.wrapper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wrapper.Location = new System.Drawing.Point(0, 0);
            this.wrapper.Name = "wrapper";
            this.wrapper.Size = new System.Drawing.Size(1000, 750);
            this.wrapper.TabIndex = 0;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.btnRefresh);
            this.pnlHeader.Controls.Add(this.cmbYear);
            this.pnlHeader.Controls.Add(this.lblYearLbl);
            this.pnlHeader.Controls.Add(this.cmbMonth);
            this.pnlHeader.Controls.Add(this.lblMonthLbl);
            this.pnlHeader.Controls.Add(this.cmbSemester);
            this.pnlHeader.Controls.Add(this.lblSemLbl);
            this.pnlHeader.Controls.Add(this.lblPageTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(12, 6, 12, 0);
            this.pnlHeader.Size = new System.Drawing.Size(1000, 100);
            this.pnlHeader.TabIndex = 0;
            this.pnlHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHeader_Paint);
            // 
            // lblPageTitle
            // 
            this.lblPageTitle.AutoSize = true;
            this.lblPageTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblPageTitle.ForeColor = System.Drawing.Color.Maroon;
            this.lblPageTitle.Location = new System.Drawing.Point(12, 4);
            this.lblPageTitle.Name = "lblPageTitle";
            this.lblPageTitle.Size = new System.Drawing.Size(188, 32);
            this.lblPageTitle.TabIndex = 0;
            this.lblPageTitle.Text = "My Attendance";
            // 
            // lblSemLbl
            // 
            this.lblSemLbl.AutoSize = true;
            this.lblSemLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSemLbl.Location = new System.Drawing.Point(12, 46);
            this.lblSemLbl.Name = "lblSemLbl";
            this.lblSemLbl.Size = new System.Drawing.Size(63, 15);
            this.lblSemLbl.TabIndex = 1;
            this.lblSemLbl.Text = "Semester:";
            // 
            // cmbSemester
            // 
            this.cmbSemester.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSemester.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbSemester.Items.AddRange(new object[] { "All", "1st Semester", "2nd Semester" });
            this.cmbSemester.Location = new System.Drawing.Point(88, 43);
            this.cmbSemester.Name = "cmbSemester";
            this.cmbSemester.Size = new System.Drawing.Size(185, 23);
            this.cmbSemester.TabIndex = 2;
            this.cmbSemester.SelectedIndexChanged += new System.EventHandler(this.Filter_Changed);
            // 
            // lblMonthLbl
            // 
            this.lblMonthLbl.AutoSize = true;
            this.lblMonthLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMonthLbl.Location = new System.Drawing.Point(12, 74);
            this.lblMonthLbl.Name = "lblMonthLbl";
            this.lblMonthLbl.Size = new System.Drawing.Size(47, 15);
            this.lblMonthLbl.TabIndex = 3;
            this.lblMonthLbl.Text = "Month:";
            // 
            // cmbMonth
            // 
            this.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMonth.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbMonth.Items.AddRange(new object[] { "All", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" });
            this.cmbMonth.Location = new System.Drawing.Point(68, 71);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(130, 23);
            this.cmbMonth.TabIndex = 4;
            this.cmbMonth.SelectedIndexChanged += new System.EventHandler(this.Filter_Changed);
            // 
            // lblYearLbl
            // 
            this.lblYearLbl.AutoSize = true;
            this.lblYearLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblYearLbl.Location = new System.Drawing.Point(210, 74);
            this.lblYearLbl.Name = "lblYearLbl";
            this.lblYearLbl.Size = new System.Drawing.Size(34, 15);
            this.lblYearLbl.TabIndex = 5;
            this.lblYearLbl.Text = "Year:";
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbYear.Location = new System.Drawing.Point(255, 71);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(90, 23);
            this.cmbYear.TabIndex = 6;
            this.cmbYear.SelectedIndexChanged += new System.EventHandler(this.Filter_Changed);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.Maroon;
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(360, 68);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(108, 27);
            this.btnRefresh.TabIndex = 7;
            this.btnRefresh.Text = "↺  Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.Filter_Changed);
            // 
            // tlpCards
            // 
            this.tlpCards.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tlpCards.ColumnCount = 5;
            this.tlpCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpCards.Controls.Add(this.pnlCardOverall, 0, 0);
            this.tlpCards.Controls.Add(this.pnlCardTotal, 1, 0);
            this.tlpCards.Controls.Add(this.pnlCardStatus, 2, 0);
            this.tlpCards.Controls.Add(this.pnlCardRequired, 3, 0);
            this.tlpCards.Controls.Add(this.pnlCardAlerts, 4, 0);
            this.tlpCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpCards.Location = new System.Drawing.Point(0, 100);
            this.tlpCards.Name = "tlpCards";
            this.tlpCards.RowCount = 1;
            this.tlpCards.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCards.Size = new System.Drawing.Size(1000, 145);
            this.tlpCards.TabIndex = 1;
            // 
            // pnlCardOverall
            // 
            this.pnlCardOverall.BackColor = System.Drawing.Color.White;
            this.pnlCardOverall.Controls.Add(this.lblOverallPct);
            this.pnlCardOverall.Controls.Add(this.lblOverallTitle);
            this.pnlCardOverall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCardOverall.Location = new System.Drawing.Point(4, 4);
            this.pnlCardOverall.Margin = new System.Windows.Forms.Padding(4);
            this.pnlCardOverall.Name = "pnlCardOverall";
            this.pnlCardOverall.Size = new System.Drawing.Size(192, 137);
            this.pnlCardOverall.TabIndex = 0;
            // 
            // lblOverallPct
            // 
            this.lblOverallPct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOverallPct.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblOverallPct.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblOverallPct.Location = new System.Drawing.Point(0, 32);
            this.lblOverallPct.Name = "lblOverallPct";
            this.lblOverallPct.Size = new System.Drawing.Size(192, 105);
            this.lblOverallPct.TabIndex = 1;
            this.lblOverallPct.Text = "–";
            this.lblOverallPct.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOverallTitle
            // 
            this.lblOverallTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblOverallTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblOverallTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblOverallTitle.Location = new System.Drawing.Point(0, 0);
            this.lblOverallTitle.Name = "lblOverallTitle";
            this.lblOverallTitle.Size = new System.Drawing.Size(192, 32);
            this.lblOverallTitle.TabIndex = 0;
            this.lblOverallTitle.Text = "Overall Attendance";
            this.lblOverallTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlCardTotal
            // 
            this.pnlCardTotal.BackColor = System.Drawing.Color.White;
            this.pnlCardTotal.Controls.Add(this.lblTotalValue);
            this.pnlCardTotal.Controls.Add(this.lblTotalTitle);
            this.pnlCardTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCardTotal.Location = new System.Drawing.Point(204, 4);
            this.pnlCardTotal.Margin = new System.Windows.Forms.Padding(4);
            this.pnlCardTotal.Name = "pnlCardTotal";
            this.pnlCardTotal.Size = new System.Drawing.Size(192, 137);
            this.pnlCardTotal.TabIndex = 1;
            // 
            // lblTotalValue
            // 
            this.lblTotalValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalValue.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblTotalValue.Location = new System.Drawing.Point(0, 32);
            this.lblTotalValue.Name = "lblTotalValue";
            this.lblTotalValue.Size = new System.Drawing.Size(192, 105);
            this.lblTotalValue.TabIndex = 1;
            this.lblTotalValue.Text = "–";
            this.lblTotalValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTotalTitle
            // 
            this.lblTotalTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTotalTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblTotalTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTotalTitle.Name = "lblTotalTitle";
            this.lblTotalTitle.Size = new System.Drawing.Size(192, 32);
            this.lblTotalTitle.TabIndex = 0;
            this.lblTotalTitle.Text = "Total Sessions";
            this.lblTotalTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlCardStatus
            // 
            this.pnlCardStatus.BackColor = System.Drawing.Color.White;
            this.pnlCardStatus.Controls.Add(this.lblStatusText);
            this.pnlCardStatus.Controls.Add(this.lblStatusTitle);
            this.pnlCardStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCardStatus.Location = new System.Drawing.Point(404, 4);
            this.pnlCardStatus.Margin = new System.Windows.Forms.Padding(4);
            this.pnlCardStatus.Name = "pnlCardStatus";
            this.pnlCardStatus.Size = new System.Drawing.Size(192, 137);
            this.pnlCardStatus.TabIndex = 2;
            // 
            // lblStatusText
            // 
            this.lblStatusText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatusText.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblStatusText.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblStatusText.Location = new System.Drawing.Point(0, 32);
            this.lblStatusText.Name = "lblStatusText";
            this.lblStatusText.Size = new System.Drawing.Size(192, 105);
            this.lblStatusText.TabIndex = 1;
            this.lblStatusText.Text = "–";
            this.lblStatusText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStatusTitle
            // 
            this.lblStatusTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStatusTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatusTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblStatusTitle.Location = new System.Drawing.Point(0, 0);
            this.lblStatusTitle.Name = "lblStatusTitle";
            this.lblStatusTitle.Size = new System.Drawing.Size(192, 32);
            this.lblStatusTitle.TabIndex = 0;
            this.lblStatusTitle.Text = "Attendance Status";
            this.lblStatusTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlCardRequired
            // 
            this.pnlCardRequired.BackColor = System.Drawing.Color.White;
            this.pnlCardRequired.Controls.Add(this.lblRequiredValue);
            this.pnlCardRequired.Controls.Add(this.lblRequiredTitle);
            this.pnlCardRequired.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCardRequired.Location = new System.Drawing.Point(604, 4);
            this.pnlCardRequired.Margin = new System.Windows.Forms.Padding(4);
            this.pnlCardRequired.Name = "pnlCardRequired";
            this.pnlCardRequired.Size = new System.Drawing.Size(192, 137);
            this.pnlCardRequired.TabIndex = 3;
            // 
            // lblRequiredValue
            // 
            this.lblRequiredValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRequiredValue.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblRequiredValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblRequiredValue.Location = new System.Drawing.Point(0, 32);
            this.lblRequiredValue.Name = "lblRequiredValue";
            this.lblRequiredValue.Size = new System.Drawing.Size(192, 105);
            this.lblRequiredValue.TabIndex = 1;
            this.lblRequiredValue.Text = "80%";
            this.lblRequiredValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRequiredTitle
            // 
            this.lblRequiredTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRequiredTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblRequiredTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblRequiredTitle.Location = new System.Drawing.Point(0, 0);
            this.lblRequiredTitle.Name = "lblRequiredTitle";
            this.lblRequiredTitle.Size = new System.Drawing.Size(192, 32);
            this.lblRequiredTitle.TabIndex = 0;
            this.lblRequiredTitle.Text = "Required Attendance";
            this.lblRequiredTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlCardAlerts
            // 
            this.pnlCardAlerts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pnlCardAlerts.Controls.Add(this.alertInner);
            this.pnlCardAlerts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCardAlerts.Location = new System.Drawing.Point(804, 4);
            this.pnlCardAlerts.Margin = new System.Windows.Forms.Padding(4);
            this.pnlCardAlerts.Name = "pnlCardAlerts";
            this.pnlCardAlerts.Padding = new System.Windows.Forms.Padding(10);
            this.pnlCardAlerts.Size = new System.Drawing.Size(192, 137);
            this.pnlCardAlerts.TabIndex = 4;
            // 
            // alertInner
            // 
            this.alertInner.BackColor = System.Drawing.Color.Transparent;
            this.alertInner.ColumnCount = 1;
            this.alertInner.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.alertInner.Controls.Add(this.lblAlertText, 0, 0);
            this.alertInner.Controls.Add(this.btnViewDetails, 0, 1);
            this.alertInner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alertInner.Location = new System.Drawing.Point(10, 10);
            this.alertInner.Name = "alertInner";
            this.alertInner.RowCount = 2;
            this.alertInner.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.alertInner.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.alertInner.Size = new System.Drawing.Size(172, 117);
            this.alertInner.TabIndex = 0;
            // 
            // lblAlertText
            // 
            this.lblAlertText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAlertText.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblAlertText.ForeColor = System.Drawing.Color.Crimson;
            this.lblAlertText.Location = new System.Drawing.Point(3, 0);
            this.lblAlertText.Name = "lblAlertText";
            this.lblAlertText.Size = new System.Drawing.Size(166, 70);
            this.lblAlertText.TabIndex = 0;
            this.lblAlertText.Text = "Loading…";
            this.lblAlertText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnViewDetails
            // 
            this.btnViewDetails.BackColor = System.Drawing.Color.Maroon;
            this.btnViewDetails.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnViewDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnViewDetails.FlatAppearance.BorderSize = 0;
            this.btnViewDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewDetails.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnViewDetails.ForeColor = System.Drawing.Color.White;
            this.btnViewDetails.Location = new System.Drawing.Point(20, 74);
            this.btnViewDetails.Margin = new System.Windows.Forms.Padding(20, 4, 20, 4);
            this.btnViewDetails.Name = "btnViewDetails";
            this.btnViewDetails.Size = new System.Drawing.Size(132, 39);
            this.btnViewDetails.TabIndex = 1;
            this.btnViewDetails.Text = "View Details";
            this.btnViewDetails.UseVisualStyleBackColor = false;
            this.btnViewDetails.Click += new System.EventHandler(this.OnViewDetailsClick);
            // 
            // pnlMiniStats
            // 
            this.pnlMiniStats.BackColor = System.Drawing.Color.White;
            this.pnlMiniStats.Controls.Add(this.lblProgressPct);
            this.pnlMiniStats.Controls.Add(this.pbAttendance);
            this.pnlMiniStats.Controls.Add(this.lblMiniExcused);
            this.pnlMiniStats.Controls.Add(this.lblMiniAbsent);
            this.pnlMiniStats.Controls.Add(this.lblMiniLate);
            this.pnlMiniStats.Controls.Add(this.lblMiniPresent);
            this.pnlMiniStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMiniStats.Location = new System.Drawing.Point(0, 245);
            this.pnlMiniStats.Name = "pnlMiniStats";
            this.pnlMiniStats.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.pnlMiniStats.Size = new System.Drawing.Size(1000, 46);
            this.pnlMiniStats.TabIndex = 2;
            this.pnlMiniStats.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlMiniStats_Paint);
            // 
            // lblMiniPresent
            // 
            this.lblMiniPresent.AutoSize = true;
            this.lblMiniPresent.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblMiniPresent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(50)))));
            this.lblMiniPresent.Location = new System.Drawing.Point(10, 13);
            this.lblMiniPresent.Name = "lblMiniPresent";
            this.lblMiniPresent.Size = new System.Drawing.Size(81, 17);
            this.lblMiniPresent.TabIndex = 0;
            this.lblMiniPresent.Text = "● Present: –";
            // 
            // lblMiniLate
            // 
            this.lblMiniLate.AutoSize = true;
            this.lblMiniLate.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblMiniLate.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblMiniLate.Location = new System.Drawing.Point(138, 13);
            this.lblMiniLate.Name = "lblMiniLate";
            this.lblMiniLate.Size = new System.Drawing.Size(59, 17);
            this.lblMiniLate.TabIndex = 1;
            this.lblMiniLate.Text = "● Late: –";
            // 
            // lblMiniAbsent
            // 
            this.lblMiniAbsent.AutoSize = true;
            this.lblMiniAbsent.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblMiniAbsent.ForeColor = System.Drawing.Color.Crimson;
            this.lblMiniAbsent.Location = new System.Drawing.Point(266, 13);
            this.lblMiniAbsent.Name = "lblMiniAbsent";
            this.lblMiniAbsent.Size = new System.Drawing.Size(78, 17);
            this.lblMiniAbsent.TabIndex = 2;
            this.lblMiniAbsent.Text = "● Absent: –";
            // 
            // lblMiniExcused
            // 
            this.lblMiniExcused.AutoSize = true;
            this.lblMiniExcused.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblMiniExcused.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblMiniExcused.Location = new System.Drawing.Point(394, 13);
            this.lblMiniExcused.Name = "lblMiniExcused";
            this.lblMiniExcused.Size = new System.Drawing.Size(85, 17);
            this.lblMiniExcused.TabIndex = 3;
            this.lblMiniExcused.Text = "● Excused: –";
            // 
            // pbAttendance
            // 
            this.pbAttendance.Location = new System.Drawing.Point(538, 13);
            this.pbAttendance.Name = "pbAttendance";
            this.pbAttendance.Size = new System.Drawing.Size(220, 18);
            this.pbAttendance.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbAttendance.TabIndex = 4;
            // 
            // lblProgressPct
            // 
            this.lblProgressPct.AutoSize = true;
            this.lblProgressPct.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblProgressPct.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblProgressPct.Location = new System.Drawing.Point(768, 13);
            this.lblProgressPct.Name = "lblProgressPct";
            this.lblProgressPct.Size = new System.Drawing.Size(24, 15);
            this.lblProgressPct.TabIndex = 5;
            this.lblProgressPct.Text = "0%";
            // 
            // pnlSubjTitle
            // 
            this.pnlSubjTitle.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlSubjTitle.Controls.Add(this.lblSubjectsTitle);
            this.pnlSubjTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSubjTitle.Location = new System.Drawing.Point(0, 291);
            this.pnlSubjTitle.Name = "pnlSubjTitle";
            this.pnlSubjTitle.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.pnlSubjTitle.Size = new System.Drawing.Size(1000, 40);
            this.pnlSubjTitle.TabIndex = 3;
            this.pnlSubjTitle.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlSubjTitle_Paint);
            // 
            // lblSubjectsTitle
            // 
            this.lblSubjectsTitle.AutoSize = true;
            this.lblSubjectsTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSubjectsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblSubjectsTitle.Location = new System.Drawing.Point(12, 10);
            this.lblSubjectsTitle.Name = "lblSubjectsTitle";
            this.lblSubjectsTitle.Size = new System.Drawing.Size(359, 19);
            this.lblSubjectsTitle.TabIndex = 0;
            this.lblSubjectsTitle.Text = "Attendance per Subject  (click a row to see its log)";
            // 
            // dgvSubjects
            // 
            this.dgvSubjects.AllowUserToAddRows = false;
            this.dgvSubjects.AllowUserToDeleteRows = false;
            this.dgvSubjects.AllowUserToResizeColumns = false;
            this.dgvSubjects.AllowUserToResizeRows = false;
            this.dgvSubjects.AutoGenerateColumns = false;
            this.dgvSubjects.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvSubjects.BackgroundColor = System.Drawing.Color.White;
            this.dgvSubjects.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSubjects.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvSubjects.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvSubjects.ColumnHeadersDefaultCellStyle = dgvHeaderStyle;
            this.dgvSubjects.ColumnHeadersHeight = 44;
            this.dgvSubjects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCode, this.colSubject, this.colSchedule, this.colSessions,
            this.colPresent, this.colAbsent, this.colLate, this.colExcused,
            this.colAttendancePct, this.colStatus});
            this.dgvSubjects.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dgvSubjects.DefaultCellStyle = dgvDefaultStyle;
            this.dgvSubjects.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvSubjects.EnableHeadersVisualStyles = false;
            this.dgvSubjects.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvSubjects.Location = new System.Drawing.Point(0, 331);
            this.dgvSubjects.Name = "dgvSubjects";
            this.dgvSubjects.ReadOnly = true;
            this.dgvSubjects.RowHeadersVisible = false;
            this.dgvSubjects.RowTemplate.Height = 42;
            this.dgvSubjects.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvSubjects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSubjects.Size = new System.Drawing.Size(1000, 10);
            this.dgvSubjects.TabIndex = 4;
            this.dgvSubjects.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvSubjects_CellClick);
            this.dgvSubjects.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DgvSubjects_CellFormatting);
            this.dgvSubjects.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.DgvSubjects_DataBindingComplete);
            this.dgvSubjects.SelectionChanged += new System.EventHandler(this.Dgv_SelectionChanged);
            // 
            // Columns For dgvSubjects
            //
            this.colCode.DataPropertyName = "Code"; this.colCode.HeaderText = "Code"; this.colCode.Name = "colCode"; this.colCode.Width = 90; this.colCode.ReadOnly = true;
            this.colSubject.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill; this.colSubject.DataPropertyName = "Subject"; this.colSubject.HeaderText = "Subject"; this.colSubject.Name = "colSubject"; this.colSubject.ReadOnly = true;
            this.colSchedule.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill; this.colSchedule.DataPropertyName = "Schedule"; this.colSchedule.HeaderText = "Schedule"; this.colSchedule.Name = "colSchedule"; this.colSchedule.ReadOnly = true;
            this.colSessions.DataPropertyName = "Sessions"; this.colSessions.HeaderText = "Sessions"; this.colSessions.Name = "colSessions"; this.colSessions.Width = 75; this.colSessions.ReadOnly = true; this.colSessions.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colPresent.DataPropertyName = "Present"; this.colPresent.HeaderText = "Present"; this.colPresent.Name = "colPresent"; this.colPresent.Width = 70; this.colPresent.ReadOnly = true; this.colPresent.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colAbsent.DataPropertyName = "Absent"; this.colAbsent.HeaderText = "Absent"; this.colAbsent.Name = "colAbsent"; this.colAbsent.Width = 70; this.colAbsent.ReadOnly = true; this.colAbsent.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colLate.DataPropertyName = "Late"; this.colLate.HeaderText = "Late"; this.colLate.Name = "colLate"; this.colLate.Width = 65; this.colLate.ReadOnly = true; this.colLate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colExcused.DataPropertyName = "Excused"; this.colExcused.HeaderText = "Excused"; this.colExcused.Name = "colExcused"; this.colExcused.Width = 72; this.colExcused.ReadOnly = true; this.colExcused.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colAttendancePct.DataPropertyName = "Attendance%"; this.colAttendancePct.HeaderText = "Attendance%"; this.colAttendancePct.Name = "colAttendancePct"; this.colAttendancePct.Width = 100; this.colAttendancePct.ReadOnly = true; this.colAttendancePct.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colStatus.DataPropertyName = "Status"; this.colStatus.HeaderText = "Status"; this.colStatus.Name = "colStatus"; this.colStatus.Width = 95; this.colStatus.ReadOnly = true; this.colStatus.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            // 
            // pnlLogTitle
            // 
            this.pnlLogTitle.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlLogTitle.Controls.Add(this.lblAttendanceLogTitle);
            this.pnlLogTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogTitle.Location = new System.Drawing.Point(0, 341);
            this.pnlLogTitle.Name = "pnlLogTitle";
            this.pnlLogTitle.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.pnlLogTitle.Size = new System.Drawing.Size(1000, 40);
            this.pnlLogTitle.TabIndex = 5;
            this.pnlLogTitle.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlLogTitle_Paint);
            // 
            // lblAttendanceLogTitle
            // 
            this.lblAttendanceLogTitle.AutoSize = true;
            this.lblAttendanceLogTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblAttendanceLogTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblAttendanceLogTitle.Location = new System.Drawing.Point(12, 10);
            this.lblAttendanceLogTitle.Name = "lblAttendanceLogTitle";
            this.lblAttendanceLogTitle.Size = new System.Drawing.Size(117, 19);
            this.lblAttendanceLogTitle.TabIndex = 0;
            this.lblAttendanceLogTitle.Text = "Attendance Log";
            // 
            // dgvLogs
            // 
            this.dgvLogs.AllowUserToAddRows = false;
            this.dgvLogs.AllowUserToDeleteRows = false;
            this.dgvLogs.AllowUserToResizeColumns = false;
            this.dgvLogs.AllowUserToResizeRows = false;
            this.dgvLogs.AutoGenerateColumns = false;
            this.dgvLogs.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvLogs.BackgroundColor = System.Drawing.Color.White;
            this.dgvLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLogs.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvLogs.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvLogs.ColumnHeadersDefaultCellStyle = dgvHeaderStyle;
            this.dgvLogs.ColumnHeadersHeight = 44;
            this.dgvLogs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLogDate, this.colLogCode, this.colLogSubject, this.colLogStatus, this.colLogRemarks});
            this.dgvLogs.DefaultCellStyle = dgvDefaultStyle;
            this.dgvLogs.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvLogs.EnableHeadersVisualStyles = false;
            this.dgvLogs.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvLogs.Location = new System.Drawing.Point(0, 381);
            this.dgvLogs.Name = "dgvLogs";
            this.dgvLogs.ReadOnly = true;
            this.dgvLogs.RowHeadersVisible = false;
            this.dgvLogs.RowTemplate.Height = 40;
            this.dgvLogs.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvLogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLogs.Size = new System.Drawing.Size(1000, 10);
            this.dgvLogs.TabIndex = 6;
            this.dgvLogs.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DgvLogs_CellFormatting);
            this.dgvLogs.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.DgvLogs_DataBindingComplete);
            this.dgvLogs.SelectionChanged += new System.EventHandler(this.Dgv_SelectionChanged);
            // 
            // Columns For dgvLogs
            //
            this.colLogDate.DataPropertyName = "Date"; this.colLogDate.HeaderText = "Date"; this.colLogDate.Name = "colLogDate"; this.colLogDate.Width = 175; this.colLogDate.ReadOnly = true;
            this.colLogCode.DataPropertyName = "Code"; this.colLogCode.HeaderText = "Code"; this.colLogCode.Name = "colLogCode"; this.colLogCode.Width = 110; this.colLogCode.ReadOnly = true;
            this.colLogSubject.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill; this.colLogSubject.DataPropertyName = "Subject"; this.colLogSubject.HeaderText = "Subject"; this.colLogSubject.Name = "colLogSubject"; this.colLogSubject.ReadOnly = true;
            this.colLogStatus.DataPropertyName = "Status"; this.colLogStatus.HeaderText = "Status"; this.colLogStatus.Name = "colLogStatus"; this.colLogStatus.Width = 95; this.colLogStatus.ReadOnly = true; this.colLogStatus.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colLogRemarks.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill; this.colLogRemarks.DataPropertyName = "Remarks"; this.colLogRemarks.HeaderText = "Remarks"; this.colLogRemarks.Name = "colLogRemarks"; this.colLogRemarks.ReadOnly = true;

            // 
            // spacer
            // 
            this.spacer.BackColor = System.Drawing.SystemColors.Control;
            this.spacer.Dock = System.Windows.Forms.DockStyle.Top;
            this.spacer.Location = new System.Drawing.Point(0, 391);
            this.spacer.Name = "spacer";
            this.spacer.Size = new System.Drawing.Size(1000, 32);
            this.spacer.TabIndex = 7;
            // 
            // AttendanceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.wrapper);
            this.Name = "AttendanceControl";
            this.Size = new System.Drawing.Size(1000, 750);
            this.Load += new System.EventHandler(this.AttendanceControl_Load);
            this.wrapper.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.tlpCards.ResumeLayout(false);
            this.pnlCardOverall.ResumeLayout(false);
            this.pnlCardTotal.ResumeLayout(false);
            this.pnlCardStatus.ResumeLayout(false);
            this.pnlCardRequired.ResumeLayout(false);
            this.pnlCardAlerts.ResumeLayout(false);
            this.alertInner.ResumeLayout(false);
            this.pnlMiniStats.ResumeLayout(false);
            this.pnlMiniStats.PerformLayout();
            this.pnlSubjTitle.ResumeLayout(false);
            this.pnlSubjTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubjects)).EndInit();
            this.pnlLogTitle.ResumeLayout(false);
            this.pnlLogTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).EndInit();
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.Panel wrapper;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Label lblSemLbl;
        private System.Windows.Forms.ComboBox cmbSemester;
        private System.Windows.Forms.Label lblMonthLbl;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.Label lblYearLbl;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.Button btnRefresh;

        private System.Windows.Forms.TableLayoutPanel tlpCards;
        private System.Windows.Forms.Panel pnlCardOverall;
        private System.Windows.Forms.Label lblOverallTitle;
        private System.Windows.Forms.Label lblOverallPct;
        private System.Windows.Forms.Panel pnlCardTotal;
        private System.Windows.Forms.Label lblTotalTitle;
        private System.Windows.Forms.Label lblTotalValue;
        private System.Windows.Forms.Panel pnlCardStatus;
        private System.Windows.Forms.Label lblStatusTitle;
        private System.Windows.Forms.Label lblStatusText;
        private System.Windows.Forms.Panel pnlCardRequired;
        private System.Windows.Forms.Label lblRequiredTitle;
        private System.Windows.Forms.Label lblRequiredValue;
        private System.Windows.Forms.Panel pnlCardAlerts;
        private System.Windows.Forms.TableLayoutPanel alertInner;
        private System.Windows.Forms.Label lblAlertText;
        private System.Windows.Forms.Button btnViewDetails;

        private System.Windows.Forms.Panel pnlMiniStats;
        private System.Windows.Forms.Label lblMiniPresent;
        private System.Windows.Forms.Label lblMiniLate;
        private System.Windows.Forms.Label lblMiniAbsent;
        private System.Windows.Forms.Label lblMiniExcused;
        private System.Windows.Forms.ProgressBar pbAttendance;
        private System.Windows.Forms.Label lblProgressPct;

        private System.Windows.Forms.Panel pnlSubjTitle;
        private System.Windows.Forms.Label lblSubjectsTitle;
        private System.Windows.Forms.DataGridView dgvSubjects;

        private System.Windows.Forms.Panel pnlLogTitle;
        private System.Windows.Forms.Label lblAttendanceLogTitle;
        private System.Windows.Forms.DataGridView dgvLogs;

        private System.Windows.Forms.Panel spacer;

        // DGV Columns
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubject;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSchedule;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSessions;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPresent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAbsent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExcused;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttendancePct;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;

        private System.Windows.Forms.DataGridViewTextBoxColumn colLogDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLogCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLogSubject;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLogStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLogRemarks;


        
    }
}