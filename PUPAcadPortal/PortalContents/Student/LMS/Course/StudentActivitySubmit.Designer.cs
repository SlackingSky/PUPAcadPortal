namespace PUPAcadPortal
{
    partial class StudentActivitySubmit
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnBack = new buttonRounded();
            this.lblActivityTitle = new System.Windows.Forms.Label();
            this.lblMeta = new System.Windows.Forms.Label();
            this.pnlBody = new System.Windows.Forms.Panel();

            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();

            this.pnlHeader.BackColor = System.Drawing.Color.Maroon;
            this.pnlHeader.Controls.Add(this.btnBack);
            this.pnlHeader.Controls.Add(this.lblActivityTitle);
            this.pnlHeader.Controls.Add(this.lblMeta);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1640, 58);
            this.pnlHeader.TabIndex = 1;

            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnBack.BorderRadius = 10;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(10, 13);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(80, 32);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "← Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            this.lblActivityTitle.AutoSize = true;
            this.lblActivityTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblActivityTitle.ForeColor = System.Drawing.Color.White;
            this.lblActivityTitle.Location = new System.Drawing.Point(102, 8);
            this.lblActivityTitle.Name = "lblActivityTitle";
            this.lblActivityTitle.Size = new System.Drawing.Size(126, 25);
            this.lblActivityTitle.TabIndex = 1;
            this.lblActivityTitle.Text = "Activity Title";
            this.lblMeta.AutoSize = true;
            this.lblMeta.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMeta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.lblMeta.Location = new System.Drawing.Point(102, 36);
            this.lblMeta.Name = "lblMeta";
            this.lblMeta.Size = new System.Drawing.Size(130, 15);
            this.lblMeta.TabIndex = 2;
            this.lblMeta.Text = "Type  ·  0 pts  ·  Due —";
            this.pnlBody.AutoScroll = true;
            this.pnlBody.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(0, 58);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Padding = new System.Windows.Forms.Padding(10);
            this.pnlBody.Size = new System.Drawing.Size(1640, 931);
            this.pnlBody.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlHeader);
            this.Name = "StudentActivitySubmit";
            this.Size = new System.Drawing.Size(1640, 989);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private buttonRounded btnBack;
        private System.Windows.Forms.Label lblActivityTitle;
        private System.Windows.Forms.Label lblMeta;
        private System.Windows.Forms.Panel pnlBody;
    }
}