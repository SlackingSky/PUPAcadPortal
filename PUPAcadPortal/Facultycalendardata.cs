using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using PUPAcadPortal.PortalContents.Instructor.LMS.Calendar;
using PUPAcadPortal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace PUPAcadPortal.Data
{
    // ── Event type enum ───────────────────────────────────────────────────────
    public enum FacultyEventType
    {
        Class = 0,
        Activity = 1,
        Quiz = 2,
        LongQuiz = 3,
        Deadline = 4,
        Exam = 5,
        Consultation = 6,
        PersonalNote = 7,
    }

    // ── Calendar event model ──────────────────────────────────────────────────
    public class FacultyCalendarEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime Date { get; set; } = DateTime.Today;
        public string StartTime { get; set; } = "";
        public string EndTime { get; set; } = "";
        public FacultyEventType Type { get; set; } = FacultyEventType.Class;
        public string Course { get; set; } = "";
        public string Room { get; set; } = "";
        public bool IsAllDay { get; set; } = false;
        public bool IsRecurring { get; set; } = false;
        public bool IsAutoSynced { get; set; } = false;
        public List<string> AttachedFiles { get; set; } = new();

        // ── Colour ────────────────────────────────────────────────────────────
        public Color GetColor() => Type switch
        {
            FacultyEventType.Class => Color.FromArgb(66, 133, 244),   // Blue
            FacultyEventType.Activity => Color.FromArgb(0, 153, 102),   // Green
            FacultyEventType.Quiz => Color.FromArgb(138, 43, 226),   // Purple
            FacultyEventType.LongQuiz => Color.FromArgb(100, 0, 180),   // Deep purple
            FacultyEventType.Deadline => Color.FromArgb(255, 140, 0),   // Orange
            FacultyEventType.Exam => Color.FromArgb(220, 53, 69),   // Red
            FacultyEventType.Consultation => Color.FromArgb(32, 178, 170),   // Teal
            FacultyEventType.PersonalNote => Color.FromArgb(100, 100, 100),   // Grey
            _ => Color.DimGray,
        };

        // ── Label ─────────────────────────────────────────────────────────────
        public string GetTypeLabel() => Type switch
        {
            FacultyEventType.Class => "Class",
            FacultyEventType.Activity => "Activity",
            FacultyEventType.Quiz => "Quiz",
            FacultyEventType.LongQuiz => "Long Quiz",
            FacultyEventType.Deadline => "Deadline",
            FacultyEventType.Exam => "Exam",
            FacultyEventType.Consultation => "Consultation",
            FacultyEventType.PersonalNote => "Note",
            _ => "Event",
        };

        // ── Icon ──────────────────────────────────────────────────────────────
        public string GetTypeIcon() => Type switch
        {
            FacultyEventType.Class => "📘",
            FacultyEventType.Activity => "📋",
            FacultyEventType.Quiz => "🧪",
            FacultyEventType.LongQuiz => "📝",
            FacultyEventType.Deadline => "📌",
            FacultyEventType.Exam => "📄",
            FacultyEventType.Consultation => "🩺",
            FacultyEventType.PersonalNote => "🗒",
            _ => "📅",
        };
    }

    // ── Notification model ────────────────────────────────────────────────────
    public class FacultyNotification
    {
        public FacultyCalendarEvent Event { get; set; } = new();
        public DateTime Date { get; set; }
        public int DaysLeft { get; set; }
        public bool IsOverdue => DaysLeft < 0;
        public bool IsToday => DaysLeft == 0;

        public string GetLabel() => IsOverdue ? "OVERDUE"
                                  : IsToday ? "TODAY"
                                  : DaysLeft == 1 ? "TOMORROW"
                                  : $"IN {DaysLeft} DAYS";
    }

    // ── Data service ─────────────────────────────────────────────────────────
    public static class FacultyCalendarData
    {
        // ── Save path ─────────────────────────────────────────────────────────
        private static readonly string SaveFolder =
            Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), "PUPAcadPortal", "Faculty");

        private static readonly string EventsFile =
    Path.Combine(SaveFolder, "faculty_events.txt");

        // ── State ─────────────────────────────────────────────────────────────
        private static bool _loaded = false;

        // All events keyed by date
        private static readonly Dictionary<DateTime, List<FacultyCalendarEvent>> _events = new();

        public static int CurrentYear { get; set; } = DateTime.Now.Year;
        public static int CurrentMonth { get; set; } = DateTime.Now.Month;

        // ── Philippine holidays (2025-2027) ───────────────────────────────────
        public static readonly Dictionary<DateTime, string> Holidays = new()
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
            { new DateTime(2027,  4,  2), "Maundy Thursday"       },
            { new DateTime(2027,  4,  3), "Good Friday"           },
            { new DateTime(2027,  4,  4), "Black Saturday"        },
            { new DateTime(2027,  4,  9), "Araw ng Kagitingan"    },
            { new DateTime(2027,  5,  1), "Labor Day"             },
            { new DateTime(2027,  6, 12), "Independence Day"      },
            { new DateTime(2027,  8, 25), "National Heroes Day"   },
            { new DateTime(2027, 11,  1), "All Saints' Day"       },
            { new DateTime(2027, 11, 30), "Bonifacio Day"         },
            { new DateTime(2027, 12,  8), "Immaculate Conception" },
            { new DateTime(2027, 12, 25), "Christmas Day"         },
            { new DateTime(2027, 12, 30), "Rizal Day"             },
            { new DateTime(2027, 12, 31), "New Year's Eve"        },
        };

        // ── Notifications (computed on every call) ────────────────────────────
        public static List<FacultyNotification> Notifications
        {
            get
            {
                var list = new List<FacultyNotification>();
                var today = DateTime.Now.Date;

                foreach (var kv in _events)
                {
                    int daysLeft = (kv.Key.Date - today).Days;
                    if (daysLeft < -1 || daysLeft > 7) continue;

                    foreach (var ev in kv.Value)
                    {
                        bool worthy = ev.Type switch
                        {
                            FacultyEventType.Exam => daysLeft <= 3,
                            FacultyEventType.LongQuiz => daysLeft <= 3,
                            FacultyEventType.Deadline => daysLeft <= 3,
                            FacultyEventType.Quiz => daysLeft <= 2,
                            FacultyEventType.Class => daysLeft == 0,
                            _ => daysLeft <= 1,
                        };
                        if (worthy)
                            list.Add(new FacultyNotification { Event = ev, Date = kv.Key, DaysLeft = daysLeft });
                    }
                }

                list.Sort((a, b) => a.Date.CompareTo(b.Date));
                return list;
            }
        }

        // ── CRUD ──────────────────────────────────────────────────────────────
        public static List<FacultyCalendarEvent> GetEventsForDate(DateTime date)
        {
            var key = date.Date;
            return _events.ContainsKey(key) ? new List<FacultyCalendarEvent>(_events[key])
                                            : new List<FacultyCalendarEvent>();
        }

        public static List<FacultyCalendarEvent> GetEventsForWeek(DateTime weekStart)
        {
            var result = new List<FacultyCalendarEvent>();
            for (int i = 0; i < 7; i++)
                result.AddRange(GetEventsForDate(weekStart.AddDays(i)));
            return result;
        }

        public static void AddEvent(DateTime date, FacultyCalendarEvent ev)
        {
            ev.Date = date.Date;
            var key = date.Date;
            if (!_events.ContainsKey(key)) _events[key] = new List<FacultyCalendarEvent>();
            _events[key].Add(ev);
            SaveData();
        }

        public static void UpdateEvent(FacultyCalendarEvent updated)
        {
            RemoveEvent(updated.Id);
            AddEvent(updated.Date, updated);
        }

        public static void RemoveEvent(Guid id)
        {
            foreach (var kv in _events)
            {
                var item = kv.Value.FirstOrDefault(e => e.Id == id);
                if (item != null) { kv.Value.Remove(item); break; }
            }
            SaveData();
        }

        public static void MoveEvent(Guid id, DateTime newDate)
        {
            FacultyCalendarEvent? found = null;
            foreach (var kv in _events)
            {
                found = kv.Value.FirstOrDefault(e => e.Id == id);
                if (found != null) { kv.Value.Remove(found); break; }
            }
            if (found == null) return;
            found.Date = newDate.Date;
            AddEvent(newDate, found);
        }

        // ── Upcoming (returns (Date, Ev) tuples matching CalendarContentInst usage) ──
        public static List<(DateTime Date, FacultyCalendarEvent Ev)> GetUpcoming(int count = 8)
        {
            var today = DateTime.Now.Date;
            var result = new List<(DateTime, FacultyCalendarEvent)>();

            foreach (var kv in _events)
            {
                if (kv.Key.Date < today) continue;
                foreach (var ev in kv.Value)
                    result.Add((kv.Key, ev));
            }

            result.Sort((a, b) => a.Item1.CompareTo(b.Item1));
            if (result.Count > count) result.RemoveRange(count, result.Count - count);
            return result;
        }

        // ── Search ────────────────────────────────────────────────────────────
        public static List<FacultyCalendarEvent> Search(
            string keyword,
            FacultyEventType? type,
            string? course,
            DateTime? from,
            DateTime? to)
        {
            var result = new List<FacultyCalendarEvent>();

            foreach (var kv in _events)
            {
                if (from.HasValue && kv.Key.Date < from.Value.Date) continue;
                if (to.HasValue && kv.Key.Date > to.Value.Date) continue;

                foreach (var ev in kv.Value)
                {
                    if (type != null && ev.Type != type) continue;
                    if (!string.IsNullOrEmpty(course) &&
                        !ev.Course.Contains(course, StringComparison.OrdinalIgnoreCase)) continue;
                    if (!string.IsNullOrEmpty(keyword) &&
                        !ev.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) &&
                        !ev.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase) &&
                        !ev.Course.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                        continue;

                    result.Add(ev);
                }
            }

            result.Sort((a, b) => a.Date.CompareTo(b.Date));
            return result;
        }

        // ── Course list ───────────────────────────────────────────────────────
        public static List<string> GetAllCourses()
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var kv in _events)
                foreach (var ev in kv.Value)
                    if (!string.IsNullOrWhiteSpace(ev.Course) && ev.Course != "General")
                        set.Add(ev.Course);
            var list = set.ToList();
            list.Sort();
            return list;
        }

        // ── LMS sync (stub – add real integration here) ───────────────────────
        public static void SyncFromLMS()
        {
            // Simulated LMS sync: add sample auto-synced events for today+1 week
            var today = DateTime.Now.Date;
            var sample = new[]
            {
                new FacultyCalendarEvent
                {
                    Title = "CS101 – Weekly Class",
                    Type  = FacultyEventType.Class,
                    Course = "CS101 – Data Structures",
                    StartTime = "09:00",
                    EndTime   = "10:30",
                    IsAutoSynced = true,
                    Date = today,
                },
                new FacultyCalendarEvent
                {
                    Title = "MATH201 – Quiz 3",
                    Type  = FacultyEventType.Quiz,
                    Course = "MATH201 – Calculus II",
                    StartTime = "13:00",
                    EndTime   = "14:00",
                    IsAutoSynced = true,
                    Date = today.AddDays(2),
                },
            };

            foreach (var ev in sample)
            {
                // Avoid duplicates
                var existing = GetEventsForDate(ev.Date);
                if (!existing.Any(e => e.Title == ev.Title && e.IsAutoSynced))
                    AddEvent(ev.Date, ev);
            }
        }

        // ── Persist ───────────────────────────────────────────────────────────
        public static void LoadData()
        {
            if (_loaded) return;
            try
            {
                Directory.CreateDirectory(SaveFolder);
                if (!File.Exists(EventsFile)) { _loaded = true; return; }

                foreach (var line in File.ReadAllLines(EventsFile, Encoding.UTF8))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var p = line.Split('|');
                    if (p.Length < 11) continue;
                    if (!DateTime.TryParse(p[0], out var date)) continue;

                    var ev = new FacultyCalendarEvent
                    {
                        Id = Guid.TryParse(p[1], out var g) ? g : Guid.NewGuid(),
                        Type = (FacultyEventType)(int.TryParse(p[2], out var t) ? t : 0),
                        Title = Unescape(p[3]),
                        Description = Unescape(p[4]),
                        StartTime = Unescape(p[5]),
                        EndTime = Unescape(p[6]),
                        Course = Unescape(p[7]),
                        Room = Unescape(p[8]),
                        IsAllDay = p[9] == "1",
                        IsRecurring = p[10] == "1",
                        IsAutoSynced = p.Length > 11 && p[11] == "1",
                        Date = date.Date,
                        AttachedFiles = p.Length > 12
                            ? new List<string>(Unescape(p[12]).Split('\t').Where(f => !string.IsNullOrEmpty(f)))
                            : new List<string>(),
                    };

                    if (!_events.ContainsKey(date.Date))
                        _events[date.Date] = new List<FacultyCalendarEvent>();
                    _events[date.Date].Add(ev);
                }
                _loaded = true;
            }
            catch { _loaded = true; }
        }

        public static void SaveData()
        {
            try
            {
                Directory.CreateDirectory(SaveFolder);
                var sb = new StringBuilder();

                foreach (var kv in _events)
                    foreach (var ev in kv.Value)
                        sb.AppendLine(string.Join("|",
                            kv.Key.ToString("yyyy-MM-dd"),
                            ev.Id.ToString(),
                            (int)ev.Type,
                            Escape(ev.Title),
                            Escape(ev.Description),
                            Escape(ev.StartTime),
                            Escape(ev.EndTime),
                            Escape(ev.Course),
                            Escape(ev.Room),
                            ev.IsAllDay ? "1" : "0",
                            ev.IsRecurring ? "1" : "0",
                            ev.IsAutoSynced ? "1" : "0",
                            Escape(string.Join("\t", ev.AttachedFiles))));

                File.WriteAllText(EventsFile, sb.ToString(), Encoding.UTF8);
            }
            catch { }
        }

        // ── String helpers ────────────────────────────────────────────────────
        private static string Escape(string s) =>
            (s ?? "").Replace("|", "\\pipe").Replace("\r\n", "\\n").Replace("\n", "\\n");

        private static string Unescape(string s) =>
            (s ?? "").Replace("\\n", "\n").Replace("\\pipe", "|");
    }
}