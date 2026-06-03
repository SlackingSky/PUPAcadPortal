namespace PUPAcadPortal
{
    partial class AddEventForm
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
            lblHeaderText = new System.Windows.Forms.Label();
            lblDate = new System.Windows.Forms.Label();
            lblType = new System.Windows.Forms.Label();
            cmbType = new System.Windows.Forms.ComboBox();
            lblTitle = new System.Windows.Forms.Label();
            txtTitle = new System.Windows.Forms.TextBox();
            lblCourse = new System.Windows.Forms.Label();
            txtCourse = new System.Windows.Forms.TextBox();
            lblStartTime = new System.Windows.Forms.Label();
            txtStartTime = new System.Windows.Forms.TextBox();
            lblEndTime = new System.Windows.Forms.Label();
            txtEndTime = new System.Windows.Forms.TextBox();
            lblRoom = new System.Windows.Forms.Label();
            txtRoom = new System.Windows.Forms.TextBox();
            lblDesc = new System.Windows.Forms.Label();
            txtDesc = new System.Windows.Forms.TextBox();
            btnSave = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();

            pnlHeader.SuspendLayout();
            SuspendLayout();

            // ── pnlHeader ──────────────────────────────────────────────────
            pnlHeader.BackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            pnlHeader.Controls.Add(lblHeaderText);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Height = 52;
            pnlHeader.Name = "pnlHeader";
            pnlHeader.TabIndex = 0;

            // ── lblHeaderText ──────────────────────────────────────────────
            lblHeaderText.Dock = System.Windows.Forms.DockStyle.Fill;
            lblHeaderText.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblHeaderText.ForeColor = System.Drawing.Color.White;
            lblHeaderText.Name = "lblHeaderText";
            lblHeaderText.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            lblHeaderText.TabIndex = 0;
            lblHeaderText.Text = "Add Event";
            lblHeaderText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // ── lblDate ────────────────────────────────────────────────────
            lblDate.AutoSize = true;
            lblDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblDate.ForeColor = System.Drawing.Color.FromArgb(128, 0, 0);
            lblDate.Location = new System.Drawing.Point(16, 62);
            lblDate.Name = "lblDate";
            lblDate.TabIndex = 1;
            lblDate.Text = "Date";

            // ── lblType ────────────────────────────────────────────────────
            lblType.AutoSize = true;
            lblType.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblType.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            lblType.Location = new System.Drawing.Point(16, 86);
            lblType.Name = "lblType";
            lblType.TabIndex = 2;
            lblType.Text = "Event Type *";

            // ── cmbType ────────────────────────────────────────────────────
            cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbType.FormattingEnabled = true;
            cmbType.Location = new System.Drawing.Point(16, 108);
            cmbType.Name = "cmbType";
            cmbType.Size = new System.Drawing.Size(220, 23);
            cmbType.TabIndex = 3;

            // ── lblTitle ───────────────────────────────────────────────────
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            lblTitle.Location = new System.Drawing.Point(16, 142);
            lblTitle.Name = "lblTitle";
            lblTitle.TabIndex = 4;
            lblTitle.Text = "Title *";

            // ── txtTitle ───────────────────────────────────────────────────
            txtTitle.Location = new System.Drawing.Point(16, 162);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new System.Drawing.Size(440, 23);
            txtTitle.TabIndex = 5;

            // ── lblCourse ──────────────────────────────────────────────────
            lblCourse.AutoSize = true;
            lblCourse.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblCourse.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            lblCourse.Location = new System.Drawing.Point(16, 196);
            lblCourse.Name = "lblCourse";
            lblCourse.TabIndex = 6;
            lblCourse.Text = "Course";

            // ── txtCourse ──────────────────────────────────────────────────
            txtCourse.Location = new System.Drawing.Point(16, 216);
            txtCourse.Name = "txtCourse";
            txtCourse.Size = new System.Drawing.Size(300, 23);
            txtCourse.TabIndex = 7;

            // ── lblStartTime ───────────────────────────────────────────────
            lblStartTime.AutoSize = true;
            lblStartTime.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblStartTime.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            lblStartTime.Location = new System.Drawing.Point(16, 250);
            lblStartTime.Name = "lblStartTime";
            lblStartTime.TabIndex = 8;
            lblStartTime.Text = "Start Time (HH:mm)";

            // ── txtStartTime ───────────────────────────────────────────────
            txtStartTime.Location = new System.Drawing.Point(16, 270);
            txtStartTime.Name = "txtStartTime";
            txtStartTime.PlaceholderText = "e.g. 08:30";
            txtStartTime.Size = new System.Drawing.Size(120, 23);
            txtStartTime.TabIndex = 9;

            // ── lblEndTime ─────────────────────────────────────────────────
            lblEndTime.AutoSize = true;
            lblEndTime.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblEndTime.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            lblEndTime.Location = new System.Drawing.Point(160, 250);
            lblEndTime.Name = "lblEndTime";
            lblEndTime.TabIndex = 10;
            lblEndTime.Text = "End Time (HH:mm)";

            // ── txtEndTime ─────────────────────────────────────────────────
            txtEndTime.Location = new System.Drawing.Point(160, 270);
            txtEndTime.Name = "txtEndTime";
            txtEndTime.PlaceholderText = "e.g. 10:00";
            txtEndTime.Size = new System.Drawing.Size(120, 23);
            txtEndTime.TabIndex = 11;

            // ── lblRoom ────────────────────────────────────────────────────
            lblRoom.AutoSize = true;
            lblRoom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblRoom.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            lblRoom.Location = new System.Drawing.Point(16, 304);
            lblRoom.Name = "lblRoom";
            lblRoom.TabIndex = 12;
            lblRoom.Text = "Room / Location";

            // ── txtRoom ────────────────────────────────────────────────────
            txtRoom.Location = new System.Drawing.Point(16, 324);
            txtRoom.Name = "txtRoom";
            txtRoom.Size = new System.Drawing.Size(200, 23);
            txtRoom.TabIndex = 13;

            // ── lblDesc ────────────────────────────────────────────────────
            lblDesc.AutoSize = true;
            lblDesc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblDesc.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            lblDesc.Location = new System.Drawing.Point(16, 358);
            lblDesc.Name = "lblDesc";
            lblDesc.TabIndex = 14;
            lblDesc.Text = "Description";

            // ── txtDesc ────────────────────────────────────────────────────
            txtDesc.Location = new System.Drawing.Point(16, 378);
            txtDesc.Multiline = true;
            txtDesc.Name = "txtDesc";
            txtDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtDesc.Size = new System.Drawing.Size(440, 68);
            txtDesc.TabIndex = 15;

            // ── btnSave ────────────────────────────────────────────────────
            btnSave.BackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSave.ForeColor = System.Drawing.Color.White;
            btnSave.Location = new System.Drawing.Point(276, 462);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(180, 34);
            btnSave.TabIndex = 16;
            btnSave.Text = "Save Event";
            btnSave.UseVisualStyleBackColor = false;

            // ── btnCancel ──────────────────────────────────────────────────
            btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCancel.Location = new System.Drawing.Point(16, 462);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(90, 34);
            btnCancel.TabIndex = 17;
            btnCancel.Text = "Cancel";
            btnCancel.Click += (s, e) => { DialogResult = System.Windows.Forms.DialogResult.Cancel; Close(); };

            // ── AddEventForm ───────────────────────────────────────────────
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(474, 514);
            Controls.Add(pnlHeader);
            Controls.Add(lblDate);
            Controls.Add(lblType);
            Controls.Add(cmbType);
            Controls.Add(lblTitle);
            Controls.Add(txtTitle);
            Controls.Add(lblCourse);
            Controls.Add(txtCourse);
            Controls.Add(lblStartTime);
            Controls.Add(txtStartTime);
            Controls.Add(lblEndTime);
            Controls.Add(txtEndTime);
            Controls.Add(lblRoom);
            Controls.Add(txtRoom);
            Controls.Add(lblDesc);
            Controls.Add(txtDesc);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            Font = new System.Drawing.Font("Segoe UI", 9F);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AddEventForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Add Event";

            pnlHeader.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        // ── Designer fields ──────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeaderText;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblCourse;
        private System.Windows.Forms.TextBox txtCourse;
        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.TextBox txtStartTime;
        private System.Windows.Forms.Label lblEndTime;
        private System.Windows.Forms.TextBox txtEndTime;
        private System.Windows.Forms.Label lblRoom;
        private System.Windows.Forms.TextBox txtRoom;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}