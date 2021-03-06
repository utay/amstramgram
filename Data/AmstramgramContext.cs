﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Data
{
    public class AmstramgramContext : IdentityDbContext<ApplicationUser>
    {
        public readonly ILogger _logger;
        private bool _migrations;

        public AmstramgramContext()
        {
            _migrations = true;
        }

        public AmstramgramContext(DbContextOptions options, ILogger<AmstramgramContext> logger)
            : base(options)
        {
            _logger = logger;
            //Database.EnsureCreated();
            //Database.Migrate();
        }

        public AmstramgramContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_migrations)
            {
                optionsBuilder.UseSqlServer("Server=sqlserver;Database=master;User=sa;Password=Strong(!)Password;");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // https://docs.microsoft.com/en-us/ef/core/modeling/relationships
            // http://stackoverflow.com/questions/38520695/multiple-relationships-to-the-same-table-in-ef7core

            base.OnModelCreating(modelBuilder);

            // users
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>()
                .HasMany(u => u.Pictures)
                .WithOne(p => p.User);
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            // pictures
            modelBuilder.Entity<Picture>().HasKey(p => p.Id);
            modelBuilder.Entity<Picture>().Property(p => p.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Picture>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Picture);

            modelBuilder.Entity<Picture>()
                .HasMany(p => p.Likes)
                .WithOne(l => l.Picture);

            modelBuilder.Entity<Picture>()
                .HasOne(p => p.User)
                .WithMany(u => u.Pictures)
                .OnDelete(DeleteBehavior.Cascade);

            // likes
            modelBuilder.Entity<Like>().HasKey(l => l.Id);
            modelBuilder.Entity<Like>().Property(c => c.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Picture)
                .WithMany(p => p.Likes)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .OnDelete(DeleteBehavior.Restrict);

            // comments
            modelBuilder.Entity<Comment>().HasKey(c => c.Id);
            modelBuilder.Entity<Comment>().Property(c => c.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Picture)
                .WithMany(p => p.Comments)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .OnDelete(DeleteBehavior.Restrict);

            // user-followers
            modelBuilder.Entity<UserFollower>().HasKey(c => c.Id);
            modelBuilder.Entity<UserFollower>().Property(c => c.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<UserFollower>()
                .HasOne(f => f.User)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserFollower>()
                .HasOne(f => f.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Cascade);

            // tags
            modelBuilder.Entity<Tag>().HasKey(t => t.Id);
            modelBuilder.Entity<Tag>().Property(t => t.Id).ValueGeneratedOnAdd();
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Picture> Pictures { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<UserFollower> Followers { get; set; }
    }
}
