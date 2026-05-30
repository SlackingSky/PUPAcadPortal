using PUPAcadPortal.PortalContents.Instructor.LMS.Course;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class SubmissionList : UserControl
    {
        // ── Events ────────────────────────────────────────────────────────────
        public event Action OnBack;

        // ── State ─────────────────────────────────────────────────────────────
        private readonly ActivityItem _activity;
        private readonly CourseActivity _course;
        private List<StudentSubmission> _submissions = new();

        private string _sortMode = "Name";
        private string _filterStatus = "All";
        private string _searchTerm = "";

        // Search debounce
        private System.Windows.Forms.Timer _searchTimer;

        // Guard: suppress combo events fired during InitializeComponent
        private bool _initializing = true;

        // ── Constructor ───────────────────────────────────────────────────────
        public SubmissionList(ActivityItem activity, CourseActivity course)
        {
            _activity = activity;
            _course = course;

            InitializeComponent();   // combos fire SelectedIndexChanged here,
                                     // but _initializing=true suppresses them

            _initializing = false;   // now safe to react to events

            SetupDebounce();
            PopulateHeader();
            LoadSampleSubmissions();
            RefreshList();

            flpSubmissions.SizeChanged += (s, e) => RefreshList();
            this.Load += (s, e) => RefreshList();
        }

        // ── Header ────────────────────────────────────────────────────────────
        private void PopulateHeader()
        {
            lblActivityTitle.Text = _activity.Title;
            lblActivityType.Text = "📋 " + _activity.TypeString;
            lblMaxPoints.Text = $"🏆 Max: {_activity.Points} pts";
        }

        // ── Debounce ──────────────────────────────────────────────────────────
        private void SetupDebounce()
        {
            _searchTimer = new System.Windows.Forms.Timer { Interval = 200 };
            _searchTimer.Tick += (s, e) =>
            {
                _searchTimer.Stop();
                RefreshList();
            };
        }

        // ── Sample data ───────────────────────────────────────────────────────
        private void LoadSampleSubmissions()
        {
            string[] names =
            {
                "ABLONG, ADRIAN PLATINO",    "BAUTISTA, CARLO SANTOS",
                "CASTRO, DIANA REYES",       "DELA CRUZ, EDUARDO MANUEL",
                "ESPIRITU, FIONA GRACE",     "FLORES, GABRIEL JOSE",
                "GARCIA, HANNAH MAE",        "HERNANDEZ, IAN CARLO",
                "IGNACIO, JESSICA ANNE",     "JIMENEZ, KENNETH RAY",
                "LOPEZ, MARIA CLARA",        "MENDOZA, PAULO REYES",
                "RAMOS, ANGELICA JOY",       "SANTOS, CHRISTIAN PAUL",
                "TORRES, JENNA MAE"
            };

            var rng = new Random(42);
            for (int i = 0; i < names.Length; i++)
            {
                bool submitted = i < 11;
                string status = !submitted ? "Missing" : (i % 3 == 0 ? "Late" : "Submitted");
                int score = (submitted && i < 10)
                    ? rng.Next(Math.Max(0, _activity.Points / 2), _activity.Points + 1)
                    : -1;

                _submissions.Add(new StudentSubmission
                {
                    StudentId = $"2024-{i + 1:D5}-SM-0",
                    StudentName = names[i],
                    Section = "BSIT-" + (i % 2 == 0 ? "1A" : "1B"),
                    GradeLevel = i % 2 == 0 ? "1st Year" : "2nd Year",
                    SubmissionTime = submitted ? DateTime.Now.AddHours(-rng.Next(1, 72)) : DateTime.MinValue,
                    Status = status,
                    Score = score,
                    IsChecked = submitted && i < 10,
                    Remarks = submitted && i < 5 ? "Good work! Keep it up." : "",
                    HasFile = submitted && _activity.Type == ActivityType.FileUpload,
                    EssayContent = submitted
                        ? "Lorem ipsum dolor sit amet, consectetur adipiscing elit. " +
                          "Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                          "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris."
                        : ""
                });
            }
        }

        // ── Refresh list ──────────────────────────────────────────────────────
        private void RefreshList()
        {
            // Safety: don't refresh before data is loaded
            if (_initializing || _submissions == null) return;

            flpSubmissions.SuspendLayout();
            flpSubmissions.Controls.Clear();

            // ── Filter ──
            var filtered = _submissions.FindAll(s =>
            {
                bool statusOk = _filterStatus == "All" ||
                                string.Equals(s.Status, _filterStatus,
                                              StringComparison.OrdinalIgnoreCase);
                bool searchOk = string.IsNullOrEmpty(_searchTerm)
                    || s.StudentName.IndexOf(_searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                    || s.StudentId.IndexOf(_searchTerm, StringComparison.OrdinalIgnoreCase) >= 0;
                return statusOk && searchOk;
            });

            // ── Sort ──
            filtered.Sort((a, b) => _sortMode switch
            {
                "Name" => string.Compare(a.StudentName, b.StudentName, StringComparison.OrdinalIgnoreCase),
                "Time" => a.SubmissionTime == DateTime.MinValue ? 1
                         : b.SubmissionTime == DateTime.MinValue ? -1
                         : a.SubmissionTime.CompareTo(b.SubmissionTime),
                "Score" => b.Score.CompareTo(a.Score),
                _ => 0
            });

            // ── Stats bar ──
            int totalSubmitted = _submissions.Count(s => s.Status != "Missing");
            int totalLate = _submissions.Count(s => s.Status == "Late");
            int totalMissing = _submissions.Count(s => s.Status == "Missing");
            int totalChecked = _submissions.Count(s => s.IsChecked);
            lblStats.Text =
                $"Submitted: {totalSubmitted}  ·  Late: {totalLate}  ·  " +
                $"Missing: {totalMissing}  ·  ✅ Checked: {totalChecked}";

            // ── Rows ──
            foreach (var sub in filtered)
                flpSubmissions.Controls.Add(CreateRow(sub));

            flpSubmissions.ResumeLayout();
        }

        // ── Row builder ───────────────────────────────────────────────────────
        private Panel CreateRow(StudentSubmission sub)
        {
            int rowW = Math.Max(980, flpSubmissions.ClientSize.Width - 30);

            var row = new Panel
            {
                Width = rowW,
                Height = 88,
                BackColor = Color.White,
                Margin = new Padding(5, 4, 5, 0),
                BorderStyle = BorderStyle.None
            };
            row.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(228, 228, 234));
                e.Graphics.DrawRectangle(pen, 0, 0, row.Width - 1, row.Height - 1);
            };

            Color statusColor = sub.Status switch
            {
                "Submitted" => Color.FromArgb(46, 160, 67),
                "Late" => Color.OrangeRed,
                "Missing" => Color.FromArgb(185, 50, 50),
                "Returned" => Color.FromArgb(90, 90, 100),
                _ => Color.Gray
            };

            // Left accent bar
            row.Controls.Add(new Panel
            {
                Width = 5,
                Dock = DockStyle.Left,
                BackColor = statusColor
            });

            // Initials avatar
            row.Controls.Add(new Label
            {
                Text = GetInitials(sub.StudentName),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(128, 0, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(14, 20),
                Size = new Size(46, 46)
            });

            // Name & ID
            row.Controls.Add(new Label
            {
                Text = sub.StudentName,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 35),
                Location = new Point(70, 8),
                Width = 260,
                Height = 22,
                AutoEllipsis = true
            });
            row.Controls.Add(new Label
            {
                Text = sub.StudentId,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.Gray,
                Location = new Point(70, 30),
                Width = 200,
                Height = 18
            });

            // Status badge
            row.Controls.Add(new Label
            {
                Text = sub.IsChecked ? "✅ " + sub.Status : sub.Status,
                BackColor = statusColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                Location = new Point(70, 55),
                Size = new Size(sub.IsChecked ? 100 : 78, 20),
                TextAlign = ContentAlignment.MiddleCenter
            });

            // Submission time
            string timeText = sub.SubmissionTime == DateTime.MinValue
                ? "Not submitted"
                : sub.SubmissionTime.ToString("MMM dd, hh:mm tt");
            row.Controls.Add(new Label
            {
                Text = timeText,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(360, 34),
                Width = 200,
                Height = 18
            });

            // Remarks preview
            if (!string.IsNullOrEmpty(sub.Remarks))
                row.Controls.Add(new Label
                {
                    Text = "💬 " + sub.Remarks,
                    Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                    ForeColor = Color.DimGray,
                    Location = new Point(360, 56),
                    Width = 240,
                    Height = 18,
                    AutoEllipsis = true
                });

            // ── Action buttons (built right-to-left) ──────────────────────────
            int right = rowW - 12;
            const int btnH = 30, gap = 6;

            // Return
            var btnReturn = MakeBtn("Return", Color.DarkOrange, 76, btnH, sub.IsChecked);
            right -= btnReturn.Width;
            btnReturn.Location = new Point(right, 28);
            btnReturn.Click += (s, e) =>
            {
                sub.Status = "Returned";
                sub.IsChecked = false;
                RefreshList();
            };
            row.Controls.Add(btnReturn);
            right -= gap;

            // Check
            var btnCheck = MakeBtn("Check", Color.FromArgb(63, 81, 181), 68, btnH,
                                   sub.Status != "Missing");
            right -= btnCheck.Width;
            btnCheck.Location = new Point(right, 28);
            btnCheck.Click += (s, e) => OpenGrading(sub);
            row.Controls.Add(btnCheck);
            right -= gap;

            // Score textbox
            var txtScore = new TextBox
            {
                Text = sub.Score >= 0 ? sub.Score.ToString() : "",
                Size = new Size(55, 30),
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "Score",
                TextAlign = HorizontalAlignment.Center,
                Enabled = !sub.IsChecked
            };
            right -= txtScore.Width;
            txtScore.Location = new Point(right, 28);
            txtScore.KeyPress += (s, e) =>
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                    e.Handled = true;
            };
            row.Controls.Add(txtScore);

            // "/ pts" label
            right -= 4;
            row.Controls.Add(new Label
            {
                Text = $"/ {_activity.Points}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(right - 48, 34),
                Width = 46,
                Height = 20
            });
            right -= 50 + gap;

            // Save score
            var btnSave = MakeBtn("Save", Color.FromArgb(128, 0, 0), 60, btnH,
                                  !sub.IsChecked);
            right -= btnSave.Width;
            btnSave.Location = new Point(right, 28);
            btnSave.Click += (s, e) =>
            {
                if (!int.TryParse(txtScore.Text, out int sc))
                {
                    MessageBox.Show("Enter a valid numeric score.",
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                sub.Score = Math.Clamp(sc, 0, _activity.Points);
                sub.IsChecked = true;

                txtScore.Text = sub.Score.ToString();
                txtScore.Enabled = false;
                btnSave.Enabled = false;

                MessageBox.Show(
                    $"Score saved: {sub.Score}/{_activity.Points}",
                    "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                RefreshList();
            };
            row.Controls.Add(btnSave);
            right -= gap;

            // Download (FileUpload only)
            if (_activity.Type == ActivityType.FileUpload && sub.HasFile)
            {
                var btnDl = MakeBtn("Download", Color.FromArgb(34, 139, 34), 92, btnH, true);
                right -= btnDl.Width;
                btnDl.Location = new Point(right, 28);
                btnDl.Click += (s, e) =>
                    MessageBox.Show("Download requires storage implementation.",
                        "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                row.Controls.Add(btnDl);
            }

            return row;
        }

        // ── Button factory ────────────────────────────────────────────────────
        private static buttonRounded MakeBtn(
            string text, Color bg, int w, int h, bool enabled)
            => new buttonRounded
            {
                Text = text,
                Size = new Size(w, h),
                BackColor = bg,
                ForeColor = Color.White,
                BorderRadius = 8,
                Enabled = enabled,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

        // ── Open GradingInterface ─────────────────────────────────────────────
        private void OpenGrading(StudentSubmission sub)
        {
            // Resolve parent at click-time, not at construction-time
            Control container = this.Parent;
            if (container == null) return;

            int idx = _submissions.IndexOf(sub);

            var gi = new GradingInterface(sub, _activity, _submissions, idx);
            gi.Dock = DockStyle.Fill;

            gi.OnBack += () =>
            {
                // Re-resolve at close-time in case parent changed
                Control c = gi.Parent ?? container;
                c.Controls.Remove(gi);
                gi.Dispose();
                c.Controls.Add(this);
                this.BringToFront();
                RefreshList();
            };

            container.Controls.Remove(this);
            container.Controls.Add(gi);
            gi.BringToFront();
        }

        // ── Toolbar event handlers ────────────────────────────────────────────

        // Back — resolves navigation at click-time (no stale closure)
        private void btnBack_Click(object sender, EventArgs e)
        {
            // If a subscriber exists (e.g. AssignmentManagement), use it
            if (OnBack != null)
            {
                OnBack.Invoke();
                return;
            }

            // Self-contained fallback: walk up to the real parent and swap back
            Control container = this.Parent;
            if (container == null) return;
            container.Controls.Remove(this);
            // The caller should have wired OnBack; if not, we just remove ourselves
        }

        // Return All
        private void btnReturnAll_Click(object sender, EventArgs e)
        {
            var checked_ = _submissions.FindAll(s => s.IsChecked);
            if (checked_.Count == 0)
            {
                MessageBox.Show("No checked submissions to return.",
                    "Return All", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Return all {checked_.Count} checked submission(s) to students?",
                "Confirm Return All", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                foreach (var s in checked_)
                {
                    s.Status = "Returned";
                    s.IsChecked = false;
                }
                RefreshList();
            }
        }

        // Search (debounced)
        private void txtSearchStudent_TextChanged(object sender, EventArgs e)
        {
            if (_initializing) return;
            _searchTerm = txtSearchStudent.Text;
            _searchTimer.Stop();
            _searchTimer.Start();
        }

        // Sort combo
        private void cmbSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_initializing) return;
            _sortMode = cmbSortBy.SelectedItem?.ToString() ?? "Name";
            RefreshList();
        }

        // Status filter combo
        private void cmbFilterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_initializing) return;
            _filterStatus = cmbFilterStatus.SelectedItem?.ToString() ?? "All";
            RefreshList();
        }

        // ── Helper ────────────────────────────────────────────────────────────
        private static string GetInitials(string name)
        {
            var parts = name.Split(',');
            if (parts.Length >= 2)
            {
                string ln = parts[0].Trim();
                string fn = parts[1].Trim();
                char first = fn.Length > 0 ? fn[0] : '?';
                char last = ln.Length > 0 ? ln[0] : '?';
                return $"{first}{last}";
            }
            return name.Length > 0 ? name[0].ToString() : "?";
        }
    }
}