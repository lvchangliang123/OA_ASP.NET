using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DataRepositories;
using WebApplication1.Models.EnumTypes;

namespace WebApplication1.Models
{
    public class Student
    {
        public int Id { get; set; }
        //[Display(Name ="名字")]
        //[Required(ErrorMessage ="请输入名字，名字不能为空"),MaxLength(50,ErrorMessage ="名字长度不能超过50个字符")]
        public string Name { get; set; }
        //[Display(Name = "主修科目")]
        //[Required(ErrorMessage ="请选择一门科目")]
        public MajorEnum? Major { get; set; }
        //[Display(Name = "电子邮箱")]
        //[Required(ErrorMessage = "请输入电子邮箱，电子邮箱不能为空")]
        //[RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",ErrorMessage ="邮箱格式不正确")]
        public string Email { get; set; }
        public string PhotoPath { get; set; }

        [NotMapped]
        [Display(Name="加密ID")]
        public string EncryptedId { get; set; }

        [Display(Name ="入学时间")]
        public DateTime EnrollmentDate { get; set; }

        //导航属性，一对多
        public ICollection<StudentCourse> StudentCourses { get; set; }

    }

  
}
