using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Data;
using PUPAcadPortal.Models;

namespace PUPAcadPortal.Services
{
    public class ScheduleService
    {
        public async Task<AcademicPeriod> GetLatestAcademicPeriodAsync()
        {
            using var db = new AppDbContext();
            return await db.AcademicPeriods.OrderByDescending(p => p.StartDate).FirstOrDefaultAsync();
        }

        public async Task<(bool Success, string Message)> LoadPreviousYearScheduleAsync(string currentPeriodId)
        {
            using var db = new AppDbContext();

            var currentPeriod = await db.AcademicPeriods.FindAsync(currentPeriodId);
            if (currentPeriod == null) return (false, "Current academic period not found.");

            if (!int.TryParse(currentPeriod.SchoolYear, out int currentSyInt))
                return (false, "Invalid school year format.");

            string prevSchoolYear = (currentSyInt - 101).ToString();

            var prevPeriod = await db.AcademicPeriods
                .FirstOrDefaultAsync(p => p.SchoolYear == prevSchoolYear && p.Semester == currentPeriod.Semester);

            if (prevPeriod == null)
            {
                return (false, $"No previous schedule found. There is no existing record for {currentPeriod.Semester} Semester, SY 20{prevSchoolYear.Substring(0, 2)}-20{prevSchoolYear.Substring(2, 2)}.");
            }

            var currentOfferings = await db.SubjectOfferings
                .Include(so => so.RoomSchedules)
                .Where(so => so.AcademicPeriodId == currentPeriodId)
                .ToListAsync();

            var prevOfferings = await db.SubjectOfferings
                .Include(so => so.RoomSchedules)
                .Where(so => so.AcademicPeriodId == prevPeriod.AcademicPeriodId)
                .ToListAsync();

            int copiedCount = 0;

            foreach (var curOff in currentOfferings)
            {
                if (curOff.RoomSchedules.Any()) continue;

                var match = prevOfferings.FirstOrDefault(p => p.SubjectId == curOff.SubjectId && p.Section == curOff.Section);

                if (match != null && match.RoomSchedules.Any())
                {
                    if (curOff.ProfessorId == null || curOff.ProfessorId == 1)
                    {
                        curOff.ProfessorId = match.ProfessorId;
                    }

                    foreach (var rs in match.RoomSchedules)
                    {
                        db.RoomSchedules.Add(new RoomSchedule
                        {
                            SubjectOfferingId = curOff.SubjectOfferingId,
                            SessionType = rs.SessionType,
                            DayOfWeek = rs.DayOfWeek,
                            RoomId = rs.RoomId,
                            StartTime = rs.StartTime,
                            EndTime = rs.EndTime
                        });
                    }
                    copiedCount++;
                }
            }

            if (copiedCount == 0)
            {
                return (false, "Found the previous semester, but no schedules could be copied (either no schedules existed back then, or your current classes already have schedules).");
            }

            await db.SaveChangesAsync();
            return (true, $"Success! Loaded {copiedCount} class schedules from the previous academic year.");
        }

        public async Task<List<ScheduleRowDto>> GetAllSchedulesAsync(string periodId)
        {
            using var db = new AppDbContext();

            var offerings = await db.SubjectOfferings
                .Include(so => so.Subject)
                .Include(so => so.RoomSchedules)
                .Where(so => so.AcademicPeriodId == periodId)
                .AsNoTracking()
                .ToListAsync();

            var yearLevels = await db.Curricula
                    .Where(c => db.SubjectOfferings
                                  .Where(so => so.AcademicPeriodId == periodId)
                                  .Select(so => so.SubjectId)
                                  .Contains(c.SubjectId))
                    .GroupBy(c => c.SubjectId)
                    .ToDictionaryAsync(g => g.Key, g => g.FirstOrDefault().YearLevel);

            var result = new List<ScheduleRowDto>();

            foreach (var so in offerings)
            {
                yearLevels.TryGetValue(so.SubjectId, out int yl);

                if (so.RoomSchedules != null && so.RoomSchedules.Any())
                {
                    foreach (var rs in so.RoomSchedules)
                    {
                        result.Add(CreateScheduleDto(so, rs, yl));
                    }
                }
                else
                {
                    result.Add(CreateScheduleDto(so, null, yl));
                }
            }

            return result;
        }

        private ScheduleRowDto CreateScheduleDto(SubjectOffering so, RoomSchedule rs, int yearLevel)
        {
            return new ScheduleRowDto
            {
                SubjectOfferingId = so.SubjectOfferingId,
                ScheduleId = rs?.ScheduleId,
                SubjectCode = so.Subject?.SubjectCode,
                SubjectTitle = so.Subject?.SubjectName,
                Lab = so.Subject?.LabUnits ?? 0,
                Lec = so.Subject?.LecUnits ?? 0,
                TotalUnits = so.Subject?.Units ?? 0,
                Section = so.Section,
                MaxSlots = so.MaxSlots,
                Day = rs?.DayOfWeek,
                StartTime = rs?.StartTime,
                EndTime = rs?.EndTime,
                RoomId = rs?.RoomId,
                ProfessorId = so.ProfessorId,
                YearLevel = yearLevel
            };
        }

