using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using PUPAcadPortal.Models;
using PUPAcadPortal.Services;

namespace PUPAcadPortal.PortalContents.Instructor.LMS
{

    public partial class QrCodeAttendanceControl : UserControl
    {
        //  Colours 
        private static readonly Color ActiveGreen = Color.FromArgb(34, 139, 34);
        private static readonly Color ExpiredRed = Color.FromArgb(200, 30, 30);
        private static readonly Color OrangeAnim = Color.FromArgb(220, 120, 0);
        private static readonly Color BorderGray = Color.FromArgb(220, 220, 220);
        private static readonly Color LabelGray = Color.FromArgb(90, 90, 90);

        private const int DEFAULT_EXPIRY = 10; // minutes

        // ── Session binding properties ────────────────────────────────────────────
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SessionId
        {
            get => _sessionId;
            set => _sessionId = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SubjectOfferingId
        {
            get => _subjectOfferingId;
            set => _subjectOfferingId = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeSpan? SessionStartTime
        {
            get => _sessionStartTime;
            set => _sessionStartTime = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeSpan? SessionEndTime
        {
            get => _sessionEndTime;
            set => _sessionEndTime = value;
        }

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
            set => _expiryMinutes = Math.Max(1, value);
        }

        public event EventHandler? QrExpired;

        // ── Private state ─────────────────────────────────────────────────────────
        private string _session = "Morning";
        private DateTime _attendanceDate = DateTime.Today;
        private int _expiryMinutes = DEFAULT_EXPIRY;
        private int _sessionId = 0;
        private string _subjectOfferingId = string.Empty;
        private TimeSpan? _sessionStartTime;
        private TimeSpan? _sessionEndTime;
        private bool _isExpired;
        private int _animStep;
        private string _currentToken = string.Empty;
        private Bitmap? _qrBitmap;

        /// <summary>QrSession.QrSessionId of the currently active DB row (0 = none).</summary>
        private int _activeQrSessionDbId = 0;

        private DateTime _generatedAt = DateTime.Now;

        // ── Constructor ───────────────────────────────────────────────────────────
        public QrCodeAttendanceControl()
        {
            InitializeComponent();
            _lblDateVal.Text = _attendanceDate.ToString("MMMM dd, yyyy");
            _lblClockVal.Text = DateTime.Now.ToString("hh:mm:ss tt");

            Reposition();
            // Do NOT call GenerateNew() in the constructor — the parent form must
            // set SessionId, SubjectOfferingId, and AttendanceDate first.
            _countdownTimer.Start();
        }

        // ── Core: Generate or reuse the active QR session from DB ─────────────────
        /// <summary>
        /// Persists (or reuses) a QrSession row in the database, then renders the
        /// cryptographically signed token as a QR code image.
        ///
        /// If an active unexpired token already exists for this ClassSession:
        ///   • The existing token is reused (no new DB row, no new QR image needed).
        ///   • The UI countdown reflects the remaining time from the DB.
        ///   • A "Reused existing QR" notice is shown briefly.
        ///
        /// If no active token exists a fresh one is created and saved.
        /// </summary>
        public void GenerateNew()
        {
            _isExpired = false;

            // Design-time / demo guard
            if (_sessionId <= 0 || string.IsNullOrWhiteSpace(_subjectOfferingId))
            {
                _currentToken = QrTokenService.Build(
                    sessionId: -1,
                    subjectOfferingId: "DEMO",
                    sessionDate: _attendanceDate,
                    startTime: _sessionStartTime,
                    endTime: _sessionEndTime);
                RenderToken(_currentToken);
                SetStatus(false, false);
                UpdateCountdown();
                RestartExpiryTimer(_expiryMinutes * 60);
                return;
            }

            try
            {
                var qrSession = QrSessionService.CreateOrGetActive(
                    sessionId: _sessionId,
                    subjectOfferingId: _subjectOfferingId,
                    sessionDate: _attendanceDate,
                    startTime: _sessionStartTime,
                    endTime: _sessionEndTime,
                    expiryMinutes: _expiryMinutes,
                    alreadyActive: out bool reused);

                _activeQrSessionDbId = qrSession.QrSessionId;
                _currentToken = qrSession.Token;
                _generatedAt = qrSession.GeneratedAt.ToLocalTime();

                RenderToken(_currentToken);
                SetStatus(false, false);
                UpdateCountdown();

                // Start the expiry timer for the remaining time from the DB
                double remainingSeconds = (qrSession.ExpiresAt - DateTime.UtcNow).TotalSeconds;
                RestartExpiryTimer(Math.Max(1, (int)remainingSeconds));

                if (reused)
                {
                    // Briefly inform the instructor that the existing code is still valid
                    string origText = _lblStatus.Text;
                    _lblStatus.Text = "● Reusing active QR";
                    _lblStatus.ForeColor = Color.FromArgb(0, 100, 180);
                    var t = new System.Windows.Forms.Timer { Interval = 2500 };
                    t.Tick += (s, e) =>
                    {
                        t.Stop(); t.Dispose();
                        if (!_lblStatus.IsDisposed)
                        {
                            _lblStatus.Text = "● Active";
                            _lblStatus.ForeColor = ActiveGreen;
                        }
                    };
                    t.Start();
                }
            }
            catch (Exception ex)
            {
                // Graceful fallback: generate an in-memory token so the UI stays functional
                _currentToken = QrTokenService.Build(
                    _sessionId, _subjectOfferingId, _attendanceDate,
                    _sessionStartTime, _sessionEndTime);
                RenderToken(_currentToken);
                SetStatus(false, false);
                UpdateCountdown();
                RestartExpiryTimer(_expiryMinutes * 60);

                System.Diagnostics.Debug.WriteLine(
                    $"[QrCodeAttendanceControl] DB write failed, using in-memory token: {ex.Message}");
            }
        }

        // ── Render helpers ────────────────────────────────────────────────────────
        private void RenderToken(string token)
        {
            int sz = _picQr.Width > 0 ? _picQr.Width : 300;
            _qrBitmap?.Dispose();
            _qrBitmap = RenderQrBitmap(token, sz);
            _picQr.Image = _qrBitmap;
        }

        private static Bitmap RenderQrBitmap(string content, int sizePixels)
        {
            try
            {
                var writer = new BarcodeWriterPixelData
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new QrCodeEncodingOptions
                    {
                        Width = sizePixels,
                        Height = sizePixels,
                        Margin = 1,
                        ErrorCorrection = ErrorCorrectionLevel.M,
                        CharacterSet = "UTF-8",
                    },
                };

                var pixelData = writer.Write(content);
                var bmp = new Bitmap(pixelData.Width, pixelData.Height,
                    System.Drawing.Imaging.PixelFormat.Format32bppRgb);

                var bmpData = bmp.LockBits(
                    new Rectangle(0, 0, bmp.Width, bmp.Height),
                    System.Drawing.Imaging.ImageLockMode.WriteOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppRgb);

                try
                {
                    System.Runtime.InteropServices.Marshal.Copy(
                        pixelData.Pixels, 0, bmpData.Scan0, pixelData.Pixels.Length);
                }
                finally
                {
                    bmp.UnlockBits(bmpData);
                }

                return bmp;
            }
            catch
            {
                var fallback = new Bitmap(sizePixels, sizePixels);
                using var g = Graphics.FromImage(fallback);
                g.Clear(Color.White);
                using var f = new Font("Segoe UI", 9f);
                g.DrawString("QR unavailable", f, Brushes.Red, 4, 4);
                return fallback;
            }
        }

        // ── Layout / resize ───────────────────────────────────────────────────────
        private void QrCodeAttendanceControl_SizeChanged(object sender, EventArgs e)
            => Reposition();

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

            if (!string.IsNullOrEmpty(_currentToken) && _picQr.Width > 0)
            {
                _qrBitmap?.Dispose();
                _qrBitmap = _isExpired
                    ? RenderExpiredBitmap(_picQr.Width)
                    : RenderQrBitmap(_currentToken, _picQr.Width);
                _picQr.Image = _qrBitmap;
            }
        }

