using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class AdminPortal : Form
    {
        private Button clickedButton;
        private Color defaultColor = Color.Maroon;
        private Color selectedColor = Color.FromArgb(109, 0, 0);
        private Color selectedUserButtonColor = Color.FromArgb(109, 0, 0);
        private Button selectedUserTypeButton;                 // tracks which button (Students/Professors) is selected
        private Color defaultUserButtonColor = Color.Maroon;   // default color for the toggle buttons
        private Dictionary<Button, ContentPanelInfo> contentPanels; // Class-level dictionary to hold button-panel mappings
                                                                    // Data storage for filtering
        private List<string[]> studentList = new List<string[]>();
        private List<string[]> professorList = new List<string[]>();
        public AdminPortal()
        {
            InitializeComponent();
            this.Resize += AdminPortal_Resize;
            this.Load += AdminPortal_Load;

            // Attach event handlers for filtering controls in View All Users
            btnSearch.Click += (s, e) => ApplyFiltersAndRefresh();
            cmbProgram.SelectedIndexChanged += (s, e) => ApplyFiltersAndRefresh();
            cmbYear.SelectedIndexChanged += (s, e) => ApplyFiltersAndRefresh();

            contentPanels = new Dictionary<Button, ContentPanelInfo>
            {
            { btnDashboard, new ContentPanelInfo { Panel = pnlDashboardContent, ResetAction = () => { /* dashboard reset logic */ } } },
            { btnSubjectOffering, new ContentPanelInfo { Panel = pnlSubOfferingContent, ResetAction = () => { /* subject offering reset */ } } },
            { btnGradesManagement, new ContentPanelInfo { Panel = pnlGradesManagementContent, ResetAction = () => { /* grades reset */ } } },
            { btnAccountingRecords, new ContentPanelInfo { Panel = pnlAccountingRecordsContent, ResetAction = () => { /* accounting reset */ } } },
            { btnEnrolledStudents, new ContentPanelInfo { Panel = pnlEnrolledStudentsContent, ResetAction = () => { /* enrolled students reset */ } } },
            { btnRegisterStudent, new ContentPanelInfo { Panel = pnlRegisterStudentContent, ResetAction = () => { /* register student reset */ } } },
            { btnRegisterProfessor, new ContentPanelInfo { Panel = pnlRegisterProfessorContent, ResetAction = () => { /* register professor reset */ } } },
            { btnViewAllUsers, new ContentPanelInfo { Panel = pnlViewAllUsersContent, ResetAction = ResetViewAllUsersPanel } }
            };
        }

        public class ContentPanelInfo
        {
            public Panel Panel { get; set; }
            public Action ResetAction { get; set; }
        }

        private void changeButtonColor(Button button)
        {
            if (clickedButton != null)
            {
                clickedButton.BackColor = defaultColor;
            }
            clickedButton = button;
            pnlYellow.Visible = true;
            pnlYellow.Parent = clickedButton.Parent;
            pnlYellow.BringToFront();
            clickedButton.BackColor = selectedColor;
        }

        //Method na nagpapakita ng content ng bawat button, wala akong maisip na iba kaya eto
        private void showContent(Button newButton)
        {
            // Reset the panel we are leaving
            if (clickedButton != null && contentPanels.ContainsKey(clickedButton))
            {
                contentPanels[clickedButton].ResetAction?.Invoke();
            }

            // Then show the new panel
            foreach (var kvp in contentPanels)
            {
                bool isVisible = (kvp.Key == newButton);
                FitContentPanel(kvp.Value.Panel);
                kvp.Value.Panel.Visible = isVisible;
            }
        }



        //Method para pag pinindot yung X sa taas o mag alt-F4, icclose lahat ng forms para di magerror pag ni run uli
        //Lagay to sa bawat form na iaadd, Step 1: Hanapin sa properties ng form yung event na FormClosing, Step 2: Double click para gumawa ng method, Step 3: Copy paste code na nasa loob nito
        private void AdminPortal_Closing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Are you sure you want to exit?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                    Application.Exit();
            }
        }

        private void AdminPortal_Resize(object sender, EventArgs e)
        {
            if (!IsHandleCreated) return;

            // Resize the currently visible main content panel
            foreach (var kvp in contentPanels)
            {
                if (kvp.Value.Panel.Visible)
                {
                    FitContentPanel(kvp.Value.Panel);
                    break;
                }
            }

            // Resize visible sub-content panels (if you have separate ones)
            if (pnlGradesManagementContent.Visible) FitContentPanel(pnlGradesManagementContent);
            if (pnlAccountingRecordsContent.Visible) FitContentPanel(pnlAccountingRecordsContent);
            if (pnlEnrolledStudentsContent.Visible) FitContentPanel(pnlEnrolledStudentsContent);
        }

        private void FitContentPanel(Panel panel)
        {
            panel.Width = this.ClientSize.Width - pnlSidebar.Width;
            panel.Height = this.ClientSize.Height - pnlHeader.Height;
            panel.Location = new Point(pnlSidebar.Width, pnlHeader.Height);
        }

        private void ShowSubContent(Panel contentPanel)
        {
            // Hide all main content panels
            foreach (var kvp in contentPanels)
            {
                kvp.Value.Panel.Visible = false;
            }

            // Hide all sub‑content panels except the one to show
            pnlGradesManagementContent.Visible = (contentPanel == pnlGradesManagementContent);
            pnlAccountingRecordsContent.Visible = (contentPanel == pnlAccountingRecordsContent);
            pnlEnrolledStudentsContent.Visible = (contentPanel == pnlEnrolledStudentsContent);

            // Position and size the selected panel
            FitContentPanel(contentPanel);
            contentPanel.Visible = true;
            contentPanel.BringToFront();
        }

        // EVENT HANDLERS [ToT]

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
            pnlRegistrarSubmenu.Visible = false;
        }

        private void btnEnrollments_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
            pnlRegistrarSubmenu.Visible = false;
        }

        private void btnSubjectOffering_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
            pnlRegistrarSubmenu.Visible = false;
        }

        private void btnLMS_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
            pnllmsSubmenu.Visible = !pnllmsSubmenu.Visible;
            if (pnllmsSubmenu.Visible)
                btnLMS.Text = " LMS                                       ⌄";
            else
                btnLMS.Text = " LMS                                        ›";
        }

        private void btnRegistrarFunctions_Click(object sender, EventArgs e)
        {
            // Change button color and show the main content panel (if any)
            changeButtonColor(sender as Button);

            // Toggle the submenu visibility
            pnlRegistrarSubmenu.Visible = !pnlRegistrarSubmenu.Visible;

            // Update the button text with arrow
            if (pnlRegistrarSubmenu.Visible)
                btnRegistrarFunctions.Text = " Registrar Functions              ⌄";
            else
                btnRegistrarFunctions.Text = " Registrar Functions              ›";
        }

        // Submenu button event handlers

        private void btnGradesManagement_Click(object sender, EventArgs e)
        {
            // Show the corresponding content panel
            ShowSubContent(pnlGradesManagementContent);
            // Optional: hide the submenu after selection (remove if you want it to stay open)
            //pnlRegistrarSubmenu.Visible = false;
            // Update the button text arrow to closed state
            btnRegistrarFunctions.Text = " Registrar Functions              ⌄";
        }

        private void btnAccountingRecords_Click(object sender, EventArgs e)
        {
            ShowSubContent(pnlAccountingRecordsContent);
            //pnlRegistrarSubmenu.Visible = false;
            btnRegistrarFunctions.Text = " Registrar Functions              ⌄";
        }

        private void btnEnrolledStudents_Click(object sender, EventArgs e)
        {
            ShowSubContent(pnlEnrolledStudentsContent);
            //pnlRegistrarSubmenu.Visible = false;
            btnRegistrarFunctions.Text = " Registrar Functions              ⌄";
        }

        // Other sidebar buttons

        private void bgtnRegisterStudents_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
            pnlRegistrarSubmenu.Visible = false;
        }

        private void btnRegisterProf_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
            pnlRegistrarSubmenu.Visible = false;
        }

        private void btnViewAllUsers_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
            pnlRegistrarSubmenu.Visible = false;

            // Force refresh to display placeholder users
            ApplyFiltersAndRefresh();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
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

        // Track current view
        private bool viewingStudents = true;

        private void AdminPortal_Load(object sender, EventArgs e)
        {
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

        private void LoadStudentPlaceholders()
        {
            studentList.Clear();
            studentList.Add(new string[] { "2024-00001-SM-0", "Juan dela Cruz", "juandc@iskolarngbayan.pup.edu.ph", "BSIT", "2nd Year", "Enrolled" });
            studentList.Add(new string[] { "2024-00002-SM-0", "Maria Santos", "mariasantos@iskolarngbayan.pup.edu.ph", "BSIT", "2nd Year", "Enrolled" });
            studentList.Add(new string[] { "2025-00003-SM-0", "Pedro Reyes", "pedror@iskolarngbayan.pup.edu.ph", "BSHM", "1st Year", "Enrolled" });
            studentList.Add(new string[] { "2023-00004-SM-0", "Ana Gonzales", "anag@iskolarngbayan.pup.edu.ph", "BSCpE", "3rd Year", "Enrolled" });
            studentList.Add(new string[] { "2024-00005-SM-0", "Jose Garcia", "joseg@iskolarngbayan.pup.edu.ph", "BSED-M", "2nd Year", "Enrolled" });

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

        private void UpdateUserTypeIndicator()
        {
            // Move the maroon indicator bar under the active button
            if (viewingStudents)
            {
                pnlUserTypeIndicator.Left = btnViewStudents.Left;
                pnlUserTypeIndicator.Width = btnViewStudents.Width;
                btnViewStudents.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold);
                btnViewProf.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Regular);
            }
            else
            {
                pnlUserTypeIndicator.Left = btnViewProf.Left;
                pnlUserTypeIndicator.Width = btnViewProf.Width;
                btnViewProf.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold);
                btnViewStudents.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Regular);
            }
            pnlUserTypeIndicator.Visible = true;
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

        private void ResetRegisterStudentPanel()
        {
            // Example: clear all input fields in that panel
            // txtStudentName.Text = "";
            // etc.
        }

        private void txtSearchViewAUs_TextChanged(object sender, EventArgs e)
        {
            ApplyFiltersAndRefresh();
        }

        
    }
}
