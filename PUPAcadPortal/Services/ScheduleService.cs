using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Data;
using PUPAcadPortal.Models;

namespace PUPAcadPortal.Services
{
    public class ScheduleService
    {
        public AcademicPeriod GetLatestAcademicPeriod()
        {
            using var db = new AppDbContext();
            return db.AcademicPeriods.OrderByDescending(p => p.StartDate).FirstOrDefault();
        }

        public List<ScheduleRowDto> GetAllSchedules(string periodId)
        {
            using var db = new AppDbContext();

            var offerings = db.SubjectOfferings
                .Include(so => so.Subject)
                .Include(so => so.RoomSchedules)
                .Where(so => so.AcademicPeriodId == periodId)
                .AsNoTracking()
                .ToList();

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

        public List<RoomDto> GetMasterRooms()
        {
            using var db = new AppDbContext();
            return db.Rooms
                .Where(r => r.Status == "Available")
                .Select(r => new RoomDto { RoomId = r.RoomId, RoomName = r.RoomName })
                .ToList();
        }

        public List<ProfessorDto> GetMasterProfessors()
        {
            using var db = new AppDbContext();
            return db.Professors
                .Include(p => p.User)
                .Select(p => new ProfessorDto { ProfessorId = p.ProfessorId, FullName = p.User.FirstName + " " + p.User.LastName })
                .ToList();
        }

        public List<RoomDto> GetValidRooms(ScheduleRowDto row, string periodId)
        {
            using var db = new AppDbContext();
            var allRooms = db.Rooms.Where(r => r.Status == "Available").ToList();

            var validRooms = allRooms.Where(r => r.Capacity >= row.MaxSlots).ToList();

            if (row.StartTime.HasValue && row.EndTime.HasValue && !string.IsNullOrEmpty(row.Day))
            {
                var occupiedRoomIds = db.RoomSchedules
                    .Where(rs => rs.SubjectOffering.AcademicPeriodId == periodId
                              && rs.DayOfWeek == row.Day
                              && rs.ScheduleId != row.ScheduleId
                              && row.StartTime < rs.EndTime
                              && row.EndTime > rs.StartTime)
                    .Select(rs => rs.RoomId)
                    .ToList();

                validRooms = validRooms.Where(r => !occupiedRoomIds.Contains(r.RoomId) || r.RoomId == row.RoomId).ToList();
            }

            return validRooms.Select(r => new RoomDto { RoomId = r.RoomId, RoomName = r.RoomName }).ToList();
        }

        public List<ProfessorDto> GetValidProfessors(ScheduleRowDto row, string periodId)
        {
            using var db = new AppDbContext();
            var allProfs = db.Professors.Include(p => p.User).ToList();
            var validProfs = new List<ProfessorDto>();

            foreach (var prof in allProfs)
            {
                int currentUnits = db.SubjectOfferings
                    .Where(so => so.AcademicPeriodId == periodId
                              && so.ProfessorId == prof.ProfessorId
                              && so.SubjectOfferingId != row.SubjectOfferingId)
                    .Select(so => so.Subject.Units)
                    .Sum();

                if (currentUnits + row.TotalUnits > prof.MaxLoad && prof.ProfessorId != row.ProfessorId)
                    continue;

                if (row.StartTime.HasValue && row.EndTime.HasValue && !string.IsNullOrEmpty(row.Day))
                {
                    bool isOccupied = db.RoomSchedules
                        .Any(rs => rs.SubjectOffering.AcademicPeriodId == periodId
                                && rs.SubjectOffering.ProfessorId == prof.ProfessorId
                                && rs.DayOfWeek == row.Day
                                && rs.ScheduleId != row.ScheduleId
                                && row.StartTime < rs.EndTime
                                && row.EndTime > rs.StartTime);

                    if (isOccupied && prof.ProfessorId != row.ProfessorId)
                        continue;
                }
                validProfs.Add(new ProfessorDto { ProfessorId = prof.ProfessorId, FullName = prof.User.FirstName + " " + prof.User.LastName });
            }
            return validProfs;
        }

        public void DuplicateSection(string subjectOfferingId)
        {
            using var db = new AppDbContext();
            var existing = db.SubjectOfferings.Find(subjectOfferingId);
            if (existing == null) return;

            int nextSec = db.SubjectOfferings.Count(so => so.SubjectId == existing.SubjectId && so.AcademicPeriodId == existing.AcademicPeriodId) + 1;

            var newOffering = new SubjectOffering
            {
                SubjectOfferingId = "SUB-OFF-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                SubjectId = existing.SubjectId,
                AcademicPeriodId = existing.AcademicPeriodId,
                ProfessorId = existing.ProfessorId,
                Section = nextSec.ToString(),
                MaxSlots = existing.MaxSlots,
                Status = existing.Status
            };
            db.SubjectOfferings.Add(newOffering);
            db.SaveChanges();
        }

        public void AddScheduleBlock(string subjectOfferingId)
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
            db.SaveChanges();
        }

        public void SaveChanges(List<ScheduleRowDto> rows)
        {
            using var db = new AppDbContext();
            foreach (var row in rows)
            {
                var offering = db.SubjectOfferings.Find(row.SubjectOfferingId);
                if (offering != null)
                {
                    offering.ProfessorId = row.ProfessorId ?? offering.ProfessorId;
                }

                if (row.ScheduleId.HasValue)
                {
                    var sched = db.RoomSchedules.Find(row.ScheduleId.Value);
                    if (sched != null && !string.IsNullOrEmpty(row.Day) && row.StartTime.HasValue && row.EndTime.HasValue)
                    {
                        sched.DayOfWeek = row.Day;
                        sched.StartTime = row.StartTime.Value;
                        sched.EndTime = row.EndTime.Value;
                        sched.RoomId = row.RoomId;
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
            db.SaveChanges();
        }
    }
}