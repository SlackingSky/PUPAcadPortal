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
        private readonly IModuleDbService _svc;
        private List<CourseModule> _modules = new();

        // ── DB-backed constructor ──────────────────────────────────────────────
        public ClassFilesPage(CourseActivity course, IModuleDbService svc)
        {
            _course = course;
            _svc = svc ?? new NullModuleDbService();

            InitializeComponent();   // designer builds _pnlHeader, _pnlScroll, _flpModules
            WireDesignerControls();  // populate labels + hook events

            this.Load += (s, e) => { LoadModulesFromDb(); RenderModules(); };
            _pnlScroll.ClientSizeChanged += (s, e) => ResizeModuleCards();
        }

        // ── Backward-compatible overload (no live DB) ──────────────────────────
        public ClassFilesPage(CourseActivity course)
            : this(course, new NullModuleDbService())
        {
            _modules = SeedSampleModules();
        }

        // ══════════════════════════════════════════════════════════════════════
        //  WireDesignerControls
        //  Replaces the old Build() method. All controls already exist thanks to
        //  InitializeComponent(); we only need to set their content and events.
        // ══════════════════════════════════════════════════════════════════════
        private void WireDesignerControls()
        {
            // ── Header labels ────────────────────────────────────────────────
            lblCourse.Text = _course.CourseName;

            string section = string.IsNullOrWhiteSpace(_course.Section)
                                ? "TBA" : _course.Section;
            string schedule = string.IsNullOrWhiteSpace(_course.Schedule)
                                ? "" : _course.Schedule;
            string instructor = string.IsNullOrWhiteSpace(_course.InstructorName)
                                ? "" : _course.InstructorName;

            lblMeta.Text = string.IsNullOrWhiteSpace(schedule)
                ? $"{_course.CourseCode}  ·  {instructor}   |   {section}"
                : $"{_course.CourseCode}  ·  {instructor}   |   {section}   |   {schedule}";

            // ── Button events ────────────────────────────────────────────────
            btnBack.Click += (s, e) => OnBack?.Invoke();
            btnActivities.Click += (s, e) => OnOpenActivities?.Invoke(_course);
            btnAddModule.Click += BtnAddModule_Click;

            // ── Reposition right-side buttons when header resizes ────────────
            _pnlHeader.SizeChanged += (s, e) =>
            {
                btnActivities.Location = new Point(_pnlHeader.Width - 150, 24);
                btnAddModule.Location = new Point(_pnlHeader.Width - 292, 24);
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
                _modules = dbModules.Select((m, i) => new CourseModule
                {
                    DbId = m.ModuleId,
                    Id = i + 1,
                    Title = m.Title,
                    Description = m.ModuleDescription ?? "",
                    FileUrl = m.FileUrl ?? "",
                    IsExpanded = false,
                    Files = new List<ModuleFile>(),
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

        // ── Fallback seed data (demo / design-time) ────────────────────────────
        private static List<CourseModule> SeedSampleModules() => new()
        {
            new CourseModule { Id = 1, Title = "Module 1 – Introduction & Course Overview",
                Description = "Covers course policies, expected outputs, grading criteria, and an overview of the subject matter." },
            new CourseModule { Id = 2, Title = "Module 2 – Fundamentals",
                Description = "Core concepts and foundational topics required for subsequent modules." },
            new CourseModule { Id = 3, Title = "Module 3 – Applied Topics",
                Description = "Hands-on application of theoretical concepts through lab exercises and case studies." },
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
                    Text = "No modules yet. Click \"+ Add Module\" to create one.",
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
            int w = Math.Max(700, _pnlScroll.ClientSize.Width - 48);
            foreach (Control ctrl in _flpModules.Controls)
            {
                if (ctrl is not Panel card || card.Tag is not CourseModule mod) continue;

                card.Width = w;

                foreach (Control c in card.Controls)
                {
                    if (c is Panel hdr && hdr.Dock == DockStyle.Top)
                    {
                        foreach (Control h in hdr.Controls)
                        {
                            if (h is Label lbl)
                            {
                                if (lbl.Text == mod.Title)
                                    lbl.Width = w - 300;
                                else if (lbl.Text == mod.Description)
                                    lbl.Width = w - 300;
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

        // ══════════════════════════════════════════════════════════════════════
        //  Card builder — Edit & Delete buttons included
        // ══════════════════════════════════════════════════════════════════════
        private Panel BuildModuleCard(CourseModule mod)
        {
            int w = Math.Max(700, _pnlScroll.ClientSize.Width > 48 ? _pnlScroll.ClientSize.Width - 48 : 700);
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

            // ── Header row ─────────────────────────────────────────────────
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
                Size = new Size(w - 300, 20),
                AutoEllipsis = true,
            };

            var lblDesc = new Label
            {
                Text = mod.Description,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.Gray,
                AutoSize = false,
                Location = new Point(54, 30),
                Size = new Size(w - 300, 16),
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

            // ── Edit button ──────────────────────────────────────────────────
            var btnEdit = new buttonRounded
            {
                Text = "✏ Edit",
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 130, 115),
                ForeColor = Color.White,
                BorderRadius = 6,
                Size = new Size(58, 24),
                Location = new Point(w - 160, 14),
                Cursor = Cursors.Hand,
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += (s, e) => EditModule(mod, card, lblTitle, lblDesc);

            // ── Delete button ────────────────────────────────────────────────
            var btnDelete = new buttonRounded
            {
                Text = "🗑 Delete",
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                BackColor = Color.FromArgb(185, 50, 50),
                ForeColor = Color.White,
                BorderRadius = 6,
                Size = new Size(68, 24),
                Location = new Point(w - 96, 14),
                Cursor = Cursors.Hand,
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += (s, e) => DeleteModule(mod);

            // ── Expand / collapse toggle ─────────────────────────────────────
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

            hdr.Controls.AddRange(new Control[]
                { lblNum, lblTitle, lblDesc, lblFileCount, btnEdit, btnDelete, btnExpand });

            // ── File list panel (collapsible) ────────────────────────────────
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
                    var fileRow = BuildFileRow(f, mod, w,
                        () => { refreshFiles(); RebuildCard(card, mod); });
                    fileRow.Location = new Point(0, fy);
                    pnlFiles.Controls.Add(fileRow);
                    fy += fileRow.Height + 4;
                }

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

            // Toggle expand / collapse
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

        // ══════════════════════════════════════════════════════════════════════
        //  CRUD handlers
        // ══════════════════════════════════════════════════════════════════════

        // ── Add Module ──────────────────────────────────────────────────────
        private void BtnAddModule_Click(object sender, EventArgs e)
        {
            using var dlg = new AddModuleDialog(_modules.Count + 1);
            if (dlg.ShowDialog() != DialogResult.OK) return;

            try
            {
                var created = _svc.CreateModule(
                    _course.SubjectOfferingId,
                    dlg.ModuleTitle,
                    string.IsNullOrWhiteSpace(dlg.ModuleDescription)
                        ? "No description yet."
                        : dlg.ModuleDescription);

                _modules.Add(new CourseModule
                {
                    DbId = created.ModuleId,
                    Id = _modules.Count + 1,
                    Title = created.Title,
                    Description = created.ModuleDescription ?? "",
                    FileUrl = created.FileUrl ?? "",
                    Files = dlg.InitialFiles,
                });

                RenderModules();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save module:\n{ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Edit Module ─────────────────────────────────────────────────────
        private void EditModule(CourseModule mod, Panel card, Label lblTitle, Label lblDesc)
        {
            using var dlg = new AddModuleDialog(mod.Id, mod.Title, mod.Description);
            if (dlg.ShowDialog() != DialogResult.OK) return;

            try
            {
                if (!string.IsNullOrEmpty(mod.DbId))
                    _svc.UpdateModule(mod.DbId, dlg.ModuleTitle, dlg.ModuleDescription, mod.FileUrl);

                mod.Title = dlg.ModuleTitle;
                mod.Description = string.IsNullOrWhiteSpace(dlg.ModuleDescription)
                                    ? "No description yet."
                                    : dlg.ModuleDescription;

                lblTitle.Text = mod.Title;
                lblDesc.Text = mod.Description;
                card.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update module:\n{ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Delete Module ───────────────────────────────────────────────────
        private void DeleteModule(CourseModule mod)
        {
            var res = MessageBox.Show(
                $"Delete \"{mod.Title}\"?\n\nActivities linked to this module will become unlinked (not deleted).",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (res != DialogResult.Yes) return;

            try
            {
                if (!string.IsNullOrEmpty(mod.DbId))
                    _svc.DeleteModule(mod.DbId);

                _modules.Remove(mod);
                RenumberModules();
                RenderModules();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete module:\n{ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //  Helpers

        private void RenumberModules()
        {
            for (int i = 0; i < _modules.Count; i++)
                _modules[i].Id = i + 1;
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
            btnRemove.Click += (s, e) => { mod.Files.Remove(f); onRemove(); };
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
                MessageBox.Show($"Download: {f.Name}", "Download",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            row.Controls.Add(btnDownload);

            return row;
        }

        private void RebuildCard(Panel card, CourseModule mod)
        {
            foreach (Control c in card.Controls)
                if (c is Panel hdrPnl && hdrPnl.Dock == DockStyle.Top)
                    foreach (Control h in hdrPnl.Controls)
                        if (h is Label lbl && lbl.Text.StartsWith("📎"))
                            lbl.Text = $"📎 {mod.Files.Count} file{(mod.Files.Count == 1 ? "" : "s")}";
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

    //  Supporting data classes

    public class CourseModule
    {
        /// <summary>PK from the DB. Empty when not yet persisted.</summary>
        public string DbId { get; set; } = string.Empty;

        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string FileUrl { get; set; } = "";
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