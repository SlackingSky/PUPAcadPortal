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
using PUPAcadPortal.Services;

namespace PUPAcadPortal.PortalContents.Student.Enrollment
{
    public partial class DashboardContentStudent : UserControl
    {
        StudentDashboardService studentDashboardService = new();
        private int _studentId = UserSession.StudentID ?? 0;
        private string _academicPeriod = GlobalSession.ActiveAcademicPeriod ?? "";

        public DashboardContentStudent()
        {
            InitializeComponent();
        }

        // ─────────────────────────────────────────────────────────────────
        // Dashboard quick actions
        // ─────────────────────────────────────────────────────────────────

        private async void DashboardContentStudent_Load(object sender, EventArgs e)
        {
            lblDashboardGreeting.Text = $"Welcome back, {UserSession.FullName}!";
            //ShowNotEnrolledState();
            //lblEnrolledUnits.Text = $"{EnrollmentContentStudent.totalUnits} Units";
            //if (EnrollmentContentStudent.isEnrolled)
            //{
            //    // Update the dashboard card
            //    lblESRegistered.Text = "Enrolled";
            //    lblESRegistered.ForeColor = Color.Green;
            //    pnlEnrollmentStatusCard.BackColor = Color.FromArgb(220, 255, 220);
            //    pbEnrollmentStatusCard.BackColor = Color.Green;
            //}

            await LoadDashboardDataAsync();
            SetupQuickActionClicks();
        }
        private void ShowNotEnrolledState()
        {

            // Update dashboard to show "Not Enrolled"
            lblESRegistered.Text = "Not Enrolled";
            lblESRegistered.ForeColor = Color.Orange;
            pnlEnrollmentStatusCard.BackColor = SystemColors.ControlLightLight;
            pbEnrollmentStatusCard.BackColor = Color.Maroon;
        }

        private void SetupQuickActionClicks()
        {
            pnlQACalendar.BindClick();
            pnlQACourses.BindClick();
            pnlQAGrades.BindClick();
            pnlQAPaymentStatus.BindClick();
            pnlQAViewEnrollment.BindClick();
        }

        private async Task LoadDashboardDataAsync()
        {
            var enrolledUnits = await studentDashboardService.GetEnrolledUnits(_studentId, _academicPeriod);
            var statusTask = studentDashboardService.GetEnrollmentStatusAsync(_studentId, _academicPeriod);
            var semesterTask = studentDashboardService.GetCurrentSemesterAsync();
            var pendingTask = studentDashboardService.GetPendingItemsCountAsync(_studentId);

            await Task.WhenAll(statusTask, semesterTask, pendingTask);

            if (this.IsDisposed) return;

            this.SafeUIUpdate(async () =>
            {
                lblEnrolledUnits.Text = $"{enrolledUnits} Units";
                lblESRegistered.Text = await statusTask;
                lblCurrentSemester.Text = await semesterTask;
                lblPendingItems.Text = (await pendingTask).ToString();
            });
        }
    }
}
