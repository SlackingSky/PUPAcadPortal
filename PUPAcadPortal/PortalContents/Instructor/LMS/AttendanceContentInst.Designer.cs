namespace PUPAcadPortal.PortalContents.Instructor.LMS
{
    partial class AttendanceContentInst
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            pnlAttendance = new Panel();
            panel24 = new Panel();
            textBox2 = new TextBox();
            btnSaveAttendance = new Button();
            btnRefresh = new Button();
            pnlGrid = new Panel();
            dgvAttendance = new DataGridView();
            pnlPagination = new Panel();
            btnPageNextDbl = new Button();
            btnPageNext = new Button();
            btnPage3 = new Button();
            btnPage2 = new Button();
            btnPage1 = new Button();
            btnPagePrev = new Button();
            btnPagePrevDbl = new Button();
            lblShowing = new Label();
            panel22 = new Panel();
            btnQRCode = new Button();
            cmbSession = new ComboBox();
            lblSession = new Label();
            btnImporttoCSV = new Button();
            dtpDate = new DateTimePicker();
            btnExport = new Button();
            lblDate = new Label();
            comboBox1 = new ComboBox();
            label5 = new Label();
            pnlControlAttendance = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel14 = new Panel();
            lblLastUpdate = new Label();
            lblDateTime = new Label();
            pictureBox26 = new PictureBox();
            lblByInstructor = new Label();
            panel17 = new Panel();
            lblExcusedPrecent = new Label();
            lblExcused = new Label();
            pictureBox27 = new PictureBox();
            lblExcusedNum = new Label();
            panel18 = new Panel();
            lblAbsentPercent = new Label();
            lblAbsent = new Label();
            pictureBox28 = new PictureBox();
            lblAbsentNum = new Label();
            panel20 = new Panel();
            lblPresent = new Label();
            lblPesentPercent = new Label();
            lblPresentNum = new Label();
            pictureBox29 = new PictureBox();
            panel21 = new Panel();
            lblLayoutAttendance = new Label();
            lblSessionAttendance = new Label();
            panel19 = new Panel();
            lblAttendanceTitle = new Label();
            pnlAttendance.SuspendLayout();
            panel24.SuspendLayout();
            pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAttendance).BeginInit();
            pnlPagination.SuspendLayout();
            panel22.SuspendLayout();
            pnlControlAttendance.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox26).BeginInit();
            panel17.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox27).BeginInit();
            panel18.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox28).BeginInit();
            panel20.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox29).BeginInit();
            panel21.SuspendLayout();
            panel19.SuspendLayout();
            SuspendLayout();
            // 
            // pnlAttendance
            // 
            pnlAttendance.AutoScroll = true;
            pnlAttendance.AutoScrollMinSize = new Size(500, 500);
            pnlAttendance.BackColor = SystemColors.Control;
            pnlAttendance.CausesValidation = false;
            pnlAttendance.Controls.Add(panel24);
            pnlAttendance.Controls.Add(pnlGrid);
            pnlAttendance.Controls.Add(panel22);
            pnlAttendance.Controls.Add(pnlControlAttendance);
            pnlAttendance.Controls.Add(panel19);
            pnlAttendance.Dock = DockStyle.Fill;
            pnlAttendance.Location = new Point(0, 0);
            pnlAttendance.Margin = new Padding(0);
            pnlAttendance.Name = "pnlAttendance";
            pnlAttendance.Size = new Size(1648, 969);
            pnlAttendance.TabIndex = 15;
            // 
            // panel24
            // 
            panel24.BackColor = Color.White;
            panel24.Controls.Add(textBox2);
            panel24.Controls.Add(btnSaveAttendance);
            panel24.Controls.Add(btnRefresh);
            panel24.Location = new Point(0, 212);
            panel24.Margin = new Padding(3, 2, 3, 2);
            panel24.Name = "panel24";
            panel24.Size = new Size(1648, 55);
            panel24.TabIndex = 38;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(29, 13);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(180, 23);
            textBox2.TabIndex = 16;
            textBox2.Text = "Search student...";
            // 
            // btnSaveAttendance
            // 
            btnSaveAttendance.BackColor = Color.Maroon;
            btnSaveAttendance.ForeColor = Color.White;
            btnSaveAttendance.Location = new Point(224, 8);
            btnSaveAttendance.Name = "btnSaveAttendance";
            btnSaveAttendance.Size = new Size(140, 32);
            btnSaveAttendance.TabIndex = 17;
            btnSaveAttendance.Text = "Save Attendance";
            btnSaveAttendance.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Location = new Point(1543, 12);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(93, 35);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            // 
            // pnlGrid
            // 
            pnlGrid.Controls.Add(dgvAttendance);
            pnlGrid.Controls.Add(pnlPagination);
            pnlGrid.Controls.Add(lblShowing);
            pnlGrid.Location = new Point(0, 266);
            pnlGrid.Name = "pnlGrid";
            pnlGrid.Size = new Size(1648, 703);
            pnlGrid.TabIndex = 36;
            // 
            // dgvAttendance
            // 
            dgvAttendance.BackgroundColor = Color.White;
            dgvAttendance.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(240, 240, 240);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvAttendance.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvAttendance.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAttendance.Dock = DockStyle.Top;
            dgvAttendance.EnableHeadersVisualStyles = false;
            dgvAttendance.Location = new Point(0, 0);
            dgvAttendance.Name = "dgvAttendance";
            dgvAttendance.Size = new Size(1648, 646);
            dgvAttendance.TabIndex = 40;
            // 
            // pnlPagination
            // 
            pnlPagination.Controls.Add(btnPageNextDbl);
            pnlPagination.Controls.Add(btnPageNext);
            pnlPagination.Controls.Add(btnPage3);
            pnlPagination.Controls.Add(btnPage2);
            pnlPagination.Controls.Add(btnPage1);
            pnlPagination.Controls.Add(btnPagePrev);
            pnlPagination.Controls.Add(btnPagePrevDbl);
            pnlPagination.Location = new Point(1233, 655);
            pnlPagination.Name = "pnlPagination";
            pnlPagination.Size = new Size(260, 40);
            pnlPagination.TabIndex = 39;
            // 
            // btnPageNextDbl
            // 
            btnPageNextDbl.Location = new Point(210, 0);
            btnPageNextDbl.Name = "btnPageNextDbl";
            btnPageNextDbl.Size = new Size(35, 30);
            btnPageNextDbl.TabIndex = 6;
            btnPageNextDbl.Text = ">>";
            btnPageNextDbl.UseVisualStyleBackColor = true;
            // 
            // btnPageNext
            // 
            btnPageNext.Location = new Point(175, 0);
            btnPageNext.Name = "btnPageNext";
            btnPageNext.Size = new Size(35, 30);
            btnPageNext.TabIndex = 5;
            btnPageNext.Text = ">";
            btnPageNext.UseVisualStyleBackColor = true;
            // 
            // btnPage3
            // 
            btnPage3.Location = new Point(140, 0);
            btnPage3.Name = "btnPage3";
            btnPage3.Size = new Size(35, 30);
            btnPage3.TabIndex = 4;
            btnPage3.Text = "3";
            btnPage3.UseVisualStyleBackColor = true;
            // 
            // btnPage2
            // 
            btnPage2.Location = new Point(105, 0);
            btnPage2.Name = "btnPage2";
            btnPage2.Size = new Size(35, 30);
            btnPage2.TabIndex = 3;
            btnPage2.Text = "2";
            btnPage2.UseVisualStyleBackColor = true;
            // 
            // btnPage1
            // 
            btnPage1.BackColor = Color.Maroon;
            btnPage1.ForeColor = Color.White;
            btnPage1.Location = new Point(70, 0);
            btnPage1.Name = "btnPage1";
            btnPage1.Size = new Size(35, 30);
            btnPage1.TabIndex = 2;
            btnPage1.Text = "1";
            btnPage1.UseVisualStyleBackColor = false;
            // 
            // btnPagePrev
            // 
            btnPagePrev.Location = new Point(35, 0);
            btnPagePrev.Name = "btnPagePrev";
            btnPagePrev.Size = new Size(35, 30);
            btnPagePrev.TabIndex = 1;
            btnPagePrev.Text = "<";
            btnPagePrev.UseVisualStyleBackColor = true;
            // 
            // btnPagePrevDbl
            // 
            btnPagePrevDbl.Location = new Point(0, 0);
            btnPagePrevDbl.Name = "btnPagePrevDbl";
            btnPagePrevDbl.Size = new Size(35, 30);
            btnPagePrevDbl.TabIndex = 0;
            btnPagePrevDbl.Text = "<<";
            btnPagePrevDbl.UseVisualStyleBackColor = true;
            // 
            // lblShowing
            // 
            lblShowing.AutoSize = true;
            lblShowing.Location = new Point(35, 655);
            lblShowing.Name = "lblShowing";
            lblShowing.Size = new Size(162, 15);
            lblShowing.TabIndex = 38;
            lblShowing.Text = "Showing 1 to 6 of 40 students";
            // 
            // panel22
            // 
            panel22.BackColor = Color.White;
            panel22.Controls.Add(btnQRCode);
            panel22.Controls.Add(cmbSession);
            panel22.Controls.Add(lblSession);
            panel22.Controls.Add(btnImporttoCSV);
            panel22.Controls.Add(dtpDate);
            panel22.Controls.Add(btnExport);
            panel22.Controls.Add(lblDate);
            panel22.Controls.Add(comboBox1);
            panel22.Controls.Add(label5);
            panel22.Dock = DockStyle.Top;
            panel22.Location = new Point(0, 40);
            panel22.Name = "panel22";
            panel22.Size = new Size(1648, 41);
            panel22.TabIndex = 34;
            // 
            // btnQRCode
            // 
            btnQRCode.BackColor = Color.Maroon;
            btnQRCode.Cursor = Cursors.Hand;
            btnQRCode.FlatStyle = FlatStyle.Popup;
            btnQRCode.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnQRCode.ForeColor = Color.White;
            btnQRCode.Location = new Point(1303, 3);
            btnQRCode.Name = "btnQRCode";
            btnQRCode.Size = new Size(120, 35);
            btnQRCode.TabIndex = 16;
            btnQRCode.Text = "Generate QR Code";
            btnQRCode.UseVisualStyleBackColor = false;
            // 
            // cmbSession
            // 
            cmbSession.FormattingEnabled = true;
            cmbSession.Location = new Point(658, 5);
            cmbSession.Name = "cmbSession";
            cmbSession.Size = new Size(220, 23);
            cmbSession.TabIndex = 15;
            cmbSession.Text = "Morning (8:00 AM - 10:00 AM)";
            // 
            // lblSession
            // 
            lblSession.AutoSize = true;
            lblSession.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSession.Location = new Point(608, 10);
            lblSession.Name = "lblSession";
            lblSession.Size = new Size(51, 15);
            lblSession.TabIndex = 14;
            lblSession.Text = "Session:";
            // 
            // btnImporttoCSV
            // 
            btnImporttoCSV.Cursor = Cursors.Hand;
            btnImporttoCSV.FlatStyle = FlatStyle.Flat;
            btnImporttoCSV.Location = new Point(1441, 3);
            btnImporttoCSV.Name = "btnImporttoCSV";
            btnImporttoCSV.Size = new Size(93, 35);
            btnImporttoCSV.TabIndex = 3;
            btnImporttoCSV.Text = "Import from CSV";
            btnImporttoCSV.UseVisualStyleBackColor = true;
            // 
            // dtpDate
            // 
            dtpDate.Format = DateTimePickerFormat.Short;
            dtpDate.Location = new Point(468, 5);
            dtpDate.Name = "dtpDate";
            dtpDate.Size = new Size(120, 23);
            dtpDate.TabIndex = 13;
            // 
            // btnExport
            // 
            btnExport.Cursor = Cursors.Hand;
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Location = new Point(1548, 3);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(93, 35);
            btnExport.TabIndex = 2;
            btnExport.Text = "Export";
            btnExport.UseVisualStyleBackColor = true;
            // 
            // lblDate
            // 
            lblDate.AutoSize = true;
            lblDate.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDate.Location = new Point(428, 10);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(37, 15);
            lblDate.TabIndex = 12;
            lblDate.Text = "Date:";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(128, 5);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(280, 23);
            comboBox1.TabIndex = 11;
            comboBox1.Text = "Introduction to Programming 1";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label5.Location = new Point(15, 10);
            label5.Name = "label5";
            label5.Size = new Size(116, 15);
            label5.TabIndex = 10;
            label5.Text = "Course and Section:";
            // 
            // pnlControlAttendance
            // 
            pnlControlAttendance.Controls.Add(tableLayoutPanel1);
            pnlControlAttendance.Location = new Point(-1, 81);
            pnlControlAttendance.Name = "pnlControlAttendance";
            pnlControlAttendance.Size = new Size(1648, 131);
            pnlControlAttendance.TabIndex = 33;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25.581398F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18.6046524F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18.6046524F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18.6046524F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18.6046524F));
            tableLayoutPanel1.Controls.Add(panel14, 4, 0);
            tableLayoutPanel1.Controls.Add(panel17, 3, 0);
            tableLayoutPanel1.Controls.Add(panel18, 2, 0);
            tableLayoutPanel1.Controls.Add(panel20, 1, 0);
            tableLayoutPanel1.Controls.Add(panel21, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Top;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1648, 131);
            tableLayoutPanel1.TabIndex = 32;
            // 
            // panel14
            // 
            panel14.BackColor = Color.White;
            panel14.Controls.Add(lblLastUpdate);
            panel14.Controls.Add(lblDateTime);
            panel14.Controls.Add(pictureBox26);
            panel14.Controls.Add(lblByInstructor);
            panel14.Dock = DockStyle.Fill;
            panel14.Location = new Point(1343, 3);
            panel14.Margin = new Padding(4, 3, 4, 3);
            panel14.Name = "panel14";
            panel14.Size = new Size(301, 125);
            panel14.TabIndex = 4;
            // 
            // lblLastUpdate
            // 
            lblLastUpdate.AutoSize = true;
            lblLastUpdate.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLastUpdate.ForeColor = Color.Black;
            lblLastUpdate.Location = new Point(85, 14);
            lblLastUpdate.Margin = new Padding(4, 0, 4, 0);
            lblLastUpdate.Name = "lblLastUpdate";
            lblLastUpdate.Size = new Size(80, 15);
            lblLastUpdate.TabIndex = 9;
            lblLastUpdate.Text = "Last Updated";
            // 
            // lblDateTime
            // 
            lblDateTime.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDateTime.ForeColor = Color.Black;
            lblDateTime.Location = new Point(88, 33);
            lblDateTime.Margin = new Padding(4, 0, 4, 0);
            lblDateTime.Name = "lblDateTime";
            lblDateTime.Size = new Size(201, 42);
            lblDateTime.TabIndex = 8;
            lblDateTime.Text = "May 24, 2024 10:50pm";
            // 
            // pictureBox26
            // 
            pictureBox26.Location = new Point(15, 19);
            pictureBox26.Name = "pictureBox26";
            pictureBox26.Size = new Size(50, 50);
            pictureBox26.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox26.TabIndex = 6;
            pictureBox26.TabStop = false;
            // 
            // lblByInstructor
            // 
            lblByInstructor.AutoSize = true;
            lblByInstructor.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblByInstructor.ForeColor = Color.Gray;
            lblByInstructor.Location = new Point(85, 75);
            lblByInstructor.Margin = new Padding(4, 0, 4, 0);
            lblByInstructor.Name = "lblByInstructor";
            lblByInstructor.Size = new Size(78, 13);
            lblByInstructor.TabIndex = 1;
            lblByInstructor.Text = "by Instruction";
            // 
            // panel17
            // 
            panel17.BackColor = Color.White;
            panel17.Controls.Add(lblExcusedPrecent);
            panel17.Controls.Add(lblExcused);
            panel17.Controls.Add(pictureBox27);
            panel17.Controls.Add(lblExcusedNum);
            panel17.Dock = DockStyle.Fill;
            panel17.Location = new Point(1037, 3);
            panel17.Margin = new Padding(4, 3, 4, 3);
            panel17.Name = "panel17";
            panel17.Size = new Size(298, 125);
            panel17.TabIndex = 3;
            // 
            // lblExcusedPrecent
            // 
            lblExcusedPrecent.AutoSize = true;
            lblExcusedPrecent.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblExcusedPrecent.ForeColor = Color.Gray;
            lblExcusedPrecent.Location = new Point(81, 77);
            lblExcusedPrecent.Margin = new Padding(4, 0, 4, 0);
            lblExcusedPrecent.Name = "lblExcusedPrecent";
            lblExcusedPrecent.Size = new Size(43, 13);
            lblExcusedPrecent.TabIndex = 6;
            lblExcusedPrecent.Text = "98.50%";
            // 
            // lblExcused
            // 
            lblExcused.AutoSize = true;
            lblExcused.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblExcused.ForeColor = Color.Black;
            lblExcused.Location = new Point(81, 19);
            lblExcused.Margin = new Padding(4, 0, 4, 0);
            lblExcused.Name = "lblExcused";
            lblExcused.Size = new Size(52, 15);
            lblExcused.TabIndex = 7;
            lblExcused.Text = "Excused";
            // 
            // pictureBox27
            // 
            pictureBox27.Location = new Point(9, 18);
            pictureBox27.Name = "pictureBox27";
            pictureBox27.Size = new Size(50, 50);
            pictureBox27.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox27.TabIndex = 5;
            pictureBox27.TabStop = false;
            // 
            // lblExcusedNum
            // 
            lblExcusedNum.AutoSize = true;
            lblExcusedNum.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblExcusedNum.Location = new Point(84, 41);
            lblExcusedNum.Margin = new Padding(4, 0, 4, 0);
            lblExcusedNum.Name = "lblExcusedNum";
            lblExcusedNum.Size = new Size(61, 25);
            lblExcusedNum.TabIndex = 2;
            lblExcusedNum.Text = "82.45";
            // 
            // panel18
            // 
            panel18.BackColor = Color.White;
            panel18.Controls.Add(lblAbsentPercent);
            panel18.Controls.Add(lblAbsent);
            panel18.Controls.Add(pictureBox28);
            panel18.Controls.Add(lblAbsentNum);
            panel18.Dock = DockStyle.Fill;
            panel18.Location = new Point(731, 3);
            panel18.Margin = new Padding(4, 3, 4, 3);
            panel18.Name = "panel18";
            panel18.Size = new Size(298, 125);
            panel18.TabIndex = 2;
            // 
            // lblAbsentPercent
            // 
            lblAbsentPercent.AutoSize = true;
            lblAbsentPercent.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblAbsentPercent.ForeColor = Color.Gray;
            lblAbsentPercent.Location = new Point(80, 76);
            lblAbsentPercent.Margin = new Padding(4, 0, 4, 0);
            lblAbsentPercent.Name = "lblAbsentPercent";
            lblAbsentPercent.Size = new Size(43, 13);
            lblAbsentPercent.TabIndex = 5;
            lblAbsentPercent.Text = "98.50%";
            // 
            // lblAbsent
            // 
            lblAbsent.AutoSize = true;
            lblAbsent.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAbsent.ForeColor = Color.Black;
            lblAbsent.Location = new Point(80, 19);
            lblAbsent.Margin = new Padding(4, 0, 4, 0);
            lblAbsent.Name = "lblAbsent";
            lblAbsent.Size = new Size(46, 15);
            lblAbsent.TabIndex = 5;
            lblAbsent.Text = "Absent";
            // 
            // pictureBox28
            // 
            pictureBox28.Location = new Point(13, 18);
            pictureBox28.Name = "pictureBox28";
            pictureBox28.Size = new Size(50, 50);
            pictureBox28.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox28.TabIndex = 4;
            pictureBox28.TabStop = false;
            // 
            // lblAbsentNum
            // 
            lblAbsentNum.AutoSize = true;
            lblAbsentNum.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAbsentNum.Location = new Point(80, 41);
            lblAbsentNum.Margin = new Padding(4, 0, 4, 0);
            lblAbsentNum.Name = "lblAbsentNum";
            lblAbsentNum.Size = new Size(34, 25);
            lblAbsentNum.TabIndex = 2;
            lblAbsentNum.Text = "15";
            // 
            // panel20
            // 
            panel20.BackColor = Color.White;
            panel20.Controls.Add(lblPresent);
            panel20.Controls.Add(lblPesentPercent);
            panel20.Controls.Add(lblPresentNum);
            panel20.Controls.Add(pictureBox29);
            panel20.Dock = DockStyle.Fill;
            panel20.Location = new Point(425, 3);
            panel20.Margin = new Padding(4, 3, 4, 3);
            panel20.Name = "panel20";
            panel20.Size = new Size(298, 125);
            panel20.TabIndex = 1;
            // 
            // lblPresent
            // 
            lblPresent.AutoSize = true;
            lblPresent.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPresent.ForeColor = Color.Black;
            lblPresent.Location = new Point(90, 19);
            lblPresent.Margin = new Padding(4, 0, 4, 0);
            lblPresent.Name = "lblPresent";
            lblPresent.Size = new Size(50, 15);
            lblPresent.TabIndex = 7;
            lblPresent.Text = "Present";
            // 
            // lblPesentPercent
            // 
            lblPesentPercent.AutoSize = true;
            lblPesentPercent.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPesentPercent.ForeColor = Color.Gray;
            lblPesentPercent.Location = new Point(87, 73);
            lblPesentPercent.Margin = new Padding(4, 0, 4, 0);
            lblPesentPercent.Name = "lblPesentPercent";
            lblPesentPercent.Size = new Size(43, 13);
            lblPesentPercent.TabIndex = 4;
            lblPesentPercent.Text = "98.50%";
            // 
            // lblPresentNum
            // 
            lblPresentNum.AutoSize = true;
            lblPresentNum.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPresentNum.Location = new Point(85, 41);
            lblPresentNum.Margin = new Padding(4, 0, 4, 0);
            lblPresentNum.Name = "lblPresentNum";
            lblPresentNum.Size = new Size(34, 25);
            lblPresentNum.TabIndex = 6;
            lblPresentNum.Text = "15";
            // 
            // pictureBox29
            // 
            pictureBox29.Location = new Point(13, 19);
            pictureBox29.Name = "pictureBox29";
            pictureBox29.Size = new Size(50, 50);
            pictureBox29.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox29.TabIndex = 3;
            pictureBox29.TabStop = false;
            // 
            // panel21
            // 
            panel21.BackColor = Color.White;
            panel21.Controls.Add(lblLayoutAttendance);
            panel21.Controls.Add(lblSessionAttendance);
            panel21.Dock = DockStyle.Fill;
            panel21.Location = new Point(4, 3);
            panel21.Margin = new Padding(4, 3, 4, 3);
            panel21.Name = "panel21";
            panel21.Size = new Size(413, 125);
            panel21.TabIndex = 0;
            // 
            // lblLayoutAttendance
            // 
            lblLayoutAttendance.BorderStyle = BorderStyle.FixedSingle;
            lblLayoutAttendance.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblLayoutAttendance.ForeColor = Color.Gray;
            lblLayoutAttendance.Location = new Point(144, 32);
            lblLayoutAttendance.Margin = new Padding(4, 0, 4, 0);
            lblLayoutAttendance.Name = "lblLayoutAttendance";
            lblLayoutAttendance.Size = new Size(252, 82);
            lblLayoutAttendance.TabIndex = 9;
            // 
            // lblSessionAttendance
            // 
            lblSessionAttendance.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSessionAttendance.ForeColor = Color.Black;
            lblSessionAttendance.Location = new Point(144, 13);
            lblSessionAttendance.Margin = new Padding(4, 0, 4, 0);
            lblSessionAttendance.Name = "lblSessionAttendance";
            lblSessionAttendance.Size = new Size(257, 107);
            lblSessionAttendance.TabIndex = 8;
            lblSessionAttendance.Text = "Session Attendance";
            // 
            // panel19
            // 
            panel19.BackColor = Color.White;
            panel19.Controls.Add(lblAttendanceTitle);
            panel19.Dock = DockStyle.Top;
            panel19.Location = new Point(0, 0);
            panel19.Margin = new Padding(3, 2, 3, 2);
            panel19.Name = "panel19";
            panel19.Size = new Size(1648, 40);
            panel19.TabIndex = 31;
            // 
            // lblAttendanceTitle
            // 
            lblAttendanceTitle.AutoSize = true;
            lblAttendanceTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblAttendanceTitle.Location = new Point(10, 7);
            lblAttendanceTitle.Name = "lblAttendanceTitle";
            lblAttendanceTitle.Size = new Size(191, 30);
            lblAttendanceTitle.TabIndex = 1;
            lblAttendanceTitle.Text = "Class Attendance";
            // 
            // AttendanceContentInst
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlAttendance);
            Name = "AttendanceContentInst";
            Size = new Size(1648, 969);
            Load += AttendanceContentInst_Load;
            pnlAttendance.ResumeLayout(false);
            panel24.ResumeLayout(false);
            panel24.PerformLayout();
            pnlGrid.ResumeLayout(false);
            pnlGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAttendance).EndInit();
            pnlPagination.ResumeLayout(false);
            panel22.ResumeLayout(false);
            panel22.PerformLayout();
            pnlControlAttendance.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            panel14.ResumeLayout(false);
            panel14.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox26).EndInit();
            panel17.ResumeLayout(false);
            panel17.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox27).EndInit();
            panel18.ResumeLayout(false);
            panel18.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox28).EndInit();
            panel20.ResumeLayout(false);
            panel20.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox29).EndInit();
            panel21.ResumeLayout(false);
            panel19.ResumeLayout(false);
            panel19.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlAttendance;
        private Panel panel24;
        private TextBox textBox2;
        private Button btnSaveAttendance;
        private Button btnRefresh;
        private Panel pnlGrid;
        private DataGridView dgvAttendance;
        private Panel pnlPagination;
        private Button btnPageNextDbl;
        private Button btnPageNext;
        private Button btnPage3;
        private Button btnPage2;
        private Button btnPage1;
        private Button btnPagePrev;
        private Button btnPagePrevDbl;
        private Label lblShowing;
        private Panel panel22;
        private Button btnQRCode;
        private ComboBox cmbSession;
        private Label lblSession;
        private Button btnImporttoCSV;
        private DateTimePicker dtpDate;
        private Button btnExport;
        private Label lblDate;
        private ComboBox comboBox1;
        private Label label5;
        private Panel pnlControlAttendance;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel14;
        private Label lblLastUpdate;
        private Label lblDateTime;
        private PictureBox pictureBox26;
        private Label lblByInstructor;
        private Panel panel17;
        private Label lblExcusedPrecent;
        private Label lblExcused;
        private PictureBox pictureBox27;
        private Label lblExcusedNum;
        private Panel panel18;
        private Label lblAbsentPercent;
        private Label lblAbsent;
        private PictureBox pictureBox28;
        private Label lblAbsentNum;
        private Panel panel20;
        private Label lblPresent;
        private Label lblPesentPercent;
        private Label lblPresentNum;
        private PictureBox pictureBox29;
        private Panel panel21;
        private Label lblLayoutAttendance;
        private Label lblSessionAttendance;
        private Panel panel19;
        private Label lblAttendanceTitle;
    }
}
