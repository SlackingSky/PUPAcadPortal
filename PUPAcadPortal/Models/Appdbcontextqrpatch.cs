using Microsoft.EntityFrameworkCore;

namespace PUPAcadPortal.Models;
public partial class AppDbContext
{
    //  DbSet 
    public virtual DbSet<QrSession> QrSessions { get; set; }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<QrSession>(entity =>
        {
            entity.HasKey(e => e.QrSessionId).HasName("PRIMARY");

            entity.ToTable("QrSession");

            entity.HasIndex(e => e.SessionId, "FK_QrSession_ClassSession");

            entity.Property(e => e.QrSessionId).HasColumnName("QrSessionID");
            entity.Property(e => e.SessionId).HasColumnName("SessionID");
            entity.Property(e => e.Token).HasMaxLength(500);
            entity.Property(e => e.GeneratedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpiresAt).HasColumnType("datetime");
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