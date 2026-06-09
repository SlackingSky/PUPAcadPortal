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
        private static readonly BaseColor MAROON = new BaseColor(107, 26, 42);
        private static readonly BaseColor DARK_GRAY = new BaseColor(34, 34, 34);
        private static readonly BaseColor MID_GRAY = new BaseColor(85, 85, 85);
        private static readonly BaseColor BORDER_GRAY = new BaseColor(170, 170, 170);

        // ── Cambria font (lazy-loaded, cached) ────────────────────────────────
        private static BaseFont _cambriaBase;
        private static BaseFont CambriaBase
        {
            get
            {
                if (_cambriaBase != null) return _cambriaBase;

                string[] candidates =
                {
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "cambria.ttc,0"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "CAMBRIA.TTC,0"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "cambria.ttf"),
                };

                foreach (var path in candidates)
                {
                    string filePath = path.Contains(",") ? path.Split(',')[0] : path;
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            _cambriaBase = BaseFont.CreateFont(path, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                            return _cambriaBase;
                        }
                        catch { /* try next */ }
                    }
                }

                _cambriaBase = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                return _cambriaBase;
            }
        }

        private static PdfFont Cambria(float size, int style, BaseColor color) =>
            new PdfFont(CambriaBase, size, style, color);

        // ── Font definitions ──────────────────────────────────────────────────
        private static PdfFont F_COUNTRY => Cambria(8.5f, PdfFont.NORMAL, DARK_GRAY);
        private static PdfFont F_UNIV => Cambria(11f, PdfFont.BOLD, DARK_GRAY);
        private static PdfFont F_CAMPUS => Cambria(8.5f, PdfFont.NORMAL, DARK_GRAY);
        private static PdfFont F_TITLE => Cambria(12f, PdfFont.BOLD, DARK_GRAY);
        private static PdfFont F_CERT => Cambria(9.5f, PdfFont.NORMAL, DARK_GRAY);
        private static PdfFont F_CERT_B => Cambria(9.5f, PdfFont.BOLD, DARK_GRAY);
        private static PdfFont F_TH => Cambria(9f, PdfFont.BOLD, BaseColor.WHITE);
        private static PdfFont F_TD => Cambria(9f, PdfFont.NORMAL, DARK_GRAY);
        private static PdfFont F_GPA_L => Cambria(9.5f, PdfFont.BOLD, DARK_GRAY);
        private static PdfFont F_GPA_V => Cambria(9.5f, PdfFont.BOLD, DARK_GRAY);
        private static PdfFont F_FNOTE => Cambria(8f, PdfFont.ITALIC, MID_GRAY);
        private static PdfFont F_PRIV => Cambria(8f, PdfFont.BOLD, MAROON);
        private static PdfFont F_SEM_LBL => Cambria(9f, PdfFont.BOLD, DARK_GRAY);
        private static PdfFont F_SEM_VAL => Cambria(9f, PdfFont.NORMAL, DARK_GRAY);

        private const float MM = 2.835f;
        private const float LM = 15 * MM;
        private const float RM = 15 * MM;
        private const float TM = 10 * MM;
        private const float BM = 10 * MM;

        public struct GradeEntry
        {
            public string SubjectCode;
            public string SubjectTitle;
            public int Units;
            public double Equivalent;
            public string Remarks;
            public string EnrollmentType;   // "Regular" | "Irregular"
        }

        // ── FIX 1: studentName and programName are now parameters instead of hardcoded ──
        /// <summary>
        /// Generates a Certificate of Grades PDF.
        /// </summary>
        /// <param name="outputPath">Destination file path.</param>
        /// <param name="logoImagePath">Hint path for the university logo.</param>
        /// <param name="studentName">Full name of the student (UPPER CASE).</param>
        /// <param name="programName">Degree program (UPPER CASE).</param>
        /// <param name="ayLabel">Academic year label, e.g. "2025 – 2026".</param>
        /// <param name="sem1Subjects">First-semester grade entries.</param>
        /// <param name="sem2Subjects">Second-semester grade entries.</param>
        public static void Generate(
            string outputPath,
            string logoImagePath,
            string studentName,
            string programName,
            string ayLabel,
            List<GradeEntry> sem1Subjects,
            List<GradeEntry> sem2Subjects)
        {
            using var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
            var doc = new Document(PageSize.A4, LM, RM, TM, BM);
            var writer = PdfWriter.GetInstance(doc, fs);
            doc.Open();

            float contentWidth = doc.PageSize.Width - LM - RM;

            // ── Logo ──────────────────────────────────────────────────
            PdfImage logo = TryLoadLogo(logoImagePath);
            if (logo != null)
            {
                logo.ScaleToFit(22 * MM, 22 * MM);
                logo.Alignment = Element.ALIGN_CENTER;
                logo.SpacingAfter = 3f;
                doc.Add(logo);
            }

            // ── Header ────────────────────────────────────────────────
            doc.Add(CenterPara("REPUBLIC OF THE PHILIPPINES", F_COUNTRY));
            doc.Add(CenterPara("POLYTECHNIC UNIVERSITY OF THE PHILIPPINES", F_UNIV));
            doc.Add(CenterPara("SANTA MARIA CAMPUS", F_CAMPUS));
            doc.Add(Gap(4 * MM));

            AddMaroonDivider(doc, contentWidth);
            doc.Add(Gap(2 * MM));

            // ── Title ─────────────────────────────────────────────────
            doc.Add(new Paragraph("CERTIFICATE OF GRADES", F_TITLE)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingBefore = 3f,
                SpacingAfter = 3f
            });

            doc.Add(Gap(2 * MM));
            AddMaroonDivider(doc, contentWidth);
            doc.Add(Gap(5 * MM));

            // ── Certification text (uses injected student name & program) ──
            // FIX 1: uses studentName and programName parameters — no longer hardcoded
            var certPara = new Paragraph { Alignment = Element.ALIGN_CENTER, Leading = 16 };
            certPara.Add(new Chunk("This is to certify that ", F_CERT));
            certPara.Add(new Chunk(studentName.ToUpperInvariant(), F_CERT_B));
            certPara.Add(new Chunk(", a ", F_CERT));
            certPara.Add(new Chunk(programName.ToUpperInvariant(), F_CERT_B));
            certPara.Add(new Chunk(
                " student, has completed and passed the following subjects listed below " +
                "with the corresponding grades.", F_CERT));
            doc.Add(certPara);
            doc.Add(Gap(5 * MM));

            // ── Semester blocks ───────────────────────────────────────
            AddSemesterBlock(doc, contentWidth, 1, ayLabel, sem1Subjects);
            AddSemesterBlock(doc, contentWidth, 2, ayLabel, sem2Subjects);

            // ── Footer ────────────────────────────────────────────────
            doc.Add(new Paragraph(" ", F_FNOTE)
            {
                Alignment = Element.ALIGN_RIGHT,
                SpacingBefore = 4f,
                SpacingAfter = 2f
            });

            AddMaroonDivider(doc, contentWidth);
            doc.Add(Gap(2 * MM));

            var priv = new Paragraph { Alignment = Element.ALIGN_CENTER, Leading = 13 };
            priv.Add(new Chunk(
                "This certificate is not valid without an official dry seal or certified true copy.\n",
                F_PRIV));
            doc.Add(priv);

            doc.Close();
        }

        // ── Logo loader ───────────────────────────────────────────────────────
        private static PdfImage TryLoadLogo(string hint)
        {
            // 1. Try embedded resource
            try
            {
                System.Drawing.Bitmap bmp = null;
                try { bmp = Properties.Resources.img__1_; } catch { }
                if (bmp == null) try { bmp = Properties.Resources.pup48x48; } catch { }

                if (bmp != null)
                {
                    using var ms = new MemoryStream();
                    bmp.Save(ms, ImageFormat.Png);
                    return PdfImage.GetInstance(ms.ToArray());
                }
            }
            catch { }

            // 2. Try file-system candidates
            string dir = string.IsNullOrEmpty(hint) ? "" : Path.GetDirectoryName(hint) ?? "";
            string[] paths =
            {
                hint,
                Path.Combine(dir, "img(1).png"),
                Path.Combine(dir, "pup48x48.png"),
                Path.Combine(dir, "pup_logo.png"),
                Path.Combine(dir, "PUPLogo.png"),
            };

            foreach (var p in paths)
                if (!string.IsNullOrEmpty(p) && File.Exists(p))
                    try { return PdfImage.GetInstance(p); } catch { }

            return null;
        }

        // ── Divider ───────────────────────────────────────────────────────────
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

        // ── Semester block ────────────────────────────────────────────────────
        private static void AddSemesterBlock(
            Document doc,
            float contentWidth,
            int semNum,
            string ayLabel,
            List<GradeEntry> subjects)
        {
            double gwa = ComputeGwa(subjects);
            string semOrd = semNum == 1 ? "1st" : "2nd";

            float[] semColW = {
                contentWidth * 0.14f,
                contentWidth * 0.10f,
                contentWidth * 0.16f,
                contentWidth * 0.60f,
            };
            var semTable = new PdfPTable(semColW)
            {
                TotalWidth = contentWidth,
                LockedWidth = true,
                SpacingAfter = 0,
            };
            semTable.AddCell(SemCell("Semester :", F_SEM_LBL, Element.ALIGN_LEFT, false));
            semTable.AddCell(SemCell(semOrd, F_SEM_VAL, Element.ALIGN_LEFT, false));
            semTable.AddCell(SemCell("School Year :", F_SEM_LBL, Element.ALIGN_LEFT, false));
            semTable.AddCell(SemCell(ayLabel, F_SEM_VAL, Element.ALIGN_LEFT, true));
            doc.Add(semTable);

            float[] gradeColW = {
                contentWidth * 0.16f,
                contentWidth * 0.58f,
                contentWidth * 0.13f,
                contentWidth * 0.13f,
            };
            var gradeTable = new PdfPTable(gradeColW)
            {
                TotalWidth = contentWidth,
                LockedWidth = true,
                SpacingAfter = 0,
                HeaderRows = 1,
            };

            gradeTable.AddCell(GradeHeaderCell("Code No.", Element.ALIGN_CENTER));
            gradeTable.AddCell(GradeHeaderCell("Descriptive Title", Element.ALIGN_LEFT));
            gradeTable.AddCell(GradeHeaderCell("Credits", Element.ALIGN_CENTER));
            gradeTable.AddCell(GradeHeaderCell("Grades", Element.ALIGN_CENTER));

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

            // GPA row
            float[] gpaColW = { contentWidth * 0.87f, contentWidth * 0.13f };
            var gpaTable = new PdfPTable(gpaColW)
            {
                TotalWidth = contentWidth,
                LockedWidth = true,
                SpacingAfter = 5 * MM,
            };
            gpaTable.AddCell(GpaCell("Grade Point Average (GPA):",
                                     F_GPA_L, Element.ALIGN_LEFT, BaseColor.WHITE));
            gpaTable.AddCell(GpaCell($"{gwa:F2}",
                                     F_GPA_V, Element.ALIGN_CENTER, new BaseColor(240, 240, 240)));
            doc.Add(gpaTable);
        }

        // ── Cell helpers ──────────────────────────────────────────────────────
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

        private static PdfPCell GradeHeaderCell(string text, int align = Element.ALIGN_CENTER) =>
            new PdfPCell(new Phrase(text, F_TH))
            {
                HorizontalAlignment = align,
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

        // ── Utilities ─────────────────────────────────────────────────────────
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