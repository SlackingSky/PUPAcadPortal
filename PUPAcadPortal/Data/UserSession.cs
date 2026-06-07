using System;

namespace PUPAcadPortal.Data
{
    /// <summary>
    /// Stores the currently logged-in user's session data as static fields.
    /// Call Login() on successful authentication and Logout() on sign-out.
    /// </summary>
    public static class UserSession
    {
        public static string? Username { get; private set; }
        public static int UserID { get; private set; }
        public static string? FirstName { get; private set; }
        public static string? LastName { get; private set; }
        public static string? Role { get; private set; }

        public static int? StudentID { get; private set; }
        public static int? ProfessorID { get; private set; }
        public static int? AdminID { get; private set; }

        public static bool IsLoggedIn =>
            !string.IsNullOrEmpty(Username);

        /// <summary>Returns "FirstName LastName", falling back to Username.</summary>
        public static string FullName =>
            string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName)
                ? (Username ?? "Unknown")
                : $"{FirstName} {LastName}".Trim();

        public static void Login(
            string username,
            int userID,
            string firstName,
            string lastName,
            string role,
            int? studentID = null,
            int? professorID = null,
            int? adminID = null)
        {
            Username = username;
            UserID = userID;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
            StudentID = studentID;
            ProfessorID = professorID;
            AdminID = adminID;
        }

        /// <summary>Clears all session data on sign-out.</summary>
        public static void Logout()
        {
            Username = null;
            UserID = 0;
            FirstName = null;
            LastName = null;
            Role = null;
            StudentID = null;
            ProfessorID = null;
            AdminID = null;
        }
    }
}