using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace PUPAcadPortal
{
    public class buttonRounded : Button
    {
        private int _borderRadius = 10;
        private bool _isHovered = false;
        private bool _isPressed = false;

        public buttonRounded()
        {
            SetStyle(
                ControlStyles.UserPaint |   // we handle OnPaint
                ControlStyles.AllPaintingInWmPaint |   // skip WM_ERASEBKGND
                ControlStyles.OptimizedDoubleBuffer |  // no flicker
                ControlStyles.ResizeRedraw |   // repaint on resize
                ControlStyles.SupportsTransparentBackColor,
                true);

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            FlatAppearance.MouseOverBackColor = Color.Transparent;
            FlatAppearance.MouseDownBackColor = Color.Transparent;
            Cursor = Cursors.Hand;
            UseCompatibleTextRendering = false;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int BorderRadius
        {
            get => _borderRadius;
            set
            {
                _borderRadius = Math.Max(0, value);
                UpdateRegion();
                Invalidate();
            }
        }

        private void UpdateRegion()
        {
            if (Width <= 0 || Height <= 0) return;

            var path = BuildPath(new Rectangle(0, 0, Width, Height), _borderRadius);
            Region = new Region(path);
            path.Dispose();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isHovered = false;
            _isPressed = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left) { _isPressed = true; Invalidate(); }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _isPressed = false;
            Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateRegion();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateRegion();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // ── 1. High-quality rendering ──
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            Rectangle rect = new Rectangle(0, 0, Width, Height);

            // ── 2. Choose background colour by state ──
            Color bg;
            if (!Enabled)
            {
                // Disabled: desaturate + darken
                bg = Desaturate(BackColor, 0.4f);
            }
            else if (_isPressed)
            {
                bg = Darken(BackColor, 0.18f);
            }
            else if (_isHovered)
            {
                bg = Lighten(BackColor, 0.14f);
            }
            else
            {
                bg = BackColor;
            }

            // ── 3. Fill rounded background ──
            using (var path = BuildPath(rect, _borderRadius))
            using (var brush = new SolidBrush(bg))
            {
                e.Graphics.FillPath(brush, path);
            }

            // ── 4. Optional subtle inner highlight (top edge) ──
            if (Enabled && !_isPressed && _borderRadius > 0)
            {
                var highlightRect = new Rectangle(1, 1, Width - 3, _borderRadius);
                using var highlightPath = BuildPath(highlightRect, Math.Max(0, _borderRadius - 1));
                using var highlightBrush = new LinearGradientBrush(
                    highlightRect,
                    Color.FromArgb(40, Color.White),
                    Color.Transparent,
                    LinearGradientMode.Vertical);
                e.Graphics.FillPath(highlightBrush, highlightPath);
            }

            // ── 5. Draw text (TextRenderer avoids GDI+ blurriness) ──
            Color textColor = Enabled ? ForeColor : Color.FromArgb(160, ForeColor);
            TextRenderer.DrawText(
                e.Graphics,
                Text,
                Font,
                rect,
                textColor,
                TextFormatFlags.HorizontalCenter |
                TextFormatFlags.VerticalCenter |
                TextFormatFlags.SingleLine |
                TextFormatFlags.EndEllipsis);
        }

        private static GraphicsPath BuildPath(Rectangle bounds, int radius)
        {
            var path = new GraphicsPath();

            if (radius <= 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            int r = Math.Min(radius, Math.Min(bounds.Width, bounds.Height) / 2);
            int d = r * 2;

            path.AddArc(bounds.Left, bounds.Top, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Top, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - d, d, d, 90, 90);

            path.CloseFigure();
            return path;
        }


        private static Color Lighten(Color c, float amount)
        {
            float a = Math.Clamp(amount, 0f, 1f);
            return Color.FromArgb(
                c.A,
                (int)Math.Min(255, c.R + (255 - c.R) * a),
                (int)Math.Min(255, c.G + (255 - c.G) * a),
                (int)Math.Min(255, c.B + (255 - c.B) * a));
        }

        private static Color Darken(Color c, float amount)
        {
            float a = Math.Clamp(amount, 0f, 1f);
            return Color.FromArgb(
                c.A,
                (int)(c.R * (1f - a)),
                (int)(c.G * (1f - a)),
                (int)(c.B * (1f - a)));
        }

        private static Color Desaturate(Color c, float amount)
        {
            float a = Math.Clamp(amount, 0f, 1f);
            float lum = c.R * 0.299f + c.G * 0.587f + c.B * 0.114f;
            return Color.FromArgb(
                c.A,
                (int)(c.R + (lum - c.R) * a),
                (int)(c.G + (lum - c.G) * a),
                (int)(c.B + (lum - c.B) * a));
        }
    }
}