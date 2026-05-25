// =============================================================================
// FILE: StudentAttendanceItem.cs
// PURPOSE: Custom UserControl – Streamlined with single multiline TextBox
//          CRITICAL: Main class is placed FIRST in the file to keep the
//                    Visual Studio designer initialization synchronized.
// =============================================================================
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    // The main UserControl must be the FIRST class in the file for the designer to work!
    public partial class StudentAttendanceItem : UserControl
    {
        // ── Data state ────────────────────────────────────────────────────────
        private string _studentId = string.Empty;
        private bool _suppressEvents = false;   // prevents event feedback loops

        // ── Custom event ──────────────────────────────────────────────────────
        public event EventHandler<AttendanceChangedEventArgs> AttendanceChanged;

        private static readonly Color LightBlueBg = Color.FromArgb(212, 230, 252);

        // ── Constructor ───────────────────────────────────────────────────────
        public StudentAttendanceItem()
        {
            // 1. Let the designer build the component variables in system memory first!
            InitializeComponent();

            // 2. Apply custom styling and behavior properties safely
            BuildLayout();
            WireEvents();
        }

        // ── Layout construction ───────────────────────────────────────────────
        private void BuildLayout()
        {
            // Safety guard to avoid null designer drops if the designer loads out of order
            if (cboStatus == null || txtRemarks == null || pnlCard == null || pnlNew == null)
                return;

            // Turn on custom owner-draw coloring for your status dropdown options
            cboStatus.FlatStyle = FlatStyle.Flat;
            cboStatus.DrawMode = DrawMode.OwnerDrawFixed;
            cboStatus.DrawItem += CboStatus_DrawItem;

            // Attach the card wrapper background paint handler for custom clean borders
            pnlCard.Paint += pnlCard_Paint;

            if (picAvatar1 != null)
                RenderDefaultAvatar(picAvatar1);
        }

        private void pnlCard_Paint(object sender, PaintEventArgs e)
        {
            if (pnlCard == null) return;
            var g = e.Graphics;
            var rect = new Rectangle(0, 0, pnlCard.Width - 1, pnlCard.Height - 1);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var brush = new SolidBrush(Color.White))
                g.FillRectangle(brush, rect);

            // Subtle base border line to cleanly define item rows
            using (var pen = new Pen(Color.FromArgb(230, 230, 230), 1))
                g.DrawRectangle(pen, rect);
        }

        // ── Owner-draw styling handler for status options ─────────────────────
        private void CboStatus_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || cboStatus == null) return;

            string item = cboStatus.Items[e.Index].ToString();
            Color fg;
            Color bg;

            // Detect if the item is currently highlighted/hovered by the user's mouse
            if (e.State.HasFlag(DrawItemState.Selected))
            {
                bg = Color.FromArgb(240, 240, 240); // Soft grey highlight instead of blinding blue
                fg = Color.Black;
            }
            else
            {
                bg = Color.White; // Clean canvas resting state
                switch (item)
                {
                    case "Present": fg = Color.FromArgb(0, 130, 0); break;
                    case "Absent": fg = Color.FromArgb(180, 0, 0); break;
                    case "Late": fg = Color.FromArgb(180, 120, 0); break;
                    case "Excused": fg = Color.FromArgb(0, 80, 160); break;
                    default: fg = Color.Black; break;
                }
            }

            // 1. Draw custom background seamlessly
            using (var bgBrush = new SolidBrush(bg))
            {
                e.Graphics.FillRectangle(bgBrush, e.Bounds);
            }

            // 2. Draw the text string safely inside its boundaries
            using (var brush = new SolidBrush(fg))
            {
                var sf = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Near
                };

                var textRect = new Rectangle(e.Bounds.X + 4, e.Bounds.Y, e.Bounds.Width - 4, e.Bounds.Height);
                e.Graphics.DrawString(item, cboStatus.Font, brush, textRect, sf);
            }

            // 3. Draw the focus rectangle overlay smoothly
            e.DrawFocusRectangle();
        }

        // ── Generates vector fallback circles ─────────────────────────────────
        private void RenderDefaultAvatar(PictureBox pb)
        {
            if (pb == null) return;
            var bmp = new Bitmap(42, 42);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(LightBlueBg);

                using (var brush = new SolidBrush(Color.FromArgb(100, 150, 210)))
                {
                    g.FillEllipse(brush, 11, 8, 20, 20);
                    g.FillEllipse(brush, 4, 25, 34, 30);
                }
            }
            pb.Image = bmp;

            using (var path = new GraphicsPath())
            {
                path.AddEllipse(0, 0, pb.Width, pb.Height);
                pb.Region = new Region(path);
            }
        }

        // ── Core Event Notification Dispatchers ───────────────────────────────
        private void WireEvents()
        {
            if (cboStatus != null) cboStatus.SelectedIndexChanged += (s, e) => FireChanged();
            if (txtRemarks != null) txtRemarks.TextChanged += (s, e) => FireChanged();
        }

        private void FireChanged()
        {
            if (_suppressEvents || cboStatus == null || txtRemarks == null) return;
            AttendanceChanged?.Invoke(this,
                new AttendanceChangedEventArgs(
                    _studentId,
                    cboStatus.SelectedItem?.ToString() ?? "",
                    txtRemarks.Text));
        }

        // ── Public API Data Hooks ──────────────────────────────────────────────
        public void PopulateData(string id, string name, string currentStatus, string currentRemarks)
        {
            _suppressEvents = true;

            _studentId = id;

            // ✅ Safely bind properties checking for null values cleanly
            if (lblName != null)
                lblName.Text = name;

            if (lblId != null)
                lblId.Text = id; // Updated to match uppercase 'ID' expected by InstructorPortal loop

            if (cboStatus != null)
            {
                // Ensure default fallback options are loaded if the drop down is blank
                if (cboStatus.Items.Count == 0)
                {
                    cboStatus.Items.AddRange(new object[] { "Present", "Absent", "Late", "Excused" });
                }

                int idx = cboStatus.Items.IndexOf(currentStatus);
                cboStatus.SelectedIndex = idx >= 0 ? idx : 0;
            }

            if (txtRemarks != null)
                txtRemarks.Text = currentRemarks;

            _suppressEvents = false;
        }

        /// <summary>
        /// Responsive scaling to prevent custom components from clipping inside the dashboard grid.
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Suspend layout rules to process dimensional shifts efficiently
            this.SuspendLayout();

            if (txtRemarks != null && cboStatus != null)
            {
                // Let the remarks textbox dynamically expand to fill the right edge margin lines smoothly
                int structuralRightMargin = this.Width - txtRemarks.Left - 15;
                if (structuralRightMargin > 100)
                {
                    txtRemarks.Width = structuralRightMargin;
                }
            }

            this.ResumeLayout();
        }


        private void InitializeComponent()
        {
            pnlCard = new RoundedPanel();
            txtRemarks = new TextBox();
            cboStatus = new ComboBox();
            lblId = new Label();
            lblName = new Label();
            picAvatar1 = new PictureBox();
            pnlNew = new RoundedPanel();
            pnlCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picAvatar1).BeginInit();
            SuspendLayout();
            // 
            // pnlCard
            // 
            pnlCard.BackColor = Color.Maroon;
            pnlCard.BorderRadius = 10;
            pnlCard.Controls.Add(txtRemarks);
            pnlCard.Controls.Add(cboStatus);
            pnlCard.Controls.Add(lblId);
            pnlCard.Controls.Add(lblName);
            pnlCard.Controls.Add(picAvatar1);
            pnlCard.Controls.Add(pnlNew);
            pnlCard.Location = new Point(3, 3);
            pnlCard.Name = "pnlCard";
            pnlCard.Padding = new Padding(3);
            pnlCard.Size = new Size(1334, 91);
            pnlCard.TabIndex = 1;
            // 
            // txtRemarks
            // 
            txtRemarks.Anchor = AnchorStyles.None;
            txtRemarks.BorderStyle = BorderStyle.None;
            txtRemarks.Location = new Point(981, 17);
            txtRemarks.Multiline = true;
            txtRemarks.Name = "txtRemarks";
            txtRemarks.Size = new Size(330, 60);
            txtRemarks.TabIndex = 4;
            // 
            // cboStatus
            // 
            cboStatus.Anchor = AnchorStyles.None;
            cboStatus.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboStatus.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboStatus.FlatStyle = FlatStyle.Flat;
            cboStatus.Font = new Font("Segoe UI Semibold", 13.2000008F, FontStyle.Bold);
            cboStatus.FormattingEnabled = true;
            cboStatus.Items.AddRange(new object[] { "Present", "Absent", "Late", "Excused" });
            cboStatus.Location = new Point(787, 32);
            cboStatus.Name = "cboStatus";
            cboStatus.Size = new Size(151, 38);
            cboStatus.TabIndex = 3;
            // 
            // lblId
            // 
            lblId.AutoSize = true;
            lblId.BackColor = Color.White;
            lblId.Font = new Font("Segoe UI Semibold", 13.2000008F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblId.Location = new Point(492, 32);
            lblId.Name = "lblId";
            lblId.Size = new Size(73, 31);
            lblId.TabIndex = 2;
            lblId.Text = "label1";
            lblId.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.BackColor = Color.White;
            lblName.Font = new Font("Segoe UI Semibold", 13.2000008F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblName.Location = new Point(152, 32);
            lblName.Name = "lblName";
            lblName.Size = new Size(73, 31);
            lblName.TabIndex = 1;
            lblName.Text = "label1";
            lblName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // picAvatar1
            // 
            picAvatar1.BackColor = Color.White;
            picAvatar1.Location = new Point(41, 14);
            picAvatar1.Name = "picAvatar1";
            picAvatar1.Size = new Size(60, 60);
            picAvatar1.SizeMode = PictureBoxSizeMode.Zoom;
            picAvatar1.TabIndex = 0;
            picAvatar1.TabStop = false;
            // 
            // pnlNew
            // 
            pnlNew.Anchor = AnchorStyles.None;
            pnlNew.BackColor = Color.White;
            pnlNew.BorderRadius = 10;
            pnlNew.Location = new Point(15, 6);
            pnlNew.Name = "pnlNew";
            pnlNew.Size = new Size(1313, 79);
            pnlNew.TabIndex = 5;
            // 
            // StudentAttendanceItem
            // 
            BackColor = SystemColors.Control;
            Controls.Add(pnlCard);
            Margin = new Padding(12, 0, 6, 6);
            Name = "StudentAttendanceItem";
            Padding = new Padding(3);
            Size = new Size(1340, 100);
            pnlCard.ResumeLayout(false);
            pnlCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picAvatar1).EndInit();
            ResumeLayout(false);

        }
    }

    // ── Helper Class moved safely to the bottom ──────────────────────────────
    public class AttendanceChangedEventArgs : EventArgs
    {
        public string StudentID { get; }
        public string Status { get; }
        public string Remarks { get; }

        public AttendanceChangedEventArgs(string studentId, string status, string remarks)
        {
            StudentID = studentId;
            Status = status;
            Remarks = remarks;
        }
    }
}