using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public int UserId { get; set; }

    public string StudentNumber { get; set; } = null!;

    public string? AdmissionType { get; set; }

    public int YearLevel { get; set; }

    public string Program { get; set; } = null!;

    public int CurriculumYear { get; set; }

    public string? AcademicStanding { get; set; }

    public virtual ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual ICollection<QrScanLog> QrScanLogs { get; set; } = new List<QrScanLog>();

    public virtual ICollection<StudentAccount> StudentAccounts { get; set; } = new List<StudentAccount>();

    public virtual ICollection<StudentDiscount> StudentDiscounts { get; set; } = new List<StudentDiscount>();

    public virtual ICollection<StudentHold> StudentHolds { get; set; } = new List<StudentHold>();

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();

    public virtual User User { get; set; } = null!;
}
