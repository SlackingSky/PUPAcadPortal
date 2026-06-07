using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PUPAcadPortal.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PUPAcadPortal.PortalContents.Admin.SubOffering
{
    public partial class CurriculumArchiveContentAdmin : UserControl
    {
        public CurriculumArchiveContentAdmin()
        {
            InitializeComponent();
            LoadCurriculum();
            Blanks();
            dgvCurriculum.AllowUserToAddRows = true;
            dgvCurriculum.AllowUserToDeleteRows = true;
        }

        private void LoadCurriculum()
        {
            dgvCurriculum.Rows.Clear();

            using (var db = new AppDbContext())
            {
                var list = db.Curricula
                    .Include(c => c.Subject)
                    .ToList();

                foreach (var c in list)
                {
                    int rowIndex = dgvCurriculum.Rows.Add();

                    dgvCurriculum.Rows[rowIndex].Cells["CourseCode2"].Value = c.Subject.SubjectCode;
                    dgvCurriculum.Rows[rowIndex].Cells["CourseTitle2"].Value = c.Subject.SubjectName;
                    dgvCurriculum.Rows[rowIndex].Cells["colProgram"].Value = c.Program;
                    dgvCurriculum.Rows[rowIndex].Cells["Year2"].Value = c.YearLevel;
                    dgvCurriculum.Rows[rowIndex].Cells["Semester1"].Value = c.SemesterIndex;
                    dgvCurriculum.Rows[rowIndex].Cells["colRevisionYear"].Value = c.RevisionYear;
                    dgvCurriculum.Rows[rowIndex].Cells["Lec2"].Value = c.Subject.LecUnits;
                    dgvCurriculum.Rows[rowIndex].Cells["Lab2"].Value = c.Subject.LabUnits;
                    dgvCurriculum.Rows[rowIndex].Cells["TotalUnits2"].Value = c.Subject.Units;
                }
            }
        }
        private void Blanks(int count = 20)
        {
            for (int i = 0; i < count; i++)
            {
                int rowIndex = dgvCurriculum.Rows.Add();
                dgvCurriculum.Rows[rowIndex].Cells["CourseCode2"].Value = "";
                dgvCurriculum.Rows[rowIndex].Cells["colProgram"].Value = "";
            }
        }
        private void btnCurriculum_Click(object sender, EventArgs e)
        {
            pnlCurriculum.Visible = true;
            pnlArchive.Visible = false;
        }

        private void btnArchive_Click(object sender, EventArgs e)
        {
            pnlArchive.Visible = true;
            pnlCurriculum.Visible = false;
        }

        private void btnUpdateCurriculum_Click(object sender, EventArgs e)
        {
            using (var db = new AppDbContext())
            {
                foreach (DataGridViewRow row in dgvCurriculum.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    string subjectCode = row.Cells["CourseCode2"].Value?.ToString();
                    string program = row.Cells["colProgram"].Value?.ToString();

                    if (string.IsNullOrWhiteSpace(subjectCode))
                        continue;

                    if (!int.TryParse(row.Cells["Year2"].Value?.ToString(), out int year))
                        continue;

                    if (!int.TryParse(row.Cells["Semester1"].Value?.ToString(), out int semester))
                        continue;

                    if (!int.TryParse(row.Cells["colRevisionYear"].Value?.ToString(), out int revisionYear))
                        continue;

                    var subject = db.Subjects
                        .FirstOrDefault(s => s.SubjectCode == subjectCode);

                    if (subject == null)
                        continue;

                    var curriculum = db.Curricula.FirstOrDefault(c =>
                        c.SubjectId == subject.SubjectId &&
                        c.Program == program);

                    if (curriculum == null)
                    {
                        curriculum = new Curriculum();
                        db.Curricula.Add(curriculum);
                    }

                    curriculum.SubjectId = subject.SubjectId;
                    curriculum.Program = program;
                    curriculum.YearLevel = year;
                    curriculum.SemesterIndex = semester;
                    curriculum.RevisionYear = revisionYear;
                }
                var dbCurricula = db.Curricula
                    .Include(c => c.Subject)
                    .ToList();

                foreach (var dbItem in dbCurricula)
                {
                    bool existsInGrid = false;

                    foreach (DataGridViewRow row in dgvCurriculum.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string subjectCode = row.Cells["CourseCode2"].Value?.ToString();
                        string program = row.Cells["colProgram"].Value?.ToString();

                        if (string.IsNullOrWhiteSpace(subjectCode))
                            continue;

                        var subject = db.Subjects
                            .FirstOrDefault(s => s.SubjectCode == subjectCode);

                        if (subject == null)
                            continue;

                        if (dbItem.SubjectId == subject.SubjectId &&
                            dbItem.Program == program)
                        {
                            existsInGrid = true;
                            break;
                        }
                    }

                    if (!existsInGrid)
                    {
                        db.Curricula.Remove(dbItem);
                    }
                }

                db.SaveChanges();
            }

            MessageBox.Show("Curriculum saved successfully.");
            LoadCurriculum();
        }
    }
}
