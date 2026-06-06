using PUPAcadPortal.Services;
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
    }
}
