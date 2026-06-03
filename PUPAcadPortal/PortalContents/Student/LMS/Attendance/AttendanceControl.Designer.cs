namespace PUPAcadPortal.PortalContents.Student.LMS.Attendance
{
    partial class AttendanceControl
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
            this.pnlScrollWrapper = new System.Windows.Forms.Panel();
            this.pnlSpacer = new System.Windows.Forms.Panel();
            this.pnlLogsSection = new System.Windows.Forms.Panel();
            this.pnlLogsGap = new System.Windows.Forms.Panel();
            this.dgvLogs = new System.Windows.Forms.DataGridView();
            this.pnlLogsTitleRow = new System.Windows.Forms.Panel();
            this.lblAttendanceLogTitle = new System.Windows.Forms.Label();
            this.pnlSubjSection = new System.Windows.Forms.Panel();
            this.pnlSubjGap = new System.Windows.Forms.Panel();
            this.dgvSubjects = new System.Windows.Forms.DataGridView();
            this.pnlSubjTitleRow = new System.Windows.Forms.Panel();
            this.lblSubjectsTitle = new System.Windows.Forms.Label();
            this.lblSubjectsHint = new System.Windows.Forms.Label();
            // ── FIX: added pnlQrGap to match sibling sections ──────────────────────
            this.pnlQrSection = new System.Windows.Forms.Panel();
            this.pnlQrGap = new System.Windows.Forms.Panel();
            this.dgvQR = new System.Windows.Forms.DataGridView();
            this.pnlQRTitle = new System.Windows.Forms.Panel();
            this.lblQrIcon = new System.Windows.Forms.Label();
            this.lblQRTitle = new System.Windows.Forms.Label();
            this.lblQrBadge = new System.Windows.Forms.Label();
            this.pnlMiniStats = new System.Windows.Forms.Panel();
            this.lblMiniPresent = new System.Windows.Forms.Label();
            this.lblMiniLate = new System.Windows.Forms.Label();
            this.lblMiniAbsent = new System.Windows.Forms.Label();
            this.lblMiniExcused = new System.Windows.Forms.Label();
            this.pnlProgress = new System.Windows.Forms.Panel();
            this.lblProgressPct = new System.Windows.Forms.Label();
            this.tlpCards = new System.Windows.Forms.TableLayoutPanel();
            this.pnlCardOverall = new System.Windows.Forms.Panel();
            this.pnlBarOverall = new System.Windows.Forms.Panel();
            this.lblOverallTitle = new System.Windows.Forms.Label();
            this.lblOverallPct = new System.Windows.Forms.Label();
            this.pnlCardPresent = new System.Windows.Forms.Panel();
            this.pnlBarPresent = new System.Windows.Forms.Panel();
            this.lblPresentTitle = new System.Windows.Forms.Label();
            this.lblPresentValue = new System.Windows.Forms.Label();
            this.pnlCardLate = new System.Windows.Forms.Panel();
            this.pnlBarLate = new System.Windows.Forms.Panel();
            this.lblLateTitle = new System.Windows.Forms.Label();
            this.lblLateValue = new System.Windows.Forms.Label();
            this.pnlCardAbsent = new System.Windows.Forms.Panel();
            this.pnlBarAbsent = new System.Windows.Forms.Panel();
            this.lblAbsentTitle = new System.Windows.Forms.Label();
            this.lblAbsentValue = new System.Windows.Forms.Label();
            this.pnlCardAlerts = new System.Windows.Forms.Panel();
            this.pnlBarAlerts = new System.Windows.Forms.Panel();
            this.lblAlertText = new System.Windows.Forms.Label();
            this.btnViewDetails = new System.Windows.Forms.Button();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblPageTitle = new System.Windows.Forms.Label();
            this.lblHeaderSubtitle = new System.Windows.Forms.Label();
            this.lblPeriodLbl = new System.Windows.Forms.Label();
            this.cmbPeriod = new System.Windows.Forms.ComboBox();
            this.lblCourseLbl = new System.Windows.Forms.Label();
            this.cmbCourse = new System.Windows.Forms.ComboBox();
            this.lblFromLbl = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblToLbl = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnScanQR = new System.Windows.Forms.Button();

            // Grid column definitions
            System.Windows.Forms.DataGridViewTextBoxColumn colQRDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            System.Windows.Forms.DataGridViewTextBoxColumn colQRCourse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            System.Windows.Forms.DataGridViewTextBoxColumn colQRSession = new System.Windows.Forms.DataGridViewTextBoxColumn();
            System.Windows.Forms.DataGridViewTextBoxColumn colQRScanTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            System.Windows.Forms.DataGridViewTextBoxColumn colQRStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();

            System.Windows.Forms.DataGridViewTextBoxColumn colSubjCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            System.Windows.Forms.DataGridViewTextBoxColumn colSubjName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            System.Windows.Forms.DataGridViewTextBoxColumn colSubjSched = new System.Windows.Forms.DataGridViewTextBoxColumn();
            System.Windows.Forms.DataGridViewTextBoxColumn colSubjSessions = new System.Windows.Forms.DataGridViewTextBoxColumn();
            System.Windows.Forms.DataGridViewTextBoxColumn colSubjPresent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            System.Windows.Forms.DataGridViewTextBoxColumn colSubjLate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            System.Windows.Forms.DataGridViewTextBoxColumn colSubjAbsent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            System.Windows.Forms.DataGridViewTextBoxColumn colSubjExcused = new System.Windows.Forms.DataGridViewTextBoxColumn();
            System.Windows.Forms.DataGridViewTextBoxColumn colSubjPct = new System.Windows.Forms.DataGridViewTextBoxColumn();
            System.Windows.Forms.DataGridViewTextBoxColumn colSubjStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();

            System.Windows.Forms.DataGridViewTextBoxColumn colLogDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            System.Windows.Forms.DataGridViewTextBoxColumn colLogCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            System.Windows.Forms.DataGridViewTextBoxColumn colLogSession = new System.Windows.Forms.DataGridViewTextBoxColumn();
            System.Windows.Forms.DataGridViewTextBoxColumn colLogPeriod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            System.Windows.Forms.DataGridViewTextBoxColumn colLogStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            System.Windows.Forms.DataGridViewTextBoxColumn colLogRemarks = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.pnlScrollWrapper.SuspendLayout();
            this.pnlLogsSection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).BeginInit();
            this.pnlLogsTitleRow.SuspendLayout();
            this.pnlSubjSection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubjects)).BeginInit();
            this.pnlSubjTitleRow.SuspendLayout();
            this.pnlQrSection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQR)).BeginInit();
            this.pnlQRTitle.SuspendLayout();
            this.pnlMiniStats.SuspendLayout();
            this.tlpCards.SuspendLayout();
            this.pnlCardOverall.SuspendLayout();
            this.pnlCardPresent.SuspendLayout();
            this.pnlCardLate.SuspendLayout();
            this.pnlCardAbsent.SuspendLayout();
            this.pnlCardAlerts.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();

            // ──────────────────────────────────────────────────────────────────────
            // AttendanceControl Root
            // ──────────────────────────────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "AttendanceControl";
            this.Size = new System.Drawing.Size(950, 700);
            this.Controls.Add(this.pnlScrollWrapper);
            this.Load += new System.EventHandler(this.AttendanceControl_Load);

            // ──────────────────────────────────────────────────────────────────────
            // pnlScrollWrapper
            // ──────────────────────────────────────────────────────────────────────
            this.pnlScrollWrapper.AutoScroll = true;
            this.pnlScrollWrapper.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.pnlScrollWrapper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScrollWrapper.Name = "pnlScrollWrapper";
            this.pnlScrollWrapper.Controls.Add(this.pnlSpacer);
            this.pnlScrollWrapper.Controls.Add(this.pnlLogsSection);
            this.pnlScrollWrapper.Controls.Add(this.pnlSubjSection);
            this.pnlScrollWrapper.Controls.Add(this.pnlQrSection);
            this.pnlScrollWrapper.Controls.Add(this.pnlMiniStats);
            this.pnlScrollWrapper.Controls.Add(this.tlpCards);
            this.pnlScrollWrapper.Controls.Add(this.pnlHeader);

            // ──────────────────────────────────────────────────────────────────────
            // pnlSpacer
            // ──────────────────────────────────────────────────────────────────────
            this.pnlSpacer.BackColor = System.Drawing.Color.Transparent;
            this.pnlSpacer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSpacer.Height = 24;
            this.pnlSpacer.Name = "pnlSpacer";

            // ──────────────────────────────────────────────────────────────────────
            // pnlLogsSection
            // ──────────────────────────────────────────────────────────────────────
            this.pnlLogsSection.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.pnlLogsSection.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogsSection.Name = "pnlLogsSection";
            this.pnlLogsSection.AutoSize = true;
            this.pnlLogsSection.Controls.Add(this.pnlLogsGap);
            this.pnlLogsSection.Controls.Add(this.dgvLogs);
            this.pnlLogsSection.Controls.Add(this.pnlLogsTitleRow);

            // pnlLogsGap
            this.pnlLogsGap.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.pnlLogsGap.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogsGap.Height = 8;
            this.pnlLogsGap.Name = "pnlLogsGap";

            // dgvLogs
            this.dgvLogs.AllowUserToAddRows = false;
            this.dgvLogs.AllowUserToDeleteRows = false;
            this.dgvLogs.AllowUserToResizeColumns = false;
            this.dgvLogs.AllowUserToResizeRows = false;
            this.dgvLogs.AutoGenerateColumns = false;
            this.dgvLogs.BackgroundColor = System.Drawing.Color.White;
            this.dgvLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLogs.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvLogs.ColumnHeadersHeight = 42;
            this.dgvLogs.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvLogs.GridColor = System.Drawing.Color.FromArgb(230, 230, 238);
            this.dgvLogs.Height = 10;
            this.dgvLogs.Name = "dgvLogs";
            this.dgvLogs.RowHeadersVisible = false;
            this.dgvLogs.RowTemplate.Height = 40;
            this.dgvLogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLogs.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvLogs.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.ApplyGridHeaderSettings(this.dgvLogs);
            this.dgvLogs.SelectionChanged += (s, e) => this.dgvLogs.ClearSelection();
            this.dgvLogs.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DgvLogs_CellFormatting);
            this.dgvLogs.DataBindingComplete += (s, e) => AttendanceControl.AutoSizeGrid(this.dgvLogs, 600);

            colLogDate.Name = "Date"; colLogDate.HeaderText = "Date"; colLogDate.DataPropertyName = "Date";
            colLogDate.Width = 155; colLogDate.ReadOnly = true;
            colLogDate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;

            colLogCode.Name = "Code"; colLogCode.HeaderText = "Code"; colLogCode.DataPropertyName = "Code";
            colLogCode.Width = 100; colLogCode.ReadOnly = true;
            colLogCode.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;

            colLogSession.Name = "Session"; colLogSession.HeaderText = "Session"; colLogSession.DataPropertyName = "Session";
            colLogSession.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colLogSession.ReadOnly = true;
            colLogSession.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;

            colLogPeriod.Name = "Period"; colLogPeriod.HeaderText = "Period"; colLogPeriod.DataPropertyName = "Period";
            colLogPeriod.Width = 100; colLogPeriod.ReadOnly = true;
            colLogPeriod.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            colLogStatus.Name = "Status"; colLogStatus.HeaderText = "Status"; colLogStatus.DataPropertyName = "Status";
            colLogStatus.Width = 90; colLogStatus.ReadOnly = true;
            colLogStatus.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            colLogRemarks.Name = "Remarks"; colLogRemarks.HeaderText = "Remarks"; colLogRemarks.DataPropertyName = "Remarks";
            colLogRemarks.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colLogRemarks.ReadOnly = true;
            colLogRemarks.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;

            this.dgvLogs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
                { colLogDate, colLogCode, colLogSession, colLogPeriod, colLogStatus, colLogRemarks });

            // pnlLogsTitleRow
            this.pnlLogsTitleRow.BackColor = System.Drawing.Color.White;
            this.pnlLogsTitleRow.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogsTitleRow.Height = 42;
            this.pnlLogsTitleRow.Name = "pnlLogsTitleRow";
            this.pnlLogsTitleRow.Controls.Add(this.lblAttendanceLogTitle);
            this.pnlLogsTitleRow.Paint += (s, e) => e.Graphics.DrawLine(
                new System.Drawing.Pen(System.Drawing.Color.FromArgb(225, 225, 235), 1),
                0, this.pnlLogsTitleRow.Height - 1, this.pnlLogsTitleRow.Width, this.pnlLogsTitleRow.Height - 1);

            // lblAttendanceLogTitle
            this.lblAttendanceLogTitle.AutoSize = true;
            this.lblAttendanceLogTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblAttendanceLogTitle.ForeColor = System.Drawing.Color.FromArgb(50, 50, 70);
            this.lblAttendanceLogTitle.Location = new System.Drawing.Point(18, 11);
            this.lblAttendanceLogTitle.Name = "lblAttendanceLogTitle";
            this.lblAttendanceLogTitle.Text = "Attendance Log";

            // ──────────────────────────────────────────────────────────────────────
            // pnlSubjSection
            // ──────────────────────────────────────────────────────────────────────
            this.pnlSubjSection.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.pnlSubjSection.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSubjSection.Name = "pnlSubjSection";
            this.pnlSubjSection.AutoSize = true;
            this.pnlSubjSection.Controls.Add(this.pnlSubjGap);
            this.pnlSubjSection.Controls.Add(this.dgvSubjects);
            this.pnlSubjSection.Controls.Add(this.pnlSubjTitleRow);

            // pnlSubjGap
            this.pnlSubjGap.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.pnlSubjGap.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSubjGap.Height = 8;
            this.pnlSubjGap.Name = "pnlSubjGap";

            // dgvSubjects
            this.dgvSubjects.AllowUserToAddRows = false;
            this.dgvSubjects.AllowUserToDeleteRows = false;
            this.dgvSubjects.AllowUserToResizeColumns = false;
            this.dgvSubjects.AllowUserToResizeRows = false;
            this.dgvSubjects.AutoGenerateColumns = false;
            this.dgvSubjects.BackgroundColor = System.Drawing.Color.White;
            this.dgvSubjects.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSubjects.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvSubjects.ColumnHeadersHeight = 42;
            this.dgvSubjects.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dgvSubjects.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvSubjects.GridColor = System.Drawing.Color.FromArgb(230, 230, 238);
            this.dgvSubjects.Height = 10;
            this.dgvSubjects.Name = "dgvSubjects";
            this.dgvSubjects.RowHeadersVisible = false;
            this.dgvSubjects.RowTemplate.Height = 42;
            this.dgvSubjects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSubjects.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvSubjects.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.ApplyGridHeaderSettings(this.dgvSubjects);
            this.dgvSubjects.SelectionChanged += (s, e) => this.dgvSubjects.ClearSelection();
            this.dgvSubjects.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DgvSubjects_CellFormatting);
            this.dgvSubjects.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvSubjects_CellClick);
            this.dgvSubjects.DataBindingComplete += (s, e) => AttendanceControl.AutoSizeGrid(this.dgvSubjects, 800);

            colSubjCode.Name = "Code"; colSubjCode.HeaderText = "Code"; colSubjCode.DataPropertyName = "Code";
            colSubjCode.Width = 90; colSubjCode.ReadOnly = true;
            colSubjCode.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;

            colSubjName.Name = "Course Name"; colSubjName.HeaderText = "Course Name"; colSubjName.DataPropertyName = "Course Name";
            colSubjName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colSubjName.ReadOnly = true;
            colSubjName.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;

            colSubjSched.Name = "Schedule"; colSubjSched.HeaderText = "Schedule"; colSubjSched.DataPropertyName = "Schedule";
            colSubjSched.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colSubjSched.ReadOnly = true;
            colSubjSched.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;

            colSubjSessions.Name = "Sessions"; colSubjSessions.HeaderText = "Sessions"; colSubjSessions.DataPropertyName = "Sessions";
            colSubjSessions.Width = 76; colSubjSessions.ReadOnly = true;
            colSubjSessions.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            colSubjPresent.Name = "Present"; colSubjPresent.HeaderText = "Present"; colSubjPresent.DataPropertyName = "Present";
            colSubjPresent.Width = 72; colSubjPresent.ReadOnly = true;
            colSubjPresent.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            colSubjLate.Name = "Late"; colSubjLate.HeaderText = "Late"; colSubjLate.DataPropertyName = "Late";
            colSubjLate.Width = 65; colSubjLate.ReadOnly = true;
            colSubjLate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            colSubjAbsent.Name = "Absent"; colSubjAbsent.HeaderText = "Absent"; colSubjAbsent.DataPropertyName = "Absent";
            colSubjAbsent.Width = 68; colSubjAbsent.ReadOnly = true;
            colSubjAbsent.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            colSubjExcused.Name = "Excused"; colSubjExcused.HeaderText = "Excused"; colSubjExcused.DataPropertyName = "Excused";
            colSubjExcused.Width = 72; colSubjExcused.ReadOnly = true;
            colSubjExcused.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            colSubjPct.Name = "Att%"; colSubjPct.HeaderText = "Att%"; colSubjPct.DataPropertyName = "Att%";
            colSubjPct.Width = 85; colSubjPct.ReadOnly = true;
            colSubjPct.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            colSubjStatus.Name = "Status"; colSubjStatus.HeaderText = "Status"; colSubjStatus.DataPropertyName = "Status";
            colSubjStatus.Width = 90; colSubjStatus.ReadOnly = true;
            colSubjStatus.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            this.dgvSubjects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
                { colSubjCode, colSubjName, colSubjSched, colSubjSessions,
                  colSubjPresent, colSubjLate, colSubjAbsent, colSubjExcused, colSubjPct, colSubjStatus });

            // pnlSubjTitleRow
            this.pnlSubjTitleRow.BackColor = System.Drawing.Color.White;
            this.pnlSubjTitleRow.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSubjTitleRow.Height = 42;
            this.pnlSubjTitleRow.Name = "pnlSubjTitleRow";
            this.pnlSubjTitleRow.Controls.Add(this.lblSubjectsTitle);
            this.pnlSubjTitleRow.Controls.Add(this.lblSubjectsHint);
            this.pnlSubjTitleRow.Paint += (s, e) => e.Graphics.DrawLine(
                new System.Drawing.Pen(System.Drawing.Color.FromArgb(225, 225, 235), 1),
                0, this.pnlSubjTitleRow.Height - 1, this.pnlSubjTitleRow.Width, this.pnlSubjTitleRow.Height - 1);

            // lblSubjectsTitle
            this.lblSubjectsTitle.AutoSize = true;
            this.lblSubjectsTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblSubjectsTitle.ForeColor = System.Drawing.Color.FromArgb(50, 50, 70);
            this.lblSubjectsTitle.Location = new System.Drawing.Point(18, 11);
            this.lblSubjectsTitle.Name = "lblSubjectsTitle";
            this.lblSubjectsTitle.Text = "Attendance per Course";

            // lblSubjectsHint
            this.lblSubjectsHint.AutoSize = true;
            this.lblSubjectsHint.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblSubjectsHint.ForeColor = System.Drawing.Color.FromArgb(140, 140, 160);
            this.lblSubjectsHint.Location = new System.Drawing.Point(204, 13);
            this.lblSubjectsHint.Name = "lblSubjectsHint";
            this.lblSubjectsHint.Text = "(click a row to view its log)";

            // ══════════════════════════════════════════════════════════════════════
            // pnlQrSection  ← FIX: AutoSize = true replaces hard-coded Height = 86
            //                       so the section grows to fit dgvQR's actual rows.
            //               Added pnlQrGap (8 px) to match sibling section styling.
            // ══════════════════════════════════════════════════════════════════════
            this.pnlQrSection.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.pnlQrSection.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlQrSection.AutoSize = true;                              // ← FIX (was Height = 86)
            this.pnlQrSection.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink; // ← FIX (new)
            this.pnlQrSection.Name = "pnlQrSection";
            this.pnlQrSection.Padding = new System.Windows.Forms.Padding(18, 6, 18, 4);
            this.pnlQrSection.Controls.Add(this.pnlQrGap);                 // ← FIX (new gap panel)
            this.pnlQrSection.Controls.Add(this.dgvQR);
            this.pnlQrSection.Controls.Add(this.pnlQRTitle);

            // pnlQrGap  ← FIX: new 8 px bottom spacer, same pattern as sibling sections
            this.pnlQrGap.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.pnlQrGap.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlQrGap.Height = 8;
            this.pnlQrGap.Name = "pnlQrGap";

            // dgvQR
            this.dgvQR.AllowUserToAddRows = false;
            this.dgvQR.AllowUserToDeleteRows = false;
            this.dgvQR.AllowUserToResizeColumns = false;
            this.dgvQR.AllowUserToResizeRows = false;
            this.dgvQR.AutoGenerateColumns = false;
            this.dgvQR.BackgroundColor = System.Drawing.Color.White;
            this.dgvQR.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvQR.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvQR.ColumnHeadersHeight = 42;
            this.dgvQR.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvQR.GridColor = System.Drawing.Color.FromArgb(230, 230, 238);
            this.dgvQR.Height = 10;
            this.dgvQR.Name = "dgvQR";
            this.dgvQR.RowHeadersVisible = false;
            this.dgvQR.RowTemplate.Height = 38;
            this.dgvQR.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvQR.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.ApplyGridHeaderSettings(this.dgvQR);
            this.dgvQR.SelectionChanged += (s, e) => this.dgvQR.ClearSelection();
            this.dgvQR.DataBindingComplete += (s, e) => AttendanceControl.AutoSizeGrid(this.dgvQR, 360);

            colQRDate.Name = "Date"; colQRDate.HeaderText = "Date"; colQRDate.DataPropertyName = "Date";
            colQRDate.Width = 155; colQRDate.ReadOnly = true;
            colQRDate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;

            colQRCourse.Name = "Course"; colQRCourse.HeaderText = "Course"; colQRCourse.DataPropertyName = "Course";
            colQRCourse.Width = 100; colQRCourse.ReadOnly = true;
            colQRCourse.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;

            colQRSession.Name = "Session"; colQRSession.HeaderText = "Session"; colQRSession.DataPropertyName = "Session";
            colQRSession.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colQRSession.ReadOnly = true;
            colQRSession.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;

            colQRScanTime.Name = "Scan Time"; colQRScanTime.HeaderText = "Scan Time"; colQRScanTime.DataPropertyName = "Scan Time";
            colQRScanTime.Width = 90; colQRScanTime.ReadOnly = true;
            colQRScanTime.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            colQRStatus.Name = "Status"; colQRStatus.HeaderText = "Status"; colQRStatus.DataPropertyName = "Status";
            colQRStatus.Width = 90; colQRStatus.ReadOnly = true;
            colQRStatus.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            this.dgvQR.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
                { colQRDate, colQRCourse, colQRSession, colQRScanTime, colQRStatus });

            // pnlQRTitle
            this.pnlQRTitle.BackColor = System.Drawing.Color.White;
            this.pnlQRTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlQRTitle.Height = 40;
            this.pnlQRTitle.Name = "pnlQRTitle";
            this.pnlQRTitle.Padding = new System.Windows.Forms.Padding(18, 0, 18, 0);
            this.pnlQRTitle.Controls.Add(this.lblQrIcon);
            this.pnlQRTitle.Controls.Add(this.lblQRTitle);
            this.pnlQRTitle.Controls.Add(this.lblQrBadge);
            this.pnlQRTitle.Paint += (s, e) => e.Graphics.DrawLine(
                new System.Drawing.Pen(System.Drawing.Color.FromArgb(225, 225, 235), 1),
                0, this.pnlQRTitle.Height - 1, this.pnlQRTitle.Width, this.pnlQRTitle.Height - 1);

            // lblQrIcon
            this.lblQrIcon.AutoSize = true;
            this.lblQrIcon.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblQrIcon.ForeColor = System.Drawing.Color.FromArgb(128, 0, 0);
            this.lblQrIcon.Location = new System.Drawing.Point(18, 8);
            this.lblQrIcon.Name = "lblQrIcon";
            this.lblQrIcon.Text = "⊞";

            // lblQRTitle
            this.lblQRTitle.AutoSize = true;
            this.lblQRTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblQRTitle.ForeColor = System.Drawing.Color.FromArgb(50, 50, 70);
            this.lblQRTitle.Location = new System.Drawing.Point(38, 10);
            this.lblQRTitle.Name = "lblQRTitle";
            this.lblQRTitle.Text = "QR Attendance History";

            // lblQrBadge
            this.lblQrBadge.AutoSize = true;
            this.lblQrBadge.BackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            this.lblQrBadge.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold);
            this.lblQrBadge.ForeColor = System.Drawing.Color.White;
            this.lblQrBadge.Location = new System.Drawing.Point(200, 9);
            this.lblQrBadge.Name = "lblQrBadge";
            this.lblQrBadge.Padding = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.lblQrBadge.Text = "QR Scan";

            // ──────────────────────────────────────────────────────────────────────
            // pnlMiniStats
            // ──────────────────────────────────────────────────────────────────────
            this.pnlMiniStats.BackColor = System.Drawing.Color.White;
            this.pnlMiniStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMiniStats.Height = 44;
            this.pnlMiniStats.Name = "pnlMiniStats";
            this.pnlMiniStats.Padding = new System.Windows.Forms.Padding(18, 0, 18, 0);
            this.pnlMiniStats.Controls.Add(this.lblMiniPresent);
            this.pnlMiniStats.Controls.Add(this.lblMiniLate);
            this.pnlMiniStats.Controls.Add(this.lblMiniAbsent);
            this.pnlMiniStats.Controls.Add(this.lblMiniExcused);
            this.pnlMiniStats.Controls.Add(this.pnlProgress);
            this.pnlMiniStats.Controls.Add(this.lblProgressPct);
            this.pnlMiniStats.Paint += (s, e) =>
            {
                e.Graphics.DrawLine(new System.Drawing.Pen(System.Drawing.Color.FromArgb(230, 230, 238), 1),
                    0, 0, this.pnlMiniStats.Width, 0);
                e.Graphics.DrawLine(new System.Drawing.Pen(System.Drawing.Color.FromArgb(230, 230, 238), 1),
                    0, this.pnlMiniStats.Height - 1, this.pnlMiniStats.Width, this.pnlMiniStats.Height - 1);
            };

            // lblMiniPresent
            this.lblMiniPresent.AutoSize = true;
            this.lblMiniPresent.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblMiniPresent.ForeColor = System.Drawing.Color.FromArgb(0, 150, 70);
            this.lblMiniPresent.Location = new System.Drawing.Point(18, 13);
            this.lblMiniPresent.Name = "lblMiniPresent";
            this.lblMiniPresent.Text = "● Present: –";

            // lblMiniLate
            this.lblMiniLate.AutoSize = true;
            this.lblMiniLate.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblMiniLate.ForeColor = System.Drawing.Color.FromArgb(200, 120, 0);
            this.lblMiniLate.Location = new System.Drawing.Point(148, 13);
            this.lblMiniLate.Name = "lblMiniLate";
            this.lblMiniLate.Text = "● Late: –";

            // lblMiniAbsent
            this.lblMiniAbsent.AutoSize = true;
            this.lblMiniAbsent.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblMiniAbsent.ForeColor = System.Drawing.Color.FromArgb(200, 40, 40);
            this.lblMiniAbsent.Location = new System.Drawing.Point(278, 13);
            this.lblMiniAbsent.Name = "lblMiniAbsent";
            this.lblMiniAbsent.Text = "● Absent: –";

            // lblMiniExcused
            this.lblMiniExcused.AutoSize = true;
            this.lblMiniExcused.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblMiniExcused.ForeColor = System.Drawing.Color.FromArgb(50, 100, 200);
            this.lblMiniExcused.Location = new System.Drawing.Point(408, 13);
            this.lblMiniExcused.Name = "lblMiniExcused";
            this.lblMiniExcused.Text = "● Excused: –";

            // pnlProgress
            this.pnlProgress.BackColor = System.Drawing.Color.FromArgb(240, 240, 245);
            this.pnlProgress.Location = new System.Drawing.Point(558, 13);
            this.pnlProgress.Name = "pnlProgress";
            this.pnlProgress.Size = new System.Drawing.Size(240, 18);
            this.pnlProgress.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawSegmentedProgress);

            // lblProgressPct
            this.lblProgressPct.AutoSize = true;
            this.lblProgressPct.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblProgressPct.ForeColor = System.Drawing.Color.FromArgb(80, 80, 100);
            this.lblProgressPct.Location = new System.Drawing.Point(808, 13);
            this.lblProgressPct.Name = "lblProgressPct";
            this.lblProgressPct.Text = "0% overall";

            // ──────────────────────────────────────────────────────────────────────
            // tlpCards  (5-column summary card row)
            // ──────────────────────────────────────────────────────────────────────
            this.tlpCards.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.tlpCards.ColumnCount = 5;
            this.tlpCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpCards.Height = 138;
            this.tlpCards.Name = "tlpCards";
            this.tlpCards.Padding = new System.Windows.Forms.Padding(12, 10, 12, 4);
            this.tlpCards.RowCount = 1;
            this.tlpCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpCards.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCards.Controls.Add(this.pnlCardOverall, 0, 0);
            this.tlpCards.Controls.Add(this.pnlCardPresent, 1, 0);
            this.tlpCards.Controls.Add(this.pnlCardLate, 2, 0);
            this.tlpCards.Controls.Add(this.pnlCardAbsent, 3, 0);
            this.tlpCards.Controls.Add(this.pnlCardAlerts, 4, 0);

            // ── Card: Overall ─────────────────────────────────────────────────────
            this.pnlCardOverall.BackColor = System.Drawing.Color.White;
            this.pnlCardOverall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCardOverall.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pnlCardOverall.Name = "pnlCardOverall";
            this.pnlCardOverall.Controls.Add(this.lblOverallPct);
            this.pnlCardOverall.Controls.Add(this.lblOverallTitle);
            this.pnlCardOverall.Controls.Add(this.pnlBarOverall);
            this.pnlCardOverall.Paint += (s, e) => AttendanceControl.DrawCardBorder(e.Graphics, this.pnlCardOverall, System.Drawing.Color.FromArgb(128, 0, 0));
            this.pnlCardOverall.Resize += (s, e) => this.pnlBarOverall.Height = this.pnlCardOverall.Height;

            this.pnlBarOverall.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.pnlBarOverall.BackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            this.pnlBarOverall.Location = new System.Drawing.Point(0, 0);
            this.pnlBarOverall.Name = "pnlBarOverall";
            this.pnlBarOverall.Size = new System.Drawing.Size(4, 124);

            this.lblOverallTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblOverallTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblOverallTitle.ForeColor = System.Drawing.Color.FromArgb(100, 100, 120);
            this.lblOverallTitle.Height = 30;
            this.lblOverallTitle.Name = "lblOverallTitle";
            this.lblOverallTitle.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.lblOverallTitle.Text = "Overall";
            this.lblOverallTitle.TextAlign = System.Drawing.ContentAlignment.BottomLeft;

            this.lblOverallPct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOverallPct.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblOverallPct.ForeColor = System.Drawing.Color.FromArgb(128, 0, 0);
            this.lblOverallPct.Name = "lblOverallPct";
            this.lblOverallPct.Padding = new System.Windows.Forms.Padding(12, 0, 0, 8);
            this.lblOverallPct.Text = "–";
            this.lblOverallPct.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // ── Card: Present ─────────────────────────────────────────────────────
            this.pnlCardPresent.BackColor = System.Drawing.Color.White;
            this.pnlCardPresent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCardPresent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pnlCardPresent.Name = "pnlCardPresent";
            this.pnlCardPresent.Controls.Add(this.lblPresentValue);
            this.pnlCardPresent.Controls.Add(this.lblPresentTitle);
            this.pnlCardPresent.Controls.Add(this.pnlBarPresent);
            this.pnlCardPresent.Paint += (s, e) => AttendanceControl.DrawCardBorder(e.Graphics, this.pnlCardPresent, System.Drawing.Color.FromArgb(0, 150, 70));
            this.pnlCardPresent.Resize += (s, e) => this.pnlBarPresent.Height = this.pnlCardPresent.Height;

            this.pnlBarPresent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.pnlBarPresent.BackColor = System.Drawing.Color.FromArgb(0, 150, 70);
            this.pnlBarPresent.Location = new System.Drawing.Point(0, 0);
            this.pnlBarPresent.Name = "pnlBarPresent";
            this.pnlBarPresent.Size = new System.Drawing.Size(4, 124);

            this.lblPresentTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPresentTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPresentTitle.ForeColor = System.Drawing.Color.FromArgb(100, 100, 120);
            this.lblPresentTitle.Height = 30;
            this.lblPresentTitle.Name = "lblPresentTitle";
            this.lblPresentTitle.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.lblPresentTitle.Text = "Present";
            this.lblPresentTitle.TextAlign = System.Drawing.ContentAlignment.BottomLeft;

            this.lblPresentValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPresentValue.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblPresentValue.ForeColor = System.Drawing.Color.FromArgb(0, 150, 70);
            this.lblPresentValue.Name = "lblPresentValue";
            this.lblPresentValue.Padding = new System.Windows.Forms.Padding(12, 0, 0, 8);
            this.lblPresentValue.Text = "–";
            this.lblPresentValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // ── Card: Late ────────────────────────────────────────────────────────
            this.pnlCardLate.BackColor = System.Drawing.Color.White;
            this.pnlCardLate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCardLate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pnlCardLate.Name = "pnlCardLate";
            this.pnlCardLate.Controls.Add(this.lblLateValue);
            this.pnlCardLate.Controls.Add(this.lblLateTitle);
            this.pnlCardLate.Controls.Add(this.pnlBarLate);
            this.pnlCardLate.Paint += (s, e) => AttendanceControl.DrawCardBorder(e.Graphics, this.pnlCardLate, System.Drawing.Color.FromArgb(220, 140, 0));
            this.pnlCardLate.Resize += (s, e) => this.pnlBarLate.Height = this.pnlCardLate.Height;

            this.pnlBarLate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.pnlBarLate.BackColor = System.Drawing.Color.FromArgb(220, 140, 0);
            this.pnlBarLate.Location = new System.Drawing.Point(0, 0);
            this.pnlBarLate.Name = "pnlBarLate";
            this.pnlBarLate.Size = new System.Drawing.Size(4, 124);

            this.lblLateTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLateTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblLateTitle.ForeColor = System.Drawing.Color.FromArgb(100, 100, 120);
            this.lblLateTitle.Height = 30;
            this.lblLateTitle.Name = "lblLateTitle";
            this.lblLateTitle.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.lblLateTitle.Text = "Late";
            this.lblLateTitle.TextAlign = System.Drawing.ContentAlignment.BottomLeft;

            this.lblLateValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLateValue.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblLateValue.ForeColor = System.Drawing.Color.FromArgb(220, 140, 0);
            this.lblLateValue.Name = "lblLateValue";
            this.lblLateValue.Padding = new System.Windows.Forms.Padding(12, 0, 0, 8);
            this.lblLateValue.Text = "–";
            this.lblLateValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // ── Card: Absent ──────────────────────────────────────────────────────
            this.pnlCardAbsent.BackColor = System.Drawing.Color.White;
            this.pnlCardAbsent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCardAbsent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pnlCardAbsent.Name = "pnlCardAbsent";
            this.pnlCardAbsent.Controls.Add(this.lblAbsentValue);
            this.pnlCardAbsent.Controls.Add(this.lblAbsentTitle);
            this.pnlCardAbsent.Controls.Add(this.pnlBarAbsent);
            this.pnlCardAbsent.Paint += (s, e) => AttendanceControl.DrawCardBorder(e.Graphics, this.pnlCardAbsent, System.Drawing.Color.FromArgb(200, 40, 40));
            this.pnlCardAbsent.Resize += (s, e) => this.pnlBarAbsent.Height = this.pnlCardAbsent.Height;

            this.pnlBarAbsent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.pnlBarAbsent.BackColor = System.Drawing.Color.FromArgb(200, 40, 40);
            this.pnlBarAbsent.Location = new System.Drawing.Point(0, 0);
            this.pnlBarAbsent.Name = "pnlBarAbsent";
            this.pnlBarAbsent.Size = new System.Drawing.Size(4, 124);

            this.lblAbsentTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAbsentTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblAbsentTitle.ForeColor = System.Drawing.Color.FromArgb(100, 100, 120);
            this.lblAbsentTitle.Height = 30;
            this.lblAbsentTitle.Name = "lblAbsentTitle";
            this.lblAbsentTitle.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.lblAbsentTitle.Text = "Absent";
            this.lblAbsentTitle.TextAlign = System.Drawing.ContentAlignment.BottomLeft;

            this.lblAbsentValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAbsentValue.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblAbsentValue.ForeColor = System.Drawing.Color.FromArgb(200, 40, 40);
            this.lblAbsentValue.Name = "lblAbsentValue";
            this.lblAbsentValue.Padding = new System.Windows.Forms.Padding(12, 0, 0, 8);
            this.lblAbsentValue.Text = "–";
            this.lblAbsentValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // ── Card: Alerts ──────────────────────────────────────────────────────
            this.pnlCardAlerts.BackColor = System.Drawing.Color.FromArgb(255, 243, 243);
            this.pnlCardAlerts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCardAlerts.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pnlCardAlerts.Name = "pnlCardAlerts";
            this.pnlCardAlerts.Controls.Add(this.lblAlertText);
            this.pnlCardAlerts.Controls.Add(this.btnViewDetails);
            this.pnlCardAlerts.Controls.Add(this.pnlBarAlerts);
            this.pnlCardAlerts.Paint += (s, e) => AttendanceControl.DrawCardBorder(e.Graphics, this.pnlCardAlerts, System.Drawing.Color.FromArgb(200, 40, 40));
            this.pnlCardAlerts.Resize += (s, e) => this.pnlBarAlerts.Height = this.pnlCardAlerts.Height;

            this.pnlBarAlerts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.pnlBarAlerts.BackColor = System.Drawing.Color.FromArgb(200, 40, 40);
            this.pnlBarAlerts.Location = new System.Drawing.Point(0, 0);
            this.pnlBarAlerts.Name = "pnlBarAlerts";
            this.pnlBarAlerts.Size = new System.Drawing.Size(4, 124);

            this.lblAlertText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAlertText.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblAlertText.ForeColor = System.Drawing.Color.FromArgb(160, 40, 40);
            this.lblAlertText.Name = "lblAlertText";
            this.lblAlertText.Padding = new System.Windows.Forms.Padding(10, 0, 10, 24);
            this.lblAlertText.Text = "Loading…";
            this.lblAlertText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.btnViewDetails.BackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            this.btnViewDetails.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnViewDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnViewDetails.FlatAppearance.BorderSize = 0;
            this.btnViewDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewDetails.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnViewDetails.ForeColor = System.Drawing.Color.White;
            this.btnViewDetails.Height = 28;
            this.btnViewDetails.Margin = new System.Windows.Forms.Padding(16, 0, 16, 8);
            this.btnViewDetails.Name = "btnViewDetails";
            this.btnViewDetails.Text = "View Details";
            this.btnViewDetails.Click += new System.EventHandler(this.OnViewDetailsClick);

            // ──────────────────────────────────────────────────────────────────────
            // pnlHeader  (filter bar)
            // ──────────────────────────────────────────────────────────────────────
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 110;
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Controls.Add(this.lblPageTitle);
            this.pnlHeader.Controls.Add(this.lblHeaderSubtitle);
            this.pnlHeader.Controls.Add(this.lblPeriodLbl);
            this.pnlHeader.Controls.Add(this.cmbPeriod);
            this.pnlHeader.Controls.Add(this.lblCourseLbl);
            this.pnlHeader.Controls.Add(this.cmbCourse);
            this.pnlHeader.Controls.Add(this.lblFromLbl);
            this.pnlHeader.Controls.Add(this.dtpFrom);
            this.pnlHeader.Controls.Add(this.lblToLbl);
            this.pnlHeader.Controls.Add(this.dtpTo);
            this.pnlHeader.Controls.Add(this.btnRefresh);
            this.pnlHeader.Controls.Add(this.btnScanQR);
            this.pnlHeader.Paint += (s, e) => e.Graphics.DrawLine(
                new System.Drawing.Pen(System.Drawing.Color.FromArgb(225, 225, 235), 1),
                0, this.pnlHeader.Height - 1, this.pnlHeader.Width, this.pnlHeader.Height - 1);

            // lblPageTitle
            this.lblPageTitle.AutoSize = true;
            this.lblPageTitle.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Bold);
            this.lblPageTitle.ForeColor = System.Drawing.Color.FromArgb(128, 0, 0);
            this.lblPageTitle.Location = new System.Drawing.Point(18, 8);
            this.lblPageTitle.Name = "lblPageTitle";
            this.lblPageTitle.Text = "My Attendance";

            // lblHeaderSubtitle
            this.lblHeaderSubtitle.AutoSize = true;
            this.lblHeaderSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHeaderSubtitle.ForeColor = System.Drawing.Color.FromArgb(120, 120, 140);
            this.lblHeaderSubtitle.Location = new System.Drawing.Point(19, 36);
            this.lblHeaderSubtitle.Name = "lblHeaderSubtitle";
            this.lblHeaderSubtitle.Text = "Track your academic attendance across all courses";

            // lblPeriodLbl
            this.lblPeriodLbl.AutoSize = true;
            this.lblPeriodLbl.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPeriodLbl.ForeColor = System.Drawing.Color.FromArgb(80, 80, 100);
            this.lblPeriodLbl.Location = new System.Drawing.Point(18, 66);
            this.lblPeriodLbl.Name = "lblPeriodLbl";
            this.lblPeriodLbl.Text = "Period:";

            // cmbPeriod
            this.cmbPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPeriod.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbPeriod.Location = new System.Drawing.Point(70, 62);
            this.cmbPeriod.Name = "cmbPeriod";
            this.cmbPeriod.Size = new System.Drawing.Size(140, 23);
            this.cmbPeriod.Items.AddRange(new object[] { "All Periods", "Prelim", "Midterm", "Final Term" });
            this.cmbPeriod.SelectedIndex = 0;
            this.cmbPeriod.SelectedIndexChanged += (s, e) => this.RefreshAll();

            // lblCourseLbl
            this.lblCourseLbl.AutoSize = true;
            this.lblCourseLbl.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblCourseLbl.ForeColor = System.Drawing.Color.FromArgb(80, 80, 100);
            this.lblCourseLbl.Location = new System.Drawing.Point(228, 66);
            this.lblCourseLbl.Name = "lblCourseLbl";
            this.lblCourseLbl.Text = "Course:";

            // cmbCourse
            this.cmbCourse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCourse.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbCourse.Location = new System.Drawing.Point(286, 62);
            this.cmbCourse.Name = "cmbCourse";
            this.cmbCourse.Size = new System.Drawing.Size(165, 23);
            this.cmbCourse.Items.AddRange(new object[] { "All Courses" });
            this.cmbCourse.SelectedIndex = 0;
            this.cmbCourse.SelectedIndexChanged += (s, e) => this.RefreshAll();

            // lblFromLbl
            this.lblFromLbl.AutoSize = true;
            this.lblFromLbl.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblFromLbl.ForeColor = System.Drawing.Color.FromArgb(80, 80, 100);
            this.lblFromLbl.Location = new System.Drawing.Point(470, 66);
            this.lblFromLbl.Name = "lblFromLbl";
            this.lblFromLbl.Text = "From:";

            // dtpFrom
            this.dtpFrom.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(512, 62);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(122, 23);
            this.dtpFrom.Value = new System.DateTime(2026, 2, 1);
            this.dtpFrom.ValueChanged += (s, e) => this.RefreshAll();

            // lblToLbl
            this.lblToLbl.AutoSize = true;
            this.lblToLbl.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblToLbl.ForeColor = System.Drawing.Color.FromArgb(80, 80, 100);
            this.lblToLbl.Location = new System.Drawing.Point(646, 66);
            this.lblToLbl.Name = "lblToLbl";
            this.lblToLbl.Text = "To:";

            // dtpTo
            this.dtpTo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(670, 62);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(122, 23);
            this.dtpTo.Value = System.DateTime.Today;
            this.dtpTo.ValueChanged += (s, e) => this.RefreshAll();

            // btnRefresh
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(808, 61);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 27);
            this.btnRefresh.Text = "↺  Refresh";
            this.btnRefresh.Click += (s, e) => this.RefreshAll();

            // btnScanQR
            this.btnScanQR.BackColor = System.Drawing.Color.FromArgb(0, 120, 80);
            this.btnScanQR.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnScanQR.FlatAppearance.BorderSize = 0;
            this.btnScanQR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScanQR.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnScanQR.ForeColor = System.Drawing.Color.White;
            this.btnScanQR.Location = new System.Drawing.Point(920, 61);
            this.btnScanQR.Name = "btnScanQR";
            this.btnScanQR.Size = new System.Drawing.Size(140, 27);
            this.btnScanQR.Text = "⊞  Scan / Upload QR";
            this.btnScanQR.UseVisualStyleBackColor = false;
            this.btnScanQR.Click += new System.EventHandler(this.BtnScanQR_Click);

            // ──────────────────────────────────────────────────────────────────────
            // Resume layouts
            // ──────────────────────────────────────────────────────────────────────
            this.pnlScrollWrapper.ResumeLayout(false);
            this.pnlScrollWrapper.PerformLayout();
            this.pnlLogsSection.ResumeLayout(false);
            this.pnlLogsSection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).EndInit();
            this.pnlLogsTitleRow.ResumeLayout(false);
            this.pnlLogsTitleRow.PerformLayout();
            this.pnlSubjSection.ResumeLayout(false);
            this.pnlSubjSection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubjects)).EndInit();
            this.pnlSubjTitleRow.ResumeLayout(false);
            this.pnlSubjTitleRow.PerformLayout();
            this.pnlQrSection.ResumeLayout(false);
            this.pnlQrSection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQR)).EndInit();
            this.pnlQRTitle.ResumeLayout(false);
            this.pnlQRTitle.PerformLayout();
            this.pnlMiniStats.ResumeLayout(false);
            this.pnlMiniStats.PerformLayout();
            this.tlpCards.ResumeLayout(false);
            this.pnlCardOverall.ResumeLayout(false);
            this.pnlCardPresent.ResumeLayout(false);
            this.pnlCardLate.ResumeLayout(false);
            this.pnlCardAbsent.ResumeLayout(false);
            this.pnlCardAlerts.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        /// <summary>
        /// Applies the standardized custom palette header layout settings uniformly across data grids.
        /// </summary>
        private void ApplyGridHeaderSettings(System.Windows.Forms.DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            dgv.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dgv.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(40, 40, 60);
            dgv.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(245, 220, 220);
            dgv.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(40, 40, 60);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(251, 251, 254);
        }

        // ── Field declarations ────────────────────────────────────────────────────

        // Scroll wrapper
        private System.Windows.Forms.Panel pnlScrollWrapper;
        private System.Windows.Forms.Panel pnlSpacer;

        // Header / filter bar
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Label lblHeaderSubtitle;
        private System.Windows.Forms.Label lblPeriodLbl;
        private System.Windows.Forms.ComboBox cmbPeriod;
        private System.Windows.Forms.Label lblCourseLbl;
        private System.Windows.Forms.ComboBox cmbCourse;
        private System.Windows.Forms.Label lblFromLbl;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblToLbl;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnScanQR;

        // Summary cards
        private System.Windows.Forms.TableLayoutPanel tlpCards;
        private System.Windows.Forms.Panel pnlCardOverall;
        private System.Windows.Forms.Panel pnlBarOverall;
        private System.Windows.Forms.Label lblOverallTitle;
        private System.Windows.Forms.Label lblOverallPct;
        private System.Windows.Forms.Panel pnlCardPresent;
        private System.Windows.Forms.Panel pnlBarPresent;
        private System.Windows.Forms.Label lblPresentTitle;
        private System.Windows.Forms.Label lblPresentValue;
        private System.Windows.Forms.Panel pnlCardLate;
        private System.Windows.Forms.Panel pnlBarLate;
        private System.Windows.Forms.Label lblLateTitle;
        private System.Windows.Forms.Label lblLateValue;
        private System.Windows.Forms.Panel pnlCardAbsent;
        private System.Windows.Forms.Panel pnlBarAbsent;
        private System.Windows.Forms.Label lblAbsentTitle;
        private System.Windows.Forms.Label lblAbsentValue;
        private System.Windows.Forms.Panel pnlCardAlerts;
        private System.Windows.Forms.Panel pnlBarAlerts;
        private System.Windows.Forms.Label lblAlertText;
        private System.Windows.Forms.Button btnViewDetails;

        // Mini stats / progress bar
        private System.Windows.Forms.Panel pnlMiniStats;
        private System.Windows.Forms.Label lblMiniPresent;
        private System.Windows.Forms.Label lblMiniLate;
        private System.Windows.Forms.Label lblMiniAbsent;
        private System.Windows.Forms.Label lblMiniExcused;
        private System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.Label lblProgressPct;

        // QR Attendance History section  ← pnlQrGap is new
        private System.Windows.Forms.Panel pnlQrSection;
        private System.Windows.Forms.Panel pnlQrGap;        // ← FIX: new field
        private System.Windows.Forms.Panel pnlQRTitle;
        private System.Windows.Forms.Label lblQrIcon;
        private System.Windows.Forms.Label lblQRTitle;
        private System.Windows.Forms.Label lblQrBadge;
        private System.Windows.Forms.DataGridView dgvQR;

        // Subjects per course section
        private System.Windows.Forms.Panel pnlSubjSection;
        private System.Windows.Forms.Panel pnlSubjTitleRow;
        private System.Windows.Forms.Label lblSubjectsTitle;
        private System.Windows.Forms.Label lblSubjectsHint;
        private System.Windows.Forms.Panel pnlSubjGap;
        private System.Windows.Forms.DataGridView dgvSubjects;

        // Attendance log section
        private System.Windows.Forms.Panel pnlLogsSection;
        private System.Windows.Forms.Panel pnlLogsTitleRow;
        private System.Windows.Forms.Label lblAttendanceLogTitle;
        private System.Windows.Forms.Panel pnlLogsGap;
        private System.Windows.Forms.DataGridView dgvLogs;
    }
}