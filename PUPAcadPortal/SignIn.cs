using System.Drawing.Drawing2D;
using MySqlConnector;
using BCrypt.Net;

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
            if (Database.VerifyUserLogin(txtUsername.Text, txtPassword.Text))
            {
                this.Hide();
                StudentPortal studentPortal = new StudentPortal(this);
                studentPortal.WindowState = this.WindowState;
                studentPortal.ShowDialog();
                txtUsername.Clear();
                txtPassword.Clear();
                this.Show();
            }
            else if (Database.VerifyUserLogin(txtUsername.Text, txtPassword.Text))
            {
                this.Hide();
                AdminPortal adminPortal = new AdminPortal();
                adminPortal.WindowState = this.WindowState;
                adminPortal.ShowDialog();
                txtUsername.Clear();
                txtPassword.Clear();
                this.Show(); ;
            }
            else if (Database.VerifyUserLogin(txtUsername.Text, txtPassword.Text))
            {
                this.Hide();
                InstructorPortal instructorPortal = new InstructorPortal();
                instructorPortal.WindowState = this.WindowState;
                instructorPortal.ShowDialog();
                txtUsername.Clear();
                txtPassword.Clear();
                this.Show(); ;
            }
            else
            {
                MessageBox.Show("Invalid username or password", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SignIn_Load(object sender, EventArgs e)
        {
            txtUsername.Select();
        }
    }
}
