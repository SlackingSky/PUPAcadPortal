using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using PUPAcadPortal.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PUPAcadPortal.Services
{
    public class ProfRegistrationService
    {
        public async Task<Professor> RegisterProf(ProfRegistrationData prd)
        {
            using (var context = new AppDbContext())
            {
                string tempPassword = "PUP" + prd.DateOfBirth.Year.ToString();
                int currentYear = DateTime.Now.Year;
                int sequence = await AutoGenerators.GetNextProfSequence(currentYear);
                string fName = prd.FirstName.ToLower().Trim();
                string lName = prd.LastName.ToLower().Trim();

                bool isDuplicate = await context.Users.AnyAsync(u =>
                    u.FirstName.ToLower() == fName &&
                    u.LastName.ToLower() == lName &&
                    u.Birthdate == prd.DateOfBirth &&
                    u.RoleId == 3);

                if (isDuplicate)
                {
                    throw new InvalidOperationException($"Registration failed. '{prd.FirstName} {prd.LastName}' born on {prd.DateOfBirth:MMMM dd, yyyy} is already registered.");
                }

                string employeeId = AutoGenerators.GenerateUniqueProfId(currentYear, sequence);
                string officialEmail = await AutoGenerators.GenerateUniqueInstitutionalEmail(prd.FirstName, prd.LastName, prd.MiddleName);
                string uniqueUsername = await AutoGenerators.GenerateUniqueUsername(prd.FirstName, prd.LastName, prd.MiddleName, isStudent: false);

                var newProf = MapToEntities(prd, employeeId, officialEmail, uniqueUsername, tempPassword);


                await EmailService.SendWelcomeEmailAsync(
                studentPersonalEmail: prd.Email,
                prd.FirstName,
                officialEmail,
                tempPassword,
                employeeId,
                uniqueUsername
                );
                context.Professors.Add(newProf);
                await context.SaveChangesAsync();

                return newProf;
            }
        }

        private Professor MapToEntities(ProfRegistrationData prd, string employeeId, string officialEmail, string uniqueUsername, string tempPassword)
        {
            var newUser = new User
            {
                FirstName = prd.FirstName,
                MiddleName = prd.MiddleName,
                LastName = prd.LastName,
                Suffix = prd.Suffix,
                Birthdate = prd.DateOfBirth,
                PersonalEmail = prd.Email,
                InstitutionalEmail = officialEmail,
                ContactNumber = InfoFormatter.FormatPhilippinePhoneNumber(prd.Phone),
                AddressLine1 = prd.Address1,
                AddressLine2 = prd.Address2,
                Region = InfoFormatter.NormalizeRegion(prd.Region),
                Province = prd.Province.Trim().ToUpper(),
                CityMunicipality = InfoFormatter.NormalizeCity(prd.City),
                Barangay = prd.Barangay,
                PostalCode = prd.PostalCode,
                Username = uniqueUsername,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(tempPassword),
                RoleId = 3,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            var newProf = new Professor
            {
                User = newUser,
                EmployeeId = employeeId,
                DepartmentId = prd.DepartmentId,
                EmploymentType = prd.EmploymentType,
                MaxLoad = prd.MaxLoad,
                HighestDegree = prd.HighestDegree,
                YearsOfExperience = prd.YearsOfExperience,
                EmploymentStatus = prd.EmploymentStatus,
                Rank = prd.Rank,
            };
            return newProf;
        }
    }
}
