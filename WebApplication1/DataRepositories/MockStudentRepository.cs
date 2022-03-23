using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.DataRepositories
{
    public class MockStudentRepository : IStudentRepository
    {
        //private List<Student> students;
        //public MockStudentRepository()
        //{
        //    students = new List<Student>
        //    {
        //        new Student{ Id=1,Name="张三",Major=Models.EnumTypes.MajorEnum.ComputerScience,Email="zhangsan@163.com"},
        //        new Student{ Id=2,Name="李四",Major=Models.EnumTypes.MajorEnum.ElectronicCommerce,Email="lisi@163.com"},
        //        new Student{ Id=3,Name="赵六",Major=Models.EnumTypes.MajorEnum.Mathematics,Email="zhaoliu@163.com"}
        //    };
        //}

        //public Student GetStudent(int id)
        //{
        //    return students.FirstOrDefault(s => s.Id == id);
        //}

        //public void Save(Student student)
        //{
        //    throw new NotImplementedException();
        //}

        //Student IStudentRepository.Add(Student student)
        //{
        //    student.Id = students.Max(s => s.Id) + 1;
        //    students.Add(student);
        //    return student;
        //}

        //IEnumerable<Student> IStudentRepository.GetAllStudents()
        //{
        //    return students;
        //}
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
