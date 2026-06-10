using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class AttendanceRecord
{
    public int AttendanceId { get; set; }

    public int SessionId { get; set; }

    public int StudentId { get; set; }

    public string Status { get; set; } = null!;

    public string? Remarks { get; set; }

    /// <summary>
    /// &apos;1 = attendance recorded via QR scan; row is read-only&apos;
    /// </summary>
    public bool IsQrVerified { get; set; }

    /// <summary>
    /// &apos;UTC timestamp of the QR scan&apos;
    /// </summary>
    public DateTime? QrScannedAt { get; set; }

    /// <summary>
    /// &apos;GUID nonce from the QR token — enforces single-use per token&apos;
    /// </summary>
    public string? QrNonce { get; set; }

    public virtual ICollection<QrScanLog> QrScanLogs { get; set; } = new List<QrScanLog>();

    public virtual ClassSession Session { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
