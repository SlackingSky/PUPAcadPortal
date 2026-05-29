using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    /// <summary>
    /// Inline Create / Edit Activity page.
    /// Emits OnSave(ActivityItem) or OnCancel; never shows a popup.
    /// </summary>
    public partial class ActivityFormPage : UserControl
    {
        public event Action<ActivityItem> OnSave;
        public event Action OnCancel;

        private readonly CourseActivity _course;
        private readonly ActivityItem? _editing;
        private readonly bool _isNew;

        // Working collections
        private List<QuizQuestion> _questions = new();
        private List<RubricCriteria> _rubricItems = new();
        private List<CourseFileItem> _files = new();
        private int _nextQId = 1, _nextRId = 1;

        public ActivityFormPage(CourseActivity course, ActivityItem? editing)
        {
            _course = course;
            _editing = editing;
            _isNew = editing == null;
            InitializeComponent();
            PopulateForm();
        }

        // ── Populate ──────────────────────────────────────────────────────────
        private void PopulateForm()
        {
            lblPageTitle.Text = _isNew ? "Create Activity" : "Edit Activity";
            lblCourseSub.Text = $"{_course.CourseCode} – {_course.CourseName}";
            btnSave.Text = _isNew ? "✔  Create Activity" : "✔  Save Changes";

            if (_editing != null)
            {
                txtTitle.Text = _editing.Title;
                txtDescription.Text = _editing.Description;
                SetCombo(cmbType, _editing.TypeString);
                dtpDeadline.Value = _editing.Deadline > DateTime.MinValue ? _editing.Deadline : DateTime.Now.AddDays(7);
                nudPoints.Value = Math.Clamp(_editing.Points, 1, 10000);
                chkRubric.Checked = _editing.HasRubric;
                _questions = new List<QuizQuestion>(_editing.Questions);
                _rubricItems = new List<RubricCriteria>(_editing.RubricItems);
                _files = new List<CourseFileItem>(_editing.AttachedFiles);
                _nextQId = _questions.Count > 0 ? _questions.Max(q => q.QuestionId) + 1 : 1;
                _nextRId = _rubricItems.Count > 0 ? _rubricItems.Max(r => r.CriteriaId) + 1 : 1;
            }

            RefreshTypePanel();
            RefreshQuizPanel();
            RefreshRubricPanel();
            RefreshFilesPanel();
        }

        // ── Type toggle ───────────────────────────────────────────────────────
        private void cmbType_SelectedIndexChanged(object sender, EventArgs e) => RefreshTypePanel();

        private void RefreshTypePanel()
        {
            string t = cmbType.SelectedItem?.ToString() ?? "Assignment";
            pnlQuizSection.Visible = t == "Quiz";
            pnlRubricSection.Visible = t is "Essay" or "Assignment";
            lblPointsNote.Text = t == "Quiz" ? "ℹ Points calculated from questions." : "";
        }

        // ── Quiz builder ──────────────────────────────────────────────────────
        private void btnAddQuestion_Click(object sender, EventArgs e)
        {
            _questions.Add(new QuizQuestion { QuestionId = _nextQId++, QuestionType = "MultipleChoice", Points = 1 });
            RefreshQuizPanel();
        }

        private void RefreshQuizPanel()
        {
            flpQuestions.SuspendLayout();
            flpQuestions.Controls.Clear();
            for (int i = 0; i < _questions.Count; i++)
                flpQuestions.Controls.Add(BuildQuestionRow(_questions[i], i + 1));
            flpQuestions.ResumeLayout();

            if (cmbType.SelectedItem?.ToString() == "Quiz" && _questions.Count > 0)
                nudPoints.Value = _questions.Sum(q => q.Points);
        }

        private Panel BuildQuestionRow(QuizQuestion q, int num)
        {
            int rowW = Math.Max(600, flpQuestions.ClientSize.Width - 16);
            var row = new Panel
            {
                Width = rowW,
                Height = 134,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 8)
            };
            row.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(225, 225, 235));
                e.Graphics.DrawRectangle(pen, 0, 0, row.Width - 1, row.Height - 1);
                using var accent = new SolidBrush(Color.FromArgb(63, 81, 181));
                e.Graphics.FillRectangle(accent, 0, 0, 4, row.Height);
            };

            var lblNum = new Label
            {
                Text = num.ToString(),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(63, 81, 181),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(12, 8),
                Size = new Size(28, 28)
            };

            var cmbQType = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(48, 8),
                Size = new Size(160, 25)
            };
            cmbQType.Items.AddRange(new object[] { "MultipleChoice", "Identification", "TrueFalse", "Essay" });
            SetCombo(cmbQType, q.QuestionType);

            var lblPts = new Label { Text = "pts:", Location = new Point(220, 12), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            var nudPts = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 1000,
                Value = q.Points,
                Location = new Point(250, 8),
                Size = new Size(60, 25),
                Font = new Font("Segoe UI", 9F)
            };
            nudPts.ValueChanged += (s, e) =>
            {
                q.Points = (int)nudPts.Value;
                if (cmbType.SelectedItem?.ToString() == "Quiz")
                    nudPoints.Value = _questions.Sum(x => x.Points);
            };

            var btnDel = new buttonRounded
            {
                Text = "✕",
                Size = new Size(28, 26),
                Location = new Point(rowW - 36, 8),
                BackColor = Color.FromArgb(195, 55, 55),
                ForeColor = Color.White,
                BorderRadius = 6,
                Font = new Font("Segoe UI", 9F)
            };
            btnDel.Click += (s, e) => { _questions.Remove(q); RefreshQuizPanel(); };

            var txtQ = new TextBox
            {
                Text = q.QuestionText,
                PlaceholderText = "Enter question text...",
                Font = new Font("Segoe UI", 10F),
                Location = new Point(12, 44),
                Size = new Size(rowW - 20, 26)
            };
            txtQ.TextChanged += (s, e) => q.QuestionText = txtQ.Text;

            var txtAns = new TextBox
            {
                Text = q.CorrectAnswer,
                PlaceholderText = GetAnswerHint(q.QuestionType),
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.DimGray,
                Location = new Point(12, 80),
                Size = new Size(rowW - 20, 26)
            };
            txtAns.TextChanged += (s, e) => q.CorrectAnswer = txtAns.Text;

            cmbQType.SelectedIndexChanged += (s, e) =>
            {
                q.QuestionType = cmbQType.SelectedItem?.ToString() ?? "MultipleChoice";
                txtAns.PlaceholderText = GetAnswerHint(q.QuestionType);
            };

            row.Controls.AddRange(new Control[] { lblNum, cmbQType, lblPts, nudPts, btnDel, txtQ, txtAns });

            row.SizeChanged += (s, e) =>
            {
                btnDel.Left = row.Width - 36;
                txtQ.Width = row.Width - 20;
                txtAns.Width = row.Width - 20;
            };
            return row;
        }

        private static string GetAnswerHint(string type) => type switch
        {
            "MultipleChoice" => "Choices: A|B|C|D   Answer key: B",
            "TrueFalse" => "Answer key: True or False",
            _ => "Answer key / model answer"
        };

        // ── Rubric builder ────────────────────────────────────────────────────
        private void chkRubric_CheckedChanged(object sender, EventArgs e)
        {
            pnlRubricRows.Visible = chkRubric.Checked;
            lblRubricNote.Visible = chkRubric.Checked;
        }

        private void btnAddCriteria_Click(object sender, EventArgs e)
        {
            _rubricItems.Add(new RubricCriteria { CriteriaId = _nextRId++, MaxPoints = 25 });
            RefreshRubricPanel();
        }

        private void RefreshRubricPanel()
        {
            flpRubric.SuspendLayout();
            flpRubric.Controls.Clear();
            foreach (var r in _rubricItems)
                flpRubric.Controls.Add(BuildRubricRow(r));
            flpRubric.ResumeLayout();

            if (chkRubric.Checked && _rubricItems.Count > 0)
            {
                int total = _rubricItems.Sum(r => r.MaxPoints);
                nudPoints.Value = Math.Clamp(total, 1, 10000);
                lblRubricNote.Text = $"📊 Rubric total: {total} pts — max score locked to rubric.";
            }
        }

        private Panel BuildRubricRow(RubricCriteria r)
        {
            int rowW = Math.Max(500, flpRubric.ClientSize.Width - 16);
            var row = new Panel
            {
                Width = rowW,
                Height = 60,
                BackColor = Color.FromArgb(250, 250, 253),
                Margin = new Padding(0, 0, 0, 4)
            };
            row.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(225, 225, 235));
                e.Graphics.DrawRectangle(pen, 0, 0, row.Width - 1, row.Height - 1);
            };

            var txtName = new TextBox { Text = r.Name, PlaceholderText = "Criteria name", Font = new Font("Segoe UI", 9.5F), Location = new Point(8, 8), Size = new Size(200, 26) };
            txtName.TextChanged += (s, e) => r.Name = txtName.Text;

            var txtDesc = new TextBox { Text = r.Description, PlaceholderText = "Description (optional)", Font = new Font("Segoe UI", 9.5F), Location = new Point(218, 8), Size = new Size(rowW - 400, 26) };
            txtDesc.TextChanged += (s, e) => r.Description = txtDesc.Text;

            var lblPts = new Label { Text = "Max pts:", Location = new Point(rowW - 175, 14), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            var nudPts = new NumericUpDown { Minimum = 1, Maximum = 9999, Value = Math.Max(1, r.MaxPoints), Location = new Point(rowW - 110, 8), Size = new Size(74, 26), Font = new Font("Segoe UI", 9F) };
            nudPts.ValueChanged += (s, e) => { r.MaxPoints = (int)nudPts.Value; RefreshRubricPanel(); };

            var btnDel = new buttonRounded { Text = "✕", Size = new Size(26, 26), Location = new Point(rowW - 34, 8), BackColor = Color.FromArgb(195, 55, 55), ForeColor = Color.White, BorderRadius = 5 };
            btnDel.Click += (s, e) => { _rubricItems.Remove(r); RefreshRubricPanel(); };

            row.Controls.AddRange(new Control[] { txtName, txtDesc, lblPts, nudPts, btnDel });
            row.SizeChanged += (s, e) =>
            {
                txtDesc.Width = row.Width - 400;
                lblPts.Left = row.Width - 175;
                nudPts.Left = row.Width - 110;
                btnDel.Left = row.Width - 34;
            };
            return row;
        }

        // ── File attach panel ─────────────────────────────────────────────────
        private void btnAttachFile_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Title = "Attach File to Activity",
                Multiselect = true,
                Filter = "All Files (*.*)|*.*|PDF|*.pdf|Word|*.docx|Images|*.png;*.jpg"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            foreach (var path in ofd.FileNames)
            {
                var fi = new System.IO.FileInfo(path);
                _files.Add(new CourseFileItem
                {
                    FileId = _files.Count + 1,
                    FileName = fi.Name,
                    FilePath = path,
                    FileType = fi.Extension.TrimStart('.').ToUpper(),
                    FileSizeBytes = fi.Length,
                    UploadedAt = DateTime.Now,
                    CourseId = _course.CourseId
                });
            }
            RefreshFilesPanel();
        }

        private void RefreshFilesPanel()
        {
            flpFiles.SuspendLayout();
            flpFiles.Controls.Clear();
            foreach (var f in _files)
                flpFiles.Controls.Add(BuildFileChip(f));
            lblNoFiles.Visible = _files.Count == 0;
            flpFiles.ResumeLayout();
        }

        private Panel BuildFileChip(CourseFileItem f)
        {
            var chip = new Panel
            {
                Width = 248,
                Height = 44,
                BackColor = Color.FromArgb(246, 246, 251),
                Margin = new Padding(0, 0, 8, 6)
            };
            chip.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(215, 215, 230));
                e.Graphics.DrawRectangle(pen, 0, 0, chip.Width - 1, chip.Height - 1);
            };

            chip.Controls.Add(new Label { Text = FileIcon(f.FileType), Font = new Font("Segoe UI", 14F), Location = new Point(6, 8), AutoSize = true });
            chip.Controls.Add(new Label { Text = f.FileName, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), Location = new Point(36, 6), Width = 168, Height = 16, AutoEllipsis = true });
            chip.Controls.Add(new Label { Text = FormatBytes(f.FileSizeBytes), Font = new Font("Segoe UI", 8F), ForeColor = Color.Gray, Location = new Point(36, 24), AutoSize = true });

            var captured = f;
            var btnRm = new buttonRounded { Text = "✕", Size = new Size(22, 22), Location = new Point(chip.Width - 28, 11), BackColor = Color.FromArgb(200, 55, 55), ForeColor = Color.White, BorderRadius = 5, Font = new Font("Segoe UI", 8F) };
            btnRm.Click += (s, e) => { _files.Remove(captured); RefreshFilesPanel(); };
            chip.Controls.Add(btnRm);
            return chip;
        }

        // ── Save ──────────────────────────────────────────────────────────────
        private void btnSave_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            string type = cmbType.SelectedItem?.ToString() ?? "Assignment";

            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            { lblError.Text = "⚠  Activity title is required."; txtTitle.Focus(); return; }

            if (_isNew && dtpDeadline.Value <= DateTime.Now.AddMinutes(-1))
            { lblError.Text = "⚠  Deadline must be in the future."; return; }

            if (type == "Quiz" && _questions.Count == 0)
            { lblError.Text = "⚠  Add at least one question for a Quiz."; return; }

            var act = _editing ?? new ActivityItem { CourseId = _course.CourseId };
            act.Title = txtTitle.Text.Trim();
            act.Description = txtDescription.Text.Trim();
            act.Type = Enum.TryParse<ActivityType>(type, out var at) ? at : ActivityType.Assignment;
            act.Deadline = dtpDeadline.Value;
            act.Points = (int)nudPoints.Value;
            act.HasRubric = chkRubric.Checked;
            act.Questions = new List<QuizQuestion>(_questions);
            act.RubricItems = new List<RubricCriteria>(_rubricItems);
            act.AttachedFiles = new List<CourseFileItem>(_files);
            if (act.TotalStudents == 0) act.TotalStudents = 35;

            OnSave?.Invoke(act);
        }

        private void btnCancel_Click(object sender, EventArgs e) => OnCancel?.Invoke();

        // ── Helpers ───────────────────────────────────────────────────────────
        private static void SetCombo(ComboBox c, string val)
        { int i = c.FindStringExact(val); c.SelectedIndex = i >= 0 ? i : 0; }

        private static string FileIcon(string ext) => ext.ToUpper() switch
        {
            "PDF" => "📄",
            "DOCX" or "DOC" => "📝",
            "XLSX" or "XLS" => "📊",
            "PNG" or "JPG" or "JPEG" => "🖼",
            "ZIP" or "RAR" => "🗜",
            _ => "📎"
        };

        private static string FormatBytes(long b)
        {
            if (b < 1024) return $"{b} B";
            if (b < 1048576) return $"{b / 1024.0:F1} KB";
            return $"{b / 1048576.0:F1} MB";
        }
    }
}