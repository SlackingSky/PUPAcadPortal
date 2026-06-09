using System;
using System.ComponentModel;

namespace PUPAcadPortal.Data
{
    public class ScheduleRowDto
    {
        public string SubjectOfferingId { get; set; }
        public int? ScheduleId { get; set; }

        [DisplayName("Subject Code")]
        public string SubjectCode { get; set; }

        [DisplayName("Subject Title")]
        public string SubjectTitle { get; set; }

        public int Lab { get; set; }
        public int Lec { get; set; }
        public int TotalUnits { get; set; }
        public string Section { get; set; }
        public int MaxSlots { get; set; }

        public string Day { get; set; }

        [DisplayName("Start Time")]
        public TimeSpan? StartTime { get; set; }

        [DisplayName("End Time")]
        public TimeSpan? EndTime { get; set; }

        public int? RoomId { get; set; }

        public int? ProfessorId { get; set; }
        public int YearLevel { get; set; }
    }

    public class RoomDto
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
    }

    public class ProfessorDto
    {
        public int ProfessorId { get; set; }
        public string FullName { get; set; }
    }
}