using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Student.LMS.Course
{
    public partial class StudentActivityList : UserControl
    {
        public event Action OnBack;
        public event Action<StudentActivityItem> OnOpenActivity;

        private StudentCourse _course;
        private List<StudentActivityItem> _activities = new();
        private System.Windows.Forms.Timer _searchTimer;

        public StudentActivityList(StudentCourse course)
        {
            _course = course;
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            lblTitle.Text = course.Name + "  \u2014  " + course.Code;
            lblInstructor.Text = "\uD83D\uDC64  " + course.Instructor + "    \uD83D\uDD50  " + course.Schedule;

            _searchTimer = new System.Windows.Forms.Timer { Interval = 180 };
            _searchTimer.Tick += (s, e) => { _searchTimer.Stop(); RenderRows(); };

            cmbFilter.SelectedIndex = 0;
            cmbStatus.SelectedIndex = 0;

            LoadSampleActivities();
            RebuildSummaryPills();

            this.Load += (s, e) => RenderRows();
            flp.SizeChanged += flp_SizeChanged;
        }

        // ── Sample data ──────────────────────────────────────────────────────

        private void LoadSampleActivities()
        {
            _activities = new List<StudentActivityItem>
            {
                new StudentActivityItem
                {
                    Id=1, Title="System Simulations Quiz", Type="Quiz",
                    Deadline=DateTime.Now.AddDays(3), Points=50,
                    SubmissionStatus="Submitted", SubmittedAt=DateTime.Now.AddDays(-1), Score=45,
                    Instructions="Answer all 25 questions within the time limit. No back-navigation allowed.",
                    Questions=SampleQuizQuestions()
                },
                new StudentActivityItem
                {
                    Id=2, Title="Integrating Methodologies Report", Type="FileUpload",
                    Deadline=DateTime.Now.AddDays(7), Points=100,
                    SubmissionStatus="Returned", SubmittedAt=DateTime.Now.AddDays(-3),
                    Score=88, ReturnedAt=DateTime.Now.AddDays(-1),
                    Remarks="Good analysis overall. Please elaborate on the integration methodology in section 3. References need to follow APA format.",
                    ScoreBreakdown="Content: 35/40  |  Format: 18/20  |  References: 20/25  |  Clarity: 15/15",
                    Suggestions="Strengthen section 3 with at least two additional peer-reviewed sources. Revisit APA 7th edition citation rules.",
                    Instructions="Upload your completed report (PDF or DOCX). Max file size: 10 MB.",
                    Attachments=new List<ActivityAttachment>
                    {
                        new ActivityAttachment { FileName="Report_Template.docx", FileType="docx", FileSize=48_200 },
                        new ActivityAttachment { FileName="Rubric.pdf",           FileType="pdf",  FileSize=124_000 }
                    }
                },
                new StudentActivityItem
                {
                    Id=3, Title="HCI Reflection Essay", Type="Essay",
                    Deadline=DateTime.Now.AddDays(14), Points=75,
                    SubmissionStatus="Pending",
                    Instructions="Write a comprehensive essay (800\u20131200 words) on Human-Computer Interaction principles and their real-world application.",
                    EssayDraft=""
                },
                new StudentActivityItem
                {
                    Id=4, Title="Programming Lab Activity 1", Type="FileUpload",
                    Deadline=DateTime.Now.AddDays(1), Points=50,
                    SubmissionStatus="Pending",
                    Instructions="Complete the Java lab exercise, zip your project folder, and upload before the deadline.",
                    Attachments=new List<ActivityAttachment>
                    {
                        new ActivityAttachment { FileName="Lab1_Instructions.pdf", FileType="pdf", FileSize=220_000 }
                    }
                },
                new StudentActivityItem
                {
                    Id=5, Title="Midterm Long Quiz", Type="LongQuiz",
                    Deadline=DateTime.Now.AddDays(-2), Points=100,
                    SubmissionStatus="Submitted", SubmittedAt=DateTime.Now.AddDays(-2).AddHours(-1), Score=92,
                    Instructions="Covers Modules 1\u20135. 50 items. You have 90 minutes.",
                    Questions=SampleQuizQuestions()
                },
                new StudentActivityItem
                {
                    Id=6, Title="Week 3 Recitation", Type="Recitation",
                    Deadline=DateTime.Now.AddDays(-5), Points=25,
                    SubmissionStatus="Overdue",
                    Instructions="Oral recitation on Chapter 3 topics. Present your answers clearly."
                },
                new StudentActivityItem
                {
                    Id=7, Title="Data Structures Quiz", Type="Quiz",
                    Deadline=DateTime.Now.AddDays(5), Points=30,
                    SubmissionStatus="Pending",
                    Instructions="15 items \u2014 Multiple choice and identification. 30 minutes only.",
                    Questions=SampleQuizQuestions()
                },
            };
        }

        private static List<ActivityQuestion> SampleQuizQuestions()
        {
            return new List<ActivityQuestion>
            {
                new ActivityQuestion { Number=1, QuestionType="MultipleChoice", Points=2,
                    Text="Which of the following is NOT a characteristic of an algorithm?",
                    Choices=new List<string>{ "Finiteness","Definiteness","Randomness","Input" },
                    CorrectAnswer="Randomness" },
                new ActivityQuestion { Number=2, QuestionType="TrueFalse", Points=1,
                    Text="A stack follows the First-In, First-Out (FIFO) principle.",
                    Choices=new List<string>{ "True","False" }, CorrectAnswer="False" },
                new ActivityQuestion { Number=3, QuestionType="Identification", Points=2,
                    Text="What data structure uses LIFO (Last-In, First-Out) ordering?",
                    CorrectAnswer="Stack" },
                new ActivityQuestion { Number=4, QuestionType="MultipleChoice", Points=2,
                    Text="What is the time complexity of binary search?",
                    Choices=new List<string>{ "O(n)","O(log n)","O(n\u00B2)","O(1)" },
                    CorrectAnswer="O(log n)" },
                new ActivityQuestion { Number=5, QuestionType="Essay", Points=5,
                    Text="Explain the difference between a stack and a queue. Provide a real-world analogy for each." },
            };
        }

        // ── Summary pills ─────────────────────────────────────────────────────

        private void RebuildSummaryPills()
        {
            pnlSummary.Controls.Clear();

            int pend = 0, sub = 0, over = 0;
            foreach (var a in _activities)
            {
                if (a.EffectiveStatus == "Pending") pend++;
                if (a.EffectiveStatus is "Submitted" or "Returned") sub++;
                if (a.EffectiveStatus == "Overdue") over++;
            }

            var pills = new (string label, Color bg, Color fg)[]
            {
                ($"Pending: {pend}",   Color.FromArgb(255,234,200), Color.FromArgb(180,70,0)),
                ($"\u2714 Done: {sub}",     Color.FromArgb(210,245,215), Color.FromArgb(27,110,27)),
                ($"\u26A0 Overdue: {over}", Color.FromArgb(255,215,215), Color.FromArgb(180,20,20)),
            };

            int x = 0;
            foreach (var (label, bg, fg) in pills)
            {
                var lbl = new Label
                {
                    Text = label,
                    BackColor = bg,
                    ForeColor = fg,
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    Location = new Point(x, 6),
                    Size = new Size(138, 22),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Padding = new Padding(4, 0, 4, 0)
                };
                pnlSummary.Controls.Add(lbl);
                x += 148;
            }
        }

        // ── Render ────────────────────────────────────────────────────────────

        private void RenderRows()
        {
            if (flp.ClientSize.Width < 100) return;

            flp.SuspendLayout();
            flp.Controls.Clear();

            string search = txtSearch?.Text?.Trim().ToLower() ?? "";
            string filter = cmbFilter?.SelectedItem?.ToString() ?? "All";
            string status = cmbStatus?.SelectedItem?.ToString() ?? "All Status";

            foreach (var act in _activities)
            {
                if (filter != "All" && act.Type != filter) continue;
                if (status != "All Status" && !act.EffectiveStatus.Equals(
                        status, StringComparison.OrdinalIgnoreCase)) continue;
                if (!string.IsNullOrEmpty(search) &&
                    !act.Title.ToLower().Contains(search)) continue;

                flp.Controls.Add(BuildRow(act));
            }

            if (flp.Controls.Count == 0)
            {
                flp.Controls.Add(new Label
                {
                    Text = "No activities match your search.",
                    Font = new Font("Segoe UI", 11F),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Margin = new Padding(20)
                });
            }

            flp.ResumeLayout(true);
        }

        // ── Row builder ───────────────────────────────────────────────────────

        private Panel BuildRow(StudentActivityItem act)
        {
            string effStatus = act.EffectiveStatus;
            bool isReturned = effStatus == "Returned";
            bool isSubmitted = effStatus == "Submitted";
            bool isOverdue = effStatus == "Overdue";

            int rowH = isReturned ? 118 : 84;
            int rowW = Math.Max(720, flp.ClientSize.Width - 28);

            var row = new Panel
            {
                Width = rowW,
                Height = rowH,
                BackColor = Color.White,
                Margin = new Padding(4, 4, 4, 0)
            };

            row.Paint += (s, e) =>
            {
                using var p = new Pen(Color.FromArgb(232, 232, 232));
                e.Graphics.DrawLine(p, 6, row.Height - 1, row.Width - 1, row.Height - 1);
            };

            // ── Left accent bar ───────────────────────────────────────────────
            Color typeColor = act.Type switch
            {
                "Quiz" => Color.FromArgb(63, 81, 181),
                "LongQuiz" => Color.FromArgb(21, 101, 192),
                "Essay" => Color.FromArgb(156, 39, 176),
                "FileUpload" => Color.FromArgb(255, 152, 0),
                "Recitation" => Color.FromArgb(0, 150, 136),
                _ => Color.Gray
            };
            row.Controls.Add(new Panel { Width = 6, Dock = DockStyle.Left, BackColor = typeColor });

            // ── Type badge ────────────────────────────────────────────────────
            string typeDisplay = act.Type switch
            {
                "FileUpload" => "File Upload",
                "LongQuiz" => "Long Quiz",
                _ => act.Type
            };
            row.Controls.Add(new Label
            {
                Text = typeDisplay,
                BackColor = typeColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                Location = new Point(16, isReturned ? 46 : 38),
                Size = new Size(86, 20),
                TextAlign = ContentAlignment.MiddleCenter
            });

            // ── Title ─────────────────────────────────────────────────────────
            row.Controls.Add(new Label
            {
                Text = act.Title,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 22, 22),
                Location = new Point(16, 8),
                Size = new Size(380, 24),
                AutoEllipsis = true
            });

            // ── Deadline ──────────────────────────────────────────────────────
            TimeSpan ts = act.Deadline - DateTime.Now;
            string dueText = ts.TotalDays < 0 ? "(Overdue)" :
                             ts.Days == 0 ? "(Due Today!)" :
                             ts.Days == 1 ? "(Due Tomorrow)" :
                                                $"(Due in {ts.Days}d)";
            Color dueColor = ts.TotalDays < 0 ? Color.Red :
                             ts.Days <= 1 ? Color.OrangeRed :
                                                Color.FromArgb(50, 120, 50);
            row.Controls.Add(new Label
            {
                Text = $"\uD83D\uDCC5  {act.Deadline:MMM dd, yyyy  hh:mm tt}   {dueText}",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = dueColor,
                Location = new Point(110, 10),
                Size = new Size(360, 18)
            });

            // ── Points / score ────────────────────────────────────────────────
            string ptsText = $"\uD83C\uDFC6  {act.Points} pts";
            if (act.Score.HasValue)
                ptsText += $"     Score: {act.Score} / {act.Points}";
            row.Controls.Add(new Label
            {
                Text = ptsText,
                Font = new Font("Segoe UI", 8.5F, act.Score.HasValue ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = act.Score.HasValue ? Color.FromArgb(128, 0, 0) : Color.FromArgb(80, 80, 80),
                Location = new Point(110, 34),
                Size = new Size(380, 18)
            });

            // ── Status badge ──────────────────────────────────────────────────
            (Color bg, Color fg, string icon) st = effStatus switch
            {
                "Submitted" => (Color.FromArgb(215, 245, 215), Color.FromArgb(27, 110, 27), "\u2714  Submitted"),
                "Returned" => (Color.FromArgb(215, 225, 255), Color.FromArgb(30, 60, 180), "\u21A9  Returned"),
                "Late" => (Color.FromArgb(255, 235, 210), Color.FromArgb(180, 70, 0), "\u23F1  Late"),
                "Overdue" => (Color.FromArgb(255, 215, 215), Color.FromArgb(180, 20, 20), "\u26A0  Overdue"),
                _ => (Color.FromArgb(232, 232, 232), Color.FromArgb(80, 80, 80), "\u25CB  Pending")
            };
            row.Controls.Add(new Label
            {
                Text = st.icon,
                BackColor = st.bg,
                ForeColor = st.fg,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Location = new Point(510, isReturned ? 18 : 30),
                Size = new Size(108, 24),
                TextAlign = ContentAlignment.MiddleCenter
            });

            // ── Returned: remarks preview ─────────────────────────────────────
            if (isReturned && !string.IsNullOrEmpty(act.Remarks))
            {
                row.Controls.Add(new Label
                {
                    Text = $"\uD83D\uDCDD  {act.Remarks}",
                    Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                    ForeColor = Color.FromArgb(60, 80, 160),
                    Location = new Point(16, 70),
                    Size = new Size(rowW - 320, 38),
                    AutoEllipsis = true
                });
                if (act.ReturnedAt.HasValue)
                    row.Controls.Add(new Label
                    {
                        Text = $"Returned  {act.ReturnedAt:MMM dd, yyyy  h:mm tt}",
                        Font = new Font("Segoe UI", 7.5F),
                        ForeColor = Color.Gray,
                        Location = new Point(510, 50),
                        Size = new Size(174, 14)
                    });
            }

            // ── Action buttons ────────────────────────────────────────────────
            // FIX (Image 2): items that are Returned cannot be answered again.
            bool locked = act.LockAfterDeadline && act.Deadline < DateTime.Now
                              && act.SubmissionStatus == "Pending";
            bool canAnswer = !locked && !isReturned && !isSubmitted;   // <-- key fix

            string btnLbl = locked ? "Locked \uD83D\uDD12" :
                            isReturned ? "Returned" :
                            isSubmitted ? "View Submission" :
                            isOverdue ? "Take Activity" :
                                         "Take Activity";

            Color btnColor = locked ? Color.FromArgb(155, 155, 155) :
                             isReturned ? Color.FromArgb(100, 100, 160) :
                             isSubmitted ? Color.FromArgb(60, 60, 60) :
                                          Color.Maroon;

            var btn = new buttonRounded
            {
                Text = btnLbl,
                Size = new Size(148, 36),
                BackColor = btnColor,
                ForeColor = Color.White,
                BorderRadius = 18,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Tag = act,
                // Returned items: still clickable to open read-only view; locked items disabled.
                Enabled = !locked,
                Cursor = locked ? Cursors.No : Cursors.Hand,
                FlatStyle = FlatStyle.Flat
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Location = new Point(rowW - btn.Width - 14,
                                     (rowH - btn.Height) / 2 - (isReturned ? 18 : 0));
            btn.Click += (s, e) =>
            {
                if (s is buttonRounded b && b.Tag is StudentActivityItem a)
                    OnOpenActivity?.Invoke(a);
            };
            row.Controls.Add(btn);

            // Remarks button (returned only)
            if (isReturned)
            {
                var btnRmk = new buttonRounded
                {
                    Text = "View Remarks",
                    Size = new Size(132, 28),
                    BackColor = Color.FromArgb(40, 70, 200),
                    ForeColor = Color.White,
                    BorderRadius = 14,
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    Tag = act,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnRmk.FlatAppearance.BorderSize = 0;
                btnRmk.Location = new Point(rowW - btnRmk.Width - 14, btn.Bottom + 8);
                btnRmk.Click += (s, e) => ShowRemarksDialog(act);
                row.Controls.Add(btnRmk);
            }

            return row;
        }

        // ── Remarks dialog ────────────────────────────────────────────────────
        // FIX (Image 1): No system close button — dialog is closed only via the
        //                maroon "Close" button inside the body.

        private void ShowRemarksDialog(StudentActivityItem act)
        {
            using var dlg = new Form
            {
                // FormBorderStyle.None removes the title-bar including the X button.
                FormBorderStyle = FormBorderStyle.None,
                Size = new Size(600, 520),
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.White,
                MaximizeBox = false,
                MinimizeBox = false,
                ShowInTaskbar = false
            };

            // Thin border drawn around the whole dialog
            dlg.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(180, 180, 180));
                e.Graphics.DrawRectangle(pen, 0, 0, dlg.Width - 1, dlg.Height - 1);
            };

            // ── Maroon header (replaces system title bar) ─────────────────────
            var pnlH = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = Color.Maroon
            };
            pnlH.Controls.Add(new Label
            {
                Text = act.Title,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(16, 0, 0, 0)
            });
            // Allow dragging the borderless dialog by dragging its header
            bool dragging = false;
            System.Drawing.Point dragStart = default;
            pnlH.MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) { dragging = true; dragStart = e.Location; } };
            pnlH.MouseMove += (s, e) => { if (dragging) dlg.Location = new System.Drawing.Point(dlg.Left + e.X - dragStart.X, dlg.Top + e.Y - dragStart.Y); };
            pnlH.MouseUp += (s, e) => dragging = false;
            dlg.Controls.Add(pnlH);

            // ── Scrollable body ───────────────────────────────────────────────
            var body = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20)
            };
            int y = 16;
            int bodyW = 530;

            if (!string.IsNullOrEmpty(act.ScoreBreakdown))
            {
                body.Controls.Add(new Label
                {
                    Text = "Score Breakdown:",
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(30, 30, 30),
                    Location = new Point(16, y),
                    AutoSize = true
                }); y += 22;

                body.Controls.Add(new Label
                {
                    Text = act.ScoreBreakdown,
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.FromArgb(70, 70, 70),
                    Location = new Point(16, y),
                    Size = new Size(bodyW, 18)
                }); y += 30;
            }

            body.Controls.Add(new Label
            {
                Text = "Instructor Feedback:",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 50, 160),
                Location = new Point(16, y),
                AutoSize = true
            }); y += 22;

            int feedbackH = Math.Max(70, MeasureTextHeight(act.Remarks, new Font("Segoe UI", 9.5F), bodyW - 24) + 24);
            var pnlFeed = new Panel
            {
                BackColor = Color.FromArgb(232, 238, 255),
                Location = new Point(16, y),
                Size = new Size(bodyW, feedbackH),
                Padding = new Padding(12)
            };
            pnlFeed.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(190, 205, 240));
                e.Graphics.DrawRectangle(pen, 0, 0, pnlFeed.Width - 1, pnlFeed.Height - 1);
            };
            pnlFeed.Controls.Add(new Label
            {
                Text = act.Remarks,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(30, 30, 80),
                Location = new Point(12, 10),
                Size = new Size(bodyW - 28, feedbackH - 20)
            });
            body.Controls.Add(pnlFeed);
            y += feedbackH + 16;

            if (!string.IsNullOrEmpty(act.Suggestions))
            {
                body.Controls.Add(new Label
                {
                    Text = "Suggestions for Improvement:",
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(50, 50, 50),
                    Location = new Point(16, y),
                    AutoSize = true
                }); y += 22;

                int suggH = Math.Max(60, MeasureTextHeight(act.Suggestions, new Font("Segoe UI", 9F), bodyW - 24) + 24);
                var pnlSugg = new Panel
                {
                    BackColor = Color.FromArgb(255, 248, 220),
                    Location = new Point(16, y),
                    Size = new Size(bodyW, suggH),
                    Padding = new Padding(12)
                };
                pnlSugg.Paint += (s, e) =>
                {
                    using var pen = new Pen(Color.FromArgb(240, 210, 140));
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlSugg.Width - 1, pnlSugg.Height - 1);
                };
                pnlSugg.Controls.Add(new Label
                {
                    Text = act.Suggestions,
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.FromArgb(100, 70, 0),
                    Location = new Point(12, 10),
                    Size = new Size(bodyW - 28, suggH - 20)
                });
                body.Controls.Add(pnlSugg);
                y += suggH + 16;
            }

            if (act.ReturnedAt.HasValue)
            {
                body.Controls.Add(new Label
                {
                    Text = $"Returned on  {act.ReturnedAt:dddd, MMMM dd, yyyy  h:mm tt}",
                    Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    Location = new Point(16, y),
                    AutoSize = true
                }); y += 28;
            }

            // Close button (only way to close — no system X)
            var btnClose = new buttonRounded
            {
                Text = "Close",
                Size = new Size(100, 36),
                BackColor = Color.Maroon,
                ForeColor = Color.White,
                BorderRadius = 18,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(bodyW - 84, y + 4),
                DialogResult = DialogResult.OK
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => dlg.Close();
            body.Controls.Add(btnClose);

            dlg.Controls.Add(body);
            dlg.AcceptButton = btnClose;
            dlg.ShowDialog(this);
        }

        private static int MeasureTextHeight(string text, Font font, int maxWidth)
        {
            if (string.IsNullOrEmpty(text)) return 0;
            var sz = TextRenderer.MeasureText(text, font,
                new Size(maxWidth, int.MaxValue),
                TextFormatFlags.WordBreak);
            return sz.Height;
        }

        // ── Events ────────────────────────────────────────────────────────────

        private void btnBack_Click(object sender, EventArgs e) => OnBack?.Invoke();
        private void txtSearch_TextChanged(object sender, EventArgs e) { _searchTimer.Stop(); _searchTimer.Start(); }
        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e) => RenderRows();
        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e) => RenderRows();
        private void flp_SizeChanged(object sender, EventArgs e) => RenderRows();
    }
}