using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using PUPAcadPortal.Services;
using PUPAcadPortal.Utils;
using System;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Admin.Enrollment
{
    public partial class ManageAccountForm : Form
    {
        private AccountingService _accountingService = new AccountingService();
        private int _studentId;
        private int _accountId;
        private string _studentName;
        private decimal _currentBalance;

        public ManageAccountForm(int studentId, string studentName, decimal balance)
        {
            InitializeComponent();
            _studentId = studentId;
            _studentName = studentName;
            _currentBalance = balance;

            this.Load += ManageAccountForm_Load;
            btnSubmitPayment.Click += btnSubmitPayment_Click;
            btnEditReference.Click += btnEditReference_Click;
        }

        private async void ManageAccountForm_Load(object sender, EventArgs e)
        {
            // Setup UI
            this.Text = $"Manage Account - {_studentName}";
            lblStudentName.Text = _studentName;
            lblBalance.Text = $"Current Balance: ₱{_currentBalance:N2}";

            if (_currentBalance <= 0)
            {
                txtPaymentAmount.Enabled = false;
                txtReference.Enabled = false;
                btnSubmitPayment.Enabled = false;
                btnSubmitPayment.Text = "Fully Paid";
            }
            else
            {
                txtPaymentAmount.Text = _currentBalance.ToString("0.00");
                txtReference.Text = GenerateReferenceNumber();
                LockReferenceBox();
            }

            await LoadDetailsAsync();
        }

        private void LockReferenceBox()
        {
            txtReference.ReadOnly = true;
            txtReference.BackColor = Color.WhiteSmoke;
            btnEditReference.Visible = true;
        }

        private void btnEditReference_Click(object sender, EventArgs e)
        {
            txtReference.ReadOnly = false;
            txtReference.BackColor = Color.White;
            btnEditReference.Visible = false;

            txtReference.Focus();
            txtReference.SelectAll();
        }

        private async Task LoadDetailsAsync()
        {
            string activePeriod = GlobalSession.ActiveAcademicPeriod;
            var details = await _accountingService.GetAccountDetailsAsync(_studentId, activePeriod);

            if (details != null)
            {
                _accountId = details.AccountId;

                dgvFees.DataSource = details.Fees;
                dgvPayments.DataSource = details.Payments;

                if (dgvFees.Columns["Amount"] != null)
                    dgvFees.Columns["Amount"].DefaultCellStyle.Format = "₱#,##0.00";

                if (dgvPayments.Columns["Amount"] != null)
                    dgvPayments.Columns["Amount"].DefaultCellStyle.Format = "₱#,##0.00";
            }
        }

        private async void btnSubmitPayment_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtPaymentAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid payment amount.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (amount > _currentBalance)
            {
                MessageBox.Show("Payment amount cannot exceed the remaining balance.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string reference = txtReference.Text.Trim();

            if (string.IsNullOrEmpty(reference))
            {
                MessageBox.Show("A reference number is required to process the payment.", "Missing Reference", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Record payment of ₱{amount:N2} for {_studentName}?", "Confirm Payment", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                btnSubmitPayment.Enabled = false;

                var result = await _accountingService.RecordPaymentAsync(_accountId, amount, reference, UserSession.UserID);

                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Payment Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(result.Message, "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnSubmitPayment.Enabled = true;
                }
            }
        }

        private string GenerateReferenceNumber()
        {
            string datePart = DateTime.Now.ToString("yyyy");

            string timePart = DateTime.Now.ToString("HHmmss");

            Random rnd = new Random();
            string salt = rnd.Next(10, 100).ToString();

            return $"PAY-{datePart}-{timePart}{salt}";
        }
    }
}