using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace PUPAcadPortal.Data
{
    /// <summary>
    /// Central in-memory store for the Faculty Calendar.
    /// Handles persistence, LMS auto-sync, and notification checks.
    /// </summary>
    public static class FacultyCalendarData
    {
        // ── Storage ─────────────────────────────────────────────────────────
        private static readonly string DataPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                         "PUPAcadPortal", "FacultyCalendar.json");

        // Main event dictionary: Date → list of events
        private static Dictionary<DateTime, List<FacultyCalendarEvent>> _events = new();

        // Notification store
        private static readonly List<FacultyNotification> _notifications = new();

        // Auto-synced course schedules (populated from LMS)
        private static readonly List<FacultyCalendarEvent> _lmsSynced = new();

        public static int CurrentYear { get; set; } = DateTime.Now.Year;
        public static int CurrentMonth { get; set; } = DateTime.Now.Month;

        // ── Holidays (Philippine) ────────────────────────────────────────────
        public static Dictionary<DateTime, string> Holidays { get; } =
            BuildPhilippineHolidays(DateTime.Now.Year);

        private static Dictionary<DateTime, string> BuildPhilippineHolidays(int year)
        {
            return new Dictionary<DateTime, string>
            {
                [new DateTime(year, 1, 1)] = "New Year's Day",
                [new DateTime(year, 4, 9)] = "Araw ng Kagitingan",
                [new DateTime(year, 5, 1)] = "Labor Day",
                [new DateTime(year, 6, 12)] = "Independence Day",
                [new DateTime(year, 8, 26)] = "National Heroes Day",
                [new DateTime(year, 11, 1)] = "All Saints' Day",
                [new DateTime(year, 11, 30)] = "Bonifacio Day",
                [new DateTime(year, 12, 25)] = "Christmas Day",
                [new DateTime(year, 12, 30)] = "Rizal Day",
            };
        }

        // ── CRUD ─────────────────────────────────────────────────────────────
        public static void AddEvent(DateTime date, FacultyCalendarEvent ev)
        {
            var key = date.Date;
            if (!_events.ContainsKey(key)) _events[key] = new List<FacultyCalendarEvent>();
            ev.Date = key;
            _events[key].Add(ev);
            RebuildNotifications();
            SaveData();
        }

        public static void UpdateEvent(FacultyCalendarEvent updated)
        {
            foreach (var kv in _events)
            {
                var found = kv.Value.FirstOrDefault(e => e.Id == updated.Id);
                if (found != null)
                {
                    kv.Value.Remove(found);
                    if (kv.Value.Count == 0) _events.Remove(kv.Key);

                    var newKey = updated.Date.Date;
                    if (!_events.ContainsKey(newKey)) _events[newKey] = new();
                    _events[newKey].Add(updated);
                    RebuildNotifications();
                    SaveData();
                    return;
                }
            }
        }

        public static void RemoveEvent(Guid id)
        {
            foreach (var kv in _events)
            {
                var found = kv.Value.FirstOrDefault(e => e.Id == id);
                if (found != null)
                {
                    kv.Value.Remove(found);
                    if (kv.Value.Count == 0) _events.Remove(kv.Key);
                    RebuildNotifications();
                    SaveData();
                    return;
                }
            }
        }

        /// <summary>Move an event to a new date (drag-drop support).</summary>
        public static void MoveEvent(Guid id, DateTime newDate)
        {
            FacultyCalendarEvent? target = null;
            foreach (var kv in _events)
            {
                target = kv.Value.FirstOrDefault(e => e.Id == id);
                if (target != null)
                {
                    kv.Value.Remove(target);
                    if (kv.Value.Count == 0) _events.Remove(kv.Key);
                    break;
                }
            }
            if (target == null) return;
            target.Date = newDate.Date;
            var key = newDate.Date;
            if (!_events.ContainsKey(key)) _events[key] = new();
            _events[key].Add(target);
            RebuildNotifications();
            SaveData();
        }

        // ── Queries ──────────────────────────────────────────────────────────
        public static List<FacultyCalendarEvent> GetEventsForDate(DateTime date)
        {
            var key = date.Date;
            var list = _events.ContainsKey(key) ? _events[key].ToList() : new List<FacultyCalendarEvent>();
            list.AddRange(_lmsSynced.Where(e => e.Date.Date == key));
            return list.OrderBy(e => e.StartTime).ToList();
        }

        public static List<FacultyCalendarEvent> GetEventsForRange(DateTime from, DateTime to) =>
            _events.Where(kv => kv.Key >= from.Date && kv.Key <= to.Date)
                   .SelectMany(kv => kv.Value)
                   .Concat(_lmsSynced.Where(e => e.Date.Date >= from.Date && e.Date.Date <= to.Date))
                   .OrderBy(e => e.Date).ThenBy(e => e.StartTime)
                   .ToList();

        public static List<FacultyCalendarEvent> GetEventsForWeek(DateTime weekStart)
            => GetEventsForRange(weekStart, weekStart.AddDays(6));

        public static List<(DateTime Date, FacultyCalendarEvent Ev)> GetUpcoming(int count = 8)
        {
            var now = DateTime.Now.Date;
            return _events
                .Where(kv => kv.Key >= now)
                .OrderBy(kv => kv.Key)
                .SelectMany(kv => kv.Value.Select(ev => (Date: kv.Key, Ev: ev)))
                .Concat(_lmsSynced.Where(e => e.Date.Date >= now)
                                  .Select(e => (Date: e.Date.Date, Ev: e)))
                .OrderBy(t => t.Date).ThenBy(t => t.Ev.StartTime)
                .Take(count)
                .ToList();
        }

        public static List<FacultyCalendarEvent> GetOverdue()
        {
            var now = DateTime.Now.Date;
            return _events
                .Where(kv => kv.Key < now)
                .SelectMany(kv => kv.Value)
                .Where(e => e.Type is FacultyEventType.Deadline or FacultyEventType.Exam
                                                                 or FacultyEventType.LongQuiz)
                .ToList();
        }

        // ── Search / Filter ──────────────────────────────────────────────────
        public static List<FacultyCalendarEvent> Search(
            string? query,
            FacultyEventType? type,
            string? course,
            DateTime? from,
            DateTime? to)
        {
            IEnumerable<FacultyCalendarEvent> all =
                _events.SelectMany(kv => kv.Value)
                       .Concat(_lmsSynced);

            if (!string.IsNullOrWhiteSpace(query))
            {
                var q = query.Trim().ToLower();
                all = all.Where(e => e.Title.ToLower().Contains(q)
                                  || e.Description.ToLower().Contains(q)
                                  || e.Course.ToLower().Contains(q));
            }
            if (type.HasValue) all = all.Where(e => e.Type == type.Value);
            if (!string.IsNullOrWhiteSpace(course))
                all = all.Where(e => e.Course == course);
            if (from.HasValue) all = all.Where(e => e.Date.Date >= from.Value.Date);
            if (to.HasValue) all = all.Where(e => e.Date.Date <= to.Value.Date);

            return all.OrderBy(e => e.Date).ThenBy(e => e.StartTime).ToList();
        }

        public static List<string> GetAllCourses() =>
            _events.SelectMany(kv => kv.Value)
                   .Concat(_lmsSynced)
                   .Select(e => e.Course)
                   .Where(c => !string.IsNullOrWhiteSpace(c))
                   .Distinct()
                   .OrderBy(c => c)
                   .ToList();

        // ── LMS Auto-Sync ────────────────────────────────────────────────────
        /// <summary>
        /// Seed auto-synced events from the LMS (activities, quizzes, assessments).
        /// Call this after loading LMS data. In production, replace with real data queries.
        /// </summary>
        public static void SyncFromLMS()
        {
            _lmsSynced.Clear();
            // Sample auto-synced items — replace with real LMS database reads
            var today = DateTime.Now.Date;
            _lmsSynced.AddRange(new[]
            {
                new FacultyCalendarEvent
                {
                    Title        = "CS101 – Midterm Exam",
                    Date         = today.AddDays(5),
                    StartTime    = "08:00",
                    EndTime      = "10:00",
                    Type         = FacultyEventType.Exam,
                    Course       = "CS101 – Data Structures",
                    Room         = "Room 302",
                    IsAutoSynced = true,
                    Description  = "Coverage: Chapters 1–6",
                },
                new FacultyCalendarEvent
                {
                    Title        = "MATH201 – Problem Set 3 Due",
                    Date         = today.AddDays(3),
                    Type         = FacultyEventType.Deadline,
                    Course       = "MATH201 – Calculus II",
                    IsAutoSynced = true,
                    IsAllDay     = true,
                    Description  = "Upload to LMS by midnight.",
                },
                new FacultyCalendarEvent
                {
                    Title        = "CS101 – Long Quiz 2",
                    Date         = today.AddDays(7),
                    StartTime    = "10:00",
                    EndTime      = "11:00",
                    Type         = FacultyEventType.LongQuiz,
                    Course       = "CS101 – Data Structures",
                    Room         = "Room 302",
                    IsAutoSynced = true,
                },
                new FacultyCalendarEvent
                {
                    Title        = "PHYS101 – Lab Activity",
                    Date         = today.AddDays(2),
                    StartTime    = "13:00",
                    EndTime      = "15:00",
                    Type         = FacultyEventType.Activity,
                    Course       = "PHYS101 – Physics I",
                    Room         = "Lab 1",
                    IsAutoSynced = true,
                },
            });
            RebuildNotifications();
        }

        // ── Notifications ────────────────────────────────────────────────────
        public static IReadOnlyList<FacultyNotification> Notifications => _notifications.AsReadOnly();

        private static void RebuildNotifications()
        {
            _notifications.Clear();
            var now = DateTime.Now;
            var all = GetUpcoming(30);

            foreach (var item in all)
            {
                int daysLeft = (item.Date.Date - now.Date).Days;
                if (daysLeft <= 3)
                    _notifications.Add(new FacultyNotification(item.Ev, item.Date, daysLeft));
            }
            // Overdue
            foreach (var ev in GetOverdue())
                _notifications.Add(new FacultyNotification(ev, ev.Date, -1));
        }

        // ── Persistence ──────────────────────────────────────────────────────
        public static void LoadData()
        {
            try
            {
                if (!File.Exists(DataPath)) { SeedDemoData(); SyncFromLMS(); return; }
                var json = File.ReadAllText(DataPath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var flat = JsonSerializer.Deserialize<List<FacultyCalendarEvent>>(json, options) ?? new();
                _events.Clear();
                foreach (var ev in flat)
                {
                    var key = ev.Date.Date;
                    if (!_events.ContainsKey(key)) _events[key] = new();
                    _events[key].Add(ev);
                }
                SyncFromLMS();
            }
            catch { SeedDemoData(); SyncFromLMS(); }
        }

        public static void SaveData()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(DataPath)!);
                var flat = _events.SelectMany(kv => kv.Value).ToList();
                var options = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(DataPath, JsonSerializer.Serialize(flat, options));
            }
            catch { /* silently fail in demo */ }
        }

        private static void SeedDemoData()
        {
            _events.Clear();
            var today = DateTime.Now.Date;

            void Add(FacultyCalendarEvent ev) { AddEventNoSave(ev.Date, ev); }

            Add(new() { Title = "CS101 – Lecture", Date = today, StartTime = "08:00", EndTime = "09:30", Type = FacultyEventType.Class, Course = "CS101 – Data Structures", Room = "Room 302" });
            Add(new() { Title = "MATH201 – Lecture", Date = today, StartTime = "10:00", EndTime = "11:30", Type = FacultyEventType.Class, Course = "MATH201 – Calculus II", Room = "Room 110" });
            Add(new() { Title = "Office Hours / Consultation", Date = today, StartTime = "13:00", EndTime = "15:00", Type = FacultyEventType.Consultation, Course = "General" });
            Add(new() { Title = "CS101 – Quiz 1", Date = today.AddDays(1), StartTime = "08:00", EndTime = "08:30", Type = FacultyEventType.Quiz, Course = "CS101 – Data Structures", Room = "Room 302" });
            Add(new() { Title = "Grading Deadline – Activity 2", Date = today.AddDays(2), Type = FacultyEventType.Deadline, Course = "CS101 – Data Structures", IsAllDay = true });
            Add(new() { Title = "Department Meeting", Date = today.AddDays(3), StartTime = "15:00", EndTime = "17:00", Type = FacultyEventType.Activity, Course = "General", Room = "Faculty Room" });
            Add(new() { Title = "CS101 – Long Quiz 1", Date = today.AddDays(4), StartTime = "08:00", EndTime = "09:00", Type = FacultyEventType.LongQuiz, Course = "CS101 – Data Structures", Room = "Room 302" });
            Add(new() { Title = "Research Submission", Date = today.AddDays(5), Type = FacultyEventType.Deadline, Course = "General", IsAllDay = true, Description = "Submit to faculty research office." });
            Add(new() { Title = "Personal: Buy textbooks", Date = today.AddDays(6), Type = FacultyEventType.PersonalNote, IsAllDay = true });
        }

        private static void AddEventNoSave(DateTime date, FacultyCalendarEvent ev)
        {
            var key = date.Date;
            if (!_events.ContainsKey(key)) _events[key] = new();
            ev.Date = key;
            _events[key].Add(ev);
        }
    }

    // ── Notification model ───────────────────────────────────────────────────
    public class FacultyNotification
    {
        public FacultyCalendarEvent Event { get; }
        public DateTime Date { get; }
        public int DaysLeft { get; }   // -1 = overdue
        public bool IsOverdue => DaysLeft < 0;
        public bool IsToday => DaysLeft == 0;

        public FacultyNotification(FacultyCalendarEvent ev, DateTime date, int daysLeft)
        {
            Event = ev;
            Date = date;
            DaysLeft = daysLeft;
        }

        public string GetLabel()
        {
            if (IsOverdue) return "Overdue";
            if (IsToday) return "Today";
            if (DaysLeft == 1) return "Tomorrow";
            return $"In {DaysLeft} days";
        }
    }
}