namespace PUPAcadPortal.PortalContents.Instructor.LMS
{
    partial class QrCodePopupForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.lblHeaderSub = new System.Windows.Forms.Label();
            this.pnlCenter = new System.Windows.Forms.Panel();
            this.pnlQrCard = new System.Windows.Forms.Panel();
            this._picQr = new System.Windows.Forms.PictureBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblCountdown = new System.Windows.Forms.Label();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblCourse = new System.Windows.Forms.Label();
            this.lblSession = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this._btnRefresh = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this._tick = new System.Windows.Forms.Timer(this.components);
            this.anim = new System.Windows.Forms.Timer(this.components);

            this.pnlHeader.SuspendLayout();
            this.pnlCenter.SuspendLayout();
            this.pnlQrCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._picQr)).BeginInit();
            this.pnlInfo.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pnlHeader.Controls.Add(this.lblHeaderTitle);
            this.pnlHeader.Controls.Add(this.lblHeaderSub);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(404, 60);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.White;
            this.lblHeaderTitle.Location = new System.Drawing.Point(0, 0);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.lblHeaderTitle.Size = new System.Drawing.Size(404, 40);
            this.lblHeaderTitle.TabIndex = 0;
            this.lblHeaderTitle.Text = "Generate QR Code";
            this.lblHeaderTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHeaderSub
            // 
            this.lblHeaderSub.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblHeaderSub.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblHeaderSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.lblHeaderSub.Location = new System.Drawing.Point(0, 40);
            this.lblHeaderSub.Name = "lblHeaderSub";
            this.lblHeaderSub.Padding = new System.Windows.Forms.Padding(19, 0, 0, 4);
            this.lblHeaderSub.Size = new System.Drawing.Size(404, 20);
            this.lblHeaderSub.TabIndex = 1;
            this.lblHeaderSub.Text = "Students scan this code to mark their attendance";
            // 
            // pnlCenter
            // 
            this.pnlCenter.BackColor = System.Drawing.Color.White;
            this.pnlCenter.Controls.Add(this.pnlQrCard);
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCenter.Location = new System.Drawing.Point(0, 60);
            this.pnlCenter.Name = "pnlCenter";
            this.pnlCenter.Size = new System.Drawing.Size(404, 304);
            this.pnlCenter.TabIndex = 1;
            // 
            // pnlQrCard
            // 
            this.pnlQrCard.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pnlQrCard.BackColor = System.Drawing.Color.White;
            this.pnlQrCard.Controls.Add(this._picQr);
            this.pnlQrCard.Location = new System.Drawing.Point(60, 12);
            this.pnlQrCard.Name = "pnlQrCard";
            this.pnlQrCard.Size = new System.Drawing.Size(284, 284);
            this.pnlQrCard.TabIndex = 0;
            // 
            // _picQr
            // 
            this._picQr.BackColor = System.Drawing.Color.White;
            this._picQr.Location = new System.Drawing.Point(12, 12);
            this._picQr.Name = "_picQr";
            this._picQr.Size = new System.Drawing.Size(260, 260);
            this._picQr.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._picQr.TabIndex = 0;
            this._picQr.TabStop = false;
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.White;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(139)))), ((int)(((byte)(14)))));
            this.lblStatus.Location = new System.Drawing.Point(0, 364);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(404, 24);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "● Active";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCountdown
            // 
            this.lblCountdown.BackColor = System.Drawing.Color.White;
            this.lblCountdown.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCountdown.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblCountdown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.lblCountdown.Location = new System.Drawing.Point(0, 388);
            this.lblCountdown.Name = "lblCountdown";
            this.lblCountdown.Size = new System.Drawing.Size(404, 22);
            this.lblCountdown.TabIndex = 3;
            this.lblCountdown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlInfo
            // 
            this.pnlInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.pnlInfo.Controls.Add(this.lblCourse);
            this.pnlInfo.Controls.Add(this.lblSession);
            this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInfo.Location = new System.Drawing.Point(0, 410);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Padding = new System.Windows.Forms.Padding(18, 8, 18, 8);
            this.pnlInfo.Size = new System.Drawing.Size(404, 72);
            this.pnlInfo.TabIndex = 4;
            // 
            // lblCourse
            // 
            this.lblCourse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCourse.BackColor = System.Drawing.Color.Transparent;
            this.lblCourse.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCourse.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.lblCourse.Location = new System.Drawing.Point(10, 8);
            this.lblCourse.Name = "lblCourse";
            this.lblCourse.Size = new System.Drawing.Size(384, 20);
            this.lblCourse.TabIndex = 0;
            this.lblCourse.Text = "Course : ";
            // 
            // lblSession
            // 
            this.lblSession.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSession.BackColor = System.Drawing.Color.Transparent;
            this.lblSession.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblSession.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.lblSession.Location = new System.Drawing.Point(10, 32);
            this.lblSession.Name = "lblSession";
            this.lblSession.Size = new System.Drawing.Size(384, 20);
            this.lblSession.TabIndex = 1;
            this.lblSession.Text = "Session : ";
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.White;
            this.pnlFooter.Controls.Add(this._btnRefresh);
            this.pnlFooter.Controls.Add(this.btnDownload);
            this.pnlFooter.Controls.Add(this.btnClose);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 541);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(404, 60);
            this.pnlFooter.TabIndex = 5;
            // 
            // _btnRefresh
            // 
            this._btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnRefresh.FlatAppearance.BorderSize = 1;
            this._btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnRefresh.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this._btnRefresh.ForeColor = System.Drawing.Color.White;
            this._btnRefresh.Location = new System.Drawing.Point(74, 12);
            this._btnRefresh.Name = "_btnRefresh";
            this._btnRefresh.Size = new System.Drawing.Size(110, 36);
            this._btnRefresh.TabIndex = 0;
            this._btnRefresh.Text = "↺  Refresh";
            this._btnRefresh.UseVisualStyleBackColor = false;
            // 
            // btnDownload
            // 
            this.btnDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownload.BackColor = System.Drawing.Color.White;
            this.btnDownload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDownload.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnDownload.FlatAppearance.BorderSize = 1;
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownload.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnDownload.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnDownload.Location = new System.Drawing.Point(192, 12);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(120, 36);
            this.btnDownload.TabIndex = 1;
            this.btnDownload.Text = "⬇  Download PNG";
            this.btnDownload.UseVisualStyleBackColor = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnClose.FlatAppearance.BorderSize = 1;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnClose.Location = new System.Drawing.Point(320, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 36);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // _tick
            // 
            this._tick.Interval = 1000;
            // 
            // anim
            // 
            this.anim.Interval = 280;
            // 
            // QrCodePopupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(404, 601);
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.lblCountdown);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pnlCenter);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlFooter);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 720);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(380, 600);
            this.Name = "QrCodePopupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "QR Code — Attendance";
            this.pnlHeader.ResumeLayout(false);
            this.pnlCenter.ResumeLayout(false);
            this.pnlQrCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._picQr)).EndInit();
            this.pnlInfo.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Label lblHeaderSub;
        private System.Windows.Forms.Panel pnlCenter;
        private System.Windows.Forms.Panel pnlQrCard;
        private System.Windows.Forms.PictureBox _picQr;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblCountdown;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Label lblCourse;
        private System.Windows.Forms.Label lblSession;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button _btnRefresh;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Timer _tick;
        private System.Windows.Forms.Timer anim;
    }
}