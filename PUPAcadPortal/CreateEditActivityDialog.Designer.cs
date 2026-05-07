using System.Reflection.PortableExecutable;

namespace PUPAcadPortal
{
    partial class CreateEditActivityDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlHeader = new System.Windows.Forms.Panel();
            lblHeader = new System.Windows.Forms.Label();
            lblTitleLabel = new System.Windows.Forms.Label();
            txtTitle = new System.Windows.Forms.TextBox();
            lblTypeLabel = new System.Windows.Forms.Label();
            cmbType = new System.Windows.Forms.ComboBox();
            lblPointsLabel = new System.Windows.Forms.Label();
            nudPoints = new System.Windows.Forms.NumericUpDown();
            lblDeadlineLabel = new System.Windows.Forms.Label();
            dtpDeadline = new System.Windows.Forms.DateTimePicker();
            lblDescLabel = new System.Windows.Forms.Label();
            txtDescription = new System.Windows.Forms.TextBox();
            lblError = new System.Windows.Forms.Label();
            btnOk = new buttonRounded();
            btnCancel = new buttonRounded();

            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPoints).BeginInit();
            SuspendLayout();

            // ── Form ───────────────────────────────────────────────────────────────
            this.Text = "Activity";
            this.ClientSize = new System.Drawing.Size(520, 430);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = System.Drawing.Color.White;
            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
            this.Name = "CreateEditActivityDialog";

            // ── pnlHeader ──────────────────────────────────────────────────────────
            pnlHeader.BackColor = System.Drawing.Color.Maroon;
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Height = 52;
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Controls.Add(lblHeader);

            // lblHeader
            lblHeader.Text = "Create New Activity";
            lblHeader.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            lblHeader.ForeColor = System.Drawing.Color.White;
            lblHeader.Location = new System.Drawing.Point(15, 14);
            lblHeader.Size = new System.Drawing.Size(460, 26);
            lblHeader.Name = "lblHeader";

            // ── Title ──────────────────────────────────────────────────────────────
            // lblTitleLabel
            lblTitleLabel.Text = "Activity Title *";
            lblTitleLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblTitleLabel.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            lblTitleLabel.Location = new System.Drawing.Point(15, 68);
            lblTitleLabel.Size = new System.Drawing.Size(200, 18);
            lblTitleLabel.Name = "lblTitleLabel";

            // txtTitle
            txtTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtTitle.Location = new System.Drawing.Point(15, 88);
            txtTitle.Size = new System.Drawing.Size(478, 26);
            txtTitle.PlaceholderText = "Enter activity title...";
            txtTitle.Name = "txtTitle";

            // ── Type + Points (same row) ───────────────────────────────────────────
            // lblTypeLabel
            lblTypeLabel.Text = "Activity Type *";
            lblTypeLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblTypeLabel.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            lblTypeLabel.Location = new System.Drawing.Point(15, 126);
            lblTypeLabel.Size = new System.Drawing.Size(130, 18);
            lblTypeLabel.Name = "lblTypeLabel";

            // cmbType
            cmbType.Font = new System.Drawing.Font("Segoe UI", 10F);
            cmbType.Location = new System.Drawing.Point(15, 146);
            cmbType.Size = new System.Drawing.Size(200, 26);
            cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbType.Items.AddRange(new object[] { "Assignment", "Quiz", "Essay", "FileUpload" });
            cmbType.SelectedIndex = 0;
            cmbType.Name = "cmbType";

            // lblPointsLabel
            lblPointsLabel.Text = "Max Points *";
            lblPointsLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblPointsLabel.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            lblPointsLabel.Location = new System.Drawing.Point(240, 126);
            lblPointsLabel.Size = new System.Drawing.Size(130, 18);
            lblPointsLabel.Name = "lblPointsLabel";

            // nudPoints
            nudPoints.Font = new System.Drawing.Font("Segoe UI", 10F);
            nudPoints.Location = new System.Drawing.Point(240, 146);
            nudPoints.Size = new System.Drawing.Size(100, 26);
            nudPoints.Minimum = 1;
            nudPoints.Maximum = 1000;
            nudPoints.Value = 100;
            nudPoints.Name = "nudPoints";

            // ── Deadline ───────────────────────────────────────────────────────────
            // lblDeadlineLabel
            lblDeadlineLabel.Text = "Deadline *";
            lblDeadlineLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblDeadlineLabel.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            lblDeadlineLabel.Location = new System.Drawing.Point(15, 186);
            lblDeadlineLabel.Size = new System.Drawing.Size(130, 18);
            lblDeadlineLabel.Name = "lblDeadlineLabel";

            // dtpDeadline
            dtpDeadline.Font = new System.Drawing.Font("Segoe UI", 10F);
            dtpDeadline.Location = new System.Drawing.Point(15, 206);
            dtpDeadline.Size = new System.Drawing.Size(360, 26);
            dtpDeadline.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dtpDeadline.CustomFormat = "MM/dd/yyyy hh:mm tt";
            dtpDeadline.Value = System.DateTime.Now.AddDays(7);
            dtpDeadline.Name = "dtpDeadline";

            // ── Description ────────────────────────────────────────────────────────
            // lblDescLabel
            lblDescLabel.Text = "Description (optional)";
            lblDescLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblDescLabel.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            lblDescLabel.Location = new System.Drawing.Point(15, 246);
            lblDescLabel.Size = new System.Drawing.Size(200, 18);
            lblDescLabel.Name = "lblDescLabel";

            // txtDescription
            txtDescription.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtDescription.Location = new System.Drawing.Point(15, 266);
            txtDescription.Size = new System.Drawing.Size(478, 70);
            txtDescription.Multiline = true;
            txtDescription.PlaceholderText = "Add instructions or description...";
            txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtDescription.Name = "txtDescription";

            // ── Error + Buttons ────────────────────────────────────────────────────
            // lblError
            lblError.Text = "";
            lblError.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblError.ForeColor = System.Drawing.Color.Red;
            lblError.Location = new System.Drawing.Point(15, 348);
            lblError.Size = new System.Drawing.Size(350, 18);
            lblError.Name = "lblError";

            // btnCancel
            btnCancel.Text = "Cancel";
            btnCancel.Location = new System.Drawing.Point(163, 375);
            btnCancel.Size = new System.Drawing.Size(120, 32);
            btnCancel.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
            btnCancel.ForeColor = System.Drawing.Color.White;
            btnCancel.BorderRadius = 10;
            btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Name = "btnCancel";
            btnCancel.Click += btnCancel_Click;

            // btnOk
            btnOk.Text = "Create";
            btnOk.Location = new System.Drawing.Point(293, 375);
            btnOk.Size = new System.Drawing.Size(150, 32);
            btnOk.BackColor = System.Drawing.Color.Maroon;
            btnOk.ForeColor = System.Drawing.Color.White;
            btnOk.BorderRadius = 10;
            btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnOk.FlatAppearance.BorderSize = 0;
            btnOk.Name = "btnOk";
            btnOk.Click += btnOk_Click;

            // ── Add controls to form ───────────────────────────────────────────────
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                pnlHeader,
                lblTitleLabel, txtTitle,
                lblTypeLabel, cmbType,
                lblPointsLabel, nudPoints,
                lblDeadlineLabel, dtpDeadline,
                lblDescLabel, txtDescription,
                lblError, btnCancel, btnOk
            });

            pnlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nudPoints).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        // ── Control declarations ───────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblTitleLabel;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblTypeLabel;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label lblPointsLabel;
        private System.Windows.Forms.NumericUpDown nudPoints;
        private System.Windows.Forms.Label lblDeadlineLabel;
        private System.Windows.Forms.DateTimePicker dtpDeadline;
        private System.Windows.Forms.Label lblDescLabel;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblError;
        private buttonRounded btnOk;
        private buttonRounded btnCancel;
    }
}