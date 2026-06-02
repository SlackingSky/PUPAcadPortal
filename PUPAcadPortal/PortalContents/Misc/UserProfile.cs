using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Misc
{
    public partial class UserProfile : UserControl
    {
        private const int MaxWidth = 172;
        private const int MaxHeight = 40;

        private const float MaxFontSize = 14f; // The normal starting size
        private const float MinFontSize = 6f;  // The smallest readable size
        public UserProfile()
        {
            InitializeComponent();
            lblName.TextChanged += lblName_TextChanged;
            WireUpAllClicks(this);
        }

        private void lblName_TextChanged(object? sender, EventArgs e)
        {
            ScaleLabelFont(lblName);
        }

        private void ScaleLabelFont(Label lbl)
        {
            if (string.IsNullOrEmpty(lbl.Text)) return;

            float currentSize = MaxFontSize;
            Font testFont = new Font(lbl.Font.FontFamily, currentSize, lbl.Font.Style);

            while (currentSize > MinFontSize)
            {
                Size textSize = TextRenderer.MeasureText(lbl.Text, testFont);

                if (textSize.Width <= MaxWidth && textSize.Height <= MaxHeight)
                {
                    break;
                }

                currentSize -= 0.5f;
                testFont.Dispose();
                testFont = new Font(lbl.Font.FontFamily, currentSize, lbl.Font.Style);
            }

            Font oldFont = lbl.Font;
            lbl.Font = testFont;

            if (oldFont != null) oldFont.Dispose();
        }

        private Bitmap GenerateCircularAvatar(string fullName, int size)
        {
            string initials = GetInitials(fullName);

            Bitmap avatar = new Bitmap(size, size);

            using (Graphics g = Graphics.FromImage(avatar))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                using (Brush bgBrush = new SolidBrush(Color.FromArgb(0xFF, 0xC1, 0x07)))
                {
                    g.FillEllipse(bgBrush, 0, 0, size - 1, size - 1);
                }

                if (initials.Length > 0)
                {
                    float fontSize = size * 0.35f;

                    using (Font font = new Font("Segoe UI", fontSize, FontStyle.Bold))
                    using (Brush textBrush = new SolidBrush(Color.FromArgb(0x1A, 0x1A, 0x1A)))
                    {
                        StringFormat format = new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        };

                        Rectangle boundingBox = new Rectangle(-10, 0, size + 20, size);
                        g.DrawString(initials, font, textBrush, boundingBox, format);
                    }
                }
            }

            return avatar;
        }

        private string GetInitials(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return "";

            string[] nameParts = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (nameParts.Length == 1)
            {
                return nameParts[0][0].ToString().ToUpper();
            }
            else
            {
                char firstInitial = nameParts[0][0];
                char lastInitial = nameParts[nameParts.Length - 1][0];

                return $"{firstInitial}{lastInitial}".ToUpper();
            }
        }

        private void UserProfile_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                return;
            }

            string fullName = UserSession.FullName;
            lblName.Text = fullName;
            lblRole.Text = UserSession.Role;
            Bitmap profilePic = GenerateCircularAvatar(fullName, 40);
            pictureBox1.Image = profilePic;
        }

        private void WireUpAllClicks(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                child.Click += (sender, e) => this.OnClick(e);
                child.Cursor = Cursors.Hand;
                if (child.HasChildren)
                {
                    WireUpAllClicks(child);
                }
            }
        }
    }
}
