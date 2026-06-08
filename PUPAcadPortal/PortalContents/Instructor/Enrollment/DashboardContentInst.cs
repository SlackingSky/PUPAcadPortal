using PUPAcadPortal.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PUPAcadPortal.PortalContents.Instructor.LMS;
using PUPAcadPortal.Data;
using PUPAcadPortal.Services;


namespace PUPAcadPortal.PortalContents.Instructor.Enrollment
{
    public partial class DashboardContentInst : UserControl
    {
        private readonly int _profId = UserSession.ProfessorID ?? 0;
        private DashboardService _services = new();

        public DashboardContentInst()
        {
            InitializeComponent();
        }

        private async void DashboardContentInst_Load(object sender, EventArgs e)
        {
            SetupQuickActionClicks();
            await LoadDashboardDataAsync();
        }

        private async Task LoadDashboardDataAsync()
        {
            var coursesHandled = _services.GetHandledCourses(_profId);
            var totalStudents = _services.GetTotalStudentsAsync(_profId);
            var gradedSubmissions = _services.GetGradedSubmissionsAsync(_profId);
            var pendingSubmissions = _services.GetPendingSubmissionsAsync(_profId);
            var upcomings = await _services.GetUpcomingEventsAsync(_profId, UserSession.UserID);
            var announcements = await _services.GetAnnouncementsAsync(UserSession.Role ?? "");

            await Task.WhenAll(coursesHandled, totalStudents, gradedSubmissions,pendingSubmissions);

            this.SafeUIUpdate(async () =>
            {
                lblActiveCourses.Text = (await coursesHandled).ToString();
                lblTotalStudent.Text = (await totalStudents).ToString();
                lblGraded.Text = (await gradedSubmissions).ToString();
                lblPendingGrades.Text = (await pendingSubmissions).ToString();

                if (upcomings.Count > 0)
                {
                    fpnlUpcoming.Controls.Clear();
                    foreach (var item in upcomings)
                    {
                        UpcomingEventReusable upcoming = new()
                        {
                            EventMonth = item.EventDate.Value.ToString("MMM"),
                            EventDay = item.EventDate.Value.Day.ToString(),
                            EventTime = $"{item.StartTime:t} - {item.EndTime:t}",
                            EventTitle = item.Title
                        };
                        fpnlUpcoming.Controls.Add(upcoming);
                    }
                }
                else
                {
                    UpcomingEventReusable upcoming = new()
                    {
                        EventMonth = "",
                        EventDay = "",
                        EventTime = "",
                        EventTitle = "No Upcoming Events"
                    };
                    fpnlUpcoming.Controls.Add(upcoming);
                }

                if (announcements.Count > 0)
                {
                    fpnlAnnouncement.Controls.Clear();
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
                }
                else
                {
                    AnnouncementEnrollmentReusable reusable = new()
                    {
                        AnnounceTitle = "No Current Announcements",
                        AnnounceDesc = "There are currently no announcements",
                        AnnounceDate = ""
                    };
                    fpnlAnnouncement.Controls.Add(reusable);
                }
            });
        }

        private void SetupQuickActionClicks()
        {
            pnlQAGrades.BindClick();
            pnlQACourses.BindClick();
            pnlQAStudentList.BindClick();
        }
    }
}
