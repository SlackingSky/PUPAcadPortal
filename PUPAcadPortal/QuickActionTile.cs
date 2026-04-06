using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace PUPAcadPortal
{
    public partial class QuickActionTile : UserControl
    {
        [Category("Appearance")]
        [Description("The icon shown on the tile.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image Icon
        {
            get => pbIcon.Image;
            set => pbIcon.Image = value;
        }

        [Category("Appearance")]
        [Description("The title text (bold, larger).")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Title
        {
            get => lblTitle.Text;
            set => lblTitle.Text = value;
        }

        [Category("Appearance")]
        [Description("The description text (smaller, below title).")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Description
        {
            get => lblDescription.Text;
            set => lblDescription.Text = value;
        }

        [Category("Action")]
        [Description("Occurs when the tile is clicked.")]
        public event EventHandler TileClick;

        public QuickActionTile()
        {
            InitializeComponent();

            // Forward clicks from all child controls
            this.Click += (s, e) => TileClick?.Invoke(this, e);
            pnlBackground.Click += (s, e) => TileClick?.Invoke(this, e);
            pnlIcon.Click += (s, e) => TileClick?.Invoke(this, e);
            pnlText.Click += (s, e) => TileClick?.Invoke(this, e);
            pbIcon.Click += (s, e) => TileClick?.Invoke(this, e);
            lblTitle.Click += (s, e) => TileClick?.Invoke(this, e);
            lblDescription.Click += (s, e) => TileClick?.Invoke(this, e);

            // Hover effects
            this.MouseEnter += (s, e) => SetHoverStyle(true);
            this.MouseLeave += (s, e) => SetHoverStyle(false);
            pnlBackground.MouseEnter += (s, e) => SetHoverStyle(true);
            pnlBackground.MouseLeave += (s, e) => SetHoverStyle(false);
            pnlIcon.MouseEnter += (s, e) => SetHoverStyle(true);
            pnlIcon.MouseLeave += (s, e) => SetHoverStyle(false);
            pnlText.MouseEnter += (s, e) => SetHoverStyle(true);
            pnlText.MouseLeave += (s, e) => SetHoverStyle(false);
        }

        private void SetHoverStyle(bool hover)
        {
            if (hover)
            {
                pnlBackground.BackColor = Color.FromArgb(245, 245, 245);
                this.Cursor = Cursors.Hand;
            }
            else
            {
                pnlBackground.BackColor = SystemColors.ControlLight;
                this.Cursor = Cursors.Default;
            }
        }
    }
}