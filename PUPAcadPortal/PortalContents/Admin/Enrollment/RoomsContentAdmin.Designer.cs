namespace PUPAcadPortal.PortalContents.Admin.Enrollment
{
    partial class RoomsContentAdmin
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            dgvRooms = new DataGridView();
            colRoomName = new DataGridViewTextBoxColumn();
            colBuilding = new DataGridViewTextBoxColumn();
            colCapacity = new DataGridViewTextBoxColumn();
            colType = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            btnAddRoom = new Button();
            lblTitle = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvRooms).BeginInit();
            SuspendLayout();
            // 
            // dgvRooms
            // 
            dgvRooms.AllowUserToAddRows = false;
            dgvRooms.AllowUserToDeleteRows = false;
            dgvRooms.AllowUserToResizeColumns = false;
            dgvRooms.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 248, 248);
            dgvRooms.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvRooms.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvRooms.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRooms.BackgroundColor = Color.White;
            dgvRooms.BorderStyle = BorderStyle.None;
            dgvRooms.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(128, 0, 0);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(128, 0, 0);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvRooms.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvRooms.ColumnHeadersHeight = 40;
            dgvRooms.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvRooms.Columns.AddRange(new DataGridViewColumn[] { colRoomName, colBuilding, colCapacity, colType, colStatus });
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = Color.White;
            dataGridViewCellStyle8.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle8.ForeColor = Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = Color.FromArgb(128, 0, 0);
            dataGridViewCellStyle8.SelectionForeColor = Color.White;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            dgvRooms.DefaultCellStyle = dataGridViewCellStyle8;
            dgvRooms.EnableHeadersVisualStyles = false;
            dgvRooms.GridColor = Color.FromArgb(220, 220, 220);
            dgvRooms.Location = new Point(20, 68);
            dgvRooms.Name = "dgvRooms";
            dgvRooms.ReadOnly = true;
            dgvRooms.RowHeadersVisible = false;
            dgvRooms.RowTemplate.Height = 35;
            dgvRooms.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRooms.Size = new Size(760, 338);
            dgvRooms.TabIndex = 0;
            // 
            // colRoomName
            // 
            colRoomName.DataPropertyName = "RoomName";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            colRoomName.DefaultCellStyle = dataGridViewCellStyle3;
            colRoomName.FillWeight = 40F;
            colRoomName.HeaderText = "";
            colRoomName.Name = "colRoomName";
            colRoomName.ReadOnly = true;
            // 
            // colBuilding
            // 
            colBuilding.DataPropertyName = "Building";
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            colBuilding.DefaultCellStyle = dataGridViewCellStyle4;
            colBuilding.HeaderText = "Building";
            colBuilding.Name = "colBuilding";
            colBuilding.ReadOnly = true;
            // 
            // colCapacity
            // 
            colCapacity.DataPropertyName = "Capacity";
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colCapacity.DefaultCellStyle = dataGridViewCellStyle5;
            colCapacity.FillWeight = 50F;
            colCapacity.HeaderText = "Capacity";
            colCapacity.Name = "colCapacity";
            colCapacity.ReadOnly = true;
            // 
            // colType
            // 
            colType.DataPropertyName = "RoomType";
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colType.DefaultCellStyle = dataGridViewCellStyle6;
            colType.FillWeight = 80F;
            colType.HeaderText = "Type";
            colType.Name = "colType";
            colType.ReadOnly = true;
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colStatus.DefaultCellStyle = dataGridViewCellStyle7;
            colStatus.FillWeight = 60F;
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            // 
            // btnAddRoom
            // 
            btnAddRoom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddRoom.BackColor = Color.FromArgb(128, 0, 0);
            btnAddRoom.FlatAppearance.BorderSize = 0;
            btnAddRoom.FlatStyle = FlatStyle.Flat;
            btnAddRoom.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAddRoom.ForeColor = Color.White;
            btnAddRoom.Location = new Point(628, 10);
            btnAddRoom.Name = "btnAddRoom";
            btnAddRoom.Size = new Size(152, 35);
            btnAddRoom.TabIndex = 1;
            btnAddRoom.Text = "+ Add Room";
            btnAddRoom.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.Location = new Point(15, 13);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(216, 30);
            lblTitle.TabIndex = 2;
            lblTitle.Text = "Room Management";
            // 
            // RoomsContentAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(lblTitle);
            Controls.Add(btnAddRoom);
            Controls.Add(dgvRooms);
            Name = "RoomsContentAdmin";
            Size = new Size(800, 441);
            ((System.ComponentModel.ISupportInitialize)dgvRooms).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.DataGridView dgvRooms;
        private System.Windows.Forms.Button btnAddRoom;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRoomName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBuilding;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCapacity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
    }
}