using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

// NOTE: Do NOT add "using PUPAcadPortal.Models;" here.
// The DB CalendarEvent is referenced fully-qualified below to avoid the
// name clash with the in-memory CalendarEvent defined in this file.

namespace PUPAcadPortal
{
    // ══════════════════════════════════════════════════════════════════════════
    //  EventType enum  (unchanged)
    // ══════════════════════════════════════════════════════════════════════════
    public enum EventType
    {
        Class = 0,
        Exam = 1,
        Deadline = 2,
        Cancelled = 3,
        Consultation = 4,
        Quiz = 5,
        SchoolEvent = 6,
    }

    // ══════════════════════════════════════════════════════════════════════════
    //  In-memory CalendarEvent  (used by all UI code — unchanged public API)
    // ══════════════════════════════════════════════════════════════════════════
    public class CalendarEvent
    {
        // ── DB tracking (new) ─────────────────────────────────────────────────
        /// <summary>
        /// Primary key of the matching row in the CalendarEvents table.
        /// 0 = not yet persisted.
        /// </summary>
        public int DbEventId { get; set; } = 0;

        /// <summary>
        /// True  = student personal event (IsPrivate = true in DB).
        /// False = instructor event       (IsPrivate = false in DB).
        /// </summary>
        public bool IsStudentEvent { get; set; } = false;

        // ── Original fields (unchanged) ───────────────────────────────────────
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public EventType Type { get; set; } = EventType.Class;
        public string StartTime { get; set; } = "";   // "HH:mm"
        public string EndTime { get; set; } = "";   // "HH:mm"
        public string Room { get; set; } = "";
        public string Course { get; set; } = "";

        public Color GetColor()
        {
            return Type switch
            {
                EventType.Class => Color.FromArgb(66, 133, 244),
                EventType.Exam => Color.FromArgb(220, 53, 69),
                EventType.Deadline => Color.FromArgb(255, 140, 0),
                EventType.Cancelled => Color.FromArgb(108, 117, 125),
                EventType.Consultation => Color.FromArgb(32, 178, 170),
                EventType.Quiz => Color.FromArgb(138, 43, 226),
                EventType.SchoolEvent => Color.FromArgb(0, 153, 102),
                _ => Color.DimGray,
            };
        }

        public string GetTypeLabel()
        {
            return Type switch
            {
                EventType.Class => "CLASS",
                EventType.Exam => "EXAM",
                EventType.Deadline => "DEADLINE",
                EventType.Cancelled => "CANCELLED",
                EventType.Consultation => "CONSULT",
                EventType.Quiz => "QUIZ",
                EventType.SchoolEvent => "SCHOOL EVENT",
                _ => "EVENT",
            };
        }

        public string GetIcon()
        {
            return Type switch
            {
                EventType.Class => "📘",
                EventType.Exam => "📝",
                EventType.Deadline => "📌",
                EventType.Cancelled => "🚫",
                EventType.Consultation => "🩺",
                EventType.Quiz => "🧪",
                EventType.SchoolEvent => "🏫",
                _ => "📅",
            };
        }

        public bool IsReminderWorthy(int daysAhead)
        {
            return Type switch
            {
                EventType.Exam => daysAhead <= 3,
                EventType.Deadline => daysAhead <= 3,
                EventType.Quiz => daysAhead <= 2,
                EventType.Class => daysAhead == 0,
                _ => false,
            };
        }
    }

    // ══════════════════════════════════════════════════════════════════════════
    //  SharedCalendarData
    // ══════════════════════════════════════════════════════════════════════════
    public static class SharedCalendarData
    {
        // ── State ─────────────────────────────────────────────────────────────
        public static bool isLoaded = false;

        /// <summary>
        /// Flat-file folder — kept only for notes (no DB table for those).
        /// </summary>
        private static readonly string SaveFolder =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "PUPAcadPortal");

        /// <summary>
        /// Set this to the logged-in user's ID at login time:
        ///     SharedCalendarData.CurrentUserId = session.UserId;
        /// Every new event row is stamped with this value.
        /// </summary>
        public static int CurrentUserId { get; set; } = 1;

