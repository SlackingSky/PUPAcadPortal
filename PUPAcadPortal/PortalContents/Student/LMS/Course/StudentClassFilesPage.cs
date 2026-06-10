using PUPAcadPortal.Models;
using PUPAcadPortal.PortalContents.Instructor.LMS.Course;
using PUPAcadPortal.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PUPAcadPortal.PortalContents.Student.LMS.Course;


namespace PUPAcadPortal
{
    public partial class StudentClassFilesPage : UserControl
    {
        // ── Events 
        public event Action OnBack;

        /// <summary>Fired when the student clicks "View Activities" — goes to Activity List.</summary>
        public event Action<StudentCourse> OnOpenActivities;

        //  State 
        private static readonly Color Maroon = Color.FromArgb(128, 0, 0);
        private static readonly Color LightBg = Color.FromArgb(245, 245, 248);
        private static readonly Color AccentBg = Color.FromArgb(252, 248, 248);

        private readonly StudentCourse _course;
        private readonly IModuleDbService _moduleSvc;
        private readonly int _studentId;
        private List<CourseModule> _modules = new();

        public StudentClassFilesPage(
            StudentCourse course,
            IModuleDbService moduleSvc,
            int studentId,
            Services.IStudentCourseDbService courseSvc)
        {
            _course = course ?? throw new ArgumentNullException(nameof(course));
            _moduleSvc = moduleSvc ?? new NullModuleDbService();
            _studentId = studentId;

            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            WireHeader();

            this.Load += (s, e) => { LoadModulesFromDb(); RenderModules(); };
            _pnlScroll.ClientSizeChanged += (s, e) => ResizeModuleCards();
        }

        /// <summary>WinForms designer no-arg constructor — no DB, empty state.</summary>
        public StudentClassFilesPage()
            : this(
                new StudentCourse { Name = "Sample Course", Code = "SMPL 001" },
                new NullModuleDbService(),
                0,
                new NullStudentCourseDbService())
        { }

        //  Header wiring 
        private void WireHeader()
        {
            lblCourse.Text = _course.Name;

            string schedule = string.IsNullOrWhiteSpace(_course.Schedule) ? "" : _course.Schedule;
            string instr = string.IsNullOrWhiteSpace(_course.Instructor) ? "" : _course.Instructor;

            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(_course.Code)) parts.Add(_course.Code);
            if (!string.IsNullOrWhiteSpace(instr)) parts.Add(instr);
            if (!string.IsNullOrWhiteSpace(schedule)) parts.Add(schedule);
            if (!string.IsNullOrWhiteSpace(_course.Room)) parts.Add(_course.Room);

            lblMeta.Text = string.Join("   |   ", parts);

            btnBack.Click += (s, e) => OnBack?.Invoke();
            btnActivities.Click += (s, e) => OnOpenActivities?.Invoke(_course);

            _pnlHeader.SizeChanged += (s, e) =>
                btnActivities.Location = new Point(_pnlHeader.Width - 160, 24);
        }

