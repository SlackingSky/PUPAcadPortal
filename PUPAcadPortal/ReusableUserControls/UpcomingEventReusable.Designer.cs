namespace PUPAcadPortal.PortalContents.Instructor.Enrollment
{
    partial class UpcomingEventReusable
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
            panel82 = new Panel();
            lblTime = new Label();
            lblTitle = new Label();
            panel83 = new Panel();
            lblMonth = new Label();
            lblDay = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel82.SuspendLayout();
            panel83.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel82
            // 
            panel82.Controls.Add(lblTime);
            panel82.Controls.Add(lblTitle);
            panel82.Controls.Add(panel83);
            panel82.Dock = DockStyle.Fill;
            panel82.Location = new Point(0, 0);
            panel82.Name = "panel82";
            panel82.Size = new Size(344, 80);
            panel82.TabIndex = 20;
            // 
            // lblTime
            // 
            lblTime.AccessibleDescription = "lblTime";
            lblTime.AutoSize = true;
            lblTime.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTime.ForeColor = Color.DimGray;
            lblTime.Location = new Point(93, 38);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(36, 17);
            lblTime.TabIndex = 19;
            lblTime.Text = "Time";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.DimGray;
            lblTitle.Location = new Point(86, 13);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(44, 21);
            lblTitle.TabIndex = 17;
            lblTitle.Text = "Title";
            // 
            // panel83
            // 
            panel83.BackColor = Color.Maroon;
            panel83.Controls.Add(tableLayoutPanel1);
            panel83.Location = new Point(3, 3);
            panel83.Name = "panel83";
            panel83.Size = new Size(70, 74);
            panel83.TabIndex = 0;
            // 
            // lblMonth
            // 
            lblMonth.Anchor = AnchorStyles.Bottom;
            lblMonth.AutoSize = true;
            lblMonth.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMonth.ForeColor = Color.White;
            lblMonth.Location = new Point(10, 12);
            lblMonth.Name = "lblMonth";
            lblMonth.Size = new Size(49, 17);
            lblMonth.TabIndex = 19;
            lblMonth.Text = "Month";
            // 
            // lblDay
            // 
            lblDay.Anchor = AnchorStyles.Top;
            lblDay.AutoSize = true;
            lblDay.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDay.ForeColor = Color.White;
            lblDay.Location = new Point(10, 29);
            lblDay.Name = "lblDay";
            lblDay.Size = new Size(50, 30);
            lblDay.TabIndex = 20;
            lblDay.Text = "Day";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(lblDay, 0, 1);
            tableLayoutPanel1.Controls.Add(lblMonth, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 40.54054F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 59.45946F));
            tableLayoutPanel1.Size = new Size(70, 74);
            tableLayoutPanel1.TabIndex = 20;
            // 
            // UpcomingEventReusable
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel82);
            Margin = new Padding(3, 3, 3, 20);
            Name = "UpcomingEventReusable";
            Size = new Size(344, 80);
            panel82.ResumeLayout(false);
            panel82.PerformLayout();
            panel83.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel82;
        private Label lblTime;
        private Label lblTitle;
        private Panel panel83;
        private Label lblMonth;
        private Label lblDay;
        private TableLayoutPanel tableLayoutPanel1;
    }
}
