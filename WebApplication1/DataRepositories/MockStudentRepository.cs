using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.DataRepositories
{
    public class MockStudentRepository : IStudentRepository
    {
        public Student Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Student> GetAllStudents()
        {
            throw new NotImplementedException();
        }

        public Student GetStudentById(int id)
        {
            throw new NotImplementedException();
        }

        public Student Insert(Student student)
        {
            throw new NotImplementedException();
        }

        public Student Update(Student student)
        {
            throw new NotImplementedException();
        }
    }
}
