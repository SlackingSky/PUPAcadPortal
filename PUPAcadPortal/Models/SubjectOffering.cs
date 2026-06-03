using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class SubjectOffering
{
    public string SubjectOfferingId { get; set; } = null!;

    public string SubjectId { get; set; } = null!;

    public int ProfessorId { get; set; }

    public string AcademicPeriodId { get; set; } = null!;

    public string Section { get; set; } = null!;

    public int MaxSlots { get; set; }

    public string Status { get; set; } = null!;

    public virtual AcademicPeriod AcademicPeriod { get; set; } = null!;

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

    public virtual ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();

    public virtual ICollection<CalendarEvent> CalendarEvents { get; set; } = new List<CalendarEvent>();

    public virtual ICollection<ClassSession> ClassSessions { get; set; } = new List<ClassSession>();

    public virtual ICollection<EnrollmentSubject> EnrollmentSubjects { get; set; } = new List<EnrollmentSubject>();

    public virtual ICollection<GradingCategory> GradingCategories { get; set; } = new List<GradingCategory>();

    public virtual ICollection<Module> Modules { get; set; } = new List<Module>();

    public virtual Professor Professor { get; set; } = null!;

    public virtual ICollection<RoomSchedule> RoomSchedules { get; set; } = new List<RoomSchedule>();

    public virtual Subject Subject { get; set; } = null!;
}
