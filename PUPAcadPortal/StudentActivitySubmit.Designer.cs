namespace PUPAcadPortal
{
    partial class StudentActivitySubmit
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
            pnlHeader = new Panel();
            btnBack = new buttonRounded();
            lblActivityTitle = new Label();
            lblMeta = new Label();
            pnlBody = new Panel();
            pnlInstructions = new Panel();
            lblInstrHeader = new Label();
            lblInstrBody = new Label();
            lblEssayPrompt = new Label();
            txtEssay = new TextBox();
            lblWordCount = new Label();
            btnSubmitEssay = new buttonRounded();
            pnlInstr2 = new Panel();
            lblInstrHeader2 = new Label();
            lblInstrBody2 = new Label();
            lblFilePrompt = new Label();
            pnlDropZone = new Panel();
            lblDropHint = new Label();
            btnBrowse = new buttonRounded();
            lblNotesPrompt = new Label();
            txtNotes = new TextBox();
            btnSubmitAssign = new buttonRounded();
            pnlHeader.SuspendLayout();
            pnlInstructions.SuspendLayout();
            pnlInstr2.SuspendLayout();
            pnlDropZone.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.Maroon;
            pnlHeader.Controls.Add(btnBack);
            pnlHeader.Controls.Add(lblActivityTitle);
            pnlHeader.Controls.Add(lblMeta);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1640, 55);
            pnlHeader.TabIndex = 1;
            // 
            // btnBack
            // 
            btnBack.BackColor = Color.FromArgb(109, 0, 0);
            btnBack.BorderRadius = 10;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBack.ForeColor = Color.White;
            btnBack.Location = new Point(10, 13);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(75, 30);
            btnBack.TabIndex = 0;
            btnBack.Text = "< Back";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += btnBack_Click;
            // 
            // lblActivityTitle
            // 
            lblActivityTitle.AutoSize = true;
            lblActivityTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblActivityTitle.ForeColor = Color.White;
            lblActivityTitle.Location = new Point(100, 10);
            lblActivityTitle.Name = "lblActivityTitle";
            lblActivityTitle.Size = new Size(121, 25);
            lblActivityTitle.TabIndex = 1;
            lblActivityTitle.Text = "Activity Title";
            // 
            // lblMeta
            // 
            lblMeta.AutoSize = true;
            lblMeta.Font = new Font("Segoe UI", 9F);
            lblMeta.ForeColor = Color.FromArgb(220, 180, 180);
            lblMeta.Location = new Point(100, 36);
            lblMeta.Name = "lblMeta";
            lblMeta.Size = new Size(122, 15);
            lblMeta.TabIndex = 2;
            lblMeta.Text = "Type  ·  0 pts  ·  Due —";
            // 
            // pnlBody
            // 
            pnlBody.AutoScroll = true;
            pnlBody.BackColor = Color.FromArgb(245, 245, 245);
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Location = new Point(0, 55);
            pnlBody.Name = "pnlBody";
            pnlBody.Padding = new Padding(20);
            pnlBody.Size = new Size(1640, 934);
            pnlBody.TabIndex = 0;
            // 
            // pnlInstructions
            // 
            pnlInstructions.BackColor = Color.White;
            pnlInstructions.BorderStyle = BorderStyle.FixedSingle;
            pnlInstructions.Controls.Add(lblInstrHeader);
            pnlInstructions.Controls.Add(lblInstrBody);
            pnlInstructions.Location = new Point(20, 20);
            pnlInstructions.Name = "pnlInstructions";
            pnlInstructions.Size = new Size(700, 130);
            pnlInstructions.TabIndex = 0;
            // 
            // lblInstrHeader
            // 
            lblInstrHeader.AutoSize = true;
            lblInstrHeader.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblInstrHeader.ForeColor = Color.Maroon;
            lblInstrHeader.Location = new Point(12, 8);
            lblInstrHeader.Name = "lblInstrHeader";
            lblInstrHeader.Size = new Size(85, 19);
            lblInstrHeader.TabIndex = 0;
            lblInstrHeader.Text = "Instructions";
            // 
            // lblInstrBody
            // 
            lblInstrBody.Font = new Font("Segoe UI", 9F);
            lblInstrBody.ForeColor = Color.FromArgb(60, 60, 60);
            lblInstrBody.Location = new Point(12, 32);
            lblInstrBody.Name = "lblInstrBody";
            lblInstrBody.Size = new Size(670, 88);
            lblInstrBody.TabIndex = 1;
            // 
            // lblEssayPrompt
            // 
            lblEssayPrompt.AutoSize = true;
            lblEssayPrompt.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblEssayPrompt.ForeColor = Color.FromArgb(40, 40, 40);
            lblEssayPrompt.Location = new Point(20, 168);
            lblEssayPrompt.Name = "lblEssayPrompt";
            lblEssayPrompt.Size = new Size(100, 23);
            lblEssayPrompt.TabIndex = 0;
            lblEssayPrompt.Text = "Your Essay Response:";
            // 
            // txtEssay
            // 
            txtEssay.Font = new Font("Segoe UI", 10F);
            txtEssay.Location = new Point(20, 194);
            txtEssay.Multiline = true;
            txtEssay.Name = "txtEssay";
            txtEssay.ScrollBars = ScrollBars.Vertical;
            txtEssay.Size = new Size(700, 260);
            txtEssay.TabIndex = 0;
            txtEssay.TextChanged += txtEssay_TextChanged;
            // 
            // lblWordCount
            // 
            lblWordCount.AutoSize = true;
            lblWordCount.Font = new Font("Segoe UI", 9F);
            lblWordCount.ForeColor = Color.Gray;
            lblWordCount.Location = new Point(20, 460);
            lblWordCount.Name = "lblWordCount";
            lblWordCount.Size = new Size(100, 23);
            lblWordCount.TabIndex = 0;
            lblWordCount.Text = "Words: 0  |  Characters: 0";
            // 
            // btnSubmitEssay
            // 
            btnSubmitEssay.BackColor = Color.Maroon;
            btnSubmitEssay.BorderRadius = 18;
            btnSubmitEssay.FlatAppearance.BorderSize = 0;
            btnSubmitEssay.FlatStyle = FlatStyle.Flat;
            btnSubmitEssay.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSubmitEssay.ForeColor = Color.White;
            btnSubmitEssay.Location = new Point(20, 488);
            btnSubmitEssay.Name = "btnSubmitEssay";
            btnSubmitEssay.Size = new Size(140, 36);
            btnSubmitEssay.TabIndex = 0;
            btnSubmitEssay.Text = "Submit Essay";
            btnSubmitEssay.UseVisualStyleBackColor = false;
            btnSubmitEssay.Click += btnSubmitEssay_Click;
            // 
            // pnlInstr2
            // 
            pnlInstr2.BackColor = Color.White;
            pnlInstr2.BorderStyle = BorderStyle.FixedSingle;
            pnlInstr2.Controls.Add(lblInstrHeader2);
            pnlInstr2.Controls.Add(lblInstrBody2);
            pnlInstr2.Location = new Point(20, 20);
            pnlInstr2.Name = "pnlInstr2";
            pnlInstr2.Size = new Size(700, 130);
            pnlInstr2.TabIndex = 0;
            // 
            // lblInstrHeader2
            // 
            lblInstrHeader2.AutoSize = true;
            lblInstrHeader2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblInstrHeader2.ForeColor = Color.Maroon;
            lblInstrHeader2.Location = new Point(12, 8);
            lblInstrHeader2.Name = "lblInstrHeader2";
            lblInstrHeader2.Size = new Size(85, 19);
            lblInstrHeader2.TabIndex = 0;
            lblInstrHeader2.Text = "Instructions";
            // 
            // lblInstrBody2
            // 
            lblInstrBody2.Font = new Font("Segoe UI", 9F);
            lblInstrBody2.ForeColor = Color.FromArgb(60, 60, 60);
            lblInstrBody2.Location = new Point(12, 32);
            lblInstrBody2.Name = "lblInstrBody2";
            lblInstrBody2.Size = new Size(670, 88);
            lblInstrBody2.TabIndex = 1;
            // 
            // lblFilePrompt
            // 
            lblFilePrompt.AutoSize = true;
            lblFilePrompt.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblFilePrompt.ForeColor = Color.FromArgb(40, 40, 40);
            lblFilePrompt.Location = new Point(20, 168);
            lblFilePrompt.Name = "lblFilePrompt";
            lblFilePrompt.Size = new Size(100, 23);
            lblFilePrompt.TabIndex = 0;
            lblFilePrompt.Text = "Attach File:";
            // 
            // pnlDropZone
            // 
            pnlDropZone.BackColor = Color.FromArgb(240, 240, 255);
            pnlDropZone.BorderStyle = BorderStyle.FixedSingle;
            pnlDropZone.Controls.Add(lblDropHint);
            pnlDropZone.Controls.Add(btnBrowse);
            pnlDropZone.Location = new Point(20, 196);
            pnlDropZone.Name = "pnlDropZone";
            pnlDropZone.Size = new Size(700, 80);
            pnlDropZone.TabIndex = 0;
            // 
            // lblDropHint
            // 
            lblDropHint.Dock = DockStyle.Fill;
            lblDropHint.Font = new Font("Segoe UI", 10F);
            lblDropHint.ForeColor = Color.Gray;
            lblDropHint.Location = new Point(0, 0);
            lblDropHint.Name = "lblDropHint";
            lblDropHint.Size = new Size(698, 78);
            lblDropHint.TabIndex = 0;
            lblDropHint.Text = "Drag & drop files here  —  or  —  click Browse";
            lblDropHint.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnBrowse
            // 
            btnBrowse.BackColor = Color.Maroon;
            btnBrowse.BorderRadius = 10;
            btnBrowse.FlatAppearance.BorderSize = 0;
            btnBrowse.FlatStyle = FlatStyle.Flat;
            btnBrowse.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBrowse.ForeColor = Color.White;
            btnBrowse.Location = new Point(598, 26);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(90, 28);
            btnBrowse.TabIndex = 1;
            btnBrowse.Text = "Browse";
            btnBrowse.UseVisualStyleBackColor = false;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // lblNotesPrompt
            // 
            lblNotesPrompt.AutoSize = true;
            lblNotesPrompt.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblNotesPrompt.ForeColor = Color.FromArgb(40, 40, 40);
            lblNotesPrompt.Location = new Point(20, 294);
            lblNotesPrompt.Name = "lblNotesPrompt";
            lblNotesPrompt.Size = new Size(100, 23);
            lblNotesPrompt.TabIndex = 0;
            lblNotesPrompt.Text = "Your Submission Notes (optional):";
            // 
            // txtNotes
            // 
            txtNotes.Font = new Font("Segoe UI", 10F);
            txtNotes.Location = new Point(20, 322);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.PlaceholderText = "Add a note to your instructor...";
            txtNotes.ScrollBars = ScrollBars.Vertical;
            txtNotes.Size = new Size(700, 100);
            txtNotes.TabIndex = 0;
            // 
            // btnSubmitAssign
            // 
            btnSubmitAssign.BackColor = Color.Maroon;
            btnSubmitAssign.BorderRadius = 18;
            btnSubmitAssign.FlatAppearance.BorderSize = 0;
            btnSubmitAssign.FlatStyle = FlatStyle.Flat;
            btnSubmitAssign.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSubmitAssign.ForeColor = Color.White;
            btnSubmitAssign.Location = new Point(20, 440);
            btnSubmitAssign.Name = "btnSubmitAssign";
            btnSubmitAssign.Size = new Size(160, 36);
            btnSubmitAssign.TabIndex = 0;
            btnSubmitAssign.Text = "Submit Assignment";
            btnSubmitAssign.UseVisualStyleBackColor = false;
            btnSubmitAssign.Click += btnSubmitAssign_Click;
            // 
            // StudentActivitySubmit
            // 
            BackColor = Color.FromArgb(245, 245, 245);
            Controls.Add(pnlBody);
            Controls.Add(pnlHeader);
            Name = "StudentActivitySubmit";
            Size = new Size(1640, 989);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlInstructions.ResumeLayout(false);
            pnlInstructions.PerformLayout();
            pnlInstr2.ResumeLayout(false);
            pnlInstr2.PerformLayout();
            pnlDropZone.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        // ── Control declarations ───────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlHeader;
        private buttonRounded btnBack;
        private System.Windows.Forms.Label lblActivityTitle;
        private System.Windows.Forms.Label lblMeta;
        private System.Windows.Forms.Panel pnlBody;

        // Essay controls
        private System.Windows.Forms.Panel pnlInstructions;
        private System.Windows.Forms.Label lblInstrHeader;
        private System.Windows.Forms.Label lblInstrBody;
        private System.Windows.Forms.Label lblEssayPrompt;
        private System.Windows.Forms.TextBox txtEssay;
        private System.Windows.Forms.Label lblWordCount;
        private buttonRounded btnSubmitEssay;

        // Assignment / FileUpload controls
        private System.Windows.Forms.Panel pnlInstr2;
        private System.Windows.Forms.Label lblInstrHeader2;
        private System.Windows.Forms.Label lblInstrBody2;
        private System.Windows.Forms.Label lblFilePrompt;
        private System.Windows.Forms.Panel pnlDropZone;
        private System.Windows.Forms.Label lblDropHint;
        private buttonRounded btnBrowse;
        private System.Windows.Forms.Label lblNotesPrompt;
        private System.Windows.Forms.TextBox txtNotes;
        private buttonRounded btnSubmitAssign;
    }
}