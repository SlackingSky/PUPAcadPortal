using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Utils
{
    public class CloseApp
    {

        public static void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Are you sure you want to exit?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                    Application.Exit();
            }
        }

        public static void Cancel_Closing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show("A database transaction is currently in progress. Please wait for it to finish before closing the application.",
                            "Action Blocked", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            e.Cancel = true;
        }
    }
}
