using CsvHelper.Configuration.Attributes;
using PUPAcadPortal.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Data
{
    public class StudentRegistrationData
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string? Suffix { get; set; }

        [TypeConverter(typeof(DateConverter))]
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address1 { get; set; }
        public string? Address2 { get; set; }
        public string Region { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Barangay { get; set; }
        public string PostalCode { get; set; }
        public string Program { get; set; }

        [Optional]
        public int CurriculumYear { get; set; }

        [Optional]
        public int YearLevel { get; set; }
        [Optional]
        public string StudentType { get; set; }
    }
}
