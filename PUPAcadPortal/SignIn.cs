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
