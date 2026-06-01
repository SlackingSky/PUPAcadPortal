using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;

namespace PUPAcadPortal.PortalContents.Student.LMS.Attendance
{
    public class QRScanResult
    {
        public string RawText { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string Period { get; set; }
        public string Session { get; set; }
        public DateTime ScanTime { get; set; }
        public bool IsValid { get; set; }
    }

    public partial class QRScanControl : UserControl
    {
        public event EventHandler<QRScanResult> QRCodeScanned;

        private FilterInfoCollection _videoDevices;
        private VideoCaptureDevice _videoSource;
        private Bitmap _currentFrame;
        private bool _cameraRunning = false;
        private readonly object _frameLock = new object();
        private System.Windows.Forms.Timer _scanTimer;

        private string _lastRaw = null;
        private DateTime _lastScanTime = DateTime.MinValue;
        private const int DEBOUNCE_MS = 2500;

        private enum ScanMode { Upload, Camera }
        private ScanMode _mode = ScanMode.Upload;
        private System.Data.DataTable _historyDT;

        public QRScanControl()
        {
            InitializeComponent();
            BuildInfoSidebarContent();
        }

        private void QRScanControl_Load(object sender, EventArgs e)
        {
            SetupHistoryGrid();
            SwitchMode(ScanMode.Upload);
            SeedDemoHistory();
        }

        private void MainAreaPanel_Resize(object sender, EventArgs e)
        {
            if (mainAreaPanel == null || pnlUpload == null) return;

            int w = mainAreaPanel.ClientSize.Width - 36;
            int sideW = Math.Min(300, Math.Max(200, w / 3));
            int scanW = w - sideW - 16;
            int scanH = mainAreaPanel.ClientSize.Height - 60;

            pnlUpload.SetBounds(18, 14, scanW, scanH);
            pnlCamera.SetBounds(18, 14, scanW, scanH);

            if (picQR != null)
                picQR.SetBounds(8, 8, pnlUpload.Width - 16, pnlUpload.Height - 16);

            int btY = 14 + scanH + 10;
            btnUpload.Location = new Point(18, btY);
            btnClearImage.Location = new Point(228, btY);

            cmbCamera.Location = new Point(18, btY);
            cmbCamera.Width = scanW - 230;
            btnStartCamera.Location = new Point(18 + scanW - 210, btY);
            btnStopCamera.Location = new Point(18 + scanW - 90, btY);

            pnlInfoSide.SetBounds(18 + scanW + 16, 14, sideW, mainAreaPanel.ClientSize.Height - 28);
        }

        private void Header_Paint(object sender, PaintEventArgs e)
        {
            var p = (Panel)sender;
            e.Graphics.DrawLine(new Pen(Color.FromArgb(225, 225, 235)), 0, p.Height - 1, p.Width, p.Height - 1);
        }

        private void Toggle_Paint(object sender, PaintEventArgs e)
        {
            var p = (Panel)sender;
            e.Graphics.DrawLine(new Pen(Color.FromArgb(225, 225, 235)), 0, p.Height - 1, p.Width, p.Height - 1);
        }

        private void Pill_Paint(object sender, PaintEventArgs e)
        {
            var pnl = (Panel)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = RoundedRect(new Rectangle(0, 0, pnl.Width - 1, pnl.Height - 1), 8))
            using (var b = new SolidBrush(Color.FromArgb(240, 240, 245)))
                e.Graphics.FillPath(b, path);
        }

