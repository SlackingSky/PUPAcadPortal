namespace PUPAcadPortal.PortalContents.Student.LMS.Course
{
    partial class StudentActivityList
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing) { _searchTimer?.Dispose(); components?.Dispose(); }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            btnBack = new buttonRounded();
            lblTitle = new Label();
            lblInstructor = new Label();
            pnlToolbar = new Panel();
            txtSearch = new TextBox();
            lblFilter = new Label();
            cmbFilter = new ComboBox();
            lblStatus = new Label();
            cmbStatus = new ComboBox();
            pnlSummary = new Panel();
            flp = new FlowLayoutPanel();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.Maroon;
            pnlHeader.Controls.Add(btnBack);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblInstructor);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1640, 80);
            pnlHeader.TabIndex = 3;
            // 
            // btnBack
            // 
            btnBack.BackColor = Color.FromArgb(110, 0, 0);
            btnBack.BorderRadius = 10;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBack.ForeColor = Color.White;
            btnBack.Location = new Point(12, 24);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(80, 32);
            btnBack.TabIndex = 0;
            btnBack.Text = "← Back";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += btnBack_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(106, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(141, 25);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Course — Code";
            // 
            // lblInstructor
            // 
            lblInstructor.AutoSize = true;
            lblInstructor.Font = new Font("Segoe UI", 8.5F);
            lblInstructor.ForeColor = Color.FromArgb(215, 175, 175);
            lblInstructor.Location = new Point(108, 40);
            lblInstructor.Name = "lblInstructor";
            lblInstructor.Size = new Size(152, 15);
            lblInstructor.TabIndex = 2;
            lblInstructor.Text = "👤  Instructor    🕐  Schedule";
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = Color.FromArgb(252, 252, 252);
            pnlToolbar.Controls.Add(cmbStatus);
            pnlToolbar.Controls.Add(txtSearch);
            pnlToolbar.Controls.Add(lblFilter);
            pnlToolbar.Controls.Add(cmbFilter);
            pnlToolbar.Controls.Add(lblStatus);
            pnlToolbar.Controls.Add(pnlSummary);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(0, 80);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(1640, 50);
            pnlToolbar.TabIndex = 2;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.Location = new Point(12, 12);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Search activities...";
            txtSearch.Size = new Size(230, 25);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // lblFilter
            // 
            lblFilter.AutoSize = true;
            lblFilter.Font = new Font("Segoe UI", 9.5F);
            lblFilter.Location = new Point(256, 15);
            lblFilter.Name = "lblFilter";
            lblFilter.Size = new Size(38, 17);
            lblFilter.TabIndex = 1;
            lblFilter.Text = "Type:";
            // 
            // cmbFilter
            // 
            cmbFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilter.Font = new Font("Segoe UI", 9.5F);
            cmbFilter.FormattingEnabled = true;
            cmbFilter.Items.AddRange(new object[] { "All", "Quiz", "LongQuiz", "Essay", "FileUpload", "Recitation" });
            cmbFilter.Location = new Point(294, 12);
            cmbFilter.Name = "cmbFilter";
            cmbFilter.Size = new Size(150, 25);
            cmbFilter.TabIndex = 2;
            cmbFilter.SelectedIndexChanged += cmbFilter_SelectedIndexChanged;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9.5F);
            lblStatus.Location = new Point(460, 15);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(46, 17);
            lblStatus.TabIndex = 3;
            lblStatus.Text = "Status:";
            // 
            // cmbStatus
            // 
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Font = new Font("Segoe UI", 9.5F);
            cmbStatus.FormattingEnabled = true;
            cmbStatus.Items.AddRange(new object[] { "All Status", "Pending", "Submitted", "Returned", "Late", "Overdue" });
            cmbStatus.Location = new Point(504, 12);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(150, 25);
            cmbStatus.TabIndex = 4;
            cmbStatus.SelectedIndexChanged += cmbStatus_SelectedIndexChanged;
            // 
            // pnlSummary
            // 
            pnlSummary.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pnlSummary.BackColor = Color.Transparent;
            pnlSummary.Location = new Point(1200, 8);
            pnlSummary.Name = "pnlSummary";
            pnlSummary.Size = new Size(430, 34);
            pnlSummary.TabIndex = 5;
            // 
            // flp
            // 
            flp.AutoScroll = true;
            flp.BackColor = Color.FromArgb(244, 244, 246);
            flp.Dock = DockStyle.Fill;
            flp.FlowDirection = FlowDirection.TopDown;
            flp.Location = new Point(0, 130);
            flp.Name = "flp";
            flp.Padding = new Padding(12);
            flp.Size = new Size(1640, 859);
            flp.TabIndex = 0;
            flp.WrapContents = false;
            flp.SizeChanged += flp_SizeChanged;
            // 
            // StudentActivityList
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 244, 246);
            Controls.Add(flp);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            Name = "StudentActivityList";
            Size = new Size(1640, 989);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            ResumeLayout(false);
        }
        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private buttonRounded btnBack;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblInstructor;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.ComboBox cmbFilter;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.FlowLayoutPanel flp;
    }
}