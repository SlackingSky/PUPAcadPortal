using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class DepartmentPrefix
{
    public int PrefixId { get; set; }

    public string Prefix { get; set; } = null!;

    public int DepartmentId { get; set; }

    public virtual Department Department { get; set; } = null!;
}
