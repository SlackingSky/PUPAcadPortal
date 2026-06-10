using iTextSharp.text;
using Org.BouncyCastle.Asn1.Crmf;

namespace PUPAcadPortal
{
    partial class StudentClassFilesPage
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
            _pnlHeader = new System.Windows.Forms.Panel();
            btnActivities = new PUPAcadPortal.buttonRounded();
            lblMeta = new System.Windows.Forms.Label();
            lblCourse = new System.Windows.Forms.Label();
            btnBack = new PUPAcadPortal.buttonRounded();
            _pnlScroll = new System.Windows.Forms.Panel();
            _flpModules = new System.Windows.Forms.FlowLayoutPanel();

            _pnlHeader.SuspendLayout();
            _pnlScroll.SuspendLayout();
            SuspendLayout();

            // ── _pnlHeader ─────────────────────────────────────────────────
            _pnlHeader.BackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            _pnlHeader.Controls.Add(btnActivities);
            _pnlHeader.Controls.Add(lblMeta);
            _pnlHeader.Controls.Add(lblCourse);
            _pnlHeader.Controls.Add(btnBack);
            _pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            _pnlHeader.Location = new System.Drawing.Point(0, 0);
            _pnlHeader.Name = "_pnlHeader";
            _pnlHeader.Size = new System.Drawing.Size(1680, 80);
            _pnlHeader.TabIndex = 0;

            // ── btnBack ────────────────────────────────────────────────────
            btnBack.BackColor = System.Drawing.Color.FromArgb(100, 0, 0);
            btnBack.Cursor = System.Windows.Forms.Cursors.Hand;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnBack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnBack.ForeColor = System.Drawing.Color.White;
            btnBack.Location = new System.Drawing.Point(12, 24);
            btnBack.Name = "btnBack";
            btnBack.Size = new System.Drawing.Size(82, 32);
            btnBack.TabIndex = 0;
            btnBack.Text = "← Back";
            btnBack.UseVisualStyleBackColor = false;

            // ── lblCourse ──────────────────────────────────────────────────
            lblCourse.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblCourse.AutoEllipsis = true;
            lblCourse.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            lblCourse.ForeColor = System.Drawing.Color.White;
            lblCourse.Location = new System.Drawing.Point(108, 8);
            lblCourse.Name = "lblCourse";
            lblCourse.Size = new System.Drawing.Size(1380, 28);
            lblCourse.TabIndex = 1;
            lblCourse.Text = "";

            // ── lblMeta ────────────────────────────────────────────────────
            lblMeta.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblMeta.AutoEllipsis = true;
            lblMeta.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            lblMeta.ForeColor = System.Drawing.Color.FromArgb(230, 185, 185);
            lblMeta.Location = new System.Drawing.Point(108, 40);
            lblMeta.Name = "lblMeta";
            lblMeta.Size = new System.Drawing.Size(1380, 18);
            lblMeta.TabIndex = 2;
            lblMeta.Text = "";

            // ── btnActivities ──────────────────────────────────────────────
            btnActivities.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnActivities.BackColor = System.Drawing.Color.FromArgb(255, 196, 0);
            btnActivities.Cursor = System.Windows.Forms.Cursors.Hand;
            btnActivities.FlatAppearance.BorderSize = 0;
            btnActivities.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnActivities.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnActivities.ForeColor = System.Drawing.Color.Black;
            btnActivities.Location = new System.Drawing.Point(1520, 24);
            btnActivities.Name = "btnActivities";
            btnActivities.Size = new System.Drawing.Size(140, 34);
            btnActivities.TabIndex = 4;
            btnActivities.Text = "📋  Activities";
            btnActivities.UseVisualStyleBackColor = false;

            // ── _pnlScroll ─────────────────────────────────────────────────
            _pnlScroll.AutoScroll = true;
            _pnlScroll.BackColor = System.Drawing.Color.FromArgb(245, 245, 248);
            _pnlScroll.Controls.Add(_flpModules);
            _pnlScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            _pnlScroll.Location = new System.Drawing.Point(0, 80);
            _pnlScroll.Name = "_pnlScroll";
            _pnlScroll.Padding = new System.Windows.Forms.Padding(24, 20, 24, 20);
            _pnlScroll.Size = new System.Drawing.Size(1680, 909);
            _pnlScroll.TabIndex = 1;

            // ── _flpModules ────────────────────────────────────────────────
            _flpModules.AutoSize = true;
            _flpModules.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _flpModules.BackColor = System.Drawing.Color.FromArgb(245, 245, 248);
            _flpModules.Dock = System.Windows.Forms.DockStyle.Top;
            _flpModules.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            _flpModules.Location = new System.Drawing.Point(24, 20);
            _flpModules.Name = "_flpModules";
            _flpModules.Size = new System.Drawing.Size(1632, 0);
            _flpModules.TabIndex = 0;
            _flpModules.WrapContents = false;

            // ── StudentClassFilesPage ──────────────────────────────────────
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 245, 248);
            Controls.Add(_pnlScroll);
            Controls.Add(_pnlHeader);
            Name = "StudentClassFilesPage";
            Size = new System.Drawing.Size(1680, 989);

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
        private PUPAcadPortal.buttonRounded btnActivities;
        private System.Windows.Forms.Panel _pnlScroll;
        private System.Windows.Forms.FlowLayoutPanel _flpModules;
    }
}