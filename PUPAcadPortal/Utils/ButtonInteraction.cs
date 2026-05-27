using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Utils
{
    internal class ButtonInteraction
    {
        private Button _clickedButton;
        private Color _defaultColor = Color.Maroon;
        private Color _selectedColor = Color.FromArgb(109, 0, 0);
        private Panel _pnlYellow;
        private FlowLayoutPanel _flowLayoutPanel;
        public void InitializeMyPanelEvents(Control panel)
        {
            foreach (Control child in panel.Controls)
            {
                if (child is Button)
                    child.Click += Clicked;

                if (child.HasChildren)
                {
                    InitializeMyPanelEvents(child);
                }
            }
        }

        private FlowLayoutPanel FindParentPanelByName(Control currentControl, string targetPanelName)
        {
            while (currentControl.Parent != null)
            {
                currentControl = currentControl.Parent;

                if (currentControl is FlowLayoutPanel && currentControl.Name == targetPanelName)
                {
                    return (FlowLayoutPanel)currentControl;
                }
            }

            return null;
        }

        private void Clicked(object sender, EventArgs e)
        {
            string flpName = "flowLayoutPanel1";
            if (_flowLayoutPanel == null)
            {
                _flowLayoutPanel = FindParentPanelByName((Button)sender, flpName);
            }
            changeButtonColor((Button)sender);
        }

        private async void changeButtonColor(Button button)
        {
            if (button == null) return;

            if (_clickedButton != null)
            {
                _clickedButton.BackColor = _defaultColor;
            }

            _clickedButton = button;

            if (_pnlYellow == null)
            {
                _pnlYellow = new Panel
                {
                    Name = "pnlYellow",
                    Size = new Size(3, button.Size.Height),
                    BackColor = Color.Gold,
                    Visible = false
                };

            }

            _clickedButton.BackColor = _selectedColor;
            _pnlYellow.Visible = true;
            _pnlYellow.Parent = button;
            _pnlYellow.Height = button.Height;
            _pnlYellow.BringToFront();
        }
    }
}
