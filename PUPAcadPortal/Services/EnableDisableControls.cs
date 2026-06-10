using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Services
{
    public static class EnableDisableControls
    {
        public static void EnableControls(this Control control)
        {
            foreach (Control c in control.Controls)
            {
                c.Enabled = true;
                if (c.HasChildren)
                    c.EnableControls();
            }
        }

        public static void DisableControls(this Control control)
        {
            foreach (Control c in control.Controls)
            {
                c.Enabled = false;
                if (c.HasChildren)
                    c.EnableControls();
            }
        }
    }
}
