using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PUPAcadPortal.Utils;
using PUPAcadPortal.PortalContents.Student.Enrollment;
using PUPAcadPortal.PortalContents.Student.LMS;

namespace PUPAcadPortal.PortalForms
{
    public partial class StudentPortal : Form
    {

        private StudentLMSHost _studentLMSHost;

        private Button clickedButton;
        private Color defaultColor = Color.Maroon;
        private Color selectedColor = Color.FromArgb(109, 0, 0);
        private readonly Dictionary<Control, RectangleF> _origBounds = new();
        private readonly Dictionary<Control, float> _origFontSz = new();


        private UrDay _selectedCell;

        public StudentPortal(Form parent)
        {
            InitializeComponent();
            //this.Resize += StudentPortal_EnrollmentResize; Breaks enrollment bottom cards, instead of being in the middle: Brylle
            pnlContainerStudentPortal.Dock = DockStyle.Fill;
            pnlContainerStudentPortal.AutoScroll = true;
            Form _parentForm = parent;


            this.Load += StudentPortal_Load;

            string[] mrow1 = { "Integrated Programming and Technologies 1", "1.00", "P" };
            string[] mrow2 = { "Principles of Accounting", "1.00", "P" };

            dgvMidtermGradeStudent.Rows.Add(mrow1);
            dgvMidtermGradeStudent.Rows.Add(mrow2);

            string[] frow1 = { "Objected Oriented Programming", "1.00", "P" };
            string[] frow2 = { "PATHFIT 4", "1.00", "P" };

            dgvFinalGradeStudent.Rows.Add(frow1);
            dgvFinalGradeStudent.Rows.Add(frow2);
        }

        private void BuildStudentLMSHost()
        {
            //if (mainContentPanel.Controls.Contains(_studentLMSHost))
            //    return;
            _studentLMSHost = new StudentLMSHost { Dock = DockStyle.Fill };
            //mainContentPanel.Controls.Clear();
            //foreach (Control control in mainContentPanel.Controls)
            //{
            //    control.Dispose();
            //}
            //mainContentPanel.Controls.Add(_studentLMSHost);
        }

        private void changeButtonColor(Button button)
        {
            if (clickedButton != null)
            {
                clickedButton.BackColor = defaultColor;
            }
            clickedButton = button;
            pnlYellow.Visible = true;
            pnlYellow.Parent = clickedButton;
            pnlYellow.BringToFront();
            clickedButton.BackColor = selectedColor;
        }

        //Method na nagpapakita ng content ng bawat button, wala akong maisip na iba kaya eto
        private void showContent(Button button)
        {
            Dictionary<Button, Panel> contents = new Dictionary<Button, Panel>
            {
                { btnAttendance, pnlAttendance  },
                { btnGrade, pnlGrades  },
            };
            //Kada button na aadd, maglagay ng panel sa form at lagay dito
            foreach (KeyValuePair<Button, Panel> content in contents)
            {
                if (content.Key == clickedButton)
                {
                    //Automatic positioning, wag pakialaman maliban nalang kung binago ang position ng sidebar
                    content.Value.Parent = pnlContainerStudentPortal;
                    content.Value.Dock = DockStyle.Fill;
                    content.Value.Visible = true;
                    content.Value.BringToFront();
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
            //else
            //{
            //    if (MessageBox.Show("Are you sure you want to Logout", "Log Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //    {
            //        e.Cancel = true;
            //    }
            //    else
            //        _parentForm.Show();
            //}
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            mainContentPanel.BringToFront();
            mainContentPanel.ShowView(new DashboardContentStudent());
        }

        private void btnEnrollment_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            mainContentPanel.ShowView(new EnrollmentContentStudent());
        }

        private void btnCourses_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            BuildStudentLMSHost();
            mainContentPanel.ShowView(_studentLMSHost);
            // Reset to course dashboard so the student always lands on the course list
            _studentLMSHost.ShowCourseDashboard();
        }

