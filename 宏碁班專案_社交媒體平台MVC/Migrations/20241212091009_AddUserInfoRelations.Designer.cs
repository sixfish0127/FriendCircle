﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using 宏碁班專案_社交媒體平台MVC.Models;

#nullable disable

namespace 宏碁班專案_社交媒體平台MVC.Migrations
{
    [DbContext(typeof(FriendCircleContext))]
    [Migration("20241212091009_AddUserInfoRelations")]
    partial class AddUserInfoRelations
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("宏碁班專案_社交媒體平台MVC.Models.CommentFavorites", b =>
                {
                    b.Property<int>("CommentId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.HasKey("CommentId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("CommentFavorites");
                });

            modelBuilder.Entity("宏碁班專案_社交媒體平台MVC.Models.CommentShares", b =>
                {
                    b.Property<int>("CommentId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.HasKey("CommentId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("CommentShares");
                });

            modelBuilder.Entity("宏碁班專案_社交媒體平台MVC.Models.Comments", b =>
                {
                    b.Property<int>("ComentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ComentId"));

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int?>("ParenCommentId")
                        .HasColumnType("int");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ComentId");

                    b.HasIndex("ParenCommentId");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("宏碁班專案_社交媒體平台MVC.Models.Posts", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex(new[] { "PostId" }, "IX_Posts")
                        .IsUnique();

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("宏碁班專案_社交媒體平台MVC.Models.ReactionType", b =>
                {
                    b.Property<int>("CommentId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int?>("ReactionType1")
                        .HasColumnType("int")
                        .HasColumnName("ReactionType");

                    b.HasKey("CommentId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("ReactionType");
                });

            modelBuilder.Entity("宏碁班專案_社交媒體平台MVC.Models.userInfo", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("address")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("birthday")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime?>("initDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("phone")
                        .HasColumnType("int");

                    b.Property<string>("userimage")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("id")
                        .HasName("PK_account");

                    b.HasIndex(new[] { "email" }, "IX_account")
                        .IsUnique();

                    b.ToTable("userInfo");
                });

            modelBuilder.Entity("宏碁班專案_社交媒體平台MVC.Models.CommentFavorites", b =>
                {
                    b.HasOne("宏碁班專案_社交媒體平台MVC.Models.Comments", "Comment")
                        .WithMany("CommentFavorites")
                        .HasForeignKey("CommentId")
                        .IsRequired()
                        .HasConstraintName("FK_CommentFavorites_Comments");

                    b.HasOne("宏碁班專案_社交媒體平台MVC.Models.userInfo", "User")
                        .WithMany("CommentFavorites")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_CommentFavorites_userInfo");

                    b.Navigation("Comment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("宏碁班專案_社交媒體平台MVC.Models.CommentShares", b =>
                {
                    b.HasOne("宏碁班專案_社交媒體平台MVC.Models.Comments", "Comment")
                        .WithMany("CommentShares")
                        .HasForeignKey("CommentId")
                        .IsRequired()
                        .HasConstraintName("FK_CommentShares_Comments");

                    b.HasOne("宏碁班專案_社交媒體平台MVC.Models.userInfo", "User")
                        .WithMany("CommentShares")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_CommentShares_userInfo");

                    b.Navigation("Comment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("宏碁班專案_社交媒體平台MVC.Models.Comments", b =>
                {
                    b.HasOne("宏碁班專案_社交媒體平台MVC.Models.Comments", "ParenComment")
                        .WithMany("InverseParenComment")
                        .HasForeignKey("ParenCommentId")
                        .HasConstraintName("FK_Comments_Comments");

                    b.HasOne("宏碁班專案_社交媒體平台MVC.Models.Posts", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .HasPrincipalKey("PostId")
                        .IsRequired()
                        .HasConstraintName("FK_Comments_Posts");

                    b.HasOne("宏碁班專案_社交媒體平台MVC.Models.userInfo", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_Comments_userInfo");

                    b.Navigation("ParenComment");

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("宏碁班專案_社交媒體平台MVC.Models.Posts", b =>
                {
                    b.HasOne("宏碁班專案_社交媒體平台MVC.Models.userInfo", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_Posts_userInfo");

                    b.Navigation("User");
                });

            modelBuilder.Entity("宏碁班專案_社交媒體平台MVC.Models.ReactionType", b =>
                {
                    b.HasOne("宏碁班專案_社交媒體平台MVC.Models.Comments", "Comment")
                        .WithMany("ReactionType")
                        .HasForeignKey("CommentId")
                        .IsRequired()
                        .HasConstraintName("FK_ReactionType_Comments");

                    b.HasOne("宏碁班專案_社交媒體平台MVC.Models.userInfo", "User")
                        .WithMany("ReactionType")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_ReactionType_userInfo");

                    b.Navigation("Comment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("宏碁班專案_社交媒體平台MVC.Models.Comments", b =>
                {
                    b.Navigation("CommentFavorites");

                    b.Navigation("CommentShares");

                    b.Navigation("InverseParenComment");

                    b.Navigation("ReactionType");
                });

            modelBuilder.Entity("宏碁班專案_社交媒體平台MVC.Models.Posts", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("宏碁班專案_社交媒體平台MVC.Models.userInfo", b =>
                {
                    b.Navigation("CommentFavorites");

                    b.Navigation("CommentShares");

                    b.Navigation("Comments");

                    b.Navigation("Posts");

                    b.Navigation("ReactionType");
                });
#pragma warning restore 612, 618
        }
    }
}
