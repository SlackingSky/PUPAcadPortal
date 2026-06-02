namespace PUPAcadPortal.PortalContents.Student.LMS.Attendance
{
    partial class AtRiskPopup
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
            this.pnlTitleBar = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlSubtitle = new System.Windows.Forms.Panel();
            this.lblSub = new System.Windows.Forms.Label();
            this.pnlScroll = new System.Windows.Forms.Panel();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnOk = new System.Windows.Forms.Button();

            this.pnlTitleBar.SuspendLayout();
            this.pnlSubtitle.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlTitleBar
            // 
            this.pnlTitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pnlTitleBar.Controls.Add(this.lblTitle);
            this.pnlTitleBar.Controls.Add(this.btnClose);
            this.pnlTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitleBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTitleBar.Name = "pnlTitleBar";
            this.pnlTitleBar.Size = new System.Drawing.Size(520, 52);
            this.pnlTitleBar.TabIndex = 0;
            this.pnlTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlTitleBar_MouseDown);
            this.pnlTitleBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlTitleBar_MouseMove);
            this.pnlTitleBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlTitleBar_MouseUp);
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(18, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(380, 52);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "At-Risk Subjects";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(470, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(40, 40);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "✕";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlSubtitle
            // 
            this.pnlSubtitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.pnlSubtitle.Controls.Add(this.lblSub);
            this.pnlSubtitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSubtitle.Location = new System.Drawing.Point(0, 52);
            this.pnlSubtitle.Name = "pnlSubtitle";
            this.pnlSubtitle.Size = new System.Drawing.Size(520, 44);
            this.pnlSubtitle.TabIndex = 1;
            this.pnlSubtitle.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlSubtitle_Paint);
            // 
            // lblSub
            // 
            this.lblSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSub.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblSub.Location = new System.Drawing.Point(0, 0);
            this.lblSub.Name = "lblSub";
            this.lblSub.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.lblSub.Size = new System.Drawing.Size(520, 44);
            this.lblSub.TabIndex = 0;
            this.lblSub.Text = "Subtitle goes here";
            this.lblSub.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlScroll
            // 
            this.pnlScroll.AutoScroll = true;
            this.pnlScroll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.pnlScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScroll.Location = new System.Drawing.Point(0, 96);
            this.pnlScroll.Name = "pnlScroll";
            this.pnlScroll.Padding = new System.Windows.Forms.Padding(16, 12, 16, 8);
            this.pnlScroll.Size = new System.Drawing.Size(520, 246);
            this.pnlScroll.TabIndex = 2;
            this.pnlScroll.Resize += new System.EventHandler(this.pnlScroll_Resize);
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.White;
            this.pnlFooter.Controls.Add(this.btnOk);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 342);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(520, 58);
            this.pnlFooter.TabIndex = 3;
            this.pnlFooter.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlFooter_Paint);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(394, 11);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(110, 36);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // AtRiskPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(520, 400);
            this.Controls.Add(this.pnlScroll);
            this.Controls.Add(this.pnlSubtitle);
            this.Controls.Add(this.pnlTitleBar);
            this.Controls.Add(this.pnlFooter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(520, 600);
            this.MinimumSize = new System.Drawing.Size(520, 260);
            this.Name = "AtRiskPopup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "At-Risk Subjects";
            this.Shown += new System.EventHandler(this.AtRiskPopup_SizeChanged);
            this.Resize += new System.EventHandler(this.AtRiskPopup_SizeChanged);
            this.pnlTitleBar.ResumeLayout(false);
            this.pnlSubtitle.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTitleBar;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlSubtitle;
        private System.Windows.Forms.Label lblSub;
        private System.Windows.Forms.Panel pnlScroll;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnOk;
    }
}