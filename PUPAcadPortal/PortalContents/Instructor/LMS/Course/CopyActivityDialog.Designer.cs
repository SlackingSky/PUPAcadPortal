namespace PUPAcadPortal
{
    partial class CopyActivityDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSub = new System.Windows.Forms.Label();
            this.lblPickLabel = new System.Windows.Forms.Label();
            this.lstCourses = new System.Windows.Forms.ListBox();
            this.lblOpts = new System.Windows.Forms.Label();
            this.chkCopyFiles = new System.Windows.Forms.CheckBox();
            this.chkCopyRubric = new System.Windows.Forms.CheckBox();
            this.chkCopyQuestions = new System.Windows.Forms.CheckBox();
            this.lblStatus = new System.Windows.Forms.Label();

            // Assuming buttonRounded is in the same namespace
            this.btnCopy = new PUPAcadPortal.buttonRounded();
            this.btnCancel = new PUPAcadPortal.buttonRounded();

            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSub);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(504, 70);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(16, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(480, 28);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Copy Activity";
            // 
            // lblSub
            // 
            this.lblSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(185)))), ((int)(((byte)(185)))));
            this.lblSub.Location = new System.Drawing.Point(16, 42);
            this.lblSub.Name = "lblSub";
            this.lblSub.Size = new System.Drawing.Size(480, 18);
            this.lblSub.TabIndex = 1;
            // 
            // lblPickLabel
            // 
            this.lblPickLabel.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPickLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(60)))));
            this.lblPickLabel.Location = new System.Drawing.Point(18, 86);
            this.lblPickLabel.Name = "lblPickLabel";
            this.lblPickLabel.Size = new System.Drawing.Size(480, 20);
            this.lblPickLabel.TabIndex = 1;
            this.lblPickLabel.Text = "Select destination course section:";
            // 
            // lstCourses
            // 
            this.lstCourses.BackColor = System.Drawing.Color.White;
            this.lstCourses.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstCourses.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lstCourses.FormattingEnabled = true;
            this.lstCourses.ItemHeight = 17;
            this.lstCourses.Location = new System.Drawing.Point(18, 114);
            this.lstCourses.Name = "lstCourses";
            this.lstCourses.Size = new System.Drawing.Size(470, 155);
            this.lstCourses.TabIndex = 2;
            // 
            // lblOpts
            // 
            this.lblOpts.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblOpts.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(60)))));
            this.lblOpts.Location = new System.Drawing.Point(18, 282);
            this.lblOpts.Name = "lblOpts";
            this.lblOpts.Size = new System.Drawing.Size(480, 20);
            this.lblOpts.TabIndex = 3;
            this.lblOpts.Text = "Copy options:";
            // 
            // chkCopyFiles
            // 
            this.chkCopyFiles.Checked = true;
            this.chkCopyFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCopyFiles.Location = new System.Drawing.Point(22, 308);
            this.chkCopyFiles.Name = "chkCopyFiles";
            this.chkCopyFiles.Size = new System.Drawing.Size(250, 22);
            this.chkCopyFiles.TabIndex = 4;
            this.chkCopyFiles.Text = "Include attached files";
            this.chkCopyFiles.UseVisualStyleBackColor = true;
            // 
            // chkCopyRubric
            // 
            this.chkCopyRubric.Checked = true;
            this.chkCopyRubric.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCopyRubric.Location = new System.Drawing.Point(22, 334);
            this.chkCopyRubric.Name = "chkCopyRubric";
            this.chkCopyRubric.Size = new System.Drawing.Size(250, 22);
            this.chkCopyRubric.TabIndex = 5;
            this.chkCopyRubric.Text = "Include rubric criteria";
            this.chkCopyRubric.UseVisualStyleBackColor = true;
            // 
            // chkCopyQuestions
            // 
            this.chkCopyQuestions.Checked = true;
            this.chkCopyQuestions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCopyQuestions.Location = new System.Drawing.Point(22, 360);
            this.chkCopyQuestions.Name = "chkCopyQuestions";
            this.chkCopyQuestions.Size = new System.Drawing.Size(250, 22);
            this.chkCopyQuestions.TabIndex = 6;
            this.chkCopyQuestions.Text = "Include quiz questions";
            this.chkCopyQuestions.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(18, 398);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(480, 18);
            this.lblStatus.TabIndex = 7;
            // 
            // btnCopy
            // 
            this.btnCopy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnCopy.BorderRadius = 10;
            this.btnCopy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCopy.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnCopy.ForeColor = System.Drawing.Color.White;
            this.btnCopy.Location = new System.Drawing.Point(18, 426);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(130, 34);
            this.btnCopy.TabIndex = 8;
            this.btnCopy.Text = "Copy Activity";
            this.btnCopy.Click += new System.EventHandler(this.BtnCopy_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(110)))));
            this.btnCancel.BorderRadius = 10;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(158, 426);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 34);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            // 
            // CopyActivityDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(248)))));
            this.ClientSize = new System.Drawing.Size(504, 480);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.chkCopyQuestions);
            this.Controls.Add(this.chkCopyRubric);
            this.Controls.Add(this.chkCopyFiles);
            this.Controls.Add(this.lblOpts);
            this.Controls.Add(this.lstCourses);
            this.Controls.Add(this.lblPickLabel);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CopyActivityDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Copy Activity";
            this.pnlHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSub;
        private System.Windows.Forms.Label lblPickLabel;
        private System.Windows.Forms.ListBox lstCourses;
        private System.Windows.Forms.Label lblOpts;
        private System.Windows.Forms.CheckBox chkCopyFiles;
        private System.Windows.Forms.CheckBox chkCopyRubric;
        private System.Windows.Forms.CheckBox chkCopyQuestions;
        private System.Windows.Forms.Label lblStatus;
        private PUPAcadPortal.buttonRounded btnCopy;
        private PUPAcadPortal.buttonRounded btnCancel;
    }
}