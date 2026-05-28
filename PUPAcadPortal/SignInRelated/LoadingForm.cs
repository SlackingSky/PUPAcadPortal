using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class LoadingForm : Form
    {
        private int startAngle = 0;
        public LoadingForm(Form parent)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Left = parent.Left + (parent.Width - this.Width) / 2;
            this.Top = parent.Top + (parent.Height - this.Height) / 2;
            this.DoubleBuffered = true;

            this.timer1.Tick += Timer1_Tick;
            this.Paint += LoadingForm_Paint;

            this.timer1.Interval = 60;
            this.timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            startAngle = (startAngle + 20) % 360;
            this.Invalidate();
        }

        private void LoadingForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int diameter = 32; 

            int x = 24;

            int y = (this.Height - diameter) / 2;

            using (Pen backPen = new Pen(Color.FromArgb(40, Color.Gray), 3))
            {
                e.Graphics.DrawEllipse(backPen, x, y, diameter, diameter);
            }

            using (Pen spinnerPen = new Pen(Color.Maroon, 3))
            {
                e.Graphics.DrawArc(spinnerPen, x, y, diameter, diameter, startAngle, 90);
            }

            using (Pen borderPen = new Pen(Color.LightGray, 1))
            {
                e.Graphics.DrawRectangle(borderPen, 0, 0, this.Width - 1, this.Height - 1);
            }
        }
    }
}