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
using PUPAcadPortal.Events;

namespace PUPAcadPortal.PortalForms
{
    public partial class StudentPortal : Form
    {
        private StudentLMSHost _studentLMSHost;
        private SubmenuAnim _submenuAnim;

        public StudentPortal()
        {
            InitializeComponent();
            _submenuAnim = new SubmenuAnim(pnllmsSubmenu, pnllmsSubmenu.Height);
            pnlContainerStudentPortal.Dock = DockStyle.Fill;
            pnlContainerStudentPortal.AutoScroll = true;


            this.Load += StudentPortal_Load;
            this.FormClosing += CloseApp.Form_Closing;
            btnLogout.Click += LogOut.LogOut_Click;
            QuickActionClickEvent.QuickActionClicked += SQuickActionClicked;
            this.Disposed += StudentPortal_Disposed;
        }

        private void StudentPortal_Disposed(object? sender, EventArgs e)
        {
            QuickActionClickEvent.QuickActionClicked -= SQuickActionClicked;
        }

        private async void SQuickActionClicked(object sender, EventArgs e)
        {
            Panel? panel = (Panel)sender;
            string[] panelNames = 
            {
                "pnlQACalendar",
                "pnlQACourses",
                "pnlQAGrades",
            };

            if (!pnllmsSubmenu.Visible && panelNames.Contains(panel.Name))
                btnLMS.PerformClick();
            Button? targetButton = panel.Name switch
            {
                "pnlQACalendar" => btnCalendar,
                "pnlQACourses" => btnCourses,
                "pnlQAGrades" => btnGrade,
                "pnlQAPaymentStatus" => btnAccounts,
                "pnlQAViewEnrollment" => btnEnrollment,
                _ => null
            };

            targetButton.PerformClick();
        }

        private void BuildStudentLMSHost()
        {
            _studentLMSHost = new StudentLMSHost { Dock = DockStyle.Fill };
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            mainContentPanel.BringToFront();
            mainContentPanel.ShowView(new DashboardContentStudent());
        }

        private void btnEnrollment_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new EnrollmentContentStudent());
        }

        private void btnCourses_Click(object sender, EventArgs e)
        {
            BuildStudentLMSHost();
            mainContentPanel.ShowView(_studentLMSHost);
            // Reset to course dashboard so the student always lands on the course list
            _studentLMSHost.ShowCourseDashboard();
        }

        private void btnAccounts_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new AccountsContentStudent());
        }
        private async void btnLMS_Click(object sender, EventArgs e)
        {
            btnLMS.Text = !pnllmsSubmenu.Visible ? " LMS                                       ⌄" : " LMS                                        ›";
            await _submenuAnim.ToggleSubMenuAsync();
        }

        private void btnAnnounce_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new AnnouncementContentStudent());
        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new CalendarContentStudent());
        }

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new AttendanceContentStudent());
        }

        private void btnGrade_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new GradesContentStudent());
        }

        private void StudentPortal_Load(object sender, EventArgs e)
        {
            ButtonInteraction buttonInteraction = new ButtonInteraction();
            buttonInteraction.InitializeMyPanelEvents(flowLayoutPanel1);
            btnDashboard.PerformClick();
        }
    }
}
