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
                // Mapped precisely to your Subject table schema
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

            // FIX: Because ProfessorID is 'int NOT NULL' in your DB, we remove the .Value check 
            // to prevent the compile error, safely grouping by the exact integer.
            var profLoads = await db.SubjectOfferings
                .Where(so => so.AcademicPeriodId == periodId && so.SubjectOfferingId != row.SubjectOfferingId)
                .GroupBy(so => so.ProfessorId)
                .Select(g => new {
                    ProfessorId = g.Key,
                    TotalUnits = g.Sum(so => so.Subject.Units)
                })
                .ToDictionaryAsync(k => k.ProfessorId, v => v.TotalUnits);

            List<int> occupiedProfIds = new List<int>(); // No longer nullable
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

            int nextSec = await db.SubjectOfferings.CountAsync(so => so.SubjectId == existing.SubjectId && so.AcademicPeriodId == existing.AcademicPeriodId) + 1;

            // FIX: Since your SubjectOfferingID is exactly VARCHAR(50), 
            // we can safely use a 40-character GUID to guarantee zero collisions!
            var newOffering = new SubjectOffering
            {
                SubjectOfferingId = "SUB-OFF-" + Guid.NewGuid().ToString("N").ToUpper(),
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
                SessionType = "Lab", // Mapped from your SessionType schema default
                StartTime = new TimeSpan(7, 0, 0),
                EndTime = new TimeSpan(10, 0, 0)
            };
            db.RoomSchedules.Add(newSched);
            await db.SaveChangesAsync();
        }

        public async Task SaveChangesAsync(List<ScheduleRowDto> rows)
        {
            if (rows == null || !rows.Any()) return;

            using var db = new AppDbContext();

            var offeringIds = rows.Select(r => r.SubjectOfferingId).Distinct().ToList();
            var offerings = await db.SubjectOfferings
                .Where(so => offeringIds.Contains(so.SubjectOfferingId))
                .ToDictionaryAsync(so => so.SubjectOfferingId);

            var scheduleIds = rows.Where(r => r.ScheduleId.HasValue).Select(r => r.ScheduleId.Value).ToList();
            var schedules = await db.RoomSchedules
                .Where(rs => scheduleIds.Contains(rs.ScheduleId))
                .ToDictionaryAsync(rs => rs.ScheduleId);

            foreach (var row in rows)
            {
                if (offerings.TryGetValue(row.SubjectOfferingId, out var offering))
                {
                    // If UI sent null (cleared cell), fallback to the existing DB instructor (Because DB is NOT NULL)
                    offering.ProfessorId = row.ProfessorId ?? offering.ProfessorId;
                }

                if (row.ScheduleId.HasValue)
                {
                    if (schedules.TryGetValue(row.ScheduleId.Value, out var sched))
                    {
                        if (!string.IsNullOrEmpty(row.Day) && row.StartTime.HasValue && row.EndTime.HasValue)
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
    }
}