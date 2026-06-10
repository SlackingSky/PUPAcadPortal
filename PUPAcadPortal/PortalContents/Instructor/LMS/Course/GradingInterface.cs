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
        //  Public events 
        public event Action OnBack;

        //  State 
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

        // Auto-save timer
        private System.Windows.Forms.Timer _autoSaveTimer;

        //  Colors 
        private static readonly Color Maroon = Color.FromArgb(128, 0, 0);
        private static readonly Color PanelBorder = Color.FromArgb(220, 218, 225);
        private static readonly Color CorrectGreen = Color.FromArgb(26, 128, 64);
        private static readonly Color WrongRed = Color.FromArgb(185, 50, 50);
        private static readonly Color QuizBlue = Color.FromArgb(63, 81, 181);

        //  Constructor (DB-backed) 
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

            // Defer full layout until the handle is created so panel sizes are known
            this.HandleCreated += (s, e) =>
            {
                BuildTypeAwareLayout();
                SetupAutoSave();
                UpdateNavButtons();
            };
        }

        //  Backward-compatible constructor 
        public GradingInterface(
            StudentSubmission submission,
            ActivityItem activity,
            List<StudentSubmission> allStudents = null,
            int index = 0)
            : this(submission, activity, allStudents, index, null) { }

        //  TOP-LEVEL LAYOUT BUILDER
        private void BuildTypeAwareLayout()
        {
            lblActivityTitle.Text = _activity.Title;
            lblMaxPoints.Text = $"Max: {_activity.Points} pts";

            BuildRightColumn();   // right panel: score / rubric / remarks
            BuildLeftColumn();    // left panel:  type-specific submission content
            LoadStudent();        // populate current-student data into both panels
        }

        //  RIGHT COLUMN — Score input, rubric, remarks
        private void BuildRightColumn()
        {
            pnlGrading.Controls.Clear();
            _rubricRows.Clear();

            int y = 10;

            // "Grading" header 
            pnlGrading.Controls.Add(new Label
            {
                Text = "🎯  Grading",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Maroon,
                Location = new Point(12, y),
                AutoSize = true,
                TabIndex = 0
            });
            y += 28;

            // Save status label 
            _lblSaveStatus = new Label
            {
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                ForeColor = Color.ForestGreen,
                Location = new Point(12, y),
                Size = new Size(pnlGrading.Width - 24, 18),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                TabIndex = 1
            };
            pnlGrading.Controls.Add(_lblSaveStatus);
            y += 24;

            // Rubric section (only for non-quiz activity types) ─
            bool isQuiz = _activity.Type == ActivityType.Quiz;
            if (!isQuiz)
                y = BuildRubricSection(y);

            // Score input panel 
            int scoreW = pnlGrading.Width - 24;
            var pnlScore = new Panel
            {
                BackColor = Color.White,
                Location = new Point(12, y),
                Size = new Size(Math.Max(240, scoreW), 90),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                TabIndex = 10
            };
            pnlScore.Paint += (s, e) =>
            {
                using var pen = new Pen(PanelBorder);
                e.Graphics.DrawRectangle(pen, 0, 0, pnlScore.Width - 1, pnlScore.Height - 1);
            };

            pnlScore.Controls.Add(new Label
            {
                Text = "Final Score:",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 35, 45),
                Location = new Point(4, 12),
                Size = new Size(115, 24)
            });

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
                Size = new Size(170, 32),
                Text = "✔  Save and Lock Score",
                UseVisualStyleBackColor = false
            };
            _btnSave.FlatAppearance.BorderSize = 0;
            _btnSave.Click += btnSaveScore_Click;

            pnlScore.Controls.AddRange(new Control[] { _txtScore, _lblScoreOf, _btnSave });
            pnlGrading.Controls.Add(pnlScore);
            y += 100;

            // Remarks panel 
            var pnlRem = new Panel
            {
                BackColor = Color.White,
                Location = new Point(12, y),
                Size = new Size(Math.Max(240, scoreW), 240),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                TabIndex = 11
            };

            pnlRem.Controls.Add(new Label
            {
                Text = "💬  Remarks / Feedback",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 70, 80),
                Location = new Point(4, 4),
                Size = new Size(260, 22)
            });

            _txtRemarks = new TextBox
            {
                Font = new Font("Segoe UI", 9.5F),
                Location = new Point(0, 30),
                Multiline = true,
                PlaceholderText = "Add comments or feedback for the student...",
                ScrollBars = ScrollBars.Vertical,
                Size = new Size(Math.Max(220, scoreW), 205),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            pnlRem.Controls.Add(_txtRemarks);
            pnlGrading.Controls.Add(pnlRem);
        }

        //  Rubric section builder
        private int BuildRubricSection(int startY)
        {
            // Only show rubric section when the activity actually has rubric items
            if (!_activity.HasRubric && (_activity.RubricItems == null || _activity.RubricItems.Count == 0))
                return startY;

            var items = (_activity.RubricItems != null && _activity.RubricItems.Count > 0)
                ? _activity.RubricItems
                : new List<RubricCriteria>
                {
                    new RubricCriteria { CriteriaId = 1, Name = "Content",   MaxPoints = 25 },
                    new RubricCriteria { CriteriaId = 2, Name = "Structure", MaxPoints = 25 },
                    new RubricCriteria { CriteriaId = 3, Name = "Grammar",   MaxPoints = 25 },
                    new RubricCriteria { CriteriaId = 4, Name = "Relevance", MaxPoints = 25 }
                };

            int y = startY;
            int boxH = 48 + items.Count * 44 + 60;
            int boxW = Math.Max(240, pnlGrading.Width - 24);

            var pnlBox = new Panel
            {
                BackColor = Color.FromArgb(251, 249, 249),
                BorderStyle = BorderStyle.None,
                Location = new Point(12, y),
                Size = new Size(boxW, boxH),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                TabIndex = 5
            };
            pnlBox.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(200, 180, 190));
                e.Graphics.DrawRectangle(pen, 0, 0, pnlBox.Width - 1, pnlBox.Height - 1);
                using var bar = new SolidBrush(Maroon);
                e.Graphics.FillRectangle(bar, 0, 0, 4, pnlBox.Height);
            };

            pnlBox.Controls.Add(new Label
            {
                Text = "📊  Rubric-Based Grading",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 70, 80),
                Location = new Point(14, 8),
                AutoSize = true
            });

            var flp = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown,
                Location = new Point(10, 34),
                Width = Math.Max(200, boxW - 20),
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
                Location = new Point(14, boxH - 48),
                Size = new Size(boxW - 28, 22)
            };
            pnlBox.Controls.Add(_lblRubricTotal);

            _chkAutoScore = new CheckBox
            {
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(60, 60, 70),
                Location = new Point(14, boxH - 26),
                Size = new Size(260, 22),
                Text = "Auto-fill score from rubric total",
                TabIndex = 1
            };
            _chkAutoScore.CheckedChanged += chkAutoScore_CheckedChanged;
            pnlBox.Controls.Add(_chkAutoScore);

            pnlGrading.Controls.Add(pnlBox);
            return y + boxH + 10;
        }

        private Panel BuildRubricRow(RubricCriteria crit, out NumericUpDown nud)
        {
            int rowW = Math.Max(280, pnlGrading.Width - 48);
            var row = new Panel
            {
                Width = rowW,
                Height = 38,
                BackColor = Color.Transparent
            };

            row.Controls.Add(new Label
            {
                Text = crit.Name,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 60),
                Location = new Point(0, 10),
                Size = new Size(118, 20)
            });

            nud = new NumericUpDown
            {
                Minimum = 0,
                Maximum = crit.MaxPoints,
                Value = 0,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(124, 8),
                Size = new Size(62, 24)
            };

            row.Controls.Add(new Label
            {
                Text = $"/ {crit.MaxPoints}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(192, 12),
                Size = new Size(50, 18)
            });

            int barW = Math.Max(40, rowW - 250);
            var bar = new Panel
            {
                Location = new Point(246, 14),
                Size = new Size(barW, 10),
                BackColor = Color.FromArgb(235, 235, 240)
            };
            var fill = new Panel { Location = Point.Empty, Size = new Size(0, 10), BackColor = Maroon };
            bar.Controls.Add(fill);

            var nudRef = nud;
            nudRef.ValueChanged += (s, e) =>
            {
                int px = crit.MaxPoints > 0 ? (int)((double)nudRef.Value / crit.MaxPoints * barW) : 0;
                fill.Width = Math.Clamp(px, 0, barW);
                UpdateRubricTotal();
            };

            row.Controls.AddRange(new Control[] { nudRef, bar });
            return row;
        }

        //  LEFT COLUMN — type-specific submission content
        private void BuildLeftColumn()
        {
            pnlEssay.Controls.Clear();

            // No submission at all → show informational banner
            if (_current.Status == "Missing" && string.IsNullOrEmpty(_current.SubmissionDbId))
            {
                BuildNoSubmissionContent();
                return;
            }

            switch (_activity.Type)
            {
                case ActivityType.Quiz: BuildQuizContent(); break;
                case ActivityType.Essay: BuildEssayContent(); break;
                case ActivityType.FileUpload: BuildFileUploadContent(); break;
                default: BuildAssignmentContent(); break;
            }
        }

        //  NO SUBMISSION banner
        private void BuildNoSubmissionContent()
        {
            pnlEssay.AutoScroll = false;

            pnlEssay.Controls.Add(new Label
            {
                Text = "📋  Submission Review",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Maroon,
                Location = new Point(12, 10),
                AutoSize = true
            });

            int pnlW = Math.Max(400, pnlEssay.Width - 24);
            var pnl = new Panel
            {
                BackColor = Color.FromArgb(252, 248, 248),
                Location = new Point(12, 40),
                Size = new Size(pnlW, 140),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            pnl.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(220, 180, 180));
                e.Graphics.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
                using var bar = new SolidBrush(WrongRed);
                e.Graphics.FillRectangle(bar, 0, 0, 5, pnl.Height);
            };
            pnl.Controls.Add(new Label
            {
                Text = "⚠  No Submission Found",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = WrongRed,
                Location = new Point(18, 28),
                AutoSize = true
            });
            pnl.Controls.Add(new Label
            {
                Text = "This student has not submitted anything for this activity.",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(100, 80, 80),
                Location = new Point(18, 68),
                AutoSize = true
            });
            pnl.Controls.Add(new Label
            {
                Text = "You may still enter a manual score in the Grading panel.",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = Color.Gray,
                Location = new Point(18, 94),
                AutoSize = true
            });
            pnlEssay.Controls.Add(pnl);
        }

        //  QUIZ — all questions with student vs correct answers
        private void BuildQuizContent()
        {
            pnlEssay.AutoScroll = true;

            // Header row 
            pnlEssay.Controls.Add(new Label
            {
                Text = "QUIZ",
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                BackColor = QuizBlue,
                ForeColor = Color.White,
                Location = new Point(12, 10),
                Size = new Size(40, 18),
                TextAlign = ContentAlignment.MiddleCenter
            });
            pnlEssay.Controls.Add(new Label
            {
                Text = "❓  Quiz Answers Review",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = QuizBlue,
                Location = new Point(60, 8),
                AutoSize = true
            });

            // Legend 
            var legendPanel = new Panel
            {
                Location = new Point(12, 34),
                Size = new Size(500, 22),
                BackColor = Color.Transparent
            };
            legendPanel.Controls.Add(new Label { Text = "▐ Correct", Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = CorrectGreen, Location = new Point(0, 3), AutoSize = true });
            legendPanel.Controls.Add(new Label { Text = "▐ Wrong", Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = WrongRed, Location = new Point(82, 3), AutoSize = true });
            legendPanel.Controls.Add(new Label { Text = "▐ Not answered", Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(140, 140, 150), Location = new Point(152, 3), AutoSize = true });
            pnlEssay.Controls.Add(legendPanel);

            // No questions guard 
            if (_activity.Questions == null || _activity.Questions.Count == 0)
            {
                pnlEssay.Controls.Add(new Label
                {
                    Text = "No quiz questions are configured for this activity.",
                    Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    Location = new Point(12, 62),
                    AutoSize = true
                });
                return;
            }

            // Parse student answers from EssayContent JSON: {"1":"A. choice","2":"C. choice"}
            var storedAnswers = ParseQuizAnswers(_current.EssayContent);

            // FlowLayoutPanel for question cards 
            int flpW = Math.Max(400, pnlEssay.Width - 32);
            var flp = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown,
                Location = new Point(12, 60),
                Width = flpW,
                WrapContents = false,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            pnlEssay.Controls.Add(flp);

            // Update FLP width when panel resizes
            pnlEssay.SizeChanged += (s, e) =>
            {
                if (!flp.IsDisposed)
                    flp.Width = Math.Max(400, pnlEssay.Width - 32);
            };

            int autoScore = 0;
            int totalPoints = 0;

            for (int qi = 0; qi < _activity.Questions.Count; qi++)
            {
                var q = _activity.Questions[qi];
                totalPoints += q.Points;

                // Question numbers are 1-based in the serialized JSON (number: 1, 2, 3...)
                // and ParseQuizAnswers returns keys as 1-based ints
                int qNum = qi + 1;
                string studentAnswer = storedAnswers.TryGetValue(qNum, out var sa) ? sa : "";
                bool answered = !string.IsNullOrWhiteSpace(studentAnswer);
                bool isCorrect = answered && string.Equals(
                    studentAnswer.Trim(), q.CorrectAnswer.Trim(),
                    StringComparison.OrdinalIgnoreCase);

                if (isCorrect) autoScore += q.Points;

                Color cardAccent = !answered ? Color.FromArgb(180, 180, 180)
                                 : isCorrect ? CorrectGreen
                                 : WrongRed;

                int cardW = flp.Width - 8;
                var card = new Panel
                {
                    Width = cardW,
                    BackColor = Color.White,
                    Margin = new Padding(0, 0, 0, 8),
                    AutoSize = false
                };

                // Capture accent color for Paint closure
                Color ca = cardAccent;
                card.Paint += (s, e) =>
                {
                    using var pen = new Pen(PanelBorder);
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                    using var acc = new SolidBrush(ca);
                    e.Graphics.FillRectangle(acc, 0, 0, 5, card.Height);
                };

                int cy = 8;

                // ── Question header row 
                int hdrW = Math.Max(300, cardW - 12);
                var pnlQH = new Panel
                {
                    Location = new Point(6, cy),
                    BackColor = Color.Transparent,
                    Size = new Size(hdrW, 28)
                };

                // Q-number badge
                pnlQH.Controls.Add(new Label
                {
                    Text = $"Q{qNum}",
                    BackColor = QuizBlue,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    Size = new Size(28, 22),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(0, 3)
                });

                // Type badge
                string typeBadge = q.QuestionType switch
                {
                    "TrueFalse" => "T/F",
                    "Identification" => "ID",
                    "Essay" => "Ess",
                    _ => "MC"
                };
                pnlQH.Controls.Add(new Label
                {
                    Text = typeBadge,
                    BackColor = Color.FromArgb(230, 230, 250),
                    ForeColor = QuizBlue,
                    Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                    Size = new Size(30, 20),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(34, 4)
                });

                // Question text
                pnlQH.Controls.Add(new Label
                {
                    Text = q.QuestionText,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(20, 20, 25),
                    Location = new Point(70, 5),
                    MaximumSize = new Size(Math.Max(80, hdrW - 160), 0),
                    AutoSize = true
                });

                // Points badge (right-aligned)
                pnlQH.Controls.Add(new Label
                {
                    Text = $"{q.Points} pt{(q.Points != 1 ? "s" : "")}",
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    ForeColor = Maroon,
                    Location = new Point(Math.Max(200, hdrW - 74), 6),
                    Size = new Size(60, 18),
                    TextAlign = ContentAlignment.MiddleRight,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                });

                card.Controls.Add(pnlQH);
                cy += 32;

                //  Divider 
                card.Controls.Add(new Panel
                {
                    Location = new Point(6, cy),
                    BackColor = PanelBorder,
                    Size = new Size(Math.Max(200, cardW - 12), 1)
                });
                cy += 6;

                //  Answer comparison row 
                int ansW = Math.Max(300, cardW - 12);
                var pnlAns = new Panel
                {
                    Location = new Point(6, cy),
                    BackColor = Color.Transparent,
                    Width = ansW,
                    Height = 28
                };

                // Student answer
                string saDisplay = answered ? studentAnswer : "(no answer)";
                Color saColor = !answered ? Color.Gray : isCorrect ? CorrectGreen : WrongRed;
                string saIcon = !answered ? "—" : isCorrect ? "✔" : "✗";

                pnlAns.Controls.Add(new Label
                {
                    Text = "Student:",
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(80, 80, 90),
                    Location = new Point(0, 4),
                    Size = new Size(66, 18)
                });
                pnlAns.Controls.Add(new Label
                {
                    Text = $"{saIcon}  {saDisplay}",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = saColor,
                    Location = new Point(70, 2),
                    MaximumSize = new Size(Math.Max(60, ansW / 2 - 80), 0),
                    AutoSize = true
                });

                // Correct answer (right half)
                int half = Math.Max(150, ansW / 2);
                pnlAns.Controls.Add(new Label
                {
                    Text = "Correct:",
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(80, 80, 90),
                    Location = new Point(half, 4),
                    Size = new Size(62, 18)
                });
                pnlAns.Controls.Add(new Label
                {
                    Text = q.CorrectAnswer,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = CorrectGreen,
                    Location = new Point(half + 66, 2),
                    MaximumSize = new Size(Math.Max(60, ansW / 2 - 80), 0),
                    AutoSize = true
                });

                card.Controls.Add(pnlAns);
                cy += 32;

                //  MC choices (collapsed visual reference) 
                if (q.QuestionType == "MultipleChoice" && q.Choices?.Count > 0)
                {
                    var pnlChoices = new Panel
                    {
                        Location = new Point(6, cy),
                        BackColor = Color.FromArgb(250, 250, 253),
                        Width = Math.Max(200, cardW - 12),
                        AutoSize = false
                    };
                    pnlChoices.Paint += (s, e) =>
                    {
                        using var pen = new Pen(Color.FromArgb(230, 230, 240));
                        e.Graphics.DrawRectangle(pen, 0, 0, pnlChoices.Width - 1, pnlChoices.Height - 1);
                    };

                    string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H" };
                    int chY = 4;
                    for (int ci = 0; ci < q.Choices.Count; ci++)
                    {
                        string letter = ci < letters.Length ? letters[ci] : $"#{ci + 1}";
                        bool isStudentChoice = !string.IsNullOrEmpty(studentAnswer) &&
                            studentAnswer.StartsWith($"{letter}.", StringComparison.OrdinalIgnoreCase);
                        bool isCorrectChoice = q.CorrectAnswer.StartsWith($"{letter}.", StringComparison.OrdinalIgnoreCase);

                        Color choiceColor = (isStudentChoice && isCorrect) ? CorrectGreen
                                           : isStudentChoice ? WrongRed
                                           : isCorrectChoice ? CorrectGreen
                                           : Color.FromArgb(60, 60, 70);
                        FontStyle fs = (isStudentChoice || isCorrectChoice) ? FontStyle.Bold : FontStyle.Regular;
                        string prefix = isStudentChoice ? (isCorrect ? "✔ " : "✗ ")
                                        : isCorrectChoice ? "★ " : "   ";

                        pnlChoices.Controls.Add(new Label
                        {
                            Text = $"{prefix}{letter}. {q.Choices[ci]}",
                            Font = new Font("Segoe UI", 8.5F, fs),
                            ForeColor = choiceColor,
                            Location = new Point(8, chY),
                            AutoSize = false,
                            Width = Math.Max(160, pnlChoices.Width - 16),
                            Height = 18,
                            AutoEllipsis = true
                        });
                        chY += 20;
                    }
                    pnlChoices.Height = chY + 4;
                    card.Controls.Add(pnlChoices);
                    cy += pnlChoices.Height + 4;
                }

                card.Height = cy + 8;
                flp.Controls.Add(card);
            }

            //  Auto-score summary bar 
            int scaledScore = totalPoints > 0
                ? (int)Math.Round((double)autoScore / totalPoints * _activity.Points)
                : 0;

            var pnlSummary = new Panel
            {
                Width = Math.Max(400, flp.Width - 8),
                Height = 56,
                BackColor = Color.FromArgb(235, 242, 255),
                Margin = new Padding(0, 6, 0, 0)
            };
            pnlSummary.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(180, 210, 240));
                e.Graphics.DrawRectangle(pen, 0, 0, pnlSummary.Width - 1, pnlSummary.Height - 1);
                using var bar = new SolidBrush(QuizBlue);
                e.Graphics.FillRectangle(bar, 0, 0, 4, pnlSummary.Height);
            };
            pnlSummary.Controls.Add(new Label
            {
                Text = $"Auto-calculated score:  {scaledScore} / {_activity.Points} pts",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Maroon,
                Location = new Point(14, 10),
                AutoSize = true
            });
            pnlSummary.Controls.Add(new Label
            {
                Text = $"({autoScore} / {totalPoints} raw points  ·  {_activity.Questions.Count} question{(_activity.Questions.Count == 1 ? "" : "s")})",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(70, 70, 90),
                Location = new Point(14, 32),
                AutoSize = true
            });

            var btnAutoFill = new buttonRounded
            {
                Text = "⟶ Auto-fill Score",
                Size = new Size(142, 28),
                BackColor = QuizBlue,
                ForeColor = Color.White,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Location = new Point(pnlSummary.Width - 158, 14),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            int capturedScore = scaledScore;
            btnAutoFill.Click += (s, e) =>
            {
                if (_txtScore != null && !_current.IsChecked)
                    _txtScore.Text = capturedScore.ToString();
            };
            pnlSummary.Controls.Add(btnAutoFill);
            flp.Controls.Add(pnlSummary);

            // Pre-fill score when not yet graded
            if (!_current.IsChecked && _txtScore != null && _current.Score < 0)
                _txtScore.Text = scaledScore.ToString();
        }

        //  ESSAY — submitted text content
        private void BuildEssayContent()
        {
            pnlEssay.AutoScroll = false;

            pnlEssay.Controls.Add(new Label
            {
                Text = "ESSAY",
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 150, 136),
                ForeColor = Color.White,
                Location = new Point(12, 10),
                Size = new Size(50, 18),
                TextAlign = ContentAlignment.MiddleCenter
            });
            pnlEssay.Controls.Add(new Label
            {
                Text = "📄  Submission Content",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Maroon,
                Location = new Point(70, 8),
                AutoSize = true
            });

            Color statusColor = GetStatusColor(_current.Status);
            pnlEssay.Controls.Add(new Label
            {
                Text = $"● {_current.Status}   ·   Submitted: {FormatDateTime(_current.SubmissionTime)}",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = statusColor,
                Location = new Point(12, 36),
                AutoSize = true
            });

            string content = _current.EssayContent ?? "";

            if (string.IsNullOrWhiteSpace(content))
            {
                pnlEssay.Controls.Add(new Label
                {
                    Text = "(No essay text was submitted.)",
                    Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    Location = new Point(12, 62),
                    AutoSize = true
                });
                return;
            }

            // Word/char count bar
            var pnlWC = new Panel
            {
                BackColor = Color.FromArgb(245, 245, 248),
                Location = new Point(12, 60),
                Height = 22,
                Width = Math.Max(400, pnlEssay.Width - 24),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            pnlWC.Controls.Add(new Label
            {
                Text = GetWordCharCount(content),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(8, 3),
                AutoSize = true
            });
            pnlEssay.Controls.Add(pnlWC);

            int txtH = Math.Max(200, pnlEssay.Height - 100);
            var txtContent = new TextBox
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10F),
                Location = new Point(12, 86),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Text = content,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                Size = new Size(Math.Max(400, pnlEssay.Width - 24), txtH)
            };
            pnlEssay.Controls.Add(txtContent);
        }

        //  ASSIGNMENT — file or text submission
        private void BuildAssignmentContent()
        {
            pnlEssay.AutoScroll = true;

            pnlEssay.Controls.Add(new Label
            {
                Text = "ASSIGNMENT",
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                BackColor = Maroon,
                ForeColor = Color.White,
                Location = new Point(12, 10),
                Size = new Size(86, 18),
                TextAlign = ContentAlignment.MiddleCenter
            });
            pnlEssay.Controls.Add(new Label
            {
                Text = "📋  Submitted Assignment",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Maroon,
                Location = new Point(106, 8),
                AutoSize = true
            });

            int y = 36;

            Color statusColor = GetStatusColor(_current.Status);
            pnlEssay.Controls.Add(new Label
            {
                Text = $"● {_current.Status}   ·   Submitted: {FormatDateTime(_current.SubmissionTime)}",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = statusColor,
                Location = new Point(12, y),
                AutoSize = true
            });
            y += 28;

            string submittedContent = _current.EssayContent ?? "";
            bool isRealUrl = IsHttpUrl(submittedContent);
            bool isLocalPath = !isRealUrl && IsLikelyLocalPath(submittedContent);

            if (string.IsNullOrWhiteSpace(submittedContent))
            {
                pnlEssay.Controls.Add(new Label
                {
                    Text = "No content was submitted for this assignment.",
                    Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    Location = new Point(12, y),
                    AutoSize = true
                });
            }
            else if (isRealUrl)
            {
                // Proper Cloudinary/remote URL → file download card
                var fileCard = BuildFileCard(submittedContent, y);
                pnlEssay.Controls.Add(fileCard);
                y += fileCard.Height + 12;
            }
            else if (isLocalPath)
            {
                // Student stored a local path (upload was never completed)
                string fn = System.IO.Path.GetFileName(submittedContent);
                int warnW = Math.Max(400, pnlEssay.Width - 24);
                var warnPanel = new Panel
                {
                    BackColor = Color.FromArgb(255, 252, 235),
                    Location = new Point(12, y),
                    Size = new Size(warnW, 106),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                };
                warnPanel.Paint += (s, e) =>
                {
                    using var pen = new Pen(Color.FromArgb(230, 200, 100));
                    e.Graphics.DrawRectangle(pen, 0, 0, warnPanel.Width - 1, warnPanel.Height - 1);
                    using var bar = new SolidBrush(Color.FromArgb(200, 160, 0));
                    e.Graphics.FillRectangle(bar, 0, 0, 4, warnPanel.Height);
                };
                warnPanel.Controls.Add(new Label
                {
                    Text = "📎  File Submitted — Upload Not Completed (Local Path)",
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(140, 100, 0),
                    Location = new Point(14, 10),
                    AutoSize = true
                });
                warnPanel.Controls.Add(new Label
                {
                    Text = $"File name: {fn}",
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.FromArgb(80, 60, 0),
                    Location = new Point(14, 34),
                    AutoSize = true
                });
                warnPanel.Controls.Add(new Label
                {
                    Text = "The student submitted a local file path. No downloadable URL is stored.",
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    Location = new Point(14, 58),
                    Size = new Size(Math.Max(300, warnW - 28), 36),
                    AutoEllipsis = true
                });
                warnPanel.Controls.Add(new Label
                {
                    Text = "The student must re-submit with a proper file upload.",
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    Location = new Point(14, 80),
                    AutoSize = true
                });
                pnlEssay.Controls.Add(warnPanel);
                y += warnPanel.Height + 12;
            }
            else
            {
                // Plain text submission
                pnlEssay.Controls.Add(new Label
                {
                    Text = "Submission Text:",
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(50, 50, 60),
                    Location = new Point(12, y),
                    AutoSize = true
                });
                y += 22;

                int txtH = Math.Max(120, Math.Min(350, pnlEssay.Height - y - 20));
                pnlEssay.Controls.Add(new TextBox
                {
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Font = new Font("Segoe UI", 10F),
                    Location = new Point(12, y),
                    Multiline = true,
                    ReadOnly = true,
                    ScrollBars = ScrollBars.Vertical,
                    Size = new Size(Math.Max(400, pnlEssay.Width - 24), txtH),
                    Text = submittedContent,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                });
            }
        }

        //  FILE UPLOAD — dedicated file review
        private void BuildFileUploadContent()
        {
            pnlEssay.AutoScroll = true;

            pnlEssay.Controls.Add(new Label
            {
                Text = "FILE UPLOAD",
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                Location = new Point(12, 10),
                Size = new Size(82, 18),
                TextAlign = ContentAlignment.MiddleCenter
            });
            pnlEssay.Controls.Add(new Label
            {
                Text = "📎  Submitted File",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Maroon,
                Location = new Point(102, 8),
                AutoSize = true
            });

            int y = 36;
            Color statusColor = GetStatusColor(_current.Status);
            pnlEssay.Controls.Add(new Label
            {
                Text = $"● {_current.Status}   ·   {FormatDateTime(_current.SubmissionTime)}",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = statusColor,
                Location = new Point(12, y),
                AutoSize = true
            });
            y += 30;

            string fileContent = _current.EssayContent ?? "";
            bool isRealUrl = IsHttpUrl(fileContent);
            bool isLocalPath = !isRealUrl && IsLikelyLocalPath(fileContent);

            if (isRealUrl)
            {
                var fileCard = BuildFileCard(fileContent, y);
                pnlEssay.Controls.Add(fileCard);
            }
            else if (isLocalPath)
            {
                string fn = System.IO.Path.GetFileName(fileContent);
                int warnW = Math.Max(400, pnlEssay.Width - 24);
                var warnPanel = new Panel
                {
                    BackColor = Color.FromArgb(255, 252, 235),
                    Location = new Point(12, y),
                    Size = new Size(warnW, 94),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                };
                warnPanel.Paint += (s, e) =>
                {
                    using var pen = new Pen(Color.FromArgb(230, 200, 100));
                    e.Graphics.DrawRectangle(pen, 0, 0, warnPanel.Width - 1, warnPanel.Height - 1);
                    using var bar = new SolidBrush(Color.FromArgb(200, 160, 0));
                    e.Graphics.FillRectangle(bar, 0, 0, 4, warnPanel.Height);
                };
                warnPanel.Controls.Add(new Label
                {
                    Text = "📎  Upload Not Completed — Local File Path Stored",
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(140, 100, 0),
                    Location = new Point(14, 10),
                    AutoSize = true
                });
                warnPanel.Controls.Add(new Label
                {
                    Text = $"File name: {fn}",
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.FromArgb(80, 60, 0),
                    Location = new Point(14, 34),
                    AutoSize = true
                });
                warnPanel.Controls.Add(new Label
                {
                    Text = "No downloadable URL is available. The student must re-submit.",
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    Location = new Point(14, 58),
                    AutoSize = true
                });
                pnlEssay.Controls.Add(warnPanel);
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

        //  Shared file card builder
        private Panel BuildFileCard(string fileUrl, int y)
        {
            bool isUrl = IsHttpUrl(fileUrl);

            string fileName;
            try
            {
                fileName = isUrl
                    ? Uri.UnescapeDataString(System.IO.Path.GetFileName(new Uri(fileUrl).AbsolutePath))
                    : fileUrl;
            }
            catch { fileName = "submission_file"; }
            if (string.IsNullOrWhiteSpace(fileName)) fileName = "submission_file";

            string ext = System.IO.Path.GetExtension(fileName).ToUpper().TrimStart('.');
            string icon = ext switch
            {
                "PDF" => "📄",
                "DOCX" or "DOC" => "📝",
                "PPTX" or "PPT" => "📊",
                "XLSX" or "XLS" => "📊",
                "PNG" or "JPG" or "JPEG" => "🖼",
                "ZIP" or "RAR" or "7Z" => "🗜",
                _ => "📎"
            };

            int cardW = Math.Max(400, pnlEssay.Width - 30);
            var card = new Panel
            {
                BackColor = Color.White,
                Location = new Point(12, y),
                Size = new Size(cardW, 72),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            card.Paint += (s, e) =>
            {
                using var pen = new Pen(PanelBorder);
                e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                using var acc = new SolidBrush(QuizBlue);
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
                ForeColor = isUrl ? Color.FromArgb(30, 50, 160) : Color.FromArgb(80, 80, 90),
                Location = new Point(56, 10),
                Size = new Size(Math.Max(100, cardW - 240), 20),
                AutoEllipsis = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            });
            card.Controls.Add(new Label
            {
                Text = isUrl ? "Stored on Cloudinary — click Download to save"
                                  : fileUrl,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.Gray,
                Location = new Point(56, 32),
                Size = new Size(Math.Max(100, cardW - 240), 18),
                AutoEllipsis = true
            });

            // Preview button for images / PDFs
            bool canPreview = new[] { "PNG", "JPG", "JPEG", "PDF" }.Contains(ext);
            if (isUrl && canPreview)
            {
                string capturedUrl = fileUrl;
                var btnOpen = new buttonRounded
                {
                    Text = "🔍 Preview",
                    Size = new Size(90, 30),
                    Location = new Point(Math.Max(200, cardW - 218), 20),
                    BackColor = Color.FromArgb(63, 81, 181),
                    ForeColor = Color.White,
                    BorderRadius = 8,
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };
                btnOpen.Click += (s, e) =>
                {
                    try
                    {
                        System.Diagnostics.Process.Start(
                            new System.Diagnostics.ProcessStartInfo(capturedUrl) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Could not open file:\n{ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                card.Controls.Add(btnOpen);
            }

            // Download button
            string capturedFileUrl = fileUrl;
            string capturedFileName = fileName;
            var btnDl = new buttonRounded
            {
                Text = "↓ Download",
                Size = new Size(108, 30),
                Location = new Point(Math.Max(200, cardW - 120), 20),
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
                        FileName = capturedFileName,
                        Filter = "All Files (*.*)|*.*"
                    };
                    if (sfd.ShowDialog() != DialogResult.OK) return;

                    string tmp = CloudinaryService.Instance.DownloadToTemp(capturedFileUrl, capturedFileName);
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

        //  Quiz answer parser
        //  Handles JSON: {"1":"A. choice","2":"C. choice"}
        //  Handles plain text: "Q1: answer\nQ2: answer"
        private static Dictionary<int, string> ParseQuizAnswers(string raw)
        {
            var dict = new Dictionary<int, string>();
            if (string.IsNullOrWhiteSpace(raw)) return dict;

            // JSON format
            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(raw);
                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    if (int.TryParse(prop.Name, out int k))
                        dict[k] = prop.Value.GetString() ?? "";
                }
                if (dict.Count > 0) return dict;
            }
            catch { }

            // Plain text fallback: "Q1: answer"
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

        //  Utility helpers
        private static bool IsHttpUrl(string s) =>
            !string.IsNullOrWhiteSpace(s) &&
            (s.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
             s.StartsWith("https://", StringComparison.OrdinalIgnoreCase));

        private static bool IsLikelyLocalPath(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            if (s.Length >= 3 && char.IsLetter(s[0]) && s[1] == ':') return true;  // C:\...
            if (s.StartsWith("\\\\")) return true;                                    // UNC
            if (s.StartsWith("/") && !s.StartsWith("//")) return true;               // Unix
            return false;
        }

        private static Color GetStatusColor(string status) => status switch
        {
            "Submitted" => Color.FromArgb(26, 128, 64),
            "Late" => Color.OrangeRed,
            "Graded" => Color.FromArgb(63, 81, 181),
            "Returned" => Color.FromArgb(90, 90, 100),
            _ => Color.Gray
        };

        private static string FormatDateTime(DateTime dt) =>
            dt == DateTime.MinValue ? "—" : dt.ToString("MMM dd, yyyy  hh:mm tt");

        private static string GetWordCharCount(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "Words: 0  |  Characters: 0";
            int words = text.Split(new[] { ' ', '\n', '\r', '\t' },
                                   StringSplitOptions.RemoveEmptyEntries).Length;
            return $"Words: {words}  |  Characters: {text.Length}";
        }

        //  STUDENT DATA LOADING
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
            bool missing = _current.Status == "Missing" && string.IsNullOrEmpty(_current.SubmissionDbId);

            if (_txtScore != null) _txtScore.ReadOnly = locked;
            if (_btnSave != null) _btnSave.Enabled = !locked && !missing;
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
                if (locked)
                {
                    _lblSaveStatus.Text = "✅ Already checked – score locked";
                    _lblSaveStatus.ForeColor = Maroon;
                }
                else if (missing)
                {
                    _lblSaveStatus.Text = "⚠ No submission — you can still enter a manual grade";
                    _lblSaveStatus.ForeColor = Color.OrangeRed;
                }
                else
                {
                    _lblSaveStatus.Text = "";
                }
            }

            UpdateRubricTotal();
        }

        //  AUTO-SAVE & RUBRIC HELPERS
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

        //  SAVE SCORE (lock)
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
            if (_txtScore != null) _txtScore.ReadOnly = true;
            if (_btnSave != null) _btnSave.Enabled = false;
            if (_txtRemarks != null) _txtRemarks.ReadOnly = true;
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

        //  STUDENT NAVIGATION
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
            // Rebuild both columns fresh for the new student.
            // BuildRightColumn() MUST be called first — it clears _rubricRows.
            BuildRightColumn();
            BuildLeftColumn();
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