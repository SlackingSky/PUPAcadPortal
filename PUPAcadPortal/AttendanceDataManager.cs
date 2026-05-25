// =============================================================================
// FILE: AttendanceDataManager.cs
// PURPOSE: Centralized static in-memory DataTable acting as the local database.
//          Seed data covers 3 students × multiple dates for several subjects.
//          Tailored for the PUPAcadPortal namespace block.
// =============================================================================
using System;
using System.Data;

namespace PUPAcadPortal
{
    public static class AttendanceDataManager
    {
        // ── Master DataTable ──────────────────────────────────────────────────
        public static DataTable AttendanceTable { get; private set; }

        // ── Event broadcast whenever the table is mutated ─────────────────────
        public static event EventHandler DataChanged;

        // ── Known subjects (drives the subject ComboBoxes on both forms) ──────
        public static readonly string[] Subjects = new[]
        {
            "Principles of Accounting",
            "Introduction to Programming",
            "Business Mathematics",
            "Fundamentals of Management"
        };

        // ── Static constructor – runs once, builds the table & seeds data ─────
        static AttendanceDataManager()
        {
            AttendanceTable = BuildSchema();
            SeedData();
        }

        // ── Schema ────────────────────────────────────────────────────────────
        private static DataTable BuildSchema()
        {
            var dt = new DataTable("Attendance");

            dt.Columns.Add("StudentID", typeof(string));
            dt.Columns.Add("StudentName", typeof(string));
            dt.Columns.Add("Subject", typeof(string));
            dt.Columns.Add("Date", typeof(DateTime));
            dt.Columns.Add("Status", typeof(string));   // Present, Absent, Late, Excused
            dt.Columns.Add("Remarks", typeof(string));

            return dt;
        }

        // ── Seed Data Injection ───────────────────────────────────────────────
        private static void SeedData()
        {
            var students = new[]
    {
        new { Id = "2024-00074-SM-0", Name = "Hans Louie L. Basilan" },
        new { Id = "2024-00102-SM-0", Name = "Alice M. Dela Cruz" },
        new { Id = "2024-00315-SM-0", Name = "John Doe S. Santos" }
    };

            DateTime today = DateTime.Today;

            // Seed ONLY today's row as an unedited baseline string
            foreach (var student in students)
            {
                foreach (string subject in Subjects)
                {
                    // Seed exactly one row for today with an empty status
                    AttendanceTable.Rows.Add(student.Id, student.Name, subject, today, "", "");
                }
            }
            AttendanceTable.AcceptChanges();
        }

        // ── Data View Extraction Channels ─────────────────────────────────────

        /// <summary>Returns records for a specific subject and date.</summary>
        public static DataView GetBySubjectAndDate(string subject, DateTime date)
        {
            string filter = $"Date = #{date:MM/dd/yyyy}# " +
                            $"AND Subject = '{EscapeFilter(subject)}'";
            return new DataView(AttendanceTable, filter, "StudentName", DataViewRowState.CurrentRows);
        }

        /// <summary>Returns a single student's timeline log history.</summary>
        public static DataView GetStudentHistory(string studentId, string subject)
        {
            string filter = $"StudentID = '{EscapeFilter(studentId)}' " +
                            $"AND Subject = '{EscapeFilter(subject)}'";
            return new DataView(AttendanceTable, filter, "Date DESC", DataViewRowState.CurrentRows);
        }

        /// <summary>
        /// Updates Status and Remarks for the matching record.
        /// Fires DataChanged so any listening form can refresh.
        /// </summary>
        public static void UpdateRecord(string studentId, string subject,
                                        DateTime date, string status, string remarks)
        {
            string filter = $"StudentID = '{EscapeFilter(studentId)}' " +
                            $"AND Subject = '{EscapeFilter(subject)}' " +
                            $"AND Date = #{date:MM/dd/yyyy}#";

            DataRow[] rows = AttendanceTable.Select(filter);
            if (rows.Length > 0)
            {
                rows[0]["Status"] = status;
                rows[0]["Remarks"] = remarks;
                AttendanceTable.AcceptChanges();

                // Fire notification to alert StudentForm to refresh metrics instantly
                DataChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        /// <summary>Returns distinct student IDs + names for a subject/date pair.</summary>
        public static DataView GetDistinctStudentsForSubjectDate(string subject, DateTime date)
        {
            return GetBySubjectAndDate(subject, date);
        }

        // Prevents single-quote breakdown inside filter text strings
        private static string EscapeFilter(string value)
            => value?.Replace("'", "''") ?? string.Empty;
    

    public static void SaveOrUpdateAttendance(string studentId, string studentName, string subject, DateTime date, string status, string remarks)
        {
            // Find if this specific record already exists in your central DataTable
            string filter = $"StudentID = '{studentId}' AND Subject = '{subject}' AND Date = '{date.ToString("yyyy-MM-dd")}'";
            DataRow[] existingRows = AttendanceTable.Select(filter);

            if (existingRows.Length > 0)
            {
                // Update the existing record
                existingRows[0]["Status"] = status;
                existingRows[0]["Remarks"] = remarks;
            }
            else
            {
                // Insert a brand new record if it wasn't there
                AttendanceTable.Rows.Add(studentId, studentName, subject, date.ToString("yyyy-MM-dd"), status, remarks);
            }
        }
    }
}