using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class ActivityItem : UserControl
    {
        // Initializing with string.Empty fixes the CS8618 non-nullable warnings
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SavedCorrectAnswer { get; set; } = "";

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SavedInstructions { get; set; } = string.Empty;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SavedAttachedFilePath { get; set; } = string.Empty;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SavedTitle { get; set; } = string.Empty;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SavedQuestion { get; set; } = string.Empty;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] SavedChoices { get; set; } = new string[4] { "", "", "", "" };

        public ActivityItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Updates the summary item with data from the quiz creation cards.
        /// </summary>
        public void UpdateQuizData(string title, string q, string a, string b, string c, string d, string correct)
        {
            lblTitle.Text = title;
            SavedTitle = title;
            SavedQuestion = q;
            SavedChoices[0] = a;
            SavedChoices[1] = b;
            SavedChoices[2] = c;
            SavedChoices[3] = d;
            SavedCorrectAnswer = correct;

            // Set the icon to the quiz resource
            actPic.Image = Properties.Resources.quiz;
        }

        /// <summary>
        /// General method for setting activity display data.
        /// </summary>
        public void SetActivityData(string title, string dueDate, Image icon)
        {
            lblTitle.Text = title;
            if (lblDueDate != null)
            {
                lblDueDate.Text = $"Due : {dueDate}";
            }
            actPic.Image = icon;
        }

        public void SetAsPosted()
        {
            btnEdit.Visible = false; // Hide edit icon
            btnPostAct.Text = "View";
            SavedTitle ??= string.Empty;
            SavedQuestion ??= string.Empty;
        }

        private void btnPostAct_Click(object sender, EventArgs e)
        {
            var mainForm = this.FindForm() as InstructorPortal;
            if (mainForm == null) return;

            string currentState = btnPostAct.Text.Trim().ToLower();

            if (currentState == "post")
            {
                if (MessageBox.Show("Post this activity to the class?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    mainForm.FlowPostedAct.Controls.Add(this);
                    mainForm.FlowPostedAct.Controls.SetChildIndex(this, 0);

                    this.btnEdit.Visible = false;
                    this.btnPostAct.Text = "View";

                    // Position trash icon near the 'View' button
                    btnRemove.Location = new Point(this.Width - 250, 58);

                }
            }
            else if (currentState == "view")
            {
                if (mainForm.pnlViewActivity != null)
                {
                    mainForm.pnlViewActivity.Visible = true;
                    mainForm.pnlViewActivity.BringToFront();
                    mainForm.pnlViewActivity.Dock = DockStyle.Fill;
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            // 1. Confirm deletion
            DialogResult confirm = MessageBox.Show("Delete this activity permanently?", "Confirm Delete",
                                                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                // 2. Use 'this.Parent' so it works in BOTH ManageAct and FlowPostedAct
                if (this.Parent != null)
                {
                    this.Parent.Controls.Remove(this);

                    // 3. Optional: Dispose to free up memory
                    this.Dispose();
                }
            }
        }
    }
}