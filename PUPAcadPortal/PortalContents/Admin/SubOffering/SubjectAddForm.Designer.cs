namespace PUPAcadPortal.PortalContents.Admin.SubOffering
{
    partial class SubjectAddForm
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblHeaderTitle = new Label();
            lblSubjectCode = new Label();
            txtSubjectCode = new TextBox();
            lblSubjectName = new Label();
            txtSubjectName = new TextBox();
            lblDepartment = new Label();
            cmbDepartment = new ComboBox();
            lblDescription = new Label();
            txtDescription = new TextBox();
            lblLecUnits = new Label();
            numLec = new NumericUpDown();
            lblLabUnits = new Label();
            numLab = new NumericUpDown();
            label1 = new Label();
            numUnits = new NumericUpDown();
            lblPrerequisites = new Label();
            clbPrerequisites = new CheckedListBox();
            btnSave = new Button();
            btnCancel = new Button();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numLec).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numLab).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numUnits).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(128, 0, 0);
            pnlHeader.Controls.Add(lblHeaderTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(400, 50);
            pnlHeader.TabIndex = 0;
            // 
            // lblHeaderTitle
            // 
            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHeaderTitle.ForeColor = Color.Gold;
            lblHeaderTitle.Location = new Point(15, 15);
            lblHeaderTitle.Name = "lblHeaderTitle";
            lblHeaderTitle.Size = new Size(141, 21);
            lblHeaderTitle.TabIndex = 0;
            lblHeaderTitle.Text = "Add New Subject";
            // 
            // lblSubjectCode
            // 
            lblSubjectCode.AutoSize = true;
            lblSubjectCode.Location = new Point(20, 75);
            lblSubjectCode.Name = "lblSubjectCode";
            lblSubjectCode.Size = new Size(88, 17);
            lblSubjectCode.TabIndex = 1;
            lblSubjectCode.Text = "Subject Code:";
            // 
            // txtSubjectCode
            // 
            txtSubjectCode.CharacterCasing = CharacterCasing.Upper;
            txtSubjectCode.Location = new Point(140, 72);
            txtSubjectCode.Name = "txtSubjectCode";
            txtSubjectCode.Size = new Size(230, 25);
            txtSubjectCode.TabIndex = 2;
            // 
            // lblSubjectName
            // 
            lblSubjectName.AutoSize = true;
            lblSubjectName.Location = new Point(20, 115);
            lblSubjectName.Name = "lblSubjectName";
            lblSubjectName.Size = new Size(92, 17);
            lblSubjectName.TabIndex = 3;
            lblSubjectName.Text = "Subject Name:";
            // 
            // txtSubjectName
            // 
            txtSubjectName.Location = new Point(140, 112);
            txtSubjectName.Name = "txtSubjectName";
            txtSubjectName.Size = new Size(230, 25);
            txtSubjectName.TabIndex = 4;
            // 
            // lblDepartment
            // 
            lblDepartment.AutoSize = true;
            lblDepartment.Location = new Point(20, 155);
            lblDepartment.Name = "lblDepartment";
            lblDepartment.Size = new Size(80, 17);
            lblDepartment.TabIndex = 5;
            lblDepartment.Text = "Department:";
            // 
            // cmbDepartment
            // 
            cmbDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDepartment.FormattingEnabled = true;
            cmbDepartment.Location = new Point(140, 152);
            cmbDepartment.Name = "cmbDepartment";
            cmbDepartment.Size = new Size(230, 25);
            cmbDepartment.TabIndex = 6;
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.Location = new Point(20, 195);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(77, 17);
            lblDescription.TabIndex = 7;
            lblDescription.Text = "Description:";
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(140, 192);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(230, 52);
            txtDescription.TabIndex = 8;
            // 
            // lblLecUnits
            // 
            lblLecUnits.AutoSize = true;
            lblLecUnits.Location = new Point(20, 260);
            lblLecUnits.Name = "lblLecUnits";
            lblLecUnits.Size = new Size(86, 17);
            lblLecUnits.TabIndex = 9;
            lblLecUnits.Text = "Lecture Units:";
            // 
            // numLec
            // 
            numLec.Location = new Point(140, 258);
            numLec.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numLec.Name = "numLec";
            numLec.Size = new Size(80, 25);
            numLec.TabIndex = 10;
            // 
            // lblLabUnits
            // 
            lblLabUnits.AutoSize = true;
            lblLabUnits.Location = new Point(235, 260);
            lblLabUnits.Name = "lblLabUnits";
            lblLabUnits.Size = new Size(65, 17);
            lblLabUnits.TabIndex = 11;
            lblLabUnits.Text = "Lab Units:";
            // 
            // numLab
            // 
            numLab.Location = new Point(306, 258);
            numLab.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numLab.Name = "numLab";
            numLab.Size = new Size(64, 25);
            numLab.TabIndex = 12;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(236, 294);
            label1.Name = "label1";
            label1.Size = new Size(40, 17);
            label1.TabIndex = 16;
            label1.Text = "Units:";
            // 
            // numUnits
            // 
            numUnits.Location = new Point(306, 292);
            numUnits.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numUnits.Name = "numUnits";
            numUnits.Size = new Size(64, 25);
            numUnits.TabIndex = 15;
            numUnits.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // lblPrerequisites
            // 
            lblPrerequisites.AutoSize = true;
            lblPrerequisites.Location = new Point(20, 330);
            lblPrerequisites.Name = "lblPrerequisites";
            lblPrerequisites.Size = new Size(86, 17);
            lblPrerequisites.TabIndex = 17;
            lblPrerequisites.Text = "Prerequisites:";
            // 
            // clbPrerequisites
            // 
            clbPrerequisites.CheckOnClick = true;
            clbPrerequisites.FormattingEnabled = true;
            clbPrerequisites.Location = new Point(140, 330);
            clbPrerequisites.Name = "clbPrerequisites";
            clbPrerequisites.Size = new Size(230, 104);
            clbPrerequisites.TabIndex = 18;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(128, 0, 0);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(190, 455);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(90, 35);
            btnSave.TabIndex = 13;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.LightGray;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Location = new Point(286, 455);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(84, 35);
            btnCancel.TabIndex = 14;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // SubjectAddForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(400, 515);
            Controls.Add(clbPrerequisites);
            Controls.Add(lblPrerequisites);
            Controls.Add(label1);
            Controls.Add(numUnits);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(numLab);
            Controls.Add(lblLabUnits);
            Controls.Add(numLec);
            Controls.Add(lblLecUnits);
            Controls.Add(txtDescription);
            Controls.Add(lblDescription);
            Controls.Add(cmbDepartment);
            Controls.Add(lblDepartment);
            Controls.Add(txtSubjectName);
            Controls.Add(lblSubjectName);
            Controls.Add(txtSubjectCode);
            Controls.Add(lblSubjectCode);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SubjectAddForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Manage Subject";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numLec).EndInit();
            ((System.ComponentModel.ISupportInitialize)numLab).EndInit();
            ((System.ComponentModel.ISupportInitialize)numUnits).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Label lblSubjectCode;
        private System.Windows.Forms.TextBox txtSubjectCode;
        private System.Windows.Forms.Label lblSubjectName;
        private System.Windows.Forms.TextBox txtSubjectName;
        private System.Windows.Forms.Label lblDepartment;
        private System.Windows.Forms.ComboBox cmbDepartment;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblLecUnits;
        private System.Windows.Forms.NumericUpDown numLec;
        private System.Windows.Forms.Label lblLabUnits;
        private System.Windows.Forms.NumericUpDown numLab;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numUnits;
        private System.Windows.Forms.Label lblPrerequisites;
        private System.Windows.Forms.CheckedListBox clbPrerequisites;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}