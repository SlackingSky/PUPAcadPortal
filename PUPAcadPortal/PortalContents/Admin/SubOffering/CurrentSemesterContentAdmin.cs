using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using PUPAcadPortal.Services;

namespace PUPAcadPortal.PortalContents.Admin.SubOffering
{
    public partial class CurrentSemesterContentAdmin : UserControl
    {
        private readonly SemesterSetupService _setupService;

        public CurrentSemesterContentAdmin()
        {
            InitializeComponent();
            _setupService = new SemesterSetupService();
            this.Load += CurrentSemesterContentAdmin_Load;

            btnInitialize.Click += btnInitialize_Click;
            btnSetCurrent.Click += btnSetCurrent_Click;
        }

        private async void CurrentSemesterContentAdmin_Load(object sender, EventArgs e)
        {
            dgvCurrentSemester.AutoGenerateColumns = false;

            dgvCurrentSemester.Columns["SubjectCode"].DataPropertyName = "SubjectCode";
            dgvCurrentSemester.Columns["SubjectTitle"].DataPropertyName = "SubjectTitle";
            dgvCurrentSemester.Columns["Lab"].DataPropertyName = "Lab";
            dgvCurrentSemester.Columns["Lec"].DataPropertyName = "Lec";
            dgvCurrentSemester.Columns["TotalUnits"].DataPropertyName = "TotalUnits";
            dgvCurrentSemester.Columns["Year"].DataPropertyName = "YearLevel";

            cmbSY.Items.Clear();
            int currentYear = DateTime.Now.Year;
            for (int i = -1; i < 3; i++)
            {
                int startYear = currentYear + i;
                cmbSY.Items.Add($"{startYear}-{startYear + 1}");
            }

            cmbSY.SelectedIndex = 1;
            cmbSem.SelectedIndex = 0;

            cmbSY.SelectedIndexChanged += async (s, ev) => await LoadOfferingsGrid();
            cmbSem.SelectedIndexChanged += async (s, ev) => await LoadOfferingsGrid();

            await LoadOfferingsGrid();
            await UpdateButtonState();
        }

        private async Task UpdateButtonState()
        {
            bool isAlreadyActive = _setupService.IsAnySemesterActive();

            btnSetCurrent.Enabled = !isAlreadyActive;

            if (isAlreadyActive)
            {
                btnSetCurrent.Text = "System Locked (Semester Active)";
                btnSetCurrent.BackColor = SystemColors.GrayText;
            }
            else
            {
                btnSetCurrent.Text = "2. Activate Semester (Lock)";
                btnSetCurrent.BackColor = Color.FromArgb(109, 0, 0);
            }
        }

        private async Task LoadOfferingsGrid()
        {
            if (cmbSY.SelectedItem == null || cmbSem.SelectedItem == null) return;

            string syFull = cmbSY.SelectedItem.ToString();
            string semRaw = cmbSem.SelectedItem.ToString();

            string? periodId = await _setupService.GetAcademicPeriodIdAsync(syFull, semRaw);

            if (periodId == null)
            {
                var previewData = await _setupService.GetCurriculumPreviewAsync(syFull, semRaw);
                dgvCurrentSemester.DataSource = new BindingList<SemesterGridItem>(previewData);

                btnSetCurrent.Enabled = false;

                btnInitialize.Enabled = true;
                btnInitialize.Text = "1. Generate Classes (Draft)";

                return;
            }

            var data = await _setupService.GetOfferingsForGridAsync(periodId, syFull, semRaw);
            dgvCurrentSemester.DataSource = new BindingList<SemesterGridItem>(data);

            string status = await _setupService.GetPeriodStatusAsync(syFull, semRaw);

            bool isDraftable = (status == "Inactive");

            btnInitialize.Enabled = isDraftable;
            btnInitialize.Text = isDraftable ? "1. Generate Classes (Draft)" : "Class Generation Locked";

            bool hasGeneratedItems = data.Count > 0 && data[0].Status != "Draft (Not Generated)";
            btnSetCurrent.Enabled = (hasGeneratedItems && isDraftable);
        }

        private async void btnInitialize_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to make a draft of an academic period?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                btnInitialize.Enabled = false;
                Application.UseWaitCursor = true;

                string syFull = cmbSY.SelectedItem.ToString();
                string semRaw = cmbSem.SelectedItem.ToString();

                var validation = await _setupService.CanCreateOrGenerateDraftAsync(syFull, semRaw);
                if (!validation.IsAllowed)
                {
                    MessageBox.Show(validation.Message, "Invalid Action", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var guard = await _setupService.CanInitializePeriod(syFull, semRaw);
                if (!guard.CanProceed)
                {
                    MessageBox.Show(guard.Message, "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string periodId = await _setupService.EnsureAcademicPeriodExistsAsync(syFull, semRaw);

                await _setupService.GenerateOfferingsFromCurriculumAsync(periodId, syFull, semRaw);

                await LoadOfferingsGrid();

                MessageBox.Show(
                    "Draft classes successfully generated based on the Curriculum!\n\n" +
                    "The semester is still in DRAFT mode. You can safely go to the 'Edit Schedules' tab to add Rooms and Professors.",
                    "Setup Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to generate classes: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnInitialize.Enabled = true;
                Application.UseWaitCursor = false;
            }
        }

        private async void btnSetCurrent_Click(object sender, EventArgs e)
        {
            if (cmbSY.SelectedItem == null || cmbSem.SelectedItem == null) return;

            try
            {
                btnSetCurrent.Enabled = false;
                Application.UseWaitCursor = true;

                string syFull = cmbSY.SelectedItem.ToString();
                string semRaw = cmbSem.SelectedItem.ToString();
                string periodId = await _setupService.EnsureAcademicPeriodExistsAsync(syFull, semRaw);

                var sequence = await _setupService.ValidateSequentialActivationAsync(periodId);
                if (!sequence.IsValid)
                {
                    MessageBox.Show(sequence.ErrorMessage, "Sequence Violation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var verification = await _setupService.ValidateBeforeActivationAsync(periodId);
                if (!verification.IsValid)
                {
                    MessageBox.Show(verification.ErrorMessage, "Scheduling Restrictions", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirmResult = MessageBox.Show(
                    "Are you sure you want to activate this semester?\n\nThis will officially start the academic period and lock the schedules from further editing.",
                    "Confirm Activation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirmResult != DialogResult.Yes) return;

                await _setupService.SetCurrentPeriodAsync(periodId);
                MessageBox.Show("The semester has been activated and is now LIVE.", "System Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to activate semester: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                await LoadOfferingsGrid();
                Application.UseWaitCursor = false;
            }
        }
    }
}