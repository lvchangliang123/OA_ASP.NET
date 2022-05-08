using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    //学生与课程的中间表
    public class StudentCourse
    {
        [Key]
        public int StudentsCourseId { get; set; }

        public int CourseID { get; set; }
        public int StudentID { get; set; }
        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}
