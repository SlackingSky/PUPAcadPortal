using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using PUPAcadPortal.Services;
using PUPAcadPortal.Utils;

namespace PUPAcadPortal.PortalContents.Student.Enrollment
{
    public partial class EnrollmentContentStudent : UserControl
    {
        public static bool isEnrolled = false;
        public static int totalUnits = 0;
        public static StudentDataService dataService;

        private List<EnrollmentData> _liveSubjects = new List<EnrollmentData>();
        private EnrollmentService _enrollmentService = new EnrollmentService();
        private bool _isAllSelected = false;

        // Dynamic State Variables
        private int _currentStudentId;
        private string _currentProgram;
        private int _currentYearLevel;

        public EnrollmentContentStudent()
        {
            InitializeComponent();
            dataService = StudentDataService.Instance;
            EnableDoubleBuffering(dgvEnrollment);

            btnSaveAndAssess.Click += btnSaveAndAssess_Click;
            dgvEnrollment.CellPainting += dgvEnrollment_CellPainting;
            dgvEnrollment.CellClick += dgvEnrollment_CellContentClick;
            dgvEnrollment.SelectionChanged += dgvEnrollment_SelectionChanged;
            dgvEnrollment.CurrentCellDirtyStateChanged += dgvEnrollment_CurrentCellDirtyStateChanged;
            dgvEnrollment.CellValueChanged += dgvEnrollment_CellValueChanged;
        }

