using CsvHelper;
using Jint.Expressions;
using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using PUPAcadPortal.PHAddress;
using PUPAcadPortal.Services;
using PUPAcadPortal.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using MySqlConnector;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Admin.Enrollment
{
    public partial class RegisterProfContentAdmin : UserControl
    {
        private bool professorFormCreated = false;
        private List<string[]> professorList = new List<string[]>();
        private PHAddressFields phAddressFields = new PHAddressFields();
        public RegisterProfContentAdmin()
        {
            InitializeComponent();
        }

        private async void RegisterProfContentAdmin_Load(object sender, EventArgs e)
        {
            await CreateProfessorRegistrationForm();

        }


        private async Task CreateProfessorRegistrationForm()
        {
            // Clear existing controls AND dispose them properly
            pnlProfRegistrationContainer.Controls.Clear();
            pnlProfRegistrationContainer.AutoScroll = true;
            pnlProfRegistrationContainer.AutoScrollPosition = new Point(0, 0); // Reset scroll position

            // Start from TOP of container (Y = 20, not higher)
            int yOffset = 20;  // ← START AT 20, NOT HIGHER
            int labelWidth = 180;
            int controlWidth = 396;
            int leftMargin = 23;
            int rightColumnX = leftMargin + labelWidth + 10; // 23 + 180 + 10 = 213

            // ===== PERSONAL INFORMATION SECTION =====
            Label lblPersonalInfo = new Label()
            {
                Text = "Personal Information",
                Font = new Font("Segoe UI", 15.75F, FontStyle.Bold),
                ForeColor = Color.Black,
                Size = new Size(250, 30),
                Location = new Point(leftMargin, yOffset)
            };
            pnlProfRegistrationContainer.Controls.Add(lblPersonalInfo);
            yOffset += 35;

            Panel line1 = new Panel()
            {
                BackColor = Color.Maroon,
                Size = new Size(860, 3),
                Location = new Point(leftMargin, yOffset)
            };
            pnlProfRegistrationContainer.Controls.Add(line1);
            yOffset += 25;

            // Professor ID
            Label lblEmployeeId = new Label()
            {
                Text = "Employee ID:*",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                Size = new Size(labelWidth, 30),
                Location = new Point(leftMargin, yOffset)
            };
            TextBox txtProfID = new TextBox()
            {
                Name = "txtProfID",
                Font = new Font("Segoe UI", 12F),
                ReadOnly = true,
                Size = new Size(controlWidth, 35),
                Location = new Point(rightColumnX, yOffset),
                PlaceholderText = "e.g., PROF-2024-001"
            };
            int currentYear = DateTime.Now.Year;
            int sequence = await AutoGenerators.GetNextProfSequence(currentYear);
            txtProfID.Text = AutoGenerators.GenerateUniqueProfId(currentYear, sequence);
            pnlProfRegistrationContainer.Controls.Add(lblEmployeeId);
            pnlProfRegistrationContainer.Controls.Add(txtProfID);
            yOffset += 45;

            // First Name
            Label lblFirstName = new Label()
            {
                Text = "First Name:*",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                Size = new Size(labelWidth, 30),
                Location = new Point(leftMargin, yOffset)
            };
            TextBox txtFirstName = new TextBox()
            {
                Name = "txtProfFirstName",
                Font = new Font("Segoe UI", 12F),
                Size = new Size(controlWidth, 35),
                Location = new Point(rightColumnX, yOffset),
                PlaceholderText = "Enter first name",
                CharacterCasing = CharacterCasing.Upper
            };
            pnlProfRegistrationContainer.Controls.Add(lblFirstName);
            pnlProfRegistrationContainer.Controls.Add(txtFirstName);
            yOffset += 45;

            // Last Name
            Label lblLastName = new Label()
            {
                Text = "Last Name:*",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                Size = new Size(labelWidth, 30),
                Location = new Point(leftMargin, yOffset)
            };
            TextBox txtLastName = new TextBox()
            {
                Name = "txtProfLastName",
                Font = new Font("Segoe UI", 12F),
                Size = new Size(controlWidth, 35),
                Location = new Point(rightColumnX, yOffset),
                PlaceholderText = "Enter last name",
                CharacterCasing = CharacterCasing.Upper
            };
            pnlProfRegistrationContainer.Controls.Add(lblLastName);
            pnlProfRegistrationContainer.Controls.Add(txtLastName);
            yOffset += 45;

            // Middle Name
            Label lblMiddleName = new Label()
            {
                Text = "Middle Name:",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                Size = new Size(labelWidth, 30),
                Location = new Point(leftMargin, yOffset)
            };
            TextBox txtMiddleName = new TextBox()
            {
                Name = "txtProfMiddleName",
                Font = new Font("Segoe UI", 12F),
                Size = new Size(controlWidth, 35),
                Location = new Point(rightColumnX, yOffset),
                PlaceholderText = "Enter middle name (optional)",
                Tag = "optional",
                CharacterCasing = CharacterCasing.Upper
            };
            pnlProfRegistrationContainer.Controls.Add(lblMiddleName);
            pnlProfRegistrationContainer.Controls.Add(txtMiddleName);
            yOffset += 45;

            // Suffix
            Label lblSuffix = new Label()
            {
                Text = "Suffix:",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                Size = new Size(labelWidth, 30),
                Location = new Point(leftMargin, yOffset)
            };
            ComboBox cmbSuffix = new ComboBox()
            {
                Name = "cmbProfSuffix",
                Font = new Font("Segoe UI", 12F),
                Size = new Size(150, 35),
                Location = new Point(rightColumnX, yOffset),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Tag = "optional"
            };
            cmbSuffix.Items.AddRange(new string[] { "", "Jr.", "Sr.", "III", "IV" });
            pnlProfRegistrationContainer.Controls.Add(lblSuffix);
            pnlProfRegistrationContainer.Controls.Add(cmbSuffix);
            yOffset += 45;

            // Birth Date
            Label lblBirthDate = new Label()
            {
                Text = "Date of Birth:*",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                Size = new Size(labelWidth, 30),
                Location = new Point(leftMargin, yOffset)
            };
            DateTimePicker dtpBirthDate = new DateTimePicker()
            {
                Name = "dtpProfBirthDate",
                Font = new Font("Segoe UI", 12F),
                Size = new Size(controlWidth, 35),
                Location = new Point(rightColumnX, yOffset),
                Value = DateTime.Now.AddYears(-30)
            };
            pnlProfRegistrationContainer.Controls.Add(lblBirthDate);
            pnlProfRegistrationContainer.Controls.Add(dtpBirthDate);
            yOffset += 55;

            // ===== CONTACT INFORMATION =====
            Label lblContact = new Label()
            {
                Text = "Contact Information",
                Font = new Font("Segoe UI", 15.75F, FontStyle.Bold),
                ForeColor = Color.Black,
                Size = new Size(250, 30),
                Location = new Point(leftMargin, yOffset)
            };
            pnlProfRegistrationContainer.Controls.Add(lblContact);
            yOffset += 35;

            Panel line2 = new Panel()
            {
                BackColor = Color.Maroon,
                Size = new Size(860, 3),
                Location = new Point(leftMargin, yOffset)
            };
            pnlProfRegistrationContainer.Controls.Add(line2);
            yOffset += 25;

            // Email
            Label lblEmail = new Label()
            {
                Text = "Email Address:*",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                Size = new Size(labelWidth, 30),
                Location = new Point(leftMargin, yOffset)
            };
            TextBox txtEmail = new TextBox()
            {
                Name = "txtProfEmail",
                Font = new Font("Segoe UI", 12F),
                Size = new Size(controlWidth, 35),
                Location = new Point(rightColumnX, yOffset),
                PlaceholderText = "Personal Email"
            };
            txtEmail.TextChanged += (s, e) =>
            {
                txtEmail.IsValidEmail();
            };
            pnlProfRegistrationContainer.Controls.Add(lblEmail);
            pnlProfRegistrationContainer.Controls.Add(txtEmail);
            yOffset += 45;

            // Phone
            Label lblPhone = new Label()
            {
                Text = "Phone Number:",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                Size = new Size(labelWidth, 30),
                Location = new Point(leftMargin, yOffset)
            };
            MaskedTextBox mtbPhone = new MaskedTextBox()
            {
                Name = "mtbProfPhone",
                Font = new Font("Segoe UI", 12F),
                Size = new Size(controlWidth, 35),
                Location = new Point(rightColumnX, yOffset),
                Mask = "(+63) 000-000-0000"
            };
            mtbPhone.MakeCursorGotoStart();
            pnlProfRegistrationContainer.Controls.Add(lblPhone);
            pnlProfRegistrationContainer.Controls.Add(mtbPhone);
            yOffset += 55;

            // ===== ADDRESS SECTION =====
            Label lblAddressHeader = new Label()
            {
                Text = "Address Information",
                Font = new Font("Segoe UI", 15.75F, FontStyle.Bold),
                ForeColor = Color.Black,
                Size = new Size(250, 30),
                Location = new Point(leftMargin, yOffset)
            };
            pnlProfRegistrationContainer.Controls.Add(lblAddressHeader);
            yOffset += 35;

            Panel line3 = new Panel()
            {
                BackColor = Color.Maroon,
                Size = new Size(860, 3),
                Location = new Point(leftMargin, yOffset)
            };
            pnlProfRegistrationContainer.Controls.Add(line3);
            yOffset += 25;

            phAddressFields.Location = new Point(leftMargin, yOffset);
            phAddressFields.Size = new Size(700, phAddressFields.Height);
            pnlProfRegistrationContainer.Controls.Add(phAddressFields);
            yOffset += phAddressFields.Height + 20;

            // ===== PROFESSIONAL INFORMATION =====
            Label lblProfessional = new Label()
            {
                Text = "Professional Information",
                Font = new Font("Segoe UI", 15.75F, FontStyle.Bold),
                ForeColor = Color.Black,
                Size = new Size(250, 30),
                Location = new Point(leftMargin, yOffset)
            };
            pnlProfRegistrationContainer.Controls.Add(lblProfessional);
            yOffset += 35;

            Panel line4 = new Panel()
            {
                BackColor = Color.Maroon,
                Size = new Size(860, 3),
                Location = new Point(leftMargin, yOffset)
            };
            pnlProfRegistrationContainer.Controls.Add(line4);
            yOffset += 25;

            // Department
            Label lblDept = new Label()
            {
                Text = "Department:*",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                Size = new Size(labelWidth, 30),
                Location = new Point(leftMargin, yOffset)
            };
            ComboBox cmbDepartment = new ComboBox()
            {
                Name = "cmbProfDepartment",
                Font = new Font("Segoe UI", 12F),
                Size = new Size(controlWidth, 35),
                Location = new Point(rightColumnX, yOffset),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            List<string> departments = new List<string>();

            using (var context = new AppDbContext())
            {
                departments = await context.Departments.Select(d => d.DepartmentName).OrderBy(department => department).ToListAsync();
            }
            departments.Insert(0, "Select Department");
            cmbDepartment.Items.AddRange([.. departments]);//new string[] { "Select Department", "Computer Science", "Information Technology", "Engineering", "Business Administration", "Accountancy", "General Education", "Liberal Arts", "Natural Sciences" });
            cmbDepartment.SelectedIndex = 0;
            pnlProfRegistrationContainer.Controls.Add(lblDept);
            pnlProfRegistrationContainer.Controls.Add(cmbDepartment);
            yOffset += 45;

            // Specialization
            //Label lblSpecialization = new Label()
            //{
            //    Text = "Specialization:*",
            //    Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
            //    Size = new Size(labelWidth, 30),
            //    Location = new Point(leftMargin, yOffset),
            //    Visible = false
            //};
            //ComboBox cmbSpecialization = new ComboBox()
            //{
            //    Name = "cmbProfSpecialization",
            //    Font = new Font("Segoe UI", 12F),
            //    Size = new Size(controlWidth, 35),
            //    Location = new Point(rightColumnX, yOffset),
            //    DropDownStyle = ComboBoxStyle.DropDownList,
            //    Visible = false
            //};
            //cmbSpecialization.Items.AddRange(new string[] { "Select Specialization", "English", "Mathematics", "Science", "History", "Filipino", "Social Studies", "Physical Education", "ICT", "TLE" });
            //cmbSpecialization.SelectedIndex = 0;
            //pnlProfRegistrationContainer.Controls.Add(lblSpecialization);
            //pnlProfRegistrationContainer.Controls.Add(cmbSpecialization);

            //// Show/hide specialization based on department (THIS MUST BE AFTER CREATING THE CONTROLS)
            //cmbDepartment.SelectedIndexChanged += (s, e) =>
            //{
            //    bool isGenEd = cmbDepartment.SelectedItem?.ToString() == "General Education";
            //    lblSpecialization.Visible = isGenEd;
            //    cmbSpecialization.Visible = isGenEd;
            //    if (isGenEd && cmbSpecialization.SelectedIndex == 0)
            //    {
            //        // Force user to select specialization if not selected
            //    }
            //};
            //yOffset += 45;


            // Employment Type
            Label lblEmpType = new Label()
            {
                Text = "Employment Type:*",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                Size = new Size(labelWidth, 30),
                Location = new Point(leftMargin, yOffset)
            };
            ComboBox cmbEmploymentType = new ComboBox()
            {
                Name = "cmbProfEmploymentType",
                Font = new Font("Segoe UI", 12F),
                Size = new Size(200, 35),
                Location = new Point(rightColumnX, yOffset),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbEmploymentType.Items.AddRange(new string[] { "Select Type", "Full-Time", "Part-Time" });
            cmbEmploymentType.SelectedIndex = 0;
            pnlProfRegistrationContainer.Controls.Add(lblEmpType);
            pnlProfRegistrationContainer.Controls.Add(cmbEmploymentType);
            yOffset += 45;

            // Max Load (auto-calculated)
            Label lblMaxLoad = new Label()
            {
                Text = "Max Load (units):",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(cmbEmploymentType.Location.X + cmbEmploymentType.Width + 5, lblEmpType.Location.Y)
            };
            TextBox txtMaxLoad = new TextBox()
            {
                Name = "txtProfMaxLoad",
                Font = new Font("Segoe UI", 12F),
                Location = new Point(lblMaxLoad.Location.X + lblMaxLoad.Width + 40,cmbEmploymentType.Location.Y),
                Size = new Size(50, cmbEmploymentType.Height),
                ReadOnly = true,
                BackColor = Color.LightGray,
                Text = "0"
            };
            pnlProfRegistrationContainer.Controls.Add(lblMaxLoad);
            pnlProfRegistrationContainer.Controls.Add(txtMaxLoad);

            cmbEmploymentType.SelectedIndexChanged += (s, e) =>
            {
                string type = cmbEmploymentType.SelectedItem?.ToString();
                txtMaxLoad.Text = type == "Full-Time" ? "24" : type == "Part-Time" ? "15" : "0";
            };

            // Highest Degree
            Label lblDegree = new Label()
            {
                Text = "Highest Degree:*",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                Size = new Size(labelWidth, 30),
                Location = new Point(leftMargin, yOffset)
            };
            ComboBox cmbDegree = new ComboBox()
            {
                Name = "cmbProfDegree",
                Font = new Font("Segoe UI", 12F),
                Size = new Size(controlWidth, 35),
                Location = new Point(rightColumnX, yOffset),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbDegree.Items.AddRange(new string[] { "Select Degree", "Bachelor's", "Master's", "Doctorate", "PhD" });
            cmbDegree.SelectedIndex = 0;
            pnlProfRegistrationContainer.Controls.Add(lblDegree);
            pnlProfRegistrationContainer.Controls.Add(cmbDegree);
            yOffset += 45;

            // Years of Experience
            Label lblExp = new Label()
            {
                Text = "Years of Experience:*",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                Size = new Size(labelWidth, 30),
                Location = new Point(leftMargin, yOffset)
            };
            NumericUpDown numExp = new NumericUpDown()
            {
                Name = "numProfExperience",
                Font = new Font("Segoe UI", 12F),
                Size = new Size(120, 35),
                Location = new Point(rightColumnX, yOffset),
                Minimum = 0,
                Maximum = 50
            };
            pnlProfRegistrationContainer.Controls.Add(lblExp);
            pnlProfRegistrationContainer.Controls.Add(numExp);
            yOffset += 45;

            Label lblStatus = new Label()
            {
                Text = "Status:*",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                Size = new Size(labelWidth, 30),
                Location = new Point(leftMargin, yOffset)
            };
            ComboBox cmbStatus = new ComboBox()
            {
                Name = "cmbProfStatus",
                Font = new Font("Segoe UI", 12F),
                Size = new Size(controlWidth, 35),
                Location = new Point(rightColumnX, yOffset),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Tag = "optional"
            };
            // REMOVED "Retired" - should only be set by admin later, not on registration
            cmbStatus.Items.AddRange(new string[] { "Active", "Probationary" });
            cmbStatus.SelectedIndex = 0;
            pnlProfRegistrationContainer.Controls.Add(lblStatus);
            pnlProfRegistrationContainer.Controls.Add(cmbStatus);
            yOffset += 55;

            // Buttons
            Button btnClear = new Button()
            {
                Text = "Clear Form",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.LightGray,
                Size = new Size(147, 40),
                Location = new Point(pnlProfRegistrationContainer.Width - 400, yOffset)
            };
            btnClear.Click += (s, e) =>
            {
                if (MessageBox.Show("Are you sure you want to clear this form?", "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
                // Clear Personal Information
                txtFirstName.Clear();
                txtLastName.Clear();
                txtMiddleName.Clear();
                cmbSuffix.SelectedIndex = 0;
                dtpBirthDate.Value = DateTime.Now.AddYears(-30);

                // Clear Contact Information
                txtEmail.Clear();
                mtbPhone.Clear();

                // Clear Address Information
                phAddressFields.ClearAddressFields();

                // Clear Professional Information
                cmbDepartment.SelectedIndex = 0;
                cmbEmploymentType.SelectedIndex = 0;
                cmbDegree.SelectedIndex = 0;
                numExp.Value = 0;
                txtMaxLoad.Text = "0";
                cmbStatus.SelectedIndex = 0;

                // Clear Specialization
                //cmbSpecialization.SelectedIndex = 0;
                //lblSpecialization.Visible = false;
                //cmbSpecialization.Visible = false;
            };
            pnlProfRegistrationContainer.Controls.Add(btnClear);

            Button btnRegister = new Button()
            {
                Text = "Register Professor",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                BackColor = Color.Maroon,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(215, 40),
                Location = new Point(btnClear.Location.X + btnClear.Width + 20, yOffset)
            };
            btnRegister.Click += async (s, e) => 
            {
                if (!pnlProfRegistrationContainer.ValidateRegistration())
                    return;
                if (!txtEmail.IsValidEmail())
                    return;

                if (MessageBox.Show("Are you sure you want to register this professor? This action cannot be undone.", "Confirm Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return;
                }

                string firstName = txtFirstName.Text.Trim();
                string lastName = txtLastName.Text.Trim();
                string middleName = txtMiddleName.Text.Trim();
                string suffix = cmbSuffix.SelectedItem?.ToString();

                string fullName = $"{lastName}, {firstName}";
                if (!string.IsNullOrWhiteSpace(middleName))
                    fullName += $" {middleName}";
                if (!string.IsNullOrWhiteSpace(suffix) && suffix != "")
                    fullName += $" {suffix}";

                string email = txtEmail.Text.Trim();
                string department = cmbDepartment.SelectedItem?.ToString();
                int departmentId;
                using (var context = new AppDbContext())
                {
                    var departments = await context.Departments.FirstOrDefaultAsync(d => d.DepartmentName == department);
                    departmentId = departments.DepartmentId;
                }
                string employmentType = cmbEmploymentType.SelectedItem.ToString();
                int maxLoad = Convert.ToInt32(txtMaxLoad.Text);
                string highestDegree = cmbDegree.SelectedItem.ToString();
                string status = cmbStatus.SelectedItem.ToString();
                int yearsOfExp = Convert.ToInt32(numExp.Value);
                string rank = "Professor";

                string address1 = phAddressFields.SelectedAddressLine1;
                string address2 = phAddressFields.SelectedAddressLine2;
                string barangay = phAddressFields.SelectedBarangayName;
                string city = phAddressFields.SelectedCityName;
                string province = phAddressFields.SelectedProvinceName;
                string region = phAddressFields.SelectedRegionName;
                string postal = phAddressFields.SelectedPostalCode;

                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

                var prd = new ProfRegistrationData
                {
                    FirstName = textInfo.ToTitleCase(txtFirstName.Text.Trim().ToLower()),
                    MiddleName = textInfo.ToTitleCase(txtMiddleName.Text.Trim().ToLower()),
                    LastName = textInfo.ToTitleCase(txtLastName.Text.Trim().ToLower()),
                    Suffix = suffix,
                    DateOfBirth = dtpBirthDate.Value,

                    Email = txtEmail.Text.Trim().ToLowerInvariant(),
                    Phone = mtbPhone.Text.Trim(),

                    Address1 = phAddressFields.SelectedAddressLine1.Trim(),
                    Address2 = phAddressFields.SelectedAddressLine2.Trim(),
                    Region = phAddressFields.SelectedRegionName,
                    Province = phAddressFields.SelectedProvinceName,
                    City = phAddressFields.SelectedCityName,
                    Barangay = phAddressFields.SelectedBarangayName,
                    PostalCode = phAddressFields.SelectedPostalCode,
                    DepartmentId = departmentId,
                    EmploymentType = employmentType,
                    MaxLoad = maxLoad,
                    HighestDegree = highestDegree,
                    YearsOfExperience = yearsOfExp,
                    EmploymentStatus = status,
                    Rank = rank
                };

                Application.UseWaitCursor = true;
                this.FindForm().FormClosing += CloseApp.Cancel_Closing;
                btnRegister.Text = "Registering Professor...";
                btnRegister.Enabled = false;
                try
                {
                    var service = new ProfRegistrationService();
                    var registeredProf = await service.RegisterProf(prd);

                    this.SafeUIUpdate(() =>
                    {
                        MessageBox.Show(
                            $"Success! '{registeredProf.User.FirstName} {registeredProf.User.LastName}' has been securely registered.\n\n" +
                            $"Generated Employee No.: {registeredProf.EmployeeId}\n" +
                            $"Generated Email: {registeredProf.User.InstitutionalEmail}\n" +
                            "An email containing their credentials has beem sent to the specified email address.",
                            "Registration Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnClear.PerformClick();
                    });
                }
                catch (InvalidOperationException ex)
                {
                    this.SafeUIUpdate(() =>
                    {
                        MessageBox.Show(ex.Message, "Duplicate Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    });
                }
                catch (DbUpdateException ex)
                {
                    string innerMessage = ex.InnerException?.Message ?? "";
                    Exception baseEx = ex.GetBaseException();
                    string baseMessage = baseEx?.Message ?? "";

                    if (innerMessage.Contains("Duplicate entry") || baseMessage.Contains("Duplicate entry"))
                    {
                        string targetMessage = innerMessage.Contains("Duplicate entry") ? innerMessage : baseMessage;

                        var match = Regex.Match(targetMessage, @"Duplicate entry '([^']*)' for key '([^']*)'");

                        if (match.Success)
                        {
                            string duplicateValue = match.Groups[1].Value;
                            string constraintName = match.Groups[2].Value;

                            this.SafeUIUpdate(() =>
                            {
                                MessageBox.Show(
                                $"The email address '{duplicateValue}' is already registered in the system. Consider using a different one.",
                                "Registration Conflict",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                                );
                            });
                        }
                        else
                        {
                            this.SafeUIUpdate(() =>
                            {
                                MessageBox.Show(
                                    "This account details match an existing record. Please use a different email.",
                                    "Duplicate Record Found",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                );
                            });
                        }
                    }
                    else
                    {
                        this.SafeUIUpdate(() =>
                        {
                            MessageBox.Show($"Database connection error: {ex.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        });
                    }
                }
                catch (Exception generalEx)
                {
                    this.SafeUIUpdate(() =>
                    {
                        MessageBox.Show($"An unexpected error occurred: {generalEx.Message}", "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    });
                }
                finally
                {
                    this.SafeUIUpdate(() => 
                    {
                        Application.UseWaitCursor = false;
                        this.FindForm().FormClosing -= CloseApp.Cancel_Closing;
                        btnRegister.Text = "Register Professor";
                        btnRegister.Enabled = true;
                    });
                }
            };
            pnlProfRegistrationContainer.Controls.Add(btnRegister);

            pnlProfRegistrationContainer.Height = yOffset + 80;
            pnlProfBottomExtension.Location = new Point(pnlProfBottomExtension.Location.X, pnlProfRegistrationContainer.Height + 150);
            professorFormCreated = true;
            Validators.AttachLiveErrorClearers(pnlProfRegistrationContainer);
            dtpBirthDate.Leave += (s, e) => dtpBirthDate.IsDateOfBirthValid();
            txtEmail.TextChanged += (s, e) => txtEmail.IsValidEmail();
        }
    }
}
