using PUPAcadPortal.PortalContents.Student.LMS.Grades;
using PUPAcadPortal.Data;     // FIX: UserSession
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
            // FIX: GradesPanel now reads UserSession.StudentID internally to load
            // the correct student's grades from the database.  No extra wiring needed
            // here — just ensure the user is logged in before this panel is shown.
            if (UserSession.StudentID == null)
            {
                MessageBox.Show(
                    "No student is currently logged in. Please log in first.",
                    "Session Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

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