using System;
using System.Collections.Generic;
using System.IO;

//
namespace PUPAcadPortal.Data
{
    public class StudentDataService
    {
        // Singleton pattern for easy access
        private static StudentDataService _instance;
        public static StudentDataService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new StudentDataService();
                return _instance;
            }
        }

        // Current student ID (you can set this after login)
        public string CurrentStudentId { get; set; } = "2024-00001";
        public string CurrentStudentName { get; set; } = "DemoStudent";

        // Enrollment status
        private bool _isEnrolled = false;

        // File path for persistent storage (temporary until database)
        private string GetEnrollmentFilePath()
        {
            return Path.Combine(Application.StartupPath, $"enrolled_{CurrentStudentId}.dat");
        }

        // Check if student is enrolled
        public bool IsStudentEnrolled()
        {
            // TEMPORARY: Check using local file
            // FUTURE: This will query the database
            return File.Exists(GetEnrollmentFilePath());
        }

        // Set enrollment status
        public async Task SetEnrollmentStatusAsync(bool enrolled)
        {
            // TEMPORARY: Save to local file
            // FUTURE: This will update the database
            string filePath = GetEnrollmentFilePath();

            if (enrolled)
            {
                await File.WriteAllTextAsync(filePath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                _isEnrolled = true;
            }
            else
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath); 
                }
                _isEnrolled = false;
            }
        }        

        // Get payment/assessment data
        public List<PaymentRecord> GetPaymentRecords(string semester = null)
        {
            var records = new List<PaymentRecord>();

            // TEMPORARY: Return mock data
            // FUTURE: This will query the database

            // Only return data if student is enrolled
            if (!IsStudentEnrolled() && semester == null)
            {
                // Return empty or placeholder data
                records.Add(new PaymentRecord
                {
                    ReferenceId = "N/A",
                    Description = "Complete enrollment to view payment details.",
                    Amount = 0,
                    DueDate = null,
                    Status = "Pending",
                    PaidDate = null
                });
                return records;
            }

            // Mock data for enrolled students
            records.Add(new PaymentRecord
            {
                ReferenceId = "2425-FirstSemester",
                Description = "Tuition & Fees - First Semester AY 2425\n(CASH - Free Education (20240903-000132))",
                Amount = 7294.00m,
                DueDate = null,
                Status = "Paid",
                PaidDate = new DateTime(2024, 9, 3)
            });

            records.Add(new PaymentRecord
            {
                ReferenceId = "2425-SecondSemester",
                Description = "Tuition & Fees - Second Semester AY 2425\n(CASH - Free Education (20250218-000283))",
                Amount = 6255.00m,
                DueDate = null,
                Status = "Paid",
                PaidDate = new DateTime(2025, 2, 18)
            });

            records.Add(new PaymentRecord
            {
                ReferenceId = "2526-FirstSemester",
                Description = "Tuition & Fees - First Semester AY 2526\n(CASH - Free Education (20250822-001891))",
                Amount = 8616.00m,
                DueDate = null,
                Status = "Paid",
                PaidDate = new DateTime(2025, 8, 22)
            });

            records.Add(new PaymentRecord
            {
                ReferenceId = "2526-SecondSemester",
                Description = "Tuition & Fees - Second Semester AY 2526\n(CASH - Free Education (20260211-006026))",
                Amount = 16566.00m,
                DueDate = null,
                Status = "Pending", // This would be updated after enrollment
                PaidDate = null
            });

            // Filter by semester if specified
            if (!string.IsNullOrEmpty(semester) && semester != "All")
            {
                records = records.FindAll(r => r.Description.Contains(semester));
            }

            return records;
        }

        // Get enrollment subjects
        public List<EnrollmentSubject> GetEnrolledSubjects()
        {
            var subjects = new List<EnrollmentSubject>();

            // TEMPORARY: Return mock data
            // FUTURE: This will query the database

            if (!IsStudentEnrolled())
                return subjects; // Return empty list if not enrolled

            // Mock enrolled subjects
            subjects.Add(new EnrollmentSubject
            {
                Code = "COMP 009",
                Title = "Object Oriented Programming",
                Units = 3,
                Schedule = "Wednesday 5:30 PM - 7:30 PM\nThursday 10:30 AM - 1:30 PM",
                Status = "Enrolled"
            });

            subjects.Add(new EnrollmentSubject
            {
                Code = "COMP 010",
                Title = "Information Management",
                Units = 3,
                Schedule = "Saturday 2:30 PM - 4:30 PM\nSaturday 5:00 PM - 8:00 PM",
                Status = "Enrolled"
            });

            return subjects;
        }
    }

    // Data model classes
    public class PaymentRecord
    {
        public string ReferenceId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; }
        public DateTime? PaidDate { get; set; }
    }

    public class EnrollmentSubject
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public int Units { get; set; }
        public string Schedule { get; set; }
        public string Status { get; set; }
    }
}