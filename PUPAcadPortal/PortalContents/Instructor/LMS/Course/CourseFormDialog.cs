using PUPAcadPortal.Services;
using System;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    /// <summary>
    /// Modal dialog for creating or editing a SubjectOffering (course).
    /// All dropdown data (subjects, academic periods) is loaded from the database.
    /// On OK, the validated DTO is written to the database via <see cref="ICourseDbService"/>.
    /// </summary>
    public partial class CourseFormDialog : Form
    {
        // ── Public output ─────────────────────────────────────────────────────
        public CourseDto Result { get; private set; } = new CourseDto();

        // ── State ──────────────────────────────────────────────────────────────
        private readonly ICourseDbService _svc;
        private readonly int _professorId;
        private readonly CourseDto? _editing;   // null = create mode

        private System.Collections.Generic.List<SubjectLookupDto> _subjects = new();
        private System.Collections.Generic.List<AcademicPeriodLookupDto> _periods = new();

        // ── Create constructor ────────────────────────────────────────────────
        public CourseFormDialog(int professorId, ICourseDbService svc)
            : this(professorId, null, svc) { }

        // ── Edit constructor ──────────────────────────────────────────────────
        public CourseFormDialog(int professorId, CourseDto? existing, ICourseDbService svc)
        {
            _professorId = professorId;
            _editing = existing;
            _svc = svc ?? throw new ArgumentNullException(nameof(svc));

            InitializeComponent();

            lblHeaderTitle.Text = existing == null ? "Create Course" : "Edit Course";
            btnSave.Text = existing == null ? "Create" : "Save Changes";

            this.Load += CourseFormDialog_Load;
        }

        // ── Load ──────────────────────────────────────────────────────────────
        private void CourseFormDialog_Load(object? sender, EventArgs e)
        {
            LoadDropdowns();
            if (_editing != null) PreFillFields();
        }

        private void LoadDropdowns()
        {
            try
            {
                _subjects = _svc.GetAllSubjects();
                _periods = _svc.GetAllAcademicPeriods();
            }
            catch (Exception ex)
            {
                lblError.Text = $"⚠ Failed to load dropdown data: {ex.Message}";
                btnSave.Enabled = false;
                return;
            }

            cmbSubject.BeginUpdate();
            cmbSubject.Items.Clear();
            foreach (var s in _subjects)
                cmbSubject.Items.Add(s.Display);
            cmbSubject.EndUpdate();

            cmbPeriod.BeginUpdate();
            cmbPeriod.Items.Clear();
            foreach (var p in _periods)
                cmbPeriod.Items.Add(p.Display);
            cmbPeriod.EndUpdate();

            if (_subjects.Count == 0)
            {
                lblError.Text = "⚠ No subjects found in the database. Please add subjects first.";
                btnSave.Enabled = false;
            }
            if (_periods.Count == 0)
            {
                lblError.Text += "\n⚠ No academic periods found.";
                btnSave.Enabled = false;
            }
        }

        private void PreFillFields()
        {
            // Subject
            int si = _subjects.FindIndex(s => s.SubjectId == _editing!.SubjectId);
            if (si >= 0) cmbSubject.SelectedIndex = si;

            // Period
            int pi = _periods.FindIndex(p => p.AcademicPeriodId == _editing!.AcademicPeriodId);
            if (pi >= 0) cmbPeriod.SelectedIndex = pi;

            txtSection.Text = _editing!.Section;
            nudMaxSlots.Value = _editing.MaxSlots > 0 ? _editing.MaxSlots : 40;

            int statusIdx = cmbStatus.FindStringExact(_editing.Status);
            cmbStatus.SelectedIndex = statusIdx >= 0 ? statusIdx : 0;
        }

        // ── Save ──────────────────────────────────────────────────────────────
        private void BtnSave_Click(object? sender, EventArgs e)
        {
            lblError.Text = "";

            // Validate
            if (cmbSubject.SelectedIndex < 0)
            { lblError.Text = "⚠ Please select a subject."; return; }
            if (cmbPeriod.SelectedIndex < 0)
            { lblError.Text = "⚠ Please select an academic period."; return; }
            if (string.IsNullOrWhiteSpace(txtSection.Text))
            { lblError.Text = "⚠ Section is required."; txtSection.Focus(); return; }

            var dto = new CourseDto
            {
                SubjectOfferingId = _editing?.SubjectOfferingId ?? string.Empty,
                SubjectId = _subjects[cmbSubject.SelectedIndex].SubjectId,
                AcademicPeriodId = _periods[cmbPeriod.SelectedIndex].AcademicPeriodId,
                Section = txtSection.Text.Trim(),
                MaxSlots = (int)nudMaxSlots.Value,
                Status = cmbStatus.SelectedItem?.ToString() ?? "Active",
            };

            try
            {
                if (_editing == null)
                {
                    // CREATE
                    Result = _svc.CreateCourse(_professorId, dto);
                }
                else
                {
                    // UPDATE
                    _svc.UpdateCourse(dto);
                    Result = dto;
                    Result.SubjectOfferingId = _editing.SubjectOfferingId;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                lblError.Text = $"⚠ {ex.Message}";
            }
        }

        private void cmbSubject_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}