namespace PUPAcadPortal.PortalContents.Admin.SubOffering
{
    partial class SubjectManagementContentAdmin
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            pnlHeader = new Panel();
            lblTitle = new Label();
            pnlAction = new Panel();
            cmbFilterDept = new ComboBox();
            txtSearch = new TextBox();
            lblWarning = new Label();
            btnDelete = new Button();
            btnEdit = new Button();
            btnAdd = new Button();
            dgvSubjects = new DataGridView();
            pnlHeader.SuspendLayout();
            pnlAction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSubjects).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(128, 0, 0);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(900, 70);
            pnlHeader.TabIndex = 2;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.Gold;
            lblTitle.Location = new Point(20, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(278, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Master Subject Catalog";
            // 
            // pnlAction
            // 
            pnlAction.BackColor = Color.WhiteSmoke;
            pnlAction.Controls.Add(cmbFilterDept);
            pnlAction.Controls.Add(txtSearch);
            pnlAction.Controls.Add(lblWarning);
            pnlAction.Controls.Add(btnDelete);
            pnlAction.Controls.Add(btnEdit);
            pnlAction.Controls.Add(btnAdd);
            pnlAction.Dock = DockStyle.Top;
            pnlAction.Location = new Point(0, 70);
            pnlAction.Name = "pnlAction";
            pnlAction.Size = new Size(900, 120);
            pnlAction.TabIndex = 1;
            // 
            // cmbFilterDept
            // 
            cmbFilterDept.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterDept.Font = new Font("Segoe UI", 11F);
            cmbFilterDept.Location = new Point(660, 70);
            cmbFilterDept.Name = "cmbFilterDept";
            cmbFilterDept.Size = new Size(200, 28);
            cmbFilterDept.TabIndex = 0;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 11F);
            txtSearch.Location = new Point(450, 70);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(200, 27);
            txtSearch.TabIndex = 1;
            // 
            // lblWarning
            // 
            lblWarning.AutoSize = true;
            lblWarning.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblWarning.ForeColor = Color.Maroon;
            lblWarning.Location = new Point(450, 20);
            lblWarning.Name = "lblWarning";
            lblWarning.Size = new Size(259, 20);
            lblWarning.TabIndex = 2;
            lblWarning.Text = "🔒 Catalog Locked: Semester Active.";
            lblWarning.Visible = false;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.Firebrick;
            btnDelete.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(300, 15);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(120, 45);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = false;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = Color.DimGray;
            btnEdit.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnEdit.ForeColor = Color.White;
            btnEdit.Location = new Point(170, 15);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(120, 45);
            btnEdit.TabIndex = 4;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = false;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.FromArgb(128, 0, 0);
            btnAdd.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(20, 15);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(140, 45);
            btnAdd.TabIndex = 5;
            btnAdd.Text = "+ Add Subject";
            btnAdd.UseVisualStyleBackColor = false;
            // 
            // dgvSubjects
            // 
            dgvSubjects.AllowUserToAddRows = false;
            dgvSubjects.AllowUserToDeleteRows = false;
            dgvSubjects.AllowUserToResizeColumns = false;
            dgvSubjects.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(245, 245, 245);
            dgvSubjects.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvSubjects.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(128, 0, 0);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dgvSubjects.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvSubjects.ColumnHeadersHeight = 45;
            dgvSubjects.Dock = DockStyle.Fill;
            dgvSubjects.EnableHeadersVisualStyles = false;
            dgvSubjects.Location = new Point(0, 190);
            dgvSubjects.Name = "dgvSubjects";
            dgvSubjects.ReadOnly = true;
            dgvSubjects.RowHeadersVisible = false;
            dgvSubjects.RowTemplate.Height = 40;
            dgvSubjects.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSubjects.Size = new Size(900, 410);
            dgvSubjects.TabIndex = 0;
            // 
            // SubjectManagementContentAdmin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(dgvSubjects);
            Controls.Add(pnlAction);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 11F);
            Name = "SubjectManagementContentAdmin";
            Size = new Size(900, 600);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlAction.ResumeLayout(false);
            pnlAction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSubjects).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlAction;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView dgvSubjects;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ComboBox cmbFilterDept;
    }
}