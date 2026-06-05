namespace PUPAcadPortal
{
    partial class ClassFilesPage
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
            _pnlHeader = new Panel();
            btnActivities = new buttonRounded();
            btnAddModule = new buttonRounded();
            lblMeta = new Label();
            lblCourse = new Label();
            btnBack = new buttonRounded();
            _pnlScroll = new Panel();
            _flpModules = new FlowLayoutPanel();

            _pnlHeader.SuspendLayout();
            _pnlScroll.SuspendLayout();
            SuspendLayout();

            // ── _pnlHeader ─────────────────────────────────────────────────
            _pnlHeader.BackColor = Color.FromArgb(128, 0, 0);
            _pnlHeader.Controls.Add(btnActivities);
            _pnlHeader.Controls.Add(btnAddModule);
            _pnlHeader.Controls.Add(lblMeta);
            _pnlHeader.Controls.Add(lblCourse);
            _pnlHeader.Controls.Add(btnBack);
            _pnlHeader.Dock = DockStyle.Top;
            _pnlHeader.Location = new Point(0, 0);
            _pnlHeader.Name = "_pnlHeader";
            _pnlHeader.Size = new Size(1680, 80);
            _pnlHeader.TabIndex = 0;

            // ── btnBack ────────────────────────────────────────────────────
            btnBack.BackColor = Color.FromArgb(100, 0, 0);
            btnBack.Cursor = Cursors.Hand;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBack.ForeColor = Color.White;
            btnBack.Location = new Point(12, 24);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(82, 32);
            btnBack.TabIndex = 0;
            btnBack.Text = "← Back";
            btnBack.UseVisualStyleBackColor = false;
            // Click event wired in WireDesignerControls()

            // ── lblCourse ──────────────────────────────────────────────────
            lblCourse.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblCourse.AutoEllipsis = true;
            lblCourse.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblCourse.ForeColor = Color.White;
            lblCourse.Location = new Point(108, 8);
            lblCourse.Name = "lblCourse";
            lblCourse.Size = new Size(1256, 28);
            lblCourse.TabIndex = 1;
            lblCourse.Text = "";   // populated in WireDesignerControls()

            // ── lblMeta ────────────────────────────────────────────────────
            lblMeta.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblMeta.AutoEllipsis = true;
            lblMeta.Font = new Font("Segoe UI", 8.5F);
            lblMeta.ForeColor = Color.FromArgb(230, 185, 185);
            lblMeta.Location = new Point(108, 40);
            lblMeta.Name = "lblMeta";
            lblMeta.Size = new Size(1256, 18);
            lblMeta.TabIndex = 2;
            lblMeta.Text = "";   // populated in WireDesignerControls()

            // ── btnAddModule ───────────────────────────────────────────────
            btnAddModule.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddModule.BackColor = Color.FromArgb(46, 160, 67);
            btnAddModule.Cursor = Cursors.Hand;
            btnAddModule.FlatAppearance.BorderSize = 0;
            btnAddModule.FlatStyle = FlatStyle.Flat;
            btnAddModule.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnAddModule.ForeColor = Color.White;
            btnAddModule.Location = new Point(1374, 24);
            btnAddModule.Name = "btnAddModule";
            btnAddModule.Size = new Size(130, 34);
            btnAddModule.TabIndex = 3;
            btnAddModule.Text = "+ Add Module";
            btnAddModule.UseVisualStyleBackColor = false;
            // Click event wired in WireDesignerControls()

            // ── btnActivities ──────────────────────────────────────────────
            btnActivities.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnActivities.BackColor = Color.FromArgb(255, 196, 0);
            btnActivities.Cursor = Cursors.Hand;
            btnActivities.FlatAppearance.BorderSize = 0;
            btnActivities.FlatStyle = FlatStyle.Flat;
            btnActivities.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnActivities.ForeColor = Color.Black;
            btnActivities.Location = new Point(1516, 24);
            btnActivities.Name = "btnActivities";
            btnActivities.Size = new Size(140, 34);
            btnActivities.TabIndex = 4;
            btnActivities.Text = "📋  Activities";
            btnActivities.UseVisualStyleBackColor = false;
            // Click event wired in WireDesignerControls()

            // ── _pnlScroll ─────────────────────────────────────────────────
            _pnlScroll.AutoScroll = true;
            _pnlScroll.BackColor = Color.FromArgb(245, 245, 248);
            _pnlScroll.Controls.Add(_flpModules);
            _pnlScroll.Dock = DockStyle.Fill;
            _pnlScroll.Location = new Point(0, 80);
            _pnlScroll.Name = "_pnlScroll";
            _pnlScroll.Padding = new Padding(24, 20, 24, 20);
            _pnlScroll.Size = new Size(1680, 909);
            _pnlScroll.TabIndex = 1;

            //  _flpModules 
            _flpModules.AutoSize = true;
            _flpModules.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpModules.BackColor = Color.FromArgb(245, 245, 248);
            _flpModules.Dock = DockStyle.Top;
            _flpModules.FlowDirection = FlowDirection.TopDown;
            _flpModules.Location = new Point(24, 20);
            _flpModules.Name = "_flpModules";
            _flpModules.Size = new Size(1632, 0);
            _flpModules.TabIndex = 0;
            _flpModules.WrapContents = false;

            //  ClassFilesPage 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 245, 248);
            Controls.Add(_pnlScroll);
            Controls.Add(_pnlHeader);
            Name = "ClassFilesPage";
            Size = new Size(1680, 989);

            _pnlHeader.ResumeLayout(false);
            _pnlScroll.ResumeLayout(false);
            _pnlScroll.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel _pnlHeader;
        private PUPAcadPortal.buttonRounded btnBack;
        private System.Windows.Forms.Label lblCourse;
        private System.Windows.Forms.Label lblMeta;
        private PUPAcadPortal.buttonRounded btnAddModule;
        private PUPAcadPortal.buttonRounded btnActivities;
        private System.Windows.Forms.Panel _pnlScroll;
        private System.Windows.Forms.FlowLayoutPanel _flpModules;
    }
}