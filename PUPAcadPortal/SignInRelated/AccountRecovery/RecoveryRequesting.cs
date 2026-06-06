using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PUPAcadPortal.Services;

namespace PUPAcadPortal.SignInRelated.AccountRecovery
{
    public partial class RecoveryRequesting : UserControl
    {
        private AccountRecoveryService _recoveryService = new AccountRecoveryService();

        public event Action<string> OnRequestSuccess;

        public RecoveryRequesting()
        {
            InitializeComponent();
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInput.Text))
            {
                MessageBox.Show("Please enter your email or username.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnSend.Enabled = false;
            btnSend.Text = "Sending...";

            try
            {
                var result = await _recoveryService.RequestPasswordResetAsync(txtInput.Text);

                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Code Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OnRequestSuccess?.Invoke(txtInput.Text);
                }
                else
                {
                    MessageBox.Show(result.Message, "Reset Request Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSend.Enabled = true;
                btnSend.Text = "Send Reset Code";
            }
        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
