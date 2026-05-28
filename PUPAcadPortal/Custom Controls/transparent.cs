using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public class TransparentPanel : Panel
    {
        private int _opacity = 127;

        // Attributes to fix WFO1000 and ensure it shows in the Properties window
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Sets the transparency level from 0 (invisible) to 255 (solid).")]
        public int Opacity
        {
            get => _opacity;
            set
            {
                // Constrain value between 0 and 255 to prevent crashes
                _opacity = Math.Max(0, Math.Min(255, value));
                this.Invalidate(); // Forces the panel to redraw when you change the value
            }
        }

        public TransparentPanel()
        {
            // Reduces flicker during scrolling and resizing
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint, true);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // 0x00000020 is WS_EX_TRANSPARENT: 
                // It tells Windows to paint the controls behind this panel first.
                cp.ExStyle |= 0x00000020;
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Using 'using' ensures the brush is cleaned up from memory immediately
            using (var brush = new SolidBrush(Color.FromArgb(_opacity, this.BackColor)))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
            base.OnPaint(e);
        }
    }
}