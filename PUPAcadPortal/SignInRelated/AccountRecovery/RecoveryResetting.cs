using PUPAcadPortal.Services;
using PUPAcadPortal.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.SignInRelated.AccountRecovery
{
    public partial class RecoveryResetting : UserControl
    {
        private AccountRecoveryService _recoveryService = new AccountRecoveryService();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Identifier { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Pin { get; set; }

        public RecoveryResetting()
        {
            InitializeComponent();
            txtPass.TextChanged += TxtPass_TextChanged;
            btnShowPass.Click += ShowPass_Click;
            btnShowPass1.Click += ShowPass_Click;
        }

        private void TxtPass_TextChanged(object? sender, EventArgs e)
        {
            var validationResult = PasswordValidator.Validate(txtPass.Text);

            if (!validationResult.IsValid)
            {
                errorProvider1.SetError(txtPass, validationResult.ErrorMessage);
            }
            else
            {
                errorProvider1.SetError(txtPass, string.Empty);
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPass.Text))
            {
                MessageBox.Show("Password cannot be empty.");
                return;
            }

            if (txtPass.Text != txtConfirm.Text)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var validationResult = PasswordValidator.Validate(txtPass.Text);

            if (!validationResult.IsValid)
            {
                MessageBox.Show("Please fix the password errors before continuing.", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPass.Focus();
                return;
            }

            btnSave.Enabled = false;
            btnSave.Text = "Saving...";

            try
            {
                var result = await _recoveryService.UpdatePasswordAsync(Identifier, Pin, txtPass.Text);

                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.ParentForm?.Close();
                }
                else
                {
                    MessageBox.Show(result.Message, "Reset Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"System error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSave.Enabled = true;
                btnSave.Text = "Save Password";
            }
        }

        private void ShowPass_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            foreach (Control control in btn.Parent.Controls)
            {
                if (control is TextBox txt)
                    txt.UseSystemPasswordChar = !txt.UseSystemPasswordChar;
            }
        }
    }
}
