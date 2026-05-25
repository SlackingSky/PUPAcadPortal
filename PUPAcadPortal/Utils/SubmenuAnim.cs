using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Utils
{
    public class SubmenuAnim
    {
        private readonly Control _panel;
        private readonly int _collapsedHeight = 0;
        private readonly int _expandedHeight;

        private bool _isExpanded = false;
        private bool _isAnimating = false;

        // Constructor to pass each menu's unique controls and dimensions
        public SubmenuAnim(Control panel, int expandedHeight)
        {
            if (panel is Panel)
            {
                _panel = panel as Panel;
            }
            else
            {
                _panel = panel as FlowLayoutPanel;
            }
            _expandedHeight = expandedHeight;
        }

        public async Task ToggleSubMenuAsync()
        {
            if (_isAnimating) return;
            _panel.Height = _isExpanded ? _expandedHeight : _collapsedHeight;
            _panel.Visible = true;
            _isAnimating = true;

            int targetHeight = _isExpanded ? _collapsedHeight : _expandedHeight;
            int step = _expandedHeight / 10;

            while (_panel.Height != targetHeight)
            {
                int newHeight = _isExpanded ? _panel.Height - step : _panel.Height + step;
                _panel.Height = _isExpanded ? Math.Max(newHeight, targetHeight) : Math.Min(newHeight, targetHeight);

                await Task.Delay(15);
            }

            _isExpanded = !_isExpanded;
            _panel.Visible = _isExpanded;
            _isAnimating = false;
        }
    }
}
