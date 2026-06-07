using Org.BouncyCastle.Asn1.Crmf;

namespace PUPAcadPortal
{
    partial class StudentCourseDashboard
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
            pnlHeader = new System.Windows.Forms.Panel();
            lblPageTitle = new System.Windows.Forms.Label();
            txtSearch = new System.Windows.Forms.TextBox();
            pnlStats = new System.Windows.Forms.Panel();

            pnlStatCourses = new System.Windows.Forms.Panel();
            pnlBarCourses = new System.Windows.Forms.Panel();
            lblStatCoursesVal = new System.Windows.Forms.Label();
            lblStatCoursesLbl = new System.Windows.Forms.Label();

            pnlStatPending = new System.Windows.Forms.Panel();
            pnlBarPending = new System.Windows.Forms.Panel();
            lblStatPendingVal = new System.Windows.Forms.Label();
            lblStatPendingLbl = new System.Windows.Forms.Label();

            pnlStatSubmitted = new System.Windows.Forms.Panel();
            pnlBarSubmitted = new System.Windows.Forms.Panel();
            lblStatSubmittedVal = new System.Windows.Forms.Label();
            lblStatSubmittedLbl = new System.Windows.Forms.Label();

            pnlStatOverdue = new System.Windows.Forms.Panel();
            pnlBarOverdue = new System.Windows.Forms.Panel();
            lblStatOverdueVal = new System.Windows.Forms.Label();
            lblStatOverdueLbl = new System.Windows.Forms.Label();

            flpCourseCards = new System.Windows.Forms.FlowLayoutPanel();

            pnlHeader.SuspendLayout();
            pnlStats.SuspendLayout();
            pnlStatCourses.SuspendLayout();
            pnlStatPending.SuspendLayout();
            pnlStatSubmitted.SuspendLayout();
            pnlStatOverdue.SuspendLayout();
            SuspendLayout();

            // ── pnlHeader ──────────────────────────────────────────────────
            pnlHeader.BackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            pnlHeader.Controls.Add(lblPageTitle);
            pnlHeader.Controls.Add(txtSearch);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(1680, 66);
            pnlHeader.TabIndex = 0;

            lblPageTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblPageTitle.ForeColor = System.Drawing.Color.White;
            lblPageTitle.Location = new System.Drawing.Point(18, 16);
            lblPageTitle.Name = "lblPageTitle";
            lblPageTitle.Size = new System.Drawing.Size(380, 32);
            lblPageTitle.TabIndex = 0;
            lblPageTitle.Text = "📚  My Courses";

            txtSearch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtSearch.Location = new System.Drawing.Point(1440, 22);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "🔍  Search courses...";
            txtSearch.Size = new System.Drawing.Size(224, 25);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += TxtSearch_TextChanged;

            // ── pnlStats ───────────────────────────────────────────────────
            pnlStats.BackColor = System.Drawing.Color.FromArgb(245, 245, 248);
            pnlStats.Controls.Add(pnlStatCourses);
            pnlStats.Controls.Add(pnlStatPending);
            pnlStats.Controls.Add(pnlStatSubmitted);
            pnlStats.Controls.Add(pnlStatOverdue);
            pnlStats.Dock = System.Windows.Forms.DockStyle.Top;
            pnlStats.Location = new System.Drawing.Point(0, 66);
            pnlStats.Name = "pnlStats";
            pnlStats.Padding = new System.Windows.Forms.Padding(18, 12, 18, 12);
            pnlStats.Size = new System.Drawing.Size(1680, 92);
            pnlStats.TabIndex = 1;

