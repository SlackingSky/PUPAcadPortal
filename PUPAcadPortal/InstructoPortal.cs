using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using static PUPAcadPortal.InstructorPortal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;


namespace PUPAcadPortal

{
    public partial class InstructorPortal : Form
    {
        private bool isEditing = false; // New flag to stop event interference
        private ActivityItem? currentEditingItem = null;
        private Button clickedButton;
        private Color defaultColor = Color.Maroon;
        private Color selectedColor = Color.FromArgb(109, 0, 0);

        // ✅ ANNOUNCEMENT SYSTEM VARIABLES (GLOBAL)
        List<Announcement> announcements = new List<Announcement>();
        int editingAnnouncementId = -1;

        private string tempAttachedPath = "";
        public InstructorPortal()
        {
            InitializeComponent();
            pnlAttendance.AutoScrollMinSize = new Size(0, 1000);
            this.DoubleBuffered = true;
            // DateTimePicker Formatting
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/dd/yyyy hh:mm tt";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "ddd, MMM dd, yyyy";

            // ✅ FIX: Wire the resize event ONLY ONCE here.
            // If you put this inside a Click event, it stacks and causes the "creeping" movement.

        }



        private void changeButtonColor(Button button)
        {
            if (button == null) return; // Add a "Guard Clause" to stop if null

            if (clickedButton != null)
            {
                clickedButton.BackColor = defaultColor;
            }
            clickedButton = button;
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
            pnlCreateAnnounce1.Visible = false;
            pnlAnnouncement.BringToFront();
            pnlAnnouncement.Visible = true;
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
            pnlLMSFiles.Visible = true;

            // 2. Bring it to the front so it's not hidden behind other main panels
            pnlLMSFiles.BringToFront();
        }



        private void pnlAttendance_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CreateAnnounce_Click_1(object sender, EventArgs e)
        {
            pnlCreateAnnounce1.Visible = true;
            pnlCreateAnnounce1.BringToFront();
            pnlCreateAnnounce1.Anchor = AnchorStyles.None;
            CenterCreateAnnouncementPanel();
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
            pnlPostedAct.Visible = true;
            pnlPostedAct.BringToFront();

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
            currentEditingItem = null; // Ensure we aren't in edit mode
            ClearAllInputs(); // This will reset everything

            pnlCreateAct.Visible = true;
            pnlCreateAct.BringToFront();

            // Force the ComboBox to show nothing
            cmbBXActType.SelectedIndex = -1;
            cmbBXActType.Text = "";

            // Hide both panels until the user picks one
            pnlQuiz1.Visible = false;
            pnlAssign.Visible = false;
        }

        private void cmbBXActType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isEditing) return; // Don't run this if we are loading an edit

