using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PUPAcadPortal.Data;

namespace PUPAcadPortal
{
    /// <summary>
    /// A search/filter panel that slides in from the top.
    /// Users can filter by text, event type, course, and date range.
    /// </summary>
    public partial class FacultySearchPanel : Panel
    {
        public event Action<List<FacultyCalendarEvent>>? ResultsChanged;

        private TextBox _txtSearch;
        private ComboBox _cboType;
        private ComboBox _cboCourse;
        private DateTimePicker _dtpFrom;
        private DateTimePicker _dtpTo;
        private CheckBox _chkDateRange;
        private Button _btnSearch;
        private Button _btnClear;
        private FlowLayoutPanel _resultsFLP;
        private Label _lblCount;

        private static readonly Color Maroon = Color.FromArgb(136, 14, 79);
        private static readonly Font UIFont = new Font("Segoe UI", 8.5f);
        private static readonly Font BoldFont = new Font("Segoe UI", 8.5f, FontStyle.Bold);

        public event Action<FacultyCalendarEvent>? EventSelected;

        public FacultySearchPanel()
        {
            BackColor = Color.White;
            BorderStyle = BorderStyle.None;
            Padding = new Padding(12, 10, 12, 8);

            Paint += FacultySearchPanel_Paint;

            BuildControls();
        }

        private void FacultySearchPanel_Paint(object? sender, PaintEventArgs e)
        {
            using var p = new Pen(Color.FromArgb(220, 220, 220), 1);
            e.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
        }

        private void BuildControls()
        {
            // ── Row 1: Search + type filter ──────────────────────────────────
            var row1 = new FlowLayoutPanel
            {
                Left = 12,
                Top = 10,
                Width = ClientSize.Width - 24,
                Height = 34,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            };

            _txtSearch = new TextBox { Width = 220, Height = 30, Font = UIFont };
            _txtSearch.KeyDown += TxtSearch_KeyDown;

            _cboType = new ComboBox { Width = 140, DropDownStyle = ComboBoxStyle.DropDownList, Font = UIFont };
            _cboType.Items.Add("All Types");
            foreach (FacultyEventType t in Enum.GetValues<FacultyEventType>())
                _cboType.Items.Add(t);
            _cboType.SelectedIndex = 0;

            _cboCourse = new ComboBox { Width = 200, Font = UIFont };
            _cboCourse.Items.Add("All Courses");
            foreach (var c in FacultyCalendarData.GetAllCourses()) _cboCourse.Items.Add(c);
            _cboCourse.Text = "All Courses";

            _btnSearch = new Button { Width = 80, Height = 30, Text = "Search", BackColor = Maroon, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = UIFont };
            _btnSearch.FlatAppearance.BorderSize = 0;
            _btnSearch.Click += BtnSearch_Click;

            _btnClear = new Button { Width = 60, Height = 30, Text = "Clear", FlatStyle = FlatStyle.Flat, Font = UIFont };
            _btnClear.Click += BtnClear_Click;

            foreach (Control c in new Control[] { _txtSearch, _cboType, _cboCourse, _btnSearch, _btnClear })
            {
                c.Margin = new Padding(0, 0, 6, 0);
                row1.Controls.Add(c);
            }
            Controls.Add(row1);

            // ── Row 2: Date range ────────────────────────────────────────────
            var row2 = new FlowLayoutPanel
            {
                Left = 12,
                Top = 50,
                Width = ClientSize.Width - 24,
                Height = 30,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            };

            _chkDateRange = new CheckBox { Text = "Date range:", AutoSize = true, Font = UIFont, Margin = new Padding(0, 4, 6, 0) };
            _chkDateRange.CheckedChanged += ChkDateRange_CheckedChanged;

            _dtpFrom = new DateTimePicker { Width = 140, Font = UIFont, Enabled = false, Format = DateTimePickerFormat.Short, Margin = new Padding(0, 0, 6, 0) };
            _dtpTo = new DateTimePicker { Width = 140, Font = UIFont, Enabled = false, Format = DateTimePickerFormat.Short, Margin = new Padding(0, 0, 6, 0) };
            _dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            _dtpTo.Value = _dtpFrom.Value.AddMonths(1).AddDays(-1);

            var lblTo = new Label { Text = "to", AutoSize = true, Font = UIFont, Margin = new Padding(0, 6, 6, 0) };
            row2.Controls.AddRange(new Control[] { _chkDateRange, _dtpFrom, lblTo, _dtpTo });
            Controls.Add(row2);

            // ── Result count ─────────────────────────────────────────────────
            _lblCount = new Label { Left = 12, Top = 86, AutoSize = true, Font = UIFont, ForeColor = Color.Gray };
            Controls.Add(_lblCount);

            // ── Results list ─────────────────────────────────────────────────
            _resultsFLP = new FlowLayoutPanel
            {
                Left = 0,
                Top = 108,
                Width = ClientSize.Width,
                Height = 160,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
            };
            _resultsFLP.HorizontalScroll.Enabled = false;
            _resultsFLP.HorizontalScroll.Visible = false;
            Controls.Add(_resultsFLP);

            Resize += (s, e) =>
            {
                row1.Width = ClientSize.Width - 24;
                row2.Width = ClientSize.Width - 24;
                _resultsFLP.Width = ClientSize.Width;
                _resultsFLP.Height = ClientSize.Height - 108;
            };
        }

