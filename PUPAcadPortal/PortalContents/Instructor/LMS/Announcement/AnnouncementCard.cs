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

        private AnnouncementCardData _data;
        private bool _expanded = false;
        public AnnouncementCard()
        {
            InitializeComponent();
            SetupContextMenu();
        }

        public void SetData(AnnouncementCardData data)
        {
            _data = data;
            UpdateDisplay();
        }

        private void SetupContextMenu()
        {
            _ctxMenu.Items.Clear();

            _ctxMenu.Items.Add("Edit", null, (s, e) => EditRequested?.Invoke(this, _data));
            _ctxMenu.Items.Add("Toggle Status", null, (s, e) => ToggleRequested?.Invoke(this, _data));
            _ctxMenu.Items.Add(new ToolStripSeparator());
            _ctxMenu.Items.Add("Delete", null, (s, e) => DeleteRequested?.Invoke(this, _data));
        }

        private void UpdateDisplay()
        {
            if (_data == null) return;

            Color iconColor = CatIconColor.GetValueOrDefault(_data.Category, Color.Gray);
            Color iconBg = CatBgColor.GetValueOrDefault(_data.Category, Color.WhiteSmoke);

            _iconCircle.BackColor = iconBg;
            _lblNew.Visible = _data.IsNew;
            _lblPin.Visible = _data.IsPinned;
            _lblTitle.Text = _data.Title;
            _lblDesc.Text = _data.Description;
            _lblAuthor.Text = "👤 " + _data.InstructorName;
            _lblViewed.Text = $"Viewed {_data.ViewedCount}/{_data.TotalStudents}";
            _lblInactive.Visible = _data.Status != "active";
            _lblCatPill.Text = _data.Category;
            _lblCatPill.BackColor = iconBg;
            _lblCatPill.ForeColor = iconColor;

            if (_ctxMenu.Items.Count > 1)
            {
                _ctxMenu.Items[1].Text = _data.Status == "active" ? "Set inactive" : "Set active";
            }

            PositionControls();
        }

        private void PositionControls()
        {
            if (_data == null || Width < 250) return;

            SuspendLayout();

            int textX = CardPad + CircleSize + 14;
            int rightPadding = 10;

            _btnMenu.Location = new Point(Width - _btnMenu.Width - rightPadding, CardPad - 2);

            int pillWidth = TextRenderer.MeasureText(_data.Category, _lblCatPill.Font).Width + 18;
            _lblCatPill.Size = new Size(pillWidth, 20);

            int inactiveBadgeWidth = _lblInactive.Visible ? (_lblInactive.Width + 6) : 0;
            int reservedRight = _btnMenu.Width + rightPadding + pillWidth + inactiveBadgeWidth + 40;

            int pinWidth = _data.IsPinned ? 20 : 0;
            _lblPin.Location = new Point(textX, CardPad + 2);

            int titleWidth = Math.Max(120, Width - textX - reservedRight);
            _lblTitle.Location = new Point(textX + pinWidth, CardPad);
            _lblTitle.Size = new Size(titleWidth, 22);

            _lblNew.Location = new Point(CardPad, _iconCircle.Bottom - _lblNew.Height + 2);

            int catX = Math.Min(_lblTitle.Right + 6, _btnMenu.Left - pillWidth - inactiveBadgeWidth - 12);
            _lblCatPill.Location = new Point(catX, CardPad + 1);
            MakeRoundedLabel(_lblCatPill, 10);

            if (_lblInactive.Visible)
            {
                int inactiveX = Math.Min(_lblCatPill.Right + 6, _btnMenu.Left - _lblInactive.Width - 6);
                _lblInactive.Location = new Point(inactiveX, CardPad + 1);
                MakeRoundedLabel(_lblInactive, 10);
            }

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

            int metaY = _lblDesc.Bottom + 10;
            _lblAuthor.Location = new Point(textX, metaY);
            _lblViewed.Location = new Point(_lblAuthor.Right + 12, metaY);

            int progressX = _lblViewed.Right + 10;
            int progressWidth = Math.Max(60, Width - progressX - 28);
            _progressTrack.Location = new Point(progressX, metaY + 6);
            _progressTrack.Size = new Size(progressWidth, BarHeight);

            int percent = _data.TotalStudents > 0
                ? (int)Math.Round(_data.ViewedCount * 100.0 / _data.TotalStudents)
                : 0;
            _progressFill.Width = (int)(_progressTrack.Width * (percent / 100.0));

            Height = Math.Max(metaY + 30, _expanded ? 155 : 110);

            ResumeLayout();
        }

        private void ToggleExpand_Event(object sender, EventArgs e)
        {
            _expanded = !_expanded;
            PositionControls();
        }

        private void Card_Resize(object sender, EventArgs e)
        {
            PositionControls();
        }

        private void Card_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.FromArgb(220, 220, 220), 1f);
            DrawRoundedRect(e.Graphics, pen, new Rectangle(0, 0, Width - 1, Height - 1), 10);
        }

        private void IconCircle_Paint(object sender, PaintEventArgs e)
        {
            if (_data == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color bg = CatBgColor.GetValueOrDefault(_data.Category, Color.WhiteSmoke);
            using var bgBrush = new SolidBrush(bg);
            g.FillEllipse(bgBrush, 0, 0, CircleSize - 1, CircleSize - 1);

            Color iconColor = CatIconColor.GetValueOrDefault(_data.Category, Color.Gray);
            string letter = _data.Category.Length > 0 ? _data.Category.Substring(0, 1) : "?";

            using var font = new Font("Segoe UI", 14f, FontStyle.Bold);
            using var brush = new SolidBrush(iconColor);
            StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            g.DrawString(letter, font, brush, new RectangleF(0, 0, CircleSize, CircleSize), sf);
        }

        private void BtnMenu_Click(object sender, EventArgs e)
        {
            if (_data == null) return;

            if (_ctxMenu.Items.Count > 1)
            {
                _ctxMenu.Items[1].Text = _data.Status == "active" ? "Set inactive" : "Set active";
            }
            _ctxMenu.Show(_btnMenu, new Point(0, _btnMenu.Height));
        }

        private static void DrawRoundedRect(Graphics g, Pen pen, Rectangle rect, int radius)
        {
            using GraphicsPath path = RoundedRectPath(rect, radius);
            g.DrawPath(pen, path);
        }

        private static GraphicsPath RoundedRectPath(Rectangle r, int radius)
        {
            GraphicsPath p = new GraphicsPath();
            p.AddArc(r.X, r.Y, radius, radius, 180, 90);
            p.AddArc(r.Right - radius, r.Y, radius, radius, 270, 90);
            p.AddArc(r.Right - radius, r.Bottom - radius, radius, radius, 0, 90);
            p.AddArc(r.X, r.Bottom - radius, radius, radius, 90, 90);
            p.CloseFigure();
            return p;
        }

        private static void MakeRoundedLabel(Label lbl, int radius)
        {
            if (lbl.Width <= 0 || lbl.Height <= 0) return;
            using GraphicsPath path = RoundedRectPath(new Rectangle(0, 0, lbl.Width, lbl.Height), radius);
            lbl.Region = new Region(path);
        }
    }

    public class AnnouncementCardData
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = "General";
        public string Status { get; set; } = "active";
        public string InstructorName { get; set; } = string.Empty;
        public DateTime PostDate { get; set; } = DateTime.Now;
        public bool IsNew { get; set; }
        public bool IsPinned { get; set; }
        public int ViewedCount { get; set; }
        public int TotalStudents { get; set; } = 40;
    }
}