using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PUPAcadPortal.Utils;
using PUPAcadPortal.PortalContents.Student.Enrollment;
using PUPAcadPortal.Data;

namespace PUPAcadPortal.PortalContents.Student.Enrollment
{
    public partial class DashboardContentStudent : UserControl
    {
        

        public DashboardContentStudent()
        {
            InitializeComponent();
        }

        // ─────────────────────────────────────────────────────────────────
        // Dashboard quick actions
        // ─────────────────────────────────────────────────────────────────

        private void DashboardContentStudent_Load(object sender, EventArgs e)
        {
            lblDashboardGreeting.Text = $"Welcome back, {UserSession.FullName}!";
            ShowNotEnrolledState();
            lblEnrolledUnits.Text = $"{EnrollmentContentStudent.totalUnits} Units";
            if (EnrollmentContentStudent.isEnrolled)
            {
                // Update the dashboard card
                lblESRegistered.Text = "Enrolled";
                lblESRegistered.ForeColor = Color.Green;
                pnlEnrollmentStatusCard.BackColor = Color.FromArgb(220, 255, 220);
                pbEnrollmentStatusCard.BackColor = Color.Green;
            }

            pnlQACalendar.BindClick();
            pnlQACourses.BindClick();
            pnlQAGrades.BindClick();
            pnlQAPaymentStatus.BindClick();
            pnlQAViewEnrollment.BindClick();
        }
        private void ShowNotEnrolledState()
        {

            // Update dashboard to show "Not Enrolled"
            lblESRegistered.Text = "Not Enrolled";
            lblESRegistered.ForeColor = Color.Orange;
            pnlEnrollmentStatusCard.BackColor = SystemColors.ControlLightLight;
            pbEnrollmentStatusCard.BackColor = Color.Maroon;
        }
    }
}
