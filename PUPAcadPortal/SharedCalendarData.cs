using System;
using System.Collections.Generic;

namespace PUPAcadPortal
{
    /// <summary>
    /// Single source of truth for calendar data shared between
    /// InstructorPortal and StudentPortal.
    /// </summary>
    public static class SharedCalendarData
    {
        // ── Shared current calendar state (used by both portals) ─────────────
        public static int CurrentYear = DateTime.Now.Year;
        public static int CurrentMonth = DateTime.Now.Month;

        // Instructor writes → Students read (announcements per date)
        public static Dictionary<DateTime, string> InstructorAnnouncements
            = new Dictionary<DateTime, string>();

        // Each student's own private notes
        public static Dictionary<DateTime, string> StudentNotes
            = new Dictionary<DateTime, string>();

        // Holidays — single source, both portals use this
        public static Dictionary<DateTime, string> Holidays
            = new Dictionary<DateTime, string>
        {
            // 2025
            { new DateTime(2025, 1, 1),  "New Year's Day" },
            { new DateTime(2025, 4, 17), "Maundy Thursday" },
            { new DateTime(2025, 4, 18), "Good Friday" },
            { new DateTime(2025, 4, 19), "Black Saturday" },
            { new DateTime(2025, 4, 9),  "Araw ng Kagitingan" },
            { new DateTime(2025, 5, 1),  "Labor Day" },
            { new DateTime(2025, 6, 12), "Independence Day" },
            { new DateTime(2025, 8, 21), "Ninoy Aquino Day" },
            { new DateTime(2025, 8, 25), "National Heroes Day" },
            { new DateTime(2025, 11, 1), "All Saints' Day" },
            { new DateTime(2025, 11, 30),"Bonifacio Day" },
            { new DateTime(2025, 12, 8), "Immaculate Conception" },
            { new DateTime(2025, 12, 25),"Christmas Day" },
            { new DateTime(2025, 12, 30),"Rizal Day" },
            { new DateTime(2025, 12, 31),"New Year's Eve" },
            // 2026
            { new DateTime(2026, 1, 1),  "New Year's Day" },
            { new DateTime(2026, 2, 25), "EDSA Revolution" },
            { new DateTime(2026, 4, 2),  "Maundy Thursday" },
            { new DateTime(2026, 4, 3),  "Good Friday" },
            { new DateTime(2026, 4, 4),  "Black Saturday" },
            { new DateTime(2026, 4, 9),  "Araw ng Kagitingan" },
            { new DateTime(2026, 5, 1),  "Labor Day" },
            { new DateTime(2026, 6, 12), "Independence Day" },
            { new DateTime(2026, 8, 21), "Ninoy Aquino Day" },
            { new DateTime(2026, 8, 31), "National Heroes Day" },
            { new DateTime(2026, 11, 1), "All Saints' Day" },
            { new DateTime(2026, 11, 2), "All Souls' Day" },
            { new DateTime(2026, 11, 30),"Bonifacio Day" },
            { new DateTime(2026, 12, 8), "Immaculate Conception" },
            { new DateTime(2026, 12, 24),"Christmas Eve" },
            { new DateTime(2026, 12, 25),"Christmas Day" },
            { new DateTime(2026, 12, 30),"Rizal Day" },
            { new DateTime(2026, 12, 31),"New Year's Eve" },
        };
    }
}