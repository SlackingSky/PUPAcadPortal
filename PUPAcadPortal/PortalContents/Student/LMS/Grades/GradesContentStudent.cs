using PUPAcadPortal.PortalContents.Student.LMS.Grades;
using System;
using System.IO;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Student.LMS.Grades
{
    /// <summary>
    /// Shell UserControl that hosts the full <see cref="GradesPanel"/> dashboard.
    /// All grade data, filtering, breakdown, and COG generation now live inside
    /// GradesPanel – this file is kept thin so the Designer can still reference it.
    /// </summary>
    public partial class GradesContentStudent : UserControl
    {
        public GradesContentStudent()
        {
            InitializeComponent();
        }

        // ─────────────────────────────────────────────────────────────────
        //  LOAD
        // ─────────────────────────────────────────────────────────────────
        private void GradesContentStudent_Load(object sender, EventArgs e)
        {
            Grades_Initialize();
        }

        // ─────────────────────────────────────────────────────────────────
        //  INITIALISE
        //  Clears pnlGrades and fills it with a docked GradesPanel instance.
        //  GradesPanel owns all logic: data, filtering, COG generation, etc.
        // ─────────────────────────────────────────────────────────────────
        private void Grades_Initialize()
        {
            var gradesPanel = new GradesPanel
            {
                Dock = DockStyle.Fill
            };

            pnlGrades.Controls.Clear();
            pnlGrades.Controls.Add(gradesPanel);
        }

        // ─────────────────────────────────────────────────────────────────
        //  LEGACY btnGenerate handler kept for the Designer-generated wiring.
        //  Actual COG generation is handled inside GradesPanel.BtnGenerateCOG_Click.
        //  This stub is only reached if someone clicks the old btnGenerate that
        //  still exists in GradesContentStudent.Designer.cs.
        // ─────────────────────────────────────────────────────────────────
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            // The GradesPanel's own Generate COG button handles generation.
            // This button can be hidden or removed from the designer if desired.
            MessageBox.Show(
                "Please use the 'Generate COG' button inside the Grades dashboard.",
                "Information",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}