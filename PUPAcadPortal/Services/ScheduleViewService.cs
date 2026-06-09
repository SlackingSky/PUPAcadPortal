using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using ClosedXML.Excel;

namespace PUPAcadPortal.Services
{
    public class ScheduleViewService
    {
        public void ExportToPdf(string outputPath, List<ScheduleReportItem> items)
        {
            // Build HTML template
            StringBuilder html = new StringBuilder("<html><head><style>@page{size:landscape;} body{font-family:Arial;padding:20px;} table{width:100%;border-collapse:collapse;} th{background:#800000;color:white;padding:8px;text-align:left;} td{border-bottom:1px solid #ddd;padding:8px;}</style></head><body>");
            html.Append("<h2>PUP Academic Portal — Class Schedule</h2><table><tr><th>Course Code</th><th>Title</th><th>Lec</th><th>Lab</th><th>Section</th><th>Day</th><th>Time</th><th>Room</th><th>Instructor</th></tr>");

            foreach (var item in items)
            {
                html.Append($"<tr>" +
                            $"<td>{item.CourseCode}</td><td>{item.CourseTitle}</td>" +
                            $"<td>{item.LecUnits}</td><td>{item.LabUnits}</td>" +
                            $"<td>{item.Section}</td><td>{item.Day}</td>" +
                            $"<td>{item.TimeRange}</td>" +
                            $"<td>{item.Room}</td><td>{item.Instructor}</td>" +
                            $"</tr>");
            }
            html.Append("</table></body></html>");

            string tempFile = Path.Combine(Path.GetTempPath(), $"report_{Guid.NewGuid()}.html");
            try
            {
                File.WriteAllText(tempFile, html.ToString());

                string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                string edgePath = Path.Combine(programFiles, @"Microsoft\Edge\Application\msedge.exe");

                if (!File.Exists(edgePath))
                {
                    edgePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Microsoft\Edge\Application\msedge.exe");
                }

                if (!File.Exists(edgePath))
                    throw new FileNotFoundException("Microsoft Edge execution path could not be found.");

                ProcessStartInfo info = new ProcessStartInfo(edgePath, $"--headless --print-to-pdf=\"{outputPath}\" \"{tempFile}\"")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false
                };

                using (Process p = Process.Start(info))
                {
                    p?.WaitForExit();
                }
            }
            finally
            {
                if (File.Exists(tempFile)) File.Delete(tempFile);
            }
        }

        public void ExportToExcel(string outputPath, List<ScheduleReportItem> items)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Class Schedule");

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

                int excelRowIndex = 2;
                foreach (var item in items)
                {
                    worksheet.Cell(excelRowIndex, 1).Value = item.CourseCode;
                    worksheet.Cell(excelRowIndex, 2).Value = item.CourseTitle;
                    worksheet.Cell(excelRowIndex, 3).Value = item.LecUnits;
                    worksheet.Cell(excelRowIndex, 4).Value = item.LabUnits;
                    worksheet.Cell(excelRowIndex, 5).Value = item.Section;
                    worksheet.Cell(excelRowIndex, 6).Value = item.Day;
                    worksheet.Cell(excelRowIndex, 7).Value = item.TimeRange;
                    worksheet.Cell(excelRowIndex, 8).Value = item.Room;
                    worksheet.Cell(excelRowIndex, 9).Value = item.Instructor;

                    for (int col = 1; col <= headers.Length; col++)
                    {
                        var cell = worksheet.Cell(excelRowIndex, col);
                        cell.Style.Font.FontName = "Segoe UI";
                        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.OutsideBorderColor = XLColor.LightGray;

                        if (col == 3 || col == 4 || col == 5 || col == 6)
                        {
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        }
                    }
                    excelRowIndex++;
                }

                worksheet.Columns().AdjustToContents();
                workbook.SaveAs(outputPath);
            }
        }
    }

    public class ScheduleReportItem
    {
        public string CourseCode { get; set; }
        public string CourseTitle { get; set; }
        public int LecUnits { get; set; }
        public int LabUnits { get; set; }
        public string Section { get; set; }
        public string Day { get; set; }
        public string TimeRange { get; set; }
        public string Room { get; set; }
        public string Instructor { get; set; }
    }
}