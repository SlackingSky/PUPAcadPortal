namespace PUPAcadPortal.PortalContents.Student.LMS
{
    partial class AnnouncementCardUC
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            pnlAccentBar = new System.Windows.Forms.Panel();
            pnlIconBlock = new System.Windows.Forms.Panel();
            lblIconLetter = new System.Windows.Forms.Label();
            pnlUnreadDot = new System.Windows.Forms.Panel();
            lblCategoryTag = new System.Windows.Forms.Label();
            lblAttachBadge = new System.Windows.Forms.Label();
            lblTitle = new System.Windows.Forms.Label();
            lblDescription = new System.Windows.Forms.Label();
            lblCourseChip = new System.Windows.Forms.Label();
            lblDate = new System.Windows.Forms.Label();
            lblOffice = new System.Windows.Forms.Label();
            lblInstructor = new System.Windows.Forms.Label();
            btnPin = new System.Windows.Forms.Button();

            pnlIconBlock.SuspendLayout();
            SuspendLayout();

            // pnlAccentBar — left edge colour strip
            pnlAccentBar.Name = "pnlAccentBar";
            pnlAccentBar.TabIndex = 0;

            // pnlIconBlock — coloured square with initial letter
            pnlIconBlock.Controls.Add(lblIconLetter);
            pnlIconBlock.Name = "pnlIconBlock";
            pnlIconBlock.TabIndex = 1;

            // lblIconLetter — letter inside the icon block
            lblIconLetter.Name = "lblIconLetter";
            lblIconLetter.TabIndex = 0;

            // pnlUnreadDot — small red circle top-right
            pnlUnreadDot.Name = "pnlUnreadDot";
            pnlUnreadDot.TabIndex = 2;

            // lblCategoryTag — e.g. "ACADEMIC", "URGENT"
            lblCategoryTag.Name = "lblCategoryTag";
            lblCategoryTag.TabIndex = 3;

            // lblAttachBadge — "📎 2 files"
            lblAttachBadge.Name = "lblAttachBadge";
            lblAttachBadge.TabIndex = 4;

            // lblTitle
            lblTitle.Name = "lblTitle";
            lblTitle.TabIndex = 5;

            // lblDescription
            lblDescription.Name = "lblDescription";
            lblDescription.TabIndex = 6;

            // lblCourseChip
            lblCourseChip.Name = "lblCourseChip";
            lblCourseChip.TabIndex = 7;

            // lblDate
            lblDate.Name = "lblDate";
            lblDate.TabIndex = 8;

            // lblOffice
            lblOffice.Name = "lblOffice";
            lblOffice.TabIndex = 9;

            // lblInstructor
            lblInstructor.Name = "lblInstructor";
            lblInstructor.TabIndex = 10;

            // btnPin
            btnPin.Name = "btnPin";
            btnPin.TabIndex = 11;
            btnPin.TabStop = false;

            // AnnouncementCardUC
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Size = new System.Drawing.Size(1174, 115);
            this.Name = "AnnouncementCardUC";
            this.Controls.Add(pnlAccentBar);
            this.Controls.Add(pnlIconBlock);
            this.Controls.Add(pnlUnreadDot);
            this.Controls.Add(lblCategoryTag);
            this.Controls.Add(lblAttachBadge);
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblDescription);
            this.Controls.Add(lblCourseChip);
            this.Controls.Add(lblDate);
            this.Controls.Add(lblOffice);
            this.Controls.Add(lblInstructor);
            this.Controls.Add(btnPin);

            pnlIconBlock.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlAccentBar;
        private System.Windows.Forms.Panel pnlIconBlock;
        private System.Windows.Forms.Label lblIconLetter;
        private System.Windows.Forms.Panel pnlUnreadDot;
        private System.Windows.Forms.Label lblCategoryTag;
        private System.Windows.Forms.Label lblAttachBadge;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblCourseChip;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblOffice;
        private System.Windows.Forms.Label lblInstructor;
        private System.Windows.Forms.Button btnPin;
    }
}