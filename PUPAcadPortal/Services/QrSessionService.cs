using System;
using System.Collections.Generic;
using System.Linq;
using PUPAcadPortal.Models;
using MySql.EntityFrameworkCore;
using MySqlConnector;

namespace PUPAcadPortal.Services
{
    public static class QrSessionService
    {
        private static AppDbContext CreateContext() => new AppDbContext();

        // ══════════════════════════════════════════════════════════════════════
        // CREATE
        // ══════════════════════════════════════════════════════════════════════
        public static QrSession Create(
            int sessionId,
            string courseCode,
            string courseName,
            string period,
            string sessionLabel,
            int expiryMinutes = 10)
        {
            using var ctx = CreateContext();

            // Soft-deactivate old rows first (no separate SaveChanges needed —
            // they are tracked by the same context instance)
            DeactivateExisting(ctx, sessionId);

            string guid = Guid.NewGuid().ToString("N");
            string payload = $"{courseCode}|{courseName}|{period}|{sessionLabel}|{guid}";
            var now = DateTime.UtcNow;

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

        // ══════════════════════════════════════════════════════════════════════
        // READ
        // ══════════════════════════════════════════════════════════════════════
        /// <summary>
        /// Returns the active <see cref="QrSession"/> for the given ClassSession,
        /// or <c>null</c> if none exists.
        /// </summary>
        public static QrSession? GetActive(int sessionId)
        {
            using var ctx = CreateContext();
            return ctx.QrSessions
                .Where(q => q.SessionId == sessionId && q.IsActive == true)
                .OrderByDescending(q => q.GeneratedAt)
                .FirstOrDefault();
        }

        /// <summary>
        /// Returns all QrSession rows for a ClassSession, newest first.
        /// Useful for audit / history display.
        /// </summary>
        public static List<QrSession> GetHistory(int sessionId)
        {
            using var ctx = CreateContext();
            return ctx.QrSessions
                .Where(q => q.SessionId == sessionId)
                .OrderByDescending(q => q.GeneratedAt)
                .ToList();
        }

        // ══════════════════════════════════════════════════════════════════════
        // UPDATE
        // ══════════════════════════════════════════════════════════════════════
        /// <summary>
        /// Sets IsActive = false and stamps ExpiresAt = UTC now for a single row.
        /// </summary>
        public static void MarkExpired(int qrSessionId)
        {
            using var ctx = CreateContext();
            var qr = ctx.QrSessions.Find(qrSessionId);
            if (qr == null) return;

            qr.IsActive = false;
            qr.ExpiresAt = DateTime.UtcNow;
            ctx.SaveChanges();
        }

        /// <summary>
        /// Soft-expires ALL active tokens for a ClassSession.
        /// </summary>
        public static void DeactivateForSession(int sessionId)
        {
            using var ctx = CreateContext();
            DeactivateExisting(ctx, sessionId);
            ctx.SaveChanges();
        }

        // ══════════════════════════════════════════════════════════════════════
        // DELETE
        // ══════════════════════════════════════════════════════════════════════
        /// <summary>
        /// Hard-deletes every row where IsActive = false AND ExpiresAt &lt;= UTC now.
        /// Returns the number of rows removed.
        /// </summary>
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

        /// <summary>
        /// Hard-deletes ALL QrSession rows for a ClassSession regardless of state.
        /// </summary>
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

        // ══════════════════════════════════════════════════════════════════════
        // Private helpers
        // ══════════════════════════════════════════════════════════════════════
        private static void DeactivateExisting(AppDbContext ctx, int sessionId)
        {
            var active = ctx.QrSessions
                .Where(q => q.SessionId == sessionId && q.IsActive == true)
                .ToList();

            foreach (var q in active)
            {
                q.IsActive = false;
                q.ExpiresAt = DateTime.UtcNow;
            }
            // SaveChanges is the caller's responsibility.
        }
    }
}