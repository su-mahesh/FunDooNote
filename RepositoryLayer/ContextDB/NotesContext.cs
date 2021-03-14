using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RepositoryLayer.Models;

#nullable disable

namespace RepositoryLayer.ContextDB
{
    public partial class NotesContext : DbContext
    {
        public NotesContext()
        {
        }

        public NotesContext(DbContextOptions<NotesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Collaborator> Collaborators { get; set; }
        public virtual DbSet<Label> Labels { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<NoteLabel> NoteLabels { get; set; }
        public virtual DbSet<UserAccount> UserAccounts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Collaborator>(entity =>
            {
                entity.Property(e => e.CollaboratorId).HasColumnName("CollaboratorID");

                entity.Property(e => e.CollaboratorEmail)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.NoteId).HasColumnName("NoteID").IsRequired();

                entity.Property(e => e.UserId).HasColumnName("UserID").IsRequired();

                entity.HasOne(d => d.Note)
                    .WithMany(p => p.Collaborators)
                    .HasForeignKey(d => d.NoteId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("CollaboratorsFK_NoteID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Collaborators)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("CollaboratorsFK_UserID");
            });

            modelBuilder.Entity<Label>(entity =>
            {
                entity.Property(e => e.LabelId).HasColumnName("LabelID").IsRequired(); ;

                entity.Property(e => e.LabelName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Labels)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("LabelsFK_UserID");
            });

            modelBuilder.Entity<Note>(entity =>
            {
                entity.Property(e => e.NoteId).HasColumnName("NoteID");

                entity.Property(e => e.BackgroundColor).HasMaxLength(10);

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ReminderOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Text).IsRequired();

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("NotesFK_UserID");
            });

            modelBuilder.Entity<NoteLabel>(entity =>
            {
                entity.ToTable("NoteLabel");

                entity.Property(e => e.NoteLabelId).HasColumnName("NoteLabelID");

                entity.Property(e => e.LabelId).HasColumnName("LabelID").IsRequired();

                entity.Property(e => e.NoteId).HasColumnName("NoteID").IsRequired();

                entity.Property(e => e.UserId).HasColumnName("UserID").IsRequired();

                entity.HasOne(d => d.Label)
                    .WithMany(p => p.NoteLabels)
                    .HasForeignKey(d => d.LabelId)
                    .HasConstraintName("NoteLabelFK_LabelID");

                entity.HasOne(d => d.Note)
                    .WithMany(p => p.NoteLabels)
                    .HasForeignKey(d => d.NoteId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("NoteLabelFK_NoteID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.NoteLabels)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("NoteLabelFK_UserID");
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("UserAccountPK");

                entity.ToTable("UserAccount");

                entity.HasIndex(e => e.Email, "UserAccountUN_Email")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
