namespace PUPAcadPortal.PortalContents.Admin
{
    partial class GradesMngContentAdmin
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
            pnlGradesManagementContent = new Panel();
            pnlGradesManagementContainer = new Panel();
            dgvGrades = new DataGridView();
            btnGMSearch2 = new Button();
            txtGMSsearchBar2 = new TextBox();
            label21 = new Label();
            label37 = new Label();
            pictureBox6 = new PictureBox();
            pnlGradesManagementContent.SuspendLayout();
            pnlGradesManagementContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvGrades).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).BeginInit();
            SuspendLayout();
            // 
            // pnlGradesManagementContent
            // 
            pnlGradesManagementContent.AutoScroll = true;
            pnlGradesManagementContent.BackColor = SystemColors.Control;
            pnlGradesManagementContent.Controls.Add(pnlGradesManagementContainer);
            pnlGradesManagementContent.Controls.Add(label21);
            pnlGradesManagementContent.Controls.Add(label37);
            pnlGradesManagementContent.Controls.Add(pictureBox6);
            pnlGradesManagementContent.Dock = DockStyle.Fill;
            pnlGradesManagementContent.Location = new Point(0, 0);
            pnlGradesManagementContent.Name = "pnlGradesManagementContent";
            pnlGradesManagementContent.Size = new Size(1272, 719);
            pnlGradesManagementContent.TabIndex = 10;
            // 
            // pnlGradesManagementContainer
            // 
            pnlGradesManagementContainer.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlGradesManagementContainer.AutoScroll = true;
            pnlGradesManagementContainer.BackColor = SystemColors.ControlLightLight;
            pnlGradesManagementContainer.BorderStyle = BorderStyle.FixedSingle;
            pnlGradesManagementContainer.Controls.Add(dgvGrades);
            pnlGradesManagementContainer.Controls.Add(btnGMSearch2);
            pnlGradesManagementContainer.Controls.Add(txtGMSsearchBar2);
            pnlGradesManagementContainer.Location = new Point(31, 109);
            pnlGradesManagementContainer.Name = "pnlGradesManagementContainer";
            pnlGradesManagementContainer.Size = new Size(1190, 461);
            pnlGradesManagementContainer.TabIndex = 13;
            // 
            // dgvGrades
            // 
            dgvGrades.AllowUserToAddRows = false;
            dgvGrades.AllowUserToDeleteRows = false;
            dgvGrades.AllowUserToResizeColumns = false;
            dgvGrades.AllowUserToResizeRows = false;
            dgvGrades.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvGrades.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvGrades.BackgroundColor = Color.White;
            dgvGrades.BorderStyle = BorderStyle.None;
            dgvGrades.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvGrades.Location = new Point(16, 54);
            dgvGrades.Name = "dgvGrades";
            dgvGrades.ReadOnly = true;
            dgvGrades.RowHeadersVisible = false;
            dgvGrades.RowHeadersWidth = 51;
            dgvGrades.Size = new Size(1138, 400);
            dgvGrades.TabIndex = 2;
            // 
            // btnGMSearch2
            // 
            btnGMSearch2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGMSearch2.BackColor = Color.Maroon;
            btnGMSearch2.FlatStyle = FlatStyle.Flat;
            btnGMSearch2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGMSearch2.ForeColor = Color.White;
            btnGMSearch2.Image = Properties.Resources.search_3_16;
            btnGMSearch2.Location = new Point(1032, 15);
            btnGMSearch2.Name = "btnGMSearch2";
            btnGMSearch2.Size = new Size(121, 33);
            btnGMSearch2.TabIndex = 1;
            btnGMSearch2.Text = "Search";
            btnGMSearch2.TextAlign = ContentAlignment.MiddleRight;
            btnGMSearch2.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnGMSearch2.UseVisualStyleBackColor = false;
            // 
            // txtGMSsearchBar2
            // 
            txtGMSsearchBar2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtGMSsearchBar2.BorderStyle = BorderStyle.FixedSingle;
            txtGMSsearchBar2.Font = new Font("Segoe UI", 12F);
            txtGMSsearchBar2.Location = new Point(19, 18);
            txtGMSsearchBar2.Name = "txtGMSsearchBar2";
            txtGMSsearchBar2.PlaceholderText = "Search by student name, ID, or subject...";
            txtGMSsearchBar2.Size = new Size(1002, 29);
            txtGMSsearchBar2.TabIndex = 0;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            label21.ForeColor = Color.DimGray;
            label21.Location = new Point(106, 65);
            label21.Name = "label21";
            label21.Size = new Size(216, 19);
            label21.TabIndex = 11;
            label21.Text = "View and manage student grades";
            // 
            // label37
            // 
            label37.AutoSize = true;
            label37.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label37.ForeColor = Color.Black;
            label37.Location = new Point(95, 25);
            label37.Name = "label37";
            label37.Size = new Size(305, 40);
            label37.TabIndex = 10;
            label37.Text = "Grades Management";
            // 
            // pictureBox6
            // 
            pictureBox6.BackColor = Color.Maroon;
            pictureBox6.Image = Properties.Resources.books_32;
            pictureBox6.Location = new Point(32, 25);
            pictureBox6.Name = "pictureBox6";
            pictureBox6.Size = new Size(57, 59);
            pictureBox6.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox6.TabIndex = 9;
            pictureBox6.TabStop = false;
            // 
            // AdminGradesMngContent
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlGradesManagementContent);
            Name = "AdminGradesMngContent";
            Size = new Size(1272, 719);
            Load += AdminGradesMngContent_Load;
            pnlGradesManagementContent.ResumeLayout(false);
            pnlGradesManagementContent.PerformLayout();
            pnlGradesManagementContainer.ResumeLayout(false);
            pnlGradesManagementContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvGrades).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlGradesManagementContent;
        private Panel pnlGradesManagementContainer;
        private DataGridView dgvGrades;
        private Button btnGMSearch2;
        private TextBox txtGMSsearchBar2;
        private Label label21;
        private Label label37;
        private PictureBox pictureBox6;
    }
}
