using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PUPAcadPortal.Services
{
    public class StudentGridItem
    {
        [DisplayName("Student No.")]
        public string StudentNumber { get; set; }

        [DisplayName("Full Name")]
        public string FullName { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        public string Program { get; set; }

        [DisplayName("Year Level")]
        public int YearLevel { get; set; }

        public string Status { get; set; }
    }

    public class EnrolledStudentService
    {
        public async Task<List<StudentGridItem>> GetEnrolledStudentsAsync()
        {
            using (var context = new AppDbContext())
            {
                var students = await context.Students
                    .Include(s => s.User)
                    .Include(s => s.Enrollments)
                        .ThenInclude(e => e.AcademicPeriod)
                    .ToListAsync();

                var resultList = new List<StudentGridItem>();

                foreach (var s in students)
                {
                    string computedStatus = "Inactive";

                    var activeEnrollment = s.Enrollments.FirstOrDefault(e =>
                        e.AcademicPeriod != null && e.AcademicPeriod.Status == "Current");

                    if (activeEnrollment != null)
                    {
                        computedStatus = "Active";
                    }
                    else if (s.AcademicStanding == "Graduated")
                    {
                        computedStatus = "Graduated";
                    }

                    resultList.Add(new StudentGridItem
                    {
                        StudentNumber = s.StudentNumber,
                        FullName = $"{s.User.LastName}, {s.User.FirstName} {s.User.MiddleName}".Trim(),
                        Email = s.User.InstitutionalEmail,
                        Program = s.Program,
                        YearLevel = s.YearLevel,
                        Status = computedStatus
                    });
                }

                return resultList;
            }
        }
    }
}