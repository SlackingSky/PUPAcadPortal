namespace PUPAcadPortal.PortalContents.Misc
{
    partial class EditProfileContent
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
            components = new System.ComponentModel.Container();
            pnlEditProfileContent = new Panel();
            pnlStudentRegistrationContainer = new Panel();
            txtIDNumber = new TextBox();
            btnShowConfirm = new Button();
            btnShowPass = new Button();
            btnVerify = new Button();
            txtPassword = new TextBox();
            phAddressFields1 = new PUPAcadPortal.PHAddress.PHAddressFields();
            btnEditInfo = new Button();
            mtbPhone = new MaskedTextBox();
            label25 = new Label();
            label26 = new Label();
            label27 = new Label();
            txtPersonalEmail = new TextBox();
            pnlStudentMaroonLine2 = new Panel();
            lblRSStudentContactInfo = new Label();
            label29 = new Label();
            txtConfirmPass = new TextBox();
            label30 = new Label();
            label32 = new Label();
            txtUsername = new TextBox();
            label33 = new Label();
            pnlStudentMaroonLine1 = new Panel();
            lblStudentPersonalInfo = new Label();
            label35 = new Label();
            label36 = new Label();
            pictureBox5 = new PictureBox();
            mySqlCommandBuilder1 = new MySqlConnector.MySqlCommandBuilder();
            errorProvider1 = new ErrorProvider(components);
            pnlEditProfileContent.SuspendLayout();
            pnlStudentRegistrationContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // pnlEditProfileContent
            // 
            pnlEditProfileContent.AutoScroll = true;
            pnlEditProfileContent.AutoScrollMinSize = new Size(0, 10);
            pnlEditProfileContent.BackColor = SystemColors.ControlLight;
            pnlEditProfileContent.Controls.Add(pnlStudentRegistrationContainer);
            pnlEditProfileContent.Controls.Add(label35);
            pnlEditProfileContent.Controls.Add(label36);
            pnlEditProfileContent.Controls.Add(pictureBox5);
            pnlEditProfileContent.Dock = DockStyle.Fill;
            pnlEditProfileContent.Location = new Point(0, 0);
            pnlEditProfileContent.Name = "pnlEditProfileContent";
            pnlEditProfileContent.Size = new Size(1254, 719);
            pnlEditProfileContent.TabIndex = 14;
            // 
            // pnlStudentRegistrationContainer
            // 
            pnlStudentRegistrationContainer.Anchor = AnchorStyles.Top;
            pnlStudentRegistrationContainer.AutoScroll = true;
            pnlStudentRegistrationContainer.BackColor = Color.White;
            pnlStudentRegistrationContainer.BorderStyle = BorderStyle.FixedSingle;
            pnlStudentRegistrationContainer.Controls.Add(txtIDNumber);
            pnlStudentRegistrationContainer.Controls.Add(btnShowConfirm);
            pnlStudentRegistrationContainer.Controls.Add(btnShowPass);
            pnlStudentRegistrationContainer.Controls.Add(btnVerify);
            pnlStudentRegistrationContainer.Controls.Add(txtPassword);
            pnlStudentRegistrationContainer.Controls.Add(phAddressFields1);
            pnlStudentRegistrationContainer.Controls.Add(btnEditInfo);
            pnlStudentRegistrationContainer.Controls.Add(mtbPhone);
            pnlStudentRegistrationContainer.Controls.Add(label25);
            pnlStudentRegistrationContainer.Controls.Add(label26);
            pnlStudentRegistrationContainer.Controls.Add(label27);
            pnlStudentRegistrationContainer.Controls.Add(txtPersonalEmail);
            pnlStudentRegistrationContainer.Controls.Add(pnlStudentMaroonLine2);
            pnlStudentRegistrationContainer.Controls.Add(lblRSStudentContactInfo);
            pnlStudentRegistrationContainer.Controls.Add(label29);
            pnlStudentRegistrationContainer.Controls.Add(txtConfirmPass);
            pnlStudentRegistrationContainer.Controls.Add(label30);
            pnlStudentRegistrationContainer.Controls.Add(label32);
            pnlStudentRegistrationContainer.Controls.Add(txtUsername);
            pnlStudentRegistrationContainer.Controls.Add(label33);
            pnlStudentRegistrationContainer.Controls.Add(pnlStudentMaroonLine1);
            pnlStudentRegistrationContainer.Controls.Add(lblStudentPersonalInfo);
            pnlStudentRegistrationContainer.Location = new Point(114, 112);
            pnlStudentRegistrationContainer.Name = "pnlStudentRegistrationContainer";
            pnlStudentRegistrationContainer.Size = new Size(959, 841);
            pnlStudentRegistrationContainer.TabIndex = 9;
            // 
            // txtIDNumber
            // 
            txtIDNumber.Font = new Font("Segoe UI", 12F);
            txtIDNumber.Location = new Point(26, 109);
            txtIDNumber.Name = "txtIDNumber";
            txtIDNumber.PlaceholderText = "ID Number";
            txtIDNumber.Size = new Size(418, 29);
            txtIDNumber.TabIndex = 64;
            txtIDNumber.Tag = "disabled";
            // 
            // btnShowConfirm
            // 
            btnShowConfirm.BackColor = Color.Transparent;
            btnShowConfirm.Cursor = Cursors.Hand;
            btnShowConfirm.FlatAppearance.BorderSize = 0;
            btnShowConfirm.FlatStyle = FlatStyle.Flat;
            btnShowConfirm.ForeColor = SystemColors.ControlText;
            btnShowConfirm.Image = Properties.Resources.Eye;
            btnShowConfirm.Location = new Point(418, 238);
            btnShowConfirm.Name = "btnShowConfirm";
            btnShowConfirm.Size = new Size(20, 20);
            btnShowConfirm.TabIndex = 63;
            btnShowConfirm.TabStop = false;
            btnShowConfirm.UseVisualStyleBackColor = false;
            btnShowConfirm.Visible = false;
            btnShowConfirm.Click += btnShowConfirm_Click;
            // 
            // btnShowPass
            // 
            btnShowPass.BackColor = Color.Transparent;
            btnShowPass.Cursor = Cursors.Hand;
            btnShowPass.FlatAppearance.BorderSize = 0;
            btnShowPass.FlatStyle = FlatStyle.Flat;
            btnShowPass.ForeColor = SystemColors.ControlText;
            btnShowPass.Image = Properties.Resources.Eye;
            btnShowPass.Location = new Point(192, 238);
            btnShowPass.Name = "btnShowPass";
            btnShowPass.Size = new Size(20, 20);
            btnShowPass.TabIndex = 62;
            btnShowPass.TabStop = false;
            btnShowPass.UseVisualStyleBackColor = false;
            btnShowPass.Visible = false;
            btnShowPass.Click += btnShowPass_Click;
            // 
            // btnVerify
            // 
            btnVerify.BackColor = Color.Maroon;
            btnVerify.FlatAppearance.BorderColor = Color.Maroon;
            btnVerify.FlatStyle = FlatStyle.Flat;
            btnVerify.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnVerify.ForeColor = Color.White;
            btnVerify.Location = new Point(462, 235);
            btnVerify.Name = "btnVerify";
            btnVerify.Size = new Size(66, 29);
            btnVerify.TabIndex = 61;
            btnVerify.Text = "Verify";
            btnVerify.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnVerify.UseVisualStyleBackColor = false;
            btnVerify.Visible = false;
            btnVerify.Click += btnVerify_Click;
            // 
            // txtPassword
            // 
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Font = new Font("Segoe UI", 12F);
            txtPassword.Location = new Point(26, 235);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = " Enter current password";
            txtPassword.Size = new Size(190, 29);
            txtPassword.TabIndex = 60;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // phAddressFields1
            // 
            phAddressFields1.AutoSize = true;
            phAddressFields1.Location = new Point(24, 432);
            phAddressFields1.Name = "phAddressFields1";
            phAddressFields1.Size = new Size(884, 296);
            phAddressFields1.TabIndex = 59;
            // 
            // btnEditInfo
            // 
            btnEditInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnEditInfo.BackColor = Color.Maroon;
            btnEditInfo.FlatAppearance.BorderColor = Color.Maroon;
            btnEditInfo.FlatStyle = FlatStyle.Flat;
            btnEditInfo.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEditInfo.ForeColor = Color.White;
            btnEditInfo.Image = Properties.Resources.student_2_161;
            btnEditInfo.Location = new Point(702, 762);
            btnEditInfo.Name = "btnEditInfo";
            btnEditInfo.Size = new Size(215, 37);
            btnEditInfo.TabIndex = 56;
            btnEditInfo.Text = "Edit Information";
            btnEditInfo.TextAlign = ContentAlignment.MiddleRight;
            btnEditInfo.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnEditInfo.UseVisualStyleBackColor = false;
            btnEditInfo.Click += btnEditInfo_Click;
            // 
            // mtbPhone
            // 
            mtbPhone.BorderStyle = BorderStyle.FixedSingle;
            mtbPhone.Font = new Font("Segoe UI", 12F);
            mtbPhone.Location = new Point(470, 361);
            mtbPhone.Mask = "(+63) 000-000-0000";
            mtbPhone.Name = "mtbPhone";
            mtbPhone.Size = new Size(437, 29);
            mtbPhone.TabIndex = 34;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label25.ForeColor = Color.Black;
            label25.Location = new Point(467, 337);
            label25.Name = "label25";
            label25.Size = new Size(120, 21);
            label25.TabIndex = 33;
            label25.Text = "Phone Number";
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label26.ForeColor = Color.Black;
            label26.Location = new Point(23, 401);
            label26.Name = "label26";
            label26.Size = new Size(70, 21);
            label26.TabIndex = 30;
            label26.Text = "Address";
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label27.ForeColor = Color.Black;
            label27.Location = new Point(23, 337);
            label27.Name = "label27";
            label27.Size = new Size(112, 21);
            label27.TabIndex = 28;
            label27.Text = "Email Address";
            // 
            // txtPersonalEmail
            // 
            txtPersonalEmail.BorderStyle = BorderStyle.FixedSingle;
            txtPersonalEmail.Font = new Font("Segoe UI", 12F);
            txtPersonalEmail.Location = new Point(26, 361);
            txtPersonalEmail.Name = "txtPersonalEmail";
            txtPersonalEmail.PlaceholderText = " Personal Email Address";
            txtPersonalEmail.Size = new Size(438, 29);
            txtPersonalEmail.TabIndex = 27;
            // 
            // pnlStudentMaroonLine2
            // 
            pnlStudentMaroonLine2.BackColor = Color.Maroon;
            pnlStudentMaroonLine2.Location = new Point(23, 315);
            pnlStudentMaroonLine2.Margin = new Padding(0);
            pnlStudentMaroonLine2.Name = "pnlStudentMaroonLine2";
            pnlStudentMaroonLine2.Size = new Size(890, 3);
            pnlStudentMaroonLine2.TabIndex = 26;
            // 
            // lblRSStudentContactInfo
            // 
            lblRSStudentContactInfo.AutoSize = true;
            lblRSStudentContactInfo.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRSStudentContactInfo.ForeColor = Color.Black;
            lblRSStudentContactInfo.Location = new Point(23, 280);
            lblRSStudentContactInfo.Name = "lblRSStudentContactInfo";
            lblRSStudentContactInfo.Size = new Size(208, 30);
            lblRSStudentContactInfo.TabIndex = 25;
            lblRSStudentContactInfo.Text = "Contact Information";
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label29.ForeColor = Color.Black;
            label29.Location = new Point(238, 211);
            label29.Name = "label29";
            label29.Size = new Size(142, 21);
            label29.TabIndex = 23;
            label29.Text = "Confirm Password";
            label29.Visible = false;
            // 
            // txtConfirmPass
            // 
            txtConfirmPass.BorderStyle = BorderStyle.FixedSingle;
            txtConfirmPass.Font = new Font("Segoe UI", 12F);
            txtConfirmPass.Location = new Point(232, 235);
            txtConfirmPass.Name = "txtConfirmPass";
            txtConfirmPass.PlaceholderText = " Confirm current password";
            txtConfirmPass.Size = new Size(210, 29);
            txtConfirmPass.TabIndex = 22;
            txtConfirmPass.UseSystemPasswordChar = true;
            txtConfirmPass.Visible = false;
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label30.ForeColor = Color.Black;
            label30.Location = new Point(23, 211);
            label30.Name = "label30";
            label30.Size = new Size(79, 21);
            label30.TabIndex = 21;
            label30.Text = "Password";
            // 
            // label32
            // 
            label32.AutoSize = true;
            label32.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label32.ForeColor = Color.Black;
            label32.Location = new Point(23, 147);
            label32.Name = "label32";
            label32.Size = new Size(83, 21);
            label32.TabIndex = 15;
            label32.Text = "Username";
            // 
            // txtUsername
            // 
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.Font = new Font("Segoe UI", 12F);
            txtUsername.Location = new Point(26, 171);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = " Enter Username";
            txtUsername.Size = new Size(416, 29);
            txtUsername.TabIndex = 14;
            // 
            // label33
            // 
            label33.AutoSize = true;
            label33.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label33.ForeColor = Color.Black;
            label33.Location = new Point(23, 83);
            label33.Name = "label33";
            label33.Size = new Size(90, 21);
            label33.TabIndex = 11;
            label33.Text = "ID Number";
            // 
            // pnlStudentMaroonLine1
            // 
            pnlStudentMaroonLine1.BackColor = Color.Maroon;
            pnlStudentMaroonLine1.Location = new Point(23, 61);
            pnlStudentMaroonLine1.Margin = new Padding(0);
            pnlStudentMaroonLine1.Name = "pnlStudentMaroonLine1";
            pnlStudentMaroonLine1.Size = new Size(890, 3);
            pnlStudentMaroonLine1.TabIndex = 9;
            // 
            // lblStudentPersonalInfo
            // 
            lblStudentPersonalInfo.AutoSize = true;
            lblStudentPersonalInfo.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStudentPersonalInfo.ForeColor = Color.Black;
            lblStudentPersonalInfo.Location = new Point(23, 26);
            lblStudentPersonalInfo.Name = "lblStudentPersonalInfo";
            lblStudentPersonalInfo.Size = new Size(212, 30);
            lblStudentPersonalInfo.TabIndex = 6;
            lblStudentPersonalInfo.Text = "Account Information";
            // 
            // label35
            // 
            label35.AutoSize = true;
            label35.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            label35.ForeColor = Color.DimGray;
            label35.Location = new Point(106, 77);
            label35.Name = "label35";
            label35.Size = new Size(209, 19);
            label35.TabIndex = 8;
            label35.Text = "View / Update your information";
            // 
            // label36
            // 
            label36.AutoSize = true;
            label36.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label36.ForeColor = Color.Black;
            label36.Location = new Point(95, 37);
            label36.Name = "label36";
            label36.Size = new Size(257, 40);
            label36.TabIndex = 7;
            label36.Text = "View Information";
            // 
            // pictureBox5
            // 
            pictureBox5.BackColor = Color.Maroon;
            pictureBox5.Image = Properties.Resources.professor_32;
            pictureBox5.Location = new Point(32, 37);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(57, 59);
            pictureBox5.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox5.TabIndex = 6;
            pictureBox5.TabStop = false;
            // 
            // mySqlCommandBuilder1
            // 
            mySqlCommandBuilder1.DataAdapter = null;
            mySqlCommandBuilder1.QuotePrefix = "`";
            mySqlCommandBuilder1.QuoteSuffix = "`";
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // EditProfileContent
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlEditProfileContent);
            Name = "EditProfileContent";
            Size = new Size(1254, 719);
            Load += EditProfileContent_Load;
            pnlEditProfileContent.ResumeLayout(false);
            pnlEditProfileContent.PerformLayout();
            pnlStudentRegistrationContainer.ResumeLayout(false);
            pnlStudentRegistrationContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlEditProfileContent;
        private Label label35;
        private Label label36;
        private PictureBox pictureBox5;
        private Panel pnlStudentRegistrationContainer;
        private PHAddress.PHAddressFields phAddressFields1;
        private Button btnEditInfo;
        private DateTimePicker dtpRSStudentBirthDate;
        private MaskedTextBox mtbPhone;
        private Label label25;
        private Label label26;
        private Label label27;
        private TextBox txtPersonalEmail;
        private Panel pnlStudentMaroonLine2;
        private Label lblRSStudentContactInfo;
        private Label label29;
        private TextBox txtRSStudentLastName;
        private Label label30;
        private TextBox txtRSStudentFirstName;
        private Label label31;
        private Label label32;
        private TextBox txtUsername;
        private Label label33;
        private Panel pnlStudentMaroonLine1;
        private Label lblStudentPersonalInfo;
        private TextBox txtPassword;
        private TextBox txtConfirmPass;
        private Button btnVerify;
        private Button btnShowPass;
        private MySqlConnector.MySqlCommandBuilder mySqlCommandBuilder1;
        private Button btnShowConfirm;
        private TextBox txtIDNumber;
        private ErrorProvider errorProvider1;
    }
}
