using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Data;
using PUPAcadPortal.Models;

namespace PUPAcadPortal.Services
{
    public class SemesterGridItem
    {
        public string SubjectCode { get; set; }
        public string SubjectTitle { get; set; }
        public int Lab { get; set; }
        public int Lec { get; set; }
        public int TotalUnits { get; set; }
        public int YearLevel { get; set; }

        public string Status { get; set; }
    }

    public class SemesterSetupService
    {
        public bool IsAnySemesterActive()
        {
            using var db = new AppDbContext();
            return db.AcademicPeriods.Any(p => p.Status == "Current");
        }

        public async Task<string?> GetAcademicPeriodIdAsync(string schoolYearFull, string semRaw)
        {
            string syDb = schoolYearFull.Replace("20", "").Replace("-", "");
            string semDb = semRaw == "1" ? "1st" : (semRaw == "2" ? "2nd" : "Summer");

            using var db = new AppDbContext();
            var period = await db.AcademicPeriods
                .FirstOrDefaultAsync(p => p.SchoolYear == syDb && p.Semester == semDb);

            return period?.AcademicPeriodId;
        }

        public async Task<List<SemesterGridItem>> GetCurriculumPreviewAsync(string syFull, string semRaw)
        {
            using var db = new AppDbContext();

            int targetYear = int.Parse(syFull.Substring(0, 4));
            int semIndex = semRaw == "1" ? 1 : (semRaw == "2" ? 2 : 3);

            int activeRevision = await db.Curricula
                .Where(c => c.RevisionYear <= targetYear)
                .OrderByDescending(c => c.RevisionYear)
                .Select(c => c.RevisionYear)
                .FirstOrDefaultAsync();

            if (activeRevision == 0) return new List<SemesterGridItem>();

            return await db.Curricula
                .Where(c => c.SemesterIndex == semIndex && c.RevisionYear == activeRevision)
                .Include(c => c.Subject)
                .Select(c => new SemesterGridItem
                {
                    SubjectCode = c.Subject.SubjectCode,
                    SubjectTitle = c.Subject.SubjectName,
                    Lab = c.Subject.LabUnits,
                    Lec = c.Subject.LecUnits,
                    TotalUnits = c.Subject.Units,
                    YearLevel = c.YearLevel,
                    Status = "Draft (Not Generated)"
                })
                .ToListAsync();
        }

        public async Task<(bool IsAllowed, string Message)> CanCreateOrGenerateDraftAsync(string syFull, string semRaw)
        {
            using var db = new AppDbContext();

            var activePeriod = await db.AcademicPeriods.FirstOrDefaultAsync(p => p.Status == "Current");
            bool draftExists = await db.AcademicPeriods.AllAsync(p => p.Status == "Inactive" && p.StartDate > activePeriod.StartDate);
            if (draftExists)
            {
                return (false, "There is already a draft semester in the system. Please finalize or delete the existing draft before creating a new one.");
            }

            string semDb = semRaw == "1" ? "1st" : (semRaw == "2" ? "2nd" : "Summer");

            if (activePeriod != null)
            {
                int curSem = activePeriod.Semester.Contains("1") ? 1 : (activePeriod.Semester.Contains("2") ? 2 : 3);
                int curYear = int.Parse(activePeriod.SchoolYear.Substring(0, 2));

                int targetSem = semRaw == "1" ? 1 : (semRaw == "2" ? 2 : 3);
                int targetYear = int.Parse(syFull.Replace("20", "").Replace("-", "").Substring(0, 2));

                bool isNext = (targetSem == curSem + 1 && targetYear == curYear) ||
                              (curSem == 3 && targetSem == 1 && targetYear == curYear + 1);

                if (!isNext)
                {
                    return (false, "Sequence Error: You are trying to create a semester that is not the immediate next one. Please finish the current semester sequence.");
                }
            }

            return (true, string.Empty);
        }

        public async Task<string> GetPeriodStatusAsync(string schoolYearFull, string semRaw)
        {
            string syDb = schoolYearFull.Replace("20", "").Replace("-", "");
            string semDb = semRaw == "1" ? "1st" : (semRaw == "2" ? "2nd" : "Summer");

            using var db = new AppDbContext();
            var period = await db.AcademicPeriods
                .FirstOrDefaultAsync(p => p.SchoolYear == syDb && p.Semester == semDb);

            return period?.Status ?? "Inactive";
        }

