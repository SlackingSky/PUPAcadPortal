using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;

namespace PUPAcadPortal.PortalContents.Instructor.LMS
{
    /// <summary>
    /// Compact embedded QR panel used inside the overlay.
    /// Exposes GenerateNew(), Session, and AttendanceDate properties.
    /// </summary>
    public partial class QrCodeAttendanceControl : UserControl
    {
        // ── Palette ──────────────────────────────────────────────────────────
        private static readonly Color ActiveGreen = Color.FromArgb(34, 139, 34);
        private static readonly Color ExpiredRed = Color.FromArgb(200, 30, 30);
        private static readonly Color OrangeAnim = Color.FromArgb(220, 120, 0);
        private static readonly Color BorderGray = Color.FromArgb(220, 220, 220);
        private static readonly Color LabelGray = Color.FromArgb(90, 90, 90);

        // ── Config ───────────────────────────────────────────────────────────
        private const int DEFAULT_EXPIRY = 10;
        private const int QR_MODULES = 25;

        // ── Properties ───────────────────────────────────────────────────────
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Session
        {
            get => _session;
            set { _session = value; if (_lblSessionVal != null) _lblSessionVal.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime AttendanceDate
        {
            get => _attendanceDate;
            set { _attendanceDate = value; if (_lblDateVal != null) _lblDateVal.Text = value.ToString("MMMM dd, yyyy"); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ExpiryMinutes
        {
            get => _expiryMinutes;
            set { _expiryMinutes = Math.Max(1, value); RestartExpiry(); }
        }

        public event EventHandler? QrExpired;

        // ── State ────────────────────────────────────────────────────────────
        private string _session = "Morning";
        private DateTime _attendanceDate = DateTime.Today;
        private int _expiryMinutes = DEFAULT_EXPIRY;
        private int _seed;
        private DateTime _generatedAt;
        private bool _isExpired;
        private int _animStep;

        // ── Constructor ──────────────────────────────────────────────────────
        public QrCodeAttendanceControl()
        {
            InitializeComponent();
            _lblDateVal.Text = _attendanceDate.ToString("MMMM dd, yyyy");
            _lblClockVal.Text = DateTime.Now.ToString("hh:mm:ss tt");

            Reposition();
            GenerateNew();
            _countdownTimer.Start();
        }

        // ── Public API ───────────────────────────────────────────────────────
        public void GenerateNew()
        {
            _seed = unchecked((int)(DateTime.Now.Ticks & 0x7FFFFFFF));
            _generatedAt = DateTime.Now;
            _isExpired = false;
            DrawQr();
            SetStatus(false, false);
            UpdateCountdown();
            RestartExpiry();
        }

        // ── Layout (Dynamic Resizing) ────────────────────────────────────────
        private void QrCodeAttendanceControl_SizeChanged(object sender, EventArgs e)
        {
            Reposition();
        }

        private void Reposition()
        {
            if (_picQr == null) return;
            int w = ClientSize.Width;
            int h = ClientSize.Height;
            const int TOP_H = 46;
            const int INFO_H = 80;
            const int BTN_H = 44;
            const int M = 10;

            int qrAvail = h - TOP_H - INFO_H - BTN_H - M;
            int qrSz = Math.Max(80, Math.Min(w - M * 2, qrAvail));
            _picQr.Size = new Size(qrSz, qrSz);
            _picQr.Location = new Point((w - qrSz) / 2, TOP_H);

            infoBar.Size = new Size(w - M * 2, INFO_H);
            infoBar.Location = new Point(M, _picQr.Bottom + 4);

            pnlBtns.Size = new Size(w, BTN_H);
            pnlBtns.Location = new Point(0, h - BTN_H);

            int gap = 6;
            int rW = (int)(w * 0.55) - M - gap / 2;
            int dW = w - M * 2 - rW - gap;
            _btnRefresh.Size = new Size(rW, 34);
            _btnRefresh.Location = new Point(M, 5);
            _btnDownload.Size = new Size(dW, 34);
            _btnDownload.Location = new Point(M + rW + gap, 5);

            DrawQr();
        }

        private void infoBar_Paint(object sender, PaintEventArgs e)
        {
            using var p = new Pen(BorderGray);
            e.Graphics.DrawLine(p, 0, 0, ((Panel)sender).Width, 0);
        }

        // ── QR Drawing ───────────────────────────────────────────────────────
        private void DrawQr()
        {
            if (_picQr == null || _picQr.Width == 0) return;
            int sz = Math.Max(_picQr.Width, 80);
            var bmp = new Bitmap(sz, sz);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            if (_isExpired) { DrawExpiredOverlay(g, sz); _picQr.Image = bmp; return; }

            var rng = new Random(_seed);
            float cell = sz / (float)QR_MODULES;

            for (int r = 0; r < QR_MODULES; r++)
                for (int c = 0; c < QR_MODULES; c++)
                {
                    bool tl = r < 8 && c < 8, tr = r < 8 && c >= QR_MODULES - 8, bl = r >= QR_MODULES - 8 && c < 8;
                    if (tl || tr || bl) continue;
                    if (r == 6 || c == 6) { if ((r + c) % 2 == 0) g.FillRectangle(Brushes.Black, c * cell, r * cell, cell - 0.5f, cell - 0.5f); continue; }
                    if (rng.NextDouble() > 0.52)
                        g.FillRectangle(Brushes.Black, c * cell, r * cell, cell - 0.5f, cell - 0.5f);
                }

            Finder(g, cell, 0, 0);
            Finder(g, cell, QR_MODULES - 7, 0);
            Finder(g, cell, 0, QR_MODULES - 7);

            _picQr.Image = bmp;
        }

        private static void Finder(Graphics g, float s, int col, int row)
        {
            float x = col * s, y = row * s;
            g.FillRectangle(Brushes.Black, x, y, 7 * s, 7 * s);
            g.FillRectangle(Brushes.White, x + s, y + s, 5 * s, 5 * s);
            g.FillRectangle(Brushes.Black, x + 2 * s, y + 2 * s, 3 * s, 3 * s);
        }

        private static void DrawExpiredOverlay(Graphics g, int sz)
        {
            using var grey = new SolidBrush(Color.FromArgb(200, 200, 200));
            var rng = new Random(1);
            float c = sz / (float)QR_MODULES;
            for (int r = 0; r < QR_MODULES; r++)
                for (int col = 0; col < QR_MODULES; col++)
                    if (rng.NextDouble() > 0.55) g.FillRectangle(grey, col * c, r * c, c - 0.5f, c - 0.5f);

            g.FillRectangle(new SolidBrush(Color.FromArgb(180, 255, 255, 255)), 0, 0, sz, sz);
            using var rb = new SolidBrush(Color.FromArgb(200, 30, 30));
            using var fnt = new Font("Segoe UI", sz * 0.09f, FontStyle.Bold);
            using var pen = new Pen(Color.FromArgb(80, 200, 30, 30), sz * 0.006f);
            var rect = new RectangleF(sz * 0.12f, sz * 0.40f, sz * 0.76f, sz * 0.20f);
            g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString("EXPIRED", fnt, rb, new RectangleF(0, 0, sz, sz), sf);
        }

        // ── Status helpers ────────────────────────────────────────────────────
        private void SetStatus(bool expired, bool refreshing)
        {
            if (refreshing) { _lblStatus.Text = "● Refreshing…"; _lblStatus.ForeColor = OrangeAnim; }
            else if (expired) { _lblStatus.Text = "● Expired"; _lblStatus.ForeColor = ExpiredRed; }
            else { _lblStatus.Text = "● Active"; _lblStatus.ForeColor = ActiveGreen; }
        }

        private void UpdateCountdown()
        {
            if (_isExpired) { _lblCountdown.Text = "Code expired — click Refresh"; return; }
            var rem = TimeSpan.FromMinutes(_expiryMinutes) - (DateTime.Now - _generatedAt);
            if (rem.TotalSeconds <= 0) { ExpireCode(); return; }
            _lblCountdown.Text = $"Expires in {(int)rem.TotalMinutes:D2}:{rem.Seconds:D2}";
            _lblCountdown.ForeColor = rem.TotalMinutes < 2 ? Color.FromArgb(200, 100, 0) : LabelGray;
        }

        private void ExpireCode()
        {
            _isExpired = true;
            SetStatus(true, false);
            _lblCountdown.Text = "Code expired — click Refresh";
            DrawQr();
            _expiryTimer?.Stop();
            _countdownTimer?.Stop();
            QrExpired?.Invoke(this, EventArgs.Empty);
        }

        // ── Timers Events ────────────────────────────────────────────────────
        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            UpdateCountdown();
            if (_lblClockVal != null && !_lblClockVal.IsDisposed)
                _lblClockVal.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void ExpiryTimer_Tick(object sender, EventArgs e)
        {
            _expiryTimer.Stop();
            ExpireCode();
        }

        private void RestartExpiry()
        {
            _expiryTimer?.Stop();
            if (_expiryTimer != null)
            {
                _expiryTimer.Interval = Math.Max(100, _expiryMinutes * 60 * 1000);
                _expiryTimer.Start();
            }
            _countdownTimer?.Start();
        }

        // ── Refresh anim ─────────────────────────────────────────────────────
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            _btnRefresh.Enabled = false;
            SetStatus(false, true);
            _animStep = 0;
            _animTimer.Start();
        }

        private void AnimTick(object sender, EventArgs e)
        {
            _animStep++;
            _lblStatus.Text = _animStep % 2 == 0 ? "● Refreshing…" : "○ Refreshing…";
            if (_animStep >= 3) { _animTimer.Stop(); _btnRefresh.Enabled = true; GenerateNew(); }
        }

        // ── Download ─────────────────────────────────────────────────────────
        private void BtnDownload_Click(object sender, EventArgs e)
        {
            if (_picQr?.Image == null) return;
            using var sfd = new SaveFileDialog
            {
                Title = "Save QR Code as PNG",
                Filter = "PNG Image (*.png)|*.png",
                FileName = $"QR_Attendance_{_attendanceDate:yyyyMMdd}_{_session.Replace(" ", "_")}.png",
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;
            try
            {
                const int SZ = 480;
                var bmp = new Bitmap(SZ, SZ);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    float cell = SZ / (float)QR_MODULES;
                    var rng = new Random(_seed);
                    for (int r = 0; r < QR_MODULES; r++)
                        for (int c = 0; c < QR_MODULES; c++)
                        {
                            bool tl = r < 8 && c < 8, tr = r < 8 && c >= QR_MODULES - 8, bl = r >= QR_MODULES - 8 && c < 8;
                            if (tl || tr || bl) continue;
                            if (r == 6 || c == 6) { if ((r + c) % 2 == 0) g.FillRectangle(Brushes.Black, c * cell, r * cell, cell - 1, cell - 1); continue; }
                            if (rng.NextDouble() > 0.52) g.FillRectangle(Brushes.Black, c * cell, r * cell, cell - 1, cell - 1);
                        }
                    Finder(g, cell, 0, 0);
                    Finder(g, cell, QR_MODULES - 7, 0);
                    Finder(g, cell, 0, QR_MODULES - 7);

                    using var wf = new Font("Segoe UI", 11f, FontStyle.Bold);
                    var sf = new StringFormat { Alignment = StringAlignment.Center };
                    g.DrawString($"PUPAcadPortal  •  {_attendanceDate:MMM dd, yyyy}  •  {_session}",
                        wf, new SolidBrush(Color.FromArgb(155, 155, 155)),
                        new RectangleF(0, SZ - 28, SZ, 26), sf);
                }
                bmp.Save(sfd.FileName, ImageFormat.Png);

                // Flash button
                string orig = _btnDownload.Text;
                _btnDownload.Text = "✓  Saved!";
                _btnDownload.BackColor = Color.FromArgb(34, 139, 34);
                var t = new System.Windows.Forms.Timer { Interval = 2000 };
                t.Tick += (s2, e2) =>
                {
                    t.Stop(); t.Dispose();
                    if (!_btnDownload.IsDisposed)
                    { _btnDownload.Text = orig; _btnDownload.BackColor = Color.FromArgb(40, 100, 180); }
                };
                t.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not save:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}