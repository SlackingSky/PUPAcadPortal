using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PUPAcadPortal.Services
{
    /// <summary>
    /// Full database-backed implementation of <see cref="ICourseDbService"/>.
    /// All data originates from the MySQL database — no hardcoded or sample values.
    /// </summary>
    public class CourseDbService : ICourseDbService
    {
        private readonly Func<AppDbContext> _ctxFactory;

        public CourseDbService(Func<AppDbContext> ctxFactory)
        {
            _ctxFactory = ctxFactory
                ?? throw new ArgumentNullException(nameof(ctxFactory));
        }

        // ── Professor: read ───────────────────────────────────────────────────

        public List<CourseDto> GetCoursesForProfessor(int professorId)
        {
            using var ctx = _ctxFactory();

            var offerings = ctx.SubjectOfferings
                .Include(o => o.Subject)
                .Include(o => o.AcademicPeriod)
                .Include(o => o.Professor).ThenInclude(p => p.User)
                .Include(o => o.RoomSchedules).ThenInclude(rs => rs.Room)
                .Include(o => o.EnrollmentSubjects)
                .Include(o => o.Activities).ThenInclude(a => a.Submissions)
                .Where(o => o.ProfessorId == professorId)
                .AsNoTracking()
                .OrderBy(o => o.Subject.SubjectCode)
                .ThenBy(o => o.Section)
                .ToList();

            return offerings.Select(MapToDto).ToList();
        }

        public CourseDto? GetCourseById(string subjectOfferingId)
        {
            if (string.IsNullOrWhiteSpace(subjectOfferingId)) return null;

            using var ctx = _ctxFactory();

            var o = ctx.SubjectOfferings
                .Include(x => x.Subject)
                .Include(x => x.AcademicPeriod)
                .Include(x => x.Professor).ThenInclude(p => p.User)
                .Include(x => x.RoomSchedules).ThenInclude(rs => rs.Room)
                .Include(x => x.EnrollmentSubjects)
                .Include(x => x.Activities).ThenInclude(a => a.Submissions)
                .AsNoTracking()
                .FirstOrDefault(x => x.SubjectOfferingId == subjectOfferingId);

            return o == null ? null : MapToDto(o);
        }

        // ── Professor: write ──────────────────────────────────────────────────

        public CourseDto CreateCourse(int professorId, CourseDto dto)
        {
            ValidateCourseDto(dto);

            using var ctx = _ctxFactory();

            // Verify subject and period exist
            bool subjectExists = ctx.Subjects.Any(s => s.SubjectId == dto.SubjectId);
            if (!subjectExists)
                throw new InvalidOperationException(
                    $"Subject '{dto.SubjectId}' does not exist in the database.");

            bool periodExists = ctx.AcademicPeriods.Any(p => p.AcademicPeriodId == dto.AcademicPeriodId);
            if (!periodExists)
                throw new InvalidOperationException(
                    $"Academic period '{dto.AcademicPeriodId}' does not exist.");

            string newId = GenerateOfferingId();

            var entity = new SubjectOffering
            {
                SubjectOfferingId = newId,
                SubjectId = dto.SubjectId.Trim(),
                AcademicPeriodId = dto.AcademicPeriodId.Trim(),
                ProfessorId = professorId,
                Section = dto.Section.Trim(),
                MaxSlots = dto.MaxSlots > 0 ? dto.MaxSlots : 40,
                Status = string.IsNullOrWhiteSpace(dto.Status) ? "Active" : dto.Status.Trim(),
            };

            ctx.SubjectOfferings.Add(entity);
            ctx.SaveChanges();

            dto.SubjectOfferingId = newId;
            dto.ProfessorId = professorId;
            return dto;
        }

        public void UpdateCourse(CourseDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.SubjectOfferingId))
                throw new ArgumentException("SubjectOfferingId is required for update.");

            ValidateCourseDto(dto);

            using var ctx = _ctxFactory();

            var entity = ctx.SubjectOfferings.Find(dto.SubjectOfferingId)
                ?? throw new InvalidOperationException(
                    $"Course '{dto.SubjectOfferingId}' not found.");

            entity.SubjectId = dto.SubjectId.Trim();
            entity.AcademicPeriodId = dto.AcademicPeriodId.Trim();
            entity.Section = dto.Section.Trim();
            entity.MaxSlots = dto.MaxSlots > 0 ? dto.MaxSlots : entity.MaxSlots;
            entity.Status = string.IsNullOrWhiteSpace(dto.Status) ? entity.Status : dto.Status.Trim();

            ctx.SaveChanges();
        }

        public void DeleteCourse(string subjectOfferingId)
        {
            if (string.IsNullOrWhiteSpace(subjectOfferingId)) return;

            using var ctx = _ctxFactory();

            var entity = ctx.SubjectOfferings
                .Include(o => o.Activities).ThenInclude(a => a.Submissions)
                .FirstOrDefault(o => o.SubjectOfferingId == subjectOfferingId)
                ?? throw new InvalidOperationException(
                    $"Course '{subjectOfferingId}' not found.");

            // Safety guard: block deletion if any submission has been graded
            bool hasGraded = entity.Activities
                .SelectMany(a => a.Submissions)
                .Any(s => s.Grade != null);

            if (hasGraded)
                throw new InvalidOperationException(
                    "This course has graded student submissions and cannot be deleted. " +
                    "Archive the course by changing its status to 'Archived' instead.");

            ctx.SubjectOfferings.Remove(entity);
            ctx.SaveChanges();
        }

        // ── Shared lookup helpers ─────────────────────────────────────────────

        public List<SubjectLookupDto> GetAllSubjects()
        {
            using var ctx = _ctxFactory();

            return ctx.Subjects
                .AsNoTracking()
                .OrderBy(s => s.SubjectCode)
                .Select(s => new SubjectLookupDto
                {
                    SubjectId = s.SubjectId,
                    SubjectCode = s.SubjectCode,
                    SubjectName = s.SubjectName,
                })
                .ToList();
        }

        public List<AcademicPeriodLookupDto> GetAllAcademicPeriods()
        {
            using var ctx = _ctxFactory();

            return ctx.AcademicPeriods
                .AsNoTracking()
                .OrderByDescending(p => p.SchoolYear)
                .ThenBy(p => p.Semester)
                .Select(p => new AcademicPeriodLookupDto
                {
                    AcademicPeriodId = p.AcademicPeriodId,
                    SchoolYear = p.SchoolYear,
                    Semester = p.Semester,
                })
                .ToList();
        }

        // ── Student: read ─────────────────────────────────────────────────────

        public List<CourseDto> GetCoursesForStudent(int studentId)
        {
            using var ctx = _ctxFactory();

            var enrollments = ctx.EnrollmentSubjects
                .Include(es => es.Enrollment)
                .Include(es => es.SubjectOffering).ThenInclude(o => o.Subject)
                .Include(es => es.SubjectOffering).ThenInclude(o => o.AcademicPeriod)
                .Include(es => es.SubjectOffering).ThenInclude(o => o.Professor).ThenInclude(p => p.User)
                .Include(es => es.SubjectOffering).ThenInclude(o => o.RoomSchedules).ThenInclude(rs => rs.Room)
                .Include(es => es.SubjectOffering).ThenInclude(o => o.Activities).ThenInclude(a => a.Submissions)
                .Where(es => es.Enrollment.StudentId == studentId
                          && es.SubjectStatus == "Enrolled")
                .AsNoTracking()
                .ToList();

            var result = new List<CourseDto>();

            foreach (var es in enrollments)
            {
                var offering = es.SubjectOffering;
                var dto = MapToDto(offering);

                // Student-specific submission aggregates
                var publishedActivities = offering.Activities
                    .Where(a => a.IsPublished).ToList();

                int pending = 0;
                int submitted = 0;
                int overdue = 0;

                foreach (var act in publishedActivities)
                {
                    var sub = act.Submissions
                        .Where(s => s.StudentId == studentId)
                        .OrderByDescending(s => s.SubmissionDate)
                        .FirstOrDefault();

                    if (sub == null)
                    {
                        if (act.Deadline < DateTime.Now) overdue++;
                        else pending++;
                    }
                    else if (!string.Equals(sub.Status, "Missing",
                                            StringComparison.OrdinalIgnoreCase))
                    {
                        submitted++;
                    }
                }

                dto.PendingCount = pending;
                dto.SubmittedCount = submitted;
                dto.OverdueCount = overdue;
                result.Add(dto);
            }

            return result
                .OrderBy(c => c.SubjectCode)
                .ThenBy(c => c.SubjectName)
                .ToList();
        }

        // ── Private helpers ───────────────────────────────────────────────────

        private static CourseDto MapToDto(SubjectOffering o)
        {
            var activities = o.Activities?.ToList() ?? new();
            var subs = activities.SelectMany(a => a.Submissions ?? new List<Submission>()).ToList();

            int enrolled = o.EnrollmentSubjects?
                .Count(es => es.SubjectStatus == "Enrolled") ?? 0;

            return new CourseDto
            {
                SubjectOfferingId = o.SubjectOfferingId,
                SubjectId = o.SubjectId,
                SubjectName = o.Subject?.SubjectName ?? o.SubjectId,
                SubjectCode = o.Subject?.SubjectCode ?? o.SubjectId,
                AcademicPeriodId = o.AcademicPeriodId,
                AcademicPeriod = o.AcademicPeriod != null
                                        ? $"{o.AcademicPeriod.SchoolYear} — {o.AcademicPeriod.Semester}"
                                        : string.Empty,
                ProfessorId = o.ProfessorId,
                InstructorName = FormatProfessorName(o.Professor),
                Section = o.Section ?? string.Empty,
                MaxSlots = o.MaxSlots,
                Status = o.Status ?? "Active",
                Schedule = FormatSchedule(o.RoomSchedules),
                Room = FormatRoom(o.RoomSchedules),
                ActivityCount = activities.Count,
                TotalAssignments = activities.Count(a =>
                    string.Equals(a.ActivityType, "Assignment", StringComparison.OrdinalIgnoreCase)),
                TotalQuizzes = activities.Count(a =>
                    string.Equals(a.ActivityType, "Quiz", StringComparison.OrdinalIgnoreCase)),
                EnrolledCount = enrolled,
                PendingSubmissions = subs.Count(s => s.Grade == null),
                CheckedSubmissions = subs.Count(s => s.Grade != null),
            };
        }

        private static void ValidateCourseDto(CourseDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.SubjectId))
                throw new ArgumentException("Subject is required.");
            if (string.IsNullOrWhiteSpace(dto.AcademicPeriodId))
                throw new ArgumentException("Academic period is required.");
            if (string.IsNullOrWhiteSpace(dto.Section))
                throw new ArgumentException("Section is required.");
        }

        private static string FormatProfessorName(Professor? professor)
        {
            if (professor?.User == null)
                return professor == null ? "TBA" : $"Professor #{professor.ProfessorId}";
            var user = professor.User;
            string full = $"{user.FirstName} {user.LastName}".Trim();
            return string.IsNullOrWhiteSpace(full) ? "TBA" : $"Prof. {full}";
        }

        private static string FormatSchedule(IEnumerable<RoomSchedule>? schedules)
        {
            if (schedules == null) return "TBA";
            var parts = schedules
                .OrderBy(s => s.DayOfWeek).ThenBy(s => s.StartTime)
                .Select(s => $"{s.DayOfWeek} {FormatTime(s.StartTime)}-{FormatTime(s.EndTime)}")
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct().Take(3).ToList();
            return parts.Count == 0 ? "TBA" : string.Join("; ", parts);
        }

        private static string FormatRoom(IEnumerable<RoomSchedule>? schedules)
        {
            if (schedules == null) return string.Empty;
            var rooms = schedules
                .Select(s => s.Room?.RoomName)
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .Distinct().ToList();
            return rooms.Count == 0 ? string.Empty : string.Join(", ", rooms);
        }

        private static string FormatTime(TimeSpan t)
            => DateTime.Today.Add(t).ToString("h:mm tt");

        private static string GenerateOfferingId()
            => $"SO-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";
    }
}