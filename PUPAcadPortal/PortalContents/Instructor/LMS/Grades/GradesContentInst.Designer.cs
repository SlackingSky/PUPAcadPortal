namespace PUPAcadPortal.PortalContents.Instructor.LMS
{
    partial class GradesContentInst
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
            btnImportCSV = new Button();
            btnReleaseGrades = new Button();
            lblPageSubtitle = new Label();
            lblPageTitle = new Label();
            pnlFilters = new Panel();
            lblRecordCount = new Label();
            txtSearch = new TextBox();
            cmbStatusFilter = new ComboBox();
            lblStatusFilter = new Label();
            cmbCourseSection = new ComboBox();
            lblCourseSection = new Label();
            pnlCards = new TableLayoutPanel();
            cardTotal = new Panel();
            lblTotalSub = new Label();
            lblTotalTitle = new Label();
            lblTotalVal = new Label();
            cardSubmitted = new Panel();
            lblSubmittedSub = new Label();
            lblSubmittedTitle = new Label();
            lblSubmittedVal = new Label();
            cardPending = new Panel();
            lblPendingSub = new Label();
            lblPendingTitle = new Label();
            lblPendingVal = new Label();
            cardAverage = new Panel();
            lblAvgSub = new Label();
            lblAvgTitle = new Label();
            lblAvgVal = new Label();
            cardHighest = new Panel();
            lblHighestSub = new Label();
            lblHighestTitle = new Label();
            lblHighestVal = new Label();
            pnlToolbar = new Panel();
            btnEditWeights = new Button();
            btnAddColumn = new Button();
            pnlBottom = new TableLayoutPanel();
            pnlGradingScale = new Panel();
            gridGradingScale = new DataGridView();
            colRange = new DataGridViewTextBoxColumn();
            colEquiv = new DataGridViewTextBoxColumn();
            colDesc = new DataGridViewTextBoxColumn();
            lblGradingScaleTitle = new Label();
            pnlLegend = new Panel();
            pnlLegendItems = new Panel();
            lblLegendTitle = new Label();
            pnlQuickActions = new Panel();
            btnQuickReleaseGrades = new Button();
            btnPrintGrades = new Button();
            btnExportCSV = new Button();
            btnSaveChanges = new Button();
            lblQuickActionsTitle = new Label();
            pnlMidWrapper = new Panel();
            tabTerms = new TabControl();
            tabMidterm = new TabPage();
            gridStudents = new DataGridView();
            tabFinalTerm = new TabPage();
            dataGridView1 = new DataGridView();
            pnlHeader.SuspendLayout();
            pnlFilters.SuspendLayout();
            pnlCards.SuspendLayout();
            cardTotal.SuspendLayout();
            cardSubmitted.SuspendLayout();
            cardPending.SuspendLayout();
            cardAverage.SuspendLayout();
            cardHighest.SuspendLayout();
            pnlToolbar.SuspendLayout();
            pnlBottom.SuspendLayout();
            pnlGradingScale.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridGradingScale).BeginInit();
            pnlLegend.SuspendLayout();
            pnlQuickActions.SuspendLayout();
            pnlMidWrapper.SuspendLayout();
            tabTerms.SuspendLayout();
            tabMidterm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridStudents).BeginInit();
            tabFinalTerm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(106, 0, 0);
            pnlHeader.Controls.Add(btnImportCSV);
            pnlHeader.Controls.Add(btnReleaseGrades);
            pnlHeader.Controls.Add(lblPageSubtitle);
            pnlHeader.Controls.Add(lblPageTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(18, 0, 18, 0);
            pnlHeader.Size = new Size(1648, 68);
            pnlHeader.TabIndex = 10;
            // 
            // btnImportCSV
            // 
            btnImportCSV.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnImportCSV.BackColor = Color.FromArgb(70, 0, 0);
            btnImportCSV.FlatAppearance.BorderColor = Color.FromArgb(130, 55, 55);
            btnImportCSV.FlatStyle = FlatStyle.Flat;
            btnImportCSV.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnImportCSV.ForeColor = Color.White;
            btnImportCSV.Location = new Point(1510, 18);
            btnImportCSV.Name = "btnImportCSV";
            btnImportCSV.Size = new Size(120, 32);
            btnImportCSV.TabIndex = 2;
            btnImportCSV.Text = "⬆  Import CSV";
            btnImportCSV.UseVisualStyleBackColor = false;
            // 
            // btnReleaseGrades
            // 
            btnReleaseGrades.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnReleaseGrades.BackColor = Color.FromArgb(70, 0, 0);
            btnReleaseGrades.FlatAppearance.BorderColor = Color.FromArgb(130, 55, 55);
            btnReleaseGrades.FlatStyle = FlatStyle.Flat;
            btnReleaseGrades.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnReleaseGrades.ForeColor = Color.White;
            btnReleaseGrades.Location = new Point(1366, 18);
            btnReleaseGrades.Name = "btnReleaseGrades";
            btnReleaseGrades.Size = new Size(136, 32);
            btnReleaseGrades.TabIndex = 1;
            btnReleaseGrades.Text = "▶  Release Grades";
            btnReleaseGrades.UseVisualStyleBackColor = false;
            // 
            // lblPageSubtitle
            // 
            lblPageSubtitle.AutoSize = true;
            lblPageSubtitle.BackColor = Color.Transparent;
            lblPageSubtitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPageSubtitle.ForeColor = Color.FromArgb(210, 185, 185);
            lblPageSubtitle.Location = new Point(20, 44);
            lblPageSubtitle.Name = "lblPageSubtitle";
            lblPageSubtitle.Size = new Size(250, 15);
            lblPageSubtitle.TabIndex = 1;
            lblPageSubtitle.Text = "Manage, compute, and release student grades";
            // 
            // lblPageTitle
            // 
            lblPageTitle.AutoSize = true;
            lblPageTitle.BackColor = Color.Transparent;
            lblPageTitle.Font = new Font("Segoe UI", 17F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPageTitle.ForeColor = Color.White;
            lblPageTitle.Location = new Point(18, 8);
            lblPageTitle.Name = "lblPageTitle";
            lblPageTitle.Size = new Size(225, 31);
            lblPageTitle.TabIndex = 0;
            lblPageTitle.Text = "Grade Management";
            // 
            // pnlFilters
            // 
            pnlFilters.BackColor = Color.White;
            pnlFilters.Controls.Add(lblRecordCount);
            pnlFilters.Controls.Add(txtSearch);
            pnlFilters.Controls.Add(cmbStatusFilter);
            pnlFilters.Controls.Add(lblStatusFilter);
            pnlFilters.Controls.Add(cmbCourseSection);
            pnlFilters.Controls.Add(lblCourseSection);
            pnlFilters.Dock = DockStyle.Top;
            pnlFilters.Location = new Point(0, 68);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Padding = new Padding(14, 5, 14, 5);
            pnlFilters.Size = new Size(1648, 54);
            pnlFilters.TabIndex = 9;
            // 
            // lblRecordCount
            // 
            lblRecordCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblRecordCount.AutoSize = true;
            lblRecordCount.Font = new Font("Segoe UI", 8.5F);
            lblRecordCount.ForeColor = Color.FromArgb(140, 120, 120);
            lblRecordCount.Location = new Point(1490, 28);
            lblRecordCount.Name = "lblRecordCount";
            lblRecordCount.Size = new Size(61, 15);
            lblRecordCount.TabIndex = 3;
            lblRecordCount.Text = "0 students";
            // 
            // txtSearch
            // 
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Font = new Font("Segoe UI", 9F);
            txtSearch.Location = new Point(484, 24);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "🔍  Search name or student ID…";
            txtSearch.Size = new Size(272, 23);
            txtSearch.TabIndex = 2;
            // 
            // cmbStatusFilter
            // 
            cmbStatusFilter.BackColor = Color.White;
            cmbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatusFilter.FlatStyle = FlatStyle.Flat;
            cmbStatusFilter.Font = new Font("Segoe UI", 9F);
            cmbStatusFilter.Location = new Point(334, 24);
            cmbStatusFilter.Name = "cmbStatusFilter";
            cmbStatusFilter.Size = new Size(140, 23);
            cmbStatusFilter.TabIndex = 1;
            // 
            // lblStatusFilter
            // 
            lblStatusFilter.AutoSize = true;
            lblStatusFilter.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            lblStatusFilter.ForeColor = Color.FromArgb(90, 90, 90);
            lblStatusFilter.Location = new Point(334, 6);
            lblStatusFilter.Name = "lblStatusFilter";
            lblStatusFilter.Size = new Size(41, 12);
            lblStatusFilter.TabIndex = 0;
            lblStatusFilter.Text = "STATUS";
            // 
            // cmbCourseSection
            // 
            cmbCourseSection.BackColor = Color.White;
            cmbCourseSection.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCourseSection.FlatStyle = FlatStyle.Flat;
            cmbCourseSection.Font = new Font("Segoe UI", 9F);
            cmbCourseSection.Location = new Point(14, 24);
            cmbCourseSection.Name = "cmbCourseSection";
            cmbCourseSection.Size = new Size(310, 23);
            cmbCourseSection.TabIndex = 0;
            // 
            // lblCourseSection
            // 
            lblCourseSection.AutoSize = true;
            lblCourseSection.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            lblCourseSection.ForeColor = Color.FromArgb(90, 90, 90);
            lblCourseSection.Location = new Point(14, 6);
            lblCourseSection.Name = "lblCourseSection";
            lblCourseSection.Size = new Size(47, 12);
            lblCourseSection.TabIndex = 0;
            lblCourseSection.Text = "SECTION";
            // 
            // pnlCards
            // 
            pnlCards.BackColor = Color.FromArgb(245, 241, 241);
            pnlCards.ColumnCount = 5;
            pnlCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            pnlCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            pnlCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            pnlCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            pnlCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            pnlCards.Controls.Add(cardTotal, 0, 0);
            pnlCards.Controls.Add(cardSubmitted, 1, 0);
            pnlCards.Controls.Add(cardPending, 2, 0);
            pnlCards.Controls.Add(cardAverage, 3, 0);
            pnlCards.Controls.Add(cardHighest, 4, 0);
            pnlCards.Dock = DockStyle.Top;
            pnlCards.Location = new Point(0, 122);
            pnlCards.Margin = new Padding(0);
            pnlCards.Name = "pnlCards";
            pnlCards.Padding = new Padding(10, 5, 10, 5);
            pnlCards.RowCount = 1;
            pnlCards.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlCards.Size = new Size(1648, 84);
            pnlCards.TabIndex = 8;
            // 
            // cardTotal
            // 
            cardTotal.BackColor = Color.White;
            cardTotal.Controls.Add(lblTotalSub);
            cardTotal.Controls.Add(lblTotalTitle);
            cardTotal.Controls.Add(lblTotalVal);
            cardTotal.Dock = DockStyle.Fill;
            cardTotal.Location = new Point(14, 7);
            cardTotal.Margin = new Padding(4, 2, 4, 2);
            cardTotal.Name = "cardTotal";
            cardTotal.Size = new Size(317, 70);
            cardTotal.TabIndex = 0;
            // 
            // lblTotalSub
            // 
            lblTotalSub.AutoSize = true;
            lblTotalSub.Font = new Font("Segoe UI", 8F);
            lblTotalSub.ForeColor = Color.FromArgb(160, 140, 140);
            lblTotalSub.Location = new Point(13, 56);
            lblTotalSub.Name = "lblTotalSub";
            lblTotalSub.Size = new Size(64, 13);
            lblTotalSub.TabIndex = 2;
            lblTotalSub.Text = "all sections";
            // 
            // lblTotalTitle
            // 
            lblTotalTitle.AutoSize = true;
            lblTotalTitle.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            lblTotalTitle.ForeColor = Color.FromArgb(120, 90, 90);
            lblTotalTitle.Location = new Point(12, 7);
            lblTotalTitle.Name = "lblTotalTitle";
            lblTotalTitle.Size = new Size(90, 12);
            lblTotalTitle.TabIndex = 0;
            lblTotalTitle.Text = "TOTAL STUDENTS";
            // 
            // lblTotalVal
            // 
            lblTotalVal.AutoSize = true;
            lblTotalVal.Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalVal.ForeColor = Color.FromArgb(106, 0, 0);
            lblTotalVal.Location = new Point(12, 20);
            lblTotalVal.Name = "lblTotalVal";
            lblTotalVal.Size = new Size(35, 41);
            lblTotalVal.TabIndex = 1;
            lblTotalVal.Text = "0";
            // 
            // cardSubmitted
            // 
            cardSubmitted.BackColor = Color.White;
            cardSubmitted.Controls.Add(lblSubmittedSub);
            cardSubmitted.Controls.Add(lblSubmittedTitle);
            cardSubmitted.Controls.Add(lblSubmittedVal);
            cardSubmitted.Dock = DockStyle.Fill;
            cardSubmitted.Location = new Point(339, 7);
            cardSubmitted.Margin = new Padding(4, 2, 4, 2);
            cardSubmitted.Name = "cardSubmitted";
            cardSubmitted.Size = new Size(317, 70);
            cardSubmitted.TabIndex = 1;
            // 
            // lblSubmittedSub
            // 
            lblSubmittedSub.AutoSize = true;
            lblSubmittedSub.Font = new Font("Segoe UI", 8F);
            lblSubmittedSub.ForeColor = Color.FromArgb(160, 140, 140);
            lblSubmittedSub.Location = new Point(13, 56);
            lblSubmittedSub.Name = "lblSubmittedSub";
            lblSubmittedSub.Size = new Size(63, 13);
            lblSubmittedSub.TabIndex = 2;
            lblSubmittedSub.Text = "0% of class";
            // 
            // lblSubmittedTitle
            // 
            lblSubmittedTitle.AutoSize = true;
            lblSubmittedTitle.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            lblSubmittedTitle.ForeColor = Color.FromArgb(120, 90, 90);
            lblSubmittedTitle.Location = new Point(12, 7);
            lblSubmittedTitle.Name = "lblSubmittedTitle";
            lblSubmittedTitle.Size = new Size(61, 12);
            lblSubmittedTitle.TabIndex = 0;
            lblSubmittedTitle.Text = "SUBMITTED";
            // 
            // lblSubmittedVal
            // 
            lblSubmittedVal.AutoSize = true;
            lblSubmittedVal.Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSubmittedVal.ForeColor = Color.FromArgb(16, 124, 65);
            lblSubmittedVal.Location = new Point(12, 20);
            lblSubmittedVal.Name = "lblSubmittedVal";
            lblSubmittedVal.Size = new Size(35, 41);
            lblSubmittedVal.TabIndex = 1;
            lblSubmittedVal.Text = "0";
            // 
            // cardPending
            // 
            cardPending.BackColor = Color.White;
            cardPending.Controls.Add(lblPendingSub);
            cardPending.Controls.Add(lblPendingTitle);
            cardPending.Controls.Add(lblPendingVal);
            cardPending.Dock = DockStyle.Fill;
            cardPending.Location = new Point(664, 7);
            cardPending.Margin = new Padding(4, 2, 4, 2);
            cardPending.Name = "cardPending";
            cardPending.Size = new Size(317, 70);
            cardPending.TabIndex = 2;
            // 
            // lblPendingSub
            // 
            lblPendingSub.AutoSize = true;
            lblPendingSub.Font = new Font("Segoe UI", 8F);
            lblPendingSub.ForeColor = Color.FromArgb(160, 140, 140);
            lblPendingSub.Location = new Point(13, 56);
            lblPendingSub.Name = "lblPendingSub";
            lblPendingSub.Size = new Size(63, 13);
            lblPendingSub.TabIndex = 2;
            lblPendingSub.Text = "0% of class";
            // 
            // lblPendingTitle
            // 
            lblPendingTitle.AutoSize = true;
            lblPendingTitle.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            lblPendingTitle.ForeColor = Color.FromArgb(120, 90, 90);
            lblPendingTitle.Location = new Point(12, 7);
            lblPendingTitle.Name = "lblPendingTitle";
            lblPendingTitle.Size = new Size(49, 12);
            lblPendingTitle.TabIndex = 0;
            lblPendingTitle.Text = "PENDING";
            // 
            // lblPendingVal
            // 
            lblPendingVal.AutoSize = true;
            lblPendingVal.Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPendingVal.ForeColor = Color.FromArgb(200, 100, 0);
            lblPendingVal.Location = new Point(12, 20);
            lblPendingVal.Name = "lblPendingVal";
            lblPendingVal.Size = new Size(35, 41);
            lblPendingVal.TabIndex = 1;
            lblPendingVal.Text = "0";
            // 
            // cardAverage
            // 
            cardAverage.BackColor = Color.White;
            cardAverage.Controls.Add(lblAvgSub);
            cardAverage.Controls.Add(lblAvgTitle);
            cardAverage.Controls.Add(lblAvgVal);
            cardAverage.Dock = DockStyle.Fill;
            cardAverage.Location = new Point(989, 7);
            cardAverage.Margin = new Padding(4, 2, 4, 2);
            cardAverage.Name = "cardAverage";
            cardAverage.Size = new Size(317, 70);
            cardAverage.TabIndex = 3;
            // 
            // lblAvgSub
            // 
            lblAvgSub.AutoSize = true;
            lblAvgSub.Font = new Font("Segoe UI", 8F);
            lblAvgSub.ForeColor = Color.FromArgb(160, 140, 140);
            lblAvgSub.Location = new Point(13, 56);
            lblAvgSub.Name = "lblAvgSub";
            lblAvgSub.Size = new Size(65, 13);
            lblAvgSub.TabIndex = 2;
            lblAvgSub.Text = "no data yet";
            // 
            // lblAvgTitle
            // 
            lblAvgTitle.AutoSize = true;
            lblAvgTitle.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            lblAvgTitle.ForeColor = Color.FromArgb(120, 90, 90);
            lblAvgTitle.Location = new Point(12, 7);
            lblAvgTitle.Name = "lblAvgTitle";
            lblAvgTitle.Size = new Size(85, 12);
            lblAvgTitle.TabIndex = 0;
            lblAvgTitle.Text = "AVERAGE GRADE";
            // 
            // lblAvgVal
            // 
            lblAvgVal.AutoSize = true;
            lblAvgVal.Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAvgVal.ForeColor = Color.FromArgb(0, 112, 192);
            lblAvgVal.Location = new Point(12, 20);
            lblAvgVal.Name = "lblAvgVal";
            lblAvgVal.Size = new Size(48, 41);
            lblAvgVal.TabIndex = 1;
            lblAvgVal.Text = "—";
            // 
            // cardHighest
            // 
            cardHighest.BackColor = Color.White;
            cardHighest.Controls.Add(lblHighestSub);
            cardHighest.Controls.Add(lblHighestTitle);
            cardHighest.Controls.Add(lblHighestVal);
            cardHighest.Dock = DockStyle.Fill;
            cardHighest.Location = new Point(1314, 7);
            cardHighest.Margin = new Padding(4, 2, 4, 2);
            cardHighest.Name = "cardHighest";
            cardHighest.Size = new Size(320, 70);
            cardHighest.TabIndex = 4;
            // 
            // lblHighestSub
            // 
            lblHighestSub.AutoSize = true;
            lblHighestSub.Font = new Font("Segoe UI", 8F);
            lblHighestSub.ForeColor = Color.FromArgb(160, 140, 140);
            lblHighestSub.Location = new Point(13, 56);
            lblHighestSub.Name = "lblHighestSub";
            lblHighestSub.Size = new Size(65, 13);
            lblHighestSub.TabIndex = 2;
            lblHighestSub.Text = "no data yet";
            // 
            // lblHighestTitle
            // 
            lblHighestTitle.AutoSize = true;
            lblHighestTitle.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            lblHighestTitle.ForeColor = Color.FromArgb(120, 90, 90);
            lblHighestTitle.Location = new Point(12, 7);
            lblHighestTitle.Name = "lblHighestTitle";
            lblHighestTitle.Size = new Size(84, 12);
            lblHighestTitle.TabIndex = 0;
            lblHighestTitle.Text = "HIGHEST GRADE";
            // 
            // lblHighestVal
            // 
            lblHighestVal.AutoSize = true;
            lblHighestVal.Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHighestVal.ForeColor = Color.FromArgb(16, 124, 65);
            lblHighestVal.Location = new Point(12, 20);
            lblHighestVal.Name = "lblHighestVal";
            lblHighestVal.Size = new Size(48, 41);
            lblHighestVal.TabIndex = 1;
            lblHighestVal.Text = "—";
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = Color.FromArgb(252, 249, 249);
            pnlToolbar.Controls.Add(btnEditWeights);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(0, 206);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Padding = new Padding(12, 4, 12, 4);
            pnlToolbar.Size = new Size(1648, 34);
            pnlToolbar.TabIndex = 7;
            // 
            // btnEditWeights
            // 
            btnEditWeights.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEditWeights.BackColor = Color.FromArgb(106, 0, 0);
            btnEditWeights.FlatAppearance.BorderSize = 0;
            btnEditWeights.FlatStyle = FlatStyle.Flat;
            btnEditWeights.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEditWeights.ForeColor = Color.White;
            btnEditWeights.Location = new Point(1474, 4);
            btnEditWeights.Name = "btnEditWeights";
            btnEditWeights.Size = new Size(162, 26);
            btnEditWeights.TabIndex = 0;
            btnEditWeights.Text = "⚙  Edit Grade Weights";
            btnEditWeights.UseVisualStyleBackColor = false;
            // 
            // btnAddColumn
            // 
            btnAddColumn.Location = new Point(0, 0);
            btnAddColumn.Name = "btnAddColumn";
            btnAddColumn.Size = new Size(1, 1);
            btnAddColumn.TabIndex = 0;
            btnAddColumn.Visible = false;
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = Color.White;
            pnlBottom.ColumnCount = 3;
            pnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37F));
            pnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31F));
            pnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            pnlBottom.Controls.Add(pnlGradingScale, 0, 0);
            pnlBottom.Controls.Add(pnlLegend, 1, 0);
            pnlBottom.Controls.Add(pnlQuickActions, 2, 0);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 777);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Padding = new Padding(10, 6, 10, 6);
            pnlBottom.RowCount = 1;
            pnlBottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlBottom.Size = new Size(1648, 192);
            pnlBottom.TabIndex = 6;
            // 
            // pnlGradingScale
            // 
            pnlGradingScale.BackColor = Color.White;
            pnlGradingScale.Controls.Add(gridGradingScale);
            pnlGradingScale.Controls.Add(lblGradingScaleTitle);
            pnlGradingScale.Dock = DockStyle.Fill;
            pnlGradingScale.Location = new Point(10, 6);
            pnlGradingScale.Margin = new Padding(0, 0, 5, 0);
            pnlGradingScale.Name = "pnlGradingScale";
            pnlGradingScale.Size = new Size(597, 180);
            pnlGradingScale.TabIndex = 0;
            // 
            // gridGradingScale
            // 
            gridGradingScale.AllowUserToAddRows = false;
            gridGradingScale.AllowUserToDeleteRows = false;
            gridGradingScale.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridGradingScale.Columns.AddRange(new DataGridViewColumn[] { colRange, colEquiv, colDesc });
            gridGradingScale.Dock = DockStyle.Fill;
            gridGradingScale.Location = new Point(0, 15);
            gridGradingScale.Name = "gridGradingScale";
            gridGradingScale.ReadOnly = true;
            gridGradingScale.RowHeadersVisible = false;
            gridGradingScale.ScrollBars = ScrollBars.Vertical;
            gridGradingScale.Size = new Size(597, 165);
            gridGradingScale.TabIndex = 0;
            // 
            // colRange
            // 
            colRange.HeaderText = "Range";
            colRange.Name = "colRange";
            colRange.ReadOnly = true;
            colRange.Width = 66;
            // 
            // colEquiv
            // 
            colEquiv.HeaderText = "Equiv.";
            colEquiv.Name = "colEquiv";
            colEquiv.ReadOnly = true;
            colEquiv.Width = 58;
            // 
            // colDesc
            // 
            colDesc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colDesc.HeaderText = "Description";
            colDesc.Name = "colDesc";
            colDesc.ReadOnly = true;
            // 
            // lblGradingScaleTitle
            // 
            lblGradingScaleTitle.AutoSize = true;
            lblGradingScaleTitle.Dock = DockStyle.Top;
            lblGradingScaleTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblGradingScaleTitle.ForeColor = Color.FromArgb(106, 0, 0);
            lblGradingScaleTitle.Location = new Point(0, 0);
            lblGradingScaleTitle.Name = "lblGradingScaleTitle";
            lblGradingScaleTitle.Size = new Size(83, 15);
            lblGradingScaleTitle.TabIndex = 1;
            lblGradingScaleTitle.Text = "Grading Scale";
            // 
            // pnlLegend
            // 
            pnlLegend.BackColor = Color.White;
            pnlLegend.Controls.Add(pnlLegendItems);
            pnlLegend.Controls.Add(lblLegendTitle);
            pnlLegend.Dock = DockStyle.Fill;
            pnlLegend.Location = new Point(615, 6);
            pnlLegend.Margin = new Padding(3, 0, 3, 0);
            pnlLegend.Name = "pnlLegend";
            pnlLegend.Size = new Size(498, 180);
            pnlLegend.TabIndex = 1;
            // 
            // pnlLegendItems
            // 
            pnlLegendItems.BackColor = Color.White;
            pnlLegendItems.Location = new Point(2, 20);
            pnlLegendItems.Name = "pnlLegendItems";
            pnlLegendItems.Size = new Size(460, 164);
            pnlLegendItems.TabIndex = 1;
            // 
            // lblLegendTitle
            // 
            lblLegendTitle.AutoSize = true;
            lblLegendTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblLegendTitle.ForeColor = Color.FromArgb(106, 0, 0);
            lblLegendTitle.Location = new Point(2, 2);
            lblLegendTitle.Name = "lblLegendTitle";
            lblLegendTitle.Size = new Size(48, 15);
            lblLegendTitle.TabIndex = 0;
            lblLegendTitle.Text = "Legend";
            // 
            // pnlQuickActions
            // 
            pnlQuickActions.BackColor = Color.White;
            pnlQuickActions.Controls.Add(btnQuickReleaseGrades);
            pnlQuickActions.Controls.Add(btnPrintGrades);
            pnlQuickActions.Controls.Add(btnExportCSV);
            pnlQuickActions.Controls.Add(btnSaveChanges);
            pnlQuickActions.Controls.Add(lblQuickActionsTitle);
            pnlQuickActions.Dock = DockStyle.Fill;
            pnlQuickActions.Location = new Point(1121, 6);
            pnlQuickActions.Margin = new Padding(5, 0, 0, 0);
            pnlQuickActions.Name = "pnlQuickActions";
            pnlQuickActions.Size = new Size(517, 180);
            pnlQuickActions.TabIndex = 2;
            // 
            // btnQuickReleaseGrades
            // 
            btnQuickReleaseGrades.BackColor = Color.FromArgb(106, 0, 0);
            btnQuickReleaseGrades.FlatAppearance.BorderSize = 0;
            btnQuickReleaseGrades.FlatStyle = FlatStyle.Flat;
            btnQuickReleaseGrades.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnQuickReleaseGrades.ForeColor = Color.White;
            btnQuickReleaseGrades.Location = new Point(2, 124);
            btnQuickReleaseGrades.Name = "btnQuickReleaseGrades";
            btnQuickReleaseGrades.Size = new Size(430, 28);
            btnQuickReleaseGrades.TabIndex = 4;
            btnQuickReleaseGrades.Text = "▶  Release Grades";
            btnQuickReleaseGrades.UseVisualStyleBackColor = false;
            btnQuickReleaseGrades.Click += BtnReleaseGrades_Click;
            // 
            // btnPrintGrades
            // 
            btnPrintGrades.BackColor = Color.White;
            btnPrintGrades.FlatAppearance.BorderColor = Color.FromArgb(190, 175, 175);
            btnPrintGrades.FlatStyle = FlatStyle.Flat;
            btnPrintGrades.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPrintGrades.ForeColor = Color.FromArgb(50, 50, 50);
            btnPrintGrades.Location = new Point(2, 90);
            btnPrintGrades.Name = "btnPrintGrades";
            btnPrintGrades.Size = new Size(430, 28);
            btnPrintGrades.TabIndex = 3;
            btnPrintGrades.Text = "🖨  Print Grades";
            btnPrintGrades.UseVisualStyleBackColor = false;
            btnPrintGrades.Click += BtnPrintGrades_Click;
            // 
            // btnExportCSV
            // 
            btnExportCSV.BackColor = Color.FromArgb(241, 196, 15);
            btnExportCSV.FlatAppearance.BorderSize = 0;
            btnExportCSV.FlatStyle = FlatStyle.Flat;
            btnExportCSV.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnExportCSV.ForeColor = Color.Black;
            btnExportCSV.Location = new Point(2, 56);
            btnExportCSV.Name = "btnExportCSV";
            btnExportCSV.Size = new Size(430, 28);
            btnExportCSV.TabIndex = 2;
            btnExportCSV.Text = "📥  Export to CSV";
            btnExportCSV.UseVisualStyleBackColor = false;
            btnExportCSV.Click += BtnExportCSV_Click;
            // 
            // btnSaveChanges
            // 
            btnSaveChanges.BackColor = Color.FromArgb(16, 124, 65);
            btnSaveChanges.FlatAppearance.BorderSize = 0;
            btnSaveChanges.FlatStyle = FlatStyle.Flat;
            btnSaveChanges.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSaveChanges.ForeColor = Color.White;
            btnSaveChanges.Location = new Point(2, 22);
            btnSaveChanges.Name = "btnSaveChanges";
            btnSaveChanges.Size = new Size(430, 28);
            btnSaveChanges.TabIndex = 1;
            btnSaveChanges.Text = "💾  Save Changes";
            btnSaveChanges.UseVisualStyleBackColor = false;
            // 
            // lblQuickActionsTitle
            // 
            lblQuickActionsTitle.AutoSize = true;
            lblQuickActionsTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblQuickActionsTitle.ForeColor = Color.FromArgb(106, 0, 0);
            lblQuickActionsTitle.Location = new Point(2, 2);
            lblQuickActionsTitle.Name = "lblQuickActionsTitle";
            lblQuickActionsTitle.Size = new Size(83, 15);
            lblQuickActionsTitle.TabIndex = 0;
            lblQuickActionsTitle.Text = "Quick Actions";
            // 
            // pnlMidWrapper
            // 
            pnlMidWrapper.BackColor = Color.White;
            pnlMidWrapper.Controls.Add(tabTerms);
            pnlMidWrapper.Dock = DockStyle.Fill;
            pnlMidWrapper.Location = new Point(0, 240);
            pnlMidWrapper.Name = "pnlMidWrapper";
            pnlMidWrapper.Size = new Size(1648, 537);
            pnlMidWrapper.TabIndex = 5;
            // 
            // tabTerms
            // 
            tabTerms.Controls.Add(tabMidterm);
            tabTerms.Controls.Add(tabFinalTerm);
            tabTerms.Dock = DockStyle.Fill;
            tabTerms.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            tabTerms.Location = new Point(0, 0);
            tabTerms.Name = "tabTerms";
            tabTerms.SelectedIndex = 0;
            tabTerms.Size = new Size(1648, 537);
            tabTerms.TabIndex = 0;
            // 
            // tabMidterm
            // 
            tabMidterm.BackColor = Color.White;
            tabMidterm.Controls.Add(gridStudents);
            tabMidterm.Location = new Point(4, 26);
            tabMidterm.Name = "tabMidterm";
            tabMidterm.Size = new Size(1640, 507);
            tabMidterm.TabIndex = 0;
            tabMidterm.Text = "  Midterm  ";
            // 
            // gridStudents
            // 
            gridStudents.AllowUserToAddRows = false;
            gridStudents.AllowUserToDeleteRows = false;
            gridStudents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridStudents.Dock = DockStyle.Fill;
            gridStudents.Location = new Point(0, 0);
            gridStudents.Name = "gridStudents";
            gridStudents.ScrollBars = ScrollBars.Vertical;
            gridStudents.Size = new Size(1640, 507);
            gridStudents.TabIndex = 0;
            // 
            // tabFinalTerm
            // 
            tabFinalTerm.BackColor = Color.White;
            tabFinalTerm.Controls.Add(dataGridView1);
            tabFinalTerm.Location = new Point(4, 26);
            tabFinalTerm.Name = "tabFinalTerm";
            tabFinalTerm.Size = new Size(1640, 507);
            tabFinalTerm.TabIndex = 1;
            tabFinalTerm.Text = "  Final Term  ";
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ScrollBars = ScrollBars.Vertical;
            dataGridView1.Size = new Size(1640, 507);
            dataGridView1.TabIndex = 0;
            // 
            // GradesContentInst
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 246, 246);
            Controls.Add(pnlMidWrapper);
            Controls.Add(pnlBottom);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlCards);
            Controls.Add(pnlFilters);
            Controls.Add(pnlHeader);
            Name = "GradesContentInst";
            Size = new Size(1648, 969);
            Load += GradesContentInst_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlFilters.ResumeLayout(false);
            pnlFilters.PerformLayout();
            pnlCards.ResumeLayout(false);
            cardTotal.ResumeLayout(false);
            cardTotal.PerformLayout();
            cardSubmitted.ResumeLayout(false);
            cardSubmitted.PerformLayout();
            cardPending.ResumeLayout(false);
            cardPending.PerformLayout();
            cardAverage.ResumeLayout(false);
            cardAverage.PerformLayout();
            cardHighest.ResumeLayout(false);
            cardHighest.PerformLayout();
            pnlToolbar.ResumeLayout(false);
            pnlBottom.ResumeLayout(false);
            pnlGradingScale.ResumeLayout(false);
            pnlGradingScale.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridGradingScale).EndInit();
            pnlLegend.ResumeLayout(false);
            pnlLegend.PerformLayout();
            pnlQuickActions.ResumeLayout(false);
            pnlQuickActions.PerformLayout();
            pnlMidWrapper.ResumeLayout(false);
            tabTerms.ResumeLayout(false);
            tabMidterm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridStudents).EndInit();
            tabFinalTerm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);

        }

        #endregion

        // ── Designer-managed fields ──────────────────────────────────────────
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Label lblPageSubtitle;
        private System.Windows.Forms.Button btnReleaseGrades;
        private System.Windows.Forms.Button btnImportCSV;

        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.Label lblCourseSection;
        private System.Windows.Forms.ComboBox cmbCourseSection;
        private System.Windows.Forms.Label lblStatusFilter;
        private System.Windows.Forms.ComboBox cmbStatusFilter;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblRecordCount;

        private System.Windows.Forms.TableLayoutPanel pnlCards;
        private System.Windows.Forms.Panel cardTotal; 
        private System.Windows.Forms.Label lblTotalTitle; 
        private System.Windows.Forms.Label lblTotalVal; 
        private System.Windows.Forms.Label lblTotalSub;
        private System.Windows.Forms.Panel cardSubmitted; 
        private System.Windows.Forms.Label lblSubmittedTitle; 
        private System.Windows.Forms.Label lblSubmittedVal; 
        private System.Windows.Forms.Label lblSubmittedSub;
        private System.Windows.Forms.Panel cardPending; 
        private System.Windows.Forms.Label lblPendingTitle; 
        private System.Windows.Forms.Label lblPendingVal; 
        private System.Windows.Forms.Label lblPendingSub;
        private System.Windows.Forms.Panel cardAverage; 
        private System.Windows.Forms.Label lblAvgTitle; 
        private System.Windows.Forms.Label lblAvgVal; 
        private System.Windows.Forms.Label lblAvgSub;
        private System.Windows.Forms.Panel cardHighest; 
        private System.Windows.Forms.Label lblHighestTitle; 
        private System.Windows.Forms.Label lblHighestVal; 
        private System.Windows.Forms.Label lblHighestSub;

        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.Button btnEditWeights;
        private System.Windows.Forms.Button btnAddColumn;
        private System.Windows.Forms.Button btnSaveChanges;
        private System.Windows.Forms.Button btnExportCSV;
        private System.Windows.Forms.Button btnPrintGrades;
        private System.Windows.Forms.Button btnQuickReleaseGrades; // Added explicitly to replace looped array button

        private System.Windows.Forms.TableLayoutPanel pnlBottom;
        private System.Windows.Forms.Panel pnlGradingScale;
        private System.Windows.Forms.Label lblGradingScaleTitle;
        private System.Windows.Forms.DataGridView gridGradingScale;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRange;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEquiv;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDesc;
        private System.Windows.Forms.Panel pnlLegend;
        private System.Windows.Forms.Label lblLegendTitle;
        private System.Windows.Forms.Panel pnlLegendItems;
        private System.Windows.Forms.Panel pnlQuickActions;
        private System.Windows.Forms.Label lblQuickActionsTitle;

        private System.Windows.Forms.Panel pnlMidWrapper;
        private System.Windows.Forms.TabControl tabTerms;
        private System.Windows.Forms.TabPage tabMidterm;
        private System.Windows.Forms.DataGridView gridStudents;
        private System.Windows.Forms.TabPage tabFinalTerm;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}