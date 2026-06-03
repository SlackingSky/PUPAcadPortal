using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class ViewAnnouncementStudent : UserControl
    {
        public event EventHandler CloseRequested;

        private static readonly System.Collections.Generic.Dictionary<string, Color> CatIconColor =
            new System.Collections.Generic.Dictionary<string, Color>
            {
                ["General"] = Color.FromArgb(55, 138, 221),
                ["Academic"] = Color.FromArgb(99, 153, 34),
                ["Schedule"] = Color.FromArgb(186, 117, 23),
                ["Events"] = Color.FromArgb(212, 83, 126),
                ["Examinations"] = Color.FromArgb(127, 119, 221),
                ["Administrative"] = Color.FromArgb(90, 90, 200),
                ["Urgent"] = Color.FromArgb(220, 50, 50),
            };

        private static readonly System.Collections.Generic.Dictionary<string, Color> CatBgColor =
            new System.Collections.Generic.Dictionary<string, Color>
            {
                ["General"] = Color.FromArgb(230, 241, 251),
                ["Academic"] = Color.FromArgb(234, 243, 222),
                ["Schedule"] = Color.FromArgb(250, 238, 218),
                ["Events"] = Color.FromArgb(251, 234, 240),
                ["Examinations"] = Color.FromArgb(238, 237, 254),
                ["Administrative"] = Color.FromArgb(230, 230, 245),
                ["Urgent"] = Color.FromArgb(255, 235, 235),
            };

        private string _currentCategory = "General";

        public ViewAnnouncementStudent()
        {
            InitializeComponent();
            WireEvents();

            this.AutoScroll = false;
        }

        private void WireEvents()
        {
            btnClose.Click += (s, e) => ClosePanel();

            btnClose.MouseEnter += (s, e) => btnClose.BackColor = Color.FromArgb(180, 0, 0);
            btnClose.MouseLeave += (s, e) => btnClose.BackColor = Color.Transparent;

            Paint += (s, e) => DrawBorder(e.Graphics);
        }

        public void LoadAnnouncement(
            string title,
            string description,
            string category,
            string officeName,
            DateTime date,
            bool isUrgent,
            bool isPinned,
            string instructorName = "")
        {
            _currentCategory = category ?? "General";

            Color iconCol = CatIconColor.ContainsKey(_currentCategory)
                ? CatIconColor[_currentCategory] : Color.Gray;
            Color iconBg = CatBgColor.ContainsKey(_currentCategory)
                ? CatBgColor[_currentCategory] : Color.WhiteSmoke;

            picCategoryIcon.BackColor = iconBg;
            picCategoryIcon.Invalidate();

            int pillW = TextRenderer.MeasureText(category, lblCategoryPill.Font).Width + 18;
            lblCategoryPill.Size = new Size(pillW, 20);
            lblCategoryPill.Location = new Point(0, 3);
            lblCategoryPill.Text = category;
            lblCategoryPill.ForeColor = iconCol;
            lblCategoryPill.BackColor = iconBg;
            ApplyRoundedRegion(lblCategoryPill, 10);

            lblUrgentBadge.Visible = isUrgent;
            lblUrgentBadge.Location = new Point(pillW + 8, 3);

            lblPinnedBadge.Visible = isPinned;
            int pinnedX = pillW + (isUrgent ? 94 : 8);
            lblPinnedBadge.Location = new Point(pinnedX, 3);

            lblTitle.Text = title;
            lblDescription.Text = description;

            lblDate.Text = "📅  " + date.ToString("MMMM d, yyyy  •  h:mm tt");
            lblOffice.Text = "🏢  " + officeName;

            bool hasInstructor = !string.IsNullOrWhiteSpace(instructorName);
            lblInstructor.Visible = hasInstructor;
            if (hasInstructor)
                lblInstructor.Text = "👤  " + instructorName;

            ReflowBody();
            Refresh();
        }

        private void ReflowBody()
        {
            this.AutoScroll = false;

            lblDescription.AutoSize = false;

            Size measured = TextRenderer.MeasureText(
                lblDescription.Text,
                lblDescription.Font,
                new Size(lblDescription.Width, 0),
                TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);

            lblDescription.Height = Math.Max(60, measured.Height + 10);

            int metaTop = lblDescription.Bottom + 20;

            lblDate.Top = metaTop;
            lblOffice.Top = metaTop;

            if (lblInstructor.Visible)
            {
                lblInstructor.Top = lblDate.Bottom + 10;
            }

            int finalBottom;

            if (lblInstructor.Visible)
                finalBottom = lblInstructor.Bottom + 20;
            else
                finalBottom = lblDate.Bottom + 20;

            this.Height = finalBottom;
            if (Parent != null)
                Parent.PerformLayout();
        }

        private void ClosePanel()
        {
            Visible = false;
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void PicCategoryIcon_Paint(object sender, PaintEventArgs e)
        {
            Color iconCol = CatIconColor.ContainsKey(_currentCategory)
                ? CatIconColor[_currentCategory] : Color.Gray;
            Color iconBg = CatBgColor.ContainsKey(_currentCategory)
                ? CatBgColor[_currentCategory] : Color.WhiteSmoke;

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var b = new SolidBrush(iconBg))
                g.FillEllipse(b, 0, 0, picCategoryIcon.Width - 1, picCategoryIcon.Height - 1);

            string letter = _currentCategory.Length > 0 ? _currentCategory[..1] : "?";
            using var fnt = new Font("Segoe UI", 13f, FontStyle.Bold);
            using var brush = new SolidBrush(iconCol);
            var sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
            };
            g.DrawString(letter, fnt, brush,
                new RectangleF(0, 0, picCategoryIcon.Width, picCategoryIcon.Height), sf);
        }

        private void DrawBorder(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.FromArgb(200, 200, 200), 1f);
            using var path = RoundedRectPath(new Rectangle(0, 0, Width - 1, Height - 1), 10);
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

        private static void ApplyRoundedRegion(Control c, int radius)
        {
            using var path = RoundedRectPath(new Rectangle(0, 0, c.Width, c.Height), radius);
            c.Region = new Region(path);
        }
    }
}