        public static int CurrentYear = DateTime.Now.Year;
        public static int CurrentMonth = DateTime.Now.Month;

        public static Dictionary<DateTime, string> InstructorAnnouncements = new();
        public static Dictionary<DateTime, string> StudentNotes = new();
        public static Dictionary<DateTime, List<CalendarEvent>> Events = new();
        public static Dictionary<DateTime, List<CalendarEvent>> StudentEvents = new();

        public static Dictionary<DateTime, string> Holidays = new Dictionary<DateTime, string>
        {
            // 2025
            { new DateTime(2025,  1,  1), "New Year's Day"        },
            { new DateTime(2025,  4,  9), "Araw ng Kagitingan"    },
            { new DateTime(2025,  4, 17), "Maundy Thursday"       },
            { new DateTime(2025,  4, 18), "Good Friday"           },
            { new DateTime(2025,  4, 19), "Black Saturday"        },
            { new DateTime(2025,  5,  1), "Labor Day"             },
            { new DateTime(2025,  6, 12), "Independence Day"      },
            { new DateTime(2025,  8, 21), "Ninoy Aquino Day"      },
            { new DateTime(2025,  8, 25), "National Heroes Day"   },
            { new DateTime(2025, 11,  1), "All Saints' Day"       },
            { new DateTime(2025, 11, 30), "Bonifacio Day"         },
            { new DateTime(2025, 12,  8), "Immaculate Conception" },
            { new DateTime(2025, 12, 25), "Christmas Day"         },
            { new DateTime(2025, 12, 30), "Rizal Day"             },
            { new DateTime(2025, 12, 31), "New Year's Eve"        },
            // 2026
            { new DateTime(2026,  1,  1), "New Year's Day"        },
            { new DateTime(2026,  2, 25), "EDSA Revolution"       },
            { new DateTime(2026,  4,  2), "Maundy Thursday"       },
            { new DateTime(2026,  4,  3), "Good Friday"           },
            { new DateTime(2026,  4,  4), "Black Saturday"        },
            { new DateTime(2026,  4,  9), "Araw ng Kagitingan"    },
            { new DateTime(2026,  5,  1), "Labor Day"             },
            { new DateTime(2026,  6, 12), "Independence Day"      },
            { new DateTime(2026,  8, 21), "Ninoy Aquino Day"      },
            { new DateTime(2026,  8, 31), "National Heroes Day"   },
            { new DateTime(2026, 11,  1), "All Saints' Day"       },
            { new DateTime(2026, 11,  2), "All Souls' Day"        },
            { new DateTime(2026, 11, 30), "Bonifacio Day"         },
            { new DateTime(2026, 12,  8), "Immaculate Conception" },
            { new DateTime(2026, 12, 24), "Christmas Eve"         },
            { new DateTime(2026, 12, 25), "Christmas Day"         },
            { new DateTime(2026, 12, 30), "Rizal Day"             },
            { new DateTime(2026, 12, 31), "New Year's Eve"        },
            // 2027
            { new DateTime(2027,  1,  1), "New Year's Day"        },
            { new DateTime(2027,  2, 25), "EDSA Revolution"       },
            { new DateTime(2027,  4,  2), "Maundy Thursday"       },
            { new DateTime(2027,  4,  3), "Good Friday"           },
            { new DateTime(2027,  4,  4), "Black Saturday"        },
            { new DateTime(2027,  4,  9), "Araw ng Kagitingan"    },
            { new DateTime(2027,  5,  1), "Labor Day"             },
            { new DateTime(2027,  6, 12), "Independence Day"      },
            { new DateTime(2027,  8, 21), "Ninoy Aquino Day"      },
            { new DateTime(2027,  8, 31), "National Heroes Day"   },
            { new DateTime(2027, 11,  1), "All Saints' Day"       },
            { new DateTime(2027, 11,  2), "All Souls' Day"        },
            { new DateTime(2027, 11, 30), "Bonifacio Day"         },
            { new DateTime(2027, 12,  8), "Immaculate Conception" },
            { new DateTime(2027, 12, 24), "Christmas Eve"         },
            { new DateTime(2027, 12, 25), "Christmas Day"         },
            { new DateTime(2027, 12, 30), "Rizal Day"             },
            { new DateTime(2027, 12, 31), "New Year's Eve"        },
        };

