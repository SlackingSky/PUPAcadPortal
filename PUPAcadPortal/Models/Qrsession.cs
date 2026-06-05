using System;

namespace PUPAcadPortal.Models;

public partial class QrSession
{
    public int QrSessionId { get; set; }

    public int SessionId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime GeneratedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool IsActive { get; set; } = true;

    public virtual ClassSession Session { get; set; } = null!;
}