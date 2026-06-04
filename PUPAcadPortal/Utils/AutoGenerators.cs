using PUPAcadPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PUPAcadPortal.Utils
{
    public class AutoGenerators
    {
        public static string GenerateUniqueInstitutionalEmail(string firstName, string lastName, string? MiddleName = null)
        {
            return GenerateUniqueInstitutionalEmailBulk(firstName, lastName, MiddleName, new HashSet<string>());
        }

        public static string GenerateUniqueInstitutionalEmailBulk(string firstName, string lastName, string? MiddleName, HashSet<string> batchEmails)
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
                while (context.Users.Any(u => u.InstitutionalEmail == proposedEmail) || batchEmails.Contains(proposedEmail))
                {
                    proposedEmail = $"{baseIdentity}{collisionCounter}{domain}";
                    collisionCounter++;
                }
            }
            return proposedEmail;
        }

        public static int GetNextStudentSequence(int currentYear)
        {
            string yearPrefix = currentYear.ToString();
            using (var context = new AppDbContext())
            {
                var lastStudent = context.Students
                    .Where(s => s.StudentNumber.StartsWith(yearPrefix))
                    .OrderByDescending(s => s.StudentNumber)
                    .FirstOrDefault();

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

        public static string GenerateUniqueUsername(string firstName, string lastName, string? middleName = null)
        {
            char middleInitial = !string.IsNullOrEmpty(middleName) ? char.ToLower(middleName[0]) : '\0';
            string cleanFirst = Regex.Replace(firstName.ToLower(), @"[^a-z0-9]", "");
            string cleanLast = Regex.Replace(lastName.ToLower(), @"[^a-z0-9]", "");
            string baseUsername = $"{cleanFirst}{(middleInitial != '\0' ? middleInitial.ToString() : "")}{cleanLast}";
            string proposedUsername = baseUsername;
            int collisionCounter = 1;
            using (var context = new AppDbContext())
            {
                while (context.Users.Any(u => u.Username == proposedUsername))
                {
                    proposedUsername = $"{baseUsername}{collisionCounter}";
                    collisionCounter++;
                }
            }
            return proposedUsername;
        }
    }
}