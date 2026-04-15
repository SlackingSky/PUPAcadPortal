namespace PUPAcadPortal
{
    partial class AddNotesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
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
            lblFormTitle = new Label();
            lblDateCaption = new Label();
            txtDate = new TextBox();
            lblNote = new Label();
            txtNote = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            btnDelete = new Button();
            pnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.Maroon;
            pnlHeader.Controls.Add(lblFormTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(414, 46);
            pnlHeader.TabIndex = 0;
            // 
            // lblFormTitle
            // 
            lblFormTitle.Dock = DockStyle.Fill;
            lblFormTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblFormTitle.ForeColor = Color.White;
            lblFormTitle.Location = new Point(0, 0);
            lblFormTitle.Name = "lblFormTitle";
            lblFormTitle.Padding = new Padding(12, 0, 0, 0);
            lblFormTitle.Size = new Size(414, 46);
            lblFormTitle.TabIndex = 0;
            lblFormTitle.Text = "Add / Edit Note";
            lblFormTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDateCaption
            // 
            lblDateCaption.AutoSize = true;
            lblDateCaption.ForeColor = Color.FromArgb(60, 60, 60);
            lblDateCaption.Location = new Point(16, 63);
            lblDateCaption.Name = "lblDateCaption";
            lblDateCaption.Size = new Size(38, 17);
            lblDateCaption.TabIndex = 1;
            lblDateCaption.Text = "Date:";
            // 
            // txtDate
            // 
            txtDate.BackColor = Color.White;
            txtDate.BorderStyle = BorderStyle.FixedSingle;
            txtDate.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            txtDate.Location = new Point(148, 60);
            txtDate.Name = "txtDate";
            txtDate.ReadOnly = true;
            txtDate.Size = new Size(238, 24);
            txtDate.TabIndex = 2;
            txtDate.TabStop = false;
            // 
            // lblNote
            // 
            lblNote.AutoSize = true;
            lblNote.ForeColor = Color.FromArgb(60, 60, 60);
            lblNote.Location = new Point(16, 103);
            lblNote.Name = "lblNote";
            lblNote.Size = new Size(40, 17);
            lblNote.TabIndex = 3;
            lblNote.Text = "Note:";
            // 
            // txtNote
            // 
            txtNote.Location = new Point(148, 100);
            txtNote.Multiline = true;
            txtNote.Name = "txtNote";
            txtNote.PlaceholderText = "My Notes";
            txtNote.ScrollBars = ScrollBars.Vertical;
            txtNote.Size = new Size(238, 120);
            txtNote.TabIndex = 4;
            txtNote.TextChanged += txtNote_TextChanged;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.Maroon;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(148, 238);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(112, 34);
            btnSave.TabIndex = 5;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 9.5F);
            btnCancel.Location = new Point(268, 238);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(112, 34);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnDelete
            // 
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 9.5F);
            btnDelete.ForeColor = Color.FromArgb(180, 0, 0);
            btnDelete.Location = new Point(16, 238);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(112, 34);
            btnDelete.TabIndex = 7;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // AddNotesForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(414, 290);
            Controls.Add(lblDateCaption);
            Controls.Add(txtDate);
            Controls.Add(lblNote);
            Controls.Add(txtNote);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            Controls.Add(btnDelete);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9.5F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AddNotesForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Add Note";
            Load += AddNotesForm_Load;
            pnlHeader.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Label lblDateCaption;
        private System.Windows.Forms.TextBox txtDate;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDelete;
    }
}