using PUPAcadPortal.PHAddress;
using PUPAcadPortal.Services;
using PUPAcadPortal.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace PUPAcadPortal.PortalContents.Admin.Enrollment
{
    public partial class RegisterStudentsContentAdmin : UserControl
    {
        private PHAddressFields phAddressFields = new PHAddressFields();
        private ComboBox cmbSuffix;
        public RegisterStudentsContentAdmin()
        {
            InitializeComponent();
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
            int yPosition = label29.Location.Y;  // Same Y as Last Name (173)
            int xPosition = txtRSStudentLastName.Location.X + txtRSStudentLastName.Width; // 439 + 437 + 10 = 886

            // But ensure it doesn't go out of bounds - adjust if needed
            if (xPosition + 120 > pnlStudentRegistrationContainer.Width)
            {
                xPosition = txtRSStudentLastName.Location.X + txtRSStudentLastName.Width - 80;
            }

            System.Windows.Forms.Label lblSuffix = new System.Windows.Forms.Label();
            lblSuffix.Text = "Suffix:";
            lblSuffix.Location = new Point(xPosition, yPosition);
            lblSuffix.AutoSize = true;
            lblSuffix.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

            cmbSuffix = new ComboBox();
            cmbSuffix.Name = "cmbSuffix";
            cmbSuffix.Tag = "optional";
            cmbSuffix.Location = new Point(lblSuffix.Location.X, txtRSStudentLastName.Location.Y);
            cmbSuffix.Size = new Size(70, 29);
            cmbSuffix.TabIndex = 3;
            cmbSuffix.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            cmbSuffix.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSuffix.Items.AddRange(new string[] { "", "Jr.", "Sr.", "I", "II", "III", "IV" });

            pnlStudentRegistrationContainer.Controls.Add(lblSuffix);
            pnlStudentRegistrationContainer.Controls.Add(cmbSuffix);
        }

        private async void btnStudentRegistration_Click(object sender, EventArgs e)
        {

            // Validate all required fields
            if (!pnlStudentRegistrationContainer.ValidateStudentRegistration())
                return;
            if (!txtRSStudentEmailAdd.Text.IsValidEmail())
                return;

            string program = cmbRSStudentProgram.Text.Trim() switch
            {
                "BS Information Technology" => "BSIT",
                _ => "No Program"
            };

            int yearLevel = cmbRSYearLevel?.SelectedItem?.ToString() switch
            {
                "1st Year" => 1,
                "2nd Year" => 2,
                "3rd Year" => 3,
                "4th Year" => 4,
                _ => 1
            };

            string applicantType = cmbRSEnrollmentStatus?.SelectedItem?.ToString() ?? "Regular";

            string? suffix = cmbSuffix?.SelectedItem?.ToString();
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

            var dto = new Data.StudentRegistrationData
            {
                FirstName = textInfo.ToTitleCase(txtRSStudentFirstName.Text.Trim().ToLower()),
                MiddleName = textInfo.ToTitleCase(txtRSStudentMiddleName.Text.Trim().ToLower()),
                LastName = textInfo.ToTitleCase(txtRSStudentLastName.Text.Trim().ToLower()),
                Suffix = suffix,
                DateOfBirth = dtpRSStudentBirthDate.Value,

                Email = txtRSStudentEmailAdd.Text.Trim().ToLowerInvariant(),
                Phone = mtbRSStudentPhoneNum.Text.Trim(),

                Address1 = phAddressFields.SelectedAddressLine1.Trim(),
                Address2 = phAddressFields.SelectedAddressLine2.Trim(),
                Region = phAddressFields.SelectedRegionName,
                Province = phAddressFields.SelectedProvinceName,
                City = phAddressFields.SelectedCityName,
                Barangay = phAddressFields.SelectedBarangayName,
                PostalCode = phAddressFields.SelectedPostalCode,

                Program = program,
                YearLevel = yearLevel,
                StudentType = applicantType
            };

            btnStudentRegistration.Enabled = false;
            this.FindForm().FormClosing += CloseApp.Cancel_Closing;
            btnStudentRegistration.Text = "Registering Student";
            Application.UseWaitCursor = true;

            try
            {
                var service = new StudentRegistrationService();

                var registeredStudent = await service.RegisterSingleStudent(dto);

                this.SafeUIUpdate(() =>
                {
                    btnStudentClearForm.PerformClick();

                    MessageBox.Show(
                        $"Success! Student '{registeredStudent.User.FirstName} {registeredStudent.User.LastName}' has been securely registered.\n\n" +
                        $"Generated Student No: {registeredStudent.StudentNumber}\n" +
                        $"Generated Email: {registeredStudent.User.InstitutionalEmail}",
                        "Registration Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
            }
            catch (InvalidOperationException ex)
            {
                this.SafeUIUpdate(() =>
                {
                    MessageBox.Show(ex.Message, "Duplicate Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                });
            }
            catch (Exception ex)
            {
                this.SafeUIUpdate(() =>
                {
                    MessageBox.Show($"A critical error occurred during registration:\n\n{ex.Message}", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
            }
            finally
            {
                Application.UseWaitCursor = false;
                this.SafeUIUpdate(() =>
                {
                    btnStudentRegistration.Enabled = true;
                    this.FindForm().FormClosing -= CloseApp.Cancel_Closing;
                    btnStudentRegistration.Text = "Register Student";
                });
            }
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

        private void AttachRealTimeValidation()
        {
            txtRSStudentFirstName.TextChanged += (s, e) => txtRSStudentFirstName.ClearFieldHighlight();
            txtRSStudentLastName.TextChanged += (s, e) => txtRSStudentLastName.ClearFieldHighlight();
            txtRSStudentEmailAdd.TextChanged += (s, e) => txtRSStudentEmailAdd.ClearFieldHighlight();
            mtbRSStudentPhoneNum.TextChanged += (s, e) => mtbRSStudentPhoneNum.ClearFieldHighlight();

            // live date validation
            dtpRSStudentBirthDate.Leave += (s, e) =>
            {
                // Re-validate when date changes
                dtpRSStudentBirthDate.IsDateOfBirthValid();
            };

            // Address field real-time validation
            TextBox txtAddress1 = phAddressFields.AddressLine1;
            ComboBox cmbBarangay = phAddressFields.BarangayComboBox;
            ComboBox cmbRegion = phAddressFields.RegionComboBox;
            ComboBox cmbProvince = phAddressFields.ProvinceComboBox;
            ComboBox cmbCity = phAddressFields.CityComboBox;
            TextBox txtPostalCode = phAddressFields.PostalTextBox;


            txtAddress1?.TextChanged += (s, e) => txtAddress1.ClearFieldHighlight();
            cmbRegion?.TextChanged += (s, e) => cmbRegion.ClearFieldHighlight();
            cmbProvince?.TextChanged += (s, e) => cmbProvince.ClearFieldHighlight();
            cmbCity?.SelectedIndexChanged += (s, e) => cmbCity.ClearFieldHighlight();
            cmbBarangay?.SelectedIndexChanged += (s, e) => cmbBarangay.ClearFieldHighlight();
            txtPostalCode?.TextChanged += (s, e) => txtPostalCode.ClearFieldHighlight();
            cmbRSYearLevel?.SelectedIndexChanged += (s, e) => cmbRSYearLevel.ClearFieldHighlight();
            cmbRSEnrollmentStatus?.SelectedIndexChanged += (s, e) => cmbRSEnrollmentStatus.ClearFieldHighlight();
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
            phAddressFields.Location = new Point(label26.Location.X, label26.Location.Y + 26);
            phAddressFields.Size = new Size(850, 250);
            pnlStudentRegistrationContainer.Controls.Add(phAddressFields);
            phAddressFields.AddressLine2.Tag = "optional";
            phAddressFields.TabIndex = 7;
            AutoSetStudentId();
            mtbRSStudentPhoneNum.MakeCursorGotoStart();

            // FIRST create dynamic controls
            AddSuffixComboBox();

            // THEN set fonts (so they apply to all controls including dynamically created ones)
            SetRegistrationFontsTo12pt();

            // THEN mark required fields
            MarkRequiredFields();

            // THEN attach real-time validation
            AttachRealTimeValidation();
        }

        private void AutoSetStudentId()
        {
            int currentYear = DateTime.Now.Year;
            int sequence = AutoGenerators.GetNextStudentSequence(currentYear);
            txtRSStudentID.Text = AutoGenerators.FormatPupStudentNumber(currentYear, sequence, Services.StudentRegistrationService.CampusBranch, Services.StudentRegistrationService.IsTransferee);
        }

        private void btnStudentClearForm_Click(object sender, EventArgs e)
        {
            Clear_Form(pnlStudentRegistrationContainer);
            phAddressFields.ClearAddressFields();
        }

        private void Clear_Form(Control control)
        {
            foreach (Control ctrl in control.Controls)
            {
                if (ctrl.Tag?.ToString() == "ignore")
                    continue;
                if (ctrl is TextBox txt)
                {
                    txt.Clear();
                    txt.ClearFieldHighlight();
                }
                else if (ctrl is ComboBox cmb)
                {
                    cmb.SelectedIndex = 0;
                    cmb.ClearFieldHighlight();
                }
                else if (ctrl is MaskedTextBox mtb)
                {
                    mtb.Clear();
                    mtb.ClearFieldHighlight();
                }
                else if (ctrl is DateTimePicker dtp)
                {
                    dtp.Value = DateTime.Now.AddDays(-1);
                    dtp.ClearFieldHighlight();
                }

                if (ctrl.HasChildren)
                    Clear_Form(ctrl);
            }
        }

        private void btnBulkRegister_Click(object sender, EventArgs e)
        {
            using (BulkStudentRegistration bulkForm = new BulkStudentRegistration())
            {
                bulkForm.StartPosition = FormStartPosition.CenterScreen;
                bulkForm.ShowDialog();
            }
        }

        private void cmbRSEnrollmentStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            Services.StudentRegistrationService.IsTransferee = cmbRSEnrollmentStatus.SelectedItem != null && cmbRSEnrollmentStatus.SelectedItem.ToString() == "Transferee";
            AutoSetStudentId();
        }
    }
}
