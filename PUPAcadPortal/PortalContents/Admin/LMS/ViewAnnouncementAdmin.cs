using PUPAcadPortal.PortalContents.Admin.LMS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Misc.LMS
{
    public partial class ViewAnnouncementAdmin : UserControl
    {
        public event EventHandler<int>? EditRequested;
        public event EventHandler<int>? DeleteRequested;
        public event EventHandler? CloseRequested;

        private static readonly Dictionary<string, Color> CatIconColor = new()
        {
            ["General"] = Color.FromArgb(55, 138, 221),
            ["Academic"] = Color.FromArgb(99, 153, 34),
            ["Events"] = Color.FromArgb(212, 83, 126),
            ["Emergency"] = Color.FromArgb(200, 30, 30),
            ["Enrollment"] = Color.FromArgb(13, 154, 138)
        };

        private static readonly Dictionary<string, Color> CatBgColor = new()
        {
            ["General"] = Color.FromArgb(230, 241, 251),
            ["Academic"] = Color.FromArgb(234, 243, 222),
            ["Events"] = Color.FromArgb(251, 234, 240),
            ["Emergency"] = Color.FromArgb(255, 224, 224),
            ["Enrollment"] = Color.FromArgb(214, 244, 241)
        };

        private int _currentId = -1;

        public ViewAnnouncementAdmin()
        {
            InitializeComponent();
        }

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
            {
                _lblStatus.Text = "● Active";
                _lblStatus.ForeColor = Color.FromArgb(22, 163, 74);
                _lblStatus.BackColor = Color.FromArgb(220, 252, 231);
            }
            else
            {
                _lblStatus.Text = "● Inactive";
                _lblStatus.ForeColor = Color.FromArgb(100, 100, 100);
                _lblStatus.BackColor = Color.FromArgb(230, 230, 230);
            }
            ApplyRounded(_lblStatus, 10);

            _lblTitle.Text = a.Title;
            _lblDesc.Text = a.Description;
            _lblAuthor.Text = "👤  " + (string.IsNullOrWhiteSpace(a.InstructorName) ? "Admin" : a.InstructorName);
            _lblDate.Text = "📅  " + a.Date.ToString("MMMM d, yyyy  •  h:mm tt");

            var targets = new List<string>();
            if (a.NotifyStudents) targets.Add("Students");
            if (a.NotifyInstructors) targets.Add("Instructors");
            _lblNotify.Text = targets.Count > 0 ? "🔔  Notification sent to: " + string.Join(" & ", targets) : "🔕  No notification sent";

            _lblAttachment.Text = !string.IsNullOrWhiteSpace(a.AttachedFile) ? "📎  Attachment: " + a.AttachedFile : "📎  No attachment";
            _lblAttachment.ForeColor = !string.IsNullOrWhiteSpace(a.AttachedFile) ? Color.FromArgb(100, 60, 160) : Color.FromArgb(160, 160, 160);

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

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Visible = false;
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_currentId < 0) return;
            if (MessageBox.Show("Delete this announcement?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DeleteRequested?.Invoke(this, _currentId);
                Visible = false;
                CloseRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (_currentId >= 0) EditRequested?.Invoke(this, _currentId);
        }
    }
}