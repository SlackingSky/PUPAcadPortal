using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
}
