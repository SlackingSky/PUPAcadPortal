using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PUPAcadPortal.PHAddress;

namespace PUPAcadPortal.PortalContents.Admin
{
    public partial class RegisterStudentsContentAdmin : UserControl
    {
        private bool isResetingForm = false;
        private List<string[]> studentList = new List<string[]>();
        private PHAddressFields phAddressFields = new PHAddressFields();
        public RegisterStudentsContentAdmin()
        {
            InitializeComponent();

            // FIRST create dynamic controls
            AddSuffixComboBox();

            // THEN set fonts (so they apply to all controls including dynamically created ones)
            SetRegistrationFontsTo12pt();

            // THEN mark required fields
            MarkRequiredFields();

            // THEN attach real-time validation
            AttachRealTimeValidation();
        }

        // Helper method to find controls by name
        private Control GetControl(string name)
        {
            // Search in the registration container
            foreach (Control ctrl in pnlStudentRegistrationContainer.Controls)
            {
                if (ctrl.Name == name) return ctrl;

                // Search in nested panels
                if (ctrl.HasChildren)
                {
                    foreach (Control child in ctrl.Controls)
                    {
                        if (child.Name == name) return child;
                    }
                }
            }
            return null;
        }

        private void AddSuffixComboBox()
        {
            // Position suffix to the right of Last Name field
            // Last Name is at X=439, Y=173, Width=437
            int yPosition = txtRSStudentLastName.Location.Y;  // Same Y as Last Name (173)
            int xPosition = txtRSStudentLastName.Location.X + txtRSStudentLastName.Width + 10; // 439 + 437 + 10 = 886

            // But ensure it doesn't go out of bounds - adjust if needed
            if (xPosition + 120 > pnlStudentRegistrationContainer.Width)
            {
                xPosition = txtRSStudentLastName.Location.X + txtRSStudentLastName.Width - 80;
            }

            System.Windows.Forms.Label lblSuffix = new System.Windows.Forms.Label();
            lblSuffix.Text = "Suffix:";
            lblSuffix.Location = new Point(xPosition, yPosition);
            lblSuffix.Size = new Size(60, 30);
            lblSuffix.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

            ComboBox cmbSuffix = new ComboBox();
            cmbSuffix.Name = "cmbSuffix";
            cmbSuffix.Location = new Point(xPosition + 55, yPosition);
            cmbSuffix.Size = new Size(70, 29);
            cmbSuffix.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            cmbSuffix.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSuffix.Items.AddRange(new string[] { "", "Jr.", "Sr.", "I", "II", "III", "IV" });

            pnlStudentRegistrationContainer.Controls.Add(lblSuffix);
            pnlStudentRegistrationContainer.Controls.Add(cmbSuffix);
        }

