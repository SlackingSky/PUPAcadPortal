namespace PUPAcadPortal.PortalContents.Instructor.LMS.Calendar
{
    partial class FacultySearchPanel
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
            this._row1 = new System.Windows.Forms.FlowLayoutPanel();
            this._txtSearch = new System.Windows.Forms.TextBox();
            this._cboType = new System.Windows.Forms.ComboBox();
            this._cboCourse = new System.Windows.Forms.ComboBox();
            this._btnSearch = new System.Windows.Forms.Button();
            this._btnClear = new System.Windows.Forms.Button();
            this._row2 = new System.Windows.Forms.FlowLayoutPanel();
            this._chkDateRange = new System.Windows.Forms.CheckBox();
            this._dtpFrom = new System.Windows.Forms.DateTimePicker();
            this._lblTo = new System.Windows.Forms.Label();
            this._dtpTo = new System.Windows.Forms.DateTimePicker();
            this._lblCount = new System.Windows.Forms.Label();
            this._resultsFLP = new System.Windows.Forms.FlowLayoutPanel();
            this._row1.SuspendLayout();
            this._row2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _row1
            // 
            this._row1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._row1.Controls.Add(this._txtSearch);
            this._row1.Controls.Add(this._cboType);
            this._row1.Controls.Add(this._cboCourse);
            this._row1.Controls.Add(this._btnSearch);
            this._row1.Controls.Add(this._btnClear);
            this._row1.Location = new System.Drawing.Point(12, 10);
            this._row1.Name = "_row1";
            this._row1.Size = new System.Drawing.Size(776, 34);
            this._row1.TabIndex = 0;
            this._row1.WrapContents = false;
            // 
            // _txtSearch
            // 
            this._txtSearch.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._txtSearch.Location = new System.Drawing.Point(0, 0);
            this._txtSearch.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this._txtSearch.Name = "_txtSearch";
            this._txtSearch.Size = new System.Drawing.Size(220, 23);
            this._txtSearch.TabIndex = 0;
            this._txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtSearch_KeyDown);
            // 
            // _cboType
            // 
            this._cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboType.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._cboType.FormattingEnabled = true;
            this._cboType.Location = new System.Drawing.Point(226, 0);
            this._cboType.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this._cboType.Name = "_cboType";
            this._cboType.Size = new System.Drawing.Size(140, 21);
            this._cboType.TabIndex = 1;
            // 
            // _cboCourse
            // 
            this._cboCourse.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._cboCourse.FormattingEnabled = true;
            this._cboCourse.Location = new System.Drawing.Point(372, 0);
            this._cboCourse.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this._cboCourse.Name = "_cboCourse";
            this._cboCourse.Size = new System.Drawing.Size(200, 21);
            this._cboCourse.TabIndex = 2;
            // 
            // _btnSearch
            // 
            this._btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(14)))), ((int)(((byte)(79)))));
            this._btnSearch.FlatAppearance.BorderSize = 0;
            this._btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnSearch.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._btnSearch.ForeColor = System.Drawing.Color.White;
            this._btnSearch.Location = new System.Drawing.Point(578, 0);
            this._btnSearch.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this._btnSearch.Name = "_btnSearch";
            this._btnSearch.Size = new System.Drawing.Size(80, 30);
            this._btnSearch.TabIndex = 3;
            this._btnSearch.Text = "Search";
            this._btnSearch.UseVisualStyleBackColor = false;
            this._btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // _btnClear
            // 
            this._btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnClear.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._btnClear.Location = new System.Drawing.Point(664, 0);
            this._btnClear.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this._btnClear.Name = "_btnClear";
            this._btnClear.Size = new System.Drawing.Size(60, 30);
            this._btnClear.TabIndex = 4;
            this._btnClear.Text = "Clear";
            this._btnClear.UseVisualStyleBackColor = true;
            this._btnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // _row2
            // 
            this._row2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._row2.Controls.Add(this._chkDateRange);
            this._row2.Controls.Add(this._dtpFrom);
            this._row2.Controls.Add(this._lblTo);
            this._row2.Controls.Add(this._dtpTo);
            this._row2.Location = new System.Drawing.Point(12, 50);
            this._row2.Name = "_row2";
            this._row2.Size = new System.Drawing.Size(776, 30);
            this._row2.TabIndex = 1;
            this._row2.WrapContents = false;
            // 
            // _chkDateRange
            // 
            this._chkDateRange.AutoSize = true;
            this._chkDateRange.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._chkDateRange.Location = new System.Drawing.Point(0, 4);
            this._chkDateRange.Margin = new System.Windows.Forms.Padding(0, 4, 6, 0);
            this._chkDateRange.Name = "_chkDateRange";
            this._chkDateRange.Size = new System.Drawing.Size(87, 19);
            this._chkDateRange.TabIndex = 0;
            this._chkDateRange.Text = "Date range:";
            this._chkDateRange.UseVisualStyleBackColor = true;
            this._chkDateRange.CheckedChanged += new System.EventHandler(this.ChkDateRange_CheckedChanged);
            // 
            // _dtpFrom
            // 
            this._dtpFrom.Enabled = false;
            this._dtpFrom.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtpFrom.Location = new System.Drawing.Point(93, 0);
            this._dtpFrom.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this._dtpFrom.Name = "_dtpFrom";
            this._dtpFrom.Size = new System.Drawing.Size(140, 23);
            this._dtpFrom.TabIndex = 1;
            // 
            // _lblTo
            // 
            this._lblTo.AutoSize = true;
            this._lblTo.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._lblTo.Location = new System.Drawing.Point(239, 6);
            this._lblTo.Margin = new System.Windows.Forms.Padding(0, 6, 6, 0);
            this._lblTo.Name = "_lblTo";
            this._lblTo.Size = new System.Drawing.Size(18, 15);
            this._lblTo.TabIndex = 2;
            this._lblTo.Text = "to";
            // 
            // _dtpTo
            // 
            this._dtpTo.Enabled = false;
            this._dtpTo.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtpTo.Location = new System.Drawing.Point(263, 0);
            this._dtpTo.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this._dtpTo.Name = "_dtpTo";
            this._dtpTo.Size = new System.Drawing.Size(140, 23);
            this._dtpTo.TabIndex = 3;
            // 
            // _lblCount
            // 
            this._lblCount.AutoSize = true;
            this._lblCount.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._lblCount.ForeColor = System.Drawing.Color.Gray;
            this._lblCount.Location = new System.Drawing.Point(12, 86);
            this._lblCount.Name = "_lblCount";
            this._lblCount.Size = new System.Drawing.Size(0, 15);
            this._lblCount.TabIndex = 2;
            // 
            // _resultsFLP
            // 
            this._resultsFLP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._resultsFLP.AutoScroll = true;
            this._resultsFLP.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._resultsFLP.Location = new System.Drawing.Point(0, 108);
            this._resultsFLP.Name = "_resultsFLP";
            this._resultsFLP.Size = new System.Drawing.Size(800, 160);
            this._resultsFLP.TabIndex = 3;
            this._resultsFLP.WrapContents = false;
            // 
            // FacultySearchPanel
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this._resultsFLP);
            this.Controls.Add(this._lblCount);
            this.Controls.Add(this._row2);
            this.Controls.Add(this._row1);
            this.Name = "FacultySearchPanel";
            this.Padding = new System.Windows.Forms.Padding(12, 10, 12, 8);
            this.Size = new System.Drawing.Size(800, 268);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FacultySearchPanel_Paint);
            this._row1.ResumeLayout(false);
            this._row1.PerformLayout();
            this._row2.ResumeLayout(false);
            this._row2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel _row1;
        private System.Windows.Forms.TextBox _txtSearch;
        private System.Windows.Forms.ComboBox _cboType;
        private System.Windows.Forms.ComboBox _cboCourse;
        private System.Windows.Forms.Button _btnSearch;
        private System.Windows.Forms.Button _btnClear;
        private System.Windows.Forms.FlowLayoutPanel _row2;
        private System.Windows.Forms.CheckBox _chkDateRange;
        private System.Windows.Forms.DateTimePicker _dtpFrom;
        private System.Windows.Forms.Label _lblTo;
        private System.Windows.Forms.DateTimePicker _dtpTo;
        private System.Windows.Forms.Label _lblCount;
        private System.Windows.Forms.FlowLayoutPanel _resultsFLP;
    }
}