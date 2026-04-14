using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace PUPAcadPortal
{
    //  Event types 
    public enum EventType { Class, Exam, Deadline, Cancelled, Consultation }

    //  Single calendar event 
    public class CalendarEvent
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public EventType Type { get; set; } = EventType.Class;
        public string StartTime { get; set; } = "";
        public string EndTime { get; set; } = "";
        public string Room { get; set; } = "";

        public Color GetColor()
        {
            switch (Type)
            {
                case EventType.Class: return Color.FromArgb(66, 133, 244);   // Blue
                case EventType.Exam: return Color.FromArgb(220, 53, 69);    // Red
                case EventType.Deadline: return Color.FromArgb(255, 140, 0);    // Orange
                case EventType.Cancelled: return Color.FromArgb(108, 117, 125);  // Gray
                case EventType.Consultation: return Color.FromArgb(32, 178, 170);   // Teal
                default: return Color.DimGray;
            }
        }

        public string GetTypeLabel()
        {
            switch (Type)
            {
                case EventType.Class: return "CLASS";
                case EventType.Exam: return "EXAM";
                case EventType.Deadline: return "DEADLINE";
                case EventType.Cancelled: return "CANCELLED";
                case EventType.Consultation: return "CONSULT";
                default: return "EVENT";
            }
        }
    }

    //  Shared data hub 
    public static class SharedCalendarData
    {
        private static readonly string SaveFolder =
            Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), "PUPAcadPortal");

        //  Current calendar position 
        public static int CurrentYear = DateTime.Now.Year;
        public static int CurrentMonth = DateTime.Now.Month;

        //  Data stores 
        public static Dictionary<DateTime, string> InstructorAnnouncements = new Dictionary<DateTime, string>();
        public static Dictionary<DateTime, string> StudentNotes = new Dictionary<DateTime, string>();
        public static Dictionary<DateTime, List<CalendarEvent>> Events = new Dictionary<DateTime, List<CalendarEvent>>();

        public static Dictionary<DateTime, string> Holidays = new Dictionary<DateTime, string>
        {
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
        };


        public static List<CalendarEvent> GetEventsForDate(DateTime date)
        {
            var key = date.Date;
            return Events.ContainsKey(key) ? Events[key] : new List<CalendarEvent>();
        }

        public static void AddEvent(DateTime date, CalendarEvent ev)
        {
            var key = date.Date;
            if (!Events.ContainsKey(key)) Events[key] = new List<CalendarEvent>();
            Events[key].Add(ev);
            SaveData();
        }

        public static void RemoveEvent(DateTime date, CalendarEvent ev)
        {
            var key = date.Date;
            if (Events.ContainsKey(key)) Events[key].Remove(ev);
            SaveData();
        }

        /// <summary>
        /// Returns the next <paramref name="count"/> events from today (across all types).
        /// FIX CS1061: tuple element is accessed as .Date (DateTime) and .Ev (CalendarEvent)
        /// using named tuple syntax so callers can write  (d, ev)  cleanly.
        /// </summary>
        public static List<(DateTime Date, CalendarEvent Ev)> GetUpcoming(int count = 5)
        {
            var result = new List<(DateTime Date, CalendarEvent Ev)>();
            DateTime today = DateTime.Now.Date;

            foreach (var kv in Events)
            {
                if (kv.Key < today) continue;
                foreach (var ev in kv.Value)
                    result.Add((kv.Key, ev));   
            }

            result.Sort((a, b) => a.Date.CompareTo(b.Date));   

            if (result.Count > count)
                result.RemoveRange(count, result.Count - count);

            return result;
        }

        public static void SaveData()
        {
            try
            {
                Directory.CreateDirectory(SaveFolder);

                // Student notes
                var sb = new StringBuilder();
                foreach (var kv in StudentNotes)
                    sb.AppendLine($"{kv.Key:yyyy-MM-dd}|{Escape(kv.Value)}");
                File.WriteAllText(Path.Combine(SaveFolder, "student_notes.txt"), sb.ToString(), Encoding.UTF8);

                // Instructor announcements
                sb.Clear();
                foreach (var kv in InstructorAnnouncements)
                    sb.AppendLine($"{kv.Key:yyyy-MM-dd}|{Escape(kv.Value)}");
                File.WriteAllText(Path.Combine(SaveFolder, "announcements.txt"), sb.ToString(), Encoding.UTF8);

                // Events
                sb.Clear();
                foreach (var kv in Events)
                    foreach (var ev in kv.Value)
                        sb.AppendLine(string.Join("|",
                            kv.Key.ToString("yyyy-MM-dd"),
                            (int)ev.Type,
                            Escape(ev.Title),
                            Escape(ev.Description),
                            Escape(ev.StartTime),
                            Escape(ev.EndTime),
                            Escape(ev.Room)));
                File.WriteAllText(Path.Combine(SaveFolder, "events.txt"), sb.ToString(), Encoding.UTF8);
            }
            catch { }
        }

        public static void LoadData()
        {
            try
            {
                // Student notes
                string path = Path.Combine(SaveFolder, "student_notes.txt");
                if (File.Exists(path))
                    foreach (var line in File.ReadAllLines(path, Encoding.UTF8))
                    {
                        var p = line.Split(new[] { '|' }, 2);
                        if (p.Length == 2 && DateTime.TryParse(p[0], out DateTime d))
                            StudentNotes[d.Date] = Unescape(p[1]);
                    }

                // Instructor announcements
                path = Path.Combine(SaveFolder, "announcements.txt");
                if (File.Exists(path))
                    foreach (var line in File.ReadAllLines(path, Encoding.UTF8))
                    {
                        var p = line.Split(new[] { '|' }, 2);
                        if (p.Length == 2 && DateTime.TryParse(p[0], out DateTime d))
                            InstructorAnnouncements[d.Date] = Unescape(p[1]);
                    }

                // Events
                path = Path.Combine(SaveFolder, "events.txt");
                if (File.Exists(path))
                    foreach (var line in File.ReadAllLines(path, Encoding.UTF8))
                    {
                        var p = line.Split('|');
                        if (p.Length >= 7 && DateTime.TryParse(p[0], out DateTime d))
                        {
                            if (!Events.ContainsKey(d.Date))
                                Events[d.Date] = new List<CalendarEvent>();
                            Events[d.Date].Add(new CalendarEvent
                            {
                                Type = (EventType)int.Parse(p[1]),
                                Title = Unescape(p[2]),
                                Description = Unescape(p[3]),
                                StartTime = Unescape(p[4]),
                                EndTime = Unescape(p[5]),
                                Room = Unescape(p[6]),
                            });
                        }
                    }
            }
            catch { }
        }

        private static string Escape(string s) => (s ?? "").Replace("|", "\\pipe").Replace("\r\n", "\\n").Replace("\n", "\\n");
        private static string Unescape(string s) => (s ?? "").Replace("\\n", "\n").Replace("\\pipe", "|");
    }
}