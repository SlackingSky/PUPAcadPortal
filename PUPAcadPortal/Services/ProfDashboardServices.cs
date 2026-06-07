using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Services
{
    public class ProfDashboardServices
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
    }
}
