using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public int UserId { get; set; }

    public string RoleTitle { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
