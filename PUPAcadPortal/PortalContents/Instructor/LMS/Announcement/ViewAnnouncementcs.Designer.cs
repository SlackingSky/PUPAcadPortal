namespace PUPAcadPortal
{
    partial class ViewAnnouncement
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
            picCategoryIcon = new Panel();
            lblFormTitle = new Label();
            btnClose = new Button();
            pnlBody = new Panel();
            lblViewed = new Label();
            lblDate = new Label();
            lblAuthor = new Label();
            btnEdit = new Button();
            btnDelete = new Button();
            lblCategoryPill = new Label();
            lblUrgentBadge = new Label();
            lblPinnedBadge = new Label();
            lblStatusBadge = new Label();
            lblTitle = new Label();
            lblDescription = new Label();
            pnlProgressRow = new Panel();
            lblProgressLabel = new Label();
            pnlProgressTrack = new Panel();
            pnlProgressFill = new Panel();
            lblProgressPct = new Label();
            pnlAttachment = new Panel();
            picFileIcon = new Panel();
            lblAttachName = new Label();
            lblAttachType = new Label();
            btnOpenFile = new Button();
            btnEditFile = new Button();
            pnlHeader.SuspendLayout();
            pnlBody.SuspendLayout();
            pnlProgressRow.SuspendLayout();
            pnlProgressTrack.SuspendLayout();
            pnlAttachment.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(139, 0, 0);
            pnlHeader.Controls.Add(picCategoryIcon);
            pnlHeader.Controls.Add(lblFormTitle);
            pnlHeader.Controls.Add(btnClose);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(826, 55);
            pnlHeader.TabIndex = 0;
            // 
            // picCategoryIcon
            // 
            picCategoryIcon.BackColor = Color.FromArgb(230, 241, 251);
            picCategoryIcon.Location = new Point(14, 8);
            picCategoryIcon.Name = "picCategoryIcon";
            picCategoryIcon.Size = new Size(38, 38);
            picCategoryIcon.TabIndex = 0;
            picCategoryIcon.Paint += PicCategoryIcon_Paint;
            // 
            // lblFormTitle
            // 
            lblFormTitle.AutoSize = true;
            lblFormTitle.Font = new Font("Segoe UI", 12F);
            lblFormTitle.ForeColor = Color.White;
            lblFormTitle.Location = new Point(60, 16);
            lblFormTitle.Name = "lblFormTitle";
            lblFormTitle.Size = new Size(167, 21);
            lblFormTitle.TabIndex = 1;
            lblFormTitle.Text = "Announcement Details";
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.Transparent;
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(180, 0, 0);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(778, 10);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(34, 34);
            btnClose.TabIndex = 2;
            btnClose.Text = "×";
            btnClose.UseVisualStyleBackColor = false;
            // 
            // pnlBody
            // 
            pnlBody.BackColor = Color.White;
            pnlBody.BorderStyle = BorderStyle.FixedSingle;
            pnlBody.Controls.Add(lblViewed);
            pnlBody.Controls.Add(lblDate);
            pnlBody.Controls.Add(lblAuthor);
            pnlBody.Controls.Add(btnEdit);
            pnlBody.Controls.Add(btnDelete);
            pnlBody.Controls.Add(lblCategoryPill);
            pnlBody.Controls.Add(lblUrgentBadge);
            pnlBody.Controls.Add(lblPinnedBadge);
            pnlBody.Controls.Add(lblStatusBadge);
            pnlBody.Controls.Add(lblTitle);
            pnlBody.Controls.Add(lblDescription);
            pnlBody.Controls.Add(pnlProgressRow);
            pnlBody.Controls.Add(pnlAttachment);
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Location = new Point(0, 0);
            pnlBody.Name = "pnlBody";
            pnlBody.Padding = new Padding(24, 18, 24, 0);
            pnlBody.Size = new Size(826, 384);
            pnlBody.TabIndex = 1;
            // 
            // lblViewed
            // 
            lblViewed.AutoSize = true;
            lblViewed.Font = new Font("Segoe UI", 8.5F);
            lblViewed.ForeColor = Color.FromArgb(90, 90, 90);
            lblViewed.Location = new Point(647, 224);
            lblViewed.Name = "lblViewed";
            lblViewed.Size = new Size(165, 15);
            lblViewed.TabIndex = 2;
            lblViewed.Text = "👁  Viewed by 0 of 40 students";
            // 
            // lblDate
            // 
            lblDate.AutoSize = true;
            lblDate.Font = new Font("Segoe UI", 8.5F);
            lblDate.ForeColor = Color.FromArgb(90, 90, 90);
            lblDate.Location = new Point(288, 224);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(141, 15);
            lblDate.TabIndex = 1;
            lblDate.Text = "📅  Jan 1, 2026  •  8:00 AM";
            // 
            // lblAuthor
            // 
            lblAuthor.AutoSize = true;
            lblAuthor.Font = new Font("Segoe UI", 8.5F);
            lblAuthor.ForeColor = Color.FromArgb(90, 90, 90);
            lblAuthor.Location = new Point(23, 224);
            lblAuthor.Name = "lblAuthor";
            lblAuthor.Size = new Size(86, 15);
            lblAuthor.TabIndex = 0;
            lblAuthor.Text = "👤  Prof. Santos";
            // 
            // btnEdit
            // 
            btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEdit.BackColor = Color.FromArgb(139, 0, 0);
            btnEdit.Cursor = Cursors.Hand;
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEdit.ForeColor = Color.White;
            btnEdit.Location = new Point(700, 337);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(110, 32);
            btnEdit.TabIndex = 1;
            btnEdit.Text = "✏  Edit";
            btnEdit.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDelete.BackColor = Color.White;
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.FlatAppearance.BorderColor = Color.FromArgb(200, 50, 50);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 9F);
            btnDelete.ForeColor = Color.FromArgb(180, 0, 0);
            btnDelete.Location = new Point(594, 337);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(100, 32);
            btnDelete.TabIndex = 0;
            btnDelete.Text = "🗑  Delete";
            btnDelete.UseVisualStyleBackColor = false;
            // 
            // lblCategoryPill
            // 
            lblCategoryPill.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblCategoryPill.Location = new Point(24, 18);
            lblCategoryPill.Name = "lblCategoryPill";
            lblCategoryPill.Size = new Size(90, 20);
            lblCategoryPill.TabIndex = 0;
            lblCategoryPill.Text = "General";
            lblCategoryPill.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblUrgentBadge
            // 
            lblUrgentBadge.BackColor = Color.Firebrick;
            lblUrgentBadge.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            lblUrgentBadge.ForeColor = Color.White;
            lblUrgentBadge.Location = new Point(122, 18);
            lblUrgentBadge.Name = "lblUrgentBadge";
            lblUrgentBadge.Size = new Size(72, 20);
            lblUrgentBadge.TabIndex = 1;
            lblUrgentBadge.Text = "⚠ URGENT";
            lblUrgentBadge.TextAlign = ContentAlignment.MiddleCenter;
            lblUrgentBadge.Visible = false;
            // 
            // lblPinnedBadge
            // 
            lblPinnedBadge.BackColor = Color.FromArgb(255, 243, 200);
            lblPinnedBadge.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            lblPinnedBadge.ForeColor = Color.FromArgb(150, 100, 0);
            lblPinnedBadge.Location = new Point(202, 18);
            lblPinnedBadge.Name = "lblPinnedBadge";
            lblPinnedBadge.Size = new Size(64, 20);
            lblPinnedBadge.TabIndex = 2;
            lblPinnedBadge.Text = "📌 Pinned";
            lblPinnedBadge.TextAlign = ContentAlignment.MiddleCenter;
            lblPinnedBadge.Visible = false;
            // 
            // lblStatusBadge
            // 
            lblStatusBadge.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            lblStatusBadge.Location = new Point(720, 18);
            lblStatusBadge.Name = "lblStatusBadge";
            lblStatusBadge.Size = new Size(76, 20);
            lblStatusBadge.TabIndex = 3;
            lblStatusBadge.Text = "● Active";
            lblStatusBadge.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(20, 20, 20);
            lblTitle.Location = new Point(23, 57);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(772, 28);
            lblTitle.TabIndex = 4;
            lblTitle.Text = "Announcement Title";
            // 
            // lblDescription
            // 
            lblDescription.Font = new Font("Segoe UI", 9.5F);
            lblDescription.ForeColor = Color.FromArgb(60, 60, 60);
            lblDescription.Location = new Point(23, 88);
            lblDescription.MaximumSize = new Size(740, 0);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(740, 0);
            lblDescription.TabIndex = 5;
            lblDescription.Text = "Description text goes here.";
            // 
            // pnlProgressRow
            // 
            pnlProgressRow.BackColor = Color.Transparent;
            pnlProgressRow.Controls.Add(lblProgressLabel);
            pnlProgressRow.Controls.Add(pnlProgressTrack);
            pnlProgressRow.Controls.Add(lblProgressPct);
            pnlProgressRow.Location = new Point(23, 245);
            pnlProgressRow.Name = "pnlProgressRow";
            pnlProgressRow.Size = new Size(789, 24);
            pnlProgressRow.TabIndex = 8;
            // 
            // lblProgressLabel
            // 
            lblProgressLabel.AutoSize = true;
            lblProgressLabel.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblProgressLabel.ForeColor = Color.FromArgb(80, 80, 80);
            lblProgressLabel.Location = new Point(5, 5);
            lblProgressLabel.Name = "lblProgressLabel";
            lblProgressLabel.Size = new Size(56, 13);
            lblProgressLabel.TabIndex = 0;
            lblProgressLabel.Text = "Read rate";
            // 
            // pnlProgressTrack
            // 
            pnlProgressTrack.BackColor = Color.FromArgb(225, 225, 225);
            pnlProgressTrack.Controls.Add(pnlProgressFill);
            pnlProgressTrack.Location = new Point(70, 9);
            pnlProgressTrack.Name = "pnlProgressTrack";
            pnlProgressTrack.Size = new Size(640, 6);
            pnlProgressTrack.TabIndex = 1;
            // 
            // pnlProgressFill
            // 
            pnlProgressFill.BackColor = Color.FromArgb(139, 0, 0);
            pnlProgressFill.Location = new Point(0, 0);
            pnlProgressFill.Name = "pnlProgressFill";
            pnlProgressFill.Size = new Size(0, 6);
            pnlProgressFill.TabIndex = 0;
            // 
            // lblProgressPct
            // 
            lblProgressPct.AutoSize = true;
            lblProgressPct.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblProgressPct.ForeColor = Color.FromArgb(80, 80, 80);
            lblProgressPct.Location = new Point(718, 5);
            lblProgressPct.Name = "lblProgressPct";
            lblProgressPct.Size = new Size(23, 13);
            lblProgressPct.TabIndex = 2;
            lblProgressPct.Text = "0%";
            // 
            // pnlAttachment
            // 
            pnlAttachment.BackColor = Color.FromArgb(248, 248, 248);
            pnlAttachment.BorderStyle = BorderStyle.FixedSingle;
            pnlAttachment.Controls.Add(picFileIcon);
            pnlAttachment.Controls.Add(lblAttachName);
            pnlAttachment.Controls.Add(lblAttachType);
            pnlAttachment.Controls.Add(btnEditFile);
            pnlAttachment.Controls.Add(btnOpenFile);
            pnlAttachment.Location = new Point(23, 275);
            pnlAttachment.Name = "pnlAttachment";
            pnlAttachment.Size = new Size(789, 56);
            pnlAttachment.TabIndex = 10;
            // 
            // picFileIcon
            // 
            picFileIcon.BackColor = Color.FromArgb(235, 235, 235);
            picFileIcon.Location = new Point(10, 8);
            picFileIcon.Name = "picFileIcon";
            picFileIcon.Size = new Size(38, 38);
            picFileIcon.TabIndex = 0;
            picFileIcon.Paint += PicFileIcon_Paint;
            // 
            // lblAttachName
            // 
            lblAttachName.AutoSize = true;
            lblAttachName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblAttachName.ForeColor = Color.FromArgb(30, 30, 30);
            lblAttachName.Location = new Point(58, 10);
            lblAttachName.Name = "lblAttachName";
            lblAttachName.Size = new Size(78, 15);
            lblAttachName.TabIndex = 1;
            lblAttachName.Text = "filename.pdf";
            // 
            // lblAttachType
            // 
            lblAttachType.AutoSize = true;
            lblAttachType.Font = new Font("Segoe UI", 8F);
            lblAttachType.ForeColor = Color.FromArgb(110, 110, 110);
            lblAttachType.Location = new Point(58, 30);
            lblAttachType.Name = "lblAttachType";
            lblAttachType.Size = new Size(83, 13);
            lblAttachType.TabIndex = 2;
            lblAttachType.Text = "PDF Document";
            // 
            // btnOpenFile
            // 
            btnOpenFile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnOpenFile.BackColor = Color.FromArgb(139, 0, 0);
            btnOpenFile.Cursor = Cursors.Hand;
            btnOpenFile.FlatAppearance.BorderSize = 0;
            btnOpenFile.FlatStyle = FlatStyle.Flat;
            btnOpenFile.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnOpenFile.ForeColor = Color.White;
            btnOpenFile.Location = new Point(569, 12);
            btnOpenFile.Name = "btnOpenFile";
            btnOpenFile.Size = new Size(100, 30);
            btnOpenFile.TabIndex = 3;
            btnOpenFile.Text = "Open File";
            btnOpenFile.UseVisualStyleBackColor = false;
            // 
            // btnEditFile
            // 
            btnEditFile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEditFile.BackColor = Color.White;
            btnEditFile.Cursor = Cursors.Hand;
            btnEditFile.FlatAppearance.BorderColor = Color.FromArgb(139, 0, 0);
            btnEditFile.FlatStyle = FlatStyle.Flat;
            btnEditFile.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnEditFile.ForeColor = Color.FromArgb(139, 0, 0);
            btnEditFile.Location = new Point(675, 12);
            btnEditFile.Name = "btnEditFile";
            btnEditFile.Size = new Size(100, 30);
            btnEditFile.TabIndex = 4;
            btnEditFile.Text = "✏  Edit File";
            btnEditFile.UseVisualStyleBackColor = false;
            // 
            // ViewAnnouncement
            // 
            BackColor = Color.White;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(pnlHeader);
            Controls.Add(pnlBody);
            Name = "ViewAnnouncement";
            Size = new Size(826, 384);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlBody.ResumeLayout(false);
            pnlBody.PerformLayout();
            pnlProgressRow.ResumeLayout(false);
            pnlProgressRow.PerformLayout();
            pnlProgressTrack.ResumeLayout(false);
            pnlAttachment.ResumeLayout(false);
            pnlAttachment.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel picCategoryIcon;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Label lblCategoryPill;
        private System.Windows.Forms.Label lblUrgentBadge;
        private System.Windows.Forms.Label lblPinnedBadge;
        private System.Windows.Forms.Label lblStatusBadge;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblViewed;
        private System.Windows.Forms.Panel pnlProgressRow;
        private System.Windows.Forms.Label lblProgressLabel;
        private System.Windows.Forms.Panel pnlProgressTrack;
        private System.Windows.Forms.Panel pnlProgressFill;
        private System.Windows.Forms.Label lblProgressPct;

        private System.Windows.Forms.Panel pnlAttachment;
        private System.Windows.Forms.Panel picFileIcon;
        private System.Windows.Forms.Label lblAttachName;
        private System.Windows.Forms.Label lblAttachType;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.Button btnEditFile;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
    }
}