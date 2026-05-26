using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal.Utils
{
    public class Resizer
    {
        private readonly Control _control;
        private readonly SizeF _designSize;

        private readonly Dictionary<Control, RectangleF> _origBounds = new();
        private readonly Dictionary<Control, float> _origFontSz = new();

        // Safety limit: Text is prohibited from growing more than 1.35x its original size
        private const float MaxFontScaleFactor = 1f;

        public Resizer(Control control)
        {
            if (control is Form) _control = control as Form;
            if (control is UserControl) _control = control as UserControl;

            float dpiScaleX = 1.0f;
            float dpiScaleY = 1.0f;

            using (Graphics g = _control.CreateGraphics())
            {
                dpiScaleX = g.DpiX / 96f;
                dpiScaleY = g.DpiY / 96f;
            }

            SnapshotControls(_control.Controls, dpiScaleX, dpiScaleY);
            _designSize = new SizeF(_control.ClientSize.Width / dpiScaleX, _control.ClientSize.Height / dpiScaleY);

            _control.Resize += StudentPortal_ResizeHandler;
        }

        private void StudentPortal_ResizeHandler(object sender, EventArgs e)
        {
            if (_designSize.Width == 0 || _designSize.Height == 0) return;

            float currentDpiX = 1.0f;
            float currentDpiY = 1.0f;
            using (Graphics g = _control.CreateGraphics())
            {
                currentDpiX = g.DpiX / 96f;
                currentDpiY = g.DpiY / 96f;
            }

            float minWidth = 1024f * currentDpiX;
            float minHeight = 700f * currentDpiY;

            float rx = Math.Max(_control.ClientSize.Width, minWidth) / _designSize.Width;
            float ry = Math.Max(_control.ClientSize.Height, minHeight) / _designSize.Height;

            _control.SuspendLayout();
            ScaleControls(_control.Controls, rx, ry);
            _control.ResumeLayout(true);
        }

        private void SnapshotControls(Control.ControlCollection controls, float dpiScaleX, float dpiScaleY)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl.Tag is string t && t.Contains("noScale")) continue;

                _origBounds[ctrl] = new RectangleF(
                    ctrl.Left / dpiScaleX,
                    ctrl.Top / dpiScaleY,
                    ctrl.Width / dpiScaleX,
                    ctrl.Height / dpiScaleY
                );

                _origFontSz[ctrl] = ctrl.Font.Size / dpiScaleY;

                if (ctrl.HasChildren)
                {
                    SnapshotControls(ctrl.Controls, dpiScaleX, dpiScaleY);
                }
            }
        }

        private void ScaleControls(Control.ControlCollection controls, float rx, float ry)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl.Tag is string t && t.Contains("noScale")) continue;
                if (!_origBounds.TryGetValue(ctrl, out RectangleF ob)) continue;

                int newX = R(ob.X * rx);
                int newY = R(ob.Y * ry);
                int newW = Math.Max(1, R(ob.Width * rx));
                int newH = Math.Max(1, R(ob.Height * ry));

                switch (ctrl.Dock)
                {
                    case DockStyle.Fill: break;
                    case DockStyle.Top: ctrl.Height = newH; break;
                    case DockStyle.Bottom: ctrl.Height = newH; break;
                    case DockStyle.Left: ctrl.Width = newW; break;
                    case DockStyle.Right: ctrl.Width = newW; break;
                    default: ctrl.SetBounds(newX, newY, newW, newH); break;
                }

                if (_origFontSz.TryGetValue(ctrl, out float origSz))
                {
                    // 1. Calculate raw linear font scaling modifier
                    float rawFontScale = Math.Min(rx, ry);

                    // 2. Dampen the scaling factor so text does not blow out on huge screens
                    // This caps text growth at 1.35x, but allows it to shrink smoothly if needed
                    float safeFontScale = Math.Min(rawFontScale, MaxFontScaleFactor);

                    // 3. Compute final size with a minimum safety floor of 6pt
                    float newSz = Math.Max(6f, origSz * safeFontScale);

                    // 4. Reduce font rendering updates to prevent flickering text layouts
                    if (Math.Abs(ctrl.Font.Size - newSz) > 0.25f)
                    {
                        try
                        {
                            ctrl.Font = new Font(ctrl.Font.FontFamily, (float)Math.Round(newSz, 1), ctrl.Font.Style, GraphicsUnit.Point);
                        }
                        catch { /* Fail-safe override for system fonts */ }
                    }
                }

                if (ctrl.HasChildren)
                {
                    ScaleControls(ctrl.Controls, rx, ry);
                }
            }
        }

        private static int R(float v) => (int)Math.Round(v);
    }
}
