using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.ReusableUserControls
{
    public partial class AdminRecentActivityReusable : UserControl
    {

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ActivityTitle
        {
            get => lblActTitle.Text;
            set => lblActTitle.Text = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ActivityTimeAgo
        {
            get => lblTimeAgo.Text;
            set
            {
                if (value.ToString() != "")
                {
                    TimeSpan diff = DateTime.Now - Convert.ToDateTime(value);

                    lblTimeAgo.Text = diff switch
                    {
                        { TotalMinutes: < 1 } => "Just Now",
                        { TotalMinutes: < 60 } => $"{Math.Floor(diff.TotalMinutes)}m ago",
                        { TotalHours: < 24 } => $"{Math.Floor(diff.TotalHours)}h ago",
                        { TotalDays: < 2 } => "Yesterday",
                        { TotalDays: < 7 } => $"{Math.Floor(diff.TotalDays)}d ago",
                        _ => $"{Math.Floor(diff.TotalDays / 7)}w ago"
                    };
                }
                else
                {
                    lblTimeAgo.Text = "";
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CreatedBy
        {
            get => lblCreatedBy.Text;
            set => lblCreatedBy.Text = value;
        }


        public AdminRecentActivityReusable()
        {
            InitializeComponent();
        }

        private void AdminRecentActivityReusable_Load(object sender, EventArgs e)
        {

        }
    }
}
