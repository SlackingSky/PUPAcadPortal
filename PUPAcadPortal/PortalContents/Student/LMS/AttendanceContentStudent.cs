using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Student.LMS
{
    public partial class AttendanceContentStudent : UserControl
    {
        private AttendanceControl _attendanceControl;

        public AttendanceContentStudent()
        {
            InitializeComponent();
        }

        private void AttendanceContentStudent_Load(object sender, EventArgs e)
        {
            if (_attendanceControl == null)
            {
                pnlAttendance.Controls.Clear();

                _attendanceControl = new AttendanceControl();
                _attendanceControl.Dock = DockStyle.Fill;
                pnlAttendance.Controls.Add(_attendanceControl);
            }
        }
    }
}
