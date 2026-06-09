using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;

namespace PUPAcadPortal.PortalContents.Instructor.LMS
{

    public partial class AttendanceGridControl : UserControl
    {
        private const int PAGE_SIZE = 15;
        private const int ROW_H = 46;
        private const int HEADER_H = 40;
        private const int BOTTOM_BAR_H = 48;

        private static readonly Color Maroon = Color.FromArgb(106, 0, 0);
        private static readonly Color MaroonDark = Color.FromArgb(80, 0, 0);
        private static readonly Color RowAlt = Color.FromArgb(252, 252, 252);
        private static readonly Color BorderGray = Color.FromArgb(230, 230, 230);
        private static readonly Color PresentGreen = Color.FromArgb(0, 140, 0);
        private static readonly Color LateOrange = Color.FromArgb(200, 110, 0);
        private static readonly Color AbsentRed = Color.Firebrick;
        private static readonly Color ExcusedGold = Color.DarkGoldenrod;

        // Tint applied to QR-verified rows so they stand out visually.
        private static readonly Color QrVerifiedTint = Color.FromArgb(232, 248, 232);  // soft green
        private static readonly Color QrVerifiedSelect = Color.FromArgb(210, 240, 210);

        private List<StudentAttendanceRecord> _source = new();
        private List<StudentAttendanceRecord> _filtered = new();
        private int _currentPage = 1;
        private int _totalPages = 1;

        public event EventHandler? AttendanceChanged;

        // ── Constructor ───────────────────────────────────────────────────────────
        public AttendanceGridControl()
        {
            InitializeComponent();

            _pageButtons = new[] { btnPage1, btnPage2, btnPage3 };
            _dgv.Columns["colRemarks"].DefaultCellStyle.ForeColor = Color.Gray;

            // Tooltip for locked rows
            var tip = new ToolTip();
            _dgv.CellToolTipTextNeeded += (s, e) =>
            {
                if (e.RowIndex < 0 || e.RowIndex >= _dgv.Rows.Count) return;
                if (_dgv.Rows[e.RowIndex].Tag is StudentAttendanceRecord r && r.IsQrVerified)
                    e.ToolTipText = "QR-verified — editing is not permitted for this record.";
            };

            btnMarkPresent.Click += (_, __) => BulkMark(AttendanceStatus.Present);
            btnMarkLate.Click += (_, __) => BulkMark(AttendanceStatus.Late);
            btnMarkAbsent.Click += (_, __) => BulkMark(AttendanceStatus.Absent);
            btnMarkExcused.Click += (_, __) => BulkMark(AttendanceStatus.Excused);

            _btnFirst.Click += (_, __) => GoToPage(1);
            _btnPrev.Click += (_, __) => GoToPage(_currentPage - 1);
            _btnNext.Click += (_, __) => GoToPage(_currentPage + 1);
            _btnLast.Click += (_, __) => GoToPage(_totalPages);

            foreach (var btn in _pageButtons)
            {
                btn.Click += (s, __) =>
                { if (int.TryParse(((Button)s!).Text, out int p)) GoToPage(p); };
            }

            SizeChanged += (_, __) => PositionAll();

            WireEvents();
            RefreshView();
        }

        public DataGridView Grid => _dgv;

        // ── Data loading ──────────────────────────────────────────────────────────
        public void LoadStudents(IEnumerable<StudentAttendanceRecord> students)
        {
            _source = students.OrderBy(s => s.LastName).ThenBy(s => s.FirstName).ToList();
            _filtered = _source.ToList();
            _currentPage = 1;
            RefreshView();
        }

        public void ApplyFilter(string query)
        {
            query = query.Trim().ToLower();
            _filtered = string.IsNullOrEmpty(query)
                ? _source.ToList()
                : _source.Where(s =>
                    s.LastName.ToLower().Contains(query) ||
                    s.FirstName.ToLower().Contains(query) ||
                    s.IdNumber.ToLower().Contains(query)).ToList();
            _currentPage = 1;
            RefreshView();
        }

        // ── Layout ────────────────────────────────────────────────────────────────
        private void PositionAll()
        {
            const int BULK_H = 38;
            int gridH = Math.Max(0, Height - BULK_H - BOTTOM_BAR_H);

            _pnlBulk.Size = new Size(Width, BULK_H);
            _pnlBulk.Location = new Point(0, 0);

            _dgv.Size = new Size(Width, gridH);
            _dgv.Location = new Point(0, BULK_H);

            if (_dgv.RowTemplate.Height > 0)
            {
                int avail = gridH - HEADER_H;
                _dgv.RowTemplate.Height = Math.Max(32, avail / PAGE_SIZE);
                foreach (DataGridViewRow row in _dgv.Rows)
                    row.Height = _dgv.RowTemplate.Height;
            }

            int barY = Height - BOTTOM_BAR_H + 8;
            _lblShowing.Location = new Point(10, barY);
            _pnlPagination.Location = new Point(Width - _pnlPagination.Width - 6, barY);
        }

