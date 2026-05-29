using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public sealed partial class QrCodePopupForm : Form
    {
        //  Colours 
        private static readonly Color Maroon = Color.FromArgb(106, 0, 0);
        private static readonly Color LightMaroon = Color.FromArgb(160, 30, 30);
        private static readonly Color ActiveGreen = Color.FromArgb(34, 139, 34);
        private static readonly Color ExpiredRed = Color.FromArgb(200, 30, 30);
        private static readonly Color OrangeBadge = Color.FromArgb(220, 120, 0);
        private static readonly Color PanelBg = Color.FromArgb(248, 248, 248);
        private static readonly Color BorderColor = Color.FromArgb(220, 220, 220);
        private static readonly Color DarkText = Color.FromArgb(30, 30, 30);
        private static readonly Color SubText = Color.FromArgb(90, 90, 90);
        //  Config 
        private const int EXPIRY_MINUTES = 10;
        private const int QR_BITMAP_SIZE = 260;
        private const int QR_MODULES = 25;
        //  Session info 
        private readonly string _course;
        private readonly string _session;
        private readonly DateTime _date;
        //  State 
        private int _seed;
        private DateTime _generatedAt;
        private bool _isExpired;
        //  Controls 
        private PictureBox _picQr = null!;
        private Label _lblStatus = null!;
        private Label _lblCountdown = null!;
        private Label _lblCourse = null!;
        private Label _lblSession = null!;
        private Label _lblDate = null!;
        private Button _btnRefresh = null!;
        private Button _btnDownload = null!;
        private Button _btnClose = null!;
        private Panel _pnlQrBorder = null!;
        //  Timers 
        private System.Windows.Forms.Timer _tickTimer = null!;
        private System.Windows.Forms.Timer _animTimer = null!;
        private int _animStep;
        //  Constructor 
        public QrCodePopupForm(string course, string session, DateTime date)
        {
            _course = course;
            _session = session;
            _date = date;

            BuildForm();
            GenerateNew();
        }

        //  Form shell 
        private void BuildForm()
        {
            Text = "QR Code – Attendance";
            Size = new Size(420, 620);
            MinimumSize = new Size(380, 580);
            MaximumSize = new Size(500, 700);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 9f);

            BuildHeader();
            BuildQrArea();
            BuildInfoStrip();
            BuildFooterButtons();
            BuildTimers();
        }

        //  Header 
        private void BuildHeader()
        {
            // Maroon top band
            var pnlHeader = new Panel
            {
                BackColor = Maroon,
                Dock = DockStyle.Top,
                Height = 58,
            };

            var lblTitle = new Label
            {
                Text = "Generate QR Code",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Padding = new Padding(18, 0, 0, 0),
            };

            var lblSub = new Label
            {
                Text = "Students scan this code to mark their attendance",
                Font = new Font("Segoe UI", 8f),
                ForeColor = Color.FromArgb(220, 200, 200),
                AutoSize = false,
                TextAlign = ContentAlignment.BottomLeft,
                Dock = DockStyle.Bottom,
                Height = 20,
                Padding = new Padding(19, 0, 0, 4),
            };

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSub);
            Controls.Add(pnlHeader);
        }

        //  QR area 
        private void BuildQrArea()
        {
            // White card with subtle shadow border
            _pnlQrBorder = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Size = new Size(QR_BITMAP_SIZE + 24, QR_BITMAP_SIZE + 24),
                Location = new Point(0, 0),   // positioned in BuildForm layout
            };
            // Draw thin rounded border via Paint
            _pnlQrBorder.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = new Rectangle(1, 1, _pnlQrBorder.Width - 3, _pnlQrBorder.Height - 3);
                using var pen = new Pen(BorderColor, 1.5f);
                g.DrawRectangle(pen, rect);
            };

            _picQr = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(QR_BITMAP_SIZE, QR_BITMAP_SIZE),
                Location = new Point(12, 12),
                BackColor = Color.White,
            };
            _pnlQrBorder.Controls.Add(_picQr);

            // Container to centre everything horizontally
            var pnlCenter = new Panel
            {
                Dock = DockStyle.Top,
                Height = QR_BITMAP_SIZE + 24 + 16,
                BackColor = Color.White,
                Padding = new Padding(0, 12, 0, 4),
            };
            pnlCenter.Controls.Add(_pnlQrBorder);

            // Centre the card on resize
            pnlCenter.Resize += (_, __) =>
                _pnlQrBorder.Location = new Point(
                    (pnlCenter.ClientSize.Width - _pnlQrBorder.Width) / 2,
                    pnlCenter.Padding.Top);

            Controls.Add(pnlCenter);
            pnlCenter.BringToFront();

            // Status badge
            _lblStatus = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = ActiveGreen,
                Text = "● Active",
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 22,
                BackColor = Color.White,
            };
            Controls.Add(_lblStatus);
            _lblStatus.BringToFront();

            // Countdown
            _lblCountdown = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 8f),
                ForeColor = SubText,
                Text = "",
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 20,
                BackColor = Color.White,
            };
            Controls.Add(_lblCountdown);
            _lblCountdown.BringToFront();
        }

        //  Info strip  
        private void BuildInfoStrip()
        {
            var pnlInfo = new Panel
            {
                Dock = DockStyle.Top,
                Height = 72,
                BackColor = PanelBg,
                Padding = new Padding(18, 8, 18, 8),
            };

            // Thin top border
            pnlInfo.Paint += (s, e) =>
            {
                e.Graphics.DrawLine(new Pen(BorderColor), 0, 0, ((Panel)s!).Width, 0);
                e.Graphics.DrawLine(new Pen(BorderColor), 0, ((Panel)s!).Height - 1, ((Panel)s!).Width, ((Panel)s!).Height - 1);
            };

            _lblCourse = MakeInfoLabel($"Course  :  {_course}", 10, 10, pnlInfo.Width - 20);
            _lblSession = MakeInfoLabel($"Session  :  {_session}   |   {_date:MMMM dd, yyyy}", 10, 30, pnlInfo.Width - 20);

            pnlInfo.Controls.Add(_lblCourse);
            pnlInfo.Controls.Add(_lblSession);
            pnlInfo.Resize += (_, __) =>
            {
                _lblCourse.Width = pnlInfo.ClientSize.Width - 20;
                _lblSession.Width = pnlInfo.ClientSize.Width - 20;
            };

            Controls.Add(pnlInfo);
            pnlInfo.BringToFront();
        }

        private static Label MakeInfoLabel(string text, int x, int y, int width)
            => new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = SubText,
                AutoSize = false,
                Location = new Point(x, y),
                Width = width,
                Height = 20,
                BackColor = Color.Transparent,
            };

        //  Footer buttons 
        private void BuildFooterButtons()
        {
            var pnlFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(14, 0, 14, 0),
            };
            pnlFooter.Paint += (s, e) =>
                e.Graphics.DrawLine(new Pen(BorderColor), 0, 0, ((Panel)s!).Width, 0);

            _btnRefresh = MakeButton("↺  Refresh", Maroon, Color.White, 110);
            _btnRefresh.Click += BtnRefresh_Click;

            _btnDownload = MakeButton("⬇  Save PNG", Color.White, Maroon, 110);
            _btnDownload.FlatAppearance.BorderColor = Maroon;
            _btnDownload.ForeColor = Maroon;
            _btnDownload.Click += BtnDownload_Click;

            _btnClose = MakeButton("Close", Color.FromArgb(240, 240, 240), DarkText, 80);
            _btnClose.FlatAppearance.BorderColor = BorderColor;
            _btnClose.Click += (_, __) => Close();

            // Layout buttons right-to-left from the right edge
            pnlFooter.Resize += (_, __) => LayoutFooterButtons(pnlFooter);

            pnlFooter.Controls.AddRange(new Control[] { _btnRefresh, _btnDownload, _btnClose });
            Controls.Add(pnlFooter);

            LayoutFooterButtons(pnlFooter);
        }

        private void LayoutFooterButtons(Panel footer)
        {
            int btnY = (footer.Height - 36) / 2;
            int right = footer.ClientSize.Width - footer.Padding.Right;

            _btnClose.Location = new Point(right - _btnClose.Width, btnY);
            _btnDownload.Location = new Point(_btnClose.Left - _btnDownload.Width - 8, btnY);
            _btnRefresh.Location = new Point(_btnDownload.Left - _btnRefresh.Width - 8, btnY);
        }

        private static Button MakeButton(string text, Color back, Color fore, int width)
        {
            var b = new Button
            {
                Text = text,
                Size = new Size(width, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = back,
                ForeColor = fore,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Cursor = Cursors.Hand,
            };
            b.FlatAppearance.BorderSize = 1;
            return b;
        }

        //  Timers  
        private void BuildTimers()
        {
            _tickTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _tickTimer.Tick += (_, __) => TickCountdown();
            _tickTimer.Start();

            _animTimer = new System.Windows.Forms.Timer { Interval = 260 };
            _animTimer.Tick += AnimTimer_Tick;
        }

        //  QR generation 
        private void GenerateNew()
        {
            _seed = unchecked((int)(DateTime.Now.Ticks & 0x7FFFFFFF));
            _generatedAt = DateTime.Now;
            _isExpired = false;

            RenderQr();
            SetStatus("● Active", ActiveGreen);
            TickCountdown();
        }

        private void RenderQr()
        {
            var bmp = new Bitmap(QR_BITMAP_SIZE, QR_BITMAP_SIZE);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.Clear(Color.White);

            if (_isExpired)
            {
                DrawExpiredState(g);
                _picQr.Image = bmp;
                return;
            }

            var rng = new Random(_seed);
            float sz = QR_BITMAP_SIZE / (float)QR_MODULES;

            for (int r = 0; r < QR_MODULES; r++)
            {
                for (int c = 0; c < QR_MODULES; c++)
                {
                    bool finderTL = r < 8 && c < 8;
                    bool finderTR = r < 8 && c >= QR_MODULES - 8;
                    bool finderBL = r >= QR_MODULES - 8 && c < 8;
                    if (finderTL || finderTR || finderBL) continue;

                    // Timing strips
                    if (r == 6 || c == 6) { if ((r + c) % 2 == 0) g.FillRectangle(Brushes.Black, c * sz, r * sz, sz - 0.6f, sz - 0.6f); continue; }

                    if (rng.NextDouble() > 0.52)
                        g.FillRectangle(Brushes.Black, c * sz, r * sz, sz - 0.6f, sz - 0.6f);
                }
            }

            DrawFinder(g, sz, 0, 0);
            DrawFinder(g, sz, QR_MODULES - 7, 0);
            DrawFinder(g, sz, 0, QR_MODULES - 7);

            _picQr.Image = bmp;
        }

        private static void DrawFinder(Graphics g, float cellSz, int colM, int rowM)
        {
            float x = colM * cellSz, y = rowM * cellSz, s = cellSz;
            g.FillRectangle(Brushes.Black, x, y, 7 * s, 7 * s);
            g.FillRectangle(Brushes.White, x + s, y + s, 5 * s, 5 * s);
            g.FillRectangle(Brushes.Black, x + 2 * s, y + 2 * s, 3 * s, 3 * s);
        }

        private static void DrawExpiredState(Graphics g)
        {
            int sz = QR_BITMAP_SIZE;
            var rng = new Random(7);
            float cell = sz / (float)QR_MODULES;
            using var greyBrush = new SolidBrush(Color.FromArgb(210, 210, 210));
            for (int r = 0; r < QR_MODULES; r++)
                for (int c = 0; c < QR_MODULES; c++)
                    if (rng.NextDouble() > 0.52)
                        g.FillRectangle(greyBrush, c * cell, r * cell, cell - 0.6f, cell - 0.6f);

            // White veil
            g.FillRectangle(new SolidBrush(Color.FromArgb(170, 255, 255, 255)), 0, 0, sz, sz);

            // Red "EXPIRED" stamp with border box
            using var stampPen = new Pen(Color.FromArgb(160, ExpiredRed), 2.5f);
            var stampRect = new Rectangle(sz / 6, (int)(sz * 0.40), sz * 2 / 3, sz / 6);
            g.DrawRectangle(stampPen, stampRect);

            using var fnt = new Font("Segoe UI", 18f, FontStyle.Bold);
            var sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
            };
            g.DrawString("EXPIRED", fnt, new SolidBrush(ExpiredRed),
                new RectangleF(0, 0, sz, sz), sf);
        }

        //  Status / countdown 
        private void SetStatus(string text, Color color)
        {
            if (_lblStatus.InvokeRequired)
                _lblStatus.Invoke(() => { _lblStatus.Text = text; _lblStatus.ForeColor = color; });
            else { _lblStatus.Text = text; _lblStatus.ForeColor = color; }
        }

        private void TickCountdown()
        {
            if (_isExpired)
            {
                _lblCountdown.Text = "Code has expired – click Refresh to generate a new one";
                return;
            }

            var elapsed = DateTime.Now - _generatedAt;
            var remaining = TimeSpan.FromMinutes(EXPIRY_MINUTES) - elapsed;

            if (remaining.TotalSeconds <= 0)
            {
                ExpireNow();
                return;
            }

            _lblCountdown.Text = $"Expires in  {(int)remaining.TotalMinutes:D2}:{remaining.Seconds:D2}";

            // Turn orange when < 2 minutes left
            _lblCountdown.ForeColor = remaining.TotalMinutes < 2
                ? Color.FromArgb(200, 100, 0)
                : SubText;
        }

        private void ExpireNow()
        {
            _isExpired = true;
            _tickTimer.Stop();
            SetStatus("● Expired", ExpiredRed);
            RenderQr();

            if (_lblCountdown.InvokeRequired)
                _lblCountdown.Invoke(() => _lblCountdown.Text = "Code has expired – click Refresh to generate a new one");
            else
                _lblCountdown.Text = "Code has expired – click Refresh to generate a new one";
        }

        //  Refresh button 
        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            _btnRefresh.Enabled = false;
            SetStatus("○ Refreshing…", OrangeBadge);
            _animStep = 0;
            _animTimer.Start();
        }

        private void AnimTimer_Tick(object? sender, EventArgs e)
        {
            _animStep++;
            _lblStatus.Text = _animStep % 2 == 0 ? "● Refreshing…" : "○ Refreshing…";

            if (_animStep >= 4)
            {
                _animTimer.Stop();
                GenerateNew();
                _tickTimer.Start();
                _btnRefresh.Enabled = true;
            }
        }

        //  Download / Save PNG 
        private void BtnDownload_Click(object? sender, EventArgs e)
        {
            if (_isExpired)
            {
                MessageBox.Show("The QR code has expired. Please refresh first.",
                    "Expired", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Title = "Save QR Code as PNG",
                Filter = "PNG Image (*.png)|*.png",
                FileName = $"QR_Attendance_{_date:yyyyMMdd}_{_session.Replace(" ", "_")}.png",
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                // Render a high-res version for saving (560 px)
                const int SAVE_SZ = 560;
                int modules = QR_MODULES;
                var bmp = new Bitmap(SAVE_SZ, SAVE_SZ);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    float cell = SAVE_SZ / (float)modules;
                    var rng = new Random(_seed);

                    for (int r = 0; r < modules; r++)
                        for (int c = 0; c < modules; c++)
                        {
                            bool tl = r < 8 && c < 8, tr = r < 8 && c >= modules - 8, bl = r >= modules - 8 && c < 8;
                            if (tl || tr || bl) continue;
                            if (r == 6 || c == 6) { if ((r + c) % 2 == 0) g.FillRectangle(Brushes.Black, c * cell, r * cell, cell - 1, cell - 1); continue; }
                            if (rng.NextDouble() > 0.52) g.FillRectangle(Brushes.Black, c * cell, r * cell, cell - 1, cell - 1);
                        }

                    DrawFinder(g, cell, 0, 0);
                    DrawFinder(g, cell, modules - 7, 0);
                    DrawFinder(g, cell, 0, modules - 7);

                    // Watermark
                    using var wFont = new Font("Segoe UI", 14f, FontStyle.Bold);
                    var sf = new StringFormat { Alignment = StringAlignment.Center };
                    g.DrawString($"PUPAcadPortal  •  {_date:MMM dd, yyyy}  •  {_session}",
                        wFont, new SolidBrush(Color.FromArgb(160, 160, 160)),
                        new RectangleF(0, SAVE_SZ - 36, SAVE_SZ, 30), sf);
                }

                bmp.Save(sfd.FileName, ImageFormat.Png);
                MessageBox.Show($"QR Code saved to:\n{sfd.FileName}",
                    "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not save: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //  Cleanup 
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _tickTimer?.Stop();
            _tickTimer?.Dispose();
            _animTimer?.Stop();
            _animTimer?.Dispose();
            base.OnFormClosed(e);
        }
    }
}