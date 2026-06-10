using PUPAcadPortal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Data
{
    public class CurriculumData
    {
        public string SubjectId { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public int LabUnits { get; set; }
        public int LecUnits { get; set; }
        public int Units { get; set; }
        public string Program { get; set; }
        public int YearLevel { get; set; }
        public int Semester { get; set; }
        public int RevisionYear { get; set; }
    }
}
