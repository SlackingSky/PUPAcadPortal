using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Utils
{
    public static class MaskedTxtCursorMover
    {
        public static void MakeCursorGotoStart(this MaskedTextBox mtb)
        {
            if (mtb.Tag?.ToString() == "CursorBound") return;
            mtb.Tag = "CursorBound";

            mtb.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Space)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            };

            mtb.KeyUp += (s, e) => EnforcePhoneCursor(mtb);

            mtb.Click += (s, e) =>
            {
                MoveMaskedCursorToStart(mtb);
                EnforcePhoneCursor(mtb);
            };

            mtb.Enter += (s, e) =>
            {
                var form = mtb.FindForm();
                if (form != null && !form.IsDisposed)
                {
                    form.BeginInvoke(new Action(() =>
                    {
                        if (mtb.IsDisposed) return;
                        MoveMaskedCursorToStart(mtb);
                        EnforcePhoneCursor(mtb);
                    }));
                }
            };
        }

        private static void EnforcePhoneCursor(MaskedTextBox maskedTextBox)
        {
            int startIndex = maskedTextBox.Mask.IndexOf('0');
            if (startIndex == -1) startIndex = 0;

            if (maskedTextBox.SelectionLength > 0) return;

            if (maskedTextBox.SelectionStart < startIndex)
            {
                maskedTextBox.SelectionStart = startIndex;
            }
        }

        private static void MoveMaskedCursorToStart(MaskedTextBox mtb)
        {
            mtb.FindForm()?.BeginInvoke(new Action(() =>
            {
                if (mtb.MaskedTextProvider != null && mtb.MaskedTextProvider.AssignedEditPositionCount == 0)
                {
                    int firstEditIndex = mtb.MaskedTextProvider.FindEditPositionFrom(0, true);
                    if (firstEditIndex != -1)
                    {
                        mtb.SelectionStart = firstEditIndex;
                    }
                    else
                    {
                        mtb.SelectionStart = 0;
                    }
                }
            }));
        }
    }
}
