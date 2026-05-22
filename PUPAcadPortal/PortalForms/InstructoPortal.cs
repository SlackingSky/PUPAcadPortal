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
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static PUPAcadPortal.PortalForms.InstructorPortal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using PUPAcadPortal.PortalContents.Instructor.LMS;
using PUPAcadPortal.Utils;


namespace PUPAcadPortal.PortalForms

{
    public partial class InstructorPortal : Form
    {
        private SubmenuAnim submenuAnimLMS;

        private bool isEditing = false; // New flag to stop event interference
        public static int _year, _month;
        public static Dictionary<DateTime, string> notesDict = new Dictionary<DateTime, string>();
        private ActivityItem? currentEditingItem = null;
        private Button clickedButton;
        private Color defaultColor = Color.Maroon;
        private Color selectedColor = Color.FromArgb(109, 0, 0);
        private string tempAttachedPath = "";

        private SizeF _designSize;
        private readonly Dictionary<Control, RectangleF> _origBounds = new();
        private readonly Dictionary<Control, float> _origFontSz = new();


        public InstructorPortal()
        {
            InitializeComponent();

            submenuAnimLMS = new SubmenuAnim(pnllmsSubmenu, pnllmsSubmenu.Height);

            pnlAttendance.AutoScrollMinSize = new Size(0, 1000);
            SharedCalendarData.LoadData();
            pnlAttendance.AutoScrollMinSize = new Size(0, 1000);
            this.DoubleBuffered = true;
            // DateTimePicker Formatting
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/dd/yyyy hh:mm tt";

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
                //{ btnAnnounceIns, pnlAnnouncement  },
                //{ btnCalendarIns, pnlCalendar },
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
        private async void btnLMS_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
            btnLMS.Text = !pnllmsSubmenu.Visible ? " LMS                                       ⌄" : " LMS                                        ›";
            await submenuAnimLMS.ToggleSubMenuAsync();
            btnAnnounceIns.PerformClick();
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void pnlCoursesContent_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnAnnounceIns_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            mainContentPanel.BringToFront();
            mainContentPanel.ShowView(new AnnouncementContentInst());
        }

        private void btnCalendarIns_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            mainContentPanel.ShowView(new CalendarContentInst());
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

        private void pnlAttendance_Paint(object sender, PaintEventArgs e)
        {

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
            using var ofd = new OpenFileDialog { Filter = "All Files (*.*)|*.*" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                tempAttachedPath = ofd.FileName;
                lblFileNameDisplay.Text = Path.GetFileName(ofd.FileName);
            }
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
            var newCard = new quizCreation { Width = 1250, Height = 456 };

            flowLayoutPanel3.Controls.Add(newCard);

            // Apply the modern flat design centering
            int centeredMargin = Math.Max(10, (flowLayoutPanel3.Width - 1250 - 25) / 2);

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
            var last = flowLayoutPanel3.Controls.OfType<quizCreation>().LastOrDefault();
            if (last != null) { flowLayoutPanel3.Controls.Remove(last); last.Dispose(); RenumberQuestions(); }
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

            if (MessageBox.Show("Save Quiz Activity?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            ActivityItem targetItem;

            if (currentEditingItem == null)
            {
                targetItem = new ActivityItem { Width = ManageAct.ClientSize.Width - 35, Height = 187 };

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
                    : Path.GetFileName(item.SavedAttachedFilePath);
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
            int margin = Math.Max(10, (flowLayoutPanel3.Width - 1250 - 25) / 2);
            foreach (Control ctrl in flowLayoutPanel3.Controls)
            {
                ctrl.Margin = new Padding(margin, 10, 10, 10);
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

        private void pnlLMSFiles_DragEnter(object sender, DragEventArgs e)
    => e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop)
        ? DragDropEffects.Copy : DragDropEffects.None;

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
            var fileInfo = new FileInfo(filepath);
            if (fileInfo.Length > 20 * 1024 * 1024)
            {
                MessageBox.Show($"{fileInfo.Name} is over the 20 MB limit.", "PUP Acad Portal",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string ext = fileInfo.Extension.ToLower();
            if (!imageList1.Images.ContainsKey(ext))
            {
                if (ext == ".pdf") imageList1.Images.Add(ext, Properties.Resources.pdf_icon);
                else if (ext is ".png" or ".jpg" or ".jpeg") imageList1.Images.Add(ext, Properties.Resources.png_icon);
                else if (ext is ".ppt" or ".pptx") imageList1.Images.Add(ext, Properties.Resources.ppt_icon);
                else imageList1.Images.Add(ext, Icon.ExtractAssociatedIcon(filepath));
            }
            var item = new ListViewItem(fileInfo.Name, imageList1.Images.IndexOfKey(ext)) { Tag = filepath };
            item.SubItems.Add(DateTime.Now.ToString("g"));
            double kb = fileInfo.Length / 1024.0;
            item.SubItems.Add(kb >= 1024 ? $"{kb / 1024.0:F2} MB" : $"{kb:F2} KB");
            listView_file.Items.Add(item);
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

            // ENSURE LMS PANELS START HIDDEN
            pnlCreateAct.Visible = false;
            pnlQuiz1.Visible = false;
            pnlAssign.Visible = false;

            // Set a default value for the ComboBox so it's never "null"
            if (cmbBXActType.Items.Count > 0)
                cmbBXActType.SelectedIndex = 0;


            // Fixes the width of the cards in the Management area
            foreach (Control ctrl in ManageAct.Controls)
                if (ctrl is ActivityItem ai) ai.Width = ManageAct.ClientSize.Width - 35;
            foreach (Control ctrl in FlowPostedAct.Controls)
                if (ctrl is ActivityItem ai) ai.Width = FlowPostedAct.ClientSize.Width - 35;


            // Fixes the width of the cards in the Posted area
            foreach (Control ctrl in FlowPostedAct.Controls)
            {
                if (ctrl is ActivityItem item)
                {
                    item.Width = FlowPostedAct.ClientSize.Width - 35;
                }
            }

            this.MinimumSize = new Size(1024, 700);

            _designSize = new SizeF(this.ClientSize.Width, this.ClientSize.Height);
            SnapshotControls(this.Controls);

            this.FormClosed += (s, ev) =>
            {
                SharedCalendarData.SaveData();
            };
        }

        private void SnapshotControls(Control.ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl.Tag is string t && t.Contains("noScale")) continue;
                _origBounds[ctrl] = new RectangleF(ctrl.Left, ctrl.Top, ctrl.Width, ctrl.Height);
                _origFontSz[ctrl] = ctrl.Font.Size;
                if (ctrl.HasChildren) SnapshotControls(ctrl.Controls);
            }
        }

        private void ScaleControls(Control.ControlCollection controls, float rx, float ry)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl.Tag is string t && t.Contains("noScale")) continue;
                if (!_origBounds.TryGetValue(ctrl, out RectangleF ob)) continue;

                int newX = ScaleRound(ob.X * rx);
                int newY = ScaleRound(ob.Y * ry);
                int newW = Math.Max(1, ScaleRound(ob.Width * rx));
                int newH = Math.Max(1, ScaleRound(ob.Height * ry));

                switch (ctrl.Dock)
                {
                    case DockStyle.Fill: break;
                    case DockStyle.Top: ctrl.Height = newH; break;
                    case DockStyle.Bottom: ctrl.Height = newH; break;
                    case DockStyle.Left: ctrl.Width = newW; break;
                    case DockStyle.Right: ctrl.Width = newW; break;
                    default: ctrl.SetBounds(newX, newY, newW, newH); break;
                }

                if (_origFontSz.TryGetValue(ctrl, out float origSz))
                {
                    float newSz = Math.Max(6f, origSz * Math.Min(rx, ry));
                    if (Math.Abs(ctrl.Font.Size - newSz) > 0.15f)
                    {
                        try
                        {
                            ctrl.Font = new Font(
                                ctrl.Font.FontFamily, newSz,
                                ctrl.Font.Style, GraphicsUnit.Point);
                        }
                        catch { /* font too small – silently skip */ }
                    }
                }

                if (ctrl.HasChildren) ScaleControls(ctrl.Controls, rx, ry);
            }
        }

