using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using PUPAcadPortal.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PUPAcadPortal.Services
{
    public class AccountRecoveryService
    {
        public async Task<(bool Success, string Message)> RequestPasswordResetAsync(string emailOrUsername)
        {
            using (var context = new AppDbContext())
            {
                var user = await context.Users.FirstOrDefaultAsync(u =>
                    u.PersonalEmail == emailOrUsername ||
                    u.InstitutionalEmail == emailOrUsername ||
                    u.Username == emailOrUsername);

                if (user == null)
                    return (false, "We could not find an account associated with that email or username.");

                if (string.IsNullOrEmpty(user.PersonalEmail) && string.IsNullOrEmpty(user.InstitutionalEmail))
                {
                    return (false, "Account found, but no email address is registered on file.");
                }

                string resetPin = new Random().Next(100000, 999999).ToString();

                user.ResetPasswordToken = BCrypt.Net.BCrypt.HashPassword(resetPin);
                user.ResetTokenExpiry = DateTime.Now.AddMinutes(15);
                await context.SaveChangesAsync();

                string targetEmail = !string.IsNullOrEmpty(user.PersonalEmail) ? user.PersonalEmail: user.InstitutionalEmail;

                string firstName = user.FirstName ?? "Student";

                await EmailService.SendPasswordResetEmailAsync(targetEmail, firstName, resetPin);

                string obscuredEmail = $"{targetEmail.Substring(0, 2)}***@{targetEmail.Split('@')[1]}";
                return (true, $"A 6-digit recovery code has been sent to {obscuredEmail}.");
            }
        }

        public async Task<bool> ValidateResetPinAsync(string emailOrUsername, string submittedPin)
        {
            using (var context = new AppDbContext())
            {
                var user = await context.Users.FirstOrDefaultAsync(u =>
                    u.PersonalEmail == emailOrUsername || u.InstitutionalEmail == emailOrUsername || u.Username == emailOrUsername);

                if (user == null || !BCrypt.Net.BCrypt.Verify(submittedPin, user.ResetPasswordToken)) return false;

                if (user.ResetTokenExpiry < DateTime.Now)
                {
                    return false;
                }

                return true;
            }
        }

        public async Task<(bool Success, string Message)> UpdatePasswordAsync(string emailOrUsername, string submittedPin, string newPassword)
        {
            using (var context = new AppDbContext())
            {
                var user = await context.Users.FirstOrDefaultAsync(u =>
                    u.PersonalEmail == emailOrUsername || u.InstitutionalEmail == emailOrUsername || u.Username == emailOrUsername);

                if (user == null || !BCrypt.Net.BCrypt.Verify(submittedPin, user.ResetPasswordToken) || user.ResetTokenExpiry < DateTime.Now)
                {
                    return (false, "Invalid or expired recovery code.");
                }

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

                user.ResetPasswordToken = null;
                user.ResetTokenExpiry = null;

                await context.SaveChangesAsync();

                return (true, "Your password has been successfully reset. You may now log in.");
            }
        }
    }
}