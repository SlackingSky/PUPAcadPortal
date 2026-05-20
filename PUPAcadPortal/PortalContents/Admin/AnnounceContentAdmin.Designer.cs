namespace PUPAcadPortal.PortalContents.Admin
{
    partial class AnnounceContentAdmin
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
            pnlAnnouncement = new Panel();
            pnlInsights = new Panel();
            lblInsights = new Label();
            panelPinned = new Panel();
            flpPinned = new FlowLayoutPanel();
            lblPinned = new Label();
            cmbSortBy = new ComboBox();
            label128 = new Label();
            cmbFilter = new ComboBox();
            lblFilter = new Label();
            pnlCategories = new Panel();
            flpCategories = new FlowLayoutPanel();
            lblCategories = new Label();
            pnlAnnouncementsList = new Panel();
            fplAnnouncement = new FlowLayoutPanel();
            lblShowing = new Label();
            panel60 = new Panel();
            label145 = new Label();
            roundedPanel37 = new Panel();
            txtAnnSearch = new TextBox();
            pictureBox59 = new PictureBox();
            btnCreateAnnouncement = new Button();
            pnlAnnouncement.SuspendLayout();
            pnlInsights.SuspendLayout();
            panelPinned.SuspendLayout();
            pnlCategories.SuspendLayout();
            pnlAnnouncementsList.SuspendLayout();
            panel60.SuspendLayout();
            roundedPanel37.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox59).BeginInit();
            SuspendLayout();
            // 
            // pnlAnnouncement
            // 
            pnlAnnouncement.Controls.Add(pnlInsights);
            pnlAnnouncement.Controls.Add(panelPinned);
            pnlAnnouncement.Controls.Add(cmbSortBy);
            pnlAnnouncement.Controls.Add(label128);
            pnlAnnouncement.Controls.Add(cmbFilter);
            pnlAnnouncement.Controls.Add(lblFilter);
            pnlAnnouncement.Controls.Add(pnlCategories);
            pnlAnnouncement.Controls.Add(pnlAnnouncementsList);
            pnlAnnouncement.Controls.Add(panel60);
            pnlAnnouncement.Controls.Add(roundedPanel37);
            pnlAnnouncement.Controls.Add(btnCreateAnnouncement);
            pnlAnnouncement.Dock = DockStyle.Fill;
            pnlAnnouncement.Location = new Point(0, 0);
            pnlAnnouncement.Name = "pnlAnnouncement";
            pnlAnnouncement.Size = new Size(1648, 969);
            pnlAnnouncement.TabIndex = 23;
            // 
            // pnlInsights
            // 
            pnlInsights.BackColor = Color.White;
            pnlInsights.BorderStyle = BorderStyle.FixedSingle;
            pnlInsights.Controls.Add(lblInsights);
            pnlInsights.Location = new Point(1441, 650);
            pnlInsights.Name = "pnlInsights";
            pnlInsights.Size = new Size(201, 317);
            pnlInsights.TabIndex = 18;
            // 
            // lblInsights
            // 
            lblInsights.AutoSize = true;
            lblInsights.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblInsights.Location = new Point(3, 2);
            lblInsights.Name = "lblInsights";
            lblInsights.Size = new Size(163, 19);
            lblInsights.TabIndex = 0;
            lblInsights.Text = "Announcement Insights";
            // 
            // panelPinned
            // 
            panelPinned.BackColor = Color.White;
            panelPinned.BorderStyle = BorderStyle.FixedSingle;
            panelPinned.Controls.Add(flpPinned);
            panelPinned.Controls.Add(lblPinned);
            panelPinned.Location = new Point(1203, 45);
            panelPinned.Name = "panelPinned";
            panelPinned.Size = new Size(439, 599);
            panelPinned.TabIndex = 8;
            // 
            // flpPinned
            // 
            flpPinned.AutoScroll = true;
            flpPinned.BackColor = Color.White;
            flpPinned.Location = new Point(5, 27);
            flpPinned.Name = "flpPinned";
            flpPinned.Size = new Size(429, 564);
            flpPinned.TabIndex = 11;
            // 
            // lblPinned
            // 
            lblPinned.AutoSize = true;
            lblPinned.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPinned.Location = new Point(5, 2);
            lblPinned.Name = "lblPinned";
            lblPinned.Size = new Size(165, 19);
            lblPinned.TabIndex = 0;
            lblPinned.Text = "Pinned Announcements";
            // 
            // cmbSortBy
            // 
            cmbSortBy.Cursor = Cursors.IBeam;
            cmbSortBy.Items.AddRange(new object[] { "Newest First" });
            cmbSortBy.Location = new Point(595, 58);
            cmbSortBy.Name = "cmbSortBy";
            cmbSortBy.Size = new Size(150, 23);
            cmbSortBy.TabIndex = 16;
            // 
            // label128
            // 
            label128.BackColor = Color.Transparent;
            label128.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label128.Location = new Point(532, 58);
            label128.Name = "label128";
            label128.Size = new Size(84, 24);
            label128.TabIndex = 17;
            label128.Text = "Sort by:";
            // 
            // cmbFilter
            // 
            cmbFilter.Cursor = Cursors.IBeam;
            cmbFilter.Items.AddRange(new object[] { "All Announcements" });
            cmbFilter.Location = new Point(353, 58);
            cmbFilter.Name = "cmbFilter";
            cmbFilter.Size = new Size(150, 23);
            cmbFilter.TabIndex = 15;
            // 
            // lblFilter
            // 
            lblFilter.BackColor = Color.Transparent;
            lblFilter.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblFilter.Location = new Point(307, 60);
            lblFilter.Name = "lblFilter";
            lblFilter.Size = new Size(84, 24);
            lblFilter.TabIndex = 10;
            lblFilter.Text = "Filter:";
            // 
            // pnlCategories
            // 
            pnlCategories.AutoScroll = true;
            pnlCategories.BackColor = Color.White;
            pnlCategories.BorderStyle = BorderStyle.FixedSingle;
            pnlCategories.Controls.Add(flpCategories);
            pnlCategories.Controls.Add(lblCategories);
            pnlCategories.Location = new Point(1203, 650);
            pnlCategories.Name = "pnlCategories";
            pnlCategories.Size = new Size(233, 317);
            pnlCategories.TabIndex = 13;
            // 
            // flpCategories
            // 
            flpCategories.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            flpCategories.BackColor = Color.White;
            flpCategories.FlowDirection = FlowDirection.TopDown;
            flpCategories.Location = new Point(4, 18);
            flpCategories.Name = "flpCategories";
            flpCategories.Size = new Size(226, 517);
            flpCategories.TabIndex = 0;
            flpCategories.WrapContents = false;
            // 
            // lblCategories
            // 
            lblCategories.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCategories.Location = new Point(6, 0);
            lblCategories.Name = "lblCategories";
            lblCategories.Size = new Size(100, 20);
            lblCategories.TabIndex = 8;
            lblCategories.Text = "Categories";
            // 
            // pnlAnnouncementsList
            // 
            pnlAnnouncementsList.BackColor = Color.White;
            pnlAnnouncementsList.BorderStyle = BorderStyle.FixedSingle;
            pnlAnnouncementsList.Controls.Add(fplAnnouncement);
            pnlAnnouncementsList.Controls.Add(lblShowing);
            pnlAnnouncementsList.Location = new Point(15, 94);
            pnlAnnouncementsList.Name = "pnlAnnouncementsList";
            pnlAnnouncementsList.Size = new Size(1182, 873);
            pnlAnnouncementsList.TabIndex = 14;
            // 
            // fplAnnouncement
            // 
            fplAnnouncement.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            fplAnnouncement.FlowDirection = FlowDirection.TopDown;
            fplAnnouncement.Location = new Point(2, 2);
            fplAnnouncement.Margin = new Padding(0);
            fplAnnouncement.Name = "fplAnnouncement";
            fplAnnouncement.Size = new Size(1177, 850);
            fplAnnouncement.TabIndex = 0;
            fplAnnouncement.WrapContents = false;
            // 
            // lblShowing
            // 
            lblShowing.BackColor = Color.Transparent;
            lblShowing.ForeColor = Color.Gray;
            lblShowing.Location = new Point(0, 850);
            lblShowing.Name = "lblShowing";
            lblShowing.Size = new Size(250, 20);
            lblShowing.TabIndex = 11;
            lblShowing.Text = "Showing 1 to 4 of 12 announcements";
            // 
            // panel60
            // 
            panel60.BackColor = SystemColors.ButtonHighlight;
            panel60.Controls.Add(label145);
            panel60.Dock = DockStyle.Top;
            panel60.Location = new Point(0, 0);
            panel60.Margin = new Padding(3, 2, 3, 2);
            panel60.Name = "panel60";
            panel60.Size = new Size(1648, 41);
            panel60.TabIndex = 9;
            // 
            // label145
            // 
            label145.AutoSize = true;
            label145.Font = new Font("Segoe UI Semibold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label145.Location = new Point(15, 8);
            label145.Name = "label145";
            label145.Size = new Size(152, 25);
            label145.TabIndex = 0;
            label145.Text = "Announcements";
            // 
            // roundedPanel37
            // 
            roundedPanel37.BackColor = Color.White;
            roundedPanel37.BorderStyle = BorderStyle.FixedSingle;
            roundedPanel37.Controls.Add(txtAnnSearch);
            roundedPanel37.Controls.Add(pictureBox59);
            roundedPanel37.Location = new Point(15, 58);
            roundedPanel37.Margin = new Padding(3, 2, 3, 2);
            roundedPanel37.Name = "roundedPanel37";
            roundedPanel37.Padding = new Padding(4);
            roundedPanel37.Size = new Size(285, 31);
            roundedPanel37.TabIndex = 8;
            // 
            // txtAnnSearch
            // 
            txtAnnSearch.BorderStyle = BorderStyle.None;
            txtAnnSearch.Cursor = Cursors.IBeam;
            txtAnnSearch.Location = new Point(38, 8);
            txtAnnSearch.Margin = new Padding(3, 2, 3, 2);
            txtAnnSearch.Name = "txtAnnSearch";
            txtAnnSearch.PlaceholderText = "Search Here";
            txtAnnSearch.Size = new Size(240, 16);
            txtAnnSearch.TabIndex = 1;
            // 
            // pictureBox59
            // 
            pictureBox59.Image = Properties.Resources.magnifier;
            pictureBox59.Location = new Point(8, 6);
            pictureBox59.Margin = new Padding(3, 2, 3, 2);
            pictureBox59.Name = "pictureBox59";
            pictureBox59.Size = new Size(25, 19);
            pictureBox59.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox59.TabIndex = 0;
            pictureBox59.TabStop = false;
            // 
            // btnCreateAnnouncement
            // 
            btnCreateAnnouncement.BackColor = Color.Maroon;
            btnCreateAnnouncement.BackgroundImageLayout = ImageLayout.None;
            btnCreateAnnouncement.Cursor = Cursors.Hand;
            btnCreateAnnouncement.FlatAppearance.BorderSize = 0;
            btnCreateAnnouncement.FlatAppearance.MouseDownBackColor = Color.FromArgb(109, 0, 0);
            btnCreateAnnouncement.FlatAppearance.MouseOverBackColor = Color.FromArgb(109, 0, 0);
            btnCreateAnnouncement.FlatStyle = FlatStyle.Flat;
            btnCreateAnnouncement.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCreateAnnouncement.ForeColor = Color.FromArgb(179, 255, 255, 255);
            btnCreateAnnouncement.ImageAlign = ContentAlignment.TopLeft;
            btnCreateAnnouncement.Location = new Point(912, 57);
            btnCreateAnnouncement.MinimumSize = new Size(227, 30);
            btnCreateAnnouncement.Name = "btnCreateAnnouncement";
            btnCreateAnnouncement.Padding = new Padding(16, 0, 18, 0);
            btnCreateAnnouncement.Size = new Size(285, 31);
            btnCreateAnnouncement.TabIndex = 7;
            btnCreateAnnouncement.Text = "    Create Announcement";
            btnCreateAnnouncement.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCreateAnnouncement.UseVisualStyleBackColor = false;
            // 
            // AdminAnnounceContent
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlAnnouncement);
            Name = "AdminAnnounceContent";
            Size = new Size(1648, 969);
            pnlAnnouncement.ResumeLayout(false);
            pnlInsights.ResumeLayout(false);
            pnlInsights.PerformLayout();
            panelPinned.ResumeLayout(false);
            panelPinned.PerformLayout();
            pnlCategories.ResumeLayout(false);
            pnlAnnouncementsList.ResumeLayout(false);
            panel60.ResumeLayout(false);
            panel60.PerformLayout();
            roundedPanel37.ResumeLayout(false);
            roundedPanel37.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox59).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlAnnouncement;
        private Panel pnlInsights;
        private Label lblInsights;
        private Panel panelPinned;
        private FlowLayoutPanel flpPinned;
        private Label lblPinned;
        private ComboBox cmbSortBy;
        private Label label128;
        private ComboBox cmbFilter;
        private Label lblFilter;
        private Panel pnlCategories;
        private FlowLayoutPanel flpCategories;
        private Label lblCategories;
        private Panel pnlAnnouncementsList;
        private FlowLayoutPanel fplAnnouncement;
        private Label lblShowing;
        private Panel panel60;
        private Label label145;
        private Panel roundedPanel37;
        private TextBox txtAnnSearch;
        private PictureBox pictureBox59;
        private Button btnCreateAnnouncement;
    }
}
