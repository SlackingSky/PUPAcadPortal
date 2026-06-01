using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalForms
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();

            // Row labels
            this.lblType = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblCourse = new System.Windows.Forms.Label();
            this.lblStart = new System.Windows.Forms.Label();
            this.lblEndSep = new System.Windows.Forms.Label();
            // lblEnd removed – End label text is handled by lblEndSep
            this.lblRoom = new System.Windows.Forms.Label();
            this.lblNotes = new System.Windows.Forms.Label();

            // Input controls
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.txtCourse = new System.Windows.Forms.TextBox();
            this.txtStartTime = new System.Windows.Forms.TextBox();
            this.txtEndTime = new System.Windows.Forms.TextBox();
            this.txtRoom = new System.Windows.Forms.TextBox();
            this.txtDesc = new System.Windows.Forms.RichTextBox();

            // Buttons
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();

            // ── pnlHeader ────────────────────────────────────────
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 52;
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.TabIndex = 0;

            // ── lblHeader ────────────────────────────────────────
            this.lblHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Add New Event";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // ── lblDate ──────────────────────────────────────────
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblDate.ForeColor = System.Drawing.Color.Gray;
            this.lblDate.Location = new System.Drawing.Point(14, 58);
            this.lblDate.Name = "lblDate";
            this.lblDate.TabIndex = 1;
            this.lblDate.Text = "";

            // ── Shared label style helper (applied inline below) ──
            // LX=14  LW=96  FX=116  FW=314
            const int LX = 14, LW = 96, FX = 116, FW = 314;

            // ── lblType ──────────────────────────────────────────
            this.lblType.AutoSize = false;
            this.lblType.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblType.ForeColor = System.Drawing.Color.FromArgb(80, 80, 90);
            this.lblType.Location = new System.Drawing.Point(LX, 84);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(LW, 24);
            this.lblType.TabIndex = 2;
            this.lblType.Text = "Type:";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // ── cmbType ──────────────────────────────────────────
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbType.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbType.Location = new System.Drawing.Point(FX, 82);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(FW, 24);
            this.cmbType.TabIndex = 3;

            // ── lblTitle ─────────────────────────────────────────
            this.lblTitle.AutoSize = false;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(80, 80, 90);
            this.lblTitle.Location = new System.Drawing.Point(LX, 122);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(LW, 24);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "Title:";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // ── txtTitle ─────────────────────────────────────────
            this.txtTitle.BackColor = System.Drawing.Color.FromArgb(250, 250, 252);
            this.txtTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTitle.Location = new System.Drawing.Point(FX, 120);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(FW, 24);
            this.txtTitle.TabIndex = 5;

            // ── lblCourse ────────────────────────────────────────
            this.lblCourse.AutoSize = false;
            this.lblCourse.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblCourse.ForeColor = System.Drawing.Color.FromArgb(80, 80, 90);
            this.lblCourse.Location = new System.Drawing.Point(LX, 160);
            this.lblCourse.Name = "lblCourse";
            this.lblCourse.Size = new System.Drawing.Size(LW, 24);
            this.lblCourse.TabIndex = 6;
            this.lblCourse.Text = "Course:";
            this.lblCourse.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // ── txtCourse ────────────────────────────────────────
            this.txtCourse.BackColor = System.Drawing.Color.FromArgb(250, 250, 252);
            this.txtCourse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCourse.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtCourse.Location = new System.Drawing.Point(FX, 158);
            this.txtCourse.Name = "txtCourse";
            this.txtCourse.PlaceholderText = "e.g. CS101 – Data Structures";
            this.txtCourse.Size = new System.Drawing.Size(FW, 24);
            this.txtCourse.TabIndex = 7;

            // ── lblStart ─────────────────────────────────────────
            this.lblStart.AutoSize = false;
            this.lblStart.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblStart.ForeColor = System.Drawing.Color.FromArgb(80, 80, 90);
            this.lblStart.Location = new System.Drawing.Point(LX, 198);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(LW, 24);
            this.lblStart.TabIndex = 8;
            this.lblStart.Text = "Start:";
            this.lblStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // ── txtStartTime ─────────────────────────────────────
            this.txtStartTime.BackColor = System.Drawing.Color.FromArgb(250, 250, 252);
            this.txtStartTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStartTime.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtStartTime.Location = new System.Drawing.Point(FX, 196);
            this.txtStartTime.Name = "txtStartTime";
            this.txtStartTime.PlaceholderText = "HH:mm";
            this.txtStartTime.Size = new System.Drawing.Size(90, 24);
            this.txtStartTime.TabIndex = 9;

            // ── lblEndSep ────────────────────────────────────────
            this.lblEndSep.AutoSize = true;
            this.lblEndSep.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblEndSep.ForeColor = System.Drawing.Color.FromArgb(80, 80, 90);
            this.lblEndSep.Location = new System.Drawing.Point(FX + 96, 200);
            this.lblEndSep.Name = "lblEndSep";
            this.lblEndSep.TabIndex = 10;
            this.lblEndSep.Text = "End:";

            // ── txtEndTime ───────────────────────────────────────
            this.txtEndTime.BackColor = System.Drawing.Color.FromArgb(250, 250, 252);
            this.txtEndTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEndTime.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtEndTime.Location = new System.Drawing.Point(FX + 136, 196);
            this.txtEndTime.Name = "txtEndTime";
            this.txtEndTime.PlaceholderText = "HH:mm";
            this.txtEndTime.Size = new System.Drawing.Size(90, 24);
            this.txtEndTime.TabIndex = 11;

            // ── lblRoom ──────────────────────────────────────────
            this.lblRoom.AutoSize = false;
            this.lblRoom.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblRoom.ForeColor = System.Drawing.Color.FromArgb(80, 80, 90);
            this.lblRoom.Location = new System.Drawing.Point(LX, 236);
            this.lblRoom.Name = "lblRoom";
            this.lblRoom.Size = new System.Drawing.Size(LW, 24);
            this.lblRoom.TabIndex = 12;
            this.lblRoom.Text = "Room:";
            this.lblRoom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // ── txtRoom ──────────────────────────────────────────
            this.txtRoom.BackColor = System.Drawing.Color.FromArgb(250, 250, 252);
            this.txtRoom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRoom.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtRoom.Location = new System.Drawing.Point(FX, 234);
            this.txtRoom.Name = "txtRoom";
            this.txtRoom.PlaceholderText = "e.g. N-302";
            this.txtRoom.Size = new System.Drawing.Size(140, 24);
            this.txtRoom.TabIndex = 13;

            // ── lblNotes ─────────────────────────────────────────
            this.lblNotes.AutoSize = false;
            this.lblNotes.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblNotes.ForeColor = System.Drawing.Color.FromArgb(80, 80, 90);
            this.lblNotes.Location = new System.Drawing.Point(LX, 274);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(LW, 24);
            this.lblNotes.TabIndex = 14;
            this.lblNotes.Text = "Notes:";
            this.lblNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;

            // ── txtDesc ──────────────────────────────────────────
            this.txtDesc.BackColor = System.Drawing.Color.FromArgb(250, 250, 252);
            this.txtDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDesc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtDesc.Location = new System.Drawing.Point(FX, 272);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtDesc.Size = new System.Drawing.Size(FW, 78);
            this.txtDesc.TabIndex = 15;

            // ── btnSave ──────────────────────────────────────────
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(128, 0, 0);
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(234, 364);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 32);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save Event";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.FlatAppearance.BorderSize = 0;

            // ── btnCancel ────────────────────────────────────────
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(240, 240, 245);
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(80, 80, 92);
            this.btnCancel.Location = new System.Drawing.Point(342, 364);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 32);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(210, 210, 218);

            // ── Form ─────────────────────────────────────────────
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(446, 410);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddEventForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Event";

            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblCourse);
            this.Controls.Add(this.txtCourse);
            this.Controls.Add(this.lblStart);
            this.Controls.Add(this.txtStartTime);
            this.Controls.Add(this.lblEndSep);
            this.Controls.Add(this.txtEndTime);
            this.Controls.Add(this.lblRoom);
            this.Controls.Add(this.txtRoom);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.txtDesc);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);

            this.pnlHeader.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        // ── Designer field declarations ───────────────────────────
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblCourse;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.Label lblEndSep;

        private System.Windows.Forms.Label lblRoom;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.TextBox txtCourse;
        private System.Windows.Forms.TextBox txtStartTime;
        private System.Windows.Forms.TextBox txtEndTime;
        private System.Windows.Forms.TextBox txtRoom;
        private System.Windows.Forms.RichTextBox txtDesc;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}