using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public int UserId { get; set; }

    public string StudentNumber { get; set; } = null!;

    public string StudentType { get; set; } = null!;

    public int YearLevel { get; set; }

    public string Program { get; set; } = null!;

    public virtual ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual ICollection<StudentAccount> StudentAccounts { get; set; } = new List<StudentAccount>();

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();

    public virtual User User { get; set; } = null!;
}
