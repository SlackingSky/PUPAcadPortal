using System.ComponentModel; // Required for the attributes below

namespace PUPAcadPortal
{
    public partial class ActivityItem : UserControl
    {
        // Add these attributes to hide them from the WinForms Designer
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SavedInstructions { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SavedAttachedFilePath { get; set; }

        // Do the same for your existing fields to be safe
        [Browsable(false)]
        public string SavedQuestion;

        [Browsable(false)]
        public string[] SavedChoices = new string[4];

        [Browsable(false)]
        public string SavedTitle;

        public ActivityItem()
        {
            InitializeComponent();
        }

        // ... rest of your code ...
    }
}