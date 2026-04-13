using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class UrDay : UserControl
    {
        private string _day;
        private DateTime fullDate;
        private Label lblNote;
        private Label _lblHoliday;
        private bool _isCurrentMonth;

        public UrDay(string day, int year = 0, int month = 0, bool isCurrentMonth = true, string holiday = "")
        {
            InitializeComponent();
            _day = day;
            _isCurrentMonth = isCurrentMonth;
            chkSelect.Hide();

            this.Margin = new Padding(0);
            this.Dock = DockStyle.None;
            this.BackColor = Color.White;

            if (year == 0) year = InstructorPortal._year;
            if (month == 0) month = InstructorPortal._month;

            // Style the existing lblDay
            lblDay.AutoSize = false;
            lblDay.Size = new Size(28, 28);
            lblDay.Location = new Point(4, 4);
            lblDay.Font = new Font("Segoe UI", 9f);
            lblDay.TextAlign = ContentAlignment.MiddleCenter;
            lblDay.BackColor = Color.Transparent;
            lblDay.Text = day;

            // Gray background for empty/overflow cells
            if (string.IsNullOrEmpty(day))
            {
                this.BackColor = Color.FromArgb(248, 248, 248);
                return;
            }

            int dayInt = int.Parse(day);
            fullDate = new DateTime(year, month, dayInt);

            // Day number color
            if (!isCurrentMonth)
                lblDay.ForeColor = Color.FromArgb(190, 190, 190);
            else if (fullDate.DayOfWeek == DayOfWeek.Sunday)
                lblDay.ForeColor = Color.Crimson;
            else
                lblDay.ForeColor = Color.FromArgb(40, 40, 40);

            // TODAY — circular maroon badge
            if (DateTime.Now.Date == fullDate.Date)
            {
                GraphicsPath gp = new GraphicsPath();
                gp.AddEllipse(0, 0, lblDay.Width, lblDay.Height);
                lblDay.Region = new Region(gp);
                lblDay.BackColor = Color.Maroon;
                lblDay.ForeColor = Color.White;
            }

            // HOLIDAY label — green pill (read-only, not clickable for notes)
            if (!string.IsNullOrEmpty(holiday))
            {
                _lblHoliday = new Label
                {
                    AutoSize = false,
                    Text = holiday,
                    Font = new Font("Segoe UI", 6.8f),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(52, 168, 83),
                    Location = new Point(2, 36),
                    Size = new Size(this.Width - 4, 16),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(3, 0, 0, 0),
                    AutoEllipsis = true,
                    Cursor = Cursors.Default
                };

                // Make holiday label pass clicks through to the panel (non-editable)
                _lblHoliday.Click += (s, e) => panel1_Click(s, e);

                this.Controls.Add(_lblHoliday);
            }

            // NOTE label — blue pill, only visible when a note exists
            int noteTop = !string.IsNullOrEmpty(holiday) ? 56 : 36;

            lblNote = new Label
            {
                AutoSize = false,
                Location = new Point(2, noteTop),
                Size = new Size(this.Width - 4, 18),
                Font = new Font("Segoe UI", 6.8f, FontStyle.Regular),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(66, 133, 244),  // Google-blue pill
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(4, 0, 4, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                AutoEllipsis = true,   // shows "..." when text is too long
                UseMnemonic = false,
                Visible = false,  // hidden until a note is saved
            };

            // Clicking the note pill also opens the edit dialog
            lblNote.Click += (s, e) => panel1_Click(s, e);

            this.Controls.Add(lblNote);

            // Keep pills sized correctly on resize
            this.Resize += (s, e) =>
            {
                if (_lblHoliday != null)
                    _lblHoliday.Size = new Size(this.Width - 4, 16);
                if (lblNote != null)
                    lblNote.Size = new Size(this.Width - 4, 18);
            };

            LoadNote();
        }

        private void LoadNote()
        {
            if (lblNote == null) return;

            bool hasNote = InstructorPortal.notesDict.ContainsKey(fullDate)
                           && !string.IsNullOrWhiteSpace(InstructorPortal.notesDict[fullDate]);

            lblNote.Text = hasNote ? InstructorPortal.notesDict[fullDate] : "";
            lblNote.Visible = hasNote;   // blue pill only appears when there's a note
        }

        // Draw cell border lines
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Pen pen = new Pen(Color.FromArgb(210, 210, 210)))
                e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
        }

        private void UrDay_Load(object sender, EventArgs e) { }

        private void panel1_Click(object sender, EventArgs e)
        {
            // Only allow notes on current month days
            if (string.IsNullOrEmpty(_day) || !_isCurrentMonth) return;

            string existingNote = InstructorPortal.notesDict.ContainsKey(fullDate)
                ? InstructorPortal.notesDict[fullDate] : "";

            AddNotesForm form = new AddNotesForm(fullDate, existingNote);
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.IsDeleted)
                    InstructorPortal.notesDict.Remove(fullDate);
                else
                    InstructorPortal.notesDict[fullDate] = form.NoteText;

                LoadNote();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e) { }
    }
}