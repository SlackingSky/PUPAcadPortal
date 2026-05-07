using System.Reflection.PortableExecutable;

namespace PUPAcadPortal
{
    partial class SubmissionList
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
            btnBack = new buttonRounded();
            lblActivityTitle = new Label();
            lblActivityType = new Label();
            lblMaxPoints = new Label();
            pnlStats = new Panel();
            lblStats = new Label();
            pnlToolbar = new Panel();
            txtSearchStudent = new TextBox();
            lblSortLabel = new Label();
            cmbSortBy = new ComboBox();
            lblFilterLabel = new Label();
            cmbFilterStatus = new ComboBox();
            flpSubmissions = new FlowLayoutPanel();
            pnlHeader.SuspendLayout();
            pnlStats.SuspendLayout();
            pnlToolbar.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.Maroon;
            pnlHeader.Controls.Add(btnBack);
            pnlHeader.Controls.Add(lblActivityTitle);
            pnlHeader.Controls.Add(lblActivityType);
            pnlHeader.Controls.Add(lblMaxPoints);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1640, 68);
            pnlHeader.TabIndex = 3;
            // 
            // btnBack
            // 
            btnBack.BackColor = Color.FromArgb(109, 0, 0);
            btnBack.BorderRadius = 10;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBack.ForeColor = Color.White;
            btnBack.Location = new Point(8, 18);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(80, 30);
            btnBack.TabIndex = 0;
            btnBack.Text = "Back";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += btnBack_Click;
            // 
            // lblActivityTitle
            // 
            lblActivityTitle.AutoEllipsis = true;
            lblActivityTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblActivityTitle.ForeColor = Color.White;
            lblActivityTitle.Location = new Point(105, 8);
            lblActivityTitle.Name = "lblActivityTitle";
            lblActivityTitle.Size = new Size(700, 30);
            lblActivityTitle.TabIndex = 1;
            lblActivityTitle.Text = "Activity Title";
            // 
            // lblActivityType
            // 
            lblActivityType.Font = new Font("Segoe UI", 9F);
            lblActivityType.ForeColor = Color.FromArgb(220, 180, 180);
            lblActivityType.Location = new Point(105, 40);
            lblActivityType.Name = "lblActivityType";
            lblActivityType.Size = new Size(120, 20);
            lblActivityType.TabIndex = 2;
            lblActivityType.Text = "Assignment";
            // 
            // lblMaxPoints
            // 
            lblMaxPoints.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblMaxPoints.ForeColor = Color.FromArgb(255, 193, 7);
            lblMaxPoints.Location = new Point(235, 40);
            lblMaxPoints.Name = "lblMaxPoints";
            lblMaxPoints.Size = new Size(150, 20);
            lblMaxPoints.TabIndex = 3;
            lblMaxPoints.Text = "Max: 100 pts";
            // 
            // pnlStats
            // 
            pnlStats.BackColor = Color.FromArgb(240, 240, 240);
            pnlStats.Controls.Add(lblStats);
            pnlStats.Dock = DockStyle.Top;
            pnlStats.Location = new Point(0, 68);
            pnlStats.Name = "pnlStats";
            pnlStats.Size = new Size(1640, 36);
            pnlStats.TabIndex = 2;
            // 
            // lblStats
            // 
            lblStats.Font = new Font("Segoe UI", 9F);
            lblStats.ForeColor = Color.FromArgb(60, 60, 60);
            lblStats.Location = new Point(15, 9);
            lblStats.Name = "lblStats";
            lblStats.Size = new Size(700, 18);
            lblStats.TabIndex = 0;
            lblStats.Text = "Submitted: 0  |  Late: 0  |  Missing: 0  |  Checked: 0";
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = Color.White;
            pnlToolbar.Controls.Add(txtSearchStudent);
            pnlToolbar.Controls.Add(lblSortLabel);
            pnlToolbar.Controls.Add(cmbSortBy);
            pnlToolbar.Controls.Add(lblFilterLabel);
            pnlToolbar.Controls.Add(cmbFilterStatus);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(0, 104);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Padding = new Padding(10, 10, 0, 0);
            pnlToolbar.Size = new Size(1640, 50);
            pnlToolbar.TabIndex = 1;
            // 
            // txtSearchStudent
            // 
            txtSearchStudent.Font = new Font("Segoe UI", 10F);
            txtSearchStudent.Location = new Point(15, 12);
            txtSearchStudent.Name = "txtSearchStudent";
            txtSearchStudent.PlaceholderText = "Search student...";
            txtSearchStudent.Size = new Size(220, 25);
            txtSearchStudent.TabIndex = 0;
            txtSearchStudent.TextChanged += txtSearchStudent_TextChanged;
            // 
            // lblSortLabel
            // 
            lblSortLabel.Font = new Font("Segoe UI", 10F);
            lblSortLabel.Location = new Point(250, 15);
            lblSortLabel.Name = "lblSortLabel";
            lblSortLabel.Size = new Size(40, 22);
            lblSortLabel.TabIndex = 1;
            lblSortLabel.Text = "Sort:";
            // 
            // cmbSortBy
            // 
            cmbSortBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSortBy.Font = new Font("Segoe UI", 10F);
            cmbSortBy.Items.AddRange(new object[] { "Name", "Time", "Score" });
            cmbSortBy.Location = new Point(293, 12);
            cmbSortBy.Name = "cmbSortBy";
            cmbSortBy.Size = new Size(120, 25);
            cmbSortBy.TabIndex = 2;
            cmbSortBy.SelectedIndexChanged += cmbSortBy_SelectedIndexChanged;
            // 
            // lblFilterLabel
            // 
            lblFilterLabel.Font = new Font("Segoe UI", 10F);
            lblFilterLabel.Location = new Point(428, 15);
            lblFilterLabel.Name = "lblFilterLabel";
            lblFilterLabel.Size = new Size(55, 22);
            lblFilterLabel.TabIndex = 3;
            lblFilterLabel.Text = "Status:";
            // 
            // cmbFilterStatus
            // 
            cmbFilterStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterStatus.Font = new Font("Segoe UI", 10F);
            cmbFilterStatus.Items.AddRange(new object[] { "All", "Submitted", "Late", "Missing", "Returned" });
            cmbFilterStatus.Location = new Point(487, 12);
            cmbFilterStatus.Name = "cmbFilterStatus";
            cmbFilterStatus.Size = new Size(130, 25);
            cmbFilterStatus.TabIndex = 4;
            cmbFilterStatus.SelectedIndexChanged += cmbFilterStatus_SelectedIndexChanged;
            // 
            // flpSubmissions
            // 
            flpSubmissions.AutoScroll = true;
            flpSubmissions.BackColor = Color.FromArgb(245, 245, 245);
            flpSubmissions.Dock = DockStyle.Fill;
            flpSubmissions.FlowDirection = FlowDirection.TopDown;
            flpSubmissions.Location = new Point(0, 154);
            flpSubmissions.Name = "flpSubmissions";
            flpSubmissions.Padding = new Padding(10);
            flpSubmissions.Size = new Size(1640, 835);
            flpSubmissions.TabIndex = 0;
            flpSubmissions.WrapContents = false;
            // 
            // SubmissionList
            // 
            Controls.Add(flpSubmissions);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlStats);
            Controls.Add(pnlHeader);
            Name = "SubmissionList";
            Size = new Size(1640, 989);
            pnlHeader.ResumeLayout(false);
            pnlStats.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private buttonRounded btnBack;
        private System.Windows.Forms.Label lblActivityTitle;
        private System.Windows.Forms.Label lblActivityType;
        private System.Windows.Forms.Label lblMaxPoints;
        private System.Windows.Forms.Panel pnlStats;
        private System.Windows.Forms.Label lblStats;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.TextBox txtSearchStudent;
        private System.Windows.Forms.Label lblSortLabel;
        private System.Windows.Forms.ComboBox cmbSortBy;
        private System.Windows.Forms.Label lblFilterLabel;
        private System.Windows.Forms.ComboBox cmbFilterStatus;
        private System.Windows.Forms.FlowLayoutPanel flpSubmissions;
    }
}