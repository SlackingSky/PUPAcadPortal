namespace PUPAcadPortal.SignInRelated.AccountRecovery
{
    partial class RecoveryVerifying
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblPrompt = new Label();
            txtPin = new MaskedTextBox();
            btnVerify = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // lblPrompt
            // 
            lblPrompt.Anchor = AnchorStyles.Bottom;
            lblPrompt.AutoSize = true;
            lblPrompt.Font = new Font("Segoe UI", 10F);
            lblPrompt.ForeColor = Color.FromArgb(128, 0, 0);
            lblPrompt.Location = new Point(19, 31);
            lblPrompt.Name = "lblPrompt";
            lblPrompt.Size = new Size(261, 19);
            lblPrompt.TabIndex = 2;
            lblPrompt.Text = "Enter the 6-digit code sent to your email:";
            // 
            // txtPin
            // 
            txtPin.Anchor = AnchorStyles.Top;
            txtPin.Font = new Font("Segoe UI", 12F);
            txtPin.Location = new Point(100, 53);
            txtPin.Mask = "000000";
            txtPin.Name = "txtPin";
            txtPin.Size = new Size(100, 29);
            txtPin.TabIndex = 1;
            txtPin.TextAlign = HorizontalAlignment.Center;
            // 
            // btnVerify
            // 
            btnVerify.Anchor = AnchorStyles.Top;
            btnVerify.BackColor = Color.FromArgb(128, 0, 0);
            btnVerify.FlatAppearance.BorderSize = 0;
            btnVerify.FlatStyle = FlatStyle.Flat;
            btnVerify.ForeColor = Color.White;
            btnVerify.Location = new Point(90, 103);
            btnVerify.Name = "btnVerify";
            btnVerify.Size = new Size(120, 35);
            btnVerify.TabIndex = 0;
            btnVerify.Text = "Verify Code";
            btnVerify.UseVisualStyleBackColor = false;
            btnVerify.Click += btnVerify_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(lblPrompt, 0, 0);
            tableLayoutPanel1.Controls.Add(btnVerify, 0, 2);
            tableLayoutPanel1.Controls.Add(txtPin, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(300, 150);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // RecoveryVerifying
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(253, 251, 247);
            Controls.Add(tableLayoutPanel1);
            Name = "RecoveryVerifying";
            Size = new Size(300, 150);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.Button btnVerify;
        public MaskedTextBox txtPin;
        private TableLayoutPanel tableLayoutPanel1;
    }
}
