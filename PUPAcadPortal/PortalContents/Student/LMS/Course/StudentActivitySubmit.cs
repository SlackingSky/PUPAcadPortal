using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Student.LMS.Course
{
    public partial class StudentActivitySubmit : UserControl
    {
        public event Action OnBack;

        private readonly StudentActivityItem _activity;
        private readonly StudentCourse _course;

        // Quiz navigation state
        private int _currentQ = 0;
        private Dictionary<int, string> _answers = new();

        // Timers declared here so Designer Dispose() can reach them
        private System.Windows.Forms.Timer _countdownTimer;
        private System.Windows.Forms.Timer _autosaveTimer;

        // File-upload runtime state
        private string _uploadedFilePath = "";
        private string _uploadedFileName = "";
        private long _uploadedFileSize = 0;

        // Dynamic label / control references
        private Label _lblDeadlineTimer;
        private TextBox _txtEssay;
        private Label _lblWordCount;
        private Label _lblAutosave;
        private Label _lblFileName;
        private Label _lblFileSize;
        private buttonRounded _btnRemoveFile;

        public StudentActivitySubmit(StudentActivityItem activity, StudentCourse course)
        {
            _activity = activity;
            _course = course;
            _answers = new Dictionary<int, string>(activity.Answers ?? new());

            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            BuildHeader();
            BuildDeadlineBar();
            BuildContentArea();

            if (_activity.Type == "Essay")
            {
                _autosaveTimer = new System.Windows.Forms.Timer { Interval = 30_000 };
                _autosaveTimer.Tick += (s, e) => AutosaveDraft();
                _autosaveTimer.Start();
            }

            StartCountdown();
        }

        // ─────────────────────────────────────────────────────────────────────
        // HEADER
        // ─────────────────────────────────────────────────────────────────────

        private void BuildHeader()
        {
            lblActivityTitle.Text = _activity.Title;

            string typeDisplay = _activity.Type switch
            {
                "LongQuiz" => "Long Quiz",
                "FileUpload" => "File Upload",
                _ => _activity.Type
            };

            lblMeta.Text = $"{typeDisplay}  \u00B7  {_activity.Points} pts  \u00B7  Due {_activity.Deadline:MMM dd, yyyy  h:mm tt}";

            if (_activity.Score.HasValue)
            {
                lblMeta.Text += $"  \u00B7  Score: {_activity.Score} / {_activity.Points}";
                lblMeta.ForeColor = Color.FromArgb(255, 210, 210);
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // DEADLINE COUNTDOWN BAR
        // FIX (Image 3): The bar is added as a Dock=Top strip INSIDE pnlBody
        //   instead of floating over it, so nothing overlaps.
        // ─────────────────────────────────────────────────────────────────────

        private void BuildDeadlineBar()
        {
            TimeSpan remaining = _activity.Deadline - DateTime.Now;
            bool isLate = remaining.TotalSeconds <= 0;

            var pnlBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 34,
                BackColor = isLate
                    ? Color.FromArgb(175, 20, 20)
                    : Color.FromArgb(45, 45, 45)
            };

            _lblDeadlineTimer = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                Text = isLate
                    ? "\u26A0  Deadline has passed \u2014 late submission"
                    : FormatCountdown(remaining)
            };

            pnlBar.Controls.Add(_lblDeadlineTimer);

            // Add the bar at the TOP of pnlBody so content flows below it.
            pnlBody.Controls.Add(pnlBar);
            pnlBar.BringToFront();
        }

        private void StartCountdown()
        {
            _countdownTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _countdownTimer.Tick += (s, e) =>
            {
                if (_lblDeadlineTimer == null || _lblDeadlineTimer.IsDisposed) return;
                TimeSpan rem = _activity.Deadline - DateTime.Now;

                if (rem.TotalSeconds <= 0)
                {
                    _lblDeadlineTimer.Text = "\u26A0  Deadline has passed \u2014 late submission";
                    if (_lblDeadlineTimer.Parent is Panel p)
                        p.BackColor = Color.FromArgb(175, 20, 20);
                    _countdownTimer.Stop();
                }
                else
                {
                    _lblDeadlineTimer.Text = FormatCountdown(rem);
                    if (_lblDeadlineTimer.Parent is Panel p)
                        p.BackColor = rem.TotalHours < 1
                            ? Color.FromArgb(175, 80, 0)
                            : Color.FromArgb(45, 45, 45);
                }
            };
            _countdownTimer.Start();
        }

        private static string FormatCountdown(TimeSpan ts)
        {
            if (ts.TotalDays >= 1) return $"\u23F1  {(int)ts.TotalDays}d {ts.Hours}h {ts.Minutes}m remaining";
            if (ts.TotalHours >= 1) return $"\u23F1  {ts.Hours}h {ts.Minutes}m {ts.Seconds}s remaining";
            return $"\u26A0  {ts.Minutes}m {ts.Seconds}s remaining \u2014 hurry!";
        }

        // ─────────────────────────────────────────────────────────────────────
        // CONTENT ROUTER
        // FIX (Image 3): Content is added to a scrollable inner Panel that
        //   sits BELOW the deadline bar, so nothing overlaps.
        // ─────────────────────────────────────────────────────────────────────

        // The scrollable area where each view adds its controls.
        private Panel _scrollArea;

        private void BuildContentArea()
        {
            // Create a scroll area that fills the rest of pnlBody (below the deadline bar)
            _scrollArea = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(0)
            };
            pnlBody.Controls.Add(_scrollArea);
            _scrollArea.SendToBack(); // deadline bar (Dock=Top, BringToFront) stays on top

            switch (_activity.Type)
            {
                case "Essay": BuildEssayView(); break;
                case "Quiz": case "LongQuiz": BuildQuizView(); break;
                case "FileUpload": BuildFileUploadView(); break;
                case "Recitation": BuildRecitationView(); break;
                default: BuildEssayView(); break;
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // SHARED helpers — now add controls to _scrollArea, not pnlBody
        // ─────────────────────────────────────────────────────────────────────

        private Panel BuildInstructionsPanel(int width)
        {
            var pnl = new Panel
            {
                BackColor = Color.FromArgb(255, 248, 224),
                Width = width,
                AutoSize = true,
                MinimumSize = new Size(width, 80),
                Padding = new Padding(18, 12, 14, 12)
            };
            pnl.Paint += (s, e) =>
                e.Graphics.DrawLine(new Pen(Color.FromArgb(255, 165, 0), 5), 0, 0, 0, pnl.Height);

            pnl.Controls.Add(new Label
            {
                Text = "\uD83D\uDCCB  Instructions",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(120, 75, 0),
                Location = new Point(22, 12),
                AutoSize = true
            });
            pnl.Controls.Add(new Label
            {
                Text = _activity.Instructions,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(55, 55, 55),
                Location = new Point(22, 34),
                MaximumSize = new Size(width - 38, 0),
                AutoSize = true
            });
            return pnl;
        }

        private int BuildAttachmentList(int startY, int w)
        {
            if (_activity.Attachments == null || _activity.Attachments.Count == 0)
                return startY;

            int y = startY;

            _scrollArea.Controls.Add(new Label
            {
                Text = "Attached Files",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 40, 40),
                Location = new Point(20, y),
                AutoSize = true
            }); y += 24;

            foreach (var att in _activity.Attachments)
            {
                string ext = Path.GetExtension(att.FileName).ToLowerInvariant();
                string icon = ext switch
                {
                    ".pdf" => "\uD83D\uDCC4",
                    ".docx" => "\uD83D\uDCDD",
                    ".doc" => "\uD83D\uDCDD",
                    ".pptx" => "\uD83D\uDCCA",
                    ".ppt" => "\uD83D\uDCCA",
                    ".png" => "\uD83D\uDDBC",
                    ".jpg" => "\uD83D\uDDBC",
                    ".jpeg" => "\uD83D\uDDBC",
                    _ => "\uD83D\uDCC1"
                };

                string sizeStr = att.FileSize > 0
                    ? (att.FileSize >= 1_048_576
                        ? $"{att.FileSize / 1_048_576.0:F1} MB"
                        : att.FileSize >= 1_024
                            ? $"{att.FileSize / 1_024.0:F0} KB"
                            : $"{att.FileSize} B")
                    : "";

                int cardW = Math.Min(480, w - 20);
                var pnlAtt = new Panel
                {
                    BackColor = Color.White,
                    Location = new Point(20, y),
                    Size = new Size(cardW, 48),
                    Cursor = Cursors.Hand,
                    BorderStyle = BorderStyle.None
                };
                pnlAtt.Paint += (s, e) =>
                {
                    using var pen = new Pen(Color.FromArgb(218, 222, 240));
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlAtt.Width - 1, pnlAtt.Height - 1);
                };

                var pnlIcon = new Panel
                {
                    Location = new Point(0, 0),
                    Size = new Size(44, 48),
                    BackColor = Color.FromArgb(240, 242, 255)
                };
                pnlIcon.Controls.Add(new Label
                {
                    Text = icon,
                    Font = new Font("Segoe UI", 14F),
                    Location = new Point(4, 6),
                    Size = new Size(34, 34),
                    TextAlign = ContentAlignment.MiddleCenter
                });
                pnlAtt.Controls.Add(pnlIcon);

                pnlAtt.Controls.Add(new Label
                {
                    Text = att.FileName,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(30, 60, 160),
                    Location = new Point(52, 8),
                    Size = new Size(cardW - 130, 18),
                    AutoEllipsis = true
                });

                string hint = string.IsNullOrEmpty(sizeStr)
                    ? "Click to download"
                    : $"{sizeStr}  \u2014  Click to download";

                pnlAtt.Controls.Add(new Label
                {
                    Text = hint,
                    Font = new Font("Segoe UI", 7.5F),
                    ForeColor = Color.Gray,
                    Location = new Point(52, 27),
                    AutoSize = true
                });

                var attCapture = att;
                void OpenFile(object s2, EventArgs e2)
                {
                    if (!string.IsNullOrEmpty(attCapture.FilePath) && File.Exists(attCapture.FilePath))
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        { FileName = attCapture.FilePath, UseShellExecute = true });
                    else
                        MessageBox.Show($"File: {attCapture.FileName}",
                            "Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                pnlAtt.Click += OpenFile;
                pnlIcon.Click += OpenFile;
                foreach (Control c in pnlAtt.Controls) c.Click += OpenFile;
                foreach (Control c in pnlIcon.Controls) c.Click += OpenFile;

                _scrollArea.Controls.Add(pnlAtt);
                y += 54;
            }
            return y + 8;
        }

        private Panel BuildRemarksPanel(int w, int y)
        {
            var pnl = new Panel
            {
                BackColor = Color.FromArgb(232, 238, 255),
                Location = new Point(20, y),
                Size = new Size(w, 100),
                Padding = new Padding(14)
            };
            pnl.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(190, 205, 240));
                e.Graphics.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };
            pnl.Controls.Add(new Label
            {
                Text = "\uD83D\uDCDD  Instructor Remarks:",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 50, 160),
                Location = new Point(14, 10),
                AutoSize = true
            });
            pnl.Controls.Add(new Label
            {
                Text = _activity.Remarks,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(35, 35, 80),
                Location = new Point(14, 34),
                Size = new Size(w - 34, 56)
            });
            return pnl;
        }

        // ══════════════════════════════════════════════════════════════════════
        // ESSAY VIEW
        // FIX (Image 3): All controls added to _scrollArea with correct Y.
        // ══════════════════════════════════════════════════════════════════════

        private void BuildEssayView()
        {
            bool submitted = _activity.SubmissionStatus is "Submitted" or "Returned";
            int w = Math.Max(640, _scrollArea.ClientSize.Width > 0
                                  ? _scrollArea.ClientSize.Width - 60
                                  : pnlBody.ClientSize.Width - 60);
            int y = 20;

            var instrPnl = BuildInstructionsPanel(w);
            instrPnl.Location = new Point(20, y);
            _scrollArea.Controls.Add(instrPnl);
            y += instrPnl.MinimumSize.Height + 20;

            if (submitted)
            {
                _scrollArea.Controls.Add(new Label
                {
                    Text = $"\u2714  Essay Submitted  \u00B7  {_activity.SubmittedAt:MMM dd, yyyy  h:mm tt}",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(27, 110, 27),
                    Location = new Point(20, y),
                    AutoSize = true
                }); y += 32;

                if (_activity.Score.HasValue)
                {
                    _scrollArea.Controls.Add(new Label
                    {
                        Text = $"Score:  {_activity.Score} / {_activity.Points} pts",
                        Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                        ForeColor = Color.Maroon,
                        Location = new Point(20, y),
                        AutoSize = true
                    }); y += 32;
                }

                _scrollArea.Controls.Add(new Label
                {
                    Text = "Submitted Response:",
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(40, 40, 40),
                    Location = new Point(20, y),
                    AutoSize = true
                }); y += 22;

                _txtEssay = new TextBox
                {
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    Font = new Font("Segoe UI", 10.5F),
                    Location = new Point(20, y),
                    Size = new Size(w, 260),
                    BackColor = Color.FromArgb(248, 248, 248),
                    ReadOnly = true,
                    Text = _activity.EssayDraft,
                    BorderStyle = BorderStyle.FixedSingle
                };
                _scrollArea.Controls.Add(_txtEssay);
                y += _txtEssay.Height + 18;

                if (!string.IsNullOrEmpty(_activity.Remarks))
                    _scrollArea.Controls.Add(BuildRemarksPanel(w, y));
            }
            else
            {
                _scrollArea.Controls.Add(new Label
                {
                    Text = "Your Essay Response:",
                    Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(35, 35, 35),
                    Location = new Point(20, y),
                    AutoSize = true
                }); y += 28;

                _txtEssay = new TextBox
                {
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    Font = new Font("Segoe UI", 10.5F),
                    Location = new Point(20, y),
                    Size = new Size(w, 320),
                    BackColor = Color.White,
                    Text = _activity.EssayDraft,
                    BorderStyle = BorderStyle.FixedSingle
                };
                _txtEssay.TextChanged += (s, e) => UpdateWordCount();
                _scrollArea.Controls.Add(_txtEssay);
                y += _txtEssay.Height + 6;

                _lblWordCount = new Label
                {
                    Font = new Font("Segoe UI", 8.5F),
                    ForeColor = Color.Gray,
                    Location = new Point(20, y),
                    AutoSize = true
                };
                _scrollArea.Controls.Add(_lblWordCount);
                UpdateWordCount();
                y += 22;

                _lblAutosave = new Label
                {
                    Text = "\uD83D\uDCBE  Draft autosaved every 30 seconds.",
                    Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                    ForeColor = Color.FromArgb(0, 135, 0),
                    Location = new Point(20, y),
                    AutoSize = true
                };
                _scrollArea.Controls.Add(_lblAutosave);
                y += 26;

                var btnSub = new buttonRounded
                {
                    Text = "Submit Essay  \u2714",
                    Size = new Size(164, 40),
                    Location = new Point(20, y),
                    BackColor = Color.Maroon,
                    ForeColor = Color.White,
                    BorderRadius = 20,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat
                };
                btnSub.FlatAppearance.BorderSize = 0;
                btnSub.Click += SubmitEssay_Click;
                _scrollArea.Controls.Add(btnSub);
            }
        }

        private void UpdateWordCount()
        {
            if (_txtEssay == null || _lblWordCount == null) return;
            string t = _txtEssay.Text.Trim();
            int words = string.IsNullOrWhiteSpace(t) ? 0
                : t.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
            _lblWordCount.Text = $"Words: {words}  |  Characters: {t.Length}";
        }

        private void AutosaveDraft()
        {
            if (_txtEssay == null || _txtEssay.IsDisposed) return;
            _activity.EssayDraft = _txtEssay.Text;
            if (_lblAutosave != null && !_lblAutosave.IsDisposed)
                _lblAutosave.Text = $"\uD83D\uDCBE  Draft autosaved at {DateTime.Now:h:mm:ss tt}";
        }

        private void SubmitEssay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtEssay?.Text))
            {
                MessageBox.Show("Please write your essay before submitting.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var r = MessageBox.Show("Submit your essay now? This action cannot be undone.",
                "Confirm Submission", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            _activity.EssayDraft = _txtEssay.Text;
            _activity.SubmissionStatus = "Submitted";
            _activity.SubmittedAt = DateTime.Now;

            MessageBox.Show("Essay submitted successfully! \u2714", "Submitted",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            OnBack?.Invoke();
        }

        // ══════════════════════════════════════════════════════════════════════
        // FILE UPLOAD VIEW
        // FIX (Image 3): All controls added to _scrollArea with correct Y.
        // ══════════════════════════════════════════════════════════════════════

        private void BuildFileUploadView()
        {
            bool isSubm = _activity.SubmissionStatus is "Submitted" or "Returned";
            int w = Math.Max(640, _scrollArea.ClientSize.Width > 0
                                  ? _scrollArea.ClientSize.Width - 60
                                  : pnlBody.ClientSize.Width - 60);
            int y = 20;

            var instrPnl = BuildInstructionsPanel(w);
            instrPnl.Location = new Point(20, y);
            _scrollArea.Controls.Add(instrPnl);
            y += instrPnl.MinimumSize.Height + 20;

            y = BuildAttachmentList(y, w);

            if (isSubm)
            {
                _scrollArea.Controls.Add(new Label
                {
                    Text = $"\u2714  File Submitted  \u00B7  {_activity.SubmittedAt:MMM dd, yyyy  h:mm tt}",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(27, 110, 27),
                    Location = new Point(20, y),
                    AutoSize = true
                }); y += 32;

                if (_activity.Score.HasValue)
                {
                    _scrollArea.Controls.Add(new Label
                    {
                        Text = $"Score:  {_activity.Score} / {_activity.Points} pts",
                        Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                        ForeColor = Color.Maroon,
                        Location = new Point(20, y),
                        AutoSize = true
                    }); y += 32;
                }

                if (!string.IsNullOrEmpty(_activity.Remarks))
                    _scrollArea.Controls.Add(BuildRemarksPanel(w, y));
            }
            else
            {
                _scrollArea.Controls.Add(new Label
                {
                    Text = "Attach Your Submission:",
                    Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(35, 35, 35),
                    Location = new Point(20, y),
                    AutoSize = true
                }); y += 28;

                var pnlDrop = new Panel
                {
                    BackColor = Color.FromArgb(244, 248, 255),
                    BorderStyle = BorderStyle.FixedSingle,
                    Location = new Point(20, y),
                    Size = new Size(w, 100)
                };
                var lblHint = new Label
                {
                    Text = "Drag & drop your file here  \u2014  or  \u2014  click  Browse",
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = Color.FromArgb(130, 130, 155),
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                var btnBrowse = new buttonRounded
                {
                    Text = "Browse",
                    Size = new Size(96, 32),
                    Location = new Point(w - 106, 32),
                    BackColor = Color.Maroon,
                    ForeColor = Color.White,
                    BorderRadius = 16,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat
                };
                btnBrowse.FlatAppearance.BorderSize = 0;
                btnBrowse.Click += BrowseFile_Click;
                pnlDrop.Controls.Add(lblHint);
                pnlDrop.Controls.Add(btnBrowse);
                btnBrowse.BringToFront();
                _scrollArea.Controls.Add(pnlDrop);
                y += 110;

                _lblFileName = new Label
                {
                    Text = string.IsNullOrEmpty(_uploadedFileName)
                                    ? "No file selected." : $"\uD83D\uDCCE  {_uploadedFileName}",
                    Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                    ForeColor = string.IsNullOrEmpty(_uploadedFileName)
                                    ? Color.Gray : Color.FromArgb(0, 105, 0),
                    Location = new Point(20, y),
                    AutoSize = true
                };
                _scrollArea.Controls.Add(_lblFileName);

                _lblFileSize = new Label
                {
                    Text = _uploadedFileSize > 0 ? $"  ({FormatFileSize(_uploadedFileSize)})" : "",
                    Font = new Font("Segoe UI", 8.5F),
                    ForeColor = Color.Gray,
                    Location = new Point(220, y),
                    AutoSize = true
                };
                _scrollArea.Controls.Add(_lblFileSize);
                y += 26;

                _btnRemoveFile = new buttonRounded
                {
                    Text = "\u2715  Remove",
                    Size = new Size(104, 28),
                    Location = new Point(20, y),
                    BackColor = Color.FromArgb(175, 30, 30),
                    ForeColor = Color.White,
                    BorderRadius = 14,
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat,
                    Visible = !string.IsNullOrEmpty(_uploadedFileName)
                };
                _btnRemoveFile.FlatAppearance.BorderSize = 0;
                _btnRemoveFile.Click += (s, e) =>
                {
                    _uploadedFilePath = ""; _uploadedFileName = ""; _uploadedFileSize = 0;
                    if (_lblFileName != null) { _lblFileName.Text = "No file selected."; _lblFileName.ForeColor = Color.Gray; }
                    if (_lblFileSize != null) _lblFileSize.Text = "";
                    _btnRemoveFile.Visible = false;
                };
                _scrollArea.Controls.Add(_btnRemoveFile);
                y += 40;

                _scrollArea.Controls.Add(new Label
                {
                    Text = "Submission Notes  (optional):",
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(40, 40, 40),
                    Location = new Point(20, y),
                    AutoSize = true
                }); y += 24;

                var txtNotes = new TextBox
                {
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    Font = new Font("Segoe UI", 10F),
                    Location = new Point(20, y),
                    Size = new Size(w, 90),
                    PlaceholderText = "Add a note to your instructor (optional)...",
                    Text = _activity.SubmissionNote
                };
                txtNotes.TextChanged += (s, e) => _activity.SubmissionNote = txtNotes.Text;
                _scrollArea.Controls.Add(txtNotes);
                y += 100;

                var btnUpload = new buttonRounded
                {
                    Text = "Upload & Submit  \u2714",
                    Size = new Size(180, 40),
                    Location = new Point(20, y),
                    BackColor = Color.Maroon,
                    ForeColor = Color.White,
                    BorderRadius = 20,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat
                };
                btnUpload.FlatAppearance.BorderSize = 0;
                btnUpload.Click += SubmitFileUpload_Click;
                _scrollArea.Controls.Add(btnUpload);
            }
        }

        private void BrowseFile_Click(object sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog
            {
                Filter = "All Files (*.*)|*.*|PDF (*.pdf)|*.pdf|Word (*.docx)|*.docx|PowerPoint (*.pptx)|*.pptx|ZIP (*.zip)|*.zip|Images (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg",
                Title = "Select File to Upload"
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _uploadedFilePath = dlg.FileName;
                _uploadedFileName = Path.GetFileName(dlg.FileName);
                _uploadedFileSize = new FileInfo(dlg.FileName).Length;
                _activity.UploadedFileName = _uploadedFileName;

                if (_lblFileName != null) { _lblFileName.Text = $"\uD83D\uDCCE  {_uploadedFileName}"; _lblFileName.ForeColor = Color.FromArgb(0, 105, 0); }
                if (_lblFileSize != null) _lblFileSize.Text = $"  ({FormatFileSize(_uploadedFileSize)})";
                if (_btnRemoveFile != null) _btnRemoveFile.Visible = true;
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
            var r = MessageBox.Show($"Submit \"{_uploadedFileName}\"?\n\nThis action cannot be undone.",
                "Confirm Upload", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            _activity.SubmissionStatus = "Submitted";
            _activity.SubmittedAt = DateTime.Now;

            MessageBox.Show("File uploaded and submitted successfully! \u2714", "Submitted",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            OnBack?.Invoke();
        }

        private static string FormatFileSize(long bytes)
        {
            if (bytes >= 1_048_576) return $"{bytes / 1_048_576.0:F1} MB";
            if (bytes >= 1_024) return $"{bytes / 1_024.0:F0} KB";
            return $"{bytes} B";
        }

        // ══════════════════════════════════════════════════════════════════════
        // QUIZ / LONG QUIZ VIEW  (Image 4 & 5)
        // FIX: Both Quiz AND LongQuiz use the same "all questions on one page"
        //      maroon-card design shown in Image 5.
        // ══════════════════════════════════════════════════════════════════════

        private void BuildQuizView()
        {
            _scrollArea.Controls.Clear();

            if (_activity.Questions == null || _activity.Questions.Count == 0)
            {
                _scrollArea.Controls.Add(new Label
                {
                    Text = "No questions available.",
                    Font = new Font("Segoe UI", 11F),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Location = new Point(20, 20)
                });
                return;
            }

            bool isSubm = _activity.SubmissionStatus is "Submitted" or "Returned";
            int totalQ = _activity.Questions.Count;
            int w = Math.Max(640, _scrollArea.ClientSize.Width > 0
                                         ? _scrollArea.ClientSize.Width - 40
                                         : pnlBody.ClientSize.Width - 40);
            int y = 16;

            // ── Instructions banner ────────────────────────────────────────
            if (!string.IsNullOrEmpty(_activity.Instructions))
            {
                var instrPnl = BuildInstructionsPanel(w);
                instrPnl.Location = new Point(16, y);
                _scrollArea.Controls.Add(instrPnl);
                y += instrPnl.MinimumSize.Height + 16;
            }

            // ── Question number pills ──────────────────────────────────────
            int pillSize = 28;
            int pillGap = 4;
            var pnlPills = new Panel
            {
                Location = new Point(16, y),
                Size = new Size(w, pillSize + 8),
                BackColor = Color.Transparent
            };
            for (int i = 0; i < totalQ; i++)
            {
                bool ans = _answers.ContainsKey(i + 1);
                int ci = i;
                var dot = new Label
                {
                    Text = (i + 1).ToString(),
                    BackColor = ans ? Color.FromArgb(27, 110, 27) : Color.FromArgb(200, 200, 200),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    Location = new Point(i * (pillSize + pillGap), 4),
                    Size = new Size(pillSize, pillSize),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Cursor = Cursors.Default
                };
                pnlPills.Controls.Add(dot);
            }
            _scrollArea.Controls.Add(pnlPills);
            y += pnlPills.Height + 12;

            // ── Render every question as a maroon card (Image 5 style) ─────
            for (int qi = 0; qi < totalQ; qi++)
            {
                var q = _activity.Questions[qi];
                int questionY = 0;

                // Outer card
                var card = new Panel
                {
                    Location = new Point(16, y),
                    Width = w,
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.None
                };
                card.Paint += (s, e) =>
                {
                    using var pen = new Pen(Color.FromArgb(100, 0, 0), 2);
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                };

                // Question number header (dark maroon bar)
                var pnlQNum = new Panel
                {
                    Location = new Point(0, 0),
                    Size = new Size(w, 32),
                    BackColor = Color.FromArgb(100, 0, 0)
                };
                pnlQNum.Controls.Add(new Label
                {
                    Text = $"Question {qi + 1}",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(12, 4),
                    AutoSize = true
                });
                card.Controls.Add(pnlQNum);
                questionY += 32;

                // Question text bar (maroon)
                string questionText = q.Text;
                int qTextH = Math.Max(40,
                    TextRenderer.MeasureText(questionText,
                        new Font("Segoe UI", 10.5F, FontStyle.Bold),
                        new Size(w - 28, int.MaxValue),
                        TextFormatFlags.WordBreak).Height + 20);

                var pnlQText = new Panel
                {
                    Location = new Point(0, questionY),
                    Size = new Size(w, qTextH),
                    BackColor = Color.FromArgb(128, 0, 0)
                };
                pnlQText.Controls.Add(new Label
                {
                    Text = questionText,
                    Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(14, 10),
                    MaximumSize = new Size(w - 28, 0),
                    AutoSize = true
                });
                // Points badge (top-right of question bar)
                pnlQText.Controls.Add(new Label
                {
                    Text = $"{q.Points} pt{(q.Points != 1 ? "s" : "")}",
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(255, 210, 160),
                    Location = new Point(w - 64, 8),
                    Size = new Size(56, 20),
                    TextAlign = ContentAlignment.MiddleRight
                });
                card.Controls.Add(pnlQText);
                questionY += qTextH;

                string saved = _answers.ContainsKey(q.Number) ? _answers[q.Number] : "";

                // ── Answer area (white background) ─────────────────────────
                if (q.QuestionType is "MultipleChoice" or "TrueFalse")
                {
                    char letter = 'A';
                    foreach (var choice in q.Choices)
                    {
                        bool isSelected = saved == choice;
                        var pnlChoice = new Panel
                        {
                            Location = new Point(0, questionY),
                            Size = new Size(w, 44),
                            BackColor = isSelected
                                ? Color.FromArgb(245, 220, 220)
                                : Color.White,
                            Cursor = isSubm ? Cursors.Default : Cursors.Hand
                        };
                        pnlChoice.Paint += (s, e) =>
                        {
                            using var pen = new Pen(Color.FromArgb(200, 170, 170));
                            e.Graphics.DrawLine(pen, 0, pnlChoice.Height - 1,
                                                     pnlChoice.Width - 1, pnlChoice.Height - 1);
                        };

                        // Letter box (A / B / C / D)
                        var pnlLetter = new Panel
                        {
                            Location = new Point(0, 0),
                            Size = new Size(44, 44),
                            BackColor = Color.FromArgb(128, 0, 0)
                        };
                        pnlLetter.Controls.Add(new Label
                        {
                            Text = letter.ToString(),
                            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                            ForeColor = Color.White,
                            Dock = DockStyle.Fill,
                            TextAlign = ContentAlignment.MiddleCenter
                        });
                        pnlChoice.Controls.Add(pnlLetter);

                        // Radio button + label
                        string capturedChoice = choice;
                        var rb = new RadioButton
                        {
                            Text = choice,
                            Font = new Font("Segoe UI", 10F),
                            ForeColor = Color.FromArgb(30, 30, 30),
                            Location = new Point(54, 10),
                            AutoSize = true,
                            Checked = isSelected,
                            Enabled = !isSubm,
                            Tag = choice
                        };
                        rb.CheckedChanged += (s, e) =>
                        {
                            if (!rb.Checked) return;
                            _answers[q.Number] = rb.Tag.ToString();
                            // Update pill color
                            if (qi < pnlPills.Controls.Count)
                                pnlPills.Controls[qi].BackColor = Color.FromArgb(27, 110, 27);
                            // Update highlight on all choices in this card
                            foreach (Control ctrl in card.Controls)
                            {
                                if (ctrl is Panel cp && cp.Height == 44 && cp.Controls.Count >= 2
                                    && cp.Controls[1] is RadioButton r2)
                                    cp.BackColor = r2.Checked
                                        ? Color.FromArgb(245, 220, 220)
                                        : Color.White;
                            }
                        };
                        pnlChoice.Controls.Add(rb);

                        // Whole row click selects the radio
                        if (!isSubm)
                        {
                            pnlChoice.Click += (s, e) => { rb.Checked = true; };
                            pnlLetter.Click += (s, e) => { rb.Checked = true; };
                            foreach (Control cc in pnlLetter.Controls)
                                cc.Click += (s, e) => { rb.Checked = true; };
                        }

                        card.Controls.Add(pnlChoice);
                        questionY += 44;
                        letter++;
                    }
                }
                else if (q.QuestionType == "Identification")
                {
                    var pnlAns = new Panel
                    {
                        Location = new Point(0, questionY),
                        Size = new Size(w, 62),
                        BackColor = Color.White,
                        Padding = new Padding(14)
                    };
                    var txt = new TextBox
                    {
                        Font = new Font("Segoe UI", 10.5F),
                        Location = new Point(14, 16),
                        Size = new Size(Math.Min(420, w - 28), 30),
                        Text = saved,
                        ReadOnly = isSubm,
                        PlaceholderText = "Type your answer here..."
                    };
                    txt.TextChanged += (s, e) =>
                    {
                        _answers[q.Number] = txt.Text;
                        if (qi < pnlPills.Controls.Count)
                            pnlPills.Controls[qi].BackColor = string.IsNullOrWhiteSpace(txt.Text)
                                ? Color.FromArgb(200, 200, 200)
                                : Color.FromArgb(27, 110, 27);
                    };
                    pnlAns.Controls.Add(txt);
                    card.Controls.Add(pnlAns);
                    questionY += 62;
                }
                else // Essay-type question
                {
                    var pnlAns = new Panel
                    {
                        Location = new Point(0, questionY),
                        Size = new Size(w, 180),
                        BackColor = Color.White,
                        Padding = new Padding(14)
                    };
                    var txtE = new TextBox
                    {
                        Multiline = true,
                        ScrollBars = ScrollBars.Vertical,
                        Font = new Font("Segoe UI", 10F),
                        Location = new Point(14, 14),
                        Size = new Size(w - 28, 148),
                        Text = saved,
                        ReadOnly = isSubm
                    };
                    txtE.TextChanged += (s, e) =>
                    {
                        _answers[q.Number] = txtE.Text;
                        if (qi < pnlPills.Controls.Count)
                            pnlPills.Controls[qi].BackColor = string.IsNullOrWhiteSpace(txtE.Text)
                                ? Color.FromArgb(200, 200, 200)
                                : Color.FromArgb(27, 110, 27);
                    };
                    pnlAns.Controls.Add(txtE);
                    card.Controls.Add(pnlAns);
                    questionY += 180;
                }

                card.Height = questionY;
                _scrollArea.Controls.Add(card);
                y += questionY + 16;
            }

            // ── Submit / status bar ────────────────────────────────────────
            if (!isSubm)
            {
                var btnSubmit = new buttonRounded
                {
                    Text = "Submit Quiz  \u2714",
                    Size = new Size(164, 42),
                    Location = new Point(16, y),
                    BackColor = Color.FromArgb(25, 105, 25),
                    ForeColor = Color.White,
                    BorderRadius = 21,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat
                };
                btnSubmit.FlatAppearance.BorderSize = 0;
                btnSubmit.Click += SubmitQuiz_Click;
                _scrollArea.Controls.Add(btnSubmit);
                y += 58;
            }
            else
            {
                _scrollArea.Controls.Add(new Label
                {
                    Text = $"\u2714  Quiz submitted  \u00B7  {_activity.SubmittedAt:MMM dd, yyyy  h:mm tt}",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(27, 110, 27),
                    Location = new Point(16, y),
                    AutoSize = true
                }); y += 32;

                if (_activity.Score.HasValue)
                {
                    _scrollArea.Controls.Add(new Label
                    {
                        Text = $"Score:  {_activity.Score} / {_activity.Points} pts",
                        Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                        ForeColor = Color.Maroon,
                        Location = new Point(16, y),
                        AutoSize = true
                    });
                }
            }
        }

        private void SubmitQuiz_Click(object sender, EventArgs e)
        {
            int unanswered = 0;
            foreach (var q in _activity.Questions)
                if (!_answers.ContainsKey(q.Number) || string.IsNullOrWhiteSpace(_answers[q.Number]))
                    unanswered++;

            string warn = unanswered > 0 ? $"You have {unanswered} unanswered question(s).\n\n" : "";
            var r = MessageBox.Show(warn + "Submit your quiz now? This cannot be undone.",
                "Confirm Submission", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            _activity.Answers = new Dictionary<int, string>(_answers);
            _activity.SubmissionStatus = "Submitted";
            _activity.SubmittedAt = DateTime.Now;

            MessageBox.Show("Quiz submitted successfully! \u2714", "Submitted",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            OnBack?.Invoke();
        }

        // ══════════════════════════════════════════════════════════════════════
        // RECITATION VIEW
        // FIX (Image 3): All controls added to _scrollArea with correct Y.
        // ══════════════════════════════════════════════════════════════════════

        private void BuildRecitationView()
        {
            bool isSubm = _activity.SubmissionStatus is "Submitted" or "Returned";
            int w = Math.Max(640, _scrollArea.ClientSize.Width > 0
                                  ? _scrollArea.ClientSize.Width - 60
                                  : pnlBody.ClientSize.Width - 60);
            int y = 20;

            var instrPnl = BuildInstructionsPanel(w);
            instrPnl.Location = new Point(20, y);
            _scrollArea.Controls.Add(instrPnl);
            y += instrPnl.MinimumSize.Height + 20;

            var pnlInfo = new Panel
            {
                BackColor = Color.FromArgb(235, 248, 255),
                Location = new Point(20, y),
                Size = new Size(w, 52),
                Padding = new Padding(14, 0, 14, 0)
            };
            pnlInfo.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(190, 220, 240));
                e.Graphics.DrawRectangle(pen, 0, 0, pnlInfo.Width - 1, pnlInfo.Height - 1);
            };
            pnlInfo.Controls.Add(new Label
            {
                Text = "\u2139  Your instructor will record your participation score during the class session.",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(20, 80, 160),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            });
            _scrollArea.Controls.Add(pnlInfo);
            y += 62;

            if (isSubm)
            {
                _scrollArea.Controls.Add(new Label
                {
                    Text = $"\u2714  Marked as Attended  \u00B7  {_activity.SubmittedAt:MMM dd, yyyy  h:mm tt}",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(27, 110, 27),
                    Location = new Point(20, y),
                    AutoSize = true
                }); y += 32;

                if (_activity.Score.HasValue)
                {
                    _scrollArea.Controls.Add(new Label
                    {
                        Text = $"Score:  {_activity.Score} / {_activity.Points} pts",
                        Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                        ForeColor = Color.Maroon,
                        Location = new Point(20, y),
                        AutoSize = true
                    }); y += 32;
                }

                if (!string.IsNullOrEmpty(_activity.Remarks))
                    _scrollArea.Controls.Add(BuildRemarksPanel(w, y));
            }
            else
            {
                _scrollArea.Controls.Add(new Label
                {
                    Text = "Participation Notes  (optional):",
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(40, 40, 40),
                    Location = new Point(20, y),
                    AutoSize = true
                }); y += 24;

                var txtNotes = new TextBox
                {
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    Font = new Font("Segoe UI", 10F),
                    Location = new Point(20, y),
                    Size = new Size(w, 120),
                    PlaceholderText = "Any notes for your instructor..."
                };
                _scrollArea.Controls.Add(txtNotes);
                y += 130;

                var btnMark = new buttonRounded
                {
                    Text = "Mark as Attended  \u2714",
                    Size = new Size(180, 40),
                    Location = new Point(20, y),
                    BackColor = Color.Maroon,
                    ForeColor = Color.White,
                    BorderRadius = 20,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat
                };
                btnMark.FlatAppearance.BorderSize = 0;
                btnMark.Click += (s, e) =>
                {
                    _activity.SubmissionStatus = "Submitted";
                    _activity.SubmittedAt = DateTime.Now;
                    MessageBox.Show("Marked as attended!", "Done",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OnBack?.Invoke();
                };
                _scrollArea.Controls.Add(btnMark);
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // BACK
        // ─────────────────────────────────────────────────────────────────────

        private void btnBack_Click(object sender, EventArgs e)
        {
            AutosaveDraft();
            OnBack?.Invoke();
        }
    }
}