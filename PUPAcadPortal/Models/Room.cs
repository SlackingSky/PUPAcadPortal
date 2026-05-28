using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string RoomName { get; set; } = null!;

    public string RoomType { get; set; } = null!;

    public string Building { get; set; } = null!;

    public int Capacity { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<CalendarEvent> CalendarEvents { get; set; } = new List<CalendarEvent>();

    public virtual ICollection<RoomSchedule> RoomSchedules { get; set; } = new List<RoomSchedule>();
}
