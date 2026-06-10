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
            pnlHeader = new Panel();
            lblHeaderTitle = new Label();
            body = new Panel();
            lblSubjectLbl = new Label();
            cmbSubject = new ComboBox();
            lblPeriodLbl = new Label();
            cmbPeriod = new ComboBox();
            lblSectionLbl = new Label();
            txtSection = new TextBox();
            lblMaxSlotsLbl = new Label();
            nudMaxSlots = new NumericUpDown();
            lblStatusLbl = new Label();
            cmbStatus = new ComboBox();
            lblError = new Label();
            divider = new Panel();
            btnCancel = new Button();
            btnSave = new Button();
            pnlHeader.SuspendLayout();
            body.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudMaxSlots).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(128, 0, 0);
            pnlHeader.Controls.Add(lblHeaderTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(520, 54);
            pnlHeader.TabIndex = 4;
            // 
            // lblHeaderTitle
            // 
            lblHeaderTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblHeaderTitle.ForeColor = Color.White;
            lblHeaderTitle.Location = new Point(18, 14);
            lblHeaderTitle.Name = "lblHeaderTitle";
            lblHeaderTitle.Size = new Size(460, 26);
            lblHeaderTitle.TabIndex = 0;
            lblHeaderTitle.Text = "Create Course";
            // 
            // body
            // 
            body.BackColor = Color.White;
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
            body.Location = new Point(0, 54);
            body.Name = "body";
            body.Size = new Size(520, 310);
            body.TabIndex = 3;
            // 
            // lblSubjectLbl
            // 
            lblSubjectLbl.AutoSize = true;
            lblSubjectLbl.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSubjectLbl.ForeColor = Color.FromArgb(60, 60, 70);
            lblSubjectLbl.Location = new Point(22, 14);
            lblSubjectLbl.Name = "lblSubjectLbl";
            lblSubjectLbl.Size = new Size(57, 15);
            lblSubjectLbl.TabIndex = 0;
            lblSubjectLbl.Text = "Subject *";
            // 
            // cmbSubject
            // 
            cmbSubject.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSubject.Font = new Font("Segoe UI", 10F);
            cmbSubject.Location = new Point(22, 34);
            cmbSubject.Name = "cmbSubject";
            cmbSubject.Size = new Size(476, 25);
            cmbSubject.TabIndex = 1;
            cmbSubject.SelectedIndexChanged += cmbSubject_SelectedIndexChanged;
            // 
            // lblPeriodLbl
            // 
            lblPeriodLbl.AutoSize = true;
            lblPeriodLbl.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPeriodLbl.ForeColor = Color.FromArgb(60, 60, 70);
            lblPeriodLbl.Location = new Point(22, 14);
            lblPeriodLbl.Name = "lblPeriodLbl";
            lblPeriodLbl.Size = new Size(108, 15);
            lblPeriodLbl.TabIndex = 2;
            lblPeriodLbl.Text = "Academic Period *";
            // 
            // cmbPeriod
            // 
            cmbPeriod.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPeriod.Font = new Font("Segoe UI", 10F);
            cmbPeriod.Location = new Point(22, 34);
            cmbPeriod.Name = "cmbPeriod";
            cmbPeriod.Size = new Size(476, 25);
            cmbPeriod.TabIndex = 3;
            // 
            // lblSectionLbl
            // 
            lblSectionLbl.AutoSize = true;
            lblSectionLbl.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSectionLbl.ForeColor = Color.FromArgb(60, 60, 70);
            lblSectionLbl.Location = new Point(22, 14);
            lblSectionLbl.Name = "lblSectionLbl";
            lblSectionLbl.Size = new Size(57, 15);
            lblSectionLbl.TabIndex = 4;
            lblSectionLbl.Text = "Section *";
            // 
            // txtSection
            // 
            txtSection.Font = new Font("Segoe UI", 10.5F);
            txtSection.Location = new Point(22, 34);
            txtSection.MaxLength = 50;
            txtSection.Name = "txtSection";
            txtSection.PlaceholderText = "e.g. BSIT-2A";
            txtSection.Size = new Size(260, 26);
            txtSection.TabIndex = 5;
            // 
            // lblMaxSlotsLbl
            // 
            lblMaxSlotsLbl.AutoSize = true;
            lblMaxSlotsLbl.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblMaxSlotsLbl.ForeColor = Color.FromArgb(60, 60, 70);
            lblMaxSlotsLbl.Location = new Point(300, 14);
            lblMaxSlotsLbl.Name = "lblMaxSlotsLbl";
            lblMaxSlotsLbl.Size = new Size(61, 15);
            lblMaxSlotsLbl.TabIndex = 6;
            lblMaxSlotsLbl.Text = "Max Slots";
            // 
            // nudMaxSlots
            // 
            nudMaxSlots.Font = new Font("Segoe UI", 10F);
            nudMaxSlots.Location = new Point(300, 34);
            nudMaxSlots.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
            nudMaxSlots.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudMaxSlots.Name = "nudMaxSlots";
            nudMaxSlots.Size = new Size(100, 25);
            nudMaxSlots.TabIndex = 7;
            nudMaxSlots.Value = new decimal(new int[] { 40, 0, 0, 0 });
            // 
            // lblStatusLbl
            // 
            lblStatusLbl.AutoSize = true;
            lblStatusLbl.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblStatusLbl.ForeColor = Color.FromArgb(60, 60, 70);
            lblStatusLbl.Location = new Point(22, 14);
            lblStatusLbl.Name = "lblStatusLbl";
            lblStatusLbl.Size = new Size(42, 15);
            lblStatusLbl.TabIndex = 8;
            lblStatusLbl.Text = "Status";
            // 
            // cmbStatus
            // 
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Font = new Font("Segoe UI", 10F);
            cmbStatus.Items.AddRange(new object[] { "Active", "Ongoing", "Completed", "Archived" });
            cmbStatus.Location = new Point(22, 34);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(200, 25);
            cmbStatus.TabIndex = 9;
            // 
            // lblError
            // 
            lblError.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblError.ForeColor = Color.Red;
            lblError.Location = new Point(22, 260);
            lblError.Name = "lblError";
            lblError.Size = new Size(476, 36);
            lblError.TabIndex = 10;
            // 
            // divider
            // 
            divider.BackColor = Color.FromArgb(218, 218, 225);
            divider.Location = new Point(0, 364);
            divider.Name = "divider";
            divider.Size = new Size(520, 1);
            divider.TabIndex = 2;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.White;
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatAppearance.BorderColor = Color.Silver;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 9.5F);
            btnCancel.ForeColor = Color.FromArgb(100, 100, 110);
            btnCancel.Location = new Point(282, 376);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(108, 36);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(128, 0, 0);
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(398, 376);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(108, 36);
            btnSave.TabIndex = 0;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += BtnSave_Click;
            // 
            // CourseFormDialog
            // 
            AcceptButton = btnSave;
            BackColor = Color.White;
            CancelButton = btnCancel;
            ClientSize = new Size(520, 428);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            Controls.Add(divider);
            Controls.Add(body);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9.5F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CourseFormDialog";
            StartPosition = FormStartPosition.CenterParent;
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