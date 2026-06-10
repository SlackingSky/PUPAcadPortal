using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using Svg.Skia;
using QRCoder; // <-- ADD THIS
using PUPAcadPortal.Data;
using PUPAcadPortal.Models;

namespace PUPAcadPortal.Services
{
    public class CorGenerationService
    {
        private string Escape(string text) => WebUtility.HtmlEncode(text ?? "");

        public async Task<string> GetEnrollmentId(int studentId, string activePeriod)
        {
            if (activePeriod == null)
                throw new ArgumentNullException(nameof(activePeriod), "There are no active period");
            if (studentId == 0)
                throw new ArgumentNullException(nameof(studentId), "Student ID not found");
            using (var context = new AppDbContext())
            {
                string enrollmentId = await context.Enrollments.Where(e => e.StudentId == studentId && e.AcademicPeriodId == activePeriod).Select(e => e.EnrollmentId).FirstOrDefaultAsync();
                if (enrollmentId == null)
                {
                    return "";
                }
                else
                    return enrollmentId;
            }
        }

        public async Task<string> GenerateCorPdfAsync(string enrollmentId, string outputPath, string svgTemplatePath = "CorTemplate.svg", string logoImagePath = "pup_logo.png")
        {
            using var db = new AppDbContext();

            var enrollment = await db.Enrollments
                .Include(e => e.Student).ThenInclude(s => s.User)
                .Include(e => e.AcademicPeriod)
                .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);

            if (enrollment == null) throw new Exception("Enrollment record not found.");

            var student = enrollment.Student;
            var user = student.User;

            var enrolledSubjects = await db.EnrollmentSubjects
                .Include(es => es.SubjectOffering).ThenInclude(so => so.Subject)
                .Include(es => es.SubjectOffering).ThenInclude(so => so.RoomSchedules)
                .Where(es => es.EnrollmentId == enrollmentId && es.SubjectStatus == "Officially Enrolled")
                .ToListAsync();

            var account = await db.StudentAccounts
                .FirstOrDefaultAsync(a => a.StudentId == student.StudentId && a.AcademicPeriodId == enrollment.AcademicPeriodId);

            var fees = account != null
                ? await db.FeeBreakdowns.Where(f => f.AccountId == account.AccountId).ToListAsync()
                : new List<FeeBreakdown>();

            if (!File.Exists(svgTemplatePath)) throw new Exception($"SVG Template not found at: {svgTemplatePath}");
            string svgContent = await File.ReadAllTextAsync(svgTemplatePath);

            string logoBase64 = "";
            if (File.Exists(logoImagePath))
            {
                byte[] logoBytes = await File.ReadAllBytesAsync(logoImagePath);
                logoBase64 = Convert.ToBase64String(logoBytes);
            }
            else
            {
                throw new Exception("E");
            }

            string qrBase64 = "";
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(student.StudentNumber, QRCodeGenerator.ECCLevel.Q))
            using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
            {
                byte[] qrCodeImage = qrCode.GetGraphic(20);
                qrBase64 = Convert.ToBase64String(qrCodeImage);
            }

            svgContent = svgContent.Replace("[YOUR_PUP_LOGO_BASE_64]", logoBase64)
                                   .Replace("[YOUR_QR_CODE_BASE_64]", qrBase64);

            string fullName = $"{user.LastName}, {user.FirstName} {user.MiddleName}".Trim().ToUpper();
            string ayString = $"20{enrollment.AcademicPeriod.SchoolYear.Substring(0, 2)}-20{enrollment.AcademicPeriod.SchoolYear.Substring(2, 2)}";
            string termDesc = enrollment.AcademicPeriod.Semester.ToUpper() == "1ST" ? "FIRST SEMESTER" :
                              enrollment.AcademicPeriod.Semester.ToUpper() == "2ND" ? "SECOND SEMESTER" : "SUMMER";
            string programDesc = student.Program == "BSIT" ? "BACHELOR OF SCIENCE IN INFORMATION TECHNOLOGY" : student.Program;
            string yearDesc = student.YearLevel switch { 1 => "First Year", 2 => "Second Year", 3 => "Third Year", 4 => "Fourth Year", _ => $"{student.YearLevel}th Year" };
            string address = $"{user.AddressLine1}, {user.CityMunicipality}, {user.Province}, (the) Philippines";
            string sectionCode = enrolledSubjects.FirstOrDefault()?.SubjectOffering.Section ?? "1";

