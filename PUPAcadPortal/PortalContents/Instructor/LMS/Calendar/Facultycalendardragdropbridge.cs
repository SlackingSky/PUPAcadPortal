using System;
using System.Windows.Forms;
using PUPAcadPortal.Data;

namespace PUPAcadPortal
{
    /// <summary>
    /// Subscribes to <see cref="UrDay.EventDropped"/> (static event) once the
    /// CalendarContentInst is loaded, and forwards moves to FacultyCalendarData.
    /// 
    /// Call <see cref="Attach"/> from CalendarContentInst_Load and
    /// <see cref="Detach"/> from Dispose.
    /// </summary>
    public static class FacultyCalendarDragDropBridge
    {
        private static Action<Guid, DateTime>? _handler;
        private static Action? _refreshCallback;

        /// <summary>
        /// Wire up the drag-drop bridge.
        /// </summary>
        /// <param name="refreshAll">
        ///   Called after a successful move to refresh all calendar views.
        /// </param>
        public static void Attach(Action refreshAll)
        {
            Detach(); // ensure no double-subscription

            _refreshCallback = refreshAll;

            _handler = (id, newDate) =>
            {
                FacultyCalendarData.MoveEvent(id, newDate);
                refreshAll();
            };

            UrDay.EventDropped += _handler;
        }

        public static void Detach()
        {
            if (_handler != null)
            {
                UrDay.EventDropped -= _handler;
                _handler = null;
                _refreshCallback = null;
            }
        }
    }
}