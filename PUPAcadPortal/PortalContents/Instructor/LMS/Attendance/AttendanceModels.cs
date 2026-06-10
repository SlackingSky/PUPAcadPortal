using System;

namespace PUPAcadPortal.PortalContents.Instructor.LMS
{
    public class CourseSection
    {
        public string Code { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string SubjectCode { get; set; } = string.Empty;

        public string Section { get; set; } = string.Empty;

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public string ScheduleDisplay { get; set; } = string.Empty;

        public string DisplayName =>
            string.IsNullOrWhiteSpace(Section)
                ? $"{SubjectCode}  –  {Title}"
                : $"{SubjectCode}  –  {Title}  [{Section}]";
    }

    public class SessionSlot
    {
        public string Label { get; set; } = string.Empty;
    }
    public class SessionKey
    {
        public string CourseDisplay { get; set; } = string.Empty;
        public string SessionLabel { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        public override string ToString()
            => $"{CourseDisplay} | {SessionLabel} | {Date:yyyy-MM-dd}";
    }


    public enum AttendanceStatus
    {
        Present,
        Late,
        Absent,
        Excused,
    }

    public class StudentAttendanceRecord
    {
        //  DB identity 
        public int AttendanceId { get; set; }

        /// <summary>Student.StudentId (FK).</summary>
        public int StudentId { get; set; }

        //  QR lock state 

        public bool IsQrVerified { get; set; }

        /// <summary>UTC timestamp of the QR scan, or null for manual records.</summary>
        public DateTime? QrScannedAt { get; set; }

        //  Display fields 

        public int RowNumber { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleInitial { get; set; } = string.Empty;
        public string IdNumber { get; set; } = string.Empty;
        public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;
        public string Remarks { get; set; } = string.Empty;
    }
}