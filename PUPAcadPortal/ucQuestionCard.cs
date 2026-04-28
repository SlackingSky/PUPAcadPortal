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
            lblCorrectAns.Text = "Correct Answer: ";
        }

        private void cmbCorrectAnswer_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update the label only within this file
            lblCorrectAns.Text = "Correct: " + cmbCorrectAnswer.Text;
        }
    }
}