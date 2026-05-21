using PUPAcadPortal.Utils;
using PUPAcadPortal.PortalContents.Admin.Enrollment;
using PUPAcadPortal.PortalContents.Admin.LMS;
using PUPAcadPortal.PortalContents.Admin.SubOffering;
using PUPAcadPortal.PHAddress;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalForms
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

        private SubmenuAnim registrarSubmenuAnim;
        private SubmenuAnim subjectOfferingSubmenuAnim;

        public AdminPortal()
        {
            InitializeComponent();
            this.Load += AdminPortal_Load;
            registrarSubmenuAnim = new SubmenuAnim(pnlRegistrarSubmenu, pnlRegistrarSubmenu.Height);
            subjectOfferingSubmenuAnim = new SubmenuAnim(pnlsubofferingSubmenu, pnlsubofferingSubmenu.Height);
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

        }

        public class ContentPanelInfo
        {
            public Panel Panel { get; set; }
            public Action ResetAction { get; set; }
        }


        //--------------------------
        private void btnSO_CurriculumArchive_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            mainContentPanel.ShowView(new CurriculumArchiveContentAdmin());
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
            mainContentPanel.ShowView(new ScheduleContentAdmin());
        }


        //-----------------------------------------------------(Edit Schedule)
        private void btnSO_EditSchedule_Click(object sender, EventArgs e)
        {
            //ShowPanel(pnlEditSchedule);
            //MessageBox.Show("Edit Schedule clicked.");
            changeButtonColor(sender as Button);
            mainContentPanel.ShowView(new EditScheduleContentAdmin());

            cmbYearLevel_EditSchedule_SelectedIndexChanged(null, EventArgs.Empty);
        }


        private void btnDashboard_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            mainContentPanel.Visible = true;
            mainContentPanel.BringToFront();
            mainContentPanel.ShowView(new DashboardContentAdmin());
        }

        private async void btnSubjectOffering_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            btnSubjectOffering.Text = !pnlsubofferingSubmenu.Visible ? " Subject Offering                    ⌄" : " Subject Offering                     ›";
            btnSO_CurrentSemester.PerformClick(); // Show Current Semester by default when Subject Offering is clicked
            await subjectOfferingSubmenuAnim.ToggleSubMenuAsync();
            //pnlRegistrarSubmenu.Visible = false;
            //pnlsubofferingSubmenu.Visible = !pnlsubofferingSubmenu.Visible;
            //if (pnlsubofferingSubmenu.Visible)
            //    btnSubjectOffering.Text = " Subject Offering                    ⌄";
            //else
            //    btnSubjectOffering.Text = " Subject Offering                     ›";
        }

        //-------------------------------------------------------------- (1 current sem page)
        private void btnSO_CurrentSemester_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            mainContentPanel.ShowView(new CurrentSemesterContentAdmin());

            // Show Current Semester panel
            //pnlSubOfferingContent.Visible = true;
            //pnlSubOfferingContent.Visible = true;
            //pnlCurrentSemester.Visible = true;

            // Hide other content panels
            //pnlEditSchedule.Visible = false;
            //pnlOtherPanel.Visible = false;

        }

        private void btnAnnouncement_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            AnnounceContentAdmin adminAnnounceContent = new AnnounceContentAdmin();
            mainContentPanel.ShowView(adminAnnounceContent);
            adminAnnounceContent.InitAnnouncementPanelIfNeeded();
        }

        private async void btnRegistrarFunctions_Click(object sender, EventArgs e)
        {
            // Change button color and show the main content panel (if any)
            changeButtonColor(sender as Button);
            btnGradesManagement.PerformClick(); // Show Grades Management by default when Registrar Functions is clicked
            btnRegistrarFunctions.Text = !pnlRegistrarSubmenu.Visible ? " Registrar Functions              ⌄" : " Registrar Functions              ›";
            await registrarSubmenuAnim.ToggleSubMenuAsync();
        }

        // Submenu button event handlers

        private void btnGradesManagement_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new GradesMngContentAdmin());
        }

        private void btnAccountingRecords_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new AccountsContentAdmin());
        }

        private void btnEnrolledStudents_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new EnrolledStudentsContentAdmin());
        }

        // Other sidebar buttons

        private void bgtnRegisterStudents_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            mainContentPanel.ShowView(new RegisterStudentsContentAdmin());
        }

        private void btnRegisterProf_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            mainContentPanel.ShowView(new RegisterProfContentAdmin());
        }

        private void btnViewAllUsers_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            mainContentPanel.ShowView(new ViewUsersContentAdmin());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        //-------------------------------------------------------------- (events current sem page)
        private void pnlCoursesContent_Paint(object sender, PaintEventArgs e)
        {

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

        private void AdminPortal_Load(object sender, EventArgs e)
        {


            try
            {
                // Loads addresses for dropdowns in Register Student/Professor forms
                AddToAddressCMB.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize geographic address databases: {ex.Message}",
                                "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            btnDashboard.PerformClick();
        }
    }
}