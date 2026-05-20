using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Admin
{
    public partial class EnrolledStudentsContentAdmin : UserControl
    {
        public EnrolledStudentsContentAdmin()
        {
            InitializeComponent();
            pnlEnrolledStudentsContent.Layout += (s, e) => ResizeEnrolledStudentsContent();
            ResizeEnrolledStudentsContent();
        }

        private void ResizeEnrolledStudentsContent()
        {
            if (!pnlEnrolledStudentsContent.Visible) return;

            int contentWidth = pnlEnrolledStudentsContent.ClientSize.Width - 32; // use ClientSize, not Width
            int gap = 10;
            int cardWidth = (contentWidth - (gap * 3)) / 4;

            pnlESTotalStudentsCard.Width = cardWidth;
            pnlESActiveCard.Width = cardWidth;
            pnlESInactiveCard.Width = cardWidth;
            pnlESGraduatedCard.Width = cardWidth;

            pnlESTotalStudentsCard.Left = 16;
            pnlESActiveCard.Left = 16 + cardWidth + gap;
            pnlESInactiveCard.Left = 16 + (cardWidth + gap) * 2;
            pnlESGraduatedCard.Left = 16 + (cardWidth + gap) * 3;

        }
    }
}
