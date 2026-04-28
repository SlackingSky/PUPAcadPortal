using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class ActivityItem : UserControl
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SavedCorrectAnswer { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SavedInstructions { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SavedAttachedFilePath { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SavedTitle { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SavedQuestion { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] SavedChoices { get; set; } = new string[4];

        public ActivityItem()
        {
            InitializeComponent();
        }

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

            // Image is updated here, but the Label logic is now in quizCreation
            actPic.Image = Properties.Resources.quiz;
        }

        public void SetActivityData(string title, string dueDate, Image icon)
        {
            lblTitle.Text = title;
            lblDueDate.Text = $"Due : {dueDate}";
            actPic.Image = icon;
        }
    }
}