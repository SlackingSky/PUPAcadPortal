using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PUPAcadPortal.PortalContents.Student.Enrollment;
using PUPAcadPortal.Data;
using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Models;
using System.Threading.Tasks;
using System.Linq;


namespace PUPAcadPortal.PortalContents.Student.Enrollment
{
    public partial class AccountsContentStudent : UserControl
    {
        private StudentDataService _dataService;
        private DataTable accountsTable;

        private const int SidePadding = 16;
        private const int CardGap = 10;
        public AccountsContentStudent()
        {
            InitializeComponent();
            this.Resize += StudentPortal_AccountsResize;
            if (EnrollmentContentStudent.dataService is null)
            {
                EnrollmentContentStudent.dataService = new StudentDataService();
            }
            _dataService = EnrollmentContentStudent.dataService;
        }
        // ─────────────────────────────────────────────────────────────────
        // ACCOUNTS – Using DataTable with existing columns
        // ─────────────────────────────────────────────────────────────────

        private void Accounts_Initialize()
        {
            pnlAccountsContent.Visible = true;
            CreateAccountsTable();

            // Initially hide the enrollment status card
            pnlEnrollStatusCard.Visible = false;
            lblEnrollStatus.Visible = false;

            SetupAccountsGridStyle();

            if (_dataService.IsStudentEnrolled())
            {
                ShowEnrolledState();
            }
            else
            {
                ShowNotEnrolledState();
            }
            PopulateSemesterFilter();
            cmbSelectSem.SelectedIndexChanged -= cmbSelectSem_SelectedIndexChanged;
            cmbSelectSem.SelectedIndexChanged += cmbSelectSem_SelectedIndexChanged;
            cmbSelectSem.SelectedIndex = 0;
        }

        private void CreateAccountsTable()
        {
            accountsTable = new DataTable();
            // Column names must match the existing DataGridView column Name properties
            accountsTable.Columns.Add("colAccountsRefID", typeof(string));
            accountsTable.Columns.Add("colAccountsDescription", typeof(string));
            accountsTable.Columns.Add("colAccountsAmount", typeof(string));
            accountsTable.Columns.Add("colAccountsDueDate", typeof(string));
            accountsTable.Columns.Add("colAccountsStatus", typeof(string));
            accountsTable.Columns.Add("colAccountsPaidDate", typeof(string));

            // Prevent auto-generation of new columns
            dgvAccounts.AutoGenerateColumns = false;
            dgvAccounts.DataSource = accountsTable;

            // Manually bind each existing column to the corresponding DataTable column
            dgvAccounts.Columns["colAccountsRefID"].DataPropertyName = "colAccountsRefID";
            dgvAccounts.Columns["colAccountsDescription"].DataPropertyName = "colAccountsDescription";
            dgvAccounts.Columns["colAccountsAmount"].DataPropertyName = "colAccountsAmount";
            dgvAccounts.Columns["colAccountsDueDate"].DataPropertyName = "colAccountsDueDate";
            dgvAccounts.Columns["colAccountsStatus"].DataPropertyName = "colAccountsStatus";
            dgvAccounts.Columns["colAccountsPaidDate"].DataPropertyName = "colAccountsPaidDate";
        }

