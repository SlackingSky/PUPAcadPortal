namespace PUPAcadPortal.PortalContents.Admin.Enrollment
{
    partial class ViewUsersContentAdmin
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            pnlViewAllUsersContent = new Panel();
            pnlViewSPsContent = new Panel();
            panel1 = new Panel();
            btnViewStudents = new Button();
            btnViewProf = new Button();
            pnlContainerdgvUsers = new Panel();
            dgvUsers = new DataGridView();
            colUserID = new DataGridViewTextBoxColumn();
            colUserName = new DataGridViewTextBoxColumn();
            colUserEmail = new DataGridViewTextBoxColumn();
            colUserProgram = new DataGridViewTextBoxColumn();
            colUserYear = new DataGridViewTextBoxColumn();
            colUserStatus = new DataGridViewTextBoxColumn();
            pnlSearchBarVAUs = new Panel();
            cmbProgram = new ComboBox();
            btnSearch = new Button();
            cmbYear = new ComboBox();
            txtSearchViewAUs = new TextBox();
            pnlUserTypeIndicator = new Panel();
            pictureBox3 = new PictureBox();
            lblViewDesc = new Label();
            lblViewAllUsers = new Label();
            pnlViewAllUsersContent.SuspendLayout();
            pnlViewSPsContent.SuspendLayout();
            panel1.SuspendLayout();
            pnlContainerdgvUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            pnlSearchBarVAUs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // pnlViewAllUsersContent
            // 
            pnlViewAllUsersContent.AutoScroll = true;
            pnlViewAllUsersContent.BackColor = SystemColors.ButtonFace;
            pnlViewAllUsersContent.Controls.Add(pnlViewSPsContent);
            pnlViewAllUsersContent.Controls.Add(pictureBox3);
            pnlViewAllUsersContent.Controls.Add(lblViewDesc);
            pnlViewAllUsersContent.Controls.Add(lblViewAllUsers);
            pnlViewAllUsersContent.Dock = DockStyle.Fill;
            pnlViewAllUsersContent.Location = new Point(0, 0);
            pnlViewAllUsersContent.Name = "pnlViewAllUsersContent";
            pnlViewAllUsersContent.Size = new Size(1254, 719);
            pnlViewAllUsersContent.TabIndex = 15;
            // 
            // pnlViewSPsContent
            // 
            pnlViewSPsContent.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlViewSPsContent.AutoScroll = true;
            pnlViewSPsContent.BackColor = Color.White;
            pnlViewSPsContent.BackgroundImageLayout = ImageLayout.Center;
            pnlViewSPsContent.BorderStyle = BorderStyle.FixedSingle;
            pnlViewSPsContent.Controls.Add(panel1);
            pnlViewSPsContent.Controls.Add(pnlContainerdgvUsers);
            pnlViewSPsContent.Controls.Add(pnlSearchBarVAUs);
            pnlViewSPsContent.Controls.Add(pnlUserTypeIndicator);
            pnlViewSPsContent.Location = new Point(14, 124);
            pnlViewSPsContent.Name = "pnlViewSPsContent";
            pnlViewSPsContent.Size = new Size(1215, 668);
            pnlViewSPsContent.TabIndex = 5;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnViewStudents);
            panel1.Controls.Add(btnViewProf);
            panel1.Location = new Point(34, 24);
            panel1.Name = "panel1";
            panel1.Size = new Size(524, 44);
            panel1.TabIndex = 17;
            // 
            // btnViewStudents
            // 
            btnViewStudents.BackColor = Color.Transparent;
            btnViewStudents.BackgroundImageLayout = ImageLayout.None;
            btnViewStudents.Cursor = Cursors.Hand;
            btnViewStudents.FlatAppearance.BorderSize = 0;
            btnViewStudents.FlatStyle = FlatStyle.Flat;
            btnViewStudents.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnViewStudents.Image = Properties.Resources.student_black_16;
            btnViewStudents.Location = new Point(4, 4);
            btnViewStudents.Name = "btnViewStudents";
            btnViewStudents.Size = new Size(247, 36);
            btnViewStudents.TabIndex = 9;
            btnViewStudents.Text = "Students";
            btnViewStudents.TextAlign = ContentAlignment.MiddleRight;
            btnViewStudents.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnViewStudents.UseVisualStyleBackColor = false;
            btnViewStudents.Click += btnViewStudents_Click;
            // 
            // btnViewProf
            // 
            btnViewProf.BackColor = Color.Transparent;
            btnViewProf.BackgroundImageLayout = ImageLayout.None;
            btnViewProf.Cursor = Cursors.Hand;
            btnViewProf.FlatAppearance.BorderSize = 0;
            btnViewProf.FlatStyle = FlatStyle.Flat;
            btnViewProf.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnViewProf.Image = Properties.Resources.professor_black_16;
            btnViewProf.Location = new Point(270, 4);
            btnViewProf.Name = "btnViewProf";
            btnViewProf.Size = new Size(247, 36);
            btnViewProf.TabIndex = 11;
            btnViewProf.Text = "Professors";
            btnViewProf.TextAlign = ContentAlignment.MiddleRight;
            btnViewProf.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnViewProf.UseVisualStyleBackColor = false;
            btnViewProf.Click += btnViewProf_Click;
            // 
            // pnlContainerdgvUsers
            // 
            pnlContainerdgvUsers.AutoScroll = true;
            pnlContainerdgvUsers.AutoSize = true;
            pnlContainerdgvUsers.Controls.Add(dgvUsers);
            pnlContainerdgvUsers.Location = new Point(37, 159);
            pnlContainerdgvUsers.Name = "pnlContainerdgvUsers";
            pnlContainerdgvUsers.Size = new Size(1144, 462);
            pnlContainerdgvUsers.TabIndex = 16;
            // 
            // dgvUsers
            // 
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.AllowUserToResizeColumns = false;
            dgvUsers.AllowUserToResizeRows = false;
            dgvUsers.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.BackgroundColor = Color.White;
            dgvUsers.BorderStyle = BorderStyle.None;
            dgvUsers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvUsers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.Maroon;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsers.Columns.AddRange(new DataGridViewColumn[] { colUserID, colUserName, colUserEmail, colUserProgram, colUserYear, colUserStatus });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(197, 202, 233);
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvUsers.DefaultCellStyle = dataGridViewCellStyle2;
            dgvUsers.EnableHeadersVisualStyles = false;
            dgvUsers.GridColor = Color.FromArgb(220, 220, 220);
            dgvUsers.Location = new Point(0, 0);
            dgvUsers.Name = "dgvUsers";
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.RowHeadersWidth = 51;
            dgvUsers.RowTemplate.Height = 40;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvUsers.Size = new Size(1141, 459);
            dgvUsers.TabIndex = 15;
            // 
            // colUserID
            // 
            colUserID.HeaderText = "ID";
            colUserID.MinimumWidth = 6;
            colUserID.Name = "colUserID";
            colUserID.ReadOnly = true;
            // 
            // colUserName
            // 
            colUserName.HeaderText = "Name";
            colUserName.MinimumWidth = 6;
            colUserName.Name = "colUserName";
            colUserName.ReadOnly = true;
            // 
            // colUserEmail
            // 
            colUserEmail.HeaderText = "Email";
            colUserEmail.MinimumWidth = 6;
            colUserEmail.Name = "colUserEmail";
            colUserEmail.ReadOnly = true;
            // 
            // colUserProgram
            // 
            colUserProgram.HeaderText = "Program";
            colUserProgram.MinimumWidth = 6;
            colUserProgram.Name = "colUserProgram";
            colUserProgram.ReadOnly = true;
            // 
            // colUserYear
            // 
            colUserYear.HeaderText = "Year";
            colUserYear.MinimumWidth = 6;
            colUserYear.Name = "colUserYear";
            colUserYear.ReadOnly = true;
            // 
            // colUserStatus
            // 
            colUserStatus.HeaderText = "Status";
            colUserStatus.MinimumWidth = 6;
            colUserStatus.Name = "colUserStatus";
            colUserStatus.ReadOnly = true;
            // 
            // pnlSearchBarVAUs
            // 
            pnlSearchBarVAUs.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlSearchBarVAUs.BackColor = SystemColors.ControlLight;
            pnlSearchBarVAUs.Controls.Add(cmbProgram);
            pnlSearchBarVAUs.Controls.Add(btnSearch);
            pnlSearchBarVAUs.Controls.Add(cmbYear);
            pnlSearchBarVAUs.Controls.Add(txtSearchViewAUs);
            pnlSearchBarVAUs.Location = new Point(35, 90);
            pnlSearchBarVAUs.Name = "pnlSearchBarVAUs";
            pnlSearchBarVAUs.Size = new Size(1143, 47);
            pnlSearchBarVAUs.TabIndex = 14;
            // 
            // cmbProgram
            // 
            cmbProgram.FormattingEnabled = true;
            cmbProgram.Items.AddRange(new object[] { "All", "BSIT", "BSHM", "BSED-M", "BSED-E", "BSCpE" });
            cmbProgram.Location = new Point(77, 15);
            cmbProgram.Name = "cmbProgram";
            cmbProgram.Size = new Size(73, 23);
            cmbProgram.TabIndex = 14;
            cmbProgram.Text = "Program";
            // 
            // btnSearch
            // 
            btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSearch.BackColor = Color.Maroon;
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSearch.ForeColor = SystemColors.ControlLightLight;
            btnSearch.Location = new Point(1047, 8);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(82, 32);
            btnSearch.TabIndex = 13;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = false;
            // 
            // cmbYear
            // 
            cmbYear.FormattingEnabled = true;
            cmbYear.Items.AddRange(new object[] { "All", "1st Year", "2nd Year", "3rd Year", "4th Year" });
            cmbYear.Location = new Point(12, 15);
            cmbYear.Name = "cmbYear";
            cmbYear.Size = new Size(59, 23);
            cmbYear.TabIndex = 13;
            cmbYear.Text = "Year";
            // 
            // txtSearchViewAUs
            // 
            txtSearchViewAUs.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSearchViewAUs.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearchViewAUs.Location = new Point(156, 13);
            txtSearchViewAUs.Name = "txtSearchViewAUs";
            txtSearchViewAUs.Size = new Size(885, 25);
            txtSearchViewAUs.TabIndex = 12;
            txtSearchViewAUs.Text = "Search here...";
            txtSearchViewAUs.TextChanged += txtSearchViewAUs_TextChanged;
            // 
            // pnlUserTypeIndicator
            // 
            pnlUserTypeIndicator.BackColor = Color.Maroon;
            pnlUserTypeIndicator.Location = new Point(35, 70);
            pnlUserTypeIndicator.Margin = new Padding(0);
            pnlUserTypeIndicator.Name = "pnlUserTypeIndicator";
            pnlUserTypeIndicator.Size = new Size(247, 4);
            pnlUserTypeIndicator.TabIndex = 8;
            pnlUserTypeIndicator.Visible = false;
            // 
            // pictureBox3
            // 
            pictureBox3.BackColor = Color.Maroon;
            pictureBox3.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox3.BorderStyle = BorderStyle.FixedSingle;
            pictureBox3.Image = Properties.Resources.visible_32;
            pictureBox3.Location = new Point(32, 37);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(57, 60);
            pictureBox3.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox3.TabIndex = 4;
            pictureBox3.TabStop = false;
            // 
            // lblViewDesc
            // 
            lblViewDesc.AutoSize = true;
            lblViewDesc.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblViewDesc.ForeColor = Color.DimGray;
            lblViewDesc.Location = new Point(106, 77);
            lblViewDesc.Name = "lblViewDesc";
            lblViewDesc.Size = new Size(211, 19);
            lblViewDesc.TabIndex = 3;
            lblViewDesc.Text = "Manage students and professors";
            // 
            // lblViewAllUsers
            // 
            lblViewAllUsers.AutoSize = true;
            lblViewAllUsers.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblViewAllUsers.ForeColor = Color.Black;
            lblViewAllUsers.Location = new Point(95, 37);
            lblViewAllUsers.Name = "lblViewAllUsers";
            lblViewAllUsers.Size = new Size(210, 40);
            lblViewAllUsers.TabIndex = 1;
            lblViewAllUsers.Text = "View All Users";
            // 
            // ViewUsersContentAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlViewAllUsersContent);
            Name = "ViewUsersContentAdmin";
            Size = new Size(1254, 719);
            Load += ViewUsersContentAdmin_Load;
            pnlViewAllUsersContent.ResumeLayout(false);
            pnlViewAllUsersContent.PerformLayout();
            pnlViewSPsContent.ResumeLayout(false);
            pnlViewSPsContent.PerformLayout();
            panel1.ResumeLayout(false);
            pnlContainerdgvUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            pnlSearchBarVAUs.ResumeLayout(false);
            pnlSearchBarVAUs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlViewAllUsersContent;
        private Panel pnlViewSPsContent;
        private Panel panel1;
        private Button btnViewStudents;
        private Button btnViewProf;
        private Panel pnlContainerdgvUsers;
        private DataGridView dgvUsers;
        private DataGridViewTextBoxColumn colUserID;
        private DataGridViewTextBoxColumn colUserName;
        private DataGridViewTextBoxColumn colUserEmail;
        private DataGridViewTextBoxColumn colUserProgram;
        private DataGridViewTextBoxColumn colUserYear;
        private DataGridViewTextBoxColumn colUserStatus;
        private Panel pnlSearchBarVAUs;
        private ComboBox cmbProgram;
        private Button btnSearch;
        private ComboBox cmbYear;
        private TextBox txtSearchViewAUs;
        private Panel pnlUserTypeIndicator;
        private PictureBox pictureBox3;
        private Label lblViewDesc;
        private Label lblViewAllUsers;
    }
}
