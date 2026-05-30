using System;
using System.Drawing;
using System.Windows.Forms;
using static PUPAcadPortal.PortalContents.Instructor.LMS.GradesContentInst;

namespace PUPAcadPortal.Dialogs
{
    public partial class EditGradePercentageControl : Form
    {
        public GradeWeights UpdatedWeights { get; private set; }
        private readonly GradeWeights _original;
        private bool _isAdjusting = false;

        public EditGradePercentageControl(GradeWeights current)
        {
            InitializeComponent();

            _original = current;

            nudAttendance.Value = (decimal)current.AttendancePct;
            nudRecitation.Value = (decimal)current.RecitationPct;
            nudSeatwork.Value = (decimal)current.SeatworkPct;
            nudAssignment.Value = (decimal)current.AssignmentPct;
            nudLongTests.Value = (decimal)current.LongTestsPct;
            nudMajorExam.Value = (decimal)current.MajorExamsPct;

            nudAttendance.ValueChanged += NudOnValueChanged;
            nudRecitation.ValueChanged += NudOnValueChanged;
            nudSeatwork.ValueChanged += NudOnValueChanged;
            nudAssignment.ValueChanged += NudOnValueChanged;
            nudLongTests.ValueChanged += NudOnValueChanged;
            nudMajorExam.ValueChanged += NudOnValueChanged;

            UpdatedWeights = CloneWeights(current);
            RefreshTotals();
        }

        private void NudOnValueChanged(object sender, EventArgs e)
        {
            if (_isAdjusting) return;
            _isAdjusting = true;
            try
            {
                decimal csTotal = nudAttendance.Value + nudRecitation.Value +
                                  nudSeatwork.Value + nudAssignment.Value + nudLongTests.Value;

                if (csTotal > 70)
                {
                    var changed = (NumericUpDown)sender;
                    if (changed != nudMajorExam)
                    {
                        decimal over = csTotal - 70;
                        changed.Value = Math.Max(0, changed.Value - over);
                        csTotal = 70;
                    }
                }

                decimal grand = csTotal + nudMajorExam.Value;
                if (grand > 100)
                    nudMajorExam.Value = Math.Max(0, 100 - csTotal);

                RefreshTotals();
            }
            finally
            {
                _isAdjusting = false;
            }
        }

        private void RefreshTotals()
        {
            decimal cs = nudAttendance.Value + nudRecitation.Value +
                            nudSeatwork.Value + nudAssignment.Value + nudLongTests.Value;
            decimal grand = cs + nudMajorExam.Value;

            lblCSTotal.Text = $"Class Standing Total: {cs} / 70";
            lblCSTotal.ForeColor = cs == 70
                ? Color.FromArgb(16, 124, 65) : Color.Firebrick;

            lblGrandTotal.Text = $"Grand Total: {grand} / 100";
            lblGrandTotal.ForeColor = grand == 100
                ? Color.FromArgb(16, 124, 65) : Color.Firebrick;

            if (btnApply != null)
                btnApply.Enabled = (grand == 100);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DetachHandlers();
            nudAttendance.Value = 10;
            nudRecitation.Value = 10;
            nudSeatwork.Value = 15;
            nudAssignment.Value = 10;
            nudLongTests.Value = 25;
            nudMajorExam.Value = 30;
            AttachHandlers();
            RefreshTotals();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            decimal grand = nudAttendance.Value + nudRecitation.Value +
                            nudSeatwork.Value + nudAssignment.Value +
                            nudLongTests.Value + nudMajorExam.Value;

            if (grand != 100)
            {
                MessageBox.Show("Total must equal 100%. Adjust the values.",
                    "Invalid Total", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UpdatedWeights = new GradeWeights
            {
                AttendancePct = (double)nudAttendance.Value,
                RecitationPct = (double)nudRecitation.Value,
                SeatworkPct = (double)nudSeatwork.Value,
                AssignmentPct = (double)nudAssignment.Value,
                LongTestsPct = (double)nudLongTests.Value,
                MajorExamsPct = (double)nudMajorExam.Value,
            };

            DialogResult = DialogResult.OK;
            Close();
        }

        private void DetachHandlers()
        {
            nudAttendance.ValueChanged -= NudOnValueChanged;
            nudRecitation.ValueChanged -= NudOnValueChanged;
            nudSeatwork.ValueChanged -= NudOnValueChanged;
            nudAssignment.ValueChanged -= NudOnValueChanged;
            nudLongTests.ValueChanged -= NudOnValueChanged;
            nudMajorExam.ValueChanged -= NudOnValueChanged;
        }

        private void AttachHandlers()
        {
            nudAttendance.ValueChanged += NudOnValueChanged;
            nudRecitation.ValueChanged += NudOnValueChanged;
            nudSeatwork.ValueChanged += NudOnValueChanged;
            nudAssignment.ValueChanged += NudOnValueChanged;
            nudLongTests.ValueChanged += NudOnValueChanged;
            nudMajorExam.ValueChanged += NudOnValueChanged;
        }

        private static GradeWeights CloneWeights(GradeWeights src) => new()
        {
            AttendancePct = src.AttendancePct,
            RecitationPct = src.RecitationPct,
            SeatworkPct = src.SeatworkPct,
            AssignmentPct = src.AssignmentPct,
            LongTestsPct = src.LongTestsPct,
            MajorExamsPct = src.MajorExamsPct,
        };
    }
}