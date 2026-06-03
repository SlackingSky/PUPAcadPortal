using System;
using System.Drawing;
using System.Windows.Forms;

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
            pnlTopBar = new Panel();
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
            pnlStatPending = new Panel();
            pnlBarPending = new Panel();
            lblTotalPending = new Label();
            lblPendingLbl = new Label();
            pnlStatChecked = new Panel();
            pnlBarChecked = new Panel();
            lblTotalChecked = new Label();
            lblCheckedLbl = new Label();
            flpCourseCards = new FlowLayoutPanel();
            pnlTopBar.SuspendLayout();
            pnlStats.SuspendLayout();
            pnlStatCourses.SuspendLayout();
            pnlStatActivities.SuspendLayout();
            pnlStatPending.SuspendLayout();
            pnlStatChecked.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTopBar
            // 
            pnlTopBar.BackColor = Color.FromArgb(128, 0, 0);
            pnlTopBar.Controls.Add(lblDashboardTitle);
            pnlTopBar.Controls.Add(txtSearchCourse);
            pnlTopBar.Dock = DockStyle.Top;
            pnlTopBar.Location = new Point(0, 0);
            pnlTopBar.Name = "pnlTopBar";
            pnlTopBar.Size = new Size(1680, 62);
            pnlTopBar.TabIndex = 3;
            // 
            // lblDashboardTitle
            // 
            lblDashboardTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblDashboardTitle.ForeColor = Color.White;
            lblDashboardTitle.Location = new Point(18, 14);
            lblDashboardTitle.Name = "lblDashboardTitle";
            lblDashboardTitle.Size = new Size(320, 32);
            lblDashboardTitle.TabIndex = 0;
            lblDashboardTitle.Text = "📚  Activity Dashboard";
            // 
            // txtSearchCourse
            // 
            txtSearchCourse.Font = new Font("Segoe UI", 10F);
            txtSearchCourse.Location = new Point(1400, 21);
            txtSearchCourse.Name = "txtSearchCourse";
            txtSearchCourse.PlaceholderText = "🔍  Search course...";
            txtSearchCourse.Size = new Size(220, 25);
            txtSearchCourse.TabIndex = 1;
            txtSearchCourse.TextChanged += txtSearchCourse_TextChanged;
            // 
            // pnlStats
            // 
            pnlStats.BackColor = Color.FromArgb(245, 245, 248);
            pnlStats.Controls.Add(pnlStatCourses);
            pnlStats.Controls.Add(pnlStatActivities);
            pnlStats.Controls.Add(pnlStatPending);
            pnlStats.Controls.Add(pnlStatChecked);
            pnlStats.Dock = DockStyle.Top;
            pnlStats.Location = new Point(0, 62);
            pnlStats.Name = "pnlStats";
            pnlStats.Padding = new Padding(18, 12, 18, 12);
            pnlStats.Size = new Size(1680, 92);
            pnlStats.TabIndex = 2;
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
            lblActivitiesLbl.Text = "Activities";
            // 
            // pnlStatPending
            // 
            pnlStatPending.BackColor = Color.White;
            pnlStatPending.Controls.Add(pnlBarPending);
            pnlStatPending.Controls.Add(lblTotalPending);
            pnlStatPending.Controls.Add(lblPendingLbl);
            pnlStatPending.Location = new Point(420, 12);
            pnlStatPending.Name = "pnlStatPending";
            pnlStatPending.Size = new Size(178, 68);
            pnlStatPending.TabIndex = 2;
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
            lblPendingLbl.Text = "Pending";
            // 
            // pnlStatChecked
            // 
            pnlStatChecked.BackColor = Color.White;
            pnlStatChecked.Controls.Add(pnlBarChecked);
            pnlStatChecked.Controls.Add(lblTotalChecked);
            pnlStatChecked.Controls.Add(lblCheckedLbl);
            pnlStatChecked.Location = new Point(612, 12);
            pnlStatChecked.Name = "pnlStatChecked";
            pnlStatChecked.Size = new Size(178, 68);
            pnlStatChecked.TabIndex = 3;
            // 
            // pnlBarChecked
            // 
            pnlBarChecked.BackColor = Color.FromArgb(46, 160, 67);
            pnlBarChecked.Dock = DockStyle.Left;
            pnlBarChecked.Location = new Point(0, 0);
            pnlBarChecked.Name = "pnlBarChecked";
            pnlBarChecked.Size = new Size(5, 68);
            pnlBarChecked.TabIndex = 0;
            // 
            // lblTotalChecked
            // 
            lblTotalChecked.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTotalChecked.ForeColor = Color.FromArgb(46, 160, 67);
            lblTotalChecked.Location = new Point(14, 6);
            lblTotalChecked.Name = "lblTotalChecked";
            lblTotalChecked.Size = new Size(100, 38);
            lblTotalChecked.TabIndex = 1;
            lblTotalChecked.Text = "0";
            // 
            // lblCheckedLbl
            // 
            lblCheckedLbl.Font = new Font("Segoe UI", 9F);
            lblCheckedLbl.ForeColor = Color.Gray;
            lblCheckedLbl.Location = new Point(14, 46);
            lblCheckedLbl.Name = "lblCheckedLbl";
            lblCheckedLbl.Size = new Size(155, 18);
            lblCheckedLbl.TabIndex = 2;
            lblCheckedLbl.Text = "Checked";
            // 
            // flpCourseCards
            // 
            flpCourseCards.AutoScroll = true;
            flpCourseCards.BackColor = Color.FromArgb(237, 237, 242);
            flpCourseCards.Dock = DockStyle.Fill;
            flpCourseCards.Location = new Point(0, 154);
            flpCourseCards.Name = "flpCourseCards";
            flpCourseCards.Padding = new Padding(10);
            flpCourseCards.Size = new Size(1680, 835);
            flpCourseCards.TabIndex = 0;
            // 
            // ActivityDashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(flpCourseCards);
            Controls.Add(pnlStats);
            Controls.Add(pnlTopBar);
            Name = "ActivityDashboard";
            Size = new Size(1680, 989);
            pnlTopBar.ResumeLayout(false);
            pnlTopBar.PerformLayout();
            pnlStats.ResumeLayout(false);
            pnlStatCourses.ResumeLayout(false);
            pnlStatActivities.ResumeLayout(false);
            pnlStatPending.ResumeLayout(false);
            pnlStatChecked.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlTopBar;
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