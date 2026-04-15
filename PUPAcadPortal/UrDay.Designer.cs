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
            lblDay = new Label();
            lblNote = new Label();
            lblAnnouncement = new Label();
            chkSelect = new CheckBox();
            panel1 = new Panel();
            SuspendLayout();
            // 
            // lblDay
            // 
            lblDay.BackColor = Color.Transparent;
            lblDay.Font = new Font("Segoe UI", 9F);
            lblDay.Location = new Point(4, 4);
            lblDay.Name = "lblDay";
            lblDay.Size = new Size(28, 28);
            lblDay.TabIndex = 0;
            lblDay.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblNote
            // 
            lblNote.AutoEllipsis = true;
            lblNote.BackColor = Color.FromArgb(100, 100, 100);
            lblNote.Font = new Font("Segoe UI", 6.8F);
            lblNote.ForeColor = Color.White;
            lblNote.Location = new Point(2, 52);
            lblNote.Name = "lblNote";
            lblNote.Padding = new Padding(3, 0, 0, 0);
            lblNote.Size = new Size(130, 16);
            lblNote.TabIndex = 1;
            lblNote.Visible = false;
            // 
            // lblAnnouncement
            // 
            lblAnnouncement.AutoEllipsis = true;
            lblAnnouncement.BackColor = Color.FromArgb(255, 140, 0);
            lblAnnouncement.Font = new Font("Segoe UI", 6.8F);
            lblAnnouncement.ForeColor = Color.White;
            lblAnnouncement.Location = new Point(2, 70);
            lblAnnouncement.Name = "lblAnnouncement";
            lblAnnouncement.Padding = new Padding(3, 0, 0, 0);
            lblAnnouncement.Size = new Size(130, 16);
            lblAnnouncement.TabIndex = 2;
            lblAnnouncement.Visible = false;
            // 
            // chkSelect
            // 
            chkSelect.AutoSize = true;
            chkSelect.Location = new Point(110, 4);
            chkSelect.Name = "chkSelect";
            chkSelect.Size = new Size(15, 14);
            chkSelect.TabIndex = 3;
            chkSelect.Visible = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Transparent;
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(134, 141);
            panel1.TabIndex = 4;
            panel1.Click += panel1_Click;
            panel1.Paint += panel1_Paint;
            // 
            // UrDay
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(lblDay);
            Controls.Add(lblNote);
            Controls.Add(lblAnnouncement);
            Controls.Add(chkSelect);
            Controls.Add(panel1);
            Name = "UrDay";
            Size = new Size(134, 141);
            Load += UrDay_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblDay;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.Label lblAnnouncement;
        private System.Windows.Forms.CheckBox chkSelect;
        private System.Windows.Forms.Panel panel1;
    }
}
