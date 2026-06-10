using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PUPAcadPortal.Services;
using PUPAcadPortal.Models;

namespace PUPAcadPortal.PortalContents.Admin.SubOffering
{
    public partial class SubjectAddForm : Form
    {
        private SubjectService _subjectService;
        private string _subjectIdToEdit = null;

        private List<DepartmentPrefix> _departmentPrefixes;

        public SubjectAddForm(string subjectId = null)
        {
            InitializeComponent();
            _subjectService = new SubjectService();
            _subjectIdToEdit = subjectId;

            this.Load += SubjectAddForm_Load;
            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            numLec.ValueChanged += CalculateTotalUnits;
            numLab.ValueChanged += CalculateTotalUnits;

            txtSubjectCode.TextChanged += txtSubjectCode_TextChanged;
        }

        private async void SubjectAddForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_subjectIdToEdit))
                {
                    numUnits.Value = 3;
                }

                var departments = await _subjectService.GetActiveDepartmentsAsync();
                var allSubjects = await _subjectService.GetAllSubjectsAsync();
                _departmentPrefixes = await _subjectService.GetDepartmentPrefixesAsync();

                cmbDepartment.DataSource = departments;
                cmbDepartment.DisplayMember = "DepartmentName";
                cmbDepartment.ValueMember = "DepartmentID";

                ((ListBox)clbPrerequisites).DataSource = allSubjects;
                ((ListBox)clbPrerequisites).DisplayMember = "SubjectCode";
                ((ListBox)clbPrerequisites).ValueMember = "SubjectID";

                if (!string.IsNullOrEmpty(_subjectIdToEdit))
                {
                    lblHeaderTitle.Text = "Edit Subject Details";
                    btnSave.Text = "Update";

                    var subject = await _subjectService.GetSubjectByIdAsync(_subjectIdToEdit);
                    if (subject != null)
                    {
                        txtSubjectCode.Text = subject.SubjectCode;
                        txtSubjectName.Text = subject.SubjectName;
                        cmbDepartment.SelectedValue = subject.DepartmentId;
                        txtDescription.Text = subject.Description;
                        numLec.Value = subject.LecUnits;
                        numLab.Value = subject.LabUnits;
                        numUnits.Value = subject.Units;

                        var existingPrereqIds = await _subjectService.GetPrerequisiteIdsAsync(_subjectIdToEdit);

                        for (int i = 0; i < clbPrerequisites.Items.Count; i++)
                        {
                            var item = (Subject)clbPrerequisites.Items[i];

                            if (existingPrereqIds.Contains(item.SubjectId))
                            {
                                clbPrerequisites.SetItemChecked(i, true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load form data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSubjectCode_TextChanged(object sender, EventArgs e)
        {
            if (_departmentPrefixes == null || _departmentPrefixes.Count == 0) return;

            string code = txtSubjectCode.Text.Trim().ToUpper();
            string[] parts = code.Split(' ');

            if (parts.Length > 0)
            {
                string prefix = parts[0];
                var matchingPrefix = _departmentPrefixes.FirstOrDefault(p => p.Prefix.ToUpper() == prefix);

                if (matchingPrefix != null)
                {
                    cmbDepartment.SelectedValue = matchingPrefix.DepartmentId;
                }
            }
        }

        private void CalculateTotalUnits(object sender, EventArgs e)
        {
            numUnits.Value = numLec.Value + numLab.Value;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSubjectCode.Text) || string.IsNullOrWhiteSpace(txtSubjectName.Text))
            {
                MessageBox.Show("Please fill in the Subject Code and Name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnSave.Enabled = false;

                string code = txtSubjectCode.Text.Trim();
                string name = txtSubjectName.Text.Trim();
                int deptId = (int)cmbDepartment.SelectedValue;
                string desc = txtDescription.Text.Trim();
                int lec = (int)numLec.Value;
                int lab = (int)numLab.Value;
                int totalUnits = (int)numUnits.Value;

                List<string> selectedPrereqs = new List<string>();
                foreach (var item in clbPrerequisites.CheckedItems)
                {
                    var checkedSubject = (Subject)item;
                    selectedPrereqs.Add(checkedSubject.SubjectId);
                }

                if (string.IsNullOrEmpty(_subjectIdToEdit))
                {
                    await _subjectService.CreateSubjectAsync(code, name, deptId, desc, lec, lab, totalUnits, selectedPrereqs);
                    MessageBox.Show("Subject created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    await _subjectService.UpdateSubjectAsync(_subjectIdToEdit, code, name, deptId, desc, lec, lab, totalUnits, selectedPrereqs);
                    MessageBox.Show("Subject updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Saving", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}