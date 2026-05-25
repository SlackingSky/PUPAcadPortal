using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static PUPAcadPortal.PortalForms.InstructorPortal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using PUPAcadPortal.PortalContents.Instructor.LMS;
using PUPAcadPortal.Utils;


namespace PUPAcadPortal.PortalForms

{
    public partial class InstructorPortal : Form
    {
        private SubmenuAnim submenuAnimLMS;

        private bool isEditing = false; // New flag to stop event interference
        public static int _year, _month;
        public static Dictionary<DateTime, string> notesDict = new Dictionary<DateTime, string>();
        private ActivityItem? currentEditingItem = null;
        private Button clickedButton;
        private Color defaultColor = Color.Maroon;
        private Color selectedColor = Color.FromArgb(109, 0, 0);

        private Panel pnlLMSActivitiesHost;
        private LMSActivityHost lmsHost;


        private SizeF _designSize;
        private readonly Dictionary<Control, RectangleF> _origBounds = new();
        private readonly Dictionary<Control, float> _origFontSz = new();


        public InstructorPortal()
        {
            InitializeComponent();

            submenuAnimLMS = new SubmenuAnim(fpnlLMSSubmenu, fpnlLMSSubmenu.Height);

            pnlAttendance.AutoScrollMinSize = new Size(0, 1000);
            SharedCalendarData.LoadData();
            pnlAttendance.AutoScrollMinSize = new Size(0, 1000);
            this.DoubleBuffered = true;

            // ✅ FIX: Wire the resize event ONLY ONCE here.
            // If you put this inside a Click event, it stacks and causes the "creeping" movement.
            //Temporary Data (Grade Feature)
            string[] row1 = { "2024-00074-SM-0", "Ablong, Adrian P." };
            string[] row2 = { "2024-00194-SM-0", "Alcaiz, Jared B." };
            string[] row3 = { "2024-00146-SM-0", "Amar, Charles Manuel C." };
            string[] row4 = { "2024-00123-SM-0", "Amen, Jessie C." };
            string[] row5 = { "2024-00274-SM-0", "Amolata, Jhayphee V." };

            dataGridView1.Rows.Add(row1);
            dataGridView1.Rows.Add(row2);
            dataGridView1.Rows.Add(row3);
            dataGridView1.Rows.Add(row4);
            dataGridView1.Rows.Add(row5);

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
            BuildLMSActivitiesPanel();
        }

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

            mainContentPanel.Controls.Add(pnlLMSActivitiesHost);
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
            if (dataGridView2 != null)
            {
                dataGridView2.AllowUserToAddRows = false;
                dataGridView2.AllowUserToOrderColumns = false;
                dataGridView2.AllowUserToResizeColumns = false;
                dataGridView2.AllowUserToResizeRows = false;

                // --- FIX 1.5: Anchor to stretch downward, and AutoSize to fill the gray space ---
                dataGridView2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

            if (cmbSelectCourse != null && cmbSelectCourse.Items.Count == 0)
            {
                cmbSelectCourse.Items.Add("IT 101 - Introduction to Computing");
                cmbSelectCourse.Items.Add("CS 102 - Data Structures");
                cmbSelectCourse.Items.Add("IS 103 - Database Management");
            }

            if (dataGridView2 != null && dataGridView2.Rows.Count == 0)
            {
                dataGridView2.Rows.Add("2021-00001-SM-0", "Eisen Nodesca", "85", "88");
                dataGridView2.Rows.Add("2021-00002-SM-0", "Clarisa Matias", "92", "95");
                dataGridView2.Rows.Add("2021-00003-SM-0", "Trisha Walang Last Name", "78", "82");
                dataGridView2.Rows.Add("2021-00004-SM-0", "Liza Soberano", "88", "90");
                dataGridView2.Rows.Add("2021-00005-SM-0", "Kween Yasmin", "72", "75");
                dataGridView2.Rows.Add("2021-00006-SM-0", "Maine Love Alden", "98", "89");
            }

            // 2. AUTO-CALCULATE GRADES LOGIC
            if (dataGridView2 != null)
            {
                dataGridView2.CellValueChanged += (s, e) =>
                {
                    if (e.RowIndex >= 0 && (e.ColumnIndex == 2 || e.ColumnIndex == 3))
                    {
                        IList cells = dataGridView2.Rows[e.RowIndex].Cells;
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

                    if (dataGridView2 != null)
                    {
                        dataGridView2.CurrentCell = null;

                        foreach (DataGridViewRow r in dataGridView2.Rows)
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

            clickedButton.BackColor = selectedColor;
            pnlYellow.Visible = true;
            pnlYellow.Parent = button;
            pnlYellow.Height = button.Height;
            pnlYellow.BringToFront();

        }

        //Method na nagpapakita ng content ng bawat button, wala akong maisip na iba kaya eto
        private void showContent(Button button)
        {
            if (button == null) return;

            var contents = new Dictionary<Button, Panel>()
            {
                { btnDashboard, pnlDashboardContent },
                { btnGrades, pnlGradesContent },
                //{ btnAnnounceIns, pnlAnnouncement  },
                //{ btnCalendarIns, pnlCalendar },
                //{ btnSubjectIns, pnlSubject },
                //{ btnActivitiesIns, pnlLMSAct },
                { btnAttendanceIns, pnlAttendance },
                { btnGradeIns, pnlGrades }
            };

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

        //Method para pag pinindot yung X sa taas o mag alt-F4, icclose lahat ng forms para di magerror pag ni run uli
        //Lagay to sa bawat form na iaadd, Step 1: Hanapin sa properties ng form yung event na FormClosing, Step 2: Double click para gumawa ng method, Step 3: Copy paste code na nasa loob nito
        private void InstructorPortal_Closing(object sender, FormClosingEventArgs e)
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
            lmsHost.ShowDashboard();
            pnlLMSActivitiesHost.Dock = DockStyle.Fill;
            pnlLMSActivitiesHost.Visible = true;
            pnlLMSActivitiesHost.BringToFront();
        }
        private async void btnLMS_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
            btnLMS.Text = !fpnlLMSSubmenu.Visible ? " LMS                                       ⌄" : " LMS                                        ›";
            await submenuAnimLMS.ToggleSubMenuAsync();
            btnAnnounceIns.PerformClick();
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void pnlCoursesContent_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnAnnounceIns_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            mainContentPanel.BringToFront();
            mainContentPanel.ShowView(new AnnouncementContentInst());
        }

        private void btnCalendarIns_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            mainContentPanel.ShowView(new CalendarContentInst());
        }

        private void btnCoursesIns_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            mainContentPanel.ShowView(new ActivityDashboard());
        }

        private void btnAttendanceIns_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void btnGradeIns_Click(object sender, EventArgs e)
        {
            changeButtonColor(sender as Button);
            showContent(clickedButton);
        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        bool expand = false;
        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void pnlAttendance_Paint(object sender, PaintEventArgs e)
        {

        }

        private void InstructorPortal_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new Size(1024, 700);

            _designSize = new SizeF(this.ClientSize.Width, this.ClientSize.Height);
            SnapshotControls(this.Controls);

            this.FormClosed += (s, ev) =>
            {
                SharedCalendarData.SaveData();
            };
        }

        private void SnapshotControls(Control.ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl.Tag is string t && t.Contains("noScale")) continue;
                _origBounds[ctrl] = new RectangleF(ctrl.Left, ctrl.Top, ctrl.Width, ctrl.Height);
                _origFontSz[ctrl] = ctrl.Font.Size;
                if (ctrl.HasChildren) SnapshotControls(ctrl.Controls);
            }
        }

