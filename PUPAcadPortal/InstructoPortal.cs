using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

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
            //aalisin ung border ng datetimepicker sa attendance tab | d ko alam kung sakin lng kasi nagbubug / nawawala ung button for the date :) - hansukal
            // Removes 2 pixels from every side of the control
            //dateTimePicker3.Region = new Region(new Rectangle(2, 2, dateTimePicker1.Width - 4, dateTimePicker1.Height - 4));
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
            pnlLMSFiles.Visible = false;
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

        private void pnlLMSFiles_DragDrop(object sender, DragEventArgs e)
        {
            // Get the array of file paths
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string filePath in files)
            {
                // Just call your helper method! It already has the size check and icon logic.
                AddFileToListView(filePath);
            }
        }

        private void AddFileToListView(string filepath)
        {
            FileInfo fileInfo = new FileInfo(filepath);
            long maxFileSize = 10 * 1024 * 1024;

            if (fileInfo.Length <= maxFileSize)
            {
                Icon fileIcon = Icon.ExtractAssociatedIcon(filepath);

                // We use the extension as a key (e.g., ".pdf")
                if (!imageList1.Images.ContainsKey(fileInfo.Extension))
                {
                    // The imageList will automatically scale the icon to 32x32 
                    // because we set the ImageSize in the Load event.
                    imageList1.Images.Add(fileInfo.Extension, fileIcon);
                }

                int iconIndex = imageList1.Images.IndexOfKey(fileInfo.Extension);

                // Ensure the index is valid
                ListViewItem item = new ListViewItem(fileInfo.Name, iconIndex);
                item.Tag = filepath;
                item.SubItems.Add($"{fileInfo.Length / 1024.0:F2} KB");

                listView_file.Items.Add(item);
            }
        }

        private void pnlLMSFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void InstructorPortal_Load(object sender, EventArgs e)
        {
            // 1. Font and Appearance
            listView_file.Font = new Font("Segoe UI", 11.5f); // Slightly larger for better readability
            listView_file.View = View.Details;
            listView_file.FullRowSelect = true;
            listView_file.GridLines = false;

            // 2. Fix the Icon Visibility & Spacing
            // Width and Height must both be large enough to see the icon
            imageList1.ImageSize = new Size(32, 32);
            imageList1.ColorDepth = ColorDepth.Depth32Bit; // Makes icons crisp
            listView_file.SmallImageList = imageList1;

            // 3. Columns
            listView_file.Columns.Clear();
            listView_file.Columns.Add("File Name", 350);
            listView_file.Columns.Add("Size", 120);
        }

        private void listView_file_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // 1. Check if an item was actually clicked
            if (listView_file.SelectedItems.Count > 0)
            {
                // 2. Get the full path we stored in the Tag earlier
                string fullPath = listView_file.SelectedItems[0].Tag.ToString();

                try
                {
                    // 3. Tell Windows to open the file
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = fullPath,
                        UseShellExecute = true // This is required for modern .NET to open files
                    };
                    Process.Start(psi);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not open file: " + ex.Message);
                }
            }
        }

        private void removeFromTheListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if an item is selected
            if (listView_file.SelectedItems.Count > 0)
            {
                // Ask for confirmation (Optional but professional)
                DialogResult result = MessageBox.Show("Remove this file from the list?",
                    "Confirm Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    foreach (ListViewItem item in listView_file.SelectedItems)
                    {
                        listView_file.Items.Remove(item);
                    }
                }
            }
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView_file.SelectedItems.Count > 0)
            {
                // 1. Get the original path we stored in the Tag
                string sourcePath = listView_file.SelectedItems[0].Tag.ToString();
                string fileName = listView_file.SelectedItems[0].Text;

                // 2. Open a Save File Dialog
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = fileName;
                saveFileDialog.Filter = "All files (*.*)|*.*";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // 3. Copy the file from the original path to the new path
                        File.Copy(sourcePath, saveFileDialog.FileName, true);
                        MessageBox.Show("File saved successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving file: " + ex.Message);
                    }
                }
            }
        }

        private void listView_file_MouseClick(object sender, MouseEventArgs e)
        {
            // This checks if the user clicked the Right Mouse Button
            if (e.Button == MouseButtons.Right)
            {
                // This finds exactly which file was under the mouse cursor
                var item = listView_file.GetItemAt(e.X, e.Y);

                if (item != null)
                {
                    // Select the item so the menu knows which file to "Remove" or "Download"
                    item.Selected = true;

                    // Show your ContextMenuStrip at the exact mouse location
                    contextMenuStrip1.Show(listView_file, e.Location);
                }
            }
        }
    }
}
