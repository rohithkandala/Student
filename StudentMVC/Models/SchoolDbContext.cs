using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StudentMVC.Models;

public partial class SchoolDbContext : DbContext
{
    public SchoolDbContext()
    {
    }

    public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
        : base(options)
    {
    }

    public DbSet<Pager> pagers { get; set; }
    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Mark> Marks { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public DbSet<EnrolledStudentClass> enrolledStudentClasses { get; set; }

    public DbSet<NoOfStudentClass> NoOfStudentClasses { get; set; }

    public DbSet<PassFailCount> passFailCounts { get; set; }

    public DbSet<TeacherClass> TeacherClasses { get; set; }

    public DbSet<Scorecard> Scorecards { get; set; }

    public DbSet<DeletedStudents> DeletedStudents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("pk_CID");

            entity.ToTable("Class");

            entity.Property(e => e.ClassName)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Teacher).WithMany(p => p.Classes)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("fk_TID");
        });

        modelBuilder.Entity<Mark>(entity =>
        {
            entity.HasKey(e => e.MarksId).HasName("pk_MID");

            entity.Property(e => e.Deleted).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.Class).WithMany(p => p.Marks)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("fk_CID");

            entity.HasOne(d => d.Student).WithMany(p => p.Marks)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("fk_SID");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("pk_SID");

            entity.ToTable("Student");

            entity.Property(e => e.Deleted).HasDefaultValueSql("((0))");
            entity.Property(e => e.StudentName)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("pk_TID");

            entity.ToTable("Teacher");

            entity.Property(e => e.TeacherName)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
