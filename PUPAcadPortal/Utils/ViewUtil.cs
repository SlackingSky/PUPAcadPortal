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
        /// <param name="containerPanel">The main hosting panel of the form (e.g., mainContentPanel).</param>
        /// <param name="newView">The instance of the UserControl to display.</param>
        private static UserControl _oldView;
        public static void ShowView(this Panel containerPanel, UserControl newView)
        {
            if (_oldView == null || _oldView?.GetType().Name != newView.GetType().Name)
            {
                // Disposing old controls to prevent memory leaks in RAM
                foreach (Control ctrl in containerPanel.Controls)
                {
                    ctrl.Dispose();
                }

                // Clearing the container completely
                containerPanel.Controls.Clear();

                // Configure the layout rules for the incoming view
                newView.Dock = DockStyle.Fill;

                // Drop it into the panel container
                containerPanel.Controls.Add(newView);
                _oldView = newView;
            }
        }
    }
}
