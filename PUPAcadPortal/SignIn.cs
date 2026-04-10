//ORIGINAL CODE (tinanggal ko lang muna for testing purposes (sam))
//using System.Drawing.Drawing2D;

//namespace PUPAcadPortal
//{
//    public partial class SignIn : Form
//    {
//        public SignIn()
//        {
//            InitializeComponent();
//            this.SetClientSizeCore(1513, 823);
//        }

//        private void btnShowPass_Click(object sender, EventArgs e)
//        {
//            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
//        }

//        private void button1_Click(object sender, EventArgs e)
//        {
//            this.Hide();
//            StudentPortal studentPortal = new StudentPortal();
//            studentPortal.WindowState = this.WindowState;
//            studentPortal.Show();
//        }
//    }
//}

//SAM - Updated SignIn.cs to include basic username validation and open different portals based on the username entered. Admin users will be directed to the Admin Portal, while student users will be directed to the Student Portal. If an invalid username is entered, an error message will be displayed.
// Note: This is a very basic implementation for demonstration purposes. In a real application, you would want to implement proper authentication and security measures. 
// Also, you can remove this code kapag nag-pull request na tayo sa main branch, since this is just for testing purposes.

using System.Drawing.Drawing2D;

namespace PUPAcadPortal
{
    public partial class SignIn : Form
    {
        public SignIn()
        {
            InitializeComponent();
            this.SetClientSizeCore(1513, 823);
        }

        private void btnShowPass_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            // For demo purposes, only check username. 
            // In a real app, validate both username and password against a database.
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please enter your username.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (username.Equals("a", StringComparison.OrdinalIgnoreCase))
            {
                // Open Admin Portal
                this.Hide();
                AdminPortal adminPortal = new AdminPortal();
                adminPortal.WindowState = this.WindowState;
                adminPortal.Show();
            }
            else if (username.Equals("s", StringComparison.OrdinalIgnoreCase))
            {
                // Open Student Portal
                this.Hide();
                StudentPortal studentPortal = new StudentPortal();
                studentPortal.WindowState = this.WindowState;
                studentPortal.Show();
            }
            else
            {
                MessageBox.Show("Invalid username. Please enter 'a' for admin or 's' for student.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
                e.SuppressKeyPress = true;
            }
        }
    }
}
