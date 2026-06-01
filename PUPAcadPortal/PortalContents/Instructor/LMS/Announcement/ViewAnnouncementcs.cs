using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PUPAcadPortal.PortalContents.Instructor.LMS;

namespace PUPAcadPortal
{
    public partial class ViewAnnouncement : UserControl
    {
        public event EventHandler<int> EditRequested;
        public event EventHandler<int> DeleteRequested;
        public event EventHandler CloseRequested;

        private int _currentId = -1;

        private static readonly Color Maroon = Color.FromArgb(139, 0, 0);

        private static readonly System.Collections.Generic.Dictionary<string, Color> CatIconColor =
            new System.Collections.Generic.Dictionary<string, Color>
            {
                ["General"] = Color.FromArgb(55, 138, 221),
                ["Academic"] = Color.FromArgb(99, 153, 34),
                ["Schedule"] = Color.FromArgb(186, 117, 23),
                ["Events"] = Color.FromArgb(212, 83, 126),
                ["Examinations"] = Color.FromArgb(127, 119, 221),
            };

        private static readonly System.Collections.Generic.Dictionary<string, Color> CatBgColor =
            new System.Collections.Generic.Dictionary<string, Color>
            {
                ["General"] = Color.FromArgb(230, 241, 251),
                ["Academic"] = Color.FromArgb(234, 243, 222),
                ["Schedule"] = Color.FromArgb(250, 238, 218),
                ["Events"] = Color.FromArgb(251, 234, 240),
                ["Examinations"] = Color.FromArgb(238, 237, 254),
            };

        public ViewAnnouncement()
        {
            InitializeComponent();
            WireEvents();

            this.AutoScroll = false;
            this.HorizontalScroll.Enabled = false;
            this.HorizontalScroll.Visible = false;
            this.VerticalScroll.Visible = false;
        }

        private void WireEvents()
        {
            btnClose.Click += (s, e) => ClosePanel();
            btnClose.MouseEnter += (s, e) => btnClose.BackColor = Color.FromArgb(180, 0, 0);
            btnClose.MouseLeave += (s, e) => btnClose.BackColor = Color.Transparent;
            btnEdit.Click += (s, e) => { if (_currentId >= 0) EditRequested?.Invoke(this, _currentId); };
            btnDelete.Click += (s, e) => ConfirmDelete();
            btnOpenFile.Click += (s, e) => OpenAttachment();
            btnEditFile.Click += (s, e) => EditAttachment();
            Paint += (s, e) => DrawBorder(e.Graphics);
        }

        public void LoadAnnouncement(AnnouncementContentInst.Announcement a)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            _currentId = a.Id;

            pnlHeader.BackColor = Maroon;

            Color iconCol = CatIconColor.ContainsKey(a.Category) ? CatIconColor[a.Category] : Color.Gray;
            Color iconBg = CatBgColor.ContainsKey(a.Category) ? CatBgColor[a.Category] : Color.WhiteSmoke;

            picCategoryIcon.BackColor = iconBg;
            picCategoryIcon.Tag = new object[] { iconCol, iconBg, a.Category };
            picCategoryIcon.Invalidate();

            lblCategoryPill.Text = a.Category;
            lblCategoryPill.ForeColor = iconCol;
            lblCategoryPill.BackColor = iconBg;
            ApplyRoundedRegion(lblCategoryPill, 10);

            lblUrgentBadge.Visible = a.IsUrgent;
            lblPinnedBadge.Visible = a.IsPinned;

            if (a.Status == "active")
            {
                lblStatusBadge.Text = "● Active";
                lblStatusBadge.ForeColor = Color.FromArgb(22, 163, 74);
                lblStatusBadge.BackColor = Color.FromArgb(220, 252, 231);
            }
            else
            {
                lblStatusBadge.Text = "● Inactive";
                lblStatusBadge.ForeColor = Color.FromArgb(100, 100, 100);
                lblStatusBadge.BackColor = Color.FromArgb(230, 230, 230);
            }
            ApplyRoundedRegion(lblStatusBadge, 10);

            lblTitle.Text = a.Title;
            lblDescription.Text = a.Description;
            lblAuthor.Text = "👤  " + a.InstructorName;
            lblDate.Text = "📅  " + a.Date.ToString("MMMM d, yyyy  •  h:mm tt");
            lblViewed.Text = $"👁  Viewed by {a.ViewedCount} of {a.TotalStudents} students";

            int pct = a.TotalStudents > 0
                ? (int)Math.Round(a.ViewedCount * 100.0 / a.TotalStudents)
                : 0;

            lblProgressPct.Text = $"{pct}%";
            pnlProgressFill.Width = (int)(pnlProgressTrack.Width * pct / 100.0);
            pnlProgressFill.BackColor = pct >= 75 ? Color.FromArgb(22, 163, 74)
                                      : pct >= 40 ? Color.FromArgb(186, 117, 23)
                                      : Maroon;

            bool hasFile = !string.IsNullOrWhiteSpace(a.AttachedFile);
            btnOpenFile.Tag = a.AttachedFile ?? string.Empty;
            btnEditFile.Tag = a.AttachedFile ?? string.Empty;
            pnlAttachment.Visible = hasFile;

