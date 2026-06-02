using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms; // Added to resolve the Control type

namespace PUPAcadPortal.Utils
{
    public static class EventUnbinder
    {
        /// <summary>
        /// Recursively unbinds internal WinForms events from a control tree to assist the Garbage Collector.
        /// </summary>
        public static void ClearAllEvents(Control control)
        {
            if (control == null || control.IsDisposed) return;
            for (int i = control.Controls.Count - 1; i >= 0; i--)
            {
                Control child = control.Controls[i];
                ClearAllEvents(child);
            }

            PropertyInfo? eventsProperty = typeof(Component).GetProperty("Events",
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (eventsProperty != null)
            {
                EventHandlerList? eventHandlerList = (EventHandlerList?)eventsProperty.GetValue(control, null);

                if (eventHandlerList != null)
                {
                    FieldInfo? headField = typeof(EventHandlerList).GetField("head",
                        BindingFlags.NonPublic | BindingFlags.Instance);

                    if (headField != null)
                    {
                        headField.SetValue(eventHandlerList, null);
                    }
                }
            }
        }
    }
}
