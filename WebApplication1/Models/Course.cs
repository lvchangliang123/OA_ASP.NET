using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name ="课程编号")]
        public int CourseID { get; set; }       //主键，但不自增
        [Display(Name ="课程名称")]
        public string Title { get; set; }
        [Display(Name ="课程学分")]
        [Range(0,5)]
        public int Credits { get; set; }

        public int DepartmentID { get; set; }
        public Department Department { get; set; }
        public ICollection<CourseAssignment> CourseAssignments { get; set; }

        //导航属性，一对多
        public ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
