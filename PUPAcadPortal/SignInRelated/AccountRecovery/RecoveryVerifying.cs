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
    public partial class RecoveryVerifying : UserControl
    {
        private AccountRecoveryService _recoveryService = new AccountRecoveryService();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Identifier { get; set; }

        public event Action OnVerifySuccess;

        public RecoveryVerifying()
        {
            InitializeComponent();
            txtPin.MakeCursorGotoStart();
        }

        private async void btnVerify_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPin.Text) || txtPin.Text.Length < 6)
            {
                MessageBox.Show("Please enter the full 6-digit code.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnVerify.Enabled = false;
            btnVerify.Text = "Verifying...";

            try
            {
                bool isValid = await _recoveryService.ValidateResetPinAsync(Identifier, txtPin.Text);

                if (isValid)
                {
                    OnVerifySuccess?.Invoke();
                }
                else
                {
                    MessageBox.Show("Invalid or expired code. Please check your email or request a new code.",
                                    "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"System error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnVerify.Enabled = true;
                btnVerify.Text = "Verify Code";
            }
        }
    }
}