        // ── Event wiring ──────────────────────────────────────────────────────────
        private void WireEvents()
        {
            _dgv.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (_dgv.IsCurrentCellDirty && _dgv.CurrentCell is DataGridViewComboBoxCell)
                    _dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };

            _dgv.CellValueChanged += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                var row = _dgv.Rows[e.RowIndex];
                if (row.Tag is not StudentAttendanceRecord rec) return;
                if (rec.IsQrVerified) return;   // ← QR-locked: ignore change

                if (e.ColumnIndex == _dgv.Columns["colStatus"]?.Index)
                {
                    string val = row.Cells["colStatus"].Value?.ToString() ?? "Present";
                    rec.Status = ParseStatus(val);
                    ApplyRemarksLock(row, rec.Status, rec.IsQrVerified);
                    ApplyRowColor(row, rec.Status, rec.IsQrVerified);
                    _dgv.InvalidateRow(e.RowIndex);
                    AttendanceChanged?.Invoke(this, EventArgs.Empty);
                }

                if (e.ColumnIndex == _dgv.Columns["colRemarks"]?.Index)
                {
                    string txt = row.Cells["colRemarks"].Value?.ToString() ?? "";
                    if (txt != "Optional remarks") rec.Remarks = txt;
                }
            };

            _dgv.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                var row = _dgv.Rows[e.RowIndex];
                if (row.Tag is not StudentAttendanceRecord rec) return;

                int statusIdx = _dgv.Columns["colStatus"]?.Index ?? -1;
                if (e.ColumnIndex == statusIdx && e.Value != null)
                {
                    // QR-verified rows show a padlock badge in the status cell
                    if (rec.IsQrVerified)
                    {
                        e.Value = "Present";
                        e.CellStyle.ForeColor = PresentGreen;
                        e.CellStyle.Font = new Font("Segoe UI", 8.75f, FontStyle.Bold);
                    }
                    else
                    {
                        e.CellStyle.ForeColor = StatusForeColor(rec.Status);
                        e.CellStyle.Font = new Font("Segoe UI", 8.75f, FontStyle.Bold);
                    }
                }

