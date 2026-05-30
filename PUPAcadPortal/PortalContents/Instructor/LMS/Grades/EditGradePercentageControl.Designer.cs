namespace PUPAcadPortal.Dialogs
{
    partial class EditGradePercentageControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeaderSubtitle = new System.Windows.Forms.Label();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.lblCSSection = new System.Windows.Forms.Label();
            this.csPanel = new System.Windows.Forms.Panel();
            this.lblCSTotal = new System.Windows.Forms.Label();
            this.lblLongTests = new System.Windows.Forms.Label();
            this.nudLongTests = new System.Windows.Forms.NumericUpDown();
            this.lblPct5 = new System.Windows.Forms.Label();
            this.lblAssignment = new System.Windows.Forms.Label();
            this.nudAssignment = new System.Windows.Forms.NumericUpDown();
            this.lblPct4 = new System.Windows.Forms.Label();
            this.lblSeatwork = new System.Windows.Forms.Label();
            this.nudSeatwork = new System.Windows.Forms.NumericUpDown();
            this.lblPct3 = new System.Windows.Forms.Label();
            this.lblRecitation = new System.Windows.Forms.Label();
            this.nudRecitation = new System.Windows.Forms.NumericUpDown();
            this.lblPct2 = new System.Windows.Forms.Label();
            this.lblAttendance = new System.Windows.Forms.Label();
            this.nudAttendance = new System.Windows.Forms.NumericUpDown();
            this.lblPct1 = new System.Windows.Forms.Label();
            this.lblMESection = new System.Windows.Forms.Label();
            this.mePanel = new System.Windows.Forms.Panel();
            this.lblMajorExam = new System.Windows.Forms.Label();
            this.nudMajorExam = new System.Windows.Forms.NumericUpDown();
            this.lblPct6 = new System.Windows.Forms.Label();
            this.lblGrandTotal = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.csPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLongTests)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAssignment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSeatwork)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecitation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAttendance)).BeginInit();
            this.mePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMajorExam)).BeginInit();
            this.SuspendLayout();

            // ── pnlHeader ─────────────────────────────────────────────────────
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pnlHeader.Controls.Add(this.lblHeaderSubtitle);
            this.pnlHeader.Controls.Add(this.lblHeaderTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(504, 72);
            this.pnlHeader.TabIndex = 0;

            // ── lblHeaderSubtitle ─────────────────────────────────────────────
            this.lblHeaderSubtitle.AutoSize = true;
            this.lblHeaderSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.lblHeaderSubtitle.Location = new System.Drawing.Point(17, 42);
            this.lblHeaderSubtitle.Name = "lblHeaderSubtitle";
            this.lblHeaderSubtitle.Size = new System.Drawing.Size(277, 15);
            this.lblHeaderSubtitle.TabIndex = 1;
            this.lblHeaderSubtitle.Text = "Set component weights — must total exactly 100%";

            // ── lblHeaderTitle ────────────────────────────────────────────────
            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.White;
            this.lblHeaderTitle.Location = new System.Drawing.Point(16, 10);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(217, 25);
            this.lblHeaderTitle.TabIndex = 0;
            this.lblHeaderTitle.Text = "Edit Grade Percentages";

            // ── lblCSSection ──────────────────────────────────────────────────
            this.lblCSSection.AutoSize = true;
            this.lblCSSection.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCSSection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblCSSection.Location = new System.Drawing.Point(14, 86);
            this.lblCSSection.Name = "lblCSSection";
            this.lblCSSection.Size = new System.Drawing.Size(262, 17);
            this.lblCSSection.TabIndex = 1;
            this.lblCSSection.Text = "Class Standing Components  (target: 70%)";

            // ── csPanel ───────────────────────────────────────────────────────
            this.csPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.csPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.csPanel.Controls.Add(this.lblCSTotal);
            this.csPanel.Controls.Add(this.lblLongTests);
            this.csPanel.Controls.Add(this.nudLongTests);
            this.csPanel.Controls.Add(this.lblPct5);
            this.csPanel.Controls.Add(this.lblAssignment);
            this.csPanel.Controls.Add(this.nudAssignment);
            this.csPanel.Controls.Add(this.lblPct4);
            this.csPanel.Controls.Add(this.lblSeatwork);
            this.csPanel.Controls.Add(this.nudSeatwork);
            this.csPanel.Controls.Add(this.lblPct3);
            this.csPanel.Controls.Add(this.lblRecitation);
            this.csPanel.Controls.Add(this.nudRecitation);
            this.csPanel.Controls.Add(this.lblPct2);
            this.csPanel.Controls.Add(this.lblAttendance);
            this.csPanel.Controls.Add(this.nudAttendance);
            this.csPanel.Controls.Add(this.lblPct1);
            this.csPanel.Location = new System.Drawing.Point(10, 108);
            this.csPanel.Name = "csPanel";
            this.csPanel.Size = new System.Drawing.Size(476, 225);
            this.csPanel.TabIndex = 2;

            // ── Attendance Row (Y=12) ──
            this.lblAttendance.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAttendance.Location = new System.Drawing.Point(14, 15);
            this.lblAttendance.Name = "lblAttendance";
            this.lblAttendance.Size = new System.Drawing.Size(250, 22);
            this.lblAttendance.TabIndex = 0;
            this.lblAttendance.Text = "Attendance";

            this.nudAttendance.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudAttendance.Location = new System.Drawing.Point(320, 12);
            this.nudAttendance.Name = "nudAttendance";
            this.nudAttendance.Size = new System.Drawing.Size(62, 24);
            this.nudAttendance.TabIndex = 1;

            this.lblPct1.AutoSize = true;
            this.lblPct1.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPct1.Location = new System.Drawing.Point(388, 15);
            this.lblPct1.Name = "lblPct1";
            this.lblPct1.Size = new System.Drawing.Size(19, 17);
            this.lblPct1.TabIndex = 2;
            this.lblPct1.Text = "%";

            // ── Recitation Row (Y=52) ──
            this.lblRecitation.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecitation.Location = new System.Drawing.Point(14, 55);
            this.lblRecitation.Name = "lblRecitation";
            this.lblRecitation.Size = new System.Drawing.Size(250, 22);
            this.lblRecitation.TabIndex = 3;
            this.lblRecitation.Text = "Recitation / Class Participation";

            this.nudRecitation.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudRecitation.Location = new System.Drawing.Point(320, 52);
            this.nudRecitation.Name = "nudRecitation";
            this.nudRecitation.Size = new System.Drawing.Size(62, 24);
            this.nudRecitation.TabIndex = 4;

            this.lblPct2.AutoSize = true;
            this.lblPct2.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPct2.Location = new System.Drawing.Point(388, 55);
            this.lblPct2.Name = "lblPct2";
            this.lblPct2.Size = new System.Drawing.Size(19, 17);
            this.lblPct2.TabIndex = 5;
            this.lblPct2.Text = "%";

            // ── Seatwork Row (Y=92) ──
            this.lblSeatwork.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSeatwork.Location = new System.Drawing.Point(14, 95);
            this.lblSeatwork.Name = "lblSeatwork";
            this.lblSeatwork.Size = new System.Drawing.Size(250, 22);
            this.lblSeatwork.TabIndex = 6;
            this.lblSeatwork.Text = "Seatwork / Short Quiz";

            this.nudSeatwork.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudSeatwork.Location = new System.Drawing.Point(320, 92);
            this.nudSeatwork.Name = "nudSeatwork";
            this.nudSeatwork.Size = new System.Drawing.Size(62, 24);
            this.nudSeatwork.TabIndex = 7;

            this.lblPct3.AutoSize = true;
            this.lblPct3.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPct3.Location = new System.Drawing.Point(388, 95);
            this.lblPct3.Name = "lblPct3";
            this.lblPct3.Size = new System.Drawing.Size(19, 17);
            this.lblPct3.TabIndex = 8;
            this.lblPct3.Text = "%";

            // ── Assignment Row (Y=132) ──
            this.lblAssignment.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAssignment.Location = new System.Drawing.Point(14, 135);
            this.lblAssignment.Name = "lblAssignment";
            this.lblAssignment.Size = new System.Drawing.Size(250, 22);
            this.lblAssignment.TabIndex = 9;
            this.lblAssignment.Text = "Assignment / Project";

            this.nudAssignment.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudAssignment.Location = new System.Drawing.Point(320, 132);
            this.nudAssignment.Name = "nudAssignment";
            this.nudAssignment.Size = new System.Drawing.Size(62, 24);
            this.nudAssignment.TabIndex = 10;

            this.lblPct4.AutoSize = true;
            this.lblPct4.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPct4.Location = new System.Drawing.Point(388, 135);
            this.lblPct4.Name = "lblPct4";
            this.lblPct4.Size = new System.Drawing.Size(19, 17);
            this.lblPct4.TabIndex = 11;
            this.lblPct4.Text = "%";

            // ── Long Tests Row (Y=172) ──
            this.lblLongTests.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLongTests.Location = new System.Drawing.Point(14, 175);
            this.lblLongTests.Name = "lblLongTests";
            this.lblLongTests.Size = new System.Drawing.Size(250, 22);
            this.lblLongTests.TabIndex = 12;
            this.lblLongTests.Text = "Long Tests";

            this.nudLongTests.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudLongTests.Location = new System.Drawing.Point(320, 172);
            this.nudLongTests.Name = "nudLongTests";
            this.nudLongTests.Size = new System.Drawing.Size(62, 24);
            this.nudLongTests.TabIndex = 13;

            this.lblPct5.AutoSize = true;
            this.lblPct5.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPct5.Location = new System.Drawing.Point(388, 175);
            this.lblPct5.Name = "lblPct5";
            this.lblPct5.Size = new System.Drawing.Size(19, 17);
            this.lblPct5.TabIndex = 14;
            this.lblPct5.Text = "%";

            // ── lblCSTotal ──
            this.lblCSTotal.AutoSize = true;
            this.lblCSTotal.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCSTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(124)))), ((int)(((byte)(65)))));
            this.lblCSTotal.Location = new System.Drawing.Point(14, 203);
            this.lblCSTotal.Name = "lblCSTotal";
            this.lblCSTotal.Size = new System.Drawing.Size(175, 17);
            this.lblCSTotal.TabIndex = 15;
            this.lblCSTotal.Text = "Class Standing Total: 0 / 70";

            // ── lblMESection ──────────────────────────────────────────────────
            this.lblMESection.AutoSize = true;
            this.lblMESection.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMESection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblMESection.Location = new System.Drawing.Point(14, 344);
            this.lblMESection.Name = "lblMESection";
            this.lblMESection.Size = new System.Drawing.Size(256, 17);
            this.lblMESection.TabIndex = 3;
            this.lblMESection.Text = "Major Exam Component  (target: 30%)";

            // ── mePanel ───────────────────────────────────────────────────────
            this.mePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.mePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mePanel.Controls.Add(this.lblMajorExam);
            this.mePanel.Controls.Add(this.nudMajorExam);
            this.mePanel.Controls.Add(this.lblPct6);
            this.mePanel.Location = new System.Drawing.Point(10, 366);
            this.mePanel.Name = "mePanel";
            this.mePanel.Size = new System.Drawing.Size(476, 52);
            this.mePanel.TabIndex = 4;

            // ── Major Exam Row (Y=12) ──
            this.lblMajorExam.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMajorExam.Location = new System.Drawing.Point(14, 15);
            this.lblMajorExam.Name = "lblMajorExam";
            this.lblMajorExam.Size = new System.Drawing.Size(250, 22);
            this.lblMajorExam.TabIndex = 0;
            this.lblMajorExam.Text = "Major Exams  (Midterm / Final)";

            this.nudMajorExam.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMajorExam.Location = new System.Drawing.Point(320, 12);
            this.nudMajorExam.Name = "nudMajorExam";
            this.nudMajorExam.Size = new System.Drawing.Size(62, 24);
            this.nudMajorExam.TabIndex = 1;

            this.lblPct6.AutoSize = true;
            this.lblPct6.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPct6.Location = new System.Drawing.Point(388, 15);
            this.lblPct6.Name = "lblPct6";
            this.lblPct6.Size = new System.Drawing.Size(19, 17);
            this.lblPct6.TabIndex = 2;
            this.lblPct6.Text = "%";

            // ── lblGrandTotal ─────────────────────────────────────────────────
            this.lblGrandTotal.AutoSize = true;
            this.lblGrandTotal.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrandTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(124)))), ((int)(((byte)(65)))));
            this.lblGrandTotal.Location = new System.Drawing.Point(16, 432);
            this.lblGrandTotal.Name = "lblGrandTotal";
            this.lblGrandTotal.Size = new System.Drawing.Size(163, 19);
            this.lblGrandTotal.TabIndex = 5;
            this.lblGrandTotal.Text = "Grand Total: 100 / 100";

            // ── btnReset ──────────────────────────────────────────────────────
            this.btnReset.BackColor = System.Drawing.Color.White;
            this.btnReset.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnReset.FlatAppearance.BorderSize = 1;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnReset.Location = new System.Drawing.Point(10, 466);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(130, 32);
            this.btnReset.TabIndex = 7;
            this.btnReset.Text = "↺  Reset Default";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);

            // ── btnCancel ─────────────────────────────────────────────────────
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnCancel.FlatAppearance.BorderSize = 1;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnCancel.Location = new System.Drawing.Point(150, 466);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 32);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // ── btnApply ──────────────────────────────────────────────────────
            this.btnApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnApply.FlatAppearance.BorderSize = 0;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.ForeColor = System.Drawing.Color.White;
            this.btnApply.Location = new System.Drawing.Point(262, 466);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(224, 32);
            this.btnApply.TabIndex = 9;
            this.btnApply.Text = "✔  Apply Changes";
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);

            // ── EditGradePercentageControl ────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(504, 512);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.lblGrandTotal);
            this.Controls.Add(this.mePanel);
            this.Controls.Add(this.lblMESection);
            this.Controls.Add(this.csPanel);
            this.Controls.Add(this.lblCSSection);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditGradePercentageControl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Grade Percentages";

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.csPanel.ResumeLayout(false);
            this.csPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLongTests)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAssignment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSeatwork)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecitation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAttendance)).EndInit();
            this.mePanel.ResumeLayout(false);
            this.mePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMajorExam)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
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
        private System.Windows.Forms.Label lblRecitation;
        private System.Windows.Forms.NumericUpDown nudRecitation;
        private System.Windows.Forms.Label lblPct2;
        private System.Windows.Forms.Label lblSeatwork;
        private System.Windows.Forms.NumericUpDown nudSeatwork;
        private System.Windows.Forms.Label lblPct3;
        private System.Windows.Forms.Label lblAssignment;
        private System.Windows.Forms.NumericUpDown nudAssignment;
        private System.Windows.Forms.Label lblPct4;
        private System.Windows.Forms.Label lblLongTests;
        private System.Windows.Forms.NumericUpDown nudLongTests;
        private System.Windows.Forms.Label lblPct5;
        private System.Windows.Forms.Label lblCSTotal;

        private System.Windows.Forms.Label lblMESection;
        private System.Windows.Forms.Panel mePanel;
        private System.Windows.Forms.Label lblMajorExam;
        private System.Windows.Forms.NumericUpDown nudMajorExam;
        private System.Windows.Forms.Label lblPct6;

        private System.Windows.Forms.Label lblGrandTotal;

        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
    }
}