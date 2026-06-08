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

        public async Task<List<ScheduleRowDto>> GetAllSchedulesAsync(string periodId)
        {
            using var db = new AppDbContext();

            var offerings = await db.SubjectOfferings
                .Include(so => so.Subject)
                .Include(so => so.RoomSchedules)
                .Where(so => so.AcademicPeriodId == periodId)
                .AsNoTracking()
                .ToListAsync();

            var result = new List<ScheduleRowDto>();

            foreach (var so in offerings)
            {
                if (so.RoomSchedules != null && so.RoomSchedules.Any())
                {
                    foreach (var rs in so.RoomSchedules)
                    {
                        result.Add(CreateScheduleDto(so, rs));
                    }
                }
                else
                {
                    result.Add(CreateScheduleDto(so, null));
                }
            }

            return result;
        }

        private ScheduleRowDto CreateScheduleDto(SubjectOffering so, RoomSchedule rs)
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
                ProfessorId = so.ProfessorId
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

            // --- PRE-SAVE CONFLICT CHECK ---
            // Extract only rows that actually have a schedule being saved
            var activeBlocks = rows
                .Where(r => r.RoomId.HasValue && r.ProfessorId.HasValue && !string.IsNullOrEmpty(r.Day) && r.StartTime.HasValue && r.EndTime.HasValue)
                .ToList();

            // Check the grid against itself to prevent the admin from double-booking in a single save
            for (int i = 0; i < activeBlocks.Count; i++)
            {
                var current = activeBlocks[i];
                for (int j = i + 1; j < activeBlocks.Count; j++)
                {
                    var other = activeBlocks[j];

                    // Do they overlap in time?
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