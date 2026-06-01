using PUPAcadPortal.PortalContents.Instructor.LMS.Course;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PUPAcadPortal
{
    public partial class AnnouncementContentInst : UserControl
    {
        private bool isEditing = false;
        private ActivityItem? currentEditingItem = null;
        private string tempAttachedPath = "";

        private List<Announcement> announcements = new List<Announcement>();
        private int editingAnnouncementId = -1;
        private string _activeCategoryFilter = "all";
        private string _activeStatusFilter = "all";
        private string _searchQuery = "";

        private CreateAnnouncement _createAnnouncementUC;
        private ViewAnnouncement _viewAnnouncementUC;
        private AnnouncementInbox _inboxUC;

        // (font-scaling fields removed – scaling caused overlay font corruption)

        private static readonly Dictionary<string, Color> CatIconColor = new()
        {
            ["General"] = Color.FromArgb(0x37, 0x8a, 0xdd),
            ["Academic"] = Color.FromArgb(0x63, 0x99, 0x22),
            ["Schedule"] = Color.FromArgb(0xba, 0x75, 0x17),
            ["Events"] = Color.FromArgb(0xd4, 0x53, 0x7e),
            ["Examinations"] = Color.FromArgb(0x7f, 0x77, 0xdd),
        };

        private static readonly Dictionary<string, Color> CatBgColor = new()
        {
            ["General"] = Color.FromArgb(0xe6, 0xf1, 0xfb),
            ["Academic"] = Color.FromArgb(0xea, 0xf3, 0xde),
            ["Schedule"] = Color.FromArgb(0xfa, 0xee, 0xda),
            ["Events"] = Color.FromArgb(0xfb, 0xea, 0xf0),
            ["Examinations"] = Color.FromArgb(0xee, 0xed, 0xfe),
        };

        public class Announcement
        {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Category { get; set; } = "General";
            public string Status { get; set; } = "active";
            public string InstructorName { get; set; } = "Prof. Santos";
            public DateTime Date { get; set; } = DateTime.Now;
            public bool IsPinned { get; set; }
            public bool IsUrgent { get; set; }
            public int ViewedCount { get; set; }
            public int TotalStudents { get; set; } = 40;
            public string? AttachedFile { get; set; }
        }

        public AnnouncementContentInst()
        {
            InitializeComponent();
        }

        private void AnnouncementContentInst_Load(object sender, EventArgs e)
        {
            cmbFilter.Items.Clear();
            cmbFilter.Items.AddRange(new object[] { "All announcements", "Active only", "Inactive only" });
            cmbFilter.SelectedIndex = 0;

            cmbSortBy.Items.Clear();
            cmbSortBy.Items.AddRange(new object[] { "Newest first", "Oldest first" });
            cmbSortBy.SelectedIndex = 0;

            textBox25.TextChanged += AnnSearch_Changed;
            cmbFilter.SelectedIndexChanged += AnnFilter_Changed;
            cmbSortBy.SelectedIndexChanged += AnnSort_Changed;

            SeedSampleAnnouncements();

            _createAnnouncementUC = new CreateAnnouncement
            {
                Visible = false,
                Anchor = AnchorStyles.None,
            };
            _createAnnouncementUC.AnnouncementPosted += OnAnnouncementPosted;
            _createAnnouncementUC.CloseRequested += (s, ev) => HideCreateAnnouncementUC();
            pnlAnnouncement.Controls.Add(_createAnnouncementUC);

            _viewAnnouncementUC = new ViewAnnouncement
            {
                Visible = false,
                Anchor = AnchorStyles.None,
            };
            _viewAnnouncementUC.EditRequested += OnViewEdit;
            _viewAnnouncementUC.DeleteRequested += OnViewDelete;
            _viewAnnouncementUC.CloseRequested += (s, ev) => HideViewAnnouncementUC();
            pnlAnnouncement.Controls.Add(_viewAnnouncementUC);

            // ── Inbox UC ────────────────────────────────────────────────────
            _inboxUC = new AnnouncementInbox
            {
                Visible = false,
                Anchor = AnchorStyles.None,
            };
            _inboxUC.CloseRequested += (s, ev) => _inboxUC.Visible = false;
            pnlAnnouncement.Controls.Add(_inboxUC);

            // Wire the Inbox button that lives in the designer (btnInbox)
            if (btnInbox != null)
                btnInbox.Click += (s, ev) => ShowInbox();

            BuildCategorySidebar();
            RenderAnnouncements();

            flpPinnedpnlInsights.AutoScroll = true;
            flpPinnedpnlInsights.WrapContents = false;
            flpPinnedpnlInsights.FlowDirection = FlowDirection.TopDown;

            flpPinned.AutoScroll = false;

            this.MinimumSize = new Size(1024, 700);
            AdjustLeftSidebarLayout();
        }

        private void AdjustLeftSidebarLayout()
        {
            Control sidebar = flpCategories.Parent;
            if (sidebar == null) return;

            Control? pinnedHeader = sidebar.Controls.Cast<Control>().FirstOrDefault(c => c.Text.Contains("Pinned"));
            Control? insightsHeader = sidebar.Controls.Cast<Control>().FirstOrDefault(c => c.Text.Contains("Insight"));

            int categoryItemsCount = 6;
            flpCategories.Height = (categoryItemsCount * 34) + ((categoryItemsCount - 1) * 4) + 8;

            flpCategories.AutoScroll = false;
            flpCategories.VerticalScroll.Enabled = false;
            flpCategories.VerticalScroll.Visible = false;

            {
                pinnedHeader.Top = flpCategories.Bottom + 20;
                flpPinnedpnlInsights.Top = pinnedHeader.Bottom + 8;
            }

            int totalSidebarHeight = sidebar.ClientSize.Height;

            int insightsSectionRequiredHeight = flpPinned.Height + 20 + 8 + 15;

            if (flpPinnedpnlInsights.Top > 0)
            {
                int availableSpaceForPinned = totalSidebarHeight - flpPinnedpnlInsights.Top - insightsSectionRequiredHeight;
                flpPinnedpnlInsights.Height = Math.Max(100, availableSpaceForPinned); // 100px minimum guardrail
            }

            if (insightsHeader != null)
            {
                insightsHeader.Top = flpPinnedpnlInsights.Bottom + 20;
                flpPinned.Top = insightsHeader.Bottom + 8;
            }
        }

        // ── Inbox show/hide ──────────────────────────────────────────────────
        private void ShowInbox()
        {
            CenterControl(_inboxUC, pnlAnnouncement);
            _inboxUC.BringToFront();
            _inboxUC.Visible = true;
        }

        private void AnnouncementContentInst_Resize(object sender, EventArgs e)
        {
            if (_createAnnouncementUC != null && _createAnnouncementUC.Visible)
                CenterControl(_createAnnouncementUC, pnlAnnouncement);
            if (_viewAnnouncementUC != null && _viewAnnouncementUC.Visible)
                CenterControl(_viewAnnouncementUC, pnlAnnouncement);
            if (_inboxUC != null && _inboxUC.Visible)
                CenterControl(_inboxUC, pnlAnnouncement);

            // Suspend layout to prevent visual flickering while resizing
            this.SuspendLayout();

            AdjustLeftSidebarLayout(); 

            RenderAnnouncements();
            BuildCategorySidebar();
            RenderPinnedAnnouncements();
            RenderAnnouncementInsights();

            this.ResumeLayout();
        }

        private void SeedSampleAnnouncements()
        {
            announcements.AddRange(new[]
            {
                new Announcement
                {
                    Id = 1, Title = "Midterm exam schedule released",
                    Description   = "The official midterm examination schedule for all BSIT 2nd year subjects has been posted. Please check your respective rooms and ensure you bring your student IDs.",
                    Category      = "Examinations", Status = "active", IsPinned = true, IsUrgent = true,
                    ViewedCount   = 28, TotalStudents = 40, Date = DateTime.Now,
                },
                new Announcement
                {
                    Id = 2, Title = "Class suspension – May 12",
                    Description   = "All classes on Monday, May 12 are suspended due to the declared public holiday. Make-up sessions will be announced separately by each faculty member.",
                    Category      = "Schedule", Status = "active",
                    ViewedCount   = 35, TotalStudents = 40, Date = DateTime.Now.AddHours(-5),
                },
                new Announcement
                {
                    Id = 3, Title = "Programming 1 – lab activity this Friday",
                    Description   = "Bring your laptops for the graded lab activity covering Modules 4 and 5. The activity will be conducted using Visual Studio 2022. No borrowing of equipment.",
                    Category      = "Academic", Status = "active",
                    ViewedCount   = 22, TotalStudents = 40, Date = DateTime.Now.AddDays(-2),
                },
                new Announcement
                {
                    Id = 4, Title = "Campus foundation day celebration",
                    Description   = "Join us for the PUP Foundation Day celebration on May 17. Activities include a student showcase, cultural performances, and a technology exhibit.",
                    Category      = "Events", Status = "inactive",
                    ViewedCount   = 18, TotalStudents = 40, Date = DateTime.Now.AddDays(-4),
                },
                new Announcement
                {
                    Id = 5, Title = "Reminder: submit assignment outputs",
                    Description   = "All pending assignment outputs for Information Management must be submitted via the LMS before May 15, 11:59 PM. Late submissions will not be accepted.",
                    Category      = "General", Status = "active",
                    ViewedCount   = 30, TotalStudents = 40, Date = DateTime.Now.AddDays(-5),
                },
                new Announcement
                {
                    Id = 6, Title = "Final Exam Coverage – Programming 1",
                    Description   = "The official final examination coverage for Introduction to Programming 1 has been posted on the LMS. The exam will cover Modules 1 through 8, with emphasis on loops, arrays, and object-oriented concepts. Review your past quizzes and lab activities as preparation.",
                    Category      = "Examinations", Status = "active", IsPinned = true,
                    ViewedCount   = 32, TotalStudents = 40, Date = DateTime.Now.AddHours(-2),
                },
                new Announcement
                {
                    Id = 7, Title = "Graded Recitation – Information Management",
                    Description   = "There will be a graded recitation next Wednesday covering database normalization and SQL joins. Come prepared with your notes. Recitation will be conducted individually and will account for 10% of your class standing.",
                    Category      = "Academic", Status = "active",
                    ViewedCount   = 15, TotalStudents = 40, Date = DateTime.Now.AddDays(-1),
                },
                new Announcement
                {
                    Id = 8, Title = "Room Change – PATHFIT 4 Classes",
                    Description   = "Effective May 13, 2026, all PATHFIT 4 classes will be moved from the covered court to the university gymnasium due to ongoing roof repairs. Please take note of this change and inform your classmates accordingly.",
                    Category      = "Schedule", Status = "active",
                    ViewedCount   = 38, TotalStudents = 40, Date = DateTime.Now.AddDays(-1).AddHours(-3),
                },
                new Announcement
                {
                    Id = 9, Title = "IT Career Fair – Volunteer Marshals Needed",
                    Description   = "The Career Services Office is looking for 15 student volunteers to serve as event marshals during the IT Career Fair on May 22. Volunteers will receive a certificate of appreciation and priority access to company booths. Interested students may sign up through the Student Affairs Office.",
                    Category      = "Events", Status = "active",
                    ViewedCount   = 20, TotalStudents = 40, Date = DateTime.Now.AddDays(-2).AddHours(-1),
                },
                new Announcement
                {
                    Id = 10, Title = "Capstone Group Submissions – Revised Deadline",
                    Description   = "The submission deadline for Capstone Project Chapter 3 has been moved to May 19, 2026 at 11:59 PM due to the university-wide system maintenance last week. All groups must submit their documents through the LMS. No hard copy will be accepted for this submission.",
                    Category      = "Academic", Status = "active", IsPinned = true, IsUrgent = true,
                    ViewedCount   = 36, TotalStudents = 40, Date = DateTime.Now.AddDays(-3),
                },
                new Announcement
                {
                    Id = 11, Title = "Classroom Conduct Reminder",
                    Description   = "Students are reminded to observe proper classroom conduct at all times. Mobile phones must be on silent mode during lectures. Eating inside the air-conditioned classrooms is strictly prohibited. Repeated violations will be noted in the class record.",
                    Category      = "General", Status = "active",
                    ViewedCount   = 25, TotalStudents = 40, Date = DateTime.Now.AddDays(-4).AddHours(-2),
                },
                new Announcement
                {
                    Id = 12, Title = "Make-Up Class – Human Computer Interaction",
                    Description   = "A make-up class for Human Computer Interaction will be held on Saturday, May 17, 2026 from 8:00 AM to 10:00 AM at Room 305, CCIS Building. Attendance is required. This session will cover the topics missed during the class suspension on May 12.",
                    Category      = "Schedule", Status = "active",
                    ViewedCount   = 29, TotalStudents = 40, Date = DateTime.Now.AddDays(-5).AddHours(-1),
                },
                new Announcement
                {
                    Id = 13, Title = "Department Research Colloquium – May 21",
                    Description   = "The CCIS Department will be holding its annual Research Colloquium on May 21, 2026 from 1:00 PM to 5:00 PM at the Audio-Visual Room. All 4th-year students are required to attend and present a brief summary of their capstone projects. 2nd and 3rd year students are encouraged to participate.",
                    Category      = "Events", Status = "active",
                    ViewedCount   = 12, TotalStudents = 40, Date = DateTime.Now.AddDays(-6),
                },
                new Announcement
                {
                    Id = 14, Title = "Quiz 3 – Principles of Accounting",
                    Description   = "Quiz 3 for Principles of Accounting will be administered on May 16, 2026 during the regular class schedule. Coverage includes Chapters 7 to 9: Accounts Receivable, Inventory Valuation, and Depreciation Methods. Open notes will NOT be allowed. A scientific calculator may be used.",
                    Category      = "Examinations", Status = "inactive",
                    ViewedCount   = 10, TotalStudents = 40, Date = DateTime.Now.AddDays(-7),
                },
                new Announcement
                {
                    Id = 15, Title = "LMS Downtime – May 14 Maintenance Window",
                    Description   = "The university Learning Management System will be unavailable on May 14, 2026 from 12:00 AM to 6:00 AM for scheduled maintenance and security updates. All assignment deadlines falling within this window have been automatically extended by 6 hours. Please plan your submissions accordingly.",
                    Category      = "General", Status = "active",
                    ViewedCount   = 40, TotalStudents = 40, Date = DateTime.Now.AddDays(-8),
                },
            });
        }

        private void BuildCategorySidebar()
        {
            flpCategories.SuspendLayout();
            flpCategories.Controls.Clear();
            flpCategories.FlowDirection = FlowDirection.TopDown;
            flpCategories.WrapContents = false;
            flpCategories.AutoScroll = true;
            flpCategories.HorizontalScroll.Enabled = false;
            flpCategories.HorizontalScroll.Visible = false;
            flpCategories.VerticalScroll.Visible = true;
            flpCategories.BackColor = Color.White;

            var categories = new[] { "all", "General", "Academic", "Schedule", "Events", "Examinations" };

            var dotColors = new Dictionary<string, Color>
            {
                ["all"] = Color.FromArgb(139, 0, 0),
                ["General"] = CatIconColor["General"],
                ["Academic"] = CatIconColor["Academic"],
                ["Schedule"] = CatIconColor["Schedule"],
                ["Events"] = CatIconColor["Events"],
                ["Examinations"] = CatIconColor["Examinations"],
            };

            foreach (var cat in categories)
            {
                int count = cat == "all" ? announcements.Count : announcements.Count(a => a.Category == cat);
                bool isActive = _activeCategoryFilter == cat;

                var row = new Panel
                {
                    Width = flpCategories.ClientSize.Width - 25,
                    Height = 34,
                    Margin = new Padding(0, 0, 0, 4),
                    BackColor = isActive ? Color.FromArgb(245, 238, 238) : Color.Transparent,
                    Cursor = Cursors.Hand,
                    Tag = cat,
                };

                var dot = new Panel
                {
                    Size = new Size(9, 9),
                    BackColor = dotColors.GetValueOrDefault(cat, Color.Gray),
                    Location = new Point(10, 13),
                    Tag = cat,
                };
                MakeCircleRegion(dot);

                var lbl = new Label
                {
                    AutoSize = false,
                    Size = new Size(row.Width - 60, 34),
                    Location = new Point(26, 0),
                    Text = cat == "all" ? "All" : cat,
                    Font = new Font("Segoe UI", 9.5f, isActive ? FontStyle.Bold : FontStyle.Regular),
                    ForeColor = Color.FromArgb(40, 40, 40),
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.Transparent,
                    Tag = cat,
                };

                var badge = new Label
                {
                    AutoSize = false,
                    Size = new Size(28, 18),
                    Location = new Point(row.Width - 36, 8),
                    Text = count.ToString(),
                    Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                    ForeColor = isActive ? Color.White : Color.FromArgb(80, 80, 80),
                    BackColor = isActive ? Color.FromArgb(139, 0, 0) : Color.FromArgb(225, 225, 225),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Tag = cat,
                };
                MakeRoundedRegion(badge, 9);

                row.Controls.Add(dot);
                row.Controls.Add(lbl);
                row.Controls.Add(badge);

                EventHandler handler = (s, ev) => CategoryFilter_Click(cat);
                row.Click += handler;
                dot.Click += handler;
                lbl.Click += handler;
                badge.Click += handler;

                flpCategories.Controls.Add(row);
            }

            flpCategories.HorizontalScroll.Maximum = 0;
            flpCategories.AutoScrollPosition = new Point(0, 0);
            flpCategories.ResumeLayout();
        }

        private void CategoryFilter_Click(string category)
        {
            _activeCategoryFilter = category;
            BuildCategorySidebar();
            RenderAnnouncements();
        }

        private void AnnSearch_Changed(object sender, EventArgs e)
        {
            _searchQuery = textBox25.Text.Trim();
            RenderAnnouncements();
        }

        private void AnnFilter_Changed(object sender, EventArgs e)
        {
            _activeStatusFilter = cmbFilter.SelectedItem?.ToString() switch
            {
                "Active only" => "active",
                "Inactive only" => "inactive",
                _ => "all",
            };
            RenderAnnouncements();
        }

        private void AnnSort_Changed(object sender, EventArgs e) => RenderAnnouncements();

        private void RenderAnnouncements()
        {
            flowLayoutPanelAnnouncements.SuspendLayout();
            flowLayoutPanelAnnouncements.Controls.Clear();
            flowLayoutPanelAnnouncements.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelAnnouncements.WrapContents = false;
            flowLayoutPanelAnnouncements.AutoScroll = true;
            flowLayoutPanelAnnouncements.HorizontalScroll.Enabled = false;
            flowLayoutPanelAnnouncements.HorizontalScroll.Visible = false;
            flowLayoutPanelAnnouncements.VerticalScroll.Visible = true;

            var filtered = announcements.AsEnumerable();

            if (_activeCategoryFilter != "all")
                filtered = filtered.Where(a => a.Category == _activeCategoryFilter);

            if (_activeStatusFilter == "active")
                filtered = filtered.Where(a => a.Status == "active");
            else if (_activeStatusFilter == "inactive")
                filtered = filtered.Where(a => a.Status != "active");

            if (!string.IsNullOrWhiteSpace(_searchQuery))
            {
                var q = _searchQuery.ToLower();
                filtered = filtered.Where(a =>
                    a.Title.ToLower().Contains(q) ||
                    a.Description.ToLower().Contains(q));
            }

            bool oldestFirst = cmbSortBy.SelectedItem?.ToString() == "Oldest first";

            var sorted = oldestFirst
                ? filtered.OrderBy(a => a.IsPinned ? 0 : 1).ThenBy(a => a.Date).ToList()
                : filtered.OrderBy(a => a.IsPinned ? 0 : 1).ThenByDescending(a => a.Date).ToList();

            int panelWidth = Math.Max(300, flowLayoutPanelAnnouncements.ClientSize.Width - 30);

            var newThreshold = TimeSpan.FromDays(1);

            foreach (var a in sorted)
            {
                var card = BuildAnnouncementCard(a, panelWidth, newThreshold);
                card.Margin = new Padding(6, 4, 6, 4);
                flowLayoutPanelAnnouncements.Controls.Add(card);
            }

            int showing = sorted.Count;
            lblShowing.Text = showing == 0
                ? "No announcements found"
                : $"Showing 1–{showing} of {showing} announcements";

            flowLayoutPanelAnnouncements.HorizontalScroll.Maximum = 0;
            flowLayoutPanelAnnouncements.AutoScrollPosition = new Point(0, 0);
            flowLayoutPanelAnnouncements.ResumeLayout();

            BuildCategorySidebar();
            RenderPinnedAnnouncements();
            RenderAnnouncementInsights();
        }

        private AnnouncementLayout BuildAnnouncementCard(
            Announcement a, int panelWidth, TimeSpan newThreshold)
        {
            var card = new AnnouncementLayout();

            card.LoadInstructor(
                id: a.Id,
                title: a.Title,
                description: a.Description,
                category: a.Category,
                status: a.Status,
                instructorName: a.InstructorName,
                date: a.Date,
                isPinned: a.IsPinned,
                isUrgent: a.IsUrgent,
                viewedCount: a.ViewedCount,
                totalStudents: a.TotalStudents,
                cardWidth: panelWidth);

            card.CardClicked += (s, id) =>
            {
                var ann = announcements.Find(x => x.Id == id);
                if (ann != null) ShowViewAnnouncementUC(ann);
            };
            card.PinToggled += (s, id) =>
            {
                var ann = announcements.Find(x => x.Id == id);
                if (ann != null)
                {
                    ann.IsPinned = !ann.IsPinned;
                    RenderAnnouncements();
                }
            };
            card.MenuEditClicked += (s, id) => EditAnnouncement(id);
            card.MenuToggleClicked += (s, id) => ToggleAnnouncement(id);
            card.MenuDeleteClicked += (s, id) => DeleteAnnouncement(id);

            return card;
        }

        private void ShowViewAnnouncementUC(Announcement a)
        {
            _viewAnnouncementUC.LoadAnnouncement(a);
            CenterControl(_viewAnnouncementUC, pnlAnnouncement);
            _viewAnnouncementUC.BringToFront();
            _viewAnnouncementUC.Visible = true;
        }

        private void HideViewAnnouncementUC() => _viewAnnouncementUC.Visible = false;

        private void OnViewEdit(object sender, int id)
        {
            HideViewAnnouncementUC();
            EditAnnouncement(id);
        }

        private void OnViewDelete(object sender, int id) => DeleteAnnouncement(id);

        private void OnAnnouncementPosted(object sender, AnnouncementData data)
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
                }
            }
            else
            {
                announcements.Insert(0, new Announcement
                {
                    Id = DateTime.Now.Millisecond + new Random().Next(1000, 9999),
                    Title = data.Title,
                    Description = data.Description,
                    Category = data.Category,
                    Date = data.PostDate,
                    Status = "active",
                    IsUrgent = data.IsUrgent,
                    IsPinned = data.IsPinned,
                    InstructorName = "Prof. Santos",
                    TotalStudents = 40,
                });
            }

            editingAnnouncementId = -1;
            RenderAnnouncements();
        }

        private void EditAnnouncement(int id)
        {
            var a = announcements.Find(x => x.Id == id);
            if (a == null) return;

            editingAnnouncementId = id;
            var data = new AnnouncementData
            {
                Title = a.Title,
                Description = a.Description,
                Category = a.Category,
                PostDate = a.Date,
                IsUrgent = a.IsUrgent,
                IsPinned = a.IsPinned,
            };
            _createAnnouncementUC.LoadForEdit(data);
            ShowCreateAnnouncementUC();
        }

        private void ToggleAnnouncement(int id)
        {
            var a = announcements.Find(x => x.Id == id);
            if (a != null) a.Status = a.Status == "active" ? "inactive" : "active";
            RenderAnnouncements();
        }

        private void DeleteAnnouncement(int id)
        {
            announcements.RemoveAll(x => x.Id == id);
            RenderAnnouncements();
        }

        private void btnCreateAnnouncement_Click(object sender, EventArgs e)
        {
            editingAnnouncementId = -1;
            _createAnnouncementUC.LoadForEdit(new AnnouncementData());
            ShowCreateAnnouncementUC();
        }

        private void ShowCreateAnnouncementUC()
        {
            CenterControl(_createAnnouncementUC, pnlAnnouncement);
            _createAnnouncementUC.BringToFront();
            _createAnnouncementUC.Visible = true;
        }

        private void HideCreateAnnouncementUC()
        {
            _createAnnouncementUC.Visible = false;
            editingAnnouncementId = -1;
        }

        private static void CenterControl(Control child, Control parent)
        {
            int maxW = Math.Max(200, parent.ClientSize.Width - 40);
            int maxH = Math.Max(100, parent.ClientSize.Height - 40);
            if (child.Width > maxW || child.Height > maxH)
            {
                float s = Math.Min((float)maxW / child.Width, (float)maxH / child.Height);
                child.Width = (int)(child.Width * s);
                child.Height = (int)(child.Height * s);
            }
            int x = (parent.ClientSize.Width - child.Width) / 2;
            int y = (parent.ClientSize.Height - child.Height) / 4;
            child.Location = new Point(Math.Max(0, x), Math.Max(0, y));
        }

        private void RenderPinnedAnnouncements()
        {
            flpPinnedpnlInsights.SuspendLayout();
            flpPinnedpnlInsights.Controls.Clear();
            flpPinnedpnlInsights.FlowDirection = FlowDirection.TopDown;
            flpPinnedpnlInsights.WrapContents = false;
            flpPinnedpnlInsights.AutoScroll = false;
            flpPinnedpnlInsights.HorizontalScroll.Enabled = false;
            flpPinnedpnlInsights.HorizontalScroll.Visible = false;
            flpPinnedpnlInsights.AutoScroll = true;
            flpPinnedpnlInsights.BackColor = Color.White;

            var pinnedList = announcements
                .Where(a => a.IsPinned)
                .OrderByDescending(a => a.Date)
                .ToList();

            if (pinnedList.Count == 0)
            {
                flpPinnedpnlInsights.Controls.Add(new Label
                {
                    Text = "No pinned announcements.",
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Padding = new Padding(4),
                });
                flpPinnedpnlInsights.ResumeLayout();
                return;
            }

            foreach (var a in pinnedList)
            {
                Color dotColor = CatIconColor.GetValueOrDefault(a.Category, Color.Gray);
                int rowW = Math.Max(100, flpPinnedpnlInsights.ClientSize.Width - 4);

                var row = new Panel
                {
                    Width = rowW,
                    Height = 52,
                    BackColor = Color.White,
                    Margin = new Padding(0, 2, 0, 2),
                    Cursor = Cursors.Hand,
                    Tag = a,
                };
                row.Paint += (s, pe) =>
                {
                    using var pen = new Pen(Color.FromArgb(230, 230, 230), 1f);
                    pe.Graphics.DrawRectangle(pen, 0, 0, row.Width - 1, row.Height - 1);
                };

                var dot = new Panel
                {
                    Size = new Size(8, 8),
                    Location = new Point(8, 22),
                    BackColor = dotColor,
                };
                MakeCircleRegion(dot);
                row.Controls.Add(dot);

                var rowTitle = new Label
                {
                    AutoSize = false,
                    Size = new Size(rowW - 28, 20),
                    Location = new Point(22, 8),
                    Text = a.Title,
                    Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(25, 25, 25),
                    AutoEllipsis = true,
                };
                row.Controls.Add(rowTitle);

                row.Controls.Add(new Label
                {
                    AutoSize = true,
                    Location = new Point(22, 28),
                    Text = a.Date.ToString("MMM d, yyyy"),
                    Font = new Font("Segoe UI", 7.5f),
                    ForeColor = Color.Gray,
                });

                EventHandler openHandler = (s, ev) =>
                {
                    var found = announcements.Find(x => x.Id == a.Id);
                    if (found != null) ShowViewAnnouncementUC(found);
                };
                row.Click += openHandler;
                rowTitle.Click += openHandler;

                flpPinnedpnlInsights.Controls.Add(row);
            }

            flpPinnedpnlInsights.ResumeLayout();
        }

        private void RenderAnnouncementInsights()
        {
            var toRemove = flpPinned.Controls
                .OfType<Control>()
                .Where(c => c.Name.StartsWith("ins_"))
                .ToList();
            foreach (var c in toRemove) flpPinned.Controls.Remove(c);

            flpPinned.BackColor = Color.White;
            flpPinned.AutoScroll = false;

            int total = announcements.Count;
            int active = announcements.Count(a => a.Status == "active");
            int pinned = announcements.Count(a => a.IsPinned);
            int urgent = announcements.Count(a => a.IsUrgent);
            int totalViews = announcements.Sum(a => a.ViewedCount);
            int totalStudents = announcements.Sum(a => a.TotalStudents);
            int reach = totalStudents > 0 ? (int)((double)totalViews / totalStudents * 100) : 0;

            void AddRow(string label, string value, int y, Color col)
            {
                flpPinned.Controls.Add(new Label
                {
                    Name = "ins_lbl_" + y,
                    AutoSize = true,
                    Text = label,
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = Color.FromArgb(80, 80, 80),
                    Location = new Point(8, y),
                });
                flpPinned.Controls.Add(new Label
                {
                    Name = "ins_val_" + y,
                    AutoSize = true,
                    Text = value,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    ForeColor = col,
                    Location = new Point(flpPinned.Width - 50, y - 2),
                });
            }

            AddRow("Total Announcements", total.ToString(), 30, Color.FromArgb(50, 50, 50));
            AddRow("Active", active.ToString(), 58, Color.ForestGreen);
            AddRow("Pinned", pinned.ToString(), 86, Color.DarkOrange);
            AddRow("Urgent", urgent.ToString(), 114, Color.Firebrick);
            AddRow("Avg. Reach", reach + "%", 142, Color.RoyalBlue);

            flpPinned.Refresh();
        }

        private static GraphicsPath RoundedRectPath(Rectangle r, int rad)
        {
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, rad, rad, 180, 90);
            p.AddArc(r.Right - rad, r.Y, rad, rad, 270, 90);
            p.AddArc(r.Right - rad, r.Bottom - rad, rad, rad, 0, 90);
            p.AddArc(r.X, r.Bottom - rad, rad, rad, 90, 90);
            p.CloseFigure();
            return p;
        }

        private static void MakeRoundedRegion(Label lbl, int radius)
        {
            using var path = RoundedRectPath(new Rectangle(0, 0, lbl.Width, lbl.Height), radius);
            lbl.Region = new Region(path);
        }

        private static void MakeCircleRegion(Panel p)
        {
            var path = new GraphicsPath();
            path.AddEllipse(0, 0, p.Width, p.Height);
            p.Region = new Region(path);
        }

        private void panelLeft_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}