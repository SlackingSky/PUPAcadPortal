using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Models;
using PUPAcadPortal.PortalContents.Instructor.LMS.Course;
using PUPAcadPortal.PortalContents.Misc.LMS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

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

            public bool NotifyStudents { get; set; } = false;
            public bool NotifyInstructors { get; set; } = false;
        }

        public AnnouncementContentInst()
        {
            InitializeComponent();

            SeedSampleAnnouncements();

            RenderAnnouncements();
        }

        //  Load 
        private void AnnouncementContentInst_Load(object sender, EventArgs e)
        {
            cmbFilter.Items.AddRange(new object[] { "All announcements", "Active only", "Inactive only" });
            cmbFilter.SelectedIndex = 0;
            cmbSortBy.Items.AddRange(new object[] { "Newest first", "Oldest first" });
            cmbSortBy.SelectedIndex = 0;

            textBox25.TextChanged += AnnSearch_Changed;
            cmbFilter.SelectedIndexChanged += AnnFilter_Changed;
            cmbSortBy.SelectedIndexChanged += AnnSort_Changed;

            SeedSampleAnnouncements();

            // Create Announcement overlay
            _createAnnouncementUC = new CreateAnnouncement { Visible = false, Anchor = AnchorStyles.None };
            _createAnnouncementUC.AnnouncementPosted += OnAnnouncementPosted;
            _createAnnouncementUC.CloseRequested += (s, ev) => HideCreateAnnouncementUC();
            pnlAnnouncement.Controls.Add(_createAnnouncementUC);

            // View Announcement overlay
            _viewAnnouncementUC = new ViewAnnouncement { Visible = false, Anchor = AnchorStyles.None };
            _viewAnnouncementUC.EditRequested += OnViewEdit;
            _viewAnnouncementUC.DeleteRequested += OnViewDelete;
            _viewAnnouncementUC.CloseRequested += (s, ev) => HideViewAnnouncementUC();
            pnlAnnouncement.Controls.Add(_viewAnnouncementUC);

            // Inbox overlay
            _inboxUC = new AnnouncementInbox { Visible = false, Anchor = AnchorStyles.None };
            _inboxUC.CloseRequested += (s, ev) => _inboxUC.Visible = false;
            pnlAnnouncement.Controls.Add(_inboxUC);

            if (btnInbox != null)
                btnInbox.Click += (s, ev) => ShowInbox();

            this.MinimumSize = new Size(1024, 700);
            RenderAnnouncements();        // calls Build* and Render* internally
            SyncMiddleRowBoxes(rebuildContent: true);   // sync sizes then rebuild with correct widths
        }

        private void SyncMiddleRowBoxes(bool rebuildContent = false)
        {
            if (pnlMiddleRow == null) return;

            int w = pnlMiddleRow.ClientSize.Width;
            int h = pnlMiddleRow.ClientSize.Height;

            if (w < 10) return;
            if (h < 10) h = 260;

            const int gap = 4;
            int catW = (w - gap) / 2;
            int insW = w - catW - gap;

            // Size both boxes side by side filling pnlMiddleRow
            pnlCatBox.SetBounds(0, 0, catW, h);
            pnlInsightBox.SetBounds(catW + gap, 0, insW, h);

            // flpPinned sits below lblInsightsTitle; fill remaining height
            int titleH = lblInsightsTitle?.Height > 0 ? lblInsightsTitle.Height : 40;
            flpPinned.SetBounds(0, titleH, insW - 2, h - titleH - 2);

            // Rebuild rows so widths match the new panel sizes (load only)
            if (rebuildContent)
            {
                BuildCategorySidebar();
                RenderAnnouncementInsights();
            }
        }

        //  Resize 
        private void AnnouncementContentInst_Resize(object sender, EventArgs e)
        {
            if (_createAnnouncementUC?.Visible == true) CenterControl(_createAnnouncementUC, pnlAnnouncement);
            if (_viewAnnouncementUC?.Visible == true) CenterControl(_viewAnnouncementUC, pnlAnnouncement);
            if (_inboxUC?.Visible == true) CenterControl(_inboxUC, pnlAnnouncement);

            this.SuspendLayout();
            RenderAnnouncements();      // rebuilds cards + sidebar content
            this.ResumeLayout(true);

            // Sync box sizes after layout; RenderAnnouncements already rebuilt the rows
            SyncMiddleRowBoxes(rebuildContent: false);
        }

        //  Inbox 
        private void ShowInbox()
        {
            CenterControl(_inboxUC, pnlAnnouncement);
            _inboxUC.BringToFront();
            _inboxUC.Visible = true;
        }

        //  Seed data 


        private void SeedSampleAnnouncements()
        {
            announcements.Clear();

            using (var context = new AppDbContext())
            {
                var dbAnnouncements = context.Announcements.ToList();

                foreach (var ann in dbAnnouncements)
                {
                    announcements.Add(new Announcement
                    {
                        Id = ann.AnnouncementId,

                        Title = ann.Title,

                        Description = ann.Content,

                        Category = ann.Category,

                        Status = "active",

                        IsPinned = ann.IsPinned,

                        IsUrgent = ann.IsUrgent,

                        AttachedFile = ann.AttachedFile,

                        Date = ann.PostedDate,

                        ViewedCount = 0,

                        TotalStudents = 0,

                        NotifyStudents = false,

                        NotifyInstructors = false
                    });
                }
            }
        }

        //  Category sidebar 
        private void BuildCategorySidebar()
        {
            flpCategories.SuspendLayout();
            flpCategories.Controls.Clear();
            flpCategories.FlowDirection = FlowDirection.TopDown;
            flpCategories.WrapContents = false;
            flpCategories.AutoScroll = false;           // no horizontal bar
            flpCategories.HorizontalScroll.Enabled = false;
            flpCategories.HorizontalScroll.Visible = false;
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

            // Row must be narrow enough to never exceed the panel's client width.
            // Subtract 2 px so the row never triggers a horizontal scrollbar.
            int rowW = Math.Max(60, flpCategories.ClientSize.Width - 2);

            // Badge constants — keep them consistent and away from the right edge
            const int BADGE_W = 22;
            const int BADGE_H = 14;
            const int BADGE_RIGHT = 4;   // gap from right edge of row

            foreach (var cat in categories)
            {
                int count = cat == "all" ? announcements.Count : announcements.Count(a => a.Category == cat);
                bool isActive = _activeCategoryFilter == cat;

                var row = new Panel
                {
                    Width = rowW,
                    Height = 28,
                    Margin = new Padding(0),
                    BackColor = isActive ? Color.FromArgb(245, 238, 238) : Color.Transparent,
                    Cursor = Cursors.Hand,
                    Tag = cat,
                };

                var dot = new Panel
                {
                    Size = new Size(7, 7),
                    Location = new Point(7, 11),
                    BackColor = dotColors.GetValueOrDefault(cat, Color.Gray),
                    Tag = cat,
                };
                MakeCircleRegion(dot);

                // Label takes space between dot and badge
                var lbl = new Label
                {
                    AutoSize = false,
                    Size = new Size(rowW - 20 - BADGE_W - BADGE_RIGHT - 4, 28),
                    Location = new Point(20, 0),
                    Text = cat == "all" ? "All" : cat,
                    Font = new Font("Segoe UI", 8f, isActive ? FontStyle.Bold : FontStyle.Regular),
                    ForeColor = Color.FromArgb(40, 40, 40),
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.Transparent,
                    Tag = cat,
                };

                var badge = new Label
                {
                    AutoSize = false,
                    Size = new Size(BADGE_W, BADGE_H),
                    Location = new Point(rowW - BADGE_W - BADGE_RIGHT, (28 - BADGE_H) / 2),
                    Text = count.ToString(),
                    Font = new Font("Segoe UI", 7f, FontStyle.Bold),
                    ForeColor = isActive ? Color.White : Color.FromArgb(80, 80, 80),
                    BackColor = isActive ? Color.FromArgb(139, 0, 0) : Color.FromArgb(220, 220, 220),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Tag = cat,
                };
                MakeRoundedRegion(badge, 7);

                row.Controls.Add(dot);
                row.Controls.Add(lbl);
                row.Controls.Add(badge);

                EventHandler handler = (s, ev) => CategoryFilter_Click(cat);
                row.Click += handler;
                dot.Click += handler;
                lbl.Click += handler;
                badge.Click += handler;

                // Bottom divider line
                row.Paint += (s, pe) =>
                {
                    using var pen = new Pen(Color.FromArgb(240, 240, 240));
                    pe.Graphics.DrawLine(pen, 0, row.Height - 1, row.Width, row.Height - 1);
                };

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

        //  Main feed render 
        private void RenderAnnouncements()
        {
            flowLayoutPanelAnnouncements.SuspendLayout();
            flowLayoutPanelAnnouncements.Controls.Clear();
            flowLayoutPanelAnnouncements.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelAnnouncements.WrapContents = false;
            flowLayoutPanelAnnouncements.AutoScroll = true;
            flowLayoutPanelAnnouncements.HorizontalScroll.Enabled = false;
            flowLayoutPanelAnnouncements.HorizontalScroll.Visible = false;

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
                    a.Title.ToLower().Contains(q) || a.Description.ToLower().Contains(q));
            }

            bool oldestFirst = cmbSortBy.SelectedItem?.ToString() == "Oldest first";
            var sorted = oldestFirst
                ? filtered.OrderBy(a => a.IsPinned ? 0 : 1).ThenBy(a => a.Date).ToList()
                : filtered.OrderBy(a => a.IsPinned ? 0 : 1).ThenByDescending(a => a.Date).ToList();

            int panelWidth = Math.Max(300, flowLayoutPanelAnnouncements.ClientSize.Width - 30);

            foreach (var a in sorted)
            {
                var card = BuildAnnouncementCard(a, panelWidth);
                card.Margin = new Padding(6, 4, 6, 4);
                flowLayoutPanelAnnouncements.Controls.Add(card);
            }

            lblShowing.Text = sorted.Count == 0
                ? "No announcements found"
                : $"Showing 1–{sorted.Count} of {sorted.Count} announcements";

            flowLayoutPanelAnnouncements.HorizontalScroll.Maximum = 0;
            flowLayoutPanelAnnouncements.AutoScrollPosition = new Point(0, 0);
            flowLayoutPanelAnnouncements.ResumeLayout();

            // Always refresh sidebar widgets when feed refreshes
            BuildCategorySidebar();
            RenderPinnedAnnouncements();
            RenderAnnouncementInsights();
        }

        private AnnouncementLayout BuildAnnouncementCard(Announcement a, int panelWidth)
        {
            var card = new AnnouncementLayout();
            card.LoadInstructor(
                id: a.Id, title: a.Title, description: a.Description, category: a.Category,
                status: a.Status, instructorName: a.InstructorName, date: a.Date,
                isPinned: a.IsPinned, isUrgent: a.IsUrgent,
                viewedCount: a.ViewedCount, totalStudents: a.TotalStudents, cardWidth: panelWidth);

            card.CardClicked += (s, id) =>
            {
                var ann = announcements.Find(x => x.Id == id);
                if (ann != null) ShowViewAnnouncementUC(ann);
            };
            card.PinToggled += (s, id) =>
            {
                var ann = announcements.Find(x => x.Id == id);
                if (ann != null) { ann.IsPinned = !ann.IsPinned; RenderAnnouncements(); }
            };
            card.MenuEditClicked += (s, id) => EditAnnouncement(id);
            card.MenuToggleClicked += (s, id) => ToggleAnnouncement(id);
            card.MenuDeleteClicked += (s, id) => DeleteAnnouncement(id);
            return card;
        }

        //  View / Edit / Delete helpers 
        private void ShowViewAnnouncementUC(Announcement a)
        {
            _viewAnnouncementUC.LoadAnnouncement(a);
            CenterControl(_viewAnnouncementUC, pnlAnnouncement);
            _viewAnnouncementUC.BringToFront();
            _viewAnnouncementUC.Visible = true;
        }

        private void HideViewAnnouncementUC() => _viewAnnouncementUC.Visible = false;

        private void OnViewEdit(object sender, int id) { HideViewAnnouncementUC(); EditAnnouncement(id); }
        private void OnViewDelete(object sender, int id) => DeleteAnnouncement(id);

        private void OnAnnouncementPosted(object sender, AnnouncementData data)
        {
            if (editingAnnouncementId != -1)
            {
                var a = announcements.Find(x => x.Id == editingAnnouncementId);
                if (a != null)
                {
                    a.Title = data.Title; a.Description = data.Description;
                    a.Category = data.Category; a.Date = data.PostDate;
                    a.IsUrgent = data.IsUrgent; a.IsPinned = data.IsPinned;
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
            _createAnnouncementUC.LoadForEdit(new AnnouncementData
            {
                Title = a.Title,
                Description = a.Description,
                Category = a.Category,
                PostDate = a.Date,
                IsUrgent = a.IsUrgent,
                IsPinned = a.IsPinned,
            });
            ShowCreateAnnouncementUC();
        }

        private void ToggleAnnouncement(int id)
        {
            var a = announcements.Find(x => x.Id == id);
            if (a != null) a.Status = a.Status == "active" ? "inactive" : "active";
            RenderAnnouncements();
        }

        private void DeleteAnnouncement(int announcementId)
        {
            var result = MessageBox.Show(
                "Are you sure you want to delete this announcement? This action cannot be undone.",
                "Delete Announcement",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (var context = new AppDbContext())
                {
                    var announcement = context.Announcements
                        .FirstOrDefault(a => a.AnnouncementId == announcementId);

                    if (announcement == null)
                    {
                        MessageBox.Show("Announcement not found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    context.Announcements.Remove(announcement);
                    context.SaveChanges();
                }

                announcements.RemoveAll(a => a.Id == announcementId);
                RenderAnnouncements();

                MessageBox.Show("Announcement deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("Failed to delete announcement from database. Please try again.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"DbUpdateException: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}\n{ex.StackTrace}");
            }
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
                child.Width = (int)(child.Width * s); child.Height = (int)(child.Height * s);
            }
            child.Location = new Point(
                Math.Max(0, (parent.ClientSize.Width - child.Width) / 2),
                Math.Max(0, (parent.ClientSize.Height - child.Height) / 4));
        }

        //  Pinned Announcements list 
        private void RenderPinnedAnnouncements()
        {
            flpPinnedpnlInsights.SuspendLayout();
            flpPinnedpnlInsights.Controls.Clear();
            flpPinnedpnlInsights.FlowDirection = FlowDirection.TopDown;
            flpPinnedpnlInsights.WrapContents = false;
            flpPinnedpnlInsights.AutoScroll = true;
            flpPinnedpnlInsights.HorizontalScroll.Enabled = false;
            flpPinnedpnlInsights.HorizontalScroll.Visible = false;
            flpPinnedpnlInsights.BackColor = Color.White;

            var pinned = announcements.Where(a => a.IsPinned).OrderByDescending(a => a.Date).ToList();

            if (pinned.Count == 0)
            {
                flpPinnedpnlInsights.Controls.Add(new Label
                {
                    Text = "No pinned announcements.",
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Padding = new Padding(10, 8, 0, 0),
                });
                flpPinnedpnlInsights.ResumeLayout();
                return;
            }

            foreach (var a in pinned)
            {
                Color dotCol = CatIconColor.GetValueOrDefault(a.Category, Color.Gray);
                int rowW = Math.Max(100, flpPinnedpnlInsights.ClientSize.Width - 4);

                var row = new Panel
                {
                    Width = rowW,
                    Height = 52,
                    BackColor = Color.White,
                    Margin = new Padding(0, 0, 0, 0),
                    Cursor = Cursors.Hand,
                    Tag = a,
                };

                // Bottom separator line
                row.Paint += (s, pe) =>
                {
                    using var pen = new Pen(Color.FromArgb(235, 235, 235));
                    pe.Graphics.DrawLine(pen, 0, row.Height - 1, row.Width, row.Height - 1);
                };

                // Left color accent
                row.Controls.Add(new Panel
                {
                    Size = new Size(3, 52),
                    Location = new Point(0, 0),
                    BackColor = dotCol,
                });

                // Category dot
                var dot = new Panel { Size = new Size(8, 8), Location = new Point(12, 22), BackColor = dotCol };
                MakeCircleRegion(dot);
                row.Controls.Add(dot);

                // Title
                var rowTitle = new Label
                {
                    AutoSize = false,
                    Size = new Size(rowW - 30, 20),
                    Location = new Point(26, 6),
                    Text = a.Title,
                    Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(25, 25, 25),
                    AutoEllipsis = true,
                    BackColor = Color.Transparent,
                };
                row.Controls.Add(rowTitle);

                // Date
                row.Controls.Add(new Label
                {
                    AutoSize = true,
                    Location = new Point(26, 28),
                    Text = a.Date.ToString("MMM d, yyyy"),
                    Font = new Font("Segoe UI", 7.5f),
                    ForeColor = Color.Gray,
                    BackColor = Color.Transparent,
                });

                EventHandler open = (s, ev) =>
                {
                    var found = announcements.Find(x => x.Id == a.Id);
                    if (found != null) ShowViewAnnouncementUC(found);
                };
                row.Click += open;
                rowTitle.Click += open;

                flpPinnedpnlInsights.Controls.Add(row);
            }

            flpPinnedpnlInsights.HorizontalScroll.Maximum = 0;
            flpPinnedpnlInsights.AutoScrollPosition = new Point(0, 0);
            flpPinnedpnlInsights.ResumeLayout();
        }

        //  Announcement Insights 
        private void RenderAnnouncementInsights()
        {
            flpPinned.SuspendLayout();
            flpPinned.Controls.Clear();
            flpPinned.BackColor = Color.White;
            flpPinned.FlowDirection = FlowDirection.TopDown;
            flpPinned.WrapContents = false;
            flpPinned.AutoScroll = false;

            int total = announcements.Count;
            int active = announcements.Count(a => a.Status == "active");
            int pinnedCnt = announcements.Count(a => a.IsPinned);
            int readCnt = announcements.Sum(a => a.ViewedCount > 0 ? 1 : 0);
            int withFiles = announcements.Count(a => !string.IsNullOrWhiteSpace(a.AttachedFile));
            int unread = total - active + (active > 0 ? active / 2 : 0);

            // Row width: use the flpPinned width; if it hasn't been laid out yet use parent width
            int rowW = flpPinned.Width > 10
                ? flpPinned.Width
                : (pnlInsightBox?.Width > 10 ? pnlInsightBox.Width - 2 : 120);

            const int ROW_H = 30;
            const int DIV_H = 1;
            const int VAL_W = 28;   
            const int VAL_RIGHT = 6;  

            void AddRow(string label, string value, Color valCol, bool addDivider = true)
            {
                // Full-width container panel
                var row = new Panel
                {
                    Name = "ins_row_" + label,
                    Width = rowW,
                    Height = ROW_H,
                    BackColor = Color.White,
                    Margin = new Padding(0),
                };

                row.Controls.Add(new Label
                {
                    AutoSize = true,
                    Text = label,
                    Font = new Font("Segoe UI", 7.5f),
                    ForeColor = Color.FromArgb(80, 80, 80),
                    Location = new Point(8, (ROW_H - 15) / 2),
                    BackColor = Color.Transparent,
                });

                // Value label: right-anchored so it stays at the right edge on resize
                var valLbl = new Label
                {
                    AutoSize = false,
                    Size = new Size(VAL_W, ROW_H),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    Location = new Point(rowW - VAL_W - VAL_RIGHT, 0),
                    Text = value,
                    Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                    ForeColor = valCol,
                    BackColor = Color.Transparent,
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                row.Controls.Add(valLbl);

                flpPinned.Controls.Add(row);

                if (addDivider)
                {
                    flpPinned.Controls.Add(new Panel
                    {
                        Name = "ins_div_" + label,
                        Width = rowW,
                        Height = DIV_H,
                        BackColor = Color.FromArgb(235, 235, 235),
                        Margin = new Padding(0),
                    });
                }
            }

            AddRow("Total", total.ToString(), Color.FromArgb(50, 50, 50));
            AddRow("Unread", unread.ToString(), Color.FromArgb(220, 50, 50));
            AddRow("Pinned", pinnedCnt.ToString(), Color.DarkOrange);
            AddRow("Read", readCnt.ToString(), Color.ForestGreen);
            AddRow("With Files", withFiles > 0 ? withFiles.ToString() : "—", Color.RoyalBlue, addDivider: false);

            flpPinned.ResumeLayout();
            flpPinned.Refresh();
        }

        //  Graphics helpers 
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
            if (lbl.Width <= 0 || lbl.Height <= 0) return;
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
            // Subtle left border separating sidebar from main content
            using var pen = new Pen(Color.FromArgb(218, 218, 218), 1f);
            e.Graphics.DrawLine(pen, 0, 0, 0, panelLeft.Height);
        }
    }
}