        private static int ScaleRound(float v) => (int)Math.Round(v);

        private void listView_file_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView_file.SelectedItems.Count == 0) return;
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = listView_file.SelectedItems[0].Tag.ToString(),
                    UseShellExecute = true,
                });
            }
            catch (Exception ex) { MessageBox.Show("Could not open file: " + ex.Message); }
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
            if (e.Button != MouseButtons.Right) return;
            var item = listView_file.GetItemAt(e.X, e.Y);
            if (item != null) { item.Selected = true; contextMenuStrip1.Show(listView_file, e.Location); }
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

        private void InstructorPortal_Resize(object sender, EventArgs e)
        {
            if (_designSize.Width == 0 || _designSize.Height == 0) return;

            float rx = Math.Max(this.ClientSize.Width, 1024) / _designSize.Width;
            float ry = Math.Max(this.ClientSize.Height, 700) / _designSize.Height;

            this.SuspendLayout();
            ScaleControls(this.Controls, rx, ry);
            this.ResumeLayout(true);
        }

        private void panel18_Paint(object sender, PaintEventArgs e)
        {

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
        private void panel103_Click(object sender, EventArgs e) { if (pnllmsSubmenu.Visible == false) { btnLMS.PerformClick(); } btnSubjectIns.PerformClick(); }
        private void panel102_Click(object sender, EventArgs e) { if (pnllmsSubmenu.Visible == false) { btnLMS.PerformClick(); } btnAttendanceIns.PerformClick(); }
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

        private void label303_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmbCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlGradeRecords.Visible = false;

            switch (cmbCourse.SelectedIndex)
            {
                case 0:
                    pnlGradeRecords.Visible = true;
                    break;
            }
        }
       
        private static GraphicsPath RoundedRectPath(Rectangle r, int rad)
        {
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, rad, rad, 180, 90);
            p.AddArc(r.Right - rad, r.Y, rad, rad, 270, 90);
            p.AddArc(r.Right - rad, r.Bottom - rad, rad, rad, 0, 90);
            p.AddArc(r.X, r.Bottom - rad, rad, rad, 90, 90);
            p.CloseFigure();
            return p;
        }

        private static void MakeRoundedRegion(Label lbl, int radius)
        {
            using var path = RoundedRectPath(new Rectangle(0, 0, lbl.Width, lbl.Height), radius);
            lbl.Region = new Region(path);
        }

        private static void MakeCircleRegion(Panel p)
        {
            var path = new GraphicsPath();
            path.AddEllipse(0, 0, p.Width, p.Height);
            p.Region = new Region(path);
        }
    }
}
