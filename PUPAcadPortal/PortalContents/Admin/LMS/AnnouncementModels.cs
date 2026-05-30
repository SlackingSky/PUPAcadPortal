using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Admin.LMS
{
    public class AnnouncementDataAdmin
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime PostDate { get; set; } = DateTime.Now;
        public bool IsUrgent { get; set; }
        public bool IsPinned { get; set; }
        public bool NotifyStudents { get; set; }
        public bool NotifyInstructors { get; set; }
        public string AttachmentPath { get; set; } = string.Empty;
        public List<string> Courses { get; set; } = new List<string>();
    }

    public class AdminAnnouncement
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = "General";
        public string Status { get; set; } = "active";
        public string InstructorName { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public bool IsPinned { get; set; }
        public bool IsUrgent { get; set; }
        public int ViewedCount { get; set; }
        public int TotalStudents { get; set; } = 40;
        public string? AttachedFile { get; set; }
        public bool NotifyStudents { get; set; }
        public bool NotifyInstructors { get; set; }
    }

    public partial class AnnouncementLayoutAdmin : UserControl
    {
        public event EventHandler<int>? CardClicked;
        public event EventHandler<int>? PinToggled;
        public event EventHandler<int>? MenuEditClicked;
        public event EventHandler<int>? MenuToggleClicked;
        public event EventHandler<int>? MenuDeleteClicked;

        public AnnouncementLayoutAdmin() { }

        public void LoadInstructor(int id, string title, string description, string category, string status, string instructorName, DateTime date, bool isPinned, bool isUrgent, int viewedCount, int totalStudents, int cardWidth) { }
        public void LoadStudent(int id, string title, string description, string category, string officeName, DateTime date, bool isUrgent, bool isPinned, bool isRead, int cardWidth, string instructorName = "") { }
    }
}