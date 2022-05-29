using System.Collections.Generic;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class TeacherListViewModel
    {
        public PagedResultDto<Teacher> Teachers { get; set; }
        public List<Course> Courses { get; set; }
        public List<StudentCourse> StudentCourses { get; set; }
        public int SelectedId { get; set; }
        public int SelectedCourseId { get; set; }

    }
}
