using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class StudentPortal : Form
    {
        private Button clickedButton;
        private Color defaultColor = Color.Maroon;
        private Color selectedColor = Color.FromArgb(109, 0, 0);
        private const int SidePadding = 16;
        private const int CardGap = 10;
        private List<string[]> enrollmentData = new List<string[]>();

        public StudentPortal()
        {
            InitializeComponent();
            txtEnrollSearch.KeyDown += txtEnrollSearch_KeyDown;
            this.Resize += StudentPortal_EnrollmentResize;
            this.Resize += StudentPortal_AccountsResize;
            pnlContainerStudentPortal.Dock = DockStyle.Fill;
            pnlContainerStudentPortal.AutoScroll = true;
            dropSubjectToolStripMenuItem.Click += dropSubjectToolStripMenuItem_Click;
            btnSaveAndAssess.Click += btnSaveAndAssess_Click;

        }

        // ─────────────────────────────────────────────────────────────────
        // Navigation
        // ─────────────────────────────────────────────────────────────────
        private void changeButtonColor(Button button)
        {
            if (clickedButton != null) clickedButton.BackColor = defaultColor;
            clickedButton = button;
            pnlYellow.Visible = true;
            pnlYellow.Parent = clickedButton.Parent;
            pnlYellow.BringToFront();
            clickedButton.BackColor = selectedColor;
        }

        private void showContent(Button button)
        {
            var contents = new Dictionary<Button, Panel>
            {
                { btnDashboard, pnlDashboardContent },
                { btnEnrollment, pnlEnrollContent },
                { btnCourses, pnlCoursesContent },
                { btnAccounts, pnlAccountsContentHolder }
            };
            foreach (var kvp in contents)
            {
                if (kvp.Key == clickedButton)
                {
                    kvp.Value.Location = new Point(pnlSidebar.Width, pnlHeader.Height);
                    kvp.Value.Visible = true;
                }
                else kvp.Value.Visible = false;
            }
        }

        private void StudentPortal_Closing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Are you sure you want to exit?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    e.Cancel = true;
                else Application.Exit();
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e) { changeButtonColor(sender as Button); showContent(clickedButton); }
        private void btnEnrollment_Click(object sender, EventArgs e) { changeButtonColor(sender as Button); showContent(clickedButton); }
        private void btnCourses_Click(object sender, EventArgs e) { changeButtonColor(sender as Button); showContent(clickedButton); }
        private void btnAccounts_Click(object sender, EventArgs e) { changeButtonColor(sender as Button); showContent(clickedButton); }
        private void btnLMS_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
            pnllmsSubmenu.Visible = !pnllmsSubmenu.Visible;
            btnLMS.Text = pnllmsSubmenu.Visible ? " LMS                                       ⌄" : " LMS                                        ›";
        }
        private void btnLogout_Click(object sender, EventArgs e) => Close();

        private void StudentPortal_Load(object sender, EventArgs e)
        {
            Enrollment_Initialize();
            Accounts_Initialize();
            btnDashboard.PerformClick();
            SetupMaroonBorders();
        }

        private void FitContentPanel(Panel panel)
        {
            panel.Width = ClientSize.Width - pnlSidebar.Width;
            panel.Height = ClientSize.Height - pnlHeader.Height;
            panel.Location = new Point(pnlSidebar.Width, pnlHeader.Height);
        }

        // ─────────────────────────────────────────────────────────────────
        // ENROLLMENT
        // ─────────────────────────────────────────────────────────────────
        private void Enrollment_Initialize()
        {
            btnSaveAndAssess.Text = "Save and Assess";   // change button text
            dgvEnrollment.Visible = true;                // ensure original grid is visible
            dgvEnrollmentConfirmed.Visible = false;      // hide confirmed grid initially
            pnlEnrollmentConfirmedDGV.Visible = false;
            pnlContainerEnrollmentDGV.Visible = true;
            dgvEnrollment.Visible = true;
            Enrollment_LoadData();
            Enrollment_UpdateTotalUnits();
            ApplyStatusStyles();
        }

        private void Enrollment_LoadData()
        {
            enrollmentData.Clear();
            dgvEnrollment.Rows.Clear();

            // Define raw schedule entries (Day, StartTimes, EndTimes, CourseCode, CourseName)
            // Each entry represents one meeting time for a course on a specific day.
            var entries = new List<(string Day, string[] Starts, string[] Ends, string Code, string Name)>
    {
        // Monday
        ("Monday", new[] { "10:30 AM" }, new[] { "1:30 PM" }, "ELEC IT-FE2", "BSIT Free Elective 2"),
        ("Monday", new[] { "2:30 PM" }, new[] { "5:30 PM" }, "COMP 014", "Quantitative Methods with Modeling and Simulation"),

        // Wednesday
        ("Wednesday", new[] { "8:00 AM", "10:30 AM" }, new[] { "10:00 AM", "1:30 PM" }, "COMP 012", "Network Administration"),
        ("Wednesday", new[] { "5:30 PM" }, new[] { "7:30 PM" }, "COMP 009", "Object Oriented Programming"),

        // Thursday
        ("Thursday", new[] { "10:30 AM" }, new[] { "1:30 PM" }, "COMP 009", "Object Oriented Programming"),
        ("Thursday", new[] { "2:30 PM", "5:00 PM" }, new[] { "4:30 PM", "8:00 PM" }, "INTE 202", "Interactive Programming and Technologies 1"),

        // Friday
        ("Friday", new[] { "10:00 AM" }, new[] { "12:00 PM" }, "PATHFIT 4", "Physical Activity Towards Health and Fitness 4"),

        // Saturday
        ("Saturday", new[] { "7:30 AM" }, new[] { "10:30 AM" }, "COMP 013", "Human Computer Interaction"),
        ("Saturday", new[] { "2:30 PM", "5:00 PM" }, new[] { "4:30 PM", "8:00 PM" }, "COMP 010", "Information Management")
    };

            // Group by course code (and name) to combine multiple days
            var grouped = entries.GroupBy(e => new { e.Code, e.Name })
                                 .Select(g => new
                                 {
                                     Code = g.Key.Code,
                                     Name = g.Key.Name,
                                     // Collect schedule lines for each day
                                     ScheduleLines = g.Select(e =>
                                     {
                                         // Build time slots for that day
                                         string times = string.Join(", ", e.Starts.Select((s, i) => $"{s} - {e.Ends[i]}"));
                                         return $"{e.Day} {times}";
                                     }).ToList()
                                 });

            int GetUnits(string courseCode) => courseCode == "PATHFIT 4" ? 2 : 3;

            foreach (var course in grouped)
            {
                // Combine all schedule lines with newline characters
                string scheduleStr = string.Join(Environment.NewLine, course.ScheduleLines);
                int units = GetUnits(course.Code);
                string status = "Enrolled";

                enrollmentData.Add(new string[] { course.Code, course.Name, units.ToString(), scheduleStr, status });
            }

            // Enable text wrapping in the schedule column to show multiple lines
            dgvEnrollment.Columns["colSchedule"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvEnrollment.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Populate the DataGridView
            foreach (var row in enrollmentData)
            {
                dgvEnrollment.Rows.Add(false, row[0], row[1], row[2], row[3], row[4], "More");
            }

            Enrollment_UpdateTotalUnits();
        }

        private void ApplyStatusStyles()
        {
            foreach (DataGridViewRow row in dgvEnrollment.Rows)
            {
                if (row.IsNewRow) continue;
                string status = row.Cells["colStatus"].Value?.ToString();
                if (status == "Enrolled")
                {
                    row.Cells["colStatus"].Style.BackColor = Color.FromArgb(240, 240, 240);
                    row.Cells["colStatus"].Style.ForeColor = Color.Black;
                }
                else if (status == "Pending")
                {
                    row.Cells["colStatus"].Style.BackColor = Color.Gold;
                    row.Cells["colStatus"].Style.ForeColor = Color.Black;
                }
                else // Dropped or others
                {
                    row.Cells["colStatus"].Style.BackColor = Color.LightGray;
                    row.Cells["colStatus"].Style.ForeColor = Color.Black;
                }
            }
        }

        private void Enrollment_UpdateTotalUnits()
        {
            int total = 0;
            foreach (DataGridViewRow row in dgvEnrollment.Rows)
            {
                if (!row.IsNewRow && int.TryParse(row.Cells["colUnits"].Value?.ToString(), out int u)) total += u;
            }
            lblEnrollTotalUnitsValue.Text = total.ToString();
        }

        private void StudentPortal_EnrollmentResize(object sender, EventArgs e)
        {
            if (!IsHandleCreated) return;
            int contentWidth = pnlEnrollContent.Width;
            int cardWidth = (contentWidth - (SidePadding * 2) - (CardGap * 2)) / 3;
            foreach (var card in new[] { pnlEnrollLeftCard, pnlEnrollMiddleCard, pnlEnrollRightCard })
                card.Width = cardWidth;
            pnlEnrollLeftCard.Left = SidePadding;
            pnlEnrollMiddleCard.Left = SidePadding + cardWidth + CardGap;
            pnlEnrollRightCard.Left = SidePadding + (cardWidth * 2) + (CardGap * 2);
        }

        private void dgvEnrollment_SelectionChanged(object sender, EventArgs e) => dgvEnrollment.ClearSelection();

        private bool allSelected = false;
        private void btnEnrollSelectAll_Click(object sender, EventArgs e)
        {
            allSelected = !allSelected;
            foreach (DataGridViewRow row in dgvEnrollment.Rows)
                row.Cells["colSelect"].Value = allSelected;
            btnEnrollSelectAll.Text = allSelected ? "Deselect All" : "Select All";
        }

        private void txtEnrollSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { btnEnrollSearch.PerformClick(); e.SuppressKeyPress = true; }
        }

        private void btnEnrollSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtEnrollSearch.Text.Trim().ToLower();
            string filterBy = cmbEnrollFilter.SelectedItem?.ToString() ?? "All";
            dgvEnrollment.Rows.Clear();

            if (string.IsNullOrEmpty(searchTerm))
            {
                foreach (var row in enrollmentData) dgvEnrollment.Rows.Add(false, row[0], row[1], row[2], row[3], row[4], "More");
                Enrollment_UpdateTotalUnits();
                ApplyStatusStyles();
                return;
            }

            var filtered = enrollmentData.Where(row =>
                (filterBy == "Course Code" && row[0].ToLower().Contains(searchTerm)) ||
                (filterBy == "Course Title" && row[1].ToLower().Contains(searchTerm)) ||
                (filterBy == "All" && (row[0].ToLower().Contains(searchTerm) || row[1].ToLower().Contains(searchTerm)))
            ).ToList();

            foreach (var row in filtered) dgvEnrollment.Rows.Add(false, row[0], row[1], row[2], row[3], row[4], "More");
            Enrollment_UpdateTotalUnits();
            ApplyStatusStyles();
        }

        private void dgvEnrollment_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Only paint custom header cells
            if (e.RowIndex == -1)
            {
                // Get column indices safely
                int colUnitsIndex = (dgvEnrollment.Columns["colUnits"]?.Index) ?? -1;
                int colStatusIndex = (dgvEnrollment.Columns["colStatus"]?.Index) ?? -1;
                int colActionIndex = (dgvEnrollment.Columns["colAction"]?.Index) ?? -1;

                // Check if current column is one of the ones we want to customise
                if (e.ColumnIndex == colUnitsIndex || e.ColumnIndex == colStatusIndex || e.ColumnIndex == colActionIndex)
                {
                    // Paint background and border only
                    e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

                    // Get header text safely
                    string headerText = dgvEnrollment.Columns[e.ColumnIndex]?.HeaderText ?? "";

                    // Use a safe font and brush
                    using (SolidBrush brush = new SolidBrush(Color.White))
                    using (StringFormat sf = new StringFormat())
                    {
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Center;

                        // Fallback font if e.CellStyle.Font is null
                        Font font = e.CellStyle?.Font ?? new Font("Segoe UI", 9F, FontStyle.Bold);

                        e.Graphics.DrawString(headerText, font, brush, e.CellBounds, sf);
                    }

                    e.Handled = true;
                }
            }
        }

        private void dgvEnrollment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvEnrollment.Columns["colSelect"].Index)
            {
                bool current = Convert.ToBoolean(dgvEnrollment.Rows[e.RowIndex].Cells["colSelect"].Value);
                dgvEnrollment.Rows[e.RowIndex].Cells["colSelect"].Value = !current;
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
            pnlViewDetails.Location = new Point((ClientSize.Width - pnlViewDetails.Width) / 2, (ClientSize.Height - pnlViewDetails.Height) / 2);
        }
        private void Enrollment_HideOverlay() => pnlViewDetails.Visible = false;
        private void Enrollment_viewDetailsToolStripMenuItem_Click(object sender, EventArgs e) => Enrollment_ShowOverlay();
        private void btnEnrollCloseDetails_Click(object sender, EventArgs e) => Enrollment_HideOverlay();
        private void dropSubjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvEnrollment.CurrentRow == null) return;

            // Confirm drop
            DialogResult result = MessageBox.Show("Are you sure you want to drop this subject?", "Confirm Drop",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            // Get course code of the row to drop
            string courseCode = dgvEnrollment.CurrentRow.Cells["colCode"].Value.ToString();

            // Remove from enrollmentData list
            var toRemove = enrollmentData.FirstOrDefault(r => r[0] == courseCode);
            if (toRemove != null) enrollmentData.Remove(toRemove);

            // Remove from DataGridView
            dgvEnrollment.Rows.RemoveAt(dgvEnrollment.CurrentRow.Index);

            // Update total units
            Enrollment_UpdateTotalUnits();
            ApplyStatusStyles();

            MessageBox.Show("Subject dropped successfully.", "Dropped", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnSaveAndAssess_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Guard: ensure dgvEnrollment exists and has rows
                if (dgvEnrollment == null || dgvEnrollment.IsDisposed)
                {
                    MessageBox.Show("Enrollment grid is not available. Please refresh the page.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (dgvEnrollment.Rows.Count == 0)
                {
                    MessageBox.Show("No courses to enroll.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 2. Confirm save
                DialogResult confirm = MessageBox.Show("Are you sure you want to save and assess your enrollment?\nThis action cannot be undone.",
                    "Confirm Enrollment", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                // 3. Collect selected rows (with null checks for columns)
                List<DataGridViewRow> selectedRows = new List<DataGridViewRow>();
                foreach (DataGridViewRow row in dgvEnrollment.Rows)
                {
                    if (row.IsNewRow) continue;
                    object cellValue = row.Cells["colSelect"]?.Value;
                    if (cellValue != null && Convert.ToBoolean(cellValue))
                        selectedRows.Add(row);
                }

                if (selectedRows.Count == 0)
                {
                    MessageBox.Show("Please select at least one subject to enroll.", "No Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 4. Build DataTable for confirmed enrollment
                DataTable confirmedTable = new DataTable();
                confirmedTable.Columns.Add("Code", typeof(string));
                confirmedTable.Columns.Add("Title", typeof(string));
                confirmedTable.Columns.Add("Units", typeof(string));
                confirmedTable.Columns.Add("Schedule", typeof(string));
                confirmedTable.Columns.Add("Status", typeof(string));

                foreach (DataGridViewRow row in selectedRows)
                {
                    confirmedTable.Rows.Add(
                        row.Cells["colCode"]?.Value ?? "",
                        row.Cells["colTitle"]?.Value ?? "",
                        row.Cells["colUnits"]?.Value ?? "",
                        row.Cells["colSchedule"]?.Value ?? "",
                        "Enrolled"
                    );
                }

                // 5. Reposition and show the confirmed grid
                pnlEnrollmentConfirmedDGV.Location = pnlContainerEnrollmentDGV.Location;
                pnlEnrollmentConfirmedDGV.Size = pnlContainerEnrollmentDGV.Size;

                // 6. Configure confirmed grid columns (check if they exist)
                dgvEnrollmentConfirmed.AutoGenerateColumns = false;
                if (dgvEnrollmentConfirmed.Columns.Contains("dataGridViewTextBoxColumn1"))
                    dgvEnrollmentConfirmed.Columns["dataGridViewTextBoxColumn1"].DataPropertyName = "Code";
                if (dgvEnrollmentConfirmed.Columns.Contains("dataGridViewTextBoxColumn2"))
                    dgvEnrollmentConfirmed.Columns["dataGridViewTextBoxColumn2"].DataPropertyName = "Title";
                if (dgvEnrollmentConfirmed.Columns.Contains("dataGridViewTextBoxColumn3"))
                    dgvEnrollmentConfirmed.Columns["dataGridViewTextBoxColumn3"].DataPropertyName = "Units";
                if (dgvEnrollmentConfirmed.Columns.Contains("dataGridViewTextBoxColumn4"))
                    dgvEnrollmentConfirmed.Columns["dataGridViewTextBoxColumn4"].DataPropertyName = "Schedule";
                if (dgvEnrollmentConfirmed.Columns.Contains("dataGridViewTextBoxColumn5"))
                    dgvEnrollmentConfirmed.Columns["dataGridViewTextBoxColumn5"].DataPropertyName = "Status";
                if (dgvEnrollmentConfirmed.Columns.Contains("dataGridViewTextBoxColumn6"))
                    dgvEnrollmentConfirmed.Columns["dataGridViewTextBoxColumn6"].Visible = false;

                dgvEnrollmentConfirmed.DataSource = confirmedTable;
                dgvEnrollmentConfirmed.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                dgvEnrollmentConfirmed.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                if (dgvEnrollmentConfirmed.Columns.Contains("Schedule"))
                    dgvEnrollmentConfirmed.Columns["Schedule"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                // 7. Hide original UI, show confirmed grid
                dgvEnrollment.Visible = false;
                pnlContainerEnrollmentDGV.Visible = false;
                btnEnrollSelectAll.Visible = false;
                btnSaveAndAssess.Visible = false;
                pnlEnrollSearchbar.Visible = false;
                pnlEnrollmentConfirmedDGV.Visible = true;
                dgvEnrollmentConfirmed.Visible = true;

                // 8. Update total units
                int totalUnits = 0;
                foreach (DataGridViewRow row in dgvEnrollmentConfirmed.Rows)
                {
                    if (row.IsNewRow) continue;
                    if (int.TryParse(row.Cells["Units"].Value?.ToString(), out int u))
                        totalUnits += u;
                }
                lblEnrollTotalUnitsValue.Text = totalUnits.ToString();

                // 9. Update enrollment status card
                lblEnrollStatusTitle.Text = "Officially Enrolled";
                lblEnrollStatusDesc.Text = "You are now officially enrolled. Your subjects have been confirmed.";
                pnlEnrollStatusCard.BackColor = Color.FromArgb(220, 255, 220);
                pictureBox5.BackColor = Color.Green;

                MessageBox.Show("You are officially enrolled!", "Enrollment Successful",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred:\n{ex.Message}\n\nPlease contact support.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // ACCOUNTS – Using DataTable with existing columns
        // ─────────────────────────────────────────────────────────────────

        private DataTable accountsTable;

        private void Accounts_Initialize()
        {
            pnlAccountsContent.Visible = true;
            CreateAccountsTable();
            LoadAccountsData();
            SetupAccountsGridStyle();
            UpdateAccountsSummary();
            PopulateSemesterFilter();
            cmbSelectSem.SelectedIndexChanged += cmbSelectSem_SelectedIndexChanged;
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

        private void LoadAccountsData()
        {
            accountsTable.Rows.Clear();

            var data = new List<(string year, string sem, string orDate, string orNo, string assessment)>
    {
        ("2425", "First Semester", "09/03/2024", "CASH - Free Education (20240903-000132)", "7,294.00"),
        ("2425", "Second Semester", "02/18/2025", "CASH - Free Education (20250218-000283)", "6,255.00"),
        ("2526", "First Semester", "08/22/2025", "CASH - Free Education (20250822-001891)", "8,616.00"),
        ("2526", "Second Semester", "02/11/2026", "CASH - Free Education (20260211-006026)", "16,566.00")
    };

            foreach (var item in data)
            {
                string refId = $"{item.year}-{item.sem.Replace(" ", "")}";
                string description = $"Tuition & Fees - {item.sem} AY {item.year}\n({item.orNo})";
                string amount = $"₱{item.assessment}";
                string dueDate = "";
                string status = "Paid";
                string paidDate = item.orDate;

                accountsTable.Rows.Add(refId, description, amount, dueDate, status, paidDate);
            }

            dgvAccounts.ClearSelection();
            dgvAccounts.Refresh();
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
                string amountStr = row["colAccountsAmount"].ToString();
                string status = row["colAccountsStatus"].ToString();

                amountStr = amountStr.Replace("₱", "").Replace(",", "");
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
        }

        private void PopulateSemesterFilter()
        {
            cmbSelectSem.Items.Clear();
            cmbSelectSem.Items.Add("All");

            foreach (DataRow row in accountsTable.Rows)
            {
                string description = row["colAccountsDescription"].ToString();
                if (description.Contains("First Semester") && !cmbSelectSem.Items.Contains("First Semester"))
                    cmbSelectSem.Items.Add("First Semester");
                else if (description.Contains("Second Semester") && !cmbSelectSem.Items.Contains("Second Semester"))
                    cmbSelectSem.Items.Add("Second Semester");
            }
            cmbSelectSem.SelectedIndex = 0;
        }

        private void cmbSelectSem_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cmbSelectSem.SelectedItem?.ToString();
            if (selected == "All")
            {
                accountsTable.DefaultView.RowFilter = "";
            }
            else
            {
                accountsTable.DefaultView.RowFilter = $"colAccountsDescription LIKE '%{selected}%'";
            }
            UpdateAccountsSummary();
        }

        private void btnAccountsDownloadStatement_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Statement of Account would be generated here (demo).", "Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void StudentPortal_AccountsResize(object sender, EventArgs e)
        {
            if (!IsHandleCreated) return;

            FitContentPanel(pnlAccountsContentHolder);

            int contentWidth = pnlAccountsContent.Width - (SidePadding * 2);
            int cardWidth = (contentWidth - (CardGap * 2)) / 3;

            // --- Top summary cards ---
            pnlTotalAssessment.Width = cardWidth;
            pnlTotalPaid.Width = cardWidth;
            pnlBalance.Width = cardWidth;
            pnlTotalAssessment.Left = SidePadding;
            pnlTotalPaid.Left = SidePadding + cardWidth + CardGap;
            pnlBalance.Left = SidePadding + (cardWidth * 2) + (CardGap * 2);

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

            // --- Enrollment Status section ---
            lblEnrollStatus.Top = pnlOnlinePayment.Bottom + 20;
            lblEnrollStatus.Left = SidePadding;

            pnlEnrollStatusCard.Width = contentWidth;
            pnlEnrollStatusCard.Top = lblEnrollStatus.Bottom + 10;

            // --- Spacer panel (ensures scrolling works) ---
            pnlSpaceProviderAccounts.Top = pnlEnrollStatusCard.Bottom + 20;
            pnlSpaceProviderAccounts.Height = 50;
        }


        // ─────────────────────────────────────────────────────────────────
        // Dashboard quick actions
        // ─────────────────────────────────────────────────────────────────
        private void btnDashboardViewEnrollment_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            btnEnrollment.PerformClick();
        }
        private void btnDashboardCourses_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            btnCourses.PerformClick();
        }
        private void btnDashboardPaymentStatus_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            btnAccounts.PerformClick();
        }

        private void btnDownloadCOR_Click(object sender, EventArgs e)
        {
            try
            {
                var img = Properties.Resources.CertificateOfRegistration;
                if (img == null) throw new Exception("Certificate image not found.");
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
            catch (Exception ex) { MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // ─────────────────────────────────────────────────────────────────
        // Maroon border for info cards (draws 1px Maroon border)
        // ─────────────────────────────────────────────────────────────────
        private void SetupMaroonBorders()
        {
            foreach (Panel p in new[] { pnlEnrollLeftCard, pnlEnrollMiddleCard, pnlEnrollRightCard })
            {
                p.BackColor = Color.White;
                p.BorderStyle = BorderStyle.None;
                p.Paint += (s, e) =>
                {
                    ControlPaint.DrawBorder(e.Graphics, p.ClientRectangle,
                        Color.Maroon, 1, ButtonBorderStyle.Solid,
                        Color.Maroon, 1, ButtonBorderStyle.Solid,
                        Color.Maroon, 1, ButtonBorderStyle.Solid,
                        Color.Maroon, 1, ButtonBorderStyle.Solid);
                };
            }
        }

        private void btnPayOnline_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You will be presented options on what online payment medium you'd prefer.", "Pay Online", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnPaymentSlip_Click(object sender, EventArgs e)
        {
            MessageBox.Show("A payment slip would be generated here (demo).", "Pay at the Cashier", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}