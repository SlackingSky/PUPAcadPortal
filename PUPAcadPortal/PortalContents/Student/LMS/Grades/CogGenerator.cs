using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfFont = iTextSharp.text.Font;
using PdfImage = iTextSharp.text.Image;

namespace PUPAcadPortal.PortalContents.Student.LMS.Grades
{
    internal static class CogGenerator
    {
        //  Colours 
        private static readonly BaseColor MAROON = new BaseColor(107, 26, 42);
        private static readonly BaseColor DARK_GRAY = new BaseColor(34, 34, 34);
        private static readonly BaseColor MID_GRAY = new BaseColor(85, 85, 85);
        private static readonly BaseColor BORDER_GRAY = new BaseColor(170, 170, 170);

        //  Fonts 
        private static readonly PdfFont F_COUNTRY = new PdfFont(PdfFont.FontFamily.HELVETICA, 8.5f, PdfFont.NORMAL, DARK_GRAY);
        private static readonly PdfFont F_UNIV = new PdfFont(PdfFont.FontFamily.HELVETICA, 11f, PdfFont.BOLD, DARK_GRAY);
        private static readonly PdfFont F_CAMPUS = new PdfFont(PdfFont.FontFamily.HELVETICA, 8.5f, PdfFont.NORMAL, DARK_GRAY);
        private static readonly PdfFont F_TITLE = new PdfFont(PdfFont.FontFamily.TIMES_ROMAN, 12f, PdfFont.BOLD, DARK_GRAY);
        private static readonly PdfFont F_CERT = new PdfFont(PdfFont.FontFamily.TIMES_ROMAN, 9.5f, PdfFont.NORMAL, DARK_GRAY);
        private static readonly PdfFont F_CERT_B = new PdfFont(PdfFont.FontFamily.TIMES_ROMAN, 9.5f, PdfFont.BOLD, DARK_GRAY);
        private static readonly PdfFont F_TH = new PdfFont(PdfFont.FontFamily.TIMES_ROMAN, 9f, PdfFont.BOLD, BaseColor.WHITE);
        private static readonly PdfFont F_TD = new PdfFont(PdfFont.FontFamily.TIMES_ROMAN, 9f, PdfFont.NORMAL, DARK_GRAY);
        private static readonly PdfFont F_GPA_L = new PdfFont(PdfFont.FontFamily.TIMES_ROMAN, 9.5f, PdfFont.BOLD, DARK_GRAY);
        private static readonly PdfFont F_GPA_V = new PdfFont(PdfFont.FontFamily.TIMES_ROMAN, 9.5f, PdfFont.BOLD, DARK_GRAY);
        private static readonly PdfFont F_FNOTE = new PdfFont(PdfFont.FontFamily.TIMES_ROMAN, 8f, PdfFont.ITALIC, MID_GRAY);
        private static readonly PdfFont F_PRIV = new PdfFont(PdfFont.FontFamily.TIMES_ROMAN, 8f, PdfFont.BOLD, MAROON);
        private static readonly PdfFont F_SEM_LBL = new PdfFont(PdfFont.FontFamily.TIMES_ROMAN, 9f, PdfFont.BOLD, DARK_GRAY);
        private static readonly PdfFont F_SEM_VAL = new PdfFont(PdfFont.FontFamily.TIMES_ROMAN, 9f, PdfFont.NORMAL, DARK_GRAY);

        //  Page margins 
        private const float MM = 2.835f;
        private const float LM = 15 * MM;
        private const float RM = 15 * MM;
        private const float TM = 10 * MM;
        private const float BM = 10 * MM;

        //  Grade entry 
        public struct GradeEntry
        {
            public string SubjectCode;
            public string SubjectTitle;
            public int Units;
            public double Equivalent;
            public string Remarks;
        }

