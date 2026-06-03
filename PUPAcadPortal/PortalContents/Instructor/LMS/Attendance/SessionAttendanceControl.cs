using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace PUPAcadPortal.PortalContents.Instructor.LMS
{
    public partial class SessionAttendanceControl : UserControl
    {
        private int _present, _late, _absent, _excused;

        public SessionAttendanceControl()
        {
            InitializeComponent();
        }

        public void SetData(int present, int late, int absent, int excused)
        {
            _present = present;
            _late = late;
            _absent = absent;
            _excused = excused;
            int total = present + late + absent + excused;

            _donut.PresentPct = total > 0 ? present / (float)total : 0;
            _donut.LatePct = total > 0 ? late / (float)total : 0;
            _donut.AbsentPct = total > 0 ? absent / (float)total : 0;
            _donut.ExcusedPct = total > 0 ? excused / (float)total : 0;
            _donut.CenterText = total > 0 ? $"{(int)(_donut.PresentPct * 100)}%" : "–";

            _lblPresent.Text = present.ToString();
            _lblLate.Text = late.ToString();
            _lblAbsent.Text = absent.ToString();
            _lblExcused.Text = excused.ToString();

            _lblPPct.Text = total > 0 ? $"{present * 100.0 / total:F1}%" : "–";
            _lblLPct.Text = total > 0 ? $"{late * 100.0 / total:F1}%" : "–";
            _lblAPct.Text = total > 0 ? $"{absent * 100.0 / total:F1}%" : "–";
            _lblEPct.Text = total > 0 ? $"{excused * 100.0 / total:F1}%" : "–";
        }

        public void SetData(int present, int absent, int excused)
            => SetData(present, 0, absent, excused);
    }

    public class DonutPanel : Panel
    {
        private float _presentPct = 1f;
        private float _latePct = 0f;
        private float _absentPct = 0f;
        private float _excusedPct = 0f;
        private string _centerText = "–";

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float PresentPct { get => _presentPct; set { _presentPct = value; Invalidate(); } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float LatePct { get => _latePct; set { _latePct = value; Invalidate(); } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float AbsentPct { get => _absentPct; set { _absentPct = value; Invalidate(); } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float ExcusedPct { get => _excusedPct; set { _excusedPct = value; Invalidate(); } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CenterText { get => _centerText; set { _centerText = value; Invalidate(); } }

        private static readonly Color PresentClr = Color.FromArgb(34, 139, 34);
        private static readonly Color LateClr = Color.FromArgb(200, 110, 0);
        private static readonly Color AbsentClr = Color.FromArgb(210, 40, 40);
        private static readonly Color ExcusedClr = Color.FromArgb(180, 140, 0);

        public DonutPanel()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            BackColor = Color.White;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int m = 5;
            var rect = new Rectangle(m, m, Width - m * 2 - 1, Height - m * 2 - 1);
            int thk = (int)(rect.Width * 0.20f);

            using var bg = new Pen(Color.FromArgb(235, 235, 235), thk);
            g.DrawEllipse(bg, rect);

            float a = -90f;
            Arc(g, rect, thk, a, _presentPct * 360f, PresentClr); a += _presentPct * 360f;
            Arc(g, rect, thk, a, _latePct * 360f, LateClr); a += _latePct * 360f;
            Arc(g, rect, thk, a, _absentPct * 360f, AbsentClr); a += _absentPct * 360f;
            Arc(g, rect, thk, a, _excusedPct * 360f, ExcusedClr);

            using var fnt = new Font("Segoe UI", 9f, FontStyle.Bold);
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString(_centerText, fnt, Brushes.Black, new RectangleF(0, 0, Width, Height), sf);
        }

        private static void Arc(Graphics g, Rectangle r, int thick, float start, float sweep, Color col)
        {
            if (sweep <= 0) return;
            using var p = new Pen(col, thick) { StartCap = LineCap.Round, EndCap = LineCap.Round };
            g.DrawArc(p, r, start, sweep);
        }
    }
}