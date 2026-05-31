using System;
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
            if (_attendanceControl != null) return;

            this.Controls.Clear();

            _attendanceControl = new AttendanceControl
            {
                Dock = DockStyle.Fill
            };
            this.Controls.Add(_attendanceControl);
        }
    }
}