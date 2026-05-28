using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace PUPAcadPortal.Utils
{
    public static class EventUnbinder
    {
        public static void ClearAllEvents(Control control)
        {
            if (control == null) return;

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

            foreach (Control child in control.Controls)
            {
                ClearAllEvents(child);
            }
        }
    }
}
