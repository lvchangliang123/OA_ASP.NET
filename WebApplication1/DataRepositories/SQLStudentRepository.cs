using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Infrastructure;
using WebApplication1.Models;

namespace WebApplication1.DataRepositories
{
    public class SQLStudentRepository : IStudentRepository
    {

        private readonly AppDbContext dbContext;

        public SQLStudentRepository(AppDbContext appDbContext)
        {
            dbContext = appDbContext;
        }

        public Student Delete(int id)
        {
            Student student = dbContext.Students.Find(id);
            if (student!=null) {
                dbContext.Students.Remove(student);
                dbContext.SaveChanges();
            }
            return student;
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return dbContext.Students;
        }

        public Student GetStudentById(int id)
        {
            return dbContext.Students.Find(id);
        }

        public Student Insert(Student student)
        {
            dbContext.Students.Add(student);
            dbContext.SaveChanges();
            return student;
        }

        public Student Update(Student student)
        {
            var updateStudent = dbContext.Students.Attach(student);
            updateStudent.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            dbContext.SaveChanges();
            return student;
        }
    }
}
