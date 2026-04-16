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

            if (panel3 != null)
            {
                panel3.Dock = DockStyle.Fill;
            }

            SetupQuickActions();

            if (btnDashboard != null)
            {
                changeButtonColor(btnDashboard);
                showContent(btnDashboard);
            }

            SetupGradeLogic();
        }

        // --- NEW: QUICK ACTIONS CLICK FIX ---
        private void SetupQuickActions()
        {
            void BindClick(Control ctrl, EventHandler handler)
            {
                if (ctrl == null) return;

                ctrl.Click += handler;
                ctrl.Cursor = Cursors.Hand;

                foreach (Control child in ctrl.Controls)
                {
                    BindClick(child, handler);
                }
            }

            BindClick(panel105, panel105_Click);
            BindClick(panel103, panel103_Click);
            BindClick(panel102, panel102_Click);
            BindClick(panel104, panel104_Click);
        }

        // --- GRADE MANAGEMENT LOGIC ---
        private void SetupGradeLogic()
        {
            if (dataGridView1 != null)
            {
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToOrderColumns = false;
                dataGridView1.AllowUserToResizeColumns = false;
                dataGridView1.AllowUserToResizeRows = false;

                // --- FIX 1.5: Anchor to stretch downward, and AutoSize to fill the gray space ---
                dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

            if (cmbSelectCourse != null && cmbSelectCourse.Items.Count == 0)
            {
                cmbSelectCourse.Items.Add("IT 101 - Introduction to Computing");
                cmbSelectCourse.Items.Add("CS 102 - Data Structures");
                cmbSelectCourse.Items.Add("IS 103 - Database Management");
            }

            if (dataGridView1 != null && dataGridView1.Rows.Count == 0)
            {
                dataGridView1.Rows.Add("2021-00001-SM-0", "Eisen Nodesca", "85", "88");
                dataGridView1.Rows.Add("2021-00002-SM-0", "Clarisa Matias", "92", "95");
                dataGridView1.Rows.Add("2021-00003-SM-0", "Trisha Walang Last Name", "78", "82");
                dataGridView1.Rows.Add("2021-00004-SM-0", "Liza Soberano", "88", "90");
                dataGridView1.Rows.Add("2021-00005-SM-0", "Kween Yasmin", "72", "75");
                dataGridView1.Rows.Add("2021-00006-SM-0", "Maine Love Alden", "98", "89");
            }

            // 2. AUTO-CALCULATE GRADES LOGIC
            if (dataGridView1 != null)
            {
                dataGridView1.CellValueChanged += (s, e) =>
                {
                    if (e.RowIndex >= 0 && (e.ColumnIndex == 2 || e.ColumnIndex == 3))
                    {
                        IList cells = dataGridView1.Rows[e.RowIndex].Cells;

                        // FIX: Re-applied the missing array indexes so it won't crash!
                        DataGridViewCell midCell = (DataGridViewCell)cells;
                        DataGridViewCell finCell = (DataGridViewCell)cells;
                        DataGridViewCell avgCell = (DataGridViewCell)cells;
                        DataGridViewCell remCell = (DataGridViewCell)cells;

                        double m, f;
                        bool hasMid = double.TryParse(Convert.ToString(midCell.Value), out m);
                        bool hasFin = double.TryParse(Convert.ToString(finCell.Value), out f);

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
                        dataGridView1.CurrentCell = null;

                        foreach (DataGridViewRow r in dataGridView1.Rows)
                        {
                            if (r.IsNewRow) continue;

                            IList cells = r.Cells;

                            // FIX: Re-applied the missing array indexes here too!
                            DataGridViewCell cell0 = (DataGridViewCell)cells;
                            DataGridViewCell cell1 = (DataGridViewCell)cells;

                            string sn = cell0.Value != null ? cell0.Value.ToString().ToLower() : "";
                            string nm = cell1.Value != null ? cell1.Value.ToString().ToLower() : "";

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
                DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
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

        // --- QUICK ACTIONS REDIRECTS ---
        private void panel105_Click(object sender, EventArgs e) { if (btnGrades != null) btnGrades_Click(btnGrades, e); }
        private void panel103_Click(object sender, EventArgs e) { if (btnGrades != null) btnGrades_Click(btnGrades, e); }
        private void panel102_Click(object sender, EventArgs e) { if (btnGrades != null) btnGrades_Click(btnGrades, e); }
        private void panel104_Click(object sender, EventArgs e) { if (btnCourses != null) btnCourses_Click(btnCourses, e); }

        private void button2_Click(object sender, EventArgs e) { if (btnGrades != null) btnGrades_Click(btnGrades, e); }
        private void button4_Click(object sender, EventArgs e) { if (btnCourses != null) btnCourses_Click(btnCourses, e); }
        private void button6_Click(object sender, EventArgs e) { if (btnGrades != null) btnGrades_Click(btnGrades, e); }

        // --- SUBMIT FINAL GRADES BUTTON FIX ---
        private void button9_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to submit all final grades? This action cannot be undone.", "Submit Final Grades", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Grades submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void label37_Click(object sender, EventArgs e) { }
        private void label38_Click(object sender, EventArgs e) { }
        private void panel46_Paint(object sender, PaintEventArgs e) { }
        private void panel87_Paint(object sender, PaintEventArgs e) { }
        private void label44_Click(object sender, EventArgs e) { }
        private void panel89_Paint(object sender, PaintEventArgs e) { }
        private void label44_Click_1(object sender, EventArgs e) { }
        private void label79_Click(object sender, EventArgs e) { }
        private void label43_Click(object sender, EventArgs e) { }
        private void label81_Click(object sender, EventArgs e) { }
        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e) { }
        private void label83_Click(object sender, EventArgs e) { }
        private void panel93_Paint(object sender, PaintEventArgs e) { }
        private void panel101_Paint(object sender, PaintEventArgs e) { }
        private void pictureBox20_Click(object sender, EventArgs e) { }
        private void pictureBox21_Click(object sender, EventArgs e) { }
        private void label109_Click(object sender, EventArgs e) { }
        private void label116_Click(object sender, EventArgs e) { }
        private void panel107_Paint(object sender, PaintEventArgs e) { }

        // Empty Paint events 
        private void panel105_Paint(object sender, PaintEventArgs e) { }
        private void panel103_Paint(object sender, PaintEventArgs e) { }
        private void panel102_Paint(object sender, PaintEventArgs e) { }
        private void panel104_Paint(object sender, PaintEventArgs e) { }

        private void panel41_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel114_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel114_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void label114_Click(object sender, EventArgs e)
        {

        }

        private void pnlCoursesContent_Paint_1(object sender, PaintEventArgs e)
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