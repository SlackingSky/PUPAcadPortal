using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Instructor.LMS
{
    public partial class AttendanceContentInst : UserControl
    {
        public AttendanceContentInst()
        {
            InitializeComponent();
        }

        private void AttendanceContentInst_Load(object sender, EventArgs e)
        {
            InitAttendance();
        }
    }
}
