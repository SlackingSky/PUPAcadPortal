using System;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal
{
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

            SetupLayout();
            ConfigureForActivityType();
        }


        private void SetupLayout()
        {
            pnlBody.AutoScroll = true;
            pnlBody.Padding = new Padding(20);
            pnlBody.Dock = DockStyle.Fill;

            txtEssay.Multiline = true;
            txtEssay.ScrollBars = ScrollBars.Vertical;

            txtNotes.Multiline = true;
            txtNotes.ScrollBars = ScrollBars.Vertical;

            pnlBody.SizeChanged += (s, e) => StretchControls();
        }

        private void StretchControls()
        {
            int availableWidth = pnlBody.ClientSize.Width
                                 - pnlBody.Padding.Left
                                 - pnlBody.Padding.Right
                                 - 40; 

            if (availableWidth < 100) return;

            Control[] wideControls = new Control[]
            {
                pnlInstructions, pnlInstr2,
                lblEssayPrompt, lblFilePrompt, lblNotesPrompt,
                txtEssay, txtNotes,
                pnlDropZone,
                lblWordCount
            };

            foreach (var ctrl in wideControls)
            {
                if (ctrl.Visible)
                    ctrl.Width = availableWidth;
            }
        }

        private void ConfigureForActivityType()
        {
            lblActivityTitle.Text = activity.Title;

            string typeDisplay =
                activity.Type == "FileUpload"
                ? "File Upload"
                : activity.Type;

            lblMeta.Text =
                $"{typeDisplay}  ·  {activity.Points} pts  ·  Due {activity.Deadline:MMM dd, yyyy}";

            ClearAllControls();

            if (activity.Type == "Essay")
                ShowEssayControls();
            else
                ShowAssignmentControls();
        }

        private void ClearAllControls()
        {
            foreach (Control c in pnlBody.Controls)
                c.Visible = false;
        }

        private void ShowEssayControls()
        {
            lblInstrBody.Text = activity.Instructions;
            txtEssay.Text = activity.EssayDraft;

            int top = 20;

            SetupControl(pnlInstructions, top);
            top += pnlInstructions.Height + 20;

            SetupControl(lblEssayPrompt, top);
            top += lblEssayPrompt.Height + 10;

            SetupControl(txtEssay, top);
            txtEssay.Height = 280;
            top += txtEssay.Height + 10;

            SetupControl(lblWordCount, top);
            top += lblWordCount.Height + 20;

            SetupControl(btnSubmitEssay, top);

            UpdateWordCount();
            StretchControls();
        }


        private void ShowAssignmentControls()
        {
            lblInstrBody2.Text = activity.Instructions;

            btnSubmitAssign.Text =
                activity.Type == "FileUpload"
                ? "Upload File"
                : "Submit Assignment";

            int top = 20;

            SetupControl(pnlInstr2, top);
            top += pnlInstr2.Height + 20;

            if (activity.Type == "FileUpload")
            {
                SetupControl(lblFilePrompt, top);
                top += lblFilePrompt.Height + 10;

                SetupControl(pnlDropZone, top);
                top += pnlDropZone.Height + 20;
            }

            SetupControl(lblNotesPrompt, top);
            top += lblNotesPrompt.Height + 10;

            SetupControl(txtNotes, top);
            txtNotes.Height = 100;
            top += txtNotes.Height + 20;

            SetupControl(btnSubmitAssign, top);
            StretchControls();
        }


        private void SetupControl(Control control, int top)
        {
            control.Visible = true;
            control.Left = 20;
            control.Top = top;

            bool isButton = control is Button || control is buttonRounded;
            if (!isButton)
            {
                int availableWidth = Math.Max(100,
                    pnlBody.ClientSize.Width
                    - pnlBody.Padding.Left
                    - pnlBody.Padding.Right
                    - 40);
                control.Width = availableWidth;
            }

            if (!pnlBody.Controls.Contains(control))
                pnlBody.Controls.Add(control);
        }

        private void UpdateWordCount()
        {
            if (txtEssay == null || lblWordCount == null)
                return;

            string text = txtEssay.Text.Trim();

            int words =
                string.IsNullOrWhiteSpace(text)
                ? 0
                : text.Split(
                    new char[] { ' ', '\n', '\r', '\t' },
                    StringSplitOptions.RemoveEmptyEntries
                ).Length;

            lblWordCount.Text =
                $"Words: {words}  |  Characters: {text.Length}";
        }

        private void btnBack_Click(object sender, EventArgs e) => OnBack?.Invoke();


        private void txtEssay_TextChanged(object sender, EventArgs e)
        {
            UpdateWordCount();
        }

        private void btnSubmitEssay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEssay.Text))
            {
                MessageBox.Show(
                    "Please write your essay before submitting.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            activity.EssayDraft = txtEssay.Text;
            activity.SubmissionStatus = "Submitted";

            MessageBox.Show(
                "Essay submitted successfully!",
                "Submitted",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            OnBack?.Invoke();
        }

        private void btnSubmitAssign_Click(object sender, EventArgs e)
        {
            activity.SubmissionStatus = "Submitted";

            string verb =
                activity.Type == "FileUpload"
                ? "Uploaded"
                : "Submitted";

            MessageBox.Show(
                $"{verb} successfully!",
                "Done",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

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
            {
                MessageBox.Show(
                    $"File selected: {System.IO.Path.GetFileName(dlg.FileName)}",
                    "File Ready",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
    }
}