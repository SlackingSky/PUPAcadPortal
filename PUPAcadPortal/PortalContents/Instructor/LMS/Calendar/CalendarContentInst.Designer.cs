namespace PUPAcadPorta
{
    partial class CalendarContentInst
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                FacultyCalendarDragDropBridge.Detach();
                UrDay.DaySelected -= OnDaySelected;
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            pnlCalendar = new System.Windows.Forms.Panel();
            lblMonthYear = new System.Windows.Forms.Label();
            FPLmonth = new System.Windows.Forms.FlowLayoutPanel();

            pnlCalendar.SuspendLayout();
            SuspendLayout();

            // pnlCalendar
            pnlCalendar.AutoScroll = false;
            pnlCalendar.BackColor = System.Drawing.SystemColors.Control;
            pnlCalendar.CausesValidation = false;
            pnlCalendar.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlCalendar.Location = new System.Drawing.Point(0, 0);
            pnlCalendar.Margin = new System.Windows.Forms.Padding(0);
            pnlCalendar.Name = "pnlCalendar";
            pnlCalendar.Size = new System.Drawing.Size(1272, 719);
            pnlCalendar.TabIndex = 52;

            // lblMonthYear  (hidden – replaced by toolbar label, kept for compat)
            lblMonthYear.AutoSize = false;
            lblMonthYear.Font = new System.Drawing.Font("Maiandra GD", 22F,
                                         System.Drawing.FontStyle.Bold,
                                         System.Drawing.GraphicsUnit.Point, 0);
            lblMonthYear.Location = new System.Drawing.Point(0, 0);
            lblMonthYear.Name = "lblMonthYear";
            lblMonthYear.Size = new System.Drawing.Size(0, 0);
            lblMonthYear.TabIndex = 8;
            lblMonthYear.Text = "";
            lblMonthYear.Visible = false;

            // FPLmonth  (placeholder – real instance created in BuildViewArea)
            FPLmonth.AutoScroll = true;
            FPLmonth.Location = new System.Drawing.Point(0, 0);
            FPLmonth.Name = "FPLmonth";
            FPLmonth.Size = new System.Drawing.Size(1, 1);
            FPLmonth.TabIndex = 0;
            FPLmonth.Visible = false;

            // UserControl
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(pnlCalendar);
            Name = "CalendarContentInst";
            Size = new System.Drawing.Size(1272, 719);
            Load += CalendarContentInst_Load;

            pnlCalendar.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        // ── Designer fields ───────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlCalendar;
        private System.Windows.Forms.Label lblMonthYear;
        private System.Windows.Forms.FlowLayoutPanel FPLmonth;
    }
}