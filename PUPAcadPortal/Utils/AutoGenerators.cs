using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PUPAcadPortal.Utils
{
    public class AutoGenerators
    {
        public static async Task<string> GenerateUniqueInstitutionalEmail(string firstName, string lastName, string? MiddleName = null)
        {
            return await GenerateUniqueInstitutionalEmailBulk(firstName, lastName, MiddleName, new HashSet<string>());
        }

        public static async Task<string> GenerateUniqueInstitutionalEmailBulk(string firstName, string lastName, string? MiddleName, HashSet<string> batchEmails)
        {
            char middleInitial = !string.IsNullOrEmpty(MiddleName) ? char.ToLower(MiddleName[0]) : '\0';
            string cleanFirst = Regex.Replace(firstName.ToLower(), @"[^a-z0-9]", "");
            string cleanLast = Regex.Replace(lastName.ToLower(), @"[^a-z0-9]", "");
            string domain = "@iskolarngbayan.pup.edu.ph";

            string baseIdentity = $"{cleanFirst}{(middleInitial != '\0' ? middleInitial.ToString() : "")}{cleanLast}";
            string proposedEmail = $"{baseIdentity}{domain}";

            int collisionCounter = 1;

            using (var context = new AppDbContext())
            {
                while (await context.Users.AnyAsync(u => u.InstitutionalEmail == proposedEmail) || batchEmails.Contains(proposedEmail))
                {
                    proposedEmail = $"{baseIdentity}{collisionCounter}{domain}";
                    collisionCounter++;
                }
            }
            return proposedEmail;
        }

        public static async Task<int> GetNextStudentSequence(int currentYear)
        {
            string yearPrefix = currentYear.ToString();
            using (var context = new AppDbContext())
            {
                var lastStudent = await context.Students
                    .Where(s => s.StudentNumber.StartsWith(yearPrefix))
                    .OrderByDescending(s => s.StudentNumber)
                    .FirstOrDefaultAsync();

                if (lastStudent != null)
                {
                    string[] parts = lastStudent.StudentNumber.Split('-');
                    if (parts.Length >= 2 && int.TryParse(parts[1], out int lastSequence))
                    {
                        return lastSequence + 1;
                    }
                }
                return 1;
            }
        }

        public static string FormatPupStudentNumber(int year, int sequence, string campusCode = "SM", bool isTransferee = false)
        {
            return $"{year}-{sequence:D5}-{campusCode}-{(isTransferee ? "1" : "0")}";
        }

        public static async Task<string> GenerateUniqueUsername(string firstName, string lastName, string? middleName = null, bool isStudent = true)
        {
            char middleInitial = !string.IsNullOrEmpty(middleName) ? char.ToLower(middleName[0]) : '\0';
            string cleanFirst = Regex.Replace(firstName.ToLower(), @"[^a-z0-9]", "");
            string cleanLast = Regex.Replace(lastName.ToLower(), @"[^a-z0-9]", "");
            string baseUsername = $"{(!isStudent ? "prof" : "")}{cleanFirst}{(middleInitial != '\0' ? middleInitial.ToString() : "")}{cleanLast}";
            string proposedUsername = baseUsername;
            int collisionCounter = 1;
            using (var context = new AppDbContext())
            {
                while (await context.Users.AnyAsync(u => u.Username == proposedUsername))
                {
                    proposedUsername = $"{baseUsername}{collisionCounter}";
                    collisionCounter++;
                }
            }
            return proposedUsername;
        }

        public static string GenerateUniqueProfId(int year, int sequence)
        {
            return $"PROF-{year}-{sequence:D5}";
        }

        public static async Task<int> GetNextProfSequence(int currentYear)
        {
            string yearPrefix = currentYear.ToString();
            using (var context = new AppDbContext())
            {
                var lastStudent = await context.Professors
                    .Where(p => p.EmployeeId.StartsWith($"PROF-{yearPrefix}"))
                    .OrderByDescending(s => s.EmployeeId)
                    .FirstOrDefaultAsync();

                if (lastStudent != null)
                {
                    string[] parts = lastStudent.EmployeeId.Split('-');
                    if (parts.Length >= 2 && int.TryParse(parts[2], out int lastSequence))
                    {
                        return lastSequence + 1;
                    }
                }
                return 1;
            }
        }
    }
}