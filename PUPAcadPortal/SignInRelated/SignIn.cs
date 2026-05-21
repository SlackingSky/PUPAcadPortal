using System.Drawing.Drawing2D;
using MySqlConnector;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Models;
using PUPAcadPortal.PortalForms;

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

        private async void btnSignIn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter both username and password.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            User authenticatedUser = null;

            using (LoadingForm loadingBox = new LoadingForm(this))
            {
                loadingBox.Show(this);
                this.Enabled = false;

                try
                {
                    string usernameInput = txtUsername.Text.ToLower().Trim();
                    string passwordInput = txtPassword.Text;

                    authenticatedUser = await Task.Run(() =>
                    {
                        using (var context = new DefaultdbContext())
                        {
                            var user = context.Users.FirstOrDefault(u => u.Username == usernameInput);

                            if (user != null && BCrypt.Net.BCrypt.Verify(passwordInput, user.PasswordHash))
                            {
                                return user;
                            }
                        }
                        return null;
                    });
                }
                catch (TimeoutException)
                {
                    this.Enabled = true;
                    loadingBox.Close();

                    MessageBox.Show(
                        "The login server is currently offline or unreachable. Please check your internet connection or try again later. Chat me (Brylle) for assistance.",
                        "Database Offline",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }
                catch (System.Data.Common.DbException ex)
                {
                    this.Enabled = true;
                    loadingBox.Close();

                    MessageBox.Show(
                        $"Database configuration error. The system could not connect. Chat me (Brylle) for assistance.\n\nDetails: {ex.Message}",
                        "Connection Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }
                catch (Exception ex)
                {
                    this.Enabled = true;
                    loadingBox.Close();

                    MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    this.Enabled = true;
                    if (!loadingBox.IsDisposed)
                    {
                        loadingBox.Close();
                    }
                }
            }

            if (authenticatedUser != null)
            {
                var role = authenticatedUser.Role.ToLower().Trim();

                this.Hide();

                if (role == "admin")
                {
                    AdminPortal adminPortal = new AdminPortal();
                    adminPortal.WindowState = this.WindowState;
                    adminPortal.ShowDialog();
                }
                else if (role == "instructor")
                {
                    InstructorPortal instructorPortal = new InstructorPortal();
                    instructorPortal.WindowState = this.WindowState;
                    instructorPortal.ShowDialog();
                }
                else if (role == "student")
                {
                    StudentPortal studentPortal = new StudentPortal(this);
                    studentPortal.WindowState = this.WindowState;
                    studentPortal.ShowDialog();
                }

                txtUsername.Clear();
                txtPassword.Clear();
                this.Show();
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

        private void panel7_Click(object sender, EventArgs e)
        {
            txtUsername.Focus();
        }

        private void panel8_Click(object sender, EventArgs e)
        {
            txtPassword.Focus();
        }
    }
}
