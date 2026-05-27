using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Utils
{
    public class LogOut
    {

        public static void LogOut_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Form form = button.FindForm();
            if (MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                form.Hide();
            }
        }
    }
}