        public async Task<List<RoomDto>> GetMasterRoomsAsync()
        {
            using var db = new AppDbContext();
            return await db.Rooms
                .Where(r => r.Status == "Available")
                .Select(r => new RoomDto { RoomId = r.RoomId, RoomName = r.RoomName })
                .ToListAsync();
        }

        public async Task<List<ProfessorDto>> GetMasterProfessorsAsync()
        {
            using var db = new AppDbContext();
            return await db.Professors
                .Include(p => p.User)
                .Select(p => new ProfessorDto
                {
                    ProfessorId = p.ProfessorId,
                    FullName = p.User != null ? p.User.FirstName + " " + p.User.LastName : "Unnamed Professor"
                })
                .ToListAsync();
        }

        public async Task<List<RoomDto>> GetValidRoomsAsync(ScheduleRowDto row, string periodId)
        {
            using var db = new AppDbContext();

            var validRooms = await db.Rooms
                .Where(r => r.Status == "Available" && r.Capacity >= row.MaxSlots)
                .ToListAsync();

            if (row.StartTime.HasValue && row.EndTime.HasValue && !string.IsNullOrEmpty(row.Day))
            {
                var occupiedRoomIds = await db.RoomSchedules
                    .Where(rs => rs.SubjectOffering.AcademicPeriodId == periodId
                              && rs.DayOfWeek == row.Day
                              && rs.ScheduleId != row.ScheduleId
                              && row.StartTime < rs.EndTime
                              && row.EndTime > rs.StartTime)
                    .Select(rs => rs.RoomId)
                    .ToListAsync();

                validRooms = validRooms.Where(r => !occupiedRoomIds.Contains(r.RoomId) || r.RoomId == row.RoomId).ToList();
            }

            return validRooms.Select(r => new RoomDto { RoomId = r.RoomId, RoomName = r.RoomName }).ToList();
        }

        public async Task<List<ProfessorDto>> GetValidProfessorsAsync(ScheduleRowDto row, string periodId)
        {
            using var db = new AppDbContext();
            var allProfs = await db.Professors.Include(p => p.User).ToListAsync();

            var profLoads = await db.SubjectOfferings
                .Where(so => so.AcademicPeriodId == periodId && so.SubjectOfferingId != row.SubjectOfferingId)
                .GroupBy(so => so.ProfessorId)
                .Select(g => new {
                    ProfessorId = g.Key,
                    TotalUnits = g.Sum(so => so.Subject.Units)
                })
                .ToDictionaryAsync(k => k.ProfessorId, v => v.TotalUnits);

            List<int> occupiedProfIds = new List<int>();
            if (row.StartTime.HasValue && row.EndTime.HasValue && !string.IsNullOrEmpty(row.Day))
            {
                occupiedProfIds = await db.RoomSchedules
                    .Where(rs => rs.SubjectOffering.AcademicPeriodId == periodId
                              && rs.DayOfWeek == row.Day
                              && rs.ScheduleId != row.ScheduleId
                              && row.StartTime < rs.EndTime
                              && row.EndTime > rs.StartTime)
                    .Select(rs => rs.SubjectOffering.ProfessorId)
                    .ToListAsync();
            }

            var validProfs = new List<ProfessorDto>();

            foreach (var prof in allProfs)
            {
                profLoads.TryGetValue(prof.ProfessorId, out int currentUnits);

                int safeMaxLoad = (prof.MaxLoad > 0) ? prof.MaxLoad : 30;

                if (currentUnits + row.TotalUnits > safeMaxLoad && prof.ProfessorId != row.ProfessorId)
                    continue;

                if (occupiedProfIds.Contains(prof.ProfessorId) && prof.ProfessorId != row.ProfessorId)
                    continue;

                validProfs.Add(new ProfessorDto
                {
                    ProfessorId = prof.ProfessorId,
                    FullName = prof.User != null ? prof.User.FirstName + " " + prof.User.LastName : "Unnamed Professor"
                });
            }
            return validProfs;
        }

        public async Task DuplicateSectionAsync(string subjectOfferingId)
        {
            using var db = new AppDbContext();
            var existing = await db.SubjectOfferings.FindAsync(subjectOfferingId);
            if (existing == null) return;

            var existingSections = await db.SubjectOfferings
                .Where(so => so.SubjectId == existing.SubjectId && so.AcademicPeriodId == existing.AcademicPeriodId)
                .Select(so => so.Section)
                .ToListAsync();

            var sectionNumbers = existingSections
                .Select(s => int.TryParse(s, out int n) ? n : 0)
                .Where(n => n > 0)
                .OrderBy(n => n)
                .ToList();

            int nextSec = 1;
            foreach (var num in sectionNumbers)
            {
                if (num == nextSec) nextSec++;
                else if (num > nextSec) break;
            }

            var newOffering = new SubjectOffering
            {
                SubjectOfferingId = "SUB-" + Guid.NewGuid().ToString().ToUpper(),
                SubjectId = existing.SubjectId,
                AcademicPeriodId = existing.AcademicPeriodId,
                ProfessorId = existing.ProfessorId,
                Section = nextSec.ToString(),
                MaxSlots = existing.MaxSlots,
                Status = existing.Status
            };

            db.SubjectOfferings.Add(newOffering);
            await db.SaveChangesAsync();
        }

