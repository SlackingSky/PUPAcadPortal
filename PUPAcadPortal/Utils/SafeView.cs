using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.Utils
{
    public static class SafeView
    {
        /// <summary>
        /// Safely executes UI updates, automatically checking if the control is disposed or cross-threaded.
        /// </summary>
        public static void SafeUIUpdate(this Control control, Action action)
        {
            if (control == null || control.IsDisposed || control.Disposing)
                return;

            if (control.InvokeRequired)
            {
                control.Invoke((MethodInvoker)delegate
                {
                    if (!control.IsDisposed && !control.Disposing)
                    {
                        action();
                    }
                });
            }
            else
            {
                action();
            }
        }
    }
}
