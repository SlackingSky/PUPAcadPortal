using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using PUPAcadPortal.Models;
using PUPAcadPortal.Services;

namespace PUPAcadPortal.PortalContents.Instructor.LMS
{

    public sealed partial class QrCodePopupForm : Form
    {
        //  Colours 
        private static readonly Color Maroon = Color.FromArgb(106, 0, 0);
        private static readonly Color ActiveGreen = Color.FromArgb(34, 139, 34);
        private static readonly Color ExpiredRed = Color.FromArgb(200, 30, 30);
        private static readonly Color OrangeBadge = Color.FromArgb(220, 120, 0);
        private static readonly Color BorderColor = Color.FromArgb(220, 220, 220);
        private static readonly Color SubText = Color.FromArgb(90, 90, 90);

        private const int EXPIRY_MINUTES = 10;

        //  Session data 
        private readonly string _course;
        private readonly string _session;
        private readonly DateTime _date;
        private readonly int _sessionId;
        private readonly string _offeringId;
        private readonly TimeSpan? _startTime;
        private readonly TimeSpan? _endTime;

        //  Internal state 
        private bool _isExpired;
        private int _animStep;

        private QrCodeAttendanceControl? _qrCodeControl = null;

        private DateTime _expiresAtUtc = DateTime.UtcNow;

        public QrCodePopupForm(
            string course,
            string session,
            DateTime date,
            int sessionId = 0,
            string offeringId = "",
            TimeSpan? startTime = null,
            TimeSpan? endTime = null)
        {
            InitializeComponent();

            _course = course;
            _session = session;
            _date = date;
            _sessionId = sessionId;
            _offeringId = offeringId;
            _startTime = startTime;
            _endTime = endTime;

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
                e.Graphics.DrawRectangle(pen, 1, 1,
                    pnlQrCard.Width - 3, pnlQrCard.Height - 3);
            };

            pnlInfo.Paint += (s, e) =>
            {
                e.Graphics.DrawLine(new Pen(BorderColor), 0, 0, pnlInfo.Width, 0);
                e.Graphics.DrawLine(new Pen(BorderColor), 0, pnlInfo.Height - 1, pnlInfo.Width, pnlInfo.Height - 1);
            };

            pnlFooter.Paint += (s, e) =>
                e.Graphics.DrawLine(new Pen(BorderColor), 0, 0, pnlFooter.Width, 0);

            BindSessionToQrControl();
            GenerateNew();
            _tick.Start();
        }

        //  Bind session data to the embedded QR control 
        private void BindSessionToQrControl()
        {
            var qrCtl = FindQrControl();
            if (qrCtl == null) return;

            qrCtl.SessionId = _sessionId;
            qrCtl.SubjectOfferingId = _offeringId;
            qrCtl.SessionStartTime = _startTime;
            qrCtl.SessionEndTime = _endTime;
            qrCtl.Session = _session;
            qrCtl.AttendanceDate = _date;
            qrCtl.ExpiryMinutes = EXPIRY_MINUTES;
        }

        //  Find the QrCodeAttendanceControl hosted in this popup 
        private QrCodeAttendanceControl? FindQrControl()
        {
            if (_qrCodeControl != null) return _qrCodeControl;
            foreach (Control c in pnlQrCard.Controls)
                if (c is QrCodeAttendanceControl qrc) return qrc;
            return null;
        }

        //  Generate / reuse QR 
        public void GenerateNew()
        {
            _isExpired = false;

            var qrCtl = FindQrControl();
            if (qrCtl != null)
            {
                BindSessionToQrControl();
                qrCtl.QrExpired -= OnQrControlExpired;
                qrCtl.QrExpired += OnQrControlExpired;
                qrCtl.GenerateNew();

                // Sync the popup-level expiry from the DB row so TickCountdown
                // shows accurate remaining time even on reopen.
                SyncExpiryFromDb();
            }
            else
            {
                RenderQrFallback();
                _expiresAtUtc = DateTime.UtcNow.AddMinutes(EXPIRY_MINUTES);
            }

            SetStatus("● Active", ActiveGreen);
            TickCountdown();
        }

        private void SyncExpiryFromDb()
        {
            if (_sessionId <= 0) return;
            try
            {
                var active = QrSessionService.GetActive(_sessionId);
                if (active != null)
                    _expiresAtUtc = active.ExpiresAt;
                else
                    _expiresAtUtc = DateTime.UtcNow.AddMinutes(EXPIRY_MINUTES);
            }
            catch
            {
                _expiresAtUtc = DateTime.UtcNow.AddMinutes(EXPIRY_MINUTES);
            }
        }

        private void OnQrControlExpired(object? sender, EventArgs e)
        {
            _isExpired = true;
            _tick.Stop();
            SetStatus("● Expired", ExpiredRed);
            SafeSetLabel(lblCountdown,
                "Code expired — click Refresh to generate a new one");
        }

