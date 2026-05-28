using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Instructor.Enrollment
{
    public partial class AnnouncementEnrollmentReusable : UserControl
    {
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string AnnounceTitle
        {
            get => lblTitle.Text; set => lblTitle.Text = value;
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string AnnounceDate
        {
            get => lblDate.Text; set => lblDate.Text = value;
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string AnnounceDesc
        {
            get => lblDescription.Text; set => lblDescription.Text = value;
        }

        public AnnouncementEnrollmentReusable()
        {
            InitializeComponent();
        }
    }
}
