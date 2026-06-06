using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Data;
using PUPAcadPortal.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Misc
{
    public partial class EditProfileContent : UserControl
    {
        public EditProfileContent()
        {
            InitializeComponent();
        }

        private async void EditProfileContent_Load(object sender, EventArgs e)
        {
            await LoadData();

            MakeFieldsReadOnly(this);
        }

        private async Task LoadData()
        {
            try
            {
                using (var context = new Models.AppDbContext())
                {
                    var user = await context.Users.FindAsync(Data.UserSession.UserID);
                    if (user != null)
                    {
                        var role = await context.Roles.FindAsync(user.RoleId);
                        string idNumber = user.RoleId switch
                        {
                            1 => (await context.Admins
                                     .Where(a => a.UserId == user.UserId)
                                     .Select(a => (int?)a.AdminId)
                                     .FirstOrDefaultAsync())?.ToString() ?? "No ID Found",

                            2 => await context.Students
                                     .Where(s => s.UserId == user.UserId)
                                     .Select(s => s.StudentNumber)
                                     .FirstOrDefaultAsync() ?? "No ID Found",
                            3 => await context.Professors
                                     .Where(p => p.UserId == user.UserId)
                                     .Select(p => p.EmployeeId)
                                     .FirstOrDefaultAsync() ?? "No ID Found",
                            _ => "No ID Found"
                        };
                        this.SafeUIUpdate(async () =>
                        {
                            txtIDNumber.Text = idNumber;
                            txtUsername.Text = user.Username;
                            txtPassword.Text = user.PasswordHash != null ? "********" : "No password set";
                            txtPersonalEmail.Text = user.PersonalEmail ?? "No personal email set";
                            mtbPhone.Text = user.ContactNumber ?? "No contact number set";
                            phAddressFields1.AddressLine1.Text = user.AddressLine1 ?? "No address line 1 set";
                            phAddressFields1.AddressLine2.Text = user.AddressLine2 ?? "No address line 2 set";
                            phAddressFields1.RegionComboBox.Text = user.Region ?? "No region set";
                            phAddressFields1.ProvinceComboBox.Text = user.Province ?? "No province set";
                            phAddressFields1.CityComboBox.Text = user.CityMunicipality ?? "No city set";
                            phAddressFields1.BarangayComboBox.Text = user.Barangay ?? "No barangay set";
                            phAddressFields1.PostalTextBox.Text = user.PostalCode ?? "No postal code set";
                            Data.UserSession.Login(user.Username, user.UserId, user.FirstName, user.LastName, role.RoleName);
                            PUPAcadPortal.Events.InfoChangedEvent.RaiseInfoChanged();
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load user profile: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditInfo_Click(object sender, EventArgs e)
        {
            btnEditInfo.Click -= btnEditInfo_Click;
            btnEditInfo.Text = "Save Changes";
            btnEditInfo.Click += SaveChanges;
            ControlEnabler(this);
            txtPassword.Clear();
            txtConfirmPass.Visible = true;
            btnShowPass.Visible = true;
            btnShowConfirm.Visible = true;
            btnVerify.Visible = true;
        }

        private async void SaveChanges(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to update your information?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                MakeFieldsReadOnly(this);
                btnEditInfo.Text = "Saving Information...";
                btnEditInfo.Enabled = false;
                await UpdateInfo();
                btnEditInfo.Text = "Edit Information";
                btnEditInfo.Enabled = true;
                btnEditInfo.Click -= SaveChanges;
                btnEditInfo.Click += btnEditInfo_Click;
                txtPassword.UseSystemPasswordChar = true;
                txtConfirmPass.UseSystemPasswordChar = true;
                txtConfirmPass.Visible = false;
                btnVerify.Visible = false;
                btnShowPass.Visible = false;
                btnShowConfirm.Visible = false;
                txtPassword.ReadOnly = true;
                await LoadData();
            }
        }

        private async Task UpdateInfo()
        {
            try
            {
                using (var context = new Models.AppDbContext())
                {
                    var user = await context.Users.FindAsync(Data.UserSession.UserID);
                    if (user != null)
                    {
                        user.Username = txtUsername.Text;
                        user.PersonalEmail = txtPersonalEmail.Text;
                        user.ContactNumber = mtbPhone.Text;
                        user.AddressLine1 = phAddressFields1.AddressLine1.Text;
                        user.AddressLine2 = phAddressFields1.AddressLine2.Text;
                        user.Region = phAddressFields1.RegionComboBox.Text;
                        user.Province = phAddressFields1.ProvinceComboBox.Text;
                        user.CityMunicipality = phAddressFields1.CityComboBox.Text;
                        user.Barangay = phAddressFields1.BarangayComboBox.Text;
                        user.PostalCode = phAddressFields1.PostalTextBox.Text;
                    }
                    var c = await context.SaveChangesAsync();
                    if (c > 0)
                    {
                        MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No changes were made to the profile.", "No Changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update profile: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnShowPass_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
        }

        private void btnShowConfirm_Click(object sender, EventArgs e)
        {
            txtConfirmPass.UseSystemPasswordChar = !txtConfirmPass.UseSystemPasswordChar;
        }

        private void ControlEnabler(Control control)
        {
            foreach (Control childControl in control.Controls)
            {
                if (childControl.Name != "txtIDNumber")
                {
                    childControl.Enabled = true;
                }
                if (childControl is TextBox textBox)
                {
                    textBox.ReadOnly = false;
                }
                if (childControl is MaskedTextBox maskedTextBox)
                {
                    maskedTextBox.ReadOnly = false;
                }
                if (childControl.HasChildren)
                {
                    ControlEnabler(childControl);
                }
            }
        }

        private async void btnVerify_Click(object sender, EventArgs e)
        {
            btnVerify.Enabled = false;
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter your current password to verify.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnVerify.Enabled = true;
                return;
            }
            if (txtPassword.Text != txtConfirmPass.Text)
            {
                MessageBox.Show("Password and confirmation do not match. Please try again.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnVerify.Enabled = true;
                return;
            }
            using (var context = new Models.AppDbContext())
            {
                var user = await context.Users.FindAsync(Data.UserSession.UserID);
                if (user != null)
                {
                    if (user.PasswordHash != null && BCrypt.Net.BCrypt.Verify(txtPassword.Text, user.PasswordHash))
                    {
                        MessageBox.Show("Verification successful! You can now edit your password.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtPassword.Clear();
                        txtConfirmPass.Clear();
                        txtPassword.PlaceholderText = "Enter new password";
                        txtConfirmPass.PlaceholderText = "Confirm new password";
                        btnVerify.Text = "Save";
                        btnVerify.Click -= btnVerify_Click;
                        btnVerify.Click += SavePassword;
                        btnVerify.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Incorrect password. Please try again.", "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnVerify.Enabled = true;
                    }
                }
            }
        }

        private async void SavePassword(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text) || string.IsNullOrWhiteSpace(txtConfirmPass.Text))
            {
                MessageBox.Show("Please fill in both password fields.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtPassword.Text != txtConfirmPass.Text)
            {
                MessageBox.Show("New password and confirmation do not match. Please try again.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (var context = new Models.AppDbContext())
            {
                var user = await context.Users.FindAsync(Data.UserSession.UserID);
                if (user != null)
                {
                    if (user.PasswordHash != null && BCrypt.Net.BCrypt.Verify(txtPassword.Text, user.PasswordHash))
                    {
                        MessageBox.Show("New password cannot be the same as the current password. Please choose a different password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text);
                    await context.SaveChangesAsync();
                    MessageBox.Show("Password updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnVerify.Click -= SavePassword;
                    btnVerify.Click += btnVerify_Click;
                    btnVerify.Text = "Verify";
                    txtPassword.Clear();
                    txtConfirmPass.Clear();
                    txtPassword.PlaceholderText = "Enter current password";
                    txtConfirmPass.PlaceholderText = "Confirm current password";
                    txtPassword.UseSystemPasswordChar = true;
                    txtConfirmPass.UseSystemPasswordChar = true;
                    txtConfirmPass.Visible = false;
                    btnVerify.Visible = false;
                    btnShowPass.Visible = false;
                    btnShowConfirm.Visible = false;
                    txtPassword.ReadOnly = true;
                    await LoadData();
                }
            }
        }

        private void MakeFieldsReadOnly(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.ReadOnly = !textBox.ReadOnly;
                }
                else if (control is MaskedTextBox maskedTextBox)
                {
                    maskedTextBox.ReadOnly = !maskedTextBox.ReadOnly;
                }
                else if (control is ComboBox || control is CheckBox)
                {
                    control.Enabled = !control.Enabled;
                    if (control.Enabled)
                    {
                        control.Enabled = !control.Enabled;
                    }
                }
                else if (control.HasChildren)
                {
                    MakeFieldsReadOnly(control);
                }
            }
        }

        private void MakeAddressFieldsReadOnly(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.ReadOnly = !textBox.ReadOnly;
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.Enabled = !comboBox.Enabled;
                }
                else if (control.HasChildren)
                {
                    MakeAddressFieldsReadOnly(control);
                }
            }
        }
    }
}
