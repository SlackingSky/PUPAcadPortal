using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PUPAcadPortal.Services;

namespace PUPAcadPortal.PortalContents.Admin.Enrollment
{
    public partial class EnrolledStudentsContentAdmin : UserControl
    {
        private readonly EnrolledStudentService _studentService = new EnrolledStudentService();

        private List<StudentGridItem> _allStudents = new List<StudentGridItem>();

        public EnrolledStudentsContentAdmin()
        {
            InitializeComponent();
            pnlEnrolledStudentsContent.Layout += (s, e) => ResizeEnrolledStudentsContent();
            ResizeEnrolledStudentsContent();

            this.Load += EnrolledStudentsContentAdmin_Load;

            txtESSearchStudents.TextChanged += (s, e) => ApplyFilters();
            cmbESEnrollmentStatus.SelectedIndexChanged += (s, e) => ApplyFilters();
            cmbESYear.SelectedIndexChanged += (s, e) => ApplyFilters();
        }

        private async void EnrolledStudentsContentAdmin_Load(object sender, EventArgs e)
        {
            InitializeDropdowns();
            await LoadStudentsFromDatabaseAsync();

            if (this.IsDisposed) return;

            CalculateDashboardStats();
            ApplyFilters();
        }

        private void InitializeDropdowns()
        {
            cmbESEnrollmentStatus.SelectedIndex = 0;

            cmbESYear.Items.Clear();
            cmbESYear.Items.Add("All Years");
            cmbESYear.Items.Add("1");
            cmbESYear.Items.Add("2");
            cmbESYear.Items.Add("3");
            cmbESYear.Items.Add("4");
            cmbESYear.Items.Add("5");
            cmbESYear.SelectedIndex = 0;
        }

        private async Task LoadStudentsFromDatabaseAsync()
        {
            try
            {
                _allStudents = await _studentService.GetEnrolledStudentsAsync();
            }
            catch (Exception ex)
            {
                if (this.IsDisposed) return;
                MessageBox.Show($"Error loading students from database: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _allStudents = new List<StudentGridItem>();
            }
        }

        private void CalculateDashboardStats()
        {
            lblESTotalStudentsValue.Text = _allStudents.Count.ToString();
            lblESActiveValue.Text = _allStudents.Count(s => s.Status == "Active").ToString();
            lblESInactiveValue.Text = _allStudents.Count(s => s.Status == "Inactive").ToString();
            lblESGraduatedValue.Text = _allStudents.Count(s => s.Status == "Graduated").ToString();
        }

        private void ApplyFilters()
        {
            if (_allStudents == null || !_allStudents.Any()) return;

            string searchText = txtESSearchStudents.Text.Trim().ToLower();
            string selectedStatus = cmbESEnrollmentStatus.SelectedItem?.ToString() ?? "All Statuses";
            string selectedYearStr = cmbESYear.SelectedItem?.ToString() ?? "All Years";

            var filteredList = _allStudents.AsEnumerable();

            if (!string.IsNullOrEmpty(searchText))
            {
                filteredList = filteredList.Where(s =>
                    s.FullName.ToLower().Contains(searchText) ||
                    s.StudentNumber.ToLower().Contains(searchText) ||
                    s.Email.ToLower().Contains(searchText));
            }

            if (selectedStatus != "All Statuses")
            {
                filteredList = filteredList.Where(s => s.Status == selectedStatus);
            }

            if (selectedYearStr != "All Years" && int.TryParse(selectedYearStr, out int year))
            {
                filteredList = filteredList.Where(s => s.YearLevel == year);
            }

            var finalBindingList = filteredList.ToList();
            dgvStudents.DataSource = new BindingList<StudentGridItem>(finalBindingList);

            if (dgvStudents.Columns.Count > 0)
            {
                dgvStudents.Columns["StudentNumber"].FillWeight = 15;

                dgvStudents.Columns["FullName"].FillWeight = 35;

                dgvStudents.Columns["Email"].FillWeight = 25;

                dgvStudents.Columns["Program"].FillWeight = 10;

                dgvStudents.Columns["YearLevel"].FillWeight = 5;
                dgvStudents.Columns["YearLevel"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvStudents.Columns["YearLevel"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvStudents.Columns["Status"].FillWeight = 10;
            }

            lblStudentList.Text = $"Student List ({finalBindingList.Count})";
        }

        private void ResizeEnrolledStudentsContent()
        {
            if (!pnlEnrolledStudentsContent.Visible) return;

            int contentWidth = pnlEnrolledStudentsContent.ClientSize.Width - 32;
            int gap = 10;
            int cardWidth = (contentWidth - (gap * 3)) / 4;

            pnlESTotalStudentsCard.Width = cardWidth;
            pnlESActiveCard.Width = cardWidth;
            pnlESInactiveCard.Width = cardWidth;
            pnlESGraduatedCard.Width = cardWidth;

            pnlESTotalStudentsCard.Left = 16;
            pnlESActiveCard.Left = 16 + cardWidth + gap;
            pnlESInactiveCard.Left = 16 + (cardWidth + gap) * 2;
            pnlESGraduatedCard.Left = 16 + (cardWidth + gap) * 3;
        }
    }
}