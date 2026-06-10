namespace PUPAcadPortal.ReusableUserControls
{
    partial class AdminRecentActivityReusable
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
            tableLayoutPanel1 = new TableLayoutPanel();
            lblCreatedBy = new Label();
            panel46 = new Panel();
            lblTimeAgo = new Label();
            lblActTitle = new Label();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 31F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62.78027F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28.4304924F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.699552F));
            tableLayoutPanel1.Controls.Add(lblCreatedBy, 2, 0);
            tableLayoutPanel1.Controls.Add(panel46, 0, 0);
            tableLayoutPanel1.Controls.Add(lblTimeAgo, 3, 0);
            tableLayoutPanel1.Controls.Add(lblActTitle, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1149, 64);
            tableLayoutPanel1.TabIndex = 21;
            // 
            // lblCreatedBy
            // 
            lblCreatedBy.Anchor = AnchorStyles.Left;
            lblCreatedBy.AutoSize = true;
            lblCreatedBy.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblCreatedBy.ForeColor = Color.DimGray;
            lblCreatedBy.Location = new Point(736, 23);
            lblCreatedBy.Name = "lblCreatedBy";
            lblCreatedBy.Size = new Size(71, 17);
            lblCreatedBy.TabIndex = 21;
            lblCreatedBy.Text = "Created By";
            // 
            // panel46
            // 
            panel46.Anchor = AnchorStyles.Right;
            panel46.BackColor = Color.FromArgb(255, 193, 7);
            panel46.Location = new Point(23, 17);
            panel46.Name = "panel46";
            panel46.Size = new Size(5, 30);
            panel46.TabIndex = 17;
            // 
            // lblTimeAgo
            // 
            lblTimeAgo.Anchor = AnchorStyles.Left;
            lblTimeAgo.AutoSize = true;
            lblTimeAgo.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTimeAgo.ForeColor = Color.DimGray;
            lblTimeAgo.Location = new Point(1054, 23);
            lblTimeAgo.Name = "lblTimeAgo";
            lblTimeAgo.Size = new Size(64, 17);
            lblTimeAgo.TabIndex = 20;
            lblTimeAgo.Text = "Time Ago";
            // 
            // lblActTitle
            // 
            lblActTitle.Anchor = AnchorStyles.Left;
            lblActTitle.AutoSize = true;
            lblActTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblActTitle.ForeColor = Color.DimGray;
            lblActTitle.Location = new Point(34, 21);
            lblActTitle.Name = "lblActTitle";
            lblActTitle.Size = new Size(107, 21);
            lblActTitle.TabIndex = 16;
            lblActTitle.Text = "Activity Title";
            // 
            // AdminRecentActivityReusable
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(3, 3, 3, 10);
            Name = "AdminRecentActivityReusable";
            Size = new Size(1149, 64);
            Load += AdminRecentActivityReusable_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel46;
        private Label lblTimeAgo;
        private Label lblActTitle;
        private Label lblCreatedBy;
    }
}
