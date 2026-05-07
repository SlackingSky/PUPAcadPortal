namespace PUPAcadPortal
{
    partial class StudentActivityList
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
            lblTitle = new Label();
            pnlToolbar = new Panel();
            txtSearch = new TextBox();
            lblFilter = new Label();
            cmbFilter = new ComboBox();
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
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1640, 56);
            pnlHeader.TabIndex = 2;
            // 
            // btnBack
            // 
            btnBack.BackColor = Color.FromArgb(109, 0, 0);
            btnBack.BorderRadius = 10;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBack.ForeColor = Color.White;
            btnBack.Location = new Point(10, 12);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(80, 32);
            btnBack.TabIndex = 0;
            btnBack.Text = "< Back";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += btnBack_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(102, 14);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(141, 25);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Course — Code";
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = Color.FromArgb(250, 250, 250);
            pnlToolbar.Controls.Add(txtSearch);
            pnlToolbar.Controls.Add(lblFilter);
            pnlToolbar.Controls.Add(cmbFilter);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(0, 56);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(1640, 48);
            pnlToolbar.TabIndex = 1;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.Location = new Point(12, 11);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Search activities...";
            txtSearch.Size = new Size(220, 25);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // lblFilter
            // 
            lblFilter.AutoSize = true;
            lblFilter.Font = new Font("Segoe UI", 10F);
            lblFilter.Location = new Point(248, 15);
            lblFilter.Name = "lblFilter";
            lblFilter.Size = new Size(42, 19);
            lblFilter.TabIndex = 1;
            lblFilter.Text = "Filter:";
            // 
            // cmbFilter
            // 
            cmbFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilter.Font = new Font("Segoe UI", 10F);
            cmbFilter.Items.AddRange(new object[] { "All", "Assignment", "Quiz", "Essay", "FileUpload" });
            cmbFilter.Location = new Point(298, 11);
            cmbFilter.Name = "cmbFilter";
            cmbFilter.Size = new Size(140, 25);
            cmbFilter.TabIndex = 2;
            cmbFilter.SelectedIndexChanged += cmbFilter_SelectedIndexChanged;
            // 
            // flp
            // 
            flp.AutoScroll = true;
            flp.BackColor = Color.FromArgb(245, 245, 245);
            flp.Dock = DockStyle.Fill;
            flp.FlowDirection = FlowDirection.TopDown;
            flp.Location = new Point(0, 104);
            flp.Name = "flp";
            flp.Padding = new Padding(10);
            flp.Size = new Size(1640, 885);
            flp.TabIndex = 0;
            flp.WrapContents = false;
            flp.SizeChanged += flp_SizeChanged;
            // 
            // StudentActivityList
            // 
            BackColor = Color.FromArgb(245, 245, 245);
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

        // ── Control declarations ───────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlHeader;
        private buttonRounded btnBack;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.ComboBox cmbFilter;
        private System.Windows.Forms.FlowLayoutPanel flp;
    }
}