        public async Task<(bool CanProceed, string Message)> CanInitializePeriod(string schoolYear, string semRaw)
        {
            using var db = new AppDbContext();

            string semDb = semRaw == "1" ? "1st" : (semRaw == "2" ? "2nd" : "Summer");

            var activePeriod = await db.AcademicPeriods.FirstOrDefaultAsync(p => p.Status == "Current");

            if (activePeriod == null) return (true, "");

            int curSem = activePeriod.Semester.Contains("1") ? 1 : (activePeriod.Semester.Contains("2") ? 2 : 3);
            int curYear = int.Parse(activePeriod.SchoolYear.Substring(0, 2));

            int targetSem = semRaw == "1" ? 1 : (semRaw == "2" ? 2 : 3);
            int targetYear = int.Parse(schoolYear.Replace("20", "").Replace("-", "").Substring(0, 2));

            bool isNext = (targetSem == curSem + 1 && targetYear == curYear) ||
                          (curSem == 3 && targetSem == 1 && targetYear == curYear + 1);

            if (activePeriod.SchoolYear == schoolYear.Replace("20", "").Replace("-", "") && activePeriod.Semester == semDb)
                return (true, "");

            if (isNext)
                return (true, "");

            return (false, "Sequence Error: You cannot generate/edit this semester. You must finish the current active semester or start the immediate next one.");
        }

        public async Task<string> EnsureAcademicPeriodExistsAsync(string schoolYearFull, string semRaw)
        {
            string syDb = schoolYearFull.Replace("20", "").Replace("-", "");
            string semDb = semRaw == "1" ? "1st" : (semRaw == "2" ? "2nd" : "Summer");

            using var db = new AppDbContext();
            var period = await db.AcademicPeriods
                .FirstOrDefaultAsync(p => p.SchoolYear == syDb && p.Semester == semDb);

            if (period != null) return period.AcademicPeriodId;

            string newId = "ACAD" + Guid.NewGuid().ToString("N").Substring(0, 4).ToUpper();
            period = new AcademicPeriod
            {
                AcademicPeriodId = newId,
                SchoolYear = syDb,
                Semester = semDb,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(5),
                Status = "Inactive"
            };
            db.AcademicPeriods.Add(period);
            await db.SaveChangesAsync();
            return newId;
        }

        public async Task SetCurrentPeriodAsync(string periodId)
        {
            using var db = new AppDbContext();
            var allPeriods = await db.AcademicPeriods.ToListAsync();

            foreach (var p in allPeriods)
            {
                p.Status = (p.AcademicPeriodId == periodId) ? "Current" : "Inactive";
            }
            await db.SaveChangesAsync();
        }

        public async Task GenerateOfferingsFromCurriculumAsync(string periodId, string schoolYearFull, string semRaw)
        {
            using var db = new AppDbContext();

            int targetYear = int.Parse(schoolYearFull.Substring(0, 4));
            int semIndex = semRaw == "1" ? 1 : (semRaw == "2" ? 2 : 3);

            int activeRevision = await db.Curricula
                .Where(c => c.RevisionYear <= targetYear)
                .OrderByDescending(c => c.RevisionYear)
                .Select(c => c.RevisionYear)
                .FirstOrDefaultAsync();

            if (activeRevision == 0) return;

            var curriculumSubjects = await db.Curricula
                .Where(c => c.SemesterIndex == semIndex && c.RevisionYear == activeRevision)
                .Include(c => c.Subject)
                .ToListAsync();

            var existingOfferings = await db.SubjectOfferings
                .Where(so => so.AcademicPeriodId == periodId)
                .Select(so => so.SubjectId)
                .ToListAsync();

            int defaultProfId = await db.Professors.Select(p => p.ProfessorId).FirstOrDefaultAsync();
            if (defaultProfId == 0) defaultProfId = 1;

            bool changesMade = false;

            foreach (var item in curriculumSubjects)
            {
                if (!existingOfferings.Contains(item.SubjectId))
                {
                    var newOffering = new SubjectOffering
                    {
                        SubjectOfferingId = "SUB-OFF-" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                        SubjectId = item.SubjectId,
                        AcademicPeriodId = periodId,
                        ProfessorId = defaultProfId,
                        Section = "1",
                        MaxSlots = 50,
                        Status = "Active"
                    };
                    db.SubjectOfferings.Add(newOffering);
                    changesMade = true;
                }
            }

            if (changesMade)
            {
                await db.SaveChangesAsync();
            }
        }

