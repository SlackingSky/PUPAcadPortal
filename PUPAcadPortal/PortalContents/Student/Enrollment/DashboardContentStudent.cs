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
using PUPAcadPortal.PortalContents.Instructor.Enrollment;

namespace PUPAcadPortal.PortalContents.Student.Enrollment
{
    public partial class DashboardContentStudent : UserControl
    {
        private StudentDashboardService _studentDashboardService = new();
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
            this.DisableControls();
            var enrolledUnits = await _studentDashboardService.GetEnrolledUnits(_studentId, _academicPeriod);
            var statusTask = _studentDashboardService.GetEnrollmentStatusAsync(_studentId, _academicPeriod);
            var semesterTask = _studentDashboardService.GetCurrentSemesterAsync();
            var pendingTask = _studentDashboardService.GetPendingItemsCountAsync(_studentId);
            var announcements = await _studentDashboardService.GetAnnouncementsAsync(UserSession.Role);
            var upcomings = await _studentDashboardService.GetEventsAsync(UserSession.StudentID ?? 0);

            await Task.WhenAll(statusTask, semesterTask, pendingTask);

            this.SafeUIUpdate(async () =>
            {
                lblEnrolledUnits.Text = $"{enrolledUnits} Units";
                lblESRegistered.Text = await statusTask;
                lblCurrentSemester.Text = await semesterTask;
                lblPendingItems.Text = (await pendingTask).ToString();
                this.EnableControls();

                foreach (var item in upcomings)
                {
                    UpcomingEventReusable upcoming = new()
                    {
                        EventMonth = item.EventDate.Value.ToString("MMM"),
                        EventDay = item.EventDate.Value.Day.ToString(),
                        EventTime = $"{item.StartTime.ToShortTimeString()} - {item.EndTime.ToShortTimeString()}",
                        EventTitle = item.Title
                    };
                    fpnlUpcoming.Controls.Add(upcoming);
                }

                foreach (var item in announcements)
                {
                    AnnouncementEnrollmentReusable reusable = new()
                    {
                        AnnounceTitle = item.Title,
                        AnnounceDesc = item.Content,
                        AnnounceDate = item.PostedDate.ToShortDateString()
                    };
                    fpnlAnnouncement.Controls.Add(reusable);
                }
            });
        }
    }
}
