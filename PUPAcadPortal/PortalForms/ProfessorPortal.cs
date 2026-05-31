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
using static PUPAcadPortal.PortalForms.ProfessorPortal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using PUPAcadPortal.PortalContents.Instructor.Enrollment;
using PUPAcadPortal.PortalContents.Instructor.LMS;
using PUPAcadPortal.Utils;
using System.Runtime.InteropServices.Marshalling;
using PUPAcadPortal.Events;
using PUPAcadPortal.PortalContents.Student.LMS.Calendar;


namespace PUPAcadPortal.PortalForms

{
    public partial class ProfessorPortal : Form
    {
        private SubmenuAnim submenuAnimLMS;

        private Panel pnlLMSActivitiesHost;
        private LMSActivityHost lmsHost;


        public ProfessorPortal()
        {
            InitializeComponent();
            submenuAnimLMS = new SubmenuAnim(fpnlLMSSubmenu, fpnlLMSSubmenu.Height);
            this.FormClosing += CloseApp.Form_Closing;
            btnLogout.Click += LogOut.LogOut_Click;
            this.Disposed += InstructorPortal_Disposed;
            QuickActionClickEvent.QuickActionClicked += DashboardContentInst_QuickActionClicked;

            SharedCalendarData.LoadData();
            this.DoubleBuffered = true;

            if (panel3 != null)
            {
                panel3.Dock = DockStyle.Fill;
            }
            BuildLMSActivitiesPanel();
        }

        private void InstructorPortal_Disposed(object? sender, EventArgs e)
        {
            QuickActionClickEvent.QuickActionClicked -= DashboardContentInst_QuickActionClicked;
        }

        private void BuildLMSActivitiesPanel()
        {
            pnlLMSActivitiesHost = new Panel
            {
                Name = "pnlLMSActivitiesHost",
                Dock = DockStyle.Fill,
                Visible = false,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            lmsHost = new LMSActivityHost { Dock = DockStyle.Fill };
            pnlLMSActivitiesHost.Controls.Add(lmsHost);

            mainContentPanel.Controls.Add(pnlLMSActivitiesHost);
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new DashboardContentInst());
        }

        private async void DashboardContentInst_QuickActionClicked(object? sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel != null)
            {
                if (!fpnlLMSSubmenu.Visible)
                    btnLMS.PerformClick();

                Button? button = panel.Name switch
                {
                    "pnlQAGrades" => btnGradeIns,
                    "pnlQACourses" => btnCoursesIns,
                    "pnlQAStudentList" => btnAttendanceIns,
                    _ => null
                };

                button?.PerformClick();
            }
        }

        private async void btnLMS_Click(object sender, EventArgs e)
        {
            btnLMS.Text = !fpnlLMSSubmenu.Visible ? " LMS                                       ⌄" : " LMS                                        ›";
            await submenuAnimLMS.ToggleSubMenuAsync();
        }

        private void btnAnnounceIns_Click(object sender, EventArgs e)
        {
            mainContentPanel.BringToFront();
            mainContentPanel.ShowView(new AnnouncementContentInst());
        }

        private void btnCalendarIns_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new PortalContents.Instructor.LMS.CalendarContentInst());
        }

        private void btnCoursesIns_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new ActivityDashboard());
        }

        private void btnAttendanceIns_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new AttendanceContentInst());
        }

        private void btnGradeIns_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new GradesContentInst());
        }

        private void InstructorPortal_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new Size(1024, 700);
            this.FormClosed += (s, ev) =>
            {
                SharedCalendarData.SaveData();
            };

            ButtonInteraction buttonInteraction = new ButtonInteraction();
            buttonInteraction.InitializeMyPanelEvents(flowLayoutPanel1);

            btnDashboard.PerformClick();
        }
    }
}
