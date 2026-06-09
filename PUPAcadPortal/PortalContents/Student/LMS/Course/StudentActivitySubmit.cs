using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using PUPAcadPortal.Services;

namespace PUPAcadPortal.PortalContents.Student.LMS.Course
{
    public partial class StudentActivitySubmit : UserControl
    {
        public event Action OnBack;

        private readonly StudentActivityItem _activity;
        private readonly StudentCourse _course;
        private readonly int _studentId;
        private readonly IStudentCourseDbService _svc;
        private readonly bool _useDb;

        // Quiz navigation state
        private int _currentQ = 0;
        private Dictionary<int, string> _answers = new();

        // Timers
        private System.Windows.Forms.Timer _countdownTimer;
        private System.Windows.Forms.Timer _autosaveTimer;

        // File-upload runtime state
        private string _uploadedFilePath = "";
        private string _uploadedFileName = "";
        private long _uploadedFileSize = 0;

        // Dynamic control references
        private Label _lblDeadlineTimer;
        private TextBox _txtEssay;
        private Label _lblWordCount;
        private Label _lblAutosave;
        private Label _lblFileName;
        private Label _lblFileSize;
        private buttonRounded _btnRemoveFile;

        // ── Per-type accent colours (mirrors ActivityFormPage) ───────────────
        private static readonly Color QuizAccent = Color.FromArgb(63, 81, 181);
        private static readonly Color EssayAccent = Color.FromArgb(0, 150, 136);
        private static readonly Color AssignmentAccent = Color.FromArgb(128, 0, 0);
        private static readonly Color FileUploadAccent = Color.FromArgb(76, 175, 80);

        private Color ActiveAccent => _activity.Type switch
        {
            "Quiz" => QuizAccent,
            "LongQuiz" => QuizAccent,
            "Essay" => EssayAccent,
            "FileUpload" => FileUploadAccent,
            _ => AssignmentAccent,
        };

        // ────────────────────────────────────────────────────────────────────
        public StudentActivitySubmit(StudentActivityItem activity, StudentCourse course)
            : this(activity, course, 0, new NullStudentCourseDbService()) { }

        public StudentActivitySubmit(
            StudentActivityItem activity,
            StudentCourse course,
            int studentId,
            IStudentCourseDbService svc)
        {
            _activity = activity;
            _course = course;
            _studentId = studentId;
            _svc = svc ?? new NullStudentCourseDbService();
            _useDb = studentId > 0 && !string.IsNullOrWhiteSpace(activity.ActivityId);
            _answers = new Dictionary<int, string>(activity.Answers ?? new());
            _uploadedFilePath = activity.UploadedFilePath;
            _uploadedFileName = activity.UploadedFileName;
            _uploadedFileSize = activity.UploadedFileSize;

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

        // ════════════════════════════════════════════════════════════════
        //  HEADER — type-aware colour strip
        // ════════════════════════════════════════════════════════════════
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

            // Apply per-type accent to the header panel
            if (pnlHeader != null)
            {
                pnlHeader.BackColor = ActiveAccent;
            }

            // Apply type badge to the header
            ApplyTypeBadge(typeDisplay);
        }

        private void ApplyTypeBadge(string typeDisplay)
        {
            if (pnlHeader == null) return;

            // Type badge in top-left of header
            string icon = _activity.Type switch
            {
                "Quiz" => "❓",
                "LongQuiz" => "📝",
                "Essay" => "✍",
                "FileUpload" => "📎",
                _ => "📋"
            };

            var badge = new Label
            {
                Text = $"{icon} {typeDisplay}",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                BackColor = Color.FromArgb(60, 255, 255, 255),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(108, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(pnlHeader.Width - 120, pnlHeader.Height - 28),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
            };
            pnlHeader.Controls.Add(badge);
        }

        // ════════════════════════════════════════════════════════════════
        //  DEADLINE BAR
        // ════════════════════════════════════════════════════════════════
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
                Text = isLate ? "⚠  Deadline has passed — late submission" : FormatCountdown(remaining)
            };

