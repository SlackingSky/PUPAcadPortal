using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Events
{
    public class InfoChangedEvent
    {
        public static event EventHandler InfoChanged;
    
        public static void RaiseInfoChanged()
        {
            InfoChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}
