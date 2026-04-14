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
                StartTime = txtStart.Text.Trim(),
                EndTime = txtEnd.Text.Trim(),
                Room = txtRoom.Text.Trim(),
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) => this.Close();
    }
}