using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Admin
{
    public partial class AnnounceContentAdmin : UserControl
    {
        // ── Announcement system ──────────────────────────────────────────
        private List<AdminAnnouncement> announcements = new List<AdminAnnouncement>();
        private int editingAnnouncementId = -1;
        private string _activeCategoryFilter = "all";
        private string _activeStatusFilter = "all";
        private string _searchQuery = "";
        private string _activeSortMode = "newest";   // NEW

        private CreateAnnouncementAdmin _createAnnouncementUC;
        private ViewAnnouncementAdmin _viewAnnouncementUC;

        private static readonly Dictionary<string, Color> AnnCatIconColor = new Dictionary<string, Color>
        {
            ["General"] = Color.FromArgb(0x37, 0x8a, 0xdd),
            ["Academic"] = Color.FromArgb(0x63, 0x99, 0x22),
            ["Events"] = Color.FromArgb(0xd4, 0x53, 0x7e),
            ["Emergency"] = Color.FromArgb(0xc8, 0x1e, 0x1e),
            ["Enrollment"] = Color.FromArgb(0x0d, 0x9a, 0x8a),
        };
        private static readonly Dictionary<string, Color> AnnCatBgColor = new Dictionary<string, Color>
        {
            ["General"] = Color.FromArgb(0xe6, 0xf1, 0xfb),
            ["Academic"] = Color.FromArgb(0xea, 0xf3, 0xde),
            ["Events"] = Color.FromArgb(0xfb, 0xea, 0xf0),
            ["Emergency"] = Color.FromArgb(0xff, 0xe0, 0xe0),
            ["Enrollment"] = Color.FromArgb(0xd6, 0xf4, 0xf1),
        };

        public AnnounceContentAdmin()
        {
            InitializeComponent();
        }

        // ════════════════════════════════════════════════════════════════
        //  ANNOUNCEMENT SYSTEM — ADMIN PORTAL
        // ════════════════════════════════════════════════════════════════

        private bool _announcementInited = false;

        public void InitAnnouncementPanelIfNeeded()
        {
            if (_announcementInited) return;
            _announcementInited = true;

            // Wire search / filter controls that live in the designer panel
            // (add null-checks so it won't crash if controls are renamed)
            if (txtAnnSearch != null)
                txtAnnSearch.TextChanged += (s, e) => { _searchQuery = txtAnnSearch.Text.Trim(); RenderAnnouncements(); };

            if (cmbFilter != null)
            {
                if (cmbFilter.Items.Count == 0)
                    cmbFilter.Items.AddRange(new object[]
                    {
                        "All announcements",
                        "Active only",
                        "Inactive only",
                        "Pinned only",
                        "Urgent only",
                        "With attachment",
                        "Notified – Students",
                        "Notified – Instructors",
                    });
                cmbFilter.SelectedIndex = 0;
                cmbFilter.SelectedIndexChanged += (s, e) =>
                {
                    _activeStatusFilter = cmbFilter.SelectedItem?.ToString() switch
                    {
                        "Active only" => "active",
                        "Inactive only" => "inactive",
                        "Pinned only" => "pinned",
                        "Urgent only" => "urgent",
                        "With attachment" => "attachment",
                        "Notified – Students" => "notify_students",
                        "Notified – Instructors" => "notify_instructors",
                        _ => "all",
                    };
                    RenderAnnouncements();
                };
            }

            if (cmbSortBy != null)
            {
                if (cmbSortBy.Items.Count == 0)
                    cmbSortBy.Items.AddRange(new object[]
                    {
                        "Newest first",
                        "Oldest first",
                        "A → Z (Title)",
                        "Z → A (Title)",
                        "Most viewed",
                        "Least viewed",
                        "Pinned first",
                        "Urgent first",
                    });
                cmbSortBy.SelectedIndex = 0;
                cmbSortBy.SelectedIndexChanged += (s, e) =>
                {
                    _activeSortMode = cmbSortBy.SelectedItem?.ToString() switch
                    {
                        "Oldest first" => "oldest",
                        "A → Z (Title)" => "az",
                        "Z → A (Title)" => "za",
                        "Most viewed" => "most_viewed",
                        "Least viewed" => "least_viewed",
                        "Pinned first" => "pinned",
                        "Urgent first" => "urgent",
                        _ => "newest",
                    };
                    RenderAnnouncements();
                };
            }

            if (btnCreateAnnouncement != null)
                btnCreateAnnouncement.Click += (s, e) =>
                {
                    editingAnnouncementId = -1;
                    _createAnnouncementUC.LoadForEdit(new AnnouncementDataAdmin());
                    ShowCreateAnnouncementUC();
                };

            // Create + wire the CreateAnnouncement UC
            _createAnnouncementUC = new CreateAnnouncementAdmin { Visible = false, Anchor = AnchorStyles.None };
            _createAnnouncementUC.AnnouncementPosted += OnAnnouncementPosted;
            _createAnnouncementUC.CloseRequested += (s, e) => HideCreateAnnouncementUC();
            pnlAnnouncement.Controls.Add(_createAnnouncementUC);

            // Create + wire the ViewAnnouncement UC
            _viewAnnouncementUC = new ViewAnnouncementAdmin { Visible = false, Anchor = AnchorStyles.None };
            _viewAnnouncementUC.EditRequested += OnViewEdit;
            _viewAnnouncementUC.DeleteRequested += (s, id) => DeleteAnnouncement(id);
            _viewAnnouncementUC.CloseRequested += (s, e) => HideViewAnnouncementUC();
            pnlAnnouncement.Controls.Add(_viewAnnouncementUC);

            // Resize → re-center overlay UCs
            pnlAnnouncement.Resize += (s, e) =>
            {
                if (_createAnnouncementUC.Visible) CenterAnnControl(_createAnnouncementUC);
                if (_viewAnnouncementUC.Visible) CenterAnnControl(_viewAnnouncementUC);
                RenderAnnouncements();
                BuildAnnCategorySidebar();
                RenderAnnInsights();
            };

            // Seed data and first render
            SeedAnnouncements();
            BuildAnnCategorySidebar();
            RenderAnnouncements();
        }

        // ── Seed sample data ─────────────────────────────────────────────
        private void SeedAnnouncements()
        {
            announcements.AddRange(new[]
            {
                new AdminAnnouncement { Id=1,  Title="Midterm exam schedule released",
                    Description="The official midterm examination schedule for all BSIT 2nd year subjects has been posted. Please check your respective rooms and ensure you bring your student IDs.",
                    Category="Academic", Status="active", IsPinned=true, IsUrgent=true,
                    NotifyStudents=true, NotifyInstructors=true,
                    ViewedCount=28, TotalStudents=40, Date=DateTime.Now },
                new AdminAnnouncement { Id=2,  Title="Class suspension – May 12",
                    Description="All classes on Monday, May 12 are suspended due to the declared public holiday.",
                    Category="General", Status="active", ViewedCount=35, TotalStudents=40,
                    NotifyStudents=true, Date=DateTime.Now.AddHours(-5) },
                new AdminAnnouncement { Id=3,  Title="Programming 1 – lab activity this Friday",
                    Description="Bring your laptops for the graded lab activity covering Modules 4 and 5.",
                    Category="Academic", Status="active", ViewedCount=22, TotalStudents=40, Date=DateTime.Now.AddDays(-2) },
                new AdminAnnouncement { Id=4,  Title="Campus foundation day celebration",
                    Description="Join us for the PUP Foundation Day celebration on May 17.",
                    Category="Events", Status="inactive", ViewedCount=18, TotalStudents=40, Date=DateTime.Now.AddDays(-4) },
                new AdminAnnouncement { Id=5,  Title="Reminder: submit assignment outputs",
                    Description="All pending assignment outputs must be submitted via the LMS before May 15, 11:59 PM.",
                    Category="General", Status="active", ViewedCount=30, TotalStudents=40, Date=DateTime.Now.AddDays(-5) },
                new AdminAnnouncement { Id=6,  Title="Final Exam Coverage – Programming 1",
                    Description="The official final examination coverage for Introduction to Programming 1 has been posted.",
                    Category="Academic", Status="active", IsPinned=true, ViewedCount=32, TotalStudents=40,
                    NotifyStudents=true, NotifyInstructors=true, Date=DateTime.Now.AddHours(-2) },
                new AdminAnnouncement { Id=7,  Title="Graded Recitation – Information Management",
                    Description="There will be a graded recitation next Wednesday covering database normalization.",
                    Category="Academic", Status="active", ViewedCount=15, TotalStudents=40, Date=DateTime.Now.AddDays(-1) },
                new AdminAnnouncement { Id=8,  Title="Enrollment Period – 1st Semester 2026-2027",
                    Description="Online enrollment for 1st Semester AY 2026-2027 opens on June 1. Please coordinate with your respective advisers for pre-enrollment clearance.",
                    Category="Enrollment", Status="active", ViewedCount=38, TotalStudents=40,
                    NotifyStudents=true, Date=DateTime.Now.AddDays(-1).AddHours(-3) },
                new AdminAnnouncement { Id=9,  Title="IT Career Fair – Volunteer Marshals Needed",
                    Description="The Career Services Office is looking for 15 student volunteers to serve as event marshals.",
                    Category="Events", Status="active", ViewedCount=20, TotalStudents=40, Date=DateTime.Now.AddDays(-2) },
                new AdminAnnouncement { Id=10, Title="Capstone Group Submissions – Revised Deadline",
                    Description="The submission deadline for Capstone Project Chapter 3 has been moved to May 19.",
                    Category="Academic", Status="active", IsPinned=true, IsUrgent=true, ViewedCount=36, TotalStudents=40,
                    NotifyStudents=true, Date=DateTime.Now.AddDays(-3) },
                new AdminAnnouncement { Id=11, Title="Enrollment Requirements Reminder",
                    Description="All students must submit updated documents (medical certificate, good moral clearance) before enrollment. Incomplete requirements will not be processed.",
                    Category="Enrollment", Status="active", ViewedCount=25, TotalStudents=40,
                    NotifyStudents=true, NotifyInstructors=true, Date=DateTime.Now.AddDays(-4) },
                new AdminAnnouncement { Id=12, Title="University Foundation Day – May 19",
                    Description="All classes are suspended on May 19 in celebration of the University's Foundation Day.",
                    Category="Events", Status="active", ViewedCount=29, TotalStudents=40, Date=DateTime.Now.AddDays(-5) },
                new AdminAnnouncement { Id=13, Title="⚠ Campus Emergency Drill – May 20",
                    Description="A mandatory earthquake and fire emergency drill will be conducted on May 20. All students and faculty must participate. Classes will be briefly suspended during the drill.",
                    Category="Emergency", Status="active", IsPinned=true, IsUrgent=true,
                    NotifyStudents=true, NotifyInstructors=true,
                    ViewedCount=40, TotalStudents=40, Date=DateTime.Now.AddHours(-1) },
            });
        }

        // ── Render cards ─────────────────────────────────────────────────
        private void RenderAnnouncements()
        {
            if (fplAnnouncement == null) return;

            fplAnnouncement.SuspendLayout();
            fplAnnouncement.Controls.Clear();
            fplAnnouncement.FlowDirection = FlowDirection.TopDown;
            fplAnnouncement.WrapContents = false;
            fplAnnouncement.AutoScroll = true;
            fplAnnouncement.HorizontalScroll.Enabled = false;
            fplAnnouncement.HorizontalScroll.Visible = false;

            var filtered = announcements.AsEnumerable();

            if (_activeCategoryFilter != "all")
                filtered = filtered.Where(a => a.Category == _activeCategoryFilter);

            filtered = _activeStatusFilter switch
            {
                "active" => filtered.Where(a => a.Status == "active"),
                "inactive" => filtered.Where(a => a.Status != "active"),
                "pinned" => filtered.Where(a => a.IsPinned),
                "urgent" => filtered.Where(a => a.IsUrgent),
                "attachment" => filtered.Where(a => !string.IsNullOrWhiteSpace(a.AttachedFile)),
                "notify_students" => filtered.Where(a => a.NotifyStudents),
                "notify_instructors" => filtered.Where(a => a.NotifyInstructors),
                _ => filtered,
            };

            if (!string.IsNullOrWhiteSpace(_searchQuery))
            {
                var q = _searchQuery.ToLower();
                filtered = filtered.Where(a =>
                    a.Title.ToLower().Contains(q) || a.Description.ToLower().Contains(q)
                    || a.Category.ToLower().Contains(q));
            }

            var sorted = _activeSortMode switch
            {
                "oldest" => filtered.OrderBy(a => a.IsPinned ? 0 : 1).ThenBy(a => a.Date),
                "az" => filtered.OrderBy(a => a.IsPinned ? 0 : 1).ThenBy(a => a.Title),
                "za" => filtered.OrderBy(a => a.IsPinned ? 0 : 1).ThenByDescending(a => a.Title),
                "most_viewed" => filtered.OrderBy(a => a.IsPinned ? 0 : 1).ThenByDescending(a => a.ViewedCount),
                "least_viewed" => filtered.OrderBy(a => a.IsPinned ? 0 : 1).ThenBy(a => a.ViewedCount),
                "pinned" => filtered.OrderBy(a => a.IsPinned ? 0 : 1).ThenByDescending(a => a.Date),
                "urgent" => filtered.OrderBy(a => a.IsUrgent ? 0 : 1).ThenByDescending(a => a.Date),
                _ => filtered.OrderBy(a => a.IsPinned ? 0 : 1).ThenByDescending(a => a.Date),
            };
            var sortedList = sorted.ToList();

            int panelWidth = Math.Max(300, fplAnnouncement.ClientSize.Width - 30);

            foreach (var a in sortedList)
            {
                var card = BuildAdminAnnouncementCard(a, panelWidth);
                card.Margin = new Padding(4, 4, 4, 4);
                fplAnnouncement.Controls.Add(card);
            }

            if (lblShowing != null)
                lblShowing.Text = sortedList.Count == 0
                    ? "No announcements found"
                    : $"Showing 1–{sortedList.Count} of {announcements.Count} announcements";

            fplAnnouncement.ResumeLayout();
            BuildAnnCategorySidebar();
            RenderAnnPinned();
            RenderAnnInsights();
        }

        // ── Self-contained card builder (no stub dependency) ─────────────
        private Panel BuildAdminAnnouncementCard(AdminAnnouncement a, int cardWidth)
        {
            Color iconCol = AnnCatIconColor.GetValueOrDefault(a.Category, Color.Gray);
            Color iconBg = AnnCatBgColor.GetValueOrDefault(a.Category, Color.WhiteSmoke);
            bool isNew = (DateTime.Now - a.Date) < TimeSpan.FromDays(1);

            // Extra bottom rows needed?
            bool hasNotify = a.NotifyStudents || a.NotifyInstructors;
            bool hasAttach = !string.IsNullOrWhiteSpace(a.AttachedFile);
            int extraHeight = (hasNotify || hasAttach) ? 22 : 0;
            int cardHeight = 112 + extraHeight;

            var card = new Panel
            {
                Width = cardWidth,
                Height = cardHeight,
                BackColor = a.Category == "Emergency"
                    ? Color.FromArgb(255, 245, 245)
                    : a.Status == "active" ? Color.White : Color.FromArgb(250, 250, 250),
                Cursor = Cursors.Hand,
                Tag = a.Id,
            };
            card.Paint += (s, pe) =>
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen = new Pen(Color.FromArgb(218, 218, 218), 1f);
                using var path = AnnRoundedPath(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 8);
                pe.Graphics.DrawPath(pen, path);
            };

            // ── Icon circle ──────────────────────────────────────────────
            var icon = new Panel { Size = new Size(42, 42), Location = new Point(12, 16), BackColor = iconBg };
            icon.Paint += (s, pe) =>
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var b = new SolidBrush(iconBg);
                pe.Graphics.FillEllipse(b, 0, 0, 41, 41);
                string letter = a.Category.Length > 0 ? a.Category.Substring(0, 1) : "?";
                using var f = new Font("Segoe UI", 13f, FontStyle.Bold);
                using var fb = new SolidBrush(iconCol);
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                pe.Graphics.DrawString(letter, f, fb, new RectangleF(0, 0, 42, 42), sf);
            };
            card.Controls.Add(icon);

            int textX = 64;
            int rightM = 12;   // right margin

            // ── NEW ribbon ───────────────────────────────────────────────
            if (isNew)
            {
                var ribbon = new System.Windows.Forms.Label
                {
                    AutoSize = false,
                    Size = new Size(38, 16),
                    Location = new Point(0, 0),
                    BackColor = Color.FromArgb(22, 163, 74),
                    ForeColor = Color.White,
                    Text = "NEW",
                    Font = new Font("Segoe UI", 7f, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                AnnMakeRounded(ribbon, 6);
                card.Controls.Add(ribbon);
                ribbon.BringToFront();
            }

            // ── ⋮ menu button ─────────────────────────────────────────────
            var btnMenu = new Button
            {
                Size = new Size(28, 28),
                Location = new Point(cardWidth - 36, 8),
                FlatStyle = FlatStyle.Flat,
                Text = "⋮",
                Font = new Font("Segoe UI", 11f),
                ForeColor = Color.FromArgb(150, 150, 150),
                Cursor = Cursors.Hand,
                TabStop = false,
                BackColor = Color.Transparent,
            };
            btnMenu.FlatAppearance.BorderSize = 0;
            btnMenu.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);

            var capturedId = a.Id;
            var capturedTitle = a.Title;

            var ctx = new ContextMenuStrip();
            ctx.Items.Add("✏  Edit", null, (s, ev) => EditAnnouncement(capturedId));
            ctx.Items.Add(a.IsPinned ? "📌  Unpin" : "📌  Pin", null, (s, ev) =>
            {
                var ann = announcements.Find(x => x.Id == capturedId);
                if (ann != null) { ann.IsPinned = !ann.IsPinned; RenderAnnouncements(); }
            });
            ctx.Items.Add(a.Status == "active" ? "⏸  Set inactive" : "▶  Set active", null, (s, ev) =>
            {
                var ann = announcements.Find(x => x.Id == capturedId);
                if (ann != null) { ann.Status = ann.Status == "active" ? "inactive" : "active"; RenderAnnouncements(); }
            });
            ctx.Items.Add(new ToolStripSeparator());
            ctx.Items.Add("🗑  Delete", null, (s, ev) =>
            {
                if (MessageBox.Show($"Delete \"{capturedTitle}\"?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    DeleteAnnouncement(capturedId);
            });
            btnMenu.Click += (s, ev) => ctx.Show(btnMenu, new Point(0, btnMenu.Height));
            card.Controls.Add(btnMenu);

            // ── Date label (left-aligned after textX, top row) ───────────
            // Keep date width bounded so it never crowds the menu button
            int dateMaxW = Math.Max(60, cardWidth - textX - 40);
            string dateText = a.Date.ToString("MMM d, yyyy  h:mm tt");
            var lblDate = new System.Windows.Forms.Label
            {
                AutoSize = false,
                Size = new Size(Math.Min(160, dateMaxW), 16),
                Location = new Point(textX, 10),
                Text = dateText,
                Font = new Font("Segoe UI", 7.5f),
                ForeColor = Color.FromArgb(130, 130, 130),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
            };
            card.Controls.Add(lblDate);

            // ── Pin + Urgent + Category badges (row 2) ────────────────────
            int badgeY = 30;
            int badgeX = textX;

            if (a.IsPinned)
            {
                var pin = new System.Windows.Forms.Label
                {
                    AutoSize = true,
                    Text = "📌",
                    Font = new Font("Segoe UI", 9f),
                    Location = new Point(badgeX, badgeY),
                    BackColor = Color.Transparent
                };
                card.Controls.Add(pin);
                badgeX += 22;
            }
            if (a.IsUrgent)
            {
                var urg = new System.Windows.Forms.Label
                {
                    AutoSize = false,
                    Size = new Size(62, 18),
                    Location = new Point(badgeX, badgeY),
                    Text = "⚠ URGENT",
                    Font = new Font("Segoe UI", 7f, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.Firebrick,
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                AnnMakeRounded(urg, 9);
                card.Controls.Add(urg);
                badgeX += 68;
            }

            var pillFont = new Font("Segoe UI", 7.5f, FontStyle.Bold);
            // Clamp pill width so it never goes off the right edge
            int pillW = Math.Min(
                TextRenderer.MeasureText(a.Category, pillFont).Width + 14,
                Math.Max(30, cardWidth - badgeX - rightM - 4));
            var catPill = new System.Windows.Forms.Label
            {
                AutoSize = false,
                Size = new Size(pillW, 18),
                Location = new Point(badgeX, badgeY),
                Text = a.Category,
                Font = pillFont,
                ForeColor = iconCol,
                BackColor = iconBg,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoEllipsis = true,
            };
            AnnMakeRounded(catPill, 9);
            card.Controls.Add(catPill);
            badgeX += pillW + 6;

            if (a.Status != "active" && badgeX + 58 < cardWidth - rightM)
            {
                var ib = new System.Windows.Forms.Label
                {
                    AutoSize = false,
                    Size = new Size(56, 18),
                    Location = new Point(badgeX, badgeY),
                    Text = "Inactive",
                    Font = new Font("Segoe UI", 7.5f),
                    ForeColor = Color.FromArgb(100, 100, 100),
                    BackColor = Color.FromArgb(230, 230, 230),
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                AnnMakeRounded(ib, 9);
                card.Controls.Add(ib);
            }

            // ── Title (row 3) ─────────────────────────────────────────────
            int titleW = Math.Max(40, cardWidth - textX - rightM - 4);
            var lblTitle = new System.Windows.Forms.Label
            {
                AutoSize = false,
                Size = new Size(titleW, 22),
                Location = new Point(textX, 52),
                Text = a.Title,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 20, 20),
                BackColor = Color.Transparent,
                AutoEllipsis = true,
            };
            card.Controls.Add(lblTitle);

            // ── Description (row 4) ───────────────────────────────────────
            int descW = Math.Max(40, cardWidth - textX - rightM - 4);
            var lblDesc = new System.Windows.Forms.Label
            {
                AutoSize = false,
                Size = new Size(descW, 20),
                Location = new Point(textX, 75),
                Text = a.Description,
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(90, 90, 90),
                BackColor = Color.Transparent,
                AutoEllipsis = true,
            };
            card.Controls.Add(lblDesc);

            // ── Author + date (row 5) — no view/read rate ─────────────────
            int authorY = 94;
            var lblAuthor = new System.Windows.Forms.Label
            {
                AutoSize = true,
                Location = new Point(textX, authorY),
                Text = "👤 " + (string.IsNullOrWhiteSpace(a.InstructorName) ? "Admin" : a.InstructorName),
                Font = new Font("Segoe UI", 7.5f),
                ForeColor = Color.FromArgb(110, 110, 110),
                BackColor = Color.Transparent,
            };
            card.Controls.Add(lblAuthor);

            // ── Notification + Attachment badges (row 6, only if present) ─
            if (hasNotify || hasAttach)
            {
                int badgeRow = 112;
                int bx = textX;

                if (hasNotify)
                {
                    var targets = new System.Collections.Generic.List<string>();
                    if (a.NotifyStudents) targets.Add("Students");
                    if (a.NotifyInstructors) targets.Add("Instructors");
                    int nbW = Math.Min(
                        TextRenderer.MeasureText("🔔 " + string.Join("+", targets), new Font("Segoe UI", 7f, FontStyle.Bold)).Width + 14,
                        Math.Max(40, cardWidth - bx - rightM - 4));
                    var notifBadge = new System.Windows.Forms.Label
                    {
                        AutoSize = false,
                        Size = new Size(nbW, 17),
                        Location = new Point(bx, badgeRow),
                        Text = "🔔 " + string.Join("+", targets),
                        Font = new Font("Segoe UI", 7f, FontStyle.Bold),
                        ForeColor = Color.FromArgb(30, 100, 180),
                        BackColor = Color.FromArgb(220, 235, 255),
                        TextAlign = ContentAlignment.MiddleCenter,
                        AutoEllipsis = true,
                    };
                    AnnMakeRounded(notifBadge, 8);
                    card.Controls.Add(notifBadge);
                    bx += nbW + 6;
                }

                if (hasAttach && bx + 44 < cardWidth - rightM)
                {
                    bool isPdf = a.AttachedFile!.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase);
                    var attachBadge = new System.Windows.Forms.Label
                    {
                        AutoSize = false,
                        Size = new Size(44, 17),
                        Location = new Point(bx, 112),
                        Text = isPdf ? "📄 PDF" : "🖼 IMG",
                        Font = new Font("Segoe UI", 7f, FontStyle.Bold),
                        ForeColor = Color.FromArgb(100, 60, 160),
                        BackColor = Color.FromArgb(238, 233, 255),
                        TextAlign = ContentAlignment.MiddleCenter,
                    };
                    AnnMakeRounded(attachBadge, 8);
                    card.Controls.Add(attachBadge);
                }
            }

            // ── Click → open detail ───────────────────────────────────────
            EventHandler openDetail = (s, ev) =>
            {
                var ann = announcements.Find(x => x.Id == capturedId);
                if (ann != null) ShowViewAnnouncementUC(ann);
            };
            card.Click += openDetail;
            lblTitle.Click += openDetail;
            lblDesc.Click += openDetail;
            icon.Click += openDetail;

            return card;
        }

        private static GraphicsPath AnnRoundedPath(Rectangle r, int rad)
        {
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, rad, rad, 180, 90);
            p.AddArc(r.Right - rad, r.Y, rad, rad, 270, 90);
            p.AddArc(r.Right - rad, r.Bottom - rad, rad, rad, 0, 90);
            p.AddArc(r.X, r.Bottom - rad, rad, rad, 90, 90);
            p.CloseFigure();
            return p;
        }

        // ── Category sidebar ─────────────────────────────────────────────
        private void BuildAnnCategorySidebar()
        {
            if (flpCategories == null) return;

            flpCategories.SuspendLayout();
            flpCategories.Controls.Clear();
            flpCategories.FlowDirection = FlowDirection.TopDown;
            flpCategories.WrapContents = false;
            flpCategories.AutoScroll = true;
            flpCategories.BackColor = Color.White;   // original white background

            var categories = new[] { "all", "General", "Academic", "Events", "Emergency", "Enrollment" };

            foreach (var cat in categories)
            {
                int count = cat == "all" ? announcements.Count : announcements.Count(a => a.Category == cat);
                bool isActive = _activeCategoryFilter == cat;

                Color dotCol = cat == "all"
                    ? Color.FromArgb(139, 0, 0)   // maroon for "All"
                    : AnnCatIconColor.GetValueOrDefault(cat, Color.Gray);

                int rowW = Math.Max(100, flpCategories.ClientSize.Width - 25);
                var row = new Panel
                {
                    Width = rowW,
                    Height = 34,
                    Margin = new Padding(0, 0, 0, 4),
                    BackColor = isActive ? Color.FromArgb(245, 238, 238) : Color.Transparent,
                    Cursor = Cursors.Hand,
                    Tag = cat
                };

                var dot = new Panel { Size = new Size(9, 9), BackColor = dotCol, Location = new Point(10, 13) };
                AnnMakeCircle(dot);

                var lbl = new System.Windows.Forms.Label
                {
                    AutoSize = false,
                    Size = new Size(rowW - 60, 34),
                    Location = new Point(26, 0),
                    Text = cat == "all" ? "All" : cat,
                    Font = new Font("Segoe UI", 9.5f, isActive ? FontStyle.Bold : FontStyle.Regular),
                    ForeColor = Color.FromArgb(40, 40, 40),    // original dark text
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.Transparent,
                };

                var badge = new System.Windows.Forms.Label
                {
                    AutoSize = false,
                    Size = new Size(28, 18),
                    Location = new Point(rowW - 36, 8),
                    Text = count.ToString(),
                    Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                    ForeColor = isActive ? Color.White : Color.FromArgb(80, 80, 80),
                    BackColor = isActive ? Color.FromArgb(139, 0, 0) : Color.FromArgb(225, 225, 225),
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                AnnMakeRounded(badge, 9);

                row.Controls.Add(dot); row.Controls.Add(lbl); row.Controls.Add(badge);

                string captured = cat;
                EventHandler h = (s, ev) => { _activeCategoryFilter = captured; BuildAnnCategorySidebar(); RenderAnnouncements(); };
                row.Click += h; dot.Click += h; lbl.Click += h; badge.Click += h;

                flpCategories.Controls.Add(row);
            }

            flpCategories.ResumeLayout();
        }

        // ── Pinned sidebar ───────────────────────────────────────────────
        private void RenderAnnPinned()
        {
            if (flpPinned == null) return;

            flpPinned.SuspendLayout();
            flpPinned.Controls.Clear();
            flpPinned.BackColor = Color.White;
            var pinned = announcements.Where(a => a.IsPinned).OrderByDescending(a => a.Date).ToList();

            if (pinned.Count == 0)
            {
                flpPinned.Controls.Add(new System.Windows.Forms.Label
                {
                    Text = "No pinned announcements.",
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Padding = new Padding(4),
                });
                flpPinned.ResumeLayout(); return;
            }

            foreach (var a in pinned)
            {
                Color dotCol = AnnCatIconColor.GetValueOrDefault(a.Category, Color.Gray);
                int rowW = Math.Max(100, flpPinned.ClientSize.Width - 4);
                var row = new Panel
                {
                    Width = rowW,
                    Height = 52,
                    BackColor = Color.White,
                    Margin = new Padding(0, 2, 0, 2),
                    Cursor = Cursors.Hand
                };
                row.Paint += (s, pe) =>
                {
                    using var pen = new Pen(Color.FromArgb(230, 230, 230), 1f);
                    pe.Graphics.DrawRectangle(pen, 0, 0, row.Width - 1, row.Height - 1);
                };

                var dot = new Panel { Size = new Size(8, 8), Location = new Point(8, 22), BackColor = dotCol };
                AnnMakeCircle(dot);

                var rowTitle = new System.Windows.Forms.Label
                {
                    AutoSize = false,
                    Size = new Size(rowW - 28, 20),
                    Location = new Point(22, 8),
                    Text = a.Title,
                    Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(25, 25, 25),
                    AutoEllipsis = true,
                };
                row.Controls.Add(dot); row.Controls.Add(rowTitle);
                row.Controls.Add(new System.Windows.Forms.Label
                {
                    AutoSize = true,
                    Location = new Point(22, 28),
                    Text = a.Date.ToString("MMM d, yyyy"),
                    Font = new Font("Segoe UI", 7.5f),
                    ForeColor = Color.Gray,
                });

                var capturedAnn = a;
                EventHandler h = (s, ev) => { var found = announcements.Find(x => x.Id == capturedAnn.Id); if (found != null) ShowViewAnnouncementUC(found); };
                row.Click += h; rowTitle.Click += h;
                flpPinned.Controls.Add(row);
            }

            flpPinned.ResumeLayout();
        }

        // ── Insights panel ───────────────────────────────────────────────
        private void RenderAnnInsights()
        {
            if (pnlInsights == null) return;

            var old = pnlInsights.Controls.OfType<Control>().Where(c => c.Name.StartsWith("ins_")).ToList();
            foreach (var c in old) pnlInsights.Controls.Remove(c);
            pnlInsights.BackColor = Color.White;   // original white background

            int total = announcements.Count;
            int active = announcements.Count(a => a.Status == "active");
            int pinned = announcements.Count(a => a.IsPinned);
            int urgent = announcements.Count(a => a.IsUrgent);
            int notified = announcements.Count(a => a.NotifyStudents || a.NotifyInstructors);

            void AddRow(string label, string value, int y, Color valCol)
            {
                pnlInsights.Controls.Add(new System.Windows.Forms.Label
                {
                    Name = "ins_l" + y,
                    AutoSize = true,
                    Text = label,
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = Color.FromArgb(80, 80, 80),   // original muted dark label
                    Location = new Point(8, y)
                });
                pnlInsights.Controls.Add(new System.Windows.Forms.Label
                {
                    Name = "ins_v" + y,
                    AutoSize = true,
                    Text = value,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    ForeColor = valCol,
                    Location = new Point(pnlInsights.Width - 50, y - 2)
                });
            }

            AddRow("Total", total.ToString(), 30, Color.FromArgb(50, 50, 50));
            AddRow("Active", active.ToString(), 58, Color.ForestGreen);
            AddRow("Pinned", pinned.ToString(), 86, Color.DarkOrange);
            AddRow("Urgent", urgent.ToString(), 114, Color.Firebrick);
            AddRow("Notified", notified.ToString(), 142, Color.RoyalBlue);
            pnlInsights.Refresh();
        }

        // ── CRUD helpers ─────────────────────────────────────────────────
        private void EditAnnouncement(int id)
        {
            var a = announcements.Find(x => x.Id == id);
            if (a == null) return;
            editingAnnouncementId = id;
            _createAnnouncementUC.LoadForEdit(new AnnouncementDataAdmin
            {
                Title = a.Title,
                Description = a.Description,
                Category = a.Category,
                PostDate = a.Date,
                IsUrgent = a.IsUrgent,
                IsPinned = a.IsPinned,
                NotifyStudents = a.NotifyStudents,
                NotifyInstructors = a.NotifyInstructors,
                AttachmentPath = a.AttachedFile ?? string.Empty,
            });
            ShowCreateAnnouncementUC();
        }

        private void DeleteAnnouncement(int id)
        {
            announcements.RemoveAll(x => x.Id == id);
            RenderAnnouncements();
        }

        private void OnAnnouncementPosted(object sender, AnnouncementDataAdmin data)
        {
            if (editingAnnouncementId != -1)
            {
                var a = announcements.Find(x => x.Id == editingAnnouncementId);
                if (a != null)
                {
                    a.Title = data.Title;
                    a.Description = data.Description;
                    a.Category = data.Category;
                    a.Date = data.PostDate;
                    a.IsUrgent = data.IsUrgent;
                    a.IsPinned = data.IsPinned;
                    a.NotifyStudents = data.NotifyStudents;
                    a.NotifyInstructors = data.NotifyInstructors;
                    if (!string.IsNullOrEmpty(data.AttachmentPath))
                        a.AttachedFile = System.IO.Path.GetFileName(data.AttachmentPath);
                }
            }
            else
            {
                announcements.Insert(0, new AdminAnnouncement
                {
                    Id = DateTime.Now.Millisecond + new Random().Next(1000, 9999),
                    Title = data.Title,
                    Description = data.Description,
                    Category = data.Category,
                    Date = data.PostDate,
                    Status = "active",
                    IsUrgent = data.IsUrgent,
                    IsPinned = data.IsPinned,
                    NotifyStudents = data.NotifyStudents,
                    NotifyInstructors = data.NotifyInstructors,
                    AttachedFile = string.IsNullOrEmpty(data.AttachmentPath)
                                        ? null
                                        : System.IO.Path.GetFileName(data.AttachmentPath),
                    InstructorName = "Admin",
                    TotalStudents = 40,
                });
            }
            editingAnnouncementId = -1;
            RenderAnnouncements();
        }

        private void OnViewEdit(object sender, int id)
        {
            HideViewAnnouncementUC();
            EditAnnouncement(id);
        }

        // ── UC visibility helpers ────────────────────────────────────────
        private void ShowCreateAnnouncementUC()
        {
            CenterAnnControl(_createAnnouncementUC);
            _createAnnouncementUC.BringToFront();
            _createAnnouncementUC.Visible = true;
        }

        private void HideCreateAnnouncementUC()
        {
            _createAnnouncementUC.Visible = false;
            editingAnnouncementId = -1;
        }

        private void ShowViewAnnouncementUC(AdminAnnouncement a)
        {
            _viewAnnouncementUC.LoadAnnouncement(a);
            CenterAnnControl(_viewAnnouncementUC);
            _viewAnnouncementUC.BringToFront();
            _viewAnnouncementUC.Visible = true;
        }

        private void HideViewAnnouncementUC() => _viewAnnouncementUC.Visible = false;

        private void CenterAnnControl(Control child)
        {
            var parent = pnlAnnouncement;
            int maxW = Math.Max(200, parent.ClientSize.Width - 40);
            int maxH = Math.Max(100, parent.ClientSize.Height - 40);
            if (child.Width > maxW || child.Height > maxH)
            {
                float s = Math.Min((float)maxW / child.Width, (float)maxH / child.Height);
                child.Width = (int)(child.Width * s);
                child.Height = (int)(child.Height * s);
            }
            child.Location = new Point(
                Math.Max(0, (parent.ClientSize.Width - child.Width) / 2),
                Math.Max(0, (parent.ClientSize.Height - child.Height) / 4));
        }

        // ── Drawing helpers ───────────────────────────────────────────────
        private static void AnnMakeCircle(Control c)
        {
            var path = new GraphicsPath();
            path.AddEllipse(0, 0, c.Width, c.Height);
            c.Region = new Region(path);
        }

        private static void AnnMakeRounded(Control c, int r)
        {
            var path = new GraphicsPath();
            path.AddArc(0, 0, r, r, 180, 90);
            path.AddArc(c.Width - r, 0, r, r, 270, 90);
            path.AddArc(c.Width - r, c.Height - r, r, r, 0, 90);
            path.AddArc(0, c.Height - r, r, r, 90, 90);
            path.CloseFigure();
            c.Region = new Region(path);
        }
    }
}
