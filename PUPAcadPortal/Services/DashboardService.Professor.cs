using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Services
{
    public partial class DashboardService
    {
        public async Task<int> GetHandledCourses(int professorId)
        {
            using (var context = new AppDbContext())
            {
                return await context.SubjectOfferings.Where(so => so.ProfessorId == professorId).CountAsync();
            }
        }

        public async Task<int> GetTotalStudentsAsync(int professorId)
        {
            using (var context = new AppDbContext())
            {
                return await context.EnrollmentSubjects
                    .Where(es => es.SubjectOffering.ProfessorId == professorId)
                    .CountAsync();
            }
        }

        public async Task<int> GetGradedSubmissionsAsync(int professorId)
        {
            using (var context = new AppDbContext())
            {
                return await context.Submissions
                    .Include(s => s.Activity)
                        .ThenInclude(a => a.SubjectOffering)
                    .Where(s => s.Status == "Graded" && s.Activity.SubjectOffering.ProfessorId == professorId)
                    .CountAsync();
            }
        }

        public async Task<int> GetPendingSubmissionsAsync(int professorId)
        {
            using (var context = new AppDbContext())
            {
                return await context.Submissions
                    .Include(s => s.Activity)
                        .ThenInclude(a => a.SubjectOffering)
                    .Where(s => s.Status != "Graded" && s.Activity.SubjectOffering.ProfessorId == professorId)
                    .CountAsync();
            }
        }

        public async Task<List<Models.CalendarEvent>> GetUpcomingEventsAsync(int professorId, int userId)
        {
            using (var context = new AppDbContext())
            {
                DateTime today = DateTime.Today;

                var handledCourseIds = context.SubjectOfferings
                    .Where(so => so.ProfessorId == professorId)
                    .Select(so => so.SubjectOfferingId);

                var upcomingEvents = await context.CalendarEvents
                    .Where(e => e.EventDate >= today)
                    .Where(e =>
                        e.UserId == userId ||
                        (e.SubjectOfferingId != null && handledCourseIds.Contains(e.SubjectOfferingId)) ||
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
