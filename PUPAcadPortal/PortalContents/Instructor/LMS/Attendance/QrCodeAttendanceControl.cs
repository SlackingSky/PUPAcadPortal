using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;

namespace PUPAcadPortal
{
    public partial class QrCodeAttendanceControl : UserControl
    {
        private const int DEFAULT_EXPIRY_MINUTES = 10;
        //  Colours
        private static readonly Color Maroon = Color.FromArgb(106, 0, 0);
        private static readonly Color LightMaroon = Color.FromArgb(180, 40, 40);
        private static readonly Color ActiveGreen = Color.FromArgb(34, 139, 34);
        private static readonly Color ExpiredRed = Color.FromArgb(200, 30, 30);
        private static readonly Color OrangeAnim = Color.FromArgb(220, 120, 0);
        private static readonly Color BorderGray = Color.FromArgb(220, 220, 220);
        private static readonly Color LabelGray = Color.FromArgb(90, 90, 90);
        private static readonly Color InfoBarBg = Color.FromArgb(248, 248, 248);
        private static readonly Color ShareBlue = Color.FromArgb(40, 100, 180);
        private static readonly Color ShareBlueDk = Color.FromArgb(30, 80, 160);
        //  State 
        private string _session = "Morning";
        private DateTime _attendanceDate = DateTime.Today;
        private int _expiryMinutes = DEFAULT_EXPIRY_MINUTES;
        private DateTime _generatedAt;
        private bool _isExpired;
        private int _seed;
        //  Controls 
        private Panel _pnlOuter;
        private Label _lblTitle;
        private Label _lblSubtitle;
        private Label _lblStatus;
        private Label _lblCountdown;
        private Panel _pnlDivider;
        private PictureBox _picQr;
        private Panel _pnlInfoBar;
        // value labels (right column of the info table)
        private Label _lblSessionVal;
        private Label _lblDateVal;
        private Label _lblClockVal;
        private Panel _pnlButtons;
        private Button _btnRefresh;
        private Button _btnShare;
        //  Timers 
        private System.Windows.Forms.Timer _expiryTimer;
        private System.Windows.Forms.Timer _countdownTimer;
        private System.Windows.Forms.Timer _refreshAnimTimer;
        private int _refreshAnimStep;
        private ToolTip _tooltip;
        public event EventHandler? QrExpired;
        //  Properties 
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Session
        {
            get => _session;
            set
            {
                _session = value;
                if (_lblSessionVal != null)
                    _lblSessionVal.Text = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime AttendanceDate
        {
            get => _attendanceDate;
            set
            {
                _attendanceDate = value;
                if (_lblDateVal != null)
                    _lblDateVal.Text = value.ToString("MMMM dd, yyyy");
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ExpiryMinutes
        {
            get => _expiryMinutes;
            set { _expiryMinutes = Math.Max(1, value); RestartExpiryTimer(); }
        }
        //  Constructor 
        public QrCodeAttendanceControl()
        {
            DoubleBuffered = true;
            BackColor = Color.White;
            MinimumSize = Size.Empty;   // let the overlay control the size exactly
            AutoScroll = false;        

            _tooltip = new ToolTip { AutoPopDelay = 1800, InitialDelay = 0 };

            BuildLayout();
            InitTimers();
            GenerateNew();
        }

        //  Public API 
        public void GenerateNew()
        {
            _seed = Environment.TickCount;
            _generatedAt = DateTime.Now;
            _isExpired = false;

            DrawQr();
            SetStatus(StatusMode.Active);
            UpdateCountdownLabel();
            RestartExpiryTimer();
        }

        // Layout
        private void BuildLayout()
        {
            _pnlOuter = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                AutoScroll = false,   
                Padding = Padding.Empty,
            };
            Controls.Add(_pnlOuter);

            //  Title 
            _lblTitle = new Label
            {
                Text = "QR Attendance",
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                ForeColor = Maroon,
                AutoSize = true,
                Location = new Point(14, 12),
            };
            _pnlOuter.Controls.Add(_lblTitle);

            _lblSubtitle = new Label
            {
                Text = "Scan to mark present",
                Font = new Font("Segoe UI", 7.5f),
                ForeColor = LabelGray,
                AutoSize = true,
                Location = new Point(14, 32),
            };
            _pnlOuter.Controls.Add(_lblSubtitle);

            //  Status + Countdown 
            _lblStatus = new Label
            {
                Text = "● Active",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = ActiveGreen,
                AutoSize = true,
                Location = new Point(14, 54),
            };
            _pnlOuter.Controls.Add(_lblStatus);

            _lblCountdown = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 7.5f),
                ForeColor = LabelGray,
                AutoSize = true,
                Location = new Point(14, 72),
            };
            _pnlOuter.Controls.Add(_lblCountdown);

            //  Thin divider 
            _pnlDivider = new Panel
            {
                Height = 1,
                BackColor = BorderGray,
                Location = new Point(10, 93),
            };
            _pnlOuter.Controls.Add(_pnlDivider);

            //  QR PictureBox 
            _picQr = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White,
                Location = new Point(10, 100),
            };
            _pnlOuter.Controls.Add(_picQr);

            const int INFO_H = 80;
            const int KEY_W = 58;   //  ("Session", "Date", "Time")

            _pnlInfoBar = new Panel
            {
                BackColor = InfoBarBg,
                AutoScroll = false,   
                Height = INFO_H,
            };
            _pnlInfoBar.Paint += (s, e) =>
            {
                using var pen = new Pen(BorderGray);
                e.Graphics.DrawLine(pen, 0, 0, ((Panel)s!).Width, 0);
            };
            _pnlOuter.Controls.Add(_pnlInfoBar);

            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                BackColor = Color.Transparent,
                AutoScroll = false,
                Padding = new Padding(8, 6, 8, 4),
                Margin = Padding.Empty,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, KEY_W));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 33.4f));
            _pnlInfoBar.Controls.Add(tbl);

            // Helper: key label (left column)
            Label MakeKey(string text) => new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                ForeColor = Maroon,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = Padding.Empty,
                AutoSize = false,
            };

            // Helper: value label (right column)
            Label MakeVal(string text, bool bold = false) => new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 8f, bold ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = LabelGray,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = Padding.Empty,
                AutoSize = false,
            };

            tbl.Controls.Add(MakeKey("Session"), 0, 0);
            _lblSessionVal = MakeVal(_session);
            tbl.Controls.Add(_lblSessionVal, 1, 0);

            tbl.Controls.Add(MakeKey("Date"), 0, 1);
            _lblDateVal = MakeVal(_attendanceDate.ToString("MMMM dd, yyyy"));
            tbl.Controls.Add(_lblDateVal, 1, 1);

            tbl.Controls.Add(MakeKey("Time"), 0, 2);
            _lblClockVal = MakeVal(DateTime.Now.ToString("hh:mm:ss tt"));
            tbl.Controls.Add(_lblClockVal, 1, 2);

            //  Button row 
            _pnlButtons = new Panel
            {
                Height = 44,
                BackColor = Color.White,
                AutoScroll = false,
            };
            _pnlOuter.Controls.Add(_pnlButtons);

            _btnRefresh = new Button
            {
                Text = "↺  Refresh QR",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = Maroon,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                Height = 36,
            };
            _btnRefresh.FlatAppearance.BorderSize = 0;
            _btnRefresh.FlatAppearance.MouseOverBackColor = LightMaroon;
            _btnRefresh.Click += BtnRefresh_Click;
            _pnlButtons.Controls.Add(_btnRefresh);

            _btnShare = new Button
            {
                Text = "⎘  Share QR",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = ShareBlue,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                Height = 36,
            };
            _btnShare.FlatAppearance.BorderSize = 0;
            _btnShare.FlatAppearance.MouseOverBackColor = ShareBlueDk;
            _btnShare.Click += BtnShare_Click;
            _pnlButtons.Controls.Add(_btnShare);

            SizeChanged += (_, __) => RepositionChildren();
            RepositionChildren();
        }

        // Reposition all children on every resize
        private void RepositionChildren()
        {
            if (_picQr == null) return;
            int w = _pnlOuter.ClientSize.Width;
            int h = _pnlOuter.ClientSize.Height;
            const int TOP_AREA = 97;   // title + status + countdown + divider
            const int INFO_H = 80;   // info bar (3 rows via TableLayoutPanel)
            const int BTN_H = 44;   // button row
            const int MARGIN = 10;
            // Divider
            _pnlDivider.Width = w - MARGIN * 2;
            _pnlDivider.Location = new Point(MARGIN, 93);
            // QR image: square, fills available vertical space
            int qrAvail = h - TOP_AREA - INFO_H - BTN_H - MARGIN * 2;
            int qrSize = Math.Max(80, Math.Min(w - MARGIN * 2, qrAvail));
            _picQr.Size = new Size(qrSize, qrSize);
            _picQr.Location = new Point((w - qrSize) / 2, TOP_AREA);
            // Info bar sits directly below the QR image
            int infoTop = _picQr.Bottom + 4;
            _pnlInfoBar.Size = new Size(w - MARGIN * 2, INFO_H);
            _pnlInfoBar.Location = new Point(MARGIN, infoTop);
            // Button row pinned to the very bottom
            _pnlButtons.Size = new Size(w, BTN_H);
            _pnlButtons.Location = new Point(0, h - BTN_H);
            // Buttons: 57 % Refresh | 6 gap | rest Share
            int gap = 6;
            int refreshW = (int)(w * 0.57) - MARGIN - gap / 2;
            int shareW = w - MARGIN * 2 - refreshW - gap;
            _btnRefresh.Size = new Size(refreshW, 36);
            _btnRefresh.Location = new Point(MARGIN, 4);
            _btnShare.Size = new Size(shareW, 36);
            _btnShare.Location = new Point(MARGIN + refreshW + gap, 4);

            DrawQr();
        }

        // QR drawing
        private void DrawQr()
        {
            if (_picQr == null) return;
            int sz = Math.Max(_picQr.Width, 80);

            var bmp = new Bitmap(sz, sz);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            if (_isExpired)
            {
                DrawExpiredOverlay(g, sz);
                _picQr.Image = bmp;
                return;
            }

            var rng = new Random(_seed);
            int modules = 25;
            float cell = sz / (float)modules;

            for (int r = 0; r < modules; r++)
                for (int c = 0; c < modules; c++)
                {
                    bool inTL = r < 8 && c < 8;
                    bool inTR = r < 8 && c >= modules - 8;
                    bool inBL = r >= modules - 8 && c < 8;
                    if (inTL || inTR || inBL) continue;
                    if (rng.NextDouble() > 0.55)
                        g.FillRectangle(Brushes.Black, c * cell, r * cell, cell - 0.5f, cell - 0.5f);
                }

            DrawFinderPattern(g, cell, 0, 0);
            DrawFinderPattern(g, cell, modules - 7, 0);
            DrawFinderPattern(g, cell, 0, modules - 7);

            for (int i = 8; i < modules - 8; i++)
                if (i % 2 == 0)
                {
                    g.FillRectangle(Brushes.Black, i * cell, 6 * cell, cell - 0.5f, cell - 0.5f);
                    g.FillRectangle(Brushes.Black, 6 * cell, i * cell, cell - 0.5f, cell - 0.5f);
                }

            _picQr.Image = bmp;
        }

        private static void DrawFinderPattern(Graphics g, float cell, int col, int row)
        {
            float x = col * cell, y = row * cell, s = cell;
            g.FillRectangle(Brushes.Black, x, y, 7 * s, 7 * s);
            g.FillRectangle(Brushes.White, x + s, y + s, 5 * s, 5 * s);
            g.FillRectangle(Brushes.Black, x + 2 * s, y + 2 * s, 3 * s, 3 * s);
        }

        private static void DrawExpiredOverlay(Graphics g, int sz)
        {
            using var grey = new SolidBrush(Color.FromArgb(200, 200, 200));
            var rng = new Random(1);
            int m = 25;
            float c = sz / (float)m;
            for (int r = 0; r < m; r++)
                for (int col = 0; col < m; col++)
                    if (rng.NextDouble() > 0.55)
                        g.FillRectangle(grey, col * c, r * c, c - 0.5f, c - 0.5f);

            g.FillRectangle(new SolidBrush(Color.FromArgb(180, 255, 255, 255)), 0, 0, sz, sz);

            using var redBrush = new SolidBrush(Color.FromArgb(200, 30, 30));
            using var font = new Font("Segoe UI", sz * 0.095f, FontStyle.Bold);
            using var bdrBrush = new SolidBrush(Color.FromArgb(80, 200, 30, 30));
            using var pen = new Pen(bdrBrush, sz * 0.006f);
            var rect = new RectangleF(sz * 0.12f, sz * 0.38f, sz * 0.76f, sz * 0.22f);
            g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString("EXPIRED", font, redBrush, new RectangleF(0, 0, sz, sz), sf);
        }

        // Status helpers
        private enum StatusMode { Active, Refreshing, Expired }

        private void SetStatus(StatusMode mode)
        {
            switch (mode)
            {
                case StatusMode.Active:
                    _lblStatus.Text = "● Active";
                    _lblStatus.ForeColor = ActiveGreen;
                    break;
                case StatusMode.Refreshing:
                    _lblStatus.Text = "● Refreshing…";
                    _lblStatus.ForeColor = OrangeAnim;
                    break;
                case StatusMode.Expired:
                    _lblStatus.Text = "● Expired";
                    _lblStatus.ForeColor = ExpiredRed;
                    break;
            }
        }

        private void UpdateCountdownLabel()
        {
            if (_isExpired)
            {
                _lblCountdown.Text = "Code expired – please refresh";
                return;
            }
            var remaining = TimeSpan.FromMinutes(_expiryMinutes) - (DateTime.Now - _generatedAt);
            if (remaining.TotalSeconds <= 0) { ExpireCode(); return; }

            _lblCountdown.Text = $"Expires in {(int)remaining.TotalMinutes:D2}:{remaining.Seconds:D2}";
            _lblCountdown.ForeColor = remaining.TotalMinutes < 2
                ? Color.FromArgb(200, 100, 0)
                : LabelGray;
        }

        private void ExpireCode()
        {
            _isExpired = true;
            SetStatus(StatusMode.Expired);
            _lblCountdown.Text = "Code expired – please refresh";
            DrawQr();
            _expiryTimer?.Stop();
            _countdownTimer?.Stop();
            QrExpired?.Invoke(this, EventArgs.Empty);
        }

        // Timers
        private void InitTimers()
        {
            // 1-second tick: countdown + live clock
            _countdownTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _countdownTimer.Tick += (_, __) =>
            {
                UpdateCountdownLabel();
                if (_lblClockVal != null && !_lblClockVal.IsDisposed)
                    _lblClockVal.Text = DateTime.Now.ToString("hh:mm:ss tt");
            };
            _countdownTimer.Start();

            // One-shot expiry timer
            _expiryTimer = new System.Windows.Forms.Timer
            {
                Interval = Math.Max(100, _expiryMinutes * 60 * 1000)
            };
            _expiryTimer.Tick += (_, __) => { _expiryTimer.Stop(); ExpireCode(); };
            _expiryTimer.Start();

            // Refresh animation (250 ms flicker)
            _refreshAnimTimer = new System.Windows.Forms.Timer { Interval = 250 };
            _refreshAnimTimer.Tick += RefreshAnimTimer_Tick;
        }

        private void RestartExpiryTimer()
        {
            _expiryTimer?.Stop();
            if (_expiryTimer != null)
            {
                _expiryTimer.Interval = Math.Max(100, _expiryMinutes * 60 * 1000);
                _expiryTimer.Start();
            }
            _countdownTimer?.Start();
        }

        // Refresh button
        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            _btnRefresh.Enabled = false;
            SetStatus(StatusMode.Refreshing);
            _refreshAnimStep = 0;
            _refreshAnimTimer.Start();
        }

        private void RefreshAnimTimer_Tick(object? sender, EventArgs e)
        {
            _refreshAnimStep++;
            _lblStatus.Text = _refreshAnimStep % 2 == 0 ? "● Refreshing…" : "○ Refreshing…";
            if (_refreshAnimStep >= 3)
            {
                _refreshAnimTimer.Stop();
                _btnRefresh.Enabled = true;
                GenerateNew();
            }
        }

        // Share button – copies a branded PNG to the clipboard
        private void BtnShare_Click(object? sender, EventArgs e)
        {
            if (_picQr?.Image == null) return;

            try
            {
                int qrSz = Math.Max(_picQr.Image.Width, 260);
                int padT = 56;
                int padB = 60;
                int totalW = qrSz + 24;
                int totalH = padT + qrSz + padB;

                using var bmp = new Bitmap(totalW, totalH);
                using var g = Graphics.FromImage(bmp);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                g.Clear(Color.White);
                // Maroon header
                g.FillRectangle(new SolidBrush(Maroon), 0, 0, totalW, padT);
                using var fntT = new Font("Segoe UI", 11f, FontStyle.Bold);
                using var fntS = new Font("Segoe UI", 8f);
                var sfL = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
                g.DrawString("QR Code Attendance", fntT, Brushes.White, new RectangleF(12, 0, totalW - 12, 32), sfL);
                g.DrawString("Scan with your phone to mark present", fntS, Brushes.White, new RectangleF(12, 30, totalW - 12, 24), sfL);
                // QR
                g.DrawImage(_picQr.Image, 12, padT, qrSz, qrSz);
                // Footer
                int footerY = padT + qrSz;
                g.FillRectangle(new SolidBrush(InfoBarBg), 0, footerY, totalW, padB);
                g.DrawLine(new Pen(BorderGray), 0, footerY, totalW, footerY);

                using var fntBold = new Font("Segoe UI", 8f, FontStyle.Bold);
                using var fntReg = new Font("Segoe UI", 8f);
                var sfC = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString($"Session : {_session}", fntBold, new SolidBrush(Maroon),
                    new RectangleF(0, footerY + 4, totalW, 22), sfC);
                g.DrawString($"Date : {_attendanceDate:MMMM dd, yyyy}   |   Time : {DateTime.Now:hh:mm tt}", fntReg, new SolidBrush(LabelGray),
                    new RectangleF(0, footerY + 26, totalW, 22), sfC);
                g.DrawString($"Generated : {DateTime.Now:hh:mm:ss tt}", fntReg, new SolidBrush(LabelGray),
                    new RectangleF(0, footerY + 44, totalW, 14), sfC);
                Clipboard.SetImage(bmp);
                // Flash button green for 2 s
                string origText = _btnShare.Text;
                Color origColor = _btnShare.BackColor;
                _btnShare.Text = "✓  Copied!";
                _btnShare.BackColor = Color.FromArgb(34, 139, 34);
                var reset = new System.Windows.Forms.Timer { Interval = 2000 };
                reset.Tick += (s2, e2) =>
                {
                    reset.Stop(); reset.Dispose();
                    if (!_btnShare.IsDisposed)
                    { _btnShare.Text = origText; _btnShare.BackColor = origColor; }
                };
                reset.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not copy to clipboard:\n" + ex.Message,
                    "Share QR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _expiryTimer?.Dispose();
                _countdownTimer?.Dispose();
                _refreshAnimTimer?.Dispose();
                _tooltip?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}