﻿using System.ComponentModel.DataAnnotations;
using WebApplication1.Models.EnumTypes;

namespace WebApplication1.Models
{
    //学生与课程的中间表
    public class StudentCourse
    {
        [Key]
        public int StudentsCourseId { get; set; }

        public int CourseID { get; set; }
        public int StudentID { get; set; }
        [DisplayFormat(NullDisplayText ="无成绩")]
        public Grade? Grade { get; set; }
        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}
