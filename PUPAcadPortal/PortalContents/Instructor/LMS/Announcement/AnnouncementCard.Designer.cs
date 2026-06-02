namespace PUPAcadPortal
{
    partial class AnnouncementCard
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
            components = new System.ComponentModel.Container();
            _iconCircle = new Panel();
            _lblNew = new Label();
            _lblPin = new Label();
            _lblTitle = new Label();
            _lblCatPill = new Label();
            _lblInactive = new Label();
            _lblDesc = new Label();
            _lblAuthor = new Label();
            _lblViewed = new Label();
            _progressTrack = new Panel();
            _progressFill = new Panel();
            _btnMenu = new Button();
            _ctxMenu = new ContextMenuStrip(components);
            _progressTrack.SuspendLayout();
            SuspendLayout();
            // 
            // _iconCircle
            // 
            _iconCircle.Location = new Point(12, 12);
            _iconCircle.Name = "_iconCircle";
            _iconCircle.Size = new Size(40, 40);
            _iconCircle.TabIndex = 0;
            _iconCircle.Paint += IconCircle_Paint;
            // 
            // _lblNew
            // 
            _lblNew.BackColor = Color.FromArgb(22, 163, 74);
            _lblNew.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            _lblNew.ForeColor = Color.White;
            _lblNew.Location = new Point(0, 0);
            _lblNew.Name = "_lblNew";
            _lblNew.Size = new Size(38, 17);
            _lblNew.TabIndex = 1;
            _lblNew.Text = "NEW";
            _lblNew.TextAlign = ContentAlignment.MiddleCenter;
            _lblNew.Visible = false;
            // 
            // _lblPin
            // 
            _lblPin.AutoSize = true;
            _lblPin.Font = new Font("Segoe UI", 9F);
            _lblPin.Location = new Point(0, 0);
            _lblPin.Name = "_lblPin";
            _lblPin.Size = new Size(19, 15);
            _lblPin.TabIndex = 2;
            _lblPin.Text = "📌";
            _lblPin.Visible = false;
            // 
            // _lblTitle
            // 
            _lblTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _lblTitle.ForeColor = Color.FromArgb(25, 25, 25);
            _lblTitle.Location = new Point(0, 0);
            _lblTitle.Name = "_lblTitle";
            _lblTitle.Size = new Size(100, 22);
            _lblTitle.TabIndex = 3;
            _lblTitle.Click += ToggleExpand_Event;
            // 
            // _lblCatPill
            // 
            _lblCatPill.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            _lblCatPill.Location = new Point(0, 0);
            _lblCatPill.Name = "_lblCatPill";
            _lblCatPill.Size = new Size(100, 20);
            _lblCatPill.TabIndex = 4;
            _lblCatPill.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // _lblInactive
            // 
            _lblInactive.BackColor = Color.FromArgb(240, 240, 240);
            _lblInactive.Font = new Font("Segoe UI", 7.5F);
            _lblInactive.ForeColor = Color.FromArgb(100, 100, 100);
            _lblInactive.Location = new Point(0, 0);
            _lblInactive.Name = "_lblInactive";
            _lblInactive.Size = new Size(58, 18);
            _lblInactive.TabIndex = 5;
            _lblInactive.Text = "Inactive";
            _lblInactive.TextAlign = ContentAlignment.MiddleCenter;
            _lblInactive.Visible = false;
            // 
            // _lblDesc
            // 
            _lblDesc.Font = new Font("Segoe UI", 8.5F);
            _lblDesc.ForeColor = Color.FromArgb(85, 85, 85);
            _lblDesc.Location = new Point(0, 0);
            _lblDesc.Name = "_lblDesc";
            _lblDesc.Size = new Size(100, 23);
            _lblDesc.TabIndex = 6;
            _lblDesc.Click += ToggleExpand_Event;
            // 
            // _lblAuthor
            // 
            _lblAuthor.AutoSize = true;
            _lblAuthor.Font = new Font("Segoe UI", 8F);
            _lblAuthor.ForeColor = Color.FromArgb(110, 110, 110);
            _lblAuthor.Location = new Point(0, 0);
            _lblAuthor.Name = "_lblAuthor";
            _lblAuthor.Size = new Size(0, 13);
            _lblAuthor.TabIndex = 7;
            // 
            // _lblViewed
            // 
            _lblViewed.AutoSize = true;
            _lblViewed.Font = new Font("Segoe UI", 8F);
            _lblViewed.ForeColor = Color.FromArgb(110, 110, 110);
            _lblViewed.Location = new Point(0, 0);
            _lblViewed.Name = "_lblViewed";
            _lblViewed.Size = new Size(0, 13);
            _lblViewed.TabIndex = 8;
            // 
            // _progressTrack
            // 
            _progressTrack.BackColor = Color.FromArgb(225, 225, 225);
            _progressTrack.Controls.Add(_progressFill);
            _progressTrack.Location = new Point(0, 0);
            _progressTrack.Name = "_progressTrack";
            _progressTrack.Size = new Size(200, 4);
            _progressTrack.TabIndex = 9;
            // 
            // _progressFill
            // 
            _progressFill.BackColor = Color.Maroon;
            _progressFill.Location = new Point(0, 0);
            _progressFill.Name = "_progressFill";
            _progressFill.Size = new Size(0, 4);
            _progressFill.TabIndex = 0;
            // 
            // _btnMenu
            // 
            _btnMenu.FlatAppearance.BorderSize = 0;
            _btnMenu.FlatStyle = FlatStyle.Flat;
            _btnMenu.Font = new Font("Segoe UI", 11F);
            _btnMenu.ForeColor = Color.Gray;
            _btnMenu.Location = new Point(0, 0);
            _btnMenu.Name = "_btnMenu";
            _btnMenu.Size = new Size(28, 28);
            _btnMenu.TabIndex = 10;
            _btnMenu.Text = "⋮";
            _btnMenu.UseVisualStyleBackColor = true;
            _btnMenu.Click += BtnMenu_Click;
            // 
            // _ctxMenu
            // 
            _ctxMenu.Name = "_ctxMenu";
            _ctxMenu.Size = new Size(61, 4);
            // 
            // AnnouncementCard
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.White;
            Controls.Add(_btnMenu);
            Controls.Add(_progressTrack);
            Controls.Add(_lblViewed);
            Controls.Add(_lblAuthor);
            Controls.Add(_lblDesc);
            Controls.Add(_lblInactive);
            Controls.Add(_lblCatPill);
            Controls.Add(_lblTitle);
            Controls.Add(_lblPin);
            Controls.Add(_lblNew);
            Controls.Add(_iconCircle);
            DoubleBuffered = true;
            Margin = new Padding(6);
            MinimumSize = new Size(500, 110);
            Name = "AnnouncementCard";
            Size = new Size(900, 110);
            Click += ToggleExpand_Event;
            Paint += Card_Paint;
            Resize += Card_Resize;
            _progressTrack.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel _iconCircle;
        private System.Windows.Forms.Label _lblNew;
        private System.Windows.Forms.Label _lblPin;
        private System.Windows.Forms.Label _lblTitle;
        private System.Windows.Forms.Label _lblCatPill;
        private System.Windows.Forms.Label _lblInactive;
        private System.Windows.Forms.Label _lblDesc;
        private System.Windows.Forms.Label _lblAuthor;
        private System.Windows.Forms.Label _lblViewed;
        private System.Windows.Forms.Panel _progressTrack;
        private System.Windows.Forms.Panel _progressFill;
        private System.Windows.Forms.Button _btnMenu;
        private System.Windows.Forms.ContextMenuStrip _ctxMenu;
    }
}