using System;
using System.Collections.Generic;
using System.Drawing;

namespace PUPAcadPortal.Data
{
    // ──────────────────────────────────────────────────────────────────────────
    //  Extended event types for faculty calendar
    // ──────────────────────────────────────────────────────────────────────────
    public enum FacultyEventType
    {
        Class = 0,
        Activity = 1,
        Quiz = 2,
        LongQuiz = 3,
        Deadline = 4,
        Exam = 5,
        Consultation = 6,
        Holiday = 7,
        PersonalNote = 8,
        Cancelled = 9,
    }

    public class FacultyCalendarEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime Date { get; set; }
        public string StartTime { get; set; } = "";   // "08:00"
        public string EndTime { get; set; } = "";   // "09:30"
        public FacultyEventType Type { get; set; } = FacultyEventType.Class;
        public string Course { get; set; } = "";   // e.g. "CS101 – Data Structures"
        public string Room { get; set; } = "";
        public bool IsAllDay { get; set; } = false;
        public bool IsRecurring { get; set; } = false;
        public bool IsAutoSynced { get; set; } = false; // came from Activities/LMS
        public List<string> AttachedFiles { get; set; } = new();
        public bool IsOverdue => !IsAllDay
                                    && Date.Date < DateTime.Now.Date;

        // ── Visual helpers ───────────────────────────────────────────────────
        public Color GetColor() => Type switch
        {
            FacultyEventType.Class => Color.FromArgb(136, 14, 79),  // maroon
            FacultyEventType.Activity => Color.FromArgb(21, 101, 192),  // blue
            FacultyEventType.Quiz => Color.FromArgb(2, 119, 189),  // cyan-blue
            FacultyEventType.LongQuiz => Color.FromArgb(0, 96, 100),  // teal
            FacultyEventType.Deadline => Color.FromArgb(183, 28, 28),  // red
            FacultyEventType.Exam => Color.FromArgb(230, 81, 0),  // orange
            FacultyEventType.Consultation => Color.FromArgb(27, 125, 57),  // green
            FacultyEventType.Holiday => Color.FromArgb(74, 20, 140),  // purple
            FacultyEventType.PersonalNote => Color.FromArgb(66, 66, 66),  // dark gray
            FacultyEventType.Cancelled => Color.FromArgb(158, 158, 158),  // gray
            _ => Color.Gray,
        };

        public string GetTypeLabel() => Type switch
        {
            FacultyEventType.Class => "Class",
            FacultyEventType.Activity => "Activity",
            FacultyEventType.Quiz => "Quiz",
            FacultyEventType.LongQuiz => "Long Quiz",
            FacultyEventType.Deadline => "Deadline",
            FacultyEventType.Exam => "Exam",
            FacultyEventType.Consultation => "Consultation",
            FacultyEventType.Holiday => "Holiday",
            FacultyEventType.PersonalNote => "Note",
            FacultyEventType.Cancelled => "Cancelled",
            _ => "Event",
        };

        public string GetTypeIcon() => Type switch
        {
            FacultyEventType.Class => "📘",
            FacultyEventType.Activity => "📋",
            FacultyEventType.Quiz => "📝",
            FacultyEventType.LongQuiz => "📄",
            FacultyEventType.Deadline => "📌",
            FacultyEventType.Exam => "🎓",
            FacultyEventType.Consultation => "🩺",
            FacultyEventType.Holiday => "🏖",
            FacultyEventType.PersonalNote => "🗒",
            FacultyEventType.Cancelled => "🚫",
            _ => "📅",
        };

        public FacultyCalendarEvent Clone() => new()
        {
            Id = this.Id,
            Title = this.Title,
            Description = this.Description,
            Date = this.Date,
            StartTime = this.StartTime,
            EndTime = this.EndTime,
            Type = this.Type,
            Course = this.Course,
            Room = this.Room,
            IsAllDay = this.IsAllDay,
            IsRecurring = this.IsRecurring,
            IsAutoSynced = this.IsAutoSynced,
            AttachedFiles = new List<string>(this.AttachedFiles),
        };
    }
}