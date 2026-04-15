using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using static PUPAcadPortal.InstructorPortal;


namespace PUPAcadPortal
{
    public partial class InstructorPortal : Form
    {
        private Button clickedButton;
        private Color defaultColor = Color.Maroon;
        private Color selectedColor = Color.FromArgb(109, 0, 0);

        // ✅ ANNOUNCEMENT SYSTEM VARIABLES (GLOBAL)
        List<Announcement> announcements = new List<Announcement>();
        int editingAnnouncementId = -1;
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
            // 1. Tell the control to use your custom string
            dateTimePicker2.Format = DateTimePickerFormat.Custom;

            // 2. Set the pattern (no extra spaces)
            // "ddd" = Fri, "MMM" = Apr, "dd" = 10
            dateTimePicker2.CustomFormat = "ddd, MMM dd, yyyy";
            this.Resize += (s, e) => UpdateCardWidths();
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
            pnlLMSFiles.Visible = true;

            // 2. Bring it to the front so it's not hidden behind other main panels
            pnlLMSFiles.BringToFront();
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

        private void flowLayoutPanel3_Resize(object sender, EventArgs e) // sa quiz din ung pinakapanel ng insert quiz
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

        private void RenumberQuestions() //sa quiz to ung add quiz - hansukal
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


        }

        private void UpdateCardWidths()
        {
            flowLayoutPanelAnnouncements.SuspendLayout();
            foreach (Control c in flowLayoutPanelAnnouncements.Controls)
            {
                if (c is Panel card)
                {
                    // Stretch the card to fill the container
                    card.Width = flowLayoutPanelAnnouncements.ClientSize.Width - 40;
                    ApplyRoundedRegion(card, 20);

                    // Move the buttons to the new right edge
                    foreach (Control sub in card.Controls)
                    {
                        if (sub is FlowLayoutPanel pnl)
                        {
                            pnl.Left = card.Width - pnl.Width - 20;
                        }
                        if (sub is Label lbl && lbl.Name == "lblDesc") // ensure lblDesc has a name
                        {
                            lbl.Width = card.Width - 150;
                        }
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

            // RESET
            editingAnnouncementId = -1;
            txtAnnTitle.Clear();
            txtAnnDesc.Clear();
            checkPinned.Checked = false;
            chckUrgent.Checked = false;

            pnlCreateAnnounce.Visible = false;

            RenderAnnouncements();
        }

        private void RenderAnnouncements()
        {
            flowLayoutPanelAnnouncements.Controls.Clear();
            flowLayoutPanelAnnouncements.SuspendLayout();

            // Ensure the container allows children to stretch
            flowLayoutPanelAnnouncements.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelAnnouncements.WrapContents = false;

            var sorted = announcements
                .OrderByDescending(a => a.IsPinned)
                .ThenByDescending(a => a.Date)
                .ToList();

            foreach (var a in sorted)
            {
                // 1. Main Card Container
                Panel card = new Panel
                {
                    // Subtracting 40 to account for padding/scrollbar
                    Width = flowLayoutPanelAnnouncements.ClientSize.Width - 40,
                    Height = 80,
                    BackColor = Color.White,
                    Margin = new Padding(10, 0, 10, 15),
                    Tag = false
                };

                // 2. Bell Icon Circle (RED BACKGROUND)
                Panel iconCircle = new Panel
                {
                    Size = new Size(50, 50),
                    Location = new Point(20, 15),
                    // Set this to Red to match your request
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
                Label lblSub = new Label { Text = $"All users • {a.Date:MMM dd, yyyy}", Font = new Font("Segoe UI", 8), ForeColor = Color.Gray, Location = new Point(85, 42), AutoSize = true };

                // 4. Description (The Expandable Body)
                Label lblDesc = new Label
                {
                    Text = a.Description,
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(85, 85),
                    // Stretch description width too
                    Width = card.Width - 150,
                    AutoSize = true,
                    Visible = false
                };

                // 5. Action Buttons Panel (RIGHT ALIGNED)
                FlowLayoutPanel pnlActions = new FlowLayoutPanel
                {
                    AutoSize = true,
                    // Anchor to the Right
                    Location = new Point(card.Width - 320, 20),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    FlowDirection = FlowDirection.LeftToRight,
                    BackColor = Color.Transparent
                };

                // Badge
                Label lblStatus = new Label
                {
                    Text = a.Status == "active" ? "Active" : "Inactive",
                    BackColor = a.Status == "active" ? Color.FromArgb(200, 240, 200) : Color.LightGray,
                    Size = new Size(65, 25),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(0, 0, 10, 0)
                };
                ApplyRoundedRegion(lblStatus, 10);

                // Create Buttons
                Button btnExpand = CreateImgButton(Properties.Resources.arrow_down, (s, e) => ToggleExpand(card, lblDesc, (Button)s));
                Button btnToggle = CreateImgButton(Properties.Resources.power_icon, (s, e) => ToggleAnnouncement(a.Id));
                Button btnEdit = CreateImgButton(Properties.Resources.edit_icon, (s, e) => EditAnnouncement(a.Id));
                Button btnDelete = CreateImgButton(Properties.Resources.delete_icon, (s, e) => DeleteAnnouncement(a.Id));

                pnlActions.Controls.AddRange(new Control[] { lblStatus, btnExpand, btnToggle, btnEdit, btnDelete });

                // Add to Card
                card.Controls.Add(pnlActions);
                card.Controls.Add(iconCircle);
                card.Controls.Add(lblTitle);
                card.Controls.Add(lblSub);
                card.Controls.Add(lblDesc);

                ApplyRoundedRegion(card, 20);
                flowLayoutPanelAnnouncements.Controls.Add(card);
            }
            flowLayoutPanelAnnouncements.ResumeLayout();
        }

        private Button CreateImgButton(Image img, EventHandler onClick)
        {
            Button btn = new Button
            {
                Image = img,
                Size = new Size(35, 35),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                ImageAlign = ContentAlignment.MiddleCenter
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

            pnlCreateAnnounce.Visible = true;
            pnlCreateAnnounce.BringToFront();
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
            pnlCreateAnnounce1.BringToFront();

            // Optional: center it
            pnlCreateAnnounce1.Location = new Point(
                (pnlAnnounce.Width - pnlCreateAnnounce1.Width) / 2,
                (pnlAnnounce.Height - pnlCreateAnnounce1.Height) / 2
            );
        }
    }
}