        // ── Instructor event CRUD ─────────────────────────────────────────────

        public static List<CalendarEvent> GetEventsForDate(DateTime date)
        {
            var key = date.Date;
            return Events.ContainsKey(key) ? Events[key] : new List<CalendarEvent>();
        }

        public static void AddEvent(DateTime date, CalendarEvent ev)
        {
            ev.IsStudentEvent = false;
            var key = date.Date;
            if (!Events.ContainsKey(key)) Events[key] = new List<CalendarEvent>();
            Events[key].Add(ev);
            SaveEventToDb(date, ev);
        }

        public static void RemoveEvent(DateTime date, CalendarEvent ev)
        {
            var key = date.Date;
            if (Events.ContainsKey(key)) Events[key].Remove(ev);
            DeleteEventFromDb(ev.DbEventId);
        }

        // ── Student event CRUD ────────────────────────────────────────────────

        public static List<CalendarEvent> GetStudentEventsForDate(DateTime date)
        {
            var key = date.Date;
            return StudentEvents.ContainsKey(key) ? StudentEvents[key] : new List<CalendarEvent>();
        }

        public static void AddStudentEvent(DateTime date, CalendarEvent ev)
        {
            ev.IsStudentEvent = true;
            var key = date.Date;
            if (!StudentEvents.ContainsKey(key)) StudentEvents[key] = new List<CalendarEvent>();
            StudentEvents[key].Add(ev);
            SaveEventToDb(date, ev);
        }

        public static void RemoveStudentEvent(DateTime date, CalendarEvent ev)
        {
            var key = date.Date;
            if (StudentEvents.ContainsKey(key)) StudentEvents[key].Remove(ev);
            DeleteEventFromDb(ev.DbEventId);
        }

        // ── Upcoming / reminder queries (unchanged) ───────────────────────────

        public static List<(DateTime Date, CalendarEvent Ev)> GetUpcoming(
            int count = 5,
            bool includeStudentEvents = false)
        {
            var result = new List<(DateTime Date, CalendarEvent Ev)>();
            DateTime today = DateTime.Now.Date;

            foreach (var kv in Events)
            {
                if (kv.Key < today) continue;
                foreach (var ev in kv.Value) result.Add((kv.Key, ev));
            }

            if (includeStudentEvents)
                foreach (var kv in StudentEvents)
                {
                    if (kv.Key < today) continue;
                    foreach (var ev in kv.Value) result.Add((kv.Key, ev));
                }

            result.Sort((a, b) => a.Date.CompareTo(b.Date));
            if (result.Count > count) result.RemoveRange(count, result.Count - count);
            return result;
        }

