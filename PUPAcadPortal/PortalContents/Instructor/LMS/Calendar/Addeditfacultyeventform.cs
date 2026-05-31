using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PUPAcadPortal.Data;

namespace PUPAcadPortal.PortalContents.Instructor.LMS.Calendar
{
    /// <summary>
    /// Modal dialog for creating or editing a <see cref="FacultyCalendarEvent"/>.
    /// Shows all fields: type, title, date, time, course, room, description, files.
    /// </summary>
    public partial class AddEditFacultyEventForm : Form
    {
        // ── Result ───────────────────────────────────────────────────────────
        public FacultyCalendarEvent? ResultEvent { get; private set; }

        private readonly FacultyCalendarEvent? _existing;
        private readonly DateTime _defaultDate;
        private readonly List<string> _files = new();

        // ── Theme configuration ──────────────────────────────────────────────
        private static readonly Color Maroon = Color.FromArgb(136, 14, 79);
        private static readonly Font UIFont = new Font("Segoe UI", 9f);

        public AddEditFacultyEventForm(DateTime defaultDate,
                                       FacultyCalendarEvent? existing = null,
                                       FacultyEventType presetType = FacultyEventType.Class)
        {
            InitializeComponent();

            _defaultDate = defaultDate;
            _existing = existing;
            if (existing != null) _files.AddRange(existing.AttachedFiles);

            SetupDynamicUI(presetType);

            if (existing != null)
                PopulateFields(existing);
        }

        private void SetupDynamicUI(FacultyEventType presetType)
        {
            // Initial Window setup
            this.Text = _existing == null ? "Add Event" : "Edit Event";
            btnSave.Text = _existing == null ? "Save Event" : "Update Event";
            dtpDate.Value = _defaultDate;
            dtpStart.Value = DateTime.Today.AddHours(8);
            dtpEnd.Value = DateTime.Today.AddHours(9).AddMinutes(30);

            // Populate Event Types
            foreach (FacultyEventType t in Enum.GetValues<FacultyEventType>())
            {
                cboType.Items.Add(t);
            }
            cboType.SelectedItem = presetType;

            // Populate Courses
            cboCourse.Items.Add("General");
            foreach (var c in FacultyCalendarData.GetAllCourses())
            {
                if (!cboCourse.Items.Contains(c)) cboCourse.Items.Add(c);
            }

            // Add common course templates
            foreach (var c in new[] { "CS101 – Data Structures", "MATH201 – Calculus II", "PHYS101 – Physics I", "ENG201 – Technical Writing" })
            {
                if (!cboCourse.Items.Contains(c)) cboCourse.Items.Add(c);
            }
            cboCourse.Text = "General";

            // Apply visual improvements
            ApplyRoundedBorder(txtTitle);
            ApplyRoundedBorder(txtRoom);
            ApplyRoundedBorder(txtDesc);

            UpdateAccentColor();
            RefreshFileList();
        }

        // ── Helpers ──────────────────────────────────────────────────────────

        private void ApplyRoundedBorder(Control c)
        {
            c.Leave += (s, e) => c.Invalidate();
            c.Enter += (s, e) => c.Invalidate();
        }

        private void RefreshFileList()
        {
            lstFiles.Items.Clear();
            foreach (var f in _files) lstFiles.Items.Add(Path.GetFileName(f));
        }

        private void UpdateAccentColor()
        {
            if (cboType.SelectedItem is FacultyEventType t)
            {
                var ev = new FacultyCalendarEvent { Type = t };
                var color = ev.GetColor();

                pnlHeader.BackColor = color;
                btnSave.BackColor = color;

                lblHeaderTitle.Text = _existing == null
                    ? $"New {ev.GetTypeLabel()} Event"
                    : $"Edit {ev.GetTypeLabel()} Event";
            }
        }

        private void PopulateFields(FacultyCalendarEvent ev)
        {
            cboType.SelectedItem = ev.Type;
            txtTitle.Text = ev.Title;
            dtpDate.Value = ev.Date.Date;
            chkAllDay.Checked = ev.IsAllDay;

            if (!ev.IsAllDay)
            {
                if (TimeSpan.TryParse(ev.StartTime, out var s)) dtpStart.Value = DateTime.Today.Add(s);
                if (TimeSpan.TryParse(ev.EndTime, out var en)) dtpEnd.Value = DateTime.Today.Add(en);
            }

            cboCourse.Text = ev.Course;
            txtRoom.Text = ev.Room;
            txtDesc.Text = ev.Description;
            chkRecurring.Checked = ev.IsRecurring;
            RefreshFileList();
        }

        // ── Event Handlers ───────────────────────────────────────────────────

        private void CboType_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            e.DrawBackground();
            var t = (FacultyEventType)cboType.Items[e.Index]!;
            var ev = new FacultyCalendarEvent { Type = t };
            var dot = new Rectangle(e.Bounds.Left + 4, e.Bounds.Top + 6, 10, 10);
            e.Graphics.FillRectangle(new SolidBrush(ev.GetColor()), dot);
            e.Graphics.DrawString($" {ev.GetTypeIcon()} {ev.GetTypeLabel()}",
                UIFont, Brushes.Black,
                new PointF(e.Bounds.Left + 20, e.Bounds.Top + 3));
            e.DrawFocusRectangle();
        }

        private void CboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateAccentColor();
        }

        private void ChkAllDay_CheckedChanged(object sender, EventArgs e)
        {
            dtpStart.Enabled = !chkAllDay.Checked;
            dtpEnd.Enabled = !chkAllDay.Checked;
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter a title for this event.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitle.Focus();
                return;
            }

            ResultEvent = new FacultyCalendarEvent
            {
                Id = _existing?.Id ?? Guid.NewGuid(),
                Title = txtTitle.Text.Trim(),
                Description = txtDesc.Text.Trim(),
                Date = dtpDate.Value.Date,
                StartTime = chkAllDay.Checked ? "" : dtpStart.Value.ToString("HH:mm"),
                EndTime = chkAllDay.Checked ? "" : dtpEnd.Value.ToString("HH:mm"),
                Type = cboType.SelectedItem is FacultyEventType t ? t : FacultyEventType.Class,
                Course = cboCourse.Text.Trim(),
                Room = txtRoom.Text.Trim(),
                IsAllDay = chkAllDay.Checked,
                IsRecurring = chkRecurring.Checked,
                IsAutoSynced = _existing?.IsAutoSynced ?? false,
                AttachedFiles = new List<string>(_files),
            };

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtnAddFile_Click(object? sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog { Multiselect = true, Title = "Attach files to event" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _files.AddRange(ofd.FileNames);
                RefreshFileList();
            }
        }

        private void BtnRemoveFile_Click(object sender, EventArgs e)
        {
            if (lstFiles.SelectedIndex >= 0)
            {
                _files.RemoveAt(lstFiles.SelectedIndex);
                RefreshFileList();
            }
        }
    }
}