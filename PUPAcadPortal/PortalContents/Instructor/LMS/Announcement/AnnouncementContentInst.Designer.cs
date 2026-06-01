namespace PUPAcadPortal
{
    partial class AnnouncementContentInst
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
            panelTop = new Panel();
            btnInbox = new Button();
            btnCreateAnnouncement = new Button();
            cmbSortBy = new ComboBox();
            cmbFilter = new ComboBox();
            textBox25 = new TextBox();
            panelLeft = new Panel();
            flpPinned = new Panel();
            lblInsightsTitle = new Label();
            flpPinnedpnlInsights = new FlowLayoutPanel();
            lblPinnedTitle = new Label();
            flpCategories = new FlowLayoutPanel();
            lblCatTitle = new Label();
            pnlAnnouncement = new Panel();
            flowLayoutPanelAnnouncements = new FlowLayoutPanel();
            lblShowing = new Label();
            panelTop.SuspendLayout();
            panelLeft.SuspendLayout();
            pnlAnnouncement.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.White;
            panelTop.Controls.Add(btnInbox);
            panelTop.Controls.Add(btnCreateAnnouncement);
            panelTop.Controls.Add(cmbSortBy);
            panelTop.Controls.Add(cmbFilter);
            panelTop.Controls.Add(textBox25);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1024, 65);
            panelTop.TabIndex = 0;
            // 
            // btnInbox
            // 
            btnInbox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnInbox.BackColor = Color.FromArgb(224, 224, 224);
            btnInbox.FlatAppearance.BorderSize = 0;
            btnInbox.FlatStyle = FlatStyle.Flat;
            btnInbox.Font = new Font("Segoe UI", 9F);
            btnInbox.Location = new Point(920, 16);
            btnInbox.Name = "btnInbox";
            btnInbox.Size = new Size(85, 33);
            btnInbox.TabIndex = 4;
            btnInbox.Text = "Inbox";
            btnInbox.UseVisualStyleBackColor = false;
            // 
            // btnCreateAnnouncement
            // 
            btnCreateAnnouncement.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCreateAnnouncement.BackColor = Color.FromArgb(139, 0, 0);
            btnCreateAnnouncement.FlatAppearance.BorderSize = 0;
            btnCreateAnnouncement.FlatStyle = FlatStyle.Flat;
            btnCreateAnnouncement.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCreateAnnouncement.ForeColor = Color.White;
            btnCreateAnnouncement.Location = new Point(740, 16);
            btnCreateAnnouncement.Name = "btnCreateAnnouncement";
            btnCreateAnnouncement.Size = new Size(170, 33);
            btnCreateAnnouncement.TabIndex = 3;
            btnCreateAnnouncement.Text = "+ Create Announcement";
            btnCreateAnnouncement.UseVisualStyleBackColor = false;
            btnCreateAnnouncement.Click += btnCreateAnnouncement_Click;
            // 
            // cmbSortBy
            // 
            cmbSortBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSortBy.Font = new Font("Segoe UI", 9.75F);
            cmbSortBy.FormattingEnabled = true;
            cmbSortBy.Location = new Point(440, 20);
            cmbSortBy.Name = "cmbSortBy";
            cmbSortBy.Size = new Size(160, 25);
            cmbSortBy.TabIndex = 2;
            // 
            // cmbFilter
            // 
            cmbFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilter.Font = new Font("Segoe UI", 9.75F);
            cmbFilter.FormattingEnabled = true;
            cmbFilter.Location = new Point(270, 20);
            cmbFilter.Name = "cmbFilter";
            cmbFilter.Size = new Size(160, 25);
            cmbFilter.TabIndex = 1;
            // 
            // textBox25
            // 
            textBox25.Font = new Font("Segoe UI", 10F);
            textBox25.Location = new Point(20, 20);
            textBox25.Name = "textBox25";
            textBox25.PlaceholderText = "Search announcements...";
            textBox25.Size = new Size(240, 25);
            textBox25.TabIndex = 0;
            // 
            // panelLeft
            // 
            panelLeft.BackColor = Color.White;
            panelLeft.Controls.Add(flpPinned);
            panelLeft.Controls.Add(lblInsightsTitle);
            panelLeft.Controls.Add(flpPinnedpnlInsights);
            panelLeft.Controls.Add(lblPinnedTitle);
            panelLeft.Controls.Add(flpCategories);
            panelLeft.Controls.Add(lblCatTitle);
            panelLeft.Dock = DockStyle.Right;
            panelLeft.Location = new Point(746, 65);
            panelLeft.Name = "panelLeft";
            panelLeft.Padding = new Padding(15);
            panelLeft.Size = new Size(278, 635);
            panelLeft.TabIndex = 1;
            panelLeft.Paint += panelLeft_Paint;
            // 
            // flpPinned
            // 
            flpPinned.Dock = DockStyle.Top;
            flpPinned.Location = new Point(15, 467);
            flpPinned.Name = "flpPinned";
            flpPinned.Size = new Size(248, 153);
            flpPinned.TabIndex = 5;
            // 
            // lblInsightsTitle
            // 
            lblInsightsTitle.Dock = DockStyle.Top;
            lblInsightsTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblInsightsTitle.Location = new Point(15, 437);
            lblInsightsTitle.Name = "lblInsightsTitle";
            lblInsightsTitle.Padding = new Padding(0, 10, 0, 5);
            lblInsightsTitle.Size = new Size(248, 30);
            lblInsightsTitle.TabIndex = 4;
            lblInsightsTitle.Text = "Pinned Announcements";
            // 
            // flpPinnedpnlInsights
            // 
            flpPinnedpnlInsights.Dock = DockStyle.Top;
            flpPinnedpnlInsights.Location = new Point(15, 237);
            flpPinnedpnlInsights.Name = "flpPinnedpnlInsights";
            flpPinnedpnlInsights.Size = new Size(248, 200);
            flpPinnedpnlInsights.TabIndex = 3;
            // 
            // lblPinnedTitle
            // 
            lblPinnedTitle.Dock = DockStyle.Top;
            lblPinnedTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPinnedTitle.Location = new Point(15, 207);
            lblPinnedTitle.Name = "lblPinnedTitle";
            lblPinnedTitle.Padding = new Padding(0, 10, 0, 5);
            lblPinnedTitle.Size = new Size(248, 30);
            lblPinnedTitle.TabIndex = 6;
            lblPinnedTitle.Text = "Insights";
            // 
            // flpCategories
            // 
            flpCategories.Dock = DockStyle.Top;
            flpCategories.Location = new Point(15, 45);
            flpCategories.Name = "flpCategories";
            flpCategories.Size = new Size(248, 162);
            flpCategories.TabIndex = 1;
            // 
            // lblCatTitle
            // 
            lblCatTitle.Dock = DockStyle.Top;
            lblCatTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCatTitle.Location = new Point(15, 15);
            lblCatTitle.Name = "lblCatTitle";
            lblCatTitle.Padding = new Padding(0, 0, 0, 5);
            lblCatTitle.Size = new Size(248, 30);
            lblCatTitle.TabIndex = 0;
            lblCatTitle.Text = "Categories";
            // 
            // pnlAnnouncement
            // 
            pnlAnnouncement.BackColor = Color.FromArgb(248, 249, 250);
            pnlAnnouncement.Controls.Add(flowLayoutPanelAnnouncements);
            pnlAnnouncement.Controls.Add(lblShowing);
            pnlAnnouncement.Dock = DockStyle.Fill;
            pnlAnnouncement.Location = new Point(0, 65);
            pnlAnnouncement.Name = "pnlAnnouncement";
            pnlAnnouncement.Padding = new Padding(20);
            pnlAnnouncement.Size = new Size(746, 635);
            pnlAnnouncement.TabIndex = 2;
            // 
            // flowLayoutPanelAnnouncements
            // 
            flowLayoutPanelAnnouncements.Dock = DockStyle.Fill;
            flowLayoutPanelAnnouncements.Location = new Point(20, 50);
            flowLayoutPanelAnnouncements.Name = "flowLayoutPanelAnnouncements";
            flowLayoutPanelAnnouncements.Size = new Size(706, 565);
            flowLayoutPanelAnnouncements.TabIndex = 1;
            // 
            // lblShowing
            // 
            lblShowing.Dock = DockStyle.Top;
            lblShowing.Font = new Font("Segoe UI", 9F);
            lblShowing.ForeColor = Color.Gray;
            lblShowing.Location = new Point(20, 20);
            lblShowing.Name = "lblShowing";
            lblShowing.Size = new Size(706, 30);
            lblShowing.TabIndex = 0;
            lblShowing.Text = "Showing 1–x of y announcements";
            lblShowing.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // AnnouncementContentInst
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlAnnouncement);
            Controls.Add(panelLeft);
            Controls.Add(panelTop);
            MinimumSize = new Size(1024, 700);
            Name = "AnnouncementContentInst";
            Size = new Size(1024, 700);
            Load += AnnouncementContentInst_Load;
            Resize += AnnouncementContentInst_Resize;
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelLeft.ResumeLayout(false);
            pnlAnnouncement.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.TextBox textBox25;
        private System.Windows.Forms.ComboBox cmbSortBy;
        private System.Windows.Forms.ComboBox cmbFilter;
        private System.Windows.Forms.Button btnCreateAnnouncement;
        private System.Windows.Forms.Button btnInbox;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.FlowLayoutPanel flpCategories;
        private System.Windows.Forms.Label lblCatTitle;
        private System.Windows.Forms.Panel flpPinned;
        private System.Windows.Forms.Label lblInsightsTitle;
        private System.Windows.Forms.FlowLayoutPanel flpPinnedpnlInsights;
        private System.Windows.Forms.Label lblPinnedTitle;
        private System.Windows.Forms.Panel pnlAnnouncement;
        private System.Windows.Forms.Label lblShowing;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelAnnouncements;
    }
}