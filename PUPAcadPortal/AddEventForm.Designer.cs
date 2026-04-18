namespace PUPAcadPortal
{
    partial class AddEventForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblHeader = new Label();
            lblDate = new Label();
            lblDateValue = new Label();
            lblType = new Label();
            cmbType = new ComboBox();
            lblTitle = new Label();
            txtTitle = new TextBox();
            lblDesc = new Label();
            txtDesc = new TextBox();
            lblStart = new Label();
            dtpStart = new DateTimePicker();
            lblEnd = new Label();
            dtpEnd = new DateTimePicker();
            lblRoom = new Label();
            txtRoom = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            pnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.Maroon;
            pnlHeader.Controls.Add(lblHeader);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(414, 44);
            pnlHeader.TabIndex = 0;
            // 
            // lblHeader
            // 
            lblHeader.Dock = DockStyle.Fill;
            lblHeader.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblHeader.ForeColor = Color.White;
            lblHeader.Location = new Point(0, 0);
            lblHeader.Name = "lblHeader";
            lblHeader.Padding = new Padding(10, 0, 0, 0);
            lblHeader.Size = new Size(414, 44);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "Add Event";
            lblHeader.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDate
            // 
            lblDate.Font = new Font("Segoe UI", 9F);
            lblDate.ForeColor = Color.FromArgb(60, 60, 60);
            lblDate.Location = new Point(12, 54);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(90, 20);
            lblDate.TabIndex = 1;
            lblDate.Text = "Date:";
            // 
            // lblDateValue
            // 
            lblDateValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDateValue.ForeColor = Color.Maroon;
            lblDateValue.Location = new Point(108, 54);
            lblDateValue.Name = "lblDateValue";
            lblDateValue.Size = new Size(286, 20);
            lblDateValue.TabIndex = 2;
            // 
            // lblType
            // 
            lblType.Font = new Font("Segoe UI", 9F);
            lblType.ForeColor = Color.FromArgb(60, 60, 60);
            lblType.Location = new Point(12, 84);
            lblType.Name = "lblType";
            lblType.Size = new Size(90, 20);
            lblType.TabIndex = 3;
            lblType.Text = "Event Type:";
            // 
            // cmbType
            // 
            cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbType.Font = new Font("Segoe UI", 9F);
            cmbType.Location = new Point(108, 81);
            cmbType.Name = "cmbType";
            cmbType.Size = new Size(180, 23);
            cmbType.TabIndex = 4;
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 9F);
            lblTitle.ForeColor = Color.FromArgb(60, 60, 60);
            lblTitle.Location = new Point(12, 118);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(90, 20);
            lblTitle.TabIndex = 5;
            lblTitle.Text = "Title: *";
            // 
            // txtTitle
            // 
            txtTitle.Font = new Font("Segoe UI", 9F);
            txtTitle.Location = new Point(108, 115);
            txtTitle.Name = "txtTitle";
            txtTitle.PlaceholderText = "Required";
            txtTitle.Size = new Size(286, 23);
            txtTitle.TabIndex = 6;
            txtTitle.Tag = "";
            // 
            // lblDesc
            // 
            lblDesc.Font = new Font("Segoe UI", 9F);
            lblDesc.ForeColor = Color.FromArgb(60, 60, 60);
            lblDesc.Location = new Point(12, 152);
            lblDesc.Name = "lblDesc";
            lblDesc.Size = new Size(90, 20);
            lblDesc.TabIndex = 7;
            lblDesc.Text = "Description:";
            // 
            // txtDesc
            // 
            txtDesc.Font = new Font("Segoe UI", 9F);
            txtDesc.Location = new Point(108, 149);
            txtDesc.Multiline = true;
            txtDesc.Name = "txtDesc";
            txtDesc.PlaceholderText = "Optional";
            txtDesc.ScrollBars = ScrollBars.Vertical;
            txtDesc.Size = new Size(286, 60);
            txtDesc.TabIndex = 8;
            // 
            // lblStart
            // 
            lblStart.Font = new Font("Segoe UI", 9F);
            lblStart.ForeColor = Color.FromArgb(60, 60, 60);
            lblStart.Location = new Point(12, 223);
            lblStart.Name = "lblStart";
            lblStart.Size = new Size(90, 20);
            lblStart.TabIndex = 9;
            lblStart.Text = "Start Time:";
            // 
            // dtpStart
            // 
            dtpStart.CustomFormat = "h:mm tt";
            dtpStart.Font = new Font("Segoe UI", 9F);
            dtpStart.Format = DateTimePickerFormat.Custom;
            dtpStart.Location = new Point(108, 220);
            dtpStart.Name = "dtpStart";
            dtpStart.ShowCheckBox = true;
            dtpStart.ShowUpDown = true;
            dtpStart.Size = new Size(180, 23);
            dtpStart.TabIndex = 10;
            // 
            // lblEnd
            // 
            lblEnd.Font = new Font("Segoe UI", 9F);
            lblEnd.ForeColor = Color.FromArgb(60, 60, 60);
            lblEnd.Location = new Point(12, 257);
            lblEnd.Name = "lblEnd";
            lblEnd.Size = new Size(90, 20);
            lblEnd.TabIndex = 11;
            lblEnd.Text = "End Time:";
            // 
            // dtpEnd
            // 
            dtpEnd.CustomFormat = "h:mm tt";
            dtpEnd.Font = new Font("Segoe UI", 9F);
            dtpEnd.Format = DateTimePickerFormat.Custom;
            dtpEnd.Location = new Point(108, 254);
            dtpEnd.Name = "dtpEnd";
            dtpEnd.ShowCheckBox = true;
            dtpEnd.ShowUpDown = true;
            dtpEnd.Size = new Size(180, 23);
            dtpEnd.TabIndex = 12;
            // 
            // lblRoom
            // 
            lblRoom.Font = new Font("Segoe UI", 9F);
            lblRoom.ForeColor = Color.FromArgb(60, 60, 60);
            lblRoom.Location = new Point(12, 291);
            lblRoom.Name = "lblRoom";
            lblRoom.Size = new Size(90, 20);
            lblRoom.TabIndex = 13;
            lblRoom.Text = "Room:";
            // 
            // txtRoom
            // 
            txtRoom.Font = new Font("Segoe UI", 9F);
            txtRoom.Location = new Point(108, 288);
            txtRoom.Name = "txtRoom";
            txtRoom.PlaceholderText = "Optional";
            txtRoom.Size = new Size(180, 23);
            txtRoom.TabIndex = 14;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.Maroon;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9F);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(228, 328);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(80, 32);
            btnSave.TabIndex = 15;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 9F);
            btnCancel.Location = new Point(316, 328);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(80, 32);
            btnCancel.TabIndex = 16;
            btnCancel.Text = "Cancel";
            btnCancel.Click += btnCancel_Click;
            // 
            // AddEventForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(414, 378);
            Controls.Add(pnlHeader);
            Controls.Add(lblDate);
            Controls.Add(lblDateValue);
            Controls.Add(lblType);
            Controls.Add(cmbType);
            Controls.Add(lblTitle);
            Controls.Add(txtTitle);
            Controls.Add(lblDesc);
            Controls.Add(txtDesc);
            Controls.Add(lblStart);
            Controls.Add(dtpStart);
            Controls.Add(lblEnd);
            Controls.Add(dtpEnd);
            Controls.Add(lblRoom);
            Controls.Add(txtRoom);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AddEventForm";
            StartPosition = FormStartPosition.CenterParent;
            Load += AddEventForm_Load;
            pnlHeader.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion


        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblDateValue;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Label lblRoom;
        private System.Windows.Forms.TextBox txtRoom;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}