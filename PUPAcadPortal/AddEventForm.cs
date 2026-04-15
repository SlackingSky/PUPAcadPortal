using System;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class AddEventForm : Form
    {
        public CalendarEvent CreatedEvent { get; private set; }

        private readonly DateTime _date;
        private readonly EventType _defaultType;

        public AddEventForm(DateTime date, EventType defaultType = EventType.Class)
        {
            _date = date;
            _defaultType = defaultType;

            InitializeComponent();

            this.Text = "Add Event \u2014 " + _date.ToString("MMMM dd, yyyy");
            lblDateValue.Text = _date.ToString("MMMM dd, yyyy");

            foreach (var t in Enum.GetValues(typeof(EventType)))
                cmbType.Items.Add(t);

            cmbType.SelectedItem = _defaultType;

            // Default time pickers to unchecked (no time set)
            dtpStart.Checked = false;
            dtpEnd.Checked = false;

            // Set a sensible default time so when user checks it, it starts at 8:00 AM
            dtpStart.Value = new DateTime(_date.Year, _date.Month, _date.Day, 8, 0, 0);
            dtpEnd.Value = new DateTime(_date.Year, _date.Month, _date.Day, 9, 0, 0);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter a title.", "Add Event",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CreatedEvent = new CalendarEvent
            {
                Type = cmbType.SelectedItem != null
                    ? (EventType)cmbType.SelectedItem
                    : EventType.Class,
                Title = txtTitle.Text.Trim(),
                Description = txtDesc.Text.Trim(),
                StartTime = dtpStart.Checked ? dtpStart.Value.ToString("h:mm tt") : "",
                EndTime = dtpEnd.Checked ? dtpEnd.Value.ToString("h:mm tt") : "",
                Room = txtRoom.Text.Trim(),
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) => this.Close();
    }
}