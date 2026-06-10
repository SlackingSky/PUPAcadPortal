using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.Utils
{
    public static class Validators
    {
        public static void AttachLiveErrorClearers(Control container)
        {
            foreach (Control child in container.Controls)
            {
                if (child == null || child.Tag?.ToString() == "optional")
                    continue;

                if (child is TextBox txt)
                {
                    txt.TextChanged += (s, e) => registrationErrorProvider.SetError(txt, "");
                }
                else if (child is ComboBox cmb)
                {
                    cmb.SelectedIndexChanged += (s, e) => registrationErrorProvider.SetError(cmb, "");
                }
                else if (child is MaskedTextBox mtb)
                {
                    mtb.TextChanged += (s, e) => registrationErrorProvider.SetError(mtb, "");
                }
                else if (child is DateTimePicker dtp)
                {
                    dtp.ValueChanged += (s, e) => registrationErrorProvider.SetError(dtp, "");
                }
                else if (child.HasChildren)
                {
                    AttachLiveErrorClearers(child);
                }
            }
        }

        private static ErrorProvider registrationErrorProvider = new ErrorProvider()
        {
            BlinkStyle = ErrorBlinkStyle.NeverBlink
        };

        public static void ClearErrors()
        {
            registrationErrorProvider.Clear();
        }

        public static bool ValidateRegistration(this Control control)
        {
            registrationErrorProvider.Clear();

            bool isValid = CheckControlsValidity(control);

            if (!isValid)
            {
                MessageBox.Show("Please fill in all required fields.\nHover over the red icons for details.",
                                "Incomplete Information",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }

            return isValid;
        }

        private static bool CheckControlsValidity(Control container)
        {
            bool isValid = true;

            foreach (Control child in container.Controls)
            {
                if (child == null || child.Tag?.ToString() == "optional")
                    continue;

                if (child is TextBox txt)
                {
                    if (string.IsNullOrWhiteSpace(txt.Text))
                    {
                        registrationErrorProvider.SetError(txt, "This text field is required.");
                        isValid = false;
                    }
                }
                else if (child is ComboBox cmb)
                {
                    if (cmb.SelectedIndex < 0)
                    {
                        registrationErrorProvider.SetError(cmb, "Please select an option from the dropdown.");
                        isValid = false;
                    }
                    if (cmb.SelectedItem.ToString() == "None")
                    {
                        registrationErrorProvider.SetError(cmb, "You cannot register a student without a curriculum");
                        isValid = false;
                    }
                }
                else if (child is MaskedTextBox mtb)
                {
                    if (!mtb.MaskCompleted)
                    {
                        registrationErrorProvider.SetError(mtb, "Please complete the required format.");
                        isValid = false;
                    }
                }
                else if (child is DateTimePicker dtp)
                {
                    if (!dtp.IsDateOfBirthValid())
                        isValid = false;
                }
                else if (child.HasChildren)
                {
                    if (!CheckControlsValidity(child))
                    {
                        isValid = false;
                    }
                }
            }

            return isValid;
        }

        private static int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        public static bool IsDateOfBirthValid(this DateTimePicker dtp)
        {
            DateTime dob = dtp.Value;
            int age = CalculateAge(dob);

            if (dob > DateTime.Today)
            {
                registrationErrorProvider.SetError(dtp, "Birth date cannot be in the future.");
                return false;
            }

            if (dob.Year < 1950)
            {
                registrationErrorProvider.SetError(dtp, "Year must be 1950 or later.");
                return false;
            }

            if (age > 70)
            {
                registrationErrorProvider.SetError(dtp, "Invalid age for registration. Maximum age is 70.");
                return false;
            }

            bool isStudent = (dtp.Tag?.ToString() == "student");

            if (isStudent)
            {
                if (age < 17)
                {
                    registrationErrorProvider.SetError(dtp, "Student must be at least 17 years old.");
                    return false;
                }
            }
            else
            {
                if (age < 21)
                {
                    registrationErrorProvider.SetError(dtp, $"Age must be between 21-70 years old. Current age: {age}");
                    return false;
                }
            }

            registrationErrorProvider.SetError(dtp, string.Empty);
            return true;
        }

        public static bool IsValidEmail(this TextBox textBox)
        {
            string email = textBox.Text;
            email = email.Trim().ToLower();

            // Self explanatory naman, checks email pattern, prevents g@com
            string emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

            if (!System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern))
            {
                registrationErrorProvider.SetError(textBox, "Please enter a valid email address! Example: user@domain.com");
                return false;
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address != email)
                {
                    registrationErrorProvider.SetError(textBox, "Please enter a valid email address! Example: user@domain.com");
                    return false;
                }
            }
            catch
            {
                registrationErrorProvider.SetError(textBox, "Please enter a valid email address! Example: user@domain.com");
                return false;
            }
            return true;
        }
    }
}