        private async Task LoadAccountsDataAsync(string selectedSemester = "All")
        {
            accountsTable.Rows.Clear();
            int currentStudentId = UserSession.StudentID ?? 0;

            if (currentStudentId == 0) return;

            using (var context = new AppDbContext())
            {
                // Fetch the student's account and pull payment histories from the database
                var studentAccount = await context.StudentAccounts
                    .Include(sa => sa.PaymentHistories)
                    .FirstOrDefaultAsync(sa => sa.StudentId == currentStudentId);

                if (studentAccount == null || studentAccount.PaymentHistories == null) return; // If no account or payment history, just return (table will be empty)

                // filter the records based on the selected sem
                var records = studentAccount.PaymentHistories.AsEnumerable();

                // If the user selected something other than "All" (e.g., "First Semester"),it'll filter the rows dynamically
                if (!string.Equals(selectedSemester, "All", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(selectedSemester))
                {
                    records = records.Where(r => r.Description != null &&
                                                 r.Description.IndexOf(selectedSemester, StringComparison.OrdinalIgnoreCase) >= 0);
                }

                foreach (var record in records)
                {
                    accountsTable.Rows.Add(
                        record.ReferenceId ?? "N/A",
                        record.Description,
                        $"₱{record.Amount:N2}",
                        record.DueDate.ToString("MM/dd/yyyy") ,
                        record.Status,
                        record.PaidDate?.ToString("MM/dd/yyyy") ?? ""
                    );
                }
            }

            // Refresh the Total Assessment, Total Paid, and Balance boxes based on the visible rows
            UpdateAccountsSummary();
        }

        private void SetupAccountsGridStyle()
        {
            dgvAccounts.EnableHeadersVisualStyles = false;
            dgvAccounts.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvAccounts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvAccounts.GridColor = Color.FromArgb(220, 220, 220);
            dgvAccounts.BackgroundColor = Color.White;
            dgvAccounts.BorderStyle = BorderStyle.None;
            dgvAccounts.RowHeadersVisible = false;
            dgvAccounts.AllowUserToAddRows = false;
            dgvAccounts.AllowUserToResizeRows = false;
            dgvAccounts.AllowUserToResizeColumns = false;
            dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Header styling – light gray, no blue
            dgvAccounts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);
            dgvAccounts.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(60, 60, 60);
            dgvAccounts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvAccounts.ColumnHeadersHeight = 45;
            dgvAccounts.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 247, 250);
            dgvAccounts.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(60, 60, 60);

            // Row styling
            dgvAccounts.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgvAccounts.DefaultCellStyle.ForeColor = Color.FromArgb(60, 60, 60);
            dgvAccounts.DefaultCellStyle.Padding = new Padding(5, 10, 5, 10);
            dgvAccounts.RowTemplate.Height = 50;
            dgvAccounts.DefaultCellStyle.SelectionBackColor = Color.White;
            dgvAccounts.DefaultCellStyle.SelectionForeColor = Color.FromArgb(60, 60, 60);

            // Wrap description column text and auto-size rows
            dgvAccounts.Columns["colAccountsDescription"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvAccounts.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Force Reference ID column header and cell colors
            dgvAccounts.Columns["colAccountsRefID"].HeaderCell.Style.BackColor = Color.FromArgb(245, 247, 250);
            dgvAccounts.Columns["colAccountsRefID"].DefaultCellStyle.ForeColor = Color.Gray;

            dgvAccounts.CellFormatting -= dgvAccounts_CellFormatting; // Prevent handler bundling duplicates
            dgvAccounts.CellFormatting += dgvAccounts_CellFormatting;
        }

        private void dgvAccounts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Amount column – bold
            if (e.ColumnIndex == dgvAccounts.Columns["colAccountsAmount"].Index && e.Value != null)
            {
                e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                e.CellStyle.ForeColor = Color.FromArgb(30, 30, 30);
            }

            // Status column – color coding
            if (e.ColumnIndex == dgvAccounts.Columns["colAccountsStatus"].Index && e.Value != null)
            {
                string status = e.Value.ToString();
                if (status == "Paid")
                {
                    e.CellStyle.ForeColor = Color.Green;
                    e.CellStyle.BackColor = Color.FromArgb(220, 255, 220);
                }
                else if (status == "Pending")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(180, 120, 0);
                    e.CellStyle.BackColor = Color.FromArgb(255, 243, 200);
                }
            }

