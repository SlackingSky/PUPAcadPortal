using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

/// <summary>
/// A table for the portal&apos;s users
/// </summary>
public partial class User
{
    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string? Email { get; set; }

    public string Role { get; set; } = null!;
}
