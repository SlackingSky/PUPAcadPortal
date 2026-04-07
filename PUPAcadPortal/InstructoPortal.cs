#nullable disable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class InstructorPortal : Form
    {
        private Button clickedButton;
        private Color defaultColor = Color.Maroon;
        private Color selectedColor = Color.FromArgb(109, 0, 0);

        public InstructorPortal()
        {
            InitializeComponent();

            // Show the dashboard by default when the app opens
            if (btnDashboard != null)
            {
                changeButtonColor(btnDashboard);
                showContent(btnDashboard);
            }
        }

        // Handles the Gold Bar and button color on the sidebar
        private void changeButtonColor(Button button)
        {
            if (button == null) return;

            if (clickedButton != null)
            {
                clickedButton.BackColor = defaultColor;
            }

            clickedButton = button;

            if (pnlYellow == null)
            {
                pnlYellow = new Panel
                {
                    Name = "pnlYellow",
                    Size = new Size(6, 64),
                    BackColor = Color.Gold,
                    Visible = false
                };
                if (pnlSidebar != null)
                    pnlSidebar.Controls.Add(pnlYellow);
                else
                    this.Controls.Add(pnlYellow);
            }

            if (clickedButton != null && clickedButton.Parent != null)
            {
                clickedButton.BackColor = selectedColor;
                pnlYellow.Visible = true;
                pnlYellow.Location = new Point(0, clickedButton.Location.Y);
                pnlYellow.BringToFront(); // Ensure the gold bar isn't hidden
            }
        }

        // Handles hiding and showing your Designer panels cleanly
        private void showContent(Button button)
        {
            if (button == null) return;

            var contents = new Dictionary<Button, Panel>();
            if (btnDashboard != null && pnlDashboardContent != null) contents.Add(btnDashboard, pnlDashboardContent);
            if (btnGrades != null && pnlGradesContent != null) contents.Add(btnGrades, pnlGradesContent);
            if (btnCourses != null && pnlCoursesContent != null) contents.Add(btnCourses, pnlCoursesContent);

            foreach (var content in contents)
            {
                if (content.Key == button)
                {
                    content.Value.Visible = true;
                    // Dock = Fill ensures it doesn't get cut off when fullscreen
                    content.Value.Parent = panel3;
                    content.Value.Dock = DockStyle.Fill;
                    content.Value.BringToFront(); // Forces it to the top so nothing overlaps it
                }
                else
                {
                    if (content.Value != null)
                    {
                        content.Value.Visible = false;
                    }
                }
            }
        }

        private void StudentPortal_Closing(object sender, FormClosingEventArgs e)
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

        // --- BUTTON CLICKS ---
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnGrades_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnCourses_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnLMS_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
            if (pnllmsSubmenu != null && btnLMS != null)
            {
                pnllmsSubmenu.Visible = !pnllmsSubmenu.Visible;
                if (pnllmsSubmenu.Visible)
                    btnLMS.Text = " LMS                                       ⌄";
                else
                    btnLMS.Text = " LMS                                       ›";
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // --- KEEP YOUR EMPTY EVENT HANDLERS HERE SO THE DESIGNER DOESNT BREAK ---
        private void pnlCoursesContent_Paint(object sender, PaintEventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void lblWelcome_Click(object sender, EventArgs e) { }
        private void pnlBanner_Paint(object sender, PaintEventArgs e) { }
        private void lblWelcomeSubtitle_Click(object sender, EventArgs e) { }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
        private void flowstats_Paint(object sender, PaintEventArgs e) { }
        private void panel3_Paint(object sender, PaintEventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void pictureBox3_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void label9_Click(object sender, EventArgs e) { }
        private void label11_Click(object sender, EventArgs e) { }
        private void label13_Click(object sender, EventArgs e) { }
        private void label13_Click_1(object sender, EventArgs e) { }
        private void panel17_Paint(object sender, PaintEventArgs e) { }
        private void label14_Click(object sender, EventArgs e) { }
        private void label16_Click(object sender, EventArgs e) { }
        private void label19_Click(object sender, EventArgs e) { }
        private void label20_Click(object sender, EventArgs e) { }
        private void label20_Click_1(object sender, EventArgs e) { }
        private void label20_Click_2(object sender, EventArgs e) { }
        private void label21_Click(object sender, EventArgs e) { }
        private void pictureBox7_Click(object sender, EventArgs e) { }
        private void pictureBox7_Click_1(object sender, EventArgs e) { }
        private void panel40_Paint(object sender, PaintEventArgs e) { }
        private void label32_Click(object sender, EventArgs e) { }
        private void label34_Click(object sender, EventArgs e) { }
        private void label36_Click(object sender, EventArgs e) { }
        private void cmbSelectCourse_Paint(object sender, PaintEventArgs e) { }
        private void flowLayoutPanel2_Paint_1(object sender, PaintEventArgs e) { }
        private void button2_Click(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void label37_Click(object sender, EventArgs e) { }
        private void label38_Click(object sender, EventArgs e) { }
        private void button4_Click(object sender, EventArgs e) { }
        private void panel46_Paint(object sender, PaintEventArgs e) { }
        private void button6_Click(object sender, EventArgs e) { }
        private void panel87_Paint(object sender, PaintEventArgs e) { }
        private void button9_Click(object sender, EventArgs e) { }
        private void label44_Click(object sender, EventArgs e) { }
        private void panel89_Paint(object sender, PaintEventArgs e) { }
        private void label44_Click_1(object sender, EventArgs e) { }
        private void label79_Click(object sender, EventArgs e) { }
        private void label43_Click(object sender, EventArgs e) { }
        private void label81_Click(object sender, EventArgs e) { }
        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e) { }
        private void label83_Click(object sender, EventArgs e) { }
    }
}