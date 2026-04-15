namespace PUPAcadPortal
{
    partial class UrDay
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
            this.lblDay = new System.Windows.Forms.Label();
            this.lblNote = new System.Windows.Forms.Label();
            this.lblAnnouncement = new System.Windows.Forms.Label();
            this.chkSelect = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(134, 110);
            this.panel1.TabIndex = 4;
            this.panel1.Click += new System.EventHandler(this.panel1_Click);
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // chkSelect
            // 
            this.chkSelect.AutoSize = true;
            this.chkSelect.Location = new System.Drawing.Point(110, 4);
            this.chkSelect.Name = "chkSelect";
            this.chkSelect.Size = new System.Drawing.Size(15, 14);
            this.chkSelect.TabIndex = 3;
            this.chkSelect.Visible = false;
            // 
            // lblDay
            // 
            this.lblDay.AutoSize = false;
            this.lblDay.BackColor = System.Drawing.Color.Transparent;
            this.lblDay.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDay.Location = new System.Drawing.Point(4, 4);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(28, 28);
            this.lblDay.TabIndex = 0;
            this.lblDay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // 
            // lblNote
            // 
            this.lblNote.AutoEllipsis = true;
            this.lblNote.AutoSize = false;
            this.lblNote.BackColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.lblNote.Font = new System.Drawing.Font("Segoe UI", 6.8F);
            this.lblNote.ForeColor = System.Drawing.Color.White;
            this.lblNote.Location = new System.Drawing.Point(2, 52);
            this.lblNote.Name = "lblNote";
            this.lblNote.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblNote.Size = new System.Drawing.Size(130, 16);
            this.lblNote.TabIndex = 1;
            this.lblNote.Visible = false;
            // 
            // lblAnnouncement
            // 
            this.lblAnnouncement.AutoEllipsis = true;
            this.lblAnnouncement.AutoSize = false;
            this.lblAnnouncement.BackColor = System.Drawing.Color.FromArgb(255, 140, 0);
            this.lblAnnouncement.Font = new System.Drawing.Font("Segoe UI", 6.8F);
            this.lblAnnouncement.ForeColor = System.Drawing.Color.White;
            this.lblAnnouncement.Location = new System.Drawing.Point(2, 70);
            this.lblAnnouncement.Name = "lblAnnouncement";
            this.lblAnnouncement.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblAnnouncement.Size = new System.Drawing.Size(130, 16);
            this.lblAnnouncement.TabIndex = 2;
            this.lblAnnouncement.Visible = false;

            // Add them to the control
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.lblAnnouncement);
            // UrDay
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblDay);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.lblAnnouncement);
            this.Controls.Add(this.chkSelect);
            this.Controls.Add(this.panel1);
            this.Name = "UrDay";
            this.Size = new System.Drawing.Size(134, 110);
            this.Load += new System.EventHandler(this.UrDay_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblDay;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.Label lblAnnouncement;
        private System.Windows.Forms.CheckBox chkSelect;
        private System.Windows.Forms.Panel panel1;
    }
}
