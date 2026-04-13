using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace PUPAcadPortal
{
    public partial class UrDay : UserControl
    {
        PictureBox picNoteIcon;
        string _day;
        DateTime fullDate;
        Label lblNote;

        public UrDay(string day)
        {
            InitializeComponent();
            _day = day;
            lblDay.Text = day;
            chkSelect.Hide();

            this.Margin = new Padding(3);
            this.Dock = DockStyle.None;

            // Skip empty cells
            if (string.IsNullOrEmpty(_day))
                return;

            int dayInt = int.Parse(_day);

            fullDate = new DateTime(
                InstructorPortal._year,
                InstructorPortal._month,
                dayInt
            );

            // Sunday color
            if (fullDate.DayOfWeek == DayOfWeek.Sunday)
                lblDay.ForeColor = Color.Red;
            else
                lblDay.ForeColor = Color.Black;

            // Highlight today
            DateTime today = DateTime.Now;
            if (today.Day == dayInt &&
                today.Month == InstructorPortal._month &&
                today.Year == InstructorPortal._year)
            {
                this.BackColor = Color.LightBlue;
            }

            // ✅ NOTE TEXT
            lblNote = new Label();
            lblNote.AutoSize = false;
            lblNote.Location = new Point(3, 20);
            lblNote.Size = new Size(this.Width - 6, this.Height - 25);
            lblNote.Font = new Font("Segoe UI", 7);
            lblNote.ForeColor = Color.Yellow;
            lblNote.BackColor = Color.Maroon;

            lblNote.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            this.Controls.Add(lblNote);

            // ✅ NOTE ICON
            picNoteIcon = new PictureBox();
            picNoteIcon.Size = new Size(12, 12);
            picNoteIcon.Location = new Point(this.Width - 18, 3);
            picNoteIcon.BackColor = Color.Transparent;

            Bitmap bmp = new Bitmap(12, 12);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.FillEllipse(Brushes.Gold, 0, 0, 10, 10);
            }

            picNoteIcon.Image = bmp;
            picNoteIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            picNoteIcon.Visible = false;
            picNoteIcon.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.Controls.Add(picNoteIcon);

            LoadNote();
        }

        private void LoadNote()
        {
            if (InstructorPortal.notesDict.ContainsKey(fullDate))
            {
                lblNote.Text = InstructorPortal.notesDict[fullDate];
                picNoteIcon.Visible = true;
            }
            else
            {
                lblNote.Text = "";
                picNoteIcon.Visible = false;
            }
        }


        private void UrDay_Load(object sender, EventArgs e)
        {

        }


        private void panel1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_day))
                return;

            string existingNote = "";

            if (InstructorPortal.notesDict.ContainsKey(fullDate))
                existingNote = InstructorPortal.notesDict[fullDate];

            AddNotesForm form = new AddNotesForm(fullDate, existingNote);

            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.IsDeleted)
                {
                    if (InstructorPortal.notesDict.ContainsKey(fullDate))
                        InstructorPortal.notesDict.Remove(fullDate);
                }
                else
                {
                    InstructorPortal.notesDict[fullDate] = form.NoteText;
                }

                LoadNote();
            }
        }
    }
}