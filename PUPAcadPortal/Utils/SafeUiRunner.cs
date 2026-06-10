using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PUPAcadPortal.Utils
{
    public static class SafeUiRunner
    {
        /// <summary>
        /// Automatically locks the UI, runs the database task, catches all errors, and unlocks the UI.
        /// </summary>
        /// <param name="action">The async code you want to run</param>
        /// <param name="controlsToLock">The controls to disable (pass 'this' to lock the whole UserControl)</param>
        public static async Task ExecuteAsync(Func<Task> action, params Control[] controlsToLock)
        {
            using var uiLock = new UiLockState(controlsToLock);

            try
            {
                await action();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Action Blocked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A system error occurred:\n\n{ex.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}