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
            mainContentPanel.ShowView(new GradesContentStudent());
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
    }
}
