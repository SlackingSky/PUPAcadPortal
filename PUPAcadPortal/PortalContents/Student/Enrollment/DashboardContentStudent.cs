using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PUPAcadPortal.Utils;
using PUPAcadPortal.PortalContents.Student.Enrollment;

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
        private void btnDashboardViewEnrollment_Click(object sender, EventArgs e)
        {
            
        }
        private void btnDashboardCourses_Click(object sender, EventArgs e)
        {

        }
        private void btnDashboardPaymentStatus_Click(object sender, EventArgs e)
        {

        }

        private void btnDashboardGrades_Click(object sender, EventArgs e)
        {

        }

        private void btnDashboardCalendar_Click(object sender, EventArgs e)
        {

        }

        private void DashboardContentStudent_Load(object sender, EventArgs e)
        {
            lblDashboardGreeting.Text = $"Welcome back, {SignIn.authenticatedUser?.FirstName} {SignIn.authenticatedUser?.LastName}!";
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
