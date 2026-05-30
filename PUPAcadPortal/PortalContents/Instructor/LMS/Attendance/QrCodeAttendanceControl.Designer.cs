namespace PUPAcadPortal.PortalContents.Instructor.LMS
{
    partial class QrCodeAttendanceControl
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
            components = new System.ComponentModel.Container();
            _lblStatus = new Label();
            _lblCountdown = new Label();
            _picQr = new PictureBox();
            infoBar = new Panel();
            tbl = new TableLayoutPanel();
            lblSessionKey = new Label();
            _lblSessionVal = new Label();
            lblDateKey = new Label();
            _lblDateVal = new Label();
            lblTimeKey = new Label();
            _lblClockVal = new Label();
            pnlBtns = new Panel();
            _btnRefresh = new Button();
            _btnDownload = new Button();
            _countdownTimer = new System.Windows.Forms.Timer(components);
            _expiryTimer = new System.Windows.Forms.Timer(components);
            _animTimer = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)_picQr).BeginInit();
            infoBar.SuspendLayout();
            tbl.SuspendLayout();
            pnlBtns.SuspendLayout();
            SuspendLayout();
            // 
            // _lblStatus
            // 
            _lblStatus.AutoSize = true;
            _lblStatus.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            _lblStatus.ForeColor = Color.FromArgb(34, 139, 34);
            _lblStatus.Location = new Point(12, 8);
            _lblStatus.Name = "_lblStatus";
            _lblStatus.Size = new Size(53, 15);
            _lblStatus.TabIndex = 0;
            _lblStatus.Text = "● Active";
            // 
            // _lblCountdown
            // 
            _lblCountdown.AutoSize = true;
            _lblCountdown.Font = new Font("Segoe UI", 7.5F);
            _lblCountdown.ForeColor = Color.FromArgb(90, 90, 90);
            _lblCountdown.Location = new Point(12, 26);
            _lblCountdown.Name = "_lblCountdown";
            _lblCountdown.Size = new Size(0, 12);
            _lblCountdown.TabIndex = 1;
            // 
            // _picQr
            // 
            _picQr.BackColor = Color.White;
            _picQr.Location = new Point(20, 46);
            _picQr.Name = "_picQr";
            _picQr.Size = new Size(80, 80);
            _picQr.SizeMode = PictureBoxSizeMode.Zoom;
            _picQr.TabIndex = 2;
            _picQr.TabStop = false;
            // 
            // infoBar
            // 
            infoBar.BackColor = Color.FromArgb(248, 248, 248);
            infoBar.Controls.Add(tbl);
            infoBar.Location = new Point(8, 130);
            infoBar.Name = "infoBar";
            infoBar.Size = new Size(200, 80);
            infoBar.TabIndex = 3;
            infoBar.Paint += infoBar_Paint;
            // 
            // tbl
            // 
            tbl.BackColor = Color.Transparent;
            tbl.ColumnCount = 2;
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 56F));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tbl.Controls.Add(lblSessionKey, 0, 0);
            tbl.Controls.Add(_lblSessionVal, 1, 0);
            tbl.Controls.Add(lblDateKey, 0, 1);
            tbl.Controls.Add(_lblDateVal, 1, 1);
            tbl.Controls.Add(lblTimeKey, 0, 2);
            tbl.Controls.Add(_lblClockVal, 1, 2);
            tbl.Dock = DockStyle.Fill;
            tbl.Location = new Point(0, 0);
            tbl.Name = "tbl";
            tbl.Padding = new Padding(8, 6, 8, 4);
            tbl.RowCount = 3;
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tbl.Size = new Size(200, 80);
            tbl.TabIndex = 0;
            // 
            // lblSessionKey
            // 
            lblSessionKey.Dock = DockStyle.Fill;
            lblSessionKey.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblSessionKey.ForeColor = Color.FromArgb(106, 0, 0);
            lblSessionKey.Location = new Point(8, 6);
            lblSessionKey.Margin = new Padding(0);
            lblSessionKey.Name = "lblSessionKey";
            lblSessionKey.Size = new Size(56, 23);
            lblSessionKey.TabIndex = 0;
            lblSessionKey.Text = "Session";
            lblSessionKey.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _lblSessionVal
            // 
            _lblSessionVal.Dock = DockStyle.Fill;
            _lblSessionVal.Font = new Font("Segoe UI", 8F);
            _lblSessionVal.ForeColor = Color.FromArgb(90, 90, 90);
            _lblSessionVal.Location = new Point(64, 6);
            _lblSessionVal.Margin = new Padding(0);
            _lblSessionVal.Name = "_lblSessionVal";
            _lblSessionVal.Size = new Size(128, 23);
            _lblSessionVal.TabIndex = 1;
            _lblSessionVal.Text = "Morning";
            _lblSessionVal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDateKey
            // 
            lblDateKey.Dock = DockStyle.Fill;
            lblDateKey.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblDateKey.ForeColor = Color.FromArgb(106, 0, 0);
            lblDateKey.Location = new Point(8, 29);
            lblDateKey.Margin = new Padding(0);
            lblDateKey.Name = "lblDateKey";
            lblDateKey.Size = new Size(56, 23);
            lblDateKey.TabIndex = 2;
            lblDateKey.Text = "Date";
            lblDateKey.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _lblDateVal
            // 
            _lblDateVal.Dock = DockStyle.Fill;
            _lblDateVal.Font = new Font("Segoe UI", 8F);
            _lblDateVal.ForeColor = Color.FromArgb(90, 90, 90);
            _lblDateVal.Location = new Point(64, 29);
            _lblDateVal.Margin = new Padding(0);
            _lblDateVal.Name = "_lblDateVal";
            _lblDateVal.Size = new Size(128, 23);
            _lblDateVal.TabIndex = 3;
            _lblDateVal.Text = "DateVal";
            _lblDateVal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTimeKey
            // 
            lblTimeKey.Dock = DockStyle.Fill;
            lblTimeKey.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblTimeKey.ForeColor = Color.FromArgb(106, 0, 0);
            lblTimeKey.Location = new Point(8, 52);
            lblTimeKey.Margin = new Padding(0);
            lblTimeKey.Name = "lblTimeKey";
            lblTimeKey.Size = new Size(56, 24);
            lblTimeKey.TabIndex = 4;
            lblTimeKey.Text = "Time";
            lblTimeKey.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _lblClockVal
            // 
            _lblClockVal.Dock = DockStyle.Fill;
            _lblClockVal.Font = new Font("Segoe UI", 8F);
            _lblClockVal.ForeColor = Color.FromArgb(90, 90, 90);
            _lblClockVal.Location = new Point(64, 52);
            _lblClockVal.Margin = new Padding(0);
            _lblClockVal.Name = "_lblClockVal";
            _lblClockVal.Size = new Size(128, 24);
            _lblClockVal.TabIndex = 5;
            _lblClockVal.Text = "ClockVal";
            _lblClockVal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlBtns
            // 
            pnlBtns.BackColor = Color.White;
            pnlBtns.Controls.Add(_btnRefresh);
            pnlBtns.Controls.Add(_btnDownload);
            pnlBtns.Location = new Point(0, 220);
            pnlBtns.Name = "pnlBtns";
            pnlBtns.Size = new Size(220, 44);
            pnlBtns.TabIndex = 4;
            // 
            // _btnRefresh
            // 
            _btnRefresh.BackColor = Color.FromArgb(106, 0, 0);
            _btnRefresh.Cursor = Cursors.Hand;
            _btnRefresh.FlatAppearance.BorderSize = 0;
            _btnRefresh.FlatAppearance.MouseOverBackColor = Color.FromArgb(160, 30, 30);
            _btnRefresh.FlatStyle = FlatStyle.Flat;
            _btnRefresh.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            _btnRefresh.ForeColor = Color.White;
            _btnRefresh.Location = new Point(10, 5);
            _btnRefresh.Name = "_btnRefresh";
            _btnRefresh.Size = new Size(95, 34);
            _btnRefresh.TabIndex = 0;
            _btnRefresh.Text = "↺  Refresh QR";
            _btnRefresh.UseVisualStyleBackColor = false;
            _btnRefresh.Click += BtnRefresh_Click;
            // 
            // _btnDownload
            // 
            _btnDownload.BackColor = Color.FromArgb(40, 100, 180);
            _btnDownload.Cursor = Cursors.Hand;
            _btnDownload.FlatAppearance.BorderSize = 0;
            _btnDownload.FlatStyle = FlatStyle.Flat;
            _btnDownload.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            _btnDownload.ForeColor = Color.White;
            _btnDownload.Location = new Point(111, 5);
            _btnDownload.Name = "_btnDownload";
            _btnDownload.Size = new Size(100, 34);
            _btnDownload.TabIndex = 1;
            _btnDownload.Text = "⬇  Download QR";
            _btnDownload.UseVisualStyleBackColor = false;
            _btnDownload.Click += BtnDownload_Click;
            // 
            // _countdownTimer
            // 
            _countdownTimer.Interval = 1000;
            _countdownTimer.Tick += CountdownTimer_Tick;
            // 
            // _expiryTimer
            // 
            _expiryTimer.Tick += ExpiryTimer_Tick;
            // 
            // _animTimer
            // 
            _animTimer.Interval = 250;
            _animTimer.Tick += AnimTick;
            // 
            // QrCodeAttendanceControl
            // 
            BackColor = Color.White;
            Controls.Add(pnlBtns);
            Controls.Add(infoBar);
            Controls.Add(_picQr);
            Controls.Add(_lblCountdown);
            Controls.Add(_lblStatus);
            DoubleBuffered = true;
            Name = "QrCodeAttendanceControl";
            Size = new Size(220, 270);
            SizeChanged += QrCodeAttendanceControl_SizeChanged;
            ((System.ComponentModel.ISupportInitialize)_picQr).EndInit();
            infoBar.ResumeLayout(false);
            tbl.ResumeLayout(false);
            pnlBtns.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label _lblStatus;
        private System.Windows.Forms.Label _lblCountdown;
        private System.Windows.Forms.PictureBox _picQr;
        private System.Windows.Forms.Panel infoBar;
        private System.Windows.Forms.TableLayoutPanel tbl;
        private System.Windows.Forms.Label lblSessionKey;
        private System.Windows.Forms.Label _lblSessionVal;
        private System.Windows.Forms.Label lblDateKey;
        private System.Windows.Forms.Label _lblDateVal;
        private System.Windows.Forms.Label lblTimeKey;
        private System.Windows.Forms.Label _lblClockVal;
        private System.Windows.Forms.Panel pnlBtns;
        private System.Windows.Forms.Button _btnRefresh;
        private System.Windows.Forms.Button _btnDownload;
        private System.Windows.Forms.Timer _countdownTimer;
        private System.Windows.Forms.Timer _expiryTimer;
        private System.Windows.Forms.Timer _animTimer;
    }
}