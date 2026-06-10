using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class CalendarEvent
{
    public int EventId { get; set; }

    public int UserId { get; set; }

    public string? SubjectOfferingId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string EventType { get; set; } = null!;

    public int? RoomId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public bool? IsPrivate { get; set; }

    public DateTime? EventDate { get; set; }

    public virtual Room? Room { get; set; }

    public virtual SubjectOffering? SubjectOffering { get; set; }

    public virtual User User { get; set; } = null!;
}
