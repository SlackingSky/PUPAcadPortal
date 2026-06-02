using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    // Data model passed into the popup
    public class AtRiskSubjectInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double Attendance { get; set; }
        public int Absent { get; set; }
    }

    // At-Risk Popup Form
    public partial class AtRiskPopup : Form
    {
        private readonly List<AtRiskSubjectInfo> _items;

        // State variables for window dragging
        private bool _dragging = false;
        private Point _dragStart = Point.Empty;

        public AtRiskPopup(List<AtRiskSubjectInfo> items)
        {
            _items = items ?? new List<AtRiskSubjectInfo>();
            InitializeComponent();
            PopulateDynamicContent();
        }

        private void PopulateDynamicContent()
        {
            // Set dynamic form sizing
            this.Size = new Size(520, _items.Count == 0 ? 260 : 180 + _items.Count * 90 + 70);

            // Configure Subtitle
            lblSub.Text = _items.Count == 0
                ? "All subjects are within the 80% attendance requirement."
                : $"{_items.Count} subject{(_items.Count > 1 ? "s" : "")} below the 80% attendance threshold.";
            lblSub.ForeColor = _items.Count == 0 ? Color.FromArgb(0, 130, 60) : Color.FromArgb(160, 80, 0);

            // Populate the scroll panel
            if (_items.Count == 0)
            {
                var pnlOk = new Panel
                {
                    Width = 460,
                    Height = 90,
                    BackColor = Color.FromArgb(240, 255, 245),
                    Left = 0,
                    Top = 8
                };
                pnlOk.Paint += (s, e) =>
                {
                    using var pen = new Pen(Color.FromArgb(180, 220, 190), 1);
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlOk.Width - 1, pnlOk.Height - 1);
                };

                var bar = new Panel { Width = 5, Height = 90, Left = 0, Top = 0, BackColor = Color.FromArgb(0, 160, 80) };

                var lbl = new Label
                {
                    Text = "✓  You're on track! All subjects are at or above 80% attendance.",
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(0, 120, 60),
                    Left = 16,
                    Top = 0,
                    Width = 420,
                    Height = 90,
                    TextAlign = ContentAlignment.MiddleLeft,
                    AutoSize = false
                };

                pnlOk.Controls.Add(bar);
                pnlOk.Controls.Add(lbl);
                pnlScroll.Controls.Add(pnlOk);
            }
            else
            {
                int y = 8;
                foreach (var item in _items)
                {
                    pnlScroll.Controls.Add(MakeSubjectCard(item, y, pnlScroll.Width - 32));
                    y += 82;
                }
            }
        }

        private Panel MakeSubjectCard(AtRiskSubjectInfo item, int top, int width)
        {
            var card = new Panel
            {
                Width = Math.Max(width, 100),
                Height = 74,
                Left = 0,
                Top = top,
                BackColor = Color.White,
                Tag = item
            };

            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen = new Pen(Color.FromArgb(240, 200, 200), 1);
                g.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            var accent = new Panel
            {
                Width = 5,
                Height = 74,
                Left = 0,
                Top = 0,
                BackColor = Color.FromArgb(128, 0, 0)
            };

            var iconPanel = new Panel
            {
                Width = 40,
                Height = 40,
                Left = 16,
                Top = 17,
                BackColor = Color.FromArgb(255, 235, 235)
            };

            iconPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var b = new SolidBrush(Color.FromArgb(255, 235, 235));
                e.Graphics.FillEllipse(b, 0, 0, 39, 39);
                using var warn = new SolidBrush(Color.FromArgb(200, 60, 60));
                e.Graphics.DrawString("!", new Font("Segoe UI", 14f, FontStyle.Bold),
                    warn, new RectangleF(0, 0, 39, 39),
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            };

            var lblCode = new Label
            {
                Text = item.Code,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(128, 0, 0),
                AutoSize = true,
                Left = 68,
                Top = 10
            };

            var lblName = new Label
            {
                Text = item.Name,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 40, 40),
                AutoSize = false,
                Left = 68,
                Top = 26,
                Height = 22,
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                Width = Math.Max(card.Width - 68 - 180, 60)
            };

            var lblNeeded = new Label
            {
                Text = "< 80% required",
                Font = new Font("Segoe UI", 8f, FontStyle.Italic),
                ForeColor = Color.FromArgb(180, 100, 100),
                AutoSize = true,
                Left = 68,
                Top = 52
            };

            var badgeColor = item.Attendance >= 70 ? Color.FromArgb(255, 180, 0) : Color.FromArgb(200, 50, 50);
            var badgeBack = item.Attendance >= 70 ? Color.FromArgb(255, 248, 220) : Color.FromArgb(255, 235, 235);

            var lblAttBadge = new Label
            {
                Text = $"{item.Attendance:F1}%",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = badgeColor,
                BackColor = badgeBack,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Size = new Size(76, 32),
                Top = 10,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Left = card.Width - 90
            };

            var lblAbsent = new Label
            {
                Text = $"Absent: {item.Absent}",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(160, 80, 80),
                AutoSize = true,
                Top = 48,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Left = card.Width - 90
            };

            card.Resize += (s, e) =>
            {
                lblAttBadge.Left = card.Width - 90;
                lblAbsent.Left = card.Width - 90;
                lblName.Width = Math.Max(card.Width - 68 - 180, 60);
            };

            card.Controls.AddRange(new Control[]
                { accent, iconPanel, lblCode, lblName, lblAttBadge, lblAbsent, lblNeeded });

            return card;
        }

        private void pnlScroll_Resize(object sender, EventArgs e)
        {
            int y = 8;
            int w = Math.Max(pnlScroll.ClientSize.Width - 32, 100);
            foreach (Control c in pnlScroll.Controls)
            {
                if (c is Panel card && card.Tag is AtRiskSubjectInfo)
                {
                    card.Top = y;
                    card.Left = 0;
                    card.Width = w;
                    y += 82;
                }
            }
        }

        // --- Custom Drawing and Events ---

        private void pnlSubtitle_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.FromArgb(220, 220, 220)),
                0, pnlSubtitle.Height - 1, pnlSubtitle.Width, pnlSubtitle.Height - 1);
        }

        private void pnlFooter_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.FromArgb(220, 220, 220)),
                0, 0, pnlFooter.Width, 0);
        }

        private void AtRiskPopup_SizeChanged(object sender, EventArgs e)
        {
            ApplyRoundedRegion(this, 12);
        }

        private static void ApplyRoundedRegion(Form f, int radius)
        {
            var path = new GraphicsPath();
            int r2 = radius * 2;
            path.AddArc(0, 0, r2, r2, 180, 90);
            path.AddArc(f.Width - r2, 0, r2, r2, 270, 90);
            path.AddArc(f.Width - r2, f.Height - r2, r2, r2, 0, 90);
            path.AddArc(0, f.Height - r2, r2, r2, 90, 90);
            path.CloseFigure();
            f.Region = new Region(path);
        }

        // --- Button Events ---

        private void btnClose_Click(object sender, EventArgs e) => this.Close();

        private void btnOk_Click(object sender, EventArgs e) => this.Close();

        // --- Dragging Logic ---

        private void pnlTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            _dragging = true;
            _dragStart = e.Location;
        }

        private void pnlTitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_dragging) return;
            var p = PointToScreen(e.Location);
            Location = new Point(p.X - _dragStart.X, p.Y - _dragStart.Y);
        }

        private void pnlTitleBar_MouseUp(object sender, MouseEventArgs e)
        {
            _dragging = false;
        }

        // --- Static Factory ---

        public static void Show(Form owner, List<AtRiskSubjectInfo> items)
        {
            using var popup = new AtRiskPopup(items);
            popup.ShowDialog(owner);
        }
    }
}