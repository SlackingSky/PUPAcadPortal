using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class StudentActivityList : UserControl
    {
        public event Action OnBack;
        public event Action<StudentActivityItem> OnOpenActivity;

        private StudentCourse _course;
        private List<StudentActivityItem> _activities;
        private System.Windows.Forms.Timer _searchTimer;

        public StudentActivityList(StudentCourse course)
        {
            _course = course;
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            lblTitle.Text = course.Name + "  —  " + course.Code;

            _searchTimer = new System.Windows.Forms.Timer { Interval = 180 };
            _searchTimer.Tick += (s, e) => { _searchTimer.Stop(); RenderRows(); };

            LoadSampleActivities();
            this.Load += (s, e) => RenderRows();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) { _searchTimer?.Dispose(); components?.Dispose(); }
            base.Dispose(disposing);
        }

        //  Sample activities (all types, all statuses) 
        private void LoadSampleActivities()
        {
            _activities = new List<StudentActivityItem>
            {
                new StudentActivityItem
                {
                    Id=1, Title="System Simulations", Type="Quiz",
                    Deadline = DateTime.Now.AddDays(3), Points=50,
                    SubmissionStatus = "Submitted", SubmittedAt = DateTime.Now.AddDays(-1),
                    Score = 45,
                    Instructions = "Answer all questions within the time limit. No back-navigation allowed.",
                    Questions = SampleQuizQuestions()
                },
                new StudentActivityItem
                {
                    Id=2, Title="Integrating Methodologies", Type="FileUpload",
                    Deadline = DateTime.Now.AddDays(7), Points=100,
                    SubmissionStatus = "Returned", SubmittedAt = DateTime.Now.AddDays(-3),
                    Score = 88, ReturnedAt = DateTime.Now.AddDays(-1),
                    Remarks = "Good analysis overall. Please elaborate on the integration methodology in section 3. References need to follow APA format.",
                    Instructions = "Upload your completed report (PDF or DOCX). Max file size: 10 MB."
                },
                new StudentActivityItem
                {
                    Id=3, Title="HCI Essay", Type="Essay",
                    Deadline = DateTime.Now.AddDays(14), Points=75,
                    SubmissionStatus = "Pending",
                    Instructions = "Write a comprehensive essay (800–1200 words) on Human-Computer Interaction principles and their real-world application."
                },
                new StudentActivityItem
                {
                    Id=4, Title="Programming Lab Activity 1", Type="FileUpload",
                    Deadline = DateTime.Now.AddDays(1), Points=50,
                    SubmissionStatus = "Pending",
                    Instructions = "Complete the programming activity (Java), zip your project folder, and upload before the deadline."
                },
                new StudentActivityItem
                {
                    Id=5, Title="Midterm Long Quiz", Type="LongQuiz",
                    Deadline = DateTime.Now.AddDays(-2), Points=100,
                    SubmissionStatus = "Submitted", SubmittedAt = DateTime.Now.AddDays(-2).AddHours(-1),
                    Score = 92,
                    Instructions = "Covers Modules 1–5. 50 items. You have 90 minutes.",
                    Questions = SampleQuizQuestions()
                },
                new StudentActivityItem
                {
                    Id=6, Title="Week 3 Recitation", Type="Recitation",
                    Deadline = DateTime.Now.AddDays(-5), Points=25,
                    SubmissionStatus = "Overdue",
                    Instructions = "Oral recitation on Chapter 3 topics."
                },
                new StudentActivityItem
                {
                    Id=7, Title="Data Structures Quiz", Type="Quiz",
                    Deadline = DateTime.Now.AddDays(5), Points=30,
                    SubmissionStatus = "Pending",
                    Instructions = "15 items. Multiple choice and identification.",
                    Questions = SampleQuizQuestions()
                },
            };
        }

        private static List<ActivityQuestion> SampleQuizQuestions()
        {
            return new List<ActivityQuestion>
            {
                new ActivityQuestion { Number=1, QuestionType="MultipleChoice", Points=2,
                    Text="Which of the following is NOT a characteristic of an algorithm?",
                    Choices=new List<string>{ "Finiteness", "Definiteness", "Randomness", "Input" },
                    CorrectAnswer="Randomness" },
                new ActivityQuestion { Number=2, QuestionType="TrueFalse", Points=1,
                    Text="A stack follows the First-In, First-Out (FIFO) principle.",
                    Choices=new List<string>{ "True", "False" },
                    CorrectAnswer="False" },
                new ActivityQuestion { Number=3, QuestionType="Identification", Points=2,
                    Text="What data structure uses LIFO (Last-In, First-Out) ordering?",
                    CorrectAnswer="Stack" },
                new ActivityQuestion { Number=4, QuestionType="MultipleChoice", Points=2,
                    Text="What is the time complexity of binary search?",
                    Choices=new List<string>{ "O(n)", "O(log n)", "O(n²)", "O(1)" },
                    CorrectAnswer="O(log n)" },
                new ActivityQuestion { Number=5, QuestionType="Essay", Points=5,
                    Text="Explain the difference between a stack and a queue. Provide a real-world analogy for each." },
            };
        }

        //  Render 
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

                if (status != "All Status" && !act.SubmissionStatus.Equals(
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

        //  Row builder 
        private Panel BuildRow(StudentActivityItem act)
        {
            bool isReturned = act.SubmissionStatus == "Returned";
            bool isSubmitted = act.SubmissionStatus == "Submitted";
            bool isOverdue = act.SubmissionStatus == "Overdue" ||
                               (act.SubmissionStatus == "Pending" && act.Deadline < DateTime.Now);

            // Adjust effective status for overdue pending
            string effectiveStatus = isOverdue && act.SubmissionStatus == "Pending"
                                     ? "Overdue"
                                     : act.SubmissionStatus;

            int rowH = isReturned ? 110 : 80;
            int rowW = Math.Max(700, flp.ClientSize.Width - 24);

            var row = new Panel
            {
                Width = rowW,
                Height = rowH,
                BackColor = Color.White,
                Margin = new Padding(4, 4, 4, 0)
            };

            // Left accent bar
            Color typeColor = act.Type switch
            {
                "Quiz" => Color.FromArgb(63, 81, 181),
                "LongQuiz" => Color.FromArgb(21, 101, 192),
                "Essay" => Color.FromArgb(156, 39, 176),
                "FileUpload" => Color.FromArgb(255, 152, 0),
                "Recitation" => Color.FromArgb(0, 150, 136),
                _ => Color.Gray
            };

            row.Controls.Add(new Panel
            {
                Width = 6,
                Dock = DockStyle.Left,
                BackColor = typeColor
            });

            // Type badge
            string typeDisplay = act.Type == "FileUpload" ? "File Upload" :
                                 act.Type == "LongQuiz" ? "Long Quiz" : act.Type;

            row.Controls.Add(new Label
            {
                Text = typeDisplay,
                BackColor = typeColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                Location = new Point(16, isReturned ? 40 : 34),
                Size = new Size(82, 18),
                TextAlign = ContentAlignment.MiddleCenter
            });

            // Title
            row.Controls.Add(new Label
            {
                Text = act.Title,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 25, 25),
                Location = new Point(16, 8),
                Size = new Size(350, 22),
                AutoEllipsis = true
            });

            // Deadline
            TimeSpan ts = act.Deadline - DateTime.Now;
            string dueText = ts.TotalDays < 0 ? "(Overdue)" :
                               ts.Days == 0 ? "(Due Today)" :
                               ts.Days == 1 ? "(Due Tomorrow)" :
                                                   $"(Due in {ts.Days}d)";
            Color dueColor = ts.TotalDays < 0 ? Color.Red :
                               ts.Days <= 1 ? Color.OrangeRed :
                                                   Color.FromArgb(60, 120, 60);

            row.Controls.Add(new Label
            {
                Text = $"📅  {act.Deadline:MMM dd, yyyy}  {dueText}",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = dueColor,
                Location = new Point(108, 10),
                Size = new Size(310, 18)
            });

            // Points
            row.Controls.Add(new Label
            {
                Text = $"🏆  {act.Points} pts" + (act.Score.HasValue ? $"   |   Score: {act.Score}/{act.Points}" : ""),
                Font = new Font("Segoe UI", 8.5F, act.Score.HasValue ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = act.Score.HasValue ? Color.FromArgb(128, 0, 0) : Color.FromArgb(80, 80, 80),
                Location = new Point(108, 34),
                Size = new Size(360, 18)
            });

            //  Status badge 
            (Color bg, Color fg, string icon) statusStyle = effectiveStatus switch
            {
                "Submitted" => (Color.FromArgb(220, 245, 220), Color.FromArgb(27, 110, 27), "✔ Submitted"),
                "Returned" => (Color.FromArgb(220, 230, 255), Color.FromArgb(30, 60, 180), "↩ Returned"),
                "Late" => (Color.FromArgb(255, 235, 210), Color.FromArgb(180, 70, 0), "⏱ Late"),
                "Overdue" => (Color.FromArgb(255, 220, 220), Color.FromArgb(180, 20, 20), "⚠ Overdue"),
                _ => (Color.FromArgb(235, 235, 235), Color.FromArgb(80, 80, 80), "○ Pending")
            };

            row.Controls.Add(new Label
            {
                Text = statusStyle.icon,
                BackColor = statusStyle.bg,
                ForeColor = statusStyle.fg,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Location = new Point(490, isReturned ? 16 : 28),
                Size = new Size(100, 22),
                TextAlign = ContentAlignment.MiddleCenter
            });

            //  Returned: show remarks section 
            if (isReturned && !string.IsNullOrEmpty(act.Remarks))
            {
                row.Controls.Add(new Label
                {
                    Text = $"📝 Instructor Remarks:  {act.Remarks}",
                    Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                    ForeColor = Color.FromArgb(60, 80, 160),
                    Location = new Point(16, 64),
                    Size = new Size(rowW - 220, 36),
                    AutoEllipsis = true
                });

                if (act.ReturnedAt.HasValue)
                {
                    row.Controls.Add(new Label
                    {
                        Text = $"Returned: {act.ReturnedAt:MMM dd, yyyy  h:mm tt}",
                        Font = new Font("Segoe UI", 7.5F),
                        ForeColor = Color.Gray,
                        Location = new Point(490, 44),
                        Size = new Size(160, 14)
                    });
                }
            }

            //  Single action button 
            bool locked = act.LockAfterDeadline && act.Deadline < DateTime.Now
                                  && act.SubmissionStatus == "Pending";
            string btnLabel = locked ? "Locked 🔒" :
                              effectiveStatus == "Pending" ||
                              effectiveStatus == "Overdue" ? "Take Activity" :
                                                               "Answer Activity";

            var btn = new buttonRounded
            {
                Text = btnLabel,
                Size = new Size(148, 34),
                BackColor = locked ? Color.FromArgb(160, 160, 160) : Color.Maroon,
                ForeColor = Color.White,
                BorderRadius = 17,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Tag = act,
                Enabled = !locked,
                Cursor = locked ? Cursors.No : Cursors.Hand,
                FlatStyle = FlatStyle.Flat
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Location = new Point(rowW - btn.Width - 12, (rowH - btn.Height) / 2);
            btn.Click += (s, e) =>
            {
                if (s is buttonRounded b && b.Tag is StudentActivityItem a)
                    OnOpenActivity?.Invoke(a);
            };
            row.Controls.Add(btn);

            //  Remarks popup button (Returned only) 
            if (isReturned)
            {
                var btnRemarks = new buttonRounded
                {
                    Text = "View Remarks",
                    Size = new Size(120, 28),
                    BackColor = Color.FromArgb(40, 70, 200),
                    ForeColor = Color.White,
                    BorderRadius = 14,
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    Tag = act,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnRemarks.FlatAppearance.BorderSize = 0;
                btnRemarks.Location = new Point(rowW - btnRemarks.Width - 12, (rowH - btnRemarks.Height) / 2 + 36);
                btnRemarks.Click += (s, e) => ShowRemarksDialog(act);
                row.Controls.Add(btnRemarks);
            }

            // Bottom divider
            row.Paint += (s, e) =>
            {
                using var p = new Pen(Color.FromArgb(235, 235, 235));
                e.Graphics.DrawLine(p, 6, row.Height - 1, row.Width - 1, row.Height - 1);
            };

            return row;
        }

        //  Remarks popup 
        private void ShowRemarksDialog(StudentActivityItem act)
        {
            using var dlg = new Form
            {
                Text = "Instructor Remarks",
                Size = new Size(520, 340),
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.White,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            dlg.Controls.Add(new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = Color.Maroon
            });
            ((Panel)dlg.Controls[0]).Controls.Add(new Label
            {
                Text = act.Title,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0)
            });

            var pnlBody = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16) };

            pnlBody.Controls.Add(new Label
            {
                Text = $"Score:  {act.Score} / {act.Points} pts",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(128, 0, 0),
                Location = new Point(16, 12),
                AutoSize = true
            });
            pnlBody.Controls.Add(new Label
            {
                Text = "Instructor Feedback:",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                Location = new Point(16, 44),
                AutoSize = true
            });
            pnlBody.Controls.Add(new Label
            {
                Text = act.Remarks,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(40, 40, 40),
                Location = new Point(16, 66),
                Size = new Size(470, 120),
                AutoSize = false
            });
            if (act.ReturnedAt.HasValue)
                pnlBody.Controls.Add(new Label
                {
                    Text = $"Returned on  {act.ReturnedAt:dddd, MMMM dd, yyyy  h:mm tt}",
                    Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    Location = new Point(16, 198),
                    AutoSize = true
                });

            var btnClose = new Button
            {
                Text = "Close",
                Size = new Size(100, 32),
                Location = new Point(390, 230),
                BackColor = Color.Maroon,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.OK
            };
            btnClose.FlatAppearance.BorderSize = 0;
            pnlBody.Controls.Add(btnClose);

            dlg.Controls.Add(pnlBody);
            dlg.ShowDialog();
        }

        //  Events 
        private void btnBack_Click(object sender, EventArgs e) => OnBack?.Invoke();

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            _searchTimer.Start();
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e) => RenderRows();
        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e) => RenderRows();
        private void flp_SizeChanged(object sender, EventArgs e) => RenderRows();
    }
}