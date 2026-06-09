using System;
using System.Collections.Generic;
using System.Linq;
using PUPAcadPortal.Models;

namespace PUPAcadPortal.Services
{
    public static class QrSessionService
    {
        private static AppDbContext CreateContext() => new AppDbContext();

        // CREATE

        public static QrSession CreateOrGetActive(
            int sessionId,
            string subjectOfferingId,
            DateTime sessionDate,
            TimeSpan? startTime,
            TimeSpan? endTime,
            int expiryMinutes,
            out bool alreadyActive)
        {
            if (sessionId <= 0)
                throw new ArgumentOutOfRangeException(nameof(sessionId));
            if (string.IsNullOrWhiteSpace(subjectOfferingId))
                throw new ArgumentNullException(nameof(subjectOfferingId));
            if (expiryMinutes < 1)
                throw new ArgumentOutOfRangeException(nameof(expiryMinutes), "Expiry must be at least 1 minute.");

            using var ctx = CreateContext();
            var now = DateTime.UtcNow;

            // Auto-expire stale rows
            // DEBUG: disabled so we can observe full token behaviour
            // AutoExpireStale(ctx, sessionId, now);

            // Return the still-valid active token if one exists
            // DEBUG: ExpiresAt > now removed — returns any active token even if expired
            var existing = ctx.QrSessions
                .Where(q => q.SessionId == sessionId
                         && q.IsActive == true
                         /* && q.ExpiresAt > now */)
                .OrderByDescending(q => q.GeneratedAt)
                .FirstOrDefault();

            if (existing != null)
            {
                alreadyActive = true;
                return existing;
            }

            // Deactivate any remaining active rows (edge case: IsActive=true but ExpiresAt <= now already handled above)
            DeactivateAll(ctx, sessionId, now);

            // Step 4: Build a real signed token
            string signedToken = QrTokenService.Build(
                sessionId,
                subjectOfferingId,
                sessionDate,
                startTime,
                endTime);

            var qr = new QrSession
            {
                SessionId = sessionId,
                Token = signedToken,
                GeneratedAt = now,
                ExpiresAt = now.AddMinutes(expiryMinutes),
                IsActive = true,
            };

            ctx.QrSessions.Add(qr);
            ctx.SaveChanges();

            alreadyActive = false;
            return qr;
        }

        public static QrSession Create(
            int sessionId,
            string courseCode,
            string courseName,
            string period,
            string sessionLabel,
            int expiryMinutes = 10)
        {
            using var ctx = CreateContext();
            var now = DateTime.UtcNow;

            AutoExpireStale(ctx, sessionId, now);
            DeactivateAll(ctx, sessionId, now);

            string payload = $"{courseCode}|{courseName}|{period}|{sessionLabel}|{Guid.NewGuid():N}";

            var qr = new QrSession
            {
                SessionId = sessionId,
                Token = payload,
                GeneratedAt = now,
                ExpiresAt = now.AddMinutes(expiryMinutes),
                IsActive = true,
            };

            ctx.QrSessions.Add(qr);
            ctx.SaveChanges();
            return qr;
        }

        // READ

        public static QrSession? GetActive(int sessionId)
        {
            using var ctx = CreateContext();
            var now = DateTime.UtcNow;
            // DEBUG: AutoExpireStale disabled so expired sessions are still returned
            // AutoExpireStale(ctx, sessionId, now);

            return ctx.QrSessions
                .Where(q => q.SessionId == sessionId
                         && q.IsActive == true
                         /* DEBUG: ExpiresAt > now check removed to bypass expiry */
                         /* && q.ExpiresAt > now */)
                .OrderByDescending(q => q.GeneratedAt)
                .FirstOrDefault();
        }

        /// <summary>Returns all QrSession rows for a ClassSession, newest first.</summary>
        public static List<QrSession> GetHistory(int sessionId)
        {
            using var ctx = CreateContext();
            return ctx.QrSessions
                .Where(q => q.SessionId == sessionId)
                .OrderByDescending(q => q.GeneratedAt)
                .ToList();
        }

        /// <summary>
        /// Returns true when the given ClassSession has an active, unexpired token.
        /// </summary>
        public static bool HasActiveToken(int sessionId)
            => GetActive(sessionId) != null;

        /// <summary>
        /// Returns the remaining lifetime in seconds of the active token,
        /// or 0 if no active token exists.
        /// </summary>
        public static double GetActiveRemainingSeconds(int sessionId)
        {
            var qr = GetActive(sessionId);
            if (qr == null) return 0;
            return Math.Max(0, (qr.ExpiresAt - DateTime.UtcNow).TotalSeconds);
        }
        // UPDATE

        public static void MarkExpired(int qrSessionId)
        {
            using var ctx = CreateContext();
            var qr = ctx.QrSessions.Find(qrSessionId);
            if (qr == null) return;

            qr.IsActive = false;
            qr.ExpiresAt = DateTime.UtcNow;
            ctx.SaveChanges();
        }

        /// <summary>Soft-expires ALL active tokens for a ClassSession.</summary>
        public static void DeactivateForSession(int sessionId)
        {
            using var ctx = CreateContext();
            var now = DateTime.UtcNow;
            DeactivateAll(ctx, sessionId, now);
            ctx.SaveChanges();
        }

        /// <summary>
        /// Scans ALL sessions and auto-expires rows whose ExpiresAt has passed.
        /// Intended for a periodic background job (e.g. called on portal startup).
        /// Returns the number of rows updated.
        /// </summary>
        public static int ExpireAllStale()
        {
            using var ctx = CreateContext();
            var now = DateTime.UtcNow;
            var stale = ctx.QrSessions
                .Where(q => q.IsActive == true && q.ExpiresAt <= now)
                .ToList();

            foreach (var q in stale)
                q.IsActive = false;

            ctx.SaveChanges();
            return stale.Count;
        }

        // DELETE

        public static int PurgeExpired()
        {
            using var ctx = CreateContext();
            var now = DateTime.UtcNow;
            var rows = ctx.QrSessions
                .Where(q => q.IsActive == false && q.ExpiresAt <= now)
                .ToList();

            if (rows.Count == 0) return 0;
            ctx.QrSessions.RemoveRange(rows);
            ctx.SaveChanges();
            return rows.Count;
        }

        public static void DeleteForSession(int sessionId)
        {
            using var ctx = CreateContext();
            var rows = ctx.QrSessions
                .Where(q => q.SessionId == sessionId)
                .ToList();
            if (rows.Count == 0) return;
            ctx.QrSessions.RemoveRange(rows);
            ctx.SaveChanges();
        }

        private static void AutoExpireStale(AppDbContext ctx, int sessionId, DateTime now)
        {
            var stale = ctx.QrSessions
                .Where(q => q.SessionId == sessionId
                         && q.IsActive == true
                         && q.ExpiresAt <= now)
                .ToList();

            foreach (var q in stale)
                q.IsActive = false;

            if (stale.Count > 0)
                ctx.SaveChanges();
        }
        private static void DeactivateAll(AppDbContext ctx, int sessionId, DateTime now)
        {
            var active = ctx.QrSessions
                .Where(q => q.SessionId == sessionId && q.IsActive == true)
                .ToList();

            foreach (var q in active)
            {
                q.IsActive = false;
                q.ExpiresAt = now;
            }
        }
    }
}