        private void infoBar_Paint(object sender, PaintEventArgs e)
        {
            using var p = new Pen(BorderGray);
            e.Graphics.DrawLine(p, 0, 0, ((Panel)sender).Width, 0);
        }

        // ── Expired overlay ───────────────────────────────────────────────────────
        private static Bitmap RenderExpiredBitmap(int size)
        {
            var bmp = new Bitmap(size, size);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.FromArgb(240, 240, 240));
            g.FillRectangle(new SolidBrush(Color.FromArgb(180, 255, 255, 255)), 0, 0, size, size);
            using var rb = new SolidBrush(Color.FromArgb(200, 30, 30));
            using var fnt = new Font("Segoe UI", size * 0.09f, FontStyle.Bold);
            var sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
            };
            using var pen = new System.Drawing.Pen(Color.FromArgb(80, 200, 30, 30), size * 0.006f);
            g.DrawRectangle(pen, size * 0.12f, size * 0.40f, size * 0.76f, size * 0.20f);
            g.DrawString("EXPIRED", fnt, rb, new RectangleF(0, 0, size, size), sf);
            return bmp;
        }

        // ── Status / countdown helpers ────────────────────────────────────────────
        private void SetStatus(bool expired, bool refreshing)
        {
            if (refreshing)
            { _lblStatus.Text = "● Refreshing…"; _lblStatus.ForeColor = OrangeAnim; }
            else if (expired)
            { _lblStatus.Text = "● Expired"; _lblStatus.ForeColor = ExpiredRed; }
            else
            { _lblStatus.Text = "● Active"; _lblStatus.ForeColor = ActiveGreen; }
        }

        private void UpdateCountdown()
        {
            if (_isExpired)
            {
                _lblCountdown.Text = "Code expired — click Refresh";
                return;
            }
            var rem = TimeSpan.FromMinutes(_expiryMinutes) - (DateTime.Now - _generatedAt);
            if (rem.TotalSeconds <= 0) { ExpireCode(); return; }
            _lblCountdown.Text = $"Expires in {(int)rem.TotalMinutes:D2}:{rem.Seconds:D2}";
            _lblCountdown.ForeColor = rem.TotalMinutes < 2
                ? Color.FromArgb(200, 100, 0)
                : LabelGray;
        }

        private void ExpireCode()
        {
            _isExpired = true;
            SetStatus(true, false);
            _lblCountdown.Text = "Code expired — click Refresh";

            _qrBitmap?.Dispose();
            _qrBitmap = RenderExpiredBitmap(_picQr.Width > 0 ? _picQr.Width : 300);
            _picQr.Image = _qrBitmap;

            _expiryTimer?.Stop();
            _countdownTimer?.Stop();

            // Sync the DB row
            if (_activeQrSessionDbId > 0)
            {
                try { QrSessionService.MarkExpired(_activeQrSessionDbId); }
                catch { /* best-effort */ }
            }

            QrExpired?.Invoke(this, EventArgs.Empty);
        }

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

        private void RestartExpiryTimer(int remainingSeconds)
        {
            _expiryTimer?.Stop();
            if (_expiryTimer != null)
            {
                int ms = Math.Max(100, remainingSeconds * 1000);
                _expiryTimer.Interval = ms;
                _expiryTimer.Start();
            }
            _countdownTimer?.Start();
        }

        // ── Refresh button ────────────────────────────────────────────────────────
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
            if (_animStep >= 3)
            {
                _animTimer.Stop();
                _btnRefresh.Enabled = true;
                // Force-expire the current DB row so CreateOrGetActive issues a new one
                if (_activeQrSessionDbId > 0)
                {
                    try { QrSessionService.MarkExpired(_activeQrSessionDbId); }
                    catch { /* best-effort */ }
                }
                _activeQrSessionDbId = 0;
                GenerateNew();
            }
        }

        // ── Download button ───────────────────────────────────────────────────────
        private void BtnDownload_Click(object sender, EventArgs e)
        {
            if (_isExpired || string.IsNullOrEmpty(_currentToken))
            {
                MessageBox.Show("QR code has expired. Please refresh first.",
                    "Expired", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Title = "Save QR Code as PNG",
                Filter = "PNG Image (*.png)|*.png",
                FileName = $"QR_Attendance_{_attendanceDate:yyyyMMdd}_{_session.Replace(" ", "_")}.png",
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                const int SAVE_SZ = 480;
                using var bmp = RenderQrBitmap(_currentToken, SAVE_SZ);

                using var g = Graphics.FromImage(bmp);
                using var wf = new Font("Segoe UI", 11f, FontStyle.Bold);
                var sf = new StringFormat { Alignment = StringAlignment.Center };
                g.DrawString(
                    $"PUPAcadPortal  •  {_attendanceDate:MMM dd, yyyy}  •  {_session}",
                    wf, new SolidBrush(Color.FromArgb(155, 155, 155)),
                    new RectangleF(0, SAVE_SZ - 28, SAVE_SZ, 26), sf);

                bmp.Save(sfd.FileName, ImageFormat.Png);

                string orig = _btnDownload.Text;
                _btnDownload.Text = "✓  Saved!";
                _btnDownload.BackColor = Color.FromArgb(34, 139, 34);
                var t = new System.Windows.Forms.Timer { Interval = 2000 };
                t.Tick += (s2, e2) =>
                {
                    t.Stop(); t.Dispose();
                    if (!_btnDownload.IsDisposed)
                    {
                        _btnDownload.Text = orig;
                        _btnDownload.BackColor = Color.FromArgb(40, 100, 180);
                    }
                };
                t.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not save:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}