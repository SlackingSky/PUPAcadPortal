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

        public AssignmentManagement(CourseActivity course)
        {
            _course = course;
            InitializeComponent();
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
            flpActivities.SuspendLayout();
            flpActivities.Controls.Clear();

            var filtered = _course.Activities.FindAll(a =>
            {
                bool typeMatch = _filterType == "All" || a.TypeString == _filterType;
                bool searchMatch = string.IsNullOrEmpty(_searchTerm)
                    || a.Title.IndexOf(_searchTerm, StringComparison.OrdinalIgnoreCase) >= 0;
                return typeMatch && searchMatch;
            });

            // Sort: overdue first, then by deadline asc
            filtered.Sort((a, b) =>
            {
                if (a.IsOverdue && !b.IsOverdue) return -1;
                if (!a.IsOverdue && b.IsOverdue) return 1;
                return a.Deadline.CompareTo(b.Deadline);
            });

            if (filtered.Count == 0)
            {
                flpActivities.Controls.Add(BuildEmptyState());
            }
            else
            {
                foreach (var act in filtered)
                    flpActivities.Controls.Add(BuildActivityCard(act));
            }

            UpdateSummaryBar(filtered);
            flpActivities.ResumeLayout();
        }

        private void UpdateSummaryBar(List<ActivityItem> list)
        {
            int total = list.Count;
            int pending = list.Sum(a => a.PendingCount);
            int checked_ = list.Sum(a => a.CheckedCount);
            lblSummaryBar.Text = $"Showing {total} of {_course.Activities.Count} activities  ·  {pending} pending checks  ·  {checked_} checked";
        }

        private Panel BuildEmptyState()
        {
            int w = Math.Max(700, flpActivities.ClientSize.Width - 40);
            var pnl = new Panel { Width = w, Height = 180, BackColor = Color.FromArgb(252, 252, 255) };
            pnl.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(220, 220, 230), 1.5f);
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

        //  Activity card builder 
        private Panel BuildActivityCard(ActivityItem act)
        {
            int w = Math.Max(700, flpActivities.ClientSize.Width - 40);
            var card = new Panel
            {
                Width = w,
                Height = 100,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 10)
            };

            // Left accent bar colour by type
            Color typeColor = act.Type switch
            {
                ActivityType.Quiz => Color.FromArgb(63, 81, 181),
                ActivityType.Essay => Color.FromArgb(0, 150, 136),
                ActivityType.FileUpload => Color.FromArgb(76, 175, 80),
                _ => Color.FromArgb(128, 0, 0)
            };

            card.Paint += (s, e) =>
            {
                using var accentBrush = new SolidBrush(typeColor);
                e.Graphics.FillRectangle(accentBrush, 0, 0, 6, card.Height);
                using var pen = new Pen(Color.FromArgb(225, 225, 232));
                e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            // ── Type badge ──
            string typeIcon = act.Type switch
            {
                ActivityType.Quiz => "❓ Quiz",
                ActivityType.Essay => "📝 Essay",
                ActivityType.FileUpload => "📎 File Upload",
                _ => "📋 Assignment"
            };
            var lblType = new Label
            {
                Text = typeIcon,
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                BackColor = typeColor,
                ForeColor = Color.White,
                Location = new Point(16, 8),
                AutoSize = false,
                Size = new Size(92, 18),
                TextAlign = ContentAlignment.MiddleCenter
            };
            card.Controls.Add(lblType);

            // ── Title ──
            var lblTitle = new Label
            {
                Text = act.Title,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 35),
                Location = new Point(16, 30),
                Width = w - 400,
                Height = 24,
                AutoEllipsis = true
            };
            card.Controls.Add(lblTitle);

            // ── Deadline ──
            TimeSpan left = act.Deadline - DateTime.Now;
            string dlText = act.IsOverdue ? "⚠ Overdue" : left.Days == 0 ? "⏰ Due Today" : $"📅 {act.Deadline:MMM dd, yyyy}";
            Color dlColor = act.IsOverdue ? Color.Red : left.Days <= 1 ? Color.OrangeRed : Color.FromArgb(80, 80, 90);
            var lblDeadline = new Label
            {
                Text = dlText,
                Font = new Font("Segoe UI", 8.5F, act.IsOverdue ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = dlColor,
                Location = new Point(16, 58),
                AutoSize = true
            };
            card.Controls.Add(lblDeadline);

            // ── Points ──
            var lblPts = new Label
            {
                Text = $"🏆 {act.Points} pts",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(100, 100, 110),
                Location = new Point(180, 58),
                AutoSize = true
            };
            card.Controls.Add(lblPts);

            // ── Submission counts ──
            var lblSub = new Label
            {
                Text = $"✅ {act.SubmittedCount}/{act.TotalStudents} submitted  ·  🔍 {act.CheckedCount} checked  ·  ⏳ {act.PendingCount} pending",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(100, 100, 110),
                Location = new Point(16, 78),
                AutoSize = true
            };
            card.Controls.Add(lblSub);

            // ── Action buttons (right-side) ──
            int btnY = 32;
            int right = w - 14;
            const int btnH = 28, gap = 6;

            var btnCheck = BuildCardBtn("Check", Color.FromArgb(63, 81, 181), 80, btnH);
            right -= btnCheck.Width;
            btnCheck.Location = new Point(right, btnY);
            btnCheck.Click += (s, e) => OpenSubmissions(act);
            card.Controls.Add(btnCheck);

            right -= gap;
            var btnEdit = BuildCardBtn("Edit", Color.FromArgb(0, 130, 115), 60, btnH);
            right -= btnEdit.Width;
            btnEdit.Location = new Point(right, btnY);
            btnEdit.Click += (s, e) => OpenActivityForm(act);
            card.Controls.Add(btnEdit);

            right -= gap;
            var btnCopy = BuildCardBtn("Copy", Color.FromArgb(90, 90, 100), 60, btnH);
            right -= btnCopy.Width;
            btnCopy.Location = new Point(right, btnY);
            btnCopy.Click += (s, e) => ShowCopyDialog(act);
            card.Controls.Add(btnCopy);

            right -= gap;
            var btnDelete = BuildCardBtn("Delete", Color.FromArgb(185, 50, 50), 68, btnH);
            right -= btnDelete.Width;
            btnDelete.Location = new Point(right, btnY);
            btnDelete.Click += (s, e) => DeleteActivity(act);
            card.Controls.Add(btnDelete);

            return card;
        }

        private static buttonRounded BuildCardBtn(string text, Color bg, int w, int h)
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

        //  Navigation to sub-pages 
        private void OpenActivityForm(ActivityItem? existingActivity)
        {
            var parent = this.Parent; if (parent == null) return;

            var form = new ActivityFormPage(_course, existingActivity);
            form.Dock = DockStyle.Fill;
            form.OnSave += saved =>
            {
                if (existingActivity == null)
                    _course.Activities.Add(saved);
                // if editing, object ref already updated
                parent.Controls.Remove(form);
                parent.Controls.Add(this);
                this.BringToFront();
                RefreshList();
            };
            form.OnCancel += () =>
            {
                parent.Controls.Remove(form);
                parent.Controls.Add(this);
                this.BringToFront();
            };

            parent.Controls.Remove(this);
            parent.Controls.Add(form);
            form.BringToFront();
        }

        private void OpenSubmissions(ActivityItem act)
        {
            var parent = this.Parent; if (parent == null) return;

            var subList = new SubmissionList(act, _course);
            subList.Dock = DockStyle.Fill;
            subList.OnBack += () =>
            {
                parent.Controls.Remove(subList);
                parent.Controls.Add(this);
                this.BringToFront();
                RefreshList();
            };

            parent.Controls.Remove(this);
            parent.Controls.Add(subList);
            subList.BringToFront();
        }

        private void ShowCopyDialog(ActivityItem act)
        {
            // All courses available to copy into – in a real app you'd fetch from DB.
            var allCourses = new List<CourseActivity> { _course }; // extend as needed
            using var dlg = new CopyActivityDialog(act, allCourses);
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show($"Activity \"{act.Title}\" copied successfully.", "Copy Activity",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DeleteActivity(ActivityItem act)
        {
            var res = MessageBox.Show($"Delete activity \"{act.Title}\"?\nThis cannot be undone.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                _course.Activities.Remove(act);
                RefreshList();
            }
        }

        private void pnlHeader_SizeChanged(object sender, System.EventArgs e)
        {
            if (this.btnSave != null && this.pnlHeader != null)
            {
                this.btnSave.Location = new System.Drawing.Point(this.pnlHeader.Width - this.btnSave.Width - 12, 17);
            }
        }

        //  Toolbar events 
        private void btnSave_Click(object sender, EventArgs e)
            => OpenActivityForm(null);

        private void btnBack_Click(object sender, EventArgs e)
            => OnBack?.Invoke();

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _searchTerm = txtSearch.Text;
            _searchTimer.Stop();
            _searchTimer.Start();
        }

        private void cmbFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _filterType = cmbFilterType.SelectedItem?.ToString() ?? "All";
            RefreshList();
        }
    }
}