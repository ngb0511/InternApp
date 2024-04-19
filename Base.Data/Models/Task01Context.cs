using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Base.Data.Models;

public partial class Task01Context : DbContext
{
    public Task01Context()
    {
    }

    public Task01Context(DbContextOptions<Task01Context> options)
        : base(options)
    {
    }

    public virtual DbSet<DummyCode> DummyCodes { get; set; }

    public virtual DbSet<Forecast> Forecasts { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<MaterialMaster> MaterialMasters { get; set; }

    public virtual DbSet<TimingPost> TimingPosts { get; set; }

    public virtual DbSet<UserAssign> UserAssigns { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=118.69.224.60, 1435;Database=task01;User ID=sa;Password=Librasoft@123;TrustServerCertificate=True");

    //=> optionsBuilder.UseSqlServer("Server=MSI;Database=task01;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DummyCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DummyCod__3214EC07F74955B8");

            entity.ToTable("DummyCode");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DummyCodes)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DummyCode__Creat__412EB0B6");
        });

        modelBuilder.Entity<Forecast>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Forecast__3214EC07F1317DD1");

            entity.ToTable("Forecast");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.PostEnd).HasColumnType("date");
            entity.Property(e => e.PostStart).HasColumnType("date");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Forecasts)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Forecast__Create__398D8EEE");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Log__3214EC078D5F57ED");

            entity.ToTable("Log");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Logs)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Log__CreatedBy__440B1D61");
        });

        modelBuilder.Entity<MaterialMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Material__3214EC07E7545363");

            entity.ToTable("MaterialMaster");
        });

        modelBuilder.Entity<TimingPost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TimingPo__3214EC0750D9494A");

            entity.ToTable("TimingPost");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.PostEnd).HasColumnType("date");
            entity.Property(e => e.PostStart).HasColumnType("date");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TimingPosts)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TimingPos__Creat__3C69FB99");
        });

        modelBuilder.Entity<UserAssign>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserAssi__3214EC07015F551F");

            entity.ToTable("UserAssign");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
