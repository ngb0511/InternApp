﻿using System;
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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=118.69.224.60, 1435;Database=task01;User ID=sa;Password=Librasoft@123;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DummyCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DummyCod__3214EC0756AD492A");

            entity.ToTable("DummyCode");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DummyCodes)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DummyCode__Creat__66603565");
        });

        modelBuilder.Entity<Forecast>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Forecast__3214EC078D39CBEE");

            entity.ToTable("Forecast");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.PostEnd).HasColumnType("date");
            entity.Property(e => e.PostStart).HasColumnType("date");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Forecasts)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Forecast__Create__5EBF139D");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Log__3214EC078063A152");

            entity.ToTable("Log");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Logs)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Log__CreatedBy__693CA210");
        });

        modelBuilder.Entity<MaterialMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Material__3214EC070077FC86");

            entity.ToTable("MaterialMaster");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<TimingPost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TimingPo__3214EC07379EB468");

            entity.ToTable("TimingPost");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.PostEnd).HasColumnType("date");
            entity.Property(e => e.PostStart).HasColumnType("date");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TimingPosts)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TimingPos__Creat__619B8048");
        });

        modelBuilder.Entity<UserAssign>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserAssi__3214EC0777F1CB85");

            entity.ToTable("UserAssign");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
