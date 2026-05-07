using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace PUPAcadPortal
{
    /// <summary>
    /// A Button subclass with a configurable corner radius for rounded appearance.
    /// Used throughout PUPAcadPortal for consistent UI styling.
    /// </summary>
    public class buttonRounded : Button
    {
        private int _borderRadius = 10;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")]
        [Description("Radius of the button corners. 0 = square, higher = rounder.")]
        public int BorderRadius
        {
            get => _borderRadius;
            set
            {
                _borderRadius = value;
                Invalidate();
            }
        }

        public buttonRounded()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = Color.Maroon;
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            Cursor = Cursors.Hand;
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Clip region to rounded rectangle
            using (GraphicsPath path = GetRoundedPath(ClientRectangle, _borderRadius))
            {
                Region = new Region(path);
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Fill background
            using (SolidBrush brush = new SolidBrush(BackColor))
            using (GraphicsPath path = GetRoundedPath(ClientRectangle, _borderRadius))
            {
                e.Graphics.FillPath(brush, path);
            }

            // Draw text
            TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);

            // Draw image if set
            if (Image != null)
            {
                int imgX = (ClientRectangle.Width - Image.Width) / 2;
                int imgY = (ClientRectangle.Height - Image.Height) / 2;
                e.Graphics.DrawImage(Image, imgX, imgY, Image.Width, Image.Height);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            // Slight hover darkening
            BackColor = ControlPaint.Dark(BackColor, 0.05f);
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            BackColor = ControlPaint.Light(BackColor, 0.05f);
            Invalidate();
        }

        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            int r = Math.Max(0, Math.Min(radius, Math.Min(rect.Width, rect.Height) / 2));
            float curve = r * 2F;
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curve, curve, 180, 90);
            path.AddArc(rect.Right - curve, rect.Y, curve, curve, 270, 90);
            path.AddArc(rect.Right - curve, rect.Bottom - curve, curve, curve, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curve, curve, curve, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}