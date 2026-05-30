using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class AnnouncementLayout : UserControl
    {
        // ── Public events ────────────────────────────────────────────────────
        public event EventHandler<int>? CardClicked;
        public event EventHandler<int>? PinToggled;          // student portal
        public event EventHandler<int>? MenuEditClicked;     // instructor portal
        public event EventHandler<int>? MenuToggleClicked;   // instructor portal
        public event EventHandler<int>? MenuDeleteClicked;   // instructor portal

        // ── Per-category colours (shared palette) ────────────────────────────
        private static readonly Dictionary<string, Color> CatIconColor = new()
        {
            ["General"] = Color.FromArgb(0x37, 0x8a, 0xdd),
            ["Academic"] = Color.FromArgb(0x63, 0x99, 0x22),
            ["Schedule"] = Color.FromArgb(0xba, 0x75, 0x17),
            ["Events"] = Color.FromArgb(0xd4, 0x53, 0x7e),
            ["Examinations"] = Color.FromArgb(0x7f, 0x77, 0xdd),
            ["Administrative"] = Color.FromArgb(90, 90, 200),
            ["Urgent"] = Color.FromArgb(220, 50, 50),
        };

        private static readonly Dictionary<string, Color> CatBgColor = new()
        {
            ["General"] = Color.FromArgb(0xe6, 0xf1, 0xfb),
            ["Academic"] = Color.FromArgb(0xea, 0xf3, 0xde),
            ["Schedule"] = Color.FromArgb(0xfa, 0xee, 0xda),
            ["Events"] = Color.FromArgb(0xfb, 0xea, 0xf0),
            ["Examinations"] = Color.FromArgb(0xee, 0xed, 0xfe),
            ["Administrative"] = Color.FromArgb(230, 230, 245),
            ["Urgent"] = Color.FromArgb(255, 235, 235),
        };

        // ── Stored id for event routing ──────────────────────────────────────
        private int _announcementId;
        private bool _isStudent;

        public AnnouncementLayout()
        {
            InitializeComponent();
        }

        // ════════════════════════════════════════════════════════════════════
        //  PUBLIC LOAD METHODS
        // ════════════════════════════════════════════════════════════════════

        /// <summary>Load card in Instructor mode.</summary>
        public void LoadInstructor(
            int id,
            string title,
            string description,
            string category,
            string status,
            string instructorName,
            DateTime date,
            bool isPinned,
            bool isUrgent,
            int viewedCount,
            int totalStudents,
            int cardWidth)
        {
            _announcementId = id;
            _isStudent = false;
            this.Width = cardWidth;

            BuildInstructorCard(
                id, title, description, category, status,
                instructorName, date, isPinned, isUrgent,
                viewedCount, totalStudents, cardWidth);
        }

        /// <summary>Load card in Student mode.</summary>
        public void LoadStudent(
            int id,
            string title,
            string description,
            string category,
            string officeName,
            DateTime date,
            bool isUrgent,
            bool isPinned,
            bool isRead,
            int cardWidth,
            string instructorName = "")
        {
            _announcementId = id;
            _isStudent = true;
            this.Width = cardWidth;

            BuildStudentCard(
                id, title, description, category,
                officeName, date, isUrgent, isPinned, isRead, cardWidth, instructorName);
        }

        // ════════════════════════════════════════════════════════════════════
        //  INSTRUCTOR CARD BUILDER
        // ════════════════════════════════════════════════════════════════════
        private void BuildInstructorCard(
            int id, string title, string description, string category,
            string status, string instructorName, DateTime date,
            bool isPinned, bool isUrgent, int viewedCount, int totalStudents,
            int panelWidth)
        {
            this.Controls.Clear();
            this.Height = 100;
            this.BackColor = status == "active" ? Color.White : Color.FromArgb(250, 250, 250);
            this.Margin = new Padding(6, 4, 6, 4);
            this.Tag = false;

            this.Paint += (s, e) => DrawCardBorder(e.Graphics, this);

            bool isNew = (DateTime.Now - date) < TimeSpan.FromDays(1);
            Color iconCol = CatIconColor.GetValueOrDefault(category, Color.Gray);
            Color iconBg = CatBgColor.GetValueOrDefault(category, Color.WhiteSmoke);

            if (isNew)
            {
                var ribbon = new Label
                {
                    AutoSize = false,
                    Size = new Size(38, 17),
                    Location = new Point(0, 0),
                    BackColor = Color.FromArgb(22, 163, 74),
                    ForeColor = Color.White,
                    Text = "NEW",
                    Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                MakeRoundedRegion(ribbon, 6);
                this.Controls.Add(ribbon);
                ribbon.BringToFront();
            }

            var iconCircle = new Panel
            {
                Size = new Size(42, 42),
                Location = new Point(12, 14),
                BackColor = iconBg,
            };
            iconCircle.Paint += (s, pe) =>
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var b = new SolidBrush(iconBg);
                pe.Graphics.FillEllipse(b, 1, 1, iconCircle.Width - 3, iconCircle.Height - 3);
                Image? catImg = GetCategoryImage(category);
                if (catImg != null)
                {
                    int pad = 8;
                    pe.Graphics.DrawImage(catImg, new Rectangle(pad, pad, iconCircle.Width - pad * 2, iconCircle.Height - pad * 2));
                }
                else
                {
                    string letter = category.Length > 0 ? category[..1] : "?";
                    using var font = new Font("Segoe UI", 13f, FontStyle.Bold);
                    using var tb = new SolidBrush(iconCol);
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    pe.Graphics.DrawString(letter, font, tb, new RectangleF(0, 0, iconCircle.Width, iconCircle.Height), sf);
                }
            };
            this.Controls.Add(iconCircle);

            int textX = 64;
            var dateFont = new Font("Segoe UI", 8f);
            string dateText = date.ToString("MMM d, yyyy  h:mm tt");
            int dateLabelW = TextRenderer.MeasureText(dateText, dateFont).Width + 4;
            const int menuBtnW = 28;
            const int menuBtnRight = 14;
            int menuBtnX = panelWidth - menuBtnRight - menuBtnW;
            int dateLabelX = menuBtnX - dateLabelW - 6;

            int titleOffsetX = 0;
            if (isPinned)
            {
                var pin = new Label
                {
                    AutoSize = true,
                    Text = "📌",
                    Font = new Font("Segoe UI", 9f),
                    Location = new Point(textX, 14),
                    BackColor = Color.Transparent,
                };
                this.Controls.Add(pin);
                titleOffsetX = 20;
            }
            if (isUrgent)
            {
                var urgIconBox = new Panel
                {
                    Size = new Size(18, 18),
                    Location = new Point(textX + titleOffsetX, 13),
                    BackColor = Color.Transparent,
                };
                urgIconBox.Paint += (s, pe) =>
                {
                    pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    try { pe.Graphics.DrawImage(Properties.Resources.urgent1, new Rectangle(0, 0, urgIconBox.Width, urgIconBox.Height)); } catch { }
                };
                this.Controls.Add(urgIconBox);
                titleOffsetX += 22;

                var urgentBadge = new Label
                {
                    AutoSize = false,
                    Size = new Size(52, 18),
                    Location = new Point(textX + titleOffsetX, 14),
                    Text = "URGENT",
                    Font = new Font("Segoe UI", 7f, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.Firebrick,
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                MakeRoundedRegion(urgentBadge, 9);
                this.Controls.Add(urgentBadge);
                titleOffsetX += 58;
            }

            var pillFont = new Font("Segoe UI", 7.5f, FontStyle.Bold);
            int pillW = TextRenderer.MeasureText(category, pillFont).Width + 14;
            int inactiveBadgeW = status != "active" ? 58 + 6 : 0;
            int titleMaxRight = dateLabelX - inactiveBadgeW - pillW - 12;
            int titleW = Math.Max(60, titleMaxRight - (textX + titleOffsetX));

            var lblTitle = new Label
            {
                AutoSize = false,
                Size = new Size(titleW, 20),
                Location = new Point(textX + titleOffsetX, 14),
                Text = title,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 25, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent,
            };
            this.Controls.Add(lblTitle);

            var catPill = new Label
            {
                AutoSize = false,
                Size = new Size(pillW, 18),
                Location = new Point(lblTitle.Right + 6, 16),
                Text = category,
                Font = pillFont,
                ForeColor = iconCol,
                BackColor = iconBg,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            MakeRoundedRegion(catPill, 9);
            this.Controls.Add(catPill);

            if (status != "active")
            {
                var inactiveBadge = new Label
                {
                    AutoSize = false,
                    Size = new Size(58, 18),
                    Location = new Point(catPill.Right + 6, 16),
                    Text = "Inactive",
                    Font = new Font("Segoe UI", 7.5f),
                    ForeColor = Color.FromArgb(100, 100, 100),
                    BackColor = Color.FromArgb(230, 230, 230),
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                MakeRoundedRegion(inactiveBadge, 9);
                this.Controls.Add(inactiveBadge);
            }

            var lblDate = new Label
            {
                AutoSize = false,
                Size = new Size(dateLabelW, 16),
                Location = new Point(dateLabelX, 17),
                Text = dateText,
                Font = dateFont,
                ForeColor = Color.FromArgb(130, 130, 130),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight,
            };
            this.Controls.Add(lblDate);

            int rightX = panelWidth - 14;
            var lblDesc = new Label
            {
                Name = "lblDesc",
                AutoSize = false,
                Size = new Size(rightX - textX - 10, 34),
                Location = new Point(textX, 37),
                Text = description,
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(90, 90, 90),
                BackColor = Color.Transparent,
            };
            this.Controls.Add(lblDesc);

            var lblAuthor = new Label
            {
                AutoSize = true,
                Location = new Point(textX, 74),
                Text = "👤 " + instructorName,
                Font = new Font("Segoe UI", 8f),
                ForeColor = Color.FromArgb(110, 110, 110),
                BackColor = Color.Transparent,
            };
            this.Controls.Add(lblAuthor);

            var lblViewed = new Label
            {
                AutoSize = true,
                Text = $"Viewed {viewedCount}/{totalStudents}",
                Font = new Font("Segoe UI", 8f),
                ForeColor = Color.FromArgb(110, 110, 110),
                BackColor = Color.Transparent,
                Location = new Point(lblAuthor.Location.X + 130, 74),
            };
            this.Controls.Add(lblViewed);

            int barX = lblViewed.Location.X + 100;
            int barW = Math.Max(40, rightX - barX - 120);
            int pct = totalStudents > 0 ? (int)Math.Round(viewedCount * 100.0 / totalStudents) : 0;

            var progressTrack = new Panel
            {
                Size = new Size(barW, 4),
                Location = new Point(barX, 78),
                BackColor = Color.FromArgb(220, 220, 220),
            };
            var progressFill = new Panel
            {
                Size = new Size((int)(barW * pct / 100.0), 4),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(139, 0, 0),
            };
            progressTrack.Controls.Add(progressFill);
            this.Controls.Add(progressTrack);

            const int pinBtnW = 32;
            int pinBtnX = panelWidth - menuBtnRight - menuBtnW - pinBtnW - 4;

            var btnPinToggle = new Button
            {
                Size = new Size(pinBtnW, 28),
                Location = new Point(pinBtnX, 12),
                FlatStyle = FlatStyle.Flat,
                Text = isPinned ? "📌" : "📍",
                Font = new Font("Segoe UI Symbol", 10f),
                ForeColor = isPinned ? Color.DarkRed : Color.Silver,
                Cursor = Cursors.Hand,
                TabStop = false,
                BackColor = Color.Transparent,
            };
            btnPinToggle.FlatAppearance.BorderSize = 0;
            btnPinToggle.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
            btnPinToggle.Click += (s, ev) => PinToggled?.Invoke(this, id);
            this.Controls.Add(btnPinToggle);

            var btnMenu = new Button
            {
                Size = new Size(menuBtnW, 28),
                Location = new Point(menuBtnX, 12),
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

            var ctx = new ContextMenuStrip();
            ctx.Items.Add("✏  Edit", null, (s, ev) => MenuEditClicked?.Invoke(this, id));
            ctx.Items.Add(isPinned ? "📌  Unpin" : "📌  Pin", null, (s, ev) => PinToggled?.Invoke(this, id));
            ctx.Items.Add(status == "active" ? "⏸  Set inactive" : "▶  Set active", null, (s, ev) => MenuToggleClicked?.Invoke(this, id));
            ctx.Items.Add(new ToolStripSeparator());
            ctx.Items.Add("🗑  Delete", null, (s, ev) =>
            {
                if (MessageBox.Show($"Delete \"{title}\"?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    MenuDeleteClicked?.Invoke(this, id);
            });
            btnMenu.Click += (s, ev) => ctx.Show(btnMenu, new Point(0, btnMenu.Height));
            this.Controls.Add(btnMenu);

            EventHandler viewHandler = (s, ev) => CardClicked?.Invoke(this, id);
            this.Click += viewHandler;
            lblTitle.Click += viewHandler;
            lblDesc.Click += viewHandler;
        }

        // ════════════════════════════════════════════════════════════════════
        //  STUDENT CARD BUILDER
        // ════════════════════════════════════════════════════════════════════
        private void BuildStudentCard(
            int id, string title, string description, string category,
            string officeName, DateTime date, bool isUrgent,
            bool isPinned, bool isRead, int cardWidth, string instructorName = "")
        {
            this.Controls.Clear();

            Color iconCol = CatIconColor.GetValueOrDefault(category, Color.Gray);
            Color iconBg = CatBgColor.GetValueOrDefault(category, Color.WhiteSmoke);

            bool useMaroonBg = !isRead;
            Color cardBg = useMaroonBg ? Color.FromArgb(139, 0, 0) : Color.White;
            Color textColor = useMaroonBg ? Color.White : Color.FromArgb(20, 20, 20);
            Color subColor = useMaroonBg ? Color.FromArgb(255, 210, 210) : Color.FromArgb(90, 90, 90);
            Color metaColor = useMaroonBg ? Color.FromArgb(255, 200, 200) : Color.Gray;

            const int indicatorW = 5;
            const int iconX = 16;
            const int iconSize = 48;
            const int textX = 76;
            const int rightColW = 220;
            const int cardPadB = 16;

            int descWidth = Math.Max(100, cardWidth - textX - rightColW - 10);

            this.Width = cardWidth;
            this.Height = 10;
            this.BackColor = cardBg;
            this.Margin = new Padding(4);
            this.Cursor = Cursors.Hand;

            var indicator = new Panel
            {
                Size = new Size(indicatorW, 10),
                Location = new Point(0, 0),
                BackColor = useMaroonBg ? Color.FromArgb(180, 0, 0) : (isUrgent ? Color.FromArgb(200, 0, 0) : iconCol),
            };
            this.Controls.Add(indicator);

            var iconCircle = new Panel
            {
                Size = new Size(iconSize, iconSize),
                Location = new Point(iconX, 20),
                BackColor = iconBg,
            };
            iconCircle.Paint += (s, pe) =>
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var bgBrush = new SolidBrush(iconBg);
                pe.Graphics.FillEllipse(bgBrush, 1, 1, iconCircle.Width - 3, iconCircle.Height - 3);
                Image? catImg = GetCategoryImage(category);
                if (catImg != null)
                {
                    int pad = 10;
                    pe.Graphics.DrawImage(catImg, new Rectangle(pad, pad, iconCircle.Width - pad * 2, iconCircle.Height - pad * 2));
                }
                else
                {
                    string letter = category.Length > 0 ? category[..1] : "?";
                    using var fnt = new Font("Segoe UI", 14f, FontStyle.Bold);
                    using var tb = new SolidBrush(iconCol);
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    pe.Graphics.DrawString(letter, fnt, tb, new RectangleF(0, 0, iconCircle.Width, iconCircle.Height), sf);
                }
            };
            this.Controls.Add(iconCircle);

            int currentY = 14;

            if (isUrgent)
            {
                var urgIconBox = new Panel
                {
                    Size = new Size(18, 18),
                    Location = new Point(textX, currentY + 1),
                    BackColor = Color.Transparent,
                };
                urgIconBox.Paint += (s, pe) =>
                {
                    pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    try { pe.Graphics.DrawImage(Properties.Resources.urgent1, new Rectangle(0, 0, urgIconBox.Width, urgIconBox.Height)); } catch { }
                };
                this.Controls.Add(urgIconBox);

                var urgBadge = new Label
                {
                    AutoSize = false,
                    Size = new Size(60, 20),
                    Location = new Point(textX + 22, currentY),
                    Text = "URGENT",
                    Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(200, 0, 0),
                    BackColor = Color.FromArgb(255, 235, 235),
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                MakeRoundedRegion(urgBadge, 8);
                this.Controls.Add(urgBadge);
                currentY += 26;
            }

            int titleOffsetX = 0;
            if (isPinned)
            {
                var pin = new Label
                {
                    AutoSize = true,
                    Text = "📌",
                    Font = new Font("Segoe UI", 9f),
                    Location = new Point(textX, currentY + 1),
                    BackColor = Color.Transparent,
                };
                this.Controls.Add(pin);
                titleOffsetX = 22;
            }

            if (!isRead)
            {
                var dot = new Panel
                {
                    Size = new Size(8, 8),
                    Location = new Point(textX - 14, currentY + 7),
                    BackColor = Color.White,
                };
                MakeCircle(dot);
                this.Controls.Add(dot);
            }

            int titleWidth = Math.Max(60, descWidth - titleOffsetX);
            var lblTitle = new Label
            {
                AutoSize = false,
                Size = new Size(titleWidth, 22),
                Location = new Point(textX + titleOffsetX, currentY),
                Text = title,
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = textColor,
                BackColor = Color.Transparent,
            };
            this.Controls.Add(lblTitle);
            currentY = lblTitle.Bottom + 5;

            var pillFont = new Font("Segoe UI", 7.5f, FontStyle.Bold);
            int pillW = TextRenderer.MeasureText(category, pillFont).Width + 14;
            var catPill = new Label
            {
                AutoSize = false,
                Size = new Size(pillW, 18),
                Location = new Point(textX, currentY),
                Text = category,
                Font = pillFont,
                ForeColor = iconCol,
                BackColor = iconBg,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            MakeRoundedRegion(catPill, 9);
            this.Controls.Add(catPill);
            currentY = catPill.Bottom + 8;

            var descFont = new Font("Segoe UI", 8.5f);
            Size measured = TextRenderer.MeasureText(description, descFont, new Size(descWidth, 0), TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);
            int descH = Math.Max(18, measured.Height + 4);
            var lblDesc = new Label
            {
                AutoSize = false,
                Size = new Size(descWidth, descH),
                Location = new Point(textX, currentY),
                Text = description,
                Font = descFont,
                ForeColor = subColor,
                BackColor = Color.Transparent,
                AutoEllipsis = false,
            };
            this.Controls.Add(lblDesc);
            currentY = lblDesc.Bottom + cardPadB;

            int cardHeight = Math.Max(90, currentY);
            this.Height = cardHeight;
            indicator.Size = new Size(indicatorW, cardHeight);

            int rightX = cardWidth - rightColW;
            this.Controls.Add(new Label
            {
                AutoSize = true,
                Text = date.ToString("MMM d, yyyy • h:mm tt"),
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = metaColor,
                Location = new Point(rightX, 14),
                BackColor = Color.Transparent,
            });
            this.Controls.Add(new Label
            {
                AutoSize = true,
                Text = officeName,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = useMaroonBg ? Color.White : Color.DimGray,
                Location = new Point(rightX, 34),
                BackColor = Color.Transparent,
            });
            if (!string.IsNullOrWhiteSpace(instructorName))
            {
                this.Controls.Add(new Label
                {
                    AutoSize = true,
                    Text = "👤 " + instructorName,
                    Font = new Font("Segoe UI", 8f),
                    ForeColor = useMaroonBg ? Color.FromArgb(255, 210, 210) : Color.DimGray,
                    Location = new Point(rightX, 54),
                    BackColor = Color.Transparent,
                });
            }

            var pinBtn = new Button
            {
                Size = new Size(36, 36),
                Location = new Point(cardWidth - 46, (cardHeight - 36) / 2),
                Text = isPinned ? "📌" : "📍",
                Font = new Font("Segoe UI Symbol", 11f),
                FlatStyle = FlatStyle.Flat,
                ForeColor = isPinned ? (useMaroonBg ? Color.White : Color.DarkRed) : (useMaroonBg ? Color.FromArgb(255, 180, 180) : Color.Silver),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                TabStop = false,
            };
            pinBtn.FlatAppearance.BorderSize = 0;
            pinBtn.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
            pinBtn.Click += (s, ev) => PinToggled?.Invoke(this, id);
            this.Controls.Add(pinBtn);

            this.Paint += (s, pe) =>
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Color borderColor = useMaroonBg ? Color.FromArgb(160, 0, 0) : Color.FromArgb(210, 210, 210);
                using var pen = new Pen(borderColor, 1f);
                pe.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
            };

            EventHandler openDetail = (s, ev) => CardClicked?.Invoke(this, id);
            this.Click += openDetail;
            lblTitle.Click += openDetail;
            lblDesc.Click += openDetail;
            iconCircle.Click += openDetail;
        }

        private static Image? GetCategoryImage(string category)
        {
            try
            {
                return category switch
                {
                    "General" => Properties.Resources.general1,
                    "Academic" => Properties.Resources.academic1,
                    "Schedule" => Properties.Resources.schedule12,
                    "Events" => Properties.Resources.events1,
                    "Examinations" => Properties.Resources.examinations1,
                    "Administrative" => Properties.Resources.administrative1,
                    "Urgent" => Properties.Resources.urgent1,
                    _ => null,
                };
            }
            catch
            {
                return null;
            }
        }

        // ════════════════════════════════════════════════════════════════════
        //  SHARED DRAWING HELPERS
        // ════════════════════════════════════════════════════════════════════
        private static void DrawCardBorder(Graphics g, Control card)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.FromArgb(220, 220, 220), 1f);
            using var path = RoundedRectPath(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 10);
            g.DrawPath(pen, path);
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

        private static void MakeRoundedRegion(Control c, int radius)
        {
            var path = new GraphicsPath();
            var r = new Rectangle(0, 0, c.Width, c.Height);
            if (c.Width <= 0 || c.Height <= 0) return;
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