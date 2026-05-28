using System.Reflection.PortableExecutable;

namespace PUPAcadPortal
{
    partial class GradingInterface
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            btnBack = new buttonRounded();
            lblActivityTitle = new Label();
            lblStudentName = new Label();
            lblStudentId = new Label();
            lblSubmissionTime = new Label();
            lblMaxPoints = new Label();
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
            pnlRubric = new Panel();
            lblRubricHeader = new Label();
            lblContentLabel = new Label();
            nudContent = new NumericUpDown();
            lblContentMax = new Label();
            lblGrammarLabel = new Label();
            nudGrammar = new NumericUpDown();
            lblGrammarMax = new Label();
            lblRelevanceLabel = new Label();
            nudRelevance = new NumericUpDown();
            lblRelevanceMax = new Label();
            lblStructureLabel = new Label();
            nudStructure = new NumericUpDown();
            lblRubricTotal = new Label();
            chkAutoScore = new CheckBox();
            pnlScoreInput = new Panel();
            lblScoreLabel = new Label();
            txtScore = new TextBox();
            lblScoreOf = new Label();
            btnSaveScore = new buttonRounded();
            lblSaveStatus = new Label();
            pnlRemarks = new Panel();
            lblRemarksHeader = new Label();
            txtRemarks = new TextBox();
            lblStructureMax = new Label();
            pnlHeader.SuspendLayout();
            pnlMain.SuspendLayout();
            pnlEssay.SuspendLayout();
            pnlWordCount.SuspendLayout();
            pnlGrading.SuspendLayout();
            pnlRubric.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudContent).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudGrammar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRelevance).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudStructure).BeginInit();
            pnlScoreInput.SuspendLayout();
            pnlRemarks.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.Maroon;
            pnlHeader.Controls.Add(btnBack);
            pnlHeader.Controls.Add(lblActivityTitle);
            pnlHeader.Controls.Add(lblStudentName);
            pnlHeader.Controls.Add(lblStudentId);
            pnlHeader.Controls.Add(lblSubmissionTime);
            pnlHeader.Controls.Add(lblMaxPoints);
            pnlHeader.Controls.Add(btnPrevStudent);
            pnlHeader.Controls.Add(btnNextStudent);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1640, 90);
            pnlHeader.TabIndex = 1;
            // 
            // btnBack
            // 
            btnBack.BackColor = Color.FromArgb(109, 0, 0);
            btnBack.BorderRadius = 10;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBack.ForeColor = Color.White;
            btnBack.Location = new Point(12, 26);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(80, 32);
            btnBack.TabIndex = 0;
            btnBack.Text = "Back";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += btnBack_Click;
            // 
            // lblActivityTitle
            // 
            lblActivityTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblActivityTitle.ForeColor = Color.White;
            lblActivityTitle.Location = new Point(105, 8);
            lblActivityTitle.Name = "lblActivityTitle";
            lblActivityTitle.Size = new Size(750, 26);
            lblActivityTitle.TabIndex = 1;
            lblActivityTitle.Text = "Activity Title";
            // 
            // lblStudentName
            // 
            lblStudentName.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblStudentName.ForeColor = Color.FromArgb(255, 220, 220);
            lblStudentName.Location = new Point(105, 36);
            lblStudentName.Name = "lblStudentName";
            lblStudentName.Size = new Size(380, 22);
            lblStudentName.TabIndex = 2;
            lblStudentName.Text = "Student Name";
            // 
            // lblStudentId
            // 
            lblStudentId.Font = new Font("Segoe UI", 9F);
            lblStudentId.ForeColor = Color.FromArgb(220, 180, 180);
            lblStudentId.Location = new Point(105, 60);
            lblStudentId.Name = "lblStudentId";
            lblStudentId.Size = new Size(200, 18);
            lblStudentId.TabIndex = 3;
            lblStudentId.Text = "2024-00001-SM-0";
            // 
            // lblSubmissionTime
            // 
            lblSubmissionTime.Font = new Font("Segoe UI", 9F);
            lblSubmissionTime.ForeColor = Color.FromArgb(220, 180, 180);
            lblSubmissionTime.Location = new Point(315, 60);
            lblSubmissionTime.Name = "lblSubmissionTime";
            lblSubmissionTime.Size = new Size(300, 18);
            lblSubmissionTime.TabIndex = 4;
            lblSubmissionTime.Text = "Submitted: --";
            // 
            // lblMaxPoints
            // 
            lblMaxPoints.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblMaxPoints.ForeColor = Color.FromArgb(255, 193, 7);
            lblMaxPoints.Location = new Point(625, 60);
            lblMaxPoints.Name = "lblMaxPoints";
            lblMaxPoints.Size = new Size(180, 18);
            lblMaxPoints.TabIndex = 5;
            lblMaxPoints.Text = "Max Points: 100";
            // 
            // btnPrevStudent
            // 
            btnPrevStudent.BackColor = Color.FromArgb(60, 60, 60);
            btnPrevStudent.BorderRadius = 10;
            btnPrevStudent.FlatAppearance.BorderSize = 0;
            btnPrevStudent.FlatStyle = FlatStyle.Flat;
            btnPrevStudent.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPrevStudent.ForeColor = Color.White;
            btnPrevStudent.Location = new Point(1415, 26);
            btnPrevStudent.Name = "btnPrevStudent";
            btnPrevStudent.Size = new Size(110, 32);
            btnPrevStudent.TabIndex = 6;
            btnPrevStudent.Text = "Prev";
            btnPrevStudent.UseVisualStyleBackColor = false;
            btnPrevStudent.Click += btnPrevStudent_Click;
            // 
            // btnNextStudent
            // 
            btnNextStudent.BackColor = Color.FromArgb(60, 60, 60);
            btnNextStudent.BorderRadius = 10;
            btnNextStudent.FlatAppearance.BorderSize = 0;
            btnNextStudent.FlatStyle = FlatStyle.Flat;
            btnNextStudent.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNextStudent.ForeColor = Color.White;
            btnNextStudent.Location = new Point(1531, 26);
            btnNextStudent.Name = "btnNextStudent";
            btnNextStudent.Size = new Size(110, 32);
            btnNextStudent.TabIndex = 7;
            btnNextStudent.Text = "Next";
            btnNextStudent.UseVisualStyleBackColor = false;
            btnNextStudent.Click += btnNextStudent_Click;
            // 
            // pnlMain
            // 
            pnlMain.AllowDrop = true;
            pnlMain.AutoScroll = true;
            pnlMain.BackColor = Color.FromArgb(245, 245, 245);
            pnlMain.Controls.Add(pnlEssay);
            pnlMain.Controls.Add(pnlGrading);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 90);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(1640, 899);
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
            pnlEssay.Size = new Size(1195, 873);
            pnlEssay.TabIndex = 0;
            // 
            // lblEssayHeader
            // 
            lblEssayHeader.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblEssayHeader.ForeColor = Color.Maroon;
            lblEssayHeader.Location = new Point(10, 8);
            lblEssayHeader.Name = "lblEssayHeader";
            lblEssayHeader.Size = new Size(300, 22);
            lblEssayHeader.TabIndex = 0;
            lblEssayHeader.Text = "Submission Content";
            // 
            // txtEssayContent
            // 
            txtEssayContent.BackColor = Color.White;
            txtEssayContent.BorderStyle = BorderStyle.None;
            txtEssayContent.Font = new Font("Segoe UI", 10F);
            txtEssayContent.Location = new Point(10, 38);
            txtEssayContent.Multiline = true;
            txtEssayContent.Name = "txtEssayContent";
            txtEssayContent.ReadOnly = true;
            txtEssayContent.ScrollBars = ScrollBars.Vertical;
            txtEssayContent.Size = new Size(1170, 779);
            txtEssayContent.TabIndex = 1;
            txtEssayContent.TextChanged += txtEssayContent_TextChanged;
            // 
            // pnlWordCount
            // 
            pnlWordCount.BackColor = Color.FromArgb(245, 245, 245);
            pnlWordCount.Controls.Add(lblWordCount);
            pnlWordCount.Controls.Add(lblCharCount);
            pnlWordCount.Location = new Point(10, 846);
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
            lblCharCount.Location = new Point(110, 3);
            lblCharCount.Name = "lblCharCount";
            lblCharCount.Size = new Size(150, 16);
            lblCharCount.TabIndex = 1;
            lblCharCount.Text = "Characters: 0";
            // 
            // pnlGrading
            // 
            pnlGrading.BackColor = Color.White;
            pnlGrading.BorderStyle = BorderStyle.FixedSingle;
            pnlGrading.Controls.Add(lblGradingHeader);
            pnlGrading.Controls.Add(pnlRubric);
            pnlGrading.Controls.Add(pnlScoreInput);
            pnlGrading.Controls.Add(pnlRemarks);
            pnlGrading.Location = new Point(1211, 10);
            pnlGrading.Name = "pnlGrading";
            pnlGrading.Size = new Size(430, 600);
            pnlGrading.TabIndex = 1;
            // 
            // lblGradingHeader
            // 
            lblGradingHeader.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblGradingHeader.ForeColor = Color.Maroon;
            lblGradingHeader.Location = new Point(10, 8);
            lblGradingHeader.Name = "lblGradingHeader";
            lblGradingHeader.Size = new Size(200, 22);
            lblGradingHeader.TabIndex = 0;
            lblGradingHeader.Text = "Grading";
            // 
            // pnlRubric
            // 
            pnlRubric.BackColor = Color.FromArgb(250, 248, 248);
            pnlRubric.BorderStyle = BorderStyle.FixedSingle;
            pnlRubric.Controls.Add(lblRubricHeader);
            pnlRubric.Controls.Add(lblContentLabel);
            pnlRubric.Controls.Add(nudContent);
            pnlRubric.Controls.Add(lblContentMax);
            pnlRubric.Controls.Add(lblGrammarLabel);
            pnlRubric.Controls.Add(nudGrammar);
            pnlRubric.Controls.Add(lblGrammarMax);
            pnlRubric.Controls.Add(lblRelevanceLabel);
            pnlRubric.Controls.Add(nudRelevance);
            pnlRubric.Controls.Add(lblRelevanceMax);
            pnlRubric.Controls.Add(lblStructureLabel);
            pnlRubric.Controls.Add(nudStructure);
            pnlRubric.Controls.Add(lblStructureMax);
            pnlRubric.Controls.Add(lblRubricTotal);
            pnlRubric.Controls.Add(chkAutoScore);
            pnlRubric.Location = new Point(10, 38);
            pnlRubric.Name = "pnlRubric";
            pnlRubric.Size = new Size(408, 220);
            pnlRubric.TabIndex = 1;
            // 
            // lblRubricHeader
            // 
            lblRubricHeader.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblRubricHeader.ForeColor = Color.FromArgb(80, 80, 80);
            lblRubricHeader.Location = new Point(15, 0);
            lblRubricHeader.Name = "lblRubricHeader";
            lblRubricHeader.Size = new Size(226, 20);
            lblRubricHeader.TabIndex = 0;
            lblRubricHeader.Text = "Rubric-Based Grading";
            // 
            // lblContentLabel
            // 
            lblContentLabel.Location = new Point(15, 29);
            lblContentLabel.Name = "lblContentLabel";
            lblContentLabel.Size = new Size(100, 23);
            lblContentLabel.TabIndex = 1;
            // 
            // nudContent
            // 
            nudContent.Location = new Point(121, 29);
            nudContent.Name = "nudContent";
            nudContent.Size = new Size(120, 23);
            nudContent.TabIndex = 2;
            nudContent.ValueChanged += nudRubric_ValueChanged;
            // 
            // lblContentMax
            // 
            lblContentMax.Location = new Point(15, 30);
            lblContentMax.Name = "lblContentMax";
            lblContentMax.Size = new Size(100, 23);
            lblContentMax.TabIndex = 3;
            // 
            // lblGrammarLabel
            // 
            lblGrammarLabel.Location = new Point(15, 94);
            lblGrammarLabel.Name = "lblGrammarLabel";
            lblGrammarLabel.Size = new Size(100, 23);
            lblGrammarLabel.TabIndex = 4;
            // 
            // nudGrammar
            // 
            nudGrammar.Location = new Point(121, 92);
            nudGrammar.Name = "nudGrammar";
            nudGrammar.Size = new Size(120, 23);
            nudGrammar.TabIndex = 5;
            nudGrammar.ValueChanged += nudRubric_ValueChanged;
            // 
            // lblGrammarMax
            // 
            lblGrammarMax.Location = new Point(15, 94);
            lblGrammarMax.Name = "lblGrammarMax";
            lblGrammarMax.Size = new Size(100, 23);
            lblGrammarMax.TabIndex = 6;
            // 
            // lblRelevanceLabel
            // 
            lblRelevanceLabel.Location = new Point(15, 124);
            lblRelevanceLabel.Name = "lblRelevanceLabel";
            lblRelevanceLabel.Size = new Size(100, 23);
            lblRelevanceLabel.TabIndex = 7;
            // 
            // nudRelevance
            // 
            nudRelevance.Location = new Point(121, 122);
            nudRelevance.Name = "nudRelevance";
            nudRelevance.Size = new Size(120, 23);
            nudRelevance.TabIndex = 8;
            nudRelevance.ValueChanged += nudRubric_ValueChanged;
            // 
            // lblRelevanceMax
            // 
            lblRelevanceMax.Location = new Point(15, 124);
            lblRelevanceMax.Name = "lblRelevanceMax";
            lblRelevanceMax.Size = new Size(100, 23);
            lblRelevanceMax.TabIndex = 9;
            // 
            // lblStructureLabel
            // 
            lblStructureLabel.Location = new Point(15, 58);
            lblStructureLabel.Name = "lblStructureLabel";
            lblStructureLabel.Size = new Size(100, 23);
            lblStructureLabel.TabIndex = 10;
            // 
            // nudStructure
            // 
            nudStructure.Location = new Point(121, 58);
            nudStructure.Name = "nudStructure";
            nudStructure.Size = new Size(120, 23);
            nudStructure.TabIndex = 11;
            nudStructure.ValueChanged += nudRubric_ValueChanged;
            // 
            // lblRubricTotal
            // 
            lblRubricTotal.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblRubricTotal.ForeColor = Color.Maroon;
            lblRubricTotal.Location = new Point(8, 158);
            lblRubricTotal.Name = "lblRubricTotal";
            lblRubricTotal.Size = new Size(260, 22);
            lblRubricTotal.TabIndex = 13;
            lblRubricTotal.Text = "Rubric Total: 0 / 100";
            // 
            // chkAutoScore
            // 
            chkAutoScore.Font = new Font("Segoe UI", 9F);
            chkAutoScore.Location = new Point(8, 184);
            chkAutoScore.Name = "chkAutoScore";
            chkAutoScore.Size = new Size(240, 22);
            chkAutoScore.TabIndex = 14;
            chkAutoScore.Text = "Auto-fill score from rubric";
            chkAutoScore.CheckedChanged += chkAutoScore_CheckedChanged;
            // 
            // pnlScoreInput
            // 
            pnlScoreInput.BackColor = Color.White;
            pnlScoreInput.Controls.Add(lblScoreLabel);
            pnlScoreInput.Controls.Add(txtScore);
            pnlScoreInput.Controls.Add(lblScoreOf);
            pnlScoreInput.Controls.Add(btnSaveScore);
            pnlScoreInput.Controls.Add(lblSaveStatus);
            pnlScoreInput.Location = new Point(10, 266);
            pnlScoreInput.Name = "pnlScoreInput";
            pnlScoreInput.Size = new Size(408, 88);
            pnlScoreInput.TabIndex = 2;
            // 
            // lblScoreLabel
            // 
            lblScoreLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblScoreLabel.ForeColor = Color.FromArgb(40, 40, 40);
            lblScoreLabel.Location = new Point(3, 10);
            lblScoreLabel.Name = "lblScoreLabel";
            lblScoreLabel.Size = new Size(110, 22);
            lblScoreLabel.TabIndex = 0;
            lblScoreLabel.Text = "Final Score:";
            // 
            // txtScore
            // 
            txtScore.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            txtScore.Location = new Point(115, 6);
            txtScore.MaxLength = 4;
            txtScore.Name = "txtScore";
            txtScore.Size = new Size(80, 32);
            txtScore.TabIndex = 1;
            txtScore.TextAlign = HorizontalAlignment.Center;
            // 
            // lblScoreOf
            // 
            lblScoreOf.Font = new Font("Segoe UI", 11F);
            lblScoreOf.ForeColor = Color.Gray;
            lblScoreOf.Location = new Point(202, 12);
            lblScoreOf.Name = "lblScoreOf";
            lblScoreOf.Size = new Size(70, 22);
            lblScoreOf.TabIndex = 2;
            lblScoreOf.Text = "/ 100";
            // 
            // btnSaveScore
            // 
            btnSaveScore.BackColor = Color.Maroon;
            btnSaveScore.BorderRadius = 10;
            btnSaveScore.FlatAppearance.BorderSize = 0;
            btnSaveScore.FlatStyle = FlatStyle.Flat;
            btnSaveScore.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSaveScore.ForeColor = Color.White;
            btnSaveScore.Location = new Point(2, 50);
            btnSaveScore.Name = "btnSaveScore";
            btnSaveScore.Size = new Size(120, 32);
            btnSaveScore.TabIndex = 3;
            btnSaveScore.Text = "Save Score";
            btnSaveScore.UseVisualStyleBackColor = false;
            btnSaveScore.Click += btnSaveScore_Click;
            // 
            // lblSaveStatus
            // 
            lblSaveStatus.Font = new Font("Segoe UI", 9F);
            lblSaveStatus.ForeColor = Color.ForestGreen;
            lblSaveStatus.Location = new Point(128, 58);
            lblSaveStatus.Name = "lblSaveStatus";
            lblSaveStatus.Size = new Size(260, 18);
            lblSaveStatus.TabIndex = 4;
            // 
            // pnlRemarks
            // 
            pnlRemarks.BackColor = Color.White;
            pnlRemarks.Controls.Add(lblRemarksHeader);
            pnlRemarks.Controls.Add(txtRemarks);
            pnlRemarks.Location = new Point(10, 362);
            pnlRemarks.Name = "pnlRemarks";
            pnlRemarks.Size = new Size(408, 220);
            pnlRemarks.TabIndex = 3;
            // 
            // lblRemarksHeader
            // 
            lblRemarksHeader.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblRemarksHeader.ForeColor = Color.FromArgb(80, 80, 80);
            lblRemarksHeader.Location = new Point(3, 3);
            lblRemarksHeader.Name = "lblRemarksHeader";
            lblRemarksHeader.Size = new Size(250, 20);
            lblRemarksHeader.TabIndex = 0;
            lblRemarksHeader.Text = "Remarks / Feedback";
            // 
            // txtRemarks
            // 
            txtRemarks.Font = new Font("Segoe UI", 9F);
            txtRemarks.Location = new Point(0, 26);
            txtRemarks.Multiline = true;
            txtRemarks.Name = "txtRemarks";
            txtRemarks.PlaceholderText = "Add comments or feedback for the student...";
            txtRemarks.ScrollBars = ScrollBars.Vertical;
            txtRemarks.Size = new Size(405, 185);
            txtRemarks.TabIndex = 1;
            // 
            // lblStructureMax
            // 
            lblStructureMax.Location = new Point(15, 58);
            lblStructureMax.Name = "lblStructureMax";
            lblStructureMax.Size = new Size(100, 23);
            lblStructureMax.TabIndex = 12;
            // 
            // GradingInterface
            // 
            Controls.Add(pnlMain);
            Controls.Add(pnlHeader);
            Name = "GradingInterface";
            Size = new Size(1640, 989);
            pnlHeader.ResumeLayout(false);
            pnlMain.ResumeLayout(false);
            pnlEssay.ResumeLayout(false);
            pnlEssay.PerformLayout();
            pnlWordCount.ResumeLayout(false);
            pnlGrading.ResumeLayout(false);
            pnlRubric.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nudContent).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudGrammar).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRelevance).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudStructure).EndInit();
            pnlScoreInput.ResumeLayout(false);
            pnlScoreInput.PerformLayout();
            pnlRemarks.ResumeLayout(false);
            pnlRemarks.PerformLayout();
            ResumeLayout(false);
        }

        /// <summary>
        /// Configures one rubric row (label + NUD + max label) at a given Y position.
        /// Called only from InitializeComponent — this is a Designer-side helper, not logic.
        /// </summary>
        private static void SetupRubricRow(
            System.Windows.Forms.Label lbl, string name,
            System.Windows.Forms.NumericUpDown nud, int max,
            System.Windows.Forms.Label lblMax, int y)
        {
            lbl.Text = name;
            lbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            lbl.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            lbl.Location = new System.Drawing.Point(8, y + 5);
            lbl.Size = new System.Drawing.Size(100, 18);
            lbl.Name = $"lbl{name}Label";

            nud.Minimum = 0;
            nud.Maximum = max;
            nud.Value = 0;
            nud.Location = new System.Drawing.Point(115, y);
            nud.Size = new System.Drawing.Size(60, 24);
            nud.Font = new System.Drawing.Font("Segoe UI", 9F);
            nud.Name = $"nud{name}";

            lblMax.Text = $"/ {max}";
            lblMax.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblMax.ForeColor = System.Drawing.Color.Gray;
            lblMax.Location = new System.Drawing.Point(180, y + 5);
            lblMax.Size = new System.Drawing.Size(50, 18);
            lblMax.Name = $"lbl{name}Max";
        }

        #endregion

        // ── Control declarations ───────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlHeader;
        private buttonRounded btnBack;
        private System.Windows.Forms.Label lblActivityTitle;
        private System.Windows.Forms.Label lblStudentName;
        private System.Windows.Forms.Label lblStudentId;
        private System.Windows.Forms.Label lblSubmissionTime;
        private System.Windows.Forms.Label lblMaxPoints;
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
        private System.Windows.Forms.Panel pnlRubric;
        private System.Windows.Forms.Label lblRubricHeader;
        private System.Windows.Forms.Label lblContentLabel;
        private System.Windows.Forms.NumericUpDown nudContent;
        private System.Windows.Forms.Label lblContentMax;
        private System.Windows.Forms.Label lblGrammarLabel;
        private System.Windows.Forms.NumericUpDown nudGrammar;
        private System.Windows.Forms.Label lblGrammarMax;
        private System.Windows.Forms.Label lblRelevanceLabel;
        private System.Windows.Forms.NumericUpDown nudRelevance;
        private System.Windows.Forms.Label lblRelevanceMax;
        private System.Windows.Forms.Label lblStructureLabel;
        private System.Windows.Forms.NumericUpDown nudStructure;
        private System.Windows.Forms.Label lblRubricTotal;
        private System.Windows.Forms.CheckBox chkAutoScore;

        private System.Windows.Forms.Panel pnlScoreInput;
        private System.Windows.Forms.Label lblScoreLabel;
        private System.Windows.Forms.TextBox txtScore;
        private System.Windows.Forms.Label lblScoreOf;
        private buttonRounded btnSaveScore;
        private System.Windows.Forms.Label lblSaveStatus;

        private System.Windows.Forms.Panel pnlRemarks;
        private System.Windows.Forms.Label lblRemarksHeader;
        private System.Windows.Forms.TextBox txtRemarks;
        private Label lblStructureMax;
    }
}