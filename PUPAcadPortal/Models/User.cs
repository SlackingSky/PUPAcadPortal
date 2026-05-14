using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

/// <summary>
/// Contains users for the portal
/// </summary>
public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string? Email { get; set; }

    public string PasswordHash { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
