using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class AnnouncementCard : UserControl
    {
        public event EventHandler<AnnouncementCardData> EditRequested;
        public event EventHandler<AnnouncementCardData> DeleteRequested;
        public event EventHandler<AnnouncementCardData> ToggleRequested;

        private static readonly Dictionary<string, Color> CatIconColor = new()
        {
            ["General"] = Color.FromArgb(55, 138, 221),
            ["Academic"] = Color.FromArgb(99, 153, 34),
            ["Schedule"] = Color.FromArgb(186, 117, 23),
            ["Events"] = Color.FromArgb(212, 83, 126),
            ["Examinations"] = Color.FromArgb(127, 119, 221),
        };

        private static readonly Dictionary<string, Color> CatBgColor = new()
        {
            ["General"] = Color.FromArgb(230, 241, 251),
            ["Academic"] = Color.FromArgb(234, 243, 222),
            ["Schedule"] = Color.FromArgb(250, 238, 218),
            ["Events"] = Color.FromArgb(251, 234, 240),
            ["Examinations"] = Color.FromArgb(238, 237, 254),
        };

        private const int CardPad = 12;
        private const int CircleSize = 40;
        private const int BarHeight = 4;

        private Panel _iconCircle;
        private Label _lblNew;
        private Label _lblPin;
        private Label _lblTitle;
        private Label _lblCatPill;
        private Label _lblInactive;
        private Label _lblDesc;
        private Label _lblAuthor;
        private Label _lblViewed;
        private Panel _progressTrack;
        private Panel _progressFill;
        private Button _btnMenu;
        private ContextMenuStrip _ctxMenu;

        private readonly AnnouncementCardData _data;

        private bool _expanded = false;

        public AnnouncementCard(AnnouncementCardData data)
        {
            _data = data;

            BuildUI();
            UpdateDisplay();
        }

        private void BuildUI()
        {
            DoubleBuffered = true;

            Width = 900;
            Height = 110;

            MinimumSize = new Size(500, 110);

            BackColor = Color.White;

            Margin = new Padding(6);

            AutoScaleMode = AutoScaleMode.None;

            // ICON
            _iconCircle = new Panel
            {
                Size = new Size(CircleSize, CircleSize),
                Location = new Point(CardPad, CardPad)
            };

            _iconCircle.Paint += IconCircle_Paint;

            Controls.Add(_iconCircle);

            // NEW
            _lblNew = new Label
            {
                Size = new Size(38, 17),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(22, 163, 74),
                ForeColor = Color.White,
                Text = "NEW",
                Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };

            Controls.Add(_lblNew);

            // PIN
            _lblPin = new Label
            {
                AutoSize = true,
                Text = "📌",
                Font = new Font("Segoe UI", 9f),
                Visible = false
            };

            Controls.Add(_lblPin);

            // TITLE
            _lblTitle = new Label
            {
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 25, 25),
                AutoSize = false
            };

            Controls.Add(_lblTitle);

            // CATEGORY
            _lblCatPill = new Label
            {
                Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false
            };

            Controls.Add(_lblCatPill);

            // INACTIVE
            _lblInactive = new Label
            {
                Size = new Size(58, 18),
                BackColor = Color.FromArgb(240, 240, 240),
                ForeColor = Color.FromArgb(100, 100, 100),
                Text = "Inactive",
                Font = new Font("Segoe UI", 7.5f),
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };

            Controls.Add(_lblInactive);

            // DESCRIPTION
            _lblDesc = new Label
            {
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(85, 85, 85),
                AutoSize = false
            };

            Controls.Add(_lblDesc);

            // AUTHOR
            _lblAuthor = new Label
            {
                Font = new Font("Segoe UI", 8f),
                ForeColor = Color.FromArgb(110, 110, 110),
                AutoSize = true
            };

            Controls.Add(_lblAuthor);

            // VIEWED
            _lblViewed = new Label
            {
                Font = new Font("Segoe UI", 8f),
                ForeColor = Color.FromArgb(110, 110, 110),
                AutoSize = true
            };

            Controls.Add(_lblViewed);

            // TRACK
            _progressTrack = new Panel
            {
                Height = BarHeight,
                BackColor = Color.FromArgb(225, 225, 225)
            };

            Controls.Add(_progressTrack);

            // FILL
            _progressFill = new Panel
            {
                Height = BarHeight,
                BackColor = Color.Maroon
            };

            _progressTrack.Controls.Add(_progressFill);

            // MENU BUTTON
            _btnMenu = new Button
            {
                Size = new Size(28, 28),
                FlatStyle = FlatStyle.Flat,
                Text = "⋮",
                Font = new Font("Segoe UI", 11f),
                ForeColor = Color.Gray
            };

            _btnMenu.FlatAppearance.BorderSize = 0;

            _btnMenu.Click += BtnMenu_Click;

            Controls.Add(_btnMenu);

            // CONTEXT MENU
            _ctxMenu = new ContextMenuStrip();

            _ctxMenu.Items.Add(
                "Edit",
                null,
                (s, e) => EditRequested?.Invoke(this, _data));

            _ctxMenu.Items.Add(
                _data.Status == "active"
                    ? "Set inactive"
                    : "Set active",
                null,
                (s, e) => ToggleRequested?.Invoke(this, _data));

            _ctxMenu.Items.Add(new ToolStripSeparator());

            _ctxMenu.Items.Add(
                "Delete",
                null,
                (s, e) => DeleteRequested?.Invoke(this, _data));

            // EVENTS
            Click += (s, e) => ToggleExpand();

            _lblTitle.Click += (s, e) => ToggleExpand();

            _lblDesc.Click += (s, e) => ToggleExpand();

            Paint += Card_Paint;

            Resize += (s, e) => PositionControls();
        }

        private void UpdateDisplay()
        {
            Color iconColor = CatIconColor.GetValueOrDefault(
                _data.Category,
                Color.Gray);

            Color iconBg = CatBgColor.GetValueOrDefault(
                _data.Category,
                Color.WhiteSmoke);

            _iconCircle.BackColor = iconBg;

            _lblNew.Visible = _data.IsNew;

            _lblPin.Visible = _data.IsPinned;

            _lblTitle.Text = _data.Title;

            _lblDesc.Text = _data.Description;

            _lblAuthor.Text =
                "👤 " + _data.InstructorName;

            _lblViewed.Text =
                $"Viewed {_data.ViewedCount}/{_data.TotalStudents}";

            _lblInactive.Visible =
                _data.Status != "active";

            _lblCatPill.Text =
                _data.Category;

            _lblCatPill.BackColor = iconBg;

            _lblCatPill.ForeColor = iconColor;

            PositionControls();
        }

        private void PositionControls()
        {
            if (Width < 250) return;

            SuspendLayout();

            int textX = CardPad + CircleSize + 14;
            int rightPadding = 10;

            // MENU BUTTON — anchor to right
            _btnMenu.Location = new Point(Width - _btnMenu.Width - rightPadding, CardPad - 2);

            // CATEGORY PILL WIDTH
            int pillWidth = TextRenderer.MeasureText(_data.Category, _lblCatPill.Font).Width + 18;
            _lblCatPill.Size = new Size(pillWidth, 20);

            // INACTIVE BADGE WIDTH
            int inactiveBadgeWidth = _lblInactive.Visible ? (_lblInactive.Width + 6) : 0;

            // Total reserved space on the right (menu + pill + inactive + gaps)
            int reservedRight = _btnMenu.Width + rightPadding + pillWidth + inactiveBadgeWidth + 40;

            // PIN
            int pinWidth = _data.IsPinned ? 20 : 0;
            _lblPin.Location = new Point(textX, CardPad + 2);

            // TITLE — shrinks to leave room for pill/inactive/menu
            int titleWidth = Math.Max(120, Width - textX - reservedRight);
            _lblTitle.Location = new Point(textX + pinWidth, CardPad);
            _lblTitle.Size = new Size(titleWidth, 22);

            // NEW badge — sits below icon circle
            _lblNew.Location = new Point(CardPad, _iconCircle.Bottom - _lblNew.Height + 2);

            // CATEGORY PILL — placed after title, clamped left of menu
            int catX = Math.Min(_lblTitle.Right + 6, _btnMenu.Left - pillWidth - inactiveBadgeWidth - 12);
            _lblCatPill.Location = new Point(catX, CardPad + 1);
            MakeRoundedLabel(_lblCatPill, 10);

            // INACTIVE BADGE — placed right of category pill
            if (_lblInactive.Visible)
            {
                int inactiveX = Math.Min(_lblCatPill.Right + 6, _btnMenu.Left - _lblInactive.Width - 6);
                _lblInactive.Location = new Point(inactiveX, CardPad + 1);
                MakeRoundedLabel(_lblInactive, 10);
            }

            // DESCRIPTION
            _lblDesc.Location = new Point(textX, CardPad + 30);
            _lblDesc.Width = Width - textX - 28;
            _lblDesc.MaximumSize = new Size(_lblDesc.Width, 0);
            _lblDesc.AutoEllipsis = !_expanded;

            Size descSize = TextRenderer.MeasureText(
                _lblDesc.Text,
                _lblDesc.Font,
                new Size(_lblDesc.Width, 0),
                TextFormatFlags.WordBreak
            );
            _lblDesc.Height = _expanded ? Math.Min(descSize.Height + 4, 80) : 36;

            // META ROW
            int metaY = _lblDesc.Bottom + 10;
            _lblAuthor.Location = new Point(textX, metaY);
            _lblViewed.Location = new Point(_lblAuthor.Right + 12, metaY);

            // PROGRESS BAR
            int progressX = _lblViewed.Right + 10;
            int progressWidth = Math.Max(60, Width - progressX - 28);
            _progressTrack.Location = new Point(progressX, metaY + 6);
            _progressTrack.Size = new Size(progressWidth, BarHeight);

            int percent = _data.TotalStudents > 0
                ? (int)Math.Round(_data.ViewedCount * 100.0 / _data.TotalStudents)
                : 0;
            _progressFill.Width = (int)(_progressTrack.Width * (percent / 100.0));

            // CARD HEIGHT
            Height = Math.Max(metaY + 30, _expanded ? 155 : 110);

            ResumeLayout();
        }

        private void ToggleExpand()
        {
            _expanded = !_expanded;

            PositionControls();
        }

        private void Card_Paint(
            object sender,
            PaintEventArgs e)
        {
            e.Graphics.SmoothingMode =
                SmoothingMode.AntiAlias;

            using var pen =
                new Pen(
                    Color.FromArgb(220, 220, 220),
                    1f
                );

            DrawRoundedRect(
                e.Graphics,
                pen,
                new Rectangle(
                    0,
                    0,
                    Width - 1,
                    Height - 1
                ),
                10
            );
        }

        private void IconCircle_Paint(
            object sender,
            PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SmoothingMode =
                SmoothingMode.AntiAlias;

            Color bg =
                CatBgColor.GetValueOrDefault(
                    _data.Category,
                    Color.WhiteSmoke
                );

            using var bgBrush =
                new SolidBrush(bg);

            g.FillEllipse(
                bgBrush,
                0,
                0,
                CircleSize - 1,
                CircleSize - 1
            );

            Color iconColor =
                CatIconColor.GetValueOrDefault(
                    _data.Category,
                    Color.Gray
                );

            string letter =
                _data.Category.Length > 0
                    ? _data.Category.Substring(0, 1)
                    : "?";

            using var font =
                new Font(
                    "Segoe UI",
                    14f,
                    FontStyle.Bold
                );

            using var brush =
                new SolidBrush(iconColor);

            StringFormat sf =
                new StringFormat
                {
                    Alignment =
                        StringAlignment.Center,
                    LineAlignment =
                        StringAlignment.Center
                };

            g.DrawString(
                letter,
                font,
                brush,
                new RectangleF(
                    0,
                    0,
                    CircleSize,
                    CircleSize
                ),
                sf
            );
        }

        private void BtnMenu_Click(
            object sender,
            EventArgs e)
        {
            if (_ctxMenu.Items[1]
                is ToolStripMenuItem toggleItem)
            {
                toggleItem.Text =
                    _data.Status == "active"
                        ? "Set inactive"
                        : "Set active";
            }

            _ctxMenu.Show(
                _btnMenu,
                new Point(
                    0,
                    _btnMenu.Height
                )
            );
        }

        private static void DrawRoundedRect(
            Graphics g,
            Pen pen,
            Rectangle rect,
            int radius)
        {
            using GraphicsPath path =
                RoundedRectPath(
                    rect,
                    radius
                );

            g.DrawPath(
                pen,
                path
            );
        }

        private static GraphicsPath RoundedRectPath(
            Rectangle r,
            int radius)
        {
            GraphicsPath p =
                new GraphicsPath();

            p.AddArc(
                r.X,
                r.Y,
                radius,
                radius,
                180,
                90
            );

            p.AddArc(
                r.Right - radius,
                r.Y,
                radius,
                radius,
                270,
                90
            );

            p.AddArc(
                r.Right - radius,
                r.Bottom - radius,
                radius,
                radius,
                0,
                90
            );

            p.AddArc(
                r.X,
                r.Bottom - radius,
                radius,
                radius,
                90,
                90
            );

            p.CloseFigure();

            return p;
        }

        private static void MakeRoundedLabel(
            Label lbl,
            int radius)
        {
            using GraphicsPath path =
                RoundedRectPath(
                    new Rectangle(
                        0,
                        0,
                        lbl.Width,
                        lbl.Height
                    ),
                    radius
                );

            lbl.Region =
                new Region(path);
        }
    }

    public class AnnouncementCardData
    {
        public int Id { get; set; }

        public string Title { get; set; }
            = string.Empty;

        public string Description { get; set; }
            = string.Empty;

        public string Category { get; set; }
            = "General";

        public string Status { get; set; }
            = "active";

        public string InstructorName { get; set; }
            = string.Empty;

        public DateTime PostDate { get; set; }
            = DateTime.Now;

        public bool IsNew { get; set; }

        public bool IsPinned { get; set; }

        public int ViewedCount { get; set; }

        public int TotalStudents { get; set; }
            = 40;
    }
}