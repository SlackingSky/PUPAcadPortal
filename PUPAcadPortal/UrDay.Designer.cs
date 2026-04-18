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
            this.chkSelect = new System.Windows.Forms.CheckBox();
            this.lblNote = new System.Windows.Forms.Label();
            this.lblAnnouncement = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();

            // lblDay
            this.lblDay.AutoSize = false;
            this.lblDay.Size = new System.Drawing.Size(28, 28);
            this.lblDay.Location = new System.Drawing.Point(4, 4);
            this.lblDay.Font = new System.Drawing.Font("Segoe UI", 9f);
            this.lblDay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDay.BackColor = System.Drawing.Color.Transparent;
            this.lblDay.Name = "lblDay";
            this.lblDay.TabIndex = 0;

            // chkSelect
            this.chkSelect.Location = new System.Drawing.Point(2, 2);
            this.chkSelect.Size = new System.Drawing.Size(16, 16);
            this.chkSelect.Visible = false;
            this.chkSelect.Name = "chkSelect";
            this.chkSelect.TabIndex = 1;

            // lblNote
            this.lblNote.AutoSize = false;
            this.lblNote.AutoEllipsis = true;
            this.lblNote.Text = "";
            this.lblNote.Font = new System.Drawing.Font("Segoe UI", 6.8f);
            this.lblNote.ForeColor = System.Drawing.Color.White;
            this.lblNote.BackColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.lblNote.Size = new System.Drawing.Size(156, 16);
            this.lblNote.Location = new System.Drawing.Point(2, 34);
            this.lblNote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblNote.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblNote.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblNote.Visible = false;
            this.lblNote.Name = "lblNote";
            this.lblNote.TabIndex = 2;

            // lblAnnouncement
            this.lblAnnouncement.AutoSize = false;
            this.lblAnnouncement.AutoEllipsis = true;
            this.lblAnnouncement.Text = "";
            this.lblAnnouncement.Font = new System.Drawing.Font("Segoe UI", 6.8f);
            this.lblAnnouncement.ForeColor = System.Drawing.Color.White;
            this.lblAnnouncement.BackColor = System.Drawing.Color.FromArgb(255, 140, 0);
            this.lblAnnouncement.Size = new System.Drawing.Size(156, 16);
            this.lblAnnouncement.Location = new System.Drawing.Point(2, 52);
            this.lblAnnouncement.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblAnnouncement.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblAnnouncement.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblAnnouncement.Visible = false;
            this.lblAnnouncement.Name = "lblAnnouncement";
            this.lblAnnouncement.TabIndex = 3;

            // panel1
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Name = "panel1";
            this.panel1.TabIndex = 4;
            this.panel1.Click += new System.EventHandler(this.panel1_Click);
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);

            // UserControl
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Size = new System.Drawing.Size(160, 110);
            this.Name = "UrDay";
            this.Load += new System.EventHandler(this.UrDay_Load);
            this.Controls.Add(this.lblDay);
            this.Controls.Add(this.chkSelect);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.lblAnnouncement);
            this.Controls.Add(this.panel1);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblDay;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.Label lblAnnouncement;
        private System.Windows.Forms.CheckBox chkSelect;
        private System.Windows.Forms.Panel panel1;
    }
}