        //  PUBLIC ENTRY POINT
        public static void Generate(
            string outputPath,
            string logoImagePath,
            string ayLabel,
            List<GradeEntry> sem1Subjects,
            List<GradeEntry> sem2Subjects)
        {
            using (var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
            {
                var doc = new Document(PageSize.A4, LM, RM, TM, BM);
                var writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                float contentWidth = doc.PageSize.Width - LM - RM;

                PdfImage logo = null;

                string resourcesDir = Path.Combine(
                    Path.GetDirectoryName(logoImagePath) ?? "", "");

                string[] candidates =
                {
                    Path.Combine(resourcesDir, "img(1).png"),
                    Path.Combine(resourcesDir, "img (1).png"),
                    logoImagePath,
                    Path.Combine(resourcesDir, "pup48x48.png"),
                    Path.Combine(resourcesDir, "pup_logo.png"),
                    Path.Combine(resourcesDir, "PUPLogo.png"),
                };

                // Also try embedded resource as fallback
                if (logo == null)
                {
                    try
                    {
                        System.Drawing.Bitmap bmp = null;
                        try { bmp = Properties.Resources.img__1_;} catch { }
                        if (bmp == null) try { bmp = Properties.Resources.pup48x48; } catch { }
                        if (bmp == null) try { bmp = Properties.Resources.img__1_; } catch { }

                        if (bmp != null)
                        {
                            using (var ms = new MemoryStream())
                            {
                                bmp.Save(ms, ImageFormat.Png);
                                logo = PdfImage.GetInstance(ms.ToArray());
                            }
                        }
                    }
                    catch { }
                }

                if (logo == null)
                {
                    string found = candidates.FirstOrDefault(
                        p => !string.IsNullOrEmpty(p) && File.Exists(p));
                    if (found != null)
                        logo = PdfImage.GetInstance(found);
                }

                if (logo != null)
                {
                    logo.ScaleToFit(22 * MM, 22 * MM);
                    logo.Alignment = Element.ALIGN_CENTER;
                    logo.SpacingAfter = 3f;
                    doc.Add(logo);
                }

                //  HEADER 
                doc.Add(CenterPara("REPUBLIC OF THE PHILIPPINES", F_COUNTRY));
                doc.Add(CenterPara("POLYTECHNIC UNIVERSITY OF THE PHILIPPINES", F_UNIV));
                doc.Add(CenterPara("SANTA MARIA CAMPUS", F_CAMPUS));
                doc.Add(Gap(4 * MM));

                AddMaroonDivider(doc, contentWidth);
                doc.Add(Gap(2 * MM));

                //  TITLE 
                var titlePara = new Paragraph("CERTIFICATE OF GRADES", F_TITLE)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingBefore = 3f,
                    SpacingAfter = 3f,
                };
                doc.Add(titlePara);

                doc.Add(Gap(2 * MM));
                AddMaroonDivider(doc, contentWidth);
                doc.Add(Gap(5 * MM));

                //  CERTIFICATION TEXT 
                var certPara = new Paragraph { Alignment = Element.ALIGN_CENTER, Leading = 16 };
                certPara.Add(new Chunk("This is to certify that ", F_CERT));
                certPara.Add(new Chunk("JUAN SANTOS DELA CRUZ", F_CERT_B));
                certPara.Add(new Chunk(", a ", F_CERT));
                certPara.Add(new Chunk("BACHELOR OF SCIENCE IN INFORMATION TECHNOLOGY", F_CERT_B));
                certPara.Add(new Chunk(
                    " student, has completed and passed the following subjects listed below with the corresponding grades.",
                    F_CERT));
                doc.Add(certPara);
                doc.Add(Gap(5 * MM));

                AddSemesterBlock(doc, contentWidth, 1, ayLabel, sem1Subjects);
                AddSemesterBlock(doc, contentWidth, 2, ayLabel, sem2Subjects);

                var fnote = new Paragraph(
                    "This is system-generated, signature is not required.", F_FNOTE)
                {
                    Alignment = Element.ALIGN_RIGHT,
                    SpacingBefore = 4f,
                    SpacingAfter = 2f,
                };
                doc.Add(fnote);

                AddMaroonDivider(doc, contentWidth);
                doc.Add(Gap(2 * MM));

                var priv = new Paragraph { Alignment = Element.ALIGN_CENTER, Leading = 13 };
                priv.Add(new Chunk(
                    "This document contains personal-identifiable information that is subject to Data Privacy.\n",
                    F_PRIV));
                priv.Add(new Chunk(
                    "Please keep this document protected and in a safe place.",
                    F_PRIV));
                doc.Add(priv);

                doc.Close();
            }
        }

        //  MAROON DIVIDER
        private static void AddMaroonDivider(Document doc, float contentWidth)
        {
            var tbl = new PdfPTable(new float[] { contentWidth })
            {
                TotalWidth = contentWidth,
                LockedWidth = true,
                SpacingAfter = 0,
            };
            tbl.AddCell(new PdfPCell(new Phrase(" "))
            {
                BorderWidthTop = 0,
                BorderWidthBottom = 1.5f,
                BorderWidthLeft = 0,
                BorderWidthRight = 0,
                BorderColorBottom = MAROON,
                PaddingBottom = 0,
                PaddingTop = 0,
                MinimumHeight = 1f,
            });
            doc.Add(tbl);
        }

