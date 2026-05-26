using System.Drawing.Drawing2D;
using MySqlConnector;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Models;
using PUPAcadPortal.PortalForms;
using PUPAcadPortal.Utils;
using ZstdSharp.Unsafe;

namespace PUPAcadPortal
{
    public partial class SignIn : Form
    {
        private Point _usableScreenLoc = Screen.PrimaryScreen.WorkingArea.Location;
        private Size _usableScreenSize = Screen.PrimaryScreen.WorkingArea.Size;
        public static User? authenticatedUser;
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

            authenticatedUser = null;

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
                        using (var context = new AppDbContext())
                        {
                            var user = context.Users.Include(u => u.Role).FirstOrDefault(u => u.Username == usernameInput);

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
                this.Hide();
                switch (authenticatedUser.Role.RoleName)
                {
                    case "Admin":
                        {
                            using (AdminPortal adminPortal = new AdminPortal())
                            {
                                adminPortal.Size = _usableScreenSize;
                                adminPortal.Location = _usableScreenLoc;
                                adminPortal.ShowDialog();
                            }
                            break;
                        }
                    case "Instructor":
                        {
                            using (InstructorPortal instructorPortal = new InstructorPortal())
                            {
                                instructorPortal.Size = _usableScreenSize;
                                instructorPortal.Location = _usableScreenLoc;
                                instructorPortal.ShowDialog();
                            }
                            break;
                        }
                    case "Student":
                        {
                            using (StudentPortal studentPortal = new StudentPortal(this))
                            {
                                studentPortal.Size = _usableScreenSize;
                                studentPortal.Location = _usableScreenLoc;
                                studentPortal.ShowDialog();
                            }
                            break;
                        }
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
            this.Size = _usableScreenSize;
            this.Location = _usableScreenLoc;
            txtUsername.Select();
        }

        // This method is for testing purposes only. It allows you to add a user with a hashed password to the database. DO NOT USE. WAG GAMITIN.
        public static void AddUser(string username, string plainTextPassword)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(plainTextPassword);
            string loweredUsername = username.ToLower();
            try
            {
                using (var context = new AppDbContext())
                {
                    // ==========================================
                    // 1. CHECK FOR OR CREATE THE ROLE
                    // ==========================================

                    // Look to see if the "Instructor" role already exists
                    var adminRole = context.Roles.FirstOrDefault(r => r.RoleName == "Student");

                    // If it doesn't exist, create it and save it to the database
                    if (adminRole == null)
                    {
                        adminRole = new Role
                        {
                            RoleName = "Student"
                        };

                        context.Roles.Add(adminRole);
                        context.SaveChanges(); // This assigns a real RoleID from MySQL
                    }

                    // ==========================================
                    // 2. CREATE THE NEW USER
                    // ==========================================

                    // Note: In a real app, never store plain text! Use BCrypt to hash it first.
                    string rawPassword = plainTextPassword;
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(rawPassword);

                    var newUser = new User
                    {
                        RoleId = adminRole.RoleId, // Link the user to the Admin role we just found/created
                        Username = username.ToLower(),
                        PasswordHash = hashedPassword,
                        Email = "DemoStudent@university.edu",
                        FirstName = "Demo",
                        LastName = "Student",
                        IsActive = true,
                        CreatedAt = DateTime.Now
                        // You can leave Birthdate, ContactNumber, etc. blank since they are NULLable in your DB
                    };

                    // Add the user to the context and push it to the live database
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

        private void AddUserForTestingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddUser(txtUsername.Text, txtPassword.Text);
        }
    }
}
