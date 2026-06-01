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
            this.SuspendLayout();
            // 
            // AttendanceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "AttendanceControl";
            this.Size = new System.Drawing.Size(900, 700);
            // Link the load event so BuildUI triggers
            this.Load += new System.EventHandler(this.AttendanceControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        // ---------------------------------------------------
        // UI Fields required by the code-behind BuildUI() 
        // ---------------------------------------------------
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Label lblSemLbl;
        private System.Windows.Forms.ComboBox cmbSemester;
        private System.Windows.Forms.Label lblMonthLbl;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.Button btnRefresh;

        private System.Windows.Forms.TableLayoutPanel tlpCards;
        private System.Windows.Forms.Panel pnlCardOverall;
        private System.Windows.Forms.Label lblOverallTitle;
        private System.Windows.Forms.Label lblOverallPct;
        private System.Windows.Forms.Panel pnlCardTotal;
        private System.Windows.Forms.Label lblTotalTitle;
        private System.Windows.Forms.Label lblTotalValue;
        private System.Windows.Forms.Panel pnlCardStatus;
        private System.Windows.Forms.Label lblStatusTitle;
        private System.Windows.Forms.Label lblStatusText;
        private System.Windows.Forms.Panel pnlCardRequired;
        private System.Windows.Forms.Label lblRequiredTitle;
        private System.Windows.Forms.Label lblRequiredValue;
        private System.Windows.Forms.Panel pnlCardAlerts;
        private System.Windows.Forms.Label lblAlertText;
        private System.Windows.Forms.Button btnViewDetails;

        private System.Windows.Forms.Panel pnlMiniStats;
        private System.Windows.Forms.Label lblMiniPresent;
        private System.Windows.Forms.Label lblMiniLate;
        private System.Windows.Forms.Label lblMiniAbsent;
        private System.Windows.Forms.Label lblMiniExcused;
        private System.Windows.Forms.ProgressBar pbAttendance;
        private System.Windows.Forms.Label lblProgressPct;

        private System.Windows.Forms.Panel pnlSubjTitle;
        private System.Windows.Forms.Label lblSubjectsTitle;
        private System.Windows.Forms.DataGridView dgvSubjects;

        private System.Windows.Forms.Panel pnlLogTitle;
        private System.Windows.Forms.Label lblAttendanceLogTitle;
        private System.Windows.Forms.DataGridView dgvLogs;
    }
}