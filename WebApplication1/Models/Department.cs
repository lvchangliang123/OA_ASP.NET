using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    /// <summary>
    /// 学院信息
    /// </summary>
    public class Department
    {
        public int DepartmentID { get; set; }
        [StringLength(50,MinimumLength =3)]
        public string Name { get; set; }
        [DataType(DataType.Currency)]
        //[Column(TypeName ="money")]
        public decimal Budget { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        [Display(Name="成立时间")]
        public DateTime StartDate { get; set; }
        public int? TeacherID { get; set; }
        /// <summary>
        /// 学院主任
        /// </summary>
        public Teacher Administrator { get; set; }
        public ICollection<Course> Courses { get; set; }

    }
}
