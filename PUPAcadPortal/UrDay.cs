using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace PUPAcadPortal
{
    public partial class UrDay : UserControl
    {
        private string _day;
        private DateTime fullDate;
        private Label _lblHoliday;
        private bool _isCurrentMonth;
        private bool _isStudent; // true = student view (announcements read-only)

        // Lets the portal push an announcement text onto this cell
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

        // Constructor
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

            if (year == 0) year = SharedCalendarData.CurrentYear;
            if (month == 0) month = SharedCalendarData.CurrentMonth;

            // Style lblDay
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
            fullDate = new DateTime(year, month, dayInt);

            // Day number colour
            if (!isCurrentMonth)
                lblDay.ForeColor = Color.FromArgb(190, 190, 190);
            else if (fullDate.DayOfWeek == DayOfWeek.Sunday)
                lblDay.ForeColor = Color.Crimson;
            else
                lblDay.ForeColor = Color.FromArgb(40, 40, 40);

            // TODAY
            if (DateTime.Now.Date == fullDate.Date)
            {
                GraphicsPath gp = new GraphicsPath();
                gp.AddEllipse(0, 0, lblDay.Width, lblDay.Height);
                lblDay.Region = new Region(gp);
                lblDay.BackColor = Color.Maroon;
                lblDay.ForeColor = Color.White;
            }

            // HOLIDAY green pill (always read-only)
            if (!string.IsNullOrEmpty(holiday))
            {
                _lblHoliday = NewMethod(holiday);

                // Show holiday name in a popup
                _lblHoliday.Click += (s, e) =>
                    MessageBox.Show(holiday,
                        "Holiday — " + fullDate.ToString("MMMM dd, yyyy"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Controls.Add(_lblHoliday);
                _lblHoliday.BringToFront();
            }

            // NOTE pill blue (student or instructor notes) 
            lblNote.Click += (s, e) => OpenNotesDialog();

            // ANNOUNCEMENT pill orange (instructor writes, student reads)
            lblAnnouncement.Click += (s, e) => OpenAnnouncementInfo();

            // Resize → keep pills wide
            this.Resize += (s, e) =>
            {
                if (_lblHoliday != null)
                    _lblHoliday.Size = new Size(this.Width - 4, 16);
                lblNote.Size = new Size(this.Width - 4, 18);
                lblAnnouncement.Size = new Size(this.Width - 4, 18);
            };

            RepositionPills();
            LoadNote();
            LoadAnnouncement();
        }

        private Label NewMethod(string holiday)
        {
            Label lbl = new Label
            {
                AutoSize = false,
                AutoEllipsis = true,
                Text = holiday,
                Font = new Font("Segoe UI", 6.8f),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(52, 168, 83),
                Location = new Point(2, 36),
                Size = new Size(this.Width - 4, 16),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(3, 0, 0, 0),
                Cursor = Cursors.Help,
            };

            // Attach tooltip separately — Label has no ToolTipText property
            ToolTip tip = new ToolTip();
            tip.SetToolTip(lbl, "Philippine Holiday — " + holiday);

            return lbl;
        }

        // Stack pills so they don't overlap
        private void RepositionPills()
        {
            int y = 36;

            if (_lblHoliday != null)
            {
                _lblHoliday.Location = new Point(2, y);
                y += 18;
            }

            lblNote.Location = new Point(2, y);
            y += lblNote.Visible ? 20 : 0;

            lblAnnouncement.Location = new Point(2, y);
        }

        // Load the correct notes dictionary
        private void LoadNote()
        {
            if (lblNote == null) return;

            // Students use SharedCalendarData.StudentNotes
            // Instructors use InstructorPortal.notesDict
            var dict = _isStudent
                ? SharedCalendarData.StudentNotes
                : InstructorPortal.notesDict;

            bool hasNote = dict.ContainsKey(fullDate)
                               && !string.IsNullOrWhiteSpace(dict[fullDate]);
            lblNote.Text = hasNote ? dict[fullDate] : "";
            lblNote.Visible = hasNote;

            RepositionPills();
        }

        // Load instructor announcement onto this cell
        private void LoadAnnouncement()
        {
            if (lblAnnouncement == null) return;

            bool has = SharedCalendarData.InstructorAnnouncements.ContainsKey(fullDate)
                       && !string.IsNullOrWhiteSpace(
                              SharedCalendarData.InstructorAnnouncements[fullDate]);

            lblAnnouncement.Text = has
                ? SharedCalendarData.InstructorAnnouncements[fullDate] : "";
            lblAnnouncement.Visible = has;

            RepositionPills();
        }

        // Open notes editor (both portals — uses correct dict)
        private void OpenNotesDialog()
        {
            if (string.IsNullOrEmpty(_day) || !_isCurrentMonth) return;

            var dict = _isStudent
                ? SharedCalendarData.StudentNotes
                : InstructorPortal.notesDict;

            string existing = dict.ContainsKey(fullDate) ? dict[fullDate] : "";

            AddNotesForm form = new AddNotesForm(fullDate, existing);
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.IsDeleted)
                    dict.Remove(fullDate);
                else
                    dict[fullDate] = form.NoteText;

                LoadNote();
            }
        }

        // Show announcement popup (read-only for everyone)
        private void OpenAnnouncementInfo()
        {
            if (string.IsNullOrWhiteSpace(lblAnnouncement.Text)) return;

            MessageBox.Show(lblAnnouncement.Text,
                "Instructor Announcement — " + fullDate.ToString("MMMM dd, yyyy"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // Cell border
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Pen pen = new Pen(Color.FromArgb(210, 210, 210)))
                e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
        }

        private void UrDay_Load(object sender, EventArgs e) { }

        // Clicking the cell background opens notes
        private void panel1_Click(object sender, EventArgs e) => OpenNotesDialog();
        private void panel1_Paint(object sender, PaintEventArgs e) { }
    }
}