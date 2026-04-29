using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class AdminPortal : Form
    {

        private Button clickedButton;
        private Color defaultColor = Color.Maroon;
        private Color selectedColor = Color.FromArgb(109, 0, 0);
        private List<string[]> _savedSchedule = new List<string[]>();
        private Color selectedUserButtonColor = Color.FromArgb(109, 0, 0);
        private Button selectedUserTypeButton;                 // tracks which button (Students/Professors) is selected
        private Color defaultUserButtonColor = Color.Maroon;   // default color for the toggle buttons
        private Dictionary<Button, ContentPanelInfo> contentPanels; // Class-level dictionary to hold button-panel mappings
                                                                    // Data storage for filtering
        private int originalFormTop;
        private DataTable gradesTable;
        private List<string[]> studentList = new List<string[]>();
        private List<string[]> professorList = new List<string[]>();

        private void ShowPanel(Panel panelToShow)
        {
            pnlSubOfferingContent.Visible = false;
            pnlCurrentSemester.Visible = false;
            pnlEditSchedule.Visible = false;
            pnlSchedule.Visible = false;
            pnlCurriculumArchive.Visible = false;
            pnlCurriculum.Visible = false;
            pnlArchive.Visible = true;

            panelToShow.Visible = true;
            panelToShow.BringToFront();
        }
        public AdminPortal()
        {
            InitializeComponent();
            this.Resize += AdminPortal_Resize;
            this.Load += AdminPortal_Load;

            // Attach resize event handlers for sub-content panels that require dynamic resizing
            pnlEnrolledStudentsContent.Layout += (s, e) => ResizeEnrolledStudentsContent();
            pnlAccountingRecordsContent.Layout += (s, e) => ResizeAccountingRecordsContent();


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

            pnlEditSchedule.Visible = false;
            pnlCurrentSemester.Visible = false;
            pnlSubOfferingContent.Visible = false;
            pnlCurriculumArchive.Visible = false;

            pnlSubOfferingContent.AutoScroll = true;


            AddDuplicateButtonColumn();
            AddRemoveButtonColumn();

            // Add 30 default empty rows to the edit schedule grid and Current Semester grid
            try { dgvSchedule.Rows.Add(30); } catch { }
            try { dgvEditSchedule.Rows.Add(30); } catch { }
            try { dgvScheduleView.Rows.Add(30); } catch { }
            try { dgvArchive.Rows.Add(25); } catch { }

            // Fill first 3 rows with default data for edit schedule and current semester table (ito na rin gamitin sa other two submenus)
            string[,] dummyData =
            {
                { "COMP 009", "Object Oriented Programming", "3.0", "2.0", "5.0" },
                { "COMP 010", "Information Management", "3.0", "2.0", "5.0" },
                { "COMP 012", "Network Administration", "3.0", "2.0", "5.0" }
            };

            string[,] curriculumData =
            {
                // First Year - First Semester
                { "COMP 001", "Introduction to Computing", "3", "2", "3", "1" },
                { "COMP 002", "Computer Programming 1", "3", "2", "3", "1" },
                { "ACCO 014", "Principles of Accounting", "0", "3", "3", "1" },
                { "GEED 004", "Mathematics in the Modern World", "0", "3", "3", "1" },
                { "GEED 005", "Purposive Communication", "0", "3", "3", "1" },
                { "GEED 032", "Filipinolohiya at Pambansang Kaunlaran", "0", "3", "3", "1" },
                { "PATHFit 1", "Physical Activities Towards Health and Fitness 1", "0", "2", "2", "1" },
                { "NSTP 001", "National Service Training Program 1", "0", "3", "3", "1" },

                // First Year - Second Semester
                { "COMP 003", "Computer Programming 2", "3", "2", "3", "1" },
                { "COMP 004", "Discrete Structures 1", "0", "3", "3", "1" },
                { "COMP 008", "Data Communications and Networking", "3", "2", "3", "1" },
                { "GEED 002", "Readings in Philippine History", "0", "3", "3", "1" },
                { "GEED 010", "People and the Earth's Ecosystem", "0", "3", "3", "1" },
                { "GEED 020", "Politics, Governance and Citizenship", "0", "3", "3", "1" },
                { "PATHFit 2", "Physical Activities Towards Health and Fitness 2", "0", "2", "2", "1" },
                { "NSTP 002", "National Service Training Program 2", "0", "3", "3", "1" },

                // Second Year - First Semester
                { "INTE 201", "Programming 3 (Structured Programming)", "3", "2", "3", "2" },
                { "COMP 006", "Data Structures and Algorithms", "3", "2", "3", "2" },
                { "COMP 007", "Operating Systems", "3", "2", "3", "2" },
                { "GEED 001", "Understanding the Self", "0", "3", "3", "2" },
                { "GEED 028", "Reading Visual Arts", "0", "3", "3", "2" },
                { "INTE-FE1", "BSIT Free Elective 1", "0", "3", "3", "2" },

                // Second Year - Second Semester
                { "INTE 202", "Integrative Programming and Technologies 1", "3", "2", "3", "2" },
                { "COMP 009", "Object-Oriented Programming", "3", "2", "3", "2" },
                { "COMP 010", "Information Management", "0", "3", "3", "2" },
                { "COMP 012", "Network Administration", "3", "2", "3", "2" },
                { "COMP 013", "Human Computer Interaction", "0", "3", "3", "2" },
                { "COMP 014", "Quantitative Methods with Modeling and Simulation", "0", "3", "3", "2" },
                { "INTE-FE2", "BSIT Free Elective 2", "0", "3", "3", "2" },

                // Third Year - First Semester
                { "INTE 401", "Information Assurance and Security 2", "0", "3", "3", "3" },
                { "CIT 3101", "Networking 2", "0", "3", "3", "3" },
                { "CIT 3102", "Systems Integration and Architecture", "0", "3", "3", "3" },
                { "CIT EL01", "Professional Elective 1", "0", "3", "3", "3" },
                { "CMR 1101", "Methods of Research for IT/IS", "0", "3", "3", "3" },
                { "CMS 1101", "Multimedia Systems", "0", "3", "3", "3" },
                { "GEED 0093", "Ethics", "0", "3", "3", "3" },
                { "GEED 0293", "Life and Works of Rizal", "0", "3", "3", "3" },

                // Third Year - Second Semester
                { "CIT 3201", "Networking 3", "0", "3", "3", "3" },
                { "CIT EL02", "Professional Elective 2", "0", "3", "3", "3" },
                { "CNA 1101", "Numerical Analysis for ITE", "0", "3", "3", "3" },
                { "CPP 4980", "Capstone Project and Research 1", "0", "3", "3", "3" },
                { "GEED 0073", "Art Appreciation", "0", "3", "3", "3" },
                { "GEED 0083", "Science, Technology, and Society", "0", "3", "3", "3" },
                { "GEED EL02", "GE Elective 2", "0", "3", "3", "3" },
                { "ZPD 1102", "Effective Communication with Personality Development", "0", "3", "3", "3" },

                // Fourth Year - First Semester
                { "CIT 4101", "Certification Course", "0", "3", "3", "4" },
                { "CIT 4102", "Systems Administration and Maintenance", "0", "3", "3", "4" },
                { "CIT EL03", "Professional Elective 3", "0", "3", "3", "4" },
                { "CIT EL04", "Professional Elective 4", "0", "3", "3", "4" },
                { "CPD 4990", "Capstone Project and Research 2", "0", "3", "3", "4" },
                { "CQM 1101", "Quantitative Methods (including Modeling and Simulation)", "0", "3", "3", "4" },
                { "GEED EL03", "GE Elective 3", "0", "3", "3", "4" },

                // Fourth Year - Second Semester
                { "CPR 4970", "Practicum for IT/IS", "0", "6", "6", "4" },
            };

            int totalRows = curriculumData.GetLength(0);
            try { dgvCurriculum.Rows.Add(totalRows > 35 ? totalRows : 35); } catch { }

            for (int row = 0; row < totalRows; row++)
            {
                for (int col = 0; col < 6; col++)
                {
                    dgvCurriculum.Rows[row].Cells[col].Value = curriculumData[row, col];
                }
            }

            string[,] archiveData =
            {
                { "1st Semester", "2025-2026", "Archived" },
                { "2nd Semester", "2026-2027", "Active" },
            };

            for (int row = 0; row < archiveData.GetLength(0); row++)
            {
                for (int col = 0; col < archiveData.GetLength(1); col++)
                {
                    dgvArchive.Rows[row].Cells[col].Value = archiveData[row, col];
                }
            }


            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    dgvEditSchedule.Rows[row].Cells[col].Value = dummyData[row, col];
                    dgvSchedule.Rows[row].Cells[col].Value = dummyData[row, col];
                    dgvScheduleView.Rows[row].Cells[col].Value = dummyData[row, col];

                }
            }

            for (int i = 0; i < 3; i++)
            {
                dgvSchedule.Rows[i].Cells["Year"].Value = "2";
            }
            // Mark original rows
            for (int i = 0; i < 3; i++)
            {
                dgvEditSchedule.Rows[i].Tag = "original";
            }

        }

        public class ContentPanelInfo
        {
            public Panel Panel { get; set; }
            public Action ResetAction { get; set; }
        }

        private void AddDuplicateButtonColumn()
        {
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            btn.Name = "colDuplicate";
            btn.HeaderText = "";
            btn.Text = "Duplicate";
            btn.UseColumnTextForButtonValue = true;

            dgvEditSchedule.Columns.Insert(11, btn); // 12th column
        }

        private void AddRemoveButtonColumn()
        {
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            btn.Name = "colRemove";
            btn.HeaderText = "";
            btn.Text = "Remove";
            btn.UseColumnTextForButtonValue = true;

            dgvEditSchedule.Columns.Insert(12, btn); // 13th column
        }

        private void dgvEditSchedule_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string colName = dgvEditSchedule.Columns[e.ColumnIndex].Name;

            if (colName == "colDuplicate")
            {
                DuplicateRow(e.RowIndex);
            }
            else if (colName == "colRemove")
            {
                RemoveRow(e.RowIndex);
            }
        }

        private void DuplicateRow(int rowIndex)
        {
            DataGridViewRow original = dgvEditSchedule.Rows[rowIndex];

            // Insert a new row right below the row duplicate is clicked
            dgvEditSchedule.Rows.Insert(rowIndex + 1, 1);
            int newRowIndex = rowIndex + 1; // The newly inserted row

            // Copy columns 0–4 (course info)
            for (int i = 0; i <= 4; i++)
            {
                dgvEditSchedule.Rows[newRowIndex].Cells[i].Value = original.Cells[i].Value;
            }

            dgvEditSchedule.Rows[newRowIndex].Tag = "Duplicate"; // Mark this row as a duplicate for potential future use (like styling or preventing multiple duplicates)]

            // Leave other columns blank let admin edit them as needed
        }

        private void RemoveRow(int rowIndex)
        {
            var row = dgvEditSchedule.Rows[rowIndex];

            // Block original rows
            if (row.Tag != null && row.Tag.ToString() == "original")
            {
                MessageBox.Show("You cannot remove original subjects.");
                return;
            }

            // Allow deletion for duplicates
            dgvEditSchedule.Rows.RemoveAt(rowIndex);
        }


        //--------------------------
        private void btnSO_CurriculumArchive_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);

            // Default — ipakita agad ang Curriculum
            pnlCurriculum.Visible = true;
            pnlArchive.Visible = false;
        }

        private void changeButtonColor(Button button)
        {
            if (clickedButton != null)
            {
                clickedButton.BackColor = defaultColor;
            }

            clickedButton = button;
            pnlYellow.Visible = true;
            pnlYellow.Parent = clickedButton;
            pnlYellow.Height = clickedButton.Height;
            pnlYellow.BringToFront();
            clickedButton.BackColor = selectedColor;
        }


        //Method na nagpapakita ng content ng bawat button, wala akong maisip na iba kaya eto
        private void showContent(Button button)
        {
            Dictionary<Button, Panel> contents = new Dictionary<Button, Panel> { };
            contents.Add(btnDashboard, pnlDashboardContent);
            contents.Add(btnSO_CurrentSemester, pnlCurrentSemester);
            contents.Add(btnSO_EditSchedule, pnlEditSchedule);
            contents.Add(btnSO_Schedule, pnlSchedule);
            contents.Add(btnSO_CurriculumArchive, pnlCurriculumArchive);


            //Kada button na aadd, maglagay ng panel sa form at lagay dito
            //Tapos, sa click event ng button, icall yung changeButtonColor(sender as Button) at showContent(clickedButton), eto lang ok na - Brylle
            foreach (KeyValuePair<Button, Panel> content in contents)
            {
                if (content.Key == button)
                {
                    //Automatic positioning, wag pakialaman maliban nalang kung binago ang position ng sidebar
                    content.Value.Parent = pnlContainerAdminPortal; //Nakalimutan ko ilagay kaya di mapakita - Brylle
                    content.Value.Dock = DockStyle.Fill;
                    content.Value.Visible = true;
                    content.Value.BringToFront();
                }
                else
                {
                    content.Value.Visible = false;
                }

                // Reset the panel we are leaving
                if (clickedButton != null && contentPanels.ContainsKey(clickedButton))
                {
                    contentPanels[clickedButton].ResetAction?.Invoke();
                }

                // Then show the new panel
                foreach (var kvp in contentPanels)
                {
                    bool isVisible = (kvp.Key == clickedButton);
                    FitContentPanel(kvp.Value.Panel);
                    kvp.Value.Panel.Visible = isVisible;
                }
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

        // Schedule panel
        private void btnSO_Schedule_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);

            dgvScheduleView.DefaultCellStyle.SelectionForeColor = Color.Black;
        }


        //-----------------------------------------------------(Edit Schedule)
        private void btnSO_EditSchedule_Click(object sender, EventArgs e)
        {
            //ShowPanel(pnlEditSchedule);
            //MessageBox.Show("Edit Schedule clicked.");
            changeButtonColor(sender as Button);
            showContent(clickedButton);

            cmbYearLevel_EditSchedule_SelectedIndexChanged(null, EventArgs.Empty);
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

            // Resize visible sub-content panels
            if (pnlGradesManagementContent.Visible) FitContentPanel(pnlGradesManagementContent);
            if (pnlAccountingRecordsContent.Visible)
            {
                FitContentPanel(pnlAccountingRecordsContent);
                ResizeAccountingRecordsContent();
            }
            if (pnlEnrolledStudentsContent.Visible)
            {
                FitContentPanel(pnlEnrolledStudentsContent);
                ResizeEnrolledStudentsContent();
            }
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
            pnlsubofferingSubmenu.Visible = !pnlsubofferingSubmenu.Visible;
            if (pnlsubofferingSubmenu.Visible)
                btnSubjectOffering.Text = " Subject Offering                    ⌄";
            else
                btnSubjectOffering.Text = " Subject Offering                     ›";
        }

        //-------------------------------------------------------------- (1 current sem page)
        private void btnSO_CurrentSemester_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);

            // Show Current Semester panel
            //pnlSubOfferingContent.Visible = true;
            //pnlCurrentSemester.Visible = true;

            // Hide other content panels
            //pnlEditSchedule.Visible = false;
            //pnlOtherPanel.Visible = false;

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

        //-------------------------------------------------------------- (events current sem page)
        private void pnlCoursesContent_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSetCurrent_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Set {cmbSY.SelectedItem} semester {cmbSem.SelectedItem} as current.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //-------------------------------------------------------------- (events Edit schedule page)

        //edit sched
        private void btnSaveSchedule_Click(object sender, EventArgs e)
        {
            string[] fieldNames = {
            "Course Code", "Course Title", "Lec Units", "Lab Units", "Total Units",
            "Section", "Day", "Start Time", "End Time", "Room", "Instructor" };


            List<string> errors = new List<string>();

            foreach (DataGridViewRow row in dgvEditSchedule.Rows)
            {
                if (row.IsNewRow) continue;

                bool hasAnyData = false;
                for (int col = 0; col <= 10; col++)
                {
                    if (col < row.Cells.Count && !string.IsNullOrWhiteSpace(row.Cells[col].Value?.ToString()))
                    {
                        hasAnyData = true;
                        break;
                    }
                }
                if (!hasAnyData) continue;

                List<string> missingFields = new List<string>();
                for (int col = 0; col <= 10; col++)
                {
                    if (col >= row.Cells.Count) break;
                    if (string.IsNullOrWhiteSpace(row.Cells[col].Value?.ToString()))
                        missingFields.Add(fieldNames[col]);
                }

                if (missingFields.Count > 0)
                    errors.Add($"Row {row.Index + 1}: Missing — {string.Join(", ", missingFields)}");
            }

            if (errors.Count > 0)
            {
                MessageBox.Show("Please complete all fields before saving.\n\n" + string.Join("\n", errors),
                    "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _savedSchedule.Clear();

            dgvScheduleView.Rows.Clear();
            foreach (var saved in _savedSchedule)
                dgvScheduleView.Rows.Add(saved);

            while (dgvScheduleView.Rows.Count < 30)
                dgvScheduleView.Rows.Add();

        }

        private void lblESCurrentSem_Click(object sender, EventArgs e)
        {

        }

        private void lblESYearLevel_Click(object sender, EventArgs e)
        {

        }

        private void pnlEditSchedule_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblSem_Click(object sender, EventArgs e)
        {

        }

        private void lblSemesterSetup_Click(object sender, EventArgs e)
        {

        }

        //edit sched
        private void btnClearSchedule_Click_1(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Are you sure you want to clear the schedule? Unsaved changes will be lost.",
                "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            //remove non-original rows
            for (int i = dgvEditSchedule.Rows.Count - 1; i >= 0; i--)
            {
                var row = dgvEditSchedule.Rows[i];
                if (row.IsNewRow) continue;
                if (row.Tag?.ToString() == "original") continue;
                dgvEditSchedule.Rows.RemoveAt(i);
            }

            foreach (DataGridViewRow row in dgvEditSchedule.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Tag?.ToString() != "original") continue;

                for (int col = 5; col <= 10; col++)
                {
                    if (col < row.Cells.Count)
                        row.Cells[col].Value = null;
                }
            }

            while (dgvEditSchedule.Rows.Count < 30)
                dgvEditSchedule.Rows.Add();

            MessageBox.Show("Schedule cleared.", "Cleared",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //schedule view only buttons
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Exporting the schedule will overwrite the existing file if it already exists. Do you want to continue?", "Export to Excel", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Implement export logic here
                MessageBox.Show("Schedule exported to Excel successfully!", "Exported", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Exporting the schedule will overwrite the existing file if it already exists. Do you want to continue?", "Export to PDF", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Implement export logic here
                MessageBox.Show("Schedule exported to PDF successfully!", "Exported", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to print this schedule?", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        }

        private void btnCurriculum_Click_1(object sender, EventArgs e)
        {
            pnlCurriculum.Visible = true;
            pnlArchive.Visible = false;
        }

        private void btnArchive_Click(object sender, EventArgs e)
        {
            pnlArchive.Visible = true;
            pnlCurriculum.Visible = false;
        }

        private void btnUpdateCurriculum_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to update the curriculum?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Curriculum updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //backend
        //match year level with section column options 
        private void cmbYearLevel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //dapat magappear lang yung subjects for specific year level
        private void cmbYearLevel_EditSchedule_SelectedIndexChanged(object sender, EventArgs e)
        {


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

            btnDashboard.PerformClick();

            // Grades Management initial setup (with null checks)
            if (pnlGMAddNewGradeForm == null)
                MessageBox.Show("pnlGMAddNewGradeForm is null! Check designer name.");
            else
                originalFormTop = pnlGMAddNewGradeForm.Top;

            if (pnlGMAddNewGradeForm != null)
                pnlGMAddNewGradeForm.Visible = false;

            if (pnlGradesManagementContainer != null)
                pnlGradesManagementContainer.Top = originalFormTop;
            else
                MessageBox.Show("pnlGradesManagementContainer is null! Check designer name.");

            // Setup grades data table
            gradesTable = new DataTable();
            gradesTable.Columns.Add("StudentName", typeof(string));
            gradesTable.Columns.Add("StudentID", typeof(string));
            gradesTable.Columns.Add("SubjectCode", typeof(string));
            gradesTable.Columns.Add("SubjectName", typeof(string));
            gradesTable.Columns.Add("Semester", typeof(string));
            gradesTable.Columns.Add("AcademicYear", typeof(string));
            gradesTable.Columns.Add("Midterm", typeof(decimal));
            gradesTable.Columns.Add("Final", typeof(decimal));
            gradesTable.Columns.Add("FinalRating", typeof(decimal));
            gradesTable.Columns.Add("Remarks", typeof(string));

            dgvGrades.DataSource = gradesTable;
            dgvGrades.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            // Initially hide the grid and show the placeholder if no rows
            UpdateGradesPlaceholderVisibility();

            // Get distinct years from student data
            var years = studentList.Select(s => s[4]).Distinct().ToList();

            // Define all standard years you want to show
            var allStandardYears = new List<string> { "1st Year", "2nd Year", "3rd Year", "4th Year" };

            // Add any missing standard years to the list
            foreach (var stdYear in allStandardYears)
            {
                if (!years.Contains(stdYear))
                    years.Add(stdYear);
            }

            // Sort years numerically (based on the number before "st"/"nd"/"rd"/"th")
            years = years.OrderBy(y => int.Parse(new string(y.TakeWhile(char.IsDigit).ToArray()))).ToList();

            // Insert "All Years" at the top
            years.Insert(0, "All Years");

            cmbGMYear.DataSource = years;
            cmbGMYear.SelectedIndex = 0;

            // Initial population of student combo
            FilterStudentComboBox();

            btnGMSearch.Click += (s, e) => FilterStudentComboBox();
            cmbGMYear.SelectedIndexChanged += (s, e) => FilterStudentComboBox();
            cmbGMSection.SelectedIndexChanged += (s, e) => FilterStudentComboBox();
        }

        private void FilterStudentComboBox()
        {
            string searchTerm = txGMSearchBar.Text.Trim().ToLower();
            string selectedYear = cmbGMYear.SelectedItem?.ToString();
            string selectedSection = cmbGMSection.SelectedItem?.ToString();

            // Treat "All Years" as no filter
            if (selectedYear == "All Years") selectedYear = null;

            // Section filtering – currently disabled because studentList has no section data.
            // If you later add a section field (e.g., s[6]), uncomment the appropriate lines.
            bool useSectionFilter = false; // set to true when you have section data
            string sectionFilter = null;
            if (useSectionFilter && selectedSection != null && selectedSection != "All Sections")
                sectionFilter = selectedSection;

            var filtered = studentList.Where(s =>
            {
                bool matchesSearch = string.IsNullOrEmpty(searchTerm) ||
                    s[0].ToLower().Contains(searchTerm) ||
                    s[1].ToLower().Contains(searchTerm) ||
                    s[2].ToLower().Contains(searchTerm);

                bool matchesYear = selectedYear == null || s[4] == selectedYear;

                bool matchesSection = true;
                if (useSectionFilter && sectionFilter != null)
                {
                    // Example: assume section is stored in s[6]
                    // matchesSection = s[6] == sectionFilter;
                }

                return matchesSearch && matchesYear && matchesSection;
            }).Select(s => new { ID = s[0], Name = $"{s[1]} ({s[0]})" }).ToList();

            cmbGMStudent.DataSource = filtered;
            cmbGMStudent.DisplayMember = "Name";
            cmbGMStudent.ValueMember = "ID";


        }

        private void UpdateGradesPlaceholderVisibility()
        {
            bool hasRows = gradesTable.Rows.Count > 0;
            //label38.Visible = !hasRows;    // "No grades found"
            //pictureBox7.Visible = !hasRows;
            dgvGrades.Visible = hasRows;
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

        private void txtSearchViewAUs_TextChanged(object sender, EventArgs e)
        {
            ApplyFiltersAndRefresh();
        }

        private void btnDashboardRegisterStudent_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            btnRegisterStudent.PerformClick();
        }

        private void btnDashboardRegisterProfessor_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            btnRegisterProfessor.PerformClick();
        }

        private void btnDashboardViewAllUsers_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            btnViewAllUsers.PerformClick();
        }

        private void btnGMAddGrades_Click(object sender, EventArgs e)
        {
            // Toggle visibility
            bool showForm = !pnlGMAddNewGradeForm.Visible;

            if (showForm)
            {
                pnlGMAddNewGradeForm.Visible = true;
                // Push container down below the form (10px gap)
                pnlGradesManagementContainer.Top = pnlGMAddNewGradeForm.Bottom + 10;
            }
            else
            {
                pnlGMAddNewGradeForm.Visible = false;
                // Bring container back to the form's original position
                pnlGradesManagementContainer.Top = originalFormTop;
            }
        }

        private void btmGMClearForm_Click(object sender, EventArgs e)
        {
            // Clear all text boxes and comboboxes inside pnlGMAddNewGradeForm
            foreach (Control ctrl in pnlGMAddNewGradeForm.Controls)
            {
                if (ctrl is TextBox tb) tb.Clear();
                else if (ctrl is ComboBox cb) cb.SelectedIndex = -1;
            }
            // Reset year combo to "All Years"
            cmbGMYear.SelectedIndex = 0;
            // Reset section combo to its default (e.g., index 0)
            cmbGMSection.SelectedIndex = 0;
            // Clear the search box
            txGMSearchBar.Clear();
            // Reset student combo
            FilterStudentComboBox();
        }

        private void btnGMSaveGrades_Click(object sender, EventArgs e)
        {
            // Validate required fields
            if (cmbGMStudent.SelectedItem == null)
            {
                MessageBox.Show("Please select a student.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(txtGMSubjectCode.Text))
            {
                MessageBox.Show("Subject Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(txtGMSubjectName.Text))
            {
                MessageBox.Show("Subject Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbGMSemester.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a semester.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(txtGMAcadYear.Text))
            {
                MessageBox.Show("Academic Year is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Parse grades
            if (!decimal.TryParse(txtGMMidtermGrade.Text, out decimal midterm))
            {
                MessageBox.Show("Invalid Midterm Grade.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!decimal.TryParse(txtGMFinalGrade.Text, out decimal final))
            {
                MessageBox.Show("Invalid Final Grade.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Calculate final rating
            decimal finalRating = (midterm + final) / 2;
            txtGMFinalRating.Text = finalRating.ToString("0.00");

            // Get descriptive remarks
            string remarks = GetGradeDescription(finalRating);
            txtGMRemarks.Text = remarks;

            // Get selected student
            dynamic selected = cmbGMStudent.SelectedItem;
            string studentName = selected.Name;
            string studentID = selected.ID;

            // Add to DataTable
            gradesTable.Rows.Add(
                studentName,
                studentID,
                txtGMSubjectCode.Text,
                txtGMSubjectName.Text,
                cmbGMSemester.SelectedItem.ToString(),
                txtGMAcadYear.Text,
                midterm,
                final,
                finalRating,
                remarks
            );


            // Update placeholder visibility
            UpdateGradesPlaceholderVisibility();

            // Clear the entry form
            btmGMClearForm_Click(sender, e);

            // Hide the add panel and restore container position
            pnlGMAddNewGradeForm.Visible = false;
            pnlGradesManagementContainer.Top = originalFormTop;

            MessageBox.Show("Grade saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string GetGradeDescription(decimal finalRating)
        {
            if (finalRating <= 1.00m) return "Excellent";
            if (finalRating <= 1.25m) return "Very Good";
            if (finalRating <= 1.50m) return "Very Good";
            if (finalRating <= 1.75m) return "Good";
            if (finalRating <= 2.00m) return "Good";
            if (finalRating <= 2.25m) return "Satisfactory";
            if (finalRating <= 2.50m) return "Satisfactory";
            if (finalRating <= 2.75m) return "Fair";
            if (finalRating <= 3.00m) return "Passed";
            return "Failed";
        }

        private void ResizeEnrolledStudentsContent()
        {
            if (!pnlEnrolledStudentsContent.Visible) return;

            int contentWidth = pnlEnrolledStudentsContent.ClientSize.Width - 32; // use ClientSize, not Width
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

        private void ResizeAccountingRecordsContent()
        {
            if (!pnlAccountingRecordsContent.Visible) return;

            // Usable width inside the panel (subtract side padding, e.g., 16px left + 16px right)
            int contentWidth = pnlAccountingRecordsContent.Width - 32; // 16px each side
            int gap = 10; // space between cards

            // Three cards: Total Amount, Paid Amount, Unpaid Amount
            int cardWidth = (contentWidth - (gap * 2)) / 3;

            // Resize and reposition the three summary cards
            pnlARTotalAmount.Width = cardWidth;
            pnlARPaidAmount.Width = cardWidth;
            pnlARUnpaidAmount.Width = cardWidth;

            pnlARTotalAmount.Left = 16;
            pnlARPaidAmount.Left = 16 + cardWidth + gap;
            pnlARUnpaidAmount.Left = 16 + (cardWidth + gap) * 2;

        }
    }

}


