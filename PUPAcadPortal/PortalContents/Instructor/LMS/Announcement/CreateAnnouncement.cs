using System;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class CreateAnnouncement : UserControl
    {
        public DialogResult DialogResult { get; private set; } = DialogResult.Cancel;

        private string _attachedFilePath = string.Empty;

        private static readonly Color Maroon = Color.FromArgb(139, 0, 0);
        private static readonly Color PlaceholderGray = Color.FromArgb(160, 160, 160);

        public CreateAnnouncement()
        {
            InitializeComponent();
            WireEvents();
            RestoreAllPlaceholders();
        }

        private void WireEvents()
        {
            btnClose.Click += (s, e) => ClosePanel();
            btnCancel.Click += (s, e) => ClosePanel();
            btnPost.Click += BtnPost_Click;
            btnBrowse.Click += BtnBrowse_Click;

            txtTitle.GotFocus += (s, e) => ClearPlaceholder(txtTitle, "Insert announcement title here...");
            txtTitle.LostFocus += (s, e) => RestorePlaceholder(txtTitle, "Insert announcement title here...");

            txtDescription.GotFocus += (s, e) => ClearPlaceholder(txtDescription, "Write your announcement details here...");
            txtDescription.LostFocus += (s, e) => RestorePlaceholder(txtDescription, "Write your announcement details here...");

            txtDescription.TextChanged += TxtDescription_TextChanged;

            pnlAttachment.Click += BtnBrowse_Click;
            lblAttachHint.Click += BtnBrowse_Click;
            lblAttachHint2.Click += BtnBrowse_Click;
            picAttachIcon.Click += BtnBrowse_Click;

            pnlAttachment.MouseEnter += (s, e) => pnlAttachment.BackColor = Color.FromArgb(245, 245, 245);
            pnlAttachment.MouseLeave += (s, e) => pnlAttachment.BackColor = Color.White;

            lnkSelectAll.LinkClicked += (s, e) => SetAllCourses(true);
            lnkClearAll.LinkClicked += (s, e) => SetAllCourses(false);
        }

        private void ClosePanel()
        {
            DialogResult = DialogResult.Cancel;
            this.Visible = false;

            OnCloseRequested(EventArgs.Empty);
        }

        public event EventHandler CloseRequested;
        protected virtual void OnCloseRequested(EventArgs e) => CloseRequested?.Invoke(this, e);
        private void ClearPlaceholder(TextBox tb, string placeholder)
        {
            if (tb.Text == placeholder)
            {
                tb.Text = string.Empty;
                tb.ForeColor = Color.Black;
            }
        }

        private void RestorePlaceholder(TextBox tb, string placeholder)
        {
            if (string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = placeholder;
                tb.ForeColor = PlaceholderGray;
            }
        }

        private void RestoreAllPlaceholders()
        {
            RestorePlaceholder(txtTitle, "Insert announcement title here...");
            RestorePlaceholder(txtDescription, "Write your announcement details here...");
        }

        private const int MaxDescChars = 500;

        private void TxtDescription_TextChanged(object sender, EventArgs e)
        {
            if (txtDescription.ForeColor == PlaceholderGray) return; // placeholder active

            int remaining = MaxDescChars - txtDescription.Text.Length;
            lblCharCount.Text = $"{Math.Max(0, remaining)} characters remaining";
            lblCharCount.ForeColor = remaining < 50 ? Color.Firebrick : Color.Gray;
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Select Attachment";
                dlg.Filter = "Documents (*.pdf;*.docx;*.pptx)|*.pdf;*.docx;*.pptx|Images (*.png;*.jpg)|*.png;*.jpg|All Files (*.*)|*.*";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    _attachedFilePath = dlg.FileName;

                    string fileName = System.IO.Path.GetFileName(dlg.FileName);
                    long fileSize = new System.IO.FileInfo(dlg.FileName).Length;
                    string sizeStr = fileSize >= 1_048_576
                        ? $"{fileSize / 1_048_576.0:F1} MB"
                        : $"{fileSize / 1024.0:F0} KB";

                    lblAttachHint.Text = fileName;
                    lblAttachHint.ForeColor = Color.Black;
                    lblAttachHint2.Text = sizeStr;
                    lblAttachHint2.ForeColor = Color.Gray;

                    btnRemoveAttach.Visible = true;
                }
            }
        }

        private void BtnRemoveAttach_Click(object sender, EventArgs e)
        {
            _attachedFilePath = string.Empty;
            lblAttachHint.Text = "Drag and drop a file here, or click Browse";
            lblAttachHint.ForeColor = Color.DimGray;
            lblAttachHint2.Text = "Supported: PDF, DOCX, PPTX, PNG, JPG  ·  Max 10 MB";
            lblAttachHint2.ForeColor = Color.Gray;
            btnRemoveAttach.Visible = false;
        }

        private void SetAllCourses(bool check)
        {
            for (int i = 0; i < clbCourses.Items.Count; i++)
                clbCourses.SetItemChecked(i, check);
        }

        private bool IsPlaceholder(TextBox tb, string placeholder) =>
            tb.Text == placeholder || string.IsNullOrWhiteSpace(tb.Text);

        private void BtnPost_Click(object sender, EventArgs e)
        {
            if (IsPlaceholder(txtTitle, "Insert announcement title here..."))
            {
                ShowError("Please enter a title for the announcement.");
                txtTitle.Focus();
                return;
            }

            if (IsPlaceholder(txtDescription, "Write your announcement details here..."))
            {
                ShowError("Please enter a description for the announcement.");
                txtDescription.Focus();
                return;
            }

            if (cmbCategory.SelectedIndex < 0)
            {
                ShowError("Please select a category.");
                cmbCategory.Focus();
                return;
            }

            if (clbCourses.CheckedItems.Count == 0)
            {
                ShowError("Please select at least one course.");
                clbCourses.Focus();
                return;
            }

            var announcement = new AnnouncementData
            {
                Title = txtTitle.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                Category = cmbCategory.SelectedItem?.ToString() ?? string.Empty,
                PostDate = dtpPostDate.Value,
                IsUrgent = chkUrgent.Checked,
                IsPinned = chkPinned.Checked,
                AttachedFile = _attachedFilePath,
            };

            foreach (var item in clbCourses.CheckedItems)
                announcement.Courses.Add(item.ToString()!);

            MessageBox.Show(
                "Your announcement has been posted successfully!",
                "Posted",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            OnAnnouncementPosted(announcement);
            ClosePanel();
        }

        public event EventHandler<AnnouncementData> AnnouncementPosted;
        protected virtual void OnAnnouncementPosted(AnnouncementData data) =>
            AnnouncementPosted?.Invoke(this, data);
        public void LoadForEdit(AnnouncementData data)
        {
            txtTitle.Text = data.Title;
            txtTitle.ForeColor = Color.Black;

            txtDescription.Text = data.Description;
            txtDescription.ForeColor = Color.Black;

            int catIdx = cmbCategory.Items.IndexOf(data.Category);
            if (catIdx >= 0) cmbCategory.SelectedIndex = catIdx;

            dtpPostDate.Value = data.PostDate;
            chkUrgent.Checked = data.IsUrgent;
            chkPinned.Checked = data.IsPinned;

            SetAllCourses(false);
            foreach (string course in data.Courses)
            {
                int idx = clbCourses.Items.IndexOf(course);
                if (idx >= 0) clbCourses.SetItemChecked(idx, true);
            }

            if (!string.IsNullOrEmpty(data.AttachedFile))
            {
                _attachedFilePath = data.AttachedFile;
                lblAttachHint.Text = System.IO.Path.GetFileName(data.AttachedFile);
                lblAttachHint.ForeColor = Color.Black;
                btnRemoveAttach.Visible = true;
            }

            lblFormTitle.Text = "Edit Announcement";
            btnPost.Text = "Save Changes";
        }
        private static void ShowError(string message) =>
            MessageBox.Show(message, "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    public class AnnouncementData
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime PostDate { get; set; } = DateTime.Now;
        public bool IsUrgent { get; set; }
        public bool IsPinned { get; set; }
        public string AttachedFile { get; set; } = string.Empty;
        public System.Collections.Generic.List<string> Courses { get; set; } = new();
    }
}