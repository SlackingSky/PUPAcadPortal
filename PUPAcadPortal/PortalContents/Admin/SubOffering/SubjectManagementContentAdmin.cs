using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PUPAcadPortal.Services;

namespace PUPAcadPortal.PortalContents.Admin.SubOffering
{
    public partial class SubjectManagementContentAdmin : UserControl
    {
        private readonly SubjectService _subjectService;
        private DataTable _subjectDataTable;

        public SubjectManagementContentAdmin()
        {
            InitializeComponent();
            _subjectService = new SubjectService();

            this.Load += SubjectManagementContentAdmin_Load;

            txtSearch.TextChanged += (s, e) => ApplyFilters();
            cmbFilterDept.SelectedIndexChanged += (s, e) => ApplyFilters();

            btnAdd.Click += btnAdd_Click;
            btnEdit.Click += btnEdit_Click;
            btnDelete.Click += btnDelete_Click;
        }

        private async void SubjectManagementContentAdmin_Load(object sender, EventArgs e)
        {
            await LoadDepartmentsAsync();
            await EvaluateSecurityLockAsync();
            await RefreshGridAsync();
        }

        private async Task LoadDepartmentsAsync()
        {
            var depts = await _subjectService.GetActiveDepartmentsAsync();
            var list = depts.Select(d => new { d.DepartmentId, d.DepartmentName }).ToList();
            list.Insert(0, new { DepartmentId = 0, DepartmentName = "All Departments" });

            cmbFilterDept.DataSource = list;
            cmbFilterDept.DisplayMember = "DepartmentName";
            cmbFilterDept.ValueMember = "DepartmentId";
        }

        private async Task EvaluateSecurityLockAsync()
        {
            bool isActive = await _subjectService.IsSemesterActiveAsync();
            //bool isActive = false;

            if (isActive)
            {
                btnAdd.Enabled = btnEdit.Enabled = btnDelete.Enabled = false;
                lblWarning.Visible = true;
                btnAdd.BackColor = btnEdit.BackColor = btnDelete.BackColor = System.Drawing.Color.Gray;
            }
        }

        private async Task RefreshGridAsync()
        {
            var subjects = await _subjectService.GetSubjectsForGridAsync();

            _subjectDataTable = new DataTable();
            _subjectDataTable.Columns.Add("SubjectId");
            _subjectDataTable.Columns.Add("SubjectCode");
            _subjectDataTable.Columns.Add("SubjectName");
            _subjectDataTable.Columns.Add("Department");
            _subjectDataTable.Columns.Add("LecUnits");
            _subjectDataTable.Columns.Add("LabUnits");
            _subjectDataTable.Columns.Add("Units");

            foreach (dynamic s in subjects)
                _subjectDataTable.Rows.Add(s.SubjectId, s.SubjectCode, s.SubjectName, s.Department, s.LecUnits, s.LabUnits, s.Units);

            dgvSubjects.AutoGenerateColumns = false;
            dgvSubjects.DataSource = _subjectDataTable;
            SetupColumns();
        }

        private void SetupColumns()
        {
            dgvSubjects.Columns.Clear();

            AddColumn("SubjectId", "ID", false);
            AddColumn("SubjectCode", "Code", true, 15);
            AddColumn("SubjectName", "Title", true, 35);
            AddColumn("Department", "Department", true, 30);
            AddColumn("LecUnits", "Lec", true, 5, true);
            AddColumn("LabUnits", "Lab", true, 5, true);
            AddColumn("Units", "Total", true, 10, true);

            foreach (DataGridViewColumn col in dgvSubjects.Columns)
            {
                if (col.Name == "SubjectId" || col.DataPropertyName == "SubjectId")
                {
                    col.Visible = false;
                }
            }

            dgvSubjects.ClearSelection();
            dgvSubjects.DefaultCellStyle.SelectionBackColor = Color.FromArgb(128, 0, 0);
            dgvSubjects.DefaultCellStyle.SelectionForeColor = Color.White;
        }

        private void AddColumn(string dataProp, string header, bool visible, int weight = 10, bool center = false)
        {
            var col = new DataGridViewTextBoxColumn
            {
                DataPropertyName = dataProp,
                HeaderText = header,
                Visible = visible,
                FillWeight = weight,
                Name = dataProp
            };

            if (center)
            {
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            dgvSubjects.Columns.Add(col);
        }

        private void ApplyFilters()
        {
            if (_subjectDataTable == null) return;
            string filter = "";
            string search = txtSearch.Text.Replace("'", "''");

            if (!string.IsNullOrEmpty(search))
                filter += $"SubjectCode LIKE '%{search}%' OR SubjectName LIKE '%{search}%'";

            if (cmbFilterDept.SelectedValue != null && cmbFilterDept.SelectedValue.ToString() != "0")
            {
                if (filter != "") filter += " AND ";
                filter += $"Department = '{cmbFilterDept.Text.Replace("'", "''")}'";
            }
            _subjectDataTable.DefaultView.RowFilter = filter;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new SubjectAddForm())
            {
                if (form.ShowDialog() == DialogResult.OK) _ = RefreshGridAsync();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvSubjects.SelectedRows.Count == 0) return;
            string id = dgvSubjects.SelectedRows[0].Cells["SubjectId"].Value.ToString();
            using (var form = new SubjectAddForm(id))
            {
                if (form.ShowDialog() == DialogResult.OK) _ = RefreshGridAsync();
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvSubjects.SelectedRows.Count == 0) return;
            string id = dgvSubjects.SelectedRows[0].Cells["SubjectId"].Value.ToString();

            if (MessageBox.Show("Delete this subject?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    await _subjectService.DeleteSubjectAsync(id);
                    await RefreshGridAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}