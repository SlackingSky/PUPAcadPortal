using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace PUPAcadPortal
{
    public partial class InstructorPortal : Form
    {
        public static int _year, _month;
        public static Dictionary<DateTime, string> notesDict = new Dictionary<DateTime, string>();
        public static Dictionary<DateTime, string> holidaysDict = new Dictionary<DateTime, string>
        {
            // 2025
            { new DateTime(2025, 1, 1),  "New Year's Day" },
            { new DateTime(2025, 4, 17), "Maundy Thursday" },
            { new DateTime(2025, 4, 18), "Good Friday" },
            { new DateTime(2025, 4, 19), "Black Saturday" },
            { new DateTime(2025, 4, 9),  "Araw ng Kagitingan" },
            { new DateTime(2025, 5, 1),  "Labor Day" },
            { new DateTime(2025, 6, 12), "Independence Day" },
            { new DateTime(2025, 8, 21), "Ninoy Aquino Day" },
            { new DateTime(2025, 8, 25), "National Heroes Day" },
            { new DateTime(2025, 11, 1), "All Saints' Day" },
            { new DateTime(2025, 11, 30),"Bonifacio Day" },
            { new DateTime(2025, 12, 8), "Immaculate Conception" },
            { new DateTime(2025, 12, 25),"Christmas Day" },
            { new DateTime(2025, 12, 30),"Rizal Day" },
            { new DateTime(2025, 12, 31),"New Year's Eve" },
            // 2026
            { new DateTime(2026, 1, 1),  "New Year's Day" },
            { new DateTime(2026, 2, 25), "EDSA Revolution" },
            { new DateTime(2026, 4, 2),  "Maundy Thursday" },
            { new DateTime(2026, 4, 3),  "Good Friday" },
            { new DateTime(2026, 4, 4),  "Black Saturday" },
            { new DateTime(2026, 4, 9),  "Araw ng Kagitingan" },
            { new DateTime(2026, 5, 1),  "Labor Day" },
            { new DateTime(2026, 6, 12), "Independence Day" },
            { new DateTime(2026, 8, 21), "Ninoy Aquino Day" },
            { new DateTime(2026, 8, 31), "National Heroes Day" },
            { new DateTime(2026, 11, 1), "All Saints' Day" },
            { new DateTime(2026, 11, 2), "All Souls' Day" },
            { new DateTime(2026, 11, 30),"Bonifacio Day" },
            { new DateTime(2026, 12, 8), "Immaculate Conception" },
            { new DateTime(2026, 12, 24),"Christmas Eve" },
            { new DateTime(2026, 12, 25),"Christmas Day" },
            { new DateTime(2026, 12, 30),"Rizal Day" },
            { new DateTime(2026, 12, 31),"New Year's Eve" },
        };

        private Button clickedButton;
        private Color defaultColor = Color.Maroon;
        private Color selectedColor = Color.FromArgb(109, 0, 0);

        public InstructorPortal()
        {
            InitializeComponent();
            pnlAttendance.AutoScrollMinSize = new Size(0, 1000);
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/dd/yyyy hh:mm tt";
        }

        private void changeButtonColor(Button button)
        {
            if (clickedButton != null)
                clickedButton.BackColor = defaultColor;

            clickedButton = button;
            pnlYellow.Visible = true;
            pnlYellow.Parent = clickedButton.Parent;
            pnlYellow.BringToFront();
            clickedButton.BackColor = selectedColor;
        }

        private void showContent(Button button)
        {
            Dictionary<Button, Panel> contents = new Dictionary<Button, Panel>();
            contents.Add(btnDashboard, pnlDashboardContent);
            contents.Add(btnGrades, pnlGradesContent);
            contents.Add(btnCourses, pnlCoursesContent);

            foreach (KeyValuePair<Button, Panel> content in contents)
            {
                if (content.Key == clickedButton)
                {
                    content.Value.Location = new Point(pnlSidebar.Size.Width, pnlHeader.Size.Height);
                    content.Value.Visible = true;
                }
                else
                {
                    content.Value.Visible = false;
                }
            }
        }

        private void StudentPortal_Closing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Are you sure you want to exit?", "Confirm Exit",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                    System.Windows.Forms.Application.Exit();
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

        private void pnlCoursesContent_Paint(object sender, PaintEventArgs e) { }

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

        private void label24_Click(object sender, EventArgs e) { }

        bool expand = false;

        private void timer1_Tick(object sender, EventArgs e) { }

        private void StatusBtn_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void CreateAnnounce_Click(object sender, EventArgs e)
        {
            pnlCreateAnnounce.Visible = !pnlCreateAnnounce.Visible;

            if (pnlCreateAnnounce.Visible)
            {
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

        private void Sub1_Paint(object sender, PaintEventArgs e) { }

        bool sidebarExpand;

        private void MenuButton_Click(object sender, EventArgs e)
        {
            sideBarTimer.Start();
        }

        private void btnAssign_Click(object sender, EventArgs e)
        {
            pnlLMSAct.Visible = true;
            pnlLMSAct.BringToFront();
        }

        private void btnClassFiles_Click(object sender, EventArgs e)
        {
            pnlClassFiles.Visible = true;
            pnlClassFiles.BringToFront();
        }

        private void pnlAttendance_Paint(object sender, PaintEventArgs e) { }

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
            string selectedAction = cmbBXActType.SelectedItem?.ToString();

            pnlQuiz1.Visible = false;
            pnlAssign.Visible = false;

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

        private void btnAddPanel_Click(object sender, EventArgs e)
        {
            ucQuestionCard newCard = new ucQuestionCard();
            newCard.Width = 1250;
            newCard.Height = 423;

            flowLayoutPanel3.Controls.Add(newCard);

            int centeredMargin = (flowLayoutPanel3.Width - 1250 - 25) / 2;
            if (centeredMargin < 0) centeredMargin = 57;

            foreach (Control ctrl in flowLayoutPanel3.Controls)
            {
                ctrl.Width = 1250;
                ctrl.Margin = new Padding(centeredMargin, 10, 10, 10);
                ctrl.Left = 0;
            }

            RenumberQuestions();

            if (flowLayoutPanel3.Controls.Contains(pnlControlBar))
                flowLayoutPanel3.Controls.SetChildIndex(pnlControlBar, -1);

            flowLayoutPanel3.ScrollControlIntoView(pnlControlBar);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var lastCard = flowLayoutPanel3.Controls.OfType<ucQuestionCard>().LastOrDefault();

            if (lastCard != null)
            {
                flowLayoutPanel3.Controls.Remove(lastCard);
                lastCard.Dispose();
                RenumberQuestions();
            }
        }

        private void btnSaveQuiz_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Do you want to save this quiz before exiting?",
                "Save Quiz", MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
                this.Close();
            else if (result == DialogResult.No)
                this.Close();
        }

        private void flowLayoutPanel3_Resize(object sender, EventArgs e)
        {
            int newMargin = (flowLayoutPanel3.Width - 1250 - 25) / 2;
            if (newMargin < 0) newMargin = 10;

            foreach (Control ctrl in flowLayoutPanel3.Controls)
                ctrl.Margin = new Padding(newMargin, 10, 10, 10);
        }

        private void RenumberQuestions()
        {
            int count = 1;
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
            string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string path in paths)
            {
                if (Directory.Exists(path))
                    AddFolderToListView(path);
                else
                    AddFileToListView(path);
            }
        }

        private void AddFolderToListView(string folderPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(folderPath);

            if (!imageList1.Images.ContainsKey("FolderIcon"))
                imageList1.Images.Add("FolderIcon", Properties.Resources.folder_icon);

            int iconIndex = imageList1.Images.IndexOfKey("FolderIcon");
            ListViewItem item = new ListViewItem(dirInfo.Name, iconIndex);
            item.Tag = folderPath;
            item.SubItems.Add("File Folder");
            item.SubItems.Add(DateTime.Now.ToString("g"));

            listView_file.Items.Add(item);
        }

        private void AddFileToListView(string filepath)
        {
            FileInfo fileInfo = new FileInfo(filepath);
            string ext = fileInfo.Extension.ToLower();
            long maxFileSize = 20 * 1024 * 1024;

            if (fileInfo.Length <= maxFileSize)
            {
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

                double sizeInKB = fileInfo.Length / 1024.0;
                string sizeDisplay = (sizeInKB >= 1024)
                    ? $"{(sizeInKB / 1024.0):F2} MB"
                    : $"{sizeInKB:F2} KB";

                item.SubItems.Add(sizeDisplay);
                item.SubItems.Add(DateTime.Now.ToString("g"));

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
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void InstructorPortal_Load(object sender, EventArgs e)
        {
            // ListView styling
            listView_file.Font = new System.Drawing.Font("Segoe UI", 11.5f);
            listView_file.View = View.Details;
            listView_file.FullRowSelect = true;
            listView_file.GridLines = false;

            imageList1.ImageSize = new Size(32, 32);
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            listView_file.SmallImageList = imageList1;

            listView_file.Columns.Clear();
            listView_file.Columns.Add("File Name", 250);
            listView_file.Columns.Add("Size", 100);
            listView_file.Columns.Add("Date Uploaded", 180);

            // Calendar responsive resize
            FPLmonth.Resize += (s, ev) => ResizeCalendarCells();

            // Show current month
            showDays(DateTime.Now.Month, DateTime.Now.Year);

            //Mouse-wheel - month navigation
            var wheelFilter = new CalendarWheelFilter(FPLmonth, delta =>
            {
                if (delta > 0)
                    picPrev_Click(this, EventArgs.Empty);  // scroll UP   = previous month
                else
                    picNext_Click(this, EventArgs.Empty);  // scroll DOWN = next month
            });
            System.Windows.Forms.Application.AddMessageFilter(wheelFilter);

            this.FormClosed += (s, ev) => System.Windows.Forms.Application.RemoveMessageFilter(wheelFilter);
        }

        private void listView_file_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView_file.SelectedItems.Count > 0)
            {
                string fullPath = listView_file.SelectedItems[0].Tag.ToString();
                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = fullPath,
                        UseShellExecute = true
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
            if (listView_file.SelectedItems.Count > 0)
            {
                DialogResult result = MessageBox.Show("Remove this file from the list?",
                    "Confirm Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    foreach (ListViewItem item in listView_file.SelectedItems)
                        listView_file.Items.Remove(item);
                }
            }
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView_file.SelectedItems.Count > 0)
            {
                string sourcePath = listView_file.SelectedItems[0].Tag.ToString();
                string fileName = listView_file.SelectedItems[0].Text;

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = fileName;
                saveFileDialog.Filter = "All files (*.*)|*.*";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
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

        private void showDays(int month, int year)
        {
            FPLmonth.Controls.Clear();
            _year = year;
            _month = month;

            string monthName = new DateTimeFormatInfo().GetMonthName(month);
            lblMonthYear.Text = monthName.ToUpper() + " " + year;

            DateTime firstDay = new DateTime(year, month, 1);
            int startDOW = (int)firstDay.DayOfWeek;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            // Leading days from previous month (grayed out)
            if (startDOW > 0)
            {
                DateTime prevMonth = firstDay.AddMonths(-1);
                int prevDays = DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month);
                for (int i = startDOW - 1; i >= 0; i--)
                {
                    int d = prevDays - i;
                    string h = GetHoliday(prevMonth.Year, prevMonth.Month, d);
                    FPLmonth.Controls.Add(new UrDay(d.ToString(), prevMonth.Year, prevMonth.Month, false, h));
                }
            }

            // Current month days
            for (int i = 1; i <= daysInMonth; i++)
            {
                string h = GetHoliday(year, month, i);
                FPLmonth.Controls.Add(new UrDay(i.ToString(), year, month, true, h));
            }

            // Trailing days from next month (grayed out)
            int total = FPLmonth.Controls.Count;
            int remainder = total % 7;
            if (remainder > 0)
            {
                DateTime nextMonth = firstDay.AddMonths(1);
                int trailing = 7 - remainder;
                for (int i = 1; i <= trailing; i++)
                {
                    string h = GetHoliday(nextMonth.Year, nextMonth.Month, i);
                    FPLmonth.Controls.Add(new UrDay(i.ToString(), nextMonth.Year, nextMonth.Month, false, h));
                }
            }

            ResizeCalendarCells();
        }

        private string GetHoliday(int year, int month, int day)
        {
            DateTime date = new DateTime(year, month, day);
            return holidaysDict.ContainsKey(date) ? holidaysDict[date] : "";
        }

        private void listView_file_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var item = listView_file.GetItemAt(e.X, e.Y);
                if (item != null)
                {
                    item.Selected = true;
                    contextMenuStrip1.Show(listView_file, e.Location);
                }
            }
        }

        private void ResizeCalendarCells()
        {
            if (FPLmonth.Controls.Count == 0) return;

            int cols = 7;
            int scrollBarWidth = SystemInformation.VerticalScrollBarWidth;
            int availableWidth = FPLmonth.ClientSize.Width - scrollBarWidth - 2;
            int cellWidth = availableWidth / cols;
            int cellHeight = 110;

            FPLmonth.SuspendLayout();
            foreach (Control ctrl in FPLmonth.Controls)
            {
                ctrl.Width = cellWidth;
                ctrl.Height = cellHeight;
                ctrl.Margin = new Padding(1);
            }
            FPLmonth.ResumeLayout();
        }

        private void picNext_Click(object sender, EventArgs e)
        {
            _month++;
            if (_month > 12) { _month = 1; _year++; }
            showDays(_month, _year);
        }

        private void picPrev_Click(object sender, EventArgs e)
        {
            _month--;
            if (_month < 1) { _month = 12; _year--; }
            showDays(_month, _year);
        }

        private void FPLmonth_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    // Mouse-wheel filter — fires month navigation when hovering the calendar
    public class CalendarWheelFilter : IMessageFilter
    {
        private const int WM_MOUSEWHEEL = 0x020A;
        private readonly Control _watchArea;
        private readonly Action<int> _onScroll;

        public CalendarWheelFilter(Control watchArea, Action<int> onScroll)
        {
            _watchArea = watchArea;
            _onScroll = onScroll;
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg != WM_MOUSEWHEEL) return false;

            Point cursor = Cursor.Position;
            if (!_watchArea.ClientRectangle.Contains(_watchArea.PointToClient(cursor)))
                return false;

            // Positive delta = scrolled up = go to previous month
            int delta = (short)(((int)m.WParam >> 16) & 0xFFFF);
            _onScroll(delta);
            return true; // swallow so the panel itself doesn't scroll
        }
    }
}