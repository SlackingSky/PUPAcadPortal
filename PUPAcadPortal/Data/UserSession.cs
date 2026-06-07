using System;
using System.Collections.Generic;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace PUPAcadPortal.Data
{
    public static class UserSession
    {
        // Defines who is currently logged in
        public static string Username {  get; private set; }
        public static int UserID { get; private set; }
        public static string? FirstName { get; private set; }
        public static string? LastName { get; private set; }
        public static string? Role { get; private set; }
        public static int? StudentID { get; private set; }
        public static int? ProfessorID { get; private set; }
        public static int? AdminID { get; private set; }
        public static string FullName => $"{FirstName} {LastName}";

        public static void Login(string username, int userID, string firstName, string lastName, string role, int? studentID = null, int? professorID = null, int? adminID = null)
        {
            Username = username ;
            UserID = userID ;
            FirstName = firstName ;
            LastName = lastName ;
            Role = role ;
            StudentID = studentID ;
            ProfessorID = professorID ;
            AdminID = adminID ;
        }

        public static void Logout()
        {
            UserID = 0;
            Username = null;
            Role = null;
            FirstName = null;
            LastName = null;
            StudentID = null;
            ProfessorID = null;
            AdminID = null;
        }

        public static void UpdateProfile(string username, int userId, string firstName, string lastName, string role)
        {
            Username= username ;
            UserID= userId ;
            FirstName= firstName ;
            LastName= lastName ;
            Role = role;
        }
    }
}
