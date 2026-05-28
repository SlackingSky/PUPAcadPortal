namespace PUPAcadPortal
{
    partial class QuickActionTile
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlBackground = new Panel();
            pnlIcon = new Panel();
            pbIcon = new PictureBox();
            pnlText = new Panel();
            lblTitle = new Label();
            lblDescription = new Label();
            pnlBackground.SuspendLayout();
            pnlIcon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbIcon).BeginInit();
            pnlText.SuspendLayout();
            SuspendLayout();

            // pnlBackground
            pnlBackground.Dock = DockStyle.Fill;
            pnlBackground.BackColor = SystemColors.ControlLight;
            pnlBackground.Controls.Add(pnlIcon);
            pnlBackground.Controls.Add(pnlText);

            // pnlIcon (left panel, fixed width)
            pnlIcon.Dock = DockStyle.Left;
            pnlIcon.Width = 90;
            pnlIcon.Controls.Add(pbIcon);

            // pbIcon
            pbIcon.Dock = DockStyle.Fill;
            pbIcon.SizeMode = PictureBoxSizeMode.Zoom;
            pbIcon.BackColor = Color.Maroon;
            pbIcon.Margin = new Padding(10);

            // pnlText (right panel, fills remaining space)
            pnlText.Dock = DockStyle.Fill;
            pnlText.Padding = new Padding(10, 8, 10, 8);
            pnlText.Controls.Add(lblTitle);
            pnlText.Controls.Add(lblDescription);

            // lblTitle
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.Maroon;
            lblTitle.Text = "Title";
            lblTitle.Height = 40;

            // lblDescription
            lblDescription.Dock = DockStyle.Top;
            lblDescription.Font = new Font("Segoe UI", 10F);
            lblDescription.ForeColor = Color.DimGray;
            lblDescription.Text = "Description";
            lblDescription.Height = 30;

            // QuickActionTile
            this.Controls.Add(pnlBackground);
            this.Size = new Size(350, 110);
            this.ResumeLayout(false);

            pnlBackground.ResumeLayout(false);
            pnlIcon.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbIcon).EndInit();
            pnlText.ResumeLayout(false);
        }

        private Panel pnlBackground;
        private Panel pnlIcon;
        private PictureBox pbIcon;
        private Panel pnlText;
        private Label lblTitle;
        private Label lblDescription;
    }
}