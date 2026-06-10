namespace PUPAcadPortal
{
    partial class ActivityFormPage
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            // ── Instantiate all controls ────────────────────────────────────
            pnlHeader = new Panel();
            btnCancel = new buttonRounded();
            lblPageTitle = new Label();
            lblCourseSub = new Label();
            btnSave = new buttonRounded();
            pnlScroll = new Panel();
            stackPanel = new FlowLayoutPanel();
            lblError = new Label();
            pnlBasic = new Panel();
            lblSectionBasic = new Label();
            lblTitleLbl = new Label();
            txtTitle = new TextBox();
            lblTypeLbl = new Label();
            cmbType = new ComboBox();
            lblDeadlineLbl = new Label();
            dtpDeadline = new DateTimePicker();
            lblPointsLbl = new Label();
            nudPoints = new NumericUpDown();
            lblPointsNote = new Label();
            lblDescLbl = new Label();
            txtDescription = new TextBox();
            // Quiz section
            pnlQuizSection = new Panel();
            lblQuizHeader = new Label();
            btnAddQuestion = new buttonRounded();
            flpQuestions = new FlowLayoutPanel();
            // Rubric section – outer wrapper
            pnlRubricSection = new Panel();
            // Left column
            pnlRubricLeft = new Panel();
            pnlRubricHeader = new Panel();
            chkRubric = new CheckBox();
            lblRubricNote = new Label();
            btnAddCriteria = new buttonRounded();
            pnlCriteriaScroll = new Panel();
            flpRubric = new FlowLayoutPanel();
            pnlRubricRows = new Panel();
            // Right column – summary panel
            pnlRubricSummary = new Panel();
            lblSummaryHeader = new Label();
            pnlSumDivider = new Panel();
            // Stat cards row 1
            pnlStatCards = new Panel();
            pnlStatCriteria = new Panel();
            lblStatCriteriaIcon = new Label();
            lblStatCriteriaVal = new Label();
            lblStatCriteriaLbl = new Label();
            pnlStatTotalPts = new Panel();
            lblStatTotalIcon = new Label();
            lblStatTotalVal = new Label();
            lblStatTotalLbl = new Label();
            // Stat cards row 2
            pnlStatCardsRow2 = new Panel();
            pnlStatRemaining = new Panel();
            lblStatRemainingIcon = new Label();
            lblStatRemainingVal = new Label();
            lblStatRemainingLbl = new Label();
            pnlStatStatus = new Panel();
            lblStatStatusIcon = new Label();
            lblStatStatusVal = new Label();
            lblStatStatusLbl = new Label();
            // Progress bar
            pnlProgressArea = new Panel();
            lblProgressLbl = new Label();
            pnlProgressBg = new Panel();
            pnlProgressFill = new Panel();
            lblProgressPct = new Label();
            // Guidelines
            pnlGradeGuidelines = new Panel();
            lblGuidelinesHeader = new Label();
            lblGuidelineText = new Label();
            // Validation
            pnlValidation = new Panel();
            lblValidationMsg = new Label();
            // Files section
            pnlFilesSection = new Panel();
            lblFilesHeader = new Label();
            btnAttachFile = new buttonRounded();
            lblNoFiles = new Label();
            flpFiles = new FlowLayoutPanel();

            pnlHeader.SuspendLayout();
            pnlScroll.SuspendLayout();
            stackPanel.SuspendLayout();
            pnlBasic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPoints).BeginInit();
            pnlQuizSection.SuspendLayout();
            pnlRubricSection.SuspendLayout();
            pnlRubricLeft.SuspendLayout();
            pnlRubricHeader.SuspendLayout();
            pnlCriteriaScroll.SuspendLayout();
            pnlRubricRows.SuspendLayout();
            pnlRubricSummary.SuspendLayout();
            pnlStatCards.SuspendLayout();
            pnlStatCriteria.SuspendLayout();
            pnlStatTotalPts.SuspendLayout();
            pnlStatCardsRow2.SuspendLayout();
            pnlStatRemaining.SuspendLayout();
            pnlStatStatus.SuspendLayout();
            pnlProgressArea.SuspendLayout();
            pnlProgressBg.SuspendLayout();
            pnlGradeGuidelines.SuspendLayout();
            pnlValidation.SuspendLayout();
            pnlFilesSection.SuspendLayout();
            SuspendLayout();

            // ── pnlHeader ───────────────────────────────────────────────────
            pnlHeader.BackColor = Color.FromArgb(128, 0, 0);
            pnlHeader.Controls.Add(btnCancel);
            pnlHeader.Controls.Add(lblPageTitle);
            pnlHeader.Controls.Add(lblCourseSub);
            pnlHeader.Controls.Add(btnSave);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1680, 68);
            pnlHeader.TabIndex = 0;
            pnlHeader.SizeChanged += pnlHeader_SizeChanged;

            btnCancel.BackColor = Color.FromArgb(100, 0, 0);
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(12, 18);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(82, 32);
            btnCancel.TabIndex = 0;
            btnCancel.Text = "← Back";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;

            lblPageTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblPageTitle.ForeColor = Color.White;
            lblPageTitle.Location = new Point(108, 10);
            lblPageTitle.Name = "lblPageTitle";
            lblPageTitle.Size = new Size(500, 28);
            lblPageTitle.TabIndex = 1;
            lblPageTitle.Text = "Create Activity";

            lblCourseSub.Font = new Font("Segoe UI", 9F);
            lblCourseSub.ForeColor = Color.FromArgb(230, 185, 185);
            lblCourseSub.Location = new Point(108, 42);
            lblCourseSub.Name = "lblCourseSub";
            lblCourseSub.Size = new Size(500, 18);
            lblCourseSub.TabIndex = 2;

            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.BackColor = Color.FromArgb(255, 196, 0);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnSave.ForeColor = Color.Black;
            btnSave.Location = new Point(1510, 17);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(160, 34);
            btnSave.TabIndex = 3;
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;

            // ── pnlScroll ───────────────────────────────────────────────────
            pnlScroll.AutoScroll = true;
            pnlScroll.BackColor = Color.FromArgb(245, 245, 248);
            pnlScroll.Controls.Add(stackPanel);
            pnlScroll.Dock = DockStyle.Fill;
            pnlScroll.Location = new Point(0, 68);
            pnlScroll.Name = "pnlScroll";
            pnlScroll.Padding = new Padding(22, 18, 22, 18);
            pnlScroll.Size = new Size(1680, 921);
            pnlScroll.TabIndex = 1;
            pnlScroll.SizeChanged += pnlScroll_SizeChanged;

            // ── stackPanel ──────────────────────────────────────────────────
            stackPanel.AutoSize = true;
            stackPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            stackPanel.Controls.Add(lblError);
            stackPanel.Controls.Add(pnlBasic);
            stackPanel.Controls.Add(pnlQuizSection);
            stackPanel.Controls.Add(pnlRubricSection);
            stackPanel.Controls.Add(pnlFilesSection);
            stackPanel.Dock = DockStyle.Top;
            stackPanel.FlowDirection = FlowDirection.TopDown;
            stackPanel.Location = new Point(22, 18);
            stackPanel.Name = "stackPanel";
            stackPanel.Size = new Size(1636, 631);
            stackPanel.TabIndex = 0;
            stackPanel.WrapContents = false;

            // ── lblError ────────────────────────────────────────────────────
            lblError.AutoSize = true;
            lblError.Font = new Font("Segoe UI", 9.5F);
            lblError.ForeColor = Color.Red;
            lblError.Location = new Point(3, 0);
            lblError.Name = "lblError";
            lblError.Size = new Size(0, 17);
            lblError.TabIndex = 0;

            // ── pnlBasic ────────────────────────────────────────────────────
            pnlBasic.BackColor = Color.White;
            pnlBasic.Controls.Add(lblSectionBasic);
            pnlBasic.Controls.Add(lblTitleLbl);
            pnlBasic.Controls.Add(txtTitle);
            pnlBasic.Controls.Add(lblTypeLbl);
            pnlBasic.Controls.Add(cmbType);
            pnlBasic.Controls.Add(lblDeadlineLbl);
            pnlBasic.Controls.Add(dtpDeadline);
            pnlBasic.Controls.Add(lblPointsLbl);
            pnlBasic.Controls.Add(nudPoints);
            pnlBasic.Controls.Add(lblPointsNote);
            pnlBasic.Controls.Add(lblDescLbl);
            pnlBasic.Controls.Add(txtDescription);
            pnlBasic.Location = new Point(0, 17);
            pnlBasic.Margin = new Padding(0, 0, 0, 14);
            pnlBasic.Name = "pnlBasic";
            pnlBasic.Padding = new Padding(18, 14, 18, 18);
            pnlBasic.Size = new Size(900, 272);
            pnlBasic.TabIndex = 1;

            lblSectionBasic.Location = new Point(0, 0);
            lblSectionBasic.Name = "lblSectionBasic";
            lblSectionBasic.Size = new Size(100, 23);
            lblSectionBasic.TabIndex = 0;

            lblTitleLbl.Location = new Point(0, 0);
            lblTitleLbl.Name = "lblTitleLbl";
            lblTitleLbl.Size = new Size(100, 23);
            lblTitleLbl.TabIndex = 1;

            txtTitle.Font = new Font("Segoe UI", 11F);
            txtTitle.Location = new Point(18, 60);
            txtTitle.Name = "txtTitle";
            txtTitle.PlaceholderText = "Enter activity title...";
            txtTitle.Size = new Size(860, 27);
            txtTitle.TabIndex = 2;

            lblTypeLbl.Location = new Point(0, 0);
            lblTypeLbl.Name = "lblTypeLbl";
            lblTypeLbl.Size = new Size(100, 23);
            lblTypeLbl.TabIndex = 3;

            cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbType.Font = new Font("Segoe UI", 10F);
            cmbType.FormattingEnabled = true;
            cmbType.Items.AddRange(new object[] { "Assignment", "Quiz", "Essay", "FileUpload" });
            cmbType.Location = new Point(18, 124);
            cmbType.Name = "cmbType";
            cmbType.Size = new Size(200, 25);
            cmbType.TabIndex = 3;
            cmbType.SelectedIndexChanged += cmbType_SelectedIndexChanged;

            lblDeadlineLbl.Location = new Point(0, 0);
            lblDeadlineLbl.Name = "lblDeadlineLbl";
            lblDeadlineLbl.Size = new Size(100, 23);
            lblDeadlineLbl.TabIndex = 4;

            dtpDeadline.CustomFormat = "MM/dd/yyyy  hh:mm tt";
            dtpDeadline.Font = new Font("Segoe UI", 10F);
            dtpDeadline.Format = DateTimePickerFormat.Custom;
            dtpDeadline.Location = new Point(238, 124);
            dtpDeadline.Name = "dtpDeadline";
            dtpDeadline.Size = new Size(320, 25);
            dtpDeadline.TabIndex = 4;
            dtpDeadline.Value = new DateTime(2026, 6, 5, 18, 58, 34, 979);

            lblPointsLbl.Location = new Point(0, 0);
            lblPointsLbl.Name = "lblPointsLbl";
            lblPointsLbl.Size = new Size(100, 23);
            lblPointsLbl.TabIndex = 5;

            nudPoints.Font = new Font("Segoe UI", 10F);
            nudPoints.Location = new Point(578, 124);
            nudPoints.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudPoints.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudPoints.Name = "nudPoints";
            nudPoints.Size = new Size(120, 25);
            nudPoints.TabIndex = 5;
            nudPoints.Value = new decimal(new int[] { 100, 0, 0, 0 });

            lblPointsNote.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblPointsNote.ForeColor = Color.DimGray;
            lblPointsNote.Location = new Point(578, 156);
            lblPointsNote.Name = "lblPointsNote";
            lblPointsNote.Size = new Size(300, 18);
            lblPointsNote.TabIndex = 6;

            lblDescLbl.Location = new Point(0, 0);
            lblDescLbl.Name = "lblDescLbl";
            lblDescLbl.Size = new Size(100, 23);
            lblDescLbl.TabIndex = 7;

            txtDescription.Font = new Font("Segoe UI", 10F);
            txtDescription.Location = new Point(18, 190);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.PlaceholderText = "Add instructions, objectives, or notes for students...";
            txtDescription.ScrollBars = ScrollBars.Vertical;
            txtDescription.Size = new Size(860, 68);
            txtDescription.TabIndex = 7;

            // ── pnlQuizSection ──────────────────────────────────────────────
            pnlQuizSection.AutoSize = true;
            pnlQuizSection.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlQuizSection.BackColor = Color.White;
            pnlQuizSection.Controls.Add(lblQuizHeader);
            pnlQuizSection.Controls.Add(btnAddQuestion);
            pnlQuizSection.Controls.Add(flpQuestions);
            pnlQuizSection.Location = new Point(0, 303);
            pnlQuizSection.Margin = new Padding(0, 0, 0, 14);
            pnlQuizSection.Name = "pnlQuizSection";
            pnlQuizSection.Padding = new Padding(18, 14, 18, 14);
            pnlQuizSection.Size = new Size(900, 67);
            pnlQuizSection.TabIndex = 2;
            pnlQuizSection.Visible = false;
            pnlQuizSection.SizeChanged += pnlQuizSection_SizeChanged;

            lblQuizHeader.AutoSize = true;
            lblQuizHeader.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblQuizHeader.ForeColor = Color.FromArgb(63, 81, 181);
            lblQuizHeader.Location = new Point(18, 14);
            lblQuizHeader.Name = "lblQuizHeader";
            lblQuizHeader.Size = new Size(126, 19);
            lblQuizHeader.TabIndex = 0;
            lblQuizHeader.Text = "❓ Quiz Questions";

            btnAddQuestion.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddQuestion.BackColor = Color.FromArgb(63, 81, 181);
            btnAddQuestion.FlatAppearance.BorderSize = 0;
            btnAddQuestion.FlatStyle = FlatStyle.Flat;
            btnAddQuestion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAddQuestion.ForeColor = Color.White;
            btnAddQuestion.Location = new Point(752, 12);
            btnAddQuestion.Name = "btnAddQuestion";
            btnAddQuestion.Size = new Size(130, 30);
            btnAddQuestion.TabIndex = 1;
            btnAddQuestion.Text = "+ Add Question";
            btnAddQuestion.UseVisualStyleBackColor = false;
            btnAddQuestion.Click += btnAddQuestion_Click;

            flpQuestions.AutoSize = true;
            flpQuestions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpQuestions.FlowDirection = FlowDirection.TopDown;
            flpQuestions.Location = new Point(18, 50);
            flpQuestions.Name = "flpQuestions";
            flpQuestions.Size = new Size(0, 0);
            flpQuestions.TabIndex = 2;
            flpQuestions.WrapContents = false;

            // ════════════════════════════════════════════════════════════════
            //  RUBRIC SECTION  –  outer wrapper
            // ════════════════════════════════════════════════════════════════
            pnlRubricSection.AutoSize = false;
            pnlRubricSection.BackColor = Color.White;
            pnlRubricSection.Controls.Add(pnlRubricLeft);
            pnlRubricSection.Controls.Add(pnlRubricSummary);
            pnlRubricSection.Location = new Point(0, 384);
            pnlRubricSection.Margin = new Padding(0, 0, 0, 14);
            pnlRubricSection.Name = "pnlRubricSection";
            pnlRubricSection.Size = new Size(1400, 46);
            pnlRubricSection.TabIndex = 3;
            pnlRubricSection.Visible = false;
            pnlRubricSection.SizeChanged += pnlRubricSection_SizeChanged;

            // ── LEFT COLUMN ─────────────────────────────────────────────────
            //    pnlRubricLeft
            //      └ pnlRubricHeader  (checkbox row + add-button – always 46px)
            //      └ pnlCriteriaScroll (scrollable list when expanded)
            //          └ pnlRubricRows → flpRubric
            // ────────────────────────────────────────────────────────────────
            pnlRubricLeft.Location = new Point(0, 0);
            pnlRubricLeft.Name = "pnlRubricLeft";
            pnlRubricLeft.BackColor = Color.White;
            pnlRubricLeft.Size = new Size(960, 46);
            pnlRubricLeft.Controls.Add(pnlRubricHeader);
            pnlRubricLeft.Controls.Add(pnlCriteriaScroll);
            pnlRubricLeft.TabIndex = 0;

            //  Header row inside left column
            pnlRubricHeader.Location = new Point(0, 0);
            pnlRubricHeader.Name = "pnlRubricHeader";
            pnlRubricHeader.BackColor = Color.White;
            pnlRubricHeader.Size = new Size(960, 46);
            pnlRubricHeader.Controls.Add(chkRubric);
            pnlRubricHeader.Controls.Add(lblRubricNote);
            pnlRubricHeader.Controls.Add(btnAddCriteria);
            pnlRubricHeader.TabIndex = 0;

            chkRubric.AutoSize = true;
            chkRubric.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            chkRubric.ForeColor = Color.FromArgb(128, 0, 0);
            chkRubric.Location = new Point(16, 11);
            chkRubric.Name = "chkRubric";
            chkRubric.TabIndex = 0;
            chkRubric.Text = "✎  Enable Rubric Grading";
            chkRubric.UseVisualStyleBackColor = true;
            chkRubric.CheckedChanged += chkRubric_CheckedChanged;

            lblRubricNote.AutoSize = true;
            lblRubricNote.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblRubricNote.ForeColor = Color.DimGray;
            lblRubricNote.Location = new Point(220, 15);
            lblRubricNote.Name = "lblRubricNote";
            lblRubricNote.TabIndex = 1;
            lblRubricNote.Text = "";
            lblRubricNote.Visible = false;

            btnAddCriteria.BackColor = Color.FromArgb(128, 0, 0);
            btnAddCriteria.FlatAppearance.BorderSize = 0;
            btnAddCriteria.FlatStyle = FlatStyle.Flat;
            btnAddCriteria.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAddCriteria.ForeColor = Color.White;
            btnAddCriteria.Location = new Point(820, 9);
            btnAddCriteria.Name = "btnAddCriteria";
            btnAddCriteria.Size = new Size(130, 28);
            btnAddCriteria.TabIndex = 2;
            btnAddCriteria.Text = "+ Add Criteria";
            btnAddCriteria.UseVisualStyleBackColor = false;
            btnAddCriteria.Visible = false;
            btnAddCriteria.Click += btnAddCriteria_Click;

            //  Scrollable criteria area
            pnlCriteriaScroll.Location = new Point(0, 46);
            pnlCriteriaScroll.Name = "pnlCriteriaScroll";
            pnlCriteriaScroll.BackColor = Color.FromArgb(248, 248, 251);
            pnlCriteriaScroll.AutoScroll = true;
            pnlCriteriaScroll.Size = new Size(960, 0);
            pnlCriteriaScroll.Controls.Add(pnlRubricRows);
            pnlCriteriaScroll.Visible = false;
            pnlCriteriaScroll.TabIndex = 1;

            pnlRubricRows.AutoSize = true;
            pnlRubricRows.Controls.Add(flpRubric);
            pnlRubricRows.Dock = DockStyle.Top;
            pnlRubricRows.Name = "pnlRubricRows";
            pnlRubricRows.BackColor = Color.FromArgb(248, 248, 251);
            pnlRubricRows.Size = new Size(960, 0);
            pnlRubricRows.TabIndex = 0;

            flpRubric.AutoSize = true;
            flpRubric.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpRubric.Dock = DockStyle.Top;
            flpRubric.FlowDirection = FlowDirection.TopDown;
            flpRubric.Name = "flpRubric";
            flpRubric.BackColor = Color.FromArgb(248, 248, 251);
            flpRubric.Padding = new Padding(8, 6, 8, 6);
            flpRubric.Size = new Size(960, 0);
            flpRubric.TabIndex = 0;
            flpRubric.WrapContents = false;

            // ── RIGHT COLUMN – SUMMARY PANEL ────────────────────────────────
            //    pnlRubricSummary
            //      ├ lblSummaryHeader
            //      ├ pnlSumDivider
            //      ├ pnlStatCards        (row 1: Criteria / Total Pts)
            //      ├ pnlStatCardsRow2    (row 2: Remaining / Status)
            //      ├ pnlProgressArea     (progress bar)
            //      ├ pnlGradeGuidelines  (tips)
            //      └ pnlValidation       (live validation message)
            // ────────────────────────────────────────────────────────────────
            pnlRubricSummary.Location = new Point(968, 0);
            pnlRubricSummary.Name = "pnlRubricSummary";
            pnlRubricSummary.BackColor = Color.FromArgb(250, 248, 255);
            pnlRubricSummary.Size = new Size(432, 46);
            pnlRubricSummary.Visible = false;
            pnlRubricSummary.TabIndex = 1;
            pnlRubricSummary.Controls.Add(lblSummaryHeader);
            pnlRubricSummary.Controls.Add(pnlSumDivider);
            pnlRubricSummary.Controls.Add(pnlStatCards);
            pnlRubricSummary.Controls.Add(pnlStatCardsRow2);
            pnlRubricSummary.Controls.Add(pnlProgressArea);
            pnlRubricSummary.Controls.Add(pnlGradeGuidelines);
            pnlRubricSummary.Controls.Add(pnlValidation);
            pnlRubricSummary.Paint += pnlRubricSummary_Paint;

            //  Summary header
            lblSummaryHeader.AutoSize = true;
            lblSummaryHeader.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblSummaryHeader.ForeColor = Color.FromArgb(128, 0, 0);
            lblSummaryHeader.Location = new Point(14, 12);
            lblSummaryHeader.Name = "lblSummaryHeader";
            lblSummaryHeader.TabIndex = 0;
            lblSummaryHeader.Text = "📊  Rubric Summary";

            pnlSumDivider.BackColor = Color.FromArgb(220, 210, 220);
            pnlSumDivider.Location = new Point(14, 34);
            pnlSumDivider.Name = "pnlSumDivider";
            pnlSumDivider.Size = new Size(404, 1);
            pnlSumDivider.TabIndex = 1;

            // ── Stat cards row 1 ────────────────────────────────────────────
            pnlStatCards.Location = new Point(14, 42);
            pnlStatCards.Name = "pnlStatCards";
            pnlStatCards.BackColor = Color.Transparent;
            pnlStatCards.Size = new Size(404, 72);
            pnlStatCards.TabIndex = 2;
            pnlStatCards.Controls.Add(pnlStatCriteria);
            pnlStatCards.Controls.Add(pnlStatTotalPts);

            //  Card: Criteria count
            pnlStatCriteria.Location = new Point(0, 0);
            pnlStatCriteria.Name = "pnlStatCriteria";
            pnlStatCriteria.BackColor = Color.White;
            pnlStatCriteria.Size = new Size(194, 68);
            pnlStatCriteria.TabIndex = 0;
            pnlStatCriteria.Paint += StatCard_Paint;
            pnlStatCriteria.Tag = "#4A4FB0";   // accent color hex for paint handler
            pnlStatCriteria.Controls.Add(lblStatCriteriaIcon);
            pnlStatCriteria.Controls.Add(lblStatCriteriaVal);
            pnlStatCriteria.Controls.Add(lblStatCriteriaLbl);

            lblStatCriteriaIcon.AutoSize = true;
            lblStatCriteriaIcon.Font = new Font("Segoe UI", 16F);
            lblStatCriteriaIcon.Location = new Point(12, 10);
            lblStatCriteriaIcon.Name = "lblStatCriteriaIcon";
            lblStatCriteriaIcon.Text = "📝";
            lblStatCriteriaIcon.TabIndex = 0;

            lblStatCriteriaVal.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblStatCriteriaVal.ForeColor = Color.FromArgb(63, 66, 160);
            lblStatCriteriaVal.Location = new Point(60, 6);
            lblStatCriteriaVal.Name = "lblStatCriteriaVal";
            lblStatCriteriaVal.Size = new Size(120, 38);
            lblStatCriteriaVal.TabIndex = 1;
            lblStatCriteriaVal.Text = "0";

            lblStatCriteriaLbl.Font = new Font("Segoe UI", 8F);
            lblStatCriteriaLbl.ForeColor = Color.FromArgb(110, 110, 120);
            lblStatCriteriaLbl.Location = new Point(60, 46);
            lblStatCriteriaLbl.Name = "lblStatCriteriaLbl";
            lblStatCriteriaLbl.Size = new Size(120, 15);
            lblStatCriteriaLbl.TabIndex = 2;
            lblStatCriteriaLbl.Text = "Total Criteria";

            //  Card: Total Points
            pnlStatTotalPts.Location = new Point(202, 0);
            pnlStatTotalPts.Name = "pnlStatTotalPts";
            pnlStatTotalPts.BackColor = Color.White;
            pnlStatTotalPts.Size = new Size(194, 68);
            pnlStatTotalPts.TabIndex = 1;
            pnlStatTotalPts.Paint += StatCard_Paint;
            pnlStatTotalPts.Tag = "#800000";
            pnlStatTotalPts.Controls.Add(lblStatTotalIcon);
            pnlStatTotalPts.Controls.Add(lblStatTotalVal);
            pnlStatTotalPts.Controls.Add(lblStatTotalLbl);

            lblStatTotalIcon.AutoSize = true;
            lblStatTotalIcon.Font = new Font("Segoe UI", 16F);
            lblStatTotalIcon.Location = new Point(12, 10);
            lblStatTotalIcon.Name = "lblStatTotalIcon";
            lblStatTotalIcon.Text = "🏆";
            lblStatTotalIcon.TabIndex = 0;

            lblStatTotalVal.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblStatTotalVal.ForeColor = Color.FromArgb(128, 0, 0);
            lblStatTotalVal.Location = new Point(60, 6);
            lblStatTotalVal.Name = "lblStatTotalVal";
            lblStatTotalVal.Size = new Size(120, 38);
            lblStatTotalVal.TabIndex = 1;
            lblStatTotalVal.Text = "0";

            lblStatTotalLbl.Font = new Font("Segoe UI", 8F);
            lblStatTotalLbl.ForeColor = Color.FromArgb(110, 110, 120);
            lblStatTotalLbl.Location = new Point(60, 46);
            lblStatTotalLbl.Name = "lblStatTotalLbl";
            lblStatTotalLbl.Size = new Size(120, 15);
            lblStatTotalLbl.TabIndex = 2;
            lblStatTotalLbl.Text = "Total Points";

            // ── Stat cards row 2 ────────────────────────────────────────────
            pnlStatCardsRow2.Location = new Point(14, 122);
            pnlStatCardsRow2.Name = "pnlStatCardsRow2";
            pnlStatCardsRow2.BackColor = Color.Transparent;
            pnlStatCardsRow2.Size = new Size(404, 72);
            pnlStatCardsRow2.TabIndex = 3;
            pnlStatCardsRow2.Controls.Add(pnlStatRemaining);
            pnlStatCardsRow2.Controls.Add(pnlStatStatus);

            //  Card: Remaining points
            pnlStatRemaining.Location = new Point(0, 0);
            pnlStatRemaining.Name = "pnlStatRemaining";
            pnlStatRemaining.BackColor = Color.White;
            pnlStatRemaining.Size = new Size(194, 68);
            pnlStatRemaining.TabIndex = 0;
            pnlStatRemaining.Paint += StatCard_Paint;
            pnlStatRemaining.Tag = "#1A8040";
            pnlStatRemaining.Controls.Add(lblStatRemainingIcon);
            pnlStatRemaining.Controls.Add(lblStatRemainingVal);
            pnlStatRemaining.Controls.Add(lblStatRemainingLbl);

            lblStatRemainingIcon.AutoSize = true;
            lblStatRemainingIcon.Font = new Font("Segoe UI", 16F);
            lblStatRemainingIcon.Location = new Point(12, 10);
            lblStatRemainingIcon.Name = "lblStatRemainingIcon";
            lblStatRemainingIcon.Text = "🎯";
            lblStatRemainingIcon.TabIndex = 0;

            lblStatRemainingVal.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblStatRemainingVal.ForeColor = Color.FromArgb(26, 128, 64);
            lblStatRemainingVal.Location = new Point(60, 6);
            lblStatRemainingVal.Name = "lblStatRemainingVal";
            lblStatRemainingVal.Size = new Size(120, 38);
            lblStatRemainingVal.TabIndex = 1;
            lblStatRemainingVal.Text = "100";

            lblStatRemainingLbl.Font = new Font("Segoe UI", 8F);
            lblStatRemainingLbl.ForeColor = Color.FromArgb(110, 110, 120);
            lblStatRemainingLbl.Location = new Point(60, 46);
            lblStatRemainingLbl.Name = "lblStatRemainingLbl";
            lblStatRemainingLbl.Size = new Size(120, 15);
            lblStatRemainingLbl.TabIndex = 2;
            lblStatRemainingLbl.Text = "Remaining Pts";

            //  Card: Rubric Status
            pnlStatStatus.Location = new Point(202, 0);
            pnlStatStatus.Name = "pnlStatStatus";
            pnlStatStatus.BackColor = Color.White;
            pnlStatStatus.Size = new Size(194, 68);
            pnlStatStatus.TabIndex = 1;
            pnlStatStatus.Paint += StatCard_Paint;
            pnlStatStatus.Tag = "#888888";
            pnlStatStatus.Controls.Add(lblStatStatusIcon);
            pnlStatStatus.Controls.Add(lblStatStatusVal);
            pnlStatStatus.Controls.Add(lblStatStatusLbl);

            lblStatStatusIcon.AutoSize = true;
            lblStatStatusIcon.Font = new Font("Segoe UI", 16F);
            lblStatStatusIcon.Location = new Point(12, 10);
            lblStatStatusIcon.Name = "lblStatStatusIcon";
            lblStatStatusIcon.Text = "⚙";
            lblStatStatusIcon.TabIndex = 0;

            lblStatStatusVal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblStatStatusVal.ForeColor = Color.Gray;
            lblStatStatusVal.Location = new Point(60, 10);
            lblStatStatusVal.Name = "lblStatStatusVal";
            lblStatStatusVal.Size = new Size(126, 30);
            lblStatStatusVal.TabIndex = 1;
            lblStatStatusVal.Text = "Not\nConfigured";

            lblStatStatusLbl.Font = new Font("Segoe UI", 8F);
            lblStatStatusLbl.ForeColor = Color.FromArgb(110, 110, 120);
            lblStatStatusLbl.Location = new Point(60, 46);
            lblStatStatusLbl.Name = "lblStatStatusLbl";
            lblStatStatusLbl.Size = new Size(126, 15);
            lblStatStatusLbl.TabIndex = 2;
            lblStatStatusLbl.Text = "Grading Status";

            // ── Progress bar ────────────────────────────────────────────────
            pnlProgressArea.Location = new Point(14, 202);
            pnlProgressArea.Name = "pnlProgressArea";
            pnlProgressArea.BackColor = Color.Transparent;
            pnlProgressArea.Size = new Size(404, 38);
            pnlProgressArea.TabIndex = 4;
            pnlProgressArea.Controls.Add(lblProgressLbl);
            pnlProgressArea.Controls.Add(pnlProgressBg);
            pnlProgressArea.Controls.Add(lblProgressPct);

            lblProgressLbl.AutoSize = true;
            lblProgressLbl.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblProgressLbl.ForeColor = Color.FromArgb(80, 80, 90);
            lblProgressLbl.Location = new Point(0, 2);
            lblProgressLbl.Name = "lblProgressLbl";
            lblProgressLbl.Text = "Point Allocation";
            lblProgressLbl.TabIndex = 0;

            pnlProgressBg.Location = new Point(0, 20);
            pnlProgressBg.Name = "pnlProgressBg";
            pnlProgressBg.BackColor = Color.FromArgb(225, 220, 230);
            pnlProgressBg.Size = new Size(360, 10);
            pnlProgressBg.TabIndex = 1;
            pnlProgressBg.Controls.Add(pnlProgressFill);

            pnlProgressFill.Dock = DockStyle.Left;
            pnlProgressFill.Location = new Point(0, 0);
            pnlProgressFill.Name = "pnlProgressFill";
            pnlProgressFill.BackColor = Color.FromArgb(128, 0, 0);
            pnlProgressFill.Size = new Size(0, 10);
            pnlProgressFill.TabIndex = 0;

            lblProgressPct.AutoSize = true;
            lblProgressPct.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblProgressPct.ForeColor = Color.FromArgb(128, 0, 0);
            lblProgressPct.Location = new Point(366, 18);
            lblProgressPct.Name = "lblProgressPct";
            lblProgressPct.Text = "0%";
            lblProgressPct.TabIndex = 2;

            // ── Grading Guidelines ──────────────────────────────────────────
            pnlGradeGuidelines.Location = new Point(14, 248);
            pnlGradeGuidelines.Name = "pnlGradeGuidelines";
            pnlGradeGuidelines.BackColor = Color.FromArgb(242, 242, 252);
            pnlGradeGuidelines.Size = new Size(404, 86);
            pnlGradeGuidelines.TabIndex = 5;
            pnlGradeGuidelines.Controls.Add(lblGuidelinesHeader);
            pnlGradeGuidelines.Controls.Add(lblGuidelineText);

            lblGuidelinesHeader.AutoSize = true;
            lblGuidelinesHeader.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblGuidelinesHeader.ForeColor = Color.FromArgb(63, 66, 160);
            lblGuidelinesHeader.Location = new Point(10, 8);
            lblGuidelinesHeader.Name = "lblGuidelinesHeader";
            lblGuidelinesHeader.TabIndex = 0;
            lblGuidelinesHeader.Text = "💡  Guidelines";

            lblGuidelineText.Font = new Font("Segoe UI", 7.5F);
            lblGuidelineText.ForeColor = Color.FromArgb(70, 70, 80);
            lblGuidelineText.Location = new Point(10, 26);
            lblGuidelineText.Name = "lblGuidelineText";
            lblGuidelineText.Size = new Size(384, 54);
            lblGuidelineText.TabIndex = 1;
            lblGuidelineText.Text =
                "• Each criterion adds to the total rubric score.\r\n" +
                "• Rubric total becomes the activity max score.\r\n" +
                "• Name each criterion clearly for student clarity.\r\n" +
                "• Add at least one criterion to enable grading.";

            // ── Validation message ──────────────────────────────────────────
            pnlValidation.Location = new Point(14, 342);
            pnlValidation.Name = "pnlValidation";
            pnlValidation.BackColor = Color.Transparent;
            pnlValidation.Size = new Size(404, 22);
            pnlValidation.TabIndex = 6;
            pnlValidation.Controls.Add(lblValidationMsg);

            lblValidationMsg.AutoSize = true;
            lblValidationMsg.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblValidationMsg.ForeColor = Color.FromArgb(180, 100, 0);
            lblValidationMsg.Location = new Point(0, 3);
            lblValidationMsg.Name = "lblValidationMsg";
            lblValidationMsg.Text = "";
            lblValidationMsg.TabIndex = 0;

            // ── pnlFilesSection ─────────────────────────────────────────────
            pnlFilesSection.BackColor = Color.White;
            pnlFilesSection.Controls.Add(lblFilesHeader);
            pnlFilesSection.Controls.Add(btnAttachFile);
            pnlFilesSection.Controls.Add(lblNoFiles);
            pnlFilesSection.Controls.Add(flpFiles);
            pnlFilesSection.Location = new Point(0, 477);
            pnlFilesSection.Margin = new Padding(0, 0, 0, 14);
            pnlFilesSection.Name = "pnlFilesSection";
            pnlFilesSection.Padding = new Padding(18, 14, 18, 14);
            pnlFilesSection.Size = new Size(900, 140);
            pnlFilesSection.TabIndex = 4;
            pnlFilesSection.SizeChanged += pnlFilesSection_SizeChanged;

            lblFilesHeader.AutoSize = true;
            lblFilesHeader.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblFilesHeader.ForeColor = Color.DarkCyan;
            lblFilesHeader.Location = new Point(18, 14);
            lblFilesHeader.Name = "lblFilesHeader";
            lblFilesHeader.Size = new Size(116, 19);
            lblFilesHeader.TabIndex = 0;
            lblFilesHeader.Text = "📎 Attached Files";

            btnAttachFile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAttachFile.BackColor = Color.DarkCyan;
            btnAttachFile.FlatAppearance.BorderSize = 0;
            btnAttachFile.FlatStyle = FlatStyle.Flat;
            btnAttachFile.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAttachFile.ForeColor = Color.White;
            btnAttachFile.Location = new Point(762, 12);
            btnAttachFile.Name = "btnAttachFile";
            btnAttachFile.Size = new Size(120, 30);
            btnAttachFile.TabIndex = 1;
            btnAttachFile.Text = "+ Attach Files";
            btnAttachFile.UseVisualStyleBackColor = false;
            btnAttachFile.Click += btnAttachFile_Click;

            lblNoFiles.AutoSize = true;
            lblNoFiles.Font = new Font("Segoe UI", 9.5F, FontStyle.Italic);
            lblNoFiles.ForeColor = Color.Gray;
            lblNoFiles.Location = new Point(18, 50);
            lblNoFiles.Name = "lblNoFiles";
            lblNoFiles.Size = new Size(288, 17);
            lblNoFiles.TabIndex = 2;
            lblNoFiles.Text = "No files attached yet. Click \"+ Attach Files\" to add.";

            flpFiles.Location = new Point(18, 50);
            flpFiles.Name = "flpFiles";
            flpFiles.Size = new Size(860, 78);
            flpFiles.TabIndex = 3;

            // ── ActivityFormPage ────────────────────────────────────────────
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlScroll);
            Controls.Add(pnlHeader);
            Name = "ActivityFormPage";
            Size = new Size(1680, 989);

            pnlHeader.ResumeLayout(false);
            pnlScroll.ResumeLayout(false);
            pnlScroll.PerformLayout();
            stackPanel.ResumeLayout(false);
            stackPanel.PerformLayout();
            pnlBasic.ResumeLayout(false);
            pnlBasic.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudPoints).EndInit();
            pnlQuizSection.ResumeLayout(false);
            pnlQuizSection.PerformLayout();
            pnlRubricSection.ResumeLayout(false);
            pnlRubricLeft.ResumeLayout(false);
            pnlRubricHeader.ResumeLayout(false);
            pnlRubricHeader.PerformLayout();
            pnlCriteriaScroll.ResumeLayout(false);
            pnlRubricRows.ResumeLayout(false);
            pnlRubricRows.PerformLayout();
            pnlRubricSummary.ResumeLayout(false);
            pnlRubricSummary.PerformLayout();
            pnlStatCards.ResumeLayout(false);
            pnlStatCriteria.ResumeLayout(false);
            pnlStatCriteria.PerformLayout();
            pnlStatTotalPts.ResumeLayout(false);
            pnlStatTotalPts.PerformLayout();
            pnlStatCardsRow2.ResumeLayout(false);
            pnlStatRemaining.ResumeLayout(false);
            pnlStatRemaining.PerformLayout();
            pnlStatStatus.ResumeLayout(false);
            pnlStatStatus.PerformLayout();
            pnlProgressArea.ResumeLayout(false);
            pnlProgressArea.PerformLayout();
            pnlProgressBg.ResumeLayout(false);
            pnlGradeGuidelines.ResumeLayout(false);
            pnlGradeGuidelines.PerformLayout();
            pnlValidation.ResumeLayout(false);
            pnlValidation.PerformLayout();
            pnlFilesSection.ResumeLayout(false);
            pnlFilesSection.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        // ── Field declarations ──────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlHeader;
        private buttonRounded btnCancel;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Label lblCourseSub;
        private buttonRounded btnSave;
        private System.Windows.Forms.Panel pnlScroll;
        private System.Windows.Forms.FlowLayoutPanel stackPanel;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Panel pnlBasic;
        private System.Windows.Forms.Label lblSectionBasic;
        private System.Windows.Forms.Label lblTitleLbl;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblTypeLbl;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label lblDeadlineLbl;
        private System.Windows.Forms.DateTimePicker dtpDeadline;
        private System.Windows.Forms.Label lblPointsLbl;
        private System.Windows.Forms.NumericUpDown nudPoints;
        private System.Windows.Forms.Label lblPointsNote;
        private System.Windows.Forms.Label lblDescLbl;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Panel pnlQuizSection;
        private System.Windows.Forms.Label lblQuizHeader;
        private buttonRounded btnAddQuestion;
        private System.Windows.Forms.FlowLayoutPanel flpQuestions;
        // Rubric outer
        private System.Windows.Forms.Panel pnlRubricSection;
        // Left column
        private System.Windows.Forms.Panel pnlRubricLeft;
        private System.Windows.Forms.Panel pnlRubricHeader;
        private System.Windows.Forms.CheckBox chkRubric;
        private System.Windows.Forms.Label lblRubricNote;
        private buttonRounded btnAddCriteria;
        private System.Windows.Forms.Panel pnlCriteriaScroll;
        private System.Windows.Forms.Panel pnlRubricRows;
        private System.Windows.Forms.FlowLayoutPanel flpRubric;
        // Right column – summary
        private System.Windows.Forms.Panel pnlRubricSummary;
        private System.Windows.Forms.Label lblSummaryHeader;
        private System.Windows.Forms.Panel pnlSumDivider;
        private System.Windows.Forms.Panel pnlStatCards;
        private System.Windows.Forms.Panel pnlStatCriteria;
        private System.Windows.Forms.Label lblStatCriteriaIcon;
        private System.Windows.Forms.Label lblStatCriteriaVal;
        private System.Windows.Forms.Label lblStatCriteriaLbl;
        private System.Windows.Forms.Panel pnlStatTotalPts;
        private System.Windows.Forms.Label lblStatTotalIcon;
        private System.Windows.Forms.Label lblStatTotalVal;
        private System.Windows.Forms.Label lblStatTotalLbl;
        private System.Windows.Forms.Panel pnlStatCardsRow2;
        private System.Windows.Forms.Panel pnlStatRemaining;
        private System.Windows.Forms.Label lblStatRemainingIcon;
        private System.Windows.Forms.Label lblStatRemainingVal;
        private System.Windows.Forms.Label lblStatRemainingLbl;
        private System.Windows.Forms.Panel pnlStatStatus;
        private System.Windows.Forms.Label lblStatStatusIcon;
        private System.Windows.Forms.Label lblStatStatusVal;
        private System.Windows.Forms.Label lblStatStatusLbl;
        private System.Windows.Forms.Panel pnlProgressArea;
        private System.Windows.Forms.Label lblProgressLbl;
        private System.Windows.Forms.Panel pnlProgressBg;
        private System.Windows.Forms.Panel pnlProgressFill;
        private System.Windows.Forms.Label lblProgressPct;
        private System.Windows.Forms.Panel pnlGradeGuidelines;
        private System.Windows.Forms.Label lblGuidelinesHeader;
        private System.Windows.Forms.Label lblGuidelineText;
        private System.Windows.Forms.Panel pnlValidation;
        private System.Windows.Forms.Label lblValidationMsg;
        private System.Windows.Forms.Panel pnlFilesSection;
        private System.Windows.Forms.Label lblFilesHeader;
        private buttonRounded btnAttachFile;
        private System.Windows.Forms.FlowLayoutPanel flpFiles;
        private System.Windows.Forms.Label lblNoFiles;
    }
}