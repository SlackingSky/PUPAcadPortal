namespace PUPAcadPortal
{
    partial class AssignmentManagement
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlHeader = new System.Windows.Forms.Panel();
            btnBack = new buttonRounded();
            lblCourseName = new System.Windows.Forms.Label();
            lblCourseCode = new System.Windows.Forms.Label();
            btnSave = new buttonRounded();
            pnlToolbar = new System.Windows.Forms.Panel();
            txtSearch = new System.Windows.Forms.TextBox();
            cmbFilterType = new System.Windows.Forms.ComboBox();
            pnlSummaryBar = new System.Windows.Forms.Panel();
            lblSummaryBar = new System.Windows.Forms.Label();
            pnlScroll = new System.Windows.Forms.Panel();
            flpActivities = new System.Windows.Forms.FlowLayoutPanel();

            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            pnlSummaryBar.SuspendLayout();
            pnlScroll.SuspendLayout();
            SuspendLayout();

            // ── pnlHeader ─────────────────────────────────────────────────────
            pnlHeader.BackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            pnlHeader.Controls.Add(btnBack);
            pnlHeader.Controls.Add(lblCourseName);
            pnlHeader.Controls.Add(lblCourseCode);
            pnlHeader.Controls.Add(btnSave);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(1680, 68);
            pnlHeader.TabIndex = 0;
            pnlHeader.SizeChanged += pnlHeader_SizeChanged;

            // btnBack
            btnBack.BackColor = System.Drawing.Color.FromArgb(100, 0, 0);
            btnBack.BorderRadius = 10;
            btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnBack.ForeColor = System.Drawing.Color.White;
            btnBack.Location = new System.Drawing.Point(12, 18);
            btnBack.Name = "btnBack";
            btnBack.Size = new System.Drawing.Size(80, 32);
            btnBack.TabIndex = 0;
            btnBack.Text = "← Back";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += btnBack_Click;

            // lblCourseName
            lblCourseName.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            lblCourseName.ForeColor = System.Drawing.Color.White;
            lblCourseName.Location = new System.Drawing.Point(106, 10);
            lblCourseName.Name = "lblCourseName";
            lblCourseName.Size = new System.Drawing.Size(700, 28);
            lblCourseName.TabIndex = 1;
            lblCourseName.Text = "Course Name";

            // lblCourseCode
            lblCourseCode.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            lblCourseCode.ForeColor = System.Drawing.Color.FromArgb(230, 185, 185);
            lblCourseCode.Location = new System.Drawing.Point(106, 40);
            lblCourseCode.Name = "lblCourseCode";
            lblCourseCode.Size = new System.Drawing.Size(500, 18);
            lblCourseCode.TabIndex = 2;
            lblCourseCode.Text = "Course Code";

            // btnSave
            btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnSave.BackColor = System.Drawing.Color.FromArgb(255, 196, 0);
            btnSave.BorderRadius = 10;
            btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnSave.ForeColor = System.Drawing.Color.Black;
            btnSave.Location = new System.Drawing.Point(1510, 17);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(158, 34);
            btnSave.TabIndex = 3;
            btnSave.Text = "+ Create Activity";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;

            // ── pnlToolbar ────────────────────────────────────────────────────
            pnlToolbar.BackColor = System.Drawing.Color.White;
            pnlToolbar.Controls.Add(txtSearch);
            pnlToolbar.Controls.Add(cmbFilterType);
            pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            pnlToolbar.Location = new System.Drawing.Point(0, 68);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Padding = new System.Windows.Forms.Padding(14, 10, 14, 10);
            pnlToolbar.Size = new System.Drawing.Size(1680, 50);
            pnlToolbar.TabIndex = 1;

            txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtSearch.Location = new System.Drawing.Point(14, 12);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "🔍  Search activities...";
            txtSearch.Size = new System.Drawing.Size(240, 26);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += txtSearch_TextChanged;

            cmbFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbFilterType.Font = new System.Drawing.Font("Segoe UI", 10F);
            cmbFilterType.Items.AddRange(new object[] { "All", "Assignment", "Quiz", "Essay", "FileUpload" });
            cmbFilterType.Location = new System.Drawing.Point(268, 12);
            cmbFilterType.Name = "cmbFilterType";
            cmbFilterType.Size = new System.Drawing.Size(150, 26);
            cmbFilterType.TabIndex = 1;
            cmbFilterType.SelectedIndex = 0;
            cmbFilterType.SelectedIndexChanged += cmbFilterType_SelectedIndexChanged;

            // ── pnlSummaryBar ─────────────────────────────────────────────────
            pnlSummaryBar.BackColor = System.Drawing.Color.FromArgb(241, 241, 246);
            pnlSummaryBar.Controls.Add(lblSummaryBar);
            pnlSummaryBar.Dock = System.Windows.Forms.DockStyle.Top;
            pnlSummaryBar.Location = new System.Drawing.Point(0, 118);
            pnlSummaryBar.Name = "pnlSummaryBar";
            pnlSummaryBar.Size = new System.Drawing.Size(1680, 30);
            pnlSummaryBar.TabIndex = 2;

            lblSummaryBar.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            lblSummaryBar.ForeColor = System.Drawing.Color.FromArgb(90, 90, 100);
            lblSummaryBar.Location = new System.Drawing.Point(18, 7);
            lblSummaryBar.Name = "lblSummaryBar";
            lblSummaryBar.Size = new System.Drawing.Size(900, 18);
            lblSummaryBar.TabIndex = 0;

            // ── pnlScroll ─────────────────────────────────────────────────────
            pnlScroll.AutoScroll = true;
            pnlScroll.BackColor = System.Drawing.Color.FromArgb(245, 245, 248);
            pnlScroll.Controls.Add(flpActivities);
            pnlScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlScroll.Location = new System.Drawing.Point(0, 148);
            pnlScroll.Name = "pnlScroll";
            pnlScroll.Size = new System.Drawing.Size(1680, 841);
            pnlScroll.TabIndex = 3;

            flpActivities.AutoSize = true;
            flpActivities.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            flpActivities.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flpActivities.Dock = System.Windows.Forms.DockStyle.Top;
            flpActivities.Location = new System.Drawing.Point(0, 0);
            flpActivities.Name = "flpActivities";
            flpActivities.Padding = new System.Windows.Forms.Padding(20, 16, 20, 20);
            flpActivities.WrapContents = false;
            flpActivities.TabIndex = 0;

            // ── AssignmentManagement ──────────────────────────────────────────
            Controls.Add(pnlScroll);
            Controls.Add(pnlSummaryBar);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            Name = "AssignmentManagement";
            Size = new System.Drawing.Size(1680, 989);

            pnlHeader.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            pnlSummaryBar.ResumeLayout(false);
            pnlScroll.ResumeLayout(false);
            pnlScroll.PerformLayout();
            ResumeLayout(false);
        }

        // ── Responsive header ──
        private void pnlHeader_SizeChanged(object sender, System.EventArgs e)
        {
            if (btnSave != null && pnlHeader != null)
                btnSave.Location = new System.Drawing.Point(pnlHeader.Width - btnSave.Width - 12, 17);
        }

        // ── Field declarations ─────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlHeader;
        private buttonRounded btnBack;
        private System.Windows.Forms.Label lblCourseName;
        private System.Windows.Forms.Label lblCourseCode;
        private buttonRounded btnSave;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ComboBox cmbFilterType;
        private System.Windows.Forms.Panel pnlSummaryBar;
        private System.Windows.Forms.Label lblSummaryBar;
        private System.Windows.Forms.Panel pnlScroll;
        private System.Windows.Forms.FlowLayoutPanel flpActivities;
    }
}