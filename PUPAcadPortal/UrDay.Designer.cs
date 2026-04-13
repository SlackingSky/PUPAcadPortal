namespace PUPAcadPortal
{
    partial class UrDay
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
            panel1 = new Panel();
            chkSelect = new CheckBox();
            lblDay = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(chkSelect);
            panel1.Controls.Add(lblDay);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(1);
            panel1.Size = new Size(165, 150);
            panel1.TabIndex = 0;
            panel1.Click += panel1_Click;
            // 
            // chkSelect
            // 
            chkSelect.AutoSize = true;
            chkSelect.Location = new Point(4, 3);
            chkSelect.Name = "chkSelect";
            chkSelect.Size = new Size(15, 14);
            chkSelect.TabIndex = 1;
            chkSelect.UseVisualStyleBackColor = true;
            // 
            // lblDay
            // 
            lblDay.AutoSize = true;
            lblDay.Font = new Font("Maiandra GD", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDay.Location = new Point(134, 5);
            lblDay.Name = "lblDay";
            lblDay.Size = new Size(26, 18);
            lblDay.TabIndex = 0;
            lblDay.Text = "00";
            // 
            // UrDay
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel1);
            Name = "UrDay";
            Size = new Size(165, 150);
            Load += UrDay_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private CheckBox chkSelect;
        private Label lblDay;
    }
}
