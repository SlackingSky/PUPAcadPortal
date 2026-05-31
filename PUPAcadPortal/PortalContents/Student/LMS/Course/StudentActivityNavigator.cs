using System;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Student.LMS.Course
{
    public partial class StudentActivityNavigator : UserControl
    {
        private StudentActivityDashboard _dashboard;
        private StudentActivityList _list;
        private StudentActivitySubmit _submit;
        private StudentCourse _currentCourse;

        private Control _current;

        public StudentActivityNavigator()
        {
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
            _currentCourse = course;

            _list = new StudentActivityList(course) { Dock = DockStyle.Fill };
            _list.OnBack += ShowDashboard;
            _list.OnOpenActivity += ShowSubmit;
            SwapView(_list);
        }

        private void ShowSubmit(StudentActivityItem activity)
        {
            _submit = new StudentActivitySubmit(activity, _currentCourse)
            { Dock = DockStyle.Fill };
            _submit.OnBack += () => ShowList(_currentCourse);
            SwapView(_submit);
        }


        private void SwapView(Control next)
        {
            SuspendLayout();

            if (_current != null)
            {
                Controls.Remove(_current);
                _current.Dispose();
            }

            _current = next;
            Controls.Add(next);
            next.BringToFront();

            ResumeLayout(true);
        }
    }
}