// ============================================================
//  AnnouncementStubs.cs  (v4 – with notifications + attachments)
//
//  Lives in the AdminPortal branch only.
//  Delete this file when the two branches are merged.
//
//  Changes from v3:
//   • Categories updated: General, Academic, Events, Schedule,
//     Examination, Emergency  (6 named + "All")
//   • CreateAnnouncement: "Send Notification" checkbox (Students / Instructors)
//   • CreateAnnouncement: attachment picker (PDF / Image)
//   • Announcement model: NotifyStudents, NotifyInstructors, AttachmentPath
//   • ViewAnnouncement: shows attachment link + notification targets
// ============================================================

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    // ----------------------------------------------------------
    //  AnnouncementData  (posted / edited data transfer object)
    // ----------------------------------------------------------
    public class AnnouncementDataAdmin
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime PostDate { get; set; } = DateTime.Now;
        public bool IsUrgent { get; set; }
        public bool IsPinned { get; set; }
        public bool NotifyStudents { get; set; }        // NEW
        public bool NotifyInstructors { get; set; }        // NEW
        public string AttachmentPath { get; set; } = string.Empty;   // NEW – local path chosen by admin
        public List<string> Courses { get; set; } = new List<string>();
    }

    // ----------------------------------------------------------
    //  Announcement  (mirrors InstructorPortal.Announcement)
    // ----------------------------------------------------------
    public class AdminAnnouncement
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = "General";
        public string Status { get; set; } = "active";
        public string InstructorName { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public bool IsPinned { get; set; }
        public bool IsUrgent { get; set; }
        public int ViewedCount { get; set; }
        public int TotalStudents { get; set; } = 40;
        public string? AttachedFile { get; set; }   // file name / path stored after upload
        public bool NotifyStudents { get; set; }   // NEW
        public bool NotifyInstructors { get; set; }   // NEW
    }

    // ----------------------------------------------------------
    //  CreateAnnouncement  (functional stub)
    // ----------------------------------------------------------
    public partial class CreateAnnouncementAdmin : UserControl
    {
        public event EventHandler<AnnouncementDataAdmin>? AnnouncementPosted;
        public event EventHandler? CloseRequested;

        // ── Categories (single source of truth) ─────────────────
        public static readonly string[] Categories =
            { "General", "Academic", "Events", "Emergency", "Enrollment" };

        // ── Colors ───────────────────────────────────────────────
        private static readonly Color Maroon = Color.FromArgb(139, 0, 0);
        private static readonly Color PlaceholderGray = Color.FromArgb(160, 160, 160);
        private static readonly Color EmergencyRed = Color.FromArgb(200, 30, 30);

        // ── Controls ─────────────────────────────────────────────
        private TextBox _txtTitle;
        private TextBox _txtDesc;
        private ComboBox _cmbCat;
        private CheckBox _chkUrgent;
        private CheckBox _chkPinned;
        private CheckBox _chkNotifyStudents;
        private CheckBox _chkNotifyInstructors;
        private Button _btnPost;
        private Button _btnCancel;

        // attachment controls
        private Label _lblAttachName;
        private Button _btnBrowse;
        private Button _btnClearAttach;
        private string _attachmentPath = string.Empty;

        public CreateAnnouncementAdmin()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            BackColor = Color.White;
            Size = new Size(720, 560);   // taller to accommodate new sections

            // ── Header ─────────────────────────────────────────────
            var hdr = new Panel { BackColor = Maroon, Dock = DockStyle.Top, Height = 50 };
            var hdrLbl = new Label
            {
                Text = "Create Announcement",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 13f),
                AutoSize = true,
                Location = new Point(50, 13)
            };
            var btnX = new Button
            {
                Text = "×",
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                Size = new Size(34, 34),
                Location = new Point(678, 8)
            };
            btnX.FlatAppearance.BorderSize = 0;
            btnX.Click += (s, e) => ClosePanel();
            hdr.Controls.Add(hdrLbl);
            hdr.Controls.Add(btnX);
            Controls.Add(hdr);

            int y = 68;

            // ── Title ───────────────────────────────────────────────
            AddLabel("Title *", 20, y); y += 20;
            _txtTitle = AddTextBox(20, y, 670, "Announcement title..."); y += 38;

            // ── Description ─────────────────────────────────────────
            AddLabel("Description *", 20, y); y += 20;
            _txtDesc = AddTextBox(20, y, 670, "Announcement details...", multiline: true, height: 80); y += 92;

            // ── Category ────────────────────────────────────────────
            AddLabel("Category *", 20, y);
            _cmbCat = new ComboBox
            {
                Location = new Point(20, y + 20),
                Size = new Size(300, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f)
            };
            _cmbCat.Items.AddRange(Categories);
            _cmbCat.SelectedIndexChanged += CmbCat_SelectedIndexChanged;
            Controls.Add(_cmbCat);
            y += 58;

            // ── Urgent / Pinned checkboxes ───────────────────────────
            _chkUrgent = new CheckBox
            {
                Text = "⚠  Mark as Urgent",
                Location = new Point(20, y),
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.Firebrick,
                AutoSize = true
            };
            _chkPinned = new CheckBox
            {
                Text = "📌  Pin to Top",
                Location = new Point(180, y),
                Font = new Font("Segoe UI", 9.5f),
                AutoSize = true
            };
            Controls.Add(_chkUrgent);
            Controls.Add(_chkPinned);
            y += 38;

            // ── Divider ─────────────────────────────────────────────
            Controls.Add(new Panel { BackColor = Color.FromArgb(220, 220, 220), Location = new Point(0, y), Size = new Size(720, 1) });
            y += 14;

            // ── Notification section ─────────────────────────────────
            AddSectionLabel("🔔  Send Notification To", 20, y);
            y += 24;

            _chkNotifyStudents = new CheckBox
            {
                Text = "👨‍🎓  All Students",
                Location = new Point(28, y),
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(30, 100, 180),
                AutoSize = true
            };
            _chkNotifyInstructors = new CheckBox
            {
                Text = "👩‍🏫  All Instructors",
                Location = new Point(200, y),
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(80, 50, 160),
                AutoSize = true
            };
            Controls.Add(_chkNotifyStudents);
            Controls.Add(_chkNotifyInstructors);
            y += 38;

            // ── Divider ─────────────────────────────────────────────
            Controls.Add(new Panel { BackColor = Color.FromArgb(220, 220, 220), Location = new Point(0, y), Size = new Size(720, 1) });
            y += 14;

            // ── Attachment section ───────────────────────────────────
            AddSectionLabel("📎  Attachment  (PDF or Image)", 20, y);
            y += 24;

            _btnBrowse = new Button
            {
                Text = "  Choose File…",
                Location = new Point(28, y),
                Size = new Size(130, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(245, 245, 245),
                ForeColor = Color.FromArgb(50, 50, 50),
                Font = new Font("Segoe UI", 9f),
                Cursor = Cursors.Hand
            };
            _btnBrowse.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            _btnBrowse.Click += BtnBrowse_Click;
            Controls.Add(_btnBrowse);

            _lblAttachName = new Label
            {
                Text = "No file chosen",
                Location = new Point(166, y + 6),
                Size = new Size(420, 20),
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(130, 130, 130),
                AutoEllipsis = true
            };
            Controls.Add(_lblAttachName);

            _btnClearAttach = new Button
            {
                Text = "✕",
                Location = new Point(594, y + 2),
                Size = new Size(28, 26),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(160, 0, 0),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = false
            };
            _btnClearAttach.FlatAppearance.BorderSize = 0;
            _btnClearAttach.Click += (s, e) => ClearAttachment();
            Controls.Add(_btnClearAttach);
            y += 44;

            // ── Divider ─────────────────────────────────────────────
            Controls.Add(new Panel { BackColor = Color.FromArgb(220, 220, 220), Location = new Point(0, y), Size = new Size(720, 1) });
            y += 12;

            // ── Action buttons ───────────────────────────────────────
            _btnCancel = new Button
            {
                Text = "✕  Cancel",
                Location = new Point(472, y),
                Size = new Size(110, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(80, 80, 80),
                Font = new Font("Segoe UI", 9.5f)
            };
            _btnCancel.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            _btnCancel.Click += (s, e) => ClosePanel();

            _btnPost = new Button
            {
                Text = "📢  Post Announcement",
                Location = new Point(590, y),
                Size = new Size(118, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = Maroon,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5f)
            };
            _btnPost.FlatAppearance.BorderSize = 0;
            _btnPost.Click += BtnPost_Click;

            Controls.Add(_btnCancel);
            Controls.Add(_btnPost);

            Size = new Size(720, y + 52);
        }

        // ── Auto-mark Emergency as urgent ───────────────────────
        private void CmbCat_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_cmbCat.SelectedItem?.ToString() == "Emergency")
            {
                _chkUrgent.Checked = true;
                _chkNotifyStudents.Checked = true;
                _chkNotifyInstructors.Checked = true;
            }
        }

        // ── Attachment browse ────────────────────────────────────
        private void BtnBrowse_Click(object? sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog
            {
                Title = "Select Attachment",
                Filter = "Allowed files (*.pdf;*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.pdf;*.png;*.jpg;*.jpeg;*.gif;*.bmp|PDF files (*.pdf)|*.pdf|Image files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp",
                Multiselect = false
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _attachmentPath = dlg.FileName;
                _lblAttachName.Text = Path.GetFileName(dlg.FileName);
                _lblAttachName.ForeColor = Color.FromArgb(20, 20, 20);
                _btnClearAttach.Visible = true;
            }
        }

        private void ClearAttachment()
        {
            _attachmentPath = string.Empty;
            _lblAttachName.Text = "No file chosen";
            _lblAttachName.ForeColor = Color.FromArgb(130, 130, 130);
            _btnClearAttach.Visible = false;
        }

        // ── Helpers ─────────────────────────────────────────────
        private Label AddLabel(string text, int x, int y)
        {
            var lbl = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50)
            };
            Controls.Add(lbl);
            return lbl;
        }

        private Label AddSectionLabel(string text, int x, int y)
        {
            var lbl = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 80, 80)
            };
            Controls.Add(lbl);
            return lbl;
        }

        private TextBox AddTextBox(int x, int y, int w, string placeholder, bool multiline = false, int height = 26)
        {
            var tb = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(w, height),
                Multiline = multiline,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10f),
                ForeColor = PlaceholderGray,
                Text = placeholder
            };
            tb.GotFocus += (s, e) => { if (tb.ForeColor == PlaceholderGray) { tb.Text = ""; tb.ForeColor = Color.Black; } };
            tb.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(tb.Text)) { tb.Text = placeholder; tb.ForeColor = PlaceholderGray; } };
            Controls.Add(tb);
            return tb;
        }

        private bool IsPlaceholder(TextBox tb)
            => tb.ForeColor == PlaceholderGray || string.IsNullOrWhiteSpace(tb.Text);

        // ── Post / save ──────────────────────────────────────────
        private void BtnPost_Click(object? sender, EventArgs e)
        {
            if (IsPlaceholder(_txtTitle)) { MessageBox.Show("Enter a title.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (IsPlaceholder(_txtDesc)) { MessageBox.Show("Enter a description.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (_cmbCat.SelectedIndex < 0) { MessageBox.Show("Select a category.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            var data = new AnnouncementDataAdmin
            {
                Title = _txtTitle.Text.Trim(),
                Description = _txtDesc.Text.Trim(),
                Category = _cmbCat.SelectedItem?.ToString() ?? "General",
                PostDate = DateTime.Now,
                IsUrgent = _chkUrgent.Checked,
                IsPinned = _chkPinned.Checked,
                NotifyStudents = _chkNotifyStudents.Checked,
                NotifyInstructors = _chkNotifyInstructors.Checked,
                AttachmentPath = _attachmentPath,
            };

            // Build a human-readable summary of who gets notified
            var targets = new List<string>();
            if (data.NotifyStudents) targets.Add("Students");
            if (data.NotifyInstructors) targets.Add("Instructors");
            string notifLine = targets.Count > 0
                ? $"\nNotification will be sent to: {string.Join(" & ", targets)}."
                : "";
            string attachLine = !string.IsNullOrEmpty(_attachmentPath)
                ? $"\nAttachment: {Path.GetFileName(_attachmentPath)}"
                : "";

            MessageBox.Show(
                $"Announcement posted successfully!{notifLine}{attachLine}",
                "Posted", MessageBoxButtons.OK, MessageBoxIcon.Information);

            AnnouncementPosted?.Invoke(this, data);
            ClosePanel();
        }

        // ── Load for edit ────────────────────────────────────────
        public void LoadForEdit(AnnouncementDataAdmin data)
        {
            // Title
            _txtTitle.Text = string.IsNullOrEmpty(data.Title) ? _txtTitle.Text : data.Title;
            _txtTitle.ForeColor = string.IsNullOrEmpty(data.Title) ? PlaceholderGray : Color.Black;

            // Description
            _txtDesc.Text = string.IsNullOrEmpty(data.Description) ? _txtDesc.Text : data.Description;
            _txtDesc.ForeColor = string.IsNullOrEmpty(data.Description) ? PlaceholderGray : Color.Black;

            // Category
            int idx = _cmbCat.Items.IndexOf(data.Category);
            if (idx >= 0) _cmbCat.SelectedIndex = idx;

            // Flags
            _chkUrgent.Checked = data.IsUrgent;
            _chkPinned.Checked = data.IsPinned;
            _chkNotifyStudents.Checked = data.NotifyStudents;
            _chkNotifyInstructors.Checked = data.NotifyInstructors;

            // Attachment
            if (!string.IsNullOrEmpty(data.AttachmentPath))
            {
                _attachmentPath = data.AttachmentPath;
                _lblAttachName.Text = Path.GetFileName(data.AttachmentPath);
                _lblAttachName.ForeColor = Color.FromArgb(20, 20, 20);
                _btnClearAttach.Visible = true;
            }
            else
            {
                ClearAttachment();
            }

            _btnPost.Text = string.IsNullOrEmpty(data.Title) ? "📢  Post Announcement" : "💾  Save Changes";
        }

        private void ClosePanel()
        {
            Visible = false;
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }

    // ----------------------------------------------------------
    //  ViewAnnouncement  (functional stub — shows real data)
    // ----------------------------------------------------------
    public partial class ViewAnnouncementAdmin : UserControl
    {
        public event EventHandler<int>? EditRequested;
        public event EventHandler<int>? DeleteRequested;
        public event EventHandler? CloseRequested;

        private static readonly Color Maroon = Color.FromArgb(139, 0, 0);

        // ── Category palette (kept in sync with Categories array above) ──
        private static readonly Dictionary<string, Color> CatIconColor = new()
        {
            ["General"] = Color.FromArgb(55, 138, 221),
            ["Academic"] = Color.FromArgb(99, 153, 34),
            ["Events"] = Color.FromArgb(212, 83, 126),
            ["Emergency"] = Color.FromArgb(200, 30, 30),
            ["Enrollment"] = Color.FromArgb(13, 154, 138),
        };
        private static readonly Dictionary<string, Color> CatBgColor = new()
        {
            ["General"] = Color.FromArgb(230, 241, 251),
            ["Academic"] = Color.FromArgb(234, 243, 222),
            ["Events"] = Color.FromArgb(251, 234, 240),
            ["Emergency"] = Color.FromArgb(255, 224, 224),
            ["Enrollment"] = Color.FromArgb(214, 244, 241),
        };

        private int _currentId = -1;

        private Label _lblTitle, _lblDesc, _lblAuthor, _lblDate, _lblStatus, _lblCat;
        private Label _lblNotify;     // notification targets
        private Label _lblAttachment; // attachment info
        private Button _btnEdit, _btnDelete;
        private Panel _pnlHeader;

        public ViewAnnouncementAdmin()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            BackColor = Color.White;
            Size = new Size(760, 440);   // slightly taller for new rows
            AutoScroll = false;

            // ── Header ─────────────────────────────────────────────
            _pnlHeader = new Panel { BackColor = Maroon, Dock = DockStyle.Top, Height = 55 };
            var hdrLbl = new Label
            {
                Text = "Announcement Details",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12f),
                AutoSize = true,
                Location = new Point(60, 16)
            };
            var btnX = new Button
            {
                Text = "×",
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                Size = new Size(34, 34),
                Location = new Point(718, 10)
            };
            btnX.FlatAppearance.BorderSize = 0;
            btnX.Click += (s, e) => { Visible = false; CloseRequested?.Invoke(this, EventArgs.Empty); };
            _pnlHeader.Controls.Add(hdrLbl);
            _pnlHeader.Controls.Add(btnX);
            Controls.Add(_pnlHeader);

            int y = 68;

            // ── Category + status badges ─────────────────────────────
            _lblCat = MakeLabel("", 24, y, bold: true, fontSize: 8f);
            _lblCat.Size = new Size(110, 20);
            _lblCat.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(_lblCat);

            _lblStatus = MakeLabel("", 142, y, bold: true, fontSize: 8f);
            _lblStatus.Size = new Size(76, 20);
            _lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(_lblStatus);
            y += 30;

            // ── Title ───────────────────────────────────────────────
            _lblTitle = MakeLabel("", 24, y, bold: true, fontSize: 14f);
            _lblTitle.Size = new Size(710, 28);
            Controls.Add(_lblTitle);
            y += 34;

            // ── Description ─────────────────────────────────────────
            _lblDesc = MakeLabel("", 24, y, fontSize: 9.5f);
            _lblDesc.Size = new Size(710, 60);
            _lblDesc.AutoEllipsis = false;
            Controls.Add(_lblDesc);
            y += 70;

            // ── Meta row ────────────────────────────────────────────
            _lblAuthor = MakeLabel("", 24, y, fontSize: 8.5f); _lblAuthor.ForeColor = Color.FromArgb(90, 90, 90); Controls.Add(_lblAuthor);
            _lblDate = MakeLabel("", 220, y, fontSize: 8.5f); _lblDate.ForeColor = Color.FromArgb(90, 90, 90); Controls.Add(_lblDate);
            y += 30;

            // ── Notification targets row (NEW) ─────────────────────
            _lblNotify = MakeLabel("", 24, y, fontSize: 8.5f);
            _lblNotify.ForeColor = Color.FromArgb(30, 100, 180);
            _lblNotify.Size = new Size(710, 18);
            Controls.Add(_lblNotify);
            y += 22;

            // ── Attachment row (NEW) ──────────────────────────────────
            _lblAttachment = MakeLabel("", 24, y, fontSize: 8.5f);
            _lblAttachment.ForeColor = Color.FromArgb(100, 60, 160);
            _lblAttachment.Size = new Size(710, 18);
            Controls.Add(_lblAttachment);
            y += 24;

            // ── Divider ─────────────────────────────────────────────
            Controls.Add(new Panel { BackColor = Color.FromArgb(220, 220, 220), Location = new Point(0, y), Size = new Size(760, 1) });
            y += 12;

            // ── Action buttons ───────────────────────────────────────
            _btnDelete = new Button
            {
                Text = "🗑  Delete",
                Location = new Point(530, y),
                Size = new Size(100, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(180, 0, 0)
            };
            _btnDelete.FlatAppearance.BorderColor = Color.FromArgb(200, 50, 50);
            _btnDelete.Click += (s, e) =>
            {
                if (_currentId < 0) return;
                if (MessageBox.Show("Delete this announcement?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DeleteRequested?.Invoke(this, _currentId);
                    Visible = false;
                    CloseRequested?.Invoke(this, EventArgs.Empty);
                }
            };

            _btnEdit = new Button
            {
                Text = "✏  Edit",
                Location = new Point(638, y),
                Size = new Size(100, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Maroon,
                ForeColor = Color.White
            };
            _btnEdit.FlatAppearance.BorderSize = 0;
            _btnEdit.Click += (s, e) => { if (_currentId >= 0) EditRequested?.Invoke(this, _currentId); };

            Controls.Add(_btnDelete);
            Controls.Add(_btnEdit);
            Size = new Size(760, y + 50);
        }

        private Label MakeLabel(string text, int x, int y, bool bold = false, float fontSize = 9f)
            => new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", fontSize, bold ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = Color.FromArgb(20, 20, 20),
                BackColor = Color.Transparent
            };

        public void LoadAnnouncement(AdminAnnouncement a)
        {
            if (a == null) return;
            _currentId = a.Id;

            Color iconCol = CatIconColor.GetValueOrDefault(a.Category, Color.Gray);
            Color iconBg = CatBgColor.GetValueOrDefault(a.Category, Color.WhiteSmoke);

            _lblCat.Text = a.Category;
            _lblCat.ForeColor = iconCol;
            _lblCat.BackColor = iconBg;
            ApplyRounded(_lblCat, 10);

            if (a.Status == "active")
            { _lblStatus.Text = "● Active"; _lblStatus.ForeColor = Color.FromArgb(22, 163, 74); _lblStatus.BackColor = Color.FromArgb(220, 252, 231); }
            else
            { _lblStatus.Text = "● Inactive"; _lblStatus.ForeColor = Color.FromArgb(100, 100, 100); _lblStatus.BackColor = Color.FromArgb(230, 230, 230); }
            ApplyRounded(_lblStatus, 10);

            _lblTitle.Text = a.Title;
            _lblDesc.Text = a.Description;
            _lblAuthor.Text = "👤  " + (string.IsNullOrWhiteSpace(a.InstructorName) ? "Admin" : a.InstructorName);
            _lblDate.Text = "📅  " + a.Date.ToString("MMMM d, yyyy  •  h:mm tt");

            // ── Notification targets (NEW) ───────────────────────
            var targets = new List<string>();
            if (a.NotifyStudents) targets.Add("Students");
            if (a.NotifyInstructors) targets.Add("Instructors");
            _lblNotify.Text = targets.Count > 0
                ? "🔔  Notification sent to: " + string.Join(" & ", targets)
                : "🔕  No notification sent";

            // ── Attachment (NEW) ─────────────────────────────────
            _lblAttachment.Text = !string.IsNullOrWhiteSpace(a.AttachedFile)
                ? "📎  Attachment: " + a.AttachedFile
                : "📎  No attachment";
            _lblAttachment.ForeColor = !string.IsNullOrWhiteSpace(a.AttachedFile)
                ? Color.FromArgb(100, 60, 160)
                : Color.FromArgb(160, 160, 160);

            Refresh();
        }

        private static void ApplyRounded(Control c, int r)
        {
            var path = new GraphicsPath();
            path.AddArc(0, 0, r, r, 180, 90);
            path.AddArc(c.Width - r, 0, r, r, 270, 90);
            path.AddArc(c.Width - r, c.Height - r, r, r, 0, 90);
            path.AddArc(0, c.Height - r, r, r, 90, 90);
            path.CloseFigure();
            c.Region = new Region(path);
        }
    }

    // ----------------------------------------------------------
    //  AnnouncementLayout  (stub — AdminPortal uses inline cards)
    // ----------------------------------------------------------
    public partial class AnnouncementLayoutAdmin : UserControl
    {
        public event EventHandler<int>? CardClicked;
        public event EventHandler<int>? PinToggled;
        public event EventHandler<int>? MenuEditClicked;
        public event EventHandler<int>? MenuToggleClicked;
        public event EventHandler<int>? MenuDeleteClicked;

        public AnnouncementLayoutAdmin() { }

        public void LoadInstructor(int id, string title, string description,
            string category, string status, string instructorName,
            DateTime date, bool isPinned, bool isUrgent,
            int viewedCount, int totalStudents, int cardWidth)
        { }

        public void LoadStudent(int id, string title, string description,
            string category, string officeName, DateTime date,
            bool isUrgent, bool isPinned, bool isRead,
            int cardWidth, string instructorName = "")
        { }
    }
}