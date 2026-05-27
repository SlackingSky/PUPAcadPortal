using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class SessionAttendanceControl : UserControl
    {
        //  Colors 
        private static readonly Color PresentColor = Color.FromArgb(52, 168, 83);
        private static readonly Color AbsentColor = Color.FromArgb(220, 53, 69);
        private static readonly Color ExcusedColor = Color.FromArgb(255, 193, 7);
        private static readonly Color TrackColor = Color.FromArgb(230, 230, 230);
        //  Data 
        private int _present = 0;
        private int _absent = 0;
        private int _excused = 0;
        private int Total => _present + _absent + _excused;
        //  Child controls 
        private DonutCircle _donut;
        private Label _lblTitle;
        private Label _lblPresent;
        private Label _lblAbsent;
        private Label _lblExcused;
        private Label _lblTotal;

        public SessionAttendanceControl()
        {
            DoubleBuffered = true;
            BackColor = Color.White;
            Padding = new Padding(10, 6, 14, 6);
            MinimumSize = new Size(240, 100);

            BuildLayout();
        }

        //  Public API 
        public void SetData(int present, int absent, int excused)
        {
            _present = present;
            _absent = absent;
            _excused = excused;

            int total = Total;
            _donut.PresentPct = total > 0 ? (float)present / total : 0f;
            _donut.AbsentPct = total > 0 ? (float)absent / total : 0f;
            _donut.ExcusedPct = total > 0 ? (float)excused / total : 0f;
            _donut.CenterText = total > 0 ? $"{present * 100 / total}%" : "0%";
            _donut.Invalidate();

            _lblPresent.Text = $"Present : {present}";
            _lblAbsent.Text = $"Absent  : {absent}";
            _lblExcused.Text = $"Excused : {excused}";
            _lblTotal.Text = $"Total   : {total}";
        }

        //  Layout 
        private void BuildLayout()
        {
            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
            };

            // CHANGE 1: donut column n
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90f));  // donut
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));  // legend
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            //  Donut 
            _donut = new DonutCircle
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = new Padding(6, 6, 4, 6),
            };
            tbl.Controls.Add(_donut, 0, 0);

            //  Legend panel 
            var legend = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(10, 6, 4, 4),
            };

            _lblTitle = MakeLabel("Session Attendance",
                new Font("Segoe UI", 9f, FontStyle.Bold), Color.Black);

            _lblPresent = MakeLegendLabel("Present : 0", PresentColor);
            _lblAbsent = MakeLegendLabel("Absent  : 0", AbsentColor);
            _lblExcused = MakeLegendLabel("Excused : 0", ExcusedColor);
            _lblTotal = MakeLabel("Total   : 0",
                new Font("Segoe UI", 9f), Color.Gray);

            var flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = false,
                BackColor = Color.Transparent,
                Padding = Padding.Empty,
                Margin = Padding.Empty,
            };

            flow.Controls.Add(_lblTitle);
            flow.Controls.Add(_lblPresent);
            flow.Controls.Add(_lblAbsent);
            flow.Controls.Add(_lblExcused);
            flow.Controls.Add(_lblTotal);

            legend.Controls.Add(flow);
            tbl.Controls.Add(legend, 1, 0);

            Controls.Add(tbl);
        }

        //  Helpers 
        private static Label MakeLabel(string text, Font font, Color fore)
            => new Label
            {
                Text = text,
                Font = font,
                ForeColor = fore,
                AutoSize = true,
                Margin = new Padding(0, 3, 0, 3),
                BackColor = Color.Transparent,
            };

        private static Label MakeLegendLabel(string text, Color dotColor)
        {
            var lbl = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9f),
                ForeColor = dotColor,
                AutoSize = true,
                Margin = new Padding(0, 4, 0, 0),
                BackColor = Color.Transparent,
            };
            return lbl;
        }

        //  Inner class: actual donut drawing panel 
        private class DonutCircle : Panel
        {
            private float _presentPct = 0f;
            private float _absentPct = 0f;
            private float _excusedPct = 0f;
            private string _centerText = "0%";

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public float PresentPct
            {
                get => _presentPct;
                set { _presentPct = value; Invalidate(); }
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public float AbsentPct
            {
                get => _absentPct;
                set { _absentPct = value; Invalidate(); }
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public float ExcusedPct
            {
                get => _excusedPct;
                set { _excusedPct = value; Invalidate(); }
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public string CenterText
            {
                get => _centerText;
                set { _centerText = value; Invalidate(); }
            }

            public DonutCircle()
            {
                DoubleBuffered = true;
                ResizeRedraw = true;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                int margin = 6;
                int size = Math.Min(Width, Height) - margin * 2;
                if (size < 10) return;

                int rx = (Width - size) / 2;
                int ry = (Height - size) / 2;
                var rect = new Rectangle(rx, ry, size, size);

                int thick = Math.Max(6, (int)(size * 0.18f));

                using (var bgPen = new Pen(TrackColor, thick))
                    g.DrawEllipse(bgPen, rect);

                float startAngle = -90f;
                DrawArc(g, rect, thick, ref startAngle, _presentPct * 360f, PresentColor);
                DrawArc(g, rect, thick, ref startAngle, _absentPct * 360f, AbsentColor);
                DrawArc(g, rect, thick, ref startAngle, _excusedPct * 360f, ExcusedColor);

                using (var font = new Font("Segoe UI", size * 0.15f, FontStyle.Bold))
                {
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center,
                    };
                    g.DrawString(_centerText, font, Brushes.Black,
                        new RectangleF(0, 0, Width, Height), sf);
                }
            }

            private static void DrawArc(Graphics g, Rectangle rect, int thickness,
                ref float startAngle, float sweepAngle, Color color)
            {
                if (sweepAngle <= 0f) return;
                using var pen = new Pen(color, thickness)
                {
                    StartCap = LineCap.Round,
                    EndCap = LineCap.Round,
                };
                g.DrawArc(pen, rect, startAngle, sweepAngle);
                startAngle += sweepAngle;
            }
        }
    }
}