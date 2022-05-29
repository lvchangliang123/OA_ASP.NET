using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Models;

namespace WebApplication1.Infrastructure.Data
{
    public static class DataInitializer
    {
        public static IApplicationBuilder UseDataInitializer(
           this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope()) {
                var dbcontext = scope.ServiceProvider.GetService<AppDbContext>();
                var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                #region 学生种子信息

                if(dbcontext.Students.Any()){
                    return builder;     //数据已经初始化
                }
                var students = new[] {
                    new Student{ Name="张三",Major=Models.EnumTypes.MajorEnum.ComputerScience,Email="zhangsan@163.com",EnrollmentDate=DateTime.Parse("2016-09-01")},
                      new Student{ Name="李四",Major=Models.EnumTypes.MajorEnum.Mathematics,Email="lisi@163.com",EnrollmentDate=DateTime.Parse("2017-09-01")},
                       new Student{ Name="王五",Major=Models.EnumTypes.MajorEnum.ElectronicCommerce,Email="wangwu@163.com",EnrollmentDate=DateTime.Parse("2018-09-01")},
                        new Student{ Name="赵六",Major=Models.EnumTypes.MajorEnum.ComputerScience,Email="zhaoliu@163.com",EnrollmentDate=DateTime.Parse("2019-09-01")},
                };
                foreach (var stu in students) {
                    dbcontext.Students.Add(stu);
                }
                dbcontext.SaveChanges();
                #endregion


                #region 教师种子数据

                var teachers = new[] {
                    new Teacher { Name = "张老师", HireDate = DateTime.Parse ("1995-03-11") },
                    new Teacher { Name = "王老师", HireDate = DateTime.Parse ("2003-03-11") },
                    new Teacher { Name = "李老师", HireDate = DateTime.Parse ("1990-03-11") },
                    new Teacher { Name = "赵老师", HireDate = DateTime.Parse ("1985-03-11") },
                    new Teacher { Name = "刘老师", HireDate = DateTime.Parse ("2003-03-11") },
                    new Teacher { Name = "胡老师", HireDate = DateTime.Parse ("2003-03-11") }
                };

                foreach (var i in teachers)
                    dbcontext.Teachers.Add(i);
                dbcontext.SaveChanges();

                #endregion 教师种子数据

                #region 学院种子数据

                var departments = new[] {
                    new Department { Name = "机械学院", Budget = 350000, StartDate = DateTime.Parse ("2017-09-01"), TeacherID = teachers.Single (i => i.Name == "张老师").Id },
                    new Department { Name = "人文学院", Budget = 100000, StartDate = DateTime.Parse ("2017-09-01"), TeacherID = teachers.Single (i => i.Name == "王老师").Id },
                    new Department { Name = "外国语学院", Budget = 350000, StartDate = DateTime.Parse ("2017-09-01"), TeacherID = teachers.Single (i => i.Name == "李老师").Id },
                    new Department { Name = "计算机学院", Budget = 100000, StartDate = DateTime.Parse ("2017-09-01"), TeacherID = teachers.Single (i => i.Name == "胡老师").Id },
                      new Department { Name = "生化学院", Budget = 250000, StartDate = DateTime.Parse ("2009-09-01"), TeacherID = teachers.Single (i => i.Name == "刘老师").Id }
                };

                foreach (var d in departments)
                    dbcontext.Departments.Add(d);
                dbcontext.SaveChanges();

                #endregion 学院种子数据

                #region 课程种子数据

                if (dbcontext.Courses.Any()) {
                    return builder;
                }
                var courses = new[] {
                                    new Course { CourseID = 1050, Title = "数学", Credits = 3,DepartmentID=departments.Single(s=>s.Name=="机械学院").DepartmentID },
                    new Course { CourseID = 4022, Title = "政治", Credits = 3,DepartmentID=departments.Single(s=>s.Name=="人文学院").DepartmentID },
                    new Course { CourseID = 4041, Title = "物理", Credits = 3,DepartmentID=departments.Single(s=>s.Name=="机械学院").DepartmentID },
                    new Course { CourseID = 1045, Title = "化学", Credits = 4,DepartmentID=departments.Single(s=>s.Name=="生化学院").DepartmentID },
                    new Course { CourseID = 3141, Title = "生物", Credits = 4,DepartmentID=departments.Single(s=>s.Name=="生化学院").DepartmentID},
                    new Course { CourseID = 2021, Title = "英语", Credits = 3,DepartmentID=departments.Single(s=>s.Name=="外国语学院").DepartmentID},
                    new Course { CourseID = 2042, Title = "历史", Credits = 4,DepartmentID=departments.Single(s=>s.Name=="人文学院").DepartmentID}
                };
                foreach (var course in courses) {
                    dbcontext.Courses.Add(course);
                }
                dbcontext.SaveChanges();

                #endregion 课程种子数据


                #region 办公室分配的种子数据

                var OfficeLocations = new[] {
                    new OfficeLocation { TeacherId = teachers.Single (i => i.Name == "王老师").Id, Location = "逸夫楼 17" },
                    new OfficeLocation { TeacherId = teachers.Single (i => i.Name == "刘老师").Id, Location = "青霞路 27" },
                    new OfficeLocation { TeacherId = teachers.Single (i => i.Name == "李老师").Id, Location = "天府楼 304" },
                    new OfficeLocation { TeacherId = teachers.Single (i => i.Name == "胡老师").Id, Location = "天工楼 102" }
                };

                foreach (var o in OfficeLocations)
                    dbcontext.OfficeLocations.Add(o);
                dbcontext.SaveChanges();

                #endregion

                #region 为老师分配课程的种子数据

                var coursetTeachers = new[] {
                    new CourseAssignment { CourseID = courses.Single (c => c.Title == "数学").CourseID, TeacherID = teachers.Single (i => i.Name == "王老师").Id },
                    new CourseAssignment { CourseID = courses.Single (c => c.Title == "数学").CourseID, TeacherID = teachers.Single (i => i.Name == "刘老师").Id },
                    new CourseAssignment { CourseID = courses.Single (c => c.Title == "政治").CourseID, TeacherID = teachers.Single (i => i.Name == "李老师").Id },
                    new CourseAssignment { CourseID = courses.Single (c => c.Title == "化学").CourseID, TeacherID = teachers.Single (i => i.Name == "胡老师").Id },
                    new CourseAssignment { CourseID = courses.Single (c => c.Title == "生物").CourseID, TeacherID = teachers.Single (i => i.Name == "赵老师").Id },
                    new CourseAssignment { CourseID = courses.Single (c => c.Title == "英语").CourseID, TeacherID = teachers.Single (i => i.Name == "李老师").Id },
                    new CourseAssignment { CourseID = courses.Single (c => c.Title == "物理").CourseID, TeacherID = teachers.Single (i => i.Name == "刘老师").Id },
                    new CourseAssignment { CourseID = courses.Single (c => c.Title == "历史").CourseID, TeacherID = teachers.Single (i => i.Name == "王老师").Id }

                };

                foreach (var ci in coursetTeachers)
                    dbcontext.CourseAssignments.Add(ci);
                dbcontext.SaveChanges();

                #endregion


                #region 学生课程关联种子数据

                var studentCourses = new[] {
                new StudentCourse { CourseID=courses.Single(c=>c.Title=="数学").CourseID,StudentID=students.Single(s=>s.Name=="张三").Id,Grade=Models.EnumTypes.Grade.A},
                new StudentCourse { CourseID=courses.Single(c=>c.Title=="英语").CourseID,StudentID=students.Single(s=>s.Name=="李四").Id,Grade=Models.EnumTypes.Grade.B},
                new StudentCourse { CourseID=courses.Single(c=>c.Title=="生物").CourseID,StudentID=students.Single(s=>s.Name=="王五").Id,Grade=Models.EnumTypes.Grade.C},
                
                };
                foreach (var sc in studentCourses) {
                    dbcontext.StudentCourses.Add(sc);
                }
                dbcontext.SaveChanges();
                #endregion

                #region 用户种子数据

                if (dbcontext.Users.Any()) {
                    return builder;
                }
                var user = new ApplicationUser { Email = "ltm@ddxc.org", UserName = "ltm@ddxc.org", EmailConfirmed = true, City = "上海" };
                userManager.CreateAsync(user,"123456").Wait();
                dbcontext.SaveChanges();
                var adminRole = "Admin";
                var role=new IdentityRole { Name=adminRole};
                dbcontext.Roles.Add(role);  
                dbcontext.SaveChanges();

                dbcontext.UserRoles.Add(new IdentityUserRole<string> {
                    RoleId = role.Id,
                    UserId = user.Id,
                });
                dbcontext.SaveChanges();

                #endregion

            }


            return builder;
        }
    }
}
