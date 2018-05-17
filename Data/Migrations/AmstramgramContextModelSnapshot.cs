﻿// <auto-generated />
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Data.Migrations
{
    [DbContext(typeof(AmstramgramContext))]
    partial class AmstramgramContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Core.Models.Comment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedAt");

                    b.Property<long>("PictureId");

                    b.Property<string>("Text");

                    b.Property<string>("UpdatedAt");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PictureId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Core.Models.Like", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedAt");

                    b.Property<long>("PictureId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PictureId");

                    b.HasIndex("UserId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("Core.Models.Picture", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Color");

                    b.Property<string>("CreatedAt");

                    b.Property<string>("Description");

                    b.Property<string>("Image");

                    b.Property<string>("UpdatedAt");

                    b.Property<long>("UserId");

                    b.Property<string>("objectID");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("Core.Models.Tag", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("PictureId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("PictureId");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("Core.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<string>("Firstname");

                    b.Property<string>("Gender");

                    b.Property<string>("Lastname");

                    b.Property<string>("Nickname");

                    b.Property<string>("Password");

                    b.Property<string>("Phone");

                    b.Property<string>("Picture");

                    b.Property<bool>("Private");

                    b.Property<string>("objectID");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Core.Models.UserFollower", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<long>("FollowerId");

                    b.Property<long>("Id");

                    b.HasKey("UserId", "FollowerId");

                    b.HasIndex("FollowerId");

                    b.ToTable("Followers");
                });

            modelBuilder.Entity("Core.Models.Comment", b =>
                {
                    b.HasOne("Core.Models.Picture", "Picture")
                        .WithMany("Comments")
                        .HasForeignKey("PictureId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Core.Models.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Core.Models.Like", b =>
                {
                    b.HasOne("Core.Models.Picture", "Picture")
                        .WithMany("Likes")
                        .HasForeignKey("PictureId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Core.Models.User", "User")
                        .WithMany("Likes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Core.Models.Picture", b =>
                {
                    b.HasOne("Core.Models.User", "User")
                        .WithMany("Pictures")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Core.Models.Tag", b =>
                {
                    b.HasOne("Core.Models.Picture", "Picture")
                        .WithMany("Tags")
                        .HasForeignKey("PictureId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Core.Models.UserFollower", b =>
                {
                    b.HasOne("Core.Models.User", "Follower")
                        .WithMany("Following")
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Core.Models.User", "User")
                        .WithMany("Followers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
