namespace PUPAcadPortal
{
    partial class studentlist
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
            roundedPanel33 = new RoundedPanel();
            roundedPanel34 = new RoundedPanel();
            lblScore = new Label();
            button9 = new buttonRounded();
            label125 = new Label();
            label124 = new Label();
            label123 = new Label();
            pictureBox48 = new PictureBox();
            roundedPanel33.SuspendLayout();
            roundedPanel34.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox48).BeginInit();
            SuspendLayout();
            // 
            // roundedPanel33
            // 
            roundedPanel33.BackColor = Color.Maroon;
            roundedPanel33.BorderRadius = 10;
            roundedPanel33.Controls.Add(roundedPanel34);
            roundedPanel33.Dock = DockStyle.Fill;
            roundedPanel33.Location = new Point(0, 0);
            roundedPanel33.Name = "roundedPanel33";
            roundedPanel33.Size = new Size(620, 75);
            roundedPanel33.TabIndex = 2;
            // 
            // roundedPanel34
            // 
            roundedPanel34.BackColor = Color.White;
            roundedPanel34.BorderRadius = 10;
            roundedPanel34.Controls.Add(lblScore);
            roundedPanel34.Controls.Add(button9);
            roundedPanel34.Controls.Add(label125);
            roundedPanel34.Controls.Add(label124);
            roundedPanel34.Controls.Add(label123);
            roundedPanel34.Controls.Add(pictureBox48);
            roundedPanel34.Location = new Point(8, 5);
            roundedPanel34.Name = "roundedPanel34";
            roundedPanel34.Size = new Size(601, 64);
            roundedPanel34.TabIndex = 2;
            // 
            // lblScore
            // 
            lblScore.BorderStyle = BorderStyle.Fixed3D;
            lblScore.Font = new Font("Arial", 15.75F, FontStyle.Bold);
            lblScore.Location = new Point(300, 33);
            lblScore.Name = "lblScore";
            lblScore.Size = new Size(218, 24);
            lblScore.TabIndex = 12;
            // 
            // button9
            // 
            button9.BackColor = Color.FromArgb(128, 0, 0);
            button9.BorderRadius = 20;
            button9.FlatAppearance.BorderSize = 0;
            button9.FlatStyle = FlatStyle.Flat;
            button9.ForeColor = Color.White;
            button9.Location = new Point(524, 26);
            button9.Name = "button9";
            button9.Size = new Size(74, 35);
            button9.TabIndex = 11;
            button9.Text = "Check";
            button9.UseVisualStyleBackColor = false;
            // 
            // label125
            // 
            label125.AutoSize = true;
            label125.Font = new Font("Arial", 15.75F, FontStyle.Bold);
            label125.Location = new Point(229, 34);
            label125.Name = "label125";
            label125.Size = new Size(76, 24);
            label125.TabIndex = 4;
            label125.Text = "Score:";
            // 
            // label124
            // 
            label124.AutoSize = true;
            label124.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label124.Location = new Point(85, 39);
            label124.Name = "label124";
            label124.Size = new Size(138, 19);
            label124.TabIndex = 3;
            label124.Text = "2024-00192-SM-0";
            // 
            // label123
            // 
            label123.AutoSize = true;
            label123.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label123.Location = new Point(85, 5);
            label123.Name = "label123";
            label123.Size = new Size(220, 19);
            label123.TabIndex = 2;
            label123.Text = "ABLONG, ADRIAN PLATINO";
            // 
            // pictureBox48
            // 
            pictureBox48.Image = Properties.Resources.profile;
            pictureBox48.Location = new Point(7, 3);
            pictureBox48.Name = "pictureBox48";
            pictureBox48.Size = new Size(63, 57);
            pictureBox48.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox48.TabIndex = 2;
            pictureBox48.TabStop = false;
            // 
            // studentlist
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(roundedPanel33);
            Name = "studentlist";
            Size = new Size(620, 75);
            roundedPanel33.ResumeLayout(false);
            roundedPanel34.ResumeLayout(false);
            roundedPanel34.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox48).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private RoundedPanel roundedPanel33;
        private RoundedPanel roundedPanel34;
        private Label lblScore;
        private buttonRounded button9;
        private Label label125;
        private Label label124;
        private Label label123;
        private PictureBox pictureBox48;
    }
}
