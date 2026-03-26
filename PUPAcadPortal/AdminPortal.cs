using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        private Dictionary<Button, Panel> contentPanels; // Class-level dictionary to hold button-panel mappings
        public AdminPortal()
        {
            InitializeComponent();
            this.Resize += AdminPortal_Resize;

            // Initialize the dictionary with main sidebar buttons
            contentPanels = new Dictionary<Button, Panel>
            {
            { btnDashboard, pnlDashboardContent },
            { btnSubjectOffering, pnlSubOfferingContent },
            { btnGradesManagement, pnlGradesManagementContent },
            { btnAccountingRecords, pnlAccountingRecordsContent },
            { btnEnrolledStudents, pnlEnrolledStudentsContent },
            { btnRegisterStudent, pnlRegisterStudentContent },
            { btnRegisterProfessor, pnlRegisterProfessorContent },
            { btnViewAllUsers, pnlViewAllUsersContent }
                // LMS is not included here because it toggles a submenu
            };
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

            if (pnlViewAllUsersContent.Visible && selectedUserTypeButton != null)
            {
                MoveIndicatorUnder(selectedUserTypeButton);
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

        private void lblViewDesc_Click(object sender, EventArgs e)
        {

        }

        private void btnViewStudents_Click(object sender, EventArgs e)
        {
            if (selectedUserTypeButton == btnViewStudents) return; // already selected
            SelectUserTypeButton(btnViewStudents);
            RefreshUsersList(); // load student data
        }

        private void btnViewProf_Click(object sender, EventArgs e)
        {
            if (selectedUserTypeButton == btnViewProf) return;
            SelectUserTypeButton(btnViewProf);
            RefreshUsersList(); // load professor data
        }

        private void SelectUserTypeButton(Button selected)
        {
            // Reset previous button
            if (selectedUserTypeButton != null)
            {
                selectedUserTypeButton.BackColor = defaultUserButtonColor;
            }

            // Set new selected button
            selectedUserTypeButton = selected;
            selected.BackColor = selectedUserButtonColor;

            // Move the indicator line below the selected button
            MoveIndicatorUnder(selected);
        }

        private void MoveIndicatorUnder(Button button)
        {
            if (pnlUserTypeIndicator == null) return;
            // Align indicator to the left edge of the button
            pnlUserTypeIndicator.Location = new Point(button.Left, button.Bottom + 2);
            // Make indicator width match the button
            pnlUserTypeIndicator.Width = button.Width;
        }

        private void SetDefaultUserTypeButton()
        {
            SelectUserTypeButton(btnViewStudents); // This will also trigger the indicator movement
        }

        private void RefreshUsersList()
        {
            string typeFilter = (selectedUserTypeButton == btnViewStudents) ? "Student" : "Professor";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefreshUsersList();
        }

        private void txtSearchViewAUs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
                e.SuppressKeyPress = true;
            }
        }
    }
}