        private void Result_Paint(object sender, PaintEventArgs e)
        {
            var pnl = (Panel)sender;
            e.Graphics.DrawLine(new Pen(Color.FromArgb(225, 225, 235)), 0, 0, pnl.Width, 0);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(225, 225, 235)), 0, pnl.Height - 1, pnl.Width, pnl.Height - 1);
        }

        private void TitleBar_Paint(object sender, PaintEventArgs e)
        {
            var p = (Panel)sender;
            e.Graphics.DrawLine(new Pen(Color.FromArgb(225, 225, 235)), 0, p.Height - 1, p.Width, p.Height - 1);
        }

        private void PnlInfoSide_Paint(object sender, PaintEventArgs e)
        {
            var pnl = (Panel)sender;
            using (var pen = new Pen(Color.FromArgb(220, 220, 235)))
                e.Graphics.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
        }

        private void PnlUpload_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var pan = pnlUpload;

            g.SmoothingMode = SmoothingMode.AntiAlias;

            bool hasImage = picQR.Image != null;
            using (var borderPen = new Pen(hasImage ? Color.FromArgb(128, 0, 0) : Color.FromArgb(200, 200, 215), 1.5f))
            {
                borderPen.DashStyle = hasImage ? DashStyle.Solid : DashStyle.Dash;
                using (var path = RoundedRect(new Rectangle(1, 1, pan.Width - 3, pan.Height - 3), 10))
                    g.DrawPath(borderPen, path);
            }

            if (!hasImage)
            {
                const string icon = "⊞";
                using (var iconFont = new Font("Segoe UI", 36f))
                {
                    var iconSize = g.MeasureString(icon, iconFont);
                    g.DrawString(icon, iconFont, new SolidBrush(Color.FromArgb(210, 210, 220)),
                        (pan.Width - iconSize.Width) / 2f, (pan.Height - iconSize.Height) / 2f - 24f);
                }

                const string msg = "Drag & drop a QR image here\nor click \"Browse\"";
                using (var msgFont = new Font("Segoe UI", 10f))
                {
                    var msgSize = g.MeasureString(msg, msgFont);
                    g.DrawString(msg, msgFont, new SolidBrush(Color.FromArgb(160, 160, 180)),
                        (pan.Width - msgSize.Width) / 2f, (pan.Height - msgSize.Height) / 2f + 30f);
                }
            }
        }

        private void BuildInfoSidebarContent()
        {
            var title = new Label { Text = "How to use", Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = Color.FromArgb(128, 0, 0), AutoSize = true, Dock = DockStyle.Top, Height = 28 };
            var sep1 = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = Color.FromArgb(230, 230, 240) };
            var stepsPanel = new Panel { Dock = DockStyle.Top, AutoSize = true, BackColor = Color.Transparent };

            var steps = new[] {
                ("1", "Upload Mode", "Click \"Browse\" or drag-and-drop a QR code image. The scanner decodes it automatically."),
                ("2", "Camera Mode", "Select your webcam, click \"Start Camera\". Hold the QR code up — detection is automatic."),
                ("3", "Confirm", "A confirmation dialog appears. Click \"Mark Attendance\" to save.")
            };

            int sy = 4;
            foreach (var (num, head, body) in steps)
            {
                var numLbl = new Label { Text = num, Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.White, BackColor = Color.FromArgb(128, 0, 0), TextAlign = ContentAlignment.MiddleCenter, Size = new Size(20, 20), Location = new Point(0, sy + 2) };
                var hLbl = new Label { Text = head, Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(50, 50, 70), AutoSize = true, Location = new Point(28, sy) };
                var bLbl = new Label { Text = body, Font = new Font("Segoe UI", 8.5f), ForeColor = Color.FromArgb(100, 100, 120), AutoSize = false, Location = new Point(28, sy + 20), Width = 220, Height = 46 };
                stepsPanel.Controls.AddRange(new Control[] { numLbl, hLbl, bLbl });
                sy += 74;
            }
            stepsPanel.Height = sy + 4;

            var sep2 = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = Color.FromArgb(230, 230, 240) };
            var fmtTitle = new Label { Text = "QR Payload format", Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(80, 80, 100), AutoSize = true, Dock = DockStyle.Top, Height = 26 };
            var fmtBox = new Panel { BackColor = Color.FromArgb(248, 248, 252), Dock = DockStyle.Top, Height = 72, Padding = new Padding(8) };
            var fmtLbl = new Label { Text = "COURSE_CODE|COURSE_NAME|\nPERIOD|SESSION\n\nExample:\nCOMP012|Network Admin|\nMidterm|Lab Session", Font = new Font("Consolas", 7.5f), ForeColor = Color.FromArgb(60, 60, 90), Dock = DockStyle.Fill, AutoSize = false };

            fmtBox.Controls.Add(fmtLbl);
            pnlInfoSide.Controls.AddRange(new Control[] { fmtBox, fmtTitle, sep2, stepsPanel, sep1, title });
        }

        private void SetupHistoryGrid()
        {
            ApplyGridStyle(dgvHistory);

            dgvHistory.Columns.Add(FixedCol("Time", "Time", 80, true));
            dgvHistory.Columns.Add(FixedCol("Course Code", "Code", 90, false));
            dgvHistory.Columns.Add(FillCol("Course Name", "Course Name", false));
            dgvHistory.Columns.Add(FixedCol("Period", "Period", 90, true));
            dgvHistory.Columns.Add(FillCol("Session", "Session", false));
            dgvHistory.Columns.Add(FixedCol("Status", "Status", 85, true));

            _historyDT = new System.Data.DataTable();
            _historyDT.Columns.Add("Time");
            _historyDT.Columns.Add("Course Code");
            _historyDT.Columns.Add("Course Name");
            _historyDT.Columns.Add("Period");
            _historyDT.Columns.Add("Session");
            _historyDT.Columns.Add("Status");
            dgvHistory.DataSource = _historyDT;
        }

        private void DgvHistory_SelectionChanged(object sender, EventArgs e) => dgvHistory.ClearSelection();
        private void DgvHistory_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            int rh = dgvHistory.Rows.GetRowsHeight(DataGridViewElementStates.Visible);
            int desired = dgvHistory.ColumnHeadersHeight + rh + 2;
            dgvHistory.Height = Math.Min(desired, 300);
            dgvHistory.ScrollBars = desired > 300 ? ScrollBars.Vertical : ScrollBars.None;
        }

        private static void ApplyGridStyle(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(128, 0, 0);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(128, 0, 0);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(40, 40, 60);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 220, 220);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(40, 40, 60);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(251, 251, 254);
        }

        private static DataGridViewTextBoxColumn FixedCol(string name, string header, int width, bool centre) =>
            new DataGridViewTextBoxColumn { Name = name, HeaderText = header, DataPropertyName = name, Width = width, AutoSizeMode = DataGridViewAutoSizeColumnMode.None, ReadOnly = true, DefaultCellStyle = { Alignment = centre ? DataGridViewContentAlignment.MiddleCenter : DataGridViewContentAlignment.MiddleLeft } };

        private static DataGridViewTextBoxColumn FillCol(string name, string header, bool centre) =>
            new DataGridViewTextBoxColumn { Name = name, HeaderText = header, DataPropertyName = name, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true, DefaultCellStyle = { Alignment = centre ? DataGridViewContentAlignment.MiddleCenter : DataGridViewContentAlignment.MiddleLeft } };

        private void BtnModeUpload_Click(object sender, EventArgs e) => SwitchMode(ScanMode.Upload);
        private void BtnModeCamera_Click(object sender, EventArgs e) => SwitchMode(ScanMode.Camera);

        private void SwitchMode(ScanMode mode)
        {
            _mode = mode;

            if (mode != ScanMode.Camera && _cameraRunning)
                StopCamera();

            bool isUpload = (mode == ScanMode.Upload);

            pnlUpload.Visible = isUpload;
            btnUpload.Visible = isUpload;
            btnClearImage.Visible = isUpload && picQR.Image != null;

            pnlCamera.Visible = !isUpload;
            cmbCamera.Visible = !isUpload;
            btnStartCamera.Visible = !isUpload;
            btnStopCamera.Visible = !isUpload;

            if (!isUpload && _videoDevices == null)
                PopulateCameraList();

            SetToggleActive(btnModeUpload, isUpload);
            SetToggleActive(btnModeCamera, !isUpload);
        }

        private void SetToggleActive(Button btn, bool active)
        {
            btn.BackColor = active ? Color.FromArgb(128, 0, 0) : Color.Transparent;
            btn.ForeColor = active ? Color.White : Color.FromArgb(80, 80, 100);
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog { Title = "Select QR Code Image", Filter = "Image files|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.tif;*.tiff|All files|*.*" })
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                LoadAndDecodeImage(dlg.FileName);
            }
        }

        private void PnlUpload_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void PnlUpload_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files?.Length > 0) LoadAndDecodeImage(files[0]);
        }

        private void BtnClearImage_Click(object sender, EventArgs e)
        {
            picQR.Image?.Dispose();
            picQR.Image = null;
            picQR.Visible = false;
            btnClearImage.Visible = false;
            HideResult();
            pnlUpload.Invalidate();
        }

        private void LoadAndDecodeImage(string path)
        {
            try
            {
                var bmp = new Bitmap(path);
                picQR.Image = bmp;
                picQR.Visible = true;
                btnClearImage.Visible = true;
                pnlUpload.Invalidate();

                string decoded = DecodeQR(bmp);
                if (decoded != null) HandleDecode(decoded);
                else ShowResultError("No QR code detected in this image. Try a clearer or higher-resolution photo.");
            }
            catch (Exception ex)
            {
                ShowResultError($"Could not open image: {ex.Message}");
            }
        }

        private void PopulateCameraList()
        {
            try
            {
                _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                cmbCamera.Items.Clear();
                if (_videoDevices.Count == 0)
                {
                    cmbCamera.Items.Add("No cameras found");
                    btnStartCamera.Enabled = false;
                    return;
                }
                foreach (FilterInfo fi in _videoDevices) cmbCamera.Items.Add(fi.Name);
                cmbCamera.SelectedIndex = 0;
                btnStartCamera.Enabled = true;
            }
            catch
            {
                cmbCamera.Items.Add("Camera access unavailable");
                btnStartCamera.Enabled = false;
            }
        }

        private void BtnStartCamera_Click(object sender, EventArgs e)
        {
            if (cmbCamera.SelectedIndex < 0 || _videoDevices == null || _videoDevices.Count == 0) return;
            try
            {
                _videoSource = new VideoCaptureDevice(_videoDevices[cmbCamera.SelectedIndex].MonikerString);
                _videoSource.NewFrame += VideoSource_NewFrame;
                _videoSource.Start();
                _cameraRunning = true;

                lblCameraStatus.Visible = false;
                btnStartCamera.Enabled = false;
                btnStopCamera.Enabled = true;
                cmbCamera.Enabled = false;

                _scanTimer = new System.Windows.Forms.Timer { Interval = 300 };
                _scanTimer.Tick += ScanTimer_Tick;
                _scanTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open camera:\n{ex.Message}", "Camera Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnStopCamera_Click(object sender, EventArgs e) => StopCamera();

        private void StopCamera()
        {
            _scanTimer?.Stop();
            _scanTimer?.Dispose();
            _scanTimer = null;

            if (_videoSource != null && _videoSource.IsRunning)
            {
                _videoSource.SignalToStop();
                _videoSource.WaitForStop();
                _videoSource.NewFrame -= VideoSource_NewFrame;
                _videoSource = null;
            }
            _cameraRunning = false;

            if (picCamera != null) picCamera.Image = null;

            if (lblCameraStatus != null)
            {
                lblCameraStatus.Text = "Camera stopped";
                lblCameraStatus.Visible = true;
            }

            if (btnStartCamera != null) btnStartCamera.Enabled = true;
            if (btnStopCamera != null) btnStopCamera.Enabled = false;
            if (cmbCamera != null) cmbCamera.Enabled = true;
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs e)
        {
            lock (_frameLock)
            {
                _currentFrame?.Dispose();
                _currentFrame = (Bitmap)e.Frame.Clone();
            }
            try
            {
                picCamera.Invoke((Action)(() =>
                {
                    lock (_frameLock)
                    {
                        if (_currentFrame != null)
                            picCamera.Image = (Bitmap)_currentFrame.Clone();
                    }
                }));
            }
            catch { /* form closing */ }
        }

        private void ScanTimer_Tick(object sender, EventArgs e)
        {
            Bitmap frame;
            lock (_frameLock)
            {
                if (_currentFrame == null) return;
                frame = (Bitmap)_currentFrame.Clone();
            }
            using (frame)
            {
                string decoded = DecodeQR(frame);
                if (decoded == null) return;
                if (decoded == _lastRaw && (DateTime.Now - _lastScanTime).TotalMilliseconds < DEBOUNCE_MS) return;

                _lastRaw = decoded;
                _lastScanTime = DateTime.Now;
                this.Invoke((Action)(() => HandleDecode(decoded)));
            }
        }

        private static string DecodeQR(Bitmap bmp)
        {
            try
            {
                Bitmap src = bmp;
                bool needDispose = false;

                if (bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                {
                    src = new Bitmap(bmp.Width, bmp.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    needDispose = true;
                    using (var g = Graphics.FromImage(src)) g.DrawImage(bmp, 0, 0);
                }

                try
                {
                    var bmpData = src.LockBits(new Rectangle(0, 0, src.Width, src.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    int byteCount = Math.Abs(bmpData.Stride) * src.Height;
                    byte[] bytes = new byte[byteCount];
                    System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, bytes, 0, byteCount);
                    src.UnlockBits(bmpData);

                    var luminance = new RGBLuminanceSource(bytes, src.Width, src.Height, RGBLuminanceSource.BitmapFormat.BGRA32);
                    var hierarchy = new ZXing.Common.HybridBinarizer(luminance);
                    var binary = new ZXing.BinaryBitmap(hierarchy);

                    var hints = new System.Collections.Generic.Dictionary<DecodeHintType, object> {
                        { DecodeHintType.TRY_HARDER, true },
                        { DecodeHintType.POSSIBLE_FORMATS, new System.Collections.Generic.List<BarcodeFormat> { BarcodeFormat.QR_CODE } }
                    };

                    var qrReader = new ZXing.QrCode.QRCodeReader();
                    var result = qrReader.decode(binary, hints);
                    return result?.Text;
                }
                finally { if (needDispose) src.Dispose(); }
            }
            catch { return null; }
        }

        private void HandleDecode(string raw)
        {
            var result = ParsePayload(raw);


            _historyDT.Rows.Add(result.ScanTime.ToString("HH:mm:ss"), result.CourseCode, result.CourseName, result.Period, result.Session, "Recorded");
            ShowResultSuccess(result);
            QRCodeScanned?.Invoke(this, result);
        }

        private static QRScanResult ParsePayload(string raw)
        {
            var r = new QRScanResult { RawText = raw, ScanTime = DateTime.Now, IsValid = false };
            var parts = raw.Split('|');
            if (parts.Length >= 2)
            {
                r.CourseCode = parts[0].Trim();
                r.CourseName = parts.Length > 1 ? parts[1].Trim() : string.Empty;
                r.Period = parts.Length > 2 ? parts[2].Trim() : "Unknown";
                r.Session = parts.Length > 3 ? parts[3].Trim() : "Unknown";
                r.IsValid = !string.IsNullOrWhiteSpace(r.CourseCode);
            }
            else
            {
                r.CourseCode = "UNKNOWN";
                r.CourseName = raw;
                r.Period = "–";
                r.Session = "–";
                r.IsValid = true;
            }
            return r;
        }

        private void ShowResultSuccess(QRScanResult r)
        {
            pnlResultAccent.BackColor = Color.FromArgb(0, 150, 70);
            lblResultStatus.Text = "✓  Attendance Recorded via QR";
            lblResultStatus.ForeColor = Color.FromArgb(0, 120, 60);
            lblResultCourse.Text = $"{r.CourseCode}  –  {r.CourseName}";
            lblResultDetail.Text = $"Period: {r.Period}  |  Session: {r.Session}  |  {r.ScanTime:MMM d, yyyy  HH:mm}";
            lblRawPayload.Text = $"Payload: {r.RawText}";
            pnlResult.Height = 108;
            pnlResult.Visible = true;
        }

        private void ShowResultError(string msg)
        {
            pnlResultAccent.BackColor = Color.FromArgb(200, 40, 40);
            lblResultStatus.Text = "✕  Decode Failed";
            lblResultStatus.ForeColor = Color.FromArgb(180, 30, 30);
            lblResultCourse.Text = msg;
            lblResultDetail.Text = string.Empty;
            lblRawPayload.Text = string.Empty;
            pnlResult.Height = 78;
            pnlResult.Visible = true;
        }

        private void HideResult()
        {
            pnlResult.Visible = false;
            pnlResult.Height = 0;
        }

        private static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void SeedDemoHistory()
        {
            if (_historyDT == null) return;
            var rows = new[] {
                new { Time="08:12:04", Code="COMP 012", Name="Network Administration", Period="Midterm", Session="Lab Session", Status="Present" },
                new { Time="14:33:17", Code="INTE 202", Name="Interactive Programming & Tech 1", Period="Final Term", Session="Lecture 2", Status="Present" }
            };
            foreach (var r in rows) _historyDT.Rows.Add(r.Time, r.Code, r.Name, r.Period, r.Session, r.Status);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            StopCamera();
            base.OnHandleDestroyed(e);
        }
    }
}