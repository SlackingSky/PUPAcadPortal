using System;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    /// <summary>
    /// Student-facing submission screen for an activity.
    /// Handles Essay and Assignment / FileUpload types.
    /// UI layout lives in StudentActivitySubmit.Designer.cs;
    /// this file contains only event logic and data setup.
    /// </summary>
    public partial class StudentActivitySubmit : UserControl
    {
        public event Action OnBack;

        private StudentActivityItem activity;
        private StudentCourse course;

        public StudentActivitySubmit(StudentActivityItem activity, StudentCourse course)
        {
            this.activity = activity;
            this.course = course;

            InitializeComponent();
            ConfigureForActivityType();
        }

        // ── Setup ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Populates header labels and shows only the controls relevant to the
        /// current activity type, hiding the others.
        /// </summary>
        private void ConfigureForActivityType()
        {
            // Header
            lblActivityTitle.Text = activity.Title;
            string typeDisplay = activity.Type == "FileUpload" ? "File Upload" : activity.Type;
            lblMeta.Text = $"{typeDisplay}  ·  {activity.Points} pts  ·  Due {activity.Deadline:MMM dd, yyyy}";

            if (activity.Type == "Essay")
            {
                ShowEssayControls();
            }
            else
            {
                ShowAssignmentControls();
            }
        }

        private void ShowEssayControls()
        {
            // Populate instruction text
            lblInstrBody.Text = activity.Instructions;
            txtEssay.Text = activity.EssayDraft;

            // Add essay controls to the body panel
            pnlBody.Controls.Add(pnlInstructions);
            pnlBody.Controls.Add(lblEssayPrompt);
            pnlBody.Controls.Add(txtEssay);
            pnlBody.Controls.Add(lblWordCount);
            pnlBody.Controls.Add(btnSubmitEssay);

            // Hide assignment-only controls
            pnlInstr2.Visible = false;
            lblFilePrompt.Visible = false;
            pnlDropZone.Visible = false;
            lblNotesPrompt.Visible = false;
            txtNotes.Visible = false;
            btnSubmitAssign.Visible = false;

            UpdateWordCount();
        }

        private void ShowAssignmentControls()
        {
            // Populate instruction text
            lblInstrBody2.Text = activity.Instructions;

            // Adjust button text for FileUpload
            btnSubmitAssign.Text = activity.Type == "FileUpload" ? "Upload File" : "Submit Assignment";

            // Add assignment controls to the body panel
            pnlBody.Controls.Add(pnlInstr2);
            pnlBody.Controls.Add(lblNotesPrompt);
            pnlBody.Controls.Add(txtNotes);
            pnlBody.Controls.Add(btnSubmitAssign);

            if (activity.Type == "FileUpload")
            {
                pnlBody.Controls.Add(lblFilePrompt);
                pnlBody.Controls.Add(pnlDropZone);

                // Shift notes/button down to make room for the drop zone
                lblNotesPrompt.Location = new System.Drawing.Point(20, 294);
                txtNotes.Location = new System.Drawing.Point(20, 322);
                btnSubmitAssign.Location = new System.Drawing.Point(20, 440);
            }
            else
            {
                lblFilePrompt.Visible = false;
                pnlDropZone.Visible = false;

                lblNotesPrompt.Location = new System.Drawing.Point(20, 168);
                txtNotes.Location = new System.Drawing.Point(20, 196);
                btnSubmitAssign.Location = new System.Drawing.Point(20, 314);
            }

            // Hide essay-only controls
            pnlInstructions.Visible = false;
            lblEssayPrompt.Visible = false;
            txtEssay.Visible = false;
            lblWordCount.Visible = false;
            btnSubmitEssay.Visible = false;
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private void UpdateWordCount()
        {
            if (txtEssay == null || lblWordCount == null) return;
            string text = txtEssay.Text.Trim();
            int words = string.IsNullOrWhiteSpace(text) ? 0
                : text.Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
            lblWordCount.Text = $"Words: {words}  |  Characters: {text.Length}";
        }

        // ── Event handlers ────────────────────────────────────────────────────

        private void btnBack_Click(object sender, EventArgs e) => OnBack?.Invoke();

        private void txtEssay_TextChanged(object sender, EventArgs e) => UpdateWordCount();

        private void btnSubmitEssay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEssay.Text))
            {
                MessageBox.Show("Please write your essay before submitting.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            activity.EssayDraft = txtEssay.Text;
            activity.SubmissionStatus = "Submitted";
            MessageBox.Show("Essay submitted successfully!", "Submitted",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            OnBack?.Invoke();
        }

        private void btnSubmitAssign_Click(object sender, EventArgs e)
        {
            activity.SubmissionStatus = "Submitted";
            string verb = activity.Type == "FileUpload" ? "Uploaded" : "Submitted";
            MessageBox.Show($"{verb} successfully!", "Done",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            OnBack?.Invoke();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog
            {
                Filter = "All Files (*.*)|*.*",
                Title = "Select file to upload"
            };
            if (dlg.ShowDialog() == DialogResult.OK)
                MessageBox.Show($"File selected: {System.IO.Path.GetFileName(dlg.FileName)}",
                    "File Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}