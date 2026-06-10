using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class AddModuleDialog : Form
    {
        public string ModuleTitle { get; private set; } = string.Empty;
        public string ModuleDescription { get; private set; } = string.Empty;
        public List<ModuleFile> InitialFiles { get; private set; } = new();

        //  Constants & State 
        private static readonly Color Maroon = Color.FromArgb(139, 0, 0);
        private static readonly Color BgGray = Color.FromArgb(248, 248, 250);
        private readonly int _moduleNumber;
        private readonly bool _isEditMode;
        public AddModuleDialog(int nextModuleNumber)
            : this(nextModuleNumber, string.Empty, string.Empty, isEdit: false) { }

        //  EDIT constructor 
        public AddModuleDialog(int moduleNumber, string existingTitle, string existingDescription)
            : this(moduleNumber, existingTitle, existingDescription, isEdit: true) { }

        //  Private shared constructor 
        private AddModuleDialog(
            int moduleNumber,
            string existingTitle,
            string existingDescription,
            bool isEdit)
        {
            _moduleNumber = moduleNumber;
            _isEditMode = isEdit;

            InitializeComponent();

            // Title + description pre-fill
            _txtTitle.Text = string.IsNullOrWhiteSpace(existingTitle)
                ? $"Module {_moduleNumber}"
                : existingTitle;
            _txtDesc.Text = existingDescription;

            // Adjust dialog heading when editing
            if (_isEditMode)
            {
                this.Text = "Edit Module";
                btnOk.Text = "Save Changes";
            }

            this.Load += (s, e) =>
            {
                _txtTitle.Focus();
                _txtTitle.SelectAll();
            };
        }

        private void IconPanel_Paint(object sender, PaintEventArgs pe)
        {
            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var b = new SolidBrush(Color.FromArgb(180, 255, 255, 255));
            pe.Graphics.FillEllipse(b, 0, 0, 31, 31);

            using var font = new Font("Segoe UI", 10F, FontStyle.Bold);
            using var fb = new SolidBrush(Maroon);
            var sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            pe.Graphics.DrawString("M", font, fb, new RectangleF(0, 0, 32, 32), sf);
        }

        private void PaintDropZone(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var pnl = sender as Panel;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using var bg = new SolidBrush(BgGray);
            g.FillRectangle(bg, 0, 0, pnl.Width, pnl.Height);

            using var pen = new Pen(Color.FromArgb(200, 200, 210), 1.5f);
            pen.DashStyle = DashStyle.Dash;
            g.DrawRectangle(pen, 1, 1, pnl.Width - 3, pnl.Height - 3);

            using var iconBrush = new SolidBrush(Color.FromArgb(170, 170, 185));
            using var iconFont = new Font("Segoe UI", 18F);
            var sf = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString("⬆", iconFont, iconBrush, new RectangleF(14, 0, 44, pnl.Height), sf);
        }

        //  & Logic 
        private void TxtTitle_Enter(object sender, EventArgs e)
        {
            if (_txtTitle.ForeColor == Color.FromArgb(30, 30, 35))
                _txtTitle.SelectAll();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtTitle.Text))
            {
                MessageBox.Show("Please enter a module title.", "Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtTitle.Focus();
                return;
            }

            ModuleTitle = _txtTitle.Text.Trim();
            ModuleDescription = _txtDesc.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BrowseFiles(object sender, EventArgs e)
        {
            // File browsing only makes sense when creating a new module
            if (_isEditMode) return;

            using var ofd = new OpenFileDialog
            {
                Title = "Attach Files to Module",
                Multiselect = true,
                Filter = "Documents & Images|*.pdf;*.docx;*.pptx;*.png;*.jpg;*.jpeg|PDF|*.pdf|Word|*.docx|PowerPoint|*.pptx|Images|*.png;*.jpg;*.jpeg|All Files (*.*)|*.*",
            };

            if (ofd.ShowDialog() != DialogResult.OK) return;

            foreach (var path in ofd.FileNames)
            {
                var fi = new FileInfo(path);
                if (fi.Length > 10_485_760)
                {
                    MessageBox.Show(
                        $"\"{fi.Name}\" exceeds the 10 MB limit and was skipped.",
                        "File Too Large", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }
                InitialFiles.Add(new ModuleFile
                {
                    Name = fi.Name,
                    SizeBytes = fi.Length,
                    Type = fi.Extension.TrimStart('.').ToUpper(),
                    LocalPath = path,   // retain local path so BtnAddModule_Click can upload
                });
            }

            RefreshFileList();
        }

        private void RefreshFileList()
        {
            if (InitialFiles.Count == 0)
            {
                _lblFileCount.Height = 0;
                _lblFileList.Height = 0;
                return;
            }

            _lblFileCount.Text = $"✅  {InitialFiles.Count} file{(InitialFiles.Count == 1 ? "" : "s")} ready to attach:";
            _lblFileCount.Height = 18;

            var names = new System.Text.StringBuilder();
            foreach (var f in InitialFiles)
                names.AppendLine($"  • {f.Name}  ({FormatBytes(f.SizeBytes)})");

            _lblFileList.Text = names.ToString().TrimEnd();
            _lblFileList.Height = Math.Min(60, InitialFiles.Count * 17 + 4);

            this.Invalidate();
        }

        private static string FormatBytes(long b)
        {
            if (b < 1024) return $"{b} B";
            if (b < 1_048_576) return $"{b / 1024.0:F1} KB";
            return $"{b / 1_048_576.0:F1} MB";
        }
    }
}