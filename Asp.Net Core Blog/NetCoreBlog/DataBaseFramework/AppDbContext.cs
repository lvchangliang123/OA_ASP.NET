using BlogModels.Dtos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseFramework
{
    public class AppDbContext : IdentityDbContext<Models.CustomerIdentityUser>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        //数据集合
        public DbSet<BlogInfoDto> Blogs { get; set; }
        public DbSet<BlogCommentDto> BlogsComments { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //修改数据库表名
            builder.Entity<BlogInfoDto>().ToTable("Blog");
            builder.Entity<BlogCommentDto>().ToTable("BlogComment");
        }
    }
}