            pnlBar.Controls.Add(_lblDeadlineTimer);
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
                    _lblDeadlineTimer.Text = "⚠  Deadline has passed — late submission";
                    if (_lblDeadlineTimer.Parent is Panel p) p.BackColor = Color.FromArgb(175, 20, 20);
                    _countdownTimer.Stop();
                }
                else
                {
                    _lblDeadlineTimer.Text = FormatCountdown(rem);
                    if (_lblDeadlineTimer.Parent is Panel p)
                        p.BackColor = rem.TotalHours < 1 ? Color.FromArgb(175, 80, 0) : Color.FromArgb(45, 45, 45);
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

        // ════════════════════════════════════════════════════════════════
        //  CONTENT AREA
        // ════════════════════════════════════════════════════════════════
        private Panel _scrollArea;

        private void BuildContentArea()
        {
            _scrollArea = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(0)
            };
            pnlBody.Controls.Add(_scrollArea);
            _scrollArea.SendToBack();

            switch (_activity.Type)
            {
                case "Essay": BuildEssayView(); break;
                case "Quiz":
                case "LongQuiz":
                    BuildQuizView();
                    _scrollArea.SizeChanged += OnScrollAreaFirstResize;
                    break;
                case "FileUpload": BuildFileUploadView(); break;
                case "Recitation": BuildRecitationView(); break;
                default: BuildAssignmentView(); break;
            }
        }

        private void OnScrollAreaFirstResize(object? sender, EventArgs e)
        {
            if (_scrollArea.ClientSize.Width <= 0) return;
            _scrollArea.SizeChanged -= OnScrollAreaFirstResize;
            if (_activity.Type is "Quiz" or "LongQuiz") BuildQuizView();
        }

        // ════════════════════════════════════════════════════════════════
        //  RUBRIC PANEL (student-facing, read-only)
        //  Displayed for Essay and Assignment types before submission
        // ════════════════════════════════════════════════════════════════
        private Panel BuildStudentRubricPanel(int w)
        {
            var rubricItems = _activity.RubricItems ?? new List<ActivityRubricItem>();
            if (rubricItems.Count == 0) return new Panel { Height = 0 };

            var pnl = new Panel
            {
                BackColor = Color.FromArgb(250, 250, 255),
                Width = w,
                AutoSize = false,
                Padding = new Padding(14, 12, 14, 14),
            };
            pnl.Paint += (s, e) =>
            {
                using var pen = new Pen(ActiveAccent, 1.5f);
                e.Graphics.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
                using var bar = new SolidBrush(ActiveAccent);
                e.Graphics.FillRectangle(bar, 0, 0, 4, pnl.Height);
            };

            int y = 12;

            // Section header
            pnl.Controls.Add(new Label
            {
                Text = "📊  Grading Rubric",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = ActiveAccent,
                Location = new Point(18, y),
                AutoSize = true,
            });
            y += 28;

            pnl.Controls.Add(new Label
            {
                Text = "Your submission will be evaluated based on the criteria below.",
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 100, 120),
                Location = new Point(18, y),
                AutoSize = true,
            });
            y += 22;

