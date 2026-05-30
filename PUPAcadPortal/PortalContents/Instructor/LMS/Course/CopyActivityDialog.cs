using PUPAcadPortal.PortalContents.Instructor.LMS.Course;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class CopyActivityDialog : Form
    {
        private readonly ActivityItem _activity;
        private readonly List<CourseActivity> _courses;

        public CopyActivityDialog(ActivityItem activity, List<CourseActivity> allCourses)
        {
            InitializeComponent();

            _activity = activity;
            _courses = allCourses;

            lblSub.Text = $"From: {_activity.Title} ({_activity.TypeString})";
            PopulateCourses();
        }

        private void PopulateCourses()
        {
            foreach (var c in _courses)
            {
                lstCourses.Items.Add($"{c.CourseCode} – {c.CourseName}");
            }

            if (lstCourses.Items.Count > 0)
            {
                lstCourses.SelectedIndex = 0;
            }
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            if (lstCourses.SelectedIndex < 0)
            {
                lblStatus.Text = "⚠ Please select a destination course.";
                return;
            }

            var dest = _courses[lstCourses.SelectedIndex];

            var copy = new ActivityItem
            {
                CourseId = dest.CourseId,
                Title = _activity.Title + " (Copy)",
                Description = _activity.Description,
                Type = _activity.Type,
                Deadline = DateTime.Now.AddDays(7),
                Points = _activity.Points,
                HasRubric = _activity.HasRubric,
                TotalStudents = 35
            };

            if (chkCopyQuestions.Checked && _activity.Questions != null)
                copy.Questions = new List<QuizQuestion>(_activity.Questions);

            if (chkCopyRubric.Checked && _activity.RubricItems != null)
                copy.RubricItems = new List<RubricCriteria>(_activity.RubricItems);

            if (chkCopyFiles.Checked && _activity.AttachedFiles != null)
                copy.AttachedFiles = new List<CourseFileItem>(_activity.AttachedFiles);

            dest.Activities.Add(copy);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}