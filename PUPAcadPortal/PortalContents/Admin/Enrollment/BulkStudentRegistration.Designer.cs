namespace PUPAcadPortal.PortalContents.Admin.Enrollment
{
    partial class BulkStudentRegistration
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            dgvStudents = new DataGridView();
            clmFirstName = new DataGridViewTextBoxColumn();
            clmMiddleName = new DataGridViewTextBoxColumn();
            clmLastName = new DataGridViewTextBoxColumn();
            clmSuffix = new DataGridViewTextBoxColumn();
            clmDOB = new DataGridViewTextBoxColumn();
            clmPersonalEmail = new DataGridViewTextBoxColumn();
            clmPhone = new DataGridViewTextBoxColumn();
            clmAddress1 = new DataGridViewTextBoxColumn();
            clmAddress2 = new DataGridViewTextBoxColumn();
            clmRegion = new DataGridViewTextBoxColumn();
            clmProvince = new DataGridViewTextBoxColumn();
            clmCity = new DataGridViewTextBoxColumn();
            clmBarangay = new DataGridViewTextBoxColumn();
            clmPostalCode = new DataGridViewTextBoxColumn();
            clmProgram = new DataGridViewTextBoxColumn();
            btnRegisterAll = new Button();
            btnImport = new Button();
            btnClearTable = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvStudents).BeginInit();
            SuspendLayout();
            // 
            // dgvStudents
            // 
            dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStudents.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dgvStudents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvStudents.Columns.AddRange(new DataGridViewColumn[] { clmFirstName, clmMiddleName, clmLastName, clmSuffix, clmDOB, clmPersonalEmail, clmPhone, clmAddress1, clmAddress2, clmRegion, clmProvince, clmCity, clmBarangay, clmPostalCode, clmProgram });
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvStudents.DefaultCellStyle = dataGridViewCellStyle1;
            dgvStudents.Location = new Point(6, 8);
            dgvStudents.Name = "dgvStudents";
            dgvStudents.RowHeadersVisible = false;
            dgvStudents.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvStudents.RowsDefaultCellStyle = dataGridViewCellStyle2;
            dgvStudents.Size = new Size(1324, 340);
            dgvStudents.TabIndex = 0;
            // 
            // clmFirstName
            // 
            clmFirstName.DataPropertyName = "FirstName";
            clmFirstName.HeaderText = "First Name";
            clmFirstName.Name = "clmFirstName";
            // 
            // clmMiddleName
            // 
            clmMiddleName.DataPropertyName = "MiddleName";
            clmMiddleName.HeaderText = "Middle Name";
            clmMiddleName.Name = "clmMiddleName";
            // 
            // clmLastName
            // 
            clmLastName.DataPropertyName = "LastName";
            clmLastName.HeaderText = "Last Name";
            clmLastName.Name = "clmLastName";
            // 
            // clmSuffix
            // 
            clmSuffix.DataPropertyName = "Suffix";
            clmSuffix.HeaderText = "Suffix";
            clmSuffix.Name = "clmSuffix";
            // 
            // clmDOB
            // 
            clmDOB.DataPropertyName = "DateOfBirth";
            clmDOB.HeaderText = "Date of birth";
            clmDOB.Name = "clmDOB";
            // 
            // clmPersonalEmail
            // 
            clmPersonalEmail.DataPropertyName = "Email";
            clmPersonalEmail.HeaderText = "Email";
            clmPersonalEmail.Name = "clmPersonalEmail";
            // 
            // clmPhone
            // 
            clmPhone.DataPropertyName = "Phone";
            clmPhone.HeaderText = "Phone";
            clmPhone.Name = "clmPhone";
            // 
            // clmAddress1
            // 
            clmAddress1.DataPropertyName = "Address1";
            clmAddress1.HeaderText = "Address 1";
            clmAddress1.Name = "clmAddress1";
            // 
            // clmAddress2
            // 
            clmAddress2.DataPropertyName = "Address2";
            clmAddress2.HeaderText = "Address 2";
            clmAddress2.Name = "clmAddress2";
            // 
            // clmRegion
            // 
            clmRegion.DataPropertyName = "Region";
            clmRegion.HeaderText = "Region";
            clmRegion.Name = "clmRegion";
            // 
            // clmProvince
            // 
            clmProvince.DataPropertyName = "Province";
            clmProvince.HeaderText = "Province";
            clmProvince.Name = "clmProvince";
            // 
            // clmCity
            // 
            clmCity.DataPropertyName = "City";
            clmCity.HeaderText = "City";
            clmCity.Name = "clmCity";
            // 
            // clmBarangay
            // 
            clmBarangay.DataPropertyName = "Barangay";
            clmBarangay.HeaderText = "Barangay";
            clmBarangay.Name = "clmBarangay";
            // 
            // clmPostalCode
            // 
            clmPostalCode.DataPropertyName = "PostalCode";
            clmPostalCode.HeaderText = "Postal Code";
            clmPostalCode.Name = "clmPostalCode";
            // 
            // clmProgram
            // 
            clmProgram.DataPropertyName = "Program";
            clmProgram.HeaderText = "Program";
            clmProgram.Name = "clmProgram";
            // 
            // btnRegisterAll
            // 
            btnRegisterAll.BackColor = Color.Maroon;
            btnRegisterAll.FlatAppearance.BorderColor = Color.Maroon;
            btnRegisterAll.FlatStyle = FlatStyle.Flat;
            btnRegisterAll.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRegisterAll.ForeColor = Color.White;
            btnRegisterAll.Image = Properties.Resources.student_2_161;
            btnRegisterAll.Location = new Point(1088, 366);
            btnRegisterAll.Name = "btnRegisterAll";
            btnRegisterAll.Size = new Size(239, 37);
            btnRegisterAll.TabIndex = 61;
            btnRegisterAll.Text = "Register All Students";
            btnRegisterAll.TextAlign = ContentAlignment.MiddleRight;
            btnRegisterAll.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnRegisterAll.UseVisualStyleBackColor = false;
            btnRegisterAll.Click += btnRegisterAll_Click;
            // 
            // btnImport
            // 
            btnImport.FlatAppearance.BorderColor = Color.Gray;
            btnImport.FlatStyle = FlatStyle.Flat;
            btnImport.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnImport.Location = new Point(932, 366);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(147, 37);
            btnImport.TabIndex = 62;
            btnImport.Text = "Import From CSV";
            btnImport.UseVisualStyleBackColor = true;
            btnImport.Click += btnImport_Click;
            // 
            // btnClearTable
            // 
            btnClearTable.FlatAppearance.BorderColor = Color.Gray;
            btnClearTable.FlatStyle = FlatStyle.Flat;
            btnClearTable.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClearTable.Location = new Point(776, 366);
            btnClearTable.Name = "btnClearTable";
            btnClearTable.Size = new Size(147, 37);
            btnClearTable.TabIndex = 63;
            btnClearTable.Text = "Clear Table";
            btnClearTable.UseVisualStyleBackColor = true;
            btnClearTable.Click += btnClearTable_Click;
            // 
            // BulkStudentRegistration
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1332, 420);
            Controls.Add(btnClearTable);
            Controls.Add(btnImport);
            Controls.Add(btnRegisterAll);
            Controls.Add(dgvStudents);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "BulkStudentRegistration";
            Text = "Bulk Student Registration";
            ((System.ComponentModel.ISupportInitialize)dgvStudents).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvStudents;
        private Button btnRegisterAll;
        private Button btnImport;
        private Button btnClearTable;
        private DataGridViewTextBoxColumn clmFirstName;
        private DataGridViewTextBoxColumn clmMiddleName;
        private DataGridViewTextBoxColumn clmLastName;
        private DataGridViewTextBoxColumn clmSuffix;
        private DataGridViewTextBoxColumn clmDOB;
        private DataGridViewTextBoxColumn clmPersonalEmail;
        private DataGridViewTextBoxColumn clmPhone;
        private DataGridViewTextBoxColumn clmAddress1;
        private DataGridViewTextBoxColumn clmAddress2;
        private DataGridViewTextBoxColumn clmRegion;
        private DataGridViewTextBoxColumn clmProvince;
        private DataGridViewTextBoxColumn clmCity;
        private DataGridViewTextBoxColumn clmBarangay;
        private DataGridViewTextBoxColumn clmPostalCode;
        private DataGridViewTextBoxColumn clmProgram;
    }
}