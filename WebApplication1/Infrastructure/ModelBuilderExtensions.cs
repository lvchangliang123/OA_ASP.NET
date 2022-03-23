using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Infrastructure
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(new Student {
                Id = 1,
                Name = "德莱厄斯",
                Major = Models.EnumTypes.MajorEnum.ComputerScience,
                Email = "123456789@163.com",
                PhotoPath="~/Imgs/nuoshou.jpeg"
            });
        }
    }
}
