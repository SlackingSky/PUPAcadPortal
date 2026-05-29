namespace PUPAcadPortal
{
    partial class SubmissionList
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnBack = new buttonRounded();
            this.btnReturnAll = new buttonRounded();
            this.lblActivityTitle = new System.Windows.Forms.Label();
            this.lblActivityType = new System.Windows.Forms.Label();
            this.lblMaxPoints = new System.Windows.Forms.Label();
            this.pnlStatsBar = new System.Windows.Forms.Panel();
            this.lblStats = new System.Windows.Forms.Label();
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.txtSearchStudent = new System.Windows.Forms.TextBox();
            this.lblSortLbl = new System.Windows.Forms.Label();
            this.cmbSortBy = new System.Windows.Forms.ComboBox();
            this.lblFilterLbl = new System.Windows.Forms.Label();
            this.cmbFilterStatus = new System.Windows.Forms.ComboBox();
            this.flpSubmissions = new System.Windows.Forms.FlowLayoutPanel();

            this.pnlHeader.SuspendLayout();
            this.pnlStatsBar.SuspendLayout();
            this.pnlToolbar.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            this.pnlHeader.Controls.Add(this.btnBack);
            this.pnlHeader.Controls.Add(this.btnReturnAll);
            this.pnlHeader.Controls.Add(this.lblActivityTitle);
            this.pnlHeader.Controls.Add(this.lblActivityType);
            this.pnlHeader.Controls.Add(this.lblMaxPoints);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1680, 68);
            this.pnlHeader.TabIndex = 3;
            this.pnlHeader.SizeChanged += new System.EventHandler(this.pnlHeader_SizeChanged);

            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(100, 0, 0);
            this.btnBack.BorderRadius = 10;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(10, 18);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(80, 32);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "← Back";
            this.btnBack.UseVisualStyleBackColor = false;

            // 
            // btnReturnAll
            // 
            this.btnReturnAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReturnAll.BackColor = System.Drawing.Color.DarkOrange;
            this.btnReturnAll.BorderRadius = 10;
            this.btnReturnAll.FlatAppearance.BorderSize = 0;
            this.btnReturnAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReturnAll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnReturnAll.ForeColor = System.Drawing.Color.White;
            this.btnReturnAll.Location = new System.Drawing.Point(1548, 18);
            this.btnReturnAll.Name = "btnReturnAll";
            this.btnReturnAll.Size = new System.Drawing.Size(120, 32);
            this.btnReturnAll.TabIndex = 4;
            this.btnReturnAll.Text = "Return All";
            this.btnReturnAll.UseVisualStyleBackColor = false;

            // 
            // lblActivityTitle
            // 
            this.lblActivityTitle.AutoEllipsis = true;
            this.lblActivityTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblActivityTitle.ForeColor = System.Drawing.Color.White;
            this.lblActivityTitle.Location = new System.Drawing.Point(104, 8);
            this.lblActivityTitle.Name = "lblActivityTitle";
            this.lblActivityTitle.Size = new System.Drawing.Size(700, 30);
            this.lblActivityTitle.TabIndex = 1;

            // 
            // lblActivityType
            // 
            this.lblActivityType.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblActivityType.ForeColor = System.Drawing.Color.FromArgb(225, 185, 185);
            this.lblActivityType.Location = new System.Drawing.Point(104, 40);
            this.lblActivityType.Name = "lblActivityType";
            this.lblActivityType.Size = new System.Drawing.Size(160, 20);
            this.lblActivityType.TabIndex = 2;

            // 
            // lblMaxPoints
            // 
            this.lblMaxPoints.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMaxPoints.ForeColor = System.Drawing.Color.FromArgb(255, 196, 0);
            this.lblMaxPoints.Location = new System.Drawing.Point(278, 40);
            this.lblMaxPoints.Name = "lblMaxPoints";
            this.lblMaxPoints.Size = new System.Drawing.Size(160, 20);
            this.lblMaxPoints.TabIndex = 3;

            // 
            // pnlStatsBar
            // 
            this.pnlStatsBar.BackColor = System.Drawing.Color.FromArgb(241, 241, 246);
            this.pnlStatsBar.Controls.Add(this.lblStats);
            this.pnlStatsBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStatsBar.Location = new System.Drawing.Point(0, 68);
            this.pnlStatsBar.Name = "pnlStatsBar";
            this.pnlStatsBar.Size = new System.Drawing.Size(1680, 30);
            this.pnlStatsBar.TabIndex = 2;

            // 
            // lblStats
            // 
            this.lblStats.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblStats.ForeColor = System.Drawing.Color.FromArgb(80, 80, 90);
            this.lblStats.Location = new System.Drawing.Point(16, 7);
            this.lblStats.Name = "lblStats";
            this.lblStats.Size = new System.Drawing.Size(900, 18);
            this.lblStats.TabIndex = 0;
            this.lblStats.Text = "Submitted: 0  ·  Late: 0  ·  Missing: 0  ·  Checked: 0";

            // 
            // pnlToolbar
            // 
            this.pnlToolbar.BackColor = System.Drawing.Color.White;
            this.pnlToolbar.Controls.Add(this.txtSearchStudent);
            this.pnlToolbar.Controls.Add(this.lblSortLbl);
            this.pnlToolbar.Controls.Add(this.cmbSortBy);
            this.pnlToolbar.Controls.Add(this.lblFilterLbl);
            this.pnlToolbar.Controls.Add(this.cmbFilterStatus);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbar.Location = new System.Drawing.Point(0, 98);
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.Padding = new System.Windows.Forms.Padding(14, 10, 14, 10);
            this.pnlToolbar.Size = new System.Drawing.Size(1680, 50);
            this.pnlToolbar.TabIndex = 1;

            // 
            // txtSearchStudent
            // 
            this.txtSearchStudent.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearchStudent.Location = new System.Drawing.Point(14, 12);
            this.txtSearchStudent.Name = "txtSearchStudent";
            this.txtSearchStudent.PlaceholderText = "🔍  Search student...";
            this.txtSearchStudent.Size = new System.Drawing.Size(230, 25);
            this.txtSearchStudent.TabIndex = 0;

            // 
            // lblSortLbl
            // 
            this.lblSortLbl.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSortLbl.Location = new System.Drawing.Point(260, 15);
            this.lblSortLbl.Name = "lblSortLbl";
            this.lblSortLbl.Size = new System.Drawing.Size(40, 22);
            this.lblSortLbl.TabIndex = 1;
            this.lblSortLbl.Text = "Sort:";

            // 
            // cmbSortBy
            // 
            this.cmbSortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSortBy.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbSortBy.Items.AddRange(new object[] { "Name", "Time", "Score" });
            this.cmbSortBy.Location = new System.Drawing.Point(303, 12);
            this.cmbSortBy.Name = "cmbSortBy";
            this.cmbSortBy.Size = new System.Drawing.Size(130, 25);
            this.cmbSortBy.TabIndex = 2;

            // 
            // lblFilterLbl
            // 
            this.lblFilterLbl.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblFilterLbl.Location = new System.Drawing.Point(448, 15);
            this.lblFilterLbl.Name = "lblFilterLbl";
            this.lblFilterLbl.Size = new System.Drawing.Size(60, 22);
            this.lblFilterLbl.TabIndex = 3;
            this.lblFilterLbl.Text = "Status:";

            // 
            // cmbFilterStatus
            // 
            this.cmbFilterStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbFilterStatus.Items.AddRange(new object[] { "All", "Submitted", "Late", "Missing", "Returned" });
            this.cmbFilterStatus.Location = new System.Drawing.Point(511, 12);
            this.cmbFilterStatus.Name = "cmbFilterStatus";
            this.cmbFilterStatus.Size = new System.Drawing.Size(140, 25);
            this.cmbFilterStatus.TabIndex = 4;

            // 
            // flpSubmissions
            // 
            this.flpSubmissions.AutoScroll = true;
            this.flpSubmissions.BackColor = System.Drawing.Color.FromArgb(245, 245, 248);
            this.flpSubmissions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpSubmissions.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpSubmissions.Location = new System.Drawing.Point(0, 148);
            this.flpSubmissions.Name = "flpSubmissions";
            this.flpSubmissions.Padding = new System.Windows.Forms.Padding(10);
            this.flpSubmissions.Size = new System.Drawing.Size(1680, 841);
            this.flpSubmissions.TabIndex = 0;
            this.flpSubmissions.WrapContents = false;

            // 
            // SubmissionList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flpSubmissions);
            this.Controls.Add(this.pnlToolbar);
            this.Controls.Add(this.pnlStatsBar);
            this.Controls.Add(this.pnlHeader);
            this.Name = "SubmissionList";
            this.Size = new System.Drawing.Size(1680, 989);

            this.pnlHeader.ResumeLayout(false);
            this.pnlStatsBar.ResumeLayout(false);
            this.pnlToolbar.ResumeLayout(false);
            this.pnlToolbar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private buttonRounded btnBack;
        private buttonRounded btnReturnAll;
        private System.Windows.Forms.Label lblActivityTitle;
        private System.Windows.Forms.Label lblActivityType;
        private System.Windows.Forms.Label lblMaxPoints;
        private System.Windows.Forms.Panel pnlStatsBar;
        private System.Windows.Forms.Label lblStats;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.TextBox txtSearchStudent;
        private System.Windows.Forms.Label lblSortLbl;
        private System.Windows.Forms.ComboBox cmbSortBy;
        private System.Windows.Forms.Label lblFilterLbl;
        private System.Windows.Forms.ComboBox cmbFilterStatus;
        private System.Windows.Forms.FlowLayoutPanel flpSubmissions;
    }
}