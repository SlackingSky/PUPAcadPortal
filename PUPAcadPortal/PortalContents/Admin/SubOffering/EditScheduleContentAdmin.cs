using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Admin.SubOffering
{
    public partial class EditScheduleContentAdmin : UserControl
    {
        public EditScheduleContentAdmin()
        {
            InitializeComponent();

            try { dgvEditSchedule.Rows.Add(30); } catch { }

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
                    dgvEditSchedule.Rows[row].Cells[col].Value = dummyData[row, col];
                }
            }
            // Mark original rows
            for (int i = 0; i < 3; i++)
            {
                dgvEditSchedule.Rows[i].Tag = "original";
            }
        }
    }
}
