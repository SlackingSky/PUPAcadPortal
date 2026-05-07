using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    /// <summary>Shows all activities for one course – student view.</summary>
    public partial class StudentActivityList : UserControl
    {
        public event Action OnBack;
        public event Action<StudentActivityItem> OnOpenActivity;

        private StudentCourse course;
        private List<StudentActivityItem> activities;

        public StudentActivityList(StudentCourse course)
        {
            this.course = course;
            InitializeComponent();

            // Populate the header title now that the course is known
            lblTitle.Text = course.Name + "  —  " + course.Code;

            LoadSampleActivities();
            RenderRows();
        }

        // ── Data ──────────────────────────────────────────────────────────────

        private void LoadSampleActivities()
        {
            activities = new List<StudentActivityItem>
            {
                new StudentActivityItem { Id=1, Title="System Simulations", Type="Quiz",
                    Deadline=DateTime.Now.AddDays(3), Points=50, SubmissionStatus="Submitted",
                    Instructions="Answer all questions within the time limit." },
                new StudentActivityItem { Id=2, Title="Integrating Methodologies", Type="Assignment",
                    Deadline=DateTime.Now.AddDays(7), Points=100, SubmissionStatus="Submitted",
                    Instructions="Complete the assignment and submit before the deadline." },
                new StudentActivityItem { Id=3, Title="HCI Essay", Type="Essay",
                    Deadline=DateTime.Now.AddDays(14), Points=75, SubmissionStatus="Not Started",
                    Instructions="Write a comprehensive essay on the topic provided." },
                new StudentActivityItem { Id=4, Title="Programming Lab Activity 1", Type="Assignment",
                    Deadline=DateTime.Now.AddDays(1), Points=50, SubmissionStatus="Submitted",
                    Instructions="Complete the programming activity and submit before the deadline." },
                new StudentActivityItem { Id=5, Title="Midterm Quiz", Type="Quiz",
                    Deadline=DateTime.Now.AddDays(-2), Points=100, SubmissionStatus="Submitted",
                    Instructions="Answer all questions." },
                new StudentActivityItem { Id=6, Title="File Upload Activity", Type="FileUpload",
                    Deadline=DateTime.Now.AddDays(5), Points=25, SubmissionStatus="Submitted",
                    Instructions="Upload your completed file before the deadline." },
            };
        }

        // ── Rendering ─────────────────────────────────────────────────────────

        private void RenderRows()
        {
            if (flp.ClientSize.Width < 100) return;

            flp.Controls.Clear();
            string search = txtSearch?.Text?.ToLower() ?? "";
            string filter = cmbFilter?.SelectedItem?.ToString() ?? "All";

            foreach (var act in activities)
            {
                if (filter != "All" && act.Type != filter) continue;
                if (!string.IsNullOrEmpty(search) && !act.Title.ToLower().Contains(search)) continue;
                flp.Controls.Add(BuildRow(act));
            }
        }

        private Panel BuildRow(StudentActivityItem act)
        {
            int rowW = Math.Max(700, flp.ClientSize.Width - 22);

            var row = new Panel
            {
                Width = rowW,
                Height = 70,
                BackColor = Color.White,
                Margin = new Padding(4, 4, 4, 0),
                BorderStyle = BorderStyle.FixedSingle
            };

            Color typeColor = act.Type switch
            {
                "Quiz" => Color.FromArgb(63, 81, 181),
                "Assignment" => Color.FromArgb(33, 150, 83),
                "Essay" => Color.FromArgb(156, 39, 176),
                "FileUpload" => Color.FromArgb(255, 152, 0),
                _ => Color.Gray
            };

            // Left colour bar
            row.Controls.Add(new Panel
            {
                Width = 6,
                Dock = DockStyle.Left,
                BackColor = typeColor
            });

            // Title
            row.Controls.Add(new Label
            {
                Text = act.Title,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 30),
                Location = new Point(16, 8),
                Size = new Size(320, 22),
                AutoEllipsis = true
            });

            // Type badge
            string typeDisplay = act.Type == "FileUpload" ? "File Upload" : act.Type;
            row.Controls.Add(new Label
            {
                Text = typeDisplay,
                BackColor = typeColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Location = new Point(16, 36),
                Size = new Size(80, 20),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            });

            // Deadline
            TimeSpan ts = act.Deadline - DateTime.Now;
            string dueText = ts.TotalDays < 0 ? "(Overdue)" :
                             ts.Days == 0 ? "(Due Today)" :
                                                $"(Due in {ts.Days}d)";
            Color dueColor = ts.TotalDays < 0 ? Color.Red :
                             ts.Days <= 1 ? Color.OrangeRed :
                                               Color.ForestGreen;
            row.Controls.Add(new Label
            {
                Text = $"{act.Deadline:MMM dd, yyyy}  {dueText}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = dueColor,
                Location = new Point(316, 10),
                Size = new Size(240, 18)
            });

            // Points
            row.Controls.Add(new Label
            {
                Text = $"{act.Points} pts",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.Maroon,
                Location = new Point(316, 36),
                Size = new Size(90, 18)
            });

            // Status badge
            Color statusBg = act.SubmissionStatus switch
            {
                "Submitted" => Color.FromArgb(76, 175, 80),
                "Late" => Color.OrangeRed,
                "Overdue" => Color.Red,
                _ => Color.Gray
            };
            row.Controls.Add(new Label
            {
                Text = act.SubmissionStatus,
                BackColor = statusBg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Location = new Point(570, 24),
                Size = new Size(90, 20),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            });

            // Action button
            string btnText = act.Type switch
            {
                "Quiz" => "Take Quiz",
                "Essay" => act.SubmissionStatus == "Submitted" ? "View" : "Write Essay",
                "FileUpload" => act.SubmissionStatus == "Submitted" ? "View" : "Upload File",
                _ => act.SubmissionStatus == "Submitted" ? "View" : "Submit"
            };
            var btn = new buttonRounded
            {
                Text = btnText,
                Size = new Size(120, 32),
                BackColor = Color.Maroon,
                ForeColor = Color.White,
                BorderRadius = 15,
                Tag = act
            };
            btn.Location = new Point(rowW - btn.Width - 12, 18);
            btn.Click += (s, e) =>
            {
                if (s is buttonRounded b && b.Tag is StudentActivityItem a)
                    OnOpenActivity?.Invoke(a);
            };
            row.Controls.Add(btn);

            return row;
        }

        // ── Event handlers ────────────────────────────────────────────────────

        private void btnBack_Click(object sender, EventArgs e) => OnBack?.Invoke();

        private void txtSearch_TextChanged(object sender, EventArgs e) => RenderRows();

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e) => RenderRows();

        private void flp_SizeChanged(object sender, EventArgs e) => RenderRows();
    }
}