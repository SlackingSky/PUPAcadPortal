using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class ActivityItem : UserControl
    {
        public ActivityItem()
        {
            InitializeComponent();
        }

        private void actPic_Click(object sender, EventArgs e)
        {

        }
        public void SetActivityData(string title, string dueDate, Image icon)
        {
            lblTitle.Text = title;
            lblDueDate.Text = $"Due : {dueDate}";
            actPic.Image = icon;
        }

        public string SavedQuestion;
        public string[] SavedChoices = new string[4];
        public string SavedTitle;

        // You also need a public method to receive the data
        public void UpdateCardData(string title, string q, string a, string b, string c, string d)
        {
            lblTitle.Text = title;
            SavedTitle = title;
            SavedQuestion = q;
            SavedChoices[0] = a;
            SavedChoices[1] = b;
            SavedChoices[2] = c;
            SavedChoices[3] = d;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }
    }
}
