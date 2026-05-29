using System;
using System.Drawing;
using System.Windows.Forms;
using static PUPAcadPortal.PortalContents.Instructor.LMS.GradesContentInst;

namespace PUPAcadPortal.Dialogs
{
    public partial class EditGradePercentageControl : Form
    {
        public GradeWeights UpdatedWeights { get; private set; }
        private GradeWeights _original;
        private bool _isAdjusting = false; 

        public EditGradePercentageControl(GradeWeights current)
        {
            InitializeComponent();

            _original = current;
            UpdatedWeights = new GradeWeights
            {
                AttendancePct = current.AttendancePct,
                RecitationPct = current.RecitationPct,
                SeatworkPct = current.SeatworkPct,
                AssignmentPct = current.AssignmentPct,
                LongTestsPct = current.LongTestsPct,
                MajorExamsPct = current.MajorExamsPct,
            };

            nudAttendance.Value = (decimal)_original.AttendancePct;
            nudRecitation.Value = (decimal)_original.RecitationPct;
            nudSeatwork.Value = (decimal)_original.SeatworkPct;
            nudAssignment.Value = (decimal)_original.AssignmentPct;
            nudLongTests.Value = (decimal)_original.LongTestsPct;
            nudMajorExam.Value = (decimal)_original.MajorExamsPct;

            nudAttendance.ValueChanged += NudOnValueChanged;
            nudRecitation.ValueChanged += NudOnValueChanged;
            nudSeatwork.ValueChanged += NudOnValueChanged;
            nudAssignment.ValueChanged += NudOnValueChanged;
            nudLongTests.ValueChanged += NudOnValueChanged;
            nudMajorExam.ValueChanged += NudOnValueChanged;

            RefreshTotals();
        }

        private void NudOnValueChanged(object sender, EventArgs e)
        {
            if (_isAdjusting) return;
            _isAdjusting = true;

            try
            {
                var changedNud = (NumericUpDown)sender;
                bool isMajor = changedNud == nudMajorExam;

                if (!isMajor)
                {
                    decimal csTotal = nudAttendance.Value + nudRecitation.Value +
                                      nudSeatwork.Value + nudAssignment.Value +
                                      nudLongTests.Value;

                    if (csTotal > 70)
                    {
                        decimal over = csTotal - 70;
                        changedNud.Value = Math.Max(0, changedNud.Value - over);
                    }

                    csTotal = nudAttendance.Value + nudRecitation.Value +
                              nudSeatwork.Value + nudAssignment.Value +
                              nudLongTests.Value;

                    decimal grand = csTotal + nudMajorExam.Value;
                    if (grand > 100)
                        nudMajorExam.Value = Math.Max(0, 100 - csTotal);
                }
                else
                {
                    decimal csTotal = nudAttendance.Value + nudRecitation.Value +
                                      nudSeatwork.Value + nudAssignment.Value +
                                      nudLongTests.Value;

                    decimal grand = csTotal + nudMajorExam.Value;
                    if (grand > 100)
                        nudMajorExam.Value = Math.Max(0, 100 - csTotal);
                }

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
                            nudSeatwork.Value + nudAssignment.Value +
                            nudLongTests.Value;
            decimal grand = cs + nudMajorExam.Value;

            lblCSTotal.Text = $"Class Standing Total: {cs} / 70";
            lblCSTotal.ForeColor = cs == 70
                ? Color.FromArgb(16, 124, 65)
                : Color.Firebrick;

            bool perfect = grand == 100;
            lblGrandTotal.Text = perfect
                ? $"Grand Total: {grand} / 100"
                : $"Grand Total: {grand} / 100";
            lblGrandTotal.ForeColor = perfect
                ? Color.FromArgb(16, 124, 65)
                : Color.Firebrick;

            if (btnApply != null)
                btnApply.Enabled = perfect;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            nudAttendance.ValueChanged -= NudOnValueChanged;
            nudRecitation.ValueChanged -= NudOnValueChanged;
            nudSeatwork.ValueChanged -= NudOnValueChanged;
            nudAssignment.ValueChanged -= NudOnValueChanged;
            nudLongTests.ValueChanged -= NudOnValueChanged;
            nudMajorExam.ValueChanged -= NudOnValueChanged;

            nudAttendance.Value = 10;
            nudRecitation.Value = 10;
            nudSeatwork.Value = 15;
            nudAssignment.Value = 10;
            nudLongTests.Value = 25;
            nudMajorExam.Value = 30;

            nudAttendance.ValueChanged += NudOnValueChanged;
            nudRecitation.ValueChanged += NudOnValueChanged;
            nudSeatwork.ValueChanged += NudOnValueChanged;
            nudAssignment.ValueChanged += NudOnValueChanged;
            nudLongTests.ValueChanged += NudOnValueChanged;
            nudMajorExam.ValueChanged += NudOnValueChanged;

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
                MessageBox.Show("Total must equal 100%. Please adjust the values.",
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
    }
}