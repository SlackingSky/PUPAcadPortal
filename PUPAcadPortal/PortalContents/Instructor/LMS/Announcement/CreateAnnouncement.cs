using CloudinaryDotNet.Core;
using PUPAcadPortal.Models;
using PUPAcadPortal.Services;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    // This UserControl provides a form for instructors to create or edit announcements

    public partial class CreateAnnouncement : UserControl
    {
        public DialogResult DialogResult { get; private set; } = DialogResult.Cancel;

        // Original path chosen by the user (for display only)
        private string _attachedFilePath = string.Empty;

        // Path of the AES-encrypted temp copy — this is what gets uploaded
        private string _encryptedTempPath = string.Empty;

        private static readonly Color Maroon = Color.FromArgb(139, 0, 0);
        private static readonly Color PlaceholderGray = Color.FromArgb(160, 160, 160);
        private static readonly Color EncryptGreen = Color.FromArgb(34, 139, 34);

        public CreateAnnouncement()
        {
            InitializeComponent();
            WireEvents();
            RestoreAllPlaceholders();
        }

        // ── Event wiring ──────────────────────────────────────────────────────

        private void WireEvents()
        {
            btnCancel.Click += (s, e) => ClosePanel();
            btnPost.Click += BtnPost_ClickAsync;   // async handler (see below)
            btnBrowse.Click += BtnBrowse_ClickAsync;

            txtTitle.GotFocus += (s, e) => ClearPlaceholder(txtTitle, "Insert announcement title here...");
            txtTitle.LostFocus += (s, e) => RestorePlaceholder(txtTitle, "Insert announcement title here...");

            txtDescription.GotFocus += (s, e) => ClearPlaceholder(txtDescription, "Write your announcement details here...");
            txtDescription.LostFocus += (s, e) => RestorePlaceholder(txtDescription, "Write your announcement details here...");

            txtDescription.TextChanged += TxtDescription_TextChanged;

            pnlAttachment.Click += BtnBrowse_ClickAsync;
            lblAttachHint.Click += BtnBrowse_ClickAsync;
            lblAttachHint2.Click += BtnBrowse_ClickAsync;
            picAttachIcon.Click += BtnBrowse_ClickAsync;

            pnlAttachment.MouseEnter += (s, e) => pnlAttachment.BackColor = Color.FromArgb(245, 245, 245);
            pnlAttachment.MouseLeave += (s, e) => pnlAttachment.BackColor = Color.White;

            lnkSelectAll.LinkClicked += (s, e) => SetAllCourses(true);
            lnkClearAll.LinkClicked += (s, e) => SetAllCourses(false);
        }

        // ── Close / placeholders ──────────────────────────────────────────────

        private void ClosePanel()
        {
            DialogResult = DialogResult.Cancel;
            DeleteEncryptedTemp();
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
            if (txtDescription.ForeColor == PlaceholderGray) return;
            int remaining = MaxDescChars - txtDescription.Text.Length;
            lblCharCount.Text = $"{Math.Max(0, remaining)} characters remaining";
            lblCharCount.ForeColor = remaining < 50 ? Color.Firebrick : Color.Gray;
        }

        // ── Browse & Encrypt ──────────────────────────────────────────────────

        private async void BtnBrowse_ClickAsync(object sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog
            {
                Title = "Select Attachment",
                Filter = "Documents (*.pdf;*.docx;*.pptx)|*.pdf;*.docx;*.pptx" +
                         "|Images (*.png;*.jpg)|*.png;*.jpg" +
                         "|All Files (*.*)|*.*"
            };

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            string chosenPath = dlg.FileName;
            string fileName = Path.GetFileName(chosenPath);
            long fileSize = new FileInfo(chosenPath).Length;
            string sizeStr = fileSize >= 1_048_576
                                    ? $"{fileSize / 1_048_576.0:F1} MB"
                                    : $"{fileSize / 1024.0:F0} KB";

            // Show "encrypting…" feedback
            lblAttachHint.Text = $"{fileName}  (encrypting…)";
            lblAttachHint.ForeColor = Color.DimGray;
            lblAttachHint2.Text = sizeStr;
            lblAttachHint2.ForeColor = Color.Gray;
            btnPost.Enabled = false;   // disable Post while encrypting

            try
            {
                // Delete any previous temp file
                DeleteEncryptedTemp();

                byte[] originalBytes = await File.ReadAllBytesAsync(chosenPath);
                byte[] encryptedBytes = await FileServerConnectService.EncryptFileAsync(originalBytes);

                string ext = Path.GetExtension(chosenPath);
                _encryptedTempPath = Path.Combine(Path.GetTempPath(),
                                            $"enc_{Guid.NewGuid():N}{ext}");
                await File.WriteAllBytesAsync(_encryptedTempPath, encryptedBytes);

                _attachedFilePath = chosenPath;   // keep for display / edit round-trip

                // Success UI
                lblAttachHint.Text = fileName;
                lblAttachHint.ForeColor = Color.Black;
                lblAttachHint2.Text = $"{sizeStr}  •  🔒 Encrypted";
                lblAttachHint2.ForeColor = EncryptGreen;
                btnRemoveAttach.Visible = true;

                System.Diagnostics.Debug.WriteLine(
                    $"File encrypted to temp: {_encryptedTempPath}");
            }
            catch (Exception ex)
            {
                // Encryption failed — reset UI
                _attachedFilePath = string.Empty;
                _encryptedTempPath = string.Empty;

                lblAttachHint.Text = "Encryption failed — please try again.";
                lblAttachHint.ForeColor = Color.Firebrick;
                lblAttachHint2.Text = ex.Message;
                lblAttachHint2.ForeColor = Color.Gray;
                btnRemoveAttach.Visible = false;

                System.Diagnostics.Debug.WriteLine($"Encryption error: {ex.Message}");
            }
            finally
            {
                btnPost.Enabled = true;
            }
        }

        // ── Remove attachment ─────────────────────────────────────────────────

        private void BtnRemoveAttach_Click(object sender, EventArgs e)
        {
            DeleteEncryptedTemp();
            _attachedFilePath = string.Empty;
            _encryptedTempPath = string.Empty;

            lblAttachHint.Text = "Drag and drop a file here, or click Browse";
            lblAttachHint.ForeColor = Color.DimGray;
            lblAttachHint2.Text = "Supported: PDF, DOCX, PPTX, PNG, JPG  ·  Max 10 MB";
            lblAttachHint2.ForeColor = Color.Gray;
            btnRemoveAttach.Visible = false;
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private void SetAllCourses(bool check)
        {
            for (int i = 0; i < clbCourses.Items.Count; i++)
                clbCourses.SetItemChecked(i, check);
        }

        private bool IsPlaceholder(TextBox tb, string placeholder) =>
            tb.Text == placeholder || string.IsNullOrWhiteSpace(tb.Text);

        /// <summary>Deletes the encrypted temp file if it exists.</summary>
        private void DeleteEncryptedTemp()
        {
            if (!string.IsNullOrEmpty(_encryptedTempPath) && File.Exists(_encryptedTempPath))
            {
                try { File.Delete(_encryptedTempPath); }
                catch { /* nothing lol */ }
            }
        }

        // ── Post announcement ─────────────────────────────────────────────────
        private async void BtnPost_ClickAsync(object sender, EventArgs e)
        {
            // ── Validation ────────────────────────────────────────────────────
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

            // ── Upload encrypted file (if any) ────────────────────────────────
            btnPost.Enabled = false;
            btnPost.Text = "Uploading…";

            string attachmentUrl = null;

            try
            {
                if (!string.IsNullOrEmpty(_encryptedTempPath) &&
                    File.Exists(_encryptedTempPath))
                {
                    // The temp file is already encrypted — upload it directly.
                    // We use the raw UploadFileAsync here (no double-encryption)
                    // because the bytes are already AES-encrypted.
                    var uploadService = new CloudinaryUploadService();

                    // Upload using the internal helper that skips re-encryption
                    attachmentUrl = await UploadAlreadyEncryptedAsync(
                                        uploadService, _encryptedTempPath);

                    if (string.IsNullOrEmpty(attachmentUrl))
                    {
                        ShowError("Failed to upload the encrypted file. " +
                                  "Please check your connection and try again.");
                        return;
                    }

                    // Clean up temp file after successful upload
                    DeleteEncryptedTemp();
                    _encryptedTempPath = string.Empty;
                }

                // ── Save to database ──────────────────────────────────────────
                using (var context = new AppDbContext())
                {
                    var ann = new Announcement
                    {
                        CreatedByUserId = 1,
                        SubjectOfferingId = null,
                        Title = txtTitle.Text.Trim(),
                        Content = txtDescription.Text.Trim(),
                        Category = cmbCategory.Text,
                        IsUrgent = chkUrgent.Checked,
                        IsPinned = chkPinned.Checked,
                        AttachedFile = attachmentUrl,   // encrypted Cloudinary URL
                        PostedDate = dtpPostDate.Value
                    };

                    context.Announcements.Add(ann);
                    context.SaveChanges();
                }

                // ── Raise event for parent container ──────────────────────────
                var data = new AnnouncementData
                {
                    Title = txtTitle.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    Category = cmbCategory.SelectedItem?.ToString() ?? string.Empty,
                    PostDate = dtpPostDate.Value,
                    IsUrgent = chkUrgent.Checked,
                    IsPinned = chkPinned.Checked,
                    AttachedFile = attachmentUrl ?? string.Empty,
                };
                OnAnnouncementPosted(data);

                MessageBox.Show("Announcement posted successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();
            }
            catch (Exception ex)
            {
                ShowError($"An unexpected error occurred:\n{ex.Message}");
                System.Diagnostics.Debug.WriteLine($"BtnPost_ClickAsync error: {ex}");
            }
            finally
            {
                btnPost.Enabled = true;
                btnPost.Text = "Post";
            }
        }

        private static async Task<string> UploadAlreadyEncryptedAsync(
            CloudinaryUploadService svc,
            string encryptedTempPath)
        {
            // This helper uploads a file that is already encrypted, without trying

            if (!File.Exists(encryptedTempPath))
                return null;

            try
            {
                await FileServerConnectService.GetDecryptedCredentialsAsync();

                string cloudName = FileServerConnectService.CloudName;
                string apiKey = FileServerConnectService.CloudKey;
                string apiSecret = FileServerConnectService.CloudSecret;

                if (string.IsNullOrEmpty(cloudName) ||
                    string.IsNullOrEmpty(apiKey) ||
                    string.IsNullOrEmpty(apiSecret))
                    return null;

                var cloudinary = new CloudinaryDotNet.Cloudinary(
                                    new CloudinaryDotNet.Account(cloudName, apiKey, apiSecret));

                var uploadParams = new CloudinaryDotNet.Actions.RawUploadParams
                {
                    File = new CloudinaryDotNet.FileDescription(encryptedTempPath),
                    Folder = "pup_announcements",
                    Tags = "encrypted"
                };

                var result = await cloudinary.UploadAsync(uploadParams);

                return result.StatusCode == System.Net.HttpStatusCode.OK
                    ? result.SecureUrl.AbsoluteUri
                    : null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"UploadAlreadyEncryptedAsync error: {ex.Message}");
                return null;
            }
        }


        public event EventHandler<AnnouncementData> AnnouncementPosted;
        protected virtual void OnAnnouncementPosted(AnnouncementData data) =>
            AnnouncementPosted?.Invoke(this, data);

        /// <summary>Loads an existing announcement into the form for editing.</summary>
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

            // Show existing attachment (already uploaded & encrypted in the cloud)
            if (!string.IsNullOrEmpty(data.AttachedFile))
            {
                _attachedFilePath = data.AttachedFile;
                _encryptedTempPath = string.Empty;   // already in cloud — no local temp

                string displayName = Path.GetFileName(data.AttachedFile);
                // If it's a URL, show a shorter label
                if (data.AttachedFile.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    displayName = "[Cloud file] " + displayName;

                lblAttachHint.Text = displayName;
                lblAttachHint.ForeColor = Color.Black;
                lblAttachHint2.Text = "🔒 Encrypted — stored in cloud";
                lblAttachHint2.ForeColor = EncryptGreen;
                btnRemoveAttach.Visible = true;
            }

            lblFormTitle.Text = "Edit Announcement";
            btnPost.Text = "Save Changes";
        }

        private static void ShowError(string message) =>
            MessageBox.Show(message, "Required Field",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
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