            if (cmbBXActType.Text == "Quiz")
            {
                pnlQuiz1.Visible = true;
                pnlQuiz1.BringToFront();
                pnlAssign.Visible = false;
            }
            else if (cmbBXActType.Text == "Assignment")
            {
                pnlAssign.Visible = true;
                pnlAssign.BringToFront();
                pnlQuiz1.Visible = false;
            }
            else
            {
                // If it's cleared or "None"
                pnlQuiz1.Visible = false;
                pnlAssign.Visible = false;
            }
        }

        private void btnAssignAttach_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "All Files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    tempAttachedPath = ofd.FileName;

                    // This updates the label you just created in Step 3
                    lblFileNameDisplay.Text = Path.GetFileName(ofd.FileName);
                }
            }
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
            quizCreation newCard = new quizCreation();
            newCard.Width = 1250;
            newCard.Height = 456;

            flowLayoutPanel3.Controls.Add(newCard);

            // Apply the modern flat design centering
            int centeredMargin = (flowLayoutPanel3.Width - 1250 - 25) / 2;
            if (centeredMargin < 0) centeredMargin = 10;

            foreach (Control ctrl in flowLayoutPanel3.Controls)
            {
                ctrl.Width = 1250;
                ctrl.Margin = new Padding(centeredMargin, 10, 10, 10);
            }

            RenumberQuestions();

            // Ensure control bar is at the end
            if (flowLayoutPanel3.Controls.Contains(pnlControlBar))
            {
                flowLayoutPanel3.Controls.SetChildIndex(pnlControlBar, -1);
            }

            flowLayoutPanel3.PerformLayout();

            // ✅ THE FIX: Scroll to the TOP of the new card specifically
            this.BeginInvoke((MethodInvoker)delegate {
                // By setting the AutoScrollPosition to the Top of the newCard,
                // you frame the 456px card inside the 591px panel.
                // The buttons will naturally appear in the remaining 135px of space.
                flowLayoutPanel3.AutoScrollPosition = new Point(0, newCard.Top - flowLayoutPanel3.AutoScrollPosition.Y);
            });
        }

        // --- BUTTON 2: REMOVE LAST QUESTION ---
        private void btnRemove_Click(object sender, EventArgs e)
        {
            var lastCard = flowLayoutPanel3.Controls.OfType<quizCreation>().LastOrDefault();

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

            if (string.IsNullOrWhiteSpace(txtActTitle.Text))
            {
                MessageBox.Show("Please enter an Activity Title.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtActTitle.Focus();
                return;
            }

            var activeQuizCard = flowLayoutPanel3.Controls.OfType<quizCreation>().FirstOrDefault();

            if (activeQuizCard == null)
            {
                MessageBox.Show("Please add a question card first!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Added validation for the quiz content itself
            if (string.IsNullOrWhiteSpace(activeQuizCard.Ques.Text) || string.IsNullOrWhiteSpace(activeQuizCard.cmbCorrectAnswer.Text))
            {
                MessageBox.Show("Please ensure the question and the correct answer are provided.", "Incomplete Quiz", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Save Quiz Activity?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ActivityItem targetItem;

                if (currentEditingItem == null)
                {
                    // CREATE NEW
                    targetItem = new ActivityItem();
                    targetItem.Width = ManageAct.ClientSize.Width - 35;
                    targetItem.Height = 187;

                    targetItem.btnEdit.Click += (s, ev) => LoadItemForEditing(targetItem);
                    targetItem.btnRemove.Click += (s, ev) => {
                        if (MessageBox.Show("Delete this activity?", "Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                            ManageAct.Controls.Remove(targetItem);
                    };

                    ManageAct.Controls.Add(targetItem);
                    ManageAct.Controls.SetChildIndex(targetItem, 0);
                }
                else
                {
                    // EDIT EXISTING
                    targetItem = currentEditingItem;
                }

                // --- TAG AND ICON ---
                targetItem.Tag = "QUIZ";
                targetItem.actPic.Image = Properties.Resources.quiz;

                // --- VISUAL UPDATES ---
                targetItem.lblTitle.Text = txtActTitle.Text;
                targetItem.lblDueDate.Text = "Due : " + dateTimePicker1.Value.ToString("MM/dd/yyyy hh:mm tt");

                // Update the label INSIDE the quizCreation control for feedback
                activeQuizCard.lblCorrectAns.Text = "Correct: " + activeQuizCard.cmbCorrectAnswer.Text;

                // --- SAVE DATA TO CARD ---
                targetItem.SavedTitle = txtActTitle.Text;
                targetItem.SavedQuestion = activeQuizCard.Ques.Text;
                targetItem.SavedChoices[0] = activeQuizCard.textBox1.Text;
                targetItem.SavedChoices[1] = activeQuizCard.textBox2.Text;
                targetItem.SavedChoices[2] = activeQuizCard.textBox3.Text;
                targetItem.SavedChoices[3] = activeQuizCard.textBox4.Text;

                // Store the correct answer in the ActivityItem
                targetItem.SavedCorrectAnswer = activeQuizCard.cmbCorrectAnswer.Text;

                ClearAllInputs();
                currentEditingItem = null;
                pnlCreateAct.Visible = false;

                MessageBox.Show("Quiz saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LoadItemForEditing(ActivityItem item)
        {
            currentEditingItem = item;
            isEditing = true;

            cmbBXActType.SelectedIndexChanged -= cmbBXActType_SelectedIndexChanged;

            pnlCreateAct.Visible = true;
            pnlCreateAct.BringToFront();
            txtActTitle.Text = item.SavedTitle;

            if (item.Tag?.ToString() == "ASSIGNMENT")
            {
                cmbBXActType.Text = "Assignment";
                pnlAssign.Visible = true;
                pnlAssign.BringToFront();
                pnlQuiz1.Visible = false;

                textBox22.Text = item.SavedInstructions;
                lblFileNameDisplay.Text = string.IsNullOrEmpty(item.SavedAttachedFilePath)
                    ? "No file attached"
                    : System.IO.Path.GetFileName(item.SavedAttachedFilePath);
            }
            else // Logic for QUIZ
            {
                cmbBXActType.Text = "Quiz";
                pnlQuiz1.Visible = true;
                pnlQuiz1.BringToFront();
                pnlAssign.Visible = false;

                var activeQuizCard = flowLayoutPanel3.Controls.OfType<quizCreation>().FirstOrDefault();
                if (activeQuizCard != null)
                {
                    activeQuizCard.Ques.Text = item.SavedQuestion;
                    activeQuizCard.textBox1.Text = item.SavedChoices[0];
                    activeQuizCard.textBox2.Text = item.SavedChoices[1];
                    activeQuizCard.textBox3.Text = item.SavedChoices[2];
                    activeQuizCard.textBox4.Text = item.SavedChoices[3];

                    // Load correct answer back to ComboBox and Label inside UC
                    activeQuizCard.cmbCorrectAnswer.Text = item.SavedCorrectAnswer;
                    activeQuizCard.lblCorrectAns.Text = "Correct: " + item.SavedCorrectAnswer;
                }
            }

            cmbBXActType.SelectedIndexChanged += cmbBXActType_SelectedIndexChanged;
            isEditing = false;
        }

        private void ClearAllInputs()
        {
            txtActTitle.Clear();
            textBox22.Clear(); // Assignment instructions
            tempAttachedPath = "";
            lblFileNameDisplay.Text = "No file attached";

            // Reset ComboBox
            cmbBXActType.SelectedIndex = -1;
            cmbBXActType.Text = "";

            // Reset Quiz Card (if it exists)
            var activeQuizCard = flowLayoutPanel3.Controls.OfType<quizCreation>().FirstOrDefault();
            if (activeQuizCard != null)
            {
                activeQuizCard.Ques.Clear();
                activeQuizCard.textBox1.Clear();
                activeQuizCard.textBox2.Clear();
                activeQuizCard.textBox3.Clear();
                activeQuizCard.textBox4.Clear();
            }
        }

        private void flowLayoutPanel3_Resize(object sender, EventArgs e) // sa quiz din ung pinakapanel ng insert quiz
        {
            // ✅ Ensure 1250 is the anchor for centering
            int newMargin = (flowLayoutPanel3.Width - 1250 - 25) / 2;

            if (newMargin < 0) newMargin = 10;

            foreach (Control ctrl in flowLayoutPanel3.Controls)
            {
                // Apply the new margin to every card
                ctrl.Margin = new Padding(newMargin, 10, 10, 10);

                // Optional: Ensure the control hasn't accidentally resized itself
                ctrl.Size = new Size(1250, 456);
            }
        }

        private void RenumberQuestions() //sa quiz to ung add quiz - hansukal
        {
            int count = 1;
            // Loop through only the UserControls (ignoring the control bar)
            foreach (Control ctrl in flowLayoutPanel3.Controls)
            {
                if (ctrl is quizCreation card)
                {
                    card.lblQuestionNumber.Text = "Question " + count;
                    count++;
                }
            }
        }

        private void pnlLMSFiles_DragDrop(object sender, DragEventArgs e) //drag and drop feature ng class files
        {
            // We get the paths from the drag event
            string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string path in paths)
            {
                if (Directory.Exists(path))
                {
                    // We are passing 'path' here
                    AddFolderToListView(path);
                }
                else
                {
                    // We are passing 'path' here
                    AddFileToListView(path);
                }
            }
        }

        private void AddFolderToListView(string folderPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(folderPath);

            if (!imageList1.Images.ContainsKey("FolderIcon"))
            {
                imageList1.Images.Add("FolderIcon", Properties.Resources.folder_icon);
            }

            int iconIndex = imageList1.Images.IndexOfKey("FolderIcon");
            ListViewItem item = new ListViewItem(dirInfo.Name, iconIndex);
            item.Tag = folderPath;

            // 1. Add DATE first
            item.SubItems.Add(DateTime.Now.ToString("g"));

            // 2. Add "File Folder" second (as the size placeholder)
            item.SubItems.Add("File Folder");

            listView_file.Items.Add(item);
        }

        private void AddFileToListView(string filepath)
        {
            FileInfo fileInfo = new FileInfo(filepath);
            string ext = fileInfo.Extension.ToLower();
            long maxFileSize = 20 * 1024 * 1024; // 20MB

            if (fileInfo.Length <= maxFileSize)
            {
                // Handle Icons
                if (!imageList1.Images.ContainsKey(ext))
                {
                    if (ext == ".pdf")
                        imageList1.Images.Add(ext, Properties.Resources.pdf_icon);
                    else if (ext == ".png" || ext == ".jpg" || ext == ".jpeg")
                        imageList1.Images.Add(ext, Properties.Resources.png_icon);
                    else if (ext == ".ppt" || ext == ".pptx")
                        imageList1.Images.Add(ext, Properties.Resources.ppt_icon);
                    else
                        imageList1.Images.Add(ext, Icon.ExtractAssociatedIcon(filepath));
                }

                int iconIndex = imageList1.Images.IndexOfKey(ext);
                ListViewItem item = new ListViewItem(fileInfo.Name, iconIndex);
                item.Tag = filepath;

                // 1. Add DATE first (Matches Column 1)
                item.SubItems.Add(DateTime.Now.ToString("g"));

                // 2. Add SIZE second (Matches Column 2)
                double sizeInKB = fileInfo.Length / 1024.0;
                string sizeDisplay = (sizeInKB >= 1024)
                    ? $"{(sizeInKB / 1024.0):F2} MB"
                    : $"{sizeInKB:F2} KB";
                item.SubItems.Add(sizeDisplay);

                listView_file.Items.Add(item);
            }
            else
            {
                MessageBox.Show($"{fileInfo.Name} is over the 20MB limit.", "PUP Acad Portal",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            // 1. Visual Styling
            listView_file.Font = new Font("Segoe UI", 11.5f);
            listView_file.View = View.Details;
            listView_file.FullRowSelect = true;
            listView_file.GridLines = false;

            // 2. Row Spacing & Icons
            imageList1.ImageSize = new Size(32, 32);
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            listView_file.SmallImageList = imageList1;

            // 3. Define Columns (Swapped order: Name -> Date -> Size)
            listView_file.Columns.Clear();
            listView_file.Columns.Add("  File Name", 650);
            listView_file.Columns.Add("  Date Uploaded", 650); // Column index 1
            listView_file.Columns.Add("  Size", 350);          // Column index 2

            // This ensures that when the window gets bigger, the cards stretch to fit.
            flowLayoutPanelAnnouncements.Resize += (s, e) => UpdateCardWidths();

            // ENSURE LMS PANELS START HIDDEN
            pnlCreateAct.Visible = false;
            pnlQuiz1.Visible = false;
            pnlAssign.Visible = false;

            // Set a default value for the ComboBox so it's never "null"
            if (cmbBXActType.Items.Count > 0)
                cmbBXActType.SelectedIndex = 0;

            // This ensures that when the window gets bigger, the cards stretch to fit.
            flowLayoutPanelAnnouncements.Resize += (s, ev) => UpdateCardWidths();

            // Fixes the width of the cards in the Management area
            foreach (Control ctrl in ManageAct.Controls)
            {
                if (ctrl is ActivityItem item)
                {
                    item.Width = ManageAct.ClientSize.Width - 35;
                }
            }

            // Fixes the width of the cards in the Posted area
            foreach (Control ctrl in FlowPostedAct.Controls)
            {
                if (ctrl is ActivityItem item)
                {
                    item.Width = FlowPostedAct.ClientSize.Width - 35;
                }
            }
        }

        private void UpdateCardWidths()
        {
            flowLayoutPanelAnnouncements.SuspendLayout();
            foreach (Control c in flowLayoutPanelAnnouncements.Controls)
            {
                if (c is Panel card)
                {
                    card.Width = flowLayoutPanelAnnouncements.ClientSize.Width - 50;
                    ApplyRoundedRegion(card, 20);

                    foreach (Control child in card.Controls)
                    {
                        if (child is FlowLayoutPanel pnl)
                            pnl.Left = card.Width - pnl.Width - 20;

                        if (child is Label lbl && lbl.Name == "lblDesc")
                            lbl.Width = card.Width - 150;
                    }
                }
            }
            flowLayoutPanelAnnouncements.ResumeLayout();
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

        private void btnUpload_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "All Files (*.*)|*.*";
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // 1. Suspend the layout to prevent "flickering" or shifting while adding
                    listView_file.BeginUpdate();

                    foreach (string filePath in openFileDialog.FileNames)
                    {
                        AddFileToListView(filePath);
                    }

                    // 2. FORCE the columns to stay at your preferred widths
                    // This prevents the columns from jumping or changing format
                    listView_file.Columns[0].Width = 650; // File Name
                    listView_file.Columns[1].Width = 650; // Date Uploaded
                    listView_file.Columns[2].Width = 380; // Size

                    // 3. Resume the layout
                    listView_file.EndUpdate();
                }
            }
        }

        private void btnPostedAct_Click(object sender, EventArgs e)
        {
            pnlPostedAct.BringToFront();
            pnlPostedAct.Visible = true;
        }

        public class Announcement
        {
            public int Id;
            public string Title;
            public string Description;
            public DateTime Date;
            public bool IsPinned;
            public bool IsUrgent;
            public string Status;
        }

        private void btnPostAnn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAnnTitle.Text)) return;

            if (editingAnnouncementId != -1)
            {
                var a = announcements.Find(x => x.Id == editingAnnouncementId);
                if (a != null)
                {
                    a.Title = txtAnnTitle.Text;
                    a.Description = txtAnnDesc.Text;
                    a.Date = dateTimePicker4.Value;
                    a.IsPinned = checkPinned.Checked;
                    a.IsUrgent = chckUrgent.Checked;
                }
            }
            else
            {
                announcements.Insert(0, new Announcement
                {
                    Id = DateTime.Now.Millisecond,
                    Title = txtAnnTitle.Text,
                    Description = txtAnnDesc.Text,
                    Date = dateTimePicker4.Value,
                    IsPinned = checkPinned.Checked,
                    IsUrgent = chckUrgent.Checked,
                    Status = "active"
                });
            }

            // --- RESET AND HIDE ---
            editingAnnouncementId = -1;
            txtAnnTitle.Clear();
            txtAnnDesc.Clear();
            checkPinned.Checked = false;
            chckUrgent.Checked = false;

            // This hides the panel after the announcement is saved
            pnlCreateAnnounce1.Visible = false;

            // Refresh the list to show the new/edited announcement
            RenderAnnouncements();
        }

        private void RenderAnnouncements()
        {
            flowLayoutPanelAnnouncements.Controls.Clear();
            flowLayoutPanelAnnouncements.SuspendLayout();

            // Container setup to ensure vertical stacking
            flowLayoutPanelAnnouncements.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelAnnouncements.WrapContents = false;
            flowLayoutPanelAnnouncements.AutoScroll = true;

            var sorted = announcements
                .OrderByDescending(a => a.IsPinned)
                .ThenByDescending(a => a.Date)
                .ToList();

            foreach (var a in sorted)
            {
                // 1. Main Card Container (Responsive Width)
                Panel card = new Panel
                {
                    Width = flowLayoutPanelAnnouncements.ClientSize.Width - 50,
                    Height = 80,
                    BackColor = Color.White,
                    Margin = new Padding(10, 5, 10, 10),
                    Tag = false
                };

                // 2. Bell Icon Circle (Red background if active)
                Panel iconCircle = new Panel
                {
                    Size = new Size(50, 50),
                    Location = new Point(20, 15),
                    BackColor = a.Status == "active" ? Color.FromArgb(255, 235, 238) : Color.FromArgb(240, 240, 240)
                };
                ApplyRoundedRegion(iconCircle, 50);

                PictureBox picBell = new PictureBox
                {
                    Image = a.Status == "active" ? Properties.Resources.bell_icon : Properties.Resources.bell_gray,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(26, 26),
                    Location = new Point(12, 12),
                    BackColor = Color.Transparent
                };
                iconCircle.Controls.Add(picBell);

                // 3. Header Labels
                Label lblTitle = new Label { Text = a.Title, Font = new Font("Segoe UI Semibold", 11), Location = new Point(85, 18), AutoSize = true };
                Label lblSub = new Label { Text = $"All Section • {a.Date:MMM dd, yyyy}", Font = new Font("Segoe UI", 8), ForeColor = Color.Gray, Location = new Point(85, 42), AutoSize = true };

                // 4. Hidden Description
                Label lblDesc = new Label
                {
                    Name = "lblDesc",
                    Text = a.Description,
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(85, 85),
                    Width = card.Width - 150,
                    AutoSize = true,
                    Visible = false
                };

                // 5. Action Panel (Anchored Right)
                FlowLayoutPanel pnlActions = new FlowLayoutPanel
                {
                    Name = "pnlActions",
                    AutoSize = true,
                    FlowDirection = FlowDirection.LeftToRight,
                    BackColor = Color.Transparent,
                    WrapContents = false,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };

                // Aligned Status Badge
                Label lblStatus = new Label
                {
                    Text = a.Status == "active" ? "Active" : "Inactive",
                    BackColor = a.Status == "active" ? Color.FromArgb(200, 240, 200) : Color.LightGray,
                    ForeColor = a.Status == "active" ? Color.DarkGreen : Color.DimGray,
                    Size = new Size(65, 25),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(0, 4, 10, 0)
                };
                ApplyRoundedRegion(lblStatus, 10);

                // Image Buttons (24x24 icons)
                Button btnExpand = CreateImgButton(Properties.Resources.arrow_down, (s, e) => ToggleExpand(card, lblDesc, (Button)s));
                Button btnToggle = CreateImgButton(Properties.Resources.power_icon, (s, e) => ToggleAnnouncement(a.Id));
                Button btnEdit = CreateImgButton(Properties.Resources.edit_icon, (s, e) => EditAnnouncement(a.Id));
                Button btnDelete = CreateImgButton(Properties.Resources.delete_icon, (s, e) => DeleteAnnouncement(a.Id));

                pnlActions.Controls.AddRange(new Control[] { lblStatus, btnExpand, btnToggle, btnEdit, btnDelete });

                // Add everything to card
                card.Controls.Add(pnlActions);
                pnlActions.Location = new Point(card.Width - pnlActions.PreferredSize.Width - 20, 22);

                card.Controls.Add(iconCircle);
                card.Controls.Add(lblTitle);
                card.Controls.Add(lblSub);
                card.Controls.Add(lblDesc);

                ApplyRoundedRegion(card, 20);
                flowLayoutPanelAnnouncements.Controls.Add(card);
            }
            flowLayoutPanelAnnouncements.ResumeLayout();
        }

        // Helper for clean 24x24 icon buttons
        private Button CreateImgButton(Image img, EventHandler onClick)
        {
            Button btn = new Button
            {
                Image = img,
                Size = new Size(32, 32), // Area is 32, icon is 24 inside
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                ImageAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(2, 0, 2, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(245, 245, 245);
            btn.Click += onClick;
            return btn;
        }


        // Logic to show/hide the description and swap arrow images
        private void ToggleExpand(Panel card, Label desc, Button btn)
        {
            bool isExpanded = (bool)card.Tag;
            if (isExpanded)
            {
                card.Height = 80;
                btn.Image = Properties.Resources.arrow_down;
                desc.Visible = false;
            }
            else
            {
                // Set height based on description
                card.Height = 110 + desc.Height;
                btn.Image = Properties.Resources.up_arrow;
                desc.Visible = true;
            }
            card.Tag = !isExpanded;

            // Refresh rounding for the new height
            ApplyRoundedRegion(card, 20);
        }

        private void CenterAnnouncementPanel()
        {
            // 1. Identify your "Main" layout boundaries
            // We want the center of the WHITE/GRAY space, not the whole window
            int sidebarWidth = pnlSidebar.Width;
            int headerHeight = pnlHeader.Height;

            // 2. Calculate the "Work Area" (the part where your LMS content shows)
            int workWidth = this.ClientSize.Width - sidebarWidth;
            int workHeight = this.ClientSize.Height - headerHeight;

            // 3. Calculate X and Y
            // X = Sidebar width + half of the remaining horizontal space
            int x = sidebarWidth + (workWidth - pnlCreateAnnounce1.Width) / 2;

            // Y = Header height + half of the remaining vertical space
            int y = headerHeight + (workHeight - pnlCreateAnnounce1.Height) / 2;

            // 4. Force position and layering
            pnlCreateAnnounce1.Location = new Point(x, y);
            pnlCreateAnnounce1.BringToFront();
        }


        // Helper to make the icons look clean
        private Button CreateIconButton(string icon, EventHandler onClick)
        {
            Button btn = new Button { Text = icon, Size = new Size(35, 30), FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 12) };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += onClick;
            return btn;
        }

        private void ApplyRoundedRegion(Control control, int radius)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(control.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(control.Width - radius, control.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, control.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            control.Region = new Region(path);
        }

        private void EditAnnouncement(int id)
        {
            var a = announcements.Find(x => x.Id == id);
            if (a == null) return;

            editingAnnouncementId = id;
            txtAnnTitle.Text = a.Title;
            txtAnnDesc.Text = a.Description;
            dateTimePicker4.Value = a.Date;
            checkPinned.Checked = a.IsPinned;
            chckUrgent.Checked = a.IsUrgent;

            // Ensure correct panel name (pnlCreateAnnounce)
            pnlCreateAnnounce1.Visible = true;
            pnlCreateAnnounce1.BringToFront();
        }

        private void ToggleAnnouncement(int id)
        {
            var a = announcements.Find(x => x.Id == id);
            if (a != null)
                a.Status = a.Status == "active" ? "inactive" : "active";

            RenderAnnouncements();
        }
        private void DeleteAnnouncement(int id)
        {
            announcements.RemoveAll(x => x.Id == id);
            RenderAnnouncements();
        }

        private void btnCreateAnnouncement_Click(object sender, EventArgs e)
        {
            pnlCreateAnnounce1.Visible = true;
            CenterAnnouncementPanel(); // Position it immediately
        }

        private void CenterCreateAnnouncementPanel()
        {
            if (pnlCreateAnnounce1 != null && pnlCreateAnnounce1.Visible)
            {
                pnlCreateAnnounce1.BringToFront();

                // Get the actual container (the gray area)
                Control parent = pnlCreateAnnounce1.Parent;

                // Use ClientSize to ignore borders/scrollbars
                // Your /4 logic for Y is great for visual balance
                int x = (parent.ClientSize.Width - pnlCreateAnnounce1.Width) / 2;
                int y = (parent.ClientSize.Height - pnlCreateAnnounce1.Height) / 4;

                // Set the location directly
                pnlCreateAnnounce1.Location = new Point(x, y);
            }
        }



        private void InstructorPortal_Resize(object sender, EventArgs e)
        {


        }

        private void panel18_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button49Canel_Click(object sender, EventArgs e)
        {
            pnlCreateAnnounce1.Visible = false;
            pnlCreateAnnounce1.SendToBack();
        }

        private void ManageAct_Resize(object sender, EventArgs e)
        {
            ManageAct.SuspendLayout();
            foreach (Control c in ManageAct.Controls)
            {
                // This makes sure that when the window gets bigger, 
                // every single UC inside grows to match the new width.
                c.Width = ManageAct.ClientSize.Width - 35;
            }
            ManageAct.ResumeLayout();
        }

        private void btnSaveAss_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtActTitle.Text))
            {
                MessageBox.Show("Please enter an Activity Title.", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string confirmMsg = (currentEditingItem == null) ? "Create this Assignment card?" : "Save changes to this Assignment?";

            if (MessageBox.Show(confirmMsg, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ActivityItem targetItem;

                if (currentEditingItem == null)
                {
                    targetItem = new ActivityItem();
                    targetItem.Width = ManageAct.ClientSize.Width - 40;
                    targetItem.Height = 187;

                    // ONLY keep the Edit logic here, because editing depends on the Form's variables.
                    targetItem.btnEdit.Click += (s, ev) => LoadItemForEditing(targetItem);

                    // REMOVED: targetItem.btnRemove.Click += ... (The UC handles this internally now)

                    ManageAct.Controls.Add(targetItem);
                    ManageAct.Controls.SetChildIndex(targetItem, 0);
                }
                else
                {
                    targetItem = currentEditingItem;
                }

                targetItem.Tag = "ASSIGNMENT";
                targetItem.actPic.Image = Properties.Resources.paper;

                targetItem.lblTitle.Text = txtActTitle.Text;
                targetItem.SavedTitle = txtActTitle.Text;
                targetItem.lblDueDate.Text = "Due : " + dateTimePicker1.Value.ToString("MMMM dd, hh:mm tt");

                targetItem.SavedInstructions = textBox22.Text;
                targetItem.SavedAttachedFilePath = tempAttachedPath;

                ClearAllInputs();
                currentEditingItem = null;
                pnlCreateAct.Visible = false;
            }
        }
    }
}
