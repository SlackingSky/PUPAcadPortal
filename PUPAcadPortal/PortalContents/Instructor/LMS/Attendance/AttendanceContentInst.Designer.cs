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
            this.pnlRoot = new System.Windows.Forms.Panel();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.pnlActionBar = new System.Windows.Forms.Panel();
            this.btnSaveAttendance = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.pnlSummaryRow = new System.Windows.Forms.Panel();
            this.pnlCardSession = new System.Windows.Forms.Panel();
            this.lblSessionAttLabel = new System.Windows.Forms.Label();
            this.panel21 = new System.Windows.Forms.Panel();
            this.pnlCardPresent = new System.Windows.Forms.Panel();
            this.lblPresentTitle = new System.Windows.Forms.Label();
            this.lblPresentNum = new System.Windows.Forms.Label();
            this.lblPresentPct = new System.Windows.Forms.Label();
            this.pnlCardLate = new System.Windows.Forms.Panel();
            this.lblLateTitle = new System.Windows.Forms.Label();
            this.lblLateNum = new System.Windows.Forms.Label();
            this.lblLatePct = new System.Windows.Forms.Label();
            this.pnlCardAbsent = new System.Windows.Forms.Panel();
            this.lblAbsentTitle = new System.Windows.Forms.Label();
            this.lblAbsentNum = new System.Windows.Forms.Label();
            this.lblAbsentPct = new System.Windows.Forms.Label();
            this.pnlCardExcused = new System.Windows.Forms.Panel();
            this.lblExcusedTitle = new System.Windows.Forms.Label();
            this.lblExcusedNum = new System.Windows.Forms.Label();
            this.lblExcusedPct = new System.Windows.Forms.Label();
            this.pnlCardLastUpdate = new System.Windows.Forms.Panel();
            this.lblLastUpdate = new System.Windows.Forms.Label();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.lblByInstructor = new System.Windows.Forms.Label();
            this.pnlFilterBar = new System.Windows.Forms.Panel();
            this.lblCourseLabel = new System.Windows.Forms.Label();
            this.cmbCourse = new System.Windows.Forms.ComboBox();
            this.lblDateLabel = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.lblSessionLabel = new System.Windows.Forms.Label();
            this.cmbSession = new System.Windows.Forms.ComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnQRCode = new System.Windows.Forms.Button();
            this.btnImportCSV = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.pnlTitleBar = new System.Windows.Forms.Panel();
            this.lblAttendanceTitle = new System.Windows.Forms.Label();

            this.pnlRoot.SuspendLayout();
            this.pnlActionBar.SuspendLayout();
            this.pnlSummaryRow.SuspendLayout();
            this.pnlCardSession.SuspendLayout();
            this.pnlCardPresent.SuspendLayout();
            this.pnlCardLate.SuspendLayout();
            this.pnlCardAbsent.SuspendLayout();
            this.pnlCardExcused.SuspendLayout();
            this.pnlCardLastUpdate.SuspendLayout();
            this.pnlFilterBar.SuspendLayout();
            this.pnlTitleBar.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlRoot
            // 
            this.pnlRoot.BackColor = System.Drawing.Color.FromArgb(245, 245, 248);
            this.pnlRoot.Controls.Add(this.pnlGrid);
            this.pnlRoot.Controls.Add(this.pnlActionBar);
            this.pnlRoot.Controls.Add(this.pnlSummaryRow);
            this.pnlRoot.Controls.Add(this.pnlFilterBar);
            this.pnlRoot.Controls.Add(this.pnlTitleBar);
            this.pnlRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRoot.Location = new System.Drawing.Point(0, 0);
            this.pnlRoot.Margin = new System.Windows.Forms.Padding(0);
            this.pnlRoot.Name = "pnlRoot";
            this.pnlRoot.Size = new System.Drawing.Size(1400, 900);

            // 
            // pnlTitleBar
            // 
            this.pnlTitleBar.BackColor = System.Drawing.Color.FromArgb(106, 0, 0);
            this.pnlTitleBar.Controls.Add(this.lblAttendanceTitle);
            this.pnlTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitleBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTitleBar.Name = "pnlTitleBar";
            this.pnlTitleBar.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.pnlTitleBar.Size = new System.Drawing.Size(1400, 48);

            // 
            // lblAttendanceTitle
            // 
            this.lblAttendanceTitle.AutoSize = true;
            this.lblAttendanceTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblAttendanceTitle.ForeColor = System.Drawing.Color.White;
            this.lblAttendanceTitle.Location = new System.Drawing.Point(16, 11);
            this.lblAttendanceTitle.Name = "lblAttendanceTitle";
            this.lblAttendanceTitle.Text = "Class Attendance";

            // 
            // pnlFilterBar
            // 
            this.pnlFilterBar.BackColor = System.Drawing.Color.White;
            this.pnlFilterBar.Controls.Add(this.lblCourseLabel);
            this.pnlFilterBar.Controls.Add(this.cmbCourse);
            this.pnlFilterBar.Controls.Add(this.lblDateLabel);
            this.pnlFilterBar.Controls.Add(this.dtpDate);
            this.pnlFilterBar.Controls.Add(this.lblSessionLabel);
            this.pnlFilterBar.Controls.Add(this.cmbSession);
            this.pnlFilterBar.Controls.Add(this.txtSearch);
            this.pnlFilterBar.Controls.Add(this.btnQRCode);
            this.pnlFilterBar.Controls.Add(this.btnImportCSV);
            this.pnlFilterBar.Controls.Add(this.btnExport);
            this.pnlFilterBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilterBar.Location = new System.Drawing.Point(0, 48);
            this.pnlFilterBar.Name = "pnlFilterBar";
            this.pnlFilterBar.Padding = new System.Windows.Forms.Padding(14, 0, 14, 0);
            this.pnlFilterBar.Size = new System.Drawing.Size(1400, 52);

            // 
            // lblCourseLabel
            // 
            this.lblCourseLabel.AutoSize = true;
            this.lblCourseLabel.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblCourseLabel.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            this.lblCourseLabel.Location = new System.Drawing.Point(14, 8);
            this.lblCourseLabel.Name = "lblCourseLabel";
            this.lblCourseLabel.Text = "Course && Section:";

            // 
            // cmbCourse
            // 
            this.cmbCourse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCourse.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbCourse.Location = new System.Drawing.Point(14, 24);
            this.cmbCourse.Name = "cmbCourse";
            this.cmbCourse.Size = new System.Drawing.Size(290, 23);
            this.cmbCourse.TabIndex = 0;

            // 
            // lblDateLabel
            // 
            this.lblDateLabel.AutoSize = true;
            this.lblDateLabel.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblDateLabel.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            this.lblDateLabel.Location = new System.Drawing.Point(316, 8);
            this.lblDateLabel.Name = "lblDateLabel";
            this.lblDateLabel.Text = "Date:";

            // 
            // dtpDate
            // 
            this.dtpDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(316, 24);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(130, 23);
            this.dtpDate.TabIndex = 1;

            // 
            // lblSessionLabel
            // 
            this.lblSessionLabel.AutoSize = true;
            this.lblSessionLabel.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblSessionLabel.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            this.lblSessionLabel.Location = new System.Drawing.Point(458, 8);
            this.lblSessionLabel.Name = "lblSessionLabel";
            this.lblSessionLabel.Text = "Session:";

            // 
            // cmbSession
            // 
            this.cmbSession.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSession.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbSession.Location = new System.Drawing.Point(458, 24);
            this.cmbSession.Name = "cmbSession";
            this.cmbSession.Size = new System.Drawing.Size(240, 23);
            this.cmbSession.TabIndex = 2;

            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSearch.ForeColor = System.Drawing.Color.Gray;
            this.txtSearch.Location = new System.Drawing.Point(712, 24);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 23);
            this.txtSearch.TabIndex = 3;
            this.txtSearch.Text = "Search student…";

            // 
            // btnQRCode
            // 
            this.btnQRCode.BackColor = System.Drawing.Color.FromArgb(106, 0, 0);
            this.btnQRCode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnQRCode.FlatAppearance.BorderSize = 0;
            this.btnQRCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQRCode.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnQRCode.ForeColor = System.Drawing.Color.White;
            this.btnQRCode.Location = new System.Drawing.Point(924, 19);
            this.btnQRCode.Name = "btnQRCode";
            this.btnQRCode.Size = new System.Drawing.Size(140, 28);
            this.btnQRCode.TabIndex = 4;
            this.btnQRCode.Text = "⊞  QR Attendance";
            this.btnQRCode.UseVisualStyleBackColor = false;

            // 
            // btnImportCSV
            // 
            this.btnImportCSV.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnImportCSV.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            this.btnImportCSV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportCSV.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.btnImportCSV.Location = new System.Drawing.Point(1076, 19);
            this.btnImportCSV.Name = "btnImportCSV";
            this.btnImportCSV.Size = new System.Drawing.Size(100, 28);
            this.btnImportCSV.TabIndex = 5;
            this.btnImportCSV.Text = "Import CSV";

            // 
            // btnExport
            // 
            this.btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExport.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.btnExport.Location = new System.Drawing.Point(1184, 19);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 28);
            this.btnExport.TabIndex = 6;
            this.btnExport.Text = "Export CSV";

            // 
            // pnlSummaryRow
            // 
            this.pnlSummaryRow.BackColor = System.Drawing.Color.FromArgb(245, 245, 248);
            this.pnlSummaryRow.Controls.Add(this.pnlCardSession);
            this.pnlSummaryRow.Controls.Add(this.pnlCardPresent);
            this.pnlSummaryRow.Controls.Add(this.pnlCardLate);
            this.pnlSummaryRow.Controls.Add(this.pnlCardAbsent);
            this.pnlSummaryRow.Controls.Add(this.pnlCardExcused);
            this.pnlSummaryRow.Controls.Add(this.pnlCardLastUpdate);
            this.pnlSummaryRow.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSummaryRow.Location = new System.Drawing.Point(0, 100);
            this.pnlSummaryRow.Name = "pnlSummaryRow";
            this.pnlSummaryRow.Padding = new System.Windows.Forms.Padding(10, 8, 10, 4);
            this.pnlSummaryRow.Size = new System.Drawing.Size(1400, 120);
            this.pnlSummaryRow.SizeChanged += new System.EventHandler(this.PnlSummaryRow_SizeChanged);

            // 
            // pnlCardSession
            // 
            this.pnlCardSession.BackColor = System.Drawing.Color.White;
            this.pnlCardSession.Controls.Add(this.lblSessionAttLabel);
            this.pnlCardSession.Controls.Add(this.panel21);
            this.pnlCardSession.Margin = new System.Windows.Forms.Padding(4);
            this.pnlCardSession.Name = "pnlCardSession";
            this.pnlCardSession.Tag = System.Drawing.Color.FromArgb(106, 0, 0);
            this.pnlCardSession.Paint += new System.Windows.Forms.PaintEventHandler(this.Card_Paint);

            // 
            // lblSessionAttLabel
            // 
            this.lblSessionAttLabel.AutoSize = true;
            this.lblSessionAttLabel.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblSessionAttLabel.ForeColor = System.Drawing.Color.FromArgb(106, 0, 0);
            this.lblSessionAttLabel.Location = new System.Drawing.Point(10, 6);
            this.lblSessionAttLabel.Name = "lblSessionAttLabel";
            this.lblSessionAttLabel.Text = "Session Summary";

            // 
            // panel21
            // 
            this.panel21.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel21.BackColor = System.Drawing.Color.Transparent;
            this.panel21.Location = new System.Drawing.Point(4, 22);
            this.panel21.Name = "panel21";
            this.panel21.Size = new System.Drawing.Size(320, 78);

            // 
            // pnlCardPresent
            // 
            this.pnlCardPresent.BackColor = System.Drawing.Color.White;
            this.pnlCardPresent.Controls.Add(this.lblPresentTitle);
            this.pnlCardPresent.Controls.Add(this.lblPresentNum);
            this.pnlCardPresent.Controls.Add(this.lblPresentPct);
            this.pnlCardPresent.Margin = new System.Windows.Forms.Padding(4);
            this.pnlCardPresent.Name = "pnlCardPresent";
            this.pnlCardPresent.Tag = System.Drawing.Color.FromArgb(34, 139, 34);
            this.pnlCardPresent.Paint += new System.Windows.Forms.PaintEventHandler(this.Card_Paint);

            // 
            // lblPresentTitle
            // 
            this.lblPresentTitle.AutoSize = true;
            this.lblPresentTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPresentTitle.ForeColor = System.Drawing.Color.FromArgb(34, 139, 34);
            this.lblPresentTitle.Location = new System.Drawing.Point(10, 12);
            this.lblPresentTitle.Name = "lblPresentTitle";
            this.lblPresentTitle.Text = "Present";

            // 
            // lblPresentNum
            // 
            this.lblPresentNum.AutoSize = true;
            this.lblPresentNum.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblPresentNum.ForeColor = System.Drawing.Color.FromArgb(34, 139, 34);
            this.lblPresentNum.Location = new System.Drawing.Point(10, 30);
            this.lblPresentNum.Name = "lblPresentNum";
            this.lblPresentNum.Text = "0";

            // 
            // lblPresentPct
            // 
            this.lblPresentPct.AutoSize = true;
            this.lblPresentPct.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblPresentPct.ForeColor = System.Drawing.Color.FromArgb(120, 120, 120);
            this.lblPresentPct.Location = new System.Drawing.Point(10, 84);
            this.lblPresentPct.Name = "lblPresentPct";
            this.lblPresentPct.Text = "0%";

            // 
            // pnlCardLate
            // 
            this.pnlCardLate.BackColor = System.Drawing.Color.White;
            this.pnlCardLate.Controls.Add(this.lblLateTitle);
            this.pnlCardLate.Controls.Add(this.lblLateNum);
            this.pnlCardLate.Controls.Add(this.lblLatePct);
            this.pnlCardLate.Margin = new System.Windows.Forms.Padding(4);
            this.pnlCardLate.Name = "pnlCardLate";
            this.pnlCardLate.Tag = System.Drawing.Color.FromArgb(200, 110, 0);
            this.pnlCardLate.Paint += new System.Windows.Forms.PaintEventHandler(this.Card_Paint);

            // 
            // lblLateTitle
            // 
            this.lblLateTitle.AutoSize = true;
            this.lblLateTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblLateTitle.ForeColor = System.Drawing.Color.FromArgb(200, 110, 0);
            this.lblLateTitle.Location = new System.Drawing.Point(10, 12);
            this.lblLateTitle.Name = "lblLateTitle";
            this.lblLateTitle.Text = "Late";

            // 
            // lblLateNum
            // 
            this.lblLateNum.AutoSize = true;
            this.lblLateNum.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblLateNum.ForeColor = System.Drawing.Color.FromArgb(200, 110, 0);
            this.lblLateNum.Location = new System.Drawing.Point(10, 30);
            this.lblLateNum.Name = "lblLateNum";
            this.lblLateNum.Text = "0";

            // 
            // lblLatePct
            // 
            this.lblLatePct.AutoSize = true;
            this.lblLatePct.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblLatePct.ForeColor = System.Drawing.Color.FromArgb(120, 120, 120);
            this.lblLatePct.Location = new System.Drawing.Point(10, 84);
            this.lblLatePct.Name = "lblLatePct";
            this.lblLatePct.Text = "0%";

            // 
            // pnlCardAbsent
            // 
            this.pnlCardAbsent.BackColor = System.Drawing.Color.White;
            this.pnlCardAbsent.Controls.Add(this.lblAbsentTitle);
            this.pnlCardAbsent.Controls.Add(this.lblAbsentNum);
            this.pnlCardAbsent.Controls.Add(this.lblAbsentPct);
            this.pnlCardAbsent.Margin = new System.Windows.Forms.Padding(4);
            this.pnlCardAbsent.Name = "pnlCardAbsent";
            this.pnlCardAbsent.Tag = System.Drawing.Color.FromArgb(210, 40, 40);
            this.pnlCardAbsent.Paint += new System.Windows.Forms.PaintEventHandler(this.Card_Paint);

            // 
            // lblAbsentTitle
            // 
            this.lblAbsentTitle.AutoSize = true;
            this.lblAbsentTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblAbsentTitle.ForeColor = System.Drawing.Color.FromArgb(210, 40, 40);
            this.lblAbsentTitle.Location = new System.Drawing.Point(10, 12);
            this.lblAbsentTitle.Name = "lblAbsentTitle";
            this.lblAbsentTitle.Text = "Absent";

            // 
            // lblAbsentNum
            // 
            this.lblAbsentNum.AutoSize = true;
            this.lblAbsentNum.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblAbsentNum.ForeColor = System.Drawing.Color.FromArgb(210, 40, 40);
            this.lblAbsentNum.Location = new System.Drawing.Point(10, 30);
            this.lblAbsentNum.Name = "lblAbsentNum";
            this.lblAbsentNum.Text = "0";

            // 
            // lblAbsentPct
            // 
            this.lblAbsentPct.AutoSize = true;
            this.lblAbsentPct.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblAbsentPct.ForeColor = System.Drawing.Color.FromArgb(120, 120, 120);
            this.lblAbsentPct.Location = new System.Drawing.Point(10, 84);
            this.lblAbsentPct.Name = "lblAbsentPct";
            this.lblAbsentPct.Text = "0%";

            // 
            // pnlCardExcused
            // 
            this.pnlCardExcused.BackColor = System.Drawing.Color.White;
            this.pnlCardExcused.Controls.Add(this.lblExcusedTitle);
            this.pnlCardExcused.Controls.Add(this.lblExcusedNum);
            this.pnlCardExcused.Controls.Add(this.lblExcusedPct);
            this.pnlCardExcused.Margin = new System.Windows.Forms.Padding(4);
            this.pnlCardExcused.Name = "pnlCardExcused";
            this.pnlCardExcused.Tag = System.Drawing.Color.FromArgb(180, 140, 0);
            this.pnlCardExcused.Paint += new System.Windows.Forms.PaintEventHandler(this.Card_Paint);

            // 
            // lblExcusedTitle
            // 
            this.lblExcusedTitle.AutoSize = true;
            this.lblExcusedTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblExcusedTitle.ForeColor = System.Drawing.Color.FromArgb(180, 140, 0);
            this.lblExcusedTitle.Location = new System.Drawing.Point(10, 12);
            this.lblExcusedTitle.Name = "lblExcusedTitle";
            this.lblExcusedTitle.Text = "Excused";

            // 
            // lblExcusedNum
            // 
            this.lblExcusedNum.AutoSize = true;
            this.lblExcusedNum.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblExcusedNum.ForeColor = System.Drawing.Color.FromArgb(180, 140, 0);
            this.lblExcusedNum.Location = new System.Drawing.Point(10, 30);
            this.lblExcusedNum.Name = "lblExcusedNum";
            this.lblExcusedNum.Text = "0";

            // 
            // lblExcusedPct
            // 
            this.lblExcusedPct.AutoSize = true;
            this.lblExcusedPct.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblExcusedPct.ForeColor = System.Drawing.Color.FromArgb(120, 120, 120);
            this.lblExcusedPct.Location = new System.Drawing.Point(10, 84);
            this.lblExcusedPct.Name = "lblExcusedPct";
            this.lblExcusedPct.Text = "0%";

            // 
            // pnlCardLastUpdate
            // 
            this.pnlCardLastUpdate.BackColor = System.Drawing.Color.White;
            this.pnlCardLastUpdate.Controls.Add(this.lblLastUpdate);
            this.pnlCardLastUpdate.Controls.Add(this.lblDateTime);
            this.pnlCardLastUpdate.Controls.Add(this.lblByInstructor);
            this.pnlCardLastUpdate.Margin = new System.Windows.Forms.Padding(4);
            this.pnlCardLastUpdate.Name = "pnlCardLastUpdate";
            this.pnlCardLastUpdate.Tag = System.Drawing.Color.FromArgb(106, 0, 0);
            this.pnlCardLastUpdate.Paint += new System.Windows.Forms.PaintEventHandler(this.Card_Paint);

            // 
            // lblLastUpdate
            // 
            this.lblLastUpdate.AutoSize = true;
            this.lblLastUpdate.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblLastUpdate.ForeColor = System.Drawing.Color.FromArgb(106, 0, 0);
            this.lblLastUpdate.Location = new System.Drawing.Point(10, 12);
            this.lblLastUpdate.Name = "lblLastUpdate";
            this.lblLastUpdate.Text = "Last Updated";

            // 
            // lblDateTime
            // 
            this.lblDateTime.AutoSize = true;
            this.lblDateTime.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblDateTime.ForeColor = System.Drawing.Color.FromArgb(70, 70, 70);
            this.lblDateTime.Location = new System.Drawing.Point(10, 32);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Text = "—";

            // 
            // lblByInstructor
            // 
            this.lblByInstructor.AutoSize = true;
            this.lblByInstructor.Font = new System.Drawing.Font("Segoe UI", 7.5F);
            this.lblByInstructor.ForeColor = System.Drawing.Color.Gray;
            this.lblByInstructor.Location = new System.Drawing.Point(10, 68);
            this.lblByInstructor.Name = "lblByInstructor";
            this.lblByInstructor.Text = "by Instructor";

            // 
            // pnlActionBar
            // 
            this.pnlActionBar.BackColor = System.Drawing.Color.White;
            this.pnlActionBar.Controls.Add(this.btnSaveAttendance);
            this.pnlActionBar.Controls.Add(this.btnRefresh);
            this.pnlActionBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlActionBar.Location = new System.Drawing.Point(0, 220);
            this.pnlActionBar.Name = "pnlActionBar";
            this.pnlActionBar.Size = new System.Drawing.Size(1400, 52);

            // 
            // btnSaveAttendance
            // 
            this.btnSaveAttendance.BackColor = System.Drawing.Color.FromArgb(106, 0, 0);
            this.btnSaveAttendance.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveAttendance.FlatAppearance.BorderSize = 0;
            this.btnSaveAttendance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveAttendance.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSaveAttendance.ForeColor = System.Drawing.Color.White;
            this.btnSaveAttendance.Location = new System.Drawing.Point(14, 11);
            this.btnSaveAttendance.Name = "btnSaveAttendance";
            this.btnSaveAttendance.Size = new System.Drawing.Size(160, 30);
            this.btnSaveAttendance.TabIndex = 0;
            this.btnSaveAttendance.Text = "✓  Save Attendance";
            this.btnSaveAttendance.UseVisualStyleBackColor = false;

            // 
            // btnRefresh
            // 
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.btnRefresh.Location = new System.Drawing.Point(184, 11);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 30);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "⟳  Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;

            // 
            // pnlGrid
            // 
            this.pnlGrid.BackColor = System.Drawing.Color.White;
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(0, 272);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(1400, 628);

            // 
            // AttendanceContentInst
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlRoot);
            this.Name = "AttendanceContentInst";
            this.Size = new System.Drawing.Size(1400, 900);
            this.Load += new System.EventHandler(this.AttendanceContentInst_Load);
            this.pnlTitleBar.ResumeLayout(false);
            this.pnlTitleBar.PerformLayout();
            this.pnlFilterBar.ResumeLayout(false);
            this.pnlFilterBar.PerformLayout();
            this.pnlSummaryRow.ResumeLayout(false);
            this.pnlCardSession.ResumeLayout(false);
            this.pnlCardSession.PerformLayout();
            this.pnlCardPresent.ResumeLayout(false);
            this.pnlCardPresent.PerformLayout();
            this.pnlCardLate.ResumeLayout(false);
            this.pnlCardLate.PerformLayout();
            this.pnlCardAbsent.ResumeLayout(false);
            this.pnlCardAbsent.PerformLayout();
            this.pnlCardExcused.ResumeLayout(false);
            this.pnlCardExcused.PerformLayout();
            this.pnlCardLastUpdate.ResumeLayout(false);
            this.pnlCardLastUpdate.PerformLayout();
            this.pnlActionBar.ResumeLayout(false);
            this.pnlRoot.ResumeLayout(false);
            this.ResumeLayout(false);
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