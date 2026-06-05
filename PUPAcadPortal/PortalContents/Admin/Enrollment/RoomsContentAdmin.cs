using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using PUPAcadPortal.PortalContents.Admin.Enrollment;
using PUPAcadPortal.Services;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Admin.Enrollment
{
    public partial class RoomsContentAdmin : UserControl
    {
        private RoomService _roomService = new RoomService();

        public RoomsContentAdmin()
        {
            InitializeComponent();

            this.Load += async (s, e) => await LoadRoomsAsync();

            btnAddRoom.Click += btnAddRoom_Click;

            dgvRooms.CellDoubleClick += dgvRooms_CellDoubleClick;
        }

        private async Task LoadRoomsAsync()
        {
            try
            {
                var rooms = await _roomService.GetAllRoomsAsync();

                dgvRooms.AutoGenerateColumns = false;
                dgvRooms.DataSource = new BindingList<RoomData>(rooms);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading room data: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddRoom_Click(object sender, EventArgs e)
        {
            OpenRoomForm(null);
        }

        private void dgvRooms_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var selectedRoom = dgvRooms.Rows[e.RowIndex].DataBoundItem as RoomData;

            if (selectedRoom != null)
            {
                OpenRoomForm(selectedRoom);
            }
        }

        private async void OpenRoomForm(RoomData room)
        {
            using (var editForm = new RoomEditForm(room))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    await LoadRoomsAsync();
                }
            }
        }
    }
}