        //  Fallback inline render (used when QrCodeAttendanceControl is absent) 
        private void RenderQrFallback()
        {
            string token;
            bool builtInMemory = false;

            try
            {
                var active = _sessionId > 0 ? QrSessionService.GetActive(_sessionId) : null;
                if (active != null)
                {
                    token = active.Token;
                    _expiresAtUtc = active.ExpiresAt;
                }
                else
                {
                    token = QrTokenService.Build(
                        _sessionId, _offeringId, _date, _startTime, _endTime);
                    _expiresAtUtc = DateTime.UtcNow.AddMinutes(EXPIRY_MINUTES);
                    builtInMemory = true;
                }
            }
            catch
            {
                token = QrTokenService.Build(
                    _sessionId, _offeringId, _date, _startTime, _endTime);
                _expiresAtUtc = DateTime.UtcNow.AddMinutes(EXPIRY_MINUTES);
                builtInMemory = true;
            }

            const int SZ = 260;
            try
            {
                var writer = new ZXing.BarcodeWriterPixelData
                {
                    Format = ZXing.BarcodeFormat.QR_CODE,
                    Options = new ZXing.QrCode.QrCodeEncodingOptions
                    {
                        Width = SZ,
                        Height = SZ,
                        Margin = 1,
                        ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.M,
                        CharacterSet = "UTF-8",
                    },
                };
                var pd = writer.Write(token);
                var bmp = new System.Drawing.Bitmap(pd.Width, pd.Height,
                    System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                var bd = bmp.LockBits(
                    new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                    System.Drawing.Imaging.ImageLockMode.WriteOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                System.Runtime.InteropServices.Marshal.Copy(
                    pd.Pixels, 0, bd.Scan0, pd.Pixels.Length);
                bmp.UnlockBits(bd);
                _picQr.Image = bmp;
                _currentToken = token;
            }
            catch
            {
                var fb = new Bitmap(SZ, SZ);
                using var g = Graphics.FromImage(fb);
                g.Clear(Color.White);
                g.DrawString("QR unavailable", new Font("Segoe UI", 10f), Brushes.Red, 8, 8);
                _picQr.Image = fb;
            }
        }

        private string _currentToken = string.Empty;

        //  Status / countdown helpers 
        private void SetStatus(string text, Color color)
        {
            SafeSetLabel(lblStatus, text);
            if (lblStatus.InvokeRequired)
                lblStatus.Invoke((Action)(() => lblStatus.ForeColor = color));
            else
                lblStatus.ForeColor = color;
        }

        private static void SafeSetLabel(Label lbl, string text)
        {
            if (lbl.InvokeRequired)
                lbl.Invoke((Action)(() => lbl.Text = text));
            else
                lbl.Text = text;
        }

        private DateTime _generatedAt = DateTime.Now;

        private void TickCountdown()
        {
            if (_isExpired)
            {
                lblCountdown.Text = "Code has expired — click Refresh to generate a new one";
                return;
            }

            // Use the DB expiry time so the countdown is accurate even after
            // the popup is closed and reopened mid-session.
            var remaining = _expiresAtUtc - DateTime.UtcNow;
            if (remaining.TotalSeconds <= 0) { ExpireNow(); return; }
            lblCountdown.Text = $"Expires in  {(int)remaining.TotalMinutes:D2}:{remaining.Seconds:D2}";
            lblCountdown.ForeColor = remaining.TotalMinutes < 2
                ? Color.FromArgb(200, 100, 0)
                : SubText;
        }

        private void ExpireNow()
        {
            _isExpired = true;
            _tick.Stop();
            SetStatus("● Expired", ExpiredRed);
            SafeSetLabel(lblCountdown,
                "Code expired — click Refresh to generate a new one");
        }

        //  Refresh button 
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
                _generatedAt = DateTime.Now;
                GenerateNew();
                _tick.Start();
                _btnRefresh.Enabled = true;
            }
        }

        //  Download button 

        private static string SanitizeFileName(string raw)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                raw = raw.Replace(c, '_');
            return raw;
        }

        private void BtnDownload_Click(object? sender, EventArgs e)
        {
            if (_isExpired)
            {
                MessageBox.Show("QR code has expired. Please refresh first.",
                    "Expired", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Bitmap? sourceBmp = FindQrControl()?.GetQrBitmap();

            string safeSession = SanitizeFileName(_session);
            using var sfd = new SaveFileDialog
            {
                Title = "Save QR Code",
                Filter = "PNG Image (*.png)|*.png",
                FileName = $"QR_Attendance_{_date:yyyyMMdd}_{safeSession}.png",
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                const int SAVE = 560;
                Bitmap saveBmp;

                if (sourceBmp != null)
                {
                    saveBmp = new Bitmap(sourceBmp, SAVE, SAVE);
                }
                else
                {
                    // Fallback: use the token from the sub-control or the local token
                    string token = FindQrControl()?.GetCurrentToken()
                                   ?? (string.IsNullOrEmpty(_currentToken)
                                       ? QrTokenService.Build(_sessionId, _offeringId, _date, _startTime, _endTime)
                                       : _currentToken);

                    var writer = new ZXing.BarcodeWriterPixelData
                    {
                        Format = ZXing.BarcodeFormat.QR_CODE,
                        Options = new ZXing.QrCode.QrCodeEncodingOptions
                        {
                            Width = SAVE,
                            Height = SAVE,
                            Margin = 1,
                            CharacterSet = "UTF-8",
                        },
                    };
                    var pd = writer.Write(token);
                    saveBmp = new Bitmap(pd.Width, pd.Height,
                        System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                    var bd = saveBmp.LockBits(
                        new Rectangle(0, 0, saveBmp.Width, saveBmp.Height),
                        ImageLockMode.WriteOnly,
                        System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                    System.Runtime.InteropServices.Marshal.Copy(
                        pd.Pixels, 0, bd.Scan0, pd.Pixels.Length);
                    saveBmp.UnlockBits(bd);
                }

                saveBmp.Save(sfd.FileName, ImageFormat.Png);
                saveBmp.Dispose();
                sourceBmp?.Dispose();

                MessageBox.Show($"QR Code saved:\n{sfd.FileName}",
                    "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                sourceBmp?.Dispose();
                MessageBox.Show($"Could not save: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //  Close 
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _tick?.Stop(); _tick?.Dispose();
            anim?.Stop(); anim?.Dispose();
            base.OnFormClosed(e);
        }
    }
}