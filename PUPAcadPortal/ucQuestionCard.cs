using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class quizCreation : UserControl
    {
        public quizCreation()
        {
            InitializeComponent();

        }

        private void ucQuestionCard_Load(object sender, EventArgs e)
        {
            // Initial label state
            lblCorrectAns.Text = "Selected Correct Answer: ";
        }

        private void cmbCorrectAnswer_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if an item is actually selected to avoid null errors
            if (cmbCorrectAnswer.SelectedItem != null)
            {
                // Update the label text immediately
                // Make sure 'lblCorrectAns' matches the Name in your Properties window
                lblCorrectAns.Text = "Selected Correct Answer: " + cmbCorrectAnswer.SelectedItem.ToString();
            }
        }
    }
}