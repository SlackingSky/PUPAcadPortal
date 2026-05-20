using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Utils
{
    public static class ViewUtil
    {
        /// <summary>
        /// Swaps the content of a container panel with a new UserControl view cleanly.
        /// </summary>
        /// <param name="containerPanel">The main hosting panel on your form (e.g., mainContentPanel).</param>
        /// <param name="newView">The instance of the UserControl you want to display.</param>
        public static void ShowView(this Panel containerPanel, UserControl newView)
        {
            // 1. Dispose of old controls to prevent memory leaks in RAM
            foreach (Control ctrl in containerPanel.Controls)
            {
                ctrl.Dispose();
            }

            // 2. Clear the container completely
            containerPanel.Controls.Clear();

            // 3. Configure the layout rules for the incoming view
            newView.Dock = DockStyle.Fill;

            // 4. Drop it into the panel container
            containerPanel.Controls.Add(newView);
        }
    }
}
