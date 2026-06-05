// ============================================================
//  GradingInterface.cs  — UPDATED
//  Adds IActivityDbService parameter so btnSaveScore persists
//  the grade directly to the Submissions table.
//  All UI logic is identical to the original.
// ============================================================

using PUPAcadPortal.PortalContents.Instructor.LMS.Course;
using PUPAcadPortal.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class GradingInterface : UserControl
    {
        public event Action OnBack;

        private StudentSubmission _current;
        private readonly ActivityItem _activity;
        private readonly List<StudentSubmission> _students;
        private readonly IActivityDbService? _svc;

        private int _index;
        private System.Windows.Forms.Timer _autoSaveTimer;
        private readonly List<(RubricCriteria criteria, NumericUpDown nud)> _rubricRows = new();

        // ── NEW constructor (DB-backed) ───────────────────────
        public GradingInterface(
            StudentSubmission submission,
            ActivityItem activity,
            List<StudentSubmission> allStudents,
            int index,
            IActivityDbService? svc)
        {
            _current = submission;
            _activity = activity;
            _students = allStudents ?? new List<StudentSubmission> { submission };
            _index = index;
            _svc = svc;

            InitializeComponent();
            BuildRubricRows();
            SetupAutoSave();
            LoadStudent();
            UpdateNavButtons();
        }

        // ── Backward-compatible constructor (no DB) ───────────
        public GradingInterface(
            StudentSubmission submission,
            ActivityItem activity,
            List<StudentSubmission> allStudents = null,
            int index = 0)
            : this(submission, activity, allStudents, index, null) { }

        // ════════════════════════════════════════════════════
        //  Rubric row builder — UNCHANGED
        // ════════════════════════════════════════════════════
        private void BuildRubricRows()
        {
            _rubricRows.Clear();
            flpRubricRows.Controls.Clear();

            var items = _activity.RubricItems.Count > 0
                ? _activity.RubricItems
                : new List<RubricCriteria>
                {
                    new RubricCriteria { CriteriaId = 1, Name = "Content",   MaxPoints = 25 },
                    new RubricCriteria { CriteriaId = 2, Name = "Structure", MaxPoints = 25 },
                    new RubricCriteria { CriteriaId = 3, Name = "Grammar",   MaxPoints = 25 },
                    new RubricCriteria { CriteriaId = 4, Name = "Relevance", MaxPoints = 25 }
                };

            foreach (var crit in items)
            {
                var row = BuildRubricRow(crit, out var nud);
                _rubricRows.Add((crit, nud));
                flpRubricRows.Controls.Add(row);
            }

            UpdateRubricTotal();
        }

        private Panel BuildRubricRow(RubricCriteria crit, out NumericUpDown nud)
        {
            var row = new Panel { Width = 406, Height = 34, BackColor = Color.Transparent };

            var lbl = new Label
            {
                Text = crit.Name,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 60),
                Location = new Point(0, 8),
                Size = new Size(110, 20)
            };

            nud = new NumericUpDown
            {
                Minimum = 0,
                Maximum = crit.MaxPoints,
                Value = 0,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(118, 5),
                Size = new Size(62, 24)
            };
            nud.ValueChanged += nudRubric_ValueChanged;

            var lblMax = new Label
            {
                Text = $"/ {crit.MaxPoints}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(186, 9),
                Size = new Size(50, 18)
            };

            var bar = new Panel { Location = new Point(240, 12), Size = new Size(160, 10), BackColor = Color.FromArgb(235, 235, 240) };
            var fill = new Panel { Location = new Point(0, 0), Size = new Size(0, 10), BackColor = Color.FromArgb(128, 0, 0) };
            bar.Controls.Add(fill);

            // Capture nud in a local so the lambda doesn't close over an out parameter.
            var nudLocal = nud;
            nudLocal.ValueChanged += (s, e) =>
            {
                int pct = crit.MaxPoints > 0 ? (int)((double)nudLocal.Value / crit.MaxPoints * 160) : 0;
                fill.Width = Math.Clamp(pct, 0, 160);
                UpdateRubricTotal();
            };

            row.Controls.AddRange(new Control[] { lbl, nudLocal, lblMax, bar });
            return row;
        }

        // ════════════════════════════════════════════════════
        //  Auto-save (unchanged logic, now calls DB)
        // ════════════════════════════════════════════════════
        private void SetupAutoSave()
        {
            _autoSaveTimer = new System.Windows.Forms.Timer { Interval = 30_000 };
            _autoSaveTimer.Tick += (s, e) =>
            {
                if (int.TryParse(txtScore.Text, out int sc))
                {
                    _current.Score = sc;
                    _current.Remarks = txtRemarks.Text;

                    // Persist auto-save to DB (non-locking — doesn't set IsChecked)
                    TrySaveGradeToDb(sc, lock_: false);

                    lblSaveStatus.Text = $"Auto-saved at {DateTime.Now:hh:mm tt}";
                    lblSaveStatus.ForeColor = Color.DimGray;
                }
            };
            _autoSaveTimer.Start();
        }

        // ════════════════════════════════════════════════════
        //  Load student data into controls — UNCHANGED
        // ════════════════════════════════════════════════════
        private void LoadStudent()
        {
            lblActivityTitle.Text = _activity.Title;
            lblMaxPoints.Text = $"Max: {_activity.Points} pts";
            lblStudentName.Text = _current.StudentName;
            lblStudentId.Text = _current.StudentId;
            lblSubmissionTime.Text = _current.SubmissionTime == DateTime.MinValue
                ? "Not submitted"
                : $"Submitted: {_current.SubmissionTime:MMM dd, yyyy  hh:mm tt}";

            txtEssayContent.Text = _current.EssayContent;
            txtRemarks.Text = _current.Remarks;
            txtScore.Text = _current.Score >= 0 ? _current.Score.ToString() : "";
            lblScoreOf.Text = $"/ {_activity.Points}";

            bool locked = _current.IsChecked;
            txtScore.ReadOnly = locked;
            btnSaveScore.Enabled = !locked;
            txtRemarks.ReadOnly = locked;

            foreach (var (crit, nud) in _rubricRows)
            {
                nud.Value = _current.RubricScores.TryGetValue(crit.CriteriaId, out int v)
                              ? Math.Min(v, crit.MaxPoints) : 0;
                nud.Enabled = !locked;
            }

            chkAutoScore.Enabled = !locked;
            lblSaveStatus.Text = locked ? "✅ Already checked – score locked" : "";
            lblSaveStatus.ForeColor = locked
                ? Color.FromArgb(128, 0, 0)
                : Color.ForestGreen;

            UpdateWordCount();
            UpdateRubricTotal();
        }

        // ════════════════════════════════════════════════════
        //  Save & lock score — NOW persists to DB
        // ════════════════════════════════════════════════════
        private void btnSaveScore_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtScore.Text, out int score))
            {
                MessageBox.Show("Please enter a valid score.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (score < 0 || score > _activity.Points)
            {
                MessageBox.Show($"Score must be between 0 and {_activity.Points}.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Collect rubric scores into ViewModel
            foreach (var (crit, nud) in _rubricRows)
                _current.RubricScores[crit.CriteriaId] = (int)nud.Value;

            _current.Score = score;
            _current.Remarks = txtRemarks.Text;

            // ── Persist to DB ────────────────────────────────
            if (!TrySaveGradeToDb(score, lock_: true))
                return;   // error already shown by helper

            // ── Lock UI ──────────────────────────────────────
            _current.IsChecked = true;
            txtScore.ReadOnly = true;
            btnSaveScore.Enabled = false;
            txtRemarks.ReadOnly = true;
            foreach (var (_, nud) in _rubricRows) nud.Enabled = false;
            chkAutoScore.Enabled = false;

            lblSaveStatus.Text = $"✅ Saved at {DateTime.Now:hh:mm tt}";
            lblSaveStatus.ForeColor = Color.FromArgb(46, 160, 67);

            MessageBox.Show(
                $"Score saved: {score}/{_activity.Points}",
                "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ── DB helper — returns false on error ────────────────
        private bool TrySaveGradeToDb(int score, bool lock_)
        {
            if (_svc == null || string.IsNullOrEmpty(_current.SubmissionDbId))
                return true;   // no service or no DB row yet — skip silently

            try
            {
                _svc.SaveGrade(_current.SubmissionDbId, score, _current.Remarks);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Grade save failed:\n{ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // ════════════════════════════════════════════════════
        //  Remaining helpers — UNCHANGED
        // ════════════════════════════════════════════════════
        private void UpdateRubricTotal()
        {
            int rubricMax = _rubricRows.Sum(r => (int)r.criteria.MaxPoints);
            if (rubricMax == 0) rubricMax = _activity.Points;

            int total = _rubricRows.Sum(r => (int)r.nud.Value);
            lblRubricTotal.Text = $"Rubric Total: {total} / {rubricMax}";

            if (chkAutoScore.Checked)
            {
                double pct = rubricMax > 0 ? (double)total / rubricMax : 0;
                txtScore.Text = ((int)Math.Round(pct * _activity.Points)).ToString();
            }
        }

        private void nudRubric_ValueChanged(object sender, EventArgs e)
            => UpdateRubricTotal();

        private void UpdateWordCount()
        {
            string text = txtEssayContent.Text.Trim();
            int words = string.IsNullOrWhiteSpace(text) ? 0
                : text.Split(new[] { ' ', '\n', '\r', '\t' },
                             StringSplitOptions.RemoveEmptyEntries).Length;
            lblWordCount.Text = $"Words: {words}";
            lblCharCount.Text = $"Characters: {text.Length}";
        }

        private void txtEssayContent_TextChanged(object sender, EventArgs e)
            => UpdateWordCount();

        private void chkAutoScore_CheckedChanged(object sender, EventArgs e)
        {
            txtScore.ReadOnly = chkAutoScore.Checked || _current.IsChecked;
            UpdateRubricTotal();
        }

        private void btnNextStudent_Click(object sender, EventArgs e)
        {
            if (_index >= _students.Count - 1) return;
            _current = _students[++_index];
            LoadStudent();
            UpdateNavButtons();
        }

        private void btnPrevStudent_Click(object sender, EventArgs e)
        {
            if (_index <= 0) return;
            _current = _students[--_index];
            LoadStudent();
            UpdateNavButtons();
        }

        private void UpdateNavButtons()
        {
            btnPrevStudent.Enabled = _index > 0;
            btnNextStudent.Enabled = _index < _students.Count - 1;
            lblNavCounter.Text = $"{_index + 1} / {_students.Count}";
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            _autoSaveTimer?.Stop();
            _autoSaveTimer?.Dispose();
            _autoSaveTimer = null;

            if (OnBack != null) { OnBack.Invoke(); return; }

            Control container = this.Parent;
            if (container != null)
            {
                container.Controls.Remove(this);
                this.Dispose();
            }
        }
    }
}