using PUPAcadPortal.PortalContents.Instructor.LMS;
using PUPAcadPortal.PortalContents.Student.LMS;
using PUPAcadPortal.Utils;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Text;

namespace PUPAcadPortal.Utils
{
    public static class ViewUtil
    {
        /// <summary>
        /// Swaps the content of a container panel with a new UserControl view cleanly.
        /// </summary>
        /// <param name="containerPanel">The main hosting panel of the form (e.g., mainContentPanel).</param>
        /// <param name="newView">The instance of the UserControl to display.</param>

        public static void ShowView(this Panel containerPanel, UserControl newView)
        {
            if (newView != null)
            {
                if (containerPanel.Controls.Count > 0 && containerPanel.Controls[0].GetType() == newView.GetType())
                {
                    newView.Dispose();
                    return;
                }
                // Disposing old controls to prevent memory leaks in RAM
                foreach (Control ctrl in containerPanel.Controls)
                {
                    EventUnbinder.ClearAllEvents(ctrl);
                    ctrl.Dispose();
                }

                containerPanel.SuspendLayout();
                // Clearing the container completely
                containerPanel.Controls.Clear();

                // Configure the layout rules for the incoming view
                newView.Dock = DockStyle.Fill;

                containerPanel.Controls.Add(newView);
                containerPanel.ResumeLayout(true);
            }
        }
    }
}
