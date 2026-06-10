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
    public partial class ForgotPasswordForm : Form
    {
        private string _userIdentifier;
        private string _verifiedPin;

        public ForgotPasswordForm()
        {
            InitializeComponent();
            LoadRequestPhase();
        }

        private void LoadRequestPhase()
        {
            var reqControl = new RecoveryRequesting();
            reqControl.OnRequestSuccess += (id) =>
            {
                _userIdentifier = id;
                LoadVerifyPhase();
            };
            pnlHost.ShowView(reqControl);
        }

        private void LoadVerifyPhase()
        {
            var verifyControl = new RecoveryVerifying();
            verifyControl.Identifier = _userIdentifier;

            verifyControl.OnVerifySuccess += () =>
            {
                _verifiedPin = verifyControl.txtPin.Text;
                LoadResetPhase();
            };
            pnlHost.ShowView(verifyControl);
        }

        private void LoadResetPhase()
        {
            var resetControl = new RecoveryResetting();
            resetControl.Identifier = _userIdentifier;
            resetControl.Pin = _verifiedPin;
            pnlHost.ShowView(resetControl);
        }
    }
}