        public async Task<List<SemesterGridItem>> GetOfferingsForGridAsync(string periodId, string schoolYearFull, string semRaw)
        {
            using var db = new AppDbContext();

            var existingOfferings = await db.SubjectOfferings
                .Where(so => so.AcademicPeriodId == periodId)
                .Include(so => so.Subject)
                .Select(so => new SemesterGridItem
                {
                    SubjectCode = so.Subject.SubjectCode,
                    SubjectTitle = so.Subject.SubjectName,
                    Lab = so.Subject.LabUnits,
                    Lec = so.Subject.LecUnits,
                    TotalUnits = so.Subject.Units,
                    YearLevel = db.Curricula.Where(c => c.SubjectId == so.SubjectId).Select(c => c.YearLevel).FirstOrDefault(),
                    Status = so.Status
                })
                .ToListAsync();

            if (existingOfferings.Any())
            {
                return existingOfferings;
            }

            int targetYear = int.Parse(schoolYearFull.Substring(0, 4));
            int semIndex = semRaw == "1" ? 1 : (semRaw == "2" ? 2 : 3);

            int activeRevision = await db.Curricula
                .Where(c => c.RevisionYear <= targetYear)
                .OrderByDescending(c => c.RevisionYear)
                .Select(c => c.RevisionYear)
                .FirstOrDefaultAsync();

            if (activeRevision == 0) return new List<SemesterGridItem>();

            var previewCurriculum = await db.Curricula
                .Where(c => c.SemesterIndex == semIndex && c.RevisionYear == activeRevision)
                .Include(c => c.Subject)
                .Select(c => new SemesterGridItem
                {
                    SubjectCode = c.Subject.SubjectCode,
                    SubjectTitle = c.Subject.SubjectName,
                    Lab = c.Subject.LabUnits,
                    Lec = c.Subject.LecUnits,
                    TotalUnits = c.Subject.Units,
                    YearLevel = c.YearLevel,
                    Status = "Draft (Not Generated)"
                })
                .ToListAsync();

            return previewCurriculum;
        }

        public async Task<(bool IsValid, string ErrorMessage)> ValidateBeforeActivationAsync(string periodId)
        {
            using var db = new AppDbContext();

            var totalOfferings = await db.SubjectOfferings.CountAsync(so => so.AcademicPeriodId == periodId);
            if (totalOfferings == 0)
            {
                return (false, "No classes have been generated for this semester yet.\n\nPlease click '1. Generate Classes (Draft)' first.");
            }

            var unscheduledCodes = await db.SubjectOfferings
                .Where(so => so.AcademicPeriodId == periodId)
                .Where(so => !so.RoomSchedules.Any())
                .Select(so => so.Subject.SubjectCode)
                .Distinct()
                .ToListAsync();

            if (unscheduledCodes.Any())
            {
                string sampleList = string.Join(", ", unscheduledCodes.Take(3));
                if (unscheduledCodes.Count > 3) sampleList += ", ...";

                return (false, $"Cannot activate semester! There are {unscheduledCodes.Count} subject offerings that have not been assigned a room or time schedule yet (e.g., {sampleList}).\n\nPlease navigate to the 'Edit Schedules' tab and complete them first.");
            }

            return (true, string.Empty);
        }

        public async Task<(bool IsValid, string ErrorMessage)> ValidateSequentialActivationAsync(string newPeriodId)
        {
            using var db = new AppDbContext();

            var newPeriod = await db.AcademicPeriods.FindAsync(newPeriodId);

            var currentPeriod = await db.AcademicPeriods.FirstOrDefaultAsync(p => p.Status == "Current");

            if (currentPeriod == null) return (true, string.Empty);

            int GetSemIndex(string sem) => sem.Contains("1") ? 1 : (sem.Contains("2") ? 2 : 3);

            int curSem = GetSemIndex(currentPeriod.Semester);
            int newSem = GetSemIndex(newPeriod.Semester);
            int curYear = int.Parse(currentPeriod.SchoolYear.Substring(0, 2));
            int newYear = int.Parse(newPeriod.SchoolYear.Substring(0, 2));

            int expectedSem;
            int expectedYear = curYear;

            if (curSem < 3)
            {
                expectedSem = curSem + 1;
            }
            else
            {
                expectedSem = 1;
                expectedYear = curYear + 1;
            }

            if (newSem == expectedSem && newYear == expectedYear)
            {
                return (true, string.Empty);
            }

            return (false, $"Sequence Error: You are trying to activate {newPeriod.SchoolYear} {newPeriod.Semester} Semester, " +
                           $"but the system expects {expectedYear.ToString().PadLeft(2, '0') + (expectedYear + 1).ToString().PadLeft(2, '0')} " +
                           $"{GetSemName(expectedSem)} Semester next. Please finalize the current semester first.");
        }

        private string GetSemName(int index) => index == 1 ? "1st" : (index == 2 ? "2nd" : "Summer");
    }
}