namespace PUPAcadPortal
{
    partial class ViewAnnouncementStudent
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
            lblOffice = new Label();
            lblDate = new Label();
            pnlBadgeRow = new Panel();
            lblCategoryPill = new Label();
            lblUrgentBadge = new Label();
            lblPinnedBadge = new Label();
            lblTitle = new Label();
            lblDescription = new Label();
            pnlDivider1 = new Panel();
            lblInstructor = new Label();
            pnlHeader.SuspendLayout();
            pnlBody.SuspendLayout();
            pnlBadgeRow.SuspendLayout();
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
            pnlHeader.Size = new Size(705, 55);
            pnlHeader.TabIndex = 0;
            // 
            // picCategoryIcon
            // 
            picCategoryIcon.BackColor = Color.FromArgb(230, 241, 251);
            picCategoryIcon.Location = new Point(14, 9);
            picCategoryIcon.Name = "picCategoryIcon";
            picCategoryIcon.Size = new Size(36, 36);
            picCategoryIcon.TabIndex = 0;
            picCategoryIcon.Paint += PicCategoryIcon_Paint;
            // 
            // lblFormTitle
            // 
            lblFormTitle.AutoSize = true;
            lblFormTitle.Font = new Font("Segoe UI", 12F);
            lblFormTitle.ForeColor = Color.White;
            lblFormTitle.Location = new Point(58, 16);
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
            btnClose.Location = new Point(658, 10);
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
            pnlBody.Controls.Add(lblOffice);
            pnlBody.Controls.Add(lblDate);
            pnlBody.Controls.Add(pnlBadgeRow);
            pnlBody.Controls.Add(lblTitle);
            pnlBody.Controls.Add(lblDescription);
            pnlBody.Controls.Add(pnlDivider1);
            pnlBody.Controls.Add(lblInstructor);
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Location = new Point(0, 0);
            pnlBody.Name = "pnlBody";
            pnlBody.Padding = new Padding(24, 18, 24, 0);
            pnlBody.Size = new Size(705, 420);
            pnlBody.TabIndex = 1;
            // 
            // lblOffice
            // 
            lblOffice.AutoSize = true;
            lblOffice.Font = new Font("Segoe UI", 8.5F);
            lblOffice.ForeColor = Color.FromArgb(90, 90, 90);
            lblOffice.Location = new Point(13, 377);
            lblOffice.Name = "lblOffice";
            lblOffice.Size = new Size(96, 15);
            lblOffice.TabIndex = 1;
            lblOffice.Text = "🏢  Admin Office";
            // 
            // lblDate
            // 
            lblDate.AutoSize = true;
            lblDate.Font = new Font("Segoe UI", 8.5F);
            lblDate.ForeColor = Color.FromArgb(90, 90, 90);
            lblDate.Location = new Point(122, 392);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(155, 15);
            lblDate.TabIndex = 0;
            lblDate.Text = "📅  April 20, 2026  •  8:00 AM";
            // 
            // pnlBadgeRow
            // 
            pnlBadgeRow.BackColor = Color.Transparent;
            pnlBadgeRow.Controls.Add(lblCategoryPill);
            pnlBadgeRow.Controls.Add(lblUrgentBadge);
            pnlBadgeRow.Controls.Add(lblPinnedBadge);
            pnlBadgeRow.Location = new Point(24, 18);
            pnlBadgeRow.Name = "pnlBadgeRow";
            pnlBadgeRow.Size = new Size(648, 26);
            pnlBadgeRow.TabIndex = 0;
            // 
            // lblCategoryPill
            // 
            lblCategoryPill.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblCategoryPill.Location = new Point(0, 3);
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
            lblUrgentBadge.Location = new Point(98, 3);
            lblUrgentBadge.Name = "lblUrgentBadge";
            lblUrgentBadge.Size = new Size(78, 20);
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
            lblPinnedBadge.Location = new Point(184, 3);
            lblPinnedBadge.Name = "lblPinnedBadge";
            lblPinnedBadge.Size = new Size(70, 20);
            lblPinnedBadge.TabIndex = 2;
            lblPinnedBadge.Text = "📌 Pinned";
            lblPinnedBadge.TextAlign = ContentAlignment.MiddleCenter;
            lblPinnedBadge.Visible = false;
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(20, 20, 20);
            lblTitle.Location = new Point(14, 52);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(653, 30);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Announcement Title";
            // 
            // lblDescription
            // 
            lblDescription.Font = new Font("Segoe UI", 9.5F);
            lblDescription.ForeColor = Color.FromArgb(60, 60, 60);
            lblDescription.Location = new Point(19, 90);
            lblDescription.MaximumSize = new Size(720, 0);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(648, 0);
            lblDescription.TabIndex = 2;
            lblDescription.Text = "Description text goes here.";
            // 
            // pnlDivider1
            // 
            pnlDivider1.BackColor = Color.FromArgb(220, 220, 220);
            pnlDivider1.Location = new Point(14, 360);
            pnlDivider1.Name = "pnlDivider1";
            pnlDivider1.Size = new Size(648, 1);
            pnlDivider1.TabIndex = 3;
            // 
            // lblInstructor
            // 
            lblInstructor.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lblInstructor.AutoSize = true;
            lblInstructor.Font = new Font("Segoe UI", 8.5F);
            lblInstructor.ForeColor = Color.FromArgb(90, 90, 90);
            lblInstructor.Location = new Point(13, 392);
            lblInstructor.Name = "lblInstructor";
            lblInstructor.Size = new Size(86, 15);
            lblInstructor.TabIndex = 5;
            lblInstructor.Text = "👤  Prof. Santos";
            lblInstructor.Visible = false;
            // 
            // ViewAnnouncementStudent
            // 
            BackColor = Color.White;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(pnlHeader);
            Controls.Add(pnlBody);
            Name = "ViewAnnouncementStudent";
            Size = new Size(705, 420);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlBody.ResumeLayout(false);
            pnlBody.PerformLayout();
            pnlBadgeRow.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel picCategoryIcon;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Panel pnlBadgeRow;
        private System.Windows.Forms.Label lblCategoryPill;
        private System.Windows.Forms.Label lblUrgentBadge;
        private System.Windows.Forms.Label lblPinnedBadge;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Panel pnlDivider1;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblOffice;
        private System.Windows.Forms.Label lblInstructor;   
    }
}