        //  Data loading
        private void LoadModulesFromDb()
        {
            try
            {
                var dbModules = _moduleSvc.GetModulesForOffering(_course.SubjectOfferingId);
                _modules = dbModules.Select((m, i) =>
                {
                    var files = new List<ModuleFile>();
                    if (!string.IsNullOrWhiteSpace(m.FileUrl))
                    {
                        string fileName = Path.GetFileName(m.FileUrl);
                        if (string.IsNullOrWhiteSpace(fileName)) fileName = m.Title;
                        string ext = Path.GetExtension(fileName).TrimStart('.').ToUpper();

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
                    $"Failed to load course files:\n{ex.Message}",
                    "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _modules = new List<CourseModule>();
            }
        }

        //  Render
        private void RenderModules()
        {
            _flpModules.SuspendLayout();
            _flpModules.Controls.Clear();

            if (_modules.Count == 0)
                _flpModules.Controls.Add(BuildEmptyState());
            else
                foreach (var mod in _modules)
                    _flpModules.Controls.Add(BuildModuleCard(mod));

            _flpModules.ResumeLayout();
            ResizeModuleCards();
        }

        private void ResizeModuleCards()
        {
            int w = Math.Max(600, _pnlScroll.ClientSize.Width - 80);
            foreach (Control ctrl in _flpModules.Controls)
                if (ctrl is Panel card)
                    card.Width = w;
        }

        //  Empty state 
        private Panel BuildEmptyState()
        {
            int w = Math.Max(600, _pnlScroll.ClientSize.Width > 80
                                        ? _pnlScroll.ClientSize.Width - 80 : 600);
            var pnl = new Panel
            {
                Width = w,
                Height = 200,
                BackColor = Color.FromArgb(252, 252, 255),
                Margin = new Padding(0, 20, 0, 0)
            };
            pnl.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(218, 218, 228), 1.5f);
                e.Graphics.DrawRectangle(pen, 1, 1, pnl.Width - 3, pnl.Height - 3);
            };
            pnl.Controls.Add(new Label
            {
                Text = "📂  No class files posted yet",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(160, 160, 170),
                AutoSize = false,
                Width = w,
                Height = 60,
                TextAlign = ContentAlignment.BottomCenter,
                Location = new Point(0, 50)
            });
            pnl.Controls.Add(new Label
            {
                Text = "Your instructor hasn't uploaded any course materials yet.\nCheck back later.",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(180, 180, 190),
                AutoSize = false,
                Width = w,
                Height = 36,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 118)
            });
            return pnl;
        }

        //  Module card builder  (student view — read-only)
        private Panel BuildModuleCard(CourseModule mod)
        {
            int w = Math.Max(600, _pnlScroll.ClientSize.Width > 80
                                        ? _pnlScroll.ClientSize.Width - 80 : 600);
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
                using var accent = new SolidBrush(Maroon);
                e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                e.Graphics.FillRectangle(accent, 0, 0, 4, card.Height);
            };

