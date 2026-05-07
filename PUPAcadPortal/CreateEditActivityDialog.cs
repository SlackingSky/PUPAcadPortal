using System;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class CreateEditActivityDialog : Form
    {
        // Properties read by AssignmentManagement after ShowDialog() returns OK
        public string ActivityTitle { get; private set; } = "";
        public string ActivityType { get; private set; } = "Assignment";
        public DateTime ActivityDeadline { get; private set; } = DateTime.Now.AddDays(7);
        public int ActivityPoints { get; private set; } = 100;

        private ActivityItem? editingActivity;

        public CreateEditActivityDialog(ActivityItem? activity)
        {
            editingActivity = activity;
            InitializeComponent();

            if (activity != null)
            {
                this.Text = "Edit Activity";
                lblHeader.Text = "Edit Activity";
                txtTitle.Text = activity.Title;

                // Match the stored type string to a combo item
                int idx = cmbType.FindStringExact(activity.Type);
                cmbType.SelectedIndex = idx >= 0 ? idx : 0;

                dtpDeadline.Value = activity.Deadline > DateTime.MinValue
                    ? activity.Deadline
                    : DateTime.Now.AddDays(7);

                nudPoints.Value = Math.Max(nudPoints.Minimum,
                    Math.Min(nudPoints.Maximum, activity.Points));
            }
            else
            {
                this.Text = "Create Activity";
                lblHeader.Text = "Create New Activity";
            }

            btnOk.Text = activity == null ? "Create" : "Save Changes";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                lblError.Text = "Activity title is required.";
                txtTitle.Focus();
                return;
            }

            // Allow past deadlines when editing an existing activity
            if (editingActivity == null && dtpDeadline.Value <= DateTime.Now)
            {
                lblError.Text = "Deadline must be in the future.";
                return;
            }

            ActivityTitle = txtTitle.Text.Trim();
            ActivityType = cmbType.SelectedItem?.ToString() ?? "Assignment";
            ActivityDeadline = dtpDeadline.Value;
            ActivityPoints = (int)nudPoints.Value;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}