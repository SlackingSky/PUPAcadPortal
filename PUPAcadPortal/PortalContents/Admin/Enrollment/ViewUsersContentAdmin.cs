using PUPAcadPortal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

namespace PUPAcadPortal.PortalContents.Admin.Enrollment
{
    public partial class ViewUsersContentAdmin : UserControl
    {
        // Track current view
        private bool viewingStudents = true;

        private List<string[]> studentList = new List<string[]>();
        private List<string[]> professorList = new List<string[]>();

        public ViewUsersContentAdmin()
        {
            InitializeComponent();

            // Attach event handlers for filtering controls in View All Users
            btnSearch.Click += (s, e) => ApplyFiltersAndRefresh();
            cmbProgram.SelectedIndexChanged += (s, e) => ApplyFiltersAndRefresh();
            cmbYear.SelectedIndexChanged += (s, e) => ApplyFiltersAndRefresh();
        }

        private async void ViewUsersContentAdmin_Load(object sender, EventArgs e)
        {
            // Force refresh to display placeholder users
            ApplyFiltersAndRefresh();

            // Make DataGridView read‑only and selection‑free
            dgvUsers.ReadOnly = true;
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.DefaultCellStyle.SelectionBackColor = dgvUsers.DefaultCellStyle.BackColor;
            dgvUsers.DefaultCellStyle.SelectionForeColor = dgvUsers.DefaultCellStyle.ForeColor;
            dgvUsers.ClearSelection();

            await LoadStudentsAsync(); // show students by default

            if (this.IsDisposed) return;

            UpdateUserTypeIndicator();

            // Set up placeholder text behavior for search box
            txtSearchViewAUs.Enter += (s, e) =>
            {
                if (txtSearchViewAUs.Text == "Search here...")
                {
                    txtSearchViewAUs.Text = "";
                    txtSearchViewAUs.ForeColor = Color.Black;
                }
            };
            txtSearchViewAUs.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearchViewAUs.Text))
                {
                    txtSearchViewAUs.Text = "Search here...";
                    txtSearchViewAUs.ForeColor = Color.Gray;
                }
            };
        }

        private void UpdateUserTypeIndicator()
        {
            // Move the maroon indicator bar under the active button
            if (viewingStudents)
            {
                pnlUserTypeIndicator.Parent = btnViewStudents;
                pnlUserTypeIndicator.Location = new Point(0, btnViewStudents.Height - pnlUserTypeIndicator.Height);
                pnlUserTypeIndicator.Width = btnViewStudents.Width;
                btnViewStudents.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold);
                btnViewProf.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Regular);
            }
            else
            {
                pnlUserTypeIndicator.Parent = btnViewProf;
                pnlUserTypeIndicator.Location = new Point(0, btnViewProf.Height - pnlUserTypeIndicator.Height);
                pnlUserTypeIndicator.Width = btnViewProf.Width;
                btnViewProf.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold);
                btnViewStudents.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Regular);
            }
            pnlUserTypeIndicator.Visible = true;
        }

        // Data loading methods for students
        private async Task LoadStudentsAsync()
        {
            using (var context = new AppDbContext())
            {
                // Fetch users who have student records, including their student details
                var students = await context.Users
                    .Where(u => u.Students.Any())
                    .Select(u => new
                    {
                        u.FirstName,
                        u.LastName,
                        u.InstitutionalEmail,
                        u.IsActive,
                        // Get the first student record associated with this user
                        Detail = u.Students.FirstOrDefault()
                    })
                    .ToListAsync();

                studentList = students.Select(x => new string[]
                {
                    x.Detail != null ? x.Detail.StudentNumber : "N/A", // Uses UserId as fallback ID
                    $"{x.FirstName} {x.LastName}",
                    x.InstitutionalEmail,
                    x.Detail.Program, // Default fallback string matching your UI screenshot
                    $"{x.Detail.YearLevel switch
                    {
                        1 => "1st",
                        2 => "2nd",
                        3 => "3rd",
                        4 => "4th",
                        5 => "5th",
                        _ => "Unknown"
                    }} Year",
                    (x.IsActive ?? true) ? "Enrolled" : "Inactive"
                }).ToList();
            }

            if (viewingStudents)
                ApplyFiltersAndRefresh();
        }

        // Data loading methods for professors
        private async Task LoadProfessorsAsync()
        {
            using (var context = new AppDbContext())
            {
                // Fetch users who have professor records
                var professors = await context.Users
                    .Where(u => u.Professors.Any())
                    .Include(u => u.Professors)
                        .ThenInclude(p => p.Department)
                    .Select(u => new
                    {
                        u.FirstName,
                        u.LastName,
                        u.InstitutionalEmail,
                        u.IsActive,
                        Detail = u.Professors.FirstOrDefault()
                    })
                    .ToListAsync();

                professorList = professors.Select(x => new string[]
                {
                    x.Detail != null ? x.Detail.EmployeeId : "N/A",
                    $"{x.FirstName} {x.LastName}",
                    x.InstitutionalEmail,
                    $"{x.Detail.Department.DepartmentCode} Dept.",
                    "N/A",
                    x.Detail.EmploymentStatus
                }).ToList();
            }

            if (!viewingStudents)
                ApplyFiltersAndRefresh();
        }

        private void ApplyFiltersAndRefresh()
        {
            // Search term – ignore placeholder
            if (this.IsDisposed) return;
            string searchTerm = txtSearchViewAUs.Text.Trim();
            if (searchTerm == "Search here...") searchTerm = "";
            searchTerm = searchTerm.ToLower();

            // Program filter – treat "All" and "Program" as no filter
            string selectedProgram = cmbProgram.SelectedItem?.ToString();
            if (selectedProgram == "Program" || selectedProgram == "All")
                selectedProgram = null;

            // Year filter – treat "All" and "Year" as no filter
            string selectedYear = cmbYear.SelectedItem?.ToString();
            if (selectedYear == "Year" || selectedYear == "All")
                selectedYear = null;

            IEnumerable<string[]> filteredData = null;

            if (viewingStudents)
            {
                filteredData = studentList.Where(student =>
                {
                    bool matchesSearch = string.IsNullOrEmpty(searchTerm) ||
                        student[0].ToLower().Contains(searchTerm) ||
                        student[1].ToLower().Contains(searchTerm) ||
                        student[2].ToLower().Contains(searchTerm);

                    bool matchesProgram = selectedProgram == null ||
                        student[3].Equals(selectedProgram, StringComparison.OrdinalIgnoreCase);

                    bool matchesYear = selectedYear == null ||
                        student[4].Equals(selectedYear, StringComparison.OrdinalIgnoreCase);

                    return matchesSearch && matchesProgram && matchesYear;
                });
            }
            else // professors
            {
                filteredData = professorList.Where(prof =>
                {
                    bool matchesSearch = string.IsNullOrEmpty(searchTerm) ||
                        prof[0].ToLower().Contains(searchTerm) ||
                        prof[1].ToLower().Contains(searchTerm) ||
                        prof[2].ToLower().Contains(searchTerm);

                    // Only add " Dept." if a real program is selected (not null)
                    bool matchesProgram = selectedProgram == null ||
                        prof[3].Equals(selectedProgram + " Dept.", StringComparison.OrdinalIgnoreCase);

                    return matchesSearch && matchesProgram;
                });
            }

            dgvUsers.Rows.Clear();
            foreach (var row in filteredData)
                dgvUsers.Rows.Add(row);

            if (dgvUsers.Rows.Count == 0)
            {
                dgvUsers.Rows.Add("No results found", "", "", "", "", "");
                dgvUsers.Rows[0].DefaultCellStyle.ForeColor = Color.Gray;
            }

            dgvUsers.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void txtSearchViewAUs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ApplyFiltersAndRefresh();
            }
        }

        private async Task ResetViewAllUsersPanel()
        {
            // Temporarily remove event handler to avoid unnecessary filtering
            txtSearchViewAUs.TextChanged += txtSearchViewAUs_TextChanged;

            txtSearchViewAUs.Text = "Search here...";
            txtSearchViewAUs.ForeColor = Color.Gray;
            cmbProgram.SelectedIndex = -1;
            cmbProgram.Text = "Program";
            cmbYear.SelectedIndex = -1;
            cmbYear.Text = "Year";
            viewingStudents = true;
            await LoadStudentsAsync();   // this calls ApplyFiltersAndRefresh once
            if (this.IsDisposed) return;
            UpdateUserTypeIndicator();

            // Re-attach event handler
            txtSearchViewAUs.TextChanged += (s, e) => ApplyFiltersAndRefresh();
        }

        //View All Users Submenu Toggle (Students/Professors)

        private async void btnViewStudents_Click(object sender, EventArgs e)
        {
            viewingStudents = true;
            await LoadStudentsAsync();

            if (this.IsDisposed) return;
            UpdateUserTypeIndicator();
        }

        private async void btnViewProf_Click(object sender, EventArgs e)
        {
            viewingStudents = false;
            await LoadProfessorsAsync();

            if (this.IsDisposed) return;
            UpdateUserTypeIndicator();
        }

        private void txtSearchViewAUs_TextChanged(object sender, EventArgs e)
        {
            ApplyFiltersAndRefresh();
        }
    }
}