        public async Task AddScheduleBlockAsync(string subjectOfferingId)
        {
            using var db = new AppDbContext();
            var newSched = new RoomSchedule
            {
                SubjectOfferingId = subjectOfferingId,
                DayOfWeek = "Monday",
                SessionType = "Lab",
                StartTime = new TimeSpan(7, 0, 0),
                EndTime = new TimeSpan(10, 0, 0)
            };
            db.RoomSchedules.Add(newSched);
            await db.SaveChangesAsync();
        }

        public async Task SaveChangesAsync(List<ScheduleRowDto> rows, string periodId)
        {
            if (rows == null || !rows.Any() || string.IsNullOrEmpty(periodId)) return;

            var activeBlocks = rows
                .Where(r => r.RoomId.HasValue && r.ProfessorId.HasValue && !string.IsNullOrEmpty(r.Day) && r.StartTime.HasValue && r.EndTime.HasValue)
                .ToList();

            for (int i = 0; i < activeBlocks.Count; i++)
            {
                var current = activeBlocks[i];
                for (int j = i + 1; j < activeBlocks.Count; j++)
                {
                    var other = activeBlocks[j];

                    bool timeOverlaps = current.Day == other.Day &&
                                        current.StartTime < other.EndTime &&
                                        current.EndTime > other.StartTime;

                    if (timeOverlaps)
                    {
                        if (current.RoomId == other.RoomId)
                            throw new InvalidOperationException($"Room Conflict: '{current.SubjectCode}' and '{other.SubjectCode}' are both assigned to the same room on {current.Day} at overlapping times.");

                        if (current.ProfessorId == other.ProfessorId)
                            throw new InvalidOperationException($"Instructor Conflict: The same professor is double-booked for '{current.SubjectCode}' and '{other.SubjectCode}' on {current.Day}.");
                    }
                }
            }

            using var db = new AppDbContext();

            var offerings = await db.SubjectOfferings
                .Where(so => so.AcademicPeriodId == periodId)
                .ToDictionaryAsync(so => so.SubjectOfferingId);

            var schedules = await db.RoomSchedules
                .Where(rs => rs.SubjectOffering.AcademicPeriodId == periodId)
                .ToDictionaryAsync(rs => rs.ScheduleId);

            foreach (var row in rows)
            {
                if (offerings.TryGetValue(row.SubjectOfferingId, out var offering))
                {
                    offering.ProfessorId = row.ProfessorId ?? offering.ProfessorId;
                }

                if (row.ScheduleId.HasValue)
                {
                    if (schedules.TryGetValue(row.ScheduleId.Value, out var sched))
                    {
                        if (string.IsNullOrEmpty(row.Day) || !row.StartTime.HasValue || !row.EndTime.HasValue)
                        {
                            db.RoomSchedules.Remove(sched);
                        }
                        else
                        {
                            sched.DayOfWeek = row.Day;
                            sched.StartTime = row.StartTime.Value;
                            sched.EndTime = row.EndTime.Value;
                            sched.RoomId = row.RoomId;
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(row.Day) && row.StartTime.HasValue && row.EndTime.HasValue)
                {
                    db.RoomSchedules.Add(new RoomSchedule
                    {
                        SubjectOfferingId = row.SubjectOfferingId,
                        DayOfWeek = row.Day,
                        SessionType = "Lecture",
                        StartTime = row.StartTime.Value,
                        EndTime = row.EndTime.Value,
                        RoomId = row.RoomId
                    });
                }
            }
            await db.SaveChangesAsync();
        }

        public async Task DeleteRowAsync(string subjectOfferingId, int? scheduleId)
        {
            using var db = new AppDbContext();

            if (scheduleId.HasValue)
            {
                var sched = await db.RoomSchedules.FindAsync(scheduleId.Value);
                if (sched != null)
                {
                    db.RoomSchedules.Remove(sched);
                    await db.SaveChangesAsync();
                }
            }
            else
            {
                var offering = await db.SubjectOfferings
                    .Include(so => so.Subject)
                    .FirstOrDefaultAsync(so => so.SubjectOfferingId == subjectOfferingId);

                if (offering != null)
                {
                    int sectionCount = await db.SubjectOfferings
                        .CountAsync(so => so.SubjectId == offering.SubjectId && so.AcademicPeriodId == offering.AcademicPeriodId);

                    if (sectionCount <= 1)
                    {
                        throw new InvalidOperationException(
                            $"Deletion Blocked: '{offering.Subject.SubjectCode}' must have at least one active section. " +
                            $"If you want to remove the time/room, use the 'Clear Schedule' button instead.");
                    }

                    db.SubjectOfferings.Remove(offering);
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}