            svgContent = svgContent.Replace("[STUDENT_NAME]", Escape(fullName))
                                   .Replace("[STUDENT_NO]", Escape(student.StudentNumber))
                                   .Replace("[ACAD_YEAR]", Escape(ayString))
                                   .Replace("[TERM]", Escape(termDesc))
                                   .Replace("[PROG_DESC]", Escape(programDesc))
                                   .Replace("[PROG_CODE]", Escape($"{student.Program}-SM"))
                                   .Replace("[CAMPUS]", "Santa Maria")
                                   .Replace("[YEAR_LEVEL]", Escape(yearDesc))
                                   .Replace("[SECTION]", Escape(sectionCode))
                                   .Replace("[ADDRESS]", Escape(address))
                                   .Replace("[CONTACT]", Escape(user.ContactNumber ?? "N/A"));

            decimal totalTuitionUnits = 0;
            decimal totalCreditedUnits = 0;

            for (int i = 0; i < 8; i++)
            {
                if (i < enrolledSubjects.Count)
                {
                    var sub = enrolledSubjects[i].SubjectOffering.Subject;
                    var sectionStr = $"{student.Program}-SM {student.YearLevel}-{enrolledSubjects[i].SubjectOffering.Section}";
                    decimal tuitionUnits = sub.LecUnits + sub.LabUnits;
                    decimal creditedUnits = sub.Units;

                    totalTuitionUnits += tuitionUnits;
                    totalCreditedUnits += creditedUnits;

                    svgContent = svgContent.Replace($"[SUB_CODE_{i}]", Escape(sub.SubjectCode))
                                           .Replace($"[SUB_TITLE_{i}]", Escape(sub.SubjectName))
                                           .Replace($"[SEC_CODE_{i}]", Escape(sectionStr))
                                           .Replace($"[T_UNITS_{i}]", tuitionUnits.ToString("0.0"))
                                           .Replace($"[C_UNITS_{i}]", creditedUnits.ToString("0.0"))
                                           .Replace($"[SCHED_{i}]", Escape(FormatSchedule(enrolledSubjects[i].SubjectOffering.RoomSchedules)));
                }
                else
                {
                    svgContent = svgContent.Replace($"[SUB_CODE_{i}]", "")
                                           .Replace($"[SUB_TITLE_{i}]", "")
                                           .Replace($"[SEC_CODE_{i}]", "")
                                           .Replace($"[T_UNITS_{i}]", "")
                                           .Replace($"[C_UNITS_{i}]", "")
                                           .Replace($"[SCHED_{i}]", "");
                }

                if (fees.Count == 0 && i == 0)
                {
                    svgContent = svgContent.Replace($"[FEE_NAME_{i}]", "- - - Not Applicable - - -")
                                           .Replace($"[FEE_AMT_{i}]", "Php 0.00");
                }
                else if (i < fees.Count)
                {
                    svgContent = svgContent.Replace($"[FEE_NAME_{i}]", Escape(fees[i].FeeName.Contains("RA 10931 (Free Higher Education Act)") ? "RA 10931" : fees[i].FeeName))
                                           .Replace($"[FEE_AMT_{i}]", $"Php {fees[i].Amount:N2}");
                }
                else
                {
                    svgContent = svgContent.Replace($"[FEE_NAME_{i}]", "")
                                           .Replace($"[FEE_AMT_{i}]", "");
                }
            }

            string assessmentText = account?.TotalAssessment <= 0 ? "FREE" : $"Php {account?.TotalAssessment:N2}";
            svgContent = svgContent.Replace("[TOTAL_T_UNITS]", totalTuitionUnits.ToString("0.0"))
                                   .Replace("[TOTAL_C_UNITS]", totalCreditedUnits.ToString("0.0"))
                                   .Replace("[MAX_UNITS]", "23.0")
                                   .Replace("[TOTAL_ASSESSMENT]", Escape(assessmentText));

            var svg = new SKSvg();
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(svgContent)))
            {
                svg.Load(stream);
            }

            float width = svg.Picture.CullRect.Width;
            float height = svg.Picture.CullRect.Height;

            using (var pdfStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
            using (var document = SKDocument.CreatePdf(pdfStream))
            {
                using (var canvas = document.BeginPage(width, height))
                {
                    canvas.DrawPicture(svg.Picture);
                    document.EndPage();
                }
                document.Close();
            }

            return outputPath;
        }

        private string FormatSchedule(ICollection<RoomSchedule>? schedules)
        {
            if (schedules == null || !schedules.Any()) return "TBA";

            var lines = schedules.Select(s => {
                string day = (s.DayOfWeek ?? "") switch
                {
                    "Monday" => "M",
                    "Tuesday" => "T",
                    "Wednesday" => "W",
                    "Thursday" => "TH",
                    "Friday" => "F",
                    "Saturday" => "S",
                    "Sunday" => "SU",
                    _ => s.DayOfWeek ?? ""
                };
                return $"{day} {s.StartTime:hh\\:mm\\:ss}-{s.EndTime:hh\\:mm\\:ss} {(s.SessionType ?? "").ToUpper()}";
            });
            return string.Join(" / ", lines);
        }
    }
}