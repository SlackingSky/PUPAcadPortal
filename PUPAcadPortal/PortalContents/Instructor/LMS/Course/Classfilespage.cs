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
        private NullModuleDbService nullModuleDbService;

        // ── DB-backed constructor ──────────────────────────────────────────────
        public ClassFilesPage(CourseActivity course, IModuleDbService svc)
        {
            _course = course;
            _svc = svc;

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

        public ClassFilesPage(CourseActivity course, NullModuleDbService nullModuleDbService)
        {
            _course = course;
            this.nullModuleDbService = nullModuleDbService;
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
            _flpModules.SuspendLayout();

            foreach (Control ctrl in _flpModules.Controls)
            {
                if (ctrl is Panel card && card.Tag is CourseModule mod)
                {
                    card.Width = w;
                    card.Invalidate(); // Ensures the border repaints correctly
                }
            }

            _flpModules.ResumeLayout(true);
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
                Height = 56,
                BackColor = Color.White,
                Dock = DockStyle.Top,
                Cursor = Cursors.Default,
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
                Size = new Size(w - 320, 20),
                AutoEllipsis = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right // Let WinForms resize width
            };

            var lblDesc = new Label
            {
                Text = mod.Description,
                Name = "lblDesc",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.Gray,
                AutoSize = false,
                Location = new Point(54, 32),
                Size = new Size(w - 320, 16),
                AutoEllipsis = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right // Let WinForms resize width
            };

            var lblFileCount = new Label
            {
                Text = $"📎 {mod.Files.Count} file{(mod.Files.Count == 1 ? "" : "s")}",
                Name = "lblFileCount",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(100, 100, 110),
                AutoSize = true,
                Location = new Point(w - 220, 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Right // Locks to the right edge
            };

            var btnEdit = new buttonRounded
            {
                Text = "✏ Edit",
                Name = "btnEdit",
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 130, 115),
                ForeColor = Color.White,
                BorderRadius = 6,
                Size = new Size(58, 26),
                Location = new Point(w - 168, 15),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right // Locks to the right edge
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += (s, e) => EditModule(mod, card, lblTitle, lblDesc);

            var btnDelete = new buttonRounded
            {
                Text = "🗑 Del",
                Name = "btnDelete",
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                BackColor = Color.FromArgb(185, 50, 50),
                ForeColor = Color.White,
                BorderRadius = 6,
                Size = new Size(58, 26),
                Location = new Point(w - 104, 15),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right // Locks to the right edge
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += (s, e) => DeleteModule(mod);

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
                Anchor = AnchorStyles.Top | AnchorStyles.Right // Locks to the right edge
            };
            btnExpand.FlatAppearance.BorderSize = 0;

            hdr.Controls.AddRange(new Control[]
                { lblNum, lblTitle, lblDesc, lblFileCount, btnEdit, btnDelete, btnExpand });

            // ── File list panel (collapsible) ────────────────────────────────
            var pnlFiles = new Panel
            {
                Location = new Point(0, 56), // Starts right under the header
                Width = w,
                BackColor = Color.FromArgb(250, 250, 252),
                Visible = expanded,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right // Keeps it synced with card width
            };

            Action refreshFiles = null!;
            refreshFiles = () =>
            {
                pnlFiles.SuspendLayout();
                pnlFiles.Controls.Clear();

                int curW = pnlFiles.Width; // Always pull dynamic current width

                // ── Module info header ─────────────────────────────────────
                var pnlInfo = new Panel
                {
                    Location = new Point(0, 0),
                    Width = curW,
                    Height = 52,
                    BackColor = Color.FromArgb(245, 242, 255),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                };
                pnlInfo.Paint += (s, e) =>
                {
                    using var pen = new Pen(Color.FromArgb(220, 215, 235));
                    e.Graphics.DrawLine(pen, 0, pnlInfo.Height - 1, pnlInfo.Width, pnlInfo.Height - 1);
                    using var acc = new SolidBrush(Maroon);
                    e.Graphics.FillRectangle(acc, 0, 0, 4, pnlInfo.Height);
                };

                pnlInfo.Controls.Add(new Label
                {
                    Text = mod.Title,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(20, 20, 30),
                    Location = new Point(14, 6),
                    Width = curW - 30,
                    Height = 20,
                    AutoEllipsis = true,
                    BackColor = Color.Transparent,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                });
                pnlInfo.Controls.Add(new Label
                {
                    Text = string.IsNullOrWhiteSpace(mod.Description)
                        ? "No description yet."
                        : mod.Description,
                    Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                    ForeColor = Color.FromArgb(100, 100, 115),
                    Location = new Point(14, 28),
                    Width = curW - 30,
                    Height = 16,
                    AutoEllipsis = true,
                    BackColor = Color.Transparent,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                });
                pnlFiles.Controls.Add(pnlInfo);

                int fy = 60;

                foreach (var f in mod.Files)
                {
                    var fileRow = BuildFileRow(f, mod, curW,
                        () => { refreshFiles(); RebuildCard(card, mod); });
                    fileRow.Location = new Point(0, fy);
                    pnlFiles.Controls.Add(fileRow);
                    fy += fileRow.Height + 4;
                }

                // ── Drag-drop upload zone ──────────────────────────────────
                int dropZoneH = 64;
                var pnlDrop = new Panel
                {
                    Location = new Point(14, fy + 6),
                    Width = curW - 28, // Exact width to maintain 14px right margin
                    Height = dropZoneH,
                    BackColor = Color.FromArgb(246, 246, 252),
                    Cursor = Cursors.Hand,
                    AllowDrop = true,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                };
                pnlDrop.Paint += (s, e) =>
                {
                    var g = e.Graphics;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    using var pen = new GraphicsPath();
                    using var dashPen = new Pen(Color.FromArgb(180, 180, 210), 1.5f);
                    dashPen.DashStyle = DashStyle.Dash;
                    g.DrawRectangle(dashPen, 1, 1, pnlDrop.Width - 3, pnlDrop.Height - 3);
                };

                var lblDropIcon = new Label
                {
                    Text = "📎",
                    Font = new Font("Segoe UI", 18F),
                    Location = new Point(16, 10),
                    AutoSize = true,
                    BackColor = Color.Transparent,
                };
                var lblDropText = new Label
                {
                    Text = "Drop files here or click to browse",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(90, 90, 115),
                    Location = new Point(54, 12),
                    AutoSize = true,
                    BackColor = Color.Transparent,
                    Cursor = Cursors.Hand,
                };
                var lblDropSub = new Label
                {
                    Text = "PDF, DOCX, PPTX, PNG, JPG — max 10 MB each",
                    Font = new Font("Segoe UI", 7.5F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    Location = new Point(54, 34),
                    AutoSize = true,
                    BackColor = Color.Transparent,
                    Cursor = Cursors.Hand,
                };
                var btnBrowse = new buttonRounded
                {
                    Text = "Browse",
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    BackColor = Maroon,
                    ForeColor = Color.White,
                    BorderRadius = 6,
                    Size = new Size(70, 26),
                    Location = new Point(pnlDrop.Width - 84, 18),
                    Cursor = Cursors.Hand,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };
                btnBrowse.FlatAppearance.BorderSize = 0;

                var btnSave = new buttonRounded
                {
                    Text = "Save",
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    BackColor = Color.FromArgb(0, 130, 115), // Matches your Edit button green
                    ForeColor = Color.White,
                    BorderRadius = 6,
                    Size = new Size(70, 26),
                    Location = new Point(pnlDrop.Width - 162, 18), // Positioned to the left of Browse
                    Cursor = Cursors.Hand,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };
                btnSave.FlatAppearance.BorderSize = 0;

                btnSave.Click += (s, e) =>
                {
                    if (mod.Files.Count == 0)
                    {
                        MessageBox.Show("No files to save.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    try
                    {

                        MessageBox.Show("Files saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to save files:\n{ex.Message}", "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                // ───────────────────────────────────────────────────────────

                // Add the new button to the panel
                pnlDrop.Controls.AddRange(new Control[]
                    { lblDropIcon, lblDropText, lblDropSub, btnSave, btnBrowse });

                pnlDrop.Controls.AddRange(new Control[]
                    { lblDropIcon, lblDropText, lblDropSub, btnBrowse });

                Action doUpload = () =>
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
                        if (fi.Length > 10_485_760)
                        {
                            MessageBox.Show($"\"{fi.Name}\" exceeds 10 MB and was skipped.",
                                "File Too Large", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }
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

                pnlDrop.Click += (s, e) => doUpload();
                lblDropText.Click += (s, e) => doUpload();
                lblDropSub.Click += (s, e) => doUpload();
                btnBrowse.Click += (s, e) => doUpload();

                pnlDrop.DragEnter += (s, e) =>
                {
                    e.Effect = e.Data?.GetDataPresent(DataFormats.FileDrop) == true
                        ? DragDropEffects.Copy : DragDropEffects.None;
                    pnlDrop.BackColor = Color.FromArgb(235, 235, 250);
                };
                pnlDrop.DragLeave += (s, e) => pnlDrop.BackColor = Color.FromArgb(246, 246, 252);
                pnlDrop.DragDrop += (s, e) =>
                {
                    pnlDrop.BackColor = Color.FromArgb(246, 246, 252);
                    if (e.Data?.GetData(DataFormats.FileDrop) is not string[] paths) return;
                    foreach (var path in paths)
                    {
                        var fi = new FileInfo(path);
                        if (fi.Length > 10_485_760) continue;
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

                pnlFiles.Controls.Add(pnlDrop);
                fy += dropZoneH + 14;

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
            hdr.Click += (s, ev) =>
            {
                var pos = hdr.PointToClient(Cursor.Position);
                var hit = hdr.GetChildAtPoint(pos);
                if (hit == null || hit == lblNum || hit == btnExpand)
                    toggleExpand(s, ev);
            };

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

        // ══════════════════════════════════════════════════════════════════════
        //  Helpers
        // ══════════════════════════════════════════════════════════════════════

        private void RenumberModules()
        {
            for (int i = 0; i < _modules.Count; i++)
                _modules[i].Id = i + 1;
        }

        private static Panel BuildFileRow(ModuleFile f, CourseModule mod, int cardW, Action onRemove)
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
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
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
                Anchor = AnchorStyles.Top | AnchorStyles.Right
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
                Anchor = AnchorStyles.Top | AnchorStyles.Right
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

    // ══════════════════════════════════════════════════════════════════════════
    //  Supporting data classes
    // ══════════════════════════════════════════════════════════════════════════

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