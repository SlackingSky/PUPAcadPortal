using Microsoft.EntityFrameworkCore;

namespace PUPAcadPortal.Models;

public partial class AppDbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        //  Activity: ActivityType default + IsPublished flag
        //  IMPORTANT: ValueGeneratedNever() tells EF Core that C# always owns
        //  these values.  Without it, EF omits columns whose value matches the
        //  DB default from the INSERT statement, causing the
        //  "Could not save changes. Please configure your entity type accordingly."
        //  error when HasDefaultValue/HasDefaultValueSql is also present.
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.Property(e => e.ActivityType)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Assignment'")
                .HasColumnName("ActivityType")
                .ValueGeneratedNever();     // ← always send the C# value

            entity.Property(e => e.IsPublished)
                .HasDefaultValue(false)
                .HasColumnName("IsPublished")
                .ValueGeneratedNever();     // ← always send the C# value
        });

        modelBuilder.Entity<Submission>(entity =>
        {
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Submitted'")
                .HasColumnName("Status")
                .ValueGeneratedNever();     // ← always send the C# value

            entity.Property(e => e.Remarks)
                .HasMaxLength(500)          // was 20 — corrected to 500
                .HasColumnName("Remarks");
        });

        modelBuilder.Entity<QrSession>(entity =>
        {
            entity.HasKey(e => e.QrSessionId).HasName("PRIMARY");

            entity.ToTable("QrSession");

            entity.HasIndex(e => e.SessionId, "FK_QrSession_ClassSession");

            entity.Property(e => e.QrSessionId)
                .HasColumnName("QrSessionID");

            entity.Property(e => e.SessionId)
                .HasColumnName("SessionID");

            entity.Property(e => e.Token)
                .HasMaxLength(500);

            entity.Property(e => e.GeneratedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");

            entity.Property(e => e.ExpiresAt)
                .HasColumnType("datetime");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");

            entity.HasOne(d => d.Session)
                .WithMany()
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_QrSession_ClassSession");
        });
    }
}