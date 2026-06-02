using System;

namespace PUPAcadPortal
{
    public class InboxMessage
    {
        public int Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string SenderName { get; set; } = "Admin Office";
        public string SenderRole { get; set; } = "Administrator";
        public DateTime ReceivedAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
        public bool IsStarred { get; set; } = false;
        public string Tag { get; set; } = "General"; 
    }
}