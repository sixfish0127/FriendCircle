﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace 宏碁班專案_社交媒體平台MVC.Models;

public partial class FriendCircleContext : DbContext
{
    public FriendCircleContext(DbContextOptions<FriendCircleContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CommentFavorites> CommentFavorites { get; set; }

    public virtual DbSet<CommentShares> CommentShares { get; set; }

    public virtual DbSet<Comments> Comments { get; set; }

    public virtual DbSet<FriendShip> FriendShip { get; set; }

    public virtual DbSet<Notifications> Notifications { get; set; }

    public virtual DbSet<Posts> Posts { get; set; }

    public virtual DbSet<ReactionType> ReactionType { get; set; }

    public virtual DbSet<userInfo> userInfo { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CommentFavorites>(entity =>
        {
            entity.HasKey(e => new { e.CommentId, e.UserId });

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Comment).WithMany(p => p.CommentFavorites)
                .HasForeignKey(d => d.CommentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CommentFavorites_Comments");

            entity.HasOne(d => d.User).WithMany(p => p.CommentFavorites)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CommentFavorites_userInfo");
        });

        modelBuilder.Entity<CommentShares>(entity =>
        {
            entity.HasKey(e => new { e.CommentId, e.UserId });

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Comment).WithMany(p => p.CommentShares)
                .HasForeignKey(d => d.CommentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CommentShares_Comments");

            entity.HasOne(d => d.User).WithMany(p => p.CommentShares)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CommentShares_userInfo");
        });

        modelBuilder.Entity<Comments>(entity =>
        {
            entity.HasKey(e => e.ComentId);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.ParenComment).WithMany(p => p.InverseParenComment)
                .HasForeignKey(d => d.ParenCommentId)
                .HasConstraintName("FK_Comments_Comments");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comments_Posts");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comments_userInfo");
        });

        modelBuilder.Entity<FriendShip>(entity =>
        {
            entity.HasIndex(e => new { e.UserId1, e.UserId2 }, "IX_FriendShip");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.UserId1Navigation).WithMany(p => p.FriendShipUserId1Navigation)
                .HasForeignKey(d => d.UserId1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FriendShip_userInfo");

            entity.HasOne(d => d.UserId2Navigation).WithMany(p => p.FriendShipUserId2Navigation)
                .HasForeignKey(d => d.UserId2)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FriendShip_userInfo1");
        });

        modelBuilder.Entity<Notifications>(entity =>
        {
            entity.Property(e => e.CreatAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Message).IsRequired();

            entity.HasOne(d => d.Comment).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.CommentId)
                .HasConstraintName("FK_Notifications_Comments");

            entity.HasOne(d => d.FriendRequest).WithMany(p => p.NotificationsFriendRequest)
                .HasForeignKey(d => d.FriendRequestId)
                .HasConstraintName("FK_Notifications_userInfo1");

            entity.HasOne(d => d.User).WithMany(p => p.NotificationsUser)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notifications_userInfo");
        });

        modelBuilder.Entity<Posts>(entity =>
        {
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Posts_userInfo");
        });

        modelBuilder.Entity<ReactionType>(entity =>
        {
            entity.HasKey(e => new { e.PostId, e.UserId });

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReactionType1).HasColumnName("ReactionType");

            entity.HasOne(d => d.Post).WithMany(p => p.ReactionType)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReactionType_Posts");

            entity.HasOne(d => d.User).WithMany(p => p.ReactionType)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReactionType_userInfo");
        });

        modelBuilder.Entity<userInfo>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK_account");

            entity.HasIndex(e => e.email, "IX_account").IsUnique();

            entity.Property(e => e.address).HasMaxLength(50);
            entity.Property(e => e.birthday)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.email)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.initDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.name).HasMaxLength(50);
            entity.Property(e => e.password)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.phone).HasMaxLength(50);
            entity.Property(e => e.userimage)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("/images/default.jpg");
            entity.Property(e=>e.status).HasDefaultValue(UserStatus.離線);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}