        private void btnAccounts_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            mainContentPanel.ShowView(new AccountsContentStudent());
        }
        private void btnLMS_Click(object sender, EventArgs e)
        {
            pnllmsSubmenu.Visible = !pnllmsSubmenu.Visible;
            if (pnllmsSubmenu.Visible)
                btnLMS.Text = " LMS                                       ⌄";
            else
                btnLMS.Text = " LMS                                        ›";
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnAnnounce_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            mainContentPanel.ShowView(new AnnouncementContentStudent());
        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            mainContentPanel.ShowView(new CalendarContentStudent());
        }

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnGrade_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            pnlSubject.BringToFront();
            pnlSubject.Visible = true;
        }

        private void btnStudFiles_Click(object sender, EventArgs e)
        {

            pnlLMSFiles.BringToFront();
            pnlLMSFiles.Visible = true;
        }

        private void btnStudAct_Click(object sender, EventArgs e)
        {
            pnlLMSFiles.Visible = false;
            pnlLMSActivities.BringToFront();
            pnlLMSActivities.Visible = true;
        }

        private void pnlAct1_MouseEnter(object sender, EventArgs e)
        {
            // Change to a light highlight color (e.g., Light Gray or Light Maroon)
            pnlAct1.BackColor = Color.FromArgb(128, 0, 0);
            pnlAct1.Cursor = Cursors.Hand; // Shows the clicking hand icon
        }

        private void pnlAct1_MouseLeave(object sender, EventArgs e)
        {
            // Change back to the original background color (usually White)
            pnlAct1.BackColor = Color.White;
        }

        private void pnlAct1_Click(object sender, EventArgs e)
        {
            // 1. Hide the panel that contains the question/choices
            // Replace 'pnlQuestionArea' with the actual parent container name
            pnlAnsAct1.Visible = false;

            // 2. Show the Answer Action panel
            pnlAnsAct1.Visible = true;
            pnlAnsAct1.BringToFront();
            pnlAnsAct1.Dock = DockStyle.Fill; // Optional: ensures it fills the space
        }

        private void btnCancelAct_Click(object sender, EventArgs e)
        {
            pnlLMSActivities.BringToFront();
            pnlLMSActivities.Visible = true;
        }

        private void btnCancelAct_Click_1(object sender, EventArgs e)
        {
            pnlLMSActivities.BringToFront();
            pnlLMSActivities.Visible = true;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            pnlLMSActivities.BringToFront();
            pnlLMSActivities.Visible = true;
        }

        private void btnAssignAttach_Click(object sender, EventArgs e)
        {
            pnlAttachAss.BringToFront();
            pnlAttachAss.Visible = true;
        }

        private void btnCancelAssign_Click(object sender, EventArgs e)
        {
            pnlLMSActivities.BringToFront(); pnlLMSActivities.Visible = true;
        }

        private void btnSaveAss_Click(object sender, EventArgs e)
        {
            pnlLMSActivities.BringToFront(); pnlLMSActivities.Visible = true;
        }

        private void btnAttachCancel_Click(object sender, EventArgs e)
        {
            pnlAttachAss.Hide();
        }

        private void btnDoneAttach_Click(object sender, EventArgs e)
        {
            pnlAttachAss.Hide();
        }

        private void pnlAss1_MouseEnter(object sender, EventArgs e)
        {
            // Highlights to Maroon on hover
            pnlAss1.BackColor = Color.Maroon;
            pnlAss1.Cursor = Cursors.Hand;
        }

        private void pnlAss1_MouseLeave(object sender, EventArgs e)
        {
            // Changes back to White when the mouse leaves
            pnlAss1.BackColor = Color.White;
        }

        private void pnlAss1_Click(object sender, EventArgs e)
        {
            // 1. Immediately reset the color to White
            pnlAss1.BackColor = Color.White;

            // 2. Switch to the Answer panel
            pnlAnsAss.Visible = true;
            pnlAnsAss.BringToFront();

            // Optional: If pnlAss1 is inside a container that you hide:
            // pnlMainContainer.Visible = false;
        }

        private void roundedPanel14_MouseLeave(object sender, EventArgs e)// ung pangatlong panel sa activities ng courses
        {
            roundedPanel14.BackColor = Color.White;
        }

        private void roundedPanel14_MouseEnter(object sender, EventArgs e)// ung pangatlong panel sa activities ng courses
        {
            roundedPanel14.BackColor = Color.Maroon;
            roundedPanel14.Cursor = Cursors.Hand;
        }

        private void roundedPanel16_MouseEnter(object sender, EventArgs e)
        {
            roundedPanel16.BackColor = Color.Maroon;
            roundedPanel16.Cursor = Cursors.Hand;
        }

        private void roundedPanel16_MouseLeave(object sender, EventArgs e)
        {
            roundedPanel16.BackColor = Color.White;
        }

        private void btnGo1_Click_1(object sender, EventArgs e)
        {
            pnlLMSFiles.Visible = false;
            pnlSubMenu.BringToFront();
            pnlSubMenu.Visible = true;
            pnlLMSActivities.BringToFront();
            pnlLMSActivities.Visible = true;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            //Define the path to the source file in the application's local "Resources" folder
            string sourceFile = Path.Combine(Application.StartupPath, "Resources", "COG-MTECH.pdf");

            //Check if the source file exists
            if (!File.Exists(sourceFile))
            {
                MessageBox.Show("Error: COG-MTECH.pdf was not found in the Resources folder.",
                                "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //Initialize the SaveFileDialog to allow user to select the save location and filename
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                //Set file type filters, default filename and dialog title
                sfd.Filter = "PDF Documents (.pdf)|.pdf";
                sfd.FileName = "COG-MTECH.pdf";
                sfd.Title = "Save COG-MTECH Report";

                //Opens dialog and check if user clicked "Save"
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        //Perform the file copy operation to the user's specified location
                        //The parameter "true" allows overwriting existing files
                        File.Copy(sourceFile, sfd.FileName, true);

                        MessageBox.Show("COG-MTECH.pdf has been saved successfully!",
                                        "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            rpnlGradeBreakdown.Visible = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            rpnlGradeBreakdown.Visible = true;
            rpnlGradeBreakdown.BringToFront();
        }

        private void btnViewGrades_Click(object sender, EventArgs e)
        {

        }


        private void btnThisWeek_Click(object sender, EventArgs e)
        {
            pnlTW.BringToFront();
            pnlTW.Visible = true;
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            pnlToday.BringToFront();
            pnlToday.Visible = true;
        }

        private void btnNextWeek_Click(object sender, EventArgs e)
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

        private void btnAll_Click(object sender, EventArgs e)
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

        private void StudentPortal_Load(object sender, EventArgs e)
        {
            btnDashboard.PerformClick();
        }

        private void ScaleControls(Control.ControlCollection controls, float rx, float ry)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl.Tag is string t && t.Contains("noScale")) continue;
                if (!_origBounds.TryGetValue(ctrl, out RectangleF ob)) continue;

                int newX = R(ob.X * rx);
                int newY = R(ob.Y * ry);
                int newW = Math.Max(1, R(ob.Width * rx));
                int newH = Math.Max(1, R(ob.Height * ry));

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
                        try { ctrl.Font = new Font(ctrl.Font.FontFamily, newSz, ctrl.Font.Style, GraphicsUnit.Point); }
                        catch { }
                    }
                }

                if (ctrl.HasChildren) ScaleControls(ctrl.Controls, rx, ry);
            }
        }

        private static int R(float v) => (int)Math.Round(v);

        
        

 

        

        private void cmbbxCourseSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the currently selected text
            string selectedCourse = cmbbxCourseSelection.SelectedItem.ToString();

            // Reset visibility for all panels first (clean slate)
            pnlAttIntro.Visible = false;
            pnlAttAcc.Visible = false;

            if (selectedCourse == "Introduction to Programming")
            {
                pnlAttIntro.Visible = true;

            }
            else if (selectedCourse == "Principles of Accounting")
            {
                pnlAttAcc.Visible = true;

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvMidtermGradeStudent.Columns[e.ColumnIndex].Name == "GradeBreakdown")
            {
                rpnlGradeBreakdown.Visible = true;
                rpnlGradeBreakdown.BringToFront();
            }
        }

        private void cmbGradingPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlMidtermGradeStudent.Visible = false;
            pnlFinalGradeStudent.Visible = false;

            switch (cmbGradingPeriod.SelectedIndex)
            {
                case 0:
                    pnlMidtermGradeStudent.Visible = true;
                    label9.Visible = true;
                    break;
                case 1:
                    pnlFinalGradeStudent.Visible = true;
                    label9.Visible = true;
                    break;
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void dgvFinalGradeStudent_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvFinalGradeStudent.Columns[e.ColumnIndex].Name == "FGradeBreakdown")
            {
                rpnlGradeBreakdown.Visible = true;
                rpnlGradeBreakdown.BringToFront();
            }
        }
    }
}
