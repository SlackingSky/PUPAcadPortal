using System;
using System.Windows.Forms;

namespace PUPAcadPortal.Utils
{
    public class CalendarWheelFilter : IMessageFilter
    {
        private const int WM_MOUSEWHEEL = 0x020A;
        private const int WHEEL_DELTA = 120;   

        private readonly Control _target;
        private readonly Action<int> _onScroll;
        private int _accumulated = 0;

        public CalendarWheelFilter(Control target, Action<int> onScroll)
        {
            if (target.IsDisposed) return;
            _target = target;
            _onScroll = onScroll;
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (_target.IsDisposed) return false;
            if (m.Msg != WM_MOUSEWHEEL) return false;

            var cursorPos = Control.MousePosition;
            var targetRect = new System.Drawing.Rectangle(
                _target.PointToScreen(System.Drawing.Point.Empty),
                _target.Size);

            if (!targetRect.Contains(cursorPos)) return false;

            int delta = (short)(((int)m.WParam >> 16) & 0xFFFF);
            _accumulated += delta;

            if (Math.Abs(_accumulated) >= WHEEL_DELTA)
            {
                int direction = _accumulated > 0 ? WHEEL_DELTA : -WHEEL_DELTA;
                _accumulated = 0;
                _onScroll?.Invoke(direction);
            }

            return true;   
        }
    }
}