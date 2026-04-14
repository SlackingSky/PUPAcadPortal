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
            panel1 = new Panel();
            chkSelect = new CheckBox();
            lblDay = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(chkSelect);
            panel1.Controls.Add(lblDay);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(1);
            panel1.Size = new Size(165, 150);
            panel1.TabIndex = 0;
            panel1.Click += panel1_Click;
            panel1.Paint += panel1_Paint;
            // 
            // chkSelect
            // 
            chkSelect.AutoSize = true;
            chkSelect.Location = new Point(4, 3);
            chkSelect.Name = "chkSelect";
            chkSelect.Size = new Size(15, 14);
            chkSelect.TabIndex = 1;
            chkSelect.UseVisualStyleBackColor = true;
            // 
            // lblDay
            // 
            lblDay.AutoSize = true;
            lblDay.Font = new Font("Maiandra GD", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDay.Location = new Point(134, 5);
            lblDay.Name = "lblDay";
            lblDay.Size = new Size(26, 18);
            lblDay.TabIndex = 0;
            lblDay.Text = "00";
            // 
            // 
            // lblNote
            // 
            this.lblNote = new System.Windows.Forms.Label();
            this.lblNote.AutoSize = false;
            this.lblNote.AutoEllipsis = true;
            this.lblNote.UseMnemonic = false;
            this.lblNote.Font = new System.Drawing.Font("Segoe UI", 6.8f, System.Drawing.FontStyle.Regular);
            this.lblNote.ForeColor = System.Drawing.Color.White;
            this.lblNote.BackColor = System.Drawing.Color.FromArgb(66, 133, 244);   // Blue
            this.lblNote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblNote.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNote.Location = new System.Drawing.Point(2, 56);
            this.lblNote.Size = new System.Drawing.Size(120, 18);
            this.lblNote.Anchor = System.Windows.Forms.AnchorStyles.Top
                                | System.Windows.Forms.AnchorStyles.Left
                                | System.Windows.Forms.AnchorStyles.Right;
            this.lblNote.Visible = false;
            this.lblNote.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblNote.Name = "lblNote";
            // 
            // lblAnnouncement
            // 
            this.lblAnnouncement = new System.Windows.Forms.Label();
            this.lblAnnouncement.AutoSize = false;
            this.lblAnnouncement.AutoEllipsis = true;
            this.lblAnnouncement.UseMnemonic = false;
            this.lblAnnouncement.Font = new System.Drawing.Font("Segoe UI", 6.8f, System.Drawing.FontStyle.Regular);
            this.lblAnnouncement.ForeColor = System.Drawing.Color.White;
            this.lblAnnouncement.BackColor = System.Drawing.Color.FromArgb(230, 81, 0);  // Orange
            this.lblAnnouncement.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblAnnouncement.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnnouncement.Location = new System.Drawing.Point(2, 76);
            this.lblAnnouncement.Size = new System.Drawing.Size(120, 18);
            this.lblAnnouncement.Anchor = System.Windows.Forms.AnchorStyles.Top
                                         | System.Windows.Forms.AnchorStyles.Left
                                         | System.Windows.Forms.AnchorStyles.Right;
            this.lblAnnouncement.Visible = false;
            this.lblAnnouncement.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblAnnouncement.Name = "lblAnnouncement";

            // Add them to the control
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.lblAnnouncement);
            // UrDay
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel1);
            Name = "UrDay";
            Size = new Size(165, 150);
            Load += UrDay_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private CheckBox chkSelect;
        private Label lblDay;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.Label lblAnnouncement;
    }
}
