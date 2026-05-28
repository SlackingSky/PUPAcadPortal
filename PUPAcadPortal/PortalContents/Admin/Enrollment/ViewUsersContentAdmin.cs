using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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

        private void ViewUsersContentAdmin_Load(object sender, EventArgs e)
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

            LoadStudentPlaceholders(); // show students by default

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

        private void LoadStudentPlaceholders()
        {
            studentList.Clear();
            studentList.Add(new string[] { "2024-00001-SM-0", "Juan dela Cruz", "juandc@iskolarngbayan.pup.edu.ph", "BSIT", "2nd Year", "Enrolled" });
            studentList.Add(new string[] { "2024-00002-SM-0", "Maria Santos", "mariasantos@iskolarngbayan.pup.edu.ph", "BSIT", "2nd Year", "Enrolled" });
            studentList.Add(new string[] { "2025-00003-SM-0", "Pedro Reyes", "pedror@iskolarngbayan.pup.edu.ph", "BSIT", "1st Year", "Enrolled" });
            studentList.Add(new string[] { "2023-00004-SM-0", "Ana Gonzales", "anag@iskolarngbayan.pup.edu.ph", "BSIT", "3rd Year", "Enrolled" });
            studentList.Add(new string[] { "2024-00005-SM-0", "Jose Garcia", "joseg@iskolarngbayan.pup.edu.ph", "BSIT", "2nd Year", "Enrolled" });

            // Refresh the grid with current filters (if viewing students)
            if (viewingStudents)
                ApplyFiltersAndRefresh();
        }

        private void LoadProfessorPlaceholders()
        {
            professorList.Clear();
            professorList.Add(new string[] { "PROF-001", "Dr. Roberto Lim", "rlim@pup.edu.ph", "BSIT Dept.", "N/A", "Active" });
            professorList.Add(new string[] { "PROF-002", "Prof. Carmen Reyes", "creyes@pup.edu.ph", "BSHM Dept.", "N/A", "Active" });
            professorList.Add(new string[] { "PROF-003", "Dr. Antonio Cruz", "acruz@pup.edu.ph", "BSCpE Dept.", "N/A", "Active" });
            professorList.Add(new string[] { "PROF-004", "Prof. Liza Ramos", "lramos@pup.edu.ph", "BSED Dept.", "N/A", "On Leave" });

            if (!viewingStudents)
                ApplyFiltersAndRefresh();
        }

        private void ApplyFiltersAndRefresh()
        {
            // Search term – ignore placeholder
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

        private void ResetViewAllUsersPanel()
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
            LoadStudentPlaceholders();   // this calls ApplyFiltersAndRefresh once
            UpdateUserTypeIndicator();

            // Re-attach event handler
            txtSearchViewAUs.TextChanged += (s, e) => ApplyFiltersAndRefresh();
        }

        //View All Users Submenu Toggle (Students/Professors)

        private void btnViewStudents_Click(object sender, EventArgs e)
        {
            viewingStudents = true;
            LoadStudentPlaceholders();
            UpdateUserTypeIndicator();
        }

        private void btnViewProf_Click(object sender, EventArgs e)
        {
            viewingStudents = false;
            LoadProfessorPlaceholders();
            UpdateUserTypeIndicator();
        }

        private void txtSearchViewAUs_TextChanged(object sender, EventArgs e)
        {
            ApplyFiltersAndRefresh();
        }
    }
}
