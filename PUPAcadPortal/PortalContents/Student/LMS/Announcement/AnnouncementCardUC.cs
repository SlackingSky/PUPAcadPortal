using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Student.LMS
{
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

        public AnnouncementCardUC()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint, true);
            this.Cursor = Cursors.Hand;
            this.Height = 115;
        }

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

            _accentColor = isUrgent
                ? Color.FromArgb(139, 0, 0)  
                : CatIconColor.GetValueOrDefault(category, Color.FromArgb(90, 90, 200));

            this.Width = cardWidth;

            this.BackColor = isRead ? Color.White : Color.FromArgb(255, 250, 250);

            pnlIconBlock.BackColor = _accentColor;
            pnlIconBlock.Size = new Size(48, 48);
            pnlIconBlock.Location = new Point(14, (115 - 48) / 2);
            MakeRoundedPanel(pnlIconBlock, 10);
            lblIconLetter.Text = GetCategoryInitial(category, isUrgent);
            lblIconLetter.Font = new Font("Segoe UI", 14f, FontStyle.Bold);
            lblIconLetter.ForeColor = Color.White;
            lblIconLetter.BackColor = Color.Transparent;
            lblIconLetter.Dock = DockStyle.Fill;
            lblIconLetter.TextAlign = ContentAlignment.MiddleCenter;
            pnlAccentBar.BackColor = isRead
                ? Color.FromArgb(220, 220, 220)
                : Color.FromArgb(139, 0, 0);   // ← Maroon
            pnlAccentBar.Size = new Size(4, 115);
            pnlAccentBar.Location = new Point(0, 0);

            pnlUnreadDot.Visible = !isRead;
            pnlUnreadDot.BackColor = Color.FromArgb(139, 0, 0);
            pnlUnreadDot.Size = new Size(10, 10);
            pnlUnreadDot.Location = new Point(cardWidth - 20, 10);
            MakeCircle(pnlUnreadDot);

            Color tagBg = CatBgColor.GetValueOrDefault(category, Color.FromArgb(230, 230, 245));
            lblCategoryTag.Text = isUrgent ? "⚠ URGENT" : category.ToUpper();
            lblCategoryTag.Font = new Font("Segoe UI", 7.5f, FontStyle.Bold);
            lblCategoryTag.ForeColor = _accentColor;
            lblCategoryTag.BackColor = tagBg;
            lblCategoryTag.Size = new Size(100, 20);
            lblCategoryTag.Location = new Point(74, 12);
            lblCategoryTag.TextAlign = ContentAlignment.MiddleCenter;
            MakeRoundedPanel(lblCategoryTag, 8);

            lblAttachBadge.Visible = attachmentCount > 0;
            if (attachmentCount > 0)
            {
                lblAttachBadge.Text = $"📎 {attachmentCount} file{(attachmentCount > 1 ? "s" : "")}";
                lblAttachBadge.Font = new Font("Segoe UI", 7.5f, FontStyle.Bold);
                lblAttachBadge.ForeColor = Color.FromArgb(40, 90, 175);
                lblAttachBadge.BackColor = Color.FromArgb(225, 238, 255);
                lblAttachBadge.Size = new Size(90, 20);
                lblAttachBadge.Location = new Point(182, 12);
                lblAttachBadge.TextAlign = ContentAlignment.MiddleCenter;
                MakeRoundedPanel(lblAttachBadge, 8);
            }

            int contentEndX = cardWidth - 195;
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);
            lblTitle.ForeColor = isRead ? Color.FromArgb(55, 55, 55) : Color.FromArgb(15, 15, 15);
            lblTitle.Location = new Point(74, 37);
            lblTitle.Size = new Size(contentEndX - 74, 22);
            lblTitle.AutoEllipsis = true;
            lblTitle.BackColor = Color.Transparent;

            lblDescription.Text = description;
            lblDescription.Font = new Font("Segoe UI", 8.5f);
            lblDescription.ForeColor = Color.FromArgb(105, 105, 105);
            lblDescription.Location = new Point(74, 62);
            lblDescription.Size = new Size(contentEndX - 74, 30);
            lblDescription.AutoEllipsis = true;
            lblDescription.BackColor = Color.Transparent;

            lblCourseChip.Visible = !string.IsNullOrEmpty(courseName);
            if (!string.IsNullOrEmpty(courseName))
            {
                lblCourseChip.Text = "📚 " + courseName;
                lblCourseChip.Font = new Font("Segoe UI", 7.5f);
                lblCourseChip.ForeColor = Color.FromArgb(45, 100, 45);
                lblCourseChip.BackColor = Color.FromArgb(220, 245, 215);
                lblCourseChip.Location = new Point(74, 93);
                lblCourseChip.Size = new Size(Math.Min(180, contentEndX - 78), 17);
                lblCourseChip.TextAlign = ContentAlignment.MiddleLeft;
                lblCourseChip.Padding = new Padding(4, 0, 0, 0);
                MakeRoundedPanel(lblCourseChip, 6);
            }

            int metaX = cardWidth - 188;

            lblDate.Text = date.ToString("MMM d, yyyy  •  h:mm tt");
            lblDate.Font = new Font("Segoe UI", 7.8f);
            lblDate.ForeColor = Color.Gray;
            lblDate.Location = new Point(metaX, 36);
            lblDate.AutoSize = true;
            lblDate.BackColor = Color.Transparent;

            lblOffice.Text = "🏢 " + officeName;
            lblOffice.Font = new Font("Segoe UI", 7.8f);
            lblOffice.ForeColor = Color.FromArgb(85, 85, 85);
            lblOffice.Location = new Point(metaX, 57);
            lblOffice.AutoSize = true;
            lblOffice.BackColor = Color.Transparent;

            lblInstructor.Text = "👤 " + instructorName;
            lblInstructor.Font = new Font("Segoe UI", 7.8f);
            lblInstructor.ForeColor = Color.FromArgb(85, 85, 85);
            lblInstructor.Location = new Point(metaX, 77);
            lblInstructor.AutoSize = true;
            lblInstructor.BackColor = Color.Transparent;

            btnPin.Size = new Size(30, 30);
            btnPin.Location = new Point(cardWidth - 40, 42);
            btnPin.Text = isPinned ? "📌" : "📍";
            btnPin.Font = new Font("Segoe UI Symbol", 11f);
            btnPin.FlatStyle = FlatStyle.Flat;
            btnPin.BackColor = Color.Transparent;
            btnPin.ForeColor = isPinned ? Color.FromArgb(139, 0, 0) : Color.FromArgb(190, 190, 190);
            btnPin.FlatAppearance.BorderSize = 0;
            btnPin.FlatAppearance.MouseOverBackColor = Color.FromArgb(20, 139, 0, 0);
            btnPin.Cursor = Cursors.Hand;

            WireEvents(btnPin);
        }

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

        private static void MakeRoundedPanel(Control c, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
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
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, c.Width, c.Height);
            c.Region = new Region(path);
        }
    }
}