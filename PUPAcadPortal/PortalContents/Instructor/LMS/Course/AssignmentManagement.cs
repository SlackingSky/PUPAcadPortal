using PUPAcadPortal.PortalContents.Instructor.LMS.Course;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class AssignmentManagement : UserControl
    {
        public event Action OnBack;
        private readonly CourseActivity _course;
        private string _searchTerm = "";
        private string _filterType = "All";
        private System.Windows.Forms.Timer _searchTimer;
        private bool _initializing = true;

        public AssignmentManagement(CourseActivity course)
        {
            _course = course;
            InitializeComponent();

            _initializing = false;

            SetupDebounce();
            PopulateHeader();
            RefreshList();

            flpActivities.SizeChanged += (s, e) => RefreshList();
            this.Load += (s, e) => RefreshList();
        }

        private void PopulateHeader()
        {
            lblCourseName.Text = _course.CourseName;
            lblCourseCode.Text = _course.CourseCode + "  ·  " + _course.InstructorName;
            btnSave.Text = "+ Create Activity";
        }

        private void SetupDebounce()
        {
            _searchTimer = new System.Windows.Forms.Timer { Interval = 200 };
            _searchTimer.Tick += (s, e) => { _searchTimer.Stop(); RefreshList(); };
        }

        private void RefreshList()
        {
            if (_initializing) return;

            flpActivities.SuspendLayout();
            flpActivities.Controls.Clear();

            var filtered = _course.Activities.FindAll(a =>
            {
                bool typeOk = _filterType == "All" ||
                              string.Equals(a.TypeString, _filterType,
                                            StringComparison.OrdinalIgnoreCase);
                bool searchOk = string.IsNullOrEmpty(_searchTerm)
                    || a.Title.IndexOf(_searchTerm, StringComparison.OrdinalIgnoreCase) >= 0;
                return typeOk && searchOk;
            });

            filtered.Sort((a, b) =>
            {
                if (a.IsOverdue && !b.IsOverdue) return -1;
                if (!a.IsOverdue && b.IsOverdue) return 1;
                return a.Deadline.CompareTo(b.Deadline);
            });

            if (filtered.Count == 0)
                flpActivities.Controls.Add(BuildEmptyState());
            else
                foreach (var act in filtered)
                    flpActivities.Controls.Add(BuildActivityCard(act));

            UpdateSummaryBar(filtered);
            flpActivities.ResumeLayout();
        }

        private void UpdateSummaryBar(List<ActivityItem> list)
        {
            int total = list.Count;
            int pending = list.Sum(a => a.PendingCount);
            int chk = list.Sum(a => a.CheckedCount);
            lblSummaryBar.Text =
                $"Showing {total} of {_course.Activities.Count} activities  " +
                $"·  {pending} pending checks  ·  {chk} checked";
        }

        private Panel BuildEmptyState()
        {
            int w = Math.Max(700, flpActivities.ClientSize.Width - 40);
            var pnl = new Panel
            {
                Width = w,
                Height = 180,
                BackColor = Color.FromArgb(252, 252, 255)
            };
            pnl.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(218, 218, 228), 1.5f);
                e.Graphics.DrawRectangle(pen, 1, 1, pnl.Width - 3, pnl.Height - 3);
            };
            pnl.Controls.Add(new Label
            {
                Text = "📋  No activities found",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(160, 160, 170),
                AutoSize = false,
                Width = w,
                Height = 60,
                TextAlign = ContentAlignment.BottomCenter,
                Location = new Point(0, 40)
            });
            pnl.Controls.Add(new Label
            {
                Text = "Create a new activity using the \"+ Create Activity\" button above.",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(180, 180, 190),
                AutoSize = false,
                Width = w,
                Height = 24,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 102)
            });
            return pnl;
        }

        private Panel BuildActivityCard(ActivityItem act)
        {
            int w = Math.Max(700, flpActivities.ClientSize.Width - 40);
            var card = new Panel
            {
                Width = w,
                Height = 100,
                BackColor = act.IsPublished ? Color.White : Color.FromArgb(250, 250, 252),
                Margin = new Padding(0, 0, 0, 10)
            };

            Color typeColor = act.Type switch
            {
                ActivityType.Quiz => Color.FromArgb(63, 81, 181),
                ActivityType.Essay => Color.FromArgb(0, 150, 136),
                ActivityType.FileUpload => Color.FromArgb(76, 175, 80),
                _ => Color.FromArgb(128, 0, 0)
            };

            card.Paint += (s, e) =>
            {
                e.Graphics.FillRectangle(new SolidBrush(typeColor), 0, 0, 6, card.Height);
                using var pen = new Pen(act.IsPublished
                    ? Color.FromArgb(225, 225, 232)
                    : Color.FromArgb(210, 210, 220));
                e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            string typeIcon = act.Type switch
            {
                ActivityType.Quiz => "❓ Quiz",
                ActivityType.Essay => "📝 Essay",
                ActivityType.FileUpload => "📎 File Upload",
                _ => "📋 Assignment"
            };
            card.Controls.Add(new Label
            {
                Text = typeIcon,
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                BackColor = typeColor,
                ForeColor = Color.White,
                Location = new Point(16, 8),
                AutoSize = false,
                Size = new Size(92, 18),
                TextAlign = ContentAlignment.MiddleCenter
            });

            // ── Published / Unpublished status pill ───────────────────────────
            var lblStatus = new Label
            {
                Text = act.IsPublished ? "● Published" : "○ Unpublished",
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = act.IsPublished ? Color.FromArgb(22, 163, 74) : Color.FromArgb(120, 120, 130),
                BackColor = act.IsPublished ? Color.FromArgb(220, 252, 231) : Color.FromArgb(235, 235, 240),
                Location = new Point(116, 8),
                AutoSize = false,
                Size = new Size(92, 18),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            using (var path = new GraphicsPath())
            {
                var r = new Rectangle(0, 0, lblStatus.Width, lblStatus.Height);
                // FIX 2: To create a perfect pill, the AddArc size parameters dictate the diameter 
                // of the circle, which should perfectly match the height of the control.
                int d = r.Height; // 18px diameter
                path.AddArc(r.X, r.Y, d, d, 180, 90);
                path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
                lblStatus.Region = new Region(path);
            }
            card.Controls.Add(lblStatus);

            card.Controls.Add(new Label
            {
                Text = act.Title,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = act.IsPublished ? Color.FromArgb(30, 30, 35) : Color.FromArgb(120, 120, 130),
                Location = new Point(16, 30),
                // FIX 1: Shrunk the maximum label width to ensure it never overlaps the left edge 
                // of the buttons, solving the cut-off / "tail" visual glitch on the green button.
                Width = w - 430,
                Height = 24,
                AutoEllipsis = true
            });

            TimeSpan left = act.Deadline - DateTime.Now;
            string dlText = act.IsOverdue ? "⚠ Overdue"
                             : left.Days == 0 ? "⏰ Due Today"
                             : $"📅 {act.Deadline:MMM dd, yyyy}";
            Color dlColor = act.IsOverdue ? Color.Red
                             : left.Days <= 1 ? Color.OrangeRed
                             : Color.FromArgb(80, 80, 90);
            card.Controls.Add(new Label
            {
                Text = dlText,
                Font = new Font("Segoe UI", 8.5F,
                               act.IsOverdue ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = dlColor,
                Location = new Point(16, 58),
                AutoSize = true
            });

            card.Controls.Add(new Label
            {
                Text = $"🏆 {act.Points} pts",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(100, 100, 110),
                Location = new Point(180, 58),
                AutoSize = true
            });

            card.Controls.Add(new Label
            {
                Text = $"✅ {act.SubmittedCount}/{act.TotalStudents} submitted  " +
                       $"·  🔍 {act.CheckedCount} checked  " +
                       $"·  ⏳ {act.PendingCount} pending",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(100, 100, 110),
                Location = new Point(16, 78),
                AutoSize = true
            });

            int btnY = 32;
            int right = w - 14;
            const int btnH = 28, gap = 6;

            var btnCheck = CardBtn("Check", Color.FromArgb(63, 81, 181), 80, btnH);
            right -= btnCheck.Width;
            btnCheck.Location = new Point(right, btnY);
            btnCheck.Click += (s, e) => OpenSubmissions(act);
            card.Controls.Add(btnCheck);

            right -= gap;
            var btnEdit = CardBtn("Edit", Color.FromArgb(0, 130, 115), 60, btnH);
            right -= btnEdit.Width;
            btnEdit.Location = new Point(right, btnY);
            btnEdit.Click += (s, e) => OpenActivityForm(act);
            card.Controls.Add(btnEdit);

            right -= gap;
            var btnCopy = CardBtn("Copy", Color.FromArgb(90, 90, 100), 60, btnH);
            right -= btnCopy.Width;
            btnCopy.Location = new Point(right, btnY);
            btnCopy.Click += (s, e) => ShowCopyDialog(act);
            card.Controls.Add(btnCopy);

            right -= gap;
            var btnDel = CardBtn("Delete", Color.FromArgb(185, 50, 50), 68, btnH);
            right -= btnDel.Width;
            btnDel.Location = new Point(right, btnY);
            btnDel.Click += (s, e) => DeleteActivity(act);
            card.Controls.Add(btnDel);

            // ── Publish / Unpublish button ─────────────────────────────────────
            right -= gap;
            Color pubColor = act.IsPublished
                ? Color.FromArgb(180, 100, 0)      // amber – "Unpublish"
                : Color.FromArgb(22, 130, 60);      // green  – "Publish"
            string pubText = act.IsPublished ? "Unpublish" : "Publish";
            var btnPublish = CardBtn(pubText, pubColor, 90, btnH);
            right -= btnPublish.Width;
            btnPublish.Location = new Point(right, btnY);
            btnPublish.Click += (s, e) => TogglePublish(act);
            card.Controls.Add(btnPublish);

            return card;
        }

        private static buttonRounded CardBtn(string text, Color bg, int w, int h)
            => new buttonRounded
            {
                Text = text,
                Size = new Size(w, h),
                BackColor = bg,
                ForeColor = Color.White,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };


        private void OpenActivityForm(ActivityItem? existing)
        {
            Control openContainer = this.Parent;
            if (openContainer == null) return;

            var form = new ActivityFormPage(_course, existing);
            form.Dock = DockStyle.Fill;

            form.OnSave += saved =>
            {
                if (existing == null)
                    _course.Activities.Add(saved);

                Control c = form.Parent ?? openContainer;
                c.Controls.Remove(form);
                form.Dispose();
                c.Controls.Add(this);
                this.BringToFront();
                RefreshList();
            };

            form.OnCancel += () =>
            {
                Control c = form.Parent ?? openContainer;
                c.Controls.Remove(form);
                form.Dispose();
                c.Controls.Add(this);
                this.BringToFront();
            };

            openContainer.Controls.Remove(this);
            openContainer.Controls.Add(form);
            form.BringToFront();
        }

        private void OpenSubmissions(ActivityItem act)
        {
            Control openContainer = this.Parent;
            if (openContainer == null) return;

            var subList = new SubmissionList(act, _course);
            subList.Dock = DockStyle.Fill;

            subList.OnBack += () =>
            {
                Control c = subList.Parent ?? openContainer;
                c.Controls.Remove(subList);
                subList.Dispose();
                c.Controls.Add(this);
                this.BringToFront();
                RefreshList();
            };

            openContainer.Controls.Remove(this);
            openContainer.Controls.Add(subList);
            subList.BringToFront();
        }

        private void ShowCopyDialog(ActivityItem act)
        {
            var allCourses = new List<CourseActivity> { _course };
            using var dlg = new CopyActivityDialog(act, allCourses);
            if (dlg.ShowDialog() == DialogResult.OK)
                MessageBox.Show(
                    $"Activity \"{act.Title}\" copied successfully.",
                    "Copy Activity", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DeleteActivity(ActivityItem act)
        {
            var res = MessageBox.Show(
                $"Delete \"{act.Title}\"?\nThis cannot be undone.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                _course.Activities.Remove(act);
                RefreshList();
            }
        }

        private void TogglePublish(ActivityItem act)
        {
            act.IsPublished = !act.IsPublished;
            string msg = act.IsPublished
                ? $"✅ \"{act.Title}\" is now published.\nStudents can see and submit this activity."
                : $"🔒 \"{act.Title}\" has been unpublished.\nStudents can no longer access this activity.";
            MessageBox.Show(msg,
                act.IsPublished ? "Activity Published" : "Activity Unpublished",
                MessageBoxButtons.OK,
                act.IsPublished ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            RefreshList();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (OnBack != null) { OnBack.Invoke(); return; }

            Control container = this.Parent;
            if (container == null) return;
            container.Controls.Remove(this);
        }

        private void btnSave_Click(object sender, EventArgs e)
            => OpenActivityForm(null);

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_initializing) return;
            _searchTerm = txtSearch.Text;
            _searchTimer.Stop();
            _searchTimer.Start();
        }

        private void cmbFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_initializing) return;
            _filterType = cmbFilterType.SelectedItem?.ToString() ?? "All";
            RefreshList();
        }
    }
}