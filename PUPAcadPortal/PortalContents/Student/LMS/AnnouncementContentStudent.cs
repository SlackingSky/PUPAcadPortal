using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Student.LMS
{
    public partial class AnnouncementContentStudent : UserControl
    {

        private SizeF _designSize;

        private readonly Dictionary<Control, RectangleF> _origBounds = new();
        private readonly Dictionary<Control, float> _origFontSz = new();
        private class StudentAnnouncement
        {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Category { get; set; } = "General";
            public string OfficeName { get; set; } = "Admin Office";
            public string InstructorName { get; set; } = string.Empty;
            public DateTime Date { get; set; } = DateTime.Now;
            public bool IsUrgent { get; set; }
            public bool IsPinned { get; set; }
            public bool IsRead { get; set; }
            public string Status { get; set; } = "active";
        }

        private List<StudentAnnouncement> _announcements = new();
        private string _annCategoryFilter = "All Categories";
        private string _annSortOrder = "Latest First";
        private string _annSearchText = "";

        private ViewAnnouncementStudent _viewStudentUC;

        private static readonly Dictionary<string, Color> CatIconColor = new()
        {
            ["General"] = Color.FromArgb(55, 138, 221),
            ["Academic"] = Color.FromArgb(99, 153, 34),
            ["Schedule"] = Color.FromArgb(186, 117, 23),
            ["Events"] = Color.FromArgb(212, 83, 126),
            ["Examinations"] = Color.FromArgb(127, 119, 221),
            ["Administrative"] = Color.FromArgb(90, 90, 200),
            ["Urgent"] = Color.FromArgb(220, 50, 50),
        };
        private static readonly Dictionary<string, Color> CatBgColor = new()
        {
            ["General"] = Color.FromArgb(230, 241, 251),
            ["Academic"] = Color.FromArgb(234, 243, 222),
            ["Schedule"] = Color.FromArgb(250, 238, 218),
            ["Events"] = Color.FromArgb(251, 234, 240),
            ["Examinations"] = Color.FromArgb(238, 237, 254),
            ["Administrative"] = Color.FromArgb(230, 230, 245),
            ["Urgent"] = Color.FromArgb(255, 235, 235),
        };
        public AnnouncementContentStudent()
        {
            InitializeComponent();
        }

        private void AnnouncementContentStudent_Load(object sender, EventArgs e)
        {
            flpAnnouncements.AutoScroll = false;
            flpAnnouncements.FlowDirection = FlowDirection.TopDown;
            flpAnnouncements.WrapContents = false;
            flpAnnouncements.HorizontalScroll.Enabled = false;
            flpAnnouncements.HorizontalScroll.Visible = false;
            flpAnnouncements.AutoScroll = true;
            this.Resize += UserControl_Resize;

            _viewStudentUC = new ViewAnnouncementStudent
            {
                Visible = false,
                Anchor = AnchorStyles.None,
            };
            _viewStudentUC.CloseRequested += (s, ev) =>
            {
                _viewStudentUC.Visible = false;
                RenderAnnouncements();
                BuildInsightsPanel();
            };
            pnlAnnounce.Controls.Add(_viewStudentUC);

            SeedAnnouncements();
            WireAnnouncementControls();
            RenderAnnouncements();
            BuildCategoryPanel();
            BuildInsightsPanel();

            _designSize = new SizeF(this.ClientSize.Width, this.ClientSize.Height);
            SnapshotControls(this.Controls);
        }

        private void UserControl_Resize(object sender, EventArgs e)
        {
            if (_designSize.Width == 0 || _designSize.Height == 0) return;

            float rx = Math.Max(this.ClientSize.Width, 1024) / _designSize.Width;
            float ry = Math.Max(this.ClientSize.Height, 700) / _designSize.Height;

            this.SuspendLayout();
            ScaleControls(this.Controls, rx, ry);
            this.ResumeLayout(true);

            if (_viewStudentUC != null && _viewStudentUC.Visible)
                CentreInPanel(_viewStudentUC, pnlAnnounce);

            RenderAnnouncements();
            BuildCategoryPanel();
            BuildPinnedPanel();
            BuildInsightsPanel();
        }

        private void ScaleControls(Control.ControlCollection controls, float rx, float ry)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl.Tag is string t && t.Contains("noScale")) continue;
                if (!_origBounds.TryGetValue(ctrl, out RectangleF ob)) continue;

                int newX = R(ob.X * rx);
                int newY = R(ob.Y * ry);
                int newW = Math.Max(1, R(ob.Width * rx));
                int newH = Math.Max(1, R(ob.Height * ry));

                switch (ctrl.Dock)
                {
                    case DockStyle.Fill: break;
                    case DockStyle.Top: ctrl.Height = newH; break;
                    case DockStyle.Bottom: ctrl.Height = newH; break;
                    case DockStyle.Left: ctrl.Width = newW; break;
                    case DockStyle.Right: ctrl.Width = newW; break;
                    default: ctrl.SetBounds(newX, newY, newW, newH); break;
                }

                if (_origFontSz.TryGetValue(ctrl, out float origSz))
                {
                    float newSz = Math.Max(6f, origSz * Math.Min(rx, ry));
                    if (Math.Abs(ctrl.Font.Size - newSz) > 0.15f)
                    {
                        try { ctrl.Font = new Font(ctrl.Font.FontFamily, newSz, ctrl.Font.Style, GraphicsUnit.Point); }
                        catch { }
                    }
                }

                if (ctrl.HasChildren) ScaleControls(ctrl.Controls, rx, ry);
            }
        }
        private void WireAnnouncementControls()
        {
            txtSearch.TextChanged += (s, e) =>
            {
                _annSearchText = txtSearch.Text.Trim();
                RenderAnnouncements();
            };

            cmbCategory.SelectedIndexChanged += (s, e) =>
            {
                _annCategoryFilter = cmbCategory.Text;
                RenderAnnouncements();
                BuildCategoryPanel();
            };

            cmbSort.SelectedIndexChanged += (s, e) =>
            {
                _annSortOrder = cmbSort.Text;
                RenderAnnouncements();
            };

            btnMarkAllRead.Click += (s, e) =>
            {
                foreach (var a in _announcements) a.IsRead = true;
                RenderAnnouncements();
                BuildInsightsPanel();
            };
        }

        private void RenderAnnouncements()
        {
            flpAnnouncements.SuspendLayout();
            flpAnnouncements.Controls.Clear();
            flpAnnouncements.FlowDirection = FlowDirection.TopDown;
            flpAnnouncements.WrapContents = false;
            flpAnnouncements.AutoScroll = false;
            flpAnnouncements.HorizontalScroll.Enabled = false;
            flpAnnouncements.HorizontalScroll.Visible = false;
            flpAnnouncements.VerticalScroll.Enabled = true;
            flpAnnouncements.AutoScroll = true;

            IEnumerable<StudentAnnouncement> filtered = _announcements;

            if (_annCategoryFilter != "All Categories" && !string.IsNullOrEmpty(_annCategoryFilter))
                filtered = filtered.Where(a => a.Category == _annCategoryFilter);

            if (!string.IsNullOrWhiteSpace(_annSearchText))
            {
                string q = _annSearchText.ToLower();
                filtered = filtered.Where(a =>
                    a.Title.ToLower().Contains(q) ||
                    a.Description.ToLower().Contains(q) ||
                    a.OfficeName.ToLower().Contains(q));
            }

            List<StudentAnnouncement> sorted;

            if (_annSortOrder == "Oldest First")
            {
                sorted = filtered
                    .OrderBy(a => a.IsPinned ? 0 : 1)
                    .ThenBy(a => a.IsRead ? 1 : 0)
                    .ThenBy(a => a.Date)
                    .ToList();
            }
            else
            {
                sorted = filtered
                    .OrderBy(a => a.IsPinned ? 0 : 1)
                    .ThenBy(a => a.IsRead ? 1 : 0)
                    .ThenByDescending(a => a.Date)
                    .ToList();
            }

            int cardWidth = Math.Max(400, flpAnnouncements.ClientSize.Width - 22);

            foreach (var ann in sorted)
                flpAnnouncements.Controls.Add(BuildCard(ann, cardWidth));

            BuildPinnedPanel();
            flpAnnouncements.ResumeLayout();
        }

        private AnnouncementLayout BuildCard(StudentAnnouncement a, int cardWidth)
        {
            var card = new AnnouncementLayout();

            card.LoadStudent(
                id: a.Id,
                title: a.Title,
                description: a.Description,
                category: a.Category,
                officeName: a.OfficeName,
                date: a.Date,
                isUrgent: a.IsUrgent,
                isPinned: a.IsPinned,
                isRead: a.IsRead,
                cardWidth: cardWidth,
                instructorName: a.InstructorName);

            card.CardClicked += (s, id) =>
            {
                var ann = _announcements.Find(x => x.Id == id);
                if (ann != null) ShowStudentAnnouncementDetail(ann);
            };
            card.PinToggled += (s, id) =>
            {
                var ann = _announcements.Find(x => x.Id == id);
                if (ann != null)
                {
                    ann.IsPinned = !ann.IsPinned;
                    RenderAnnouncements();
                    BuildPinnedPanel();
                }
            };

            return card;
        }

        private void ShowStudentAnnouncementDetail(StudentAnnouncement a)
        {
            a.IsRead = true;

            _viewStudentUC.LoadAnnouncement(
                a.Title,
                a.Description,
                a.Category,
                a.OfficeName,
                a.Date,
                a.IsUrgent,
                a.IsPinned,
                a.InstructorName);

            CentreInPanel(_viewStudentUC, pnlAnnounce);

            _viewStudentUC.BringToFront();
            _viewStudentUC.Visible = true;

            RenderAnnouncements();
            BuildInsightsPanel();
        }


        private void BuildPinnedPanel()
        {
            flpPinned.Controls.Clear();
            flpPinned.FlowDirection = FlowDirection.TopDown;
            flpPinned.WrapContents = false;
            flpPinned.AutoScroll = false;
            flpPinned.HorizontalScroll.Enabled = false;
            flpPinned.HorizontalScroll.Visible = false;
            flpPinned.AutoScroll = true;

            var pinned = _announcements.Where(a => a.IsPinned).ToList();
            if (pinned.Count == 0)
            {
                flpPinned.Controls.Add(new Label
                {
                    Text = "No pinned announcements.",
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Padding = new Padding(4),
                });
                return;
            }

            foreach (var a in pinned)
            {
                Color iconCol = CatIconColor.GetValueOrDefault(a.Category, Color.Gray);

                int rowW = Math.Max(100, flpPinned.ClientSize.Width - 4);
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

                var dot = new Panel { Size = new Size(8, 8), Location = new Point(8, 22), BackColor = iconCol };
                MakeCircle(dot);
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

                row.Click += (s, ev) => ShowStudentAnnouncementDetail(a);
                rowTitle.Click += (s, ev) => ShowStudentAnnouncementDetail(a);
                flpPinned.Controls.Add(row);
            }
        }

        private void BuildCategoryPanel()
        {
            flpCategories.Controls.Clear();
            flpCategories.FlowDirection = FlowDirection.TopDown;
            flpCategories.WrapContents = false;
            flpCategories.BackColor = Color.White;
            flpCategories.AutoScroll = false;
            flpCategories.HorizontalScroll.Enabled = false;
            flpCategories.HorizontalScroll.Visible = false;
            flpCategories.AutoScroll = true;

            var cats = new[]
            {
                "All Categories", "General", "Academic", "Administrative",
                "Events", "Examinations", "Schedule", "Urgent"
            };

            foreach (var cat in cats)
            {
                int count = cat == "All Categories"
                    ? _announcements.Count
                    : _announcements.Count(a => a.Category == cat);

                bool isActive = _annCategoryFilter == cat;
                Color dotCol = CatIconColor.GetValueOrDefault(cat, Color.FromArgb(139, 0, 0));

                int rowW = Math.Max(100, flpCategories.ClientSize.Width - 4);
                var row = new Panel
                {
                    Width = rowW,
                    Height = 32,
                    BackColor = isActive ? Color.FromArgb(245, 238, 238) : Color.Transparent,
                    Cursor = Cursors.Hand,
                    Margin = new Padding(0, 1, 0, 1),
                };

                var dot = new Panel { Size = new Size(9, 9), Location = new Point(8, 12), BackColor = dotCol };
                MakeCircle(dot);
                row.Controls.Add(dot);

                var lbl = new Label
                {
                    AutoSize = false,
                    Size = new Size(rowW - 52, 32),
                    Location = new Point(24, 0),
                    Text = cat == "All Categories" ? "All" : cat,
                    Font = new Font("Segoe UI", 9f, isActive ? FontStyle.Bold : FontStyle.Regular),
                    ForeColor = Color.FromArgb(40, 40, 40),
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.Transparent,
                };
                row.Controls.Add(lbl);

                var badge = new Label
                {
                    AutoSize = false,
                    Size = new Size(28, 18),
                    Location = new Point(rowW - 34, 7),
                    Text = count.ToString(),
                    Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                    ForeColor = isActive ? Color.White : Color.FromArgb(80, 80, 80),
                    BackColor = isActive ? Color.FromArgb(139, 0, 0) : Color.FromArgb(225, 225, 225),
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                MakeRoundedRegion(badge, 9);
                row.Controls.Add(badge);

                EventHandler handler = (s, ev) =>
                {
                    _annCategoryFilter = cat;
                    cmbCategory.Text = cat;
                    BuildCategoryPanel();
                    RenderAnnouncements();
                };
                row.Click += handler;
                dot.Click += handler;
                lbl.Click += handler;
                badge.Click += handler;

                flpCategories.Controls.Add(row);
            }
        }

        private void BuildInsightsPanel()
        {
            var toRemove = pnlInsights.Controls
                .OfType<Control>()
                .Where(c => c.Name.StartsWith("ins_"))
                .ToList();
            foreach (var c in toRemove) pnlInsights.Controls.Remove(c);

            int total = _announcements.Count;
            int unread = _announcements.Count(a => !a.IsRead);
            int pinned = _announcements.Count(a => a.IsPinned);

            void AddRow(string label, string value, int y, Color col)
            {
                pnlInsights.Controls.Add(new Label
                {
                    Name = "ins_lbl_" + y,
                    AutoSize = true,
                    Text = label,
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = Color.FromArgb(80, 80, 80),
                    Location = new Point(8, y),
                });
                pnlInsights.Controls.Add(new Label
                {
                    Name = "ins_val_" + y,
                    AutoSize = true,
                    Text = value,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    ForeColor = col,
                    Location = new Point(pnlInsights.Width - 50, y - 2),
                });
            }

            AddRow("Total Announcements", total.ToString(), 30, Color.FromArgb(50, 50, 50));
            AddRow("Unread", unread.ToString(), 58, unread > 0 ? Color.FromArgb(200, 0, 0) : Color.Green);
            AddRow("Pinned", pinned.ToString(), 86, Color.FromArgb(150, 100, 0));
            AddRow("Read", (total - unread).ToString(), 114, Color.FromArgb(22, 163, 74));
        }

        private void SeedAnnouncements()
        {
            _announcements = new List<StudentAnnouncement>
            {
                new() {
                    Id = 1, Title = "Updated Travel Reimbursement Policy",
                    Description   = "Please note that the mileage reimbursement rate for university-related travel has been adjusted. All reimbursement claims submitted after May 1, 2026 must use the new rate of ₱12.00 per km.",
                    Category = "Administrative", OfficeName = "Admin Office", InstructorName = "Dr. Reyes",
                    Date = new DateTime(2026, 4, 15, 10, 30, 0),
                    IsUrgent = true, IsPinned = true, IsRead = false,
                },
                new() {
                    Id = 2, Title = "Midterm Examination Schedule Released",
                    Description   = "The official midterm examination schedule for all BSIT 2nd year subjects is now available. Please check the LMS for your room assignments and bring your student ID on exam day.",
                    Category = "Examinations", OfficeName = "Registrar's Office", InstructorName = "Prof. Santos",
                    Date = new DateTime(2026, 4, 20, 8, 0, 0),
                    IsUrgent = true, IsPinned = true, IsRead = false,
                },
                new() {
                    Id = 3, Title = "Programming 1 – Lab Activity This Friday",
                    Description   = "Bring your laptops for the graded lab activity covering Modules 4 and 5. The activity will be conducted using Visual Studio 2022. No borrowing of equipment will be allowed.",
                    Category = "Academic", OfficeName = "CCIS Department", InstructorName = "Prof. Santos",
                    Date = new DateTime(2026, 4, 18, 9, 0, 0),
                    IsUrgent = false, IsPinned = false, IsRead = true,
                },
                new() {
                    Id = 4, Title = "PUP Foundation Day Celebration – May 17",
                    Description   = "Join us for the PUP Foundation Day celebration. Activities include a student showcase, cultural performances, and a technology exhibit. Attendance is encouraged for all students.",
                    Category = "Events", OfficeName = "Student Affairs Office", InstructorName = "Dr. Cruz",
                    Date = new DateTime(2026, 4, 10, 14, 0, 0),
                    IsUrgent = false, IsPinned = false, IsRead = false,
                },
                new() {
                    Id = 5, Title = "Reminder: Submit Assignment Outputs",
                    Description   = "All pending assignment outputs for Information Management must be submitted via the LMS portal before May 15 at 11:59 PM. Late submissions will not be accepted under any circumstance.",
                    Category = "Academic", OfficeName = "CCIS Department", InstructorName = "Prof. Santos",
                    Date = new DateTime(2026, 4, 8, 11, 0, 0),
                    IsUrgent = false, IsPinned = false, IsRead = true,
                },
                new() {
                    Id = 6, Title = "Library Hours Extended Until June",
                    Description   = "The university library will extend its operating hours to 8:00 AM – 9:00 PM on weekdays starting May 1 through June 30, 2026 to support students during the examination period.",
                    Category = "General", OfficeName = "Library Services", InstructorName = "Librarian Gomez",
                    Date = new DateTime(2026, 4, 5, 7, 30, 0),
                    IsUrgent = false, IsPinned = false, IsRead = false,
                },
                new() {
                    Id = 7, Title = "Enrollment for 2nd Semester Now Open",
                    Description   = "Online enrollment for the 2nd semester of Academic Year 2026–2027 is now open. Students must settle all outstanding balances and secure their assessment forms before enrolling. Visit the Registrar's portal for the step-by-step guide.",
                    Category = "General", OfficeName = "Registrar's Office", InstructorName = "Registrar Dela Torre",
                    Date = new DateTime(2026, 5, 2, 8, 0, 0),
                    IsUrgent = false, IsPinned = false, IsRead = false,
                },
                new() {
                    Id = 8, Title = "Scholarship Application Deadline – May 20",
                    Description   = "All students applying for the CHED Scholarship and PUP Internal Scholarship must submit their complete documentary requirements to the Scholarship Office no later than May 20, 2026 at 5:00 PM. No extensions will be granted.",
                    Category = "Administrative", OfficeName = "Scholarship Office", InstructorName = "Dr. Valdez",
                    Date = new DateTime(2026, 5, 5, 9, 0, 0),
                    IsUrgent = true, IsPinned = false, IsRead = false,
                },
                new() {
                    Id = 9, Title = "Campus-Wide Maintenance: May 14 (No Classes)",
                    Description   = "There will be no classes on May 14, 2026 due to scheduled campus-wide electrical maintenance. All LMS deadlines falling on this date are automatically extended by 24 hours. Students are advised to plan accordingly.",
                    Category = "Schedule", OfficeName = "Facilities Management Office", InstructorName = "Engr. Bautista",
                    Date = new DateTime(2026, 5, 8, 7, 0, 0),
                    IsUrgent = true, IsPinned = true, IsRead = false,
                },
                new() {
                    Id = 10, Title = "Intramural Sports Registration Open",
                    Description   = "Registration for the Annual PUP Intramural Sports Festival is now open. Students interested in joining basketball, volleyball, badminton, or swimming events must register through their respective department coordinators before May 22, 2026.",
                    Category = "Events", OfficeName = "Student Affairs Office", InstructorName = "Coach Mendoza",
                    Date = new DateTime(2026, 5, 6, 10, 0, 0),
                    IsUrgent = false, IsPinned = false, IsRead = true,
                },
                new() {
                    Id = 11, Title = "Final Examination Coverage Posted",
                    Description   = "The official final examination coverage for all BSIT subjects has been posted on the LMS. Students are advised to review the coverage carefully and contact their respective professors for any clarifications. Good luck on your finals!",
                    Category = "Examinations", OfficeName = "CCIS Department", InstructorName = "Prof. Santos",
                    Date = new DateTime(2026, 5, 9, 8, 30, 0),
                    IsUrgent = false, IsPinned = false, IsRead = false,
                },
                new() {
                    Id = 12, Title = "Academic Integrity Seminar – May 16",
                    Description   = "All students are required to attend the Academic Integrity and Anti-Plagiarism Seminar on May 16, 2026 at 1:00 PM via Zoom. Attendance will be recorded and counted toward your class standing. The meeting link will be sent through your official university email.",
                    Category = "Academic", OfficeName = "CCIS Department", InstructorName = "Dr. Reyes",
                    Date = new DateTime(2026, 5, 10, 9, 0, 0),
                    IsUrgent = true, IsPinned = false, IsRead = false,
                },
                new() {
                    Id = 13, Title = "Lost & Found: Items at Security Office",
                    Description   = "Several items including IDs, umbrellas, and a laptop bag have been turned over to the Security Office near Gate 1. Owners may claim their belongings by presenting valid identification. Unclaimed items will be turned over to the Student Affairs Office after May 25, 2026.",
                    Category = "General", OfficeName = "Security Office", InstructorName = "Guard Navarro",
                    Date = new DateTime(2026, 5, 7, 14, 0, 0),
                    IsUrgent = false, IsPinned = false, IsRead = true,
                },
                new() {
                    Id = 14, Title = "IT Career Fair – May 22 at PUP Gymnasium",
                    Description   = "The PUP Career Services Office invites all BSIT students to the annual IT Career Fair on May 22, 2026 from 9:00 AM to 4:00 PM at the university gymnasium. Over 30 technology companies will be present. Bring printed resumes and dress business casual.",
                    Category = "Events", OfficeName = "Career Services Office", InstructorName = "Dr. Cruz",
                    Date = new DateTime(2026, 5, 8, 10, 0, 0),
                    IsUrgent = false, IsPinned = true, IsRead = false,
                },
                new() {
                    Id = 15, Title = "Class Schedule Adjustment – May 13",
                    Description   = "Due to the university-wide convocation, all morning classes on May 13, 2026 are moved to the afternoon session. Students with afternoon conflicts are advised to coordinate with their respective professors. The updated schedule is posted on the Registrar's portal.",
                    Category = "Schedule", OfficeName = "Registrar's Office", InstructorName = "Registrar Dela Torre",
                    Date = new DateTime(2026, 5, 10, 7, 30, 0),
                    IsUrgent = true, IsPinned = false, IsRead = false,
                },
                new() {
                    Id = 16, Title = "Capstone Project Defense Schedule Released",
                    Description   = "The official schedule for 4th-year BSIT Capstone Project defenses has been published on the departmental bulletin board and LMS. All groups are required to submit their final manuscripts and slide decks at least three days before their assigned defense date. Coordinate with your adviser immediately.",
                    Category = "Academic", OfficeName = "CCIS Department", InstructorName = "Prof. Santos",
                    Date = new DateTime(2026, 5, 11, 8, 0, 0),
                    IsUrgent = false, IsPinned = true, IsRead = false,
                },
            };
        }

        private void SnapshotControls(Control.ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl.Tag is string t && t.Contains("noScale")) continue;
                _origBounds[ctrl] = new RectangleF(ctrl.Left, ctrl.Top, ctrl.Width, ctrl.Height);
                _origFontSz[ctrl] = ctrl.Font.Size;
                if (ctrl.HasChildren) SnapshotControls(ctrl.Controls);
            }
        }

        private static int R(float v) => (int)Math.Round(v);

        private static void CentreInPanel(Control child, Control parent)
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

        // ════════════════════════════════════════════════════════════════════
        //  DRAWING HELPERS
        // ════════════════════════════════════════════════════════════════════
        private static void MakeRoundedRegion(Control c, int radius)
        {
            var path = new GraphicsPath();
            var r = new Rectangle(0, 0, c.Width, c.Height);
            path.AddArc(r.X, r.Y, radius, radius, 180, 90);
            path.AddArc(r.Right - radius, r.Y, radius, radius, 270, 90);
            path.AddArc(r.Right - radius, r.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(r.X, r.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            c.Region = new Region(path);
        }

        private static void MakeCircle(Control c)
        {
            var path = new GraphicsPath();
            path.AddEllipse(0, 0, c.Width, c.Height);
            c.Region = new Region(path);
        }
    }
}
