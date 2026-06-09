namespace PUPAcadPortal
{
    partial class GradingInterface
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _autoSaveTimer?.Stop();
                _autoSaveTimer?.Dispose();
                if (components != null) components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            pnlHeader = new System.Windows.Forms.Panel();
            btnBack = new buttonRounded();
            lblActivityTitle = new System.Windows.Forms.Label();
            lblStudentName = new System.Windows.Forms.Label();
            lblStudentId = new System.Windows.Forms.Label();
            lblSubmissionTime = new System.Windows.Forms.Label();
            lblMaxPoints = new System.Windows.Forms.Label();
            lblNavCounter = new System.Windows.Forms.Label();
            btnPrevStudent = new buttonRounded();
            btnNextStudent = new buttonRounded();

            pnlMain = new System.Windows.Forms.Panel();
            pnlEssay = new System.Windows.Forms.Panel();
            pnlGrading = new System.Windows.Forms.Panel();

            flpRubricRows = new System.Windows.Forms.FlowLayoutPanel();
            lblRubricTotal = new System.Windows.Forms.Label();
            chkAutoScore = new System.Windows.Forms.CheckBox();
            lblSaveStatus = new System.Windows.Forms.Label();
            txtScore = new System.Windows.Forms.TextBox();
            lblScoreOf = new System.Windows.Forms.Label();
            txtRemarks = new System.Windows.Forms.TextBox();
            txtEssayContent = new System.Windows.Forms.TextBox();
            lblWordCount = new System.Windows.Forms.Label();
            lblCharCount = new System.Windows.Forms.Label();
            // (these panels are still used by designer but their
            //  controls are populated dynamically in code-behind)
            pnlRubricBox = new System.Windows.Forms.Panel();
            pnlScoreInput = new System.Windows.Forms.Panel();
            pnlRemarks = new System.Windows.Forms.Panel();
            pnlWordCount = new System.Windows.Forms.Panel();
            // ─────────────────────────────────────────────────

            pnlHeader.SuspendLayout();
            pnlMain.SuspendLayout();
            SuspendLayout();

            // ── pnlHeader ─────────────────────────────────────
            pnlHeader.BackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            pnlHeader.Controls.Add(btnBack);
            pnlHeader.Controls.Add(lblActivityTitle);
            pnlHeader.Controls.Add(lblStudentName);
            pnlHeader.Controls.Add(lblStudentId);
            pnlHeader.Controls.Add(lblSubmissionTime);
            pnlHeader.Controls.Add(lblMaxPoints);
            pnlHeader.Controls.Add(lblNavCounter);
            pnlHeader.Controls.Add(btnPrevStudent);
            pnlHeader.Controls.Add(btnNextStudent);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(1680, 90);
            pnlHeader.TabIndex = 1;

            // btnBack
            btnBack.BackColor = System.Drawing.Color.FromArgb(100, 0, 0);
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnBack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnBack.ForeColor = System.Drawing.Color.White;
            btnBack.Location = new System.Drawing.Point(12, 28);
            btnBack.Name = "btnBack";
            btnBack.Size = new System.Drawing.Size(80, 32);
            btnBack.TabIndex = 0;
            btnBack.Text = "← Back";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += btnBack_Click;

            // lblActivityTitle
            lblActivityTitle.AutoEllipsis = true;
            lblActivityTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            lblActivityTitle.ForeColor = System.Drawing.Color.White;
            lblActivityTitle.Location = new System.Drawing.Point(106, 8);
            lblActivityTitle.Name = "lblActivityTitle";
            lblActivityTitle.Size = new System.Drawing.Size(740, 26);
            lblActivityTitle.TabIndex = 1;

            // lblStudentName
            lblStudentName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            lblStudentName.ForeColor = System.Drawing.Color.White;
            lblStudentName.Location = new System.Drawing.Point(106, 36);
            lblStudentName.Name = "lblStudentName";
            lblStudentName.Size = new System.Drawing.Size(380, 22);
            lblStudentName.TabIndex = 2;

            // lblStudentId
            lblStudentId.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblStudentId.ForeColor = System.Drawing.Color.FromArgb(220, 185, 185);
            lblStudentId.Location = new System.Drawing.Point(106, 60);
            lblStudentId.Name = "lblStudentId";
            lblStudentId.Size = new System.Drawing.Size(200, 18);
            lblStudentId.TabIndex = 3;

            // lblSubmissionTime
            lblSubmissionTime.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblSubmissionTime.ForeColor = System.Drawing.Color.FromArgb(220, 185, 185);
            lblSubmissionTime.Location = new System.Drawing.Point(316, 60);
            lblSubmissionTime.Name = "lblSubmissionTime";
            lblSubmissionTime.Size = new System.Drawing.Size(310, 18);
            lblSubmissionTime.TabIndex = 4;

            // lblMaxPoints
            lblMaxPoints.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblMaxPoints.ForeColor = System.Drawing.Color.FromArgb(255, 196, 0);
            lblMaxPoints.Location = new System.Drawing.Point(636, 60);
            lblMaxPoints.Name = "lblMaxPoints";
            lblMaxPoints.Size = new System.Drawing.Size(160, 18);
            lblMaxPoints.TabIndex = 5;

            // lblNavCounter
            lblNavCounter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblNavCounter.ForeColor = System.Drawing.Color.FromArgb(230, 185, 185);
            lblNavCounter.Location = new System.Drawing.Point(1380, 60);
            lblNavCounter.Name = "lblNavCounter";
            lblNavCounter.Size = new System.Drawing.Size(80, 18);
            lblNavCounter.TabIndex = 8;
            lblNavCounter.Text = "1 / 1";
            lblNavCounter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // btnPrevStudent
            btnPrevStudent.BackColor = System.Drawing.Color.FromArgb(90, 0, 0);
            btnPrevStudent.FlatAppearance.BorderSize = 0;
            btnPrevStudent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnPrevStudent.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnPrevStudent.ForeColor = System.Drawing.Color.White;
            btnPrevStudent.Location = new System.Drawing.Point(1464, 28);
            btnPrevStudent.Name = "btnPrevStudent";
            btnPrevStudent.Size = new System.Drawing.Size(96, 32);
            btnPrevStudent.TabIndex = 6;
            btnPrevStudent.Text = "◀ Prev";
            btnPrevStudent.UseVisualStyleBackColor = false;
            btnPrevStudent.Click += btnPrevStudent_Click;

            // btnNextStudent
            btnNextStudent.BackColor = System.Drawing.Color.FromArgb(90, 0, 0);
            btnNextStudent.FlatAppearance.BorderSize = 0;
            btnNextStudent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnNextStudent.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnNextStudent.ForeColor = System.Drawing.Color.White;
            btnNextStudent.Location = new System.Drawing.Point(1568, 28);
            btnNextStudent.Name = "btnNextStudent";
            btnNextStudent.Size = new System.Drawing.Size(96, 32);
            btnNextStudent.TabIndex = 7;
            btnNextStudent.Text = "Next ▶";
            btnNextStudent.UseVisualStyleBackColor = false;
            btnNextStudent.Click += btnNextStudent_Click;

            // ── pnlMain — fills remaining height ─────────────
            pnlMain.AutoScroll = false;
            pnlMain.BackColor = System.Drawing.Color.FromArgb(245, 245, 248);
            pnlMain.Controls.Add(pnlGrading);
            pnlMain.Controls.Add(pnlEssay);
            pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlMain.Location = new System.Drawing.Point(0, 90);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new System.Drawing.Size(1680, 899);
            pnlMain.TabIndex = 0;

            // ── pnlEssay — left 72%, shows submission content ─
            pnlEssay.BackColor = System.Drawing.Color.White;
            pnlEssay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pnlEssay.Location = new System.Drawing.Point(10, 10);
            pnlEssay.Name = "pnlEssay";
            pnlEssay.Size = new System.Drawing.Size(1200, 878);
            pnlEssay.Anchor = System.Windows.Forms.AnchorStyles.Top
                                 | System.Windows.Forms.AnchorStyles.Left
                                 | System.Windows.Forms.AnchorStyles.Bottom;
            pnlEssay.TabIndex = 0;

            // ── pnlGrading — right ~26%, grading controls ─────
            pnlGrading.BackColor = System.Drawing.Color.White;
            pnlGrading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pnlGrading.AutoScroll = true;
            pnlGrading.Location = new System.Drawing.Point(1218, 10);
            pnlGrading.Name = "pnlGrading";
            pnlGrading.Size = new System.Drawing.Size(448, 878);
            pnlGrading.Anchor = System.Windows.Forms.AnchorStyles.Top
                                   | System.Windows.Forms.AnchorStyles.Right
                                   | System.Windows.Forms.AnchorStyles.Bottom;
            pnlGrading.TabIndex = 1;

            // ── GradingInterface ──────────────────────────────
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(pnlMain);
            Controls.Add(pnlHeader);
            Name = "GradingInterface";
            Size = new System.Drawing.Size(1680, 989);

            pnlHeader.ResumeLayout(false);
            pnlMain.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private buttonRounded btnBack;
        private System.Windows.Forms.Label lblActivityTitle;
        private System.Windows.Forms.Label lblStudentName;
        private System.Windows.Forms.Label lblStudentId;
        private System.Windows.Forms.Label lblSubmissionTime;
        private System.Windows.Forms.Label lblMaxPoints;
        private System.Windows.Forms.Label lblNavCounter;
        private buttonRounded btnPrevStudent;
        private buttonRounded btnNextStudent;

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlEssay;
        private System.Windows.Forms.Panel pnlGrading;
        private System.Windows.Forms.FlowLayoutPanel flpRubricRows;
        private System.Windows.Forms.Label lblRubricTotal;
        private System.Windows.Forms.CheckBox chkAutoScore;
        private System.Windows.Forms.Label lblSaveStatus;
        private System.Windows.Forms.TextBox txtScore;
        private System.Windows.Forms.Label lblScoreOf;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.TextBox txtEssayContent;
        private System.Windows.Forms.Label lblWordCount;
        private System.Windows.Forms.Label lblCharCount;
        private System.Windows.Forms.Panel pnlRubricBox;
        private System.Windows.Forms.Panel pnlScoreInput;
        private System.Windows.Forms.Panel pnlRemarks;
        private System.Windows.Forms.Panel pnlWordCount;
        private System.Windows.Forms.Label lblGradingHeader;
        private System.Windows.Forms.Label lblEssayHeader;
        private System.Windows.Forms.Label lblRubricHeader;
        private System.Windows.Forms.Label lblScoreLabel;
        private buttonRounded btnSaveScore;
        private System.Windows.Forms.Label lblRemarksHeader;
    }
}