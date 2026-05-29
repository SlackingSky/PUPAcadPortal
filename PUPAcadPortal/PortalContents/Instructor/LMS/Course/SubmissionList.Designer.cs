namespace PUPAcadPortal
{
    partial class SubmissionList
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
            btnReturnAll = new buttonRounded();
            lblActivityTitle = new System.Windows.Forms.Label();
            lblActivityType = new System.Windows.Forms.Label();
            lblMaxPoints = new System.Windows.Forms.Label();
            pnlStatsBar = new System.Windows.Forms.Panel();
            lblStats = new System.Windows.Forms.Label();
            pnlToolbar = new System.Windows.Forms.Panel();
            txtSearchStudent = new System.Windows.Forms.TextBox();
            lblSortLbl = new System.Windows.Forms.Label();
            cmbSortBy = new System.Windows.Forms.ComboBox();
            lblFilterLbl = new System.Windows.Forms.Label();
            cmbFilterStatus = new System.Windows.Forms.ComboBox();
            flpSubmissions = new System.Windows.Forms.FlowLayoutPanel();

            pnlHeader.SuspendLayout();
            pnlStatsBar.SuspendLayout();
            pnlToolbar.SuspendLayout();
            SuspendLayout();

            // ── pnlHeader ─────────────────────────────────────────────────────
            pnlHeader.BackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            pnlHeader.Controls.Add(btnBack);
            pnlHeader.Controls.Add(btnReturnAll);
            pnlHeader.Controls.Add(lblActivityTitle);
            pnlHeader.Controls.Add(lblActivityType);
            pnlHeader.Controls.Add(lblMaxPoints);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(1680, 68);
            pnlHeader.TabIndex = 3;
            pnlHeader.SizeChanged += pnlHeader_SizeChanged;

            btnBack.BackColor = System.Drawing.Color.FromArgb(100, 0, 0);
            btnBack.BorderRadius = 10;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnBack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnBack.ForeColor = System.Drawing.Color.White;
            btnBack.Location = new System.Drawing.Point(10, 18);
            btnBack.Name = "btnBack";
            btnBack.Size = new System.Drawing.Size(80, 32);
            btnBack.TabIndex = 0;
            btnBack.Text = "← Back";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += btnBack_Click;

            btnReturnAll.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnReturnAll.BackColor = System.Drawing.Color.DarkOrange;
            btnReturnAll.BorderRadius = 10;
            btnReturnAll.FlatAppearance.BorderSize = 0;
            btnReturnAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnReturnAll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnReturnAll.ForeColor = System.Drawing.Color.White;
            btnReturnAll.Location = new System.Drawing.Point(1548, 18);
            btnReturnAll.Name = "btnReturnAll";
            btnReturnAll.Size = new System.Drawing.Size(120, 32);
            btnReturnAll.TabIndex = 4;
            btnReturnAll.Text = "Return All";
            btnReturnAll.UseVisualStyleBackColor = false;
            btnReturnAll.Click += btnReturnAll_Click;

            lblActivityTitle.AutoEllipsis = true;
            lblActivityTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            lblActivityTitle.ForeColor = System.Drawing.Color.White;
            lblActivityTitle.Location = new System.Drawing.Point(104, 8);
            lblActivityTitle.Name = "lblActivityTitle";
            lblActivityTitle.Size = new System.Drawing.Size(700, 30);
            lblActivityTitle.TabIndex = 1;

            lblActivityType.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblActivityType.ForeColor = System.Drawing.Color.FromArgb(225, 185, 185);
            lblActivityType.Location = new System.Drawing.Point(104, 40);
            lblActivityType.Name = "lblActivityType";
            lblActivityType.Size = new System.Drawing.Size(160, 20);
            lblActivityType.TabIndex = 2;

            lblMaxPoints.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblMaxPoints.ForeColor = System.Drawing.Color.FromArgb(255, 196, 0);
            lblMaxPoints.Location = new System.Drawing.Point(278, 40);
            lblMaxPoints.Name = "lblMaxPoints";
            lblMaxPoints.Size = new System.Drawing.Size(160, 20);
            lblMaxPoints.TabIndex = 3;

            // ── pnlStatsBar ───────────────────────────────────────────────────
            pnlStatsBar.BackColor = System.Drawing.Color.FromArgb(241, 241, 246);
            pnlStatsBar.Controls.Add(lblStats);
            pnlStatsBar.Dock = System.Windows.Forms.DockStyle.Top;
            pnlStatsBar.Location = new System.Drawing.Point(0, 68);
            pnlStatsBar.Name = "pnlStatsBar";
            pnlStatsBar.Size = new System.Drawing.Size(1680, 30);
            pnlStatsBar.TabIndex = 2;

            lblStats.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            lblStats.ForeColor = System.Drawing.Color.FromArgb(80, 80, 90);
            lblStats.Location = new System.Drawing.Point(16, 7);
            lblStats.Name = "lblStats";
            lblStats.Size = new System.Drawing.Size(900, 18);
            lblStats.TabIndex = 0;
            lblStats.Text = "Submitted: 0  ·  Late: 0  ·  Missing: 0  ·  Checked: 0";

            // ── pnlToolbar ────────────────────────────────────────────────────
            pnlToolbar.BackColor = System.Drawing.Color.White;
            pnlToolbar.Controls.Add(txtSearchStudent);
            pnlToolbar.Controls.Add(lblSortLbl);
            pnlToolbar.Controls.Add(cmbSortBy);
            pnlToolbar.Controls.Add(lblFilterLbl);
            pnlToolbar.Controls.Add(cmbFilterStatus);
            pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            pnlToolbar.Location = new System.Drawing.Point(0, 98);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Padding = new System.Windows.Forms.Padding(14, 10, 14, 10);
            pnlToolbar.Size = new System.Drawing.Size(1680, 50);
            pnlToolbar.TabIndex = 1;

            txtSearchStudent.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtSearchStudent.Location = new System.Drawing.Point(14, 12);
            txtSearchStudent.Name = "txtSearchStudent";
            txtSearchStudent.PlaceholderText = "🔍  Search student...";
            txtSearchStudent.Size = new System.Drawing.Size(230, 26);
            txtSearchStudent.TabIndex = 0;
            txtSearchStudent.TextChanged += txtSearchStudent_TextChanged;

            lblSortLbl.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblSortLbl.Location = new System.Drawing.Point(260, 15);
            lblSortLbl.Name = "lblSortLbl";
            lblSortLbl.Size = new System.Drawing.Size(40, 22);
            lblSortLbl.TabIndex = 1;
            lblSortLbl.Text = "Sort:";

            cmbSortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbSortBy.Font = new System.Drawing.Font("Segoe UI", 10F);
            cmbSortBy.Items.AddRange(new object[] { "Name", "Time", "Score" });
            cmbSortBy.Location = new System.Drawing.Point(303, 12);
            cmbSortBy.Name = "cmbSortBy";
            cmbSortBy.Size = new System.Drawing.Size(130, 26);
            cmbSortBy.TabIndex = 2;
            cmbSortBy.SelectedIndex = 0;
            cmbSortBy.SelectedIndexChanged += cmbSortBy_SelectedIndexChanged;

            lblFilterLbl.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblFilterLbl.Location = new System.Drawing.Point(448, 15);
            lblFilterLbl.Name = "lblFilterLbl";
            lblFilterLbl.Size = new System.Drawing.Size(60, 22);
            lblFilterLbl.TabIndex = 3;
            lblFilterLbl.Text = "Status:";

            cmbFilterStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbFilterStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            cmbFilterStatus.Items.AddRange(new object[] { "All", "Submitted", "Late", "Missing", "Returned" });
            cmbFilterStatus.Location = new System.Drawing.Point(511, 12);
            cmbFilterStatus.Name = "cmbFilterStatus";
            cmbFilterStatus.Size = new System.Drawing.Size(140, 26);
            cmbFilterStatus.TabIndex = 4;
            cmbFilterStatus.SelectedIndex = 0;
            cmbFilterStatus.SelectedIndexChanged += cmbFilterStatus_SelectedIndexChanged;

            // ── flpSubmissions ────────────────────────────────────────────────
            flpSubmissions.AutoScroll = true;
            flpSubmissions.BackColor = System.Drawing.Color.FromArgb(245, 245, 248);
            flpSubmissions.Dock = System.Windows.Forms.DockStyle.Fill;
            flpSubmissions.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flpSubmissions.Location = new System.Drawing.Point(0, 148);
            flpSubmissions.Name = "flpSubmissions";
            flpSubmissions.Padding = new System.Windows.Forms.Padding(10);
            flpSubmissions.WrapContents = false;
            flpSubmissions.TabIndex = 0;

            // ── SubmissionList ────────────────────────────────────────────────
            Controls.Add(flpSubmissions);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlStatsBar);
            Controls.Add(pnlHeader);
            Name = "SubmissionList";
            Size = new System.Drawing.Size(1680, 989);

            pnlHeader.ResumeLayout(false);
            pnlStatsBar.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            ResumeLayout(false);
        }

        private void pnlHeader_SizeChanged(object sender, System.EventArgs e)
        {
            if (btnReturnAll != null && pnlHeader != null)
                btnReturnAll.Location = new System.Drawing.Point(pnlHeader.Width - btnReturnAll.Width - 12, 18);
        }

        // ── Field declarations ─────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlHeader;
        private buttonRounded btnBack;
        private buttonRounded btnReturnAll;
        private System.Windows.Forms.Label lblActivityTitle;
        private System.Windows.Forms.Label lblActivityType;
        private System.Windows.Forms.Label lblMaxPoints;
        private System.Windows.Forms.Panel pnlStatsBar;
        private System.Windows.Forms.Label lblStats;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.TextBox txtSearchStudent;
        private System.Windows.Forms.Label lblSortLbl;
        private System.Windows.Forms.ComboBox cmbSortBy;
        private System.Windows.Forms.Label lblFilterLbl;
        private System.Windows.Forms.ComboBox cmbFilterStatus;
        private System.Windows.Forms.FlowLayoutPanel flpSubmissions;
    }
}