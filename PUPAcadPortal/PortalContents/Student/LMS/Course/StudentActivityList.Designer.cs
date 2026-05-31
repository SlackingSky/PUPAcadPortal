namespace PUPAcadPortal.PortalContents.Student.LMS.Course
{
    partial class StudentActivityList
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing) { _searchTimer?.Dispose(); components?.Dispose(); }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnBack = new buttonRounded();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblInstructor = new System.Windows.Forms.Label();

            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.cmbFilter = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.pnlSummary = new System.Windows.Forms.Panel();

            this.flp = new System.Windows.Forms.FlowLayoutPanel();

            this.pnlHeader.SuspendLayout();
            this.pnlToolbar.SuspendLayout();
            this.SuspendLayout();

            // ── pnlHeader ──────────────────────────────────────────────────
            this.pnlHeader.BackColor = System.Drawing.Color.Maroon;
            this.pnlHeader.Controls.Add(this.btnBack);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblInstructor);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1640, 64);
            this.pnlHeader.TabIndex = 3;

            this.btnBack.BackColor = System.Drawing.Color.FromArgb(110, 0, 0);
            this.btnBack.BorderRadius = 10;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(12, 16);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(80, 32);
            this.btnBack.Text = "← Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(106, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Text = "Course — Code";

            this.lblInstructor.AutoSize = true;
            this.lblInstructor.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblInstructor.ForeColor = System.Drawing.Color.FromArgb(215, 175, 175);
            this.lblInstructor.Location = new System.Drawing.Point(108, 40);
            this.lblInstructor.Text = "Instructor";

            // ── pnlToolbar ─────────────────────────────────────────────────
            this.pnlToolbar.BackColor = System.Drawing.Color.FromArgb(252, 252, 252);
            this.pnlToolbar.Controls.Add(this.txtSearch);
            this.pnlToolbar.Controls.Add(this.lblFilter);
            this.pnlToolbar.Controls.Add(this.cmbFilter);
            this.pnlToolbar.Controls.Add(this.lblStatus);
            this.pnlToolbar.Controls.Add(this.cmbStatus);
            this.pnlToolbar.Controls.Add(this.pnlSummary);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.Size = new System.Drawing.Size(1640, 54);
            this.pnlToolbar.TabIndex = 2;

            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.Location = new System.Drawing.Point(12, 14);
            this.txtSearch.PlaceholderText = "🔍  Search activities...";
            this.txtSearch.Size = new System.Drawing.Size(230, 26);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);

            this.lblFilter.AutoSize = true;
            this.lblFilter.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblFilter.Location = new System.Drawing.Point(256, 17);
            this.lblFilter.Text = "Type:";

            this.cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilter.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbFilter.FormattingEnabled = true;
            this.cmbFilter.Items.AddRange(new object[] {
                "All", "Quiz", "LongQuiz", "Essay", "FileUpload", "Recitation" });
            this.cmbFilter.Location = new System.Drawing.Point(294, 14);
            this.cmbFilter.Size = new System.Drawing.Size(150, 26);
            this.cmbFilter.SelectedIndexChanged += new System.EventHandler(this.cmbFilter_SelectedIndexChanged);

            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblStatus.Location = new System.Drawing.Point(460, 17);
            this.lblStatus.Text = "Status:";

            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] {
                "All Status", "Pending", "Submitted", "Returned", "Late", "Overdue" });
            this.cmbStatus.Location = new System.Drawing.Point(504, 14);
            this.cmbStatus.Size = new System.Drawing.Size(150, 26);
            this.cmbStatus.SelectedIndexChanged += new System.EventHandler(this.cmbStatus_SelectedIndexChanged);

            // Compact summary pills (right side of toolbar)
            this.pnlSummary.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.pnlSummary.BackColor = System.Drawing.Color.Transparent;
            this.pnlSummary.Location = new System.Drawing.Point(1200, 10);
            this.pnlSummary.Size = new System.Drawing.Size(430, 34);
            this.pnlSummary.Name = "pnlSummary";

            // ── flp ────────────────────────────────────────────────────────
            this.flp.AutoScroll = true;
            this.flp.BackColor = System.Drawing.Color.FromArgb(244, 244, 246);
            this.flp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flp.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flp.WrapContents = false;
            this.flp.Name = "flp";
            this.flp.Padding = new System.Windows.Forms.Padding(12);
            this.flp.TabIndex = 0;
            this.flp.SizeChanged += new System.EventHandler(this.flp_SizeChanged);

            // ── root ───────────────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(244, 244, 246);
            this.Controls.Add(this.flp);
            this.Controls.Add(this.pnlToolbar);
            this.Controls.Add(this.pnlHeader);
            this.Name = "StudentActivityList";
            this.Size = new System.Drawing.Size(1640, 989);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlToolbar.ResumeLayout(false);
            this.pnlToolbar.PerformLayout();
            this.ResumeLayout(false);
        }
        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private buttonRounded btnBack;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblInstructor;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.ComboBox cmbFilter;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.FlowLayoutPanel flp;
    }
}