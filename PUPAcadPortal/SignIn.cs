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

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.ToLower() == "student")
            {
                this.Hide();
                StudentPortal studentPortal = new StudentPortal();
                studentPortal.WindowState = this.WindowState;
                studentPortal.StartPosition = FormStartPosition.CenterScreen;
                studentPortal.Show();
            }
            else if (txtUsername.Text.ToLower() == "admin")
            {
                this.Hide();
                AdminPortal adminPortal = new AdminPortal();
                adminPortal.WindowState = this.WindowState;
                adminPortal.StartPosition = FormStartPosition.CenterScreen;
                adminPortal.Show();
            }
            else if (txtUsername.Text.ToLower() == "instructor")
            {
                this.Hide();
                InstructorPortal instructorPortal = new InstructorPortal();
                instructorPortal.WindowState = this.WindowState;
                instructorPortal.StartPosition = FormStartPosition.CenterScreen;
                instructorPortal.Show();
            }
            else
            {
                MessageBox.Show("Invalid username. Please enter 'student', 'admin', or 'instructor'.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
