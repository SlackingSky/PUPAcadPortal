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

        private void button2_Click(object sender, EventArgs e)
        {
            pnlTW.BringToFront();
            pnlTW.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pnlToday.BringToFront();
            pnlToday.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pnlNW.BringToFront();
            pnlNW.Visible = true;
        }

        private void buttonRounded2_Click(object sender, EventArgs e)
        {
            pnlRA.BringToFront();
            pnlRA.Visible = true;
        }

        private void buttonRounded6_Click(object sender, EventArgs e)
        {
            pnlAll.BringToFront();
            pnlAll.Visible = true;
            pnlVA.Visible = false;
        }

        private void buttonRounded7_Click(object sender, EventArgs e)
        {
            pnlAll.BringToFront();
            pnlAll.Visible = true;
            pnlVA.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pnlAll.BringToFront();
            pnlAll.Visible = true;
        }

        private void buttonRounded1_Click(object sender, EventArgs e)
        {
            pnlVA.BringToFront();
            pnlVA.Visible = true;
        }

        private void buttonRounded9_Click(object sender, EventArgs e)
        {
            pnlVA.BringToFront();
            pnlVA.Visible = true;
        }

        private void buttonRounded10_Click(object sender, EventArgs e)
        {
            pnlRA.BringToFront();
            pnlRA.Visible = true;
        }

        private void buttonRounded15_Click(object sender, EventArgs e)
        {
            pnlAll.BringToFront();
            pnlAll.Visible = true;
            pnlRA.Visible = false;
        }

        private void label44_Click(object sender, EventArgs e)
        {

        }
    }
}
