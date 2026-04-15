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
            lblFormTitle = new Label();
            lblDateCaption = new Label();
            lblDateValue = new Label();
            lblType = new Label();
            cmbType = new ComboBox();
            lblTitle = new Label();
            txtTitle = new TextBox();
            lblStartTime = new Label();
            dtpStart = new DateTimePicker();
            dtpEnd = new DateTimePicker();
            lblRoom = new Label();
            txtRoom = new TextBox();
            lblDesc = new Label();
            txtDesc = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            lblEndTime = new Label();
            pnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.Maroon;
            pnlHeader.Controls.Add(lblFormTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(414, 46);
            pnlHeader.TabIndex = 0;
            // 
            // lblFormTitle
            // 
            lblFormTitle.Dock = DockStyle.Fill;
            lblFormTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblFormTitle.ForeColor = Color.White;
            lblFormTitle.Location = new Point(0, 0);
            lblFormTitle.Name = "lblFormTitle";
            lblFormTitle.Padding = new Padding(12, 0, 0, 0);
            lblFormTitle.Size = new Size(414, 46);
            lblFormTitle.TabIndex = 0;
            lblFormTitle.Text = "Add Calendar Event";
            lblFormTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDateCaption
            // 
            lblDateCaption.AutoSize = true;
            lblDateCaption.ForeColor = Color.FromArgb(60, 60, 60);
            lblDateCaption.Location = new Point(16, 63);
            lblDateCaption.Name = "lblDateCaption";
            lblDateCaption.Size = new Size(38, 17);
            lblDateCaption.TabIndex = 1;
            lblDateCaption.Text = "Date:";
            // 
            // lblDateValue
            // 
            lblDateValue.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblDateValue.ForeColor = Color.FromArgb(50, 50, 50);
            lblDateValue.Location = new Point(148, 62);
            lblDateValue.Name = "lblDateValue";
            lblDateValue.Size = new Size(238, 20);
            lblDateValue.TabIndex = 2;
            // 
            // lblType
            // 
            lblType.AutoSize = true;
            lblType.ForeColor = Color.FromArgb(60, 60, 60);
            lblType.Location = new Point(16, 103);
            lblType.Name = "lblType";
            lblType.Size = new Size(38, 17);
            lblType.TabIndex = 3;
            lblType.Text = "Type:";
            // 
            // cmbType
            // 
            cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbType.Location = new Point(148, 100);
            cmbType.Name = "cmbType";
            cmbType.Size = new Size(238, 25);
            cmbType.TabIndex = 4;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.ForeColor = Color.FromArgb(60, 60, 60);
            lblTitle.Location = new Point(16, 141);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(35, 17);
            lblTitle.TabIndex = 5;
            lblTitle.Text = "Title:";
            // 
            // txtTitle
            // 
            txtTitle.Location = new Point(148, 138);
            txtTitle.Name = "txtTitle";
            txtTitle.PlaceholderText = "Required";
            txtTitle.Size = new Size(238, 24);
            txtTitle.TabIndex = 6;
            // 
            // lblStartTime
            // 
            lblStartTime.AutoSize = true;
            lblStartTime.ForeColor = Color.FromArgb(60, 60, 60);
            lblStartTime.Location = new Point(16, 179);
            lblStartTime.Name = "lblStartTime";
            lblStartTime.Size = new Size(67, 17);
            lblStartTime.TabIndex = 7;
            lblStartTime.Text = "Start time:";
            // 
            // dtpStart
            // 
            dtpStart.Checked = false;
            dtpStart.CustomFormat = "hh:mm tt";
            dtpStart.Format = DateTimePickerFormat.Custom;
            dtpStart.Location = new Point(148, 176);
            dtpStart.Name = "dtpStart";
            dtpStart.ShowCheckBox = true;
            dtpStart.Size = new Size(110, 24);
            dtpStart.TabIndex = 8;
            // 
            // dtpEnd
            // 
            dtpEnd.Checked = false;
            dtpEnd.CustomFormat = "hh:mm tt";
            dtpEnd.Format = DateTimePickerFormat.Custom;
            dtpEnd.Location = new Point(148, 212);
            dtpEnd.Name = "dtpEnd";
            dtpEnd.ShowCheckBox = true;
            dtpEnd.Size = new Size(110, 24);
            dtpEnd.TabIndex = 10;
            // 
            // lblRoom
            // 
            lblRoom.AutoSize = true;
            lblRoom.ForeColor = Color.FromArgb(60, 60, 60);
            lblRoom.Location = new Point(16, 245);
            lblRoom.Name = "lblRoom";
            lblRoom.Size = new Size(46, 17);
            lblRoom.TabIndex = 11;
            lblRoom.Text = "Room:";
            // 
            // txtRoom
            // 
            txtRoom.Location = new Point(148, 242);
            txtRoom.Name = "txtRoom";
            txtRoom.PlaceholderText = "Optional";
            txtRoom.Size = new Size(238, 24);
            txtRoom.TabIndex = 12;
            // 
            // lblDesc
            // 
            lblDesc.AutoSize = true;
            lblDesc.ForeColor = Color.FromArgb(60, 60, 60);
            lblDesc.Location = new Point(16, 283);
            lblDesc.Name = "lblDesc";
            lblDesc.Size = new Size(77, 17);
            lblDesc.TabIndex = 13;
            lblDesc.Text = "Description:";
            // 
            // txtDesc
            // 
            txtDesc.Location = new Point(148, 280);
            txtDesc.Multiline = true;
            txtDesc.Name = "txtDesc";
            txtDesc.PlaceholderText = "Optional";
            txtDesc.ScrollBars = ScrollBars.Vertical;
            txtDesc.Size = new Size(238, 68);
            txtDesc.TabIndex = 14;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.Maroon;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(148, 364);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(112, 34);
            btnSave.TabIndex = 15;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 9.5F);
            btnCancel.Location = new Point(268, 364);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(112, 34);
            btnCancel.TabIndex = 16;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // lblEndTime
            // 
            lblEndTime.AutoSize = true;
            lblEndTime.ForeColor = Color.FromArgb(60, 60, 60);
            lblEndTime.Location = new Point(16, 212);
            lblEndTime.Name = "lblEndTime";
            lblEndTime.Size = new Size(62, 17);
            lblEndTime.TabIndex = 17;
            lblEndTime.Text = "End time:";
            // 
            // AddEventForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(414, 409);
            Controls.Add(lblEndTime);
            Controls.Add(lblDateCaption);
            Controls.Add(lblDateValue);
            Controls.Add(lblType);
            Controls.Add(cmbType);
            Controls.Add(lblTitle);
            Controls.Add(txtTitle);
            Controls.Add(lblStartTime);
            Controls.Add(dtpStart);
            Controls.Add(dtpEnd);
            Controls.Add(lblRoom);
            Controls.Add(txtRoom);
            Controls.Add(lblDesc);
            Controls.Add(txtDesc);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9.5F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AddEventForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Add Event";
            pnlHeader.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Label lblDateCaption;
        private System.Windows.Forms.Label lblDateValue;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Label lblRoom;
        private System.Windows.Forms.TextBox txtRoom;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private Label lblEndTime;
    }
}