            // Reference ID – gray
            if (e.ColumnIndex == dgvAccounts.Columns["colAccountsRefID"].Index && e.Value != null)
            {
                e.CellStyle.ForeColor = Color.Gray;
            }
        }

        private void UpdateAccountsSummary()
        {
            decimal totalAssessment = 0;
            decimal totalPaid = 0;

            // Use the DataTable's default view (respects filtering)
            DataTable dt = (DataTable)dgvAccounts.DataSource;
            if (dt == null) return;

            foreach (DataRow row in dt.Rows)
            {
                string amountStr = row["colAccountsAmount"]?.ToString() ?? "";
                string status = row["colAccountsStatus"]?.ToString() ?? "";

                amountStr = amountStr.Replace("₱", "").Replace(",", "").Trim(); 
                if (decimal.TryParse(amountStr, out decimal amount))
                {
                    totalAssessment += amount;
                    if (status.Equals("Paid", StringComparison.OrdinalIgnoreCase))
                        totalPaid += amount;
                }
            }

            decimal balance = totalAssessment - totalPaid;

            lblTAPeso.Text = $"₱{totalAssessment:N2}";
            lblTPPeso.Text = $"₱{totalPaid:N2}";
            lblBalancePeso.Text = $"₱{balance:N2}";
            dgvAccounts.ClearSelection();
        }

        private void PopulateSemesterFilter()
        {
            cmbSelectSem.Items.Clear();
            cmbSelectSem.Items.Add("All");

            int currentStudentId = UserSession.StudentID ?? 0;
            if (currentStudentId == 0) return;

            // Read directly from DB context instead of the empty table to fix startup population
            using (var context = new AppDbContext())
            {
                var studentAccount = context.StudentAccounts
                    .Include(sa => sa.PaymentHistories)
                    .FirstOrDefault(sa => sa.StudentId == currentStudentId);

                if (studentAccount?.PaymentHistories == null) return;

                foreach (var record in studentAccount.PaymentHistories)
                {
                    string description = record.Description ?? "";
                    if (description.Contains("First Semester") && !cmbSelectSem.Items.Contains("First Semester"))
                        cmbSelectSem.Items.Add("First Semester");
                    else if (description.Contains("Second Semester") && !cmbSelectSem.Items.Contains("Second Semester"))
                        cmbSelectSem.Items.Add("Second Semester");
                }
            }
        }

        private async void cmbSelectSem_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cmbSelectSem.SelectedItem?.ToString() ?? "All";

            if (!_dataService.IsStudentEnrolled())
            {
                accountsTable.Rows.Clear();
                accountsTable.Rows.Add("N/A", "Complete enrollment to view payment details.", "₱0.00", "", "Pending", "");
                return;
            }
            await LoadAccountsDataAsync(selected);
        }


        private void StudentPortal_AccountsResize(object sender, EventArgs e)
        {
            if (!IsHandleCreated) return;

            int contentWidth = pnlAccountsContent.Width - (SidePadding * 2);
            int cardWidth = (contentWidth - (CardGap * 2)) / 3;

            // --- Top summary cards ---
            pnlTotalAssessment.Width = cardWidth;
            pnlTotalPaid.Width = cardWidth;
            pnlBalance.Width = cardWidth;
            pnlTotalAssessment.Left = SidePadding;
            pnlTotalPaid.Left = SidePadding + cardWidth + CardGap;
            pnlBalance.Left = SidePadding + (cardWidth * 2) + (CardGap * 2);
            pbPaid.Left = (pnlTotalPaid.Width - 95) - 30;
            pbTotalAssessment.Left = (pnlTotalPaid.Width - 95) - 30;
            pbBalance.Left = (pnlTotalPaid.Width - 95) - 30;

            // --- Free Education panel (full width) ---
            pnlAccountsFreeEd.Width = contentWidth;
            // Adjust the description label inside it to wrap and fill the available width
            // The label is inside pnlAccountsFreeEd, positioned to the right of the picture box
            // Picture box is at Left=12, Width=78, so label starts at 96 and should fill the rest minus a margin
            int descLabelWidth = pnlAccountsFreeEd.Width - 96 - 20; // 20px right margin
            if (descLabelWidth > 0)
            {
                lblDescriptionFreeEducProg.Width = descLabelWidth;
                // Also adjust the note label if needed
                lblNoteFreeEducProg.Width = descLabelWidth;
            }

            // --- Semester selection panel ---
            pnlAccountsSelectSem.Width = contentWidth;
            pnlAccountsSelectSem.Top = pnlAccountsFreeEd.Bottom + 20;

            // --- Payment History label and Download button ---
            lblPaymentHistory.Top = pnlAccountsSelectSem.Bottom + 20;
            lblPaymentHistory.Left = SidePadding;

            btnAccountsDownloadStatement.Top = pnlAccountsSelectSem.Bottom + 15; // align with label
            btnAccountsDownloadStatement.Left = contentWidth - btnAccountsDownloadStatement.Width + SidePadding;

            // --- DataGridView ---
            dgvAccounts.Top = lblPaymentHistory.Bottom + 10;
            dgvAccounts.Width = contentWidth;
            dgvAccounts.Height = 300; // fixed height, you can adjust or make dynamic

            // --- Payment Methods section ---
            int paymentCardWidth = (contentWidth - CardGap) / 2;

            lblPaymentMethods.Top = dgvAccounts.Bottom + 20;
            lblPaymentMethods.Left = SidePadding;

            pnlOnlinePayment.Width = paymentCardWidth;
            pnlCashier.Width = paymentCardWidth;
            pnlOnlinePayment.Left = SidePadding;
            pnlCashier.Left = SidePadding + paymentCardWidth + CardGap;
            pnlOnlinePayment.Top = lblPaymentMethods.Bottom + 10;
            pnlCashier.Top = lblPaymentMethods.Bottom + 10;
            btnPaymentSlip.Width = pnlOnlinePayment.Width - 25;
            btnPayOnline.Width = pnlOnlinePayment.Width - 25;

            // --- Enrollment Status section ---
            lblEnrollStatus.Top = pnlOnlinePayment.Bottom + 20;
            lblEnrollStatus.Left = SidePadding;

            pnlEnrollStatusCard.Width = contentWidth;
            pnlEnrollStatusCard.Top = lblEnrollStatus.Bottom + 10;

            // --- Spacer panel (ensures scrolling works) ---
            pnlSpaceProviderAccounts.Top = pnlEnrollStatusCard.Bottom + 20;
            pnlSpaceProviderAccounts.Height = 50;
        }

        private void ShowEnrolledState()
        {
            // Show the enrollment status card
            pnlEnrollStatusCard.Visible = true;
            lblEnrollStatus.Visible = true;

            // Enable enrollment-dependent features
            btnAccountsDownloadStatement.Enabled = true;
            btnAccountsDownloadStatement.BackColor = Color.Maroon;
        }

        // Show not enrolled state (empty/placeholder data)
        private void ShowNotEnrolledState()
        {
            // Hide the enrollment status card
            pnlEnrollStatusCard.Visible = false;
            lblEnrollStatus.Visible = false;

            // Clear or show placeholder in accounts table
            accountsTable.Rows.Clear();
            accountsTable.Rows.Add("N/A", "Complete enrollment to view payment details.", "₱0.00", "", "Pending", "");

            // Disable enrollment-dependent features
            btnAccountsDownloadStatement.Enabled = false;
            btnAccountsDownloadStatement.BackColor = Color.DarkGray;

            // Update summary to show zeros
            lblTAPeso.Text = "₱0.00";
            lblTPPeso.Text = "₱0.00";
            lblBalancePeso.Text = "₱0.00";
        }

        private void btnAccountsDownloadStatement_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Statement of Account would be generated here (demo).", "Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AccountsContentStudent_Load(object sender, EventArgs e)
        {
            Accounts_Initialize();
            if (EnrollmentContentStudent.isEnrolled)
            {
                // Update the accounts page card
                lblEnrollStatusTitle.Text = "Officially Enrolled";
                lblEnrollStatusDesc.Text = "You are now officially enrolled. Your subjects have been confirmed.";
                pnlEnrollStatusCard.BackColor = Color.FromArgb(220, 255, 220);
                pictureBox50.BackColor = Color.Green;

                // 12. Show the enrollment status card on accounts page (was hidden before)
                pnlEnrollStatusCard.Visible = true;
                lblEnrollStatus.Visible = true;

                ShowEnrolledState();
            }
        }
    }
}