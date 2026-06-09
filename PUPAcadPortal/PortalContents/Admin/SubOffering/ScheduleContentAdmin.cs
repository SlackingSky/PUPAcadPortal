using ClosedXML.Excel;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Models;
using System.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using PUPAcadPortal.Services;


namespace PUPAcadPortal.PortalContents.Admin.SubOffering
{
    public partial class ScheduleContentAdmin : UserControl
    {
        private readonly ScheduleViewService _exportService;

        public ScheduleContentAdmin()
        {
            InitializeComponent();
            _exportService = new ScheduleViewService();
            LoadSchedule();
        }

        private async void ScheduleContentAdmin_Load(object sender, EventArgs e)
        {

        }

        private void LoadSchedule()
        {
            dgvScheduleView.Rows.Clear();
            dgvScheduleView.AutoGenerateColumns = false;

            using (var db = new AppDbContext())
            {
                var list = db.RoomSchedules
                    .Include(r => r.SubjectOffering).ThenInclude(o => o.Subject)
                    .Include(r => r.SubjectOffering).ThenInclude(o => o.Professor).ThenInclude(p => p.User)
                    .Include(r => r.Room)
                    .ToList();

                var groupedOfferings = list.GroupBy(r => r.SubjectOfferingId).Select(g => g.First()).ToList();

                foreach (var r in groupedOfferings)
                {
                    int rowIndex = dgvScheduleView.Rows.Add();
                    dgvScheduleView.Rows[rowIndex].Cells["CourseCode1"].Value = r.SubjectOffering.Subject.SubjectCode;
                    dgvScheduleView.Rows[rowIndex].Cells["CourseTitle1"].Value = r.SubjectOffering.Subject.SubjectName;
                    dgvScheduleView.Rows[rowIndex].Cells["Lec1"].Value = r.SubjectOffering.Subject.LecUnits;
                    dgvScheduleView.Rows[rowIndex].Cells["Lab1"].Value = r.SubjectOffering.Subject.LabUnits;
                    dgvScheduleView.Rows[rowIndex].Cells["TotalUnits1"].Value = r.SubjectOffering.Subject.Units;
                    dgvScheduleView.Rows[rowIndex].Cells["Section1"].Value = r.SubjectOffering.Section;
                    dgvScheduleView.Rows[rowIndex].Cells["Day1"].Value = r.DayOfWeek;
                    dgvScheduleView.Rows[rowIndex].Cells["Start1"].Value = r.StartTime;
                    dgvScheduleView.Rows[rowIndex].Cells["End1"].Value = r.EndTime;
                    dgvScheduleView.Rows[rowIndex].Cells["Room1"].Value = r.Room != null ? r.Room.RoomName : "N/A";
                    dgvScheduleView.Rows[rowIndex].Cells["Instructor1"].Value =
                        r.SubjectOffering.Professor?.User != null
                            ? $"{r.SubjectOffering.Professor.User.LastName}, {r.SubjectOffering.Professor.User.FirstName}"
                            : "N/A";
                }
            }
        }

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "PDF Files (*.pdf)|*.pdf", FileName = "PDFClass Schedules.pdf" })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    var data = GetReportDataFromGrid();
                    _exportService.ExportToPdf(sfd.FileName, data);
                    MessageBox.Show("PDF Exported Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbooks (*.xlsx)|*.xlsx", FileName = "Class Schedules.xlsx" })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    var data = GetReportDataFromGrid();
                    _exportService.ExportToExcel(sfd.FileName, data);
                    MessageBox.Show("Excel Exported Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Excel Compilation Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private List<ScheduleReportItem> GetReportDataFromGrid()
        {
            var list = new List<ScheduleReportItem>();
            foreach (DataGridViewRow row in dgvScheduleView.Rows)
            {
                if (row.IsNewRow) continue;

                list.Add(new ScheduleReportItem
                {
                    CourseCode = row.Cells["CourseCode1"].Value?.ToString() ?? "",
                    CourseTitle = row.Cells["CourseTitle1"].Value?.ToString() ?? "",
                    LecUnits = Convert.ToInt32(row.Cells["Lec1"].Value ?? 0),
                    LabUnits = Convert.ToInt32(row.Cells["Lab1"].Value ?? 0),
                    Section = row.Cells["Section1"].Value?.ToString() ?? "",
                    Day = row.Cells["Day1"].Value?.ToString() ?? "",
                    TimeRange = $"{row.Cells["Start1"].Value} - {row.Cells["End1"].Value}",
                    Room = row.Cells["Room1"].Value?.ToString() ?? "N/A",
                    Instructor = row.Cells["Instructor1"].Value?.ToString() ?? "N/A"
                });
            }
            return list;
        }
    }
}