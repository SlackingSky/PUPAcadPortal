using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using PUPAcadPortal.Services;
using PUPAcadPortal.Utils;

namespace PUPAcadPortal.PortalContents.Admin.SubOffering
{
    public partial class EditScheduleContentAdmin : UserControl
    {
        private readonly ScheduleService _scheduleService;
        private string _activePeriodId;
        private bool _isPeriodActiveLock = false;
        private Button[] _buttons;

        private List<RoomDto> _masterRooms;
        private List<ProfessorDto> _masterProfessors;

        private List<ScheduleRowDto> _allSchedules;
        private BindingList<ScheduleRowDto> _gridData;

        public EditScheduleContentAdmin()
        {
            InitializeComponent();
            _scheduleService = new ScheduleService();

            this.Load += EditScheduleContentAdmin_Load;

            dgvEditSchedule.EditMode = DataGridViewEditMode.EditOnEnter; 
            dgvEditSchedule.CellClick += DgvEditSchedule_CellClick;

            dgvEditSchedule.CellBeginEdit += DgvEditSchedule_CellBeginEdit;
            dgvEditSchedule.CellEndEdit += DgvEditSchedule_CellEndEdit;
            dgvEditSchedule.DataError += DgvEditSchedule_DataError;
            dgvEditSchedule.CellValidating += DgvEditSchedule_CellValidating;
            dgvEditSchedule.CellValueChanged += DgvEditSchedule_CellValueChanged;
            dgvEditSchedule.CellMouseUp += DgvEditSchedule_CellMouseUp;

            DupeRowToolStripMenuItem.Text = "Duplicate Section (New Class)";
            var addSchedItem = new ToolStripMenuItem("Add Schedule Block (e.g. Lab)");
            addSchedItem.Click += AddSchedToolStripMenuItem_Click;
            cms1.Items.Add(addSchedItem);
            var deleteRowItem = new ToolStripMenuItem("Delete Row (Section/Schedule)");
            deleteRowItem.Click += DeleteRowToolStripMenuItem_Click;
            cms1.Items.Add(deleteRowItem);

            btnSaveSchedule.Click += btnSaveSchedule_Click;
            btnClearSchedule.Click += btnClearSchedule_Click;
            btnLoadPrevious.Click += btnLoadPrevious_Click;

            if (cmbYearLevel != null)
            {
                cmbYearLevel.SelectedIndexChanged += CmbYearLevel_SelectedIndexChanged;
            }
            _buttons = [ btnClearSchedule, btnLoadPrevious, btnSaveSchedule ]; 
        }

        private async void btnLoadPrevious_Click(object sender, EventArgs e)
        {
            if (_isPeriodActiveLock || string.IsNullOrEmpty(_activePeriodId)) return;

            var confirm = MessageBox.Show(
                "Are you sure you want to load the schedules from the previous academic year?\n\n" +
                "This will only apply to classes that are currently empty. It will NOT overwrite any schedules you have already made.",
                "Confirm Smart Copy", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (confirm != DialogResult.Yes) return;

            try
            {
                dgvEditSchedule.EndEdit();

                var result = await _scheduleService.LoadPreviousYearScheduleAsync(_activePeriodId);

                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Schedules Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    await RefreshGridAsync();
                }
                else
                {
                    MessageBox.Show(result.Message, "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while copying schedules: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLoadPrevious.Enabled = true;
                Application.UseWaitCursor = false;
            }
        }

        private void DgvEditSchedule_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_isPeriodActiveLock || e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvEditSchedule.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn)
            {
                if (dgvEditSchedule.EditingControl is ComboBox cb)
                {
                    cb.DroppedDown = true;
                }
            }
        }

        private void CmbYearLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (_allSchedules == null) return;

            dgvEditSchedule.EditMode = DataGridViewEditMode.EditProgrammatically;   
            string selectedFilter = cmbYearLevel.SelectedItem?.ToString() ?? "All";
            IEnumerable<ScheduleRowDto> filtered = _allSchedules;

            if (selectedFilter != "All")
            {
                if (int.TryParse(selectedFilter.Substring(0, 1), out int year))
                {
                    filtered = filtered.Where(x => x.YearLevel == year);
                }
            }

            _gridData = new BindingList<ScheduleRowDto>(filtered.ToList());
            dgvEditSchedule.DataSource = _gridData;

