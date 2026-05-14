using System.Drawing.Drawing2D;
using MySqlConnector;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Models;

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
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter both username and password.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            try
            {
                using (var context = new DefaultdbContext())
                {
                    var user = context.Users.FirstOrDefault(u => u.Username == txtUsername.Text.ToLower().Trim());


                    if (user != null && BCrypt.Net.BCrypt.Verify(txtPassword.Text, user.PasswordHash))
                    {
                        var role = user.Role.ToLower().Trim();
                        if (role == "admin")
                        {
                            this.Hide();
                            AdminPortal adminPortal = new AdminPortal();
                            adminPortal.WindowState = this.WindowState;
                            adminPortal.ShowDialog();
                            txtUsername.Clear();
                            txtPassword.Clear();
                            this.Show(); ;
                        }
                        else if (role == "instructor")
                        {
                            this.Hide();
                            InstructorPortal instructorPortal = new InstructorPortal();
                            instructorPortal.WindowState = this.WindowState;
                            instructorPortal.ShowDialog();
                            txtUsername.Clear();
                            txtPassword.Clear();
                            this.Show(); ;
                        }
                        else if (role == "student")
                        {
                            this.Hide();
                            StudentPortal studentPortal = new StudentPortal(this);
                            studentPortal.WindowState = this.WindowState;
                            studentPortal.ShowDialog();
                            txtUsername.Clear();
                            txtPassword.Clear();
                            this.Show(); ;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Database authentication error: {ex.Message}");
                return;
            }
        }

        private void SignIn_Load(object sender, EventArgs e)
        {
            txtUsername.Select();
        }

        // This method is for testing purposes only. It allows you to add a user with a hashed password to the database. DO NOT USE. WAG GAMITIN.
        public static void AddUser(string username, string plainTextPassword)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(plainTextPassword);
            string loweredUsername = username.ToLower();
            try
            {
                using (var context = new DefaultdbContext())
                {
                    var newUser = new User
                    {
                        Username = loweredUsername,
                        PasswordHash = passwordHash,
                        Role = plainTextPassword,
                        DisplayName = username
                    };
                    context.Users.Add(newUser);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Database error: {ex.Message}");
            }
        }
    }
}
