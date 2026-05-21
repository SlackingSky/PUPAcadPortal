using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Admin.Enrollment
{
    public partial class AccountsContentAdmin : UserControl
    {
        public AccountsContentAdmin()
        {
            InitializeComponent();
            pnlAccountingRecordsContent.Layout += (s, e) => ResizeAccountingRecordsContent();
        }

        private void AdminAccountsContent_Resize(object sender, EventArgs e)
        {
            if (pnlAccountingRecordsContent.Visible)
            {
                ResizeAccountingRecordsContent();
            }
        }

        private void ResizeAccountingRecordsContent()
        {
            if (!pnlAccountingRecordsContent.Visible) return;

            // Usable width inside the panel (subtract side padding, e.g., 16px left + 16px right)
            int contentWidth = pnlAccountingRecordsContent.Width - 32; // 16px each side
            int gap = 10; // space between cards

            // Three cards: Total Amount, Paid Amount, Unpaid Amount
            int cardWidth = (contentWidth - (gap * 2)) / 3;

            // Resize and reposition the three summary cards
            pnlARTotalAmount.Width = cardWidth;
            pnlARPaidAmount.Width = cardWidth;
            pnlARUnpaidAmount.Width = cardWidth;

            pnlARTotalAmount.Left = 16;
            pnlARPaidAmount.Left = 16 + cardWidth + gap;
            pnlARUnpaidAmount.Left = 16 + (cardWidth + gap) * 2;

        }
    }
}
