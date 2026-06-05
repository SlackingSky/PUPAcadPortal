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
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
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
            dgvRooms.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvRooms.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRooms.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dgvRooms.BackgroundColor = Color.White;
            dgvRooms.CellBorderStyle = DataGridViewCellBorderStyle.RaisedHorizontal;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvRooms.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvRooms.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRooms.Columns.AddRange(new DataGridViewColumn[] { colRoomName, colBuilding, colCapacity, colType, colStatus });
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = SystemColors.Window;
            dataGridViewCellStyle7.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle7.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.False;
            dgvRooms.DefaultCellStyle = dataGridViewCellStyle7;
            dgvRooms.Location = new Point(20, 68);
            dgvRooms.Name = "dgvRooms";
            dgvRooms.ReadOnly = true;
            dgvRooms.RowHeadersVisible = false;
            dgvRooms.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvRooms.ScrollBars = ScrollBars.Vertical;
            dgvRooms.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRooms.Size = new Size(760, 338);
            dgvRooms.TabIndex = 0;
            // 
            // colRoomName
            // 
            colRoomName.DataPropertyName = "RoomName";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            colRoomName.DefaultCellStyle = dataGridViewCellStyle2;
            colRoomName.FillWeight = 200F;
            colRoomName.HeaderText = "Room Name";
            colRoomName.Name = "colRoomName";
            colRoomName.ReadOnly = true;
            colRoomName.Resizable = DataGridViewTriState.False;
            // 
            // colBuilding
            // 
            colBuilding.DataPropertyName = "Building";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            colBuilding.DefaultCellStyle = dataGridViewCellStyle3;
            colBuilding.HeaderText = "Building";
            colBuilding.Name = "colBuilding";
            colBuilding.ReadOnly = true;
            colBuilding.Resizable = DataGridViewTriState.False;
            // 
            // colCapacity
            // 
            colCapacity.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            colCapacity.DataPropertyName = "Capacity";
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colCapacity.DefaultCellStyle = dataGridViewCellStyle4;
            colCapacity.FillWeight = 50F;
            colCapacity.HeaderText = "Capacity";
            colCapacity.Name = "colCapacity";
            colCapacity.ReadOnly = true;
            colCapacity.Resizable = DataGridViewTriState.False;
            colCapacity.Width = 94;
            // 
            // colType
            // 
            colType.DataPropertyName = "RoomType";
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colType.DefaultCellStyle = dataGridViewCellStyle5;
            colType.HeaderText = "Type";
            colType.Name = "colType";
            colType.ReadOnly = true;
            colType.Resizable = DataGridViewTriState.False;
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colStatus.DefaultCellStyle = dataGridViewCellStyle6;
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colStatus.Resizable = DataGridViewTriState.False;
            // 
            // btnAddRoom
            // 
            btnAddRoom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddRoom.BackColor = Color.Maroon;
            btnAddRoom.FlatStyle = FlatStyle.Flat;
            btnAddRoom.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAddRoom.ForeColor = Color.White;
            btnAddRoom.Location = new Point(628, 10);
            btnAddRoom.Name = "btnAddRoom";
            btnAddRoom.Size = new Size(152, 42);
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
            BackColor = Color.WhiteSmoke;
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
        private DataGridViewTextBoxColumn colRoomName;
        private DataGridViewTextBoxColumn colBuilding;
        private DataGridViewTextBoxColumn colCapacity;
        private DataGridViewTextBoxColumn colType;
        private DataGridViewTextBoxColumn colStatus;
    }
}