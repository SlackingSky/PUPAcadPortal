namespace PUPAcadPortal
{
    partial class AnnouncementInbox
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
            _pnlHeader = new Panel();
            _lblHeader = new Label();
            _lblUnreadBadge = new Label();
            _pnlLeft = new Panel();
            _txtSearch = new TextBox();
            _flpList = new FlowLayoutPanel();
            _pnlRight = new Panel();
            _lblDetailTag = new Label();
            _lblDetailSubject = new Label();
            _lblDetailMeta = new Label();
            _dividerH = new Panel();
            _lblDetailBody = new Label();
            _pnlActions = new Panel();
            _btnStar = new Button();
            _btnMarkRead = new Button();
            _btnDelete = new Button();
            _dividerV = new Panel();
            _btnClose = new Button();
            _pnlHeader.SuspendLayout();
            _pnlLeft.SuspendLayout();
            _pnlRight.SuspendLayout();
            _pnlActions.SuspendLayout();
            SuspendLayout();
            // 
            // _pnlHeader
            // 
            _pnlHeader.BackColor = Color.FromArgb(139, 0, 0);
            _pnlHeader.Controls.Add(_lblHeader);
            _pnlHeader.Controls.Add(_lblUnreadBadge);
            _pnlHeader.Dock = DockStyle.Top;
            _pnlHeader.Location = new Point(0, 0);
            _pnlHeader.Name = "_pnlHeader";
            _pnlHeader.Size = new Size(1171, 48);
            _pnlHeader.TabIndex = 3;
            // 
            // _lblHeader
            // 
            _lblHeader.AutoSize = true;
            _lblHeader.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            _lblHeader.ForeColor = Color.White;
            _lblHeader.Location = new Point(14, 13);
            _lblHeader.Name = "_lblHeader";
            _lblHeader.Size = new Size(140, 21);
            _lblHeader.TabIndex = 0;
            _lblHeader.Text = "📬  Admin Inbox";
            // 
            // _lblUnreadBadge
            // 
            _lblUnreadBadge.BackColor = Color.Firebrick;
            _lblUnreadBadge.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            _lblUnreadBadge.ForeColor = Color.White;
            _lblUnreadBadge.Location = new Point(165, 16);
            _lblUnreadBadge.Name = "_lblUnreadBadge";
            _lblUnreadBadge.Size = new Size(24, 16);
            _lblUnreadBadge.TabIndex = 1;
            _lblUnreadBadge.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // _pnlLeft
            // 
            _pnlLeft.BackColor = Color.FromArgb(248, 248, 249);
            _pnlLeft.Controls.Add(_txtSearch);
            _pnlLeft.Controls.Add(_flpList);
            _pnlLeft.Dock = DockStyle.Left;
            _pnlLeft.Location = new Point(0, 48);
            _pnlLeft.Name = "_pnlLeft";
            _pnlLeft.Size = new Size(290, 532);
            _pnlLeft.TabIndex = 2;
            // 
            // _txtSearch
            // 
            _txtSearch.BorderStyle = BorderStyle.FixedSingle;
            _txtSearch.Font = new Font("Segoe UI", 9F);
            _txtSearch.Location = new Point(8, 8);
            _txtSearch.Name = "_txtSearch";
            _txtSearch.PlaceholderText = "🔍  Search messages…";
            _txtSearch.Size = new Size(274, 23);
            _txtSearch.TabIndex = 0;
            _txtSearch.TextChanged += TxtSearch_TextChanged;
            // 
            // _flpList
            // 
            _flpList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _flpList.AutoScroll = true;
            _flpList.BackColor = Color.FromArgb(248, 248, 249);
            _flpList.FlowDirection = FlowDirection.TopDown;
            _flpList.Location = new Point(0, 40);
            _flpList.Name = "_flpList";
            _flpList.Size = new Size(290, 924);
            _flpList.TabIndex = 1;
            _flpList.WrapContents = false;
            // 
            // _pnlRight
            // 
            _pnlRight.BackColor = Color.White;
            _pnlRight.Controls.Add(_lblDetailTag);
            _pnlRight.Controls.Add(_lblDetailSubject);
            _pnlRight.Controls.Add(_lblDetailMeta);
            _pnlRight.Controls.Add(_dividerH);
            _pnlRight.Controls.Add(_pnlActions);
            _pnlRight.Controls.Add(_lblDetailBody);
            _pnlRight.Dock = DockStyle.Fill;
            _pnlRight.Location = new Point(291, 48);
            _pnlRight.Name = "_pnlRight";
            _pnlRight.Padding = new Padding(20, 16, 20, 16);
            _pnlRight.Size = new Size(880, 532);
            _pnlRight.TabIndex = 0;
            // 
            // _lblDetailTag
            // 
            _lblDetailTag.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            _lblDetailTag.Location = new Point(20, 16);
            _lblDetailTag.Name = "_lblDetailTag";
            _lblDetailTag.Size = new Size(72, 20);
            _lblDetailTag.TabIndex = 0;
            _lblDetailTag.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // _lblDetailSubject
            // 
            _lblDetailSubject.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _lblDetailSubject.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            _lblDetailSubject.ForeColor = Color.FromArgb(20, 20, 20);
            _lblDetailSubject.Location = new Point(20, 44);
            _lblDetailSubject.Name = "_lblDetailSubject";
            _lblDetailSubject.Size = new Size(736, 32);
            _lblDetailSubject.TabIndex = 1;
            // 
            // _lblDetailMeta
            // 
            _lblDetailMeta.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _lblDetailMeta.Font = new Font("Segoe UI", 8.5F);
            _lblDetailMeta.ForeColor = Color.Gray;
            _lblDetailMeta.Location = new Point(20, 80);
            _lblDetailMeta.Name = "_lblDetailMeta";
            _lblDetailMeta.Size = new Size(736, 18);
            _lblDetailMeta.TabIndex = 2;
            // 
            // _dividerH
            // 
            _dividerH.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _dividerH.BackColor = Color.FromArgb(230, 230, 230);
            _dividerH.Location = new Point(20, 104);
            _dividerH.Name = "_dividerH";
            _dividerH.Size = new Size(736, 1);
            _dividerH.TabIndex = 3;
            // 
            // _lblDetailBody
            // 
            _lblDetailBody.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _lblDetailBody.Font = new Font("Segoe UI", 9.5F);
            _lblDetailBody.ForeColor = Color.FromArgb(50, 50, 50);
            _lblDetailBody.Location = new Point(20, 116);
            _lblDetailBody.Name = "_lblDetailBody";
            _lblDetailBody.Size = new Size(736, 806);
            _lblDetailBody.TabIndex = 4;
            _lblDetailBody.UseMnemonic = false;
            // 
            // _pnlActions
            // 
            _pnlActions.BackColor = Color.White;
            _pnlActions.Controls.Add(_btnClose);
            _pnlActions.Controls.Add(_btnStar);
            _pnlActions.Controls.Add(_btnMarkRead);
            _pnlActions.Controls.Add(_btnDelete);
            _pnlActions.Dock = DockStyle.Bottom;
            _pnlActions.Location = new Point(20, 460);
            _pnlActions.Name = "_pnlActions";
            _pnlActions.Padding = new Padding(20, 12, 20, 0);
            _pnlActions.Size = new Size(840, 56);
            _pnlActions.TabIndex = 5;
            // 
            // _btnStar
            // 
            _btnStar.BackColor = Color.FromArgb(255, 249, 220);
            _btnStar.Cursor = Cursors.Hand;
            _btnStar.FlatAppearance.BorderColor = Color.FromArgb(220, 200, 100);
            _btnStar.FlatStyle = FlatStyle.Flat;
            _btnStar.Font = new Font("Segoe UI", 9F);
            _btnStar.ForeColor = Color.FromArgb(130, 100, 0);
            _btnStar.Location = new Point(20, 12);
            _btnStar.Name = "_btnStar";
            _btnStar.Size = new Size(88, 30);
            _btnStar.TabIndex = 0;
            _btnStar.Text = "☆  Star";
            _btnStar.UseVisualStyleBackColor = false;
            _btnStar.Click += BtnStar_Click;
            // 
            // _btnMarkRead
            // 
            _btnMarkRead.BackColor = Color.FromArgb(220, 252, 231);
            _btnMarkRead.Cursor = Cursors.Hand;
            _btnMarkRead.FlatAppearance.BorderColor = Color.FromArgb(140, 210, 160);
            _btnMarkRead.FlatStyle = FlatStyle.Flat;
            _btnMarkRead.Font = new Font("Segoe UI", 9F);
            _btnMarkRead.ForeColor = Color.FromArgb(30, 110, 60);
            _btnMarkRead.Location = new Point(116, 12);
            _btnMarkRead.Name = "_btnMarkRead";
            _btnMarkRead.Size = new Size(110, 30);
            _btnMarkRead.TabIndex = 1;
            _btnMarkRead.Text = "✓  Mark Read";
            _btnMarkRead.UseVisualStyleBackColor = false;
            _btnMarkRead.Click += BtnMarkRead_Click;
            // 
            // _btnDelete
            // 
            _btnDelete.BackColor = Color.White;
            _btnDelete.Cursor = Cursors.Hand;
            _btnDelete.FlatAppearance.BorderColor = Color.FromArgb(200, 50, 50);
            _btnDelete.FlatStyle = FlatStyle.Flat;
            _btnDelete.Font = new Font("Segoe UI", 9F);
            _btnDelete.ForeColor = Color.FromArgb(180, 0, 0);
            _btnDelete.Location = new Point(234, 12);
            _btnDelete.Name = "_btnDelete";
            _btnDelete.Size = new Size(90, 30);
            _btnDelete.TabIndex = 2;
            _btnDelete.Text = "🗑  Delete";
            _btnDelete.UseVisualStyleBackColor = false;
            _btnDelete.Click += BtnDelete_Click;
            // 
            // _dividerV
            // 
            _dividerV.BackColor = Color.FromArgb(220, 220, 220);
            _dividerV.Dock = DockStyle.Left;
            _dividerV.Location = new Point(290, 48);
            _dividerV.Name = "_dividerV";
            _dividerV.Size = new Size(1, 532);
            _dividerV.TabIndex = 1;
            // 
            // _btnClose
            // 
            _btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _btnClose.BackColor = Color.FromArgb(110, 0, 0);
            _btnClose.Cursor = Cursors.Hand;
            _btnClose.FlatAppearance.BorderSize = 0;
            _btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(160, 0, 0);
            _btnClose.FlatStyle = FlatStyle.Flat;
            _btnClose.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnClose.ForeColor = Color.White;
            _btnClose.Location = new Point(753, 12);
            _btnClose.Name = "_btnClose";
            _btnClose.Size = new Size(64, 28);
            _btnClose.TabIndex = 3;
            _btnClose.Text = "Close";
            _btnClose.UseVisualStyleBackColor = false;
            // 
            // AnnouncementInbox
            // 
            BackColor = Color.White;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(_pnlRight);
            Controls.Add(_dividerV);
            Controls.Add(_pnlLeft);
            Controls.Add(_pnlHeader);
            Name = "AnnouncementInbox";
            Size = new Size(1171, 580);
            Resize += AnnouncementInbox_Resize;
            _pnlHeader.ResumeLayout(false);
            _pnlHeader.PerformLayout();
            _pnlLeft.ResumeLayout(false);
            _pnlLeft.PerformLayout();
            _pnlRight.ResumeLayout(false);
            _pnlActions.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel _pnlHeader;
        private System.Windows.Forms.Label _lblHeader;
        private System.Windows.Forms.Label _lblUnreadBadge;
        private System.Windows.Forms.Panel _pnlLeft;
        private System.Windows.Forms.TextBox _txtSearch;
        private System.Windows.Forms.FlowLayoutPanel _flpList;
        private System.Windows.Forms.Panel _pnlRight;
        private System.Windows.Forms.Label _lblDetailTag;
        private System.Windows.Forms.Label _lblDetailSubject;
        private System.Windows.Forms.Label _lblDetailMeta;
        private System.Windows.Forms.Label _lblDetailBody;
        private System.Windows.Forms.Panel _pnlActions;
        private System.Windows.Forms.Button _btnStar;
        private System.Windows.Forms.Button _btnMarkRead;
        private System.Windows.Forms.Button _btnDelete;
        private System.Windows.Forms.Panel _dividerV;
        private System.Windows.Forms.Panel _dividerH;
        private Button _btnClose;
    }
}