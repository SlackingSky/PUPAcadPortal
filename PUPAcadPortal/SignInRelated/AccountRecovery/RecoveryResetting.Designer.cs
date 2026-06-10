namespace PUPAcadPortal.SignInRelated.AccountRecovery
{
    partial class RecoveryResetting
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblPass = new Label();
            txtPass = new TextBox();
            lblConfirm = new Label();
            txtConfirm = new TextBox();
            btnSave = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new Panel();
            btnShowPass = new Button();
            panel2 = new Panel();
            btnShowPass1 = new Button();
            errorProvider1 = new ErrorProvider(components);
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // lblPass
            // 
            lblPass.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblPass.AutoSize = true;
            lblPass.ForeColor = Color.FromArgb(128, 0, 0);
            lblPass.Location = new Point(42, 29);
            lblPass.Name = "lblPass";
            lblPass.Size = new Size(105, 15);
            lblPass.TabIndex = 4;
            lblPass.Text = "New Password:      ";
            // 
            // txtPass
            // 
            txtPass.Anchor = AnchorStyles.None;
            txtPass.Location = new Point(40, 6);
            txtPass.Name = "txtPass";
            txtPass.Size = new Size(212, 23);
            txtPass.TabIndex = 3;
            txtPass.UseSystemPasswordChar = true;
            // 
            // lblConfirm
            // 
            lblConfirm.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblConfirm.AutoSize = true;
            lblConfirm.ForeColor = Color.FromArgb(128, 0, 0);
            lblConfirm.Location = new Point(40, 90);
            lblConfirm.Name = "lblConfirm";
            lblConfirm.Size = new Size(107, 15);
            lblConfirm.TabIndex = 2;
            lblConfirm.Text = "Confirm Password:";
            // 
            // txtConfirm
            // 
            txtConfirm.Anchor = AnchorStyles.None;
            txtConfirm.Location = new Point(40, 6);
            txtConfirm.Name = "txtConfirm";
            txtConfirm.Size = new Size(212, 23);
            txtConfirm.TabIndex = 1;
            txtConfirm.UseSystemPasswordChar = true;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.None;
            btnSave.BackColor = Color.FromArgb(128, 0, 0);
            tableLayoutPanel1.SetColumnSpan(btnSave, 2);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(90, 155);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(120, 34);
            btnSave.TabIndex = 0;
            btnSave.Text = "Save Password";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(lblPass, 0, 0);
            tableLayoutPanel1.Controls.Add(btnSave, 0, 4);
            tableLayoutPanel1.Controls.Add(lblConfirm, 0, 2);
            tableLayoutPanel1.Controls.Add(panel1, 0, 1);
            tableLayoutPanel1.Controls.Add(panel2, 0, 3);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 21F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(300, 200);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.SetColumnSpan(panel1, 2);
            panel1.Controls.Add(btnShowPass);
            panel1.Controls.Add(txtPass);
            panel1.Location = new Point(3, 47);
            panel1.Name = "panel1";
            panel1.Size = new Size(294, 34);
            panel1.TabIndex = 5;
            // 
            // btnShowPass
            // 
            btnShowPass.Cursor = Cursors.Hand;
            btnShowPass.FlatAppearance.BorderSize = 0;
            btnShowPass.FlatStyle = FlatStyle.Flat;
            btnShowPass.ForeColor = SystemColors.ControlText;
            btnShowPass.Image = Properties.Resources.Eye;
            btnShowPass.Location = new Point(226, 6);
            btnShowPass.Name = "btnShowPass";
            btnShowPass.Size = new Size(20, 20);
            btnShowPass.TabIndex = 4;
            btnShowPass.TabStop = false;
            btnShowPass.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.SetColumnSpan(panel2, 2);
            panel2.Controls.Add(btnShowPass1);
            panel2.Controls.Add(txtConfirm);
            panel2.Location = new Point(3, 108);
            panel2.Name = "panel2";
            panel2.Size = new Size(294, 34);
            panel2.TabIndex = 6;
            // 
            // btnShowPass1
            // 
            btnShowPass1.Cursor = Cursors.Hand;
            btnShowPass1.FlatAppearance.BorderSize = 0;
            btnShowPass1.FlatStyle = FlatStyle.Flat;
            btnShowPass1.ForeColor = SystemColors.ControlText;
            btnShowPass1.Image = Properties.Resources.Eye;
            btnShowPass1.Location = new Point(226, 7);
            btnShowPass1.Name = "btnShowPass1";
            btnShowPass1.Size = new Size(20, 20);
            btnShowPass1.TabIndex = 5;
            btnShowPass1.TabStop = false;
            btnShowPass1.UseVisualStyleBackColor = true;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // RecoveryResetting
            // 
            BackColor = Color.FromArgb(253, 251, 247);
            Controls.Add(tableLayoutPanel1);
            Name = "RecoveryResetting";
            Size = new Size(300, 200);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Label lblConfirm;
        private System.Windows.Forms.TextBox txtConfirm;
        private System.Windows.Forms.Button btnSave;
        private TableLayoutPanel tableLayoutPanel1;
        private ErrorProvider errorProvider1;
        private Panel panel1;
        private Button btnShowPass;
        private Panel panel2;
        private Button btnShowPass1;
    }
}