            //  Collapsed header row 
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
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 20, 25),
                AutoSize = false,
                Location = new Point(54, 10),
                Size = new Size(w - 200, 20),
                AutoEllipsis = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            };

            var lblDesc = new Label
            {
                Text = mod.Description,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.Gray,
                AutoSize = false,
                Location = new Point(54, 32),
                Size = new Size(w - 200, 16),
                AutoEllipsis = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            };

            int fileCount = mod.Files.Count;
            bool hasFiles = fileCount > 0;
            Color pillBg = hasFiles ? Color.FromArgb(215, 245, 215) : Color.FromArgb(240, 240, 240);
            Color pillFg = hasFiles ? Color.FromArgb(27, 110, 27) : Color.FromArgb(130, 130, 130);

            var lblFileCount = new Label
            {
                Text = hasFiles ? $"📎 {fileCount} file{(fileCount == 1 ? "" : "s")}" : "📂 No files",
                Font = new Font("Segoe UI", 8.5F, hasFiles ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = pillFg,
                BackColor = pillBg,
                AutoSize = true,
                Location = new Point(w - 126, 18),
                Padding = new Padding(6, 3, 6, 3),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
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
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
            };
            btnExpand.FlatAppearance.BorderSize = 0;

            hdr.Controls.AddRange(new Control[] { lblNum, lblTitle, lblDesc, lblFileCount, btnExpand });

            //  Expandable file list 
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

                int fw = pnlFiles.Width;
                int fy = 10;

                if (mod.Files.Count == 0)
                {
                    pnlFiles.Controls.Add(new Label
                    {
                        Text = "No files have been attached to this module.",
                        Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                        ForeColor = Color.Gray,
                        AutoSize = true,
                        Location = new Point(20, fy),
                    });
                    fy += 34;
                }
                else
                {
                    foreach (var f in mod.Files)
                    {
                        var fileRow = BuildFileRow(f, fw);
                        fileRow.Location = new Point(0, fy);
                        pnlFiles.Controls.Add(fileRow);
                        fy += fileRow.Height + 4;
                    }
                }

                pnlFiles.Height = fy + 10;
                pnlFiles.ResumeLayout();
                RecalcCardHeight(card, hdr, pnlFiles);
            };

            EventHandler toggleExpand = (s, ev) =>
            {
                mod.IsExpanded = !mod.IsExpanded;
                pnlFiles.Visible = mod.IsExpanded;
                btnExpand.Text = mod.IsExpanded ? "▲" : "▼";
                hdr.BackColor = mod.IsExpanded ? AccentBg : Color.White;
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

            hdr.MouseEnter += (s, e) => { if (!mod.IsExpanded) hdr.BackColor = Color.FromArgb(252, 248, 248); };
            hdr.MouseLeave += (s, e) => { if (!mod.IsExpanded) hdr.BackColor = Color.White; };

            card.Controls.Add(pnlFiles);
            card.Controls.Add(hdr);

            if (expanded) refreshFiles();
            RecalcCardHeight(card, hdr, pnlFiles);
            return card;
        }

        //  File row  (student view — download only)
        private static Panel BuildFileRow(ModuleFile f, int cardW)
        {
            var row = new Panel
            {
                Width = cardW,
                Height = 44,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
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
                "JPG" or "JPEG"
                    or "PNG"
                    or "GIF" => "🖼",
                "ZIP" or "RAR"
                    or "7Z" => "🗜",
                _ => "📎",
            };

            Color badgeClr = f.Type switch
            {
                "PDF" => Color.FromArgb(220, 50, 50),
                "DOCX" or "DOC" => Color.FromArgb(40, 100, 200),
                "PPTX" or "PPT" => Color.FromArgb(200, 80, 20),
                "XLSX" or "XLS" => Color.FromArgb(30, 140, 60),
                _ => Color.FromArgb(100, 100, 120),
            };

            // Icon panel
            var pnlIcon = new Panel
            {
                Size = new Size(44, 44),
                Location = new Point(14, 0),
                BackColor = Color.FromArgb(245, 245, 250),
            };
            pnlIcon.Controls.Add(new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 14F),
                Location = new Point(4, 4),
                Size = new Size(36, 36),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent,
            });
            row.Controls.Add(pnlIcon);

            // File name
            row.Controls.Add(new Label
            {
                Text = f.Name,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 35),
                AutoSize = false,
                Location = new Point(66, 6),
                Size = new Size(cardW - 210, 18),
                AutoEllipsis = true,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            });

            // File type badge
            var meta = new Label
            {
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = badgeClr,
                Text = string.IsNullOrEmpty(f.Type) ? "FILE" : f.Type,
                AutoSize = true,
                Location = new Point(66, 26),
                Padding = new Padding(4, 1, 4, 1),
            };
            row.Controls.Add(meta);

            if (f.SizeBytes > 0)
            {
                row.Controls.Add(new Label
                {
                    Text = "  " + FormatBytes(f.SizeBytes),
                    Font = new Font("Segoe UI", 7.5F),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Location = new Point(66 + meta.PreferredWidth + 2, 26),
                    BackColor = Color.Transparent,
                });
            }

            // Download button
            var btnDownload = new buttonRounded
            {
                Text = f.IsUploaded ? "↓  Download" : "Not Available",
                Size = new Size(100, 28),
                Location = new Point(cardW - 114, 8),
                BackColor = f.IsUploaded ? Color.FromArgb(40, 110, 200) : Color.FromArgb(180, 180, 180),
                ForeColor = Color.White,
                BorderRadius = 6,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Cursor = f.IsUploaded ? Cursors.Hand : Cursors.Default,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Enabled = f.IsUploaded,
            };
            btnDownload.FlatAppearance.BorderSize = 0;

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
                    File.Copy(tempPath, sfd.FileName, overwrite: true);
                    try { File.Delete(tempPath); } catch { }

                    MessageBox.Show($"File saved to:\n{sfd.FileName}",
                        "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Download failed:\n{ex.Message}",
                        "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            row.Controls.Add(btnDownload);

            return row;
        }

        private static void RecalcCardHeight(Panel card, Panel hdr, Panel files)
            => card.Height = hdr.Height + (files.Visible ? files.Height : 0);

        private static string FormatBytes(long b)
        {
            if (b < 1_024) return $"{b} B";
            if (b < 1_048_576) return $"{b / 1_024.0:F1} KB";
            return $"{b / 1_048_576.0:F1} MB";
        }
    }
}