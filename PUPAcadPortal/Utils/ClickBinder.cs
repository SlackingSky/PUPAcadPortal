using PUPAcadPortal.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Utils
{
    internal static class ClickBinder
    {
        public static void BindClick(this Control ctrl)
        {
            if (ctrl == null) return;
            InternalClickBinder(ctrl, ctrl);
        }

        private static void InternalClickBinder(Control currentControl, Control parentPanel)
        {
            currentControl.Cursor = Cursors.Hand;
            currentControl.Click += (sender, e) =>
            {
                QuickActionClickEvent.OnClick(parentPanel, e);
            };

            foreach (Control child in currentControl.Controls)
            {
                InternalClickBinder(child, parentPanel);
            }
        }
    }
}
