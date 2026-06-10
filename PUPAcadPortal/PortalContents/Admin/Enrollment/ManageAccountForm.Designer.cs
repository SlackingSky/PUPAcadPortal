namespace PUPAcadPortal.PortalContents.Admin.Enrollment
{
    partial class ManageAccountForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            lblStudentName = new Label();
            lblBalance = new Label();
            lblFeesTitle = new Label();
            dgvFees = new DataGridView();
            lblPaymentsTitle = new Label();
            dgvPayments = new DataGridView();
            grpRecordPayment = new GroupBox();
            btnEditReference = new Button();
            btnSubmitPayment = new Button();
            txtReference = new TextBox();
            lblReference = new Label();
            txtPaymentAmount = new TextBox();
            lblAmount = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvFees).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvPayments).BeginInit();
            grpRecordPayment.SuspendLayout();
            SuspendLayout();
            // 
            // lblStudentName
            // 
            lblStudentName.AutoSize = true;
            lblStudentName.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStudentName.Location = new Point(12, 15);
            lblStudentName.Name = "lblStudentName";
            lblStudentName.Size = new Size(140, 25);
            lblStudentName.TabIndex = 0;
            lblStudentName.Text = "Student Name";
            // 
            // lblBalance
            // 
            lblBalance.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblBalance.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBalance.ForeColor = Color.DarkRed;
            lblBalance.Location = new Point(462, 15);
            lblBalance.Name = "lblBalance";
            lblBalance.Size = new Size(310, 25);
            lblBalance.TabIndex = 1;
            lblBalance.Text = "Current Balance: ₱0.00";
            lblBalance.TextAlign = ContentAlignment.TopRight;
            // 
            // lblFeesTitle
            // 
            lblFeesTitle.AutoSize = true;
            lblFeesTitle.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFeesTitle.Location = new Point(12, 60);
            lblFeesTitle.Name = "lblFeesTitle";
            lblFeesTitle.Size = new Size(156, 20);
            lblFeesTitle.TabIndex = 2;
            lblFeesTitle.Text = "Statement of Account";
            // 
            // dgvFees
            // 
            dgvFees.AllowUserToAddRows = false;
            dgvFees.AllowUserToDeleteRows = false;
            dgvFees.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvFees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvFees.BackgroundColor = Color.White;
            dgvFees.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.WhiteSmoke;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvFees.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvFees.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvFees.Location = new Point(17, 85);
            dgvFees.Name = "dgvFees";
            dgvFees.ReadOnly = true;
            dgvFees.RowHeadersVisible = false;
            dgvFees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFees.Size = new Size(755, 150);
            dgvFees.TabIndex = 3;
            // 
            // lblPaymentsTitle
            // 
            lblPaymentsTitle.AutoSize = true;
            lblPaymentsTitle.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPaymentsTitle.Location = new Point(13, 250);
            lblPaymentsTitle.Name = "lblPaymentsTitle";
            lblPaymentsTitle.Size = new Size(123, 20);
            lblPaymentsTitle.TabIndex = 4;
            lblPaymentsTitle.Text = "Payment History";
            // 
            // dgvPayments
            // 
            dgvPayments.AllowUserToAddRows = false;
            dgvPayments.AllowUserToDeleteRows = false;
            dgvPayments.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPayments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPayments.BackgroundColor = Color.White;
            dgvPayments.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.WhiteSmoke;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvPayments.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvPayments.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPayments.Location = new Point(17, 275);
            dgvPayments.Name = "dgvPayments";
            dgvPayments.ReadOnly = true;
            dgvPayments.RowHeadersVisible = false;
            dgvPayments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPayments.Size = new Size(755, 150);
            dgvPayments.TabIndex = 5;
            // 
            // grpRecordPayment
            // 
            grpRecordPayment.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpRecordPayment.Controls.Add(btnEditReference);
            grpRecordPayment.Controls.Add(btnSubmitPayment);
            grpRecordPayment.Controls.Add(txtReference);
            grpRecordPayment.Controls.Add(lblReference);
            grpRecordPayment.Controls.Add(txtPaymentAmount);
            grpRecordPayment.Controls.Add(lblAmount);
            grpRecordPayment.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpRecordPayment.Location = new Point(17, 445);
            grpRecordPayment.Name = "grpRecordPayment";
            grpRecordPayment.Size = new Size(755, 95);
            grpRecordPayment.TabIndex = 6;
            grpRecordPayment.TabStop = false;
            grpRecordPayment.Text = "Record a New Payment";
            // 
            // btnEditReference
            // 
            btnEditReference.Cursor = Cursors.Hand;
            btnEditReference.FlatStyle = FlatStyle.System;
            btnEditReference.Location = new Point(535, 39);
            btnEditReference.Name = "btnEditReference";
            btnEditReference.Size = new Size(50, 29);
            btnEditReference.TabIndex = 7;
            btnEditReference.Text = "Edit";
            btnEditReference.UseVisualStyleBackColor = true;
            // 
            // btnSubmitPayment
            // 
            btnSubmitPayment.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSubmitPayment.BackColor = Color.Maroon;
            btnSubmitPayment.Cursor = Cursors.Hand;
            btnSubmitPayment.FlatAppearance.BorderSize = 0;
            btnSubmitPayment.FlatStyle = FlatStyle.Flat;
            btnSubmitPayment.ForeColor = Color.White;
            btnSubmitPayment.Location = new Point(595, 36);
            btnSubmitPayment.Name = "btnSubmitPayment";
            btnSubmitPayment.Size = new Size(140, 35);
            btnSubmitPayment.TabIndex = 4;
            btnSubmitPayment.Text = "Record Payment";
            btnSubmitPayment.UseVisualStyleBackColor = false;
            // 
            // txtReference
            // 
            txtReference.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtReference.Location = new Point(365, 40);
            txtReference.Name = "txtReference";
            txtReference.Size = new Size(165, 27);
            txtReference.TabIndex = 3;
            // 
            // lblReference
            // 
            lblReference.AutoSize = true;
            lblReference.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblReference.Location = new Point(265, 45);
            lblReference.Name = "lblReference";
            lblReference.Size = new Size(98, 17);
            lblReference.TabIndex = 2;
            lblReference.Text = "Reference No. :";
            // 
            // txtPaymentAmount
            // 
            txtPaymentAmount.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPaymentAmount.Location = new Point(100, 40);
            txtPaymentAmount.Name = "txtPaymentAmount";
            txtPaymentAmount.Size = new Size(135, 27);
            txtPaymentAmount.TabIndex = 1;
            // 
            // lblAmount
            // 
            lblAmount.AutoSize = true;
            lblAmount.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblAmount.Location = new Point(20, 45);
            lblAmount.Name = "lblAmount";
            lblAmount.Size = new Size(76, 17);
            lblAmount.TabIndex = 0;
            lblAmount.Text = "Amount (₱):";
            // 
            // ManageAccountForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(784, 561);
            Controls.Add(grpRecordPayment);
            Controls.Add(dgvPayments);
            Controls.Add(lblPaymentsTitle);
            Controls.Add(dgvFees);
            Controls.Add(lblFeesTitle);
            Controls.Add(lblBalance);
            Controls.Add(lblStudentName);
            Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ManageAccountForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Manage Student Account";
            ((System.ComponentModel.ISupportInitialize)dgvFees).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvPayments).EndInit();
            grpRecordPayment.ResumeLayout(false);
            grpRecordPayment.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStudentName;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label lblFeesTitle;
        private System.Windows.Forms.DataGridView dgvFees;
        private System.Windows.Forms.Label lblPaymentsTitle;
        private System.Windows.Forms.DataGridView dgvPayments;
        private System.Windows.Forms.GroupBox grpRecordPayment;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.TextBox txtPaymentAmount;
        private System.Windows.Forms.Label lblReference;
        private System.Windows.Forms.TextBox txtReference;
        private System.Windows.Forms.Button btnSubmitPayment;
        private System.Windows.Forms.Button btnEditReference;
    }
}