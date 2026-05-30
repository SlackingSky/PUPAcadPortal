namespace PUPAcadPortal
{
    partial class AssignmentManagement
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
            pnlHeader = new Panel();
            btnBack = new buttonRounded();
            lblCourseName = new Label();
            lblCourseCode = new Label();
            btnSave = new buttonRounded();
            pnlToolbar = new Panel();
            txtSearch = new TextBox();
            cmbFilterType = new ComboBox();
            pnlSummaryBar = new Panel();
            lblSummaryBar = new Label();
            pnlScroll = new Panel();
            flpActivities = new FlowLayoutPanel();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            pnlSummaryBar.SuspendLayout();
            pnlScroll.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(128, 0, 0);
            pnlHeader.Controls.Add(btnBack);
            pnlHeader.Controls.Add(lblCourseName);
            pnlHeader.Controls.Add(lblCourseCode);
            pnlHeader.Controls.Add(btnSave);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1680, 68);
            pnlHeader.TabIndex = 0;
            // 
            // btnBack
            // 
            btnBack.BackColor = Color.FromArgb(100, 0, 0);
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBack.ForeColor = Color.White;
            btnBack.Location = new Point(12, 18);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(80, 32);
            btnBack.TabIndex = 0;
            btnBack.Text = "← Back";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += btnBack_Click;
            // 
            // lblCourseName
            // 
            lblCourseName.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblCourseName.ForeColor = Color.White;
            lblCourseName.Location = new Point(106, 10);
            lblCourseName.Name = "lblCourseName";
            lblCourseName.Size = new Size(700, 28);
            lblCourseName.TabIndex = 1;
            lblCourseName.Text = "Course Name";
            // 
            // lblCourseCode
            // 
            lblCourseCode.Font = new Font("Segoe UI", 8.5F);
            lblCourseCode.ForeColor = Color.FromArgb(230, 185, 185);
            lblCourseCode.Location = new Point(106, 40);
            lblCourseCode.Name = "lblCourseCode";
            lblCourseCode.Size = new Size(500, 18);
            lblCourseCode.TabIndex = 2;
            lblCourseCode.Text = "Course Code";
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.BackColor = Color.FromArgb(255, 196, 0);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnSave.ForeColor = Color.Black;
            btnSave.Location = new Point(1510, 17);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(158, 34);
            btnSave.TabIndex = 3;
            btnSave.Text = "+ Create Activity";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = Color.White;
            pnlToolbar.Controls.Add(txtSearch);
            pnlToolbar.Controls.Add(cmbFilterType);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(0, 68);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Padding = new Padding(14, 10, 14, 10);
            pnlToolbar.Size = new Size(1680, 50);
            pnlToolbar.TabIndex = 1;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.Location = new Point(14, 12);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "🔍  Search activities...";
            txtSearch.Size = new Size(240, 25);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // cmbFilterType
            // 
            cmbFilterType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterType.Font = new Font("Segoe UI", 10F);
            cmbFilterType.FormattingEnabled = true;
            cmbFilterType.Items.AddRange(new object[] { "All", "Assignment", "Quiz", "Essay", "FileUpload" });
            cmbFilterType.Location = new Point(268, 12);
            cmbFilterType.Name = "cmbFilterType";
            cmbFilterType.Size = new Size(150, 25);
            cmbFilterType.TabIndex = 1;
            cmbFilterType.SelectedIndexChanged += cmbFilterType_SelectedIndexChanged;
            // 
            // pnlSummaryBar
            // 
            pnlSummaryBar.BackColor = Color.FromArgb(241, 241, 246);
            pnlSummaryBar.Controls.Add(lblSummaryBar);
            pnlSummaryBar.Dock = DockStyle.Top;
            pnlSummaryBar.Location = new Point(0, 118);
            pnlSummaryBar.Name = "pnlSummaryBar";
            pnlSummaryBar.Size = new Size(1680, 30);
            pnlSummaryBar.TabIndex = 2;
            // 
            // lblSummaryBar
            // 
            lblSummaryBar.Font = new Font("Segoe UI", 8.5F);
            lblSummaryBar.ForeColor = Color.FromArgb(90, 90, 100);
            lblSummaryBar.Location = new Point(18, 7);
            lblSummaryBar.Name = "lblSummaryBar";
            lblSummaryBar.Size = new Size(900, 18);
            lblSummaryBar.TabIndex = 0;
            // 
            // pnlScroll
            // 
            pnlScroll.AutoScroll = true;
            pnlScroll.BackColor = Color.FromArgb(245, 245, 248);
            pnlScroll.Controls.Add(flpActivities);
            pnlScroll.Dock = DockStyle.Fill;
            pnlScroll.Location = new Point(0, 148);
            pnlScroll.Name = "pnlScroll";
            pnlScroll.Size = new Size(1680, 841);
            pnlScroll.TabIndex = 3;
            // 
            // flpActivities
            // 
            flpActivities.AutoSize = true;
            flpActivities.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpActivities.Dock = DockStyle.Top;
            flpActivities.FlowDirection = FlowDirection.TopDown;
            flpActivities.Location = new Point(0, 0);
            flpActivities.Name = "flpActivities";
            flpActivities.Padding = new Padding(20, 16, 20, 20);
            flpActivities.Size = new Size(1680, 36);
            flpActivities.TabIndex = 0;
            flpActivities.WrapContents = false;
            // 
            // AssignmentManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlScroll);
            Controls.Add(pnlSummaryBar);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            Name = "AssignmentManagement";
            Size = new Size(1680, 989);
            pnlHeader.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            pnlSummaryBar.ResumeLayout(false);
            pnlScroll.ResumeLayout(false);
            pnlScroll.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private buttonRounded btnBack;
        private System.Windows.Forms.Label lblCourseName;
        private System.Windows.Forms.Label lblCourseCode;
        private buttonRounded btnSave;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ComboBox cmbFilterType;
        private System.Windows.Forms.Panel pnlSummaryBar;
        private System.Windows.Forms.Label lblSummaryBar;
        private System.Windows.Forms.Panel pnlScroll;
        private System.Windows.Forms.FlowLayoutPanel flpActivities;

        
    }
}