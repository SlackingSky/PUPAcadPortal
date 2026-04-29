using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
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
        public static int _year, _month;
        public static Dictionary<DateTime, string> notesDict = new Dictionary<DateTime, string>();
        private FlowLayoutPanel pnlDayHeaders;
        private ActivityItem? currentEditingItem = null;
        private Button clickedButton;
        private Color defaultColor = Color.Maroon;
        private Color selectedColor = Color.FromArgb(109, 0, 0);
        private Panel pnlBottom;
        private Label lblSelectedDate;
        private FlowLayoutPanel flpDayEvents;
        private FlowLayoutPanel flpUpcoming;
        private Label lblNoEvents;
        private Label lblNoUpcoming;
        private DateTime _lastSelectedDate = DateTime.Now.Date;
        private EventType? _activeFilter = null;


        // ✅ ANNOUNCEMENT SYSTEM VARIABLES (GLOBAL)
        List<Announcement> announcements = new List<Announcement>();
        int editingAnnouncementId = -1;

        private string tempAttachedPath = "";
        public InstructorPortal()
        {
            InitializeComponent();
            pnlAttendance.AutoScrollMinSize = new Size(0, 1000);
            SharedCalendarData.LoadData();
            pnlAttendance.AutoScrollMinSize = new Size(0, 1000);
            this.DoubleBuffered = true;
            // DateTimePicker Formatting
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/dd/yyyy hh:mm tt";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "ddd, MMM dd, yyyy";

            // ✅ FIX: Wire the resize event ONLY ONCE here.
            // If you put this inside a Click event, it stacks and causes the "creeping" movement.
            //Temporary Data (Grade Feature)
            string[] row1 = { "2024-00074-SM-0", "Ablong, Adrian P." };
            string[] row2 = { "2024-00194-SM-0", "Alcaiz, Jared B." };
            string[] row3 = { "2024-00146-SM-0", "Amar, Charles Manuel C." };
            string[] row4 = { "2024-00123-SM-0", "Amen, Jessie C." };
            string[] row5 = { "2024-00274-SM-0", "Amolata, Jhayphee V." };

            dataGridView1.Rows.Add(row1);
            dataGridView1.Rows.Add(row2);
            dataGridView1.Rows.Add(row3);
            dataGridView1.Rows.Add(row4);
            dataGridView1.Rows.Add(row5);

            if (panel3 != null)
            {
                panel3.Dock = DockStyle.Fill;
            }

            SetupQuickActions();

            if (btnDashboard != null)
            {
                changeButtonColor(btnDashboard);
                showContent(btnDashboard);
            }

            SetupGradeLogic();

        }

        // --- NEW: QUICK ACTIONS CLICK FIX ---
        private void SetupQuickActions()
        {
            void BindClick(Control ctrl, EventHandler handler)
            {
                if (ctrl == null) return;

                ctrl.Click += handler;
                ctrl.Cursor = Cursors.Hand;

                foreach (Control child in ctrl.Controls)
                {
                    BindClick(child, handler);
                }
            }

            BindClick(panel105, panel105_Click);
            BindClick(panel103, panel103_Click);
            BindClick(panel102, panel102_Click);
            BindClick(panel104, panel104_Click);
        }

        // --- GRADE MANAGEMENT LOGIC ---
        private void SetupGradeLogic()
        {
            if (dataGridView2 != null)
            {               
                dataGridView2.AllowUserToAddRows = false;
                dataGridView2.AllowUserToOrderColumns = false;
                dataGridView2.AllowUserToResizeColumns = false;
                dataGridView2.AllowUserToResizeRows = false;

                // --- FIX 1.5: Anchor to stretch downward, and AutoSize to fill the gray space ---
                dataGridView2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

            if (cmbSelectCourse != null && cmbSelectCourse.Items.Count == 0)
            {
                cmbSelectCourse.Items.Add("IT 101 - Introduction to Computing");
                cmbSelectCourse.Items.Add("CS 102 - Data Structures");
                cmbSelectCourse.Items.Add("IS 103 - Database Management");
            }

            if (dataGridView2 != null && dataGridView2.Rows.Count == 0)
            {
                dataGridView2.Rows.Add("2021-00001-SM-0", "Eisen Nodesca", "85", "88");
                dataGridView2.Rows.Add("2021-00002-SM-0", "Clarisa Matias", "92", "95");
                dataGridView2.Rows.Add("2021-00003-SM-0", "Trisha Walang Last Name", "78", "82");
                dataGridView2.Rows.Add("2021-00004-SM-0", "Liza Soberano", "88", "90");
                dataGridView2.Rows.Add("2021-00005-SM-0", "Kween Yasmin", "72", "75");
                dataGridView2.Rows.Add("2021-00006-SM-0", "Maine Love Alden", "98", "89");
            }

            // 2. AUTO-CALCULATE GRADES LOGIC
            if (dataGridView2 != null)
            {
                dataGridView2.CellValueChanged += (s, e) =>
                {
                    if (e.RowIndex >= 0 && (e.ColumnIndex == 2 || e.ColumnIndex == 3))
                    {
                        IList cells = dataGridView2.Rows[e.RowIndex].Cells;
                        // FIX: Re-applied the missing array indexes so it won't crash!
                        DataGridViewCell midCell = (DataGridViewCell)cells;
                        DataGridViewCell finCell = (DataGridViewCell)cells;
                        DataGridViewCell avgCell = (DataGridViewCell)cells;
                        DataGridViewCell remCell = (DataGridViewCell)cells;

                        double m, f;
                        bool hasMid = double.TryParse(Convert.ToString(midCell.Value), out m);
                        bool hasFin = double.TryParse(Convert.ToString(finCell.Value), out f);

                        if (hasMid && hasFin)
                        {
                            double avg = (m + f) / 2.0;
                            avgCell.Value = avg.ToString("F2");
                            remCell.Value = avg >= 75.0 ? "Passed" : "Failed";
                            if (remCell.Style != null) remCell.Style.ForeColor = avg >= 75.0 ? Color.Green : Color.Red;
                        }
                        else
                        {
                            avgCell.Value = "";
                            remCell.Value = "Incomplete";
                            if (remCell.Style != null) remCell.Style.ForeColor = Color.Gray;
                        }
                    }
                };
            }

            // 3. SEARCH BAR LOGIC
            if (textBox1 != null)
            {
                textBox1.TextChanged += (s, e) =>
                {
                    string q = textBox1.Text != null ? textBox1.Text.Trim().ToLower() : "";

                    if (dataGridView2 != null)
                    {
                        dataGridView2.CurrentCell = null;

                        foreach (DataGridViewRow r in dataGridView2.Rows)
                        {
                            if (r.IsNewRow) continue;

                            IList cells = r.Cells;

                            // FIX: Re-applied the missing array indexes here too!
                            DataGridViewCell cell0 = (DataGridViewCell)cells;
                            DataGridViewCell cell1 = (DataGridViewCell)cells;

                            string sn = cell0.Value != null ? cell0.Value.ToString().ToLower() : "";
                            string nm = cell1.Value != null ? cell1.Value.ToString().ToLower() : "";

                            r.Visible = string.IsNullOrEmpty(q) || sn.Contains(q) || nm.Contains(q);
                        }
                    }
                };
            }
        }

        // --- SIDEBAR NAVIGATION LOGIC ---
        private void changeButtonColor(Button button)
        {
            if (button == null) return;

            if (clickedButton != null)
            {
                clickedButton.BackColor = defaultColor;
            }

            clickedButton = button;

            if (pnlYellow == null)
            {
                pnlYellow = new Panel
                {
                    Name = "pnlYellow",
                    Size = new Size(6, 64),
                    BackColor = Color.Gold,
                    Visible = false
                };
                if (pnlSidebar != null)
                    pnlSidebar.Controls.Add(pnlYellow);
                else
                    this.Controls.Add(pnlYellow);
            }

            clickedButton.BackColor = selectedColor;
            pnlYellow.Visible = true;
            pnlYellow.Parent = button;
            pnlYellow.Height = button.Height;
            pnlYellow.BringToFront();

        }

        //Method na nagpapakita ng content ng bawat button, wala akong maisip na iba kaya eto
        private void showContent(Button button)
        {
            if (button == null) return;

            var contents = new Dictionary<Button, Panel>() 
            {
                { btnDashboard, pnlDashboardContent },
                { btnGrades, pnlGradesContent },
                { btnAnnounceIns, pnlAnnouncement  },
                { btnCalendarIns, pnlCalendar },
                { btnSubjectIns, pnlSubject },
                { btnActivitiesIns, pnlLMSAct },
                { btnAttendanceIns, pnlAttendance },
                { btnGradeIns, pnlGrades }
            };

            foreach (var content in contents)
            {
                if (content.Key == button)
                {
                    content.Value.Visible = true;
                    content.Value.Parent = panel3;
                    content.Value.Dock = DockStyle.Fill;
                    content.Value.BringToFront();
                }
                else
                {
                    if (content.Value != null)
                    {
                        content.Value.Visible = false;
                    }
                }
            }
        }

        //Method para pag pinindot yung X sa taas o mag alt-F4, icclose lahat ng forms para di magerror pag ni run uli
        //Lagay to sa bawat form na iaadd, Step 1: Hanapin sa properties ng form yung event na FormClosing, Step 2: Double click para gumawa ng method, Step 3: Copy paste code na nasa loob nito
        private void InstructorPortal_Closing(object sender, FormClosingEventArgs e)
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
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnCalendarIns_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnSubjectIns_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnActivitiesIns_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnAttendanceIns_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnGradeIns_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
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
            this.BeginInvoke((MethodInvoker)delegate
            {
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

            if (MessageBox.Show("Save Quiz Activity?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ActivityItem targetItem;

                if (currentEditingItem == null)
                {
                    targetItem = new ActivityItem();
                    targetItem.Width = ManageAct.ClientSize.Width - 35;
                    targetItem.Height = 187;

                    targetItem.btnEdit.Click += (s, ev) => LoadItemForEditing(targetItem);

                    // --- REMOVED THE btnRemove.Click LISTENER FROM HERE ---
                    // It is now handled internally by the ActivityItem class.

                    ManageAct.Controls.Add(targetItem);
                    ManageAct.Controls.SetChildIndex(targetItem, 0);
                }
                else
                {
                    targetItem = currentEditingItem;
                }

                targetItem.Tag = "QUIZ";
                targetItem.actPic.Image = Properties.Resources.quiz;
                targetItem.lblTitle.Text = txtActTitle.Text;
                targetItem.lblDueDate.Text = "Due : " + dateTimePicker1.Value.ToString("MM/dd/yyyy hh:mm tt");
                targetItem.SavedTitle = txtActTitle.Text;
                targetItem.SavedQuestion = activeQuizCard.Ques.Text;
                targetItem.SavedChoices[0] = activeQuizCard.textBox1.Text;
                targetItem.SavedChoices[1] = activeQuizCard.textBox2.Text;
                targetItem.SavedChoices[2] = activeQuizCard.textBox3.Text;
                targetItem.SavedChoices[3] = activeQuizCard.textBox4.Text;
                targetItem.SavedCorrectAnswer = activeQuizCard.cmbCorrectAnswer.Text;

                ClearAllInputs();
                currentEditingItem = null;
                pnlCreateAct.Visible = false;

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
            BuildDayHeaders();
            BuildBottomPanel();

            FPLmonth.Resize += (s, ev) => { ResizeCalendarCells(); AlignDayHeaders(); };
            this.Resize += OnFormResized;

            showDays(DateTime.Now.Month, DateTime.Now.Year);

            UrDay.DaySelected += OnDaySelected;
            OnDaySelected(DateTime.Now.Date);

            var wheelFilter = new CalendarWheelFilter(FPLmonth, delta =>
            {
                if (delta > 0) picPrev_Click(this, EventArgs.Empty);
                else picNext_Click(this, EventArgs.Empty);
            });
            System.Windows.Forms.Application.AddMessageFilter(wheelFilter);

            this.FormClosed += (s, ev) =>
            {
                UrDay.DaySelected -= OnDaySelected;
                System.Windows.Forms.Application.RemoveMessageFilter(wheelFilter);
                this.Resize -= OnFormResized;
                SharedCalendarData.SaveData();
            };

            this.BeginInvoke((Action)(() =>
            {
                FitCalendarPanel();
                ResizeCalendarCells();
                CenterMonthLabel();
                PositionBottomPanel();
            }));
        }

        private void BuildBottomPanel()
        {
            const int BOTTOM_H = 220;

            pnlBottom = new Panel
            {
                BackColor = Color.White,
                Height = BOTTOM_H,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
            };
            pnlBottom.Parent = pnlCalendar;
            pnlBottom.BringToFront();

            var sep = new Panel { Height = 1, Dock = DockStyle.Top, BackColor = Color.FromArgb(220, 220, 220) };
            pnlBottom.Controls.Add(sep);

            var pnlLeft = new Panel { Left = 0, Top = 4, Width = 520, Height = BOTTOM_H - 8, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom };
            pnlBottom.Controls.Add(pnlLeft);

            lblSelectedDate = new Label
            {
                Text = "Select a day to manage events",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.Maroon,
                Left = 12,
                Top = 6,
                AutoSize = true,
            };
            pnlLeft.Controls.Add(lblSelectedDate);

            var btnAddEvent = new Button
            {
                Text = "+ Add Event",
                Left = 12,
                Top = 28,
                Width = 95,
                Height = 26,
                BackColor = Color.Maroon,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5f),
                Cursor = Cursors.Hand,
            };
            btnAddEvent.FlatAppearance.BorderSize = 0;
            btnAddEvent.Click += (s, e) => QuickAddEventForSelected();
            pnlLeft.Controls.Add(btnAddEvent);

            int bx = 118;
            var filters = new[] {
                ("All",      (EventType?)null),
                ("Class",    (EventType?)EventType.Class),
                ("Exam",     (EventType?)EventType.Exam),
                ("Deadline", (EventType?)EventType.Deadline),
                ("Consult",  (EventType?)EventType.Consultation),
            };
            foreach (var (label, ft) in filters)
            {
                var captured = ft;
                Color accent = ft == null ? Color.FromArgb(90, 90, 90) : new CalendarEvent { Type = ft.Value }.GetColor();
                var btn = MakeFilterButton(label, accent);
                btn.Left = bx; btn.Top = 30;
                btn.Click += (s, e) =>
                {
                    _activeFilter = captured;
                    RefreshDayDetail(_lastSelectedDate);
                };
                pnlLeft.Controls.Add(btn);
                bx += btn.Width + 5;
            }

            flpDayEvents = new FlowLayoutPanel
            {
                Left = 0,
                Top = 62,
                Width = 510,
                Height = BOTTOM_H - 70,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
            };
            lblNoEvents = new Label { Text = "No events for this day. Use '+ Add Event' to create one.", ForeColor = Color.Gray, AutoSize = true, Padding = new Padding(8) };
            flpDayEvents.Controls.Add(lblNoEvents);
            pnlLeft.Controls.Add(flpDayEvents);

            var div = new Panel { Left = 530, Top = 8, Width = 1, Height = BOTTOM_H - 16, BackColor = Color.FromArgb(220, 220, 220), Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left };
            pnlBottom.Controls.Add(div);
            var pnlRight = new Panel { Left = 538, Top = 4, Height = BOTTOM_H - 8, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom };
            pnlBottom.Controls.Add(pnlRight);

            var lblUpTitle = new Label { Text = "Upcoming", Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = Color.FromArgb(60, 60, 60), Left = 8, Top = 6, AutoSize = true };
            pnlRight.Controls.Add(lblUpTitle);

            BuildLegend(pnlRight, 8, 30);

            flpUpcoming = new FlowLayoutPanel
            {
                Left = 0,
                Top = 80,
                Width = 400,
                Height = BOTTOM_H - 90,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
            };
            lblNoUpcoming = new Label { Text = "No upcoming events.", ForeColor = Color.Gray, AutoSize = true, Padding = new Padding(8) };
            flpUpcoming.Controls.Add(lblNoUpcoming);
            pnlRight.Controls.Add(flpUpcoming);

            RefreshUpcoming();
            PositionBottomPanel();
        }
        private Button MakeFilterButton(string label, Color accent)
        {
            var btn = new Button
            {
                Text = label,
                Width = 70,
                Height = 24,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8f),
                BackColor = Color.White,
                ForeColor = accent,
            };
            btn.FlatAppearance.BorderColor = accent;
            return btn;
        }
        private void BuildLegend(Panel parent, int x, int y)
        {
            var types = new[] {
                (EventType.Class,        "Class"),
                (EventType.Exam,         "Exam"),
                (EventType.Deadline,     "Deadline"),
                (EventType.Consultation, "Consult"),
                (EventType.Cancelled,    "Cancelled"),
            };
            int cx = x;
            foreach (var (t, name) in types)
            {
                Color c = new CalendarEvent { Type = t }.GetColor();
                var dot = new Panel { Width = 10, Height = 10, BackColor = c, Left = cx, Top = y + 2 };
                var lbl = new Label { Text = name, Left = cx + 13, Top = y, AutoSize = true, Font = new Font("Segoe UI", 8f), ForeColor = Color.FromArgb(60, 60, 60) };
                parent.Controls.Add(dot);
                parent.Controls.Add(lbl);
                cx += 75;
            }
        }

        private void QuickAddEventForSelected()
        {
            using var dlg = new AddEventForm(_lastSelectedDate);
            if (dlg.ShowDialog() == DialogResult.OK && dlg.CreatedEvent != null)
            {
                SharedCalendarData.AddEvent(_lastSelectedDate, dlg.CreatedEvent);
                foreach (Control ctrl in FPLmonth.Controls)
                    if (ctrl is UrDay ud) ud.RefreshEventPills();

                RefreshDayDetail(_lastSelectedDate);
                RefreshUpcoming();
            }
        }
        private void OnDaySelected(DateTime date)
        {
            _lastSelectedDate = date;
            RefreshDayDetail(date);
            RefreshUpcoming();
        }
        private void RefreshDayDetail(DateTime date)
        {
            _lastSelectedDate = date;
            if (lblSelectedDate != null)
                lblSelectedDate.Text = date.ToString("dddd, MMMM dd, yyyy");

            if (flpDayEvents == null) return;
            flpDayEvents.Controls.Clear();

            var events = SharedCalendarData.GetEventsForDate(date)
                .Where(ev => _activeFilter == null || ev.Type == _activeFilter)
                .ToList();

            if (notesDict.ContainsKey(date.Date) && !string.IsNullOrWhiteSpace(notesDict[date.Date]))
            {
                flpDayEvents.Controls.Add(MakeEventCard(
                    "🗒 Note", notesDict[date.Date],
                    Color.FromArgb(100, 100, 100), date, null));
            }

            if (events.Count == 0 && flpDayEvents.Controls.Count == 0)
            {
                flpDayEvents.Controls.Add(lblNoEvents);
                return;
            }

            foreach (var ev in events)
            {
                string body = "";
                if (!string.IsNullOrEmpty(ev.StartTime)) body += ev.StartTime + (string.IsNullOrEmpty(ev.EndTime) ? "" : " – " + ev.EndTime) + "\n";
                if (!string.IsNullOrEmpty(ev.Room)) body += "Room: " + ev.Room + "\n";
                body += ev.Description;
                flpDayEvents.Controls.Add(MakeEventCard($"[{ev.GetTypeLabel()}]  {ev.Title}", body.Trim(), ev.GetColor(), date, ev));
            }
        }
        private void RefreshUpcoming()
        {
            if (flpUpcoming == null) return;
            flpUpcoming.Controls.Clear();
            var upcoming = SharedCalendarData.GetUpcoming(6);
            if (upcoming.Count == 0) { flpUpcoming.Controls.Add(lblNoUpcoming); return; }
            foreach (var (d, ev) in upcoming)
            {
                int daysLeft = (d.Date - DateTime.Now.Date).Days;
                string when = daysLeft == 0 ? "Today" : daysLeft == 1 ? "Tomorrow" : $"In {daysLeft} days";
                flpUpcoming.Controls.Add(MakeUpcomingStrip(ev, d, when));
            }
        }
        private Panel MakeEventCard(string title, string body, Color accent, DateTime date, CalendarEvent ev)
        {
            var card = new Panel
            {
                Width = (flpDayEvents.Width > 20 ? flpDayEvents.Width : 460) - 16,
                Height = 48,
                BackColor = Color.FromArgb(245, 248, 255),
                Margin = new Padding(4, 3, 4, 0),
            };
            var bar = new Panel { Width = 5, Height = card.Height, Left = 0, Top = 0, BackColor = accent };
            var lblT = new Label { Text = title, Left = 12, Top = 3, Width = card.Width - 80, Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = Color.FromArgb(40, 40, 40), AutoSize = false, Height = 18 };
            var lblB = new Label { Text = body, Left = 12, Top = 22, Width = card.Width - 80, Font = new Font("Segoe UI", 8f), ForeColor = Color.FromArgb(90, 90, 90), AutoSize = false, Height = 22, AutoEllipsis = true };

            card.Controls.AddRange(new Control[] { bar, lblT, lblB });

            if (ev != null)
            {
                var btnDel = new Button
                {
                    Text = "✕",
                    Left = card.Width - 30,
                    Top = 12,
                    Width = 22,
                    Height = 22,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.Gray,
                    BackColor = Color.Transparent,
                    Font = new Font("Segoe UI", 8f),
                    Cursor = Cursors.Hand,
                };
                btnDel.FlatAppearance.BorderSize = 0;
                var capturedEv = ev;
                btnDel.Click += (s, e) =>
                {
                    if (MessageBox.Show($"Remove '{capturedEv.Title}'?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SharedCalendarData.RemoveEvent(date, capturedEv);
                        foreach (Control ctrl in FPLmonth.Controls)
                            if (ctrl is UrDay ud) ud.RefreshEventPills();
                        RefreshDayDetail(date);
                        RefreshUpcoming();
                    }
                };
                card.Controls.Add(btnDel);
            }

            return card;
        }
        private Panel MakeUpcomingStrip(CalendarEvent ev, DateTime date, string when)
        {
            var strip = new Panel { Width = (flpUpcoming.Width > 10 ? flpUpcoming.Width : 380) - 8, Height = 36, BackColor = Color.White, Margin = new Padding(4, 2, 4, 0) };
            var dot = new Panel { Width = 8, Height = 8, Top = 14, Left = 4, BackColor = ev.GetColor() };
            var lblT = new Label { Text = ev.Title, Left = 18, Top = 2, Width = strip.Width - 80, Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = Color.FromArgb(40, 40, 40), AutoSize = false, Height = 18, AutoEllipsis = true };
            var lblW = new Label { Text = when, Left = 18, Top = 20, Width = strip.Width - 80, Font = new Font("Segoe UI", 7.5f), ForeColor = Color.Gray, AutoSize = false, Height = 14 };
            var lblD = new Label { Text = date.ToString("MMM dd"), Left = strip.Width - 58, Top = 10, Width = 54, Font = new Font("Segoe UI", 8f), ForeColor = Color.FromArgb(90, 90, 90), AutoSize = false, TextAlign = ContentAlignment.MiddleRight };
            strip.Controls.AddRange(new Control[] { dot, lblT, lblW, lblD });
            return strip;
        }

        private void PositionBottomPanel()
        {
            if (pnlBottom == null || pnlCalendar == null) return;
            pnlBottom.Width = pnlCalendar.ClientSize.Width;
            pnlBottom.Top = pnlCalendar.ClientSize.Height - pnlBottom.Height;
            pnlBottom.Left = 0;

            foreach (Control c in pnlBottom.Controls)
                if (c is Panel rp && rp.Left > 530)
                    rp.Width = pnlBottom.Width - rp.Left - 8;
        }

        private void BuildDayHeaders()
        {
            string[] days = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            pnlDayHeaders = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Height = 32,
                Padding = new Padding(0),
                Margin = new Padding(0),
                BackColor = Color.FromArgb(245, 245, 245),
            };
            pnlDayHeaders.Parent = FPLmonth.Parent;
            pnlDayHeaders.Left = FPLmonth.Left;
            pnlDayHeaders.Width = FPLmonth.Width;
            pnlDayHeaders.Top = FPLmonth.Top - 32;
            pnlDayHeaders.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            foreach (string d in days)
                pnlDayHeaders.Controls.Add(new Label
                {
                    Text = d,
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = false,
                    Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                    ForeColor = d == "Sunday" ? Color.Crimson : Color.FromArgb(80, 80, 80),
                    BackColor = Color.Transparent,
                    Margin = new Padding(1, 0, 1, 0),
                    Height = 32,
                });
            pnlDayHeaders.BringToFront();
        }

        private void AlignDayHeaders()
        {
            if (pnlDayHeaders == null) return;
            pnlDayHeaders.Left = FPLmonth.Left;
            pnlDayHeaders.Width = FPLmonth.Width;
            pnlDayHeaders.Top = FPLmonth.Top - pnlDayHeaders.Height;
            int available = FPLmonth.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 2;
            int cellWidth = available / 7;
            foreach (Control ctrl in pnlDayHeaders.Controls) ctrl.Width = cellWidth;
        }

        private void showDays(int month, int year)
        {
            FPLmonth.Controls.Clear();
            _year = year; _month = month;
            SharedCalendarData.CurrentYear = year;
            SharedCalendarData.CurrentMonth = month;

            string monthName = new DateTimeFormatInfo().GetMonthName(month);
            lblMonthYear.Text = monthName.ToUpper() + " " + year;
            CenterMonthLabel();

            DateTime firstDay = new DateTime(year, month, 1);
            int startDOW = (int)firstDay.DayOfWeek;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            if (startDOW > 0)
            {
                DateTime prev = firstDay.AddMonths(-1);
                int prevDays = DateTime.DaysInMonth(prev.Year, prev.Month);
                for (int i = startDOW - 1; i >= 0; i--)
                {
                    int d = prevDays - i;
                    FPLmonth.Controls.Add(new UrDay(d.ToString(), prev.Year, prev.Month, false, GetHoliday(prev.Year, prev.Month, d), isStudent: false));
                }
            }

            for (int i = 1; i <= daysInMonth; i++)
                FPLmonth.Controls.Add(new UrDay(i.ToString(), year, month, true, GetHoliday(year, month, i), isStudent: false));

            int total = FPLmonth.Controls.Count;
            int remainder = total % 7;
            if (remainder > 0)
            {
                DateTime next = firstDay.AddMonths(1);
                for (int i = 1; i <= 7 - remainder; i++)
                    FPLmonth.Controls.Add(new UrDay(i.ToString(), next.Year, next.Month, false, GetHoliday(next.Year, next.Month, i), isStudent: false));
            }

            ResizeCalendarCells();
        }

        private string GetHoliday(int year, int month, int day)
        {
            var date = new DateTime(year, month, day);
            return SharedCalendarData.Holidays.ContainsKey(date) ? SharedCalendarData.Holidays[date] : "";
        }

        private void ResizeCalendarCells()
        {
            if (FPLmonth.Controls.Count == 0) return;
            int available = FPLmonth.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 2;
            int cellWidth = available / 7;
            FPLmonth.SuspendLayout();
            foreach (Control ctrl in FPLmonth.Controls)
            {
                ctrl.Width = cellWidth;
                ctrl.Height = 110;
                ctrl.Margin = new Padding(1);
            }
            FPLmonth.ResumeLayout();
            AlignDayHeaders();
        }
        private void OnFormResized(object sender, EventArgs e)
        {
            if (!this.IsHandleCreated || this.IsDisposed) return;
            this.BeginInvoke((Action)(() =>
            {
                try { FitCalendarPanel(); ResizeCalendarCells(); CenterMonthLabel(); PositionBottomPanel(); }
                catch { }
            }));
        }

        private void FitCalendarPanel()
        {
            if (pnlCalendar == null || !pnlCalendar.Visible) return;
            if (pnlSidebar == null || pnlHeader == null) return;

            pnlCalendar.Left = pnlSidebar.Width;
            pnlCalendar.Top = pnlHeader.Height;
            pnlCalendar.Width = this.ClientSize.Width - pnlSidebar.Width;
            pnlCalendar.Height = this.ClientSize.Height - pnlHeader.Height;

            const int BOTTOM_H = 220;
            if (FPLmonth != null)
            {
                int headerBottom = pnlDayHeaders != null
                    ? pnlDayHeaders.Top + pnlDayHeaders.Height : FPLmonth.Top;
                FPLmonth.Width = pnlCalendar.ClientSize.Width - FPLmonth.Left - 4;
                FPLmonth.Top = headerBottom;
                FPLmonth.Height = pnlCalendar.ClientSize.Height - FPLmonth.Top - BOTTOM_H - 4;
            }
        }

        private void CenterMonthLabel()
        {
            if (lblMonthYear == null || pnlCalendar == null) return;
            lblMonthYear.AutoSize = false;
            lblMonthYear.TextAlign = ContentAlignment.MiddleCenter;
            lblMonthYear.Width = pnlCalendar.ClientSize.Width;
            lblMonthYear.Left = 0;
        }

        private void picNext_Click(object sender, EventArgs e) { _month++; if (_month > 12) { _month = 1; _year++; } showDays(_month, _year); }
        private void picPrev_Click(object sender, EventArgs e) { _month--; if (_month < 1) { _month = 12; _year--; } showDays(_month, _year); }


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
                    Status = "active"
                });
            }

            // --- RESET AND HIDE ---
            editingAnnouncementId = -1;
            txtAnnTitle.Clear();
            txtAnnDesc.Clear();

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

        private void btnCancelAssign_Click(object sender, EventArgs e)
        {
            // 1. Clear all Text fields
            txtActTitle.Clear();
            textBox22.Clear();

            // 2. Reset Labels and ComboBoxes
            lblFileNameDisplay.Text = "No file selected"; // Or string.Empty
            cmbBXActType.SelectedIndex = -1; // Deselects the current item

            // 3. Reset the DatePicker to current time (Optional but recommended)
            dateTimePicker1.Value = DateTime.Now;

            // 4. Reset internal tracking variables
            currentEditingItem = null;
            tempAttachedPath = string.Empty;

            // 5. Hide the creation panel and return to the main view
            pnlCreateAct.Visible = false;

            // Optional: Refresh the management flow layout to ensure no UI ghosts
            ManageAct.Refresh();
        }

        private void button45_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            button46.Visible = true;
            button47.Visible = true;
        }

        private void buttonRounded1_Click(object sender, EventArgs e)
        {
            pnlSub1.Visible = true;
            pnlSub1.BringToFront();
        }

        private void buttonRounded2_Click(object sender, EventArgs e)
        {
            pnlStudents1.Visible = true;
            pnlStudents1.BringToFront();
        }

        private void buttonRounded7_Click(object sender, EventArgs e)
        {
            pnlSub1.Visible = false;
        }

        private void buttonRounded4_Click(object sender, EventArgs e)
        {
            pnlStudents1.Visible = false;
        }

        private void buttonRounded15_Click(object sender, EventArgs e)
        {
            pnlStudents1.BringToFront();
            pnlGrade.Visible = false;
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            pnlGrade.Visible = true;
            pnlGrade.BringToFront();
        }

        private void buttonRounded16_Click(object sender, EventArgs e)
        {
            lblScore.Text = rtxtScore.Text;
            MessageBox.Show("Score saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // --- QUICK ACTIONS REDIRECTS ---
        private void panel105_Click(object sender, EventArgs e) { if (btnGrades != null) btnGrades_Click(btnGrades, e); }
        private void panel103_Click(object sender, EventArgs e) { if (btnGrades != null) btnGrades_Click(btnGrades, e); }
        private void panel102_Click(object sender, EventArgs e) { if (btnGrades != null) btnGrades_Click(btnGrades, e); }
        private void panel104_Click(object sender, EventArgs e) { if (btnCourses != null) btnCourses_Click(btnCourses, e); }

        private void button2_Click(object sender, EventArgs e) { if (btnGrades != null) btnGrades_Click(btnGrades, e); }
        private void button4_Click(object sender, EventArgs e) { if (btnCourses != null) btnCourses_Click(btnCourses, e); }
        private void button6_Click(object sender, EventArgs e) { if (btnGrades != null) btnGrades_Click(btnGrades, e); }

        // --- SUBMIT FINAL GRADES BUTTON FIX ---
        private void button9_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to submit all final grades? This action cannot be undone.", "Submit Final Grades", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Grades submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // --- DATA MODEL ---
        public class StudentGrade
        {
            public string StudentNumber { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public double? Midterm { get; set; }
            public double? Finals { get; set; }
            public double? Average => (Midterm.HasValue && Finals.HasValue) ? (Midterm + Finals) / 2.0 : null;
            public string Remarks => Average.HasValue ? (Average >= 75.0 ? "Passed" : "Failed") : "Incomplete";
        }
    }
}
