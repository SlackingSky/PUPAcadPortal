using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using PUPAcadPortal.Services;

namespace PUPAcadPortal.PortalContents.Admin.SubOffering
{
    public partial class EditScheduleContentAdmin : UserControl
    {
        private readonly ScheduleService _scheduleService;
        private string _activePeriodId;
        private bool _isPeriodActiveLock = false;

        private List<RoomDto> _masterRooms;
        private List<ProfessorDto> _masterProfessors;
        private BindingList<ScheduleRowDto> _gridData;

        public EditScheduleContentAdmin()
        {
            InitializeComponent();
            _scheduleService = new ScheduleService();

            this.Load += EditScheduleContentAdmin_Load;

            dgvEditSchedule.CellBeginEdit += DgvEditSchedule_CellBeginEdit;
            dgvEditSchedule.CellEndEdit += DgvEditSchedule_CellEndEdit;
            dgvEditSchedule.DataError += DgvEditSchedule_DataError;
            dgvEditSchedule.CellValidating += DgvEditSchedule_CellValidating;
            dgvEditSchedule.CellValueChanged += DgvEditSchedule_CellValueChanged;

            DupeRowToolStripMenuItem.Text = "Duplicate Section (New Class)";
            var addSchedItem = new ToolStripMenuItem("Add Schedule Block (e.g. Lab)");
            addSchedItem.Click += AddSchedToolStripMenuItem_Click;
            cms1.Items.Add(addSchedItem);

            btnSaveSchedule.Click += btnSaveSchedule_Click;
            btnClearSchedule.Click += btnClearSchedule_Click;
        }

        private async void EditScheduleContentAdmin_Load(object sender, EventArgs e)
        {
            try
            {
                Application.UseWaitCursor = true;

                var period = await _scheduleService.GetLatestAcademicPeriodAsync();
                if (period != null)
                {
                    _activePeriodId = period.AcademicPeriodId;
                    lblYearLevel.Text = $"Year Level: {period.SchoolYear}";
                    lblCurrentSem.Text = $"Semester: {period.Semester}";

                    _isPeriodActiveLock = (period.Status == "Current");
                    if (_isPeriodActiveLock)
                    {
                        dgvEditSchedule.ReadOnly = true;
                        btnSaveSchedule.Enabled = false;
                        btnClearSchedule.Enabled = false;
                        lblCurrentSem.Text += " (LOCKED - ONGOING PERIOD)";
                    }
                }

                await LoadMasterDropdownsAsync();
                await RefreshGridAsync();
            }
            finally
            {
                Application.UseWaitCursor = false;
            }
        }

        private async Task LoadMasterDropdownsAsync()
        {
            _masterRooms = await _scheduleService.GetMasterRoomsAsync();
            _masterProfessors = await _scheduleService.GetMasterProfessorsAsync();

            if (dgvEditSchedule.Columns["ESRoom"] is DataGridViewComboBoxColumn roomCol)
            {
                roomCol.DataSource = _masterRooms;
                roomCol.DisplayMember = "RoomName";
                roomCol.ValueMember = "RoomId";
            }

            if (dgvEditSchedule.Columns["ESInstructor"] is DataGridViewComboBoxColumn profCol)
            {
                profCol.DataSource = _masterProfessors;
                profCol.DisplayMember = "FullName";
                profCol.ValueMember = "ProfessorId";
            }

            dgvEditSchedule.Columns["ESCourseCode"].DataPropertyName = "SubjectCode";
            dgvEditSchedule.Columns["ESCourseTitle"].DataPropertyName = "SubjectTitle";
            dgvEditSchedule.Columns["ESLab"].DataPropertyName = "Lab";
            dgvEditSchedule.Columns["ESLec"].DataPropertyName = "Lec";
            dgvEditSchedule.Columns["ESTotalUnits"].DataPropertyName = "TotalUnits";
            dgvEditSchedule.Columns["ESSection"].DataPropertyName = "Section";
            dgvEditSchedule.Columns["ESDay"].DataPropertyName = "Day";
            dgvEditSchedule.Columns["ESStartTime"].DataPropertyName = "StartTime";
            dgvEditSchedule.Columns["EsEndTime"].DataPropertyName = "EndTime";
            dgvEditSchedule.Columns["ESRoom"].DataPropertyName = "RoomId";
            dgvEditSchedule.Columns["ESInstructor"].DataPropertyName = "ProfessorId";

            dgvEditSchedule.Columns["ESStartTime"].DefaultCellStyle.Format = "hh\\:mm";
            dgvEditSchedule.Columns["EsEndTime"].DefaultCellStyle.Format = "hh\\:mm";
        }

