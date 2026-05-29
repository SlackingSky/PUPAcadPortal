namespace PUPAcadPortal
{
    partial class ActivityDashboard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.pnlTopBar = new System.Windows.Forms.Panel();
            this.lblDashboardTitle = new System.Windows.Forms.Label();
            this.txtSearchCourse = new System.Windows.Forms.TextBox();
            this.cmbFilterCourse = new System.Windows.Forms.ComboBox();
            this.pnlStats = new System.Windows.Forms.Panel();
            this.pnlStatCourses = new System.Windows.Forms.Panel();
            this.pnlBarCourses = new System.Windows.Forms.Panel();
            this.lblTotalCourses = new System.Windows.Forms.Label();
            this.lblCoursesLbl = new System.Windows.Forms.Label();
            this.pnlStatActivities = new System.Windows.Forms.Panel();
            this.pnlBarActivities = new System.Windows.Forms.Panel();
            this.lblTotalActivities = new System.Windows.Forms.Label();
            this.lblActivitiesLbl = new System.Windows.Forms.Label();
            this.pnlStatPending = new System.Windows.Forms.Panel();
            this.pnlBarPending = new System.Windows.Forms.Panel();
            this.lblTotalPending = new System.Windows.Forms.Label();
            this.lblPendingLbl = new System.Windows.Forms.Label();
            this.pnlStatChecked = new System.Windows.Forms.Panel();
            this.pnlBarChecked = new System.Windows.Forms.Panel();
            this.lblTotalChecked = new System.Windows.Forms.Label();
            this.lblCheckedLbl = new System.Windows.Forms.Label();
            this.flpCourseCards = new System.Windows.Forms.FlowLayoutPanel();

            this.pnlTopBar.SuspendLayout();
            this.pnlStats.SuspendLayout();
            this.pnlStatCourses.SuspendLayout();
            this.pnlStatActivities.SuspendLayout();
            this.pnlStatPending.SuspendLayout();
            this.pnlStatChecked.SuspendLayout();
            this.SuspendLayout();

            // ── pnlTopBar ─────────────────────────────────────────────────────
            this.pnlTopBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pnlTopBar.Controls.Add(this.lblDashboardTitle);
            this.pnlTopBar.Controls.Add(this.txtSearchCourse);
            this.pnlTopBar.Controls.Add(this.cmbFilterCourse);
            this.pnlTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTopBar.Name = "pnlTopBar";
            this.pnlTopBar.Size = new System.Drawing.Size(1680, 62);
            this.pnlTopBar.TabIndex = 3;

            // ── lblDashboardTitle ─────────────────────────────────────────────
            this.lblDashboardTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblDashboardTitle.ForeColor = System.Drawing.Color.White;
            this.lblDashboardTitle.Location = new System.Drawing.Point(18, 14);
            this.lblDashboardTitle.Name = "lblDashboardTitle";
            this.lblDashboardTitle.Size = new System.Drawing.Size(320, 32);
            this.lblDashboardTitle.TabIndex = 0;
            this.lblDashboardTitle.Text = "📚  Activity Dashboard";

            // ── txtSearchCourse ───────────────────────────────────────────────
            this.txtSearchCourse.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearchCourse.Location = new System.Drawing.Point(1260, 18);
            this.txtSearchCourse.Name = "txtSearchCourse";
            this.txtSearchCourse.PlaceholderText = "🔍  Search course...";
            this.txtSearchCourse.Size = new System.Drawing.Size(220, 25);
            this.txtSearchCourse.TabIndex = 1;
            this.txtSearchCourse.TextChanged += new System.EventHandler(this.txtSearchCourse_TextChanged);

            // ── cmbFilterCourse ───────────────────────────────────────────────
            this.cmbFilterCourse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterCourse.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbFilterCourse.FormattingEnabled = true;
            this.cmbFilterCourse.Items.AddRange(new object[] {
            "All",
            "Active",
            "Ongoing",
            "Completed"});
            this.cmbFilterCourse.Location = new System.Drawing.Point(1492, 18);
            this.cmbFilterCourse.Name = "cmbFilterCourse";
            this.cmbFilterCourse.Size = new System.Drawing.Size(150, 25);
            this.cmbFilterCourse.TabIndex = 2;
            this.cmbFilterCourse.SelectedIndexChanged += new System.EventHandler(this.cmbFilterCourse_SelectedIndexChanged);

            // ── pnlStats ──────────────────────────────────────────────────────
            this.pnlStats.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(248)))));
            this.pnlStats.Controls.Add(this.pnlStatCourses);
            this.pnlStats.Controls.Add(this.pnlStatActivities);
            this.pnlStats.Controls.Add(this.pnlStatPending);
            this.pnlStats.Controls.Add(this.pnlStatChecked);
            this.pnlStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStats.Location = new System.Drawing.Point(0, 62);
            this.pnlStats.Name = "pnlStats";
            this.pnlStats.Padding = new System.Windows.Forms.Padding(18, 12, 18, 12);
            this.pnlStats.Size = new System.Drawing.Size(1680, 92);
            this.pnlStats.TabIndex = 2;

            // ── pnlStatCourses ────────────────────────────────────────────────
            this.pnlStatCourses.BackColor = System.Drawing.Color.White;
            this.pnlStatCourses.Controls.Add(this.pnlBarCourses);
            this.pnlStatCourses.Controls.Add(this.lblTotalCourses);
            this.pnlStatCourses.Controls.Add(this.lblCoursesLbl);
            this.pnlStatCourses.Location = new System.Drawing.Point(36, 12);
            this.pnlStatCourses.Name = "pnlStatCourses";
            this.pnlStatCourses.Size = new System.Drawing.Size(178, 68);
            this.pnlStatCourses.TabIndex = 0;

            // pnlBarCourses
            this.pnlBarCourses.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pnlBarCourses.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlBarCourses.Location = new System.Drawing.Point(0, 0);
            this.pnlBarCourses.Name = "pnlBarCourses";
            this.pnlBarCourses.Size = new System.Drawing.Size(5, 68);
            this.pnlBarCourses.TabIndex = 0;

            // lblTotalCourses
            this.lblTotalCourses.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTotalCourses.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblTotalCourses.Location = new System.Drawing.Point(14, 6);
            this.lblTotalCourses.Name = "lblTotalCourses";
            this.lblTotalCourses.Size = new System.Drawing.Size(100, 38);
            this.lblTotalCourses.TabIndex = 1;
            this.lblTotalCourses.Text = "0";

            // lblCoursesLbl
            this.lblCoursesLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCoursesLbl.ForeColor = System.Drawing.Color.Gray;
            this.lblCoursesLbl.Location = new System.Drawing.Point(14, 46);
            this.lblCoursesLbl.Name = "lblCoursesLbl";
            this.lblCoursesLbl.Size = new System.Drawing.Size(155, 18);
            this.lblCoursesLbl.TabIndex = 2;
            this.lblCoursesLbl.Text = "Courses";

            // ── pnlStatActivities ─────────────────────────────────────────────
            this.pnlStatActivities.BackColor = System.Drawing.Color.White;
            this.pnlStatActivities.Controls.Add(this.pnlBarActivities);
            this.pnlStatActivities.Controls.Add(this.lblTotalActivities);
            this.pnlStatActivities.Controls.Add(this.lblActivitiesLbl);
            this.pnlStatActivities.Location = new System.Drawing.Point(228, 12);
            this.pnlStatActivities.Name = "pnlStatActivities";
            this.pnlStatActivities.Size = new System.Drawing.Size(178, 68);
            this.pnlStatActivities.TabIndex = 1;

            // pnlBarActivities
            this.pnlBarActivities.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(81)))), ((int)(((byte)(181)))));
            this.pnlBarActivities.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlBarActivities.Location = new System.Drawing.Point(0, 0);
            this.pnlBarActivities.Name = "pnlBarActivities";
            this.pnlBarActivities.Size = new System.Drawing.Size(5, 68);
            this.pnlBarActivities.TabIndex = 0;

            // lblTotalActivities
            this.lblTotalActivities.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTotalActivities.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(81)))), ((int)(((byte)(181)))));
            this.lblTotalActivities.Location = new System.Drawing.Point(14, 6);
            this.lblTotalActivities.Name = "lblTotalActivities";
            this.lblTotalActivities.Size = new System.Drawing.Size(100, 38);
            this.lblTotalActivities.TabIndex = 1;
            this.lblTotalActivities.Text = "0";

            // lblActivitiesLbl
            this.lblActivitiesLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblActivitiesLbl.ForeColor = System.Drawing.Color.Gray;
            this.lblActivitiesLbl.Location = new System.Drawing.Point(14, 46);
            this.lblActivitiesLbl.Name = "lblActivitiesLbl";
            this.lblActivitiesLbl.Size = new System.Drawing.Size(155, 18);
            this.lblActivitiesLbl.TabIndex = 2;
            this.lblActivitiesLbl.Text = "Activities";

            // ── pnlStatPending ────────────────────────────────────────────────
            this.pnlStatPending.BackColor = System.Drawing.Color.White;
            this.pnlStatPending.Controls.Add(this.pnlBarPending);
            this.pnlStatPending.Controls.Add(this.lblTotalPending);
            this.pnlStatPending.Controls.Add(this.lblPendingLbl);
            this.pnlStatPending.Location = new System.Drawing.Point(420, 12);
            this.pnlStatPending.Name = "pnlStatPending";
            this.pnlStatPending.Size = new System.Drawing.Size(178, 68);
            this.pnlStatPending.TabIndex = 2;

            // pnlBarPending
            this.pnlBarPending.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(84)))), ((int)(((byte)(0)))));
            this.pnlBarPending.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlBarPending.Location = new System.Drawing.Point(0, 0);
            this.pnlBarPending.Name = "pnlBarPending";
            this.pnlBarPending.Size = new System.Drawing.Size(5, 68);
            this.pnlBarPending.TabIndex = 0;

            // lblTotalPending
            this.lblTotalPending.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTotalPending.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(84)))), ((int)(((byte)(0)))));
            this.lblTotalPending.Location = new System.Drawing.Point(14, 6);
            this.lblTotalPending.Name = "lblTotalPending";
            this.lblTotalPending.Size = new System.Drawing.Size(100, 38);
            this.lblTotalPending.TabIndex = 1;
            this.lblTotalPending.Text = "0";

            // lblPendingLbl
            this.lblPendingLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPendingLbl.ForeColor = System.Drawing.Color.Gray;
            this.lblPendingLbl.Location = new System.Drawing.Point(14, 46);
            this.lblPendingLbl.Name = "lblPendingLbl";
            this.lblPendingLbl.Size = new System.Drawing.Size(155, 18);
            this.lblPendingLbl.TabIndex = 2;
            this.lblPendingLbl.Text = "Pending";

            this.pnlStatChecked.BackColor = System.Drawing.Color.White;
            this.pnlStatChecked.Controls.Add(this.pnlBarChecked);
            this.pnlStatChecked.Controls.Add(this.lblTotalChecked);
            this.pnlStatChecked.Controls.Add(this.lblCheckedLbl);
            this.pnlStatChecked.Location = new System.Drawing.Point(612, 12);
            this.pnlStatChecked.Name = "pnlStatChecked";
            this.pnlStatChecked.Size = new System.Drawing.Size(178, 68);
            this.pnlStatChecked.TabIndex = 3;

            this.pnlBarChecked.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(160)))), ((int)(((byte)(67)))));
            this.pnlBarChecked.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlBarChecked.Location = new System.Drawing.Point(0, 0);
            this.pnlBarChecked.Name = "pnlBarChecked";
            this.pnlBarChecked.Size = new System.Drawing.Size(5, 68);
            this.pnlBarChecked.TabIndex = 0;

            this.lblTotalChecked.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTotalChecked.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(160)))), ((int)(((byte)(67)))));
            this.lblTotalChecked.Location = new System.Drawing.Point(14, 6);
            this.lblTotalChecked.Name = "lblTotalChecked";
            this.lblTotalChecked.Size = new System.Drawing.Size(100, 38);
            this.lblTotalChecked.TabIndex = 1;
            this.lblTotalChecked.Text = "0";

            this.lblCheckedLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCheckedLbl.ForeColor = System.Drawing.Color.Gray;
            this.lblCheckedLbl.Location = new System.Drawing.Point(14, 46);
            this.lblCheckedLbl.Name = "lblCheckedLbl";
            this.lblCheckedLbl.Size = new System.Drawing.Size(155, 18);
            this.lblCheckedLbl.TabIndex = 2;
            this.lblCheckedLbl.Text = "Checked";

            this.flpCourseCards.AutoScroll = true;
            this.flpCourseCards.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(242)))));
            this.flpCourseCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpCourseCards.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flpCourseCards.Location = new System.Drawing.Point(0, 154);
            this.flpCourseCards.Name = "flpCourseCards";
            this.flpCourseCards.Padding = new System.Windows.Forms.Padding(10);
            this.flpCourseCards.Size = new System.Drawing.Size(1680, 835);
            this.flpCourseCards.TabIndex = 0;

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flpCourseCards);
            this.Controls.Add(this.pnlStats);
            this.Controls.Add(this.pnlTopBar);
            this.Name = "ActivityDashboard";
            this.Size = new System.Drawing.Size(1680, 989);

            this.pnlTopBar.ResumeLayout(false);
            this.pnlTopBar.PerformLayout();
            this.pnlStats.ResumeLayout(false);
            this.pnlStatCourses.ResumeLayout(false);
            this.pnlStatActivities.ResumeLayout(false);
            this.pnlStatPending.ResumeLayout(false);
            this.pnlStatChecked.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.Label lblDashboardTitle;
        private System.Windows.Forms.TextBox txtSearchCourse;
        private System.Windows.Forms.ComboBox cmbFilterCourse;
        private System.Windows.Forms.Panel pnlStats;

        private System.Windows.Forms.Panel pnlStatCourses;
        private System.Windows.Forms.Panel pnlBarCourses;
        private System.Windows.Forms.Label lblTotalCourses;
        private System.Windows.Forms.Label lblCoursesLbl;

        private System.Windows.Forms.Panel pnlStatActivities;
        private System.Windows.Forms.Panel pnlBarActivities;
        private System.Windows.Forms.Label lblTotalActivities;
        private System.Windows.Forms.Label lblActivitiesLbl;

        private System.Windows.Forms.Panel pnlStatPending;
        private System.Windows.Forms.Panel pnlBarPending;
        private System.Windows.Forms.Label lblTotalPending;
        private System.Windows.Forms.Label lblPendingLbl;

        private System.Windows.Forms.Panel pnlStatChecked;
        private System.Windows.Forms.Panel pnlBarChecked;
        private System.Windows.Forms.Label lblTotalChecked;
        private System.Windows.Forms.Label lblCheckedLbl;

        private System.Windows.Forms.FlowLayoutPanel flpCourseCards;
    }
}