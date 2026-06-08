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

        //  WireDesignerControls
        //  Replaces the old Build() method. All controls already exist thanks to
        //  InitializeComponent(); we only need to set their content and events.
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
                _modules = dbModules.Select((m, i) =>
                {
                    // Restore any file that was previously uploaded to Cloudinary
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
                        hdr.Width = w;
                        foreach (Control h in hdr.Controls)
                        {
                            if (h is Label lbl)
                            {
                                if (lbl.Name == "lblTitle")
                                    lbl.Width = w - 320;
                                else if (lbl.Name == "lblDesc")
                                    lbl.Width = w - 320;
                                else if (lbl.Name == "lblFileCount")
                                    lbl.Left = w - 220;
                            }
                            else if (h is buttonRounded br)
                            {
                                if (br.Name == "btnEdit")
                                    br.Location = new Point(w - 168, br.Location.Y);
                                else if (br.Name == "btnDelete")
                                    br.Location = new Point(w - 104, br.Location.Y);
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
            };

            var lblFileCount = new Label
            {
                Text = $"📎 {mod.Files.Count} file{(mod.Files.Count == 1 ? "" : "s")}",
                Name = "lblFileCount",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(100, 100, 110),
                AutoSize = true,
                Location = new Point(w - 220, 20),
            };

            // ── Edit button ──────────────────────────────────────────────────
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
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += (s, e) => EditModule(mod, card, lblTitle, lblDesc);

            // ── Delete button ────────────────────────────────────────────────
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
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += (s, e) => DeleteModule(mod);

            // ── Expand / collapse toggle (chevron only) ──────────────────────
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

                // ── Drag-and-drop upload zone ───────────────────────────────
                int dropH = 64;
                var pnlDrop = new Panel
                {
                    Location = new Point(14, fy + 6),
                    Width = w - 28,
                    Height = dropH,
                    BackColor = Color.FromArgb(246, 246, 252),
                    Cursor = Cursors.Hand,
                    AllowDrop = true,
                };
                pnlDrop.Paint += (s, e) =>
                {
                    var g = e.Graphics;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    using var dashPen = new Pen(Color.FromArgb(180, 180, 210), 1.5f);
                    dashPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    g.DrawRectangle(dashPen, 1, 1, pnlDrop.Width - 3, pnlDrop.Height - 3);
                };

                pnlDrop.Controls.Add(new Label
                {
                    Text = "📎",
                    Font = new Font("Segoe UI", 18F),
                    Location = new Point(16, 10),
                    AutoSize = true,
                    BackColor = Color.Transparent,
                });
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
                };
                btnBrowse.FlatAppearance.BorderSize = 0;
                pnlDrop.Controls.AddRange(new Control[]
                    { lblDropText, lblDropSub, btnBrowse });

                // ── Shared upload-to-Cloudinary logic ───────────────────────
                Action doUpload = () =>
                {
                    using var ofd = new OpenFileDialog
                    {
                        Title = "Upload File to Module",
                        Filter = "All Files (*.*)|*.*|PDF|*.pdf|Word|*.docx|PowerPoint|*.pptx|Images|*.png;*.jpg",
                        Multiselect = true,
                    };
                    if (ofd.ShowDialog() != DialogResult.OK) return;

                    var errors = new System.Text.StringBuilder();
                    foreach (var localPath in ofd.FileNames)
                    {
                        var fi = new FileInfo(localPath);
                        if (fi.Length > 10_485_760)
                        {
                            errors.AppendLine($"\"{fi.Name}\" exceeds 10 MB and was skipped.");
                            continue;
                        }

                        // Stage entry immediately so user sees it
                        var mf = new ModuleFile
                        {
                            Name = fi.Name,
                            SizeBytes = fi.Length,
                            Type = fi.Extension.TrimStart('.').ToUpper(),
                            LocalPath = localPath,
                        };
                        mod.Files.Add(mf);
                        refreshFiles();     // show pending state

                        // Upload to Cloudinary
                        try
                        {
                            string folder = $"modules/{(string.IsNullOrEmpty(mod.DbId) ? "new" : mod.DbId)}";
                            string hint = $"{System.IO.Path.GetFileNameWithoutExtension(fi.Name)}";
                            string url = CloudinaryService.Instance.UploadFile(localPath, folder, hint);
                            string pubId = CloudinaryService.ExtractPublicIdFromUrl(url);

                            mf.CloudinaryUrl = url;
                            mf.CloudinaryPublicId = pubId;
                            mf.LocalPath = "";   // clear local path — now on server

                            // Persist the URL back to the DB module record
                            // (stores the last uploaded file URL; multiple files share the module row)
                            if (!string.IsNullOrEmpty(mod.DbId))
                            {
                                try { _svc.UpdateModule(mod.DbId, mod.Title, mod.Description, url); }
                                catch { /* non-critical — URL stored in memory */ }
                            }
                        }
                        catch (Exception ex)
                        {
                            errors.AppendLine($"Upload failed for \"{fi.Name}\":\n{ex.Message}");
                            // Leave mf in list as pending so user can retry
                        }

                        refreshFiles();   // update to show uploaded / error state
                        RebuildCard(card, mod);
                    }

                    if (errors.Length > 0)
                        MessageBox.Show(errors.ToString().TrimEnd(),
                            "Upload Warnings", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                };

                // Wire click and drag-and-drop to doUpload
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
                pnlDrop.DragLeave += (s, e) =>
                    pnlDrop.BackColor = Color.FromArgb(246, 246, 252);
                pnlDrop.DragDrop += (s, e) =>
                {
                    pnlDrop.BackColor = Color.FromArgb(246, 246, 252);
                    if (e.Data?.GetData(DataFormats.FileDrop) is not string[] paths) return;

                    var errors = new System.Text.StringBuilder();
                    foreach (var localPath in paths)
                    {
                        var fi = new FileInfo(localPath);
                        if (fi.Length > 10_485_760)
                        {
                            errors.AppendLine($"\"{fi.Name}\" exceeds 10 MB and was skipped.");
                            continue;
                        }

                        var mf = new ModuleFile
                        {
                            Name = fi.Name,
                            SizeBytes = fi.Length,
                            Type = fi.Extension.TrimStart('.').ToUpper(),
                            LocalPath = localPath,
                        };
                        mod.Files.Add(mf);
                        refreshFiles();

                        try
                        {
                            string folder = $"modules/{(string.IsNullOrEmpty(mod.DbId) ? "new" : mod.DbId)}";
                            string url = CloudinaryService.Instance.UploadFile(localPath, folder,
                                               System.IO.Path.GetFileNameWithoutExtension(fi.Name));
                            mf.CloudinaryUrl = url;
                            mf.CloudinaryPublicId = CloudinaryService.ExtractPublicIdFromUrl(url);
                            mf.LocalPath = "";

                            if (!string.IsNullOrEmpty(mod.DbId))
                            {
                                try { _svc.UpdateModule(mod.DbId, mod.Title, mod.Description, url); }
                                catch { }
                            }
                        }
                        catch (Exception ex)
                        {
                            errors.AppendLine($"Upload failed for \"{fi.Name}\":\n{ex.Message}");
                        }

                        refreshFiles();
                        RebuildCard(card, mod);
                    }

                    if (errors.Length > 0)
                        MessageBox.Show(errors.ToString().TrimEnd(),
                            "Upload Warnings", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                };

                pnlFiles.Controls.Add(pnlDrop);
                fy += dropH + 14;

                pnlFiles.Height = fy + 8;
                pnlFiles.ResumeLayout();
                RecalcCardHeight(card, hdr, pnlFiles);
            };

            // Toggle expand / collapse — only on chevron button and the header panel background
            // (NOT on lblTitle or lblDesc which should not be clickable)
            EventHandler toggleExpand = (s, ev) =>
            {
                mod.IsExpanded = !mod.IsExpanded;
                pnlFiles.Visible = mod.IsExpanded;
                btnExpand.Text = mod.IsExpanded ? "▲" : "▼";
                if (mod.IsExpanded) refreshFiles();
                RecalcCardHeight(card, hdr, pnlFiles);
            };
            btnExpand.Click += toggleExpand;

            // Only the number badge and the blank hdr area trigger expand (not title/desc)
            lblNum.Cursor = Cursors.Hand;
            lblNum.Click += toggleExpand;
            hdr.Click += (s, ev) =>
            {
                // Only fire if click was on the bare panel, not child controls
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
                // Upload any files that were attached in the dialog
                string? firstUrl = null;
                var uploadedFiles = new List<ModuleFile>();
                var uploadErrors = new System.Text.StringBuilder();

                foreach (var mf in dlg.InitialFiles)
                {
                    if (string.IsNullOrEmpty(mf.LocalPath) || !File.Exists(mf.LocalPath))
                    {
                        uploadedFiles.Add(mf);   // already uploaded or no local path
                        continue;
                    }

                    try
                    {
                        // We don't have a DbId yet — use a temp folder; will rename on first edit
                        string folder = $"modules/new_{Guid.NewGuid():N[..8]}";
                        string url = CloudinaryService.Instance.UploadFile(
                            mf.LocalPath, folder,
                            System.IO.Path.GetFileNameWithoutExtension(mf.Name));

                        mf.CloudinaryUrl = url;
                        mf.CloudinaryPublicId = CloudinaryService.ExtractPublicIdFromUrl(url);
                        mf.LocalPath = "";
                        firstUrl ??= url;
                        uploadedFiles.Add(mf);
                    }
                    catch (Exception ex)
                    {
                        uploadErrors.AppendLine($"Upload failed for \"{mf.Name}\":\n{ex.Message}");
                        uploadedFiles.Add(mf);   // keep as pending
                    }
                }

                if (uploadErrors.Length > 0)
                    MessageBox.Show(uploadErrors.ToString().TrimEnd(),
                        "Upload Warnings", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Create the DB module record (optionally with the first file URL)
                var created = _svc.CreateModule(
                    _course.SubjectOfferingId,
                    dlg.ModuleTitle,
                    string.IsNullOrWhiteSpace(dlg.ModuleDescription)
                        ? "No description yet."
                        : dlg.ModuleDescription,
                    firstUrl);

                _modules.Add(new CourseModule
                {
                    DbId = created.ModuleId,
                    Id = _modules.Count + 1,
                    Title = created.Title,
                    Description = created.ModuleDescription ?? "",
                    FileUrl = created.FileUrl ?? "",
                    Files = uploadedFiles,
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

            // Show upload-pending badge if not yet on Cloudinary
            var lblName = new Label
            {
                Text = f.Name,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = f.IsUploaded
                    ? Color.FromArgb(30, 30, 35)
                    : Color.FromArgb(160, 100, 0),
                AutoSize = false,
                Location = new Point(52, 8),
                Size = new Size(cardW - 260, 16),
                AutoEllipsis = true,
                BackColor = Color.Transparent,
            };
            row.Controls.Add(lblName);

            row.Controls.Add(new Label
            {
                Text = f.IsUploaded ? FormatBytes(f.SizeBytes) : $"{FormatBytes(f.SizeBytes)}  ⏳ pending upload",
                Font = new Font("Segoe UI", 8F),
                ForeColor = f.IsUploaded ? Color.Gray : Color.FromArgb(160, 100, 0),
                AutoSize = true,
                Location = new Point(52, 24),
                BackColor = Color.Transparent,
            });

            int right = cardW - 10;

            // ── Delete button — removes from Cloudinary then from list ───────
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
                // Delete from Cloudinary if previously uploaded
                if (f.IsUploaded)
                {
                    try
                    {
                        bool isRaw = CloudinaryService.IsRawType(
                            System.IO.Path.GetExtension(f.Name));
                        CloudinaryService.Instance.DeleteFile(f.CloudinaryPublicId, isRaw);
                    }
                    catch (Exception ex)
                    {
                        // Non-blocking: warn but still remove locally
                        MessageBox.Show(
                            $"Warning: could not delete \"{f.Name}\" from Cloudinary.\n{ex.Message}",
                            "Cloudinary Warning",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                mod.Files.Remove(f);
                onRemove();
            };
            row.Controls.Add(btnRemove);
            right -= 32;

            // ── Download button — fetches from Cloudinary URL ─────────────────
            var btnDownload = new buttonRounded
            {
                Text = "↓ Download",
                Size = new Size(90, 26),
                Location = new Point(right - 90, 7),
                BackColor = f.IsUploaded
                    ? Color.FromArgb(55, 138, 221)
                    : Color.FromArgb(170, 170, 180),
                ForeColor = Color.White,
                BorderRadius = 6,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Cursor = f.IsUploaded ? Cursors.Hand : Cursors.Default,
                Enabled = f.IsUploaded,
            };
            btnDownload.Click += (s, e) =>
            {
                if (!f.IsUploaded)
                {
                    MessageBox.Show("This file has not been uploaded to the server yet.",
                        "Not Uploaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                try
                {
                    // Ask user where to save
                    using var sfd = new SaveFileDialog
                    {
                        Title = "Save File As",
                        FileName = f.Name,
                        Filter = "All Files (*.*)|*.*",
                    };
                    if (sfd.ShowDialog() != DialogResult.OK) return;

                    string tempPath = CloudinaryService.Instance.DownloadToTemp(
                        f.CloudinaryUrl, f.Name);

                    System.IO.File.Copy(tempPath, sfd.FileName, overwrite: true);
                    try { System.IO.File.Delete(tempPath); } catch { }

                    MessageBox.Show($"File saved to:\n{sfd.FileName}",
                        "Download Complete",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        /// <summary>Local filesystem path while the file is staged for upload.</summary>
        public string LocalPath { get; set; } = "";

        /// <summary>Cloudinary public_id after upload (e.g. "modules/MOD-xxx/filename").</summary>
        public string CloudinaryPublicId { get; set; } = "";

        /// <summary>Persistent HTTPS URL returned by Cloudinary. Stored in Module.FileUrl.</summary>
        public string CloudinaryUrl { get; set; } = "";

        /// <summary>True once the file has been uploaded to Cloudinary.</summary>
        public bool IsUploaded => !string.IsNullOrEmpty(CloudinaryUrl);
    }
}