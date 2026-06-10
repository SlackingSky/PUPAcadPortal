using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    /// <summary>
    /// Application-level mouse-wheel message filter.
    /// When <see cref="IsEnabled"/> is true and the wheel fires over the
    /// watched <see cref="_watchControl"/>, the supplied delta callback is
    /// invoked (positive = scroll up / prev, negative = scroll down / next).
    ///
    /// Controls added via <see cref="AddExclusion"/> are skipped so that
    /// scrollable panels (FlowLayoutPanel, etc.) keep their native scroll.
    /// </summary>
    public class CalendarWheelFilter : IMessageFilter
    {
        private const int WM_MOUSEWHEEL = 0x020A;

        private readonly Control _watchControl;
        private readonly Action<int> _onDelta;
        private readonly List<Control> _exclusions = new();

        /// <summary>
        /// When false the filter passes all messages through untouched.
        /// Flip to false in Weekly/Daily view so the hour-grid can scroll.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <param name="watchControl">
        ///   The top-level container to watch (e.g. pnlCalendar or pnlViewBody).
        /// </param>
        /// <param name="onDelta">
        ///   Called with +120 for scroll-up and -120 for scroll-down.
        /// </param>
        public CalendarWheelFilter(Control watchControl, Action<int> onDelta)
        {
            _watchControl = watchControl;
            _onDelta = onDelta;
        }

        /// <summary>
        /// Register a child control that should receive native wheel scroll
        /// (e.g. a FlowLayoutPanel with AutoScroll = true).
        /// </summary>
        public void AddExclusion(Control control)
        {
            if (control != null && !_exclusions.Contains(control))
                _exclusions.Add(control);
        }

        /// <summary>Remove a previously registered exclusion.</summary>
        public void RemoveExclusion(Control control) =>
            _exclusions.Remove(control);

        public bool PreFilterMessage(ref Message m)
        {
            if (!IsEnabled || m.Msg != WM_MOUSEWHEEL)
                return false;

            // ============================================================
            // FIX: SAFETY CHECK ADDED HERE
            // If the watchControl has been closed/disposed, ignore the message
            // otherwise it will throw "ObjectDisposedException"
            // ============================================================
            if (_watchControl == null || _watchControl.IsDisposed)
            {
                return false;
            }

            // Identify which window the cursor is over
            var cursor = Cursor.Position;
            var hitCtrl = FindControlAtPoint(_watchControl, _watchControl.PointToClient(cursor));

            if (hitCtrl == null)
                return false;   // not inside our watch-control

            // Let excluded / scrollable controls handle it themselves
            if (IsExcluded(hitCtrl))
                return false;

            // Extract the signed wheel delta from WPARAM high word
            int delta = (short)((m.WParam.ToInt64() >> 16) & 0xFFFF);
            _onDelta(delta);
            return true;        // consumed
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private bool IsExcluded(Control? c)
        {
            while (c != null)
            {
                if (_exclusions.Contains(c)) return true;
                c = c.Parent;
            }
            return false;
        }

        private static Control? FindControlAtPoint(Control root, System.Drawing.Point pt)
        {
            // Added extra safety check here as well
            if (root == null || root.IsDisposed || !root.ClientRectangle.Contains(pt))
                return null;

            foreach (Control child in root.Controls)
            {
                if (child == null || child.IsDisposed || !child.Visible) continue;
                var childPt = new System.Drawing.Point(pt.X - child.Left, pt.Y - child.Top);
                var hit = FindControlAtPoint(child, childPt);
                if (hit != null) return hit;
            }
            return root;
        }
    }
}
