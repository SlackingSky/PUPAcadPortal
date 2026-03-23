using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace PUPAcadPortal
{
    public class buttonRounded : Button
    {
        // Fields for customization
        private int borderRadius = 20;
        private Color hoverColor = Color.Maroon;
        private Color normalColor = Color.FromArgb(128, 0, 0); // PUP Maroon

        [Category("Appearance")]
        [Description("Sets the roundness of the button corners.")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderRadius
        {
            get => borderRadius;
            set
            {
                borderRadius = value;
                this.Invalidate(); // Redraws the button in the designer
            }
        }

        public buttonRounded()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.BackColor = normalColor;
            this.ForeColor = Color.White;
            this.Size = new Size(150, 40);
            this.Cursor = Cursors.Hand;
        }

        // Logic for smooth rounded edges
        private GraphicsPath GetFigurePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rectSurface = this.ClientRectangle;
            int smoothSize = 2;

            if (borderRadius > 2)
            {
                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
                {
                    // Set the button region to the rounded path
                    this.Region = new Region(pathSurface);

                    // Draw the surface border for high-quality anti-aliasing
                    using (Pen penSurface = new Pen(this.Parent.BackColor, smoothSize))
                    {
                        pevent.Graphics.DrawPath(penSurface, pathSurface);
                    }
                }
            }
            else
            {
                this.Region = new Region(rectSurface);
            }
        }

        // Hover Effect Logic
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.BackColor = Color.FromArgb(160, 0, 0); // Lighter maroon on hover
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.BackColor = normalColor;
        }
    }
}