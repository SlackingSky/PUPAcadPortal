using PUPAcadPortal.Data;
using PUPAcadPortal.PortalContents.Student.LMS.Calendar;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalForms
{
    public partial class AddEventForm : Form
    {
        public CalendarEvent CreatedEvent { get; private set; }

        private static readonly Color C_Primary = Color.FromArgb(128, 0, 0);
        private static readonly Color C_Surface = Color.FromArgb(250, 250, 252);
        private static readonly Color C_Border = Color.FromArgb(210, 210, 218);
        private static readonly Color C_TextDark = Color.FromArgb(28, 28, 36);
        private static readonly Color C_TextMid = Color.FromArgb(80, 80, 92);

        private readonly DateTime _date;
        public AddEventForm(DateTime date) : this(date, EventType.Class) { }

        public AddEventForm(DateTime date, EventType preselect) : this(date, preselect, "") { }

        public AddEventForm(DateTime date, EventType preselect, string startTime)
        {
            _date = date;
            InitializeComponent();
            PopulateTypeCombo(preselect);
            UpdateHeaderColor();

            this.Text = "Add Event \u2014 " + date.ToString("MMMM dd, yyyy");
            lblDate.Text = date.ToString("dddd, MMMM dd, yyyy");

            if (!string.IsNullOrWhiteSpace(startTime))
                txtStartTime.Text = startTime;

            cmbType.SelectedIndexChanged += (s, e) => UpdateHeaderColor();
            btnSave.Click += BtnSave_Click;
        }

        private void PopulateTypeCombo(EventType preselect)
        {
            cmbType.Items.Clear();
            foreach (var val in Enum.GetValues(typeof(EventType)))
                cmbType.Items.Add(val);
            cmbType.SelectedItem = preselect;
            if (cmbType.SelectedIndex < 0) cmbType.SelectedIndex = 0;
        }

        private void UpdateHeaderColor()
        {
            if (cmbType.SelectedItem is EventType t)
            {
                Color c = new CalendarEvent { Type = t }.GetColor();
                pnlHeader.BackColor = c;
                btnSave.BackColor = c;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter an event title.", "Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitle.Focus();
                return;
            }

            // Start time format
            if (!string.IsNullOrWhiteSpace(txtStartTime.Text) &&
                !TimeSpan.TryParse(txtStartTime.Text, out _))
            {
                MessageBox.Show("Start time must be in HH:mm format (e.g. 08:30).", "Invalid Time",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStartTime.Focus();
                return;
            }

            // End time format
            if (!string.IsNullOrWhiteSpace(txtEndTime.Text) &&
                !TimeSpan.TryParse(txtEndTime.Text, out _))
            {
                MessageBox.Show("End time must be in HH:mm format (e.g. 10:00).", "Invalid Time",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEndTime.Focus();
                return;
            }

            CreatedEvent = new CalendarEvent
            {
                Type = cmbType.SelectedItem is EventType t ? t : EventType.Class,
                Title = txtTitle.Text.Trim(),
                Course = txtCourse.Text.Trim(),
                StartTime = txtStartTime.Text.Trim(),
                EndTime = txtEndTime.Text.Trim(),
                Room = txtRoom.Text.Trim(),
                Description = txtDesc.Text.Trim(),
            };

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}