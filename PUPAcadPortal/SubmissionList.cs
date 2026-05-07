using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class SubmissionList : UserControl
    {
        public event Action OnBack;

        private ActivityItem currentActivity;
        private CourseActivity currentCourse;

        private List<StudentSubmission> submissions =
            new List<StudentSubmission>();

        private string sortMode = "Name";
        private string filterStatus = "All";

        public SubmissionList(ActivityItem activity, CourseActivity course)
        {
            InitializeComponent();

            currentActivity = activity;
            currentCourse = course;

            lblActivityTitle.Text = activity.Title;
            lblActivityType.Text = activity.Type;
            lblMaxPoints.Text = $"Max: {activity.Points} pts";

            // SCROLL SETTINGS
            flpSubmissions.AutoScroll = true;
            flpSubmissions.WrapContents = false;
            flpSubmissions.FlowDirection = FlowDirection.TopDown;

            // SORT OPTIONS
            cmbSortBy.Items.Clear();

            cmbSortBy.Items.AddRange(new object[]
            {
                "Name",
                "Time",
                "Score",
                "Section",
                "Grade Level"
            });

            cmbSortBy.SelectedIndex = 0;

            LoadSampleSubmissions();

            flpSubmissions.SizeChanged += (s, e) => RefreshList();
            this.Load += (s, e) => RefreshList();
        }

        private void LoadSampleSubmissions()
        {
            var names = new[]
            {
                "ABLONG, ADRIAN PLATINO",
                "BAUTISTA, CARLO SANTOS",
                "CASTRO, DIANA REYES",
                "DELA CRUZ, EDUARDO MANUEL",
                "ESPIRITU, FIONA GRACE",
                "FLORES, GABRIEL JOSE",
                "GARCIA, HANNAH MAE",
                "HERNANDEZ, IAN CARLO",
                "IGNACIO, JESSICA ANNE",
                "JIMENEZ, KENNETH RAY",
                "LOPEZ, MARIA CLARA",
                "MENDOZA, PAULO REYES",
                "RAMOS, ANGELICA JOY",
                "SANTOS, CHRISTIAN PAUL",
                "TORRES, JENNA MAE"
            };

            string[] sections =
            {
                "BSIT-1A",
                "BSIT-1B",
                "BSIT-2A",
                "BSIT-2B"
            };

            string[] gradeLevels =
            {
                "1st Year",
                "2nd Year"
            };

            var random = new Random(42);

            int maxScore = currentActivity.Points;

            for (int i = 0; i < names.Length; i++)
            {
                string status =
                    i < 11
                    ? (i % 3 == 0 ? "Late" : "Submitted")
                    : "Missing";

                bool hasSubmission = status != "Missing";

                int scoreMin = Math.Max(0, (int)(maxScore * 0.5));
                int scoreMax = maxScore;

                int score =
                    (hasSubmission && i < 10 && scoreMax > 0)
                    ? random.Next(scoreMin, scoreMax + 1)
                    : -1;

                submissions.Add(new StudentSubmission
                {
                    StudentId = $"2024-{(i + 1):D5}-SM-0",

                    StudentName = names[i],

                    Section = sections[i % sections.Length],

                    GradeLevel = gradeLevels[i % gradeLevels.Length],

                    SubmissionTime = hasSubmission
                        ? DateTime.Now.AddHours(-random.Next(1, 72))
                        : DateTime.MinValue,

                    Status = status,

                    Score = score,

                    IsChecked = hasSubmission && i < 10,

                    Remarks =
                        hasSubmission && i < 5
                        ? "Good work!"
                        : "",

                    HasFile =
                        hasSubmission &&
                        currentActivity.Type == "FileUpload",

                    EssayContent =
                        hasSubmission
                        ? "This is a sample essay submission."
                        : ""
                });
            }
        }

        private void RefreshList()
        {
            flpSubmissions.SuspendLayout();

            flpSubmissions.Controls.Clear();

            var filtered = submissions.FindAll(s =>
                filterStatus == "All" ||
                s.Status == filterStatus);

            filtered.Sort((a, b) =>
            {
                return sortMode switch
                {
                    "Name" =>
                        string.Compare(a.StudentName, b.StudentName),

                    "Time" =>
                        a.SubmissionTime == DateTime.MinValue
                        ? 1
                        : b.SubmissionTime == DateTime.MinValue
                        ? -1
                        : a.SubmissionTime.CompareTo(b.SubmissionTime),

                    "Score" =>
                        b.Score.CompareTo(a.Score),

                    "Section" =>
                        string.Compare(a.Section, b.Section),

                    "Grade Level" =>
                        string.Compare(a.GradeLevel, b.GradeLevel),

                    _ => 0
                };
            });

            int submitted =
                submissions.FindAll(s => s.Status != "Missing").Count;

            int missing =
                submissions.FindAll(s => s.Status == "Missing").Count;

            int late =
                submissions.FindAll(s => s.Status == "Late").Count;

            int checked_ =
                submissions.FindAll(s => s.IsChecked).Count;

            lblStats.Text =
                $"Submitted: {submitted} | " +
                $"Late: {late} | " +
                $"Missing: {missing} | " +
                $"Checked: {checked_}";

            foreach (var sub in filtered)
            {
                flpSubmissions.Controls.Add(CreateSubmissionRow(sub));
            }

            flpSubmissions.ResumeLayout();
        }

        private Panel CreateSubmissionRow(StudentSubmission sub)
        {
            int rowWidth =
                Math.Max(980, flpSubmissions.ClientSize.Width - 30);

            var row = new Panel
            {
                Width = rowWidth,
                Height = 85,
                BackColor = Color.White,
                Margin = new Padding(5, 5, 5, 0),
                BorderStyle = BorderStyle.FixedSingle
            };

            Color statusColor = sub.Status switch
            {
                "Submitted" => Color.ForestGreen,
                "Late" => Color.OrangeRed,
                "Missing" => Color.Red,
                "Returned" => Color.Gray,
                _ => Color.Gray
            };

            var pnlStatusBar = new Panel
            {
                Width = 5,
                Dock = DockStyle.Left,
                BackColor = statusColor
            };

            var lblInitials = new Label
            {
                Text = GetInitials(sub.StudentName),
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(14, 18),
                Size = new Size(46, 46),
                BackColor = Color.Maroon
            };

            var lblName = new Label
            {
                Text = sub.StudentName,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(70, 8),
                Width = 250,
                Height = 22,
                AutoEllipsis = true
            };

            var lblId = new Label
            {
                Text = sub.StudentId,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(70, 30),
                Width = 200,
                Height = 18
            };

            var lblStatus = new Label
            {
                Text = sub.Status,
                BackColor = statusColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Location = new Point(70, 55),
                Size = new Size(80, 18),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblSection = new Label
            {
                Text = sub.Section,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Location = new Point(165, 55),
                Width = 80,
                Height = 18
            };

            var lblGrade = new Label
            {
                Text = sub.GradeLevel,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.DarkGreen,
                Location = new Point(255, 55),
                Width = 90,
                Height = 18
            };

            string timeText =
                sub.SubmissionTime == DateTime.MinValue
                ? "Not submitted"
                : sub.SubmissionTime.ToString("MMM dd, hh:mm tt");

            var lblTime = new Label
            {
                Text = timeText,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(360, 32),
                Width = 180,
                Height = 18
            };

            // RIGHT SIDE CONTROLS

            int spacing = 8;
            int right = rowWidth - 15;

            // RETURN BUTTON

            var btnReturn = new buttonRounded
            {
                Text = "Return",
                Size = new Size(75, 30),
                BackColor = Color.DarkOrange,
                ForeColor = Color.White,
                BorderRadius = 8,
                Tag = sub,
                Enabled = sub.IsChecked
            };

            right -= btnReturn.Width;

            btnReturn.Location = new Point(right, 25);

            btnReturn.Click += (s, e) =>
            {
                if (s is buttonRounded b &&
                    b.Tag is StudentSubmission st)
                {
                    st.Status = "Returned";
                    st.IsChecked = false;

                    MessageBox.Show(
                        $"Submission returned to {st.StudentName}.",
                        "Returned",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    RefreshList();
                }
            };

            // CHECK BUTTON

            right -= spacing;

            var btnCheck = new buttonRounded
            {
                Text = "Check",
                Size = new Size(70, 30),
                BackColor = Color.FromArgb(63, 81, 181),
                ForeColor = Color.White,
                BorderRadius = 8,
                Tag = sub,
                Enabled = sub.Status != "Missing"
            };

            right -= btnCheck.Width;

            btnCheck.Location = new Point(right, 25);

            btnCheck.Click += BtnCheckSubmission_Click;

            // SAVE BUTTON

            right -= spacing;

            var btnSaveScore = new buttonRounded
            {
                Text = "Save",
                Size = new Size(65, 30),
                BackColor = Color.Maroon,
                ForeColor = Color.White,
                BorderRadius = 8
            };

            right -= btnSaveScore.Width;

            btnSaveScore.Location = new Point(right, 25);

            // SCORE LABEL

            right -= spacing;

            var lblScoreOf = new Label
            {
                Text = $"/ {currentActivity.Points}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Size = new Size(45, 30),
                TextAlign = ContentAlignment.MiddleLeft
            };

            right -= lblScoreOf.Width;

            lblScoreOf.Location = new Point(right, 25);

            // SCORE BOX

            right -= spacing;

            var txtScore = new TextBox
            {
                Text =
                    sub.Score >= 0
                    ? sub.Score.ToString()
                    : "",

                Size = new Size(55, 30),

                Font = new Font("Segoe UI", 10F),

                PlaceholderText = "Score",

                TextAlign = HorizontalAlignment.Center,

                Tag = sub
            };

            right -= txtScore.Width;

            txtScore.Location = new Point(right, 25);

            txtScore.KeyPress += (s, e) =>
            {
                if (!char.IsDigit(e.KeyChar) &&
                    !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            // DOWNLOAD BUTTON

            if (currentActivity.Type == "FileUpload" &&
                sub.HasFile)
            {
                right -= spacing;

                var btnDownload = new buttonRounded
                {
                    Text = "Download",
                    Size = new Size(95, 30),
                    BackColor = Color.ForestGreen,
                    ForeColor = Color.White,
                    BorderRadius = 8,
                    Tag = sub
                };

                right -= btnDownload.Width;

                btnDownload.Location = new Point(right, 25);

                btnDownload.Click += (s, e) =>
                {
                    MessageBox.Show(
                        "Download feature requires storage implementation.",
                        "Info",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                };

                row.Controls.Add(btnDownload);
            }

            // SAVE SCORE

            btnSaveScore.Tag = new object[]
            {
                sub,
                txtScore
            };

            btnSaveScore.Click += (s, e) =>
            {
                var data =
                    (object[])((buttonRounded)s).Tag;

                var student =
                    (StudentSubmission)data[0];

                var scoreBox =
                    (TextBox)data[1];

                if (int.TryParse(scoreBox.Text, out int sc))
                {
                    student.Score =
                        Math.Min(sc, currentActivity.Points);

                    student.IsChecked = true;

                    scoreBox.Text =
                        student.Score.ToString();

                    MessageBox.Show(
                        $"Score saved: {student.Score}/{currentActivity.Points}",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    RefreshList();
                }
                else
                {
                    MessageBox.Show(
                        "Please enter valid numeric score.",
                        "Validation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            };

            row.Controls.AddRange(new Control[]
            {
                pnlStatusBar,
                lblInitials,
                lblName,
                lblId,
                lblStatus,
                lblSection,
                lblGrade,
                lblTime,
                txtScore,
                lblScoreOf,
                btnSaveScore,
                btnCheck,
                btnReturn
            });

            return row;
        }

        private void BtnCheckSubmission_Click(
            object sender,
            EventArgs e)
        {
            if (!(sender is buttonRounded btn &&
                btn.Tag is StudentSubmission sub))
            {
                return;
            }

            var parentContainer = this.Parent;

            if (parentContainer == null)
            {
                return;
            }

            int idx = submissions.IndexOf(sub);

            var gradingInterface =
                new GradingInterface(
                    sub,
                    currentActivity,
                    submissions,
                    idx);

            gradingInterface.Dock = DockStyle.Fill;

            gradingInterface.OnBack += () =>
            {
                parentContainer.Controls.Remove(gradingInterface);

                parentContainer.Controls.Add(this);

                this.BringToFront();

                RefreshList();
            };

            parentContainer.Controls.Remove(this);

            parentContainer.Controls.Add(gradingInterface);

            gradingInterface.BringToFront();
        }

        private string GetInitials(string name)
        {
            var parts = name.Split(',');

            if (parts.Length >= 2)
            {
                string lastName = parts[0].Trim();
                string firstName = parts[1].Trim();

                return
                    $"{(firstName.Length > 0 ? firstName[0] : ' ')}" +
                    $"{(lastName.Length > 0 ? lastName[0] : ' ')}";
            }

            return
                name.Length > 0
                ? name[0].ToString()
                : "?";
        }

        private void cmbSortBy_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            sortMode =
                cmbSortBy.SelectedItem?.ToString() ?? "Name";

            RefreshList();
        }

        private void cmbFilterStatus_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            filterStatus =
                cmbFilterStatus.SelectedItem?.ToString() ?? "All";

            RefreshList();
        }

        private void txtSearchStudent_TextChanged(
            object sender,
            EventArgs e)
        {
            string search =
                txtSearchStudent.Text.ToLower();

            foreach (Control ctrl in flpSubmissions.Controls)
            {
                if (ctrl is Panel row)
                {
                    string nameText = "";

                    foreach (Control c in row.Controls)
                    {
                        if (c is Label lbl &&
                            lbl.Font.Bold)
                        {
                            nameText =
                                lbl.Text.ToLower();

                            break;
                        }
                    }

                    row.Visible =
                        string.IsNullOrEmpty(search) ||
                        nameText.Contains(search);
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            OnBack?.Invoke();
        }
    }

    public class StudentSubmission
    {
        public string StudentId { get; set; } = "";

        public string StudentName { get; set; } = "";

        public string Section { get; set; } = "";

        public string GradeLevel { get; set; } = "";

        public DateTime SubmissionTime { get; set; }

        public string Status { get; set; } = "Missing";

        public int Score { get; set; } = -1;

        public bool IsChecked { get; set; } = false;

        public string Remarks { get; set; } = "";

        public bool HasFile { get; set; } = false;

        public string EssayContent { get; set; } =
            "Sample essay content.";
    }
}