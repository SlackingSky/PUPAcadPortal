using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PUPAcadPortal.Utils
{
    public static class PasswordValidator
    {
        /// <summary>
        /// Validates a password based on standard security rules.
        /// Returns a tuple containing a boolean (IsValid) and the specific ErrorMessage.
        /// </summary>
        public static (bool IsValid, string ErrorMessage) Validate(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return (false, "Password cannot be empty.");

            if (password.Length < 8)
                return (false, "Password must be at least 8 characters long.");

            if (!Regex.IsMatch(password, @"[A-Z]"))
                return (false, "Password must contain at least one uppercase letter.");

            if (!Regex.IsMatch(password, @"[a-z]"))
                return (false, "Password must contain at least one lowercase letter.");

            if (!Regex.IsMatch(password, @"[0-9]"))
                return (false, "Password must contain at least one number.");

                return (false, "Password must contain at least one special character (e.g., @, #, $, !, %, *, ?, &).");

            return (true, string.Empty);
        }
    }
}