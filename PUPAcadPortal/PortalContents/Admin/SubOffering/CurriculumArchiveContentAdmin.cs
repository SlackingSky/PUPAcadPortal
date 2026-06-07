using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using PUPAcadPortal.Services;
using PUPAcadPortal.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Admin.SubOffering
{
    public partial class CurriculumArchiveContentAdmin : UserControl
    {
        private CurriculumService _curriculumService = new();
        private List<Subject> _subjects;

        public CurriculumArchiveContentAdmin()
        {
            InitializeComponent();
            SetupComboBoxColumns();
            LoadCurriculum();
            dgvCurriculum.AllowUserToAddRows = true;
            dgvCurriculum.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvCurriculum.AllowUserToDeleteRows = true;
            dtpRevisionYear.ValueChanged += DtpRevisionYear_ValueChanged;
            dgvCurriculum.DefaultValuesNeeded += DgvCurriculum_DefaultValuesNeeded;
            dgvCurriculum.DataError += DgvCurriculum_DataError;
            dgvCurriculum.EditingControlShowing += DgvCurriculum_EditingControlShowing;
            dgvCurriculum.CurrentCellDirtyStateChanged += DgvCurriculum_CurrentCellDirtyStateChanged;
            dgvCurriculum.CellValueChanged += DgvCurriculum_CellValueChanged;
            dgvCurriculum.CellClick += DgvCurriculum_CellClick;
            btnLoadPrevious.Click += btnLoadPrevious_Click;
        }

        private void DgvCurriculum_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvCurriculum.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn)
            {
                dgvCurriculum.BeginEdit(true);

                if (dgvCurriculum.EditingControl is ComboBox combo)
                {
                    combo.DroppedDown = true;
                }
            }
        }

        private void DgvCurriculum_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvCurriculum.IsCurrentCellDirty && dgvCurriculum.CurrentCell.OwningColumn.Name == "CourseCode2")
            {
                dgvCurriculum.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DgvCurriculum_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvCurriculum.Columns[e.ColumnIndex].Name == "CourseCode2")
            {
                string selectedCode = dgvCurriculum.Rows[e.RowIndex].Cells["CourseCode2"].Value?.ToString();

                if (!string.IsNullOrEmpty(selectedCode))
                {
                    var subjectInfo = _subjects.FirstOrDefault(s => s.SubjectCode == selectedCode);

                    if (subjectInfo != null)
                    {
                        dgvCurriculum.Rows[e.RowIndex].Cells["CourseTitle2"].Value = subjectInfo.SubjectName;
                        dgvCurriculum.Rows[e.RowIndex].Cells["Lab2"].Value = subjectInfo.LabUnits;
                        dgvCurriculum.Rows[e.RowIndex].Cells["Lec2"].Value = subjectInfo.LecUnits;
                        dgvCurriculum.Rows[e.RowIndex].Cells["TotalUnits2"].Value = subjectInfo.Units;
                    }
                }
            }
        }


        private void DgvCurriculum_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox combo)
            {
                combo.MaxDropDownItems = 10;
                combo.IntegralHeight = false;

                combo.BeginInvoke(new Action(() =>
                {
                    if (combo.Parent != null && combo.Visible && dgvCurriculum.Focused)
                    {
                        combo.DroppedDown = true;
                    }
                }));
            }
        }

        private void DgvCurriculum_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void SetupComboBoxColumns()
        {
            if (dgvCurriculum.Columns["colProgram"] is DataGridViewComboBoxColumn programCol)
            {
                programCol.DataSource = new List<string> { "BSIT" };
                programCol.DataPropertyName = "Program";
            }

            if (dgvCurriculum.Columns["Year2"] is DataGridViewComboBoxColumn yearCol)
            {
                var years = new[]
                {
            new { Value = 1, Display = "1st Year" },
            new { Value = 2, Display = "2nd Year" },
            new { Value = 3, Display = "3rd Year" },
            new { Value = 4, Display = "4th Year" },
            new { Value = 5, Display = "5th Year" }
        }.ToList();

                yearCol.DataSource = years;
                yearCol.DisplayMember = "Display";
                yearCol.ValueMember = "Value";
                yearCol.DataPropertyName = "YearLevel";
            }

            if (dgvCurriculum.Columns["Semester1"] is DataGridViewComboBoxColumn semCol)
            {
                var semesters = new[]
                {
            new { Value = 1, Display = "First Semester" },
            new { Value = 2, Display = "Second Semester" },
            new { Value = 3, Display = "Summer" }
        }.ToList();

                semCol.DataSource = semesters;
                semCol.DisplayMember = "Display";
                semCol.ValueMember = "Value";
                semCol.DataPropertyName = "Semester";
            }
        }

        private void DgvCurriculum_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["colRevisionYear"].Value = dtpRevisionYear.Value.Year;

            e.Row.Cells["Year2"].Value = 1;
            e.Row.Cells["Semester1"].Value = 1;

            e.Row.Cells["colProgram"].Value = "BSIT";
            e.Row.Cells["CourseCode2"].Value = "";
        }

        private async void DtpRevisionYear_ValueChanged(object? sender, EventArgs e)
        {
            LoadCurriculum();
        }

        private async void LoadCurriculum()
        {
            this.DisableControls();
            dgvCurriculum.DataSource = null;
            dgvCurriculum.Rows.Clear();

            _subjects = await _curriculumService.GetSubjects();
            _subjects = [.. _subjects.OrderBy(s => s.SubjectCode)];

            _subjects.Insert(0, new Subject
            {
                SubjectCode = "",
                SubjectName = "",
                LabUnits = 0,
                LecUnits = 0,
                Units = 0
            });

            if (dgvCurriculum.Columns["CourseCode2"] is DataGridViewComboBoxColumn subCode)
            {
                subCode.DataSource = _subjects;
                subCode.DisplayMember = "SubjectCode";
                subCode.ValueMember = "SubjectCode";
                subCode.DataPropertyName = "SubjectCode";
                subCode.MaxDropDownItems = 10;
            }

            List<CurriculumData> list = await _curriculumService.GetCurriculumAsync(dtpRevisionYear.Value.Year);
            dgvCurriculum.AutoGenerateColumns = false;
            dgvCurriculum.DataSource = new BindingList<CurriculumData>(list);
            dgvCurriculum.ClearSelection();
            this.EnableControls();
        }
        private void btnCurriculum_Click(object sender, EventArgs e)
        {
            pnlCurriculum.Visible = true;
            pnlArchive.Visible = false;
        }

        private void btnArchive_Click(object sender, EventArgs e)
        {
            pnlArchive.Visible = true;
            pnlCurriculum.Visible = false;
        }

        private async void btnUpdateCurriculum_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to update the curriculum?", "Update Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            dgvCurriculum.EndEdit();

            var boundList = dgvCurriculum.DataSource as BindingList<CurriculumData>;
            if (boundList == null) return;

            try
            {
                btnUpdateCurriculum.Enabled = false;
                Application.UseWaitCursor = true;

                int selectedYear = dtpRevisionYear.Value.Year;
                await _curriculumService.UpdateCurriculumAsync(boundList.ToList(), selectedYear);

                this.SafeUIUpdate(() =>
                {
                    MessageBox.Show("Curriculum saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadCurriculum();
                });
            }
            catch (Exception ex)
            {
                this.SafeUIUpdate(() =>
                {
                    MessageBox.Show($"An error occurred while saving: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
            }
            finally
            {
                btnUpdateCurriculum.Enabled = true;
                Application.UseWaitCursor = false;
            }
        }

        private async void btnLoadPrevious_Click(object sender, EventArgs e)
        {
            int currentTargetYear = dtpRevisionYear.Value.Year;

            var confirm = MessageBox.Show(
                $"Are you sure you want to load the previous curriculum into {currentTargetYear}? This will append the subjects to the bottom of your current grid.",
                "Confirm Append",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            btnLoadPrevious.Enabled = false;

            try
            {
                var previousData = await _curriculumService.GetPreviousCurriculumAsync(currentTargetYear);

                if (this.IsDisposed) return;

                if (previousData == null || previousData.Count == 0)
                {
                    MessageBox.Show("No previous curriculum was found in the database to copy from.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var currentGridData = dgvCurriculum.DataSource as BindingList<CurriculumData>;

                if (currentGridData == null)
                {
                    currentGridData = new BindingList<CurriculumData>();
                    dgvCurriculum.DataSource = currentGridData;
                }

                int addedCount = 0;

                foreach (var previousItem in previousData)
                {
                    bool alreadyInGrid = currentGridData.Any(c =>
                        c.SubjectCode == previousItem.SubjectCode &&
                        c.Program == previousItem.Program);

                    if (!alreadyInGrid)
                    {
                        previousItem.RevisionYear = currentTargetYear;
                        currentGridData.Add(previousItem);
                        addedCount++;
                    }
                }

                MessageBox.Show($"Successfully appended {addedCount} subjects from the previous revision!\n\n(Skipped {previousData.Count - addedCount} duplicates).", "Loaded Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                if (this.IsDisposed) return;
                MessageBox.Show($"Error loading previous curriculum: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!this.IsDisposed) btnLoadPrevious.Enabled = true;
            }
        }
    }
}
