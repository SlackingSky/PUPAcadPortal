using ClosedXML.Excel;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Models;
using WkHtmlToPdfDotNet; //For Service Transfer

using System.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;


namespace PUPAcadPortal.PortalContents.Admin.SubOffering
{
    public partial class ScheduleContentAdmin : UserControl
    {

        public ScheduleContentAdmin()
        {
            InitializeComponent();
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
                    .Include(r => r.SubjectOffering)
                        .ThenInclude(o => o.Subject)
                    .Include(r => r.SubjectOffering)
                        .ThenInclude(o => o.Professor)
                            .ThenInclude(p => p.User)
                    .Include(r => r.Room)
                    .ToList();

                var groupedOfferings = list
                    .GroupBy(r => r.SubjectOfferingId)
                    .Select(g => g.First())
                    .ToList();

                foreach (var r in groupedOfferings)
                {
                    int rowIndex = dgvScheduleView.Rows.Add();

                    dgvScheduleView.Rows[rowIndex].Cells["CourseCode1"].Value =
                        r.SubjectOffering.Subject.SubjectCode;

                    dgvScheduleView.Rows[rowIndex].Cells["CourseTitle1"].Value =
                        r.SubjectOffering.Subject.SubjectName;

                    dgvScheduleView.Rows[rowIndex].Cells["Lec1"].Value =
                        r.SubjectOffering.Subject.LecUnits;

                    dgvScheduleView.Rows[rowIndex].Cells["Lab1"].Value =
                        r.SubjectOffering.Subject.LabUnits;

                    dgvScheduleView.Rows[rowIndex].Cells["TotalUnits1"].Value =
                        r.SubjectOffering.Subject.Units;

                    dgvScheduleView.Rows[rowIndex].Cells["Section1"].Value =
                        r.SubjectOffering.Section;

                    dgvScheduleView.Rows[rowIndex].Cells["Day1"].Value =
                        r.DayOfWeek;

                    dgvScheduleView.Rows[rowIndex].Cells["Start1"].Value =
                        r.StartTime;

                    dgvScheduleView.Rows[rowIndex].Cells["End1"].Value =
                        r.EndTime;

                    dgvScheduleView.Rows[rowIndex].Cells["Room1"].Value =
                        r.Room != null ? r.Room.RoomName : "N/A";

                    dgvScheduleView.Rows[rowIndex].Cells["Instructor1"].Value =
                        r.SubjectOffering.Professor != null && r.SubjectOffering.Professor.User != null
                            ? r.SubjectOffering.Professor.User.LastName + ", " + r.SubjectOffering.Professor.User.FirstName
                            : "N/A";
                }
            }
        }

