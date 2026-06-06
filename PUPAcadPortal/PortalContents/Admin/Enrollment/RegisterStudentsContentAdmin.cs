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

        private StudentRegistrationService _service = new();

        public RegisterStudentsContentAdmin()
        {
            InitializeComponent();
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
            if (!pnlStudentRegistrationContainer.ValidateRegistration())
                return;
            if (!txtRSStudentEmailAdd.IsValidEmail())
                return;

            if (MessageBox.Show("Are you sure you want to register this student? This action cannot be undone.", "Confirm Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }

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
            btnStudentRegistration.Text = "Registering Student...";
            Application.UseWaitCursor = true;

            try
            {
                var registeredStudent = await _service.RegisterSingleStudent(dto);

                this.SafeUIUpdate(() =>
                {
                    MessageBox.Show(
                        $"Success! Student '{registeredStudent.User.FirstName} {registeredStudent.User.LastName}' has been securely registered.\n\n" +
                        $"Generated Student No: {registeredStudent.StudentNumber}\n" +
                        $"Generated Email: {registeredStudent.User.InstitutionalEmail}\n" +
                        "An email containing their credentials has beem sent to the specified email address.",
                        "Registration Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    btnStudentClearForm.PerformClick();
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
            phAddressFields.TabIndex = 7;
            AutoSetStudentId();
            cmbProgram_SelectedIndexChanged(sender, e);
            mtbRSStudentPhoneNum.MakeCursorGotoStart();
            if (cmbRSEnrollmentStatus.Items.Count > 0) cmbRSEnrollmentStatus.SelectedIndex = 0;
            if (cmbRSYearLevel.Items.Count > 0) cmbRSYearLevel.SelectedIndex = 0;

            // FIRST create dynamic controls
            AddSuffixComboBox();

            // THEN set fonts (so they apply to all controls including dynamically created ones)
            SetRegistrationFontsTo12pt();

            Validators.AttachLiveErrorClearers(pnlStudentRegistrationContainer);
            dtpRSStudentBirthDate.Leave += (s, e) => dtpRSStudentBirthDate.IsDateOfBirthValid();
            txtRSStudentEmailAdd.TextChanged += (s, e) => txtRSStudentEmailAdd.IsValidEmail();
        }

        private async Task AutoSetStudentId()
        {
            int currentYear = DateTime.Now.Year;
            int sequence = await AutoGenerators.GetNextStudentSequence(currentYear);
            txtRSStudentID.Text = AutoGenerators.FormatPupStudentNumber(currentYear, sequence, Services.StudentRegistrationService.CampusBranch, Services.StudentRegistrationService.IsTransferee);
        }

        private void btnStudentClearForm_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear this form?", "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            Clear_Form(pnlStudentRegistrationContainer);
            phAddressFields.ClearAddressFields();
            Validators.ClearErrors();
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
                }
                else if (ctrl is ComboBox cmb)
                {
                    cmb.SelectedIndex = 0;
                }
                else if (ctrl is MaskedTextBox mtb)
                {
                    mtb.Clear();
                }
                else if (ctrl is DateTimePicker dtp)
                {
                    dtp.Value = DateTime.Now.AddDays(-1);
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

        private async void cmbRSEnrollmentStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            Services.StudentRegistrationService.IsTransferee = cmbRSEnrollmentStatus.SelectedItem != null && cmbRSEnrollmentStatus.SelectedItem.ToString() == "Transferee";
            await AutoSetStudentId();
        }

        private async void cmbProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRSStudentProgram.SelectedItem != null)
            {
                string selectedProgram = cmbRSStudentProgram.Text.Trim() switch
                {
                    "BS Information Technology" => "BSIT",
                    _ => "No Program"
                };

                var activeYears = await _service.GetAvailableCurriculumYearsAsync(selectedProgram);

                this.SafeUIUpdate(() =>
                {
                    cmbCurriculumYear.DataSource = activeYears;

                    if (activeYears != null && activeYears.Count > 0)
                    {
                        cmbCurriculumYear.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbCurriculumYear.DataSource = null;
                        cmbCurriculumYear.Items.Add("None");
                        cmbCurriculumYear.SelectedIndex = 0;
                        cmbCurriculumYear.Enabled = false;
                    }
                });
            }
        }
    }
}
