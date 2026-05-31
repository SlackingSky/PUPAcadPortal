namespace PUPAcadPortal.PortalContents.Instructor.LMS.Calendar
{
    partial class FacultyNotificationsPanel
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this._lblTitle = new System.Windows.Forms.Label();
            this._btnClose = new System.Windows.Forms.Button();
            this._flp = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(14)))), ((int)(((byte)(79)))));
            this.pnlHeader.Controls.Add(this._lblTitle);
            this.pnlHeader.Controls.Add(this._btnClose);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(320, 44);
            this.pnlHeader.TabIndex = 0;
            // 
            // _lblTitle
            // 
            this._lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this._lblTitle.ForeColor = System.Drawing.Color.White;
            this._lblTitle.Location = new System.Drawing.Point(0, 0);
            this._lblTitle.Name = "_lblTitle";
            this._lblTitle.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this._lblTitle.Size = new System.Drawing.Size(290, 44);
            this._lblTitle.TabIndex = 0;
            this._lblTitle.Text = "🔔  Notifications";
            this._lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _btnClose
            // 
            this._btnClose.BackColor = System.Drawing.Color.Transparent;
            this._btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this._btnClose.FlatAppearance.BorderSize = 0;
            this._btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnClose.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._btnClose.ForeColor = System.Drawing.Color.White;
            this._btnClose.Location = new System.Drawing.Point(290, 0);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(30, 44);
            this._btnClose.TabIndex = 1;
            this._btnClose.Text = "✕";
            this._btnClose.UseVisualStyleBackColor = false;
            this._btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // _flp
            // 
            this._flp.AutoScroll = true;
            this._flp.Dock = System.Windows.Forms.DockStyle.Fill;
            this._flp.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._flp.Location = new System.Drawing.Point(0, 44);
            this._flp.Name = "_flp";
            this._flp.Padding = new System.Windows.Forms.Padding(8);
            this._flp.Size = new System.Drawing.Size(320, 256);
            this._flp.TabIndex = 1;
            this._flp.WrapContents = false;
            // 
            // FacultyNotificationsPanel
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this._flp);
            this.Controls.Add(this.pnlHeader);
            this.Name = "FacultyNotificationsPanel";
            this.Size = new System.Drawing.Size(320, 300);
            this.pnlHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label _lblTitle;
        private System.Windows.Forms.Button _btnClose;
        private System.Windows.Forms.FlowLayoutPanel _flp;
    }
}