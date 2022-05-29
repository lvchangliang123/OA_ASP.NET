namespace WebApplication1.Models
{
    public class CourseAssignment
    {
        //两个ID，实现联合主键,使用fluent API实现
        public int TeacherID { get; set; }
        public int CourseID { get; set; }
        public Teacher Teacher { get; set; }
        public Course Course { get; set; }

    }
}
