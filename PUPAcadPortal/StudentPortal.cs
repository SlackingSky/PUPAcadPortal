using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class StudentPortal : Form
    {
        public static int _year, _month;
        private FlowLayoutPanel pnlDayHeaders;

        private Button clickedButton;
        private Color defaultColor = Color.Maroon;
        private Color selectedColor = Color.FromArgb(109, 0, 0);

        public StudentPortal()
        {
            InitializeComponent();
            // Build dynamic day-header row
            BuildDayHeaders();

            // Calendar resize hooks
            FPLmonth.Resize += (s, ev) =>
            {
                ResizeCalendarCells();
                AlignDayHeaders();
            };

            this.Resize += OnFormResized;

            // Show current month 
            showDays(DateTime.Now.Month, DateTime.Now.Year);

            // Mouse-wheel → month navigation
            var wheelFilter = new CalendarWheelFilter(FPLmonth, delta =>
            {
                if (delta > 0) picPrev_Click(this, EventArgs.Empty);
                else picNext_Click(this, EventArgs.Empty);
            });
            Application.AddMessageFilter(wheelFilter);

            this.FormClosed += (s, ev) =>
            {
                Application.RemoveMessageFilter(wheelFilter);
                this.Resize -= OnFormResized;
            };
        }

        //Sidebar helpers (unchanged)
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
            contents.Add(btnEnrollment, pnlEnrollContent);
            contents.Add(btnCourses, pnlCoursesContent);
            contents.Add(btnAccounts, pnlAccountsContent);

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
                    e.Cancel = true;
                else
                    Application.Exit();
            }
        }

        // Form Load — wire up calendar
        private void StudentPortal_Load(object sender, EventArgs e)
        {

        }

        // Build dynamic Sun–Sat header row 
        private void BuildDayHeaders()
        {
            string[] days = { "Sunday","Monday","Tuesday","Wednesday",
                               "Thursday","Friday","Saturday" };

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
            {
                pnlDayHeaders.Controls.Add(new Label
                {
                    Text = d,
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = false,
                    Font = new Font("Segoe UI", 9f),
                    ForeColor = (d == "Sunday") ? Color.Crimson : Color.FromArgb(80, 80, 80),
                    BackColor = Color.Transparent,
                    Margin = new Padding(1, 0, 1, 0),
                    Height = 32,
                });
            }

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

            foreach (Control ctrl in pnlDayHeaders.Controls)
                ctrl.Width = cellWidth;
        }

        // Show days in the calendar
        private void showDays(int month, int year)
        {
            FPLmonth.Controls.Clear();
            _year = year;
            _month = month;

            //  Keep shared state in sync 
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
                    string h = GetHoliday(prev.Year, prev.Month, d);
                    FPLmonth.Controls.Add(
                        new UrDay(d.ToString(), prev.Year, prev.Month, false, h, isStudent: true));
                }
            }

            for (int i = 1; i <= daysInMonth; i++)
            {
                string h = GetHoliday(year, month, i);
                FPLmonth.Controls.Add(
                    new UrDay(i.ToString(), year, month, true, h, isStudent: true));
            }

            int total = FPLmonth.Controls.Count;
            int remainder = total % 7;
            if (remainder > 0)
            {
                DateTime next = firstDay.AddMonths(1);
                for (int i = 1; i <= 7 - remainder; i++)
                {
                    string h = GetHoliday(next.Year, next.Month, i);
                    FPLmonth.Controls.Add(
                        new UrDay(i.ToString(), next.Year, next.Month, false, h, isStudent: true));
                }
            }

            ResizeCalendarCells();
        }
        private void OnFormResized(object sender, EventArgs e)
        {
            if (!this.IsHandleCreated || this.IsDisposed) return;

            this.BeginInvoke((Action)(() =>
            {
                try
                {
                    FitCalendarPanel();
                    ResizeCalendarCells();
                    CenterMonthLabel();
                }
                catch { }
            }));
        }

        private string GetHoliday(int year, int month, int day)
        {
            DateTime date = new DateTime(year, month, day);
            return SharedCalendarData.Holidays.ContainsKey(date)
                ? SharedCalendarData.Holidays[date] : "";
        }

        private void ResizeCalendarCells()
        {
            if (FPLmonth.Controls.Count == 0) return;

            int available = FPLmonth.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 2;
            int cellWidth = available / 7;
            int cellHeight = 110;

            FPLmonth.SuspendLayout();
            foreach (Control ctrl in FPLmonth.Controls)
            {
                ctrl.Width = cellWidth;
                ctrl.Height = cellHeight;
                ctrl.Margin = new Padding(1);
            }
            FPLmonth.ResumeLayout();

            AlignDayHeaders();
        }



        private void FitCalendarPanel()
        {
            if (pnlCalendar == null || !pnlCalendar.Visible) return;
            if (pnlSidebar == null || pnlHeader == null) return;

            pnlCalendar.Left = pnlSidebar.Width;
            pnlCalendar.Top = pnlHeader.Height;
            pnlCalendar.Width = this.ClientSize.Width - pnlSidebar.Width;
            pnlCalendar.Height = this.ClientSize.Height - pnlHeader.Height;

            if (FPLmonth != null)
            {
                int headerBottom = (pnlDayHeaders != null)
                    ? pnlDayHeaders.Top + pnlDayHeaders.Height
                    : FPLmonth.Top;

                FPLmonth.Width = pnlCalendar.ClientSize.Width - FPLmonth.Left - 4;
                FPLmonth.Top = headerBottom;
                FPLmonth.Height = pnlCalendar.ClientSize.Height - FPLmonth.Top - 4;
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

        // Navigation arrows 
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

        // All original button handlers below (unchanged) 
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button); showContent(clickedButton);
        }
        private void btnEnrollment_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button); showContent(clickedButton);
        }
        private void btnCourses_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button); showContent(clickedButton);
        }
        private void btnAccounts_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button); showContent(clickedButton);
        }
        private void btnLMS_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
            pnllmsSubmenu.Visible = !pnllmsSubmenu.Visible;
            btnLMS.Text = pnllmsSubmenu.Visible
                ? " LMS                                       ⌄"
                : " LMS                                        ›";
        }
        private void btnLogout_Click(object sender, EventArgs e) => this.Close();

        private void btnAnnounce_Click(object sender, EventArgs e)
        {
            pnlAnnounce.BringToFront(); pnlAnnounce.Visible = true;
        }
        private void btnCalendar_Click(object sender, EventArgs e)
        {
            pnlSubject.Visible = false;
            pnlCalendar.BringToFront();
            pnlCalendar.Visible = true;
        }
        private void btnSubject_Click(object sender, EventArgs e)
        {
            pnlSubject.BringToFront(); pnlSubject.Visible = true;
        }
        private void btnActivities_Click(object sender, EventArgs e)
        {
            pnlActivities.BringToFront(); pnlActivities.Visible = true;
        }
        private void btnAttendance_Click(object sender, EventArgs e)
        {
            pnlAttendance.BringToFront(); pnlAttendance.Visible = true;
        }
        private void btnGrade_Click(object sender, EventArgs e)
        {
            pnlGrades.BringToFront(); pnlGrades.Visible = true;
        }
        private void btnGo1_Click(object sender, EventArgs e)
        {
            pnlLMSFiles.Visible = false;
            pnlSubMenu.BringToFront(); pnlSubMenu.Visible = true;
            pnlLMSActivities.BringToFront(); pnlLMSActivities.Visible = true;
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            pnlSubject.BringToFront(); pnlSubject.Visible = true;
        }
        private void btnStudFiles_Click(object sender, EventArgs e)
        {
            pnlLMSFiles.BringToFront(); pnlLMSFiles.Visible = true;
        }
        private void btnStudAct_Click(object sender, EventArgs e)
        {
            pnlLMSFiles.Visible = false;
            pnlLMSActivities.BringToFront(); pnlLMSActivities.Visible = true;
        }
        private void pnlAct1_MouseEnter(object sender, EventArgs e)
        {
            pnlAct1.BackColor = Color.FromArgb(128, 0, 0);
            pnlAct1.Cursor = Cursors.Hand;
        }
        private void pnlAct1_MouseLeave(object sender, EventArgs e)
        {
            pnlAct1.BackColor = Color.White;
        }
        private void pnlAct1_Click(object sender, EventArgs e)
        {
            pnlAnsAct1.Visible = true;
            pnlAnsAct1.BringToFront();
            pnlAnsAct1.Dock = DockStyle.Fill;
        }
        private void btnCancelAct_Click(object sender, EventArgs e)
        {
            pnlLMSActivities.BringToFront(); pnlLMSActivities.Visible = true;
        }
        private void btnCancelAct_Click_1(object sender, EventArgs e)
        {
            pnlLMSActivities.BringToFront(); pnlLMSActivities.Visible = true;
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            pnlLMSActivities.BringToFront(); pnlLMSActivities.Visible = true;
        }
        private void btnAssignAttach_Click(object sender, EventArgs e)
        {
            pnlAttachAss.BringToFront(); pnlAttachAss.Visible = true;
        }
        private void btnCancelAssign_Click(object sender, EventArgs e)
        {
            pnlLMSActivities.BringToFront(); pnlLMSActivities.Visible = true;
        }
        private void btnSaveAss_Click(object sender, EventArgs e)
        {
            pnlLMSActivities.BringToFront(); pnlLMSActivities.Visible = true;
        }
        private void btnAttachCancel_Click(object sender, EventArgs e) => pnlAttachAss.Hide();
        private void btnDoneAttach_Click(object sender, EventArgs e) => pnlAttachAss.Hide();
        private void pnlAss1_MouseEnter(object sender, EventArgs e)
        {
            pnlAss1.BackColor = Color.Maroon; pnlAss1.Cursor = Cursors.Hand;
        }
        private void pnlAss1_MouseLeave(object sender, EventArgs e) => pnlAss1.BackColor = Color.White;
        private void pnlAss1_Click(object sender, EventArgs e)
        {
            pnlAss1.BackColor = Color.White;
            pnlAnsAss.Visible = true; pnlAnsAss.BringToFront();
        }
        private void roundedPanel14_MouseLeave(object sender, EventArgs e) => roundedPanel14.BackColor = Color.White;
        private void roundedPanel14_MouseEnter(object sender, EventArgs e)
        {
            roundedPanel14.BackColor = Color.Maroon; roundedPanel14.Cursor = Cursors.Hand;
        }
        private void roundedPanel16_MouseEnter(object sender, EventArgs e)
        {
            roundedPanel16.BackColor = Color.Maroon; roundedPanel16.Cursor = Cursors.Hand;
        }
        private void roundedPanel16_MouseLeave(object sender, EventArgs e) => roundedPanel16.BackColor = Color.White;
    }
}