using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Instructor.LMS
{
    partial class AttendanceContentInst
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
            pnlRoot = new Panel();
            pnlGrid = new Panel();
            pnlActionBar = new Panel();
            btnSaveAttendance = new Button();
            btnRefresh = new Button();
            pnlSummaryRow = new Panel();
            pnlCardSession = new Panel();
            lblSessionAttLabel = new Label();
            panel21 = new Panel();
            pnlCardPresent = new Panel();
            lblPresentTitle = new Label();
            lblPresentNum = new Label();
            lblPresentPct = new Label();
            pnlCardLate = new Panel();
            lblLateTitle = new Label();
            lblLateNum = new Label();
            lblLatePct = new Label();
            pnlCardAbsent = new Panel();
            lblAbsentTitle = new Label();
            lblAbsentNum = new Label();
            lblAbsentPct = new Label();
            pnlCardExcused = new Panel();
            lblExcusedTitle = new Label();
            lblExcusedNum = new Label();
            lblExcusedPct = new Label();
            pnlCardLastUpdate = new Panel();
            lblLastUpdate = new Label();
            lblDateTime = new Label();
            lblByInstructor = new Label();
            pnlFilterBar = new Panel();
            lblCourseLabel = new Label();
            cmbCourse = new ComboBox();
            lblDateLabel = new Label();
            dtpDate = new DateTimePicker();
            lblSessionLabel = new Label();
            cmbSession = new ComboBox();
            txtSearch = new TextBox();
            btnQRCode = new Button();
            btnImportCSV = new Button();
            btnExport = new Button();
            pnlTitleBar = new Panel();
            lblAttendanceTitle = new Label();
            pnlRoot.SuspendLayout();
            pnlActionBar.SuspendLayout();
            pnlSummaryRow.SuspendLayout();
            pnlCardSession.SuspendLayout();
            pnlCardPresent.SuspendLayout();
            pnlCardLate.SuspendLayout();
            pnlCardAbsent.SuspendLayout();
            pnlCardExcused.SuspendLayout();
            pnlCardLastUpdate.SuspendLayout();
            pnlFilterBar.SuspendLayout();
            pnlTitleBar.SuspendLayout();
            SuspendLayout();
            // 
            // pnlRoot
            // 
            pnlRoot.BackColor = Color.FromArgb(245, 245, 248);
            pnlRoot.Controls.Add(pnlGrid);
            pnlRoot.Controls.Add(pnlActionBar);
            pnlRoot.Controls.Add(pnlSummaryRow);
            pnlRoot.Controls.Add(pnlFilterBar);
            pnlRoot.Controls.Add(pnlTitleBar);
            pnlRoot.Dock = DockStyle.Fill;
            pnlRoot.Location = new Point(0, 0);
            pnlRoot.Margin = new Padding(0);
            pnlRoot.Name = "pnlRoot";
            pnlRoot.Size = new Size(1649, 989);
            pnlRoot.TabIndex = 0;
            // 
            // pnlGrid
            // 
            pnlGrid.BackColor = Color.White;
            pnlGrid.Dock = DockStyle.Fill;
            pnlGrid.Location = new Point(0, 282);
            pnlGrid.Name = "pnlGrid";
            pnlGrid.Size = new Size(1649, 707);
            pnlGrid.TabIndex = 0;
            // 
            // pnlActionBar
            // 
            pnlActionBar.BackColor = Color.White;
            pnlActionBar.Controls.Add(btnSaveAttendance);
            pnlActionBar.Controls.Add(btnRefresh);
            pnlActionBar.Dock = DockStyle.Top;
            pnlActionBar.Location = new Point(0, 230);
            pnlActionBar.Name = "pnlActionBar";
            pnlActionBar.Size = new Size(1649, 52);
            pnlActionBar.TabIndex = 1;
            // 
            // btnSaveAttendance
            // 
            btnSaveAttendance.BackColor = Color.FromArgb(106, 0, 0);
            btnSaveAttendance.Cursor = Cursors.Hand;
            btnSaveAttendance.FlatAppearance.BorderSize = 0;
            btnSaveAttendance.FlatStyle = FlatStyle.Flat;
            btnSaveAttendance.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSaveAttendance.ForeColor = Color.White;
            btnSaveAttendance.Location = new Point(14, 11);
            btnSaveAttendance.Name = "btnSaveAttendance";
            btnSaveAttendance.Size = new Size(160, 30);
            btnSaveAttendance.TabIndex = 0;
            btnSaveAttendance.Text = "✓  Save Attendance";
            btnSaveAttendance.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 8.5F);
            btnRefresh.Location = new Point(184, 11);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 30);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "⟳  Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            // 
            // pnlSummaryRow
            // 
            pnlSummaryRow.BackColor = Color.FromArgb(245, 245, 248);
            pnlSummaryRow.Controls.Add(pnlCardSession);
            pnlSummaryRow.Controls.Add(pnlCardPresent);
            pnlSummaryRow.Controls.Add(pnlCardLate);
            pnlSummaryRow.Controls.Add(pnlCardAbsent);
            pnlSummaryRow.Controls.Add(pnlCardExcused);
            pnlSummaryRow.Controls.Add(pnlCardLastUpdate);
            pnlSummaryRow.Dock = DockStyle.Top;
            pnlSummaryRow.Location = new Point(0, 100);
            pnlSummaryRow.Name = "pnlSummaryRow";
            pnlSummaryRow.Padding = new Padding(10, 8, 10, 4);
            pnlSummaryRow.Size = new Size(1649, 130);
            pnlSummaryRow.TabIndex = 2;
            pnlSummaryRow.SizeChanged += PnlSummaryRow_SizeChanged;
            // 
            // pnlCardSession
            // 
            pnlCardSession.BackColor = Color.White;
            pnlCardSession.Controls.Add(lblSessionAttLabel);
            pnlCardSession.Controls.Add(panel21);
            pnlCardSession.Location = new Point(0, 0);
            pnlCardSession.Margin = new Padding(4);
            pnlCardSession.Name = "pnlCardSession";
            pnlCardSession.Size = new Size(1649, 129);
            pnlCardSession.TabIndex = 0;
            pnlCardSession.Tag = Color.FromArgb(106, 0, 0);
            pnlCardSession.Paint += Card_Paint;
            // 
            // lblSessionAttLabel
            // 
            lblSessionAttLabel.AutoSize = true;
            lblSessionAttLabel.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblSessionAttLabel.ForeColor = Color.FromArgb(106, 0, 0);
            lblSessionAttLabel.Location = new Point(10, 6);
            lblSessionAttLabel.Name = "lblSessionAttLabel";
            lblSessionAttLabel.Size = new Size(98, 13);
            lblSessionAttLabel.TabIndex = 0;
            lblSessionAttLabel.Text = "Session Summary";
            // 
            // panel21
            // 
            panel21.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel21.BackColor = Color.Transparent;
            panel21.Location = new Point(4, 22);
            panel21.Name = "panel21";
            panel21.Size = new Size(1769, 104);
            panel21.TabIndex = 1;
            // 
            // pnlCardPresent
            // 
            pnlCardPresent.BackColor = Color.White;
            pnlCardPresent.Controls.Add(lblPresentTitle);
            pnlCardPresent.Controls.Add(lblPresentNum);
            pnlCardPresent.Controls.Add(lblPresentPct);
            pnlCardPresent.Location = new Point(0, 0);
            pnlCardPresent.Margin = new Padding(4);
            pnlCardPresent.Name = "pnlCardPresent";
            pnlCardPresent.Size = new Size(200, 100);
            pnlCardPresent.TabIndex = 1;
            pnlCardPresent.Tag = Color.FromArgb(34, 139, 34);
            pnlCardPresent.Paint += Card_Paint;
            // 
            // lblPresentTitle
            // 
            lblPresentTitle.AutoSize = true;
            lblPresentTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblPresentTitle.ForeColor = Color.FromArgb(34, 139, 34);
            lblPresentTitle.Location = new Point(10, 12);
            lblPresentTitle.Name = "lblPresentTitle";
            lblPresentTitle.Size = new Size(50, 15);
            lblPresentTitle.TabIndex = 0;
            lblPresentTitle.Text = "Present";
            // 
            // lblPresentNum
            // 
            lblPresentNum.AutoSize = true;
            lblPresentNum.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lblPresentNum.ForeColor = Color.FromArgb(34, 139, 34);
            lblPresentNum.Location = new Point(10, 30);
            lblPresentNum.Name = "lblPresentNum";
            lblPresentNum.Size = new Size(40, 47);
            lblPresentNum.TabIndex = 1;
            lblPresentNum.Text = "0";
            // 
            // lblPresentPct
            // 
            lblPresentPct.AutoSize = true;
            lblPresentPct.Font = new Font("Segoe UI", 8F);
            lblPresentPct.ForeColor = Color.FromArgb(120, 120, 120);
            lblPresentPct.Location = new Point(10, 84);
            lblPresentPct.Name = "lblPresentPct";
            lblPresentPct.Size = new Size(22, 13);
            lblPresentPct.TabIndex = 2;
            lblPresentPct.Text = "0%";
            // 
            // pnlCardLate
            // 
            pnlCardLate.BackColor = Color.White;
            pnlCardLate.Controls.Add(lblLateTitle);
            pnlCardLate.Controls.Add(lblLateNum);
            pnlCardLate.Controls.Add(lblLatePct);
            pnlCardLate.Location = new Point(0, 0);
            pnlCardLate.Margin = new Padding(4);
            pnlCardLate.Name = "pnlCardLate";
            pnlCardLate.Size = new Size(200, 100);
            pnlCardLate.TabIndex = 2;
            pnlCardLate.Tag = Color.FromArgb(200, 110, 0);
            pnlCardLate.Paint += Card_Paint;
            // 
            // lblLateTitle
            // 
            lblLateTitle.AutoSize = true;
            lblLateTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblLateTitle.ForeColor = Color.FromArgb(200, 110, 0);
            lblLateTitle.Location = new Point(10, 12);
            lblLateTitle.Name = "lblLateTitle";
            lblLateTitle.Size = new Size(31, 15);
            lblLateTitle.TabIndex = 0;
            lblLateTitle.Text = "Late";
            // 
            // lblLateNum
            // 
            lblLateNum.AutoSize = true;
            lblLateNum.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lblLateNum.ForeColor = Color.FromArgb(200, 110, 0);
            lblLateNum.Location = new Point(10, 30);
            lblLateNum.Name = "lblLateNum";
            lblLateNum.Size = new Size(40, 47);
            lblLateNum.TabIndex = 1;
            lblLateNum.Text = "0";
            // 
            // lblLatePct
            // 
            lblLatePct.AutoSize = true;
            lblLatePct.Font = new Font("Segoe UI", 8F);
            lblLatePct.ForeColor = Color.FromArgb(120, 120, 120);
            lblLatePct.Location = new Point(10, 84);
            lblLatePct.Name = "lblLatePct";
            lblLatePct.Size = new Size(22, 13);
            lblLatePct.TabIndex = 2;
            lblLatePct.Text = "0%";
            // 
            // pnlCardAbsent
            // 
            pnlCardAbsent.BackColor = Color.White;
            pnlCardAbsent.Controls.Add(lblAbsentTitle);
            pnlCardAbsent.Controls.Add(lblAbsentNum);
            pnlCardAbsent.Controls.Add(lblAbsentPct);
            pnlCardAbsent.Location = new Point(0, 0);
            pnlCardAbsent.Margin = new Padding(4);
            pnlCardAbsent.Name = "pnlCardAbsent";
            pnlCardAbsent.Size = new Size(200, 100);
            pnlCardAbsent.TabIndex = 3;
            pnlCardAbsent.Tag = Color.FromArgb(210, 40, 40);
            pnlCardAbsent.Paint += Card_Paint;
            // 
            // lblAbsentTitle
            // 
            lblAbsentTitle.AutoSize = true;
            lblAbsentTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblAbsentTitle.ForeColor = Color.FromArgb(210, 40, 40);
            lblAbsentTitle.Location = new Point(10, 12);
            lblAbsentTitle.Name = "lblAbsentTitle";
            lblAbsentTitle.Size = new Size(46, 15);
            lblAbsentTitle.TabIndex = 0;
            lblAbsentTitle.Text = "Absent";
            // 
            // lblAbsentNum
            // 
            lblAbsentNum.AutoSize = true;
            lblAbsentNum.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lblAbsentNum.ForeColor = Color.FromArgb(210, 40, 40);
            lblAbsentNum.Location = new Point(10, 30);
            lblAbsentNum.Name = "lblAbsentNum";
            lblAbsentNum.Size = new Size(40, 47);
            lblAbsentNum.TabIndex = 1;
            lblAbsentNum.Text = "0";
            // 
            // lblAbsentPct
            // 
            lblAbsentPct.AutoSize = true;
            lblAbsentPct.Font = new Font("Segoe UI", 8F);
            lblAbsentPct.ForeColor = Color.FromArgb(120, 120, 120);
            lblAbsentPct.Location = new Point(10, 84);
            lblAbsentPct.Name = "lblAbsentPct";
            lblAbsentPct.Size = new Size(22, 13);
            lblAbsentPct.TabIndex = 2;
            lblAbsentPct.Text = "0%";
            // 
            // pnlCardExcused
            // 
            pnlCardExcused.BackColor = Color.White;
            pnlCardExcused.Controls.Add(lblExcusedTitle);
            pnlCardExcused.Controls.Add(lblExcusedNum);
            pnlCardExcused.Controls.Add(lblExcusedPct);
            pnlCardExcused.Location = new Point(0, 0);
            pnlCardExcused.Margin = new Padding(4);
            pnlCardExcused.Name = "pnlCardExcused";
            pnlCardExcused.Size = new Size(200, 100);
            pnlCardExcused.TabIndex = 4;
            pnlCardExcused.Tag = Color.FromArgb(180, 140, 0);
            pnlCardExcused.Paint += Card_Paint;
            // 
            // lblExcusedTitle
            // 
            lblExcusedTitle.AutoSize = true;
            lblExcusedTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblExcusedTitle.ForeColor = Color.FromArgb(180, 140, 0);
            lblExcusedTitle.Location = new Point(10, 12);
            lblExcusedTitle.Name = "lblExcusedTitle";
            lblExcusedTitle.Size = new Size(52, 15);
            lblExcusedTitle.TabIndex = 0;
            lblExcusedTitle.Text = "Excused";
            // 
            // lblExcusedNum
            // 
            lblExcusedNum.AutoSize = true;
            lblExcusedNum.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lblExcusedNum.ForeColor = Color.FromArgb(180, 140, 0);
            lblExcusedNum.Location = new Point(10, 30);
            lblExcusedNum.Name = "lblExcusedNum";
            lblExcusedNum.Size = new Size(40, 47);
            lblExcusedNum.TabIndex = 1;
            lblExcusedNum.Text = "0";
            // 
            // lblExcusedPct
            // 
            lblExcusedPct.AutoSize = true;
            lblExcusedPct.Font = new Font("Segoe UI", 8F);
            lblExcusedPct.ForeColor = Color.FromArgb(120, 120, 120);
            lblExcusedPct.Location = new Point(10, 84);
            lblExcusedPct.Name = "lblExcusedPct";
            lblExcusedPct.Size = new Size(22, 13);
            lblExcusedPct.TabIndex = 2;
            lblExcusedPct.Text = "0%";
            // 
            // pnlCardLastUpdate
            // 
            pnlCardLastUpdate.BackColor = Color.White;
            pnlCardLastUpdate.Controls.Add(lblLastUpdate);
            pnlCardLastUpdate.Controls.Add(lblDateTime);
            pnlCardLastUpdate.Controls.Add(lblByInstructor);
            pnlCardLastUpdate.Location = new Point(0, 0);
            pnlCardLastUpdate.Margin = new Padding(4);
            pnlCardLastUpdate.Name = "pnlCardLastUpdate";
            pnlCardLastUpdate.Size = new Size(200, 100);
            pnlCardLastUpdate.TabIndex = 5;
            pnlCardLastUpdate.Tag = Color.FromArgb(106, 0, 0);
            pnlCardLastUpdate.Paint += Card_Paint;
            // 
            // lblLastUpdate
            // 
            lblLastUpdate.AutoSize = true;
            lblLastUpdate.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblLastUpdate.ForeColor = Color.FromArgb(106, 0, 0);
            lblLastUpdate.Location = new Point(10, 12);
            lblLastUpdate.Name = "lblLastUpdate";
            lblLastUpdate.Size = new Size(80, 15);
            lblLastUpdate.TabIndex = 0;
            lblLastUpdate.Text = "Last Updated";
            // 
            // lblDateTime
            // 
            lblDateTime.AutoSize = true;
            lblDateTime.Font = new Font("Segoe UI", 8F);
            lblDateTime.ForeColor = Color.FromArgb(70, 70, 70);
            lblDateTime.Location = new Point(10, 32);
            lblDateTime.Name = "lblDateTime";
            lblDateTime.Size = new Size(18, 13);
            lblDateTime.TabIndex = 1;
            lblDateTime.Text = "—";
            // 
            // lblByInstructor
            // 
            lblByInstructor.AutoSize = true;
            lblByInstructor.Font = new Font("Segoe UI", 7.5F);
            lblByInstructor.ForeColor = Color.Gray;
            lblByInstructor.Location = new Point(10, 68);
            lblByInstructor.Name = "lblByInstructor";
            lblByInstructor.Size = new Size(61, 12);
            lblByInstructor.TabIndex = 2;
            lblByInstructor.Text = "by Instructor";
            // 
            // pnlFilterBar
            // 
            pnlFilterBar.BackColor = Color.White;
            pnlFilterBar.Controls.Add(lblCourseLabel);
            pnlFilterBar.Controls.Add(cmbCourse);
            pnlFilterBar.Controls.Add(lblDateLabel);
            pnlFilterBar.Controls.Add(dtpDate);
            pnlFilterBar.Controls.Add(lblSessionLabel);
            pnlFilterBar.Controls.Add(cmbSession);
            pnlFilterBar.Controls.Add(txtSearch);
            pnlFilterBar.Controls.Add(btnQRCode);
            pnlFilterBar.Controls.Add(btnImportCSV);
            pnlFilterBar.Controls.Add(btnExport);
            pnlFilterBar.Dock = DockStyle.Top;
            pnlFilterBar.Location = new Point(0, 48);
            pnlFilterBar.Name = "pnlFilterBar";
            pnlFilterBar.Padding = new Padding(14, 0, 14, 0);
            pnlFilterBar.Size = new Size(1649, 52);
            pnlFilterBar.TabIndex = 3;
            // 
            // lblCourseLabel
            // 
            lblCourseLabel.AutoSize = true;
            lblCourseLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblCourseLabel.ForeColor = Color.FromArgb(80, 80, 80);
            lblCourseLabel.Location = new Point(14, 8);
            lblCourseLabel.Name = "lblCourseLabel";
            lblCourseLabel.Size = new Size(106, 15);
            lblCourseLabel.TabIndex = 0;
            lblCourseLabel.Text = "Course && Section:";
            // 
            // cmbCourse
            // 
            cmbCourse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCourse.Font = new Font("Segoe UI", 9F);
            cmbCourse.Location = new Point(14, 24);
            cmbCourse.Name = "cmbCourse";
            cmbCourse.Size = new Size(290, 23);
            cmbCourse.TabIndex = 0;
            // 
            // lblDateLabel
            // 
            lblDateLabel.AutoSize = true;
            lblDateLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblDateLabel.ForeColor = Color.FromArgb(80, 80, 80);
            lblDateLabel.Location = new Point(316, 8);
            lblDateLabel.Name = "lblDateLabel";
            lblDateLabel.Size = new Size(37, 15);
            lblDateLabel.TabIndex = 1;
            lblDateLabel.Text = "Date:";
            // 
            // dtpDate
            // 
            dtpDate.Font = new Font("Segoe UI", 9F);
            dtpDate.Format = DateTimePickerFormat.Short;
            dtpDate.Location = new Point(316, 24);
            dtpDate.Name = "dtpDate";
            dtpDate.Size = new Size(130, 23);
            dtpDate.TabIndex = 1;
            // 
            // lblSessionLabel
            // 
            lblSessionLabel.AutoSize = true;
            lblSessionLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblSessionLabel.ForeColor = Color.FromArgb(80, 80, 80);
            lblSessionLabel.Location = new Point(458, 8);
            lblSessionLabel.Name = "lblSessionLabel";
            lblSessionLabel.Size = new Size(51, 15);
            lblSessionLabel.TabIndex = 2;
            lblSessionLabel.Text = "Session:";
            // 
            // cmbSession
            // 
            cmbSession.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSession.Font = new Font("Segoe UI", 9F);
            cmbSession.Location = new Point(458, 24);
            cmbSession.Name = "cmbSession";
            cmbSession.Size = new Size(240, 23);
            cmbSession.TabIndex = 2;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 9F);
            txtSearch.ForeColor = Color.Gray;
            txtSearch.Location = new Point(712, 24);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(200, 23);
            txtSearch.TabIndex = 3;
            txtSearch.Text = "Search student…";
            // 
            // btnQRCode
            // 
            btnQRCode.BackColor = Color.FromArgb(106, 0, 0);
            btnQRCode.Cursor = Cursors.Hand;
            btnQRCode.FlatAppearance.BorderSize = 0;
            btnQRCode.FlatStyle = FlatStyle.Flat;
            btnQRCode.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnQRCode.ForeColor = Color.White;
            btnQRCode.Location = new Point(924, 19);
            btnQRCode.Name = "btnQRCode";
            btnQRCode.Size = new Size(140, 28);
            btnQRCode.TabIndex = 4;
            btnQRCode.Text = "⊞  QR Attendance";
            btnQRCode.UseVisualStyleBackColor = false;
            // 
            // btnImportCSV
            // 
            btnImportCSV.Cursor = Cursors.Hand;
            btnImportCSV.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnImportCSV.FlatStyle = FlatStyle.Flat;
            btnImportCSV.Font = new Font("Segoe UI", 8.5F);
            btnImportCSV.Location = new Point(1426, 21);
            btnImportCSV.Name = "btnImportCSV";
            btnImportCSV.Size = new Size(100, 28);
            btnImportCSV.TabIndex = 5;
            btnImportCSV.Text = "Import CSV";
            // 
            // btnExport
            // 
            btnExport.Cursor = Cursors.Hand;
            btnExport.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Font = new Font("Segoe UI", 8.5F);
            btnExport.Location = new Point(1532, 20);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(100, 28);
            btnExport.TabIndex = 6;
            btnExport.Text = "Export CSV";
            // 
            // pnlTitleBar
            // 
            pnlTitleBar.BackColor = Color.FromArgb(106, 0, 0);
            pnlTitleBar.Controls.Add(lblAttendanceTitle);
            pnlTitleBar.Dock = DockStyle.Top;
            pnlTitleBar.Location = new Point(0, 0);
            pnlTitleBar.Name = "pnlTitleBar";
            pnlTitleBar.Padding = new Padding(16, 0, 0, 0);
            pnlTitleBar.Size = new Size(1649, 48);
            pnlTitleBar.TabIndex = 4;
            // 
            // lblAttendanceTitle
            // 
            lblAttendanceTitle.AutoSize = true;
            lblAttendanceTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblAttendanceTitle.ForeColor = Color.White;
            lblAttendanceTitle.Location = new Point(16, 11);
            lblAttendanceTitle.Name = "lblAttendanceTitle";
            lblAttendanceTitle.Size = new Size(162, 25);
            lblAttendanceTitle.TabIndex = 0;
            lblAttendanceTitle.Text = "Class Attendance";
            // 
            // AttendanceContentInst
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlRoot);
            Name = "AttendanceContentInst";
            Size = new Size(1649, 989);
            Load += AttendanceContentInst_Load;
            pnlRoot.ResumeLayout(false);
            pnlActionBar.ResumeLayout(false);
            pnlSummaryRow.ResumeLayout(false);
            pnlCardSession.ResumeLayout(false);
            pnlCardSession.PerformLayout();
            pnlCardPresent.ResumeLayout(false);
            pnlCardPresent.PerformLayout();
            pnlCardLate.ResumeLayout(false);
            pnlCardLate.PerformLayout();
            pnlCardAbsent.ResumeLayout(false);
            pnlCardAbsent.PerformLayout();
            pnlCardExcused.ResumeLayout(false);
            pnlCardExcused.PerformLayout();
            pnlCardLastUpdate.ResumeLayout(false);
            pnlCardLastUpdate.PerformLayout();
            pnlFilterBar.ResumeLayout(false);
            pnlFilterBar.PerformLayout();
            pnlTitleBar.ResumeLayout(false);
            pnlTitleBar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlRoot;
        private System.Windows.Forms.Panel pnlTitleBar;
        private System.Windows.Forms.Label lblAttendanceTitle;
        private System.Windows.Forms.Panel pnlFilterBar;
        private System.Windows.Forms.Label lblCourseLabel;
        private System.Windows.Forms.ComboBox cmbCourse;
        private System.Windows.Forms.Label lblDateLabel;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label lblSessionLabel;
        private System.Windows.Forms.ComboBox cmbSession;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnQRCode;
        private System.Windows.Forms.Button btnImportCSV;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Panel pnlSummaryRow;
        private System.Windows.Forms.Panel pnlCardSession;
        private System.Windows.Forms.Panel panel21;
        private System.Windows.Forms.Label lblSessionAttLabel;
        private System.Windows.Forms.Panel pnlCardPresent;
        private System.Windows.Forms.Label lblPresentTitle;
        private System.Windows.Forms.Label lblPresentNum;
        private System.Windows.Forms.Label lblPresentPct;
        private System.Windows.Forms.Panel pnlCardLate;
        private System.Windows.Forms.Label lblLateTitle;
        private System.Windows.Forms.Label lblLateNum;
        private System.Windows.Forms.Label lblLatePct;
        private System.Windows.Forms.Panel pnlCardAbsent;
        private System.Windows.Forms.Label lblAbsentTitle;
        private System.Windows.Forms.Label lblAbsentNum;
        private System.Windows.Forms.Label lblAbsentPct;
        private System.Windows.Forms.Panel pnlCardExcused;
        private System.Windows.Forms.Label lblExcusedTitle;
        private System.Windows.Forms.Label lblExcusedNum;
        private System.Windows.Forms.Label lblExcusedPct;
        private System.Windows.Forms.Panel pnlCardLastUpdate;
        private System.Windows.Forms.Label lblLastUpdate;
        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.Label lblByInstructor;
        private System.Windows.Forms.Panel pnlActionBar;
        private System.Windows.Forms.Button btnSaveAttendance;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel pnlGrid;
    }
}