            if (hasFile)
            {
                string ext = System.IO.Path.GetExtension(a.AttachedFile ?? "").ToLower();
                lblAttachName.Text = System.IO.Path.GetFileName(a.AttachedFile);
                lblAttachType.Text = ext switch
                {
                    ".pdf" => "PDF Document",
                    ".docx" => "Word Document",
                    ".pptx" => "PowerPoint Presentation",
                    ".png" => "PNG Image",
                    ".jpg" => "JPEG Image",
                    ".jpeg" => "JPEG Image",
                    _ => "File",
                };
                picFileIcon.BackColor = ext switch
                {
                    ".pdf" => Color.FromArgb(255, 235, 230),
                    ".docx" => Color.FromArgb(230, 241, 255),
                    ".pptx" => Color.FromArgb(255, 240, 230),
                    _ => Color.FromArgb(235, 235, 235),
                };
                picFileIcon.Tag = ext;
                picFileIcon.Invalidate();
            }

            ReflowDescription();
            Refresh();
        }

        private void ReflowDescription()
        {
            Size measured = TextRenderer.MeasureText(
                lblDescription.Text,
                lblDescription.Font,
                new Size(lblDescription.Width, 0),
                TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);

            lblDescription.Height = Math.Max(40, measured.Height + 10);

            lblAuthor.Top = lblDescription.Bottom + 20;
            lblDate.Top = lblAuthor.Top;
            lblViewed.Top = lblAuthor.Top;

            pnlProgressRow.Top = lblAuthor.Bottom + 12;

            int bottom = pnlProgressRow.Bottom + 20;

            if (pnlAttachment.Visible)
            {
                pnlAttachment.Top = pnlProgressRow.Bottom + 16;
                bottom = pnlAttachment.Bottom + 20;
            }

            this.Height = bottom;

            this.AutoScroll = false;

            if (Parent != null)
                Parent.PerformLayout();
        }

        private void ConfirmDelete()
        {
            if (_currentId < 0) return;
            var result = MessageBox.Show(
                $"Permanently delete this announcement?\n\nThis action cannot be undone.",
                "Delete Announcement",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                DeleteRequested?.Invoke(this, _currentId);
                ClosePanel();
            }
        }

        private void OpenAttachment()
        {
            string path = btnOpenFile.Tag as string;
            if (string.IsNullOrWhiteSpace(path)) return;
            try
            {
                Process.Start(new ProcessStartInfo { FileName = path, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open the file:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditAttachment()
        {
            string path = btnOpenFile.Tag as string;
            if (string.IsNullOrWhiteSpace(path)) return;

            string ext = System.IO.Path.GetExtension(path).ToLower();
            if (ext == ".pdf")
            {
                var res = MessageBox.Show(
                    "PDF files are typically read-only.\nDo you still want to open it for viewing?",
                    "Edit File",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);
                if (res != DialogResult.Yes) return;
            }

            try
            {
                Process.Start(new ProcessStartInfo { FileName = path, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open the file for editing:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClosePanel()
        {
            Visible = false;
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void DrawBorder(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.FromArgb(200, 200, 200), 1f);
            using var path = RoundedRectPath(new Rectangle(0, 0, Width - 1, Height - 1), 10);
            g.DrawPath(pen, path);
        }

        internal void PicCategoryIcon_Paint(object sender, PaintEventArgs e)
        {
            if (picCategoryIcon.Tag is not object[] arr || arr.Length < 3) return;
            Color iconCol = (Color)arr[0];
            Color iconBg = (Color)arr[1];
            string cat = arr[2] as string ?? "?";

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var b = new SolidBrush(iconBg))
                g.FillEllipse(b, 0, 0, picCategoryIcon.Width - 1, picCategoryIcon.Height - 1);

            string letter = cat.Length > 0 ? cat[..1] : "?";
            using var font = new Font("Segoe UI", 16f, FontStyle.Bold);
            using var brush = new SolidBrush(iconCol);
            var sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
            };
            g.DrawString(letter, font, brush,
                new RectangleF(0, 0, picCategoryIcon.Width, picCategoryIcon.Height), sf);
        }

        internal void PicFileIcon_Paint(object sender, PaintEventArgs e)
        {
            string ext = picFileIcon.Tag as string ?? string.Empty;
            string symbol = ext switch
            {
                ".pdf" => "PDF",
                ".docx" => "DOC",
                ".pptx" => "PPT",
                ".png" => "IMG",
                ".jpg" => "IMG",
                ".jpeg" => "IMG",
                _ => "FILE",
            };
            Color textCol = ext switch
            {
                ".pdf" => Color.FromArgb(200, 60, 30),
                ".docx" => Color.FromArgb(30, 90, 200),
                ".pptx" => Color.FromArgb(200, 90, 30),
                _ => Color.FromArgb(80, 80, 80),
            };
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using var font = new Font("Segoe UI", 8f, FontStyle.Bold);
            using var brush = new SolidBrush(textCol);
            var sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
            };
            g.DrawString(symbol, font, brush,
                new RectangleF(0, 0, picFileIcon.Width, picFileIcon.Height), sf);
        }

        private static GraphicsPath RoundedRectPath(Rectangle r, int rad)
        {
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, rad, rad, 180, 90);
            p.AddArc(r.Right - rad, r.Y, rad, rad, 270, 90);
            p.AddArc(r.Right - rad, r.Bottom - rad, rad, rad, 0, 90);
            p.AddArc(r.X, r.Bottom - rad, rad, rad, 90, 90);
            p.CloseFigure();
            return p;
        }

        private static void ApplyRoundedRegion(Control c, int radius)
        {
            using var path = RoundedRectPath(new Rectangle(0, 0, c.Width, c.Height), radius);
            c.Region = new Region(path);
        }
    }
}