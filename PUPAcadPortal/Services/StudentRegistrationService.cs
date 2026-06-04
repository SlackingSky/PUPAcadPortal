using PUPAcadPortal.Models;
using PUPAcadPortal.Utils;
using PUPAcadPortal.Data;
using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Services
{
    public class StudentRegistrationService
    {
        public static bool IsTransferee { get; set; } = false;
        public static string CampusBranch { get; set; } = "SM";

        public async Task<Student> RegisterSingleStudent(StudentRegistrationData dto)
        {
            using (var context = new AppDbContext())
            {
                int currentYear = DateTime.Now.Year;
                int sequence = AutoGenerators.GetNextStudentSequence(currentYear);
                string fName = dto.FirstName.ToLower().Trim();
                string lName = dto.LastName.ToLower().Trim();

                bool isDuplicate = context.Users.Any(u =>
                    u.FirstName.ToLower() == fName &&
                    u.LastName.ToLower() == lName &&
                    u.Birthdate == dto.DateOfBirth);

                if (isDuplicate)
                {
                    throw new InvalidOperationException($"Registration failed. '{dto.FirstName} {dto.LastName}' born on {dto.DateOfBirth:MMMM dd, yyyy} is already registered.");
                }

                string studentNumber = AutoGenerators.FormatPupStudentNumber(currentYear, sequence, CampusBranch, IsTransferee);
                string officialEmail = AutoGenerators.GenerateUniqueInstitutionalEmail(dto.FirstName, dto.LastName, dto.MiddleName);
                string uniqueUsername = AutoGenerators.GenerateUniqueUsername(dto.FirstName, dto.LastName, dto.MiddleName);

                var newStudent = MapToEntities(dto, studentNumber, officialEmail, uniqueUsername);

                context.Students.Add(newStudent);
                await context.SaveChangesAsync();

                return newStudent;
            }
        }

        public async Task<(int Processed, List<StudentRegistrationData> SkippedRecords)> RegisterBulkStudents(List<StudentRegistrationData> dtos)
        {
            int processedCount = 0;
            var skippedRecords = new List<StudentRegistrationData>();
            using (var context = new AppDbContext())
            {
                int currentYear = DateTime.Now.Year;
                int currentSequence = AutoGenerators.GetNextStudentSequence(currentYear);
                HashSet<string> batchEmails = new HashSet<string>();
                var newStudentsToSave = new List<Student>();

                var existingUsers = context.Users.Select(u => new { u.FirstName, u.LastName, u.Birthdate }).ToList();
                HashSet<string> existingIdentities = new HashSet<string>();
                foreach (var u in existingUsers)
                {
                    string dob = u.Birthdate.HasValue ? u.Birthdate.Value.ToString("yyyyMMdd") : "20000101";
                    existingIdentities.Add($"{u.FirstName.ToLower()}|{u.LastName.ToLower()}|{dob}");
                }

                foreach (var dto in dtos)
                {
                    string studentIdentityKey = $"{dto.FirstName.ToLower()}|{dto.LastName.ToLower()}|{dto.DateOfBirth:yyyyMMdd}";
                    if (existingIdentities.Contains(studentIdentityKey))
                    {
                        skippedRecords.Add(dto);
                        continue;
                    }

                    existingIdentities.Add(studentIdentityKey);

                    string studentNumber = AutoGenerators.FormatPupStudentNumber(currentYear, currentSequence, CampusBranch);
                    string uniqueUsername = AutoGenerators.GenerateUniqueUsername(dto.FirstName, dto.LastName, dto.MiddleName);
                    currentSequence++;

                    string officialEmail = AutoGenerators.GenerateUniqueInstitutionalEmailBulk(dto.FirstName, dto.LastName, dto.MiddleName, batchEmails);
                    batchEmails.Add(officialEmail);

                    var newStudent = MapToEntities(dto, studentNumber, officialEmail, uniqueUsername);
                    newStudentsToSave.Add(newStudent);
                    processedCount++;

                    if (processedCount % 500 == 0)
                    {
                        context.Students.AddRange(newStudentsToSave);
                        await context.SaveChangesAsync();
                        newStudentsToSave.Clear();
                    }
                }

                if (newStudentsToSave.Count > 0)
                {
                    context.Students.AddRange(newStudentsToSave);
                    await context.SaveChangesAsync();
                }
            }
            return (processedCount, skippedRecords);
        }

        private Student MapToEntities(StudentRegistrationData dto, string studentNum, string officialEmail, string uniqueUsername)
        {
            string tempPassword = "PUP" + dto.DateOfBirth.Year.ToString();

            var newUser = new User
            {
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                Suffix = dto.Suffix,
                Birthdate = dto.DateOfBirth,
                PersonalEmail = dto.Email,
                InstitutionalEmail = officialEmail,
                ContactNumber = InfoFormatter.FormatPhilippinePhoneNumber(dto.Phone),
                AddressLine1 = dto.Address1,
                AddressLine2 = dto.Address2,
                Region = InfoFormatter.NormalizeRegion(dto.Region),
                Province = dto.Province.Trim().ToUpper(),
                CityMunicipality = InfoFormatter.NormalizeCity(dto.City),
                Barangay = dto.Barangay,
                PostalCode = dto.PostalCode,
                Username = uniqueUsername,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(tempPassword),
                RoleId = 2,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            var newStudent = new Student
            {
                User = newUser,
                StudentNumber = studentNum,
                StudentType =  dto.StudentType ?? "Regular",
                Program = dto.Program,
                YearLevel = dto.YearLevel > 0 ? dto.YearLevel : 1
            };

            return newStudent;
        }
    }
}