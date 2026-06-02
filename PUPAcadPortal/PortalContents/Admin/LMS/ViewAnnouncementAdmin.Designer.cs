namespace PUPAcadPortal.PortalContents.Misc.LMS
{
    partial class ViewAnnouncementAdmin
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
            this._pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this._lblCat = new System.Windows.Forms.Label();
            this._lblStatus = new System.Windows.Forms.Label();
            this._lblTitle = new System.Windows.Forms.Label();
            this._lblDesc = new System.Windows.Forms.Label();
            this._lblAuthor = new System.Windows.Forms.Label();
            this._lblDate = new System.Windows.Forms.Label();
            this._lblNotify = new System.Windows.Forms.Label();
            this._lblAttachment = new System.Windows.Forms.Label();
            this.pnlDivider = new System.Windows.Forms.Panel();
            this._btnDelete = new System.Windows.Forms.Button();
            this._btnEdit = new System.Windows.Forms.Button();

            this._pnlHeader.SuspendLayout();
            this.SuspendLayout();

            // _pnlHeader
            this._pnlHeader.BackColor = System.Drawing.Color.FromArgb(139, 0, 0);
            this._pnlHeader.Controls.Add(this.btnClose);
            this._pnlHeader.Controls.Add(this.lblHeaderTitle);
            this._pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnlHeader.Location = new System.Drawing.Point(0, 0);
            this._pnlHeader.Name = "_pnlHeader";
            this._pnlHeader.Size = new System.Drawing.Size(760, 55);

            // lblHeaderTitle
            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.White;
            this.lblHeaderTitle.Location = new System.Drawing.Point(60, 16);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Text = "Announcement Details";

            // btnClose
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(718, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(34, 34);
            this.btnClose.Text = "×";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);

            // _lblCat
            this._lblCat.BackColor = System.Drawing.Color.Transparent;
            this._lblCat.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this._lblCat.Location = new System.Drawing.Point(24, 68);
            this._lblCat.Name = "_lblCat";
            this._lblCat.Size = new System.Drawing.Size(110, 20);
            this._lblCat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // _lblStatus
            this._lblStatus.BackColor = System.Drawing.Color.Transparent;
            this._lblStatus.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this._lblStatus.Location = new System.Drawing.Point(142, 68);
            this._lblStatus.Name = "_lblStatus";
            this._lblStatus.Size = new System.Drawing.Size(76, 20);
            this._lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // _lblTitle
            this._lblTitle.AutoSize = true;
            this._lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this._lblTitle.Location = new System.Drawing.Point(24, 98);
            this._lblTitle.MaximumSize = new System.Drawing.Size(710, 0);
            this._lblTitle.Name = "_lblTitle";

            // _lblDesc
            this._lblDesc.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._lblDesc.Location = new System.Drawing.Point(24, 132);
            this._lblDesc.Name = "_lblDesc";
            this._lblDesc.Size = new System.Drawing.Size(710, 60);

            // _lblAuthor
            this._lblAuthor.AutoSize = true;
            this._lblAuthor.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._lblAuthor.ForeColor = System.Drawing.Color.FromArgb(90, 90, 90);
            this._lblAuthor.Location = new System.Drawing.Point(24, 202);
            this._lblAuthor.Name = "_lblAuthor";

            // _lblDate
            this._lblDate.AutoSize = true;
            this._lblDate.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._lblDate.ForeColor = System.Drawing.Color.FromArgb(90, 90, 90);
            this._lblDate.Location = new System.Drawing.Point(220, 202);
            this._lblDate.Name = "_lblDate";

            // _lblNotify
            this._lblNotify.AutoSize = true;
            this._lblNotify.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._lblNotify.ForeColor = System.Drawing.Color.FromArgb(30, 100, 180);
            this._lblNotify.Location = new System.Drawing.Point(24, 232);
            this._lblNotify.Name = "_lblNotify";

            // _lblAttachment
            this._lblAttachment.AutoSize = true;
            this._lblAttachment.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._lblAttachment.ForeColor = System.Drawing.Color.FromArgb(100, 60, 160);
            this._lblAttachment.Location = new System.Drawing.Point(24, 254);
            this._lblAttachment.Name = "_lblAttachment";

            // pnlDivider
            this.pnlDivider.BackColor = System.Drawing.Color.FromArgb(220, 220, 220);
            this.pnlDivider.Location = new System.Drawing.Point(0, 278);
            this.pnlDivider.Name = "pnlDivider";
            this.pnlDivider.Size = new System.Drawing.Size(760, 1);

            // _btnDelete
            this._btnDelete.BackColor = System.Drawing.Color.White;
            this._btnDelete.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 50, 50);
            this._btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnDelete.ForeColor = System.Drawing.Color.FromArgb(180, 0, 0);
            this._btnDelete.Location = new System.Drawing.Point(530, 290);
            this._btnDelete.Name = "_btnDelete";
            this._btnDelete.Size = new System.Drawing.Size(100, 32);
            this._btnDelete.Text = "🗑  Delete";
            this._btnDelete.UseVisualStyleBackColor = false;
            this._btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);

            // _btnEdit
            this._btnEdit.BackColor = System.Drawing.Color.FromArgb(139, 0, 0);
            this._btnEdit.FlatAppearance.BorderSize = 0;
            this._btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnEdit.ForeColor = System.Drawing.Color.White;
            this._btnEdit.Location = new System.Drawing.Point(638, 290);
            this._btnEdit.Name = "_btnEdit";
            this._btnEdit.Size = new System.Drawing.Size(100, 32);
            this._btnEdit.Text = "✏  Edit";
            this._btnEdit.UseVisualStyleBackColor = false;
            this._btnEdit.Click += new System.EventHandler(this.BtnEdit_Click);

            // ViewAnnouncementAdmin
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this._btnEdit);
            this.Controls.Add(this._btnDelete);
            this.Controls.Add(this.pnlDivider);
            this.Controls.Add(this._lblAttachment);
            this.Controls.Add(this._lblNotify);
            this.Controls.Add(this._lblDate);
            this.Controls.Add(this._lblAuthor);
            this.Controls.Add(this._lblDesc);
            this.Controls.Add(this._lblTitle);
            this.Controls.Add(this._lblStatus);
            this.Controls.Add(this._lblCat);
            this.Controls.Add(this._pnlHeader);
            this.Name = "ViewAnnouncementAdmin";
            this.Size = new System.Drawing.Size(760, 440);
            this._pnlHeader.ResumeLayout(false);
            this._pnlHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel _pnlHeader;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label _lblCat;
        private System.Windows.Forms.Label _lblStatus;
        private System.Windows.Forms.Label _lblTitle;
        private System.Windows.Forms.Label _lblDesc;
        private System.Windows.Forms.Label _lblAuthor;
        private System.Windows.Forms.Label _lblDate;
        private System.Windows.Forms.Label _lblNotify;
        private System.Windows.Forms.Label _lblAttachment;
        private System.Windows.Forms.Panel pnlDivider;
        private System.Windows.Forms.Button _btnDelete;
        private System.Windows.Forms.Button _btnEdit;
    }
}