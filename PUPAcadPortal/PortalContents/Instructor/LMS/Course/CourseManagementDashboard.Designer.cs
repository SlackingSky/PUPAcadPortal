namespace PUPAcadPortal
{
    partial class CourseManagementDashboard
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
            lblDashboardTitle = new Label();
            txtSearchCourse = new TextBox();
            pnlStats = new Panel();
            pnlStatCourses = new Panel();
            pnlBarCourses = new Panel();
            lblTotalCourses = new Label();
            lblCoursesLbl = new Label();
            pnlStatActivities = new Panel();
            pnlBarActivities = new Panel();
            lblTotalActivities = new Label();
            lblActivitiesLbl = new Label();
            pnlStatStudents = new Panel();
            pnlBarStudents = new Panel();
            lblTotalStudents = new Label();
            lblStudentsLbl = new Label();
            pnlStatPending = new Panel();
            pnlBarPending = new Panel();
            lblTotalPending = new Label();
            lblPendingLbl = new Label();
            flpCourseCards = new FlowLayoutPanel();
            pnlHeader.SuspendLayout();
            pnlStats.SuspendLayout();
            pnlStatCourses.SuspendLayout();
            pnlStatActivities.SuspendLayout();
            pnlStatStudents.SuspendLayout();
            pnlStatPending.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(128, 0, 0);
            pnlHeader.Controls.Add(lblDashboardTitle);
            pnlHeader.Controls.Add(txtSearchCourse);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1680, 66);
            pnlHeader.TabIndex = 0;
            // 
            // lblDashboardTitle
            // 
            lblDashboardTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblDashboardTitle.ForeColor = Color.White;
            lblDashboardTitle.Location = new Point(18, 16);
            lblDashboardTitle.Name = "lblDashboardTitle";
            lblDashboardTitle.Size = new Size(380, 32);
            lblDashboardTitle.TabIndex = 0;
            lblDashboardTitle.Text = "📚  Course Management";
            // 
            // txtSearchCourse
            // 
            txtSearchCourse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtSearchCourse.Font = new Font("Segoe UI", 10F);
            txtSearchCourse.Location = new Point(1429, 23);
            txtSearchCourse.Name = "txtSearchCourse";
            txtSearchCourse.PlaceholderText = "🔍  Search courses...";
            txtSearchCourse.Size = new Size(220, 25);
            txtSearchCourse.TabIndex = 1;
            txtSearchCourse.TextChanged += TxtSearchCourse_TextChanged;
            // 
            // pnlStats
            // 
            pnlStats.BackColor = Color.FromArgb(245, 245, 248);
            pnlStats.Controls.Add(pnlStatCourses);
            pnlStats.Controls.Add(pnlStatActivities);
            pnlStats.Controls.Add(pnlStatStudents);
            pnlStats.Controls.Add(pnlStatPending);
            pnlStats.Dock = DockStyle.Top;
            pnlStats.Location = new Point(0, 66);
            pnlStats.Name = "pnlStats";
            pnlStats.Padding = new Padding(18, 12, 18, 12);
            pnlStats.Size = new Size(1680, 92);
            pnlStats.TabIndex = 1;
            // 
            // pnlStatCourses
            // 
            pnlStatCourses.BackColor = Color.White;
            pnlStatCourses.Controls.Add(pnlBarCourses);
            pnlStatCourses.Controls.Add(lblTotalCourses);
            pnlStatCourses.Controls.Add(lblCoursesLbl);
            pnlStatCourses.Location = new Point(36, 12);
            pnlStatCourses.Name = "pnlStatCourses";
            pnlStatCourses.Size = new Size(178, 68);
            pnlStatCourses.TabIndex = 0;
            // 
            // pnlBarCourses
            // 
            pnlBarCourses.BackColor = Color.FromArgb(128, 0, 0);
            pnlBarCourses.Dock = DockStyle.Left;
            pnlBarCourses.Location = new Point(0, 0);
            pnlBarCourses.Name = "pnlBarCourses";
            pnlBarCourses.Size = new Size(5, 68);
            pnlBarCourses.TabIndex = 0;
            // 
            // lblTotalCourses
            // 
            lblTotalCourses.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTotalCourses.ForeColor = Color.FromArgb(128, 0, 0);
            lblTotalCourses.Location = new Point(14, 6);
            lblTotalCourses.Name = "lblTotalCourses";
            lblTotalCourses.Size = new Size(100, 38);
            lblTotalCourses.TabIndex = 1;
            lblTotalCourses.Text = "0";
            // 
            // lblCoursesLbl
            // 
            lblCoursesLbl.Font = new Font("Segoe UI", 9F);
            lblCoursesLbl.ForeColor = Color.Gray;
            lblCoursesLbl.Location = new Point(14, 46);
            lblCoursesLbl.Name = "lblCoursesLbl";
            lblCoursesLbl.Size = new Size(155, 18);
            lblCoursesLbl.TabIndex = 2;
            lblCoursesLbl.Text = "Courses";
            // 
            // pnlStatActivities
            // 
            pnlStatActivities.BackColor = Color.White;
            pnlStatActivities.Controls.Add(pnlBarActivities);
            pnlStatActivities.Controls.Add(lblTotalActivities);
            pnlStatActivities.Controls.Add(lblActivitiesLbl);
            pnlStatActivities.Location = new Point(228, 12);
            pnlStatActivities.Name = "pnlStatActivities";
            pnlStatActivities.Size = new Size(178, 68);
            pnlStatActivities.TabIndex = 1;
            // 
            // pnlBarActivities
            // 
            pnlBarActivities.BackColor = Color.FromArgb(63, 81, 181);
            pnlBarActivities.Dock = DockStyle.Left;
            pnlBarActivities.Location = new Point(0, 0);
            pnlBarActivities.Name = "pnlBarActivities";
            pnlBarActivities.Size = new Size(5, 68);
            pnlBarActivities.TabIndex = 0;
            // 
            // lblTotalActivities
            // 
            lblTotalActivities.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTotalActivities.ForeColor = Color.FromArgb(63, 81, 181);
            lblTotalActivities.Location = new Point(14, 6);
            lblTotalActivities.Name = "lblTotalActivities";
            lblTotalActivities.Size = new Size(100, 38);
            lblTotalActivities.TabIndex = 1;
            lblTotalActivities.Text = "0";
            // 
            // lblActivitiesLbl
            // 
            lblActivitiesLbl.Font = new Font("Segoe UI", 9F);
            lblActivitiesLbl.ForeColor = Color.Gray;
            lblActivitiesLbl.Location = new Point(14, 46);
            lblActivitiesLbl.Name = "lblActivitiesLbl";
            lblActivitiesLbl.Size = new Size(155, 18);
            lblActivitiesLbl.TabIndex = 2;
            lblActivitiesLbl.Text = "Total Activities";
            // 
            // pnlStatStudents
            // 
            pnlStatStudents.BackColor = Color.White;
            pnlStatStudents.Controls.Add(pnlBarStudents);
            pnlStatStudents.Controls.Add(lblTotalStudents);
            pnlStatStudents.Controls.Add(lblStudentsLbl);
            pnlStatStudents.Location = new Point(420, 12);
            pnlStatStudents.Name = "pnlStatStudents";
            pnlStatStudents.Size = new Size(178, 68);
            pnlStatStudents.TabIndex = 2;
            // 
            // pnlBarStudents
            // 
            pnlBarStudents.BackColor = Color.FromArgb(0, 150, 136);
            pnlBarStudents.Dock = DockStyle.Left;
            pnlBarStudents.Location = new Point(0, 0);
            pnlBarStudents.Name = "pnlBarStudents";
            pnlBarStudents.Size = new Size(5, 68);
            pnlBarStudents.TabIndex = 0;
            // 
            // lblTotalStudents
            // 
            lblTotalStudents.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTotalStudents.ForeColor = Color.FromArgb(0, 150, 136);
            lblTotalStudents.Location = new Point(14, 6);
            lblTotalStudents.Name = "lblTotalStudents";
            lblTotalStudents.Size = new Size(100, 38);
            lblTotalStudents.TabIndex = 1;
            lblTotalStudents.Text = "0";
            // 
            // lblStudentsLbl
            // 
            lblStudentsLbl.Font = new Font("Segoe UI", 9F);
            lblStudentsLbl.ForeColor = Color.Gray;
            lblStudentsLbl.Location = new Point(14, 46);
            lblStudentsLbl.Name = "lblStudentsLbl";
            lblStudentsLbl.Size = new Size(155, 18);
            lblStudentsLbl.TabIndex = 2;
            lblStudentsLbl.Text = "Enrolled Students";
            // 
            // pnlStatPending
            // 
            pnlStatPending.BackColor = Color.White;
            pnlStatPending.Controls.Add(pnlBarPending);
            pnlStatPending.Controls.Add(lblTotalPending);
            pnlStatPending.Controls.Add(lblPendingLbl);
            pnlStatPending.Location = new Point(612, 12);
            pnlStatPending.Name = "pnlStatPending";
            pnlStatPending.Size = new Size(178, 68);
            pnlStatPending.TabIndex = 3;
            // 
            // pnlBarPending
            // 
            pnlBarPending.BackColor = Color.FromArgb(211, 84, 0);
            pnlBarPending.Dock = DockStyle.Left;
            pnlBarPending.Location = new Point(0, 0);
            pnlBarPending.Name = "pnlBarPending";
            pnlBarPending.Size = new Size(5, 68);
            pnlBarPending.TabIndex = 0;
            // 
            // lblTotalPending
            // 
            lblTotalPending.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTotalPending.ForeColor = Color.FromArgb(211, 84, 0);
            lblTotalPending.Location = new Point(14, 6);
            lblTotalPending.Name = "lblTotalPending";
            lblTotalPending.Size = new Size(100, 38);
            lblTotalPending.TabIndex = 1;
            lblTotalPending.Text = "0";
            // 
            // lblPendingLbl
            // 
            lblPendingLbl.Font = new Font("Segoe UI", 9F);
            lblPendingLbl.ForeColor = Color.Gray;
            lblPendingLbl.Location = new Point(14, 46);
            lblPendingLbl.Name = "lblPendingLbl";
            lblPendingLbl.Size = new Size(155, 18);
            lblPendingLbl.TabIndex = 2;
            lblPendingLbl.Text = "Pending Submissions";
            // 
            // flpCourseCards
            // 
            flpCourseCards.AutoScroll = true;
            flpCourseCards.BackColor = Color.FromArgb(237, 237, 242);
            flpCourseCards.Dock = DockStyle.Fill;
            flpCourseCards.Location = new Point(0, 158);
            flpCourseCards.Name = "flpCourseCards";
            flpCourseCards.Padding = new Padding(10);
            flpCourseCards.Size = new Size(1680, 831);
            flpCourseCards.TabIndex = 2;
            // 
            // CourseManagementDashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(flpCourseCards);
            Controls.Add(pnlStats);
            Controls.Add(pnlHeader);
            Name = "CourseManagementDashboard";
            Size = new Size(1680, 989);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlStats.ResumeLayout(false);
            pnlStatCourses.ResumeLayout(false);
            pnlStatActivities.ResumeLayout(false);
            pnlStatStudents.ResumeLayout(false);
            pnlStatPending.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblDashboardTitle;
        private System.Windows.Forms.TextBox txtSearchCourse;
        private System.Windows.Forms.Panel pnlStats;
        private System.Windows.Forms.Panel pnlStatCourses;
        private System.Windows.Forms.Panel pnlBarCourses;
        private System.Windows.Forms.Label lblTotalCourses;
        private System.Windows.Forms.Label lblCoursesLbl;
        private System.Windows.Forms.Panel pnlStatActivities;
        private System.Windows.Forms.Panel pnlBarActivities;
        private System.Windows.Forms.Label lblTotalActivities;
        private System.Windows.Forms.Label lblActivitiesLbl;
        private System.Windows.Forms.Panel pnlStatStudents;
        private System.Windows.Forms.Panel pnlBarStudents;
        private System.Windows.Forms.Label lblTotalStudents;
        private System.Windows.Forms.Label lblStudentsLbl;
        private System.Windows.Forms.Panel pnlStatPending;
        private System.Windows.Forms.Panel pnlBarPending;
        private System.Windows.Forms.Label lblTotalPending;
        private System.Windows.Forms.Label lblPendingLbl;
        private System.Windows.Forms.FlowLayoutPanel flpCourseCards;
    }
}