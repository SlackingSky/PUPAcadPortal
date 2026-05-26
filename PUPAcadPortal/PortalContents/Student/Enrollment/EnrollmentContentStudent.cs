using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PUPAcadPortal.Data;

namespace PUPAcadPortal.PortalContents.Student.Enrollment
{
    public partial class EnrollmentContentStudent : UserControl
    {
        public static bool isEnrolled = false; // Static variable to track enrollment status across the application
        public static int totalUnits = 0;
        public static StudentDataService dataService;
        private List<string[]> enrollmentData = new List<string[]>();
        public EnrollmentContentStudent()
        {
            InitializeComponent();

            dataService = StudentDataService.Instance;
            btnSaveAndAssess.Click += btnSaveAndAssess_Click;
        }

        // ─────────────────────────────────────────────────────────────────
        // ENROLLMENT
        // ─────────────────────────────────────────────────────────────────
        private void Enrollment_Initialize()
        {
            if (isEnrolled)
            {
                btnSaveAndAssess.Visible = false; // Hide the button if already enrolled
                dgvEnrollment.Visible = false;
                pnlContainerEnrollmentDGV.Visible = false;
                btnSaveAndAssess.Enabled = false;
                dgvEnrollmentConfirmed.Visible = true;
                pnlEnrollmentConfirmedDGV.Visible = true;
                lblTotalUnitsValue.Text = totalUnits.ToString();
                btnDownloadCOR.Enabled = true;
                btnDownloadCOR.BackColor = Color.Maroon;
                return;
            }
            btnSaveAndAssess.Text = "Save and Assess";   // change button text
            dgvEnrollment.Visible = true;                // ensure original grid is visible
            dgvEnrollmentConfirmed.Visible = false;      // hide confirmed grid initially
            pnlEnrollmentConfirmedDGV.Visible = false;
            pnlContainerEnrollmentDGV.Visible = true;
            dgvEnrollment.Visible = true;

            // Initialize COR button as disabled (grey)
            btnDownloadCOR.Enabled = false;
            btnDownloadCOR.BackColor = Color.DarkGray;

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
                string status = "Pending";

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
            // Reset the header checkbox state
            _isAllSelected = false;
            dgvEnrollment.InvalidateColumn(dgvEnrollment.Columns["colSelect"].Index);
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
                if (!row.IsNewRow)
                {
                    // Check if the row is selected
                    bool isSelected = row.Cells["colSelect"].Value != null && (bool)row.Cells["colSelect"].Value == true;
                    if (isSelected && int.TryParse(row.Cells["colUnits"].Value?.ToString(), out int u))
                    {
                        total += u;
                    }
                }
            }

            // Update the total units label
            lblTotalUnitsValue.Text = total.ToString();
            totalUnits = total;
        }

        private void dgvEnrollment_SelectionChanged(object sender, EventArgs e) => dgvEnrollment.ClearSelection();

        private bool allSelected = false;
        private bool _isAllSelected = false;

