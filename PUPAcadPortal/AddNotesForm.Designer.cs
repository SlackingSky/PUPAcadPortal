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
            lblHeader = new Label();
            lblDate = new Label();
            lblDateVal = new Label();
            lblNote = new Label();
            txtNote = new TextBox();
            btnSave = new Button();
            btnDelete = new Button();
            btnCancel = new Button();
            pnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.Maroon;
            pnlHeader.Controls.Add(lblHeader);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(398, 44);
            pnlHeader.TabIndex = 0;
            // 
            // lblHeader
            // 
            lblHeader.Dock = DockStyle.Fill;
            lblHeader.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblHeader.ForeColor = Color.White;
            lblHeader.Location = new Point(0, 0);
            lblHeader.Name = "lblHeader";
            lblHeader.Padding = new Padding(10, 0, 0, 0);
            lblHeader.Size = new Size(398, 44);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "Add / Edit Note";
            lblHeader.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDate
            // 
            lblDate.Font = new Font("Segoe UI", 9F);
            lblDate.ForeColor = Color.FromArgb(60, 60, 60);
            lblDate.Location = new Point(12, 54);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(40, 20);
            lblDate.TabIndex = 1;
            lblDate.Text = "Date:";
            // 
            // lblDateVal
            // 
            lblDateVal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDateVal.ForeColor = Color.Maroon;
            lblDateVal.Location = new Point(58, 54);
            lblDateVal.Name = "lblDateVal";
            lblDateVal.Size = new Size(310, 20);
            lblDateVal.TabIndex = 2;
            // 
            // lblNote
            // 
            lblNote.Font = new Font("Segoe UI", 9F);
            lblNote.ForeColor = Color.FromArgb(60, 60, 60);
            lblNote.Location = new Point(12, 84);
            lblNote.Name = "lblNote";
            lblNote.Size = new Size(40, 20);
            lblNote.TabIndex = 3;
            lblNote.Text = "Note:";
            // 
            // txtNote
            // 
            txtNote.Font = new Font("Segoe UI", 9F);
            txtNote.Location = new Point(12, 108);
            txtNote.Multiline = true;
            txtNote.Name = "txtNote";
            txtNote.PlaceholderText = "My Notes";
            txtNote.ScrollBars = ScrollBars.Vertical;
            txtNote.Size = new Size(372, 88);
            txtNote.TabIndex = 4;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.Maroon;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9F);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(196, 212);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(90, 30);
            btnSave.TabIndex = 5;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnDelete
            // 
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 9F);
            btnDelete.ForeColor = Color.Maroon;
            btnDelete.Location = new Point(12, 212);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(90, 30);
            btnDelete.TabIndex = 6;
            btnDelete.Text = "Delete";
            btnDelete.Click += btnDelete_Click;
            // 
            // btnCancel
            // 
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 9F);
            btnCancel.Location = new Point(294, 212);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 30);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Cancel";
            btnCancel.Click += btnCancel_Click;
            // 
            // AddNotesForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(398, 258);
            Controls.Add(pnlHeader);
            Controls.Add(lblDate);
            Controls.Add(lblDateVal);
            Controls.Add(lblNote);
            Controls.Add(txtNote);
            Controls.Add(btnSave);
            Controls.Add(btnDelete);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AddNotesForm";
            StartPosition = FormStartPosition.CenterParent;
            pnlHeader.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblDateVal;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
    }
}