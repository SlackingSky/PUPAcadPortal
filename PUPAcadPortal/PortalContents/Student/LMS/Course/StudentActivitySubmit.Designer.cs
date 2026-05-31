namespace PUPAcadPortal.PortalContents.Student.LMS.Course
{
    partial class StudentActivitySubmit
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _countdownTimer?.Dispose();
                _autosaveTimer?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnBack = new buttonRounded();
            this.lblActivityTitle = new System.Windows.Forms.Label();
            this.lblMeta = new System.Windows.Forms.Label();
            this.pnlBody = new System.Windows.Forms.Panel();

            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();

            // ── pnlHeader ──────────────────────────────────────────────────
            this.pnlHeader.BackColor = System.Drawing.Color.Maroon;
            this.pnlHeader.Controls.Add(this.btnBack);
            this.pnlHeader.Controls.Add(this.lblActivityTitle);
            this.pnlHeader.Controls.Add(this.lblMeta);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1640, 62);
            this.pnlHeader.TabIndex = 2;

            this.btnBack.BackColor = System.Drawing.Color.FromArgb(110, 0, 0);
            this.btnBack.BorderRadius = 10;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(12, 15);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(80, 32);
            this.btnBack.Text = "← Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);

            this.lblActivityTitle.AutoSize = true;
            this.lblActivityTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblActivityTitle.ForeColor = System.Drawing.Color.White;
            this.lblActivityTitle.Location = new System.Drawing.Point(106, 8);
            this.lblActivityTitle.Text = "Activity Title";

            this.lblMeta.AutoSize = true;
            this.lblMeta.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblMeta.ForeColor = System.Drawing.Color.FromArgb(215, 175, 175);
            this.lblMeta.Location = new System.Drawing.Point(108, 38);
            this.lblMeta.Text = "Type  ·  0 pts  ·  Due —";

            // ── pnlBody ────────────────────────────────────────────────────
            this.pnlBody.AutoScroll = true;
            this.pnlBody.BackColor = System.Drawing.Color.FromArgb(246, 246, 248);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Padding = new System.Windows.Forms.Padding(10);
            this.pnlBody.TabIndex = 0;

            // ── root ───────────────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(246, 246, 248);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlHeader);
            this.Name = "StudentActivitySubmit";
            this.Size = new System.Drawing.Size(1640, 989);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);
        }
        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private buttonRounded btnBack;
        private System.Windows.Forms.Label lblActivityTitle;
        private System.Windows.Forms.Label lblMeta;
        private System.Windows.Forms.Panel pnlBody;
    }
}