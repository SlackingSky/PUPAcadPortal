using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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
            SetupDashboardUI();
            SetupGradesUI();
        }

        private void SetupGradesUI()
        {
            // Clear any existing controls in grades panel
            pnlGradesContent.Controls.Clear();

            // Top controls: title, select course, search, upload/download
            Label lblTitle = new Label();
            lblTitle.Text = "Grade Management";
            lblTitle.Font = new Font("Arial", 18F, FontStyle.Bold, GraphicsUnit.Pixel);
            lblTitle.ForeColor = Color.FromArgb(109, 0, 0);
            lblTitle.Location = new Point(16, 12);
            lblTitle.Size = new Size(300, 28);

            ComboBox cmbCourse = new ComboBox();
            cmbCourse.Location = new Point(340, 12);
            cmbCourse.Size = new Size(240, 26);
            cmbCourse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCourse.Items.AddRange(new object[] { "Select Course", "Course 1", "Course 2" });
            cmbCourse.SelectedIndex = 0;

            TextBox txtSearch = new TextBox();
            txtSearch.PlaceholderText = "Search by name or student number...";
            txtSearch.Location = new Point(600, 12);
            txtSearch.Size = new Size(320, 26);

            Button btnUpload = new Button();
            btnUpload.Text = "Upload Grades";
            btnUpload.Location = new Point(940, 12);
            btnUpload.Size = new Size(120, 28);

            Button btnDownload = new Button();
            btnDownload.Text = "Download";
            btnDownload.Location = new Point(1072, 12);
            btnDownload.Size = new Size(90, 28);

            // DataGridView for grades
            DataGridView dgv = new DataGridView();
            dgv.Location = new Point(16, 56);
            dgv.Size = new Size(pnlGradesContent.ClientSize.Width - 40, pnlGradesContent.ClientSize.Height - 160);
            dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // columns
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "StudentNumber", HeaderText = "Student Number", Width = 140 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Name", HeaderText = "Name", Width = 220 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Midterm", HeaderText = "Midterm", Width = 80 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Finals", HeaderText = "Finals", Width = 80 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Average", HeaderText = "Average", ReadOnly = true, Width = 100 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Remarks", HeaderText = "Remarks", ReadOnly = true, Width = 120 });
            DataGridViewButtonColumn actionCol = new DataGridViewButtonColumn();
            actionCol.Name = "Action";
            actionCol.HeaderText = "Action";
            actionCol.Text = "Upload";
            actionCol.UseColumnTextForButtonValue = true;
            actionCol.Width = 90;
            dgv.Columns.Add(actionCol);

            // sample rows
            void AddRow(string sn, string name, string mid, string fin)
            {
                int rowIndex = dgv.Rows.Add(sn, name, mid, fin, "", "", "Upload");
                DataGridViewRow row = dgv.Rows[rowIndex];
                double m, f;
                if (double.TryParse(mid, out m) && double.TryParse(fin, out f))
                {
                    double avg = (m + f) / 2.0;
                    row.Cells[4].Value = avg.ToString("F2");
                    row.Cells[5].Value = avg >= 75.0 ? "Passed" : "Failed";
                }
                else
                {
                    row.Cells[4].Value = "";
                    row.Cells[5].Value = "Incomplete";
                }
            }

            AddRow("20230001", "Student 1", "85", "88");
            AddRow("20230002", "Student 2", "92", "95");
            AddRow("20230003", "Student 3", "78", "82");
            AddRow("20230004", "Student 4", "88", "90");
            AddRow("20230005", "Student 5", "72", "75");
            AddRow("20230006", "Student 6", "", "");

            // bottom submission area
            Panel pnlSubmit = new Panel();
            pnlSubmit.Location = new Point(16, pnlGradesContent.ClientSize.Height - 88);
            pnlSubmit.Size = new Size(pnlGradesContent.ClientSize.Width - 40, 64);
            pnlSubmit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            Label lblReady = new Label();
            lblReady.Text = "Ready to Submit?";
            lblReady.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
            lblReady.Location = new Point(6, 12);
            lblReady.Size = new Size(200, 22);

            Button btnSubmit = new Button();
            btnSubmit.Text = "Submit Final Grades";
            btnSubmit.Location = new Point(pnlSubmit.Width - 180, 12);
            btnSubmit.Size = new Size(160, 32);
            btnSubmit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSubmit.Click += (s, e) =>
            {
                if (MessageBox.Show("Please review all grades before final submission. Submit final grades?", "Submit Final Grades", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    MessageBox.Show("Final grades submitted.", "Submitted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            pnlSubmit.Controls.Add(lblReady);
            pnlSubmit.Controls.Add(btnSubmit);

            // wire up search filtering
            txtSearch.TextChanged += (s, e) =>
            {
                string q = txtSearch.Text.Trim().ToLower();
                foreach (DataGridViewRow r in dgv.Rows)
                {
                    if (r.IsNewRow) continue;
                    string sn = (r.Cells[0].Value ?? string.Empty).ToString().ToLower();
                    string nm = (r.Cells[1].Value ?? string.Empty).ToString().ToLower();
                    r.Visible = string.IsNullOrEmpty(q) || sn.Contains(q) || nm.Contains(q);
                }
            };

            // action column click stub
            dgv.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == dgv.Columns["Action"].Index)
                {
                    MessageBox.Show("Upload for student " + dgv.Rows[e.RowIndex].Cells[1].Value, "Upload", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            // add controls to panel
            pnlGradesContent.Controls.Add(lblTitle);
            pnlGradesContent.Controls.Add(cmbCourse);
            pnlGradesContent.Controls.Add(txtSearch);
            pnlGradesContent.Controls.Add(btnUpload);
            pnlGradesContent.Controls.Add(btnDownload);
            pnlGradesContent.Controls.Add(dgv);
            pnlGradesContent.Controls.Add(pnlSubmit);
        }

        private void SetupDashboardUI()
        {
            // Create banner
            pnlBanner = new Panel();
            pnlBanner.Location = new Point(18, 16);
            pnlBanner.Name = "pnlBanner";
            pnlBanner.Size = new Size(1418, 136);
            pnlBanner.BackColor = Color.FromArgb(109, 0, 0);
            pnlBanner.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            lblWelcome = new Label();
            lblWelcome.AutoSize = false;
            lblWelcome.Location = new Point(35, 18);
            lblWelcome.Size = new Size(1100, 44);
            lblWelcome.Font = new Font("Arial", 35F, FontStyle.Bold, GraphicsUnit.Pixel);
            lblWelcome.ForeColor = Color.White;
            lblWelcome.Text = "Welcome back, Professor!";

            lblWelcomeSubtitle = new Label();
            lblWelcomeSubtitle.AutoSize = false;
            lblWelcomeSubtitle.Location = new Point(24, 66);
            lblWelcomeSubtitle.Size = new Size(1100, 28);
            lblWelcomeSubtitle.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblWelcomeSubtitle.ForeColor = Color.FromArgb(230, 230, 230);
            lblWelcomeSubtitle.Text = "Manage your courses and student grades";

            pnlBanner.Controls.Add(lblWelcome);
            pnlBanner.Controls.Add(lblWelcomeSubtitle);
            pnlDashboardContent.Controls.Add(pnlBanner);

            // Anonymize sidebar name/role
            try
            {
                label1.Text = "Professor"; // anonymized role label
                label2.Text = ""; // no personal name shown
            }
            catch { }

            // Stats
            pnlStats = new Panel();
            pnlStats.Location = new Point(18, 168);
            pnlStats.Name = "pnlStats";
            pnlStats.Size = new Size(1418, 108);
            pnlStats.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            string[] statTitles = { "Active Courses", "Total Students", "Graded Assignments", "Pending Grades" };
            string[] statValues = { "3", "120", "45", "15" };
            Color[] statColors = { Color.DodgerBlue, Color.SeaGreen, Color.MediumPurple, Color.Gold };

            for (int i = 0; i < 4; i++)
            {
                Panel card = new Panel();
                card.Size = new Size(328, 92);
                card.Location = new Point(i * 342, 0);
                card.BackColor = Color.White;
                card.Padding = new Padding(12);

                Label t = new Label();
                t.AutoSize = false;
                t.Size = new Size(220, 18);
                t.Location = new Point(8, 8);
                t.Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Pixel);
                t.ForeColor = Color.FromArgb(100, 0, 0, 0);
                t.Text = statTitles[i];

                Label n = new Label();
                n.AutoSize = false;
                n.Size = new Size(140, 30);
                n.Location = new Point(8, 28);
                n.Font = new Font("Arial", 18F, FontStyle.Bold, GraphicsUnit.Pixel);
                n.ForeColor = Color.FromArgb(40, 40, 40);
                n.Text = statValues[i];

                Panel icon = new Panel();
                icon.Size = new Size(44, 44);
                icon.Location = new Point(268, 24);
                icon.BackColor = statColors[i];

                card.Controls.Add(t);
                card.Controls.Add(n);
                card.Controls.Add(icon);
                pnlStats.Controls.Add(card);
            }
            pnlDashboardContent.Controls.Add(pnlStats);

            // Lower main area: Quick Actions and Upcoming Events
            pnlMainLower = new Panel();
            pnlMainLower.Location = new Point(18, 292);
            pnlMainLower.Name = "pnlMainLower";
            pnlMainLower.Size = new Size(1418, 980);
            pnlMainLower.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // Quick Actions
            pnlQuickActions = new Panel();
            pnlQuickActions.Location = new Point(0, 0);
            pnlQuickActions.Size = new Size(960, 960);
            pnlQuickActions.BackColor = Color.White;
            pnlQuickActions.Padding = new Padding(12);

            Label lblQA = new Label();
            lblQA.Text = "Quick Actions";
            lblQA.Font = new Font("Arial", 16F, FontStyle.Bold, GraphicsUnit.Pixel);
            lblQA.ForeColor = Color.FromArgb(109, 0, 0);
            lblQA.Location = new Point(12, 12);
            lblQA.Size = new Size(300, 26);
            pnlQuickActions.Controls.Add(lblQA);

            string[,] actions = new string[2, 2] {
                { "Grade Submissions", "Class Schedule" },
                { "Course Materials", "Student List" }
            };
            string[,] actionsSub = new string[2, 2] {
                { "Submit student grades", "View teaching schedule" },
                { "Upload course resources", "View enrolled students" }
            };

            for (int r = 0; r < 2; r++)
            {
                for (int c = 0; c < 2; c++)
                {
                    Panel action = new Panel();
                    action.Size = new Size(440, 140);
                    action.Location = new Point(12 + c * 452, 56 + r * 164);
                    action.BackColor = Color.FromArgb(245, 245, 245);

                    Label actionTitle = new Label();
                    actionTitle.Text = actions[r, c];
                    actionTitle.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
                    actionTitle.Location = new Point(72, 16);
                    actionTitle.Size = new Size(340, 20);

                    Label actionSub = new Label();
                    actionSub.Text = actionsSub[r, c];
                    actionSub.Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Pixel);
                    actionSub.ForeColor = Color.FromArgb(120, 120, 120);
                    actionSub.Location = new Point(72, 40);
                    actionSub.Size = new Size(340, 20);

                    Panel actionIcon = new Panel();
                    actionIcon.Size = new Size(52, 52);
                    actionIcon.Location = new Point(8, 40);
                    actionIcon.BackColor = Color.FromArgb(109, 0, 0);

                    action.Controls.Add(actionIcon);
                    action.Controls.Add(actionTitle);
                    action.Controls.Add(actionSub);
                    pnlQuickActions.Controls.Add(action);
                }
            }
            pnlMainLower.Controls.Add(pnlQuickActions);

            // Upcoming Events
            pnlUpcoming = new Panel();
            pnlUpcoming.Location = new Point(984, 0);
            pnlUpcoming.Size = new Size(430, 960);
            pnlUpcoming.BackColor = Color.White;

            Label lblUpcoming = new Label();
            lblUpcoming.Text = "Upcoming Events";
            lblUpcoming.Font = new Font("Arial", 16F, FontStyle.Bold, GraphicsUnit.Pixel);
            lblUpcoming.ForeColor = Color.FromArgb(109, 0, 0);
            lblUpcoming.Location = new Point(12, 12);
            lblUpcoming.Size = new Size(300, 26);
            pnlUpcoming.Controls.Add(lblUpcoming);

            string[] eventDates = { "Mar\n10", "Mar\n15", "Mar\n20" };
            string[] eventTitles = { "Enrollment Period Ends", "Classes Begin", "Add/Drop Period Ends" };
            string[] eventTimes = { "11:59 PM", "8:00 AM", "5:00 PM" };

            for (int e = 0; e < 3; e++)
            {
                Panel ev = new Panel();
                ev.Location = new Point(12, 56 + e * 96);
                ev.Size = new Size(400, 80);

                Label dateBox = new Label();
                dateBox.Size = new Size(64, 64);
                dateBox.Location = new Point(0, 8);
                dateBox.BackColor = Color.FromArgb(109, 0, 0);
                dateBox.ForeColor = Color.White;
                dateBox.TextAlign = ContentAlignment.MiddleCenter;
                dateBox.Font = new Font("Arial", 10F, FontStyle.Bold, GraphicsUnit.Pixel);
                dateBox.Text = eventDates[e];

                Label etitle = new Label();
                etitle.Location = new Point(84, 8);
                etitle.Size = new Size(300, 20);
                etitle.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
                etitle.Text = eventTitles[e];

                Label etime = new Label();
                etime.Location = new Point(84, 34);
                etime.Size = new Size(300, 20);
                etime.Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Pixel);
                etime.ForeColor = Color.FromArgb(120, 120, 120);
                etime.Text = eventTimes[e];

                ev.Controls.Add(dateBox);
                ev.Controls.Add(etitle);
                ev.Controls.Add(etime);
                pnlUpcoming.Controls.Add(ev);
            }
            pnlMainLower.Controls.Add(pnlUpcoming);
            pnlDashboardContent.Controls.Add(pnlMainLower);
        }

        private void changeButtonColor(Button button)
        {
            // guard: don't proceed if no button provided
            if (button == null) return;

            if (clickedButton != null)
            {
                clickedButton.BackColor = defaultColor;
            }

            clickedButton = button;

            // ensure pnlYellow exists (lazy init) to avoid NullReferenceException
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
            if (clickedButton?.Parent != null)
                clickedButton.BackColor = selectedColor;
        }

        //Method na nagpapakita ng content ng bawat button, wala akong maisip na iba kaya eto
        private void showContent(Button button)
        {
            Dictionary<Button, Panel> contents = new Dictionary<Button, Panel> { };
            contents.Add(btnDashboard, pnlDashboardContent);
            contents.Add(btnGrades, pnlGradesContent);
            contents.Add(btnCourses, pnlCoursesContent);
            //Kada button na aadd, maglagay ng panel sa form at lagay dito
            foreach (KeyValuePair<Button, Panel> content in contents)
            {
                if (content.Key == clickedButton)
                {
                    //Automatic positioning, wag pakialaman maliban nalang kung binago ang position ng sidebar
                    content.Value.Parent = panel3;
                    content.Value.Location = new Point(pnlSidebar.Size.Width, pnlHeader.Size.Height);
                    content.Value.Visible = true;
                    content.Value.BringToFront();
                }
                else
                {
                    content.Value.Visible = false;
                }
            }
        }

        //Method para pag pinindot yung X sa taas o mag alt-F4, icclose lahat ng forms para di magerror pag ni run uli
        //Lagay to sa bawat form na iaadd, Step 1: Hanapin sa properties ng form yung event na FormClosing, Step 2: Double click para gumawa ng method, Step 3: Copy paste code na nasa loob nito
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
            pnllmsSubmenu.Visible = !pnllmsSubmenu.Visible;
            if (pnllmsSubmenu.Visible)
                btnLMS.Text = " LMS                                       ⌄";
            else
                btnLMS.Text = " LMS                                        ›";
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pnlCoursesContent_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lblWelcome_Click(object sender, EventArgs e)
        {

        }

        private void pnlBanner_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblWelcomeSubtitle_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowstats_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click_1(object sender, EventArgs e)
        {

        }

        private void panel17_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click_1(object sender, EventArgs e)
        {

        }

        private void label20_Click_2(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click_1(object sender, EventArgs e)
        {

        }
    }
}
