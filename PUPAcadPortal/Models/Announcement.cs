using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class Announcement
{
    public int AnnouncementId { get; set; }

    public int CreatedByUserId { get; set; }

    public string? SubjectOfferingId { get; set; }

    public string? Title { get; set; } = null!;

    public string? Content { get; set; } = null!;

    public string? Category { get; set; } = null!;

    public bool IsUrgent { get; set; }

    public bool IsPinned { get; set; }

    public string? AttachedFile { get; set; }

    public string? OriginalFileName { get; set; }
    public int TargetRoleId { get; set; }

    public DateTime PostedDate { get; set; }

    public string? OriginalFileName { get; set; }

    public int? TargetRoleId { get; set; }

    public virtual User CreatedByUser { get; set; } = null!;

    public virtual SubjectOffering? SubjectOffering { get; set; }

    public virtual ICollection<Role> TargetRoles { get; set; } = new List<Role>();

  
}
