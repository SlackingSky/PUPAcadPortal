using PUPAcadPortal.Models;
using PUPAcadPortal.PortalContents.Instructor.LMS.Course; 
using PUPAcadPortal.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class StudentClassFilesPage : UserControl
    {
        public event Action OnBack;
        public event Action<CourseActivity> OnOpenActivities;

        private static readonly Color Maroon = Color.FromArgb(128, 0, 0);
        private static readonly Color LightBg = Color.FromArgb(245, 245, 248);

        private readonly CourseActivity _course;
        private readonly IModuleDbService _svc;
        private List<CourseModule> _modules = new();
        private NullModuleDbService nullModuleDbService;

        // ── DB-backed constructor ──────────────────────────────────────────────
        public StudentClassFilesPage(CourseActivity course, IModuleDbService svc)
        {
            _course = course;
            _svc = svc;

            InitializeComponent();
            WireDesignerControls();

            this.Load += (s, e) => { LoadModulesFromDb(); RenderModules(); };
            _pnlScroll.ClientSizeChanged += (s, e) => ResizeModuleCards();
        }

        //  Backward-compatible overload (no live DB) 
        public StudentClassFilesPage(CourseActivity course)
            : this(course, new NullModuleDbService())
        {
            _modules = SeedSampleModules();
        }

        public StudentClassFilesPage(CourseActivity course, NullModuleDbService nullModuleDbService)
        {
            _course = course;
            this.nullModuleDbService = nullModuleDbService;
        }

        private void WireDesignerControls()
        {
            // ── Header labels ────────────────────────────────────────────────
            lblCourse.Text = _course.CourseName;

            string section = string.IsNullOrWhiteSpace(_course.Section) ? "TBA" : _course.Section;
            string schedule = string.IsNullOrWhiteSpace(_course.Schedule) ? "" : _course.Schedule;
            string instructor = string.IsNullOrWhiteSpace(_course.InstructorName) ? "" : _course.InstructorName;

            lblMeta.Text = string.IsNullOrWhiteSpace(schedule)
                ? $"{_course.CourseCode}  ·  {instructor}   |   {section}"
                : $"{_course.CourseCode}  ·  {instructor}   |   {section}   |   {schedule}";

            // ── Button events ────────────────────────────────────────────────
            btnBack.Click += (s, e) => OnBack?.Invoke();
            btnActivities.Click += (s, e) => OnOpenActivities?.Invoke(_course);

            // ── Reposition right-side buttons when header resizes ────────────
            _pnlHeader.SizeChanged += (s, e) =>
            {
                btnActivities.Location = new Point(_pnlHeader.Width - 160, 24);
            };
        }

        // ══════════════════════════════════════════════════════════════════════
        //  Data loading
        // ══════════════════════════════════════════════════════════════════════
        private void LoadModulesFromDb()
        {
            try
            {
                var dbModules = _svc.GetModulesForOffering(_course.SubjectOfferingId);
                _modules = dbModules.Select((m, i) =>
                {
                    var files = new List<ModuleFile>();
                    if (!string.IsNullOrWhiteSpace(m.FileUrl))
                    {
                        string fileName = System.IO.Path.GetFileName(m.FileUrl);
                        if (string.IsNullOrWhiteSpace(fileName)) fileName = "file";
                        string ext = System.IO.Path.GetExtension(fileName).TrimStart('.').ToUpper();

                        files.Add(new ModuleFile
                        {
                            Name = fileName,
                            Type = ext,
                            CloudinaryUrl = m.FileUrl,
                            CloudinaryPublicId = CloudinaryService.ExtractPublicIdFromUrl(m.FileUrl),
                        });
                    }

                    return new CourseModule
                    {
                        DbId = m.ModuleId,
                        Id = i + 1,
                        Title = m.Title,
                        Description = m.ModuleDescription ?? "",
                        FileUrl = m.FileUrl ?? "",
                        IsExpanded = false,
                        Files = files,
                    };
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to load modules:\n{ex.Message}",
                    "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _modules = new List<CourseModule>();
            }
        }

        private static List<CourseModule> SeedSampleModules() => new()
        {
            new CourseModule { Id = 1, Title = "Module 1 – Introduction & Course Overview",
                Description = "Covers course policies, expected outputs, grading criteria, and an overview of the subject matter." },
            new CourseModule { Id = 2, Title = "Module 2 – Fundamentals",
                Description = "Core concepts and foundational topics required for subsequent modules." }
        };

        // ══════════════════════════════════════════════════════════════════════
        //  Render / resize
        // ══════════════════════════════════════════════════════════════════════
        private void RenderModules()
        {
            _flpModules.SuspendLayout();
            _flpModules.Controls.Clear();

            if (_modules.Count == 0)
            {
                _flpModules.Controls.Add(new Label
                {
                    Text = "No modules have been posted by your instructor yet.",
                    Font = new Font("Segoe UI", 11F),
                    ForeColor = Color.FromArgb(160, 160, 170),
                    AutoSize = true,
                    Margin = new Padding(10, 20, 0, 0),
                });
            }
            else
            {
                foreach (var mod in _modules)
                    _flpModules.Controls.Add(BuildModuleCard(mod));
            }

            _flpModules.ResumeLayout();
            ResizeModuleCards();
        }

        private void ResizeModuleCards()
        {
            int w = Math.Max(600, _pnlScroll.ClientSize.Width - 80);
            foreach (Control ctrl in _flpModules.Controls)
            {
                if (ctrl is Panel card)
                {
                    card.Width = w;
                }
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  Card builder (Student View - Read Only)
        // ══════════════════════════════════════════════════════════════════════
        private Panel BuildModuleCard(CourseModule mod)
        {
            int w = Math.Max(600, _pnlScroll.ClientSize.Width > 80 ? _pnlScroll.ClientSize.Width - 80 : 600);
            bool expanded = mod.IsExpanded;

            var card = new Panel
            {
                Width = w,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 12),
                Tag = mod,
            };
            card.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(220, 220, 228), 1f);
                e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                using var accent = new SolidBrush(Maroon);
                e.Graphics.FillRectangle(accent, 0, 0, 4, card.Height);
            };

            //  Header row 
            var hdr = new Panel
            {
                Height = 56,
                BackColor = Color.White,
                Dock = DockStyle.Top,
                Cursor = Cursors.Hand,
            };

            var lblNum = new Label
            {
                Text = mod.Id.ToString(),
                Name = "lblNum",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Maroon,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(14, 13),
                Size = new Size(30, 30),
            };

            var lblTitle = new Label
            {
                Text = mod.Title,
                Name = "lblTitle",
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 20, 25),
                AutoSize = false,
                Location = new Point(54, 10),
                Size = new Size(w - 200, 20), // Expanded width since no edit/delete buttons
                AutoEllipsis = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            var lblDesc = new Label
            {
                Text = mod.Description,
                Name = "lblDesc",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.Gray,
                AutoSize = false,
                Location = new Point(54, 32),
                Size = new Size(w - 200, 16),
                AutoEllipsis = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            var lblFileCount = new Label
            {
                Text = $"📎 {mod.Files.Count} file{(mod.Files.Count == 1 ? "" : "s")}",
                Name = "lblFileCount",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(100, 100, 110),
                AutoSize = true,
                Location = new Point(w - 120, 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            var btnExpand = new Button
            {
                Text = expanded ? "▲" : "▼",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 100, 110),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(34, 34),
                Location = new Point(w - 42, 11),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnExpand.FlatAppearance.BorderSize = 0;

            hdr.Controls.AddRange(new Control[]
                { lblNum, lblTitle, lblDesc, lblFileCount, btnExpand });

            //  File list panel (collapsible) 
            var pnlFiles = new Panel
            {
                Width = w,
                Location = new Point(0, 56),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.FromArgb(250, 250, 252),
                Visible = expanded,
            };

            Action refreshFiles = () =>
            {
                pnlFiles.SuspendLayout();
                pnlFiles.Controls.Clear();

                int currentWidth = pnlFiles.Width;
                int fy = 10;

                if (mod.Files.Count == 0)
                {
                    var lblEmpty = new Label
                    {
                        Text = "No files attached to this module.",
                        Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                        ForeColor = Color.Gray,
                        AutoSize = true,
                        Location = new Point(14, fy)
                    };
                    pnlFiles.Controls.Add(lblEmpty);
                    fy += 30;
                }
                else
                {
                    foreach (var f in mod.Files)
                    {
                        var fileRow = BuildFileRow(f, currentWidth);
                        fileRow.Location = new Point(0, fy);
                        pnlFiles.Controls.Add(fileRow);
                        fy += fileRow.Height + 4;
                    }
                }

                pnlFiles.Height = fy + 8;
                pnlFiles.ResumeLayout();
                RecalcCardHeight(card, hdr, pnlFiles);
            };

            EventHandler toggleExpand = (s, ev) =>
            {
                mod.IsExpanded = !mod.IsExpanded;
                pnlFiles.Visible = mod.IsExpanded;
                btnExpand.Text = mod.IsExpanded ? "▲" : "▼";
                if (mod.IsExpanded) refreshFiles();
                RecalcCardHeight(card, hdr, pnlFiles);
            };

            btnExpand.Click += toggleExpand;
            lblNum.Cursor = Cursors.Hand;
            lblNum.Click += toggleExpand;
            lblTitle.Cursor = Cursors.Hand;
            lblTitle.Click += toggleExpand;

            hdr.Click += (s, ev) =>
            {
                var pos = hdr.PointToClient(Cursor.Position);
                var hit = hdr.GetChildAtPoint(pos);
                if (hit == null || hit == lblNum || hit == btnExpand || hit == lblTitle)
                    toggleExpand(s, ev);
            };

            card.Controls.Add(pnlFiles);
            card.Controls.Add(hdr);

            if (expanded) refreshFiles();
            RecalcCardHeight(card, hdr, pnlFiles);

            return card;
        }

        //  Helpers

        private static Panel BuildFileRow(ModuleFile f, int cardW)
        {
            var row = new Panel
            {
                Width = cardW,
                Height = 40,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            row.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(235, 235, 240));
                e.Graphics.DrawLine(pen, 14, row.Height - 1, row.Width - 14, row.Height - 1);
            };

            string icon = f.Type switch
            {
                "PDF" => "📄",
                "DOCX" or "DOC" => "📝",
                "PPTX" or "PPT" => "📊",
                "XLSX" or "XLS" => "📊",
                _ => "📎",
            };

            row.Controls.Add(new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 13F),
                AutoSize = true,
                Location = new Point(20, 9),
                BackColor = Color.Transparent,
            });

            var lblName = new Label
            {
                Text = f.Name,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 35),
                AutoSize = false,
                Location = new Point(52, 8),
                Size = new Size(cardW - 180, 16),
                AutoEllipsis = true,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            row.Controls.Add(lblName);

            row.Controls.Add(new Label
            {
                Text = FormatBytes(f.SizeBytes),
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(52, 24),
                BackColor = Color.Transparent,
            });

            int right = cardW - 10;

            var btnDownload = new buttonRounded
            {
                Text = "↓ Download",
                Size = new Size(90, 26),
                Location = new Point(right - 90, 7),
                BackColor = Color.FromArgb(55, 138, 221),
                ForeColor = Color.White,
                BorderRadius = 6,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            btnDownload.Click += (s, e) =>
            {
                if (!f.IsUploaded)
                {
                    MessageBox.Show("This file is not available for download.",
                        "Not Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                try
                {
                    using var sfd = new SaveFileDialog
                    {
                        Title = "Save File As",
                        FileName = f.Name,
                        Filter = "All Files (*.*)|*.*",
                    };
                    if (sfd.ShowDialog() != DialogResult.OK) return;

                    string tempPath = CloudinaryService.Instance.DownloadToTemp(f.CloudinaryUrl, f.Name);
                    System.IO.File.Copy(tempPath, sfd.FileName, overwrite: true);
                    try { System.IO.File.Delete(tempPath); } catch { }

                    MessageBox.Show($"File saved to:\n{sfd.FileName}", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Download failed:\n{ex.Message}", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            row.Controls.Add(btnDownload);

            return row;
        }

        private static void RecalcCardHeight(Panel card, Panel hdr, Panel files)
        {
            card.Height = hdr.Height + (files.Visible ? files.Height : 0);
        }

        private static string FormatBytes(long b)
        {
            if (b < 1024) return $"{b} B";
            if (b < 1_048_576) return $"{b / 1024.0:F1} KB";
            return $"{b / 1_048_576.0:F1} MB";
        }
    }
}