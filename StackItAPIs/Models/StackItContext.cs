using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StackItAPIs.Models;

public partial class StackItContext : DbContext
{
    public StackItContext()
    {
    }

    public StackItContext(DbContextOptions<StackItContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionTag> QuestionTags { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vote> Votes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Answers__3213E83F963719A9");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.IsAccepted)
                .HasDefaultValue(false)
                .HasColumnName("isAccepted");
            entity.Property(e => e.QuestionId).HasColumnName("questionId");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.VoteScore)
                .HasDefaultValue(0)
                .HasColumnName("voteScore");

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_Answers_Question");

            entity.HasOne(d => d.User).WithMany(p => p.Answers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Answers_User");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comments__3213E83F43B15D69");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CommentableId).HasColumnName("commentableId");
            entity.Property(e => e.CommentableType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("commentableType");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.VoteScore)
                .HasDefaultValue(0)
                .HasColumnName("voteScore");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Comments_User");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3213E83F1A258BE4");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.IsRead)
                .HasDefaultValue(false)
                .HasColumnName("isRead");
            entity.Property(e => e.Message)
                .HasColumnType("text")
                .HasColumnName("message");
            entity.Property(e => e.RelatedId).HasColumnName("relatedId");
            entity.Property(e => e.RelatedType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("relatedType");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("type");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Notifications_User");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3213E83F787A2C11");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AcceptedAnswerId).HasColumnName("acceptedAnswerId");
            entity.Property(e => e.AnswerCount)
                .HasDefaultValue(0)
                .HasColumnName("answerCount");
            entity.Property(e => e.ClosedReason)
                .HasColumnType("text")
                .HasColumnName("closedReason");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.IsClosed)
                .HasDefaultValue(false)
                .HasColumnName("isClosed");
            entity.Property(e => e.Title)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.ViewCount)
                .HasDefaultValue(0)
                .HasColumnName("viewCount");
            entity.Property(e => e.VoteScore)
                .HasDefaultValue(0)
                .HasColumnName("voteScore");

            entity.HasOne(d => d.User).WithMany(p => p.Questions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Questions_User");
        });

        modelBuilder.Entity<QuestionTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3213E83F7E2B3F5F");

            entity.HasIndex(e => new { e.QuestionId, e.TagId }, "uq_QuestionTag").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.QuestionId).HasColumnName("questionId");
            entity.Property(e => e.TagId).HasColumnName("tagId");

            entity.HasOne(d => d.Question).WithMany(p => p.QuestionTags)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_QuestionTags_Question");

            entity.HasOne(d => d.Tag).WithMany(p => p.QuestionTags)
                .HasForeignKey(d => d.TagId)
                .HasConstraintName("FK_QuestionTags_Tag");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tags__3213E83FE534503B");

            entity.HasIndex(e => e.Name, "UQ__Tags__72E12F1B067A826E").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Color)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasDefaultValue("#007bff")
                .HasColumnName("color");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.UsageCount)
                .HasDefaultValue(0)
                .HasColumnName("usageCount");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3213E83FAA04EE11");

            entity.HasIndex(e => e.Email, "UQ__Users__AB6E61648623CA99").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__Users__F3DBC57209BE306F").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bio)
                .HasColumnType("text")
                .HasColumnName("bio");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("fullName");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsBanned)
                .HasDefaultValue(false)
                .HasColumnName("isBanned");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.ProfilePictureUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("profilePictureUrl");
            entity.Property(e => e.Reputation)
                .HasDefaultValue(0)
                .HasColumnName("reputation");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("user")
                .HasColumnName("role");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Vote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Votes__3213E83F7CDC18F3");

            entity.HasIndex(e => new { e.UserId, e.VotableType, e.VotableId }, "uq_Vote").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.VotableId).HasColumnName("votableId");
            entity.Property(e => e.VotableType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("votableType");
            entity.Property(e => e.VoteType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("voteType");

            entity.HasOne(d => d.User).WithMany(p => p.Votes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Votes_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
