namespace PUPAcadPortal.PortalContents.Instructor.LMS.Calendar
{
    partial class FacultyDayView
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
            this._headerPanel = new System.Windows.Forms.Panel();
            this._gridPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // _headerPanel
            // 
            this._headerPanel.BackColor = System.Drawing.Color.White;
            this._headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._headerPanel.Location = new System.Drawing.Point(0, 0);
            this._headerPanel.Name = "_headerPanel";
            this._headerPanel.Size = new System.Drawing.Size(600, 50);
            this._headerPanel.TabIndex = 0;
            this._headerPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.HeaderPanel_Paint);
            // 
            // _gridPanel
            // 
            this._gridPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._gridPanel.Location = new System.Drawing.Point(0, 50);
            this._gridPanel.Name = "_gridPanel";
            this._gridPanel.Size = new System.Drawing.Size(600, 1440);
            this._gridPanel.TabIndex = 1;
            this._gridPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.GridPanel_Paint);
            this._gridPanel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.GridPanel_MouseDoubleClick);
            // 
            // FacultyDayView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this._gridPanel);
            this.Controls.Add(this._headerPanel);
            this.Name = "FacultyDayView";
            this.Size = new System.Drawing.Size(600, 600);
            this.Resize += new System.EventHandler(this.FacultyDayView_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _headerPanel;
        private System.Windows.Forms.Panel _gridPanel;
    }
}