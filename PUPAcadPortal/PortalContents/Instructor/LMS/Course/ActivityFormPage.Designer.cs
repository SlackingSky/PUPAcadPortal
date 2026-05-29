namespace PUPAcadPortal
{
    partial class ActivityFormPage
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
            pnlQuizSection = new Panel();
            lblQuizHeader = new Label();
            btnAddQuestion = new buttonRounded();
            flpQuestions = new FlowLayoutPanel();
            pnlRubricSection = new Panel();
            chkRubric = new CheckBox();
            lblRubricNote = new Label();
            btnAddCriteria = new buttonRounded();
            pnlRubricRows = new Panel();
            flpRubric = new FlowLayoutPanel();
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
            pnlRubricRows.SuspendLayout();
            pnlFilesSection.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(128, 0, 0);
            pnlHeader.Controls.Add(btnCancel);
            pnlHeader.Controls.Add(lblPageTitle);
            pnlHeader.Controls.Add(lblCourseSub);
            pnlHeader.Controls.Add(btnSave);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1200, 68);
            pnlHeader.TabIndex = 0;
            pnlHeader.SizeChanged += pnlHeader_SizeChanged;
            // 
            // btnCancel
            // 
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
            // 
            // lblPageTitle
            // 
            lblPageTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblPageTitle.ForeColor = Color.White;
            lblPageTitle.Location = new Point(108, 10);
            lblPageTitle.Name = "lblPageTitle";
            lblPageTitle.Size = new Size(500, 28);
            lblPageTitle.TabIndex = 1;
            lblPageTitle.Text = "Create Activity";
            // 
            // lblCourseSub
            // 
            lblCourseSub.Font = new Font("Segoe UI", 9F);
            lblCourseSub.ForeColor = Color.FromArgb(230, 185, 185);
            lblCourseSub.Location = new Point(108, 42);
            lblCourseSub.Name = "lblCourseSub";
            lblCourseSub.Size = new Size(500, 18);
            lblCourseSub.TabIndex = 2;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.BackColor = Color.FromArgb(255, 196, 0);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnSave.ForeColor = Color.Black;
            btnSave.Location = new Point(1030, 17);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(160, 34);
            btnSave.TabIndex = 3;
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // pnlScroll
            // 
            pnlScroll.AutoScroll = true;
            pnlScroll.BackColor = Color.FromArgb(245, 245, 248);
            pnlScroll.Controls.Add(stackPanel);
            pnlScroll.Dock = DockStyle.Fill;
            pnlScroll.Location = new Point(0, 68);
            pnlScroll.Name = "pnlScroll";
            pnlScroll.Padding = new Padding(22, 18, 22, 18);
            pnlScroll.Size = new Size(1200, 832);
            pnlScroll.TabIndex = 1;
            pnlScroll.SizeChanged += pnlScroll_SizeChanged;
            // 
            // stackPanel
            // 
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
            stackPanel.Size = new Size(1156, 631);
            stackPanel.TabIndex = 0;
            stackPanel.WrapContents = false;
            // 
            // lblError
            // 
            lblError.AutoSize = true;
            lblError.Font = new Font("Segoe UI", 9.5F);
            lblError.ForeColor = Color.Red;
            lblError.Location = new Point(3, 0);
            lblError.Name = "lblError";
            lblError.Size = new Size(0, 17);
            lblError.TabIndex = 0;
            // 
            // pnlBasic
            // 
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
            // 
            // lblSectionBasic
            // 
            lblSectionBasic.Location = new Point(0, 0);
            lblSectionBasic.Name = "lblSectionBasic";
            lblSectionBasic.Size = new Size(100, 23);
            lblSectionBasic.TabIndex = 0;
            // 
            // lblTitleLbl
            // 
            lblTitleLbl.Location = new Point(0, 0);
            lblTitleLbl.Name = "lblTitleLbl";
            lblTitleLbl.Size = new Size(100, 23);
            lblTitleLbl.TabIndex = 1;
            // 
            // txtTitle
            // 
            txtTitle.Font = new Font("Segoe UI", 11F);
            txtTitle.Location = new Point(18, 60);
            txtTitle.Name = "txtTitle";
            txtTitle.PlaceholderText = "Enter activity title...";
            txtTitle.Size = new Size(860, 27);
            txtTitle.TabIndex = 2;
            // 
            // lblTypeLbl
            // 
            lblTypeLbl.Location = new Point(0, 0);
            lblTypeLbl.Name = "lblTypeLbl";
            lblTypeLbl.Size = new Size(100, 23);
            lblTypeLbl.TabIndex = 3;
            // 
            // cmbType
            // 
            cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbType.Font = new Font("Segoe UI", 10F);
            cmbType.FormattingEnabled = true;
            cmbType.Items.AddRange(new object[] { "Assignment", "Quiz", "Essay", "FileUpload" });
            cmbType.Location = new Point(18, 124);
            cmbType.Name = "cmbType";
            cmbType.Size = new Size(200, 25);
            cmbType.TabIndex = 3;
            cmbType.SelectedIndexChanged += cmbType_SelectedIndexChanged;
            // 
            // lblDeadlineLbl
            // 
            lblDeadlineLbl.Location = new Point(0, 0);
            lblDeadlineLbl.Name = "lblDeadlineLbl";
            lblDeadlineLbl.Size = new Size(100, 23);
            lblDeadlineLbl.TabIndex = 4;
            // 
            // dtpDeadline
            // 
            dtpDeadline.CustomFormat = "MM/dd/yyyy  hh:mm tt";
            dtpDeadline.Font = new Font("Segoe UI", 10F);
            dtpDeadline.Format = DateTimePickerFormat.Custom;
            dtpDeadline.Location = new Point(238, 124);
            dtpDeadline.Name = "dtpDeadline";
            dtpDeadline.Size = new Size(320, 25);
            dtpDeadline.TabIndex = 4;
            dtpDeadline.Value = new DateTime(2026, 6, 5, 18, 58, 34, 979);
            // 
            // lblPointsLbl
            // 
            lblPointsLbl.Location = new Point(0, 0);
            lblPointsLbl.Name = "lblPointsLbl";
            lblPointsLbl.Size = new Size(100, 23);
            lblPointsLbl.TabIndex = 5;
            // 
            // nudPoints
            // 
            nudPoints.Font = new Font("Segoe UI", 10F);
            nudPoints.Location = new Point(578, 124);
            nudPoints.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudPoints.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudPoints.Name = "nudPoints";
            nudPoints.Size = new Size(120, 25);
            nudPoints.TabIndex = 5;
            nudPoints.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // lblPointsNote
            // 
            lblPointsNote.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblPointsNote.ForeColor = Color.DimGray;
            lblPointsNote.Location = new Point(578, 156);
            lblPointsNote.Name = "lblPointsNote";
            lblPointsNote.Size = new Size(300, 18);
            lblPointsNote.TabIndex = 6;
            // 
            // lblDescLbl
            // 
            lblDescLbl.Location = new Point(0, 0);
            lblDescLbl.Name = "lblDescLbl";
            lblDescLbl.Size = new Size(100, 23);
            lblDescLbl.TabIndex = 7;
            // 
            // txtDescription
            // 
            txtDescription.Font = new Font("Segoe UI", 10F);
            txtDescription.Location = new Point(18, 190);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.PlaceholderText = "Add instructions, objectives, or notes for students...";
            txtDescription.ScrollBars = ScrollBars.Vertical;
            txtDescription.Size = new Size(860, 68);
            txtDescription.TabIndex = 7;
            // 
            // pnlQuizSection
            // 
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
            // 
            // lblQuizHeader
            // 
            lblQuizHeader.AutoSize = true;
            lblQuizHeader.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblQuizHeader.ForeColor = Color.FromArgb(63, 81, 181);
            lblQuizHeader.Location = new Point(18, 14);
            lblQuizHeader.Name = "lblQuizHeader";
            lblQuizHeader.Size = new Size(126, 19);
            lblQuizHeader.TabIndex = 0;
            lblQuizHeader.Text = "❓ Quiz Questions";
            // 
            // btnAddQuestion
            // 
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
            // 
            // flpQuestions
            // 
            flpQuestions.AutoSize = true;
            flpQuestions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpQuestions.FlowDirection = FlowDirection.TopDown;
            flpQuestions.Location = new Point(18, 50);
            flpQuestions.Name = "flpQuestions";
            flpQuestions.Size = new Size(0, 0);
            flpQuestions.TabIndex = 2;
            flpQuestions.WrapContents = false;
            // 
            // pnlRubricSection
            // 
            pnlRubricSection.AutoSize = true;
            pnlRubricSection.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlRubricSection.BackColor = Color.White;
            pnlRubricSection.Controls.Add(chkRubric);
            pnlRubricSection.Controls.Add(lblRubricNote);
            pnlRubricSection.Controls.Add(btnAddCriteria);
            pnlRubricSection.Controls.Add(pnlRubricRows);
            pnlRubricSection.Location = new Point(0, 384);
            pnlRubricSection.Margin = new Padding(0, 0, 0, 14);
            pnlRubricSection.Name = "pnlRubricSection";
            pnlRubricSection.Padding = new Padding(18, 14, 18, 14);
            pnlRubricSection.Size = new Size(903, 79);
            pnlRubricSection.TabIndex = 3;
            pnlRubricSection.Visible = false;
            pnlRubricSection.SizeChanged += pnlRubricSection_SizeChanged;
            // 
            // chkRubric
            // 
            chkRubric.AutoSize = true;
            chkRubric.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            chkRubric.ForeColor = Color.DarkSlateBlue;
            chkRubric.Location = new Point(18, 14);
            chkRubric.Name = "chkRubric";
            chkRubric.Size = new Size(177, 23);
            chkRubric.TabIndex = 0;
            chkRubric.Text = "Enable Rubric Grading";
            chkRubric.UseVisualStyleBackColor = true;
            chkRubric.CheckedChanged += chkRubric_CheckedChanged;
            // 
            // lblRubricNote
            // 
            lblRubricNote.AutoSize = true;
            lblRubricNote.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblRubricNote.ForeColor = Color.DimGray;
            lblRubricNote.Location = new Point(18, 44);
            lblRubricNote.Name = "lblRubricNote";
            lblRubricNote.Size = new Size(177, 15);
            lblRubricNote.TabIndex = 1;
            lblRubricNote.Text = "Define criteria and points below.";
            lblRubricNote.Visible = false;
            // 
            // btnAddCriteria
            // 
            btnAddCriteria.BackColor = Color.DarkSlateBlue;
            btnAddCriteria.FlatAppearance.BorderSize = 0;
            btnAddCriteria.FlatStyle = FlatStyle.Flat;
            btnAddCriteria.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAddCriteria.ForeColor = Color.White;
            btnAddCriteria.Location = new Point(757, 12);
            btnAddCriteria.Name = "btnAddCriteria";
            btnAddCriteria.Size = new Size(125, 30);
            btnAddCriteria.TabIndex = 2;
            btnAddCriteria.Text = "+ Add Criteria";
            btnAddCriteria.UseVisualStyleBackColor = false;
            btnAddCriteria.Click += btnAddCriteria_Click;
            // 
            // pnlRubricRows
            // 
            pnlRubricRows.AutoSize = true;
            pnlRubricRows.Controls.Add(flpRubric);
            pnlRubricRows.Location = new Point(18, 62);
            pnlRubricRows.Name = "pnlRubricRows";
            pnlRubricRows.Size = new Size(860, 0);
            pnlRubricRows.TabIndex = 3;
            pnlRubricRows.Visible = false;
            // 
            // flpRubric
            // 
            flpRubric.AutoSize = true;
            flpRubric.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpRubric.Dock = DockStyle.Top;
            flpRubric.FlowDirection = FlowDirection.TopDown;
            flpRubric.Location = new Point(0, 0);
            flpRubric.Name = "flpRubric";
            flpRubric.Size = new Size(860, 0);
            flpRubric.TabIndex = 0;
            flpRubric.WrapContents = false;
            // 
            // pnlFilesSection
            // 
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
            // 
            // lblFilesHeader
            // 
            lblFilesHeader.AutoSize = true;
            lblFilesHeader.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblFilesHeader.ForeColor = Color.DarkCyan;
            lblFilesHeader.Location = new Point(18, 14);
            lblFilesHeader.Name = "lblFilesHeader";
            lblFilesHeader.Size = new Size(116, 19);
            lblFilesHeader.TabIndex = 0;
            lblFilesHeader.Text = "📎 Attached Files";
            // 
            // btnAttachFile
            // 
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
            // 
            // lblNoFiles
            // 
            lblNoFiles.AutoSize = true;
            lblNoFiles.Font = new Font("Segoe UI", 9.5F, FontStyle.Italic);
            lblNoFiles.ForeColor = Color.Gray;
            lblNoFiles.Location = new Point(18, 50);
            lblNoFiles.Name = "lblNoFiles";
            lblNoFiles.Size = new Size(288, 17);
            lblNoFiles.TabIndex = 2;
            lblNoFiles.Text = "No files attached yet. Click \"+ Attach Files\" to add.";
            // 
            // flpFiles
            // 
            flpFiles.Location = new Point(18, 50);
            flpFiles.Name = "flpFiles";
            flpFiles.Size = new Size(860, 78);
            flpFiles.TabIndex = 3;
            // 
            // ActivityFormPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlScroll);
            Controls.Add(pnlHeader);
            Name = "ActivityFormPage";
            Size = new Size(1200, 900);
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

        #endregion

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