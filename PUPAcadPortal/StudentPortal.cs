using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class StudentPortal : Form
    {
        private Button clickedButton;
        private Color defaultColor = Color.Maroon;
        private Color selectedColor = Color.FromArgb(109, 0, 0);

        private bool headerChecked = false;

        private const int SidePadding = 16;
        private const int CardGap = 10;
        private const int EnrollmentGridHeight = 300;

        // ---------- NEW: Store original enrollment data ----------
        private List<string[]> enrollmentData = new List<string[]>();

        public StudentPortal()
        {
            InitializeComponent();
            txtEnrollSearch.KeyDown += txtEnrollSearch_KeyDown;
            this.Resize += StudentPortal_EnrollmentResize;
            this.Resize += StudentPortal_AccountsResize;

            pnlContainerAdminPortal.Dock = DockStyle.Fill;
            pnlContainerAdminPortal.AutoScroll = true;
        }

        private void changeButtonColor(Button button)
        {
            if (clickedButton != null)
            {
                clickedButton.BackColor = defaultColor;
            }
            clickedButton = button;
            pnlYellow.Visible = true;
            pnlYellow.Parent = clickedButton.Parent;
            pnlYellow.BringToFront();
            clickedButton.BackColor = selectedColor;
        }

        //Method na nagpapakita ng content ng bawat button, wala akong maisip na iba kaya eto
        private void showContent(Button button)
        {
            Dictionary<Button, Panel> contents = new Dictionary<Button, Panel> { };
            contents.Add(btnDashboard, pnlDashboardContent);
            contents.Add(btnEnrollment, pnlEnrollContent);
            contents.Add(btnCourses, pnlCoursesContent);
            contents.Add(btnAccounts, pnlAccountsContentHolder);
            //Kada button na aadd, maglagay ng panel sa form at lagay dito
            foreach (KeyValuePair<Button, Panel> content in contents)
            {
                if (content.Key == clickedButton)
                {
                    //Automatic positioning, wag pakialaman maliban nalang kung binago ang position ng sidebar
                    content.Value.Location = new Point(pnlSidebar.Size.Width, pnlHeader.Size.Height);
                    content.Value.Visible = true;
                }
                else
                {
                    content.Value.Visible = false;
                }
            }
        }

        //Method para pag pinindot yung X sa taas o mag alt-F4, icclose lahat ng forms para di magerror pag ni run uli
        private void StudentPortal_Closing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Are you sure you want to exit?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                    Application.Exit();
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnEnrollment_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnCourses_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnAccounts_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }
        private void btnLMS_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
            pnllmsSubmenu.Visible = !pnllmsSubmenu.Visible;
            if (pnllmsSubmenu.Visible)
                btnLMS.Text = " LMS                                       ⌄";
            else
                btnLMS.Text = " LMS                                        ›";
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StudentPortal_Load(object sender, EventArgs e)
        {
            Enrollment_Initialize();
            Accounts_Initialize();
        }

        //Shared Utilities by Enrollment and Accounts
        private void FitContentPanel(Panel panel)
        {
            panel.Width = this.ClientSize.Width - pnlSidebar.Width;
            panel.Height = this.ClientSize.Height - pnlHeader.Height;
            panel.Location = new Point(pnlSidebar.Width, pnlHeader.Height);
        }

        // ==================== ENROLLMENT ====================

        private void Enrollment_Initialize()
        {
            Enrollment_LoadData();
            Enrollment_UpdateTotalUnits();
        }

        // ---------- MODIFIED: Load data into both the list and the grid ----------
        private void Enrollment_LoadData()
        {
            // Clear previous data
            enrollmentData.Clear();
            dgvEnrollment.Rows.Clear();

            // Add dummy rows
            enrollmentData.Add(new string[] { "COMP 009", "Object Oriented Programming", "3", "MWF 7:30AM", "Enrolled" });
            enrollmentData.Add(new string[] { "ELEC IT-FE2", "BSIT Free Elective 2", "3", "TTH 9:00AM - 12:00PM", "Pending" });
            enrollmentData.Add(new string[] { "INTE 202", "Integrative Programming", "3", "MWF 10:00AM - 10:02AM", "Enrolled" });
            enrollmentData.Add(new string[] { "PATHFIT 4", "Physical Activity Towards Health and Fitness 4", "2", "TTH 1:00PM - 3:00PM", "Dropped" });

            // Populate DataGridView
            foreach (var row in enrollmentData)
            {
                dgvEnrollment.Rows.Add(false, row[0], row[1], row[2], row[3], row[4], "More");
            }

            Enrollment_UpdateTotalUnits();
        }

        // ---------- MODIFIED: Update total units based on visible rows ----------
        private void Enrollment_UpdateTotalUnits()
        {
            int total = 0;
            foreach (DataGridViewRow row in dgvEnrollment.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells["colUnits"].Value != null)
                {
                    int units;
                    if (int.TryParse(row.Cells["colUnits"].Value.ToString(), out units))
                        total += units;
                }
            }
            lblEnrollTotalUnitsValue.Text = total.ToString();
        }

        private void StudentPortal_EnrollmentResize(object sender, EventArgs e)
        {
            if (!IsHandleCreated) return;

            int contentWidth = pnlEnrollContent.Width;
            int cardWidth = (contentWidth - (SidePadding * 2) - (CardGap * 2)) / 3;

            pnlEnrollLeftCard.Width = cardWidth;
            pnlEnrollMiddleCard.Width = cardWidth;
            pnlEnrollRightCard.Width = cardWidth;

            pnlEnrollLeftCard.Left = SidePadding;
            pnlEnrollMiddleCard.Left = SidePadding + cardWidth + CardGap;
            pnlEnrollRightCard.Left = SidePadding + (cardWidth * 2) + (CardGap * 2);
        }

        private void dgvEnrollment_SelectionChanged(object sender, EventArgs e)
        {
            dgvEnrollment.ClearSelection();
        }

        private bool allSelected = false;

        private void btnEnrollSelectAll_Click(object sender, EventArgs e)
        {
            allSelected = !allSelected;
            foreach (DataGridViewRow row in dgvEnrollment.Rows)
            {
                row.Cells["colSelect"].Value = allSelected;
            }
            btnEnrollSelectAll.Text = allSelected ? "Deselect All" : "Select All";
        }

        // Press ENTER in search box = triggers search button
        private void txtEnrollSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnEnrollSearch.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        // ---------- MODIFIED: Search / filter logic ----------
        private void btnEnrollSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtEnrollSearch.Text.Trim().ToLower();
            string filterBy = cmbEnrollFilter.SelectedItem?.ToString() ?? "All";

            // Clear current grid rows
            dgvEnrollment.Rows.Clear();

            // If search is empty, show all original data
            if (string.IsNullOrEmpty(searchTerm))
            {
                foreach (var row in enrollmentData)
                {
                    dgvEnrollment.Rows.Add(false, row[0], row[1], row[2], row[3], row[4], "More");
                }
                Enrollment_UpdateTotalUnits();
                return;
            }

            // Filter based on selected column
            List<string[]> filtered = new List<string[]>();
            foreach (var row in enrollmentData)
            {
                bool match = false;
                if (filterBy == "Course Code")
                {
                    match = row[0].ToLower().Contains(searchTerm);
                }
                else if (filterBy == "Course Title")
                {
                    match = row[1].ToLower().Contains(searchTerm);
                }
                else // "All" – search in both Code and Title
                {
                    match = row[0].ToLower().Contains(searchTerm) || row[1].ToLower().Contains(searchTerm);
                }
                if (match)
                    filtered.Add(row);
            }

            // Add filtered rows to grid
            foreach (var row in filtered)
            {
                dgvEnrollment.Rows.Add(false, row[0], row[1], row[2], row[3], row[4], "More");
            }

            // Update total units based on filtered results
            Enrollment_UpdateTotalUnits();
        }

        private void dgvEnrollment_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && (
                e.ColumnIndex == dgvEnrollment.Columns["colUnits"].Index ||
                e.ColumnIndex == dgvEnrollment.Columns["colStatus"].Index ||
                e.ColumnIndex == dgvEnrollment.Columns["colAction"].Index))
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    string headerText = dgvEnrollment.Columns[e.ColumnIndex].HeaderText;
                    e.Graphics.DrawString(headerText, e.CellStyle.Font, brush, e.CellBounds, sf);
                }
                e.Handled = true;
            }
        }

        private void dgvEnrollment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvEnrollment.Columns["colSelect"].Index)
            {
                bool currentValue = Convert.ToBoolean(dgvEnrollment.Rows[e.RowIndex].Cells["colSelect"].Value);
                dgvEnrollment.Rows[e.RowIndex].Cells["colSelect"].Value = !currentValue;
                dgvEnrollment.ClearSelection();
                dgvEnrollment.CurrentCell = null;
            }

            if (e.ColumnIndex == dgvEnrollment.Columns["colAction"].Index && e.RowIndex >= 0)
            {
                Rectangle cellRect = dgvEnrollment.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                Point menuLocation = dgvEnrollment.PointToScreen(new Point(cellRect.Left, cellRect.Bottom));
                cmsEnrollAction.Show(menuLocation);
            }
        }

        private void Enrollment_ShowOverlay()
        {
            pnlViewDetails.BringToFront();
            pnlViewDetails.Visible = true;
            pnlViewDetails.Location = new Point(
                (this.ClientSize.Width - pnlViewDetails.Width) / 2,
                (this.ClientSize.Height - pnlViewDetails.Height) / 2
            );
        }

        private void Enrollment_HideOverlay()
        {
            pnlViewDetails.Visible = false;
        }

        private void Enrollment_viewDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvEnrollment.CurrentRow != null)
            {
                string code = dgvEnrollment.CurrentRow.Cells["colCode"].Value.ToString();
                string title = dgvEnrollment.CurrentRow.Cells["colTitle"].Value.ToString();
                string units = dgvEnrollment.CurrentRow.Cells["colUnits"].Value.ToString();
                string schedule = dgvEnrollment.CurrentRow.Cells["colSchedule"].Value.ToString();
                string status = dgvEnrollment.CurrentRow.Cells["colStatus"].Value.ToString();
                // You can show details in the overlay if you wish
            }
            Enrollment_ShowOverlay();
        }

        private void btnEnrollCloseDetails_Click(object sender, EventArgs e)
        {
            Enrollment_HideOverlay();
        }

        // ==================== ACCOUNTS ====================

        private void Accounts_Initialize()
        {
            pnlAccountsContent.Visible = true;
            Accounts_LoadData();
        }

        private void Accounts_LoadData()
        {
            dgvAccounts.Rows.Clear();
            dgvAccounts.Rows.Add("TF-001", "Tuition Fee", "FREE", "Mar 15, 2026", "Paid", "Mar 1, 2026");
            dgvAccounts.Rows.Add("MF-001", "Miscellaneous Fees", "FREE", "Mar 15, 2026", "Paid", "Mar 1, 2026");
            dgvAccounts.Rows.Add("LF-001", "Laboratory Fees (CS 102)", "₱500", "Mar 15, 2026", "Paid", "Mar 2, 2026");
            dgvAccounts.Rows.Add("ID-001", "ID Replacement", "₱150", "Mar 20, 2026", "Pending", "-");
            dgvAccounts.Rows.Add("LB-001", "Library Fee", "₱100", "Mar 15, 2026", "Paid", "Mar 1, 2026");
        }

        private void StudentPortal_AccountsResize(object sender, EventArgs e)
        {
            if (!IsHandleCreated) return;
            FitContentPanel(pnlAccountsContentHolder);
            int contentWidth = pnlAccountsContent.Width - (SidePadding * 2);
            int cardWidth = (contentWidth - (CardGap * 2)) / 3;
            dgvAccounts.Width = contentWidth;
            pnlAccountsFreeEd.Width = contentWidth;
            pnlAccountsSelectSem.Width = contentWidth;
            btnAccountsDownloadStatement.Left = contentWidth - btnAccountsDownloadStatement.Width + SidePadding;
            pnlTotalAssessment.Width = cardWidth;
            pnlTotalPaid.Width = cardWidth;
            pnlBalance.Width = cardWidth;
            pnlTotalAssessment.Left = SidePadding;
            pnlTotalPaid.Left = SidePadding + cardWidth + CardGap;
            pnlBalance.Left = SidePadding + (cardWidth * 2) + (CardGap * 2);
        }

        private void ShowDashboardPanel(Panel targetPanel)
        {
            // List of all main content panels
            Panel[] allContentPanels = { pnlDashboardContent, pnlEnrollContent, pnlCoursesContent, pnlAccountsContentHolder };

            foreach (Panel p in allContentPanels)
            {
                if (p == targetPanel)
                {
                    FitContentPanel(p);   // size and position
                    p.Visible = true;
                    p.BringToFront();
                }
                else
                {
                    p.Visible = false;
                }
            }
        }

        private void btnDashboardViewEnrollment_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            ShowDashboardPanel(pnlEnrollContent);
        }

        private void btnDashboardCourses_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            ShowDashboardPanel(pnlCoursesContent);
        }

        private void btnDashboardPaymentStatus_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            ShowDashboardPanel(pnlAccountsContentHolder);
        }

        private void btnDownloadCOR_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the image from resources (change name to match yours)
                System.Drawing.Image img = Properties.Resources.CertificateOfRegistration;

                if (img == null)
                {
                    MessageBox.Show("Certificate image not found in resources.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Let the user choose where to save the file
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "PNG Image|*.png";
                    sfd.FileName = "Certificate_of_Registration.png";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        MessageBox.Show("File saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}