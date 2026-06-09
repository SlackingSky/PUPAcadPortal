using PUPAcadPortal.PortalContents.Instructor.LMS.Course;
using PUPAcadPortal.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class GradingInterface : UserControl
    {
        // ── Public events ─────────────────────────────────────
        public event Action OnBack;

        // ── State ──────────────────────────────────────────────
        private StudentSubmission _current;
        private readonly ActivityItem _activity;
        private readonly List<StudentSubmission> _students;
        private readonly IActivityDbService? _svc;
        private int _index;

        // Right-column shared controls (all types)
        private TextBox _txtScore;
        private Label _lblScoreOf;
        private TextBox _txtRemarks;
        private Label _lblSaveStatus;
        private buttonRounded _btnSave;
        private readonly List<(RubricCriteria criteria, NumericUpDown nud)> _rubricRows = new();
        private CheckBox _chkAutoScore;
        private Label _lblRubricTotal;

        // Left-column dynamic content panel
        private Panel _pnlContent;

        // Auto-save timer
        private System.Windows.Forms.Timer _autoSaveTimer;

        // ── Colors ─────────────────────────────────────────────
        private static readonly Color Maroon = Color.FromArgb(128, 0, 0);
        private static readonly Color MaroonDark = Color.FromArgb(100, 0, 0);
        private static readonly Color LightBg = Color.FromArgb(247, 247, 250);
        private static readonly Color PanelBorder = Color.FromArgb(220, 218, 225);
        private static readonly Color CorrectGreen = Color.FromArgb(26, 128, 64);
        private static readonly Color WrongRed = Color.FromArgb(185, 50, 50);

        // ── Constructor (DB-backed) ────────────────────────────
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
            BuildTypeAwareLayout();
            SetupAutoSave();
            UpdateNavButtons();
        }

        // ── Backward-compatible constructor ───────────────────
        public GradingInterface(
            StudentSubmission submission,
            ActivityItem activity,
            List<StudentSubmission> allStudents = null,
            int index = 0)
            : this(submission, activity, allStudents, index, null) { }

        // ══════════════════════════════════════════════════════
        //  TOP-LEVEL LAYOUT BUILDER
        //  Replaces the old static Designer layout with a
        //  fully dynamic two-column layout rebuilt on every
        //  student-navigation action.
        // ══════════════════════════════════════════════════════
        private void BuildTypeAwareLayout()
        {
            // Header labels (declared in Designer, just fill them)
            lblActivityTitle.Text = _activity.Title;
            lblMaxPoints.Text = $"Max: {_activity.Points} pts";

            // Build the shared right-column grading panel
            BuildRightColumn();

            // Build left content panel based on type
            BuildLeftColumn();

            // Load student data into both columns
            LoadStudent();
        }

        // ══════════════════════════════════════════════════════
        //  RIGHT COLUMN — Score input, rubric, remarks
        //  (shared by all activity types)
        // ══════════════════════════════════════════════════════
        private void BuildRightColumn()
        {
            pnlGrading.Controls.Clear();
            _rubricRows.Clear();

            int y = 10;

            // ── "Grading" header ────────────────────────────
            var lblGH = new Label
            {
                Text = "🎯  Grading",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Maroon,
                Location = new Point(12, y),
                AutoSize = true,
                TabIndex = 0
            };
            pnlGrading.Controls.Add(lblGH);
            y += 28;

            // ── Save status ──────────────────────────────────
            _lblSaveStatus = new Label
            {
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                ForeColor = Color.ForestGreen,
                Location = new Point(12, y),
                Size = new Size(420, 18),
                TabIndex = 1
            };
            pnlGrading.Controls.Add(_lblSaveStatus);
            y += 24;

            // ── Rubric section (quiz skips this) ─────────────
            bool isQuiz = _activity.Type == ActivityType.Quiz;

            if (!isQuiz)
            {
                y = BuildRubricSection(y);
            }

            // ── Score input row ───────────────────────────────
            var pnlScore = new Panel
            {
                BackColor = Color.White,
                Location = new Point(12, y),
                Size = new Size(420, 90),
                TabIndex = 10
            };
            pnlScore.Paint += (s, e) =>
            {
                using var pen = new Pen(PanelBorder);
                e.Graphics.DrawRectangle(pen, 0, 0, pnlScore.Width - 1, pnlScore.Height - 1);
            };

            var lblSL = new Label
            {
                Text = "Final Score:",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 35, 45),
                Location = new Point(4, 12),
                Size = new Size(115, 24)
            };

            _txtScore = new TextBox
            {
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                Location = new Point(122, 8),
                MaxLength = 5,
                Size = new Size(80, 34),
                TextAlign = HorizontalAlignment.Center
            };

            _lblScoreOf = new Label
            {
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.Gray,
                Location = new Point(210, 14),
                Size = new Size(100, 22),
                Text = $"/ {_activity.Points}"
            };

            _btnSave = new buttonRounded
            {
                BackColor = Maroon,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(4, 54),
                Name = "btnSaveScore",
                Size = new Size(170, 32),
                Text = "✔  Save and Lock Score",
                UseVisualStyleBackColor = false
            };
            _btnSave.FlatAppearance.BorderSize = 0;
            _btnSave.Click += btnSaveScore_Click;

            pnlScore.Controls.AddRange(new Control[] { lblSL, _txtScore, _lblScoreOf, _btnSave });
            pnlGrading.Controls.Add(pnlScore);
            y += 100;

            // ── Remarks / Feedback ────────────────────────────
            var pnlRem = new Panel
            {
                BackColor = Color.White,
                Location = new Point(12, y),
                Size = new Size(420, 240),
                TabIndex = 11
            };

            var lblRH = new Label
            {
                Text = "💬  Remarks / Feedback",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 70, 80),
                Location = new Point(4, 4),
                Size = new Size(260, 22)
            };

            _txtRemarks = new TextBox
            {
                Font = new Font("Segoe UI", 9.5F),
                Location = new Point(0, 30),
                Multiline = true,
                PlaceholderText = "Add comments or feedback for the student...",
                ScrollBars = ScrollBars.Vertical,
                Size = new Size(418, 205)
            };

            pnlRem.Controls.AddRange(new Control[] { lblRH, _txtRemarks });
            pnlGrading.Controls.Add(pnlRem);
        }

        private int BuildRubricSection(int startY)
        {
            int y = startY;

            var items = _activity.RubricItems.Count > 0
                ? _activity.RubricItems
                : new List<RubricCriteria>
                {
                    new RubricCriteria { CriteriaId = 1, Name = "Content",   MaxPoints = 25 },
                    new RubricCriteria { CriteriaId = 2, Name = "Structure", MaxPoints = 25 },
                    new RubricCriteria { CriteriaId = 3, Name = "Grammar",   MaxPoints = 25 },
                    new RubricCriteria { CriteriaId = 4, Name = "Relevance", MaxPoints = 25 }
                };

            // Container
            var pnlBox = new Panel
            {
                BackColor = Color.FromArgb(251, 249, 249),
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(12, y),
                Size = new Size(420, 40 + items.Count * 40 + 60),
                TabIndex = 5
            };

            var lblRH = new Label
            {
                Text = "📊  Rubric-Based Grading",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 70, 80),
                Location = new Point(10, 8),
                Size = new Size(240, 20)
            };
            pnlBox.Controls.Add(lblRH);

            var flp = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown,
                Location = new Point(10, 34),
                WrapContents = false,
                BackColor = Color.Transparent
            };

            foreach (var crit in items)
            {
                var row = BuildRubricRow(crit, out var nud);
                _rubricRows.Add((crit, nud));
                flp.Controls.Add(row);
            }
            pnlBox.Controls.Add(flp);

            _lblRubricTotal = new Label
            {
                Text = $"Rubric Total: 0 / {items.Sum(r => r.MaxPoints)}",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Maroon,
                Location = new Point(10, pnlBox.Height - 48),
                Size = new Size(280, 22)
            };
            pnlBox.Controls.Add(_lblRubricTotal);

            _chkAutoScore = new CheckBox
            {
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(60, 60, 70),
                Location = new Point(10, pnlBox.Height - 26),
                Size = new Size(260, 22),
                Text = "Auto-fill score from rubric total",
                TabIndex = 1
            };
            _chkAutoScore.CheckedChanged += chkAutoScore_CheckedChanged;
            pnlBox.Controls.Add(_chkAutoScore);

            pnlGrading.Controls.Add(pnlBox);
            y += pnlBox.Height + 10;
            return y;
        }

        private Panel BuildRubricRow(RubricCriteria crit, out NumericUpDown nud)
        {
            var row = new Panel { Width = 400, Height = 34, BackColor = Color.Transparent };

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

            var lblMax = new Label
            {
                Text = $"/ {crit.MaxPoints}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(186, 9),
                Size = new Size(50, 18)
            };

            var bar = new Panel { Location = new Point(240, 12), Size = new Size(150, 10), BackColor = Color.FromArgb(235, 235, 240) };
            var fill = new Panel { Location = new Point(0, 0), Size = new Size(0, 10), BackColor = Maroon };
            bar.Controls.Add(fill);

            var nudCap = nud;
            nudCap.ValueChanged += (s, e) =>
            {
                int pct = crit.MaxPoints > 0 ? (int)((double)nudCap.Value / crit.MaxPoints * 150) : 0;
                fill.Width = Math.Clamp(pct, 0, 150);
                UpdateRubricTotal();
            };

            row.Controls.AddRange(new Control[] { lbl, nudCap, lblMax, bar });
            return row;
        }

        // ══════════════════════════════════════════════════════
        //  LEFT COLUMN — type-specific content
        // ══════════════════════════════════════════════════════
        private void BuildLeftColumn()
        {
            // Remove old content panel if exists
            pnlEssay.Controls.Clear();

            switch (_activity.Type)
            {
                case ActivityType.Quiz:
                    BuildQuizContent();
                    break;
                case ActivityType.Essay:
                    BuildEssayContent();
                    break;
                case ActivityType.FileUpload:
                    BuildFileUploadContent();
                    break;
                default: // Assignment
                    BuildAssignmentContent();
                    break;
            }
        }

        // ──────────────────────────────────────────────────────
        //  QUIZ — show each question, student answer vs correct
        // ──────────────────────────────────────────────────────
        private void BuildQuizContent()
        {
            pnlEssay.AutoScroll = true;

            // Header
            var hdr = new Label
            {
                Text = "❓  Quiz Answers Review",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Maroon,
                Location = new Point(12, 10),
                AutoSize = true
            };
            pnlEssay.Controls.Add(hdr);

            // Legend
            var legendPanel = new Panel
            {
                Location = new Point(12, 36),
                Size = new Size(500, 24),
                BackColor = Color.Transparent
            };
            legendPanel.Controls.Add(new Label { Text = "▐ Correct", Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = CorrectGreen, Location = new Point(0, 4), AutoSize = true });
            legendPanel.Controls.Add(new Label { Text = "▐ Wrong", Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = WrongRed, Location = new Point(90, 4), AutoSize = true });
            legendPanel.Controls.Add(new Label { Text = "▐ Not answered", Font = new Font("Segoe UI", 8.5F), ForeColor = Color.Gray, Location = new Point(160, 4), AutoSize = true });
            pnlEssay.Controls.Add(legendPanel);

            if (_activity.Questions == null || _activity.Questions.Count == 0)
            {
                pnlEssay.Controls.Add(new Label
                {
                    Text = "No quiz questions are configured for this activity.",
                    Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    Location = new Point(12, 70),
                    AutoSize = true
                });
                return;
            }

            // ── Parse stored answers from EssayContent (JSON-key:value lines)
            var storedAnswers = ParseQuizAnswers(_current.EssayContent);

            // Scroll container
            var flp = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown,
                Location = new Point(12, 68),
                Width = pnlEssay.Width - 30,
                WrapContents = false,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            pnlEssay.Controls.Add(flp);

            int autoScore = 0;
            int totalPoints = 0;

            for (int qi = 0; qi < _activity.Questions.Count; qi++)
            {
                var q = _activity.Questions[qi];
                totalPoints += q.Points;
                string studentAnswer = storedAnswers.TryGetValue(q.QuestionId, out var sa) ? sa : "";
                bool answered = !string.IsNullOrWhiteSpace(studentAnswer);
                bool isCorrect = answered &&
                    string.Equals(studentAnswer.Trim(), q.CorrectAnswer.Trim(),
                                  StringComparison.OrdinalIgnoreCase);

                if (isCorrect) autoScore += q.Points;

                Color cardAccent = !answered ? Color.FromArgb(180, 180, 180)
                                 : isCorrect ? CorrectGreen
                                 : WrongRed;

                var card = new Panel
                {
                    Width = flp.Width - 8,
                    BackColor = Color.White,
                    Margin = new Padding(0, 0, 0, 8),
                    AutoSize = false
                };
                card.Paint += (s, e) =>
                {
                    using var pen = new Pen(PanelBorder);
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                    using var acc = new SolidBrush(cardAccent);
                    e.Graphics.FillRectangle(acc, 0, 0, 5, card.Height);
                };

                int cy = 8;

                // Question header row
                var pnlQH = new Panel { Location = new Point(6, cy), BackColor = Color.Transparent, Size = new Size(card.Width - 12, 24) };
                pnlQH.Controls.Add(new Label
                {
                    Text = $"Q{qi + 1}",
                    BackColor = Maroon,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    Size = new Size(26, 20),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(0, 2)
                });
                pnlQH.Controls.Add(new Label
                {
                    Text = q.QuestionText,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(20, 20, 25),
                    Location = new Point(32, 4),
                    MaximumSize = new Size(card.Width - 120, 0),
                    AutoSize = true
                });
                // Points badge
                pnlQH.Controls.Add(new Label
                {
                    Text = $"{q.Points} pt{(q.Points != 1 ? "s" : "")}",
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    ForeColor = Maroon,
                    Location = new Point(pnlQH.Width - 52, 4),
                    Size = new Size(50, 18),
                    TextAlign = ContentAlignment.MiddleRight,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                });
                card.Controls.Add(pnlQH);
                cy += 28;

                // Divider
                card.Controls.Add(new Panel { Location = new Point(6, cy), BackColor = PanelBorder, Size = new Size(card.Width - 12, 1) });
                cy += 6;

                // Answer comparison row
                var pnlAns = new Panel { Location = new Point(6, cy), BackColor = Color.Transparent, Width = card.Width - 12 };

                // Student answer
                string saDisplay = answered ? studentAnswer : "(no answer)";
                Color saColor = !answered ? Color.Gray : isCorrect ? CorrectGreen : WrongRed;
                string saIcon = !answered ? "—" : isCorrect ? "✔" : "✗";

                pnlAns.Controls.Add(new Label
                {
                    Text = "Student:",
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(80, 80, 90),
                    Location = new Point(0, 2),
                    Size = new Size(68, 18)
                });
                pnlAns.Controls.Add(new Label
                {
                    Text = $"{saIcon}  {saDisplay}",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = saColor,
                    Location = new Point(72, 0),
                    MaximumSize = new Size(card.Width / 2 - 80, 0),
                    AutoSize = true
                });

                // Correct answer
                pnlAns.Controls.Add(new Label
                {
                    Text = "Correct:",
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(80, 80, 90),
                    Location = new Point(card.Width / 2, 2),
                    Size = new Size(62, 18)
                });
                pnlAns.Controls.Add(new Label
                {
                    Text = q.CorrectAnswer,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = CorrectGreen,
                    Location = new Point(card.Width / 2 + 66, 0),
                    MaximumSize = new Size(card.Width / 2 - 80, 0),
                    AutoSize = true
                });

                pnlAns.Height = 24;
                card.Controls.Add(pnlAns);
                cy += 30;

                card.Height = cy + 6;
                flp.Controls.Add(card);
            }

            // Auto-score summary bar
            int pct = totalPoints > 0 ? (int)Math.Round((double)autoScore / totalPoints * _activity.Points) : 0;

            var pnlSummary = new Panel
            {
                Width = flp.Width - 8,
                Height = 48,
                BackColor = Color.FromArgb(244, 248, 255),
                Margin = new Padding(0, 4, 0, 0)
            };
            pnlSummary.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(190, 210, 240));
                e.Graphics.DrawRectangle(pen, 0, 0, pnlSummary.Width - 1, pnlSummary.Height - 1);
            };
            pnlSummary.Controls.Add(new Label
            {
                Text = $"Auto-calculated score:  {pct} / {_activity.Points} pts",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Maroon,
                Location = new Point(12, 14),
                AutoSize = true
            });
            pnlSummary.Controls.Add(new Label
            {
                Text = $"({autoScore} / {totalPoints} quiz pts correct)",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(80, 80, 90),
                Location = new Point(300, 16),
                AutoSize = true
            });
            // Auto-fill button
            var btnAutoFill = new buttonRounded
            {
                Text = "⟶ Auto-fill Score",
                Size = new Size(140, 28),
                BackColor = Color.FromArgb(63, 81, 181),
                ForeColor = Color.White,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Location = new Point(pnlSummary.Width - 158, 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnAutoFill.Click += (s, e) =>
            {
                if (_txtScore != null && !_current.IsChecked)
                    _txtScore.Text = pct.ToString();
            };
            pnlSummary.Controls.Add(btnAutoFill);
            flp.Controls.Add(pnlSummary);

            // Pre-fill score box with auto-calculated value if not yet graded
            if (!_current.IsChecked && _txtScore != null)
                _txtScore.Text = pct.ToString();
        }

        // ──────────────────────────────────────────────────────
        //  ESSAY — display submitted essay text
        // ──────────────────────────────────────────────────────
        private void BuildEssayContent()
        {
            pnlEssay.AutoScroll = false;

            var hdr = new Label
            {
                Text = "📄  Submission Content",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Maroon,
                Location = new Point(12, 10),
                AutoSize = true
            };
            pnlEssay.Controls.Add(hdr);

            var txtContent = new TextBox
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10F),
                Location = new Point(12, 40),
                Multiline = true,
                Name = "txtEssayContent",
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Size = new Size(pnlEssay.Width - 24, pnlEssay.Height - 90),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                Text = _current.EssayContent
            };
            pnlEssay.Controls.Add(txtContent);

            // Word / char count
            var pnlWC = new Panel
            {
                BackColor = Color.FromArgb(245, 245, 248),
                Dock = DockStyle.Bottom,
                Height = 24
            };
            pnlWC.Controls.Add(new Label
            {
                Text = GetWordCharCount(_current.EssayContent),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(12, 4),
                AutoSize = true
            });
            pnlEssay.Controls.Add(pnlWC);
        }

        // ──────────────────────────────────────────────────────
        //  ASSIGNMENT — submitted file info + essay notes
        // ──────────────────────────────────────────────────────
        private void BuildAssignmentContent()
        {
            pnlEssay.AutoScroll = true;

            var hdr = new Label
            {
                Text = "📋  Submitted Assignment",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Maroon,
                Location = new Point(12, 10),
                AutoSize = true
            };
            pnlEssay.Controls.Add(hdr);

            int y = 40;

            // Status / time row
            bool hasSubmission = !string.IsNullOrEmpty(_current.EssayContent)
                                 || _current.HasFile
                                 || _current.SubmissionTime != DateTime.MinValue;

            Color statusColor = _current.Status switch
            {
                "Submitted" => CorrectGreen,
                "Late" => Color.OrangeRed,
                _ => Color.Gray
            };
            var lblStatus = new Label
            {
                Text = $"● {_current.Status}  ·  Submitted: {(_current.SubmissionTime != DateTime.MinValue ? _current.SubmissionTime.ToString("MMM dd, yyyy  hh:mm tt") : "—")}",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = statusColor,
                Location = new Point(12, y),
                AutoSize = true
            };
            pnlEssay.Controls.Add(lblStatus);
            y += 28;

            if (_current.HasFile)
            {
                // File attached panel
                var pnlFile = BuildFileCard(_current.EssayContent, y);
                pnlEssay.Controls.Add(pnlFile);
                y += pnlFile.Height + 12;
            }

            if (!string.IsNullOrEmpty(_current.EssayContent) && !_current.HasFile)
            {
                // Text-based submission
                pnlEssay.Controls.Add(new Label
                {
                    Text = "Submission Text:",
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(50, 50, 60),
                    Location = new Point(12, y),
                    AutoSize = true
                });
                y += 22;

                var txt = new TextBox
                {
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Font = new Font("Segoe UI", 10F),
                    Location = new Point(12, y),
                    Multiline = true,
                    ReadOnly = true,
                    ScrollBars = ScrollBars.Vertical,
                    Size = new Size(pnlEssay.Width - 30, 300),
                    Text = _current.EssayContent,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                };
                pnlEssay.Controls.Add(txt);
                y += 310;
            }

            if (!hasSubmission)
            {
                pnlEssay.Controls.Add(new Label
                {
                    Text = "No content submitted for this assignment.",
                    Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    Location = new Point(12, y),
                    AutoSize = true
                });
            }
        }

        // ──────────────────────────────────────────────────────
        //  FILE UPLOAD — download card + direct score
        // ──────────────────────────────────────────────────────
        private void BuildFileUploadContent()
        {
            pnlEssay.AutoScroll = true;

            var hdr = new Label
            {
                Text = "📎  Submitted File",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Maroon,
                Location = new Point(12, 10),
                AutoSize = true
            };
            pnlEssay.Controls.Add(hdr);

            int y = 40;

            // Status badge
            Color sc = _current.Status switch { "Submitted" => CorrectGreen, "Late" => Color.OrangeRed, _ => Color.Gray };
            pnlEssay.Controls.Add(new Label
            {
                Text = $"● {_current.Status}  ·  {(_current.SubmissionTime != DateTime.MinValue ? _current.SubmissionTime.ToString("MMM dd, yyyy  hh:mm tt") : "—")}",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = sc,
                Location = new Point(12, y),
                AutoSize = true
            });
            y += 30;

            if (_current.HasFile && !string.IsNullOrEmpty(_current.EssayContent))
            {
                var pnlFile = BuildFileCard(_current.EssayContent, y);
                pnlEssay.Controls.Add(pnlFile);
                y += pnlFile.Height + 12;
            }
            else
            {
                pnlEssay.Controls.Add(new Label
                {
                    Text = "No file was attached to this submission.",
                    Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    Location = new Point(12, y),
                    AutoSize = true
                });
            }
        }

        // ──────────────────────────────────────────────────────
        //  SHARED HELPERS
        // ──────────────────────────────────────────────────────

        private Panel BuildFileCard(string fileUrl, int y)
        {
            bool isUrl = !string.IsNullOrWhiteSpace(fileUrl) &&
                         (fileUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                          fileUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase));

            string fileName = isUrl
                ? System.IO.Path.GetFileName(new Uri(fileUrl).LocalPath)
                : fileUrl;
            if (string.IsNullOrWhiteSpace(fileName)) fileName = "submission_file";

            string ext = System.IO.Path.GetExtension(fileName).ToUpper().TrimStart('.');
            string icon = ext switch
            {
                "PDF" => "📄",
                "DOCX" or "DOC" => "📝",
                "PPTX" or "PPT" => "📊",
                "XLSX" or "XLS" => "📊",
                "PNG" or "JPG" or "JPEG" => "🖼",
                "ZIP" or "RAR" => "🗜",
                _ => "📎"
            };

            var card = new Panel
            {
                BackColor = Color.White,
                Location = new Point(12, y),
                Size = new Size(pnlEssay.Width - 30, 64),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            card.Paint += (s, e) =>
            {
                using var pen = new Pen(PanelBorder);
                e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                using var acc = new SolidBrush(Color.FromArgb(63, 81, 181));
                e.Graphics.FillRectangle(acc, 0, 0, 5, card.Height);
            };

            card.Controls.Add(new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 20F),
                Location = new Point(14, 14),
                AutoSize = true
            });
            card.Controls.Add(new Label
            {
                Text = fileName,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 50, 160),
                Location = new Point(56, 10),
                Size = new Size(card.Width - 200, 20),
                AutoEllipsis = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            });
            card.Controls.Add(new Label
            {
                Text = isUrl ? "Cloudinary — click Download to save" : fileUrl,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.Gray,
                Location = new Point(56, 30),
                Size = new Size(card.Width - 200, 18),
                AutoEllipsis = true
            });

            var btnDl = new buttonRounded
            {
                Text = "↓ Download",
                Size = new Size(108, 30),
                Location = new Point(card.Width - 120, 17),
                BackColor = isUrl ? Color.FromArgb(34, 139, 34) : Color.FromArgb(170, 170, 180),
                ForeColor = Color.White,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Enabled = isUrl,
                Cursor = isUrl ? Cursors.Hand : Cursors.Default,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnDl.Click += (s, e) =>
            {
                if (!isUrl) return;
                try
                {
                    using var sfd = new SaveFileDialog
                    {
                        Title = "Save Submission As",
                        FileName = fileName,
                        Filter = "All Files (*.*)|*.*"
                    };
                    if (sfd.ShowDialog() != DialogResult.OK) return;

                    string tmp = CloudinaryService.Instance.DownloadToTemp(fileUrl, fileName);
                    System.IO.File.Copy(tmp, sfd.FileName, overwrite: true);
                    try { System.IO.File.Delete(tmp); } catch { }

                    MessageBox.Show($"Saved to:\n{sfd.FileName}", "Download Complete",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Download failed:\n{ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            card.Controls.Add(btnDl);
            return card;
        }

        private static Dictionary<int, string> ParseQuizAnswers(string raw)
        {
            var dict = new Dictionary<int, string>();
            if (string.IsNullOrWhiteSpace(raw)) return dict;

            // Try JSON format: {"1":"answer","2":"answer"}
            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(raw);
                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    if (int.TryParse(prop.Name, out int k))
                        dict[k] = prop.Value.GetString() ?? "";
                }
                return dict;
            }
            catch { }

            // Fallback: plain text "Q1: answer\nQ2: answer"
            foreach (var line in raw.Split('\n'))
            {
                var parts = line.Split(':', 2);
                if (parts.Length == 2)
                {
                    string key = parts[0].Trim().TrimStart('Q', 'q');
                    if (int.TryParse(key, out int idx))
                        dict[idx] = parts[1].Trim();
                }
            }
            return dict;
        }

        private static string GetWordCharCount(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "Words: 0  |  Characters: 0";
            int words = text.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
            return $"Words: {words}  |  Characters: {text.Length}";
        }

        // ══════════════════════════════════════════════════════
        //  STUDENT DATA LOADING (fills shared right column)
        // ══════════════════════════════════════════════════════
        private void LoadStudent()
        {
            lblStudentName.Text = _current.StudentName;
            lblStudentId.Text = _current.StudentId;
            lblSubmissionTime.Text = _current.SubmissionTime == DateTime.MinValue
                ? "Not submitted"
                : $"Submitted: {_current.SubmissionTime:MMM dd, yyyy  hh:mm tt}";

            if (_txtRemarks != null) _txtRemarks.Text = _current.Remarks;
            if (_txtScore != null) _txtScore.Text = _current.Score >= 0 ? _current.Score.ToString() : "";
            if (_lblScoreOf != null) _lblScoreOf.Text = $"/ {_activity.Points}";

            bool locked = _current.IsChecked;

            if (_txtScore != null) _txtScore.ReadOnly = locked;
            if (_btnSave != null) _btnSave.Enabled = !locked;
            if (_txtRemarks != null) _txtRemarks.ReadOnly = locked;

            foreach (var (crit, nud) in _rubricRows)
            {
                nud.Value = _current.RubricScores.TryGetValue(crit.CriteriaId, out int v)
                              ? Math.Min(v, crit.MaxPoints) : 0;
                nud.Enabled = !locked;
            }

            if (_chkAutoScore != null) _chkAutoScore.Enabled = !locked;
            if (_lblSaveStatus != null)
            {
                _lblSaveStatus.Text = locked ? "✅ Already checked – score locked" : "";
                _lblSaveStatus.ForeColor = locked ? Maroon : Color.ForestGreen;
            }

            UpdateRubricTotal();
        }

        // ══════════════════════════════════════════════════════
        //  AUTO-SAVE & RUBRIC HELPERS
        // ══════════════════════════════════════════════════════
        private void SetupAutoSave()
        {
            _autoSaveTimer = new System.Windows.Forms.Timer { Interval = 30_000 };
            _autoSaveTimer.Tick += (s, e) =>
            {
                if (_txtScore == null || !int.TryParse(_txtScore.Text, out int sc)) return;
                _current.Score = sc;
                _current.Remarks = _txtRemarks?.Text ?? "";
                TrySaveGradeToDb(sc);
                if (_lblSaveStatus != null)
                {
                    _lblSaveStatus.Text = $"Auto-saved at {DateTime.Now:hh:mm tt}";
                    _lblSaveStatus.ForeColor = Color.DimGray;
                }
            };
            _autoSaveTimer.Start();
        }

        private void UpdateRubricTotal()
        {
            if (_rubricRows.Count == 0 || _lblRubricTotal == null) return;

            int rubricMax = _rubricRows.Sum(r => (int)r.criteria.MaxPoints);
            int total = _rubricRows.Sum(r => (int)r.nud.Value);
            _lblRubricTotal.Text = $"Rubric Total: {total} / {rubricMax}";

            if (_chkAutoScore?.Checked == true && _txtScore != null && !_current.IsChecked)
            {
                double pct = rubricMax > 0 ? (double)total / rubricMax : 0;
                _txtScore.Text = ((int)Math.Round(pct * _activity.Points)).ToString();
            }
        }

        private void chkAutoScore_CheckedChanged(object sender, EventArgs e)
        {
            if (_txtScore != null)
                _txtScore.ReadOnly = (_chkAutoScore?.Checked == true) || _current.IsChecked;
            UpdateRubricTotal();
        }

        // ══════════════════════════════════════════════════════
        //  SAVE SCORE (lock)
        // ══════════════════════════════════════════════════════
        private void btnSaveScore_Click(object sender, EventArgs e)
        {
            if (_txtScore == null || !int.TryParse(_txtScore.Text, out int score))
            {
                MessageBox.Show("Please enter a valid numeric score.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (score < 0 || score > _activity.Points)
            {
                MessageBox.Show($"Score must be between 0 and {_activity.Points}.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var (crit, nud) in _rubricRows)
                _current.RubricScores[crit.CriteriaId] = (int)nud.Value;

            _current.Score = score;
            _current.Remarks = _txtRemarks?.Text ?? "";

            if (!TrySaveGradeToDb(score)) return;

            // Lock UI
            _current.IsChecked = true;
            if (_txtScore != null) { _txtScore.ReadOnly = true; }
            if (_btnSave != null) { _btnSave.Enabled = false; }
            if (_txtRemarks != null) { _txtRemarks.ReadOnly = true; }
            foreach (var (_, nud) in _rubricRows) nud.Enabled = false;
            if (_chkAutoScore != null) _chkAutoScore.Enabled = false;

            if (_lblSaveStatus != null)
            {
                _lblSaveStatus.Text = $"✅ Saved at {DateTime.Now:hh:mm tt}";
                _lblSaveStatus.ForeColor = Color.FromArgb(46, 160, 67);
            }

            MessageBox.Show($"Score saved: {score}/{_activity.Points}",
                "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool TrySaveGradeToDb(int score)
        {
            if (_svc == null || string.IsNullOrEmpty(_current.SubmissionDbId)) return true;
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

        // ══════════════════════════════════════════════════════
        //  STUDENT NAVIGATION
        // ══════════════════════════════════════════════════════
        private void btnNextStudent_Click(object sender, EventArgs e)
        {
            if (_index >= _students.Count - 1) return;
            _current = _students[++_index];
            RefreshForNewStudent();
        }

        private void btnPrevStudent_Click(object sender, EventArgs e)
        {
            if (_index <= 0) return;
            _current = _students[--_index];
            RefreshForNewStudent();
        }

        private void RefreshForNewStudent()
        {
            // Rebuild left column (quiz answers differ per student)
            BuildLeftColumn();
            // Reload right column values
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