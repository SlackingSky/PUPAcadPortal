using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using PUPAcadPortal.Models; // Ensure this is included for AppDbContext

namespace PUPAcadPortal
{
    public partial class AnnouncementInbox : UserControl
    {
        public event EventHandler? CloseRequested;
        private readonly int _currentUserId; // Added to track the role
        private readonly int _currentUserRoleId; // The ID used to filter messages

        private static readonly Color Maroon = Color.FromArgb(139, 0, 0);
        private static readonly Color LightBg = Color.FromArgb(248, 248, 249);

        private static readonly Dictionary<string, Color> TagColor = new()
        {
            ["General"] = Color.FromArgb(55, 138, 221),
            ["Policy"] = Color.FromArgb(127, 119, 221),
            ["Event"] = Color.FromArgb(212, 83, 126),
            ["Urgent"] = Color.Firebrick,
        };

        private static readonly Dictionary<string, Color> TagBg = new()
        {
            ["General"] = Color.FromArgb(230, 241, 251),
            ["Policy"] = Color.FromArgb(238, 237, 254),
            ["Event"] = Color.FromArgb(251, 234, 240),
            ["Urgent"] = Color.FromArgb(255, 235, 235),
        };

        private List<InboxMessage> _messages = new();
        private InboxMessage? _selected;

        // UPDATED CONSTRUCTOR: Now requires the role of the user
        public AnnouncementInbox(int userRoleId)
        {
            InitializeComponent();
            _currentUserRoleId = userRoleId;

            // Instead of seeding static data, we fetch from DB
            FetchMessagesFromDb();

            RenderList();
            ShowEmptyDetail();

            _flpList.Size = new Size(_pnlLeft.Width, Math.Max(40, _pnlLeft.Height - 40));
            MakeRound(_lblUnreadBadge, 8);
        }

