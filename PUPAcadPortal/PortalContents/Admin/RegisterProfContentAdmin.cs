using PUPAcadPortal.PHAddress;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Admin
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

        private void RegisterProfContentAdmin_Load(object sender, EventArgs e)
        {
            CreateProfessorRegistrationForm();
        }

        private void CreateProfessorRegistrationForm()
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
            Label lblProfID = new Label()
            {
                Text = "Professor ID:*",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                Size = new Size(labelWidth, 30),
                Location = new Point(leftMargin, yOffset)
            };
            TextBox txtProfID = new TextBox()
            {
                Name = "txtProfID",
                Font = new Font("Segoe UI", 12F),
                Size = new Size(controlWidth, 35),
                Location = new Point(rightColumnX, yOffset),
                PlaceholderText = "e.g., PROF-2024-001"
            };
            pnlProfRegistrationContainer.Controls.Add(lblProfID);
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
                PlaceholderText = "Enter first name"
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
                PlaceholderText = "Enter last name"
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
                PlaceholderText = "Enter middle name (optional)"
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
                DropDownStyle = ComboBoxStyle.DropDownList
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
                PlaceholderText = "professor@pup.edu.ph"
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
                Mask = "0999-999-9999"
            };
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
            phAddressFields.Size = new Size(850, 250);
            pnlProfRegistrationContainer.Controls.Add(phAddressFields);
            yOffset += 260;

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
            cmbDepartment.Items.AddRange(new string[] { "Select Department", "Computer Science", "Information Technology", "Engineering", "Business Administration", "Accountancy", "General Education", "Liberal Arts", "Natural Sciences" });
            cmbDepartment.SelectedIndex = 0;
            pnlProfRegistrationContainer.Controls.Add(lblDept);
            pnlProfRegistrationContainer.Controls.Add(cmbDepartment);
            yOffset += 45;

            // Specialization
            Label lblSpecialization = new Label()
            {
                Text = "Specialization:*",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                Size = new Size(labelWidth, 30),
                Location = new Point(leftMargin, yOffset),
                Visible = false
            };
            ComboBox cmbSpecialization = new ComboBox()
            {
                Name = "cmbProfSpecialization",
                Font = new Font("Segoe UI", 12F),
                Size = new Size(controlWidth, 35),
                Location = new Point(rightColumnX, yOffset),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Visible = false
            };
            cmbSpecialization.Items.AddRange(new string[] { "Select Specialization", "English", "Mathematics", "Science", "History", "Filipino", "Social Studies", "Physical Education", "ICT", "TLE" });
            cmbSpecialization.SelectedIndex = 0;
            pnlProfRegistrationContainer.Controls.Add(lblSpecialization);
            pnlProfRegistrationContainer.Controls.Add(cmbSpecialization);

            // Show/hide specialization based on department (THIS MUST BE AFTER CREATING THE CONTROLS)
            cmbDepartment.SelectedIndexChanged += (s, e) =>
            {
                bool isGenEd = cmbDepartment.SelectedItem?.ToString() == "General Education";
                lblSpecialization.Visible = isGenEd;
                cmbSpecialization.Visible = isGenEd;
                if (isGenEd && cmbSpecialization.SelectedIndex == 0)
                {
                    // Force user to select specialization if not selected
                }
            };
            yOffset += 45;


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
                Size = new Size(130, 30),
                Location = new Point(rightColumnX + 210, yOffset - 45)
            };
            TextBox txtMaxLoad = new TextBox()
            {
                Name = "txtProfMaxLoad",
                Font = new Font("Segoe UI", 12F),
                Size = new Size(80, 35),
                Location = new Point(rightColumnX + 350, yOffset - 45),
                ReadOnly = true,
                BackColor = Color.LightGray,
                Text = "0"
            };
            pnlProfRegistrationContainer.Controls.Add(lblMaxLoad);
            pnlProfRegistrationContainer.Controls.Add(txtMaxLoad);

            cmbEmploymentType.SelectedIndexChanged += (s, e) =>
            {
                string type = cmbEmploymentType.SelectedItem?.ToString();
                txtMaxLoad.Text = type == "Full-Time" ? "40" : type == "Part-Time" ? "12" : "0";
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
                DropDownStyle = ComboBoxStyle.DropDownList
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
                Location = new Point(400, yOffset)
            };
            btnClear.Click += (s, e) =>
            {
                // Clear Personal Information
                txtProfID.Clear();
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
                cmbSpecialization.SelectedIndex = 0;
                lblSpecialization.Visible = false;
                cmbSpecialization.Visible = false;
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
                Location = new Point(560, yOffset)
            };
            btnRegister.Click += (s, e) =>
            {
                if (!ValidateProfessorRegistration())
                    return;

                string profId = txtProfID.Text.Trim();
                string firstName = txtFirstName.Text.Trim();
                string lastName = txtLastName.Text.Trim();
                string middleName = txtMiddleName.Text.Trim();
                string suffix = cmbSuffix.SelectedItem?.ToString() ?? "";

                string fullName = $"{lastName}, {firstName}";
                if (!string.IsNullOrWhiteSpace(middleName))
                    fullName += $" {middleName}";
                if (!string.IsNullOrWhiteSpace(suffix) && suffix != "")
                    fullName += $" {suffix}";

                string email = txtEmail.Text.Trim();
                string department = cmbDepartment.SelectedItem?.ToString() ?? "";
                string specialization = department == "General Education" ? cmbSpecialization.SelectedItem?.ToString() ?? "N/A" : "N/A";
                string employmentType = cmbEmploymentType.SelectedItem?.ToString() ?? "";
                string maxLoad = txtMaxLoad.Text;
                string status = cmbStatus.SelectedItem?.ToString() ?? "Active";

                string address1 = phAddressFields.SelectedAddressLine1;
                string address2 = phAddressFields.SelectedAddressLine2;
                string barangay = phAddressFields.SelectedBarangayName;
                string city = phAddressFields.SelectedCityName;
                string province = phAddressFields.SelectedProvinceName;
                string region = phAddressFields.SelectedRegionName      ;
                string postal = phAddressFields.SelectedPostalCode;

                string fullAddress = $"{address1}, {address2}, {barangay}, {city}, {province}, {region} {postal}".Trim();

                professorList.Add(new string[] {
            profId, fullName, email, department, specialization,
            employmentType, maxLoad, fullAddress, status
        });

                MessageBox.Show($"Professor {fullName} registered successfully!\nMax Load: {maxLoad} units", "Success");

                // Clear form
                txtProfID.Clear(); txtFirstName.Clear(); txtLastName.Clear(); txtMiddleName.Clear();
                txtEmail.Clear(); mtbPhone.Clear();
                dtpBirthDate.Value = DateTime.Now.AddYears(-30);
                phAddressFields.ClearAddressFields();
                cmbDepartment.SelectedIndex = 0; cmbEmploymentType.SelectedIndex = 0;
                cmbDegree.SelectedIndex = 0; cmbStatus.SelectedIndex = 0; cmbSuffix.SelectedIndex = 0;
                cmbSpecialization.SelectedIndex = 0; numExp.Value = 0; txtMaxLoad.Text = "0";
                lblSpecialization.Visible = false; cmbSpecialization.Visible = false;
            };
            pnlProfRegistrationContainer.Controls.Add(btnRegister);

            pnlProfRegistrationContainer.AutoScrollMinSize = new Size(0, yOffset + 100);
            professorFormCreated = true;
        }

        private Control GetControl(string name)
        {
            // Search in the registration container
            foreach (Control ctrl in this.Controls)
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

        private void ClearProfessorHighlights()
        {
            var profControls = new Control[] {
        GetControl("txtProfID"),
        GetControl("txtProfFirstName"),
        GetControl("txtProfLastName"),
        GetControl("txtProfEmail"),
        GetControl("mtbProfPhone"),
        GetControl("dtpProfBirthDate"),
        GetControl("cmbProfDepartment"),
        GetControl("cmbProfEmploymentType"),
        GetControl("cmbProfDegree"),
        GetControl("cmbProfStatus"),
        GetControl("txtProfProvince"),
        GetControl("txtProfCity"),
        GetControl("txtProfBarangay"),
        GetControl("txtProfAddress1")
        };

            foreach (var ctrl in profControls)
            {
                if (ctrl != null)
                {
                    ctrl.BackColor = Color.White;
                    ctrl.Font = new Font(ctrl.Font, FontStyle.Regular);
                }
            }
        }

        private bool ValidateProfessorRegistration()
        {
            // Clear previous highlights
            ClearProfessorHighlights();

            // Get all controls (using the names from your CreateProfessorRegistrationForm)
            TextBox txtProfID = GetControl("txtProfID") as TextBox;
            TextBox txtFirstName = GetControl("txtProfFirstName") as TextBox;
            TextBox txtLastName = GetControl("txtProfLastName") as TextBox;
            TextBox txtEmail = GetControl("txtProfEmail") as TextBox;
            MaskedTextBox mtbPhone = GetControl("mtbProfPhone") as MaskedTextBox;
            DateTimePicker dtpBirthDate = GetControl("dtpProfBirthDate") as DateTimePicker;
            ComboBox cmbDepartment = GetControl("cmbProfDepartment") as ComboBox;
            ComboBox cmbEmploymentType = GetControl("cmbProfEmploymentType") as ComboBox;
            ComboBox cmbDegree = GetControl("cmbProfDegree") as ComboBox;
            ComboBox cmbStatus = GetControl("cmbProfStatus") as ComboBox;
            TextBox txtProvince = GetControl("txtProfProvince") as TextBox;
            TextBox txtMunicipality = GetControl("txtProfMunicipality") as TextBox;
            TextBox txtBarangay = GetControl("txtProfBarangay") as TextBox;
            TextBox txtStreet = GetControl("txtProfStreet") as TextBox;

            // Professor ID validation
            if (txtProfID == null || string.IsNullOrWhiteSpace(txtProfID.Text))
            {
                MessageBox.Show("Professor ID is required", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProfID?.Focus();
                return false;
            }
            else if (!IsValidProfessorId(txtProfID.Text.Trim()))
            {
                HighlightProfField(txtProfID);
                MessageBox.Show("Professor ID must be in format: PROF-YYYY-NNN (e.g., PROF-2024-001)", "Invalid Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProfID.Focus();
                return false;
            }
            else if (professorList.Any(p => p[0] == txtProfID.Text.Trim()))
            {
                HighlightProfField(txtProfID);
                MessageBox.Show("Professor ID already exists!", "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProfID.Focus();
                return false;
            }

            // Name validation
            if (txtFirstName == null || string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("First name is required", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFirstName?.Focus();
                return false;
            }

            if (txtLastName == null || string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Last name is required", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLastName?.Focus();
                return false;
            }

            // Email validation
            if (txtEmail == null || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Email address is required", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail?.Focus();
                return false;
            }
            else if (!IsValidEmail(txtEmail.Text))
            {
                HighlightProfField(txtEmail);
                MessageBox.Show("Enter a valid email address (e.g., name@domain.com)", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            // Phone validation
            if (mtbPhone == null || string.IsNullOrWhiteSpace(mtbPhone.Text) || mtbPhone.Text.Replace("-", "").Replace(" ", "").Length < 11)
            {
                HighlightProfField(mtbPhone);
                MessageBox.Show("Enter a valid 11-digit phone number", "Invalid Phone", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtbPhone?.Focus();
                return false;
            }

            // Age validation
            if (dtpBirthDate == null)
            {
                MessageBox.Show("Birth date is required", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            int age = CalculateAge(dtpBirthDate.Value);
            if (dtpBirthDate.Value > DateTime.Now)
            {
                HighlightProfField(dtpBirthDate);
                MessageBox.Show("Birth date cannot be in the future", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpBirthDate.Focus();
                return false;
            }
            else if (age < 21 || age > 70)
            {
                HighlightProfField(dtpBirthDate);
                MessageBox.Show($"Age must be between 21-70 years old. Current age: {age}", "Age Requirement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpBirthDate.Focus();
                return false;
            }

            // Department validation
            if (cmbDepartment == null || cmbDepartment.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select a department", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbDepartment?.Focus();
                return false;
            }

            // Specialization for Gen Ed (handled in your existing code)

            // Employment Type validation
            if (cmbEmploymentType == null || cmbEmploymentType.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select employment type", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbEmploymentType?.Focus();
                return false;
            }

            // Degree validation
            if (cmbDegree == null || cmbDegree.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select highest degree", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbDegree?.Focus();
                return false;
            }

            // Status validation (should not be retired on registration)
            if (cmbStatus == null || cmbStatus.SelectedIndex < 0)
            {
                MessageBox.Show("Please select status", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Address validation - use the correct control names
            TextBox txtProfProvince = GetControl("txtProfProvince") as TextBox;
            TextBox txtProfCity = GetControl("txtProfCity") as TextBox;
            TextBox txtProfBarangay = GetControl("txtProfBarangay") as TextBox;
            TextBox txtProfAddress1 = GetControl("txtProfAddress1") as TextBox;

            if (txtProfProvince == null || string.IsNullOrWhiteSpace(txtProfProvince.Text))
            {
                HighlightProfField(txtProfProvince);
                MessageBox.Show("Province is required", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProfProvince?.Focus();
                return false;
            }
            if (txtProfCity == null || string.IsNullOrWhiteSpace(txtProfCity.Text))
            {
                HighlightProfField(txtProfCity);
                MessageBox.Show("City/Municipality is required", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProfCity?.Focus();
                return false;
            }
            if (txtProfBarangay == null || string.IsNullOrWhiteSpace(txtProfBarangay.Text))
            {
                HighlightProfField(txtProfBarangay);
                MessageBox.Show("Barangay is required", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProfBarangay?.Focus();
                return false;
            }
            if (txtProfAddress1 == null || string.IsNullOrWhiteSpace(txtProfAddress1.Text))
            {
                HighlightProfField(txtProfAddress1);
                MessageBox.Show("Street address is required", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProfAddress1?.Focus();
                return false;
            }

            return true;
        }
        private bool IsValidProfessorId(string profId)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(profId, @"^PROF-\d{4}-\d{3}$");
        }

        private void HighlightProfField(Control control)
        {
            control.BackColor = Color.FromArgb(255, 220, 220);
            control.Font = new Font(control.Font, FontStyle.Bold);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
