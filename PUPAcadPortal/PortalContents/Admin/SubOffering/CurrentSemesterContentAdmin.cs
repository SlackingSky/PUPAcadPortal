using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Admin.SubOffering
{
    public partial class CurrentSemesterContentAdmin : UserControl
    {
        public CurrentSemesterContentAdmin()
        {
            InitializeComponent();

            try { dgvCurrentSemester.Rows.Add(30); } catch { }

            string[,] dummyData =
{
                { "COMP 009", "Object Oriented Programming", "3.0", "2.0", "5.0" },
                { "COMP 010", "Information Management", "3.0", "2.0", "5.0" },
                { "COMP 012", "Network Administration", "3.0", "2.0", "5.0" }
            };

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    dgvCurrentSemester.Rows[row].Cells[col].Value = dummyData[row, col];
                }
            }

            for (int i = 0; i < 3; i++)
            {
                dgvCurrentSemester.Rows[i].Cells["Year"].Value = "2";
            }
        }

        private void btnSetCurrent_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Set {cmbSY.SelectedItem} semester {cmbSem.SelectedItem} as current.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
