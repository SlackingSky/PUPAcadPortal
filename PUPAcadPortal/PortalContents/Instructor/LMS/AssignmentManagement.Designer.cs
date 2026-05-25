using System.Reflection.PortableExecutable;

namespace PUPAcadPortal
{
    partial class AssignmentManagement
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
            pnlHeader = new Panel();
            btnBack = new buttonRounded();
            lblCourseTitle = new Label();
            btnCreateAssignment = new buttonRounded();
            pnlToolbar = new Panel();
            txtSearchActivities = new TextBox();
            lblFilterLabel = new Label();
            cmbFilterType = new ComboBox();
            flpActivities = new FlowLayoutPanel();
            pnlPagination = new Panel();
            btnPrevPage = new buttonRounded();
            lblPageInfo = new Label();
            btnNextPage = new buttonRounded();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            pnlPagination.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.Maroon;
            pnlHeader.Controls.Add(btnBack);
            pnlHeader.Controls.Add(lblCourseTitle);
            pnlHeader.Controls.Add(btnCreateAssignment);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1661, 60);
            pnlHeader.TabIndex = 3;
            // 
            // btnBack
            // 
            btnBack.BackColor = Color.Maroon;
            btnBack.BorderRadius = 10;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBack.ForeColor = Color.White;
            btnBack.Location = new Point(15, 13);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(80, 32);
            btnBack.TabIndex = 0;
            btnBack.Text = "Back";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += btnBack_Click;
            // 
            // lblCourseTitle
            // 
            lblCourseTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblCourseTitle.ForeColor = Color.White;
            lblCourseTitle.Location = new Point(105, 15);
            lblCourseTitle.Name = "lblCourseTitle";
            lblCourseTitle.Size = new Size(700, 30);
            lblCourseTitle.TabIndex = 1;
            lblCourseTitle.Text = "Course Activities";
            // 
            // btnCreateAssignment
            // 
            btnCreateAssignment.BackColor = Color.FromArgb(255, 193, 7);
            btnCreateAssignment.BorderRadius = 10;
            btnCreateAssignment.FlatAppearance.BorderSize = 0;
            btnCreateAssignment.FlatStyle = FlatStyle.Flat;
            btnCreateAssignment.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCreateAssignment.ForeColor = Color.Black;
            btnCreateAssignment.Location = new Point(1493, 15);
            btnCreateAssignment.Name = "btnCreateAssignment";
            btnCreateAssignment.Size = new Size(150, 32);
            btnCreateAssignment.TabIndex = 2;
            btnCreateAssignment.Text = "+ Create Activity";
            btnCreateAssignment.UseVisualStyleBackColor = false;
            btnCreateAssignment.Click += btnCreateAssignment_Click;
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = Color.FromArgb(245, 245, 245);
            pnlToolbar.Controls.Add(txtSearchActivities);
            pnlToolbar.Controls.Add(lblFilterLabel);
            pnlToolbar.Controls.Add(cmbFilterType);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(0, 60);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(1661, 50);
            pnlToolbar.TabIndex = 2;
            // 
            // txtSearchActivities
            // 
            txtSearchActivities.Font = new Font("Segoe UI", 10F);
            txtSearchActivities.Location = new Point(15, 12);
            txtSearchActivities.Name = "txtSearchActivities";
            txtSearchActivities.PlaceholderText = "Search activities...";
            txtSearchActivities.Size = new Size(250, 25);
            txtSearchActivities.TabIndex = 0;
            txtSearchActivities.TextChanged += txtSearchActivities_TextChanged;
            // 
            // lblFilterLabel
            // 
            lblFilterLabel.Font = new Font("Segoe UI", 10F);
            lblFilterLabel.Location = new Point(285, 15);
            lblFilterLabel.Name = "lblFilterLabel";
            lblFilterLabel.Size = new Size(45, 22);
            lblFilterLabel.TabIndex = 1;
            lblFilterLabel.Text = "Filter:";
            // 
            // cmbFilterType
            // 
            cmbFilterType.Font = new Font("Segoe UI", 10F);
            cmbFilterType.Items.AddRange(new object[] { "All", "Assignment", "Quiz", "Essay", "FileUpload" });
            cmbFilterType.Location = new Point(335, 12);
            cmbFilterType.Name = "cmbFilterType";
            cmbFilterType.Size = new Size(140, 25);
            cmbFilterType.TabIndex = 2;
            cmbFilterType.SelectedIndexChanged += cmbFilterType_SelectedIndexChanged;
            // 
            // flpActivities
            // 
            flpActivities.AutoScroll = true;
            flpActivities.BackColor = Color.FromArgb(240, 240, 240);
            flpActivities.Dock = DockStyle.Fill;
            flpActivities.FlowDirection = FlowDirection.TopDown;
            flpActivities.Location = new Point(0, 110);
            flpActivities.Name = "flpActivities";
            flpActivities.Padding = new Padding(10);
            flpActivities.Size = new Size(1661, 834);
            flpActivities.TabIndex = 0;
            flpActivities.WrapContents = false;
            // 
            // pnlPagination
            // 
            pnlPagination.BackColor = Color.White;
            pnlPagination.Controls.Add(btnPrevPage);
            pnlPagination.Controls.Add(lblPageInfo);
            pnlPagination.Controls.Add(btnNextPage);
            pnlPagination.Dock = DockStyle.Bottom;
            pnlPagination.Location = new Point(0, 944);
            pnlPagination.Name = "pnlPagination";
            pnlPagination.Size = new Size(1661, 45);
            pnlPagination.TabIndex = 1;
            // 
            // btnPrevPage
            // 
            btnPrevPage.BackColor = Color.Maroon;
            btnPrevPage.BorderRadius = 10;
            btnPrevPage.FlatAppearance.BorderSize = 0;
            btnPrevPage.FlatStyle = FlatStyle.Flat;
            btnPrevPage.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPrevPage.ForeColor = Color.White;
            btnPrevPage.Location = new Point(480, 8);
            btnPrevPage.Name = "btnPrevPage";
            btnPrevPage.Size = new Size(80, 28);
            btnPrevPage.TabIndex = 0;
            btnPrevPage.Text = "< Prev";
            btnPrevPage.UseVisualStyleBackColor = false;
            btnPrevPage.Click += btnPrevPage_Click;
            // 
            // lblPageInfo
            // 
            lblPageInfo.Font = new Font("Segoe UI", 9F);
            lblPageInfo.ForeColor = Color.Gray;
            lblPageInfo.Location = new Point(570, 14);
            lblPageInfo.Name = "lblPageInfo";
            lblPageInfo.Size = new Size(200, 18);
            lblPageInfo.TabIndex = 1;
            lblPageInfo.Text = "Page 1 of 1";
            lblPageInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnNextPage
            // 
            btnNextPage.BackColor = Color.Maroon;
            btnNextPage.BorderRadius = 10;
            btnNextPage.FlatAppearance.BorderSize = 0;
            btnNextPage.FlatStyle = FlatStyle.Flat;
            btnNextPage.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNextPage.ForeColor = Color.White;
            btnNextPage.Location = new Point(780, 8);
            btnNextPage.Name = "btnNextPage";
            btnNextPage.Size = new Size(80, 28);
            btnNextPage.TabIndex = 2;
            btnNextPage.Text = "Next >";
            btnNextPage.UseVisualStyleBackColor = false;
            btnNextPage.Click += btnNextPage_Click;
            // 
            // AssignmentManagement
            // 
            Controls.Add(flpActivities);
            Controls.Add(pnlPagination);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            Name = "AssignmentManagement";
            Size = new Size(1661, 989);
            pnlHeader.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            pnlPagination.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private buttonRounded btnBack;
        private System.Windows.Forms.Label lblCourseTitle;
        private buttonRounded btnCreateAssignment;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.TextBox txtSearchActivities;
        private System.Windows.Forms.Label lblFilterLabel;
        private System.Windows.Forms.ComboBox cmbFilterType;
        private System.Windows.Forms.FlowLayoutPanel flpActivities;
        private System.Windows.Forms.Panel pnlPagination;
        private buttonRounded btnPrevPage;
        private System.Windows.Forms.Label lblPageInfo;
        private buttonRounded btnNextPage;
    }
}
