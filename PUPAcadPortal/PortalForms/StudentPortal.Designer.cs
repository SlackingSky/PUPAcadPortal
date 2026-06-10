namespace PUPAcadPortal.PortalForms
{
    partial class StudentPortal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StudentPortal));
            pnlSidebar = new Panel();
            userProfile1 = new PUPAcadPortal.PortalContents.Misc.UserProfile();
            panel12 = new Panel();
            panel13 = new Panel();
            btnLogout = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            panel7 = new Panel();
            pnlYellow = new Panel();
            btnDashboard = new Button();
            panel8 = new Panel();
            btnEnrollment = new Button();
            panel10 = new Panel();
            btnAccounts = new Button();
            panel11 = new Panel();
            btnLMS = new Button();
            pnllmsSubmenu = new Panel();
            flowLayoutPanel2 = new FlowLayoutPanel();
            btnAnnounce = new Button();
            btnCalendar = new Button();
            btnCourses = new Button();
            btnAttendance = new Button();
            btnGrade = new Button();
            pnlContainerStudentPortal = new Panel();
            mainContentPanel = new Panel();
            pnlHeader = new Panel();
            panel15 = new Panel();
            panel16 = new Panel();
            label3 = new Label();
            label4 = new Label();
            pictureBox2 = new PictureBox();
            pnlSidebar.SuspendLayout();
            panel12.SuspendLayout();
            panel13.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel7.SuspendLayout();
            panel8.SuspendLayout();
            panel10.SuspendLayout();
            panel11.SuspendLayout();
            pnllmsSubmenu.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            pnlContainerStudentPortal.SuspendLayout();
            pnlHeader.SuspendLayout();
            panel15.SuspendLayout();
            panel16.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // pnlSidebar
            // 
            pnlSidebar.BackColor = Color.Maroon;
            pnlSidebar.Controls.Add(userProfile1);
            pnlSidebar.Controls.Add(panel12);
            pnlSidebar.Controls.Add(flowLayoutPanel1);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Location = new Point(0, 0);
            pnlSidebar.Margin = new Padding(0);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Padding = new Padding(0, 72, 0, 45);
            pnlSidebar.Size = new Size(256, 759);
            pnlSidebar.TabIndex = 0;
            // 
            // userProfile1
            // 
            userProfile1.Dock = DockStyle.Top;
            userProfile1.Location = new Point(0, 72);
            userProfile1.Margin = new Padding(3, 4, 3, 4);
            userProfile1.Name = "userProfile1";
            userProfile1.Size = new Size(256, 73);
            userProfile1.TabIndex = 2;
            // 
            // panel12
            // 
            panel12.BackColor = Color.FromArgb(30, 109, 0, 0);
            panel12.Controls.Add(panel13);
            panel12.Dock = DockStyle.Bottom;
            panel12.Location = new Point(0, 633);
            panel12.Margin = new Padding(0);
            panel12.Name = "panel12";
            panel12.Size = new Size(256, 81);
            panel12.TabIndex = 0;
            // 
            // panel13
            // 
            panel13.Controls.Add(btnLogout);
            panel13.Location = new Point(16, 16);
            panel13.Margin = new Padding(0);
            panel13.Name = "panel13";
            panel13.Size = new Size(224, 48);
            panel13.TabIndex = 4;
            // 
            // btnLogout
            // 
            btnLogout.BackgroundImageLayout = ImageLayout.None;
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseDownBackColor = Color.FromArgb(109, 0, 0);
            btnLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(109, 0, 0);
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            btnLogout.ForeColor = Color.White;
            btnLogout.Image = Properties.Resources.LogOut;
            btnLogout.ImageAlign = ContentAlignment.MiddleLeft;
            btnLogout.Location = new Point(0, 0);
            btnLogout.Name = "btnLogout";
            btnLogout.Padding = new Padding(16, 0, 0, 0);
            btnLogout.Size = new Size(224, 48);
            btnLogout.TabIndex = 3;
            btnLogout.Text = " Logout";
            btnLogout.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnLogout.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.BackColor = Color.Transparent;
            flowLayoutPanel1.Controls.Add(panel7);
            flowLayoutPanel1.Controls.Add(panel8);
            flowLayoutPanel1.Controls.Add(panel10);
            flowLayoutPanel1.Controls.Add(panel11);
            flowLayoutPanel1.Controls.Add(pnllmsSubmenu);
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(0, 145);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(0, 0, 0, 45);
            flowLayoutPanel1.Size = new Size(256, 468);
            flowLayoutPanel1.TabIndex = 1;
            flowLayoutPanel1.WrapContents = false;
            // 
            // panel7
            // 
            panel7.Controls.Add(pnlYellow);
            panel7.Controls.Add(btnDashboard);
            panel7.Location = new Point(0, 16);
            panel7.Margin = new Padding(0, 16, 0, 0);
            panel7.Name = "panel7";
            panel7.Size = new Size(256, 48);
            panel7.TabIndex = 0;
            // 
            // pnlYellow
            // 
            pnlYellow.BackColor = Color.FromArgb(255, 193, 7);
            pnlYellow.Location = new Point(0, 0);
            pnlYellow.Margin = new Padding(0);
            pnlYellow.Name = "pnlYellow";
            pnlYellow.Size = new Size(4, 48);
            pnlYellow.TabIndex = 6;
            pnlYellow.Visible = false;
            // 
            // btnDashboard
            // 
            btnDashboard.BackgroundImageLayout = ImageLayout.None;
            btnDashboard.Cursor = Cursors.Hand;
            btnDashboard.FlatAppearance.BorderSize = 0;
            btnDashboard.FlatAppearance.MouseDownBackColor = Color.FromArgb(109, 0, 0);
            btnDashboard.FlatAppearance.MouseOverBackColor = Color.FromArgb(109, 0, 0);
            btnDashboard.FlatStyle = FlatStyle.Flat;
            btnDashboard.Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            btnDashboard.ForeColor = Color.FromArgb(179, 255, 255, 255);
            btnDashboard.Image = Properties.Resources.item_icon;
            btnDashboard.ImageAlign = ContentAlignment.MiddleLeft;
            btnDashboard.Location = new Point(0, 0);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Padding = new Padding(16, 0, 0, 0);
            btnDashboard.Size = new Size(256, 48);
            btnDashboard.TabIndex = 3;
            btnDashboard.Text = " Dashboard";
            btnDashboard.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnDashboard.UseVisualStyleBackColor = true;
            btnDashboard.Click += btnDashboard_Click;
            // 
            // panel8
            // 
            panel8.Controls.Add(btnEnrollment);
            panel8.Location = new Point(0, 64);
            panel8.Margin = new Padding(0);
            panel8.Name = "panel8";
            panel8.Size = new Size(256, 48);
            panel8.TabIndex = 1;
            // 
            // btnEnrollment
            // 
            btnEnrollment.BackgroundImageLayout = ImageLayout.None;
            btnEnrollment.Cursor = Cursors.Hand;
            btnEnrollment.FlatAppearance.BorderSize = 0;
            btnEnrollment.FlatAppearance.MouseDownBackColor = Color.FromArgb(109, 0, 0);
            btnEnrollment.FlatAppearance.MouseOverBackColor = Color.FromArgb(109, 0, 0);
            btnEnrollment.FlatStyle = FlatStyle.Flat;
            btnEnrollment.Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            btnEnrollment.ForeColor = Color.FromArgb(179, 255, 255, 255);
            btnEnrollment.Image = Properties.Resources.Enrollment;
            btnEnrollment.ImageAlign = ContentAlignment.MiddleLeft;
            btnEnrollment.Location = new Point(0, 0);
            btnEnrollment.Name = "btnEnrollment";
            btnEnrollment.Padding = new Padding(16, 0, 0, 0);
            btnEnrollment.Size = new Size(256, 48);
            btnEnrollment.TabIndex = 3;
            btnEnrollment.Text = " My Enrollment";
            btnEnrollment.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnEnrollment.UseVisualStyleBackColor = true;
            btnEnrollment.Click += btnEnrollment_Click;
            // 
            // panel10
            // 
            panel10.Controls.Add(btnAccounts);
            panel10.Location = new Point(0, 112);
            panel10.Margin = new Padding(0);
            panel10.Name = "panel10";
            panel10.Size = new Size(256, 48);
            panel10.TabIndex = 3;
            // 
            // btnAccounts
            // 
            btnAccounts.BackgroundImageLayout = ImageLayout.None;
            btnAccounts.Cursor = Cursors.Hand;
            btnAccounts.FlatAppearance.BorderSize = 0;
            btnAccounts.FlatAppearance.MouseDownBackColor = Color.FromArgb(109, 0, 0);
            btnAccounts.FlatAppearance.MouseOverBackColor = Color.FromArgb(109, 0, 0);
            btnAccounts.FlatStyle = FlatStyle.Flat;
            btnAccounts.Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            btnAccounts.ForeColor = Color.FromArgb(179, 255, 255, 255);
            btnAccounts.Image = Properties.Resources.Accounts;
            btnAccounts.ImageAlign = ContentAlignment.MiddleLeft;
            btnAccounts.Location = new Point(0, 0);
            btnAccounts.Name = "btnAccounts";
            btnAccounts.Padding = new Padding(16, 0, 0, 0);
            btnAccounts.Size = new Size(256, 48);
            btnAccounts.TabIndex = 3;
            btnAccounts.Text = " Accounts";
            btnAccounts.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnAccounts.UseVisualStyleBackColor = true;
            btnAccounts.Click += btnAccounts_Click;
            // 
            // panel11
            // 
            panel11.Controls.Add(btnLMS);
            panel11.Location = new Point(0, 160);
            panel11.Margin = new Padding(0);
            panel11.Name = "panel11";
            panel11.Size = new Size(256, 48);
            panel11.TabIndex = 4;
            // 
            // btnLMS
            // 
            btnLMS.BackgroundImageLayout = ImageLayout.None;
            btnLMS.Cursor = Cursors.Hand;
            btnLMS.FlatAppearance.BorderSize = 0;
            btnLMS.FlatAppearance.MouseDownBackColor = Color.FromArgb(109, 0, 0);
            btnLMS.FlatAppearance.MouseOverBackColor = Color.FromArgb(109, 0, 0);
            btnLMS.FlatStyle = FlatStyle.Flat;
            btnLMS.Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            btnLMS.ForeColor = Color.FromArgb(179, 255, 255, 255);
            btnLMS.Image = Properties.Resources.LMS;
            btnLMS.ImageAlign = ContentAlignment.MiddleLeft;
            btnLMS.Location = new Point(0, 0);
            btnLMS.Name = "btnLMS";
            btnLMS.Padding = new Padding(16, 0, 0, 0);
            btnLMS.Size = new Size(256, 48);
            btnLMS.TabIndex = 3;
            btnLMS.Text = " LMS                                        ›";
            btnLMS.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnLMS.UseVisualStyleBackColor = true;
            btnLMS.Click += btnLMS_Click;
            // 
            // pnllmsSubmenu
            // 
            pnllmsSubmenu.BackColor = Color.FromArgb(128, 109, 0, 0);
            pnllmsSubmenu.Controls.Add(flowLayoutPanel2);
            pnllmsSubmenu.Location = new Point(0, 208);
            pnllmsSubmenu.Margin = new Padding(0);
            pnllmsSubmenu.Name = "pnllmsSubmenu";
            pnllmsSubmenu.Size = new Size(256, 281);
            pnllmsSubmenu.TabIndex = 6;
            pnllmsSubmenu.Visible = false;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Controls.Add(btnAnnounce);
            flowLayoutPanel2.Controls.Add(btnCalendar);
            flowLayoutPanel2.Controls.Add(btnCourses);
            flowLayoutPanel2.Controls.Add(btnAttendance);
            flowLayoutPanel2.Controls.Add(btnGrade);
            flowLayoutPanel2.Dock = DockStyle.Fill;
            flowLayoutPanel2.Location = new Point(0, 0);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(256, 281);
            flowLayoutPanel2.TabIndex = 5;
            // 
            // btnAnnounce
            // 
            btnAnnounce.BackgroundImageLayout = ImageLayout.None;
            btnAnnounce.Cursor = Cursors.Hand;
            btnAnnounce.FlatAppearance.BorderSize = 0;
            btnAnnounce.FlatAppearance.MouseDownBackColor = Color.FromArgb(109, 0, 0);
            btnAnnounce.FlatAppearance.MouseOverBackColor = Color.FromArgb(109, 0, 0);
            btnAnnounce.FlatStyle = FlatStyle.Flat;
            btnAnnounce.Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            btnAnnounce.ForeColor = Color.FromArgb(179, 255, 255, 255);
            btnAnnounce.Image = Properties.Resources.marketing;
            btnAnnounce.ImageAlign = ContentAlignment.MiddleLeft;
            btnAnnounce.Location = new Point(3, 3);
            btnAnnounce.Name = "btnAnnounce";
            btnAnnounce.Padding = new Padding(16, 0, 0, 0);
            btnAnnounce.Size = new Size(256, 48);
            btnAnnounce.TabIndex = 4;
            btnAnnounce.Text = " Announcements";
            btnAnnounce.TextAlign = ContentAlignment.MiddleLeft;
            btnAnnounce.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnAnnounce.UseVisualStyleBackColor = true;
            btnAnnounce.Click += btnAnnounce_Click;
            // 
            // btnCalendar
            // 
            btnCalendar.BackgroundImageLayout = ImageLayout.None;
            btnCalendar.Cursor = Cursors.Hand;
            btnCalendar.FlatAppearance.BorderSize = 0;
            btnCalendar.FlatAppearance.MouseDownBackColor = Color.FromArgb(109, 0, 0);
            btnCalendar.FlatAppearance.MouseOverBackColor = Color.FromArgb(109, 0, 0);
            btnCalendar.FlatStyle = FlatStyle.Flat;
            btnCalendar.Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            btnCalendar.ForeColor = Color.FromArgb(179, 255, 255, 255);
            btnCalendar.Image = Properties.Resources.calendar__4_;
            btnCalendar.ImageAlign = ContentAlignment.MiddleLeft;
            btnCalendar.Location = new Point(3, 57);
            btnCalendar.Name = "btnCalendar";
            btnCalendar.Padding = new Padding(16, 0, 0, 0);
            btnCalendar.Size = new Size(256, 48);
            btnCalendar.TabIndex = 7;
            btnCalendar.Text = " Calendar";
            btnCalendar.TextAlign = ContentAlignment.MiddleLeft;
            btnCalendar.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCalendar.UseVisualStyleBackColor = true;
            btnCalendar.Click += btnCalendar_Click;
            // 
            // btnCourses
            // 
            btnCourses.BackgroundImageLayout = ImageLayout.None;
            btnCourses.Cursor = Cursors.Hand;
            btnCourses.FlatAppearance.BorderSize = 0;
            btnCourses.FlatAppearance.MouseDownBackColor = Color.FromArgb(109, 0, 0);
            btnCourses.FlatAppearance.MouseOverBackColor = Color.FromArgb(109, 0, 0);
            btnCourses.FlatStyle = FlatStyle.Flat;
            btnCourses.Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            btnCourses.ForeColor = Color.FromArgb(179, 255, 255, 255);
            btnCourses.Image = (Image)resources.GetObject("btnCourses.Image");
            btnCourses.ImageAlign = ContentAlignment.MiddleLeft;
            btnCourses.Location = new Point(3, 111);
            btnCourses.Name = "btnCourses";
            btnCourses.Padding = new Padding(16, 0, 0, 0);
            btnCourses.Size = new Size(256, 48);
            btnCourses.TabIndex = 6;
            btnCourses.Text = "Courses";
            btnCourses.TextAlign = ContentAlignment.MiddleLeft;
            btnCourses.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCourses.UseVisualStyleBackColor = true;
            btnCourses.Click += btnCourses_Click;
            // 
            // btnAttendance
            // 
            btnAttendance.BackgroundImageLayout = ImageLayout.None;
            btnAttendance.Cursor = Cursors.Hand;
            btnAttendance.FlatAppearance.BorderSize = 0;
            btnAttendance.FlatAppearance.MouseDownBackColor = Color.FromArgb(109, 0, 0);
            btnAttendance.FlatAppearance.MouseOverBackColor = Color.FromArgb(109, 0, 0);
            btnAttendance.FlatStyle = FlatStyle.Flat;
            btnAttendance.Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            btnAttendance.ForeColor = Color.FromArgb(179, 255, 255, 255);
            btnAttendance.Image = Properties.Resources.user_check;
            btnAttendance.ImageAlign = ContentAlignment.MiddleLeft;
            btnAttendance.Location = new Point(3, 165);
            btnAttendance.Name = "btnAttendance";
            btnAttendance.Padding = new Padding(16, 0, 0, 0);
            btnAttendance.Size = new Size(256, 48);
            btnAttendance.TabIndex = 8;
            btnAttendance.Text = "  Attendance";
            btnAttendance.TextAlign = ContentAlignment.MiddleLeft;
            btnAttendance.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnAttendance.UseVisualStyleBackColor = true;
            btnAttendance.Click += btnAttendance_Click;
            // 
            // btnGrade
            // 
            btnGrade.BackgroundImageLayout = ImageLayout.None;
            btnGrade.Cursor = Cursors.Hand;
            btnGrade.FlatAppearance.BorderSize = 0;
            btnGrade.FlatAppearance.MouseDownBackColor = Color.FromArgb(109, 0, 0);
            btnGrade.FlatAppearance.MouseOverBackColor = Color.FromArgb(109, 0, 0);
            btnGrade.FlatStyle = FlatStyle.Flat;
            btnGrade.Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            btnGrade.ForeColor = Color.FromArgb(179, 255, 255, 255);
            btnGrade.Image = Properties.Resources.report;
            btnGrade.ImageAlign = ContentAlignment.MiddleLeft;
            btnGrade.Location = new Point(3, 219);
            btnGrade.Name = "btnGrade";
            btnGrade.Padding = new Padding(16, 0, 0, 0);
            btnGrade.Size = new Size(256, 48);
            btnGrade.TabIndex = 9;
            btnGrade.Text = "  Grades";
            btnGrade.TextAlign = ContentAlignment.MiddleLeft;
            btnGrade.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnGrade.UseVisualStyleBackColor = true;
            btnGrade.Click += btnGrade_Click;
            // 
            // pnlContainerStudentPortal
            // 
            pnlContainerStudentPortal.Controls.Add(mainContentPanel);
            pnlContainerStudentPortal.Controls.Add(pnlHeader);
            pnlContainerStudentPortal.Controls.Add(pnlSidebar);
            pnlContainerStudentPortal.Dock = DockStyle.Fill;
            pnlContainerStudentPortal.Location = new Point(0, 0);
            pnlContainerStudentPortal.Name = "pnlContainerStudentPortal";
            pnlContainerStudentPortal.Size = new Size(1676, 759);
            pnlContainerStudentPortal.TabIndex = 0;
            // 
            // mainContentPanel
            // 
            mainContentPanel.Dock = DockStyle.Fill;
            mainContentPanel.Location = new Point(256, 72);
            mainContentPanel.Name = "mainContentPanel";
            mainContentPanel.Size = new Size(1420, 687);
            mainContentPanel.TabIndex = 28;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(42, 42, 42);
            pnlHeader.Controls.Add(panel15);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(256, 0);
            pnlHeader.Margin = new Padding(0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1420, 72);
            pnlHeader.TabIndex = 2;
            // 
            // panel15
            // 
            panel15.Controls.Add(panel16);
            panel15.Controls.Add(pictureBox2);
            panel15.Location = new Point(16, 8);
            panel15.Name = "panel15";
            panel15.Size = new Size(384, 48);
            panel15.TabIndex = 1;
            // 
            // panel16
            // 
            panel16.Controls.Add(label3);
            panel16.Controls.Add(label4);
            panel16.Location = new Point(64, 6);
            panel16.Name = "panel16";
            panel16.Size = new Size(320, 36);
            panel16.TabIndex = 1;
            // 
            // label3
            // 
            label3.Font = new Font("Arial", 10.2400007F, FontStyle.Bold, GraphicsUnit.Pixel, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(0, 0);
            label3.Name = "label3";
            label3.Size = new Size(320, 24);
            label3.TabIndex = 2;
            label3.Text = "Polytechnic University of the Philippines";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Arial", 7.68000031F, FontStyle.Regular, GraphicsUnit.Pixel);
            label4.ForeColor = Color.FromArgb(255, 193, 7);
            label4.Location = new Point(0, 20);
            label4.Name = "label4";
            label4.Size = new Size(320, 16);
            label4.TabIndex = 2;
            label4.Text = "Academic Portal";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.pup48x48;
            pictureBox2.Location = new Point(0, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(48, 48);
            pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            // 
            // StudentPortal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1676, 759);
            Controls.Add(pnlContainerStudentPortal);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "StudentPortal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "StudentPortal";
            WindowState = FormWindowState.Maximized;
            Load += StudentPortal_Load;
            pnlSidebar.ResumeLayout(false);
            panel12.ResumeLayout(false);
            panel13.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            panel7.ResumeLayout(false);
            panel8.ResumeLayout(false);
            panel10.ResumeLayout(false);
            panel11.ResumeLayout(false);
            pnllmsSubmenu.ResumeLayout(false);
            flowLayoutPanel2.ResumeLayout(false);
            pnlContainerStudentPortal.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            panel15.ResumeLayout(false);
            panel15.PerformLayout();
            panel16.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlSidebar;
        private Panel pnlContainerStudentPortal;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel panel7;
        private Button btnDashboard;
        private Panel panel8;
        private Button btnEnrollment;
        private Panel panel10;
        private Button btnAccounts;
        private Panel panel11;
        private Button btnLMS;
        private Panel panel12;
        private Panel panel13;
        private Button btnLogout;
        private Panel pnlHeader;
        private Panel panel15;
        private Panel panel16;
        private PictureBox pictureBox2;
        private Label label3;
        private Label label4;
        private Panel pnlYellow;
        private PUPAcadPortal.PortalContents.Misc.UserProfile userProfile1;
        private Panel mainContentPanel;
        private Panel pnllmsSubmenu;
        private FlowLayoutPanel flowLayoutPanel2;
        private Button btnAnnounce;
        private Button btnCalendar;
        private Button btnCourses;
        private Button btnAttendance;
        private Button btnGrade;
    }
}