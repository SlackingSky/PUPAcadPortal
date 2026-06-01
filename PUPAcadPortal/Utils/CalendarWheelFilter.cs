using System;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal.Utils
{
    public class CalendarWheelFilter : IMessageFilter
    {
        private const int WM_MOUSEWHEEL = 0x020A;

        private readonly Control _target;
        private readonly Action<int> _onScroll;   // positive = scroll up/prev, negative = next

        /// <summary>
        /// When false the filter passes every wheel message through untouched.
        /// Toggle this off whenever a search/notification panel is visible.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        public CalendarWheelFilter(Control target, Action<int> onScroll)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _onScroll = onScroll ?? throw new ArgumentNullException(nameof(onScroll));
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg != WM_MOUSEWHEEL)
                return false;

            // Disabled – let the message pass through (scrolls search/notif panels)
            if (!IsEnabled)
                return false;

            // Only intercept when cursor is inside the target control tree
            var cursor = Control.MousePosition;
            if (!IsOverControl(_target, cursor))
                return false;

            // High word of wParam = delta (positive = forward/up)
            int delta = (short)((long)m.WParam.ToInt64() >> 16);
            _onScroll(delta);
            return true;   // swallow
        }

        private static bool IsOverControl(Control root, Point screenPt)
        {
            if (!root.IsHandleCreated || !root.Visible) return false;
            var clientPt = root.PointToClient(screenPt);
            return root.ClientRectangle.Contains(clientPt);
        }
    }
}