using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;

namespace PUPAcadPortal.PortalContents.Instructor.LMS
{
    //  Data model 
    public enum AttendanceStatus { Present, Absent, Excused,Late }

    public class StudentAttendanceRecord
    {
        // ── DB backing fields (NEW) ───────────────────────────────────────────
        /// <summary>
        /// AttendanceRecord.AttendanceId — 0 means this record does not yet
        /// exist in the database (will be INSERTed on save).
        /// </summary>
        public int AttendanceId { get; set; }

        /// <summary>
        /// Student.StudentId — used to match this UI row to the DB Student entity.
        /// </summary>
        public int StudentId { get; set; }

        // ── Display fields ────────────────────────────────────────────────────
        public int RowNumber { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleInitial { get; set; } = string.Empty;

        /// <summary>StudentNumber (e.g. "2024-01234-SM-0")</summary>
        public string IdNumber { get; set; } = string.Empty;

        public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;
        public string Remarks { get; set; } = string.Empty;
    }

    //  Course / Session catalogue 

    public class CourseSection
    {
        /// <summary>SubjectOfferingId — the PK used in ClassSession lookups.</summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>Subject name (e.g. "Introduction to Programming 1")</summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>Section identifier (e.g. "BSIT 1-1")</summary>
        public string Section { get; set; } = string.Empty;

        /// <summary>Formatted string shown in the ComboBox.</summary>
        public string DisplayName => $"{Title} — {Section}";
    }

    public class SessionSlot
    {
        public string Label { get; set; } = string.Empty;
    }

    //  Attendance data per session 

    public class SessionKey
    {
        public string CourseDisplay { get; set; } = string.Empty;
        public string SessionLabel { get; set; } = string.Empty;
        public System.DateTime Date { get; set; }

        public bool Equals(SessionKey? other)
        {
            if (other is null) return false;
            return CourseDisplay == other.CourseDisplay &&
                   SessionLabel == other.SessionLabel &&
                   Date.Date == other.Date.Date;
        }

        public override bool Equals(object? obj) => Equals(obj as SessionKey);

        public override int GetHashCode() =>
            System.HashCode.Combine(CourseDisplay, SessionLabel, Date.Date);
    }

    public class ManualEntryDialog : Form
    {
        private string _selectedStatus = "Present";
        public string SelectedStatus { get => _selectedStatus; private set => _selectedStatus = value; }
        private ComboBox _combo;

        public ManualEntryDialog(string studentName, string currentStatus)
        {
            Text = "Manual Entry";
            Size = new Size(320, 160);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            var lbl = new Label
            {
                Text = $"Student: {studentName}",
                Location = new Point(16, 16),
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
            };

            _combo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(16, 44),
                Width = 270,
                Font = new Font("Segoe UI", 10f),
            };
            _combo.Items.AddRange(new object[] { "Present", "Absent", "Excused" });
            _combo.Text = currentStatus;

            var btnOk = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new Point(130, 82),
                Size = new Size(75, 28),
                BackColor = Color.FromArgb(128, 0, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
            };
            btnOk.FlatAppearance.BorderSize = 0;

            var btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new Point(214, 82),
                Size = new Size(75, 28),
            };

            btnOk.Click += (s, e) => SelectedStatus = _combo.Text;
            Controls.AddRange(new Control[] { lbl, _combo, btnOk, btnCancel });
            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }
    }
}