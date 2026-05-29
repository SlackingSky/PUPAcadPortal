using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using PUPAcadPortal.PortalContents.Instructor.LMS;

namespace PUPAcadPortal
{
    public partial class AttendanceGridControl : UserControl
    {
        private const int PAGE_SIZE = 15;
        private List<StudentAttendanceRecord> _source = new();
        private List<StudentAttendanceRecord> _filtered = new();
        private int _currentPage = 1;
        private int _totalPages = 1;
        private DataGridView _dgv;
        private Label _lblShowing;
        private Panel _pnlPagination;
        private Button _btnFirst, _btnPrev, _btnNext, _btnLast;
        private Button[] _pageButtons;
        public event EventHandler? AttendanceChanged;
        public AttendanceGridControl()
        {
            DoubleBuffered = true;
            BackColor = Color.White;

            BuildLayout();
            SetupGridColumns();
            WireGridEvents();
            RefreshView();
        }

        //  Public API 

        public void LoadStudents(IEnumerable<StudentAttendanceRecord> students)
        {
            _source = students
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .ToList();
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
        public DataGridView Grid => _dgv;

        //  Layout 

        private void BuildLayout()
        {
            _dgv = new DataGridView
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom |
                         AnchorStyles.Left | AnchorStyles.Right,
                Location = new Point(0, 0),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(230, 230, 230),
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ScrollBars = ScrollBars.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = false,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None,
            };
            _dgv.RowTemplate.Height = 44;

            // Header style
            _dgv.EnableHeadersVisualStyles = false;
            _dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(106, 0, 0);
            _dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            _dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            _dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _dgv.ColumnHeadersHeight = 38;
            _dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Row style
            _dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9f);
            _dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 240, 240);
            _dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

            Controls.Add(_dgv);

