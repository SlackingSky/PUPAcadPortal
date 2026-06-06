using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using PUPAcadPortal.Services;
using PUPAcadPortal.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Admin.Enrollment
{
    public partial class AccountsContentAdmin : UserControl
    {
        private AccountingService _accountingService = new AccountingService();
        private List<AccountingData> _allRecords = new List<AccountingData>();

        public AccountsContentAdmin()
        {
            InitializeComponent();

            // Apply the PUP Aesthetic immediately on initialization
            ApplyPUPAesthetic(dgvAccountingRecords);

            pnlAccountingRecordsContent.Layout += (s, e) => ResizeAccountingRecordsContent();
            this.Load += AccountsContentAdmin_Load;

            btnARSearch.Click += btnSearch_Click;
            txtARSearchBar.KeyDown += txtSearch_KeyDown;

            dgvAccountingRecords.CellDoubleClick += dgvAccountingRecords_CellDoubleClick;
            dgvAccountingRecords.CellFormatting += dgvAccountingRecords_CellFormatting;
            dgvAccountingRecords.CellToolTipTextNeeded += dgvAccountingRecords_CellToolTipTextNeeded;
        }

        private void ApplyPUPAesthetic(DataGridView grid)
        {
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = Color.FromArgb(220, 220, 220);
            grid.RowHeadersVisible = false;
            grid.AllowUserToAddRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            grid.EnableHeadersVisualStyles = false;
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = Color.FromArgb(128, 0, 0);
            headerStyle.ForeColor = Color.White;
            headerStyle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.ColumnHeadersDefaultCellStyle = headerStyle;
            grid.ColumnHeadersHeight = 45;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            grid.DefaultCellStyle.Font = new Font("Segoe UI", 11F);
            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = Color.Black;

            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(128, 0, 0);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;

            grid.RowTemplate.Height = 40;
        }

        private async void AccountsContentAdmin_Load(object sender, EventArgs e)
        {
            dgvAccountingRecords.AutoGenerateColumns = true;
            await LoadAccountingDataAsync();
        }

        private async Task LoadAccountingDataAsync()
        {
            try
            {
                string activePeriod = GlobalSession.ActiveAcademicPeriod;

                _allRecords = await _accountingService.GetStudentAccountingRecordsAsync(activePeriod);

                UpdateSummaryCards(_allRecords);
                BindGrid(_allRecords);
                dgvAccountingRecords.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading records: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSummaryCards(List<AccountingData> records)
        {
            decimal totalAssessed = records.Sum(r => r.TotalAmount);
            decimal totalPaid = records.Sum(r => r.PaidAmount);
            decimal totalUnpaid = records.Sum(r => r.UnpaidAmount);

            lblTotalAmount.Text = $"₱{totalAssessed:N2}";
            lblPaid.Text = $"₱{totalPaid:N2}";
            lblUnpaid.Text = $"₱{totalUnpaid:N2}";
        }

        private void BindGrid(List<AccountingData> data)
        {
            var sortedData = data
            .OrderByDescending(d => d.Status == "Pending Payment")
            .ThenBy(d => d.FullName)
            .ToList();
            dgvAccountingRecords.DataSource = new BindingList<AccountingData>(sortedData);

            if (dgvAccountingRecords.Columns["StudentId"] != null)
            {
                dgvAccountingRecords.Columns["StudentId"].Visible = false;
            }

            if (dgvAccountingRecords.Columns["StudentNo"] != null)
            {
                dgvAccountingRecords.Columns["StudentNo"].HeaderText = "Student Number";
                dgvAccountingRecords.Columns["StudentNo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            if (dgvAccountingRecords.Columns["Program"] != null)
            {
                dgvAccountingRecords.Columns["Program"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgvAccountingRecords.Columns["Program"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvAccountingRecords.Columns["FullName"] != null)
            {
                dgvAccountingRecords.Columns["FullName"].HeaderText = "Full Name";
                dgvAccountingRecords.Columns["FullName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (dgvAccountingRecords.Columns["Status"] != null)
            {
                dgvAccountingRecords.Columns["Status"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            if (dgvAccountingRecords.Columns["TotalAmount"] != null)
            {
                dgvAccountingRecords.Columns["TotalAmount"].HeaderText = "Total Assessed";
                dgvAccountingRecords.Columns["TotalAmount"].DefaultCellStyle.Format = "₱#,##0.00";
                dgvAccountingRecords.Columns["TotalAmount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgvAccountingRecords.Columns["TotalAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvAccountingRecords.Columns["PaidAmount"] != null)
            {
                dgvAccountingRecords.Columns["PaidAmount"].HeaderText = "Amount Paid";
                dgvAccountingRecords.Columns["PaidAmount"].DefaultCellStyle.Format = "₱#,##0.00";
                dgvAccountingRecords.Columns["PaidAmount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgvAccountingRecords.Columns["PaidAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvAccountingRecords.Columns["UnpaidAmount"] != null)
            {
                dgvAccountingRecords.Columns["UnpaidAmount"].HeaderText = "Balance";
                dgvAccountingRecords.Columns["UnpaidAmount"].DefaultCellStyle.Format = "₱#,##0.00";
                dgvAccountingRecords.Columns["UnpaidAmount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgvAccountingRecords.Columns["UnpaidAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PerformSearch();
                e.SuppressKeyPress = true;
            }
        }

        private void PerformSearch()
        {
            string keyword = txtARSearchBar.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                BindGrid(_allRecords);
                UpdateSummaryCards(_allRecords);
                return;
            }

            var filteredList = _allRecords.Where(r =>
                (r.FullName != null && r.FullName.ToLower().Contains(keyword)) ||
                (r.StudentNo != null && r.StudentNo.ToLower().Contains(keyword)) ||
                (r.Status != null && r.Status.ToLower().Contains(keyword))
            ).ToList();

            BindGrid(filteredList);
            UpdateSummaryCards(filteredList);
        }

        private void dgvAccountingRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var selectedRecord = dgvAccountingRecords.Rows[e.RowIndex].DataBoundItem as AccountingData;

            if (selectedRecord != null)
            {
                using (var manageForm = new ManageAccountForm(selectedRecord.StudentId, selectedRecord.FullName, selectedRecord.UnpaidAmount))
                {
                    manageForm.StartPosition = FormStartPosition.CenterParent;

                    var result = manageForm.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        AccountsContentAdmin_Load(this, EventArgs.Empty);
                    }
                }
            }
        }

        private void AdminAccountsContent_Resize(object sender, EventArgs e)
        {
            if (pnlAccountingRecordsContent.Visible)
            {
                ResizeAccountingRecordsContent();
            }
        }

        private void ResizeAccountingRecordsContent()
        {
            if (!pnlAccountingRecordsContent.Visible) return;

            int contentWidth = pnlAccountingRecordsContent.Width - 32;
            int gap = 10;

            int cardWidth = (contentWidth - (gap * 2)) / 3;

            pnlARTotalAmount.Width = cardWidth;
            pnlARPaidAmount.Width = cardWidth;
            pnlARUnpaidAmount.Width = cardWidth;

            pnlARTotalAmount.Left = 16;
            pnlARPaidAmount.Left = 16 + cardWidth + gap;
            pnlARUnpaidAmount.Left = 16 + (cardWidth + gap) * 2;
        }

        private void dgvAccountingRecords_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvAccountingRecords.Columns["Status"] != null &&
                e.ColumnIndex == dgvAccountingRecords.Columns["Status"].Index)
            {
                var row = dgvAccountingRecords.Rows[e.RowIndex];
                var rowData = row.DataBoundItem as AccountingData;

                if (rowData != null)
                {
                    e.CellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

                    if (rowData.Status == "Pending Payment")
                    {
                        e.CellStyle.BackColor = Color.DarkOrange;
                        e.CellStyle.ForeColor = Color.White;
                        e.CellStyle.SelectionBackColor = Color.OrangeRed;
                        e.CellStyle.SelectionForeColor = Color.White;
                    }
                    else if (rowData.Status == "Fully Paid")
                    {
                        e.CellStyle.BackColor = Color.SeaGreen;
                        e.CellStyle.ForeColor = Color.White;
                        e.CellStyle.SelectionBackColor = Color.MediumSeaGreen;
                        e.CellStyle.SelectionForeColor = Color.White;
                    }
                }
            }
        }

        private void dgvAccountingRecords_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                e.ToolTipText = "Double-click anywhere on this row to manage the student's account.";
            }
        }
    }
}