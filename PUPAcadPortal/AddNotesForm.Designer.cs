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
            btnSave = new Button();
            btnCancel = new Button();
            txtDate = new TextBox();
            btnDelete = new Button();
            txtNote = new TextBox();
            SuspendLayout();
            // 
            // btnSave
            // 
            btnSave.Location = new Point(12, 350);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(80, 35);
            btnSave.TabIndex = 3;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(417, 350);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 35);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // txtDate
            // 
            txtDate.CausesValidation = false;
            txtDate.Location = new Point(12, 12);
            txtDate.Name = "txtDate";
            txtDate.ReadOnly = true;
            txtDate.Size = new Size(100, 23);
            txtDate.TabIndex = 6;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(98, 350);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(80, 35);
            btnDelete.TabIndex = 7;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // txtNote
            // 
            txtNote.Location = new Point(12, 41);
            txtNote.Multiline = true;
            txtNote.Name = "txtNote";
            txtNote.Size = new Size(480, 285);
            txtNote.TabIndex = 2;
            txtNote.TextChanged += txtNote_TextChanged;
            // 
            // AddNotesForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(504, 393);
            Controls.Add(txtNote);
            Controls.Add(btnDelete);
            Controls.Add(txtDate);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Name = "AddNotesForm";
            SizeGripStyle = SizeGripStyle.Show;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Add Notes";
            Load += AddNotesForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnSave;
        private Button btnCancel;
        private TextBox txtDate;
        private Button btnDelete;
        private TextBox txtNote;
    }
}