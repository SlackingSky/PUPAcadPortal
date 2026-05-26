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
            pnlCalendar = new Panel();
            picPrev = new PictureBox();
            picNext = new PictureBox();
            lblMonthYear = new Label();
            FPLmonth = new FlowLayoutPanel();
            pnlCalendar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picPrev).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picNext).BeginInit();
            SuspendLayout();
            // 
            // pnlCalendar
            // 
            pnlCalendar.AutoScroll = true;
            pnlCalendar.BackColor = SystemColors.Control;
            pnlCalendar.CausesValidation = false;
            pnlCalendar.Controls.Add(picPrev);
            pnlCalendar.Controls.Add(picNext);
            pnlCalendar.Controls.Add(lblMonthYear);
            pnlCalendar.Controls.Add(FPLmonth);
            pnlCalendar.Dock = DockStyle.Fill;
            pnlCalendar.Location = new Point(0, 0);
            pnlCalendar.Margin = new Padding(0);
            pnlCalendar.Name = "pnlCalendar";
            pnlCalendar.Size = new Size(1272, 719);
            pnlCalendar.TabIndex = 34;
            // 
            // picPrev
            // 
            picPrev.Cursor = Cursors.Hand;
            picPrev.Location = new Point(11, 8);
            picPrev.Name = "picPrev";
            picPrev.Size = new Size(32, 32);
            picPrev.SizeMode = PictureBoxSizeMode.Zoom;
            picPrev.TabIndex = 11;
            picPrev.TabStop = false;
            // 
            // picNext
            // 
            picNext.Cursor = Cursors.Hand;
            picNext.Location = new Point(52, 8);
            picNext.Name = "picNext";
            picNext.Size = new Size(32, 32);
            picNext.SizeMode = PictureBoxSizeMode.Zoom;
            picNext.TabIndex = 12;
            picNext.TabStop = false;
            // 
            // lblMonthYear
            // 
            lblMonthYear.AutoSize = true;
            lblMonthYear.Font = new Font("Maiandra GD", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMonthYear.Location = new Point(527, 1);
            lblMonthYear.Name = "lblMonthYear";
            lblMonthYear.Size = new Size(208, 39);
            lblMonthYear.TabIndex = 10;
            lblMonthYear.Text = "Month 0000";
            lblMonthYear.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FPLmonth
            // 
            FPLmonth.AutoScroll = true;
            FPLmonth.Location = new Point(11, 68);
            FPLmonth.Name = "FPLmonth";
            FPLmonth.Size = new Size(1239, 635);
            FPLmonth.TabIndex = 9;
            // 
            // CalendarContentStudent
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlCalendar);
            Name = "CalendarContentStudent";
            Size = new Size(1272, 719);
            pnlCalendar.ResumeLayout(false);
            pnlCalendar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picPrev).EndInit();
            ((System.ComponentModel.ISupportInitialize)picNext).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlCalendar;
        private PictureBox picPrev;
        private PictureBox picNext;
        private Label lblMonthYear;
        private FlowLayoutPanel FPLmonth;
    }
}
