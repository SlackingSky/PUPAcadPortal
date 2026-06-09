using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class QrSession
{
    public int QrSessionId { get; set; }

    public int SessionId { get; set; }

    /// <summary>
    /// &apos;Cryptographically signed JWT-style token (base64url.sig)&apos;
    /// </summary>
    public string Token { get; set; } = null!;

    public DateTime GeneratedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool? IsActive { get; set; }

    public virtual ClassSession Session { get; set; } = null!;
}
