using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal.Utils
{
    /// <summary>
    /// Global message filter that intercepts WM_MOUSEWHEEL when the cursor
    /// is inside a target control (or any of its children), then fires the
    /// provided callback with the scroll delta.
    ///
    /// Set <see cref="IsEnabled"/> to false (e.g. while a search/notification
    /// overlay is open) to let the wheel scroll those panels normally instead
    /// of navigating months/weeks/days.
    ///
    /// Use <see cref="AddExclusion"/> to register controls whose area should
    /// NOT trigger navigation (e.g. the bottom detail strip, upcoming list).
    /// </summary>
    public class CalendarWheelFilter : IMessageFilter
    {
        private const int WM_MOUSEWHEEL = 0x020A;

        private readonly Control _target;
        private readonly Action<int> _onScroll;
        private readonly List<Control> _exclusions = new();

        /// <summary>
        /// When false the filter passes every wheel message through untouched.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        public CalendarWheelFilter(Control target, Action<int> onScroll)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _onScroll = onScroll ?? throw new ArgumentNullException(nameof(onScroll));
        }

        /// <summary>
        /// Register a control whose screen area should be excluded from
        /// wheel-navigation interception (wheel events there pass through).
        /// </summary>
        public void AddExclusion(Control c) => _exclusions.Add(c);

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg != WM_MOUSEWHEEL)
                return false;

            if (!IsEnabled)
                return false;

            var cursor = Control.MousePosition;

            // Don't intercept when cursor is over an excluded control
            foreach (var ex in _exclusions)
                if (IsOverControl(ex, cursor))
                    return false;

            // Only intercept when cursor is inside the target control tree
            if (!IsOverControl(_target, cursor))
                return false;

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