        private void dgvEnrollment_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvEnrollment.Columns["colSelect"].Index)
            {
                _isAllSelected = _liveSubjects
                    .Where(s => s.IsEligible && s.Status == "Pending")
                    .All(s => s.IsSelected);

                dgvEnrollment.InvalidateCell(dgvEnrollment.Columns["colSelect"].Index, -1);

                Enrollment_UpdateTotalUnits();
            }
        }

        private void EnableDoubleBuffering(DataGridView dgv)
        {
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.SetProperty,
                null, dgv, new object[] { true });
        }

        private async void EnrollmentContentStudent_Load(object sender, EventArgs e)
        {
            btnDownloadCOR.Enabled = false;
            SetupMaroonBorders();
            await Enrollment_InitializeAsync();
        }

        private async Task Enrollment_InitializeAsync()
        {
            try
            {
                this.DisableControls();
                Application.UseWaitCursor = true;
                if (!UserSession.StudentID.HasValue)
                {
                    MessageBox.Show("Security Error: Only active students can access the enrollment module.", "Unauthorized", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _currentStudentId = UserSession.StudentID.Value;

                var studentProfile = await _enrollmentService.GetStudentProfileAsync(_currentStudentId);

                _currentProgram = studentProfile.Program;
                _currentYearLevel = studentProfile.YearLevel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Profile Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.SafeUIUpdate(async() =>
            {
                pnlRA10391.Visible = await _enrollmentService.IsStudentIskolar(_currentStudentId);
            });


            this.SafeUIUpdate(async () =>
            {
                dgvEnrollment.Visible = true;
                pnlContainerEnrollmentDGV.Visible = true;

                dgvEnrollment.AutoGenerateColumns = false;
                if (dgvEnrollment.Columns.Contains("colSelect")) dgvEnrollment.Columns["colSelect"].DataPropertyName = "IsSelected";
                if (dgvEnrollment.Columns.Contains("colCode")) dgvEnrollment.Columns["colCode"].DataPropertyName = "Code";
                if (dgvEnrollment.Columns.Contains("colTitle")) dgvEnrollment.Columns["colTitle"].DataPropertyName = "CourseTitle";
                if (dgvEnrollment.Columns.Contains("colUnits")) dgvEnrollment.Columns["colUnits"].DataPropertyName = "Units";
                if (dgvEnrollment.Columns.Contains("colSchedule")) dgvEnrollment.Columns["colSchedule"].DataPropertyName = "Schedule";
                if (dgvEnrollment.Columns.Contains("colStatus")) dgvEnrollment.Columns["colStatus"].DataPropertyName = "Status";
            });

            _liveSubjects = await _enrollmentService.GetAvailableSubjectsAsync(
                _currentStudentId,
                GlobalSession.ActiveAcademicPeriod,
                _currentProgram,
                _currentYearLevel,
                GlobalSession.ActiveSemesterIndex);

            int maxUnits = _liveSubjects.Sum(s => s.Units);

            this.SafeUIUpdate(() =>
            {
                lblMaximumUnits.Text = $"{maxUnits} Units";
            });
            

            bool isIrregular = _liveSubjects.Any(s => !s.IsEligible);

            if (isIrregular)
            {
                this.SafeUIUpdate(() =>
                {
                    lblScholasticStatus.Text = "Irregular";
                    lblScholasticStatus.ForeColor = Color.DarkRed;
                });
            }
            else
            {
                this.SafeUIUpdate(() =>
                {
                    lblScholasticStatus.Text = "Regular";
                    lblScholasticStatus.ForeColor = Color.Black;
                });
            }


            this.SafeUIUpdate(() =>
            {
                dgvEnrollment.Columns["colSchedule"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgvEnrollment.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dgvEnrollment.DataSource = new BindingList<EnrollmentData>(_liveSubjects);
            });

            Enrollment_UpdateTotalUnits();
            _isAllSelected = false;

            this.SafeUIUpdate(() =>
            {
                dgvEnrollment.InvalidateColumn(dgvEnrollment.Columns["colSelect"].Index);
            });
            Application.UseWaitCursor = false;
            if (this != null)
            {
                this.EnableControls();

                CheckGlobalEnrollmentStatus();
            }
        }

        private void CheckGlobalEnrollmentStatus()
        {
            bool hasEnrolledSubjects = _liveSubjects.Any(s => s.Status == "Officially Enrolled");
            bool hasPendingEligibleSubjects = _liveSubjects.Any(s => s.IsEligible && s.Status == "Pending");
            bool isFullyEnrolled = hasEnrolledSubjects && !hasPendingEligibleSubjects;

            isEnrolled = isFullyEnrolled;

            if (isFullyEnrolled)
            {
                btnDownloadCOR.Enabled = true;
                btnDownloadCOR.BackColor = Color.Maroon;
            }
            else
            {
                btnDownloadCOR.Enabled = false;
                btnDownloadCOR.BackColor = Color.DarkGray;
            }

            btnSaveAndAssess.Visible = hasPendingEligibleSubjects;
        }

        private void Enrollment_UpdateTotalUnits()
        {
            int total = 0;
            foreach (var subject in _liveSubjects)
            {
                if (subject.IsSelected && (subject.Status == "Pending" || subject.Status == "Officially Enrolled"))
                {
                    total += subject.Units;
                }
            }

            this.SafeUIUpdate(() =>
            {
                lblTotalUnitsValue.Text = total.ToString();
            });
            totalUnits = total;
        }

        private async void btnSaveAndAssess_Click(object sender, EventArgs e)
        {
            try
            {
                var subjectsToEnroll = _liveSubjects.Where(s => s.IsSelected && s.Status == "Pending").ToList();

                if (!subjectsToEnroll.Any())
                {
                    MessageBox.Show("Please select at least one pending subject.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int selectedUnits = subjectsToEnroll.Sum(s => s.Units);
                int alreadyEnrolledUnits = _liveSubjects.Where(s => s.Status == "Officially Enrolled").Sum(s => s.Units);
                int totalAttemptedUnits = selectedUnits + alreadyEnrolledUnits;
                int requiredRegularUnits = _liveSubjects.Sum(s => s.Units);

                if (totalAttemptedUnits != requiredRegularUnits)
                {
                    bool hasFailedPrereqs = _liveSubjects.Any(s => !s.IsEligible);

                    if (hasFailedPrereqs)
                    {
                        DialogResult irregularConfirm = MessageBox.Show(
                            $"You are attempting to enroll in {totalAttemptedUnits} units instead of the regular {requiredRegularUnits} units.\n\nBecause you have locked subjects, you will be processed as an Irregular student. Do you wish to proceed?",
                            "Irregular Enrollment Status", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                        if (irregularConfirm != DialogResult.Yes) return;
                    }
                    else
                    {
                        MessageBox.Show(
                            $"As a Regular student, you must take the full {requiredRegularUnits} units for this semester.\n\nYou only selected {totalAttemptedUnits} units. Please check all available subjects to proceed.",
                            "Underload Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }

                DialogResult confirm = MessageBox.Show("Are you sure you want to save and assess your enrollment?", "Confirm Enrollment", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                btnSaveAndAssess.Enabled = false;
                btnSaveAndAssess.Text = "Processing...";
                Application.UseWaitCursor = true;

                var result = await _enrollmentService.ProcessSaveAndAssessAsync(_currentStudentId, GlobalSession.ActiveAcademicPeriod, subjectsToEnroll);

                this.SafeUIUpdate(async () =>
                {
                    if (!result.Success)
                    {
                        MessageBox.Show(result.Message, "Transaction Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    await Enrollment_InitializeAsync();
                    CheckGlobalEnrollmentStatus();
                    dataService.SetEnrollmentStatus(true);

                    MessageBox.Show("You successfully saved and assessed, if you see any pending payments, please contact the cashier.", "Enrollment Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
            }
            catch (Exception ex)
            {
                this.SafeUIUpdate(() =>
                {
                    MessageBox.Show($"An error occurred:\n{ex.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                });

            }
            finally
            {
                Application.UseWaitCursor = false;
                this.SafeUIUpdate(() =>
                {
                    btnSaveAndAssess.Enabled = true;
                    btnSaveAndAssess.Text = "Save and Assess";
                });
            }
        }

        private void dgvEnrollment_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvEnrollment.IsCurrentCellDirty)
            {
                dgvEnrollment.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvEnrollment_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (dgvEnrollment == null) return;

            var colSelect = dgvEnrollment.Columns["colSelect"];
            if (e.RowIndex == -1 && colSelect != null && e.ColumnIndex == colSelect.Index)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
                int x = e.CellBounds.Left + (e.CellBounds.Width - 14) / 2;
                int y = e.CellBounds.Top + (e.CellBounds.Height - 14) / 2;
                e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
                ButtonState state = _isAllSelected ? ButtonState.Checked : ButtonState.Normal;
                Rectangle checkRect = new Rectangle(x, y, 14, 14);
                ControlPaint.DrawCheckBox(e.Graphics, checkRect, state);
                e.Handled = true;
                return;
            }

            if (e.RowIndex >= 0 && colSelect != null && e.ColumnIndex == colSelect.Index)
            {
                var rowData = dgvEnrollment.Rows[e.RowIndex].DataBoundItem as EnrollmentData;
                if (rowData != null)
                {
                    bool isNotEnrollable = !rowData.IsEligible ||
                                           rowData.Status == "Officially Enrolled" ||
                                           rowData.Status == "Pending Payment";

                    if (isNotEnrollable)
                    {
                        e.Paint(e.CellBounds, DataGridViewPaintParts.Background |
                                              DataGridViewPaintParts.Border |
                                              DataGridViewPaintParts.SelectionBackground);

                        e.Handled = true;
                        return;
                    }
                }
            }

            var colStatus = dgvEnrollment.Columns["colStatus"];
            if (e.RowIndex >= 0 && colStatus != null && e.ColumnIndex == colStatus.Index)
            {
                if (e.RowIndex < dgvEnrollment.Rows.Count)
                {
                    var row = dgvEnrollment.Rows[e.RowIndex];
                    var rowData = row.DataBoundItem as EnrollmentData;

                    if (rowData == null) return;

                    if (rowData.Status == "Officially Enrolled")
                    {
                        e.CellStyle.BackColor = Color.SeaGreen;
                    }
                    else if (rowData.Status == "Pending Payment")
                    {
                        e.CellStyle.BackColor = Color.DarkOrange;
                    }
                    else if (!rowData.IsEligible)
                    {
                        e.CellStyle.BackColor = Color.LightCoral;

                        if (row.DefaultCellStyle.Font == null || row.DefaultCellStyle.Font.Style != FontStyle.Strikeout)
                        {
                            row.DefaultCellStyle.Font = new Font(dgvEnrollment.Font, FontStyle.Strikeout);
                        }
                    }
                    else if (rowData.Status == "Pending")
                    {
                        e.CellStyle.BackColor = Color.Gold;
                    }

                    e.CellStyle.ForeColor = (rowData.Status == "Officially Enrolled" || rowData.Status == "Pending Payment" || !rowData.IsEligible)
                                             ? Color.White : Color.Black;
                }
            }
        }

        private void dgvEnrollment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvEnrollment.CommitEdit(DataGridViewDataErrorContexts.Commit);

            var colSelect = dgvEnrollment.Columns["colSelect"];
            var colAction = dgvEnrollment.Columns["colAction"];

            if (e.RowIndex == -1 && colSelect != null && e.ColumnIndex == colSelect.Index)
            {
                _isAllSelected = !_isAllSelected;
                foreach (var subject in _liveSubjects)
                {
                    if (subject.IsEligible && subject.Status == "Pending")
                        subject.IsSelected = _isAllSelected;
                }
                dgvEnrollment.InvalidateColumn(e.ColumnIndex);
                Enrollment_UpdateTotalUnits();
                return;
            }

            if (e.RowIndex >= 0 && colSelect != null && e.ColumnIndex == colSelect.Index)
            {
                var rowData = dgvEnrollment.Rows[e.RowIndex].DataBoundItem as EnrollmentData;
                if (rowData != null)
                {
                    if (rowData.Status == "Officially Enrolled") return;

                    if (!rowData.IsEligible)
                    {
                        rowData.IsSelected = false;
                        dgvEnrollment.InvalidateRow(e.RowIndex);
                        MessageBox.Show(rowData.PrerequisiteMessage, "Prerequisite Locked", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    _isAllSelected = _liveSubjects
                        .Where(s => s.IsEligible && s.Status == "Pending")
                        .Any() &&
                        _liveSubjects
                        .Where(s => s.IsEligible && s.Status == "Pending")
                        .All(s => s.IsSelected);

                    dgvEnrollment.InvalidateColumn(e.ColumnIndex);
                    Enrollment_UpdateTotalUnits();
                }
                return;
            }

            if (e.RowIndex >= 0 && colAction != null && e.ColumnIndex == colAction.Index)
            {
                var rowData = dgvEnrollment.Rows[e.RowIndex].DataBoundItem as EnrollmentData;
                if (rowData != null && !rowData.IsEligible)
                {
                    MessageBox.Show(rowData.PrerequisiteMessage, "Subject Locked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void dgvEnrollment_SelectionChanged(object sender, EventArgs e) => dgvEnrollment.ClearSelection();

        private void SetupMaroonBorders()
        {
            foreach (Panel p in new[] { pnlEnrollMiddleCard, pnlEnrollRightCard })
            {
                p.BackColor = Color.White;
                p.BorderStyle = BorderStyle.None;
                p.Paint += (s, ev) =>
                {
                    ControlPaint.DrawBorder(ev.Graphics, p.ClientRectangle,
                        Color.Maroon, 1, ButtonBorderStyle.Solid, Color.Maroon, 1, ButtonBorderStyle.Solid,
                        Color.Maroon, 1, ButtonBorderStyle.Solid, Color.Maroon, 1, ButtonBorderStyle.Solid);
                };
            }
        }

        private void btnDownloadCOR_Click(object sender, EventArgs e)
        {
            try
            {
                var img = Properties.Resources.CertificateOfRegistration;
                if (img == null) throw new Exception("Certificate asset not found.");
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

        private void Enrollment_ShowOverlay()
        {
            pnlViewDetails.Parent = pnlEnrollContent;
            pnlViewDetails.BringToFront();
            pnlViewDetails.Visible = true;
            pnlViewDetails.Location = new Point((pnlViewDetails.Parent.Width - pnlViewDetails.Width) / 2, (pnlViewDetails.Parent.Height - pnlViewDetails.Height) / 2);
        }

        private void Enrollment_HideOverlay() => pnlViewDetails.Visible = false;
        private void btnEnrollCloseDetails_Click(object sender, EventArgs e) => Enrollment_HideOverlay();

        private void Enrollment_viewDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvEnrollment.CurrentRow != null)
            {
                var data = dgvEnrollment.CurrentRow.DataBoundItem as EnrollmentData;
                if (data != null)
                {
                    lblDetailCode.Text = $"Code: {data.Code}";
                    lblDetailTitle.Text = $"Title: {data.CourseTitle}";
                    lblDetailUnits.Text = $"Units: {data.Units}";
                    txtDetailSchedule.Text = $"Schedule: {data.Schedule}";
                    lblDetailStatus.Text = $"Status: {data.Status}";
                }
            }
            Enrollment_ShowOverlay();
        }

        private void dropSubjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvEnrollment.CurrentRow == null) return;
            var data = dgvEnrollment.CurrentRow.DataBoundItem as EnrollmentData;
            if (data == null) return;

            if (data.Status == "Officially Enrolled")
            {
                MessageBox.Show("You cannot drop an officially enrolled subject here. Please visit the Registrar.", "Action Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show($"Are you sure you want to drop {data.Code} from your cart?", "Confirm Drop", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            _liveSubjects.Remove(data);
            dgvEnrollment.DataSource = new BindingList<EnrollmentData>(_liveSubjects);

            Enrollment_UpdateTotalUnits();
            CheckGlobalEnrollmentStatus();
        }

        private void btnDlOOWaiver_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] pdfBytes = Properties.Resources.OptOutWaiver;

                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    MessageBox.Show("The waiver form could not be found in the system.", "File Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string tempFolder = System.IO.Path.GetTempPath();

                string tempFilePath = System.IO.Path.Combine(tempFolder, "Opt-Out Waiver and Voluntary Contribution Form with Guidelines.pdf");

                System.IO.File.WriteAllBytes(tempFilePath, pdfBytes);

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = tempFilePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to open the document:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}