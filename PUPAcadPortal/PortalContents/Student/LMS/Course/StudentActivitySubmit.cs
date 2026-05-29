using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class StudentActivitySubmit : UserControl
    {
        public event Action OnBack;

        private readonly StudentActivityItem _activity;
        private readonly StudentCourse _course;
        // Quiz state
        private int _currentQ = 0;
        private Dictionary<int, string> _answers = new();
        // Timer / deadline
        private System.Windows.Forms.Timer _countdownTimer;
        private System.Windows.Forms.Timer _autosaveTimer;
        // File upload
        private string _uploadedFilePath = "";
        private string _uploadedFileName = "";
        // Controls built in code (no Designer involvement for dynamic sections)
        private Panel _pnlContent;
        private Label _lblDeadlineTimer;

        public StudentActivitySubmit(StudentActivityItem activity, StudentCourse course)
        {
            _activity = activity;
            _course = course;
            _answers = new Dictionary<int, string>(_activity.Answers ?? new());

            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            BuildHeader();
            BuildDeadlineBar();
            BuildContentArea();

            // Autosave for essay (every 30 s)
            if (_activity.Type == "Essay")
            {
                _autosaveTimer = new System.Windows.Forms.Timer { Interval = 30_000 };
                _autosaveTimer.Tick += (s, e) => AutosaveDraft();
                _autosaveTimer.Start();
            }

            StartCountdown();
        }
        
        // HEADER
        private void BuildHeader()
        {
            // pnlHeader already exists from Designer; populate labels
            lblActivityTitle.Text = _activity.Title;

            string typeDisplay = _activity.Type switch
            {
                "LongQuiz" => "Long Quiz",
                "FileUpload" => "File Upload",
                _ => _activity.Type
            };

            lblMeta.Text = $"{typeDisplay}  ·  {_activity.Points} pts  ·  Due {_activity.Deadline:MMM dd, yyyy}";

            // Show score + returned info in header if graded
            if (_activity.Score.HasValue)
            {
                lblMeta.Text += $"  ·  Score: {_activity.Score}/{_activity.Points}";
                lblMeta.ForeColor = Color.FromArgb(255, 210, 210);
            }
        }

        // DEADLINE COUNTDOWN BAR
        private void BuildDeadlineBar()
        {
            TimeSpan remaining = _activity.Deadline - DateTime.Now;
            bool isLate = remaining.TotalSeconds <= 0;

            var pnlBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 32,
                BackColor = isLate ? Color.FromArgb(180, 20, 20) : Color.FromArgb(50, 50, 50)
            };

            _lblDeadlineTimer = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                Text = isLate ? "⚠  Deadline has passed — late submission" : FormatCountdown(remaining)
            };

            pnlBar.Controls.Add(_lblDeadlineTimer);

            // Insert below pnlHeader (pnlHeader is Dock Top, so just add another Dock Top)
            Controls.Add(pnlBar);
            pnlBar.BringToFront();

            // Re-order: header on top, then deadline bar, then body
            pnlHeader.BringToFront();
            pnlBar.SendToBack();
            pnlBody.SendToBack();
        }

        private void StartCountdown()
        {
            _countdownTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _countdownTimer.Tick += (s, e) =>
            {
                TimeSpan rem = _activity.Deadline - DateTime.Now;
                if (_lblDeadlineTimer == null) return;

                if (rem.TotalSeconds <= 0)
                {
                    _lblDeadlineTimer.Text = "⚠  Deadline has passed — late submission";
                    if (_lblDeadlineTimer.Parent is Panel p) p.BackColor = Color.FromArgb(180, 20, 20);
                    _countdownTimer.Stop();
                }
                else
                {
                    _lblDeadlineTimer.Text = FormatCountdown(rem);
                    if (_lblDeadlineTimer.Parent is Panel p)
                        p.BackColor = rem.TotalHours < 1 ? Color.FromArgb(180, 80, 0) : Color.FromArgb(50, 50, 50);
                }
            };
            _countdownTimer.Start();
        }

        private static string FormatCountdown(TimeSpan ts)
        {
            if (ts.TotalDays >= 1)
                return $"⏱  {(int)ts.TotalDays}d {ts.Hours}h {ts.Minutes}m remaining";
            if (ts.TotalHours >= 1)
                return $"⏱  {ts.Hours}h {ts.Minutes}m {ts.Seconds}s remaining";
            return $"⚠  {ts.Minutes}m {ts.Seconds}s remaining";
        }

        // CONTENT AREA ROUTER
        private void BuildContentArea()
        {
            pnlBody.Controls.Clear();

            switch (_activity.Type)
            {
                case "Essay":
                    BuildEssayView();
                    break;
                case "Quiz":
                case "LongQuiz":
                    BuildQuizView();
                    break;
                case "FileUpload":
                    BuildFileUploadView();
                    break;
                case "Recitation":
                    BuildRecitationView();
                    break;
                default:
                    BuildEssayView();
                    break;
            }
        }

        // SHARED: INSTRUCTIONS PANEL
        private Panel BuildInstructionsPanel(int width)
        {
            var pnl = new Panel
            {
                BackColor = Color.FromArgb(255, 248, 225),
                BorderStyle = BorderStyle.None,
                Width = width,
                AutoSize = true,
                MinimumSize = new Size(width, 80),
                Padding = new Padding(14, 10, 14, 10)
            };

            pnl.Paint += (s, e) =>
            {
                e.Graphics.DrawLine(new Pen(Color.FromArgb(255, 160, 0), 4), 0, 0, 0, pnl.Height);
            };

            pnl.Controls.Add(new Label
            {
                Text = "📋  Instructions",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(128, 80, 0),
                Location = new Point(18, 10),
                AutoSize = true
            });
            pnl.Controls.Add(new Label
            {
                Text = _activity.Instructions,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(60, 60, 60),
                Location = new Point(18, 34),
                Size = new Size(width - 36, 0),
                AutoSize = false,
                MaximumSize = new Size(width - 36, 200)
            });

            return pnl;
        }

        // ESSAY VIEW
        private TextBox _txtEssay;
        private Label _lblWordCount;
        private Label _lblAutosave;

        private void BuildEssayView()
        {
            bool alreadySubmitted = _activity.SubmissionStatus is "Submitted" or "Returned";
            int w = Math.Max(600, pnlBody.ClientSize.Width - 60);

            int y = 20;

            // Instructions
            var instrPnl = BuildInstructionsPanel(w);
            instrPnl.Location = new Point(20, y);
            pnlBody.Controls.Add(instrPnl);
            y += instrPnl.MinimumSize.Height + 20;

            // Prompt
            var lblPrompt = new Label
            {
                Text = "Your Essay Response:",
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 40, 40),
                Location = new Point(20, y),
                AutoSize = true
            };
            pnlBody.Controls.Add(lblPrompt);
            y += 28;

            // Essay text area
            _txtEssay = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 10F),
                Location = new Point(20, y),
                Size = new Size(w, 300),
                BackColor = alreadySubmitted ? Color.FromArgb(248, 248, 248) : Color.White,
                ReadOnly = alreadySubmitted,
                Text = _activity.EssayDraft
            };
            _txtEssay.TextChanged += (s, e) => UpdateWordCount();
            pnlBody.Controls.Add(_txtEssay);
            y += _txtEssay.Height + 6;

            // Word / char count
            _lblWordCount = new Label
            {
                Text = "Words: 0  |  Characters: 0",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.Gray,
                Location = new Point(20, y),
                AutoSize = true
            };
            pnlBody.Controls.Add(_lblWordCount);
            UpdateWordCount();
            y += 22;

            // Autosave indicator
            _lblAutosave = new Label
            {
                Text = "Draft autosaved every 30 seconds.",
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.FromArgb(0, 140, 0),
                Location = new Point(20, y),
                AutoSize = true
            };
            pnlBody.Controls.Add(_lblAutosave);
            y += 26;

            if (!alreadySubmitted)
            {
                var btnSubmit = new buttonRounded
                {
                    Text = "Submit Essay",
                    Size = new Size(148, 38),
                    Location = new Point(20, y),
                    BackColor = Color.Maroon,
                    ForeColor = Color.White,
                    BorderRadius = 19,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat
                };
                btnSubmit.FlatAppearance.BorderSize = 0;
                btnSubmit.Click += SubmitEssay_Click;
                pnlBody.Controls.Add(btnSubmit);
            }
            else
            {
                // Show returned feedback
                if (_activity.Score.HasValue)
                {
                    pnlBody.Controls.Add(new Label
                    {
                        Text = $"✔  Submitted  ·  Score: {_activity.Score}/{_activity.Points} pts",
                        Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                        ForeColor = Color.FromArgb(27, 110, 27),
                        Location = new Point(20, y),
                        AutoSize = true
                    });
                    y += 28;
                }

                if (!string.IsNullOrEmpty(_activity.Remarks))
                {
                    var pnlRemarks = new Panel
                    {
                        BackColor = Color.FromArgb(230, 235, 255),
                        Location = new Point(20, y),
                        Size = new Size(w, 0),
                        AutoSize = true,
                        Padding = new Padding(12)
                    };
                    pnlRemarks.Controls.Add(new Label
                    {
                        Text = "📝  Instructor Remarks",
                        Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                        ForeColor = Color.FromArgb(30, 50, 180),
                        Location = new Point(12, 10),
                        AutoSize = true
                    });
                    pnlRemarks.Controls.Add(new Label
                    {
                        Text = _activity.Remarks,
                        Font = new Font("Segoe UI", 9.5F),
                        ForeColor = Color.FromArgb(40, 40, 80),
                        Location = new Point(12, 34),
                        Size = new Size(w - 28, 60)
                    });
                    pnlBody.Controls.Add(pnlRemarks);
                }
            }

            StretchBodyControls(w);
        }

        private void UpdateWordCount()
        {
            if (_txtEssay == null || _lblWordCount == null) return;
            string t = _txtEssay.Text.Trim();
            int words = string.IsNullOrWhiteSpace(t) ? 0 :
                t.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
            _lblWordCount.Text = $"Words: {words}  |  Characters: {t.Length}";
        }

        private void AutosaveDraft()
        {
            if (_txtEssay == null) return;
            _activity.EssayDraft = _txtEssay.Text;
            if (_lblAutosave != null)
                _lblAutosave.Text = $"Draft autosaved at {DateTime.Now:h:mm:ss tt}";
        }

        private void SubmitEssay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtEssay?.Text))
            {
                MessageBox.Show("Please write your essay before submitting.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to submit your essay?\nThis action cannot be undone.",
                "Confirm Submission", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            _activity.EssayDraft = _txtEssay.Text;
            _activity.SubmissionStatus = "Submitted";
            _activity.SubmittedAt = DateTime.Now;

            MessageBox.Show("Essay submitted successfully!", "Submitted",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            OnBack?.Invoke();
        }

        // QUIZ / LONG QUIZ VIEW
        private void BuildQuizView()
        {
            pnlBody.Controls.Clear();

            if (_activity.Questions == null || _activity.Questions.Count == 0)
            {
                pnlBody.Controls.Add(new Label
                {
                    Text = "No questions available for this activity.",
                    Font = new Font("Segoe UI", 11F),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Location = new Point(20, 20)
                });
                return;
            }

            int w = Math.Max(600, pnlBody.ClientSize.Width - 60);
            int y = 20;

            //  Progress bar strip 
            var pnlProgress = new Panel
            {
                Location = new Point(20, y),
                Size = new Size(w, 44),
                BackColor = Color.FromArgb(245, 245, 245)
            };

            int totalQ = _activity.Questions.Count;
            int answered = _answers.Count;

            pnlProgress.Controls.Add(new Label
            {
                Text = $"Question  {_currentQ + 1}  of  {totalQ}",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 40, 40),
                Location = new Point(0, 0),
                AutoSize = true
            });
            pnlProgress.Controls.Add(new Label
            {
                Text = $"{answered}/{totalQ} answered",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.Gray,
                Location = new Point(0, 22),
                AutoSize = true
            });

            // Answered navigation dots
            int dotX = w - (totalQ * 24) - 4;
            for (int i = 0; i < totalQ; i++)
            {
                bool ans = _answers.ContainsKey(i + 1);
                bool cur = i == _currentQ;
                pnlProgress.Controls.Add(new Label
                {
                    Text = (i + 1).ToString(),
                    BackColor = cur ? Color.Maroon : ans ? Color.ForestGreen : Color.FromArgb(210, 210, 210),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 7F, FontStyle.Bold),
                    Location = new Point(dotX + i * 24, 8),
                    Size = new Size(20, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                });
            }

            pnlBody.Controls.Add(pnlProgress);
            y += pnlProgress.Height + 14;

            //  Current question card 
            var q = _activity.Questions[_currentQ];
            var pnlQ = new Panel
            {
                BackColor = Color.White,
                Location = new Point(20, y),
                Width = w,
                Height = 0,
                AutoSize = true,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(16)
            };

            int qy = 14;

            // Question type badge
            string qTypeBadge = q.QuestionType switch
            {
                "MultipleChoice" => "Multiple Choice",
                "TrueFalse" => "True or False",
                "Identification" => "Identification",
                "Essay" => "Essay",
                _ => q.QuestionType
            };
            Color badgeColor = q.QuestionType switch
            {
                "MultipleChoice" => Color.FromArgb(63, 81, 181),
                "TrueFalse" => Color.FromArgb(0, 150, 136),
                "Identification" => Color.FromArgb(156, 39, 176),
                _ => Color.Gray
            };
            pnlQ.Controls.Add(new Label
            {
                Text = qTypeBadge,
                BackColor = badgeColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Location = new Point(16, qy),
                Size = new Size(130, 20),
                TextAlign = ContentAlignment.MiddleCenter
            });

            pnlQ.Controls.Add(new Label
            {
                Text = $"{q.Points} pt{(q.Points != 1 ? "s" : "")}",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.Maroon,
                Location = new Point(w - 70, qy),
                Size = new Size(54, 20),
                TextAlign = ContentAlignment.MiddleRight
            });
            qy += 28;

            // Question text
            pnlQ.Controls.Add(new Label
            {
                Text = $"{_currentQ + 1}.  {q.Text}",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.FromArgb(25, 25, 25),
                Location = new Point(16, qy),
                Size = new Size(w - 36, 0),
                AutoSize = false,
                MaximumSize = new Size(w - 36, 300)
            });
            qy += 60;

            //  Answer area 
            string savedAnswer = _answers.ContainsKey(q.Number) ? _answers[q.Number] : "";
            bool isSubmitted = _activity.SubmissionStatus is "Submitted" or "Returned";

            if (q.QuestionType is "MultipleChoice" or "TrueFalse")
            {
                foreach (var choice in q.Choices)
                {
                    var rb = new RadioButton
                    {
                        Text = choice,
                        Font = new Font("Segoe UI", 10F),
                        ForeColor = Color.FromArgb(40, 40, 40),
                        Location = new Point(20, qy),
                        AutoSize = true,
                        Checked = (savedAnswer == choice),
                        Enabled = !isSubmitted,
                        Tag = choice
                    };
                    rb.CheckedChanged += (s, e) =>
                    {
                        if (rb.Checked) _answers[q.Number] = rb.Tag.ToString();
                    };
                    pnlQ.Controls.Add(rb);
                    qy += 32;
                }
            }
            else if (q.QuestionType == "Identification")
            {
                pnlQ.Controls.Add(new Label
                {
                    Text = "Answer:",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(60, 60, 60),
                    Location = new Point(20, qy),
                    AutoSize = true
                });
                qy += 22;

                var txtIdent = new TextBox
                {
                    Font = new Font("Segoe UI", 10F),
                    Location = new Point(20, qy),
                    Size = new Size(Math.Min(400, w - 36), 28),
                    Text = savedAnswer,
                    ReadOnly = isSubmitted,
                    PlaceholderText = "Type your answer here..."
                };
                txtIdent.TextChanged += (s, e) => _answers[q.Number] = txtIdent.Text;
                pnlQ.Controls.Add(txtIdent);
                qy += 38;
            }
            else // Essay-type question
            {
                pnlQ.Controls.Add(new Label
                {
                    Text = "Your Answer:",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(60, 60, 60),
                    Location = new Point(20, qy),
                    AutoSize = true
                });
                qy += 22;

                var txtEssQ = new TextBox
                {
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    Font = new Font("Segoe UI", 10F),
                    Location = new Point(20, qy),
                    Size = new Size(w - 36, 150),
                    Text = savedAnswer,
                    ReadOnly = isSubmitted
                };
                txtEssQ.TextChanged += (s, e) => _answers[q.Number] = txtEssQ.Text;
                pnlQ.Controls.Add(txtEssQ);
                qy += 160;
            }

            pnlQ.Height = qy + 10;
            pnlBody.Controls.Add(pnlQ);
            y += pnlQ.Height + 20;

            //  Navigation buttons 
            var pnlNav = new Panel
            {
                Location = new Point(20, y),
                Size = new Size(w, 48)
            };

            if (_currentQ > 0)
            {
                var btnPrev = new buttonRounded
                {
                    Text = "← Previous",
                    Size = new Size(120, 36),
                    Location = new Point(0, 6),
                    BackColor = Color.FromArgb(70, 70, 70),
                    ForeColor = Color.White,
                    BorderRadius = 18,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat
                };
                btnPrev.FlatAppearance.BorderSize = 0;
                btnPrev.Click += (s, e) => { _currentQ--; BuildQuizView(); };
                pnlNav.Controls.Add(btnPrev);
            }

            if (_currentQ < totalQ - 1)
            {
                var btnNext = new buttonRounded
                {
                    Text = "Next →",
                    Size = new Size(120, 36),
                    Location = new Point(130, 6),
                    BackColor = Color.Maroon,
                    ForeColor = Color.White,
                    BorderRadius = 18,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat
                };
                btnNext.FlatAppearance.BorderSize = 0;
                btnNext.Click += (s, e) => { _currentQ++; BuildQuizView(); };
                pnlNav.Controls.Add(btnNext);
            }
            else if (!isSubmitted)
            {
                var btnSubmit = new buttonRounded
                {
                    Text = "Submit Quiz",
                    Size = new Size(140, 36),
                    Location = new Point(130, 6),
                    BackColor = Color.FromArgb(27, 110, 27),
                    ForeColor = Color.White,
                    BorderRadius = 18,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat
                };
                btnSubmit.FlatAppearance.BorderSize = 0;
                btnSubmit.Click += SubmitQuiz_Click;
                pnlNav.Controls.Add(btnSubmit);
            }

            pnlBody.Controls.Add(pnlNav);
        }

        private void SubmitQuiz_Click(object sender, EventArgs e)
        {
            int unanswered = 0;
            foreach (var q in _activity.Questions)
                if (!_answers.ContainsKey(q.Number) || string.IsNullOrWhiteSpace(_answers[q.Number]))
                    unanswered++;

            string warnMsg = unanswered > 0
                ? $"You have {unanswered} unanswered question(s).\n\n"
                : "";

            var result = MessageBox.Show(
                warnMsg + "Submit your quiz now? This cannot be undone.",
                "Confirm Submission", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            _activity.Answers = new Dictionary<int, string>(_answers);
            _activity.SubmissionStatus = "Submitted";
            _activity.SubmittedAt = DateTime.Now;

            MessageBox.Show("Quiz submitted successfully!", "Submitted",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            OnBack?.Invoke();
        }

        // FILE UPLOAD VIEW
        private Label _lblFileName;

        private void BuildFileUploadView()
        {
            bool isSubmitted = _activity.SubmissionStatus is "Submitted" or "Returned";
            int w = Math.Max(600, pnlBody.ClientSize.Width - 60);
            int y = 20;

            // Instructions
            var instrPnl = BuildInstructionsPanel(w);
            instrPnl.Location = new Point(20, y);
            pnlBody.Controls.Add(instrPnl);
            y += instrPnl.MinimumSize.Height + 20;

            // Attachment references (downloadable)
            if (_activity.Attachments?.Count > 0)
            {
                pnlBody.Controls.Add(new Label
                {
                    Text = "📎  Reference Files:",
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(40, 40, 40),
                    Location = new Point(20, y),
                    AutoSize = true
                });
                y += 22;

                foreach (var att in _activity.Attachments)
                {
                    var lnk = new LinkLabel
                    {
                        Text = $"📄  {att.FileName}",
                        Font = new Font("Segoe UI", 9.5F),
                        Location = new Point(30, y),
                        AutoSize = true,
                        Tag = att.FilePath
                    };
                    lnk.Click += (s, e) => MessageBox.Show($"Opening: {att.FileName}", "File",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    pnlBody.Controls.Add(lnk);
                    y += 24;
                }
                y += 10;
            }

            if (!isSubmitted)
            {
                pnlBody.Controls.Add(new Label
                {
                    Text = "Attach Your File:",
                    Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(40, 40, 40),
                    Location = new Point(20, y),
                    AutoSize = true
                });
                y += 28;

                // Drop zone
                var pnlDrop = new Panel
                {
                    BackColor = Color.FromArgb(245, 248, 255),
                    BorderStyle = BorderStyle.FixedSingle,
                    Location = new Point(20, y),
                    Size = new Size(w, 90)
                };

                var lblHint = new Label
                {
                    Text = "Drag & drop your file here  —  or  —  click Browse",
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = Color.Gray,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                var btnBrowse = new buttonRounded
                {
                    Text = "Browse",
                    Size = new Size(90, 30),
                    Location = new Point(w - 100, 28),
                    BackColor = Color.Maroon,
                    ForeColor = Color.White,
                    BorderRadius = 15,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat
                };
                btnBrowse.FlatAppearance.BorderSize = 0;
                btnBrowse.Click += BrowseFile_Click;
                pnlDrop.Controls.Add(lblHint);
                pnlDrop.Controls.Add(btnBrowse);
                btnBrowse.BringToFront();
                pnlBody.Controls.Add(pnlDrop);
                y += 100;

                // Selected file name
                _lblFileName = new Label
                {
                    Text = string.IsNullOrEmpty(_uploadedFileName)
                                ? "No file selected." : $"📎  {_uploadedFileName}",
                    Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                    ForeColor = string.IsNullOrEmpty(_uploadedFileName)
                                ? Color.Gray : Color.FromArgb(0, 100, 0),
                    Location = new Point(20, y),
                    AutoSize = true
                };
                pnlBody.Controls.Add(_lblFileName);
                y += 26;

                // Remove file button
                var btnRemove = new buttonRounded
                {
                    Text = "✕  Remove File",
                    Size = new Size(120, 28),
                    Location = new Point(20, y),
                    BackColor = Color.FromArgb(180, 30, 30),
                    ForeColor = Color.White,
                    BorderRadius = 14,
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat,
                    Visible = !string.IsNullOrEmpty(_uploadedFileName)
                };
                btnRemove.FlatAppearance.BorderSize = 0;
                btnRemove.Click += (s, e) =>
                {
                    _uploadedFilePath = "";
                    _uploadedFileName = "";
                    if (_lblFileName != null)
                    {
                        _lblFileName.Text = "No file selected.";
                        _lblFileName.ForeColor = Color.Gray;
                    }
                    btnRemove.Visible = false;
                };
                pnlBody.Controls.Add(btnRemove);
                y += 40;

                // Notes
                pnlBody.Controls.Add(new Label
                {
                    Text = "Submission Notes (optional):",
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(40, 40, 40),
                    Location = new Point(20, y),
                    AutoSize = true
                });
                y += 24;

                var txtNotes = new TextBox
                {
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    Font = new Font("Segoe UI", 10F),
                    Location = new Point(20, y),
                    Size = new Size(w, 90),
                    PlaceholderText = "Add a note to your instructor (optional)..."
                };
                txtNotes.TextChanged += (s, e) => _activity.SubmissionNote = txtNotes.Text;
                pnlBody.Controls.Add(txtNotes);
                y += 100;

                var btnUpload = new buttonRounded
                {
                    Text = "Upload & Submit",
                    Size = new Size(160, 38),
                    Location = new Point(20, y),
                    BackColor = Color.Maroon,
                    ForeColor = Color.White,
                    BorderRadius = 19,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat
                };
                btnUpload.FlatAppearance.BorderSize = 0;
                btnUpload.Click += SubmitFileUpload_Click;
                pnlBody.Controls.Add(btnUpload);
            }
            else
            {
                // Already submitted
                pnlBody.Controls.Add(new Label
                {
                    Text = $"✔  File Submitted  ·  {_activity.SubmittedAt:MMM dd, yyyy  h:mm tt}",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(27, 110, 27),
                    Location = new Point(20, y),
                    AutoSize = true
                });
                y += 30;

                if (!string.IsNullOrEmpty(_activity.UploadedFileName))
                {
                    pnlBody.Controls.Add(new Label
                    {
                        Text = $"📎  {_activity.UploadedFileName}",
                        Font = new Font("Segoe UI", 9.5F),
                        ForeColor = Color.FromArgb(0, 100, 160),
                        Location = new Point(20, y),
                        AutoSize = true
                    });
                    y += 26;
                }

                if (_activity.Score.HasValue)
                {
                    pnlBody.Controls.Add(new Label
                    {
                        Text = $"Score:  {_activity.Score} / {_activity.Points} pts",
                        Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                        ForeColor = Color.FromArgb(128, 0, 0),
                        Location = new Point(20, y),
                        AutoSize = true
                    });
                    y += 28;
                }

                if (!string.IsNullOrEmpty(_activity.Remarks))
                {
                    var pnlR = new Panel
                    {
                        BackColor = Color.FromArgb(230, 235, 255),
                        Location = new Point(20, y),
                        Size = new Size(w, 90),
                        Padding = new Padding(12)
                    };
                    pnlR.Controls.Add(new Label
                    {
                        Text = "📝  Instructor Remarks:",
                        Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                        ForeColor = Color.FromArgb(30, 50, 180),
                        Location = new Point(12, 10),
                        AutoSize = true
                    });
                    pnlR.Controls.Add(new Label
                    {
                        Text = _activity.Remarks,
                        Font = new Font("Segoe UI", 9.5F),
                        ForeColor = Color.FromArgb(40, 40, 80),
                        Location = new Point(12, 34),
                        Size = new Size(w - 28, 50)
                    });
                    pnlBody.Controls.Add(pnlR);
                }
            }
        }

        private void BrowseFile_Click(object sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog
            {
                Filter = "All Files (*.*)|*.*|PDF Files (*.pdf)|*.pdf|Word Documents (*.docx)|*.docx|ZIP Files (*.zip)|*.zip",
                Title = "Select File to Upload"
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _uploadedFilePath = dlg.FileName;
                _uploadedFileName = Path.GetFileName(dlg.FileName);
                _activity.UploadedFileName = _uploadedFileName;

                if (_lblFileName != null)
                {
                    _lblFileName.Text = $"📎  {_uploadedFileName}";
                    _lblFileName.ForeColor = Color.FromArgb(0, 100, 0);
                }
            }
        }

        private void SubmitFileUpload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_uploadedFileName))
            {
                MessageBox.Show("Please select a file before submitting.",
                    "No File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Submit \"{_uploadedFileName}\"?\n\nThis action cannot be undone.",
                "Confirm Upload", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            _activity.SubmissionStatus = "Submitted";
            _activity.SubmittedAt = DateTime.Now;

            MessageBox.Show("File uploaded and submitted successfully!", "Submitted",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            OnBack?.Invoke();
        }

        // RECITATION VIEW
        private void BuildRecitationView()
        {
            bool isSubmitted = _activity.SubmissionStatus is "Submitted" or "Returned";
            int w = Math.Max(600, pnlBody.ClientSize.Width - 60);
            int y = 20;

            var instrPnl = BuildInstructionsPanel(w);
            instrPnl.Location = new Point(20, y);
            pnlBody.Controls.Add(instrPnl);
            y += instrPnl.MinimumSize.Height + 20;

            pnlBody.Controls.Add(new Label
            {
                Text = "Participation / Attendance Notes (optional):",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 40, 40),
                Location = new Point(20, y),
                AutoSize = true
            });
            y += 24;

            var txtNotes = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 10F),
                Location = new Point(20, y),
                Size = new Size(w, 120),
                PlaceholderText = "Any notes for your instructor...",
                ReadOnly = isSubmitted
            };
            pnlBody.Controls.Add(txtNotes);
            y += 130;

            if (!isSubmitted)
            {
                var btnMark = new buttonRounded
                {
                    Text = "Mark as Attended",
                    Size = new Size(160, 38),
                    Location = new Point(20, y),
                    BackColor = Color.Maroon,
                    ForeColor = Color.White,
                    BorderRadius = 19,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat
                };
                btnMark.FlatAppearance.BorderSize = 0;
                btnMark.Click += (s, e) =>
                {
                    _activity.SubmissionStatus = "Submitted";
                    _activity.SubmittedAt = DateTime.Now;
                    MessageBox.Show("Marked as attended!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OnBack?.Invoke();
                };
                pnlBody.Controls.Add(btnMark);
            }
        }

        // HELPERS
        private void StretchBodyControls(int targetWidth)
        {
            foreach (Control c in pnlBody.Controls)
            {
                if (c is Button || c is buttonRounded) continue;
                if (c.Width < targetWidth) c.Width = targetWidth;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            AutosaveDraft();
            OnBack?.Invoke();
        }
    }
}