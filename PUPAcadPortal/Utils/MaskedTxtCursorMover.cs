using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Utils
{
    public static class MaskedTxtCursorMover
    {
        private static void MakeCursosGotoStart(this MaskedTextBox maskedTextBox)
        {
            maskedTextBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Space)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            };

            maskedTextBox.KeyUp += (s, e) => EnforcePhoneCursor(maskedTextBox);

            maskedTextBox.Click += (s, e) => EnforcePhoneCursor(maskedTextBox);

            maskedTextBox.Enter += (s, e) => maskedTextBox.FindForm()?.BeginInvoke(new Action(() => EnforcePhoneCursor(maskedTextBox)));
            maskedTextBox.Click += (s, e) => MoveMaskedCursorToStart(maskedTextBox);
            maskedTextBox.Enter += (s, e) => MoveMaskedCursorToStart(maskedTextBox);
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
