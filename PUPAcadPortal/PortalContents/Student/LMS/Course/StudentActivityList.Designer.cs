namespace PUPAcadPortal
{
    partial class StudentActivityList
    {
        private System.ComponentModel.IContainer components = null;

        

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnBack = new buttonRounded();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.cmbFilter = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.flp = new System.Windows.Forms.FlowLayoutPanel();

            this.pnlHeader.SuspendLayout();
            this.pnlToolbar.SuspendLayout();
            this.SuspendLayout();

            this.pnlHeader.BackColor = System.Drawing.Color.Maroon;
            this.pnlHeader.Controls.Add(this.btnBack);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1640, 56);
            this.pnlHeader.TabIndex = 2;

            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnBack.BorderRadius = 10;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(10, 12);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(80, 32);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "← Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(102, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(142, 25);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Course — Code";

            this.pnlToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.pnlToolbar.Controls.Add(this.txtSearch);
            this.pnlToolbar.Controls.Add(this.lblFilter);
            this.pnlToolbar.Controls.Add(this.cmbFilter);
            this.pnlToolbar.Controls.Add(this.lblStatus);
            this.pnlToolbar.Controls.Add(this.cmbStatus);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbar.Location = new System.Drawing.Point(0, 56);
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.Size = new System.Drawing.Size(1640, 50);
            this.pnlToolbar.TabIndex = 1;

            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.Location = new System.Drawing.Point(12, 12);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "Search activities...";
            this.txtSearch.Size = new System.Drawing.Size(220, 25);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);

            this.lblFilter.AutoSize = true;
            this.lblFilter.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblFilter.Location = new System.Drawing.Point(248, 15);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(38, 17);
            this.lblFilter.TabIndex = 1;
            this.lblFilter.Text = "Type:";

            this.cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilter.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbFilter.FormattingEnabled = true;
            this.cmbFilter.Items.AddRange(new object[] {
            "All",
            "Quiz",
            "LongQuiz",
            "Essay",
            "FileUpload",
            "Recitation"});
            this.cmbFilter.Location = new System.Drawing.Point(288, 12);
            this.cmbFilter.Name = "cmbFilter";
            this.cmbFilter.Size = new System.Drawing.Size(140, 25);
            this.cmbFilter.TabIndex = 2;
            this.cmbFilter.SelectedIndexChanged += new System.EventHandler(this.cmbFilter_SelectedIndexChanged);

            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblStatus.Location = new System.Drawing.Point(444, 15);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(46, 17);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Status:";

            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] {
            "All Status",
            "Pending",
            "Submitted",
            "Returned",
            "Late",
            "Overdue"});
            this.cmbStatus.Location = new System.Drawing.Point(492, 12);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(140, 25);
            this.cmbStatus.TabIndex = 4;
            this.cmbStatus.SelectedIndexChanged += new System.EventHandler(this.cmbStatus_SelectedIndexChanged);

            this.flp.AutoScroll = true;
            this.flp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.flp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flp.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flp.Location = new System.Drawing.Point(0, 106);
            this.flp.Name = "flp";
            this.flp.Padding = new System.Windows.Forms.Padding(10);
            this.flp.Size = new System.Drawing.Size(1640, 883);
            this.flp.TabIndex = 5;
            this.flp.WrapContents = false;
            this.flp.SizeChanged += new System.EventHandler(this.flp_SizeChanged);

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.Controls.Add(this.flp);
            this.Controls.Add(this.pnlToolbar);
            this.Controls.Add(this.pnlHeader);
            this.Name = "StudentActivityList";
            this.Size = new System.Drawing.Size(1640, 989);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlToolbar.ResumeLayout(false);
            this.pnlToolbar.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private buttonRounded btnBack;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.ComboBox cmbFilter;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.FlowLayoutPanel flp;
    }
}