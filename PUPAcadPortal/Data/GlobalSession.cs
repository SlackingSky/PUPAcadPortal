using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Data
{
    public static class GlobalSession
    {
        public static string ActiveAcademicPeriod { get; set; }
        public static int ActiveSemesterIndex { get; set; }

        public static string ActiveSemesterName { get; set; }

        public static string ActiveSchoolYear { get; set; }
    }
}
