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

        // Timers — declared here so Designer Dispose() can reach them
        private System.Windows.Forms.Timer _countdownTimer;
        private System.Windows.Forms.Timer _autosaveTimer;

        // File upload runtime state
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

            lblMeta.Text = $"{typeDisplay}  ·  {_activity.Points} pts  ·  Due {_activity.Deadline:MMM dd, yyyy  h:mm tt}";

            if (_activity.Score.HasValue)
            {
                lblMeta.Text += $"  ·  Score: {_activity.Score} / {_activity.Points}";
                lblMeta.ForeColor = Color.FromArgb(255, 210, 210);
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // DEADLINE COUNTDOWN BAR
        // ─────────────────────────────────────────────────────────────────────

        private void BuildDeadlineBar()
        {
            TimeSpan remaining = _activity.Deadline - DateTime.Now;
            bool isLate = remaining.TotalSeconds <= 0;

            var pnlBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 34,
                BackColor = isLate ? Color.FromArgb(175, 20, 20) : Color.FromArgb(45, 45, 45)
            };

            _lblDeadlineTimer = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                Text = isLate
                            ? "⚠  Deadline has passed — late submission"
                            : FormatCountdown(remaining)
            };

            pnlBar.Controls.Add(_lblDeadlineTimer);
            Controls.Add(pnlBar);

            pnlHeader.BringToFront();
            pnlBar.SendToBack();
            pnlBody.SendToBack();
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
                    _lblDeadlineTimer.Text = "⚠  Deadline has passed — late submission";
                    if (_lblDeadlineTimer.Parent is Panel p) p.BackColor = Color.FromArgb(175, 20, 20);
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
            if (ts.TotalDays >= 1) return $"⏱  {(int)ts.TotalDays}d {ts.Hours}h {ts.Minutes}m remaining";
            if (ts.TotalHours >= 1) return $"⏱  {ts.Hours}h {ts.Minutes}m {ts.Seconds}s remaining";
            return $"⚠  {ts.Minutes}m {ts.Seconds}s remaining — hurry!";
        }

        // ─────────────────────────────────────────────────────────────────────
        // CONTENT ROUTER
        // ─────────────────────────────────────────────────────────────────────

        private void BuildContentArea()
        {
            pnlBody.Controls.Clear();
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
        // SHARED: Amber instructions card
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
                Text = "📋  Instructions",
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

        // ─────────────────────────────────────────────────────────────────────
        // SHARED: Attached reference-files list
        //         Returns updated Y position.
        // ─────────────────────────────────────────────────────────────────────

        private int BuildAttachmentList(int startY, int w)
        {
            if (_activity.Attachments == null || _activity.Attachments.Count == 0)
                return startY;

            int y = startY;

            pnlBody.Controls.Add(new Label
            {
                Text = "Attached Files",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 40, 40),
                Location = new Point(20, y),
                AutoSize = true
            }); y += 24;

            foreach (var att in _activity.Attachments)
            {
                // Determine icon from file extension since FileType may or may not be set
                string ext = System.IO.Path.GetExtension(att.FileName).ToLowerInvariant();
                string icon = ext switch
                {
                    ".pdf" => "📄",
                    ".docx" => "📝",
                    ".doc" => "📝",
                    ".pptx" => "📊",
                    ".ppt" => "📊",
                    ".png" => "🖼",
                    ".jpg" => "🖼",
                    ".jpeg" => "🖼",
                    _ => "📁"
                };

                // Friendly file size — works whether FileSize is set or not
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
                    : $"{sizeStr}  —  Click to download";

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

                pnlBody.Controls.Add(pnlAtt);
                y += 54;
            }
            return y + 8;
        }

        // ─────────────────────────────────────────────────────────────────────
        // SHARED: Blue instructor-remarks card
        // ─────────────────────────────────────────────────────────────────────

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
                Text = "📝  Instructor Remarks:",
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
        // ══════════════════════════════════════════════════════════════════════

        private void BuildEssayView()
        {
            bool submitted = _activity.SubmissionStatus is "Submitted" or "Returned";
            int w = Math.Max(640, pnlBody.ClientSize.Width - 60);
            int y = 20;

            var instrPnl = BuildInstructionsPanel(w);
            instrPnl.Location = new Point(20, y);
            pnlBody.Controls.Add(instrPnl);
            y += instrPnl.MinimumSize.Height + 20;

            if (submitted)
            {
                // ✔ Submitted status line
                pnlBody.Controls.Add(new Label
                {
                    Text = $"✔  Essay Submitted  ·  {_activity.SubmittedAt:MMM dd, yyyy  h:mm tt}",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(27, 110, 27),
                    Location = new Point(20, y),
                    AutoSize = true
                }); y += 32;

                if (_activity.Score.HasValue)
                {
                    pnlBody.Controls.Add(new Label
                    {
                        Text = $"Score:  {_activity.Score} / {_activity.Points} pts",
                        Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                        ForeColor = Color.Maroon,
                        Location = new Point(20, y),
                        AutoSize = true
                    }); y += 32;
                }

                pnlBody.Controls.Add(new Label
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
                pnlBody.Controls.Add(_txtEssay);
                y += _txtEssay.Height + 18;

                if (!string.IsNullOrEmpty(_activity.Remarks))
                    pnlBody.Controls.Add(BuildRemarksPanel(w, y));
            }
            else
            {
                pnlBody.Controls.Add(new Label
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
                pnlBody.Controls.Add(_txtEssay);
                y += _txtEssay.Height + 6;

                _lblWordCount = new Label
                {
                    Font = new Font("Segoe UI", 8.5F),
                    ForeColor = Color.Gray,
                    Location = new Point(20, y),
                    AutoSize = true
                };
                pnlBody.Controls.Add(_lblWordCount);
                UpdateWordCount();
                y += 22;

                _lblAutosave = new Label
                {
                    Text = "💾  Draft autosaved every 30 seconds.",
                    Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                    ForeColor = Color.FromArgb(0, 135, 0),
                    Location = new Point(20, y),
                    AutoSize = true
                };
                pnlBody.Controls.Add(_lblAutosave);
                y += 26;

                var btnSub = new buttonRounded
                {
                    Text = "Submit Essay  ✔",
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
                pnlBody.Controls.Add(btnSub);
            }
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
            if (_txtEssay == null || _txtEssay.IsDisposed) return;
            _activity.EssayDraft = _txtEssay.Text;
            if (_lblAutosave != null && !_lblAutosave.IsDisposed)
                _lblAutosave.Text = $"💾  Draft autosaved at {DateTime.Now:h:mm:ss tt}";
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

            MessageBox.Show("Essay submitted successfully! ✔", "Submitted",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            OnBack?.Invoke();
        }

        // ══════════════════════════════════════════════════════════════════════
        // FILE UPLOAD VIEW
        // ══════════════════════════════════════════════════════════════════════

        private void BuildFileUploadView()
        {
            bool isSubm = _activity.SubmissionStatus is "Submitted" or "Returned";
            int w = Math.Max(640, pnlBody.ClientSize.Width - 60);
            int y = 20;

            var instrPnl = BuildInstructionsPanel(w);
            instrPnl.Location = new Point(20, y);
            pnlBody.Controls.Add(instrPnl);
            y += instrPnl.MinimumSize.Height + 20;

            y = BuildAttachmentList(y, w);

            if (isSubm)
            {
                pnlBody.Controls.Add(new Label
                {
                    Text = $"✔  File Submitted  ·  {_activity.SubmittedAt:MMM dd, yyyy  h:mm tt}",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(27, 110, 27),
                    Location = new Point(20, y),
                    AutoSize = true
                }); y += 32;

                if (_activity.Score.HasValue)
                {
                    pnlBody.Controls.Add(new Label
                    {
                        Text = $"Score:  {_activity.Score} / {_activity.Points} pts",
                        Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                        ForeColor = Color.Maroon,
                        Location = new Point(20, y),
                        AutoSize = true
                    }); y += 32;
                }

                if (!string.IsNullOrEmpty(_activity.Remarks))
                    pnlBody.Controls.Add(BuildRemarksPanel(w, y));
            }
            else
            {
                pnlBody.Controls.Add(new Label
                {
                    Text = "Attach Your Submission:",
                    Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(35, 35, 35),
                    Location = new Point(20, y),
                    AutoSize = true
                }); y += 28;

                // Drop zone
                var pnlDrop = new Panel
                {
                    BackColor = Color.FromArgb(244, 248, 255),
                    BorderStyle = BorderStyle.FixedSingle,
                    Location = new Point(20, y),
                    Size = new Size(w, 100)
                };
                var lblHint = new Label
                {
                    Text = "Drag & drop your file here  —  or  —  click  Browse",
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
                pnlBody.Controls.Add(pnlDrop);
                y += 110;

                _lblFileName = new Label
                {
                    Text = string.IsNullOrEmpty(_uploadedFileName)
                                ? "No file selected." : $"📎  {_uploadedFileName}",
                    Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                    ForeColor = string.IsNullOrEmpty(_uploadedFileName)
                                ? Color.Gray : Color.FromArgb(0, 105, 0),
                    Location = new Point(20, y),
                    AutoSize = true
                };
                pnlBody.Controls.Add(_lblFileName);

                _lblFileSize = new Label
                {
                    Text = _uploadedFileSize > 0 ? $"  ({FormatFileSize(_uploadedFileSize)})" : "",
                    Font = new Font("Segoe UI", 8.5F),
                    ForeColor = Color.Gray,
                    Location = new Point(220, y),
                    AutoSize = true
                };
                pnlBody.Controls.Add(_lblFileSize);
                y += 26;

                _btnRemoveFile = new buttonRounded
                {
                    Text = "✕  Remove",
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
                pnlBody.Controls.Add(_btnRemoveFile);
                y += 40;

                pnlBody.Controls.Add(new Label
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
                pnlBody.Controls.Add(txtNotes);
                y += 100;

                var btnUpload = new buttonRounded
                {
                    Text = "Upload & Submit  ✔",
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
                pnlBody.Controls.Add(btnUpload);
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

                if (_lblFileName != null) { _lblFileName.Text = $"📎  {_uploadedFileName}"; _lblFileName.ForeColor = Color.FromArgb(0, 105, 0); }
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

            MessageBox.Show("File uploaded and submitted successfully! ✔", "Submitted",
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
        // QUIZ / LONG QUIZ VIEW
        // ══════════════════════════════════════════════════════════════════════

        private void BuildQuizView()
        {
            pnlBody.Controls.Clear();

            if (_activity.Questions == null || _activity.Questions.Count == 0)
            {
                pnlBody.Controls.Add(new Label
                {
                    Text = "No questions available.",
                    Font = new Font("Segoe UI", 11F),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Location = new Point(20, 20)
                });
                return;
            }

            int w = Math.Max(640, pnlBody.ClientSize.Width - 60);
            int y = 20;
            int totalQ = _activity.Questions.Count;
            bool isSubm = _activity.SubmissionStatus is "Submitted" or "Returned";

            // ── Progress strip ─────────────────────────────────────────────
            var pnlProg = new Panel
            {
                Location = new Point(20, y),
                Size = new Size(w, 54),
                BackColor = Color.Transparent
            };
            pnlProg.Controls.Add(new Label
            {
                Text = $"Question  {_currentQ + 1}  of  {totalQ}",
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 30),
                Location = new Point(0, 0),
                AutoSize = true
            });
            pnlProg.Controls.Add(new Label
            {
                Text = $"{_answers.Count}/{totalQ} answered",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.Gray,
                Location = new Point(0, 24),
                AutoSize = true
            });

            // Question navigation dots
            int dotSize = Math.Max(20, Math.Min(26, (w - 220) / Math.Max(1, totalQ) - 4));
            int dotStartX = w - totalQ * (dotSize + 4);
            for (int i = 0; i < totalQ; i++)
            {
                bool ans = _answers.ContainsKey(i + 1);
                bool cur = i == _currentQ;
                int ci = i;
                var dot = new Label
                {
                    Text = (i + 1).ToString(),
                    BackColor = cur ? Color.Maroon : ans ? Color.ForestGreen : Color.FromArgb(205, 205, 205),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                    Location = new Point(dotStartX + i * (dotSize + 4), 10),
                    Size = new Size(dotSize, dotSize),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Cursor = Cursors.Hand
                };
                dot.Click += (s, e) => { _currentQ = ci; BuildQuizView(); };
                pnlProg.Controls.Add(dot);
            }
            pnlBody.Controls.Add(pnlProg);
            y += pnlProg.Height + 12;

            // ── White question card ────────────────────────────────────────
            var q = _activity.Questions[_currentQ];
            var pnlQ = new Panel
            {
                BackColor = Color.White,
                Location = new Point(20, y),
                Width = w,
                AutoSize = true,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(18)
            };
            int qy = 16;

            // Type badge
            string typeBadge = q.QuestionType switch
            {
                "MultipleChoice" => "Multiple Choice",
                "TrueFalse" => "True or False",
                "Identification" => "Identification",
                "Essay" => "Essay",
                _ => q.QuestionType
            };
            Color badgeClr = q.QuestionType switch
            {
                "MultipleChoice" => Color.FromArgb(63, 81, 181),
                "TrueFalse" => Color.FromArgb(0, 150, 136),
                "Identification" => Color.FromArgb(156, 39, 176),
                _ => Color.Gray
            };
            pnlQ.Controls.Add(new Label
            {
                Text = typeBadge,
                BackColor = badgeClr,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Location = new Point(18, qy),
                Size = new Size(130, 22),
                TextAlign = ContentAlignment.MiddleCenter
            });
            pnlQ.Controls.Add(new Label
            {
                Text = $"{q.Points} pt{(q.Points != 1 ? "s" : "")}",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.Maroon,
                Location = new Point(w - 76, qy),
                Size = new Size(56, 22),
                TextAlign = ContentAlignment.MiddleRight
            }); qy += 34;

            // Question text
            pnlQ.Controls.Add(new Label
            {
                Text = $"{_currentQ + 1}.  {q.Text}",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.FromArgb(22, 22, 22),
                Location = new Point(18, qy),
                MaximumSize = new Size(w - 38, 0),
                AutoSize = true
            }); qy += 66;

            // Answer area
            string saved = _answers.ContainsKey(q.Number) ? _answers[q.Number] : "";

            if (q.QuestionType is "MultipleChoice" or "TrueFalse")
            {
                foreach (var choice in q.Choices)
                {
                    var rowPnl = new Panel
                    {
                        Location = new Point(16, qy),
                        Size = new Size(w - 36, 38),
                        BackColor = saved == choice ? Color.FromArgb(245, 235, 235) : Color.Transparent
                    };
                    string capturedChoice = choice;
                    var rb = new RadioButton
                    {
                        Text = choice,
                        Font = new Font("Segoe UI", 10.5F),
                        ForeColor = Color.FromArgb(30, 30, 30),
                        Location = new Point(8, 6),
                        AutoSize = true,
                        Checked = saved == choice,
                        Enabled = !isSubm,
                        Tag = choice
                    };
                    rb.CheckedChanged += (s, e) =>
                    {
                        if (!rb.Checked) return;
                        _answers[q.Number] = rb.Tag.ToString();
                        foreach (Control ctrl in pnlQ.Controls)
                            if (ctrl is Panel rp && rp.Controls.Count > 0 && rp.Controls[0] is RadioButton r2)
                                rp.BackColor = r2.Checked ? Color.FromArgb(245, 235, 235) : Color.Transparent;
                    };
                    rowPnl.Controls.Add(rb);
                    pnlQ.Controls.Add(rowPnl);
                    qy += 40;
                }
            }
            else if (q.QuestionType == "Identification")
            {
                pnlQ.Controls.Add(new Label
                {
                    Text = "Answer:",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(55, 55, 55),
                    Location = new Point(22, qy),
                    AutoSize = true
                }); qy += 22;
                var txt = new TextBox
                {
                    Font = new Font("Segoe UI", 10.5F),
                    Location = new Point(22, qy),
                    Size = new Size(Math.Min(420, w - 44), 30),
                    Text = saved,
                    ReadOnly = isSubm,
                    PlaceholderText = "Type your answer here..."
                };
                txt.TextChanged += (s, e) => _answers[q.Number] = txt.Text;
                pnlQ.Controls.Add(txt); qy += 42;
            }
            else
            {
                pnlQ.Controls.Add(new Label
                {
                    Text = "Your Answer:",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(55, 55, 55),
                    Location = new Point(22, qy),
                    AutoSize = true
                }); qy += 22;
                var txtE = new TextBox
                {
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    Font = new Font("Segoe UI", 10F),
                    Location = new Point(22, qy),
                    Size = new Size(w - 44, 160),
                    Text = saved,
                    ReadOnly = isSubm
                };
                txtE.TextChanged += (s, e) => _answers[q.Number] = txtE.Text;
                pnlQ.Controls.Add(txtE); qy += 170;
            }

            pnlQ.Height = qy + 16;
            pnlBody.Controls.Add(pnlQ);
            y += pnlQ.Height + 20;

            // ── Navigation buttons ─────────────────────────────────────────
            var pnlNav = new Panel { Location = new Point(20, y), Size = new Size(w, 50), BackColor = Color.Transparent };

            if (_currentQ > 0)
            {
                var btnPrev = MakeNavBtn("← Previous", Color.FromArgb(65, 65, 65), 0);
                btnPrev.Click += (s, e) => { _currentQ--; BuildQuizView(); };
                pnlNav.Controls.Add(btnPrev);
            }

            if (_currentQ < totalQ - 1)
            {
                var btnNext = MakeNavBtn("Next →", Color.Maroon, 142);
                btnNext.Click += (s, e) => { _currentQ++; BuildQuizView(); };
                pnlNav.Controls.Add(btnNext);
            }
            else if (!isSubm)
            {
                var btnSubmit = MakeNavBtn("Submit Quiz  ✔", Color.FromArgb(25, 105, 25), 142);
                btnSubmit.Click += SubmitQuiz_Click;
                pnlNav.Controls.Add(btnSubmit);
            }

            pnlBody.Controls.Add(pnlNav);
        }

        private buttonRounded MakeNavBtn(string text, Color back, int x)
        {
            var btn = new buttonRounded
            {
                Text = text,
                Size = new Size(134, 38),
                Location = new Point(x, 6),
                BackColor = back,
                ForeColor = Color.White,
                BorderRadius = 19,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
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

            MessageBox.Show("Quiz submitted successfully! ✔", "Submitted",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            OnBack?.Invoke();
        }

        // ══════════════════════════════════════════════════════════════════════
        // RECITATION VIEW
        // ══════════════════════════════════════════════════════════════════════

        private void BuildRecitationView()
        {
            bool isSubm = _activity.SubmissionStatus is "Submitted" or "Returned";
            int w = Math.Max(640, pnlBody.ClientSize.Width - 60);
            int y = 20;

            var instrPnl = BuildInstructionsPanel(w);
            instrPnl.Location = new Point(20, y);
            pnlBody.Controls.Add(instrPnl);
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
                Text = "ℹ  Your instructor will record your participation score during the class session.",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(20, 80, 160),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            });
            pnlBody.Controls.Add(pnlInfo);
            y += 62;

            if (isSubm)
            {
                pnlBody.Controls.Add(new Label
                {
                    Text = $"✔  Marked as Attended  ·  {_activity.SubmittedAt:MMM dd, yyyy  h:mm tt}",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(27, 110, 27),
                    Location = new Point(20, y),
                    AutoSize = true
                }); y += 32;

                if (_activity.Score.HasValue)
                {
                    pnlBody.Controls.Add(new Label
                    {
                        Text = $"Score:  {_activity.Score} / {_activity.Points} pts",
                        Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                        ForeColor = Color.Maroon,
                        Location = new Point(20, y),
                        AutoSize = true
                    }); y += 32;
                }

                if (!string.IsNullOrEmpty(_activity.Remarks))
                    pnlBody.Controls.Add(BuildRemarksPanel(w, y));
            }
            else
            {
                pnlBody.Controls.Add(new Label
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
                pnlBody.Controls.Add(txtNotes);
                y += 130;

                var btnMark = new buttonRounded
                {
                    Text = "Mark as Attended  ✔",
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
                pnlBody.Controls.Add(btnMark);
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