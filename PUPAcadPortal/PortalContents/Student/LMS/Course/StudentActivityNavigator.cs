using System;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class StudentActivityNavigator : UserControl
    {
        private StudentActivityDashboard _dashboard;
        private StudentActivityList _list;
        private StudentActivitySubmit _submit;

        private Control _current;

        public StudentActivityNavigator()
        {
            // This is required to link with the Designer.cs file
            InitializeComponent();

            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(245, 245, 245);

            ShowDashboard();
        }

        private void ShowDashboard()
        {
            _dashboard = new StudentActivityDashboard { Dock = DockStyle.Fill };
            _dashboard.OnOpenCourse += ShowList;
            SwapView(_dashboard);
        }

        private void ShowList(StudentCourse course)
        {
            _list = new StudentActivityList(course) { Dock = DockStyle.Fill };
            _list.OnBack += ShowDashboard;
            _list.OnOpenActivity += ShowSubmit;
            SwapView(_list);
        }

        private void ShowSubmit(StudentActivityItem activity)
        {
            StudentCourse course = (_list != null) ? new StudentCourse { Id = 0, Name = "", Code = "", Instructor = "" } : null;

            _submit = new StudentActivitySubmit(activity, course) { Dock = DockStyle.Fill };
            _submit.OnBack += () => ShowList(GetCurrentCourse());
            SwapView(_submit);
        }

        private StudentCourse GetCurrentCourse()
        {
            return _list != null
                ? new StudentCourse { Name = "", Code = "" }
                : new StudentCourse();
        }

        private void SwapView(Control next)
        {
            SuspendLayout();

            if (_current != null)
            {
                Controls.Remove(_current);
                _current.Dispose(); // Memory management is critical here
            }

            _current = next;
            Controls.Add(next);
            next.BringToFront();

            ResumeLayout(true);
        }
    }
}