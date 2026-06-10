using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;
using CsvHelper.Configuration.Attributes;
using PUPAcadPortal.Utils;

namespace PUPAcadPortal.Data
{
    public class ProfRegistrationData
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string? Suffix { get; set; }
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
        public int DepartmentId { get; set; }
        public string EmploymentType { get; set; }
        public int MaxLoad {  get; set; }
        public string HighestDegree { get; set; }
        public int YearsOfExperience { get; set; }
        public string EmploymentStatus { get; set; }
        public string Rank { get; set; }
    }
}
