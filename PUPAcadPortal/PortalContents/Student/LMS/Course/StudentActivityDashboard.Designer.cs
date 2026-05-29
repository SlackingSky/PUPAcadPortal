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
            pnlHeader = new Panel();
            lblTitle = new Label();
            txtSearch = new TextBox();
            pnlStats = new Panel();
            pnlStatTotal = new Panel();
            lblTotalValue = new Label();
            lblTotalName = new Label();
            pnlStatPending = new Panel();
            lblPendingValue = new Label();
            lblPendingName = new Label();
            pnlStatSubmitted = new Panel();
            lblSubmittedValue = new Label();
            lblSubmittedName = new Label();
            pnlStatOverdue = new Panel();
            lblOverdueValue = new Label();
            lblOverdueName = new Label();
            flpCards = new FlowLayoutPanel();
            pnlHeader.SuspendLayout();
            pnlStats.SuspendLayout();
            pnlStatTotal.SuspendLayout();
            pnlStatPending.SuspendLayout();
            pnlStatSubmitted.SuspendLayout();
            pnlStatOverdue.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.Maroon;
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(txtSearch);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1640, 50);
            pnlHeader.TabIndex = 2;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(16, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(124, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "My Activities";
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.Location = new Point(1391, 12);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Search course...";
            txtSearch.Size = new Size(220, 25);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // pnlStats
            // 
            pnlStats.BackColor = Color.White;
            pnlStats.Controls.Add(pnlStatTotal);
            pnlStats.Controls.Add(pnlStatPending);
            pnlStats.Controls.Add(pnlStatSubmitted);
            pnlStats.Controls.Add(pnlStatOverdue);
            pnlStats.Dock = DockStyle.Top;
            pnlStats.Location = new Point(0, 50);
            pnlStats.Name = "pnlStats";
            pnlStats.Padding = new Padding(16, 8, 16, 0);
            pnlStats.Size = new Size(1640, 80);
            pnlStats.TabIndex = 1;
            // 
            // pnlStatTotal
            // 
            pnlStatTotal.BackColor = Color.White;
            pnlStatTotal.Controls.Add(lblTotalValue);
            pnlStatTotal.Controls.Add(lblTotalName);
            pnlStatTotal.Location = new Point(16, 8);
            pnlStatTotal.Name = "pnlStatTotal";
            pnlStatTotal.Size = new Size(160, 60);
            pnlStatTotal.TabIndex = 0;
            // 
            // lblTotalValue
            // 
            lblTotalValue.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTotalValue.ForeColor = Color.Maroon;
            lblTotalValue.Location = new Point(10, 5);
            lblTotalValue.Name = "lblTotalValue";
            lblTotalValue.Size = new Size(90, 36);
            lblTotalValue.TabIndex = 0;
            lblTotalValue.Text = "0";
            // 
            // lblTotalName
            // 
            lblTotalName.Font = new Font("Segoe UI", 9F);
            lblTotalName.ForeColor = Color.Gray;
            lblTotalName.Location = new Point(7, 39);
            lblTotalName.Name = "lblTotalName";
            lblTotalName.Size = new Size(150, 18);
            lblTotalName.TabIndex = 1;
            lblTotalName.Text = "Total Activities";
            // 
            // pnlStatPending
            // 
            pnlStatPending.BackColor = Color.White;
            pnlStatPending.Controls.Add(lblPendingValue);
            pnlStatPending.Controls.Add(lblPendingName);
            pnlStatPending.Location = new Point(200, 8);
            pnlStatPending.Name = "pnlStatPending";
            pnlStatPending.Size = new Size(160, 60);
            pnlStatPending.TabIndex = 1;
            // 
            // lblPendingValue
            // 
            lblPendingValue.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblPendingValue.ForeColor = Color.OrangeRed;
            lblPendingValue.Location = new Point(10, 5);
            lblPendingValue.Name = "lblPendingValue";
            lblPendingValue.Size = new Size(90, 36);
            lblPendingValue.TabIndex = 0;
            lblPendingValue.Text = "0";
            // 
            // lblPendingName
            // 
            lblPendingName.Font = new Font("Segoe UI", 9F);
            lblPendingName.ForeColor = Color.Gray;
            lblPendingName.Location = new Point(8, 38);
            lblPendingName.Name = "lblPendingName";
            lblPendingName.Size = new Size(150, 18);
            lblPendingName.TabIndex = 1;
            lblPendingName.Text = "Pending";
            // 
            // pnlStatSubmitted
            // 
            pnlStatSubmitted.BackColor = Color.White;
            pnlStatSubmitted.Controls.Add(lblSubmittedValue);
            pnlStatSubmitted.Controls.Add(lblSubmittedName);
            pnlStatSubmitted.Location = new Point(384, 8);
            pnlStatSubmitted.Name = "pnlStatSubmitted";
            pnlStatSubmitted.Size = new Size(160, 60);
            pnlStatSubmitted.TabIndex = 2;
            // 
            // lblSubmittedValue
            // 
            lblSubmittedValue.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblSubmittedValue.ForeColor = Color.ForestGreen;
            lblSubmittedValue.Location = new Point(10, 5);
            lblSubmittedValue.Name = "lblSubmittedValue";
            lblSubmittedValue.Size = new Size(90, 36);
            lblSubmittedValue.TabIndex = 0;
            lblSubmittedValue.Text = "0";
            // 
            // lblSubmittedName
            // 
            lblSubmittedName.Font = new Font("Segoe UI", 9F);
            lblSubmittedName.ForeColor = Color.Gray;
            lblSubmittedName.Location = new Point(7, 39);
            lblSubmittedName.Name = "lblSubmittedName";
            lblSubmittedName.Size = new Size(150, 18);
            lblSubmittedName.TabIndex = 1;
            lblSubmittedName.Text = "Submitted";
            // 
            // pnlStatOverdue
            // 
            pnlStatOverdue.BackColor = Color.White;
            pnlStatOverdue.Controls.Add(lblOverdueValue);
            pnlStatOverdue.Controls.Add(lblOverdueName);
            pnlStatOverdue.Location = new Point(568, 8);
            pnlStatOverdue.Name = "pnlStatOverdue";
            pnlStatOverdue.Size = new Size(160, 60);
            pnlStatOverdue.TabIndex = 3;
            // 
            // lblOverdueValue
            // 
            lblOverdueValue.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblOverdueValue.ForeColor = Color.Red;
            lblOverdueValue.Location = new Point(10, 5);
            lblOverdueValue.Name = "lblOverdueValue";
            lblOverdueValue.Size = new Size(90, 36);
            lblOverdueValue.TabIndex = 0;
            lblOverdueValue.Text = "0";
            // 
            // lblOverdueName
            // 
            lblOverdueName.Font = new Font("Segoe UI", 9F);
            lblOverdueName.ForeColor = Color.Gray;
            lblOverdueName.Location = new Point(7, 38);
            lblOverdueName.Name = "lblOverdueName";
            lblOverdueName.Size = new Size(150, 18);
            lblOverdueName.TabIndex = 1;
            lblOverdueName.Text = "Overdue";
            // 
            // flpCards
            // 
            flpCards.AutoScroll = true;
            flpCards.BackColor = Color.FromArgb(240, 240, 240);
            flpCards.Dock = DockStyle.Fill;
            flpCards.Location = new Point(0, 130);
            flpCards.Name = "flpCards";
            flpCards.Padding = new Padding(16);
            flpCards.Size = new Size(1640, 859);
            flpCards.TabIndex = 0;
            // 
            // StudentActivityDashboard
            // 
            BackColor = Color.FromArgb(245, 245, 245);
            Controls.Add(flpCards);
            Controls.Add(pnlStats);
            Controls.Add(pnlHeader);
            Name = "StudentActivityDashboard";
            Size = new Size(1640, 989);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlStats.ResumeLayout(false);
            pnlStatTotal.ResumeLayout(false);
            pnlStatPending.ResumeLayout(false);
            pnlStatSubmitted.ResumeLayout(false);
            pnlStatOverdue.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        // ── Control declarations ───────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtSearch;
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