        private int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        private bool ValidateDateOfBirth()
        {
            DateTime dob = dtpRSStudentBirthDate.Value;
            int age = CalculateAge(dob);

            // Check if date is in the future
            if (dob > DateTime.Today)
            {
                HighlightInvalidField(dtpRSStudentBirthDate);
                MessageBox.Show("Birth date cannot be in the future.", "Invalid Date",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Check if age is reasonable (at least 17, max 70 for college)
            if (age < 17)
            {
                HighlightInvalidField(dtpRSStudentBirthDate);
                MessageBox.Show($"Student must be at least 17 years old. Current age: {age}",
                    "Age Requirement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (age > 70)
            {
                HighlightInvalidField(dtpRSStudentBirthDate);
                MessageBox.Show($"Please check the birth date. Age: {age}",
                    "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Check for unreasonable year (before 1950)
            if (dob.Year < 1950)
            {
                HighlightInvalidField(dtpRSStudentBirthDate);
                MessageBox.Show("Please enter a valid birth date (year should be 1950 or later).",
                    "Invalid Year", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            ClearFieldHighlight(dtpRSStudentBirthDate);
            return true;
        }

        private void btnStudentRegistration_Click(object sender, EventArgs e)
        {

            // Validate all required fields
            if (!ValidateStudentRegistration())
                return;

            // Check if student ID already exists
            if (IsStudentIdExists(txtRSStudentID.Text.Trim()))
            {
                MessageBox.Show("Student ID already exists. Please check the ID number.",
                    "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Collect all student information
            string studentId = txtRSStudentID.Text.Trim();
            string firstName = txtRSStudentFirstName.Text.Trim();
            string lastName = txtRSStudentLastName.Text.Trim();
            string middleName = txtRSStudentMiddleName.Text.Trim();
            ComboBox cmbSuffix = GetControl("cmbSuffix") as ComboBox;
            string suffix = cmbSuffix?.SelectedItem?.ToString() ?? "";

            // Format full name
            string fullName = $"{lastName}, {firstName}";
            if (!string.IsNullOrWhiteSpace(middleName))
                fullName += $" {middleName}";
            if (!string.IsNullOrWhiteSpace(suffix) && suffix != "")
                fullName += $" {suffix}";

            // Personal email (from enrollment form, entered by admin)
            string personalEmail = txtRSStudentEmailAdd.Text.Trim();

            // Generate PUP email from name
            string pupEmail = GeneratePupEmailFromName(firstName, lastName, middleName);

            string course = "BSIT"; // Auto-set to BSIT

            // Get year level
            string yearLevel = cmbRSYearLevel?.SelectedItem?.ToString() ?? "1st Year";

            // Applicant type (from dropdown)
            string applicantType = cmbRSEnrollmentStatus?.SelectedItem?.ToString() ?? "Regular Freshman";

            // Create new student record with 7 columns
            string[] newStudent = new string[]
            {
        studentId,
        fullName,
        personalEmail,      // Personal email from enrollment form
        pupEmail,           // Auto-generated PUP email
        course,
        yearLevel,
        applicantType       // Applicant type instead of enrollment status
            };

            // Add to student list
            studentList.Add(newStudent);

            // Create login account for student
            CreateStudentAccount(studentId, fullName, pupEmail);

            // Log the registration
            LogStudentRegistration(studentId, fullName, pupEmail);

            // Single success message
            MessageBox.Show($"Student {fullName} ({studentId}) has been registered successfully!\n\n" +
                $"Personal Email: {personalEmail}\n" +
                $"PUP Email: {pupEmail}\n\n" +
                "Login account has been created.\n" +
                "Default Password: pup123456\n\n" +
                "The student will appear in View All Users under Students tab.",
                "Registration Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Clear the form for next registration
            ClearStudentRegistrationForm();

            // Optional: Ask if user wants to view the student
            //DialogResult viewResult = MessageBox.Show("Do you want to view all users now?",
            //    "View Student", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //if (viewResult == DialogResult.Yes)
            //{
            //    viewingStudents = true;
            //    btnViewAllUsers.PerformClick();
            //}
        }

        private void ClearStudentRegistrationForm()
        {
            // Clear textboxes
            txtRSStudentID.Clear();
            txtRSStudentFirstName.Clear();
            txtRSStudentLastName.Clear();
            txtRSStudentMiddleName.Clear();

            // Clear address fields
            TextBox txtAddress1 = GetControl("txtAddress1") as TextBox;
            TextBox txtAddress2 = GetControl("txtAddress2") as TextBox;
            TextBox txtBarangay = GetControl("txtBarangay") as TextBox;
            TextBox txtPostal = GetControl("txtPostal") as TextBox;

            if (txtAddress1 != null) txtAddress1.Clear();
            if (txtAddress2 != null) txtAddress2.Clear();
            if (txtBarangay != null) txtBarangay.Clear();
            if (txtPostal != null) txtPostal.Clear();

            // Reset dropdowns
            if (cmbRSYearLevel != null) cmbRSYearLevel.SelectedIndex = -1;

            // Reset address dropdowns
            ComboBox cmbRegion = GetControl("cmbRegion") as ComboBox;
            ComboBox cmbProvince = GetControl("cmbProvince") as ComboBox;
            ComboBox cmbCity = GetControl("cmbCity") as ComboBox;
            ComboBox cmbSuffix = GetControl("cmbSuffix") as ComboBox;

            if (cmbRegion != null) cmbRegion.SelectedIndex = -1;
            if (cmbProvince != null) cmbProvince.SelectedIndex = -1;
            if (cmbCity != null) cmbCity.SelectedIndex = -1;
            if (cmbSuffix != null) cmbSuffix.SelectedIndex = 0;

            // Clear phone and email
            mtbRSStudentPhoneNum?.Clear();
            txtRSStudentEmailAdd?.Clear();

            // Reset date picker to a reasonable default (18 years ago)
            dtpRSStudentBirthDate.Value = DateTime.Now.AddYears(-18);

            // Clear all highlights
            ClearAllHighlights();
        }

        private void MarkRequiredFields()
        {
            // Add red asterisk to required field labels
            AddRequiredAsterisk(label33, "Student ID");      // Student ID label
            AddRequiredAsterisk(label30, "First Name");      // First Name label  
            AddRequiredAsterisk(label29, "Last Name");       // Last Name label
            AddRequiredAsterisk(label27, "Email");           // Email label
            AddRequiredAsterisk(label25, "Phone");           // Phone label
            AddRequiredAsterisk(label26, "Address");         // Address label
        }

        private void AddRequiredAsterisk(Label label, string originalText)
        {
            if (!label.Text.Contains("*"))
            {
                label.Text = originalText + " *";
                label.ForeColor = Color.Maroon;
            }
        }

        private void HighlightInvalidField(Control control)
        {
            if (control is TextBox txt)
            {
                txt.BackColor = Color.FromArgb(255, 220, 220);
                txt.Font = new Font(txt.Font, FontStyle.Bold);
            }
            else if (control is ComboBox cmb)
            {
                cmb.BackColor = Color.FromArgb(255, 220, 220);
                cmb.Font = new Font(cmb.Font, FontStyle.Bold);
            }
            else if (control is MaskedTextBox mtb)
            {
                mtb.BackColor = Color.FromArgb(255, 220, 220);
                mtb.Font = new Font(mtb.Font, FontStyle.Bold);
            }
            else if (control is DateTimePicker dtp)
            {
                dtp.BackColor = Color.FromArgb(255, 220, 220);
                dtp.Font = new Font(dtp.Font, FontStyle.Bold);
                dtp.CalendarForeColor = Color.Maroon;
                dtp.CalendarTitleBackColor = Color.FromArgb(255, 220, 220);
            }
        }

        private void ClearFieldHighlight(Control control)
        {
            if (control is TextBox txt)
            {
                txt.BackColor = Color.White;
                txt.Font = new Font(txt.Font, FontStyle.Regular);
            }
            else if (control is ComboBox cmb)
            {
                cmb.BackColor = Color.White;
                cmb.Font = new Font(cmb.Font, FontStyle.Regular);
            }
            else if (control is MaskedTextBox mtb)
            {
                mtb.BackColor = Color.White;
                mtb.Font = new Font(mtb.Font, FontStyle.Regular);
            }
            else if (control is DateTimePicker dtp)
            {
                dtp.BackColor = Color.White;
                dtp.Font = new Font(dtp.Font, FontStyle.Regular);
                dtp.CalendarForeColor = SystemColors.WindowText;
                dtp.CalendarTitleBackColor = SystemColors.Control;
            }
        }

        private void ClearAllHighlights()
        {
            ClearFieldHighlight(txtRSStudentID);
            ClearFieldHighlight(txtRSStudentFirstName);
            ClearFieldHighlight(txtRSStudentLastName);
            ClearFieldHighlight(txtRSStudentMiddleName);
            ClearFieldHighlight(txtRSStudentEmailAdd);
            ClearFieldHighlight(mtbRSStudentPhoneNum);

            // Clear address field highlights
            TextBox txtAddress1 = GetControl("txtAddress1") as TextBox;
            TextBox txtBarangay = GetControl("txtBarangay") as TextBox;
            ComboBox cmbRegion = GetControl("cmbRegion") as ComboBox;

            if (txtAddress1 != null) ClearFieldHighlight(txtAddress1);
            if (txtBarangay != null) ClearFieldHighlight(txtBarangay);
            if (cmbRegion != null) ClearFieldHighlight(cmbRegion);
        }

        private bool IsValidStudentId(string studentId)
        {
            // Check length (YYYY-NNNNN-CC-X = 14 characters)
            // Example: 2024-00001-SM-0 = 14 chars
            if (studentId.Length != 15)
            {
                return false;
            }


            // Split by hyphen
            string[] parts = studentId.Split('-');
            if (parts.Length != 4)
                return false;

            // Part 1: Year of admission (4 digits)
            if (parts[0].Length != 4 || !parts[0].All(char.IsDigit))
                return false;

            // Part 2: Enrollee number (5 digits)
            if (parts[1].Length != 5 || !parts[1].All(char.IsDigit))
                return false;

            // Part 3: Campus initials (2 letters, uppercase)
            if (parts[2].Length != 2 || !parts[2].All(char.IsLetter) || parts[2] != parts[2].ToUpper())
                return false;

            // Part 4: Enrollment status (0 or 1, single digit)
            if (parts[3].Length != 1 || (parts[3] != "0" && parts[3] != "1"))
                return false;

            // Optional: Validate year is reasonable (not future, not too old)
            int year = int.Parse(parts[0]);
            int currentYear = DateTime.Now.Year;
            if (year < 2000 || year > currentYear + 1)
                return false;

            return true;
        }

        private bool ValidateStudentRegistration()
        {
            bool isValid = true;

            // Clear all previous highlights
            ClearAllHighlights();

            // Student ID validation
            if (string.IsNullOrWhiteSpace(txtRSStudentID.Text))
            {
                HighlightInvalidField(txtRSStudentID);
                isValid = false;
            }
            else if (!IsValidStudentId(txtRSStudentID.Text.Trim()))
            {
                HighlightInvalidField(txtRSStudentID);
                MessageBox.Show("Invalid Student ID format.\n\n" +
                                "Format must be: YYYY-NNNNN-CC-X\n" +
                                "Example: 2024-00001-SM-0\n\n" +
                                "Where:\n" +
                                "• YYYY = Year of admission (e.g., 2024)\n" +
                                "• NNNNN = 5-digit enrollee number\n" +
                                "• CC = Campus initials (e.g., SM for Sta. Maria, MN for Manila)\n" +
                                "• X = Enrollment status (0 = Freshman, 1 = Transferee)",
                                "Invalid Format",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                isValid = false;
            }

            // First Name validation
            if (string.IsNullOrWhiteSpace(txtRSStudentFirstName.Text))
            {
                HighlightInvalidField(txtRSStudentFirstName);
                isValid = false;
            }

            // Last Name validation
            if (string.IsNullOrWhiteSpace(txtRSStudentLastName.Text))
            {
                HighlightInvalidField(txtRSStudentLastName);
                isValid = false;
            }

            // Email validation
            if (string.IsNullOrWhiteSpace(txtRSStudentEmailAdd.Text))
            {
                HighlightInvalidField(txtRSStudentEmailAdd);
                isValid = false;
            }
            else if (!txtRSStudentEmailAdd.Text.Contains("@"))
            {
                HighlightInvalidField(txtRSStudentEmailAdd);
                isValid = false;
            }

            // Phone validation
            if (string.IsNullOrWhiteSpace(mtbRSStudentPhoneNum.Text))
            {
                HighlightInvalidField(mtbRSStudentPhoneNum);
                isValid = false;
            }

            // Date of Birth validation
            if (!ValidateDateOfBirth())
            {
                isValid = false;
            }

            // APPLICANT TYPE VALIDATION 
            if (cmbRSEnrollmentStatus == null ||
                cmbRSEnrollmentStatus.SelectedItem == null ||
                cmbRSEnrollmentStatus.SelectedItem.ToString() == "Select Applicant Type" ||
                cmbRSEnrollmentStatus.SelectedIndex <= 0)
            {
                HighlightInvalidField(cmbRSEnrollmentStatus);
                isValid = false;
            }

            // Year Level validation
            if (cmbRSYearLevel == null ||
            cmbRSYearLevel.SelectedItem == null ||
            cmbRSYearLevel.SelectedItem.ToString() == "Select Year Level" ||
            cmbRSYearLevel.SelectedIndex <= 0)
            {
                HighlightInvalidField(cmbRSYearLevel);
                isValid = false;
            }

            // Address validation
            TextBox txtAddress1 = GetControl("txtAddress1") as TextBox;
            if (txtAddress1 != null && string.IsNullOrWhiteSpace(txtAddress1.Text))
            {
                HighlightInvalidField(txtAddress1);
                isValid = false;
            }

            TextBox txtBarangay = GetControl("txtBarangay") as TextBox;
            if (txtBarangay != null && string.IsNullOrWhiteSpace(txtBarangay.Text))
            {
                HighlightInvalidField(txtBarangay);
                isValid = false;
            }

            ComboBox cmbRegion = GetControl("cmbRegion") as ComboBox;
            if (cmbRegion != null && (cmbRegion.SelectedItem == null || string.IsNullOrWhiteSpace(cmbRegion.SelectedItem.ToString())))
            {
                HighlightInvalidField(cmbRegion);
                isValid = false;
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

        private void AttachRealTimeValidation()
        {
            txtRSStudentID.TextChanged += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtRSStudentID.Text))
                {
                    HighlightInvalidField(txtRSStudentID);
                }
                else if (IsValidStudentId(txtRSStudentID.Text.Trim()))
                {
                    ClearFieldHighlight(txtRSStudentID);

                    // Auto-set Applicant Type based on the status digit
                    string[] parts = txtRSStudentID.Text.Split('-');
                    if (parts.Length == 4 && cmbRSEnrollmentStatus != null)
                    {
                        string statusDigit = parts[3];
                        if (statusDigit == "0")
                        {
                            cmbRSEnrollmentStatus.SelectedItem = "Regular Freshman";
                            ClearFieldHighlight(cmbRSEnrollmentStatus);
                        }
                        else if (statusDigit == "1")
                        {
                            cmbRSEnrollmentStatus.SelectedItem = "Transferee";
                            ClearFieldHighlight(cmbRSEnrollmentStatus);
                        }
                    }
                }
                else
                {
                    HighlightInvalidField(txtRSStudentID);
                }
            };

            txtRSStudentFirstName.TextChanged += (s, e) => ClearFieldHighlight(txtRSStudentFirstName);
            txtRSStudentLastName.TextChanged += (s, e) => ClearFieldHighlight(txtRSStudentLastName);
            txtRSStudentEmailAdd.TextChanged += (s, e) => ClearFieldHighlight(txtRSStudentEmailAdd);
            mtbRSStudentPhoneNum.TextChanged += (s, e) => ClearFieldHighlight(mtbRSStudentPhoneNum);

            // live date validation
            dtpRSStudentBirthDate.ValueChanged += (s, e) =>
            {
                if (isResetingForm) return;
                // Re-validate when date changes
                if (!ValidateDateOfBirth())
                {
                    HighlightInvalidField(dtpRSStudentBirthDate);
                }
                else
                {
                    ClearFieldHighlight(dtpRSStudentBirthDate);
                }
            };

            // Address field real-time validation
            TextBox txtAddress1 = GetControl("txtAddress1") as TextBox;
            TextBox txtBarangay = GetControl("txtBarangay") as TextBox;
            ComboBox cmbRegion = GetControl("cmbRegion") as ComboBox;

            if (txtAddress1 != null) txtAddress1.TextChanged += (s, e) => ClearFieldHighlight(txtAddress1);
            if (txtBarangay != null) txtBarangay.TextChanged += (s, e) => ClearFieldHighlight(txtBarangay);
            if (cmbRegion != null) cmbRegion.SelectedIndexChanged += (s, e) => ClearFieldHighlight(cmbRegion);
        }

        private string GeneratePupEmailFromName(string firstName, string lastName, string middleName)
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

        private string RemoveSpacesAndSpecialChars(string input)
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

        private string RemoveSpecialCharacters(string input)
        {
            // Keep only letters (a-z)
            return new string(input.Where(c => char.IsLetter(c)).ToArray());
        }

        private bool IsStudentIdExists(string studentId)
        {
            return studentList.Any(s => s[0] == studentId);
        }

        private void CreateStudentAccount(string studentId, string fullName, string pupEmail)
        {
            try
            {
                // Create a simple account structure
                string accountEntry = $"{studentId}|{fullName}|{pupEmail}|pup123456|Student|Active|{DateTime.Now:yyyy-MM-dd}";
                string accountPath = Path.Combine(Application.StartupPath, "StudentAccounts.txt");
                File.AppendAllText(accountPath, accountEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Account creation failed: {ex.Message}");
            }
        }

        private void LogStudentRegistration(string studentId, string studentName, string pupEmail)
        {
            try
            {
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | REGISTERED | Student | " +
                                  $"{studentId} | {studentName} | {pupEmail} | Registered by: {Environment.UserName}";
                string logPath = Path.Combine(Application.StartupPath, "AuditLog.txt");
                File.AppendAllText(logPath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Logging failed: {ex.Message}");
            }
        }

        private void SetRegistrationFontsTo12pt()
        {
            // Font for labels (Bold)
            Font labelFont = new Font("Segoe UI", 12F, FontStyle.Bold);
            // Font for textboxes and inputs (Regular)
            Font inputFont = new Font("Segoe UI", 12F, FontStyle.Regular);

            // Update all labels in the registration container
            foreach (Control ctrl in pnlStudentRegistrationContainer.Controls)
            {
                if (ctrl is Label lbl)
                {
                    lbl.Font = labelFont;
                }
                else if (ctrl is TextBox txt)
                {
                    txt.Font = inputFont;
                }
                else if (ctrl is ComboBox cmb)
                {
                    cmb.Font = inputFont;
                }
                else if (ctrl is MaskedTextBox mtb)
                {
                    mtb.Font = inputFont;
                }
                else if (ctrl is DateTimePicker dtp)
                {
                    dtp.Font = inputFont;
                }
                else if (ctrl is Panel panel && panel.HasChildren)
                {
                    // Recursively update child controls
                    foreach (Control child in panel.Controls)
                    {
                        if (child is Label lblChild) lblChild.Font = labelFont;
                        else if (child is TextBox txtChild) txtChild.Font = inputFont;
                        else if (child is ComboBox cmbChild) cmbChild.Font = inputFont;
                        else if (child is MaskedTextBox mtbChild) mtbChild.Font = inputFont;
                        else if (child is DateTimePicker dtpChild) dtpChild.Font = inputFont;
                    }
                }
            }
        }

        private void RegisterStudentsContentAdmin_Load(object sender, EventArgs e)
        {
            //CreateFlexibleAddressSection();
            phAddressFields = new PHAddressFields();
            phAddressFields.Location = new Point(26, 425);
            phAddressFields.Size = new Size(850, 250);
            pnlStudentRegistrationContainer.Controls.Add(phAddressFields);
        }

        private void btnStudentClearForm_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in pnlStudentRegistrationContainer.Controls)
            {
                if (ctrl is TextBox txt)
                {
                    txt.Clear();
                }
                else if (ctrl is ComboBox cmb)
                {
                    cmb.SelectedIndex = -1;
                }
                else if (ctrl is MaskedTextBox mtb)
                {
                    mtb.Clear();
                }
                else if (ctrl is DateTimePicker dtp)
                {
                    isResetingForm = true;
                    dtp.Value = DateTime.Now.AddDays(-1);
                    isResetingForm = false;
                }
            }
            phAddressFields.ClearAddressFields();
        }
    }
}
