namespace PUPAcadPortal
{
    partial class CourseFormDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlHeader = new System.Windows.Forms.Panel();
            lblHeaderTitle = new System.Windows.Forms.Label();
            body = new System.Windows.Forms.Panel();
            lblSubjectLbl = new System.Windows.Forms.Label();
            cmbSubject = new System.Windows.Forms.ComboBox();
            lblPeriodLbl = new System.Windows.Forms.Label();
            cmbPeriod = new System.Windows.Forms.ComboBox();
            lblSectionLbl = new System.Windows.Forms.Label();
            txtSection = new System.Windows.Forms.TextBox();
            lblMaxSlotsLbl = new System.Windows.Forms.Label();
            nudMaxSlots = new System.Windows.Forms.NumericUpDown();
            lblStatusLbl = new System.Windows.Forms.Label();
            cmbStatus = new System.Windows.Forms.ComboBox();
            lblError = new System.Windows.Forms.Label();
            divider = new System.Windows.Forms.Panel();
            btnCancel = new System.Windows.Forms.Button();
            btnSave = new System.Windows.Forms.Button();

            pnlHeader.SuspendLayout();
            body.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudMaxSlots).BeginInit();
            SuspendLayout();

            // ── pnlHeader ──────────────────────────────────────────────────
            pnlHeader.BackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            pnlHeader.Controls.Add(lblHeaderTitle);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Size = new System.Drawing.Size(520, 54);
            pnlHeader.Name = "pnlHeader";

            lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            lblHeaderTitle.ForeColor = System.Drawing.Color.White;
            lblHeaderTitle.Location = new System.Drawing.Point(18, 14);
            lblHeaderTitle.Size = new System.Drawing.Size(460, 26);
            lblHeaderTitle.Name = "lblHeaderTitle";
            lblHeaderTitle.Text = "Create Course";

            // ── body ───────────────────────────────────────────────────────
            body.BackColor = System.Drawing.Color.White;
            body.Controls.Add(lblSubjectLbl);
            body.Controls.Add(cmbSubject);
            body.Controls.Add(lblPeriodLbl);
            body.Controls.Add(cmbPeriod);
            body.Controls.Add(lblSectionLbl);
            body.Controls.Add(txtSection);
            body.Controls.Add(lblMaxSlotsLbl);
            body.Controls.Add(nudMaxSlots);
            body.Controls.Add(lblStatusLbl);
            body.Controls.Add(cmbStatus);
            body.Controls.Add(lblError);
            body.Location = new System.Drawing.Point(0, 54);
            body.Size = new System.Drawing.Size(520, 310);
            body.Name = "body";

            int labelX = 22, fieldX = 22, rowH = 50;

            // Subject
            lblSubjectLbl.Text = "Subject *";
            lblSubjectLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblSubjectLbl.ForeColor = System.Drawing.Color.FromArgb(60, 60, 70);
            lblSubjectLbl.Location = new System.Drawing.Point(labelX, 14);
            lblSubjectLbl.AutoSize = true;
            lblSubjectLbl.Name = "lblSubjectLbl";

            cmbSubject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbSubject.Font = new System.Drawing.Font("Segoe UI", 10F);
            cmbSubject.Location = new System.Drawing.Point(fieldX, 34);
            cmbSubject.Size = new System.Drawing.Size(476, 26);
            cmbSubject.Name = "cmbSubject";

            // Period
            lblPeriodLbl.Text = "Academic Period *";
            lblPeriodLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblPeriodLbl.ForeColor = System.Drawing.Color.FromArgb(60, 60, 70);
            lblPeriodLbl.Location = new System.Drawing.Point(labelX, 14 + rowH);
            lblPeriodLbl.AutoSize = true;
            lblPeriodLbl.Name = "lblPeriodLbl";

            cmbPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbPeriod.Font = new System.Drawing.Font("Segoe UI", 10F);
            cmbPeriod.Location = new System.Drawing.Point(fieldX, 34 + rowH);
            cmbPeriod.Size = new System.Drawing.Size(476, 26);
            cmbPeriod.Name = "cmbPeriod";

            // Section + MaxSlots on same row
            lblSectionLbl.Text = "Section *";
            lblSectionLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblSectionLbl.ForeColor = System.Drawing.Color.FromArgb(60, 60, 70);
            lblSectionLbl.Location = new System.Drawing.Point(labelX, 14 + rowH * 2);
            lblSectionLbl.AutoSize = true;
            lblSectionLbl.Name = "lblSectionLbl";

            txtSection.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            txtSection.Location = new System.Drawing.Point(fieldX, 34 + rowH * 2);
            txtSection.Size = new System.Drawing.Size(260, 27);
            txtSection.MaxLength = 50;
            txtSection.PlaceholderText = "e.g. BSIT-2A";
            txtSection.Name = "txtSection";

            lblMaxSlotsLbl.Text = "Max Slots";
            lblMaxSlotsLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblMaxSlotsLbl.ForeColor = System.Drawing.Color.FromArgb(60, 60, 70);
            lblMaxSlotsLbl.Location = new System.Drawing.Point(300, 14 + rowH * 2);
            lblMaxSlotsLbl.AutoSize = true;
            lblMaxSlotsLbl.Name = "lblMaxSlotsLbl";

            nudMaxSlots.Font = new System.Drawing.Font("Segoe UI", 10F);
            nudMaxSlots.Location = new System.Drawing.Point(300, 34 + rowH * 2);
            nudMaxSlots.Size = new System.Drawing.Size(100, 27);
            nudMaxSlots.Minimum = 1;
            nudMaxSlots.Maximum = 500;
            nudMaxSlots.Value = 40;
            nudMaxSlots.Name = "nudMaxSlots";

            // Status
            lblStatusLbl.Text = "Status";
            lblStatusLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblStatusLbl.ForeColor = System.Drawing.Color.FromArgb(60, 60, 70);
            lblStatusLbl.Location = new System.Drawing.Point(labelX, 14 + rowH * 3);
            lblStatusLbl.AutoSize = true;
            lblStatusLbl.Name = "lblStatusLbl";

            cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            cmbStatus.Items.AddRange(new object[] { "Active", "Ongoing", "Completed", "Archived" });
            cmbStatus.Location = new System.Drawing.Point(fieldX, 34 + rowH * 3);
            cmbStatus.Size = new System.Drawing.Size(200, 26);
            cmbStatus.SelectedIndex = 0;
            cmbStatus.Name = "cmbStatus";

            // Error label
            lblError.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            lblError.ForeColor = System.Drawing.Color.Red;
            lblError.Location = new System.Drawing.Point(fieldX, 260);
            lblError.Size = new System.Drawing.Size(476, 36);
            lblError.Name = "lblError";
            lblError.Text = "";

            // ── Divider ────────────────────────────────────────────────────
            divider.BackColor = System.Drawing.Color.FromArgb(218, 218, 225);
            divider.Location = new System.Drawing.Point(0, 364);
            divider.Size = new System.Drawing.Size(520, 1);
            divider.Name = "divider";

            // ── Buttons ────────────────────────────────────────────────────
            btnCancel.BackColor = System.Drawing.Color.White;
            btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCancel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            btnCancel.ForeColor = System.Drawing.Color.FromArgb(100, 100, 110);
            btnCancel.Location = new System.Drawing.Point(282, 376);
            btnCancel.Size = new System.Drawing.Size(108, 36);
            btnCancel.Name = "btnCancel";
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;

            btnSave.BackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnSave.ForeColor = System.Drawing.Color.White;
            btnSave.Location = new System.Drawing.Point(398, 376);
            btnSave.Size = new System.Drawing.Size(108, 36);
            btnSave.Name = "btnSave";
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += BtnSave_Click;

            // ── Form ───────────────────────────────────────────────────────
            AcceptButton = btnSave;
            BackColor = System.Drawing.Color.White;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(520, 428);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            Controls.Add(divider);
            Controls.Add(body);
            Controls.Add(pnlHeader);
            Font = new System.Drawing.Font("Segoe UI", 9.5F);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CourseFormDialog";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Course";

            pnlHeader.ResumeLayout(false);
            body.ResumeLayout(false);
            body.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudMaxSlots).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Panel body;
        private System.Windows.Forms.Label lblSubjectLbl;
        private System.Windows.Forms.ComboBox cmbSubject;
        private System.Windows.Forms.Label lblPeriodLbl;
        private System.Windows.Forms.ComboBox cmbPeriod;
        private System.Windows.Forms.Label lblSectionLbl;
        private System.Windows.Forms.TextBox txtSection;
        private System.Windows.Forms.Label lblMaxSlotsLbl;
        private System.Windows.Forms.NumericUpDown nudMaxSlots;
        private System.Windows.Forms.Label lblStatusLbl;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Panel divider;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
    }
}