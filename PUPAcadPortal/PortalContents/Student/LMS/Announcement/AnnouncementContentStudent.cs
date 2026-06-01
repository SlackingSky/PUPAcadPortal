using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Student.LMS
{
    //  ATTACHMENT MODEL
    public class AnnouncementAttachment
    {
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = "pdf";   // pdf, docx, pptx, img
        public long FileSizeBytes { get; set; } = 0;

        public string SizeLabel => FileSizeBytes >= 1_048_576
            ? $"{FileSizeBytes / 1_048_576.0:0.#} MB"
            : $"{FileSizeBytes / 1024.0:0.#} KB";
    }

    //  MAIN USER CONTROL
    public partial class AnnouncementContentStudent : UserControl
    {
        //  Design-time sizing snapshot for proportional scaling 
        private SizeF _designSize;
        private readonly Dictionary<Control, RectangleF> _origBounds = new();
        private readonly Dictionary<Control, float> _origFontSz = new();

        private class StudentAnnouncement
        {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Category { get; set; } = "General";
            public string CourseName { get; set; } = string.Empty;
            public string OfficeName { get; set; } = "Admin Office";
            public string InstructorName { get; set; } = string.Empty;
            public DateTime Date { get; set; } = DateTime.Now;
            public bool IsUrgent { get; set; }
            public bool IsPinned { get; set; }
            public bool IsRead { get; set; }
            public string Status { get; set; } = "active";
            public List<AnnouncementAttachment> Attachments { get; set; } = new();
        }

        private List<StudentAnnouncement> _announcements = new();
        private string _annCategoryFilter = "All Categories";
        private string _annSortOrder = "Latest First";
        private string _annSearchText = "";

        //  Category colour maps 
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

        //  LOAD
        private void AnnouncementContentStudent_Load(object sender, EventArgs e)
        {
            // ── Fix ComboBoxes: DropDownList so they are not editable ─────────
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSort.DropDownStyle = ComboBoxStyle.DropDownList;

            // Rebuild items programmatically so they are clean
            cmbCategory.Items.Clear();
            cmbCategory.Items.AddRange(new object[]
                { "All Categories", "General", "Academic", "Administrative",
                  "Events", "Examinations", "Schedule", "Urgent" });
            cmbCategory.SelectedIndex = 0;

            cmbSort.Items.Clear();
            cmbSort.Items.AddRange(new object[] { "Latest First", "Oldest First" });
            cmbSort.SelectedIndex = 0;

            //  FlowLayout setup 
            flpAnnouncements.AutoScroll = false;
            flpAnnouncements.FlowDirection = FlowDirection.TopDown;
            flpAnnouncements.WrapContents = false;
            flpAnnouncements.HorizontalScroll.Enabled = false;
            flpAnnouncements.HorizontalScroll.Visible = false;
            flpAnnouncements.AutoScroll = true;

            this.Resize += UserControl_Resize;

            SeedAnnouncements();
            WireAnnouncementControls();
            RenderAnnouncements();
            BuildCategoryPanel();
            BuildInsightsPanel();

            _designSize = new SizeF(this.ClientSize.Width, this.ClientSize.Height);
            SnapshotControls(this.Controls);
        }

        //  RESIZE / SCALE
        private void UserControl_Resize(object sender, EventArgs e)
        {
            if (_designSize.Width == 0 || _designSize.Height == 0) return;

            float rx = Math.Max(this.ClientSize.Width, 1024) / _designSize.Width;
            float ry = Math.Max(this.ClientSize.Height, 700) / _designSize.Height;

            this.SuspendLayout();
            ScaleControls(this.Controls, rx, ry);
            this.ResumeLayout(true);

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

                int newX = R(ob.X * rx), newY = R(ob.Y * ry);
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

        //  WIRE CONTROLS
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

        //  RENDER ANNOUNCEMENT LIST
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
                    a.OfficeName.ToLower().Contains(q) ||
                    a.CourseName.ToLower().Contains(q));
            }

            List<StudentAnnouncement> sorted = _annSortOrder == "Oldest First"
                ? filtered
                    .OrderBy(a => a.IsPinned ? 0 : 1)
                    .ThenBy(a => a.IsRead ? 1 : 0)
                    .ThenBy(a => a.Date)
                    .ToList()
                : filtered
                    .OrderBy(a => a.IsPinned ? 0 : 1)
                    .ThenBy(a => a.IsRead ? 1 : 0)
                    .ThenByDescending(a => a.Date)
                    .ToList();

            int cardWidth = Math.Max(400, flpAnnouncements.ClientSize.Width - 22);

            foreach (var ann in sorted)
                flpAnnouncements.Controls.Add(BuildCard(ann, cardWidth));

            BuildPinnedPanel();
            flpAnnouncements.ResumeLayout();
        }

        //  BUILD CARD  —  delegates to AnnouncementCardUC (separate UserControl)
        private AnnouncementCardUC BuildCard(StudentAnnouncement a, int cardWidth)
        {
            var card = new AnnouncementCardUC();

            card.Load(
                id: a.Id,
                title: a.Title,
                description: a.Description,
                category: a.Category,
                courseName: a.CourseName,
                officeName: a.OfficeName,
                instructorName: a.InstructorName,
                date: a.Date,
                isUrgent: a.IsUrgent,
                isPinned: a.IsPinned,
                isRead: a.IsRead,
                attachmentCount: a.Attachments.Count,
                cardWidth: cardWidth);

            card.Margin = new Padding(0, 0, 0, 4);

            card.CardClicked += (s, id) =>
            {
                var ann = _announcements.Find(x => x.Id == id);
                if (ann != null) ShowDetailView(ann);
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

        //  DETAIL VIEW  — proper Form popup (fixes all layout/clipping issues)
        private Panel _detailPanel; // kept for API compat; not used by popup path

        private void ShowDetailView(StudentAnnouncement a)
        {
            //  Accent colour — computed BEFORE marking as read 
            // Maroon for unread/urgent; category colour for already-read items
            Color accent = (a.IsUrgent || !a.IsRead)
                ? Color.FromArgb(139, 0, 0)
                : CatIconColor.GetValueOrDefault(a.Category, Color.FromArgb(90, 90, 200));

            // Mark read AFTER colour decision
            a.IsRead = true;
            RenderAnnouncements();
            BuildInsightsPanel();

            //  Constants 
            const int POPUP_W = 800;
            const int HDR_H = 54;
            const int PAD = 28;       // left/right padding inside body
            const int CORNER = 12;

            // content width = popup - both paddings - scrollbar allowance
            int contentW = POPUP_W - PAD * 2 - SystemInformation.VerticalScrollBarWidth - 4;

            //  Build popup Form 
            var popup = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual,   // we position manually
                BackColor = Color.White,
                Width = POPUP_W,
                Height = 100,            // will be recalculated below
                ShowInTaskbar = false,
                KeyPreview = true,
            };
            popup.KeyDown += (s, ke) => { if (ke.KeyCode == Keys.Escape) popup.Close(); };

            //  Header 
            var hdr = new Panel
            {
                Dock = DockStyle.Top,
                Height = HDR_H,
                BackColor = accent,
            };
            popup.Controls.Add(hdr);

            // Category / urgent pill
            string pillTx = a.IsUrgent ? "⚠  URGENT" : a.Category.ToUpper();
            var pillFont = new Font("Segoe UI", 8f, FontStyle.Bold);
            int pillW = TextRenderer.MeasureText(pillTx, pillFont).Width + 22;
            var pill = new Label
            {
                AutoSize = false,
                Size = new Size(pillW, 24),
                Location = new Point(PAD, (HDR_H - 24) / 2),
                Text = pillTx,
                Font = pillFont,
                ForeColor = accent,
                BackColor = Color.FromArgb(228, 255, 255, 255),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            MakeRoundedRegion(pill, 8);
            hdr.Controls.Add(pill);

            var btnClose = new Button
            {
                Size = new Size(32, 32),
                Location = new Point(POPUP_W - 44, (HDR_H - 32) / 2),
                Text = "✕",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                TabStop = false,
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 0, 0, 0);
            btnClose.Click += (s, e) => popup.Close();
            hdr.Controls.Add(btnClose);

            // drag-to-move by header
            bool dragging = false;
            Point dragStart = Point.Empty;
            hdr.MouseDown += (s, me) =>
            {
                if (me.Button == MouseButtons.Left) { dragging = true; dragStart = me.Location; }
            };
            hdr.MouseMove += (s, me) =>
            {
                if (!dragging) return;
                popup.Location = new Point(
                    popup.Location.X + me.X - dragStart.X,
                    popup.Location.Y + me.Y - dragStart.Y);
            };
            hdr.MouseUp += (s, me) => dragging = false;

            //  Body scroll panel 
            // body fills the form below the header
            var body = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White,
            };
            popup.Controls.Add(body);

            // ── Inner content panel – all controls go here at fixed positions ─
            // We add it to body; its Height will be set after measuring all children.
            var inner = new Panel
            {
                Left = 0,
                Top = 0,
                Width = POPUP_W - SystemInformation.VerticalScrollBarWidth,
                BackColor = Color.White,
            };
            body.Controls.Add(inner);

            // Running y inside inner, offset from top
            int y = PAD - 4;

            //  Title 
            var lblTitle = new Label
            {
                AutoSize = false,
                Width = contentW,
                Location = new Point(PAD, y),
                Text = a.Title,
                Font = new Font("Segoe UI Semibold", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(18, 18, 18),
            };
            lblTitle.Height = TextRenderer.MeasureText(
                a.Title, lblTitle.Font, new Size(contentW, 0),
                TextFormatFlags.WordBreak).Height + 4;
            inner.Controls.Add(lblTitle);
            y += lblTitle.Height + 14;

            // Build each chip, measure, wrap manually so we don't rely on
            // FlowLayoutPanel.GetPreferredSize (which is unreliable before layout)
            var chips = new List<(string text, Color fg, Color bg)>
            {
                ("📅 " + a.Date.ToString("MMM d, yyyy  •  h:mm tt"),
                    Color.FromArgb(55,55,55), Color.FromArgb(236,236,236)),
                ("🏢 " + a.OfficeName,
                    Color.FromArgb(35,80,155), Color.FromArgb(224,238,255)),
                ("👤 " + a.InstructorName,
                    Color.FromArgb(65,65,65), Color.FromArgb(236,236,236)),
            };
            if (!string.IsNullOrEmpty(a.CourseName))
                chips.Add(("📚 " + a.CourseName,
                    Color.FromArgb(40, 100, 40), Color.FromArgb(218, 244, 212)));

            var chipFont = new Font("Segoe UI", 8f);
            int cx = PAD, cy = y, chipH = 26, chipGap = 6;
            foreach (var (text, fg, bg) in chips)
            {
                int cw = TextRenderer.MeasureText(text, chipFont).Width + 22;
                if (cx + cw > PAD + contentW && cx > PAD)   // wrap row
                { cx = PAD; cy += chipH + chipGap; }

                var chip = new Label
                {
                    AutoSize = false,
                    Size = new Size(cw, chipH),
                    Location = new Point(cx, cy),
                    Text = text,
                    Font = chipFont,
                    ForeColor = fg,
                    BackColor = bg,
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                MakeRoundedRegion(chip, 9);
                inner.Controls.Add(chip);
                cx += cw + chipGap;
            }
            y = cy + chipH + 16;    // y after last chip row

            //  Divider 
            inner.Controls.Add(new Panel
            {
                Location = new Point(PAD, y),
                Size = new Size(contentW, 1),
                BackColor = Color.FromArgb(222, 222, 222),
            });
            y += 14;

            //  Description 
            var descFont = new Font("Segoe UI", 9.5f);
            var lblDesc = new Label
            {
                AutoSize = false,
                Width = contentW,
                Location = new Point(PAD, y),
                Text = a.Description,
                Font = descFont,
                ForeColor = Color.FromArgb(48, 48, 48),
            };
            lblDesc.Height = TextRenderer.MeasureText(
                a.Description, descFont, new Size(contentW, 0),
                TextFormatFlags.WordBreak).Height + 8;
            inner.Controls.Add(lblDesc);
            y += lblDesc.Height + 18;

            // Attachments 
            if (a.Attachments.Count > 0)
            {
                // section divider
                inner.Controls.Add(new Panel
                {
                    Location = new Point(PAD, y),
                    Size = new Size(contentW, 1),
                    BackColor = Color.FromArgb(222, 222, 222),
                });
                y += 14;

                // section header
                inner.Controls.Add(new Label
                {
                    AutoSize = true,
                    Location = new Point(PAD, y),
                    Text = "📎  Attachments",
                    Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(30, 30, 30),
                    BackColor = Color.Transparent,
                });
                y += 34;

                foreach (var att in a.Attachments)
                {
                    var attRow = BuildAttachmentRow(att, contentW);
                    attRow.Location = new Point(PAD, y);
                    inner.Controls.Add(attRow);
                    y += attRow.Height + 8;
                }
            }

            y += PAD;   // bottom padding

            //  Size form to content 
            inner.Height = y;

            var screen = Screen.FromControl(this).WorkingArea;
            int maxBodyH = (int)(screen.Height * 0.84) - HDR_H;
            int bodyH = Math.Min(y, maxBodyH);
            popup.Height = HDR_H + bodyH;

            // Round corners (rebuild with final height)
            void ApplyCorners()
            {
                var rp = new GraphicsPath();
                int d = CORNER * 2;
                int pw = popup.Width, ph = popup.Height;
                rp.AddArc(0, 0, d, d, 180, 90);
                rp.AddArc(pw - d, 0, d, d, 270, 90);
                rp.AddArc(pw - d, ph - d, d, d, 0, 90);
                rp.AddArc(0, ph - d, d, d, 90, 90);
                rp.CloseFigure();
                popup.Region = new Region(rp);
            }
            ApplyCorners();

            //  Centre over the parent window 
            var parentForm = this.FindForm();
            Rectangle refRect;
            if (parentForm != null && parentForm.Visible)
                refRect = parentForm.Bounds;
            else
                refRect = screen;

            popup.Location = new Point(
                refRect.X + (refRect.Width - popup.Width) / 2,
                refRect.Y + (refRect.Height - popup.Height) / 2);

            // Clamp within working area
            int maxX = screen.Right - popup.Width;
            int maxY = screen.Bottom - popup.Height;
            popup.Location = new Point(
                Math.Max(screen.Left, Math.Min(popup.Left, maxX)),
                Math.Max(screen.Top, Math.Min(popup.Top, maxY)));

            //  Show 
            IWin32Window owner = (this.FindForm() as IWin32Window) ?? this;
            popup.ShowDialog(owner);
        }

        //  ATTACHMENT ROW
        private Panel BuildAttachmentRow(AnnouncementAttachment att, int rowWidth)
        {
            //  Colours & labels per file type 
            var (label, iconColor, bgColor) = att.FileType.ToLower() switch
            {
                "pdf" => ("PDF", Color.FromArgb(192, 40, 40), Color.FromArgb(255, 242, 242)),
                "docx" => ("DOC", Color.FromArgb(22, 96, 185), Color.FromArgb(232, 241, 255)),
                "pptx" => ("PPT", Color.FromArgb(195, 90, 18), Color.FromArgb(255, 242, 228)),
                "img" => ("IMG", Color.FromArgb(55, 148, 55), Color.FromArgb(230, 248, 230)),
                _ => ("FILE", Color.FromArgb(100, 100, 100), Color.FromArgb(240, 240, 240)),
            };

            // Maps to your Resources: pdf_icon.png, ppt_icon.png, paper-clip.png,
            // images(1).png, document_icon.png
            Image resourceIcon = att.FileType.ToLower() switch
            {
                "pdf" => TryGetResource("pdf_icon"),
                "docx" => TryGetResource("paper_clip"),          // paper-clip.png
                "pptx" => TryGetResource("ppt_icon"),
                "img" => TryGetResource("images_1"),           
                _ => TryGetResource("document_icon"),
            };

            //  Row panel 
            const int ROW_H = 60;
            const int ICON_S = 42;
            const int ICON_X = 10;
            const int ICON_Y = 9;
            const int TEXT_X = 62;
            const int BTN_H = 30;
            const int BTN_W = 74;
            const int BTN_GAP = 6;
            const int BTN_AREA = BTN_W * 2 + BTN_GAP + 16;  // total right reserve

            var row = new Panel
            {
                Size = new Size(rowWidth, ROW_H),
                BackColor = bgColor,
                Margin = new Padding(0),
            };

            // Rounded border painted — no Region clipping
            row.Paint += (s, pe) =>
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var rr = new Rectangle(0, 0, row.Width - 1, row.Height - 1);
                using var gp = new GraphicsPath();
                const int cr = 8;
                gp.AddArc(rr.X, rr.Y, cr, cr, 180, 90);
                gp.AddArc(rr.Right - cr, rr.Y, cr, cr, 270, 90);
                gp.AddArc(rr.Right - cr, rr.Bottom - cr, cr, cr, 0, 90);
                gp.AddArc(rr.X, rr.Bottom - cr, cr, cr, 90, 90);
                gp.CloseFigure();
                using var bg = new SolidBrush(bgColor);
                pe.Graphics.FillPath(bg, gp);
                using var border = new Pen(Color.FromArgb(205, 205, 205), 1f);
                pe.Graphics.DrawPath(border, gp);
            };

            //  Icon badge 
            if (resourceIcon != null)
            {
                // Real resource image inside a coloured rounded box
                var iconBox = new Panel
                {
                    Size = new Size(ICON_S, ICON_S),
                    Location = new Point(ICON_X, ICON_Y),
                    BackColor = iconColor,
                };
                MakeRoundedRegion(iconBox, 8);

                var pic = new PictureBox
                {
                    Size = new Size(ICON_S - 10, ICON_S - 10),
                    Location = new Point(5, 5),
                    Image = resourceIcon,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Transparent,
                };
                iconBox.Controls.Add(pic);
                row.Controls.Add(iconBox);
            }
            else
            {
                // Fallback: text label badge
                var badge = new Label
                {
                    AutoSize = false,
                    Size = new Size(ICON_S, ICON_S),
                    Location = new Point(ICON_X, ICON_Y),
                    Text = label,
                    Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = iconColor,
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                MakeRoundedRegion(badge, 8);
                row.Controls.Add(badge);
            }

            //  File name 
            int nameW = rowWidth - TEXT_X - BTN_AREA - 4;
            row.Controls.Add(new Label
            {
                AutoSize = false,
                Size = new Size(nameW, 22),
                Location = new Point(TEXT_X, 10),
                Text = att.FileName,
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 20, 20),
                AutoEllipsis = true,
                BackColor = Color.Transparent,
            });

            //  Type + size 
            row.Controls.Add(new Label
            {
                AutoSize = true,
                Location = new Point(TEXT_X, 34),
                Text = att.FileType.ToUpper() + "  •  " + att.SizeLabel,
                Font = new Font("Segoe UI", 7.5f),
                ForeColor = Color.FromArgb(105, 105, 105),
                BackColor = Color.Transparent,
            });

            // ── Buttons (View + Save) 
            int btnY = (ROW_H - BTN_H) / 2;
            int btnSaveX = rowWidth - BTN_W - 8;
            int btnViewX = btnSaveX - BTN_W - BTN_GAP;

            // View button
            var btnView = new Button
            {
                Size = new Size(BTN_W, BTN_H),
                Location = new Point(btnViewX, btnY),
                Text = "👁  View",
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = iconColor,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                TabStop = false,
            };
            btnView.FlatAppearance.BorderSize = 0;
            btnView.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(iconColor, 0.12f);
            btnView.Click += (s, e) => HandleViewFile(att);
            row.Controls.Add(btnView);

            // Save button
            var btnSave = new Button
            {
                Size = new Size(BTN_W, BTN_H),
                Location = new Point(btnSaveX, btnY),
                Text = "⬇  Save",
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = iconColor,
                Cursor = Cursors.Hand,
                TabStop = false,
            };
            btnSave.FlatAppearance.BorderColor = iconColor;
            btnSave.FlatAppearance.BorderSize = 1;
            btnSave.FlatAppearance.MouseOverBackColor = Color.FromArgb(18, iconColor.R, iconColor.G, iconColor.B);
            btnSave.Click += (s, e) => HandleSaveFile(att);
            row.Controls.Add(btnSave);

            return row;
        }

        // ── Try to get a resource image by name (returns null if not found) ──
        private static Image TryGetResource(string name)
        {
            try
            {
                var prop = typeof(Properties.Resources).GetProperty(
                    name,
                    System.Reflection.BindingFlags.Static |
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.NonPublic);
                return prop?.GetValue(null) as Image;
            }
            catch { return null; }
        }

        //  View: open with the system's default viewer 
        private static void HandleViewFile(AnnouncementAttachment att)
        {
            // If you store real file paths in att, replace the MessageBox with:
            //   System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            //   { FileName = att.FilePath, UseShellExecute = true });
            MessageBox.Show(
                $"Opening \"" + att.FileName + "\" with the default viewer.\n\n"
                + "To activate: set att.FilePath and call Process.Start().",
                "View File", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ── Save: opens SaveFileDialog with the correct extension pre-filled ──
        private static void HandleSaveFile(AnnouncementAttachment att)
        {
            string ext = att.FileType.ToLower() switch
            {
                "pdf" => "pdf",
                "docx" => "docx",
                "pptx" => "pptx",
                "img" => "png",
                _ => "bin",
            };
            string filter = att.FileType.ToLower() switch
            {
                "pdf" => "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
                "docx" => "Word Documents (*.docx)|*.docx|All Files (*.*)|*.*",
                "pptx" => "PowerPoint Files (*.pptx)|*.pptx|All Files (*.*)|*.*",
                "img" => "Image Files (*.png;*.jpg)|*.png;*.jpg|All Files (*.*)|*.*",
                _ => "All Files (*.*)|*.*",
            };

            using var dlg = new SaveFileDialog
            {
                Title = "Save File",
                FileName = att.FileName,
                Filter = filter,
                DefaultExt = ext,
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                // To activate: copy the actual file bytes to dlg.FileName
                // File.Copy(att.FilePath, dlg.FileName, overwrite: true);
                MessageBox.Show(
                    $"Ready to save to:\n" + dlg.FileName + "\n\n"
                    + "To activate: wire att.FilePath and use File.Copy().",
                    "Save File", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        
        private void BuildPinnedPanel()
        {
            flpPinned.Controls.Clear();
            flpPinned.FlowDirection = FlowDirection.TopDown;
            flpPinned.WrapContents = false;
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

                EventHandler open = (s, ev) => ShowDetailView(a);
                row.Click += open;
                rowTitle.Click += open;
                flpPinned.Controls.Add(row);
            }
        }

        //  CATEGORY PANEL
        private void BuildCategoryPanel()
        {
            flpCategories.Controls.Clear();
            flpCategories.FlowDirection = FlowDirection.TopDown;
            flpCategories.WrapContents = false;
            flpCategories.AutoScroll = true;

            var cats = new[]
            {
                "All Categories","General","Academic","Administrative",
                "Events","Examinations","Schedule","Urgent"
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

        //  INSIGHTS PANEL
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
            int withFiles = _announcements.Count(a => a.Attachments.Count > 0);

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

            AddRow("Total", total.ToString(), 30, Color.FromArgb(50, 50, 50));
            AddRow("Unread", unread.ToString(), 58, unread > 0 ? Color.FromArgb(200, 0, 0) : Color.Green);
            AddRow("Pinned", pinned.ToString(), 86, Color.FromArgb(150, 100, 0));
            AddRow("Read", (total - unread).ToString(), 114, Color.FromArgb(22, 163, 74));
            AddRow("With Files", withFiles.ToString(), 142, Color.FromArgb(50, 100, 180));
        }

        //  SEED DATA  (with attachments)
        private void SeedAnnouncements()
        {
            _announcements = new List<StudentAnnouncement>
            {
                new() {
                    Id = 1, Title = "Updated Travel Reimbursement Policy",
                    Description    = "Please note that the mileage reimbursement rate for university-related travel has been adjusted. All reimbursement claims submitted after May 1, 2026 must use the new rate of ₱12.00 per km.",
                    Category = "Administrative", CourseName = "", OfficeName = "Admin Office", InstructorName = "Dr. Reyes",
                    Date = new DateTime(2026, 4, 15, 10, 30, 0), IsUrgent = true, IsPinned = true, IsRead = false,
                    Attachments = new()
                    {
                        new() { FileName = "Travel_Reimbursement_Policy_2026.pdf",  FileType = "pdf",  FileSizeBytes = 512_000 },
                        new() { FileName = "Reimbursement_Form_v3.docx",             FileType = "docx", FileSizeBytes = 124_000 },
                    },
                },
                new() {
                    Id = 2, Title = "Midterm Examination Schedule Released",
                    Description    = "The official midterm examination schedule for all BSIT 2nd year subjects is now available. Please check the LMS for your room assignments and bring your student ID on exam day.",
                    Category = "Examinations", CourseName = "BSIT 2nd Year", OfficeName = "Registrar's Office", InstructorName = "Prof. Santos",
                    Date = new DateTime(2026, 4, 20, 8, 0, 0), IsUrgent = true, IsPinned = true, IsRead = false,
                    Attachments = new()
                    {
                        new() { FileName = "Midterm_Schedule_AY2026.pdf",   FileType = "pdf",  FileSizeBytes = 320_000 },
                        new() { FileName = "Room_Assignments_Midterm.xlsx", FileType = "docx", FileSizeBytes = 88_000  },
                    },
                },
                new() {
                    Id = 3, Title = "Programming 1 – Lab Activity This Friday",
                    Description    = "Bring your laptops for the graded lab activity covering Modules 4 and 5. The activity will be conducted using Visual Studio 2022. No borrowing of equipment will be allowed.",
                    Category = "Academic", CourseName = "CC111 – Programming 1", OfficeName = "CCIS Department", InstructorName = "Prof. Santos",
                    Date = new DateTime(2026, 4, 18, 9, 0, 0), IsUrgent = false, IsPinned = false, IsRead = true,
                    Attachments = new()
                    {
                        new() { FileName = "Lab_Activity_4_Instructions.pdf", FileType = "pdf",  FileSizeBytes = 210_000 },
                        new() { FileName = "StarterCode_Module4.zip",         FileType = "docx", FileSizeBytes = 1_048_576 },
                    },
                },
                new() {
                    Id = 4, Title = "PUP Foundation Day Celebration – May 17",
                    Description    = "Join us for the PUP Foundation Day celebration. Activities include a student showcase, cultural performances, and a technology exhibit. Attendance is encouraged for all students.",
                    Category = "Events", CourseName = "", OfficeName = "Student Affairs Office", InstructorName = "Dr. Cruz",
                    Date = new DateTime(2026, 4, 10, 14, 0, 0), IsUrgent = false, IsPinned = false, IsRead = false,
                    Attachments = new()
                    {
                        new() { FileName = "Foundation_Day_Program.pptx",  FileType = "pptx", FileSizeBytes = 2_097_152 },
                        new() { FileName = "Event_Poster_2026.png",        FileType = "img",  FileSizeBytes = 450_000  },
                    },
                },
                new() {
                    Id = 5, Title = "Reminder: Submit Assignment Outputs",
                    Description    = "All pending assignment outputs for Information Management must be submitted via the LMS portal before May 15 at 11:59 PM. Late submissions will not be accepted under any circumstance.",
                    Category = "Academic", CourseName = "IT222 – Information Management", OfficeName = "CCIS Department", InstructorName = "Prof. Santos",
                    Date = new DateTime(2026, 4, 8, 11, 0, 0), IsUrgent = false, IsPinned = false, IsRead = true,
                    Attachments = new(),
                },
                new() {
                    Id = 6, Title = "Library Hours Extended Until June",
                    Description    = "The university library will extend its operating hours to 8:00 AM – 9:00 PM on weekdays starting May 1 through June 30, 2026 to support students during the examination period.",
                    Category = "General", CourseName = "", OfficeName = "Library Services", InstructorName = "Librarian Gomez",
                    Date = new DateTime(2026, 4, 5, 7, 30, 0), IsUrgent = false, IsPinned = false, IsRead = false,
                    Attachments = new()
                    {
                        new() { FileName = "Library_Extended_Hours_Notice.pdf", FileType = "pdf", FileSizeBytes = 95_000 },
                    },
                },
                new() {
                    Id = 7, Title = "Enrollment for 2nd Semester Now Open",
                    Description    = "Online enrollment for the 2nd semester of Academic Year 2026–2027 is now open. Students must settle all outstanding balances and secure their assessment forms before enrolling.",
                    Category = "General", CourseName = "", OfficeName = "Registrar's Office", InstructorName = "Registrar Dela Torre",
                    Date = new DateTime(2026, 5, 2, 8, 0, 0), IsUrgent = false, IsPinned = false, IsRead = false,
                    Attachments = new()
                    {
                        new() { FileName = "Enrollment_Guide_2ndSem.pdf",   FileType = "pdf",  FileSizeBytes = 680_000  },
                        new() { FileName = "Assessment_Form_Template.docx", FileType = "docx", FileSizeBytes = 120_000  },
                    },
                },
                new() {
                    Id = 8, Title = "Scholarship Application Deadline – May 20",
                    Description    = "All students applying for the CHED Scholarship and PUP Internal Scholarship must submit their complete documentary requirements to the Scholarship Office no later than May 20, 2026 at 5:00 PM.",
                    Category = "Administrative", CourseName = "", OfficeName = "Scholarship Office", InstructorName = "Dr. Valdez",
                    Date = new DateTime(2026, 5, 5, 9, 0, 0), IsUrgent = true, IsPinned = false, IsRead = false,
                    Attachments = new()
                    {
                        new() { FileName = "Scholarship_Requirements_Checklist.pdf", FileType = "pdf",  FileSizeBytes = 400_000 },
                        new() { FileName = "Application_Form_CHED_2026.docx",        FileType = "docx", FileSizeBytes = 190_000 },
                    },
                },
                new() {
                    Id = 9, Title = "Campus-Wide Maintenance: May 14 (No Classes)",
                    Description    = "There will be no classes on May 14, 2026 due to scheduled campus-wide electrical maintenance. All LMS deadlines falling on this date are automatically extended by 24 hours.",
                    Category = "Schedule", CourseName = "", OfficeName = "Facilities Management Office", InstructorName = "Engr. Bautista",
                    Date = new DateTime(2026, 5, 8, 7, 0, 0), IsUrgent = true, IsPinned = true, IsRead = false,
                    Attachments = new(),
                },
                new() {
                    Id = 10, Title = "Intramural Sports Registration Open",
                    Description    = "Registration for the Annual PUP Intramural Sports Festival is now open. Students interested in joining basketball, volleyball, badminton, or swimming events must register through their respective department coordinators before May 22, 2026.",
                    Category = "Events", CourseName = "", OfficeName = "Student Affairs Office", InstructorName = "Coach Mendoza",
                    Date = new DateTime(2026, 5, 6, 10, 0, 0), IsUrgent = false, IsPinned = false, IsRead = true,
                    Attachments = new()
                    {
                        new() { FileName = "Intramural_Sports_Registration_Form.docx", FileType = "docx", FileSizeBytes = 150_000 },
                        new() { FileName = "Sports_Fest_Schedule_2026.pptx",           FileType = "pptx", FileSizeBytes = 3_145_728 },
                    },
                },
                new() {
                    Id = 11, Title = "Final Examination Coverage Posted",
                    Description    = "The official final examination coverage for all BSIT subjects has been posted on the LMS. Students are advised to review the coverage carefully and contact their respective professors for any clarifications. Good luck on your finals!",
                    Category = "Examinations", CourseName = "BSIT – All Subjects", OfficeName = "CCIS Department", InstructorName = "Prof. Santos",
                    Date = new DateTime(2026, 5, 9, 8, 30, 0), IsUrgent = false, IsPinned = false, IsRead = false,
                    Attachments = new()
                    {
                        new() { FileName = "Finals_Coverage_BSIT_1stYear.pdf",  FileType = "pdf", FileSizeBytes = 760_000 },
                        new() { FileName = "Finals_Coverage_BSIT_2ndYear.pdf",  FileType = "pdf", FileSizeBytes = 820_000 },
                        new() { FileName = "Study_Guide_Summary.docx",          FileType = "docx", FileSizeBytes = 210_000 },
                    },
                },
                new() {
                    Id = 12, Title = "Academic Integrity Seminar – May 16",
                    Description    = "All students are required to attend the Academic Integrity and Anti-Plagiarism Seminar on May 16, 2026 at 1:00 PM via Zoom. Attendance will be recorded and counted toward your class standing.",
                    Category = "Academic", CourseName = "All BSIT Sections", OfficeName = "CCIS Department", InstructorName = "Dr. Reyes",
                    Date = new DateTime(2026, 5, 10, 9, 0, 0), IsUrgent = true, IsPinned = false, IsRead = false,
                    Attachments = new()
                    {
                        new() { FileName = "Academic_Integrity_Seminar_Slides.pptx", FileType = "pptx", FileSizeBytes = 4_194_304 },
                        new() { FileName = "Zoom_Meeting_Details.pdf",               FileType = "pdf",  FileSizeBytes = 60_000   },
                    },
                },
                new() {
                    Id = 13, Title = "Lost & Found: Items at Security Office",
                    Description    = "Several items including IDs, umbrellas, and a laptop bag have been turned over to the Security Office near Gate 1. Owners may claim their belongings by presenting valid identification.",
                    Category = "General", CourseName = "", OfficeName = "Security Office", InstructorName = "Guard Navarro",
                    Date = new DateTime(2026, 5, 7, 14, 0, 0), IsUrgent = false, IsPinned = false, IsRead = true,
                    Attachments = new()
                    {
                        new() { FileName = "LostAndFound_Photo_May7.jpg", FileType = "img", FileSizeBytes = 380_000 },
                    },
                },
                new() {
                    Id = 14, Title = "IT Career Fair – May 22 at PUP Gymnasium",
                    Description    = "The PUP Career Services Office invites all BSIT students to the annual IT Career Fair on May 22, 2026 from 9:00 AM to 4:00 PM at the university gymnasium. Over 30 technology companies will be present.",
                    Category = "Events", CourseName = "BSIT – All Years", OfficeName = "Career Services Office", InstructorName = "Dr. Cruz",
                    Date = new DateTime(2026, 5, 8, 10, 0, 0), IsUrgent = false, IsPinned = true, IsRead = false,
                    Attachments = new()
                    {
                        new() { FileName = "IT_CareerFair_2026_Program.pdf",    FileType = "pdf",  FileSizeBytes = 530_000 },
                        new() { FileName = "Career_Fair_Poster_Official.png",   FileType = "img",  FileSizeBytes = 620_000 },
                        new() { FileName = "Resume_Workshop_Slides.pptx",       FileType = "pptx", FileSizeBytes = 2_621_440 },
                    },
                },
                new() {
                    Id = 15, Title = "Class Schedule Adjustment – May 13",
                    Description    = "Due to the university-wide convocation, all morning classes on May 13, 2026 are moved to the afternoon session. Students with afternoon conflicts are advised to coordinate with their respective professors.",
                    Category = "Schedule", CourseName = "", OfficeName = "Registrar's Office", InstructorName = "Registrar Dela Torre",
                    Date = new DateTime(2026, 5, 10, 7, 30, 0), IsUrgent = true, IsPinned = false, IsRead = false,
                    Attachments = new(),
                },
                new() {
                    Id = 16, Title = "Capstone Project Defense Schedule Released",
                    Description    = "The official schedule for 4th-year BSIT Capstone Project defenses has been published on the departmental bulletin board and LMS. All groups are required to submit their final manuscripts and slide decks at least three days before their assigned defense date.",
                    Category = "Academic", CourseName = "BSIT 4th Year – Capstone", OfficeName = "CCIS Department", InstructorName = "Prof. Santos",
                    Date = new DateTime(2026, 5, 11, 8, 0, 0), IsUrgent = false, IsPinned = true, IsRead = false,
                    Attachments = new()
                    {
                        new() { FileName = "Capstone_Defense_Schedule_2026.pdf",  FileType = "pdf",  FileSizeBytes = 420_000 },
                        new() { FileName = "Defense_Manuscript_Template.docx",    FileType = "docx", FileSizeBytes = 300_000 },
                        new() { FileName = "Capstone_Presentation_Guide.pptx",   FileType = "pptx", FileSizeBytes = 1_887_437 },
                    },
                },
            };
        }

        // ═════════════════════════════════════════════════════════════════════
        //  SNAPSHOT  (for responsive scaling)
        // ═════════════════════════════════════════════════════════════════════
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

        //  HELPERS
        private static int R(float v) => (int)Math.Round(v);


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