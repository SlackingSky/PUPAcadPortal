using PUPAcadPortal.Models;
using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Services
{
    /// <summary>
    /// Design-time / unit-test stub that performs no database access.
    /// Returns empty collections and throws for write operations that would
    /// require a live database, so bugs are obvious during development.
    /// </summary>
    public class NullCourseDbService : ICourseDbService
    {
        // ── Professor: read ───────────────────────────────────────────────────

        public List<CourseDto> GetCoursesForProfessor(int professorId)
            => new List<CourseDto>();

        public CourseDto? GetCourseById(string subjectOfferingId)
            => null;

        // ── Professor: write ──────────────────────────────────────────────────

        public CourseDto CreateCourse(int professorId, CourseDto dto)
        {
            // Return the DTO with a placeholder ID so the UI can display something.
            dto.SubjectOfferingId = $"NULL-{Guid.NewGuid():N}";
            dto.ProfessorId = professorId;
            return dto;
        }

        public void UpdateCourse(CourseDto dto) { /* no-op */ }

        public void DeleteCourse(string subjectOfferingId) { /* no-op */ }

        // ── Shared lookup helpers ─────────────────────────────────────────────

        public List<SubjectLookupDto> GetAllSubjects()
            => new List<SubjectLookupDto>();

        public List<AcademicPeriodLookupDto> GetAllAcademicPeriods()
            => new List<AcademicPeriodLookupDto>();

        // ── Student: read ─────────────────────────────────────────────────────

        public List<CourseDto> GetCoursesForStudent(int studentId)
            => new List<CourseDto>();
    }
}