namespace PUPAcadPortal.PortalContents.Instructor.LMS
{
    partial class AttendanceGridControl
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
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            _pnlBulk = new Panel();
            btnMarkExcused = new Button();
            btnMarkAbsent = new Button();
            btnMarkLate = new Button();
            btnMarkPresent = new Button();
            lblBulk = new Label();
            _dgv = new DataGridView();
            colNo = new DataGridViewTextBoxColumn();
            colLastName = new DataGridViewTextBoxColumn();
            colFirstName = new DataGridViewTextBoxColumn();
            colMI = new DataGridViewTextBoxColumn();
            colId = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewComboBoxColumn();
            colRemarks = new DataGridViewTextBoxColumn();
            _lblShowing = new Label();
            _pnlPagination = new Panel();
            _btnLast = new Button();
            _btnNext = new Button();
            btnPage3 = new Button();
            btnPage2 = new Button();
            btnPage1 = new Button();
            _btnPrev = new Button();
            _btnFirst = new Button();
            _pnlBulk.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_dgv).BeginInit();
            _pnlPagination.SuspendLayout();
            SuspendLayout();
            // 
            // _pnlBulk
            // 
            _pnlBulk.BackColor = Color.FromArgb(248, 244, 244);
            _pnlBulk.Controls.Add(btnMarkExcused);
            _pnlBulk.Controls.Add(btnMarkAbsent);
            _pnlBulk.Controls.Add(btnMarkLate);
            _pnlBulk.Controls.Add(btnMarkPresent);
            _pnlBulk.Controls.Add(lblBulk);
            _pnlBulk.Location = new Point(0, 0);
            _pnlBulk.Name = "_pnlBulk";
            _pnlBulk.Size = new Size(800, 38);
            _pnlBulk.TabIndex = 0;
            // 
            // btnMarkExcused
            // 
            btnMarkExcused.BackColor = Color.FromArgb(255, 250, 210);
            btnMarkExcused.Cursor = Cursors.Hand;
            btnMarkExcused.FlatAppearance.BorderSize = 0;
            btnMarkExcused.FlatStyle = FlatStyle.Flat;
            btnMarkExcused.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnMarkExcused.ForeColor = Color.DarkGoldenrod;
            btnMarkExcused.Location = new Point(342, 6);
            btnMarkExcused.Name = "btnMarkExcused";
            btnMarkExcused.Size = new Size(82, 26);
            btnMarkExcused.TabIndex = 4;
            btnMarkExcused.Text = "~ Excused";
            btnMarkExcused.UseVisualStyleBackColor = false;
            // 
            // btnMarkAbsent
            // 
            btnMarkAbsent.BackColor = Color.FromArgb(252, 228, 228);
            btnMarkAbsent.Cursor = Cursors.Hand;
            btnMarkAbsent.FlatAppearance.BorderSize = 0;
            btnMarkAbsent.FlatStyle = FlatStyle.Flat;
            btnMarkAbsent.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnMarkAbsent.ForeColor = Color.Firebrick;
            btnMarkAbsent.Location = new Point(254, 6);
            btnMarkAbsent.Name = "btnMarkAbsent";
            btnMarkAbsent.Size = new Size(82, 26);
            btnMarkAbsent.TabIndex = 3;
            btnMarkAbsent.Text = "✗ Absent";
            btnMarkAbsent.UseVisualStyleBackColor = false;
            // 
            // btnMarkLate
            // 
            btnMarkLate.BackColor = Color.FromArgb(255, 243, 210);
            btnMarkLate.Cursor = Cursors.Hand;
            btnMarkLate.FlatAppearance.BorderSize = 0;
            btnMarkLate.FlatStyle = FlatStyle.Flat;
            btnMarkLate.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnMarkLate.ForeColor = Color.FromArgb(200, 110, 0);
            btnMarkLate.Location = new Point(166, 6);
            btnMarkLate.Name = "btnMarkLate";
            btnMarkLate.Size = new Size(82, 26);
            btnMarkLate.TabIndex = 2;
            btnMarkLate.Text = "⧖ Late";
            btnMarkLate.UseVisualStyleBackColor = false;
            // 
            // btnMarkPresent
            // 
            btnMarkPresent.BackColor = Color.FromArgb(220, 248, 220);
            btnMarkPresent.Cursor = Cursors.Hand;
            btnMarkPresent.FlatAppearance.BorderSize = 0;
            btnMarkPresent.FlatStyle = FlatStyle.Flat;
            btnMarkPresent.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnMarkPresent.ForeColor = Color.FromArgb(0, 140, 0);
            btnMarkPresent.Location = new Point(78, 6);
            btnMarkPresent.Name = "btnMarkPresent";
            btnMarkPresent.Size = new Size(82, 26);
            btnMarkPresent.TabIndex = 1;
            btnMarkPresent.Text = "✓ Present";
            btnMarkPresent.UseVisualStyleBackColor = false;
            // 
            // lblBulk
            // 
            lblBulk.AutoSize = true;
            lblBulk.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblBulk.ForeColor = Color.FromArgb(70, 70, 70);
            lblBulk.Location = new Point(10, 10);
            lblBulk.Name = "lblBulk";
            lblBulk.Size = new Size(56, 15);
            lblBulk.TabIndex = 0;
            lblBulk.Text = "Mark All:";
            // 
            // _dgv
            // 
            _dgv.AllowUserToAddRows = false;
            _dgv.AllowUserToDeleteRows = false;
            _dgv.BackgroundColor = Color.White;
            _dgv.BorderStyle = BorderStyle.None;
            _dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            _dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(106, 0, 0);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 8.75F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            _dgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            _dgv.ColumnHeadersHeight = 40;
            _dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _dgv.Columns.AddRange(new DataGridViewColumn[] { colNo, colLastName, colFirstName, colMI, colId, colStatus, colRemarks });
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = SystemColors.Window;
            dataGridViewCellStyle8.Font = new Font("Segoe UI", 8.75F);
            dataGridViewCellStyle8.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = Color.FromArgb(245, 238, 238);
            dataGridViewCellStyle8.SelectionForeColor = Color.Black;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            _dgv.DefaultCellStyle = dataGridViewCellStyle8;
            _dgv.EnableHeadersVisualStyles = false;
            _dgv.GridColor = Color.FromArgb(230, 230, 230);
            _dgv.Location = new Point(0, 38);
            _dgv.Name = "_dgv";
            _dgv.RowHeadersVisible = false;
            _dgv.RowTemplate.Height = 46;
            _dgv.ScrollBars = ScrollBars.None;
            _dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _dgv.Size = new Size(800, 350);
            _dgv.TabIndex = 1;
            // 
            // colNo
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colNo.DefaultCellStyle = dataGridViewCellStyle2;
            colNo.HeaderText = "#";
            colNo.Name = "colNo";
            colNo.ReadOnly = true;
            colNo.Width = 44;
            // 
            // colLastName
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            colLastName.DefaultCellStyle = dataGridViewCellStyle3;
            colLastName.HeaderText = "Last Name";
            colLastName.Name = "colLastName";
            colLastName.ReadOnly = true;
            colLastName.Width = 130;
            // 
            // colFirstName
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            colFirstName.DefaultCellStyle = dataGridViewCellStyle4;
            colFirstName.HeaderText = "First Name";
            colFirstName.Name = "colFirstName";
            colFirstName.ReadOnly = true;
            colFirstName.Width = 130;
            // 
            // colMI
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colMI.DefaultCellStyle = dataGridViewCellStyle5;
            colMI.HeaderText = "MI";
            colMI.Name = "colMI";
            colMI.ReadOnly = true;
            colMI.Width = 42;
            // 
            // colId
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colId.DefaultCellStyle = dataGridViewCellStyle6;
            colId.HeaderText = "ID Number";
            colId.Name = "colId";
            colId.ReadOnly = true;
            colId.Width = 165;
            // 
            // colStatus
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colStatus.DefaultCellStyle = dataGridViewCellStyle7;
            colStatus.FlatStyle = FlatStyle.Flat;
            colStatus.HeaderText = "Status";
            colStatus.Items.AddRange(new object[] { "Present", "Late", "Absent", "Excused" });
            colStatus.Name = "colStatus";
            colStatus.Width = 130;
            // 
            // colRemarks
            // 
            colRemarks.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colRemarks.HeaderText = "Remarks";
            colRemarks.Name = "colRemarks";
            // 
            // _lblShowing
            // 
            _lblShowing.AutoSize = true;
            _lblShowing.Font = new Font("Segoe UI", 8.5F);
            _lblShowing.ForeColor = Color.FromArgb(80, 80, 80);
            _lblShowing.Location = new Point(10, 404);
            _lblShowing.Name = "_lblShowing";
            _lblShowing.Size = new Size(59, 15);
            _lblShowing.TabIndex = 2;
            _lblShowing.Text = "Loading...";
            // 
            // _pnlPagination
            // 
            _pnlPagination.BackColor = Color.Transparent;
            _pnlPagination.Controls.Add(_btnLast);
            _pnlPagination.Controls.Add(_btnNext);
            _pnlPagination.Controls.Add(btnPage3);
            _pnlPagination.Controls.Add(btnPage2);
            _pnlPagination.Controls.Add(btnPage1);
            _pnlPagination.Controls.Add(_btnPrev);
            _pnlPagination.Controls.Add(_btnFirst);
            _pnlPagination.Location = new Point(554, 400);
            _pnlPagination.Name = "_pnlPagination";
            _pnlPagination.Size = new Size(240, 32);
            _pnlPagination.TabIndex = 3;
            // 
            // _btnLast
            // 
            _btnLast.BackColor = Color.White;
            _btnLast.Cursor = Cursors.Hand;
            _btnLast.FlatAppearance.BorderColor = Color.FromArgb(230, 230, 230);
            _btnLast.FlatStyle = FlatStyle.Flat;
            _btnLast.Font = new Font("Segoe UI", 9F);
            _btnLast.ForeColor = Color.Black;
            _btnLast.Location = new Point(204, 0);
            _btnLast.Name = "_btnLast";
            _btnLast.Size = new Size(30, 30);
            _btnLast.TabIndex = 6;
            _btnLast.Text = "»";
            _btnLast.UseVisualStyleBackColor = false;
            // 
            // _btnNext
            // 
            _btnNext.BackColor = Color.White;
            _btnNext.Cursor = Cursors.Hand;
            _btnNext.FlatAppearance.BorderColor = Color.FromArgb(230, 230, 230);
            _btnNext.FlatStyle = FlatStyle.Flat;
            _btnNext.Font = new Font("Segoe UI", 9F);
            _btnNext.ForeColor = Color.Black;
            _btnNext.Location = new Point(172, 0);
            _btnNext.Name = "_btnNext";
            _btnNext.Size = new Size(30, 30);
            _btnNext.TabIndex = 5;
            _btnNext.Text = "›";
            _btnNext.UseVisualStyleBackColor = false;
            // 
            // btnPage3
            // 
            btnPage3.BackColor = Color.White;
            btnPage3.Cursor = Cursors.Hand;
            btnPage3.FlatAppearance.BorderColor = Color.FromArgb(230, 230, 230);
            btnPage3.FlatStyle = FlatStyle.Flat;
            btnPage3.Font = new Font("Segoe UI", 9F);
            btnPage3.ForeColor = Color.Black;
            btnPage3.Location = new Point(136, 0);
            btnPage3.Name = "btnPage3";
            btnPage3.Size = new Size(34, 30);
            btnPage3.TabIndex = 4;
            btnPage3.Text = "3";
            btnPage3.UseVisualStyleBackColor = false;
            // 
            // btnPage2
            // 
            btnPage2.BackColor = Color.White;
            btnPage2.Cursor = Cursors.Hand;
            btnPage2.FlatAppearance.BorderColor = Color.FromArgb(230, 230, 230);
            btnPage2.FlatStyle = FlatStyle.Flat;
            btnPage2.Font = new Font("Segoe UI", 9F);
            btnPage2.ForeColor = Color.Black;
            btnPage2.Location = new Point(100, 0);
            btnPage2.Name = "btnPage2";
            btnPage2.Size = new Size(34, 30);
            btnPage2.TabIndex = 3;
            btnPage2.Text = "2";
            btnPage2.UseVisualStyleBackColor = false;
            // 
            // btnPage1
            // 
            btnPage1.BackColor = Color.White;
            btnPage1.Cursor = Cursors.Hand;
            btnPage1.FlatAppearance.BorderColor = Color.FromArgb(230, 230, 230);
            btnPage1.FlatStyle = FlatStyle.Flat;
            btnPage1.Font = new Font("Segoe UI", 9F);
            btnPage1.ForeColor = Color.Black;
            btnPage1.Location = new Point(64, 0);
            btnPage1.Name = "btnPage1";
            btnPage1.Size = new Size(34, 30);
            btnPage1.TabIndex = 2;
            btnPage1.Text = "1";
            btnPage1.UseVisualStyleBackColor = false;
            // 
            // _btnPrev
            // 
            _btnPrev.BackColor = Color.White;
            _btnPrev.Cursor = Cursors.Hand;
            _btnPrev.FlatAppearance.BorderColor = Color.FromArgb(230, 230, 230);
            _btnPrev.FlatStyle = FlatStyle.Flat;
            _btnPrev.Font = new Font("Segoe UI", 9F);
            _btnPrev.ForeColor = Color.Black;
            _btnPrev.Location = new Point(32, 0);
            _btnPrev.Name = "_btnPrev";
            _btnPrev.Size = new Size(30, 30);
            _btnPrev.TabIndex = 1;
            _btnPrev.Text = "‹";
            _btnPrev.UseVisualStyleBackColor = false;
            // 
            // _btnFirst
            // 
            _btnFirst.BackColor = Color.White;
            _btnFirst.Cursor = Cursors.Hand;
            _btnFirst.FlatAppearance.BorderColor = Color.FromArgb(230, 230, 230);
            _btnFirst.FlatStyle = FlatStyle.Flat;
            _btnFirst.Font = new Font("Segoe UI", 9F);
            _btnFirst.ForeColor = Color.Black;
            _btnFirst.Location = new Point(0, 0);
            _btnFirst.Name = "_btnFirst";
            _btnFirst.Size = new Size(30, 30);
            _btnFirst.TabIndex = 0;
            _btnFirst.Text = "«";
            _btnFirst.UseVisualStyleBackColor = false;
            // 
            // AttendanceGridControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(_pnlPagination);
            Controls.Add(_lblShowing);
            Controls.Add(_dgv);
            Controls.Add(_pnlBulk);
            DoubleBuffered = true;
            Name = "AttendanceGridControl";
            Size = new Size(800, 440);
            _pnlBulk.ResumeLayout(false);
            _pnlBulk.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_dgv).EndInit();
            _pnlPagination.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel _pnlBulk;
        private System.Windows.Forms.Label lblBulk;
        private System.Windows.Forms.Button btnMarkPresent;
        private System.Windows.Forms.Button btnMarkLate;
        private System.Windows.Forms.Button btnMarkAbsent;
        private System.Windows.Forms.Button btnMarkExcused;
        private System.Windows.Forms.DataGridView _dgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMI;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewComboBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRemarks;
        private System.Windows.Forms.Label _lblShowing;
        private System.Windows.Forms.Panel _pnlPagination;
        private System.Windows.Forms.Button _btnFirst;
        private System.Windows.Forms.Button _btnPrev;
        private System.Windows.Forms.Button btnPage1;
        private System.Windows.Forms.Button btnPage2;
        private System.Windows.Forms.Button btnPage3;
        private System.Windows.Forms.Button _btnNext;
        private System.Windows.Forms.Button _btnLast;
    }
}