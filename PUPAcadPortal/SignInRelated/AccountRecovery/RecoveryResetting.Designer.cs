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
            lblPass = new Label();
            txtPass = new TextBox();
            lblConfirm = new Label();
            txtConfirm = new TextBox();
            btnSave = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
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
            tableLayoutPanel1.SetColumnSpan(txtPass, 2);
            txtPass.Location = new Point(44, 52);
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
            tableLayoutPanel1.SetColumnSpan(txtConfirm, 2);
            txtConfirm.Location = new Point(44, 113);
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
            tableLayoutPanel1.Controls.Add(txtConfirm, 0, 3);
            tableLayoutPanel1.Controls.Add(lblConfirm, 0, 2);
            tableLayoutPanel1.Controls.Add(txtPass, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 21F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
            tableLayoutPanel1.Size = new Size(300, 200);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // RecoveryResetting
            // 
            BackColor = Color.FromArgb(253, 251, 247);
            Controls.Add(tableLayoutPanel1);
            Name = "RecoveryResetting";
            Size = new Size(300, 200);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Label lblConfirm;
        private System.Windows.Forms.TextBox txtConfirm;
        private System.Windows.Forms.Button btnSave;
        private TableLayoutPanel tableLayoutPanel1;
    }
}
