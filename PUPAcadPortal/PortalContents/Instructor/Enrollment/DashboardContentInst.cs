using PUPAcadPortal.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PUPAcadPortal.PortalContents.Instructor.LMS;
using PUPAcadPortal.Data;
using PUPAcadPortal.Services;


namespace PUPAcadPortal.PortalContents.Instructor.Enrollment
{
    public partial class DashboardContentInst : UserControl
    {
        private readonly int _profId = UserSession.ProfessorID ?? 0;
        private ProfDashboardServices _services = new();

        public DashboardContentInst()
        {
            InitializeComponent();
        }

        private async void DashboardContentInst_Load(object sender, EventArgs e)
        {
            SetupQuickActionClicks();
            await LoadDashboardDataAsync();
        }

        private async Task LoadDashboardDataAsync()
        {
            var coursesHandled = _services.GetHandledCourses(_profId);
            var totalStudents = _services.GetTotalStudentsAsync(_profId);

            await Task.WhenAll(coursesHandled, totalStudents);

            this.SafeUIUpdate(async () =>
            {
                lblActiveCourses.Text = (await coursesHandled).ToString();
                lblTotalStudent.Text = (await totalStudents).ToString();
            });
        }

        private void SetupQuickActionClicks()
        {
            pnlQAGrades.BindClick();
            pnlQACourses.BindClick();
            pnlQAStudentList.BindClick();
        }
    }
}
