namespace PUPAcadPortal.PortalContents.Misc.LMS
{
    partial class CreateAnnouncementAdmin
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this._txtTitle = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this._txtDesc = new System.Windows.Forms.TextBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this._cmbCat = new System.Windows.Forms.ComboBox();
            this._chkUrgent = new System.Windows.Forms.CheckBox();
            this._chkPinned = new System.Windows.Forms.CheckBox();
            this.pnlDivider1 = new System.Windows.Forms.Panel();
            this.lblNotify = new System.Windows.Forms.Label();
            this._chkNotifyStudents = new System.Windows.Forms.CheckBox();
            this._chkNotifyInstructors = new System.Windows.Forms.CheckBox();
            this.pnlDivider2 = new System.Windows.Forms.Panel();
            this.lblAttach = new System.Windows.Forms.Label();
            this._btnBrowse = new System.Windows.Forms.Button();
            this._lblAttachName = new System.Windows.Forms.Label();
            this._btnClearAttach = new System.Windows.Forms.Button();
            this.pnlDivider3 = new System.Windows.Forms.Panel();
            this._btnCancel = new System.Windows.Forms.Button();
            this._btnPost = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();

            // pnlHeader
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(139, 0, 0);
            this.pnlHeader.Controls.Add(this.btnClose);
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(720, 50);

            // lblHeader
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(50, 13);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Text = "Create Announcement";

            // btnClose
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(678, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(34, 34);
            this.btnClose.Text = "×";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.lblTitle.Location = new System.Drawing.Point(20, 68);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Text = "Title *";

            // _txtTitle
            this._txtTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._txtTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this._txtTitle.Location = new System.Drawing.Point(20, 88);
            this._txtTitle.Name = "_txtTitle";
            this._txtTitle.Size = new System.Drawing.Size(670, 26);

            // lblDesc
            this.lblDesc.AutoSize = true;
            this.lblDesc.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblDesc.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.lblDesc.Location = new System.Drawing.Point(20, 126);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Text = "Description *";

            // _txtDesc
            this._txtDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._txtDesc.Font = new System.Drawing.Font("Segoe UI", 10F);
            this._txtDesc.Location = new System.Drawing.Point(20, 146);
            this._txtDesc.Multiline = true;
            this._txtDesc.Name = "_txtDesc";
            this._txtDesc.Size = new System.Drawing.Size(670, 80);

            // lblCategory
            this.lblCategory.AutoSize = true;
            this.lblCategory.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblCategory.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.lblCategory.Location = new System.Drawing.Point(20, 238);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Text = "Category *";

            // _cmbCat
            this._cmbCat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbCat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._cmbCat.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._cmbCat.Location = new System.Drawing.Point(20, 258);
            this._cmbCat.Name = "_cmbCat";
            this._cmbCat.Size = new System.Drawing.Size(300, 26);
            this._cmbCat.SelectedIndexChanged += new System.EventHandler(this.CmbCat_SelectedIndexChanged);

            // _chkUrgent
            this._chkUrgent.AutoSize = true;
            this._chkUrgent.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._chkUrgent.ForeColor = System.Drawing.Color.Firebrick;
            this._chkUrgent.Location = new System.Drawing.Point(20, 296);
            this._chkUrgent.Name = "_chkUrgent";
            this._chkUrgent.Text = "⚠  Mark as Urgent";
            this._chkUrgent.UseVisualStyleBackColor = true;

            // _chkPinned
            this._chkPinned.AutoSize = true;
            this._chkPinned.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._chkPinned.Location = new System.Drawing.Point(180, 296);
            this._chkPinned.Name = "_chkPinned";
            this._chkPinned.Text = "📌  Pin to Top";
            this._chkPinned.UseVisualStyleBackColor = true;

            // pnlDivider1
            this.pnlDivider1.BackColor = System.Drawing.Color.FromArgb(220, 220, 220);
            this.pnlDivider1.Location = new System.Drawing.Point(0, 334);
            this.pnlDivider1.Name = "pnlDivider1";
            this.pnlDivider1.Size = new System.Drawing.Size(720, 1);

            // lblNotify
            this.lblNotify.AutoSize = true;
            this.lblNotify.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNotify.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            this.lblNotify.Location = new System.Drawing.Point(20, 348);
            this.lblNotify.Name = "lblNotify";
            this.lblNotify.Text = "🔔  Send Notification To";

            // _chkNotifyStudents
            this._chkNotifyStudents.AutoSize = true;
            this._chkNotifyStudents.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._chkNotifyStudents.ForeColor = System.Drawing.Color.FromArgb(30, 100, 180);
            this._chkNotifyStudents.Location = new System.Drawing.Point(28, 372);
            this._chkNotifyStudents.Name = "_chkNotifyStudents";
            this._chkNotifyStudents.Text = "👨‍🎓  All Students";
            this._chkNotifyStudents.UseVisualStyleBackColor = true;

            // _chkNotifyInstructors
            this._chkNotifyInstructors.AutoSize = true;
            this._chkNotifyInstructors.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._chkNotifyInstructors.ForeColor = System.Drawing.Color.FromArgb(80, 50, 160);
            this._chkNotifyInstructors.Location = new System.Drawing.Point(200, 372);
            this._chkNotifyInstructors.Name = "_chkNotifyInstructors";
            this._chkNotifyInstructors.Text = "👩‍🏫  All Instructors";
            this._chkNotifyInstructors.UseVisualStyleBackColor = true;

            // pnlDivider2
            this.pnlDivider2.BackColor = System.Drawing.Color.FromArgb(220, 220, 220);
            this.pnlDivider2.Location = new System.Drawing.Point(0, 410);
            this.pnlDivider2.Name = "pnlDivider2";
            this.pnlDivider2.Size = new System.Drawing.Size(720, 1);

            // lblAttach
            this.lblAttach.AutoSize = true;
            this.lblAttach.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAttach.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            this.lblAttach.Location = new System.Drawing.Point(20, 422);
            this.lblAttach.Name = "lblAttach";
            this.lblAttach.Text = "📎  Attachment  (PDF or Image)";

            // _btnBrowse
            this._btnBrowse.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            this._btnBrowse.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnBrowse.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            this._btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnBrowse.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._btnBrowse.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this._btnBrowse.Location = new System.Drawing.Point(28, 446);
            this._btnBrowse.Name = "_btnBrowse";
            this._btnBrowse.Size = new System.Drawing.Size(130, 30);
            this._btnBrowse.Text = "  Choose File…";
            this._btnBrowse.UseVisualStyleBackColor = false;
            this._btnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);

