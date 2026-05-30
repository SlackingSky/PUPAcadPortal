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
            this.components = new System.ComponentModel.Container();
            this._lblStatus = new System.Windows.Forms.Label();
            this._lblCountdown = new System.Windows.Forms.Label();
            this._picQr = new System.Windows.Forms.PictureBox();
            this.infoBar = new System.Windows.Forms.Panel();
            this.tbl = new System.Windows.Forms.TableLayoutPanel();
            this.lblSessionKey = new System.Windows.Forms.Label();
            this._lblSessionVal = new System.Windows.Forms.Label();
            this.lblDateKey = new System.Windows.Forms.Label();
            this._lblDateVal = new System.Windows.Forms.Label();
            this.lblTimeKey = new System.Windows.Forms.Label();
            this._lblClockVal = new System.Windows.Forms.Label();
            this.pnlBtns = new System.Windows.Forms.Panel();
            this._btnRefresh = new System.Windows.Forms.Button();
            this._btnDownload = new System.Windows.Forms.Button();
            this._countdownTimer = new System.Windows.Forms.Timer(this.components);
            this._expiryTimer = new System.Windows.Forms.Timer(this.components);
            this._animTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._picQr)).BeginInit();
            this.infoBar.SuspendLayout();
            this.tbl.SuspendLayout();
            this.pnlBtns.SuspendLayout();
            this.SuspendLayout();
            // 
            // _lblStatus
            // 
            this._lblStatus.AutoSize = true;
            this._lblStatus.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this._lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(139)))), ((int)(((byte)(34)))));
            this._lblStatus.Location = new System.Drawing.Point(12, 8);
            this._lblStatus.Name = "_lblStatus";
            this._lblStatus.Size = new System.Drawing.Size(55, 15);
            this._lblStatus.TabIndex = 0;
            this._lblStatus.Text = "● Active";
            // 
            // _lblCountdown
            // 
            this._lblCountdown.AutoSize = true;
            this._lblCountdown.Font = new System.Drawing.Font("Segoe UI", 7.5F);
            this._lblCountdown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this._lblCountdown.Location = new System.Drawing.Point(12, 26);
            this._lblCountdown.Name = "_lblCountdown";
            this._lblCountdown.Size = new System.Drawing.Size(0, 12);
            this._lblCountdown.TabIndex = 1;
            // 
            // _picQr
            // 
            this._picQr.BackColor = System.Drawing.Color.White;
            this._picQr.Location = new System.Drawing.Point(20, 46);
            this._picQr.Name = "_picQr";
            this._picQr.Size = new System.Drawing.Size(80, 80);
            this._picQr.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._picQr.TabIndex = 2;
            this._picQr.TabStop = false;
            // 
            // infoBar
            // 
            this.infoBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.infoBar.Controls.Add(this.tbl);
            this.infoBar.Location = new System.Drawing.Point(8, 130);
            this.infoBar.Name = "infoBar";
            this.infoBar.Size = new System.Drawing.Size(200, 80);
            this.infoBar.TabIndex = 3;
            this.infoBar.Paint += new System.Windows.Forms.PaintEventHandler(this.infoBar_Paint);
            // 
            // tbl
            // 
            this.tbl.BackColor = System.Drawing.Color.Transparent;
            this.tbl.ColumnCount = 2;
            this.tbl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tbl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbl.Controls.Add(this.lblSessionKey, 0, 0);
            this.tbl.Controls.Add(this._lblSessionVal, 1, 0);
            this.tbl.Controls.Add(this.lblDateKey, 0, 1);
            this.tbl.Controls.Add(this._lblDateVal, 1, 1);
            this.tbl.Controls.Add(this.lblTimeKey, 0, 2);
            this.tbl.Controls.Add(this._lblClockVal, 1, 2);
            this.tbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbl.Location = new System.Drawing.Point(0, 0);
            this.tbl.Name = "tbl";
            this.tbl.Padding = new System.Windows.Forms.Padding(8, 6, 8, 4);
            this.tbl.RowCount = 3;
            this.tbl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tbl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tbl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tbl.Size = new System.Drawing.Size(200, 80);
            this.tbl.TabIndex = 0;
            // 
            // lblSessionKey
            // 
            this.lblSessionKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSessionKey.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblSessionKey.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblSessionKey.Location = new System.Drawing.Point(8, 6);
            this.lblSessionKey.Margin = new System.Windows.Forms.Padding(0);
            this.lblSessionKey.Name = "lblSessionKey";
            this.lblSessionKey.Size = new System.Drawing.Size(56, 23);
            this.lblSessionKey.TabIndex = 0;
            this.lblSessionKey.Text = "Session";
            this.lblSessionKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lblSessionVal
            // 
            this._lblSessionVal.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblSessionVal.Font = new System.Drawing.Font("Segoe UI", 8F);
            this._lblSessionVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this._lblSessionVal.Location = new System.Drawing.Point(64, 6);
            this._lblSessionVal.Margin = new System.Windows.Forms.Padding(0);
            this._lblSessionVal.Name = "_lblSessionVal";
            this._lblSessionVal.Size = new System.Drawing.Size(128, 23);
            this._lblSessionVal.TabIndex = 1;
            this._lblSessionVal.Text = "Morning";
            this._lblSessionVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDateKey
            // 
            this.lblDateKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDateKey.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblDateKey.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblDateKey.Location = new System.Drawing.Point(8, 29);
            this.lblDateKey.Margin = new System.Windows.Forms.Padding(0);
            this.lblDateKey.Name = "lblDateKey";
            this.lblDateKey.Size = new System.Drawing.Size(56, 23);
            this.lblDateKey.TabIndex = 2;
            this.lblDateKey.Text = "Date";
            this.lblDateKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lblDateVal
            // 
            this._lblDateVal.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblDateVal.Font = new System.Drawing.Font("Segoe UI", 8F);
            this._lblDateVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this._lblDateVal.Location = new System.Drawing.Point(64, 29);
            this._lblDateVal.Margin = new System.Windows.Forms.Padding(0);
            this._lblDateVal.Name = "_lblDateVal";
            this._lblDateVal.Size = new System.Drawing.Size(128, 23);
            this._lblDateVal.TabIndex = 3;
            this._lblDateVal.Text = "DateVal";
            this._lblDateVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTimeKey
            // 
            this.lblTimeKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTimeKey.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblTimeKey.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblTimeKey.Location = new System.Drawing.Point(8, 52);
            this.lblTimeKey.Margin = new System.Windows.Forms.Padding(0);
            this.lblTimeKey.Name = "lblTimeKey";
            this.lblTimeKey.Size = new System.Drawing.Size(56, 24);
            this.lblTimeKey.TabIndex = 4;
            this.lblTimeKey.Text = "Time";
            this.lblTimeKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lblClockVal
            // 
            this._lblClockVal.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblClockVal.Font = new System.Drawing.Font("Segoe UI", 8F);
            this._lblClockVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this._lblClockVal.Location = new System.Drawing.Point(64, 52);
            this._lblClockVal.Margin = new System.Windows.Forms.Padding(0);
            this._lblClockVal.Name = "_lblClockVal";
            this._lblClockVal.Size = new System.Drawing.Size(128, 24);
            this._lblClockVal.TabIndex = 5;
            this._lblClockVal.Text = "ClockVal";
            this._lblClockVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBtns
            // 
            this.pnlBtns.BackColor = System.Drawing.Color.White;
            this.pnlBtns.Controls.Add(this._btnRefresh);
            this.pnlBtns.Controls.Add(this._btnDownload);
            this.pnlBtns.Location = new System.Drawing.Point(0, 220);
            this.pnlBtns.Name = "pnlBtns";
            this.pnlBtns.Size = new System.Drawing.Size(220, 44);
            this.pnlBtns.TabIndex = 4;
            // 
            // _btnRefresh
            // 
            this._btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnRefresh.FlatAppearance.BorderSize = 0;
            this._btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this._btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnRefresh.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this._btnRefresh.ForeColor = System.Drawing.Color.White;
            this._btnRefresh.Location = new System.Drawing.Point(10, 5);
            this._btnRefresh.Name = "_btnRefresh";
            this._btnRefresh.Size = new System.Drawing.Size(95, 34);
            this._btnRefresh.TabIndex = 0;
            this._btnRefresh.Text = "↺  Refresh QR";
            this._btnRefresh.UseVisualStyleBackColor = false;
            this._btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // _btnDownload
            // 
            this._btnDownload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(100)))), ((int)(((byte)(180)))));
            this._btnDownload.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnDownload.FlatAppearance.BorderSize = 0;
            this._btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnDownload.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this._btnDownload.ForeColor = System.Drawing.Color.White;
            this._btnDownload.Location = new System.Drawing.Point(111, 5);
            this._btnDownload.Name = "_btnDownload";
            this._btnDownload.Size = new System.Drawing.Size(100, 34);
            this._btnDownload.TabIndex = 1;
            this._btnDownload.Text = "⬇  Download QR";
            this._btnDownload.UseVisualStyleBackColor = false;
            this._btnDownload.Click += new System.EventHandler(this.BtnDownload_Click);
            // 
            // _countdownTimer
            // 
            this._countdownTimer.Interval = 1000;
            this._countdownTimer.Tick += new System.EventHandler(this.CountdownTimer_Tick);
            // 
            // _expiryTimer
            // 
            this._expiryTimer.Tick += new System.EventHandler(this.ExpiryTimer_Tick);
            // 
            // _animTimer
            // 
            this._animTimer.Interval = 250;
            this._animTimer.Tick += new System.EventHandler(this.AnimTick);
            // 
            // QrCodeAttendanceControl
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlBtns);
            this.Controls.Add(this.infoBar);
            this.Controls.Add(this._picQr);
            this.Controls.Add(this._lblCountdown);
            this.Controls.Add(this._lblStatus);
            this.DoubleBuffered = true;
            this.Name = "QrCodeAttendanceControl";
            this.Size = new System.Drawing.Size(220, 270);
            this.SizeChanged += new System.EventHandler(this.QrCodeAttendanceControl_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this._picQr)).EndInit();
            this.infoBar.ResumeLayout(false);
            this.tbl.ResumeLayout(false);
            this.pnlBtns.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
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