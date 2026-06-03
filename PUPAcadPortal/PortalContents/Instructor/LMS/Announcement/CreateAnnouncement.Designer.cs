namespace PUPAcadPortal
{
    partial class CreateAnnouncement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateAnnouncement));
            pnlHeader = new Panel();
            picMegaphone = new PictureBox();
            lblFormTitle = new Label();
            lblTitle = new Label();
            txtTitle = new TextBox();
            lblDescription = new Label();
            txtDescription = new TextBox();
            lblCharCount = new Label();
            lblAttachment = new Label();
            pnlAttachment = new Panel();
            picAttachIcon = new PictureBox();
            lblAttachHint = new Label();
            lblAttachHint2 = new Label();
            btnBrowse = new Button();
            lblCourseSelection = new Label();
            clbCourses = new CheckedListBox();
            lnkSelectAll = new LinkLabel();
            lnkClearAll = new LinkLabel();
            lblCategory = new Label();
            cmbCategory = new ComboBox();
            lblPostDate = new Label();
            dtpPostDate = new DateTimePicker();
            lblPostDateHint = new Label();
            pnlFlags = new Panel();
            chkUrgent = new CheckBox();
            chkPinned = new CheckBox();
            pnlDivider = new Panel();
            btnCancel = new Button();
            btnPost = new Button();
            btnRemoveAttach = new Button();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picMegaphone).BeginInit();
            pnlAttachment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picAttachIcon).BeginInit();
            pnlFlags.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(139, 0, 0);
            pnlHeader.Controls.Add(picMegaphone);
            pnlHeader.Controls.Add(lblFormTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(958, 52);
            pnlHeader.TabIndex = 0;
            // 
            // picMegaphone
            // 
            picMegaphone.BackColor = Color.Transparent;
            picMegaphone.Image = (Image)resources.GetObject("picMegaphone.Image");
            picMegaphone.Location = new Point(14, 9);
            picMegaphone.Name = "picMegaphone";
            picMegaphone.Size = new Size(32, 32);
            picMegaphone.SizeMode = PictureBoxSizeMode.StretchImage;
            picMegaphone.TabIndex = 0;
            picMegaphone.TabStop = false;
            // 
            // lblFormTitle
            // 
            lblFormTitle.AutoSize = true;
            lblFormTitle.Font = new Font("Segoe UI", 13F);
            lblFormTitle.ForeColor = Color.White;
            lblFormTitle.Location = new Point(54, 14);
            lblFormTitle.Name = "lblFormTitle";
            lblFormTitle.Size = new Size(188, 25);
            lblFormTitle.TabIndex = 1;
            lblFormTitle.Text = "Create Announcement";
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(50, 50, 50);
            lblTitle.Location = new Point(20, 68);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(100, 18);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Title";
            // 
            // txtTitle
            // 
            txtTitle.BorderStyle = BorderStyle.FixedSingle;
            txtTitle.Cursor = Cursors.IBeam;
            txtTitle.Font = new Font("Segoe UI", 10F);
            txtTitle.ForeColor = Color.FromArgb(160, 160, 160);
            txtTitle.Location = new Point(20, 90);
            txtTitle.MaxLength = 120;
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(560, 25);
            txtTitle.TabIndex = 2;
            txtTitle.Text = "Insert announcement title here...";
            // 
            // lblDescription
            // 
            lblDescription.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblDescription.ForeColor = Color.FromArgb(50, 50, 50);
            lblDescription.Location = new Point(20, 132);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(120, 18);
            lblDescription.TabIndex = 3;
            lblDescription.Text = "Description";
            // 
            // txtDescription
            // 
            txtDescription.BorderStyle = BorderStyle.FixedSingle;
            txtDescription.Cursor = Cursors.IBeam;
            txtDescription.Font = new Font("Segoe UI", 10F);
            txtDescription.ForeColor = Color.FromArgb(160, 160, 160);
            txtDescription.Location = new Point(20, 154);
            txtDescription.MaxLength = 500;
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.ScrollBars = ScrollBars.Vertical;
            txtDescription.Size = new Size(560, 130);
            txtDescription.TabIndex = 4;
            txtDescription.Text = "Write your announcement details here...";
            // 
            // lblCharCount
            // 
            lblCharCount.Font = new Font("Segoe UI", 8F);
            lblCharCount.ForeColor = Color.Gray;
            lblCharCount.Location = new Point(20, 288);
            lblCharCount.Name = "lblCharCount";
            lblCharCount.Size = new Size(200, 16);
            lblCharCount.TabIndex = 5;
            lblCharCount.Text = "500 characters remaining";
            // 
            // lblAttachment
            // 
            lblAttachment.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblAttachment.ForeColor = Color.FromArgb(50, 50, 50);
            lblAttachment.Location = new Point(20, 316);
            lblAttachment.Name = "lblAttachment";
            lblAttachment.Size = new Size(160, 18);
            lblAttachment.TabIndex = 6;
            lblAttachment.Text = "Attachment (Optional)";
            // 
            // pnlAttachment
            // 
            pnlAttachment.BackColor = Color.White;
            pnlAttachment.BorderStyle = BorderStyle.FixedSingle;
            pnlAttachment.Controls.Add(picAttachIcon);
            pnlAttachment.Controls.Add(lblAttachHint);
            pnlAttachment.Controls.Add(lblAttachHint2);
            pnlAttachment.Controls.Add(btnBrowse);
            pnlAttachment.Cursor = Cursors.Hand;
            pnlAttachment.Location = new Point(20, 338);
            pnlAttachment.Name = "pnlAttachment";
            pnlAttachment.Size = new Size(560, 72);
            pnlAttachment.TabIndex = 7;
            // 
            // picAttachIcon
            // 
            picAttachIcon.BackColor = Color.Transparent;
            picAttachIcon.Image = (Image)resources.GetObject("picAttachIcon.Image");
            picAttachIcon.Location = new Point(16, 14);
            picAttachIcon.Name = "picAttachIcon";
            picAttachIcon.Size = new Size(40, 36);
            picAttachIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            picAttachIcon.TabIndex = 0;
            picAttachIcon.TabStop = false;
            // 
            // lblAttachHint
            // 
            lblAttachHint.Font = new Font("Segoe UI", 9F);
            lblAttachHint.ForeColor = Color.DimGray;
            lblAttachHint.Location = new Point(62, 14);
            lblAttachHint.Name = "lblAttachHint";
            lblAttachHint.Size = new Size(320, 18);
            lblAttachHint.TabIndex = 1;
            lblAttachHint.Text = "Drag and drop a file here, or click Browse";
            // 
            // lblAttachHint2
            // 
            lblAttachHint2.Font = new Font("Segoe UI", 8F);
            lblAttachHint2.ForeColor = Color.Gray;
            lblAttachHint2.Location = new Point(62, 36);
            lblAttachHint2.Name = "lblAttachHint2";
            lblAttachHint2.Size = new Size(320, 16);
            lblAttachHint2.TabIndex = 2;
            lblAttachHint2.Text = "Supported: PDF, DOCX, PPTX, PNG, JPG  ·  Max 10 MB";
            // 
            // btnBrowse
            // 
            btnBrowse.BackColor = Color.FromArgb(139, 0, 0);
            btnBrowse.Cursor = Cursors.Hand;
            btnBrowse.FlatAppearance.BorderSize = 0;
            btnBrowse.FlatStyle = FlatStyle.Flat;
            btnBrowse.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnBrowse.ForeColor = Color.White;
            btnBrowse.Location = new Point(465, 20);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(90, 32);
            btnBrowse.TabIndex = 3;
            btnBrowse.Text = "Browse Files";
            btnBrowse.UseVisualStyleBackColor = false;
            // 
            // lblCourseSelection
            // 
            lblCourseSelection.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblCourseSelection.ForeColor = Color.FromArgb(50, 50, 50);
            lblCourseSelection.Location = new Point(608, 132);
            lblCourseSelection.Name = "lblCourseSelection";
            lblCourseSelection.Size = new Size(130, 18);
            lblCourseSelection.TabIndex = 10;
            lblCourseSelection.Text = "Course Selection *";
            // 
            // clbCourses
            // 
            clbCourses.BorderStyle = BorderStyle.FixedSingle;
            clbCourses.CheckOnClick = true;
            clbCourses.Font = new Font("Segoe UI", 9.5F);
            clbCourses.FormattingEnabled = true;
            clbCourses.Items.AddRange(new object[] { "Introduction to Programming 1", "Principles of Accounting", "PATHFIT 4", "Information Management", "Programming and Technologies 1", "Human Computer Interaction" });
            clbCourses.Location = new Point(608, 154);
            clbCourses.Name = "clbCourses";
            clbCourses.Size = new Size(332, 116);
            clbCourses.TabIndex = 13;
            // 
            // lnkSelectAll
            // 
            lnkSelectAll.ActiveLinkColor = Color.FromArgb(139, 0, 0);
            lnkSelectAll.Font = new Font("Segoe UI", 8F);
            lnkSelectAll.LinkColor = Color.FromArgb(139, 0, 0);
            lnkSelectAll.Location = new Point(760, 134);
            lnkSelectAll.Name = "lnkSelectAll";
            lnkSelectAll.Size = new Size(60, 16);
            lnkSelectAll.TabIndex = 11;
            lnkSelectAll.TabStop = true;
            lnkSelectAll.Text = "Select All";
            // 
            // lnkClearAll
            // 
            lnkClearAll.ActiveLinkColor = Color.Gray;
            lnkClearAll.Font = new Font("Segoe UI", 8F);
            lnkClearAll.LinkColor = Color.Gray;
            lnkClearAll.Location = new Point(826, 134);
            lnkClearAll.Name = "lnkClearAll";
            lnkClearAll.Size = new Size(52, 16);
            lnkClearAll.TabIndex = 12;
            lnkClearAll.TabStop = true;
            lnkClearAll.Text = "Clear All";
            // 
            // lblCategory
            // 
            lblCategory.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblCategory.ForeColor = Color.FromArgb(50, 50, 50);
            lblCategory.Location = new Point(608, 68);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(80, 18);
            lblCategory.TabIndex = 8;
            lblCategory.Text = "Category";
            // 
            // cmbCategory
            // 
            cmbCategory.AccessibleDescription = "";
            cmbCategory.AccessibleName = "";
            cmbCategory.BackColor = Color.White;
            cmbCategory.Cursor = Cursors.Hand;
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.FlatStyle = FlatStyle.Flat;
            cmbCategory.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cmbCategory.FormattingEnabled = true;
            cmbCategory.Items.AddRange(new object[] { "Academic", "Events", "Examinations", "General", "Schedule" });
            cmbCategory.Location = new Point(608, 90);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(332, 25);
            cmbCategory.Sorted = true;
            cmbCategory.TabIndex = 9;
            cmbCategory.Tag = "";
            // 
            // lblPostDate
            // 
            lblPostDate.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblPostDate.ForeColor = Color.FromArgb(50, 50, 50);
            lblPostDate.Location = new Point(608, 300);
            lblPostDate.Name = "lblPostDate";
            lblPostDate.Size = new Size(80, 18);
            lblPostDate.TabIndex = 14;
            lblPostDate.Text = "Post Date";
            // 
            // dtpPostDate
            // 
            dtpPostDate.CalendarFont = new Font("Segoe UI", 9F);
            dtpPostDate.CustomFormat = "dd MMM yyyy";
            dtpPostDate.Font = new Font("Segoe UI", 10F);
            dtpPostDate.Format = DateTimePickerFormat.Custom;
            dtpPostDate.Location = new Point(608, 322);
            dtpPostDate.MinDate = new DateTime(2026, 5, 9, 0, 0, 0, 0);
            dtpPostDate.Name = "dtpPostDate";
            dtpPostDate.Size = new Size(332, 25);
            dtpPostDate.TabIndex = 15;
            dtpPostDate.Value = new DateTime(2026, 5, 9, 0, 0, 0, 0);
            // 
            // lblPostDateHint
            // 
            lblPostDateHint.Font = new Font("Segoe UI", 8F);
            lblPostDateHint.ForeColor = Color.Gray;
            lblPostDateHint.Location = new Point(608, 354);
            lblPostDateHint.Name = "lblPostDateHint";
            lblPostDateHint.Size = new Size(332, 16);
            lblPostDateHint.TabIndex = 16;
            lblPostDateHint.Text = "Select the date this announcement will go live.";
            // 
            // pnlFlags
            // 
            pnlFlags.BackColor = Color.FromArgb(250, 245, 245);
            pnlFlags.BorderStyle = BorderStyle.FixedSingle;
            pnlFlags.Controls.Add(chkUrgent);
            pnlFlags.Controls.Add(chkPinned);
            pnlFlags.Location = new Point(608, 378);
            pnlFlags.Name = "pnlFlags";
            pnlFlags.Size = new Size(332, 42);
            pnlFlags.TabIndex = 17;
            // 
            // chkUrgent
            // 
            chkUrgent.AutoSize = true;
            chkUrgent.Font = new Font("Segoe UI", 9.5F);
            chkUrgent.ForeColor = Color.FromArgb(180, 0, 0);
            chkUrgent.Location = new Point(12, 10);
            chkUrgent.Name = "chkUrgent";
            chkUrgent.Size = new Size(118, 21);
            chkUrgent.TabIndex = 0;
            chkUrgent.Text = "Mark as Urgent";
            // 
            // chkPinned
            // 
            chkPinned.AutoSize = true;
            chkPinned.Font = new Font("Segoe UI", 9.5F);
            chkPinned.ForeColor = Color.FromArgb(50, 50, 50);
            chkPinned.Location = new Point(175, 10);
            chkPinned.Name = "chkPinned";
            chkPinned.Size = new Size(112, 21);
            chkPinned.TabIndex = 1;
            chkPinned.Text = "📌  Pin to Top";
            // 
            // pnlDivider
            // 
            pnlDivider.BackColor = Color.FromArgb(220, 220, 220);
            pnlDivider.Location = new Point(0, 426);
            pnlDivider.Name = "pnlDivider";
            pnlDivider.Size = new Size(960, 1);
            pnlDivider.TabIndex = 18;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.White;
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.FlatAppearance.BorderColor = Color.Silver;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 9.5F);
            btnCancel.ForeColor = Color.FromArgb(80, 80, 80);
            btnCancel.Location = new Point(692, 438);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(110, 36);
            btnCancel.TabIndex = 19;
            btnCancel.Text = "✕  Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnPost
            // 
            btnPost.BackColor = Color.FromArgb(139, 0, 0);
            btnPost.Cursor = Cursors.Hand;
            btnPost.FlatAppearance.BorderSize = 0;
            btnPost.FlatStyle = FlatStyle.Flat;
            btnPost.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnPost.ForeColor = Color.White;
            btnPost.Location = new Point(812, 438);
            btnPost.Name = "btnPost";
            btnPost.Size = new Size(138, 36);
            btnPost.TabIndex = 20;
            btnPost.Text = "📢  Post Announcement";
            btnPost.UseVisualStyleBackColor = false;
            // 
            // btnRemoveAttach
            // 
            btnRemoveAttach.BackColor = Color.White;
            btnRemoveAttach.Cursor = Cursors.Hand;
            btnRemoveAttach.FlatAppearance.BorderColor = Color.Silver;
            btnRemoveAttach.FlatStyle = FlatStyle.Flat;
            btnRemoveAttach.Font = new Font("Segoe UI", 8F);
            btnRemoveAttach.ForeColor = Color.Firebrick;
            btnRemoveAttach.Location = new Point(630, 441);
            btnRemoveAttach.Name = "btnRemoveAttach";
            btnRemoveAttach.Size = new Size(56, 32);
            btnRemoveAttach.TabIndex = 4;
            btnRemoveAttach.Text = "✕ Remove";
            btnRemoveAttach.UseVisualStyleBackColor = false;
            btnRemoveAttach.Visible = false;
            btnRemoveAttach.Click += BtnRemoveAttach_Click;
            // 
            // CreateAnnouncement
            // 
            BackColor = Color.White;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(pnlHeader);
            Controls.Add(lblTitle);
            Controls.Add(txtTitle);
            Controls.Add(lblDescription);
            Controls.Add(btnRemoveAttach);
            Controls.Add(txtDescription);
            Controls.Add(lblCharCount);
            Controls.Add(lblAttachment);
            Controls.Add(pnlAttachment);
            Controls.Add(lblCategory);
            Controls.Add(cmbCategory);
            Controls.Add(lblCourseSelection);
            Controls.Add(lnkSelectAll);
            Controls.Add(lnkClearAll);
            Controls.Add(clbCourses);
            Controls.Add(lblPostDate);
            Controls.Add(dtpPostDate);
            Controls.Add(lblPostDateHint);
            Controls.Add(pnlFlags);
            Controls.Add(pnlDivider);
            Controls.Add(btnCancel);
            Controls.Add(btnPost);
            Name = "CreateAnnouncement";
            Size = new Size(958, 484);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picMegaphone).EndInit();
            pnlAttachment.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picAttachIcon).EndInit();
            pnlFlags.ResumeLayout(false);
            pnlFlags.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        // ── Header ────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.PictureBox picMegaphone;
        private System.Windows.Forms.Label lblFormTitle;

        // ── Left column ───────────────────────────────────────────────────
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblCharCount;
        private System.Windows.Forms.Label lblAttachment;
        private System.Windows.Forms.Panel pnlAttachment;
        private System.Windows.Forms.PictureBox picAttachIcon;
        private System.Windows.Forms.Label lblAttachHint;
        private System.Windows.Forms.Label lblAttachHint2;
        private System.Windows.Forms.Button btnBrowse;

        // ── Right column ──────────────────────────────────────────────────
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label lblCourseSelection;
        private System.Windows.Forms.LinkLabel lnkSelectAll;
        private System.Windows.Forms.LinkLabel lnkClearAll;
        private System.Windows.Forms.CheckedListBox clbCourses;
        private System.Windows.Forms.Label lblPostDate;
        private System.Windows.Forms.DateTimePicker dtpPostDate;
        private System.Windows.Forms.Label lblPostDateHint;
        private System.Windows.Forms.Panel pnlFlags;
        private System.Windows.Forms.CheckBox chkUrgent;
        private System.Windows.Forms.CheckBox chkPinned;

        // ── Footer ────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlDivider;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnPost;
        private Button btnRemoveAttach;
    }
}