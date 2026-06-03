namespace PUPAcadPortal
{
    partial class CalendarContentInst
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
            lblMonthYear = new Label();
            FPLmonth = new FlowLayoutPanel();
            pnlCalendar.SuspendLayout();
            SuspendLayout();
            // 
            // pnlCalendar
            // 
            pnlCalendar.AutoScroll = true;
            pnlCalendar.BackColor = SystemColors.Control;
            pnlCalendar.CausesValidation = false;
            pnlCalendar.Controls.Add(lblMonthYear);
            pnlCalendar.Controls.Add(FPLmonth);
            pnlCalendar.Dock = DockStyle.Fill;
            pnlCalendar.Location = new Point(0, 0);
            pnlCalendar.Margin = new Padding(0);
            pnlCalendar.Name = "pnlCalendar";
            pnlCalendar.Size = new Size(1272, 719);
            pnlCalendar.TabIndex = 52;
            // 
            // lblMonthYear
            // 
            lblMonthYear.AutoSize = true;
            lblMonthYear.Font = new Font("Maiandra GD", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMonthYear.Location = new Point(540, 5);
            lblMonthYear.Name = "lblMonthYear";
            lblMonthYear.Size = new Size(208, 39);
            lblMonthYear.TabIndex = 8;
            lblMonthYear.Text = "Month 0000";
            lblMonthYear.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FPLmonth
            // 
            FPLmonth.AutoScroll = true;
            FPLmonth.Location = new Point(24, 72);
            FPLmonth.Name = "FPLmonth";
            FPLmonth.Size = new Size(1239, 635);
            FPLmonth.TabIndex = 0;
            // 
            // CalendarContentInst
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlCalendar);
            Name = "CalendarContentInst";
            Size = new Size(1272, 719);
            Load += CalendarContentInst_Load;
            pnlCalendar.ResumeLayout(false);
            pnlCalendar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlCalendar;
        private Label lblMonthYear;
        private FlowLayoutPanel FPLmonth;
    }
}