        private void DoSearch()
        {
            FacultyEventType? type = _cboType.SelectedItem is FacultyEventType t ? (FacultyEventType?)t : null;
            string? course = _cboCourse.Text == "All Courses" ? null : _cboCourse.Text;
            DateTime? from = _chkDateRange.Checked ? (DateTime?)_dtpFrom.Value : null;
            DateTime? to = _chkDateRange.Checked ? (DateTime?)_dtpTo.Value : null;

            var results = FacultyCalendarData.Search(_txtSearch.Text, type, course, from, to);
            ShowResults(results);
            ResultsChanged?.Invoke(results);
        }

        private void ClearSearch()
        {
            _txtSearch.Text = "";
            _cboType.SelectedIndex = 0;
            _cboCourse.Text = "All Courses";
            _chkDateRange.Checked = false;
            _resultsFLP.Controls.Clear();
            _lblCount.Text = "";
            ResultsChanged?.Invoke(new List<FacultyCalendarEvent>());
        }

        private void TxtSearch_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) DoSearch();
        }

        private void BtnSearch_Click(object? sender, EventArgs e) => DoSearch();

        private void BtnClear_Click(object? sender, EventArgs e) => ClearSearch();

        private void ChkDateRange_CheckedChanged(object? sender, EventArgs e)
        {
            _dtpFrom.Enabled = _chkDateRange.Checked;
            _dtpTo.Enabled = _chkDateRange.Checked;
        }

        private void ShowResults(List<FacultyCalendarEvent> results)
        {
            _resultsFLP.Controls.Clear();
            _lblCount.Text = $"{results.Count} result{(results.Count == 1 ? "" : "s")} found";

            foreach (var ev in results)
            {
                int rw = Math.Max(80, _resultsFLP.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 4);
                var row = new Panel
                {
                    Width = rw,
                    Height = 42,
                    BackColor = Color.White,
                    Margin = new Padding(0, 0, 0, 2),
                    Cursor = Cursors.Hand,
                    Tag = ev,
                };

                var dot = new Panel { Width = 6, Height = 6, Top = 18, Left = 6, BackColor = ev.GetColor() };
                var lTitle = new Label
                {
                    Text = $"{ev.GetTypeIcon()} {ev.Title}",
                    Left = 18,
                    Top = 4,
                    Width = rw - 140,
                    Font = BoldFont,
                    ForeColor = Color.FromArgb(40, 40, 40),
                    AutoSize = false,
                    Height = 18,
                    AutoEllipsis = true,
                };
                var lSub = new Label
                {
                    Text = $"{ev.Date:MMM dd}  •  {ev.GetTypeLabel()}  •  {ev.Course}",
                    Left = 18,
                    Top = 24,
                    Width = rw - 24,
                    Font = new Font("Segoe UI", 7.5f),
                    ForeColor = Color.Gray,
                    AutoSize = false,
                    Height = 14,
                    AutoEllipsis = true,
                };
                var lDate = new Label
                {
                    Text = ev.Date.ToString("ddd, MMM dd"),
                    Left = rw - 110,
                    Top = 4,
                    Width = 100,
                    Font = UIFont,
                    ForeColor = Color.FromArgb(100, 100, 100),
                    TextAlign = ContentAlignment.MiddleRight,
                    AutoSize = false,
                    Height = 18,
                };

                row.Controls.AddRange(new Control[] { dot, lTitle, lSub, lDate });
                row.Paint += (s, pe) =>
                {
                    using var p = new Pen(Color.FromArgb(235, 235, 235));
                    pe.Graphics.DrawLine(p, 0, row.Height - 1, row.Width, row.Height - 1);
                };

                var capturedEv = ev;
                Action clicked = () => EventSelected?.Invoke(capturedEv);
                row.Click += (s, e) => clicked();
                lTitle.Click += (s, e) => clicked();

                _resultsFLP.Controls.Add(row);
            }
        }

        public void RefreshCourses()
        {
            var current = _cboCourse.Text;
            _cboCourse.Items.Clear();
            _cboCourse.Items.Add("All Courses");
            foreach (var c in FacultyCalendarData.GetAllCourses()) _cboCourse.Items.Add(c);
            _cboCourse.Text = current;
        }
    }
}