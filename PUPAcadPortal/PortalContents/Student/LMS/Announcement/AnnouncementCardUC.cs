using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Student.LMS
{
    //  ANNOUNCEMENT CARD 
    public partial class AnnouncementCardUC : UserControl
    {
        public event EventHandler<int> CardClicked;
        public event EventHandler<int> PinToggled;
        private int _announcementId;
        private bool _isRead;
        private bool _isPinned;
        private bool _isUrgent;
        private string _category = string.Empty;
        private Color _accentColor;

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

        // Resource filenames (without extension) as they appear in the Resources folder
        private static readonly Dictionary<string, string> CatResourceName = new()
        {
            ["General"] = "general1",          // general1.png
            ["Academic"] = "academic",           // academic.png
            ["Schedule"] = "schedule1",          // schedule1.png
            ["Events"] = "events1",            // events1.png
            ["Examinations"] = "examinationms1",     // examinationms1.png
            ["Administrative"] = "administrativive",   // administrativive.png
            ["Urgent"] = "urgent1",            // urgent1.png
        };

        public AnnouncementCardUC()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint, true);
            this.Cursor = Cursors.Hand;
            this.Height = 115;
        }

        //  PUBLIC LOAD METHOD
        // ═════════════════════════════════════════════════════════════════════
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
            int cardWidth)
        {
            _announcementId = id;
            _isRead = isRead;
            _isPinned = isPinned;
            _isUrgent = isUrgent;
            _category = category;

            _accentColor = (isUrgent || !isRead)
                ? Color.FromArgb(139, 0, 0)
                : CatIconColor.GetValueOrDefault(category, Color.FromArgb(90, 90, 200));

            this.Width = cardWidth;
            this.BackColor = isRead ? Color.White : Color.FromArgb(255, 249, 249);

            // ── Left accent bar ───────────────────────────────────────────────
            pnlAccentBar.BackColor = isRead
                ? Color.FromArgb(215, 215, 215)
                : Color.FromArgb(139, 0, 0);
            pnlAccentBar.Size = new Size(4, 115);
            pnlAccentBar.Location = new Point(0, 0);

            // ─────────────────────────────────────────────────────────────────
            //  ICON BLOCK — painted rounded square (no Region = no child clipping)
            // ─────────────────────────────────────────────────────────────────
            const int ICON_SZ = 50;
            const int IMG_PAD = 9;   // inner padding for the white icon image
            const int IMG_SZ = ICON_SZ - IMG_PAD * 2;
            const int CORNER_R = 10;

            pnlIconBlock.Size = new Size(ICON_SZ, ICON_SZ);
            pnlIconBlock.Location = new Point(14, (115 - ICON_SZ) / 2);
            pnlIconBlock.BackColor = Color.Transparent; // transparent; we paint it ourselves
            pnlIconBlock.Region = null;              // no region — children won't be clipped

            // Remove old Paint handlers then add fresh one
            Color paintAccent = _accentColor;
            pnlIconBlock.Paint -= IconBlockPaint;       // clear previous (in case of reload)
            void IconBlockPaint(object s, PaintEventArgs pe)
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var path = new GraphicsPath();
                int d = CORNER_R * 2;
                var rc = new Rectangle(0, 0, ICON_SZ - 1, ICON_SZ - 1);
                path.AddArc(rc.X, rc.Y, d, d, 180, 90);
                path.AddArc(rc.Right - d, rc.Y, d, d, 270, 90);
                path.AddArc(rc.Right - d, rc.Bottom - d, d, d, 0, 90);
                path.AddArc(rc.X, rc.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
                using var brush = new SolidBrush(paintAccent);
                pe.Graphics.FillPath(brush, path);
            }
            pnlIconBlock.Paint += IconBlockPaint;

            // Determine resource
            string resName = isUrgent
                ? "urgent1"
                : CatResourceName.GetValueOrDefault(category, "general1");
            Image resImg = TryGetResource(resName);

            // Hide fallback label by default
            lblIconLetter.Visible = false;

            // Remove old PictureBox children, add fresh one each Load call
            for (int ci = pnlIconBlock.Controls.Count - 1; ci >= 0; ci--)
                if (pnlIconBlock.Controls[ci] is PictureBox oldPic)
                {
                    oldPic.Image?.Dispose();
                    pnlIconBlock.Controls.RemoveAt(ci);
                }

            if (resImg != null)
            {
                // White silhouette — crisp against coloured background
                var whiteBmp = MakeTintedWhite(resImg, IMG_SZ);
                var pic = new PictureBox
                {
                    Size = new Size(IMG_SZ, IMG_SZ),
                    Location = new Point(IMG_PAD, IMG_PAD),
                    BackColor = Color.Transparent,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Image = whiteBmp,
                    TabStop = false,
                };
                pnlIconBlock.Controls.Add(pic);
                pic.BringToFront();
            }
            else
            {
                // Fallback: white letter initial
                lblIconLetter.Visible = true;
                lblIconLetter.Text = GetCategoryInitial(category, isUrgent);
                lblIconLetter.Font = new Font("Segoe UI", 14f, FontStyle.Bold);
                lblIconLetter.ForeColor = Color.White;
                lblIconLetter.BackColor = Color.Transparent;
                lblIconLetter.Dock = DockStyle.Fill;
                lblIconLetter.TextAlign = ContentAlignment.MiddleCenter;
            }

            pnlIconBlock.Invalidate();

            // ── Unread dot ────────────────────────────────────────────────────
            pnlUnreadDot.Visible = !isRead;
            pnlUnreadDot.BackColor = Color.FromArgb(139, 0, 0);
            pnlUnreadDot.Size = new Size(10, 10);
            pnlUnreadDot.Location = new Point(cardWidth - 20, 10);
            MakeCircle(pnlUnreadDot);

            // ── Category tag ──────────────────────────────────────────────────
            Color tagBg = CatBgColor.GetValueOrDefault(category, Color.FromArgb(230, 230, 245));
            lblCategoryTag.Text = isUrgent ? "⚠ URGENT" : category.ToUpper();
            lblCategoryTag.Font = new Font("Segoe UI", 7.5f, FontStyle.Bold);
            lblCategoryTag.ForeColor = _accentColor;
            lblCategoryTag.BackColor = tagBg;
            lblCategoryTag.Size = new Size(100, 20);
            lblCategoryTag.Location = new Point(74, 12);
            lblCategoryTag.TextAlign = ContentAlignment.MiddleCenter;
            MakeRoundedPanel(lblCategoryTag, 8);

            // ── Attachment badge ──────────────────────────────────────────────
            lblAttachBadge.Visible = attachmentCount > 0;
            if (attachmentCount > 0)
            {
                // Use paper-clip resource icon next to count
                Image clipImg = TryGetResource("paper_clip");
                lblAttachBadge.Text = clipImg == null
                    ? $"📎 {attachmentCount} file{(attachmentCount > 1 ? "s" : "")}"
                    : $"  {attachmentCount} file{(attachmentCount > 1 ? "s" : "")}";
                lblAttachBadge.Font = new Font("Segoe UI", 7.5f, FontStyle.Bold);
                lblAttachBadge.ForeColor = Color.FromArgb(40, 90, 175);
                lblAttachBadge.BackColor = Color.FromArgb(225, 238, 255);
                lblAttachBadge.Size = new Size(clipImg != null ? 100 : 90, 20);
                lblAttachBadge.Location = new Point(182, 12);
                lblAttachBadge.TextAlign = ContentAlignment.MiddleCenter;
                MakeRoundedPanel(lblAttachBadge, 8);

                if (clipImg != null)
                {
                    // Small clip icon to the left inside the badge
                    PictureBox clipPic = null;
                    foreach (Control c in lblAttachBadge.Controls)
                        if (c is PictureBox pb) { clipPic = pb; break; }
                    if (clipPic == null)
                    {
                        clipPic = new PictureBox
                        {
                            BackColor = Color.Transparent,
                            SizeMode = PictureBoxSizeMode.Zoom,
                            TabStop = false,
                            Size = new Size(13, 13),
                            Location = new Point(4, 3),
                        };
                        lblAttachBadge.Controls.Add(clipPic);
                    }
                    clipPic.Image = clipImg;
                }
            }

            // ── Title ─────────────────────────────────────────────────────────
            int contentEndX = cardWidth - 195;
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);
            lblTitle.ForeColor = isRead ? Color.FromArgb(55, 55, 55) : Color.FromArgb(15, 15, 15);
            lblTitle.Location = new Point(74, 37);
            lblTitle.Size = new Size(contentEndX - 74, 22);
            lblTitle.AutoEllipsis = true;
            lblTitle.BackColor = Color.Transparent;

            // ── Description ───────────────────────────────────────────────────
            lblDescription.Text = description;
            lblDescription.Font = new Font("Segoe UI", 8.5f);
            lblDescription.ForeColor = Color.FromArgb(105, 105, 105);
            lblDescription.Location = new Point(74, 62);
            lblDescription.Size = new Size(contentEndX - 74, 30);
            lblDescription.AutoEllipsis = true;
            lblDescription.BackColor = Color.Transparent;

            // ── Course chip ───────────────────────────────────────────────────
            lblCourseChip.Visible = !string.IsNullOrEmpty(courseName);
            if (!string.IsNullOrEmpty(courseName))
            {
                lblCourseChip.Text = "  " + courseName;
                lblCourseChip.Font = new Font("Segoe UI", 7.5f);
                lblCourseChip.ForeColor = Color.FromArgb(45, 100, 45);
                lblCourseChip.BackColor = Color.FromArgb(220, 245, 215);
                lblCourseChip.Location = new Point(74, 93);
                lblCourseChip.Size = new Size(Math.Min(200, contentEndX - 78), 17);
                lblCourseChip.TextAlign = ContentAlignment.MiddleLeft;
                lblCourseChip.Padding = new Padding(4, 0, 0, 0);
                MakeRoundedPanel(lblCourseChip, 6);

                // Course/book icon inside chip
                Image bookImg = TryGetResource("academic1");
                if (bookImg != null)
                {
                    PictureBox bookPic = null;
                    foreach (Control c in lblCourseChip.Controls)
                        if (c is PictureBox pb) { bookPic = pb; break; }
                    if (bookPic == null)
                    {
                        bookPic = new PictureBox
                        {
                            BackColor = Color.Transparent,
                            SizeMode = PictureBoxSizeMode.Zoom,
                            TabStop = false,
                            Size = new Size(11, 11),
                            Location = new Point(3, 3),
                        };
                        lblCourseChip.Controls.Add(bookPic);
                    }
                    bookPic.Image = bookImg;
                }
            }

            // ── Right meta ────────────────────────────────────────────────────
            int metaX = cardWidth - 188;

            // Calendar icon for date
            SetLabelWithIcon(lblDate,
                date.ToString("MMM d, yyyy  •  h:mm tt"),
                "calendar_1_0", 12,
                new Font("Segoe UI", 7.8f), Color.Gray, metaX, 36);

            // Building icon for office
            SetLabelWithIcon(lblOffice,
                officeName,
                "administrativive", 12,
                new Font("Segoe UI", 7.8f), Color.FromArgb(85, 85, 85), metaX, 57);

            // Person icon for instructor
            SetLabelWithIcon(lblInstructor,
                instructorName,
                "student_2_16", 12,
                new Font("Segoe UI", 7.8f), Color.FromArgb(85, 85, 85), metaX, 77);

            // ── Pin button — use marketing.png (pushpin) ──────────────────────
            btnPin.Size = new Size(30, 30);
            btnPin.Location = new Point(cardWidth - 40, 42);
            btnPin.FlatStyle = FlatStyle.Flat;
            btnPin.BackColor = Color.Transparent;
            btnPin.FlatAppearance.BorderSize = 0;
            btnPin.FlatAppearance.MouseOverBackColor = Color.FromArgb(20, 139, 0, 0);
            btnPin.Cursor = Cursors.Hand;

            Image pinImg = TryGetResource("marketing");
            if (pinImg != null)
            {
                btnPin.Text = string.Empty;
                btnPin.Image = isPinned
                    ? MakeTintedColor(pinImg, 30, Color.FromArgb(139, 0, 0))
                    : MakeTintedGray(pinImg, 30);
                btnPin.ImageAlign = ContentAlignment.MiddleCenter;
            }
            else
            {
                btnPin.Text = isPinned ? "📌" : "📍";
                btnPin.Font = new Font("Segoe UI Symbol", 11f);
                btnPin.ForeColor = isPinned ? Color.FromArgb(139, 0, 0) : Color.FromArgb(190, 190, 190);
            }

            WireEvents(btnPin);
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Helper: set label text + small leading icon image via PictureBox
        // ─────────────────────────────────────────────────────────────────────
        private static void SetLabelWithIcon(
            Label lbl, string text, string resName, int iconSize,
            Font font, Color foreColor, int x, int y)
        {
            Image img = TryGetResource(resName);

            lbl.Font = font;
            lbl.ForeColor = foreColor;
            lbl.AutoSize = true;
            lbl.BackColor = Color.Transparent;

            if (img != null)
            {
                lbl.Text = "      " + text;   // space for icon
                lbl.Location = new Point(x, y);

                PictureBox pic = null;
                foreach (Control c in lbl.Controls)
                    if (c is PictureBox pb) { pic = pb; break; }
                if (pic == null)
                {
                    pic = new PictureBox
                    {
                        BackColor = Color.Transparent,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        TabStop = false,
                        Size = new Size(iconSize, iconSize),
                    };
                    lbl.Controls.Add(pic);
                }
                pic.Image = img;
                pic.Location = new Point(0, (lbl.Height > 0 ? (lbl.Height - iconSize) / 2 : 1));
                pic.BringToFront();
            }
            else
            {
                lbl.Text = text;
                lbl.Location = new Point(x, y);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  WIRE EVENTS
        // ═════════════════════════════════════════════════════════════════════
        private bool _wired = false;
        private void WireEvents(Button pinBtn)
        {
            if (_wired) return;
            _wired = true;

            pinBtn.Click += (s, e) =>
            {
                _isPinned = !_isPinned;
                PinToggled?.Invoke(this, _announcementId);
            };

            foreach (Control c in this.Controls)
            {
                if (c == pinBtn) continue;
                c.Click += (s, e) => CardClicked?.Invoke(this, _announcementId);
            }
            this.Click += (s, e) => CardClicked?.Invoke(this, _announcementId);
        }

        // ═════════════════════════════════════════════════════════════════════
        //  PAINT
        // ═════════════════════════════════════════════════════════════════════
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using var pen = new Pen(Color.FromArgb(232, 232, 232), 1f);
            e.Graphics.DrawLine(pen, 0, this.Height - 1, this.Width, this.Height - 1);

            if (!_isRead)
            {
                using var brush = new LinearGradientBrush(
                    new Point(4, 0), new Point(52, 0),
                    Color.FromArgb(45, 139, 0, 0),
                    Color.Transparent);
                e.Graphics.FillRectangle(brush, 4, 0, 48, this.Height);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  IMAGE HELPERS
        // ═════════════════════════════════════════════════════════════════════

        // Load from Properties.Resources by name (reflection)
        private static Image TryGetResource(string name)
        {
            try
            {
                var prop = typeof(Properties.Resources).GetProperty(
                    name,
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                return prop?.GetValue(null) as Image;
            }
            catch { return null; }
        }

        // ── Core: resize source image to target size ──────────────────────────
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

        // ── Pixel-level recolor: replace every non-transparent pixel with
        //    targetColor, keeping the original alpha channel intact.
        //    This gives a crisp silhouette in any colour against any background.
        private static Bitmap RecolorToSilhouette(Image src, int size, Color targetColor)
        {
            var resized = ResizeImage(src, size);
            var result = new Bitmap(size, size, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                {
                    Color px = resized.GetPixel(x, y);
                    if (px.A < 8) { result.SetPixel(x, y, Color.Transparent); continue; }
                    // Preserve original alpha so anti-aliased edges stay smooth
                    result.SetPixel(x, y, Color.FromArgb(px.A, targetColor));
                }
            resized.Dispose();
            return result;
        }

        // ── White silhouette — used on coloured icon blocks ───────────────────
        private static Bitmap MakeTintedWhite(Image src, int size)
            => RecolorToSilhouette(src, size, Color.White);

        // ── Coloured silhouette — used for pin button active state ────────────
        private static Bitmap MakeTintedColor(Image src, int size, Color tint)
            => RecolorToSilhouette(src, size, tint);

        // ── Gray silhouette — used for pin button inactive state ──────────────
        private static Bitmap MakeTintedGray(Image src, int size)
            => RecolorToSilhouette(src, size, Color.FromArgb(170, 170, 170));

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
                _ => category.Length > 0
                    ? category[0].ToString().ToUpper() : "?",
            };
        }

        private static void MakeRoundedPanel(Control c, int radius)
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