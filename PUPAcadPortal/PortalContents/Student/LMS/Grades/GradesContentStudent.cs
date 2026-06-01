using PUPAcadPortal.PortalContents.Student.LMS.Grades;
using System;
using System.IO;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Student.LMS.Grades
{
    public partial class GradesContentStudent : UserControl
    {
        public GradesContentStudent()
        {
            InitializeComponent();
        }

        //  LOAD
        private void GradesContentStudent_Load(object sender, EventArgs e)
        {
            Grades_Initialize();
        }

        private void Grades_Initialize()
        {
            var gradesPanel = new GradesPanel
            {
                Dock = DockStyle.Fill
            };

            pnlGrades.Controls.Clear();
            pnlGrades.Controls.Add(gradesPanel);
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Please use the 'Generate COG' button inside the Grades dashboard.",
                "Information",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}