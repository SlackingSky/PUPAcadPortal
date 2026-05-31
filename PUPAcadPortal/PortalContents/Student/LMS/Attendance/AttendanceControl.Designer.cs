namespace PUPAcadPortal
{
    partial class AttendanceControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // AttendanceControl
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "AttendanceControl";
            this.Size = new System.Drawing.Size(1100, 750);
            this.Load += new System.EventHandler(this.AttendanceControl_Load);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Label lblPeriodLbl;
        private System.Windows.Forms.ComboBox cmbPeriod;
        private System.Windows.Forms.ComboBox cmbCourse;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TableLayoutPanel tlpCards;
        private System.Windows.Forms.Panel pnlCardOverall;
        private System.Windows.Forms.Label lblOverallTitle;
        private System.Windows.Forms.Label lblOverallPct;
        private System.Windows.Forms.Panel pnlCardPresent;
        private System.Windows.Forms.Label lblPresentValue;
        private System.Windows.Forms.Panel pnlCardLate;
        private System.Windows.Forms.Label lblLateValue;
        private System.Windows.Forms.Panel pnlCardAbsent;
        private System.Windows.Forms.Label lblAbsentValue;
        private System.Windows.Forms.Panel pnlCardAlerts;
        private System.Windows.Forms.Label lblAlertText;
        private System.Windows.Forms.Button btnViewDetails;
        private System.Windows.Forms.Panel pnlMiniStats;
        private System.Windows.Forms.Label lblMiniPresent;
        private System.Windows.Forms.Label lblMiniLate;
        private System.Windows.Forms.Label lblMiniAbsent;
        private System.Windows.Forms.Label lblMiniExcused;
        private System.Windows.Forms.Panel lblProgressPct_UNUSED; 
        private System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.Label lblProgressPct;
        private System.Windows.Forms.Panel pnlQRTitle;
        private System.Windows.Forms.Label lblQRTitle;
        private System.Windows.Forms.DataGridView dgvQR;
        private System.Windows.Forms.Label lblSubjectsTitle;
        private System.Windows.Forms.DataGridView dgvSubjects;
        private System.Windows.Forms.Label lblAttendanceLogTitle;
        private System.Windows.Forms.DataGridView dgvLogs;
    }
}