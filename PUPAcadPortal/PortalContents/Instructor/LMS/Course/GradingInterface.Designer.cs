namespace PUPAcadPortal
{
    partial class GradingInterface
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
            btnBack = new buttonRounded();
            lblActivityTitle = new Label();
            lblStudentName = new Label();
            lblStudentId = new Label();
            lblSubmissionTime = new Label();
            lblMaxPoints = new Label();
            lblNavCounter = new Label();
            btnPrevStudent = new buttonRounded();
            btnNextStudent = new buttonRounded();
            pnlMain = new Panel();
            pnlEssay = new Panel();
            lblEssayHeader = new Label();
            txtEssayContent = new TextBox();
            pnlWordCount = new Panel();
            lblWordCount = new Label();
            lblCharCount = new Label();
            pnlGrading = new Panel();
            lblGradingHeader = new Label();
            lblSaveStatus = new Label();
            pnlRubricBox = new Panel();
            lblRubricHeader = new Label();
            flpRubricRows = new FlowLayoutPanel();
            lblRubricTotal = new Label();
            chkAutoScore = new CheckBox();
            pnlScoreInput = new Panel();
            lblScoreLabel = new Label();
            txtScore = new TextBox();
            lblScoreOf = new Label();
            btnSaveScore = new buttonRounded();
            pnlRemarks = new Panel();
            lblRemarksHeader = new Label();
            txtRemarks = new TextBox();
            pnlHeader.SuspendLayout();
            pnlMain.SuspendLayout();
            pnlEssay.SuspendLayout();
            pnlWordCount.SuspendLayout();
            pnlGrading.SuspendLayout();
            pnlRubricBox.SuspendLayout();
            pnlScoreInput.SuspendLayout();
            pnlRemarks.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(128, 0, 0);
            pnlHeader.Controls.Add(btnBack);
            pnlHeader.Controls.Add(lblActivityTitle);
            pnlHeader.Controls.Add(lblStudentName);
            pnlHeader.Controls.Add(lblStudentId);
            pnlHeader.Controls.Add(lblSubmissionTime);
            pnlHeader.Controls.Add(lblMaxPoints);
            pnlHeader.Controls.Add(lblNavCounter);
            pnlHeader.Controls.Add(btnPrevStudent);
            pnlHeader.Controls.Add(btnNextStudent);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1680, 90);
            pnlHeader.TabIndex = 1;
            // 
            // btnBack
            // 
            btnBack.BackColor = Color.FromArgb(100, 0, 0);
            btnBack.BorderRadius = 10;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBack.ForeColor = Color.White;
            btnBack.Location = new Point(12, 28);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(80, 32);
            btnBack.TabIndex = 0;
            btnBack.Text = "← Back";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += btnBack_Click;
            // 
            // lblActivityTitle
            // 
            lblActivityTitle.AutoEllipsis = true;
            lblActivityTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblActivityTitle.ForeColor = Color.White;
            lblActivityTitle.Location = new Point(106, 8);
            lblActivityTitle.Name = "lblActivityTitle";
            lblActivityTitle.Size = new Size(740, 26);
            lblActivityTitle.TabIndex = 1;
            // 
            // lblStudentName
            // 
            lblStudentName.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblStudentName.ForeColor = Color.FromArgb(255, 220, 220);
            lblStudentName.Location = new Point(106, 36);
            lblStudentName.Name = "lblStudentName";
            lblStudentName.Size = new Size(380, 22);
            lblStudentName.TabIndex = 2;
            // 
            // lblStudentId
            // 
            lblStudentId.Font = new Font("Segoe UI", 9F);
            lblStudentId.ForeColor = Color.FromArgb(220, 185, 185);
            lblStudentId.Location = new Point(106, 60);
            lblStudentId.Name = "lblStudentId";
            lblStudentId.Size = new Size(200, 18);
            lblStudentId.TabIndex = 3;
            // 
            // lblSubmissionTime
            // 
            lblSubmissionTime.Font = new Font("Segoe UI", 9F);
            lblSubmissionTime.ForeColor = Color.FromArgb(220, 185, 185);
            lblSubmissionTime.Location = new Point(316, 60);
            lblSubmissionTime.Name = "lblSubmissionTime";
            lblSubmissionTime.Size = new Size(310, 18);
            lblSubmissionTime.TabIndex = 4;
            // 
            // lblMaxPoints
            // 
            lblMaxPoints.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblMaxPoints.ForeColor = Color.FromArgb(255, 196, 0);
            lblMaxPoints.Location = new Point(636, 60);
            lblMaxPoints.Name = "lblMaxPoints";
            lblMaxPoints.Size = new Size(160, 18);
            lblMaxPoints.TabIndex = 5;
            // 
            // lblNavCounter
            // 
            lblNavCounter.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNavCounter.ForeColor = Color.FromArgb(230, 185, 185);
            lblNavCounter.Location = new Point(1380, 60);
            lblNavCounter.Name = "lblNavCounter";
            lblNavCounter.Size = new Size(80, 18);
            lblNavCounter.TabIndex = 8;
            lblNavCounter.Text = "1 / 1";
            lblNavCounter.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnPrevStudent
            // 
            btnPrevStudent.BackColor = Color.FromArgb(90, 0, 0);
            btnPrevStudent.BorderRadius = 10;
            btnPrevStudent.FlatAppearance.BorderSize = 0;
            btnPrevStudent.FlatStyle = FlatStyle.Flat;
            btnPrevStudent.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPrevStudent.ForeColor = Color.White;
            btnPrevStudent.Location = new Point(1464, 28);
            btnPrevStudent.Name = "btnPrevStudent";
            btnPrevStudent.Size = new Size(96, 32);
            btnPrevStudent.TabIndex = 6;
            btnPrevStudent.Text = "◀ Prev";
            btnPrevStudent.UseVisualStyleBackColor = false;
            btnPrevStudent.Click += btnPrevStudent_Click;
            // 
            // btnNextStudent
            // 
            btnNextStudent.BackColor = Color.FromArgb(90, 0, 0);
            btnNextStudent.BorderRadius = 10;
            btnNextStudent.FlatAppearance.BorderSize = 0;
            btnNextStudent.FlatStyle = FlatStyle.Flat;
            btnNextStudent.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNextStudent.ForeColor = Color.White;
            btnNextStudent.Location = new Point(1568, 28);
            btnNextStudent.Name = "btnNextStudent";
            btnNextStudent.Size = new Size(96, 32);
            btnNextStudent.TabIndex = 7;
            btnNextStudent.Text = "Next ▶";
            btnNextStudent.UseVisualStyleBackColor = false;
            btnNextStudent.Click += btnNextStudent_Click;
            // 
            // pnlMain
            // 
            pnlMain.AutoScroll = true;
            pnlMain.BackColor = Color.FromArgb(245, 245, 248);
            pnlMain.Controls.Add(pnlEssay);
            pnlMain.Controls.Add(pnlGrading);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 90);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(1680, 899);
            pnlMain.TabIndex = 0;
            // 
            // pnlEssay
            // 
            pnlEssay.BackColor = Color.White;
            pnlEssay.BorderStyle = BorderStyle.FixedSingle;
            pnlEssay.Controls.Add(lblEssayHeader);
            pnlEssay.Controls.Add(txtEssayContent);
            pnlEssay.Controls.Add(pnlWordCount);
            pnlEssay.Location = new Point(10, 10);
            pnlEssay.Name = "pnlEssay";
            pnlEssay.Size = new Size(1200, 880);
            pnlEssay.TabIndex = 0;
            // 
            // lblEssayHeader
            // 
            lblEssayHeader.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblEssayHeader.ForeColor = Color.FromArgb(128, 0, 0);
            lblEssayHeader.Location = new Point(12, 10);
            lblEssayHeader.Name = "lblEssayHeader";
            lblEssayHeader.Size = new Size(300, 22);
            lblEssayHeader.TabIndex = 0;
            lblEssayHeader.Text = "📄 Submission Content";
            // 
            // txtEssayContent
            // 
            txtEssayContent.BackColor = Color.White;
            txtEssayContent.BorderStyle = BorderStyle.None;
            txtEssayContent.Font = new Font("Segoe UI", 10F);
            txtEssayContent.Location = new Point(12, 40);
            txtEssayContent.Multiline = true;
            txtEssayContent.Name = "txtEssayContent";
            txtEssayContent.ReadOnly = true;
            txtEssayContent.ScrollBars = ScrollBars.Vertical;
            txtEssayContent.Size = new Size(1170, 810);
            txtEssayContent.TabIndex = 1;
            txtEssayContent.TextChanged += txtEssayContent_TextChanged;
            // 
            // pnlWordCount
            // 
            pnlWordCount.BackColor = Color.FromArgb(245, 245, 248);
            pnlWordCount.Controls.Add(lblWordCount);
            pnlWordCount.Controls.Add(lblCharCount);
            pnlWordCount.Location = new Point(12, 852);
            pnlWordCount.Name = "pnlWordCount";
            pnlWordCount.Size = new Size(1170, 22);
            pnlWordCount.TabIndex = 2;
            // 
            // lblWordCount
            // 
            lblWordCount.Font = new Font("Segoe UI", 9F);
            lblWordCount.ForeColor = Color.Gray;
            lblWordCount.Location = new Point(4, 3);
            lblWordCount.Name = "lblWordCount";
            lblWordCount.Size = new Size(100, 16);
            lblWordCount.TabIndex = 0;
            lblWordCount.Text = "Words: 0";
            // 
            // lblCharCount
            // 
            lblCharCount.Font = new Font("Segoe UI", 9F);
            lblCharCount.ForeColor = Color.Gray;
            lblCharCount.Location = new Point(112, 3);
            lblCharCount.Name = "lblCharCount";
            lblCharCount.Size = new Size(160, 16);
            lblCharCount.TabIndex = 1;
            lblCharCount.Text = "Characters: 0";
            // 
            // pnlGrading
            // 
            pnlGrading.BackColor = Color.White;
            pnlGrading.BorderStyle = BorderStyle.FixedSingle;
            pnlGrading.Controls.Add(lblGradingHeader);
            pnlGrading.Controls.Add(lblSaveStatus);
            pnlGrading.Controls.Add(pnlRubricBox);
            pnlGrading.Controls.Add(pnlScoreInput);
            pnlGrading.Controls.Add(pnlRemarks);
            pnlGrading.Location = new Point(1218, 10);
            pnlGrading.Name = "pnlGrading";
            pnlGrading.Size = new Size(446, 880);
            pnlGrading.TabIndex = 1;
            // 
            // lblGradingHeader
            // 
            lblGradingHeader.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblGradingHeader.ForeColor = Color.FromArgb(128, 0, 0);
            lblGradingHeader.Location = new Point(12, 10);
            lblGradingHeader.Name = "lblGradingHeader";
            lblGradingHeader.Size = new Size(200, 24);
            lblGradingHeader.TabIndex = 0;
            lblGradingHeader.Text = "🎯 Grading";
            // 
            // lblSaveStatus
            // 
            lblSaveStatus.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblSaveStatus.ForeColor = Color.ForestGreen;
            lblSaveStatus.Location = new Point(12, 36);
            lblSaveStatus.Name = "lblSaveStatus";
            lblSaveStatus.Size = new Size(420, 18);
            lblSaveStatus.TabIndex = 1;
            // 
            // pnlRubricBox
            // 
            pnlRubricBox.BackColor = Color.FromArgb(251, 249, 249);
            pnlRubricBox.BorderStyle = BorderStyle.FixedSingle;
            pnlRubricBox.Controls.Add(lblRubricHeader);
            pnlRubricBox.Controls.Add(flpRubricRows);
            pnlRubricBox.Controls.Add(lblRubricTotal);
            pnlRubricBox.Controls.Add(chkAutoScore);
            pnlRubricBox.Location = new Point(12, 58);
            pnlRubricBox.Name = "pnlRubricBox";
            pnlRubricBox.Size = new Size(420, 300);
            pnlRubricBox.TabIndex = 2;
            // 
            // lblRubricHeader
            // 
            lblRubricHeader.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblRubricHeader.ForeColor = Color.FromArgb(70, 70, 80);
            lblRubricHeader.Location = new Point(10, 8);
            lblRubricHeader.Name = "lblRubricHeader";
            lblRubricHeader.Size = new Size(240, 20);
            lblRubricHeader.TabIndex = 0;
            lblRubricHeader.Text = "📊 Rubric-Based Grading";
            // 
            // flpRubricRows
            // 
            flpRubricRows.AutoSize = true;
            flpRubricRows.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpRubricRows.FlowDirection = FlowDirection.TopDown;
            flpRubricRows.Location = new Point(10, 34);
            flpRubricRows.Name = "flpRubricRows";
            flpRubricRows.Size = new Size(0, 0);
            flpRubricRows.TabIndex = 1;
            flpRubricRows.WrapContents = false;
            // 
            // lblRubricTotal
            // 
            lblRubricTotal.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblRubricTotal.ForeColor = Color.FromArgb(128, 0, 0);
            lblRubricTotal.Location = new Point(10, 240);
            lblRubricTotal.Name = "lblRubricTotal";
            lblRubricTotal.Size = new Size(280, 22);
            lblRubricTotal.TabIndex = 2;
            lblRubricTotal.Text = "Rubric Total: 0 / 100";
            // 
            // chkAutoScore
            // 
            chkAutoScore.Font = new Font("Segoe UI", 9F);
            chkAutoScore.ForeColor = Color.FromArgb(60, 60, 70);
            chkAutoScore.Location = new Point(10, 268);
            chkAutoScore.Name = "chkAutoScore";
            chkAutoScore.Size = new Size(240, 22);
            chkAutoScore.TabIndex = 3;
            chkAutoScore.Text = "Auto-fill score from rubric total";
            chkAutoScore.CheckedChanged += chkAutoScore_CheckedChanged;
            // 
            // pnlScoreInput
            // 
            pnlScoreInput.BackColor = Color.White;
            pnlScoreInput.Controls.Add(lblScoreLabel);
            pnlScoreInput.Controls.Add(txtScore);
            pnlScoreInput.Controls.Add(lblScoreOf);
            pnlScoreInput.Controls.Add(btnSaveScore);
            pnlScoreInput.Location = new Point(12, 368);
            pnlScoreInput.Name = "pnlScoreInput";
            pnlScoreInput.Size = new Size(420, 90);
            pnlScoreInput.TabIndex = 3;
            // 
            // lblScoreLabel
            // 
            lblScoreLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblScoreLabel.ForeColor = Color.FromArgb(35, 35, 45);
            lblScoreLabel.Location = new Point(4, 12);
            lblScoreLabel.Name = "lblScoreLabel";
            lblScoreLabel.Size = new Size(115, 24);
            lblScoreLabel.TabIndex = 0;
            lblScoreLabel.Text = "Final Score:";
            // 
            // txtScore
            // 
            txtScore.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            txtScore.Location = new Point(122, 8);
            txtScore.MaxLength = 5;
            txtScore.Name = "txtScore";
            txtScore.Size = new Size(80, 34);
            txtScore.TabIndex = 1;
            txtScore.TextAlign = HorizontalAlignment.Center;
            // 
            // lblScoreOf
            // 
            lblScoreOf.Font = new Font("Segoe UI", 11F);
            lblScoreOf.ForeColor = Color.Gray;
            lblScoreOf.Location = new Point(210, 14);
            lblScoreOf.Name = "lblScoreOf";
            lblScoreOf.Size = new Size(80, 22);
            lblScoreOf.TabIndex = 2;
            lblScoreOf.Text = "/ 100";
            // 
            // btnSaveScore
            // 
            btnSaveScore.BackColor = Color.FromArgb(128, 0, 0);
            btnSaveScore.BorderRadius = 10;
            btnSaveScore.FlatAppearance.BorderSize = 0;
            btnSaveScore.FlatStyle = FlatStyle.Flat;
            btnSaveScore.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSaveScore.ForeColor = Color.White;
            btnSaveScore.Location = new Point(4, 54);
            btnSaveScore.Name = "btnSaveScore";
            btnSaveScore.Size = new Size(149, 32);
            btnSaveScore.TabIndex = 3;
            btnSaveScore.Text = "✔ Save and Lock Score";
            btnSaveScore.UseVisualStyleBackColor = false;
            btnSaveScore.Click += btnSaveScore_Click;
            // 
            // pnlRemarks
            // 
            pnlRemarks.BackColor = Color.White;
            pnlRemarks.Controls.Add(lblRemarksHeader);
            pnlRemarks.Controls.Add(txtRemarks);
            pnlRemarks.Location = new Point(12, 468);
            pnlRemarks.Name = "pnlRemarks";
            pnlRemarks.Size = new Size(420, 240);
            pnlRemarks.TabIndex = 4;
            // 
            // lblRemarksHeader
            // 
            lblRemarksHeader.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblRemarksHeader.ForeColor = Color.FromArgb(70, 70, 80);
            lblRemarksHeader.Location = new Point(4, 4);
            lblRemarksHeader.Name = "lblRemarksHeader";
            lblRemarksHeader.Size = new Size(260, 22);
            lblRemarksHeader.TabIndex = 0;
            lblRemarksHeader.Text = "💬 Remarks / Feedback";
            // 
            // txtRemarks
            // 
            txtRemarks.Font = new Font("Segoe UI", 9.5F);
            txtRemarks.Location = new Point(0, 30);
            txtRemarks.Multiline = true;
            txtRemarks.Name = "txtRemarks";
            txtRemarks.PlaceholderText = "Add comments or feedback for the student...";
            txtRemarks.ScrollBars = ScrollBars.Vertical;
            txtRemarks.Size = new Size(418, 205);
            txtRemarks.TabIndex = 1;
            // 
            // GradingInterface
            // 
            Controls.Add(pnlMain);
            Controls.Add(pnlHeader);
            Name = "GradingInterface";
            Size = new Size(1680, 989);
            pnlHeader.ResumeLayout(false);
            pnlMain.ResumeLayout(false);
            pnlEssay.ResumeLayout(false);
            pnlEssay.PerformLayout();
            pnlWordCount.ResumeLayout(false);
            pnlGrading.ResumeLayout(false);
            pnlRubricBox.ResumeLayout(false);
            pnlRubricBox.PerformLayout();
            pnlScoreInput.ResumeLayout(false);
            pnlScoreInput.PerformLayout();
            pnlRemarks.ResumeLayout(false);
            pnlRemarks.PerformLayout();
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
        private System.Windows.Forms.Label lblEssayHeader;
        private System.Windows.Forms.TextBox txtEssayContent;
        private System.Windows.Forms.Panel pnlWordCount;
        private System.Windows.Forms.Label lblWordCount;
        private System.Windows.Forms.Label lblCharCount;
        private System.Windows.Forms.Panel pnlGrading;
        private System.Windows.Forms.Label lblGradingHeader;
        private System.Windows.Forms.Label lblSaveStatus;
        private System.Windows.Forms.Panel pnlRubricBox;
        private System.Windows.Forms.Label lblRubricHeader;
        private System.Windows.Forms.FlowLayoutPanel flpRubricRows;
        private System.Windows.Forms.Label lblRubricTotal;
        private System.Windows.Forms.CheckBox chkAutoScore;
        private System.Windows.Forms.Panel pnlScoreInput;
        private System.Windows.Forms.Label lblScoreLabel;
        private System.Windows.Forms.TextBox txtScore;
        private System.Windows.Forms.Label lblScoreOf;
        private buttonRounded btnSaveScore;
        private System.Windows.Forms.Panel pnlRemarks;
        private System.Windows.Forms.Label lblRemarksHeader;
        private System.Windows.Forms.TextBox txtRemarks;
    }
}