        public static List<(DateTime Date, CalendarEvent Ev, int DaysLeft)> GetUpcomingReminders(
            int daysAhead = 3,
            bool includeStudentEvents = false)
        {
            var result = new List<(DateTime, CalendarEvent, int)>();
            DateTime today = DateTime.Now.Date;

            void Scan(Dictionary<DateTime, List<CalendarEvent>> store)
            {
                foreach (var kv in store)
                {
                    int days = (kv.Key.Date - today).Days;
                    if (days < 0 || days > daysAhead) continue;
                    foreach (var ev in kv.Value)
                        if (ev.IsReminderWorthy(days))
                            result.Add((kv.Key, ev, days));
                }
            }

            Scan(Events);
            if (includeStudentEvents) Scan(StudentEvents);

            result.Sort((a, b) =>
            {
                int cmp = a.Item1.CompareTo(b.Item1);
                return cmp != 0 ? cmp : a.Item2.Type.CompareTo(b.Item2.Type);
            });
            return result;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  DB persistence — fully-qualified PUPAcadPortal.Models.CalendarEvent
        //  so there is zero ambiguity with the in-memory class above.
        // ══════════════════════════════════════════════════════════════════════

        private static void SaveEventToDb(DateTime date, CalendarEvent ev)
        {
            try
            {
                DateTime startDt = BuildDateTime(date, ev.StartTime);
                DateTime endDt = BuildDateTime(date, ev.EndTime);
                string fullDesc = PackDescription(ev.Description, ev.Room, ev.Course);

                using var ctx = new AppDbContext();

                if (ev.DbEventId > 0)
                {
                    // UPDATE existing row
                    var row = ctx.CalendarEvents.Find(ev.DbEventId);
                    if (row != null)
                    {
                        row.Title = ev.Title;
                        row.Description = fullDesc;
                        row.EventType = ev.Type.ToString();
                        row.StartTime = startDt;
                        row.EndTime = endDt;
                        row.IsPrivate = ev.IsStudentEvent;
                        ctx.SaveChanges();
                    }
                }
                else
                {
                    // INSERT new row — use fully-qualified name to avoid clash
                    var row = new PUPAcadPortal.Models.CalendarEvent
                    {
                        UserId = CurrentUserId,
                        Title = ev.Title,
                        Description = fullDesc,
                        EventType = ev.Type.ToString(),
                        StartTime = startDt,
                        EndTime = endDt,
                        IsPrivate = ev.IsStudentEvent,
                        // RoomId / SubjectOfferingId left null — no FK needed
                    };
                    ctx.CalendarEvents.Add(row);
                    ctx.SaveChanges();

                    // Write the generated PK back so future deletes work
                    ev.DbEventId = row.EventId;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[CalendarDB] SaveEventToDb error: {ex.Message}");
            }
        }

        private static void DeleteEventFromDb(int dbEventId)
        {
            if (dbEventId <= 0) return;
            try
            {
                using var ctx = new AppDbContext();
                var row = ctx.CalendarEvents.Find(dbEventId);
                if (row != null)
                {
                    ctx.CalendarEvents.Remove(row);
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[CalendarDB] DeleteEventFromDb error: {ex.Message}");
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  SaveData / LoadData
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Saves notes to flat files. Events are already written to DB
        /// individually by AddEvent / AddStudentEvent — no duplication here.
        /// </summary>
        public static void SaveData()
        {
            try
            {
                Directory.CreateDirectory(SaveFolder);

                var sb = new StringBuilder();

                // Student notes
                foreach (var kv in StudentNotes)
                    sb.AppendLine($"{kv.Key:yyyy-MM-dd}|{Escape(kv.Value)}");
                File.WriteAllText(
                    Path.Combine(SaveFolder, "student_notes.txt"),
                    sb.ToString(), Encoding.UTF8);

                // Instructor announcement-notes
                sb.Clear();
                foreach (var kv in InstructorAnnouncements)
                    sb.AppendLine($"{kv.Key:yyyy-MM-dd}|{Escape(kv.Value)}");
                File.WriteAllText(
                    Path.Combine(SaveFolder, "announcements.txt"),
                    sb.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[CalendarDB] SaveData error: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads notes from flat files and events from the database (once per session).
        /// </summary>
        public static void LoadData()
        {
            if (isLoaded) return;
            try
            {
                LoadNotesFromFiles();
                LoadEventsFromDb();
                isLoaded = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[CalendarDB] LoadData error: {ex.Message}");
            }
        }

        // ── Note loading ──────────────────────────────────────────────────────

        private static void LoadNotesFromFiles()
        {
            try
            {
                string path = Path.Combine(SaveFolder, "student_notes.txt");
                if (File.Exists(path))
                    foreach (var line in File.ReadAllLines(path, Encoding.UTF8))
                    {
                        var p = line.Split(new[] { '|' }, 2);
                        if (p.Length == 2 && DateTime.TryParse(p[0], out DateTime d))
                            StudentNotes[d.Date] = Unescape(p[1]);
                    }

                path = Path.Combine(SaveFolder, "announcements.txt");
                if (File.Exists(path))
                    foreach (var line in File.ReadAllLines(path, Encoding.UTF8))
                    {
                        var p = line.Split(new[] { '|' }, 2);
                        if (p.Length == 2 && DateTime.TryParse(p[0], out DateTime d))
                            InstructorAnnouncements[d.Date] = Unescape(p[1]);
                    }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[CalendarDB] LoadNotesFromFiles error: {ex.Message}");
            }
        }

        // ── Event loading (DB) ────────────────────────────────────────────────

        private static void LoadEventsFromDb()
        {
            try
            {
                using var ctx = new AppDbContext();

                // Load all rows. To filter by user add:
                //   .Where(e => e.UserId == CurrentUserId)
                var rows = ctx.CalendarEvents
                              .AsNoTracking()
                              .ToList();

                foreach (var row in rows)
                {
                    // Parse EventType string → enum (default Class on failure)
                    if (!Enum.TryParse<EventType>(row.EventType, ignoreCase: true, out EventType type))
                        type = EventType.Class;

                    // Unpack Room & Course from the packed Description field
                    UnpackDescription(row.Description ?? "",
                        out string desc, out string room, out string course);

                    var ev = new CalendarEvent   // in-memory class
                    {
                        DbEventId = row.EventId,
                        IsStudentEvent = row.IsPrivate ?? false,
                        Title = row.Title,
                        Description = desc,
                        Type = type,
                        StartTime = row.StartTime.ToString("HH:mm"),
                        EndTime = row.EndTime.ToString("HH:mm"),
                        Room = room,
                        Course = course,
                    };

                    DateTime key = row.StartTime.Date;

                    if (ev.IsStudentEvent)
                    {
                        if (!StudentEvents.ContainsKey(key)) StudentEvents[key] = new List<CalendarEvent>();
                        StudentEvents[key].Add(ev);
                    }
                    else
                    {
                        if (!Events.ContainsKey(key)) Events[key] = new List<CalendarEvent>();
                        Events[key].Add(ev);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[CalendarDB] LoadEventsFromDb error: {ex.Message}");
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  Helpers
        // ══════════════════════════════════════════════════════════════════════

        private static DateTime BuildDateTime(DateTime date, string timeStr)
        {
            if (!string.IsNullOrWhiteSpace(timeStr) &&
                TimeSpan.TryParse(timeStr, out TimeSpan ts))
                return date.Date + ts;
            return date.Date; // midnight fallback
        }

        // Room & Course are packed into Description because the DB model has
        // no dedicated columns for them.
        // Format: "<desc>|||ROOM:<room>|||COURSE:<course>"
        private const string RoomSentinel = "|||ROOM:";
        private const string CourseSentinel = "|||COURSE:";

        private static string PackDescription(string desc, string room, string course)
        {
            var sb = new StringBuilder(desc ?? "");
            if (!string.IsNullOrEmpty(room)) sb.Append(RoomSentinel).Append(room);
            if (!string.IsNullOrEmpty(course)) sb.Append(CourseSentinel).Append(course);
            return sb.ToString();
        }

        private static void UnpackDescription(
            string packed, out string desc, out string room, out string course)
        {
            // Strip Course (appended last, so rightmost)
            int cIdx = packed.IndexOf(CourseSentinel, StringComparison.Ordinal);
            if (cIdx >= 0) { course = packed.Substring(cIdx + CourseSentinel.Length); packed = packed.Substring(0, cIdx); }
            else { course = ""; }

            // Strip Room
            int rIdx = packed.IndexOf(RoomSentinel, StringComparison.Ordinal);
            if (rIdx >= 0) { room = packed.Substring(rIdx + RoomSentinel.Length); packed = packed.Substring(0, rIdx); }
            else { room = ""; }

            desc = packed;
        }

        // Flat-file escape helpers (notes only)
        private static string Escape(string s)
            => (s ?? "").Replace("|", "\\pipe")
                        .Replace("\r\n", "\\n")
                        .Replace("\n", "\\n");

        private static string Unescape(string s)
            => (s ?? "").Replace("\\n", "\n")
                        .Replace("\\pipe", "|");
    }
}