using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CourseID { get; set; }       //主键，但不自增
        public string Title { get; set; }
        public int Credits { get; set; }
        //导航属性，一对多
        public ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
