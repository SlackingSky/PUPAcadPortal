namespace PUPAcadPortal.PortalContents.Student.LMS.Course
{
    partial class StudentActivityDashboard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblStudentName = new System.Windows.Forms.Label();
            this.pnlHeaderRight = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.pnlNotifBadge = new System.Windows.Forms.Panel();
            this.lblNotifCount = new System.Windows.Forms.Label();
            this.lblNotifBell = new System.Windows.Forms.Label();

            this.pnlStats = new System.Windows.Forms.Panel();
            this.pnlStatTotal = new System.Windows.Forms.Panel();
            this.lblTotalValue = new System.Windows.Forms.Label();
            this.lblTotalName = new System.Windows.Forms.Label();
            this.pnlStatPending = new System.Windows.Forms.Panel();
            this.lblPendingValue = new System.Windows.Forms.Label();
            this.lblPendingName = new System.Windows.Forms.Label();
            this.pnlStatSubmitted = new System.Windows.Forms.Panel();
            this.lblSubmittedValue = new System.Windows.Forms.Label();
            this.lblSubmittedName = new System.Windows.Forms.Label();
            this.pnlStatOverdue = new System.Windows.Forms.Panel();
            this.lblOverdueValue = new System.Windows.Forms.Label();
            this.lblOverdueName = new System.Windows.Forms.Label();

            this.pnlSectionBar = new System.Windows.Forms.Panel();
            this.lblSectionTitle = new System.Windows.Forms.Label();

            this.flpCards = new System.Windows.Forms.FlowLayoutPanel();

            this.pnlHeader.SuspendLayout();
            this.pnlStats.SuspendLayout();
            this.pnlSectionBar.SuspendLayout();
            this.SuspendLayout();

            // ── pnlHeader ──────────────────────────────────────────────────
            this.pnlHeader.BackColor = System.Drawing.Color.Maroon;
            this.pnlHeader.Controls.Add(this.lblWelcome);
            this.pnlHeader.Controls.Add(this.lblStudentName);
            this.pnlHeader.Controls.Add(this.pnlHeaderRight);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1640, 72);
            this.pnlHeader.TabIndex = 0;

            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(220, 175, 175);
            this.lblWelcome.Location = new System.Drawing.Point(20, 10);
            this.lblWelcome.Text = "Welcome back,";

            this.lblStudentName.AutoSize = true;
            this.lblStudentName.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Bold);
            this.lblStudentName.ForeColor = System.Drawing.Color.White;
            this.lblStudentName.Location = new System.Drawing.Point(18, 28);
            this.lblStudentName.Text = "Student";

            // right-side cluster
            this.pnlHeaderRight.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.pnlHeaderRight.BackColor = System.Drawing.Color.Transparent;
            this.pnlHeaderRight.Controls.Add(this.txtSearch);
            this.pnlHeaderRight.Controls.Add(this.pnlNotifBadge);
            this.pnlHeaderRight.Location = new System.Drawing.Point(1250, 14);
            this.pnlHeaderRight.Size = new System.Drawing.Size(380, 44);
            this.pnlHeaderRight.Name = "pnlHeaderRight";

            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.Location = new System.Drawing.Point(0, 9);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "🔍  Search course or instructor...";
            this.txtSearch.Size = new System.Drawing.Size(300, 26);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);

            this.pnlNotifBadge.Location = new System.Drawing.Point(316, 6);
            this.pnlNotifBadge.Size = new System.Drawing.Size(52, 32);
            this.pnlNotifBadge.BackColor = System.Drawing.Color.Transparent;
            this.pnlNotifBadge.Controls.Add(this.lblNotifBell);
            this.pnlNotifBadge.Controls.Add(this.lblNotifCount);
            this.pnlNotifBadge.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlNotifBadge.Click += new System.EventHandler(this.pnlNotifBadge_Click);

            this.lblNotifBell.Text = "🔔";
            this.lblNotifBell.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.lblNotifBell.ForeColor = System.Drawing.Color.White;
            this.lblNotifBell.Location = new System.Drawing.Point(0, 0);
            this.lblNotifBell.Size = new System.Drawing.Size(34, 32);
            this.lblNotifBell.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblNotifCount.Text = "3";
            this.lblNotifCount.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Bold);
            this.lblNotifCount.ForeColor = System.Drawing.Color.White;
            this.lblNotifCount.BackColor = System.Drawing.Color.FromArgb(220, 40, 40);
            this.lblNotifCount.Location = new System.Drawing.Point(22, 0);
            this.lblNotifCount.Size = new System.Drawing.Size(18, 14);
            this.lblNotifCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ── pnlStats ───────────────────────────────────────────────────
            this.pnlStats.BackColor = System.Drawing.Color.White;
            this.pnlStats.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.pnlStatTotal, this.pnlStatPending,
                this.pnlStatSubmitted, this.pnlStatOverdue });
            this.pnlStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStats.Name = "pnlStats";
            this.pnlStats.Padding = new System.Windows.Forms.Padding(20, 12, 20, 0);
            this.pnlStats.Size = new System.Drawing.Size(1640, 82);
            this.pnlStats.TabIndex = 1;

            // stat panels helper
            SetupStatPanel(this.pnlStatTotal, this.lblTotalValue, this.lblTotalName,
                           16, "0", "Total Activities", System.Drawing.Color.Maroon);
            SetupStatPanel(this.pnlStatPending, this.lblPendingValue, this.lblPendingName,
                           206, "0", "Pending", System.Drawing.Color.OrangeRed);
            SetupStatPanel(this.pnlStatSubmitted, this.lblSubmittedValue, this.lblSubmittedName,
                           396, "0", "Submitted", System.Drawing.Color.FromArgb(27, 120, 27));
            SetupStatPanel(this.pnlStatOverdue, this.lblOverdueValue, this.lblOverdueName,
                           586, "0", "Overdue", System.Drawing.Color.FromArgb(180, 30, 30));

            // ── pnlSectionBar ──────────────────────────────────────────────
            this.pnlSectionBar.BackColor = System.Drawing.Color.FromArgb(248, 248, 248);
            this.pnlSectionBar.Controls.Add(this.lblSectionTitle);
            this.pnlSectionBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSectionBar.Name = "pnlSectionBar";
            this.pnlSectionBar.Size = new System.Drawing.Size(1640, 38);
            this.pnlSectionBar.TabIndex = 2;

            this.lblSectionTitle.AutoSize = true;
            this.lblSectionTitle.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblSectionTitle.ForeColor = System.Drawing.Color.FromArgb(80, 0, 0);
            this.lblSectionTitle.Location = new System.Drawing.Point(20, 9);
            this.lblSectionTitle.Text = "My Courses";

            // ── flpCards ───────────────────────────────────────────────────
            this.flpCards.AutoScroll = true;
            this.flpCards.BackColor = System.Drawing.Color.FromArgb(242, 242, 244);
            this.flpCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpCards.Name = "flpCards";
            this.flpCards.Padding = new System.Windows.Forms.Padding(16);
            this.flpCards.TabIndex = 3;
            this.flpCards.SizeChanged += new System.EventHandler(this.flpCards_SizeChanged);

            // ── root ───────────────────────────────────────────────────────
            this.BackColor = System.Drawing.Color.FromArgb(242, 242, 244);
            this.Controls.Add(this.flpCards);
            this.Controls.Add(this.pnlSectionBar);
            this.Controls.Add(this.pnlStats);
            this.Controls.Add(this.pnlHeader);
            this.Name = "StudentActivityDashboard";
            this.Size = new System.Drawing.Size(1640, 989);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlStats.ResumeLayout(false);
            this.pnlSectionBar.ResumeLayout(false);
            this.pnlSectionBar.PerformLayout();
            this.ResumeLayout(false);
        }

        private void SetupStatPanel(
            System.Windows.Forms.Panel pnl,
            System.Windows.Forms.Label lblVal,
            System.Windows.Forms.Label lblName,
            int x, string val, string name, System.Drawing.Color color)
        {
            pnl.BackColor = System.Drawing.Color.White;
            pnl.Controls.Add(lblVal);
            pnl.Controls.Add(lblName);
            pnl.Location = new System.Drawing.Point(x, 10);
            pnl.Size = new System.Drawing.Size(178, 62);

            lblVal.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            lblVal.ForeColor = color;
            lblVal.Location = new System.Drawing.Point(8, 2);
            lblVal.Size = new System.Drawing.Size(162, 38);
            lblVal.Text = val;

            lblName.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            lblName.ForeColor = System.Drawing.Color.FromArgb(120, 120, 120);
            lblName.Location = new System.Drawing.Point(8, 42);
            lblName.Size = new System.Drawing.Size(162, 16);
            lblName.Text = name;
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblStudentName;
        private System.Windows.Forms.Panel pnlHeaderRight;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Panel pnlNotifBadge;
        private System.Windows.Forms.Label lblNotifBell;
        private System.Windows.Forms.Label lblNotifCount;

        private System.Windows.Forms.Panel pnlStats;
        private System.Windows.Forms.Panel pnlStatTotal;
        private System.Windows.Forms.Label lblTotalValue;
        private System.Windows.Forms.Label lblTotalName;
        private System.Windows.Forms.Panel pnlStatPending;
        private System.Windows.Forms.Label lblPendingValue;
        private System.Windows.Forms.Label lblPendingName;
        private System.Windows.Forms.Panel pnlStatSubmitted;
        private System.Windows.Forms.Label lblSubmittedValue;
        private System.Windows.Forms.Label lblSubmittedName;
        private System.Windows.Forms.Panel pnlStatOverdue;
        private System.Windows.Forms.Label lblOverdueValue;
        private System.Windows.Forms.Label lblOverdueName;

        private System.Windows.Forms.Panel pnlSectionBar;
        private System.Windows.Forms.Label lblSectionTitle;
        private System.Windows.Forms.FlowLayoutPanel flpCards;
    }
}