        //  SEMESTER BLOCK
        private static void AddSemesterBlock(
            Document doc,
            float contentWidth,
            int semNum,
            string ayLabel,
            List<GradeEntry> subjects)
        {
            double gwa = ComputeGwa(subjects);
            string semOrd = semNum == 1 ? "1st" : "2nd";

            float[] semColW = { contentWidth * 0.155f, contentWidth * 0.09f, contentWidth * 0.165f, contentWidth * 0.59f };
            var semTable = new PdfPTable(semColW) { TotalWidth = contentWidth, LockedWidth = true, SpacingAfter = 0 };
            semTable.AddCell(SemCell("Semester :", F_SEM_LBL, Element.ALIGN_LEFT, false));
            semTable.AddCell(SemCell(semOrd, F_SEM_VAL, Element.ALIGN_LEFT, false));
            semTable.AddCell(SemCell("School Year :", F_SEM_LBL, Element.ALIGN_LEFT, false));
            semTable.AddCell(SemCell(ayLabel, F_SEM_VAL, Element.ALIGN_LEFT, true));
            doc.Add(semTable);

            float[] gradeColW = { contentWidth * 0.15f, contentWidth * 0.55f, contentWidth * 0.15f, contentWidth * 0.15f };
            var gradeTable = new PdfPTable(gradeColW)
            {
                TotalWidth = contentWidth,
                LockedWidth = true,
                SpacingAfter = 0,
                HeaderRows = 1,
            };

            gradeTable.AddCell(GradeHeaderCell("Code No."));
            gradeTable.AddCell(GradeHeaderCell("Descriptive Title"));
            gradeTable.AddCell(GradeHeaderCell("Credits"));
            gradeTable.AddCell(GradeHeaderCell("Grades"));

            bool shade = false;
            foreach (var s in subjects)
            {
                BaseColor bg = shade ? new BaseColor(247, 247, 247) : BaseColor.WHITE;
                shade = !shade;
                gradeTable.AddCell(GradeCell(s.SubjectCode, F_TD, Element.ALIGN_CENTER, bg));
                gradeTable.AddCell(GradeCell(s.SubjectTitle, F_TD, Element.ALIGN_LEFT, bg));
                gradeTable.AddCell(GradeCell($"{s.Units:F2}", F_TD, Element.ALIGN_CENTER, bg));
                gradeTable.AddCell(GradeCell($"{s.Equivalent:F2}", F_TD, Element.ALIGN_CENTER, bg));
            }
            doc.Add(gradeTable);

            float[] gpaColW = { contentWidth * 0.80f, contentWidth * 0.20f };
            var gpaTable = new PdfPTable(gpaColW)
            {
                TotalWidth = contentWidth,
                LockedWidth = true,
                SpacingAfter = 4 * MM,
            };
            gpaTable.AddCell(GpaCell("Grade Point Average (GPA):", F_GPA_L, Element.ALIGN_LEFT, BaseColor.WHITE));
            gpaTable.AddCell(GpaCell($"{gwa:F2}", F_GPA_V, Element.ALIGN_CENTER, new BaseColor(240, 240, 240)));
            doc.Add(gpaTable);
        }

        //  CELL HELPERS
        private static PdfPCell SemCell(string text, PdfFont font, int align, bool lastCol) =>
            new PdfPCell(new Phrase(text, font))
            {
                HorizontalAlignment = align,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 4,
                PaddingBottom = 4,
                PaddingLeft = 6,
                PaddingRight = 4,
                BorderColor = BORDER_GRAY,
                BorderWidthTop = 1f,
                BorderWidthBottom = 1f,
                BorderWidthLeft = 1f,
                BorderWidthRight = lastCol ? 1f : 0f,
            };

        private static PdfPCell GradeHeaderCell(string text) =>
            new PdfPCell(new Phrase(text, F_TH))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = MAROON,
                PaddingTop = 5,
                PaddingBottom = 5,
                PaddingLeft = 6,
                PaddingRight = 6,
                BorderColor = BORDER_GRAY,
                BorderWidth = 0.5f,
            };

        private static PdfPCell GradeCell(string text, PdfFont font, int align, BaseColor bg) =>
            new PdfPCell(new Phrase(text, font))
            {
                HorizontalAlignment = align,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = bg,
                PaddingTop = 3,
                PaddingBottom = 3,
                PaddingLeft = 6,
                PaddingRight = 6,
                BorderColor = BORDER_GRAY,
                BorderWidth = 0.5f,
            };

        private static PdfPCell GpaCell(string text, PdfFont font, int align, BaseColor bg) =>
            new PdfPCell(new Phrase(text, font))
            {
                HorizontalAlignment = align,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = bg,
                PaddingTop = 4,
                PaddingBottom = 4,
                PaddingLeft = align == Element.ALIGN_LEFT ? 6 : 4,
                PaddingRight = 4,
                BorderColor = BORDER_GRAY,
                BorderWidth = 1f,
            };

        //  UTILITIES
        private static Paragraph CenterPara(string text, PdfFont font) =>
            new Paragraph(text, font) { Alignment = Element.ALIGN_CENTER };

        private static Paragraph Gap(float pts) =>
            new Paragraph(" ") { Leading = pts, SpacingAfter = 0, SpacingBefore = 0 };

        private static double ComputeGwa(List<GradeEntry> subjects)
        {
            double tw = subjects.Where(e => e.Remarks == "PASSED").Sum(e => e.Equivalent * e.Units);
            int tu = subjects.Where(e => e.Remarks == "PASSED").Sum(e => e.Units);
            return tu > 0 ? Math.Round(tw / tu, 2) : 0;
        }
    }
}