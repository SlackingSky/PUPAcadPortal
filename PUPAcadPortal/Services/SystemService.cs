using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using PUPAcadPortal.Utils;
using System;
using System.Threading.Tasks;

namespace PUPAcadPortal.Services
{
    public class SystemService
    {
        public async Task LoadCurrentAcademicPeriodAsync()
        {
            using (var context = new AppDbContext())
            {
                var activePeriod = await context.AcademicPeriods
                    .AsNoTracking()
                    .FirstOrDefaultAsync(ap => ap.Status == "Current");


                if (activePeriod == null)
                {
                    MessageBox.Show("There are currently no active academic period, you may continue using the app but some features might not work");

                    return;
                }

                GlobalSession.ActiveAcademicPeriod = activePeriod.AcademicPeriodId;

                GlobalSession.ActiveSchoolYear = activePeriod.SchoolYear;

                GlobalSession.ActiveSemesterName = activePeriod.Semester;

                GlobalSession.ActiveSemesterIndex = activePeriod.Semester switch
                {
                    "1st" => 1,
                    "2nd" => 2,
                    "Summer" => 3,
                    _ => 1
                };
            }
        }
    }
}