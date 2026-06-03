namespace PUPAcadPortal.PortalContents.Instructor.LMS.Calendar
{
    partial class FacultyWeekView
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
            this._headerRow = new System.Windows.Forms.Panel();
            this._gridPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // _headerRow
            // 
            this._headerRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this._headerRow.Dock = System.Windows.Forms.DockStyle.Top;
            this._headerRow.Location = new System.Drawing.Point(0, 0);
            this._headerRow.Name = "_headerRow";
            this._headerRow.Size = new System.Drawing.Size(800, 36);
            this._headerRow.TabIndex = 0;
            this._headerRow.Paint += new System.Windows.Forms.PaintEventHandler(this.HeaderRow_Paint);
            // 
            // _gridPanel
            // 
            this._gridPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._gridPanel.BackColor = System.Drawing.Color.White;
            this._gridPanel.Location = new System.Drawing.Point(0, 36);
            this._gridPanel.Name = "_gridPanel";
            this._gridPanel.Size = new System.Drawing.Size(800, 1248); // 52px * 24 hours
            this._gridPanel.TabIndex = 1;
            this._gridPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.GridPanel_Paint);
            this._gridPanel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.GridPanel_DoubleClick);
            // 
            // FacultyWeekView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this._gridPanel);
            this.Controls.Add(this._headerRow);
            this.Name = "FacultyWeekView";
            this.Size = new System.Drawing.Size(800, 600);
            this.Resize += new System.EventHandler(this.FacultyWeekView_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _headerRow;
        private System.Windows.Forms.Panel _gridPanel;
    }
}