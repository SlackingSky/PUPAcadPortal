using CloudinaryDotNet.Actions;
using Org.BouncyCastle.Asn1.Cmp;
using PUPAcadPortal.PortalContents.Student.LMS.Attendance;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Student.LMS.Attendance
{
    public partial class QRConfirmDialog : Form
    {
        private readonly QRScanResult _result;

        // Drag support
        private bool _drag;
        private System.Drawing.Point _dragStart;

        public QRConfirmDialog(QRScanResult result)
        {
            InitializeComponent();
            _result = result;

            // Wire up rounding events
            this.Resize += (s, e) => ApplyRounded(12);
            this.Shown += (s, e) => ApplyRounded(12);

            PopulateDynamicData();
        }

        private void PopulateDynamicData()
        {
            // Update colors and text based on validation state
            iconStrip.BackColor = _result.IsValid
                ? Color.FromArgb(242, 255, 248)
                : Color.FromArgb(255, 243, 243);

            lblStatus.Text = _result.IsValid ? "QR Code Decoded Successfully" : "QR Code Format Not Recognised";
            lblStatus.ForeColor = _result.IsValid ? Color.FromArgb(0, 110, 50) : Color.FromArgb(160, 30, 30);

            lblSub.Text = _result.IsValid
                ? "Review the details below and confirm to record your attendance."
                : "The QR code was decoded but the content format is unrecognised.";

            // Populate Grid Data
            var rows = new[]
            {
                ("Course Code",  _result.CourseCode ?? "–"),
                ("Course Name",  _result.CourseName ?? "–"),
                ("Period",       _result.Period     ?? "–"),
                ("Session",      _result.Session    ?? "–"),
                ("Scan Time",    _result.ScanTime.ToString("MMMM d, yyyy  HH:mm:ss")),
            };

            grid.RowCount = rows.Length;
            for (int i = 0; i < rows.Length; i++)
            {
                grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));

                var kLbl = new Label
                {
                    Text = rows[i].Item1,
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(100, 100, 120),
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft
                };

                var vLbl = new Label
                {
                    Text = rows[i].Item2,
                    Font = new Font("Segoe UI", 9f),
                    ForeColor = Color.FromArgb(30, 30, 50),
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft
                };

                grid.Controls.Add(kLbl, 0, i);
                grid.Controls.Add(vLbl, 1, i);
            }
        }

        private void ApplyRounded(int r)
        {
            var path = new GraphicsPath();
            int d = r * 2;
            path.AddArc(0, 0, d, d, 180, 90);
            path.AddArc(Width - d, 0, d, d, 270, 90);
            path.AddArc(Width - d, Height - d, d, d, 0, 90);
            path.AddArc(0, Height - d, d, d, 90, 90);
            path.CloseFigure();
            Region = new Region(path);
        }

        // --- Event Handlers (Wired in Designer) ---

        private void BtnX_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        // Drag Support Events
        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            _drag = true;
            _dragStart = e.Location;
        }

        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_drag) return;
            var p = PointToScreen(e.Location);
            Location = new System.Drawing.Point(p.X - _dragStart.X, p.Y - _dragStart.Y);
        }

        private void TitleBar_MouseUp(object sender, MouseEventArgs e)
        {
            _drag = false;
        }

        // Custom Paint Events
        private void IconStrip_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.FromArgb(220, 220, 235)),
                0, iconStrip.Height - 1, iconStrip.Width, iconStrip.Height - 1);
        }

        private void Footer_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.FromArgb(220, 220, 235)),
                0, 0, footer.Width, 0);
        }

        private void IconCircle_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Color fill = _result.IsValid
                ? Color.FromArgb(220, 255, 235)
                : Color.FromArgb(255, 225, 225);

            using (var b = new SolidBrush(fill))
            {
                e.Graphics.FillEllipse(b, 0, 0, 47, 47);
            }

            string icon = _result.IsValid ? "✓" : "!";
            Color iColor = _result.IsValid
                ? Color.FromArgb(0, 130, 60)
                : Color.FromArgb(180, 30, 30);

            using (var ib = new SolidBrush(iColor))
            using (var f = new Font("Segoe UI", 16f, FontStyle.Bold))
            {
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                e.Graphics.DrawString(icon, f, ib, new RectangleF(0, 0, 47, 47), sf);
            }
        }
    }
}