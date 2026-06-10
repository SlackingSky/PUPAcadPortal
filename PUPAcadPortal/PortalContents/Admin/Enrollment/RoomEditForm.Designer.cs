namespace PUPAcadPortal.PortalContents.Admin.Enrollment
{
    partial class RoomEditForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblHeaderTitle = new Label();
            txtRoomName = new TextBox();
            txtBuilding = new TextBox();
            numCapacity = new NumericUpDown();
            cmbRoomType = new ComboBox();
            cmbStatus = new ComboBox();
            btnSave = new Button();
            btnCancel = new Button();
            lblRoomName = new Label();
            lblBuilding = new Label();
            lblCapacity = new Label();
            lblType = new Label();
            lblStatus = new Label();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numCapacity).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(128, 0, 0);
            pnlHeader.Controls.Add(lblHeaderTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(350, 50);
            pnlHeader.TabIndex = 0;
            // 
            // lblHeaderTitle
            // 
            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHeaderTitle.ForeColor = Color.Gold;
            lblHeaderTitle.Location = new Point(15, 15);
            lblHeaderTitle.Name = "lblHeaderTitle";
            lblHeaderTitle.Size = new Size(112, 21);
            lblHeaderTitle.TabIndex = 0;
            lblHeaderTitle.Text = "Room Details";
            // 
            // txtRoomName
            // 
            txtRoomName.Location = new Point(140, 70);
            txtRoomName.Name = "txtRoomName";
            txtRoomName.Size = new Size(180, 25);
            txtRoomName.TabIndex = 1;
            // 
            // txtBuilding
            // 
            txtBuilding.Location = new Point(140, 110);
            txtBuilding.Name = "txtBuilding";
            txtBuilding.Size = new Size(180, 25);
            txtBuilding.TabIndex = 2;
            // 
            // numCapacity
            // 
            numCapacity.Location = new Point(140, 150);
            numCapacity.Name = "numCapacity";
            numCapacity.Size = new Size(180, 25);
            numCapacity.TabIndex = 3;
            // 
            // cmbRoomType
            // 
            cmbRoomType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRoomType.FormattingEnabled = true;
            cmbRoomType.Items.AddRange(new object[] { "Lecture Room", "Computer Laboratory", "Office", "Storage" });
            cmbRoomType.Location = new Point(140, 190);
            cmbRoomType.Name = "cmbRoomType";
            cmbRoomType.Size = new Size(180, 25);
            cmbRoomType.TabIndex = 4;
            // 
            // cmbStatus
            // 
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.FormattingEnabled = true;
            cmbStatus.Items.AddRange(new object[] { "Available", "Maintenance", "Occupied" });
            cmbStatus.Location = new Point(140, 230);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(180, 25);
            cmbStatus.TabIndex = 5;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(128, 0, 0);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(140, 280);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(85, 35);
            btnSave.TabIndex = 6;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.LightGray;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 9.75F);
            btnCancel.Location = new Point(235, 280);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(85, 35);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // lblRoomName
            // 
            lblRoomName.AutoSize = true;
            lblRoomName.Location = new Point(20, 73);
            lblRoomName.Name = "lblRoomName";
            lblRoomName.Size = new Size(85, 17);
            lblRoomName.TabIndex = 8;
            lblRoomName.Text = "Room Name:";
            // 
            // lblBuilding
            // 
            lblBuilding.AutoSize = true;
            lblBuilding.Location = new Point(20, 113);
            lblBuilding.Name = "lblBuilding";
            lblBuilding.Size = new Size(57, 17);
            lblBuilding.TabIndex = 9;
            lblBuilding.Text = "Building:";
            // 
            // lblCapacity
            // 
            lblCapacity.AutoSize = true;
            lblCapacity.Location = new Point(20, 153);
            lblCapacity.Name = "lblCapacity";
            lblCapacity.Size = new Size(60, 17);
            lblCapacity.TabIndex = 10;
            lblCapacity.Text = "Capacity:";
            // 
            // lblType
            // 
            lblType.AutoSize = true;
            lblType.Location = new Point(20, 193);
            lblType.Name = "lblType";
            lblType.Size = new Size(77, 17);
            lblType.TabIndex = 11;
            lblType.Text = "Room Type:";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(20, 233);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(46, 17);
            lblStatus.TabIndex = 12;
            lblStatus.Text = "Status:";
            // 
            // RoomEditForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(350, 340);
            Controls.Add(pnlHeader);
            Controls.Add(txtRoomName);
            Controls.Add(txtBuilding);
            Controls.Add(numCapacity);
            Controls.Add(cmbRoomType);
            Controls.Add(cmbStatus);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            Controls.Add(lblRoomName);
            Controls.Add(lblBuilding);
            Controls.Add(lblCapacity);
            Controls.Add(lblType);
            Controls.Add(lblStatus);
            Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "RoomEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Load += RoomEditForm_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numCapacity).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.TextBox txtRoomName;
        private System.Windows.Forms.TextBox txtBuilding;
        private System.Windows.Forms.NumericUpDown numCapacity;
        private System.Windows.Forms.ComboBox cmbRoomType;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblRoomName;
        private System.Windows.Forms.Label lblBuilding;
        private System.Windows.Forms.Label lblCapacity;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblStatus;
    }
}