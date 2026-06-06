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
                // 1. Query the database for the single row where Status is "Current"
                var activePeriod = await context.AcademicPeriods
                    .AsNoTracking() // AsNoTracking makes read-only queries much faster
                    .FirstOrDefaultAsync(ap => ap.Status == "Current");


                // 2. Safety Net: If the database is missing a current period, stop the app
                if (activePeriod == null)
                {
                    throw new Exception("System Offline: No active academic period was found in the database. Please contact the Registrar.");
                }

                // 3. Inject the live database values into your Global Session memory
                GlobalSession.ActiveAcademicPeriod = activePeriod.AcademicPeriodId; // e.g., "ACAD0001"

                // 4. Map the text ("1st") to the Integer index (1) for the curriculum engine
                GlobalSession.ActiveSemesterIndex = activePeriod.Semester switch
                {
                    "1st" => 1,
                    "2nd" => 2,
                    "Summer" => 3,
                    _ => 1 // Default fallback just in case
                };
            }
        }
    }
}