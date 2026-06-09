using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PUPAcadPortal.Services;

namespace PUPAcadPortal.PortalContents.Student.LMS.Attendance
{

    public partial class QRConfirmDialog : Form
    {
        // ── Colours ───────────────────────────────────────────────────────────────
        private static readonly Color SuccessGreen = Color.FromArgb(0, 150, 70);
        private static readonly Color ErrorRed = Color.FromArgb(200, 40, 40);
        private static readonly Color BorderGray = Color.FromArgb(220, 220, 235);
        private static readonly Color SubGray = Color.FromArgb(100, 100, 120);

        // ── State ─────────────────────────────────────────────────────────────────
        private readonly QrScanOutcome? _outcome;   // service result (preferred)
        private readonly QRScanResult? _legacy;    // fallback for old callers

        private bool _drag;
        private System.Drawing.Point _dragStart;

        // ── Constructor: service outcome (new path) ───────────────────────────────
        public QRConfirmDialog(QrScanOutcome outcome)
        {
            InitializeComponent();
            _outcome = outcome;

            WireCommon();
            PopulateFromOutcome(outcome);
        }

        // ── Constructor: legacy QRScanResult (backward-compat) ───────────────────
        public QRConfirmDialog(QRScanResult result)
        {
            InitializeComponent();
            _legacy = result;

            WireCommon();
            PopulateFromLegacy(result);
        }

        // ── Shared wiring ─────────────────────────────────────────────────────────
        private void WireCommon()
        {
            this.Resize += (s, e) => ApplyRounded(12);
            this.Shown += (s, e) => ApplyRounded(12);
        }

        // ── Populate from QrScanOutcome ───────────────────────────────────────────
        private void PopulateFromOutcome(QrScanOutcome outcome)
        {
            bool ok = outcome.Success;

            // Icon strip background
            iconStrip.BackColor = ok
                ? Color.FromArgb(242, 255, 248)
                : Color.FromArgb(255, 243, 243);

            // Status header
            lblStatus.Text = ok
                ? "Attendance Successfully Recorded"
                : "Attendance Not Recorded";
            lblStatus.ForeColor = ok ? Color.FromArgb(0, 110, 50) : Color.FromArgb(160, 30, 30);

            // Sub-text
            lblSub.Text = ok
                ? "Your attendance has been saved and verified. This record cannot be edited."
                : outcome.Message;

            if (ok)
            {
                // Full success record detail
                var rows = new (string Key, string Value)[]
                {
                    ("Student",      outcome.StudentName),
                    ("Course Code",  outcome.CourseCode),
                    ("Subject",      outcome.SubjectName),
                    ("Section",      outcome.Section),
                    ("Date",         outcome.SessionDate),
                    ("Time In",      outcome.TimeIn),
                    ("Status",       "Present  (QR Verified)"),
                    ("Record ID",    $"#{outcome.AttendanceId}"),
                };
                BuildDetailGrid(rows);

                // Change confirm button to "Done" — record is already persisted
                if (btnConfirm != null)
                {
                    btnConfirm.Text = "Done";
                    btnConfirm.BackColor = Color.FromArgb(0, 130, 60);
                    btnConfirm.ForeColor = Color.White;
                }
                if (btnCancel != null)
                    btnCancel.Visible = false;
            }
            else
            {
                // Failure: just show the error message — no detail table
                var rows = new (string Key, string Value)[] { ("Reason", outcome.Message) };
                BuildDetailGrid(rows);

                if (btnConfirm != null) btnConfirm.Visible = false;
                if (btnCancel != null) btnCancel.Text = "Close";
            }
        }

        // ── Populate from legacy QRScanResult ─────────────────────────────────────
        private void PopulateFromLegacy(QRScanResult result)
        {
            iconStrip.BackColor = result.IsValid
                ? Color.FromArgb(242, 255, 248)
                : Color.FromArgb(255, 243, 243);

            lblStatus.Text = result.IsValid
                ? "QR Code Decoded Successfully"
                : "QR Code Format Not Recognised";
            lblStatus.ForeColor = result.IsValid
                ? Color.FromArgb(0, 110, 50)
                : Color.FromArgb(160, 30, 30);

            lblSub.Text = result.IsValid
                ? "Review the details below and confirm to record your attendance."
                : "The QR code was decoded but the content format is unrecognised.";

            var rows = new (string Key, string Value)[]
            {
                ("Course Code", result.CourseCode ?? "–"),
                ("Course Name", result.CourseName ?? "–"),
                ("Period",      result.Period     ?? "–"),
                ("Session",     result.Session    ?? "–"),
                ("Scan Time",   result.ScanTime.ToString("MMMM d, yyyy  HH:mm:ss")),
            };
            BuildDetailGrid(rows);

            if (btnConfirm != null)
                btnConfirm.Visible = result.IsValid;
        }

        // ── Build the detail TableLayoutPanel rows ────────────────────────────────
        private void BuildDetailGrid((string Key, string Value)[] rows)
        {
            if (grid == null) return;
            grid.Controls.Clear();
            grid.RowCount = rows.Length;

            for (int i = 0; i < rows.Length; i++)
            {
                grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));

                var kLbl = new Label
                {
                    Text = rows[i].Key,
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                    ForeColor = SubGray,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft,
                };

                var vLbl = new Label
                {
                    Text = rows[i].Value,
                    Font = new Font("Segoe UI", 9f),
                    ForeColor = Color.FromArgb(30, 30, 50),
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft,
                };

                // Highlight the status row in green if it contains the QR badge
                if (rows[i].Value.Contains("QR Verified"))
                {
                    vLbl.ForeColor = Color.FromArgb(0, 120, 60);
                    vLbl.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                }

                grid.Controls.Add(kLbl, 0, i);
                grid.Controls.Add(vLbl, 1, i);
            }
        }

        // ── Rounded form ──────────────────────────────────────────────────────────
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

        // ── Button handlers ───────────────────────────────────────────────────────
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

        // ── Drag handlers ─────────────────────────────────────────────────────────
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
            => _drag = false;

        // ── Paint handlers ────────────────────────────────────────────────────────
        private void IconStrip_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(BorderGray),
                0, iconStrip.Height - 1, iconStrip.Width, iconStrip.Height - 1);
        }

        private void Footer_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(BorderGray), 0, 0, footer.Width, 0);
        }

        private void IconCircle_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            bool ok = _outcome?.Success ?? (_legacy?.IsValid ?? false);
            Color fill = ok ? Color.FromArgb(220, 255, 235) : Color.FromArgb(255, 225, 225);

            using (var b = new SolidBrush(fill))
                e.Graphics.FillEllipse(b, 0, 0, 47, 47);

            string icon = ok ? "✓" : "!";
            Color iColor = ok ? Color.FromArgb(0, 130, 60) : Color.FromArgb(180, 30, 30);

            using (var ib = new SolidBrush(iColor))
            using (var f = new Font("Segoe UI", 16f, FontStyle.Bold))
            {
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                };
                e.Graphics.DrawString(icon, f, ib, new RectangleF(0, 0, 47, 47), sf);
            }
        }
    }
}