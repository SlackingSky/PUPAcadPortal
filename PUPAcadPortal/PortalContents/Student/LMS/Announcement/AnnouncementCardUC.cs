using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Student.LMS
{
    
    public partial class AnnouncementCardUC : UserControl
    {
        // ── Events 
        public event EventHandler<int>? CardClicked;
        public event EventHandler<int>? PinToggled;

        // ── State 
        private int _announcementId;
        private bool _isRead;
        private bool _isPinned;
        private bool _isUrgent;
        private bool _isNew;
        private string _category = string.Empty;
        private Color _accentColor;


        // ── Category colours (same as before) 
        public static readonly Dictionary<string, Color> CatIconColor = new()
        {
            ["General"] = Color.FromArgb(55, 138, 221),
            ["Academic"] = Color.FromArgb(99, 153, 34),
            ["Schedule"] = Color.FromArgb(186, 117, 23),
            ["Events"] = Color.FromArgb(212, 83, 126),
            ["Examinations"] = Color.FromArgb(127, 119, 221),
            ["Administrative"] = Color.FromArgb(90, 90, 200),
            ["Urgent"] = Color.FromArgb(220, 50, 50),
        };
        public static readonly Dictionary<string, Color> CatBgColor = new()
        {
            ["General"] = Color.FromArgb(230, 241, 251),
            ["Academic"] = Color.FromArgb(234, 243, 222),
            ["Schedule"] = Color.FromArgb(250, 238, 218),
            ["Events"] = Color.FromArgb(251, 234, 240),
            ["Examinations"] = Color.FromArgb(238, 237, 254),
            ["Administrative"] = Color.FromArgb(230, 230, 245),
            ["Urgent"] = Color.FromArgb(255, 235, 235),
        };

        // Resource names for category icons
        private static readonly Dictionary<string, string> CatResourceName = new()
        {
            ["General"] = "general1",
            ["Academic"] = "academic",
            ["Schedule"] = "schedule1",
            ["Events"] = "events1",
            ["Examinations"] = "examinationms1",
            ["Administrative"] = "administrativive",
            ["Urgent"] = "urgent1",
        };

        // ── Fonts / colours 
        private static readonly Color Maroon = Color.FromArgb(139, 0, 0);
        private static readonly Color GridLine = Color.FromArgb(235, 235, 235);
        private static readonly Font FontTitle = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        private static readonly Font FontDesc = new Font("Segoe UI", 8.5f);
        private static readonly Font FontSmall = new Font("Segoe UI", 7.5f);
        private static readonly Font FontBadge = new Font("Segoe UI", 7f, FontStyle.Bold);
        private static readonly Font FontCat = new Font("Segoe UI", 7.5f, FontStyle.Bold);

        // ── Constructor 
        public AnnouncementCardUC()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
            Cursor = Cursors.Hand;
            Height = 110;
        }

        //  PUBLIC LOAD
        public void Load(
            int id,
            string title,
            string description,
            string category,
            string courseName,
            string officeName,
            string instructorName,
            DateTime date,
            bool isUrgent,
            bool isPinned,
            bool isRead,
            int attachmentCount,
            int cardWidth,
            bool isNew = false,
            int viewedCount = 0,
            int totalStudents = 40)
        {
            _announcementId = id;
            _isRead = isRead;
            _isPinned = isPinned;
            _isUrgent = isUrgent;
            _isNew = isNew;
            _category = category;

            _accentColor = (isUrgent || !isRead)
                ? Maroon
                : CatIconColor.GetValueOrDefault(category, Color.FromArgb(90, 90, 200));

            Width = cardWidth;
            BackColor = isRead ? Color.White : Color.FromArgb(255, 249, 249);

            // Remove all old dynamic controls (keep the designer stubs)
            SuspendLayout();
            Controls.Clear();
            ResumeLayout(false);

            BuildCard(title, description, category, courseName,
                      officeName, instructorName, date,
                      isUrgent, isPinned, isRead, attachmentCount,
                      cardWidth, isNew, viewedCount, totalStudents);

            WireClickEvents();
            Invalidate();
        }

        //  BUILD ALL CHILD CONTROLS
        private void BuildCard(
            string title,
            string description,
            string category,
            string courseName,
            string officeName,
            string instructorName,
            DateTime date,
            bool isUrgent,
            bool isPinned,
            bool isRead,
            int attachmentCount,
            int cardWidth,
            bool isNew,
            int viewedCount,
            int totalStudents)
        {
            const int ICON_SZ = 46;
            const int ICON_X = 10;
            const int ICON_Y = 10;
            const int TEXT_X = ICON_X + ICON_SZ + 10;

            Color iconCol = isUrgent
                ? Maroon
                : CatIconColor.GetValueOrDefault(category, Color.FromArgb(90, 90, 200));
            // Light pastel background — original icon image rendered as-is on top
            Color iconBg = isUrgent
                ? Color.FromArgb(255, 235, 235)
                : CatBgColor.GetValueOrDefault(category, Color.FromArgb(230, 230, 245));

            // ── Category icon block 
            var iconBlock = new Panel
            {
                Size = new Size(ICON_SZ, ICON_SZ),
                Location = new Point(ICON_X, ICON_Y),
                BackColor = iconBg,
            };
            MakeRoundedRegion(iconBlock, 8);

            string resName = isUrgent
                ? "urgent1"
                : CatResourceName.GetValueOrDefault(category, "general1");
            Image? resImg = TryGetResource(resName);

            if (resImg != null)
            {
                const int IMG_PAD = 6;
                int imgSz = ICON_SZ - IMG_PAD * 2;
                // Display the original icon image without any recoloring
                var pic = new PictureBox
                {
                    Size = new Size(imgSz, imgSz),
                    Location = new Point(IMG_PAD, IMG_PAD),
                    BackColor = Color.Transparent,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Image = ResizeImage(resImg, imgSz),
                    TabStop = false,
                };
                iconBlock.Controls.Add(pic);
            }
            else
            {
                var lbl = new Label
                {
                    Dock = DockStyle.Fill,
                    Text = GetCategoryInitial(category, isUrgent),
                    Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                    ForeColor = iconCol,
                    BackColor = Color.Transparent,
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                iconBlock.Controls.Add(lbl);
            }
            Controls.Add(iconBlock);

            // ── ROW 1: pin W + URGENT + Title 
            int rx = TEXT_X;
            int ry = 10;

            

            // "NEW" badge
            if (isNew || !isRead)
            {
                var newBadge = new Label
                {
                    Text = "NEW",
                    AutoSize = false,
                    Size = new Size(34, 18),
                    Location = new Point(rx, ry + 2),
                    Font = FontBadge,
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(34, 197, 94),
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                MakeRoundedRegion(newBadge, 6);
                Controls.Add(newBadge);
                rx += 40;
            }

            // "URGENT" pill
            if (isUrgent)
            {
                var urgentPill = new Label
                {
                    Text = "⚠ URGENT",
                    AutoSize = false,
                    Size = new Size(70, 18),
                    Location = new Point(rx, ry + 2),
                    Font = FontBadge,
                    ForeColor = Color.White,
                    BackColor = Maroon,
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                MakeRoundedRegion(urgentPill, 6);
                Controls.Add(urgentPill);
                rx += 76;
            }

            // Title
            int titleW = cardWidth - rx - 220; // leave room for right-side meta
            var lblTitle = new Label
            {
                Text = title,
                Location = new Point(rx, ry),
                Size = new Size(Math.Max(100, titleW), 22),
                Font = isRead ? new Font("Segoe UI", 9.5f) : FontTitle,
                ForeColor = isRead ? Color.FromArgb(55, 55, 55) : Color.FromArgb(15, 15, 15),
                AutoEllipsis = true,
                AutoSize = false,
                BackColor = Color.Transparent,
            };
            Controls.Add(lblTitle);

            //  ROW 2: Description 
            var lblDesc = new Label
            {
                Text = description,
                Location = new Point(TEXT_X, 35),
                Size = new Size(cardWidth - TEXT_X - 220, 32),
                Font = FontDesc,
                ForeColor = Color.FromArgb(85, 85, 85),
                AutoEllipsis = true,
                AutoSize = false,
                BackColor = Color.Transparent,
            };
            Controls.Add(lblDesc);

            //  ROW 3:  Viewed progress bar 
            int bottomY = 76;

            // Instructor label (left)
            var lblInstr = new Label
            {
                Text = "👤 " + instructorName,
                Location = new Point(TEXT_X, bottomY),
                AutoSize = true,
                Font = FontSmall,
                ForeColor = Color.FromArgb(100, 100, 100),
                BackColor = Color.Transparent,
            };
            Controls.Add(lblInstr);


            // ─ RIGHT META: Category pill + Date + ⋮ menu 
            int metaRight = cardWidth - 8;

            // Three-dot menu button
            /*var btnMenu = new Button
            {
                Text = "⋮",
                Size = new Size(24, 24),
                Location = new Point(metaRight - 24, 8),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(160, 160, 160),
                Cursor = Cursors.Hand,
                TabStop = false,
            };
            btnMenu.FlatAppearance.BorderSize = 0;
            Controls.Add(btnMenu);
            metaRight -= 28;*/


            // Pin emoji button
            var btnPin = new Button
            {
                Text = isPinned ? "📌" : "📍",
                Size = new Size(30, 30),
                Location = new Point(metaRight - 24, 8),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10f),
                BackColor = Color.Transparent,
                ForeColor = isPinned ? Maroon : Color.FromArgb(180, 180, 180),
                Cursor = Cursors.Hand,
                TabStop = false,
            };
            btnPin.FlatAppearance.BorderSize = 0;
            btnPin.FlatAppearance.MouseOverBackColor = Color.FromArgb(20, 139, 0, 0);
            btnPin.Click += (s, e) =>
            {
                _isPinned = !_isPinned;
                PinToggled?.Invoke(this, _announcementId);
            };
            Controls.Add(btnPin);
            metaRight -= 28;

            // Date label
            string dateStr = date.ToString("MMM d, yyyy  h:mm tt");
            var lblDate = new Label
            {
                Text = dateStr,
                AutoSize = true,
                Font = FontSmall,
                ForeColor = Color.FromArgb(130, 130, 130),
                BackColor = Color.Transparent,
            };
            lblDate.Location = new Point(metaRight - 140, 14);
            Controls.Add(lblDate);

            // Category pill
            Color catCol = CatIconColor.GetValueOrDefault(category, Color.FromArgb(90, 90, 200));
            Color catBg = CatBgColor.GetValueOrDefault(category, Color.FromArgb(230, 230, 245));
            string catText = isUrgent ? "Urgent" : category;
            int catW = TextRenderer.MeasureText(catText, FontCat).Width + 20;

            var lblCat = new Label
            {
                Text = catText,
                AutoSize = false,
                Size = new Size(catW, 22),
                Location = new Point(metaRight - 140, 38),
                Font = FontCat,
                ForeColor = catCol,
                BackColor = catBg,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            MakeRoundedRegion(lblCat, 8);
            Controls.Add(lblCat);

            // Attachment badge (small, bottom-right area)
            if (attachmentCount > 0)
            {
                var lblAttach = new Label
                {
                    Text = $"📎 {attachmentCount}",
                    AutoSize = true,
                    Location = new Point(metaRight - 140, bottomY),
                    Font = FontSmall,
                    ForeColor = Color.FromArgb(40, 90, 175),
                    BackColor = Color.Transparent,
                };
                Controls.Add(lblAttach);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  WIRE CLICK EVENTS (all non-pin children fire CardClicked)
        // ═════════════════════════════════════════════════════════════════════
        private void WireClickEvents()
        {
            foreach (Control c in Controls)
            {
                // Skip pin button — it has its own handler
                if (c is Button btn && btn.Text is "📌" or "📍") continue;
                // Skip menu button
                if (c is Button mb && mb.Text == "⋮") continue;
                c.Click += (s, e) => CardClicked?.Invoke(this, _announcementId);
            }
            Click += (s, e) => CardClicked?.Invoke(this, _announcementId);
        }

        // ═════════════════════════════════════════════════════════════════════
        //  PAINT — bottom border + subtle unread glow
        // ═════════════════════════════════════════════════════════════════════
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Bottom separator line
            using var pen = new Pen(GridLine, 1f);
            e.Graphics.DrawLine(pen, 0, Height - 1, Width, Height - 1);


        }

        // ═════════════════════════════════════════════════════════════════════
        //  IMAGE HELPERS
        // ═════════════════════════════════════════════════════════════════════
        private static Image? TryGetResource(string name)
        {
            try
            {
                var prop = typeof(Properties.Resources).GetProperty(
                    name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                return prop?.GetValue(null) as Image;
            }
            catch { return null; }
        }

        private static Bitmap ResizeImage(Image src, int size)
        {
            var bmp = new Bitmap(size, size, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.Transparent);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.DrawImage(src, 0, 0, size, size);
            return bmp;
        }

        /// <summary>
        /// Recolor every opaque pixel to <paramref name="targetColor"/>,
        /// preserving the original alpha for smooth anti-aliased edges.
        /// </summary>
        private static Bitmap RecolorToSilhouette(Image src, int size, Color targetColor)
        {
            var resized = ResizeImage(src, size);
            var result = new Bitmap(size, size, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                {
                    Color px = resized.GetPixel(x, y);
                    if (px.A < 8) { result.SetPixel(x, y, Color.Transparent); continue; }
                    result.SetPixel(x, y, Color.FromArgb(px.A, targetColor));
                }
            resized.Dispose();
            return result;
        }

        private static string GetCategoryInitial(string category, bool isUrgent)
        {
            if (isUrgent) return "!";
            return category switch
            {
                "General" => "G",
                "Academic" => "A",
                "Schedule" => "S",
                "Events" => "E",
                "Examinations" => "X",
                "Administrative" => "AD",
                "Urgent" => "!",
                _ => category.Length > 0 ? category[0].ToString().ToUpper() : "?",
            };
        }

        // ═════════════════════════════════════════════════════════════════════
        //  SHAPE HELPERS
        // ═════════════════════════════════════════════════════════════════════
        private static void MakeRoundedRegion(Control c, int radius)
        {
            var path = new GraphicsPath();
            var r = new Rectangle(0, 0, c.Width, c.Height);
            int d = radius * 2;
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            c.Region = new Region(path);
        }
    }
}