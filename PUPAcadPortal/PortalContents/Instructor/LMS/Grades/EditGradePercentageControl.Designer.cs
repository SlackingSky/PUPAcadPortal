namespace PUPAcadPortal.Dialogs
{
    partial class EditGradePercentageControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblHeaderSubtitle = new Label();
            lblHeaderTitle = new Label();
            lblCSSection = new Label();
            csPanel = new Panel();
            lblCSTotal = new Label();
            lblPct5 = new Label();
            nudLongTests = new NumericUpDown();
            lblLongTests = new Label();
            lblPct4 = new Label();
            nudAssignment = new NumericUpDown();
            lblAssignment = new Label();
            lblPct3 = new Label();
            nudSeatwork = new NumericUpDown();
            lblSeatwork = new Label();
            lblPct2 = new Label();
            nudRecitation = new NumericUpDown();
            lblRecitation = new Label();
            lblPct1 = new Label();
            nudAttendance = new NumericUpDown();
            lblAttendance = new Label();
            lblMESection = new Label();
            mePanel = new Panel();
            lblPct6 = new Label();
            nudMajorExam = new NumericUpDown();
            lblMajorExam = new Label();
            lblGrandTotal = new Label();
            btnReset = new Button();
            btnCancel = new Button();
            btnApply = new Button();
            pnlHeader.SuspendLayout();
            csPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudLongTests).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAssignment).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudSeatwork).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRecitation).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAttendance).BeginInit();
            mePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudMajorExam).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(106, 0, 0);
            pnlHeader.Controls.Add(lblHeaderSubtitle);
            pnlHeader.Controls.Add(lblHeaderTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(504, 70);
            pnlHeader.TabIndex = 0;
            // 
            // lblHeaderSubtitle
            // 
            lblHeaderSubtitle.AutoSize = true;
            lblHeaderSubtitle.Font = new Font("Segoe UI", 9F);
            lblHeaderSubtitle.ForeColor = Color.FromArgb(220, 220, 220);
            lblHeaderSubtitle.Location = new Point(16, 38);
            lblHeaderSubtitle.Name = "lblHeaderSubtitle";
            lblHeaderSubtitle.Size = new Size(307, 15);
            lblHeaderSubtitle.TabIndex = 1;
            lblHeaderSubtitle.Text = "Set weights for each grade component (must total 100%)";
            // 
            // lblHeaderTitle
            // 
            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblHeaderTitle.ForeColor = Color.White;
            lblHeaderTitle.Location = new Point(16, 10);
            lblHeaderTitle.Name = "lblHeaderTitle";
            lblHeaderTitle.Size = new Size(217, 25);
            lblHeaderTitle.TabIndex = 0;
            lblHeaderTitle.Text = "Edit Grade Percentages";
            // 
            // lblCSSection
            // 
            lblCSSection.AutoSize = true;
            lblCSSection.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblCSSection.ForeColor = Color.FromArgb(106, 0, 0);
            lblCSSection.Location = new Point(14, 85);
            lblCSSection.Name = "lblCSSection";
            lblCSSection.Size = new Size(260, 17);
            lblCSSection.TabIndex = 1;
            lblCSSection.Text = "Class Standing Components (target 70%)";
            // 
            // csPanel
            // 
            csPanel.BackColor = Color.FromArgb(253, 245, 245);
            csPanel.BorderStyle = BorderStyle.FixedSingle;
            csPanel.Controls.Add(lblCSTotal);
            csPanel.Controls.Add(lblPct5);
            csPanel.Controls.Add(nudLongTests);
            csPanel.Controls.Add(lblLongTests);
            csPanel.Controls.Add(lblPct4);
            csPanel.Controls.Add(nudAssignment);
            csPanel.Controls.Add(lblAssignment);
            csPanel.Controls.Add(lblPct3);
            csPanel.Controls.Add(nudSeatwork);
            csPanel.Controls.Add(lblSeatwork);
            csPanel.Controls.Add(lblPct2);
            csPanel.Controls.Add(nudRecitation);
            csPanel.Controls.Add(lblRecitation);
            csPanel.Controls.Add(lblPct1);
            csPanel.Controls.Add(nudAttendance);
            csPanel.Controls.Add(lblAttendance);
            csPanel.Location = new Point(10, 107);
            csPanel.Name = "csPanel";
            csPanel.Size = new Size(475, 215);
            csPanel.TabIndex = 2;
            // 
            // lblCSTotal
            // 
            lblCSTotal.AutoSize = true;
            lblCSTotal.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblCSTotal.ForeColor = Color.FromArgb(16, 124, 65);
            lblCSTotal.Location = new Point(14, 196);
            lblCSTotal.Name = "lblCSTotal";
            lblCSTotal.Size = new Size(176, 17);
            lblCSTotal.TabIndex = 15;
            lblCSTotal.Text = "Class Standing Total: 0 / 70";
            // 
            // lblPct5
            // 
            lblPct5.AutoSize = true;
            lblPct5.Font = new Font("Segoe UI", 9.5F);
            lblPct5.Location = new Point(385, 159);
            lblPct5.Name = "lblPct5";
            lblPct5.Size = new Size(19, 17);
            lblPct5.TabIndex = 14;
            lblPct5.Text = "%";
            // 
            // nudLongTests
            // 
            nudLongTests.Font = new Font("Segoe UI", 9.5F);
            nudLongTests.Location = new Point(320, 156);
            nudLongTests.Name = "nudLongTests";
            nudLongTests.Size = new Size(60, 24);
            nudLongTests.TabIndex = 13;
            // 
            // lblLongTests
            // 
            lblLongTests.Font = new Font("Segoe UI", 9.5F);
            lblLongTests.Location = new Point(14, 158);
            lblLongTests.Name = "lblLongTests";
            lblLongTests.Size = new Size(260, 22);
            lblLongTests.TabIndex = 12;
            lblLongTests.Text = "Long Tests";
            // 
            // lblPct4
            // 
            lblPct4.AutoSize = true;
            lblPct4.Font = new Font("Segoe UI", 9.5F);
            lblPct4.Location = new Point(385, 123);
            lblPct4.Name = "lblPct4";
            lblPct4.Size = new Size(19, 17);
            lblPct4.TabIndex = 11;
            lblPct4.Text = "%";
            // 
            // nudAssignment
            // 
            nudAssignment.Font = new Font("Segoe UI", 9.5F);
            nudAssignment.Location = new Point(320, 120);
            nudAssignment.Name = "nudAssignment";
            nudAssignment.Size = new Size(60, 24);
            nudAssignment.TabIndex = 10;
            // 
            // lblAssignment
            // 
            lblAssignment.Font = new Font("Segoe UI", 9.5F);
            lblAssignment.Location = new Point(14, 122);
            lblAssignment.Name = "lblAssignment";
            lblAssignment.Size = new Size(260, 22);
            lblAssignment.TabIndex = 9;
            lblAssignment.Text = "Assignment / Project";
            // 
            // lblPct3
            // 
            lblPct3.AutoSize = true;
            lblPct3.Font = new Font("Segoe UI", 9.5F);
            lblPct3.Location = new Point(385, 87);
            lblPct3.Name = "lblPct3";
            lblPct3.Size = new Size(19, 17);
            lblPct3.TabIndex = 8;
            lblPct3.Text = "%";
            // 
            // nudSeatwork
            // 
            nudSeatwork.Font = new Font("Segoe UI", 9.5F);
            nudSeatwork.Location = new Point(320, 84);
            nudSeatwork.Name = "nudSeatwork";
            nudSeatwork.Size = new Size(60, 24);
            nudSeatwork.TabIndex = 7;
            // 
            // lblSeatwork
            // 
            lblSeatwork.Font = new Font("Segoe UI", 9.5F);
            lblSeatwork.Location = new Point(14, 86);
            lblSeatwork.Name = "lblSeatwork";
            lblSeatwork.Size = new Size(260, 22);
            lblSeatwork.TabIndex = 6;
            lblSeatwork.Text = "Seatwork / Short Quiz";
            // 
            // lblPct2
            // 
            lblPct2.AutoSize = true;
            lblPct2.Font = new Font("Segoe UI", 9.5F);
            lblPct2.Location = new Point(385, 51);
            lblPct2.Name = "lblPct2";
            lblPct2.Size = new Size(19, 17);
            lblPct2.TabIndex = 5;
            lblPct2.Text = "%";
            // 
            // nudRecitation
            // 
            nudRecitation.Font = new Font("Segoe UI", 9.5F);
            nudRecitation.Location = new Point(320, 48);
            nudRecitation.Name = "nudRecitation";
            nudRecitation.Size = new Size(60, 24);
            nudRecitation.TabIndex = 4;
            // 
            // lblRecitation
            // 
            lblRecitation.Font = new Font("Segoe UI", 9.5F);
            lblRecitation.Location = new Point(14, 50);
            lblRecitation.Name = "lblRecitation";
            lblRecitation.Size = new Size(260, 22);
            lblRecitation.TabIndex = 3;
            lblRecitation.Text = "Recitation / Class Participation";
            // 
            // lblPct1
            // 
            lblPct1.AutoSize = true;
            lblPct1.Font = new Font("Segoe UI", 9.5F);
            lblPct1.Location = new Point(385, 15);
            lblPct1.Name = "lblPct1";
            lblPct1.Size = new Size(19, 17);
            lblPct1.TabIndex = 2;
            lblPct1.Text = "%";
            // 
            // nudAttendance
            // 
            nudAttendance.Font = new Font("Segoe UI", 9.5F);
            nudAttendance.Location = new Point(320, 12);
            nudAttendance.Name = "nudAttendance";
            nudAttendance.Size = new Size(60, 24);
            nudAttendance.TabIndex = 1;
            // 
            // lblAttendance
            // 
            lblAttendance.Font = new Font("Segoe UI", 9.5F);
            lblAttendance.Location = new Point(14, 14);
            lblAttendance.Name = "lblAttendance";
            lblAttendance.Size = new Size(260, 22);
            lblAttendance.TabIndex = 0;
            lblAttendance.Text = "Attendance";
            // 
            // lblMESection
            // 
            lblMESection.AutoSize = true;
            lblMESection.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblMESection.ForeColor = Color.FromArgb(106, 0, 0);
            lblMESection.Location = new Point(14, 332);
            lblMESection.Name = "lblMESection";
            lblMESection.Size = new Size(237, 17);
            lblMESection.TabIndex = 3;
            lblMESection.Text = "Major Exam Component (target 30%)";
            // 
            // mePanel
            // 
            mePanel.BackColor = Color.FromArgb(253, 245, 245);
            mePanel.BorderStyle = BorderStyle.FixedSingle;
            mePanel.Controls.Add(lblPct6);
            mePanel.Controls.Add(nudMajorExam);
            mePanel.Controls.Add(lblMajorExam);
            mePanel.Location = new Point(10, 354);
            mePanel.Name = "mePanel";
            mePanel.Size = new Size(475, 60);
            mePanel.TabIndex = 4;
            // 
            // lblPct6
            // 
            lblPct6.AutoSize = true;
            lblPct6.Font = new Font("Segoe UI", 9.5F);
            lblPct6.Location = new Point(385, 15);
            lblPct6.Name = "lblPct6";
            lblPct6.Size = new Size(19, 17);
            lblPct6.TabIndex = 17;
            lblPct6.Text = "%";
            // 
            // nudMajorExam
            // 
            nudMajorExam.Font = new Font("Segoe UI", 9.5F);
            nudMajorExam.Location = new Point(320, 12);
            nudMajorExam.Name = "nudMajorExam";
            nudMajorExam.Size = new Size(60, 24);
            nudMajorExam.TabIndex = 16;
            // 
            // lblMajorExam
            // 
            lblMajorExam.Font = new Font("Segoe UI", 9.5F);
            lblMajorExam.Location = new Point(14, 14);
            lblMajorExam.Name = "lblMajorExam";
            lblMajorExam.Size = new Size(260, 22);
            lblMajorExam.TabIndex = 15;
            lblMajorExam.Text = "Major Exams (Midterm / Final)";
            // 
            // lblGrandTotal
            // 
            lblGrandTotal.AutoSize = true;
            lblGrandTotal.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblGrandTotal.ForeColor = Color.FromArgb(16, 124, 65);
            lblGrandTotal.Location = new Point(16, 428);
            lblGrandTotal.Name = "lblGrandTotal";
            lblGrandTotal.Size = new Size(161, 19);
            lblGrandTotal.TabIndex = 5;
            lblGrandTotal.Text = "Grand Total: 100 / 100 ";
            // 
            // btnReset
            // 
            btnReset.BackColor = Color.White;
            btnReset.FlatAppearance.BorderColor = Color.LightGray;
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnReset.ForeColor = Color.Black;
            btnReset.Location = new Point(10, 466);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(130, 32);
            btnReset.TabIndex = 7;
            btnReset.Text = "Reset to Default";
            btnReset.UseVisualStyleBackColor = false;
            btnReset.Click += btnReset_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.White;
            btnCancel.FlatAppearance.BorderColor = Color.LightGray;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCancel.ForeColor = Color.Black;
            btnCancel.Location = new Point(151, 466);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 32);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnApply
            // 
            btnApply.BackColor = Color.FromArgb(106, 0, 0);
            btnApply.FlatAppearance.BorderColor = Color.LightGray;
            btnApply.FlatStyle = FlatStyle.Flat;
            btnApply.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnApply.ForeColor = Color.White;
            btnApply.Location = new Point(271, 466);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(200, 32);
            btnApply.TabIndex = 9;
            btnApply.Text = "Apply Changes";
            btnApply.UseVisualStyleBackColor = false;
            btnApply.Click += btnApply_Click;
            // 
            // EditGradePercentageControl
            // 
            BackColor = Color.White;
            ClientSize = new Size(504, 508);
            Controls.Add(btnApply);
            Controls.Add(btnCancel);
            Controls.Add(btnReset);
            Controls.Add(lblGrandTotal);
            Controls.Add(mePanel);
            Controls.Add(lblMESection);
            Controls.Add(csPanel);
            Controls.Add(lblCSSection);
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EditGradePercentageControl";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Edit Grade Percentage";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            csPanel.ResumeLayout(false);
            csPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudLongTests).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAssignment).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudSeatwork).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRecitation).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAttendance).EndInit();
            mePanel.ResumeLayout(false);
            mePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudMajorExam).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Label lblHeaderSubtitle;
        private System.Windows.Forms.Label lblCSSection;
        private System.Windows.Forms.Panel csPanel;
        private System.Windows.Forms.Label lblAttendance;
        private System.Windows.Forms.NumericUpDown nudAttendance;
        private System.Windows.Forms.Label lblPct1;
        private System.Windows.Forms.Label lblPct2;
        private System.Windows.Forms.NumericUpDown nudRecitation;
        private System.Windows.Forms.Label lblRecitation;
        private System.Windows.Forms.Label lblPct3;
        private System.Windows.Forms.NumericUpDown nudSeatwork;
        private System.Windows.Forms.Label lblSeatwork;
        private System.Windows.Forms.Label lblPct4;
        private System.Windows.Forms.NumericUpDown nudAssignment;
        private System.Windows.Forms.Label lblAssignment;
        private System.Windows.Forms.Label lblPct5;
        private System.Windows.Forms.NumericUpDown nudLongTests;
        private System.Windows.Forms.Label lblLongTests;
        private System.Windows.Forms.Label lblCSTotal;
        private System.Windows.Forms.Label lblMESection;
        private System.Windows.Forms.Panel mePanel;
        private System.Windows.Forms.Label lblPct6;
        private System.Windows.Forms.NumericUpDown nudMajorExam;
        private System.Windows.Forms.Label lblMajorExam;
        private System.Windows.Forms.Label lblGrandTotal;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
    }
}
