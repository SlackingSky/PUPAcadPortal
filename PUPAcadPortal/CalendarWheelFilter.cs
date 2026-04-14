using System;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    /// <summary>
    /// Message filter that intercepts mouse-wheel events over a target control
    /// and forwards them as a scroll delta to a callback.
    /// Fixes CS0246: CalendarWheelFilter not found.
    /// </summary>
    public class CalendarWheelFilter : IMessageFilter
    {
        private const int WM_MOUSEWHEEL = 0x020A;

        private readonly Control _target;
        private readonly Action<int> _onScroll; 

        public CalendarWheelFilter(Control target, Action<int> onScroll)
        {
            _target = target;
            _onScroll = onScroll;
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg != WM_MOUSEWHEEL) return false;

            var cursorPos = Control.MousePosition;
            var targetRect = new System.Drawing.Rectangle(
                _target.PointToScreen(System.Drawing.Point.Empty),
                _target.Size);

            if (!targetRect.Contains(cursorPos)) return false;

            int delta = (short)(((int)m.WParam >> 16) & 0xFFFF);
            _onScroll?.Invoke(delta);

            return true; 
        }
    }
}