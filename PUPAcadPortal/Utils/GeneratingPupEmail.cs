using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Utils
{
    public class GeneratingPupEmail
    {
        public string GeneratePupEmailFromName(string firstName, string lastName, string middleName)
        {
            // Clean and format the name parts
            string cleanFirstName = RemoveSpacesAndSpecialChars(firstName).ToLower();
            string cleanLastName = RemoveSpacesAndSpecialChars(lastName).ToLower();

            // Get middle initial (first letter of middle name, if exists)
            string middleInitial = "";
            if (!string.IsNullOrWhiteSpace(middleName))
            {
                middleInitial = middleName.Trim().ToLower();
                // Take only the first character of the middle name
                if (middleInitial.Length > 0)
                {
                    middleInitial = middleInitial[0].ToString();
                }
            }

            // Combine: firstname + middleinitial + lastname
            string emailUsername = cleanFirstName + middleInitial + cleanLastName;

            // Remove any remaining special characters (keep only letters)
            emailUsername = RemoveSpecialCharacters(emailUsername);

            return $"{emailUsername}@iskolarngbayan.pup.edu.ph";
        }

        public string RemoveSpacesAndSpecialChars(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "";

            // Remove spaces, periods, commas, etc.
            string cleaned = input.Replace(" ", "");
            cleaned = cleaned.Replace(".", "");
            cleaned = cleaned.Replace(",", "");
            cleaned = cleaned.Replace("'", "");
            cleaned = cleaned.Replace("-", "");
            cleaned = cleaned.Replace("ñ", "n");
            cleaned = cleaned.Replace("Ñ", "n");

            return cleaned;
        }

        public string RemoveSpecialCharacters(string input)
        {
            // Keep only letters (a-z)
            return new string(input.Where(c => char.IsLetter(c)).ToArray());
        }

    }
}