        private void PDFVer()
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "PDF Files (*.pdf)|*.pdf", FileName = "PDFClass Schedules.pdf" })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;

                //Ui for when viewed in browser
                StringBuilder html = new StringBuilder("<html><head><style>body{font-family:Arial;padding:20px;} table{width:100%;border-collapse:collapse;} th{background:#800000;color:white;padding:8px;text-align:left;} td{border-bottom:1px solid #ddd;padding:8px;}</style></head><body>");
                html.Append("<h2>PUP Academic Portal — Master Class Schedules</h2><table><tr><th>Course Code</th><th>Title</th><th>Lec</th><th>Lab</th><th>Section</th><th>Day</th><th>Time</th><th>Room</th><th>Instructor</th></tr>");

                //DataGrid Info Extractor
                foreach (DataGridViewRow row in dgvScheduleView.Rows)
                {
                    if (row.IsNewRow) continue;
                    html.Append($"<tr>" +
                                $"<td>{row.Cells["CourseCode1"].Value}</td><td>{row.Cells["CourseTitle1"].Value}</td>" +
                                $"<td>{row.Cells["Lec1"].Value}</td><td>{row.Cells["Lab1"].Value}</td>" +
                                $"<td>{row.Cells["Section1"].Value}</td><td>{row.Cells["Day1"].Value}</td>" +
                                $"<td>{row.Cells["Start1"].Value} - {row.Cells["End1"].Value}</td>" +
                                $"<td>{row.Cells["Room1"].Value}</td><td>{row.Cells["Instructor1"].Value}</td>" +
                                $"</tr>");
                }
                html.Append("</table></body></html>");

                try
                {

                    //Write out to the temp directory
                    string tempFile = Path.Combine(Path.GetTempPath(), "report.html");
                    File.WriteAllText(tempFile, html.ToString());

                    //keep this, Line, NuGetPackage is not working :>(I think)
                    string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                    string edgePath = Path.Combine(programFiles, @"Microsoft\Edge\Application\msedge.exe");


                    if (!File.Exists(edgePath))
                    {
                        edgePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Microsoft\Edge\Application\msedge.exe");
                    }

                    ProcessStartInfo info = new ProcessStartInfo(edgePath, $"--headless --print-to-pdf=\"{sfd.FileName}\" \"{tempFile}\"")
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false
                    };

                    using (Process p = Process.Start(info))
                    {
                        p.WaitForExit();
                    }

                    // Deleter if Copy of same name exists
                    if (File.Exists(tempFile)) File.Delete(tempFile);
                    MessageBox.Show("PDF Exported Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void ExcelVer()
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbooks (*.xlsx)|*.xlsx", FileName = "Class Schedules.xlsx" })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    //Blank Excel Creator
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Class Schedule");

                        //UI
                        string[] headers = { "Course Code", "Title", "Lec", "Lab", "Section", "Day", "Time", "Room", "Instructor" };
                        for (int i = 0; i < headers.Length; i++)
                        {
                            var cell = worksheet.Cell(1, i + 1);
                            cell.Value = headers[i];
                            cell.Style.Font.Bold = true;
                            cell.Style.Font.FontName = "Segoe UI";
                            cell.Style.Font.FontColor = XLColor.White;
                            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#800000");
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        }

                        //DataGrid data extractor
                        int excelRowIndex = 2;
                        foreach (DataGridViewRow row in dgvScheduleView.Rows)
                        {
                            if (row.IsNewRow) continue;

                            worksheet.Cell(excelRowIndex, 1).Value = row.Cells["CourseCode1"].Value?.ToString() ?? "";
                            worksheet.Cell(excelRowIndex, 2).Value = row.Cells["CourseTitle1"].Value?.ToString() ?? "";
                            worksheet.Cell(excelRowIndex, 3).Value = Convert.ToInt32(row.Cells["Lec1"].Value ?? 0);
                            worksheet.Cell(excelRowIndex, 4).Value = Convert.ToInt32(row.Cells["Lab1"].Value ?? 0);
                            worksheet.Cell(excelRowIndex, 5).Value = row.Cells["Section1"].Value?.ToString() ?? "";
                            worksheet.Cell(excelRowIndex, 6).Value = row.Cells["Day1"].Value?.ToString() ?? "";
                            worksheet.Cell(excelRowIndex, 7).Value = $"{row.Cells["Start1"].Value} - {row.Cells["End1"].Value}";
                            worksheet.Cell(excelRowIndex, 8).Value = row.Cells["Room1"].Value?.ToString() ?? "N/A";
                            worksheet.Cell(excelRowIndex, 9).Value = row.Cells["Instructor1"].Value?.ToString() ?? "N/A";

                            //FontStyle toh
                            for (int col = 1; col <= headers.Length; col++)
                            {
                                var cell = worksheet.Cell(excelRowIndex, col);
                                cell.Style.Font.FontName = "Segoe UI";
                                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                cell.Style.Border.OutsideBorderColor = XLColor.LightGray;

                                // Center specific structural columns for high visual clarity
                                if (col == 3 || col == 4 || col == 5 || col == 6)
                                {
                                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                }
                            }

                            excelRowIndex++;
                        }

                        //Auto adjusts the table if chars are too long for table
                        worksheet.Columns().AdjustToContents();
                        workbook.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show("Excel Exported Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Excel Compilation Engine Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            PDFVer();
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExcelVer();
        }
    }
}