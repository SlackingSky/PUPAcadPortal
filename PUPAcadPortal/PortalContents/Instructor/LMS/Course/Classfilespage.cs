using PUPAcadPortal.PortalContents.Instructor.LMS.Course;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace PUPAcadPortal
{

    public partial class ClassFilesPage : UserControl
    {
        public event Action OnBack;
        public event Action<CourseActivity> OnOpenActivities;

        private static readonly Color Maroon = Color.FromArgb(128, 0, 0);
        private static readonly Color MaroonDark = Color.FromArgb(100, 0, 0);
        private static readonly Color LightBg = Color.FromArgb(245, 245, 248);

        private readonly CourseActivity _course;
        private readonly List<CourseModule> _modules;



        public ClassFilesPage(CourseActivity course)
        {
            _course = course;
            _modules = SeedModules();
            Build();

            // Defer initial render until the control is fully laid out so that
            // _pnlScroll.ClientSize.Width returns the real value (not 0).
            this.Load += (s, e) => RenderModules();

            // Re-render when the scroll panel resizes so cards always fill the width.
            _pnlScroll.ClientSizeChanged += (s, e) => ResizeModuleCards();
        }

        // ── seed ─────────────────────────────────────────────────────────────
        private static List<CourseModule> SeedModules() => new()
        {
            new CourseModule { Id=1, Title="Module 1 – Introduction & Course Overview",
                Description="Covers course policies, expected outputs, grading criteria, and an overview of the subject matter.",
                Files = new List<ModuleFile>
                {
                    new ModuleFile { Name="Course Syllabus.pdf",    SizeBytes=420_000, Type="PDF" },
                    new ModuleFile { Name="Week 1 Lecture Slides.pptx", SizeBytes=2_100_000, Type="PPTX" },
                }},
            new CourseModule { Id=2, Title="Module 2 – Fundamentals",
                Description="Core concepts and foundational topics required for subsequent modules.",
                Files = new List<ModuleFile>
                {
                    new ModuleFile { Name="Fundamentals Handout.docx", SizeBytes=850_000, Type="DOCX" },
                    new ModuleFile { Name="Practice Exercises.pdf",    SizeBytes=310_000, Type="PDF" },
                }},
            new CourseModule { Id=3, Title="Module 3 – Applied Topics",
                Description="Hands-on application of theoretical concepts through lab exercises and case studies.",
                Files = new List<ModuleFile>()},
        };

        // ── build ─────────────────────────────────────────────────────────────
        private void Build()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = LightBg;

            // ── header ───────────────────────────────────────────────────────
            _pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Maroon,
            };

            var btnBack = new buttonRounded
            {
                Text = "Back",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = MaroonDark,
                ForeColor = Color.White,
                BorderRadius = 10,
                Size = new Size(82, 32),
                Location = new Point(12, 24),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
            };
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Click += (s, e) => OnBack?.Invoke();

            var lblCourse = new Label
            {
                Text = _course.CourseName,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Location = new Point(108, 8),
                Size = new Size(700, 28),
                AutoEllipsis = true,
            };

            string sectionText = $"{_course.CourseCode}  ·  {_course.InstructorName}";
            // If course exposes Section & Schedule you can add them here;
            // we pull them from CourseActivity extended props below.
            string section = string.IsNullOrWhiteSpace(_course.Section) ? "BSIT 2-2" : _course.Section;
            string schedule = string.IsNullOrWhiteSpace(_course.Schedule) ? "T 1:00 PM–2:30 PM | W 8:30 AM–10:00 AM" : _course.Schedule;
            var lblMeta = new Label
            {
                Text = $"{sectionText}   |   {section}   |   {schedule}",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(230, 185, 185),
                AutoSize = false,
                Location = new Point(108, 40),
                Size = new Size(900, 18),
            };

            // "Open Activities" button ──────────────────────────────────────
            var btnActivities = new buttonRounded
            {
                Text = "📋  Activities",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                BackColor = Color.FromArgb(255, 196, 0),
                ForeColor = Color.Black,
                BorderRadius = 14,
                Size = new Size(140, 34),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand,
            };
            btnActivities.FlatAppearance.BorderSize = 0;
            btnActivities.Click += (s, e) => OnOpenActivities?.Invoke(_course);

            // Add module button ─────────────────────────────────────────────
            var btnAddModule = new buttonRounded
            {
                Text = "+ Add Module",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 160, 67),
                ForeColor = Color.White,
                BorderRadius = 14,
                Size = new Size(130, 34),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand,
            };
            btnAddModule.FlatAppearance.BorderSize = 0;
            btnAddModule.Click += BtnAddModule_Click;

            _pnlHeader.Controls.AddRange(new Control[] { btnBack, lblCourse, lblMeta });

            // Reposition right-side buttons on resize
            _pnlHeader.SizeChanged += (s, e) =>
            {
                btnActivities.Location = new Point(_pnlHeader.Width - 150, 24);
                btnAddModule.Location = new Point(_pnlHeader.Width - 292, 24);
            };
            _pnlHeader.Controls.AddRange(new Control[] { btnActivities, btnAddModule });

            // ── scroll area ──────────────────────────────────────────────────
            _pnlScroll = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = LightBg,
                Padding = new Padding(24, 20, 24, 20),
            };

            _flpModules = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = LightBg,
            };

            _pnlScroll.Controls.Add(_flpModules);
            this.Controls.Add(_pnlScroll);
            this.Controls.Add(_pnlHeader);
        }

        // ── render ────────────────────────────────────────────────────────────
        private void RenderModules()
        {
            _flpModules.SuspendLayout();
            _flpModules.Controls.Clear();

            foreach (var mod in _modules)
                _flpModules.Controls.Add(BuildModuleCard(mod));

            _flpModules.ResumeLayout();

            // Apply correct widths immediately after building
            ResizeModuleCards();
        }

        // ── resize all cards to match the current scroll panel width ─────────
        private void ResizeModuleCards()
        {
            int w = Math.Max(700, _pnlScroll.ClientSize.Width - 48);
            foreach (Control ctrl in _flpModules.Controls)
            {
                if (!(ctrl is Panel card) || card.Tag is not CourseModule mod) continue;

                card.Width = w;

                // Resize inner controls that depend on card width
                foreach (Control c in card.Controls)
                {
                    if (c is Panel hdr && hdr.Dock == DockStyle.Top)
                    {
                        // Update title, description and file-count label widths inside header
                        foreach (Control h in hdr.Controls)
                        {
                            if (h is Label lbl)
                            {
                                if (lbl.Text == mod.Title)
                                    lbl.Width = w - 220;
                                else if (lbl.Text == mod.Description)
                                    lbl.Width = w - 220;
                                else if (lbl.Text.StartsWith("📎"))
                                    lbl.Left = w - 210;
                            }
                            else if (h is Button btnExp)
                                btnExp.Left = w - 40;
                        }
                    }
                    else if (c is Panel pnlFiles && c.Visible)
                    {
                        pnlFiles.Width = w;
                        foreach (Control fc in pnlFiles.Controls)
                            fc.Width = w;
                    }
                }

                card.Invalidate();
            }
        }

        private Panel BuildModuleCard(CourseModule mod)
        {
            // Use the scroll panel's current client width; fall back to 700 if not yet laid out
            int w = _pnlScroll.ClientSize.Width > 48
                ? _pnlScroll.ClientSize.Width - 48
                : 700;
            w = Math.Max(700, w);
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

            // ── header row ───────────────────────────────────────────────────
            var hdr = new Panel
            {
                Height = 52,
                BackColor = Color.White,
                Dock = DockStyle.Top,
                Cursor = Cursors.Hand,
            };

            var lblNum = new Label
            {
                Text = mod.Id.ToString(),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Maroon,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(14, 11),
                Size = new Size(30, 30),
            };

            var lblTitle = new Label
            {
                Text = mod.Title,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 20, 25),
                AutoSize = false,
                Location = new Point(54, 10),
                Size = new Size(w - 220, 20),
                AutoEllipsis = true,
            };

            var lblDesc = new Label
            {
                Text = mod.Description,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.Gray,
                AutoSize = false,
                Location = new Point(54, 30),
                Size = new Size(w - 220, 16),
                AutoEllipsis = true,
            };

            var lblFileCount = new Label
            {
                Text = $"📎 {mod.Files.Count} file{(mod.Files.Count == 1 ? "" : "s")}",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(100, 100, 110),
                AutoSize = true,
                Location = new Point(w - 210, 18),
            };

            var btnExpand = new Button
            {
                Text = expanded ? "▲" : "▼",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 100, 110),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(28, 28),
                Location = new Point(w - 40, 12),
                Cursor = Cursors.Hand,
            };
            btnExpand.FlatAppearance.BorderSize = 0;
            hdr.Controls.AddRange(new Control[] { lblNum, lblTitle, lblDesc, lblFileCount, btnExpand });

            // ── file list (collapsible) ───────────────────────────────────────
            var pnlFiles = new Panel
            {
                Width = w,
                BackColor = Color.FromArgb(250, 250, 252),
                Visible = expanded,
            };

            Action refreshFiles = null!;
            refreshFiles = () =>
            {
                pnlFiles.SuspendLayout();
                pnlFiles.Controls.Clear();

                int fy = 10;
                foreach (var f in mod.Files)
                {
                    var fileRow = BuildFileRow(f, mod, w, () => { refreshFiles(); RebuildCard(card, mod); });
                    fileRow.Location = new Point(0, fy);
                    pnlFiles.Controls.Add(fileRow);
                    fy += fileRow.Height + 4;
                }

                // Upload button
                var btnUpload = new buttonRounded
                {
                    Text = "+ Upload File",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    BackColor = Color.FromArgb(55, 138, 221),
                    ForeColor = Color.White,
                    BorderRadius = 8,
                    Size = new Size(120, 28),
                    Location = new Point(14, fy),
                    Cursor = Cursors.Hand,
                };
                btnUpload.FlatAppearance.BorderSize = 0;
                btnUpload.Click += (s, e) =>
                {
                    using var ofd = new OpenFileDialog
                    {
                        Title = "Upload File to Module",
                        Filter = "All Files (*.*)|*.*|PDF|*.pdf|Word|*.docx|PowerPoint|*.pptx|Images|*.png;*.jpg",
                        Multiselect = true,
                    };
                    if (ofd.ShowDialog() != DialogResult.OK) return;
                    foreach (var path in ofd.FileNames)
                    {
                        var fi = new FileInfo(path);
                        mod.Files.Add(new ModuleFile
                        {
                            Name = fi.Name,
                            SizeBytes = fi.Length,
                            Type = fi.Extension.TrimStart('.').ToUpper(),
                        });
                    }
                    refreshFiles();
                    RebuildCard(card, mod);
                };
                pnlFiles.Controls.Add(btnUpload);
                fy += 38;

                pnlFiles.Height = fy + 8;
                pnlFiles.ResumeLayout();
                RecalcCardHeight(card, hdr, pnlFiles);
            };

            // toggle expand
            EventHandler toggleExpand = (s, ev) =>
            {
                mod.IsExpanded = !mod.IsExpanded;
                pnlFiles.Visible = mod.IsExpanded;
                btnExpand.Text = mod.IsExpanded ? "▲" : "▼";
                if (mod.IsExpanded) refreshFiles();
                RecalcCardHeight(card, hdr, pnlFiles);
            };
            hdr.Click += toggleExpand;
            lblTitle.Click += toggleExpand;
            lblNum.Click += toggleExpand;
            btnExpand.Click += toggleExpand;

            card.Controls.Add(pnlFiles);
            card.Controls.Add(hdr);

            if (expanded) refreshFiles();
            RecalcCardHeight(card, hdr, pnlFiles);

            return card;
        }

        private static Panel BuildFileRow(ModuleFile f, CourseModule mod, int cardW, Action onRemove)
        {
            var row = new Panel { Width = cardW, Height = 40, BackColor = Color.Transparent };
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
            row.Controls.Add(new Label
            {
                Text = f.Name,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 35),
                AutoSize = false,
                Location = new Point(52, 8),
                Size = new Size(cardW - 260, 16),
                AutoEllipsis = true,
                BackColor = Color.Transparent,
            });
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

            var btnRemove = new buttonRounded
            {
                Text = "✕",
                Size = new Size(26, 26),
                Location = new Point(right - 26, 7),
                BackColor = Color.FromArgb(195, 55, 55),
                ForeColor = Color.White,
                BorderRadius = 6,
                Font = new Font("Segoe UI", 8F),
                Cursor = Cursors.Hand,
            };
            btnRemove.Click += (s, e) =>
            {
                mod.Files.Remove(f);
                onRemove();
            };
            row.Controls.Add(btnRemove);
            right -= 32;

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
            };
            btnDownload.Click += (s, e) =>
                MessageBox.Show($"Download: {f.Name}", "Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
            row.Controls.Add(btnDownload);

            return row;
        }

        private void BtnAddModule_Click(object sender, EventArgs e)
        {
            using var dlg = new AddModuleDialog(_modules.Count + 1);
            if (dlg.ShowDialog() != DialogResult.OK) return;

            _modules.Add(new CourseModule
            {
                Id = _modules.Count + 1,
                Title = dlg.ModuleTitle,
                Description = string.IsNullOrWhiteSpace(dlg.ModuleDescription)
                    ? "No description yet."
                    : dlg.ModuleDescription,
                Files = dlg.InitialFiles,
            });
            RenderModules();   // RenderModules now calls ResizeModuleCards internally
        }

        private void RebuildCard(Panel card, CourseModule mod)
        {
            // Refresh file-count label
            foreach (Control c in card.Controls)
                if (c is Panel hdrPnl && hdrPnl.Dock == DockStyle.Top)
                    foreach (Control h in hdrPnl.Controls)
                        if (h is Label lbl && lbl.Text.StartsWith("📎"))
                            lbl.Text = $"📎 {mod.Files.Count} file{(mod.Files.Count == 1 ? "" : "s")}";
        }

        private static void RecalcCardHeight(Panel card, Panel hdr, Panel files)
        {
            int h = hdr.Height;
            if (files.Visible) h += files.Height;
            card.Height = h;
        }

        private static string FormatBytes(long b)
        {
            if (b < 1024) return $"{b} B";
            if (b < 1_048_576) return $"{b / 1024.0:F1} KB";
            return $"{b / 1_048_576.0:F1} MB";
        }
    }

    // ── supporting data classes ───────────────────────────────────────────────
    public class CourseModule
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public bool IsExpanded { get; set; } = false;
        public List<ModuleFile> Files { get; set; } = new();
    }

    public class ModuleFile
    {
        public string Name { get; set; } = "";
        public long SizeBytes { get; set; }
        public string Type { get; set; } = "";
    }
}