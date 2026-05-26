using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class ContentResizer
{
    private readonly UserControl _targetUserControl;
    private readonly Size _initialFormSize;
    private readonly List<KeyValuePair<Control, ControlData>> _controlDataList = new List<KeyValuePair<Control, ControlData>>();
    private readonly float _minFontSize;

    private struct ControlData
    {
        public Rectangle Bounds;
        public float OriginalFontSize;
        public string FontName;
        public FontStyle FontStyle;
    }

    /// <summary>
    /// Initializes the resizer safely. Call this right after InitializeComponent().
    /// </summary>
    public ContentResizer(UserControl userControl, float minFontSize = 8.0f)
    {
        _targetUserControl = userControl ?? throw new ArgumentNullException(nameof(userControl));
        _initialFormSize = userControl.ClientSize;
        _minFontSize = minFontSize;

        // Cache positions safely using a linear list to avoid dictionary lookups during drag operations
        RegisterControls(_targetUserControl);

        _targetUserControl.Resize += OnFormResize;
    }

    private void RegisterControls(Control parent)
    {
        if (parent == null || parent.Controls == null) return;

        foreach (Control control in parent.Controls)
        {
            if (control == null || control.IsDisposed) continue;

            // Safe font fallback checking
            string fontName = "Arial";
            float fontSize = 10f;
            FontStyle fontStyle = FontStyle.Regular;

            if (control.Font != null)
            {
                fontName = control.Font.Name ?? "Arial";
                fontSize = control.Font.Size;
                fontStyle = control.Font.Style;
            }

            _controlDataList.Add(new KeyValuePair<Control, ControlData>(control, new ControlData
            {
                Bounds = control.Bounds,
                OriginalFontSize = fontSize,
                FontName = fontName,
                FontStyle = fontStyle
            }));

            if (control.HasChildren)
            {
                RegisterControls(control);
            }
        }
    }

    private void OnFormResize(object sender, EventArgs e)
    {
        // Prevent division by zero crashes when minimized
        if (_targetUserControl == null || _targetUserControl.IsDisposed) return;
        if (_targetUserControl.ClientSize.Width <= 0 || _targetUserControl.ClientSize.Height <= 0) return;

        float widthRatio = (float)_targetUserControl.ClientSize.Width / _initialFormSize.Width;
        float heightRatio = (float)_targetUserControl.ClientSize.Height / _initialFormSize.Height;
        float fontRatio = Math.Min(widthRatio, heightRatio);

        // Suspend rendering to prevent flickering and stop layout loops
        _targetUserControl.SuspendLayout();

        // Loop through a static index array to ensure changes to controls don't crash the loop
        int totalControls = _controlDataList.Count;
        for (int i = 0; i < totalControls; i++)
        {
            var kvp = _controlDataList[i];
            Control control = kvp.Key;

            // Skip controls that were dynamically destroyed/disposed during app runtime
            if (control == null || control.IsDisposed) continue;

            ControlData initial = kvp.Value;

            try
            {
                // Rescale bounds
                int newX = (int)(initial.Bounds.X * widthRatio);
                int newY = (int)(initial.Bounds.Y * heightRatio);
                int newWidth = (int)(initial.Bounds.Width * widthRatio);
                int newHeight = (int)(initial.Bounds.Height * heightRatio);

                control.SetBounds(newX, newY, newWidth, newHeight);

                // Compute and clamp font size
                float targetFontSize = initial.OriginalFontSize * fontRatio;
                if (targetFontSize < _minFontSize) targetFontSize = _minFontSize;
                if (targetFontSize > initial.OriginalFontSize) targetFontSize = initial.OriginalFontSize;

                // Safely update font only if value jumps past a noticeable delta
                if (control.Font == null || Math.Abs(control.Font.Size - targetFontSize) > 0.1f)
                {
                    Font oldFont = control.Font;
                    control.Font = new Font(initial.FontName, targetFontSize, initial.FontStyle);

                    // Native memory cleanup
                    if (oldFont != null && !oldFont.IsSystemFont)
                    {
                        oldFont.Dispose();
                    }
                }
            }
            catch
            {
                // Silent fail-safe for individual corrupt UI controls so the rest of the application survives
                continue;
            }
        }

        _targetUserControl.ResumeLayout(true);
    }
}
