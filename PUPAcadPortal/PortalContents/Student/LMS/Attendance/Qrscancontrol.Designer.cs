namespace PUPAcadPortal.PortalContents.Student.LMS.Attendance
{
    partial class QRScanControl
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
            this.scrollPanel = new System.Windows.Forms.Panel();
            this.historyPanel = new System.Windows.Forms.Panel();
            this.dgvHistory = new System.Windows.Forms.DataGridView();
            this.historyTitleBar = new System.Windows.Forms.Panel();
            this.lblHistoryTitle = new System.Windows.Forms.Label();
            this.historyGap = new System.Windows.Forms.Panel();
            this.pnlResult = new System.Windows.Forms.Panel();
            this.pnlResultInner = new System.Windows.Forms.Panel();
            this.lblRawPayload = new System.Windows.Forms.Label();
            this.lblResultDetail = new System.Windows.Forms.Label();
            this.lblResultCourse = new System.Windows.Forms.Label();
            this.lblResultStatus = new System.Windows.Forms.Label();
            this.pnlResultAccent = new System.Windows.Forms.Panel();
            this.mainAreaPanel = new System.Windows.Forms.Panel();
            this.pnlUpload = new System.Windows.Forms.Panel();
            this.picQR = new System.Windows.Forms.PictureBox();
            this.pnlCamera = new System.Windows.Forms.Panel();
            this.lblCameraStatus = new System.Windows.Forms.Label();
            this.picCamera = new System.Windows.Forms.PictureBox();
            this.pnlInfoSide = new System.Windows.Forms.Panel();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnClearImage = new System.Windows.Forms.Button();
            this.cmbCamera = new System.Windows.Forms.ComboBox();
            this.btnStartCamera = new System.Windows.Forms.Button();
            this.btnStopCamera = new System.Windows.Forms.Button();
            this.togglePanel = new System.Windows.Forms.Panel();
            this.pillPanel = new System.Windows.Forms.Panel();
            this.btnModeCamera = new System.Windows.Forms.Button();
            this.btnModeUpload = new System.Windows.Forms.Button();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblHeaderSub = new System.Windows.Forms.Label();
            this.lblPageTitle = new System.Windows.Forms.Label();
            this.lblHeaderIcon = new System.Windows.Forms.Label();
            this.topSpacer = new System.Windows.Forms.Panel();

            this.scrollPanel.SuspendLayout();
            this.historyPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
            this.historyTitleBar.SuspendLayout();
            this.pnlResult.SuspendLayout();
            this.pnlResultInner.SuspendLayout();
            this.mainAreaPanel.SuspendLayout();
            this.pnlUpload.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picQR)).BeginInit();
            this.pnlCamera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCamera)).BeginInit();
            this.togglePanel.SuspendLayout();
            this.pillPanel.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.SuspendLayout();

            // 
            // scrollPanel
            // 
            this.scrollPanel.AutoScroll = true;
            this.scrollPanel.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.scrollPanel.Controls.Add(this.historyPanel);
            this.scrollPanel.Controls.Add(this.pnlResult);
            this.scrollPanel.Controls.Add(this.mainAreaPanel);
            this.scrollPanel.Controls.Add(this.togglePanel);
            this.scrollPanel.Controls.Add(this.headerPanel);
            this.scrollPanel.Controls.Add(this.topSpacer);
            this.scrollPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scrollPanel.Location = new System.Drawing.Point(0, 0);
            this.scrollPanel.Name = "scrollPanel";
            this.scrollPanel.Size = new System.Drawing.Size(900, 800);
            this.scrollPanel.TabIndex = 0;

            // 
            // topSpacer
            // 
            this.topSpacer.BackColor = System.Drawing.Color.Transparent;
            this.topSpacer.Dock = System.Windows.Forms.DockStyle.Top;
            this.topSpacer.Height = 28;
            this.topSpacer.Name = "topSpacer";

            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.White;
            this.headerPanel.Controls.Add(this.lblHeaderSub);
            this.headerPanel.Controls.Add(this.lblPageTitle);
            this.headerPanel.Controls.Add(this.lblHeaderIcon);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 28);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(900, 68);
            this.headerPanel.TabIndex = 1;
            this.headerPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.Header_Paint);

            // 
            // lblHeaderIcon
            // 
            this.lblHeaderIcon.AutoSize = true;
            this.lblHeaderIcon.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.lblHeaderIcon.ForeColor = System.Drawing.Color.FromArgb(128, 0, 0);
            this.lblHeaderIcon.Location = new System.Drawing.Point(18, 14);
            this.lblHeaderIcon.Name = "lblHeaderIcon";
            this.lblHeaderIcon.Text = "⊞";

            // 
            // lblPageTitle
            // 
            this.lblPageTitle.AutoSize = true;
            this.lblPageTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblPageTitle.ForeColor = System.Drawing.Color.FromArgb(128, 0, 0);
            this.lblPageTitle.Location = new System.Drawing.Point(52, 10);
            this.lblPageTitle.Name = "lblPageTitle";
            this.lblPageTitle.Text = "QR Attendance";

            // 
            // lblHeaderSub
            // 
            this.lblHeaderSub.AutoSize = true;
            this.lblHeaderSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHeaderSub.ForeColor = System.Drawing.Color.FromArgb(120, 120, 140);
            this.lblHeaderSub.Location = new System.Drawing.Point(53, 38);
            this.lblHeaderSub.Name = "lblHeaderSub";
            this.lblHeaderSub.Text = "Scan or upload a QR code to record your attendance";

            // 
            // togglePanel
            // 
            this.togglePanel.BackColor = System.Drawing.Color.White;
            this.togglePanel.Controls.Add(this.pillPanel);
            this.togglePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.togglePanel.Location = new System.Drawing.Point(0, 96);
            this.togglePanel.Name = "togglePanel";
            this.togglePanel.Padding = new System.Windows.Forms.Padding(18, 10, 18, 6);
            this.togglePanel.Size = new System.Drawing.Size(900, 52);
            this.togglePanel.TabIndex = 2;
            this.togglePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.Toggle_Paint);

            // 
            // pillPanel
            // 
            this.pillPanel.BackColor = System.Drawing.Color.FromArgb(240, 240, 245);
            this.pillPanel.Controls.Add(this.btnModeCamera);
            this.pillPanel.Controls.Add(this.btnModeUpload);
            this.pillPanel.Location = new System.Drawing.Point(18, 10);
            this.pillPanel.Name = "pillPanel";
            this.pillPanel.Size = new System.Drawing.Size(280, 32);
            this.pillPanel.TabIndex = 0;
            this.pillPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.Pill_Paint);

            // 
            // btnModeUpload
            // 
            this.btnModeUpload.BackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            this.btnModeUpload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnModeUpload.FlatAppearance.BorderSize = 0;
            this.btnModeUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModeUpload.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnModeUpload.ForeColor = System.Drawing.Color.White;
            this.btnModeUpload.Location = new System.Drawing.Point(2, 2);
            this.btnModeUpload.Name = "btnModeUpload";
            this.btnModeUpload.Size = new System.Drawing.Size(136, 28);
            this.btnModeUpload.TabIndex = 0;
            this.btnModeUpload.Text = "📁  Upload Image";
            this.btnModeUpload.UseVisualStyleBackColor = false;
            this.btnModeUpload.Click += new System.EventHandler(this.BtnModeUpload_Click);

            // 
            // btnModeCamera
            // 
            this.btnModeCamera.BackColor = System.Drawing.Color.Transparent;
            this.btnModeCamera.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnModeCamera.FlatAppearance.BorderSize = 0;
            this.btnModeCamera.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModeCamera.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnModeCamera.ForeColor = System.Drawing.Color.FromArgb(80, 80, 100);
            this.btnModeCamera.Location = new System.Drawing.Point(140, 2);
            this.btnModeCamera.Name = "btnModeCamera";
            this.btnModeCamera.Size = new System.Drawing.Size(136, 28);
            this.btnModeCamera.TabIndex = 1;
            this.btnModeCamera.Text = "📷  Live Camera";
            this.btnModeCamera.UseVisualStyleBackColor = false;
            this.btnModeCamera.Click += new System.EventHandler(this.BtnModeCamera_Click);

            // 
            // mainAreaPanel
            // 
            this.mainAreaPanel.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.mainAreaPanel.Controls.Add(this.pnlUpload);
            this.mainAreaPanel.Controls.Add(this.pnlCamera);
            this.mainAreaPanel.Controls.Add(this.pnlInfoSide);
            this.mainAreaPanel.Controls.Add(this.btnUpload);
            this.mainAreaPanel.Controls.Add(this.btnClearImage);
            this.mainAreaPanel.Controls.Add(this.cmbCamera);
            this.mainAreaPanel.Controls.Add(this.btnStartCamera);
            this.mainAreaPanel.Controls.Add(this.btnStopCamera);
            this.mainAreaPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.mainAreaPanel.Location = new System.Drawing.Point(0, 148);
            this.mainAreaPanel.Name = "mainAreaPanel";
            this.mainAreaPanel.Padding = new System.Windows.Forms.Padding(18, 14, 18, 0);
            this.mainAreaPanel.Size = new System.Drawing.Size(900, 420);
            this.mainAreaPanel.TabIndex = 3;
            this.mainAreaPanel.Resize += new System.EventHandler(this.MainAreaPanel_Resize);

            // 
            // pnlUpload
            // 
            this.pnlUpload.AllowDrop = true;
            this.pnlUpload.BackColor = System.Drawing.Color.White;
            this.pnlUpload.Controls.Add(this.picQR);
            this.pnlUpload.Location = new System.Drawing.Point(18, 14);
            this.pnlUpload.Name = "pnlUpload";
            this.pnlUpload.Size = new System.Drawing.Size(400, 392);
            this.pnlUpload.TabIndex = 0;
            this.pnlUpload.DragDrop += new System.Windows.Forms.DragEventHandler(this.PnlUpload_DragDrop);
            this.pnlUpload.DragEnter += new System.Windows.Forms.DragEventHandler(this.PnlUpload_DragEnter);
            this.pnlUpload.Paint += new System.Windows.Forms.PaintEventHandler(this.PnlUpload_Paint);
            // 
            // picQR
            // 
            this.picQR.BackColor = System.Drawing.Color.Transparent;
            this.picQR.Location = new System.Drawing.Point(8, 8);
            this.picQR.Name = "picQR";
            this.picQR.Size = new System.Drawing.Size(384, 376);
            this.picQR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picQR.TabIndex = 0;
            this.picQR.TabStop = false;
            this.picQR.Visible = false;

            // 
            // pnlCamera
            // 
            this.pnlCamera.BackColor = System.Drawing.Color.Black;
            this.pnlCamera.Controls.Add(this.lblCameraStatus);
            this.pnlCamera.Controls.Add(this.picCamera);
            this.pnlCamera.Location = new System.Drawing.Point(18, 14);
            this.pnlCamera.Name = "pnlCamera";
            this.pnlCamera.Size = new System.Drawing.Size(400, 392);
            this.pnlCamera.TabIndex = 1;
            this.pnlCamera.Visible = false;

            // 
            // lblCameraStatus
            // 
            this.lblCameraStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCameraStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCameraStatus.ForeColor = System.Drawing.Color.White;
            this.lblCameraStatus.Location = new System.Drawing.Point(0, 0);
            this.lblCameraStatus.Name = "lblCameraStatus";
            this.lblCameraStatus.Size = new System.Drawing.Size(400, 392);
            this.lblCameraStatus.TabIndex = 1;
            this.lblCameraStatus.Text = "Camera not started";
            this.lblCameraStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // picCamera
            // 
            this.picCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picCamera.Location = new System.Drawing.Point(0, 0);
            this.picCamera.Name = "picCamera";
            this.picCamera.Size = new System.Drawing.Size(400, 392);
            this.picCamera.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picCamera.TabIndex = 0;
            this.picCamera.TabStop = false;

            // 
            // pnlInfoSide
            // 
            this.pnlInfoSide.BackColor = System.Drawing.Color.White;
            this.pnlInfoSide.Location = new System.Drawing.Point(582, 14);
            this.pnlInfoSide.Name = "pnlInfoSide";
            this.pnlInfoSide.Padding = new System.Windows.Forms.Padding(14);
            this.pnlInfoSide.Size = new System.Drawing.Size(300, 392);
            this.pnlInfoSide.TabIndex = 2;
            this.pnlInfoSide.Paint += new System.Windows.Forms.PaintEventHandler(this.PnlInfoSide_Paint);

            // Buttons & Controls Below Main Area
            // 
            this.btnUpload.BackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            this.btnUpload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpload.FlatAppearance.BorderSize = 0;
            this.btnUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpload.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnUpload.ForeColor = System.Drawing.Color.White;
            this.btnUpload.Location = new System.Drawing.Point(18, 416);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(200, 34);
            this.btnUpload.TabIndex = 3;
            this.btnUpload.Text = "Browse / Upload QR Image";
            this.btnUpload.UseVisualStyleBackColor = false;
            this.btnUpload.Click += new System.EventHandler(this.BtnUpload_Click);

            this.btnClearImage.BackColor = System.Drawing.Color.White;
            this.btnClearImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearImage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 215);
            this.btnClearImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearImage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnClearImage.ForeColor = System.Drawing.Color.FromArgb(80, 80, 100);
            this.btnClearImage.Location = new System.Drawing.Point(228, 416);
            this.btnClearImage.Name = "btnClearImage";
            this.btnClearImage.Size = new System.Drawing.Size(90, 34);
            this.btnClearImage.TabIndex = 4;
            this.btnClearImage.Text = "✕  Clear";
            this.btnClearImage.UseVisualStyleBackColor = false;
            this.btnClearImage.Visible = false;
            this.btnClearImage.Click += new System.EventHandler(this.BtnClearImage_Click);

            this.cmbCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCamera.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbCamera.Location = new System.Drawing.Point(18, 416);
            this.cmbCamera.Name = "cmbCamera";
            this.cmbCamera.Size = new System.Drawing.Size(200, 28);
            this.cmbCamera.TabIndex = 5;
            this.cmbCamera.Visible = false;

            this.btnStartCamera.BackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            this.btnStartCamera.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStartCamera.FlatAppearance.BorderSize = 0;
            this.btnStartCamera.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartCamera.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStartCamera.ForeColor = System.Drawing.Color.White;
            this.btnStartCamera.Location = new System.Drawing.Point(228, 416);
            this.btnStartCamera.Name = "btnStartCamera";
            this.btnStartCamera.Size = new System.Drawing.Size(130, 34);
            this.btnStartCamera.TabIndex = 6;
            this.btnStartCamera.Text = "▶  Start Camera";
            this.btnStartCamera.UseVisualStyleBackColor = false;
            this.btnStartCamera.Visible = false;
            this.btnStartCamera.Click += new System.EventHandler(this.BtnStartCamera_Click);

            this.btnStopCamera.BackColor = System.Drawing.Color.White;
            this.btnStopCamera.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStopCamera.Enabled = false;
            this.btnStopCamera.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 215);
            this.btnStopCamera.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopCamera.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnStopCamera.ForeColor = System.Drawing.Color.FromArgb(80, 80, 100);
            this.btnStopCamera.Location = new System.Drawing.Point(368, 416);
            this.btnStopCamera.Name = "btnStopCamera";
            this.btnStopCamera.Size = new System.Drawing.Size(80, 34);
            this.btnStopCamera.TabIndex = 7;
            this.btnStopCamera.Text = "■  Stop";
            this.btnStopCamera.UseVisualStyleBackColor = false;
            this.btnStopCamera.Visible = false;
            this.btnStopCamera.Click += new System.EventHandler(this.BtnStopCamera_Click);

            // 
            // pnlResult
            // 
            this.pnlResult.BackColor = System.Drawing.Color.White;
            this.pnlResult.Controls.Add(this.pnlResultInner);
            this.pnlResult.Controls.Add(this.pnlResultAccent);
            this.pnlResult.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlResult.Location = new System.Drawing.Point(0, 568);
            this.pnlResult.Name = "pnlResult";
            this.pnlResult.Size = new System.Drawing.Size(900, 0);
            this.pnlResult.TabIndex = 4;
            this.pnlResult.Visible = false;
            this.pnlResult.Paint += new System.Windows.Forms.PaintEventHandler(this.Result_Paint);

            // 
            // pnlResultInner
            // 
            this.pnlResultInner.Controls.Add(this.lblRawPayload);
            this.pnlResultInner.Controls.Add(this.lblResultDetail);
            this.pnlResultInner.Controls.Add(this.lblResultCourse);
            this.pnlResultInner.Controls.Add(this.lblResultStatus);
            this.pnlResultInner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlResultInner.Location = new System.Drawing.Point(4, 0);
            this.pnlResultInner.Name = "pnlResultInner";
            this.pnlResultInner.Padding = new System.Windows.Forms.Padding(16, 10, 16, 10);
            this.pnlResultInner.Size = new System.Drawing.Size(896, 0);
            this.pnlResultInner.TabIndex = 1;

            // 
            // lblResultStatus
            // 
            this.lblResultStatus.AutoSize = true;
            this.lblResultStatus.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblResultStatus.ForeColor = System.Drawing.Color.FromArgb(0, 120, 60);
            this.lblResultStatus.Location = new System.Drawing.Point(16, 10);
            this.lblResultStatus.Name = "lblResultStatus";
            this.lblResultStatus.Text = "✓  Attendance Recorded";

            // 
            // lblResultCourse
            // 
            this.lblResultCourse.AutoSize = true;
            this.lblResultCourse.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblResultCourse.ForeColor = System.Drawing.Color.FromArgb(60, 60, 80);
            this.lblResultCourse.Location = new System.Drawing.Point(16, 34);
            this.lblResultCourse.Name = "lblResultCourse";

            // 
            // lblResultDetail
            // 
            this.lblResultDetail.AutoSize = true;
            this.lblResultDetail.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblResultDetail.ForeColor = System.Drawing.Color.FromArgb(100, 100, 120);
            this.lblResultDetail.Location = new System.Drawing.Point(16, 56);
            this.lblResultDetail.Name = "lblResultDetail";

            // 
            // lblRawPayload
            // 
            this.lblRawPayload.AutoSize = true;
            this.lblRawPayload.Font = new System.Drawing.Font("Consolas", 8F);
            this.lblRawPayload.ForeColor = System.Drawing.Color.FromArgb(120, 120, 140);
            this.lblRawPayload.Location = new System.Drawing.Point(16, 78);
            this.lblRawPayload.Name = "lblRawPayload";

            // 
            // pnlResultAccent
            // 
            this.pnlResultAccent.BackColor = System.Drawing.Color.ForestGreen;
            this.pnlResultAccent.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlResultAccent.Location = new System.Drawing.Point(0, 0);
            this.pnlResultAccent.Name = "pnlResultAccent";
            this.pnlResultAccent.Size = new System.Drawing.Size(4, 0);
            this.pnlResultAccent.TabIndex = 0;

            // 
            // historyPanel
            // 
            this.historyPanel.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.historyPanel.Controls.Add(this.dgvHistory);
            this.historyPanel.Controls.Add(this.historyTitleBar);
            this.historyPanel.Controls.Add(this.historyGap);
            this.historyPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.historyPanel.Location = new System.Drawing.Point(0, 568);
            this.historyPanel.Name = "historyPanel";
            this.historyPanel.Size = new System.Drawing.Size(900, 300);
            this.historyPanel.TabIndex = 5;

            // 
            // historyGap
            // 
            this.historyGap.BackColor = System.Drawing.Color.Transparent;
            this.historyGap.Dock = System.Windows.Forms.DockStyle.Top;
            this.historyGap.Height = 8;
            this.historyGap.Name = "historyGap";

            // 
            // historyTitleBar
            // 
            this.historyTitleBar.BackColor = System.Drawing.Color.White;
            this.historyTitleBar.Controls.Add(this.lblHistoryTitle);
            this.historyTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.historyTitleBar.Height = 42;
            this.historyTitleBar.Name = "historyTitleBar";
            this.historyTitleBar.Paint += new System.Windows.Forms.PaintEventHandler(this.TitleBar_Paint);

            // 
            // lblHistoryTitle
            // 
            this.lblHistoryTitle.AutoSize = true;
            this.lblHistoryTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblHistoryTitle.ForeColor = System.Drawing.Color.FromArgb(50, 50, 70);
            this.lblHistoryTitle.Location = new System.Drawing.Point(18, 11);
            this.lblHistoryTitle.Name = "lblHistoryTitle";
            this.lblHistoryTitle.Text = "Scan History (this session)";

            // 
            // dgvHistory
            // 
            this.dgvHistory.AllowUserToAddRows = false;
            this.dgvHistory.AllowUserToDeleteRows = false;
            this.dgvHistory.AllowUserToResizeColumns = false;
            this.dgvHistory.AllowUserToResizeRows = false;
            this.dgvHistory.AutoGenerateColumns = false;
            this.dgvHistory.BackgroundColor = System.Drawing.Color.White;
            this.dgvHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvHistory.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvHistory.ColumnHeadersHeight = 40;
            this.dgvHistory.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvHistory.GridColor = System.Drawing.Color.FromArgb(230, 230, 238);
            this.dgvHistory.Location = new System.Drawing.Point(0, 50);
            this.dgvHistory.Name = "dgvHistory";
            this.dgvHistory.RowHeadersVisible = false;
            this.dgvHistory.RowTemplate.Height = 38;
            this.dgvHistory.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHistory.Size = new System.Drawing.Size(900, 10);
            this.dgvHistory.TabIndex = 2;
            this.dgvHistory.SelectionChanged += new System.EventHandler(this.DgvHistory_SelectionChanged);
            this.dgvHistory.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.DgvHistory_DataBindingComplete);

            // 
            // QRScanControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.Controls.Add(this.scrollPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "QRScanControl";
            this.Size = new System.Drawing.Size(900, 800);
            this.Load += new System.EventHandler(this.QRScanControl_Load);
            this.scrollPanel.ResumeLayout(false);
            this.historyPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
            this.historyTitleBar.ResumeLayout(false);
            this.historyTitleBar.PerformLayout();
            this.pnlResult.ResumeLayout(false);
            this.pnlResultInner.ResumeLayout(false);
            this.pnlResultInner.PerformLayout();
            this.mainAreaPanel.ResumeLayout(false);
            this.pnlUpload.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picQR)).EndInit();
            this.pnlCamera.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCamera)).EndInit();
            this.togglePanel.ResumeLayout(false);
            this.pillPanel.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        // Standard WinForms declarations
        private System.Windows.Forms.Panel scrollPanel;
        private System.Windows.Forms.Panel topSpacer;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Label lblHeaderIcon;
        private System.Windows.Forms.Label lblHeaderSub;
        private System.Windows.Forms.Panel togglePanel;
        private System.Windows.Forms.Panel pillPanel;
        private System.Windows.Forms.Button btnModeUpload;
        private System.Windows.Forms.Button btnModeCamera;
        private System.Windows.Forms.Panel mainAreaPanel;
        private System.Windows.Forms.Panel pnlUpload;
        private System.Windows.Forms.PictureBox picQR;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnClearImage;
        private System.Windows.Forms.Panel pnlCamera;
        private System.Windows.Forms.Label lblCameraStatus;
        private System.Windows.Forms.PictureBox picCamera;
        private System.Windows.Forms.ComboBox cmbCamera;
        private System.Windows.Forms.Button btnStartCamera;
        private System.Windows.Forms.Button btnStopCamera;
        private System.Windows.Forms.Panel pnlInfoSide;
        private System.Windows.Forms.Panel pnlResult;
        private System.Windows.Forms.Panel pnlResultAccent;
        private System.Windows.Forms.Panel pnlResultInner;
        private System.Windows.Forms.Label lblResultStatus;
        private System.Windows.Forms.Label lblResultCourse;
        private System.Windows.Forms.Label lblResultDetail;
        private System.Windows.Forms.Label lblRawPayload;
        private System.Windows.Forms.Panel historyPanel;
        private System.Windows.Forms.Panel historyGap;
        private System.Windows.Forms.Panel historyTitleBar;
        private System.Windows.Forms.Label lblHistoryTitle;
        private System.Windows.Forms.DataGridView dgvHistory;
    }
}