            // Column headers
            var pnlHeader = new Panel
            {
                Location = new Point(14, y),
                BackColor = ActiveAccent,
                Height = 26,
                Width = w - 28,
            };
            pnlHeader.Controls.Add(new Label
            {
                Text = "Criterion",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 5),
                AutoSize = true,
            });
            pnlHeader.Controls.Add(new Label
            {
                Text = "Description",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(200, 5),
                AutoSize = true,
            });
            pnlHeader.Controls.Add(new Label
            {
                Text = "Points",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(w - 90, 5),
                AutoSize = true,
            });
            pnl.Controls.Add(pnlHeader);
            y += 28;

            int total = 0;
            bool odd = false;
            foreach (var crit in rubricItems)
            {
                odd = !odd;
                var row = new Panel
                {
                    Location = new Point(14, y),
                    BackColor = odd ? Color.White : Color.FromArgb(243, 243, 250),
                    Height = 34,
                    Width = w - 28,
                };
                row.Paint += (s, e) =>
                {
                    using var pen = new Pen(Color.FromArgb(225, 225, 235));
                    e.Graphics.DrawLine(pen, 0, row.Height - 1, row.Width, row.Height - 1);
                };

                row.Controls.Add(new Label
                {
                    Text = crit.Name,
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(30, 30, 40),
                    Location = new Point(10, 9),
                    Size = new Size(185, 18),
                    AutoEllipsis = true,
                });
                row.Controls.Add(new Label
                {
                    Text = crit.Description,
                    Font = new Font("Segoe UI", 8.5F),
                    ForeColor = Color.FromArgb(80, 80, 90),
                    Location = new Point(200, 9),
                    Size = new Size(w - 310, 18),
                    AutoEllipsis = true,
                });
                row.Controls.Add(new Label
                {
                    Text = $"{crit.MaxPoints} pts",
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    ForeColor = ActiveAccent,
                    Location = new Point(w - 90, 9),
                    Size = new Size(72, 18),
                    TextAlign = ContentAlignment.MiddleLeft,
                });

                pnl.Controls.Add(row);
                y += 36;
                total += crit.MaxPoints;
            }

            // Total row
            var pnlTotal = new Panel
            {
                Location = new Point(14, y),
                BackColor = Color.FromArgb(240, 245, 255),
                Height = 30,
                Width = w - 28,
            };
            pnlTotal.Paint += (s, e) =>
            {
                using var pen = new Pen(ActiveAccent);
                e.Graphics.DrawRectangle(pen, 0, 0, pnlTotal.Width - 1, pnlTotal.Height - 1);
            };
            pnlTotal.Controls.Add(new Label
            {
                Text = "TOTAL",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = ActiveAccent,
                Location = new Point(10, 7),
                AutoSize = true,
            });
            pnlTotal.Controls.Add(new Label
            {
                Text = $"{total} pts",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = ActiveAccent,
                Location = new Point(w - 90, 7),
                AutoSize = true,
            });
            pnl.Controls.Add(pnlTotal);
            y += 34;

            pnl.Height = y + 14;
            return pnl;
        }

        // Lightweight struct to hold rubric data from the activity
        private class ActivityRubricCriteria
        {
            public string Name { get; set; } = "";
            public string Description { get; set; } = "";
            public int MaxPoints { get; set; }
        }

        // ════════════════════════════════════════════════════════════════
        //  SHARED HELPERS
        // ════════════════════════════════════════════════════════════════
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

        private int BuildAttachmentList(int startY, int w)
        {
            if (_activity.Attachments == null || _activity.Attachments.Count == 0) return startY;

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
                    ".pdf" => "📄",
                    ".docx" or ".doc" => "📝",
                    ".pptx" or ".ppt" => "📊",
                    ".png" or ".jpg" or ".jpeg" => "🖼",
                    _ => "📁"
                };

                string sizeStr = att.FileSize > 0
                    ? (att.FileSize >= 1_048_576 ? $"{att.FileSize / 1_048_576.0:F1} MB"
                     : att.FileSize >= 1_024 ? $"{att.FileSize / 1_024.0:F0} KB"
                     : $"{att.FileSize} B")
                    : "";

                int cardW = Math.Min(480, w - 20);
                var pnlAtt = new Panel
                {
                    BackColor = Color.White,
                    Location = new Point(20, y),
                    Size = new Size(cardW, 48),
                    Cursor = Cursors.Hand
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

                string hint = string.IsNullOrEmpty(sizeStr) ? "Click to download" : $"{sizeStr}  —  Click to download";
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
                        MessageBox.Show($"File: {attCapture.FileName}", "Download",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        // ════════════════════════════════════════════════════════════════
        //  ASSIGNMENT VIEW (distinct from essay)
        // ════════════════════════════════════════════════════════════════
        private void BuildAssignmentView()
        {
            bool submitted = _activity.SubmissionStatus is "Submitted" or "Returned";
            int w = Math.Max(640, _scrollArea.ClientSize.Width > 0
                                  ? _scrollArea.ClientSize.Width - 60
                                  : pnlBody.ClientSize.Width - 60);
            int y = 20;

            // Assignment type banner
            var typeBanner = new Panel
            {
                BackColor = Color.FromArgb(245, 235, 235),
                Location = new Point(20, y),
                Size = new Size(w, 40),
            };
            typeBanner.Paint += (s, e) =>
            {
                using var pen = new Pen(AssignmentAccent, 1.5f);
                e.Graphics.DrawRectangle(pen, 0, 0, typeBanner.Width - 1, typeBanner.Height - 1);
                using var bar = new SolidBrush(AssignmentAccent);
                e.Graphics.FillRectangle(bar, 0, 0, 4, typeBanner.Height);
            };
            typeBanner.Controls.Add(new Label
            {
                Text = "📋  Assignment  —  Submit your work below",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = AssignmentAccent,
                Location = new Point(16, 11),
                AutoSize = true,
                BackColor = Color.Transparent,
            });
            _scrollArea.Controls.Add(typeBanner);
            y += 50;

            // Instructions
            var instrPnl = BuildInstructionsPanel(w);
            instrPnl.Location = new Point(20, y);
            _scrollArea.Controls.Add(instrPnl);
            y += instrPnl.MinimumSize.Height + 16;

            // Rubric — show BEFORE submission so students see grading criteria
            var rubricPnl = BuildStudentRubricPanel(w);
            if (rubricPnl.Height > 0)
            {
                rubricPnl.Location = new Point(20, y);
                _scrollArea.Controls.Add(rubricPnl);
                y += rubricPnl.Height + 16;
            }

            y = BuildAttachmentList(y, w);

            if (submitted)
            {
                _scrollArea.Controls.Add(new Label
                {
                    Text = $"✔  Assignment Submitted  ·  {_activity.SubmittedAt:MMM dd, yyyy  h:mm tt}",
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
                        ForeColor = AssignmentAccent,
                        Location = new Point(20, y),
                        AutoSize = true
                    }); y += 32;
                }

                if (!string.IsNullOrEmpty(_activity.Remarks))
                    _scrollArea.Controls.Add(BuildRemarksPanel(w, y));
            }
            else
            {
                // File upload zone for assignment
                _scrollArea.Controls.Add(new Label
                {
                    Text = "Attach Your Submission File:",
                    Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(35, 35, 35),
                    Location = new Point(20, y),
                    AutoSize = true
                }); y += 28;

                var pnlDrop = new Panel
                {
                    BackColor = Color.FromArgb(250, 245, 245),
                    BorderStyle = BorderStyle.FixedSingle,
                    Location = new Point(20, y),
                    Size = new Size(w, 100)
                };
                pnlDrop.Paint += (s, e) =>
                {
                    using var pen = new Pen(AssignmentAccent, 1f);
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlDrop.Width - 1, pnlDrop.Height - 1);
                };
                var lblHint = new Label
                {
                    Text = "Drag & drop your assignment file here  —  or  —  click  Browse",
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = Color.FromArgb(130, 80, 80),
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                var btnBrowse = new buttonRounded
                {
                    Text = "Browse",
                    Size = new Size(96, 32),
                    Location = new Point(w - 106, 32),
                    BackColor = AssignmentAccent,
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
                    Text = string.IsNullOrEmpty(_uploadedFileName) ? "No file selected." : $"📎  {_uploadedFileName}",
                    Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                    ForeColor = string.IsNullOrEmpty(_uploadedFileName) ? Color.Gray : Color.FromArgb(0, 105, 0),
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
                    _activity.UploadedFilePath = _activity.UploadedFileName = "";
                    _activity.UploadedFileSize = 0;
                    if (_lblFileName != null) { _lblFileName.Text = "No file selected."; _lblFileName.ForeColor = Color.Gray; }
                    if (_lblFileSize != null) _lblFileSize.Text = "";
                    _btnRemoveFile.Visible = false;
                };
                _scrollArea.Controls.Add(_btnRemoveFile);
                y += 40;

                // Notes
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

                var btnSubmit = new buttonRounded
                {
                    Text = "Upload & Submit  ✔",
                    Size = new Size(180, 40),
                    Location = new Point(20, y),
                    BackColor = AssignmentAccent,
                    ForeColor = Color.White,
                    BorderRadius = 20,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat
                };
                btnSubmit.FlatAppearance.BorderSize = 0;
                btnSubmit.Click += SubmitFileUpload_Click;
                _scrollArea.Controls.Add(btnSubmit);
            }
        }

        // ════════════════════════════════════════════════════════════════
        //  ESSAY VIEW (teal accent, rubric shown before submission)
        // ════════════════════════════════════════════════════════════════
        private void BuildEssayView()
        {
            bool submitted = _activity.SubmissionStatus is "Submitted" or "Returned";
            int w = Math.Max(640, _scrollArea.ClientSize.Width > 0
                                  ? _scrollArea.ClientSize.Width - 60
                                  : pnlBody.ClientSize.Width - 60);
            int y = 20;

            // Essay type banner
            var typeBanner = new Panel
            {
                BackColor = Color.FromArgb(232, 250, 247),
                Location = new Point(20, y),
                Size = new Size(w, 40),
            };
            typeBanner.Paint += (s, e) =>
            {
                using var pen = new Pen(EssayAccent, 1.5f);
                e.Graphics.DrawRectangle(pen, 0, 0, typeBanner.Width - 1, typeBanner.Height - 1);
                using var bar = new SolidBrush(EssayAccent);
                e.Graphics.FillRectangle(bar, 0, 0, 4, typeBanner.Height);
            };
            typeBanner.Controls.Add(new Label
            {
                Text = "✍  Essay  —  Write your response below",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = EssayAccent,
                Location = new Point(16, 11),
                AutoSize = true,
                BackColor = Color.Transparent,
            });
            _scrollArea.Controls.Add(typeBanner);
            y += 50;

            var instrPnl = BuildInstructionsPanel(w);
            instrPnl.Location = new Point(20, y);
            _scrollArea.Controls.Add(instrPnl);
            y += instrPnl.MinimumSize.Height + 16;

            // Rubric before submission
            if (!submitted)
            {
                var rubricPnl = BuildStudentRubricPanel(w);
                if (rubricPnl.Height > 0)
                {
                    rubricPnl.Location = new Point(20, y);
                    _scrollArea.Controls.Add(rubricPnl);
                    y += rubricPnl.Height + 16;
                }
            }

            if (submitted)
            {
                _scrollArea.Controls.Add(new Label
                {
                    Text = $"✔  Essay Submitted  ·  {_activity.SubmittedAt:MMM dd, yyyy  h:mm tt}",
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
                        ForeColor = EssayAccent,
                        Location = new Point(20, y),
                        AutoSize = true
                    }); y += 32;

                    // Show rubric with scores after grading
                    var gradedRubric = BuildStudentRubricPanel(w);
                    if (gradedRubric.Height > 0)
                    {
                        gradedRubric.Location = new Point(20, y);
                        _scrollArea.Controls.Add(gradedRubric);
                        y += gradedRubric.Height + 16;
                    }
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
                    Text = "💾  Draft autosaved every 30 seconds.",
                    Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                    ForeColor = Color.FromArgb(0, 135, 0),
                    Location = new Point(20, y),
                    AutoSize = true
                };
                _scrollArea.Controls.Add(_lblAutosave);
                y += 26;

                var btnSub = new buttonRounded
                {
                    Text = "Submit Essay  ✔",
                    Size = new Size(164, 40),
                    Location = new Point(20, y),
                    BackColor = EssayAccent,
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

        // ════════════════════════════════════════════════════════════════
        //  FILE UPLOAD VIEW (green accent)
        // ════════════════════════════════════════════════════════════════
        private void BuildFileUploadView()
        {
            bool isSubm = _activity.SubmissionStatus is "Submitted" or "Returned";
            int w = Math.Max(640, _scrollArea.ClientSize.Width > 0
                                  ? _scrollArea.ClientSize.Width - 60
                                  : pnlBody.ClientSize.Width - 60);
            int y = 20;

            // FileUpload type banner
            var typeBanner = new Panel
            {
                BackColor = Color.FromArgb(232, 248, 234),
                Location = new Point(20, y),
                Size = new Size(w, 40),
            };
            typeBanner.Paint += (s, e) =>
            {
                using var pen = new Pen(FileUploadAccent, 1.5f);
                e.Graphics.DrawRectangle(pen, 0, 0, typeBanner.Width - 1, typeBanner.Height - 1);
                using var bar = new SolidBrush(FileUploadAccent);
                e.Graphics.FillRectangle(bar, 0, 0, 4, typeBanner.Height);
            };
            typeBanner.Controls.Add(new Label
            {
                Text = "📎  File Upload  —  Attach and submit your file",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = FileUploadAccent,
                Location = new Point(16, 11),
                AutoSize = true,
                BackColor = Color.Transparent,
            });
            _scrollArea.Controls.Add(typeBanner);
            y += 50;

            var instrPnl = BuildInstructionsPanel(w);
            instrPnl.Location = new Point(20, y);
            _scrollArea.Controls.Add(instrPnl);
            y += instrPnl.MinimumSize.Height + 20;

            y = BuildAttachmentList(y, w);

            if (isSubm)
            {
                _scrollArea.Controls.Add(new Label
                {
                    Text = $"✔  File Submitted  ·  {_activity.SubmittedAt:MMM dd, yyyy  h:mm tt}",
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
                        ForeColor = FileUploadAccent,
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
                    BackColor = Color.FromArgb(244, 250, 244),
                    BorderStyle = BorderStyle.FixedSingle,
                    Location = new Point(20, y),
                    Size = new Size(w, 100)
                };
                pnlDrop.Paint += (s, e) =>
                {
                    using var pen = new Pen(FileUploadAccent, 1f);
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlDrop.Width - 1, pnlDrop.Height - 1);
                };
                var lblHint = new Label
                {
                    Text = "Drag & drop your file here  —  or  —  click  Browse",
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = Color.FromArgb(60, 120, 80),
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                var btnBrowse = new buttonRounded
                {
                    Text = "Browse",
                    Size = new Size(96, 32),
                    Location = new Point(w - 106, 32),
                    BackColor = FileUploadAccent,
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
                    Text = string.IsNullOrEmpty(_uploadedFileName) ? "No file selected." : $"📎  {_uploadedFileName}",
                    Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                    ForeColor = string.IsNullOrEmpty(_uploadedFileName) ? Color.Gray : Color.FromArgb(0, 105, 0),
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
                    _activity.UploadedFilePath = _activity.UploadedFileName = "";
                    _activity.UploadedFileSize = 0;
                    if (_lblFileName != null) { _lblFileName.Text = "No file selected."; _lblFileName.ForeColor = Color.Gray; }
                    if (_lblFileSize != null) _lblFileSize.Text = "";
                    _btnRemoveFile.Visible = false;
                };
                _scrollArea.Controls.Add(_btnRemoveFile);
                y += 40;

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
                    Text = "Upload & Submit  ✔",
                    Size = new Size(180, 40),
                    Location = new Point(20, y),
                    BackColor = FileUploadAccent,
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

        // ════════════════════════════════════════════════════════════════
        //  QUIZ VIEW (indigo accent — all questions one page)
        // ════════════════════════════════════════════════════════════════
        private void BuildQuizView()
        {
            _scrollArea.Controls.Clear();

            if (_activity.Questions == null || _activity.Questions.Count == 0)
            {
                var pnlEmpty = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(250, 250, 252) };
                pnlEmpty.Controls.Add(new Label
                {
                    Text = "📋",
                    Font = new Font("Segoe UI", 36F),
                    ForeColor = Color.FromArgb(200, 200, 210),
                    AutoSize = true,
                    Location = new Point(0, 60),
                });
                pnlEmpty.Controls.Add(new Label
                {
                    Text = "No questions have been added to this quiz yet.",
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(160, 160, 170),
                    AutoSize = false,
                    Height = 30,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(0, 110)
                });
                pnlEmpty.Controls.Add(new Label
                {
                    Text = "Your instructor has not set up any questions for this activity.\nPlease check back later or contact your instructor.",
                    Font = new Font("Segoe UI", 9.5F),
                    ForeColor = Color.FromArgb(180, 180, 190),
                    AutoSize = false,
                    Height = 44,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(0, 148)
                });
                pnlEmpty.SizeChanged += (s, e) =>
                {
                    foreach (Control c in pnlEmpty.Controls)
                    {
                        if (c is Label lbl && !lbl.AutoSize)
                        { lbl.Width = pnlEmpty.Width - 40; lbl.Left = 20; }
                        else if (c is Label ico && ico.AutoSize)
                            ico.Left = (pnlEmpty.Width - ico.Width) / 2;
                    }
                };
                _scrollArea.Controls.Add(pnlEmpty);
                return;
            }

            bool isSubm = _activity.SubmissionStatus is "Submitted" or "Returned";
            int totalQ = _activity.Questions.Count;

            int GetW() => Math.Max(640,
                _scrollArea.ClientSize.Width > 40 ? _scrollArea.ClientSize.Width - 40 :
                pnlBody.ClientSize.Width > 40 ? pnlBody.ClientSize.Width - 40 : 900);

            int w = GetW();
            int y = 16;

            // Quiz type banner
            var typeBanner = new Panel
            {
                BackColor = Color.FromArgb(235, 238, 255),
                Location = new Point(16, y),
                Size = new Size(w, 40),
            };
            typeBanner.Paint += (s, e) =>
            {
                using var pen = new Pen(QuizAccent, 1.5f);
                e.Graphics.DrawRectangle(pen, 0, 0, typeBanner.Width - 1, typeBanner.Height - 1);
                using var bar = new SolidBrush(QuizAccent);
                e.Graphics.FillRectangle(bar, 0, 0, 4, typeBanner.Height);
            };
            typeBanner.Controls.Add(new Label
            {
                Text = $"❓  {(_activity.Type == "LongQuiz" ? "Long Quiz" : "Quiz")}  —  {totalQ} questions  ·  {_activity.Points} pts total",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = QuizAccent,
                Location = new Point(16, 11),
                AutoSize = true,
                BackColor = Color.Transparent,
            });
            _scrollArea.Controls.Add(typeBanner);
            y += 50;

            if (!string.IsNullOrEmpty(_activity.Instructions))
            {
                var instrPnl = BuildInstructionsPanel(w);
                instrPnl.Location = new Point(16, y);
                _scrollArea.Controls.Add(instrPnl);
                y += instrPnl.MinimumSize.Height + 16;
            }

            // Question number pills
            int pillSize = 28, pillGap = 4;
            var pnlPills = new Panel
            {
                Location = new Point(16, y),
                Size = new Size(w, pillSize + 8),
                BackColor = Color.Transparent
            };
            for (int i = 0; i < totalQ; i++)
            {
                bool ans = _answers.ContainsKey(i + 1);
                var dot = new Label
                {
                    Text = (i + 1).ToString(),
                    BackColor = ans ? Color.FromArgb(27, 110, 27) : Color.FromArgb(200, 200, 200),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    Location = new Point(i * (pillSize + pillGap), 4),
                    Size = new Size(pillSize, pillSize),
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                pnlPills.Controls.Add(dot);
            }
            _scrollArea.Controls.Add(pnlPills);
            y += pnlPills.Height + 12;

            // Questions
            for (int qi = 0; qi < totalQ; qi++)
            {
                var q = _activity.Questions[qi];
                int questionY = 0;

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

                // Question number header
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

                // Question text bar
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
                            BackColor = isSelected ? Color.FromArgb(245, 220, 220) : Color.White,
                            Cursor = isSubm ? Cursors.Default : Cursors.Hand
                        };
                        pnlChoice.Paint += (s, e) =>
                        {
                            using var pen = new Pen(Color.FromArgb(200, 170, 170));
                            e.Graphics.DrawLine(pen, 0, pnlChoice.Height - 1, pnlChoice.Width - 1, pnlChoice.Height - 1);
                        };

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
                            if (qi < pnlPills.Controls.Count)
                                pnlPills.Controls[qi].BackColor = Color.FromArgb(27, 110, 27);
                            foreach (Control ctrl in card.Controls)
                                if (ctrl is Panel cp && cp.Height == 44 && cp.Controls.Count >= 2
                                    && cp.Controls[1] is RadioButton r2)
                                    cp.BackColor = r2.Checked ? Color.FromArgb(245, 220, 220) : Color.White;
                        };
                        pnlChoice.Controls.Add(rb);

                        if (!isSubm)
                        {
                            pnlChoice.Click += (s, e) => rb.Checked = true;
                            pnlLetter.Click += (s, e) => rb.Checked = true;
                            foreach (Control cc in pnlLetter.Controls)
                                cc.Click += (s, e) => rb.Checked = true;
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
                                ? Color.FromArgb(200, 200, 200) : Color.FromArgb(27, 110, 27);
                    };
                    pnlAns.Controls.Add(txt);
                    card.Controls.Add(pnlAns);
                    questionY += 62;
                }
                else // Essay-type
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
                                ? Color.FromArgb(200, 200, 200) : Color.FromArgb(27, 110, 27);
                    };
                    pnlAns.Controls.Add(txtE);
                    card.Controls.Add(pnlAns);
                    questionY += 180;
                }

                card.Height = questionY;
                _scrollArea.Controls.Add(card);
                y += questionY + 16;
            }

            if (!isSubm)
            {
                var btnSubmit = new buttonRounded
                {
                    Text = "Submit Quiz  ✔",
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
                    Text = $"✔  Quiz submitted  ·  {_activity.SubmittedAt:MMM dd, yyyy  h:mm tt}",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(27, 110, 27),
                    Location = new Point(16, y),
                    AutoSize = true
                }); y += 32;

                if (_activity.Score.HasValue)
                    _scrollArea.Controls.Add(new Label
                    {
                        Text = $"Score:  {_activity.Score} / {_activity.Points} pts",
                        Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                        ForeColor = QuizAccent,
                        Location = new Point(16, y),
                        AutoSize = true
                    });
            }
        }

        // ════════════════════════════════════════════════════════════════
        //  RECITATION VIEW
        // ════════════════════════════════════════════════════════════════
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
                Text = "ℹ  Your instructor will record your participation score during the class session.",
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
                    Text = $"✔  Marked as Attended  ·  {_activity.SubmittedAt:MMM dd, yyyy  h:mm tt}",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(27, 110, 27),
                    Location = new Point(20, y),
                    AutoSize = true
                }); y += 32;

                if (_activity.Score.HasValue)
                    _scrollArea.Controls.Add(new Label
                    {
                        Text = $"Score:  {_activity.Score} / {_activity.Points} pts",
                        Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                        ForeColor = AssignmentAccent,
                        Location = new Point(20, y),
                        AutoSize = true
                    });

                if (!string.IsNullOrEmpty(_activity.Remarks))
                    _scrollArea.Controls.Add(BuildRemarksPanel(w, y + 36));
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
                    Text = "Mark as Attended  ✔",
                    Size = new Size(180, 40),
                    Location = new Point(20, y),
                    BackColor = AssignmentAccent,
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
                    if (!SaveSubmissionToDb()) return;
                    MessageBox.Show("Marked as attended!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OnBack?.Invoke();
                };
                _scrollArea.Controls.Add(btnMark);
            }
        }

        // ════════════════════════════════════════════════════════════════
        //  FILE BROWSE & SUBMIT
        // ════════════════════════════════════════════════════════════════
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
                _activity.UploadedFilePath = _uploadedFilePath;
                _activity.UploadedFileSize = _uploadedFileSize;

                if (_lblFileName != null) { _lblFileName.Text = $"📎  {_uploadedFileName}"; _lblFileName.ForeColor = Color.FromArgb(0, 105, 0); }
                if (_lblFileSize != null) _lblFileSize.Text = $"  ({FormatFileSize(_uploadedFileSize)})";
                if (_btnRemoveFile != null) _btnRemoveFile.Visible = true;
            }
        }

        private void SubmitFileUpload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_uploadedFileName))
            {
                MessageBox.Show("Please select a file before submitting.", "No File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var r = MessageBox.Show($"Submit \"{_uploadedFileName}\"?\n\nThis action cannot be undone.",
                "Confirm Upload", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            _activity.SubmissionStatus = "Submitted";
            _activity.SubmittedAt = DateTime.Now;
            if (!SaveSubmissionToDb()) return;

            MessageBox.Show("File uploaded and submitted successfully! ✔", "Submitted",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            OnBack?.Invoke();
        }

        private void SubmitEssay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtEssay?.Text))
            {
                MessageBox.Show("Please write your essay before submitting.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var r = MessageBox.Show("Submit your essay now? This action cannot be undone.",
                "Confirm Submission", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            _activity.EssayDraft = _txtEssay.Text;
            _activity.SubmissionStatus = "Submitted";
            _activity.SubmittedAt = DateTime.Now;
            if (!SaveSubmissionToDb()) return;

            MessageBox.Show("Essay submitted successfully! ✔", "Submitted",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            OnBack?.Invoke();
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
            if (!SaveSubmissionToDb()) return;

            MessageBox.Show("Quiz submitted successfully! ✔", "Submitted",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            OnBack?.Invoke();
        }

        private bool SaveSubmissionToDb()
        {
            if (!_useDb) return true;
            try { _svc.SubmitActivity(_studentId, _activity); return true; }
            catch (Exception ex)
            {
                MessageBox.Show($"Submission failed:\n{ex.Message}", "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
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
                _lblAutosave.Text = $"💾  Draft autosaved at {DateTime.Now:h:mm:ss tt}";
        }

        private static string FormatFileSize(long bytes)
        {
            if (bytes >= 1_048_576) return $"{bytes / 1_048_576.0:F1} MB";
            if (bytes >= 1_024) return $"{bytes / 1_024.0:F0} KB";
            return $"{bytes} B";
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            AutosaveDraft();
            OnBack?.Invoke();
        }
    }
}