        private void ScaleControls(Control.ControlCollection controls, float rx, float ry)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl.Tag is string t && t.Contains("noScale")) continue;
                if (!_origBounds.TryGetValue(ctrl, out RectangleF ob)) continue;

                int newX = ScaleRound(ob.X * rx);
                int newY = ScaleRound(ob.Y * ry);
                int newW = Math.Max(1, ScaleRound(ob.Width * rx));
                int newH = Math.Max(1, ScaleRound(ob.Height * ry));

                switch (ctrl.Dock)
                {
                    case DockStyle.Fill: break;
                    case DockStyle.Top: ctrl.Height = newH; break;
                    case DockStyle.Bottom: ctrl.Height = newH; break;
                    case DockStyle.Left: ctrl.Width = newW; break;
                    case DockStyle.Right: ctrl.Width = newW; break;
                    default: ctrl.SetBounds(newX, newY, newW, newH); break;
                }

                if (_origFontSz.TryGetValue(ctrl, out float origSz))
                {
                    float newSz = Math.Max(6f, origSz * Math.Min(rx, ry));
                    if (Math.Abs(ctrl.Font.Size - newSz) > 0.15f)
                    {
                        try
                        {
                            ctrl.Font = new Font(
                                ctrl.Font.FontFamily, newSz,
                                ctrl.Font.Style, GraphicsUnit.Point);
                        }
                        catch { /* font too small – silently skip */ }
                    }
                }

                if (ctrl.HasChildren) ScaleControls(ctrl.Controls, rx, ry);
            }
        }

        private static int ScaleRound(float v) => (int)Math.Round(v);

        private void InstructorPortal_Resize(object sender, EventArgs e)
        {
            if (_designSize.Width == 0 || _designSize.Height == 0) return;

            float rx = Math.Max(this.ClientSize.Width, 1024) / _designSize.Width;
            float ry = Math.Max(this.ClientSize.Height, 700) / _designSize.Height;

            this.SuspendLayout();
            ScaleControls(this.Controls, rx, ry);
            this.ResumeLayout(true);
        }

        private void panel18_Paint(object sender, PaintEventArgs e)
        {

        }

        // --- QUICK ACTIONS REDIRECTS ---
        private void panel105_Click(object sender, EventArgs e) { if (btnGrades != null) btnGrades_Click(btnGrades, e); }
        private void panel103_Click(object sender, EventArgs e) { if (fpnlLMSSubmenu.Visible == false) { btnLMS.PerformClick(); } } //btnSubjectIns.PerformClick(); }
        private void panel102_Click(object sender, EventArgs e) { if (fpnlLMSSubmenu.Visible == false) { btnLMS.PerformClick(); } btnAttendanceIns.PerformClick(); }
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

        private void label303_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmbCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlGradeRecords.Visible = false;

            switch (cmbCourse.SelectedIndex)
            {
                case 0:
                    pnlGradeRecords.Visible = true;
                    break;
            }
        }
       
        private static GraphicsPath RoundedRectPath(Rectangle r, int rad)
        {
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, rad, rad, 180, 90);
            p.AddArc(r.Right - rad, r.Y, rad, rad, 270, 90);
            p.AddArc(r.Right - rad, r.Bottom - rad, rad, rad, 0, 90);
            p.AddArc(r.X, r.Bottom - rad, rad, rad, 90, 90);
            p.CloseFigure();
            return p;
        }

        private static void MakeRoundedRegion(Label lbl, int radius)
        {
            using var path = RoundedRectPath(new Rectangle(0, 0, lbl.Width, lbl.Height), radius);
            lbl.Region = new Region(path);
        }

        private static void MakeCircleRegion(Panel p)
        {
            var path = new GraphicsPath();
            path.AddEllipse(0, 0, p.Width, p.Height);
            p.Region = new Region(path);
        }
    }
}
