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
            pnlHeader = new Panel();
            lblWelcome = new Label();
            lblStudentName = new Label();
            pnlHeaderRight = new Panel();
            txtSearch = new TextBox();
            pnlNotifBadge = new Panel();
            lblNotifBell = new Label();
            lblNotifCount = new Label();
            pnlStats = new Panel();
            pnlStatTotal = new Panel();
            pnlStatPending = new Panel();
            pnlStatSubmitted = new Panel();
            pnlStatOverdue = new Panel();
            lblTotalValue = new Label();
            lblTotalName = new Label();
            lblPendingValue = new Label();
            lblPendingName = new Label();
            lblSubmittedValue = new Label();
            lblSubmittedName = new Label();
            lblOverdueValue = new Label();
            lblOverdueName = new Label();
            pnlSectionBar = new Panel();
            lblSectionTitle = new Label();
            flpCards = new FlowLayoutPanel();
            pnlHeader.SuspendLayout();
            pnlHeaderRight.SuspendLayout();
            pnlNotifBadge.SuspendLayout();
            pnlStats.SuspendLayout();
            pnlSectionBar.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.Maroon;
            pnlHeader.Controls.Add(lblWelcome);
            pnlHeader.Controls.Add(lblStudentName);
            pnlHeader.Controls.Add(pnlHeaderRight);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1640, 72);
            pnlHeader.TabIndex = 0;
            // 
            // lblWelcome
            // 
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Segoe UI", 9.5F);
            lblWelcome.ForeColor = Color.FromArgb(220, 175, 175);
            lblWelcome.Location = new Point(20, 10);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(95, 17);
            lblWelcome.TabIndex = 0;
            lblWelcome.Text = "Welcome back,";
            // 
            // lblStudentName
            // 
            lblStudentName.AutoSize = true;
            lblStudentName.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblStudentName.ForeColor = Color.White;
            lblStudentName.Location = new Point(18, 28);
            lblStudentName.Name = "lblStudentName";
            lblStudentName.Size = new Size(98, 31);
            lblStudentName.TabIndex = 1;
            lblStudentName.Text = "Student";
            // 
            // pnlHeaderRight
            // 
            pnlHeaderRight.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pnlHeaderRight.BackColor = Color.Transparent;
            pnlHeaderRight.Controls.Add(txtSearch);
            pnlHeaderRight.Controls.Add(pnlNotifBadge);
            pnlHeaderRight.Location = new Point(1250, 14);
            pnlHeaderRight.Name = "pnlHeaderRight";
            pnlHeaderRight.Size = new Size(380, 44);
            pnlHeaderRight.TabIndex = 2;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.Location = new Point(0, 9);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "🔍  Search course or instructor...";
            txtSearch.Size = new Size(275, 25);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // pnlNotifBadge
            // 
            pnlNotifBadge.BackColor = Color.Transparent;
            pnlNotifBadge.Controls.Add(lblNotifBell);
            pnlNotifBadge.Controls.Add(lblNotifCount);
            pnlNotifBadge.Cursor = Cursors.Hand;
            pnlNotifBadge.Location = new Point(316, 6);
            pnlNotifBadge.Name = "pnlNotifBadge";
            pnlNotifBadge.Size = new Size(52, 32);
            pnlNotifBadge.TabIndex = 1;
            pnlNotifBadge.Click += pnlNotifBadge_Click;
            // 
            // lblNotifBell
            // 
            lblNotifBell.Font = new Font("Segoe UI", 16F);
            lblNotifBell.ForeColor = Color.White;
            lblNotifBell.Location = new Point(0, 0);
            lblNotifBell.Name = "lblNotifBell";
            lblNotifBell.Size = new Size(34, 32);
            lblNotifBell.TabIndex = 0;
            lblNotifBell.Text = "🔔";
            lblNotifBell.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblNotifCount
            // 
            lblNotifCount.BackColor = Color.FromArgb(220, 40, 40);
            lblNotifCount.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            lblNotifCount.ForeColor = Color.White;
            lblNotifCount.Location = new Point(22, 0);
            lblNotifCount.Name = "lblNotifCount";
            lblNotifCount.Size = new Size(18, 14);
            lblNotifCount.TabIndex = 1;
            lblNotifCount.Text = "3";
            lblNotifCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlStats
            // 
            pnlStats.BackColor = Color.White;
            pnlStats.Controls.Add(pnlStatTotal);
            pnlStats.Controls.Add(pnlStatPending);
            pnlStats.Controls.Add(pnlStatSubmitted);
            pnlStats.Controls.Add(pnlStatOverdue);
            pnlStats.Dock = DockStyle.Top;
            pnlStats.Location = new Point(0, 72);
            pnlStats.Name = "pnlStats";
            pnlStats.Padding = new Padding(20, 12, 20, 0);
            pnlStats.Size = new Size(1640, 82);
            pnlStats.TabIndex = 1;
            // 
            // pnlStatTotal
            // 
            pnlStatTotal.Location = new Point(0, 0);
            pnlStatTotal.Name = "pnlStatTotal";
            pnlStatTotal.Size = new Size(200, 100);
            pnlStatTotal.TabIndex = 0;
            // 
            // pnlStatPending
            // 
            pnlStatPending.Location = new Point(0, 0);
            pnlStatPending.Name = "pnlStatPending";
            pnlStatPending.Size = new Size(200, 100);
            pnlStatPending.TabIndex = 1;
            // 
            // pnlStatSubmitted
            // 
            pnlStatSubmitted.Location = new Point(0, 0);
            pnlStatSubmitted.Name = "pnlStatSubmitted";
            pnlStatSubmitted.Size = new Size(200, 100);
            pnlStatSubmitted.TabIndex = 2;
            // 
            // pnlStatOverdue
            // 
            pnlStatOverdue.Location = new Point(0, 0);
            pnlStatOverdue.Name = "pnlStatOverdue";
            pnlStatOverdue.Size = new Size(200, 100);
            pnlStatOverdue.TabIndex = 3;
            // 
            // lblTotalValue
            // 
            lblTotalValue.Location = new Point(0, 0);
            lblTotalValue.Name = "lblTotalValue";
            lblTotalValue.Size = new Size(100, 23);
            lblTotalValue.TabIndex = 0;
            // 
            // lblTotalName
            // 
            lblTotalName.Location = new Point(0, 0);
            lblTotalName.Name = "lblTotalName";
            lblTotalName.Size = new Size(100, 23);
            lblTotalName.TabIndex = 0;
            // 
            // lblPendingValue
            // 
            lblPendingValue.Location = new Point(0, 0);
            lblPendingValue.Name = "lblPendingValue";
            lblPendingValue.Size = new Size(100, 23);
            lblPendingValue.TabIndex = 0;
            // 
            // lblPendingName
            // 
            lblPendingName.Location = new Point(0, 0);
            lblPendingName.Name = "lblPendingName";
            lblPendingName.Size = new Size(100, 23);
            lblPendingName.TabIndex = 0;
            // 
            // lblSubmittedValue
            // 
            lblSubmittedValue.Location = new Point(0, 0);
            lblSubmittedValue.Name = "lblSubmittedValue";
            lblSubmittedValue.Size = new Size(100, 23);
            lblSubmittedValue.TabIndex = 0;
            // 
            // lblSubmittedName
            // 
            lblSubmittedName.Location = new Point(0, 0);
            lblSubmittedName.Name = "lblSubmittedName";
            lblSubmittedName.Size = new Size(100, 23);
            lblSubmittedName.TabIndex = 0;
            // 
            // lblOverdueValue
            // 
            lblOverdueValue.Location = new Point(0, 0);
            lblOverdueValue.Name = "lblOverdueValue";
            lblOverdueValue.Size = new Size(100, 23);
            lblOverdueValue.TabIndex = 0;
            // 
            // lblOverdueName
            // 
            lblOverdueName.Location = new Point(0, 0);
            lblOverdueName.Name = "lblOverdueName";
            lblOverdueName.Size = new Size(100, 23);
            lblOverdueName.TabIndex = 0;
            // 
            // pnlSectionBar
            // 
            pnlSectionBar.BackColor = Color.FromArgb(248, 248, 248);
            pnlSectionBar.Controls.Add(lblSectionTitle);
            pnlSectionBar.Dock = DockStyle.Top;
            pnlSectionBar.Location = new Point(0, 154);
            pnlSectionBar.Name = "pnlSectionBar";
            pnlSectionBar.Size = new Size(1640, 38);
            pnlSectionBar.TabIndex = 2;
            // 
            // lblSectionTitle
            // 
            lblSectionTitle.AutoSize = true;
            lblSectionTitle.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblSectionTitle.ForeColor = Color.FromArgb(80, 0, 0);
            lblSectionTitle.Location = new Point(20, 9);
            lblSectionTitle.Name = "lblSectionTitle";
            lblSectionTitle.Size = new Size(86, 19);
            lblSectionTitle.TabIndex = 0;
            lblSectionTitle.Text = "My Courses";
            // 
            // flpCards
            // 
            flpCards.AutoScroll = true;
            flpCards.BackColor = Color.FromArgb(242, 242, 244);
            flpCards.Dock = DockStyle.Fill;
            flpCards.Location = new Point(0, 192);
            flpCards.Name = "flpCards";
            flpCards.Padding = new Padding(16);
            flpCards.Size = new Size(1640, 797);
            flpCards.TabIndex = 3;
            flpCards.SizeChanged += flpCards_SizeChanged;
            // 
            // StudentActivityDashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(242, 242, 244);
            Controls.Add(flpCards);
            Controls.Add(pnlSectionBar);
            Controls.Add(pnlStats);
            Controls.Add(pnlHeader);
            Name = "StudentActivityDashboard";
            Size = new Size(1640, 989);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlHeaderRight.ResumeLayout(false);
            pnlHeaderRight.PerformLayout();
            pnlNotifBadge.ResumeLayout(false);
            pnlStats.ResumeLayout(false);
            pnlSectionBar.ResumeLayout(false);
            pnlSectionBar.PerformLayout();
            ResumeLayout(false);
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