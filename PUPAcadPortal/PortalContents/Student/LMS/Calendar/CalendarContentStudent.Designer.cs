namespace PUPAcadPortal.PortalContents.Student.LMS
{
    partial class CalendarContentStudent
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
            pnlCalendar = new System.Windows.Forms.Panel();
            // Navigation picture boxes kept for CalendarWheelFilter compatibility
            picPrev = new System.Windows.Forms.PictureBox();
            picNext = new System.Windows.Forms.PictureBox();
            // lblMonthYear and FPLmonth are legacy stubs kept for
            // any external references; they are hidden at runtime
            lblMonthYear = new System.Windows.Forms.Label();
            FPLmonth = new System.Windows.Forms.FlowLayoutPanel();

            pnlCalendar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picPrev).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picNext).BeginInit();
            SuspendLayout();

            // ── pnlCalendar ──────────────────────────────────────
            pnlCalendar.AutoScroll = false;
            pnlCalendar.BackColor = System.Drawing.SystemColors.Control;
            pnlCalendar.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlCalendar.Location = new System.Drawing.Point(0, 0);
            pnlCalendar.Margin = new System.Windows.Forms.Padding(0);
            pnlCalendar.Name = "pnlCalendar";
            pnlCalendar.Size = new System.Drawing.Size(1272, 719);
            pnlCalendar.TabIndex = 34;

            // ── picPrev (hidden – kept for CalendarWheelFilter) ──
            picPrev.Location = new System.Drawing.Point(-100, -100);
            picPrev.Name = "picPrev";
            picPrev.Size = new System.Drawing.Size(1, 1);
            picPrev.TabIndex = 11;
            picPrev.TabStop = false;
            picPrev.Visible = false;

            // ── picNext (hidden – kept for CalendarWheelFilter) ──
            picNext.Location = new System.Drawing.Point(-100, -100);
            picNext.Name = "picNext";
            picNext.Size = new System.Drawing.Size(1, 1);
            picNext.TabIndex = 12;
            picNext.TabStop = false;
            picNext.Visible = false;

            // ── lblMonthYear (hidden legacy stub) ────────────────
            lblMonthYear.AutoSize = false;
            lblMonthYear.Location = new System.Drawing.Point(-200, -200);
            lblMonthYear.Name = "lblMonthYear";
            lblMonthYear.Size = new System.Drawing.Size(1, 1);
            lblMonthYear.TabIndex = 10;
            lblMonthYear.Visible = false;

            // ── FPLmonth (hidden legacy stub) ────────────────────
            FPLmonth.Location = new System.Drawing.Point(-200, -200);
            FPLmonth.Name = "FPLmonth";
            FPLmonth.Size = new System.Drawing.Size(1, 1);
            FPLmonth.TabIndex = 9;
            FPLmonth.Visible = false;

            // ── CalendarContentStudent ───────────────────────────
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            pnlCalendar.Controls.Add(picPrev);
            pnlCalendar.Controls.Add(picNext);
            pnlCalendar.Controls.Add(lblMonthYear);
            pnlCalendar.Controls.Add(FPLmonth);
            Controls.Add(pnlCalendar);
            Name = "CalendarContentStudent";
            Size = new System.Drawing.Size(1272, 719);

            pnlCalendar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picPrev).EndInit();
            ((System.ComponentModel.ISupportInitialize)picNext).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlCalendar;
        private System.Windows.Forms.PictureBox picPrev;
        private System.Windows.Forms.PictureBox picNext;
        private System.Windows.Forms.Label lblMonthYear;
        private System.Windows.Forms.FlowLayoutPanel FPLmonth;
    }
}