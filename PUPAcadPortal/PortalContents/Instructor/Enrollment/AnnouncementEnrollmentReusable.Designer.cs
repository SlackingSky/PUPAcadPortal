namespace PUPAcadPortal.PortalContents.Instructor.Enrollment
{
    partial class AnnouncementEnrollmentReusable
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
            panel76 = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel77 = new Panel();
            lblDescription = new Label();
            lblTitle = new Label();
            lblDate = new Label();
            panel76.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel76
            // 
            panel76.Controls.Add(tableLayoutPanel1);
            panel76.Dock = DockStyle.Fill;
            panel76.Location = new Point(0, 0);
            panel76.Name = "panel76";
            panel76.Size = new Size(1146, 100);
            panel76.TabIndex = 19;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 35F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel77, 0, 0);
            tableLayoutPanel1.Controls.Add(lblDescription, 1, 2);
            tableLayoutPanel1.Controls.Add(lblTitle, 1, 0);
            tableLayoutPanel1.Controls.Add(lblDate, 1, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 51.7241364F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 48.2758636F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tableLayoutPanel1.Size = new Size(1146, 100);
            tableLayoutPanel1.TabIndex = 20;
            // 
            // panel77
            // 
            panel77.Anchor = AnchorStyles.Right;
            panel77.BackColor = Color.FromArgb(255, 193, 7);
            panel77.Location = new Point(27, 10);
            panel77.Name = "panel77";
            tableLayoutPanel1.SetRowSpan(panel77, 3);
            panel77.Size = new Size(5, 80);
            panel77.TabIndex = 17;
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDescription.ForeColor = Color.DimGray;
            lblDescription.Location = new Point(38, 58);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(512, 17);
            lblDescription.TabIndex = 19;
            lblDescription.Text = "Please review the updated enrollment procedures for the upcoming academic year.";
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.DimGray;
            lblTitle.Location = new Point(38, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(312, 21);
            lblTitle.TabIndex = 16;
            lblTitle.Text = "Enrollment Guidelines for AY 2025-2026";
            // 
            // lblDate
            // 
            lblDate.AutoSize = true;
            lblDate.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDate.ForeColor = Color.DimGray;
            lblDate.Location = new Point(38, 30);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(98, 17);
            lblDate.TabIndex = 18;
            lblDate.Text = "March 62, 2026";
            // 
            // AnnouncementEnrollmentReusable
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel76);
            Margin = new Padding(3, 3, 3, 20);
            Name = "AnnouncementEnrollmentReusable";
            Size = new Size(1146, 100);
            panel76.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel76;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel77;
        private Label lblDescription;
        private Label lblTitle;
        private Label lblDate;
    }
}
