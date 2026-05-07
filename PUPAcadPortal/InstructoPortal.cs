using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class InstructorPortal : Form
    {
        private Button clickedButton;
        private Color defaultColor = Color.Maroon;
        private Color selectedColor = Color.FromArgb(109, 0, 0);

        // ── New: LMS Activities host ──────────────────────────────────────────
        private Panel pnlLMSActivitiesHost;
        private LMSActivityHost lmsHost;

        public InstructorPortal()
        {
            InitializeComponent();
            pnlAttendance.AutoScrollMinSize = new Size(0, 1000);
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/dd/yyyy hh:mm tt";
            BuildLMSActivitiesPanel();
        }

        // ── Build the LMS Activities panel ────────────────────────────────────
        private void BuildLMSActivitiesPanel()
        {
            pnlLMSActivitiesHost = new Panel
            {
                Name = "pnlLMSActivitiesHost",
                Dock = DockStyle.Fill,
                Visible = false,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            lmsHost = new LMSActivityHost { Dock = DockStyle.Fill };
            pnlLMSActivitiesHost.Controls.Add(lmsHost);

            panel3.Controls.Add(pnlLMSActivitiesHost);
        }

        // ── Sidebar helpers ───────────────────────────────────────────────────
        private void changeButtonColor(Button button)
        {
            if (clickedButton != null)
                clickedButton.BackColor = defaultColor;

            clickedButton = button;
            pnlYellow.Visible = true;
            pnlYellow.Parent = clickedButton.Parent;
            pnlYellow.BringToFront();
            clickedButton.BackColor = selectedColor;
        }

        private void showContent(Button button)
        {
            var contents = new Dictionary<Button, Panel>
            {
                { btnDashboard, pnlDashboardContent },
                { btnGrades,    pnlGradesContent    },
                { btnCourses,   pnlCoursesContent   },
            };

            foreach (var kv in contents)
            {
                if (kv.Key == clickedButton)
                {
                    kv.Value.Location = new Point(pnlSidebar.Width, pnlHeader.Height);
                    kv.Value.Visible = true;
                }
                else
                {
                    kv.Value.Visible = false;
                }
            }

            if (button != btnActivitiesIns)
                pnlLMSActivitiesHost.Visible = false;
        }

        // ── Form close ────────────────────────────────────────────────────────
        private void StudentPortal_Closing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Are you sure you want to exit?", "Confirm Exit",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    e.Cancel = true;
                else
                    Application.Exit();
            }
        }

        // ── Main sidebar buttons ──────────────────────────────────────────────
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
            btnLMS.Text = pnllmsSubmenu.Visible
                ? " LMS                                       ⌄"
                : " LMS                                        ›";
        }

        private void btnLogout_Click(object sender, EventArgs e) { this.Close(); }

        // ── LMS sub-menu buttons ──────────────────────────────────────────────
        private void btnAnnounceIns_Click(object sender, EventArgs e)
        {
            pnlAnnounce.BringToFront();
            pnlAnnounce.Visible = true;
        }

        private void btnCalendarIns_Click(object sender, EventArgs e)
        {
            pnlCalendar.BringToFront();
            pnlCalendar.Visible = true;
        }

        private void btnSubjectIns_Click(object sender, EventArgs e)
        {
            pnlSubject.BringToFront();
            pnlSubject.Visible = true;
        }

        /// <summary>
        /// Shows the full LMS Activity Management section.
        /// Chain: ActivityDashboard -> AssignmentManagement -> SubmissionList -> GradingInterface
        /// All navigation is handled internally by LMSActivityHost.
        /// </summary>
        private void btnActivitiesIns_Click(object sender, EventArgs e)
        {
            // Hide all other content panels
            pnlDashboardContent.Visible = false;
            pnlGradesContent.Visible = false;
            pnlCoursesContent.Visible = false;
            pnlLMSAct.Visible = false;
            pnlAnnounce.Visible = false;
            pnlCalendar.Visible = false;
            pnlSubject.Visible = false;
            pnlAttendance.Visible = false;
            pnlGrades.Visible = false;

            // Reset to dashboard so instructor always lands on the course list
            lmsHost.ShowDashboard();

            pnlLMSActivitiesHost.Dock = DockStyle.Fill;
            pnlLMSActivitiesHost.Visible = true;
            pnlLMSActivitiesHost.BringToFront();
        }

        private void btnAttendanceIns_Click(object sender, EventArgs e)
        {
            pnlAttendance.BringToFront();
            pnlAttendance.Visible = true;
        }

        private void btnGradeIns_Click(object sender, EventArgs e)
        {
            pnlGrades.BringToFront();
            pnlGrades.Visible = true;
        }

        // ── Announcement panel ────────────────────────────────────────────────
        private void CreateAnnounce_Click(object sender, EventArgs e)
        {
            pnlCreateAnnounce.Visible = !pnlCreateAnnounce.Visible;
            if (pnlCreateAnnounce.Visible)
            {
                pnlCreateAnnounce.BringToFront();
                pnlCreateAnnounce.Location = new Point(
                    (Width - pnlCreateAnnounce.Width) / 4,
                    (Height - pnlCreateAnnounce.Height) / 4);
            }
        }

        private void CreateAnnounce_Click_1(object sender, EventArgs e)
        {
            pnlCreateAnnounce.Visible = true;
            pnlCreateAnnounce.BringToFront();
        }

        private void btnCancelPost_Click(object sender, EventArgs e)
        {
            pnlCreateAnnounce.Visible = false;
            pnlAnnounce.BringToFront();
        }

        // ── LMS sub-panel navigation (existing) ───────────────────────────────
        private void btnGo1_Click(object sender, EventArgs e)
        {
            pnlSubMenu.Visible = true;
            pnlSubMenu.BringToFront();
            pnlLMSActivities.Visible = true;
            pnlLMSActivities.BringToFront();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            pnlSubject.Visible = true;
            pnlSubject.BringToFront();
        }

        private void btnGeneralAnnounce_Click(object sender, EventArgs e)
        {
            pnlGenChats.Visible = true;
            pnlGenChats.BringToFront();
        }

        private void btnLMSActSub_Click(object sender, EventArgs e)
        {
            pnlLMSActivities.Visible = true;
            pnlLMSActivities.BringToFront();
        }

        private void btnLMSFiles_Click(object sender, EventArgs e)
        {
            pnlLMSFiles.Visible = true;
            pnlLMSFiles.BringToFront();
        }

        private void btnCreateAct_Click(object sender, EventArgs e)
        {
            pnlCreateAct.Visible = true;
            pnlCreateAct.BringToFront();
        }

        private void cmbBXActType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sel = cmbBXActType.SelectedItem?.ToString();
            pnlQuiz1.Visible = false;
            pnlAssign.Visible = false;
            switch (sel)
            {
                case "Quiz":
                    pnlQuiz1.Visible = true; pnlQuiz1.BringToFront(); break;
                case "Assignment":
                    pnlAssign.Visible = true; pnlAssign.BringToFront(); break;
            }
        }

        private void btnAssignAttach_Click(object sender, EventArgs e)
        {
            pnlAttachAss.Visible = true;
            pnlAttachAss.BringToFront();
        }

        private void btnAttachCancel_Click(object sender, EventArgs e) { pnlAttachAss.Visible = false; }
        private void btnAttachDone_Click(object sender, EventArgs e) { pnlAttachAss.Visible = false; }
        private void btnAttachCancel_Click_1(object sender, EventArgs e) { pnlAttachAss.Visible = false; }
        private void btnDoneAttach_Click(object sender, EventArgs e) { pnlAttachAss.Visible = false; }

        // ── Quiz card builder ─────────────────────────────────────────────────
        private void btnAddPanel_Click(object sender, EventArgs e)
        {
            var newCard = new ucQuestionCard { Width = 1250, Height = 423 };
            flowLayoutPanel3.Controls.Add(newCard);

            int margin = Math.Max(10, (flowLayoutPanel3.Width - 1250 - 25) / 2);
            foreach (Control ctrl in flowLayoutPanel3.Controls)
            {
                ctrl.Width = 1250;
                ctrl.Margin = new Padding(margin, 10, 10, 10);
                ctrl.Left = 0;
            }

            RenumberQuestions();
            if (flowLayoutPanel3.Controls.Contains(pnlControlBar))
                flowLayoutPanel3.Controls.SetChildIndex(pnlControlBar, -1);
            flowLayoutPanel3.ScrollControlIntoView(pnlControlBar);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var last = flowLayoutPanel3.Controls.OfType<ucQuestionCard>().LastOrDefault();
            if (last != null)
            {
                flowLayoutPanel3.Controls.Remove(last);
                last.Dispose();
                RenumberQuestions();
            }
        }

        private void btnSaveQuiz_Click(object sender, EventArgs e)
        {
            var r = MessageBox.Show("Do you want to save this quiz before exiting?",
                "Save Quiz", MessageBoxButtons.YesNoCancel);
            if (r == DialogResult.Yes || r == DialogResult.No)
                this.Close();
        }

        private void flowLayoutPanel3_Resize(object sender, EventArgs e)
        {
            int m = Math.Max(10, (flowLayoutPanel3.Width - 1250 - 25) / 2);
            foreach (Control ctrl in flowLayoutPanel3.Controls)
                ctrl.Margin = new Padding(m, 10, 10, 10);
        }

        private void RenumberQuestions()
        {
            int n = 1;
            foreach (Control ctrl in flowLayoutPanel3.Controls)
                if (ctrl is ucQuestionCard c) c.lblQuestionNumber.Text = "Question " + n++;
        }

        // ── Student / grade panel handlers ────────────────────────────────────
        private void buttonRounded1_Click(object sender, EventArgs e) { pnlSub1.Visible = true; pnlSub1.BringToFront(); }
        private void buttonRounded2_Click(object sender, EventArgs e) { pnlStudents1.Visible = true; pnlStudents1.BringToFront(); }
        private void buttonRounded7_Click(object sender, EventArgs e) { pnlSub1.Visible = false; }
        private void buttonRounded3_Click(object sender, EventArgs e) { pnlStudents1.Visible = false; }
        private void button9_Click(object sender, EventArgs e) { }
        private void buttonRounded15_Click(object sender, EventArgs e) { pnlStudents1.BringToFront(); pnlGrade.Visible = false; }
        private void button9_Click_1(object sender, EventArgs e) { pnlGrade.Visible = true; pnlGrade.BringToFront(); }
        private void buttonRounded16_Click(object sender, EventArgs e)
        {
            lblScore.Text = rtxtScore.Text;
            MessageBox.Show("Score saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAssign_Click(object sender, EventArgs e) { pnlLMSAct.Visible = true; pnlLMSAct.BringToFront(); }
        private void btnClassFiles_Click(object sender, EventArgs e) { pnlClassFiles.Visible = true; pnlClassFiles.BringToFront(); }
        private void MenuButton_Click(object sender, EventArgs e) { sideBarTimer.Start(); }

        bool sidebarExpand;
        bool expand = false;
        private void timer1_Tick(object sender, EventArgs e) { }
        private void StatusBtn_Click(object sender, EventArgs e) { timer1.Start(); }
        private void StatusBtn2_Click(object sender, EventArgs e) { timer1.Start(); }
        private void Sub1_Paint(object sender, PaintEventArgs e) { }
        private void pnlCoursesContent_Paint(object sender, PaintEventArgs e) { }
        private void pnlAttendance_Paint(object sender, PaintEventArgs e) { }

        // ── Stub handlers ─────────────────────────────────────────────────────
        private void flpActivities_Paint(object sender, PaintEventArgs e) { }
        private void lblCardTitle_Click(object sender, EventArgs e) { }
        private void lblCardDesc_Click(object sender, EventArgs e) { }
        private void picCardImage_Click(object sender, EventArgs e) { }
        private void lblCardTag_Click(object sender, EventArgs e) { }
        private void pnlItem1_Paint(object sender, PaintEventArgs e) { }
        private void picHeaderIcon_Click(object sender, EventArgs e) { }
        private void pnlListContainer_Paint(object sender, PaintEventArgs e) { }
        private void label114_Click(object sender, EventArgs e) { }
        private void button40_Click(object sender, EventArgs e) { }
        private void label113_Click(object sender, EventArgs e) { }
        private void button27_Click(object sender, EventArgs e) { }
        private void flowLayoutPanel8_Paint(object sender, PaintEventArgs e) { }
        private void roundedPanel33_Paint(object sender, PaintEventArgs e) { }
        private void label123_Click(object sender, EventArgs e) { }
        private void lblScore_Click(object sender, EventArgs e) { }
        private void label121_Click(object sender, EventArgs e) { }
        private void rtxtScore_TextChanged(object sender, EventArgs e) { }
        private void richTextBox4_TextChanged(object sender, EventArgs e) { }
        private void txtEssayContent_TextChanged(object sender, EventArgs e) { }
        private void label120_Click(object sender, EventArgs e) { }
        private void label126_Click(object sender, EventArgs e) { }
        private void lblHeaderTitle_Click(object sender, EventArgs e) { }
        private void label24_Click(object sender, EventArgs e) { }
    }
}