            // Bottom bar
            _lblShowing = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(80, 80, 80),
                Text = "Loading...",
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
            };

            _pnlPagination = new Panel
            {
                Height = 30,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                BackColor = Color.Transparent,
            };

            _btnFirst = MakeNavButton("«");
            _btnPrev = MakeNavButton("‹");
            _pageButtons = new[] { MakePageButton("1"), MakePageButton("2"), MakePageButton("3") };
            _btnNext = MakeNavButton("›");
            _btnLast = MakeNavButton("»");

            int bx = 0;
            void AddBtn(Button b)
            {
                b.Location = new Point(bx, 0);
                _pnlPagination.Controls.Add(b);
                bx += b.Width + 2;
            }
            AddBtn(_btnFirst);
            AddBtn(_btnPrev);
            foreach (var pb in _pageButtons) AddBtn(pb);
            AddBtn(_btnNext);
            AddBtn(_btnLast);
            _pnlPagination.Width = bx;

            Controls.Add(_lblShowing);
            Controls.Add(_pnlPagination);

            _btnFirst.Click += (_, __) => GoToPage(1);
            _btnPrev.Click += (_, __) => GoToPage(_currentPage - 1);
            _btnNext.Click += (_, __) => GoToPage(_currentPage + 1);
            _btnLast.Click += (_, __) => GoToPage(_totalPages);
            foreach (var btn in _pageButtons)
            {
                var b = btn;
                b.Click += (s, __) =>
                {
                    if (int.TryParse(((Button)s!).Text, out int p)) GoToPage(p);
                };
            }

            SizeChanged += (_, __) => PositionBottomBar();
            PositionBottomBar();
        }

        private void PositionBottomBar()
        {
            const int BOTTOM_H = 44;
            int gridH = Math.Max(0, Height - BOTTOM_H);
            _dgv.Size = new Size(Width, gridH);

            if (_dgv.RowTemplate.Height > 0)
            {
                int available = gridH - _dgv.ColumnHeadersHeight;
                _dgv.RowTemplate.Height = Math.Max(30, available / PAGE_SIZE);
                foreach (DataGridViewRow row in _dgv.Rows)
                    row.Height = _dgv.RowTemplate.Height;
            }

            int barY = Height - BOTTOM_H + 7;
            _lblShowing.Location = new Point(10, barY);
            _pnlPagination.Location = new Point(Width - _pnlPagination.Width - 5, barY);
        }

        //  Grid columns 

        private void SetupGridColumns()
        {
            _dgv.Columns.Clear();

            //  name columns
            _dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colLastName",
                HeaderText = "Last Name",
                Width = 140,
                ReadOnly = true,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft },
            });

            _dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colFirstName",
                HeaderText = "First Name",
                Width = 140,
                ReadOnly = true,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft },
            });

            _dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colMI",
                HeaderText = "MI",
                Width = 46,
                ReadOnly = true,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter },
            });

            _dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colId",
                HeaderText = "ID Number",
                Width = 175,
                ReadOnly = true,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter },
            });

            var statusCol = new DataGridViewComboBoxColumn
            {
                Name = "colStatus",
                HeaderText = "Status",
                Width = 145,
                FlatStyle = FlatStyle.Flat,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
            };
            statusCol.Items.AddRange("Present", "Absent", "Excused");
            statusCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            statusCol.DefaultCellStyle.ForeColor = Color.Black;
            _dgv.Columns.Add(statusCol);

            //Remarks
            _dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colRemarks",
                HeaderText = "Remarks",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = { ForeColor = Color.Gray },
            });
        }

        //  Grid events 

        private void WireGridEvents()
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

                if (e.ColumnIndex == _dgv.Columns["colStatus"]?.Index)
                {
                    string val = row.Cells["colStatus"].Value?.ToString() ?? "Present";
                    rec.Status = val switch
                    {
                        "Absent" => AttendanceStatus.Absent,
                        "Excused" => AttendanceStatus.Excused,
                        _ => AttendanceStatus.Present,
                    };
                    ApplyRemarksLock(row, rec.Status);

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

                // Row background tint
                Color bg = rec.Status switch
                {
                    AttendanceStatus.Absent => Color.FromArgb(255, 245, 245),
                    AttendanceStatus.Excused => Color.FromArgb(255, 253, 235),
                    _ => Color.White,
                };
                if (!row.Selected)
                {
                    row.DefaultCellStyle.BackColor = bg;
                    row.DefaultCellStyle.SelectionBackColor = bg;
                }

                int statusIdx = _dgv.Columns["colStatus"]?.Index ?? -1;
                if (e.ColumnIndex == statusIdx && e.Value != null)
                {
                    e.CellStyle.ForeColor = rec.Status switch
                    {
                        AttendanceStatus.Absent => Color.Firebrick,
                        AttendanceStatus.Excused => Color.DarkGoldenrod,
                        AttendanceStatus.Present => Color.FromArgb(0, 140, 0),
                        _ => Color.Black,
                    };
                    e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                }

                int remarksIdx = _dgv.Columns["colRemarks"]?.Index ?? -1;
                if (e.ColumnIndex == remarksIdx)
                {
                    if (rec.Status != AttendanceStatus.Excused)
                    {
                        e.CellStyle.BackColor = Color.FromArgb(245, 245, 245);
                        e.CellStyle.ForeColor = Color.FromArgb(160, 160, 160);
                        e.CellStyle.SelectionBackColor = Color.FromArgb(235, 235, 235);
                    }
                    else if (e.Value?.ToString() == "Optional remarks")
                    {
                        e.CellStyle.ForeColor = Color.LightGray;
                    }
                }
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
                    cb.ForeColor = StatusColor(cb.Text);
                    cb.SelectedIndexChanged -= StatusCombo_SelectedIndexChanged;
                    cb.SelectedIndexChanged += StatusCombo_SelectedIndexChanged;
                }
            };

            _dgv.CellBeginEdit += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                var row = _dgv.Rows[e.RowIndex];
                if (row.Tag is not StudentAttendanceRecord rec) return;

                int remarksIdx = _dgv.Columns["colRemarks"]?.Index ?? -1;
                if (e.ColumnIndex == remarksIdx && rec.Status != AttendanceStatus.Excused)
                    e.Cancel = true;   
            };
        }

        private void ApplyRemarksLock(DataGridViewRow row, AttendanceStatus status)
        {
            int remarksIdx = _dgv.Columns["colRemarks"]?.Index ?? -1;
            if (remarksIdx < 0) return;

            var cell = row.Cells[remarksIdx];
            if (status != AttendanceStatus.Excused)
            {
                cell.ReadOnly = true;
                cell.Style.BackColor = Color.FromArgb(245, 245, 245);
                cell.Style.ForeColor = Color.FromArgb(160, 160, 160);
                cell.Style.SelectionBackColor = Color.FromArgb(235, 235, 235);
            }
            else
            {
                cell.ReadOnly = false;
                cell.Style.BackColor = Color.Empty;   
                cell.Style.ForeColor = Color.Empty;
                cell.Style.SelectionBackColor = Color.Empty;
            }
        }

        // Status 
        private void StatusCombo_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || sender is not ComboBox cb) return;
            e.DrawBackground();
            string item = cb.Items[e.Index]?.ToString() ?? "";
            Color fore = (e.State & DrawItemState.Selected) != 0 ? Color.White : StatusColor(item);
            Color back = (e.State & DrawItemState.Selected) != 0 ? Color.FromArgb(106, 0, 0) : Color.White;
            using var brush = new SolidBrush(back);
            e.Graphics.FillRectangle(brush, e.Bounds);
            using var foreBrush = new SolidBrush(fore);
            using var font = new Font("Segoe UI", 9f, FontStyle.Bold);
            e.Graphics.DrawString(item, font, foreBrush,
                e.Bounds.X + 4, e.Bounds.Y + (e.Bounds.Height - font.Height) / 2);
        }

        private void StatusCombo_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (sender is ComboBox cb) cb.ForeColor = StatusColor(cb.Text);
        }

        private static Color StatusColor(string? status) => status switch
        {
            "Absent" => Color.Firebrick,
            "Excused" => Color.DarkGoldenrod,
            "Present" => Color.FromArgb(0, 140, 0),
            _ => Color.Black,
        };

        //  Core refresh 

        private void RefreshView()
        {
            _totalPages = Math.Max(1, (int)Math.Ceiling(_filtered.Count / (double)PAGE_SIZE));
            _currentPage = Math.Clamp(_currentPage, 1, _totalPages);

            var page = _filtered
                .Skip((_currentPage - 1) * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .ToList();

            _dgv.SuspendLayout();
            _dgv.Rows.Clear();

            for (int i = 0; i < PAGE_SIZE; i++)
            {
                if (i < page.Count)
                {
                    var s = page[i];
                    int idx = _dgv.Rows.Add(
                        s.LastName,
                        s.FirstName,
                        s.MiddleInitial,
                        s.IdNumber,
                        s.Status.ToString(),
                        string.IsNullOrWhiteSpace(s.Remarks) ? "Optional remarks" : s.Remarks);
                    _dgv.Rows[idx].Tag = s;

                    ApplyRemarksLock(_dgv.Rows[idx], s.Status);
                }
                else
                {
                    int idx = _dgv.Rows.Add("", "", "", "", "Present", "");
                    _dgv.Rows[idx].Tag = null;
                    _dgv.Rows[idx].ReadOnly = true;
                    _dgv.Rows[idx].DefaultCellStyle.BackColor = Color.FromArgb(252, 252, 252);
                    _dgv.Rows[idx].DefaultCellStyle.ForeColor = Color.Transparent;
                }
            }

            _dgv.ResumeLayout();
            UpdatePagination();
            UpdateShowingLabel();
            PositionBottomBar();
        }

        //  Pagination 

        private void UpdatePagination()
        {
            int[] visible = GetVisiblePages();
            for (int i = 0; i < _pageButtons.Length; i++)
            {
                if (i < visible.Length)
                {
                    _pageButtons[i].Text = visible[i].ToString();
                    _pageButtons[i].Visible = true;
                    bool active = visible[i] == _currentPage;
                    _pageButtons[i].BackColor = active ? Color.FromArgb(128, 0, 0) : Color.White;
                    _pageButtons[i].ForeColor = active ? Color.White : Color.Black;
                    _pageButtons[i].Font = new Font("Segoe UI", 9f,
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
            int tot = _filtered.Count;
            _lblShowing.Text = tot == 0
                ? "No students found"
                : $"Showing {start} to {end} of {tot} students";
        }

        private void GoToPage(int page)
        {
            if (page < 1 || page > _totalPages) return;
            _currentPage = page;
            RefreshView();
        }

        //  Button factories 

        private static Button MakeNavButton(string text)
        {
            var b = new Button
            {
                Text = text,
                Size = new Size(32, 30),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f),
                BackColor = Color.White,
                ForeColor = Color.Black,
                Cursor = Cursors.Hand,
            };
            b.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            return b;
        }

        private static Button MakePageButton(string text)
        {
            var b = MakeNavButton(text);
            b.Size = new Size(35, 30);
            return b;
        }
    }
}