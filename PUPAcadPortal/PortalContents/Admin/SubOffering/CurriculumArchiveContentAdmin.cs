using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Admin.SubOffering
{
    public partial class CurriculumArchiveContentAdmin : UserControl
    {
        public CurriculumArchiveContentAdmin()
        {
            InitializeComponent();

            try { dgvArchive.Rows.Add(25); } catch { }

            string[,] curriculumData =
            {
                // First Year - First Semester
                { "COMP 001", "Introduction to Computing", "3", "2", "3", "1" },
                { "COMP 002", "Computer Programming 1", "3", "2", "3", "1" },
                { "ACCO 014", "Principles of Accounting", "0", "3", "3", "1" },
                { "GEED 004", "Mathematics in the Modern World", "0", "3", "3", "1" },
                { "GEED 005", "Purposive Communication", "0", "3", "3", "1" },
                { "GEED 032", "Filipinolohiya at Pambansang Kaunlaran", "0", "3", "3", "1" },
                { "PATHFit 1", "Physical Activities Towards Health and Fitness 1", "0", "2", "2", "1" },
                { "NSTP 001", "National Service Training Program 1", "0", "3", "3", "1" },

                // First Year - Second Semester
                { "COMP 003", "Computer Programming 2", "3", "2", "3", "1" },
                { "COMP 004", "Discrete Structures 1", "0", "3", "3", "1" },
                { "COMP 008", "Data Communications and Networking", "3", "2", "3", "1" },
                { "GEED 002", "Readings in Philippine History", "0", "3", "3", "1" },
                { "GEED 010", "People and the Earth's Ecosystem", "0", "3", "3", "1" },
                { "GEED 020", "Politics, Governance and Citizenship", "0", "3", "3", "1" },
                { "PATHFit 2", "Physical Activities Towards Health and Fitness 2", "0", "2", "2", "1" },
                { "NSTP 002", "National Service Training Program 2", "0", "3", "3", "1" },

                // Second Year - First Semester
                { "INTE 201", "Programming 3 (Structured Programming)", "3", "2", "3", "2" },
                { "COMP 006", "Data Structures and Algorithms", "3", "2", "3", "2" },
                { "COMP 007", "Operating Systems", "3", "2", "3", "2" },
                { "GEED 001", "Understanding the Self", "0", "3", "3", "2" },
                { "GEED 028", "Reading Visual Arts", "0", "3", "3", "2" },
                { "INTE-FE1", "BSIT Free Elective 1", "0", "3", "3", "2" },

                // Second Year - Second Semester
                { "INTE 202", "Integrative Programming and Technologies 1", "3", "2", "3", "2" },
                { "COMP 009", "Object-Oriented Programming", "3", "2", "3", "2" },
                { "COMP 010", "Information Management", "0", "3", "3", "2" },
                { "COMP 012", "Network Administration", "3", "2", "3", "2" },
                { "COMP 013", "Human Computer Interaction", "0", "3", "3", "2" },
                { "COMP 014", "Quantitative Methods with Modeling and Simulation", "0", "3", "3", "2" },
                { "INTE-FE2", "BSIT Free Elective 2", "0", "3", "3", "2" },

                // Third Year - First Semester
                { "INTE 401", "Information Assurance and Security 2", "0", "3", "3", "3" },
                { "CIT 3101", "Networking 2", "0", "3", "3", "3" },
                { "CIT 3102", "Systems Integration and Architecture", "0", "3", "3", "3" },
                { "CIT EL01", "Professional Elective 1", "0", "3", "3", "3" },
                { "CMR 1101", "Methods of Research for IT/IS", "0", "3", "3", "3" },
                { "CMS 1101", "Multimedia Systems", "0", "3", "3", "3" },
                { "GEED 0093", "Ethics", "0", "3", "3", "3" },
                { "GEED 0293", "Life and Works of Rizal", "0", "3", "3", "3" },

                // Third Year - Second Semester
                { "CIT 3201", "Networking 3", "0", "3", "3", "3" },
                { "CIT EL02", "Professional Elective 2", "0", "3", "3", "3" },
                { "CNA 1101", "Numerical Analysis for ITE", "0", "3", "3", "3" },
                { "CPP 4980", "Capstone Project and Research 1", "0", "3", "3", "3" },
                { "GEED 0073", "Art Appreciation", "0", "3", "3", "3" },
                { "GEED 0083", "Science, Technology, and Society", "0", "3", "3", "3" },
                { "GEED EL02", "GE Elective 2", "0", "3", "3", "3" },
                { "ZPD 1102", "Effective Communication with Personality Development", "0", "3", "3", "3" },

                // Fourth Year - First Semester
                { "CIT 4101", "Certification Course", "0", "3", "3", "4" },
                { "CIT 4102", "Systems Administration and Maintenance", "0", "3", "3", "4" },
                { "CIT EL03", "Professional Elective 3", "0", "3", "3", "4" },
                { "CIT EL04", "Professional Elective 4", "0", "3", "3", "4" },
                { "CPD 4990", "Capstone Project and Research 2", "0", "3", "3", "4" },
                { "CQM 1101", "Quantitative Methods (including Modeling and Simulation)", "0", "3", "3", "4" },
                { "GEED EL03", "GE Elective 3", "0", "3", "3", "4" },

                // Fourth Year - Second Semester
                { "CPR 4970", "Practicum for IT/IS", "0", "6", "6", "4" },
            };

            int totalRows = curriculumData.GetLength(0);
            try { dgvCurriculum.Rows.Add(totalRows > 35 ? totalRows : 35); } catch { }

            for (int row = 0; row < totalRows; row++)
            {
                for (int col = 0; col < 6; col++)
                {
                    dgvCurriculum.Rows[row].Cells[col].Value = curriculumData[row, col];
                }
            }

            string[,] archiveData =
            {
                { "1st Semester", "2025-2026", "Archived" },
                { "2nd Semester", "2026-2027", "Active" },
            };

            for (int row = 0; row < archiveData.GetLength(0); row++)
            {
                for (int col = 0; col < archiveData.GetLength(1); col++)
                {
                    dgvArchive.Rows[row].Cells[col].Value = archiveData[row, col];
                }
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
    }
}
