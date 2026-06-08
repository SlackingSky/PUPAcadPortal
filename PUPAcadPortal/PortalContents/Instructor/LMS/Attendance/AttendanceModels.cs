using System;

namespace PUPAcadPortal.PortalContents.Instructor.LMS
{
    /// <summary>
    /// Lightweight view-model representing a single SubjectOffering in the
    /// instructor's course dropdown.  StartTime / EndTime are resolved from
    /// the first RoomSchedule row for the offering so the QR token can embed
    /// the correct attendance window without an extra DB round-trip.
    /// </summary>
    public class CourseSection
    {
        /// <summary>SubjectOffering.SubjectOfferingId (PK).</summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>Subject.SubjectName.</summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>SubjectOffering.Section (e.g. "BSIT 2-1").</summary>
        public string Section { get; set; } = string.Empty;

        /// <summary>
        /// First RoomSchedule.StartTime for this offering, or null if no
        /// schedule has been defined yet.
        /// </summary>
        public TimeSpan? StartTime { get; set; }

        /// <summary>
        /// First RoomSchedule.EndTime for this offering, or null if no
        /// schedule has been defined yet.
        /// </summary>
        public TimeSpan? EndTime { get; set; }

        /// <summary>
        /// Display string shown in the course combo-box:
        /// "COMP 012 – Network Administration  [BSIT 2-1]"
        /// </summary>
        public string DisplayName =>
            string.IsNullOrWhiteSpace(Section)
                ? Title
                : $"{Title}  [{Section}]";
    }

    // ─────────────────────────────────────────────────────────────────────────────
    // SessionSlot — kept for the session combo-box labels
    // ─────────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Represents a human-readable session time-slot label shown in the
    /// session combo-box.  The actual attendance window is driven by
    /// RoomSchedule, not by this label.
    /// </summary>
    public class SessionSlot
    {
        public string Label { get; set; } = string.Empty;
    }

    // ─────────────────────────────────────────────────────────────────────────────
    // SessionKey — used to identify the currently selected session combination
    // ─────────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Identifies the combination of course, session label, and date that the
    /// instructor currently has selected in the filter bar.
    /// </summary>
    public class SessionKey
    {
        public string CourseDisplay { get; set; } = string.Empty;
        public string SessionLabel { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        public override string ToString()
            => $"{CourseDisplay} | {SessionLabel} | {Date:yyyy-MM-dd}";


       
    }

    // ─────────────────────────────────────────────────────────────────────────────
    // AttendanceStatus enum
    // ─────────────────────────────────────────────────────────────────────────────

    /// <summary>Attendance statuses used throughout the instructor module.</summary>
    public enum AttendanceStatus
    {
        Present,
        Late,
        Absent,
        Excused,
    }

    // ─────────────────────────────────────────────────────────────────────────────
    // StudentAttendanceRecord — per-student UI model
    // ─────────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Represents one student's attendance record for the currently selected
    /// ClassSession, as displayed in the AttendanceGridControl.
    /// </summary>
    public class StudentAttendanceRecord
    {
        // ── DB identity ───────────────────────────────────────────────────────────

        /// <summary>AttendanceRecord.AttendanceId (0 if not yet persisted).</summary>
        public int AttendanceId { get; set; }

        /// <summary>Student.StudentId (FK).</summary>
        public int StudentId { get; set; }

        // ── QR lock state ─────────────────────────────────────────────────────────

        /// <summary>
        /// True when this record was created by a successful QR scan.
        /// QR-verified records are read-only in the grid and cannot be
        /// overwritten by manual Save or CSV import.
        /// </summary>
        public bool IsQrVerified { get; set; }

        /// <summary>UTC timestamp of the QR scan, or null for manual records.</summary>
        public DateTime? QrScannedAt { get; set; }

        // ── Display fields ────────────────────────────────────────────────────────

        public int RowNumber { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleInitial { get; set; } = string.Empty;
        public string IdNumber { get; set; } = string.Empty;

        // ── Editable fields ───────────────────────────────────────────────────────

        public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;
        public string Remarks { get; set; } = string.Empty;
    }
}