                int remarksIdx = _dgv.Columns["colRemarks"]?.Index ?? -1;
                if (e.ColumnIndex == remarksIdx)
                {
                    if (rec.IsQrVerified)
                    {
                        e.Value = "Attendance Verified via QR Code";
                        e.CellStyle.ForeColor = Color.FromArgb(0, 120, 60);
                        e.CellStyle.Font = new Font("Segoe UI", 8.5f, FontStyle.Italic);
                    }
                    else if (rec.Status != AttendanceStatus.Excused)
                    {
                        e.CellStyle.BackColor = Color.FromArgb(245, 245, 245);
                        e.CellStyle.ForeColor = Color.FromArgb(160, 160, 160);
                        e.CellStyle.SelectionBackColor = Color.FromArgb(235, 235, 235);
                    }
                }
            };

            _dgv.RowPrePaint += (s, e) =>
            {
                if (e.RowIndex < 0 || e.RowIndex >= _dgv.Rows.Count) return;
                var row = _dgv.Rows[e.RowIndex];
                if (row.Tag is StudentAttendanceRecord rec)
                    ApplyRowColor(row, rec.Status, rec.IsQrVerified);
                else
                    row.DefaultCellStyle.BackColor = e.RowIndex % 2 == 0 ? Color.White : RowAlt;
            };

            _dgv.EditingControlShowing += (s, e) =>
            {
                int remarksIdx = _dgv.Columns["colRemarks"]?.Index ?? -1;
                if (_dgv.CurrentCell?.ColumnIndex == remarksIdx && e.Control is TextBox tb)
                    if (tb.Text == "Optional remarks") { tb.Text = ""; tb.ForeColor = Color.Black; }

                int statusIdx = _dgv.Columns["colStatus"]?.Index ?? -1;
                if (_dgv.CurrentCell?.ColumnIndex == statusIdx && e.Control is ComboBox cb)
                {
                    cb.DrawMode = DrawMode.OwnerDrawFixed;
                    cb.DrawItem -= StatusCombo_DrawItem;
                    cb.DrawItem += StatusCombo_DrawItem;
                }
            };

            // ── Block ALL edits on QR-verified rows ───────────────────────────────
            _dgv.CellBeginEdit += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                var row = _dgv.Rows[e.RowIndex];

                // Block if QR-verified
                if (row.Tag is StudentAttendanceRecord rec && rec.IsQrVerified)
                {
                    e.Cancel = true;
                    return;
                }

                // Original: block remarks unless Excused
                if (row.Tag is StudentAttendanceRecord rec2)
                {
                    int ri = _dgv.Columns["colRemarks"]?.Index ?? -1;
                    if (e.ColumnIndex == ri && rec2.Status != AttendanceStatus.Excused)
                        e.Cancel = true;
                }
            };
        }

        // ── Row colour helpers ────────────────────────────────────────────────────
        private static void ApplyRemarksLock(DataGridViewRow row, AttendanceStatus status, bool isQr)
        {
            int ri = row.DataGridView?.Columns["colRemarks"]?.Index ?? -1;
            if (ri < 0) return;
            var cell = row.Cells[ri];

            if (isQr)
            {
                cell.ReadOnly = true;
                cell.Style.BackColor = QrVerifiedTint;
                cell.Style.SelectionBackColor = QrVerifiedSelect;
                return;
            }

            bool editable = status == AttendanceStatus.Excused;
            cell.ReadOnly = !editable;
            cell.Style.BackColor = editable ? Color.Empty : Color.FromArgb(245, 245, 245);
            cell.Style.ForeColor = editable ? Color.Empty : Color.FromArgb(160, 160, 160);
            cell.Style.SelectionBackColor = editable ? Color.Empty : Color.FromArgb(235, 235, 235);
        }

        private static void ApplyRowColor(DataGridViewRow row, AttendanceStatus status, bool isQr)
        {
            Color bg;
            if (isQr)
            {
                bg = QrVerifiedTint;
            }
            else
            {
                bg = status switch
                {
                    AttendanceStatus.Absent => Color.FromArgb(255, 244, 244),
                    AttendanceStatus.Late => Color.FromArgb(255, 250, 235),
                    AttendanceStatus.Excused => Color.FromArgb(255, 253, 230),
                    _ => Color.White,
                };
            }
            row.DefaultCellStyle.BackColor = bg;
            row.DefaultCellStyle.SelectionBackColor = isQr ? QrVerifiedSelect : bg;
        }

        // Overload kept for callers that do not know IsQrVerified
        private static void ApplyRowColor(DataGridViewRow row, AttendanceStatus status)
            => ApplyRowColor(row, status, false);

        private void StatusCombo_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || sender is not ComboBox cb) return;
            e.DrawBackground();
            string item = cb.Items[e.Index]?.ToString() ?? "";
            var st = ParseStatus(item);
            bool sel = (e.State & DrawItemState.Selected) != 0;
            Color back = sel ? Maroon : Color.White;
            Color fore = sel ? Color.White : StatusForeColor(st);
            using var bb = new SolidBrush(back); e.Graphics.FillRectangle(bb, e.Bounds);
            using var fb = new SolidBrush(fore);
            using var f = new Font("Segoe UI", 8.75f, FontStyle.Bold);
            e.Graphics.DrawString(item, f, fb, e.Bounds.X + 4,
                e.Bounds.Y + (e.Bounds.Height - f.Height) / 2);
        }

        // ── Bulk mark (skips QR-verified rows) ────────────────────────────────────
        private void BulkMark(AttendanceStatus status)
        {
            int skipped = 0;
            foreach (var rec in _allOnPage)
            {
                if (rec.IsQrVerified) { skipped++; continue; }
                rec.Status = status;
            }

            _dgv.SuspendLayout();
            foreach (DataGridViewRow row in _dgv.Rows)
            {
                if (row.Tag is not StudentAttendanceRecord rec) continue;
                if (rec.IsQrVerified) continue;   // do not overwrite QR-locked rows
                row.Cells["colStatus"].Value = StatusString(status);
                ApplyRemarksLock(row, status, false);
                ApplyRowColor(row, status, false);
            }
            _dgv.ResumeLayout();
            _dgv.Invalidate();
            AttendanceChanged?.Invoke(this, EventArgs.Empty);

            if (skipped > 0)
            {
                // Inform faculty without interrupting workflow
                var tip = new ToolTip();
                tip.Show(
                    $"{skipped} QR-verified record{(skipped > 1 ? "s" : "")} were not changed.",
                    this, Width / 2, Height - 20, 3000);
            }
        }

        // ── Pagination / view ─────────────────────────────────────────────────────
        private List<StudentAttendanceRecord> _allOnPage = new();
        private Button[] _pageButtons;

        private void RefreshView()
        {
            _totalPages = Math.Max(1, (int)Math.Ceiling(_filtered.Count / (double)PAGE_SIZE));
            _currentPage = Math.Clamp(_currentPage, 1, _totalPages);

            var page = _filtered
                .Skip((_currentPage - 1) * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .ToList();

            _allOnPage = page;

            _dgv.SuspendLayout();
            _dgv.Rows.Clear();

            for (int i = 0; i < PAGE_SIZE; i++)
            {
                if (i < page.Count)
                {
                    var s = page[i];

                    // Remarks column: QR-verified rows show the verification label
                    string remarksDisplay = s.IsQrVerified
                        ? "Attendance Verified via QR Code"
                        : string.IsNullOrWhiteSpace(s.Remarks)
                            ? "Optional remarks"
                            : s.Remarks;

                    int idx = _dgv.Rows.Add(
                        s.RowNumber,
                        s.LastName,
                        s.FirstName,
                        s.MiddleInitial,
                        s.IdNumber,
                        StatusString(s.Status),
                        remarksDisplay);

                    var row = _dgv.Rows[idx];
                    row.Tag = s;

                    // Lock the entire row if QR-verified
                    row.ReadOnly = s.IsQrVerified;

                    ApplyRemarksLock(row, s.Status, s.IsQrVerified);
                    ApplyRowColor(row, s.Status, s.IsQrVerified);
                }
                else
                {
                    int idx = _dgv.Rows.Add("", "", "", "", "", "Present", "");
                    var row = _dgv.Rows[idx];
                    row.Tag = null;
                    row.ReadOnly = true;
                    row.DefaultCellStyle.BackColor = (i % 2 == 0) ? Color.White : RowAlt;
                    row.DefaultCellStyle.ForeColor = Color.Transparent;
                }
            }

            _dgv.ResumeLayout();
            UpdatePagination();
            UpdateShowingLabel();
            PositionAll();
        }

        private void UpdatePagination()
        {
            int[] vis = GetVisiblePages();
            for (int i = 0; i < _pageButtons.Length; i++)
            {
                if (i < vis.Length)
                {
                    _pageButtons[i].Text = vis[i].ToString();
                    _pageButtons[i].Visible = true;
                    bool active = vis[i] == _currentPage;
                    _pageButtons[i].BackColor = active ? Maroon : Color.White;
                    _pageButtons[i].ForeColor = active ? Color.White : Color.Black;
                    _pageButtons[i].Font = new Font("Segoe UI", 8.75f,
                        active ? FontStyle.Bold : FontStyle.Regular);
                }
                else _pageButtons[i].Visible = false;
            }
            _btnFirst.Enabled = _btnPrev.Enabled = _currentPage > 1;
            _btnNext.Enabled = _btnLast.Enabled = _currentPage < _totalPages;
        }

        private int[] GetVisiblePages()
        {
            if (_totalPages <= 3) return Enumerable.Range(1, _totalPages).ToArray();
            if (_currentPage == 1) return new[] { 1, 2, 3 };
            if (_currentPage == _totalPages) return new[] { _totalPages - 2, _totalPages - 1, _totalPages };
            return new[] { _currentPage - 1, _currentPage, _currentPage + 1 };
        }

        private void UpdateShowingLabel()
        {
            int start = (_currentPage - 1) * PAGE_SIZE + 1;
            int end = Math.Min(_currentPage * PAGE_SIZE, _filtered.Count);
            _lblShowing.Text = _filtered.Count == 0
                ? "No students found"
                : $"Showing {start}–{end} of {_filtered.Count} students";
        }

        private void GoToPage(int page)
        {
            if (page < 1 || page > _totalPages) return;
            _currentPage = page;
            RefreshView();
        }

        // ── Static helpers ────────────────────────────────────────────────────────
        private static AttendanceStatus ParseStatus(string? s) => s switch
        {
            "Absent" => AttendanceStatus.Absent,
            "Late" => AttendanceStatus.Late,
            "Excused" => AttendanceStatus.Excused,
            _ => AttendanceStatus.Present,
        };

        private static string StatusString(AttendanceStatus s) => s switch
        {
            AttendanceStatus.Absent => "Absent",
            AttendanceStatus.Late => "Late",
            AttendanceStatus.Excused => "Excused",
            _ => "Present",
        };

        private static Color StatusForeColor(AttendanceStatus s) => s switch
        {
            AttendanceStatus.Absent => AbsentRed,
            AttendanceStatus.Late => LateOrange,
            AttendanceStatus.Excused => ExcusedGold,
            _ => PresentGreen,
        };
    }
}