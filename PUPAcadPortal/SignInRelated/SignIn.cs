using BCrypt.Net;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using PUPAcadPortal.PortalForms;
using PUPAcadPortal.Services;
using PUPAcadPortal.Utils;
using System.Configuration;
using System.Drawing.Drawing2D;
using ZstdSharp.Unsafe;
using static PUPAcadPortal.Services.FileServerConnectService;

namespace PUPAcadPortal
{
    public partial class SignIn : Form
    {
        private Point _usableScreenLoc = Screen.PrimaryScreen.WorkingArea.Location;
        private Size _usableScreenSize = Screen.PrimaryScreen.WorkingArea.Size;
        public SignIn()
        {
            InitializeComponent();
            this.SetClientSizeCore(1513, 823);
            this.FormClosing += CloseApp.Form_Closing;
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

            // Declares who is trying to sign in
            User? authenticatedUser = null;

            using (LoadingForm loadingBox = new LoadingForm(this))
            {
                loadingBox.Show(this);
                this.Enabled = false;

                try
                {
                    string usernameInput = txtUsername.Text.ToLower().Trim();
                    string passwordInput = txtPassword.Text;

                    // Fetches data from database matching user credentials
                    authenticatedUser = await Task.Run(() =>
                    {
                        using (var context = new AppDbContext())
                        {
                            // Searches database with the same username
                            var user = context.Users.Include(u => u.Role).FirstOrDefault(u => u.Username == usernameInput);

                            // If found, compares password hash to database
                            if (user != null && BCrypt.Net.BCrypt.Verify(passwordInput, user.PasswordHash))
                            {
                                if (!(bool)user.IsActive)
                                {
                                    MessageBox.Show("Your account has been deactivated. Please contact the administrator.", "Account Disabled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                                // Fetches the respective IDs of the user, only 1 ID can be selected per user based on their role
                                (int? studentId, int? professorId, int?adminId) = user.Role.RoleName switch
                                {
                                    "Student" => (
                                        // v this line gets the user's student id since userid is a foreign key of the student table
                                        (int?)context.Students.Where(s => s.UserId == user.UserId).Select(s => (int?)s.StudentId).FirstOrDefault(),
                                        (int?)null,
                                        (int?)null
                                    ),

                                    "Professor" => (
                                        (int?)null,
                                        (int?)context.Professors.Where(p => p.UserId == user.UserId).Select(p => (int?)p.ProfessorId).FirstOrDefault(),
                                        (int?)null
                                    ),

                                    "Admin" => (
                                        (int?)null,
                                        (int?)null,
                                        (int?)context.Admins.Where(a => a.UserId == user.UserId).Select(a => (int?)a.AdminId).FirstOrDefault()
                                    ),

                                    _ => ((int?)null, (int?)null, (int?)null)
                                };

                                // This saves the login session
                                UserSession.Login
                                (
                                    username: user.Username,
                                    userID: user.UserId,
                                    firstName: user.FirstName,
                                    lastName: user.LastName,
                                    role: user.Role.RoleName,
                                    studentID: studentId,
                                    professorID: professorId,
                                    adminID: adminId
                                );
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
                // if authentication success, checks user roles and shows portal based on it
                using Form? form = authenticatedUser.Role.RoleName switch
                {
                    "Admin" => new AdminPortal(),
                    "Professor" => new ProfessorPortal(),
                    "Student" => new StudentPortal(),
                    _ => null
                };

                if (form != null)
                 {
                    form.Size = _usableScreenSize;
                    form.Location = _usableScreenLoc;
                    var systemService = new SystemService();
                    await systemService.LoadCurrentAcademicPeriodAsync();
                    form.ShowDialog();
                    txtUsername.Clear();
                    txtPassword.Clear();
                    this.Show();
                }
            }
            else
            {
                MessageBox.Show("Invalid username or password", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void SignIn_Load(object sender, EventArgs e)
        {
            // Gets credentials
            await DBConnectService.GetDecryptedConnectionStringAsync();
            await FileServerConnectService.GetDecryptedCredentialsAsync();
            this.Size = _usableScreenSize;
            this.Location = _usableScreenLoc;
            txtUsername.Select();
            lnkForgotPassword.TabStop = false;
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
                    var role = context.Roles.FirstOrDefault(r => r.RoleName == "Professor");


                    if (role == null)
                    {
                        role = new Role
                        {
                            RoleName = "Professor"
                        };

                        context.Roles.Add(role);
                        context.SaveChanges();
                    }

                    string rawPassword = plainTextPassword;
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(rawPassword);

                    var newUser = new User
                    {
                        RoleId = role.RoleId,
                        Username = username.ToLower(),
                        PasswordHash = hashedPassword,
                        PersonalEmail = "DemoProfessor@university.edu",
                        FirstName = "Demo",
                        LastName = "Professor",
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    };

                    context.Users.Add(newUser);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}");
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

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtUsername.Text))
                {
                    txtPassword.Focus();
                }
                else
                {
                    txtUsername.Focus();
                    txtUsername.BackColor = Color.FromArgb(255, 192, 192);
                    panel7.BackColor = Color.FromArgb(255, 192, 192);
                    lblUsernameWarn.Visible = true;
                }
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrEmpty(txtUsername.Text))
                {
                    txtUsername_KeyDown(sender, e);
                }
                else if (!string.IsNullOrEmpty(txtPassword.Text))
                {
                    btnSignIn.PerformClick();
                }
                else
                {
                    txtPassword.Focus();
                    txtPassword.BackColor = Color.FromArgb(255, 192, 192);
                    panel8.BackColor = Color.FromArgb(255, 192, 192);
                    lblPassWarn.Visible = true;
                }
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            lblUsernameWarn.Visible = false;
            txtUsername.BackColor = Color.White;
            panel7.BackColor = Color.White;
            PUPAcadPortal.Events.InfoChangedEvent.RaiseInfoChanged();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            lblPassWarn.Visible = false;
            txtPassword.BackColor = Color.White;
            panel8.BackColor = Color.White;
        }

        private void SignIn_Activated(object sender, EventArgs e)
        {
            txtUsername.Focus();
        }

        private void lnkForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var recoveryForm = new PUPAcadPortal.SignInRelated.AccountRecovery.ForgotPasswordForm())
            {
                recoveryForm.ShowDialog();
            }
        }
    }
}
