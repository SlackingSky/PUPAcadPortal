using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using PUPAcadPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace PUPAcadPortal.Services
{
    internal class ScheduleService
    {
        private readonly AppDbContext _context = new AppDbContext();

        //Dropdown sa grid

        public List<Room> GetRooms()
        {
            return _context.Rooms
                .Where(r => r.Status == "Available")
                .ToList();
        }

        public List<object> GetProfessors()
        {
            return _context.Professors
                .Include(p => p.User)
                .Select(p => new
                {
                    p.ProfessorId,
                    FullName = p.User.FirstName + " " + p.User.LastName
                })
                .ToList<object>();
        }

        //Grid

        public List<object> GetAllScheduleRows()
        {
            var query =
                from so in _context.SubjectOfferings
                join s in _context.Subjects on so.SubjectId equals s.SubjectId
                join rs in _context.RoomSchedules on so.SubjectOfferingId equals rs.SubjectOfferingId into rsGroup
                from rs in rsGroup.DefaultIfEmpty()

                join r in _context.Rooms on rs.RoomId equals r.RoomId into roomGroup
                from room in roomGroup.DefaultIfEmpty()

                join p in _context.Professors on so.ProfessorId equals p.ProfessorId
                join u in _context.Users on p.UserId equals u.UserId

                select new
                {
                    s.SubjectCode,
                    SubjectTitle = s.SubjectName,
                    so.Section,
                    rs.DayOfWeek,
                    rs.StartTime,
                    rs.EndTime,
                    Room = room != null ? room.RoomName : "",
                    Instructor = u.FirstName + " " + u.LastName
                };

            return query.ToList<object>();
        }

        public List<object> GetFilteredSchedule(int yearLevel, int semesterIndex, int revisionYear)
        {
            var query =
                from c in _context.Curricula
                join so in _context.SubjectOfferings on c.SubjectId equals so.SubjectId
                join s in _context.Subjects on so.SubjectId equals s.SubjectId
                join rs in _context.RoomSchedules on so.SubjectOfferingId equals rs.SubjectOfferingId into rsGroup
                from rs in rsGroup.DefaultIfEmpty()

                join r in _context.Rooms on rs.RoomId equals r.RoomId into roomGroup
                from room in roomGroup.DefaultIfEmpty()

                join p in _context.Professors on so.ProfessorId equals p.ProfessorId
                join u in _context.Users on p.UserId equals u.UserId

                where c.YearLevel == yearLevel
                   && c.SemesterIndex == semesterIndex
                   && c.RevisionYear == revisionYear

                select new
                {
                    s.SubjectCode,
                    SubjectTitle = s.SubjectName,
                    so.Section,
                    rs.DayOfWeek,
                    rs.StartTime,
                    rs.EndTime,
                    Room = room != null ? room.RoomName : "",
                    Instructor = u.FirstName + " " + u.LastName
                };

            return query.ToList<object>();
        }

        //duplication

        public void DuplicateOffering(string subjectCode)
        {
            var subject = _context.Subjects
                .FirstOrDefault(s => s.SubjectCode == subjectCode);

            if (subject == null) return;

            var original = _context.SubjectOfferings
                .FirstOrDefault(o => o.SubjectId == subject.SubjectId);

            if (original == null) return;

            int nextSection = GetNextSectionNumber(subjectCode);

            var newOffering = new SubjectOffering
            {
                SubjectOfferingId = Guid.NewGuid().ToString(),
                SubjectId = original.SubjectId,
                AcademicPeriodId = original.AcademicPeriodId,
                ProfessorId = original.ProfessorId,
                Section = $"Section {nextSection}",
                Status = original.Status,
                MaxSlots = original.MaxSlots
            };

            _context.SubjectOfferings.Add(newOffering);
            _context.SaveChanges();

            var schedules = _context.RoomSchedules
                .Where(r => r.SubjectOfferingId == original.SubjectOfferingId)
                .ToList();

            foreach (var sched in schedules)
            {
                _context.RoomSchedules.Add(new RoomSchedule
                {
                    SubjectOfferingId = newOffering.SubjectOfferingId,
                    RoomId = sched.RoomId,
                    DayOfWeek = sched.DayOfWeek,
                    StartTime = sched.StartTime,
                    EndTime = sched.EndTime,
                    SessionType = sched.SessionType
                });
            }

            _context.SaveChanges();
        }

        public int GetNextSectionNumber(string subjectCode)
        {
            var subject = _context.Subjects
                .FirstOrDefault(s => s.SubjectCode == subjectCode);

            if (subject == null) return 1;

            return _context.SubjectOfferings
                .Count(o => o.SubjectId == subject.SubjectId) + 1;
        }

       //dito ung conflict system

        public bool RoomConflict(int roomId, string day, TimeSpan start, TimeSpan end)
        {
            return _context.RoomSchedules
                .Any(r =>
                    r.RoomId == roomId &&
                    r.DayOfWeek == day &&
                    start < r.EndTime &&
                    end > r.StartTime);
        }

        public bool HasProfessorConflict(int professorId, string day, TimeSpan start, TimeSpan end)
        {
            return _context.RoomSchedules
                .Include(r => r.SubjectOffering)
                .Any(r =>
                    r.SubjectOffering.ProfessorId == professorId &&
                    r.DayOfWeek == day &&
                    start < r.EndTime &&
                    end > r.StartTime);
        }

        public bool CanSchedule(int professorId, int roomId, string day, TimeSpan start, TimeSpan end)
        {
            return !RoomConflict(roomId, day, start, end)
                && !HasProfessorConflict(professorId, day, start, end);
        }
    }
}