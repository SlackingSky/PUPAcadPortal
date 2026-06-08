using iTextSharp.text;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using PUPAcadPortal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Services
{
    public partial class DashboardService
    {
        public async Task<int> GetEnrolledUnits(int studentId, string academicPeriodId)
        {
            using (var context = new AppDbContext())
            {
                List<int> enrolledUnits = await context.EnrollmentSubjects
                    .Where(es => es.Enrollment.StudentId == studentId
                              && es.Enrollment.AcademicPeriodId == academicPeriodId
                              && (es.SubjectStatus == "Officially Enrolled"))
                    .Select(es => es.SubjectOffering.Subject.Units)
                    .ToListAsync();

                return enrolledUnits.Sum();
            }
        }

        public async Task<string> GetEnrollmentStatusAsync(int studentId, string periodId)
        {
            using (var context = new AppDbContext())
            {
                var enrollment = await context.Enrollments
                    .FirstOrDefaultAsync(e => e.StudentId == studentId && e.AcademicPeriodId == periodId);

                return enrollment != null ? enrollment.Status : "Not Enrolled";
            }
        }

        public async Task<string> GetCurrentSemesterAsync()
        {
            using (var context = new AppDbContext())
            {
                var period = await context.AcademicPeriods
                    .FirstOrDefaultAsync(ap => ap.Status == "Current");

                return period != null ? $"{period.SchoolYear} {period.Semester} Semester" : "TBA";
            }
        }

        public async Task<int> GetPendingItemsCountAsync(int studentId)
        {
            using (var context = new AppDbContext())
            {
                int pendingEnrollments = await context.Enrollments
                    .CountAsync(e => e.StudentId == studentId && e.Status == "Pending Payment");

                int activeHolds = await context.StudentHolds
                    .CountAsync(h => h.StudentId == studentId && h.IsResolved == false);

                return pendingEnrollments + activeHolds;
            }
        }

        public async Task<List<Announcement>> GetAnnouncementsAsync(string role)
        {
            using (var context = new AppDbContext())
            {
                return await context.Announcements
                    .Where(a => a.TargetRoles.Any(r => r.RoleName == role))
                    .OrderByDescending(a => a.IsUrgent)
                    .OrderByDescending(a => a.PostedDate)
                    .Take(5)
                    .ToListAsync();
            }
        }

        public async Task<List<Models.CalendarEvent>> GetEventsAsync(int studentId, int userId)
        {
            using (var context = new AppDbContext())
            {
                DateTime today = DateTime.Today;

                var enrolledCourseIds = context.EnrollmentSubjects
                    .Where(es => es.Enrollment.StudentId == studentId && es.SubjectStatus == "Officially Enrolled")
                    .Select(es => es.SubjectOfferingId);

                var upcomingEvents = await context.CalendarEvents
                    .Where(e => e.EventDate>= today)
                    .Where(e =>
                        e.UserId == userId ||                                                                
                        (e.SubjectOfferingId != null && enrolledCourseIds.Contains(e.SubjectOfferingId)) ||  
                        e.EventType == "University"                                                          
                    )
                    .OrderBy(e => e.EventDate)
                    .Take(5)
                    .ToListAsync();

                return upcomingEvents;
            }
        }
    }
}
