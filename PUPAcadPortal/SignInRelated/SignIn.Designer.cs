using System.Drawing.Drawing2D;

namespace PUPAcadPortal
{
    partial class SignIn
    {

        public Color color1 = Color.FromArgb(255, 80, 0, 0);
        public Color color2 = Color.FromArgb(230, 80, 0, 0);
        public Color color3 = Color.FromArgb(204, 80, 0, 0);
        public float angle = 45f; // 90f for Vertical

        protected override void OnPaint(PaintEventArgs e)
        {

            try
            {
                base.OnPaint(e);
                using (LinearGradientBrush brush = new LinearGradientBrush(
                this.ClientRectangle, color1, color3, angle))
                {


                    // Define 3 colors at positions 0%, 50%, and 100%
                    ColorBlend cb = new ColorBlend();
                    cb.Colors = new Color[] { color1, color2, color3 };
                    cb.Positions = new float[] { 0f, 0.5f, 1f };
                    brush.InterpolationColors = cb;

                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
                }
            }
            catch (Exception)
            {
                return;
            }
            
        }
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignIn));
            lnkForgotPassword = new LinkLabel();
            panel1 = new Panel();
            panel9 = new Panel();
            label7 = new Label();
            label6 = new Label();
            panel3 = new Panel();
            panel4 = new Panel();
            btnSignIn = new buttonRounded();
            panel6 = new Panel();
            lblPassWarn = new Label();
            panel8 = new Panel();
            btnShowPass = new Button();
            pictureBox3 = new PictureBox();
            txtPassword = new TextBox();
            label5 = new Label();
            panel5 = new Panel();
            lblUsernameWarn = new Label();
            panel7 = new Panel();
            pictureBox2 = new PictureBox();
            txtUsername = new TextBox();
            label4 = new Label();
            label3 = new Label();
            panel2 = new Panel();
            label2 = new Label();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            panel1.SuspendLayout();
            panel9.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            panel6.SuspendLayout();
            panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            panel5.SuspendLayout();
            panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // lnkForgotPassword
            // 
            lnkForgotPassword.AutoSize = true;
            lnkForgotPassword.Location = new Point(4, 178);
            lnkForgotPassword.Name = "lnkForgotPassword";
            lnkForgotPassword.Size = new Size(100, 15);
            lnkForgotPassword.TabIndex = 99999999;
            lnkForgotPassword.TabStop = true;
            lnkForgotPassword.Text = "Forgot Password?";
            lnkForgotPassword.LinkClicked += lnkForgotPassword_LinkClicked;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.None;
            panel1.BackColor = Color.Transparent;
            panel1.Controls.Add(panel9);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Location = new Point(533, 16);
            panel1.Name = "panel1";
            panel1.Size = new Size(448, 791);
            panel1.TabIndex = 0;
            // 
            // panel9
            // 
            panel9.Anchor = AnchorStyles.Bottom;
            panel9.Controls.Add(label7);
            panel9.Controls.Add(label6);
            panel9.Location = new Point(0, 747);
            panel9.Name = "panel9";
            panel9.Size = new Size(448, 44);
            panel9.TabIndex = 4;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label7.Font = new Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
            label7.ForeColor = Color.White;
            label7.Location = new Point(0, 24);
            label7.Name = "label7";
            label7.Size = new Size(448, 20);
            label7.TabIndex = 1;
            label7.Text = "Santa Maria Campus";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label6.Font = new Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
            label6.ForeColor = Color.White;
            label6.Location = new Point(0, 0);
            label6.Name = "label6";
            label6.Size = new Size(448, 20);
            label6.TabIndex = 0;
            label6.Text = "© 2026 Polytechnic University of the Philippines";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            panel3.Anchor = AnchorStyles.None;
            panel3.BackColor = Color.Transparent;
            panel3.BackgroundImage = Properties.Resources.Container__2_;
            panel3.BackgroundImageLayout = ImageLayout.Stretch;
            panel3.Controls.Add(panel4);
            panel3.Controls.Add(label3);
            panel3.Location = new Point(0, 212);
            panel3.Margin = new Padding(0);
            panel3.Name = "panel3";
            panel3.Size = new Size(448, 511);
            panel3.TabIndex = 3;
            // 
            // panel4
            // 
            panel4.Anchor = AnchorStyles.None;
            panel4.BackColor = Color.Transparent;
            panel4.Controls.Add(lnkForgotPassword);
            panel4.Controls.Add(btnSignIn);
            panel4.Controls.Add(panel6);
            panel4.Controls.Add(panel5);
            panel4.Location = new Point(32, 160);
            panel4.Name = "panel4";
            panel4.Size = new Size(384, 275);
            panel4.TabIndex = 0;
            // 
            // btnSignIn
            // 
            btnSignIn.BackColor = Color.FromArgb(128, 0, 0);
            btnSignIn.BorderRadius = 20;
            btnSignIn.FlatAppearance.BorderSize = 0;
            btnSignIn.FlatStyle = FlatStyle.Flat;
            btnSignIn.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSignIn.ForeColor = Color.White;
            btnSignIn.Location = new Point(0, 227);
            btnSignIn.Name = "btnSignIn";
            btnSignIn.Size = new Size(384, 48);
            btnSignIn.TabIndex = 4;
            btnSignIn.Text = "Sign In";
            btnSignIn.UseVisualStyleBackColor = false;
            btnSignIn.Click += btnSignIn_Click;
            // 
            // panel6
            // 
            panel6.Anchor = AnchorStyles.None;
            panel6.Controls.Add(lblPassWarn);
            panel6.Controls.Add(panel8);
            panel6.Controls.Add(label5);
            panel6.Location = new Point(0, 94);
            panel6.Name = "panel6";
            panel6.Size = new Size(384, 78);
            panel6.TabIndex = 1;
            // 
            // lblPassWarn
            // 
            lblPassWarn.AutoSize = true;
            lblPassWarn.ForeColor = Color.Red;
            lblPassWarn.Location = new Point(82, 2);
            lblPassWarn.Name = "lblPassWarn";
            lblPassWarn.Size = new Size(107, 15);
            lblPassWarn.TabIndex = 5;
            lblPassWarn.Text = "Password Required";
            lblPassWarn.Visible = false;
            // 
            // panel8
            // 
            panel8.Anchor = AnchorStyles.None;
            panel8.BackgroundImage = Properties.Resources.input;
            panel8.BackgroundImageLayout = ImageLayout.Stretch;
            panel8.Controls.Add(btnShowPass);
            panel8.Controls.Add(pictureBox3);
            panel8.Controls.Add(txtPassword);
            panel8.Cursor = Cursors.IBeam;
            panel8.Location = new Point(0, 28);
            panel8.Name = "panel8";
            panel8.Size = new Size(384, 50);
            panel8.TabIndex = 0;
            panel8.Click += panel8_Click;
            // 
            // btnShowPass
            // 
            btnShowPass.Cursor = Cursors.Hand;
            btnShowPass.FlatAppearance.BorderSize = 0;
            btnShowPass.FlatStyle = FlatStyle.Flat;
            btnShowPass.ForeColor = SystemColors.ControlText;
            btnShowPass.Image = Properties.Resources.Eye;
            btnShowPass.Location = new Point(352, 15);
            btnShowPass.Name = "btnShowPass";
            btnShowPass.Size = new Size(20, 20);
            btnShowPass.TabIndex = 3;
            btnShowPass.TabStop = false;
            btnShowPass.UseVisualStyleBackColor = true;
            btnShowPass.Click += btnShowPass_Click;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.Lock;
            pictureBox3.Location = new Point(12, 15);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(20, 20);
            pictureBox3.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox3.TabIndex = 2;
            pictureBox3.TabStop = false;
            // 
            // txtPassword
            // 
            txtPassword.BackColor = Color.White;
            txtPassword.BorderStyle = BorderStyle.None;
            txtPassword.Cursor = Cursors.IBeam;
            txtPassword.Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
            txtPassword.Location = new Point(40, 15);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Enter your password";
            txtPassword.Size = new Size(312, 19);
            txtPassword.TabIndex = 0;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.TextChanged += txtPassword_TextChanged;
            txtPassword.KeyDown += txtPassword_KeyDown;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.None;
            label5.Font = new Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
            label5.Location = new Point(0, 0);
            label5.Name = "label5";
            label5.Size = new Size(384, 20);
            label5.TabIndex = 1;
            label5.Text = "Password:";
            // 
            // panel5
            // 
            panel5.Anchor = AnchorStyles.None;
            panel5.Controls.Add(lblUsernameWarn);
            panel5.Controls.Add(panel7);
            panel5.Controls.Add(label4);
            panel5.Location = new Point(0, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(384, 78);
            panel5.TabIndex = 0;
            // 
            // lblUsernameWarn
            // 
            lblUsernameWarn.AutoSize = true;
            lblUsernameWarn.ForeColor = Color.Red;
            lblUsernameWarn.Location = new Point(82, 2);
            lblUsernameWarn.Name = "lblUsernameWarn";
            lblUsernameWarn.Size = new Size(110, 15);
            lblUsernameWarn.TabIndex = 4;
            lblUsernameWarn.Text = "Username Required";
            lblUsernameWarn.Visible = false;
            // 
            // panel7
            // 
            panel7.Anchor = AnchorStyles.None;
            panel7.BackColor = Color.White;
            panel7.BackgroundImage = Properties.Resources.input;
            panel7.BackgroundImageLayout = ImageLayout.Stretch;
            panel7.Controls.Add(pictureBox2);
            panel7.Controls.Add(txtUsername);
            panel7.Cursor = Cursors.IBeam;
            panel7.Location = new Point(0, 28);
            panel7.Name = "panel7";
            panel7.Size = new Size(384, 50);
            panel7.TabIndex = 0;
            panel7.Click += panel7_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.User;
            pictureBox2.Location = new Point(12, 15);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(20, 20);
            pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox2.TabIndex = 2;
            pictureBox2.TabStop = false;
            // 
            // txtUsername
            // 
            txtUsername.BackColor = Color.White;
            txtUsername.BorderStyle = BorderStyle.None;
            txtUsername.Cursor = Cursors.IBeam;
            txtUsername.Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
            txtUsername.Location = new Point(40, 15);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Enter your username";
            txtUsername.Size = new Size(328, 19);
            txtUsername.TabIndex = 0;
            txtUsername.TextChanged += txtUsername_TextChanged;
            txtUsername.KeyDown += txtUsername_KeyDown;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.None;
            label4.Font = new Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
            label4.Location = new Point(0, 0);
            label4.Name = "label4";
            label4.Size = new Size(384, 20);
            label4.TabIndex = 0;
            label4.Text = "Username:";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.None;
            label3.Font = new Font("Arial", 24F, FontStyle.Bold, GraphicsUnit.Pixel, 0);
            label3.ForeColor = Color.Maroon;
            label3.Location = new Point(32, 32);
            label3.Name = "label3";
            label3.Size = new Size(384, 32);
            label3.TabIndex = 3;
            label3.Text = "Sign In";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.None;
            panel2.Controls.Add(label2);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(pictureBox1);
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(448, 180);
            panel2.TabIndex = 0;
            // 
            // label2
            // 
            label2.Font = new Font("Arial", 16F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(0, 156);
            label2.Name = "label2";
            label2.Size = new Size(448, 24);
            label2.TabIndex = 2;
            label2.Text = "Polytechnic University of the Philippines";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.Font = new Font("Arial", 30F, FontStyle.Bold, GraphicsUnit.Pixel, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(0, 112);
            label1.Name = "label1";
            label1.Size = new Size(448, 36);
            label1.TabIndex = 1;
            label1.Text = "PUP Academic Portal";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.img__1_;
            pictureBox1.Location = new Point(176, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(96, 96);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // SignIn
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Maroon;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(1513, 823);
            Controls.Add(panel1);
            DoubleBuffered = true;
            ForeColor = SystemColors.ControlText;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "SignIn";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sign In";
            Activated += SignIn_Activated;
            Load += SignIn_Load;
            panel1.ResumeLayout(false);
            panel9.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            panel8.ResumeLayout(false);
            panel8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel7.ResumeLayout(false);
            panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Label label1;
        private PictureBox pictureBox1;
        private Label label2;
        private Panel panel3;
        private Label label3;
        private Panel panel4;
        private Panel panel6;
        private Label label5;
        private Panel panel5;
        private Label label4;
        private TextBox txtUsername;
        private PictureBox pictureBox2;
        private Panel panel8;
        private PictureBox pictureBox3;
        private TextBox txtPassword;
        private Button btnShowPass;
        private Panel panel9;
        private Label label6;
        private Label label7;
        private buttonRounded btnSignIn;
        private Panel panel7;
        private Label lblUsernameWarn;
        private Label lblPassWarn;
        private LinkLabel lnkForgotPassword;
    }
}
