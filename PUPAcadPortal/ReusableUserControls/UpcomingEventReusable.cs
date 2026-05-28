using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Instructor.Enrollment
{

    public partial class UpcomingEventReusable : UserControl
    {

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string EventTitle
        {
            get => lblTitle.Text; set => lblTitle.Text = value;
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string EventTime
        {
            get => lblTime.Text; set => lblTime.Text = value;
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string EventMonth
        {
            get => lblMonth.Text; set => lblMonth.Text = value;
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string EventDay
        {
            get => lblDay.Text; set => lblDay.Text = value;
        }

        public UpcomingEventReusable()
        {
            InitializeComponent();
        }
    }
}