            dgvEditSchedule.CurrentCell = null;
            dgvEditSchedule.ClearSelection();
            dgvEditSchedule.EditMode = DataGridViewEditMode.EditOnEnter;

        }

        private async void EditScheduleContentAdmin_Load(object sender, EventArgs e)
        {
            await SafeUiRunner.ExecuteAsync(async () =>
            {
                var period = await _scheduleService.GetLatestAcademicPeriodAsync();

                if (this.IsDisposed) return;
                if (period != null)
                {
                    _activePeriodId = period.AcademicPeriodId;
                    lblCurrentSem.Text = $"{period.Semester} Semester School Year {period.SchoolYear}";

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

                if (this.IsDisposed) return;
                if (cmbYearLevel.Items.Count > 0) cmbYearLevel.SelectedIndex = 0;

                await RefreshGridAsync();
            }, _buttons);
        }

        private async Task LoadMasterDropdownsAsync()
        {
            _masterRooms = await _scheduleService.GetMasterRoomsAsync();
            _masterProfessors = await _scheduleService.GetMasterProfessorsAsync();

            if (this.IsDisposed) return;

            if (!dgvEditSchedule.Columns.Contains("ESYearLevel"))
            {
                dgvEditSchedule.Columns.Insert(0, new DataGridViewTextBoxColumn
                {
                    Name = "ESYearLevel",
                    HeaderText = "Year",
                    DataPropertyName = "YearLevel",
                    ReadOnly = true
                });
            }

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

            dgvEditSchedule.Columns["ESYearLevel"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvEditSchedule.Columns["ESCourseCode"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dgvEditSchedule.Columns["ESCourseTitle"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvEditSchedule.Columns["ESLab"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            dgvEditSchedule.Columns["ESLec"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            dgvEditSchedule.Columns["ESTotalUnits"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            dgvEditSchedule.Columns["ESSection"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvEditSchedule.Columns["ESDay"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvEditSchedule.Columns["ESStartTime"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvEditSchedule.Columns["EsEndTime"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvEditSchedule.Columns["ESRoom"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvEditSchedule.Columns["ESInstructor"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private async Task RefreshGridAsync()
        {
            if (string.IsNullOrEmpty(_activePeriodId)) return;

            await SafeUiRunner.ExecuteAsync(async () =>
            {
                _allSchedules = await _scheduleService.GetAllSchedulesAsync(_activePeriodId);
                this.SafeUIUpdate(() =>
                {
                    dgvEditSchedule.AutoGenerateColumns = false;
                    ApplyFilter();
                });
            }, _buttons);
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
                foreach (var item in _allSchedules.Where(x => x.SubjectOfferingId == row.SubjectOfferingId))
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

        private void DgvEditSchedule_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    dgvEditSchedule.ClearSelection();
                    dgvEditSchedule.Rows[e.RowIndex].Selected = true;
                    dgvEditSchedule.CurrentCell = dgvEditSchedule.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    cms1.Show(Cursor.Position);
                }
            }
        }

        private async void DupeRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_isPeriodActiveLock || dgvEditSchedule.CurrentRow == null) return;
            var row = dgvEditSchedule.CurrentRow.DataBoundItem as ScheduleRowDto;
            if (row == null) return;

            await SafeUiRunner.ExecuteAsync(async () =>
            {
                await _scheduleService.DuplicateSectionAsync(row.SubjectOfferingId);
                await RefreshGridAsync();
            }, _buttons);
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

        private async void DeleteRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_isPeriodActiveLock || dgvEditSchedule.CurrentRow == null) return;

            var row = dgvEditSchedule.CurrentRow.DataBoundItem as ScheduleRowDto;
            if (row == null) return;

            var confirm = MessageBox.Show(
                "Are you sure you want to delete this row?\n\nIf it has a schedule, only the schedule block will be removed. If it is an empty section, the entire class section will be permanently deleted.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                Application.UseWaitCursor = true;
                await _scheduleService.DeleteRowAsync(row.SubjectOfferingId, row.ScheduleId);
                await RefreshGridAsync();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Action Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete row: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            await SafeUiRunner.ExecuteAsync(async () =>
            {
                await _scheduleService.SaveChangesAsync(_allSchedules, _activePeriodId);
                MessageBox.Show("Schedules Saved Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await RefreshGridAsync();
            }, _buttons);
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