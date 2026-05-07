using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class AssignmentManagement : UserControl
    {
        public event Action OnBack;
        private CourseActivity currentCourse;
        private List<ActivityItem> activities = new List<ActivityItem>();
        private string filterType = "All";
        private int currentPage = 1;
        private const int ItemsPerPage = 5;

        public AssignmentManagement(CourseActivity course)
        {
            InitializeComponent();
            currentCourse = course;
            lblCourseTitle.Text = course.CourseName + " — " + course.CourseCode;
            LoadSampleActivities();

            // Refresh list once the FlowLayoutPanel has actually been laid out
            flpActivities.SizeChanged += (s, e) => RefreshActivityList();
            this.Load += (s, e) => RefreshActivityList();
        }

        private void LoadSampleActivities()
        {
            activities = new List<ActivityItem>
            {
                new ActivityItem { Id=1, Title="System Simulations", Type="Quiz",
                    Deadline=DateTime.Now.AddDays(3), Points=50, Status="Published",
                    TotalStudents=40, Submitted=35, Checked=30, IsDraft=false },
                new ActivityItem { Id=2, Title="Integrating Methodologies", Type="Assignment",
                    Deadline=DateTime.Now.AddDays(7), Points=100, Status="Published",
                    TotalStudents=40, Submitted=20, Checked=15, IsDraft=false },
                new ActivityItem { Id=3, Title="Human Computer Interactions Essay", Type="Essay",
                    Deadline=DateTime.Now.AddDays(14), Points=75, Status="Draft",
                    TotalStudents=40, Submitted=0, Checked=0, IsDraft=true },
                new ActivityItem { Id=4, Title="Programming Lab Activity 1", Type="Assignment",
                    Deadline=DateTime.Now.AddDays(1), Points=50, Status="Published",
                    TotalStudents=40, Submitted=38, Checked=38, IsDraft=false },
                new ActivityItem { Id=5, Title="Midterm Quiz", Type="Quiz",
                    Deadline=DateTime.Now.AddDays(-2), Points=100, Status="Published",
                    TotalStudents=40, Submitted=40, Checked=40, IsDraft=false },
                new ActivityItem { Id=6, Title="File Upload Activity", Type="FileUpload",
                    Deadline=DateTime.Now.AddDays(5), Points=25, Status="Published",
                    TotalStudents=40, Submitted=10, Checked=5, IsDraft=false },
            };
        }

        private void RefreshActivityList()
        {
            // Guard: don't render until the panel has a real width
            if (flpActivities.ClientSize.Width < 100) return;

            flpActivities.Controls.Clear();

            string search = txtSearchActivities.Text.ToLower();
            var filtered = activities.FindAll(a =>
            {
                bool matchType = filterType == "All" || a.Type == filterType;
                bool matchSearch = string.IsNullOrEmpty(search) ||
                    a.Title.ToLower().Contains(search);
                return matchType && matchSearch;
            });

            int totalPages = (int)Math.Ceiling(filtered.Count / (double)ItemsPerPage);
            if (currentPage > totalPages && totalPages > 0) currentPage = totalPages;

            int start = (currentPage - 1) * ItemsPerPage;
            var pageItems = filtered.GetRange(start, Math.Min(ItemsPerPage, filtered.Count - start));

            // Auto-lock overdue published activities
            foreach (var a in activities)
            {
                if (!a.IsDraft && a.Deadline < DateTime.Now && !a.IsLocked)
                    a.IsLocked = true;
            }

            foreach (var activity in pageItems)
            {
                var row = CreateActivityRow(activity);
                flpActivities.Controls.Add(row);
            }

            lblPageInfo.Text = $"Page {currentPage} of {Math.Max(1, totalPages)}  ({filtered.Count} items)";
            btnPrevPage.Enabled = currentPage > 1;
            btnNextPage.Enabled = currentPage < totalPages;
        }

        private Panel CreateActivityRow(ActivityItem activity)
        {
            // Use the actual client width so rows always fit
            int rowWidth = Math.Max(900, flpActivities.ClientSize.Width - 22);

            var row = new Panel
            {
                Width = rowWidth,
                Height = 70,
                BackColor = activity.IsDraft ? Color.FromArgb(255, 250, 230) : Color.White,
                Margin = new Padding(5, 5, 5, 0),
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Default
            };

            Color typeColor = activity.Type switch
            {
                "Quiz" => Color.FromArgb(63, 81, 181),
                "Assignment" => Color.FromArgb(33, 150, 83),
                "Essay" => Color.FromArgb(156, 39, 176),
                "FileUpload" => Color.FromArgb(255, 152, 0),
                _ => Color.Gray
            };

            // Left colour bar
            var pnlTypeBar = new Panel
            {
                Width = 5,
                Dock = DockStyle.Left,
                BackColor = typeColor
            };

            // Title
            var lblTitle = new Label
            {
                Text = activity.Title,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 40, 40),
                Location = new Point(15, 8),
                Size = new Size(280, 22),
                AutoEllipsis = true
            };

            // Type badge
            string typeDisplay = activity.Type == "FileUpload" ? "File Upload" : activity.Type;
            var lblType = new Label
            {
                Text = typeDisplay,
                BackColor = typeColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Location = new Point(15, 34),
                Size = new Size(80, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Deadline
            TimeSpan deadline = activity.Deadline - DateTime.Now;
            string dueText = deadline.TotalDays < 0 ? "Overdue" :
                             deadline.Days == 0 ? "Due Today" :
                             $"Due in {deadline.Days}d";
            Color dueColor = deadline.TotalDays < 0 ? Color.Red :
                             deadline.Days <= 2 ? Color.OrangeRed : Color.ForestGreen;

            var lblDeadline = new Label
            {
                Text = $"{activity.Deadline:MMM dd, yyyy}  ({dueText})",
                Font = new Font("Segoe UI", 9F),
                ForeColor = dueColor,
                Location = new Point(310, 10),
                Size = new Size(200, 18)
            };

            // Points
            var lblPoints = new Label
            {
                Text = $"{activity.Points} pts",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.Maroon,
                Location = new Point(310, 32),
                Size = new Size(80, 18)
            };

            // Submission stats
            var lblSubmission = new Label
            {
                Text = $"Submitted: {activity.Submitted}/{activity.TotalStudents}   Checked: {activity.Checked}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(520, 26),
                Size = new Size(260, 18)
            };

            // Status badge
            var lblStatusBadge = new Label
            {
                Text = activity.Status,
                BackColor = activity.IsDraft ? Color.FromArgb(255, 200, 50) : Color.FromArgb(76, 175, 80),
                ForeColor = activity.IsDraft ? Color.Black : Color.White,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Location = new Point(520, 8),
                Size = new Size(70, 18),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // ── Action buttons — anchored from the right ──────────────────────
            // Layout (right-to-left): [Copy] [Publish/Unpublish] [Delete] [Edit] [Students]
            // Starting x = rowWidth - 10 - (button widths + gaps)
            // Total button area: 80+90+65+60+80+10gaps = ~400 px from right edge
            int rx = rowWidth - 405; // left edge of the button group

            var btnViewStudents = new buttonRounded
            {
                Text = "Students",
                Location = new Point(rx, 20),
                Size = new Size(80, 28),
                BackColor = Color.FromArgb(33, 150, 83),
                ForeColor = Color.White,
                BorderRadius = 10,
                Tag = activity
            };
            btnViewStudents.Click += BtnViewStudents_Click;

            var btnEdit = new buttonRounded
            {
                Text = "Edit",
                Location = new Point(rx + 88, 20),
                Size = new Size(60, 28),
                BackColor = Color.FromArgb(33, 100, 180),
                ForeColor = Color.White,
                BorderRadius = 10,
                Tag = activity
            };
            btnEdit.Click += BtnEditActivity_Click;

            var btnDelete = new buttonRounded
            {
                Text = "Delete",
                Location = new Point(rx + 156, 20),
                Size = new Size(65, 28),
                BackColor = Color.FromArgb(180, 30, 30),
                ForeColor = Color.White,
                BorderRadius = 10,
                Tag = activity
            };
            btnDelete.Click += BtnDeleteActivity_Click;

            var btnPublish = new buttonRounded
            {
                Text = activity.IsDraft ? "Publish" : "Unpublish",
                Location = new Point(rx + 229, 20),
                Size = new Size(85, 28),
                BackColor = activity.IsDraft ? Color.DarkGoldenrod : Color.SlateGray,
                ForeColor = Color.White,
                BorderRadius = 10,
                Tag = activity
            };
            btnPublish.Click += BtnPublishToggle_Click;

            var btnDuplicate = new buttonRounded
            {
                Text = "Copy",
                Location = new Point(rx + 322, 20),
                Size = new Size(60, 28),
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                BorderRadius = 10,
                Tag = activity
            };
            btnDuplicate.Click += (s, e) =>
            {
                if (s is buttonRounded b && b.Tag is ActivityItem src)
                {
                    var copy = new ActivityItem
                    {
                        Id = activities.Count + 1,
                        Title = src.Title + " (Copy)",
                        Type = src.Type,
                        Deadline = src.Deadline,
                        Points = src.Points,
                        Status = "Draft",
                        IsDraft = true,
                        TotalStudents = src.TotalStudents,
                        Submitted = 0,
                        Checked = 0
                    };
                    activities.Insert(0, copy);
                    RefreshActivityList();
                }
            };

            row.Controls.AddRange(new Control[] {
                pnlTypeBar, lblTitle, lblType, lblDeadline, lblPoints,
                lblSubmission, lblStatusBadge,
                btnViewStudents, btnEdit, btnDelete, btnPublish, btnDuplicate
            });

            return row;
        }

        // ── Button handlers ───────────────────────────────────────────────────

        private void BtnViewStudents_Click(object sender, EventArgs e)
        {
            if (sender is buttonRounded btn && btn.Tag is ActivityItem activity)
            {
                var submissionList = new SubmissionList(activity, currentCourse);
                submissionList.Dock = DockStyle.Fill;

                submissionList.OnBack += () =>
                {
                    var container = this.Parent;
                    if (container != null)
                    {
                        container.Controls.Remove(submissionList);
                        container.Controls.Add(this);
                        this.BringToFront();
                    }
                };

                var parentContainer = this.Parent;
                if (parentContainer != null)
                {
                    parentContainer.Controls.Remove(this);
                    parentContainer.Controls.Add(submissionList);
                    submissionList.BringToFront();
                }
            }
        }

        private void BtnEditActivity_Click(object sender, EventArgs e)
        {
            if (sender is buttonRounded btn && btn.Tag is ActivityItem activity)
            {
                var dlg = new CreateEditActivityDialog(activity);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    activity.Title = dlg.ActivityTitle;
                    activity.Deadline = dlg.ActivityDeadline;
                    activity.Points = dlg.ActivityPoints;
                    RefreshActivityList();
                }
            }
        }

        private void BtnDeleteActivity_Click(object sender, EventArgs e)
        {
            if (sender is buttonRounded btn && btn.Tag is ActivityItem activity)
            {
                var result = MessageBox.Show(
                    $"Delete \"{activity.Title}\"?", "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    activities.Remove(activity);
                    RefreshActivityList();
                }
            }
        }

        private void BtnPublishToggle_Click(object sender, EventArgs e)
        {
            if (sender is buttonRounded btn && btn.Tag is ActivityItem activity)
            {
                activity.IsDraft = !activity.IsDraft;
                activity.Status = activity.IsDraft ? "Draft" : "Published";
                RefreshActivityList();
            }
        }

        private void btnCreateAssignment_Click(object sender, EventArgs e)
        {
            var dlg = new CreateEditActivityDialog(null);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var newActivity = new ActivityItem
                {
                    Id = activities.Count + 1,
                    Title = dlg.ActivityTitle,
                    Type = dlg.ActivityType,
                    Deadline = dlg.ActivityDeadline,
                    Points = dlg.ActivityPoints,
                    Status = "Draft",
                    IsDraft = true,
                    TotalStudents = 40,
                    Submitted = 0,
                    Checked = 0
                };
                activities.Insert(0, newActivity);
                RefreshActivityList();
            }
        }

        private void btnBack_Click(object sender, EventArgs e) => OnBack?.Invoke();

        private void txtSearchActivities_TextChanged(object sender, EventArgs e)
        {
            currentPage = 1;
            RefreshActivityList();
        }

        private void cmbFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterType = cmbFilterType.SelectedItem?.ToString() ?? "All";
            currentPage = 1;
            RefreshActivityList();
        }

        private void btnNextPage_Click(object sender, EventArgs e) { currentPage++; RefreshActivityList(); }
        private void btnPrevPage_Click(object sender, EventArgs e) { currentPage--; RefreshActivityList(); }
    }

    public class ActivityItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Type { get; set; } = "Assignment";
        public DateTime Deadline { get; set; }
        public int Points { get; set; }
        public string Status { get; set; } = "Draft";
        public bool IsDraft { get; set; } = true;
        public int TotalStudents { get; set; }
        public int Submitted { get; set; }
        public int Checked { get; set; }
        public bool IsLocked { get; set; } = false;
    }
}