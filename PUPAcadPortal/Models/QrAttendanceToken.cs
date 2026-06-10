using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PUPAcadPortal.Models
{
    public sealed class QrTokenPayload
    {
        public int Sid { get; set; }   // ClassSession.SessionId
        public string Oid { get; set; } = string.Empty;   // SubjectOfferingId
        public string Date { get; set; } = string.Empty;   // yyyy-MM-dd
        public string From { get; set; } = string.Empty;   // HH:mm  window open
        public string To { get; set; } = string.Empty;   // HH:mm  window close
        public long Exp { get; set; }   // unix seconds  – hard expiry
        public long Iat { get; set; }   // unix seconds  – issued at
        public string Nonce { get; set; } = string.Empty;   // GUID  – replay guard
    }

    public enum QrValidationResult
    {
        Valid,
        InvalidSignature,
        Expired,                  // token exp has passed
        OutsideAttendanceWindow,  // current time < From  or > To  + grace
        SessionDateMismatch,      // scanned on a different calendar date
        SessionNotFound,          // ClassSession row missing
        AlreadyRecorded,          // student already has a record for this session
        MalformedToken,           // can't parse JSON / bad base64
    }

    public sealed class QrValidationOutcome
    {
        public QrValidationResult Result { get; init; }
        public QrTokenPayload? Payload { get; init; }
        public string Message { get; init; } = string.Empty;

        public bool IsValid => Result == QrValidationResult.Valid;

        public static QrValidationOutcome Fail(QrValidationResult r, string msg)
            => new() { Result = r, Message = msg };

        public static QrValidationOutcome Ok(QrTokenPayload p)
            => new() { Result = QrValidationResult.Valid, Payload = p, Message = "OK" };
    }

    public static class QrTokenService
    {
        // Grace period added to the session-end time before the QR expires.
        // A student arriving a few minutes late can still scan.
        private const int GRACE_MINUTES = 10;

        // Key is read once and cached.  In production wire this to
        // IConfiguration / secure key store.
        private static readonly byte[] _signingKey = DeriveKey(
            System.Configuration.ConfigurationManager.AppSettings["QrSigningKey"]
                ?? "PUPAcadPortal_DefaultDev_Key_CHANGE_IN_PROD");

        public static string Build(
            int sessionId,
            string subjectOfferingId,
            DateTime sessionDate,
            TimeSpan? startTime,
            TimeSpan? endTime)
        {
            var start = startTime ?? TimeSpan.Zero;
            var end = endTime ?? new TimeSpan(23, 59, 0);

            var windowClose = sessionDate.Date + end + TimeSpan.FromMinutes(GRACE_MINUTES);
            long exp = ToUnix(windowClose);
            long iat = ToUnix(DateTime.UtcNow);

            var payload = new QrTokenPayload
            {
                Sid = sessionId,
                Oid = subjectOfferingId,
                Date = sessionDate.ToString("yyyy-MM-dd"),
                From = start.ToString(@"hh\:mm"),
                To = end.ToString(@"hh\:mm"),
                Exp = exp,
                Iat = iat,
                Nonce = Guid.NewGuid().ToString("N"),
            };

            string json = JsonSerializer.Serialize(payload);
            string b64Body = ToBase64Url(Encoding.UTF8.GetBytes(json));
            string sig = ToBase64Url(Sign(b64Body));

            return $"{b64Body}.{sig}";
        }

        /// <summary>
        /// Validates a QR token string.
        /// Call this on the student side when the QR is scanned.
        /// Does NOT check for duplicate records — that check requires DB access
        /// and is done separately in QrAttendanceService.
        public static QrValidationOutcome Validate(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return QrValidationOutcome.Fail(QrValidationResult.MalformedToken,
                    "Empty token.");

            var parts = token.Split('.');
            if (parts.Length != 2)
                return QrValidationOutcome.Fail(QrValidationResult.MalformedToken,
                    "Token must have exactly two dot-separated parts.");

            // 1. Signature check (tamper-proof)
            byte[] expectedSig = Sign(parts[0]);
            byte[] actualSig;
            try { actualSig = FromBase64Url(parts[1]); }
            catch
            {
                return QrValidationOutcome.Fail(QrValidationResult.InvalidSignature,
                        "Signature is not valid base64.");
            }

            if (!CryptographicOperations.FixedTimeEquals(expectedSig, actualSig))
                return QrValidationOutcome.Fail(QrValidationResult.InvalidSignature,
                    "QR code signature is invalid or the code has been tampered with.");

            // 2. Deserialize payload
            QrTokenPayload payload;
            try
            {
                byte[] jsonBytes = FromBase64Url(parts[0]);
                string json = Encoding.UTF8.GetString(jsonBytes);
                payload = JsonSerializer.Deserialize<QrTokenPayload>(json)
                          ?? throw new Exception("null payload");
            }
            catch
            {
                return QrValidationOutcome.Fail(QrValidationResult.MalformedToken,
                    "Could not parse QR payload.");
            }

            // 3. Hard expiry (exp field)
            var now = DateTime.UtcNow;
            if (now > FromUnix(payload.Exp))
                return QrValidationOutcome.Fail(QrValidationResult.Expired,
                    $"QR code expired at {FromUnix(payload.Exp):HH:mm} UTC.");

            // 4. Session date must match today (local)
            var localNow = DateTime.Now;
            if (!DateTime.TryParse(payload.Date, out DateTime sessionDate)
                || sessionDate.Date != localNow.Date)
                return QrValidationOutcome.Fail(QrValidationResult.SessionDateMismatch,
                    $"This QR code is for {payload.Date}, not today ({localNow:yyyy-MM-dd}).");

            // 5. Attendance window (From .. To + grace already baked into exp)
            if (TimeSpan.TryParse(payload.From, out var windowOpen)
                && localNow.TimeOfDay < windowOpen)
                return QrValidationOutcome.Fail(QrValidationResult.OutsideAttendanceWindow,
                    $"Attendance window does not open until {payload.From}.");

            return QrValidationOutcome.Ok(payload);
        }

        private static byte[] Sign(string data)
        {
            using var hmac = new HMACSHA256(_signingKey);
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        private static byte[] DeriveKey(string passphrase)
        {
            using var sha = SHA256.Create();
            return sha.ComputeHash(Encoding.UTF8.GetBytes(passphrase));
        }

        private static long ToUnix(DateTime dt)
            => ((DateTimeOffset)dt.ToUniversalTime()).ToUnixTimeSeconds();

        private static DateTime FromUnix(long unix)
            => DateTimeOffset.FromUnixTimeSeconds(unix).UtcDateTime;

        private static string ToBase64Url(byte[] data)
            => Convert.ToBase64String(data)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');

        private static byte[] FromBase64Url(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            switch (s.Length % 4)
            {
                case 2: s += "=="; break;
                case 3: s += "="; break;
            }
            return Convert.FromBase64String(s);
        }
    }
}