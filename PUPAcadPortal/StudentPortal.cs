using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class StudentPortal : Form
    {
        private Button clickedButton;
        private Color defaultColor = Color.Maroon;
        private Color selectedColor = Color.FromArgb(109, 0, 0);
        public StudentPortal()
        {
            InitializeComponent();
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
            contents.Add(btnEnrollment, pnlEnrollContent);
            contents.Add(btnCourses, pnlCoursesContent);
            contents.Add(btnAccounts, pnlAccountsContent);
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

        private void btnEnrollment_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnCourses_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnAccounts_Click(object sender, EventArgs e)
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

        private void btnAnnounce_Click(object sender, EventArgs e)
        {
            pnlAnnounce.BringToFront();
            pnlAnnounce.Visible = true;
        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            pnlSubject.Visible = false;
            pnlCalendar.BringToFront();
            pnlCalendar.Visible = true;
        }

        private void btnSubject_Click(object sender, EventArgs e)
        {
            pnlSubject.BringToFront();
            pnlSubject.Visible = true;
        }

        private void btnActivities_Click(object sender, EventArgs e)
        {
            pnlActivities.BringToFront();
            pnlActivities.Visible = true;
        }

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            pnlAttendance.BringToFront();
            pnlAttendance.Visible = true;
        }

        private void btnGrade_Click(object sender, EventArgs e)
        {
            pnlGrades.BringToFront();
            pnlGrades.Visible = true;
        }

        private void btnGo1_Click(object sender, EventArgs e)
        {
            pnlLMSFiles.Visible = false;
            pnlSubMenu.BringToFront();
            pnlSubMenu.Visible = true;
            pnlLMSActivities.BringToFront();
            pnlLMSActivities.Visible = true;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            pnlSubject.BringToFront();
            pnlSubject.Visible = true;
        }

        private void btnStudFiles_Click(object sender, EventArgs e)
        {

            pnlLMSFiles.BringToFront();
            pnlLMSFiles.Visible = true;
        }

        private void btnStudAct_Click(object sender, EventArgs e)
        {
            pnlLMSFiles.Visible = false;
            pnlLMSActivities.BringToFront();
            pnlLMSActivities.Visible = true;
        }

        private void pnlAct1_MouseEnter(object sender, EventArgs e)
        {
            // Change to a light highlight color (e.g., Light Gray or Light Maroon)
            pnlAct1.BackColor = Color.FromArgb(128, 0, 0);
            pnlAct1.Cursor = Cursors.Hand; // Shows the clicking hand icon
        }

        private void pnlAct1_MouseLeave(object sender, EventArgs e)
        {
            // Change back to the original background color (usually White)
            pnlAct1.BackColor = Color.White;
        }

        private void pnlAct1_Click(object sender, EventArgs e)
        {
            // 1. Hide the panel that contains the question/choices
            // Replace 'pnlQuestionArea' with the actual parent container name
            pnlAnsAct1.Visible = false;

            // 2. Show the Answer Action panel
            pnlAnsAct1.Visible = true;
            pnlAnsAct1.BringToFront();
            pnlAnsAct1.Dock = DockStyle.Fill; // Optional: ensures it fills the space
        }

        private void btnCancelAct_Click(object sender, EventArgs e)
        {
            pnlLMSActivities.BringToFront();
            pnlLMSActivities.Visible = true;
        }

        private void btnCancelAct_Click_1(object sender, EventArgs e)
        {
            pnlLMSActivities.BringToFront();
            pnlLMSActivities.Visible = true;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            pnlLMSActivities.BringToFront();
            pnlLMSActivities.Visible = true;
        }

        private void btnAssignAttach_Click(object sender, EventArgs e)
        {
            pnlAttachAss.BringToFront();
            pnlAttachAss.Visible = true;
        }

        private void btnCancelAssign_Click(object sender, EventArgs e)
        {
            pnlLMSActivities.BringToFront(); pnlLMSActivities.Visible = true;
        }

        private void btnSaveAss_Click(object sender, EventArgs e)
        {
            pnlLMSActivities.BringToFront(); pnlLMSActivities.Visible = true;
        }

        private void btnAttachCancel_Click(object sender, EventArgs e)
        {
            pnlAttachAss.Hide();
        }

        private void btnDoneAttach_Click(object sender, EventArgs e)
        {
            pnlAttachAss.Hide();
        }

        private void pnlAss1_MouseEnter(object sender, EventArgs e)
        {
            // Highlights to Maroon on hover
            pnlAss1.BackColor = Color.Maroon;
            pnlAss1.Cursor = Cursors.Hand;
        }

        private void pnlAss1_MouseLeave(object sender, EventArgs e)
        {
            // Changes back to White when the mouse leaves
            pnlAss1.BackColor = Color.White;
        }

        private void pnlAss1_Click(object sender, EventArgs e)
        {
            // 1. Immediately reset the color to White
            pnlAss1.BackColor = Color.White;

            // 2. Switch to the Answer panel
            pnlAnsAss.Visible = true;
            pnlAnsAss.BringToFront();

            // Optional: If pnlAss1 is inside a container that you hide:
            // pnlMainContainer.Visible = false;
        }

        private void roundedPanel14_MouseLeave(object sender, EventArgs e)// ung pangatlong panel sa activities ng courses
        {
            roundedPanel14.BackColor = Color.White;
        }

        private void roundedPanel14_MouseEnter(object sender, EventArgs e)// ung pangatlong panel sa activities ng courses
        {
            roundedPanel14.BackColor = Color.Maroon;
            roundedPanel14.Cursor = Cursors.Hand;
        }

        private void roundedPanel16_MouseEnter(object sender, EventArgs e)
        {
            roundedPanel16.BackColor = Color.Maroon;
            roundedPanel16.Cursor = Cursors.Hand;
        }

        private void roundedPanel16_MouseLeave(object sender, EventArgs e)
        {
            roundedPanel16.BackColor = Color.White;
        }
    }
}