        private void dgvEnrollment_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Only paint custom header cells
            if (e.RowIndex == -1)
            {
                if (e.ColumnIndex == dgvEnrollment.Columns["colSelect"].Index)
                {
                    e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
                    int x = e.CellBounds.Left + (e.CellBounds.Width - 14) / 2;
                    int y = e.CellBounds.Top + (e.CellBounds.Height - 14) / 2;
                    Rectangle checkRect = new Rectangle(x, y, 14, 14);
                    ButtonState state = _isAllSelected ? ButtonState.Checked : ButtonState.Normal;
                    ControlPaint.DrawCheckBox(e.Graphics, checkRect, state);
                    e.Handled = true;
                    return;
                }

                // Existing custom painting for Units, Status, Action columns (unchanged)
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
            // ----- HEADER CHECKBOX CLICK (Select/Deselect All) -----
            if (e.RowIndex == -1 && e.ColumnIndex == dgvEnrollment.Columns["colSelect"].Index)
            {
                // Toggle master state
                _isAllSelected = !_isAllSelected;

                // Update all row checkboxes
                foreach (DataGridViewRow row in dgvEnrollment.Rows)
                {
                    if (!row.IsNewRow)
                        row.Cells["colSelect"].Value = _isAllSelected;
                }

                // Force the grid to commit changes and refresh
                dgvEnrollment.EndEdit();
                dgvEnrollment.Refresh();

                // Redraw the header checkbox
                dgvEnrollment.InvalidateColumn(dgvEnrollment.Columns["colSelect"].Index);

                // Update total units
                Enrollment_UpdateTotalUnits();
                return;
            }

            // ----- ROW CHECKBOX CLICK (update header state) -----
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvEnrollment.Columns["colSelect"].Index)
            {
                // Use BeginInvoke to let the row checkbox toggle finish first
                this.BeginInvoke(new Action(() =>
                {
                    // Check if all rows are now selected
                    bool allSelected = true;
                    foreach (DataGridViewRow row in dgvEnrollment.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            object val = row.Cells["colSelect"].Value;
                            bool isChecked = (val != null && (bool)val == true);
                            if (!isChecked)
                            {
                                allSelected = false;
                                break;
                            }
                        }
                    }
                    _isAllSelected = allSelected;
                    dgvEnrollment.InvalidateColumn(dgvEnrollment.Columns["colSelect"].Index);
                    Enrollment_UpdateTotalUnits();
                }));
                return;
            }

            // ----- EXISTING ACTION COLUMN CLICK (context menu) -----
            if (e.ColumnIndex == dgvEnrollment.Columns["colAction"].Index && e.RowIndex >= 0)
            {
                Rectangle cellRect = dgvEnrollment.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                Point menuLocation = dgvEnrollment.PointToScreen(new Point(cellRect.Left, cellRect.Bottom));
                cmsEnrollAction.Show(menuLocation);
            }
        }

        private void Enrollment_ShowOverlay()
        {
            pnlViewDetails.Parent = pnlEnrollContent;
            pnlViewDetails.BringToFront();
            pnlViewDetails.Visible = true;
            pnlViewDetails.Location = new Point((pnlViewDetails.Parent.Width - pnlViewDetails.Width) / 2, (pnlViewDetails.Parent.Height - pnlViewDetails.Height) / 2);
        }
        private void Enrollment_HideOverlay() => pnlViewDetails.Visible = false;
        private void Enrollment_viewDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvEnrollment.CurrentRow != null)
            {
                // Retrieve values from the current row
                string code = dgvEnrollment.CurrentRow.Cells["colCode"].Value?.ToString() ?? "";
                string title = dgvEnrollment.CurrentRow.Cells["colTitle"].Value?.ToString() ?? "";
                string units = dgvEnrollment.CurrentRow.Cells["colUnits"].Value?.ToString() ?? "";
                string schedule = dgvEnrollment.CurrentRow.Cells["colSchedule"].Value?.ToString() ?? "";
                string status = dgvEnrollment.CurrentRow.Cells["colStatus"].Value?.ToString() ?? "";

                // Assign to labels (make sure these label names exist in pnlViewDetails)
                lblDetailCode.Text = $"Code: {code}";
                lblDetailTitle.Text = $"Title: {title}";
                lblDetailUnits.Text = $"Units: {units}";
                txtDetailSchedule.Text = $"Schedule: {schedule}";
                lblDetailStatus.Text = $"Status: {status}";
            }
            Enrollment_ShowOverlay();
        }
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

                // 3. Collect selected rows
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

                int totalUnits = 0;
                foreach (DataGridViewRow row in selectedRows)
                {
                    string unitsStr = row.Cells["colUnits"]?.Value?.ToString() ?? "0";
                    confirmedTable.Rows.Add(
                        row.Cells["colCode"]?.Value ?? "",
                        row.Cells["colTitle"]?.Value ?? "",
                        unitsStr,
                        row.Cells["colSchedule"]?.Value ?? "",
                        "Enrolled"
                    );
                    if (int.TryParse(unitsStr, out int u))
                        totalUnits += u;
                }

                // 5. Reposition the confirmed grid panel
                pnlEnrollmentConfirmedDGV.Location = pnlContainerEnrollmentDGV.Location;
                pnlEnrollmentConfirmedDGV.Size = pnlContainerEnrollmentDGV.Size;

                // 6. Configure confirmed grid columns using actual designer column names
                dgvEnrollmentConfirmed.AutoGenerateColumns = false;

                if (dgvEnrollmentConfirmed.Columns.Contains("colCode2"))
                    dgvEnrollmentConfirmed.Columns["colCode2"].DataPropertyName = "Code";
                if (dgvEnrollmentConfirmed.Columns.Contains("colCourseTitle2"))
                    dgvEnrollmentConfirmed.Columns["colCourseTitle2"].DataPropertyName = "Title";
                if (dgvEnrollmentConfirmed.Columns.Contains("colUnits2"))
                    dgvEnrollmentConfirmed.Columns["colUnits2"].DataPropertyName = "Units";
                if (dgvEnrollmentConfirmed.Columns.Contains("colSchedule2"))
                    dgvEnrollmentConfirmed.Columns["colSchedule2"].DataPropertyName = "Schedule";
                if (dgvEnrollmentConfirmed.Columns.Contains("colStatus2"))
                    dgvEnrollmentConfirmed.Columns["colStatus2"].DataPropertyName = "Status";
                if (dgvEnrollmentConfirmed.Columns.Contains("colAction2"))
                    dgvEnrollmentConfirmed.Columns["colAction2"].Visible = false;

                // 7. Bind data
                dgvEnrollmentConfirmed.DataSource = confirmedTable;

                // --- FORCE PROPER DISPLAY WITH 12PT FONT ---
                dgvEnrollmentConfirmed.SuspendLayout();

                dgvEnrollmentConfirmed.DefaultCellStyle.Font = new Font("Segoe UI", 12F);
                dgvEnrollmentConfirmed.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

                dgvEnrollmentConfirmed.RowTemplate.Height = 35;
                dgvEnrollmentConfirmed.ColumnHeadersHeight = 40;

                dgvEnrollmentConfirmed.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                if (dgvEnrollmentConfirmed.Columns.Contains("colCourseTitle2"))
                {
                    dgvEnrollmentConfirmed.Columns["colCourseTitle2"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvEnrollmentConfirmed.Columns["colCourseTitle2"].MinimumWidth = 250;
                }

                if (dgvEnrollmentConfirmed.Columns.Contains("colSchedule2"))
                    dgvEnrollmentConfirmed.Columns["colSchedule2"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                dgvEnrollmentConfirmed.ResumeLayout();
                dgvEnrollmentConfirmed.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                // 8. Remove selection highlight
                dgvEnrollmentConfirmed.EnableHeadersVisualStyles = false;
                dgvEnrollmentConfirmed.DefaultCellStyle.SelectionBackColor = dgvEnrollmentConfirmed.DefaultCellStyle.BackColor;
                dgvEnrollmentConfirmed.DefaultCellStyle.SelectionForeColor = dgvEnrollmentConfirmed.DefaultCellStyle.ForeColor;
                dgvEnrollmentConfirmed.ColumnHeadersDefaultCellStyle.SelectionBackColor = dgvEnrollmentConfirmed.ColumnHeadersDefaultCellStyle.BackColor;
                dgvEnrollmentConfirmed.ColumnHeadersDefaultCellStyle.SelectionForeColor = dgvEnrollmentConfirmed.ColumnHeadersDefaultCellStyle.ForeColor;
                dgvEnrollmentConfirmed.RowHeadersVisible = false;

                // 9. Center align Units and Status columns
                if (dgvEnrollmentConfirmed.Columns.Contains("colUnits2"))
                {
                    dgvEnrollmentConfirmed.Columns["colUnits2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvEnrollmentConfirmed.Columns["colUnits2"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dgvEnrollmentConfirmed.Columns.Contains("colStatus2"))
                {
                    dgvEnrollmentConfirmed.Columns["colStatus2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvEnrollmentConfirmed.Columns["colStatus2"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                // 10. Hide original UI, show confirmed grid
                dgvEnrollment.Visible = false;
                pnlContainerEnrollmentDGV.Visible = false;
                btnSaveAndAssess.Visible = false;
                pnlEnrollmentConfirmedDGV.Visible = true;
                dgvEnrollmentConfirmed.Visible = true;

                // 13. Save enrollment status
                SetEnrollmentStatus(true);

                //// 14. Refresh accounts data to show real payment information
                //RefreshAccountsAfterEnrollment();
                isEnrolled = true;

                MessageBox.Show("You are officially enrolled!", "Enrollment Successful",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred:\n{ex.Message}\n\nPlease contact support.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Enable COR download button after successful enrollment
            btnDownloadCOR.Enabled = true;
            btnDownloadCOR.BackColor = Color.Maroon;

            // After successful enrollment confirmation
            dataService.SetEnrollmentStatus(true);
            //RefreshAccountsAfterEnrollment();
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

        // Method to save enrollment status after successful enrollment
        private void SetEnrollmentStatus(bool enrolled)
        {
            dataService.SetEnrollmentStatus(enrolled);
        }

        private void EnrollmentContentStudent_Load(object sender, EventArgs e)
        {
            Enrollment_Initialize();
            SetupMaroonBorders();
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
    }
}
