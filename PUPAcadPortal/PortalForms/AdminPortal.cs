using PUPAcadPortal.Utils;
using PUPAcadPortal.PortalContents.Admin.Enrollment;
using PUPAcadPortal.PortalContents.Admin.LMS;
using PUPAcadPortal.PortalContents.Admin.SubOffering;
using PUPAcadPortal.PHAddress;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PUPAcadPortal.Models;
using PUPAcadPortal.Events;

namespace PUPAcadPortal.PortalForms
{
    public partial class AdminPortal : Form
    {
        private SubmenuAnim registrarSubmenuAnim;
        private SubmenuAnim subjectOfferingSubmenuAnim;

        public AdminPortal()
        {
            InitializeComponent();
            this.Load += AdminPortal_Load;
            this.FormClosing += CloseApp.Form_Closing;
            btnLogout.Click += LogOut.LogOut_Click;
            QuickActionClickEvent.QuickActionClicked += AQuickActionClicked;
            this.Disposed += AdminPortal_Disposed;
            registrarSubmenuAnim = new SubmenuAnim(pnlRegistrarSubmenu, pnlRegistrarSubmenu.Height);
            subjectOfferingSubmenuAnim = new SubmenuAnim(pnlsubofferingSubmenu, pnlsubofferingSubmenu.Height);
        }

        private void AdminPortal_Disposed(object? sender, EventArgs e)
        {
            QuickActionClickEvent.QuickActionClicked -= AQuickActionClicked;
        }

        private void AQuickActionClicked(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;
            Button? button = panel.Name switch
            {
                "pnlDashboardRegisterProfessor" => btnRegisterProfessor,
                "pnlDashboardRegisterStudent" => btnRegisterStudent,
                "pnlDashboardViewAllUsers" => btnViewAllUsers,
                _ => null
            };

            button.PerformClick();
        }



        //--------------------------
        private void btnSO_CurriculumArchive_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new CurriculumArchiveContentAdmin());
        }

        // Schedule panel
        private void btnSO_Schedule_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new ScheduleContentAdmin());
        }


        //-----------------------------------------------------(Edit Schedule)
        private void btnSO_EditSchedule_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new EditScheduleContentAdmin());
        }


        private void btnDashboard_Click(object sender, EventArgs e)
        {
            mainContentPanel.Visible = true;
            mainContentPanel.BringToFront();
            mainContentPanel.ShowView(new DashboardContentAdmin());
        }

        private async void btnSubjectOffering_Click(object sender, EventArgs e)
        {
            btnSubjectOffering.Text = !pnlsubofferingSubmenu.Visible ? " Subject Offering                    ⌄" : " Subject Offering                     ›";
            await subjectOfferingSubmenuAnim.ToggleSubMenuAsync();
        }

        //-------------------------------------------------------------- (1 current sem page)
        private void btnSO_CurrentSemester_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new CurrentSemesterContentAdmin());
        }

        private void btnAnnouncement_Click(object sender, EventArgs e)
        {
            AnnounceContentAdmin adminAnnounceContent = new AnnounceContentAdmin();
            mainContentPanel.ShowView(adminAnnounceContent);
            adminAnnounceContent.InitAnnouncementPanelIfNeeded();
        }

        private async void btnRegistrarFunctions_Click(object sender, EventArgs e)
        {
            btnRegistrarFunctions.Text = !pnlRegistrarSubmenu.Visible ? " Registrar Functions              ⌄" : " Registrar Functions              ›";
            await registrarSubmenuAnim.ToggleSubMenuAsync();
        }

        // Submenu button event handlers

        private void btnGradesManagement_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new GradesMngContentAdmin());
        }

        private void btnAccountingRecords_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new AccountsContentAdmin());
        }

        private void btnEnrolledStudents_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new EnrolledStudentsContentAdmin());
        }

        // Other sidebar buttons

        private void bgtnRegisterStudents_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new RegisterStudentsContentAdmin());
        }

        private void btnRegisterProf_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new RegisterProfContentAdmin());
        }

        private void btnViewAllUsers_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new ViewUsersContentAdmin());
        }

        //-------------------------------------------------------------- (events current sem page)

        private void AdminPortal_Load(object sender, EventArgs e)
        {
            ButtonInteraction buttonInteraction = new ButtonInteraction();
            buttonInteraction.InitializeMyPanelEvents(flowLayoutPanel1);

            btnDashboard.PerformClick();
        }

        private void btnManageRooms_Click(object sender, EventArgs e)

        {
            mainContentPanel.ShowView(new RoomsContentAdmin());
        }

        private void btnManageSubs_Click(object sender, EventArgs e)
        {
            mainContentPanel.ShowView(new SubjectManagementContentAdmin());
        }
    }
}