using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class InstructorPortal : Form
    {
        private Button clickedButton;
        private Color defaultColor = Color.Maroon;
        private Color selectedColor = Color.FromArgb(109, 0, 0);
        public InstructorPortal()
        {
            InitializeComponent();
            pnlAttendance.AutoScrollMinSize = new Size(0, 1000);
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
        private void showContent(Button button)
        {
            Dictionary<Button, Panel> contents = new Dictionary<Button, Panel> { };
            contents.Add(btnDashboard, pnlDashboardContent);
            contents.Add(btnGrades, pnlGradesContent);
            contents.Add(btnCourses, pnlCoursesContent);
            //Kada button na aadd, maglagay ng panel sa form at lagay dito
            foreach (KeyValuePair<Button, Panel> content in contents)
            {
                if (content.Key == clickedButton)
                {
                    //Automatic positioning, wag pakialaman maliban nalang kung binago ang position ng sidebar
                    content.Value.Location = new Point(pnlSidebar.Size.Width, pnlHeader.Size.Height);
                    content.Value.Visible = true;
                }
                else
                {
                    content.Value.Visible = false;
                }
            }
        }

        //Method para pag pinindot yung X sa taas o mag alt-F4, icclose lahat ng forms para di magerror pag ni run uli
        //Lagay to sa bawat form na iaadd, Step 1: Hanapin sa properties ng form yung event na FormClosing, Step 2: Double click para gumawa ng method, Step 3: Copy paste code na nasa loob nito
        private void StudentPortal_Closing(object sender, FormClosingEventArgs e)
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

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnGrades_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnCourses_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
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
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pnlCoursesContent_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnAnnounceIns_Click(object sender, EventArgs e)
        {
            pnlAnnounce.BringToFront();
            pnlAnnounce.Visible = true;
        }

        private void btnCalendarIns_Click(object sender, EventArgs e)
        {
            pnlCalendar.BringToFront();
            pnlCalendar.Visible = true;
        }

        private void btnSubjectIns_Click(object sender, EventArgs e)
        {
            pnlSubject.BringToFront();
            pnlSubject.Visible = true;
        }

        private void btnActivitiesIns_Click(object sender, EventArgs e)
        {
            pnlLMSAct.BringToFront();
            pnlLMSAct.Visible = true;
        }

        private void btnAttendanceIns_Click(object sender, EventArgs e)
        {
            pnlAttendance.BringToFront();
            pnlAttendance.Visible = true;
        }

        private void btnGradeIns_Click(object sender, EventArgs e)
        {
            pnlGrades.BringToFront();
            pnlGrades.Visible = true;
        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        bool expand = false;
        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void StatusBtn_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void CreateAnnounce_Click(object sender, EventArgs e)
        {
            pnlCreateAnnounce.Visible = !pnlCreateAnnounce.Visible;

            if (pnlCreateAnnounce.Visible)
            {
                // Ensure it sits on top of all other controls/panels
                pnlCreateAnnounce.BringToFront();


                pnlCreateAnnounce.Location = new Point(
                (this.Width - pnlCreateAnnounce.Width) / 4,
                (this.Height - pnlCreateAnnounce.Height) / 4);

            }
        }

        private void StatusBtn2_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }


        private void Sub1_Paint(object sender, PaintEventArgs e)
        {

        }
        bool sidebarExpand;

        private void MenuButton_Click(object sender, EventArgs e)
        {
            sideBarTimer.Start();
        }

        private void btnAssign_Click(object sender, EventArgs e)
        {
            // 1. Show the target panel
            pnlLMSAct.Visible = true;

            // 2. Bring it to the front so it's not hidden behind other main panels
            pnlLMSAct.BringToFront();


        }
        private void btnClassFiles_Click(object sender, EventArgs e)
        {
            pnlClassFiles.Visible = true;

            // 2. Bring it to the front so it's not hidden behind other main panels
            pnlClassFiles.BringToFront();
        }



        private void pnlAttendance_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CreateAnnounce_Click_1(object sender, EventArgs e)
        {
            pnlCreateAnnounce.Visible = true;
            pnlCreateAnnounce.BringToFront();
        }

        private void btnCancelPost_Click(object sender, EventArgs e)
        {
            pnlCreateAnnounce.Visible = false;
            pnlAnnounce.BringToFront();
        }

        private void btnGo1_Click(object sender, EventArgs e)
        {
            pnlSubMenu.Visible = true;
            pnlSubMenu.BringToFront();
            pnlGenChats.Visible = true;
            pnlGenChats.BringToFront();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            pnlSubject.Visible = true;
            pnlSubject.BringToFront();
        }

        private void btnGeneralAnnounce_Click(object sender, EventArgs e)
        {
            pnlGenChats.Visible = true;
            pnlGenChats.BringToFront();
        }

        private void btnLMSActSub_Click(object sender, EventArgs e)
        {
            pnlLMSActivities.Visible = true;
            pnlLMSActivities.BringToFront();
        }

        private void btnLMSFiles_Click(object sender, EventArgs e)
        {
            pnlLMSFiles.Visible = true;
            pnlLMSFiles.BringToFront();
        }

        private void btnCreateAct_Click(object sender, EventArgs e)
        {
            pnlCreateAct.Visible = true;
            pnlCreateAct.BringToFront();
        }
    }
}
