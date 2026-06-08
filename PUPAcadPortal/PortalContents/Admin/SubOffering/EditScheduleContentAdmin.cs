using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using PUPAcadPortal.Models;
using PUPAcadPortal.Services;
using System.Drawing.Text;


namespace PUPAcadPortal.PortalContents.Admin.SubOffering
{
    public partial class EditScheduleContentAdmin : UserControl
    {
        private void EditScheduleContentAdmin_Load(object sender, EventArgs e)
        {
            LoadDropdowns();
            GridFilter();
            _Isloading = false;
        }


        public EditScheduleContentAdmin()
        {
            InitializeComponent();

            _scheduleService = new ScheduleService();

            cmbESYearLevel.SelectedIndexChanged += FilterChanged;
            cmbESCurrentSem.SelectedIndexChanged += FilterChanged;
            cmbESRevYear.SelectedIndexChanged += FilterChanged;



        }
        private readonly ScheduleService _scheduleService;

        private void GridFilter()
        {
            int year = (int)cmbESYearLevel.SelectedValue;
            int sem = (int)cmbESCurrentSem.SelectedValue;
            int rev = (int)cmbESRevYear.SelectedValue;

            dgvEditSchedule.DataSource =
                _scheduleService.GetFilteredSchedule(year, sem, rev);

        }

        private void RefreshGrid()
        {
            int year = (int)cmbESYearLevel.SelectedValue;
            int sem = (int)cmbESCurrentSem.SelectedValue;
            int rev = (int)cmbESRevYear.SelectedValue;

            dgvEditSchedule.DataSource =
                _scheduleService.GetFilteredSchedule(year, sem, rev);
        }
        private void LoadDropdowns()
        {
            ESRoom.DataSource = _scheduleService.GetRooms();
            ESRoom.DisplayMember = "RoomName";
            ESRoom.ValueMember = "RoomId";

            ESInstructor.DataSource = _scheduleService.GetProfessors();
            ESInstructor.DisplayMember = "FullName";
            ESInstructor.ValueMember = "ProfessorId";
        }



        private void DuplicateRow()
        {
            if (dgvEditSchedule.CurrentRow == null) return;

            string subjectCode = Convert.ToString(dgvEditSchedule.CurrentRow.Cells["SubjectCode"].Value);

            if (string.IsNullOrEmpty(subjectCode)) return;

            _scheduleService.DuplicateOffering(subjectCode);

            GridFilter();
        }

        private bool _Isloading = true;
        private void FilterChanged(object sender, EventArgs e)
        {
            if (_Isloading) return;
            GridFilter();
        }
        private void dgvEditSchedule_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmbESCurrentSem_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void cmbESYearLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void cmbESRevYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void DupeRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DuplicateRow();
        }
    }
}