            // Stat card helper — Enrolled
            pnlStatCourses.BackColor = System.Drawing.Color.White;
            pnlStatCourses.Controls.Add(pnlBarCourses);
            pnlStatCourses.Controls.Add(lblStatCoursesVal);
            pnlStatCourses.Controls.Add(lblStatCoursesLbl);
            pnlStatCourses.Location = new System.Drawing.Point(36, 12);
            pnlStatCourses.Name = "pnlStatCourses";
            pnlStatCourses.Size = new System.Drawing.Size(178, 68);
            pnlStatCourses.TabIndex = 0;
            pnlBarCourses.BackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            pnlBarCourses.Dock = System.Windows.Forms.DockStyle.Left;
            pnlBarCourses.Size = new System.Drawing.Size(5, 68);
            pnlBarCourses.Name = "pnlBarCourses";
            lblStatCoursesVal.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            lblStatCoursesVal.ForeColor = System.Drawing.Color.FromArgb(128, 0, 0);
            lblStatCoursesVal.Location = new System.Drawing.Point(14, 6);
            lblStatCoursesVal.Name = "lblStatCoursesVal";
            lblStatCoursesVal.Size = new System.Drawing.Size(100, 38);
            lblStatCoursesVal.Text = "0";
            lblStatCoursesLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblStatCoursesLbl.ForeColor = System.Drawing.Color.Gray;
            lblStatCoursesLbl.Location = new System.Drawing.Point(14, 46);
            lblStatCoursesLbl.Name = "lblStatCoursesLbl";
            lblStatCoursesLbl.Size = new System.Drawing.Size(155, 18);
            lblStatCoursesLbl.Text = "Enrolled Courses";

            // Stat card — Pending
            pnlStatPending.BackColor = System.Drawing.Color.White;
            pnlStatPending.Controls.Add(pnlBarPending);
            pnlStatPending.Controls.Add(lblStatPendingVal);
            pnlStatPending.Controls.Add(lblStatPendingLbl);
            pnlStatPending.Location = new System.Drawing.Point(228, 12);
            pnlStatPending.Name = "pnlStatPending";
            pnlStatPending.Size = new System.Drawing.Size(178, 68);
            pnlStatPending.TabIndex = 1;
            pnlBarPending.BackColor = System.Drawing.Color.FromArgb(211, 84, 0);
            pnlBarPending.Dock = System.Windows.Forms.DockStyle.Left;
            pnlBarPending.Size = new System.Drawing.Size(5, 68);
            pnlBarPending.Name = "pnlBarPending";
            lblStatPendingVal.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            lblStatPendingVal.ForeColor = System.Drawing.Color.FromArgb(211, 84, 0);
            lblStatPendingVal.Location = new System.Drawing.Point(14, 6);
            lblStatPendingVal.Name = "lblStatPendingVal";
            lblStatPendingVal.Size = new System.Drawing.Size(100, 38);
            lblStatPendingVal.Text = "0";
            lblStatPendingLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblStatPendingLbl.ForeColor = System.Drawing.Color.Gray;
            lblStatPendingLbl.Location = new System.Drawing.Point(14, 46);
            lblStatPendingLbl.Name = "lblStatPendingLbl";
            lblStatPendingLbl.Size = new System.Drawing.Size(155, 18);
            lblStatPendingLbl.Text = "Pending Tasks";

            // Stat card — Submitted
            pnlStatSubmitted.BackColor = System.Drawing.Color.White;
            pnlStatSubmitted.Controls.Add(pnlBarSubmitted);
            pnlStatSubmitted.Controls.Add(lblStatSubmittedVal);
            pnlStatSubmitted.Controls.Add(lblStatSubmittedLbl);
            pnlStatSubmitted.Location = new System.Drawing.Point(420, 12);
            pnlStatSubmitted.Name = "pnlStatSubmitted";
            pnlStatSubmitted.Size = new System.Drawing.Size(178, 68);
            pnlStatSubmitted.TabIndex = 2;
            pnlBarSubmitted.BackColor = System.Drawing.Color.FromArgb(46, 160, 67);
            pnlBarSubmitted.Dock = System.Windows.Forms.DockStyle.Left;
            pnlBarSubmitted.Size = new System.Drawing.Size(5, 68);
            pnlBarSubmitted.Name = "pnlBarSubmitted";
            lblStatSubmittedVal.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            lblStatSubmittedVal.ForeColor = System.Drawing.Color.FromArgb(46, 160, 67);
            lblStatSubmittedVal.Location = new System.Drawing.Point(14, 6);
            lblStatSubmittedVal.Name = "lblStatSubmittedVal";
            lblStatSubmittedVal.Size = new System.Drawing.Size(100, 38);
            lblStatSubmittedVal.Text = "0";
            lblStatSubmittedLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblStatSubmittedLbl.ForeColor = System.Drawing.Color.Gray;
            lblStatSubmittedLbl.Location = new System.Drawing.Point(14, 46);
            lblStatSubmittedLbl.Name = "lblStatSubmittedLbl";
            lblStatSubmittedLbl.Size = new System.Drawing.Size(155, 18);
            lblStatSubmittedLbl.Text = "Submitted";

