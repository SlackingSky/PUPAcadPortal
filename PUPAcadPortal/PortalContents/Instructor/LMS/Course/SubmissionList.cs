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
        public event Action OnBack;

        private readonly ActivityItem _activity;
        private readonly CourseActivity _course;
        private List<StudentSubmission> _submissions = new();

        private string _sortMode = "Name";
        private string _filterStatus = "All";
        private string _searchTerm = "";
        private System.Windows.Forms.Timer _searchTimer;

        public SubmissionList(ActivityItem activity, CourseActivity course)
        {
            _activity = activity;
            _course = course;
            InitializeComponent();
            SetupDebounce();
            PopulateHeader();
            LoadSampleSubmissions();
            RefreshList();

            cmbSortBy.SelectedIndex = 0;
            cmbFilterStatus.SelectedIndex = 0;
            flpSubmissions.SizeChanged += (s, e) => RefreshList();
            this.Load += (s, e) => RefreshList();
        }

        private void pnlHeader_SizeChanged(object sender, EventArgs e)
        {
            if (btnReturnAll != null && pnlHeader != null)
            {
                btnReturnAll.Location = new System.Drawing.Point(pnlHeader.Width - btnReturnAll.Width - 12, 18);
            }
        }

        //  Header 
        private void PopulateHeader()
        {
            lblActivityTitle.Text = _activity.Title;
            lblActivityType.Text = "📋 " + _activity.TypeString;
            lblMaxPoints.Text = $"🏆 Max: {_activity.Points} pts";
        }

        //  Debounce 
        private void SetupDebounce()
        {
            _searchTimer = new System.Windows.Forms.Timer { Interval = 180 };
            _searchTimer.Tick += (s, e) => { _searchTimer.Stop(); RefreshList(); };
        }

        //  Sample data 
        private void LoadSampleSubmissions()
        {
            string[] names =
            {
                "ABLONG, ADRIAN PLATINO", "BAUTISTA, CARLO SANTOS", "CASTRO, DIANA REYES",
                "DELA CRUZ, EDUARDO MANUEL", "ESPIRITU, FIONA GRACE", "FLORES, GABRIEL JOSE",
                "GARCIA, HANNAH MAE", "HERNANDEZ, IAN CARLO", "IGNACIO, JESSICA ANNE",
                "JIMENEZ, KENNETH RAY", "LOPEZ, MARIA CLARA", "MENDOZA, PAULO REYES",
                "RAMOS, ANGELICA JOY", "SANTOS, CHRISTIAN PAUL", "TORRES, JENNA MAE"
            };

            var rng = new Random(42);
            for (int i = 0; i < names.Length; i++)
            {
                bool submitted = i < 11;
                string status = !submitted ? "Missing" : (i % 3 == 0 ? "Late" : "Submitted");
                int score = (submitted && i < 10) ? rng.Next(Math.Max(0, _activity.Points / 2), _activity.Points + 1) : -1;

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
                        ? "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur."
                        : ""
                });
            }
        }

        //  Refresh list 
        private void RefreshList()
        {
            flpSubmissions.SuspendLayout();
            flpSubmissions.Controls.Clear();

            var filtered = _submissions.FindAll(s =>
            {
                bool statusOk = _filterStatus == "All" || s.Status == _filterStatus;
                bool searchOk = string.IsNullOrEmpty(_searchTerm)
                    || s.StudentName.IndexOf(_searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                    || s.StudentId.IndexOf(_searchTerm, StringComparison.OrdinalIgnoreCase) >= 0;
                return statusOk && searchOk;
            });

            filtered.Sort((a, b) => _sortMode switch
            {
                "Name" => string.Compare(a.StudentName, b.StudentName, StringComparison.Ordinal),
                "Time" => a.SubmissionTime == DateTime.MinValue ? 1 : b.SubmissionTime == DateTime.MinValue ? -1 : a.SubmissionTime.CompareTo(b.SubmissionTime),
                "Score" => b.Score.CompareTo(a.Score),
                _ => 0
            });

            // Stats bar
            int submitted = _submissions.Count(s => s.Status != "Missing");
            int late = _submissions.Count(s => s.Status == "Late");
            int missing = _submissions.Count(s => s.Status == "Missing");
            int chk = _submissions.Count(s => s.IsChecked);
            lblStats.Text = $"Submitted: {submitted}  ·  Late: {late}  ·  Missing: {missing}  ·  ✅ Checked: {chk}";

            foreach (var sub in filtered)
                flpSubmissions.Controls.Add(CreateRow(sub));

            flpSubmissions.ResumeLayout();
        }

        //  Row builder 
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
                using var pen = new Pen(Color.FromArgb(230, 230, 236));
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
            row.Controls.Add(new Panel { Width = 5, Dock = DockStyle.Left, BackColor = statusColor });

            // Initials circle
            var initials = GetInitials(sub.StudentName);
            var lblInit = new Label
            {
                Text = initials,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(128, 0, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(14, 20),
                Size = new Size(46, 46)
            };
            row.Controls.Add(lblInit);

            // Name
            row.Controls.Add(new Label { Text = sub.StudentName, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 30, 35), Location = new Point(70, 8), Width = 260, Height = 22, AutoEllipsis = true });
            row.Controls.Add(new Label { Text = sub.StudentId, Font = new Font("Segoe UI", 8.5F), ForeColor = Color.Gray, Location = new Point(70, 30), Width = 200, Height = 18 });

            // Status badge
            row.Controls.Add(new Label
            {
                Text = sub.IsChecked ? "✅ " + sub.Status : sub.Status,
                BackColor = statusColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                Location = new Point(70, 55),
                Size = new Size(sub.IsChecked ? 100 : 80, 20),
                TextAlign = ContentAlignment.MiddleCenter
            });

            // Time
            string timeText = sub.SubmissionTime == DateTime.MinValue ? "Not submitted" : sub.SubmissionTime.ToString("MMM dd, hh:mm tt");
            row.Controls.Add(new Label { Text = timeText, Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray, Location = new Point(360, 34), Width = 200, Height = 18 });

            // Remarks preview
            if (!string.IsNullOrEmpty(sub.Remarks))
                row.Controls.Add(new Label { Text = "💬 " + sub.Remarks, Font = new Font("Segoe UI", 8F, FontStyle.Italic), ForeColor = Color.DimGray, Location = new Point(360, 56), Width = 240, Height = 18, AutoEllipsis = true });

            int right = rowW - 12;
            const int btnH = 30, gap = 6;

            // Return button
            var btnReturn = BuildBtn("Return", Color.DarkOrange, 75, btnH, sub.IsChecked);
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

            // Check button
            var btnCheck = BuildBtn("Check", Color.FromArgb(63, 81, 181), 68, btnH, sub.Status != "Missing");
            right -= btnCheck.Width;
            btnCheck.Location = new Point(right, 28);
            btnCheck.Click += (s, e) => OpenGrading(sub);
            row.Controls.Add(btnCheck);

            right -= gap;

            // Score save
            var txtScore = new TextBox
            {
                Text = sub.Score >= 0 ? sub.Score.ToString() : "",
                Size = new Size(55, 30),
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "Score",
                TextAlign = HorizontalAlignment.Center,
                Enabled = !sub.IsChecked  // Lock after checking
            };
            right -= txtScore.Width;
            txtScore.Location = new Point(right, 28);
            txtScore.KeyPress += (s, e) => { if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true; };
            row.Controls.Add(txtScore);

            right -= 4;
            row.Controls.Add(new Label { Text = $"/ {_activity.Points}", Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray, Location = new Point(right - 48, 34), Width = 46, Height = 20 });
            right -= 50 + gap;

            var btnSaveScore = BuildBtn("Save", Color.FromArgb(128, 0, 0), 60, btnH, !sub.IsChecked);
            right -= btnSaveScore.Width;
            btnSaveScore.Location = new Point(right, 28);
            btnSaveScore.Click += (s, e) =>
            {
                if (!int.TryParse(txtScore.Text, out int sc))
                { MessageBox.Show("Enter a valid numeric score.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                sub.Score = Math.Clamp(sc, 0, _activity.Points);
                sub.IsChecked = true;
                txtScore.Text = sub.Score.ToString();
                txtScore.Enabled = false;
                btnSaveScore.Enabled = false;
                MessageBox.Show($"Score saved: {sub.Score}/{_activity.Points}", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshList();
            };
            row.Controls.Add(btnSaveScore);

            // Download button for FileUpload
            if (_activity.Type == ActivityType.FileUpload && sub.HasFile)
            {
                right -= gap;
                var btnDl = BuildBtn("Download", Color.FromArgb(34, 139, 34), 90, btnH, true);
                right -= btnDl.Width;
                btnDl.Location = new Point(right, 28);
                btnDl.Click += (s, e) => MessageBox.Show("Download requires storage implementation.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                row.Controls.Add(btnDl);
            }

            return row;
        }

        private static buttonRounded BuildBtn(string text, Color bg, int w, int h, bool enabled)
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

        //  Open grading 
        private void OpenGrading(StudentSubmission sub)
        {
            var parent = this.Parent; if (parent == null) return;
            int idx = _submissions.IndexOf(sub);

            var gi = new GradingInterface(sub, _activity, _submissions, idx);
            gi.Dock = DockStyle.Fill;
            gi.OnBack += () =>
            {
                parent.Controls.Remove(gi);
                parent.Controls.Add(this);
                this.BringToFront();
                RefreshList();
            };

            parent.Controls.Remove(this);
            parent.Controls.Add(gi);
            gi.BringToFront();
        }

        //  Toolbar events 
        private void btnBack_Click(object sender, EventArgs e) => OnBack?.Invoke();

        private void txtSearchStudent_TextChanged(object sender, EventArgs e)
        {
            _searchTerm = txtSearchStudent.Text;
            _searchTimer.Stop();
            _searchTimer.Start();
        }

        private void cmbSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            _sortMode = cmbSortBy.SelectedItem?.ToString() ?? "Name";
            RefreshList();
        }

        private void cmbFilterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            _filterStatus = cmbFilterStatus.SelectedItem?.ToString() ?? "All";
            RefreshList();
        }

        private void btnReturnAll_Click(object sender, EventArgs e)
        {
            var chkd = _submissions.FindAll(s => s.IsChecked);
            if (chkd.Count == 0) { MessageBox.Show("No checked submissions to return.", "Return All", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            if (MessageBox.Show($"Return all {chkd.Count} checked submission(s) to students?", "Confirm Return All", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (var s in chkd) { s.Status = "Returned"; s.IsChecked = false; }
                RefreshList();
            }
        }

        //  Helper 
        private static string GetInitials(string name)
        {
            var parts = name.Split(',');
            if (parts.Length >= 2)
            {
                string ln = parts[0].Trim(), fn = parts[1].Trim();
                return $"{(fn.Length > 0 ? fn[0] : ' ')}{(ln.Length > 0 ? ln[0] : ' ')}";
            }
            return name.Length > 0 ? name[0].ToString() : "?";
        }
    }
}