        // NEW METHOD: This replaces SeedMessages()
        private void FetchMessagesFromDb()
        {
            _messages.Clear();
            try
            {
                using (var context = new AppDbContext())
                {
                    // DEBUG: See what role we are searching for
                    System.Diagnostics.Debug.WriteLine($"[Inbox Debug] Searching for Role ID: {_currentUserRoleId}");

                    var allAnnouncements = context.Announcements.ToList();
                    System.Diagnostics.Debug.WriteLine($"[Inbox Debug] Total announcements in DB: {allAnnouncements.Count}");

                    var dbAnnouncements = allAnnouncements
                        .Where(a => a.TargetRoleId == _currentUserRoleId)
                        .ToList();

                    System.Diagnostics.Debug.WriteLine($"[Inbox Debug] Found {dbAnnouncements.Count} announcements matching role.");

                    foreach (var ann in dbAnnouncements)
                    {
                        _messages.Add(new InboxMessage
                        {
                            Id = ann.AnnouncementId,
                            TargetRoleId = ann.TargetRoleId,
                            Subject = ann.Title,
                            Body = ann.Content,
                            SenderName = ann.CreatedByUser?.FirstName + " " + ann.CreatedByUser?.LastName ?? "Admin Office",
                            SenderRole = "Administrator",
                            ReceivedAt = ann.PostedDate,
                            Tag = ann.Category,
                            IsRead = false,
                            IsStarred = false
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error fetching inbox: {ex.Message}");
            }
        }


        private void RenderList(string? search = null)
        {
            _flpList.SuspendLayout();
            _flpList.Controls.Clear();

            var msgs = string.IsNullOrWhiteSpace(search)
                ? _messages
                : _messages.Where(m =>
                    m.Subject.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    m.SenderName.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var msg in msgs.OrderByDescending(m => m.ReceivedAt))
            {
                var row = BuildRow(msg);
                _flpList.Controls.Add(row);
            }

            UpdateUnreadBadge();
            _flpList.ResumeLayout();
        }

        private Panel BuildRow(InboxMessage msg)
        {
            bool isSelected = _selected?.Id == msg.Id;
            var row = new Panel
            {
                Width = 265,
                Height = 68,
                BackColor = isSelected ? Color.FromArgb(240, 235, 235) : Color.White,
                Margin = new Padding(4, 0, 4, 4),
                Cursor = Cursors.Hand,
                Tag = msg,
            };

            row.Paint += (s, pe) =>
            {
                using var pen = new Pen(Color.FromArgb(230, 230, 230), 1f);
                pe.Graphics.DrawRectangle(pen, 0, 0, row.Width - 1, row.Height - 1);
                if (isSelected)
                {
                    using var accent = new SolidBrush(Maroon);
                    pe.Graphics.FillRectangle(accent, 0, 0, 3, row.Height);
                }
            };

            if (!msg.IsRead)
            {
                var dot = new Panel { Size = new Size(8, 8), Location = new Point(6, 14), BackColor = Maroon };
                MakeCircle(dot);
                row.Controls.Add(dot);
            }

            if (msg.IsStarred)
            {
                row.Controls.Add(new Label { Text = "★", Font = new Font("Segoe UI", 9F), ForeColor = Color.Goldenrod, AutoSize = true, Location = new Point(6, 44), BackColor = Color.Transparent });
            }

            row.Controls.Add(new Label
            {
                Text = msg.Subject,
                Font = new Font("Segoe UI", 9F, msg.IsRead ? FontStyle.Regular : FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 20, 20),
                AutoSize = false,
                Size = new Size(220, 18),
                Location = new Point(18, 8),
                AutoEllipsis = true,
                BackColor = Color.Transparent,
            });

            row.Controls.Add(new Label
            {
                Text = msg.SenderName,
                Font = new Font("Segoe UI", 7.5F),
                ForeColor = Color.FromArgb(100, 100, 100),
                AutoSize = false,
                Size = new Size(160, 16),
                Location = new Point(18, 28),
                BackColor = Color.Transparent,
            });

            string timeStr = msg.ReceivedAt.Date == DateTime.Today
                ? msg.ReceivedAt.ToString("h:mm tt")
                : msg.ReceivedAt.ToString("MMM d");

            row.Controls.Add(new Label
            {
                Text = timeStr,
                Font = new Font("Segoe UI", 7.5F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(200, 8),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.TopRight,
            });

            Color tCol = TagColor.GetValueOrDefault(msg.Tag, Color.Gray);
            Color tBg = TagBg.GetValueOrDefault(msg.Tag, Color.WhiteSmoke);
            var pill = new Label
            {
                Text = msg.Tag,
                Font = new Font("Segoe UI", 7F, FontStyle.Bold),
                ForeColor = tCol,
                BackColor = tBg,
                AutoSize = false,
                Size = new Size(TextRenderer.MeasureText(msg.Tag, new Font("Segoe UI", 7F, FontStyle.Bold)).Width + 10, 16),
                Location = new Point(18, 46),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            MakeRound(pill, 8);
            row.Controls.Add(pill);

            EventHandler clickH = (s, ev) =>
            {
                _selected = msg;
                if (!msg.IsRead) { msg.IsRead = true; UpdateUnreadBadge(); }
                ShowDetail(msg);
                RenderList(_txtSearch.Text);
            };
            row.Click += clickH;
            foreach (Control c in row.Controls) c.Click += clickH;

            return row;
        }

        // ... (Keep ShowEmptyDetail, ShowDetail, UpdateUnreadBadge, and all other helper methods exactly as they were) ...

        private void ShowEmptyDetail()
        {
            _lblDetailSubject.Text = "Select a message to read";
            _lblDetailMeta.Text = "";
            _lblDetailBody.Text = "Your inbox messages will appear here.\nClick any message on the left to view its contents.";
            _lblDetailTag.Visible = false;
            _btnStar.Enabled = _btnMarkRead.Enabled = _btnDelete.Enabled = false;
        }

        private void ShowDetail(InboxMessage msg)
        {
            Color tCol = TagColor.GetValueOrDefault(msg.Tag, Color.Gray);
            Color tBg = TagBg.GetValueOrDefault(msg.Tag, Color.WhiteSmoke);
            _lblDetailTag.Text = msg.Tag;
            _lblDetailTag.ForeColor = tCol;
            _lblDetailTag.BackColor = tBg;
            _lblDetailTag.Visible = true;
            MakeRound(_lblDetailTag, 10);

            _lblDetailSubject.Text = msg.Subject;
            _lblDetailMeta.Text = $"From: {msg.SenderName}  ({msg.SenderRole})   ·   {msg.ReceivedAt:MMMM d, yyyy  •  h:mm tt}";
            _lblDetailBody.Text = msg.Body;

            _btnStar.Text = msg.IsStarred ? "★  Starred" : "☆  Star";
            _btnStar.ForeColor = msg.IsStarred ? Color.Goldenrod : Color.FromArgb(130, 100, 0);
            _btnMarkRead.Text = msg.IsRead ? "✓  Mark Unread" : "✓  Mark Read";
            _btnStar.Enabled = _btnMarkRead.Enabled = _btnDelete.Enabled = true;
        }

        private void UpdateUnreadBadge()
        {
            int unread = _messages.Count(m => !m.IsRead);
            _lblUnreadBadge.Text = unread.ToString();
            _lblUnreadBadge.Visible = unread > 0;
        }

        private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);
        private void TxtSearch_TextChanged(object? sender, EventArgs e) => RenderList(_txtSearch.Text);

        private void BtnStar_Click(object? sender, EventArgs e)
        {
            if (_selected == null) return;
            _selected.IsStarred = !_selected.IsStarred;
            ShowDetail(_selected);
            RenderList(_txtSearch.Text);
        }

        private void BtnMarkRead_Click(object? sender, EventArgs e)
        {
            if (_selected == null) return;
            _selected.IsRead = !_selected.IsRead;
            ShowDetail(_selected);
            RenderList(_txtSearch.Text);
            UpdateUnreadBadge();
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (_selected == null) return;
            if (MessageBox.Show($"Delete \"{_selected.Subject}\"?", "Delete Message", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                _messages.Remove(_selected);
                _selected = null;
                ShowEmptyDetail();
                RenderList(_txtSearch.Text);
            }
        }

        private void AnnouncementInbox_Resize(object? sender, EventArgs e)
        {
            _flpList.Size = new Size(_pnlLeft.Width, Math.Max(40, _pnlLeft.Height - 40));
            if (_lblUnreadBadge.Visible) MakeRound(_lblUnreadBadge, 8);
            if (_lblDetailTag.Visible) MakeRound(_lblDetailTag, 10);
        }

        private static void MakeRound(Control c, int r)
        {
            if (c.Width <= 0 || c.Height <= 0) return;
            using var path = new GraphicsPath();
            var rect = new Rectangle(0, 0, c.Width, c.Height);
            path.AddArc(rect.X, rect.Y, r, r, 180, 90);
            path.AddArc(rect.Right - r, rect.Y, r, r, 270, 90);
            path.AddArc(rect.Right - r, rect.Bottom - r, r, r, 0, 90);
            path.AddArc(rect.X, rect.Bottom - r, r, r, 90, 90);
            path.CloseFigure();
            c.Region = new Region(path);
        }

        private static void MakeCircle(Panel p)
        {
            using var path = new GraphicsPath();
            path.AddEllipse(0, 0, p.Width, p.Height);
            p.Region = new Region(path);
        }
    }
}
