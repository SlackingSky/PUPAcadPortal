using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using PUPAcadPortal.Utils;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
            string tempPassword = "PUP" + dto.DateOfBirth.Year.ToString();
            int currentYear = DateTime.Now.Year;
            int sequence = await AutoGenerators.GetNextStudentSequence(currentYear);
            string fName = dto.FirstName.ToLower().Trim();
            string lName = dto.LastName.ToLower().Trim();

            bool isDuplicate = await context.Users.AnyAsync(u =>
                u.FirstName.ToLower() == fName &&
                u.LastName.ToLower() == lName &&
                u.Birthdate == dto.DateOfBirth &&
                u.RoleId == 2);

            if (isDuplicate)
            {
                throw new InvalidOperationException($"Registration failed. '{dto.FirstName} {dto.LastName}' born on {dto.DateOfBirth:MMMM dd, yyyy} is already registered.");
            }

            string studentNumber = AutoGenerators.FormatPupStudentNumber(currentYear, sequence, CampusBranch, IsTransferee);
            string officialEmail = await AutoGenerators.GenerateUniqueInstitutionalEmail(dto.FirstName, dto.LastName, dto.MiddleName);
            string uniqueUsername = await AutoGenerators.GenerateUniqueUsername(dto.FirstName, dto.LastName, dto.MiddleName);

            var newStudent = MapToEntities(dto, studentNumber, officialEmail, uniqueUsername, tempPassword);


            await EmailService.SendWelcomeEmailAsync(
            studentPersonalEmail: dto.Email,
            dto.FirstName,
            officialEmail,
            tempPassword,
            studentNumber,
            uniqueUsername
            );
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
                int currentSequence = await AutoGenerators.GetNextStudentSequence(currentYear);
                HashSet<string> batchEmails = new HashSet<string>();
                var newStudentsToSave = new List<Student>();

                var existingUsers = await context.Users.Select(u => new { u.FirstName, u.LastName, u.Birthdate }).ToListAsync();
                HashSet<string> existingIdentities = new HashSet<string>();
                foreach (var u in existingUsers)
                {
                    string dob = u.Birthdate.HasValue ? u.Birthdate.Value.ToString("yyyyMMdd") : "20000101";
                    existingIdentities.Add($"{u.FirstName.ToLower()}|{u.LastName.ToLower()}|{dob}");
                }

                foreach (var dto in dtos)
                {
                    string tempPassword = "PUP" + dto.DateOfBirth.Year.ToString();
                    string studentIdentityKey = $"{dto.FirstName.ToLower()}|{dto.LastName.ToLower()}|{dto.DateOfBirth:yyyyMMdd}";
                    if (existingIdentities.Contains(studentIdentityKey))
                    {
                        skippedRecords.Add(dto);
                        continue;
                    }

                    existingIdentities.Add(studentIdentityKey);

                    string studentNumber = AutoGenerators.FormatPupStudentNumber(currentYear, currentSequence, CampusBranch);
                    string uniqueUsername = await AutoGenerators.GenerateUniqueUsername(dto.FirstName, dto.LastName, dto.MiddleName);
                    currentSequence++;

                    string officialEmail = await AutoGenerators.GenerateUniqueInstitutionalEmailBulk(dto.FirstName, dto.LastName, dto.MiddleName, batchEmails);
                    batchEmails.Add(officialEmail);

                    var newStudent = MapToEntities(dto, studentNumber, officialEmail, uniqueUsername, tempPassword);

                    newStudentsToSave.Add(newStudent);
                    processedCount++;
                    await EmailService.SendWelcomeEmailAsync(
                    studentPersonalEmail: dto.Email,
                    dto.FirstName,
                    officialEmail,
                    tempPassword,
                    studentNumber,
                    uniqueUsername
                    );
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

        private Student MapToEntities(StudentRegistrationData dto, string studentNum, string officialEmail, string uniqueUsername, string tempPassword)
        {
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