using PUPAcadPortal.Models;
using System.Collections.Generic;

namespace PUPAcadPortal.Services
{
    /// <summary>
    /// Defines all database operations related to courses (SubjectOfferings)
    /// for both Professor (CRUD) and Student (read-only) roles.
    /// </summary>
    public interface ICourseDbService
    {
        // ── Professor: read ───────────────────────────────────────────────────

        /// <summary>Returns all subject offerings assigned to the given professor.</summary>
        List<CourseDto> GetCoursesForProfessor(int professorId);

        /// <summary>Returns a single course by its SubjectOfferingId.</summary>
        CourseDto? GetCourseById(string subjectOfferingId);

        // ── Professor: write ──────────────────────────────────────────────────

        /// <summary>
        /// Creates a new SubjectOffering.
        /// The caller supplies SubjectId, AcademicPeriodId, Section, MaxSlots, and Status.
        /// Returns the fully-populated DTO (with the generated SubjectOfferingId).
        /// </summary>
        CourseDto CreateCourse(int professorId, CourseDto dto);

        /// <summary>Updates an existing SubjectOffering's mutable fields.</summary>
        void UpdateCourse(CourseDto dto);

        /// <summary>
        /// Deletes a SubjectOffering and cascades to Activities, Modules,
        /// GradingCategories, and EnrollmentSubjects where the DB allows.
        /// Throws if the offering has graded submissions (data-integrity guard).
        /// </summary>
        void DeleteCourse(string subjectOfferingId);

        // ── Shared lookup helpers ─────────────────────────────────────────────

        /// <summary>Returns all subjects (for professor's "create course" dropdown).</summary>
        List<SubjectLookupDto> GetAllSubjects();

        /// <summary>Returns all academic periods (for "create/edit course" dropdown).</summary>
        List<AcademicPeriodLookupDto> GetAllAcademicPeriods();

        // ── Student: read ─────────────────────────────────────────────────────

        /// <summary>Returns all courses in which the given student is enrolled.</summary>
        List<CourseDto> GetCoursesForStudent(int studentId);
    }

    // ── Lightweight DTOs ──────────────────────────────────────────────────────

    public class CourseDto
    {
        public string SubjectOfferingId { get; set; } = string.Empty;
        public string SubjectId { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string SubjectCode { get; set; } = string.Empty;
        public string AcademicPeriodId { get; set; } = string.Empty;
        public string AcademicPeriod { get; set; } = string.Empty;  // display label
        public int ProfessorId { get; set; }
        public string InstructorName { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public int MaxSlots { get; set; } = 40;
        public string Status { get; set; } = "Active";
        public string Schedule { get; set; } = string.Empty;
        public string Room { get; set; } = string.Empty;

        // Aggregated statistics (populated from Activities / Submissions)
        public int ActivityCount { get; set; }
        public int TotalAssignments { get; set; }
        public int TotalQuizzes { get; set; }
        public int EnrolledCount { get; set; }
        public int PendingSubmissions { get; set; }
        public int CheckedSubmissions { get; set; }

        // Student-side aggregates (populated when role = student)
        public int PendingCount { get; set; }
        public int SubmittedCount { get; set; }
        public int OverdueCount { get; set; }
    }

    public class SubjectLookupDto
    {
        public string SubjectId { get; set; } = string.Empty;
        public string SubjectCode { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string Display => $"{SubjectCode} – {SubjectName}";
    }

    public class AcademicPeriodLookupDto
    {
        public string AcademicPeriodId { get; set; } = string.Empty;
        public string SchoolYear { get; set; } = string.Empty;
        public string Semester { get; set; } = string.Empty;
        public string Display => $"{SchoolYear} — {Semester}";
    }
}