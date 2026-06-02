namespace PUPAcadPortal
{
    partial class AddModuleDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblHeaderTitle = new Label();
            iconPanel = new Panel();
            body = new Panel();
            lblTitle = new Label();
            _txtTitle = new TextBox();
            lblDesc = new Label();
            _txtDesc = new TextBox();
            lblFiles = new Label();
            _pnlDrop = new Panel();
            lblDropInstruction = new Label();
            lblDropSubtext = new Label();
            btnBrowse = new Button();
            _lblFileCount = new Label();
            _lblFileList = new Label();
            divider = new Panel();
            btnCancel = new Button();
            btnOk = new Button();
            pnlHeader.SuspendLayout();
            body.SuspendLayout();
            _pnlDrop.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(139, 0, 0);
            pnlHeader.Controls.Add(lblHeaderTitle);
            pnlHeader.Controls.Add(iconPanel);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(520, 56);
            pnlHeader.TabIndex = 0;
            // 
            // lblHeaderTitle
            // 
            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblHeaderTitle.ForeColor = Color.White;
            lblHeaderTitle.Location = new Point(54, 16);
            lblHeaderTitle.Name = "lblHeaderTitle";
            lblHeaderTitle.Size = new Size(117, 25);
            lblHeaderTitle.TabIndex = 1;
            lblHeaderTitle.Text = "Add Module";
            // 
            // iconPanel
            // 
            iconPanel.BackColor = Color.Transparent;
            iconPanel.Location = new Point(14, 12);
            iconPanel.Name = "iconPanel";
            iconPanel.Size = new Size(32, 32);
            iconPanel.TabIndex = 0;
            iconPanel.Paint += IconPanel_Paint;
            // 
            // body
            // 
            body.BackColor = Color.White;
            body.Controls.Add(lblTitle);
            body.Controls.Add(_txtTitle);
            body.Controls.Add(lblDesc);
            body.Controls.Add(_txtDesc);
            body.Controls.Add(lblFiles);
            body.Controls.Add(_pnlDrop);
            body.Controls.Add(_lblFileCount);
            body.Controls.Add(_lblFileList);
            body.Location = new Point(0, 56);
            body.Name = "body";
            body.Size = new Size(520, 364);
            body.TabIndex = 1;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(60, 60, 70);
            lblTitle.Location = new Point(20, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(85, 15);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Module Title *";
            // 
            // _txtTitle
            // 
            _txtTitle.BorderStyle = BorderStyle.FixedSingle;
            _txtTitle.Font = new Font("Segoe UI", 10.5F);
            _txtTitle.ForeColor = Color.FromArgb(30, 30, 35);
            _txtTitle.Location = new Point(20, 38);
            _txtTitle.MaxLength = 120;
            _txtTitle.Name = "_txtTitle";
            _txtTitle.Size = new Size(480, 26);
            _txtTitle.TabIndex = 1;
            _txtTitle.Enter += TxtTitle_Enter;
            // 
            // lblDesc
            // 
            lblDesc.AutoSize = true;
            lblDesc.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDesc.ForeColor = Color.FromArgb(60, 60, 70);
            lblDesc.Location = new Point(20, 74);
            lblDesc.Name = "lblDesc";
            lblDesc.Size = new Size(127, 15);
            lblDesc.TabIndex = 2;
            lblDesc.Text = "Description (optional)";
            // 
            // _txtDesc
            // 
            _txtDesc.BorderStyle = BorderStyle.FixedSingle;
            _txtDesc.Font = new Font("Segoe UI", 9.5F);
            _txtDesc.Location = new Point(20, 94);
            _txtDesc.MaxLength = 500;
            _txtDesc.Multiline = true;
            _txtDesc.Name = "_txtDesc";
            _txtDesc.PlaceholderText = "Briefly describe what this module covers…";
            _txtDesc.ScrollBars = ScrollBars.Vertical;
            _txtDesc.Size = new Size(480, 72);
            _txtDesc.TabIndex = 3;
            // 
            // lblFiles
            // 
            lblFiles.AutoSize = true;
            lblFiles.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFiles.ForeColor = Color.FromArgb(60, 60, 70);
            lblFiles.Location = new Point(20, 178);
            lblFiles.Name = "lblFiles";
            lblFiles.Size = new Size(127, 15);
            lblFiles.TabIndex = 4;
            lblFiles.Text = "Attach Files (optional)";
            // 
            // _pnlDrop
            // 
            _pnlDrop.BackColor = Color.FromArgb(248, 248, 250);
            _pnlDrop.Controls.Add(btnBrowse);
            _pnlDrop.Controls.Add(lblDropInstruction);
            _pnlDrop.Controls.Add(lblDropSubtext);
            _pnlDrop.Cursor = Cursors.Hand;
            _pnlDrop.Location = new Point(20, 198);
            _pnlDrop.Name = "_pnlDrop";
            _pnlDrop.Size = new Size(480, 78);
            _pnlDrop.TabIndex = 5;
            _pnlDrop.Click += BrowseFiles;
            _pnlDrop.Paint += PaintDropZone;
            // 
            // lblDropInstruction
            // 
            lblDropInstruction.BackColor = Color.Transparent;
            lblDropInstruction.Font = new Font("Segoe UI", 9F);
            lblDropInstruction.ForeColor = Color.FromArgb(100, 100, 110);
            lblDropInstruction.Location = new Point(60, 16);
            lblDropInstruction.Name = "lblDropInstruction";
            lblDropInstruction.Size = new Size(322, 20);
            lblDropInstruction.TabIndex = 0;
            lblDropInstruction.Text = "📎  Drop files here or click to browse";
            // 
            // lblDropSubtext
            // 
            lblDropSubtext.BackColor = Color.Transparent;
            lblDropSubtext.Font = new Font("Segoe UI", 8F);
            lblDropSubtext.ForeColor = Color.FromArgb(140, 140, 150);
            lblDropSubtext.Location = new Point(60, 38);
            lblDropSubtext.Name = "lblDropSubtext";
            lblDropSubtext.Size = new Size(322, 16);
            lblDropSubtext.TabIndex = 1;
            lblDropSubtext.Text = "Supported: PDF, DOCX, PPTX, PNG, JPG  ·  Max 10 MB";
            // 
            // btnBrowse
            // 
            btnBrowse.BackColor = Color.FromArgb(139, 0, 0);
            btnBrowse.Cursor = Cursors.Hand;
            btnBrowse.FlatAppearance.BorderSize = 0;
            btnBrowse.FlatStyle = FlatStyle.Flat;
            btnBrowse.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnBrowse.ForeColor = Color.White;
            btnBrowse.Location = new Point(388, 24);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(80, 28);
            btnBrowse.TabIndex = 2;
            btnBrowse.Text = "Browse";
            btnBrowse.UseVisualStyleBackColor = false;
            btnBrowse.Click += BrowseFiles;
            // 
            // _lblFileCount
            // 
            _lblFileCount.BackColor = Color.White;
            _lblFileCount.Font = new Font("Segoe UI", 8.5F);
            _lblFileCount.ForeColor = Color.FromArgb(80, 80, 90);
            _lblFileCount.Location = new Point(20, 280);
            _lblFileCount.Name = "_lblFileCount";
            _lblFileCount.Size = new Size(480, 0);
            _lblFileCount.TabIndex = 6;
            // 
            // _lblFileList
            // 
            _lblFileList.BackColor = Color.White;
            _lblFileList.Font = new Font("Segoe UI", 8F);
            _lblFileList.ForeColor = Color.Gray;
            _lblFileList.Location = new Point(24, 296);
            _lblFileList.Name = "_lblFileList";
            _lblFileList.Size = new Size(476, 0);
            _lblFileList.TabIndex = 7;
            // 
            // divider
            // 
            divider.BackColor = Color.FromArgb(218, 218, 225);
            divider.Location = new Point(0, 420);
            divider.Name = "divider";
            divider.Size = new Size(520, 1);
            divider.TabIndex = 2;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.White;
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatAppearance.BorderColor = Color.Silver;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 9.5F);
            btnCancel.ForeColor = Color.FromArgb(100, 100, 110);
            btnCancel.Location = new Point(282, 432);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(108, 36);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnOk
            // 
            btnOk.BackColor = Color.FromArgb(139, 0, 0);
            btnOk.Cursor = Cursors.Hand;
            btnOk.FlatStyle = FlatStyle.Flat;
            btnOk.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnOk.ForeColor = Color.White;
            btnOk.Location = new Point(398, 432);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(108, 36);
            btnOk.TabIndex = 4;
            btnOk.Text = "Add Module";
            btnOk.UseVisualStyleBackColor = false;
            btnOk.Click += BtnOk_Click;
            // 
            // AddModuleDialog
            // 
            AcceptButton = btnOk;
            BackColor = Color.White;
            CancelButton = btnCancel;
            ClientSize = new Size(520, 480);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
            Controls.Add(divider);
            Controls.Add(body);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9.5F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AddModuleDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Add Module";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            body.ResumeLayout(false);
            body.PerformLayout();
            _pnlDrop.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Panel iconPanel;
        private System.Windows.Forms.Panel body;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox _txtTitle;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.TextBox _txtDesc;
        private System.Windows.Forms.Label lblFiles;
        private System.Windows.Forms.Panel _pnlDrop;
        private System.Windows.Forms.Label lblDropInstruction;
        private System.Windows.Forms.Label lblDropSubtext;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label _lblFileCount;
        private System.Windows.Forms.Label _lblFileList;
        private System.Windows.Forms.Panel divider;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
    }
}