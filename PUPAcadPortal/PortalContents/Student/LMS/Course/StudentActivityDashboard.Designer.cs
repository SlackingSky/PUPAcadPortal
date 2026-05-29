namespace PUPAcadPortal
{
    partial class StudentActivityDashboard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblStudentName = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.pnlSectionBar = new System.Windows.Forms.Panel();
            this.lblSectionTitle = new System.Windows.Forms.Label();
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
            this.flpCards = new System.Windows.Forms.FlowLayoutPanel();

            this.pnlHeader.SuspendLayout();
            this.pnlSectionBar.SuspendLayout();
            this.pnlStats.SuspendLayout();
            this.pnlStatTotal.SuspendLayout();
            this.pnlStatPending.SuspendLayout();
            this.pnlStatSubmitted.SuspendLayout();
            this.pnlStatOverdue.SuspendLayout();
            this.SuspendLayout();

            this.pnlHeader.BackColor = System.Drawing.Color.Maroon;
            this.pnlHeader.Controls.Add(this.lblWelcome);
            this.pnlHeader.Controls.Add(this.lblStudentName);
            this.pnlHeader.Controls.Add(this.txtSearch);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1640, 68);
            this.pnlHeader.TabIndex = 3;

            // lblWelcome
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.lblWelcome.Location = new System.Drawing.Point(16, 10);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(69, 19);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Welcome,";

            // lblStudentName
            this.lblStudentName.AutoSize = true;
            this.lblStudentName.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblStudentName.ForeColor = System.Drawing.Color.White;
            this.lblStudentName.Location = new System.Drawing.Point(14, 28);
            this.lblStudentName.Name = "lblStudentName";
            this.lblStudentName.Size = new System.Drawing.Size(95, 30);
            this.lblStudentName.TabIndex = 1;
            this.lblStudentName.Text = "Student";

            // txtSearch
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.Location = new System.Drawing.Point(1390, 22);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "Search course or instructor...";
            this.txtSearch.Size = new System.Drawing.Size(234, 25);
            this.txtSearch.TabIndex = 2;

            this.pnlSectionBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.pnlSectionBar.Controls.Add(this.lblSectionTitle);
            this.pnlSectionBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSectionBar.Location = new System.Drawing.Point(0, 150);
            this.pnlSectionBar.Name = "pnlSectionBar";
            this.pnlSectionBar.Size = new System.Drawing.Size(1640, 36);
            this.pnlSectionBar.TabIndex = 2;

            // lblSectionTitle
            this.lblSectionTitle.AutoSize = true;
            this.lblSectionTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblSectionTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblSectionTitle.Location = new System.Drawing.Point(16, 8);
            this.lblSectionTitle.Name = "lblSectionTitle";
            this.lblSectionTitle.Size = new System.Drawing.Size(64, 20);
            this.lblSectionTitle.TabIndex = 0;
            this.lblSectionTitle.Text = "Courses";

            this.pnlStats.BackColor = System.Drawing.Color.White;
            this.pnlStats.Controls.Add(this.pnlStatTotal);
            this.pnlStats.Controls.Add(this.pnlStatPending);
            this.pnlStats.Controls.Add(this.pnlStatSubmitted);
            this.pnlStats.Controls.Add(this.pnlStatOverdue);
            this.pnlStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStats.Location = new System.Drawing.Point(0, 68);
            this.pnlStats.Name = "pnlStats";
            this.pnlStats.Padding = new System.Windows.Forms.Padding(16, 10, 16, 0);
            this.pnlStats.Size = new System.Drawing.Size(1640, 82);
            this.pnlStats.TabIndex = 1;

            this.pnlStatTotal.BackColor = System.Drawing.Color.White;
            this.pnlStatTotal.Controls.Add(this.lblTotalValue);
            this.pnlStatTotal.Controls.Add(this.lblTotalName);
            this.pnlStatTotal.Location = new System.Drawing.Point(16, 10);
            this.pnlStatTotal.Name = "pnlStatTotal";
            this.pnlStatTotal.Size = new System.Drawing.Size(160, 60);
            this.pnlStatTotal.TabIndex = 0;

            // lblTotalValue
            this.lblTotalValue.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTotalValue.ForeColor = System.Drawing.Color.Maroon;
            this.lblTotalValue.Location = new System.Drawing.Point(10, 4);
            this.lblTotalValue.Name = "lblTotalValue";
            this.lblTotalValue.Size = new System.Drawing.Size(140, 34);
            this.lblTotalValue.TabIndex = 0;
            this.lblTotalValue.Text = "0";

            // lblTotalName
            this.lblTotalName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTotalName.ForeColor = System.Drawing.Color.Gray;
            this.lblTotalName.Location = new System.Drawing.Point(10, 40);
            this.lblTotalName.Name = "lblTotalName";
            this.lblTotalName.Size = new System.Drawing.Size(140, 16);
            this.lblTotalName.TabIndex = 1;
            this.lblTotalName.Text = "Total Activities";

            this.pnlStatPending.BackColor = System.Drawing.Color.White;
            this.pnlStatPending.Controls.Add(this.lblPendingValue);
            this.pnlStatPending.Controls.Add(this.lblPendingName);
            this.pnlStatPending.Location = new System.Drawing.Point(196, 10);
            this.pnlStatPending.Name = "pnlStatPending";
            this.pnlStatPending.Size = new System.Drawing.Size(160, 60);
            this.pnlStatPending.TabIndex = 1;

            // lblPendingValue
            this.lblPendingValue.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblPendingValue.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblPendingValue.Location = new System.Drawing.Point(10, 4);
            this.lblPendingValue.Name = "lblPendingValue";
            this.lblPendingValue.Size = new System.Drawing.Size(140, 34);
            this.lblPendingValue.TabIndex = 0;
            this.lblPendingValue.Text = "0";

            // lblPendingName
            this.lblPendingName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPendingName.ForeColor = System.Drawing.Color.Gray;
            this.lblPendingName.Location = new System.Drawing.Point(10, 40);
            this.lblPendingName.Name = "lblPendingName";
            this.lblPendingName.Size = new System.Drawing.Size(140, 16);
            this.lblPendingName.TabIndex = 1;
            this.lblPendingName.Text = "Pending";

            this.pnlStatSubmitted.BackColor = System.Drawing.Color.White;
            this.pnlStatSubmitted.Controls.Add(this.lblSubmittedValue);
            this.pnlStatSubmitted.Controls.Add(this.lblSubmittedName);
            this.pnlStatSubmitted.Location = new System.Drawing.Point(376, 10);
            this.pnlStatSubmitted.Name = "pnlStatSubmitted";
            this.pnlStatSubmitted.Size = new System.Drawing.Size(160, 60);
            this.pnlStatSubmitted.TabIndex = 2;

            // lblSubmittedValue
            this.lblSubmittedValue.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblSubmittedValue.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblSubmittedValue.Location = new System.Drawing.Point(10, 4);
            this.lblSubmittedValue.Name = "lblSubmittedValue";
            this.lblSubmittedValue.Size = new System.Drawing.Size(140, 34);
            this.lblSubmittedValue.TabIndex = 0;
            this.lblSubmittedValue.Text = "0";

            // lblSubmittedName
            this.lblSubmittedName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubmittedName.ForeColor = System.Drawing.Color.Gray;
            this.lblSubmittedName.Location = new System.Drawing.Point(10, 40);
            this.lblSubmittedName.Name = "lblSubmittedName";
            this.lblSubmittedName.Size = new System.Drawing.Size(140, 16);
            this.lblSubmittedName.TabIndex = 1;
            this.lblSubmittedName.Text = "Submitted";

            this.pnlStatOverdue.BackColor = System.Drawing.Color.White;
            this.pnlStatOverdue.Controls.Add(this.lblOverdueValue);
            this.pnlStatOverdue.Controls.Add(this.lblOverdueName);
            this.pnlStatOverdue.Location = new System.Drawing.Point(556, 10);
            this.pnlStatOverdue.Name = "pnlStatOverdue";
            this.pnlStatOverdue.Size = new System.Drawing.Size(160, 60);
            this.pnlStatOverdue.TabIndex = 3;

            // lblOverdueValue
            this.lblOverdueValue.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblOverdueValue.ForeColor = System.Drawing.Color.Red;
            this.lblOverdueValue.Location = new System.Drawing.Point(10, 4);
            this.lblOverdueValue.Name = "lblOverdueValue";
            this.lblOverdueValue.Size = new System.Drawing.Size(140, 34);
            this.lblOverdueValue.TabIndex = 0;
            this.lblOverdueValue.Text = "0";

            // lblOverdueName
            this.lblOverdueName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblOverdueName.ForeColor = System.Drawing.Color.Gray;
            this.lblOverdueName.Location = new System.Drawing.Point(10, 40);
            this.lblOverdueName.Name = "lblOverdueName";
            this.lblOverdueName.Size = new System.Drawing.Size(140, 16);
            this.lblOverdueName.TabIndex = 1;
            this.lblOverdueName.Text = "Overdue";

            this.flpCards.AutoScroll = true;
            this.flpCards.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.flpCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpCards.Location = new System.Drawing.Point(0, 186);
            this.flpCards.Name = "flpCards";
            this.flpCards.Padding = new System.Windows.Forms.Padding(16);
            this.flpCards.Size = new System.Drawing.Size(1640, 803);
            this.flpCards.TabIndex = 4;

            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Controls.Add(this.flpCards);
            this.Controls.Add(this.pnlSectionBar);
            this.Controls.Add(this.pnlStats);
            this.Controls.Add(this.pnlHeader);
            this.Name = "StudentActivityDashboard";
            this.Size = new System.Drawing.Size(1640, 989);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlSectionBar.ResumeLayout(false);
            this.pnlSectionBar.PerformLayout();
            this.pnlStats.ResumeLayout(false);
            this.pnlStatTotal.ResumeLayout(false);
            this.pnlStatPending.ResumeLayout(false);
            this.pnlStatSubmitted.ResumeLayout(false);
            this.pnlStatOverdue.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblStudentName;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Panel pnlSectionBar;
        private System.Windows.Forms.Label lblSectionTitle;
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
        private System.Windows.Forms.FlowLayoutPanel flpCards;
    }
}