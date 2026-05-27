using PUPAcadPortal.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PUPAcadPortal.PortalContents.Instructor.LMS;

namespace PUPAcadPortal.PortalContents.Instructor.Enrollment
{
    public partial class DashboardContentInst : UserControl
    {
        public DashboardContentInst()
        {
            InitializeComponent();
        }

        private void DashboardContentInst_Load(object sender, EventArgs e)
        {
            pnlQAGrades.BindClick();
            pnlQACourses.BindClick();
            pnlQAStudentList.BindClick();
        }
    }
}
