using Org.BouncyCastle.Asn1.Cmp;
using PUPAcadPortal.Models;
using PUPAcadPortal.Services;
using PUPAcadPortal.Data;
using System;
using System.Windows.Forms;
using System.Linq.Expressions;

namespace PUPAcadPortal.PortalContents.Admin.Enrollment
{
    public partial class RoomEditForm : Form
    {
        private RoomData _currentRoom;
        private RoomService _service = new RoomService();
        public RoomEditForm(RoomData room)
        {
            InitializeComponent();

            _currentRoom = room ?? new RoomData();

            this.Text = (room == null) ? "Add New Room" : "Edit Room Details";
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to save this room?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            if (string.IsNullOrWhiteSpace(txtRoomName.Text))
            {
                MessageBox.Show("Please enter a room name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _currentRoom.RoomName = txtRoomName.Text.Trim();
            _currentRoom.Building = txtBuilding.Text.Trim();
            _currentRoom.Capacity = (int)numCapacity.Value;
            _currentRoom.RoomType = cmbRoomType.Text;
            _currentRoom.Status = cmbStatus.Text;

            try
            {
                btnSave.Enabled = false;
                await _service.SaveRoomAsync(_currentRoom);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving room: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private async void RoomEditForm_Load(object sender, EventArgs e)
        {
            if (_currentRoom.RoomId != 0)
            {
                txtRoomName.Text = _currentRoom.RoomName;
                txtBuilding.Text = _currentRoom.Building;
                numCapacity.Value = _currentRoom.Capacity;
                cmbRoomType.Text = _currentRoom.RoomType;
                cmbStatus.Text = _currentRoom.Status;
            }
            else
            {

                txtBuilding.Text = "Main Building";
                numCapacity.Value = 50;

                cmbRoomType.SelectedIndex = 0;
                cmbStatus.SelectedIndex = 0;
            }
        }
    }
}