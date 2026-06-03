namespace PUPAcadPortal.PortalContents.Student.LMS.Attendance
{
    partial class QRConfirmDialog
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
            this.titleBar = new System.Windows.Forms.Panel();
            this.btnX = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.iconStrip = new System.Windows.Forms.Panel();
            this.lblSub = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.iconCircle = new System.Windows.Forms.Panel();
            this.body = new System.Windows.Forms.Panel();
            this.grid = new System.Windows.Forms.TableLayoutPanel();
            this.footer = new System.Windows.Forms.Panel();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            this.titleBar.SuspendLayout();
            this.iconStrip.SuspendLayout();
            this.body.SuspendLayout();
            this.footer.SuspendLayout();
            this.SuspendLayout();

            // 
            // titleBar
            // 
            this.titleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.titleBar.Controls.Add(this.btnX);
            this.titleBar.Controls.Add(this.lblTitle);
            this.titleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBar.Location = new System.Drawing.Point(0, 0);
            this.titleBar.Name = "titleBar";
            this.titleBar.Size = new System.Drawing.Size(460, 50);
            this.titleBar.TabIndex = 0;
            this.titleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseDown);
            this.titleBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseMove);
            this.titleBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseUp);
            // 
            // btnX
            // 
            this.btnX.BackColor = System.Drawing.Color.Transparent;
            this.btnX.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnX.FlatAppearance.BorderSize = 0;
            this.btnX.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnX.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnX.ForeColor = System.Drawing.Color.White;
            this.btnX.Location = new System.Drawing.Point(410, 5);
            this.btnX.Name = "btnX";
            this.btnX.Size = new System.Drawing.Size(40, 40);
            this.btnX.TabIndex = 1;
            this.btnX.Text = "✕";
            this.btnX.UseVisualStyleBackColor = false;
            this.btnX.Click += new System.EventHandler(this.BtnX_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(18, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(360, 50);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Confirm Attendance";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // iconStrip
            // 
            this.iconStrip.Controls.Add(this.lblSub);
            this.iconStrip.Controls.Add(this.lblStatus);
            this.iconStrip.Controls.Add(this.iconCircle);
            this.iconStrip.Dock = System.Windows.Forms.DockStyle.Top;
            this.iconStrip.Location = new System.Drawing.Point(0, 50);
            this.iconStrip.Name = "iconStrip";
            this.iconStrip.Padding = new System.Windows.Forms.Padding(18, 0, 18, 0);
            this.iconStrip.Size = new System.Drawing.Size(460, 72);
            this.iconStrip.TabIndex = 1;
            this.iconStrip.Paint += new System.Windows.Forms.PaintEventHandler(this.IconStrip_Paint);
            // 
            // lblSub
            // 
            this.lblSub.AutoSize = true;
            this.lblSub.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblSub.Location = new System.Drawing.Point(78, 38);
            this.lblSub.Name = "lblSub";
            this.lblSub.Size = new System.Drawing.Size(0, 15);
            this.lblSub.TabIndex = 2;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(78, 14);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 19);
            this.lblStatus.TabIndex = 1;
            // 
            // iconCircle
            // 
            this.iconCircle.BackColor = System.Drawing.Color.Transparent;
            this.iconCircle.Location = new System.Drawing.Point(18, 12);
            this.iconCircle.Name = "iconCircle";
            this.iconCircle.Size = new System.Drawing.Size(48, 48);
            this.iconCircle.TabIndex = 0;
            this.iconCircle.Paint += new System.Windows.Forms.PaintEventHandler(this.IconCircle_Paint);
            // 
            // body
            // 
            this.body.Controls.Add(this.grid);
            this.body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.body.Location = new System.Drawing.Point(0, 122);
            this.body.Name = "body";
            this.body.Padding = new System.Windows.Forms.Padding(22, 12, 22, 12);
            this.body.Size = new System.Drawing.Size(460, 160);
            this.body.TabIndex = 2;
            // 
            // grid
            // 
            this.grid.AutoSize = true;
            this.grid.BackColor = System.Drawing.Color.Transparent;
            this.grid.ColumnCount = 2;
            this.grid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.grid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.grid.Dock = System.Windows.Forms.DockStyle.Top;
            this.grid.Location = new System.Drawing.Point(22, 12);
            this.grid.Name = "grid";
            this.grid.RowCount = 1;
            this.grid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.grid.Size = new System.Drawing.Size(416, 32);
            this.grid.TabIndex = 0;
            // 
            // footer
            // 
            this.footer.BackColor = System.Drawing.Color.White;
            this.footer.Controls.Add(this.btnConfirm);
            this.footer.Controls.Add(this.btnCancel);
            this.footer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footer.Location = new System.Drawing.Point(0, 282);
            this.footer.Name = "footer";
            this.footer.Size = new System.Drawing.Size(460, 58);
            this.footer.TabIndex = 3;
            this.footer.Paint += new System.Windows.Forms.PaintEventHandler(this.Footer_Paint);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfirm.FlatAppearance.BorderSize = 0;
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.Location = new System.Drawing.Point(302, 12);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(140, 34);
            this.btnConfirm.TabIndex = 1;
            this.btnConfirm.Text = "Mark Attendance";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(215)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.btnCancel.Location = new System.Drawing.Point(192, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 34);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // QRConfirmDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(460, 340);
            this.Controls.Add(this.body);
            this.Controls.Add(this.footer);
            this.Controls.Add(this.iconStrip);
            this.Controls.Add(this.titleBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(460, 310);
            this.Name = "QRConfirmDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "QRConfirmDialog";
            this.titleBar.ResumeLayout(false);
            this.iconStrip.ResumeLayout(false);
            this.iconStrip.PerformLayout();
            this.body.ResumeLayout(false);
            this.body.PerformLayout();
            this.footer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel titleBar;
        private System.Windows.Forms.Button btnX;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel iconStrip;
        private System.Windows.Forms.Panel iconCircle;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblSub;
        private System.Windows.Forms.Panel body;
        private System.Windows.Forms.TableLayoutPanel grid;
        private System.Windows.Forms.Panel footer;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
    }
}