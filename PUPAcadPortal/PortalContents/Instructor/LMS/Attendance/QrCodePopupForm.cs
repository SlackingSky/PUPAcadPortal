using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Instructor.LMS
{
    public sealed partial class QrCodePopupForm : Form
    {
        private static readonly Color Maroon = Color.FromArgb(106, 0, 0);
        private static readonly Color ActiveGreen = Color.FromArgb(34, 139, 34);
        private static readonly Color ExpiredRed = Color.FromArgb(200, 30, 30);
        private static readonly Color OrangeBadge = Color.FromArgb(220, 120, 0);
        private static readonly Color BorderColor = Color.FromArgb(220, 220, 220);
        private static readonly Color SubText = Color.FromArgb(90, 90, 90);
        private const int EXPIRY_MINUTES = 10;
        private const int QR_SIZE = 260;
        private const int QR_MODULES = 25;
        private readonly string _course;
        private readonly string _session;
        private readonly DateTime _date;
        private int _seed;
        private DateTime _generatedAt;
        private bool _isExpired;
        private int _animStep;
        public QrCodePopupForm(string course, string session, DateTime date)
        {
            InitializeComponent();

            _course = course;
            _session = session;
            _date = date;
            lblCourse.Text = $"Course  :  {_course}";
            lblSession.Text = $"Session  :  {_session}   |   {_date:MMMM dd, yyyy}";
            _btnRefresh.Click += BtnRefresh_Click;
            btnDownload.Click += BtnDownload_Click;
            btnClose.Click += (_, __) => Close();

            _tick.Tick += (_, __) => TickCountdown();
            anim.Tick += AnimTick;
            pnlQrCard.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen = new Pen(BorderColor, 1.5f);
                e.Graphics.DrawRectangle(pen, 1, 1, pnlQrCard.Width - 3, pnlQrCard.Height - 3);
            };

            pnlInfo.Paint += (s, e) =>
            {
                e.Graphics.DrawLine(new Pen(BorderColor), 0, 0, pnlInfo.Width, 0);
                e.Graphics.DrawLine(new Pen(BorderColor), 0, pnlInfo.Height - 1, pnlInfo.Width, pnlInfo.Height - 1);
            };

            pnlFooter.Paint += (s, e) =>
                e.Graphics.DrawLine(new Pen(BorderColor), 0, 0, pnlFooter.Width, 0);

            GenerateNew();
            _tick.Start();
        }
        public void GenerateNew()
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
            var bmp = new Bitmap(QR_SIZE, QR_SIZE);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.Clear(Color.White);

            if (_isExpired) { DrawExpired(g); _picQr.Image = bmp; return; }

            var rng = new Random(_seed);
            float sz = QR_SIZE / (float)QR_MODULES;

            for (int r = 0; r < QR_MODULES; r++)
                for (int c = 0; c < QR_MODULES; c++)
                {
                    bool tl = r < 8 && c < 8, tr = r < 8 && c >= QR_MODULES - 8,
                         bl = r >= QR_MODULES - 8 && c < 8;
                    if (tl || tr || bl) continue;
                    if (r == 6 || c == 6)
                    { if ((r + c) % 2 == 0) g.FillRectangle(Brushes.Black, c * sz, r * sz, sz - 0.6f, sz - 0.6f); continue; }
                    if (rng.NextDouble() > 0.52)
                        g.FillRectangle(Brushes.Black, c * sz, r * sz, sz - 0.6f, sz - 0.6f);
                }

            DrawFinder(g, sz, 0, 0);
            DrawFinder(g, sz, QR_MODULES - 7, 0);
            DrawFinder(g, sz, 0, QR_MODULES - 7);

            _picQr.Image = bmp;
        }

        private static void DrawFinder(Graphics g, float s, int col, int row)
        {
            float x = col * s, y = row * s;
            g.FillRectangle(Brushes.Black, x, y, 7 * s, 7 * s);
            g.FillRectangle(Brushes.White, x + s, y + s, 5 * s, 5 * s);
            g.FillRectangle(Brushes.Black, x + 2 * s, y + 2 * s, 3 * s, 3 * s);
        }

        private static void DrawExpired(Graphics g)
        {
            int sz = QR_SIZE;
            var rng = new Random(7);
            float cell = sz / (float)QR_MODULES;
            using var grey = new SolidBrush(Color.FromArgb(210, 210, 210));
            for (int r = 0; r < QR_MODULES; r++)
                for (int c = 0; c < QR_MODULES; c++)
                    if (rng.NextDouble() > 0.52)
                        g.FillRectangle(grey, c * cell, r * cell, cell - 0.6f, cell - 0.6f);

            g.FillRectangle(new SolidBrush(Color.FromArgb(170, 255, 255, 255)), 0, 0, sz, sz);

            using var pen = new Pen(Color.FromArgb(160, 200, 30, 30), 2.5f);
            var stampRect = new Rectangle(sz / 6, (int)(sz * 0.40), sz * 2 / 3, sz / 6);
            g.DrawRectangle(pen, stampRect);

            using var fnt = new Font("Segoe UI", 18f, FontStyle.Bold);
            var sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString("EXPIRED", fnt, new SolidBrush(Color.FromArgb(200, 30, 30)),
                new RectangleF(0, 0, sz, sz), sf);
        }
        private void SetStatus(string text, Color color)
        {
            if (lblStatus.InvokeRequired)
                lblStatus.Invoke(() => { lblStatus.Text = text; lblStatus.ForeColor = color; });
            else { lblStatus.Text = text; lblStatus.ForeColor = color; }
        }

        private void TickCountdown()
        {
            if (_isExpired)
            {
                lblCountdown.Text = "Code has expired — click Refresh to generate a new one";
                return;
            }
            var remaining = TimeSpan.FromMinutes(EXPIRY_MINUTES) - (DateTime.Now - _generatedAt);
            if (remaining.TotalSeconds <= 0) { ExpireNow(); return; }
            lblCountdown.Text = $"Expires in  {(int)remaining.TotalMinutes:D2}:{remaining.Seconds:D2}";
            lblCountdown.ForeColor = remaining.TotalMinutes < 2 ? Color.FromArgb(200, 100, 0) : SubText;
        }

        private void ExpireNow()
        {
            _isExpired = true;
            _tick.Stop();
            SetStatus("● Expired", ExpiredRed);
            RenderQr();
            if (lblCountdown.InvokeRequired)
                lblCountdown.Invoke(() => lblCountdown.Text = "Code expired — click Refresh to generate a new one");
            else
                lblCountdown.Text = "Code expired — click Refresh to generate a new one";
        }
        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            _btnRefresh.Enabled = false;
            SetStatus("○ Refreshing…", OrangeBadge);
            _animStep = 0;
            anim.Start();
        }

        private void AnimTick(object? sender, EventArgs e)
        {
            _animStep++;
            lblStatus.Text = _animStep % 2 == 0 ? "● Refreshing…" : "○ Refreshing…";
            if (_animStep >= 4)
            {
                anim.Stop();
                GenerateNew();
                _tick.Start();
                _btnRefresh.Enabled = true;
            }
        }
        private void BtnDownload_Click(object? sender, EventArgs e)
        {
            if (_isExpired)
            {
                MessageBox.Show("QR code has expired. Please refresh first.",
                    "Expired", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Title = "Save QR Code",
                Filter = "PNG Image (*.png)|*.png",
                FileName = $"QR_Attendance_{_date:yyyyMMdd}_{_session.Replace(" ", "_")}.png",
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                const int SAVE = 560;
                var bmp = new Bitmap(SAVE, SAVE);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    float cell = SAVE / (float)QR_MODULES;
                    var rng = new Random(_seed);

                    for (int r = 0; r < QR_MODULES; r++)
                        for (int c = 0; c < QR_MODULES; c++)
                        {
                            bool tl = r < 8 && c < 8, tr = r < 8 && c >= QR_MODULES - 8, bl = r >= QR_MODULES - 8 && c < 8;
                            if (tl || tr || bl) continue;
                            if (r == 6 || c == 6) { if ((r + c) % 2 == 0) g.FillRectangle(Brushes.Black, c * cell, r * cell, cell - 1, cell - 1); continue; }
                            if (rng.NextDouble() > 0.52) g.FillRectangle(Brushes.Black, c * cell, r * cell, cell - 1, cell - 1);
                        }
                    DrawFinder(g, cell, 0, 0);
                    DrawFinder(g, cell, QR_MODULES - 7, 0);
                    DrawFinder(g, cell, 0, QR_MODULES - 7);

                    using var wf = new Font("Segoe UI", 12f, FontStyle.Bold);
                    var sf = new StringFormat { Alignment = StringAlignment.Center };
                    g.DrawString($"PUPAcadPortal  •  {_date:MMM dd, yyyy}  •  {_session}",
                        wf, new SolidBrush(Color.FromArgb(150, 150, 150)),
                        new RectangleF(0, SAVE - 36, SAVE, 30), sf);
                }
                bmp.Save(sfd.FileName, ImageFormat.Png);
                MessageBox.Show($"QR Code saved:\n{sfd.FileName}",
                    "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not save: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _tick?.Stop(); _tick?.Dispose();
            anim?.Stop(); anim?.Dispose();
            base.OnFormClosed(e);
        }
    }
}