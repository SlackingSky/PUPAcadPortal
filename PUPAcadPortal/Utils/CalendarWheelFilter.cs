using System;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal.Utils
{
    /// <summary>
    /// Global message filter that intercepts WM_MOUSEWHEEL when the cursor
    /// is inside a target control (or any of its children), then fires the
    /// provided callback with the scroll delta.
    /// Used by CalendarContentInst to scroll months/weeks/days.
    /// </summary>
    public class CalendarWheelFilter : IMessageFilter
    {
        private const int WM_MOUSEWHEEL = 0x020A;

        private readonly Control _target;
        private readonly Action<int> _onScroll;   // positive = scroll up/prev, negative = next

        public CalendarWheelFilter(Control target, Action<int> onScroll)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _onScroll = onScroll ?? throw new ArgumentNullException(nameof(onScroll));
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg != WM_MOUSEWHEEL)
                return false;

            // Only intercept when cursor is inside the target control tree
            var cursor = Control.MousePosition;
            if (!IsOverControl(_target, cursor))
                return false;

            // High word of wParam = delta (positive = forward/up)
            // Use ToInt64() to avoid OverflowException on .NET 10 x64
            int delta = (short)((long)m.WParam.ToInt64() >> 16);
            _onScroll(delta);
            return true;   // swallow – prevents accidental scrolling of nested panels
        }

        private static bool IsOverControl(Control root, Point screenPt)
        {
            if (!root.IsHandleCreated || !root.Visible) return false;
            var clientPt = root.PointToClient(screenPt);
            return root.ClientRectangle.Contains(clientPt);
        }
    }
}