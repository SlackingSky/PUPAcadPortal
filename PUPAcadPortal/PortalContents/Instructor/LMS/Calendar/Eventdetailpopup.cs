using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using PUPAcadPortal.Data;

namespace PUPAcadPortal.PortalContents.Instructor.LMS.Calendar
{
    /// <summary>
    /// Rich read-only popup for viewing a <see cref="FacultyCalendarEvent"/>.
    /// Provides Edit and Delete shortcuts.
    /// </summary>
    public partial class EventDetailPopup : Form
    {
        public enum ResultAction { None, Edit, Delete }
        public ResultAction Action { get; private set; } = ResultAction.None;

        private static readonly Color Maroon = Color.FromArgb(136, 14, 79);
        private static readonly Font UIFont = new Font("Segoe UI", 9f);
        private static readonly Font BoldFont = new Font("Segoe UI", 9f, FontStyle.Bold);
        private static readonly Font SmFont = new Font("Segoe UI", 8f);

        public EventDetailPopup(FacultyCalendarEvent ev)
        {
            Text = "Event Details";
            Size = new Size(480, 400);
            MinimumSize = new Size(420, 340);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.White;
            Font = UIFont;

            Color accent = ev.GetColor();

            // ── Coloured header strip ─────────────────────────────────────────
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 64, BackColor = accent };
            var lblIcon = new Label
            {
                Text = ev.GetTypeIcon(),
                Left = 14,
                Top = 10,
                Width = 40,
                Height = 40,
                Font = new Font("Segoe UI", 20f),
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            var lblTitle = new Label
            {
                Text = ev.Title,
                Left = 60,
                Top = 8,
                Width = 380,
                Height = 30,
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoEllipsis = true,
                AutoSize = false,
            };
            var lblType = new Label
            {
                Text = ev.GetTypeLabel() + (ev.IsAutoSynced ? "  🔄 LMS Synced" : ""),
                Left = 60,
                Top = 40,
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(230, 230, 230),
                BackColor = Color.Transparent,
            };
            pnlHeader.Controls.AddRange(new Control[] { lblIcon, lblTitle, lblType });
            Controls.Add(pnlHeader);

            // ── Body ──────────────────────────────────────────────────────────
            var body = new Panel { Left = 0, Top = 64, Width = ClientSize.Width, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom };
            Controls.Add(body);

            int y = 14;
            const int ROW_H = 28;

            void AddRow(string icon, string labelText, string value)
            {
                if (string.IsNullOrWhiteSpace(value)) return;
                var lIco = new Label { Text = icon, Left = 14, Top = y, Width = 22, Height = ROW_H, Font = new Font("Segoe UI", 11f), TextAlign = ContentAlignment.MiddleCenter, AutoSize = false };
                var lLbl = new Label { Text = labelText, Left = 38, Top = y, Width = 90, Height = ROW_H, Font = BoldFont, ForeColor = Color.FromArgb(80, 80, 80), AutoSize = false, TextAlign = ContentAlignment.MiddleLeft };
                var lVal = new Label { Text = value, Left = 132, Top = y, Width = body.Width - 148, Height = ROW_H, Font = UIFont, ForeColor = Color.FromArgb(40, 40, 40), AutoSize = false, AutoEllipsis = true, TextAlign = ContentAlignment.MiddleLeft, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
                body.Controls.AddRange(new Control[] { lIco, lLbl, lVal });
                y += ROW_H;
            }

            // Date
            string dateStr = ev.Date.ToString("dddd, MMMM dd, yyyy");
            AddRow("📅", "Date", dateStr);

            // Time
            if (!ev.IsAllDay && !string.IsNullOrEmpty(ev.StartTime))
            {
                string timeStr = ev.StartTime;
                if (!string.IsNullOrEmpty(ev.EndTime)) timeStr += " – " + ev.EndTime;
                AddRow("🕐", "Time", timeStr);
            }
            else if (ev.IsAllDay)
            {
                AddRow("🕐", "Time", "All-day");
            }

            // Course
            if (!string.IsNullOrEmpty(ev.Course))
                AddRow("📚", "Course", ev.Course);

            // Room
            if (!string.IsNullOrEmpty(ev.Room))
                AddRow("📍", "Room", ev.Room);

            // Recurrence
            if (ev.IsRecurring)
                AddRow("🔁", "Recurrence", "Weekly");

            y += 4;

            // Separator
            var sep = new Panel { Left = 14, Top = y, Width = body.Width - 28, Height = 1, BackColor = Color.FromArgb(230, 230, 230), Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top };
            body.Controls.Add(sep);
            y += 8;

            // Description
            if (!string.IsNullOrEmpty(ev.Description))
            {
                var lblDescHdr = new Label { Text = "Description", Left = 14, Top = y, AutoSize = true, Font = BoldFont, ForeColor = Color.FromArgb(80, 80, 80) };
                y += 22;
                var lblDesc = new Label
                {
                    Text = ev.Description,
                    Left = 14,
                    Top = y,
                    Width = body.Width - 28,
                    Font = UIFont,
                    ForeColor = Color.FromArgb(60, 60, 60),
                    AutoSize = false,
                    Height = 48,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                };
                body.Controls.AddRange(new Control[] { lblDescHdr, lblDesc });
                y += 52;
            }

            // Attached files
            if (ev.AttachedFiles.Count > 0)
            {
                y += 4;
                var lblFilesHdr = new Label { Text = "Attached Files", Left = 14, Top = y, AutoSize = true, Font = BoldFont, ForeColor = Color.FromArgb(80, 80, 80) };
                body.Controls.Add(lblFilesHdr);
                y += 22;

                foreach (var f in ev.AttachedFiles)
                {
                    var lnk = new LinkLabel
                    {
                        Text = Path.GetFileName(f),
                        Left = 14,
                        Top = y,
                        AutoSize = true,
                        Font = SmFont,
                        Tag = f,
                    };
                    lnk.Click += (s, e2) =>
                    {
                        try { System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(f) { UseShellExecute = true }); }
                        catch { MessageBox.Show("Could not open file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                    };
                    body.Controls.Add(lnk);
                    y += 20;
                }
            }

            body.Height = ClientSize.Height - 64;
            body.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            // ── Action buttons ────────────────────────────────────────────────
            var pnlActions = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 46,
                BackColor = Color.FromArgb(248, 248, 248),
            };
            pnlActions.Paint += (s, pe) =>
            {
                using var p = new Pen(Color.FromArgb(220, 220, 220));
                pe.Graphics.DrawLine(p, 0, 0, pnlActions.Width, 0);
            };

            var btnClose = new Button
            {
                Text = "Close",
                Left = 10,
                Top = 8,
                Width = 80,
                Height = 30,
                FlatStyle = FlatStyle.Flat,
                Font = UIFont,
                Cursor = Cursors.Hand,
            };
            btnClose.Click += (s, e2) => { Action = ResultAction.None; Close(); };

            var btnEdit = new Button
            {
                Text = "✏  Edit",
                Top = 8,
                Width = 88,
                Height = 30,
                BackColor = Color.FromArgb(21, 101, 192),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = UIFont,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Left = pnlActions.Width - 196;
            btnEdit.Click += (s, e2) =>
            {
                if (ev.IsAutoSynced)
                {
                    MessageBox.Show("This event is synced from the LMS and cannot be edited here.\nEdit it in the course activities section.",
                        "LMS Event", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                Action = ResultAction.Edit;
                DialogResult = DialogResult.OK;
                Close();
            };

            var btnDelete = new Button
            {
                Text = "🗑  Delete",
                Top = 8,
                Width = 88,
                Height = 30,
                BackColor = Color.FromArgb(183, 28, 28),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = UIFont,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Left = pnlActions.Width - 100;
            btnDelete.Click += (s, e2) =>
            {
                if (ev.IsAutoSynced)
                {
                    MessageBox.Show("LMS-synced events cannot be deleted here.", "LMS Event",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (MessageBox.Show($"Delete \"{ev.Title}\"?", "Confirm Delete",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    Action = ResultAction.Delete;
                    DialogResult = DialogResult.OK;
                    Close();
                }
            };

            pnlActions.Controls.AddRange(new Control[] { btnClose, btnEdit, btnDelete });
            pnlActions.Resize += (s, e2) =>
            {
                btnEdit.Left = pnlActions.Width - 196;
                btnDelete.Left = pnlActions.Width - 100;
            };
            Controls.Add(pnlActions);

            AcceptButton = btnClose;
        }
    }
}