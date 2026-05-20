namespace PUPAcadPortal.PortalContents.Admin
{
    partial class EnrolledStudentsContentAdmin
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
            pnlEnrolledStudentsContent = new Panel();
            pnlESGraduatedCard = new Panel();
            pictureBox18 = new PictureBox();
            label51 = new Label();
            lblESGraduatedValue = new Label();
            pnlERStudentListCOntainer = new Panel();
            label57 = new Label();
            pnlESFilterContainer = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel3 = new Panel();
            label56 = new Label();
            cmbESYear = new ComboBox();
            panel2 = new Panel();
            label55 = new Label();
            cmbESEnrollmentStatus = new ComboBox();
            panel1 = new Panel();
            label54 = new Label();
            txtESSearchStudents = new TextBox();
            label53 = new Label();
            pictureBox9 = new PictureBox();
            pnlESInactiveCard = new Panel();
            pictureBox21 = new PictureBox();
            label48 = new Label();
            lblESInactiveValue = new Label();
            pnlESActiveCard = new Panel();
            pictureBox20 = new PictureBox();
            label46 = new Label();
            lblESActiveValue = new Label();
            pnlESTotalStudentsCard = new Panel();
            pictureBox19 = new PictureBox();
            label45 = new Label();
            lblESTotalStudentsValue = new Label();
            lblEnrolledStudents = new Label();
            lblEnrolledStudentDesc = new Label();
            pictureBox8 = new PictureBox();
            pnlEnrolledStudentsContent.SuspendLayout();
            pnlESGraduatedCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox18).BeginInit();
            pnlERStudentListCOntainer.SuspendLayout();
            pnlESFilterContainer.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox9).BeginInit();
            pnlESInactiveCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox21).BeginInit();
            pnlESActiveCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox20).BeginInit();
            pnlESTotalStudentsCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox19).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).BeginInit();
            SuspendLayout();
            // 
            // pnlEnrolledStudentsContent
            // 
            pnlEnrolledStudentsContent.AutoScroll = true;
            pnlEnrolledStudentsContent.Controls.Add(pnlESGraduatedCard);
            pnlEnrolledStudentsContent.Controls.Add(pnlERStudentListCOntainer);
            pnlEnrolledStudentsContent.Controls.Add(pnlESFilterContainer);
            pnlEnrolledStudentsContent.Controls.Add(pnlESInactiveCard);
            pnlEnrolledStudentsContent.Controls.Add(pnlESActiveCard);
            pnlEnrolledStudentsContent.Controls.Add(pnlESTotalStudentsCard);
            pnlEnrolledStudentsContent.Controls.Add(lblEnrolledStudents);
            pnlEnrolledStudentsContent.Controls.Add(lblEnrolledStudentDesc);
            pnlEnrolledStudentsContent.Controls.Add(pictureBox8);
            pnlEnrolledStudentsContent.Dock = DockStyle.Fill;
            pnlEnrolledStudentsContent.Location = new Point(0, 0);
            pnlEnrolledStudentsContent.Name = "pnlEnrolledStudentsContent";
            pnlEnrolledStudentsContent.Size = new Size(1254, 719);
            pnlEnrolledStudentsContent.TabIndex = 12;
            // 
            // pnlESGraduatedCard
            // 
            pnlESGraduatedCard.BackColor = Color.White;
            pnlESGraduatedCard.BorderStyle = BorderStyle.Fixed3D;
            pnlESGraduatedCard.Controls.Add(pictureBox18);
            pnlESGraduatedCard.Controls.Add(label51);
            pnlESGraduatedCard.Controls.Add(lblESGraduatedValue);
            pnlESGraduatedCard.Location = new Point(948, 128);
            pnlESGraduatedCard.Margin = new Padding(3, 2, 3, 2);
            pnlESGraduatedCard.Name = "pnlESGraduatedCard";
            pnlESGraduatedCard.Size = new Size(280, 95);
            pnlESGraduatedCard.TabIndex = 14;
            // 
            // pictureBox18
            // 
            pictureBox18.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox18.BackColor = Color.Maroon;
            pictureBox18.Image = Properties.Resources.graduation_cap_32;
            pictureBox18.Location = new Point(196, 8);
            pictureBox18.Name = "pictureBox18";
            pictureBox18.Size = new Size(66, 74);
            pictureBox18.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox18.TabIndex = 22;
            pictureBox18.TabStop = false;
            // 
            // label51
            // 
            label51.AutoSize = true;
            label51.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label51.ForeColor = Color.Maroon;
            label51.Location = new Point(3, 8);
            label51.Name = "label51";
            label51.Size = new Size(90, 21);
            label51.TabIndex = 16;
            label51.Text = "Graduated";
            // 
            // lblESGraduatedValue
            // 
            lblESGraduatedValue.AutoSize = true;
            lblESGraduatedValue.BackColor = Color.Transparent;
            lblESGraduatedValue.Font = new Font("Segoe UI", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblESGraduatedValue.ForeColor = Color.Black;
            lblESGraduatedValue.Location = new Point(3, 22);
            lblESGraduatedValue.Name = "lblESGraduatedValue";
            lblESGraduatedValue.Size = new Size(56, 65);
            lblESGraduatedValue.TabIndex = 17;
            lblESGraduatedValue.Text = "0";
            // 
            // pnlERStudentListCOntainer
            // 
            pnlERStudentListCOntainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlERStudentListCOntainer.BackColor = Color.White;
            pnlERStudentListCOntainer.Controls.Add(label57);
            pnlERStudentListCOntainer.Location = new Point(31, 415);
            pnlERStudentListCOntainer.Margin = new Padding(3, 2, 3, 2);
            pnlERStudentListCOntainer.Name = "pnlERStudentListCOntainer";
            pnlERStudentListCOntainer.Size = new Size(1212, 304);
            pnlERStudentListCOntainer.TabIndex = 16;
            // 
            // label57
            // 
            label57.AutoSize = true;
            label57.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label57.ForeColor = Color.Black;
            label57.Location = new Point(22, 20);
            label57.Name = "label57";
            label57.Size = new Size(179, 32);
            label57.TabIndex = 17;
            label57.Text = "Student List (0)";
            // 
            // pnlESFilterContainer
            // 
            pnlESFilterContainer.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlESFilterContainer.BackColor = Color.White;
            pnlESFilterContainer.Controls.Add(tableLayoutPanel1);
            pnlESFilterContainer.Controls.Add(label53);
            pnlESFilterContainer.Controls.Add(pictureBox9);
            pnlESFilterContainer.Location = new Point(32, 250);
            pnlESFilterContainer.Margin = new Padding(3, 2, 3, 2);
            pnlESFilterContainer.Name = "pnlESFilterContainer";
            pnlESFilterContainer.Size = new Size(1213, 139);
            pnlESFilterContainer.TabIndex = 15;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Controls.Add(panel3, 2, 0);
            tableLayoutPanel1.Controls.Add(panel2, 1, 0);
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Location = new Point(6, 58);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(1200, 56);
            tableLayoutPanel1.TabIndex = 23;
            // 
            // panel3
            // 
            panel3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel3.Controls.Add(label56);
            panel3.Controls.Add(cmbESYear);
            panel3.Location = new Point(803, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(394, 50);
            panel3.TabIndex = 25;
            // 
            // label56
            // 
            label56.AutoSize = true;
            label56.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label56.ForeColor = Color.DimGray;
            label56.Location = new Point(1, 1);
            label56.Name = "label56";
            label56.Size = new Size(43, 21);
            label56.TabIndex = 21;
            label56.Text = "Year";
            // 
            // cmbESYear
            // 
            cmbESYear.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbESYear.FormattingEnabled = true;
            cmbESYear.Location = new Point(2, 26);
            cmbESYear.Margin = new Padding(3, 2, 3, 2);
            cmbESYear.Name = "cmbESYear";
            cmbESYear.Size = new Size(390, 23);
            cmbESYear.TabIndex = 22;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel2.Controls.Add(label55);
            panel2.Controls.Add(cmbESEnrollmentStatus);
            panel2.Location = new Point(403, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(394, 50);
            panel2.TabIndex = 24;
            // 
            // label55
            // 
            label55.AutoSize = true;
            label55.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label55.ForeColor = Color.DimGray;
            label55.Location = new Point(1, 1);
            label55.Name = "label55";
            label55.Size = new Size(146, 21);
            label55.TabIndex = 19;
            label55.Text = "Enrollment Status";
            // 
            // cmbESEnrollmentStatus
            // 
            cmbESEnrollmentStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbESEnrollmentStatus.FormattingEnabled = true;
            cmbESEnrollmentStatus.Items.AddRange(new object[] { "All Stasuses", "Active", "Inactive", "Graduated ", "Withdrawn" });
            cmbESEnrollmentStatus.Location = new Point(2, 26);
            cmbESEnrollmentStatus.Margin = new Padding(3, 2, 3, 2);
            cmbESEnrollmentStatus.Name = "cmbESEnrollmentStatus";
            cmbESEnrollmentStatus.Size = new Size(390, 23);
            cmbESEnrollmentStatus.TabIndex = 20;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(label54);
            panel1.Controls.Add(txtESSearchStudents);
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(394, 50);
            panel1.TabIndex = 23;
            // 
            // label54
            // 
            label54.AutoSize = true;
            label54.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label54.ForeColor = Color.DimGray;
            label54.Location = new Point(1, 1);
            label54.Name = "label54";
            label54.Size = new Size(132, 21);
            label54.TabIndex = 17;
            label54.Text = "Search Students";
            // 
            // txtESSearchStudents
            // 
            txtESSearchStudents.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtESSearchStudents.BorderStyle = BorderStyle.FixedSingle;
            txtESSearchStudents.Location = new Point(1, 25);
            txtESSearchStudents.Margin = new Padding(3, 2, 3, 2);
            txtESSearchStudents.Name = "txtESSearchStudents";
            txtESSearchStudents.PlaceholderText = "Search by name, ID, or email...";
            txtESSearchStudents.Size = new Size(392, 23);
            txtESSearchStudents.TabIndex = 18;
            // 
            // label53
            // 
            label53.AutoSize = true;
            label53.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label53.ForeColor = Color.Black;
            label53.Location = new Point(54, 17);
            label53.Name = "label53";
            label53.Size = new Size(79, 32);
            label53.TabIndex = 16;
            label53.Text = "Filters";
            // 
            // pictureBox9
            // 
            pictureBox9.Image = Properties.Resources.empty_filter_24;
            pictureBox9.Location = new Point(12, 10);
            pictureBox9.Margin = new Padding(3, 2, 3, 2);
            pictureBox9.Name = "pictureBox9";
            pictureBox9.Size = new Size(54, 46);
            pictureBox9.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox9.TabIndex = 0;
            pictureBox9.TabStop = false;
            // 
            // pnlESInactiveCard
            // 
            pnlESInactiveCard.BackColor = Color.White;
            pnlESInactiveCard.BorderStyle = BorderStyle.Fixed3D;
            pnlESInactiveCard.Controls.Add(pictureBox21);
            pnlESInactiveCard.Controls.Add(label48);
            pnlESInactiveCard.Controls.Add(lblESInactiveValue);
            pnlESInactiveCard.Location = new Point(637, 128);
            pnlESInactiveCard.Margin = new Padding(3, 2, 3, 2);
            pnlESInactiveCard.Name = "pnlESInactiveCard";
            pnlESInactiveCard.Size = new Size(280, 95);
            pnlESInactiveCard.TabIndex = 13;
            // 
            // pictureBox21
            // 
            pictureBox21.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox21.BackColor = Color.Maroon;
            pictureBox21.Image = Properties.Resources.x_mark_3_32;
            pictureBox21.Location = new Point(201, 8);
            pictureBox21.Name = "pictureBox21";
            pictureBox21.Size = new Size(66, 74);
            pictureBox21.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox21.TabIndex = 24;
            pictureBox21.TabStop = false;
            // 
            // label48
            // 
            label48.AutoSize = true;
            label48.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label48.ForeColor = Color.Maroon;
            label48.Location = new Point(3, 8);
            label48.Name = "label48";
            label48.Size = new Size(71, 21);
            label48.TabIndex = 16;
            label48.Text = "Inactive";
            // 
            // lblESInactiveValue
            // 
            lblESInactiveValue.AutoSize = true;
            lblESInactiveValue.BackColor = Color.Transparent;
            lblESInactiveValue.Font = new Font("Segoe UI", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblESInactiveValue.ForeColor = Color.Black;
            lblESInactiveValue.Location = new Point(3, 22);
            lblESInactiveValue.Name = "lblESInactiveValue";
            lblESInactiveValue.Size = new Size(56, 65);
            lblESInactiveValue.TabIndex = 17;
            lblESInactiveValue.Text = "0";
            // 
            // pnlESActiveCard
            // 
            pnlESActiveCard.BackColor = Color.White;
            pnlESActiveCard.BorderStyle = BorderStyle.Fixed3D;
            pnlESActiveCard.Controls.Add(pictureBox20);
            pnlESActiveCard.Controls.Add(label46);
            pnlESActiveCard.Controls.Add(lblESActiveValue);
            pnlESActiveCard.Location = new Point(333, 128);
            pnlESActiveCard.Margin = new Padding(3, 2, 3, 2);
            pnlESActiveCard.Name = "pnlESActiveCard";
            pnlESActiveCard.Size = new Size(280, 95);
            pnlESActiveCard.TabIndex = 12;
            // 
            // pictureBox20
            // 
            pictureBox20.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox20.BackColor = Color.Maroon;
            pictureBox20.Image = Properties.Resources.ok_32;
            pictureBox20.Location = new Point(202, 7);
            pictureBox20.Name = "pictureBox20";
            pictureBox20.Size = new Size(66, 74);
            pictureBox20.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox20.TabIndex = 24;
            pictureBox20.TabStop = false;
            // 
            // label46
            // 
            label46.AutoSize = true;
            label46.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label46.ForeColor = Color.Maroon;
            label46.Location = new Point(3, 8);
            label46.Name = "label46";
            label46.Size = new Size(58, 21);
            label46.TabIndex = 16;
            label46.Text = "Active";
            // 
            // lblESActiveValue
            // 
            lblESActiveValue.AutoSize = true;
            lblESActiveValue.BackColor = Color.Transparent;
            lblESActiveValue.Font = new Font("Segoe UI", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblESActiveValue.ForeColor = Color.Black;
            lblESActiveValue.Location = new Point(3, 22);
            lblESActiveValue.Name = "lblESActiveValue";
            lblESActiveValue.Size = new Size(56, 65);
            lblESActiveValue.TabIndex = 17;
            lblESActiveValue.Text = "0";
            // 
            // pnlESTotalStudentsCard
            // 
            pnlESTotalStudentsCard.BackColor = Color.White;
            pnlESTotalStudentsCard.BorderStyle = BorderStyle.Fixed3D;
            pnlESTotalStudentsCard.Controls.Add(pictureBox19);
            pnlESTotalStudentsCard.Controls.Add(label45);
            pnlESTotalStudentsCard.Controls.Add(lblESTotalStudentsValue);
            pnlESTotalStudentsCard.Location = new Point(32, 128);
            pnlESTotalStudentsCard.Margin = new Padding(3, 2, 3, 2);
            pnlESTotalStudentsCard.Name = "pnlESTotalStudentsCard";
            pnlESTotalStudentsCard.Size = new Size(280, 95);
            pnlESTotalStudentsCard.TabIndex = 11;
            // 
            // pictureBox19
            // 
            pictureBox19.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox19.BackColor = Color.Maroon;
            pictureBox19.Image = Properties.Resources.students_32;
            pictureBox19.Location = new Point(195, 8);
            pictureBox19.Name = "pictureBox19";
            pictureBox19.Size = new Size(66, 74);
            pictureBox19.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox19.TabIndex = 23;
            pictureBox19.TabStop = false;
            // 
            // label45
            // 
            label45.AutoSize = true;
            label45.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label45.ForeColor = Color.Maroon;
            label45.Location = new Point(3, 8);
            label45.Name = "label45";
            label45.Size = new Size(119, 21);
            label45.TabIndex = 15;
            label45.Text = "Total Students";
            // 
            // lblESTotalStudentsValue
            // 
            lblESTotalStudentsValue.AutoSize = true;
            lblESTotalStudentsValue.BackColor = Color.Transparent;
            lblESTotalStudentsValue.Font = new Font("Segoe UI", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblESTotalStudentsValue.ForeColor = Color.Black;
            lblESTotalStudentsValue.Location = new Point(3, 22);
            lblESTotalStudentsValue.Name = "lblESTotalStudentsValue";
            lblESTotalStudentsValue.Size = new Size(56, 65);
            lblESTotalStudentsValue.TabIndex = 15;
            lblESTotalStudentsValue.Text = "0";
            // 
            // lblEnrolledStudents
            // 
            lblEnrolledStudents.AutoSize = true;
            lblEnrolledStudents.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEnrolledStudents.ForeColor = Color.Black;
            lblEnrolledStudents.Location = new Point(94, 37);
            lblEnrolledStudents.Name = "lblEnrolledStudents";
            lblEnrolledStudents.Size = new Size(258, 40);
            lblEnrolledStudents.TabIndex = 10;
            lblEnrolledStudents.Text = "Enrolled Students";
            // 
            // lblEnrolledStudentDesc
            // 
            lblEnrolledStudentDesc.AutoSize = true;
            lblEnrolledStudentDesc.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblEnrolledStudentDesc.ForeColor = Color.DimGray;
            lblEnrolledStudentDesc.Location = new Point(102, 79);
            lblEnrolledStudentDesc.Name = "lblEnrolledStudentDesc";
            lblEnrolledStudentDesc.Size = new Size(221, 19);
            lblEnrolledStudentDesc.TabIndex = 9;
            lblEnrolledStudentDesc.Text = "Complete list of enrolled students";
            // 
            // pictureBox8
            // 
            pictureBox8.BackColor = Color.Maroon;
            pictureBox8.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox8.BorderStyle = BorderStyle.FixedSingle;
            pictureBox8.Image = Properties.Resources.enrolledstudents_32;
            pictureBox8.Location = new Point(32, 37);
            pictureBox8.Name = "pictureBox8";
            pictureBox8.Size = new Size(57, 60);
            pictureBox8.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox8.TabIndex = 5;
            pictureBox8.TabStop = false;
            // 
            // EnrolledStudentsContentAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlEnrolledStudentsContent);
            Name = "EnrolledStudentsContentAdmin";
            Size = new Size(1254, 719);
            pnlEnrolledStudentsContent.ResumeLayout(false);
            pnlEnrolledStudentsContent.PerformLayout();
            pnlESGraduatedCard.ResumeLayout(false);
            pnlESGraduatedCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox18).EndInit();
            pnlERStudentListCOntainer.ResumeLayout(false);
            pnlERStudentListCOntainer.PerformLayout();
            pnlESFilterContainer.ResumeLayout(false);
            pnlESFilterContainer.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox9).EndInit();
            pnlESInactiveCard.ResumeLayout(false);
            pnlESInactiveCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox21).EndInit();
            pnlESActiveCard.ResumeLayout(false);
            pnlESActiveCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox20).EndInit();
            pnlESTotalStudentsCard.ResumeLayout(false);
            pnlESTotalStudentsCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox19).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlEnrolledStudentsContent;
        private Panel pnlESGraduatedCard;
        private PictureBox pictureBox18;
        private Label label51;
        private Label lblESGraduatedValue;
        private Panel pnlERStudentListCOntainer;
        private Label label57;
        private Panel pnlESFilterContainer;
        private ComboBox cmbESYear;
        private Label label56;
        private ComboBox cmbESEnrollmentStatus;
        private Label label55;
        private TextBox txtESSearchStudents;
        private Label label54;
        private Label label53;
        private PictureBox pictureBox9;
        private Panel pnlESInactiveCard;
        private PictureBox pictureBox21;
        private Label label48;
        private Label lblESInactiveValue;
        private Panel pnlESActiveCard;
        private PictureBox pictureBox20;
        private Label label46;
        private Label lblESActiveValue;
        private Panel pnlESTotalStudentsCard;
        private PictureBox pictureBox19;
        private Label label45;
        private Label lblESTotalStudentsValue;
        private Label lblEnrolledStudents;
        private Label lblEnrolledStudentDesc;
        private PictureBox pictureBox8;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
    }
}
