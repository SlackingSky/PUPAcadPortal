using System.Reflection.Emit;

namespace PUPAcadPortal
{
    partial class ActivityDashboard
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlTopBar = new Panel();
            lblDashboardTitle = new System.Windows.Forms.Label();
            txtSearchCourse = new TextBox();
            cmbFilterCourse = new ComboBox();
            pnlSummaryStats = new Panel();
            pnlStatCourses = new Panel();
            pnlStatActivities = new Panel();
            pnlStatPending = new Panel();
            pnlStatChecked = new Panel();
            lblTotalCourses = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            lblTotalActivities = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            lblTotalPending = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            lblTotalChecked = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            flpCourseCards = new FlowLayoutPanel();
            pnlTopBar.SuspendLayout();
            pnlSummaryStats.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTopBar
            // 
            pnlTopBar.BackColor = Color.Maroon;
            pnlTopBar.Controls.Add(lblDashboardTitle);
            pnlTopBar.Controls.Add(txtSearchCourse);
            pnlTopBar.Controls.Add(cmbFilterCourse);
            pnlTopBar.Dock = DockStyle.Top;
            pnlTopBar.Location = new Point(0, 0);
            pnlTopBar.Name = "pnlTopBar";
            pnlTopBar.Size = new Size(1661, 60);
            pnlTopBar.TabIndex = 2;
            // 
            // lblDashboardTitle
            // 
            lblDashboardTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblDashboardTitle.ForeColor = Color.White;
            lblDashboardTitle.Location = new Point(15, 15);
            lblDashboardTitle.Name = "lblDashboardTitle";
            lblDashboardTitle.Size = new Size(280, 30);
            lblDashboardTitle.TabIndex = 0;
            lblDashboardTitle.Text = "Activity Dashboard";
            // 
            // txtSearchCourse
            // 
            txtSearchCourse.Font = new Font("Segoe UI", 10F);
            txtSearchCourse.Location = new Point(1247, 17);
            txtSearchCourse.Name = "txtSearchCourse";
            txtSearchCourse.PlaceholderText = "Search course...";
            txtSearchCourse.Size = new Size(220, 25);
            txtSearchCourse.TabIndex = 1;
            txtSearchCourse.TextChanged += txtSearchCourse_TextChanged;
            // 
            // cmbFilterCourse
            // 
            cmbFilterCourse.Font = new Font("Segoe UI", 10F);
            cmbFilterCourse.Items.AddRange(new object[] { "All", "Active", "Ongoing", "Completed" });
            cmbFilterCourse.Location = new Point(1479, 17);
            cmbFilterCourse.Name = "cmbFilterCourse";
            cmbFilterCourse.Size = new Size(150, 25);
            cmbFilterCourse.TabIndex = 2;
            cmbFilterCourse.SelectedIndexChanged += cmbFilterCourse_SelectedIndexChanged;
            // 
            // pnlSummaryStats
            // 
            pnlSummaryStats.BackColor = Color.FromArgb(245, 245, 245);
            pnlSummaryStats.Controls.Add(pnlStatCourses);
            pnlSummaryStats.Controls.Add(pnlStatActivities);
            pnlSummaryStats.Controls.Add(pnlStatPending);
            pnlSummaryStats.Controls.Add(pnlStatChecked);
            pnlSummaryStats.Dock = DockStyle.Top;
            pnlSummaryStats.Location = new Point(0, 60);
            pnlSummaryStats.Name = "pnlSummaryStats";
            pnlSummaryStats.Padding = new Padding(15, 15, 0, 0);
            pnlSummaryStats.Size = new Size(1661, 90);
            pnlSummaryStats.TabIndex = 1;
            // 
            // pnlStatCourses
            // 
            pnlStatCourses.Location = new Point(0, 0);
            pnlStatCourses.Name = "pnlStatCourses";
            pnlStatCourses.Size = new Size(200, 100);
            pnlStatCourses.TabIndex = 0;
            // 
            // pnlStatActivities
            // 
            pnlStatActivities.Location = new Point(0, 0);
            pnlStatActivities.Name = "pnlStatActivities";
            pnlStatActivities.Size = new Size(200, 100);
            pnlStatActivities.TabIndex = 1;
            // 
            // pnlStatPending
            // 
            pnlStatPending.Location = new Point(0, 0);
            pnlStatPending.Name = "pnlStatPending";
            pnlStatPending.Size = new Size(200, 100);
            pnlStatPending.TabIndex = 2;
            // 
            // pnlStatChecked
            // 
            pnlStatChecked.Location = new Point(0, 0);
            pnlStatChecked.Name = "pnlStatChecked";
            pnlStatChecked.Size = new Size(200, 100);
            pnlStatChecked.TabIndex = 3;
            // 
            // lblTotalCourses
            // 
            lblTotalCourses.Location = new Point(0, 0);
            lblTotalCourses.Name = "lblTotalCourses";
            lblTotalCourses.Size = new Size(100, 23);
            lblTotalCourses.TabIndex = 0;
            // 
            // label1
            // 
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(100, 23);
            label1.TabIndex = 0;
            // 
            // lblTotalActivities
            // 
            lblTotalActivities.Location = new Point(0, 0);
            lblTotalActivities.Name = "lblTotalActivities";
            lblTotalActivities.Size = new Size(100, 23);
            lblTotalActivities.TabIndex = 0;
            // 
            // label2
            // 
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.Size = new Size(100, 23);
            label2.TabIndex = 0;
            // 
            // lblTotalPending
            // 
            lblTotalPending.Location = new Point(0, 0);
            lblTotalPending.Name = "lblTotalPending";
            lblTotalPending.Size = new Size(100, 23);
            lblTotalPending.TabIndex = 0;
            // 
            // label3
            // 
            label3.Location = new Point(0, 0);
            label3.Name = "label3";
            label3.Size = new Size(100, 23);
            label3.TabIndex = 0;
            // 
            // lblTotalChecked
            // 
            lblTotalChecked.Location = new Point(0, 0);
            lblTotalChecked.Name = "lblTotalChecked";
            lblTotalChecked.Size = new Size(100, 23);
            lblTotalChecked.TabIndex = 0;
            // 
            // label4
            // 
            label4.Location = new Point(0, 0);
            label4.Name = "label4";
            label4.Size = new Size(100, 23);
            label4.TabIndex = 0;
            // 
            // flpCourseCards
            // 
            flpCourseCards.AutoScroll = true;
            flpCourseCards.BackColor = Color.FromArgb(240, 240, 240);
            flpCourseCards.Dock = DockStyle.Fill;
            flpCourseCards.Location = new Point(0, 150);
            flpCourseCards.Name = "flpCourseCards";
            flpCourseCards.Padding = new Padding(10);
            flpCourseCards.Size = new Size(1661, 839);
            flpCourseCards.TabIndex = 0;
            // 
            // ActivityDashboard
            // 
            Controls.Add(flpCourseCards);
            Controls.Add(pnlSummaryStats);
            Controls.Add(pnlTopBar);
            Name = "ActivityDashboard";
            Size = new Size(1661, 989);
            pnlTopBar.ResumeLayout(false);
            pnlTopBar.PerformLayout();
            pnlSummaryStats.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void SetupStatPanel(System.Windows.Forms.Panel panel, System.Windows.Forms.Label lblValue,
            string value, System.Windows.Forms.Label lblName, string name, int x)
        {
            panel.BackColor = System.Drawing.Color.White;
            panel.Location = new System.Drawing.Point(x + 15, 10);
            panel.Size = new System.Drawing.Size(170, 65);
            panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            lblValue.Text = value;
            lblValue.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            lblValue.ForeColor = System.Drawing.Color.Maroon;
            lblValue.Location = new System.Drawing.Point(10, 5);
            lblValue.Size = new System.Drawing.Size(100, 38);

            lblName.Text = name;
            lblName.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblName.ForeColor = System.Drawing.Color.Gray;
            lblName.Location = new System.Drawing.Point(10, 43);
            lblName.Size = new System.Drawing.Size(150, 18);

            panel.Controls.Add(lblValue);
            panel.Controls.Add(lblName);
        }

        #endregion

        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.Label lblDashboardTitle;
        private System.Windows.Forms.TextBox txtSearchCourse;
        private System.Windows.Forms.ComboBox cmbFilterCourse;
        private System.Windows.Forms.Panel pnlSummaryStats;
        private System.Windows.Forms.Panel pnlStatCourses;
        private System.Windows.Forms.Label lblTotalCourses;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlStatActivities;
        private System.Windows.Forms.Label lblTotalActivities;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlStatPending;
        private System.Windows.Forms.Label lblTotalPending;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlStatChecked;
        private System.Windows.Forms.Label lblTotalChecked;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FlowLayoutPanel flpCourseCards;
    }
}