        private async Task RefreshGridAsync()
        {
            if (string.IsNullOrEmpty(_activePeriodId)) return;

            var list = await _scheduleService.GetAllSchedulesAsync(_activePeriodId);
            _gridData = new BindingList<ScheduleRowDto>(list);

            dgvEditSchedule.AutoGenerateColumns = false;
            dgvEditSchedule.DataSource = _gridData;
        }

        private async void DgvEditSchedule_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (_isPeriodActiveLock) { e.Cancel = true; return; }

            var colName = dgvEditSchedule.Columns[e.ColumnIndex].Name;
            var row = dgvEditSchedule.Rows[e.RowIndex].DataBoundItem as ScheduleRowDto;
            if (row == null) return;

            if (colName == "ESRoom")
            {
                var cell = (DataGridViewComboBoxCell)dgvEditSchedule.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.DataSource = await _scheduleService.GetValidRoomsAsync(row, _activePeriodId);
            }
            else if (colName == "ESInstructor")
            {
                var cell = (DataGridViewComboBoxCell)dgvEditSchedule.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.DataSource = await _scheduleService.GetValidProfessorsAsync(row, _activePeriodId);
            }
        }

        private void DgvEditSchedule_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var colName = dgvEditSchedule.Columns[e.ColumnIndex].Name;

            if (colName == "ESRoom")
            {
                var cell = (DataGridViewComboBoxCell)dgvEditSchedule.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.DataSource = _masterRooms;
            }
            else if (colName == "ESInstructor")
            {
                var cell = (DataGridViewComboBoxCell)dgvEditSchedule.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.DataSource = _masterProfessors;
            }
        }

        private void DgvEditSchedule_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var colName = dgvEditSchedule.Columns[e.ColumnIndex].Name;
            var row = dgvEditSchedule.Rows[e.RowIndex].DataBoundItem as ScheduleRowDto;
            if (row == null) return;

            if (colName == "ESInstructor")
            {
                foreach (var item in _gridData.Where(x => x.SubjectOfferingId == row.SubjectOfferingId))
                {
                    item.ProfessorId = row.ProfessorId;
                }
                dgvEditSchedule.Refresh();
            }
        }

        private void DgvEditSchedule_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0 || _isPeriodActiveLock) return;
            var colName = dgvEditSchedule.Columns[e.ColumnIndex].Name;
            var formattedValue = e.FormattedValue?.ToString();

            if (colName == "ESStartTime" || colName == "EsEndTime")
            {
                if (!string.IsNullOrWhiteSpace(formattedValue) && !TimeSpan.TryParse(formattedValue, out _))
                {
                    MessageBox.Show("Invalid time format. Please use HH:MM format.", "Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                }
            }
        }

        private void DgvEditSchedule_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private async void DupeRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_isPeriodActiveLock || dgvEditSchedule.CurrentRow == null) return;
            var row = dgvEditSchedule.CurrentRow.DataBoundItem as ScheduleRowDto;
            if (row == null) return;

            try
            {
                Application.UseWaitCursor = true;
                await _scheduleService.DuplicateSectionAsync(row.SubjectOfferingId);
                await RefreshGridAsync();
            }
            finally
            {
                Application.UseWaitCursor = false;
            }
        }

        private async void AddSchedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_isPeriodActiveLock || dgvEditSchedule.CurrentRow == null) return;
            var row = dgvEditSchedule.CurrentRow.DataBoundItem as ScheduleRowDto;
            if (row == null) return;

            try
            {
                Application.UseWaitCursor = true;
                await _scheduleService.AddScheduleBlockAsync(row.SubjectOfferingId);
                await RefreshGridAsync();
            }
            finally
            {
                Application.UseWaitCursor = false;
            }
        }

        private async void btnSaveSchedule_Click(object sender, EventArgs e)
        {
            if (_isPeriodActiveLock) return;

            dgvEditSchedule.EndEdit();

            try
            {
                btnSaveSchedule.Enabled = false;
                Application.UseWaitCursor = true;

                await _scheduleService.SaveChangesAsync(_gridData.ToList());
                MessageBox.Show("Schedules Saved Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                await RefreshGridAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save schedules: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSaveSchedule.Enabled = true;
                Application.UseWaitCursor = false;
            }
        }

        private void btnClearSchedule_Click(object sender, EventArgs e)
        {
            if (_isPeriodActiveLock || dgvEditSchedule.CurrentRow == null) return;
            var row = dgvEditSchedule.CurrentRow.DataBoundItem as ScheduleRowDto;

            if (row != null)
            {
                row.Day = null;
                row.StartTime = null;
                row.EndTime = null;
                row.RoomId = null;
                dgvEditSchedule.Refresh();
            }
        }
    }
}