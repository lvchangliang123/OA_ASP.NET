using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VueNetBlog.Server.Models
{
    public partial class BlogDbContext : IdentityDbContext<User>
    {
        public BlogDbContext()
        {

        }

        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<AutherInfo> AutherInfos { get; set; } = null!;
        public virtual DbSet<Blog> Blogs { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Daily> Dailies { get; set; } = null!;
        public virtual DbSet<SourceCode> SourceCodes { get; set; } = null!;
        public new virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL("Data Source=localhost;Initial Catalog=VueNetBlogDB_Test;User=root;Password=123456");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AutherInfo>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.AutherInfos)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_AutherInfo_UserId");
            });

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Blog_UserId");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.BlogId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Comment_BlogId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Comment_UserId");
            });

            modelBuilder.Entity<Daily>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Dailies)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Daily_UserId");
            });

            modelBuilder.Entity<SourceCode>(entity =>
            {
                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.SourceCodes)
                    .HasForeignKey(d => d.BlogId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_SourceCode_BlogId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
