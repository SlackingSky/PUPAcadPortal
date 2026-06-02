namespace PUPAcadPortal
{
    partial class AnnouncementContentInst
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
            panelTop = new Panel();
            btnInbox = new Button();
            btnCreateAnnouncement = new Button();
            cmbSortBy = new ComboBox();
            cmbFilter = new ComboBox();
            textBox25 = new TextBox();
            panelLeft = new Panel();
            tblSidebar = new TableLayoutPanel();
            lblPinnedTitle = new Label();
            flpPinnedpnlInsights = new FlowLayoutPanel();
            pnlMiddleRow = new Panel();
            pnlCatBox = new Panel();
            flpCategories = new FlowLayoutPanel();
            lblCatTitle = new Label();
            pnlInsightBox = new Panel();
            flpPinned = new Panel();
            lblInsightsTitle = new Label();
            pnlQuickTips = new Panel();
            btnManageNotif = new Button();
            lblQuickTipsBody = new Label();
            lblQuickTipsTitle = new Label();
            pnlAnnouncement = new Panel();
            flowLayoutPanelAnnouncements = new FlowLayoutPanel();
            lblShowing = new Label();
            panelTop.SuspendLayout();
            panelLeft.SuspendLayout();
            tblSidebar.SuspendLayout();
            pnlMiddleRow.SuspendLayout();
            pnlCatBox.SuspendLayout();
            pnlInsightBox.SuspendLayout();
            pnlQuickTips.SuspendLayout();
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
            panelLeft.BackColor = Color.FromArgb(250, 250, 251);
            panelLeft.Controls.Add(tblSidebar);
            panelLeft.Dock = DockStyle.Right;
            panelLeft.Location = new Point(744, 65);
            panelLeft.Name = "panelLeft";
            panelLeft.Padding = new Padding(10);
            panelLeft.Size = new Size(280, 635);
            panelLeft.TabIndex = 1;
            panelLeft.Paint += panelLeft_Paint;
            // 
            // tblSidebar
            // 
            tblSidebar.BackColor = Color.Transparent;
            tblSidebar.ColumnCount = 1;
            tblSidebar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblSidebar.Controls.Add(lblPinnedTitle, 0, 0);
            tblSidebar.Controls.Add(flpPinnedpnlInsights, 0, 1);
            tblSidebar.Controls.Add(pnlMiddleRow, 0, 2);
            tblSidebar.Controls.Add(pnlQuickTips, 0, 3);
            tblSidebar.Dock = DockStyle.Fill;
            tblSidebar.Location = new Point(10, 10);
            tblSidebar.Margin = new Padding(0);
            tblSidebar.Name = "tblSidebar";
            tblSidebar.RowCount = 4;
            tblSidebar.RowStyles.Add(new RowStyle());
            tblSidebar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblSidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 246F));
            tblSidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 94F));
            tblSidebar.Size = new Size(260, 615);
            tblSidebar.TabIndex = 0;
            // 
            // lblPinnedTitle
            // 
            lblPinnedTitle.Dock = DockStyle.Fill;
            lblPinnedTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPinnedTitle.ForeColor = Color.FromArgb(30, 30, 30);
            lblPinnedTitle.Location = new Point(0, 0);
            lblPinnedTitle.Margin = new Padding(0, 0, 0, 4);
            lblPinnedTitle.Name = "lblPinnedTitle";
            lblPinnedTitle.Padding = new Padding(2, 4, 0, 4);
            lblPinnedTitle.Size = new Size(260, 28);
            lblPinnedTitle.TabIndex = 0;
            lblPinnedTitle.Text = "📌  Pinned Announcements";
            // 
            // flpPinnedpnlInsights
            // 
            flpPinnedpnlInsights.AutoScroll = true;
            flpPinnedpnlInsights.BackColor = Color.White;
            flpPinnedpnlInsights.BorderStyle = BorderStyle.FixedSingle;
            flpPinnedpnlInsights.Dock = DockStyle.Fill;
            flpPinnedpnlInsights.FlowDirection = FlowDirection.TopDown;
            flpPinnedpnlInsights.Location = new Point(0, 32);
            flpPinnedpnlInsights.Margin = new Padding(0, 0, 0, 6);
            flpPinnedpnlInsights.Name = "flpPinnedpnlInsights";
            flpPinnedpnlInsights.Size = new Size(260, 237);
            flpPinnedpnlInsights.TabIndex = 1;
            flpPinnedpnlInsights.WrapContents = false;
            // 
            // pnlMiddleRow
            // 
            pnlMiddleRow.BackColor = Color.Transparent;
            pnlMiddleRow.Controls.Add(pnlCatBox);
            pnlMiddleRow.Controls.Add(pnlInsightBox);
            pnlMiddleRow.Dock = DockStyle.Fill;
            pnlMiddleRow.Location = new Point(0, 275);
            pnlMiddleRow.Margin = new Padding(0, 0, 0, 6);
            pnlMiddleRow.Name = "pnlMiddleRow";
            pnlMiddleRow.Size = new Size(260, 240);
            pnlMiddleRow.TabIndex = 2;
            // 
            // pnlCatBox
            // 
            pnlCatBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pnlCatBox.BackColor = Color.White;
            pnlCatBox.BorderStyle = BorderStyle.FixedSingle;
            pnlCatBox.Controls.Add(flpCategories);
            pnlCatBox.Controls.Add(lblCatTitle);
            pnlCatBox.Location = new Point(0, 0);
            pnlCatBox.Name = "pnlCatBox";
            pnlCatBox.Size = new Size(139, 380);
            pnlCatBox.TabIndex = 0;
            // 
            // flpCategories
            // 
            flpCategories.AutoScroll = true;
            flpCategories.BackColor = Color.White;
            flpCategories.Dock = DockStyle.Fill;
            flpCategories.FlowDirection = FlowDirection.TopDown;
            flpCategories.Location = new Point(0, 28);
            flpCategories.Name = "flpCategories";
            flpCategories.Size = new Size(137, 350);
            flpCategories.TabIndex = 0;
            flpCategories.WrapContents = false;
            // 
            // lblCatTitle
            // 
            lblCatTitle.BackColor = Color.White;
            lblCatTitle.Dock = DockStyle.Top;
            lblCatTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCatTitle.ForeColor = Color.FromArgb(30, 30, 30);
            lblCatTitle.Location = new Point(0, 0);
            lblCatTitle.Name = "lblCatTitle";
            lblCatTitle.Padding = new Padding(8, 8, 4, 4);
            lblCatTitle.Size = new Size(137, 28);
            lblCatTitle.TabIndex = 1;
            lblCatTitle.Text = "Categories";
            // 
            // pnlInsightBox
            // 
            pnlInsightBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            pnlInsightBox.BackColor = Color.White;
            pnlInsightBox.BorderStyle = BorderStyle.FixedSingle;
            pnlInsightBox.Controls.Add(flpPinned);
            pnlInsightBox.Controls.Add(lblInsightsTitle);
            pnlInsightBox.Location = new Point(144, 0);
            pnlInsightBox.Name = "pnlInsightBox";
            pnlInsightBox.Size = new Size(174, 380);
            pnlInsightBox.TabIndex = 1;
            // 
            // flpPinned
            // 
            flpPinned.BackColor = Color.White;
            flpPinned.Dock = DockStyle.Fill;
            flpPinned.Location = new Point(0, 40);
            flpPinned.Name = "flpPinned";
            flpPinned.Size = new Size(172, 338);
            flpPinned.TabIndex = 1;
            // 
            // lblInsightsTitle
            // 
            lblInsightsTitle.BackColor = Color.White;
            lblInsightsTitle.Dock = DockStyle.Top;
            lblInsightsTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblInsightsTitle.ForeColor = Color.FromArgb(30, 30, 30);
            lblInsightsTitle.Location = new Point(0, 0);
            lblInsightsTitle.Name = "lblInsightsTitle";
            lblInsightsTitle.Padding = new Padding(8, 8, 4, 4);
            lblInsightsTitle.Size = new Size(172, 40);
            lblInsightsTitle.TabIndex = 2;
            lblInsightsTitle.Text = "Announcement\nInsights";
            // 
            // pnlQuickTips
            // 
            pnlQuickTips.BackColor = Color.White;
            pnlQuickTips.BorderStyle = BorderStyle.FixedSingle;
            pnlQuickTips.Controls.Add(btnManageNotif);
            pnlQuickTips.Controls.Add(lblQuickTipsBody);
            pnlQuickTips.Controls.Add(lblQuickTipsTitle);
            pnlQuickTips.Dock = DockStyle.Fill;
            pnlQuickTips.Location = new Point(0, 521);
            pnlQuickTips.Margin = new Padding(0);
            pnlQuickTips.Name = "pnlQuickTips";
            pnlQuickTips.Padding = new Padding(10, 8, 10, 8);
            pnlQuickTips.Size = new Size(260, 94);
            pnlQuickTips.TabIndex = 3;
            // 
            // btnManageNotif
            // 
            btnManageNotif.BackColor = Color.White;
            btnManageNotif.Cursor = Cursors.Hand;
            btnManageNotif.FlatAppearance.BorderColor = Color.FromArgb(190, 190, 190);
            btnManageNotif.FlatStyle = FlatStyle.Flat;
            btnManageNotif.Font = new Font("Segoe UI", 8.5F);
            btnManageNotif.ForeColor = Color.FromArgb(50, 50, 50);
            btnManageNotif.Location = new Point(10, 50);
            btnManageNotif.Name = "btnManageNotif";
            btnManageNotif.Size = new Size(238, 28);
            btnManageNotif.TabIndex = 2;
            btnManageNotif.Text = "Manage Notifications";
            btnManageNotif.UseVisualStyleBackColor = false;
            // 
            // lblQuickTipsBody
            // 
            lblQuickTipsBody.BackColor = Color.Transparent;
            lblQuickTipsBody.Font = new Font("Segoe UI", 8F);
            lblQuickTipsBody.ForeColor = Color.FromArgb(100, 100, 100);
            lblQuickTipsBody.Location = new Point(10, 28);
            lblQuickTipsBody.Name = "lblQuickTipsBody";
            lblQuickTipsBody.Size = new Size(238, 16);
            lblQuickTipsBody.TabIndex = 1;
            lblQuickTipsBody.Text = "Enable email notifications to stay updated.";
            // 
            // lblQuickTipsTitle
            // 
            lblQuickTipsTitle.AutoSize = true;
            lblQuickTipsTitle.BackColor = Color.Transparent;
            lblQuickTipsTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblQuickTipsTitle.ForeColor = Color.FromArgb(30, 30, 30);
            lblQuickTipsTitle.Location = new Point(10, 10);
            lblQuickTipsTitle.Name = "lblQuickTipsTitle";
            lblQuickTipsTitle.Size = new Size(64, 15);
            lblQuickTipsTitle.TabIndex = 0;
            lblQuickTipsTitle.Text = "Quick Tips";
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
            pnlAnnouncement.Size = new Size(744, 635);
            pnlAnnouncement.TabIndex = 2;
            // 
            // flowLayoutPanelAnnouncements
            // 
            flowLayoutPanelAnnouncements.Dock = DockStyle.Fill;
            flowLayoutPanelAnnouncements.Location = new Point(20, 50);
            flowLayoutPanelAnnouncements.Name = "flowLayoutPanelAnnouncements";
            flowLayoutPanelAnnouncements.Size = new Size(704, 565);
            flowLayoutPanelAnnouncements.TabIndex = 1;
            // 
            // lblShowing
            // 
            lblShowing.Dock = DockStyle.Top;
            lblShowing.Font = new Font("Segoe UI", 9F);
            lblShowing.ForeColor = Color.Gray;
            lblShowing.Location = new Point(20, 20);
            lblShowing.Name = "lblShowing";
            lblShowing.Size = new Size(704, 30);
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
            tblSidebar.ResumeLayout(false);
            pnlMiddleRow.ResumeLayout(false);
            pnlCatBox.ResumeLayout(false);
            pnlInsightBox.ResumeLayout(false);
            pnlQuickTips.ResumeLayout(false);
            pnlQuickTips.PerformLayout();
            pnlAnnouncement.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        // ── Top bar ────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.TextBox textBox25;
        private System.Windows.Forms.ComboBox cmbSortBy;
        private System.Windows.Forms.ComboBox cmbFilter;
        private System.Windows.Forms.Button btnCreateAnnouncement;
        private System.Windows.Forms.Button btnInbox;

        // ── Sidebar ────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.TableLayoutPanel tblSidebar;

        private System.Windows.Forms.Label lblPinnedTitle;
        private System.Windows.Forms.FlowLayoutPanel flpPinnedpnlInsights;

        private System.Windows.Forms.Panel pnlMiddleRow;
        private System.Windows.Forms.Panel pnlCatBox;
        private System.Windows.Forms.Label lblCatTitle;
        private System.Windows.Forms.FlowLayoutPanel flpCategories;
        private System.Windows.Forms.Panel pnlInsightBox;
        private System.Windows.Forms.Label lblInsightsTitle;
        private System.Windows.Forms.Panel flpPinned;

        private System.Windows.Forms.Panel pnlQuickTips;
        private System.Windows.Forms.Label lblQuickTipsTitle;
        private System.Windows.Forms.Label lblQuickTipsBody;
        private System.Windows.Forms.Button btnManageNotif;

        // ── Main feed ──────────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlAnnouncement;
        private System.Windows.Forms.Label lblShowing;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelAnnouncements;
    }
}