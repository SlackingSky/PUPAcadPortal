namespace PUPAcadPortal.SignInRelated.AccountRecovery
{
    partial class RecoveryRequesting
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
            txtInput = new TextBox();
            btnSend = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // lblPrompt
            // 
            lblPrompt.Anchor = AnchorStyles.Bottom;
            lblPrompt.Font = new Font("Segoe UI", 10F);
            lblPrompt.ForeColor = Color.FromArgb(128, 0, 0);
            lblPrompt.Location = new Point(52, 31);
            lblPrompt.Name = "lblPrompt";
            lblPrompt.Size = new Size(195, 19);
            lblPrompt.TabIndex = 2;
            lblPrompt.Text = "Enter your Email or Username:";
            // 
            // txtInput
            // 
            txtInput.Anchor = AnchorStyles.None;
            txtInput.Location = new Point(53, 63);
            txtInput.Name = "txtInput";
            txtInput.Size = new Size(194, 23);
            txtInput.TabIndex = 1;
            txtInput.TextChanged += txtInput_TextChanged;
            // 
            // btnSend
            // 
            btnSend.Anchor = AnchorStyles.Top;
            btnSend.BackColor = Color.FromArgb(128, 0, 0);
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.ForeColor = Color.White;
            btnSend.Location = new Point(90, 103);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(120, 30);
            btnSend.TabIndex = 0;
            btnSend.Text = "Send Reset Code";
            btnSend.UseVisualStyleBackColor = false;
            btnSend.Click += btnSend_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(lblPrompt, 0, 0);
            tableLayoutPanel1.Controls.Add(btnSend, 0, 2);
            tableLayoutPanel1.Controls.Add(txtInput, 0, 1);
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(300, 150);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // RecoveryRequesting
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(253, 251, 247);
            Controls.Add(tableLayoutPanel1);
            Name = "RecoveryRequesting";
            Size = new Size(300, 150);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button btnSend;
        private TableLayoutPanel tableLayoutPanel1;
    }
}
