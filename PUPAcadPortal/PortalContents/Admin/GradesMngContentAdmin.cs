using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Admin
{
    public partial class GradesMngContentAdmin : UserControl
    {

        private DataTable gradesTable;
        public GradesMngContentAdmin()
        {
            InitializeComponent();
            ResizeGradesManagementContent();
        }


        private void ResizeGradesManagementContent()
        {
            if (!pnlGradesManagementContent.Visible) return;

            // Adjust any grade management specific layouts here if needed
            pnlGradesManagementContainer.Width = pnlGradesManagementContent.ClientSize.Width - 62;
            pnlGradesManagementContainer.Height = pnlGradesManagementContent.ClientSize.Height - 150;

            dgvGrades.Width = pnlGradesManagementContainer.ClientSize.Width - 32;
            dgvGrades.Height = pnlGradesManagementContainer.ClientSize.Height - 70;
        }

        private void UpdateGradesPlaceholderVisibility()
        {
            bool hasRows = gradesTable.Rows.Count > 0;
            //label38.Visible = !hasRows;    // "No grades found"
            //pictureBox7.Visible = !hasRows;
            dgvGrades.Visible = hasRows;
        }

        private void AdminGradesMngContent_Load(object sender, EventArgs e)
        {
            gradesTable = new DataTable();
            gradesTable.Columns.Add("StudentName", typeof(string));
            gradesTable.Columns.Add("StudentID", typeof(string));
            gradesTable.Columns.Add("SubjectCode", typeof(string));
            gradesTable.Columns.Add("SubjectName", typeof(string));
            gradesTable.Columns.Add("Semester", typeof(string));
            gradesTable.Columns.Add("AcademicYear", typeof(string));
            gradesTable.Columns.Add("Midterm", typeof(decimal));
            gradesTable.Columns.Add("Final", typeof(decimal));
            gradesTable.Columns.Add("FinalRating", typeof(decimal));
            gradesTable.Columns.Add("Remarks", typeof(string));
            //gradesTable.Rows.Add("John Doe", "20210001", "CS101", "Introduction to Computer Science", "1st Semester", "2021-2022", 85.5m, 90.0m, 88.0m, "Passed");
            //gradesTable.Rows.Add("Jane Smith", "20210002", "CS102", "Data Structures", "1st Semester", "2021-2022", 78.0m, 82.5m, 80.0m, "Passed");
            //gradesTable.Rows.Add("Alice Johnson", "20210003", "CS103", "Algorithms", "1st Semester", "2021-2022", 92.0m, 95.0m, 93.5m, "Passed");

            dgvGrades.DataSource = gradesTable;
            dgvGrades.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            // Initially hide the grid and show the placeholder if no rows
            UpdateGradesPlaceholderVisibility();
        }
    }
}
