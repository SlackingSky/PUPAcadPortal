using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Events
{
    internal class QuickActionClickEvent
    {
        public static event EventHandler? QuickActionClicked;

        public static void OnClick(object? sender, EventArgs e)
        {
            QuickActionClicked?.Invoke(sender, e);
        }
    }
}
