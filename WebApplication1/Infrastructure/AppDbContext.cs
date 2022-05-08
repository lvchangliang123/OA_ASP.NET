using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Infrastructure
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //重写方法，创建种子数据
            modelBuilder.Seed();

            //手动修改生成的数据库表名
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<StudentCourse>().ToTable("StudentCourse");
            modelBuilder.Entity<Student>().ToTable("Student");

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

    }
}
