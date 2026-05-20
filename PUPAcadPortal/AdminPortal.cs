using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        // ── Announcement system ──────────────────────────────────────────
        private List<AdminAnnouncement> announcements = new List<AdminAnnouncement>();
        private int editingAnnouncementId = -1;
        private string _activeCategoryFilter = "all";
        private string _activeStatusFilter = "all";
        private string _searchQuery = "";
        private string _activeSortMode = "newest";   // NEW

        private CreateAnnouncementAdmin _createAnnouncementUC;
        private ViewAnnouncementAdmin _viewAnnouncementUC;

        private static readonly Dictionary<string, Color> AnnCatIconColor = new Dictionary<string, Color>
        {
            ["General"] = Color.FromArgb(0x37, 0x8a, 0xdd),
            ["Academic"] = Color.FromArgb(0x63, 0x99, 0x22),
            ["Events"] = Color.FromArgb(0xd4, 0x53, 0x7e),
            ["Emergency"] = Color.FromArgb(0xc8, 0x1e, 0x1e),
            ["Enrollment"] = Color.FromArgb(0x0d, 0x9a, 0x8a),
        };
        private static readonly Dictionary<string, Color> AnnCatBgColor = new Dictionary<string, Color>
        {
            ["General"] = Color.FromArgb(0xe6, 0xf1, 0xfb),
            ["Academic"] = Color.FromArgb(0xea, 0xf3, 0xde),
            ["Events"] = Color.FromArgb(0xfb, 0xea, 0xf0),
            ["Emergency"] = Color.FromArgb(0xff, 0xe0, 0xe0),
            ["Enrollment"] = Color.FromArgb(0xd6, 0xf4, 0xf1),
        };

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
            contents.Add(btnAnnouncement, pnlAnnouncement);
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
            this.Hide();
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

        private void btnAnnouncement_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
            InitAnnouncementPanelIfNeeded();
        }

        // ════════════════════════════════════════════════════════════════
        //  ANNOUNCEMENT SYSTEM — ADMIN PORTAL
        // ════════════════════════════════════════════════════════════════

        private bool _announcementInited = false;

        private void InitAnnouncementPanelIfNeeded()
        {
            if (_announcementInited) return;
            _announcementInited = true;

            // Wire search / filter controls that live in the designer panel
            // (add null-checks so it won't crash if controls are renamed)
            if (txtAnnSearch != null)
                txtAnnSearch.TextChanged += (s, e) => { _searchQuery = txtAnnSearch.Text.Trim(); RenderAnnouncements(); };

            if (cmbFilter != null)
            {
                if (cmbFilter.Items.Count == 0)
                    cmbFilter.Items.AddRange(new object[]
                    {
                        "All announcements",
                        "Active only",
                        "Inactive only",
                        "Pinned only",
                        "Urgent only",
                        "With attachment",
                        "Notified – Students",
                        "Notified – Instructors",
                    });
                cmbFilter.SelectedIndex = 0;
                cmbFilter.SelectedIndexChanged += (s, e) =>
                {
                    _activeStatusFilter = cmbFilter.SelectedItem?.ToString() switch
                    {
                        "Active only" => "active",
                        "Inactive only" => "inactive",
                        "Pinned only" => "pinned",
                        "Urgent only" => "urgent",
                        "With attachment" => "attachment",
                        "Notified – Students" => "notify_students",
                        "Notified – Instructors" => "notify_instructors",
                        _ => "all",
                    };
                    RenderAnnouncements();
                };
            }

            if (cmbSortBy != null)
            {
                if (cmbSortBy.Items.Count == 0)
                    cmbSortBy.Items.AddRange(new object[]
                    {
                        "Newest first",
                        "Oldest first",
                        "A → Z (Title)",
                        "Z → A (Title)",
                        "Most viewed",
                        "Least viewed",
                        "Pinned first",
                        "Urgent first",
                    });
                cmbSortBy.SelectedIndex = 0;
                cmbSortBy.SelectedIndexChanged += (s, e) =>
                {
                    _activeSortMode = cmbSortBy.SelectedItem?.ToString() switch
                    {
                        "Oldest first" => "oldest",
                        "A → Z (Title)" => "az",
                        "Z → A (Title)" => "za",
                        "Most viewed" => "most_viewed",
                        "Least viewed" => "least_viewed",
                        "Pinned first" => "pinned",
                        "Urgent first" => "urgent",
                        _ => "newest",
                    };
                    RenderAnnouncements();
                };
            }

            if (btnCreateAnnouncement != null)
                btnCreateAnnouncement.Click += (s, e) =>
                {
                    editingAnnouncementId = -1;
                    _createAnnouncementUC.LoadForEdit(new AnnouncementDataAdmin());
                    ShowCreateAnnouncementUC();
                };

            // Create + wire the CreateAnnouncement UC
            _createAnnouncementUC = new CreateAnnouncementAdmin { Visible = false, Anchor = AnchorStyles.None };
            _createAnnouncementUC.AnnouncementPosted += OnAnnouncementPosted;
            _createAnnouncementUC.CloseRequested += (s, e) => HideCreateAnnouncementUC();
            pnlAnnouncement.Controls.Add(_createAnnouncementUC);

            // Create + wire the ViewAnnouncement UC
            _viewAnnouncementUC = new ViewAnnouncementAdmin { Visible = false, Anchor = AnchorStyles.None };
            _viewAnnouncementUC.EditRequested += OnViewEdit;
            _viewAnnouncementUC.DeleteRequested += (s, id) => DeleteAnnouncement(id);
            _viewAnnouncementUC.CloseRequested += (s, e) => HideViewAnnouncementUC();
            pnlAnnouncement.Controls.Add(_viewAnnouncementUC);

            // Resize → re-center overlay UCs
            pnlAnnouncement.Resize += (s, e) =>
            {
                if (_createAnnouncementUC.Visible) CenterAnnControl(_createAnnouncementUC);
                if (_viewAnnouncementUC.Visible) CenterAnnControl(_viewAnnouncementUC);
                RenderAnnouncements();
                BuildAnnCategorySidebar();
                RenderAnnInsights();
            };

            // Seed data and first render
            SeedAnnouncements();
            BuildAnnCategorySidebar();
            RenderAnnouncements();
        }

        // ── Seed sample data ─────────────────────────────────────────────
        private void SeedAnnouncements()
        {
            announcements.AddRange(new[]
            {
                new AdminAnnouncement { Id=1,  Title="Midterm exam schedule released",
                    Description="The official midterm examination schedule for all BSIT 2nd year subjects has been posted. Please check your respective rooms and ensure you bring your student IDs.",
                    Category="Academic", Status="active", IsPinned=true, IsUrgent=true,
                    NotifyStudents=true, NotifyInstructors=true,
                    ViewedCount=28, TotalStudents=40, Date=DateTime.Now },
                new AdminAnnouncement { Id=2,  Title="Class suspension – May 12",
                    Description="All classes on Monday, May 12 are suspended due to the declared public holiday.",
                    Category="General", Status="active", ViewedCount=35, TotalStudents=40,
                    NotifyStudents=true, Date=DateTime.Now.AddHours(-5) },
                new AdminAnnouncement { Id=3,  Title="Programming 1 – lab activity this Friday",
                    Description="Bring your laptops for the graded lab activity covering Modules 4 and 5.",
                    Category="Academic", Status="active", ViewedCount=22, TotalStudents=40, Date=DateTime.Now.AddDays(-2) },
                new AdminAnnouncement { Id=4,  Title="Campus foundation day celebration",
                    Description="Join us for the PUP Foundation Day celebration on May 17.",
                    Category="Events", Status="inactive", ViewedCount=18, TotalStudents=40, Date=DateTime.Now.AddDays(-4) },
                new AdminAnnouncement { Id=5,  Title="Reminder: submit assignment outputs",
                    Description="All pending assignment outputs must be submitted via the LMS before May 15, 11:59 PM.",
                    Category="General", Status="active", ViewedCount=30, TotalStudents=40, Date=DateTime.Now.AddDays(-5) },
                new AdminAnnouncement { Id=6,  Title="Final Exam Coverage – Programming 1",
                    Description="The official final examination coverage for Introduction to Programming 1 has been posted.",
                    Category="Academic", Status="active", IsPinned=true, ViewedCount=32, TotalStudents=40,
                    NotifyStudents=true, NotifyInstructors=true, Date=DateTime.Now.AddHours(-2) },
                new AdminAnnouncement { Id=7,  Title="Graded Recitation – Information Management",
                    Description="There will be a graded recitation next Wednesday covering database normalization.",
                    Category="Academic", Status="active", ViewedCount=15, TotalStudents=40, Date=DateTime.Now.AddDays(-1) },
                new AdminAnnouncement { Id=8,  Title="Enrollment Period – 1st Semester 2026-2027",
                    Description="Online enrollment for 1st Semester AY 2026-2027 opens on June 1. Please coordinate with your respective advisers for pre-enrollment clearance.",
                    Category="Enrollment", Status="active", ViewedCount=38, TotalStudents=40,
                    NotifyStudents=true, Date=DateTime.Now.AddDays(-1).AddHours(-3) },
                new AdminAnnouncement { Id=9,  Title="IT Career Fair – Volunteer Marshals Needed",
                    Description="The Career Services Office is looking for 15 student volunteers to serve as event marshals.",
                    Category="Events", Status="active", ViewedCount=20, TotalStudents=40, Date=DateTime.Now.AddDays(-2) },
                new AdminAnnouncement { Id=10, Title="Capstone Group Submissions – Revised Deadline",
                    Description="The submission deadline for Capstone Project Chapter 3 has been moved to May 19.",
                    Category="Academic", Status="active", IsPinned=true, IsUrgent=true, ViewedCount=36, TotalStudents=40,
                    NotifyStudents=true, Date=DateTime.Now.AddDays(-3) },
                new AdminAnnouncement { Id=11, Title="Enrollment Requirements Reminder",
                    Description="All students must submit updated documents (medical certificate, good moral clearance) before enrollment. Incomplete requirements will not be processed.",
                    Category="Enrollment", Status="active", ViewedCount=25, TotalStudents=40,
                    NotifyStudents=true, NotifyInstructors=true, Date=DateTime.Now.AddDays(-4) },
                new AdminAnnouncement { Id=12, Title="University Foundation Day – May 19",
                    Description="All classes are suspended on May 19 in celebration of the University's Foundation Day.",
                    Category="Events", Status="active", ViewedCount=29, TotalStudents=40, Date=DateTime.Now.AddDays(-5) },
                new AdminAnnouncement { Id=13, Title="⚠ Campus Emergency Drill – May 20",
                    Description="A mandatory earthquake and fire emergency drill will be conducted on May 20. All students and faculty must participate. Classes will be briefly suspended during the drill.",
                    Category="Emergency", Status="active", IsPinned=true, IsUrgent=true,
                    NotifyStudents=true, NotifyInstructors=true,
                    ViewedCount=40, TotalStudents=40, Date=DateTime.Now.AddHours(-1) },
            });
        }

        // ── Render cards ─────────────────────────────────────────────────
        private void RenderAnnouncements()
        {
            if (fplAnnouncement == null) return;

            fplAnnouncement.SuspendLayout();
            fplAnnouncement.Controls.Clear();
            fplAnnouncement.FlowDirection = FlowDirection.TopDown;
            fplAnnouncement.WrapContents = false;
            fplAnnouncement.AutoScroll = true;
            fplAnnouncement.HorizontalScroll.Enabled = false;
            fplAnnouncement.HorizontalScroll.Visible = false;

            var filtered = announcements.AsEnumerable();

            if (_activeCategoryFilter != "all")
                filtered = filtered.Where(a => a.Category == _activeCategoryFilter);

            filtered = _activeStatusFilter switch
            {
                "active" => filtered.Where(a => a.Status == "active"),
                "inactive" => filtered.Where(a => a.Status != "active"),
                "pinned" => filtered.Where(a => a.IsPinned),
                "urgent" => filtered.Where(a => a.IsUrgent),
                "attachment" => filtered.Where(a => !string.IsNullOrWhiteSpace(a.AttachedFile)),
                "notify_students" => filtered.Where(a => a.NotifyStudents),
                "notify_instructors" => filtered.Where(a => a.NotifyInstructors),
                _ => filtered,
            };

            if (!string.IsNullOrWhiteSpace(_searchQuery))
            {
                var q = _searchQuery.ToLower();
                filtered = filtered.Where(a =>
                    a.Title.ToLower().Contains(q) || a.Description.ToLower().Contains(q)
                    || a.Category.ToLower().Contains(q));
            }

            var sorted = _activeSortMode switch
            {
                "oldest" => filtered.OrderBy(a => a.IsPinned ? 0 : 1).ThenBy(a => a.Date),
                "az" => filtered.OrderBy(a => a.IsPinned ? 0 : 1).ThenBy(a => a.Title),
                "za" => filtered.OrderBy(a => a.IsPinned ? 0 : 1).ThenByDescending(a => a.Title),
                "most_viewed" => filtered.OrderBy(a => a.IsPinned ? 0 : 1).ThenByDescending(a => a.ViewedCount),
                "least_viewed" => filtered.OrderBy(a => a.IsPinned ? 0 : 1).ThenBy(a => a.ViewedCount),
                "pinned" => filtered.OrderBy(a => a.IsPinned ? 0 : 1).ThenByDescending(a => a.Date),
                "urgent" => filtered.OrderBy(a => a.IsUrgent ? 0 : 1).ThenByDescending(a => a.Date),
                _ => filtered.OrderBy(a => a.IsPinned ? 0 : 1).ThenByDescending(a => a.Date),
            };
            var sortedList = sorted.ToList();

            int panelWidth = Math.Max(300, fplAnnouncement.ClientSize.Width - 30);

            foreach (var a in sortedList)
            {
                var card = BuildAdminAnnouncementCard(a, panelWidth);
                card.Margin = new Padding(4, 4, 4, 4);
                fplAnnouncement.Controls.Add(card);
            }

            if (lblShowing != null)
                lblShowing.Text = sortedList.Count == 0
                    ? "No announcements found"
                    : $"Showing 1–{sortedList.Count} of {announcements.Count} announcements";

            fplAnnouncement.ResumeLayout();
            BuildAnnCategorySidebar();
            RenderAnnPinned();
            RenderAnnInsights();
        }

        // ── Self-contained card builder (no stub dependency) ─────────────
        private Panel BuildAdminAnnouncementCard(AdminAnnouncement a, int cardWidth)
        {
            Color iconCol = AnnCatIconColor.GetValueOrDefault(a.Category, Color.Gray);
            Color iconBg = AnnCatBgColor.GetValueOrDefault(a.Category, Color.WhiteSmoke);
            bool isNew = (DateTime.Now - a.Date) < TimeSpan.FromDays(1);

            // Extra bottom rows needed?
            bool hasNotify = a.NotifyStudents || a.NotifyInstructors;
            bool hasAttach = !string.IsNullOrWhiteSpace(a.AttachedFile);
            int extraHeight = (hasNotify || hasAttach) ? 22 : 0;
            int cardHeight = 112 + extraHeight;

            var card = new Panel
            {
                Width = cardWidth,
                Height = cardHeight,
                BackColor = a.Category == "Emergency"
                    ? Color.FromArgb(255, 245, 245)
                    : a.Status == "active" ? Color.White : Color.FromArgb(250, 250, 250),
                Cursor = Cursors.Hand,
                Tag = a.Id,
            };
            card.Paint += (s, pe) =>
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen = new Pen(Color.FromArgb(218, 218, 218), 1f);
                using var path = AnnRoundedPath(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 8);
                pe.Graphics.DrawPath(pen, path);
            };

            // ── Icon circle ──────────────────────────────────────────────
            var icon = new Panel { Size = new Size(42, 42), Location = new Point(12, 16), BackColor = iconBg };
            icon.Paint += (s, pe) =>
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var b = new SolidBrush(iconBg);
                pe.Graphics.FillEllipse(b, 0, 0, 41, 41);
                string letter = a.Category.Length > 0 ? a.Category.Substring(0, 1) : "?";
                using var f = new Font("Segoe UI", 13f, FontStyle.Bold);
                using var fb = new SolidBrush(iconCol);
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                pe.Graphics.DrawString(letter, f, fb, new RectangleF(0, 0, 42, 42), sf);
            };
            card.Controls.Add(icon);

            int textX = 64;
            int rightM = 12;   // right margin

            // ── NEW ribbon ───────────────────────────────────────────────
            if (isNew)
            {
                var ribbon = new System.Windows.Forms.Label
                {
                    AutoSize = false,
                    Size = new Size(38, 16),
                    Location = new Point(0, 0),
                    BackColor = Color.FromArgb(22, 163, 74),
                    ForeColor = Color.White,
                    Text = "NEW",
                    Font = new Font("Segoe UI", 7f, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                AnnMakeRounded(ribbon, 6);
                card.Controls.Add(ribbon);
                ribbon.BringToFront();
            }

            // ── ⋮ menu button ─────────────────────────────────────────────
            var btnMenu = new Button
            {
                Size = new Size(28, 28),
                Location = new Point(cardWidth - 36, 8),
                FlatStyle = FlatStyle.Flat,
                Text = "⋮",
                Font = new Font("Segoe UI", 11f),
                ForeColor = Color.FromArgb(150, 150, 150),
                Cursor = Cursors.Hand,
                TabStop = false,
                BackColor = Color.Transparent,
            };
            btnMenu.FlatAppearance.BorderSize = 0;
            btnMenu.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);

            var capturedId = a.Id;
            var capturedTitle = a.Title;

            var ctx = new ContextMenuStrip();
            ctx.Items.Add("✏  Edit", null, (s, ev) => EditAnnouncement(capturedId));
            ctx.Items.Add(a.IsPinned ? "📌  Unpin" : "📌  Pin", null, (s, ev) =>
            {
                var ann = announcements.Find(x => x.Id == capturedId);
                if (ann != null) { ann.IsPinned = !ann.IsPinned; RenderAnnouncements(); }
            });
            ctx.Items.Add(a.Status == "active" ? "⏸  Set inactive" : "▶  Set active", null, (s, ev) =>
            {
                var ann = announcements.Find(x => x.Id == capturedId);
                if (ann != null) { ann.Status = ann.Status == "active" ? "inactive" : "active"; RenderAnnouncements(); }
            });
            ctx.Items.Add(new ToolStripSeparator());
            ctx.Items.Add("🗑  Delete", null, (s, ev) =>
            {
                if (MessageBox.Show($"Delete \"{capturedTitle}\"?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    DeleteAnnouncement(capturedId);
            });
            btnMenu.Click += (s, ev) => ctx.Show(btnMenu, new Point(0, btnMenu.Height));
            card.Controls.Add(btnMenu);

            // ── Date label (left-aligned after textX, top row) ───────────
            // Keep date width bounded so it never crowds the menu button
            int dateMaxW = Math.Max(60, cardWidth - textX - 40);
            string dateText = a.Date.ToString("MMM d, yyyy  h:mm tt");
            var lblDate = new System.Windows.Forms.Label
            {
                AutoSize = false,
                Size = new Size(Math.Min(160, dateMaxW), 16),
                Location = new Point(textX, 10),
                Text = dateText,
                Font = new Font("Segoe UI", 7.5f),
                ForeColor = Color.FromArgb(130, 130, 130),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
            };
            card.Controls.Add(lblDate);

            // ── Pin + Urgent + Category badges (row 2) ────────────────────
            int badgeY = 30;
            int badgeX = textX;

            if (a.IsPinned)
            {
                var pin = new System.Windows.Forms.Label
                {
                    AutoSize = true,
                    Text = "📌",
                    Font = new Font("Segoe UI", 9f),
                    Location = new Point(badgeX, badgeY),
                    BackColor = Color.Transparent
                };
                card.Controls.Add(pin);
                badgeX += 22;
            }
            if (a.IsUrgent)
            {
                var urg = new System.Windows.Forms.Label
                {
                    AutoSize = false,
                    Size = new Size(62, 18),
                    Location = new Point(badgeX, badgeY),
                    Text = "⚠ URGENT",
                    Font = new Font("Segoe UI", 7f, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.Firebrick,
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                AnnMakeRounded(urg, 9);
                card.Controls.Add(urg);
                badgeX += 68;
            }

            var pillFont = new Font("Segoe UI", 7.5f, FontStyle.Bold);
            // Clamp pill width so it never goes off the right edge
            int pillW = Math.Min(
                TextRenderer.MeasureText(a.Category, pillFont).Width + 14,
                Math.Max(30, cardWidth - badgeX - rightM - 4));
            var catPill = new System.Windows.Forms.Label
            {
                AutoSize = false,
                Size = new Size(pillW, 18),
                Location = new Point(badgeX, badgeY),
                Text = a.Category,
                Font = pillFont,
                ForeColor = iconCol,
                BackColor = iconBg,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoEllipsis = true,
            };
            AnnMakeRounded(catPill, 9);
            card.Controls.Add(catPill);
            badgeX += pillW + 6;

            if (a.Status != "active" && badgeX + 58 < cardWidth - rightM)
            {
                var ib = new System.Windows.Forms.Label
                {
                    AutoSize = false,
                    Size = new Size(56, 18),
                    Location = new Point(badgeX, badgeY),
                    Text = "Inactive",
                    Font = new Font("Segoe UI", 7.5f),
                    ForeColor = Color.FromArgb(100, 100, 100),
                    BackColor = Color.FromArgb(230, 230, 230),
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                AnnMakeRounded(ib, 9);
                card.Controls.Add(ib);
            }

            // ── Title (row 3) ─────────────────────────────────────────────
            int titleW = Math.Max(40, cardWidth - textX - rightM - 4);
            var lblTitle = new System.Windows.Forms.Label
            {
                AutoSize = false,
                Size = new Size(titleW, 22),
                Location = new Point(textX, 52),
                Text = a.Title,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 20, 20),
                BackColor = Color.Transparent,
                AutoEllipsis = true,
            };
            card.Controls.Add(lblTitle);

            // ── Description (row 4) ───────────────────────────────────────
            int descW = Math.Max(40, cardWidth - textX - rightM - 4);
            var lblDesc = new System.Windows.Forms.Label
            {
                AutoSize = false,
                Size = new Size(descW, 20),
                Location = new Point(textX, 75),
                Text = a.Description,
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(90, 90, 90),
                BackColor = Color.Transparent,
                AutoEllipsis = true,
            };
            card.Controls.Add(lblDesc);

            // ── Author + date (row 5) — no view/read rate ─────────────────
            int authorY = 94;
            var lblAuthor = new System.Windows.Forms.Label
            {
                AutoSize = true,
                Location = new Point(textX, authorY),
                Text = "👤 " + (string.IsNullOrWhiteSpace(a.InstructorName) ? "Admin" : a.InstructorName),
                Font = new Font("Segoe UI", 7.5f),
                ForeColor = Color.FromArgb(110, 110, 110),
                BackColor = Color.Transparent,
            };
            card.Controls.Add(lblAuthor);

            // ── Notification + Attachment badges (row 6, only if present) ─
            if (hasNotify || hasAttach)
            {
                int badgeRow = 112;
                int bx = textX;

                if (hasNotify)
                {
                    var targets = new System.Collections.Generic.List<string>();
                    if (a.NotifyStudents) targets.Add("Students");
                    if (a.NotifyInstructors) targets.Add("Instructors");
                    int nbW = Math.Min(
                        TextRenderer.MeasureText("🔔 " + string.Join("+", targets), new Font("Segoe UI", 7f, FontStyle.Bold)).Width + 14,
                        Math.Max(40, cardWidth - bx - rightM - 4));
                    var notifBadge = new System.Windows.Forms.Label
                    {
                        AutoSize = false,
                        Size = new Size(nbW, 17),
                        Location = new Point(bx, badgeRow),
                        Text = "🔔 " + string.Join("+", targets),
                        Font = new Font("Segoe UI", 7f, FontStyle.Bold),
                        ForeColor = Color.FromArgb(30, 100, 180),
                        BackColor = Color.FromArgb(220, 235, 255),
                        TextAlign = ContentAlignment.MiddleCenter,
                        AutoEllipsis = true,
                    };
                    AnnMakeRounded(notifBadge, 8);
                    card.Controls.Add(notifBadge);
                    bx += nbW + 6;
                }

                if (hasAttach && bx + 44 < cardWidth - rightM)
                {
                    bool isPdf = a.AttachedFile!.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase);
                    var attachBadge = new System.Windows.Forms.Label
                    {
                        AutoSize = false,
                        Size = new Size(44, 17),
                        Location = new Point(bx, 112),
                        Text = isPdf ? "📄 PDF" : "🖼 IMG",
                        Font = new Font("Segoe UI", 7f, FontStyle.Bold),
                        ForeColor = Color.FromArgb(100, 60, 160),
                        BackColor = Color.FromArgb(238, 233, 255),
                        TextAlign = ContentAlignment.MiddleCenter,
                    };
                    AnnMakeRounded(attachBadge, 8);
                    card.Controls.Add(attachBadge);
                }
            }

            // ── Click → open detail ───────────────────────────────────────
            EventHandler openDetail = (s, ev) =>
            {
                var ann = announcements.Find(x => x.Id == capturedId);
                if (ann != null) ShowViewAnnouncementUC(ann);
            };
            card.Click += openDetail;
            lblTitle.Click += openDetail;
            lblDesc.Click += openDetail;
            icon.Click += openDetail;

            return card;
        }

        private static GraphicsPath AnnRoundedPath(Rectangle r, int rad)
        {
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, rad, rad, 180, 90);
            p.AddArc(r.Right - rad, r.Y, rad, rad, 270, 90);
            p.AddArc(r.Right - rad, r.Bottom - rad, rad, rad, 0, 90);
            p.AddArc(r.X, r.Bottom - rad, rad, rad, 90, 90);
            p.CloseFigure();
            return p;
        }

        // ── Category sidebar ─────────────────────────────────────────────
        private void BuildAnnCategorySidebar()
        {
            if (flpCategories == null) return;

            flpCategories.SuspendLayout();
            flpCategories.Controls.Clear();
            flpCategories.FlowDirection = FlowDirection.TopDown;
            flpCategories.WrapContents = false;
            flpCategories.AutoScroll = true;
            flpCategories.BackColor = Color.White;   // original white background

            var categories = new[] { "all", "General", "Academic", "Events", "Emergency", "Enrollment" };

            foreach (var cat in categories)
            {
                int count = cat == "all" ? announcements.Count : announcements.Count(a => a.Category == cat);
                bool isActive = _activeCategoryFilter == cat;

                Color dotCol = cat == "all"
                    ? Color.FromArgb(139, 0, 0)   // maroon for "All"
                    : AnnCatIconColor.GetValueOrDefault(cat, Color.Gray);

                int rowW = Math.Max(100, flpCategories.ClientSize.Width - 25);
                var row = new Panel
                {
                    Width = rowW,
                    Height = 34,
                    Margin = new Padding(0, 0, 0, 4),
                    BackColor = isActive ? Color.FromArgb(245, 238, 238) : Color.Transparent,
                    Cursor = Cursors.Hand,
                    Tag = cat
                };

                var dot = new Panel { Size = new Size(9, 9), BackColor = dotCol, Location = new Point(10, 13) };
                AnnMakeCircle(dot);

                var lbl = new System.Windows.Forms.Label
                {
                    AutoSize = false,
                    Size = new Size(rowW - 60, 34),
                    Location = new Point(26, 0),
                    Text = cat == "all" ? "All" : cat,
                    Font = new Font("Segoe UI", 9.5f, isActive ? FontStyle.Bold : FontStyle.Regular),
                    ForeColor = Color.FromArgb(40, 40, 40),    // original dark text
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.Transparent,
                };

                var badge = new System.Windows.Forms.Label
                {
                    AutoSize = false,
                    Size = new Size(28, 18),
                    Location = new Point(rowW - 36, 8),
                    Text = count.ToString(),
                    Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                    ForeColor = isActive ? Color.White : Color.FromArgb(80, 80, 80),
                    BackColor = isActive ? Color.FromArgb(139, 0, 0) : Color.FromArgb(225, 225, 225),
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                AnnMakeRounded(badge, 9);

                row.Controls.Add(dot); row.Controls.Add(lbl); row.Controls.Add(badge);

                string captured = cat;
                EventHandler h = (s, ev) => { _activeCategoryFilter = captured; BuildAnnCategorySidebar(); RenderAnnouncements(); };
                row.Click += h; dot.Click += h; lbl.Click += h; badge.Click += h;

                flpCategories.Controls.Add(row);
            }

            flpCategories.ResumeLayout();
        }

        // ── Pinned sidebar ───────────────────────────────────────────────
        private void RenderAnnPinned()
        {
            if (flpPinned == null) return;

            flpPinned.SuspendLayout();
            flpPinned.Controls.Clear();
            flpPinned.BackColor = Color.White;
            var pinned = announcements.Where(a => a.IsPinned).OrderByDescending(a => a.Date).ToList();

            if (pinned.Count == 0)
            {
                flpPinned.Controls.Add(new System.Windows.Forms.Label
                {
                    Text = "No pinned announcements.",
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Padding = new Padding(4),
                });
                flpPinned.ResumeLayout(); return;
            }

            foreach (var a in pinned)
            {
                Color dotCol = AnnCatIconColor.GetValueOrDefault(a.Category, Color.Gray);
                int rowW = Math.Max(100, flpPinned.ClientSize.Width - 4);
                var row = new Panel
                {
                    Width = rowW,
                    Height = 52,
                    BackColor = Color.White,
                    Margin = new Padding(0, 2, 0, 2),
                    Cursor = Cursors.Hand
                };
                row.Paint += (s, pe) =>
                {
                    using var pen = new Pen(Color.FromArgb(230, 230, 230), 1f);
                    pe.Graphics.DrawRectangle(pen, 0, 0, row.Width - 1, row.Height - 1);
                };

                var dot = new Panel { Size = new Size(8, 8), Location = new Point(8, 22), BackColor = dotCol };
                AnnMakeCircle(dot);

                var rowTitle = new System.Windows.Forms.Label
                {
                    AutoSize = false,
                    Size = new Size(rowW - 28, 20),
                    Location = new Point(22, 8),
                    Text = a.Title,
                    Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(25, 25, 25),
                    AutoEllipsis = true,
                };
                row.Controls.Add(dot); row.Controls.Add(rowTitle);
                row.Controls.Add(new System.Windows.Forms.Label
                {
                    AutoSize = true,
                    Location = new Point(22, 28),
                    Text = a.Date.ToString("MMM d, yyyy"),
                    Font = new Font("Segoe UI", 7.5f),
                    ForeColor = Color.Gray,
                });

                var capturedAnn = a;
                EventHandler h = (s, ev) => { var found = announcements.Find(x => x.Id == capturedAnn.Id); if (found != null) ShowViewAnnouncementUC(found); };
                row.Click += h; rowTitle.Click += h;
                flpPinned.Controls.Add(row);
            }

            flpPinned.ResumeLayout();
        }

        // ── Insights panel ───────────────────────────────────────────────
        private void RenderAnnInsights()
        {
            if (pnlInsights == null) return;

            var old = pnlInsights.Controls.OfType<Control>().Where(c => c.Name.StartsWith("ins_")).ToList();
            foreach (var c in old) pnlInsights.Controls.Remove(c);
            pnlInsights.BackColor = Color.White;   // original white background

            int total = announcements.Count;
            int active = announcements.Count(a => a.Status == "active");
            int pinned = announcements.Count(a => a.IsPinned);
            int urgent = announcements.Count(a => a.IsUrgent);
            int notified = announcements.Count(a => a.NotifyStudents || a.NotifyInstructors);

            void AddRow(string label, string value, int y, Color valCol)
            {
                pnlInsights.Controls.Add(new System.Windows.Forms.Label
                {
                    Name = "ins_l" + y,
                    AutoSize = true,
                    Text = label,
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = Color.FromArgb(80, 80, 80),   // original muted dark label
                    Location = new Point(8, y)
                });
                pnlInsights.Controls.Add(new System.Windows.Forms.Label
                {
                    Name = "ins_v" + y,
                    AutoSize = true,
                    Text = value,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    ForeColor = valCol,
                    Location = new Point(pnlInsights.Width - 50, y - 2)
                });
            }

            AddRow("Total", total.ToString(), 30, Color.FromArgb(50, 50, 50));
            AddRow("Active", active.ToString(), 58, Color.ForestGreen);
            AddRow("Pinned", pinned.ToString(), 86, Color.DarkOrange);
            AddRow("Urgent", urgent.ToString(), 114, Color.Firebrick);
            AddRow("Notified", notified.ToString(), 142, Color.RoyalBlue);
            pnlInsights.Refresh();
        }

        // ── CRUD helpers ─────────────────────────────────────────────────
        private void EditAnnouncement(int id)
        {
            var a = announcements.Find(x => x.Id == id);
            if (a == null) return;
            editingAnnouncementId = id;
            _createAnnouncementUC.LoadForEdit(new AnnouncementDataAdmin
            {
                Title = a.Title,
                Description = a.Description,
                Category = a.Category,
                PostDate = a.Date,
                IsUrgent = a.IsUrgent,
                IsPinned = a.IsPinned,
                NotifyStudents = a.NotifyStudents,
                NotifyInstructors = a.NotifyInstructors,
                AttachmentPath = a.AttachedFile ?? string.Empty,
            });
            ShowCreateAnnouncementUC();
        }

        private void DeleteAnnouncement(int id)
        {
            announcements.RemoveAll(x => x.Id == id);
            RenderAnnouncements();
        }

        private void OnAnnouncementPosted(object sender, AnnouncementDataAdmin data)
        {
            if (editingAnnouncementId != -1)
            {
                var a = announcements.Find(x => x.Id == editingAnnouncementId);
                if (a != null)
                {
                    a.Title = data.Title;
                    a.Description = data.Description;
                    a.Category = data.Category;
                    a.Date = data.PostDate;
                    a.IsUrgent = data.IsUrgent;
                    a.IsPinned = data.IsPinned;
                    a.NotifyStudents = data.NotifyStudents;
                    a.NotifyInstructors = data.NotifyInstructors;
                    if (!string.IsNullOrEmpty(data.AttachmentPath))
                        a.AttachedFile = System.IO.Path.GetFileName(data.AttachmentPath);
                }
            }
            else
            {
                announcements.Insert(0, new AdminAnnouncement
                {
                    Id = DateTime.Now.Millisecond + new Random().Next(1000, 9999),
                    Title = data.Title,
                    Description = data.Description,
                    Category = data.Category,
                    Date = data.PostDate,
                    Status = "active",
                    IsUrgent = data.IsUrgent,
                    IsPinned = data.IsPinned,
                    NotifyStudents = data.NotifyStudents,
                    NotifyInstructors = data.NotifyInstructors,
                    AttachedFile = string.IsNullOrEmpty(data.AttachmentPath)
                                        ? null
                                        : System.IO.Path.GetFileName(data.AttachmentPath),
                    InstructorName = "Admin",
                    TotalStudents = 40,
                });
            }
            editingAnnouncementId = -1;
            RenderAnnouncements();
        }

        private void OnViewEdit(object sender, int id)
        {
            HideViewAnnouncementUC();
            EditAnnouncement(id);
        }

        // ── UC visibility helpers ────────────────────────────────────────
        private void ShowCreateAnnouncementUC()
        {
            CenterAnnControl(_createAnnouncementUC);
            _createAnnouncementUC.BringToFront();
            _createAnnouncementUC.Visible = true;
        }

        private void HideCreateAnnouncementUC()
        {
            _createAnnouncementUC.Visible = false;
            editingAnnouncementId = -1;
        }

        private void ShowViewAnnouncementUC(AdminAnnouncement a)
        {
            _viewAnnouncementUC.LoadAnnouncement(a);
            CenterAnnControl(_viewAnnouncementUC);
            _viewAnnouncementUC.BringToFront();
            _viewAnnouncementUC.Visible = true;
        }

        private void HideViewAnnouncementUC() => _viewAnnouncementUC.Visible = false;

        private void CenterAnnControl(Control child)
        {
            var parent = pnlAnnouncement;
            int maxW = Math.Max(200, parent.ClientSize.Width - 40);
            int maxH = Math.Max(100, parent.ClientSize.Height - 40);
            if (child.Width > maxW || child.Height > maxH)
            {
                float s = Math.Min((float)maxW / child.Width, (float)maxH / child.Height);
                child.Width = (int)(child.Width * s);
                child.Height = (int)(child.Height * s);
            }
            child.Location = new Point(
                Math.Max(0, (parent.ClientSize.Width - child.Width) / 2),
                Math.Max(0, (parent.ClientSize.Height - child.Height) / 4));
        }

        // ── Drawing helpers ───────────────────────────────────────────────
        private static void AnnMakeCircle(Control c)
        {
            var path = new GraphicsPath();
            path.AddEllipse(0, 0, c.Width, c.Height);
            c.Region = new Region(path);
        }

        private static void AnnMakeRounded(Control c, int r)
        {
            var path = new GraphicsPath();
            path.AddArc(0, 0, r, r, 180, 90);
            path.AddArc(c.Width - r, 0, r, r, 270, 90);
            path.AddArc(c.Width - r, c.Height - r, r, r, 0, 90);
            path.AddArc(0, c.Height - r, r, r, 90, 90);
            path.CloseFigure();
            c.Region = new Region(path);
        }
    }

}