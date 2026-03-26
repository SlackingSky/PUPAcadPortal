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
            // 1. Set the format to Custom
            dateTimePicker1.Format = DateTimePickerFormat.Custom;

            // 2. Set the pattern to include both Date and Time
            // This will show: 03/24/2026 09:30 PM
            dateTimePicker1.CustomFormat = "MM/dd/yyyy hh:mm tt";
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
            pnlLMSActivities.Visible = true;
            pnlLMSActivities.BringToFront();
            
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

        private void cmbBXActType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected text safely
            string selectedAction = cmbBXActType.SelectedItem?.ToString();

            // First, hide all panels to "reset" the view
            pnlQuiz1.Visible = false;
            pnlAssign.Visible = false;

            // Now, show only the one that matches the selection
            switch (selectedAction)
            {
                case "Quiz":
                    pnlQuiz1.Visible = true;
                    pnlQuiz1.BringToFront();
                    break;

                case "Assignment":
                    pnlAssign.Visible = true;
                    pnlAssign.BringToFront();
                    break;
            }
        }

        private void btnAssignAttach_Click(object sender, EventArgs e)
        {
            pnlAttachAss.Visible = true;
            pnlAttachAss.BringToFront();
        }

        private void btnAttachCancel_Click(object sender, EventArgs e)
        {
            pnlAttachAss.Visible = false;

        }

        private void btnAttachDone_Click(object sender, EventArgs e)
        {
            pnlAttachAss.Visible = false;
        }

        private void btnAttachCancel_Click_1(object sender, EventArgs e)
        {
            pnlAttachAss.Visible = false;
        }

        private void btnDoneAttach_Click(object sender, EventArgs e)
        {
            pnlAttachAss.Visible = false;
        }

        // --- BUTTON 1: ADD QUESTION ---
        private void btnAddPanel_Click(object sender, EventArgs e)
        {
            ucQuestionCard newCard = new ucQuestionCard();
            // Match your new size
            newCard.Width = 1250;
            newCard.Height = 423;

            flowLayoutPanel3.Controls.Add(newCard);

            // --- THE ALIGNMENT FIX ---
            // Calculate the margin based on your 1363px panel width
            int centeredMargin = (flowLayoutPanel3.Width - 1250 - 25) / 2;
            if (centeredMargin < 0) centeredMargin = 57;

            foreach (Control ctrl in flowLayoutPanel3.Controls)
            {
                // Force EVERY control to be exactly 1250 wide
                ctrl.Width = 1250;

                // Force EVERY control to have the exact same left margin
                ctrl.Margin = new Padding(centeredMargin, 10, 10, 10);

                // Remove any internal offsets
                ctrl.Left = 0;
            }

            RenumberQuestions();

            if (flowLayoutPanel3.Controls.Contains(pnlControlBar))
            {
                flowLayoutPanel3.Controls.SetChildIndex(pnlControlBar, -1);
            }

            flowLayoutPanel3.ScrollControlIntoView(pnlControlBar);
        }

        // --- BUTTON 2: REMOVE LAST QUESTION ---
        private void btnRemove_Click(object sender, EventArgs e)
        {
            var lastCard = flowLayoutPanel3.Controls.OfType<ucQuestionCard>().LastOrDefault();

            if (lastCard != null)
            {
                flowLayoutPanel3.Controls.Remove(lastCard);
                lastCard.Dispose();

                // NEW: Renumber the remaining cards so there are no gaps
                RenumberQuestions();
            }
        }

        // --- BUTTON 3: SAVE & EXIT ---
        private void btnSaveQuiz_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to save this quiz before exiting?",
                                                "Save Quiz", MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                // Add your Save Logic here later
                this.Close();
            }
            else if (result == DialogResult.No)
            {
                this.Close();
            }
        }

        private void flowLayoutPanel3_Resize(object sender, EventArgs e)
        {
            // Calculate new centering margin based on current panel width
            // Formula: (Current Width - Card Width - Scrollbar Width) / 2
            int newMargin = (flowLayoutPanel3.Width - 1250 - 25) / 2;

            // If the window is small, don't let the margin go negative
            if (newMargin < 0) newMargin = 10;

            foreach (Control ctrl in flowLayoutPanel3.Controls)
            {
                // Apply the new margin to every card and the control bar
                ctrl.Margin = new Padding(newMargin, 10, 10, 10);
            }
        }

        private void RenumberQuestions()
        {
            int count = 1;
            // Loop through only the UserControls (ignoring the control bar)
            foreach (Control ctrl in flowLayoutPanel3.Controls)
            {
                if (ctrl is ucQuestionCard card)
                {
                    card.lblQuestionNumber.Text = "Question " + count;
                    count++;
                }
            }
        }
    }
}
