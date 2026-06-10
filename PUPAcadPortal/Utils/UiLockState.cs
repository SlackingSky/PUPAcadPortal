using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PUPAcadPortal.Utils
{
    public class UiLockState : IDisposable
    {
        private readonly Dictionary<Control, bool> _originalStates = new Dictionary<Control, bool>();
        private readonly System.Windows.Forms.Timer _safeguardTimer;
        private bool _isDisposed = false;

        /// <summary>
        /// Locks specific controls and sets a wait cursor. Automatically restores their exact previous state on Dispose or Timeout.
        /// </summary>
        /// <param name="controlsToLock">A comma-separated list of controls to lock</param>
        public UiLockState(params Control[] controlsToLock)
        {
            foreach (var ctrl in controlsToLock)
            {
                if (ctrl != null && !ctrl.IsDisposed)
                {
                    _originalStates[ctrl] = ctrl.Enabled;
                    ctrl.Enabled = false;                
                }
            }

            Application.UseWaitCursor = true;

            _safeguardTimer = new System.Windows.Forms.Timer { Interval = 15000 };
            _safeguardTimer.Tick += SafeguardTimer_Tick;
            _safeguardTimer.Start();
        }

        private void SafeguardTimer_Tick(object sender, EventArgs e)
        {
            RestoreState();
        }

        public void Dispose()
        {
            RestoreState();
        }

        private void RestoreState()
        {
            if (_isDisposed) return;
            _isDisposed = true;

            if (_safeguardTimer != null)
            {
                _safeguardTimer.Stop();
                _safeguardTimer.Dispose();
            }

            Application.UseWaitCursor = false;

            foreach (var kvp in _originalStates)
            {
                var ctrl = kvp.Key;
                var wasEnabled = kvp.Value;

                if (ctrl != null && !ctrl.IsDisposed)
                {
                    ctrl.Enabled = wasEnabled;
                }
            }
        }
    }
}