            // _lblAttachName
            this._lblAttachName.AutoEllipsis = true;
            this._lblAttachName.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._lblAttachName.ForeColor = System.Drawing.Color.FromArgb(130, 130, 130);
            this._lblAttachName.Location = new System.Drawing.Point(166, 452);
            this._lblAttachName.Name = "_lblAttachName";
            this._lblAttachName.Size = new System.Drawing.Size(420, 20);
            this._lblAttachName.Text = "No file chosen";

            // _btnClearAttach
            this._btnClearAttach.BackColor = System.Drawing.Color.Transparent;
            this._btnClearAttach.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnClearAttach.FlatAppearance.BorderSize = 0;
            this._btnClearAttach.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnClearAttach.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this._btnClearAttach.ForeColor = System.Drawing.Color.FromArgb(160, 0, 0);
            this._btnClearAttach.Location = new System.Drawing.Point(594, 448);
            this._btnClearAttach.Name = "_btnClearAttach";
            this._btnClearAttach.Size = new System.Drawing.Size(28, 26);
            this._btnClearAttach.Text = "✕";
            this._btnClearAttach.UseVisualStyleBackColor = false;
            this._btnClearAttach.Visible = false;
            this._btnClearAttach.Click += new System.EventHandler(this.BtnClearAttach_Click);

            // pnlDivider3
            this.pnlDivider3.BackColor = System.Drawing.Color.FromArgb(220, 220, 220);
            this.pnlDivider3.Location = new System.Drawing.Point(0, 490);
            this.pnlDivider3.Name = "pnlDivider3";
            this.pnlDivider3.Size = new System.Drawing.Size(720, 1);

            // _btnCancel
            this._btnCancel.BackColor = System.Drawing.Color.White;
            this._btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            this._btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnCancel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._btnCancel.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            this._btnCancel.Location = new System.Drawing.Point(472, 502);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(110, 36);
            this._btnCancel.Text = "✕  Cancel";
            this._btnCancel.UseVisualStyleBackColor = false;
            this._btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);

            // _btnPost
            this._btnPost.BackColor = System.Drawing.Color.FromArgb(139, 0, 0);
            this._btnPost.FlatAppearance.BorderSize = 0;
            this._btnPost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnPost.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._btnPost.ForeColor = System.Drawing.Color.White;
            this._btnPost.Location = new System.Drawing.Point(590, 502);
            this._btnPost.Name = "_btnPost";
            this._btnPost.Size = new System.Drawing.Size(118, 36);
            this._btnPost.Text = "📢  Post";
            this._btnPost.UseVisualStyleBackColor = false;
            this._btnPost.Click += new System.EventHandler(this.BtnPost_Click);

            // CreateAnnouncementAdmin
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this._btnPost);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this.pnlDivider3);
            this.Controls.Add(this._btnClearAttach);
            this.Controls.Add(this._lblAttachName);
            this.Controls.Add(this._btnBrowse);
            this.Controls.Add(this.lblAttach);
            this.Controls.Add(this.pnlDivider2);
            this.Controls.Add(this._chkNotifyInstructors);
            this.Controls.Add(this._chkNotifyStudents);
            this.Controls.Add(this.lblNotify);
            this.Controls.Add(this.pnlDivider1);
            this.Controls.Add(this._chkPinned);
            this.Controls.Add(this._chkUrgent);
            this.Controls.Add(this._cmbCat);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this._txtDesc);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this._txtTitle);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlHeader);
            this.Name = "CreateAnnouncementAdmin";
            this.Size = new System.Drawing.Size(720, 560);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox _txtTitle;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.TextBox _txtDesc;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox _cmbCat;
        private System.Windows.Forms.CheckBox _chkUrgent;
        private System.Windows.Forms.CheckBox _chkPinned;
        private System.Windows.Forms.Panel pnlDivider1;
        private System.Windows.Forms.Label lblNotify;
        private System.Windows.Forms.CheckBox _chkNotifyStudents;
        private System.Windows.Forms.CheckBox _chkNotifyInstructors;
        private System.Windows.Forms.Panel pnlDivider2;
        private System.Windows.Forms.Label lblAttach;
        private System.Windows.Forms.Button _btnBrowse;
        private System.Windows.Forms.Label _lblAttachName;
        private System.Windows.Forms.Button _btnClearAttach;
        private System.Windows.Forms.Panel pnlDivider3;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.Button _btnPost;
    }
}