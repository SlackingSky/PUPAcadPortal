using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PUPAcadPortal.Data
{
    public class EnrollmentData : INotifyPropertyChanged
    {
        private bool _isSelected;

        public string SubjectOfferingID { get; set; }
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
        public string Code { get; set; }
        public string CourseTitle { get; set; }
        public int Units { get; set; }
        public string Schedule { get; set; }
        public string Status { get; set; }
        public bool IsEligible { get; set; }
        public string PrerequisiteMessage { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}