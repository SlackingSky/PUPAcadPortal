using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class GradingInterface : UserControl
    {
        public event Action OnBack;

        private StudentSubmission currentSubmission;
        private ActivityItem currentActivity;
        private System.Windows.Forms.Timer autoSaveTimer;
        private List<StudentSubmission> studentList;
        private int studentIndex;

        private const int RubricCriterionMax = 25;
        private const int RubricTotalMax = RubricCriterionMax * 4;

        public GradingInterface(
            StudentSubmission submission,
            ActivityItem activity,
            List<StudentSubmission> allStudents = null,
            int index = 0)
        {
            InitializeComponent();

            currentSubmission = submission;
            currentActivity = activity;
            studentList = allStudents;
            studentIndex = index;

            SetupRubricLabels();
            SetupRubricStyle();


            nudContent.Maximum = RubricCriterionMax;
            nudStructure.Maximum = RubricCriterionMax;
            nudGrammar.Maximum = RubricCriterionMax;
            nudRelevance.Maximum = RubricCriterionMax;

            nudContent.ValueChanged += nudRubric_ValueChanged;
            nudStructure.ValueChanged += nudRubric_ValueChanged;
            nudGrammar.ValueChanged += nudRubric_ValueChanged;
            nudRelevance.ValueChanged += nudRubric_ValueChanged;

            LoadSubmissionData();

            if (studentList != null)
            {
                btnPrevStudent.Enabled = studentIndex > 0;
                btnNextStudent.Enabled = studentIndex < studentList.Count - 1;
            }
        }


        private void SetupRubricLabels()
        {
            lblContentLabel.Text = "Content";
            lblStructureLabel.Text = "Structure";
            lblGrammarLabel.Text = "Grammar";
            lblRelevanceLabel.Text = "Relevance";

            lblContentMax.Text = $"/ {RubricCriterionMax}";
            lblStructureMax.Text = $"/ {RubricCriterionMax}";
            lblGrammarMax.Text = $"/ {RubricCriterionMax}";
            lblRelevanceMax.Text = $"/ {RubricCriterionMax}";

        }

        private void SetupRubricStyle()
        {
            Font rubricFont = new Font("Segoe UI", 9F, FontStyle.Bold);

            lblContentLabel.Font = rubricFont;
            lblStructureLabel.Font = rubricFont;
            lblGrammarLabel.Font = rubricFont;
            lblRelevanceLabel.Font = rubricFont;

            lblRubricTotal.ForeColor = Color.Maroon;
            lblRubricTotal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }

        
        private void LoadSubmissionData()
        {
            autoSaveTimer?.Stop();
            autoSaveTimer?.Dispose();

            autoSaveTimer = new System.Windows.Forms.Timer();
            autoSaveTimer.Interval = 30000;

            autoSaveTimer.Tick += (s, e) =>
            {
                if (int.TryParse(txtScore.Text, out int score))
                {
                    currentSubmission.Score = score;
                    currentSubmission.Remarks = txtRemarks.Text;

                    lblSaveStatus.Text =
                        $"Auto-saved at {DateTime.Now:hh:mm tt}";

                    lblSaveStatus.ForeColor = Color.Gray;
                }
            };

            autoSaveTimer.Start();

            lblStudentName.Text = currentSubmission.StudentName;
            lblStudentId.Text = currentSubmission.StudentId;
            lblActivityTitle.Text = currentActivity.Title;
            lblMaxPoints.Text =
                $"Max Points: {currentActivity.Points}";

            lblSubmissionTime.Text =
                currentSubmission.SubmissionTime == DateTime.MinValue
                ? "Not submitted"
                : $"Submitted: {currentSubmission.SubmissionTime:MMM dd, yyyy hh:mm tt}";

            txtEssayContent.Text = currentSubmission.EssayContent;
            txtRemarks.Text = currentSubmission.Remarks;
            txtScore.Text =
                currentSubmission.Score >= 0
                ? currentSubmission.Score.ToString()
                : "";

            lblScoreOf.Text = $"/ {currentActivity.Points}";
            nudContent.Value = 0;
            nudStructure.Value = 0;
            nudGrammar.Value = 0;
            nudRelevance.Value = 0;

            UpdateWordCount();
            UpdateRubricTotal();
        }

        private void UpdateRubricTotal()
        {
            int total =
                (int)nudContent.Value +
                (int)nudStructure.Value +
                (int)nudGrammar.Value +
                (int)nudRelevance.Value;

            lblRubricTotal.Text =
                $"Rubric Total: {total} / {RubricTotalMax}";

            if (chkAutoScore.Checked)
            {
                double percentage =
                    (double)total / RubricTotalMax;

                int computedScore =
                    (int)Math.Round(
                        percentage * currentActivity.Points);

                txtScore.Text = computedScore.ToString();
            }
        }

        private void nudRubric_ValueChanged(
            object sender,
            EventArgs e)
        {
            UpdateRubricTotal();
        }

        private void UpdateWordCount()
        {
            string text = txtEssayContent.Text.Trim();

            int words =
                string.IsNullOrWhiteSpace(text)
                ? 0
                : text.Split(
                    new char[]
                    {
                        ' ',
                        '\n',
                        '\r',
                        '\t'
                    },
                    StringSplitOptions.RemoveEmptyEntries
                ).Length;

            lblWordCount.Text = $"Words: {words}";
            lblCharCount.Text = $"Characters: {text.Length}";
        }

        private void txtEssayContent_TextChanged(
            object sender,
            EventArgs e)
        {
            UpdateWordCount();
        }

        private void btnSaveScore_Click(
            object sender,
            EventArgs e)
        {
            if (!int.TryParse(txtScore.Text, out int score))
            {
                MessageBox.Show(
                    "Please enter a valid score.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (score < 0 || score > currentActivity.Points)
            {
                MessageBox.Show(
                    $"Score must be between 0 and {currentActivity.Points}.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            currentSubmission.Score = score;
            currentSubmission.IsChecked = true;
            currentSubmission.Remarks = txtRemarks.Text;

            lblSaveStatus.Text =
                $"Saved at {DateTime.Now:hh:mm tt}";

            lblSaveStatus.ForeColor = Color.ForestGreen;

            MessageBox.Show(
                $"Score saved: {score}/{currentActivity.Points}",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void chkAutoScore_CheckedChanged(
            object sender,
            EventArgs e)
        {
            txtScore.ReadOnly = chkAutoScore.Checked;
            UpdateRubricTotal();
        }

        private void btnNextStudent_Click(
            object sender,
            EventArgs e)
        {
            if (studentList == null ||
                studentIndex >= studentList.Count - 1)
                return;

            currentSubmission = studentList[++studentIndex];

            LoadSubmissionData();

            btnPrevStudent.Enabled = true;

            btnNextStudent.Enabled =
                studentIndex < studentList.Count - 1;
        }

        private void btnPrevStudent_Click(
            object sender,
            EventArgs e)
        {
            if (studentList == null ||
                studentIndex <= 0)
                return;

            currentSubmission = studentList[--studentIndex];

            LoadSubmissionData();

            btnNextStudent.Enabled = true;

            btnPrevStudent.Enabled =
                studentIndex > 0;
        }

        private void btnBack_Click(
            object sender,
            EventArgs e)
        {
            autoSaveTimer?.Stop();
            autoSaveTimer?.Dispose();

            var parentContainer = this.Parent;

            if (parentContainer != null)
            {
                parentContainer.Controls.Remove(this);
            }

            OnBack?.Invoke();
        }
    }
}