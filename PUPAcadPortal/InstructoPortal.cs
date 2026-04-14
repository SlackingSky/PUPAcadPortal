#nullable disable
using System;
using System.Collections;
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

            // Buhayin ang logic para sa Grades (Auto-Compute at Search)
            SetupGradeLogic();
        }

        // --- GRADE MANAGEMENT LOGIC (AUTO-COMPUTE & SEARCH) ---
        private void SetupGradeLogic()
        {
            // --- FIX: Lock down DataGridView UI settings ---
            if (dataGridView1 != null)
            {
                dataGridView1.AllowUserToAddRows = false;       // Prevents random blank row/edit button at the bottom
                dataGridView1.AllowUserToOrderColumns = false;  // Prevents column reordering
                dataGridView1.AllowUserToResizeColumns = false; // Prevents column resizing
                dataGridView1.AllowUserToResizeRows = false;    // Prevents row resizing
            }

            // --- FIX: Populate the Select Course ComboBox ---
            if (cmbSelectCourse != null && cmbSelectCourse.Items.Count == 0)
            {
                cmbSelectCourse.Items.Add("IT 101 - Introduction to Computing");
                cmbSelectCourse.Items.Add("CS 102 - Data Structures");
                cmbSelectCourse.Items.Add("IS 103 - Database Management");
                // Optional: Select the first item by default
                // cmbSelectCourse.SelectedIndex = 0; 
            }

            // 1. LALAGYAN NG SAMPLE DATA
            if (dataGridView1 != null && dataGridView1.Rows.Count == 0)
            {
                dataGridView1.Rows.Add("2021-00001-MN-0", "Eisen Nodesca", "85", "88");
                dataGridView1.Rows.Add("2021-00002-MN-0", "Clarisa Matias", "92", "95");
                dataGridView1.Rows.Add("2021-00003-MN-0", "Trisha Walang Last Name", "78", "82");
                dataGridView1.Rows.Add("2021-00004-MN-0", "Liza Soberano", "88", "90");
                dataGridView1.Rows.Add("2021-00005-MN-0", "Kween Yasmin", "72", "75");
                dataGridView1.Rows.Add("2021-00006-MN-0", "Maine Love Alden", "", "");
            }

            // 2. AUTO-CALCULATE GRADES LOGIC
            if (dataGridView1 != null)
            {
                dataGridView1.CellValueChanged += (s, e) =>
                {
                    // I-check kung Midterm (Column 2) o Finals (Column 3) ang in-edit
                    if (e.RowIndex >= 0 && (e.ColumnIndex == 2 || e.ColumnIndex == 3))
                    {
                        IList cells = dataGridView1.Rows[e.RowIndex].Cells;

                        // --- FIX: Added index [] to access specific cells ---
                        [cite_start] DataGridViewCell midCell = (DataGridViewCell)cells[2];
                        [cite_start] DataGridViewCell finCell = (DataGridViewCell)cells[3];
                        [cite_start] DataGridViewCell avgCell = (DataGridViewCell)cells[4];
                        [cite_start] DataGridViewCell remCell = (DataGridViewCell)cells[5];

                        double m, f;
                        bool hasMid = double.TryParse(Convert.ToString(midCell.Value), out m);
                        bool hasFin = double.TryParse(Convert.ToString(finCell.Value), out f);

                        // Kung may laman pareho ang Midterm at Finals, i-compute
                        if (hasMid && hasFin)
                        {
                            double avg = (m + f) / 2.0;
                            avgCell.Value = avg.ToString("F2");
                            remCell.Value = avg >= 75.0 ? "Passed" : "Failed";
                            if (remCell.Style != null) remCell.Style.ForeColor = avg >= 75.0 ? Color.Green : Color.Red;
                        }
                        else
                        {
                            avgCell.Value = "";
                            remCell.Value = "Incomplete";
                            if (remCell.Style != null) remCell.Style.ForeColor = Color.Gray;
                        }
                    }
                };
            }

            // 3. SEARCH BAR LOGIC
            if (textBox1 != null)
            {
                textBox1.TextChanged += (s, e) =>
                {
                    string q = textBox1.Text != null ? textBox1.Text.Trim().ToLower() : "";

                    if (dataGridView1 != null)
                    {
                        // Kailangan i-null ang CurrentCell para makapag-hide tayo ng rows nang walang error
                        dataGridView1.CurrentCell = null;

                        foreach (DataGridViewRow r in dataGridView1.Rows)
                        {
                            if (r.IsNewRow) continue;

                            IList cells = r.Cells;

                            // --- FIX: Added index [] to fix the casting crash ---
                            DataGridViewCell cell0 = (DataGridViewCell)cells; // Student Number
                            [cite_start] DataGridViewCell cell1 = (DataGridViewCell)cells[1]; // Name

                            string sn = cell0.Value != null ? cell0.Value.ToString().ToLower() : "";
                            string nm = cell1.Value != null ? cell1.Value.ToString().ToLower() : "";

                            // I-hide o i-show ang row depende kung may match
                            r.Visible = string.IsNullOrEmpty(q) || sn.Contains(q) || nm.Contains(q);
                        }
                    }
                };
            }
        }

        // --- SIDEBAR NAVIGATION LOGIC ---
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
                pnlYellow.BringToFront();
            }
        }

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
                    content.Value.Parent = panel3;
                    content.Value.Dock = DockStyle.Fill;
                    content.Value.BringToFront();
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

        // --- EMPTY EVENT HANDLERS (DO NOT DELETE) ---
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
        private void panel93_Paint(object sender, PaintEventArgs e) { }

        private void panel101_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {

        }

        private void label109_Click(object sender, EventArgs e)
        {

        }

        private void label116_Click(object sender, EventArgs e)
        {

        }

        private void panel107_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    // --- DATA MODEL ---
    public class StudentGrade
    {
        public string StudentNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double? Midterm { get; set; }
        public double? Finals { get; set; }
        public double? Average => (Midterm.HasValue && Finals.HasValue) ? (Midterm + Finals) / 2.0 : null;
        public string Remarks => Average.HasValue ? (Average >= 75.0 ? "Passed" : "Failed") : "Incomplete";
    }
}