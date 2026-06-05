using PUPAcadPortal.PortalContents.Instructor.LMS.Course;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PUPAcadPortal.Models;

namespace PUPAcadPortal
{
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

        // DB-loaded dropdowns (populated when service is available)
        private List<Models.GradingCategory> _dbCategories = new();
        private List<Models.Module> _dbModules = new();

        // ── Primary constructor (4-arg, DB-backed) ────────────
        public ActivityFormPage(
            CourseActivity course,
            ActivityItem? editing,
            List<Models.GradingCategory> categories,
            List<Models.Module> modules)
        {
            _course = course;
            _editing = editing;
            _isNew = editing == null;
            _dbCategories = categories ?? new();
            _dbModules = modules ?? new();

            InitializeComponent();
            PopulateForm();
        }

        //  Backward-compatible 3-arg overload 
        public ActivityFormPage(CourseActivity course, ActivityItem? editing, List<Models.GradingCategory> categories)
            : this(course, editing, categories, new List<Models.Module>()) { }

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

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e) => RefreshTypePanel();

        private void RefreshTypePanel()
        {
            string t = cmbType.SelectedItem?.ToString() ?? "Assignment";
            pnlQuizSection.Visible = t == "Quiz";
            pnlRubricSection.Visible = t is "Essay" or "Assignment";
            lblPointsNote.Text = t == "Quiz" ? "ℹ Points calculated from questions." : "";

            if (pnlRubricSection.Visible && chkRubric.Checked)
                ApplyRubricLayout();
        }

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

        private static readonly string[] ChoiceLetters = { "A", "B", "C", "D", "E", "F", "G", "H" };

        private Panel BuildQuestionRow(QuizQuestion q, int num)
        {
            if (q.QuestionType == "MultipleChoice" && q.Choices.Count < 4)
            {
                while (q.Choices.Count < 4) q.Choices.Add("");
            }

            int rowW = Math.Max(700, flpQuestions.ClientSize.Width - 16);
            var row = new Panel
            {
                Width = rowW,
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
                Location = new Point(12, 10),
                Size = new Size(28, 28)
            };

            var cmbQType = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(48, 10),
                Size = new Size(160, 25)
            };
            cmbQType.Items.AddRange(new object[] { "MultipleChoice", "Identification", "TrueFalse", "Essay" });
            SetCombo(cmbQType, q.QuestionType);

            var lblPts = new Label { Text = "pts:", Location = new Point(220, 14), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            var nudPts = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 1000,
                Value = q.Points,
                Location = new Point(250, 10),
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
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
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
                Location = new Point(12, 46),
                Size = new Size(rowW - 20, 26)
            };
            txtQ.TextChanged += (s, e) => q.QuestionText = txtQ.Text;
            int splitX = (int)(rowW * 0.55);
            int rightW = rowW - splitX - 12;

            var pnlChoices = new Panel
            {
                Location = new Point(12, 80),
                BackColor = Color.Transparent
            };
            var pnlRight = new Panel
            {
                Location = new Point(splitX, 80),
                Width = rightW,
                BackColor = Color.FromArgb(248, 248, 252)
            };
            pnlRight.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(210, 210, 230));
                e.Graphics.DrawRectangle(pen, 0, 0, pnlRight.Width - 1, pnlRight.Height - 1);
            };
            var lblCorrect = new Label
            {
                Text = "✔  Correct Answer:",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 130, 80),
                Location = new Point(10, 10),
                AutoSize = true
            };

            var cmbCorrect = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 130, 80),
                Location = new Point(10, 34),
                Size = new Size(rightW - 20, 28)
            };

            var cmbTF = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 130, 80),
                Location = new Point(10, 34),
                Size = new Size(rightW - 20, 28),
                Visible = false
            };
            cmbTF.Items.AddRange(new object[] { "True", "False" });
            var txtOpenAns = new TextBox
            {
                PlaceholderText = "Model answer / key phrase...",
                Font = new Font("Segoe UI", 9.5F),
                Location = new Point(10, 34),
                Size = new Size(rightW - 20, 26),
                Visible = false
            };
            txtOpenAns.TextChanged += (s, e) => q.CorrectAnswer = txtOpenAns.Text;

            pnlRight.Controls.AddRange(new Control[] { lblCorrect, cmbCorrect, cmbTF, txtOpenAns });
            cmbCorrect.SelectedIndexChanged += (s, e) =>
                q.CorrectAnswer = cmbCorrect.SelectedItem?.ToString() ?? "";
            void SelectSavedAnswer()
            {
                if (cmbCorrect.Items.Contains(q.CorrectAnswer))
                    cmbCorrect.SelectedItem = q.CorrectAnswer;
                else if (cmbCorrect.Items.Count > 0)
                    cmbCorrect.SelectedIndex = 0;
            }

            void RebuildChoices()
            {
                pnlChoices.SuspendLayout();
                pnlChoices.Controls.Clear();
                int choiceW = splitX - 20;
                int y = 0;

                for (int ci = 0; ci < q.Choices.Count; ci++)
                {
                    int idx = ci;
                    string letter = idx < ChoiceLetters.Length ? ChoiceLetters[idx] : $"#{idx + 1}";

                    var lblLetter = new Label
                    {
                        Text = letter + ".",
                        Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                        ForeColor = Color.FromArgb(63, 81, 181),
                        Location = new Point(0, y + 4),
                        Size = new Size(24, 22)
                    };

                    var txtChoice = new TextBox
                    {
                        Text = q.Choices[idx],
                        PlaceholderText = $"Choice {letter}...",
                        Font = new Font("Segoe UI", 9.5F),
                        Location = new Point(28, y),
                        Size = new Size(choiceW - 66, 26)
                    };
                    int capturedIdx = idx;
                    txtChoice.TextChanged += (s, e) =>
                    {
                        if (capturedIdx < q.Choices.Count)
                            q.Choices[capturedIdx] = txtChoice.Text;
                        RefreshCorrectAnswerCombo(q, cmbCorrect);
                        SelectSavedAnswer();
                    };

                    var btnRemChoice = new buttonRounded
                    {
                        Text = "−",
                        Size = new Size(26, 26),
                        Location = new Point(choiceW - 34, y),
                        BackColor = idx >= 4
                            ? Color.FromArgb(195, 55, 55)
                            : Color.FromArgb(200, 200, 210),
                        ForeColor = Color.White,
                        BorderRadius = 5,
                        Enabled = idx >= 4,   // A–D are permanent
                        Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                    };
                    btnRemChoice.Click += (s, e) =>
                    {
                        if (q.Choices.Count > 4)
                        {
                            q.Choices.RemoveAt(idx);
                            RebuildChoices();
                        }
                    };

                    pnlChoices.Controls.AddRange(new Control[] { lblLetter, txtChoice, btnRemChoice });
                    y += 32;
                }

                if (q.Choices.Count < ChoiceLetters.Length)
                {
                    var btnAddChoice = new buttonRounded
                    {
                        Text = $"+ Add {(q.Choices.Count < ChoiceLetters.Length ? ChoiceLetters[q.Choices.Count] : "?")} Choice",
                        Size = new Size(130, 26),
                        Location = new Point(28, y),
                        BackColor = Color.FromArgb(63, 81, 181),
                        ForeColor = Color.White,
                        BorderRadius = 6,
                        Font = new Font("Segoe UI", 8.5F, FontStyle.Bold)
                    };
                    btnAddChoice.Click += (s, e) =>
                    {
                        q.Choices.Add("");
                        RebuildChoices();
                    };
                    pnlChoices.Controls.Add(btnAddChoice);
                    y += 32;
                }

                pnlChoices.Height = y + 4;
                pnlChoices.Width = choiceW;

                RefreshCorrectAnswerCombo(q, cmbCorrect);
                SelectSavedAnswer();

                pnlChoices.ResumeLayout();
                pnlRight.Location = new Point(splitX, 80);
                pnlRight.Height = Math.Max(pnlChoices.Height, 80);

                row.Height = 80 + Math.Max(pnlChoices.Height, pnlRight.Height) + 12;
                row.Invalidate();
            }

            void ApplyQuestionType(string qt)
            {
                q.QuestionType = qt;
                bool isMC = qt == "MultipleChoice";
                bool isTF = qt == "TrueFalse";
                bool isOpen = qt is "Identification" or "Essay";

                pnlChoices.Visible = isMC;
                cmbCorrect.Visible = isMC;
                cmbTF.Visible = isTF;
                txtOpenAns.Visible = isOpen;

                if (isTF)
                {
                    string saved = q.CorrectAnswer;
                    if (!string.IsNullOrEmpty(saved) && cmbTF.Items.Contains(saved))
                        cmbTF.SelectedItem = saved;
                    else cmbTF.SelectedIndex = 0;
                }

                if (isOpen) txtOpenAns.Text = q.CorrectAnswer;

                if (isMC) RebuildChoices();
                else
                {
                    pnlRight.Height = 72;
                    row.Height = 80 + pnlRight.Height + 12;
                    row.Invalidate();
                }
            }

            cmbTF.SelectedIndexChanged += (s, e) => q.CorrectAnswer = cmbTF.SelectedItem?.ToString() ?? "True";

            cmbQType.SelectedIndexChanged += (s, e) =>
            {
                string qt = cmbQType.SelectedItem?.ToString() ?? "MultipleChoice";
                ApplyQuestionType(qt);
            };

            row.Controls.AddRange(new Control[] { lblNum, cmbQType, lblPts, nudPts, btnDel, txtQ, pnlChoices, pnlRight });

            void PositionDeleteBtn() => btnDel.Location = new Point(row.Width - 36, 10);
            PositionDeleteBtn();
            row.SizeChanged += (s, e) =>
            {
                PositionDeleteBtn();
                txtQ.Width = row.Width - 20;
            };

            ApplyQuestionType(q.QuestionType);

            return row;
        }

        private static void RefreshCorrectAnswerCombo(QuizQuestion q, ComboBox cmb)
        {
            string saved = cmb.SelectedItem?.ToString() ?? q.CorrectAnswer;
            cmb.Items.Clear();
            for (int i = 0; i < q.Choices.Count; i++)
            {
                string letter = i < ChoiceLetters.Length ? ChoiceLetters[i] : $"#{i + 1}";
                string text = q.Choices[i].Trim();
                string display = string.IsNullOrEmpty(text) ? $"{letter}. (empty)" : $"{letter}. {text}";
                cmb.Items.Add(display);
            }
            // Restore selection by letter prefix
            for (int i = 0; i < cmb.Items.Count; i++)
            {
                if (cmb.Items[i].ToString()!.StartsWith(saved))
                { cmb.SelectedIndex = i; return; }
            }
            if (cmb.Items.Count > 0) cmb.SelectedIndex = 0;
        }

        private static string GetAnswerHint(string type) => type switch
        {
            "TrueFalse" => "Answer key: True or False",
            _ => "Answer key / model answer"
        };

        // ════════════════════════════════════════════════════════════════════
        //  RUBRIC SECTION CONSTANTS
        // ════════════════════════════════════════════════════════════════════
        // Summary panel height when rubric is enabled (fixed height so it never
        // grows/shrinks unpredictably and the section wrapper matches).
        private const int SummaryPanelH = 370;
        // Max scrollable height for the criteria list before it scrolls
        private const int CriteriaMaxH = SummaryPanelH;
        // Row height for each criteria item
        private const int CriteriaRowH = 58;

        // ── Toggle rubric on/off ─────────────────────────────────────────────
        private void chkRubric_CheckedChanged(object sender, EventArgs e)
        {
            bool on = chkRubric.Checked;

            // Show/hide criteria scroll and add-button
            pnlCriteriaScroll.Visible = on;
            btnAddCriteria.Visible = on;
            lblRubricNote.Visible = on;

            // Show/hide summary panel
            pnlRubricSummary.Visible = on;

            if (on)
            {
                RefreshRubricPanel();
                ApplyRubricLayout();
            }
            else
            {
                // Collapse to header-only height
                pnlRubricLeft.Height = 46;
                pnlRubricSummary.Height = 46;
                pnlRubricSection.Height = 46;
            }
        }

        private void btnAddCriteria_Click(object sender, EventArgs e)
        {
            _rubricItems.Add(new RubricCriteria { CriteriaId = _nextRId++, MaxPoints = 25 });
            RefreshRubricPanel();
        }

        // ── Refresh criteria list + update all summary widgets ───────────────
        private void RefreshRubricPanel()
        {
            // ── Rebuild criteria rows ────────────────────────────────────────
            flpRubric.SuspendLayout();
            flpRubric.Controls.Clear();
            int leftW = pnlCriteriaScroll.Width > 20 ? pnlCriteriaScroll.Width : 960;
            for (int i = 0; i < _rubricItems.Count; i++)
                flpRubric.Controls.Add(BuildRubricRow(_rubricItems[i], i + 1, leftW));
            flpRubric.ResumeLayout();

            // ── Compute stats ────────────────────────────────────────────────
            int count = _rubricItems.Count;
            int totalPts = _rubricItems.Sum(r => r.MaxPoints);
            int actPts = (int)nudPoints.Value;
            int remaining;

            if (chkRubric.Checked && count > 0)
            {
                nudPoints.Value = Math.Clamp(totalPts, 1, 10000);
                actPts = totalPts;
                remaining = 0;
                lblRubricNote.Text = $"Max score locked to rubric ({totalPts} pts)";
            }
            else
            {
                remaining = Math.Max(0, actPts - totalPts);
            }

            // ── Stat card values ────────────────────────────────────────────
            lblStatCriteriaVal.Text = count.ToString();
            lblStatTotalVal.Text = totalPts.ToString();
            lblStatRemainingVal.Text = remaining.ToString();

            // Remaining color
            bool balanced = remaining == 0 && count > 0;
            lblStatRemainingVal.ForeColor = balanced
                ? Color.FromArgb(26, 128, 64)
                : remaining == 0 ? Color.FromArgb(26, 128, 64)
                : Color.OrangeRed;

            // Status card
            (string statusText, Color statusColor, string statusIcon) = count switch
            {
                0 => ("Not\nConfigured", Color.FromArgb(130, 130, 140), "⚙"),
                1 => ("Incomplete\n(add more)", Color.FromArgb(200, 100, 0), "⚠"),
                _ when totalPts > 0 => ("✔  Ready", Color.FromArgb(26, 128, 64), "✔"),
                _ => ("⚠ Check pts", Color.OrangeRed, "⚠")
            };
            lblStatStatusVal.Text = statusText;
            lblStatStatusVal.ForeColor = statusColor;
            lblStatStatusIcon.Text = statusIcon;
            // Refresh the status card accent color
            pnlStatStatus.Tag = ColorTranslator.ToHtml(statusColor);
            pnlStatStatus.Invalidate();

            // ── Progress bar ────────────────────────────────────────────────
            int pct = actPts > 0 ? Math.Min(100, (int)Math.Round(totalPts * 100.0 / actPts)) : 0;
            int fillW = (int)(pnlProgressBg.Width * pct / 100.0);
            pnlProgressFill.Width = fillW;
            lblProgressPct.Text = $"{pct}%";
            pnlProgressFill.BackColor = pct >= 100
                ? Color.FromArgb(26, 128, 64)
                : pct >= 60 ? Color.FromArgb(255, 196, 0)
                : Color.FromArgb(128, 0, 0);

            // ── Validation message ──────────────────────────────────────────
            lblValidationMsg.Text = count == 0
                ? "⚠  Add at least one criterion to enable rubric grading."
                : count == 1
                ? "ℹ  Consider adding more criteria for balanced grading."
                : totalPts == 0
                ? "⚠  All criteria have 0 points — please set point values."
                : balanced
                ? "✔  Rubric is complete and ready to use."
                : $"ℹ  Rubric total: {totalPts} pts (activity max set to match).";
            lblValidationMsg.ForeColor = count == 0 || totalPts == 0
                ? Color.FromArgb(180, 60, 0)
                : balanced ? Color.FromArgb(26, 128, 64)
                : Color.FromArgb(100, 80, 0);

            // ── Resize layout ────────────────────────────────────────────────
            ApplyRubricLayout();
        }

        // ── Central layout engine for the rubric section ─────────────────────
        private void ApplyRubricLayout()
        {
            if (!chkRubric.Checked || pnlRubricSection == null) return;

            // Total section width
            int secW = Math.Max(900, pnlRubricSection.Width);
            int leftPad = 14;
            int gap = 12;

            // Left = ~70%, Right = ~30%, min 370px each
            int rightW = Math.Max(432, (int)(secW * 0.30));
            int leftW = Math.Max(460, secW - rightW - gap - leftPad);

            // ── Position left column ─────────────────────────────────────────
            pnlRubricLeft.Location = new Point(leftPad, 0);
            pnlRubricLeft.Width = leftW;
            pnlRubricHeader.Width = leftW;
            btnAddCriteria.Location = new Point(leftW - btnAddCriteria.Width - 4, 9);

            // Scrollable criteria area height: fits up to CriteriaMaxH
            int rowCount = _rubricItems.Count;
            int contentH = rowCount * (CriteriaRowH + 6) + 12;
            int criteriaH = rowCount == 0 ? 0 : Math.Min(contentH, CriteriaMaxH);

            pnlCriteriaScroll.Width = leftW;
            pnlCriteriaScroll.Height = criteriaH;
            flpRubric.Width = leftW - SystemInformation.VerticalScrollBarWidth - 2;

            // Resize criteria row widths if width changed
            foreach (Control c in flpRubric.Controls)
                if (c is Panel row) ResizeCriteriaRow(row, flpRubric.Width);

            int leftH = 46 + criteriaH;
            pnlRubricLeft.Height = leftH;

            // ── Position right column ────────────────────────────────────────
            pnlRubricSummary.Location = new Point(leftPad + leftW + gap, 0);
            pnlRubricSummary.Width = rightW;
            pnlRubricSummary.Height = SummaryPanelH;

            // Scale inner summary width items
            int innerW = rightW - 28;
            pnlSumDivider.Width = innerW;
            pnlStatCards.Width = innerW;
            pnlStatCardsRow2.Width = innerW;
            pnlProgressArea.Width = innerW;
            pnlGradeGuidelines.Width = innerW;
            pnlValidation.Width = innerW;

            // Stat card widths (2 per row, with 8px gap)
            int cardW = (innerW - 8) / 2;
            pnlStatCriteria.Width = cardW;
            pnlStatTotalPts.Width = cardW;
            pnlStatTotalPts.Left = cardW + 8;
            pnlStatRemaining.Width = cardW;
            pnlStatStatus.Width = cardW;
            pnlStatStatus.Left = cardW + 8;

            // Progress bar width
            int progW = innerW - lblProgressPct.Width - 8;
            pnlProgressBg.Width = Math.Max(60, progW);

            // Guideline panel text width
            lblGuidelineText.Width = innerW - 20;

            // ── Section outer height ─────────────────────────────────────────
            int sectionH = Math.Max(leftH, SummaryPanelH);
            pnlRubricSection.Height = sectionH;

            pnlRubricSection.Invalidate(true);
            pnlRubricSummary.Invalidate();
        }

        // ── Paint handler for the summary panel border + accent bar ──────────
        private void pnlRubricSummary_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Border
            using var borderPen = new Pen(Color.FromArgb(210, 200, 215), 1f);
            g.DrawRectangle(borderPen, 0, 0, pnlRubricSummary.Width - 1, pnlRubricSummary.Height - 1);

            // Left accent bar (maroon)
            using var accentBrush = new SolidBrush(Color.FromArgb(128, 0, 0));
            g.FillRectangle(accentBrush, 0, 0, 4, pnlRubricSummary.Height);
        }

        // ── Shared stat-card Paint: draws border + colored top accent bar ────
        private void StatCard_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            var panel = (Panel)sender;
            var g = e.Graphics;

            // Border
            using var pen = new Pen(Color.FromArgb(225, 220, 235), 1f);
            g.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);

            // Top accent bar using Tag color
            try
            {
                string hex = panel.Tag?.ToString() ?? "#888888";
                var c = ColorTranslator.FromHtml(hex);
                using var bar = new SolidBrush(Color.FromArgb(220, c.R, c.G, c.B));
                g.FillRectangle(bar, 1, 0, panel.Width - 2, 3);
            }
            catch { /* ignore bad hex */ }
        }

        // ── Build a single criteria row ───────────────────────────────────────
        private Panel BuildRubricRow(RubricCriteria r, int rowNum, int rowW)
        {
            rowW = Math.Max(300, rowW);
            var row = new Panel
            {
                Width = rowW,
                Height = CriteriaRowH,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 6),
                Tag = "criteria-row"
            };
            row.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(225, 218, 230), 1f);
                e.Graphics.DrawRectangle(pen, 0, 0, row.Width - 1, row.Height - 1);
                // Left accent
                using var acc = new SolidBrush(Color.FromArgb(128, 0, 0));
                e.Graphics.FillRectangle(acc, 0, 0, 3, row.Height);
            };

            // Row number badge
            var lblNum = new Label
            {
                Text = rowNum.ToString(),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(128, 0, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(8, (CriteriaRowH - 24) / 2),
                Size = new Size(24, 24),
                TabIndex = 0
            };

            // Criteria name
            var txtName = new TextBox
            {
                Text = r.Name,
                PlaceholderText = "Criteria name",
                Font = new Font("Segoe UI", 9.5F),
                Location = new Point(38, (CriteriaRowH - 26) / 2),
                Size = new Size(Math.Max(80, (int)(rowW * 0.32)), 26),
                TabIndex = 1
            };
            txtName.TextChanged += (s, e) => r.Name = txtName.Text;

            // Description
            int descX = txtName.Right + 8;
            var txtDesc = new TextBox
            {
                Text = r.Description,
                PlaceholderText = "Description (optional)",
                Font = new Font("Segoe UI", 9.5F),
                Location = new Point(descX, (CriteriaRowH - 26) / 2),
                Size = new Size(Math.Max(60, rowW - descX - 170), 26),
                TabIndex = 2
            };
            txtDesc.TextChanged += (s, e) => r.Description = txtDesc.Text;

            // Max pts label
            var lblPts = new Label
            {
                Text = "pts:",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(80, 80, 90),
                Location = new Point(rowW - 158, (CriteriaRowH - 18) / 2 + 2),
                AutoSize = true,
                TabIndex = 3
            };

            // Max pts numeric
            var nudPts = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 9999,
                Value = Math.Max(1, r.MaxPoints),
                Font = new Font("Segoe UI", 9F),
                Location = new Point(rowW - 132, (CriteriaRowH - 26) / 2),
                Size = new Size(80, 26),
                TabIndex = 4
            };
            nudPts.ValueChanged += (s, e) => { r.MaxPoints = (int)nudPts.Value; RefreshRubricPanel(); };

            // Delete button
            var btnDel = new buttonRounded
            {
                Text = "✕",
                Size = new Size(28, 28),
                Location = new Point(rowW - 40, (CriteriaRowH - 28) / 2),
                BackColor = Color.FromArgb(195, 55, 55),
                ForeColor = Color.White,
                BorderRadius = 6,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TabIndex = 5
            };
            btnDel.FlatAppearance.BorderSize = 0;
            btnDel.Click += (s, e) => { _rubricItems.Remove(r); RefreshRubricPanel(); };

            row.Controls.AddRange(new Control[] { lblNum, txtName, txtDesc, lblPts, nudPts, btnDel });
            return row;
        }

        // ── Resize a criteria row's fluid controls ────────────────────────────
        private static void ResizeCriteriaRow(Panel row, int newRowW)
        {
            if (newRowW < 200) return;
            foreach (Control c in row.Controls)
            {
                if (c is TextBox tb)
                {
                    if (tb.TabIndex == 1) // name
                        tb.Width = Math.Max(80, (int)(newRowW * 0.32));
                    else if (tb.TabIndex == 2) // description
                    {
                        int descX = row.Controls.OfType<TextBox>()
                                       .FirstOrDefault(t => t.TabIndex == 1)?.Right + 8 ?? 200;
                        tb.Location = new Point(descX, tb.Top);
                        tb.Width = Math.Max(60, newRowW - descX - 170);
                    }
                }
                else if (c is Label lbl && lbl.TabIndex == 3)
                    lbl.Left = newRowW - 158;
                else if (c is NumericUpDown nud)
                    nud.Left = newRowW - 132;
                else if (c is buttonRounded btn)
                    btn.Left = newRowW - 40;
            }
            row.Width = newRowW;
        }

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

        private void pnlHeader_SizeChanged(object sender, System.EventArgs e)
        {
            if (btnSave != null && pnlHeader != null)
                btnSave.Location = new Point(pnlHeader.Width - btnSave.Width - 12, 17);
        }

        private void pnlRubricSection_SizeChanged(object sender, System.EventArgs e)
        {
            if (chkRubric?.Checked == true)
                ApplyRubricLayout();
        }

        private void pnlScroll_SizeChanged(object sender, System.EventArgs e)
        {
            if (pnlScroll == null || stackPanel == null) return;
            int w = Math.Max(700, pnlScroll.ClientSize.Width - 44);
            foreach (Control c in stackPanel.Controls)
            {
                c.Width = w;
                if (c is Panel p)
                    foreach (Control inner in p.Controls)
                        if (inner is TextBox tb && (tb.Name == "txtTitle" || tb.Name == "txtDescription"))
                            tb.Width = w - 36;
            }
            if (chkRubric?.Checked == true)
                ApplyRubricLayout();
        }

        private static void SetupSectionLabel(System.Windows.Forms.Label lbl, string text, int x, int y, System.Drawing.Color color)
        {
            lbl.AutoSize = true;
            lbl.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            lbl.ForeColor = color;
            lbl.Location = new System.Drawing.Point(x, y);
            lbl.Text = text;
            lbl.TabIndex = 0;
        }

        private static void SetupFieldLabel(System.Windows.Forms.Label lbl, string text, int x, int y)
        {
            lbl.AutoSize = true;
            lbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            lbl.Location = new System.Drawing.Point(x, y);
            lbl.Text = text;
        }

        private void btnCancel_Click(object sender, EventArgs e) => OnCancel?.Invoke();

        private void pnlQuizSection_SizeChanged(object sender, System.EventArgs e) { }
        private void pnlFilesSection_SizeChanged(object sender, System.EventArgs e) { }

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