using PUPAcadPortal.PortalContents.Admin.LMS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Misc.LMS
{
    public partial class CreateAnnouncementAdmin : UserControl
    {
        public event EventHandler<AnnouncementDataAdmin>? AnnouncementPosted;
        public event EventHandler? CloseRequested;

        public static readonly string[] Categories = { "General", "Academic", "Events", "Emergency", "Enrollment" };
        private static readonly Color PlaceholderGray = Color.FromArgb(160, 160, 160);

        private string attachmentPath = string.Empty;
        private string _titlePlaceholder = "Announcement title...";
        private string _descPlaceholder = "Announcement details...";

        public CreateAnnouncementAdmin()
        {
            InitializeComponent();
            _cmbCat.Items.AddRange(Categories);
            SetupPlaceholders();
        }

        private void SetupPlaceholders()
        {
            _txtTitle.Text = _titlePlaceholder;
            _txtTitle.ForeColor = PlaceholderGray;
            _txtTitle.GotFocus += (s, e) => { if (_txtTitle.ForeColor == PlaceholderGray) { _txtTitle.Text = ""; _txtTitle.ForeColor = Color.Black; } };
            _txtTitle.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(_txtTitle.Text)) { _txtTitle.Text = _titlePlaceholder; _txtTitle.ForeColor = PlaceholderGray; } };
            _txtDesc.Text = _descPlaceholder;
            _txtDesc.ForeColor = PlaceholderGray;
            _txtDesc.GotFocus += (s, e) => { if (_txtDesc.ForeColor == PlaceholderGray) { _txtDesc.Text = ""; _txtDesc.ForeColor = Color.Black; } };
            _txtDesc.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(_txtDesc.Text)) { _txtDesc.Text = _descPlaceholder; _txtDesc.ForeColor = PlaceholderGray; } };
        }

        private bool IsPlaceholder(TextBox tb) => tb.ForeColor == PlaceholderGray || string.IsNullOrWhiteSpace(tb.Text);

        private void CmbCat_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_cmbCat.SelectedItem?.ToString() == "Emergency")
            {
                _chkUrgent.Checked = true;
                _chkNotifyStudents.Checked = true;
                _chkNotifyInstructors.Checked = true;
            }
        }

        private void BtnBrowse_Click(object? sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog
            {
                Title = "Select Attachment",
                Filter = "Allowed files (*.pdf;*.png;*.jpg)|*.pdf;*.png;*.jpg|PDF files (*.pdf)|*.pdf|Image files (*.png;*.jpg)|*.png;*.jpg",
                Multiselect = false
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                attachmentPath = dlg.FileName;
                _lblAttachName.Text = Path.GetFileName(dlg.FileName);
                _lblAttachName.ForeColor = Color.FromArgb(20, 20, 20);
                _btnClearAttach.Visible = true;
            }
        }

        private void BtnClearAttach_Click(object? sender, EventArgs e) => ClearAttachment();

        private void ClearAttachment()
        {
            attachmentPath = string.Empty;
            _lblAttachName.Text = "No file chosen";
            _lblAttachName.ForeColor = Color.FromArgb(130, 130, 130);
            _btnClearAttach.Visible = false;
        }

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
                AttachmentPath = attachmentPath,
            };

            var targets = new List<string>();
            if (data.NotifyStudents) targets.Add("Students");
            if (data.NotifyInstructors) targets.Add("Instructors");
            string notifLine = targets.Count > 0 ? $"\nNotification will be sent to: {string.Join(" & ", targets)}." : "";
            string attachLine = !string.IsNullOrEmpty(attachmentPath) ? $"\nAttachment: {Path.GetFileName(attachmentPath)}" : "";

            MessageBox.Show($"Announcement posted successfully!{notifLine}{attachLine}", "Posted", MessageBoxButtons.OK, MessageBoxIcon.Information);

            AnnouncementPosted?.Invoke(this, data);
            ClosePanel();
        }

        public void LoadForEdit(AnnouncementDataAdmin data)
        {
            _txtTitle.Text = string.IsNullOrEmpty(data.Title) ? _titlePlaceholder : data.Title;
            _txtTitle.ForeColor = string.IsNullOrEmpty(data.Title) ? PlaceholderGray : Color.Black;

            _txtDesc.Text = string.IsNullOrEmpty(data.Description) ? _descPlaceholder : data.Description;
            _txtDesc.ForeColor = string.IsNullOrEmpty(data.Description) ? PlaceholderGray : Color.Black;

            int idx = _cmbCat.Items.IndexOf(data.Category);
            if (idx >= 0) _cmbCat.SelectedIndex = idx;

            _chkUrgent.Checked = data.IsUrgent;
            _chkPinned.Checked = data.IsPinned;
            _chkNotifyStudents.Checked = data.NotifyStudents;
            _chkNotifyInstructors.Checked = data.NotifyInstructors;

            if (!string.IsNullOrEmpty(data.AttachmentPath))
            {
                attachmentPath = data.AttachmentPath;
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

        private void BtnClose_Click(object? sender, EventArgs e) => ClosePanel();
        private void BtnCancel_Click(object? sender, EventArgs e) => ClosePanel();

        private void ClosePanel()
        {
            Visible = false;
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}