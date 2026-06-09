using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class StudentGrade
{
    public int GradeId { get; set; }

    public string? StudentId { get; set; }

    public string? StudentName { get; set; }

    public string? SubjectCourse { get; set; }

    public float? MtAttendance { get; set; }

    public float? MtRecitation { get; set; }

    public float? MtSeatwork { get; set; }

    public float? MtAssignment { get; set; }

    public float? MtLongTests { get; set; }

    public float? MtMajorExam { get; set; }

    public float? FtAttendance { get; set; }

    public float? FtRecitation { get; set; }

    public float? FtSeatwork { get; set; }

    public float? FtAssignment { get; set; }

    public float? FtLongTests { get; set; }

    public float? FtMajorExam { get; set; }

    public string? GradeStatus { get; set; }

    public int? Released { get; set; }

    public int? StudentUserId { get; set; }

    public int? InstructorUserId { get; set; }
}
