namespace PUPAcadPortal
{
    partial class ActivityItem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActivityItem));
            roundedPanel8 = new RoundedPanel();
            roundedPanel7 = new RoundedPanel();
            btnEdit = new Button();
            lblDueDate = new Label();
            lblTitle = new Label();
            actPic = new PictureBox();
            btnRemove = new Button();
            btnPostAct = new buttonRounded();
            roundedPanel8.SuspendLayout();
            roundedPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)actPic).BeginInit();
            SuspendLayout();
            // 
            // roundedPanel8
            // 
            roundedPanel8.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            roundedPanel8.BackColor = Color.Maroon;
            roundedPanel8.BackgroundImageLayout = ImageLayout.Center;
            roundedPanel8.BorderRadius = 10;
            roundedPanel8.BorderStyle = BorderStyle.FixedSingle;
            roundedPanel8.Controls.Add(roundedPanel7);
            roundedPanel8.Location = new Point(2, 6);
            roundedPanel8.Margin = new Padding(3, 0, 3, 3);
            roundedPanel8.Name = "roundedPanel8";
            roundedPanel8.Size = new Size(1414, 174);
            roundedPanel8.TabIndex = 4;
            // 
            // roundedPanel7
            // 
            roundedPanel7.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            roundedPanel7.BackColor = Color.White;
            roundedPanel7.BackgroundImageLayout = ImageLayout.Center;
            roundedPanel7.BorderRadius = 10;
            roundedPanel7.BorderStyle = BorderStyle.FixedSingle;
            roundedPanel7.Controls.Add(btnEdit);
            roundedPanel7.Controls.Add(lblDueDate);
            roundedPanel7.Controls.Add(lblTitle);
            roundedPanel7.Controls.Add(actPic);
            roundedPanel7.Controls.Add(btnRemove);
            roundedPanel7.Controls.Add(btnPostAct);
            roundedPanel7.Location = new Point(20, 4);
            roundedPanel7.Name = "roundedPanel7";
            roundedPanel7.Size = new Size(1385, 164);
            roundedPanel7.TabIndex = 2;
            // 
            // btnEdit
            // 
            btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEdit.BackColor = Color.Transparent;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Font = new Font("Arial", 16.2F, FontStyle.Regular, GraphicsUnit.Pixel);
            btnEdit.ForeColor = Color.White;
            btnEdit.Image = (Image)resources.GetObject("btnEdit.Image");
            btnEdit.Location = new Point(1168, 58);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(44, 43);
            btnEdit.TabIndex = 13;
            btnEdit.UseVisualStyleBackColor = false;
            // 
            // lblDueDate
            // 
            lblDueDate.AutoSize = true;
            lblDueDate.Font = new Font("Segoe UI Semibold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDueDate.Location = new Point(208, 82);
            lblDueDate.Name = "lblDueDate";
            lblDueDate.Size = new Size(0, 31);
            lblDueDate.TabIndex = 12;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 16.8F, FontStyle.Bold);
            lblTitle.Location = new Point(206, 44);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(0, 38);
            lblTitle.TabIndex = 11;
            // 
            // actPic
            // 
            actPic.Location = new Point(82, 39);
            actPic.Name = "actPic";
            actPic.Size = new Size(80, 77);
            actPic.SizeMode = PictureBoxSizeMode.StretchImage;
            actPic.TabIndex = 9;
            actPic.TabStop = false;
            // 
            // btnRemove
            // 
            btnRemove.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRemove.BackColor = Color.Transparent;
            btnRemove.FlatStyle = FlatStyle.Flat;
            btnRemove.Font = new Font("Arial", 16.2F, FontStyle.Regular, GraphicsUnit.Pixel);
            btnRemove.ForeColor = Color.White;
            btnRemove.Image = (Image)resources.GetObject("btnRemove.Image");
            btnRemove.Location = new Point(1107, 58);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(44, 43);
            btnRemove.TabIndex = 9;
            btnRemove.UseVisualStyleBackColor = false;
            btnRemove.Click += btnRemove_Click;
            // 
            // btnPostAct
            // 
            btnPostAct.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPostAct.BackColor = Color.FromArgb(128, 0, 0);
            btnPostAct.BorderRadius = 10;
            btnPostAct.FlatAppearance.BorderSize = 0;
            btnPostAct.FlatStyle = FlatStyle.Flat;
            btnPostAct.Font = new Font("Arial", 16.2F, FontStyle.Regular, GraphicsUnit.Pixel);
            btnPostAct.ForeColor = Color.White;
            btnPostAct.Location = new Point(1229, 58);
            btnPostAct.Name = "btnPostAct";
            btnPostAct.Size = new Size(130, 43);
            btnPostAct.TabIndex = 10;
            btnPostAct.Text = "Post";
            btnPostAct.UseVisualStyleBackColor = false;
            btnPostAct.Click += btnPostAct_Click;
            // 
            // ActivityItem
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            Controls.Add(roundedPanel8);
            Name = "ActivityItem";
            Size = new Size(1419, 187);
            roundedPanel8.ResumeLayout(false);
            roundedPanel7.ResumeLayout(false);
            roundedPanel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)actPic).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private RoundedPanel roundedPanel8;
        private RoundedPanel roundedPanel7;
        public Label lblDueDate;
        public Label lblTitle;
        public PictureBox actPic;
        public Button btnEdit;
        public Button btnRemove;
        public buttonRounded btnPostAct;
    }
}
