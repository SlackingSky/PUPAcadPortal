/*
 * UrDay.cs  –  Calendar Day Cell (Faculty Edition)
 * Extends the original UrDay to support FacultyCalendarData alongside
 * the legacy SharedCalendarData, and adds drag-drop source capability
 * so events can be dragged to other cells on the monthly grid.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using PUPAcadPortal.PortalForms;
using PUPAcadPortal.Data;
using PUPAcadPortal.PortalContents.Student.LMS.Calendar;

namespace PUPAcadPortal
{
    public partial class UrDay : UserControl
    {
        // ── Static events ────────────────────────────────────────────────────
        public static event Action<DateTime>? DaySelected;

        // ── Drag-drop: fired when user drags an event onto another cell ──────
        public static event Action<Guid, DateTime>? EventDropped;

        // ── Fields ────────────────────────────────────────────────────────────
        private string _day;
        private DateTime _fullDate;
        private Label _lblHoliday;
        private Label _lblHolidayInline;
        private bool _isCurrentMonth;
        private bool _isStudent;
        private bool _isSelected;
        private bool _isHovered;
        private bool _isDragOver;       // highlight when an event hovers over this cell

        private readonly List<Button> _eventPills = new();
        private Button? _btnEventsDropdown;
        private Button? _btnFacultyDrop;  // second dropdown for faculty events
        private Button? _btnNoteDropdown;
        private ContextMenuStrip _ctxMenu;

        // Drag state
        private FacultyCalendarEvent? _dragSourceEvent;
        private bool _isDraggingEvent;
        private Point _dragStartPt;
        private Point _mouseDownPt;

        // ── Theme ─────────────────────────────────────────────────
        private static readonly Color C_Primary = Color.FromArgb(128, 0, 0);
        private static readonly Color C_Selected = Color.FromArgb(255, 245, 245);
        private static readonly Color C_Dim = Color.FromArgb(248, 248, 248);

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string AnnouncementText
        {
            get => lblAnnouncement.Text;
            set
            {
                lblAnnouncement.Text = value ?? "";
                lblAnnouncement.Visible = !string.IsNullOrWhiteSpace(value);
                RepositionPills();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime CellDate => _fullDate;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                BackColor = value ? C_Selected : Color.White;
                Invalidate();
            }
        }

        // ── Constructor ───────────────────────────────────────────────────────
        public UrDay(string day, int year = 0, int month = 0,
                     bool isCurrentMonth = true, string holiday = "",
                     bool isStudent = false)
        {
            InitializeComponent();

            _day = day;
            _isCurrentMonth = isCurrentMonth;
            _isStudent = isStudent;
            chkSelect.Hide();

            Margin = new Padding(0);
            Dock = DockStyle.None;
            BackColor = Color.White;
            Cursor = Cursors.Hand;

            AllowDrop = true;   // accept drops from other cells

            if (year == 0) year = SharedCalendarData.CurrentYear;
            if (month == 0) month = SharedCalendarData.CurrentMonth;

            lblDay.AutoSize = false;
            lblDay.Size = new Size(28, 28);
            lblDay.Location = new Point(4, 4);
            lblDay.Font = new Font("Segoe UI", 9f);
            lblDay.TextAlign = ContentAlignment.MiddleCenter;
            lblDay.BackColor = Color.Transparent;
            lblDay.Text = day;

            if (string.IsNullOrEmpty(day))
            {
                BackColor = C_Dim;
                return;
            }

            int dayInt = int.Parse(day);
            _fullDate = new DateTime(year, month, dayInt);

            // Day number colour
            if (!isCurrentMonth)
                lblDay.ForeColor = Color.FromArgb(200, 200, 200);
            else if (_fullDate.DayOfWeek == DayOfWeek.Sunday)
                lblDay.ForeColor = Color.Crimson;
            else
                lblDay.ForeColor = Color.FromArgb(40, 40, 40);

            // Today circle
            if (DateTime.Now.Date == _fullDate.Date)
            {
                var gp = new GraphicsPath();
                gp.AddEllipse(0, 0, lblDay.Width, lblDay.Height);
                lblDay.Region = new Region(gp);
                lblDay.BackColor = C_Primary;
                lblDay.ForeColor = Color.White;
            }

            // Holiday badge
            if (!string.IsNullOrEmpty(holiday))
            {
                _lblHolidayInline = new Label
                {
                    AutoSize = false,
                    AutoEllipsis = true,
                    Text = holiday,
                    Font = new Font("Segoe UI", 6.5f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(30, 120, 60),
                    BackColor = Color.FromArgb(220, 245, 220),
                    Location = new Point(36, 6),
                    Height = 18,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(2, 0, 2, 0),
                    Cursor = Cursors.Help,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                    Width = Width - 38,
                };
                _lblHolidayInline.Click += (s, e) =>
                    MessageBox.Show(holiday, "Holiday — " + _fullDate.ToString("MMMM dd, yyyy"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                new ToolTip().SetToolTip(_lblHolidayInline, "Philippine Holiday — " + holiday);
                Controls.Add(_lblHolidayInline);
                _lblHolidayInline.BringToFront();
            }

            // Wire events
            lblNote.Click += (s, e) => OpenNotesDialog();
            lblAnnouncement.Click += (s, e) => OpenAnnouncementInfo();
            Click += Cell_Click;
            lblDay.Click += Cell_Click;
            MouseEnter += (s, e) => { if (_isCurrentMonth && !string.IsNullOrEmpty(_day)) { _isHovered = true; Invalidate(); } };
            MouseLeave += (s, e) => { _isHovered = false; Invalidate(); };
            lblDay.MouseEnter += (s, e) => { if (_isCurrentMonth && !string.IsNullOrEmpty(_day)) { _isHovered = true; Invalidate(); } };
            lblDay.MouseLeave += (s, e) => { _isHovered = false; Invalidate(); };
            MouseClick += Cell_MouseClick;

            // Drag-drop receive
            DragEnter += (s, e) =>
            {
                if (e.Data?.GetDataPresent(typeof(Guid)) == true && _isCurrentMonth)
                {
                    e.Effect = DragDropEffects.Move;
                    _isDragOver = true;
                    Invalidate();
                }
            };
            DragLeave += (s, e) => { _isDragOver = false; Invalidate(); };
            DragDrop += (s, e) =>
            {
                _isDragOver = false;
                Invalidate();
                if (e.Data?.GetData(typeof(Guid)) is Guid id)
                    EventDropped?.Invoke(id, _fullDate);
            };

            Resize += (s, e) =>
            {
                if (_lblHolidayInline != null) _lblHolidayInline.Width = Width - 38;
                if (_btnEventsDropdown != null) _btnEventsDropdown.Width = Math.Max(10, Width - 4);
                if (_btnFacultyDrop != null) _btnFacultyDrop.Width = Math.Max(10, Width - 4);
                if (_btnNoteDropdown != null) _btnNoteDropdown.Width = Math.Max(10, Width - 4);
                foreach (var btn in _eventPills) btn.Width = Math.Max(10, Width - 4);
            };

            BuildContextMenu();
            RepositionPills();
            LoadNote();
            LoadAnnouncement();
            RefreshEventPills();
        }

        // ══════════════════════════════════════════════════════════════════════
        //  CONTEXT MENU
        // ══════════════════════════════════════════════════════════════════════
        private void BuildContextMenu()
        {
            _ctxMenu = new ContextMenuStrip();

            // ── Faculty event types ──────────────────────────────────────────
            var addEvent = new ToolStripMenuItem(_isStudent ? "🗓  Add My Event" : "🗓  Add Event");
            addEvent.DropDownItems.Add(new ToolStripMenuItem("📘 Class", null, (s, e) => QuickAddFacultyEvent(FacultyEventType.Class)));
            addEvent.DropDownItems.Add(new ToolStripMenuItem("📋 Activity", null, (s, e) => QuickAddFacultyEvent(FacultyEventType.Activity)));
            addEvent.DropDownItems.Add(new ToolStripMenuItem("📝 Quiz", null, (s, e) => QuickAddFacultyEvent(FacultyEventType.Quiz)));
            addEvent.DropDownItems.Add(new ToolStripMenuItem("📄 Long Quiz", null, (s, e) => QuickAddFacultyEvent(FacultyEventType.LongQuiz)));
            addEvent.DropDownItems.Add(new ToolStripMenuItem("🎓 Exam", null, (s, e) => QuickAddFacultyEvent(FacultyEventType.Exam)));
            addEvent.DropDownItems.Add(new ToolStripMenuItem("📌 Deadline", null, (s, e) => QuickAddFacultyEvent(FacultyEventType.Deadline)));
            addEvent.DropDownItems.Add(new ToolStripMenuItem("🩺 Consultation", null, (s, e) => QuickAddFacultyEvent(FacultyEventType.Consultation)));
            addEvent.DropDownItems.Add(new ToolStripSeparator());
            addEvent.DropDownItems.Add(new ToolStripMenuItem("🗒 Personal Note", null, (s, e) => QuickAddFacultyEvent(FacultyEventType.PersonalNote)));

            var addNote = new ToolStripMenuItem("🗒  Add / Edit Note", null, (s, e) => OpenNotesDialog());
            var mnuRemove = new ToolStripMenuItem("🗑  Remove Event");

            _ctxMenu.Items.AddRange(new ToolStripItem[]
            {
                addEvent,
                new ToolStripSeparator(),
                addNote,
                new ToolStripSeparator(),
                mnuRemove,
            });

            _ctxMenu.Opening += (s, e) =>
            {
                mnuRemove.DropDownItems.Clear();

                // Legacy events
                var legacy = _isStudent
                    ? SharedCalendarData.GetStudentEventsForDate(_fullDate)
                    : SharedCalendarData.GetEventsForDate(_fullDate);

                // Faculty events
                var faculty = FacultyCalendarData.GetEventsForDate(_fullDate)
                    .Where(ev => !ev.IsAutoSynced)
                    .ToList();

                mnuRemove.Enabled = _isCurrentMonth && (legacy.Count + faculty.Count) > 0;

                foreach (var ev in legacy)
                {
                    var cap = ev;
                    var item = new ToolStripMenuItem($"{cap.GetIcon()} [{cap.GetTypeLabel()}]  {cap.Title}");
                    item.Click += (ss, ee) =>
                    {
                        if (MessageBox.Show($"Remove \"{cap.Title}\" from {_fullDate:MMMM dd, yyyy}?",
                                            "Confirm Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            if (_isStudent) SharedCalendarData.RemoveStudentEvent(_fullDate, cap);
                            else SharedCalendarData.RemoveEvent(_fullDate, cap);
                            RefreshEventPills();
                            DaySelected?.Invoke(_fullDate);
                        }
                    };
                    mnuRemove.DropDownItems.Add(item);
                }

                foreach (var ev in faculty)
                {
                    var cap = ev;
                    var item = new ToolStripMenuItem($"{cap.GetTypeIcon()} [{cap.GetTypeLabel()}]  {cap.Title}");
                    item.Click += (ss, ee) =>
                    {
                        if (MessageBox.Show($"Remove \"{cap.Title}\" from {_fullDate:MMMM dd, yyyy}?",
                                            "Confirm Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            FacultyCalendarData.RemoveEvent(cap.Id);
                            RefreshEventPills();
                            DaySelected?.Invoke(_fullDate);
                        }
                    };
                    mnuRemove.DropDownItems.Add(item);
                }
            };

            ContextMenuStrip = _ctxMenu;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  EVENT PILLS
        // ══════════════════════════════════════════════════════════════════════
        public void RefreshEventPills()
        {
            // Remove old
            foreach (var b in _eventPills) Controls.Remove(b);
            _eventPills.Clear();
            if (_btnEventsDropdown != null) { Controls.Remove(_btnEventsDropdown); _btnEventsDropdown = null; }
            if (_btnFacultyDrop != null) { Controls.Remove(_btnFacultyDrop); _btnFacultyDrop = null; }

            // ── Legacy (SharedCalendarData) events ───────────────────────────
            var legacyEvents = SharedCalendarData.GetEventsForDate(_fullDate);
            var personalEvents = _isStudent ? SharedCalendarData.GetStudentEventsForDate(_fullDate)
                                            : new List<CalendarEvent>();
            int legacyCount = legacyEvents.Count + personalEvents.Count;

            if (legacyCount > 0)
            {
                _btnEventsDropdown = MakeGroupedDropdown($"📅 Events ({legacyCount})  ▾", C_Primary);
                var capShared = new List<CalendarEvent>(legacyEvents);
                var capPersonal = new List<CalendarEvent>(personalEvents);

                _btnEventsDropdown.Click += (s, e) =>
                {
                    var cms = new ContextMenuStrip { Font = new Font("Segoe UI", 8.5f) };
                    foreach (var ev in capShared)
                    {
                        string line = BuildEventLine(ev.GetTypeLabel(), ev.Title, ev.StartTime, ev.EndTime, ev.Room, ev.Course);
                        var item = new ToolStripMenuItem($"{ev.GetIcon()} {line}") { ForeColor = ev.GetColor(), Enabled = false };
                        cms.Items.Add(item);
                    }
                    if (capPersonal.Count > 0)
                    {
                        if (capShared.Count > 0) cms.Items.Add(new ToolStripSeparator());
                        foreach (var ev in capPersonal)
                        {
                            string line = BuildEventLine("MY " + ev.GetTypeLabel(), ev.Title, ev.StartTime, ev.EndTime, "", ev.Course);
                            cms.Items.Add(new ToolStripMenuItem($"{ev.GetIcon()} {line}") { ForeColor = Color.FromArgb(80, 80, 80), Enabled = false });
                        }
                    }
                    cms.Show(_btnEventsDropdown, 0, _btnEventsDropdown!.Height);
                };
                Controls.Add(_btnEventsDropdown);
                _btnEventsDropdown.BringToFront();
            }

            // ── Faculty (FacultyCalendarData) events ─────────────────────────
            var facultyEvents = FacultyCalendarData.GetEventsForDate(_fullDate);
            if (facultyEvents.Count > 0)
            {
                // Group by first type for colour
                Color baseColor = facultyEvents[0].GetColor();

                // Use a distinct colour per type if all same, else dark primary
                bool mixed = facultyEvents.Select(ev => ev.Type).Distinct().Count() > 1;
                Color btnColor = mixed ? Color.FromArgb(100, C_Primary.R, C_Primary.G, C_Primary.B) : baseColor;

                string label = facultyEvents.Count == 1
                    ? $"{facultyEvents[0].GetTypeIcon()} {TruncateStr(facultyEvents[0].Title, 18)}  ▾"
                    : $"📅 +{facultyEvents.Count} events  ▾";

                _btnFacultyDrop = MakeGroupedDropdown(label, btnColor);

                // Enable drag FROM this button
                _btnFacultyDrop.MouseDown += FacultyDrop_MouseDown;
                _btnFacultyDrop.MouseMove += FacultyDrop_MouseMove;
                _btnFacultyDrop.MouseUp += FacultyDrop_MouseUp;

                var capFaculty = new List<FacultyCalendarEvent>(facultyEvents);
                _btnFacultyDrop.Click += (s, e) =>
                {
                    var cms = new ContextMenuStrip { Font = new Font("Segoe UI", 8.5f) };
                    foreach (var ev in capFaculty)
                    {
                        string line = BuildEventLine(ev.GetTypeLabel(), ev.Title, ev.StartTime, ev.EndTime, ev.Room, "");
                        if (ev.IsAutoSynced) line += "  🔄";
                        var item = new ToolStripMenuItem($"{ev.GetTypeIcon()} {line}") { ForeColor = ev.GetColor(), Enabled = false };
                        cms.Items.Add(item);
                    }
                    cms.Show(_btnFacultyDrop, 0, _btnFacultyDrop!.Height);
                };

                Controls.Add(_btnFacultyDrop);
                _btnFacultyDrop.BringToFront();
            }

            RepositionPills();
        }

        // ── Drag source from faculty pill ────────────────────────────────────
        private void FacultyDrop_MouseDown(object? s, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            _mouseDownPt = e.Location;
        }

        private void FacultyDrop_MouseMove(object? s, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            var dist = Math.Abs(e.X - _mouseDownPt.X) + Math.Abs(e.Y - _mouseDownPt.Y);
            if (dist < 8) return;

            var events = FacultyCalendarData.GetEventsForDate(_fullDate);
            if (events.Count == 0) return;

            // If single event, drag it; otherwise show a menu to pick
            FacultyCalendarEvent? toDrag = events.Count == 1 ? events[0] : null;

            if (toDrag == null)
            {
                // Let user pick from context; we just capture first non-synced one
                toDrag = events.FirstOrDefault(ev => !ev.IsAutoSynced) ?? events[0];
            }

            _dragSourceEvent = toDrag;
            DoDragDrop(toDrag.Id, DragDropEffects.Move);
        }

        private void FacultyDrop_MouseUp(object? s, MouseEventArgs e)
        {
            _dragSourceEvent = null;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  QUICK ADD 
        // ══════════════════════════════════════════════════════════════════════
        private void QuickAddFacultyEvent(FacultyEventType type)
        {
            if (!_isCurrentMonth) return;
            using var dlg = new PortalContents.Instructor.LMS.Calendar.AddEditFacultyEventForm(_fullDate, null, type);
            if (dlg.ShowDialog() != DialogResult.OK || dlg.ResultEvent == null) return;
            FacultyCalendarData.AddEvent(_fullDate, dlg.ResultEvent);
            RefreshEventPills();
            DaySelected?.Invoke(_fullDate);
        }

        // Legacy QuickAddEvent (kept so existing references don't break)
        private void QuickAddEvent(EventType type)
        {
            if (!_isCurrentMonth) return;
            using var dlg = new AddEventForm(_fullDate, type);
            if (dlg.ShowDialog() != DialogResult.OK || dlg.CreatedEvent == null) return;
            if (_isStudent) SharedCalendarData.AddStudentEvent(_fullDate, dlg.CreatedEvent);
            else SharedCalendarData.AddEvent(_fullDate, dlg.CreatedEvent);
            RefreshEventPills();
            DaySelected?.Invoke(_fullDate);
        }

        // ══════════════════════════════════════════════════════════════════════
        //  NOTES / ANNOUNCEMENTS 
        // ══════════════════════════════════════════════════════════════════════
        private void OpenAnnouncementDialog()
        {
            if (string.IsNullOrEmpty(_day) || !_isCurrentMonth) return;
            string existing = SharedCalendarData.InstructorAnnouncements.ContainsKey(_fullDate)
                ? SharedCalendarData.InstructorAnnouncements[_fullDate] : "";

            using var frm = new Form
            {
                Text = "Add Announcement — " + _fullDate.ToString("MMMM dd, yyyy"),
                Size = new Size(400, 210),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White,
            };
            var pnlH = new Panel { Dock = DockStyle.Top, Height = 42, BackColor = C_Primary };
            var lH = new Label { Text = "Add Announcement", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 11f, FontStyle.Bold), ForeColor = Color.White, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(10, 0, 0, 0) };
            pnlH.Controls.Add(lH);
            var lbl = new Label { Text = "Announcement text:", Left = 12, Top = 54, AutoSize = true, Font = new Font("Segoe UI", 9f) };
            var txt = new TextBox { Left = 12, Top = 73, Width = 360, Height = 56, Multiline = true, ScrollBars = ScrollBars.Vertical, Text = existing, Font = new Font("Segoe UI", 9f) };
            var btnOk = new Button { Text = "Save", Left = 188, Top = 138, Width = 90, Height = 30, BackColor = C_Primary, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.OK };
            var btnCn = new Button { Text = "Cancel", Left = 286, Top = 138, Width = 90, Height = 30, FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.Cancel };
            btnOk.FlatAppearance.BorderSize = 0;
            frm.Controls.AddRange(new Control[] { pnlH, lbl, txt, btnOk, btnCn });
            frm.AcceptButton = btnOk; frm.CancelButton = btnCn;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                    SharedCalendarData.InstructorAnnouncements.Remove(_fullDate);
                else
                    SharedCalendarData.InstructorAnnouncements[_fullDate] = txt.Text.Trim();
                SharedCalendarData.SaveData();
                LoadAnnouncement();
                DaySelected?.Invoke(_fullDate);
            }
        }

        private void OnRemoveAnnouncement(object sender, EventArgs e)
        {
            string title = SharedCalendarData.InstructorAnnouncements.ContainsKey(_fullDate)
                ? SharedCalendarData.InstructorAnnouncements[_fullDate] : "";
            if (MessageBox.Show($"Remove the announcement for {_fullDate:MMMM dd, yyyy}?\n\n\"{title}\"",
                    "Confirm Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SharedCalendarData.InstructorAnnouncements.Remove(_fullDate);
                SharedCalendarData.SaveData();
                LoadAnnouncement();
                DaySelected?.Invoke(_fullDate);
            }
        }

        private void LoadNote()
        {
            if (lblNote == null) return;
            lblNote.Visible = false;
            if (_btnNoteDropdown != null) { Controls.Remove(_btnNoteDropdown); _btnNoteDropdown = null; }

            var dict = _isStudent
                ? SharedCalendarData.StudentNotes
                : PortalContents.Instructor.LMS.Calendar.CalendarContentInst.notesDict;
            bool has = dict.ContainsKey(_fullDate) && !string.IsNullOrWhiteSpace(dict[_fullDate]);

            if (has)
            {
                string noteText = dict[_fullDate];
                _btnNoteDropdown = MakeGroupedDropdown("🗒 Note  ▾", Color.FromArgb(80, 80, 80));
                _btnNoteDropdown.Click += (s, e) =>
                {
                    var cms = new ContextMenuStrip { Font = new Font("Segoe UI", 8.5f) };
                    string p = noteText.Length > 60 ? noteText[..57] + "…" : noteText;
                    cms.Items.Add(new ToolStripMenuItem(p) { Enabled = false, ForeColor = Color.FromArgb(50, 50, 50) });
                    cms.Items.Add(new ToolStripSeparator());
                    cms.Items.Add(new ToolStripMenuItem("✏  Edit Note", null, (ss, ee) => OpenNotesDialog()));
                    cms.Show(_btnNoteDropdown, 0, _btnNoteDropdown!.Height);
                };
                Controls.Add(_btnNoteDropdown);
                _btnNoteDropdown.BringToFront();
            }
            RepositionPills();
        }

        private void LoadAnnouncement()
        {
            if (lblAnnouncement == null) return;
            lblAnnouncement.Visible = false;
            RepositionPills();
        }

        private void OpenNotesDialog()
        {
            if (string.IsNullOrEmpty(_day) || !_isCurrentMonth) return;
            var dict = _isStudent
                ? SharedCalendarData.StudentNotes
                : PortalContents.Instructor.LMS.Calendar.CalendarContentInst.notesDict;
            string existing = dict.ContainsKey(_fullDate) ? dict[_fullDate] : "";

            using var form = new AddNotesForm(_fullDate, existing);
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.IsDeleted) dict.Remove(_fullDate);
                else dict[_fullDate] = form.NoteText;
                SharedCalendarData.SaveData();
                LoadNote();
                DaySelected?.Invoke(_fullDate);
            }
        }

        private void OpenAnnouncementInfo()
        {
            if (string.IsNullOrWhiteSpace(lblAnnouncement.Text)) return;
            MessageBox.Show(lblAnnouncement.Text,
                "Instructor Announcement — " + _fullDate.ToString("MMMM dd, yyyy"),
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ══════════════════════════════════════════════════════════════════════
        //  LAYOUT HELPERS
        // ══════════════════════════════════════════════════════════════════════
        private void RepositionPills()
        {
            int y = 34; // below the day-number circle

            if (_btnEventsDropdown != null)
            {
                _btnEventsDropdown.Location = new Point(2, y);
                _btnEventsDropdown.Width = Math.Max(10, Width - 4);
                y += 22;
            }

            if (_btnFacultyDrop != null)
            {
                _btnFacultyDrop.Location = new Point(2, y);
                _btnFacultyDrop.Width = Math.Max(10, Width - 4);
                y += 22;
            }

            if (_btnNoteDropdown != null)
            {
                _btnNoteDropdown.Location = new Point(2, y);
                _btnNoteDropdown.Width = Math.Max(10, Width - 4);
            }

            if (lblNote != null) lblNote.Visible = false;
            if (lblAnnouncement != null) lblAnnouncement.Visible = false;
        }

        private Button MakeGroupedDropdown(string label, Color backColor)
        {
            float brightness = (backColor.R * 299f + backColor.G * 587f + backColor.B * 114f) / 1000f;
            Color fg = brightness < 128 ? Color.White : Color.FromArgb(30, 30, 30);
            var btn = new Button
            {
                AutoSize = false,
                Text = label,
                Font = new Font("Segoe UI", 7.5f),
                ForeColor = fg,
                BackColor = backColor,
                Size = new Size(Math.Max(10, Width - 4), 20),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5, 0, 4, 0),
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                UseVisualStyleBackColor = false,
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(backColor, 0.10f);
            return btn;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  MOUSE / PAINT
        // ══════════════════════════════════════════════════════════════════════
        private void Cell_Click(object sender, EventArgs e)
        {
            if (!_isCurrentMonth) return;
            DaySelected?.Invoke(_fullDate);
        }

        private void Cell_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && _isCurrentMonth)
                _ctxMenu?.Show(this, e.Location);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Color borderColor;
            int penWidth;

            if (_isDragOver)
            {
                borderColor = Color.FromArgb(21, 101, 192);
                penWidth = 2;
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(15, 21, 101, 192)),
                    ClientRectangle);
            }
            else if (_isSelected || (_isHovered && _isCurrentMonth))
            {
                borderColor = C_Primary;
                penWidth = 2;
            }
            else
            {
                borderColor = Color.FromArgb(210, 210, 210);
                penWidth = 1;
            }

            using var pen = new Pen(borderColor, penWidth);
            e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
        }

        // ── Utilities ─────────────────────────────────────────────────────────
        private static string BuildEventLine(string typeLabel, string title,
                                             string start, string end, string room, string course)
        {
            string line = $"[{typeLabel}]  {title}";
            if (!string.IsNullOrEmpty(start))
                line += $"  •  {start}" + (string.IsNullOrEmpty(end) ? "" : "–" + end);
            if (!string.IsNullOrEmpty(room))
                line += $"  •  Rm {room}";
            if (!string.IsNullOrEmpty(course))
                line += $"  •  {course}";
            return line;
        }

        private static string TruncateStr(string s, int max) =>
            s.Length <= max ? s : s[..max] + "…";

        private static Color BlendWithWhite(Color c, float ratio)
        {
            ratio = Math.Clamp(ratio, 0f, 1f);
            return Color.FromArgb(
                (int)(c.R + (255 - c.R) * (1f - ratio)),
                (int)(c.G + (255 - c.G) * (1f - ratio)),
                (int)(c.B + (255 - c.B) * (1f - ratio)));
        }

        private void ShowEventDetails(CalendarEvent ev, bool isPersonal)
        {
            string header = isPersonal ? "[MY EVENT]" : $"[{ev.GetTypeLabel()}]";
            string msg = $"{header}  {ev.Title}\n";
            if (!string.IsNullOrEmpty(ev.StartTime))
                msg += $"Time: {ev.StartTime}" + (string.IsNullOrEmpty(ev.EndTime) ? "" : $" – {ev.EndTime}") + "\n";
            if (!string.IsNullOrEmpty(ev.Room)) msg += $"Room: {ev.Room}\n";
            if (!string.IsNullOrEmpty(ev.Description)) msg += $"\n{ev.Description}";
            MessageBox.Show(msg.Trim(), _fullDate.ToString("MMMM dd, yyyy"),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ── Designer stubs ────────────────────────────────────────────────────
        private void UrDay_Load(object sender, EventArgs e) { }
        private void panel1_Click(object sender, EventArgs e) => Cell_Click(sender, e);
        private void panel1_Paint(object sender, PaintEventArgs e) { }
    }
}