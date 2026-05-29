namespace PUPAcadPortal
{
    partial class ActivityDashboard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlTopBar = new System.Windows.Forms.Panel();
            lblDashboardTitle = new System.Windows.Forms.Label();
            txtSearchCourse = new System.Windows.Forms.TextBox();
            cmbFilterCourse = new System.Windows.Forms.ComboBox();
            pnlStats = new System.Windows.Forms.Panel();
            pnlStatCourses = new System.Windows.Forms.Panel();
            lblTotalCourses = new System.Windows.Forms.Label();
            lblCoursesLbl = new System.Windows.Forms.Label();
            pnlStatActivities = new System.Windows.Forms.Panel();
            lblTotalActivities = new System.Windows.Forms.Label();
            lblActivitiesLbl = new System.Windows.Forms.Label();
            pnlStatPending = new System.Windows.Forms.Panel();
            lblTotalPending = new System.Windows.Forms.Label();
            lblPendingLbl = new System.Windows.Forms.Label();
            pnlStatChecked = new System.Windows.Forms.Panel();
            lblTotalChecked = new System.Windows.Forms.Label();
            lblCheckedLbl = new System.Windows.Forms.Label();
            flpCourseCards = new System.Windows.Forms.FlowLayoutPanel();

            pnlTopBar.SuspendLayout();
            pnlStats.SuspendLayout();
            pnlStatCourses.SuspendLayout();
            pnlStatActivities.SuspendLayout();
            pnlStatPending.SuspendLayout();
            pnlStatChecked.SuspendLayout();
            SuspendLayout();

            // ── pnlTopBar ─────────────────────────────────────────────────────
            pnlTopBar.BackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            pnlTopBar.Controls.Add(lblDashboardTitle);
            pnlTopBar.Controls.Add(txtSearchCourse);
            pnlTopBar.Controls.Add(cmbFilterCourse);
            pnlTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            pnlTopBar.Location = new System.Drawing.Point(0, 0);
            pnlTopBar.Name = "pnlTopBar";
            pnlTopBar.Size = new System.Drawing.Size(1680, 62);
            pnlTopBar.TabIndex = 3;

            // lblDashboardTitle
            lblDashboardTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblDashboardTitle.ForeColor = System.Drawing.Color.White;
            lblDashboardTitle.Location = new System.Drawing.Point(18, 14);
            lblDashboardTitle.Name = "lblDashboardTitle";
            lblDashboardTitle.Size = new System.Drawing.Size(320, 32);
            lblDashboardTitle.TabIndex = 0;
            lblDashboardTitle.Text = "📚  Activity Dashboard";

            // txtSearchCourse
            txtSearchCourse.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtSearchCourse.Location = new System.Drawing.Point(1260, 18);
            txtSearchCourse.Name = "txtSearchCourse";
            txtSearchCourse.PlaceholderText = "🔍  Search course...";
            txtSearchCourse.Size = new System.Drawing.Size(220, 26);
            txtSearchCourse.TabIndex = 1;
            txtSearchCourse.TextChanged += txtSearchCourse_TextChanged;

            // cmbFilterCourse
            cmbFilterCourse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbFilterCourse.Font = new System.Drawing.Font("Segoe UI", 10F);
            cmbFilterCourse.Items.AddRange(new object[] { "All", "Active", "Ongoing", "Completed" });
            cmbFilterCourse.Location = new System.Drawing.Point(1492, 18);
            cmbFilterCourse.Name = "cmbFilterCourse";
            cmbFilterCourse.Size = new System.Drawing.Size(150, 26);
            cmbFilterCourse.TabIndex = 2;
            cmbFilterCourse.SelectedIndex = 0;
            cmbFilterCourse.SelectedIndexChanged += cmbFilterCourse_SelectedIndexChanged;

            // ── pnlStats ──────────────────────────────────────────────────────
            pnlStats.BackColor = System.Drawing.Color.FromArgb(245, 245, 248);
            pnlStats.Controls.Add(pnlStatCourses);
            pnlStats.Controls.Add(pnlStatActivities);
            pnlStats.Controls.Add(pnlStatPending);
            pnlStats.Controls.Add(pnlStatChecked);
            pnlStats.Dock = System.Windows.Forms.DockStyle.Top;
            pnlStats.Location = new System.Drawing.Point(0, 62);
            pnlStats.Name = "pnlStats";
            pnlStats.Padding = new System.Windows.Forms.Padding(18, 12, 18, 12);
            pnlStats.Size = new System.Drawing.Size(1680, 92);
            pnlStats.TabIndex = 2;

            BuildStatPanel(pnlStatCourses, lblTotalCourses, lblCoursesLbl, "Courses", 18, System.Drawing.Color.FromArgb(128, 0, 0));
            BuildStatPanel(pnlStatActivities, lblTotalActivities, lblActivitiesLbl, "Activities", 210, System.Drawing.Color.FromArgb(63, 81, 181));
            BuildStatPanel(pnlStatPending, lblTotalPending, lblPendingLbl, "Pending", 402, System.Drawing.Color.FromArgb(211, 84, 0));
            BuildStatPanel(pnlStatChecked, lblTotalChecked, lblCheckedLbl, "Checked", 594, System.Drawing.Color.FromArgb(46, 160, 67));

            // ── flpCourseCards ────────────────────────────────────────────────
            flpCourseCards.AutoScroll = true;
            flpCourseCards.BackColor = System.Drawing.Color.FromArgb(237, 237, 242);
            flpCourseCards.Dock = System.Windows.Forms.DockStyle.Fill;
            flpCourseCards.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            flpCourseCards.Location = new System.Drawing.Point(0, 154);
            flpCourseCards.Name = "flpCourseCards";
            flpCourseCards.Padding = new System.Windows.Forms.Padding(10);
            flpCourseCards.Size = new System.Drawing.Size(1680, 835);
            flpCourseCards.TabIndex = 0;
            flpCourseCards.WrapContents = true;

            // ── ActivityDashboard ─────────────────────────────────────────────
            Controls.Add(flpCourseCards);
            Controls.Add(pnlStats);
            Controls.Add(pnlTopBar);
            Name = "ActivityDashboard";
            Size = new System.Drawing.Size(1680, 989);

            pnlTopBar.ResumeLayout(false);
            pnlTopBar.PerformLayout();
            pnlStats.ResumeLayout(false);
            pnlStatCourses.ResumeLayout(false);
            pnlStatActivities.ResumeLayout(false);
            pnlStatPending.ResumeLayout(false);
            pnlStatChecked.ResumeLayout(false);
            ResumeLayout(false);
        }

        private static void BuildStatPanel(
            System.Windows.Forms.Panel panel,
            System.Windows.Forms.Label lblVal,
            System.Windows.Forms.Label lblName,
            string name, int x,
            System.Drawing.Color accent)
        {
            panel.BackColor = System.Drawing.Color.White;
            panel.Location = new System.Drawing.Point(x + 18, 12);
            panel.Size = new System.Drawing.Size(178, 68);
            panel.BorderStyle = System.Windows.Forms.BorderStyle.None;

            // Left accent bar
            var bar = new System.Windows.Forms.Panel
            {
                BackColor = accent,
                Dock = System.Windows.Forms.DockStyle.Left,
                Width = 5
            };
            panel.Controls.Add(bar);

            lblVal.Text = "0";
            lblVal.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            lblVal.ForeColor = accent;
            lblVal.Location = new System.Drawing.Point(14, 6);
            lblVal.Size = new System.Drawing.Size(100, 38);
            lblVal.TabIndex = 1;

            lblName.Text = name;
            lblName.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblName.ForeColor = System.Drawing.Color.Gray;
            lblName.Location = new System.Drawing.Point(14, 46);
            lblName.Size = new System.Drawing.Size(155, 18);
            lblName.TabIndex = 2;

            panel.Controls.Add(lblVal);
            panel.Controls.Add(lblName);
        }

        // ── Control declarations ──────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.Label lblDashboardTitle;
        private System.Windows.Forms.TextBox txtSearchCourse;
        private System.Windows.Forms.ComboBox cmbFilterCourse;
        private System.Windows.Forms.Panel pnlStats;
        private System.Windows.Forms.Panel pnlStatCourses;
        private System.Windows.Forms.Label lblTotalCourses;
        private System.Windows.Forms.Label lblCoursesLbl;
        private System.Windows.Forms.Panel pnlStatActivities;
        private System.Windows.Forms.Label lblTotalActivities;
        private System.Windows.Forms.Label lblActivitiesLbl;
        private System.Windows.Forms.Panel pnlStatPending;
        private System.Windows.Forms.Label lblTotalPending;
        private System.Windows.Forms.Label lblPendingLbl;
        private System.Windows.Forms.Panel pnlStatChecked;
        private System.Windows.Forms.Label lblTotalChecked;
        private System.Windows.Forms.Label lblCheckedLbl;
        private System.Windows.Forms.FlowLayoutPanel flpCourseCards;
    }
}