            // Stat card — Overdue
            pnlStatOverdue.BackColor = System.Drawing.Color.White;
            pnlStatOverdue.Controls.Add(pnlBarOverdue);
            pnlStatOverdue.Controls.Add(lblStatOverdueVal);
            pnlStatOverdue.Controls.Add(lblStatOverdueLbl);
            pnlStatOverdue.Location = new System.Drawing.Point(612, 12);
            pnlStatOverdue.Name = "pnlStatOverdue";
            pnlStatOverdue.Size = new System.Drawing.Size(178, 68);
            pnlStatOverdue.TabIndex = 3;
            pnlBarOverdue.BackColor = System.Drawing.Color.FromArgb(185, 50, 50);
            pnlBarOverdue.Dock = System.Windows.Forms.DockStyle.Left;
            pnlBarOverdue.Size = new System.Drawing.Size(5, 68);
            pnlBarOverdue.Name = "pnlBarOverdue";
            lblStatOverdueVal.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            lblStatOverdueVal.ForeColor = System.Drawing.Color.FromArgb(185, 50, 50);
            lblStatOverdueVal.Location = new System.Drawing.Point(14, 6);
            lblStatOverdueVal.Name = "lblStatOverdueVal";
            lblStatOverdueVal.Size = new System.Drawing.Size(100, 38);
            lblStatOverdueVal.Text = "0";
            lblStatOverdueLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblStatOverdueLbl.ForeColor = System.Drawing.Color.Gray;
            lblStatOverdueLbl.Location = new System.Drawing.Point(14, 46);
            lblStatOverdueLbl.Name = "lblStatOverdueLbl";
            lblStatOverdueLbl.Size = new System.Drawing.Size(155, 18);
            lblStatOverdueLbl.Text = "Overdue";

            // ── flpCourseCards ─────────────────────────────────────────────
            flpCourseCards.AutoScroll = true;
            flpCourseCards.BackColor = System.Drawing.Color.FromArgb(237, 237, 242);
            flpCourseCards.Dock = System.Windows.Forms.DockStyle.Fill;
            flpCourseCards.Location = new System.Drawing.Point(0, 158);
            flpCourseCards.Name = "flpCourseCards";
            flpCourseCards.Padding = new System.Windows.Forms.Padding(10);
            flpCourseCards.Size = new System.Drawing.Size(1680, 831);
            flpCourseCards.TabIndex = 2;

            // ── StudentCourseDashboard ─────────────────────────────────────
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(flpCourseCards);
            Controls.Add(pnlStats);
            Controls.Add(pnlHeader);
            Name = "StudentCourseDashboard";
            Size = new System.Drawing.Size(1680, 989);

            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlStats.ResumeLayout(false);
            pnlStatCourses.ResumeLayout(false);
            pnlStatPending.ResumeLayout(false);
            pnlStatSubmitted.ResumeLayout(false);
            pnlStatOverdue.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Panel pnlStats;
        private System.Windows.Forms.Panel pnlStatCourses;
        private System.Windows.Forms.Panel pnlBarCourses;
        private System.Windows.Forms.Label lblStatCoursesVal;
        private System.Windows.Forms.Label lblStatCoursesLbl;
        private System.Windows.Forms.Panel pnlStatPending;
        private System.Windows.Forms.Panel pnlBarPending;
        private System.Windows.Forms.Label lblStatPendingVal;
        private System.Windows.Forms.Label lblStatPendingLbl;
        private System.Windows.Forms.Panel pnlStatSubmitted;
        private System.Windows.Forms.Panel pnlBarSubmitted;
        private System.Windows.Forms.Label lblStatSubmittedVal;
        private System.Windows.Forms.Label lblStatSubmittedLbl;
        private System.Windows.Forms.Panel pnlStatOverdue;
        private System.Windows.Forms.Panel pnlBarOverdue;
        private System.Windows.Forms.Label lblStatOverdueVal;
        private System.Windows.Forms.Label lblStatOverdueLbl;
        private System.Windows.Forms.FlowLayoutPanel flpCourseCards;
    }
}