using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace PUPAcadPortal.Utils
{
    public class Resizer
    {
        private UserControl _userControl;

        private SizeF _designSize;

        private readonly Dictionary<Control, RectangleF> _origBounds = new();
        private readonly Dictionary<Control, float> _origFontSz = new();

        public Resizer(UserControl userControl)
        {
            _userControl = userControl;
            _userControl.Resize += StudentPortal_ResizeHandler;
            SnapshotControls(_userControl.Controls);
            _designSize = userControl.ClientSize;
        }
        
        private void StudentPortal_ResizeHandler(object sender, EventArgs e)
        {
            if (_designSize.Width == 0 || _designSize.Height == 0) return;

            float rx = Math.Max(_userControl.ClientSize.Width, 1024) / _designSize.Width;
            float ry = Math.Max(_userControl.ClientSize.Height, 700) / _designSize.Height;

            _userControl.SuspendLayout();
            ScaleControls(_userControl.Controls, rx, ry);
            _userControl.ResumeLayout(true);
        }

        private void SnapshotControls(Control.ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl.Tag is string t && t.Contains("noScale")) continue;
                _origBounds[ctrl] = new RectangleF(ctrl.Left, ctrl.Top, ctrl.Width, ctrl.Height);
                _origFontSz[ctrl] = ctrl.Font.Size;
                if (ctrl.HasChildren) SnapshotControls(ctrl.Controls);
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
                    float newSz = Math.Max(6f, origSz * Math.Min(rx, ry));
                    if (Math.Abs(ctrl.Font.Size - newSz) > 0.15f)
                    {
                        try { ctrl.Font = new Font(ctrl.Font.FontFamily, newSz, ctrl.Font.Style, GraphicsUnit.Point); }
                        catch { }
                    }
                }

                if (ctrl.HasChildren) ScaleControls(ctrl.Controls, rx, ry);
            }
        }

        private static int R(float v) => (int)Math.Round(v);
    }
}
