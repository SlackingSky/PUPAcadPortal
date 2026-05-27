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

        public AtRiskPopup(List<AtRiskSubjectInfo> items)
        {
            _items = items;
            BuildForm();
        }

        private void BuildForm()
        {
            //  Form chrome 
            this.Text = "At-Risk Subjects";
            this.FormBorderStyle = FormBorderStyle.None;   
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.Size = new Size(520, _items.Count == 0 ? 260 : 180 + _items.Count * 90 + 70);
            this.MaximumSize = new Size(520, 600);
            this.MinimumSize = new Size(520, 260);
            this.AutoScroll = false;
            this.ShowInTaskbar = false;
            this.Shown += (s, e) => ApplyRoundedRegion(this, 12);
            this.Resize += (s, e) => ApplyRoundedRegion(this, 12);

            //  Title bar 
            var titleBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = Color.FromArgb(128, 0, 0)
            };

            var lblTitle = new Label
            {
                Text = "At-Risk Subjects",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Left = 18,
                Top = 0,
                Width = 380,
                Height = 52
            };

            var btnClose = new Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 11f),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Size = new Size(40, 40),
                Location = new Point(this.Width - 50, 6)
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(160, 0, 0);
            btnClose.Click += (s, e) => this.Close();
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // Allow dragging by title bar
            bool dragging = false;
            Point dragStart = Point.Empty;
            titleBar.MouseDown += (s, e) => { dragging = true; dragStart = e.Location; };
            titleBar.MouseMove += (s, e) =>
            {
                if (!dragging) return;
                var p = PointToScreen(e.Location);
                Location = new Point(p.X - dragStart.X, p.Y - dragStart.Y);
            };
            titleBar.MouseUp += (s, e) => dragging = false;

            titleBar.Controls.Add(lblTitle);
            titleBar.Controls.Add(btnClose);

            //  Subtitle strip 
            var subtitle = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = Color.FromArgb(248, 248, 248)
            };
            subtitle.Paint += (s, e) =>
                e.Graphics.DrawLine(new Pen(Color.FromArgb(220, 220, 220)),
                    0, subtitle.Height - 1, subtitle.Width, subtitle.Height - 1);

            var lblSub = new Label
            {
                Text = _items.Count == 0
                    ? "All subjects are within the 80% attendance requirement."
                    : $"{_items.Count} subject{(_items.Count > 1 ? "s" : "")} below the 80% attendance threshold.",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = _items.Count == 0 ? Color.FromArgb(0, 130, 60) : Color.FromArgb(160, 80, 0),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Padding = new Padding(18, 0, 0, 0)
            };
            subtitle.Controls.Add(lblSub);

            //  Scrollable content area 
            var scroll = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(252, 252, 252),
                Padding = new Padding(16, 12, 16, 8)
            };

            if (_items.Count == 0)
            {
                // All-clear state
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
                scroll.Controls.Add(pnlOk);
            }
            else
            {
                int y = 8;
                foreach (var item in _items)
                {
                    scroll.Controls.Add(MakeSubjectCard(item, y, scroll.Width - 32));
                    y += 82;
                }
            }

            //  Footer 
            var footer = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 58,
                BackColor = Color.White
            };
            footer.Paint += (s, e) =>
                e.Graphics.DrawLine(new Pen(Color.FromArgb(220, 220, 220)),
                    0, 0, footer.Width, 0);

            var btnOk = new Button
            {
                Text = "OK",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                BackColor = Color.FromArgb(128, 0, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 36),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
            btnOk.Location = new Point(footer.Width - 126, 11);
            btnOk.FlatAppearance.BorderSize = 0;
            btnOk.Click += (s, e) => this.Close();
            btnOk.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            footer.Controls.Add(btnOk);

            //  Assemble (bottom-up for Dock=Top stacking) 
            this.Controls.Add(scroll);
            this.Controls.Add(subtitle);
            this.Controls.Add(titleBar);
            this.Controls.Add(footer);
            // Reposition cards after scroll is sized
            scroll.Resize += (s, e) => RepositionCards(scroll);
        }

        private void RepositionCards(Panel scroll)
        {
            int y = 8;
            int w = Math.Max(scroll.ClientSize.Width - 32, 100);
            foreach (Control c in scroll.Controls)
            {
                if (c is Panel card && card.Tag is AtRiskSubjectInfo)
                {
                    card.Top = y;
                    card.Left = 0;
                    card.Width = w;   // setting Width fires the card.Resize event
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

            // Left accent bar
            var accent = new Panel
            {
                Width = 5,
                Height = 74,
                Left = 0,
                Top = 0,
                BackColor = Color.FromArgb(128, 0, 0)
            };

            // Warning icon panel
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

            // Subject code + name
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
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right
            };
            // Set right margin to leave room for badge
            lblName.Width = Math.Max(card.Width - 68 - 180, 60);

            var lblNeeded = new Label
            {
                Text = "< 80% required",
                Font = new Font("Segoe UI", 8f, FontStyle.Italic),
                ForeColor = Color.FromArgb(180, 100, 100),
                AutoSize = true,
                Left = 68,
                Top = 52
            };

            // Attendance badge — anchored to the right
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
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            lblAttBadge.Left = card.Width - 90;

            var lblAbsent = new Label
            {
                Text = $"Absent: {item.Absent}",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(160, 80, 80),
                AutoSize = true,
                Top = 48,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            lblAbsent.Left = card.Width - 90;

            // Reposition right-anchored controls when card resizes
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

        //  Static factory: build from AttendanceControl data 
        public static void Show(Form owner, List<AtRiskSubjectInfo> items)
        {
            using var popup = new AtRiskPopup(items);
            popup.ShowDialog(owner);
        }
    }
}