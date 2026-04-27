using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace PUPAcadPortal
{
    public partial class UrDay : UserControl
    {
        public static event Action<DateTime> DaySelected;
        private string _day;
        private DateTime _fullDate;
        private Label _lblHoliday;
        private bool _isCurrentMonth;
        private bool _isStudent;
        private bool _isSelected;
        private bool _isHovered = false;
        private readonly List<Label> _eventPills = new List<Label>();
        private ContextMenuStrip _ctxMenu;
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
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                this.BackColor = value ? Color.FromArgb(240, 245, 255) : Color.White;
                Invalidate();
            }
        }

        public UrDay(string day,
                     int year = 0,
                     int month = 0,
                     bool isCurrentMonth = true,
                     string holiday = "",
                     bool isStudent = false)
        {
            InitializeComponent();

            _day = day;
            _isCurrentMonth = isCurrentMonth;
            _isStudent = isStudent;
            chkSelect.Hide();

            this.Margin = new Padding(0);
            this.Dock = DockStyle.None;
            this.BackColor = Color.White;
            this.Cursor = Cursors.Hand;

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
                this.BackColor = Color.FromArgb(248, 248, 248);
                return;
            }

            int dayInt = int.Parse(day);
            _fullDate = new DateTime(year, month, dayInt);

            if (!isCurrentMonth)
                lblDay.ForeColor = Color.FromArgb(200, 200, 200);
            else if (_fullDate.DayOfWeek == DayOfWeek.Sunday)
                lblDay.ForeColor = Color.Crimson;
            else
                lblDay.ForeColor = Color.FromArgb(40, 40, 40);

            if (DateTime.Now.Date == _fullDate.Date)
            {
                var gp = new GraphicsPath();
                gp.AddEllipse(0, 0, lblDay.Width, lblDay.Height);
                lblDay.Region = new Region(gp);
                lblDay.BackColor = Color.Maroon;
                lblDay.ForeColor = Color.White;
            }

            if (!string.IsNullOrEmpty(holiday))
            {
                _lblHoliday = MakePill(holiday, Color.FromArgb(52, 168, 83));
                _lblHoliday.Cursor = Cursors.Help;
                _lblHoliday.Click += (s, e) =>
                    MessageBox.Show(holiday,
                        "Holiday — " + _fullDate.ToString("MMMM dd, yyyy"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                var tip = new ToolTip();
                tip.SetToolTip(_lblHoliday, "Philippine Holiday — " + holiday);
                this.Controls.Add(_lblHoliday);
                _lblHoliday.BringToFront();
            }

            lblNote.Click += (s, e) => OpenNotesDialog();
            lblAnnouncement.Click += (s, e) => OpenAnnouncementInfo();
            this.Click += Cell_Click;
            this.MouseEnter += (s, e) => { if (_isCurrentMonth && !string.IsNullOrEmpty(_day)) { _isHovered = true; Invalidate(); } };
            this.MouseLeave += (s, e) => { _isHovered = false; Invalidate(); };
            lblDay.MouseEnter += (s, e) => { if (_isCurrentMonth && !string.IsNullOrEmpty(_day)) { _isHovered = true; Invalidate(); } };
            lblDay.MouseLeave += (s, e) => { _isHovered = false; Invalidate(); };
            lblDay.Click += Cell_Click;
            BuildContextMenu();
            this.MouseClick += Cell_MouseClick;

            this.Resize += (s, e) =>
            {
                if (_lblHoliday != null) _lblHoliday.Width = this.Width - 4;
                lblNote.Width = this.Width - 4;
                lblAnnouncement.Width = this.Width - 4;
                foreach (var lbl in _eventPills) lbl.Width = this.Width - 4;
            };

            RepositionPills();
            LoadNote();
            LoadAnnouncement();
            RefreshEventPills();
        }

        private void BuildContextMenu()
        {
            _ctxMenu = new ContextMenuStrip();

            if (!_isStudent)
            {
                var addEvent = new ToolStripMenuItem("🗓  Add Event");
                addEvent.DropDownItems.Add(new ToolStripMenuItem("📘 Class", null, (s, e) => QuickAddEvent(EventType.Class)));
                addEvent.DropDownItems.Add(new ToolStripMenuItem("📝 Exam", null, (s, e) => QuickAddEvent(EventType.Exam)));
                addEvent.DropDownItems.Add(new ToolStripMenuItem("📌 Deadline", null, (s, e) => QuickAddEvent(EventType.Deadline)));
                addEvent.DropDownItems.Add(new ToolStripMenuItem("🩺 Consultation", null, (s, e) => QuickAddEvent(EventType.Consultation)));

                var addNote = new ToolStripMenuItem("🗒  Add Note", null, (s, e) => OpenNotesDialog());

                _ctxMenu.Items.AddRange(new ToolStripItem[]
                {
                    addEvent,
                    new ToolStripSeparator(),
                    addNote,
                    new ToolStripSeparator(),
                });

                var mnuRemove = new ToolStripMenuItem("🗑  Remove Event");
                _ctxMenu.Items.Add(mnuRemove);


                _ctxMenu.Opening += (s, e) =>
                {
                    mnuRemove.DropDownItems.Clear();
                    var events = SharedCalendarData.GetEventsForDate(_fullDate);

                    mnuRemove.Enabled = _isCurrentMonth && events.Count > 0;

                    foreach (var ev in events)
                    {
                        var capturedEv = ev;
                        string prefix = capturedEv.Type switch
                        {
                            EventType.Class => "📘",
                            EventType.Exam => "📝",
                            EventType.Deadline => "📌",
                            EventType.Consultation => "🩺",
                            EventType.Cancelled => "🚫",
                            _ => "📅",
                        };
                        var item = new ToolStripMenuItem($"{prefix} [{capturedEv.GetTypeLabel()}]  {capturedEv.Title}");
                        item.Click += (ss, ee) =>
                        {
                            if (MessageBox.Show(
                                    $"Remove \"{capturedEv.Title}\" from {_fullDate:MMMM dd, yyyy}?",
                                    "Confirm Remove",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                SharedCalendarData.RemoveEvent(_fullDate, capturedEv);
                                RefreshEventPills();
                                DaySelected?.Invoke(_fullDate);
                            }
                        };
                        mnuRemove.DropDownItems.Add(item);
                    }

                    
                };
            }
            else
            {
                var addEvent = new ToolStripMenuItem("🗓  Add My Event");
                addEvent.DropDownItems.Add(new ToolStripMenuItem("📘 Class", null, (s, e) => QuickAddEvent(EventType.Class)));
                addEvent.DropDownItems.Add(new ToolStripMenuItem("📝 Exam", null, (s, e) => QuickAddEvent(EventType.Exam)));
                addEvent.DropDownItems.Add(new ToolStripMenuItem("📌 Deadline", null, (s, e) => QuickAddEvent(EventType.Deadline)));
                addEvent.DropDownItems.Add(new ToolStripMenuItem("🩺 Consultation", null, (s, e) => QuickAddEvent(EventType.Consultation)));

                var addNote = new ToolStripMenuItem("🗒  Add / Edit Note", null, (s, e) => OpenNotesDialog());

                _ctxMenu.Items.AddRange(new ToolStripItem[]
                {
                    addEvent,
                    new ToolStripSeparator(),
                    addNote,
                    new ToolStripSeparator(),
                });

                var mnuRemove = new ToolStripMenuItem("🗑  Remove My Event");
                _ctxMenu.Items.Add(mnuRemove);

                _ctxMenu.Opening += (s, e) =>
                {
                    mnuRemove.DropDownItems.Clear();
                    var personalEvents = SharedCalendarData.GetStudentEventsForDate(_fullDate);

                    mnuRemove.Enabled = _isCurrentMonth && personalEvents.Count > 0;

                    foreach (var ev in personalEvents)
                    {
                        var capturedEv = ev;
                        string prefix = capturedEv.Type switch
                        {
                            EventType.Class => "📘",
                            EventType.Exam => "📝",
                            EventType.Deadline => "📌",
                            EventType.Consultation => "🩺",
                            EventType.Cancelled => "🚫",
                            _ => "📅",
                        };
                        var item = new ToolStripMenuItem($"{prefix} [{capturedEv.GetTypeLabel()}]  {capturedEv.Title}");
                        item.Click += (ss, ee) =>
                        {
                            if (MessageBox.Show(
                                    $"Remove \"{capturedEv.Title}\" from {_fullDate:MMMM dd, yyyy}?",
                                    "Confirm Remove",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                SharedCalendarData.RemoveStudentEvent(_fullDate, capturedEv);
                                RefreshEventPills();
                                DaySelected?.Invoke(_fullDate);
                            }
                        };
                        mnuRemove.DropDownItems.Add(item);
                    }
                };
            }

            this.ContextMenuStrip = _ctxMenu;
        }

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
                BackColor = Color.White
            };
            var pnlH = new Panel { Dock = DockStyle.Top, Height = 42, BackColor = Color.Maroon };
            var lH = new Label
            {
                Text = "Add Announcement",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };
            pnlH.Controls.Add(lH);

            var lbl = new Label { Text = "Announcement text:", Left = 12, Top = 54, AutoSize = true, Font = new Font("Segoe UI", 9f), ForeColor = Color.FromArgb(60, 60, 60) };
            var txt = new TextBox { Left = 12, Top = 73, Width = 360, Height = 56, Multiline = true, ScrollBars = ScrollBars.Vertical, Text = existing, Font = new Font("Segoe UI", 9f) };
            var btnOk = new Button { Text = "Save", Left = 188, Top = 138, Width = 90, Height = 30, BackColor = Color.Maroon, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.OK };
            btnOk.FlatAppearance.BorderSize = 0;
            var btnCnl = new Button { Text = "Cancel", Left = 286, Top = 138, Width = 90, Height = 30, FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.Cancel };
            frm.Controls.AddRange(new Control[] { pnlH, lbl, txt, btnOk, btnCnl });
            frm.AcceptButton = btnOk;
            frm.CancelButton = btnCnl;

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

            if (MessageBox.Show(
                    $"Remove the announcement for {_fullDate:MMMM dd, yyyy}?\n\n\"{title}\"",
                    "Confirm Remove",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SharedCalendarData.InstructorAnnouncements.Remove(_fullDate);
                SharedCalendarData.SaveData();
                LoadAnnouncement();
                DaySelected?.Invoke(_fullDate);
            }
        }

        private void QuickAddEvent(EventType type)
        {
            if (!_isCurrentMonth) return;

            using var dlg = new AddEventForm(_fullDate, type);
            if (dlg.ShowDialog() != DialogResult.OK || dlg.CreatedEvent == null) return;

            if (_isStudent)
                SharedCalendarData.AddStudentEvent(_fullDate, dlg.CreatedEvent);
            else
                SharedCalendarData.AddEvent(_fullDate, dlg.CreatedEvent);

            RefreshEventPills();
            DaySelected?.Invoke(_fullDate);
        }

        public void RefreshEventPills()
        {
            foreach (var lbl in _eventPills) this.Controls.Remove(lbl);
            _eventPills.Clear();

            var sharedEvents = SharedCalendarData.GetEventsForDate(_fullDate);
            foreach (var ev in sharedEvents)
            {
                var pill = MakePill(ev.Title, ev.GetColor(), isPersonal: false);
                pill.Tag = ev;
                pill.Click += (s, e) => ShowEventDetails(ev, isPersonal: false);

                var tip = new ToolTip();
                string tipText = ev.GetTypeLabel();
                if (!string.IsNullOrEmpty(ev.StartTime)) tipText += $"  {ev.StartTime}";
                if (!string.IsNullOrEmpty(ev.Room)) tipText += $"  •  {ev.Room}";
                tip.SetToolTip(pill, tipText);

                _eventPills.Add(pill);
                this.Controls.Add(pill);
                pill.BringToFront();
            }

            if (_isStudent)
            {
                var personalEvents = SharedCalendarData.GetStudentEventsForDate(_fullDate);
                foreach (var ev in personalEvents)
                {
                    Color personalColor = BlendWithWhite(ev.GetColor(), 0.70f);
                    var pill = MakePill("🔒 " + ev.Title, personalColor, isPersonal: true);
                    pill.Tag = ev;
                    pill.Click += (s, e) => ShowEventDetails(ev, isPersonal: true);

                    var tip = new ToolTip();
                    string tipText = "[MY EVENT] " + ev.GetTypeLabel();
                    if (!string.IsNullOrEmpty(ev.StartTime)) tipText += $"  {ev.StartTime}";
                    if (!string.IsNullOrEmpty(ev.Room)) tipText += $"  •  {ev.Room}";
                    tip.SetToolTip(pill, tipText);

                    _eventPills.Add(pill);
                    this.Controls.Add(pill);
                    pill.BringToFront();
                }
            }

            RepositionPills();
        }

        private void ShowEventDetails(CalendarEvent ev, bool isPersonal)
        {
            string header = isPersonal ? "[MY EVENT]" : $"[{ev.GetTypeLabel()}]";
            string msg = $"{header}  {ev.Title}\n";
            if (!string.IsNullOrEmpty(ev.StartTime))
                msg += $"Time: {ev.StartTime}" + (string.IsNullOrEmpty(ev.EndTime) ? "" : $" – {ev.EndTime}") + "\n";
            if (!string.IsNullOrEmpty(ev.Room))
                msg += $"Room: {ev.Room}\n";
            if (!string.IsNullOrEmpty(ev.Description))
                msg += $"\n{ev.Description}";

            MessageBox.Show(msg.Trim(),
                _fullDate.ToString("MMMM dd, yyyy"),
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static Color BlendWithWhite(Color c, float ratio)
        {
            ratio = Math.Max(0f, Math.Min(1f, ratio));
            return Color.FromArgb(
                (int)(c.R + (255 - c.R) * (1f - ratio)),
                (int)(c.G + (255 - c.G) * (1f - ratio)),
                (int)(c.B + (255 - c.B) * (1f - ratio)));
        }

        private Label MakePill(string text, Color back, bool isPersonal = false)
        {
            return new Label
            {
                AutoSize = false,
                AutoEllipsis = true,
                Text = text,
                Font = new Font("Segoe UI", 6.8f, isPersonal ? FontStyle.Italic : FontStyle.Regular),
                ForeColor = isPersonal ? Color.FromArgb(40, 40, 40) : Color.White,
                BackColor = back,
                Size = new Size(this.Width - 4, 16),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(3, 0, 0, 0),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            };
        }

        private void RepositionPills()
        {
            int y = 34;

            if (_lblHoliday != null)
            {
                _lblHoliday.Location = new Point(2, y);
                y += 18;
            }

            foreach (var lbl in _eventPills)
            {
                lbl.Location = new Point(2, y);
                y += 18;
            }

            if (lblNote.Visible)
            {
                lblNote.Location = new Point(2, y);
                y += 18;
            }

            if (lblAnnouncement.Visible)
                lblAnnouncement.Location = new Point(2, y);
        }

        private void LoadNote()
        {
            if (lblNote == null) return;
            var dict = _isStudent ? SharedCalendarData.StudentNotes : InstructorPortal.notesDict;
            bool has = dict.ContainsKey(_fullDate) && !string.IsNullOrWhiteSpace(dict[_fullDate]);
            lblNote.Text = has ? dict[_fullDate] : "";
            lblNote.Visible = has;
            RepositionPills();
        }

        private void LoadAnnouncement()
        {
            if (lblAnnouncement == null) return;
            bool has = SharedCalendarData.InstructorAnnouncements.ContainsKey(_fullDate)
                       && !string.IsNullOrWhiteSpace(SharedCalendarData.InstructorAnnouncements[_fullDate]);
            lblAnnouncement.Text = has ? SharedCalendarData.InstructorAnnouncements[_fullDate] : "";
            lblAnnouncement.Visible = has;
            RepositionPills();
        }

        private void OpenNotesDialog()
        {
            if (string.IsNullOrEmpty(_day) || !_isCurrentMonth) return;
            var dict = _isStudent ? SharedCalendarData.StudentNotes : InstructorPortal.notesDict;
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
            Color borderColor = _isSelected
                ? Color.FromArgb(66, 133, 244)
                : _isHovered && _isCurrentMonth
                    ? Color.FromArgb(200, 80, 40)
                    : Color.FromArgb(210, 210, 210);
            int penWidth = (_isSelected || _isHovered) ? 2 : 1;
            using (var pen = new Pen(borderColor, penWidth))
                e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
        }

        private void UrDay_Load(object sender, EventArgs e) { }
        private void panel1_Click(object sender, EventArgs e) => Cell_Click(sender, e);
        private void panel1_Paint(object sender, PaintEventArgs e) { }
    }
}