using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace PUPAcadPortal.Utils
{
    public static class Validators
    {
        private static void HighlightInvalidField(Control control)
        {
            if (control is TextBox txt)
            {
                txt.BackColor = Color.FromArgb(255, 220, 220);
            }
            else if (control is ComboBox cmb)
            {
                cmb.BackColor = Color.FromArgb(255, 220, 220);
            }
            else if (control is MaskedTextBox mtb)
            {
                mtb.BackColor = Color.FromArgb(255, 220, 220);
            }
            else if (control is DateTimePicker dtp)
            {
                dtp.BackColor = Color.FromArgb(255, 220, 220);
                dtp.CalendarForeColor = Color.Maroon;
                dtp.CalendarTitleBackColor = Color.FromArgb(255, 220, 220);
            }
        }

        private static int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        public static void ClearFieldHighlight(this TextBox textBox)
        {
            textBox.BackColor = Color.White;
            textBox.Font = new Font(textBox.Font, FontStyle.Regular);
        }

        public static void ClearFieldHighlight(this ComboBox cmb)
        {
            cmb.BackColor = Color.White;
            cmb.Font = new Font(cmb.Font, FontStyle.Regular);
        }

        public static void ClearFieldHighlight(this MaskedTextBox mtb)
        {
            mtb.BackColor = Color.White;
            mtb.Font = new Font(mtb.Font, FontStyle.Regular);
        }

        public static void ClearFieldHighlight(this DateTimePicker dtp)
        {
            dtp.BackColor = Color.White;
            dtp.Font = new Font(dtp.Font, FontStyle.Regular);
            dtp.CalendarForeColor = SystemColors.WindowText;
            dtp.CalendarTitleBackColor = SystemColors.Control;
        }

        public static bool IsDateOfBirthValid(this DateTimePicker dtp)
        {
            DateTime dob = dtp.Value;
            int age = CalculateAge(dob);

            // Check if date is in the future
            if (dob > DateTime.Today)
            {
                HighlightInvalidField(dtp);
                MessageBox.Show("Birth date cannot be in the future.", "Invalid Date",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Check if age is reasonable (at least 17, max 70 for college)
            if (age < 17)
            {
                HighlightInvalidField(dtp);
                MessageBox.Show($"Student must be at least 17 years old. Current age: {age}",
                    "Age Requirement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (age > 70)
            {
                HighlightInvalidField(dtp);
                MessageBox.Show($"Please check the birth date. Age: {age}",
                    "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Check for unreasonable year (before 1950)
            if (dob.Year < 1950)
            {
                HighlightInvalidField(dtp);
                MessageBox.Show("Please enter a valid birth date (year should be 1950 or later).",
                    "Invalid Year", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            ClearFieldHighlight(dtp);
            return true;
        }

        public static bool ValidateStudentRegistration(this Control control)
        {
            bool isValid = true;

            // Clear all previous highlights
            control.ClearAllHighlights();
            foreach (Control child in control.Controls)
            {
                if (child != null)
                {
                    if (child.Tag?.ToString() == "optional")
                        continue;
                    if (child is TextBox txt)
                    {
                        if (string.IsNullOrWhiteSpace(txt.Text))
                        {
                            HighlightInvalidField(txt);
                            isValid = false;
                        }
                    }
                    else if (child is ComboBox cmb)
                    {
                        if (cmb.SelectedIndex <= 0)
                        {
                            HighlightInvalidField(cmb);
                            isValid = false;
                        }
                    }
                    else if (child is MaskedTextBox mtb)
                    {
                        if (string.IsNullOrWhiteSpace(mtb.Text))
                        {
                            HighlightInvalidField(mtb);
                            isValid = false;
                        }
                    }
                    else if (child.HasChildren)
                    {
                        child.ValidateStudentRegistration();
                    }
                }
            }

            // Show summary message if invalid
            if (!isValid)
            {
                MessageBox.Show("Please fill in all required fields (marked with *).\n\n" +
                                "Required fields have been highlighted in light red.",
                                "Incomplete Information",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }

            return isValid;
        }

        private static void ClearAllHighlights(this Control control)
        {
            foreach (Control child in control.Controls)
            {
                if (child != null)
                {
                    if (child is TextBox txt) txt.ClearFieldHighlight();
                    else if (child is ComboBox cmb) cmb.ClearFieldHighlight();
                    else if (child is MaskedTextBox mtb) mtb.ClearFieldHighlight();
                    else if (child is DateTimePicker dtp) dtp.ClearFieldHighlight();

                    if (child.HasChildren)
                    {
                        child.ClearAllHighlights();
                    }
                }
            }
        }

        public static bool IsValidEmail(this string email)
        {
            email = email.Trim().ToLower();

            // Self explanatory naman, checks email pattern, prevents g@com
            string emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

            if (!System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern))
            {
                MessageBox.Show("Please enter a valid email address!\n\nExample: student@domain.com",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address != email)
                {
                    MessageBox.Show("Please enter a valid email address!", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("Please enter a valid email address!\n\nExample: student@domain.com",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
    }
}
