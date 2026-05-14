using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PUPAcadPortal.Models;

public partial class DefaultdbContext : DbContext
{
    public DefaultdbContext()
    {
    }

    public DefaultdbContext(DbContextOptions<DefaultdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("Server=mysql-pupacadportal-pupacadportal.h.aivencloud.com;Port=15204;Database=defaultdb;Uid=pupacadportalapp;Pwd=pupacadportalapp;SslMode=Required;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("A table for the portal's users"));

            entity.Property(e => e.Username).HasMaxLength(100);
            entity.Property(e => e.DisplayName).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(60);
            entity.Property(e => e.Role).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
