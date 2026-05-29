namespace PUPAcadPortal
{
    partial class ActivityFormPage
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlHeader = new System.Windows.Forms.Panel();
            btnCancel = new buttonRounded();
            lblPageTitle = new System.Windows.Forms.Label();
            lblCourseSub = new System.Windows.Forms.Label();
            btnSave = new buttonRounded();
            pnlScroll = new System.Windows.Forms.Panel();
            stackPanel = new System.Windows.Forms.FlowLayoutPanel();
            lblError = new System.Windows.Forms.Label();
            pnlBasic = new System.Windows.Forms.Panel();
            lblSectionBasic = new System.Windows.Forms.Label();
            lblTitleLbl = new System.Windows.Forms.Label();
            txtTitle = new System.Windows.Forms.TextBox();
            lblTypeLbl = new System.Windows.Forms.Label();
            cmbType = new System.Windows.Forms.ComboBox();
            lblDeadlineLbl = new System.Windows.Forms.Label();
            dtpDeadline = new System.Windows.Forms.DateTimePicker();
            lblPointsLbl = new System.Windows.Forms.Label();
            nudPoints = new System.Windows.Forms.NumericUpDown();
            lblPointsNote = new System.Windows.Forms.Label();
            lblDescLbl = new System.Windows.Forms.Label();
            txtDescription = new System.Windows.Forms.TextBox();
            pnlQuizSection = new System.Windows.Forms.Panel();
            lblQuizHeader = new System.Windows.Forms.Label();
            btnAddQuestion = new buttonRounded();
            flpQuestions = new System.Windows.Forms.FlowLayoutPanel();
            pnlRubricSection = new System.Windows.Forms.Panel();
            chkRubric = new System.Windows.Forms.CheckBox();
            lblRubricNote = new System.Windows.Forms.Label();
            btnAddCriteria = new buttonRounded();
            pnlRubricRows = new System.Windows.Forms.Panel();
            flpRubric = new System.Windows.Forms.FlowLayoutPanel();
            pnlFilesSection = new System.Windows.Forms.Panel();
            lblFilesHeader = new System.Windows.Forms.Label();
            btnAttachFile = new buttonRounded();
            lblNoFiles = new System.Windows.Forms.Label();
            flpFiles = new System.Windows.Forms.FlowLayoutPanel();

            pnlHeader.SuspendLayout();
            pnlScroll.SuspendLayout();
            stackPanel.SuspendLayout();
            pnlBasic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPoints).BeginInit();
            pnlQuizSection.SuspendLayout();
            pnlRubricSection.SuspendLayout();
            pnlRubricRows.SuspendLayout();
            pnlFilesSection.SuspendLayout();
            SuspendLayout();

            // ── pnlHeader ─────────────────────────────────────────────────────
            pnlHeader.BackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            pnlHeader.Controls.Add(btnCancel);
            pnlHeader.Controls.Add(lblPageTitle);
            pnlHeader.Controls.Add(lblCourseSub);
            pnlHeader.Controls.Add(btnSave);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(1200, 68);
            pnlHeader.TabIndex = 0;
            pnlHeader.SizeChanged += pnlHeader_SizeChanged;

            btnCancel.BackColor = System.Drawing.Color.FromArgb(100, 0, 0);
            btnCancel.BorderRadius = 8;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnCancel.ForeColor = System.Drawing.Color.White;
            btnCancel.Location = new System.Drawing.Point(12, 18);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(82, 32);
            btnCancel.TabIndex = 0;
            btnCancel.Text = "← Back";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;

            lblPageTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            lblPageTitle.ForeColor = System.Drawing.Color.White;
            lblPageTitle.Location = new System.Drawing.Point(108, 10);
            lblPageTitle.Name = "lblPageTitle";
            lblPageTitle.Size = new System.Drawing.Size(500, 28);
            lblPageTitle.TabIndex = 1;
            lblPageTitle.Text = "Create Activity";

            lblCourseSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblCourseSub.ForeColor = System.Drawing.Color.FromArgb(230, 185, 185);
            lblCourseSub.Location = new System.Drawing.Point(108, 42);
            lblCourseSub.Name = "lblCourseSub";
            lblCourseSub.Size = new System.Drawing.Size(500, 18);
            lblCourseSub.TabIndex = 2;

            btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnSave.BackColor = System.Drawing.Color.FromArgb(255, 196, 0);
            btnSave.BorderRadius = 10;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSave.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnSave.ForeColor = System.Drawing.Color.Black;
            btnSave.Location = new System.Drawing.Point(1030, 17);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(160, 34);
            btnSave.TabIndex = 3;
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;

            // ── pnlScroll ─────────────────────────────────────────────────────
            pnlScroll.AutoScroll = true;
            pnlScroll.BackColor = System.Drawing.Color.FromArgb(245, 245, 248);
            pnlScroll.Controls.Add(stackPanel);
            pnlScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlScroll.Name = "pnlScroll";
            pnlScroll.Padding = new System.Windows.Forms.Padding(22, 18, 22, 18);
            pnlScroll.TabIndex = 1;
            pnlScroll.SizeChanged += pnlScroll_SizeChanged;

            // ── stackPanel ────────────────────────────────────────────────────
            stackPanel.AutoSize = true;
            stackPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            stackPanel.Controls.Add(lblError);
            stackPanel.Controls.Add(pnlBasic);
            stackPanel.Controls.Add(pnlQuizSection);
            stackPanel.Controls.Add(pnlRubricSection);
            stackPanel.Controls.Add(pnlFilesSection);
            stackPanel.Dock = System.Windows.Forms.DockStyle.Top;
            stackPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            stackPanel.Name = "stackPanel";
            stackPanel.WrapContents = false;
            stackPanel.TabIndex = 0;

            // ── lblError ──────────────────────────────────────────────────────
            lblError.AutoSize = true;
            lblError.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            lblError.ForeColor = System.Drawing.Color.Red;
            lblError.Name = "lblError";
            lblError.TabIndex = 0;

            // ── pnlBasic ──────────────────────────────────────────────────────
            pnlBasic.BackColor = System.Drawing.Color.White;
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
            pnlBasic.Margin = new System.Windows.Forms.Padding(0, 0, 0, 14);
            pnlBasic.Name = "pnlBasic";
            pnlBasic.Padding = new System.Windows.Forms.Padding(18, 14, 18, 18);
            pnlBasic.Size = new System.Drawing.Size(900, 272);
            pnlBasic.TabIndex = 1;

            SetupSectionLabel(lblSectionBasic, "Activity Details", 18, 14, System.Drawing.Color.FromArgb(128, 0, 0));

            SetupFieldLabel(lblTitleLbl, "Activity Title *", 18, 40);
            txtTitle.Font = new System.Drawing.Font("Segoe UI", 11F);
            txtTitle.Location = new System.Drawing.Point(18, 60);
            txtTitle.Name = "txtTitle";
            txtTitle.PlaceholderText = "Enter activity title...";
            txtTitle.Size = new System.Drawing.Size(860, 28);
            txtTitle.TabIndex = 2;

            SetupFieldLabel(lblTypeLbl, "Activity Type *", 18, 104);
            cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbType.Font = new System.Drawing.Font("Segoe UI", 10F);
            cmbType.Items.AddRange(new object[] { "Assignment", "Quiz", "Essay", "FileUpload" });
            cmbType.Location = new System.Drawing.Point(18, 124);
            cmbType.Name = "cmbType";
            cmbType.Size = new System.Drawing.Size(200, 26);
            cmbType.TabIndex = 3;
            cmbType.SelectedIndex = 0;
            cmbType.SelectedIndexChanged += cmbType_SelectedIndexChanged;

            SetupFieldLabel(lblDeadlineLbl, "Deadline *", 238, 104);
            dtpDeadline.CustomFormat = "MM/dd/yyyy  hh:mm tt";
            dtpDeadline.Font = new System.Drawing.Font("Segoe UI", 10F);
            dtpDeadline.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dtpDeadline.Location = new System.Drawing.Point(238, 124);
            dtpDeadline.Name = "dtpDeadline";
            dtpDeadline.Size = new System.Drawing.Size(320, 26);
            dtpDeadline.TabIndex = 4;
            dtpDeadline.Value = System.DateTime.Now.AddDays(7);

            SetupFieldLabel(lblPointsLbl, "Max Points *", 578, 104);
            nudPoints.Font = new System.Drawing.Font("Segoe UI", 10F);
            nudPoints.Location = new System.Drawing.Point(578, 124);
            nudPoints.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudPoints.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudPoints.Name = "nudPoints";
            nudPoints.Size = new System.Drawing.Size(120, 26);
            nudPoints.TabIndex = 5;
            nudPoints.Value = new decimal(new int[] { 100, 0, 0, 0 });

            lblPointsNote.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            lblPointsNote.ForeColor = System.Drawing.Color.DimGray;
            lblPointsNote.Location = new System.Drawing.Point(578, 156);
            lblPointsNote.Name = "lblPointsNote";
            lblPointsNote.Size = new System.Drawing.Size(300, 18);
            lblPointsNote.TabIndex = 6;

            SetupFieldLabel(lblDescLbl, "Instructions / Description", 18, 170);
            txtDescription.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtDescription.Location = new System.Drawing.Point(18, 190);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.PlaceholderText = "Add instructions, objectives, or notes for students...";
            txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtDescription.Size = new System.Drawing.Size(860, 68);
            txtDescription.TabIndex = 7;

            // ── pnlQuizSection ────────────────────────────────────────────────
            pnlQuizSection.AutoSize = true;
            pnlQuizSection.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            pnlQuizSection.BackColor = System.Drawing.Color.White;
            pnlQuizSection.Controls.Add(lblQuizHeader);
            pnlQuizSection.Controls.Add(btnAddQuestion);
            pnlQuizSection.Controls.Add(flpQuestions);
            pnlQuizSection.Margin = new System.Windows.Forms.Padding(0, 0, 0, 14);
            pnlQuizSection.Name = "pnlQuizSection";
            pnlQuizSection.Padding = new System.Windows.Forms.Padding(18, 14, 18, 14);
            pnlQuizSection.Size = new System.Drawing.Size(900, 68);
            pnlQuizSection.TabIndex = 2;
            pnlQuizSection.Visible = false;
            pnlQuizSection.SizeChanged += pnlQuizSection_SizeChanged;

            lblQuizHeader.AutoSize = true;
            lblQuizHeader.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            lblQuizHeader.ForeColor = System.Drawing.Color.FromArgb(63, 81, 181);
            lblQuizHeader.Location = new System.Drawing.Point(18, 14);
            lblQuizHeader.Name = "lblQuizHeader";
            lblQuizHeader.TabIndex = 0;
            lblQuizHeader.Text = "❓ Quiz Questions";

            btnAddQuestion.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnAddQuestion.BackColor = System.Drawing.Color.FromArgb(63, 81, 181);
            btnAddQuestion.BorderRadius = 8;
            btnAddQuestion.FlatAppearance.BorderSize = 0;
            btnAddQuestion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAddQuestion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnAddQuestion.ForeColor = System.Drawing.Color.White;
            btnAddQuestion.Location = new System.Drawing.Point(752, 12);
            btnAddQuestion.Name = "btnAddQuestion";
            btnAddQuestion.Size = new System.Drawing.Size(130, 30);
            btnAddQuestion.TabIndex = 1;
            btnAddQuestion.Text = "+ Add Question";
            btnAddQuestion.UseVisualStyleBackColor = false;
            btnAddQuestion.Click += btnAddQuestion_Click;

            flpQuestions.AutoSize = true;
            flpQuestions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            flpQuestions.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flpQuestions.Location = new System.Drawing.Point(18, 50);
            flpQuestions.Name = "flpQuestions";
            flpQuestions.WrapContents = false;
            flpQuestions.TabIndex = 2;

            // ── pnlRubricSection ──────────────────────────────────────────────
            pnlRubricSection.AutoSize = true;
            pnlRubricSection.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            pnlRubricSection.BackColor = System.Drawing.Color.White;
            pnlRubricSection.Controls.Add(chkRubric);
            pnlRubricSection.Controls.Add(lblRubricNote);
            pnlRubricSection.Controls.Add(btnAddCriteria);
            pnlRubricSection.Controls.Add(pnlRubricRows);
            pnlRubricSection.Margin = new System.Windows.Forms.Padding(0, 0, 0, 14);
            pnlRubricSection.Name = "pnlRubricSection";
            pnlRubricSection.Padding = new System.Windows.Forms.Padding(18, 14, 18, 14);
            pnlRubricSection.Size = new System.Drawing.Size(900, 78);
            pnlRubricSection.TabIndex = 3;
            pnlRubricSection.Visible = false;
            pnlRubricSection.SizeChanged += pnlRubricSection_SizeChanged;

            chkRubric.AutoSize = true;
            chkRubric.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            chkRubric.ForeColor = System.Drawing.Color.DarkSlateBlue;
            chkRubric.Location = new System.Drawing.Point(18, 14);
            chkRubric.Name = "chkRubric";
            chkRubric.TabIndex = 0;
            chkRubric.Text = "Enable Rubric Grading";
            chkRubric.CheckedChanged += chkRubric_CheckedChanged;

            lblRubricNote.AutoSize = true;
            lblRubricNote.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            lblRubricNote.ForeColor = System.Drawing.Color.DimGray;
            lblRubricNote.Location = new System.Drawing.Point(18, 44);
            lblRubricNote.Name = "lblRubricNote";
            lblRubricNote.TabIndex = 1;
            lblRubricNote.Text = "Define criteria and points below.";
            lblRubricNote.Visible = false;

            btnAddCriteria.BackColor = System.Drawing.Color.DarkSlateBlue;
            btnAddCriteria.BorderRadius = 8;
            btnAddCriteria.FlatAppearance.BorderSize = 0;
            btnAddCriteria.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAddCriteria.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnAddCriteria.ForeColor = System.Drawing.Color.White;
            btnAddCriteria.Location = new System.Drawing.Point(757, 12);
            btnAddCriteria.Name = "btnAddCriteria";
            btnAddCriteria.Size = new System.Drawing.Size(125, 30);
            btnAddCriteria.TabIndex = 2;
            btnAddCriteria.Text = "+ Add Criteria";
            btnAddCriteria.UseVisualStyleBackColor = false;
            btnAddCriteria.Click += btnAddCriteria_Click;

            pnlRubricRows.AutoSize = true;
            pnlRubricRows.Controls.Add(flpRubric);
            pnlRubricRows.Location = new System.Drawing.Point(18, 62);
            pnlRubricRows.Name = "pnlRubricRows";
            pnlRubricRows.Size = new System.Drawing.Size(860, 0);
            pnlRubricRows.TabIndex = 3;
            pnlRubricRows.Visible = false;

            flpRubric.AutoSize = true;
            flpRubric.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            flpRubric.Dock = System.Windows.Forms.DockStyle.Top;
            flpRubric.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flpRubric.Name = "flpRubric";
            flpRubric.WrapContents = false;
            flpRubric.TabIndex = 0;

            // ── pnlFilesSection ───────────────────────────────────────────────
            pnlFilesSection.BackColor = System.Drawing.Color.White;
            pnlFilesSection.Controls.Add(lblFilesHeader);
            pnlFilesSection.Controls.Add(btnAttachFile);
            pnlFilesSection.Controls.Add(lblNoFiles);
            pnlFilesSection.Controls.Add(flpFiles);
            pnlFilesSection.Margin = new System.Windows.Forms.Padding(0, 0, 0, 14);
            pnlFilesSection.Name = "pnlFilesSection";
            pnlFilesSection.Padding = new System.Windows.Forms.Padding(18, 14, 18, 14);
            pnlFilesSection.Size = new System.Drawing.Size(900, 140);
            pnlFilesSection.TabIndex = 4;
            pnlFilesSection.SizeChanged += pnlFilesSection_SizeChanged;

            lblFilesHeader.AutoSize = true;
            lblFilesHeader.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            lblFilesHeader.ForeColor = System.Drawing.Color.DarkCyan;
            lblFilesHeader.Location = new System.Drawing.Point(18, 14);
            lblFilesHeader.Name = "lblFilesHeader";
            lblFilesHeader.TabIndex = 0;
            lblFilesHeader.Text = "📎 Attached Files";

            btnAttachFile.BackColor = System.Drawing.Color.DarkCyan;
            btnAttachFile.BorderRadius = 8;
            btnAttachFile.FlatAppearance.BorderSize = 0;
            btnAttachFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAttachFile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnAttachFile.ForeColor = System.Drawing.Color.White;
            btnAttachFile.Location = new System.Drawing.Point(762, 12);
            btnAttachFile.Name = "btnAttachFile";
            btnAttachFile.Size = new System.Drawing.Size(120, 30);
            btnAttachFile.TabIndex = 1;
            btnAttachFile.Text = "+ Attach Files";
            btnAttachFile.UseVisualStyleBackColor = false;
            btnAttachFile.Click += btnAttachFile_Click;

            lblNoFiles.AutoSize = true;
            lblNoFiles.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Italic);
            lblNoFiles.ForeColor = System.Drawing.Color.Gray;
            lblNoFiles.Location = new System.Drawing.Point(18, 50);
            lblNoFiles.Name = "lblNoFiles";
            lblNoFiles.TabIndex = 2;
            lblNoFiles.Text = "No files attached yet. Click \"+ Attach Files\" to add.";

            flpFiles.Location = new System.Drawing.Point(18, 50);
            flpFiles.Name = "flpFiles";
            flpFiles.Size = new System.Drawing.Size(860, 78);
            flpFiles.TabIndex = 3;

            // ── ActivityFormPage ──────────────────────────────────────────────
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(pnlScroll);
            Controls.Add(pnlHeader);
            Name = "ActivityFormPage";
            Size = new System.Drawing.Size(1200, 900);

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
            pnlRubricSection.PerformLayout();
            pnlRubricRows.ResumeLayout(false);
            pnlRubricRows.PerformLayout();
            pnlFilesSection.ResumeLayout(false);
            pnlFilesSection.PerformLayout();
            ResumeLayout(false);
        }

        // ── Designer-side responsive helpers ──────────────────────────────────
        private void pnlHeader_SizeChanged(object sender, System.EventArgs e)
        {
            if (btnSave != null && pnlHeader != null)
                btnSave.Location = new System.Drawing.Point(pnlHeader.Width - btnSave.Width - 12, 17);
        }

        private void pnlQuizSection_SizeChanged(object sender, System.EventArgs e)
        {
            if (btnAddQuestion != null && pnlQuizSection != null)
                btnAddQuestion.Location = new System.Drawing.Point(pnlQuizSection.Width - 148, 12);
        }

        private void pnlRubricSection_SizeChanged(object sender, System.EventArgs e)
        {
            if (btnAddCriteria != null && pnlRubricSection != null)
                btnAddCriteria.Location = new System.Drawing.Point(pnlRubricSection.Width - 143, 12);
        }

        private void pnlFilesSection_SizeChanged(object sender, System.EventArgs e)
        {
            if (btnAttachFile != null && pnlFilesSection != null)
                btnAttachFile.Location = new System.Drawing.Point(pnlFilesSection.Width - 138, 12);
        }

        private void pnlScroll_SizeChanged(object sender, System.EventArgs e)
        {
            if (pnlScroll == null || stackPanel == null) return;
            int w = System.Math.Max(600, pnlScroll.ClientSize.Width - 44);
            foreach (System.Windows.Forms.Control c in stackPanel.Controls)
            {
                c.Width = w;
                if (c is System.Windows.Forms.Panel p)
                {
                    foreach (System.Windows.Forms.Control inner in p.Controls)
                    {
                        if (inner is System.Windows.Forms.TextBox tb &&
                            (tb.Name == "txtTitle" || tb.Name == "txtDescription"))
                            tb.Width = w - 36;
                    }
                }
            }
        }

        // ── Static designer helpers ───────────────────────────────────────────
        private static void SetupSectionLabel(System.Windows.Forms.Label lbl, string text, int x, int y, System.Drawing.Color color)
        {
            lbl.AutoSize = true;
            lbl.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            lbl.ForeColor = color;
            lbl.Location = new System.Drawing.Point(x, y);
            lbl.Text = text;
            lbl.TabIndex = 0;
        }

        private static void SetupFieldLabel(System.Windows.Forms.Label lbl, string text, int x, int y)
        {
            lbl.AutoSize = true;
            lbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            lbl.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            lbl.Location = new System.Drawing.Point(x, y);
            lbl.Text = text;
        }

        // ── Field declarations ─────────────────────────────────────────────────
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
        private System.Windows.Forms.Panel pnlRubricSection;
        private System.Windows.Forms.CheckBox chkRubric;
        private System.Windows.Forms.Label lblRubricNote;
        private buttonRounded btnAddCriteria;
        private System.Windows.Forms.Panel pnlRubricRows;
        private System.Windows.Forms.FlowLayoutPanel flpRubric;
        private System.Windows.Forms.Panel pnlFilesSection;
        private System.Windows.Forms.Label lblFilesHeader;
        private buttonRounded btnAttachFile;
        private System.Windows.Forms.FlowLayoutPanel flpFiles;
        private System.Windows.Forms.Label lblNoFiles;
    }
}