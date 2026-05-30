using System;
using System.Collections.Generic;

namespace PUPAcadPortal.PortalContents.Instructor.LMS
{
    // ── Enums ────────────────────────────────────────────────────────────────
    public enum AttendanceStatus { Present, Late, Absent, Excused }
    public enum AttendanceMethod { Manual, QRCode, AutoSession }

    // ── Student record ───────────────────────────────────────────────────────
    public class StudentAttendanceRecord
    {
        public int    RowNumber     { get; set; }
        public string LastName      { get; set; } = "";
        public string FirstName     { get; set; } = "";
        public string MiddleInitial { get; set; } = "";
        public string IdNumber      { get; set; } = "";
        public AttendanceStatus Status  { get; set; } = AttendanceStatus.Present;
        public string Remarks       { get; set; } = "";

        public string FullName =>
            string.IsNullOrWhiteSpace(MiddleInitial)
                ? $"{FirstName} {LastName}"
                : $"{FirstName} {MiddleInitial}. {LastName}";

        // Back-compat shim
        public string Name
        {
            get => FullName;
            set
            {
                var parts = (value ?? "").Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) return;
                LastName  = parts[^1];
                FirstName = parts[0];
                if (parts.Length >= 3) MiddleInitial = parts[1].TrimEnd('.');
            }
        }
    }

    // ── Course catalogue ─────────────────────────────────────────────────────
    public class CourseSection
    {
        public string Code    { get; set; } = "";
        public string Title   { get; set; } = "";
        public string Section { get; set; } = "";
        public string DisplayName => $"{Code} - {Title} ({Section})";
    }

    // ── Session slot ─────────────────────────────────────────────────────────
    public class SessionSlot
    {
        public string Label { get; set; } = "";
        public override string ToString() => Label;
    }

    // ── Session key (dictionary key for snapshots) ───────────────────────────
    public class SessionKey
    {
        public string   CourseDisplay { get; set; } = "";
        public string   SessionLabel  { get; set; } = "";
        public DateTime Date          { get; set; }

        public override bool Equals(object? obj) =>
            obj is SessionKey k &&
            k.CourseDisplay == CourseDisplay &&
            k.SessionLabel  == SessionLabel  &&
            k.Date.Date     == Date.Date;

        public override int GetHashCode() =>
            HashCode.Combine(CourseDisplay, SessionLabel, Date.Date);
    }
}
