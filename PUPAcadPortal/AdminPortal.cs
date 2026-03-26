using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
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
        private Dictionary<Button, Panel> contentPanels; // Class-level dictionary to hold button-panel mappings
        public AdminPortal()
        {
            InitializeComponent();
            this.Resize += AdminPortal_Resize;

            // Initialize the dictionary with main sidebar buttons
            contentPanels = new Dictionary<Button, Panel>
            {
            { btnDashboard, pnlDashboardContent },
            { btnEnrollments, pnlEnrollContent },
            { btnSubjectOffering, pnlSubOfferingContent },
            { btnGradesManagement, pnlGradesManagementContent },
            { btnAccountingRecords, pnlAccountingRecordsContent },
            { btnEnrolledStudents, pnlEnrolledStudentsContent },
            { btnRegisterStudent, pnlRegisterStudentContent },
            { btnRegisterProfessor, pnlRegisterProfessorContent },
            { btnViewAllUsers, pnlViewAllUsersContent }
                // LMS is not included here because it toggles a submenu
            };
        private List<string[]> _savedSchedule = new List<string[]>();

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


            pnlEditSchedule.Visible = false;
            pnlCurrentSemester.Visible = false;
            pnlSubOfferingContent.Visible = false;
            pnlCurriculumArchive.Visible = false;

            this.WindowState = FormWindowState.Maximized;

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
                { "2025-2026", "1st Semester", "Archived" },
                { "2026-2027", "2nd Semester", "Active" },
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
            pnlYellow.Parent = clickedButton.Parent;
            pnlYellow.Height = clickedButton.Height;
            pnlYellow.Location = clickedButton.Location;
            pnlYellow.BringToFront();
            clickedButton.BackColor = selectedColor;
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

      

        //-------------------------------------


        //Method na nagpapakita ng content ng bawat button, wala akong maisip na iba kaya eto
        private void showContent(Button button)
        {
            foreach (var kvp in contentPanels)
            {
                if (kvp.Key == clickedButton)
                {
                    //Automatic positioning, wag pakialaman maliban nalang kung binago ang position ng sidebar
                    //content.Value.Location = new Point(pnlSidebar.Size.Width, pnlHeader.Size.Height);
                    FitContentPanel(kvp.Value);
                    kvp.Value.Visible = true;
                }
                else
                {
                    kvp.Value.Visible = false;
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

        private void AdminPortal_Resize(object sender, EventArgs e)
        {
            if (!IsHandleCreated) return;

            // Resize the currently visible main content panel (if any)
            foreach (var kvp in contentPanels) // you'll need a class-level dictionary for this
            {
                if (kvp.Value.Visible)
                {
                    FitContentPanel(kvp.Value);
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
                kvp.Value.Visible = false;
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
        }

        private void btnEnrollments_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnSubjectOffering_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
            pnlsubofferingSubmenu.Visible = !pnlsubofferingSubmenu.Visible;
            if (pnlsubofferingSubmenu.Visible)
                btnSubjectOffering.Text = " Subject Offering                    ⌄";
            else
                btnSubjectOffering.Text = " Subject Offering                     ›";
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
            showContent(clickedButton);

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
            btnRegistrarFunctions.Text = " Registrar Functions    ›";
        }

        private void btnAccountingRecords_Click(object sender, EventArgs e)
        {
            ShowSubContent(pnlAccountingRecordsContent);
            //pnlRegistrarSubmenu.Visible = false;
            btnRegistrarFunctions.Text = " Registrar Functions    ›";
        }

        private void btnEnrolledStudents_Click(object sender, EventArgs e)
        {
            ShowSubContent(pnlEnrolledStudentsContent);
            //pnlRegistrarSubmenu.Visible = false;
            btnRegistrarFunctions.Text = " Registrar Functions    ›";
        }

        // Other sidebar buttons

        private void bgtnRegisterStudents_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnRegisterProf_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnViewAllUsers_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
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

        //match year level with section column options 
        private void cmbYearLevel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //dapat magappear lang yung subjects for specific year level
        private void cmbYearLevel_EditSchedule_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

    }

}


